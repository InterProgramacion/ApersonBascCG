Public Class frmArticulos
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

    Private Sub frmArticulos_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Arma el ComboBox
        Datos.cargar_combo(cmbGrupo, _
                          "Select Nombre_dep, Codigo_dep From [dbo].[icatalogodepartamentos] Order by 1", _
                          "Codigo_dep", "Nombre_dep")

        Datos.cargar_combo(cmbSubGrupo, _
                          "Select Nombre_sde, Codigo_sde From [dbo].[icatalogosubdepartamentos] Order by 1", _
                          "Codigo_sde", "Nombre_sde")

        Datos.cargar_combo(cmbLinea, _
                          "Select Nombre_lin, Codigo_lin From [dbo].[icatalogolineas] Order by 1", _
                          "Codigo_lin", "Nombre_lin")

        Datos.cargar_combo(cmbMarca, _
                          "Select Nombre_mar, Codigo_mar From [dbo].[icatalogomarcas] Order by 1", _
                          "Codigo_mar", "Nombre_mar")

        Datos.cargar_combo(cmbTipoArticulo, _
                          "Select Nombre_tar, Codigo_tar From [dbo].[icatalogotipart] Order by 1", _
                          "Codigo_tar", "Nombre_tar")

        Consulta("")
        Botonos(True)
    End Sub

    'Sub Modulo Botonos
    'Sirver para Muestra o Oculta los Botonos
    Private Sub Botonos(ByVal Habilita As Boolean)
        '-Botonos
        btnNuevo.Visible = Habilita
        btnBuscar.Enabled = Habilita
        btnSalir.Visible = Habilita
        btnPrimero.Visible = Habilita
        btnAnterior.Visible = Habilita
        btnSiguiente.Visible = Habilita
        btnUltimo.Visible = Habilita

        If Me.Grabando Then
            btnBorrar.Text = "Cancelar"
            btnGrabar.Text = "Grabar"
        Else
            btnGrabar.Text = "Actualizar"
            btnBorrar.Text = "Borrar"
        End If

        '-Campos
        txtId.Enabled = False
        txtCodigo.Enabled = Not Habilita
    End Sub

    Private Sub Textos()
        txtId.Text = ""
        txtId.Enabled = False
        txtCodigo.Text = ""
        txtDescripcion.Text = ""
        txtCodigoBarras.Text = ""
        txtCertificado.Text = ""
        txtTenencia.Text = ""
        txtCodigoFacbrica.Text = ""
        chkActivo.Checked = False
        chkEsServicio.Checked = False
        chkPNegativos.Checked = False
        chkNoAfectaIva.Checked = False
        chkCambiaDescripcion.Checked = False

        cmbGrupo.Text = ""
        cmbSubGrupo.Text = ""
        cmbTipoArticulo.Text = ""
        cmbLinea.Text = ""
        cmbMarca.Text = ""

        'Se posiciona para Iniciar 
        txtCodigo.Focus()
    End Sub

#End Region

