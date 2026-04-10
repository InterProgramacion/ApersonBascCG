Public Class frmRegistroCalibracion

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
#End Region

#Region "Entorno"

    Private Sub frmRegistroCalibracion_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Arma el ComboBox
        Datos.cargar_combo(cmbBascula, _
                           "Select descripcion_bas, bascula_bas From [dbo].[basculas] Order by 1", _
                           "bascula_bas", "descripcion_bas")

        Consulta("")
        Botonos(True)

        Me.BackColor = Color.White
        Label4.ForeColor = Color.SeaGreen
    End Sub

    'Sub Modulo Botonos
    'Sirver para Muestra o Oculta los Botonos
    Private Sub Botonos(ByVal Habilita As Boolean)
        '-Botonos
        btnNuevo.Visible = Habilita
        btnBuscar.Enabled = Habilita
        btnSalir.visible = Habilita
        btnprimero.visible = Habilita
        btnanterior.visible = Habilita
        btnsiguiente.visible = Habilita
        btnultimo.visible = Habilita

        If Me.Grabando Then
            btnBorrar.text = "cancelar"
            btnGrabar.text = "grabar"
        Else
            btnGrabar.text = "actualizar"
            btnBorrar.text = "borrar"
        End If

        '-Campos
        txtId.Enabled = False

        '-Colores
        btnNuevo.BackColor = Color.SeaGreen
        btnBuscar.BackColor = Color.SeaGreen
        btnSalir.BackColor = Color.SeaGreen
        btnGrabar.BackColor = Color.SeaGreen
        btnBorrar.BackColor = Color.SeaGreen
    End Sub

    Private Sub Textos()
        txtId.Text = ""
        txtId.Enabled = False

        txtRegCPN.Text = ""
        txtProveedor.Text = ""
        txtAcreditado.Text = ""
        dpFechaCalibra.Value = Date.Now
        txtRegistro.Text = ""
        dpFechaProxima.Value = Date.Now
        txtDiasFalta.Text = ""

        'Se posiciona para Iniciar 
        txtRegCPN.Focus()
    End Sub

#End Region

#Region "Consulta"
    'Procedimiento para mostrar el Primer Registro
    Private Sub Consulta(ByVal _codigo As String)
        Try
            'Para mostrar el Dato
            If _codigo = String.Empty Then
                dTable = Datos.consulta_reader("SELECT TOP 1 * FROM dbo.registcalib ORDER BY Id_reg")
            Else
                dTable = Datos.consulta_reader("SELECT * FROM dbo.registcalib WHERE Id_reg=" & _codigo)
            End If

            Dim nCod As String = ""

            'Setea Campos
            For Each dRow As DataRow In dTable.Rows
                txtId.Text = dRow.Item("Id_reg").ToString
                txtRegCPN.Text = dRow.Item("cpn_reg").ToString
                cmbBascula.SelectedValue = dRow.Item("bascula_reg").ToString
                txtProveedor.Text = dRow.Item("empresaprov_reg").ToString
                txtAcreditado.Text = dRow.Item("acreditadooga_reg").ToString
                dpFechaCalibra.Value = dRow.Item("fechacalibra_reg").ToString
                txtRegistro.Text = dRow.Item("registro_reg").ToString
                dpFechaProxima.Value = dRow.Item("fechaproxima_reg").ToString
                txtDiasFalta.Text = dRow.Item("avisardias_reg").ToString
            Next

            'Asigna Linea Actual
            Me.LineaActual = dTable.Rows.Item(0).Item("Id_reg")

            Me.Grabando = False
            Me.NuevoReg = False
            Navega()

        Catch ex As Exception
            MessageBox.Show("ERROR: " & ex.Message, "Comprobar tabla * Usuario")
        End Try
    End Sub

    'Para Hacer Navegar
    Private Sub Navega()
        'Contar Lineas
        dTable = Datos.consulta_reader("SELECT Count(1) As TotRec FROM dbo.registcalib")
        Me.TotLineas = dTable.Rows.Item(0).Item("TotRec")

        'Primera Linea
        dTable = Datos.consulta_reader("SELECT TOP 1 Id_reg FROM dbo.registcalib ORDER BY Id_reg")
        Me.PrimLinea = dTable.Rows.Item(0).Item("Id_reg")

        'Ultima Linea
        dTable = Datos.consulta_reader("SELECT TOP 1 Id_reg FROM dbo.registcalib ORDER BY Id_reg DESC")
        Me.UltiLinea = dTable.Rows.Item(0).Item("Id_reg")

        'Verifica si hay registros
        If Me.TotLineas = 0 Then
            Me.btnprimero.Enabled = False
            Me.btnanterior.Enabled = False
            Me.btnsiguiente.Enabled = False
            Me.btnultimo.Enabled = False
        End If

        'Para Control de Botones Navegar
        If Me.PrimLinea = Me.LineaActual Then
            btnanterior.Enabled = False
            btnprimero.Enabled = False
        Else
            btnanterior.Enabled = True
            btnprimero.Enabled = True
        End If

        If Me.UltiLinea = Me.LineaActual Then
            btnsiguiente.Enabled = False
            btnultimo.Enabled = False
        Else
            btnsiguiente.Enabled = True
            btnultimo.Enabled = True
        End If
    End Sub
