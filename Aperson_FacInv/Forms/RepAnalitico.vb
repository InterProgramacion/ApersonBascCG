Imports CrystalDecisions.Shared
Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Drawing.Printing

Public Class RepAnalitico

#Region "Variables"
    Dim Datos As New DatosGenereral()
    Dim dTable As New DataTable
    Dim Empresa As String = LibreriaGeneral.nEmpresa

    Dim Parametros As New ParameterFields()
    Dim paramFecha1 As New ParameterField()
    Dim paramFecha2 As New ParameterField()
    Dim paramHora1 As New ParameterField()
    Dim paramHora2 As New ParameterField()
    Dim paramBuque As New ParameterField()
    Dim paramViaje As New ParameterField()
    Dim paramBodega As New ParameterField()
    Dim paramProducto As New ParameterField()
    Dim paramNaviera As New ParameterField()

    Dim myDiscreteValue As New ParameterDiscreteValue()
    Dim myDiscreteValue2 As New ParameterDiscreteValue()
    Dim myDiscreteValue3 As New ParameterDiscreteValue()
    Dim myDiscreteValue4 As New ParameterDiscreteValue()
    Dim myDiscreteValue5 As New ParameterDiscreteValue()
    Dim myDiscreteValue6 As New ParameterDiscreteValue()
    Dim myDiscreteValue7 As New ParameterDiscreteValue()
    Dim myDiscreteValue8 As New ParameterDiscreteValue()
    Dim myDiscreteValue9 As New ParameterDiscreteValue()
    Dim myDiscreteValue10 As New ParameterDiscreteValue()


