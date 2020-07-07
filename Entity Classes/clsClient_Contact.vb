Option Strict On
Option Explicit On


' ***********************************************************************
' Class Comments Section
' ***********************************************************************
' Class name: clsClient_Contact
' Purpose: Configure all properties and methods for clsClient_Contact
' Parameters: None
' Return: None
' Change Log
'           1/21/2010 - Created By: Tom Jones


Public Class clsClient_Contact



'**************************************************
' Private variable declarations
'**************************************************
Private intclicontact_id As Integer
Private intclicontact_comp_id As Integer
Private strclicontact_sirname As String
Private strclicontact_first_name As String
Private strclicontact_middle_initial As String
Private strclicontact_last_name As String
Private strclicontact_suffix As String
Private strclicontact_title As String
Private strclicontact_email_address As String
Private strclicontact_date_updated As String
  Private intclicontact_jetnet_contact_id As Integer
  Private strclicontact_notes As String
  Private strclicontact_email_list As String
  Private strclicontact_preferred_name As String
  Private strclicontact_status As String
  Private intclicontact_user_id As Integer

'**************************************************
' Setters and getters
'**************************************************
Public Property clicontact_id As Integer
    Get
       Return intclicontact_id
    End Get
    Set (ByVal Value As Integer)
        intclicontact_id = Value
    End Set
End Property


Public Property clicontact_comp_id As Integer
    Get
       Return intclicontact_comp_id
    End Get
    Set (ByVal Value As Integer)
        intclicontact_comp_id = Value
    End Set
End Property


Public Property clicontact_sirname As String
    Get
       Return strclicontact_sirname
    End Get
    Set (ByVal Value As String)
        strclicontact_sirname = Value
    End Set
End Property


Public Property clicontact_first_name As String
    Get
       Return strclicontact_first_name
    End Get
    Set (ByVal Value As String)
        strclicontact_first_name = Value
    End Set
End Property


Public Property clicontact_middle_initial As String
    Get
       Return strclicontact_middle_initial
    End Get
    Set (ByVal Value As String)
        strclicontact_middle_initial = Value
    End Set
End Property


Public Property clicontact_last_name As String
    Get
       Return strclicontact_last_name
    End Get
    Set (ByVal Value As String)
        strclicontact_last_name = Value
    End Set
End Property


Public Property clicontact_suffix As String
    Get
       Return strclicontact_suffix
    End Get
    Set (ByVal Value As String)
        strclicontact_suffix = Value
    End Set
End Property


Public Property clicontact_title As String
    Get
       Return strclicontact_title
    End Get
    Set (ByVal Value As String)
        strclicontact_title = Value
    End Set
End Property


Public Property clicontact_email_address As String
    Get
       Return strclicontact_email_address
    End Get
    Set (ByVal Value As String)
        strclicontact_email_address = Value
    End Set
End Property


Public Property clicontact_date_updated As String
    Get
       Return strclicontact_date_updated
    End Get
    Set (ByVal Value As String)
        strclicontact_date_updated = Value
    End Set
End Property


Public Property clicontact_jetnet_contact_id As Integer
    Get
       Return intclicontact_jetnet_contact_id
    End Get
    Set (ByVal Value As Integer)
        intclicontact_jetnet_contact_id = Value
    End Set
  End Property

  Public Property clicontact_notes() As String
    Get
      Return strclicontact_notes
    End Get
    Set(ByVal Value As String)
      strclicontact_notes = Value
    End Set
  End Property

  Public Property clicontact_email_list() As String
    Get
      Return strclicontact_email_list
    End Get
    Set(ByVal Value As String)
      strclicontact_email_list = Value
    End Set
  End Property

  Public Property clicontact_preferred_name() As String
    Get
      Return strclicontact_preferred_name
    End Get
    Set(ByVal Value As String)
      strclicontact_preferred_name = Value
    End Set
  End Property

  Public Property clicontact_status() As String
    Get
      Return strclicontact_status
    End Get
    Set(ByVal Value As String)
      strclicontact_status = Value
    End Set
  End Property

  Public Property clicontact_user_id() As Integer
    Get
      Return intclicontact_user_id
    End Get
    Set(ByVal Value As Integer)
      intclicontact_user_id = Value
    End Set
  End Property