#Region "Consulta"
    'Procedimiento para mostrar el Primer Registro
    Private Sub Consulta(ByVal _codigo As String)
        Try
            'Para mostrar el Dato
            If _codigo = String.Empty Then
                dTable = Datos.consulta_reader("SELECT TOP 1 * FROM dbo.icatalogoarticulos ORDER BY id_art")
            Else
                dTable = Datos.consulta_reader("SELECT * FROM dbo.icatalogoarticulos WHERE codigo_art='" & _codigo & "'")
            End If

            Dim nCod As String = ""

            'Setea Campos
            For Each dRow As DataRow In dTable.Rows
                txtId.Text = dRow.Item("id_art").ToString
                txtCodigo.Text = dRow.Item("codigo_art").ToString
                txtDescripcion.Text = dRow.Item("nombre_art").ToString
                txtCodigoBarras.Text = dRow.Item("refere_art").ToString
                txtCertificado.Text = dRow.Item("registro_art").ToString
                txtTenencia.Text = dRow.Item("tenenemp_art").ToString
                txtCodigoFacbrica.Text = dRow.Item("codigoFac_art").ToString
                chkActivo.Checked = dRow.Item("inacti_art").ToString
                chkEsServicio.Checked = dRow.Item("servicio_art").ToString
                chkPNegativos.Checked = dRow.Item("dejaneg_art").ToString
                chkNoAfectaIva.Checked = dRow.Item("noiva_art").ToString
                chkCambiaDescripcion.Checked = dRow.Item("pddesfac_art").ToString
                cmbGrupo.SelectedValue = dRow.Item("depto_art").ToString
                cmbSubGrupo.SelectedValue = dRow.Item("sdept_art").ToString
                cmbTipoArticulo.SelectedValue = dRow.Item("articu_art").ToString
                cmbLinea.SelectedValue = dRow.Item("linea_art").ToString
                cmbMarca.SelectedValue = dRow.Item("marca_art").ToString
            Next

            'Asigna Linea Actual
            Me.LineaActual = dTable.Rows.Item(0).Item("id_art")

            Me.Grabando = False
            Navega()

        Catch ex As Exception
            MessageBox.Show("ERROR: " & ex.Message, "Comprobar tabla * icatalogoarticulos")
        End Try
    End Sub

    'Para Hacer Navegar
    Private Sub Navega()
        'Contar Lineas
        dTable = Datos.consulta_reader("SELECT Count(1) As TotRec FROM dbo.icatalogoarticulos")
        Me.TotLineas = dTable.Rows.Item(0).Item("TotRec")

        'Primera Linea
        dTable = Datos.consulta_reader("SELECT TOP 1 id_art FROM dbo.icatalogoarticulos ORDER BY id_art")
        Me.PrimLinea = dTable.Rows.Item(0).Item("id_art")

        'Ultima Linea
        dTable = Datos.consulta_reader("SELECT TOP 1 id_art FROM dbo.icatalogoarticulos ORDER BY id_art DESC")
        Me.UltiLinea = dTable.Rows.Item(0).Item("id_art")

        'Verifica si hay registros
        If Me.TotLineas = 0 Then
            Me.btnPrimero.Enabled = False
            Me.btnAnterior.Enabled = False
            Me.btnSiguiente.Enabled = False
            Me.btnUltimo.Enabled = False
        End If

        'Para Control de Botones Navegar
        If Me.PrimLinea = Me.LineaActual Then
            btnAnterior.Enabled = False
            btnPrimero.Enabled = False
        Else
            btnAnterior.Enabled = True
            btnPrimero.Enabled = True
        End If

        If Me.UltiLinea = Me.LineaActual Then
            btnSiguiente.Enabled = False
            btnUltimo.Enabled = False
        Else
            btnSiguiente.Enabled = True
            btnUltimo.Enabled = True
        End If
    End Sub
#End Region

