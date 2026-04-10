Public Class frmUsuario


#Region "Variables"
    Dim Datos As New DatosGenereral()
    Dim NuevoReg As Boolean
    Dim Grabando As Boolean
    Dim dTable As New DataTable
    Dim TotLineas As Integer
    Dim PrimLinea As Integer
    Dim UltiLinea As Integer
    Dim LineaActual As Integer
    Dim Empresa As String = LibreriaGeneral.nEmpresa
#End Region

#Region "Entorno"

    Private Sub frmUsuario_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Consulta("")
        Botonos(True)

        Me.BackColor = Color.White
        Label4.ForeColor = Color.SeaGreen
    End Sub

    'Sub Modulo Botonos
    'Sirver para Muestra o Oculta los Botonos
    Private Sub Botonos(ByVal Habilita As Boolean)
        '-Botonos
        btnNuevo.Visible = Habilita
        btnBuscar.Enabled = Habilita
        btnSalir.visible = Habilita
        btnprimero.visible = Habilita
        btnanterior.visible = Habilita
        btnsiguiente.visible = Habilita
        btnultimo.visible = Habilita

        If Me.Grabando Then
            btnBorrar.text = "cancelar"
            btnGrabar.text = "grabar"
        Else
            btnGrabar.text = "actualizar"
            btnBorrar.text = "borrar"
        End If

        '-Campos
        txtId.Enabled = False

        '-Colores
        btnNuevo.BackColor = Color.SeaGreen
        btnBuscar.BackColor = Color.SeaGreen
        btnSalir.BackColor = Color.SeaGreen
        btnGrabar.BackColor = Color.SeaGreen
        btnBorrar.BackColor = Color.SeaGreen
    End Sub

    Private Sub Textos()
        txtId.Text = ""
        txtId.Enabled = False
        txtUsuario.Text = ""
        txtNombre.Text = ""
        cmbPerfil.Text = "USUARIO"
        txtContraseña.Text = ""
        txtCfContraseña.Text = ""
        cmbEstado.Text = "ACTIVO"

        'Se posiciona para Iniciar 
        txtUsuario.Focus()
    End Sub

#End Region

#Region "Consulta"
    'Procedimiento para mostrar el Primer Registro
    Private Sub Consulta(ByVal _codigo As String)
        Try
            'Para mostrar el Dato
            If _codigo = String.Empty Then
                dTable = Datos.consulta_reader("SELECT TOP 1 * FROM dbo.Usuario ORDER BY Id_usu")
            Else
                dTable = Datos.consulta_reader("SELECT * FROM dbo.Usuario WHERE Id_usu=" & _codigo)
            End If

            Dim nCod As String = ""

            'Setea Campos
            For Each dRow As DataRow In dTable.Rows
                txtId.Text = dRow.Item("Id_usu").ToString
                txtUsuario.Text = dRow.Item("Log_usu").ToString
                txtNombre.Text = dRow.Item("nombre_usu").ToString
                cmbPerfil.Text = dRow.Item("Pefil_usu").ToString
                txtContraseña.Text = dRow.Item("Contraseña_usu").ToString.Trim
                txtCfContraseña.Text = dRow.Item("Contraseña_usu").ToString.Trim
                cmbEstado.Text = dRow.Item("baja_usu").ToString
            Next

            'Asigna Linea Actual
            Me.LineaActual = dTable.Rows.Item(0).Item("Id_usu")

            Me.Grabando = False
            Me.NuevoReg = False
            Navega()

        Catch ex As Exception
            MessageBox.Show("ERROR: " & ex.Message, "Comprobar tabla * Usuario")
        End Try
    End Sub

    'Para Hacer Navegar
    Private Sub Navega()
        'Contar Lineas
        dTable = Datos.consulta_reader("SELECT Count(1) As TotRec FROM dbo.Usuario")
        Me.TotLineas = dTable.Rows.Item(0).Item("TotRec")

        'Primera Linea
        dTable = Datos.consulta_reader("SELECT TOP 1 Id_usu FROM dbo.Usuario ORDER BY Id_usu")
        Me.PrimLinea = dTable.Rows.Item(0).Item("Id_usu")

        'Ultima Linea
        dTable = Datos.consulta_reader("SELECT TOP 1 Id_usu FROM dbo.Usuario ORDER BY Id_usu DESC")
        Me.UltiLinea = dTable.Rows.Item(0).Item("Id_usu")

        'Verifica si hay registros
        If Me.TotLineas = 0 Then
            Me.btnprimero.Enabled = False
            Me.btnanterior.Enabled = False
            Me.btnsiguiente.Enabled = False
            Me.btnultimo.Enabled = False
        End If

        'Para Control de Botones Navegar
        If Me.PrimLinea = Me.LineaActual Then
            btnanterior.Enabled = False
            btnprimero.Enabled = False
        Else
            btnanterior.Enabled = True
            btnprimero.Enabled = True
        End If

        If Me.UltiLinea = Me.LineaActual Then
            btnsiguiente.Enabled = False
            btnultimo.Enabled = False
        Else
            btnsiguiente.Enabled = True
            btnultimo.Enabled = True
        End If
    End Sub
