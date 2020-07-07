Option Strict On
Option Explicit On


' ***********************************************************************
' Class Comments Section
' ***********************************************************************
' Class name: clsClient_Aircraft_Propeller
' Purpose: Configure all properties and methods for clsClient_Aircraft_Propeller
' Parameters: None
' Return: None
' Change Log
'           06/24/2010 - Created By: Tom Jones


Public Class clsClient_Aircraft_Propeller



'**************************************************
' Private variable declarations
'**************************************************
Private intcliacpr_cliac_id As Integer
Private strcliacpr_prop_1_ser_nbr As String
Private strcliacpr_prop_2_ser_nbr As String
Private strcliacpr_prop_3_ser_nbr As String
Private strcliacpr_prop_4_ser_nbr As String
Private intcliacpr_prop_1_ttsn_hours As Integer
Private intcliacpr_prop_2_ttsn_hours As Integer
Private intcliacpr_prop_3_ttsn_hours As Integer
Private intcliacpr_prop_4_ttsn_hours As Integer
Private intcliacpr_prop_1_tsoh_hours As Integer
Private intcliacpr_prop_2_tsoh_hours As Integer
Private intcliacpr_prop_3_tsoh_hours As Integer
Private intcliacpr_prop_4_tsoh_hours As Integer
Private strcliacpr_prop_1_month_year_oh As String
Private strcliacpr_prop_2_month_year_oh As String
Private strcliacpr_prop_3_month_year_oh As String
Private strcliacpr_prop_4_month_year_oh As String


'**************************************************
' Setters and getters
'**************************************************
Public Property cliacpr_cliac_id As Integer
    Get
       Return intcliacpr_cliac_id
    End Get
    Set (ByVal Value As Integer)
        intcliacpr_cliac_id = Value
    End Set
End Property


Public Property cliacpr_prop_1_ser_nbr As String
    Get
       Return strcliacpr_prop_1_ser_nbr
    End Get
    Set (ByVal Value As String)
        strcliacpr_prop_1_ser_nbr = Value
    End Set
End Property


Public Property cliacpr_prop_2_ser_nbr As String
    Get
       Return strcliacpr_prop_2_ser_nbr
    End Get
    Set (ByVal Value As String)
        strcliacpr_prop_2_ser_nbr = Value
    End Set
End Property


Public Property cliacpr_prop_3_ser_nbr As String
    Get
       Return strcliacpr_prop_3_ser_nbr
    End Get
    Set (ByVal Value As String)
        strcliacpr_prop_3_ser_nbr = Value
    End Set
End Property


Public Property cliacpr_prop_4_ser_nbr As String
    Get
       Return strcliacpr_prop_4_ser_nbr
    End Get
    Set (ByVal Value As String)
        strcliacpr_prop_4_ser_nbr = Value
    End Set
End Property


Public Property cliacpr_prop_1_ttsn_hours As Integer
    Get
       Return intcliacpr_prop_1_ttsn_hours
    End Get
    Set (ByVal Value As Integer)
        intcliacpr_prop_1_ttsn_hours = Value
    End Set
End Property


Public Property cliacpr_prop_2_ttsn_hours As Integer
    Get
       Return intcliacpr_prop_2_ttsn_hours
    End Get
    Set (ByVal Value As Integer)
        intcliacpr_prop_2_ttsn_hours = Value
    End Set
End Property


Public Property cliacpr_prop_3_ttsn_hours As Integer
    Get
       Return intcliacpr_prop_3_ttsn_hours
    End Get
    Set (ByVal Value As Integer)
        intcliacpr_prop_3_ttsn_hours = Value
    End Set
End Property


Public Property cliacpr_prop_4_ttsn_hours As Integer
    Get
       Return intcliacpr_prop_4_ttsn_hours
    End Get
    Set (ByVal Value As Integer)
        intcliacpr_prop_4_ttsn_hours = Value
    End Set
End Property


Public Property cliacpr_prop_1_tsoh_hours As Integer
    Get
       Return intcliacpr_prop_1_tsoh_hours
    End Get
    Set (ByVal Value As Integer)
        intcliacpr_prop_1_tsoh_hours = Value
    End Set
End Property


Public Property cliacpr_prop_2_tsoh_hours As Integer
    Get
       Return intcliacpr_prop_2_tsoh_hours
    End Get
    Set (ByVal Value As Integer)
        intcliacpr_prop_2_tsoh_hours = Value
    End Set