#End Region


    Private Sub RepAnalitico_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtBuque.Text = "X"
        txtNomBuque.Text = "TODOS LOS BUSQUES"
        txtViaje.Text = "0"
        txtBodega.Text = "0"
        txtProducto.Text = "X"
        txtNomProducto.Text = "TODOS LOS PRODUCTOS"
        txtNaviera.Text = "X"
        txtNomNaviera.Text = "TODAS LA EMPRESAS"
        dHoraIni.Text = "00:00"
        dHoraFin.Text = "23:59"
    End Sub


    Private Sub btnGrabar_Click(sender As Object, e As EventArgs) Handles btnGrabar.Click
        Dim Fecha1 As DateTime = dFechaIni.Value
        Dim Fecha2 As DateTime = dFechaFin.Value


        If chkTicket.Checked = False Then

            'Conecta
            Dim TuReporte As New Rep1                   'Un Reporte que tengas como pivote
            Dim ocInforme As New ReportDocument
            Dim formulario As New Genera_ReporteCatalogos   'Pantalla como objeto CrystalReportPreview
            Dim NomRepo As String = ""

            NomRepo = LibreriaGeneral.cPathReport & "\RepAnalisa3.rpt"                    'Nombre de la ubicacion y Nombre de Reporte a Mostrar


            If ChekProducto.Checked Then
                NomRepo = LibreriaGeneral.cPathReport & "\RepAnalisa1.rpt"
            End If
            If ChekProductoResumen.Checked Then
                NomRepo = LibreriaGeneral.cPathReport & "\RepAnalisa2.rpt"
            End If
            If ChekCamion.Checked Then
                NomRepo = LibreriaGeneral.cPathReport & "\RepAnalisa5.rpt"
            End If
            If ChekBuque.Checked Then
                NomRepo = LibreriaGeneral.cPathReport & "\RepAnalisa4.rpt"
            End If

            ocInforme.Load(NomRepo, OpenReportMethod.OpenReportByDefault)

            Dim conexion As ConnectionInfo = New ConnectionInfo
            conexion.ServerName = LibreriaGeneral.cServer
            conexion.DatabaseName = LibreriaGeneral.cBaseD
            conexion.UserID = LibreriaGeneral.cUsu
            conexion.Password = LibreriaGeneral.cPass
            conexion.Type = ConnectionInfoType.SQL

            For Each tab As Table In ocInforme.Database.Tables
                Dim tablas As TableLogOnInfo = tab.LogOnInfo
                tablas.ConnectionInfo = conexion
                tab.ApplyLogOnInfo(tablas)
            Next

            'Tipos de Parametro
            paramFecha1.ParameterValueType = ParameterValueKind.DateParameter
            paramFecha2.ParameterValueType = ParameterValueKind.DateParameter
            paramHora1.ParameterValueType = ParameterValueKind.StringParameter
            paramHora2.ParameterValueType = ParameterValueKind.StringParameter

            paramBuque.ParameterValueType = ParameterValueKind.StringParameter
            paramViaje.ParameterValueType = ParameterValueKind.NumberParameter
            paramBodega.ParameterValueType = ParameterValueKind.NumberParameter
            paramProducto.ParameterValueType = ParameterValueKind.StringParameter
            paramNaviera.ParameterValueType = ParameterValueKind.StringParameter

            'Asignamos parametro de Store Procedure
            paramFecha1.ParameterFieldName = "@FechaIni"
            paramFecha2.ParameterFieldName = "@FechaFin"
            paramHora1.ParameterFieldName = "@HoraIni"
            paramHora2.ParameterFieldName = "@HoraFin"

            paramBuque.ParameterFieldName = "@Buque"
            paramViaje.ParameterFieldName = "@Viaje"
            paramBodega.ParameterFieldName = "@Bodega"
            paramProducto.ParameterFieldName = "@Produc"
            paramNaviera.ParameterFieldName = "@Naviera"

            'Damos Valor a Cada Parametro
            myDiscreteValue.Value = dFechaIni.Value
            myDiscreteValue2.Value = dFechaFin.Value
            myDiscreteValue9.Value = dHoraIni.Text.Trim & ":00"
            myDiscreteValue10.Value = dHoraFin.Text.Trim & ":59"

            myDiscreteValue3.Value = txtBuque.Text
            myDiscreteValue4.Value = Convert.ToInt32(txtViaje.Text)
            myDiscreteValue5.Value = Convert.ToInt32(txtBodega.Text)
            myDiscreteValue7.Value = txtProducto.Text
            myDiscreteValue8.Value = txtNaviera.Text

            'Agregarmos el Valor al Parametro de Store Procedure
            paramFecha1.CurrentValues.Add(myDiscreteValue)
            paramFecha2.CurrentValues.Add(myDiscreteValue2)
            paramHora1.CurrentValues.Add(myDiscreteValue9)
            paramHora2.CurrentValues.Add(myDiscreteValue10)
            paramBuque.CurrentValues.Add(myDiscreteValue3)
            paramViaje.CurrentValues.Add(myDiscreteValue4)
            paramBodega.CurrentValues.Add(myDiscreteValue5)
            paramProducto.CurrentValues.Add(myDiscreteValue7)
            paramNaviera.CurrentValues.Add(myDiscreteValue8)

            'Los Adderimos al Reporte
            Parametros.Add(paramFecha1)
            Parametros.Add(paramFecha2)
            Parametros.Add(paramHora1)
            Parametros.Add(paramHora2)
            Parametros.Add(paramBuque)
            Parametros.Add(paramViaje)
            Parametros.Add(paramBodega)
            Parametros.Add(paramProducto)
            Parametros.Add(paramNaviera)

            formulario.CrystalReportViewer1.ParameterFieldInfo = Parametros    'Si usus parametros, si no quitaselo
            formulario.CrystalReportViewer1.ReportSource = ocInforme
            formulario.CrystalReportViewer1.Dock = DockStyle.Fill


            Dim frmReporte As New Form()
            With frmReporte
                .Controls.Add(formulario.CrystalReportViewer1)
                .Text = "Informes de Basculas"
                .WindowState = FormWindowState.Maximized
                .ShowDialog()
            End With
            conectarme.Close()

            ocInforme = Nothing
            formulario = Nothing
            frmReporte = Nothing
        Else

            Call Imprime_Bodega()

        End If

        GC.Collect()
        GC.WaitForPendingFinalizers()
        GC.Collect()


    End Sub


    Private Sub btnSalir_Click(sender As Object, e As EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    'Imprime Tiket en una impresora TM-U220A
    Sub Imprime_Bodega()

        Dim printDoc As New PrintDocument
        AddHandler printDoc.PrintPage, AddressOf ImprBodega

        Try
            printDoc.PrinterSettings.PrinterName = "EPSON TM-U220 Receipt"
            printDoc.Print()

        Catch ex As Exception
            MessageBox.Show("No se Genero Impresión de Ticket " + ex.Message)
        Finally
            printDoc = Nothing
        End Try
    End Sub


    ''Código:
    Private Sub ImprBodega(ByVal sender As Object, ByVal e As PrintPageEventArgs)

        On Error Resume Next

        Dim xPos As Single = 0
        Dim prFont As New Font("Courier New", 10, FontStyle.Bold)
        Dim yPos As Single = 0

        'Logo de la Empresa
        'Dim cFile As String = IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.GetName.CodeBase).Remove(0, 6) & "\LogoEmp.jpg"

        'Variables de Ticket Cabecera
        Dim Empresa As String = vbNullString
        Dim Empresa2 As String = vbNullString
        Dim Boleta As String = vbNullString
        Dim Fecha As String = vbNullString
        Dim TipoOpereacion As String = vbNullString
        Dim Contenedor As String = vbNullString
        Dim Bodega As String = vbNullString

        'Variables de Ticket Detalle
        Dim PesoManifiesto As String = vbNullString
        Dim TaraContenedor As String = vbNullString
        Dim PesoBruto As String = vbNullString
        Dim Neto As String = vbNullString
        Dim Titulo As String = vbNullString

        Dim Lineas As String = vbNullString
        Dim Lineasd As String = vbNullString
        Dim uLineas As String = vbNullString

        'Variablas de uso SQL
        Dim dTableI As New DataTable

        'Arma Cursor de SAP, Para impresion de Factura.
        Dim StringSql As String = <sqlExp>
                                    EXEC [dbo].[sp_RepAnaliticosIII] '<%= dFechaIni.Value %>','<%= dFechaFin.Value %>','<%= txtBuque.Text %>',<%= Convert.ToInt32(txtViaje.Text) %>,<%= Convert.ToInt32(txtBodega.Text) %>,'<%= txtProducto.Text %>','<%= txtNaviera.Text %>','<%= dHoraIni.Text %>','<%= dHoraFin.Text %>'
                              </sqlExp>.Value

        'Para mostrar el Dato
        dTableI = Datos.consulta_reader(StringSql)

        If dTableI.Rows.Count > 0 Then

            Lineas = "______________________________"
            Lineasd = "=============================="


            uLineas = "     ** Ultima Linea **     "

            Titulo = "PESO BRUTO" & " PESO TARA" & " PESO NETO"


            Empresa = " *- P A H A M E -*  "
            Empresa2 = "      Comercializadora   "
            TipoOpereacion = "** REPORTE BODEGAS **"

            xPos = 0
            yPos = 0

            'Encabezado
            prFont = New Font("Courier New", 14, FontStyle.Bold)
            yPos = yPos : e.Graphics.DrawString(Empresa, prFont, Brushes.Red, xPos, yPos)
            prFont = New Font("Courier New", 10, FontStyle.Bold)
            yPos = yPos + 20 : e.Graphics.DrawString(Empresa2, prFont, Brushes.Red, xPos, yPos)
            prFont = New Font("Courier New", 12, FontStyle.Bold)
            yPos = yPos + 20 : e.Graphics.DrawString(TipoOpereacion, prFont, Brushes.Red, xPos, yPos)
            prFont = New Font("Courier New", 9, FontStyle.Regular)
            yPos = yPos + 20 : e.Graphics.DrawString(Lineasd, prFont, Brushes.Black, xPos, yPos)
            yPos = yPos + 10 : e.Graphics.DrawString("BUQUE: " & dTableI.Rows(0).Item("nombre_buq").ToString.Substring(0, 20), prFont, Brushes.Black, xPos, yPos)
            yPos = yPos + 20 : e.Graphics.DrawString("VIAJE BUQUE: " & dTableI.Rows(0).Item("Idvbuq_pes").ToString, prFont, Brushes.Black, xPos, yPos)
            yPos = yPos + 20 : e.Graphics.DrawString(Lineasd, prFont, Brushes.Black, xPos, yPos)


            'Variables de Ticket Detalle
            Dim PesoBrutoT As Double = 0.0
            Dim NetoT As Double = 0.0
            Dim PesoManifiestoT As Double = 0.0
            Dim PesoBrutoTg As Double = 0.0
            Dim NetoTg As Double = 0.0
            Dim PesoManifiestoTg As Double = 0.0
            Dim PesajesTot As Double = 0.0

            Dim SumaPie As Integer = 0

            'Sub Cabecera
            Dim Bodega2 As String = dTableI.Rows(0).Item("Bodegabuq_pes").ToString

            yPos = yPos + 30 : e.Graphics.DrawString("BODEGA: " & dTableI.Rows(0).Item("Bodegabuq_pes").ToString, prFont, Brushes.Black, xPos, yPos)
            yPos = yPos + 20 : e.Graphics.DrawString(Titulo, prFont, Brushes.Black, xPos, yPos)
            yPos = yPos + 10 : e.Graphics.DrawString(Lineas, prFont, Brushes.Black, xPos, yPos)

            For Lin As Integer = 0 To dTableI.Rows.Count - 1

                'Bodega3 = dTableI.Rows(Lin).Item("Bodegabuq_pes").ToString

                'Imprime el Detalle por Bodega
                If Bodega2 <> dTableI.Rows(Lin).Item("Bodegabuq_pes").ToString Then

                    If SumaPie > 1 Then
                        'Imprime Totales
                        yPos = yPos + 10 : e.Graphics.DrawString(Lineas, prFont, Brushes.Black, xPos, yPos)
                        Contenedor = TextoDerecha2(Format(Val(PesoBrutoT), "####,##0")) & TextoDerecha2(Format(Val(PesoManifiestoT), "####,##0")) & TextoDerecha2(Format(Val(NetoT), "####,##0"))
                        yPos = yPos + 20 : e.Graphics.DrawString(Contenedor, prFont, Brushes.Black, xPos, yPos)
                    End If
                    yPos = yPos + 10 : e.Graphics.DrawString(Lineasd, prFont, Brushes.Black, xPos, yPos)


                    'Limpia Totales
                    PesoBrutoT = 0.0
                    NetoT = 0.0
                    PesoManifiestoT = 0.0
                    SumaPie = 0

                    yPos = yPos + 40 : e.Graphics.DrawString("BODEGA: " & dTableI.Rows(Lin).Item("Bodegabuq_pes").ToString, prFont, Brushes.Black, xPos, yPos)
                    yPos = yPos + 20 : e.Graphics.DrawString(Titulo, prFont, Brushes.Black, xPos, yPos)
                    yPos = yPos + 10 : e.Graphics.DrawString(Lineas, prFont, Brushes.Black, xPos, yPos)
                End If


                PesoBruto = Convert.ToDouble(dTableI.Rows(Lin).Item("pesobruto_pes").ToString)
                PesoManifiesto = (Convert.ToDouble(dTableI.Rows(Lin).Item("pesotara_pes").ToString) + Convert.ToDouble(dTableI.Rows(Lin).Item("pesotaracont_pes").ToString) + Convert.ToDouble(dTableI.Rows(Lin).Item("pesoarana_pes").ToString))
                Neto = Convert.ToDouble(dTableI.Rows(Lin).Item("pesobruto_pes").ToString) - (Convert.ToDouble(dTableI.Rows(Lin).Item("pesotara_pes").ToString) +
                                                                                Convert.ToDouble(dTableI.Rows(Lin).Item("pesotaracont_pes").ToString) +
                                                                                Convert.ToDouble(dTableI.Rows(Lin).Item("pesoarana_pes").ToString))

                Contenedor = TextoDerecha2(Format(Val(PesoBruto), "####,##0")) & TextoDerecha2(Format(Val(PesoManifiesto), "####,##0")) & TextoDerecha2(Format(Val(Neto), "####,##0"))

                yPos = yPos + 20 : e.Graphics.DrawString("FECHA: " & dTableI.Rows(Lin).Item("Fechahora_Salida_pes").ToString.Substring(0, 10) & " PJS: " & dTableI.Rows(Lin).Item("Pesajes_pes").ToString.Trim, prFont, Brushes.Black, xPos, yPos)
                yPos = yPos + 20 : e.Graphics.DrawString(Contenedor, prFont, Brushes.Black, xPos, yPos)


                'Suma SubTotal
                PesoBrutoT = PesoBrutoT + PesoBruto
                NetoT = NetoT + Neto
                PesoManifiestoT = PesoManifiestoT + PesoManifiesto

                'Suma Total
                PesoBrutoTg = PesoBrutoTg + PesoBruto
                NetoTg = NetoTg + Neto
                PesoManifiestoTg = PesoManifiestoTg + PesoManifiesto
                PesajesTot = PesajesTot + Convert.ToInt32(dTableI.Rows(Lin).Item("Pesajes_pes").ToString)

                Bodega2 = dTableI.Rows(Lin).Item("Bodegabuq_pes").ToString

                SumaPie = SumaPie + 1
            Next

            If SumaPie > 1 Then
                'Imprime Totales Ultima LInea
                yPos = yPos + 10 : e.Graphics.DrawString(Lineas, prFont, Brushes.Black, xPos, yPos)
                Contenedor = TextoDerecha2(Format(Val(PesoBrutoT), "####,##0")) & TextoDerecha2(Format(Val(PesoManifiestoT), "####,##0")) & TextoDerecha2(Format(Val(NetoT), "####,##0"))
                yPos = yPos + 20 : e.Graphics.DrawString(Contenedor, prFont, Brushes.Black, xPos, yPos)
                yPos = yPos + 10 : e.Graphics.DrawString(Lineasd, prFont, Brushes.Black, xPos, yPos)
            End If

            'Ultima Linea de totales
            yPos = yPos + 30 : e.Graphics.DrawString("TOT. PESO BRUTO: " & TextoDerecha3(Format(Val(PesoBrutoTg), "######,##0")), prFont, Brushes.Black, xPos, yPos)
            yPos = yPos + 20 : e.Graphics.DrawString("TOT. PESO TARA:. " & TextoDerecha3(Format(Val(PesoManifiestoTg), "######,##0")), prFont, Brushes.Black, xPos, yPos)
            yPos = yPos + 20 : e.Graphics.DrawString("TOT. PESO NETO:. " & TextoDerecha3(Format(Val(NetoTg), "######,##0")), prFont, Brushes.Black, xPos, yPos)
            yPos = yPos + 20 : e.Graphics.DrawString("TOT. PESAJES:... " & TextoDerecha3(Format(Val(PesajesTot), "######,##0")), prFont, Brushes.Black, xPos, yPos)


            yPos = yPos + 40 : e.Graphics.DrawString("     ** Ultima Linea **     ", prFont, Brushes.Black, xPos, yPos)

        End If


        e.HasMorePages = False

    End Sub


    Function TextoDerecha2(ByVal par1 As String) As String
        Dim ticket As String = ""
        Dim max As Integer = par1.Length
        Dim cort As Integer = 0
        Dim parte1 As Integer = 0

        If (max > 10) Then
            cort = max - 10
            parte1 = par1.Remove(10, cort)           '// si es mayor que 40 caracteres, lo corta
        Else
            parte1 = par1                           '// **********
            max = 10 - par1.Length                  '/ obtiene la cantidad de espacios para llegar a 12
            For i As Integer = 1 To max
                ticket += " "                      '// agrega espacios para alinear a la derecha
                '//Agrega el texto
            Next
        End If
        ticket = ticket & par1

        Return ticket

    End Function

    Function TextoDerecha3(ByVal par1 As String) As String
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


#Region "Busquedas"

    '--------------------------Despliega pantalla para buscar el Producto
    Private Sub BuscaProdu()
        Dim stringSQL As String

        stringSQL = "SELECT id_pro As Codigo, nombre_pro As Nombre FROM [dbo].[Producto]"

        'Script para Filtrar el Grid
        Dim stringSqlFiltro As String = "SELECT id_pro As Codigo, nombre_pro As Nombre FROM [dbo].[Producto] WHERE id_pro "

        Dim frmBuscar As New BuscaCodigo(stringSQL, {"Código", "Nombre", "50", "200"}, 2, stringSqlFiltro, "Lista Productos")
        frmBuscar.ShowDialog()

        'Retorna los datos del Cajero selecionado y 
        'se posiciona en Serie Recibo
        txtProducto.Text = ""
        If frmBuscar.Codigo <> String.Empty Then
            txtProducto.Text = frmBuscar.Codigo

            txtProducto.Focus()
        End If

    End Sub

    Private Sub btnBuscaProdu_Click(sender As Object, e As EventArgs)
        BuscaProdu()
    End Sub
    '-----------------------------------Fin Busca Producto

    '--------------------------Despliega pantalla para buscar el Buque
    Private Sub BuscaBuque()
        Dim stringSQL As String

        stringSQL = "SELECT Id_buq As Codigo, nombre_buq As Nombre FROM [dbo].[Buques]"

        'Script para Filtrar el Grid
        Dim stringSqlFiltro As String = "SELECT Id_buq As Codigo, nombre_buq As Nombre FROM [dbo].[Buques] WHERE Id_buq "

        Dim frmBuscar As New BuscaCodigo(stringSQL, {"Código", "Nombre", "50", "200"}, 2, stringSqlFiltro, "Lista Buques")
        frmBuscar.ShowDialog()

        'Retorna los datos del Cajero selecionado y 
        'se posiciona en Serie Recibo
        txtBuque.Text = ""
        If frmBuscar.Codigo <> String.Empty Then
            txtBuque.Text = frmBuscar.Codigo

            txtBuque.Focus()
        End If

    End Sub

    '--------------------------Despliega pantalla para buscar el Navoiera
    Private Sub BuscaNaviera()
        Dim stringSQL As String

        stringSQL = "SELECT id_cli As Codigo, nombre_cli As Nombre FROM [dbo].[Cliente]"

        'Script para Filtrar el Grid
        Dim stringSqlFiltro As String = "SELECT id_cli As Codigo, nombre_cli As Nombre FROM [dbo].[Cliente] WHERE id_cli "

        Dim frmBuscar As New BuscaCodigo(stringSQL, {"Código", "Nombre", "50", "200"}, 2, stringSqlFiltro, "Lista Clientes")
        frmBuscar.ShowDialog()

        'Retorna los datos del Cajero selecionado y 
        'se posiciona en Serie Recibo
        txtNaviera.Text = ""
        If frmBuscar.Codigo <> String.Empty Then
            txtNaviera.Text = frmBuscar.Codigo

            txtNaviera.Focus()
        End If

    End Sub
    '-----------------------------------Fin Busca Pais Naviera
#End Region

#Region "ValidaBusquedas"
    Private Sub txtProducto_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtProducto.Validating
        Try
            If txtProducto.Text.ToUpper <> "X" Then
                'Nombre Proveedor
                dTable = Datos.consulta_reader("SELECT nombre_pro FROM dbo.Producto WHERE id_pro=" & txtProducto.Text)
                txtNomProducto.Text = dTable.Rows.Item(0).Item("nombre_pro")
            Else
                txtNomProducto.Text = "TODOS LOS PRODUCTOS"
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtBuque_Validating(sender As Object, e As EventArgs) Handles txtBuque.Validating

        Try
            If txtBuque.Text.ToUpper <> "X" Then
                'Nombre Buque
                dTable = Datos.consulta_reader("SELECT  fechaviaje_buq fecha, viaje_buq, nombre_buq FROM dbo.Buques WHERE Id_buq=" & txtBuque.Text)
                txtNomBuque.Text = dTable.Rows.Item(0).Item("nombre_buq")
            Else
                txtNomBuque.Text = "TODOS LOS BUQUES"
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtNaviera_Validating(sender As Object, e As EventArgs) Handles txtNaviera.Validating
        Try
            If txtNaviera.Text.ToUpper <> "X" Then

                'Nombre Proveedor
                dTable = Datos.consulta_reader("SELECT nombre_cli FROM dbo.Cliente WHERE id_cli=" & txtNaviera.Text)
                txtNomNaviera.Text = dTable.Rows.Item(0).Item("nombre_cli")
            Else
                txtNomNaviera.Text = "TODAS LAS NAVIERAS"
            End If

        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "F2"
    'KeyDown, cuando presionamos F2 en el txtCodigo_Cajero
    'Funciona para que cuando presionamos dicha tecla nos muestre la ayuda.
    Private Sub txtIdEmple_KeyDown(sender As Object, e As KeyEventArgs) Handles txtProducto.KeyDown
        Select Case e.KeyCode
            Case Keys.F2
                BuscaProdu()
        End Select
    End Sub

    Private Sub txtBuque_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBuque.KeyDown
        Select Case e.KeyCode
            Case Keys.F2
                BuscaBuque()
        End Select
    End Sub

    Private Sub txtNaviera_KeyDown(sender As Object, e As KeyEventArgs) Handles txtNaviera.KeyDown
        Select Case e.KeyCode
            Case Keys.F2
                BuscaNaviera()
        End Select
    End Sub

#End Region


    'Private Sub ChekBuque_Click(sender As Object, e As EventArgs) Handles ChekBuque.Click
    '    ChekCamion.Checked = False
    '    ChekProducto.Checked = False
    '    ChekProductoResumen.Checked = False
    'End Sub

    'Private Sub ChekCamion_Click(sender As Object, e As EventArgs) Handles ChekCamion.Click
    '    ChekCamion.Checked = True
    '    ChekProducto.Checked = False
    '    ChekProductoResumen.Checked = False
    'End Sub

    'Private Sub ChekProducto_Click(sender As Object, e As EventArgs) Handles ChekProducto.Click
    '    ChekCamion.Checked = False
    '    ChekProductoResumen.Checked = False
    '    ChekBuque.Checked = False
    'End Sub

    'Private Sub ChekProductoResumen_Click(sender As Object, e As EventArgs) Handles ChekProductoResumen.Click
    '    ChekCamion.Checked = False
    '    ChekProducto.Checked = False
    '    ChekBuque.Checked = False
    'End Sub


End Class