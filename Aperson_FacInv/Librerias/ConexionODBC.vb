Imports System.Data
Imports System.Data.Odbc

Module ConexionODBC

    'Publicas para conectarme a los ODBC reportes
    Public conectarme As New OdbcConnection
    Public dataset, dat As New DataSet
    Public adapter, ada As New OdbcDataAdapter
    Public stringSQLData, buscar As String

    Public Sub conexiones()

        Dim ConfigCE As New appConfigEditor("ConfigSQL.xml")
        Dim str_conexion As String = ConfigCE.getAppSettingValue("SQLPathODBC").ToString

        conectarme.ConnectionString = str_conexion
        conectarme.Open()
    End Sub
End Module