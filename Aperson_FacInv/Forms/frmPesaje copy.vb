Imports TomaPesoLib
Imports System.IO.Ports
Imports System.Text.RegularExpressions
Imports System.Globalization


Public Class frmPesaje

#Region "Variables"
    Dim Datos As New DatosGenereral()
    Dim NuevoReg As Boolean
    Dim dTable As New DataTable
    Dim dTable2 As New DataTable

    Dim PesoManifestado As Double = 0.0
    Dim PesoTara As Double = 0.0
    Dim PesoTaraCont As Double = 0.0
    Dim PesoBruto As Double = 0.0
    Dim PesoArana As Double = 0.0
    Dim PesoNeto As Double = 0.0
    Dim PesoVGM As Double = 0.0
    Dim EstaPesando As String = ""
    Dim EsContene As Boolean = False
    Dim Hbl As Integer = 0
    Dim EsDesdeTemporal As Boolean = False
    Dim IdPesTemporal As Integer = 0
    Dim IdBlBuque As Integer = 0
    Dim IdConsignatario As Integer = 0
    Dim IdDetSolicitudDesp As Integer = 0
    Dim IdDetalleBuque As Integer = 0


    Private WithEvents miBascula As Bascula  'Se instancia la DLL C#
#End Region

#Region "Entorno"

    'Private Sub frmPesaje_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
    '    Select Case e.KeyCode

    '        Case Keys.F3

    '            btnNuevo_Click(sender, e)

    '        Case Keys.F4

    '            btnGrabar_Click(sender, e)

    '    End Select
    'End Sub

    Private Sub frmPesaje_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Saul
        'LibreriaGeneral.nBascu = "01"

        If Convert.ToInt32(LibreriaGeneral.nBascu) = 0 Then
            MessageBox.Show("La BASCULA no tiene Computadora Asiganda" & vbCrLf & "Comunicarse con Administrador")
            Me.Close()
            End
        Else
            ' Inicializar la báscula con el puerto COM adecuado
            miBascula = New Bascula(LibreriaGeneral.gPuertoCOM, LibreriaGeneral.gBitsPorSegundo) ' Asegúrate de poner el puerto correcto
            AddHandler miBascula.DatosRecibidos, AddressOf ActualizarPeso
            miBascula.Iniciar()
            txtTomaPeso.ReadOnly = False ''modificado por ML 26112025 1990

            'Para Calibraciones ------------------------------------------------------------------
            Dim FechaProx As Date = Now
            Dim Aviso As Integer = 0
            Dim FechaHoy As Date = Now 'Fecha de Hoy
            Dim dDiasDif As Long

            'Para mostrar el Dato
            dTable = Datos.consulta_reader("Select FechaProxima_reg, AvisarDias_reg FROM registcalib WHERE bascula_reg ='" & LibreriaGeneral.nBascu & "'")

            If dTable.Rows.Count > 0 Then
                'Setea Campos
                For Each dRow As DataRow In dTable.Rows
                    'Infomrativos
                    FechaProx = dRow.Item("FechaProxima_reg").ToString
                    Aviso = dRow.Item("AvisarDias_reg").ToString
                Next
            End If

            If Aviso > 0 Then
                dDiasDif = DateDiff(DateInterval.DayOfYear, FechaHoy, FechaProx)

                If (Aviso >= dDiasDif) Then
                    Dim frmS As New Form
                    frmS = MensajesBascula
                    LibreriaGeneral.xMensaje = "La proxima Calibración es en: " & dDiasDif & " Días"
                    frmS.ShowDialog()
                End If
            End If

            'Para Certificacioones ------------------------------------------------------------------
            FechaProx = Now
            Aviso = 0
            FechaHoy = Now 'Fecha de Hoy

            'Para mostrar el Dato
            dTable = Datos.consulta_reader("Select FechaProxima_reg, AvisarDias_reg FROM RegistCertifica WHERE bascula_reg ='" & LibreriaGeneral.nBascu & "'")

            If dTable.Rows.Count > 0 Then
                'Setea Campos
                For Each dRow As DataRow In dTable.Rows
                    'Infomrativos
                    FechaProx = dRow.Item("FechaProxima_reg").ToString
                    Aviso = dRow.Item("AvisarDias_reg").ToString
                Next
            End If

            If Aviso > 0 Then
                dDiasDif = DateDiff(DateInterval.DayOfYear, FechaHoy, FechaProx)

                If (Aviso >= dDiasDif) Then
                    Dim frmS As New Form
                    frmS = MensajesBascula
                    LibreriaGeneral.xMensaje = "La proxima Certificacion es en: " & dDiasDif & " Días"
                    frmS.ShowDialog()
                End If
            End If
            '----------------------------------------avisos fechas para certificados y calibracion

            'Arma el ComboBoxT
            Datos.cargar_combo(cmbClaseCarga,
                               "Select NombreClase_cla, id_cla From [dbo].[clasecarga] Order by 1",
                               "id_cla", "NombreClase_cla")



            Textos()            'Limpia Textos
            LeerData(grdData)   'Llena Datos Pendientes de Salir
            LeerTemporales(dataSol)
            SumaTicket()        'Suma Correla
        End If

    End Sub

    'Muestra Ticket
    Private Sub SumaTicket()
        'Para mostrar el Dato
        dTable = Datos.consulta_reader("SELECT CAST(nticketboleta_cor + 1 AS VARCHAR(10)) AS 'Corre' FROM correlactu")

        If dTable.Rows.Count > 0 Then
            'Setea Campos
            For Each dRow As DataRow In dTable.Rows
                'Infomrativos
                txtTicket.Text = dRow.Item("Corre").ToString
            Next
        End If

        txtTicket.Refresh()

    End Sub

    Private Sub Textos()
        'Cabecera
        txtPlaca.Text = ""
        txtNomTransporte.Text = ""
        txtNomTransporte.ReadOnly = True
        txtNomTransporte.ForeColor = Color.Black         ' Color del texto
        txtNomTransporte.BackColor = Color.LightGray     ' (Opcional) Cambiar el color de fondo para simular un estado deshabilitado

        cmbClaseCarga.Text = ""

        'Contenedor
        txtPrefijo.Text = ""
        txtContenedor.Text = ""
        txtMedida.Text = ""
        txtTipoCarga.Text = ""
        txtDiceContenedor.Text = ""
        EsContene = False
        NuevoReg = False

        'Nuevo contenedor de traslado
        txtTras.Text = "N"
        cbTraslado.Text = "NORMAL"

        'Solictados
        txtProducto.Text = ""
        txtNomProducto.Text = ""
        txtTipoCarga.Text = ""
        txtBuque.Text = ""
        txtBuque.Enabled = True
        txtNomBuque.Text = ""
        txtViaje.Text = ""
        txtNaviera.Text = ""
        txtNomNaviera.Text = ""
        txtTicket.Text = ""
        txtBodega.Text = ""
        txtBodega.Enabled = True
        txtNomBodega.Text = ""

        'Infomrativos
        txtBasculaEntrada.Text = ""
        txtBasculaSalida.Text = ""
        dFechaEntrada.Format = DateTimePickerFormat.Short
        dFechaSalida.Format = DateTimePickerFormat.Short
        hEntrada.Format = DateTimePickerFormat.Time
        hSalida.Format = DateTimePickerFormat.Time
        txtOperadorEntrada.Text = ""
        txtOperadorSalida.Text = ""
        dFechav.Format = DateTimePickerFormat.Short

        'Pesaje
        txtPManifestado.Text = "0"
        txtPTaraVehiculo.Text = "0"
        txtPTaraContenedor.Text = "0"
        txtPesoBruto.Text = "0"
        txtPesoAraña.Text = "0"
        txtPesoNeto.Text = "0"
        TxtPesoVGM.Text = "0"
        TxtQuintales.Text = "0"
        txtTomaPeso.Text = ""

        'BL
        txtBL.Text = ""
        txtDuca.Text = ""
        txtNomConsigna.Text = ""
        txtBL_Manifiesto.Text = "0"
        txtBL_PesoBascula.Text = "0"
        txtBL_Diferencia.Text = "0"

        IdBlBuque = 0   '<<< importante: limpiar el id cuando se limpia la pantalla
        IdConsignatario = 0   '<<< limpiar también
        txtAPROBADO.Text = "0"
        IdDetSolicitudDesp = 0

        txtBL.Enabled = False
        txtBL_Diferencia.BackColor = SystemColors.Control

        'Pesaje
        txtPManifestado.Enabled = True
        txtPTaraContenedor.Enabled = True
        txtPesoAraña.Enabled = True

        txtPTaraVehiculo.Enabled = False
        txtPesoBruto.Enabled = False
        txtPesoNeto.Enabled = False
        TxtQuintales.Enabled = False

        btnGrabar.Enabled = False
        PicCamion.Visible = False

        'Se posiciona para Iniciar 
        txtPlaca.Focus()
    End Sub

    Private Sub btnSalir_Click(sender As Object, e As EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    '' Este evento se dispara cuando el botón recibe el foco
    'Private Sub btnNuevo_Enter(sender As Object, e As EventArgs) Handles btnNuevo.Enter
    '    ' Cambiar el color de fondo del botón al recibir el foco
    '    If txtTomaPeso.Text IsNot String.Empty Then
    '        btnNuevo.BackColor = Color.Green
    '    End If
    'End Sub

    '' Este evento se dispara cuando el botón pierde el foco
    'Private Sub btnNuevo_Leave(sender As Object, e As EventArgs) Handles btnNuevo.Leave
    '    ' Restaurar el color de fondo original del botón
    '    btnNuevo.BackColor = Color.DarkGray
    'End Sub
#End Region

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

    '--------------------------Despliega pantalla para buscar el Navoiera
    Private Sub BuscaBodega()
        Dim stringSQL As String

        stringSQL = "SELECT id_bod As Codigo, nombre_bod As Nombre FROM [dbo].[Bodega]"

        'Script para Filtrar el Grid
        Dim stringSqlFiltro As String = "SELECT id_bod As Codigo, nombre_bod As Nombre FROM [dbo].[Bodega] WHERE id_bod "

        Dim frmBuscar As New BuscaCodigo(stringSQL, {"Código", "Nombre", "50", "200"}, 2, stringSqlFiltro, "Lista Bodegas")
        frmBuscar.ShowDialog()

        'Retorna los datos del Cajero selecionado y 
        'se posiciona en Serie Recibo
        txtBodega.Text = ""
        If frmBuscar.Codigo <> String.Empty Then
            txtBodega.Text = frmBuscar.Codigo

            txtBodega.Focus()
        End If

    End Sub
    '-----------------------------------Fin Busca Bodega


    '--------------------------Despliega pantalla para buscar el Navoiera
    Private Sub buscaTransporte()
        Dim stringSQL As String

        stringSQL = "SELECT placa_fca As Codigo, model_fca As Nombre, color_fca  FROM [dbo].[fcamiones]"

        'Script para Filtrar el Grid
        Dim stringSqlFiltro As String = "SELECT placa_fca As Codigo, model_fca As Nombre FROM [dbo].[fcamiones] WHERE placa_fca "

        Dim frmBuscar As New BuscaCodigo(stringSQL, {"Placa", "Modelo", "Color", "150", "175", "200"}, 3, stringSqlFiltro, "Lista Camiones")
        frmBuscar.ShowDialog()

        'Retorna los datos del Cajero selecionado y 
        'se posiciona en Serie Recibo
        txtPlaca.Text = ""
        If frmBuscar.Codigo <> String.Empty Then
            txtPlaca.Text = frmBuscar.Codigo

            txtPlaca.Focus()
        End If

    End Sub
    '-----------------------------------Fin Busca Pais Naviera

#End Region

#Region "ValidaBusquedas"
    Private Sub txtProducto_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtProducto.Validating
        Try
            'Nombre Proveedor
            dTable = Datos.consulta_reader("SELECT nombre_pro FROM dbo.Producto WHERE id_pro='" & txtProducto.Text & "'")
            txtNomProducto.Text = dTable.Rows.Item(0).Item("nombre_pro")

            'If txtNomProducto.Text = String.Empty Then
            '    MessageBox.Show("ERROR: Producto no puede ser Blanco!!")
            '    txtProducto.Focus()
            'End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtBuque_Validating(sender As Object, e As EventArgs) Handles txtBuque.Validating

        Try
            dTable = Datos.consulta_reader("SELECT  fechaviaje_buq fecha, viaje_buq, nombre_buq FROM dbo.Buques WHERE Id_buq='" & txtBuque.Text & "'")
            txtNomBuque.Text = dTable.Rows.Item(0).Item("nombre_buq")
            txtViaje.Text = dTable.Rows.Item(0).Item("viaje_buq")
            dFechav.Value = dTable.Rows.Item(0).Item("fecha")

            'Habilita BL
            habilita_BL()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtNaviera_Validating(sender As Object, e As EventArgs) Handles txtNaviera.Validating
        Try
            'Nombre Proveedor
            dTable = Datos.consulta_reader("SELECT nombre_cli FROM dbo.Cliente WHERE id_cli='" & txtNaviera.Text & "'")
            txtNomNaviera.Text = dTable.Rows.Item(0).Item("nombre_cli")

            'Se posiciona para Tara Contenedor
            If EsContene = True Then           'No Es contenedor Then
                txtPManifestado.Focus()
            Else
                btnNuevo.Focus()
            End If

            ''Prueba1 = ENTRADA
            'txtTomaPeso.Text = "20000.00"
            PicCamion.Visible = True
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtBodega_Validating(sender As Object, e As EventArgs) Handles txtBodega.Validating
        Try
            'Nombre Proveedor
            dTable = Datos.consulta_reader("SELECT nombre_bod FROM dbo.Bodega WHERE id_bod='" & txtBodega.Text & "'")
            txtNomBodega.Text = dTable.Rows.Item(0).Item("nombre_bod")
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtClase_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtClase.Validating
        Try
            'If EstaPesando = "ENTRADA" Then
            'Clase Carga Abrevitura
            txtClase.Text = txtClase.Text.ToUpper
            Dim x As String = "SELECT nombreclase_cla, prefijocont_cla FROM dbo.clasecarga WHERE Abrevia_cla='" & txtClase.Text.Trim & "'"
            dTable = Datos.consulta_reader(x)

            EsContene = dTable.Rows.Item(0).Item("prefijocont_cla").ToString
            cmbClaseCarga.Text = dTable.Rows.Item(0).Item("nombreclase_cla").ToString

            'End If

            If EsContene = False Then           'GRANEL

                txtPrefijo.Text = ""
                txtContenedor.Text = ""
                txtMedida.Text = ""
                txtTipoCarga.Text = ""
                txtDiceContenedor.Text = ""

                txtBodega.Enabled = True
                txtBuque.Enabled = True

                txtTipo.Focus()
            Else

                'Nombre Bodega sin Dato
                dTable = Datos.consulta_reader("SELECT id_bod FROM dbo.bodega WHERE RTRIM(LTRIM(nombre_bod))='SIN DATO'")
                txtBodega.Text = dTable.Rows.Item(0).Item("id_bod")

                dTable = Datos.consulta_reader("SELECT nombre_bod FROM dbo.bodega WHERE id_bod='" & txtBodega.Text & "'")
                txtNomBodega.Text = dTable.Rows.Item(0).Item("nombre_bod")
                txtBodega.Enabled = False

                'Nombre Buque Patio
                dTable = Datos.consulta_reader("SELECT id_buq FROM dbo.buques WHERE RTRIM(LTRIM(nombre_buq))='PATIO RECEPCION'")
                txtBuque.Text = dTable.Rows.Item(0).Item("id_buq")

                dTable = Datos.consulta_reader("SELECT nombre_buq FROM dbo.buques WHERE id_buq='" & txtBuque.Text & "'")
                txtNomBuque.Text = dTable.Rows.Item(0).Item("nombre_buq")
                txtBuque.Enabled = False

                txtPrefijo.Focus()
            End If
            ' NuevoReg = True

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub txtTras_Validating(sender As Object, e As EventArgs) Handles txtTras.TextChanged
        Dim inicioCursor As Integer = txtTras.SelectionStart
        txtTras.Text = txtTras.Text.ToUpper()
        txtTras.SelectionStart = inicioCursor
        Dim valor As String = txtTras.Text.Trim().ToUpper()

        If valor = "A" Then
            cbTraslado.Text = "AUTOMATICO"
        ElseIf valor = "N" Then
            cbTraslado.Text = "NORMAL"
        End If
    End Sub


    Private Sub txtTipo_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtTipo.Validating

        If EstaPesando = "ENTRADA" Then
            txtTipo.Text = txtTipo.Text.ToUpper

            If txtTipo.Text = "D" Then
                cmbTipoOperacion.Text = "DESPACHO"
            Else
                If txtTipo.Text = "R" Then
                    cmbTipoOperacion.Text = "RECEPCION"
                End If
            End If

            'Mueve_Camion()
        End If

        If txtTipo.Text = String.Empty Then
            txtTipo.Focus()
        Else
            If txtTras.Enabled AndAlso txtTras.Visible Then
                txtTras.Focus()

            ElseIf txtBuque.Enabled Then
                txtBuque.Focus()
            Else
                txtProducto.Focus()
            End If
        End If
    End Sub

    Private Sub habilita_BL()

        Try
            dTable = Datos.consulta_reader("SELECT DISTINCT [buque_dbq] FROM [dbo].[Detalle_Buque] WHERE buque_dbq='" & txtBuque.Text & "'")

            If dTable.Rows.Count = 0 Then
                'BL
                txtBL.Text = ""
                txtBL.Enabled = False

                txtDuca.Text = ""
                txtNomConsigna.Text = ""
                txtBL_Manifiesto.Text = "0"
                txtBL_PesoBascula.Text = "0"
                txtBL_Diferencia.Text = "0"

                'Nombre Bodega sin Dato
                dTable = Datos.consulta_reader("SELECT id_bod FROM dbo.bodega WHERE RTRIM(LTRIM(nombre_bod))='SIN DATO'")
                txtBodega.Text = dTable.Rows.Item(0).Item("id_bod")

                dTable = Datos.consulta_reader("SELECT nombre_bod FROM dbo.bodega WHERE id_bod='" & txtBodega.Text & "'")
                txtNomBodega.Text = dTable.Rows.Item(0).Item("nombre_bod")
                txtBodega.Enabled = False

            Else
                txtBodega.Enabled = True
                txtBL.Enabled = True
            End If
        Catch ex As Exception

        End Try

    End Sub

#End Region

    Private Sub txtPlaca_Validating(sender As Object, e As EventArgs) Handles txtPlaca.Validating
        Try

            'Dim xx = txtPlaca.Text.Trim

            If txtPlaca.Text.Trim IsNot String.Empty Then

                'Mayuscula
                txtPlaca.Text = txtPlaca.Text.ToUpper
                txtPlaca.Text = txtPlaca.Text.Trim

                'Nombre Proveedor
                'dTable = Datos.consulta_reader("SELECT nombre_tpt, numero_fca FROM dbo.fcamiones LEFT JOIN dbo.transporte ON id_tpt = transporte_fca WHERE rtrim(ltrim(placa_fca))='" & txtPlaca.Text.Trim & "'")
                dTable = Datos.consulta_readerp("SELECT placa_fca FROM dbo.fcamiones WHERE rtrim(ltrim(placa_fca))='" & txtPlaca.Text.Trim & "'")

                If dTable.Rows.Count > 0 Then
                    txtNomTransporte.Text = "Existe: " & dTable.Rows.Item(0).Item("placa_fca").ToString()
                    txtNomTransporte.ForeColor = Color.Black         ' Color del texto
                Else
                    txtNomTransporte.Text = "Se Grabara como nueva Placa..!"
                    txtNomTransporte.ForeColor = Color.Red         ' Color del texto

                    'MessageBox.Show("ERROR: " & "La Placa no Existe ")
                End If

                txtNomTransporte.Refresh()
                'Para ver si ya existe una entrada
                If IdDetSolicitudDesp = 0 Then
                    ControlEntrada()
                Else

                End If
            End If
        Catch ex As Exception
            'MsgBox(ex.Message)
        End Try

    End Sub

#Region "Busque F2"
    'KeyDown, cuando presionamos F2 en el txtCodigo_Cajero
    'Funciona para que cuando presionamos dicha tecla nos muestre la ayuda.
    Private Sub txtProducto_KeyDown(sender As Object, e As KeyEventArgs) Handles txtProducto.KeyDown
        Select Case e.KeyCode
            Case Keys.F2
                BuscaProdu()
                'Case e.KeyCode.Equals(Keys.Down)

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

    Private Sub txtBodega_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBodega.KeyDown
        Select Case e.KeyCode
            Case Keys.F2
                BuscaBodega()
        End Select
    End Sub

    Private Sub txtPlaca_KeyDown(sender As Object, e As KeyEventArgs) Handles txtPlaca.KeyDown
        Select Case e.KeyCode
            Case Keys.F2
                buscaTransporte()
        End Select
    End Sub
#End Region


#Region "Grabar Y Tomar Peso"

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click

        'Valida Campos
        If ValidaCampos() Then

            Dim SadoPeso As Boolean = False
            Dim PesoQuintal As Double = 0.00
            btnGrabar.Enabled = False

            Try
                'LibreriaGeneral._EstePeso = lblPeso.Text

                'Dim frmPeso As New frmTomaPesaje
                'frmPeso.ShowDialog()

                txtPManifestado.Text = txtTomaPeso.Text
                PesoTaraCont = txtPTaraContenedor.Text
                PesoArana = txtPesoAraña.Text

                PesoManifestado = Val(txtTomaPeso.Text)

                If EsDesdeTemporal Then
                    txtPTaraContenedor.Text = Format(PesoManifestado, "###,###,###")
                    txtPTaraContenedor.Refresh()
                End If


                If txtPManifestado.Text <> "Fallo" Or txtPManifestado.Text <> "" Or txtPManifestado.Text <> "ERROR" Then
                    PesoManifestado = Val(txtTomaPeso.Text)

                    Mueve_Camion(720, 846)
                End If

            Catch ex As Exception
                txtPManifestado.Text = "0.00"

                PesoTaraCont = txtPTaraContenedor.Text
                PesoArana = txtPesoAraña.Text
                PesoManifestado = Val("0.0")

            End Try

            'PesoManifestado = "25,000.00"

            'MessageBox.Show(txtPManifestado.Text)

            If PesoManifestado > 0 Then
                'PesoTaraCont = txtPTaraContenedor.Text
                'PesoArana = txtPesoAraña.Text
                'PesoManifestado = Val("100.00")

                SadoPeso = True

                If cmbTipoOperacion.Text.Trim = "DESPACHO" Then
                    If EstaPesando = "ENTRADA" Then
                        '-->Tara Vehiculo = Lo que tomo el LECTOR
                        '-->Pero Neto = Lo que tomo del LECTOR
                        txtPTaraVehiculo.Text = Format(PesoManifestado, "###,###,###")
                        'txtPesoNeto.Text = Format(PesoManifestado, "###,###,###")
                    Else
                        '-->Peso Bruto = Lo que tomo el LECTOR
                        '-->Pero Neto =  Peso Bruto - (Tara + TaraCont + PesoAraña)
                        '-->Peso VGM = Tara + PesoNeto
                        txtPesoBruto.Text = Format(PesoManifestado, "###,###,###")
                        txtPesoNeto.Text = Format(Convert.ToDouble(txtPesoBruto.Text) - (Convert.ToDouble(txtPTaraVehiculo.Text) + Convert.ToDouble(txtPTaraContenedor.Text) + Convert.ToDouble(txtPesoAraña.Text)), "###,###,###")

                        'Validad Contenedor
                        If txtPesoNeto.Text.Trim = "" Then
                            txtPesoNeto.Text = "0.00"
                        End If
                        If txtPTaraContenedor.Text.Trim = "" Then
                            txtPTaraContenedor.Text = "0.00"
                        End If

                        TxtPesoVGM.Text = Format(Convert.ToDouble(txtPesoNeto.Text) + Convert.ToDouble(txtPTaraContenedor.Text), "###,###,###")

                        If txtBL.Text IsNot String.Empty Then
                            PesoQuintal = Math.Round((Convert.ToDouble(txtPesoNeto.Text) * 2.2046) / 100, 5)
                            TxtQuintales.Text = PesoQuintal.ToString("F3")
                        End If

                    End If

                Else  'EXPORT - RECEPCION

                    If EstaPesando = "ENTRADA" Then
                        '-->Peso Bruto = Lo que tomo el LECTOR
                        '-->Pero Neto = Lo que tomo del LECTOR
                        txtPesoBruto.Text = Format(PesoManifestado, "###,###,###")
                        'txtPesoNeto.Text = Format(PesoManifestado, "###,###,###")
                        txtPesoNeto.Text = Format(PesoManifestado - (Convert.ToDouble(txtPTaraContenedor.Text) + Convert.ToDouble(txtPesoAraña.Text)), "###,###,###")
                    Else
                        '-->Peso Tara = Lo que tomo el LECTOR
                        '-->Pero Neto =  Peso Bruto - (Tara + TaraCont + PesoAraña)
                        '-->Peso VGM = Tara + PesoNeto

                        txtPTaraVehiculo.Text = Format(PesoManifestado, "###,###,###")
                        txtPesoNeto.Text = Format(Convert.ToDouble(txtPesoBruto.Text) - (Convert.ToDouble(txtPTaraVehiculo.Text) + Convert.ToDouble(txtPTaraContenedor.Text) + Convert.ToDouble(txtPesoAraña.Text)), "###,###,###")


                        'Validad Contenedor
                        If txtPesoNeto.Text.Trim = "" Then
                            txtPesoNeto.Text = "0.00"
                        End If
                        If txtPTaraContenedor.Text.Trim = "" Then
                            txtPTaraContenedor.Text = "0.00"
                        End If

                        TxtPesoVGM.Text = Format(Convert.ToDouble(txtPesoNeto.Text) + Convert.ToDouble(txtPTaraContenedor.Text), "###,###,###")

                        If txtBL.Text IsNot String.Empty Then
                            PesoQuintal = Math.Round((Convert.ToDouble(txtPesoNeto.Text) * 2.2046) / 100, 5)
                            TxtQuintales.Text = PesoQuintal.ToString("F3")
                        End If

                    End If

                End If
            Else
                SadoPeso = False
                btnGrabar.Enabled = False
            End If

            'Verifica Saldo Peso
            If SadoPeso Then
                btnGrabar.Enabled = True
                'SendKeys.Send("{TAB}")
                btnGrabar.Focus()
                'Else
                '    btnGrabar.Enabled = True
                '    txtNaviera.Focus()
            End If

        End If
    End Sub


    Private Function ValidaCampos() As Boolean
        Dim Si As Boolean = False

        'Solictados
        txtProducto_Validating(txtProducto.Text, Nothing)
        txtBuque_Validating(txtBuque.Text, Nothing)
        txtNaviera_Validating(txtNaviera.Text, Nothing)
        txtBodega_Validating(txtBodega.Text, Nothing)
        txtTras_Validating(txtTras.Text, Nothing)

        If cmbTipoOperacion.Text = String.Empty Then
            MessageBox.Show("ERROR: El Tipo de Operación No puede Ser Blanco")
            txtTipo.Focus()
        ElseIf txtNomBuque.Text = String.Empty Then
            MessageBox.Show("ERROR: El Buque No puede ser Blanco")
            txtBuque.Focus()
        ElseIf txtNomProducto.Text = String.Empty Then
            MessageBox.Show("ERROR: El Producto No puede ser Blanco")
            txtProducto.Focus()
        ElseIf txtNomNaviera.Text = String.Empty Then
            MessageBox.Show("ERROR: La Empresa No puede ser Blanco")
            txtNaviera.Focus()
        ElseIf txtNomBodega.Text = String.Empty Then
            MessageBox.Show("ERROR: La Bodega No puede ser Blanco")
            txtBodega.Focus()
        ElseIf txtBL.Text = String.Empty And txtBL.Enabled Then
            MessageBox.Show("ERROR: El BL No puede ser Blanco")
            txtBL.Focus()
        ElseIf txtDuca.Text = String.Empty And txtBL.Enabled Then
            MessageBox.Show("ERROR: La Duca No puede ser Blanco")
            txtBL.Focus()
        ElseIf cbTraslado.Text = String.Empty Then
            MessageBox.Show("ERROR: El Tipo de Traslado No puede Ser Blanco")
            txtTras.Focus()
        Else
            Si = True
        End If

        'Valida Contenedort
        If Si = True Then
            If txtClase.Text = "C" Then

                If txtPrefijo.Text = String.Empty Then
                    MessageBox.Show("ERROR: El Prefijo No puede Ser Blanco")
                    txtPrefijo.Focus()
                ElseIf txtContenedor.Text = String.Empty Then
                    MessageBox.Show("ERROR: El Contenedor No puede ser Blanco")
                    txtContenedor.Focus()
                ElseIf txtMedida.Text = String.Empty Then
                    MessageBox.Show("ERROR: La Medida No puede ser Blanco")
                    txtMedida.Focus()
                ElseIf txtTipoCarga.Text = String.Empty Then
                    MessageBox.Show("ERROR: El Tipo de Carga No puede ser Blanco")
                    txtTipoCarga.Focus()
                Else
                    Si = True
                End If
            End If
        End If

        Return Si
    End Function

    Private Sub btnGrabar_Click(sender As Object, e As EventArgs) Handles btnGrabar.Click
        If Not ValidarInventario() Then
            Exit Sub
        End If
        Grabar()
    End Sub


    Private Sub cmbClaseCarga_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbClaseCarga.SelectedIndexChanged

        ''Clase Carga
        'dTable2 = Datos.consulta_reader("select Abrevia_cla from clasecarga WHERE NombreClase_cla = '" & cmbClaseCarga.Text & "'")

        'If dTable2.Rows.Count > 0 Then

        '    For Each dRow2 As DataRow In dTable2.Rows
        '        txtClase.Text = dRow2.Item("Abrevia_cla").ToString
        '    Next
        'End If


        If EsContene = False Then           'No Es contenedor
            'Contenedor
            txtPrefijo.Enabled = False
            txtContenedor.Enabled = False
            txtMedida.Enabled = False
            txtTipoCarga.Enabled = False
            txtDiceContenedor.Enabled = False
        Else

            'Contenedor
            txtPrefijo.Enabled = True
            txtContenedor.Enabled = True
            txtMedida.Enabled = True
            txtTipoCarga.Enabled = True
            txtDiceContenedor.Enabled = True
        End If

    End Sub

    'Verifica si ya existe una entrada y obtiene todos los valores y los muestra
    'para obtener unicamente la salida
    Private Sub ControlEntrada()
        Try

            Dim PesoQuintal As Double
            Dim PesoNeto As Double

            'Para mostrar el Dato
            Dim StringSqlE As String = <sqlExp>
                                        SELECT 
                                            p.*,
                                            d.iddetalle_solicitud_despacho,
                                            d.aprobado AS aprobado_detalle
                                        FROM pesajes p
                                        LEFT JOIN detalle_solicitud_despacho d
                                            ON d.iddetalle_solicitud_despacho = p.iddetalle_solicitud_despacho
                                        WHERE p.ticketboleta_pes IN (
                                                SELECT ticketboleta_pes 
                                                FROM pesajes
                                                GROUP BY ticketboleta_pes
                                                HAVING COUNT(ticketboleta_pes) = 1
                                            )
                                          AND RTRIM(LTRIM(p.placa_pes)) = '<%= txtPlaca.Text %>' 
                                          AND (
                                                LTRIM(RTRIM(p.EstePesaje_pes)) = 'ENTRADA'
                                                OR LTRIM(RTRIM(p.EstePesaje_pes)) LIKE 'TEMPORAL%'
                                              )
                                          AND LTRIM(RTRIM(p.cierre_pes)) = 'NO'
                                          AND p.baja_pes = 0
                                    </sqlExp>.Value

            dTable = Datos.consulta_reader(StringSqlE)

            If dTable.Rows.Count > 0 Then

                'Setea Campos
                For Each dRow As DataRow In dTable.Rows
                    IdPesTemporal = CInt(dRow.Item("id_pes"))
                    txtTicket.Text = dRow.Item("ticketboleta_pes").ToString

                    'Infomrativos
                    txtBasculaEntrada.Text = dRow.Item("Idbas_entrada_pes").ToString
                    dFechaEntrada.Value = dRow.Item("fechahora_entrada_pes")
                    hEntrada.Value = dRow.Item("fechahora_entrada_pes")
                    txtOperadorEntrada.Text = dRow.Item("idusu_entrada_pes").ToString

                    'Contenedor
                    txtPrefijo.Text = dRow.Item("prefijocont_pes").ToString
                    txtContenedor.Text = dRow.Item("numidentcont_pes").ToString
                    txtMedida.Text = IIf(dRow.Item("tamanocon_pes").ToString = "0", "", dRow.Item("tamanocon_pes").ToString)
                    txtTipoCarga.Text = dRow.Item("tipocarga_pes").ToString
                    txtDiceContenedor.Text = dRow.Item("diceContener_pes").ToString

                    'Pesaje
                    txtPTaraVehiculo.Text = dRow.Item("pesotara_pes").ToString
                    txtPTaraContenedor.Text = dRow.Item("pesotaracont_pes").ToString

                    'Solictados
                    txtProducto.Text = dRow.Item("idpro_pes").ToString
                    txtProducto_Validating(txtProducto.Text, Nothing)

                    cmbTipoOperacion.Text = dRow.Item("tipooperacion_pes").ToString

                    txtBuque.Text = dRow.Item("idbuq_pes").ToString
                    txtBuque_Validating(txtBuque.Text, Nothing)
                    'txtViaje.Text = dRow.Item("idcla_pes").ToString

                    txtNaviera.Text = dRow.Item("idcli_pes").ToString
                    txtNaviera_Validating(txtNaviera.Text, Nothing)

                    txtBodega.Text = dRow.Item("bodegabuq_pes").ToString
                    txtBodega_Validating(txtBodega.Text, Nothing)

                    txtBL.Text = dRow.Item("blbuq_pes").ToString.Trim
                    txtBL_Validating(txtBL.Text, Nothing)

                    ' === GUARDAR EL ID DEL DETALLE Y CARGAR EL APROBADO ===
                    If Not IsDBNull(dRow("iddetalle_solicitud_despacho")) Then
                        IdDetSolicitudDesp = CInt(dRow("iddetalle_solicitud_despacho"))
                        CargarAprobadoDesdeDetalle()
                    Else
                        IdDetSolicitudDesp = 0
                        txtAPROBADO.Text = "0.00"
                    End If

                    'Pesaje   
                    If dRow.Item("TipoOperacion_pes").ToString.Trim = "DESPACHO" Then

                        If dRow.Item("EstePesaje_pes").ToString.Trim = "ENTRADA" OrElse
                        dRow.Item("EstePesaje_pes").ToString.Trim = "TEMPORAL" Then

                            txtPTaraVehiculo.Text = dRow.Item("PesoTara_pes").ToString
                            txtPesoNeto.Text = dRow.Item("PesoTara_pes").ToString
                        Else
                            txtPesoBruto.Text = dRow.Item("PesoBruto_pes").ToString
                            txtPesoNeto.Text = Convert.ToDouble(txtPesoBruto.Text) - Convert.ToDouble(txtPTaraVehiculo.Text)

                            If txtBL.Text IsNot String.Empty Then
                                PesoQuintal = Math.Round((Convert.ToDouble(txtPesoNeto.Text) * 2.2046) / 100, 5)
                                TxtQuintales.Text = PesoQuintal.ToString("F3")
                            End If
                        End If

                    Else  'EXPORT

                        If dRow.Item("EstePesaje_pes").ToString.Trim = "ENTRADA" OrElse
                           dRow.Item("EstePesaje_pes").ToString.Trim = "TEMPORAL" Then

                            txtPesoBruto.Text = dRow.Item("PesoBruto_pes").ToString

                            PesoNeto = Convert.ToDouble(txtPesoBruto.Text) - Convert.ToDouble(txtPTaraContenedor.Text)
                            txtPesoNeto.Text = PesoNeto.ToString("F3")
                        Else
                            txtPTaraVehiculo.Text = dRow.Item("PesoTara_pes").ToString

                            PesoNeto = Convert.ToDouble(txtPesoBruto.Text) - Convert.ToDouble(txtPTaraVehiculo.Text)
                            txtPesoNeto.Text = PesoNeto.ToString("F3")

                            If txtBL.Text IsNot String.Empty Then
                                PesoQuintal = Math.Round((Convert.ToDouble(txtPesoNeto.Text) * 2.2046) / 100, 5)
                                TxtQuintales.Text = PesoQuintal.ToString("F3")
                            End If

                        End If

                    End If

                    If cmbTipoOperacion.Text.Trim = "DESPACHO" Then
                        txtTipo.Text = "D"
                    Else
                        txtTipo.Text = "R"
                    End If

                    'Clase Carga
                    dTable2 = Datos.consulta_reader("select nombreclase_cla,Abrevia_cla from clasecarga WHERE id_cla = '" & dRow.Item("idcla_pes").ToString & "'")

                    If dTable2.Rows.Count > 0 Then

                        For Each dRow2 As DataRow In dTable2.Rows
                            cmbClaseCarga.Text = dRow2.Item("NombreClase_cla").ToString
                            txtClase.Text = dRow2.Item("Abrevia_cla").ToString
                        Next
                    End If

                    'Horas, Usuario y bascula
                    txtBasculaEntrada.Text = dRow.Item("idbas_entrada_pes").ToString
                    dFechaEntrada.Value = dRow.Item("fechahora_entrada_pes").ToString
                    txtOperadorEntrada.Text = dRow.Item("idusu_entrada_pes").ToString

                    Dim rawEstado As String = dRow.Item("EstePesaje_pes").ToString().Trim()
                    Dim partes() As String = rawEstado.Split("|"c)

                    Dim tipoPesaje As String = partes(0)  ' "ENTRADA", "SALIDA" o "TEMPORAL"

                    If tipoPesaje = "TEMPORAL" Then

                        EsDesdeTemporal = True
                        EstaPesando = "ENTRADA"
                    Else

                        EsDesdeTemporal = False
                        EstaPesando = "SALIDA"
                    End If

                Next

            Else

                EsDesdeTemporal = False
                EstaPesando = "ENTRADA"
                SumaTicket()
            End If


        Catch ex As Exception
            MessageBox.Show("ERROR: " & ex.Message, "Comprobar tabla * Producto")
        End Try
    End Sub
#End Region


    'Procedimiento que graba tanto la entrada como la Salida
    'Si hay una entrada tiene que existir una Sadlia
    Private Sub Grabar()

        Dim host As Integer
        Dim Periodo As Integer
        Dim Ticket As String = ""
        Dim pActivo As Integer = "0"

        Dim Bascula As String = LibreriaGeneral.nBascu
        Dim Usuario As String = LibreriaGeneral.usuario
        Dim fechahora As DateTime = DateTime.Now
        Dim StringSql As String
        Dim Paso As String = "No"
        Dim IdCorrela As String = "0"
        Dim Paso1 As Integer = 0
        Dim fechahoraFormat As String = fechahora.ToString("yyyyMMdd H:mm:ss")
        Dim vfechaFormat As String = dFechav.Value.ToString("yyyyMMdd")
        Dim vFechaEntrada As String = dFechaEntrada.Value.ToString("yyyyMMdd H:mm:ss")

        'Para verficar si hay dos entradas o dos salidas del mismo Ticket
        Dim StringSqlE As String = <sqlExp>
                                          SELECT Ticketboleta_pes, COUNT(ticketboleta_pes) As datos, 
											 'ENTRADA' As Tipo
                                          FROM pesajes
                                          WHERE RTRIM(LTRIM(Ticketboleta_pes)) = '<%= txtTicket.Text %>'
											 AND EstePesaje_pes = 'ENTRADA'
                                             GROUP by ticketboleta_pes
                                             HAVING COUNT(ticketboleta_pes) > 0
										 UNION ALL
										 SELECT Ticketboleta_pes, COUNT(ticketboleta_pes) As datos, 
											 'SALIDA' As Tipo
                                          FROM pesajes
                                             WHERE RTRIM(LTRIM(Ticketboleta_pes)) = '<%= txtTicket.Text %>'
											 AND EstePesaje_pes = 'SALIDA'
                                             GROUP by ticketboleta_pes
                                             HAVING COUNT(ticketboleta_pes) > 0
                                     </sqlExp>.Value

        dTable2 = Datos.consulta_reader(StringSqlE)

        If dTable2.Rows.Count > 0 Then
            Dim Existe As String = ""

            'MessageBox.Show("ERROR: " & " El No. de Ticket esta duplicado, por favor Revisar !!!")

            'Setea Campos
            For Each dRow As DataRow In dTable2.Rows
                If EstaPesando = dRow.Item("Tipo").ToString.Trim Then

                    Existe = dRow.Item("Ticketboleta_pes").ToString + " " + dRow.Item("Tipo").ToString

                    Paso1 = 1
                End If
            Next

            If Paso1 = 1 Then
                MessageBox.Show("ERROR: " & "  El No. de Ticket  " & Existe & "  Ya Existe !!")
            End If

        End If


        If Paso1 = 0 Then
            Try

                'Suma Correlativo
                dTable = Datos.consulta_reader("SELECT NEXT VALUE FOR [dbo].[SeqPesajesId] AS rNext")

                If dTable IsNot Nothing AndAlso dTable.Rows.Count > 0 Then
                    ' Obtenemos el ID único que SQL ya reservó para nosotros
                    IdCorrela = dTable.Rows(0).Item("rNext").ToString
                Else
                    ' Este caso es muy raro que ocurra con secuencias, pero se deja por seguridad
                    IdCorrela = "1"
                End If

                ' Valor para iddetalle_solicitud_despacho
                Dim detSolicitudValor As String
                If IdDetSolicitudDesp > 0 Then
                    detSolicitudValor = IdDetSolicitudDesp.ToString()
                Else
                    detSolicitudValor = "NULL"
                End If

                If EstaPesando = "ENTRADA" Then


                    If EsDesdeTemporal Then
                        Ticket = txtTicket.Text
                    Else
                        SumaTicket()
                        Ticket = txtTicket.Text
                    End If

                    StringSql = <SqlString>
                             INSERT INTO [dbo].[pesajes]
                                       ([id_pes], [host_pes]
                                       ,[periodocorrelativo_pes]
                                       ,[ticketboleta_pes]
                                       ,[placa_pes]
                                       ,[Idcla_pes]
                                       ,[prefijocont_pes]
                                       ,[numidentcont_pes]
                                       ,[tamanocon_pes]
                                       ,[tipocarga_pes]
                                       ,[dicecontener_pes]
                                       ,[Idpro_pes]
                                       ,[tipooperacion_pes]
                                       ,[Idbuq_pes]
                                       ,[bodegabuq_pes]
                                       ,[Idvbuq_pes]
                                       ,[fechav_pes]
                                       ,[Idcli_pes]
                                       ,[Idpaisdest_pes]
                                       ,[Idpaisorig_pes]
                                       ,[pesomanif_pes]
                                       ,[pesotara_pes]
                                       ,[pesotaracont_pes]
                                       ,[pesobruto_pes]
                                       ,[pesoarana_pes]
                                       ,[Idbas_entrada_pes]
                                       ,[fechahora_entrada_pes]
                                       ,[idusu_entrada_pes]
                                       ,[id_usu_insert_pes]
                                       ,[fecha_insert_pes]
                                       ,[cierre_pes] 
                                       ,[EstePesaje_pes]
                                       ,[baja_pes]
                                       ,[blbuq_pes]
                                       ,[empresa_pes]
                                       ,[sync_pes]
                                       ,[iddetalle_solicitud_despacho])
                                    VALUES
                                       ('<%= IdCorrela %>', <%= host %>
                                       ,<%= Periodo %>
                                       ,'<%= Ticket %>'
                                       ,'<%= txtPlaca.Text %>'
                                       ,'<%= cmbClaseCarga.SelectedItem("id_cla").ToString %>'
                                       ,'<%= txtPrefijo.Text %>'
                                       ,'<%= txtContenedor.Text %>'
                                       ,'<%= txtMedida.Text %>'
                                       ,'<%= txtTipoCarga.Text %>'
                                       ,'<%= txtDiceContenedor.Text %>'
                                       ,'<%= txtProducto.Text %>'
                                       ,'<%= cmbTipoOperacion.Text %>'
                                       ,'<%= txtBuque.Text %>'
                                       ,'<%= txtBodega.Text %>'
                                       ,'<%= txtViaje.Text %>'
                                       ,'<%= vfechaFormat %>'
                                       ,'<%= txtNaviera.Text %>'
                                       ,'<%= "4" %>'
                                       ,'<%= "4" %>'
                                       ,<%= txtPManifestado.Text.Replace(",", "") %>
                                       ,<%= txtPTaraVehiculo.Text.Replace(",", "") %>
                                       ,<%= txtPTaraContenedor.Text.Replace(",", "") %>
                                       ,<%= txtPesoBruto.Text.Replace(",", "") %>
                                       ,<%= txtPesoAraña.Text.Replace(",", "") %>
                                       ,'<%= Bascula %>'
                                       ,'<%= fechahoraFormat %>'
                                       ,'<%= Usuario %>'
                                       ,'<%= Usuario %>'
                                       ,'<%= fechahoraFormat %>'
                                       ,'<%= "NO" %>'
                                       ,'<%= EstaPesando %>'
                                       ,<%= pActivo %>
                                       ,'<%= txtBL.Text %>'
                                       ,'<%= LibreriaGeneral.gEmpresa %>'
                                       ,1
                                       ,<%= detSolicitudValor %>)
                                </SqlString>

                Else

                    'SumaTicket()
                    Ticket = txtTicket.Text

                    StringSql = <SqlString>
                             INSERT INTO [dbo].[pesajes]
                                       ([id_pes], [host_pes]
                                       ,[periodocorrelativo_pes]
                                       ,[ticketboleta_pes]
                                       ,[placa_pes]
                                       ,[Idcla_pes]
                                       ,[prefijocont_pes]
                                       ,[numidentcont_pes]
                                       ,[tamanocon_pes]
                                       ,[tipocarga_pes]
                                       ,[dicecontener_pes]
                                       ,[Idpro_pes]
                                       ,[tipooperacion_pes]
                                       ,[Idbuq_pes]
                                       ,[bodegabuq_pes]
                                       ,[Idvbuq_pes]
                                       ,[fechav_pes]
                                       ,[Idcli_pes]
                                       ,[Idpaisdest_pes]
                                       ,[Idpaisorig_pes]
                                       ,[pesomanif_pes]
                                       ,[pesotara_pes]
                                       ,[pesotaracont_pes]
                                       ,[pesobruto_pes]
                                       ,[pesoarana_pes]
                                       ,[Idbas_entrada_pes]
                                       ,[fechahora_entrada_pes]
                                       ,[idusu_entrada_pes]
                                       ,[Idbas_salida_pes]
                                       ,[idusu_salida_pes]                                     
                                       ,[fechahora_salida_pes]
                                       ,[fechahora_cierre_pes]
                                       ,[cierre_pes]
                                       ,[id_usu_insert_pes]
                                       ,[fecha_insert_pes]
                                       ,[EstePesaje_pes]
                                       ,[baja_pes]
                                       ,[blbuq_pes]
                                       ,[empresa_pes]
                                       ,[sync_pes]
                                       ,[iddetalle_solicitud_despacho])
                                    VALUES
                                       ('<%= IdCorrela %>', <%= host %>
                                       ,<%= Periodo %>
                                       ,'<%= Ticket %>'
                                       ,'<%= txtPlaca.Text %>'
                                       ,'<%= cmbClaseCarga.SelectedItem("id_cla").ToString %>'
                                       ,'<%= txtPrefijo.Text %>'
                                       ,'<%= txtContenedor.Text %>'
                                       ,'<%= txtMedida.Text %>'
                                       ,'<%= txtTipoCarga.Text %>'
                                       ,'<%= txtDiceContenedor.Text %>'
                                       ,'<%= txtProducto.Text %>'
                                       ,'<%= cmbTipoOperacion.Text %>'
                                       ,'<%= txtBuque.Text %>'
                                       ,'<%= txtBodega.Text %>'
                                       ,'<%= txtViaje.Text %>'
                                       ,'<%= vfechaFormat %>'
                                       ,'<%= txtNaviera.Text %>'
                                       ,'<%= "4" %>'
                                       ,'<%= "4" %>'
                                       ,<%= txtPManifestado.Text.Replace(",", "") %>
                                       ,<%= txtPTaraVehiculo.Text.Replace(",", "") %>
                                       ,<%= txtPTaraContenedor.Text.Replace(",", "") %>
                                       ,<%= txtPesoBruto.Text.Replace(",", "") %>
                                       ,<%= txtPesoAraña.Text.Replace(",", "") %>
                                       ,'<%= txtBasculaEntrada.Text %>'
                                       ,'<%= vFechaEntrada %>'
                                       ,'<%= txtOperadorEntrada.Text %>'
                                       ,'<%= Bascula %>'
                                       ,'<%= Usuario %>'
                                       ,'<%= fechahoraFormat %>'
                                       ,'<%= fechahoraFormat %>'
                                       ,'<%= "CIERRE" %>'
                                       ,'<%= Usuario %>'
                                       ,'<%= fechahoraFormat %>'
                                       ,'<%= EstaPesando %>'
                                       ,<%= pActivo %>
                                       ,'<%= txtBL.Text %>'
                                       ,'<%= LibreriaGeneral.gEmpresa %>'
                                       ,1
                                       ,<%= detSolicitudValor %>)
                                </SqlString>
                End If

                Try

                    'LLamamos la rutina para grabar
                    Datos.consulta_non_query(StringSql)

                    Paso = "Si"
                Catch ex As Exception
                    MessageBox.Show("ERROR: " & ex.Message, "Comprobar tabla * Pesaje")
                    Paso = "No"
                End Try

            Catch ex As Exception
                MessageBox.Show("ERROR: " & ex.Message, "Comprobar tabla * Pesaje")
                Paso = "No"
            End Try

            If EstaPesando = "ENTRADA" And Paso = "Si" Then

                If Not EsDesdeTemporal Then
                    Datos.consulta_non_queryDeta("
            UPDATE correlactu 
            SET nticketboleta_cor = (SELECT CAST(nticketboleta_cor + 1 AS VARCHAR(10)) AS 'X' FROM correlactu)
        ")
                End If

                'Para mostrar el Dato
                'Datos.consulta_non_queryDeta("UPDATE correlactu SET nticketboleta_cor = (SELECT REPLICATE('0', 10-LEN(CAST(nticketboleta_cor + 1 AS VARCHAR(10))))+CAST(nticketboleta_cor + 1 AS VARCHAR(10)) AS 'X' FROM correlactu)")

                'Para Verficicar y Grabar la Placa 
                dTable2 = Datos.consulta_reader("SELECT placa_fca FROM dbo.Fcamiones WHERE placa_fca='" & txtPlaca.Text & "'")

                If dTable2.Rows.Count = 0 Then

                    'Suma Correlativo
                    dTable = Datos.consulta_reader("SELECT MAX(id_fca)+1 rMax FROM [dbo].[fcamiones]")

                    Dim txtId As String = ""
                    If dTable.Rows.Count > 0 Then
                        'Setea Campos
                        For Each dRow As DataRow In dTable.Rows
                            'Infomrativos
                            txtId = dRow.Item("rMax").ToString
                        Next
                    Else
                        txtId = "1"
                    End If


                    'Se arma el script para grabar SQL
                    Dim StringSql4 As String = <sqlExp>
                                    INSERT INTO [dbo].[fcamiones] ([id_fca], [placa_fca],
                                                                [model_fca],
                                                                [color_fca],
                                                                [numero_fca],
                                                                [baja_fca],
                                                                [transporte_fca],
                                                                [actualiza_fca])
                                                     VALUES('<%= txtId %>', '<%= txtPlaca.Text %>',
                                                          '<%= "" %>',
                                                          '<%= "" %>',
                                                          '<%= "0" %>',
                                                          '<%= "ACTIVO" %>',
                                                          '<%= "0" %>',
                                                          '<%= "1" %>')
                                     </sqlExp>.Value

                    'LLamamos la rutina para grabar
                    Datos.consulta_non_queryDeta(StringSql4)

                End If

                If IdDetSolicitudDesp > 0 Then
                    Try
                        Dim sqlUpdDet As String = <sqlExp>
                UPDATE detalle_solicitud_despacho
                SET estado_pesaje = 3
                WHERE iddetalle_solicitud_despacho = <%= IdDetSolicitudDesp %>
                                                  </sqlExp>.Value

                        Datos.consulta_non_queryDeta(sqlUpdDet)
                    Catch ex As Exception
                        MessageBox.Show("No se pudo actualizar el estado de pesaje del detalle de solicitud: " &
                                        ex.Message,
                                        "Detalle solicitud", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End Try
                End If
            Else

                'Se arma el script para grabar SQL 
                'y Actuliza Saldo de Bascula y Diferencia en BL
                Dim StringBlSql As String = <sqlExp>
                                     UPDATE [dbo].[Detalle_Buque] SET 
                                                                 [bascula_dbq]  = <%= txtBL_PesoBascula.Text.Replace(",", "") %>,
                                                                 [diferencia_dbq] = <%= txtBL_Diferencia.Text.Replace(",", "") %>,
                                                                 sync_dbq = '<%= "2" %>'
                                       WHERE bl_dbq = '<%= txtBL.Text %>' 
                                          AND buque_dbq = '<%= txtBuque.Text %>' 
                                          AND producto_dbq = '<%= txtProducto.Text %>'
                                          AND empresa_dbq = '<%= LibreriaGeneral.gEmpresa %>'
                                  </sqlExp>.Value

                'LLamamos la rutina para grabar
                Datos.consulta_non_queryDeta(StringBlSql)
            End If

            If Paso = "Si" Then
                If EstaPesando = "SALIDA" Then
                    Dim tipoOp As String = cmbTipoOperacion.Text.Trim().ToUpper()

                    If tipoOp = "RECEPCION" Then
                        GoTo SaltarInventario

                    End If

                    Try
                        Dim pesoReal As Decimal
                        Dim textoPeso As String = txtPesoNeto.Text.Trim()
                        Dim blBuqueValor As String
                        Dim whereDetBuque As String

                        If IdDetalleBuque <= 0 Then
                            MessageBox.Show("No se pudo validar inventario.", "Inventario")
                            GoTo SaltarInventario
                        End If

                        Dim consignatarioValor As String = IdConsignatario.ToString()

                        Dim clienteValor As String
                        If String.IsNullOrWhiteSpace(txtNaviera.Text) Then
                            clienteValor = "NULL"
                        Else
                            clienteValor = txtNaviera.Text.Trim()
                        End If

                        textoPeso = textoPeso.Replace(",", "").Replace(" ", "")

                        If Not Decimal.TryParse(textoPeso,
                                NumberStyles.Any,
                                CultureInfo.InvariantCulture,
                                pesoReal) Then
                            MessageBox.Show("No se pudo convertir el Peso Neto para inventario.", "Inventario")
                            GoTo SaltarInventario
                        End If

                        ' Lo formateamos a 2 decimales para SQL (decimal(10,2))
                        Dim pesoRealSql As String = pesoReal.ToString("0.00", CultureInfo.InvariantCulture)

                        '--- 1) Armamos el WHERE dinámico (por el tema de NULL) ---
                        whereDetBuque = " AND iddetalle_buque = " & IdDetalleBuque

                        Dim whereCons As String = " AND consignatario_id = " & IdConsignatario

                        Dim whereCli As String
                        If clienteValor = "NULL" Then
                            whereCli = " AND cliente_id IS NULL"
                        Else
                            whereCli = " AND cliente_id = " & clienteValor
                        End If

                        '--- 2) Ver si ya existe inventario con esas mismas llaves ---
                        Dim sqlBuscaInv As String = <sqlExp>
                                                        SELECT TOP 1 *
                                                        FROM [dbo].[inventario]
                                                        WHERE bodega_idbodega = '<%= txtBodega.Text %>'
                                                        <%= whereDetBuque %>
                                                        <%= whereCons %>
                                                        <%= whereCli %>
                                                    </sqlExp>.Value



                        Dim dtInv As DataTable = Datos.consulta_reader(sqlBuscaInv)


                        If tipoOp = "DESPACHO" Then

                            If cbTraslado.Text.Trim().ToUpper() = "AUTOMATICO" Then
                                Using con As New SqlClient.SqlConnection(Datos.srt_conexion)
                                    Try
                                        con.Open()
                                        Using cmd As New SqlClient.SqlCommand("SP_Traslado_FIFO", con)
                                            cmd.CommandType = CommandType.StoredProcedure

                                            cmd.Parameters.AddWithValue("@tipo_traslado", "SALIDA")
                                            cmd.Parameters.AddWithValue("@bl_origen", txtBL.Text.Trim())
                                            cmd.Parameters.AddWithValue("@buque_id_origen", Val(txtBuque.Text))
                                            cmd.Parameters.AddWithValue("@cantidad_solicitada", pesoRealSql)
                                            cmd.Parameters.AddWithValue("@bodega_id", Val(txtBodega.Text))
                                            cmd.Parameters.AddWithValue("@producto_id", Val(txtProducto.Text))
                                            cmd.Parameters.AddWithValue("@consignatario_id", Val(IdConsignatario))
                                            cmd.Parameters.AddWithValue("@duca_origen", txtDuca.Text.Trim())
                                            Using reader As SqlClient.SqlDataReader = cmd.ExecuteReader()
                                                If reader.Read() Then

                                                    Dim msg As String = reader("mensaje").ToString()
                                                    MessageBox.Show(msg, "Traslado FIFO", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                                End If
                                            End Using
                                        End Using
                                    Catch ex As SqlClient.SqlException

                                        MessageBox.Show("Error en Traslado: " & ex.Message, "Error FIFO", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        Textos()
                                        SumaTicket()
                                        LeerData(grdData)
                                        LeerTemporales(dataSol)
                                        Return ' Detener proceso
                                    Catch ex As Exception
                                        MessageBox.Show("Error General: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        Textos()
                                        SumaTicket()
                                        LeerData(grdData)
                                        LeerTemporales(dataSol)
                                        Return
                                    End Try
                                End Using
                                GoTo SaltarInventario
                            End If

                            If dtInv.Rows.Count = 0 Then
                                MessageBox.Show("No existe inventario para este despacho.", "Inventario")
                                GoTo SaltarInventario
                            End If

                            Dim stockActual As Decimal = CDec(dtInv.Rows(0)("stock"))

                            Dim mermaPermitida As Decimal = stockActual * 0.05D
                            Dim maxDespachable As Decimal = stockActual + mermaPermitida

                            If pesoReal > maxDespachable Then
                                MessageBox.Show(
                                    "No hay suficiente inventario ni con el 5% de merma permitido." & vbCrLf &
                                    "Stock actual: " & stockActual.ToString("0.00") & " kg" & vbCrLf &
                                    "Merma permitida (5%): " & mermaPermitida.ToString("0.00") & " kg" & vbCrLf &
                                    "Máximo despachable: " & maxDespachable.ToString("0.00") & " kg" & vbCrLf &
                                    "Peso a despachar: " & pesoReal.ToString("0.00") & " kg",
                                    "Inventario insuficiente"
                                )
                                GoTo SaltarInventario   ' importante: no sigue el flujo ni imprime ticket
                            End If

                            ' Si llega aquí es porque está dentro del 5% permitido
                            Dim cantidadARestar As Decimal = Math.Min(pesoReal, stockActual)

                            Dim sqlUpdateInv As String = <sqlExp>
                                                            UPDATE [dbo].[inventario]
                                                            SET stock = stock - <%= cantidadARestar.ToString("0.00", CultureInfo.InvariantCulture) %>,
                                                                fecha_actualizacion = GETDATE()
                                                            WHERE bodega_idbodega = '<%= txtBodega.Text %>'
                                                            <%= whereDetBuque %>
                                                             <%= whereCons %>
                                                             <%= whereCli %>
                                                         </sqlExp>.Value

                            Datos.consulta_non_queryDeta(sqlUpdateInv)

                            If IdDetSolicitudDesp > 0 Then

                                ' Dejar aprobado en 0 para este detalle
                                Dim sqlUpdDetAprobado As String = <sqlExp>
                                            UPDATE [dbo].[detalle_solicitud_despacho]
                                            SET despachado = ISNULL(aprobado, 0),
                                                aprobado   = 0
                                            WHERE iddetalle_solicitud_despacho = <%= IdDetSolicitudDesp %>
                                                                  </sqlExp>.Value

                                Datos.consulta_non_queryDeta(sqlUpdDetAprobado)

                                ' poner en estado 2 el detalle de orden de despacho
                                Dim sqlUpdDetOrden As String = <sqlExp>
                                                    UPDATE [dbo].[detalle_orden_despacho]
                                                    SET estado = 2
                                                    WHERE iddetalle_solicitud_despacho = <%= IdDetSolicitudDesp %>
                                                               </sqlExp>.Value

                                Datos.consulta_non_queryDeta(sqlUpdDetOrden)

                                ' Poner la orden en estado 2
                                Dim sqlUpdOrden As String = <sqlExp>
                                        UPDATE [dbo].[orden_despacho]
                                        SET estado = 2
                                        WHERE idsolicitud_despacho = (
                                            SELECT TOP 1 solicitud_despacho_idsolicitud_despacho
                                            FROM detalle_solicitud_despacho
                                            WHERE iddetalle_solicitud_despacho = <%= IdDetSolicitudDesp %>
                                        )
                                    </sqlExp>.Value

                                Datos.consulta_non_queryDeta(sqlUpdOrden)

                            End If

                        End If
                        ' fin RECEPCION / DESPACHO
                    Catch ex As Exception
                        MessageBox.Show("ERROR al insertar/actualizar inventario: " & ex.Message, "Inventario")
                    End Try
                End If

SaltarInventario:

                'Mueve Cambio
                Mueve_Camion(846, 1182)

                If MessageBox.Show("Imprimir la Boleta..?", "", MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.Yes Then

                    If LibreriaGeneral.gPrinter = "B" Then
                        Datos.ImprBoleta(Ticket, EstaPesando)
                    Else
                        Datos.Imprime_Ticket(Ticket, EstaPesando)
                    End If
                End If

                Textos()
                SumaTicket()
            End If

            LeerData(grdData)   'Llena Datos Pendientes de Salir
            LeerTemporales(dataSol)

        End If  '//IF  para validar los 

    End Sub

    '-Carga los Tickets pendientes de Cerrar
    Public Sub LeerData(ByRef grdData As DataGridView)
        Dim dv As New DataView

        'Se arma el script para grabar SQL
        Dim StringSql As String = <sqlExp>
                                    SELECT ticketboleta_pes,
		                                        placa_pes,
		                                        SUBSTRING(tipooperacion_pes,1,3)
                                        From pesajes
                                        WHERE ticketboleta_pes IN (Select ticketboleta_pes 
							                                        From pesajes
							                                        GROUP BY ticketboleta_pes
							                                        HAVING count(ticketboleta_pes) = 1
							                                        )
                                        AND baja_pes = 0
                                        AND LTRIM(RTRIM(EstePesaje_pes)) = 'ENTRADA'
                                        ORDER BY CONVERT(int,ticketboleta_pes);        
                                     </sqlExp>.Value

        'Arma Select para Grid
        dv = Datos.consulta_dv(StringSql)

        grdData.DataSource = dv

        grdData.SelectionMode = DataGridViewSelectionMode.FullRowSelect  'Marca toda la Linea en el DataGridView
        grdData.MultiSelect = False  'Solo seleccinar una fila
        grdData.AllowUserToDeleteRows = False
        grdData.ScrollBars = ScrollBars.Vertical

        'Encabezado del Grid
        grdData.Columns(0).HeaderText = "Boletas"
        grdData.Columns(1).HeaderText = "Placa"
        grdData.Columns(2).HeaderText = "Tipo"

        'Ancho de las columnas
        grdData.Columns(0).Width = 75
        grdData.Columns(1).Width = 70
        grdData.Columns(2).Width = 50

        grdData.ReadOnly = True
        'grdData.ScrollBars = ScrollBars.Both

        If dv.Count > 0 Then
            ''Posición Primera Linea
            grdData.Rows(0).Selected = True
            grdData.CurrentCell = grdData.Rows(0).Cells(0)
        End If

    End Sub

    Private Sub grdData_DoubleClick(sender As Object, e As EventArgs) Handles grdData.DoubleClick

        'Limpia todos los datos 
        Textos()
        SumaTicket()

        Dim dRow As DataGridViewSelectedCellCollection = grdData.SelectedCells

        txtPlaca.Text = dRow.Item(1).Value  'Seleciona la Placa
        txtTicket.Text = dRow.Item(0).Value
        txtPlaca.Focus()
    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnRefresca.Click
        'Limpia todos los datos 
        Textos()
        SumaTicket()

        grdData.DataSource = Nothing
        dataSol.DataSource = Nothing

        LeerData(grdData)
        LeerTemporales(dataSol)
    End Sub

#Region "Saldo"
    Private Sub txtPlaca_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPlaca.KeyPress,
                                                                                    txtClase.KeyPress,
                                                                                    txtTipo.KeyPress,
                                                                                    txtTras.KeyPress,
                                                                                    txtBuque.KeyPress,
                                                                                    txtProducto.KeyPress,
                                                                                    txtBL.KeyPress,
                                                                                    txtBodega.KeyPress,
                                                                                    txtPrefijo.KeyPress,
                                                                                    txtContenedor.KeyPress,
                                                                                    txtMedida.KeyPress,
                                                                                    txtTipoCarga.KeyPress,
                                                                                    txtPTaraContenedor.KeyPress

        If e.KeyChar = ChrW(Keys.Enter) Then
            'Posicion en el nuevo Campo
            SendKeys.Send("{TAB}")
            e.Handled = True
        End If
    End Sub

    Private Sub txtNaviera_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) Handles txtNaviera.PreviewKeyDown
        ' Detectar Enter o Tab
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Tab Then

            ' If EstaPesando = "SALIDA" Then
            '    txtBuque.Focus()
            'Else
            btnNuevo.Focus()

            'If e.KeyCode = Keys.Tab Then
            '    SendKeys.Send("{RIGHT}")
            'End If
            'End If
        End If
    End Sub


    Private Sub txtDiceContenedor_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) Handles txtDiceContenedor.PreviewKeyDown
        ' Detectar Enter o Tab
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Tab Then
            txtTipo.Focus()
        End If
    End Sub

    Private Sub btnSalir_Enter(sender As Object, e As EventArgs) Handles btnSalir.Enter
        If btnGrabar.Enabled = False Then
            btnNuevo.Focus()
        End If
    End Sub

    Private Sub txtPesoAraña_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) Handles txtPesoAraña.PreviewKeyDown
        ' Detectar Enter o Tab
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Tab Then
            btnNuevo.Focus()
        End If
    End Sub

    'Tipo:  Cambio BL
    'Fecha: 01/11/2024
    'Programador: SHA
    '------------------------------------------------------------------------------------------
    Private Sub txtBL_Validating(sender As Object, e As EventArgs) Handles txtBL.Validating

        Dim ControlSaldoBl As Double = 0.00
        Dim pMerma As Double = 0.00
        Dim PesoManifestado As Double = 0.00
        Dim PesoMerma As Double = 0.00
        Dim PesoActual As Double = 0.00
        Dim PesoMenor As Double = 0.00
        Dim PesoMayor As Double = 0.00

        If txtBL.Text <> String.Empty Then

            txtDuca.Text = ""
            txtNomConsigna.Text = ""
            txtBL_Manifiesto.Text = "0"
            txtBL_PesoBascula.Text = "0"
            txtBL_Diferencia.Text = "0"

            Try

                'Se arma el script para grabar SQL
                Dim StringSql As String = <sqlExp>
                                         SELECT 
                                            db.Id_dbq,
                                            db.duca_dbq,
                                            db.bl_dbq,
                                            cs.nombre_csg,
                                            db.porcentaje_merma_dbq,
                                            db.manifiesto_dbq,
                                            ISNULL(db.bascula_dbq,0) As bascula_dbq,
                                            ISNULL(db.diferencia_dbq,0) As diferencia_dbq,
                                            db.autorizado_dbq,
                                            db.activo_dbq,
                                            db.consignatario_dbq AS IdConsignatario
                                        FROM [dbo].[Detalle_Buque] db
                                        LEFT JOIN [dbo].[Consignatario] cs ON cs.Id_csg = db.consignatario_dbq 
                                        WHERE db.bl_dbq = '<%= txtBL.Text %>' 
                                          AND db.buque_dbq = '<%= txtBuque.Text %>' 
                                          AND db.producto_dbq = '<%= txtProducto.Text %>' 
                                          AND db.activo_dbq = 'ACTIVO'
                                  </sqlExp>.Value

                dTable = Datos.consulta_reader(StringSql)

                'Setea Campos
                For Each dRow As DataRow In dTable.Rows
                    txtDuca.Text = dRow.Item("duca_dbq").ToString()
                    txtNomConsigna.Text = dRow.Item("nombre_csg").ToString()
                    txtBL.Text = dRow.Item("bl_dbq").ToString()
                    txtBL_Manifiesto.Text = dRow.Item("manifiesto_dbq").ToString()
                    txtBL_PesoBascula.Text = dRow.Item("bascula_dbq").ToString()
                    txtBL_Diferencia.Text = dRow.Item("diferencia_dbq").ToString()
                    pMerma = Convert.ToDouble(dRow.Item("porcentaje_merma_dbq").ToString()) / 100

                    If dTable.Columns.Contains("Id_dbq") AndAlso Not IsDBNull(dRow("Id_dbq")) Then
                        IdDetalleBuque = CInt(dRow("Id_dbq"))
                    Else
                        IdDetalleBuque = 0
                    End If
                    ' id del Consignatario
                    If dTable.Columns.Contains("IdConsignatario") AndAlso Not IsDBNull(dRow("IdConsignatario")) Then
                        IdConsignatario = CInt(dRow("IdConsignatario"))
                    Else
                        IdConsignatario = 0
                    End If
                Next

                If Convert.ToDouble(txtBL_Manifiesto.Text) > 0 Then

                    PesoActual = Math.Round((Convert.ToDouble(txtBL_PesoBascula.Text)), 4)
                    txtBL_PesoBascula.Text = PesoActual.ToString("F3")

                    PesoManifestado = Math.Round(Convert.ToDouble(txtBL_Manifiesto.Text), 4)
                    PesoMerma = Math.Round(Convert.ToDouble(txtBL_Manifiesto.Text) * pMerma, 4)
                    PesoMenor = Math.Round(PesoManifestado - PesoMerma, 4)
                    PesoMayor = Math.Round(PesoManifestado + PesoMerma, 4)

                End If

            Catch ex As Exception

            End Try
        End If
    End Sub


#End Region

#Region "PesajeDll"
    Private Sub ActualizarPeso(ByVal data As String)
        ' Asegurarse de actualizar el Label en el hilo de la interfaz gráfica
        If InvokeRequired Then
            Invoke(New Action(Of String)(AddressOf ActualizarPeso), data)
        Else
            If data.Contains("UL") OrElse data.Contains("US") OrElse data.Contains("ER") Then
            Else
                txtTomaPeso.Text = Regex.Replace(data, "[^\d]", "")
                txtTomaPeso.Refresh()
            End If
        End If
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        ' Detener la báscula al cerrar el formulario
        miBascula.Detener()
    End Sub

#End Region

#Region "Camion"
    Private velocidad As Integer = 10
    'Private inicio As Integer = 720
    Private destino As Integer = 846

    Private Sub Mueve_Camion(_inicio As Integer, _destino As Integer)
        'Dim direccion As String = txtDireccion.Text.ToUpper() ' Obtener el texto en mayúsculas

        PicCamion.Left = _inicio
        destino = _destino
        TimerDerecha.Start() ' Mueve hacia la derecha
    End Sub

    Private Sub TimerDerecha_Tick(sender As Object, e As EventArgs) Handles TimerDerecha.Tick
        If PicCamion.Left < destino Then
            PicCamion.Left += velocidad ' Mueve el camión a la derecha
        Else
            TimerDerecha.Stop() ' Detiene si llega al borde
        End If
    End Sub

#End Region
    Public Sub LeerTemporales(ByRef grid As DataGridView)
        Dim dv As New DataView

        Dim StringSql As String = "

   SELECT 
        od.iddetalle_orden_despacho AS NoOrden,
        CAST(d.iddetalle_solicitud_despacho AS VARCHAR(10)) AS Solicitud,
        c.placa_fca AS Placa,
        cli.nombre_cli AS Cliente,

        dbq.bl_dbq AS BL,

        -- Cantidad en QQ con tu fórmula (kg * 2.2046 / 100)
        CAST(ROUND((ISNULL(d.aprobado, 0) * 2.2046) / 100.0, 0) AS INT) AS CantidadQQ,
        -- Póliza = DUCA + BL
        (dbq.duca_dbq + '-' + dbq.bl_dbq) AS Poliza,
        -- Consignatario
    
     cons.nombre_csg AS Consignatario

    FROM detalle_solicitud_despacho d
    INNER JOIN detalle_orden_despacho od 
        ON od.iddetalle_solicitud_despacho = d.iddetalle_solicitud_despacho

    INNER JOIN fcamiones c
        ON c.id_fca = od.fcamiones_idfcamiones

    INNER JOIN Cliente cli
        ON cli.id_cli = d.cliente_idcliente

    INNER JOIN Detalle_Buque dbq
    ON dbq.Id_dbq = d.iddetalle_buque

    INNER JOIN consignatario cons
    ON cons.id_csg = dbq.consignatario_dbq

    WHERE d.estado_pesaje = 2
    ORDER BY d.iddetalle_solicitud_despacho;
    "

        dv = Datos.consulta_dv(StringSql)

        grid.DataSource = dv

        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        grid.MultiSelect = False
        grid.AllowUserToDeleteRows = False
        grid.ScrollBars = ScrollBars.Both

        grid.Columns(1).Visible = False   ' Solicitud

        ' Encabezados
        grid.Columns(0).HeaderText = "No. Orden"   ' iddetalle_orden_despacho
        grid.Columns(2).HeaderText = "Placa"
        grid.Columns(3).HeaderText = "Empresa"
        grid.Columns(4).HeaderText = "BL"
        grid.Columns(5).HeaderText = "Cantidad qq"
        grid.Columns(6).HeaderText = "Poliza"
        grid.Columns(7).HeaderText = "Consignatario"

        ' Anchos
        grid.Columns(0).Width = 75
        grid.Columns(2).Width = 70
        grid.Columns(3).Width = 95
        grid.Columns(4).Width = 40
        grid.Columns(5).Width = 75
        grid.Columns(6).Width = 90
        grid.Columns(7).Width = 90

        grid.ReadOnly = True

        If dv.Count > 0 Then
            grid.Rows(0).Selected = True
            grid.CurrentCell = grid.Rows(0).Cells(0)
        End If
    End Sub


    Private Sub dataSol_DoubleClick(sender As Object, e As EventArgs) Handles dataSol.DoubleClick
        Textos()
        SumaTicket()

        Dim row As DataGridViewSelectedCellCollection = dataSol.SelectedCells

        If row IsNot Nothing AndAlso row.Count > 0 Then
            Dim idDet As Integer = CInt(row(1).Value)
            txtPlaca.Text = row(2).Value.ToString()

            IdDetSolicitudDesp = idDet

            EsDesdeTemporal = False
            EstaPesando = "ENTRADA"

            ' Cargar toda la información desde el detalle de solicitud
            CargarDesdeDetalleSolicitud(idDet)
            txtTipo.Focus()
        End If
    End Sub

    Private Sub CargarDesdeDetalleSolicitud(ByVal idDet As Integer)
        Try
            ' Consulta que conecta detalle_solicitud_despacho con BL, inventario, producto, buque, bodega y cliente
            Dim StringSql As String = <sqlExp>
             SELECT 
                d.iddetalle_solicitud_despacho,
                d.aprobado,
                d.cliente_idcliente,
                d.solicitud_despacho_idsolicitud_despacho,
                d.iddetalle_buque,
                d.duca AS duca_det,

                inv.idinventario,
                inv.bodega_idbodega,
                inv.consignatario_id,
                inv.cliente_id,
                inv.duca AS duca_inv,

                cli.id_cli,
                cli.nombre_cli,

                dbq.Id_dbq,
                dbq.bl_dbq,
                dbq.duca_dbq,
                dbq.consignatario_dbq,
                dbq.producto_dbq,
                dbq.buque_dbq,             
                dbq.viaje_buque_dbq,
                dbq.fechaviaje_buque_dbq,

                pro.id_pro,
                pro.nombre_pro,
                buq.Id_buq,
                buq.nombre_buq,
                buq.viaje_buq,
                buq.fechaviaje_buq,
                bod.id_bod,
                bod.nombre_bod,
                csg.Id_csg,
                csg.nombre_csg

            FROM detalle_solicitud_despacho d
            LEFT JOIN inventario inv 
                ON inv.idinventario = d.inventario_idinventario
            LEFT JOIN Cliente cli 
                ON cli.id_cli = d.cliente_idcliente
            LEFT JOIN Detalle_Buque dbq
                ON dbq.Id_dbq = d.iddetalle_buque
            LEFT JOIN Producto pro
                ON pro.id_pro = dbq.producto_dbq
            LEFT JOIN buques buq
                ON buq.Id_buq = dbq.buque_dbq   
            LEFT JOIN Bodega bod 
                ON bod.id_bod = inv.bodega_idbodega
            LEFT JOIN Consignatario csg 
                ON csg.Id_csg = dbq.consignatario_dbq
            WHERE d.iddetalle_solicitud_despacho = <%= idDet %>
                                      </sqlExp>.Value

            dTable = Datos.consulta_reader(StringSql)

            If dTable.Rows.Count = 0 Then
                MessageBox.Show("No se encontró información para el detalle de solicitud: " & idDet.ToString(), "Pesaje")
                Exit Sub
            End If

            Dim dRow As DataRow = dTable.Rows(0)
            IdDetSolicitudDesp = CInt(dRow("iddetalle_solicitud_despacho"))

            ' ====== IdDetalleBuque (reusando IdBlBuque) ======
            If Not IsDBNull(dRow("Id_dbq")) Then
                IdDetalleBuque = CInt(dRow("Id_dbq"))
            Else
                IdDetalleBuque = 0
            End If

            ' ====== BL ======
            txtBL.Text = If(IsDBNull(dRow("bl_dbq")), "", dRow("bl_dbq").ToString())

            ' ====== Producto ======
            If Not IsDBNull(dRow("id_pro")) Then
                txtProducto.Text = dRow("id_pro").ToString()
            Else
                txtProducto.Text = ""
            End If

            If Not IsDBNull(dRow("nombre_pro")) Then
                txtNomProducto.Text = dRow("nombre_pro").ToString()
            Else
                txtNomProducto.Text = ""
            End If

            ' ====== Buque ======
            If Not IsDBNull(dRow("Id_buq")) Then
                txtBuque.Text = dRow("Id_buq").ToString()
            Else
                txtBuque.Text = ""
            End If

            If Not IsDBNull(dRow("nombre_buq")) Then
                txtNomBuque.Text = dRow("nombre_buq").ToString()
            Else
                txtNomBuque.Text = ""
            End If

            If Not IsDBNull(dRow("viaje_buq")) Then
                txtViaje.Text = dRow("viaje_buq").ToString()
            Else
                txtViaje.Text = ""
            End If

            If Not IsDBNull(dRow("fechaviaje_buq")) Then
                dFechav.Value = CDate(dRow("fechaviaje_buq"))
            End If

            ' ====== Bodega ======
            If Not IsDBNull(dRow("id_bod")) Then
                txtBodega.Text = dRow("id_bod").ToString()
            Else
                txtBodega.Text = ""
            End If

            If Not IsDBNull(dRow("nombre_bod")) Then
                txtNomBodega.Text = dRow("nombre_bod").ToString()
            Else
                txtNomBodega.Text = ""
            End If

            ' ====== Cliente (Naviera) ======
            If Not IsDBNull(dRow("cliente_idcliente")) Then
                txtNaviera.Text = dRow("cliente_idcliente").ToString()
            Else
                txtNaviera.Text = ""
            End If

            If Not IsDBNull(dRow("nombre_cli")) Then
                txtNomNaviera.Text = dRow("nombre_cli").ToString()
            Else
                txtNomNaviera.Text = ""
            End If

            ' ====== Consignatario (informativo) ======
            If Not IsDBNull(dRow("Id_csg")) Then
                txtNomConsigna.Text = dRow("nombre_csg").ToString()
                IdConsignatario = CInt(dRow("Id_csg"))
            Else
                txtNomConsigna.Text = ""
                IdConsignatario = 0
            End If

            If Not IsDBNull(dRow("duca_dbq")) Then
                txtDuca.Text = dRow("duca_dbq").ToString()
            Else
                txtDuca.Text = ""
            End If


            Dim aprobadoValKg As Decimal = 0D
            If Not IsDBNull(dRow("aprobado")) Then
                aprobadoValKg = CDec(dRow("aprobado"))
            End If

            Dim aprobadoValQQ As Decimal = Math.Round((aprobadoValKg * 2.2046D) / 100D, 0)
            ' Mostrar sin decimales
            txtAPROBADO.Text = aprobadoValQQ.ToString("N0")


            cmbTipoOperacion.Text = "DESPACHO"
            txtTipo.Text = "D"
            EstaPesando = "ENTRADA"

            ' Refrescamos
            txtBL.Refresh()
            txtProducto.Refresh()
            txtBuque.Refresh()
            txtBodega.Refresh()
            txtNaviera.Refresh()
            txtAPROBADO.Refresh()
            Try
                Dim sqlClase As String = "
                    SELECT id_cla, nombreclase_cla, prefijocont_cla
                    FROM clasecarga
                    WHERE Abrevia_cla = 'G'
                "

                Dim dtClase As DataTable = Datos.consulta_reader(sqlClase)

                If dtClase.Rows.Count > 0 Then
                    txtClase.Text = "G"
                    EsContene = dtClase.Rows(0)("prefijocont_cla")
                    cmbClaseCarga.SelectedValue = dtClase.Rows(0)("id_cla")
                    cmbClaseCarga_SelectedIndexChanged(cmbClaseCarga, EventArgs.Empty)
                Else
                    txtClase.Text = "G"
                End If

            Catch ex2 As Exception
                ' No interrumpimos el flujo
            End Try

        Catch ex As Exception
            MessageBox.Show("ERROR al cargar datos desde el detalle de solicitud: " & ex.Message, "Pesaje")
        End Try
    End Sub
    Private Sub CargarAprobadoDesdeDetalle()
        Try
            txtAPROBADO.Text = "0.00"

            If IdDetSolicitudDesp <= 0 Then Exit Sub

            Dim sqlAprobado As String = <sqlExp>
            SELECT aprobado
            FROM detalle_solicitud_despacho
            WHERE iddetalle_solicitud_despacho = <%= IdDetSolicitudDesp %>
                                        </sqlExp>.Value

            Dim dtAprobado As DataTable = Datos.consulta_reader(sqlAprobado)

            If dtAprobado.Rows.Count > 0 Then
                Dim aprobadoKg As Decimal = CDec(dtAprobado.Rows(0)("aprobado"))
                Dim aprobadoQQ As Decimal = Math.Round((aprobadoKg * 2.2046D) / 100D, 0)

                ' Mostrar sin decimales
                txtAPROBADO.Text = aprobadoQQ.ToString("N0")
            End If

        Catch ex As Exception
            txtAPROBADO.Text = "0.00"
        End Try
    End Sub


    Private Function ValidarInventario() As Boolean
        Try
            ' Solo aplica para SALIDA / DESPACHO
            If EstaPesando <> "SALIDA" Then Return True
            If cmbTipoOperacion.Text.Trim().ToUpper() <> "DESPACHO" Then Return True

            ' Si es AUTOMATICO, no validar aquí (el SP lo maneja)
            If cbTraslado.Text.Trim().ToUpper() = "AUTOMATICO" Then Return True

            ' Obtener el peso neto real
            Dim pesoReal As Decimal
            If Not Decimal.TryParse(txtPesoNeto.Text.Replace(",", "").Trim(), pesoReal) Then
                MessageBox.Show("No se pudo obtener el peso neto.", "Error")
                Return False
            End If

            ' WHERE dinámico por BL, Consignatario y Cliente
            Dim whereDetBuque As String = If(IdDetalleBuque = 0, " AND iddetalle_buque IS NULL", " AND iddetalle_buque = " & IdDetalleBuque)
            Dim whereCons As String = " AND consignatario_id = " & IdConsignatario
            Dim whereCli As String = If(String.IsNullOrEmpty(txtNaviera.Text), " AND cliente_id IS NULL", " AND cliente_id = " & txtNaviera.Text)

            ' Buscar inventario
            Dim sql As String =
            "SELECT TOP 1 stock
             FROM inventario
             WHERE bodega_idbodega = '" & txtBodega.Text & "'
             " & whereDetBuque & whereCons & whereCli

            Dim dt As DataTable = Datos.consulta_reader(sql)

            If dt.Rows.Count = 0 Then
                MessageBox.Show(
                "No existe inventario para este despacho." & vbCrLf & "Inventario"
            )
                Return False
            End If

            Dim stockActual As Decimal = CDec(dt.Rows(0)("stock"))

            Dim mermaPermitida As Decimal = stockActual * 0.05D
            Dim maxDespachable As Decimal = stockActual + mermaPermitida

            If pesoReal > maxDespachable Then
                MessageBox.Show(
                        "Peso excede el 5% de merma permitida." & vbCrLf &
                        "Stock actual: " & stockActual.ToString("0.00") & " kg" & vbCrLf &
                        "Merma permitida: " & mermaPermitida.ToString("0.00") & " kg" & vbCrLf &
                        "Máximo permitido: " & maxDespachable.ToString("0.00") & " kg" & vbCrLf &
                        "Peso del camión: " & pesoReal.ToString("0.00") & " kg",
                        "Exceso de tolerancia"
                    )
                Return False
            End If

            Return True


            Return True

        Catch ex As Exception
            MessageBox.Show("Error validando inventario: " & ex.Message)
            Return False
        End Try
    End Function
End Class