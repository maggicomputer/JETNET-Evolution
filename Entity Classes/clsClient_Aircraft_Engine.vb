Option Strict On
Option Explicit On


' ***********************************************************************
' Class Comments Section
' ***********************************************************************
' Class name: clsClient_Aircraft_Engine
' Purpose: Configure all properties and methods for clsClient_Aircraft_Engine
' Parameters: None
' Return: None
' Change Log
'           06/21/2010 - Created By: Tom Jones


Public Class clsClient_Aircraft_Engine



'**************************************************
' Private variable declarations
'**************************************************
Private intcliacep_cliac_id As Integer
Private strcliacep_engine_name As String
Private intcliacep_engine_maintenance_program As Integer
Private intcliacep_engine_management_program As Integer
Private strcliacep_engine_tbo_oc_flag As String
    Private intcliacep_engine_noise_rating As Nullable(Of Integer)
Private strcliacep_engine_model_config As String
Private strcliacep_engine_overhaul_done_by_name As String
Private strcliacep_engine_overhaul_done_month_year As String
Private strcliacep_engine_hot_inspection_done_by_name As String
Private strcliacep_engine_hot_inspection_done_month_year As String
Private strcliacep_engine_1_ser_nbr As String
Private strcliacep_engine_2_ser_nbr As String
Private strcliacep_engine_3_ser_nbr As String
Private strcliacep_engine_4_ser_nbr As String
    Private intcliacep_engine_1_ttsn_hours As Nullable(Of Integer)
    Private intcliacep_engine_2_ttsn_hours As Nullable(Of Integer)
    Private intcliacep_engine_3_ttsn_hours As Nullable(Of Integer)
    Private intcliacep_engine_4_ttsn_hours As Nullable(Of Integer)
    Private intcliacep_engine_1_tsoh_hours As Nullable(Of Integer)
    Private intcliacep_engine_2_tsoh_hours As Nullable(Of Integer)
    Private intcliacep_engine_3_tsoh_hours As Nullable(Of Integer)
    Private intcliacep_engine_4_tsoh_hours As Nullable(Of Integer)
    Private intcliacep_engine_1_tshi_hours As Nullable(Of Integer)
    Private intcliacep_engine_2_tshi_hours As Nullable(Of Integer)
    Private intcliacep_engine_3_tshi_hours As Nullable(Of Integer)
    Private intcliacep_engine_4_tshi_hours As Nullable(Of Integer)
    Private intcliacep_engine_1_tbo_hours As Nullable(Of Integer)
    Private intcliacep_engine_2_tbo_hours As Nullable(Of Integer)
    Private intcliacep_engine_3_tbo_hours As Nullable(Of Integer)
    Private intcliacep_engine_4_tbo_hours As Nullable(Of Integer)
    Private intcliacep_engine_1_tsn_cycle As Nullable(Of Integer)
    Private intcliacep_engine_2_tsn_cycle As Nullable(Of Integer)
    Private intcliacep_engine_3_tsn_cycle As Nullable(Of Integer)
    Private intcliacep_engine_4_tsn_cycle As Nullable(Of Integer)
    Private intcliacep_engine_1_tsoh_cycle As Nullable(Of Integer)
    Private intcliacep_engine_2_tsoh_cycle As Nullable(Of Integer)
    Private intcliacep_engine_3_tsoh_cycle As Nullable(Of Integer)
    Private intcliacep_engine_4_tsoh_cycle As Nullable(Of Integer)
    Private intcliacep_engine_1_tshi_cycle As Nullable(Of Integer)
    Private intcliacep_engine_2_tshi_cycle As Nullable(Of Integer)
    Private intcliacep_engine_3_tshi_cycle As Nullable(Of Integer)
    Private intcliacep_engine_4_tshi_cycle As Nullable(Of Integer)


'**************************************************
' Setters and getters
'**************************************************
Public Property cliacep_cliac_id As Integer
    Get
       Return intcliacep_cliac_id
    End Get
    Set (ByVal Value As Integer)
        intcliacep_cliac_id = Value
    End Set
End Property


Public Property cliacep_engine_name As String
    Get
       Return strcliacep_engine_name
    End Get
    Set (ByVal Value As String)
        strcliacep_engine_name = Value
    End Set
End Property