#End Region

#Region "Grabar"
    Private Sub btnGrabar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGrabar.Click

        If ValidaCampos() Then 'Si es verdadero
            If NuevoReg = True Then

                'Suma Correlativo
                dTable = Datos.consulta_reader("SELECT MAX(Id_usu)+1 rMax FROM [dbo].[Usuario]")

                If dTable.Rows.Count > 0 Then
                    'Setea Campos
                    For Each dRow As DataRow In dTable.Rows
                        'Infomrativos
                        txtId.Text = dRow.Item("rMax").ToString
                    Next
                Else
                    txtId.Text = "1"
                End If



                'Se arma el script para grabar SQL
                Dim StringSql As String = <sqlExp>
                                    INSERT INTO [dbo].[Usuario] ([id_usu], [Log_usu],
                                                                [nombre_usu],
                                                                [Pefil_usu],
                                                                [Contraseña_usu],
                                                                [baja_usu])
                                        VALUES('<%= txtId.Text %>', '<%= txtUsuario.Text %>',
                                              '<%= txtNombre.Text %>',
                                              '<%= cmbPerfil.Text %>',
                                              '<%= txtContraseña.Text %>',
                                              '<%= cmbEstado.Text %>')
                                     </sqlExp>.Value

                'LLamamos la rutina para grabar
                Datos.consulta_non_query(StringSql)

            Else
                'Se arma el script para grabar SQL
                Dim StringSql As String = <sqlExp>
                                     UPDATE [dbo].[Usuario] SET [Log_usu] =  '<%= txtUsuario.Text %>',
                                                               [nombre_usu] = '<%= txtNombre.Text %>',
                                                               [Pefil_usu] =  '<%= cmbPerfil.Text %>',
                                                               [Contraseña_usu] = '<%= txtContraseña.Text %>',
                                                               [baja_usu] = '<%= cmbEstado.Text %>'
                                      WHERE Id_usu = '<%= txtId.Text %>' 
                                  </sqlExp>.Value

                'LLamamos la rutina para grabar
                Datos.consulta_non_query(StringSql)
            End If

            'Refrescamos los dato de pantalla
            Consulta("")
            Botonos(True)
            'txtDescripcion.Focus()
        End If
    End Sub

    Private Function ValidaCampos() As Boolean
        Dim Si As Boolean = False

        If txtUsuario.Text = String.Empty Then
            MessageBox.Show("ERROR: Peso Minimo no puede quedar en Blanco")
            txtUsuario.Focus()
        ElseIf txtNombre.Text = String.Empty Then
            MessageBox.Show("ERROR: Peso Maximo no puede quedar en Blanco")
            txtNombre.Focus()
        ElseIf txtCfContraseña.Text = String.Empty Then
            MessageBox.Show("ERROR: Confirmación Contraseña no puede quedar en Blanco")
            txtCfContraseña.Focus()
        ElseIf txtContraseña.Text = String.Empty Then
            MessageBox.Show("ERROR: Contraseña no puede quedar en Blanco")
            txtContraseña.Focus()
        ElseIf txtContraseña.Text.Trim <> txtCfContraseña.Text.Trim Then
            MessageBox.Show("ERROR: Verificar Contraseña")
            txtCfContraseña.Focus()
        Else
            Si = True
        End If
        Return Si
    End Function
