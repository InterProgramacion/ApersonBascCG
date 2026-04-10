Public Class frmControlBLs

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
    Dim NuevoRegSuper As Integer

#End Region

#Region "Entorno"

    Private Sub frmControlBLs_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Consulta("")
        'Botonos(True)
        btnSalir.Visible = True
        btnBuscar.Enabled = True

        'Boton para Borrar 
        btnAlta.Visible = False
        btnBaja.Visible = False

        txtId.Focus()
    End Sub

    'Sub Modulo Botonos
    'Sirver para Muestra o Oculta los Botonos
    Private Sub Botonos(ByVal Habilita As Boolean)
        '-Botonos
        btnBuscar.Enabled = Habilita
        btnSalir.Visible = Habilita

        If Me.Grabando Then
            btnBorrar.Text = "cancelar"
        Else
            btnBorrar.Text = "borrar"
        End If

        '-Campos
        'txtId.Focus()
        'txtId.Enabled = False
    End Sub

    Private Sub Textos()
        txtLinea.Text = "0"
        'txtBuque.Text = ""
        'txtNomBuque.Text = ""
        txtMerma.Text = "0"
        txtDuca.Text = ""
        dFechav.Format = DateTimePickerFormat.Short

        txtConsigna.Text = ""
        txtNomConsigna.Text = ""
        txtBL.Text = ""
        txtProducto.Text = ""
        txtNomProducto.Text = ""
        txtManifiesto.Text = "0"
        txtPesoBascula.Text = "0.00"
        txtDiferencia.Text = "0.00"

        cmbEstadoSAT.Text = "ACTIVO"
        cmbEstado.Text = "ACTIVO"

        txtBL.Enabled = True
        txtProducto.Enabled = True

        txtDiferencia.Enabled = False
        txtManifiesto.Enabled = True
        txtMerma.Enabled = True
        cmbEstado.Enabled = False

        If txtId.Text = String.Empty Then
            btnBuscar.Enabled = True
        Else
            btnBuscar.Enabled = False
        End If

        NuevoRegSuper = 0
        'Se posiciona para Iniciar 
        'txtId.Focus()
    End Sub

#End Region

#Region "Consulta"
    'Procedimiento para mostrar el Primer Registro
    Private Sub Consulta(ByVal _codigo As String)
        Try

            'Se arma el script para grabar SQL
            Dim StringSql As String = <sqlExp>
                                         SELECT [Correlativo_dbq],
                                                [linea_dbq],
                                                [buque_dbq],
									            bs.nombre_buq,
                                                [viaje_buque_dbq],
                                                [fechaviaje_buque_dbq],
                                                [duca_dbq],
                                                [bl_dbq],
                                                [consignatario_dbq],
									            cs.nombre_csg,
                                                [porcentaje_merma_dbq],
                                                [producto_dbq],
									            pd.nombre_pro,
                                                [manifiesto_dbq],
                                                ISNULL([bascula_dbq],0) As [bascula_dbq],
                                                ISNULL([diferencia_dbq],0) As [diferencia_dbq],
                                                [autorizado_dbq],
                                                [activo_dbq]
                                          FROM [dbo].[Detalle_Buque] db
							              LEFT JOIN [dbo].[buques] bs ON bs.Id_buq = db.buque_dbq
							              LEFT JOIN [dbo].[Consignatario] cs ON cs.Id_csg = db.consignatario_dbq 
							              LEFT JOIN [dbo].[Producto] pd ON pd.Id_pro = db.producto_dbq
							              WHERE RTRIM(LTRIM(Correlativo_dbq)) = '<%= txtId.Text.Trim %>' 
                                          AND [linea_dbq] = '<%= _codigo %>' 
                                  </sqlExp>.Value

            dTable = Datos.consulta_reader(StringSql)

            'Setea Campos
            For Each dRow As DataRow In dTable.Rows
                txtLinea.Text = dRow.Item("linea_dbq").ToString()
                txtBuque.Text = dRow.Item("buque_dbq").ToString()
                txtNomBuque.Text = dRow.Item("nombre_buq").ToString()
                txtViaje.Text = dRow.Item("viaje_buque_dbq").ToString()
                dFechav.Value = dRow.Item("fechaviaje_buque_dbq")
                txtDuca.Text = dRow.Item("duca_dbq").ToString()
                txtConsigna.Text = dRow.Item("consignatario_dbq").ToString()
                txtNomConsigna.Text = dRow.Item("nombre_csg").ToString()
                txtMerma.Text = dRow.Item("porcentaje_merma_dbq").ToString()
                txtBL.Text = dRow.Item("bl_dbq").ToString()
                txtProducto.Text = dRow.Item("producto_dbq").ToString()
                txtNomProducto.Text = dRow.Item("nombre_pro").ToString()
                txtManifiesto.Text = dRow.Item("manifiesto_dbq").ToString()
                txtPesoBascula.Text = dRow.Item("bascula_dbq").ToString()
                txtDiferencia.Text = dRow.Item("diferencia_dbq").ToString()
                cmbEstadoSAT.Text = dRow.Item("autorizado_dbq").ToString()
                cmbEstado.Text = dRow.Item("activo_dbq").ToString()
            Next

            Me.Grabando = True
            Me.NuevoReg = False

            btnAlta.Text = "Actualizar"
            btnAlta.BackColor = Color.DarkCyan

            txtId.Enabled = False
            txtBuque.Enabled = False


            txtDuca.Focus()
        Catch ex As Exception
            MessageBox.Show("ERROR: " & ex.Message, "Comprobar tabla * Detalle_Buque")
        End Try
    End Sub
