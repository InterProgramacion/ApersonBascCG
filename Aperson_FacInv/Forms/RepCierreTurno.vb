Imports CrystalDecisions.Shared
Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine

Public Class RepCierreTurno

#Region "Variables"
    Dim Datos As New DatosGenereral()
    Dim dTable As New DataTable
    Dim Empresa As String = LibreriaGeneral.nEmpresa

    Dim Parametros As New ParameterFields()
    Dim paramFecha1 As New ParameterField()
    Dim paramFecha2 As New ParameterField()
    Dim paramUsuario As New ParameterField()


    Dim myDiscreteValue As New ParameterDiscreteValue()
    Dim myDiscreteValue2 As New ParameterDiscreteValue()
    Dim myDiscreteValue3 As New ParameterDiscreteValue()

#End Region


    Private Sub RepCierreTurno_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Arma el ComboBox
        Datos.cargar_combo(cmbUsuario,
                           "Select nombre_usu, Log_usu From [dbo].[Usuario] WHERE baja_usu = 'ACTIVO' Order by 1",
                           "Log_usu", "nombre_usu")

        ChekDetallado.CheckState = CheckState.Checked

    End Sub


    Private Sub btnGrabar_Click(sender As Object, e As EventArgs) Handles btnGrabar.Click
        Dim Fecha1 As DateTime = dFechaIni.Value
        Dim Fecha2 As DateTime = dFechaFin.Value
        Dim usuari As String = cmbUsuario.SelectedItem("Log_usu").ToString.Trim

        'Datos.Reporte_PorFecha(Fecha1, Fecha2)

        'Conecta
        'Call conexiones()
        Dim TuReporte As New Rep1                   'Un Reporte que tengas como pivote
        Dim ocInforme As New ReportDocument
        'ocInforme = REPORTEDOC

        'Dim rpt As New _NoRep   'Reporte Pilotos
        Dim formulario As New Genera_ReporteCatalogos   'Pantalla como objeto CrystalReportPreview
        Dim NomRepo As String = ""

        NomRepo = LibreriaGeneral.cPathReport & "\CierreTurno.rpt"                    'Nombre de la ubicacion y Nombre de Reporte a Mostrar

        If ChekDetallado.Checked Then
            NomRepo = LibreriaGeneral.cPathReport & "\CierreTurno.rpt"
        End If

        If ChekResumido.Checked Then
            NomRepo = LibreriaGeneral.cPathReport & "\CierreTurno2.rpt"
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

        'SetDBLogonForReport(myConnectionInfo)

        'rptv.DataBind()

        'Tipos de Parametro
        paramFecha1.ParameterValueType = ParameterValueKind.DateParameter
        paramFecha2.ParameterValueType = ParameterValueKind.DateParameter
        paramUsuario.ParameterValueType = ParameterValueKind.StringParameter


        'Asignamos parametro de Store Procedure
        paramFecha1.ParameterFieldName = "@FechaIni"
        paramFecha2.ParameterFieldName = "@FechaFin"
        paramUsuario.ParameterFieldName = "@cUsuario"


        'Damos Valor a Cada Parametro
        myDiscreteValue.Value = dFechaIni.Value
        myDiscreteValue2.Value = dFechaFin.Value
        myDiscreteValue3.Value = usuari.Trim


        'Agregarmos el Valor al Parametro de Store Procedure
        paramFecha1.CurrentValues.Add(myDiscreteValue)
        paramFecha2.CurrentValues.Add(myDiscreteValue2)
        paramUsuario.CurrentValues.Add(myDiscreteValue3)

        'Los Adderimos al Reporte
        Parametros.Add(paramFecha1)
        Parametros.Add(paramFecha2)
        Parametros.Add(paramUsuario)

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

        GC.Collect()
        GC.WaitForPendingFinalizers()
        GC.Collect()

    End Sub


    Private Sub btnSalir_Click(sender As Object, e As EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub


    Private Sub ChekDetallado_Click(sender As Object, e As EventArgs) Handles ChekDetallado.Click
        ChekDetallado.Checked = True
        ChekResumido.Checked = False
    End Sub

    Private Sub ChekResumido_Click(sender As Object, e As EventArgs) Handles ChekResumido.Click
        ChekDetallado.Checked = False
        ChekResumido.Checked = True
    End Sub

End Class