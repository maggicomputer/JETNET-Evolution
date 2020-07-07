Option Strict On
Option Explicit On


' ***********************************************************************
' Class Comments Section
' ***********************************************************************
' Class name: clsClient_Error_Log
' Purpose: Configure all properties and methods for clsClient_Error_Log
' Parameters: None
' Return: None
' Change Log
'           2/13/2010 - Created By: Tom Jones


Public Class clsClient_Error_Log



'**************************************************
' Private variable declarations
'**************************************************
Private intclierror_id As Integer
Private strclierror_location As String
Private strclierror_desc As String
Private strclierror_time As String


'**************************************************
' Setters and getters
'**************************************************
Public Property clierror_id As Integer
    Get
       Return intclierror_id
    End Get
    Set (ByVal Value As Integer)
        intclierror_id = Value
    End Set
End Property


Public Property clierror_location As String
    Get
       Return strclierror_location
    End Get
    Set (ByVal Value As String)
        strclierror_location = Value
    End Set
End Property


Public Property clierror_desc As String
    Get
       Return strclierror_desc
    End Get
    Set (ByVal Value As String)
        strclierror_desc = Value
    End Set
End Property


Public Property clierror_time As String
    Get
       Return strclierror_time
    End Get
    Set (ByVal Value As String)
        strclierror_time = Value
    End Set
End Property


'**************************************************
' Constructors
'**************************************************


'   Default Constructor
Public Sub New()
    clierror_id = 0
clierror_location = ""
clierror_desc = ""
clierror_time = ""
End Sub


'   Parameter Based Constructor
Public Sub New(ByVal aclierror_id As Integer ,ByVal aclierror_location As String ,ByVal aclierror_desc As String ,ByVal aclierror_time As String)
clierror_id = aclierror_id
clierror_location = aclierror_location
clierror_desc = aclierror_desc
clierror_time = aclierror_time
End Sub




' ***********************************************************************
' Methods
' ***********************************************************************


' Method name: ClassInfo
' Purpose: to generate a string with all assigned parameters
' Parameters: Client_Error_Log
' Return: String with all assigned parameters
' Change Log
'           2/13/2010 - Created By: Tom Jones
Public Function ClassInfo(byval aClient_Error_Log as clsClient_Error_Log) as string
Try
Dim ClassInformation as String


ClassInformation = " clierror_id = "  & aClient_Error_Log.clierror_id & vbnewline & " clierror_location = "  & aClient_Error_Log.clierror_location & vbnewline & " clierror_desc = "  & aClient_Error_Log.clierror_desc & vbnewline & " clierror_time = "  & aClient_Error_Log.clierror_time & vbnewline  


' return the string
Return ClassInformation
Catch ex As Exception
      'MsgBox("Error occured in classInfo. Class: clsClient_Error_Log. Error:" & ex.Message)
Return Nothing
End Try
End Function


End Class