#End Region

#Region "Grabar"
    Private Sub btnGrabar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAlta.Click

        If ValidaCampos() Then 'Si es verdadero

            Dim IdCorrela As String = ""
            Dim IdLinea As String = ""
            Dim Usuario As String = LibreriaGeneral.usuario
            Dim fechahora As DateTime = DateTime.Now
            Dim fechahoraFormat As String = fechahora.ToString("yyyyMMdd H:mm:ss")
            Dim vfechaFormat As String = dFechav.Value.ToString("yyyyMMdd")

            If NuevoReg = True Then

                'Suma Correlativo
                dTable.Clear()
                dTable = Datos.consulta_reader("SELECT ISNULL(MAX(id_dbq),0) +1  rMax FROM dbo.Detalle_Buque")

                If dTable.Rows.Count > 0 Then
                    'Setea Campos
                    For Each dRow As DataRow In dTable.Rows
                        'Infomrativos
                        IdCorrela = dRow.Item("rMax").ToString
                    Next
                Else
                    IdCorrela = "1"
                End If


                'Suma Correlativo Linea
                dTable.Clear()
                dTable = Datos.consulta_reader("SELECT ISNULL(MAX(linea_dbq),0) + 1 rMax FROM dbo.Detalle_Buque WHERE Correlativo_dbq='" & txtId.Text & "'")

                If dTable.Rows.Count > 0 Then
                    'Setea Campos
                    For Each dRow As DataRow In dTable.Rows
                        'Infomrativos
                        IdLinea = dRow.Item("rMax").ToString
                    Next
                Else
                    IdLinea = "1"
                End If

                'Se arma el script para grabar SQL
                Dim StringSql As String = <sqlExp>

                                    INSERT INTO [dbo].[Detalle_Buque] ([Id_dbq],
                                                                [Correlativo_dbq],
                                                                [linea_dbq],
                                                                [buque_dbq],
                                                                [viaje_buque_dbq],
                                                                [fechaviaje_buque_dbq],
                                                                [duca_dbq],
                                                                [bl_dbq],
                                                                [porcentaje_merma_dbq],
                                                                [consignatario_dbq],
                                                                [producto_dbq],
                                                                [manifiesto_dbq],
                                                                [autorizado_dbq],
                                                                [activo_dbq],
                                                                [id_usu_insert_dbq],
                                                                [fecha_insert_dbq],
                                                                [empresa_dbq],
                                                                [sync_dbq])
                                        VALUES('<%= IdCorrela %>',
                                              '<%= txtId.Text %>',
                                              '<%= IdLinea %>',
                                              '<%= txtBuque.Text %>',
                                              '<%= txtViaje.Text %>',
                                              '<%= vfechaFormat %>',
                                              '<%= txtDuca.Text %>',
                                              '<%= txtBL.Text %>',
                                              '<%= txtMerma.Text %>',
                                              '<%= txtConsigna.Text %>',
                                              '<%= txtProducto.Text %>',
                                              '<%= txtManifiesto.Text %>',
                                              '<%= cmbEstadoSAT.Text %>',
                                              '<%= cmbEstado.Text %>',
                                              '<%= Usuario %>',
                                              '<%= fechahoraFormat %>',
                                              '<%= LibreriaGeneral.gEmpresa %>',
                                              1
                                              )
                                                INSERT INTO duca_buque (duca, buque_idbuque)
                                                VALUES ('<%= txtDuca.Text %>', '<%= txtBuque.Text %>');

                                     </sqlExp>.Value

                'LLamamos la rutina para grabar
                Datos.consulta_non_queryy(StringSql)

                Dim IdDuca As String = ""
                Dim sqlGetId As String = "SELECT MAX(idduca_buque) AS idduca FROM duca_buque"
                dTable.Clear()
                dTable = Datos.consulta_reader(sqlGetId)

                If dTable.Rows.Count > 0 Then
                    IdDuca = dTable.Rows(0)("idduca").ToString()
                End If

                If IdDuca <> "" Then
                    Dim sqlBL As String = "INSERT INTO bl_buque (" &
                          "bl, buque_idbuque, consignatario_idconsignatario, producto_idproducto, porcentaje_merma, registrado_selectivo, duca_buque_idduca_buque) " &
                          "VALUES (" &
                          "'" & txtBL.Text & "', " &
                          "'" & txtBuque.Text & "', " &
                          "'" & txtConsigna.Text & "', " &
                          "'" & txtProducto.Text & "', " &
                          "'" & txtMerma.Text & "', " &
                          "0, " &
                          IdDuca & ")"

                    Datos.consulta_non_queryy(sqlBL)
                End If



            Else
                'Se arma el script para grabar SQL
                Dim StringSql As String = <sqlExp>
                                     UPDATE [dbo].[Detalle_Buque] SET 
                                                                [buque_dbq] = '<%= txtBuque.Text %>',
                                                                [viaje_buque_dbq] = '<%= txtViaje.Text %>',
                                                                [fechaviaje_buque_dbq] = '<%= vfechaFormat %>',
                                                                [duca_dbq] = '<%= txtDuca.Text %>',
                                                                [bl_dbq] = '<%= txtBL.Text %>',
                                                                [porcentaje_merma_dbq] = '<%= txtMerma.Text %>',
                                                                [consignatario_dbq] = '<%= txtConsigna.Text %>',
                                                                [producto_dbq] = '<%= txtProducto.Text %>',
                                                                [manifiesto_dbq] = '<%= txtManifiesto.Text %>',
                                                                [diferencia_dbq] = '<%= txtDiferencia.Text %>',
                                                                [bascula_dbq] = '<%= txtPesoBascula.Text %>',
                                                                [autorizado_dbq] = '<%= cmbEstadoSAT.Text %>',
                                                                [activo_dbq] = '<%= cmbEstado.Text %>',
                                                                [id_usu_update_dbq] = '<%= Usuario %>',
                                                                [fecha_update_dbq]  = '<%= fechahoraFormat %>',
                                                                [sync_dbq]          = '<%= "2" %>'
                                      WHERE [Correlativo_dbq] = '<%= txtId.Text %>' 
                                        AND [linea_dbq] = '<%= txtLinea.Text %>'
                                
                                         INSERT INTO duca_buque (duca, buque_idbuque)
                                        VALUES ('<%= txtDuca.Text %>', '<%= txtBuque.Text %>');

                                </sqlExp>.Value

                'LLamamos la rutina para grabar
                Datos.consulta_non_queryy(StringSql)
                Dim IdDuca As String = ""
                Dim sqlGetId As String = "SELECT MAX(idduca_buque) AS idduca FROM duca_buque"
                dTable.Clear()
                dTable = Datos.consulta_reader(sqlGetId)

                If dTable.Rows.Count > 0 Then
                    IdDuca = dTable.Rows(0)("idduca").ToString()
                End If

                If IdDuca <> "" Then
                    Dim sqlBL As String = "INSERT INTO bl_buque (" &
                          "bl, buque_idbuque, consignatario_idconsignatario, producto_idproducto, porcentaje_merma, registrado_selectivo, duca_buque_idduca_buque) " &
                          "VALUES (" &
                          "'" & txtBL.Text & "', " &
                          "'" & txtBuque.Text & "', " &
                          "'" & txtConsigna.Text & "', " &
                          "'" & txtProducto.Text & "', " &
                          "'" & txtMerma.Text & "', " &
                          "0, " &
                          IdDuca & ")"

                    Datos.consulta_non_queryy(sqlBL)
                End If

            End If

            'Refrescamos los dato de pantalla
            'Consulta("")
            'Botonos(True)
            'Textos()

            txtId_Validating(txtId.Text, Nothing)

            txtDuca.Focus()
        End If
        MsgBox("La operacion se realizo con exito!", MsgBoxStyle.Information, "Operacion exitosa!")
    End Sub

    Private Function ValidaCampos() As Boolean
        Dim Si As Boolean = False

        'Solictados
        txtProducto_Validating(txtProducto.Text, Nothing)
        txtBuque_Validating(txtBuque.Text, Nothing)
        txtConsigna_Validating(txtConsigna.Text, Nothing)

        If NuevoReg = False And Convert.ToDouble(txtPesoBascula.Text) > 0 And NuevoRegSuper = 0 Then
            MessageBox.Show("ERROR: BL No puede ser Modificado, !!Ya tiene Pesajes!!")
        Else
            If txtManifiesto.Text = String.Empty Then
                MessageBox.Show("ERROR: Peso Manifiesto no puede quedar en Blanco")
                txtManifiesto.Focus()
            ElseIf txtNomBuque.Text = String.Empty Then
                MessageBox.Show("ERROR: Buque no puede quedar en Blanco")
                txtBuque.Focus()
            ElseIf txtNomProducto.Text = String.Empty Then
                MessageBox.Show("ERROR: Producto no puede quedar en Blanco")
                txtProducto.Focus()
            ElseIf txtDuca.Text = String.Empty Then
                MessageBox.Show("ERROR: Duca no puede quedar en Blanco")
                txtDuca.Focus()
            ElseIf txtNomConsigna.Text = String.Empty Then
                MessageBox.Show("ERROR: Consignatario no puede quedar en Blanco")
                txtConsigna.Focus()
            ElseIf txtMerma.Text = String.Empty Then
                MessageBox.Show("ERROR: Merma no puede quedar en Blanco")
                txtMerma.Focus()
            ElseIf txtBL.Text = String.Empty Then
                MessageBox.Show("ERROR: BL no puede quedar en Blanco")
                txtBL.Focus()
            ElseIf cmbEstadoSAT.Text = String.Empty Then
                MessageBox.Show("ERROR: Estado SAP no puede quedar en Blanco")
                cmbEstadoSAT.Focus()
            Else
                Si = True
            End If

            If Si Then
                If txtBuque.Text <> String.Empty And NuevoReg = True Then
                    dTable = Datos.consulta_reader("SELECT Id_buq FROM dbo.Buques WHERE Id_buq='" & txtBuque.Text & "' AND  baja_buq = 'CERRADO'")
                    If dTable.Rows.Count > 0 Then
                        MessageBox.Show("ERROR: El Buque esta CERRADO, no se puede utlizar")
                        Si = False
                        txtBuque.Focus()
                    End If
                End If
            End If

            If Si Then
                If txtDuca.Text <> String.Empty And NuevoReg = True Then
                    dTable.Clear()
                    dTable = Datos.consulta_reader("SELECT ISNULL(duca_dbq, '') duca_dbq FROM dbo.Detalle_Buque WHERE Correlativo_dbq='" & txtId.Text & "' AND duca_dbq='" & txtDuca.Text & "'")

                    If dTable.Rows.Count > 0 Then
                        MessageBox.Show("ERROR: Duca Ya existe en este Correlativo, vea el Detalle")
                        Si = False
                        txtDuca.Focus()
                    End If
                End If
            End If

            If Si Then
                If txtBL.Text <> String.Empty And NuevoReg = True Then
                    dTable.Clear()
                    dTable = Datos.consulta_reader("SELECT ISNULL(bl_dbq, 0) bl_dbq FROM dbo.Detalle_Buque WHERE Correlativo_dbq='" & txtId.Text & "' AND bl_dbq= '" & txtBL.Text & "'")

                    If dTable.Rows.Count > 0 Then
                        MessageBox.Show("ERROR: BL Ya existe en este Correlativo, vea el Detalle")
                        Si = False
                        txtBL.Focus()
                    End If
                End If
            End If

        End If
        Return Si
    End Function
