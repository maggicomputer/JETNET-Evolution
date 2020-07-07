' Class Name: clsGenericMethods
' Purpose: to hold all the methods for interfacing with the DA class
' Parameters: None
' Return: None
' Change Log
'   7/29/2008: Created Tom Jones
Imports System.Data.OleDb

Public Class clsGenericMethods

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
      ' declare a variable to hold the number of rows effected
      Dim intReturnedRows As Integer
      intReturnedRows = clsGenericMethodsDA.AddRecord(connectionString, aObject, boolAutoNumber)
      Return intReturnedRows
    Catch ex As Exception
      'MessageBox.Show("Error occured in clsGenericMethods. Method: AddRecord. Error: " & ex.Message)
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
      ' declare a variable to hold the number of rows effected
      Dim intReturnedRows As Integer
      intReturnedRows = clsGenericMethodsDA.DeleteRecord(connectionString, aObject)
      Return intReturnedRows
    Catch ex As Exception
      'MessageBox.Show("Error occured in clsGenericMethods. Method: DeleteRecord. Error: " & ex.Message)
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

      ' declare a variable to hold the number of rows effected
      Dim intReturnedRows As Integer
      intReturnedRows = clsGenericMethodsDA.UpdateRecord(connectionString, aObject)
      Return intReturnedRows

    Catch ex As Exception
      ' Let the user know what went wrong.
      MsgBox("An error occured in clsGenericMethods. Method: UpdateRecord. Error: " & ex.Message)
      Return Nothing
    End Try
  End Function
  ' Method Name: UpdateRecord
  ' Purpose: To update a existing record to the database
  ' Parameters: 
  '   object - an object to update (this is a class – i.e., clsCustomers, clsProducts, etc.)
  ' Returns: number of rows effected - should be one: integer
  ' Change Log
  '   7/29/2008: Created Tom Jones

  Public Shared Function GetRecord(ByVal connectionString As String, ByVal aObject As Object, ByVal ds As DataSet) As DataSet
    Try
      ds = clsGenericMethodsDA.GetRecord(connectionString, aObject, ds)
      Return ds

    Catch ex As Exception
      ' Let the user know what went wrong.
      MsgBox("An error occured in clsGenericMethods. Method: UpdateRecord. Error: " & ex.Message)
      Return Nothing
    End Try
  End Function
End Class
