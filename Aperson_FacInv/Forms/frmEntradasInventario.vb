Public Class frmEntradasInventario

#Region "Variables"
    Dim Datos As New DatosGenereral()
    Dim NuevoReg As Boolean
    Dim Grabando As Boolean
    Dim dTable As New DataTable
    Dim TotLineas As Integer
    Dim PrimLinea As Integer
    Dim UltiLinea As Integer
    Dim LineaActual As Integer
    Dim Empresa As String = LibreriaGeneral.nEmpresa
    Dim dv As New DataView
    Dim dLinea As Integer = 0
    Dim dEstacion As String = "01"
#End Region

#Region "Entorno"

    Private Sub frmEntradasInventario_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Arma el ComboBox
        Datos.cargar_combo(cmbBodega, _
                           "Select Nombre_bod, Codigo_bod From [dbo].[icatalogobodegas] Order by 1", _
                           "Codigo_bod", "Nombre_bod")

        'Solo las Incidencias que son Entradas (1,2)
        Datos.cargar_combo(cmbMotivo, _
                           "Select* From [dbo].[icatalogotipdoc] WHERE incinv_tdo < 3 order by 1", _
                           "Codigo_tdo", "Nombre_tdo")

        'Limpia la tabla Temporal al Volver a Entrar si hay datos de la transaccion aterior
        Datos.consulta_non_queryDeta("DELETE FROM [dbo].[Temp_Movimiento] WHERE estacion= '" & dEstacion & "'")

        Botonos(False)
        Textos()        'Textos Cabecera
        TextosDeta()    'Textos Detalle

        GridCabecera()
    End Sub

    'Sub Modulo Botonos
    'Sirver para Muestra o Oculta los Botonos
    Private Sub Botonos(ByVal Habilita As Boolean)
        '-Botonos
        btnNuevo.Visible = Not Habilita
        btnBuscar.Enabled = Habilita
        btnSalir.Visible = Not Habilita
        btnGrabar.Visible = Habilita
        btnBajas.Visible = Habilita
        btnBorrar.Visible = Habilita

        If Me.Grabando Then
            btnBorrar.Text = "Cancelar"
            btnGrabar.Text = "Grabar"
        Else
            btnGrabar.Text = "Actualizar"
            btnBorrar.Text = "Cancelar"
        End If

        btnBuscar.Enabled = Habilita
        btnBuscaArticulo.Enabled = Habilita
        btnBuscaProveedor.Enabled = Habilita

        '-Campos
        txtDocumento.Enabled = Habilita
        txtNombreProveedor.Enabled = False
        txtDescripArticulo.Enabled = False

        txtDateDocumento.Enabled = Habilita
        cmbBodega.Enabled = Habilita
        cmbMotivo.Enabled = Habilita
        txtProveedor.Enabled = Habilita
        txtArticulo.Enabled = Habilita
        txtCantidad.Enabled = Habilita
        txtPrecio.Enabled = Habilita
        txtTotal.Enabled = Habilita
        txtObservaciones.Enabled = Habilita
    End Sub

    Private Sub Textos()
        txtDocumento.Text = ""
        txtDateDocumento.Value = Date.Now
        cmbBodega.Text = ""
        cmbMotivo.Text = ""
        txtProveedor.Text = ""
        txtNombreProveedor.Text = ""

        'Se posiciona para Iniciar 
        cmbMotivo.Focus()
    End Sub

    Private Sub TextosDeta()
        txtArticulo.Text = ""
        txtDescripArticulo.Text = ""
        txtCantidad.Text = ""
        txtPrecio.Text = ""

        txtTotal.Text = ""
        txtTotal.Enabled = False
    End Sub

#End Region

