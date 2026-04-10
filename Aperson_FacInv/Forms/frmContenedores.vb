Public Class frmContenedores

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

    Private Sub frmContenedores_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Consulta("")
        Botonos(True)

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
    End Sub

    Private Sub Textos()
        txtId.Text = ""
        txtId.Enabled = False
        txtTamaño.Text = ""
        txtPesoMin.Text = ""
        txtPesoMax.Text = ""
        cmbTipo.Text = "CONTENEDOR"
        cmbEstado.Text = "ACTIVO"


        'Se posiciona para Iniciar 
        txtTamaño.Focus()
    End Sub

#End Region

#Region "Consulta"
    'Procedimiento para mostrar el Primer Registro
    Private Sub Consulta(ByVal _codigo As String)
        Try
            'Para mostrar el Dato
            If _codigo = String.Empty Then
                dTable = Datos.consulta_reader("SELECT TOP 1 * FROM dbo.Contenedores ORDER BY Id_con")
            Else
                dTable = Datos.consulta_reader("SELECT * FROM dbo.Contenedores WHERE Id_con=" & _codigo)
            End If

            Dim nCod As String = ""

            'Setea Campos
            For Each dRow As DataRow In dTable.Rows
                txtId.Text = dRow.Item("Id_con").ToString
                txtTamaño.Text = dRow.Item("tamano_con").ToString
                txtPesoMin.Text = dRow.Item("pesomin_con").ToString
                txtPesoMax.Text = dRow.Item("pesomax_con").ToString
                cmbTipo.Text = dRow.Item("tipo_con").ToString
                cmbEstado.Text = dRow.Item("baja_con").ToString
            Next

            'Asigna Linea Actual
            Me.LineaActual = dTable.Rows.Item(0).Item("Id_con")

            Me.Grabando = False
            Me.NuevoReg = False
            Navega()

        Catch ex As Exception
            MessageBox.Show("ERROR: " & ex.Message, "Comprobar tabla * Contenedores")
        End Try
    End Sub

    'Para Hacer Navegar
    Private Sub Navega()
        'Contar Lineas
        dTable = Datos.consulta_reader("SELECT Count(1) As TotRec FROM dbo.Contenedores")
        Me.TotLineas = dTable.Rows.Item(0).Item("TotRec")

        'Primera Linea
        dTable = Datos.consulta_reader("SELECT TOP 1 Id_con FROM dbo.Contenedores ORDER BY Id_con")
        Me.PrimLinea = dTable.Rows.Item(0).Item("Id_con")

        'Ultima Linea
        dTable = Datos.consulta_reader("SELECT TOP 1 Id_con FROM dbo.Contenedores ORDER BY Id_con DESC")
        Me.UltiLinea = dTable.Rows.Item(0).Item("Id_con")

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
                dTable = Datos.consulta_reader("SELECT MAX(id_con)+1 rMax FROM [dbo].[Contenedores]")

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
                                    INSERT INTO [dbo].[Contenedores] ([id_con], [tamano_con],
                                                                [pesomin_con],
                                                                [pesomax_con],
                                                                [tipo_con],
                                                                [baja_con])
                                        VALUES('<%= txtId.Text %>',
                                              <%= txtTamaño.Text %>,
                                              <%= txtPesoMin.Text %>,
                                              <%= txtPesoMax.Text %>,
                                             '<%= cmbTipo.Text %>',
                                             '<%= cmbEstado.Text %>')
                                     </sqlExp>.Value

                'LLamamos la rutina para grabar
                Datos.consulta_non_query(StringSql)

            Else
                'Se arma el script para grabar SQL
                Dim StringSql As String = <sqlExp>
                                     UPDATE [dbo].[Contenedores] SET [tamano_con] = <%= txtTamaño.Text %>,
                                                                [pesomin_con] =  <%= txtPesoMin.Text %>,
                                                                [pesomax_con] = <%= txtPesoMax.Text %>,
                                                                [tipo_con] = '<%= cmbTipo.Text %>',
                                                                [baja_con] = '<%= cmbEstado.Text %>'
                                      WHERE Id_con = '<%= txtId.Text %>' 
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

        If txtTamaño.Text = String.Empty Then
            MessageBox.Show("ERROR: La Placa no puede ser Blanco")
            txtTamaño.Focus()
        ElseIf txtPesoMin.Text = String.Empty Then
            MessageBox.Show("ERROR: El Modelo no puede ser Blanco")
            txtPesoMin.Focus()
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
            'LLamamos la rutina Eliminar
            Datos.consulta_non_query("DELETE FROM [dbo].[Contenedores] WHERE Id_con='" & txtId.Text & "'")

            'Refrescamos los dato de pantalla
            Consulta("")
            Botonos(True)
        End If
        'txtDescripcion.Focus()
    End Sub

    'Despliega pantalla para buscar el Cajero
    Private Sub BuscaCodigo()
        Dim stringSQL As String

        stringSQL = "SELECT Id_con As Codigo, tamano_con As Nombre, pesomin_con, pesomax_con FROM [dbo].[Contenedores]"

        'Script para Filtrar el Grid
        Dim stringSqlFiltro As String = "SELECT Id_con As Codigo, tamano_con As Nombre FROM [dbo].[Contenedores] WHERE Id_con "

        'Instancia Ayuda Cliente
        'Dim frmBuscar As New BuscaCodigo(stringSQL, stringSqlFiltro, "Clientes")
        'frmBuscar.ShowDialog()

        Dim frmBuscar As New BuscaCodigo(stringSQL, {"Código", "Tamaño", "Peso Minimo", "Peso Maximo", "50", "100", "100", "150"}, 4, stringSqlFiltro, "Registro Contendores / Arañas")
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
        dTable = Datos.consulta_reader("SELECT TOP 1 Id_con FROM dbo.Contenedores ORDER BY Id_con")

        For Each dRow As DataRow In dTable.Rows
            Me.LineaActual = dRow("Id_con")

            Consulta(dRow("Id_con").ToString)
        Next
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnanterior.Click
        'Para Anterior
        dTable = Datos.consulta_reader("SELECT TOP 1 Id_con FROM dbo.Contenedores WHERE Id_con <" & Me.LineaActual & "ORDER BY Id_con DESC")

        For Each dRow As DataRow In dTable.Rows
            Me.LineaActual = dRow("Id_con")

            Consulta(dRow("Id_con").ToString)
        Next
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnsiguiente.Click
        'Para Sigiente
        dTable = Datos.consulta_reader("SELECT TOP 1 Id_con FROM dbo.Contenedores WHERE Id_con >" & Me.LineaActual & "ORDER BY Id_con")

        For Each dRow As DataRow In dTable.Rows
            Me.LineaActual = dRow("Id_con")

            Consulta(dRow("Id_con").ToString)
        Next
    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnultimo.Click
        'Para Inicio
        dTable = Datos.consulta_reader("SELECT TOP 1 Id_con FROM dbo.Contenedores ORDER BY Id_con DESC")

        For Each dRow As DataRow In dTable.Rows
            Me.UltiLinea = dRow("Id_con")
            Me.LineaActual = dRow("Id_con")

            Consulta(dRow("Id_con").ToString)
        Next

    End Sub
#End Region

End Class