Imports System.IO
Imports System.Data
Public Class frmCarga_Placas
#Region "Variables"
    Dim Datos As New DatosGenereral()
    Dim dTable As New DataTable
    Dim dTGraba As DataTable


    Dim id As String = ""
    Dim Placa As String = ""
    Dim Modelo As String = ""
    Dim Color As String = ""
    Dim Numero As String = ""
    Dim Transporte As String = ""

#End Region

    Private Sub BtnCargarCSV_Click(sender As Object, e As EventArgs) Handles BtnCargarCSV.Click
        ' Mostrar el cuadro de diálogo para seleccionar un archivo CSV
        Dim openFileDialog As New OpenFileDialog With {
            .Filter = "Archivos CSV (*.csv)|*.csv",
            .Title = "Seleccionar archivo CSV"
        }

        If openFileDialog.ShowDialog() = DialogResult.OK Then
            Dim csvPath As String = openFileDialog.FileName
            Dim dt As DataTable = CargarCSV(csvPath)
            If dt IsNot Nothing Then
                grdData.DataSource = dt ' Mostrar datos en el DataGridView


                grdData.SelectionMode = DataGridViewSelectionMode.FullRowSelect  'Marca toda la Linea en el DataGridView
                grdData.MultiSelect = False  'Solo seleccinar una fila
                grdData.AllowUserToDeleteRows = False
                grdData.ScrollBars = ScrollBars.Vertical

                'Encabezado del Grid
                grdData.Columns(0).HeaderText = "PLACA"
                grdData.Columns(1).HeaderText = "MODELO"
                grdData.Columns(2).HeaderText = "COLOR"
                grdData.Columns(3).HeaderText = "CODIGO TRANSPORTE"
                grdData.Columns(4).HeaderText = "NUMERO"

                'Encabezado del Grid Centrado
                For Each column As DataGridViewColumn In grdData.Columns
                    column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                Next

                'Ancho de las columnas
                grdData.Columns(0).Width = 100
                grdData.Columns(1).Width = 80
                grdData.Columns(2).Width = 150
                grdData.Columns(3).Width = 100
                grdData.Columns(4).Width = 80

                grdData.ReadOnly = True
                grdData.ScrollBars = ScrollBars.Both

                If dt.Rows.Count > 0 Then
                    ''Posición Primera Linea
                    grdData.Rows(0).Selected = True
                    grdData.CurrentCell = grdData.Rows(0).Cells(0)
                End If

                btnGrabar.Visible = True
            End If
        End If


    End Sub

    Private Function DetectarDelimitador(rutaArchivo As String) As Char
        ' Leer la primera línea para determinar el delimitador
        Using sr As New StreamReader(rutaArchivo)
            Dim primeraLinea As String = sr.ReadLine()
            If primeraLinea IsNot Nothing Then
                If primeraLinea.Contains(";") Then
                    Return ";"c ' Usar punto y coma
                ElseIf primeraLinea.Contains(",") Then
                    Return ","c ' Usar coma
                End If
            End If
        End Using
        Return ","c ' Predeterminado: coma
    End Function

    Private Function CargarCSV(rutaArchivo As String) As DataTable
        Dim dt As New DataTable()

        Try
            ' Detectar el delimitador automáticamente
            Dim delimitador As Char = DetectarDelimitador(rutaArchivo)

            Using sr As New StreamReader(rutaArchivo)
                Dim primeraLinea As Boolean = True
                While Not sr.EndOfStream
                    Dim linea As String = sr.ReadLine().Trim() ' Quitar espacios en blanco de la línea

                    ' Omitir líneas vacías
                    If String.IsNullOrWhiteSpace(linea) Then Continue While

                    Dim valores As String() = linea.Split(delimitador) ' Usar delimitador detectado

                    If primeraLinea Then
                        ' Crear columnas con los nombres del encabezado
                        For Each valor As String In valores
                            dt.Columns.Add(valor.Trim())
                        Next
                        primeraLinea = False
                    Else
                        dt.Rows.Add(valores)
                    End If
                End While
            End Using
        Catch ex As Exception
            MessageBox.Show("Error al cargar el archivo CSV: " & ex.Message)
            Return Nothing
        End Try

        Return dt
    End Function

    Private Sub btnGrabar_Click(sender As Object, e As EventArgs) Handles btnGrabar.Click

        'For Each dRow As DataGridView In grdData

        'Dim dtG As DataTable

        'Lipar Data
        If dTGraba IsNot Nothing Then
            dTGraba.Clear()
        Else
            ' Inicializa la tabla si es necesario
            dTGraba = New DataTable()
        End If

        dTGraba.Columns.Add("Campo")
        dTGraba.Columns.Add("Valor")
        dTGraba.Columns.Add("Explicacion")

        For Each Fila As DataGridViewRow In grdData.Rows

            If Not Fila.IsNewRow Then
                'ceEvaluacion.CursoAlumno = Fila.Cells(0).Value
                Placa = If(Fila.Cells(0).Value IsNot Nothing, Fila.Cells(0).Value.ToString().Trim, String.Empty)
                Modelo = If(Fila.Cells(1).Value IsNot Nothing, Fila.Cells(1).Value.ToString().Trim, String.Empty)
                Color = If(Fila.Cells(2).Value IsNot Nothing, Fila.Cells(2).Value.ToString().Trim, String.Empty)
                Transporte = If(Fila.Cells(3).Value IsNot Nothing, Fila.Cells(3).Value.ToString().Trim, String.Empty)
                Numero = If(Fila.Cells(4).Value IsNot Nothing, Fila.Cells(4).Value.ToString().Trim, String.Empty)

                If ValidaCampos() Then 'Si es verdadero

                    'Suma Correlativo
                    dTable = Datos.consulta_reader("SELECT MAX(id_fca)+1 rMax FROM [dbo].[fcamiones]")

                    If dTable.Rows.Count > 0 Then
                        'Setea Campos
                        For Each dRow As DataRow In dTable.Rows
                            'Infomrativos
                            id = dRow.Item("rMax").ToString
                        Next
                    Else
                        id = "1"
                    End If


                    'Se arma el script para grabar SQL
                    Dim StringSql As String = <sqlExp>
                        INSERT INTO [dbo].[fcamiones] ([id_fca],
                                                       [placa_fca],
                                                       [model_fca],
                                                       [color_fca],
                                                       [numero_fca],
                                                       [transporte_fca],
                                                       [baja_fca])
                                                VALUES('<%= id %>', 
                                                    '<%= Placa %>',
                                                    '<%= Modelo %>',
                                                    '<%= Color %>',
                                                    '<%= Numero %>',
                                                    '<%= Transporte %>',
                                                    '<%= "ACTIVO" %>')
                                </sqlExp>.Value

                    'LLamamos la rutina para grabar
                    Datos.consulta_non_queryDeta(StringSql)
                End If
            End If
        Next

        If dTGraba IsNot Nothing Then
            grdDataError.DataSource = dTGraba   'Mostrar datos en el DataGridView


            grdDataError.SelectionMode = DataGridViewSelectionMode.FullRowSelect  'Marca toda la Linea en el DataGridView
            grdDataError.MultiSelect = False  'Solo seleccinar una fila
            grdDataError.AllowUserToDeleteRows = False
            grdDataError.ScrollBars = ScrollBars.Vertical

            'Encabezado del Grid
            grdDataError.Columns(0).HeaderText = "CAMPO"
            grdDataError.Columns(1).HeaderText = "VALOR"
            grdDataError.Columns(2).HeaderText = "EXPLICACION"

            'Encabezado del Grid Centrado
            For Each column As DataGridViewColumn In grdDataError.Columns
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            Next

            'Ancho de las columnas
            grdDataError.Columns(0).Width = 100
            grdDataError.Columns(1).Width = 100
            grdDataError.Columns(2).Width = 250

            grdDataError.ReadOnly = True
            grdDataError.ScrollBars = ScrollBars.Both

            TabControl1.SelectedTab = TabPage2
        End If

        MessageBox.Show("Proceso Terminado, Revise Errores!! ")

    End Sub


    Private Function ValidaCampos() As Boolean
        Dim Si As Boolean = False

        If Placa = String.Empty Then
            Si = False
            dTGraba.Rows.Add("Placa", Placa, "Placa No Puede quedar Blanco")
        ElseIf Modelo = String.Empty Then
            Si = False
            dTGraba.Rows.Add("Modelo", Modelo, "Modelo No Puede quedar Blanco")
        Else
            Si = True
        End If

        If Si = True Then
            'Suma Correlativo
            dTable = Datos.consulta_reader("select placa_fca from [dbo].[fcamiones] where rtrim(ltrim(placa_fca)) = '" & Placa.Trim() & "'")

            If dTable.Rows.Count > 0 Then
                Si = False
                dTGraba.Rows.Add("Placa", Placa, "Placa ya Existe!")
            Else
                Si = True
            End If
        End If

        Return Si
    End Function

    Private Sub frmCarga_Placas_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnGrabar.Visible = False
    End Sub

    Private Sub btnSalir_Click(sender As Object, e As EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub
End Class