'Imports Microsoft.Office.Core
'Imports Microsoft.Office.Interop

Public Class LibreriaGeneral

#Region "Variables Puplicas"

    'Codigo Cajero
    'Private Shared _CodCajero As String = String.Empty
    'Public Shared Property CodCajero() As String
    '    Get
    '        Return _CodCajero
    '    End Get
    '    Set(ByVal value As String)
    '        _CodCajero = value
    '    End Set
    'End Property

    Private Shared _nEmpresa As String = "01"
    Public Shared Property nEmpresa() As String
        Get
            Return _nEmpresa
        End Get
        Set(ByVal value As String)
            _nEmpresa = value
        End Set
    End Property

    'Variable jobTible en SAP
    'Public Shared Property posicion As String = "Caja"
    'Public Shared Property PorcentajeIva As Double = Math.Round(12.0 / 100, 4)
    'Public Shared Property EmpresaM As Integer = 4
    ''-------------}----------------------------------------------------------

    Public Property NomInforme As String = ""
    Public Property dParam As Date = Date.Now
    Public Property dParam2 As Date = Date.Now

    'Basculas
    Public Shared Property usuario As String = ""
    Public Shared Property nBascu As String = "0"
    Public Shared Property cCompu As String = ""
    Public Shared Property nBoleta As String = ""
    Public Shared Property nTipoI As String = ""
    Public Shared Property xMensaje As String = ""
    Public Shared Property pusuario As String = ""


    Public Shared Property cServer As String = ""
    Public Shared Property cBaseD As String = ""
    Public Shared Property cUsu As String = ""
    Public Shared Property cPass As String = ""
    Public Shared Property cPathReport As String = ""

    Public Shared Property N_Ticket As String = ""
    Public Shared Property _EstePeso As String = ""
    Public Shared Property gPrinter As String = ""

    Public Shared Property gPuertoCOM As String = ""
    Public Shared Property gBitsPorSegundo As Integer = 0

    Public Shared Property gEmpresa As String = ""
    Public Shared Property gNombreEmpre As String = ""

    Public Shared Property nBodega As String = " "
    Public Shared Property nombreBodega As String = " "
    Public Shared Property idEmpresa As Int16 = 0

    '*-----------------------------------------
#End Region

    'Sub para mostrar los mensajes estandarizados.
    Public Shared Sub Mensaje(ByVal strMensaje As String, ByVal strMensajeDetallado As String)
        MessageBox.Show(strMensaje & vbCrLf & "Detalle: " + strMensajeDetallado)
    End Sub


    Public Shared Function DescripMes(ByVal _Mes As String) As String
        Dim RegresaMes As String = ""

        Select Case CInt(_Mes)
            Case Is = 1
                RegresaMes = "Enero"
            Case Is = 2
                RegresaMes = "Febrero"
            Case Is = 3
                RegresaMes = "Marzo"
            Case Is = 4
                RegresaMes = "Abril"
            Case Is = 5
                RegresaMes = "Mayo"
            Case Is = 6
                RegresaMes = "Junio"
            Case Is = 7
                RegresaMes = "Julio"
            Case Is = 8
                RegresaMes = "Agosto"
            Case Is = 9
                RegresaMes = "Septiembre"
            Case Is = 10
                RegresaMes = "Octubre"
            Case Is = 11
                RegresaMes = "Noviembre"
            Case Is = 12
                RegresaMes = "Diciembre"
        End Select

        Return RegresaMes
    End Function
End Class