Public Property cliacep_engine_maintenance_program As Integer
    Get
       Return intcliacep_engine_maintenance_program
    End Get
    Set (ByVal Value As Integer)
        intcliacep_engine_maintenance_program = Value
    End Set
End Property


Public Property cliacep_engine_management_program As Integer
    Get
       Return intcliacep_engine_management_program
    End Get
    Set (ByVal Value As Integer)
        intcliacep_engine_management_program = Value
    End Set
End Property


Public Property cliacep_engine_tbo_oc_flag As String
    Get
       Return strcliacep_engine_tbo_oc_flag
    End Get
    Set (ByVal Value As String)
        strcliacep_engine_tbo_oc_flag = Value
    End Set
End Property


    Public Property cliacep_engine_noise_rating() As Nullable(Of Integer)
        Get
            Return intcliacep_engine_noise_rating
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            intcliacep_engine_noise_rating = Value
        End Set
    End Property


Public Property cliacep_engine_model_config As String
    Get
       Return strcliacep_engine_model_config
    End Get
    Set (ByVal Value As String)
        strcliacep_engine_model_config = Value
    End Set
End Property


Public Property cliacep_engine_overhaul_done_by_name As String
    Get
       Return strcliacep_engine_overhaul_done_by_name
    End Get
    Set (ByVal Value As String)
        strcliacep_engine_overhaul_done_by_name = Value
    End Set
End Property


Public Property cliacep_engine_overhaul_done_month_year As String
    Get
       Return strcliacep_engine_overhaul_done_month_year
    End Get
    Set (ByVal Value As String)
        strcliacep_engine_overhaul_done_month_year = Value
    End Set
End Property


Public Property cliacep_engine_hot_inspection_done_by_name As String
    Get
       Return strcliacep_engine_hot_inspection_done_by_name
    End Get
    Set (ByVal Value As String)
        strcliacep_engine_hot_inspection_done_by_name = Value
    End Set
End Property


Public Property cliacep_engine_hot_inspection_done_month_year As String
    Get
       Return strcliacep_engine_hot_inspection_done_month_year
    End Get
    Set (ByVal Value As String)
        strcliacep_engine_hot_inspection_done_month_year = Value
    End Set
End Property


Public Property cliacep_engine_1_ser_nbr As String
    Get
       Return strcliacep_engine_1_ser_nbr
    End Get
    Set (ByVal Value As String)
        strcliacep_engine_1_ser_nbr = Value
    End Set
End Property


Public Property cliacep_engine_2_ser_nbr As String
    Get
       Return strcliacep_engine_2_ser_nbr
    End Get
    Set (ByVal Value As String)
        strcliacep_engine_2_ser_nbr = Value
    End Set
End Property


Public Property cliacep_engine_3_ser_nbr As String
    Get
       Return strcliacep_engine_3_ser_nbr
    End Get
    Set (ByVal Value As String)
        strcliacep_engine_3_ser_nbr = Value
    End Set
End Property


Public Property cliacep_engine_4_ser_nbr As String
    Get
       Return strcliacep_engine_4_ser_nbr
    End Get
    Set (ByVal Value As String)
        strcliacep_engine_4_ser_nbr = Value
    End Set
