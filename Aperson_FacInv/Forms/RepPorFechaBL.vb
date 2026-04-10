Imports CrystalDecisions.Shared
Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports System.Drawing.Printing

Public Class RepPorFechaBL


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
    Dim paramConsigna As New ParameterField()
    Dim paramBL As New ParameterField()
    Dim paramOrden As New ParameterField()

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
    Dim myDiscreteValue11 As New ParameterDiscreteValue()
    Dim myDiscreteValue12 As New ParameterDiscreteValue()

#End Region


    Private Sub RepAnalitico_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtBuque.Text = "X"
        txtNomBuque.Text = "TODOS LOS BUSQUES"
        txtViaje.Text = "0"
        txtBodega.Text = "0"
        txtProducto.Text = "X"
        txtNomProducto.Text = "TODOS LOS PRODUCTOS"
        txtConsigna.Text = "X"
        txtNomConsigna.Text = "TODOS LOS CONSIGNATARIOS"
        txtBL.Text = "X"
        dHoraIni.Text = "00:00"
        dHoraFin.Text = "23:59"
    End Sub


    Private Sub btnGrabar_Click(sender As Object, e As EventArgs) Handles btnGrabar.Click
        Dim Fecha1 As DateTime = dFechaIni.Value
        Dim Fecha2 As DateTime = dFechaFin.Value


        'Conecta
        Dim TuReporte As New Rep1                   'Un Reporte que tengas como pivote
        Dim ocInforme As New ReportDocument
        Dim formulario As New Genera_ReporteCatalogos   'Pantalla como objeto CrystalReportPreview
        Dim NomRepo As String = ""
        Dim tipor As Integer = 1

        NomRepo = LibreriaGeneral.cPathReport & "\RepAnalisaBL1.rpt"                    'Detallado


        If ChekBuque.Checked Then
            NomRepo = LibreriaGeneral.cPathReport & "\RepAnalisaBL2.rpt"
            tipor = 2
        End If
        If ChekBl.Checked Then
            NomRepo = LibreriaGeneral.cPathReport & "\RepAnalisaBL3.rpt"
            tipor = 3
        End If
        If CheckConsigna.Checked Then
            NomRepo = LibreriaGeneral.cPathReport & "\RepAnalisaBL4.rpt"
            tipor = 4
        End If
        If ChekProducto.Checked Then
            NomRepo = LibreriaGeneral.cPathReport & "\RepAnalisaBL5.rpt"
            tipor = 5
        End If
        If ChekProductoResumen.Checked Then
            NomRepo = LibreriaGeneral.cPathReport & "\RepAnalisaBL6.rpt"
            tipor = 6
        End If
        If CheckBLInfo.Checked Then
            NomRepo = LibreriaGeneral.cPathReport & "\RepAnalisaBL7.rpt"
            tipor = 7
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
        paramViaje.ParameterValueType = ParameterValueKind.StringParameter
        paramBodega.ParameterValueType = ParameterValueKind.NumberParameter
        paramProducto.ParameterValueType = ParameterValueKind.StringParameter
        paramConsigna.ParameterValueType = ParameterValueKind.StringParameter
        paramBL.ParameterValueType = ParameterValueKind.StringParameter
        paramOrden.ParameterValueType = ParameterValueKind.NumberParameter


        'Asignamos parametro de Store Procedure
        paramFecha1.ParameterFieldName = "@FechaIni"
        paramFecha2.ParameterFieldName = "@FechaFin"
        paramHora1.ParameterFieldName = "@HoraIni"
        paramHora2.ParameterFieldName = "@HoraFin"

        paramBuque.ParameterFieldName = "@Buque"
        paramViaje.ParameterFieldName = "@Viaje"
        paramBodega.ParameterFieldName = "@Bodega"
        paramProducto.ParameterFieldName = "@Produc"
        paramConsigna.ParameterFieldName = "@Consigna"
        paramBL.ParameterFieldName = "@Nbl"
        paramOrden.ParameterFieldName = "@TipoR"


        'Damos Valor a Cada Parametro
        myDiscreteValue.Value = dFechaIni.Value
        myDiscreteValue2.Value = dFechaFin.Value
        myDiscreteValue9.Value = dHoraIni.Text.Trim & ":00"
        myDiscreteValue10.Value = dHoraFin.Text.Trim & ":59"

        myDiscreteValue3.Value = txtBuque.Text
        myDiscreteValue4.Value = txtViaje.Text
        myDiscreteValue5.Value = Convert.ToInt32(txtBodega.Text)
        myDiscreteValue7.Value = txtProducto.Text
        myDiscreteValue8.Value = txtConsigna.Text
        myDiscreteValue11.Value = txtBL.Text
        myDiscreteValue12.Value = tipor


        'Agregarmos el Valor al Parametro de Store Procedure
        paramFecha1.CurrentValues.Add(myDiscreteValue)
        paramFecha2.CurrentValues.Add(myDiscreteValue2)
        paramHora1.CurrentValues.Add(myDiscreteValue9)
        paramHora2.CurrentValues.Add(myDiscreteValue10)
        paramBuque.CurrentValues.Add(myDiscreteValue3)
        paramViaje.CurrentValues.Add(myDiscreteValue4)
        paramBodega.CurrentValues.Add(myDiscreteValue5)
        paramProducto.CurrentValues.Add(myDiscreteValue7)
        paramConsigna.CurrentValues.Add(myDiscreteValue8)
        paramBL.CurrentValues.Add(myDiscreteValue11)
        paramOrden.CurrentValues.Add(myDiscreteValue12)


        'Los Adderimos al Reporte
        Parametros.Add(paramFecha1)
        Parametros.Add(paramFecha2)
        Parametros.Add(paramHora1)
        Parametros.Add(paramHora2)
        Parametros.Add(paramBuque)
        Parametros.Add(paramViaje)
        Parametros.Add(paramBodega)
        Parametros.Add(paramProducto)
        Parametros.Add(paramConsigna)
        Parametros.Add(paramBL)
        Parametros.Add(paramOrden)

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
    Private Sub BuscaConsigna()
        Dim stringSQL As String

        stringSQL = "SELECT id_csg As Codigo, nombre_csg As Nombre FROM [dbo].[Consignatario]"

        'Script para Filtrar el Grid
        Dim stringSqlFiltro As String = "SELECT id_csg As Codigo, nombre_csg As Nombre FROM [dbo].[Consignatario] WHERE id_csg "

        Dim frmBuscar As New BuscaCodigo(stringSQL, {"Código", "Nombre", "50", "200"}, 2, stringSqlFiltro, "Lista Consignatario")
        frmBuscar.ShowDialog()

        'Retorna los datos del Cajero selecionado y 
        'se posiciona en Serie Recibo
        txtConsigna.Text = ""
        If frmBuscar.Codigo <> String.Empty Then
            txtConsigna.Text = frmBuscar.Codigo

            txtConsigna.Focus()
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

    Private Sub txtConsiga_Validating(sender As Object, e As EventArgs) Handles txtConsigna.Validating
        Try
            If txtConsigna.Text.ToUpper <> "X" Then

                'Nombre Proveedor
                dTable = Datos.consulta_reader("SELECT nombre_csg FROM dbo.Consignatario WHERE id_csg=" & txtConsigna.Text)
                txtNomConsigna.Text = dTable.Rows.Item(0).Item("nombre_csg")
            Else
                txtNomConsigna.Text = "TODOS LOS CONSIGNATARIOS"
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

    Private Sub txtConsigna_KeyDown(sender As Object, e As KeyEventArgs) Handles txtConsigna.KeyDown
        Select Case e.KeyCode
            Case Keys.F2
                BuscaConsigna()
        End Select
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBLInfo.CheckedChanged

    End Sub


#End Region

End Class