#End Region

#Region "Botones"
    Private Sub btnSalir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub

    'Private Sub btnNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Me.NuevoReg = True
    '    Me.Grabando = True

    '    btnAlta.Visible = True
    '    Botonos(False)
    '    Textos()
    'End Sub

    Private Sub btnBorrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBorrar.Click
        'Si es modo nuevo o Editar
        If btnBorrar.Text = "cancelar" Then
            grdData.DataSource = Nothing
            grdData.Rows.Clear()
            grdData.Columns.Clear()
            dTable.Clear()

            txtId.Text = ""
            txtBuque.Text = ""
            txtNomBuque.Text = ""

            txtId.Enabled = True
            txtBuque.Enabled = True

            Botonos(True)
            btnAlta.Visible = False
            btnBaja.Visible = False
            btnHabilitar.Visible = False
            btnBuscar.Enabled = True

            btnBorrar.Visible = False
            Me.Grabando = False

            Textos()

            'Textos()
            txtId.Focus()
            'txtBuque.Text = ""
            'txtNomBuque.Text = ""

        End If

        If btnBorrar.Text = "borrar" Then
            'LLamamos la rutina Eliminar
            Datos.consulta_non_query("DELETE FROM [dbo].[Detalle_Buque] WHERE Correlativo_dbq='" & txtId.Text & "'")

            'Refrescamos los dato de pantalla
            Consulta("")

            txtId.Text = ""
            txtBuque.Text = ""
            txtNomBuque.Text = ""
            txtId.Focus()
            'Botonos(True)

            txtId_Validating(txtId.Text, Nothing)
        End If
        'txtDescripcion.Focus()
    End Sub

    'Despliega pantalla para buscar el Cajero
    Private Sub BuscaCodigo()
        Dim stringSQL As String

        stringSQL = "SELECT DISTINCT [Correlativo_dbq] As Codigo, [buque_dbq] As Nombre, [viaje_buque_dbq], [fechaviaje_buque_dbq] FROM [dbo].[Detalle_Buque]"

        'Script para Filtrar el Grid
        Dim stringSqlFiltro As String = "SELECT [Correlativo_dbq] As Codigo,  [buque_dbq] As Nombre FROM [dbo].[Detalle_Buque] WHERE Correlativo_dbq "

        'Instancia Ayuda Cliente
        'Dim frmBuscar As New BuscaCodigo(stringSQL, stringSqlFiltro, "Clientes")
        'frmBuscar.ShowDialog()

        Dim frmBuscar As New BuscaCodigo(stringSQL, {"Código", "Buque", "Viaje", "FechaV", "75", "75", "75", "75"}, 4, stringSqlFiltro, "Detalles Buque")
        frmBuscar.ShowDialog()

        'Retorna los datos del Cajero selecionado y 
        'se posiciona en Serie Recibo
        txtId.Text = ""
        If frmBuscar.Codigo <> String.Empty Then
            'Consulta(frmBuscar.Codigo)
            txtId.Text = frmBuscar.Codigo
        End If
        txtId.Focus()
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        BuscaCodigo()
    End Sub

