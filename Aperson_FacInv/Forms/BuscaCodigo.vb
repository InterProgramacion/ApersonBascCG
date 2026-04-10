Public Class BuscaCodigo
    Dim dv As New DataView
    Dim DatosB As New DatosGenereral()

    Dim stringSQL As String
    Dim ContColumnas() As String
    Dim nColumn As Integer
    Dim SQLFiltro As String
    Dim titulo As String

    Sub New(_stringSQL As String, _Columns() As String, _nColumn As Integer, _SqlFiltro As String, _Titulo As String)
        InitializeComponent()
        ContColumnas = _Columns
        nColumn = _nColumn
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
        Dim Diccionario As New Dictionary(Of String, Object)

        Me.txtBuscar.Text = ""

        'Arma Select para Grid
        dv = DatosB.consulta_dv(stringSQL)
        grdData.DataSource = dv

        grdData.SelectionMode = DataGridViewSelectionMode.FullRowSelect  'Marca toda la Linea en el DataGridView
        grdData.MultiSelect = False  'Solo seleccinar una fila

        For i As Integer = 0 To nColumn
            grdData.Columns(i).HeaderText = ContColumnas(i)
            grdData.Columns(i).Width = CInt(ContColumnas(i + nColumn))

            'Le quito una colunma (porque inicia con Cero el array)
            If nColumn - i = 1 Then
                i = nColumn
            End If
        Next i

        grdData.ReadOnly = True
        grdData.ScrollBars = ScrollBars.Both

        'Posición Primera Linea
        grdData.Rows(0).Selected = True
        grdData.CurrentCell = grdData.Rows(0).Cells(0)

        Me.txtBuscar.Focus()

    End Sub
    'Filtrar Grid
    Private Sub txtBuscar_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBuscar.TextChanged
        dv.RowFilter = String.Format("Nombre Like '%{0}%'", txtBuscar.Text)
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