End Property


Public Property cliacpr_prop_3_tsoh_hours As Integer
    Get
       Return intcliacpr_prop_3_tsoh_hours
    End Get
    Set (ByVal Value As Integer)
        intcliacpr_prop_3_tsoh_hours = Value
    End Set
End Property


Public Property cliacpr_prop_4_tsoh_hours As Integer
    Get
       Return intcliacpr_prop_4_tsoh_hours
    End Get
    Set (ByVal Value As Integer)
        intcliacpr_prop_4_tsoh_hours = Value
    End Set
End Property


Public Property cliacpr_prop_1_month_year_oh As String
    Get
       Return strcliacpr_prop_1_month_year_oh
    End Get
    Set (ByVal Value As String)
        strcliacpr_prop_1_month_year_oh = Value
    End Set
End Property


Public Property cliacpr_prop_2_month_year_oh As String
    Get
       Return strcliacpr_prop_2_month_year_oh
    End Get
    Set (ByVal Value As String)
        strcliacpr_prop_2_month_year_oh = Value
    End Set
End Property


Public Property cliacpr_prop_3_month_year_oh As String
    Get
       Return strcliacpr_prop_3_month_year_oh
    End Get
    Set (ByVal Value As String)
        strcliacpr_prop_3_month_year_oh = Value
    End Set
End Property


Public Property cliacpr_prop_4_month_year_oh As String
    Get
       Return strcliacpr_prop_4_month_year_oh
    End Get
    Set (ByVal Value As String)
        strcliacpr_prop_4_month_year_oh = Value
    End Set
End Property


'**************************************************
' Constructors
'**************************************************


'   Default Constructor
Public Sub New()
    cliacpr_cliac_id = 0
cliacpr_prop_1_ser_nbr = ""
cliacpr_prop_2_ser_nbr = ""
cliacpr_prop_3_ser_nbr = ""
cliacpr_prop_4_ser_nbr = ""
    cliacpr_prop_1_ttsn_hours = 0
    cliacpr_prop_2_ttsn_hours = 0
    cliacpr_prop_3_ttsn_hours = 0
    cliacpr_prop_4_ttsn_hours = 0
    cliacpr_prop_1_tsoh_hours = 0
    cliacpr_prop_2_tsoh_hours = 0
    cliacpr_prop_3_tsoh_hours = 0
    cliacpr_prop_4_tsoh_hours = 0
cliacpr_prop_1_month_year_oh = ""
cliacpr_prop_2_month_year_oh = ""
cliacpr_prop_3_month_year_oh = ""
cliacpr_prop_4_month_year_oh = ""
End Sub


'   Parameter Based Constructor
Public Sub New(ByVal acliacpr_cliac_id As Integer ,ByVal acliacpr_prop_1_ser_nbr As String ,ByVal acliacpr_prop_2_ser_nbr As String ,ByVal acliacpr_prop_3_ser_nbr As String ,ByVal acliacpr_prop_4_ser_nbr As String ,ByVal acliacpr_prop_1_ttsn_hours As Integer ,ByVal acliacpr_prop_2_ttsn_hours As Integer ,ByVal acliacpr_prop_3_ttsn_hours As Integer ,ByVal acliacpr_prop_4_ttsn_hours As Integer ,ByVal acliacpr_prop_1_tsoh_hours As Integer ,ByVal acliacpr_prop_2_tsoh_hours As Integer ,ByVal acliacpr_prop_3_tsoh_hours As Integer ,ByVal acliacpr_prop_4_tsoh_hours As Integer ,ByVal acliacpr_prop_1_month_year_oh As String ,ByVal acliacpr_prop_2_month_year_oh As String ,ByVal acliacpr_prop_3_month_year_oh As String ,ByVal acliacpr_prop_4_month_year_oh As String)
cliacpr_cliac_id = acliacpr_cliac_id
cliacpr_prop_1_ser_nbr = acliacpr_prop_1_ser_nbr
cliacpr_prop_2_ser_nbr = acliacpr_prop_2_ser_nbr
cliacpr_prop_3_ser_nbr = acliacpr_prop_3_ser_nbr
cliacpr_prop_4_ser_nbr = acliacpr_prop_4_ser_nbr
cliacpr_prop_1_ttsn_hours = acliacpr_prop_1_ttsn_hours
cliacpr_prop_2_ttsn_hours = acliacpr_prop_2_ttsn_hours
cliacpr_prop_3_ttsn_hours = acliacpr_prop_3_ttsn_hours
cliacpr_prop_4_ttsn_hours = acliacpr_prop_4_ttsn_hours
cliacpr_prop_1_tsoh_hours = acliacpr_prop_1_tsoh_hours
cliacpr_prop_2_tsoh_hours = acliacpr_prop_2_tsoh_hours
cliacpr_prop_3_tsoh_hours = acliacpr_prop_3_tsoh_hours
cliacpr_prop_4_tsoh_hours = acliacpr_prop_4_tsoh_hours
cliacpr_prop_1_month_year_oh = acliacpr_prop_1_month_year_oh
cliacpr_prop_2_month_year_oh = acliacpr_prop_2_month_year_oh
cliacpr_prop_3_month_year_oh = acliacpr_prop_3_month_year_oh
cliacpr_prop_4_month_year_oh = acliacpr_prop_4_month_year_oh
End Sub




