Public Class PrueReporte

    Private Sub PrueReporte_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TODO: This line of code loads data into the 'DataSet1.buques' table. You can move, or remove it, as needed.
        Me.buquesTableAdapter.Fill(Me.DataSet1.buques)
        Me.ReportViewer1.Dock = DockStyle.Fill

        Me.ReportViewer1.RefreshReport()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub


End Class