'**************************************************
' Constructors
'**************************************************


'   Default Constructor
Public Sub New()
    clicontact_id = 0
    clicontact_comp_id = 0
clicontact_sirname = ""
clicontact_first_name = ""
clicontact_middle_initial = ""
clicontact_last_name = ""
clicontact_suffix = ""
clicontact_title = ""
clicontact_email_address = ""
clicontact_date_updated = ""
    clicontact_jetnet_contact_id = 0
    clicontact_notes = ""
    clicontact_email_list = ""
    clicontact_preferred_name = ""
    clicontact_status = "Y"
    clicontact_user_id = 0
End Sub


'   Parameter Based Constructor
  Public Sub New(ByVal aclicontact_id As Integer, ByVal aclicontact_comp_id As Integer, ByVal aclicontact_sirname As String, ByVal aclicontact_first_name As String, ByVal aclicontact_middle_initial As String, ByVal aclicontact_last_name As String, ByVal aclicontact_suffix As String, ByVal aclicontact_title As String, ByVal aclicontact_email_address As String, ByVal aclicontact_date_updated As String, ByVal aclicontact_jetnet_contact_id As Integer, ByVal astrclicontact_notes As String, ByVal aclicontact_email_list As String, ByVal aclicontact_preferred_name As String, ByVal aclicontact_status As String, ByVal aclicontact_user_id As Integer)
    clicontact_id = aclicontact_id
    clicontact_comp_id = aclicontact_comp_id
    clicontact_sirname = aclicontact_sirname
    clicontact_first_name = aclicontact_first_name
    clicontact_middle_initial = aclicontact_middle_initial
    clicontact_last_name = aclicontact_last_name
    clicontact_suffix = aclicontact_suffix
    clicontact_title = aclicontact_title
    clicontact_email_address = aclicontact_email_address
    clicontact_date_updated = aclicontact_date_updated
    clicontact_jetnet_contact_id = aclicontact_jetnet_contact_id
    clicontact_notes = astrclicontact_notes
    clicontact_email_list = aclicontact_email_list
    clicontact_preferred_name = aclicontact_preferred_name
    clicontact_status = aclicontact_status
    clicontact_user_id = aclicontact_user_id
  End Sub




' ***********************************************************************
' Methods
' ***********************************************************************


' Method name: ClassInfo
' Purpose: to generate a string with all assigned parameters
' Parameters: Client_Contact
' Return: String with all assigned parameters
' Change Log
'           1/21/2010 - Created By: Tom Jones
Public Function ClassInfo(byval aClient_Contact as clsClient_Contact) as string
Try
Dim ClassInformation as String


      ClassInformation = " clicontact_id = " & aClient_Contact.clicontact_id & vbNewLine & " clicontact_comp_id = " & aClient_Contact.clicontact_comp_id & vbNewLine & " clicontact_sirname = " & aClient_Contact.clicontact_sirname & vbNewLine & " clicontact_first_name = " & aClient_Contact.clicontact_first_name & vbNewLine & " clicontact_middle_initial = " & aClient_Contact.clicontact_middle_initial & vbNewLine & " clicontact_last_name = " & aClient_Contact.clicontact_last_name & vbNewLine & " clicontact_suffix = " & aClient_Contact.clicontact_suffix & vbNewLine & " clicontact_title = " & aClient_Contact.clicontact_title & vbNewLine & " clicontact_email_address = " & aClient_Contact.clicontact_email_address & vbNewLine & " clicontact_date_updated = " & aClient_Contact.clicontact_date_updated & vbNewLine & " clicontact_jetnet_contact_id = " & aClient_Contact.clicontact_jetnet_contact_id & vbNewLine & " clicontact_notes = " & aClient_Contact.clicontact_notes & vbNewLine & " clicontact_email_list " & aClient_Contact.clicontact_email_list & vbNewLine & " clicontact_preferred_name " & aClient_Contact.clicontact_preferred_name & vbNewLine & " clicontact_status " & aClient_Contact.clicontact_status & "clicontact_user_id " & aClient_Contact.clicontact_user_id


' return the string
Return ClassInformation
Catch ex As Exception
            'MsgBox("Error occured in classInfo. Class: clsClient_Contact. Error:" & ex.Message)
Return Nothing
End Try
End Function


End Class
