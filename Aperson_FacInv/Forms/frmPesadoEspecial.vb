Imports TomaPesoLib
Imports System.IO.Ports
Imports System.Text.RegularExpressions

Public Class frmPesajeEspecial

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
    Private prefijoCor As String = ""
    Dim EsContene As Boolean = False

    Private WithEvents miBascula As Bascula  'Se instancia la DLL C#
#End Region

#Region "Entorno"

    Private Sub frmPesajeEspecial_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Saul
        'LibreriaGeneral.nBascu = "01"

        If Convert.ToInt32(LibreriaGeneral.nBascu) = 0 Then
            MessageBox.Show("La BASCULA no tiene Computadora Asiganda" & vbCrLf & "Comunicarse con Administrador")
            Me.Close()
            End
        Else
            'Inicializar la báscula con el puerto COM adecuado
            miBascula = New Bascula(LibreriaGeneral.gPuertoCOM, LibreriaGeneral.gBitsPorSegundo) ' Asegúrate de poner el puerto correcto
            AddHandler miBascula.DatosRecibidos, AddressOf ActualizarPeso
            miBascula.Iniciar()
            txtTomaPeso.ReadOnly = True

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
            SumaTicket()        'Suma Correla
        End If

    End Sub

    'Muestra Ticket
    Private Sub SumaTicket()
        'Para mostrar el Dato (siempre RECEPCION en pesado especial)
        dTable = Datos.consulta_reader("SELECT prefijo_cor, CAST(nticketboleta_cor + 1 AS VARCHAR(10)) AS 'Corre' FROM correlactu WHERE empresa_cor = " & LibreriaGeneral.idEmpresa & " AND tipo_cor = 'RECEPCION' AND estatus_cor = 'activo'")

        If dTable.Rows.Count > 0 Then
            'Setea Campos
            For Each dRow As DataRow In dTable.Rows
                'Infomrativos
                prefijoCor = dRow.Item("prefijo_cor").ToString.Trim()
                txtTicket.Text = dRow.Item("Corre").ToString
            Next
        End If

    End Sub

    Private Sub Textos()
        'Cabecera
        txtPlaca.Text = ""
        txtNomTransporte.Text = ""
        txtNomTransporte.ReadOnly = True
        txtNomTransporte.ForeColor = Color.Black         ' Color del texto
        txtNomTransporte.BackColor = Color.LightGray     ' (Opcional) Cambiar el color de fondo para simular un estado deshabilitado

        'Cabecera
        txtPlaca.Text = ""
        cmbClaseCarga.Text = ""

        'Contenedor
        txtPrefijo.Text = ""
        txtContenedor.Text = ""
        txtMedida.Text = ""
        txtTipoCarga.Text = ""
        txtDiceContenedor.Text = ""
        EsContene = False

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
        txtPManifestado.Text = "0.00"
        txtPTaraVehiculo.Text = "0.00"
        txtPTaraContenedor.Text = "0.00"
        txtPesoBruto.Text = "0.00"
        txtPesoAraña.Text = "0.00"
        txtPesoNeto.Text = "0.00"
        TxtQuintales.Text = "0"
        txtTomaPeso.Text = ""

        'BL
        txtBL.Text = ""
        txtDuca.Text = ""
        txtNomConsigna.Text = ""
        txtBL_Manifiesto.Text = "0"
        txtBL_PesoBascula.Text = "0"
        txtBL_Diferencia.Text = "0"

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


#End Region