End Property


    Public Property cliacep_engine_1_ttsn_hours() As Nullable(Of Integer)
        Get
            Return intcliacep_engine_1_ttsn_hours
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            intcliacep_engine_1_ttsn_hours = Value
        End Set
    End Property


    Public Property cliacep_engine_2_ttsn_hours() As Nullable(Of Integer)
        Get
            Return intcliacep_engine_2_ttsn_hours
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            intcliacep_engine_2_ttsn_hours = Value
        End Set
    End Property


    Public Property cliacep_engine_3_ttsn_hours() As Nullable(Of Integer)
        Get
            Return intcliacep_engine_3_ttsn_hours
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            intcliacep_engine_3_ttsn_hours = Value
        End Set
    End Property


    Public Property cliacep_engine_4_ttsn_hours() As Nullable(Of Integer)
        Get
            Return intcliacep_engine_4_ttsn_hours
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            intcliacep_engine_4_ttsn_hours = Value
        End Set
    End Property


    Public Property cliacep_engine_1_tsoh_hours() As Nullable(Of Integer)
        Get
            Return intcliacep_engine_1_tsoh_hours
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            intcliacep_engine_1_tsoh_hours = Value
        End Set
    End Property


    Public Property cliacep_engine_2_tsoh_hours() As Nullable(Of Integer)
        Get
            Return intcliacep_engine_2_tsoh_hours
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            intcliacep_engine_2_tsoh_hours = Value
        End Set
    End Property


    Public Property cliacep_engine_3_tsoh_hours() As Nullable(Of Integer)
        Get
            Return intcliacep_engine_3_tsoh_hours
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            intcliacep_engine_3_tsoh_hours = Value
        End Set
    End Property


    Public Property cliacep_engine_4_tsoh_hours() As Nullable(Of Integer)
        Get
            Return intcliacep_engine_4_tsoh_hours
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            intcliacep_engine_4_tsoh_hours = Value
        End Set
    End Property


    Public Property cliacep_engine_1_tshi_hours() As Nullable(Of Integer)
        Get
            Return intcliacep_engine_1_tshi_hours
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            intcliacep_engine_1_tshi_hours = Value
        End Set
    End Property


    Public Property cliacep_engine_2_tshi_hours() As Nullable(Of Integer)
        Get
            Return intcliacep_engine_2_tshi_hours
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            intcliacep_engine_2_tshi_hours = Value
        End Set
    End Property


    Public Property cliacep_engine_3_tshi_hours() As Nullable(Of Integer)
        Get
            Return intcliacep_engine_3_tshi_hours
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            intcliacep_engine_3_tshi_hours = Value
        End Set
    End Property


    Public Property cliacep_engine_4_tshi_hours() As Nullable(Of Integer)
        Get
            Return intcliacep_engine_4_tshi_hours
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            intcliacep_engine_4_tshi_hours = Value
        End Set
    End Property


    Public Property cliacep_engine_1_tbo_hours() As Nullable(Of Integer)
        Get
            Return intcliacep_engine_1_tbo_hours
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            intcliacep_engine_1_tbo_hours = Value
        End Set
    End Property


    Public Property cliacep_engine_2_tbo_hours() As Nullable(Of Integer)
        Get
            Return intcliacep_engine_2_tbo_hours
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            intcliacep_engine_2_tbo_hours = Value
        End Set
    End Property


    Public Property cliacep_engine_3_tbo_hours() As Nullable(Of Integer)
        Get
            Return intcliacep_engine_3_tbo_hours
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            intcliacep_engine_3_tbo_hours = Value
        End Set
    End Property


    Public Property cliacep_engine_4_tbo_hours() As Nullable(Of Integer)
        Get
            Return intcliacep_engine_4_tbo_hours
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            intcliacep_engine_4_tbo_hours = Value
        End Set
    End Property


    Public Property cliacep_engine_1_tsn_cycle() As Nullable(Of Integer)
        Get
            Return intcliacep_engine_1_tsn_cycle
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            intcliacep_engine_1_tsn_cycle = Value
        End Set
    End Property


    Public Property cliacep_engine_2_tsn_cycle() As Nullable(Of Integer)
        Get
            Return intcliacep_engine_2_tsn_cycle
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            intcliacep_engine_2_tsn_cycle = Value
        End Set
    End Property


    Public Property cliacep_engine_3_tsn_cycle() As Nullable(Of Integer)
        Get
            Return intcliacep_engine_3_tsn_cycle
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            intcliacep_engine_3_tsn_cycle = Value
        End Set
    End Property


    Public Property cliacep_engine_4_tsn_cycle() As Nullable(Of Integer)
        Get
            Return intcliacep_engine_4_tsn_cycle
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            intcliacep_engine_4_tsn_cycle = Value
        End Set
    End Property


    Public Property cliacep_engine_1_tsoh_cycle() As Nullable(Of Integer)
        Get
            Return intcliacep_engine_1_tsoh_cycle
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            intcliacep_engine_1_tsoh_cycle = Value
        End Set
    End Property


    Public Property cliacep_engine_2_tsoh_cycle() As Nullable(Of Integer)
        Get
            Return intcliacep_engine_2_tsoh_cycle
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            intcliacep_engine_2_tsoh_cycle = Value
        End Set
    End Property


    Public Property cliacep_engine_3_tsoh_cycle() As Nullable(Of Integer)
        Get
            Return intcliacep_engine_3_tsoh_cycle
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            intcliacep_engine_3_tsoh_cycle = Value
        End Set
    End Property


    Public Property cliacep_engine_4_tsoh_cycle() As Nullable(Of Integer)
        Get
            Return intcliacep_engine_4_tsoh_cycle
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            intcliacep_engine_4_tsoh_cycle = Value
        End Set
    End Property


    Public Property cliacep_engine_1_tshi_cycle() As Nullable(Of Integer)
        Get
            Return intcliacep_engine_1_tshi_cycle
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            intcliacep_engine_1_tshi_cycle = Value
        End Set
    End Property


    Public Property cliacep_engine_2_tshi_cycle() As Nullable(Of Integer)
        Get
            Return intcliacep_engine_2_tshi_cycle
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            intcliacep_engine_2_tshi_cycle = Value
        End Set
    End Property


    Public Property cliacep_engine_3_tshi_cycle() As Nullable(Of Integer)
        Get
            Return intcliacep_engine_3_tshi_cycle
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            intcliacep_engine_3_tshi_cycle = Value
        End Set
    End Property


    Public Property cliacep_engine_4_tshi_cycle() As Nullable(Of Integer)
        Get
            Return intcliacep_engine_4_tshi_cycle
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            intcliacep_engine_4_tshi_cycle = Value
        End Set
    End Property


