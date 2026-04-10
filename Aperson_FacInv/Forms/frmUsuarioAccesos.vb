Public Class frmUsuarioAccesos
    Dim dv As New DataView
    Dim DatosB As New DatosGenereral()
    Dim stringSQL As String
    Dim cLogUsu As String
    Dim cNuevo As Boolean = True
    Dim dTable2 As New DataTable


    Sub New(ByVal _cLogUsu As String)
        InitializeComponent()

        cLogUsu = _cLogUsu
    End Sub

    Private Sub frmUsuarioAccesos_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        LeerData(grdData)
    End Sub

#Region "DataGrid"
    Public Sub LeerData(ByRef grdData As DataGridView)
        Dim dTable As New DataTable

        Try

            dTable2 = DatosB.consulta_reader("SELECT * FROM dbo.usuarioAccesos WHERE Log_usu='" & cLogUsu & "'")

            If dTable2.Rows.Count = 0 Then
                '-- Si es nuevo

                Me.cNuevo = True

                'Arma Select para Grid
                dv = DatosB.consulta_dv("SELECT codigo_opt, descripcion_opt FROM dbo.opcionMenu ORDER BY codigo_opt")

                grdData.DataSource = dv

                grdData.SelectionMode = DataGridViewSelectionMode.FullRowSelect  'Marca toda la Linea en el DataGridView
                grdData.MultiSelect = False  'Solo seleccinar una fila

                'Encabezado del Grid
                grdData.Columns(0).HeaderText = "CODIGO"
                grdData.Columns(1).HeaderText = "DESCRIPCION MENU"

                'Ancho de las columnas
                grdData.Columns(0).Width = 100
                grdData.Columns(1).Width = 260


                'Aca se agrega la columna tipo checkbox
                Dim dgvcolumncheck As New DataGridViewCheckBoxColumn()
                grdData.Columns.Add(dgvcolumncheck)

                dgvcolumncheck.HeaderText = "ACCESO"
                dgvcolumncheck.Name = "chk"
                'grdData.Columns(4).v

                'Accesos a las Columnas
                grdData.Columns(0).ReadOnly = True
                grdData.Columns(1).ReadOnly = True
                grdData.Columns(2).ReadOnly = False
                grdData.ScrollBars = ScrollBars.Both

                'Los pongo todos como True para seleccionar los que no queremos
                For i As Integer = 0 To grdData.Rows.Count - 1
                    grdData.Rows(i).Cells(2).Value = True
                Next

            Else
                'Si es Edicion
                Me.cNuevo = False

                'Arma Select para Grid
                dv = DatosB.consulta_dv("SELECT a.codigo_usu, b.descripcion_opt, a.Acceso FROM dbo.usuarioAccesos a LEFT JOIN dbo.opcionMenu b ON b.codigo_opt = a.codigo_usu WHERE Log_usu ='" & cLogUsu & "' ORDER BY codigo_usu")
                grdData.DataSource = dv
                grdData.SelectionMode = DataGridViewSelectionMode.FullRowSelect  'Marca toda la Linea en el DataGridView
                grdData.MultiSelect = False  'Solo seleccinar una fila

                'Encabezado del Grid
                grdData.Columns(0).HeaderText = "CODIGO"
                grdData.Columns(1).HeaderText = "DESCRIPCION MENU"
                grdData.Columns(2).HeaderText = "ACCESO"

                'Ancho de las columnas
                grdData.Columns(0).Width = 100
                grdData.Columns(1).Width = 260

                'Accesos a las Columnas
                grdData.Columns(0).ReadOnly = True
                grdData.Columns(1).ReadOnly = True
                grdData.Columns(2).ReadOnly = False
                grdData.ScrollBars = ScrollBars.Both

            End If

        Catch ex As Exception
            MessageBox.Show("Error " & ex.Message)
        End Try

        grdData.ScrollBars = ScrollBars.Both

        ''Posición Primera Linea
        grdData.Rows(0).Selected = True
        grdData.CurrentCell = grdData.Rows(0).Cells(0)

        'Me.txtBuscar.Focus()
    End Sub

#End Region

#Region "Botones"
    Private Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click
        Me.Close()
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click

        Dim Usu As String = Me.cLogUsu
        Dim codigoItem As String
        Dim AccesosSi As Integer
        Dim StringSql As String = ""
        Dim x As Integer = 0
        Dim dTablea As New DataTable


        Try

            StringSql = <sqlExp>
                                 DELETE FROM [dbo].[usuarioAccesos]  WHERE [Log_usu] ='<%= Usu %>'
                          </sqlExp>.Value

            'LLamamos la rutina para grabar
            DatosB.consulta_non_queryDeta(StringSql)

            'Setea Campos
            For I As Integer = 0 To grdData.RowCount - 1

                dTablea = DatosB.consulta_reader("SELECT MAX(Id_usu)+1 rMax FROM [dbo].[usuarioAccesos]")

                If dTablea.Rows.Count > 0 Then
                    'Setea Campos
                    For Each dRow As DataRow In dTablea.Rows
                        'Infomrativos
                        x = dRow.Item("rMax").ToString
                    Next
                Else
                    x = "1"
                End If


                codigoItem = grdData.Rows(I).Cells(0).Value.ToString.Trim
                AccesosSi = IIf(grdData.Rows(I).Cells(2).Value = True, 1, 0)

                'Se arma el script para grabar SQL
                StringSql = <sqlExp>
                                        INSERT INTO [dbo].[usuarioAccesos] (
	                                                            [id_usu],
                                                                [Log_usu],
                                                                [codigo_usu],
                                                                [acceso])
                                                    VALUES('<%= x %>',
                                                          '<%= Usu %>',
                                                          '<%= codigoItem %>',
                                                          <%= AccesosSi %>)
                                    </sqlExp>.Value

                'LLamamos la rutina para grabar
                DatosB.consulta_non_queryDeta(StringSql)
            Next

            MsgBox("La operacion se realizo con exito!", MsgBoxStyle.Information, "Operacion exitosa!")

        Catch ex As Exception
            MsgBox("Error al tratar de grabar Accesos!", MsgBoxStyle.Information, "Error al Grabar!")
        Finally

            Me.Close()
        End Try
    End Sub
#End Region

End Class