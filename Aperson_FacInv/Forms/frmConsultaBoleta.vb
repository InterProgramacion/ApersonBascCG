Public Class frmConsultaBoleta

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
    Dim EstaPesando As String = ""

#End Region

#Region "Entorno"

    Private Sub frmConsultaBoleta_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'If LibreriaGeneral.nBascu = 0 Then
        '    MessageBox.Show("La BASCULA no tiene Computadora Asiganda" & vbCrLf & "Comunicarse con Administrador")
        '    Me.Close()
        '    End
        'Else

        'Arma el ComboBoxT
        Datos.cargar_combo(cmbClaseCarga, _
                           "Select NombreClase_cla, id_cla From [dbo].[clasecarga] Order by 1", _
                           "id_cla", "NombreClase_cla")



        Textos()            'Limpia Textos
        'LeerData(grdData)   'Llena Datos Pendientes de Salir
        ' SumaTicket()        'Suma Correla
        'End If
    End Sub

    'Muestra Ticket
    Private Sub SumaTicket()
        'Para mostrar el Dato
        dTable = Datos.consulta_reader("SELECT REPLICATE('0', LEN(nticketboleta_cor)-1) + RTRIM(LTRIM(CAST(nticketboleta_cor+1 as VARCHAR(5)))) AS Corre FROM correlactu WHERE empresa_cor = " & LibreriaGeneral.idEmpresa & " AND tipo_cor = 'RECEPCION' AND estatus_cor = 'activo'")

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
        ' txtPlaca.Text = ""
        cmbClaseCarga.Text = ""

        'Contenedor
        txtPrefijo.Text = ""
        txtContenedor.Text = ""
        txtMedida.Text = ""
        txtTipoCarga.Text = ""
        txtDiceContenedor.Text = ""

        'Solictados
        txtProducto.Text = ""
        txtNomProducto.Text = ""
        txtTipoCarga.Text = ""
        txtBuque.Text = ""
        txtNomBuque.Text = ""
        txtViaje.Text = ""
        txtBodega.Text = ""
        txtPaisOrigen.Text = ""
        txtNomPaisOrigien.Text = ""
        txtPaisDestino.Text = ""
        txtNomPaisDestino.Text = ""
        txtNaviera.Text = ""
        txtNomNaviera.Text = ""
        ' txtTicket.Text = ""

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


        'Pesaje
        txtPManifestado.Enabled = True
        txtPTaraContenedor.Enabled = True
        txtPesoAraña.Enabled = True


        txtPTaraVehiculo.Enabled = False
        txtPesoBruto.Enabled = False
        txtPesoNeto.Enabled = False

        'Se posiciona para Iniciar 
        ' txtPlaca.Focus()
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
    '--------------------------Despliega pantalla para buscar el Pais Origen
    Private Sub BuscaPaisO()
        Dim stringSQL As String

        stringSQL = "SELECT id_pai As Codigo, nombre_pai As Nombre FROM [dbo].[paisdestorig]"

        'Script para Filtrar el Grid
        Dim stringSqlFiltro As String = "SELECT id_pai As Codigo, nombre_pai As Nombre FROM [dbo].[paisdestorig] WHERE id_pai "

        Dim frmBuscar As New BuscaCodigo(stringSQL, {"Código", "Nombre", "50", "200"}, 2, stringSqlFiltro, "Lista Paises Origen")
        frmBuscar.ShowDialog()

        'Retorna los datos del Cajero selecionado y 
        'se posiciona en Serie Recibo
        txtPaisOrigen.Text = ""
        If frmBuscar.Codigo <> String.Empty Then
            txtPaisOrigen.Text = frmBuscar.Codigo

            txtPaisOrigen.Focus()
        End If

    End Sub

    '--------------------------Despliega pantalla para buscar el Pais Destino
    Private Sub BuscaPaisD()
        Dim stringSQL As String

        stringSQL = "SELECT id_pai As Codigo, nombre_pai As Nombre FROM [dbo].[paisdestorig]"

        'Script para Filtrar el Grid
        Dim stringSqlFiltro As String = "SELECT id_pai As Codigo, nombre_pai As Nombre FROM [dbo].[paisdestorig] WHERE id_pai "

        Dim frmBuscar As New BuscaCodigo(stringSQL, {"Código", "Nombre", "50", "200"}, 2, stringSqlFiltro, "Lista Paises Destino")
        frmBuscar.ShowDialog()

        'Retorna los datos del Cajero selecionado y 
        'se posiciona en Serie Recibo
        txtPaisDestino.Text = ""
        If frmBuscar.Codigo <> String.Empty Then
            txtPaisDestino.Text = frmBuscar.Codigo

            txtPaisDestino.Focus()
        End If

    End Sub
    '-----------------------------------Fin Busca Pais Destino

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
#End Region