#End Region

#Region "Busque F2"
    'KeyDown, cuando presionamos F2 en el txtCodigo_Cajero
    'Funciona para que cuando presionamos dicha tecla nos muestre la ayuda.
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
            Case e.KeyCode.Equals(Keys.Down)

        End Select
    End Sub

    Private Sub txtProducto_KeyDown(sender As Object, e As KeyEventArgs) Handles txtProducto.KeyDown
        Select Case e.KeyCode
            Case Keys.F2
                BuscaProdu()
        End Select
    End Sub
#End Region

#Region "Presiona Tecla"
    Private Sub txtDuca_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtDuca.KeyPress,
                                                                                   txtMerma.KeyPress,
                                                                                   txtBL.KeyPress,
                                                                                   txtManifiesto.KeyPress,
                                                                                   cmbEstadoSAT.KeyPress,
                                                                                   txtBuque.KeyPress,
                                                                                   txtConsigna.KeyPress,
                                                                                   txtProducto.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then
            'Posicion en el nuevo Campo
            SendKeys.Send("{TAB}")
            e.Handled = True
        End If
    End Sub

    Private Sub txtId_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtId.KeyPress

        If e.KeyChar = ChrW(Keys.Enter) Then
            ' txtId_Validating(txtId.Text, Nothing)
            'Posicion en el nuevo Campo
            SendKeys.Send("{TAB}")
            e.Handled = True
        End If
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
    Private Sub BuscaConsigna()
        Dim stringSQL As String

        stringSQL = "SELECT id_csg As Codigo, nombre_csg As Nombre FROM [dbo].[consignatario]"

        'Script para Filtrar el Grid
        Dim stringSqlFiltro As String = "SELECT id_csg As Codigo, nombre_csg As Nombre FROM [dbo].[consignatario] WHERE id_csg "

        Dim frmBuscar As New BuscaCodigo(stringSQL, {"Código", "Nombre", "50", "200"}, 2, stringSqlFiltro, "Lista Consignatarios")
        frmBuscar.ShowDialog()

        'Retorna los datos del Cajero selecionado y 
        'se posiciona en Serie Recibo
        txtConsigna.Text = ""
        If frmBuscar.Codigo <> String.Empty Then
            txtConsigna.Text = frmBuscar.Codigo

            txtConsigna.Focus()
        End If

    End Sub
