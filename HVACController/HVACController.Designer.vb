<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class HVACController
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.CommunicationSetupMenuItem = New System.Windows.Forms.ToolStripButton()
        Me.SaveSettingsMenuItem = New System.Windows.Forms.ToolStripButton()
        Me.ExitMenuItem = New System.Windows.Forms.ToolStripButton()
        Me.lblTime = New System.Windows.Forms.Label()
        Me.lblCurrentTemp = New System.Windows.Forms.Label()
        Me.lblHighSetpoint = New System.Windows.Forms.Label()
        Me.btnIncreaseHigh = New System.Windows.Forms.Button()
        Me.btnDecreaseHigh = New System.Windows.Forms.Button()
        Me.lblLowSetpoint = New System.Windows.Forms.Label()
        Me.btnDecreaseLow = New System.Windows.Forms.Button()
        Me.btnIncreaseLow = New System.Windows.Forms.Button()
        Me.lblCurrentSetpoint = New System.Windows.Forms.Label()
        Me.lblMode = New System.Windows.Forms.Label()
        Me.lblFanMode = New System.Windows.Forms.Label()
        Me.lblFanStatus = New System.Windows.Forms.Label()
        Me.lblFault = New System.Windows.Forms.Label()
        Me.lblCommStatus = New System.Windows.Forms.Label()
        Me.ConnectButton = New System.Windows.Forms.Button()
        Me.PortComboBox = New System.Windows.Forms.ComboBox()
        Me.PICSerialPort = New System.IO.Ports.SerialPort(Me.components)
        Me.FanDelayTimer = New System.Windows.Forms.Timer(Me.components)
        Me.CommandTimer = New System.Windows.Forms.Timer(Me.components)
        Me.ReadTimer = New System.Windows.Forms.Timer(Me.components)
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.BackColor = System.Drawing.Color.DarkGray
        Me.ToolStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CommunicationSetupMenuItem, Me.SaveSettingsMenuItem, Me.ExitMenuItem})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(1109, 27)
        Me.ToolStrip1.TabIndex = 0
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'CommunicationSetupMenuItem
        '
        Me.CommunicationSetupMenuItem.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.CommunicationSetupMenuItem.MergeIndex = 0
        Me.CommunicationSetupMenuItem.Name = "CommunicationSetupMenuItem"
        Me.CommunicationSetupMenuItem.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never
        Me.CommunicationSetupMenuItem.Size = New System.Drawing.Size(160, 24)
        Me.CommunicationSetupMenuItem.Text = "Communication Setup"
        '
        'SaveSettingsMenuItem
        '
        Me.SaveSettingsMenuItem.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.SaveSettingsMenuItem.MergeIndex = 1
        Me.SaveSettingsMenuItem.Name = "SaveSettingsMenuItem"
        Me.SaveSettingsMenuItem.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never
        Me.SaveSettingsMenuItem.Size = New System.Drawing.Size(101, 24)
        Me.SaveSettingsMenuItem.Text = "Save Settings"
        '
        'ExitMenuItem
        '
        Me.ExitMenuItem.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ExitMenuItem.MergeIndex = 2
        Me.ExitMenuItem.Name = "ExitMenuItem"
        Me.ExitMenuItem.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never
        Me.ExitMenuItem.Size = New System.Drawing.Size(37, 24)
        Me.ExitMenuItem.Text = "Exit"
        '
        'lblTime
        '
        Me.lblTime.AutoSize = True
        Me.lblTime.BackColor = System.Drawing.Color.Transparent
        Me.lblTime.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTime.Location = New System.Drawing.Point(503, 69)
        Me.lblTime.Name = "lblTime"
        Me.lblTime.Size = New System.Drawing.Size(48, 41)
        Me.lblTime.TabIndex = 1
        Me.lblTime.Text = """"""
        '
        'lblCurrentTemp
        '
        Me.lblCurrentTemp.AutoSize = True
        Me.lblCurrentTemp.BackColor = System.Drawing.Color.Transparent
        Me.lblCurrentTemp.Font = New System.Drawing.Font("Segoe UI", 72.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrentTemp.Location = New System.Drawing.Point(72, 249)
        Me.lblCurrentTemp.Name = "lblCurrentTemp"
        Me.lblCurrentTemp.Size = New System.Drawing.Size(358, 159)
        Me.lblCurrentTemp.TabIndex = 2
        Me.lblCurrentTemp.Text = "--.--F"
        '
        'lblHighSetpoint
        '
        Me.lblHighSetpoint.AutoSize = True
        Me.lblHighSetpoint.BackColor = System.Drawing.Color.Transparent
        Me.lblHighSetpoint.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblHighSetpoint.Location = New System.Drawing.Point(318, 501)
        Me.lblHighSetpoint.Name = "lblHighSetpoint"
        Me.lblHighSetpoint.Size = New System.Drawing.Size(198, 28)
        Me.lblHighSetpoint.TabIndex = 3
        Me.lblHighSetpoint.Text = "Cool Setpoint: --.--F"
        '
        'btnIncreaseHigh
        '
        Me.btnIncreaseHigh.Location = New System.Drawing.Point(412, 551)
        Me.btnIncreaseHigh.Name = "btnIncreaseHigh"
        Me.btnIncreaseHigh.Size = New System.Drawing.Size(57, 37)
        Me.btnIncreaseHigh.TabIndex = 4
        Me.btnIncreaseHigh.Text = "+"
        Me.btnIncreaseHigh.UseVisualStyleBackColor = True
        '
        'btnDecreaseHigh
        '
        Me.btnDecreaseHigh.Location = New System.Drawing.Point(349, 551)
        Me.btnDecreaseHigh.Name = "btnDecreaseHigh"
        Me.btnDecreaseHigh.Size = New System.Drawing.Size(57, 37)
        Me.btnDecreaseHigh.TabIndex = 5
        Me.btnDecreaseHigh.Text = "-"
        Me.btnDecreaseHigh.UseVisualStyleBackColor = True
        '
        'lblLowSetpoint
        '
        Me.lblLowSetpoint.AutoSize = True
        Me.lblLowSetpoint.BackColor = System.Drawing.Color.Transparent
        Me.lblLowSetpoint.Location = New System.Drawing.Point(599, 501)
        Me.lblLowSetpoint.Name = "lblLowSetpoint"
        Me.lblLowSetpoint.Size = New System.Drawing.Size(187, 28)
        Me.lblLowSetpoint.TabIndex = 6
        Me.lblLowSetpoint.Text = "Heat Setpoint:--.-F"
        '
        'btnDecreaseLow
        '
        Me.btnDecreaseLow.Location = New System.Drawing.Point(632, 551)
        Me.btnDecreaseLow.Name = "btnDecreaseLow"
        Me.btnDecreaseLow.Size = New System.Drawing.Size(57, 37)
        Me.btnDecreaseLow.TabIndex = 7
        Me.btnDecreaseLow.Text = "-"
        Me.btnDecreaseLow.UseVisualStyleBackColor = True
        '
        'btnIncreaseLow
        '
        Me.btnIncreaseLow.Location = New System.Drawing.Point(695, 551)
        Me.btnIncreaseLow.Name = "btnIncreaseLow"
        Me.btnIncreaseLow.Size = New System.Drawing.Size(57, 37)
        Me.btnIncreaseLow.TabIndex = 8
        Me.btnIncreaseLow.Text = "+"
        Me.btnIncreaseLow.UseVisualStyleBackColor = True
        '
        'lblCurrentSetpoint
        '
        Me.lblCurrentSetpoint.AutoSize = True
        Me.lblCurrentSetpoint.BackColor = System.Drawing.Color.Transparent
        Me.lblCurrentSetpoint.Location = New System.Drawing.Point(455, 457)
        Me.lblCurrentSetpoint.Name = "lblCurrentSetpoint"
        Me.lblCurrentSetpoint.Size = New System.Drawing.Size(219, 28)
        Me.lblCurrentSetpoint.TabIndex = 9
        Me.lblCurrentSetpoint.Text = "Current Setpoint: --.-F"
        '
        'lblMode
        '
        Me.lblMode.AutoSize = True
        Me.lblMode.BackColor = System.Drawing.Color.Transparent
        Me.lblMode.Location = New System.Drawing.Point(644, 616)
        Me.lblMode.Name = "lblMode"
        Me.lblMode.Size = New System.Drawing.Size(108, 28)
        Me.lblMode.TabIndex = 10
        Me.lblMode.Text = "Mode: Off"
        '
        'lblFanMode
        '
        Me.lblFanMode.AutoSize = True
        Me.lblFanMode.BackColor = System.Drawing.Color.Transparent
        Me.lblFanMode.Location = New System.Drawing.Point(344, 656)
        Me.lblFanMode.Name = "lblFanMode"
        Me.lblFanMode.Size = New System.Drawing.Size(161, 28)
        Me.lblFanMode.TabIndex = 11
        Me.lblFanMode.Text = "Fan Mode: Auto"
        '
        'lblFanStatus
        '
        Me.lblFanStatus.AutoSize = True
        Me.lblFanStatus.BackColor = System.Drawing.Color.Transparent
        Me.lblFanStatus.Location = New System.Drawing.Point(344, 616)
        Me.lblFanStatus.Name = "lblFanStatus"
        Me.lblFanStatus.Size = New System.Drawing.Size(86, 28)
        Me.lblFanStatus.TabIndex = 12
        Me.lblFanStatus.Text = "Fan: Off"
        '
        'lblFault
        '
        Me.lblFault.AutoSize = True
        Me.lblFault.BackColor = System.Drawing.Color.Transparent
        Me.lblFault.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblFault.Location = New System.Drawing.Point(761, 650)
        Me.lblFault.Name = "lblFault"
        Me.lblFault.Size = New System.Drawing.Size(120, 28)
        Me.lblFault.TabIndex = 13
        Me.lblFault.Text = "Fault: None"
        '
        'lblCommStatus
        '
        Me.lblCommStatus.AutoSize = True
        Me.lblCommStatus.BackColor = System.Drawing.Color.Transparent
        Me.lblCommStatus.ForeColor = System.Drawing.Color.Red
        Me.lblCommStatus.Location = New System.Drawing.Point(761, 690)
        Me.lblCommStatus.Name = "lblCommStatus"
        Me.lblCommStatus.Size = New System.Drawing.Size(211, 28)
        Me.lblCommStatus.TabIndex = 14
        Me.lblCommStatus.Text = "Comm: Disconnected"
        '
        'ConnectButton
        '
        Me.ConnectButton.Location = New System.Drawing.Point(978, 674)
        Me.ConnectButton.Name = "ConnectButton"
        Me.ConnectButton.Size = New System.Drawing.Size(119, 60)
        Me.ConnectButton.TabIndex = 15
        Me.ConnectButton.Text = "Connect"
        Me.ConnectButton.UseVisualStyleBackColor = True
        '
        'PortComboBox
        '
        Me.PortComboBox.FormattingEnabled = True
        Me.PortComboBox.Location = New System.Drawing.Point(976, 632)
        Me.PortComboBox.Name = "PortComboBox"
        Me.PortComboBox.Size = New System.Drawing.Size(121, 36)
        Me.PortComboBox.TabIndex = 16
        '
        'FanDelayTimer
        '
        Me.FanDelayTimer.Interval = 5000
        '
        'CommandTimer
        '
        Me.CommandTimer.Interval = 500
        '
        'ReadTimer
        '
        '
        'HVACController
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(12.0!, 28.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightGray
        Me.BackgroundImage = Global.HVACController.My.Resources.Resources.Bengal_ReversedOrange
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.ClientSize = New System.Drawing.Size(1109, 746)
        Me.Controls.Add(Me.PortComboBox)
        Me.Controls.Add(Me.ConnectButton)
        Me.Controls.Add(Me.lblCommStatus)
        Me.Controls.Add(Me.lblFault)
        Me.Controls.Add(Me.lblFanStatus)
        Me.Controls.Add(Me.lblFanMode)
        Me.Controls.Add(Me.lblMode)
        Me.Controls.Add(Me.lblCurrentSetpoint)
        Me.Controls.Add(Me.btnIncreaseLow)
        Me.Controls.Add(Me.btnDecreaseLow)
        Me.Controls.Add(Me.lblLowSetpoint)
        Me.Controls.Add(Me.btnDecreaseHigh)
        Me.Controls.Add(Me.btnIncreaseHigh)
        Me.Controls.Add(Me.lblHighSetpoint)
        Me.Controls.Add(Me.lblCurrentTemp)
        Me.Controls.Add(Me.lblTime)
        Me.Controls.Add(Me.ToolStrip1)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "HVACController"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "HVAC Controller"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents lblTime As Label
    Friend WithEvents lblCurrentTemp As Label
    Friend WithEvents lblHighSetpoint As Label
    Friend WithEvents btnIncreaseHigh As Button
    Friend WithEvents btnDecreaseHigh As Button
    Friend WithEvents lblLowSetpoint As Label
    Friend WithEvents btnDecreaseLow As Button
    Friend WithEvents btnIncreaseLow As Button
    Friend WithEvents lblCurrentSetpoint As Label
    Friend WithEvents lblMode As Label
    Friend WithEvents lblFanMode As Label
    Friend WithEvents lblFanStatus As Label
    Friend WithEvents lblFault As Label
    Friend WithEvents lblCommStatus As Label
    Friend WithEvents CommunicationSetupMenuItem As ToolStripButton
    Friend WithEvents SaveSettingsMenuItem As ToolStripButton
    Friend WithEvents ExitMenuItem As ToolStripButton
    Friend WithEvents ConnectButton As Button
    Friend WithEvents PortComboBox As ComboBox
    Friend WithEvents PICSerialPort As IO.Ports.SerialPort
    Friend WithEvents FanDelayTimer As Timer
    Friend WithEvents CommandTimer As Timer
    Friend WithEvents ReadTimer As Timer
End Class