#Region "Consulta"
    'Procedimiento para mostrar el Primer Registro
    Private Sub Consulta(ByVal _codigo As String, ByVal _Motivo As String)
        Try
            'Para mostrar el Dato
            dTable = Datos.consulta_reader("SELECT * FROM dbo.icabeceratransacciones WHERE numdoc_cdo=" & _codigo & " AND coddoc_cdo = " & _Motivo)

            Dim nCod As String = ""

            'Setea Campos
            For Each dRow As DataRow In dTable.Rows
                txtDocumento.Text = dRow.Item("numdoc_cdo").ToString
                cmbMotivo.SelectedIndex = dRow.Item("coddoc_cdo").ToString
                txtDateDocumento.Value = dRow.Item("fecha_cdo").ToString
                cmbBodega.SelectedValue = dRow.Item("abodeg_cdo").ToString
                cmbMotivo.SelectedValue = dRow.Item("coddoc_cdo").ToString
                txtProveedor.Text = dRow.Item("clipro_cdo").ToString
            Next

            'Nombre Proveedor
            dTable = Datos.consulta_reader("SELECT nombre_prv FROM dbo.ccCatalogoProveedores WHERE codigo_prv=" & txtProveedor.Text)
            txtNombreProveedor.Text = dTable.Rows.Item(0).Item("nombre_prv")
           
            'Detalle
            Dim sqlString As String = <SqlString> 
                                            SELECT a.codart_hco, b.nombre_art, a.linea_hco, 
                                                   a.pcosto_hco,a.canart_hco 
                                            FROM [dbo].[ihistorialcostos] a 
                                            LEFT JOIN [dbo].[icatalogoarticulos] b ON b.codigo_art = a.codart_hco and b.empresa_art = a.empresa_hco
                                            WHERE a.numdoc_hco= '<%= txtDocumento.Text %>' 
                                             AND a.coddoc_hco = '<%= cmbMotivo.SelectedValue %>'
                                            ORDER BY a.numdoc_hco,a.linea_hco
                                      </SqlString>

            dTable = Datos.consulta_reader(sqlString)

            'Setea Campos
            For Each dRow As DataRow In dTable.Rows
                Dim StringSqlIII As String = <sqlExp>
                                      INSERT INTO [dbo].[Temp_Movimiento]
                                            (Articulo,
                                             Descripcion, 
                                             Cantidad, 
                                             Precio, 
                                             Total,
                                             Documento,
                                             tipo,
                                             linea,
                                             estacion)      
                                      VALUES('<%= dRow.Item("codart_hco").ToString %>',
                                             '<%= dRow.Item("nombre_art").ToString %>',
                                              <%= dRow.Item("canart_hco").ToString %>,
                                              <%= dRow.Item("pcosto_hco").ToString %>,
                                              <%= Math.Round(CDbl(dRow.Item("pcosto_hco") * dRow.Item("canart_hco")), 2) %>,
                                              '<%= txtDocumento.Text %>',
                                              '<%= cmbMotivo.SelectedValue %>',
                                              <%= dRow.Item("linea_hco").ToString %>,
                                             '<%= dEstacion %>')
                                 </sqlExp>.Value

                'Asigna Linea
                dLinea = CInt(dRow.Item("linea_hco"))

                'LLamamos la rutina para grabar
                Datos.consulta_non_queryDeta(StringSqlIII)
            Next

            'Muestra los Datos Detalle
            GridCabecera()

            Me.Grabando = False
            Me.NuevoReg = False

        Catch ex As Exception
            MessageBox.Show("ERROR: " & ex.Message, "Comprobar tabla * icabeceratransacciones")
        End Try
    End Sub

#End Region

