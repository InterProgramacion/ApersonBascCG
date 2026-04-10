Imports CrystalDecisions.Shared
Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine


Public Class ReportePorFecha

    Dim Parametros As New ParameterFields()
    Dim paramFecha1 As New ParameterField()
    Dim paramFecha2 As New ParameterField()

    Dim myDiscreteValue As New ParameterDiscreteValue()
    Dim myDiscreteValue2 As New ParameterDiscreteValue()

    Dim Datos As New DatosGenereral()
    Dim dTable As New DataTable

    Private Sub ReportePorFecha_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        ''Para mostrar el Dato
        ''Arma Cursor de SAP, Para impresion de Factura.
        'Dim _ConSql As String = <sqlExp>

        '                        	SELECT a.ticketboleta_pes, 
        '                                  a.placa_pes,
        '                                    	ISNULL(fc.nombre_tpt,'') As nombre_tpt,
        '                         ISNULL(fc.numero_fca,'') As numero_cfa,
        '                                  a.Idcla_pes,
        '                                  b.nombreclase_cla,
        '                                  ISNULL(a.prefijocont_pes,'') As prefijocont_pes,
        '                                  ISNULL(a.numidentcont_pes,'') As numidentcont_pes,
        '                                  ISNULL(a.Idcon_pes,'') As Idcon_pes,
        '                                  ISNULL(a.tamanocon_pes,'') As tamanocon_pes,
        '                                  a.dicecontener_pes,
        '                                  a.tipocarga_pes,
        '                                  a.Idpro_pes,
        '                                  g.nombre_pro,
        '                                  a.tipooperacion_pes,
        '                                  a.Idbuq_pes,
        '                                  c.nombre_buq,
        '                                  a.bodegabuq_pes,
        '                                  a.Idvbuq_pes,
        '                                  a.Idcli_pes,
        '                                        d.nombre_cli,
        '                                  a.Idpaisdest_pes,
        '                                     e.nombre_pai As pdestino,
        '                                  a.Idpaisorig_pes,
        '                                        f.nombre_pai As porigien,
        '                                  a.pesomanif_pes,
        '                                  a.pesotara_pes,
        '                                  a.pesotaracont_pes,
        '                                  a.pesobruto_pes,
        '                                  a.pesoarana_pes,
        '                                  ISNULL(a.Idbas_entrada_pes,'') As Idbas_entrada_pes,
        '                                  ISNULL(a.fechahora_entrada_pes,'') As fechahora_entrada_pes,
        '                                  ISNULL(a.idusu_entrada_pes,'') As idusu_entrada_pes,
        '                                  ISNULL(a.Idbas_salida_pes,'') As Idbas_salida_pes,
        '                                  ISNULL(a.fechahora_salida_pes,'') As fechahora_salida_pes,
        '                                  ISNULL(a.idusu_salida_pes,'') As idusu_salida_pes,
        '                                  ISNULL(a.EstePesaje_pes,'') As EstePesaje_pes
        '                                FROM pesajes a
        '                                    LEFT JOIN clasecarga b ON b.Id_cla = a.Idcla_pes
        '                                    LEFT JOIN buques c ON c.Id_buq = a.Idbuq_pes 
        '                                    LEFT JOIN Cliente d ON d.Id_cli = a.Idcli_pes
        '                                    LEFT JOIN paisdestorig e ON e.Id_pai = a.Idpaisdest_pes
        '                                    LEFT JOIN paisdestorig f ON f.id_pai = a.Idpaisorig_pes
        '                                    LEFT JOIN Producto g ON g.Id_pro = a.Idpro_pes
        '                                    LEFT JOIN (select c1.*,t1.nombre_tpt from fcamiones c1
        '                       LEFT JOIN Transporte t1 ON t1.Id_tpt = c1.transporte_fca) fc ON fc.placa_fca = a.placa_pes
        '							    WHERE convert(date, fechahora_salida_pes,103) BETWEEN '<%= Format(DateTimePicker1.Value, "yyyyMMdd") %>' AND '<%= Format(DateTimePicker2.Value, "yyyyMMdd") %>';
        '                       </sqlExp>.Value


        Dim TuReporte As New Rep1
        Dim ocInforme As New ReportDocument
        Dim NomRepo As String = LibreriaGeneral.cPathReport & "\Rep1.rpt"
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

        paramFecha1.ParameterValueType = ParameterValueKind.DateParameter
        paramFecha2.ParameterValueType = ParameterValueKind.DateParameter

        paramFecha1.ParameterFieldName = "@FechaIni"
        paramFecha2.ParameterFieldName = "@FechaFin"

        myDiscreteValue.Value = DateTimePicker1.Value
        myDiscreteValue2.Value = DateTimePicker2.Value

        paramFecha1.CurrentValues.Add(myDiscreteValue)
        paramFecha2.CurrentValues.Add(myDiscreteValue2)

        Parametros.Add(paramFecha1)
        Parametros.Add(paramFecha2)

        CrystalReportViewer1.ParameterFieldInfo = Parametros
        CrystalReportViewer1.ReportSource = ocInforme

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub
End Class