#End Region

#Region "Grabar"
    Private Sub btnGrabar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGrabar.Click

        Dim Fechahora As String = dpFechaCalibra.Value.ToString("yyyyMMdd H:mm:ss")
        Dim Fechahora2 As String = dpFechaProxima.Value.ToString("yyyyMMdd H:mm:ss")

        If ValidaCampos() Then 'Si es verdadero
            If NuevoReg = True Then


                'Suma Correlativo
                dTable = Datos.consulta_reader("SELECT MAX(Id_reg)+1 rMax FROM [dbo].[registcalib]")

                If dTable.Rows.Count > 0 Then
                    'Setea Campos
                    For Each dRow As DataRow In dTable.Rows
                        'Infomrativos
                        txtId.Text = dRow.Item("rMax").ToString
                    Next
                Else
                    txtId.Text = "1"
                End If



                'Se arma el script para grabar SQL
                Dim StringSql As String = <sqlExp>
                                        INSERT INTO [dbo].[registcalib] ([id_reg], [cpn_reg],
	                                                                     [empresaprov_reg],
	                                                                     [acreditadooga_reg],
	                                                                     [bascula_reg],
	                                                                     [fechacalibra_reg],
	                                                                     [registro_reg],
	                                                                     [fechaproxima_reg],
                                                                         [avisardias_reg],
                                                                         [empresa_reg],
                                                                         [sync_reg])
                                            VALUES('<%= txtId.Text %>', '<%= txtRegCPN.Text %>',
                                                  '<%= txtProveedor.Text %>',
                                                  '<%= txtAcreditado.Text %>',
                                                  '<%= cmbBascula.SelectedItem("bascula_bas").ToString %>',
                                                  '<%= Fechahora %>',
                                                  '<%= txtRegistro.Text %>',
                                                  '<%= Fechahora2 %>',
                                                  '<%= txtDiasFalta.Text %>',
                                                  '<%= LibreriaGeneral.gEmpresa %>',
                                                  1)
                                         </sqlExp>.Value

                'LLamamos la rutina para grabar
                Datos.consulta_non_query(StringSql)

            Else
                'Se arma el script para grabar SQL
                Dim StringSql As String = <sqlExp>
                                         UPDATE [dbo].[registcalib] SET [cpn_reg]  =  '<%= txtRegCPN.Text %>',
	                                                                     [empresaprov_reg]  =  '<%= txtProveedor.Text %>',
	                                                                     [acreditadooga_reg]  =  '<%= txtAcreditado.Text %>',
	                                                                     [bascula_reg]  =  '<%= cmbBascula.SelectedItem("bascula_bas").ToString %>',
	                                                                     [fechacalibra_reg]  =  '<%= Fechahora %>',
	                                                                     [registro_reg]  =  '<%= txtRegistro.Text %>',
                                                                         [fechaproxima_reg] = '<%= Fechahora2 %>',
                                                                         [avisardias_reg] = '<%= txtDiasFalta.Text %>',
                                                                         [sync_reg]       = 2
                                          WHERE Id_reg = '<%= txtId.Text %>' 
                                          AND [empresa_reg] = '<%= LibreriaGeneral.gEmpresa %>'
                                      </sqlExp>.Value

                'LLamamos la rutina para grabar
                Datos.consulta_non_query(StringSql)
            End If

            'Refrescamos los dato de pantalla
            Consulta("")
            Botonos(True)
            'txtDescripcion.Focus()
        End If
    End Sub

    Private Function ValidaCampos() As Boolean
        Dim Si As Boolean = False

        'If txtUsuario.Text = String.Empty Then
        '    MessageBox.Show("ERROR: Peso Minimo no puede quedar en Blanco")
        '    txtUsuario.Focus()
        'ElseIf txtNombre.Text = String.Empty Then
        '    MessageBox.Show("ERROR: Peso Maximo no puede quedar en Blanco")
        '    txtNombre.Focus()
        'ElseIf txtCfContraseña.Text = String.Empty Then
        '    MessageBox.Show("ERROR: Confirmación Contraseña no puede quedar en Blanco")
        '    txtCfContraseña.Focus()
        'ElseIf txtContraseña.Text = String.Empty Then
        '    MessageBox.Show("ERROR: Contraseña no puede quedar en Blanco")
        '    txtContraseña.Focus()
        'ElseIf txtContraseña.Text.Trim <> txtCfContraseña.Text.Trim Then
        '    MessageBox.Show("ERROR: Verificar Contraseña")
        '    txtCfContraseña.Focus()
        'Else
        Si = True
        'End If
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
        Botonos(False)
        Textos()
    End Sub

    Private Sub btnBorrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBorrar.Click
        'Si es modo nuevo o Editar
        If Me.Grabando Then
            Consulta("")
            Botonos(True)
        Else
            'LLamamos la rutina Eliminar
            Datos.consulta_non_query("DELETE FROM [dbo].[registcalib] WHERE Id_reg='" & txtId.Text & "'")

            'Refrescamos los dato de pantalla
            Consulta("")
            Botonos(True)
        End If
        'txtDescripcion.Focus()
    End Sub

    'Despliega pantalla para buscar el Cajero
    Private Sub BuscaCodigo()
        Dim stringSQL As String

        stringSQL = "SELECT Id_reg As Codigo, cpn_reg As Nombre, bascula_reg FROM [dbo].[registcalib]"

        'Script para Filtrar el Grid
        Dim stringSqlFiltro As String = "SELECT Id_reg As Codigo, cpn_reg As Nombre FROM [dbo].[registcalib] WHERE Id_reg "

        'Instancia Ayuda Cliente
        'Dim frmBuscar As New BuscaCodigo(stringSQL, stringSqlFiltro, "Clientes")
        'frmBuscar.ShowDialog()

        Dim frmBuscar As New BuscaCodigo(stringSQL, {"Código", "Registro", "Bascula", "50", "100", "100"}, 3, stringSqlFiltro, "Registros Calibracion")
        frmBuscar.ShowDialog()

        'Retorna los datos del Cajero selecionado y 
        'se posiciona en Serie Recibo
        txtId.Text = ""
        If frmBuscar.Codigo <> String.Empty Then
            Consulta(frmBuscar.Codigo)
        End If
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        BuscaCodigo()
    End Sub