'**************************************************
' Constructors
'**************************************************


'   Default Constructor
Public Sub New()
    cliacep_cliac_id = 0
cliacep_engine_name = ""
    cliacep_engine_maintenance_program = 0
    cliacep_engine_management_program = 0
cliacep_engine_tbo_oc_flag = ""
        'cliacep_engine_noise_rating = 0
cliacep_engine_model_config = ""
cliacep_engine_overhaul_done_by_name = ""
cliacep_engine_overhaul_done_month_year = ""
cliacep_engine_hot_inspection_done_by_name = ""
cliacep_engine_hot_inspection_done_month_year = ""
cliacep_engine_1_ser_nbr = ""
cliacep_engine_2_ser_nbr = ""
cliacep_engine_3_ser_nbr = ""
cliacep_engine_4_ser_nbr = ""
        'cliacep_engine_1_ttsn_hours = 0
        'cliacep_engine_2_ttsn_hours = 0
        'cliacep_engine_3_ttsn_hours = 0
        'cliacep_engine_4_ttsn_hours = 0
        'cliacep_engine_1_tsoh_hours = 0
        'cliacep_engine_2_tsoh_hours = 0
        'cliacep_engine_3_tsoh_hours = 0
        'cliacep_engine_4_tsoh_hours = 0
        'cliacep_engine_1_tshi_hours = 0
        'cliacep_engine_2_tshi_hours = 0
        'cliacep_engine_3_tshi_hours = 0
        'cliacep_engine_4_tshi_hours = 0
        'cliacep_engine_1_tbo_hours = 0
        'cliacep_engine_2_tbo_hours = 0
        'cliacep_engine_3_tbo_hours = 0
        'cliacep_engine_4_tbo_hours = 0
        'cliacep_engine_1_tsn_cycle = 0
        'cliacep_engine_2_tsn_cycle = 0
        'cliacep_engine_3_tsn_cycle = 0
        'cliacep_engine_4_tsn_cycle = 0
        'cliacep_engine_1_tsoh_cycle = 0
        'cliacep_engine_2_tsoh_cycle = 0
        'cliacep_engine_3_tsoh_cycle = 0
        'cliacep_engine_4_tsoh_cycle = 0
        'cliacep_engine_1_tshi_cycle = 0
        'cliacep_engine_2_tshi_cycle = 0
        'cliacep_engine_3_tshi_cycle = 0
        'cliacep_engine_4_tshi_cycle = 0
End Sub


