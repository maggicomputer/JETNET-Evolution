Option Explicit On
Imports System.Data.OleDb


' Comment Section
' Module Name: mdlDatabaseUtilties
' Purpose: All the methods for interacting and connecting/ disconnecting from/to the database directly
' Parameters: None
' Change Log:
'   7/16/08: Created - Tom Jones

Module mdlDatabaseUtilties

  ' Method Name: openDatabase
  ' Purpose: Open the database connection
  ' Parameters: None
  ' Return: database connection
  ' Change Log:
  '   7/16/08: Created - Tom Jones

  Public Function openDatabase(ByVal connectionString As String) As OleDbConnection
    ' error handling
    Try
      ' get the connection string based on the database selected
      ' the getDBConnectionString function is located in mdlEncryption

      ' check to make syre the connection string was found
      If connectionString <> "" Then
        Dim dbConnection As New OleDbConnection(connectionString)

        ' open the database connection
        dbConnection.Open()
        ' return the connection
        Return dbConnection
      Else
        MsgBox("No database connection string found")
        Return Nothing
      End If

    Catch ex As Exception
      MsgBox("Error occured in clsDatabaseUtilities. Method: openDatabase. " & ex.Message)
      Return Nothing
    End Try

  End Function

  ' Method Name: closeDatabase
  ' Purpose: Close the database connection
  ' Parameters: database connection to be closed
  ' Return: nothing
  ' Change Log:
  '   7/16/08: Created - Tom Jones

  Public Sub closeDatabase(ByVal dbConn As OleDbConnection)
    ' error handling
    Try
      dbConn.Close()
    Catch ex As Exception
      MsgBox("Error occured in clsDatabaseUtilities. Method: closeDatabase. " & ex.Message)
    End Try

  End Sub
  ' Method Name: getSQLCommand
  ' Date Created: 8/25/05
  ' Created By: Tom Jones
  ' Purpose: To get the SQL command stored in the database
  ' Parameters: 
  '   dbConection - an open database connection
  '   DatabaseType - Type of database to connect to
  '   FormName - name of the form calling for the SQL string
  '   SQLCommand - Type of SQL Command (i.e., INSERT, DELETE, UPDATE, etc.
  ' Return: SQL string stored in the database that corresponds to the parameters
  ' Last Edit Date: 8/25/05
  ' Last Edit By: Tom Jones
  ' Change Log: 
  '   8/25/05 - Created - Tom Jones

  Public Function populateDataset(ByVal dbConnection As OleDbConnection, ByVal DatabaseType As String, ByVal formName As String, ByVal SQLCommand As String) As String
    Try

      ' define a variable to hold the sql string stored in tblSQL
      Dim ReturnSQLCommand As String

      ' build the sql string based on the parameters passed in
      Dim sqlStringToExecute As String

      sqlStringToExecute = "SELECT tblSQL.SQLString, tblSQL.FormName, tblSQL.CommandType, tblSQL.DatabaseType" & _
                   " FROM tblSQL" & _
                   " WHERE (((tblSQL.FormName)='" & formName & "') AND ((tblSQL.CommandType)='" & SQLCommand & "') AND ((tblSQL.DatabaseType)='" & DatabaseType & "'));"

      ' execute the passed in sql string
      Dim oda As New OleDbDataAdapter(sqlStringToExecute, dbConnection)
      Dim ds As New DataSet
      oda.Fill(ds, "tblSQL")

      ' get the sql statement from the tblSQL
      ' the first column retrieved is the SQL statement
      Dim dr As DataRow = ds.Tables("tblSQL").Rows(0)
      ReturnSQLCommand = dr.Item(0)

      ' clear memory
      oda.Dispose()
      ds.Dispose()

      ' return the SQL string that was stored in the database
      Return ReturnSQLCommand

    Catch ex As Exception
      MsgBox(ex.Message)
      Return Nothing
    End Try
  End Function
End Module
