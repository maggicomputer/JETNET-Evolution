Option Strict On
Option Explicit On


' ***********************************************************************
' Class Comments Section
' ***********************************************************************
' Class name: clsclient_transaction_category
' Purpose: Configure all properties and methods for clsclient_transaction_category
' Parameters: None
' Return: None
' Change Log
'           08/03/2010 - Created By: Tom Jones


Public Class clsclient_transaction_category



'**************************************************
' Private variable declarations
'**************************************************
Private strclitcat_code As String
Private strclitcat_name As String
Private strclitcat_type As String
Private strclitcat_tofrom_businesstype As String


'**************************************************
' Setters and getters
'**************************************************
Public Property clitcat_code As String
    Get
       Return strclitcat_code
    End Get
    Set (ByVal Value As String)
        strclitcat_code = Value
    End Set
End Property


Public Property clitcat_name As String
    Get
       Return strclitcat_name
    End Get
    Set (ByVal Value As String)
        strclitcat_name = Value
    End Set
End Property


Public Property clitcat_type As String
    Get
       Return strclitcat_type
    End Get
    Set (ByVal Value As String)
        strclitcat_type = Value
    End Set
End Property


Public Property clitcat_tofrom_businesstype As String
    Get
       Return strclitcat_tofrom_businesstype
    End Get
    Set (ByVal Value As String)
        strclitcat_tofrom_businesstype = Value
    End Set
End Property


'**************************************************
' Constructors
'**************************************************


'   Default Constructor
Public Sub New()
clitcat_code = ""
clitcat_name = ""
clitcat_type = ""
clitcat_tofrom_businesstype = ""
End Sub


'   Parameter Based Constructor
Public Sub New(ByVal aclitcat_code As String ,ByVal aclitcat_name As String ,ByVal aclitcat_type As String ,ByVal aclitcat_tofrom_businesstype As String)
clitcat_code = aclitcat_code
clitcat_name = aclitcat_name
clitcat_type = aclitcat_type
clitcat_tofrom_businesstype = aclitcat_tofrom_businesstype
End Sub




' ***********************************************************************
' Methods
' ***********************************************************************


' Method name: ClassInfo
' Purpose: to generate a string with all assigned parameters
' Parameters: client_transaction_category
' Return: String with all assigned parameters
' Change Log
'           08/03/2010 - Created By: Tom Jones
Public Function ClassInfo(byval aclient_transaction_category as clsclient_transaction_category) as string
Try
Dim ClassInformation as String


ClassInformation = " clitcat_code = "  & aclient_transaction_category.clitcat_code & vbnewline & _ 
" clitcat_name = "  & aclient_transaction_category.clitcat_name & vbnewline & _ 
" clitcat_type = "  & aclient_transaction_category.clitcat_type & vbnewline & _ 
" clitcat_tofrom_businesstype = "  & aclient_transaction_category.clitcat_tofrom_businesstype & vbnewline  _ 



' return the string
Return ClassInformation
Catch ex As Exception
MsgBox("Error occured in classInfo. Class: clsclient_transaction_category. Error:" & ex.Message)
Return Nothing
End Try
End Function


End Class