#Region "ValidaBusquedas"
    Private Sub txtProducto_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs)
        Try
            'Nombre Proveedor
            dTable = Datos.consulta_reader("SELECT nombre_pro FROM dbo.Producto WHERE id_pro=" & txtProducto.Text)
            txtNomProducto.Text = dTable.Rows.Item(0).Item("nombre_pro")
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtBuque_Validating(sender As Object, e As EventArgs) Handles txtBuque.Validating

        Try
            'Nombre Buque
            dTable = Datos.consulta_reader("SELECT nombre_buq FROM dbo.Buques WHERE Id_buq=" & txtBuque.Text)
            txtNomBuque.Text = dTable.Rows.Item(0).Item("nombre_buq")

            'Ultimo Viajte
            dTable = Datos.consulta_reader("select top 1 fechaviaje_buq fecha, viaje_buq FROM [dbo].[buqueviaje] where Id_buq ='" & txtBuque.Text & "' order by 1 desc")
            txtViaje.Text = dTable.Rows.Item(0).Item("viaje_buq")
            dFechav.Value = dTable.Rows.Item(0).Item("fecha")
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtPaisOrigen_Validating(sender As Object, e As EventArgs) Handles txtPaisOrigen.Validating
        Try
            'Nombre Proveedor
            dTable = Datos.consulta_reader("SELECT nombre_pai FROM dbo.paisdestorig WHERE id_pai=" & txtPaisOrigen.Text)
            txtNomPaisOrigien.Text = dTable.Rows.Item(0).Item("nombre_pai")
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtPaisDestino_Validating(sender As Object, e As EventArgs) Handles txtPaisDestino.Validating
        Try
            'Nombre Proveedor
            dTable = Datos.consulta_reader("SELECT nombre_pai FROM dbo.paisdestorig WHERE id_pai=" & txtPaisDestino.Text)
            txtNomPaisDestino.Text = dTable.Rows.Item(0).Item("nombre_pai")
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtNaviera_Validating(sender As Object, e As EventArgs) Handles txtNaviera.Validating
        Try
            'Nombre Proveedor
            dTable = Datos.consulta_reader("SELECT nombre_cli FROM dbo.Cliente WHERE id_cli=" & txtNaviera.Text)
            txtNomNaviera.Text = dTable.Rows.Item(0).Item("nombre_cli")
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtPlaca_Validating(sender As Object, e As EventArgs) Handles txtPlaca.Validating
        Try
            If txtPlaca.Text.Trim <> String.Empty Then
                'Nombre Proveedor
                dTable = Datos.consulta_reader("SELECT nombre_tpt FROM dbo.fcamiones LEFT JOIN dbo.transporte ON id_tpt = transporte_fca WHERE rtrim(ltrim(placa_fca))='" & txtPlaca.Text.Trim & "'")
                If dTable.Rows.Count > 0 Then
                    'txtNomTransporte.Text = dTable.Rows.Item(0).Item("nombre_tpt")
                Else
                    ' txtNomTransporte.Text = ""
                End If

                'Para ver si ya existe una entrada
                ControlEntrada()
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub txtClase_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtClase.Validating
        Try
            If EstaPesando = "ENTRADA" Then
                'Nombre Proveedor
                txtClase.Text = txtClase.Text.ToUpper
                dTable = Datos.consulta_reader("SELECT nombreclase_cla FROM dbo.clasecarga WHERE Abrevia_cla='" & txtClase.Text.Trim & "'")
                cmbClaseCarga.Text = dTable.Rows.Item(0).Item("nombreclase_cla")
            End If

            txtTipo.Focus()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtTipo_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs)

        If EstaPesando = "ENTRADA" Then

            txtTipo.Text = txtTipo.Text.ToUpper
            txtPaisOrigen.Enabled = True
            txtPaisDestino.Enabled = True
            txtPaisOrigen.Text = ""
            txtPaisDestino.Text = ""
            txtNomPaisOrigien.Text = ""
            txtNomPaisDestino.Text = ""

            If txtTipo.Text = "I" Then
                cmbTipoOperacion.Text = "IMPORTACION"


                'Nombre Pais
                dTable = Datos.consulta_reader("SELECT id_pai FROM dbo.paisdestorig WHERE pais_pai='SD'")
                txtPaisOrigen.Text = dTable.Rows.Item(0).Item("id_pai")

                dTable = Datos.consulta_reader("SELECT nombre_pai FROM dbo.paisdestorig WHERE id_pai=" & txtPaisOrigen.Text)
                txtNomPaisOrigien.Text = dTable.Rows.Item(0).Item("nombre_pai")
                txtPaisOrigen.Enabled = False

            Else
                cmbTipoOperacion.Text = "EXPORTACION"

                'Nombre Pais
                dTable = Datos.consulta_reader("SELECT id_pai FROM dbo.paisdestorig WHERE pais_pai='SD'")
                txtPaisDestino.Text = dTable.Rows.Item(0).Item("id_pai")

                dTable = Datos.consulta_reader("SELECT nombre_pai FROM dbo.paisdestorig WHERE id_pai=" & txtPaisDestino.Text)
                txtNomPaisDestino.Text = dTable.Rows.Item(0).Item("nombre_pai")
                txtPaisDestino.Enabled = False
            End If
        End If

        txtBuque.Focus()
    End Sub

    'KeyDown, cuando presionamos F2 en el txtCodigo_Cajero
    'Funciona para que cuando presionamos dicha tecla nos muestre la ayuda.
    Private Sub txtIdEmple_KeyDown(sender As Object, e As KeyEventArgs)
        Select Case e.KeyCode
            Case Keys.F2
                BuscaProdu()
        End Select
    End Sub

    Private Sub txtBuque_KeyDown(sender As Object, e As KeyEventArgs)
        Select Case e.KeyCode
            Case Keys.F2
                BuscaBuque()
        End Select
    End Sub

    Private Sub txtPaisOrigen_KeyDown(sender As Object, e As KeyEventArgs)
        Select Case e.KeyCode
            Case Keys.F2
                BuscaPaisO()
        End Select
    End Sub

    Private Sub txtPaisDestino_KeyDown(sender As Object, e As KeyEventArgs)
        Select Case e.KeyCode
            Case Keys.F2
                BuscaPaisD()
        End Select
    End Sub

    Private Sub txtNaviera_KeyDown(sender As Object, e As KeyEventArgs)
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


    Private Sub txtBodega_Validating(sender As Object, e As EventArgs) Handles txtBodega.Validating
        'Try
        '    'Nombre Bodega y Producto
        '    Dim sqlStringI As String = <SqlString> 
        '                                    SELECT producto_buq, 
        '                                        producto.nombre_pro 
        '                                    FROM [dbo].[buqueviaje]
        '                                    LEFT JOIN  [dbo].[producto] ON id_pro = producto_buq
        '                                    WHERE Id_buq = '<%= txtBuque.Text %>' 
        '                                    AND viaje_buq = '<%= txtViaje.Text %>'
        '                                    AND fechaviaje_buq = '<%= Format(dFechav.Value, "yyyy-MM-dd") %>' 
        '                                    AND bodega_buq = '<%= txtBodega.Text %>'
        '                               </SqlString>

        '    dTable = Datos.consulta_reader(sqlStringI)

        '    txtProducto.Text = dTable.Rows.Item(0).Item("producto_buq")
        '    txtNomProducto.Text = dTable.Rows.Item(0).Item("nombre_pro")

        'Catch ex As Exception

        'End Try

    End Sub
