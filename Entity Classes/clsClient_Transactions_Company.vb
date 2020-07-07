Option Strict On
Option Explicit On


' ***********************************************************************
' Class Comments Section
' ***********************************************************************
' Class name: clsClient_Transactions_Company
' Purpose: Configure all properties and methods for clsClient_Transactions_Company
' Parameters: None
' Return: None
' Change Log
'           08/03/2010 - Created By: Tom Jones


Public Class clsClient_Transactions_Company



  '**************************************************
  ' Private variable declarations
  '**************************************************
  Private intclitcomp_id As Integer
  Private intclitcomp_trans_id As Integer
  Private strclitcomp_name As String
  Private strclitcomp_alternate_name_type As String
  Private strclitcomp_alternate_name As String
  Private strclitcomp_address1 As String
  Private strclitcomp_address2 As String
  Private strclitcomp_city As String
  Private strclitcomp_state As String
  Private strclitcomp_zip_code As String
  Private strclitcomp_country As String
  Private strclitcomp_agency_type As String
  Private strclitcomp_web_address As String
  Private strclitcomp_email_address As String
  Private datclitcomp_action_date As System.DateTime


  '**************************************************
  ' Setters and getters
  '**************************************************
  Public Property clitcomp_id As Integer
    Get
      Return intclitcomp_id
    End Get
    Set(ByVal Value As Integer)
      intclitcomp_id = Value
    End Set
  End Property


  Public Property clitcomp_trans_id As Integer
    Get
      Return intclitcomp_trans_id
    End Get
    Set(ByVal Value As Integer)
      intclitcomp_trans_id = Value
    End Set
  End Property


  Public Property clitcomp_name As String
    Get
      Return strclitcomp_name
    End Get
    Set(ByVal Value As String)
      strclitcomp_name = Value
    End Set
  End Property


  Public Property clitcomp_alternate_name_type As String
    Get
      Return strclitcomp_alternate_name_type
    End Get
    Set(ByVal Value As String)
      strclitcomp_alternate_name_type = Value
    End Set
  End Property


  Public Property clitcomp_alternate_name As String
    Get
      Return strclitcomp_alternate_name
    End Get
    Set(ByVal Value As String)
      strclitcomp_alternate_name = Value
    End Set
  End Property


  Public Property clitcomp_address1 As String
    Get
      Return strclitcomp_address1
    End Get
    Set(ByVal Value As String)
      strclitcomp_address1 = Value
    End Set
  End Property


  Public Property clitcomp_address2 As String
    Get
      Return strclitcomp_address2
    End Get
    Set(ByVal Value As String)
      strclitcomp_address2 = Value
    End Set
  End Property


  Public Property clitcomp_city As String
    Get
      Return strclitcomp_city
    End Get
    Set(ByVal Value As String)
      strclitcomp_city = Value
    End Set
  End Property


  Public Property clitcomp_state As String
    Get
      Return strclitcomp_state
    End Get
    Set(ByVal Value As String)
      strclitcomp_state = Value
    End Set
  End Property


  Public Property clitcomp_zip_code As String
    Get
      Return strclitcomp_zip_code
    End Get
    Set(ByVal Value As String)
      strclitcomp_zip_code = Value
    End Set
  End Property


  Public Property clitcomp_country As String
    Get
      Return strclitcomp_country
    End Get
    Set(ByVal Value As String)
      strclitcomp_country = Value
    End Set
  End Property


  Public Property clitcomp_agency_type As String
    Get
      Return strclitcomp_agency_type
    End Get
    Set(ByVal Value As String)
      strclitcomp_agency_type = Value
    End Set
  End Property


  Public Property clitcomp_web_address As String
    Get
      Return strclitcomp_web_address
    End Get
    Set(ByVal Value As String)
      strclitcomp_web_address = Value
    End Set
  End Property


  Public Property clitcomp_email_address As String
    Get
      Return strclitcomp_email_address
    End Get
    Set(ByVal Value As String)
      strclitcomp_email_address = Value
    End Set
  End Property


  Public Property clitcomp_action_date As System.DateTime
    Get
      Return datclitcomp_action_date
    End Get
    Set(ByVal Value As System.DateTime)
      datclitcomp_action_date = Value
    End Set
  End Property


  '**************************************************
  ' Constructors
  '**************************************************


  '   Default Constructor
  Public Sub New()
    clitcomp_id = 0
    clitcomp_trans_id = 0
    clitcomp_name = ""
    clitcomp_alternate_name_type = ""
    clitcomp_alternate_name = ""
    clitcomp_address1 = ""
    clitcomp_address2 = ""
    clitcomp_city = ""
    clitcomp_state = ""
    clitcomp_zip_code = ""
    clitcomp_country = ""
    clitcomp_agency_type = ""
    clitcomp_web_address = ""
    clitcomp_email_address = ""
    clitcomp_action_date = System.DateTime.Now.ToLocalTime
  End Sub


  '   Parameter Based Constructor
  Public Sub New(ByVal aclitcomp_id As Integer, ByVal aclitcomp_trans_id As Integer, ByVal aclitcomp_name As String, ByVal aclitcomp_alternate_name_type As String, ByVal aclitcomp_alternate_name As String, ByVal aclitcomp_address1 As String, ByVal aclitcomp_address2 As String, ByVal aclitcomp_city As String, ByVal aclitcomp_state As String, ByVal aclitcomp_zip_code As String, ByVal aclitcomp_country As String, ByVal aclitcomp_agency_type As String, ByVal aclitcomp_web_address As String, ByVal aclitcomp_email_address As String, ByVal aclitcomp_action_date As System.DateTime)
    clitcomp_id = aclitcomp_id
    clitcomp_trans_id = aclitcomp_trans_id
    clitcomp_name = aclitcomp_name
    clitcomp_alternate_name_type = aclitcomp_alternate_name_type
    clitcomp_alternate_name = aclitcomp_alternate_name
    clitcomp_address1 = aclitcomp_address1
    clitcomp_address2 = aclitcomp_address2
    clitcomp_city = aclitcomp_city
    clitcomp_state = aclitcomp_state
    clitcomp_zip_code = aclitcomp_zip_code
    clitcomp_country = aclitcomp_country
    clitcomp_agency_type = aclitcomp_agency_type
    clitcomp_web_address = aclitcomp_web_address
    clitcomp_email_address = aclitcomp_email_address
    clitcomp_action_date = aclitcomp_action_date
  End Sub




  ' ***********************************************************************
  ' Methods
  ' ***********************************************************************


  ' Method name: ClassInfo
  ' Purpose: to generate a string with all assigned parameters
  ' Parameters: Client_Transactions_Company
  ' Return: String with all assigned parameters
  ' Change Log
  '           08/03/2010 - Created By: Tom Jones
  Public Function ClassInfo(ByVal aClient_Transactions_Company As clsClient_Transactions_Company) As String
    Try
      Dim ClassInformation As String


      ClassInformation = " clitcomp_id = " & aClient_Transactions_Company.clitcomp_id & vbNewLine & _
      " clitcomp_trans_id = " & aClient_Transactions_Company.clitcomp_trans_id & vbNewLine & _
      " clitcomp_name = " & aClient_Transactions_Company.clitcomp_name & vbNewLine & _
      " clitcomp_alternate_name_type = " & aClient_Transactions_Company.clitcomp_alternate_name_type & vbNewLine & _
      " clitcomp_alternate_name = " & aClient_Transactions_Company.clitcomp_alternate_name & vbNewLine & _
      " clitcomp_address1 = " & aClient_Transactions_Company.clitcomp_address1 & vbNewLine & _
      " clitcomp_address2 = " & aClient_Transactions_Company.clitcomp_address2 & vbNewLine & _
      " clitcomp_city = " & aClient_Transactions_Company.clitcomp_city & vbNewLine & _
      " clitcomp_state = " & aClient_Transactions_Company.clitcomp_state & vbNewLine & _
      " clitcomp_zip_code = " & aClient_Transactions_Company.clitcomp_zip_code & vbNewLine & _
      " clitcomp_country = " & aClient_Transactions_Company.clitcomp_country & vbNewLine & _
      " clitcomp_agency_type = " & aClient_Transactions_Company.clitcomp_agency_type & vbNewLine & _
      " clitcomp_web_address = " & aClient_Transactions_Company.clitcomp_web_address & vbNewLine & _
      " clitcomp_email_address = " & aClient_Transactions_Company.clitcomp_email_address & vbNewLine & _
      " clitcomp_action_date = " & aClient_Transactions_Company.clitcomp_action_date & vbNewLine



      ' return the string
      Return ClassInformation
    Catch ex As Exception
      MsgBox("Error occured in classInfo. Class: clsClient_Transactions_Company. Error:" & ex.Message)
      Return Nothing
    End Try
  End Function


End Class
