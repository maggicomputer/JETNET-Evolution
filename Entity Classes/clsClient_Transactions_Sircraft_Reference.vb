Option Strict On
Option Explicit On


' ***********************************************************************
' Class Comments Section
' ***********************************************************************
' Class name: clsClient_Transactions_Sircraft_Reference
' Purpose: Configure all properties and methods for clsClient_Transactions_Sircraft_Reference
' Parameters: None
' Return: None
' Change Log
'           08/03/2010 - Created By: Tom Jones


Public Class clsClient_Transactions_Sircraft_Reference



  '**************************************************
  ' Private variable declarations
  '**************************************************
  Private intclitcref_id As Integer
  Private intclitcref_client_ac_id As Integer
  Private intclitcref_client_trans_id As Integer
  Private intclitcref_client_comp_id As Integer
  Private intclitcref_client_contact_id As Integer
  Private strclitcref_contact_type As String
  Private dblclitcref_owner_percentage As Double
  Private datclitcref_date_fraction_expires As System.DateTime
  Private strclitcref_business_type As String
  Private strclitcref_operator_flag As String


  '**************************************************
  ' Setters and getters
  '**************************************************
  Public Property clitcref_id As Integer
    Get
      Return intclitcref_id
    End Get
    Set(ByVal Value As Integer)
      intclitcref_id = Value
    End Set
  End Property


  Public Property clitcref_client_ac_id As Integer
    Get
      Return intclitcref_client_ac_id
    End Get
    Set(ByVal Value As Integer)
      intclitcref_client_ac_id = Value
    End Set
  End Property


  Public Property clitcref_client_trans_id As Integer
    Get
      Return intclitcref_client_trans_id
    End Get
    Set(ByVal Value As Integer)
      intclitcref_client_trans_id = Value
    End Set
  End Property


  Public Property clitcref_client_comp_id As Integer
    Get
      Return intclitcref_client_comp_id
    End Get
    Set(ByVal Value As Integer)
      intclitcref_client_comp_id = Value
    End Set
  End Property


  Public Property clitcref_client_contact_id As Integer
    Get
      Return intclitcref_client_contact_id
    End Get
    Set(ByVal Value As Integer)
      intclitcref_client_contact_id = Value
    End Set
  End Property


  Public Property clitcref_contact_type As String
    Get
      Return strclitcref_contact_type
    End Get
    Set(ByVal Value As String)
      strclitcref_contact_type = Value
    End Set
  End Property


  Public Property clitcref_owner_percentage As Double
    Get
      Return dblclitcref_owner_percentage
    End Get
    Set(ByVal Value As Double)
      dblclitcref_owner_percentage = Value
    End Set
  End Property


  Public Property clitcref_date_fraction_expires As System.DateTime
    Get
      Return datclitcref_date_fraction_expires
    End Get
    Set(ByVal Value As System.DateTime)
      datclitcref_date_fraction_expires = Value
    End Set
  End Property


  Public Property clitcref_business_type As String
    Get
      Return strclitcref_business_type
    End Get
    Set(ByVal Value As String)
      strclitcref_business_type = Value
    End Set
  End Property


  Public Property clitcref_operator_flag As String
    Get
      Return strclitcref_operator_flag
    End Get
    Set(ByVal Value As String)
      strclitcref_operator_flag = Value
    End Set
  End Property


  '**************************************************
  ' Constructors
  '**************************************************


  '   Default Constructor
  Public Sub New()
    clitcref_id = 0
    clitcref_client_ac_id = 0
    clitcref_client_trans_id = 0
    clitcref_client_comp_id = 0
    clitcref_client_contact_id = 0
    clitcref_contact_type = ""
    clitcref_owner_percentage = 0.0
    clitcref_date_fraction_expires = System.DateTime.Now.ToLocalTime
    clitcref_business_type = ""
    clitcref_operator_flag = ""
  End Sub


  '   Parameter Based Constructor
  Public Sub New(ByVal aclitcref_id As Integer, ByVal aclitcref_client_ac_id As Integer, ByVal aclitcref_client_trans_id As Integer, ByVal aclitcref_client_comp_id As Integer, ByVal aclitcref_client_contact_id As Integer, ByVal aclitcref_contact_type As String, ByVal aclitcref_owner_percentage As Double, ByVal aclitcref_date_fraction_expires As System.DateTime, ByVal aclitcref_business_type As String, ByVal aclitcref_operator_flag As String)
    clitcref_id = aclitcref_id
    clitcref_client_ac_id = aclitcref_client_ac_id
    clitcref_client_trans_id = aclitcref_client_trans_id
    clitcref_client_comp_id = aclitcref_client_comp_id
    clitcref_client_contact_id = aclitcref_client_contact_id
    clitcref_contact_type = aclitcref_contact_type
    clitcref_owner_percentage = aclitcref_owner_percentage
    clitcref_date_fraction_expires = aclitcref_date_fraction_expires
    clitcref_business_type = aclitcref_business_type
    clitcref_operator_flag = aclitcref_operator_flag
  End Sub




  ' ***********************************************************************
  ' Methods
  ' ***********************************************************************


  ' Method name: ClassInfo
  ' Purpose: to generate a string with all assigned parameters
  ' Parameters: Table
  ' Return: String with all assigned parameters
  ' Change Log
  '           08/03/2010 - Created By: Tom Jones
  Public Function ClassInfo(ByVal aTable As clsClient_Transactions_Sircraft_Reference) As String
    Try
      Dim ClassInformation As String


      ClassInformation = " clitcref_id = " & aTable.clitcref_id & " clitcref_client_ac_id = " & aTable.clitcref_client_ac_id & " clitcref_client_trans_id = " & aTable.clitcref_client_trans_id & " clitcref_client_comp_id = " & aTable.clitcref_client_comp_id & " clitcref_client_contact_id = " & aTable.clitcref_client_contact_id & " clitcref_contact_type = " & aTable.clitcref_contact_type & " clitcref_owner_percentage = " & aTable.clitcref_owner_percentage & " clitcref_date_fraction_expires = " & aTable.clitcref_date_fraction_expires & " clitcref_business_type = " & aTable.clitcref_business_type & " clitcref_operator_flag = " & aTable.clitcref_operator_flag


      ' return the string
      Return ClassInformation
    Catch ex As Exception
      MsgBox("Error occured in classInfo. Class: clsClient_Transactions_Sircraft_Reference. Error:" & ex.Message)
      Return Nothing
    End Try
  End Function


End Class
