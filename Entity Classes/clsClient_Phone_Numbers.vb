Option Strict On
Option Explicit On


' ***********************************************************************
' Class Comments Section
' ***********************************************************************
' Class name: clsClient_Phone_Numbers
' Purpose: Configure all properties and methods for clsClient_Phone_Numbers
' Parameters: None
' Return: None
' Change Log
'           1/21/2010 - Created By: Tom Jones


Public Class clsClient_Phone_Numbers



'**************************************************
' Private variable declarations
'**************************************************
Private intclipnum_id As Integer
Private intclipnum_comp_id As Integer
Private intclipnum_contact_id As Integer
Private strclipnum_type As String
Private strclipnum_number As String


'**************************************************
' Setters and getters
'**************************************************
Public Property clipnum_id As Integer
    Get
       Return intclipnum_id
    End Get
    Set (ByVal Value As Integer)
        intclipnum_id = Value
    End Set
End Property


Public Property clipnum_comp_id As Integer
    Get
       Return intclipnum_comp_id
    End Get
    Set (ByVal Value As Integer)
        intclipnum_comp_id = Value
    End Set
End Property


Public Property clipnum_contact_id As Integer
    Get
       Return intclipnum_contact_id
    End Get
    Set (ByVal Value As Integer)
        intclipnum_contact_id = Value
    End Set
End Property


Public Property clipnum_type As String
    Get
       Return strclipnum_type
    End Get
    Set (ByVal Value As String)
        strclipnum_type = Value
    End Set
End Property


Public Property clipnum_number As String
    Get
       Return strclipnum_number
    End Get
    Set (ByVal Value As String)
        strclipnum_number = Value
    End Set
End Property


'**************************************************
' Constructors
'**************************************************


'   Default Constructor
Public Sub New()
    clipnum_id = 0
    clipnum_comp_id = 0
    clipnum_contact_id = 0
clipnum_type = ""
clipnum_number = ""
End Sub


'   Parameter Based Constructor
Public Sub New(ByVal aclipnum_id As Integer ,ByVal aclipnum_comp_id As Integer ,ByVal aclipnum_contact_id As Integer ,ByVal aclipnum_type As String ,ByVal aclipnum_number As String)
clipnum_id = aclipnum_id
clipnum_comp_id = aclipnum_comp_id
clipnum_contact_id = aclipnum_contact_id
clipnum_type = aclipnum_type
clipnum_number = aclipnum_number
End Sub




' ***********************************************************************
' Methods
' ***********************************************************************


' Method name: ClassInfo
' Purpose: to generate a string with all assigned parameters
' Parameters: Client_Phone_Numbers
' Return: String with all assigned parameters
' Change Log
'           1/21/2010 - Created By: Tom Jones
Public Function ClassInfo(byval aClient_Phone_Numbers as clsClient_Phone_Numbers) as string
Try
Dim ClassInformation as String


ClassInformation = " clipnum_id = "  & aClient_Phone_Numbers.clipnum_id & vbnewline & " clipnum_comp_id = "  & aClient_Phone_Numbers.clipnum_comp_id & vbnewline & " clipnum_contact_id = "  & aClient_Phone_Numbers.clipnum_contact_id & vbnewline & " clipnum_type = "  & aClient_Phone_Numbers.clipnum_type & vbnewline & " clipnum_number = "  & aClient_Phone_Numbers.clipnum_number & vbnewline  


' return the string
Return ClassInformation
Catch ex As Exception
            'MsgBox("Error occured in classInfo. Class: clsClient_Phone_Numbers. Error:" & ex.Message)
Return Nothing
End Try
End Function


End Class