#End Region

    'Private Sub btnGrabar_Click(sender As Object, e As EventArgs) Handles btnGrabar.Click
    '    Grabar()
    'End Sub


    Private Sub cmbClaseCarga_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbClaseCarga.SelectedIndexChanged
        If cmbClaseCarga.Text.Trim <> "CONTENEDOR" Then

            'Contenedor
            txtPrefijo.Enabled = False
            txtContenedor.Enabled = False
            txtMedida.Enabled = False
            txtTipoCarga.Enabled = False
            txtDiceContenedor.Enabled = False
        End If

    End Sub

    Private Sub ControlEntrada()
        Try
            'Para mostrar el Dato
            dTable = Datos.consulta_reader("select * from pesajes WHERE SUBSTRING(ticketboleta_pes, PATINDEX('%[0-9]%', ticketboleta_pes + '0'), LEN(ticketboleta_pes)) = '" & txtTicket.Text.Trim & "' AND EstePesaje_pes = '" & EstaPesando & "'")

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

                    'Solictados
                    txtProducto.Text = dRow.Item("idpro_pes").ToString
                    txtProducto_Validating(txtProducto.Text, Nothing)

                    cmbTipoOperacion.Text = dRow.Item("TipoCarga_pes").ToString
                    txtBuque.Text = dRow.Item("idbuq_pes").ToString
                    txtBuque_Validating(txtBuque.Text, Nothing)

                    txtViaje.Text = dRow.Item("IdvBuq_pes").ToString
                    txtBodega.Text = dRow.Item("bodegabuq_pes").ToString

                    txtPaisOrigen.Text = dRow.Item("idpaisorig_pes").ToString
                    txtPaisOrigen_Validating(txtPaisOrigen.Text, Nothing)

                    txtPaisDestino.Text = dRow.Item("idpaisdest_pes").ToString
                    txtPaisDestino_Validating(txtPaisDestino.Text, Nothing)

                    txtNaviera.Text = dRow.Item("idcli_pes").ToString
                    txtNaviera_Validating(txtNaviera.Text, Nothing)


                    'Pesaje   
                    If dRow.Item("TipoOperacion_pes").ToString = "IMPORTACION" Then

                        If dRow.Item("EstePesaje_pes").ToString = "ENTRADA" Then

                            If dRow.Item("PesoTara_pes").ToString = "0.000" Then
                                txtPTaraVehiculo.Text = "0"
                            Else
                                txtPTaraVehiculo.Text = Format(Convert.ToDouble(dRow.Item("PesoTara_pes").ToString), "###,###,###,###")
                            End If

                            txtPesoNeto.Text = Format(Convert.ToDouble(dRow.Item("PesoTara_pes").ToString), "###,###,###,###")
                        Else
                            txtPTaraVehiculo.Text = Format(Convert.ToDouble(dRow.Item("PesoTara_pes").ToString), "###,###,###,###")
                            txtPesoBruto.Text = Format(Convert.ToDouble(dRow.Item("PesoBruto_pes").ToString), "###,###,###,###")
                            txtPesoNeto.Text = Format(Convert.ToDouble(txtPesoBruto.Text) - (Convert.ToDouble(txtPTaraVehiculo.Text) + Convert.ToDouble(txtPTaraContenedor.Text) + Convert.ToDouble(txtPesoAraña.Text)), "###,###,###,###")
                            'TxtPesoVGM.Text = Format(Convert.ToDouble(txtPesoNeto.Text) + Convert.ToDouble(txtPTaraContenedor.Text), "###,###,###,###")


                            txtBasculaSalida.Text = dRow.Item("idbas_salida_pes").ToString
                            dFechaSalida.Value = dRow.Item("fechahora_salida_pes").ToString
                            hSalida.Value = dRow.Item("fechahora_salida_pes").ToString
                            txtOperadorSalida.Text = dRow.Item("idusu_salida_pes").ToString

                        End If

                    Else  'EXPORT

                        If dRow.Item("EstePesaje_pes").ToString = "ENTRADA" Then

                            If dRow.Item("PesoBruto_pes").ToString = "0.000" Then
                                txtPesoBruto.Text = "0"
                            Else
                                txtPesoBruto.Text = Format(Convert.ToDouble(dRow.Item("PesoBruto_pes").ToString), "###,###,###,###")
                            End If

                            txtPesoNeto.Text = Format(Convert.ToDouble(dRow.Item("PesoBruto_pes").ToString), "###,###,###,###")
                        Else
                            txtPesoBruto.Text = Format(Convert.ToDouble(dRow.Item("PesoBruto_pes").ToString), "###,###,###,###")
                            txtPTaraVehiculo.Text = Format(Convert.ToDouble(dRow.Item("PesoTara_pes").ToString), "###,###,###,###")

                            txtPesoNeto.Text = Format(Convert.ToDouble(txtPesoBruto.Text) - (Convert.ToDouble(txtPTaraVehiculo.Text) + Convert.ToDouble(txtPTaraContenedor.Text) + Convert.ToDouble(txtPesoAraña.Text)), "###,###,###,###")
                            ' TxtPesoVGM.Text = Format(Convert.ToDouble(txtPesoNeto.Text) + Convert.ToDouble(txtPTaraContenedor.Text), "###,###,###,###")


                            txtBasculaSalida.Text = dRow.Item("idbas_salida_pes").ToString
                            dFechaSalida.Value = dRow.Item("fechahora_salida_pes").ToString
                            hSalida.Value = dRow.Item("fechahora_salida_pes").ToString
                            txtOperadorSalida.Text = dRow.Item("idusu_salida_pes").ToString

                        End If
                    End If

                    If cmbTipoOperacion.Text = "IMPORTACION" Then
                        txtTipo.Text = "I"
                    Else
                        txtTipo.Text = "E"
                    End If

                    'Clase Carga
                    dTable2 = Datos.consulta_reader("select nombreclase_cla,Abrevia_cla from clasecarga WHERE id_cla = '" & dRow.Item("idcla_pes").ToString & "'")

                    If dTable2.Rows.Count > 0 Then

                        For Each dRow2 As DataRow In dTable2.Rows
                            cmbClaseCarga.Text = dRow2.Item("NombreClase_cla").ToString
                            txtClase.Text = dRow2.Item("Abrevia_cla").ToString
                        Next
                    End If


                Next
            Else
            End If

        Catch ex As Exception
            MessageBox.Show("ERROR: " & ex.Message, "Comprobar tabla * Producto")
        End Try
    End Sub

    'Private Sub Grabar()

    '    Dim host As Integer
    '    Dim Periodo As Integer
    '    Dim Ticket As String = ""


    '    Dim Bascula As String = LibreriaGeneral.nBascu
    '    Dim Usuario As String = LibreriaGeneral.usuario
    '    Dim fechahora As DateTime = DateTime.Now
    '    Dim StringSql As String
    '    Dim Paso As String = ""

    '    Try

    '        If EstaPesando = "ENTRADA" Then

    '            SumaTicket()
    '            Ticket = txtTicket.Text

    '            StringSql = <SqlString>
    '                         INSERT INTO [DbAperBascu].[dbo].[pesajes]
    '                                   ([host_pes]
    '                                   ,[periodocorrelativo_pes]
    '                                   ,[ticketboleta_pes]
    '                                   ,[placa_pes]
    '                                   ,[Idcla_pes]
    '                                   ,[prefijocont_pes]
    '                                   ,[numidentcont_pes]
    '                                   ,[tipocarga_pes]
    '                                   ,[dicecontener_pes]
    '                                   ,[Idpro_pes]
    '                                   ,[tipooperacion_pes]
    '                                   ,[Idbuq_pes]
    '                                   ,[bodegabuq_pes]
    '                                   ,[Idvbuq_pes]
    '                                   ,[fechav_pes]
    '                                   ,[Idcli_pes]
    '                                   ,[Idpaisdest_pes]
    '                                   ,[Idpaisorig_pes]
    '                                   ,[pesomanif_pes]
    '                                   ,[pesotara_pes]
    '                                   ,[pesotaracont_pes]
    '                                   ,[pesobruto_pes]
    '                                   ,[pesoarana_pes]
    '                                   ,[Idbas_entrada_pes]
    '                                   ,[fechahora_entrada_pes]
    '                                   ,[idusu_entrada_pes]
    '                                   ,[id_usu_insert_pes]
    '                                   ,[fecha_insert_pes]
    '                                   ,[cierre_pes] 
    '                                   ,[EstePesaje_pes])
    '                                VALUES
    '                                   (<%= host %>
    '                                   ,<%= Periodo %>
    '                                   ,'<%= Ticket %>'
    '                                   ,'<%= txtPlaca.Text %>'
    '                                   ,'<%= cmbClaseCarga.SelectedItem("id_cla").ToString %>'
    '                                   ,'<%= txtPrefijo.Text %>'
    '                                   ,'<%= txtContenedor.Text %>'
    '                                   ,'<%= cmbTipoOperacion.Text %>'
    '                                   ,'<%= txtDiceContenedor.Text %>'
    '                                   ,'<%= txtProducto.Text %>'
    '                                   ,'<%= cmbTipoOperacion.Text %>'
    '                                   ,'<%= txtBuque.Text %>'
    '                                   ,'<%= txtBodega.Text %>'
    '                                   ,'<%= txtViaje.Text %>'
    '                                   ,'<%= dFechav.Text %>'
    '                                   ,'<%= txtNaviera.Text %>'
    '                                   ,'<%= txtPaisDestino.Text %>'
    '                                   ,'<%= txtPaisOrigen.Text %>'
    '                                   ,<%= txtPManifestado.Text %>
    '                                   ,<%= txtPTaraVehiculo.Text %>
    '                                   ,<%= txtPTaraContenedor.Text %>
    '                                   ,<%= txtPesoBruto.Text %>
    '                                   ,<%= txtPesoAraña.Text %>
    '                                   ,'<%= Bascula %>'
    '                                   ,'<%= Format(fechahora, "dd-MM-yyyy H:mm:ss") %>'
    '                                   ,'<%= Usuario %>'
    '                                   ,'<%= Usuario %>'
    '                                   ,'<%= Format(fechahora, "dd-MM-yyyy  H:mm:ss") %>'
    '                                   ,'<%= "NO" %>'
    '                                   ,'<%= EstaPesando %>')
    '                            </SqlString>

    '        Else

    '            'SumaTicket()
    '            Ticket = txtTicket.Text

    '            StringSql = <SqlString>
    '                         INSERT INTO [DbAperBascu].[dbo].[pesajes]
    '                                   ([host_pes]
    '                                   ,[periodocorrelativo_pes]
    '                                   ,[ticketboleta_pes]
    '                                   ,[placa_pes]
    '                                   ,[Idcla_pes]
    '                                   ,[prefijocont_pes]
    '                                   ,[numidentcont_pes]
    '                                   ,[tipocarga_pes]
    '                                   ,[dicecontener_pes]
    '                                   ,[Idpro_pes]
    '                                   ,[tipooperacion_pes]
    '                                   ,[Idbuq_pes]
    '                                   ,[bodegabuq_pes]
    '                                   ,[Idvbuq_pes]
    '                                   ,[fechav_pes]
    '                                   ,[Idcli_pes]
    '                                   ,[Idpaisdest_pes]
    '                                   ,[Idpaisorig_pes]
    '                                   ,[pesomanif_pes]
    '                                   ,[pesotara_pes]
    '                                   ,[pesotaracont_pes]
    '                                   ,[pesobruto_pes]
    '                                   ,[pesoarana_pes]
    '                                   ,[Idbas_entrada_pes]
    '                                   ,[fechahora_entrada_pes]
    '                                   ,[idusu_entrada_pes]
    '                                   ,[Idbas_salida_pes]
    '                                   ,[idusu_salida_pes]                                     
    '                                   ,[fechahora_salida_pes]
    '                                   ,[fechahora_cierre_pes]
    '                                   ,[cierre_pes]
    '                                   ,[id_usu_insert_pes]
    '                                   ,[fecha_insert_pes]
    '                                   ,[EstePesaje_pes])
    '                                VALUES
    '                                   (<%= host %>
    '                                   ,<%= Periodo %>
    '                                   ,'<%= Ticket %>'
    '                                   ,'<%= txtPlaca.Text %>'
    '                                   ,'<%= cmbClaseCarga.SelectedItem("id_cla").ToString %>'
    '                                   ,'<%= txtPrefijo.Text %>'
    '                                   ,'<%= txtContenedor.Text %>'
    '                                   ,'<%= cmbTipoOperacion.Text %>'
    '                                   ,'<%= txtDiceContenedor.Text %>'
    '                                   ,'<%= txtProducto.Text %>'
    '                                   ,'<%= cmbTipoOperacion.Text %>'
    '                                   ,'<%= txtBuque.Text %>'
    '                                   ,'<%= txtBodega.Text %>'
    '                                   ,'<%= txtViaje.Text %>'
    '                                   ,'<%= dFechav.Text %>'
    '                                   ,'<%= txtNaviera.Text %>'
    '                                   ,'<%= txtPaisDestino.Text %>'
    '                                   ,'<%= txtPaisOrigen.Text %>'
    '                                   ,<%= txtPManifestado.Text %>
    '                                   ,<%= txtPTaraVehiculo.Text %>
    '                                   ,<%= txtPTaraContenedor.Text %>
    '                                   ,<%= txtPesoBruto.Text %>
    '                                   ,<%= txtPesoAraña.Text %>
    '                                   ,'<%= Bascula %>'
    '                                   ,'<%= Format(fechahora, "dd-MM-yyyy H:mm:ss") %>'
    '                                   ,'<%= Usuario %>'
    '                                   ,'<%= Bascula %>'
    '                                   ,'<%= Usuario %>'
    '                                   ,'<%= Format(fechahora, "dd-MM-yyyy H:mm:ss") %>'
    '                                   ,'<%= Format(fechahora, "dd-MM-yyyy H:mm:ss") %>'
    '                                   ,'<%= "CIERRE" %>'
    '                                   ,'<%= Usuario %>'
    '                                   ,'<%= Format(fechahora, "dd-MM-yyyy H:mm:ss") %>'
    '                                   ,'<%= EstaPesando %>')
    '                            </SqlString>
    '        End If

    '        'LLamamos la rutina para grabar
    '        Datos.consulta_non_query(StringSql)
    '        Textos()

    '        Paso = "Si"
    '    Catch ex As Exception
    '        MessageBox.Show("ERROR: " & ex.Message, "Comprobar tabla * Pesaje")
    '        Paso = "No"
    '    End Try

    '    If EstaPesando = "ENTRADA" And Paso = "Si" Then
    '        'Para mostrar el Dato
    '        Datos.consulta_non_queryDeta("UPDATE correlactu SET nticketboleta_cor = (SELECT REPLICATE('0', LEN(nticketboleta_cor)-1)+RTRIM(LTRIM(CAST(nticketboleta_cor+1 as VARCHAR(5)))) AS 'X' FROM correlactu)")
    '    End If

    '    If MessageBox.Show("Imprimera la Boelta..?", "", MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.Yes Then
    '        Datos.ImprBoleta(Ticket, EstaPesando)
    '    End If

    '    LeerData(grdData)   'Llena Datos Pendientes de Salir
    'End Sub


    '--------------------------Despliega pantalla para buscar el Buque
    Private Sub LeerData()
        'Dim stringSQL As String

        'stringSQL = "SELECT Id_buq As Codigo, nombre_buq As Nombre FROM [dbo].[Buques]"

        'Se arma el script para grabar SQL
        Dim StringSql As String = <sqlExp>
                                    SELECT SUBSTRING(ticketboleta_pes, PATINDEX('%[0-9]%', ticketboleta_pes + '0'), LEN(ticketboleta_pes)) As Codigo,
		                                        placa_pes As Nombre,
		                                        SUBSTRING(tipocarga_pes,1,3) As Carga
                                        From pesajes
                                   </sqlExp>.Value


        'Script para Filtrar el Grid
        Dim stringSqlFiltro As String = "SELECT SUBSTRING(ticketboleta_pes, PATINDEX('%[0-9]%', ticketboleta_pes + '0'), LEN(ticketboleta_pes)) As Codigo, placa_pes As Nombre FROM [dbo].[pesajes] WHERE SUBSTRING(ticketboleta_pes, PATINDEX('%[0-9]%', ticketboleta_pes + '0'), LEN(ticketboleta_pes)) "

        Dim frmBuscar As New BuscaCodigo(stringSQL, {"Ticket", "Placa", "Tipo", "100", "100", "75"}, 3, stringSqlFiltro, "Listado Boletas")
        frmBuscar.ShowDialog()

        'Retorna los datos del Cajero selecionado y 
        'se posiciona en Serie Recibo
        txtTicket.Text = ""
        If frmBuscar.Codigo <> String.Empty Then
            txtTicket.Text = frmBuscar.Codigo

            txtTicket.Focus()
        End If

    End Sub

    ''-Carga los Tickets pendientes de Cerrar
    'Public Sub LeerData(ByRef grdData As DataGridView)
    '    Dim dv As New DataView

    '    'Se arma el script para grabar SQL
    '    Dim StringSql As String = <sqlExp>
    '                                SELECT ticketboleta_pes,
    '                                      placa_pes,
    '                                      SUBSTRING(tipocarga_pes,1,3)
    '                                    From pesajes
    '                                    WHERE ticketboleta_pes IN (Select ticketboleta_pes 
    '			                                        From pesajes
    '			                                        GROUP BY ticketboleta_pes
    '			                                        HAVING count(ticketboleta_pes) = 1
    '			                                        )
    '                                 </sqlExp>.Value

    '    'Arma Select para Grid
    '    dv = Datos.consulta_dv(StringSql)

    '    grdData.DataSource = dv

    '    grdData.SelectionMode = DataGridViewSelectionMode.FullRowSelect  'Marca toda la Linea en el DataGridView
    '    grdData.MultiSelect = False  'Solo seleccinar una fila
    '    grdData.AllowUserToDeleteRows = False
    '    grdData.ScrollBars = ScrollBars.Vertical

    '    'Encabezado del Grid
    '    grdData.Columns(0).HeaderText = "Boletea"
    '    grdData.Columns(1).HeaderText = "Placa"
    '    grdData.Columns(2).HeaderText = "TipO"

    '    'Ancho de las columnas
    '    grdData.Columns(0).Width = 75
    '    grdData.Columns(1).Width = 70
    '    grdData.Columns(2).Width = 50

    '    grdData.ReadOnly = True
    '    'grdData.ScrollBars = ScrollBars.Both

    '    ''Posición Primera Linea
    '    grdData.Rows(0).Selected = True
    '    grdData.CurrentCell = grdData.Rows(0).Cells(0)
    'End Sub

    'Private Sub grdData_DoubleClick(sender As Object, e As EventArgs) Handles grdData.DoubleClick

    '    Dim dRow As DataGridViewSelectedCellCollection = grdData.SelectedCells

    '    txtPlaca.Text = dRow.Item(1).Value  'Seleciona la Placa
    '    txtTicket.Text = dRow.Item(0).Value
    '    txtPlaca.Focus()
    'End Sub

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

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Textos()

        EstaPesando = cmbTipo.Text

        ControlEntrada()
    End Sub

    Private Sub btnGrabar_Click(sender As Object, e As EventArgs) Handles btnGrabar.Click
        If MessageBox.Show("Imprimera la Boelta..?", "", MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.Yes Then
            
            If LibreriaGeneral.gPrinter = "B" Then
                Datos.ImprBoleta(txtTicket.Text, EstaPesando)
            Else
                Datos.Imprime_Ticket(txtTicket.Text, EstaPesando)
            End If
        End If
    End Sub

  
End Class