<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmCargaMasiva
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
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

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCargaMasiva))
        Me.lblTitulo = New System.Windows.Forms.Label()
        Me.grpArchivo = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnCargar = New System.Windows.Forms.Button()
        Me.btnSeleccionar = New System.Windows.Forms.Button()
        Me.txtRuta = New System.Windows.Forms.TextBox()
        Me.lblHoja = New System.Windows.Forms.Label()
        Me.grpBotones = New System.Windows.Forms.GroupBox()
        Me.btnSalir = New System.Windows.Forms.Button()
        Me.btnDB = New System.Windows.Forms.Button()
        Me.dgvDatos = New System.Windows.Forms.DataGridView()
        Me.lblCargando = New System.Windows.Forms.Label()
        Me.grpArchivo.SuspendLayout()
        Me.grpBotones.SuspendLayout()
        CType(Me.dgvDatos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblTitulo
        '
        Me.lblTitulo.AutoSize = True
        Me.lblTitulo.Location = New System.Drawing.Point(409, 11)
        Me.lblTitulo.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblTitulo.Name = "lblTitulo"
        Me.lblTitulo.Size = New System.Drawing.Size(221, 16)
        Me.lblTitulo.TabIndex = 0
        Me.lblTitulo.Text = "Carga Masiva de Datos – INGRESO"
        '
        'grpArchivo
        '
        Me.grpArchivo.Controls.Add(Me.Label1)
        Me.grpArchivo.Controls.Add(Me.btnCargar)
        Me.grpArchivo.Controls.Add(Me.btnSeleccionar)
        Me.grpArchivo.Controls.Add(Me.txtRuta)
        Me.grpArchivo.Location = New System.Drawing.Point(16, 46)
        Me.grpArchivo.Margin = New System.Windows.Forms.Padding(4)
        Me.grpArchivo.Name = "grpArchivo"
        Me.grpArchivo.Padding = New System.Windows.Forms.Padding(4)
        Me.grpArchivo.Size = New System.Drawing.Size(1309, 107)
        Me.grpArchivo.TabIndex = 1
        Me.grpArchivo.TabStop = False
        Me.grpArchivo.Text = "Datos del Archivo"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 31)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(38, 16)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Ruta:"
        '
        'btnCargar
        '
        Me.btnCargar.Location = New System.Drawing.Point(61, 63)
        Me.btnCargar.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCargar.Name = "btnCargar"
        Me.btnCargar.Size = New System.Drawing.Size(100, 28)
        Me.btnCargar.TabIndex = 4
        Me.btnCargar.Text = "Cargar Datos"
        Me.btnCargar.UseVisualStyleBackColor = True
        '
        'btnSeleccionar
        '
        Me.btnSeleccionar.Location = New System.Drawing.Point(603, 27)
        Me.btnSeleccionar.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSeleccionar.Name = "btnSeleccionar"
        Me.btnSeleccionar.Size = New System.Drawing.Size(157, 28)
        Me.btnSeleccionar.TabIndex = 3
        Me.btnSeleccionar.Text = "Seleccionar Archivo"
        Me.btnSeleccionar.UseVisualStyleBackColor = True
        '
        'txtRuta
        '
        Me.txtRuta.Location = New System.Drawing.Point(61, 27)
        Me.txtRuta.Margin = New System.Windows.Forms.Padding(4)
        Me.txtRuta.Name = "txtRuta"
        Me.txtRuta.ReadOnly = True
        Me.txtRuta.Size = New System.Drawing.Size(532, 22)
        Me.txtRuta.TabIndex = 2
        '
        'lblHoja
        '
        Me.lblHoja.AutoSize = True
        Me.lblHoja.Location = New System.Drawing.Point(967, 89)
        Me.lblHoja.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblHoja.Name = "lblHoja"
        Me.lblHoja.Size = New System.Drawing.Size(0, 16)
        Me.lblHoja.TabIndex = 5
        '
        'grpBotones
        '
        Me.grpBotones.Controls.Add(Me.btnSalir)
        Me.grpBotones.Controls.Add(Me.btnDB)
        Me.grpBotones.Location = New System.Drawing.Point(16, 874)
        Me.grpBotones.Margin = New System.Windows.Forms.Padding(4)
        Me.grpBotones.Name = "grpBotones"
        Me.grpBotones.Padding = New System.Windows.Forms.Padding(4)
        Me.grpBotones.Size = New System.Drawing.Size(253, 53)
        Me.grpBotones.TabIndex = 8
        Me.grpBotones.TabStop = False
        Me.grpBotones.Text = "Opciones"
        '
        'btnSalir
        '
        Me.btnSalir.Location = New System.Drawing.Point(129, 17)
        Me.btnSalir.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSalir.Name = "btnSalir"
        Me.btnSalir.Size = New System.Drawing.Size(100, 28)
        Me.btnSalir.TabIndex = 1
        Me.btnSalir.Text = "Salir"
        Me.btnSalir.UseVisualStyleBackColor = True
        '
        'btnDB
        '
        Me.btnDB.Location = New System.Drawing.Point(8, 17)
        Me.btnDB.Margin = New System.Windows.Forms.Padding(4)
        Me.btnDB.Name = "btnDB"
        Me.btnDB.Size = New System.Drawing.Size(100, 28)
        Me.btnDB.TabIndex = 0
        Me.btnDB.Text = "Guardar"
        Me.btnDB.UseVisualStyleBackColor = True
        '
        'dgvDatos
        '
        Me.dgvDatos.AllowUserToAddRows = False
        Me.dgvDatos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvDatos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDatos.Location = New System.Drawing.Point(16, 160)
        Me.dgvDatos.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvDatos.Name = "dgvDatos"
        Me.dgvDatos.RowHeadersWidth = 51
        Me.dgvDatos.Size = New System.Drawing.Size(1309, 706)
        Me.dgvDatos.TabIndex = 6
        '
        'lblCargando
        '
        Me.lblCargando.AutoSize = True
        Me.lblCargando.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblCargando.Font = New System.Drawing.Font("Microsoft Sans Serif", 19.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCargando.ForeColor = System.Drawing.SystemColors.Window
        Me.lblCargando.Location = New System.Drawing.Point(395, 475)
        Me.lblCargando.Name = "lblCargando"
        Me.lblCargando.Size = New System.Drawing.Size(492, 38)
        Me.lblCargando.TabIndex = 9
        Me.lblCargando.Text = "Cargando por favor espere. . . "
        Me.lblCargando.Visible = False
        '
        'frmCargaMasiva
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1341, 942)
        Me.Controls.Add(Me.lblCargando)
        Me.Controls.Add(Me.grpBotones)
        Me.Controls.Add(Me.dgvDatos)
        Me.Controls.Add(Me.lblHoja)
        Me.Controls.Add(Me.grpArchivo)
        Me.Controls.Add(Me.lblTitulo)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmCargaMasiva"
        Me.Text = "Carga Masiva"
        Me.grpArchivo.ResumeLayout(False)
        Me.grpArchivo.PerformLayout()
        Me.grpBotones.ResumeLayout(False)
        CType(Me.dgvDatos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblTitulo As Label
    Friend WithEvents grpArchivo As GroupBox
    Friend WithEvents txtRuta As TextBox
    Friend WithEvents btnSeleccionar As Button
    Friend WithEvents btnCargar As Button
    Friend WithEvents lblHoja As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents btnSalir As Button
    Friend WithEvents btnDB As Button
    Friend WithEvents grpBotones As GroupBox
    Friend WithEvents dgvDatos As DataGridView
    Friend WithEvents lblCargando As Label
End Class
