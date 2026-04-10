Public Class frmBuques

#Region "Variables"
    Dim Datos As New DatosGenereral()
    Dim NuevoReg As Boolean
    Dim Grabando As Boolean
    Dim dTable As New DataTable
    Dim TotLineas As Integer
    Dim PrimLinea As Integer
    Dim UltiLinea As Integer
    Dim LineaActual As Integer
    Dim dv As New DataView
    Dim dLinea As Integer = 0
#End Region

#Region "Entorno"

    'Private Sub frmBuques_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress

    '    'If CInt(Val(e.KeyChar)) = CInt(Keys.F5) Then
    '    '    MsgBox("Precionamos la Telcla F5")
    '    'End If

    '    Select Case e.KeyChar
    '        Case ChrW(Keys.Enter), ChrW(Keys.Enter) ' mayúsculas y minúsculas
    '            'Label5.Text &= vbCrLf & "Tecla W en el formulario"
    '            txtNombre.Text &= vbCrLf & "Tecla ENTER (KeyPress)"
    '    End Select
    'End Sub

#Region "Efectos Campos"
    Private Sub onEnterKey(sender As Object, e As KeyEventArgs) Handles _
                                                                txtNombre.KeyDown, _
                                                                txtBodega.KeyDown, _
                                                                txtViaje.KeyDown

        If e.KeyCode = Keys.Enter Then

            'MsgBox(DirectCast(sender, System.Windows.Forms.TextBox).Name)

            Select Case (DirectCast(sender, System.Windows.Forms.TextBox).Name)
                Case "txtNombre"
                    txtViaje.Focus()
                Case "txtViaje"
                    DTP_FechaA.Focus()
                Case "txtBodega"
                    Me.ActiveControl = btnGrabar

            End Select

        End If
    End Sub

    Private Sub DTP_FechaA_KeyDown(sender As Object, e As KeyEventArgs) Handles DTP_FechaA.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtBodega.Focus()
        End If
    End Sub

    'Private Sub txtCodigo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCodigo.KeyPress
    '    If InStr(1, "0123456789.-" & Chr(8), e.KeyChar) = 0 Then
    '        e.KeyChar = ""
    '    End If
    'End Sub

    ''Calor de Entrada y Salida Cajas ------------------------------
    Private Sub txtNombre_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNombre.GotFocus, txtViaje.GotFocus, txtBodega.GotFocus, DTP_FechaA.GotFocus
        sender.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
    End Sub
    Private Sub txtNombre_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNombre.LostFocus, txtViaje.LostFocus, txtBodega.LostFocus, DTP_FechaA.LostFocus
        sender.BackColor = System.Drawing.Color.White
    End Sub

    Private Sub btnNuevo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNuevo.GotFocus, _
                                                                                               btnGrabar.GotFocus, _
                                                                                               btnBorrar.GotFocus, _
                                                                                               btnSalir.GotFocus
        sender.BackColor = System.Drawing.Color.SeaGreen
    End Sub
    Private Sub btnNuevo_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNuevo.LostFocus, _
                                                                                                btnGrabar.LostFocus, _
                                                                                                btnBorrar.LostFocus, _
                                                                                                btnSalir.LostFocus
        sender.BackColor = System.Drawing.Color.MidnightBlue
    End Sub


    Private Sub btnNuevo_MouseEnter(sender As Object, e As EventArgs) Handles btnNuevo.MouseEnter, _
                                                                              btnGrabar.MouseEnter, _
                                                                              btnBorrar.MouseEnter, _
                                                                              btnSalir.MouseEnter
        sender.BackColor = System.Drawing.Color.SeaGreen
    End Sub

    Private Sub btnNuevo_MouseLeave(sender As Object, e As EventArgs) Handles btnNuevo.MouseLeave, _
                                                                              btnGrabar.MouseLeave, _
                                                                              btnBorrar.MouseLeave, _
                                                                              btnSalir.MouseLeave
        sender.BackColor = System.Drawing.Color.MidnightBlue
    End Sub
