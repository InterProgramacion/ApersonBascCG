Imports System.Windows.Forms

Public Class MDIMenu
    Dim Datos As New DatosGenereral

    Private infoStrip As New ToolStrip()

    Private Sub BodegasToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BodegasToolStripMenuItem.Click
        If Datos.TieneAcceso("0101") Then
            Dim frmS As New Form
            frmS = frmClientes
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If

    End Sub

    Private Sub GruposToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GruposToolStripMenuItem.Click

        If Datos.TieneAcceso("0102") Then
            Dim frmS As New Form
            frmS = frmProducto
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

    Private Sub ToolStripButton8_Click(sender As Object, e As EventArgs) Handles ToolStripButton8.Click
        Me.Close()
    End Sub

    Private Sub LineasToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LineasToolStripMenuItem.Click

        If Datos.TieneAcceso("0104") Then
            Dim frmS As New Form
            frmS = frmTurnos
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

    Private Sub MarcasToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MarcasToolStripMenuItem.Click

        If Datos.TieneAcceso("0106") Then
            Dim frmS As New Form
            frmS = frmFlotillaCamiones
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

    Private Sub ListadoDePreciosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ListadoDePreciosToolStripMenuItem.Click

        If Datos.TieneAcceso("0107") Then
            Dim frmS As New Form
            frmS = frmContenedores
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click

        If Datos.TieneAcceso("0108") Then

            Dim frmS As New Form
            frmS = frmClaseCarca
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

    Private Sub MotivosDeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MotivosDeToolStripMenuItem.Click

        If Datos.TieneAcceso("0109") Then

            Dim frmS As New Form
            frmS = frmPaises
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

    Private Sub VendedoresToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VendedoresToolStripMenuItem.Click

        If Datos.TieneAcceso("0110") Then

            Dim frmS As New Form
            frmS = frmPesosMM
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

    Private Sub ToolStripButton4_Click(sender As Object, e As EventArgs) Handles ToolStripButton4.Click

        If Datos.TieneAcceso("0103") Then

            Dim frmS As New Form
            frmS = frmBuques
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

    
    Private Sub ArticulosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ArticulosToolStripMenuItem.Click

        If Datos.TieneAcceso("0111") Then

            Dim frmS As New Form
            frmS = frmUsuario
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

  
    Private Sub NumerosBasculasAutorizadaParaPesarToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NumerosBasculasAutorizadaParaPesarToolStripMenuItem.Click

        If Datos.TieneAcceso("0501") Then

            Dim frmS As New Form
            frmS = frmBasculas
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

    Private Sub AsignacionDeBasculasAUnaComputadoraToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AsignacionDeBasculasAUnaComputadoraToolStripMenuItem.Click

        If Datos.TieneAcceso("0502") Then
            Dim frmS As New Form
            frmS = frmActivarComputadora
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

    Private Sub ConfiguracionDeTransmisionDeDatosDeBasculaAPcToolStripMenuItem_Click(sender As Object, e As EventArgs)

        If Datos.TieneAcceso("0101") Then

            Dim frmS As New Form
            frmS = frmConfiguracionPuerto
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

    Private Sub CorrelativosDeTicketsOBoletasToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CorrelativosDeTicketsOBoletasToolStripMenuItem.Click

        If Datos.TieneAcceso("0502") Then

            Dim frmS As New Form
            frmS = frmCorrealtivo
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

    Private Sub CalendarioDeCalibracionesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CalendarioDeCalibracionesToolStripMenuItem.Click

        If Datos.TieneAcceso("0601") Then

            Dim frmS As New Form
            frmS = frmRegistroCertificado
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

    Private Sub RegistroDeCalibracionesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RegistroDeCalibracionesToolStripMenuItem.Click

        If Datos.TieneAcceso("0602") Then

            Dim frmS As New Form
            frmS = frmRegistroCalibracion
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

    Private Sub EntradasInventarioToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EntradasInventarioToolStripMenuItem.Click

        If Datos.TieneAcceso("0201") Then

            Dim frmS As New Form
            frmS = frmPesaje
            frmS.Owner = Me  '"Me" hace referencia al formulario principal
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

    'Teclas Rapidas HotKey
    Private Sub MDIMenu_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown

        Select Case e.KeyCode

            Case Keys.F2

                If Datos.TieneAcceso("0201") Then

                    Dim frmS As New Form
                    frmS = frmPesaje
                    frmS.Owner = Me  '"Me" hace referencia al formulario principal
                    frmS.Show()
                Else
                    MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
                End If

                'Case Keys.F3

                'sgBox("Has pulsado F3")

        End Select
    End Sub


    Private Sub MDIMenu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        infoStrip.Dock = DockStyle.Top
        infoStrip.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow
        infoStrip.BackColor = Color.LightGray
        infoStrip.RenderMode = ToolStripRenderMode.System

        Dim r As Rectangle = My.Computer.Screen.WorkingArea
        PicLogo.Location = New Point(r.Width - PicLogo.Width, r.Height - PicLogo.Height - BarraBottom.Height * 2)

        AsignaValores_Variables()

        Me.Controls.Add(infoStrip)
        infoStrip.BringToFront()

    End Sub

    Private Sub AsignaValores_Variables()
        Dim frmLogin As New Login
        frmLogin.txtUsuario.Text = ""
        frmLogin.txtContrasena.Text = ""
        frmLogin.ShowDialog()

        If frmLogin.Conectado = False Then
            'lblData.Visible = False
            End
        Else
            Dim lblSistema As New ToolStripLabel("Sistema V3.0")
            lblSistema.Image = My.Resources.cash_register
            lblSistema.ImageAlign = ContentAlignment.MiddleLeft
            lblSistema.TextAlign = ContentAlignment.MiddleLeft
            infoStrip.Items.Add(lblSistema)

            infoStrip.Items.Add(New ToolStripLabel(" | "))

            Dim lblUsuario As New ToolStripLabel("Usuario: " & LibreriaGeneral.usuario)
            lblUsuario.Image = My.Resources.cash_register
            lblUsuario.ImageAlign = ContentAlignment.MiddleLeft
            infoStrip.Items.Add(lblUsuario)

            infoStrip.Items.Add(New ToolStripLabel(" | "))

            Dim lblPC As New ToolStripLabel("PC: " & LibreriaGeneral.cCompu)
            lblPC.Image = My.Resources.display
            lblPC.ImageAlign = ContentAlignment.MiddleLeft
            infoStrip.Items.Add(lblPC)

            infoStrip.Items.Add(New ToolStripLabel(" | "))

            Dim lblBascula As New ToolStripLabel("Báscula: " & LibreriaGeneral.nBascu)
            lblBascula.Image = My.Resources.savecopy
            lblBascula.ImageAlign = ContentAlignment.MiddleLeft
            infoStrip.Items.Add(lblBascula)

            infoStrip.Items.Add(New ToolStripLabel(" | "))

            ' Empresa con icono de edificio/empresa
            Dim lblEmpresa As New ToolStripLabel("Empresa: " & LibreriaGeneral.gNombreEmpre)
            infoStrip.Items.Add(lblEmpresa)


            'txtUsuario.Text = "Usuario: " & LibreriaGeneral.usuario
            'txtBascula.Text = "Bascula: " & LibreriaGeneral.nBascu
            'txtPc.Text = "PC: " & LibreriaGeneral.cCompu
            'txtEmpresa.Text = "Empresa: " & LibreriaGeneral.gNombreEmpre
            'txtSistema.Text = "Sistema V3.0"


            '    Sst_PrincipalLbl_Caja.Text = d.CodCajero
            '    Sst_PrincipalLbl_Nombre.Text = DatosGenerales.NombreCaja
            '    Sst_PrincipalLbl_Ubicacion.Text = DatosGenerales.UbicacionCaja
            '    Sst_PrincipalLbl_Usuario.Text = "Usuario SAP " & DatosGenerales.CodigoUSAP & " - " & DatosGenerales.usuarioCajero
            '    Sst_Principallbl_DataBase.Text = "Data Base: " & DatosGenerales.cNomDBase
            '    Sst_Version.Text = "V2.4.5"
            '    DatosGenerales.CajaAbierta(DatosGenerales.CodCajero)

            '    lblData.Visible = True
            '    lblData.Text = frmLogin.DataConecta
        End If

    End Sub

    Private Sub TransporteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TransporteToolStripMenuItem.Click

        If Datos.TieneAcceso("0105") Then

            Dim frmS As New Form
            frmS = frmTransporte
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

    Private Sub PesajesRealizadoPorRangoDeFechaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PesajesRealizadoPorRangoDeFechaToolStripMenuItem.Click

        If Datos.TieneAcceso("0301") Then

            Dim frmS As New Form
            frmS = RepPorFecha
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

    Private Sub ConsultaDeBoleteaPesajeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConsultaDeBoleteaPesajeToolStripMenuItem.Click

        If Datos.TieneAcceso("0205") Then

            Dim frmS As New Form
            frmS = frmConsultaBoleta
            frmS.Owner = Me  '"Me" hace referencia al formulario principal
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

    Private Sub ListadosDeCatálogosToolStripMenuItem_Click(sender As Object, e As EventArgs)

            Dim frmS As New Form
            frmS = RepListadosCatalogos
            frmS.Show()
    End Sub

    Private Sub ToolStripMenuItem2_Click_1(sender As Object, e As EventArgs)

        Dim frms As New Form
        frms = Rview_Buques
        frms.Show()
    End Sub

    Private Sub ListadoClientesToolStripMenuItem_Click(sender As Object, e As EventArgs)

        Dim frms As New Form
        frms = Rview_Clientes
        frms.Show()
    End Sub

    Private Sub ListadoProductosToolStripMenuItem_Click(sender As Object, e As EventArgs)

        Dim frms As New Form
        frms = Rview_Productos
        frms.Show()
    End Sub

    Private Sub ListadoCamionesToolStripMenuItem_Click(sender As Object, e As EventArgs)

        Dim frms As New Form
        frms = Rview_Camiones
        frms.Show()
    End Sub

    Private Sub ListadoTransporteToolStripMenuItem_Click(sender As Object, e As EventArgs)

        Dim frms As New Form
        frms = Rview_Transporte
        frms.Show()
    End Sub

    Private Sub ListadoBasculasToolStripMenuItem_Click(sender As Object, e As EventArgs)

        Dim frms As New Form
        frms = Rview_Bascula
        frms.Show()
    End Sub

    Private Sub ListadoUsuariosToolStripMenuItem_Click(sender As Object, e As EventArgs)

        Dim frms As New Form
        frms = Rview_Usuarios
        frms.Show()
    End Sub

    Private Sub CierrePorUsuarioIngresoYAnulacionesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CierrePorUsuarioIngresoYAnulacionesToolStripMenuItem.Click

        If Datos.TieneAcceso("0401") Then

            Dim frms As New Form
            frms = frmCierreTurno
            frms.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub


    Private Sub ReportesCrystalToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReportesCrystalToolStripMenuItem.Click
        Dim frms As New Form
            frms = RepListadosCatalogos
            frms.Show()
    End Sub

    Private Sub ToolStripMenuItem3_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem3.Click

        If Datos.TieneAcceso("0202") Then

            Dim frms As New Form
            frms = frmPesajeEspecial
            frms.Owner = Me  '"Me" hace referencia al formulario principal
            frms.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

    Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click

        If Datos.TieneAcceso("0205") Then

            Dim frms As New Form
            frms = frmConsultaBoletaII
            frms.Owner = Me  '"Me" hace referencia al formulario principal
            frms.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

    Private Sub ToolStripButton5_Click(sender As Object, e As EventArgs) Handles ToolStripButton5.Click

        If Datos.TieneAcceso("0206") Then

            Dim frms As New Form
            frms = frmPesajeModifica
            frms.Owner = Me  '"Me" hace referencia al formulario principal
            frms.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

    Private Sub ReportesDeCierreToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReportesDeCierreToolStripMenuItem.Click

        If Datos.TieneAcceso("0402") Then

            Dim frmS As New Form
            frmS = RepCierreTurno
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

    Private Sub SalirDelSistemaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SalirDelSistemaToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub ModificaciónBoletaPesajeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ModificaciónBoletaPesajeToolStripMenuItem.Click
        If Datos.TieneAcceso("0206") Then

            Dim frms As New Form
            frms = frmPesajeModifica
            frms.Owner = Me  '"Me" hace referencia al formulario principal
            frms.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If

    End Sub

    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        If Datos.TieneAcceso("0101") Then

            Dim frmS As New Form
            frmS = frmProducto
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

    Private Sub SubGruposToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SubGruposToolStripMenuItem.Click
        If Datos.TieneAcceso("0103") Then

            Dim frmS As New Form
            frmS = frmBuques
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
        If Datos.TieneAcceso("0201") Then

            Dim frmS As New Form
            frmS = frmPesaje
            frmS.Owner = Me
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

    Private Sub ToolStripButton6_Click(sender As Object, e As EventArgs) Handles ToolStripButton6.Click

        If Datos.TieneAcceso("0302") Then
            Dim frmS As New Form
            frmS = RepAnalitico
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If


    End Sub

    Private Sub EstadisticaDePesajePorProductoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EstadisticaDePesajePorProductoToolStripMenuItem.Click
        If Datos.TieneAcceso("0302") Then
            Dim frmS As New Form
            frmS = RepAnalitico
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

    Private Sub ToolStripButton7_Click(sender As Object, e As EventArgs) Handles ToolStripButton7.Click
        If Datos.TieneAcceso("0301") Then

            Dim frmS As New Form
            frmS = RepPorFecha
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If

    End Sub

    Private Sub AnulacionDeBolegtaPesajeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AnulacionDeBolegtaPesajeToolStripMenuItem.Click
        If Datos.TieneAcceso("0103") Then

            Dim frmS As New Form
            frmS = frmAnulaBoleta
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

    Private Sub CierreDeTurnoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CierreDeTurnoToolStripMenuItem.Click
        If Datos.TieneAcceso("0302") Then
            Dim frmS As New Form
            frmS = frmCorteTurno
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

    Private Sub ConsigatariosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ConsigatariosToolStripMenuItem.Click
        If Datos.TieneAcceso("0101") Then

            Dim frmS As New Form
            frmS = frmConsignatario
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

    Private Sub PorcentajeMermasToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PorcentajeMermasToolStripMenuItem.Click
        If Datos.TieneAcceso("0602") Then

            Dim frmS As New Form
            frmS = frmParametro
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

    Private Sub IngresoDeBLsYConsignatariosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles IngresoDeBLsYConsignatariosToolStripMenuItem.Click
        If Datos.TieneAcceso("0201") Then

            Dim frmS As New Form
            frmS = frmControlBLs
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

    Private Sub PesajesRealizadoPorBLToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PesajesRealizadoPorBLToolStripMenuItem.Click
        If Datos.TieneAcceso("0301") Then

            Dim frmS As New Form
            frmS = RepPorFechaBL
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

    Private Sub ToolStripButton9_Click(sender As Object, e As EventArgs) Handles ToolStripButton9.Click
        If Datos.TieneAcceso("0201") Then

            Dim frmS As New Form
            frmS = frmControlBLs
            frmS.Owner = Me
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

    Private Sub ToolStripButton10_Click(sender As Object, e As EventArgs) Handles ToolStripButton10.Click
        If Datos.TieneAcceso("0301") Then

            Dim frmS As New Form
            frmS = RepPorFechaBL
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

    Private Sub InformeLogBLToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles InformeLogBLToolStripMenuItem.Click
        If Datos.TieneAcceso("0301") Then

            Dim frmS As New Form
            frmS = RepLogBL
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

    Private Sub CargasFlotillaCamionesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CargasFlotillaCamionesToolStripMenuItem.Click
        If Datos.TieneAcceso("0301") Then

            Dim frmS As New Form
            frmS = frmCarga_Placas
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

    Private Sub BodegasToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles BodegasToolStripMenuItem1.Click
        If Datos.TieneAcceso("0101") Then
            Dim frmS As New Form
            frmS = frmBodega
            frmS.Show()
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If

    End Sub

    Private Sub SincronizaciónDeDatosToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SincronizaciónDeDatosToolStripMenuItem.Click

        If Datos.TieneAcceso("0101") Then
            Dim dTableI As New DataTable
            Dim DatosI As New DatosGenereral()

            'Arma Cursor de SAP, Para impresion de Factura.
            Dim StringSql As String = <sqlExp>
                                        EXEC [dbo].[Sync_Datos]
                                  </sqlExp>.Value

            'Para mostrar el Dato
            dTableI = DatosI.consulta_reader(StringSql)

            MsgBox("El Proceso Termino Satisfactoriamente", MsgBoxStyle.Exclamation, "Sistema Baculas..")
        Else
            MsgBox("El Usuario no tiene Acceso a esta opcion !!", MsgBoxStyle.Exclamation, "Sistema Seguridad..")
        End If
    End Sub

    Private Sub CargaMasivaToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CargaMasivaToolStripMenuItem.Click
        Dim frmS As New Form
        frmS = frmCargaMasiva
        frmS.Owner = Me
        frmS.Show()
    End Sub
End Class
