Public Class Rview_Usuarios

    Private Sub Rview_Usuarios_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TODO: This line of code loads data into the 'DataSet6.usuario' table. You can move, or remove it, as needed.
        Me.usuarioTableAdapter.Fill(Me.DataSet6.usuario)

        Me.ReportViewer1.Dock = DockStyle.Fill

        Me.ReportViewer1.RefreshReport()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub
End Class