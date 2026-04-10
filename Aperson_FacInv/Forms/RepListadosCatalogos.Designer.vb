<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RepListadosCatalogos
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
        Me.btnBuques = New System.Windows.Forms.Button()
        Me.btnClientes = New System.Windows.Forms.Button()
        Me.btnProductos = New System.Windows.Forms.Button()
        Me.btnCamiones = New System.Windows.Forms.Button()
        Me.btnTransporte = New System.Windows.Forms.Button()
        Me.btrnBasculas = New System.Windows.Forms.Button()
        Me.btnUsuarios = New System.Windows.Forms.Button()
        Me.btnSalir = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnBuques
        '
        Me.btnBuques.Location = New System.Drawing.Point(17, 19)
        Me.btnBuques.Name = "btnBuques"
        Me.btnBuques.Size = New System.Drawing.Size(166, 39)
        Me.btnBuques.TabIndex = 0
        Me.btnBuques.Text = "Listado de Buques"
        Me.btnBuques.UseVisualStyleBackColor = True
        '
        'btnClientes
        '
        Me.btnClientes.Location = New System.Drawing.Point(17, 61)
        Me.btnClientes.Name = "btnClientes"
        Me.btnClientes.Size = New System.Drawing.Size(166, 39)
        Me.btnClientes.TabIndex = 1
        Me.btnClientes.Text = "Listado Clientes"
        Me.btnClientes.UseVisualStyleBackColor = True
        '
        'btnProductos
        '
        Me.btnProductos.Location = New System.Drawing.Point(17, 103)
        Me.btnProductos.Name = "btnProductos"
        Me.btnProductos.Size = New System.Drawing.Size(166, 39)
        Me.btnProductos.TabIndex = 2
        Me.btnProductos.Text = "Listado Productos"
        Me.btnProductos.UseVisualStyleBackColor = True
        '
        'btnCamiones
        '
        Me.btnCamiones.Location = New System.Drawing.Point(17, 146)
        Me.btnCamiones.Name = "btnCamiones"
        Me.btnCamiones.Size = New System.Drawing.Size(166, 39)
        Me.btnCamiones.TabIndex = 3
        Me.btnCamiones.Text = "Listado Camiones"
        Me.btnCamiones.UseVisualStyleBackColor = True
        '
        'btnTransporte
        '
        Me.btnTransporte.Location = New System.Drawing.Point(17, 189)
        Me.btnTransporte.Name = "btnTransporte"
        Me.btnTransporte.Size = New System.Drawing.Size(166, 39)
        Me.btnTransporte.TabIndex = 4
        Me.btnTransporte.Text = "Listado Transportes"
        Me.btnTransporte.UseVisualStyleBackColor = True
        '
        'btrnBasculas
        '
        Me.btrnBasculas.Location = New System.Drawing.Point(17, 232)
        Me.btrnBasculas.Name = "btrnBasculas"
        Me.btrnBasculas.Size = New System.Drawing.Size(166, 39)
        Me.btrnBasculas.TabIndex = 5
        Me.btrnBasculas.Text = "Listado Basculas"
        Me.btrnBasculas.UseVisualStyleBackColor = True
        '
        'btnUsuarios
        '
        Me.btnUsuarios.Location = New System.Drawing.Point(17, 325)
        Me.btnUsuarios.Name = "btnUsuarios"
        Me.btnUsuarios.Size = New System.Drawing.Size(166, 39)
        Me.btnUsuarios.TabIndex = 6
        Me.btnUsuarios.Text = "Listado Usuarios"
        Me.btnUsuarios.UseVisualStyleBackColor = True
        '
        'btnSalir
        '
        Me.btnSalir.Location = New System.Drawing.Point(17, 370)
        Me.btnSalir.Name = "btnSalir"
        Me.btnSalir.Size = New System.Drawing.Size(166, 39)
        Me.btnSalir.TabIndex = 7
        Me.btnSalir.Text = "Salir"
        Me.btnSalir.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnBuques)
        Me.GroupBox1.Controls.Add(Me.btnSalir)
        Me.GroupBox1.Controls.Add(Me.btnClientes)
        Me.GroupBox1.Controls.Add(Me.btnUsuarios)
        Me.GroupBox1.Controls.Add(Me.btnProductos)
        Me.GroupBox1.Controls.Add(Me.btrnBasculas)
        Me.GroupBox1.Controls.Add(Me.btnCamiones)
        Me.GroupBox1.Controls.Add(Me.btnTransporte)
        Me.GroupBox1.Location = New System.Drawing.Point(25, 52)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(200, 435)
        Me.GroupBox1.TabIndex = 8
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Listados de Reportes"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Navy
        Me.Label1.Location = New System.Drawing.Point(45, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(163, 20)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "Listados Catálogos"
        '
        'RepListadosCatalogos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(258, 514)
        Me.ControlBox = False
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "RepListadosCatalogos"
        Me.Text = "RepListadosCatalogos"
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnBuques As System.Windows.Forms.Button
    Friend WithEvents btnClientes As System.Windows.Forms.Button
    Friend WithEvents btnProductos As System.Windows.Forms.Button
    Friend WithEvents btnCamiones As System.Windows.Forms.Button
    Friend WithEvents btnTransporte As System.Windows.Forms.Button
    Friend WithEvents btrnBasculas As System.Windows.Forms.Button
    Friend WithEvents btnUsuarios As System.Windows.Forms.Button
    Friend WithEvents btnSalir As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
