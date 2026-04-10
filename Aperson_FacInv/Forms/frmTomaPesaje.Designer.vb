<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTomaPesaje
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
        Me.txtTomaPeso = New System.Windows.Forms.TextBox()
        Me.btnAceptar = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.btnVolveraPesar = New System.Windows.Forms.Button()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'txtTomaPeso
        '
        Me.txtTomaPeso.BackColor = System.Drawing.SystemColors.Window
        Me.txtTomaPeso.Enabled = False
        Me.txtTomaPeso.Font = New System.Drawing.Font("Copperplate Gothic Bold", 27.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTomaPeso.ForeColor = System.Drawing.Color.ForestGreen
        Me.txtTomaPeso.Location = New System.Drawing.Point(198, 42)
        Me.txtTomaPeso.Name = "txtTomaPeso"
        Me.txtTomaPeso.Size = New System.Drawing.Size(302, 49)
        Me.txtTomaPeso.TabIndex = 0
        Me.txtTomaPeso.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnAceptar
        '
        Me.btnAceptar.BackColor = System.Drawing.Color.LightYellow
        Me.btnAceptar.Location = New System.Drawing.Point(255, 119)
        Me.btnAceptar.Name = "btnAceptar"
        Me.btnAceptar.Size = New System.Drawing.Size(94, 32)
        Me.btnAceptar.TabIndex = 1
        Me.btnAceptar.Text = "Aceptar"
        Me.btnAceptar.UseVisualStyleBackColor = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.AperBascu.My.Resources.Resources.weighing
        Me.PictureBox1.Location = New System.Drawing.Point(12, 16)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(144, 135)
        Me.PictureBox1.TabIndex = 2
        Me.PictureBox1.TabStop = False
        '
        'btnVolveraPesar
        '
        Me.btnVolveraPesar.BackColor = System.Drawing.Color.LightCoral
        Me.btnVolveraPesar.Location = New System.Drawing.Point(366, 119)
        Me.btnVolveraPesar.Name = "btnVolveraPesar"
        Me.btnVolveraPesar.Size = New System.Drawing.Size(94, 32)
        Me.btnVolveraPesar.TabIndex = 3
        Me.btnVolveraPesar.Text = "Cancelar"
        Me.btnVolveraPesar.UseVisualStyleBackColor = False
        '
        'frmTomaPesaje
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Gainsboro
        Me.ClientSize = New System.Drawing.Size(525, 163)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnVolveraPesar)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.btnAceptar)
        Me.Controls.Add(Me.txtTomaPeso)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "frmTomaPesaje"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Toma de PESO"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtTomaPeso As System.Windows.Forms.TextBox
    Friend WithEvents btnAceptar As System.Windows.Forms.Button
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents btnVolveraPesar As System.Windows.Forms.Button
End Class