#Region "ValidaBusquedas"
    Private Sub txtProducto_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtProducto.Validating
        Try
            'Nombre Proveedor
            dTable = Datos.consulta_reader("SELECT nombre_pro FROM dbo.Producto WHERE id_pro='" & txtProducto.Text & "'")
            txtNomProducto.Text = dTable.Rows.Item(0).Item("nombre_pro")
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtBuque_Validating(sender As Object, e As EventArgs) Handles txtBuque.Validating

        Try
            'Nombre Buque
            dTable = Datos.consulta_reader("SELECT  fechaviaje_buq fecha, viaje_buq, nombre_buq FROM dbo.Buques WHERE Id_buq= '" & txtBuque.Text & "'")
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
                'If EstaPesando = "ENTRADA" Then
                '    btnNuevo.Focus()
                'Else
                '    'txtBodega.Focus()
                'End If
            End If

            ''Prueba1 = ENTRADA
            'txtTomaPeso.Text = "15000.00"
            PicCamion.Visible = True
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtClase_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtClase.Validating
        Try
            'If EstaPesando = "ENTRADA" Then
            'Nombre Proveedor
            txtClase.Text = txtClase.Text.ToUpper
            Dim x As String = "SELECT nombreclase_cla, prefijocont_cla FROM dbo.clasecarga WHERE Abrevia_cla='" & txtClase.Text.Trim & "'"
            dTable = Datos.consulta_reader(x)

            EsContene = dTable.Rows.Item(0).Item("prefijocont_cla").ToString
            cmbClaseCarga.Text = dTable.Rows.Item(0).Item("nombreclase_cla").ToString

            'End If

            If EsContene = False Then           'No Es contenedor
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

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub txtTipo_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtTipo.Validating

        If EstaPesando = "ENTRADA" Then

            txtTipo.Text = txtTipo.Text.ToUpper

            If txtTipo.Text = "D" Then
                cmbTipoOperacion.Text = "DESPACHO"

                If EstaPesando = "ENTRADA" Then
                    txtPTaraVehiculo.Enabled = True
                Else
                    txtPTaraVehiculo.Enabled = True
                    txtPesoBruto.Enabled = True
                End If

            Else
                If txtTipo.Text = "R" Then
                    cmbTipoOperacion.Text = "RECEPCION"

                    If EstaPesando = "ENTRADA" Then
                        txtPesoBruto.Enabled = True
                    Else
                        txtPTaraVehiculo.Enabled = True
                        txtPesoBruto.Enabled = True
                    End If
                End If
            End If
        End If

        If txtTipo.Text = String.Empty Then
            txtTipo.Focus()
        Else
            If txtBuque.Enabled Then
                txtBuque.Focus()
            Else
                txtProducto.Focus()
            End If
        End If
    End Sub

    Private Sub txtBodega_Validating(sender As Object, e As EventArgs) Handles txtBodega.Validating
        Try
            'Nombre Proveedor
            dTable = Datos.consulta_reader("SELECT nombre_bod FROM dbo.Bodega WHERE id_bod='" & txtBodega.Text & "'")
            txtNomBodega.Text = dTable.Rows.Item(0).Item("nombre_bod")
        Catch ex As Exception

        End Try
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

            Dim xx = txtPlaca.Text.Trim

            If xx <> String.Empty Then

                'Mayuscula
                txtPlaca.Text = txtPlaca.Text.ToUpper
                dTable = Datos.consulta_readerp("SELECT placa_fca FROM dbo.fcamiones WHERE rtrim(ltrim(placa_fca))='" & txtPlaca.Text.Trim & "'")

                If dTable.Rows.Count > 0 Then
                    txtNomTransporte.Text = "Existe: " & dTable.Rows.Item(0).Item("placa_fca").ToString()
                    txtNomTransporte.ForeColor = Color.Black         ' Color del texto
                Else
                    txtNomTransporte.Text = "Se Grabara como nueva Placa..!"
                    txtNomTransporte.ForeColor = Color.Red         ' Color del texto
                End If

                txtNomTransporte.Refresh()

                ControlEntrada()
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

    Private Sub txtPlaca_KeyDown(sender As Object, e As KeyEventArgs) Handles txtPlaca.KeyDown
        Select Case e.KeyCode
            Case Keys.F2
                buscaTransporte()
        End Select
    End Sub

    Private Sub txtBodega_KeyDown(sender As Object, e As KeyEventArgs) Handles txtBodega.KeyDown
        Select Case e.KeyCode
            Case Keys.F2
                BuscaBodega()
        End Select
    End Sub

