Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports Microsoft.Office.Core
Imports Microsoft.Office.Interop
Imports System.Data.Odbc
Imports System.Xml.XPath
Imports System.Xml
Imports System.Drawing.Printing

Public Class DatosGenereral
    Dim ConfigCE As New appConfigEditor("ConfigSQL.xml")
    'Dim str_conexion2 As String = ConfigCE.getAppSettingValue("SQLPath").ToString

    Dim vServer As String = ConfigCE.getAppSettingValue("DataServer").ToString
    Dim vData As String = ConfigCE.getAppSettingValue("DataBase").ToString
    Dim vUsu As String = ConfigCE.getAppSettingValue("DataUser").ToString
    Dim vPass As String = ConfigCE.getAppSettingValue("DataPassword").ToString
    Dim cFRep As String = ConfigCE.getAppSettingValue("Reportes").ToString
    Dim gBol As String = ConfigCE.getAppSettingValue("TBoleta").ToString
    Dim pCOM As String = ConfigCE.getAppSettingValue("PuertoCOM").ToString
    Dim bSegundo As String = ConfigCE.getAppSettingValue("BitsxS").ToString
    Dim pBascula As String = ConfigCE.getAppSettingValue("Bascula").ToString
    Dim pEmpresa As String = ConfigCE.getAppSettingValue("Empresa").ToString

    Dim str_conexion As String = "Server=" & vServer & "; Initial Catalog=" & vData & "; User Id=" & vUsu & "; Password=" & vPass & ";"
    Dim conexion As New SqlConnection
    Dim cmd As SqlCommand


    'Constructor
    Public Sub New()
        ' extraer()

        LibreriaGeneral.cServer = vServer
        LibreriaGeneral.cBaseD = vData
        LibreriaGeneral.cUsu = vUsu
        LibreriaGeneral.cPass = vPass
        LibreriaGeneral.cPathReport = cFRep
        LibreriaGeneral.gPrinter = gBol

        LibreriaGeneral.gPuertoCOM = pCOM
        LibreriaGeneral.gBitsPorSegundo = CInt(bSegundo)

        LibreriaGeneral.nBascu = pBascula
        LibreriaGeneral.gEmpresa = pEmpresa

    End Sub

    '*Variables ---------------------------------
    Public Property srt_conexion() As String
        Get
            Return Me.str_conexion
        End Get
        Set(ByVal str As String)
            Me.str_conexion = str
        End Set
    End Property

    Public Sub New(ByVal str As String)
        Me.str_conexion = str
    End Sub

    'Para verificar la entrada si hay o no conexion
    Public Function Conecto() As Boolean
        Dim SiHay As Boolean = False
        Try
            conexion.ConnectionString = str_conexion
            conexion.Open()

            If conexion.State() = ConnectionState.Open Then
                conexion.Close()
            End If
            SiHay = True
        Catch ex As Exception
            MsgBox("ERROR: NO HAY CONEXION")
        End Try

        Return SiHay

    End Function

    Public Sub consulta_non_query(ByVal consulta As String)

        'Este metodo recibe como parametro la consulta completa y sirve para hacer INSERT, UPDATE Y DELETE
        conexion.ConnectionString = str_conexion
        cmd = New SqlCommand(consulta, conexion)
        conexion.Open()
        Try
            cmd.ExecuteNonQuery()
            MsgBox("La operacion se realizo con exito!", MsgBoxStyle.Information, "Operacion exitosa!")

        Catch ex As Exception
            ' MsgBox("Error al operar con la base de datos!", MsgBoxStyle.Critical, "Error!")
        End Try
        conexion.Close()
    End Sub
    Public Sub consulta_non_queryy(ByVal consulta As String)

        'Este metodo recibe como parametro la consulta completa y sirve para hacer INSERT, UPDATE Y DELETE
        conexion.ConnectionString = str_conexion
        cmd = New SqlCommand(consulta, conexion)
        conexion.Open()
        Try
            cmd.ExecuteNonQuery()

        Catch ex As Exception
            ' MsgBox("Error al operar con la base de datos!", MsgBoxStyle.Critical, "Error!")
        End Try
        conexion.Close()
    End Sub


    ''' <summary>
    ''' Ejecuta INSERT/UPDATE/DELETE y devuelve True si tuvo éxito.
    ''' En caso de error, mensajeError contiene el detalle para mostrarlo al usuario.
    ''' La conexión siempre se cierra (Finally), aunque ocurra una excepción.
    ''' </summary>
    Public Function EjecutarSQL(ByVal consulta As String, ByRef mensajeError As String) As Boolean
        mensajeError = ""
        conexion.ConnectionString = str_conexion
        cmd = New SqlCommand(consulta, conexion)
        Try
            conexion.Open()
            cmd.ExecuteNonQuery()
            Return True
        Catch ex As SqlException
            mensajeError = "SQL Error " & ex.Number & ": " & ex.Message
            Return False
        Catch ex As Exception
            mensajeError = ex.Message
            Return False
        Finally
            Try
                If conexion.State = ConnectionState.Open Then conexion.Close()
            Catch
            End Try
        End Try
    End Function

    Public Sub consulta_non_queryDeta(ByVal consulta As String)

        'Este metodo recibe como parametro la consulta completa y sirve para hacer INSERT, UPDATE Y DELETE
        conexion.ConnectionString = str_conexion
        cmd = New SqlCommand(consulta, conexion)
        conexion.Open()

        Dim dv As New DataView
        Dim adapter As New SqlDataAdapter
        Dim ds As New DataSet

        Try
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            MsgBox("Error al operar con la base de datos!", MsgBoxStyle.Critical, "Error!")
        End Try
        conexion.Close()
    End Sub

    Public Function consulta_reader(ByVal consulta As String) As DataTable

        'Este metodo recibe como parametro la consulta completa y sirve para hacer SELECT
        Dim dt As New DataTable
        conexion.ConnectionString = str_conexion
        cmd = New SqlCommand(consulta, conexion)
        conexion.Open()

        Try
            dt.Load(cmd.ExecuteReader())
        Catch ex As Exception
            MsgBox("Error al operar con la base de datos!", MsgBoxStyle.Critical, "Error!")

        End Try
        conexion.Close()
        Return dt

    End Function

    Public Function consulta_readerp(ByVal consulta As String) As DataTable

        'Este metodo recibe como parametro la consulta completa y sirve para hacer SELECT
        Dim dt As New DataTable
        conexion.ConnectionString = str_conexion
        cmd = New SqlCommand(consulta, conexion)
        conexion.Open()

        Try
            dt.Load(cmd.ExecuteReader())
        Catch ex As Exception

        End Try
        conexion.Close()
        Return dt

    End Function

    Public Sub cargar_lista(ByRef lista As ListBox, ByVal consulta As String, ByVal valueMember As String, ByVal displayMember As String)
        Dim dt As New Data.DataTable
        conexion.ConnectionString = str_conexion
        cmd = New SqlCommand(consulta, conexion)
        conexion.Open()

        Try
            dt.Load(cmd.ExecuteReader())

            lista.DataSource = dt
            lista.ValueMember = valueMember
            lista.DisplayMember = displayMember
        Catch ex As Exception
            MsgBox("Error al operar con la base de datos!", MsgBoxStyle.Critical, "Error!")

        End Try
        conexion.Close()
    End Sub

    Public Sub cargar_combo(ByRef combo As ComboBox, ByVal consulta As String, ByVal valueMember As String, ByVal displayMember As String)
        Dim dt As New DataTable
        conexion.ConnectionString = str_conexion
        cmd = New SqlCommand(consulta, conexion)
        conexion.Open()

        Try
            dt.Load(cmd.ExecuteReader())

            combo.DisplayMember = displayMember
            combo.ValueMember = valueMember
            combo.DataSource = dt
            combo.SelectedIndex = -1

        Catch ex As Exception
            'MsgBox("Error al operar con la base de datos!", MsgBoxStyle.Critical, "Error!")

        End Try
        conexion.Close()
    End Sub

    Public Function verificar_existencia(ByVal consulta As String) As Boolean
        'Devuelve true si existe, entonces no grabamos, o devuelve false si no existe entoinces debemos grabar.
        Dim dt As New DataTable
        conexion.ConnectionString = str_conexion
        cmd = New SqlCommand(consulta, conexion)
        conexion.Open()
        Try
            dt.Load(cmd.ExecuteReader())

        Catch ex As Exception
            ' MsgBox("Error al operar con la base de datos!", MsgBoxStyle.Critical, "Error!")

        End Try

        conexion.Close()

        If dt.Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function consulta_dv(ByVal consulta As String) As DataView
        'Este metodo recibe como parametro la consulta completa y sirve para hacer SELECT
        Dim dv As New DataView
        Dim adapter As New SqlDataAdapter
        Dim ds As New DataSet
        'Dim command As SqlCommand
        conexion.ConnectionString = str_conexion
        cmd = New SqlCommand(consulta, conexion)
        conexion.Open()
        Try
            'dt.Load(cmd.ExecuteReader())
            'command = New SqlCommand(sql, connection)

            adapter.SelectCommand = cmd
            adapter.Fill(ds)
            adapter.Dispose()

            dv = ds.Tables(0).DefaultView
        Catch ex As Exception
            ' MsgBox("Error al operar con la base de datos!", MsgBoxStyle.Critical, "Error!")
        End Try
        conexion.Close()
        Return dv

    End Function


    Public Function consulta_ds(ByVal consulta As String) As DataView
        'Este metodo recibe como parametro la consulta completa y sirve para hacer SELECT
        Dim dv As New DataView
        Dim adapter As New SqlDataAdapter
        Dim ds As New DataSet
        'Dim command As SqlCommand
        conexion.ConnectionString = str_conexion
        cmd = New SqlCommand(consulta, conexion)
        conexion.Open()
        Try
            'dt.Load(cmd.ExecuteReader())
            'command = New SqlCommand(sql, connection)

            adapter.SelectCommand = cmd
            adapter.Fill(ds)
            adapter.Dispose()

            dv = ds.Tables(0).DefaultView
        Catch ex As Exception
            'MsgBox("Error al operar con la base de datos!", MsgBoxStyle.Critical, "Error!")
        End Try
        conexion.Close()
        Return dv

    End Function

    Public Function TieneAcceso(ByVal consulta As String) As Boolean
        'Devuelve true si existe, entonces no grabamos, o devuelve false si no existe entoinces debemos grabar.
        Dim dt As New DataTable
        conexion.ConnectionString = str_conexion
        consulta = "SELECT codigo_usu, Acceso FROM dbo.usuarioAccesos WHERE Log_usu ='" & LibreriaGeneral.usuario & "' AND codigo_usu = '" & consulta & "' AND Acceso = 1"
        cmd = New SqlCommand(consulta, conexion)
        conexion.Open()
        Try
            dt.Load(cmd.ExecuteReader())

        Catch ex As Exception
            MsgBox("Error al operar con la base de datos!", MsgBoxStyle.Critical, "Error!")

        End Try

        conexion.Close()

        If dt.Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function LeePeso() As String

        'Limpia la Memoria y borrar el Archivo .PRN
        GC.Collect()
        GC.WaitForPendingFinalizers()
        GC.Collect()

        Dim Regreso As String = "Fallo"

        Try
            Dim Paso As Boolean = True
            Dim SegMento1 As Double = 0.0
            Dim SegMento2 As Double = 0.0
            Dim SegMento3 As Double = 0.0
            Dim SegMento4 As Double = 0.0
            Dim Contenido As String = ""
            Dim Coinciden As Integer = 0
            Dim NomPeso As String = ""

            'llama a unproceso de peso 
            '--------------------------------------------------------------------------

            'Confirmar que Existe el archivo en Basculas
            If Not File.Exists("C:\aperbascu\aperpeso.PRN") Then

                If System.Windows.Forms.SystemInformation.ComputerName.ToUpper = "BASCULAPH01" Then
                    NomPeso = "\leerpeso.exe "
                End If

                If System.Windows.Forms.SystemInformation.ComputerName.ToUpper = "BASCULAPH02" Then
                    NomPeso = "\leerpeso2.exe "
                End If

                If System.Windows.Forms.SystemInformation.ComputerName.ToUpper = "BASCULAPH03" Then
                    NomPeso = "\leerpeso3.exe "
                End If

                'para leeer el exce de peso
                Dim pobj As New Process()
                pobj.StartInfo.RedirectStandardOutput = True
                pobj.StartInfo.FileName = IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.GetName.CodeBase).Remove(0, 6) & NomPeso
                pobj.StartInfo.UseShellExecute = False
                pobj.StartInfo.CreateNoWindow = True
                pobj.Start()

                Dim outstr As String = pobj.StandardOutput.ReadToEnd()

                pobj.WaitForExit()
            End If
            '------------------------TERMINA DE EJECUTAR UN ARCHIVO TEMPORAL--


            Dim curFile As String = "c:\aperbascu\aperpeso.PRN"
            If File.Exists(curFile) Then

                My.Computer.FileSystem.DeleteFile("c:\aperbascu\aperpeso.prn")

                If System.Windows.Forms.SystemInformation.ComputerName.ToUpper = "BASCULAPH01" Then
                    NomPeso = "\leerpeso.exe "
                End If

                If System.Windows.Forms.SystemInformation.ComputerName.ToUpper = "BASCULAPH02" Then
                    NomPeso = "\leerpeso2.exe "
                End If

                If System.Windows.Forms.SystemInformation.ComputerName.ToUpper = "BASCULAPH03" Then
                    NomPeso = "\leerpeso3.exe "
                End If

                'para leeer el exce de peso
                Dim pobj As New Process()
                pobj.StartInfo.RedirectStandardOutput = True
                pobj.StartInfo.FileName = IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.GetName.CodeBase).Remove(0, 6) & NomPeso
                pobj.StartInfo.UseShellExecute = False
                pobj.StartInfo.CreateNoWindow = True
                pobj.Start()

                Dim outstr As String = pobj.StandardOutput.ReadToEnd()

                pobj.WaitForExit()
                '--------------------------------------------------------------------------------
            End If

            Coinciden = 0
            Dim lineas As New ArrayList()
            SegMento1 = 0.0
            SegMento2 = 0.0
            SegMento3 = 0.0
            SegMento4 = 0.0
            Contenido = ""
            Coinciden = 0
            Dim nLineas = 0


            If File.Exists("C:\AperBascu\aperpeso.PRN") Then

                Dim freader As New StreamReader("C:\AperBascu\aperpeso.PRN")
                Do
                    Contenido = freader.ReadLine()
                    If Not Contenido Is Nothing Then
                        'si quiero leer solo las líneas que no estén en blanco incluyo esta condicion
                        If Contenido.Length <> 0 Then
                            lineas.Add(Contenido)
                            ' kPesaje = contenido
                            nLineas = nLineas + 1
                        End If
                    End If

                Loop Until Contenido Is Nothing
                freader.Close()

                Dim P0 As String = ""
                Dim P1 As String = ""
                Dim P2 As String = ""
                Dim P3 As String = ""

                If nLineas > 3 Then
                    '- Toma cada Segmento y asigna el valor a cada uno
                    SegMento1 = Convert.ToDouble(lineas(0).ToString.Substring(InStr(3, lineas(0), " ", CompareMethod.Text)))
                    SegMento2 = Convert.ToDouble(lineas(1).ToString.Substring(InStr(3, lineas(1), " ", CompareMethod.Text)))
                    SegMento3 = Convert.ToDouble(lineas(2).ToString.Substring(InStr(3, lineas(2), " ", CompareMethod.Text)))
                    SegMento4 = Convert.ToDouble(lineas(3).ToString.Substring(InStr(3, lineas(3), " ", CompareMethod.Text)))

                    P0 = lineas(0).ToString.Substring(InStr(lineas(0), Chr(34)), 1)
                    P1 = lineas(1).ToString.Substring(InStr(lineas(1), Chr(34)), 1)
                    P2 = lineas(2).ToString.Substring(InStr(lineas(2), Chr(34)), 1)
                    P3 = lineas(3).ToString.Substring(InStr(lineas(3), Chr(34)), 1)

                Else
                    If nLineas > 2 Then
                        SegMento1 = Convert.ToDouble(lineas(0).ToString.Substring(InStr(3, lineas(0), " ", CompareMethod.Text)))
                        SegMento2 = Convert.ToDouble(lineas(1).ToString.Substring(InStr(3, lineas(1), " ", CompareMethod.Text)))
                        SegMento3 = Convert.ToDouble(lineas(2).ToString.Substring(InStr(3, lineas(2), " ", CompareMethod.Text)))

                        P0 = lineas(0).ToString.Substring(InStr(lineas(0), Chr(34)), 1)
                        P1 = lineas(1).ToString.Substring(InStr(lineas(1), Chr(34)), 1)
                        P2 = lineas(2).ToString.Substring(InStr(lineas(2), Chr(34)), 1)
                    End If
                End If


                If nLineas > 2 And (P0 & P1 & P3) = "000" Then
                    If SegMento1 = SegMento2 Then
                        Coinciden = Coinciden + 1
                        SegMento4 = SegMento1
                    End If

                    If SegMento1 = SegMento3 Then
                        Coinciden = Coinciden + 1
                        SegMento4 = SegMento1
                    End If

                    If SegMento2 = SegMento3 Then
                        Coinciden = Coinciden + 1
                        SegMento4 = SegMento2
                    End If
                Else
                    Coinciden = 0
                End If

                'Retorna
                If Coinciden > 0 Then
                    Regreso = Convert.ToString(Val(SegMento4 / 1000000))
                    'Return Regreso
                Else
                    'Return Regreso
                End If

            Else
                'Regreso = "Regenerar"
                'Return Regreso
            End If

        Catch ex As Exception

            ' Regreso = "Regenerar"
            'Return Regreso

        Finally

            'Limpia la Memoria para la Factura
            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()

        End Try

        Return Regreso

    End Function

    'Impresion Factura Salvador y Honduras
    Public Sub ImprBoleta(ByVal N_Ticket As String, ByVal _EstePeso As String)

        Dim dTableI As New DataTable
        Dim dTableII As New DataTable
        Dim DatosI As New DatosGenereral()

        'Arma Cursor de SAP, Para impresion de Factura.
        Dim StringSql As String = <sqlExp>
                                        EXEC [dbo].[sp_ImprBoleta] '<%= _EstePeso %>','<%= N_Ticket %>'
                                  </sqlExp>.Value


        'Para mostrar el Dato
        dTableI = DatosI.consulta_reader(StringSql)

        Try

            If dTableI.Rows.Count > 0 Then

                '//IMPRESION RECIBO DESDE EXCEL
                '--------------------------------------------------------------------------------
                Dim oXL As New Excel.Application
                Dim oWB As Excel.Workbook
                Dim oSheet As Excel.Worksheet
                'Dim oRng As Excel.Range

                'Referenciamos el Objeto Excel
                'oXL = CreateObject("Excel.Application")
                oXL.Visible = False
                oXL.DisplayAlerts = False

                Dim cFile As String = ""
                Dim cadena As String = ""

                'Localiza la Ruta del Archivo que usaremos como Plantilla
                cFile = IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.GetName.CodeBase).Remove(0, 6) & "\Formato_Boleta.xlsx"

                Dim Neto As Double = 0.0
                Dim Vgm As Double = 0.0

                'Abre Archivo
                oWB = oXL.Workbooks.Open(cFile)
                oSheet = oWB.ActiveSheet

                'Calibracion
                Dim Calibracion As String = ""
                Dim Certificacion As String = ""

                'Para mostrar el Dato
                dTableII = DatosI.consulta_reader("Select cpn_reg FROM registcalib WHERE bascula_reg ='" & LibreriaGeneral.nBascu & "'")

                If dTableII.Rows.Count > 0 Then
                    'Setea Campos
                    For Each dRow As DataRow In dTableII.Rows
                        'Infomrativos
                        Calibracion = dRow.Item("cpn_reg").ToString
                    Next
                End If

                'Certificacion
                dTableII = DatosI.consulta_reader("Select cpn_reg FROM RegistCertifica WHERE bascula_reg ='" & LibreriaGeneral.nBascu & "'")

                If dTableII.Rows.Count > 0 Then
                    'Setea Campos
                    For Each dRow As DataRow In dTableII.Rows
                        'Infomrativos
                        Certificacion = dRow.Item("cpn_reg").ToString
                    Next
                End If


                For Each dRow As DataRow In dTableI.Rows

                    'Recorre el Cursor, en este caso es una unica linea
                    oSheet.Range("G2").Value = "'" & dRow.Item("ticketboleta_pes").ToString
                    oSheet.Range("C4").Value = dRow.Item("tipooperacion_pes").ToString
                    oSheet.Range("B6").Value = dRow.Item("placa_pes").ToString & " " & IIf(dRow.Item("nombre_tpt").ToString.Trim <> "", dRow.Item("nombre_tpt").ToString.Trim & "     UND.:  " & dRow.Item("numero_cfa").ToString, "")
                    oSheet.Range("B7").Value = dRow.Item("prefijocont_pes").ToString.Trim & "-" & dRow.Item("numidentcont_pes").ToString & " " & IIf(dRow.Item("tamanocon_pes").ToString.Trim <> "", dRow.Item("tamanocon_pes").ToString, "")
                    oSheet.Range("B8").Value = dRow.Item("nombreclase_cla").ToString


                    oSheet.Range("B9").Value = dRow.Item("nombre_pro").ToString
                    oSheet.Range("B10").Value = dRow.Item("nombre_cli").ToString
                    oSheet.Range("B11").Value = dRow.Item("nombre_buq").ToString
                    oSheet.Range("B12").Value = dRow.Item("Idvbuq_pes").ToString
                    oSheet.Range("D12").Value = dRow.Item("bodegabuq_pes").ToString

                    '-Peso
                    oSheet.Range("G8").Value = dRow.Item("pesomanif_pes").ToString
                    oSheet.Range("G9").Value = dRow.Item("pesobruto_pes").ToString
                    oSheet.Range("G10").Value = dRow.Item("pesotaracont_pes").ToString
                    oSheet.Range("G11").Value = dRow.Item("pesoarana_pes").ToString
                    oSheet.Range("G12").Value = dRow.Item("pesotara_pes").ToString

                    '-Calcula el Peso Neto
                    If dRow.Item("tipooperacion_pes").ToString.Trim = "IMPORTACION" Then

                        If dRow.Item("EstePesaje_pes").ToString.Trim = "ENTRADA" Then
                            Neto = Convert.ToDouble(dRow.Item("pesotara_pes").ToString)
                        Else
                            Neto = Convert.ToDouble(dRow.Item("pesobruto_pes").ToString) - (Convert.ToDouble(dRow.Item("pesotara_pes").ToString) + _
                                                                                            Convert.ToDouble(dRow.Item("pesotaracont_pes").ToString) + _
                                                                                            Convert.ToDouble(dRow.Item("pesoarana_pes").ToString))
                            Vgm = Neto + Convert.ToDouble(dRow.Item("pesotaracont_pes").ToString)
                        End If

                        'De una vez impreime el Pais
                        oSheet.Range("A14").Value = "DESTINO:"
                        oSheet.Range("B14").Value = dRow.Item("pdestino").ToString

                    Else   'EXPORT
                        If dRow.Item("EstePesaje_pes").ToString.Trim = "ENTRADA" Then
                            Neto = Convert.ToDouble(dRow.Item("pesobruto_pes").ToString)
                        Else
                            Neto = Convert.ToDouble(dRow.Item("pesobruto_pes").ToString) - (Convert.ToDouble(dRow.Item("pesotara_pes").ToString) + _
                                                                                            Convert.ToDouble(dRow.Item("pesotaracont_pes").ToString) + _
                                                                                            Convert.ToDouble(dRow.Item("pesoarana_pes").ToString))
                            Vgm = Neto + Convert.ToDouble(dRow.Item("pesotaracont_pes").ToString)
                        End If

                        'De una vez impreime el Pais
                        oSheet.Range("A14").Value = "PROCEDENCIA:"
                        oSheet.Range("B14").Value = dRow.Item("porigien").ToString

                    End If

                    oSheet.Range("G13").Value = Neto
                    oSheet.Range("G14").Value = Vgm

                    If dRow.Item("EstePesaje_pes").ToString.Trim = "ENTRADA" Then
                        oSheet.Range("B17").Value = dRow.Item("idusu_entrada_pes").ToString
                        oSheet.Range("D17").Value = "BASCULA " & dRow.Item("Idbas_entrada_pes").ToString
                        oSheet.Range("G17").Value = dRow.Item("fechahora_entrada_pes").ToString
                    End If

                    If dRow.Item("EstePesaje_pes").ToString.Trim = "SALIDA" Then
                        'Datos Entrada
                        oSheet.Range("B17").Value = dRow.Item("idusu_entrada_pes").ToString
                        oSheet.Range("D17").Value = "BASCULA " & dRow.Item("Idbas_entrada_pes").ToString
                        oSheet.Range("G17").Value = dRow.Item("fechahora_entrada_pes").ToString

                        oSheet.Range("B18").Value = dRow.Item("idusu_salida_pes").ToString
                        oSheet.Range("D18").Value = "BASCULA " & dRow.Item("Idbas_salida_pes").ToString
                        oSheet.Range("G18").Value = dRow.Item("fechahora_salida_pes").ToString
                    End If

                    'Certificacion y Calibracion
                    oSheet.Range("B20").Value = "'" & Certificacion
                    oSheet.Range("B21").Value = "'" & Calibracion
                Next

                oSheet.PrintOut()

                ''Cierra el Documento
                oXL.Visible = False
                oXL.UserControl = False
                oXL.Workbooks(1).Close(SaveChanges:=False)
                'oXL.Workbooks

                'Make sure that you release object references.
                oXL.Quit()

                ' Liberar el objeto Excel.Range utilizado
                ReleaseComObject(oWB)
                ReleaseComObject(oSheet)
                ReleaseComObject(oXL)


                'System.Runtime.InteropServices.Marshal.ReleaseComObject(oXL)

                'Limpia la Memoria y borrar el Archivo .PRN
                'CerrarProcesoExcel()
                GC.Collect()
                GC.WaitForPendingFinalizers()
                GC.Collect()
            End If
        Catch ex As Exception
            MsgBox("No se Genero Impresión de Recibo", ex.Message)
        End Try
    End Sub


    'Imprime Tiket en una impresora TM-U220A
    Sub Imprime_Ticket(ByVal N_Ticket As String, ByVal _EstePeso As String)

        Dim printDoc As New PrintDocument
        AddHandler printDoc.PrintPage, AddressOf ImprTicket

        Try
            LibreriaGeneral._EstePeso = _EstePeso
            LibreriaGeneral.N_Ticket = N_Ticket

            printDoc.PrinterSettings.PrinterName = "EPSON TM-U220 Receipt"
            printDoc.Print()

        Catch ex As Exception
            MessageBox.Show("No se Genero Impresión de Ticket " + ex.Message)
        Finally
            printDoc = Nothing
        End Try
    End Sub

    'Código:
    Private Sub ImprTicket(ByVal sender As Object, ByVal e As PrintPageEventArgs)

        On Error Resume Next

        Dim xPos As Single = 0
        Dim prFont As New Font("Courier New", 10, FontStyle.Bold)
        Dim yPos As Single = 0

        'Logo de la Empresa
        'Dim cFile As String = IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.GetName.CodeBase).Remove(0, 6) & "\LogoEmp.jpg"

        'Variables de Ticket Cabecera
        Dim Empresa As String = vbNullString
        Dim Empresa2 As String = vbNullString
        Dim Empresa3 As String = vbNullString
        Dim Boleta As String = vbNullString
        Dim TicketNo As String = vbNullString
        Dim TipoOpereacion As String = vbNullString
        Dim Placa As String = vbNullString
        Dim Transporte As String = vbNullString
        Dim Contenedor As String = vbNullString
        Dim TamanoContenedor As String = vbNullString
        Dim TipoCarga As String = vbNullString
        Dim Producto As String = vbNullString
        Dim Vaniera As String = vbNullString
        Dim Barco As String = vbNullString
        Dim Viaje As String = vbNullString
        Dim Bodega As String = vbNullString

        Dim Bl As String = vbNullString
        Dim Consingna As String = vbNullString
        Dim Consingna2 As String = vbNullString
        Dim Duca As String = vbNullString

        'Variables de Ticket Detalle
        Dim PesoManifiesto As String = vbNullString
        Dim PesoBruto As String = vbNullString
        Dim TaraContenedor As String = vbNullString
        Dim TaraAraña As String = vbNullString
        Dim TaraEquipo As String = vbNullString
        Dim PesoNeto As String = vbNullString
        Dim VGM As String = vbNullString
        Dim Neto As Integer = 0
        Dim nVGM As Integer = 0
        Dim DestinoPro As String = vbNullString


        Dim Lineas As String = vbNullString
        Dim uLineas As String = vbNullString

        Dim Calibracion As String = vbNullString
        Dim Certificacion As String = vbNullString
        Dim FehayHEntrada As String = vbNullString
        Dim FehcayHSalida As String = vbNullString
        Dim Entrada As String = vbNullString
        Dim Salida As String = vbNullString

        'Variablas de uso SQL
        Dim dTableI As New DataTable
        Dim dTableII As New DataTable
        Dim DatosI As New DatosGenereral()

        'Arma Cursor de SAP, Para impresion de Factura.
        Dim StringSql As String = <sqlExp>
                                        EXEC [dbo].[sp_ImprBoleta] '<%= LibreriaGeneral._EstePeso %>','<%= LibreriaGeneral.N_Ticket %>'
                                  </sqlExp>.Value

        'Para mostrar el Dato
        dTableI = DatosI.consulta_reader(StringSql)

        If dTableI.Rows.Count > 0 Then

            Lineas = "______________________________"

            uLineas = "     ** Ultima Linea **     "

            'Para mostrar el Dato
            dTableII = DatosI.consulta_reader("Select cpn_reg FROM registcalib WHERE bascula_reg ='" & LibreriaGeneral.nBascu & "'")

            If dTableII.Rows.Count > 0 Then
                'Setea Campos
                For Each dRow As DataRow In dTableII.Rows
                    'Infomrativos
                    Calibracion = "CALIBRACION:.. " & dRow.Item("cpn_reg").ToString
                Next
            End If

            'Certificacion
            dTableII = DatosI.consulta_reader("Select cpn_reg FROM RegistCertifica WHERE bascula_reg ='" & LibreriaGeneral.nBascu & "'")

            If dTableII.Rows.Count > 0 Then
                'Setea Campos
                For Each dRow As DataRow In dTableII.Rows
                    'Infomrativos
                    Certificacion = "CERTIFICADO:.. " & dRow.Item("cpn_reg").ToString
                Next
            End If

            For Each dRow As DataRow In dTableI.Rows

                Empresa = "* C O G R A N S A *"
                Empresa2 = "        Corporación          "
                Empresa3 = "Granelera del Atlántico, S.A."
                Boleta = "** BOLETA DE PESAJE **"
                TicketNo = "    No. BOLETA: " & dRow.Item("ticketboleta_pes").ToString

                TipoOpereacion = "TIPO DE OPERACIÓN: " & dRow.Item("tipooperacion_pes").ToString
                Placa = "PLACA:....... " & dRow.Item("placa_pes").ToString & " "
                Transporte = IIf(dRow.Item("nombre_tpt").ToString.Trim <> "", dRow.Item("nombre_tpt").ToString.Trim & "     UND.:  " & dRow.Item("numero_cfa").ToString.Trim, "")

                'Se modifico Contenedor
                If dRow.Item("numidentcont_pes").ToString.Trim <> String.Empty Then
                    Contenedor = "CONTENEDOR:.. " & dRow.Item("prefijocont_pes").ToString.Trim & "-" & dRow.Item("numidentcont_pes").ToString.Trim
                    TamanoContenedor = "TAMAÑO:...... " & dRow.Item("tamanocon_pes").ToString.Trim
                Else
                    Contenedor = ""
                    TamanoContenedor = ""
                End If

                TipoCarga = "TIPO CARGA:.. " & dRow.Item("nombreclase_cla").ToString.Trim
                Producto = "PRODUCTO:.... " & dRow.Item("nombre_pro").ToString.Trim
                Vaniera = "EMPRESA:..... " & dRow.Item("nombre_cli").ToString.Trim
                Barco = "BARCO:....... " & dRow.Item("nombre_buq").ToString.Trim
                Viaje = "VIAJE:....... " & dRow.Item("Idvbuq_pes").ToString.Trim
                Bodega = "BODEGA:...... " & dRow.Item("nombre_bod").ToString.Trim

                'Se Agrego BL
                If dRow.Item("bl_dbq").ToString.Trim <> String.Empty Then
                    Bl = "BL:.......... " & dRow.Item("bl_dbq").ToString.Trim
                    Consingna = "CONSIGNATARIO:" & dRow.Item("nombre_csg").ToString.Trim
                    Duca = "DUCA:........ " & dRow.Item("duca_dbq").ToString.Trim
                Else
                    Bl = ""
                    Consingna = ""
                    Duca = ""
                    Bodega = ""
                End If

                PesoManifiesto = "PESO MANIFESTADO: " & TextoDerecha(Format(Val("0"), "#####,##0"))
                PesoBruto = "PESO BRUTO:...... " & TextoDerecha(Format(Val(dRow.Item("pesobruto_pes").ToString), "#######,##0"))

                If Val(dRow.Item("pesotaracont_pes").ToString) > 0 Then
                    TaraContenedor = "TARA CONTENEDOR:. " & TextoDerecha(Format(Val(dRow.Item("pesotaracont_pes").ToString), "#######,##0"))
                Else
                    TaraContenedor = ""
                End If
                If Val(dRow.Item("pesoarana_pes").ToString) > 0 Then
                    TaraAraña = "TARA ARAÑA:...... " & TextoDerecha(Format(Val(dRow.Item("pesoarana_pes").ToString), "#######,##0"))
                Else
                    TaraAraña = ""
                End If

                TaraEquipo = "TARA EQUIPO:..... " & TextoDerecha(Format(Val(dRow.Item("pesotara_pes").ToString), "#######,##0"))

                '-Calcula el Peso Neto
                If dRow.Item("tipooperacion_pes").ToString.Trim = "DESPACHO" Then

                    If dRow.Item("EstePesaje_pes").ToString.Trim = "ENTRADA" Then
                        Neto = Format(Val(dRow.Item("pesotara_pes").ToString), "#####,##0")

                    Else   'Salida Import
                        Neto = Convert.ToDouble(dRow.Item("pesobruto_pes").ToString) - (Convert.ToDouble(dRow.Item("pesotara_pes").ToString) +
                                                                                        Convert.ToDouble(dRow.Item("pesotaracont_pes").ToString) +
                                                                                        Convert.ToDouble(dRow.Item("pesoarana_pes").ToString))
                        nVGM = Neto + Convert.ToDouble(dRow.Item("pesotaracont_pes").ToString)
                    End If

                    'De una vez imprime el Pais
                    DestinoPro = "DESTINO:..... " & dRow.Item("pdestino").ToString

                Else   'EXPORT

                    If dRow.Item("EstePesaje_pes").ToString.Trim = "ENTRADA" Then
                        Neto = Format(Val(dRow.Item("pesobruto_pes").ToString) - (Val(dRow.Item("pesotaracont_pes").ToString) + Val(dRow.Item("pesoarana_pes").ToString)), "#####,##0")
                    Else
                        Neto = Format(Val(dRow.Item("pesobruto_pes").ToString) - (Val(dRow.Item("pesotara_pes").ToString) + Val(dRow.Item("pesotaracont_pes").ToString) + Val(dRow.Item("pesoarana_pes").ToString)), "#####,##0")

                        nVGM = Format(Neto + Convert.ToDouble(dRow.Item("pesotaracont_pes").ToString), "#####,##0")
                    End If

                    'De una vez imprime el Pais
                    DestinoPro = "PROCEDENCIA:. " & dRow.Item("porigien").ToString
                End If


                PesoNeto = "PESO NETO:....... " & TextoDerecha(Format(Val(Neto), "#####,##0"))

                'Si es Granel le quito el VGM
                If Contenedor <> String.Empty Then
                    VGM = "VGM:............. " & TextoDerecha(Format(Val(nVGM), "#####,##0"))
                End If

                If dRow.Item("EstePesaje_pes").ToString.Trim = "ENTRADA" Then
                    Entrada = "ENTRADA: " & IIf(dRow.Item("idusu_entrada_pes").ToString.Length > 10, dRow.Item("idusu_entrada_pes").ToString.Substring(0, 10), dRow.Item("idusu_entrada_pes").ToString) & " Bascula " & dRow.Item("Idbas_entrada_pes").ToString.Trim
                    FehayHEntrada = "F/H ENT.:" & dRow.Item("fechahora_entrada_pes").ToString.Trim

                    Salida = ""
                    FehcayHSalida = ""

                End If

                If dRow.Item("EstePesaje_pes").ToString.Trim = "SALIDA" Then
                    'Datos Entrada
                    Entrada = "ENTRADA: " & IIf(dRow.Item("idusu_entrada_pes").ToString.Length > 10, dRow.Item("idusu_entrada_pes").ToString.Substring(0, 10), dRow.Item("idusu_entrada_pes").ToString) & " BASCULA " & dRow.Item("Idbas_entrada_pes").ToString.Trim
                    FehayHEntrada = "F/H ENTR.: " & dRow.Item("fechahora_entrada_pes").ToString.Trim

                    Salida = "SALIDA:. " & IIf(dRow.Item("idusu_salida_pes").ToString.Length, dRow.Item("idusu_salida_pes").ToString.Substring(0, 10), dRow.Item("idusu_salida_pes").ToString) & " BASCULA " & dRow.Item("Idbas_salida_pes").ToString.Trim
                    FehcayHSalida = "F/H SALD.: " & dRow.Item("fechahora_salida_pes").ToString
                End If

                'Sucursal…

                'Logotipo de la empresa
                'yPos = yPos + 10 : e.Graphics.DrawImage(Image.FromFile(cFile), xPos, yPos)
                'yPos = yPos + 120

                xPos = 10
                yPos = 0

                'Datos de imprision
                'Encabezado
                prFont = New Font("Courier New", 14, FontStyle.Bold)
                yPos = yPos : e.Graphics.DrawString(Empresa, prFont, Brushes.Red, xPos, yPos)
                prFont = New Font("Courier New", 9, FontStyle.Regular)
                yPos = yPos + 20 : e.Graphics.DrawString(Empresa2, prFont, Brushes.Red, xPos, yPos)
                yPos = yPos + 10 : e.Graphics.DrawString(Empresa3, prFont, Brushes.Red, xPos, yPos)

                prFont = New Font("Courier New", 12, FontStyle.Bold)
                yPos = yPos + 30 : e.Graphics.DrawString(Boleta, prFont, Brushes.Red, xPos, yPos)
                yPos = yPos + 20 : e.Graphics.DrawString(TicketNo, prFont, Brushes.Red, xPos, yPos)

                'Datos del Ticket
                prFont = New Font("Courier New", 9, FontStyle.Regular)
                yPos = yPos + 20 : e.Graphics.DrawString(TipoOpereacion, prFont, Brushes.Black, xPos, yPos)

                yPos = yPos + 20 : e.Graphics.DrawString(Lineas, prFont, Brushes.Black, xPos, yPos)

                yPos = yPos + 30 : e.Graphics.DrawString(Placa, prFont, Brushes.Black, xPos, yPos)
                yPos = yPos + 10 : e.Graphics.DrawString(Transporte, prFont, Brushes.Black, xPos, yPos)

                If Contenedor <> String.Empty Then
                    yPos = yPos + 20 : e.Graphics.DrawString(Contenedor, prFont, Brushes.Black, xPos, yPos)
                End If
                If TamanoContenedor <> String.Empty Then
                    yPos = yPos + 20 : e.Graphics.DrawString(TamanoContenedor, prFont, Brushes.Black, xPos, yPos)
                End If

                yPos = yPos + 20 : e.Graphics.DrawString(TipoCarga, prFont, Brushes.Black, xPos, yPos)
                yPos = yPos + 20 : e.Graphics.DrawString(Producto, prFont, Brushes.Black, xPos, yPos)
                yPos = yPos + 20 : e.Graphics.DrawString(Vaniera, prFont, Brushes.Black, xPos, yPos)
                yPos = yPos + 20 : e.Graphics.DrawString(Barco, prFont, Brushes.Black, xPos, yPos)
                yPos = yPos + 20 : e.Graphics.DrawString(Viaje, prFont, Brushes.Black, xPos, yPos)

                'Se Agrego BL
                If Bl <> String.Empty Then
                    yPos = yPos + 20 : e.Graphics.DrawString(Bodega, prFont, Brushes.Black, xPos, yPos)

                    yPos = yPos + 20 : e.Graphics.DrawString(Bl, prFont, Brushes.Black, xPos, yPos)
                    yPos = yPos + 20 : e.Graphics.DrawString(Consingna, prFont, Brushes.Black, xPos, yPos)
                    yPos = yPos + 20 : e.Graphics.DrawString(Duca, prFont, Brushes.Black, xPos, yPos)
                End If

                yPos = yPos + 30 : e.Graphics.DrawString("      DETALLE PESO KG.", prFont, Brushes.Black, xPos, yPos)
                yPos = yPos + 10 : e.Graphics.DrawString(Lineas, prFont, Brushes.Black, xPos, yPos)

                yPos = yPos + 20 : e.Graphics.DrawString(PesoManifiesto, prFont, Brushes.Black, xPos, yPos)
                yPos = yPos + 20 : e.Graphics.DrawString(PesoBruto, prFont, Brushes.Black, xPos, yPos)

                If TaraContenedor <> String.Empty Then
                    yPos = yPos + 20 : e.Graphics.DrawString(TaraContenedor, prFont, Brushes.Black, xPos, yPos)
                End If

                If TaraAraña <> String.Empty Then
                    yPos = yPos + 20 : e.Graphics.DrawString(TaraAraña, prFont, Brushes.Black, xPos, yPos)
                End If

                yPos = yPos + 20 : e.Graphics.DrawString(TaraEquipo, prFont, Brushes.Black, xPos, yPos)
                yPos = yPos + 20 : e.Graphics.DrawString(PesoNeto, prFont, Brushes.Black, xPos, yPos)

                If Contenedor <> String.Empty Then
                    yPos = yPos + 20 : e.Graphics.DrawString(VGM, prFont, Brushes.Black, xPos, yPos)
                End If

                yPos = yPos + 20 : e.Graphics.DrawString(Lineas, prFont, Brushes.Black, xPos, yPos)

                yPos = yPos + 20 : e.Graphics.DrawString(FehayHEntrada, prFont, Brushes.Black, xPos, yPos)
                yPos = yPos + 20 : e.Graphics.DrawString(Entrada, prFont, Brushes.Black, xPos, yPos)
                yPos = yPos + 20 : e.Graphics.DrawString(FehcayHSalida, prFont, Brushes.Black, xPos, yPos)
                yPos = yPos + 20 : e.Graphics.DrawString(Salida, prFont, Brushes.Black, xPos, yPos)

                yPos = yPos + 30 : e.Graphics.DrawString(Calibracion, prFont, Brushes.Black, xPos, yPos)
                yPos = yPos + 20 : e.Graphics.DrawString(Certificacion, prFont, Brushes.Black, xPos, yPos)

                yPos = yPos + 40 : e.Graphics.DrawString(uLineas, prFont, Brushes.Black, xPos, yPos)

                e.HasMorePages = False

            Next dRow

        End If

    End Sub


    Function TextoDerecha(ByVal par1 As String) As String
        Dim ticket As String = ""
        Dim max As Integer = par1.Length
        Dim cort As Integer = 0
        Dim parte1 As Integer = 0

        If (max > 12) Then
            cort = max - 12
            parte1 = par1.Remove(12, cort)           '// si es mayor que 40 caracteres, lo corta
        Else
            parte1 = par1                           '// **********
            max = 12 - par1.Length                  '/ obtiene la cantidad de espacios para llegar a 12
            For i As Integer = 1 To max
                ticket += " "                      '// agrega espacios para alinear a la derecha
                '//Agrega el texto
            Next
        End If
        ticket = ticket & par1

        Return ticket

    End Function

    'Impresion Factura Salvador y Honduras
    Public Sub Reporte_PorFecha(ByVal Fecha1 As DateTime, ByVal Fecha2 As DateTime)

        Dim dTableI As New DataTable
        Dim DatosI As New DatosGenereral()

        ' ''Arma Cursor de SAP, Para impresion de Factura.
        ''Dim StringSql As String = <sqlExp>
        ''                             -- exec sp_imprBoleta ='<%= _EstePeso %>','<%= N_Ticket %>'
        ''                          </sqlExp>.Value

        ''Arma Cursor de SAP, Para impresion de Factura.
        'Dim StringSql As String = <sqlExp>
        '                                EXEC [dbo].[sp_ImprBoleta] '<%= _EstePeso %>','<%= N_Ticket %>'
        '                          </sqlExp>.Value


        'Para mostrar el Dato
        'Arma Cursor de SAP, Para impresion de Factura.
        Dim StringSql As String = <sqlExp>
                                      
                                	SELECT a.ticketboleta_pes, 
		                                        a.placa_pes,
                                            	ISNULL(fc.nombre_tpt,'') As nombre_tpt,
					                            ISNULL(fc.numero_fca,'') As numero_cfa,
		                                        a.Idcla_pes,
		                                        b.nombreclase_cla,
		                                        ISNULL(a.prefijocont_pes,'') As prefijocont_pes,
		                                        ISNULL(a.numidentcont_pes,'') As numidentcont_pes,
		                                        ISNULL(a.Idcon_pes,'') As Idcon_pes,
		                                        ISNULL(a.tamanocon_pes,'') As tamanocon_pes,
		                                        a.dicecontener_pes,
		                                        a.tipocarga_pes,
		                                        a.Idpro_pes,
		                                        g.nombre_pro,
		                                        a.tipooperacion_pes,
		                                        a.Idbuq_pes,
		                                        c.nombre_buq,
		                                        a.bodegabuq_pes,
		                                        a.Idvbuq_pes,
		                                        a.Idcli_pes,
                                                d.nombre_cli,
		                                        a.Idpaisdest_pes,
	                                            e.nombre_pai As pdestino,
		                                        a.Idpaisorig_pes,
                                                f.nombre_pai As porigien,
		                                        a.pesomanif_pes,
		                                        a.pesotara_pes,
		                                        a.pesotaracont_pes,
		                                        a.pesobruto_pes,
		                                        a.pesoarana_pes,
		                                        ISNULL(a.Idbas_entrada_pes,'') As Idbas_entrada_pes,
		                                        ISNULL(a.fechahora_entrada_pes,'') As fechahora_entrada_pes,
		                                        ISNULL(a.idusu_entrada_pes,'') As idusu_entrada_pes,
		                                        ISNULL(a.Idbas_salida_pes,'') As Idbas_salida_pes,
		                                        ISNULL(a.fechahora_salida_pes,'') As fechahora_salida_pes,
		                                        ISNULL(a.idusu_salida_pes,'') As idusu_salida_pes,
		                                        ISNULL(a.EstePesaje_pes,'') As EstePesaje_pes
                                        FROM pesajes a
                                            LEFT JOIN clasecarga b ON b.Id_cla = a.Idcla_pes
                                            LEFT JOIN buques c ON c.Id_buq = a.Idbuq_pes 
                                            LEFT JOIN Cliente d ON d.Id_cli = a.Idcli_pes
                                            LEFT JOIN paisdestorig e ON e.Id_pai = a.Idpaisdest_pes
                                            LEFT JOIN paisdestorig f ON f.id_pai = a.Idpaisorig_pes
                                            LEFT JOIN Producto g ON g.Id_pro = a.Idpro_pes
                                            LEFT JOIN (select c1.*,t1.nombre_tpt from fcamiones c1
							                        LEFT JOIN Transporte t1 ON t1.Id_tpt = c1.transporte_fca) fc ON fc.placa_fca = a.placa_pes
        							    WHERE convert(date, fechahora_salida_pes,103) BETWEEN '<%= Format(Fecha1, "yyyyMMdd") %>' AND '<%= Format(Fecha2, "yyyyMMdd") %>';
                               </sqlExp>.Value


        ''Para mostrar el Dato
        dTableI = DatosI.consulta_reader(StringSql)

        Try

            If dTableI.Rows.Count > 0 Then

                '//IMPRESION RECIBO DESDE EXCEL
                '--------------------------------------------------------------------------------
                Dim oXL As New Excel.Application
                Dim oWB As Excel.Workbook
                Dim oSheet As Excel.Worksheet
                'Dim oRng As Excel.Range

                'Referenciamos el Objeto Excel
                'oXL = CreateObject("Excel.Application")
                oXL.Visible = False
                oXL.DisplayAlerts = False

                Dim cFile As String = ""
                Dim cadena As String = ""

                'Localiza la Ruta del Archivo que usaremos como Plantilla
                cFile = IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.GetName.CodeBase).Remove(0, 6) & "\Plantilla_PorFecha.xlsx"

                Dim Neto As Double = 0.0
                Dim Vgm As Double = 0.0

                'Abre Archivo
                oWB = oXL.Workbooks.Open(cFile)
                oSheet = oWB.ActiveSheet

                Dim Lin As Integer = 6
                Dim nTotLin As Integer = 0

                'Recorre el Cursor, en este caso es una unica linea
                oSheet.Range("C2").Value = "INFORME DE PESAJES DURANTE  La Fecha: " & Format(Fecha1, "dd-MM-yyyy").ToString & "  HASTA  " & Format(Fecha2, "dd-MM-yyyy").ToString


                For Each dRow As DataRow In dTableI.Rows

                    'Recorre el Cursor, en este caso es una unica linea
                    oSheet.Range("A" + Convert.ToString(Lin).Trim).Value = dRow.Item("fechahora_salida_pes").ToString

                    oSheet.Range("C" + Convert.ToString(Lin).Trim).Value = dRow.Item("nombre_pro").ToString
                    oSheet.Range("D" + Convert.ToString(Lin).Trim).Value = dRow.Item("numero_cfa").ToString
                    oSheet.Range("E" + Convert.ToString(Lin).Trim).Value = dRow.Item("placa_pes").ToString
                    oSheet.Range("F" + Convert.ToString(Lin).Trim).Value = dRow.Item("nombre_cli").ToString
                    oSheet.Range("G" + Convert.ToString(Lin).Trim).Value = "'" & dRow.Item("ticketboleta_pes").ToString
                    oSheet.Range("H" + Convert.ToString(Lin).Trim).Value = dRow.Item("fechahora_salida_pes").ToString
                    oSheet.Range("I" + Convert.ToString(Lin).Trim).Value = dRow.Item("tipooperacion_pes").ToString.Substring(0, 3)

                    oSheet.Range("J" + Convert.ToString(Lin).Trim).Value = dRow.Item("nombre_buq").ToString
                    oSheet.Range("K" + Convert.ToString(Lin).Trim).Value = dRow.Item("Idvbuq_pes").ToString
                    oSheet.Range("L" + Convert.ToString(Lin).Trim).Value = dRow.Item("bodegabuq_pes").ToString

                    '-Peso
                    oSheet.Range("M" + Convert.ToString(Lin).Trim).Value = dRow.Item("pesobruto_pes").ToString
                    oSheet.Range("N" + Convert.ToString(Lin).Trim).Value = dRow.Item("pesotara_pes").ToString

                    '-Calcula el Peso Neto
                    If dRow.Item("tipooperacion_pes").ToString.Trim = "IMPORTACION" Then

                        If dRow.Item("EstePesaje_pes").ToString.Trim = "ENTRADA" Then
                            Neto = Convert.ToDouble(dRow.Item("pesotara_pes").ToString)
                        Else
                            Neto = Convert.ToDouble(dRow.Item("pesobruto_pes").ToString) - (Convert.ToDouble(dRow.Item("pesotara_pes").ToString) + _
                                                                                            Convert.ToDouble(dRow.Item("pesotaracont_pes").ToString) + _
                                                                                            Convert.ToDouble(dRow.Item("pesoarana_pes").ToString))
                            Vgm = Neto + Convert.ToDouble(dRow.Item("pesotaracont_pes").ToString)
                        End If

                        'De una vez impreime el Pais
                        oSheet.Range("B5").Value = "DESTINO:"
                        oSheet.Range("B" + Convert.ToString(Lin).Trim).Value = dRow.Item("pdestino").ToString

                    Else   'EXPORT
                        If dRow.Item("EstePesaje_pes").ToString.Trim = "ENTRADA" Then
                            Neto = Convert.ToDouble(dRow.Item("pesobruto_pes").ToString)
                        Else
                            Neto = Convert.ToDouble(dRow.Item("pesobruto_pes").ToString) - (Convert.ToDouble(dRow.Item("pesotara_pes").ToString) + _
                                                                                            Convert.ToDouble(dRow.Item("pesotaracont_pes").ToString) + _
                                                                                            Convert.ToDouble(dRow.Item("pesoarana_pes").ToString))
                            Vgm = Neto + Convert.ToDouble(dRow.Item("pesotaracont_pes").ToString)
                        End If

                        'De una vez impreime el Pais
                        oSheet.Range("B5").Value = "PROCEDENCIA:"
                        oSheet.Range("B" + Convert.ToString(Lin).Trim).Value = dRow.Item("porigien").ToString

                    End If

                    oSheet.Range("O" + Convert.ToString(Lin).Trim).Value = Neto
                    oSheet.Range("P" + Convert.ToString(Lin).Trim).Value = Vgm

                    Lin = Lin + 1
                    nTotLin = nTotLin + 1
                Next

                'objRango = objHojaExcel.Range("A4:E4")
                'objRango.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                'objRango.Cells.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.XlLineStyle.xlLineStyleNone
                'objRango.Cells.Borders(Excel.XlBordersIndex.xlDiagonalUp).LineStyle = Excel.XlLineStyle.xlLineStyleNone
                'objRango.Cells.Borders(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlLineStyleNone
                'objRango.Cells.Borders(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous
                'objRango.Cells.Borders(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous
                'objRango.Cells.Borders(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous
                'objRango.Columns.AutoFit()

                nTotLin = nTotLin - 1

                Dim i As Integer = Lin - 1
                oSheet.Range("M" + Convert.ToString(Lin).Trim).Value = "=SUBTOTAL(9,M6:M" + Convert.ToString(i).Trim & ")"
                oSheet.Range("N" + Convert.ToString(Lin).Trim).Value = "=SUBTOTAL(9,N6:N" + Convert.ToString(i).Trim & ")"
                oSheet.Range("O" + Convert.ToString(Lin).Trim).Value = "=SUBTOTAL(9,O6:O" + Convert.ToString(i).Trim & ")"
                oSheet.Range("P" + Convert.ToString(Lin).Trim).Value = "=SUBTOTAL(9,P6:P" + Convert.ToString(i).Trim & ")"

                oSheet.Range("K" + Convert.ToString(Lin).Trim).Value = "TOTAL =>"
                oSheet.Range("F" + Convert.ToString(Lin).Trim).Value = "BOLETAS =>"
                nTotLin = nTotLin + 1
                oSheet.Range("G" + Convert.ToString(Lin).Trim).Value = nTotLin.ToString

                'oSheet.GET_Range("M17:P17").Borders(oXL.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous
                ''oRng = oSheet.Range("M" + Convert.ToString(Lin).Trim + ":P" + Convert.ToString(Lin).Trim 
                ' oRng = oSheet.Range("M17:P17")

                'oRng = oSheet.Range("M" & Convert.ToString(Lin).Trim & ":P" & Convert.ToString(Lin).Trim)

                'oRng = oSheet.Range("M17:P17")
                'oRng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                'oRng.Cells.Borders(Excel.XlBordersIndex.xlDiagonalDown).LineStyle = Excel.XlLineStyle.xlContinuous

                'oRng.Columns.AutoFit()
                'oSheet.PrintOut()

                ''Cierra el Documento
                oXL.Visible = True
                oXL.UserControl = True
                'oXL.Workbooks(1).Close(SaveChanges:=False)
                'oXL.Workbooks

                'Make sure that you release object references.
                'oXL.Quit()

                '' Liberar el objeto Excel.Range utilizado
                'ReleaseComObject(oWB)
                'ReleaseComObject(oSheet)
                'ReleaseComObject(oXL)


                'System.Runtime.InteropServices.Marshal.ReleaseComObject(oXL)

                'Limpia la Memoria y borrar el Archivo .PRN
                'CerrarProcesoExcel()
                GC.Collect()
                GC.WaitForPendingFinalizers()
                GC.Collect()
            End If
        Catch ex As Exception
            MsgBox("No se Genero Impresión de Recibo", ex.Message)
        End Try
    End Sub


    ''' <summary>
    ''' Disminuye el recuento de referencias del contenedor RCW (Contenedor al que se
    ''' puede llamar en tiempo de ejecución) especificado, asociado al objeto COM
    ''' indicado. 
    ''' </summary>
    ''' <param name="obj">Objeto COM que se va a liberar.</param>
    ''' <remarks></remarks>
    Friend Sub ReleaseComObject(obj As Object)

        ' Ver en la ayuda de MSDN: Contenedor al que se puede llamar
        ' en tiempo de ejecución

        ' Una aplicación de Office no se cierra después de la automatización desde Visual Studio. NET
        ' Office application does not quit after automation from Visual Studio .NET client
        ' http://support.microsoft.com/kb/317109

        If (obj Is Nothing) Then Return

        Try
            While (System.Runtime.InteropServices.Marshal.ReleaseComObject(obj) > 0)
                ' Sin implementación
            End While

        Catch
            ' Deshechamos devolver la posible excepción
            ' si no es un objeto COM válido.

        Finally
            obj = Nothing

        End Try
    End Sub


End Class
