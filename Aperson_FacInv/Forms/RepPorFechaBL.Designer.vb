<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RepPorFechaBL
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer


    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.txtBL = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.dHoraFin = New System.Windows.Forms.TextBox()
        Me.dHoraIni = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtNomConsigna = New System.Windows.Forms.TextBox()
        Me.txtConsigna = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.txtBodega = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtViaje = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtNomProducto = New System.Windows.Forms.TextBox()
        Me.txtProducto = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtNomBuque = New System.Windows.Forms.TextBox()
        Me.txtBuque = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.dFechaFin = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.dFechaIni = New System.Windows.Forms.DateTimePicker()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.btnGrabar = New System.Windows.Forms.Button()
        Me.btnSalir = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.CheckBLInfo = New System.Windows.Forms.CheckBox()
        Me.CheckConsigna = New System.Windows.Forms.CheckBox()
        Me.ChekBuque = New System.Windows.Forms.CheckBox()
        Me.ChekBl = New System.Windows.Forms.CheckBox()
        Me.ChekProductoResumen = New System.Windows.Forms.CheckBox()
        Me.ChekProducto = New System.Windows.Forms.CheckBox()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.SeaGreen
        Me.Label4.Location = New System.Drawing.Point(12, 6)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(264, 24)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Reporte BL y Consignatario"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.txtBL)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.dHoraFin)
        Me.GroupBox2.Controls.Add(Me.dHoraIni)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.txtNomConsigna)
        Me.GroupBox2.Controls.Add(Me.txtConsigna)
        Me.GroupBox2.Controls.Add(Me.Label15)
        Me.GroupBox2.Controls.Add(Me.txtBodega)
        Me.GroupBox2.Controls.Add(Me.Label11)
        Me.GroupBox2.Controls.Add(Me.txtViaje)
        Me.GroupBox2.Controls.Add(Me.Label10)
        Me.GroupBox2.Controls.Add(Me.txtNomProducto)
        Me.GroupBox2.Controls.Add(Me.txtProducto)
        Me.GroupBox2.Controls.Add(Me.Label12)
        Me.GroupBox2.Controls.Add(Me.txtNomBuque)
        Me.GroupBox2.Controls.Add(Me.txtBuque)
        Me.GroupBox2.Controls.Add(Me.Label9)
        Me.GroupBox2.Controls.Add(Me.dFechaFin)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.dFechaIni)
        Me.GroupBox2.Controls.Add(Me.Label31)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 41)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(689, 219)
        Me.GroupBox2.TabIndex = 0
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Rangos de Impresión"
        '
        'txtBL
        '
        Me.txtBL.Location = New System.Drawing.Point(155, 183)
        Me.txtBL.Name = "txtBL"
        Me.txtBL.Size = New System.Drawing.Size(94, 20)
        Me.txtBL.TabIndex = 76
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(112, 187)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(23, 13)
        Me.Label5.TabIndex = 77
        Me.Label5.Text = "BL:"
        '
        'dHoraFin
        '
        Me.dHoraFin.Location = New System.Drawing.Point(484, 54)
        Me.dHoraFin.Name = "dHoraFin"
        Me.dHoraFin.Size = New System.Drawing.Size(92, 20)
        Me.dHoraFin.TabIndex = 4
        '
        'dHoraIni
        '
        Me.dHoraIni.Location = New System.Drawing.Point(155, 55)
        Me.dHoraIni.Name = "dHoraIni"
        Me.dHoraIni.Size = New System.Drawing.Size(92, 20)
        Me.dHoraIni.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(444, 57)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(33, 13)
        Me.Label2.TabIndex = 75
        Me.Label2.Text = "Hora:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(108, 57)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(33, 13)
        Me.Label3.TabIndex = 74
        Me.Label3.Text = "Hora:"
        '
        'txtNomConsigna
        '
        Me.txtNomConsigna.Enabled = False
        Me.txtNomConsigna.Location = New System.Drawing.Point(210, 157)
        Me.txtNomConsigna.Name = "txtNomConsigna"
        Me.txtNomConsigna.Size = New System.Drawing.Size(367, 20)
        Me.txtNomConsigna.TabIndex = 68
        '
        'txtConsigna
        '
        Me.txtConsigna.Location = New System.Drawing.Point(155, 157)
        Me.txtConsigna.Name = "txtConsigna"
        Me.txtConsigna.Size = New System.Drawing.Size(51, 20)
        Me.txtConsigna.TabIndex = 9
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(67, 161)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(74, 13)
        Me.Label15.TabIndex = 71
        Me.Label15.Text = "Consignatario:"
        '
        'txtBodega
        '
        Me.txtBodega.Location = New System.Drawing.Point(483, 106)
        Me.txtBodega.Name = "txtBodega"
        Me.txtBodega.Size = New System.Drawing.Size(94, 20)
        Me.txtBodega.TabIndex = 7
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(399, 110)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(81, 13)
        Me.Label11.TabIndex = 70
        Me.Label11.Text = "Bodega Buque:"
        '
        'txtViaje
        '
        Me.txtViaje.Location = New System.Drawing.Point(155, 106)
        Me.txtViaje.Name = "txtViaje"
        Me.txtViaje.Size = New System.Drawing.Size(68, 20)
        Me.txtViaje.TabIndex = 6
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(54, 108)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(87, 13)
        Me.Label10.TabIndex = 69
        Me.Label10.Text = "No. Viaje Buque:"
        '
        'txtNomProducto
        '
        Me.txtNomProducto.Enabled = False
        Me.txtNomProducto.Location = New System.Drawing.Point(210, 131)
        Me.txtNomProducto.Name = "txtNomProducto"
        Me.txtNomProducto.Size = New System.Drawing.Size(367, 20)
        Me.txtNomProducto.TabIndex = 61
        '
        'txtProducto
        '
        Me.txtProducto.Location = New System.Drawing.Point(155, 131)
        Me.txtProducto.Name = "txtProducto"
        Me.txtProducto.Size = New System.Drawing.Size(51, 20)
        Me.txtProducto.TabIndex = 8
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(88, 133)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(53, 13)
        Me.Label12.TabIndex = 60
        Me.Label12.Text = "Producto:"
        '
        'txtNomBuque
        '
        Me.txtNomBuque.Enabled = False
        Me.txtNomBuque.Location = New System.Drawing.Point(210, 80)
        Me.txtNomBuque.Name = "txtNomBuque"
        Me.txtNomBuque.Size = New System.Drawing.Size(367, 20)
        Me.txtNomBuque.TabIndex = 58
        '
        'txtBuque
        '
        Me.txtBuque.Location = New System.Drawing.Point(155, 80)
        Me.txtBuque.Name = "txtBuque"
        Me.txtBuque.Size = New System.Drawing.Size(51, 20)
        Me.txtBuque.TabIndex = 5
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(100, 81)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(41, 13)
        Me.Label9.TabIndex = 59
        Me.Label9.Text = "Buque:"
        '
        'dFechaFin
        '
        Me.dFechaFin.Checked = False
        Me.dFechaFin.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dFechaFin.Location = New System.Drawing.Point(484, 28)
        Me.dFechaFin.Name = "dFechaFin"
        Me.dFechaFin.Size = New System.Drawing.Size(92, 20)
        Me.dFechaFin.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(437, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(40, 13)
        Me.Label1.TabIndex = 56
        Me.Label1.Text = "Fecha:"
        '
        'dFechaIni
        '
        Me.dFechaIni.Checked = False
        Me.dFechaIni.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dFechaIni.Location = New System.Drawing.Point(155, 29)
        Me.dFechaIni.Name = "dFechaIni"
        Me.dFechaIni.Size = New System.Drawing.Size(92, 20)
        Me.dFechaIni.TabIndex = 1
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.Location = New System.Drawing.Point(101, 32)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(40, 13)
        Me.Label31.TabIndex = 54
        Me.Label31.Text = "Fecha:"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.btnGrabar)
        Me.GroupBox3.Controls.Add(Me.btnSalir)
        Me.GroupBox3.Location = New System.Drawing.Point(16, 389)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(689, 89)
        Me.GroupBox3.TabIndex = 28
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Botones Utilitarios"
        '
        'btnGrabar
        '
        Me.btnGrabar.AutoSize = True
        Me.btnGrabar.BackColor = System.Drawing.Color.DarkSlateGray
        Me.btnGrabar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGrabar.Font = New System.Drawing.Font("MS Reference Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGrabar.ForeColor = System.Drawing.SystemColors.Window
        Me.btnGrabar.Image = Global.AperBascu.My.Resources.Resources.printer
        Me.btnGrabar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnGrabar.Location = New System.Drawing.Point(226, 19)
        Me.btnGrabar.Name = "btnGrabar"
        Me.btnGrabar.Size = New System.Drawing.Size(119, 56)
        Me.btnGrabar.TabIndex = 1
        Me.btnGrabar.Text = "Imprimir"
        Me.btnGrabar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnGrabar.UseVisualStyleBackColor = False
        '
        'btnSalir
        '
        Me.btnSalir.AutoSize = True
        Me.btnSalir.BackColor = System.Drawing.Color.DarkSlateGray
        Me.btnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSalir.Font = New System.Drawing.Font("MS Reference Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSalir.ForeColor = System.Drawing.SystemColors.Window
        Me.btnSalir.Image = Global.AperBascu.My.Resources.Resources.ClosedDoor
        Me.btnSalir.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSalir.Location = New System.Drawing.Point(351, 19)
        Me.btnSalir.Name = "btnSalir"
        Me.btnSalir.Size = New System.Drawing.Size(119, 56)
        Me.btnSalir.TabIndex = 3
        Me.btnSalir.Text = "Salir"
        Me.btnSalir.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSalir.UseVisualStyleBackColor = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.CheckBLInfo)
        Me.GroupBox1.Controls.Add(Me.CheckConsigna)
        Me.GroupBox1.Controls.Add(Me.ChekBuque)
        Me.GroupBox1.Controls.Add(Me.ChekBl)
        Me.GroupBox1.Controls.Add(Me.ChekProductoResumen)
        Me.GroupBox1.Controls.Add(Me.ChekProducto)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 275)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(689, 75)
        Me.GroupBox1.TabIndex = 29
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Reportes Agrupados"
        '
        'CheckBLInfo
        '
        Me.CheckBLInfo.AutoSize = True
        Me.CheckBLInfo.Location = New System.Drawing.Point(428, 40)
        Me.CheckBLInfo.Name = "CheckBLInfo"
        Me.CheckBLInfo.Size = New System.Drawing.Size(113, 17)
        Me.CheckBLInfo.TabIndex = 15
        Me.CheckBLInfo.Text = "Informe Control BL"
        Me.CheckBLInfo.UseVisualStyleBackColor = True
        Me.CheckBLInfo.Visible = False
        '
        'CheckConsigna
        '
        Me.CheckConsigna.AutoSize = True
        Me.CheckConsigna.Location = New System.Drawing.Point(283, 42)
        Me.CheckConsigna.Name = "CheckConsigna"
        Me.CheckConsigna.Size = New System.Drawing.Size(90, 17)
        Me.CheckConsigna.TabIndex = 14
        Me.CheckConsigna.Text = "Consignatario"
        Me.CheckConsigna.UseVisualStyleBackColor = True
        '
        'ChekBuque
        '
        Me.ChekBuque.AutoSize = True
        Me.ChekBuque.Location = New System.Drawing.Point(130, 19)
        Me.ChekBuque.Name = "ChekBuque"
        Me.ChekBuque.Size = New System.Drawing.Size(76, 17)
        Me.ChekBuque.TabIndex = 13
        Me.ChekBuque.Text = "Por Buque"
        Me.ChekBuque.UseVisualStyleBackColor = True
        '
        'ChekBl
        '
        Me.ChekBl.AutoSize = True
        Me.ChekBl.Location = New System.Drawing.Point(130, 42)
        Me.ChekBl.Name = "ChekBl"
        Me.ChekBl.Size = New System.Drawing.Size(39, 17)
        Me.ChekBl.TabIndex = 12
        Me.ChekBl.Text = "BL"
        Me.ChekBl.UseVisualStyleBackColor = True
        '
        'ChekProductoResumen
        '
        Me.ChekProductoResumen.AutoSize = True
        Me.ChekProductoResumen.Location = New System.Drawing.Point(428, 18)
        Me.ChekProductoResumen.Name = "ChekProductoResumen"
        Me.ChekProductoResumen.Size = New System.Drawing.Size(125, 17)
        Me.ChekProductoResumen.TabIndex = 11
        Me.ChekProductoResumen.Text = "Resumido por Buque"
        Me.ChekProductoResumen.UseVisualStyleBackColor = True
        '
        'ChekProducto
        '
        Me.ChekProducto.AutoSize = True
        Me.ChekProducto.Location = New System.Drawing.Point(283, 19)
        Me.ChekProducto.Name = "ChekProducto"
        Me.ChekProducto.Size = New System.Drawing.Size(66, 17)
        Me.ChekProducto.TabIndex = 10
        Me.ChekProducto.Text = "Poducto"
        Me.ChekProducto.UseVisualStyleBackColor = True
        '
        'RepPorFechaBL
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(715, 502)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.Label4)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "RepPorFechaBL"
        Me.Text = "Informe para Analisis"
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents btnGrabar As System.Windows.Forms.Button
    Friend WithEvents btnSalir As System.Windows.Forms.Button
    Friend WithEvents dFechaFin As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dFechaIni As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents txtNomBuque As System.Windows.Forms.TextBox
    Friend WithEvents txtBuque As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtNomConsigna As System.Windows.Forms.TextBox
    Friend WithEvents txtConsigna As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txtBodega As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtViaje As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtNomProducto As System.Windows.Forms.TextBox
    Friend WithEvents txtProducto As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents ChekProducto As System.Windows.Forms.CheckBox
    Friend WithEvents ChekBl As System.Windows.Forms.CheckBox
    Friend WithEvents ChekProductoResumen As System.Windows.Forms.CheckBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ChekBuque As System.Windows.Forms.CheckBox
    Friend WithEvents dHoraFin As System.Windows.Forms.TextBox
    Friend WithEvents dHoraIni As System.Windows.Forms.TextBox
    Friend WithEvents txtBL As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents CheckConsigna As CheckBox
    Friend WithEvents CheckBLInfo As CheckBox
End Class
