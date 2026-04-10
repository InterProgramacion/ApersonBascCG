Public Class frmFlotillaCamiones

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

    Private Sub frmFlotillaCamiones_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Arma el ComboBox
        Datos.cargar_combo(cmbTransporte, _
                           "Select nombre_tpt, id_tpt From [dbo].[Transporte] Order by 1", _
                           "id_tpt", "nombre_tpt")

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
        txtPlaca.Text = ""
        txtModelo.Text = ""
        txtColor.Text = ""
        txtNumero.Text = ""
        cmbEstatus.Text = "ACTIVO"

        'Se posiciona para Iniciar 
        txtPlaca.Focus()
    End Sub

#End Region

#Region "Consulta"
    'Procedimiento para mostrar el Primer Registro
    Private Sub Consulta(ByVal _codigo As String)
        Try
            'Para mostrar el Dato
            If _codigo = String.Empty Then
                dTable = Datos.consulta_reader("SELECT TOP 1 * FROM dbo.Fcamiones ORDER BY id_fca")
            Else
                dTable = Datos.consulta_reader("SELECT * FROM dbo.Fcamiones WHERE id_fca=" & _codigo)
            End If

            Dim nCod As String = ""

            'Setea Campos
            For Each dRow As DataRow In dTable.Rows
                txtId.Text = dRow.Item("id_fca").ToString
                txtPlaca.Text = dRow.Item("placa_fca").ToString
                txtModelo.Text = dRow.Item("model_fca").ToString
                txtColor.Text = dRow.Item("color_fca").ToString
                txtNumero.Text = dRow.Item("numero_fca").ToString
                cmbEstatus.Text = dRow.Item("baja_fca").ToString
                cmbTransporte.SelectedValue = dRow.Item("transporte_fca").ToString
            Next

            'Asigna Linea Actual
            Me.LineaActual = dTable.Rows.Item(0).Item("id_fca")

            Me.Grabando = False
            Me.NuevoReg = False
            Navega()

        Catch ex As Exception
            MessageBox.Show("ERROR: " & ex.Message, "Comprobar tabla * FCamiones")
        End Try
    End Sub

    'Para Hacer Navegar
    Private Sub Navega()
        'Contar Lineas
        dTable = Datos.consulta_reader("SELECT Count(1) As TotRec FROM dbo.FCamiones")
        Me.TotLineas = dTable.Rows.Item(0).Item("TotRec")

        'Primera Linea
        dTable = Datos.consulta_reader("SELECT TOP 1 id_fca FROM dbo.FCamiones ORDER BY id_fca")
        Me.PrimLinea = dTable.Rows.Item(0).Item("id_fca")

        'Ultima Linea
        dTable = Datos.consulta_reader("SELECT TOP 1 id_fca FROM dbo.FCamiones ORDER BY id_fca DESC")
        Me.UltiLinea = dTable.Rows.Item(0).Item("id_fca")

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
                dTable = Datos.consulta_reader("SELECT MAX(id_fca)+1 rMax FROM [dbo].[fcamiones]")

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
                                    INSERT INTO [dbo].[fcamiones] ([id_fca], [placa_fca],
                                                                [model_fca],
                                                                [color_fca],
                                                                [numero_fca],
                                                                [baja_fca],
                                                                [transporte_fca],actualiza_fca)
                                                     VALUES('<%= txtId.Text %>', '<%= txtPlaca.Text %>',
                                                          '<%= txtModelo.Text %>',
                                                          '<%= txtColor.Text %>',
                                                          '<%= txtNumero.Text %>',
                                                          '<%= cmbEstatus.Text %>',
                                                          '<%= cmbTransporte.SelectedItem("id_tpt").ToString %>',
                                                          1)
                                     </sqlExp>.Value

                'LLamamos la rutina para grabar
                Datos.consulta_non_query(StringSql)
            Else
                'Se arma el script para grabar SQL
                Dim StringSql As String = <sqlExp>
                                     UPDATE [dbo].[fcamiones] SET [placa_fca] = '<%= txtPlaca.Text %>',
                                                                [model_fca] =  '<%= txtModelo.Text %>',
                                                                [color_fca] = '<%= txtColor.Text %>',
                                                                [numero_fca] = '<%= txtNumero.Text %>',
                                                                [baja_fca] = '<%= cmbEstatus.Text %>',
                                                                [transporte_fca] = '<%= cmbTransporte.SelectedItem("id_tpt").ToString %>',
                                                                [actualiza_fca] = '<%= "2" %>'
                                      WHERE id_fca = '<%= txtId.Text %>' 
                                      AND actualiza_fca IN(1,2)
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

        If txtPlaca.Text = String.Empty Then
            MessageBox.Show("ERROR: La Placa no puede ser Blanco")
            txtPlaca.Focus()
        ElseIf txtModelo.Text = String.Empty Then
            MessageBox.Show("ERROR: El Modelo no puede ser Blanco")
            txtModelo.Focus()
        Else

            Si = True
        End If

        If NuevoReg = True Then
            'Suma Correlativo
            dTable = Datos.consulta_reader("select placa_fca from [dbo].[fcamiones] where rtrim(ltrim(placa_fca)) = '" & txtPlaca.Text.Trim & "'")

            If dTable.Rows.Count > 0 Then
                MessageBox.Show("ERROR: La Placa ya existe por favor Verificar !!!")
                txtPlaca.Focus()
                Si = False
            Else
                Si = True
            End If
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
            Datos.consulta_non_query("DELETE FROM [dbo].[fcamiones] WHERE id_fca='" & txtId.Text & "'")

            'Refrescamos los dato de pantalla
            Consulta("")
            Botonos(True)
        End If
        'txtDescripcion.Focus()
    End Sub

    'Despliega pantalla para buscar el Cajero
    Private Sub BuscaCodigo()
        Dim stringSQL As String

        stringSQL = "SELECT id_fca As Codigo, placa_fca As Nombre FROM [dbo].[fcamiones]"

        'Script para Filtrar el Grid
        Dim stringSqlFiltro As String = "SELECT id_fca As Codigo, placa_fca As Nombre FROM [dbo].[fcamiones] WHERE id_fca "

        'Instancia Ayuda Cliente
        'Dim frmBuscar As New BuscaCodigo(stringSQL, stringSqlFiltro, "Clientes")
        'frmBuscar.ShowDialog()

        Dim frmBuscar As New BuscaCodigo(stringSQL, {"Código", "Placa", "50", "100"}, 2, stringSqlFiltro, "Flotilla Camiones")
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
        dTable = Datos.consulta_reader("SELECT TOP 1 id_fca FROM dbo.fcamiones ORDER BY id_fca")

        For Each dRow As DataRow In dTable.Rows
            Me.LineaActual = dRow("id_fca")

            Consulta(dRow("id_fca").ToString)
        Next
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnanterior.Click
        'Para Anterior
        dTable = Datos.consulta_reader("SELECT TOP 1 id_fca FROM dbo.fcamiones WHERE id_fca <" & Me.LineaActual & "ORDER BY id_fca DESC")

        For Each dRow As DataRow In dTable.Rows
            Me.LineaActual = dRow("id_fca")

            Consulta(dRow("id_fca").ToString)
        Next
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnsiguiente.Click
        'Para Sigiente
        dTable = Datos.consulta_reader("SELECT TOP 1 id_fca FROM dbo.fcamiones WHERE id_fca >" & Me.LineaActual & "ORDER BY id_fca")

        For Each dRow As DataRow In dTable.Rows
            Me.LineaActual = dRow("id_fca")

            Consulta(dRow("id_fca").ToString)
        Next
    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnultimo.Click
        'Para Inicio
        dTable = Datos.consulta_reader("SELECT TOP 1 id_fca FROM dbo.fcamiones ORDER BY id_fca DESC")

        For Each dRow As DataRow In dTable.Rows
            Me.UltiLinea = dRow("id_fca")
            Me.LineaActual = dRow("id_fca")

            Consulta(dRow("id_fca").ToString)
        Next

    End Sub
#End Region

End Class