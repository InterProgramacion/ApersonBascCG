Public Class frmCierreTurno

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

    Private Sub frmCierreTurno_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Arma el ComboBox
        Datos.cargar_combo(cmbUsuario, _
                           "Select nombre_usu, Log_usu From [dbo].[Usuario] WHERE baja_usu = 'ACTIVO' Order by 1", _
                           "Log_usu", "nombre_usu")


    End Sub

    Private Sub Cierre()
        Try

            Dim Fecha1 As DateTime = dFechaIni.Value
            Dim Fecha2 As DateTime = dFechaFin.Value
            Dim usuari As String = cmbUsuario.SelectedItem("Log_usu").ToString.Trim

            'Para mostrar el Dato
            'Arma Cursor de SAP, Para impresion de Factura.
            Dim StringSql As String = <sqlExp>
                                         Select id_pes,ticketboleta_pes,fecha_insert_pes 
                                        FROM pesajes
                                        WHERE convert(date, fecha_insert_pes,103) BETWEEN '<%= Format(Fecha1, "yyyyMMdd") %>' AND '<%= Format(Fecha2, "yyyyMMdd") %>'
                                        AND id_usu_insert_pes = '<%= usuari %>'
                                  </sqlExp>.Value


            ''Para mostrar el Dato
            dTable = Datos.consulta_reader(StringSql)

            Dim nCod As String = ""

            ''Setea Campos
            For Each dRow As DataRow In dTable.Rows

                'llamamos la rutina para grabar
                Datos.consulta_non_queryDeta("UPDATE [dbo].[pesajes] SET  [cierreTurno_pes] = 'SI' WHERE  id_pes=" & dRow.Item("id_pes").ToString)
            Next


        Catch ex As Exception
            MessageBox.Show("ERROR: " & ex.Message, "Comprobar tabla * FCamiones")
        End Try
        
            MsgBox("La operacion se realizo con exito!", MsgBoxStyle.Information, "Operacion exitosa!")
       
    End Sub
  
    Private Sub btnGrabar_Click(sender As Object, e As EventArgs) Handles btnGrabar.Click
        Cierre()
    End Sub

    Private Sub btnSalir_Click(sender As Object, e As EventArgs) Handles btnSalir.Click
        Me.Close()
    End Sub
End Class