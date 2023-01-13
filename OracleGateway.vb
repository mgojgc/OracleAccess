Imports System.Data
Imports Oracle.ManagedDataAccess.Client
Public Class OracleGateway
    Private _objDR As OracleDataReader,
            _objConn As OracleConnection,
            _objCmd As OracleCommand


    Public Function GetSpecificValue(ByVal strSQL As String, ByVal strConnectionString As String)
        'returns a single output
        _objConn = New OracleConnection(strConnectionString)
        Try
            _objCmd = New OracleCommand(strSQL, _objConn)
            _objConn.Open()
            _objDR = _objCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If Not _objDR.Read() Then
                Return ""
            Else
                If IsDBNull(_objDR(0)) Then
                    Return ""
                Else
                    Return _objDR(0)
                End If
            End If

        Catch err As Exception
            Return ""

        Finally
            _objConn.Close()
            _objDR = Nothing
            _objConn = Nothing

        End Try


    End Function

    Public Function CheckExist(ByVal strSQL As String, ByVal strConnectionString As String) As Boolean
        _objConn = New OracleConnection(strConnectionString)
        Try
            _objCmd = New OracleCommand(strSQL, _objConn)
            _objConn.Open()
            _objDR = _objCmd.ExecuteReader(CommandBehavior.CloseConnection)
            If Not _objDR.Read() Then
                Return False
            Else
                If IsDBNull(_objDR(0)) Then
                    Return False
                Else
                    Return True

                End If
            End If

        Catch err As Exception
            Return False

        Finally
            _objConn.Close()
            _objDR = Nothing
            _objConn = Nothing
        End Try


    End Function

    Public Function GetMultiColumn(ByVal strSQL As String, ByVal strConnectionString As String) As String()
        'Retrieve single line data and returns as 1d array
        Dim myList As New List(Of String)(),
            i As Integer

        _objConn = New OracleConnection(strConnectionString)
        Try
            _objCmd = New OracleCommand(strSQL, _objConn)
            _objConn.Open()
            _objDR = _objCmd.ExecuteReader(CommandBehavior.CloseConnection)
            While _objDR.Read()
                For i = 0 To _objDR.FieldCount - 1
                    myList.Add(_objDR(i).ToString)
                Next
            End While
            GetMultiColumn = myList.ToArray

        Catch err As Exception
            myList.Add("Err: " & err.Message)
            GetMultiColumn = myList.ToArray
        Finally
            _objConn.Close()
            _objDR = Nothing
            _objConn = Nothing

        End Try


    End Function

    Function GetMultiRow(ByVal strSQL As String, ByVal strConnectionString As String) As String()
        'retrieve multiple rows and returns 1d array
        Dim myList As New List(Of String)()

        _objConn = New OracleConnection(strConnectionString)
        Try
            _objCmd = New OracleCommand(strSQL, _objConn)
            _objConn.Open()
            _objDR = _objCmd.ExecuteReader(CommandBehavior.CloseConnection)

            While _objDR.Read()
                myList.Add(_objDR(0).ToString)
            End While
            GetMultiRow = myList.ToArray

        Catch err As Exception
            myList.Add("Err: " & err.Message)
            GetMultiRow = myList.ToArray

        Finally
            _objConn.Close()
            _objDR = Nothing
            _objConn = Nothing
        End Try

    End Function
    Function GetMultiRecordColumn(ByVal strSQL As String, ByVal strConnectionString As String) As String()
        'Returns a dynamic 1d Array.. Columns are delimited by ~
        Dim strlist As String,
            myList As New List(Of String)(),
            i As Integer,
            y As Integer

        _objConn = New OracleConnection(strConnectionString)

        Try
            _objCmd = New OracleCommand(strSQL, _objConn)
            _objConn.Open()
            _objDR = _objCmd.ExecuteReader(CommandBehavior.CloseConnection)
            i = 0
            While _objDR.Read()
                strlist = ""
                For y = 0 To _objDR.FieldCount - 1
                    If y <> _objDR.FieldCount - 1 Then
                        strlist = strlist & If(IsDBNull(_objDR(y)), "", _objDR(y)) & "~"
                    Else
                        strlist = strlist & If(IsDBNull(_objDR(y)), "", _objDR(y))
                    End If
                Next
                myList.Add(strlist.ToString)
                i += 1
            End While
            GetMultiRecordColumn = myList.ToArray
        Catch err As Exception
            myList.Add("Err: " & err.Message)
            GetMultiRecordColumn = myList.ToArray

        Finally
            _objConn.Close()
            _objDR = Nothing
            _objConn = Nothing
        End Try

    End Function

    Public Function InputUpdate(ByVal strSQL As String, ByVal strConnectionString As String)
        'for Add/Delete/update Operation
        _objConn = New OracleConnection(strConnectionString)
        Try
            _objCmd = New OracleCommand(strSQL, _objConn)
            _objConn.Open()
            _objCmd.ExecuteNonQuery()
            Return True

        Catch e As Exception
            Return False

        Finally
            _objConn.Close()
            _objDR = Nothing
            _objConn = Nothing
        End Try
    End Function
End Class
