Public Class BuscaCodigoA
    Dim dv As New DataView
    Dim DatosB As New DatosGenereral()
    Dim stringSQL As String
    Dim ContColumnas() As String
    Dim nColumn As Integer
    Dim SQLFiltro As String
    Dim titulo As String

    Sub New(ByVal _stringSQL As String, ByVal _SqlFiltro As String, ByVal _Titulo As String)
        InitializeComponent()
        'ContColumnas = {"Código","Nombre","50","250"}
        nColumn = 2
        stringSQL = _stringSQL
        SQLFiltro = _SqlFiltro
        titulo = _Titulo
    End Sub

#Region "Variables Retorno"
    Private _Codigo As String
    Public ReadOnly Property Codigo() As String
        Get
            Return _Codigo
        End Get
    End Property

    Private _Nombre As String
    Public ReadOnly Property Nombre() As String
        Get
            Return _Nombre
        End Get
    End Property
#End Region

    Private Sub frmBuscaCliente_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Me.Text = Me.Text & " " & titulo
        GrupoDatosBuscar.Text = GrupoDatosBuscar.Text & " " & titulo

        btnNuevo.Visible = False
        LeerData(grdData)

    End Sub

#Region "DataGrid"
    Public Sub LeerData(ByRef grdData As DataGridView)
        Dim dTable As New DataTable

        Me.txtBuscar.Text = ""

        'Arma Select para Grid
        dv = DatosB.consulta_dv(stringSQL)

        grdData.DataSource = dv

        grdData.SelectionMode = DataGridViewSelectionMode.FullRowSelect  'Marca toda la Linea en el DataGridView
        grdData.MultiSelect = False  'Solo seleccinar una fila

        'Encabezado del Grid
        grdData.Columns(0).HeaderText = "Boleta"
        grdData.Columns(1).HeaderText = "Placa"
        grdData.Columns(2).HeaderText = "Tipo Pesaje"

        'Ancho de las columnas
        grdData.Columns(0).Width = 150
        grdData.Columns(1).Width = 100
        grdData.Columns(2).Width = 150


        grdData.ReadOnly = True
        grdData.ScrollBars = ScrollBars.Both

        ''Posición Primera Linea
        grdData.Rows(0).Selected = True
        grdData.CurrentCell = grdData.Rows(0).Cells(0)

        Me.txtBuscar.Focus()
    End Sub

    'Filtrar Grid
    Private Sub txtBuscar_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBuscar.TextChanged
        dv.RowFilter = String.Format("Codigo Like '%{0}%'", txtBuscar.Text)
    End Sub
    Private Sub txtBuscar2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBuscar2.TextChanged
        dv.RowFilter = String.Format("Nombre Like '%{0}%'", txtBuscar2.Text)
    End Sub

    'Selecionar Fila en Grid
    Private Sub grdData_DoubleClick(ByVal sender As Object, ByVal e As EventArgs) Handles grdData.DoubleClick, btnNuevo.Click

        Dim dRow As DataGridViewSelectedCellCollection = grdData.SelectedCells
        Dim dTable As New DataTable

        Try
            dTable = DatosB.consulta_reader(SQLFiltro & "='" & dRow.Item(0).Value & "'")

            For Each Row As DataRow In dTable.Rows
                _Codigo = Row.Item("Codigo")
                _Nombre = Row.Item("Nombre")
            Next Row

            'Cierra el form
            dTable = Nothing
            Me.Close()

            Return
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    '
#End Region

#Region "Botones"
    Private Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click
        Me.Close()
    End Sub

    Private Sub grdData_CellClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles grdData.CellClick
        btnNuevo.Visible = True
    End Sub

    Private Sub txtBuscar_GotFocus(ByVal sender As Object, ByVal e As EventArgs) Handles txtBuscar.GotFocus
        btnNuevo.Visible = False
    End Sub
#End Region


End Class