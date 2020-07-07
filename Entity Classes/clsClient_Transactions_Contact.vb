Option Strict On
Option Explicit On


' ***********************************************************************
' Class Comments Section
' ***********************************************************************
' Class name: clsclient_transactions_contact
' Purpose: Configure all properties and methods for clsclient_transactions_contact
' Parameters: None
' Return: None
' Change Log
'           08/03/2010 - Created By: Tom Jones


Public Class clsclient_transactions_contact



  '**************************************************
  ' Private variable declarations
  '**************************************************
  Private intclitcontact_id As Integer
  Private intclitcontact_trans_id As Integer
  Private intclitcontact_comp_id As Integer
  Private strclitcontact_sirname As String
  Private strclitcontact_first_name As String
  Private strclitcontact_middle_initial As String
  Private strclitcontact_last_name As String
  Private strclitcontact_suffix As String
  Private strclitcontact_title As String
  Private strclitcontact_email_address As String
  Private datclitcontact_action_date As System.DateTime


  '**************************************************
  ' Setters and getters
  '**************************************************
  Public Property clitcontact_id As Integer
    Get
      Return intclitcontact_id
    End Get
    Set(ByVal Value As Integer)
      intclitcontact_id = Value
    End Set
  End Property


  Public Property clitcontact_trans_id As Integer
    Get
      Return intclitcontact_trans_id
    End Get
    Set(ByVal Value As Integer)
      intclitcontact_trans_id = Value
    End Set
  End Property


  Public Property clitcontact_comp_id As Integer
    Get
      Return intclitcontact_comp_id
    End Get
    Set(ByVal Value As Integer)
      intclitcontact_comp_id = Value
    End Set
  End Property


  Public Property clitcontact_sirname As String
    Get
      Return strclitcontact_sirname
    End Get
    Set(ByVal Value As String)
      strclitcontact_sirname = Value
    End Set
  End Property


  Public Property clitcontact_first_name As String
    Get
      Return strclitcontact_first_name
    End Get
    Set(ByVal Value As String)
      strclitcontact_first_name = Value
    End Set
  End Property


  Public Property clitcontact_middle_initial As String
    Get
      Return strclitcontact_middle_initial
    End Get
    Set(ByVal Value As String)
      strclitcontact_middle_initial = Value
    End Set
  End Property


  Public Property clitcontact_last_name As String
    Get
      Return strclitcontact_last_name
    End Get
    Set(ByVal Value As String)
      strclitcontact_last_name = Value
    End Set
  End Property


  Public Property clitcontact_suffix As String
    Get
      Return strclitcontact_suffix
    End Get
    Set(ByVal Value As String)
      strclitcontact_suffix = Value
    End Set
  End Property


  Public Property clitcontact_title As String
    Get
      Return strclitcontact_title
    End Get
    Set(ByVal Value As String)
      strclitcontact_title = Value
    End Set
  End Property


  Public Property clitcontact_email_address As String
    Get
      Return strclitcontact_email_address
    End Get
    Set(ByVal Value As String)
      strclitcontact_email_address = Value
    End Set
  End Property


  Public Property clitcontact_action_date As System.DateTime
    Get
      Return datclitcontact_action_date
    End Get
    Set(ByVal Value As System.DateTime)
      datclitcontact_action_date = Value
    End Set
  End Property


  '**************************************************
  ' Constructors
  '**************************************************


  '   Default Constructor
  Public Sub New()
    clitcontact_id = 0
    clitcontact_trans_id = 0
    clitcontact_comp_id = 0
    clitcontact_sirname = ""
    clitcontact_first_name = ""
    clitcontact_middle_initial = ""
    clitcontact_last_name = ""
    clitcontact_suffix = ""
    clitcontact_title = ""
    clitcontact_email_address = ""
    clitcontact_action_date = System.DateTime.Now.ToLocalTime
  End Sub


  '   Parameter Based Constructor
  Public Sub New(ByVal aclitcontact_id As Integer, ByVal aclitcontact_trans_id As Integer, ByVal aclitcontact_comp_id As Integer, ByVal aclitcontact_sirname As String, ByVal aclitcontact_first_name As String, ByVal aclitcontact_middle_initial As String, ByVal aclitcontact_last_name As String, ByVal aclitcontact_suffix As String, ByVal aclitcontact_title As String, ByVal aclitcontact_email_address As String, ByVal aclitcontact_action_date As System.DateTime)
    clitcontact_id = aclitcontact_id
    clitcontact_trans_id = aclitcontact_trans_id
    clitcontact_comp_id = aclitcontact_comp_id
    clitcontact_sirname = aclitcontact_sirname
    clitcontact_first_name = aclitcontact_first_name
    clitcontact_middle_initial = aclitcontact_middle_initial
    clitcontact_last_name = aclitcontact_last_name
    clitcontact_suffix = aclitcontact_suffix
    clitcontact_title = aclitcontact_title
    clitcontact_email_address = aclitcontact_email_address
    clitcontact_action_date = aclitcontact_action_date
  End Sub




  ' ***********************************************************************
  ' Methods
  ' ***********************************************************************


  ' Method name: ClassInfo
  ' Purpose: to generate a string with all assigned parameters
  ' Parameters: client_transactions_contact
  ' Return: String with all assigned parameters
  ' Change Log
  '           08/03/2010 - Created By: Tom Jones
  Public Function ClassInfo(ByVal aclient_transactions_contact As clsclient_transactions_contact) As String
    Try
      Dim ClassInformation As String


      ClassInformation = " clitcontact_id = " & aclient_transactions_contact.clitcontact_id & vbnewline & _
      " clitcontact_trans_id = " & aclient_transactions_contact.clitcontact_trans_id & vbnewline & _
      " clitcontact_comp_id = " & aclient_transactions_contact.clitcontact_comp_id & vbnewline & _
      " clitcontact_sirname = " & aclient_transactions_contact.clitcontact_sirname & vbnewline & _
      " clitcontact_first_name = " & aclient_transactions_contact.clitcontact_first_name & vbnewline & _
      " clitcontact_middle_initial = " & aclient_transactions_contact.clitcontact_middle_initial & vbnewline & _
      " clitcontact_last_name = " & aclient_transactions_contact.clitcontact_last_name & vbnewline & _
      " clitcontact_suffix = " & aclient_transactions_contact.clitcontact_suffix & vbnewline & _
      " clitcontact_title = " & aclient_transactions_contact.clitcontact_title & vbnewline & _
      " clitcontact_email_address = " & aclient_transactions_contact.clitcontact_email_address & vbnewline & _
      " clitcontact_action_date = " & aclient_transactions_contact.clitcontact_action_date & vbnewline



      ' return the string
      Return ClassInformation
    Catch ex As Exception
      MsgBox("Error occured in classInfo. Class: clsclient_transactions_contact. Error:" & ex.Message)
      Return Nothing
    End Try
  End Function


End Class