#End Region


    'Private Sub frmBuques_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
    '    Select Case (e.KeyCode)
    '        Case Keys.F1
    '            MsgBox("hola f1")
    '        Case 113
    '            MsgBox("Hola f2")
    '        Case 114
    '            MsgBox("Hola f3")
    '        Case 65
    '            MsgBox("Hola letra a")
    '    End Select
    'End Sub



    Private Sub frmBuques_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Textos()        'Textos Cabecera

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

        '-Campos
        'txtNombre.Enabled = Habilita

        '-Colores
        btnNuevo.BackColor = Color.SeaGreen
        btnBuscar.BackColor = Color.SeaGreen
        btnSalir.BackColor = Color.SeaGreen
        btnGrabar.BackColor = Color.SeaGreen
        btnBorrar.BackColor = Color.SeaGreen
    End Sub

    Private Sub Textos()
        txtId.Text = ""
        txtNombre.Text = ""
        txtViaje.Text = ""
        txtBodega.Text = ""
        DTP_FechaA.Value = Date.Now

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
                dTable = Datos.consulta_reader("SELECT TOP 1 * FROM dbo.buques ORDER BY Id_buq")
            Else
                dTable = Datos.consulta_reader("SELECT * FROM dbo.buques WHERE Id_buq=" & _codigo)
            End If
            Dim nCod As String = ""

            'Setea Campos
            For Each dRow As DataRow In dTable.Rows
                txtId.Text = dRow.Item("Id_buq").ToString
                txtNombre.Text = dRow.Item("nombre_buq").ToString
                txtViaje.Text = dRow.Item("viaje_buq").ToString
                DTP_FechaA.Value = dRow.Item("fechaviaje_buq").ToString
                txtBodega.Text = dRow.Item("Bodegas_buq").ToString
                cmbEstado.Text = dRow.Item("baja_buq").ToString
            Next

            'Asigna Linea Actual
            Me.LineaActual = dTable.Rows.Item(0).Item("Id_buq")

            Me.Grabando = False
            Me.NuevoReg = False

            Navega()
        Catch ex As Exception
            MessageBox.Show("ERROR: " & ex.Message, "Comprobar tabla * buques")
        End Try
    End Sub

    'Para Hacer Navegar
    Private Sub Navega()
        'Contar Lineas
        dTable = Datos.consulta_reader("SELECT Count(1) As TotRec FROM dbo.buques")
        Me.TotLineas = dTable.Rows.Item(0).Item("TotRec")

        'Primera Linea
        dTable = Datos.consulta_reader("SELECT TOP 1 Id_buq FROM dbo.buques ORDER BY Id_buq")
        Me.PrimLinea = dTable.Rows.Item(0).Item("Id_buq")

        'Ultima Linea
        dTable = Datos.consulta_reader("SELECT TOP 1 Id_buq FROM dbo.buques ORDER BY Id_buq DESC")
        Me.UltiLinea = dTable.Rows.Item(0).Item("Id_buq")

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

            'Otiene el ultimo Id y Graba Detalle
            Dim UlId As String = ""

            '--Si el Nuevo e Inserta
            If NuevoReg = True Then

                'Suma Correlativo
                dTable = Datos.consulta_reader("SELECT MAX(id_buq)+1 rMax FROM [dbo].[buques]")

                If dTable.Rows.Count > 0 Then
                    'Setea Campos
                    For Each dRow As DataRow In dTable.Rows
                        'Infomrativos
                        txtId.Text = dRow.Item("rMax").ToString
                    Next
                Else
                    txtId.Text = "1"
                End If

                Try
                    'Se arma el script para grabar SQL
                    Dim StringSql As String = <sqlExp>
                                      INSERT INTO [dbo].[buques]
                                                   ([id_buq],
                                                    [nombre_buq],
                                                    [viaje_buq],
                                                    [fechaviaje_buq],
                                                    [Bodegas_buq],
                                                    [baja_buq], [actualizo_buq])
                                            VALUES('<%= txtId.Text %>',
                                                    '<%= txtNombre.Text %>',
                                                    '<%= txtViaje.Text %>',
                                                    '<%= Format(DTP_FechaA.Value, "yyyyMMdd") %>',
                                                    <%= txtBodega.Text %>,
                                                    '<%= cmbEstado.Text %>',1
                                                    )
                                        </sqlExp>

                    'llama para Grabar Encabezado
                    Datos.consulta_non_query(StringSql)
                Catch ex As Exception
                    MsgBox("Error al Grabar, No Se Grabo Documento !!!" & ex.Message)
                Finally
                    'Limpia la Tabla Temporal
                    'Datos.consulta_non_queryDeta("DELETE FROM [dbo].[buqueviaje_tmp]")
                End Try

            Else
                'Se arma el script para grabar SQL
                Dim StringSql As String = <sqlExp>
                                      UPDATE [dbo].[buques] SET nombre_buq      = '<%= txtNombre.Text %>',
                                                                viaje_buq       = '<%= txtViaje.Text %>',
                                                                fechaviaje_buq  = '<%= Format(DTP_FechaA.Value, "yyyyMMdd") %>',
                                                                Bodegas_buq     = <%= txtBodega.Text %>,
                                                                baja_buq        = '<%= cmbEstado.Text %>',
                                                                actualizo_buq   = 2
                                        WHERE Id_buq = '<%= txtId.Text %>' 
                                        AND actualizo_buq IN(1,2)
                                  </sqlExp>.Value

                'LLamamos la rutina para grabar
                Datos.consulta_non_query(StringSql)

                UlId = txtId.Text
            End If


            'Refrescamos los dato de pantalla
            Botonos(True)
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

        'Limpia la tabla Temporal al Volver a Entrar si hay datos de la transaccion aterior
        Datos.consulta_non_queryDeta("DELETE FROM [dbo].[buqueviaje_tmp]")

        dLinea = 0
        'GridCabecera()
        'TextosDeta()
    End Sub

    Private Sub btnBorrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBorrar.Click

        'Si es modo nuevo o Editar
        If Me.Grabando Then
            Consulta("")
            Botonos(True)
        Else
            'LLamamos la rutina Eliminar
            Datos.consulta_non_query("DELETE FROM [dbo].[[buqueviaje] WHERE Id_buq='" & txtId.Text & "'")
            Datos.consulta_non_query("DELETE FROM [dbo].[buques] WHERE Id_buq='" & txtId.Text & "'")

            'Refrescamos los dato de pantalla
            Consulta("")
            Botonos(True)
        End If

        txtNombre.Focus()
    End Sub

    'Despliega pantalla para buscar el Cajero
    Private Sub BuscaDocumento()
        Dim stringSQL As String

        stringSQL = "SELECT Id_buq As Codigo, nombre_buq As Nombre FROM [dbo].[buques]"

        'Script para Filtrar el Grid
        Dim stringSqlFiltro As String = "SELECT Id_buq As Codigo, nombre_buq As Nombre FROM [dbo].[Buques] WHERE Id_buq "

        'Instancia Ayuda Producto
        'Dim frmBuscar As New BuscaCodigo(stringSQL, stringSqlFiltro, "Productos")
        'frmBuscar.ShowDialog()

        Dim frmBuscar As New BuscaCodigo(stringSQL, {"Código", "Nombre", "50", "200"}, 2, stringSqlFiltro, "Buques")
        frmBuscar.ShowDialog()

        'Retorna los datos del Cajero selecionado y 
        'se posiciona en Serie Recibo
        txtId.Text = ""
        If frmBuscar.Codigo <> String.Empty Then
            Consulta(frmBuscar.Codigo)
        End If
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        BuscaDocumento()
    End Sub

