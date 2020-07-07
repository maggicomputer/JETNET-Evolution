Option Strict On
Option Explicit On


' ***********************************************************************
' Class Comments Section
' ***********************************************************************
' Class name: clsClient_Transactions_Phone_Numbers
' Purpose: Configure all properties and methods for clsClient_Transactions_Phone_Numbers
' Parameters: None
' Return: None
' Change Log
'           08/03/2010 - Created By: Tom Jones


Public Class clsClient_Transactions_Phone_Numbers



  '**************************************************
  ' Private variable declarations
  '**************************************************
  Private intclitpnum_comp_id As Integer
  Private intclitpnum_contact_id As Integer
  Private intclitpnum_trans_id As Integer
  Private strclitpnum_type As String
  Private strclitpnum_number As String


  '**************************************************
  ' Setters and getters
  '**************************************************
  Public Property clitpnum_comp_id As Integer
    Get
      Return intclitpnum_comp_id
    End Get
    Set(ByVal Value As Integer)
      intclitpnum_comp_id = Value
    End Set
  End Property


  Public Property clitpnum_contact_id As Integer
    Get
      Return intclitpnum_contact_id
    End Get
    Set(ByVal Value As Integer)
      intclitpnum_contact_id = Value
    End Set
  End Property


  Public Property clitpnum_trans_id As Integer
    Get
      Return intclitpnum_trans_id
    End Get
    Set(ByVal Value As Integer)
      intclitpnum_trans_id = Value
    End Set
  End Property


  Public Property clitpnum_type As String
    Get
      Return strclitpnum_type
    End Get
    Set(ByVal Value As String)
      strclitpnum_type = Value
    End Set
  End Property


  Public Property clitpnum_number As String
    Get
      Return strclitpnum_number
    End Get
    Set(ByVal Value As String)
      strclitpnum_number = Value
    End Set
  End Property


  '**************************************************
  ' Constructors
  '**************************************************


  '   Default Constructor
  Public Sub New()
    clitpnum_comp_id = 0
    clitpnum_contact_id = 0
    clitpnum_trans_id = 0
    clitpnum_type = ""
    clitpnum_number = ""
  End Sub


  '   Parameter Based Constructor
  Public Sub New(ByVal aclitpnum_comp_id As Integer, ByVal aclitpnum_contact_id As Integer, ByVal aclitpnum_trans_id As Integer, ByVal aclitpnum_type As String, ByVal aclitpnum_number As String)
    clitpnum_comp_id = aclitpnum_comp_id
    clitpnum_contact_id = aclitpnum_contact_id
    clitpnum_trans_id = aclitpnum_trans_id
    clitpnum_type = aclitpnum_type
    clitpnum_number = aclitpnum_number
  End Sub




  ' ***********************************************************************
  ' Methods
  ' ***********************************************************************


  ' Method name: ClassInfo
  ' Purpose: to generate a string with all assigned parameters
  ' Parameters: Client_Transactions_Phone_Numbers
  ' Return: String with all assigned parameters
  ' Change Log
  '           08/03/2010 - Created By: Tom Jones
  Public Function ClassInfo(ByVal aClient_Transactions_Phone_Numbers As clsClient_Transactions_Phone_Numbers) As String
    Try
      Dim ClassInformation As String


      ClassInformation = " clitpnum_comp_id = " & aClient_Transactions_Phone_Numbers.clitpnum_comp_id & vbnewline & _
      " clitpnum_contact_id = " & aClient_Transactions_Phone_Numbers.clitpnum_contact_id & vbnewline & _
      " clitpnum_trans_id = " & aClient_Transactions_Phone_Numbers.clitpnum_trans_id & vbnewline & _
      " clitpnum_type = " & aClient_Transactions_Phone_Numbers.clitpnum_type & vbnewline & _
      " clitpnum_number = " & aClient_Transactions_Phone_Numbers.clitpnum_number & vbnewline



      ' return the string
      Return ClassInformation
    Catch ex As Exception
      MsgBox("Error occured in classInfo. Class: clsClient_Transactions_Phone_Numbers. Error:" & ex.Message)
      Return Nothing
    End Try
  End Function


End Class
