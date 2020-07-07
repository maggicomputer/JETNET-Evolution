' Class Name: clsGenericMethodsDA
' Purpose: to interface with the database
' Parameters: None
' Return: None
' Change Log
'   7/29/2008: Created Tom Jones

Imports System.Data.OleDb

Public Class clsGenericMethodsDA

  ' Method Name: AddRecord
  ' Purpose: add a record to the database
  ' Parameters:
  '   object - an object to add
  '   boolean -   determines whether we are adding to an autonumber field
  '               if so, skip the first field. TRUE means skip the first field
  ' Return: integer - number of rows effected by the add. Should be 1
  ' Change Log
  '   7/29/2008: Created Tom Jones

  Public Shared Function AddRecord(ByVal connectionString As String, ByVal aObject As Object, ByVal boolAutoNumber As Boolean) As Integer
    Try
      ' declare a variable to hold the auto number boolean value
      Dim boolSaveAutoNumber As Boolean = boolAutoNumber

      ' declare a variable to hold the results of the add
      Dim intReturnedRows As Integer = 0

      ' declare a data adapter
      Dim oda As New OleDbDataAdapter

      ' declare a command for the data adapter
      Dim dbCommand As New OleDbCommand

      ' connect to the database
      Dim dbConnect As OleDbConnection = openDatabase(connectionString)

      ' check the connection
      If dbConnect Is Nothing Then
        Return Nothing
      End If

      ' get the class type
      Dim classType As Type = aObject.GetType

      ' get the class name from the properties of the type
      Dim className As String = classType.Name

      ' truncates the class and appends the tbl prefix
      Dim strTableName As String = Right(className, Len(className) - 3)

      ' SQL Insert

      Dim SQLString As String = "INSERT INTO " & strTableName & " ("

      ' get a list of all the properties of the object
      Dim classPropertiesArray() As Reflection.PropertyInfo = classType.GetProperties

      ' declare an object type for each property
      Dim classProperty As Reflection.PropertyInfo

      ' get the total number of items in the array
      Dim propertyCount As Integer = classPropertiesArray.Length

      ' set a counter to prevent too many commas
      Dim counter As Integer = 0

      ' loop to go through the array
      For Each classProperty In classPropertiesArray
        ' check to see if there is an autonumber field
        If boolSaveAutoNumber = True Then
          ' skip the first field
          boolSaveAutoNumber = False
          counter = counter + 1
        Else
          ' gets the property name and appends it to the sql strong
          SQLString = SQLString & classProperty.Name

          ' increment the counter
          counter = counter + 1

          If counter < propertyCount Then
            ' check to see if it is the last property. if it is do NOT add a comma to the end
            SQLString = SQLString & ","
          End If
        End If
      Next

      ' terminate the SQL string
      SQLString = SQLString & ") VALUES ("

      ' reset the control variables
      counter = 0
      boolSaveAutoNumber = boolAutoNumber

      ' loop to go through the array
      ' add in ? marks to allow for data typing automatically
      For Each classProperty In classPropertiesArray
        ' check to see if there is an autonumber field
        If boolSaveAutoNumber = True Then
          ' skip the first field
          boolSaveAutoNumber = False
          counter = counter + 1
        Else
          SQLString = SQLString & "?"
          counter = counter + 1
          If counter < propertyCount Then
            ' check to see if it is the last property. if it is do NOT add a comma to the end
            SQLString = SQLString & ","
          End If
        End If
      Next

      ' terminate the sql string
      SQLString = SQLString & ")"

      boolSaveAutoNumber = boolAutoNumber
      ' declare an array to get the parameter values
      Dim parameterValue As Object
      For Each classProperty In classPropertiesArray
        ' check to see if there is an autonumber field
        If boolSaveAutoNumber = True Then
          ' skip the first field
          boolSaveAutoNumber = False
        Else
          parameterValue = classProperty.GetValue(aObject, Nothing)
          dbCommand.Parameters.AddWithValue("@" & classProperty.Name, parameterValue)
        End If

      Next

      ' set up the database commands
      dbCommand.CommandText = SQLString
      dbCommand.Connection = dbConnect
      oda.InsertCommand = dbCommand

      ' execute the query
      intReturnedRows = oda.InsertCommand.ExecuteNonQuery

      ' close the db connection
      closeDatabase(dbConnect)

      Return intReturnedRows

    Catch ex As Exception
      'MessageBox.Show("Error occured in clsGenericMethodsDA. Method: AddRecord. Error: " & ex.Message)
      Return 0
    End Try
  End Function

  ' Method Name: DeleteRecord
  ' Purpose: delete a record from the database
  ' Parameters:
  '   object - an object to delete
  ' Return: integer - number of rows effected by the add. Should be 1
  ' Change Log
  '   7/29/2008: Created Tom Jones

  Public Shared Function DeleteRecord(ByVal connectionString As String, ByVal aObject As Object) As Integer
    Try
      ' declare a variable to hold the results of the delete
      Dim intReturnedRows As Integer = 0

      ' declare a data adapter
      Dim oda As New OleDbDataAdapter

      ' declare a command for the data adapter
      Dim dbCommand As New OleDbCommand

      ' connect to the database
      Dim dbConnect As OleDbConnection = openDatabase(connectionString)

      ' check the connection
      If dbConnect Is Nothing Then
        Return Nothing
      End If

      ' get the class type
      Dim classType As Type = aObject.GetType

      ' get the class name from the properties of the type
      Dim className As String = classType.Name

      ' truncates the class and appends the tbl prefix
      Dim tableName As String = "tbl" & Right(className, Len(className) - 3)

      ' SQL DELETE
      Dim SQLString As String = "DELETE FROM " & tableName & " WHERE "

      ' get a list of all the properties of the object
      Dim classPropertiesArray() As Reflection.PropertyInfo = classType.GetProperties

      ' declare an object type for each property
      Dim classProperty As Reflection.PropertyInfo

      ' assume the first field/ property is the primary key field
      classProperty = classPropertiesArray(0)

      ' terminate the SQL string
      SQLString = SQLString & classProperty.Name & " = ?"

      ' declare an array to get the parameter values'
      Dim parameterValue As Object
      parameterValue = classProperty.GetValue(aObject, Nothing)
      dbCommand.Parameters.AddWithValue("@" & classProperty.Name, parameterValue)

      ' set up the database commands
      dbCommand.CommandText = SQLString
      dbCommand.Connection = dbConnect
      oda.DeleteCommand = dbCommand

      ' execute the query
      intReturnedRows = oda.DeleteCommand.ExecuteNonQuery

      ' close the db connection
      closeDatabase(dbConnect)

      Return intReturnedRows

    Catch ex As Exception
      'MessageBox.Show("Error occured in clsGenericMethodsDA. Method: DeleteRecord. Error: " & ex.Message)
      Return 0
    End Try
  End Function


  ' Method Name: UpdateRecord
  ' Purpose: To update a existing record to the database
  ' Parameters: 
  '   object - an object to update (this is a class – i.e., clsCustomers, clsProducts, etc.)
  ' Returns: number of rows effected - should be one: integer
  ' Change Log
  '   7/29/2008: Created Tom Jones

  Public Shared Function UpdateRecord(ByVal connectionString As String, ByVal aObject As Object) As Integer
    Try
      ' declare variable to hold the rows affected
      Dim returnedRows As Integer

      ' define the data adapter
      Dim oda As New OleDbDataAdapter

      ' define the command for the data adapter
      Dim dbComm As New OleDbCommand

      ' connect to the database
      Dim dbConn As OleDbConnection = openDatabase(connectionString)

      ' check the connection
      If dbConn Is Nothing Then
        Return Nothing
      End If

      ' **************************************************************
      ' SQL UPDATE - build string
      ' **************************************************************

      ' get the class name from the properties of the type
      Dim classType As Type = aObject.GetType()
      Dim className As String = classType.Name

      ' truncates the class and appends the tbl prefix
      Dim strTableName As String = "tbl" & Right(className, Len(className) - 3)

      Dim SQLString As String
      ' initialize the SQL string
      SQLString = "UPDATE " & strTableName & " SET "

      ' this gets a list of all the properties in the object
      ' and puts them into an array
      Dim classPropertiesArray() As Reflection.PropertyInfo = classType.GetProperties()

      ' now declare an object type to get each property
      Dim classProperty As Reflection.PropertyInfo

      ' get the total number of properties for the class
      Dim propertyCount As Integer = classPropertiesArray.Length
      Dim counter As Integer = 0
      Dim primaryKeyField As String = Nothing
      Dim primaryKey As String = ""
      ' declare a variable to hold the parameter value
      Dim parameterValue As Object = Nothing
      Dim PKparameterValue As Object = Nothing
      Dim primaryKeyProperty As String = Nothing
      Dim gotPK As Boolean = False

      ' this goes through each item in the array to get the specific attribute for that class
      For Each classProperty In classPropertiesArray
        ' if it the first property, assumption is that it is the primary key
        If counter = 0 Then
          primaryKeyField = classProperty.Name
          ' get the value of the property using the object passed in
          parameterValue = classProperty.GetValue(aObject, Nothing)
          dbComm.Parameters.Add(New OleDbParameter("@" & classProperty.Name, parameterValue))
          primaryKey = parameterValue
        Else
          ' get the value of the property using the object passed in
          parameterValue = classProperty.GetValue(aObject, Nothing)
          ' add the new parameter
          dbComm.Parameters.Add(New OleDbParameter("@" & classProperty.Name, parameterValue))
          'dbComm.Parameters.Add("@" & classProperty.Name, parameterValue)
          ' get the property name and append it to the SQL string
          SQLString = SQLString & classProperty.Name
          ' add a question mark and append it to the SQL string
          SQLString = SQLString & " = '" & parameterValue & "'"
          ' check to see if we need to add a comma to the string. 
          ' yes, if there are more properties in the array
          If counter < propertyCount - 1 Then
            SQLString = SQLString & ","
          End If
        End If

        counter = counter + 1
      Next

      ' terminate the SQL string
      SQLString = SQLString & " WHERE " & primaryKeyField & " = '" & primaryKey & "'"

      ' **************************************************************
      ' Add Parameters
      ' **************************************************************

      ' create and add the parameter as needed
      ' need to deal with the primary key being the last parameter to add
      ' For Each classProperty In classPropertiesArray
      ' get the primary key name and value and store them to add last
      'If Not gotPK Then
      ' still need to get the primary key
      'primaryKeyProperty = classProperty.Name
      'PKparameterValue = classProperty.GetValue(aObject, Nothing)
      'gotPK = True
      ' Else
      ' get the value of the property using the object passed in
      'parameterValue = classProperty.GetValue(aObject, Nothing)
      ' add the new parameter
      'dbComm.Parameters.Add(New OleDbParameter("@" & classProperty.Name, parameterValue))
      'dbComm.Parameters.Add("@" & classProperty.Name, parameterValue)
      '  End If
      'Next

      ' add the last parameter - primary key
      ' dbComm.Parameters.Add(New OleDbParameter("@" & primaryKeyProperty, PKparameterValue))
      ' dbComm.Parameters.Add("@" & primaryKeyProperty, PKparameterValue)

      ' **************************************************************
      '  Set up the database commands
      ' **************************************************************

      dbComm.CommandText = SQLString
      dbComm.Connection = dbConn
      oda.UpdateCommand = dbComm

      ' execute the query
      returnedRows = oda.UpdateCommand.ExecuteNonQuery

      ' close the database
      closeDatabase(dbConn)

      ' Return the number of rows affected
      Return returnedRows

    Catch ex As Exception
      ' Let the user know what went wrong.
      MsgBox("An error occured in clsGenericMethodsDA. Method: UpdateRecord. Error: " & ex.Message)
      Return Nothing
    End Try
  End Function
  ' Method Name: GetRecord
  ' Purpose: Get a record from the database
  ' Parameters:
  '   object - an object that holds all records
  ' Return: integer - number of rows effected by the add. Should be 1
  ' Change Log
  '   7/29/2008: Created Tom Jones

  Public Shared Function GetRecord(ByVal connectionString As String, ByVal aObject As Object, ByVal ds As DataSet) As DataSet
    Try
      Try
        ' declare variable to hold the rows affected
        Dim returnedRows As Integer = 0

        ' define the data adapter
        Dim oda As New OleDbDataAdapter


        ' define the command for the data adapter
        Dim dbComm As New OleDbCommand

        ' connect to the database
        Dim dbConn As OleDbConnection = openDatabase(connectionString)

        ' check the connection
        If dbConn Is Nothing Then
          Return Nothing
        End If

        ' **************************************************************
        ' SQL UPDATE - build string
        ' **************************************************************

        ' get the class name from the properties of the type
        Dim classType As Type = aObject.GetType()
        Dim className As String = classType.Name

        ' truncates the class and appends the tbl prefix
        Dim strTableName As String = Right(className, Len(className) - 3)

        Dim SQLString As String
        ' initialize the SQL string
        SQLString = "SELECT * FROM " & strTableName

        dbComm.CommandText = SQLString
        dbComm.Connection = dbConn

        ' execute the query
        ' returnedRows = oda.SelectCommand.ExecuteNonQuery
        oda.SelectCommand = dbComm

        oda.Fill(ds, strTableName)

      Catch ex As Exception
        ' Let the user know what went wrong.
        MsgBox("An error occured in clsGenericMethodsDA. Method: GetRecord. Error: " & ex.Message)
        Return Nothing
      End Try

    Catch ex As Exception
      'MessageBox.Show("Error occured in clsGenericMethodsDA. Method: GetRecord. Error: " & ex.Message)
      'Return 0
    End Try

    Return ds

  End Function

End Class