#Region "Grabar"
    Private Sub btnGrabar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGrabar.Click

        If ValidaCampos() Then 'Si es verdadero

            '--Si el Nuevo e Inserta
            If NuevoReg = True Then

                Try
                    'Se arma el script para grabar SQL
                    Dim StringSql As String = <sqlExp>
                                      INSERT INTO [dbo].[icabeceratransacciones]
                                                   ([empresa_cdo]
                                                   ,[coddoc_cdo]
                                                   ,[numdoc_cdo]
                                                   ,[fecha_cdo]
                                                   ,[coment_cdo]
                                                   ,[abodeg_cdo]
                                                   ,[clipro_cdo]
                                                   ,[gencxp_cdo]
                                                   ,[nolico_cdo])
                                            VALUES('<%= Empresa %>',
                                                 '<%= cmbMotivo.SelectedValue %>',
                                                 '<%= txtDocumento.Text %>',
                                                 '<%= Format(txtDateDocumento.Value, "dd-MM-yyyy") %>',
                                                 '<%= txtObservaciones.Text %>',
                                                 '<%= cmbBodega.SelectedValue %>',
                                                 '<%= txtProveedor.Text %>',
                                                  <%= 0 %>,
                                                  <%= 0 %>)
                                        </sqlExp>

                    'llama para Grabar Encabezado
                    Datos.consulta_non_queryDeta(StringSql)

                    'Arma la Data Temporal
                    Dim sqlStringI As String = <SqlString> 
                                           SELECT * FROM dbo.Temp_Movimiento a
                                            WHERE documento= '<%= txtDocumento.Text %>' 
                                             AND tipo =<%= cmbMotivo.SelectedValue %>
                                            ORDER BY documento,tipo,linea,estacion
                                      </SqlString>

                    dTable = Datos.consulta_reader(sqlStringI)

                    'Setea Campos
                    For Each dRow As DataRow In dTable.Rows

                        Dim StringSqlII As String = <sqlExp>
                                  INSERT INTO [dbo].[ihistorialcostos]
                                                       ([sistema_hco]
                                                       ,[coddoc_hco]
                                                       ,[empresa_hco]
                                                       ,[linea_hco]
                                                       ,[numdoc_hco]
                                                       ,[bodega_hco]
                                                       ,[codart_hco]
                                                       ,[codprv_hco]
                                                       ,[fecha_hco]
                                                       ,[pcosto_hco]
                                                       ,[canart_hco])
                                                 VALUES
                                                       ('<%= "I" %>'
                                                       ,'<%= cmbMotivo.SelectedValue %>'
                                                       ,'<%= Empresa %>'
                                                       ,'<%= dRow.Item("Linea") %>'
                                                       ,'<%= txtDocumento.Text %>'
                                                       ,'<%= cmbBodega.SelectedValue %>'
                                                       ,'<%= dRow.Item("articulo").ToString %>'
                                                       ,'<%= txtProveedor.Text %>'
                                                       ,'<%= Format(txtDateDocumento.Value, "dd-MM-yyyy") %>'
                                                       , <%= dRow.Item("precio") %>
                                                       , <%= dRow.Item("cantidad") %>)
                                                </sqlExp>

                        'llama para Grabar Detalle Ingresos Inventario
                        Datos.consulta_non_queryDeta(StringSqlII)

                    Next

                    MsgBox("La operacion se realizo con exito!", MsgBoxStyle.Information, "Operacion exitosa!")
                Catch ex As Exception
                    MsgBox("Error al Grabar, No Se Grabo Documento !!!" & ex.Message)
                Finally
                    'Limpia la Tabla Temporal
                    Datos.consulta_non_queryDeta("DELETE FROM [dbo].[Temp_Movimiento] WHERE documento= '" & txtDocumento.Text & "' AND tipo = " & cmbMotivo.SelectedValue)
                End Try

            Else
                '--Si el Editar e Modificar

                'Se arma el script para grabar SQL
                Dim StringSql As String = <sqlExp>
                                      UPDATE [dbo].[icabeceratransacciones] SET empresa_mar = '<%= Empresa %>',
                                                                          Codigo_mar = '<%= txtDocumento.Text %>',
                                        WHERE id_mar = '<%= txtDocumento.Text %>' 
                                  </sqlExp>.Value

                'LLamamos la rutina para grabar
                Datos.consulta_non_query(StringSql)
            End If

            'Refrescamos los dato de pantalla
            Botonos(True)
        End If
    End Sub

    Private Function ValidaCampos() As Boolean
        Dim Si As Boolean = False

        If txtDocumento.Text = String.Empty Then
            MessageBox.Show("ERROR: El Codigo no puede ser Blanco")
            txtDocumento.Focus()
        ElseIf txtDocumento.Text = String.Empty Then
            MessageBox.Show("ERROR: El Nombre no puede ser Blanco")
            txtDocumento.Focus()
        Else
            Si = True
        End If
        Return Si
    End Function
#End Region

