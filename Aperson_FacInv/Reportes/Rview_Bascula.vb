Public Class Rview_Bascula

    Private Sub Rview_Bascula_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TODO: This line of code loads data into the 'DataSet5.basculas' table. You can move, or remove it, as needed.
        Me.basculasTableAdapter.Fill(Me.DataSet5.basculas)

        Me.ReportViewer1.Dock = DockStyle.Fill

        Me.ReportViewer1.RefreshReport()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub
End Class