'   Parameter Based Constructor
Public Sub New(ByVal acliacep_cliac_id As Integer ,ByVal acliacep_engine_name As String ,ByVal acliacep_engine_maintenance_program As Integer ,ByVal acliacep_engine_management_program As Integer ,ByVal acliacep_engine_tbo_oc_flag As String ,ByVal acliacep_engine_noise_rating As Integer ,ByVal acliacep_engine_model_config As String ,ByVal acliacep_engine_overhaul_done_by_name As String ,ByVal acliacep_engine_overhaul_done_month_year As String ,ByVal acliacep_engine_hot_inspection_done_by_name As String ,ByVal acliacep_engine_hot_inspection_done_month_year As String ,ByVal acliacep_engine_1_ser_nbr As String ,ByVal acliacep_engine_2_ser_nbr As String ,ByVal acliacep_engine_3_ser_nbr As String ,ByVal acliacep_engine_4_ser_nbr As String ,ByVal acliacep_engine_1_ttsn_hours As Integer ,ByVal acliacep_engine_2_ttsn_hours As Integer ,ByVal acliacep_engine_3_ttsn_hours As Integer ,ByVal acliacep_engine_4_ttsn_hours As Integer ,ByVal acliacep_engine_1_tsoh_hours As Integer ,ByVal acliacep_engine_2_tsoh_hours As Integer ,ByVal acliacep_engine_3_tsoh_hours As Integer ,ByVal acliacep_engine_4_tsoh_hours As Integer ,ByVal acliacep_engine_1_tshi_hours As Integer ,ByVal acliacep_engine_2_tshi_hours As Integer ,ByVal acliacep_engine_3_tshi_hours As Integer ,ByVal acliacep_engine_4_tshi_hours As Integer ,ByVal acliacep_engine_1_tbo_hours As Integer ,ByVal acliacep_engine_2_tbo_hours As Integer ,ByVal acliacep_engine_3_tbo_hours As Integer ,ByVal acliacep_engine_4_tbo_hours As Integer ,ByVal acliacep_engine_1_tsn_cycle As Integer ,ByVal acliacep_engine_2_tsn_cycle As Integer ,ByVal acliacep_engine_3_tsn_cycle As Integer ,ByVal acliacep_engine_4_tsn_cycle As Integer ,ByVal acliacep_engine_1_tsoh_cycle As Integer ,ByVal acliacep_engine_2_tsoh_cycle As Integer ,ByVal acliacep_engine_3_tsoh_cycle As Integer ,ByVal acliacep_engine_4_tsoh_cycle As Integer ,ByVal acliacep_engine_1_tshi_cycle As Integer ,ByVal acliacep_engine_2_tshi_cycle As Integer ,ByVal acliacep_engine_3_tshi_cycle As Integer ,ByVal acliacep_engine_4_tshi_cycle As Integer)
cliacep_cliac_id = acliacep_cliac_id
cliacep_engine_name = acliacep_engine_name
cliacep_engine_maintenance_program = acliacep_engine_maintenance_program
cliacep_engine_management_program = acliacep_engine_management_program
cliacep_engine_tbo_oc_flag = acliacep_engine_tbo_oc_flag
cliacep_engine_noise_rating = acliacep_engine_noise_rating
cliacep_engine_model_config = acliacep_engine_model_config
cliacep_engine_overhaul_done_by_name = acliacep_engine_overhaul_done_by_name
cliacep_engine_overhaul_done_month_year = acliacep_engine_overhaul_done_month_year
cliacep_engine_hot_inspection_done_by_name = acliacep_engine_hot_inspection_done_by_name
cliacep_engine_hot_inspection_done_month_year = acliacep_engine_hot_inspection_done_month_year
cliacep_engine_1_ser_nbr = acliacep_engine_1_ser_nbr
cliacep_engine_2_ser_nbr = acliacep_engine_2_ser_nbr
cliacep_engine_3_ser_nbr = acliacep_engine_3_ser_nbr
cliacep_engine_4_ser_nbr = acliacep_engine_4_ser_nbr
cliacep_engine_1_ttsn_hours = acliacep_engine_1_ttsn_hours
cliacep_engine_2_ttsn_hours = acliacep_engine_2_ttsn_hours
cliacep_engine_3_ttsn_hours = acliacep_engine_3_ttsn_hours
cliacep_engine_4_ttsn_hours = acliacep_engine_4_ttsn_hours
cliacep_engine_1_tsoh_hours = acliacep_engine_1_tsoh_hours
cliacep_engine_2_tsoh_hours = acliacep_engine_2_tsoh_hours
cliacep_engine_3_tsoh_hours = acliacep_engine_3_tsoh_hours
cliacep_engine_4_tsoh_hours = acliacep_engine_4_tsoh_hours
cliacep_engine_1_tshi_hours = acliacep_engine_1_tshi_hours
cliacep_engine_2_tshi_hours = acliacep_engine_2_tshi_hours
cliacep_engine_3_tshi_hours = acliacep_engine_3_tshi_hours
cliacep_engine_4_tshi_hours = acliacep_engine_4_tshi_hours
cliacep_engine_1_tbo_hours = acliacep_engine_1_tbo_hours
cliacep_engine_2_tbo_hours = acliacep_engine_2_tbo_hours
cliacep_engine_3_tbo_hours = acliacep_engine_3_tbo_hours
cliacep_engine_4_tbo_hours = acliacep_engine_4_tbo_hours
cliacep_engine_1_tsn_cycle = acliacep_engine_1_tsn_cycle
cliacep_engine_2_tsn_cycle = acliacep_engine_2_tsn_cycle
cliacep_engine_3_tsn_cycle = acliacep_engine_3_tsn_cycle
cliacep_engine_4_tsn_cycle = acliacep_engine_4_tsn_cycle
cliacep_engine_1_tsoh_cycle = acliacep_engine_1_tsoh_cycle
cliacep_engine_2_tsoh_cycle = acliacep_engine_2_tsoh_cycle
cliacep_engine_3_tsoh_cycle = acliacep_engine_3_tsoh_cycle
cliacep_engine_4_tsoh_cycle = acliacep_engine_4_tsoh_cycle
cliacep_engine_1_tshi_cycle = acliacep_engine_1_tshi_cycle
cliacep_engine_2_tshi_cycle = acliacep_engine_2_tshi_cycle
cliacep_engine_3_tshi_cycle = acliacep_engine_3_tshi_cycle
cliacep_engine_4_tshi_cycle = acliacep_engine_4_tshi_cycle
End Sub




