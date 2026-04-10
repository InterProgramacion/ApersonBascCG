Public Class frmPesajeModifica

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
    Dim PesoNetoHis As Double = 0.00
    Dim ProductoHis As String = ""
    Dim BlHis As String = ""

#End Region

#Region "Entorno"

    Private Sub frmPesajeModifica_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Saul
        'LibreriaGeneral.nBascu = "01"

        If Convert.ToInt32(LibreriaGeneral.nBascu) = 0 Then
            MessageBox.Show("La BASCULA no tiene Computadora Asiganda" & vbCrLf & "Comunicarse con Administrador")
            Me.Close()
            End
        Else

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
            Datos.cargar_combo(cmbClaseCarga, _
                               "Select NombreClase_cla, id_cla From [dbo].[clasecarga] Order by 1", _
                               "id_cla", "NombreClase_cla")

            Textos()            'Limpia Textos

        End If

    End Sub

    'Muestra Ticket
    Private Sub SumaTicket()
        'Para mostrar el Dato
        dTable = Datos.consulta_reader("SELECT CAST(nticketboleta_cor + 1 AS VARCHAR(10)) AS 'Corre' FROM correlactu WHERE empresa_cor = " & LibreriaGeneral.idEmpresa & " AND tipo_cor = 'RECEPCION' AND estatus_cor = 'activo'")

        If dTable.Rows.Count > 0 Then
            'Setea Campos
            For Each dRow As DataRow In dTable.Rows
                'Infomrativos
                txtTicket.Text = dRow.Item("Corre").ToString
            Next
        End If

    End Sub

    Private Sub Textos()
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
        TxtQuintales.Text = "0.00"

        'BL
        txtBL.Text = ""
        txtDuca.Text = ""
        txtNomConsigna.Text = ""
        txtBL_Manifiesto.Text = "0"
        txtBL_PesoBascula.Text = "0"
        txtBL_Diferencia.Text = "0"

        'Pesaje
        txtPManifestado.Enabled = True
        txtPTaraContenedor.Enabled = True
        txtPesoAraña.Enabled = True
        TxtQuintales.Enabled = False


        txtPTaraVehiculo.Enabled = False
        txtPesoBruto.Enabled = False
        txtPesoNeto.Enabled = False

        ''Se posiciona para Iniciar 
        'txtPlaca.Focus()
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

        stringSQL = "SELECT placa_fca As Codigo, chasis_fca As Nombre, color_fca FROM [dbo].[fcamiones]"

        'Script para Filtrar el Grid
        Dim stringSqlFiltro As String = "SELECT placa_fca As Codigo, chasis_fca As Nombre FROM [dbo].[fcamiones] WHERE placa_fca "

        Dim frmBuscar As New BuscaCodigo(stringSQL, {"Placa", "Chasis", "Color", "150", "175", "200"}, 3, stringSqlFiltro, "Lista Camiones")
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
    Private Sub txtProducto_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs)
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
            dTable = Datos.consulta_reader("SELECT  fechaviaje_buq fecha, viaje_buq, nombre_buq FROM dbo.Buques WHERE Id_buq='" & txtBuque.Text & "'")
            txtNomBuque.Text = dTable.Rows.Item(0).Item("nombre_buq")
            txtViaje.Text = dTable.Rows.Item(0).Item("viaje_buq")
            dFechav.Value = dTable.Rows.Item(0).Item("fecha")

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
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtClase_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtClase.Validating
        Try
            If EstaPesando = "ENTRADA" Then
                'Nombre Proveedor
                txtClase.Text = txtClase.Text.ToUpper
                Dim x As String = "SELECT nombreclase_cla, prefijocont_cla FROM dbo.clasecarga WHERE Abrevia_cla='" & txtClase.Text.Trim & "'"
                dTable = Datos.consulta_reader(x)

                EsContene = dTable.Rows.Item(0).Item("prefijocont_cla").ToString
                cmbClaseCarga.Text = dTable.Rows.Item(0).Item("nombreclase_cla").ToString

            End If

            If EsContene = False Then           'No Es contenedor

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
                txtBL.Enabled = True
                txtBodega.Enabled = True
            End If
        Catch ex As Exception

        End Try

    End Sub
