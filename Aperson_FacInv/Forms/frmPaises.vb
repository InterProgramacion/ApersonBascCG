Public Class frmPaises


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

    Private Sub frmPaises_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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
        txtPais.Text = ""
        txtNombre.Text = ""
        cmbEstatus.Text = "ACTIVO"

        'Se posiciona para Iniciar 
        txtPais.Focus()
    End Sub

#End Region

#Region "Consulta"
    'Procedimiento para mostrar el Primer Registro
    Private Sub Consulta(ByVal _codigo As String)
        Try
            'Para mostrar el Dato
            If _codigo = String.Empty Then
                dTable = Datos.consulta_reader("SELECT TOP 1 * FROM dbo.paisdestorig ORDER BY id_pai")
            Else
                dTable = Datos.consulta_reader("SELECT * FROM dbo.paisdestorig WHERE id_pai=" & _codigo)
            End If

            Dim nCod As String = ""

            'Setea Campos
            For Each dRow As DataRow In dTable.Rows
                txtId.Text = dRow.Item("id_pai").ToString
                txtPais.Text = dRow.Item("pais_pai").ToString
                txtNombre.Text = dRow.Item("nombre_pai").ToString
                cmbEstatus.Text = dRow.Item("baja_pai").ToString
            Next

            'Asigna Linea Actual
            Me.LineaActual = dTable.Rows.Item(0).Item("id_pai")

            Me.Grabando = False
            Me.NuevoReg = False
            Navega()

        Catch ex As Exception
            MessageBox.Show("ERROR: " & ex.Message, "Comprobar tabla * paisdestorig")
        End Try
    End Sub

    'Para Hacer Navegar
    Private Sub Navega()
        'Contar Lineas
        dTable = Datos.consulta_reader("SELECT Count(1) As TotRec FROM dbo.paisdestorig")
        Me.TotLineas = dTable.Rows.Item(0).Item("TotRec")

        'Primera Linea
        dTable = Datos.consulta_reader("SELECT TOP 1 id_pai FROM dbo.paisdestorig ORDER BY id_pai")
        Me.PrimLinea = dTable.Rows.Item(0).Item("id_pai")

        'Ultima Linea
        dTable = Datos.consulta_reader("SELECT TOP 1 id_pai FROM dbo.paisdestorig ORDER BY id_pai DESC")
        Me.UltiLinea = dTable.Rows.Item(0).Item("id_pai")

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
                dTable = Datos.consulta_reader("SELECT MAX(id_pai)+1 rMax FROM [dbo].[paisdestorig]")

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
                                    INSERT INTO [dbo].[paisdestorig] ([id_pai], [pais_pai],
                                                                [nombre_pai],
                                                                [baja_pai],
                                                                [actualiza_pai])
                                                     VALUES('<%= txtId.Text %>', '<%= txtPais.Text %>',
                                                          '<%= txtNombre.Text %>',
                                                          '<%= cmbEstatus.Text %>',1)
                                     </sqlExp>.Value

                'LLamamos la rutina para grabar
                Datos.consulta_non_query(StringSql)

            Else
                'Se arma el script para grabar SQL
                Dim StringSql As String = <sqlExp>
                                     UPDATE [dbo].[paisdestorig] SET [pais_pai] = '<%= txtPais.Text %>',
                                                                [nombre_pai] =  '<%= txtNombre.Text %>',
                                                                [baja_pai]   =  '<%= cmbEstatus.Text %>',
                                                                actualiza_pai = 2    
                                      WHERE id_pai = '<%= txtId.Text %>' 
                                      AND actualiza_pai IN(1,2)
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
        ElseIf txtPais.Text = String.Empty Then
            MessageBox.Show("ERROR: El Nit no puede ser Blanco")
            txtPais.Focus()
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
            Datos.consulta_non_query("DELETE FROM [dbo].[paisdestorig] WHERE id_pai='" & txtId.Text & "'")

            'Refrescamos los dato de pantalla
            Consulta("")
            Botonos(True)
        End If
        'txtDescripcion.Focus()
    End Sub

    'Despliega pantalla para buscar el Cajero
    Private Sub BuscaCodigo()
        Dim stringSQL As String

        stringSQL = "SELECT id_pai As Codigo, nombre_pai As Nombre FROM [dbo].[paisdestorig]"

        'Script para Filtrar el Grid
        Dim stringSqlFiltro As String = "SELECT id_pai As Codigo, nombre_pai As Nombre FROM [dbo].[paisdestorig] WHERE id_pai "

        'Instancia Ayuda paisdestorig
        'Dim frmBuscar As New BuscaCodigo(stringSQL, stringSqlFiltro, "paisdestorigs")
        'frmBuscar.ShowDialog()

        Dim frmBuscar As New BuscaCodigo(stringSQL, {"Código", "Nombre", "50", "250"}, 2, stringSqlFiltro, "Listado Países")
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
        dTable = Datos.consulta_reader("SELECT TOP 1 id_pai FROM dbo.paisdestorig ORDER BY id_pai")

        For Each dRow As DataRow In dTable.Rows
            Me.LineaActual = dRow("id_pai")

            Consulta(dRow("id_pai").ToString)
        Next
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnanterior.Click
        'Para Anterior
        dTable = Datos.consulta_reader("SELECT TOP 1 id_pai FROM dbo.paisdestorig WHERE id_pai <" & Me.LineaActual & "ORDER BY id_pai DESC")

        For Each dRow As DataRow In dTable.Rows
            Me.LineaActual = dRow("id_pai")

            Consulta(dRow("id_pai").ToString)
        Next
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnsiguiente.Click
        'Para Sigiente
        dTable = Datos.consulta_reader("SELECT TOP 1 id_pai FROM dbo.paisdestorig WHERE id_pai >" & Me.LineaActual & "ORDER BY id_pai")

        For Each dRow As DataRow In dTable.Rows
            Me.LineaActual = dRow("id_pai")

            Consulta(dRow("id_pai").ToString)
        Next
    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnultimo.Click
        'Para Inicio
        dTable = Datos.consulta_reader("SELECT TOP 1 id_pai FROM dbo.paisdestorig ORDER BY id_pai DESC")

        For Each dRow As DataRow In dTable.Rows
            Me.UltiLinea = dRow("id_pai")
            Me.LineaActual = dRow("id_pai")

            Consulta(dRow("id_pai").ToString)
        Next

    End Sub
#End Region


End Class