' ***********************************************************************
' Methods
' ***********************************************************************


' Method name: ClassInfo
' Purpose: to generate a string with all assigned parameters
' Parameters: Client_Aircraft_Propeller
' Return: String with all assigned parameters
' Change Log
'           06/24/2010 - Created By: Tom Jones
Public Function ClassInfo(byval aClient_Aircraft_Propeller as clsClient_Aircraft_Propeller) as string
Try
Dim ClassInformation as String


ClassInformation = " cliacpr_cliac_id = "  & aClient_Aircraft_Propeller.cliacpr_cliac_id & vbnewline & " cliacpr_prop_1_ser_nbr = "  & aClient_Aircraft_Propeller.cliacpr_prop_1_ser_nbr & vbnewline & " cliacpr_prop_2_ser_nbr = "  & aClient_Aircraft_Propeller.cliacpr_prop_2_ser_nbr & vbnewline & " cliacpr_prop_3_ser_nbr = "  & aClient_Aircraft_Propeller.cliacpr_prop_3_ser_nbr & vbnewline & " cliacpr_prop_4_ser_nbr = "  & aClient_Aircraft_Propeller.cliacpr_prop_4_ser_nbr & vbnewline & " cliacpr_prop_1_ttsn_hours = "  & aClient_Aircraft_Propeller.cliacpr_prop_1_ttsn_hours & vbnewline & " cliacpr_prop_2_ttsn_hours = "  & aClient_Aircraft_Propeller.cliacpr_prop_2_ttsn_hours & vbnewline & " cliacpr_prop_3_ttsn_hours = "  & aClient_Aircraft_Propeller.cliacpr_prop_3_ttsn_hours & vbnewline & " cliacpr_prop_4_ttsn_hours = "  & aClient_Aircraft_Propeller.cliacpr_prop_4_ttsn_hours & vbnewline & " cliacpr_prop_1_tsoh_hours = "  & aClient_Aircraft_Propeller.cliacpr_prop_1_tsoh_hours & vbnewline & " cliacpr_prop_2_tsoh_hours = "  & aClient_Aircraft_Propeller.cliacpr_prop_2_tsoh_hours & vbnewline & " cliacpr_prop_3_tsoh_hours = "  & aClient_Aircraft_Propeller.cliacpr_prop_3_tsoh_hours & vbnewline & " cliacpr_prop_4_tsoh_hours = "  & aClient_Aircraft_Propeller.cliacpr_prop_4_tsoh_hours & vbnewline & " cliacpr_prop_1_month_year_oh = "  & aClient_Aircraft_Propeller.cliacpr_prop_1_month_year_oh & vbnewline & " cliacpr_prop_2_month_year_oh = "  & aClient_Aircraft_Propeller.cliacpr_prop_2_month_year_oh & vbnewline & " cliacpr_prop_3_month_year_oh = "  & aClient_Aircraft_Propeller.cliacpr_prop_3_month_year_oh & vbnewline & " cliacpr_prop_4_month_year_oh = "  & aClient_Aircraft_Propeller.cliacpr_prop_4_month_year_oh & vbnewline  


' return the string
Return ClassInformation
Catch ex As Exception
MsgBox("Error occured in classInfo. Class: clsClient_Aircraft_Propeller. Error:" & ex.Message)
Return Nothing
End Try
End Function


End Class
