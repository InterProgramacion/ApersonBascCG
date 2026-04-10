Public Class Login

#Region "Variables"
    Dim Datos As New DatosGenereral()
    Dim dTable As New DataTable
    Dim dTable2 As New DataTable
    Dim Empresa As New DataTable

    Private _conectado As Boolean = False
    Private _DataCone As String = ""

    Private _userValidated As Boolean = False
    Private id_usu As Integer

    Public ReadOnly Property Conectado() As Boolean
        Get
            Return _conectado
        End Get
    End Property

    Public ReadOnly Property DataConecta() As String
        Get
            Return _DataCone
        End Get
    End Property
#End Region

    Private Sub txtContrasena_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtContrasena.Validating
        If sender.Text <> String.Empty Then
            ValidaClave()
        End If
    End Sub

    Private Sub btn_Cancelar_Click(sender As Object, e As EventArgs) Handles btn_Cancelar.Click
        Me.Close()
    End Sub

    Private Sub Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CargarEmpresas()
    End Sub

#Region "Saltos"
    Private Sub txtContrasena_KeyPress(ByVal sender As Object, _
                                 ByVal e As System.Windows.Forms.KeyPressEventArgs) _
                                  Handles txtContrasena.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then
            ValidaClave()
        End If
    End Sub

    Private Sub txtUsuario_KeyPress(ByVal sender As Object, _
                                 ByVal e As System.Windows.Forms.KeyPressEventArgs) _
                                  Handles txtUsuario.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then
            txtContrasena.Focus()
        End If
    End Sub
#End Region


    Private Sub ValidaClave()
        If Me.ActiveControl Is cmbEmpresa Then Exit Sub

        If cmbEmpresa.SelectedValue Is Nothing OrElse IsDBNull(cmbEmpresa.SelectedValue) _
            OrElse CInt(cmbEmpresa.SelectedValue) <= 0 Then
            MessageBox.Show("Selecciona una empresa.")
            cmbEmpresa.Focus()
            Exit Sub
        End If


        LibreriaGeneral.gNombreEmpre = CInt(cmbEmpresa.SelectedValue)


        dTable = Datos.consulta_reader("SELECT * FROM dbo.Usuario WHERE RTRIM(LTRIM(Contraseña_usu))= '" & txtContrasena.Text & "' AND RTRIM(LTRIM(Log_usu))='" & txtUsuario.Text & "'") ''Modificado por ML 1990
        dTable2 = Datos.consulta_reader("SELECT * FROM asigbascpc where nombrePC_asi = '" & System.Windows.Forms.SystemInformation.ComputerName & "'")

        If dTable.Rows.Count = 0 Then
            MessageBox.Show("Contraseña Incorrecta, por favor intente de Nuevo !!!!")
            txtContrasena.Focus()
        Else

            'Setea Campos
            For Each dRow As DataRow In dTable.Rows
                LibreriaGeneral.usuario = dRow.Item("Log_usu").ToString
                LibreriaGeneral.pusuario = dRow.Item("Pefil_usu").ToString.Trim
                id_usu = dRow.Item("id_usu")

            Next

            LibreriaGeneral.gEmpresa = CInt(cmbEmpresa.SelectedValue)
            LibreriaGeneral.idEmpresa = CInt(cmbEmpresa.SelectedValue)
            LibreriaGeneral.gNombreEmpre = cmbEmpresa.Text
            Try

                dTable2 = Datos.consulta_reader("SELECT * FROM Empresa WHERE id_emp='" & LibreriaGeneral.gEmpresa & "'")

                If dTable2.Rows.Count = 0 Then
                    MessageBox.Show("❌ La Empresa no esta Registrada!!!!")
                    Me.Close()
                Else
                    'Setea Campos
                    For Each dRow As DataRow In dTable2.Rows
                        LibreriaGeneral.nBodega = dRow.Item("id_bod").ToString

                    Next
                End If
            Catch ex As Exception

            End Try

            _conectado = True
            Me.Close()
        End If

    End Sub

    Private Sub btn_Aceptar_Click(sender As Object, e As EventArgs) Handles btn_Aceptar.Click
        If Not _userValidated Then
            ValidaClave()
        Else


            _conectado = True
            Me.Close()
        End If
    End Sub


    Private Sub txtUsuario_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtUsuario.Validating
        txtUsuario.Text = txtUsuario.Text.ToUpper
        txtUsuario.Refresh()
    End Sub

    Private Sub CargarEmpresas()
        Try
            Dim dtbEmpresa As DataTable = Datos.consulta_reader("SELECT Id_Emp, nombre_emp FROM dbo.Empresa ORDER BY nombre_emp")

            Dim ph As DataRow = dtbEmpresa.NewRow()
            ph("Id_Emp") = -1
            ph("nombre_emp") = "— Seleccione empresa —"
            dtbEmpresa.Rows.InsertAt(ph, 0)


            cmbEmpresa.DataSource = dtbEmpresa
            cmbEmpresa.DisplayMember = "nombre_emp"
            cmbEmpresa.ValueMember = "Id_Emp"
            cmbEmpresa.SelectedIndex = 0

        Catch ex As Exception
            MessageBox.Show("No se pudieron cargar las empresas. " &
                            "Verifique la conexión.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
            MessageBox.Show(ex.Message)
        End Try
    End Sub
End Class