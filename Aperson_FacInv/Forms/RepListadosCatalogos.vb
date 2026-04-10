Imports CrystalDecisions.Shared
Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine

Public Class RepListadosCatalogos
    Dim Datos As New DatosGenereral()
    Dim dTable As New DataTable

    Private Sub Button8_Click(sender As Object, e As EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

#Region "Muestra Reporte"
    Private Sub _VistaReporte(ByVal REPORTEDOC As ReportDocument, _ConSql As String, _Vista As String, _Titulo As String)
        'Conecta
        'Call conexiones()
        Dim TuReporte As New Rep1
        Dim ocInforme As New ReportDocument
        ocInforme = REPORTEDOC

        'Dim rpt As New _NoRep   'Reporte Pilotos
        Dim formulario As New Genera_ReporteCatalogos

        'dat.Clear()
        'stringSQLData = _ConSql
        'adapter = consulta_non_queryDeta(stringSQLData) 'New Odbc.OdbcDataAdapter(stringSQLData, conectarme)
        ''adapter.Fill(dat, _Vista)

        'Dim myConnectionInfo As CrystalDecisions.Shared.ConnectionInfo = New CrystalDecisions.Shared.ConnectionInfo()
        'myConnectionInfo.DatabaseName = "DbAperBascu"
        'myConnectionInfo.UserID = "sa"
        'myConnectionInfo.Password = "@Sha123"

        'rpt.SetDataSource(dat)

        Dim NomRepo As String = LibreriaGeneral.cPathReport & "\Rep_ListadoProductos.rpt"
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

        formulario.CrystalReportViewer1.ReportSource = ocInforme
        formulario.CrystalReportViewer1.Dock = DockStyle.Fill


        Dim frmReporte As New Form()
        With frmReporte
            .Controls.Add(formulario.CrystalReportViewer1)
            .Text = _Titulo
            .WindowState = FormWindowState.Maximized
            .ShowDialog()
        End With
        conectarme.Close()

        ocInforme = Nothing
        formulario = Nothing
        frmReporte = Nothing

    End Sub

#End Region


    'Private Sub btnClientes_Click(sender As Object, e As EventArgs) Handles btnClientes.Click

    '    _VistaReporte(New Rep_ListaClientes, "Select * from [dbo].[Cliente]", "Cliente", "Litado Clientes")


    '    GC.Collect()
    '    GC.WaitForPendingFinalizers()
    '    GC.Collect()
    'End Sub

    Private Sub btnProductos_Click(sender As Object, e As EventArgs) Handles btnProductos.Click

        _VistaReporte(New Rep_ListadoProductos, "Select * from [dbo].[Producto]", "Productos", "Litado Productos")


        GC.Collect()
        GC.WaitForPendingFinalizers()
        GC.Collect()
    End Sub

    'Private Sub btnCamiones_Click(sender As Object, e As EventArgs) Handles btnCamiones.Click

    '    _VistaReporte(New Rep_ListaCamiones, "Select * from [dbo].[fCamiones]", "fCamiones", "Litado Flortilla Camiones")


    '    GC.Collect()
    '    GC.WaitForPendingFinalizers()
    '    GC.Collect()
    'End Sub


    'Private Sub btnTransporte_Click(sender As Object, e As EventArgs) Handles btnTransporte.Click

    '    _VistaReporte(New Rep_ListaTransporte, "Select * from [dbo].[Transporte]", "Transporte", "Litado Transporte")


    '    GC.Collect()
    '    GC.WaitForPendingFinalizers()
    '    GC.Collect()
    'End Sub

    'Private Sub btrnBasculas_Click(sender As Object, e As EventArgs) Handles btrnBasculas.Click

    '    _VistaReporte(New Rep_ListaBasculas, "Select * from [dbo].[Basculas]", "Basculas", "Litado Basculas")


    '    GC.Collect()
    '    GC.WaitForPendingFinalizers()
    '    GC.Collect()
    'End Sub

    'Private Sub btnUsuarios_Click(sender As Object, e As EventArgs) Handles btnUsuarios.Click

    '    _VistaReporte(New Rep_ListaUsuarios, "Select * from [dbo].[Usuario]", "Usuario", "Litado Usuarios")


    '    GC.Collect()
    '    GC.WaitForPendingFinalizers()
    '    GC.Collect()
    'End Sub

    Private Sub btnBuques_Click(sender As Object, e As EventArgs) Handles btnBuques.Click

    End Sub
End Class