#End Region


    Private Sub txtPlaca_Validating(sender As Object, e As EventArgs) Handles txtPlaca.Validating

        Try
            If txtPlaca.Text.Trim <> String.Empty Then

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
                'ControlEntrada()
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
            Case e.KeyCode.Equals(Keys.Down)

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

    Private Sub btnGrabar_Click(sender As Object, e As EventArgs) Handles btnGrabar.Click
        Grabar()
    End Sub


    Private Sub cmbClaseCarga_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbClaseCarga.SelectedIndexChanged

        If txtContenedor.Text.Trim <> String.Empty Then           'No Es contenedor

            'Contenedor
            txtPrefijo.Enabled = True
            txtContenedor.Enabled = True
            txtMedida.Enabled = True
            txtTipoCarga.Enabled = True
            txtDiceContenedor.Enabled = True
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

            Dim PesoQuintal As Double

            'Para mostrar el Dato
            dTable = Datos.consulta_reader("select * from pesajes WHERE SUBSTRING(ticketboleta_pes, PATINDEX('%[0-9]%', ticketboleta_pes + '0'), LEN(ticketboleta_pes)) = '" & txtTicket.Text.Trim & "' AND EstePesaje_pes = '" & EstaPesando & "' AND baja_pes = '0'")

            If dTable.Rows.Count > 0 Then

                'Setea Campos
                For Each dRow As DataRow In dTable.Rows
                    'Infomrativos
                    txtBasculaEntrada.Text = dRow.Item("Idbas_entrada_pes").ToString
                    dFechaEntrada.Value = dRow.Item("fechahora_entrada_pes")
                    hEntrada.Value = dRow.Item("fechahora_entrada_pes")
                    txtOperadorEntrada.Text = dRow.Item("idusu_entrada_pes").ToString

                    'Pesaje
                    txtPlaca.Text = dRow.Item("placa_pes").ToString
                    txtPTaraContenedor.Text = dRow.Item("pesotaracont_pes").ToString

                    'Contenedor
                    txtPrefijo.Text = dRow.Item("prefijocont_pes").ToString
                    txtContenedor.Text = dRow.Item("numidentcont_pes").ToString
                    txtMedida.Text = IIf(dRow.Item("tamanocon_pes").ToString = "0", "", dRow.Item("tamanocon_pes").ToString)
                    txtTipoCarga.Text = dRow.Item("tipocarga_pes").ToString
                    txtDiceContenedor.Text = dRow.Item("diceContener_pes").ToString


                    'Solictados
                    txtProducto.Text = dRow.Item("idpro_pes").ToString
                    txtProducto_Validating(txtProducto.Text, Nothing)
                    ProductoHis = txtProducto.Text


                    cmbTipoOperacion.Text = dRow.Item("tipooperacion_pes").ToString
                    txtBuque.Text = dRow.Item("idbuq_pes").ToString
                    txtBuque_Validating(txtBuque.Text, Nothing)
                    txtViaje.Text = dRow.Item("IdvBuq_pes").ToString

                    txtBodega.Text = dRow.Item("bodegabuq_pes").ToString
                    txtBodega_Validating(txtBodega.Text, Nothing)

                    txtNaviera.Text = dRow.Item("idcli_pes").ToString
                    txtNaviera_Validating(txtNaviera.Text, Nothing)

                    txtBL.Text = dRow.Item("blbuq_pes").ToString.Trim
                    txtBL_Validating(txtBL.Text, Nothing)
                    BlHis = txtBL.Text
                    txtBL.Enabled = True

                    'Pesaje   
                    If dRow.Item("TipoOperacion_pes").ToString.Trim = "DESPACHO" Then

                        If dRow.Item("EstePesaje_pes").ToString.Trim = "ENTRADA" Then

                            If dRow.Item("PesoTara_pes").ToString = "0.000" Then
                                txtPTaraVehiculo.Text = "0.000"
                            Else
                                txtPTaraVehiculo.Text = Format(Convert.ToDouble(dRow.Item("PesoTara_pes").ToString), "###,###,###,###")
                            End If

                            txtPesoNeto.Text = Format(Convert.ToDouble(txtPTaraVehiculo.Text), "###,###,###,###")

                            'Habilita Campos
                            txtPTaraVehiculo.Enabled = True
                            txtPesoBruto.Enabled = True
                            txtPesoNeto.Enabled = False
                            TxtPesoVGM.Enabled = False
                            txtBL.Enabled = False
                        Else

                            txtPTaraVehiculo.Text = Format(Convert.ToDouble(dRow.Item("PesoTara_pes").ToString), "###,###,###,###")
                            txtPesoBruto.Text = Format(Convert.ToDouble(dRow.Item("PesoBruto_pes").ToString), "###,###,###,###")
                            txtPesoNeto.Text = Format(Convert.ToDouble(txtPesoBruto.Text) - (Convert.ToDouble(txtPTaraVehiculo.Text) + Convert.ToDouble(txtPTaraContenedor.Text) + Convert.ToDouble(txtPesoAraña.Text)), "###,###,###,###")
                            TxtPesoVGM.Text = Format(Convert.ToDouble(txtPesoNeto.Text) + Convert.ToDouble(txtPTaraContenedor.Text), "###,###,###,###")

                            If txtBL.Text IsNot String.Empty Then
                                PesoQuintal = Math.Round((Convert.ToDouble(txtPesoNeto.Text) * 2.2046) / 100, 5)
                                TxtQuintales.Text = PesoQuintal.ToString("F3")
                            End If

                            'Habilita Campos
                            txtPesoBruto.Enabled = True
                            txtPTaraVehiculo.Enabled = True
                            txtPesoNeto.Enabled = False
                            TxtPesoVGM.Enabled = False

                            'Para el Historial de Peso
                            PesoNetoHis = Convert.ToDouble(txtPesoNeto.Text)

                        End If

                    Else  'EXPORT

                        txtBL.Enabled = False

                        If dRow.Item("EstePesaje_pes").ToString.Trim = "ENTRADA" Then

                            If dRow.Item("PesoTara_pes").ToString = "0.000" Then
                                txtPTaraVehiculo.Text = "0.000"
                            Else
                                txtPTaraVehiculo.Text = Format(Convert.ToDouble(dRow.Item("PesoTara_pes").ToString), "###,###,###,###")
                            End If

                            If dRow.Item("PesoBruto_pes").ToString = "0.000" Then
                                txtPesoBruto.Text = "0.00"
                            Else
                                txtPesoBruto.Text = Format(Convert.ToDouble(dRow.Item("PesoBruto_pes").ToString), "###,###,###,###")
                            End If
                            txtPesoNeto.Text = Format(Convert.ToDouble(txtPesoBruto.Text.ToString) - Convert.ToDouble(txtPTaraContenedor.Text.ToString), "###,###,###,###")

                            'Habilita Campos Peso
                            txtPesoBruto.Enabled = True
                            txtPTaraVehiculo.Enabled = False
                            txtPesoNeto.Enabled = False
                            TxtPesoVGM.Enabled = False
                        Else
                            txtPesoBruto.Text = Format(Convert.ToDouble(dRow.Item("PesoBruto_pes").ToString), "###,###,###,###")
                            txtPTaraVehiculo.Text = Format(Convert.ToDouble(dRow.Item("PesoTara_pes").ToString), "###,###,###,###")

                            txtPesoNeto.Text = Format(Convert.ToDouble(txtPesoBruto.Text) - (Convert.ToDouble(txtPTaraVehiculo.Text) + Convert.ToDouble(txtPTaraContenedor.Text) + Convert.ToDouble(txtPesoAraña.Text)), "###,###,###,###")
                            TxtPesoVGM.Text = Format(Convert.ToDouble(txtPesoNeto.Text) + Convert.ToDouble(txtPTaraContenedor.Text), "###,###,###,###")

                            If txtBL.Text IsNot String.Empty Then
                                PesoQuintal = Math.Round((Convert.ToDouble(txtPesoNeto.Text) * 2.2046) / 100, 5)
                                TxtQuintales.Text = PesoQuintal.ToString("F3")
                            End If

                            'Habilita Campos
                            txtPesoBruto.Enabled = True
                            txtPTaraVehiculo.Enabled = True
                            txtPesoNeto.Enabled = False
                            TxtPesoVGM.Enabled = False
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

                    'Horas, Usuario y bascula
                    If EstaPesando = "SALIDA" Then
                        txtBasculaSalida.Text = dRow.Item("idbas_salida_pes").ToString
                        dFechaSalida.Value = dRow.Item("fechahora_salida_pes").ToString
                        hSalida.Value = dRow.Item("fechahora_salida_pes").ToString
                        txtOperadorSalida.Text = dRow.Item("idusu_salida_pes").ToString


                    End If

                Next
            Else
                MessageBox.Show("ERROR: Boleta No Existe!!")
                txtTicket.Focus()
            End If

        Catch ex As Exception
            MessageBox.Show("ERROR: " & ex.Message, "Comprobar tabla * Producto")
        End Try
    End Sub

