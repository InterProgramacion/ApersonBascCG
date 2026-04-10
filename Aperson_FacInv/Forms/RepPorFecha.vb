Imports CrystalDecisions.Shared
Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine

Public Class RepPorFecha

#Region "Variables"
    Dim Datos As New DatosGenereral()
    Dim dTable As New DataTable
    Dim Empresa As String = LibreriaGeneral.nEmpresa

    Dim Parametros As New ParameterFields()
    Dim paramFecha1 As New ParameterField()
    Dim paramFecha2 As New ParameterField()
    Dim paramBuque As New ParameterField()
    Dim paramViaje As New ParameterField()
    Dim paramBodega As New ParameterField()
    Dim paramTipoO As New ParameterField()
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

#End Region

    Private Sub RepPorFecha_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtBuque.Text = "X"
        txtNomBuque.Text = "TODOS LOS BUSQUES"
        txtViaje.Text = "0"
        txtBodega.Text = "0"
        cmbTipoOperacion.Text = "TODOS"
        txtProducto.Text = "X"
        txtNomProducto.Text = "TODOS LOS PRODUCTOS"
        txtNaviera.Text = "X"
        txtNomNaviera.Text = "TODAS LAS EMPRESAS"
    End Sub


    Private Sub btnGrabar_Click(sender As Object, e As EventArgs) Handles btnGrabar.Click
        Dim Fecha1 As DateTime = dFechaIni.Value
        Dim Fecha2 As DateTime = dFechaFin.Value

        'Datos.Reporte_PorFecha(Fecha1, Fecha2)

        'Conecta
        'Call conexiones()
        Dim TuReporte As New Rep1                   'Un Reporte que tengas como pivote
        Dim ocInforme As New ReportDocument
        'ocInforme = REPORTEDOC

        'Dim rpt As New _NoRep   'Reporte Pilotos
        Dim formulario As New Genera_ReporteCatalogos   'Pantalla como objeto CrystalReportPreview
        Dim NomRepo As String = ""

        NomRepo = LibreriaGeneral.cPathReport & "\RepFecha1.rpt"                    'Nombre de la ubicacion y Nombre de Reporte a Mostrar

        If ChekBuque.Checked Then
            NomRepo = LibreriaGeneral.cPathReport & "\RepFecha2.rpt"
        End If

        If ChekBodega.Checked Then
            NomRepo = LibreriaGeneral.cPathReport & "\RepFecha4.rpt"
        End If

        If ChekNaviera.Checked Then
            NomRepo = LibreriaGeneral.cPathReport & "\RepFecha7.rpt"
        End If

        If ChekProducto.Checked Then
            NomRepo = LibreriaGeneral.cPathReport & "\RepFecha6.rpt"
        End If
        If ChekTipoO.Checked Then
            NomRepo = LibreriaGeneral.cPathReport & "\RepFecha5.rpt"
        End If
        If ChekViaje.Checked Then
            NomRepo = LibreriaGeneral.cPathReport & "\RepFecha3.rpt"
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

        paramBuque.ParameterValueType = ParameterValueKind.StringParameter
        paramViaje.ParameterValueType = ParameterValueKind.NumberParameter
        paramBodega.ParameterValueType = ParameterValueKind.NumberParameter
        paramTipoO.ParameterValueType = ParameterValueKind.StringParameter
        paramProducto.ParameterValueType = ParameterValueKind.StringParameter
        paramNaviera.ParameterValueType = ParameterValueKind.StringParameter

        'Asignamos parametro de Store Procedure
        paramFecha1.ParameterFieldName = "@FechaIni"
        paramFecha2.ParameterFieldName = "@FechaFin"
        paramBuque.ParameterFieldName = "@Buque"
        paramViaje.ParameterFieldName = "@Viaje"
        paramBodega.ParameterFieldName = "@Bodega"
        paramTipoO.ParameterFieldName = "@TipoO"
        paramProducto.ParameterFieldName = "@Produc"
        paramNaviera.ParameterFieldName = "@Naviera"

        'Damos Valor a Cada Parametro
        myDiscreteValue.Value = dFechaIni.Value
        myDiscreteValue2.Value = dFechaFin.Value
        myDiscreteValue3.Value = txtBuque.Text
        myDiscreteValue4.Value = txtViaje.Text
        myDiscreteValue5.Value = Convert.ToInt32(txtBodega.Text)
        myDiscreteValue6.Value = cmbTipoOperacion.Text
        myDiscreteValue7.Value = txtProducto.Text
        myDiscreteValue8.Value = txtNaviera.Text

        'Agregarmos el Valor al Parametro de Store Procedure
        paramFecha1.CurrentValues.Add(myDiscreteValue)
        paramFecha2.CurrentValues.Add(myDiscreteValue2)
        paramBuque.CurrentValues.Add(myDiscreteValue3)
        paramViaje.CurrentValues.Add(myDiscreteValue4)
        paramBodega.CurrentValues.Add(myDiscreteValue5)
        paramTipoO.CurrentValues.Add(myDiscreteValue6)
        paramProducto.CurrentValues.Add(myDiscreteValue7)
        paramNaviera.CurrentValues.Add(myDiscreteValue8)

        'Los Adderimos al Reporte
        Parametros.Add(paramFecha1)
        Parametros.Add(paramFecha2)
        Parametros.Add(paramBuque)
        Parametros.Add(paramViaje)
        Parametros.Add(paramBodega)
        Parametros.Add(paramTipoO)
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

        GC.Collect()
        GC.WaitForPendingFinalizers()
        GC.Collect()

    End Sub


    Private Sub btnSalir_Click(sender As Object, e As EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub



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


    Private Sub ChekViaje_Click(sender As Object, e As EventArgs) Handles ChekViaje.Click

        'ChekBodega.Checked = False
        ChekBuque.Checked = False
        ChekNaviera.Checked = False
        ChekProducto.Checked = False
        ChekTipoO.Checked = False
        ChekViaje.Checked = True

    End Sub

    Private Sub ChekBuque_Click(sender As Object, e As EventArgs) Handles ChekBuque.Click
        'ChekBodega.Checked = False
        ChekBuque.Checked = True
        ChekNaviera.Checked = False
        ChekProducto.Checked = False
        ChekTipoO.Checked = False
        ChekViaje.Checked = False
    End Sub

    Private Sub ChekBodega_Click(sender As Object, e As EventArgs)
        'ChekBodega.Checked = True
        ChekBuque.Checked = False
        ChekNaviera.Checked = False
        ChekProducto.Checked = False
        ChekTipoO.Checked = False
        ChekViaje.Checked = False
    End Sub

    Private Sub ChekTipoO_Click(sender As Object, e As EventArgs) Handles ChekTipoO.Click
        ' ChekBodega.Checked = False
        ChekBuque.Checked = False
        ChekNaviera.Checked = False
        ChekProducto.Checked = False
        ChekTipoO.Checked = True
        ChekViaje.Checked = False
    End Sub

    Private Sub ChekProducto_Click(sender As Object, e As EventArgs) Handles ChekProducto.Click
        ' ChekBodega.Checked = False
        ChekBuque.Checked = False
        ChekNaviera.Checked = False
        ChekProducto.Checked = True
        ChekTipoO.Checked = False
        ChekViaje.Checked = False
    End Sub

    Private Sub ChekNaviera_Click(sender As Object, e As EventArgs) Handles ChekNaviera.Click
        ' ChekBodega.Checked = False
        ChekBuque.Checked = False
        ChekNaviera.Checked = True
        ChekProducto.Checked = False
        ChekTipoO.Checked = False
        ChekViaje.Checked = False
    End Sub


End Class