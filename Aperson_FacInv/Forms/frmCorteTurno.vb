Imports CrystalDecisions.Shared
Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Drawing.Printing

Public Class frmCorteTurno

#Region "Variables"
    Dim Datos As New DatosGenereral()
    Dim dTable As New DataTable
    Dim Empresa As String = LibreriaGeneral.nEmpresa

    Dim Parametros As New ParameterFields()
    Dim paramFecha1 As New ParameterField()
    Dim paramFecha2 As New ParameterField()
    Dim paramHora1 As New ParameterField()
    Dim paramHora2 As New ParameterField()
    Dim paramUsuario As New ParameterField()

    Dim myDiscreteValue As New ParameterDiscreteValue()
    Dim myDiscreteValue2 As New ParameterDiscreteValue()
    Dim myDiscreteValue3 As New ParameterDiscreteValue()
    Dim myDiscreteValue4 As New ParameterDiscreteValue()
    Dim myDiscreteValue5 As New ParameterDiscreteValue()
    Dim myDiscreteValue6 As New ParameterDiscreteValue()

#End Region


    Private Sub frmCorteTurno_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dHoraIni.Text = "00:00"
        dHoraFin.Text = "23:59"

        'Arma el ComboBox
        Datos.cargar_combo(cmbUsuario,
                           "Select nombre_usu, Log_usu From [dbo].[Usuario] WHERE baja_usu = 'ACTIVO' Order by 1",
                           "Log_usu", "nombre_usu")

        chkTicket.Checked = True

    End Sub


    Private Sub btnGrabar_Click(sender As Object, e As EventArgs) Handles btnGrabar.Click
        Dim Fecha1 As DateTime = dFechaIni.Value
        Dim Fecha2 As DateTime = dFechaFin.Value

        If chkTicket.Checked = False Then

            ''Conecta
            'Dim TuReporte As New Rep1                   'Un Reporte que tengas como pivote
            'Dim ocInforme As New ReportDocument
            'Dim formulario As New Genera_ReporteCatalogos   'Pantalla como objeto CrystalReportPreview
            'Dim NomRepo As String = ""

            'NomRepo = LibreriaGeneral.cPathReport & "\RepAnalisa3.rpt"                    'Nombre de la ubicacion y Nombre de Reporte a Mostrar


            'If ChekProducto.Checked Then
            '    NomRepo = LibreriaGeneral.cPathReport & "\RepAnalisa1.rpt"
            'End If
            'If ChekProductoResumen.Checked Then
            '    NomRepo = LibreriaGeneral.cPathReport & "\RepAnalisa2.rpt"
            'End If
            'If ChekCamion.Checked Then
            '    NomRepo = LibreriaGeneral.cPathReport & "\RepAnalisa5.rpt"
            'End If
            'If ChekBuque.Checked Then
            '    NomRepo = LibreriaGeneral.cPathReport & "\RepAnalisa4.rpt"
            'End If

            'ocInforme.Load(NomRepo, OpenReportMethod.OpenReportByDefault)

            'Dim conexion As ConnectionInfo = New ConnectionInfo
            'conexion.ServerName = LibreriaGeneral.cServer
            'conexion.DatabaseName = LibreriaGeneral.cBaseD
            'conexion.UserID = LibreriaGeneral.cUsu
            'conexion.Password = LibreriaGeneral.cPass
            'conexion.Type = ConnectionInfoType.SQL

            'For Each tab As Table In ocInforme.Database.Tables
            '    Dim tablas As TableLogOnInfo = tab.LogOnInfo
            '    tablas.ConnectionInfo = conexion
            '    tab.ApplyLogOnInfo(tablas)
            'Next

            ''Tipos de Parametro
            'paramFecha1.ParameterValueType = ParameterValueKind.DateParameter
            'paramFecha2.ParameterValueType = ParameterValueKind.DateParameter
            'paramHora1.ParameterValueType = ParameterValueKind.StringParameter
            'paramHora2.ParameterValueType = ParameterValueKind.StringParameter

            'paramBuque.ParameterValueType = ParameterValueKind.StringParameter
            'paramViaje.ParameterValueType = ParameterValueKind.NumberParameter
            'paramBodega.ParameterValueType = ParameterValueKind.NumberParameter
            'paramProducto.ParameterValueType = ParameterValueKind.StringParameter
            'paramNaviera.ParameterValueType = ParameterValueKind.StringParameter

            ''Asignamos parametro de Store Procedure
            'paramFecha1.ParameterFieldName = "@FechaIni"
            'paramFecha2.ParameterFieldName = "@FechaFin"
            'paramHora1.ParameterFieldName = "@HoraIni"
            'paramHora2.ParameterFieldName = "@HoraFin"

            'paramBuque.ParameterFieldName = "@Buque"
            'paramViaje.ParameterFieldName = "@Viaje"
            'paramBodega.ParameterFieldName = "@Bodega"
            'paramProducto.ParameterFieldName = "@Produc"
            'paramNaviera.ParameterFieldName = "@Naviera"

            ''Damos Valor a Cada Parametro
            'myDiscreteValue.Value = dFechaIni.Value
            'myDiscreteValue2.Value = dFechaFin.Value
            'myDiscreteValue9.Value = dHoraIni.Text
            'myDiscreteValue10.Value = dHoraFin.Text

            'myDiscreteValue3.Value = txtBuque.Text
            'myDiscreteValue4.Value = Convert.ToInt32(txtViaje.Text)
            'myDiscreteValue5.Value = Convert.ToInt32(txtBodega.Text)
            'myDiscreteValue7.Value = txtProducto.Text
            'myDiscreteValue8.Value = txtNaviera.Text

            ''Agregarmos el Valor al Parametro de Store Procedure
            'paramFecha1.CurrentValues.Add(myDiscreteValue)
            'paramFecha2.CurrentValues.Add(myDiscreteValue2)
            'paramHora1.CurrentValues.Add(myDiscreteValue9)
            'paramHora2.CurrentValues.Add(myDiscreteValue10)
            'paramBuque.CurrentValues.Add(myDiscreteValue3)
            'paramViaje.CurrentValues.Add(myDiscreteValue4)
            'paramBodega.CurrentValues.Add(myDiscreteValue5)
            'paramProducto.CurrentValues.Add(myDiscreteValue7)
            'paramNaviera.CurrentValues.Add(myDiscreteValue8)

            ''Los Adderimos al Reporte
            'Parametros.Add(paramFecha1)
            'Parametros.Add(paramFecha2)
            'Parametros.Add(paramHora1)
            'Parametros.Add(paramHora2)
            'Parametros.Add(paramBuque)
            'Parametros.Add(paramViaje)
            'Parametros.Add(paramBodega)
            'Parametros.Add(paramProducto)
            'Parametros.Add(paramNaviera)

            'formulario.CrystalReportViewer1.ParameterFieldInfo = Parametros    'Si usus parametros, si no quitaselo
            'formulario.CrystalReportViewer1.ReportSource = ocInforme
            'formulario.CrystalReportViewer1.Dock = DockStyle.Fill


            'Dim frmReporte As New Form()
            'With frmReporte
            '    .Controls.Add(formulario.CrystalReportViewer1)
            '    .Text = "Informes de Basculas"
            '    .WindowState = FormWindowState.Maximized
            '    .ShowDialog()
            'End With
            'conectarme.Close()

            'ocInforme = Nothing
            'formulario = Nothing
            'frmReporte = Nothing
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
        Dim TipoOpereacion As String = vbNullString


        Dim Lineas As String = vbNullString
        Dim Lineasd As String = vbNullString
        Dim uLineas As String = vbNullString

        'Variablas de uso SQL
        Dim dTableI As New DataTable

        'Arma Cursor de SAP, Para impresion de Factura.
        Dim StringSql As String = <sqlExp>
                                    EXEC [sp_CierreTurno] '<%= dFechaIni.Value %>','<%= dFechaFin.Value %>','<%= cmbUsuario.SelectedValue %>','<%= dHoraIni.Text.Trim & ":00" %>','<%= dHoraFin.Text.Trim & ":59" %>'
                              </sqlExp>.Value

        'Para mostrar el Dato
        dTableI = Datos.consulta_reader(StringSql)

        If dTableI.Rows.Count > 0 Then

            Lineas = "______________________________"
            Lineasd = "=============================="


            Empresa = " *- P A H A M E -*  "
            Empresa2 = "      Comercializadora   "
            TipoOpereacion = "** CIERRE DE TURNO **"

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

            'Sub Cabecera

            yPos = yPos + 30 : e.Graphics.DrawString("FECHA CIERRE: " & dTableI.Rows(0).Item("Fecha").ToString.Substring(0, 10), prFont, Brushes.Black, xPos, yPos)
            yPos = yPos + 20 : e.Graphics.DrawString("OPERADOR: " & dTableI.Rows(0).Item("Usuario").ToString.Substring(0, 20), prFont, Brushes.Black, xPos, yPos)

            yPos = yPos + 30 : e.Graphics.DrawString("ENTRADA: " & dHoraIni.Text, prFont, Brushes.Black, xPos, yPos)
            yPos = yPos + 20 : e.Graphics.DrawString("SALIDA: " & dHoraFin.Text, prFont, Brushes.Black, xPos, yPos)

            yPos = yPos + 20 : e.Graphics.DrawString(Lineas, prFont, Brushes.Black, xPos, yPos)

            yPos = yPos + 20 : e.Graphics.DrawString("TOTAL TICKETS: " & TextoDerecha2(Format(Val(dTableI.Rows(0).Item("Tticket")), "######,##0")), prFont, Brushes.Black, xPos, yPos)


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



End Class