#End Region

#Region "Grabar Y Tomar Peso"

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click

        Dim SadoPeso As Boolean = False
        Dim PesoQuintal As Double = 0.00
        btnGrabar.Enabled = False

        'Valida Campos
        If ValidaCampos() Then

            Try

                'Dim frmPeso As New frmTomaPesaje
                'frmPeso.ShowDialog()

                'Dim TomoPeso As String = ""
                'TomoPeso = frmPeso.Peso

                'If TomoPeso = "" Or TomoPeso = "ERROR" Or TomoPeso = "Fallo" Then
                '    btnGrabar.Enabled = False
                '    btnGrabar.Refresh()
                '    'btnGrabar.Focus()
                'Else
                txtPManifestado.Text = txtTomaPeso.Text
                PesoTaraCont = txtPTaraContenedor.Text
                PesoArana = txtPesoAraña.Text

                PesoManifestado = Val(txtTomaPeso.Text)

                If txtPManifestado.Text <> "Fallo" Or txtPManifestado.Text <> "" Or txtPManifestado.Text <> "ERROR" Then
                    PesoManifestado = Val(txtTomaPeso.Text)

                    Mueve_Camion(531, 657)
                End If

                ''----------------------------------------------------------------------
                '' Mostrar un cuadro de entrada para que el usuario ingrese su nombre
                'Dim peso As String = InputBox("Ingrese Cantidad:", "Cantidad Peso", "0.00")

                '' Verificar si se ingresó un valor
                'If Not String.IsNullOrEmpty(peso) Then

                '    txtPManifestado.Text = peso

                '    PesoTaraCont = txtPTaraContenedor.Text
                '    PesoArana = txtPesoAraña.Text
                '    'PesoManifestado = Val(frmPeso.Peso)  
                '    PesoManifestado = Val(peso)
                'Else
                '    MessageBox.Show("No ingresaste ningún peso", "Advertencia")
                'End If
                ''---------------------------------------------------------------------


            Catch ex As Exception
                txtPManifestado.Text = "0.00"


                PesoTaraCont = txtPTaraContenedor.Text
                PesoArana = txtPesoAraña.Text
                PesoManifestado = Val("0.0")

            End Try

            If PesoManifestado > 0 Then
                'PesoTaraCont = txtPTaraContenedor.Text
                'PesoArana = txtPesoAraña.Text
                'PesoManifestado = Val("100.00")

                SadoPeso = True


                If cmbTipoOperacion.Text.Trim = "DESPACHO" Then

                    '-->Peso Bruto = Lo que tomo el LECTOR
                    '-->Pero Neto =  Peso Bruto - (Tara + TaraCont + PesoAraña)
                    '-->Peso VGM = Tara + PesoNeto
                    txtPesoBruto.Text = PesoManifestado
                    txtPesoNeto.Text = Convert.ToDouble(txtPesoBruto.Text) - (Convert.ToDouble(txtPTaraVehiculo.Text) + Convert.ToDouble(txtPTaraContenedor.Text) + Convert.ToDouble(txtPesoAraña.Text))
                    TxtPesoVGM.Text = Convert.ToDouble(txtPesoNeto.Text) + Convert.ToDouble(txtPTaraContenedor.Text)

                    If txtBL.Text IsNot String.Empty Then
                        PesoQuintal = Math.Round((Convert.ToDouble(txtPesoNeto.Text) * 2.2046) / 100, 5)
                        TxtQuintales.Text = PesoQuintal.ToString("F3")
                    End If

                Else  'EXPORT

                    txtPTaraVehiculo.Text = PesoManifestado
                    txtPesoNeto.Text = Convert.ToDouble(txtPesoBruto.Text) - (Convert.ToDouble(txtPTaraVehiculo.Text) + Convert.ToDouble(txtPTaraContenedor.Text) + Convert.ToDouble(txtPesoAraña.Text))
                    TxtPesoVGM.Text = Convert.ToDouble(txtPesoNeto.Text) + Convert.ToDouble(txtPTaraContenedor.Text)

                    If txtBL.Text IsNot String.Empty Then
                        PesoQuintal = Math.Round((Convert.ToDouble(txtPesoNeto.Text) * 2.2046) / 100, 5)
                        TxtQuintales.Text = PesoQuintal.ToString("F3")
                    End If
                End If

                ''Verifica Saldo Peso
                'If SadoPeso Then
                '    btnGrabar.Enabled = True
                'Else
                '    btnGrabar.Enabled = False
                '    txtNaviera.Focus()
                'End If

            Else
                SadoPeso = False
                btnGrabar.Enabled = True
            End If

            'Verifica Saldo Peso
            If SadoPeso Then
                btnGrabar.Enabled = True

                'Else
                '   btnGrabar.Enabled = False
                btnGrabar.Focus()
            End If
        End If
    End Sub


    Private Function ValidaCampos() As Boolean
        Dim Si As Boolean = False

        'Solictados
        txtProducto_Validating(txtProducto.Text, Nothing)
        txtBuque_Validating(txtBuque.Text, Nothing)
        txtBodega_Validating(txtBodega.Text, Nothing)
        txtNaviera_Validating(txtNaviera.Text, Nothing)

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
            MessageBox.Show("ERROR: El Naviera No puede ser Blanco")
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
        Grabar()
    End Sub


    Private Sub cmbClaseCarga_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbClaseCarga.SelectedIndexChanged

        If EsContene = False Then           'No Es contenedor

            'Contenedor
            txtPrefijo.Text = ""
            txtContenedor.Text = ""
            txtMedida.Text = ""
            txtTipoCarga.Text = ""
            txtDiceContenedor.Text = ""

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

    Private Sub ControlEntrada()
        Try

            'Para mostrar el Dato
            Dim StringSqlE As String = <sqlExp>
                                              SELECT *  From pesajes
                                                     WHERE ticketboleta_pes IN (Select ticketboleta_pes 
    					                                             From pesajes
    			                                                      GROUP BY ticketboleta_pes
    					                                              HAVING count(ticketboleta_pes) = 1
    			                                                     )
                                                  AND RTRIM(LTRIM(placa_pes)) = '<%= txtPlaca.Text %>' AND EstePesaje_pes = 'ENTRADA' AND LTRIM(RTRIM(cierre_pes)) ='NO' AND baja_pes = 0
                                     </sqlExp>.Value


            dTable = Datos.consulta_reader(StringSqlE)

            If dTable.Rows.Count > 0 Then

                'Setea Campos
                For Each dRow As DataRow In dTable.Rows
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
                    txtViaje.Text = dRow.Item("idcla_pes").ToString

                    txtBodega.Text = dRow.Item("bodegabuq_pes").ToString
                    txtBodega_Validating(txtBodega.Text, Nothing)

                    txtNaviera.Text = dRow.Item("idcli_pes").ToString
                    txtNaviera_Validating(txtNaviera.Text, Nothing)

                    txtBL.Text = dRow.Item("blbuq_pes").ToString.Trim
                    txtBL_Validating(txtBL.Text, Nothing)

                    'Pesaje   
                    If dRow.Item("TipoOperacion_pes").ToString = "DESPACHO" Then

                        If dRow.Item("EstePesaje_pes").ToString = "ENTRADA" Then
                            txtPTaraVehiculo.Text = dRow.Item("PesoTara_pes").ToString
                            txtPesoNeto.Text = dRow.Item("PesoTara_pes").ToString
                        Else
                            txtPesoBruto.Text = dRow.Item("PesoBruto_pes").ToString
                            txtPesoNeto.Text = Convert.ToDouble(txtPesoBruto.Text) - Convert.ToDouble(txtPTaraVehiculo.Text)

                            If txtBL.Text IsNot String.Empty Then
                                TxtQuintales.Text = Format((Convert.ToDouble(txtPesoNeto.Text) * 2.46) / 100, "###,###,###")
                            End If
                        End If

                    Else  'EXPORT

                        If dRow.Item("EstePesaje_pes").ToString = "ENTRADA" Then
                            txtPesoBruto.Text = dRow.Item("PesoBruto_pes").ToString

                            txtPesoNeto.Text = Convert.ToDouble(txtPesoBruto.Text) - Convert.ToDouble(txtPTaraContenedor.Text)
                        Else
                            txtPTaraVehiculo.Text = dRow.Item("PesoTara_pes").ToString
                            txtPesoNeto.Text = Convert.ToDouble(txtPesoBruto.Text) - Convert.ToDouble(txtPTaraVehiculo.Text)

                            If txtBL.Text IsNot String.Empty Then
                                TxtQuintales.Text = Format((Convert.ToDouble(txtPesoNeto.Text) * 2.46) / 100, "###,###,###")
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

                Next

                EstaPesando = "SALIDA"
            Else

                EstaPesando = "ENTRADA"
            End If

        Catch ex As Exception
            MessageBox.Show("ERROR: " & ex.Message, "Comprobar tabla * Producto")
        End Try
    End Sub