#End Region

#Region "ValidaBusquedas"
    Private Sub txtProducto_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtProducto.Validating
        Try
            'Nombre Proveedor
            dTable = Datos.consulta_reader("SELECT nombre_pro FROM dbo.Producto WHERE id_pro='" & txtProducto.Text  & "'")
            txtNomProducto.Text = dTable.Rows.Item(0).Item("nombre_pro")
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtBuque_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtBuque.Validating
        Try
            'Nombre Buque
            dTable = Datos.consulta_reader("SELECT fechaviaje_buq fecha, viaje_buq, nombre_buq FROM dbo.Buques WHERE Id_buq='" & txtBuque.Text & "'")
            txtNomBuque.Text = dTable.Rows.Item(0).Item("nombre_buq")
            txtViaje.Text = dTable.Rows.Item(0).Item("viaje_buq")
            dFechav.Value = dTable.Rows.Item(0).Item("fecha")
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtConsigna_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles txtConsigna.Validating
        Try
            'Nombre Proveedor
            'dTable.Clear()
            'If txtConsigna.Text <> String.Empty Then
            dTable = Datos.consulta_reader("SELECT nombre_csg FROM dbo.consignatario WHERE id_csg='" & txtConsigna.Text & "'")
            txtNomConsigna.Text = dTable.Rows.Item(0).Item("nombre_csg")

            'Para mostrar el Merma
            If Convert.ToDouble(txtMerma.Text) = 0.00 Then
                dTable = Datos.consulta_reader("Select porcentaje_mrm FROM [dbo].[porcentaje_merma]")
                txtMerma.Text = dTable.Rows.Item(0).Item("porcentaje_mrm")
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtId_Validating(sender As Object, e As EventArgs) Handles txtId.Validating

        If txtId.Text <> String.Empty Then
            Me.NuevoReg = True
            Me.Grabando = True

            btnBorrar.Visible = True

            btnAlta.Visible = True
            btnBaja.Visible = False
            btnHabilitar.Visible = False

            Botonos(False)
            Textos()
            btnAlta.Text = "Insertar"
            btnAlta.BackColor = Color.Green

            'Para mostrar el Merma
            If txtBuque.Text = String.Empty Then
                dTable = Datos.consulta_reader("Select Id_buq FROM [dbo].[buques] where  baja_buq = 'ABIERTO'")
                txtBuque.Text = dTable.Rows.Item(0).Item("Id_buq")
            End If

            LeerData(grdData)
        End If
    End Sub
