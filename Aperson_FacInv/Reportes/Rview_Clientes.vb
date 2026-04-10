Public Class Rview_Clientes

    Private Sub Rview_Clientes_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TODO: This line of code loads data into the 'DataSet2.Cliente' table. You can move, or remove it, as needed.
        Me.ClienteTableAdapter.Fill(Me.DataSet2.Cliente)

        Me.ReportViewer1.Dock = DockStyle.Fill

        Me.ReportViewer1.RefreshReport()
    End Sub
End Class