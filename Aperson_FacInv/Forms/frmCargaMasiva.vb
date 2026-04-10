Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports Microsoft.ReportingServices.Diagnostics.Internal
Imports OfficeOpenXml

Public Class frmCargaMasiva

    Dim Datos As New DatosGenereral()
    ' Variable para almacenar los datos originales del Excel
    Private datosEgresoParaGuardar As List(Of Dictionary(Of String, Object)) = Nothing
    Private tipoHojaCargada As String = ""
    Private Sub btnSeleccionar_Click(sender As Object, e As EventArgs) Handles btnSeleccionar.Click
        Dim ofd As New OpenFileDialog()
        ofd.Filter = "Archivos Excel|*.xlsx;*.xls"
        If ofd.ShowDialog() = DialogResult.OK Then
            txtRuta.Text = ofd.FileName
        End If
    End Sub

    Private Sub btnCargar_Click(sender As Object, e As EventArgs) Handles btnCargar.Click
        If String.IsNullOrWhiteSpace(txtRuta.Text) Then
            MessageBox.Show("Seleccione un archivo primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Try
            Dim fileInfo As New FileInfo(txtRuta.Text)
            Using package As New ExcelPackage(fileInfo)
                ' Buscar las hojas disponibles
                Dim hojaEgreso As ExcelWorksheet = Nothing

                For Each ws As ExcelWorksheet In package.Workbook.Worksheets
                    If ws.Name.ToUpper().Contains("EGRESO") Then
                        hojaEgreso = ws
                    End If
                Next

                ' Preguntar al usuario qué hoja desea cargar
                Dim resultado As DialogResult = MessageBox.Show("¿Desea cargar la hoja EGRESO?" & vbCrLf &
                    "Sí", "Seleccionar Opcion",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question)

                If resultado = DialogResult.Cancel Then
                    Return
                End If

                If resultado = DialogResult.OK Then
                    lblCargando.Visible = True
                    lblCargando.BringToFront()
                    lblCargando.Refresh()
                    Application.DoEvents()
                    If hojaEgreso IsNot Nothing Then
                        LeerDatosEgresoEnMemoria(hojaEgreso)
                        CargarYAgruparEgreso(hojaEgreso)
                        tipoHojaCargada = "EGRESO"
                    Else
                        MessageBox.Show("No se encontró la hoja EGRESO.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If

                End If

            End Using

        Catch ex As Exception
            MessageBox.Show("Error al leer el archivo: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If lblCargando.Visible Then
                lblCargando.Visible = False
            End If
        End Try
    End Sub

    Private Sub CargarYAgruparIngreso(hoja As ExcelWorksheet)
        Try
            Dim startRow As Integer = 5 ' Los encabezados están en la fila 5
            Dim datosOriginales As New List(Of Dictionary(Of String, Object))()

            ' Primero leer todos los datos sin agrupar
            For row As Integer = startRow + 1 To hoja.Dimension.End.Row
                Try
                    Dim cliente As String = hoja.Cells(row, 1).Text.Trim()
                    Dim buque As String = hoja.Cells(row, 2).Text.Trim()
                    Dim producto As String = hoja.Cells(row, 5).Text.Trim()
                    Dim viaje As String = hoja.Cells(row, 13).Text.Trim() ' Columna VIAJE


                    Dim pesoNetoText As String = hoja.Cells(row, 17).Text.Trim()
                    Dim pesoNeto As Decimal = 0

                    ' Verificar si la fila tiene datos válidos
                    If Not String.IsNullOrEmpty(cliente) AndAlso
                       Not String.IsNullOrEmpty(buque) AndAlso
                       Not String.IsNullOrEmpty(producto) Then

                        If Decimal.TryParse(pesoNetoText.Replace(",", ""), pesoNeto) AndAlso pesoNeto > 0 Then
                            Dim registro As New Dictionary(Of String, Object)()
                            registro("Cliente") = cliente
                            registro("Buque") = buque
                            registro("Producto") = producto
                            registro("Viaje") = viaje
                            registro("PesoNeto") = pesoNeto

                            datosOriginales.Add(registro)
                        End If

                    End If
                Catch ex As Exception
                    ' Continuar con la siguiente fila si hay error en esta
                    Continue For
                End Try
            Next

            ' Ahora agrupar los datos manualmente
            Dim grupos As New Dictionary(Of String, List(Of Dictionary(Of String, Object)))()

            For Each registro In datosOriginales
                Dim clave As String = $"{registro("Cliente")}|{registro("Buque")}|{registro("Producto")}|{registro("Viaje")}"

                If Not grupos.ContainsKey(clave) Then
                    grupos(clave) = New List(Of Dictionary(Of String, Object))()
                End If

                grupos(clave).Add(registro)
            Next

            ' Crear el DataTable con los datos agrupados
            Dim dt As New DataTable()
            dt.Columns.Add("CLIENTE", GetType(String))
            dt.Columns.Add("BUQUE", GetType(String))
            dt.Columns.Add("PRODUCTO", GetType(String))
            dt.Columns.Add("VIAJE", GetType(String))
            dt.Columns.Add("TOTAL PESO NETO", GetType(Decimal))
            dt.Columns.Add("CANT. REGISTROS", GetType(Integer))

            ' Llenar el DataTable con los grupos
            For Each grupo In grupos
                If grupo.Value.Count > 0 Then
                    Dim primerRegistro = grupo.Value(0)
                    Dim totalPesoNeto As Decimal = grupo.Value.Sum(Function(x) CDec(x("PesoNeto")))

                    dt.Rows.Add(
                        primerRegistro("Cliente").ToString(),
                        primerRegistro("Buque").ToString(),
                        primerRegistro("Producto").ToString(),
                        primerRegistro("Viaje").ToString(),
                        totalPesoNeto,
                        grupo.Value.Count
                    )
                End If
            Next

            ' Ordenar por buque, luego por cliente
            Dim dv As DataView = dt.DefaultView
            dv.Sort = "BUQUE ASC, CLIENTE ASC"
            dt = dv.ToTable()

            dgvDatos.DataSource = dt
            lblHoja.Text = $"INGRESO - Agrupado ({dt.Rows.Count} grupos de {datosOriginales.Count} registros)"

            ' Formatear la columna de peso neto
            If dgvDatos.Columns.Contains("TOTAL PESO NETO") Then
                dgvDatos.Columns("TOTAL PESO NETO").DefaultCellStyle.Format = "N2"
            End If

        Catch ex As Exception
            MessageBox.Show("Error al procesar INGRESO: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub CargarYAgruparEgreso(hoja As ExcelWorksheet)
        Try
            Dim startRow As Integer = 5 ' Los encabezados están en la fila 5
            Dim datosOriginales As New List(Of Dictionary(Of String, Object))()

            ' Primero leer todos los datos sin agrupar
            For row As Integer = startRow + 1 To hoja.Dimension.End.Row
                Try
                    Dim cliente As String = hoja.Cells(row, 1).Text.Trim()
                    Dim buque As String = hoja.Cells(row, 2).Text.Trim()
                    Dim producto As String = hoja.Cells(row, 5).Text.Trim()
                    Dim bl As String = hoja.Cells(row, 10).Text.Trim()
                    Dim duca As String = hoja.Cells(row, 11).Text.Trim()
                    Dim consignatario As String = hoja.Cells(row, 12).Text.Trim()
                    Dim bodega As String = hoja.Cells(row, 13).Text.Trim()

                    ' FECHA: columna 3 del Excel
                    Dim fechaVal As Object = hoja.Cells(row, 3).Value
                    Dim fechaViaje As DateTime
                    If fechaVal IsNot Nothing AndAlso DateTime.TryParse(fechaVal.ToString(), fechaViaje) Then
                        fechaViaje = fechaViaje.Date
                    Else
                        ' si no hay fecha válida, saltamos la fila
                        Continue For
                    End If

                    ' Leer peso neto
                    Dim pesoNetoText As String = hoja.Cells(row, 16).Text.Trim()
                    Dim pesoNeto As Decimal = 0

                    If Not String.IsNullOrEmpty(cliente) AndAlso
                   Not String.IsNullOrEmpty(buque) AndAlso
                   Not String.IsNullOrEmpty(producto) Then

                        If Decimal.TryParse(pesoNetoText.Replace(",", ""), pesoNeto) AndAlso pesoNeto > 0 Then
                            Dim registro As New Dictionary(Of String, Object)()
                            registro("Cliente") = cliente
                            registro("Buque") = buque
                            registro("Producto") = producto
                            registro("BL") = bl
                            registro("DUCA") = duca
                            registro("Consignatario") = consignatario
                            registro("Bodega") = bodega
                            registro("PesoNeto") = pesoNeto
                            registro("Fecha") = fechaViaje

                            datosOriginales.Add(registro)
                        End If
                    End If
                Catch
                    Continue For
                End Try
            Next

            ' Agrupar por CLIENTE, BUQUE, PRODUCTO, BL, DUCA, CONSIGNATARIO, BODEGA, FECHA
            Dim grupos As New Dictionary(Of String, List(Of Dictionary(Of String, Object)))()

            For Each registro In datosOriginales
                Dim f As Date = CType(registro("Fecha"), Date)
                Dim clave As String =
                $"{registro("Cliente")}|{registro("Buque")}|{registro("Producto")}|{registro("BL")}|{registro("DUCA")}|{registro("Consignatario")}|{registro("Bodega")}|{f:yyyyMMdd}"

                If Not grupos.ContainsKey(clave) Then
                    grupos(clave) = New List(Of Dictionary(Of String, Object))()
                End If

                grupos(clave).Add(registro)
            Next

            ' Crear el DataTable con los datos agrupados
            Dim dt As New DataTable()
            dt.Columns.Add("CLIENTE", GetType(String))
            dt.Columns.Add("BUQUE", GetType(String))
            dt.Columns.Add("PRODUCTO", GetType(String))
            dt.Columns.Add("BL", GetType(String))
            dt.Columns.Add("DUCA", GetType(String))
            dt.Columns.Add("CONSIGNATARIO", GetType(String))
            dt.Columns.Add("BODEGA", GetType(String))
            dt.Columns.Add("TOTAL PESO NETO", GetType(Decimal))
            dt.Columns.Add("CANT. REGISTROS", GetType(Integer))
            dt.Columns.Add("FECHA", GetType(Date)) ' 👈 nueva columna

            ' Llenar el DataTable con los grupos
            For Each grupo In grupos
                If grupo.Value.Count > 0 Then
                    Dim primerRegistro = grupo.Value(0)
                    Dim totalPesoNeto As Decimal = grupo.Value.Sum(Function(x) CDec(x("PesoNeto")))
                    Dim fechaViaje As Date = CType(primerRegistro("Fecha"), Date)

                    dt.Rows.Add(
                    primerRegistro("Cliente").ToString(),
                    primerRegistro("Buque").ToString(),
                    primerRegistro("Producto").ToString(),
                    primerRegistro("BL").ToString(),
                    primerRegistro("DUCA").ToString(),
                    primerRegistro("Consignatario").ToString(),
                    primerRegistro("Bodega").ToString(),
                    totalPesoNeto,
                    grupo.Value.Count,
                    fechaViaje
                )
                End If
            Next

            ' Ordenar por buque, fecha, cliente
            Dim dv As DataView = dt.DefaultView
            dv.Sort = "BUQUE ASC, FECHA ASC, CLIENTE ASC"
            dt = dv.ToTable()

            dgvDatos.DataSource = dt
            lblHoja.Text = $"EGRESO - Agrupado ({dt.Rows.Count} grupos de {datosOriginales.Count} registros)"

            If dgvDatos.Columns.Contains("TOTAL PESO NETO") Then
                dgvDatos.Columns("TOTAL PESO NETO").DefaultCellStyle.Format = "N2"
            End If

        Catch ex As Exception
            MessageBox.Show("Error al procesar EGRESO: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Private Sub btnSalir_Click(sender As Object, e As EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub btnDB_Click(sender As Object, e As EventArgs) Handles btnDB.Click
        If dgvDatos.DataSource Is Nothing Then
            MessageBox.Show("No hay datos para guardar. Cargue primero un archivo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim resultado As DialogResult = MessageBox.Show("¿Está seguro de guardar los datos en la base de datos?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If resultado = DialogResult.No Then Return

        Try

            GuardarEgresoDB()

            GuardarPesajesDB()

        Catch ex As Exception
            MessageBox.Show("Error al guardar en base de datos: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub GuardarEgresoDB()
        Dim connectionString As String = Datos.srt_conexion

        Using connection As New SqlConnection(connectionString)
            connection.Open()

            Using transaction As SqlTransaction = connection.BeginTransaction()
                Try
                    Dim dt As DataTable = CType(dgvDatos.DataSource, DataTable)
                    Dim registrosGuardados As Integer = 0

                    For Each row As DataRow In dt.Rows
                        ' Obtener o crear IDs de las tablas maestras
                        Dim fechaBuque As Date

                        If TypeOf row("FECHA") Is Date Then
                            fechaBuque = CDate(row("FECHA"))
                        ElseIf Not Date.TryParse(row("FECHA").ToString(), fechaBuque) Then
                            fechaBuque = Date.Today
                        End If

                        Dim idBuque As Integer = ObtenerBuque(row("BUQUE").ToString(), fechaBuque, connection, transaction)
                        Dim idCliente As Integer = ObtenerCliente(row("CLIENTE").ToString(), connection, transaction)
                        Dim idConsignatario As Integer = ObtenerConsignatario(row("CONSIGNATARIO").ToString(), connection, transaction)
                        Dim idProducto As Integer = ObtenerProducto(row("PRODUCTO").ToString(), connection, transaction)
                        Dim idBodega As Integer = ObtenerBodega(row("BODEGA").ToString(), connection, transaction)

                        Dim sqlDucaBuque = "INSERT INTO duca_buque (duca, buque_idbuque) VALUES (@duca, @id_buque); SELECT SCOPE_IDENTITY();"
                        Dim idDucaBuque As Integer

                        Using cmdDucaBuque As New SqlCommand(sqlDucaBuque, connection, transaction)
                            cmdDucaBuque.Parameters.AddWithValue("@duca", row("DUCA").ToString())
                            cmdDucaBuque.Parameters.AddWithValue("@id_buque", idBuque)
                            idDucaBuque = Convert.ToInt32(cmdDucaBuque.ExecuteScalar())
                        End Using

                        Dim sqlBLBuque = "INSERT INTO bl_buque (bl, buque_idbuque, consignatario_idconsignatario, producto_idproducto, 
                                        porcentaje_merma, duca_buque_idduca_buque) 
                                        VALUES (@bl, @id_buque, @id_consignatario, @id_producto, @merma, @id_ducabuque); SELECT SCOPE_IDENTITY();"
                        Dim idBLBuque As Integer

                        Using cmdBLBuque As New SqlCommand(sqlBLBuque, connection, transaction)
                            cmdBLBuque.Parameters.AddWithValue("@bl", row("BL").ToString())
                            cmdBLBuque.Parameters.AddWithValue("@id_buque", idBuque)
                            cmdBLBuque.Parameters.AddWithValue("@id_consignatario", idConsignatario)
                            cmdBLBuque.Parameters.AddWithValue("@id_producto", idProducto)
                            cmdBLBuque.Parameters.AddWithValue("@merma", 5)
                            cmdBLBuque.Parameters.AddWithValue("@id_ducabuque", idDucaBuque)

                            idBLBuque = Convert.ToInt32(cmdBLBuque.ExecuteScalar())
                        End Using

                        ' Insertar en tabla principal de egresos
                        Dim sqlEgreso As String = "INSERT INTO inventario (cantidad_actual, fecha_actualizacion, dias_almacenamiento, bodega_idbodega, bl_buque_id, consignatario_id, cliente_id, stock) 
                                                  VALUES (@cantidad, @fecha, @dias, @id_bodega, @id_blbuque, @id_consignatario, @id_cliente, @stock)"

                        Using cmdEgreso As New SqlCommand(sqlEgreso, connection, transaction)
                            cmdEgreso.Parameters.AddWithValue("@cantidad", CDec(row("TOTAL PESO NETO")))
                            cmdEgreso.Parameters.AddWithValue("@fecha", DateTime.Now)
                            cmdEgreso.Parameters.AddWithValue("@dias", 1)
                            cmdEgreso.Parameters.AddWithValue("@id_bodega", idBodega)
                            cmdEgreso.Parameters.AddWithValue("@id_blbuque", idBLBuque)
                            cmdEgreso.Parameters.AddWithValue("@id_consignatario", idConsignatario)
                            cmdEgreso.Parameters.AddWithValue("@id_cliente", idCliente)
                            cmdEgreso.Parameters.AddWithValue("@stock", CDec(row("TOTAL PESO NETO")))


                            cmdEgreso.ExecuteNonQuery()
                            registrosGuardados += 1
                        End Using
                    Next

                    transaction.Commit()

                Catch ex As Exception
                    transaction.Rollback()
                    Throw ex
                End Try
            End Using
        End Using
    End Sub

    Private Function ObtenerCliente(nombreCliente As String, connection As SqlConnection, transaction As SqlTransaction) As Integer
        ' Buscar si existe
        Dim sqlBuscar As String = "SELECT Id_cli FROM Cliente WHERE nombre_cli = @nombre"
        Using cmd As New SqlCommand(sqlBuscar, connection, transaction)
            cmd.Parameters.AddWithValue("@nombre", nombreCliente)
            Dim resultado = cmd.ExecuteScalar()
            If resultado IsNot Nothing Then
                Return CInt(resultado)
            End If
        End Using
    End Function

    Private Function ObtenerConsignatario(nombreConsignatario As String, connection As SqlConnection, transaction As SqlTransaction) As Integer
        Dim sqlBuscar As String = "SELECT Id_csg FROM Consignatario WHERE nombre_csg = @nombre"
        Using cmd As New SqlCommand(sqlBuscar, connection, transaction)
            cmd.Parameters.AddWithValue("@nombre", nombreConsignatario)
            Dim resultado = cmd.ExecuteScalar()
            If resultado IsNot Nothing Then
                Return CInt(resultado)
            End If
        End Using
    End Function

    Private buquesCache As New Dictionary(Of String, Integer)()

    Private Function ObtenerBuque(nombreBuque As String,
                              fechaViaje As Date,
                              connection As SqlConnection,
                              transaction As SqlTransaction) As Integer

        Dim nombreClave As String = If(nombreBuque, "").Trim().ToUpper()
        Dim clave As String = nombreClave & "|" & fechaViaje.ToString("yyyyMM")

        If buquesCache.ContainsKey(clave) Then
            Return buquesCache(clave)
        End If


        Dim nuevoIdBuque As Integer
        Dim sqlMaxId As String = "SELECT ISNULL(MAX(Id_buq), 0) + 1 FROM buques WITH (UPDLOCK, HOLDLOCK);"
        Using cmdMax As New SqlCommand(sqlMaxId, connection, transaction)
            nuevoIdBuque = Convert.ToInt32(cmdMax.ExecuteScalar())
        End Using

        Dim nuevoViaje As Integer
        Dim sqlMaxViaje As String = "SELECT ISNULL(MAX(CAST(viaje_buq AS INT)), 0) + 1 FROM buques WITH (UPDLOCK, HOLDLOCK);"
        Using cmdMax2 As New SqlCommand(sqlMaxViaje, connection, transaction)
            nuevoViaje = Convert.ToInt32(cmdMax2.ExecuteScalar())
        End Using

        Dim sqlInsert As String =
        "INSERT INTO buques " &
        "(Id_buq, nombre_buq, id_usu_insert_buq, fecha_insert_buq, id_usu_update_buq, fecha_update_buq, " &
        " id_usu_delete_buq, fecha_delete_buq, viaje_buq, fechaviaje_buq, Bodegas_buq, baja_buq, actualizo_buq) " &
        "VALUES (@Id_buq, @nombre_buq, @id_usu_insert_buq, @fecha_insert_buq, @id_usu_update_buq, @fecha_update_buq, " &
        " @id_usu_delete_buq, @fecha_delete_buq, @viaje_buq, @fechaviaje_buq, @Bodegas_buq, @baja_buq, @actualizo_buq);"

        Using cmdIns As New SqlCommand(sqlInsert, connection, transaction)
            cmdIns.Parameters.AddWithValue("@Id_buq", nuevoIdBuque)
            cmdIns.Parameters.AddWithValue("@nombre_buq", nombreBuque)

            cmdIns.Parameters.AddWithValue("@id_usu_insert_buq", DBNull.Value)
            cmdIns.Parameters.AddWithValue("@fecha_insert_buq", DBNull.Value)
            cmdIns.Parameters.AddWithValue("@id_usu_update_buq", DBNull.Value)
            cmdIns.Parameters.AddWithValue("@fecha_update_buq", DBNull.Value)
            cmdIns.Parameters.AddWithValue("@id_usu_delete_buq", DBNull.Value)
            cmdIns.Parameters.AddWithValue("@fecha_delete_buq", DBNull.Value)
            cmdIns.Parameters.AddWithValue("@viaje_buq", nuevoViaje.ToString())
            cmdIns.Parameters.AddWithValue("@fechaviaje_buq", fechaViaje.Date)
            cmdIns.Parameters.AddWithValue("@Bodegas_buq", 0)
            cmdIns.Parameters.AddWithValue("@baja_buq", "Abierto")
            cmdIns.Parameters.AddWithValue("@actualizo_buq", 3)

            cmdIns.ExecuteNonQuery()
        End Using


        buquesCache(clave) = nuevoIdBuque
        Return nuevoIdBuque

    End Function


    Private Function ObtenerProducto(nombreProducto As String, connection As SqlConnection, transaction As SqlTransaction) As Integer
        Dim sqlBuscar As String = "SELECT Id_pro FROM Producto WHERE nombre_pro = @nombre"
        Using cmd As New SqlCommand(sqlBuscar, connection, transaction)
            cmd.Parameters.AddWithValue("@nombre", nombreProducto)
            Dim resultado = cmd.ExecuteScalar()
            If resultado IsNot Nothing Then
                Return CInt(resultado)
            End If
        End Using
    End Function

    Private Function ObtenerBodega(nombreBodega As String, connection As SqlConnection, transaction As SqlTransaction) As Integer
        Dim sqlBuscar As String = "SELECT Id_bod FROM Bodega WHERE nombre_bod = @nombre"
        Using cmd As New SqlCommand(sqlBuscar, connection, transaction)
            cmd.Parameters.AddWithValue("@nombre", nombreBodega)
            Dim resultado = cmd.ExecuteScalar()
            If resultado IsNot Nothing Then
                Return CInt(resultado)
            End If
        End Using
    End Function
    Private Sub GuardarPesajesDB()
        If datosEgresoParaGuardar Is Nothing OrElse datosEgresoParaGuardar.Count = 0 Then
            MessageBox.Show("No hay datos de pesajes para guardar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim cnnStr As String = Datos.srt_conexion
        Dim insertSql As String =
    "INSERT INTO dbo.pesajes(id_pes,host_pes,periodocorrelativo_pes,ticketboleta_pes,placa_pes,Idcla_pes,prefijocont_pes,numidentcont_pes,tipocarga_pes,tamanocon_pes,
    dicecontener_pes,Idpro_pes,tipooperacion_pes,Idbuq_pes,bodegabuq_pes,Idvbuq_pes,fechav_pes,Idcli_pes,Idpaisdest_pes,Idpaisorig_pes,pesomanif_pes,pesotara_pes,pesotaracont_pes,
    pesobruto_pes,pesoarana_pes,Idbas_entrada_pes,fechahora_entrada_pes,idusu_entrada_pes,fechahora_salida_pes,idusu_salida_pes,fechahora_cierre_pes,cierre_pes,
    baja_pes,id_usu_insert_pes,fecha_insert_pes,EstePesaje_pes,blbuq_pes,empresa_pes,sync_pes, idbas_salida_pes)
    
    VALUES(@id_pes,@host,@periodo,@ticket,@placa,@idcla,@prefijo,@numident,@tipocarga,@tamanocon,@dicecontener,@idpro,@tipooper,@idbuq,@bodega,@idvbuq,@fechav,@idcli,
    @paisdest,@paisorig,@pesomanif,@pesotara,@pesotaracont,@pesobruto,@pesoarana,@idbas_ent,@fh_ent,@idusu_ent,@fh_sal,@idusu_sal,@fh_cierre,@cierre,@baja,@id_usu_ins,
    @f_ins,@este,@bl,@empresa,@sync, @idbas_salida_pes );"


        Dim insertados As Integer = 0
        Dim errores As New List(Of String)

        Using cnn As New SqlConnection(cnnStr)
            cnn.Open()

            Using tr = cnn.BeginTransaction()
                Try
                    For Each reg In datosEgresoParaGuardar
                        Try
                            ' PRIMER REGISTRO: ENTRADA
                            Dim IdCorrela As Integer
                            Using cmdNext As New SqlCommand("SELECT ISNULL(MAX(id_pes),0)+1 FROM dbo.pesajes WITH (UPDLOCK, HOLDLOCK);", cnn, tr)
                                IdCorrela = Convert.ToInt32(cmdNext.ExecuteScalar())
                            End Using

                            Dim idCliente As Integer = ObtenerCliente(reg("cliente").ToString(), cnn, tr)
                            Dim fechaViaje As Date = CType(reg("fh_ent"), DateTime).Date
                            Dim idBuque As Integer = ObtenerBuque(reg("buque").ToString(), fechaViaje, cnn, tr)
                            Dim idProducto As Integer = ObtenerProducto(reg("producto").ToString(), cnn, tr)
                            Dim idBodega As Integer = ObtenerBodega(reg("bodega").ToString(), cnn, tr)

                            Using cmd As New SqlCommand(insertSql, cnn, tr)
                                cmd.Parameters.AddWithValue("@id_pes", IdCorrela)
                                cmd.Parameters.AddWithValue("@host", "0")
                                cmd.Parameters.AddWithValue("@periodo", "0")
                                cmd.Parameters.AddWithValue("@ticket", " ")
                                cmd.Parameters.AddWithValue("@placa", reg("placa").ToString())
                                cmd.Parameters.AddWithValue("@prefijo", " ")
                                cmd.Parameters.AddWithValue("@numident", " ")
                                cmd.Parameters.AddWithValue("@tipocarga", " ")
                                cmd.Parameters.AddWithValue("@tamanocon", "0")
                                cmd.Parameters.AddWithValue("@dicecontener", " ")
                                cmd.Parameters.AddWithValue("@tipooper", "RECEPCION")
                                cmd.Parameters.AddWithValue("@bodega", idBodega)
                                cmd.Parameters.AddWithValue("@bl", reg("bl").ToString())
                                cmd.Parameters.AddWithValue("@empresa", LibreriaGeneral.idEmpresa)
                                cmd.Parameters.AddWithValue("@idcla", 1)
                                cmd.Parameters.AddWithValue("@idpro", idProducto)
                                cmd.Parameters.AddWithValue("@idbuq", idBuque)
                                cmd.Parameters.AddWithValue("@idvbuq", " ")
                                cmd.Parameters.AddWithValue("@idcli", idCliente)
                                cmd.Parameters.AddWithValue("@paisdest", 4)
                                cmd.Parameters.AddWithValue("@paisorig", 4)
                                cmd.Parameters.AddWithValue("@idbas_salida_pes", "")
                                cmd.Parameters.AddWithValue("@idbas_ent", "01")
                                cmd.Parameters.AddWithValue("@idusu_ent", "CARLOS")
                                cmd.Parameters.AddWithValue("@idusu_sal", "")
                                cmd.Parameters.AddWithValue("@id_usu_ins", "CARLOS")
                                cmd.Parameters.AddWithValue("@fechav", SqlDbType.DateTime).Value = CType(reg("fh_ent"), DateTime)
                                cmd.Parameters.Add("@fh_ent", SqlDbType.DateTime).Value = CType(reg("fh_ent"), DateTime)
                                cmd.Parameters.AddWithValue("@fh_sal", DBNull.Value)
                                cmd.Parameters.AddWithValue("@fh_cierre", DBNull.Value)
                                cmd.Parameters.AddWithValue("@f_ins", SqlDbType.DateTime).Value = CType(reg("fh_ent"), DateTime)
                                cmd.Parameters.AddWithValue("@pesomanif", "0")
                                cmd.Parameters.AddWithValue("@pesotara", CDec(reg("pesotara")))
                                cmd.Parameters.AddWithValue("@pesobruto", CDec(reg("pesobruto")))
                                cmd.Parameters.AddWithValue("@pesotaracont", " 0")
                                cmd.Parameters.AddWithValue("@pesoarana", "0")
                                cmd.Parameters.AddWithValue("@cierre", "NO")
                                cmd.Parameters.AddWithValue("@baja", 0)
                                cmd.Parameters.AddWithValue("@este", "Entrada")
                                cmd.Parameters.AddWithValue("@sync", 1)

                                cmd.ExecuteNonQuery()
                                insertados += 1
                            End Using

                            ' SEGUNDO REGISTRO: SALIDA
                            Dim IdCorrela2 As Integer
                            Using cmdNext2 As New SqlCommand("SELECT ISNULL(MAX(id_pes),0)+1 FROM dbo.pesajes WITH (UPDLOCK, HOLDLOCK);", cnn, tr)
                                IdCorrela2 = Convert.ToInt32(cmdNext2.ExecuteScalar())
                            End Using

                            Using cmd2 As New SqlCommand(insertSql, cnn, tr)
                                cmd2.Parameters.AddWithValue("@id_pes", IdCorrela2)
                                cmd2.Parameters.AddWithValue("@host", "0")
                                cmd2.Parameters.AddWithValue("@periodo", "0")
                                cmd2.Parameters.AddWithValue("@ticket", " ")
                                cmd2.Parameters.AddWithValue("@placa", reg("placa").ToString())
                                cmd2.Parameters.AddWithValue("@prefijo", " ")
                                cmd2.Parameters.AddWithValue("@numident", " ")
                                cmd2.Parameters.AddWithValue("@tipocarga", " ")
                                cmd2.Parameters.AddWithValue("@tamanocon", "0")
                                cmd2.Parameters.AddWithValue("@dicecontener", " ")
                                cmd2.Parameters.AddWithValue("@tipooper", "RECEPCION")
                                cmd2.Parameters.AddWithValue("@bodega", idBodega)
                                cmd2.Parameters.AddWithValue("@bl", reg("bl").ToString())
                                cmd2.Parameters.AddWithValue("@empresa", LibreriaGeneral.idEmpresa)
                                cmd2.Parameters.AddWithValue("@idcla", 1)
                                cmd2.Parameters.AddWithValue("@idpro", idProducto)
                                cmd2.Parameters.AddWithValue("@idbuq", idBuque)
                                cmd2.Parameters.AddWithValue("@idvbuq", " ")
                                cmd2.Parameters.AddWithValue("@idcli", idCliente)
                                cmd2.Parameters.AddWithValue("@paisdest", 4)
                                cmd2.Parameters.AddWithValue("@paisorig", 4)
                                cmd2.Parameters.AddWithValue("@idbas_ent", "01")
                                cmd2.Parameters.AddWithValue("@idbas_salida_pes", "01")
                                cmd2.Parameters.AddWithValue("@idusu_ent", "CARLOS")
                                cmd2.Parameters.AddWithValue("@idusu_sal", "CARLOS")
                                cmd2.Parameters.AddWithValue("@id_usu_ins", "CARLOS")
                                cmd2.Parameters.AddWithValue("@fechav", SqlDbType.DateTime).Value = CType(reg("fh_ent"), DateTime)
                                cmd2.Parameters.AddWithValue("@fh_ent", SqlDbType.DateTime).Value = CType(reg("fh_ent"), DateTime)
                                cmd2.Parameters.AddWithValue("@fh_sal", SqlDbType.DateTime).Value = CType(reg("fh_sal"), DateTime)
                                cmd2.Parameters.AddWithValue("@fh_cierre", SqlDbType.DateTime).Value = CType(reg("fh_sal"), DateTime)
                                cmd2.Parameters.AddWithValue("@f_ins", SqlDbType.DateTime).Value = CType(reg("fh_sal"), DateTime)
                                cmd2.Parameters.AddWithValue("@pesomanif", "0")
                                cmd2.Parameters.AddWithValue("@pesotara", CDec(reg("pesotara")))
                                cmd2.Parameters.AddWithValue("@pesobruto", "0")
                                cmd2.Parameters.AddWithValue("@pesotaracont", "0")
                                cmd2.Parameters.AddWithValue("@pesoarana", "0")
                                cmd2.Parameters.AddWithValue("@cierre", "CIERRE")
                                cmd2.Parameters.AddWithValue("@baja", 0)
                                cmd2.Parameters.AddWithValue("@este", "Salida")
                                cmd2.Parameters.AddWithValue("@sync", 1)

                                cmd2.ExecuteNonQuery()
                                insertados += 1
                            End Using

                        Catch exFila As Exception
                            errores.Add($"Placa '{reg("placa")}': {exFila.Message}")
                        End Try
                    Next

                    tr.Commit()

                    Dim msg As String = $"Pesajes guardados: {insertados:#,0}"
                    If errores.Count > 0 Then
                        msg &= Environment.NewLine & "Errores (primeros 10):" & Environment.NewLine &
                               String.Join(Environment.NewLine, errores.Take(10))
                        If errores.Count > 10 Then msg &= Environment.NewLine & $"(+ {errores.Count - 10} más...)"
                    End If
                    MessageBox.Show(msg, "Resultado", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Catch ex As Exception
                    tr.Rollback()
                    Throw ex
                End Try
            End Using
        End Using
    End Sub


    ' MÉTODO: Solo lee los datos del Excel y los guarda en memoria (SIN MOSTRAR)
    Private Sub LeerDatosEgresoEnMemoria(hoja As ExcelWorksheet)
        Try
            Dim startRow As Integer = 5
            datosEgresoParaGuardar = New List(Of Dictionary(Of String, Object))()

            ' Leer todas las placas (col 9)
            For row As Integer = startRow + 1 To hoja.Dimension.End.Row
                Try
                    Dim cliente As String = hoja.Cells(row, 1).Text
                    Dim buque As String = hoja.Cells(row, 2).Text
                    Dim producto As String = hoja.Cells(row, 5).Text
                    Dim placa As String = hoja.Cells(row, 9).Text
                    Dim bl As String = hoja.Cells(row, 10).Text
                    Dim bodega As String = hoja.Cells(row, 13).Text
                    Dim pesotara As Decimal = hoja.Cells(row, 15).GetValue(Of Decimal)
                    Dim pesobruto As Decimal = hoja.Cells(row, 14).GetValue(Of Decimal)

                    Dim fecha As DateTime
                    Try
                        fecha = DateTime.FromOADate(CDbl(hoja.Cells(row, 3).Value))
                    Catch
                        DateTime.TryParse(hoja.Cells(row, 3).Text, fecha)
                    End Try

                    Dim hora As DateTime
                    Try
                        hora = DateTime.FromOADate(CDbl(hoja.Cells(row, 6).Value))
                    Catch
                        DateTime.TryParse(hoja.Cells(row, 6).Text, hora)
                    End Try

                    Dim horas As DateTime
                    Try
                        horas = DateTime.FromOADate(CDbl(hoja.Cells(row, 7).Value))
                    Catch
                        DateTime.TryParse(hoja.Cells(row, 7).Text, horas)
                    End Try

                    Dim fh_ent As DateTime = fecha.Date.Add(hora.TimeOfDay)
                    Dim fh_sal As DateTime = fecha.Date.Add(horas.TimeOfDay)

                    If Not String.IsNullOrEmpty(cliente) AndAlso Not String.IsNullOrEmpty(placa) Then
                        Dim registro As New Dictionary(Of String, Object)()
                        registro("placa") = placa
                        registro("cliente") = cliente
                        registro("buque") = buque
                        registro("producto") = producto
                        registro("bl") = bl
                        registro("pesobruto") = pesobruto
                        registro("pesotara") = pesotara
                        registro("bodega") = bodega
                        registro("fh_ent") = fh_ent
                        registro("fh_sal") = fh_sal
                        datosEgresoParaGuardar.Add(registro)
                    End If
                Catch
                    Continue For
                End Try
            Next

            ' Solo mensaje simple sin mostrar datos en ningún lado
            MessageBox.Show($"Archivo cargado: {datosEgresoParaGuardar.Count} registros listos para guardar.",
                          "Carga Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("Error al leer datos de EGRESO: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub lblCargando_Click(sender As Object, e As EventArgs) Handles lblCargando.Click

    End Sub
End Class