#End Region

#Region "Botones Navegar"
    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnprimero.Click
        'Para Inicio
        dTable = Datos.consulta_reader("SELECT TOP 1 Id_reg FROM dbo.registcalib ORDER BY Id_reg")

        For Each dRow As DataRow In dTable.Rows
            Me.LineaActual = dRow("Id_reg")

            Consulta(dRow("Id_reg").ToString)
        Next
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnanterior.Click
        'Para Anterior
        dTable = Datos.consulta_reader("SELECT TOP 1 Id_reg FROM dbo.registcalib WHERE Id_reg <" & Me.LineaActual & "ORDER BY Id_reg DESC")

        For Each dRow As DataRow In dTable.Rows
            Me.LineaActual = dRow("Id_reg")

            Consulta(dRow("Id_reg").ToString)
        Next
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnsiguiente.Click
        'Para Sigiente
        dTable = Datos.consulta_reader("SELECT TOP 1 Id_reg FROM dbo.registcalib WHERE Id_reg >" & Me.LineaActual & "ORDER BY Id_reg")

        For Each dRow As DataRow In dTable.Rows
            Me.LineaActual = dRow("Id_reg")

            Consulta(dRow("Id_reg").ToString)
        Next
    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnultimo.Click
        'Para Inicio
        dTable = Datos.consulta_reader("SELECT TOP 1 Id_reg FROM dbo.registcalib ORDER BY Id_reg DESC")

        For Each dRow As DataRow In dTable.Rows
            Me.UltiLinea = dRow("Id_reg")
            Me.LineaActual = dRow("Id_reg")

            Consulta(dRow("Id_reg").ToString)
        Next

    End Sub
#End Region

   
End Class