#End Region

#Region "Botones Navegar"
    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnprimero.Click
        'Para Inicio
        dTable = Datos.consulta_reader("SELECT TOP 1 Id_buq FROM [dbo].[buques] ORDER BY Id_buq")

        For Each dRow As DataRow In dTable.Rows
            Me.LineaActual = dRow("Id_buq")

            Consulta(dRow("Id_buq").ToString)
        Next
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnanterior.Click
        'Para Anterior
        dTable = Datos.consulta_reader("SELECT TOP 1 Id_buq FROM [dbo].[buques] WHERE Id_buq <" & Me.LineaActual & "ORDER BY Id_buq DESC")

        For Each dRow As DataRow In dTable.Rows
            Me.LineaActual = dRow("Id_buq")

            Consulta(dRow("Id_buq").ToString)
        Next
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnsiguiente.Click
        'Para Sigiente
        dTable = Datos.consulta_reader("SELECT TOP 1 Id_buq FROM[dbo].[buques] WHERE Id_buq >" & Me.LineaActual & "ORDER BY Id_buq")

        For Each dRow As DataRow In dTable.Rows
            Me.LineaActual = dRow("Id_buq")

            Consulta(dRow("Id_buq").ToString)
        Next
    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnultimo.Click
        'Para Inicio
        dTable = Datos.consulta_reader("SELECT TOP 1 Id_buq FROM [dbo].[buques] ORDER BY Id_buq DESC")

        For Each dRow As DataRow In dTable.Rows
            Me.UltiLinea = dRow("Id_buq")
            Me.LineaActual = dRow("Id_buq")

            Consulta(dRow("Id_buq").ToString)
        Next

    End Sub


#End Region

End Class