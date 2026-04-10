Public Class frmClientes

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

    Private Sub frmClientes_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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
        txtNit.Text = ""
        txtNombre.Text = ""
        txtContacto.Text = ""
        cmbEstatus.Text = "ACTIVO"

        'Se posiciona para Iniciar 
        txtNit.Focus()
    End Sub

#End Region

#Region "Consulta"
    'Procedimiento para mostrar el Primer Registro
    Private Sub Consulta(ByVal _codigo As String)
        Try
            'Para mostrar el Dato
            If _codigo = String.Empty Then
                dTable = Datos.consulta_reader("SELECT TOP 1 * FROM dbo.Cliente ORDER BY Id_cli")
            Else
                dTable = Datos.consulta_reader("SELECT * FROM dbo.Cliente WHERE id_cli=" & _codigo)
            End If

            Dim nCod As String = ""

            'Setea Campos
            For Each dRow As DataRow In dTable.Rows
                txtId.Text = dRow.Item("id_cli").ToString
                txtNit.Text = dRow.Item("nit_cli").ToString
                txtNombre.Text = dRow.Item("nombre_cli").ToString
                txtContacto.Text = dRow.Item("contacto_cli").ToString
                cmbEstatus.Text = dRow.Item("baja_cli").ToString
            Next

            'Asigna Linea Actual
            Me.LineaActual = dTable.Rows.Item(0).Item("id_cli")

            Me.Grabando = False
            Me.NuevoReg = False
            Navega()

        Catch ex As Exception
            MessageBox.Show("ERROR: " & ex.Message, "Comprobar tabla * Cliente")
        End Try
    End Sub

    'Para Hacer Navegar
    Private Sub Navega()
        'Contar Lineas
        dTable = Datos.consulta_reader("SELECT Count(1) As TotRec FROM dbo.Cliente")
        Me.TotLineas = dTable.Rows.Item(0).Item("TotRec")

        'Primera Linea
        dTable = Datos.consulta_reader("SELECT TOP 1 id_cli FROM dbo.Cliente ORDER BY id_cli")
        Me.PrimLinea = dTable.Rows.Item(0).Item("id_cli")

        'Ultima Linea
        dTable = Datos.consulta_reader("SELECT TOP 1 id_cli FROM dbo.Cliente ORDER BY id_cli DESC")
        Me.UltiLinea = dTable.Rows.Item(0).Item("id_cli")

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
                dTable = Datos.consulta_reader("SELECT MAX(id_cli)+1 rMax FROM [dbo].[Cliente]")

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
                                    INSERT INTO [dbo].[Cliente] ([id_cli],
                                                                [nit_cli],
                                                                [nombre_cli],
                                                                [contacto_cli],
                                                                [baja_cli],actualiza_cli)
                                                     VALUES('<%= txtId.Text %>',
                                                          '<%= txtNit.Text %>',
                                                          '<%= txtNombre.Text %>',
                                                          '<%= txtContacto.Text %>',
                                                          '<%= cmbEstatus.Text %>',1)
                                     </sqlExp>.Value

                'LLamamos la rutina para grabar
                Datos.consulta_non_query(StringSql)

            Else
                'Se arma el script para grabar SQL
                Dim StringSql As String = <sqlExp>
                                     UPDATE [dbo].[Cliente] SET [nit_cli] = '<%= txtNit.Text %>',
                                                                [nombre_cli] =  '<%= txtNombre.Text %>',
                                                                [contacto_cli] = '<%= txtContacto.Text %>',
                                                                actualiza_cli = 2
                                      WHERE id_cli = '<%= txtId.Text %>' 
                                      AND actualiza_cli IN(1,2)
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
        ElseIf txtNit.Text = String.Empty Then
            MessageBox.Show("ERROR: El Nit no puede ser Blanco")
            txtNit.Focus()
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
            Datos.consulta_non_query("DELETE FROM [dbo].[Cliente] WHERE id_cli='" & txtId.Text & "'")

            'Refrescamos los dato de pantalla
            Consulta("")
            Botonos(True)
        End If
        'txtDescripcion.Focus()
    End Sub

    'Despliega pantalla para buscar el Cajero
    Private Sub BuscaCodigo()
        Dim stringSQL As String

        stringSQL = "SELECT id_cli As Codigo, Nombre_cli As Nombre, Nit_cli FROM [dbo].[Cliente]"

        'Script para Filtrar el Grid
        Dim stringSqlFiltro As String = "SELECT id_cli As Codigo, Nombre_cli As Nombre FROM [dbo].[Cliente] WHERE id_cli "

        'Instancia Ayuda Cliente
        'Dim frmBuscar As New BuscaCodigo(stringSQL, stringSqlFiltro, "Clientes")
        'frmBuscar.ShowDialog()

        Dim frmBuscar As New BuscaCodigo(stringSQL, {"Código", "Nombre", "Nit", "50", "250", "150"}, 3, stringSqlFiltro, "Clientes")
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
        dTable = Datos.consulta_reader("SELECT TOP 1 id_cli FROM dbo.Cliente ORDER BY id_cli")

        For Each dRow As DataRow In dTable.Rows
            Me.LineaActual = dRow("id_cli")

            Consulta(dRow("id_cli").ToString)
        Next
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnanterior.Click
        'Para Anterior
        dTable = Datos.consulta_reader("SELECT TOP 1 id_cli FROM dbo.Cliente WHERE id_cli <" & Me.LineaActual & "ORDER BY id_cli DESC")

        For Each dRow As DataRow In dTable.Rows
            Me.LineaActual = dRow("id_cli")

            Consulta(dRow("id_cli").ToString)
        Next
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnsiguiente.Click
        'Para Sigiente
        dTable = Datos.consulta_reader("SELECT TOP 1 id_cli FROM dbo.Cliente WHERE id_cli >" & Me.LineaActual & "ORDER BY id_cli")

        For Each dRow As DataRow In dTable.Rows
            Me.LineaActual = dRow("id_cli")

            Consulta(dRow("id_cli").ToString)
        Next
    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnultimo.Click
        'Para Inicio
        dTable = Datos.consulta_reader("SELECT TOP 1 id_cli FROM dbo.Cliente ORDER BY id_cli DESC")

        For Each dRow As DataRow In dTable.Rows
            Me.UltiLinea = dRow("id_cli")
            Me.LineaActual = dRow("id_cli")

            Consulta(dRow("id_cli").ToString)
        Next

    End Sub
#End Region



End Class