' ***********************************************************************
' Methods
' ***********************************************************************


' Method name: ClassInfo
' Purpose: to generate a string with all assigned parameters
' Parameters: Client_Aircraft_Engine
' Return: String with all assigned parameters
' Change Log
'           06/21/2010 - Created By: Tom Jones
Public Function ClassInfo(byval aClient_Aircraft_Engine as clsClient_Aircraft_Engine) as string
Try
Dim ClassInformation as String


ClassInformation = " cliacep_cliac_id = "  & aClient_Aircraft_Engine.cliacep_cliac_id & vbnewline & " cliacep_engine_name = "  & aClient_Aircraft_Engine.cliacep_engine_name & vbnewline & " cliacep_engine_maintenance_program = "  & aClient_Aircraft_Engine.cliacep_engine_maintenance_program & vbnewline & " cliacep_engine_management_program = "  & aClient_Aircraft_Engine.cliacep_engine_management_program & vbnewline & " cliacep_engine_tbo_oc_flag = "  & aClient_Aircraft_Engine.cliacep_engine_tbo_oc_flag & vbnewline & " cliacep_engine_noise_rating = "  & aClient_Aircraft_Engine.cliacep_engine_noise_rating & vbnewline & " cliacep_engine_model_config = "  & aClient_Aircraft_Engine.cliacep_engine_model_config & vbnewline & " cliacep_engine_overhaul_done_by_name = "  & aClient_Aircraft_Engine.cliacep_engine_overhaul_done_by_name & vbnewline & " cliacep_engine_overhaul_done_month_year = "  & aClient_Aircraft_Engine.cliacep_engine_overhaul_done_month_year & vbnewline & " cliacep_engine_hot_inspection_done_by_name = "  & aClient_Aircraft_Engine.cliacep_engine_hot_inspection_done_by_name & vbnewline & " cliacep_engine_hot_inspection_done_month_year = "  & aClient_Aircraft_Engine.cliacep_engine_hot_inspection_done_month_year & vbnewline & " cliacep_engine_1_ser_nbr = "  & aClient_Aircraft_Engine.cliacep_engine_1_ser_nbr & vbnewline & " cliacep_engine_2_ser_nbr = "  & aClient_Aircraft_Engine.cliacep_engine_2_ser_nbr & vbnewline & " cliacep_engine_3_ser_nbr = "  & aClient_Aircraft_Engine.cliacep_engine_3_ser_nbr & vbnewline & " cliacep_engine_4_ser_nbr = "  & aClient_Aircraft_Engine.cliacep_engine_4_ser_nbr & vbnewline & " cliacep_engine_1_ttsn_hours = "  & aClient_Aircraft_Engine.cliacep_engine_1_ttsn_hours & vbnewline & " cliacep_engine_2_ttsn_hours = "  & aClient_Aircraft_Engine.cliacep_engine_2_ttsn_hours & vbnewline & " cliacep_engine_3_ttsn_hours = "  & aClient_Aircraft_Engine.cliacep_engine_3_ttsn_hours & vbnewline & " cliacep_engine_4_ttsn_hours = "  & aClient_Aircraft_Engine.cliacep_engine_4_ttsn_hours & vbnewline & " cliacep_engine_1_tsoh_hours = "  & aClient_Aircraft_Engine.cliacep_engine_1_tsoh_hours & vbnewline & " cliacep_engine_2_tsoh_hours = "  & aClient_Aircraft_Engine.cliacep_engine_2_tsoh_hours & vbnewline & " cliacep_engine_3_tsoh_hours = "  & aClient_Aircraft_Engine.cliacep_engine_3_tsoh_hours & vbnewline & " cliacep_engine_4_tsoh_hours = "  & aClient_Aircraft_Engine.cliacep_engine_4_tsoh_hours & vbnewline & " cliacep_engine_1_tshi_hours = "  & aClient_Aircraft_Engine.cliacep_engine_1_tshi_hours & vbnewline & " cliacep_engine_2_tshi_hours = "  & aClient_Aircraft_Engine.cliacep_engine_2_tshi_hours & vbnewline & " cliacep_engine_3_tshi_hours = "  & aClient_Aircraft_Engine.cliacep_engine_3_tshi_hours & vbnewline & " cliacep_engine_4_tshi_hours = "  & aClient_Aircraft_Engine.cliacep_engine_4_tshi_hours & vbnewline & " cliacep_engine_1_tbo_hours = "  & aClient_Aircraft_Engine.cliacep_engine_1_tbo_hours & vbnewline & " cliacep_engine_2_tbo_hours = "  & aClient_Aircraft_Engine.cliacep_engine_2_tbo_hours & vbnewline & " cliacep_engine_3_tbo_hours = "  & aClient_Aircraft_Engine.cliacep_engine_3_tbo_hours & vbnewline & " cliacep_engine_4_tbo_hours = "  & aClient_Aircraft_Engine.cliacep_engine_4_tbo_hours & vbnewline & " cliacep_engine_1_tsn_cycle = "  & aClient_Aircraft_Engine.cliacep_engine_1_tsn_cycle & vbnewline & " cliacep_engine_2_tsn_cycle = "  & aClient_Aircraft_Engine.cliacep_engine_2_tsn_cycle & vbnewline & " cliacep_engine_3_tsn_cycle = "  & aClient_Aircraft_Engine.cliacep_engine_3_tsn_cycle & vbnewline & " cliacep_engine_4_tsn_cycle = "  & aClient_Aircraft_Engine.cliacep_engine_4_tsn_cycle & vbnewline & " cliacep_engine_1_tsoh_cycle = "  & aClient_Aircraft_Engine.cliacep_engine_1_tsoh_cycle & vbnewline & " cliacep_engine_2_tsoh_cycle = "  & aClient_Aircraft_Engine.cliacep_engine_2_tsoh_cycle & vbnewline & " cliacep_engine_3_tsoh_cycle = "  & aClient_Aircraft_Engine.cliacep_engine_3_tsoh_cycle & vbnewline & " cliacep_engine_4_tsoh_cycle = "  & aClient_Aircraft_Engine.cliacep_engine_4_tsoh_cycle & vbnewline & " cliacep_engine_1_tshi_cycle = "  & aClient_Aircraft_Engine.cliacep_engine_1_tshi_cycle & vbnewline & " cliacep_engine_2_tshi_cycle = "  & aClient_Aircraft_Engine.cliacep_engine_2_tshi_cycle & vbnewline & " cliacep_engine_3_tshi_cycle = "  & aClient_Aircraft_Engine.cliacep_engine_3_tshi_cycle & vbnewline & " cliacep_engine_4_tshi_cycle = "  & aClient_Aircraft_Engine.cliacep_engine_4_tshi_cycle & vbnewline  


' return the string
Return ClassInformation
Catch ex As Exception
MsgBox("Error occured in classInfo. Class: clsClient_Aircraft_Engine. Error:" & ex.Message)
Return Nothing
End Try
End Function


End Class