#Region "Botones"
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    Private Sub btnNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuevo.Click
        Me.NuevoReg = True
        Me.Grabando = True
        Botonos(True)
        Textos()
        dLinea = 0
    End Sub

    Private Sub btnBorrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBorrar.Click
        'Si es modo nuevo o Editar
        If Me.NuevoReg Then
            'Consulta("")
            Botonos(False)

            'LLamamos la rutina Eliminar
            'Limpia la tabla Temporal al Volver a Entrar si hay datos de la transaccion aterior
            Datos.consulta_non_queryDeta("DELETE FROM [dbo].[Temp_Movimiento] WHERE estacion= '" & dEstacion & "'")
            GridCabecera()
        Else
            Botonos(False)
            'LLamamos la rutina Eliminar
            Datos.consulta_non_queryDeta("DELETE FROM [dbo].[Temp_Movimiento] WHERE estacion= '" & dEstacion & "'")

            'Refrescamos los dato de pantalla
            GridCabecera()
        End If
        cmbMotivo.Focus()
    End Sub

    'Despliega pantalla para buscar el Cajero
    Private Sub BuscaDocumento()
        Dim stringSQL As String

        stringSQL = "SELECT Codigo_mar As Codigo, Nombre_mar As Nombre FROM [dbo].[icatalogomarcas]"

        'Script para Filtrar el Grid
        Dim stringSqlFiltro As String = "SELECT Codigo_mar As Codigo, Nombre_mar As Nombre FROM [dbo].[icatalogomarcas] WHERE Codigo_mar "

        ''Instancia Ayuda Cliente
        'Dim frmBuscar As New BuscaCodigo(stringSQL, stringSqlFiltro, "Marcas")
        'frmBuscar.ShowDialog()

        'Retorna los datos del Cajero selecionado y 
        'se posiciona en Serie Recibo
        'txtId.Text = ""
        'If frmBuscar.Codigo <> String.Empty Then
        '    'Consulta(frmBuscar.Codigo)
        'End If
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        BuscaDocumento()
    End Sub


    Private Sub btnBuscaProveedor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscaProveedor.Click
        Dim stringSQL As String

        stringSQL = "SELECT Codigo_prv As Codigo, Nombre_prv As Nombre FROM [dbo].[ccCatalogoProveedores]"

        'Script para Filtrar el Grid
        Dim stringSqlFiltro As String = "SELECT Codigo_prv As Codigo, Nombre_prv As Nombre FROM [dbo].[ccCatalogoProveedores] WHERE Codigo_prv "

        ''Instancia Ayuda Cliente
        'Dim frmBuscar As New BuscaCodigo(stringSQL, stringSqlFiltro, "Lista Proveedores")
        'frmBuscar.ShowDialog()

        ''Retorna los datos del Cajero selecionado y 
        ''se posiciona en Serie Recibo
        ''txtId.Text = ""
        'If frmBuscar.Codigo <> String.Empty Then
        '    txtProveedor.Text = frmBuscar.Codigo
        '    txtNombreProveedor.Text = frmBuscar.Nombre
        '    txtArticulo.Focus()
        'End If
    End Sub

    Private Sub btnBuscaArticulo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscaArticulo.Click
        Dim stringSQL As String

        stringSQL = "SELECT codigo_art As Codigo, Nombre_art As Nombre, Registro_art, TenenEmp_art FROM [dbo].[icatalogoarticulos]"

        'Script para Filtrar el Grid
        Dim stringSqlFiltro As String = "SELECT Codigo_art As Codigo, Nombre_art As Nombre FROM [dbo].[icatalogoarticulos] WHERE Codigo_art "

        'Instancia Ayuda Cliente
        Dim frmBuscar As New BuscaCodigoA(stringSQL, stringSqlFiltro, "Lista de Articulos")
        frmBuscar.ShowDialog()

        'Retorna los datos del Cajero selecionado y 
        'se posiciona en Serie Recibo
        'txtId.Text = ""
        If frmBuscar.Codigo <> String.Empty Then
            txtArticulo.Text = frmBuscar.Codigo
            txtDescripArticulo.Text = frmBuscar.Nombre
            txtCantidad.Focus()
        End If
    End Sub

#End Region

