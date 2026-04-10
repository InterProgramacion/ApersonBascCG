Public Class frmCalbracion

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

    Private Sub frmCalbracion_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.BackColor = Color.White
        Label4.ForeColor = Color.SeaGreen
    End Sub

#End Region

    '#Region "Entorno"

    '    Private Sub frmCalbracion_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '        Consulta("")
    '        Botonos(True)

    '    End Sub

    '    'Sub Modulo Botonos
    '    'Sirver para Muestra o Oculta los Botonos
    '    Private Sub Botonos(ByVal Habilita As Boolean)
    '        '-Botonos
    '        btnNuevo.Visible = Habilita
    '        btnBuscar.Enabled = Habilita
    '        btnSalir.visible = Habilita
    '        btnprimero.visible = Habilita
    '        btnanterior.visible = Habilita
    '        btnsiguiente.visible = Habilita
    '        btnultimo.visible = Habilita

    '        If Me.Grabando Then
    '            btnBorrar.text = "cancelar"
    '            btnGrabar.text = "grabar"
    '        Else
    '            btnGrabar.text = "actualizar"
    '            btnBorrar.text = "borrar"
    '        End If

    '        '-Campos
    '        txtId.Enabled = False
    '    End Sub

    '    Private Sub Textos()
    '        txtId.Text = ""
    '        txtId.Enabled = False
    '        txtUsuario.Text = ""
    '        txtNombre.Text = ""
    '        cmbPerfil.Text = "USUARIO"
    '        txtContraseña.Text = ""
    '        txtCfContraseña.Text = ""
    '        cmbEstado.Text = "ACTIVO"

    '        'Se posiciona para Iniciar 
    '        txtUsuario.Focus()
    '    End Sub

    '#End Region

    '#Region "Consulta"
    '    'Procedimiento para mostrar el Primer Registro
    '    Private Sub Consulta(ByVal _codigo As String)
    '        Try
    '            'Para mostrar el Dato
    '            If _codigo = String.Empty Then
    '                dTable = Datos.consulta_reader("SELECT TOP 1 * FROM dbo.Usuario ORDER BY Id_buq")
    '            Else
    '                dTable = Datos.consulta_reader("SELECT * FROM dbo.Usuario WHERE Id_buq=" & _codigo)
    '            End If

    '            Dim nCod As String = ""

    '            'Setea Campos
    '            For Each dRow As DataRow In dTable.Rows
    '                txtId.Text = dRow.Item("Id_buq").ToString
    '                txtUsuario.Text = dRow.Item("Log_usu").ToString
    '                txtNombre.Text = dRow.Item("nombre_usu").ToString
    '                cmbPerfil.Text = dRow.Item("Pefil_usu").ToString
    '                txtContraseña.Text = dRow.Item("Contraseña_usu").ToString.Trim
    '                txtCfContraseña.Text = dRow.Item("Contraseña_usu").ToString.Trim
    '                cmbEstado.Text = dRow.Item("baja_usu").ToString
    '            Next

    '            'Asigna Linea Actual
    '            Me.LineaActual = dTable.Rows.Item(0).Item("Id_buq")

    '            Me.Grabando = False
    '            Me.NuevoReg = False
    '            Navega()

    '        Catch ex As Exception
    '            MessageBox.Show("ERROR: " & ex.Message, "Comprobar tabla * Usuario")
    '        End Try
    '    End Sub

    '    'Para Hacer Navegar
    '    Private Sub Navega()
    '        'Contar Lineas
    '        dTable = Datos.consulta_reader("SELECT Count(1) As TotRec FROM dbo.Usuario")
    '        Me.TotLineas = dTable.Rows.Item(0).Item("TotRec")

    '        'Primera Linea
    '        dTable = Datos.consulta_reader("SELECT TOP 1 Id_buq FROM dbo.Usuario ORDER BY Id_buq")
    '        Me.PrimLinea = dTable.Rows.Item(0).Item("Id_buq")

    '        'Ultima Linea
    '        dTable = Datos.consulta_reader("SELECT TOP 1 Id_buq FROM dbo.Usuario ORDER BY Id_buq DESC")
    '        Me.UltiLinea = dTable.Rows.Item(0).Item("Id_buq")

    '        'Verifica si hay registros
    '        If Me.TotLineas = 0 Then
    '            Me.btnprimero.Enabled = False
    '            Me.btnanterior.Enabled = False
    '            Me.btnsiguiente.Enabled = False
    '            Me.btnultimo.Enabled = False
    '        End If

    '        'Para Control de Botones Navegar
    '        If Me.PrimLinea = Me.LineaActual Then
    '            btnanterior.Enabled = False
    '            btnprimero.Enabled = False
    '        Else
    '            btnanterior.Enabled = True
    '            btnprimero.Enabled = True
    '        End If

    '        If Me.UltiLinea = Me.LineaActual Then
    '            btnsiguiente.Enabled = False
    '            btnultimo.Enabled = False
    '        Else
    '            btnsiguiente.Enabled = True
    '            btnultimo.Enabled = True
    '        End If
    '    End Sub
    '#End Region

    '#Region "Grabar"
    '    Private Sub btnGrabar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGrabar.Click

    '        If ValidaCampos() Then 'Si es verdadero
    '            If NuevoReg = True Then
    '                'Se arma el script para grabar SQL
    '                Dim StringSql As String = <sqlExp>
    '                                    INSERT INTO [dbo].[Usuario] ([Log_usu],
    '                                                                [nombre_usu],
    '                                                                [Pefil_usu],
    '                                                                [Contraseña_usu],
    '                                                                [baja_usu])
    '                                        VALUES('<%= txtUsuario.Text %>',
    '                                              '<%= txtNombre.Text %>',
    '                                              '<%= cmbPerfil.Text %>',
    '                                              '<%= txtContraseña.Text %>',
    '                                              '<%= cmbEstado.Text %>')
    '                                     </sqlExp>.Value

    '                'LLamamos la rutina para grabar
    '                Datos.consulta_non_query(StringSql)

    '            Else
    '                'Se arma el script para grabar SQL
    '                Dim StringSql As String = <sqlExp>
    '                                     UPDATE [dbo].[Usuario] SET [Log_usu] =  '<%= txtUsuario.Text %>',
    '                                                               [nombre_usu] = '<%= txtNombre.Text %>',
    '                                                               [Pefil_usu] =  '<%= cmbPerfil.Text %>',
    '                                                               [Contraseña_usu] = '<%= txtContraseña.Text %>',
    '                                                               [baja_usu] = '<%= cmbEstado.Text %>'
    '                                      WHERE Id_buq = '<%= txtId.Text %>' 
    '                                  </sqlExp>.Value

    '                'LLamamos la rutina para grabar
    '                Datos.consulta_non_query(StringSql)
    '            End If

    '            'Refrescamos los dato de pantalla
    '            Consulta("")
    '            Botonos(True)
    '            'txtDescripcion.Focus()
    '        End If
    '    End Sub

    '    Private Function ValidaCampos() As Boolean
    '        Dim Si As Boolean = False

    '        If txtUsuario.Text = String.Empty Then
    '            MessageBox.Show("ERROR: Peso Minimo no puede quedar en Blanco")
    '            txtUsuario.Focus()
    '        ElseIf txtNombre.Text = String.Empty Then
    '            MessageBox.Show("ERROR: Peso Maximo no puede quedar en Blanco")
    '            txtNombre.Focus()
    '        ElseIf txtCfContraseña.Text = String.Empty Then
    '            MessageBox.Show("ERROR: Confirmación Contraseña no puede quedar en Blanco")
    '            txtCfContraseña.Focus()
    '        ElseIf txtContraseña.Text = String.Empty Then
    '            MessageBox.Show("ERROR: Contraseña no puede quedar en Blanco")
    '            txtContraseña.Focus()
    '        ElseIf txtContraseña.Text.Trim <> txtCfContraseña.Text.Trim Then
    '            MessageBox.Show("ERROR: Verificar Contraseña")
    '            txtCfContraseña.Focus()
    '        Else
    '            Si = True
    '        End If
    '        Return Si
    '    End Function
    '#End Region

    '#Region "Botones"
    '    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
    '        Me.Close()
    '    End Sub

    '    Private Sub btnNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuevo.Click
    '        Me.NuevoReg = True
    '        Me.Grabando = True
    '        Botonos(False)
    '        Textos()
    '        cmbEstado.Enabled = False
    '    End Sub

    '    Private Sub btnBorrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBorrar.Click
    '        'Si es modo nuevo o Editar
    '        If Me.Grabando Then
    '            Consulta("")
    '            Botonos(True)
    '        Else
    '            'LLamamos la rutina Eliminar
    '            Datos.consulta_non_query("DELETE FROM [dbo].[Usuario] WHERE Id_buq='" & txtId.Text & "'")

    '            'Refrescamos los dato de pantalla
    '            Consulta("")
    '            Botonos(True)
    '        End If
    '        'txtDescripcion.Focus()
    '    End Sub

    '    'Despliega pantalla para buscar el Cajero
    '    Private Sub BuscaCodigo()
    '        Dim stringSQL As String

    '        stringSQL = "SELECT Id_buq As Codigo, pesotaramin_pes As Nombre, pesotaramax_pes FROM [dbo].[Usuario]"

    '        'Script para Filtrar el Grid
    '        Dim stringSqlFiltro As String = "SELECT Id_buq As Codigo, pesotaramin_pes As Nombre FROM [dbo].[Usuario] WHERE Id_buq "

    '        'Instancia Ayuda Cliente
    '        'Dim frmBuscar As New BuscaCodigo(stringSQL, stringSqlFiltro, "Clientes")
    '        'frmBuscar.ShowDialog()

    '        Dim frmBuscar As New BuscaCodigo(stringSQL, {"Código", "Peso Tara Minimo", "Peso Tara Maximo", "50", "100", "100"}, 3, stringSqlFiltro, "Peso Maximo y Minimos")
    '        frmBuscar.ShowDialog()

    '        'Retorna los datos del Cajero selecionado y 
    '        'se posiciona en Serie Recibo
    '        txtId.Text = ""
    '        If frmBuscar.Codigo <> String.Empty Then
    '            Consulta(frmBuscar.Codigo)
    '        End If
    '    End Sub

    '    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
    '        BuscaCodigo()
    '    End Sub

    '#End Region

    '#Region "Botones Navegar"
    '    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnprimero.Click
    '        'Para Inicio
    '        dTable = Datos.consulta_reader("SELECT TOP 1 Id_buq FROM dbo.Usuario ORDER BY Id_buq")

    '        For Each dRow As DataRow In dTable.Rows
    '            Me.LineaActual = dRow("Id_buq")

    '            Consulta(dRow("Id_buq").ToString)
    '        Next
    '    End Sub

    '    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnanterior.Click
    '        'Para Anterior
    '        dTable = Datos.consulta_reader("SELECT TOP 1 Id_buq FROM dbo.Usuario WHERE Id_buq <" & Me.LineaActual & "ORDER BY Id_buq DESC")

    '        For Each dRow As DataRow In dTable.Rows
    '            Me.LineaActual = dRow("Id_buq")

    '            Consulta(dRow("Id_buq").ToString)
    '        Next
    '    End Sub

    '    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnsiguiente.Click
    '        'Para Sigiente
    '        dTable = Datos.consulta_reader("SELECT TOP 1 Id_buq FROM dbo.Usuario WHERE Id_buq >" & Me.LineaActual & "ORDER BY Id_buq")

    '        For Each dRow As DataRow In dTable.Rows
    '            Me.LineaActual = dRow("Id_buq")

    '            Consulta(dRow("Id_buq").ToString)
    '        Next
    '    End Sub

    '    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnultimo.Click
    '        'Para Inicio
    '        dTable = Datos.consulta_reader("SELECT TOP 1 Id_buq FROM dbo.Usuario ORDER BY Id_buq DESC")

    '        For Each dRow As DataRow In dTable.Rows
    '            Me.UltiLinea = dRow("Id_buq")
    '            Me.LineaActual = dRow("Id_buq")

    '            Consulta(dRow("Id_buq").ToString)
    '        Next

    '    End Sub
    '#End Region


End Class