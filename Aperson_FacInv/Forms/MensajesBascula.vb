Public Class MensajesBascula

    'Public Shared meSaLabel As String = "Este Mensaje"
    Public Shared Property meSaLabel As String = ""


    Private Sub MensajesBascula_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.mlabel.Text = LibreriaGeneral.xMensaje
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub
End Class