#End Region

    Private Sub Grabar()

        Dim host As Integer
        Dim Periodo As Integer
        Dim Ticket As String = ""
        Dim pActivo As Integer = "0"

        Dim Bascula As String = LibreriaGeneral.nBascu
        Dim Usuario As String = LibreriaGeneral.usuario
        Dim fechahora As DateTime = DateTime.Now
        Dim StringSql As String
        Dim Paso As String = ""
        Dim IdCorrela As String = "0"
        Dim fechahoraFormat As String = fechahora.ToString("yyyyMMdd H:mm:ss")
        Dim vfechaFormat As String = dFechav.Value.ToString("yyyyMMdd")
        Dim vFechaEntrada As String = dFechaEntrada.Value.ToString("yyyyMMdd H:mm:ss")


        Try

            'Suma Correlativo
            dTable = Datos.consulta_reader("SELECT MAX(id_pes)+1 rMax FROM [dbo].[pesajes]")

            If dTable.Rows.Count > 0 Then
                'Setea Campos
                For Each dRow As DataRow In dTable.Rows
                    'Infomrativos
                    IdCorrela = dRow.Item("rMax").ToString
                Next
            Else
                IdCorrela = "1"
            End If

            'Graga Entrada
            SumaTicket()
            Ticket = prefijoCor & txtTicket.Text

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
                                       ,[empresa_pes])
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
                                       ,'<%= "ENTRADA" %>'
                                       ,<%= pActivo %>
                                       ,'<%= txtBL.Text %>'
                                       ,'<%= LibreriaGeneral.gEmpresa %>')
                                </SqlString>

            Try

                'LLamamos la rutina para grabar
                Datos.consulta_non_query(StringSql)

                Paso = "Si"
            Catch ex As Exception
                MessageBox.Show("ERROR: " & ex.Message, "Comprobar tabla * Pesaje")
                Paso = "No"
            End Try


            If Paso = "Si" Then
                'Suma Correlativo
                dTable = Datos.consulta_reader("SELECT MAX(id_pes)+1 rMax FROM [dbo].[pesajes]")

                If dTable.Rows.Count > 0 Then
                    'Setea Campos
                    For Each dRow As DataRow In dTable.Rows
                        'Infomrativos
                        IdCorrela = dRow.Item("rMax").ToString
                    Next
                Else
                    IdCorrela = "1"
                End If


                'Ingresa SALIDA
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
                                       ,[empresa_pes])
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
                                       ,'<%= Bascula %>'
                                       ,'<%= Usuario %>'
                                       ,'<%= fechahoraFormat %>'
                                       ,'<%= fechahoraFormat %>'
                                       ,'<%= "CIERRE" %>'
                                       ,'<%= Usuario %>'
                                       ,'<%= fechahoraFormat %>'
                                       ,'<%= "SALIDA" %>'
                                       ,<%= pActivo %>
                                       ,'<%= txtBL.Text %>'
                                       ,'<%= LibreriaGeneral.gEmpresa %>')
                                </SqlString>


                Try

                    'LLamamos la rutina para grabar
                    Datos.consulta_non_query(StringSql)

                    Paso = "Si"
                Catch ex As Exception
                    MessageBox.Show("ERROR: " & ex.Message, "Comprobar tabla * Pesaje")
                    Paso = "No"
                End Try

            End If

        Catch ex As Exception
            MessageBox.Show("ERROR: " & ex.Message, "Comprobar tabla * Pesaje")
            Paso = "No"
        End Try

        If EstaPesando = "ENTRADA" And Paso = "Si" Then
            'Para mostrar el Dato
            'Datos.consulta_non_queryDeta("UPDATE correlactu SET nticketboleta_cor = (SELECT REPLICATE('0', 10-LEN(CAST(nticketboleta_cor + 1 AS VARCHAR(10))))+CAST(nticketboleta_cor + 1 AS VARCHAR(10)) AS 'X' FROM correlactu)")

            Datos.consulta_non_queryDeta("UPDATE correlactu SET nticketboleta_cor = (SELECT CAST(nticketboleta_cor + 1 AS VARCHAR(10)) AS 'X' FROM correlactu WHERE empresa_cor = " & LibreriaGeneral.idEmpresa & " AND tipo_cor = 'RECEPCION' AND estatus_cor = 'activo') WHERE empresa_cor = " & LibreriaGeneral.idEmpresa & " AND tipo_cor = 'RECEPCION' AND estatus_cor = 'activo'")

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
                                                                [transporte_fca])
                                                     VALUES('<%= txtId %>', '<%= txtPlaca.Text %>',
                                                          '<%= "" %>',
                                                          '<%= "" %>',
                                                          '<%= "0" %>',
                                                          '<%= "ACTIVO" %>',
                                                          '<%= "0" %>')
                                     </sqlExp>.Value

                'LLamamos la rutina para grabar
                Datos.consulta_non_queryDeta(StringSql4)

            End If

        Else

            'Se arma el script para grabar SQL 
            'y Actuliza Saldo de Bascula y Diferencia en BL
            Dim StringBlSql As String = <sqlExp>
                                     UPDATE [dbo].[Detalle_Buque] SET 
                                                                 [bascula_dbq]  = <%= txtBL_PesoBascula.Text.Replace(",", "") %>,
                                                                 [diferencia_dbq] = <%= txtBL_Diferencia.Text.Replace(",", "") %>
                                       WHERE bl_dbq = '<%= txtBL.Text %>' 
                                          AND buque_dbq = '<%= txtBuque.Text %>' 
                                          AND producto_dbq = '<%= txtProducto.Text %>' 
                                  </sqlExp>.Value

            'LLamamos la rutina para grabar
            Datos.consulta_non_queryDeta(StringBlSql)

        End If

        If Paso = "Si" Then

            'Mueve Cambio
            Mueve_Camion(657, 998)


            If MessageBox.Show("Imprimir la Boleta..?", "", MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.Yes Then

                If LibreriaGeneral.gPrinter = "B" Then
                    Datos.ImprBoleta(Ticket, "SALIDA")
                Else
                    Datos.Imprime_Ticket(Ticket, "SALIDA")
                End If
            End If

            Textos()
        End If

    End Sub

    Private Sub txtPlaca_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPlaca.KeyPress,
                                                                                    txtClase.KeyPress,
                                                                                    txtTipo.KeyPress,
                                                                                    txtBuque.KeyPress,
                                                                                    txtViaje.KeyPress,
                                                                                    txtProducto.KeyPress,
                                                                                    txtBL.KeyPress,
                                                                                    txtBodega.KeyPress,
                                                                                    txtPrefijo.KeyPress,
                                                                                    txtContenedor.KeyPress,
                                                                                    txtMedida.KeyPress,
                                                                                    txtTipoCarga.KeyPress,
                                                                                    txtPTaraContenedor.KeyPress, txtNaviera.KeyPress


        If e.KeyChar = ChrW(Keys.Enter) Then
            'Posicion en el nuevo Campo
            SendKeys.Send("{TAB}")
            e.Handled = True
        End If
    End Sub

    Private Sub txtNaviera_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) Handles txtNaviera.PreviewKeyDown
        ' Detectar Enter o Tab
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Tab Then

            'If EstaPesando = "SALIDA" Then
            'If txtBodega.Text.ToString.Trim <> "0" AndAlso txtBodega.Text.ToString.Trim <> "" Then
            '    btnNuevo.Focus()

            '    If e.KeyCode = Keys.Tab Then
            '        SendKeys.Send("{RIGHT}")
            '    End If

            'Else
            '    txtBodega.Focus()
            'End If
            'Else
            btnNuevo.Focus()

            '    If e.KeyCode = Keys.Tab Then
            '        SendKeys.Send("{RIGHT}")
            '    End If
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
                                         SELECT [duca_dbq],
                                                [bl_dbq],
                                                cs.nombre_csg,
                                                [porcentaje_merma_dbq],
                                                [manifiesto_dbq],
                                                ISNULL([bascula_dbq],0) As [bascula_dbq],
                                                ISNULL([diferencia_dbq],0) As [diferencia_dbq],
                                                [autorizado_dbq],
                                                [activo_dbq]
                                          FROM [dbo].[Detalle_Buque] db
							              LEFT JOIN [dbo].[buques] bs ON bs.Id_buq = db.buque_dbq
							              LEFT JOIN [dbo].[Consignatario] cs ON cs.Id_csg = db.consignatario_dbq 
							              LEFT JOIN [dbo].[Producto] pd ON pd.Id_pro = db.producto_dbq
							              WHERE bl_dbq = '<%= txtBL.Text %>' 
                                          AND buque_dbq = '<%= txtBuque.Text %>' 
                                          AND producto_dbq = '<%= txtProducto.Text %>' 
                                          AND activo_dbq = 'ACTIVO'
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
                Next

                If Convert.ToDouble(txtBL_Manifiesto.Text) > 0 Or Convert.ToDouble(txtBL_Diferencia.Text) > 0 Then

                    PesoActual = Math.Round((Convert.ToDouble(txtBL_PesoBascula.Text)), 4)
                    txtBL_PesoBascula.Text = PesoActual.ToString("F3")

                    PesoManifestado = Math.Round(Convert.ToDouble(txtBL_Manifiesto.Text), 4)
                    PesoMerma = Math.Round(Convert.ToDouble(txtBL_Manifiesto.Text) * pMerma, 4)
                    PesoMenor = Math.Round(PesoManifestado - PesoMerma, 4)
                    PesoMayor = Math.Round(PesoManifestado + PesoMerma, 4)

                    'If PesoActual < PesoMenor Then
                    '    txtBL_Diferencia.BackColor = SystemColors.Control
                    'ElseIf PesoActual >= PesoMenor And PesoActual < PesoManifestado Then
                    '    txtBL_Diferencia.BackColor = Color.LimeGreen
                    'ElseIf PesoActual <= PesoMayor Then
                    '    txtBL_Diferencia.BackColor = Color.Yellow
                    'ElseIf PesoActual > PesoMayor Then
                    '    txtBL_Diferencia.BackColor = Color.Red

                    '    MessageBox.Show("ERROR: El Pesaje llego a su Limite!!")
                    '    txtNaviera.Focus()
                    'End If
                End If

            Catch ex As Exception

            End Try
        End If
    End Sub

#Region "PesajeDll"
    Private Sub ActualizarPeso(ByVal data As String)
        ' Asegurarse de actualizar el Label en el hilo de la interfaz gráfica
        If InvokeRequired Then
            Invoke(New Action(Of String)(AddressOf ActualizarPeso), data)
        Else
            If data.Contains("UL") OrElse data.Contains("US") OrElse data.Contains("ER") Then
            Else
                txtTomaPeso.Text = Regex.Replace(data, "[^\d]", "")
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


End Class