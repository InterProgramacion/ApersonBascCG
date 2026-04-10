Imports System.Threading
Imports System.IO
Imports System.Text
'Imports System.IO.Ports
Imports TomaPesoLib
Imports System.IO.Ports

Public Class frmTomaPesaje
#Region "Variables Retorno"
    Private _Peso As String
    Public ReadOnly Property Peso() As String
        Get
            Return _Peso
        End Get
    End Property

    'Dim archBatch As String = My.Computer.FileSystem.CurrentDirectory & "\LogPeso.txt"
    Dim mySerialPort As New SerialPort(LibreriaGeneral.gPuertoCOM)
    Dim BufferSalida As Integer = 0
    Dim Datos As New DatosGenereral()
    Private WithEvents miBascula As Bascula  'Se instancia la DLL C#

#End Region

    Private Sub btnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click

        _Peso = txtTomaPeso.Text

        'mySerialPort.Close()

        '   If File.Exists(archBatch) Then
        '  File.Delete(archBatch)
        ' End If

        Me.Close()
    End Sub

    Private Sub frmTomaPesaje_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' Inicializar la báscula con el puerto COM adecuado
        miBascula = New Bascula(LibreriaGeneral.gPuertoCOM, LibreriaGeneral.gBitsPorSegundo) ' Asegúrate de poner el puerto correcto
        AddHandler miBascula.DatosRecibidos, AddressOf ActualizarPeso
        miBascula.Iniciar()

        'LeeBascula()

        'Borra Archivo temporal si Existiera    
        'If File.Exists(archBatch) Then
        ' File.Delete(archBatch)
        'End If

        'txtTomaPeso.Text = LibreriaGeneral._EstePeso


        'Llama la Camptura
        'CampuraPerso()
    End Sub


#Region "PesajeDll"
    Private Sub ActualizarPeso(ByVal data As String)
        ' Asegurarse de actualizar el Label en el hilo de la interfaz gráfica
        If InvokeRequired Then
            Invoke(New Action(Of String)(AddressOf ActualizarPeso), data)
        Else
            txtTomaPeso.Text = data
        End If
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        ' Detener la báscula al cerrar el formulario
        miBascula.Detener()
    End Sub

#End Region


    ''Pone Parametros para Pesaje
    'Private Sub CampuraPerso()
    '    Try

    '        'mySerialPort.BaudRate = 9600
    '        mySerialPort.BaudRate = CInt(LibreriaGeneral.gBitsPorSegundo)

    '        mySerialPort.Parity = Parity.None
    '        mySerialPort.StopBits = StopBits.One
    '        mySerialPort.DataBits = 8
    '        mySerialPort.Handshake = Handshake.None

    '        mySerialPort.ReadBufferSize = 10000
    '        mySerialPort.ReadTimeout = 100
    '        mySerialPort.WriteBufferSize = 10000
    '        mySerialPort.WriteTimeout = 100
    '        mySerialPort.RtsEnable = True
    '        mySerialPort.DtrEnable = True
    '        Thread.Sleep(100)

    '        Dim archBatch As String = My.Computer.FileSystem.CurrentDirectory & "\LogPeso.txt"

    '        AddHandler mySerialPort.DataReceived, AddressOf DataReceviedHandler

    '        mySerialPort.Open()

    '    Catch ex As Exception
    '        MsgBox(ex.Message, MsgBoxStyle.Critical)
    '    End Try
    'End Sub


    'Private Sub DataReceviedHandler(sender As Object, e As SerialDataReceivedEventArgs)
    '    Try
    '        Dim sp As SerialPort = CType(sender, SerialPort)
    '        Dim indata As String = sp.ReadExisting()
    '        Dim indataN As String = ""
    '        BufferSalida = BufferSalida + 1


    '        indataN = indata.Replace("""", "")
    '        indataN = indataN.Replace("?", "")
    '        indataN = indataN.Replace("0  ", "")
    '        indataN = indataN.Replace(" ", "")
    '        indataN = indataN.Replace("", "")
    '        indataN = indataN.Replace("C", "")
    '        indataN = indataN.Replace(" C", "")
    '        indataN = indataN.Replace(" c", "")
    '        indataN = indataN.Replace("@", "")
    '        indataN = indataN.Replace(" @", "")

    '        My.Computer.FileSystem.WriteAllText(archBatch, indataN, True)

    '        If BufferSalida > 10 Then
    '            mySerialPort.Close()

    '            Dim fileReader As System.IO.StreamReader
    '            fileReader = My.Computer.FileSystem.OpenTextFileReader(archBatch)
    '            Dim Contenido As String
    '            'Contenido = fileReader.ReadLine()
    '            'Ciclo para ir validando linea por linea si no cumple con los datos
    '            Do
    '                Contenido = fileReader.ReadLine()
    '                If Not Contenido Is Nothing Then         '//Si no es nada 
    '                    If Contenido.Length <> 0 Then        '//Si tiene mas de un espacio 
    '                        If IsNumeric(Contenido) Then     '//Si es Numerico
    '                            If Val(Contenido) > 0 Then   '//Si es mayor que Cero

    '                                If Val(Contenido / 1000000) > 1 And Val(Contenido / 1000000) < 100000 Then
    '                                    txtTomaPeso.Text = Convert.ToString(Val(Contenido / 1000000))
    '                                    Exit Do  '//Salgo
    '                                Else
    '                                    txtTomaPeso.Text = "ERROR"
    '                                End If

    '                            End If
    '                        End If
    '                    End If
    '                End If
    '            Loop Until Contenido Is Nothing
    '            fileReader.Close()

    '            If File.Exists(archBatch) Then
    '                File.Delete(archBatch)
    '            End If

    '            'MessageBox.Show("El Peso: " & CInt(stringReader) / 1000000)

    '            'Me.Close()


    '        End If

    '    Catch ex As Exception
    '        MsgBox("Error: Vuelve a Pesar.." & ex.Message)
    '    End Try

    'End Sub


    Private Sub btnVolveraPesar_Click(sender As Object, e As EventArgs) Handles btnVolveraPesar.Click
        'Cancelar

        'CampuraPerso()

        _Peso = txtTomaPeso.Text

        'mySerialPort.Close()

        'If File.Exists(archBatch) Then
        '    File.Delete(archBatch)
        'End If

        Me.Close()
    End Sub

    'Private Sub LeeBascula()

    '    btnVolveraPesar.Visible = False
    '    txtTomaPeso.Text = ""

    '    txtTomaPeso.Text = Datos.LeePeso()

    '    If txtTomaPeso.Text = "Fallo" Then
    '        btnVolveraPesar.Visible = True
    '        btnAceptar.Visible = False
    '    Else
    '        btnVolveraPesar.Visible = False
    '        btnAceptar.Visible = True
    '    End If

    'End Sub
End Class