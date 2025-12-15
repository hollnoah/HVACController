'NOAH HOLLOWAY
'RCET 3371
'FALL 2025
'HVAC CONTROLLER

Imports System.IO.Ports
Imports System.IO
Imports System.Drawing

'MUST PRESS QY@ BUTTONS 2 & 3 SIMULTANEOUSLY FOR COOLING, AND 3 & 5 SIMULTANEOUSLY FOR HEATING
Public Class HVACController

    ' ---------------- HVAC Values ----------------
    Private currentTemp As Double = 70
    Private unitTemp As Double = 70

    Private highSetpoint As Double = 75
    Private lowSetpoint As Double = 68
    Private hysteresis As Double = 2

    ' ---------------- Digital Inputs ----------------
    Private safetyOK As Boolean
    Private heatEnable As Boolean
    Private fanOnly As Boolean
    Private coolEnable As Boolean
    Private pressureOK As Boolean = True

    ' ---------------- Outputs (actual driven outputs) ----------------
    Private fanOn As Boolean
    Private heatOn As Boolean
    Private coolOn As Boolean
    Private warningOn As Boolean

    Private fanDelayComplete As Boolean = False

    ' ---------------- Communication / state ----------------
    Private connected As Boolean = False
    Private lastPort As String = ""
    Private currentMode As String = "Off"
    Private fanMode As String = "Auto"
    Private faultMessage As String = "None"

    ' ---------------- Timers not created in designer ----------------
    Private WithEvents ClockTimer As New Timer()
    Private WithEvents ReconnectTimer As New Timer()
    Private WithEvents VerificationTimer As New Timer()
    Private WithEvents PressureCheckTimer As New Timer()

    ' filenames
    Private ReadOnly CONFIG_FILE As String = "HVAC Settings.txt"   ' persistent settings (port + setpoints)
    ' Daily error log
    Private ReadOnly DAILY_LOG_PREFIX As String = "HVACSystem-"   ' final log file: HVACSystem-YYmmDD.log

    ' ---------------- Form Load ----------------
    Private Sub HVACController_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' Populate serial ports in combobox
            PopulatePorts()

            ' Configure designer timers 
            CommandTimer.Interval = 100
            ReadTimer.Interval = 20
            FanDelayTimer.Interval = 5000

            ' Configure additional timers
            ClockTimer.Interval = 1000
            ReconnectTimer.Interval = 10000
            VerificationTimer.Interval = 30000
            PressureCheckTimer.Interval = 120000

            ' Start clock and reconnect attempts
            ClockTimer.Start()
            ReconnectTimer.Start()

            ' Load saved settings (port & setpoints)
            LoadSettings()

            ' Try to auto-connect to QY@ board
            AutoConnect()
        Catch ex As Exception
            ' Log and show the exception so startup doesn't silently fail
            Try
                LogError("Startup exception: " & ex.ToString())
            Catch
                ' ignore logging failures
            End Try
            MessageBox.Show("An error occurred during startup:" & vbCrLf & ex.ToString(), "Startup Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ---------------- Populate COM Ports ----------------
    Private Sub PopulatePorts()
        PortComboBox.Items.Clear()
        PortComboBox.Items.AddRange(SerialPort.GetPortNames())
        If PortComboBox.Items.Count > 0 Then PortComboBox.SelectedIndex = 0
        If lastPort <> "" AndAlso PortComboBox.Items.Contains(lastPort) Then
            PortComboBox.SelectedItem = lastPort
        End If
    End Sub

    ' ---------------- Connect ----------------
    Private Sub ConnectButton_Click(sender As Object, e As EventArgs) Handles ConnectButton.Click
        Dim portToUse As String = Nothing

        If PortComboBox.SelectedItem IsNot Nothing Then
            portToUse = PortComboBox.SelectedItem.ToString()
        ElseIf lastPort <> "" Then
            portToUse = lastPort
        End If

        If String.IsNullOrEmpty(portToUse) Then
            MessageBox.Show("Select a COM port")
            Return
        End If

        Try
            If PICSerialPort.IsOpen Then PICSerialPort.Close()

            PICSerialPort.PortName = portToUse
            PICSerialPort.BaudRate = 9600
            PICSerialPort.DataBits = 8
            PICSerialPort.Parity = Parity.None
            PICSerialPort.StopBits = StopBits.One
            PICSerialPort.Open()

            MessageBox.Show($"Connected to {PICSerialPort.PortName}")

            lastPort = PICSerialPort.PortName
            connected = True
            UpdateCommStatus()

            CommandTimer.Start()
            ReadTimer.Start()

        Catch ex As Exception
            MessageBox.Show($"Connection failed: {ex.Message}")
            connected = False
            UpdateCommStatus()
        End Try
    End Sub

    ' ---------------- AutoConnect ----------------
    Private Sub AutoConnect()
        Dim ports As String() = SerialPort.GetPortNames()

        ' Try last known port first
        If lastPort <> "" AndAlso ports.Contains(lastPort) Then
            If TryVerifyPort(lastPort) Then Return
        End If

        ' Try others
        For Each p As String In ports
            If p = lastPort Then Continue For
            If TryVerifyPort(p) Then
                lastPort = p
                Return
            End If
        Next

        ' If not found
        connected = False
        UpdateCommStatus()
    End Sub

    Private Function TryVerifyPort(portName As String) As Boolean
        Try
            If PICSerialPort.IsOpen Then PICSerialPort.Close()
            PICSerialPort.PortName = portName
            PICSerialPort.BaudRate = 9600
            PICSerialPort.DataBits = 8
            PICSerialPort.Parity = Parity.None
            PICSerialPort.StopBits = StopBits.One
            PICSerialPort.Open()

            ' Request analog packet
            PICSerialPort.Write(New Byte() {&H53}, 0, 1)
            Threading.Thread.Sleep(120)

            If PICSerialPort.BytesToRead >= 4 Then
                Dim tmp(3) As Byte
                PICSerialPort.Read(tmp, 0, 4)
                connected = True
                UpdateCommStatus()
                CommandTimer.Start()
                ReadTimer.Start()
                Return True
            End If

            PICSerialPort.Close()
        Catch
            ' ignore and return false
        End Try
        Return False
    End Function

    ' ---------------- Command Timer ----------------
    Private Sub CommandTimer_Tick(sender As Object, e As EventArgs) Handles CommandTimer.Tick
        If Not PICSerialPort.IsOpen Then
            connected = False
            UpdateCommStatus()
            Return
        End If

        ' Request analog (A1 + A2)
        PICSerialPort.Write(New Byte() {&H53}, 0, 1)

        ' Request digital inputs
        PICSerialPort.Write(New Byte() {&H30}, 0, 1)
    End Sub

    ' ---------------- Read Timer ----------------
    Private Sub ReadTimer_Tick(sender As Object, e As EventArgs) Handles ReadTimer.Tick
        If Not PICSerialPort.IsOpen Then Exit Sub

        ' Read analog packet (4 bytes)
        If PICSerialPort.BytesToRead >= 4 Then
            Dim a(3) As Byte
            PICSerialPort.Read(a, 0, 4)
            ProcessAnalogPacket(a)
        End If

        ' Read digital packet (1 byte)
        If PICSerialPort.BytesToRead >= 1 Then
            Dim d As Integer = PICSerialPort.ReadByte()
            ProcessDigitalInputs(CByte(d))
        End If

        DecideHVAC()
        SendOutputs()
        UpdateGUI()
    End Sub

    ' ---------------- Analog Processing ----------------
    Private Sub ProcessAnalogPacket(b() As Byte)
        Dim rawRoom As Integer = (CInt(b(0)) << 2) Or (b(1) >> 6)
        Dim rawUnit As Integer = (CInt(b(2)) << 2) Or (b(3) >> 6)

        currentTemp = 40 + (rawRoom / 1023.0) * 60
        unitTemp = 40 + (rawUnit / 1023.0) * 60
    End Sub

    ' ---------------- Digital Inputs ----------------
    Private Sub ProcessDigitalInputs(d As Byte)
        ' decode raw input bits
        Dim tempSafety As Boolean = (d And &H1) <> 0
        Dim tempHeatSwitch As Boolean = (d And &H2) <> 0
        Dim tempFanOnly As Boolean = (d And &H4) <> 0
        Dim tempPressure As Boolean = (d And &H8) <> 0
        Dim tempCoolSwitch As Boolean = (d And &H10) <> 0

        ' update non-switch inputs directly
        safetyOK = tempSafety
        pressureOK = tempPressure
        fanOnly = tempFanOnly

        ' Map switches directly to heatEnable/coolEnable so each switch independently enables/disables that mode.
        heatEnable = tempHeatSwitch
        coolEnable = tempCoolSwitch
    End Sub

    ' ---------------- HVAC Decision Logic ----------------
    Private Sub DecideHVAC()
        ' Safety interlock
        If Not safetyOK Then
            fanOn = False
            heatOn = False
            coolOn = False
            warningOn = True
            fanDelayComplete = False
            faultMessage = "Safety interlock LOW - Disabled"
            LogError(faultMessage)
            UpdateFault()
            Return
        Else
            warningOn = False
            faultMessage = "None"
        End If

        ' Fan-only mode
        If fanOnly Then
            fanOn = True
            heatOn = False
            coolOn = False
            fanDelayComplete = False
            currentMode = "Fan Only"
            fanMode = "On"
            If PressureCheckTimer.Enabled = False Then PressureCheckTimer.Start()
            Return
        End If

        If heatEnable AndAlso coolEnable Then
            ' invalid; both requested
            faultMessage = "Invalid: both heat and cool requested"
            LogError(faultMessage)
            UpdateFault()
            currentMode = "Invalid"
            heatOn = False
            coolOn = False
            fanOn = False
            Return
        ElseIf heatEnable Then
            currentMode = "Heating"
        ElseIf coolEnable Then
            currentMode = "Cooling"
        Else
            currentMode = "Off"
        End If

        fanMode = If(fanOnly, "On", "Auto")

        ' Heating
        If currentMode = "Heating" Then
            If currentTemp < lowSetpoint - hysteresis / 2 Then
                fanOn = True
                If Not FanDelayTimer.Enabled AndAlso Not fanDelayComplete Then
                    FanDelayTimer.Start()
                End If
                If fanDelayComplete Then
                    heatOn = True
                    ' start verification
                    If Not VerificationTimer.Enabled Then VerificationTimer.Start()
                End If
            ElseIf currentTemp > lowSetpoint + hysteresis / 2 Then
                heatOn = False
                fanDelayComplete = False
                If Not fanOnly Then FanDelayTimer.Start()
            End If
        End If

        ' Cooling
        If currentMode = "Cooling" Then
            If currentTemp > highSetpoint + hysteresis / 2 Then
                fanOn = True
                If Not FanDelayTimer.Enabled AndAlso Not fanDelayComplete Then
                    FanDelayTimer.Start()
                End If
                If fanDelayComplete Then
                    coolOn = True
                    If Not VerificationTimer.Enabled Then VerificationTimer.Start()
                End If
            ElseIf currentTemp < highSetpoint - hysteresis / 2 Then
                coolOn = False
                fanDelayComplete = False
                If Not fanOnly Then FanDelayTimer.Start()
            End If
        End If

        If fanOn AndAlso Not PressureCheckTimer.Enabled Then
            PressureCheckTimer.Start()
        End If

        ' If nothing active, ensure outputs off
        If Not heatOn AndAlso Not coolOn AndAlso Not fanOnly Then
            fanOn = False
        End If
    End Sub

    ' ---------------- Fan Delay ----------------
    Private Sub FanDelayTimer_Tick(sender As Object, e As EventArgs) Handles FanDelayTimer.Tick
        FanDelayTimer.Stop()
        fanDelayComplete = True
    End Sub

    ' ---------------- Verification Timer (30s) ----------------
    Private Sub VerificationTimer_Tick(sender As Object, e As EventArgs) Handles VerificationTimer.Tick
        If heatOn Then
            ' unit should be hotter than room when heating
            If unitTemp <= currentTemp Then
                faultMessage = "System temperature verification failed - Maintenance required"
                LogError(faultMessage)
                heatOn = False
                fanOn = False
                UpdateFault()
            End If
        ElseIf coolOn Then
            If unitTemp >= currentTemp Then
                faultMessage = "System temperature verification failed - Maintenance required"
                LogError(faultMessage)
                coolOn = False
                fanOn = False
                UpdateFault()
            End If
        Else
            VerificationTimer.Stop()
        End If
    End Sub

    ' ---------------- Pressure Check Timer (2min) ----------------
    Private Sub PressureCheckTimer_Tick(sender As Object, e As EventArgs) Handles PressureCheckTimer.Tick
        If fanOn AndAlso Not pressureOK Then
            faultMessage = "Differential pressure sensor LOW - Heating/Cooling disabled"
            LogError(faultMessage)
            heatOn = False
            coolOn = False
            fanOn = False
            UpdateFault()
        End If

        If Not fanOn Then PressureCheckTimer.Stop()
    End Sub

    ' ---------------- Reconnect Timer ----------------
    Private Sub ReconnectTimer_Tick(sender As Object, e As EventArgs) Handles ReconnectTimer.Tick
        If Not connected OrElse Not PICSerialPort.IsOpen Then
            AutoConnect()
            If Not connected Then
                LogError("Communication lost with QY@ board - Attempting reconnect")
            End If
        End If
    End Sub

    ' ---------------- Send Outputs ----------------
    Private Sub SendOutputs()
        If Not PICSerialPort.IsOpen Then Return

        Dim outputByte As Byte = 0

        If warningOn Then outputByte = outputByte Or &H1
        If heatOn Then outputByte = outputByte Or &H2
        If fanOn Then outputByte = outputByte Or &H8
        If coolOn Then outputByte = outputByte Or &H10

        Try
            PICSerialPort.Write(New Byte() {&H20, outputByte}, 0, 2)
        Catch ex As Exception
            connected = False
            UpdateCommStatus()
            LogError($"Failed to send outputs: {ex.Message}")
        End Try
    End Sub

    ' ---------------- GUI Update ----------------
    Private Sub UpdateGUI()
        ' Time label updated by ClockTimer
        lblCurrentTemp.Text = $"{currentTemp:F1} °F (Unit: {unitTemp:F1} °F)"
        lblHighSetpoint.Text = $"Cool Setpoint: {highSetpoint:F1} °F"
        lblLowSetpoint.Text = $"Heat Setpoint: {lowSetpoint:F1} °F"

        If currentMode = "Heating" Then
            lblCurrentSetpoint.Text = $"Current Setpoint: {lowSetpoint:F1} °F"
        ElseIf currentMode = "Cooling" Then
            lblCurrentSetpoint.Text = $"Current Setpoint: {highSetpoint:F1} °F"
        Else
            lblCurrentSetpoint.Text = "Current Setpoint: N/A"
        End If

        lblMode.Text = $"Mode: {currentMode}"
        lblFanMode.Text = $"Fan Mode: {fanMode}"
        lblFanStatus.Text = $"Fan: {If(fanOn, "On", "Off")}"
        UpdateFault()
        UpdateCommStatus()
    End Sub

    ' ---------------- Clock Timer ----------------
    Private Sub ClockTimer_Tick(sender As Object, e As EventArgs) Handles ClockTimer.Tick
        lblTime.Text = DateTime.Now.ToString("hh:mm tt")
    End Sub

    ' ---------------- Update Fault & Comm ----------------
    Private Sub UpdateFault()
        lblFault.Text = $"Fault: {faultMessage}"
        lblFault.ForeColor = If(faultMessage = "None", Color.Green, Color.Red)
    End Sub

    Private Sub UpdateCommStatus()
        lblCommStatus.Text = $"Comm: {If(connected AndAlso PICSerialPort.IsOpen, "Connected", "Disconnected")}"
        lblCommStatus.ForeColor = If(connected AndAlso PICSerialPort.IsOpen, Color.Green, Color.Red)
    End Sub

    ' ---------------- Setpoint Buttons ----------------
    Private Sub btnIncreaseHigh_Click(sender As Object, e As EventArgs) Handles btnIncreaseHigh.Click
        highSetpoint = Math.Min(90.0, highSetpoint + 0.5)
        UpdateGUI()
    End Sub

    Private Sub btnDecreaseHigh_Click(sender As Object, e As EventArgs) Handles btnDecreaseHigh.Click
        highSetpoint = Math.Max(50.0, highSetpoint - 0.5)
        UpdateGUI()
    End Sub

    Private Sub btnIncreaseLow_Click(sender As Object, e As EventArgs) Handles btnIncreaseLow.Click
        lowSetpoint = Math.Min(90.0, lowSetpoint + 0.5)
        UpdateGUI()
    End Sub

    Private Sub btnDecreaseLow_Click(sender As Object, e As EventArgs) Handles btnDecreaseLow.Click
        lowSetpoint = Math.Max(50.0, lowSetpoint - 0.5)
        UpdateGUI()
    End Sub

    ' ---------------- Toolstrip Menu Handlers ----------------
    Private Sub CommunicationSetupMenuItem_Click(sender As Object, e As EventArgs) Handles CommunicationSetupMenuItem.Click
        Dim port As String = InputBox("Enter COM port:", "Communication Setup", lastPort)
        If port <> "" Then
            Try
                If PICSerialPort.IsOpen Then PICSerialPort.Close()
                PICSerialPort.PortName = port
                PICSerialPort.BaudRate = 9600
                PICSerialPort.DataBits = 8
                PICSerialPort.Parity = Parity.None
                PICSerialPort.StopBits = StopBits.One
                PICSerialPort.Open()
                connected = True
                lastPort = port
                CommandTimer.Start()
                ReadTimer.Start()
                UpdateCommStatus()
            Catch ex As Exception
                MessageBox.Show($"Failed to connect: {ex.Message}")
                connected = False
                UpdateCommStatus()
            End Try
        End If
    End Sub

    Private Sub SaveSettingsMenuItem_Click(sender As Object, e As EventArgs) Handles SaveSettingsMenuItem.Click
        SaveSettings()
    End Sub

    Private Sub ExitMenuItem_Click(sender As Object, e As EventArgs) Handles ExitMenuItem.Click
        Me.Close()
    End Sub

    ' ---------------- Load / Save Settings ----------------
    Private Sub LoadSettings()
        If File.Exists(CONFIG_FILE) Then
            Try
                Dim lines() As String = File.ReadAllLines(CONFIG_FILE)
                For Each line As String In lines
                    If line.StartsWith("port=") Then lastPort = line.Substring(5)
                    If line.StartsWith("high=") Then Double.TryParse(line.Substring(5), highSetpoint)
                    If line.StartsWith("low=") Then Double.TryParse(line.Substring(4), lowSetpoint)
                Next
                ' Re-populate ports so saved port is selected
                PopulatePorts()
            Catch
                ' ignore malformed config
            End Try
            UpdateGUI()
        End If
    End Sub

    Private Sub SaveSettings()
        ' Ensure lastPort reflects the current user selection if present
        If PortComboBox.SelectedItem IsNot Nothing Then
            lastPort = PortComboBox.SelectedItem.ToString()
        End If

        Try
            Dim timestamp As String = DateTime.Now.ToString("yyMMdd-HHmmss")
            Using writer As New StreamWriter(CONFIG_FILE, False)
                writer.WriteLine($"{timestamp}: Saved settings")
                writer.WriteLine($"port={lastPort}")
                writer.WriteLine($"high={highSetpoint}")
                writer.WriteLine($"low={lowSetpoint}")
            End Using
            MessageBox.Show("Settings saved", "Save Settings", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show($"Failed to save settings: {ex.Message}", "Save Settings", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ---------------- Logging ----------------
    Private Sub LogError(desc As String)
        ' timestamp format: yyMMdd-HHmmss  
        Dim timestamp As String = DateTime.Now.ToString("yyMMdd-HHmmss")
        Dim logLine As String = $"{timestamp}: {desc}"

        ' Append to daily log file (per assignment)
        Try
            Dim logDate As String = DateTime.Now.ToString("yyMMdd")
            Dim logFile As String = $"{DAILY_LOG_PREFIX}{logDate}.log"
            Using writer As New StreamWriter(logFile, True)
                writer.WriteLine(logLine)
            End Using
        Catch
            ' ignore logging failures
        End Try
    End Sub

    ' ---------------- Form Closing ----------------
    Private Sub HVACController_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            If PICSerialPort.IsOpen Then PICSerialPort.Close()
        Catch
        End Try

        CommandTimer.Stop()
        ReadTimer.Stop()
        FanDelayTimer.Stop()
        VerificationTimer.Stop()
        PressureCheckTimer.Stop()
        ReconnectTimer.Stop()
        ClockTimer.Stop()
    End Sub

End Class