#End Region

#Region "Botones"
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub btnNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuevo.Click
        Me.NuevoReg = True
        Me.Grabando = True
        Botonos(False)
        Textos()
        cmbEstado.Enabled = False
    End Sub

    Private Sub btnBorrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBorrar.Click
        'Si es modo nuevo o Editar
        If Me.Grabando Then
            Consulta("")
            Botonos(True)
        Else
            Datos.consulta_non_queryDeta("DELETE FROM dbo.usuarioAccesos WHERE Log_usu ='" & txtId.Text & "'")
            'LLamamos la rutina Eliminar
            Datos.consulta_non_query("DELETE FROM [dbo].[Usuario] WHERE Id_usu='" & txtId.Text & "'")

            'Refrescamos los dato de pantalla
            Consulta("")
            Botonos(True)
        End If
        'txtDescripcion.Focus()
    End Sub

    'Despliega pantalla para buscar el Cajero
    Private Sub BuscaCodigo()
        Dim stringSQL As String

        stringSQL = "SELECT Id_usu As Codigo, pesotaramin_pes As Nombre, pesotaramax_pes FROM [dbo].[Usuario]"

        'Script para Filtrar el Grid
        Dim stringSqlFiltro As String = "SELECT Id_usu As Codigo, pesotaramin_pes As Nombre FROM [dbo].[Usuario] WHERE Id_usu "

        'Instancia Ayuda Cliente
        'Dim frmBuscar As New BuscaCodigo(stringSQL, stringSqlFiltro, "Clientes")
        'frmBuscar.ShowDialog()

        Dim frmBuscar As New BuscaCodigo(stringSQL, {"Código", "Peso Tara Minimo", "Peso Tara Maximo", "50", "100", "100"}, 3, stringSqlFiltro, "Peso Maximo y Minimos")
        frmBuscar.ShowDialog()

        'Retorna los datos del Cajero selecionado y 
        'se posiciona en Serie Recibo
        txtId.Text = ""
        If frmBuscar.Codigo <> String.Empty Then
            Consulta(frmBuscar.Codigo)
        End If
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        BuscaCodigo()
    End Sub

#End Region

#Region "Botones Navegar"
    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnprimero.Click
        'Para Inicio
        dTable = Datos.consulta_reader("SELECT TOP 1 Id_usu FROM dbo.Usuario ORDER BY Id_usu")

        For Each dRow As DataRow In dTable.Rows
            Me.LineaActual = dRow("Id_usu")

            Consulta(dRow("Id_usu").ToString)
        Next
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnanterior.Click
        'Para Anterior
        dTable = Datos.consulta_reader("SELECT TOP 1 Id_usu FROM dbo.Usuario WHERE Id_usu <" & Me.LineaActual & "ORDER BY Id_usu DESC")

        For Each dRow As DataRow In dTable.Rows
            Me.LineaActual = dRow("Id_usu")

            Consulta(dRow("Id_usu").ToString)
        Next
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnsiguiente.Click
        'Para Sigiente
        dTable = Datos.consulta_reader("SELECT TOP 1 Id_usu FROM dbo.Usuario WHERE Id_usu >" & Me.LineaActual & "ORDER BY Id_usu")

        For Each dRow As DataRow In dTable.Rows
            Me.LineaActual = dRow("Id_usu")

            Consulta(dRow("Id_usu").ToString)
        Next
    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnultimo.Click
        'Para Inicio
        dTable = Datos.consulta_reader("SELECT TOP 1 Id_usu FROM dbo.Usuario ORDER BY Id_usu DESC")

        For Each dRow As DataRow In dTable.Rows
            Me.UltiLinea = dRow("Id_usu")
            Me.LineaActual = dRow("Id_usu")

            Consulta(dRow("Id_usu").ToString)
        Next

    End Sub
#End Region


    'Pantalla para Accesos
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnAcceso.Click
        Dim frms As New frmUsuarioAccesos(txtUsuario.Text)
        frms.ShowDialog()
    End Sub
End Class