Option Strict On
Option Explicit On


' ***********************************************************************
' Class Comments Section
' ***********************************************************************
' Class name: clsLocal_Notes
' Purpose: Configure all properties and methods for clsLocal_Notes
' Parameters: None
' Return: None
' Change Log
'           1/22/2010 - Created By: Tom Jones


Public Class clsLocal_Notes



    '**************************************************
    ' Private variable declarations
    '**************************************************
    Private strlnote_schedule_start_date As Nullable(Of DateTime)
    Private strlnote_schedule_end_date As Nullable(Of DateTime)

    Private declnote_wanted_max_price As Nullable(Of Decimal)
    Private intlnote_wanted_max_aftt As Nullable(Of Integer)
    Private intlnote_id As Integer
    Private intlnote_jetnet_ac_id As Integer
    Private intlnote_jetnet_comp_id As Integer
    Private intlnote_client_ac_id As Integer
    Private intlnote_client_comp_id As Integer
    Private intlnote_notecat_key As Integer
    Private intlnote_user_id As Integer
    Private intlnote_clipri_ID As Integer
    Private intlnote_client_contact_id As Integer
    Private intlnote_jetnet_contact_id As Integer
    Private intlnote_jetnet_amod_id As Integer
    Private intlnote_client_amod_id As Integer
    Private strlnote_cash_value As Nullable(Of Integer)
    Private strlnote_capture_percentage As Nullable(Of Integer)
    Private lnglnote_jetnet_yacht_id As Long = 0
    Private lnglnote_jetnet_yacht_model_id As Long = 0

    Private strlnote_status As String
    Private strlnote_note As String
    Private strlnote_entry_date As String
    Private strlnote_action_date As String
    Private strlnote_user_login As String
    Private strlnote_user_name As String
    Private strlnote_document_flag As String
    Private strlnote_document_name As String
    Private strlnote_opportunity_status As String
    Private strlnote_wanted_end_year As String
    Private strlnote_wanted_damage_hist As String
    Private strlnote_wanted_damage_cur As String
  Private strlnote_wanted_start_year As String

 
  Private strlnote_estval_type As String = ""
  Private lnglnote_estval_asking_price_value As Long = 0
  Private lnglnote_estval_take_price_value As Long = 0
  Private lnglnote_estval_estimated_value_value As Long = 0
  Private lnglnote_estval_aftt_value As Long = 0
  Private lnglnote_estval_total_landings_value As Long = 0
 

    '**************************************************
    ' Setters and getters
    '**************************************************
    Public Property lnote_schedule_start_date() As Nullable(Of DateTime)
        Get
            Return strlnote_schedule_start_date
        End Get
        Set(ByVal Value As Nullable(Of DateTime))
            strlnote_schedule_start_date = Value
        End Set
    End Property

    Public Property lnote_schedule_end_date() As Nullable(Of DateTime)
        Get
            Return strlnote_schedule_end_date
        End Get
        Set(ByVal Value As Nullable(Of DateTime))
            strlnote_schedule_end_date = Value
        End Set
    End Property


    Public Property lnote_id() As Integer
        Get
            Return intlnote_id
        End Get
        Set(ByVal Value As Integer)
            intlnote_id = Value
        End Set
    End Property


    Public Property lnote_jetnet_ac_id() As Integer
        Get
            Return intlnote_jetnet_ac_id
        End Get
        Set(ByVal Value As Integer)
            intlnote_jetnet_ac_id = Value
        End Set
    End Property

    Public Property lnote_jetnet_yacht_id() As Long
        Get
            Return lnglnote_jetnet_yacht_id
        End Get
        Set(ByVal Value As Long)
            lnglnote_jetnet_yacht_id = Value
        End Set
    End Property

    Public Property lnote_jetnet_yacht_model_id() As Long
        Get
            Return lnglnote_jetnet_yacht_model_id
        End Get
        Set(ByVal Value As Long)
            lnglnote_jetnet_yacht_model_id = Value
        End Set
    End Property

    Public Property lnote_jetnet_comp_id() As Integer
        Get
            Return intlnote_jetnet_comp_id
        End Get
        Set(ByVal Value As Integer)
            intlnote_jetnet_comp_id = Value
        End Set
    End Property


    Public Property lnote_client_ac_id() As Integer
        Get
            Return intlnote_client_ac_id
        End Get
        Set(ByVal Value As Integer)
            intlnote_client_ac_id = Value
        End Set
    End Property


    Public Property lnote_client_comp_id() As Integer
        Get
            Return intlnote_client_comp_id
        End Get
        Set(ByVal Value As Integer)
            intlnote_client_comp_id = Value
        End Set
    End Property

    Public Property lnote_cash_value() As Nullable(Of Integer)
        Get
            Return strlnote_cash_value
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            strlnote_cash_value = Value
        End Set
    End Property

    Public Property lnote_capture_percentage() As Nullable(Of Integer)
        Get
            Return strlnote_capture_percentage
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            strlnote_capture_percentage = Value
        End Set
    End Property

    Public Property lnote_notecat_key() As Integer
        Get
            Return intlnote_notecat_key
        End Get
        Set(ByVal Value As Integer)
            intlnote_notecat_key = Value
        End Set
    End Property

    Public Property lnote_user_id() As Integer
        Get
            Return intlnote_user_id
        End Get
        Set(ByVal Value As Integer)
            intlnote_user_id = Value
        End Set
    End Property

    Public Property lnote_clipri_ID() As Integer
        Get
            Return intlnote_clipri_ID
        End Get
        Set(ByVal Value As Integer)
            intlnote_clipri_ID = Value
        End Set
    End Property

    Public Property lnote_client_contact_id() As Integer
        Get
            Return intlnote_client_contact_id
        End Get
        Set(ByVal Value As Integer)
            intlnote_client_contact_id = Value
        End Set
    End Property

    Public Property lnote_jetnet_contact_id() As Integer
        Get
            Return intlnote_jetnet_contact_id
        End Get
        Set(ByVal Value As Integer)
            intlnote_jetnet_contact_id = Value
        End Set
    End Property

    Public Property lnote_jetnet_amod_id() As Integer
        Get
            Return intlnote_jetnet_amod_id
        End Get
        Set(ByVal Value As Integer)
            intlnote_jetnet_amod_id = Value
        End Set
    End Property

    Public Property lnote_client_amod_id() As Integer
        Get
            Return intlnote_client_amod_id
        End Get
        Set(ByVal Value As Integer)
            intlnote_client_amod_id = Value
        End Set
    End Property

    Public Property lnote_wanted_max_aftt() As Nullable(Of Integer)
        Get
            Return intlnote_wanted_max_aftt
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            intlnote_wanted_max_aftt = Value
        End Set
    End Property
    Public Property lnote_wanted_max_price() As Nullable(Of Decimal)
        Get
            Return declnote_wanted_max_price
        End Get
        Set(ByVal Value As Nullable(Of Decimal))
            declnote_wanted_max_price = Value
        End Set
    End Property

    Public Property lnote_note() As String
        Get
            Return strlnote_note
        End Get
        Set(ByVal Value As String)
            strlnote_note = Value
        End Set
    End Property

    Public Property lnote_entry_date() As String
        Get
            Return strlnote_entry_date
        End Get
        Set(ByVal Value As String)
            strlnote_entry_date = Value
        End Set
    End Property

    Public Property lnote_action_date() As String
        Get
            Return strlnote_action_date
        End Get
        Set(ByVal Value As String)
            strlnote_action_date = Value
        End Set
    End Property

    Public Property lnote_user_login() As String
        Get
            Return strlnote_user_login
        End Get
        Set(ByVal Value As String)
            strlnote_user_login = Value
        End Set
    End Property

    Public Property lnote_user_name() As String
        Get
            Return strlnote_user_name
        End Get
        Set(ByVal Value As String)
            strlnote_user_name = Value
        End Set
    End Property
    Public Property lnote_opportunity_status() As String
        Get
            Return strlnote_opportunity_status
        End Get
        Set(ByVal Value As String)
            strlnote_opportunity_status = Value
        End Set
    End Property

    Public Property lnote_status() As String
        Get
            Return strlnote_status
        End Get
        Set(ByVal Value As String)
            strlnote_status = Value
        End Set
    End Property

    Public Property lnote_document_flag() As String
        Get
            Return strlnote_document_flag
        End Get
        Set(ByVal Value As String)
            strlnote_document_flag = Value
        End Set
    End Property

    Public Property lnote_document_name() As String
        Get
            Return strlnote_document_name
        End Get
        Set(ByVal Value As String)
            strlnote_document_name = Value
        End Set
    End Property

    Public Property lnote_wanted_end_year() As String
        Get
            Return strlnote_wanted_end_year
        End Get
        Set(ByVal Value As String)
            strlnote_wanted_end_year = Value
        End Set
    End Property

    Public Property lnote_wanted_start_year() As String
        Get
            Return strlnote_wanted_start_year
        End Get
        Set(ByVal Value As String)
            strlnote_wanted_start_year = Value
        End Set
    End Property

    Public Property lnote_wanted_damage_hist() As String
        Get
            Return strlnote_wanted_damage_hist
        End Get
        Set(ByVal Value As String)
            strlnote_wanted_damage_hist = Value
        End Set
    End Property

  Public Property lnote_wanted_damage_cur() As String
    Get
      Return strlnote_wanted_damage_cur
    End Get
    Set(ByVal Value As String)
      strlnote_wanted_damage_cur = Value
    End Set
  End Property

  Public Property lnote_estval_asking_price() As Long
    Get
      Return lnglnote_estval_asking_price_value
    End Get
    Set(ByVal Value As Long)
      lnglnote_estval_asking_price_value = Value
    End Set
  End Property


  Public Property lnote_estval_take_price() As Long
    Get
      Return lnglnote_estval_take_price_value
    End Get
    Set(ByVal Value As Long)
      lnglnote_estval_take_price_value = Value
    End Set
  End Property


  Public Property lnote_estval_estimated_value() As Long
    Get
      Return lnglnote_estval_estimated_value_value
    End Get
    Set(ByVal Value As Long)
      lnglnote_estval_estimated_value_value = Value
    End Set
  End Property
 
  Public Property lnote_estval_aftt() As Long
    Get
      Return lnglnote_estval_aftt_value
    End Get
    Set(ByVal Value As Long)
      lnglnote_estval_aftt_value = Value
    End Set
  End Property
 
  Public Property lnote_estval_total_landings() As Long
    Get
      Return lnglnote_estval_total_landings_value
    End Get
    Set(ByVal Value As Long)
      lnglnote_estval_total_landings_value = Value
    End Set
  End Property

  Public Property lnote_estval_type() As String
    Get
      Return strlnote_estval_type
    End Get
    Set(ByVal Value As String)
      strlnote_estval_type = Value
    End Set
  End Property




    '**************************************************
    ' Constructors
    '**************************************************


    '   Default Constructor
    Public Sub New()
        lnote_id = 0
        lnote_jetnet_ac_id = 0
        lnote_jetnet_comp_id = 0
        lnote_client_ac_id = 0
        lnote_client_comp_id = 0
        lnote_jetnet_yacht_id = 0
        lnote_jetnet_yacht_model_id = 0
        lnote_note = ""
        lnote_entry_date = ""
        lnote_action_date = ""
        lnote_user_login = ""
        lnote_user_name = ""
        lnote_notecat_key = 0
        lnote_status = ""
        lnote_document_name = ""
        'lnote_schedule_start_date = ""
        'lnote_schedule_end_date = ""
        lnote_user_id = 0
        lnote_clipri_ID = 0
        lnote_client_contact_id = 0
        lnote_jetnet_contact_id = 0
        lnote_document_flag = "N"
        lnote_jetnet_amod_id = 0
        lnote_client_amod_id = 0
        lnote_opportunity_status = ""
        ' lnote_cash_value = 0
        ' lnote_capture_percentage = 0
        lnote_wanted_end_year = ""
        lnote_wanted_start_year = ""
        lnote_wanted_damage_cur = ""
    lnote_wanted_damage_hist = ""
    'ADDED IN MSW -5/31/16
    lnote_estval_asking_price = 0
    lnote_estval_take_price = 0
    lnote_estval_estimated_value = 0
    lnote_estval_aftt = 0
    lnote_estval_total_landings = 0
    lnote_estval_type = ""


    End Sub


    '   Parameter Based Constructor
    Public Sub New(ByVal alnote_id As Integer, ByVal alnote_jetnet_ac_id As Integer, ByVal alnote_jetnet_comp_id As Integer, ByVal alnote_client_ac_id As Integer, ByVal alnote_client_comp_id As Integer, ByVal alnote_note As String, ByVal alnote_entry_date As String, ByVal alnote_action_date As String, ByVal alnote_user_login As String, ByVal alnote_user_name As String, ByVal alnote_notecat_key As Integer, ByVal alnote_status As String, ByVal alnote_schedule_start_date As Nullable(Of DateTime), ByVal alnote_schedule_end_date As Nullable(Of DateTime), ByVal alnote_user_id As Integer, ByVal alnote_clipri_ID As Integer, ByVal alnote_client_contact_id As Integer, ByVal alnote_jetnet_contact_id As Integer, ByVal alnote_document_flag As String, ByVal alnote_document_name As String, ByVal alnote_jetnet_amod_id As Integer, ByVal alnote_client_amod_id As Integer, ByVal alnote_wanted_start_year As String, ByVal alnote_wanted_end_year As String, ByVal alnote_wanted_max_aftt As Nullable(Of Integer), ByVal alnote_wanted_damage_hist As String, ByVal alnote_wanted_damage_cur As String, ByVal alnote_wanted_max_price As Nullable(Of Decimal), ByVal alnote_jetnet_yacht_id As Long, ByVal alnote_jetnet_yacht_model_id As Long)

        lnote_id = alnote_id
        lnote_jetnet_ac_id = alnote_jetnet_ac_id
        lnote_jetnet_comp_id = alnote_jetnet_comp_id
        lnote_client_ac_id = alnote_client_ac_id
        lnote_client_comp_id = alnote_client_comp_id
        lnote_note = alnote_note
        lnote_entry_date = alnote_entry_date
        lnote_action_date = alnote_action_date
        lnote_user_login = alnote_user_login
        lnote_user_name = alnote_user_name
        lnote_notecat_key = alnote_notecat_key
        lnote_status = alnote_status
        lnote_schedule_start_date = alnote_schedule_start_date
        lnote_schedule_end_date = alnote_schedule_end_date
        lnote_user_id = alnote_user_id
        lnote_clipri_ID = alnote_clipri_ID
        lnote_client_contact_id = alnote_client_contact_id
        lnote_jetnet_contact_id = alnote_jetnet_contact_id
        lnote_document_flag = alnote_document_flag
        lnote_document_name = alnote_document_name
        lnote_jetnet_amod_id = alnote_jetnet_amod_id
        lnote_client_amod_id = alnote_client_amod_id
        lnote_wanted_damage_cur = alnote_wanted_damage_cur
        lnote_wanted_damage_hist = alnote_wanted_damage_hist
        lnote_wanted_end_year = alnote_wanted_end_year
        lnote_wanted_start_year = alnote_wanted_start_year
        lnote_wanted_max_price = alnote_wanted_max_price
        lnote_wanted_max_aftt = alnote_wanted_max_aftt

        lnote_jetnet_yacht_id = alnote_jetnet_yacht_id
    lnote_jetnet_yacht_model_id = alnote_jetnet_yacht_model_id


    'ADDED IN MSW -5/31/16
    lnote_estval_asking_price = lnglnote_estval_asking_price_value
    lnote_estval_take_price = lnglnote_estval_take_price_value
    lnote_estval_estimated_value = lnglnote_estval_estimated_value_value
    lnote_estval_aftt = lnglnote_estval_aftt_value
    lnote_estval_total_landings = lnglnote_estval_total_landings_value
    lnote_estval_type = ""

    End Sub




    ' ***********************************************************************
    ' Methods
    ' ***********************************************************************


    ' Method name: ClassInfo
    ' Purpose: to generate a string with all assigned parameters
    ' Parameters: Local_Notes
    ' Return: String with all assigned parameters
    ' Change Log
    '           1/22/2010 - Created By: Tom Jones
    Public Function ClassInfo(ByVal aLocal_Notes As clsLocal_Notes) As String
        Try
            Dim ClassInformation As String


            ClassInformation = " lnote_id = " & aLocal_Notes.lnote_id & vbNewLine
            ClassInformation = ClassInformation & " lnote_jetnet_ac_id = " & aLocal_Notes.lnote_jetnet_ac_id & vbNewLine
            ClassInformation = ClassInformation & " lnote_jetnet_comp_id = " & aLocal_Notes.lnote_jetnet_comp_id & vbNewLine
            ClassInformation = ClassInformation & " lnote_client_ac_id = " & aLocal_Notes.lnote_client_ac_id & vbNewLine
            ClassInformation = ClassInformation & " lnote_client_comp_id = " & aLocal_Notes.lnote_client_comp_id & vbNewLine
            ClassInformation = ClassInformation & " lnote_note = " & aLocal_Notes.lnote_note & vbNewLine
            ClassInformation = ClassInformation & " lnote_entry_date = " & aLocal_Notes.lnote_entry_date & vbNewLine
            ClassInformation = ClassInformation & " lnote_action_date = " & aLocal_Notes.strlnote_action_date & vbNewLine
            ClassInformation = ClassInformation & " lnote_user_login = " & aLocal_Notes.lnote_user_login & vbNewLine
            ClassInformation = ClassInformation & " lnote_user_name = " & aLocal_Notes.lnote_user_name & vbNewLine
            ClassInformation = ClassInformation & " lnote_notecat_key = " & aLocal_Notes.lnote_notecat_key & vbNewLine
            ClassInformation = ClassInformation & " lnote_status = " & aLocal_Notes.lnote_status & vbNewLine
            ClassInformation = ClassInformation & " lnote_schedule_start_date = " & aLocal_Notes.strlnote_schedule_start_date & vbNewLine
            ClassInformation = ClassInformation & "lnote_schedule_end_date = " & aLocal_Notes.strlnote_schedule_end_date & vbNewLine
            ClassInformation = ClassInformation & "lnote_user_id = " & aLocal_Notes.intlnote_user_id & vbNewLine
            ClassInformation = ClassInformation & " & lnote_clipri_ID = " & aLocal_Notes.lnote_clipri_ID & vbNewLine
            ClassInformation = ClassInformation & " lnote_document_flag = " & aLocal_Notes.lnote_document_flag & vbNewLine
            ClassInformation = ClassInformation & " lnote_jetnet_amod_id = " & aLocal_Notes.lnote_jetnet_amod_id & vbNewLine
            ClassInformation = ClassInformation & " lnote_client_amod_id = " & aLocal_Notes.lnote_client_amod_id & vbNewLine

            ClassInformation = ClassInformation & " lnote_client_amod_id = " & aLocal_Notes.lnote_wanted_damage_cur & vbNewLine
            ClassInformation = ClassInformation & " lnote_client_amod_id = " & aLocal_Notes.lnote_wanted_damage_hist & vbNewLine
            ClassInformation = ClassInformation & " lnote_client_amod_id = " & aLocal_Notes.lnote_wanted_end_year & vbNewLine
            ClassInformation = ClassInformation & " lnote_client_amod_id = " & aLocal_Notes.lnote_wanted_start_year & vbNewLine
            ClassInformation = ClassInformation & " lnote_client_amod_id = " & aLocal_Notes.lnote_wanted_max_price & vbNewLine
            ClassInformation = ClassInformation & " lnote_client_amod_id = " & aLocal_Notes.lnote_wanted_max_aftt & vbNewLine
            ClassInformation = ClassInformation & " lnote_jetnet_yacht_id = " & aLocal_Notes.lnote_jetnet_yacht_id & vbNewLine
      ClassInformation = ClassInformation & " lnote_jetnet_model_id = " & aLocal_Notes.lnote_jetnet_yacht_model_id & vbNewLine

      'ADDED IN MSW -5/31/16
      ClassInformation = ClassInformation & " lnote_estval_asking_price = " & aLocal_Notes.lnote_estval_asking_price & vbNewLine
      ClassInformation = ClassInformation & " lnote_estval_take_price = " & aLocal_Notes.lnote_estval_take_price & vbNewLine
      ClassInformation = ClassInformation & " lnote_estval_estimated_value = " & aLocal_Notes.lnote_estval_estimated_value & vbNewLine
      ClassInformation = ClassInformation & " lnote_estval_aftt = " & aLocal_Notes.lnote_estval_aftt & vbNewLine
      ClassInformation = ClassInformation & " lnote_estval_total_landings = " & aLocal_Notes.lnote_estval_total_landings & vbNewLine
      ClassInformation = ClassInformation & " lnote_estval_type = " & aLocal_Notes.lnote_estval_type & vbNewLine


 
            ' return the string
            Return ClassInformation
        Catch ex As Exception
            'MsgBox("Error occured in classInfo. Class: clsLocal_Notes. Error:" & ex.Message)
            Return Nothing
        End Try
    End Function


End Class