#Region "Grabar"
    Private Sub btnGrabar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGrabar.Click

        If ValidaCampos() Then 'Si es verdadero
            If NuevoReg = True Then
                'Se arma el script para grabar SQL
                Dim StringSql As String = <sqlExp>
                                            INSERT INTO [dbo].[icatalogoarticulos]
                                                       ([empresa_art]
                                                       ,[nombre_art]
                                                       ,[codigo_art]
                                                       ,[depto_art]
                                                       ,[sdept_art]
                                                       ,[linea_art]
                                                       ,[refere_art]
                                                       ,[articu_art]
                                                       ,[marca_art]
                                                       ,[servicio_art]
                                                       ,[pddesfac_art]
                                                       ,[noiva_art]
                                                       ,[dejaneg_art]
                                                       ,[inacti_art]
                                                       ,[codigoFac_art]
                                                       ,[registro_art]
                                                       ,[tenenemp_art])
                                                 VALUES
                                                       ('<%= Empresa %>',
                                                        '<%= txtDescripcion.Text %>',
                                                        '<%= txtCodigo.Text %>',
                                                        '<%= cmbGrupo.SelectedValue %>',
                                                        '<%= cmbSubGrupo.SelectedValue %>',
                                                        '<%= cmbLinea.SelectedValue %>',
                                                        '<%= txtCodigoBarras.Text %>',
                                                        '<%= cmbTipoArticulo.SelectedValue %>',
                                                        '<%= cmbMarca.SelectedValue %>',
                                                        <%= IIf(chkEsServicio.Checked, 1, 0) %>,
                                                        <%= IIf(chkCambiaDescripcion.Checked, 1, 0) %>,  
                                                        <%= IIf(chkNoAfectaIva.Checked, 1, 0) %>,
                                                        <%= IIf(chkPNegativos.Checked, 1, 0) %>,
                                                        <%= IIf(chkActivo.Checked, 1, 0) %>),
                                                        '<%= txtCodigoFacbrica.Text %>',
                                                        '<%= txtCertificado.Text %>',
                                                        '<%= txtTenencia.Text %>')
                                                </sqlExp>.Value

                'LLamamos la rutina para grabar
                Datos.consulta_non_query(StringSql)

            Else
                'Se arma el script para grabar SQL
                Dim StringSql As String = <sqlExp>
                                     UPDATE [dbo].[icatalogoarticulos] SET [Empresa_art]  =  '<%= Empresa %>',
                                                                           [nombre_art]   =  '<%= txtDescripcion.Text %>',
                                                                           [codigo_art]   =  '<%= txtCodigo.Text %>',
                                                                           [refere_art]   =  '<%= txtCodigoBarras.Text %>',
                                                                           [inacti_art]   =   <%= IIf(chkActivo.Checked, 1, 0) %>,
                                                                           [servicio_art] =   <%= IIf(chkEsServicio.Checked, 1, 0) %>,
                                                                           [dejaneg_art]  =   <%= IIf(chkPNegativos.Checked, 1, 0) %>,
                                                                           [noiva_art]    =   <%= IIf(chkNoAfectaIva.Checked, 1, 0) %>,
                                                                           [pddesfac_art] =   <%= IIf(chkCambiaDescripcion.Checked, 1, 0) %>,
                                                                           [depto_art]    =  '<%= cmbGrupo.SelectedValue %>',
                                                                           [sdept_art]    =  '<%= cmbSubGrupo.SelectedValue %>',       
                                                                           [articu_art]   =  '<%= cmbTipoArticulo.SelectedValue %>',
                                                                           [linea_art]    =  '<%= cmbLinea.SelectedValue %>',
                                                                           [marca_art]    =  '<%= cmbMarca.SelectedValue %>',
                                                                           [codigoFac_art]=  '<%= txtCodigoFacbrica.Text %>',
                                                                           [registro_art] =  '<%= txtCertificado.Text %>',
                                                                           [tenenemp_art] =  '<%= txtTenencia.Text %>'
                                      WHERE id_art = '<%= txtId.Text %>' 
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

        If txtCodigo.Text = String.Empty Then
            MessageBox.Show("ERROR: El Codigo no puede ser Blanco")
            txtCodigo.Focus()
        ElseIf txtDescripcion.Text = String.Empty Then
            MessageBox.Show("ERROR: La descripción no puede ser Blanco")
            txtDescripcion.Focus()
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
            Datos.consulta_non_query("DELETE FROM [dbo].[icatalogoarticulos] WHERE id_art='" & txtId.Text & "'")

            'Refrescamos los dato de pantalla
            Consulta("")
            Botonos(True)
        End If
        'txtDescripcion.Focus()
    End Sub

    'Despliega pantalla para buscar el Cajero
    Private Sub BuscaCodigo()
        Dim stringSQL As String

        stringSQL = "SELECT codigo_art As Codigo, Nombre_art As Nombre, Registro_art, TenenEmp_art FROM [dbo].[icatalogoarticulos]"

        'Script para Filtrar el Gridro
        Dim stringSqlFiltro As String = "SELECT codigo_art As Codigo, Nombre_art As Nombre FROM [dbo].[icatalogoarticulos] WHERE codigo_art "

        'Instancia Ayuda Cliente
        Dim frmBuscar As New BuscaCodigoA(stringSQL, stringSqlFiltro, "Articulos")
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
    Private Sub btnPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrimero.Click
        'Para Inicio
        dTable = Datos.consulta_reader("SELECT TOP 1 id_art, codigo_art FROM dbo.icatalogoarticulos ORDER BY id_art")

        For Each dRow As DataRow In dTable.Rows
            Me.LineaActual = dRow("id_art")

            Consulta(dRow("codigo_art").ToString)
        Next
    End Sub

    Private Sub btnAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnterior.Click
        'Para Anterior
        dTable = Datos.consulta_reader("SELECT TOP 1 id_art, codigo_art FROM dbo.icatalogoarticulos WHERE id_art <" & Me.LineaActual & "ORDER BY id_art DESC")

        For Each dRow As DataRow In dTable.Rows
            Me.LineaActual = dRow("id_art")

            Consulta(dRow("codigo_art").ToString)
        Next
    End Sub

    Private Sub btnSiguiente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSiguiente.Click
        'Para Sigiente
        dTable = Datos.consulta_reader("SELECT TOP 1 id_art, codigo_art FROM dbo.icatalogoarticulos WHERE id_art >" & Me.LineaActual & "ORDER BY id_art")

        For Each dRow As DataRow In dTable.Rows
            Me.LineaActual = dRow("id_art")

            Consulta(dRow("codigo_art").ToString)
        Next
    End Sub

    Private Sub btnUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUltimo.Click
        'Para Inicio
        dTable = Datos.consulta_reader("SELECT TOP 1 id_art, codigo_art FROM dbo.icatalogoarticulos ORDER BY id_art DESC")

        For Each dRow As DataRow In dTable.Rows
            Me.UltiLinea = dRow("id_art")
            Me.LineaActual = dRow("id_art")

            Consulta(dRow("codigo_art").ToString)
        Next

    End Sub
#End Region


End Class