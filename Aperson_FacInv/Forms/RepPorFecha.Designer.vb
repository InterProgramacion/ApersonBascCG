<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RepPorFecha
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer


    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtNomNaviera = New System.Windows.Forms.TextBox()
        Me.txtNaviera = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.txtViaje = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.cmbTipoOperacion = New System.Windows.Forms.ComboBox()
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
        Me.ChekNaviera = New System.Windows.Forms.CheckBox()
        Me.ChekProducto = New System.Windows.Forms.CheckBox()
        Me.ChekTipoO = New System.Windows.Forms.CheckBox()
        Me.ChekViaje = New System.Windows.Forms.CheckBox()
        Me.ChekBuque = New System.Windows.Forms.CheckBox()
        Me.txtBodega = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ChekBodega = New System.Windows.Forms.CheckBox()
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
        Me.Label4.Size = New System.Drawing.Size(283, 24)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Reporte por Rango de Fecha"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.txtBodega)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.txtNomNaviera)
        Me.GroupBox2.Controls.Add(Me.txtNaviera)
        Me.GroupBox2.Controls.Add(Me.Label15)
        Me.GroupBox2.Controls.Add(Me.txtViaje)
        Me.GroupBox2.Controls.Add(Me.Label10)
        Me.GroupBox2.Controls.Add(Me.cmbTipoOperacion)
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
        Me.GroupBox2.Size = New System.Drawing.Size(689, 200)
        Me.GroupBox2.TabIndex = 0
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Rangos de Impresión"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(62, 112)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(83, 13)
        Me.Label2.TabIndex = 72
        Me.Label2.Text = "Tipo Operación:"
        '
        'txtNomNaviera
        '
        Me.txtNomNaviera.Enabled = False
        Me.txtNomNaviera.Location = New System.Drawing.Point(210, 160)
        Me.txtNomNaviera.Name = "txtNomNaviera"
        Me.txtNomNaviera.Size = New System.Drawing.Size(367, 20)
        Me.txtNomNaviera.TabIndex = 68
        '
        'txtNaviera
        '
        Me.txtNaviera.Location = New System.Drawing.Point(155, 160)
        Me.txtNaviera.Name = "txtNaviera"
        Me.txtNaviera.Size = New System.Drawing.Size(51, 20)
        Me.txtNaviera.TabIndex = 8
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(98, 161)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(51, 13)
        Me.Label15.TabIndex = 71
        Me.Label15.Text = "Empresa:"
        '
        'txtViaje
        '
        Me.txtViaje.Location = New System.Drawing.Point(155, 81)
        Me.txtViaje.Name = "txtViaje"
        Me.txtViaje.Size = New System.Drawing.Size(68, 20)
        Me.txtViaje.TabIndex = 4
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(58, 84)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(87, 13)
        Me.Label10.TabIndex = 69
        Me.Label10.Text = "No. Viaje Buque:"
        '
        'cmbTipoOperacion
        '
        Me.cmbTipoOperacion.FormattingEnabled = True
        Me.cmbTipoOperacion.Items.AddRange(New Object() {"TODOS", "RECEPCION", "DESPACHO"})
        Me.cmbTipoOperacion.Location = New System.Drawing.Point(155, 107)
        Me.cmbTipoOperacion.Name = "cmbTipoOperacion"
        Me.cmbTipoOperacion.Size = New System.Drawing.Size(194, 21)
        Me.cmbTipoOperacion.TabIndex = 5
        '
        'txtNomProducto
        '
        Me.txtNomProducto.Enabled = False
        Me.txtNomProducto.Location = New System.Drawing.Point(210, 134)
        Me.txtNomProducto.Name = "txtNomProducto"
        Me.txtNomProducto.Size = New System.Drawing.Size(367, 20)
        Me.txtNomProducto.TabIndex = 61
        '
        'txtProducto
        '
        Me.txtProducto.Location = New System.Drawing.Point(155, 134)
        Me.txtProducto.Name = "txtProducto"
        Me.txtProducto.Size = New System.Drawing.Size(51, 20)
        Me.txtProducto.TabIndex = 7
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(92, 136)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(53, 13)
        Me.Label12.TabIndex = 60
        Me.Label12.Text = "Producto:"
        '
        'txtNomBuque
        '
        Me.txtNomBuque.Enabled = False
        Me.txtNomBuque.Location = New System.Drawing.Point(210, 55)
        Me.txtNomBuque.Name = "txtNomBuque"
        Me.txtNomBuque.Size = New System.Drawing.Size(367, 20)
        Me.txtNomBuque.TabIndex = 58
        '
        'txtBuque
        '
        Me.txtBuque.Location = New System.Drawing.Point(155, 55)
        Me.txtBuque.Name = "txtBuque"
        Me.txtBuque.Size = New System.Drawing.Size(51, 20)
        Me.txtBuque.TabIndex = 3
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(104, 56)
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
        Me.Label31.Location = New System.Drawing.Point(105, 32)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(40, 13)
        Me.Label31.TabIndex = 54
        Me.Label31.Text = "Fecha:"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.btnGrabar)
        Me.GroupBox3.Controls.Add(Me.btnSalir)
        Me.GroupBox3.Location = New System.Drawing.Point(16, 351)
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
        Me.GroupBox1.Controls.Add(Me.ChekBodega)
        Me.GroupBox1.Controls.Add(Me.ChekNaviera)
        Me.GroupBox1.Controls.Add(Me.ChekProducto)
        Me.GroupBox1.Controls.Add(Me.ChekTipoO)
        Me.GroupBox1.Controls.Add(Me.ChekViaje)
        Me.GroupBox1.Controls.Add(Me.ChekBuque)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 254)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(689, 75)
        Me.GroupBox1.TabIndex = 29
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Reportes Agrupados"
        '
        'ChekNaviera
        '
        Me.ChekNaviera.AutoSize = True
        Me.ChekNaviera.Location = New System.Drawing.Point(345, 45)
        Me.ChekNaviera.Name = "ChekNaviera"
        Me.ChekNaviera.Size = New System.Drawing.Size(86, 17)
        Me.ChekNaviera.TabIndex = 14
        Me.ChekNaviera.Text = "Por Empresa"
        Me.ChekNaviera.UseVisualStyleBackColor = True
        '
        'ChekProducto
        '
        Me.ChekProducto.AutoSize = True
        Me.ChekProducto.Location = New System.Drawing.Point(345, 19)
        Me.ChekProducto.Name = "ChekProducto"
        Me.ChekProducto.Size = New System.Drawing.Size(88, 17)
        Me.ChekProducto.TabIndex = 13
        Me.ChekProducto.Text = "Por Producto"
        Me.ChekProducto.UseVisualStyleBackColor = True
        '
        'ChekTipoO
        '
        Me.ChekTipoO.AutoSize = True
        Me.ChekTipoO.Location = New System.Drawing.Point(210, 42)
        Me.ChekTipoO.Name = "ChekTipoO"
        Me.ChekTipoO.Size = New System.Drawing.Size(118, 17)
        Me.ChekTipoO.TabIndex = 12
        Me.ChekTipoO.Text = "Por Tipo Operación"
        Me.ChekTipoO.UseVisualStyleBackColor = True
        '
        'ChekViaje
        '
        Me.ChekViaje.AutoSize = True
        Me.ChekViaje.Location = New System.Drawing.Point(108, 45)
        Me.ChekViaje.Name = "ChekViaje"
        Me.ChekViaje.Size = New System.Drawing.Size(68, 17)
        Me.ChekViaje.TabIndex = 10
        Me.ChekViaje.Text = "Por Viaje"
        Me.ChekViaje.UseVisualStyleBackColor = True
        '
        'ChekBuque
        '
        Me.ChekBuque.AutoSize = True
        Me.ChekBuque.Location = New System.Drawing.Point(108, 22)
        Me.ChekBuque.Name = "ChekBuque"
        Me.ChekBuque.Size = New System.Drawing.Size(76, 17)
        Me.ChekBuque.TabIndex = 9
        Me.ChekBuque.Text = "Por Buque"
        Me.ChekBuque.UseVisualStyleBackColor = True
        '
        'txtBodega
        '
        Me.txtBodega.Location = New System.Drawing.Point(508, 81)
        Me.txtBodega.Name = "txtBodega"
        Me.txtBodega.Size = New System.Drawing.Size(68, 20)
        Me.txtBodega.TabIndex = 73
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(455, 84)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(47, 13)
        Me.Label3.TabIndex = 74
        Me.Label3.Text = "Bodega:"
        '
        'ChekBodega
        '
        Me.ChekBodega.AutoSize = True
        Me.ChekBodega.Location = New System.Drawing.Point(210, 22)
        Me.ChekBodega.Name = "ChekBodega"
        Me.ChekBodega.Size = New System.Drawing.Size(82, 17)
        Me.ChekBodega.TabIndex = 11
        Me.ChekBodega.Text = "Por Bodega"
        Me.ChekBodega.UseVisualStyleBackColor = True
        '
        'RepPorFecha
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(715, 462)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.Label4)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "RepPorFecha"
        Me.Text = "Informe por Fechas"
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
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtNomNaviera As System.Windows.Forms.TextBox
    Friend WithEvents txtNaviera As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txtViaje As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents cmbTipoOperacion As System.Windows.Forms.ComboBox
    Friend WithEvents txtNomProducto As System.Windows.Forms.TextBox
    Friend WithEvents txtProducto As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents ChekBuque As System.Windows.Forms.CheckBox
    Friend WithEvents ChekNaviera As System.Windows.Forms.CheckBox
    Friend WithEvents ChekProducto As System.Windows.Forms.CheckBox
    Friend WithEvents ChekTipoO As System.Windows.Forms.CheckBox
    Friend WithEvents ChekViaje As System.Windows.Forms.CheckBox
    Friend WithEvents txtBodega As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents ChekBodega As CheckBox
End Class