#Region "Detalle"
    Private Sub GridCabecera()
        Try
            Dim sqlString As String = <SqlString> 
                                       SELECT Articulo, Descripcion, Cantidad, Precio, Total
                                         FROM dbo.Temp_Movimiento a
                                        WHERE documento= '<%= txtDocumento.Text %>' 
                                         AND tipo = '<%= cmbMotivo.SelectedValue %>'
                                        ORDER BY documento,tipo,linea,estacion
                                      </SqlString>

            dv = Datos.consulta_dv(sqlString)

            grdData.DataSource = dv

            grdData.SelectionMode = DataGridViewSelectionMode.FullRowSelect  'Marca toda la Linea en el DataGridView
            grdData.MultiSelect = False  'Solo seleccinar una fila

            'Encabezado del Grid
            grdData.Columns(0).HeaderText = "ARTICULO"
            grdData.Columns(1).HeaderText = "NOMBRE - ARTICULO"
            grdData.Columns(2).HeaderText = "CANTIDAD"
            grdData.Columns(3).HeaderText = "COSTO SIN/IVA"
            grdData.Columns(4).HeaderText = "TOTAL"

            'Ancho de las columnas
            grdData.Columns(0).Width = 130
            grdData.Columns(1).Width = 325
            grdData.Columns(2).Width = 112
            grdData.Columns(3).Width = 112
            grdData.Columns(4).Width = 115

            grdData.Columns(2).DefaultCellStyle.Format = "#,####,###.##"
            grdData.Columns(3).DefaultCellStyle.Format = "##,###.#######"
            grdData.Columns(4).DefaultCellStyle.Format = "###,####,###.##"
            grdData.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            grdData.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            grdData.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        Catch s As Exception
            MsgBox(s.Message)
        End Try
    End Sub

    'Se Suma una linea al Grid
    Private Sub GridDetalle(ByVal _Linea As Integer)
        Dim Paso As Integer = 0
        Try
            If txtArticulo.Text = String.Empty Then
                Paso = 1
            ElseIf CDbl(txtCantidad.Text) = 0.0 Then
                Paso = 1
            End If

            If Paso = 0 Then
                Dim StringSql As String = <sqlExp>
                                      INSERT INTO [dbo].[Temp_Movimiento]
                                            (Articulo,
                                             Descripcion, 
                                             Cantidad, 
                                             Precio, 
                                             Total,
                                             Documento,
                                             tipo,
                                             linea,
                                             estacion)      
                                      VALUES('<%= txtArticulo.Text %>',
                                             '<%= txtDescripArticulo.Text %>',
                                              <%= txtCantidad.Text %>,
                                              <%= txtPrecio.Text %>,
                                              <%= txtTotal.Text %>,
                                              '<%= txtDocumento.Text %>',
                                              '<%= cmbMotivo.SelectedValue %>',
                                              <%= _Linea %>,
                                             '<%= dEstacion %>')
                                 </sqlExp>.Value

                'LLamamos la rutina para grabar
                Datos.consulta_non_queryDeta(StringSql)

                'Suma a la Linea
                dLinea += 1
            End If

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information)
        End Try
    End Sub

#End Region

#Region "ParaDetalle"
    Private Sub btnAltas_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAltas.Click
        GridDetalle(dLinea)
        GridCabecera()
        TextosDeta()

        'Actualiza Detalle
        btnAltas.Enabled = False
        btnBajas.Enabled = True

        txtArticulo.Focus()
    End Sub

    Private Sub txtCantidad_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCantidad.TextChanged
        'Actualiza Detalle
        btnAltas.Enabled = True
        btnBajas.Enabled = False
    End Sub

    Private Sub txtPrecio_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPrecio.TextChanged
        If Not txtPrecio.Text = String.Empty And Not txtCantidad.Text = String.Empty Then
            txtTotal.Text = Math.Round(CDbl(txtPrecio.Text) * CDbl(txtCantidad.Text), 2)
        End If
    End Sub
#End Region

    Private Sub txtDocumento_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDocumento.Validated
        If Me.Grabando Then
            Consulta(txtDocumento.Text, cmbMotivo.SelectedValue)
        End If
    End Sub

    Private Sub txtProveedor_TextChanged(sender As Object, e As EventArgs) Handles txtProveedor.TextChanged

    End Sub
End Class