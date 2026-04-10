Public Class frmTurnos

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

    Private Sub frmTurnos_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Consulta("")
        Botonos(True)

        DTP_Hora.Format = DateTimePickerFormat.Time
        DTP_HoraFin.Format = DateTimePickerFormat.Time

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
        txtNumero.Text = ""
        txtNombre.Text = ""
        cmbEstatus.Text = "ACTIVO"

        DTP_Hora.Format = DateTimePickerFormat.Time
        DTP_HoraFin.Format = DateTimePickerFormat.Time
        'Se posiciona para Iniciar 
        txtNumero.Focus()
    End Sub

#End Region

#Region "Consulta"
    'Procedimiento para mostrar el Primer Registro
    Private Sub Consulta(ByVal _codigo As String)
        Try
            'Para mostrar el Dato
            If _codigo = String.Empty Then
                dTable = Datos.consulta_reader("SELECT TOP 1 * FROM dbo.Turnos ORDER BY id_tur")
            Else
                dTable = Datos.consulta_reader("SELECT * FROM dbo.Turnos WHERE id_tur=" & _codigo)
            End If

            Dim nCod As String = ""

            'Setea Campos
            For Each dRow As DataRow In dTable.Rows
                txtId.Text = dRow.Item("id_tur").ToString
                txtNumero.Text = dRow.Item("numero_tur").ToString
                txtNombre.Text = dRow.Item("nombre_tur").ToString
                'txtContacto.Text = dRow.Item("contacto_cli").ToString
                DTP_Hora.Value = dRow.Item("horaIni_tur").ToString
                DTP_HoraFin.Value = dRow.Item("horaFin_tur").ToString
                cmbEstatus.Text = dRow.Item("baja_tur").ToString
            Next

            'Asigna Linea Actual
            Me.LineaActual = dTable.Rows.Item(0).Item("id_tur")

            Me.Grabando = False
            Me.NuevoReg = False
            Navega()

        Catch ex As Exception
            MessageBox.Show("ERROR: " & ex.Message, "Comprobar tabla * Turnos")
        End Try
    End Sub

    'Para Hacer Navegar
    Private Sub Navega()
        'Contar Lineas
        dTable = Datos.consulta_reader("SELECT Count(1) As TotRec FROM dbo.Turnos")
        Me.TotLineas = dTable.Rows.Item(0).Item("TotRec")

        'Primera Linea
        dTable = Datos.consulta_reader("SELECT TOP 1 id_tur FROM dbo.Turnos ORDER BY id_tur")
        Me.PrimLinea = dTable.Rows.Item(0).Item("id_tur")

        'Ultima Linea
        dTable = Datos.consulta_reader("SELECT TOP 1 id_tur FROM dbo.Turnos ORDER BY id_tur DESC")
        Me.UltiLinea = dTable.Rows.Item(0).Item("id_tur")

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
                dTable = Datos.consulta_reader("SELECT MAX(id_tur)+1 rMax FROM [dbo].[Turnos]")

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
                                    INSERT INTO [dbo].[Turnos] ([id_tur], [numero_tur],
                                                                [nombre_tur],
                                                                [horaIni_tur],
                                                                [horaFin_tur],
                                                                [baja_tur])
                                                     VALUES('<%= txtId.Text %>', '<%= txtNumero.Text %>',
                                                          '<%= txtNombre.Text %>',
                                                           '<%= Format(DTP_Hora.Value, "HH:mm:ss") %>',
                                                           '<%= Format(DTP_HoraFin.Value, "HH:mm:ss") %>',
                                                          '<%= cmbEstatus.Text %>')
                                     </sqlExp>.Value

                'LLamamos la rutina para grabar
                Datos.consulta_non_query(StringSql)

            Else
                'Se arma el script para grabar SQL
                Dim StringSql As String = <sqlExp>
                                     UPDATE [dbo].[Turnos] SET [numero_tur] = '<%= txtNumero.Text %>',
                                                                [nombre_tur] =  '<%= txtNombre.Text %>',
                                                                [horaIni_tur] = '<%= Format(DTP_Hora.Value, "HH:mm:ss") %>',
                                                                [horaFin_tur] = '<%= Format(DTP_HoraFin.Value, "HH:mm:ss") %>',
                                                                [baja_tur] =    '<%= cmbEstatus.Text %>'
                                      WHERE id_tur = '<%= txtId.Text %>' 
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
        ElseIf txtNumero.Text = String.Empty Then
            MessageBox.Show("ERROR: El Nit no puede ser Blanco")
            txtNumero.Focus()
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
            Datos.consulta_non_query("DELETE FROM [dbo].[Turnos] WHERE id_tur='" & txtId.Text & "'")

            'Refrescamos los dato de pantalla
            Consulta("")
            Botonos(True)
        End If
        'txtDescripcion.Focus()
    End Sub

    'Despliega pantalla para buscar el Cajero
    Private Sub BuscaCodigo()
        Dim stringSQL As String

        stringSQL = "SELECT id_tur As Codigo, nombre_tur As Nombre, numero_tur FROM [dbo].[Turnos]"

        'Script para Filtrar el Grid
        Dim stringSqlFiltro As String = "SELECT id_tur As Codigo, nombre_tur As Nombre FROM [dbo].[Turnos] WHERE id_tur "

        'Instancia Ayuda Turnos
        'Dim frmBuscar As New BuscaCodigo(stringSQL, stringSqlFiltro, "Turnoss")
        'frmBuscar.ShowDialog()

        Dim frmBuscar As New BuscaCodigo(stringSQL, {"Código", "Nombre", "Número", "50", "180", "200"}, 3, stringSqlFiltro, "Turnos")
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
        dTable = Datos.consulta_reader("SELECT TOP 1 id_tur FROM dbo.Turnos ORDER BY id_tur")

        For Each dRow As DataRow In dTable.Rows
            Me.LineaActual = dRow("id_tur")

            Consulta(dRow("id_tur").ToString)
        Next
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnanterior.Click
        'Para Anterior
        dTable = Datos.consulta_reader("SELECT TOP 1 id_tur FROM dbo.Turnos WHERE id_tur <" & Me.LineaActual & "ORDER BY id_tur DESC")

        For Each dRow As DataRow In dTable.Rows
            Me.LineaActual = dRow("id_tur")

            Consulta(dRow("id_tur").ToString)
        Next
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnsiguiente.Click
        'Para Sigiente
        dTable = Datos.consulta_reader("SELECT TOP 1 id_tur FROM dbo.Turnos WHERE id_tur >" & Me.LineaActual & "ORDER BY id_tur")

        For Each dRow As DataRow In dTable.Rows
            Me.LineaActual = dRow("id_tur")

            Consulta(dRow("id_tur").ToString)
        Next
    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnultimo.Click
        'Para Inicio
        dTable = Datos.consulta_reader("SELECT TOP 1 id_tur FROM dbo.Turnos ORDER BY id_tur DESC")

        For Each dRow As DataRow In dTable.Rows
            Me.UltiLinea = dRow("id_tur")
            Me.LineaActual = dRow("id_tur")

            Consulta(dRow("id_tur").ToString)
        Next

    End Sub
#End Region

    Private Sub DTP_Hora_ValueChanged(sender As Object, e As EventArgs) Handles DTP_Hora.ValueChanged

    End Sub
End Class