Public Class frmBodega

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

    Private Sub frmBodega_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Consulta("")
        Botonos(True)
        'txtDescripcion.Focus()

        ''Arma el ComboBox
        'Datos.cargar_combo(cmbVendedor, _
        '                  "Select Nombre_Ven, Codigo_Ven From [dbo].[fCatalogoVendedores] Order by 1", _
        '                 "Codigo_Ven", "Nombre_Ven")

        Me.BackColor = Color.White
        Label4.ForeColor = Color.SeaGreen
    End Sub

    'Sub Modulo Botonos
    'Sirver para Muestra o Oculta los Botonos
    Private Sub Botonos(ByVal Habilita As Boolean)
        '-Botonos
        btnNuevo.Visible = Habilita
        btnBuscar.Enabled = Habilita
        btnSalir.Visible = Habilita
        btnprimero.Visible = Habilita
        btnanterior.Visible = Habilita
        btnsiguiente.Visible = Habilita
        btnultimo.Visible = Habilita

        If Me.Grabando Then
            btnBorrar.Text = "cancelar"
            btnGrabar.Text = "grabar"
        Else
            btnGrabar.Text = "actualizar"
            btnBorrar.Text = "borrar"
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
        txtContacto.Text = ""
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
                dTable = Datos.consulta_reader("SELECT TOP 1 * FROM dbo.Bodega ORDER BY Id_bod")
            Else
                dTable = Datos.consulta_reader("SELECT * FROM dbo.Bodega WHERE id_bod=" & _codigo)
            End If

            Dim nCod As String = ""

            'Setea Campos
            For Each dRow As DataRow In dTable.Rows
                txtId.Text = dRow.Item("id_bod").ToString
                txtNombre.Text = dRow.Item("nombre_bod").ToString
                txtContacto.Text = dRow.Item("contacto_bod").ToString
                cmbEstatus.Text = dRow.Item("baja_bod").ToString
            Next

            'Asigna Linea Actual
            Me.LineaActual = dTable.Rows.Item(0).Item("id_bod")

            Me.Grabando = False
            Me.NuevoReg = False
            Navega()

        Catch ex As Exception
            MessageBox.Show("ERROR: " & ex.Message, "Comprobar tabla * Bodega")
        End Try
    End Sub

    'Para Hacer Navegar
    Private Sub Navega()
        'Contar Lineas
        dTable = Datos.consulta_reader("SELECT Count(1) As TotRec FROM dbo.Bodega")
        Me.TotLineas = dTable.Rows.Item(0).Item("TotRec")

        'Primera Linea
        dTable = Datos.consulta_reader("SELECT TOP 1 id_bod FROM dbo.Bodega ORDER BY id_bod")
        Me.PrimLinea = dTable.Rows.Item(0).Item("id_bod")

        'Ultima Linea
        dTable = Datos.consulta_reader("SELECT TOP 1 id_bod FROM dbo.Bodega ORDER BY id_bod DESC")
        Me.UltiLinea = dTable.Rows.Item(0).Item("id_bod")

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
                dTable = Datos.consulta_reader("SELECT MAX(id_bod)+1 rMax FROM [dbo].[Bodega]")

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
                                    INSERT INTO [dbo].[Bodega] ([id_bod],
                                                                [nombre_bod],
                                                                [contacto_bod],
                                                                [baja_bod], [actualiza_bod])
                                                     VALUES('<%= txtId.Text %>',
                                                          '<%= txtNombre.Text %>',
                                                          '<%= txtContacto.Text %>',
                                                          '<%= cmbEstatus.Text %>',1)
                                     </sqlExp>.Value

                'LLamamos la rutina para grabar
                Datos.consulta_non_query(StringSql)

            Else
                'Se arma el script para grabar SQL
                Dim StringSql As String = <sqlExp>
                                     UPDATE [dbo].[Bodega] SET [nombre_bod] =  '<%= txtNombre.Text %>',
                                                               [contacto_bod] = '<%= txtContacto.Text %>',
                                                               [actualiza_bod] = 2
                                      WHERE id_bod = '<%= txtId.Text %>' 
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
            Datos.consulta_non_query("DELETE FROM [dbo].[Bodega] WHERE id_bod='" & txtId.Text & "'")

            'Refrescamos los dato de pantalla
            Consulta("")
            Botonos(True)
        End If
        'txtDescripcion.Focus()
    End Sub

    'Despliega pantalla para buscar el Cajero
    Private Sub BuscaCodigo()
        Dim stringSQL As String

        stringSQL = "SELECT id_bod As Codigo, nombre_bod As Nombre FROM [dbo].[Bodega]"

        'Script para Filtrar el Grid
        Dim stringSqlFiltro As String = "SELECT id_bod As Codigo, nombre_bod As Nombre FROM [dbo].[Bodega] WHERE id_bod "

        'Instancia Ayuda Bodega
        'Dim frmBuscar As New BuscaCodigo(stringSQL, stringSqlFiltro, "Bodegas")
        'frmBuscar.ShowDialog()

        Dim frmBuscar As New BuscaCodigo(stringSQL, {"Código", "Nombre", "50", "250"}, 2, stringSqlFiltro, "Bodegas")
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
        dTable = Datos.consulta_reader("SELECT TOP 1 id_bod FROM dbo.Bodega ORDER BY id_bod")

        For Each dRow As DataRow In dTable.Rows
            Me.LineaActual = dRow("id_bod")

            Consulta(dRow("id_bod").ToString)
        Next
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnanterior.Click
        'Para Anterior
        dTable = Datos.consulta_reader("SELECT TOP 1 id_bod FROM dbo.Bodega WHERE id_bod <" & Me.LineaActual & "ORDER BY id_bod DESC")

        For Each dRow As DataRow In dTable.Rows
            Me.LineaActual = dRow("id_bod")

            Consulta(dRow("id_bod").ToString)
        Next
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnsiguiente.Click
        'Para Sigiente
        dTable = Datos.consulta_reader("SELECT TOP 1 id_bod FROM dbo.Bodega WHERE id_bod >" & Me.LineaActual & "ORDER BY id_bod")

        For Each dRow As DataRow In dTable.Rows
            Me.LineaActual = dRow("id_bod")

            Consulta(dRow("id_bod").ToString)
        Next
    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnultimo.Click
        'Para Inicio
        dTable = Datos.consulta_reader("SELECT TOP 1 id_bod FROM dbo.Bodega ORDER BY id_bod DESC")

        For Each dRow As DataRow In dTable.Rows
            Me.UltiLinea = dRow("id_bod")
            Me.LineaActual = dRow("id_bod")

            Consulta(dRow("id_bod").ToString)
        Next

    End Sub
#End Region


End Class