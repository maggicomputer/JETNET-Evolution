Option Strict On
Option Explicit On


' ***********************************************************************
' Class Comments Section
' ***********************************************************************
' Class name: clsClient_Document
' Purpose: Configure all properties and methods for clsClient_Document
' Parameters: None
' Return: None
' Change Log
'           3/3/2010 - Created By: Tom Jones


Public Class clsClient_Document



'**************************************************
' Private variable declarations
'**************************************************
Private intclidoc_id As Integer
Private intclidoc_lnote_id As Integer
Private strclidoc_title As String
Private strclidoc_description As String
Private strclidoc_type As String


'**************************************************
' Setters and getters
'**************************************************
Public Property clidoc_id As Integer
    Get
       Return intclidoc_id
    End Get
    Set (ByVal Value As Integer)
        intclidoc_id = Value
    End Set
End Property


Public Property clidoc_lnote_id As Integer
    Get
       Return intclidoc_lnote_id
    End Get
    Set (ByVal Value As Integer)
        intclidoc_lnote_id = Value
    End Set
End Property


Public Property clidoc_title As String
    Get
       Return strclidoc_title
    End Get
    Set (ByVal Value As String)
        strclidoc_title = Value
    End Set
End Property


Public Property clidoc_description As String
    Get
       Return strclidoc_description
    End Get
    Set (ByVal Value As String)
        strclidoc_description = Value
    End Set
End Property


Public Property clidoc_type As String
    Get
       Return strclidoc_type
    End Get
    Set (ByVal Value As String)
        strclidoc_type = Value
    End Set
End Property


'**************************************************
' Constructors
'**************************************************


'   Default Constructor
Public Sub New()
    clidoc_id = 0
    clidoc_lnote_id = 0
clidoc_title = ""
clidoc_description = ""
clidoc_type = ""
End Sub


'   Parameter Based Constructor
Public Sub New(ByVal aclidoc_id As Integer ,ByVal aclidoc_lnote_id As Integer ,ByVal aclidoc_title As String ,ByVal aclidoc_description As String ,ByVal aclidoc_type As String)
clidoc_id = aclidoc_id
clidoc_lnote_id = aclidoc_lnote_id
clidoc_title = aclidoc_title
clidoc_description = aclidoc_description
clidoc_type = aclidoc_type
End Sub




' ***********************************************************************
' Methods
' ***********************************************************************


' Method name: ClassInfo
' Purpose: to generate a string with all assigned parameters
' Parameters: Client_Document
' Return: String with all assigned parameters
' Change Log
'           3/3/2010 - Created By: Tom Jones
Public Function ClassInfo(byval aClient_Document as clsClient_Document) as string
Try
Dim ClassInformation as String


ClassInformation = " clidoc_id = "  & aClient_Document.clidoc_id & vbnewline & " clidoc_lnote_id = "  & aClient_Document.clidoc_lnote_id & vbnewline & " clidoc_title = "  & aClient_Document.clidoc_title & vbnewline & " clidoc_description = "  & aClient_Document.clidoc_description & vbnewline & " clidoc_type = "  & aClient_Document.clidoc_type & vbnewline  


' return the string
Return ClassInformation
Catch ex As Exception
MsgBox("Error occured in classInfo. Class: clsClient_Document. Error:" & ex.Message)
Return Nothing
End Try
End Function


End Class