#End Region

#Region "Detalle"
    '-Lista Detalles 
    Public Sub LeerData(ByRef grdData As DataGridView)
        Dim dv As New DataView

        'Se arma el script para grabar SQL
        Dim StringSql As String = <sqlExp>
                                        SELECT  [Correlativo_dbq],
                                                [linea_dbq],
                                                [buque_dbq],
                                                [viaje_buque_dbq],
                                                [fechaviaje_buque_dbq],
                                                [duca_dbq],
                                                [bl_dbq],
                                                [consignatario_dbq],
                                                [producto_dbq],
                                                [manifiesto_dbq],
                                                [activo_dbq]
                                          FROM [dbo].[Detalle_Buque]    
							              WHERE RTRIM(LTRIM(Correlativo_dbq)) = '<%= txtId.Text.Trim %>' 
                                  </sqlExp>.Value

        'Arma Select para Grid
        dv = Datos.consulta_dv(StringSql)

        grdData.DataSource = dv

        grdData.SelectionMode = DataGridViewSelectionMode.FullRowSelect  'Marca toda la Linea en el DataGridView
        grdData.MultiSelect = False  'Solo seleccinar una fila
        grdData.AllowUserToDeleteRows = False
        grdData.ScrollBars = ScrollBars.Vertical

        'Encabezado del Grid
        grdData.Columns(0).HeaderText = "CORRELATIVO"
        grdData.Columns(1).HeaderText = "LINEA"
        grdData.Columns(2).HeaderText = "BUQUE"
        grdData.Columns(3).HeaderText = "VIAJE"
        grdData.Columns(4).HeaderText = "FECHA"
        grdData.Columns(5).HeaderText = "DUCA"
        grdData.Columns(6).HeaderText = "BL"
        grdData.Columns(7).HeaderText = "CONSIGNATARIO"
        grdData.Columns(8).HeaderText = "PRODUCTO"
        grdData.Columns(9).HeaderText = "MANIFIESTO"
        grdData.Columns(10).HeaderText = "ESTATUS"

        'Encabezado del Grid Centrado
        For Each column As DataGridViewColumn In grdData.Columns
            column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        Next

        'Ancho de las columnas
        grdData.Columns(0).Width = 100
        grdData.Columns(1).Width = 70
        grdData.Columns(2).Width = 70
        grdData.Columns(3).Width = 70
        grdData.Columns(4).Width = 75
        grdData.Columns(5).Width = 150
        grdData.Columns(6).Width = 75
        grdData.Columns(7).Width = 100
        grdData.Columns(8).Width = 75
        grdData.Columns(9).Width = 150
        grdData.Columns(10).Width = 75

        grdData.ReadOnly = True
        grdData.ScrollBars = ScrollBars.Both

        If dv.Count > 0 Then
            ''Posición Primera Linea
            grdData.Rows(0).Selected = True
            grdData.CurrentCell = grdData.Rows(0).Cells(0)
        End If

        btnHabilitar.Enabled = True
    End Sub

    Private Sub grdData_DoubleClick(sender As Object, e As EventArgs) Handles grdData.DoubleClick

        'Limpia todos los datos 
        Textos()

        Dim Pusuario As String = LibreriaGeneral.pusuario

        Dim dRow As DataGridViewSelectedCellCollection = grdData.SelectedCells

        txtLinea.Text = dRow.Item(1).Value

        Consulta(txtLinea.Text)

        'Boton para Borrar 
        btnAlta.Visible = True
        btnBaja.Visible = True

        If Pusuario = "SUPERVISOR" Or Pusuario = "ADMINISTRADOR" Then
            btnHabilitar.Visible = True
        End If

        txtBL.Enabled = False
        txtProducto.Enabled = False

        If Convert.ToDouble(txtPesoBascula.Text) = 0 Then
            btnHabilitar.Visible = False

            txtDiferencia.Enabled = False
            txtManifiesto.Enabled = False
            txtPesoBascula.Enabled = False

            txtMerma.Enabled = False
            cmbEstado.Enabled = False
        Else
            If Pusuario = "SUPERVISOR" Or Pusuario = "ADMINISTRADOR" Then
                btnHabilitar.Visible = True

                txtDiferencia.Enabled = True
                txtManifiesto.Enabled = True
                txtPesoBascula.Enabled = True

                txtMerma.Enabled = True
                cmbEstado.Enabled = True
            End If
        End If
    End Sub

    Private Sub btnBaja_Click(sender As Object, e As EventArgs) Handles btnBaja.Click
        'LLamamos la rutina Eliminar
        If Convert.ToDouble(txtPesoBascula.Text) = 0 Then
            Datos.consulta_reader("DELETE FROM dbo.Detalle_Buque WHERE Correlativo_dbq='" & txtId.Text & "' AND linea_dbq= '" & txtLinea.Text & "'")

            LeerData(grdData)

            txtId_Validating(txtId.Text, Nothing)
            ' txtDuca.Focus()

            'Limpia todos los datos 
            'Textos()
            'txtId.Focus()
        Else
            MessageBox.Show("ERROR: El Bl no se puede Borrar, Ya tiene Pesajes")
        End If
    End Sub

    Private Sub btnHabilitar_Click(sender As Object, e As EventArgs) Handles btnHabilitar.Click

        'Se arma el script para grabar SQL
        Dim StringSql As String = <sqlExp>
                                         SELECT [duca_dbq],
                                                [consignatario_dbq],
								                [porcentaje_merma_dbq],
                                                [manifiesto_dbq],
                                                ISNULL([bascula_dbq],0) As [bascula_dbq],
                                                ISNULL([diferencia_dbq],0) As [diferencia_dbq],
                                                [autorizado_dbq],
                                                [activo_dbq]
                                          FROM [dbo].[Detalle_Buque] db
							              WHERE RTRIM(LTRIM(Correlativo_dbq)) = '<%= txtId.Text.Trim %>' 
                                          AND [linea_dbq] = '<%= txtLinea.Text %>' 
                                  </sqlExp>.Value

        dTable = Datos.consulta_reader(StringSql)

        Dim txtDucad As String = ""
        Dim txtConsignad As String = ""
        Dim txtMermad As String = ""
        Dim txtManifiestod As String = ""
        Dim txtDiferenciad As String = ""
        Dim txtPesoBasculad As String = ""
        Dim cmbEstadod As String = ""

        'Setea Campos
        For Each dRow As DataRow In dTable.Rows
            txtDucad = dRow.Item("duca_dbq").ToString()
            txtConsignad = dRow.Item("consignatario_dbq").ToString()
            txtMermad = dRow.Item("porcentaje_merma_dbq").ToString()
            txtManifiestod = dRow.Item("manifiesto_dbq").ToString()
            txtDiferenciad = dRow.Item("diferencia_dbq").ToString()
            txtPesoBasculad = dRow.Item("bascula_dbq").ToString()

            cmbEstadod = dRow.Item("activo_dbq").ToString()
        Next

        If txtDucad <> txtDuca.Text Then
            graba_log(txtDucad, txtDuca.Text, "Duca")
        End If

        If txtConsignad <> txtConsigna.Text Then
            graba_log(txtDucad, txtConsigna.Text, "Consignatario")
        End If

        If txtMermad <> txtMerma.Text Then
            graba_log(txtMermad, txtMerma.Text, "Merma")
        End If

        If txtManifiestod <> txtManifiesto.Text Then
            graba_log(txtManifiestod, txtManifiesto.Text, "Manifiesto")
        End If

        If txtDiferenciad <> txtDiferencia.Text Then
            graba_log(txtDiferenciad, txtDiferencia.Text, "Diferencia")
        End If

        If txtPesoBasculad <> txtPesoBascula.Text Then
            graba_log(txtPesoBasculad, txtPesoBascula.Text, "Peso_Bascula")
        End If

        If cmbEstadod <> cmbEstado.Text Then
            graba_log(cmbEstadod, cmbEstado.Text, "Estatus")
        End If

        NuevoReg = False
        NuevoRegSuper = 1
        btnGrabar_Click(Nothing, EventArgs.Empty)
    End Sub

    Private Sub graba_log(DatoAnterior As String, DatoActual As String, Pantalla As String)

        Dim CorrelaId As String = ""
        Dim Usuario As String = LibreriaGeneral.usuario
        Dim fechahora As DateTime = DateTime.Now
        Dim fechahoraFormat As String = fechahora.ToString("yyyyMMdd H:mm:ss")

        'Suma Correlativo
        dTable = Datos.consulta_reader("SELECT ISNULL(MAX(Id_con)+1,0) rMax FROM [dbo].[control_log]")

        If dTable.Rows.Count > 0 Then
            'Setea Campos
            For Each dRow As DataRow In dTable.Rows
                'Infomrativos
                CorrelaId = dRow.Item("rMax").ToString
            Next
        Else
            CorrelaId = "1"
        End If

        'Se arma el script para grabar SQL
        Dim StringSql As String = <sqlExp>
                                    INSERT INTO [dbo].[control_log] ([Id_con], 
                                                                [valor_anterior_con],
                                                                [valor_actual_con],
                                                                [pantalla_con],
                                                                [id_afectado_con],
                                                                [id_bl_con],
                                                                [id_linea_con],
                                                                [id_usua_úpdate_con],
                                                                [fecha_update_con])
                                                     VALUES('<%= CorrelaId %>',
                                                             '<%= DatoAnterior %>',  
                                                             '<%= DatoActual %>',
                                                             '<%= Pantalla %>',
                                                             '<%= txtId.Text %>',
                                                             '<%= txtBL.Text %>',
                                                             '<%= txtLinea.Text %>',
                                                             '<%= Usuario %>',
                                                             '<%= fechahoraFormat %>'
                                                          )
                                     </sqlExp>.Value

        'LLamamos la rutina para grabar y no muestra mensaje de Grabado
        Datos.consulta_non_queryDeta(StringSql)
    End Sub


#End Region
End Class