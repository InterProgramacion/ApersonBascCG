Public Class frmCorrealtivo

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
    Dim TipoCorActual As String = ""
#End Region

#Region "Ento"

    Private Sub frmCorrealtivo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Consulta("")
        Botonos(True)
    End Sub

    'Sub Modulo Botonos
    'Sirver para Muestra o Oculta los Botonos
    Private Sub Botonos(ByVal Habilita As Boolean)
        '-Botonos
        btnSalir.Visible = Habilita
        btnGrabar.Text = "actualizar"
        
        '-Campos
        txtId.Enabled = False
    End Sub

    
#End Region

#Region "Consulta"
    'Procedimiento para mostrar el Primer Registro
    Private Sub Consulta(ByVal _codigo As String)
        Try
            'Para mostrar el Dato
            If _codigo = String.Empty Then
                dTable = Datos.consulta_reader("SELECT TOP 1 * FROM dbo.correlactu WHERE empresa_cor = " & LibreriaGeneral.idEmpresa & " ORDER BY Id_cor")
            Else
                dTable = Datos.consulta_reader("SELECT * FROM dbo.correlactu WHERE Id_cor=" & _codigo)
            End If

            Dim nCod As String = ""

            'Setea Campos
            For Each dRow As DataRow In dTable.Rows
                txtId.Text = dRow.Item("Id_cor").ToString
                txtTicket.Text = dRow.Item("nTicketBoleta_cor").ToString
                DTP_Hora.Value = dRow.Item("nFechaCorrelativo_cor").ToString
                If Not IsDBNull(dRow.Item("prefijo_cor")) Then txtPrefijo.Text = dRow.Item("prefijo_cor").ToString.Trim()
                If Not IsDBNull(dRow.Item("tipo_cor")) Then txtTipo.Text = dRow.Item("tipo_cor").ToString.Trim()
                If Not IsDBNull(dRow.Item("estatus_cor")) Then txtEstatus.Text = dRow.Item("estatus_cor").ToString.Trim()
                TipoCorActual = If(IsDBNull(dRow.Item("tipo_cor")), "", dRow.Item("tipo_cor").ToString.Trim())
            Next

        Catch ex As Exception
            MessageBox.Show("ERROR: " & ex.Message, "Comprobar tabla * Correalativo")
        End Try
    End Sub
#End Region

#Region "Grabar"
    Private Sub btnGrabar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGrabar.Click

        Dim Fechahora As DateTime = DTP_Hora.Value

        Dim fechahoraFormat As String = Fechahora.ToString("yyyyMMdd H:mm:ss")

        If ValidaCampos() Then 'Si es verdadero
            'Se arma el script para grabar SQL
            Dim StringSql As String = <sqlExp>
                                         UPDATE [dbo].[correlactu] SET [nTicketBoleta_cor] = '<%= txtTicket.Text %>',
                                                                   [nFechaCorrelativo_cor] = '<%= fechahoraFormat %>',
                                                                   [prefijo_cor] = '<%= txtPrefijo.Text.Trim() %>',
                                                                   [estatus_cor] = '<%= txtEstatus.Text.Trim() %>'
                                          WHERE Id_cor = '<%= txtId.Text %>'
                                          AND empresa_cor = <%= LibreriaGeneral.idEmpresa %>
                                          AND tipo_cor = '<%= TipoCorActual %>'
                                      </sqlExp>.Value

            'LLamamos la rutina para grabar
            Datos.consulta_non_query(StringSql)

            Me.Close()
        End If
    End Sub

    Private Function ValidaCampos() As Boolean
        Dim Si As Boolean = False

        If txtTicket.Text = String.Empty Then
            MessageBox.Show("ERROR: El Ticket no puede quedar en Blanco")
            txtTicket.Focus()
        ElseIf txtPrefijo.Text.Trim() = String.Empty Then
            MessageBox.Show("ERROR: El Prefijo no puede quedar en Blanco")
            txtPrefijo.Focus()
        ElseIf txtEstatus.Text.Trim() = String.Empty Then
            MessageBox.Show("ERROR: El Estatus no puede quedar en Blanco")
            txtEstatus.Focus()
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

    'Despliega pantalla para buscar el Cajero
    Private Sub BuscaCodigo()
        Dim stringSQL As String

        stringSQL = "SELECT Id_cor As Codigo, nTicketBoleta_cor As Nombre FROM [dbo].[correlactu] WHERE empresa_cor = " & LibreriaGeneral.idEmpresa

        'Script para Filtrar el Grid
        Dim stringSqlFiltro As String = "SELECT Id_cor As Codigo, nTicketBoleta_cor As Nombre FROM [dbo].[correlactu] WHERE empresa_cor = " & LibreriaGeneral.idEmpresa & " AND Id_cor "

        Dim frmBuscar As New BuscaCodigo(stringSQL, {"Código", "No. Boleta", "50", "100"}, 2, stringSqlFiltro, "Correaltivo Boletas")
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

End Class