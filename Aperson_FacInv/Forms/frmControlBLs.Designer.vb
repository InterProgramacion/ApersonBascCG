<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmControlBLs
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
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.txtViaje = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.txtNomConsigna = New System.Windows.Forms.TextBox()
        Me.cmbEstado = New System.Windows.Forms.ComboBox()
        Me.txtConsigna = New System.Windows.Forms.TextBox()
        Me.txtMerma = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtLinea = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtId = New System.Windows.Forms.TextBox()
        Me.btnBuscar = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtDiferencia = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.dFechav = New System.Windows.Forms.DateTimePicker()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.txtBL = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.txtNomBuque = New System.Windows.Forms.TextBox()
        Me.txtBuque = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtNomProducto = New System.Windows.Forms.TextBox()
        Me.txtProducto = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtPesoBascula = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtDuca = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cmbEstadoSAT = New System.Windows.Forms.ComboBox()
        Me.txtManifiesto = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.btnBaja = New System.Windows.Forms.Button()
        Me.btnAlta = New System.Windows.Forms.Button()
        Me.btnHabilitar = New System.Windows.Forms.Button()
        Me.btnSalir = New System.Windows.Forms.Button()
        Me.btnBorrar = New System.Windows.Forms.Button()
        Me.GrupoDatosBuscar = New System.Windows.Forms.GroupBox()
        Me.grdData = New System.Windows.Forms.DataGridView()
        Me.Label44 = New System.Windows.Forms.Label()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GrupoDatosBuscar.SuspendLayout()
        CType(Me.grdData, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.SeaGreen
        Me.Label4.Location = New System.Drawing.Point(16, 7)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(502, 29)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Detalle de BLs - Consignatarios y Pesajes"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label44)
        Me.GroupBox2.Controls.Add(Me.txtViaje)
        Me.GroupBox2.Controls.Add(Me.Label14)
        Me.GroupBox2.Controls.Add(Me.Label15)
        Me.GroupBox2.Controls.Add(Me.txtNomConsigna)
        Me.GroupBox2.Controls.Add(Me.cmbEstado)
        Me.GroupBox2.Controls.Add(Me.txtConsigna)
        Me.GroupBox2.Controls.Add(Me.txtMerma)
        Me.GroupBox2.Controls.Add(Me.Label10)
        Me.GroupBox2.Controls.Add(Me.txtLinea)
        Me.GroupBox2.Controls.Add(Me.Label11)
        Me.GroupBox2.Controls.Add(Me.txtId)
        Me.GroupBox2.Controls.Add(Me.btnBuscar)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.txtDiferencia)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.dFechav)
        Me.GroupBox2.Controls.Add(Me.Label31)
        Me.GroupBox2.Controls.Add(Me.txtBL)
        Me.GroupBox2.Controls.Add(Me.Label13)
        Me.GroupBox2.Controls.Add(Me.txtNomBuque)
        Me.GroupBox2.Controls.Add(Me.txtBuque)
        Me.GroupBox2.Controls.Add(Me.Label9)
        Me.GroupBox2.Controls.Add(Me.txtNomProducto)
        Me.GroupBox2.Controls.Add(Me.txtProducto)
        Me.GroupBox2.Controls.Add(Me.Label12)
        Me.GroupBox2.Controls.Add(Me.txtPesoBascula)
        Me.GroupBox2.Controls.Add(Me.Label7)
        Me.GroupBox2.Controls.Add(Me.txtDuca)
        Me.GroupBox2.Controls.Add(Me.Label8)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Controls.Add(Me.cmbEstadoSAT)
        Me.GroupBox2.Controls.Add(Me.txtManifiesto)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Location = New System.Drawing.Point(16, 50)
        Me.GroupBox2.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox2.Size = New System.Drawing.Size(1463, 156)
        Me.GroupBox2.TabIndex = 0
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Datos Generales"
        '
        'txtViaje
        '
        Me.txtViaje.Enabled = False
        Me.txtViaje.Location = New System.Drawing.Point(117, 54)
        Me.txtViaje.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtViaje.Name = "txtViaje"
        Me.txtViaje.Size = New System.Drawing.Size(89, 22)
        Me.txtViaje.TabIndex = 6
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(451, 121)
        Me.Label14.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(54, 16)
        Me.Label14.TabIndex = 80
        Me.Label14.Text = "Estatus:"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(19, 62)
        Me.Label15.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(65, 16)
        Me.Label15.TabIndex = 80
        Me.Label15.Text = "No. Viaje:"
        '
        'txtNomConsigna
        '
        Me.txtNomConsigna.Enabled = False
        Me.txtNomConsigna.Location = New System.Drawing.Point(939, 54)
        Me.txtNomConsigna.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtNomConsigna.Name = "txtNomConsigna"
        Me.txtNomConsigna.Size = New System.Drawing.Size(488, 22)
        Me.txtNomConsigna.TabIndex = 78
        '
        'cmbEstado
        '
        Me.cmbEstado.Enabled = False
        Me.cmbEstado.FormattingEnabled = True
        Me.cmbEstado.Items.AddRange(New Object() {"ACTIVO", "INACTIVO"})
        Me.cmbEstado.Location = New System.Drawing.Point(519, 117)
        Me.cmbEstado.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cmbEstado.Name = "cmbEstado"
        Me.cmbEstado.Size = New System.Drawing.Size(228, 24)
        Me.cmbEstado.TabIndex = 15
        '
        'txtConsigna
        '
        Me.txtConsigna.Location = New System.Drawing.Point(865, 54)
        Me.txtConsigna.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtConsigna.Name = "txtConsigna"
        Me.txtConsigna.Size = New System.Drawing.Size(67, 22)
        Me.txtConsigna.TabIndex = 9
        '
        'txtMerma
        '
        Me.txtMerma.Location = New System.Drawing.Point(117, 86)
        Me.txtMerma.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtMerma.Name = "txtMerma"
        Me.txtMerma.Size = New System.Drawing.Size(89, 22)
        Me.txtMerma.TabIndex = 10
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(19, 91)
        Me.Label10.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(52, 16)
        Me.Label10.TabIndex = 63
        Me.Label10.Text = "Merma:"
        '
        'txtLinea
        '
        Me.txtLinea.Enabled = False
        Me.txtLinea.Location = New System.Drawing.Point(408, 23)
        Me.txtLinea.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtLinea.Name = "txtLinea"
        Me.txtLinea.Size = New System.Drawing.Size(76, 22)
        Me.txtLinea.TabIndex = 2
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(356, 27)
        Me.Label11.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(43, 16)
        Me.Label11.TabIndex = 76
        Me.Label11.Text = "Linea:"
        '
        'txtId
        '
        Me.txtId.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtId.Location = New System.Drawing.Point(156, 22)
        Me.txtId.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtId.Name = "txtId"
        Me.txtId.Size = New System.Drawing.Size(189, 26)
        Me.txtId.TabIndex = 4
        '
        'btnBuscar
        '
        Me.btnBuscar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBuscar.Image = Global.AperBascu.My.Resources.Resources.LOCATE
        Me.btnBuscar.Location = New System.Drawing.Point(117, 22)
        Me.btnBuscar.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnBuscar.Name = "btnBuscar"
        Me.btnBuscar.Size = New System.Drawing.Size(29, 25)
        Me.btnBuscar.TabIndex = 72
        Me.btnBuscar.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(19, 28)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(75, 16)
        Me.Label1.TabIndex = 73
        Me.Label1.Text = "Correlativo:"
        '
        'txtDiferencia
        '
        Me.txtDiferencia.Enabled = False
        Me.txtDiferencia.Location = New System.Drawing.Point(1227, 117)
        Me.txtDiferencia.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtDiferencia.Name = "txtDiferencia"
        Me.txtDiferencia.Size = New System.Drawing.Size(179, 22)
        Me.txtDiferencia.TabIndex = 17
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(1133, 122)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(79, 16)
        Me.Label2.TabIndex = 67
        Me.Label2.Text = "Difderencia:"
        '
        'dFechav
        '
        Me.dFechav.Enabled = False
        Me.dFechav.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dFechav.Location = New System.Drawing.Point(305, 57)
        Me.dFechav.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.dFechav.Name = "dFechav"
        Me.dFechav.Size = New System.Drawing.Size(121, 22)
        Me.dFechav.TabIndex = 7
        Me.dFechav.Value = New Date(2017, 7, 4, 4, 25, 31, 0)
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.Location = New System.Drawing.Point(232, 59)
        Me.Label31.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(48, 16)
        Me.Label31.TabIndex = 65
        Me.Label31.Text = "Fecha:"
        '
        'txtBL
        '
        Me.txtBL.Location = New System.Drawing.Point(305, 86)
        Me.txtBL.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtBL.Name = "txtBL"
        Me.txtBL.Size = New System.Drawing.Size(121, 22)
        Me.txtBL.TabIndex = 11
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(755, 59)
        Me.Label13.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(93, 16)
        Me.Label13.TabIndex = 64
        Me.Label13.Text = "Consignatario:"
        '
        'txtNomBuque
        '
        Me.txtNomBuque.Enabled = False
        Me.txtNomBuque.Location = New System.Drawing.Point(939, 23)
        Me.txtNomBuque.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtNomBuque.Name = "txtNomBuque"
        Me.txtNomBuque.Size = New System.Drawing.Size(488, 22)
        Me.txtNomBuque.TabIndex = 56
        '
        'txtBuque
        '
        Me.txtBuque.Location = New System.Drawing.Point(865, 23)
        Me.txtBuque.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtBuque.Name = "txtBuque"
        Me.txtBuque.Size = New System.Drawing.Size(67, 22)
        Me.txtBuque.TabIndex = 5
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(755, 27)
        Me.Label9.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(49, 16)
        Me.Label9.TabIndex = 62
        Me.Label9.Text = "Buque:"
        '
        'txtNomProducto
        '
        Me.txtNomProducto.Enabled = False
        Me.txtNomProducto.Location = New System.Drawing.Point(592, 85)
        Me.txtNomProducto.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtNomProducto.Name = "txtNomProducto"
        Me.txtNomProducto.Size = New System.Drawing.Size(488, 22)
        Me.txtNomProducto.TabIndex = 54
        '
        'txtProducto
        '
        Me.txtProducto.Location = New System.Drawing.Point(519, 85)
        Me.txtProducto.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtProducto.Name = "txtProducto"
        Me.txtProducto.Size = New System.Drawing.Size(67, 22)
        Me.txtProducto.TabIndex = 12
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(443, 89)
        Me.Label12.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(64, 16)
        Me.Label12.TabIndex = 53
        Me.Label12.Text = "Producto:"
        '
        'txtPesoBascula
        '
        Me.txtPesoBascula.Enabled = False
        Me.txtPesoBascula.Location = New System.Drawing.Point(880, 117)
        Me.txtPesoBascula.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtPesoBascula.Name = "txtPesoBascula"
        Me.txtPesoBascula.Size = New System.Drawing.Size(200, 22)
        Me.txtPesoBascula.TabIndex = 16
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(772, 121)
        Me.Label7.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(94, 16)
        Me.Label7.TabIndex = 36
        Me.Label7.Text = "Peso Bascula:"
        '
        'txtDuca
        '
        Me.txtDuca.Location = New System.Drawing.Point(517, 54)
        Me.txtDuca.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtDuca.Name = "txtDuca"
        Me.txtDuca.Size = New System.Drawing.Size(228, 22)
        Me.txtDuca.TabIndex = 8
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(443, 59)
        Me.Label8.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(42, 16)
        Me.Label8.TabIndex = 33
        Me.Label8.Text = "Duca:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(8, 126)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(96, 16)
        Me.Label6.TabIndex = 32
        Me.Label6.Text = "Autrizado SAT:"
        '
        'cmbEstadoSAT
        '
        Me.cmbEstadoSAT.FormattingEnabled = True
        Me.cmbEstadoSAT.Items.AddRange(New Object() {"ACTIVO", "INACTIVO"})
        Me.cmbEstadoSAT.Location = New System.Drawing.Point(117, 119)
        Me.cmbEstadoSAT.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.cmbEstadoSAT.Name = "cmbEstadoSAT"
        Me.cmbEstadoSAT.Size = New System.Drawing.Size(228, 24)
        Me.cmbEstadoSAT.TabIndex = 14
        '
        'txtManifiesto
        '
        Me.txtManifiesto.Location = New System.Drawing.Point(1227, 85)
        Me.txtManifiesto.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtManifiesto.Name = "txtManifiesto"
        Me.txtManifiesto.Size = New System.Drawing.Size(179, 22)
        Me.txtManifiesto.TabIndex = 13
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(1133, 90)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(71, 16)
        Me.Label5.TabIndex = 29
        Me.Label5.Text = "Manifiesto:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(232, 90)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(26, 16)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "BL:"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.btnBaja)
        Me.GroupBox3.Controls.Add(Me.btnAlta)
        Me.GroupBox3.Controls.Add(Me.btnHabilitar)
        Me.GroupBox3.Controls.Add(Me.btnSalir)
        Me.GroupBox3.Controls.Add(Me.btnBorrar)
        Me.GroupBox3.Location = New System.Drawing.Point(16, 646)
        Me.GroupBox3.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GroupBox3.Size = New System.Drawing.Size(1463, 138)
        Me.GroupBox3.TabIndex = 28
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Botones Utilitarios"
        '
        'btnBaja
        '
        Me.btnBaja.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnBaja.BackColor = System.Drawing.Color.IndianRed
        Me.btnBaja.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBaja.Font = New System.Drawing.Font("MS Reference Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBaja.ForeColor = System.Drawing.SystemColors.Window
        Me.btnBaja.Image = Global.AperBascu.My.Resources.Resources.list2_delete
        Me.btnBaja.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnBaja.Location = New System.Drawing.Point(188, 38)
        Me.btnBaja.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnBaja.Name = "btnBaja"
        Me.btnBaja.Size = New System.Drawing.Size(159, 50)
        Me.btnBaja.TabIndex = 22
        Me.btnBaja.Text = "Borrar"
        Me.btnBaja.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnBaja.UseVisualStyleBackColor = False
        '
        'btnAlta
        '
        Me.btnAlta.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnAlta.BackColor = System.Drawing.Color.Green
        Me.btnAlta.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAlta.Font = New System.Drawing.Font("MS Reference Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAlta.ForeColor = System.Drawing.SystemColors.Window
        Me.btnAlta.Image = Global.AperBascu.My.Resources.Resources.list_accept
        Me.btnAlta.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnAlta.Location = New System.Drawing.Point(23, 38)
        Me.btnAlta.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnAlta.Name = "btnAlta"
        Me.btnAlta.Size = New System.Drawing.Size(159, 50)
        Me.btnAlta.TabIndex = 21
        Me.btnAlta.Text = "Insertar"
        Me.btnAlta.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnAlta.UseVisualStyleBackColor = False
        '
        'btnHabilitar
        '
        Me.btnHabilitar.AutoSize = True
        Me.btnHabilitar.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.btnHabilitar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnHabilitar.Font = New System.Drawing.Font("MS Reference Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnHabilitar.ForeColor = System.Drawing.SystemColors.MenuText
        Me.btnHabilitar.Image = Global.AperBascu.My.Resources.Resources.pack2
        Me.btnHabilitar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnHabilitar.Location = New System.Drawing.Point(621, 38)
        Me.btnHabilitar.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnHabilitar.Name = "btnHabilitar"
        Me.btnHabilitar.Size = New System.Drawing.Size(215, 52)
        Me.btnHabilitar.TabIndex = 20
        Me.btnHabilitar.Text = "Afecta Cambios"
        Me.btnHabilitar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnHabilitar.UseVisualStyleBackColor = False
        Me.btnHabilitar.Visible = False
        '
        'btnSalir
        '
        Me.btnSalir.AutoSize = True
        Me.btnSalir.BackColor = System.Drawing.Color.SeaGreen
        Me.btnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSalir.Font = New System.Drawing.Font("MS Reference Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSalir.ForeColor = System.Drawing.SystemColors.Window
        Me.btnSalir.Image = Global.AperBascu.My.Resources.Resources.ClosedDoor
        Me.btnSalir.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnSalir.Location = New System.Drawing.Point(1293, 38)
        Me.btnSalir.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnSalir.Name = "btnSalir"
        Me.btnSalir.Size = New System.Drawing.Size(159, 69)
        Me.btnSalir.TabIndex = 3
        Me.btnSalir.Text = "Salir"
        Me.btnSalir.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnSalir.UseVisualStyleBackColor = False
        '
        'btnBorrar
        '
        Me.btnBorrar.AutoSize = True
        Me.btnBorrar.BackColor = System.Drawing.Color.SeaGreen
        Me.btnBorrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBorrar.Font = New System.Drawing.Font("MS Reference Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBorrar.ForeColor = System.Drawing.SystemColors.Window
        Me.btnBorrar.Image = Global.AperBascu.My.Resources.Resources.Stop_21
        Me.btnBorrar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnBorrar.Location = New System.Drawing.Point(1127, 38)
        Me.btnBorrar.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.btnBorrar.Name = "btnBorrar"
        Me.btnBorrar.Size = New System.Drawing.Size(159, 69)
        Me.btnBorrar.TabIndex = 2
        Me.btnBorrar.Text = "Eliminar"
        Me.btnBorrar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnBorrar.UseVisualStyleBackColor = False
        '
        'GrupoDatosBuscar
        '
        Me.GrupoDatosBuscar.Controls.Add(Me.grdData)
        Me.GrupoDatosBuscar.Font = New System.Drawing.Font("Calibri", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GrupoDatosBuscar.Location = New System.Drawing.Point(16, 214)
        Me.GrupoDatosBuscar.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GrupoDatosBuscar.Name = "GrupoDatosBuscar"
        Me.GrupoDatosBuscar.Padding = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.GrupoDatosBuscar.Size = New System.Drawing.Size(1463, 425)
        Me.GrupoDatosBuscar.TabIndex = 29
        Me.GrupoDatosBuscar.TabStop = False
        Me.GrupoDatosBuscar.Text = "Detalle de Buque"
        '
        'grdData
        '
        Me.grdData.AllowUserToAddRows = False
        Me.grdData.AllowUserToDeleteRows = False
        Me.grdData.AllowUserToResizeRows = False
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.Khaki
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.Black
        Me.grdData.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle4
        Me.grdData.ColumnHeadersHeight = 29
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Calibri", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.Khaki
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.grdData.DefaultCellStyle = DataGridViewCellStyle5
        Me.grdData.Location = New System.Drawing.Point(8, 25)
        Me.grdData.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.grdData.Name = "grdData"
        Me.grdData.RowHeadersWidth = 51
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.Khaki
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Black
        Me.grdData.RowsDefaultCellStyle = DataGridViewCellStyle6
        Me.grdData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.grdData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.grdData.ShowCellToolTips = False
        Me.grdData.Size = New System.Drawing.Size(1447, 393)
        Me.grdData.TabIndex = 21
        '
        'Label44
        '
        Me.Label44.AutoSize = True
        Me.Label44.Location = New System.Drawing.Point(1414, 88)
        Me.Label44.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label44.Name = "Label44"
        Me.Label44.Size = New System.Drawing.Size(26, 16)
        Me.Label44.TabIndex = 81
        Me.Label44.Text = "TN"
        '
        'frmControlBLs
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1495, 799)
        Me.Controls.Add(Me.GrupoDatosBuscar)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.Label4)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmControlBLs"
        Me.Text = "Detalle de BLs y Consignatarios"
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GrupoDatosBuscar.ResumeLayout(False)
        CType(Me.grdData, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents btnSalir As System.Windows.Forms.Button
    Friend WithEvents btnBorrar As System.Windows.Forms.Button
    Friend WithEvents txtManifiesto As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cmbEstadoSAT As System.Windows.Forms.ComboBox
    Friend WithEvents txtPesoBascula As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtDuca As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents dFechav As DateTimePicker
    Friend WithEvents Label31 As Label
    Friend WithEvents txtBL As TextBox
    Friend WithEvents Label13 As Label
    Friend WithEvents txtMerma As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents txtNomBuque As TextBox
    Friend WithEvents txtBuque As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents txtNomProducto As TextBox
    Friend WithEvents txtProducto As TextBox
    Friend WithEvents Label12 As Label
    Friend WithEvents txtDiferencia As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents GrupoDatosBuscar As GroupBox
    Friend WithEvents grdData As DataGridView
    Friend WithEvents txtId As TextBox
    Friend WithEvents btnBuscar As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents txtLinea As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents txtNomConsigna As TextBox
    Friend WithEvents txtConsigna As TextBox
    Friend WithEvents txtViaje As TextBox
    Friend WithEvents Label15 As Label
    Friend WithEvents Label14 As Label
    Friend WithEvents cmbEstado As ComboBox
    Friend WithEvents btnHabilitar As Button
    Friend WithEvents btnBaja As Button
    Friend WithEvents btnAlta As Button
    Friend WithEvents Label44 As Label
End Class
