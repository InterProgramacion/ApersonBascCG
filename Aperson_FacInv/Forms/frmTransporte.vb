Public Class frmTransporte

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

    Private Sub frmTransporte_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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
        txtNombre.Text = ""
        cmbEstatus.Text = "ACTIVO"

        'Se posiciona para Iniciar 
        txtNombre.Focus()
    End Sub

#End Region

#Region "Consulta"
    'Procedimiento para mostrar el Primer Registro
    Private Sub Consulta(ByVal _codigo As String)
        Try
            'Para mostrar el Dato
            If _codigo = String.Empty Then
                dTable = Datos.consulta_reader("SELECT TOP 1 * FROM dbo.Transporte ORDER BY id_tpt")
            Else
                dTable = Datos.consulta_reader("SELECT * FROM dbo.Transporte WHERE id_tpt=" & _codigo)
            End If

            Dim nCod As String = ""

            'Setea Campos
            For Each dRow As DataRow In dTable.Rows
                txtId.Text = dRow.Item("id_tpt").ToString
                txtNombre.Text = dRow.Item("nombre_tpt").ToString
                cmbEstatus.Text = dRow.Item("baja_tpt").ToString
            Next

            'Asigna Linea Actual
            Me.LineaActual = dTable.Rows.Item(0).Item("id_tpt")

            Me.Grabando = False
            Me.NuevoReg = False
            Navega()

        Catch ex As Exception
            MessageBox.Show("ERROR: " & ex.Message, "Comprobar tabla * Transporte")
        End Try
    End Sub

    'Para Hacer Navegar
    Private Sub Navega()
        'Contar Lineas
        dTable = Datos.consulta_reader("SELECT Count(1) As TotRec FROM dbo.Transporte")
        Me.TotLineas = dTable.Rows.Item(0).Item("TotRec")

        'Primera Linea
        dTable = Datos.consulta_reader("SELECT TOP 1 id_tpt FROM dbo.Transporte ORDER BY id_tpt")
        Me.PrimLinea = dTable.Rows.Item(0).Item("id_tpt")

        'Ultima Linea
        dTable = Datos.consulta_reader("SELECT TOP 1 id_tpt FROM dbo.Transporte ORDER BY id_tpt DESC")
        Me.UltiLinea = dTable.Rows.Item(0).Item("id_tpt")

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
                dTable = Datos.consulta_reader("SELECT MAX(id_tpt)+1 rMax FROM [dbo].[Transporte]")

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
                                    INSERT INTO [dbo].[Transporte] ([id_tpt], [nombre_tpt],
                                                                  [baja_tpt],[actualiza_tpt])
                                                     VALUES('<%= txtId.Text %>', '<%= txtNombre.Text %>',
                                                          '<%= cmbEstatus.Text %>',1)
                                     </sqlExp>.Value

                'LLamamos la rutina para grabar
                Datos.consulta_non_query(StringSql)

            Else
                'Se arma el script para grabar SQL
                Dim StringSql As String = <sqlExp>
                                     UPDATE [dbo].[Transporte] SET [nombre_tpt] = '<%= txtNombre.Text %>',
                                                                  [baja_tpt]  = '<%= cmbEstatus.Text %>',
                                                                  actualiza_tpt = 2
                                      WHERE id_tpt = '<%= txtId.Text %>' 
                                      AND actualiza_tpt IN(1,2)
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

        If txtNombre.Text = String.Empty Then
            MessageBox.Show("ERROR: El Nombre no puede ser Blanco")
            txtNombre.Focus()
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
        cmbEstatus.Enabled = False
    End Sub

    Private Sub btnBorrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBorrar.Click
        'Si es modo nuevo o Editar
        If Me.Grabando Then
            Consulta("")
            Botonos(True)
        Else
            'LLamamos la rutina Eliminar
            Datos.consulta_non_query("DELETE FROM [dbo].[Transporte] WHERE id_tpt='" & txtId.Text & "'")

            'Refrescamos los dato de pantalla
            Consulta("")
            Botonos(True)
        End If
        'txtDescripcion.Focus()
    End Sub

    'Despliega pantalla para buscar el Cajero
    Private Sub BuscaCodigo()
        Dim stringSQL As String

        stringSQL = "SELECT id_tpt As Codigo, nombre_tpt As Nombre FROM [dbo].[Transporte]"

        'Script para Filtrar el Grid
        Dim stringSqlFiltro As String = "SELECT id_tpt As Codigo, nombre_tpt As Nombre FROM [dbo].[Transporte] WHERE id_tpt "

        'Instancia Ayuda Transporte
        'Dim frmBuscar As New BuscaCodigo(stringSQL, stringSqlFiltro, "Transportes")
        'frmBuscar.ShowDialog()

        Dim frmBuscar As New BuscaCodigo(stringSQL, {"Código", "Nombre", "50", "200"}, 2, stringSqlFiltro, "Transportes")
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
        dTable = Datos.consulta_reader("SELECT TOP 1 id_tpt FROM dbo.Transporte ORDER BY id_tpt")

        For Each dRow As DataRow In dTable.Rows
            Me.LineaActual = dRow("id_tpt")

            Consulta(dRow("id_tpt").ToString)
        Next
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnanterior.Click
        'Para Anterior
        dTable = Datos.consulta_reader("SELECT TOP 1 id_tpt FROM dbo.Transporte WHERE id_tpt <" & Me.LineaActual & "ORDER BY id_tpt DESC")

        For Each dRow As DataRow In dTable.Rows
            Me.LineaActual = dRow("id_tpt")

            Consulta(dRow("id_tpt").ToString)
        Next
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnsiguiente.Click
        'Para Sigiente
        dTable = Datos.consulta_reader("SELECT TOP 1 id_tpt FROM dbo.Transporte WHERE id_tpt >" & Me.LineaActual & "ORDER BY id_tpt")

        For Each dRow As DataRow In dTable.Rows
            Me.LineaActual = dRow("id_tpt")

            Consulta(dRow("id_tpt").ToString)
        Next
    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnultimo.Click
        'Para Inicio
        dTable = Datos.consulta_reader("SELECT TOP 1 id_tpt FROM dbo.Transporte ORDER BY id_tpt DESC")

        For Each dRow As DataRow In dTable.Rows
            Me.UltiLinea = dRow("id_tpt")
            Me.LineaActual = dRow("id_tpt")

            Consulta(dRow("id_tpt").ToString)
        Next

    End Sub
#End Region

End Class