#End Region


    Private Sub Grabar()


        Dim Ticket As String = ""
        Dim StringSql As String
        Dim Paso As String = ""

        Dim vfechaFormat As String = dFechav.Value.ToString("yyyyMMdd")

        Try

            Ticket = txtTicket.Text

            StringSql = <SqlString>
                             UPDATE [dbo].[pesajes] SET  [placa_pes] = '<%= txtPlaca.Text %>',
                                                                       [Idcla_pes] = '<%= cmbClaseCarga.SelectedItem("id_cla").ToString %>',
                                                                       [prefijocont_pes] = '<%= txtPrefijo.Text %>',
                                                                       [numidentcont_pes] = '<%= txtContenedor.Text %>',
                                                                       [tamanocon_pes] = '<%= txtMedida.Text %>',
                                                                       [tipocarga_pes] = '<%= txtTipoCarga.Text %>',
                                                                       [dicecontener_pes] = '<%= txtDiceContenedor.Text %>',
                                                                       [Idpro_pes] = '<%= txtProducto.Text %>',
                                                                       [tipooperacion_pes] = '<%= cmbTipoOperacion.Text %>',
                                                                       [Idbuq_pes] = '<%= txtBuque.Text %>',
                                                                       [bodegabuq_pes] = '<%= txtBodega.Text %>',
                                                                       [Idvbuq_pes] = '<%= txtViaje.Text %>',
                                                                       [fechav_pes] = '<%= vfechaFormat %>',
                                                                       [Idcli_pes] = '<%= txtNaviera.Text %>',
                                                                       [pesomanif_pes] = <%= txtPManifestado.Text.Replace(",", "") %>,
                                                                       [pesotara_pes] = <%= txtPTaraVehiculo.Text.Replace(",", "") %>,
                                                                       [pesotaracont_pes] = <%= txtPTaraContenedor.Text.Replace(",", "") %>,
                                                                       [pesobruto_pes] = <%= txtPesoBruto.Text.Replace(",", "") %>,
                                                                       [pesoarana_pes] = <%= txtPesoAraña.Text.Replace(",", "") %>,
                                                                       [blbuq_pes] = '<%= txtBL.Text %>'
                                    WHERE ticketboleta_pes = '<%= Ticket %>' 
                                    AND EstePesaje_pes = '<%= EstaPesando %>'
                            </SqlString>

            Try

                'LLamamos la rutina para grabar
                Datos.consulta_non_query(StringSql)

                Paso = "Si"

            Catch ex As Exception

                Paso = "No"

            End Try


        Catch ex As Exception
            MessageBox.Show("ERROR: " & ex.Message, "Comprobar tabla * Pesaje")
            Paso = "No"
        End Try

        If Paso = "Si" Then
            If MessageBox.Show("Imprimir la Boleta..?", "", MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.Yes Then

                If LibreriaGeneral.gPrinter = "B" Then
                    Datos.ImprBoleta(Ticket, EstaPesando)
                Else
                    Datos.Imprime_Ticket(Ticket, EstaPesando)
                End If

            End If

            Textos()

        End If

    End Sub


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Textos()

        EstaPesando = cmbTipo.Text

        ControlEntrada()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'Se arma el script para grabar SQL
        Dim StringSql As String = <sqlExp>
                                    SELECT DISTINCT TOP 10000 SUBSTRING(ticketboleta_pes, PATINDEX('%[0-9]%', ticketboleta_pes + '0'), LEN(ticketboleta_pes)) As Codigo,
		                                        placa_pes As Nombre,
		                                        SUBSTRING(tipocarga_pes,1,3) As Carga
                                        From pesajes
                                        ORDER BY TRY_CONVERT(BIGINT, SUBSTRING(ticketboleta_pes, PATINDEX('%[0-9]%', ticketboleta_pes + '0'), LEN(ticketboleta_pes))) DESC

                                   </sqlExp>.Value


        'Script para Filtrar el Grid
        Dim stringSqlFiltro As String = "SELECT SUBSTRING(ticketboleta_pes, PATINDEX('%[0-9]%', ticketboleta_pes + '0'), LEN(ticketboleta_pes)) As Codigo, placa_pes As Nombre FROM [dbo].[pesajes] WHERE SUBSTRING(ticketboleta_pes, PATINDEX('%[0-9]%', ticketboleta_pes + '0'), LEN(ticketboleta_pes)) "

        Dim frmBuscar As New BuscaCodigoA(StringSql, stringSqlFiltro, "Listado Boletas")
        frmBuscar.ShowDialog()

        'Retorna los datos del Cajero selecionado y 
        'se posiciona en Serie Recibo
        txtTicket.Text = ""
        If frmBuscar.Codigo <> String.Empty Then
            txtTicket.Text = frmBuscar.Codigo

            txtTicket.Focus()
        End If
    End Sub


    Private Sub txtNaviera_PreviewKeyDown(sender As Object, e As PreviewKeyDownEventArgs) Handles txtNaviera.PreviewKeyDown
        ' Detectar Enter o Tab
        If e.KeyCode = Keys.Enter Or e.KeyCode = Keys.Tab Then

            If EstaPesando = "SALIDA" Then
                'If txtBodega.Text.ToString.Trim <> "0" AndAlso txtBodega.Text.ToString.Trim <> "" Then
                '    btnGrabar.Focus()

                '    If e.KeyCode = Keys.Tab Then
                '        SendKeys.Send("{RIGHT}")
                '    End If

                'Else
                '    txtBodega.Focus()
                'End If
            Else
                btnGrabar.Focus()

                If e.KeyCode = Keys.Tab Then
                    SendKeys.Send("{RIGHT}")
                End If
            End If
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

    'Private Sub txtPesoBruto_Validating(sender As Object, e As EventArgs) Handles txtPesoBruto.Validating

    '    Dim PesoBruto As Double = 0.00
    '    Dim PesoTara As Double = 0.00

    '    PesoBruto = Convert.ToDouble(txtPesoBruto.Text)
    '    If PesoBruto < 1000 Then
    '        PesoBruto = PesoBruto * 1000
    '    End If

    '    PesoTara = Convert.ToDouble(txtPTaraVehiculo.Text)
    '    If PesoTara < 1000 Then
    '        PesoTara = PesoTara * 1000
    '    End If

    '    txtPesoNeto.Text = Format(PesoBruto - PesoTara + Convert.ToDouble(txtPTaraContenedor.Text) + Convert.ToDouble(txtPesoAraña.Text), "###,###,###,###")

    '    TxtPesoVGM.Text = txtPesoNeto.Text

    '    'If cmbTipoOperacion.Text.Trim = "IMPORTACION" And EstaPesando = "SALIDA" Then
    '    '    Control_PesoBl()
    '    'End If

    'End Sub


    'Private Sub txtPesoBruto_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPesoBruto.KeyPress

    '    If e.KeyChar = ChrW(Keys.Enter) Then
    '        'Posicion en el nuevo Campo
    '        btnGrabar.Focus()
    '        e.Handled = True
    '    End If
    'End Sub

End Class
