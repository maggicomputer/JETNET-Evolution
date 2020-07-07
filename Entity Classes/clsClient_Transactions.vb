Option Strict On
Option Explicit On


' ***********************************************************************
' Class Comments Section
' ***********************************************************************
' Class name: clsClient_Transactions
' Purpose: Configure all properties and methods for clsClient_Transactions
' Parameters: None
' Return: None
' Change Log
'           06/29/2010 - Created By: Tom Jones


Public Class clsClient_Transactions



  '**************************************************
  ' Private variable declarations
  '**************************************************
  Private intclitrans_id As Integer
  Private intclitrans_jetnet_trans_id As Integer
  Private lngclitrans_cliac_id As Long
  Private lngclitrans_jetnet_ac_id As Long
  Private intclitrans_cliamod_id As Integer
  Private intclitrans_lifecycle As Integer
  Private strclitrans_ownership As String
  Private strclitrans_date As System.DateTime
  Private strclitrans_type As String
  Private strclitrans_subject As String
  Private strclitrans_customer_note As String
  Private strclitrans_newac_flag As String
  Private strclitrans_internal_trans_flag As String
  Private strclitrans_date_listed As System.DateTime
  Private strclitrans_exclusive_flag As String
  Private strclitrans_asking_wordage As String
  Private dblclitrans_asking_price As Double
  Private strclitrans_ser_nbr As String
  Private strclitrans_reg_nbr As String
  Private strclitrans_country_of_registration As String
  Private intclitrans_airframe_total_hours As Integer
  Private intclitrans_airframe_total_landings As Integer
  Private strclitrans_aport_iata_code As String
  Private strclitrans_aport_icao_code As String
  Private strclitrans_aport_name As String
  Private strclitrans_aport_state As String
  Private strclitrans_aport_country As String
  Private strclitrans_aport_city As String
  Private strclitrans_aport_private As String
  Private strclitrans_action_date As System.DateTime
  Private dblclitrans_sold_price As Double
  Private strclitrans_sold_price_type As String
  Private strclitrans_deal_type As String
  Private strclitrans_year_mfr As String
  Private dblclitrans_est_price As Double
  Private strclitrans_value_description As String


  'New fields added 5/4/2015

  Private strclitrans_subcategory_code As String
  Private strclitrans_subcat_code_part1 As String
  Private strclitrans_subcat_code_part2 As String
  Private strclitrans_subcat_code_part3 As String
  Private strclitrans_retail_flag As String

  '**************************************************
  ' Setters and getters
  '**************************************************
  'New fields added 5/4/2015
  Public Property clitrans_subcategory_code() As String
    Get
      Return strclitrans_subcategory_code
    End Get
    Set(ByVal Value As String)
      strclitrans_subcategory_code = Value
    End Set
  End Property
  Public Property clitrans_subcat_code_part1() As String
    Get
      Return strclitrans_subcat_code_part1
    End Get
    Set(ByVal Value As String)
      strclitrans_subcat_code_part1 = Value
    End Set
  End Property
  Public Property clitrans_subcat_code_part2() As String
    Get
      Return strclitrans_subcat_code_part2
    End Get
    Set(ByVal Value As String)
      strclitrans_subcat_code_part2 = Value
    End Set
  End Property
  Public Property clitrans_subcat_code_part3() As String
    Get
      Return strclitrans_subcat_code_part3
    End Get
    Set(ByVal Value As String)
      strclitrans_subcat_code_part3 = Value
    End Set
  End Property
  Public Property clitrans_retail_flag() As String
    Get
      Return strclitrans_retail_flag
    End Get
    Set(ByVal Value As String)
      strclitrans_retail_flag = Value
    End Set
  End Property
  'End new fields

  Public Property clitrans_id() As Integer
    Get
      Return intclitrans_id
    End Get
    Set(ByVal Value As Integer)
      intclitrans_id = Value
    End Set
  End Property


  Public Property clitrans_jetnet_trans_id() As Integer
    Get
      Return intclitrans_jetnet_trans_id
    End Get
    Set(ByVal Value As Integer)
      intclitrans_jetnet_trans_id = Value
    End Set
  End Property


  Public Property clitrans_cliac_id() As Long
    Get
      Return lngclitrans_cliac_id
    End Get
    Set(ByVal Value As Long)
      lngclitrans_cliac_id = Value
    End Set
  End Property

  Public Property clitrans_jetnet_ac_id() As Long
    Get
      Return lngclitrans_jetnet_ac_id
    End Get
    Set(ByVal Value As Long)
      lngclitrans_jetnet_ac_id = Value
    End Set
  End Property
  Public Property clitrans_cliamod_id() As Integer
    Get
      Return intclitrans_cliamod_id
    End Get
    Set(ByVal Value As Integer)
      intclitrans_cliamod_id = Value
    End Set
  End Property


  Public Property clitrans_lifecycle() As Integer
    Get
      Return intclitrans_lifecycle
    End Get
    Set(ByVal Value As Integer)
      intclitrans_lifecycle = Value
    End Set
  End Property


  Public Property clitrans_ownership() As String
    Get
      Return strclitrans_ownership
    End Get
    Set(ByVal Value As String)
      strclitrans_ownership = Value
    End Set
  End Property


  Public Property clitrans_date() As System.DateTime
    Get
      Return strclitrans_date
    End Get
    Set(ByVal Value As System.DateTime)
      strclitrans_date = Value
    End Set
  End Property
  Public Property clitrans_type() As String
    Get
      Return strclitrans_type
    End Get
    Set(ByVal Value As String)
      strclitrans_type = Value
    End Set
  End Property
  Public Property clitrans_deal_type() As String
    Get
      Return strclitrans_deal_type
    End Get
    Set(ByVal Value As String)
      strclitrans_deal_type = Value
    End Set
  End Property
  Public Property clitrans_year_mfr() As String
    Get
      Return strclitrans_year_mfr
    End Get
    Set(ByVal Value As String)
      strclitrans_year_mfr = Value
    End Set
  End Property

  'strclitrans_value_description
  Public Property clitrans_value_description() As String
    Get
      Return strclitrans_value_description
    End Get
    Set(ByVal Value As String)
      strclitrans_value_description = Value
    End Set
  End Property

  Public Property clitrans_subject() As String
    Get
      Return strclitrans_subject
    End Get
    Set(ByVal Value As String)
      strclitrans_subject = Value
    End Set
  End Property


  Public Property clitrans_customer_note() As String
    Get
      Return strclitrans_customer_note
    End Get
    Set(ByVal Value As String)
      strclitrans_customer_note = Value
    End Set
  End Property


  Public Property clitrans_newac_flag() As String
    Get
      Return strclitrans_newac_flag
    End Get
    Set(ByVal Value As String)
      strclitrans_newac_flag = Value
    End Set
  End Property


  Public Property clitrans_internal_trans_flag() As String
    Get
      Return strclitrans_internal_trans_flag
    End Get
    Set(ByVal Value As String)
      strclitrans_internal_trans_flag = Value
    End Set
  End Property


  Public Property clitrans_date_listed() As System.DateTime
    Get
      Return strclitrans_date_listed
    End Get
    Set(ByVal Value As System.DateTime)
      strclitrans_date_listed = Value
    End Set
  End Property


  Public Property clitrans_exclusive_flag() As String
    Get
      Return strclitrans_exclusive_flag
    End Get
    Set(ByVal Value As String)
      strclitrans_exclusive_flag = Value
    End Set
  End Property


  Public Property clitrans_asking_wordage() As String
    Get
      Return strclitrans_asking_wordage
    End Get
    Set(ByVal Value As String)
      strclitrans_asking_wordage = Value
    End Set
  End Property


  Public Property clitrans_asking_price() As Double
    Get
      Return dblclitrans_asking_price
    End Get
    Set(ByVal Value As Double)
      dblclitrans_asking_price = Value
    End Set
  End Property


  Public Property clitrans_ser_nbr() As String
    Get
      Return strclitrans_ser_nbr
    End Get
    Set(ByVal Value As String)
      strclitrans_ser_nbr = Value
    End Set
  End Property


  Public Property clitrans_reg_nbr() As String
    Get
      Return strclitrans_reg_nbr
    End Get
    Set(ByVal Value As String)
      strclitrans_reg_nbr = Value
    End Set
  End Property


  Public Property clitrans_country_of_registration() As String
    Get
      Return strclitrans_country_of_registration
    End Get
    Set(ByVal Value As String)
      strclitrans_country_of_registration = Value
    End Set
  End Property


  Public Property clitrans_airframe_total_hours() As Integer
    Get
      Return intclitrans_airframe_total_hours
    End Get
    Set(ByVal Value As Integer)
      intclitrans_airframe_total_hours = Value
    End Set
  End Property


  Public Property clitrans_airframe_total_landings() As Integer
    Get
      Return intclitrans_airframe_total_landings
    End Get
    Set(ByVal Value As Integer)
      intclitrans_airframe_total_landings = Value
    End Set
  End Property


  Public Property clitrans_aport_iata_code() As String
    Get
      Return strclitrans_aport_iata_code
    End Get
    Set(ByVal Value As String)
      strclitrans_aport_iata_code = Value
    End Set
  End Property


  Public Property clitrans_aport_icao_code() As String
    Get
      Return strclitrans_aport_icao_code
    End Get
    Set(ByVal Value As String)
      strclitrans_aport_icao_code = Value
    End Set
  End Property


  Public Property clitrans_aport_name() As String
    Get
      Return strclitrans_aport_name
    End Get
    Set(ByVal Value As String)
      strclitrans_aport_name = Value
    End Set
  End Property


  Public Property clitrans_aport_state() As String
    Get
      Return strclitrans_aport_state
    End Get
    Set(ByVal Value As String)
      strclitrans_aport_state = Value
    End Set
  End Property


  Public Property clitrans_aport_country() As String
    Get
      Return strclitrans_aport_country
    End Get
    Set(ByVal Value As String)
      strclitrans_aport_country = Value
    End Set
  End Property


  Public Property clitrans_aport_city() As String
    Get
      Return strclitrans_aport_city
    End Get
    Set(ByVal Value As String)
      strclitrans_aport_city = Value
    End Set
  End Property


  Public Property clitrans_aport_private() As String
    Get
      Return strclitrans_aport_private
    End Get
    Set(ByVal Value As String)
      strclitrans_aport_private = Value
    End Set
  End Property


  Public Property clitrans_action_date() As System.DateTime
    Get
      Return strclitrans_action_date
    End Get
    Set(ByVal Value As System.DateTime)
      strclitrans_action_date = Value
    End Set
  End Property


  Public Property clitrans_sold_price() As Double
    Get
      Return dblclitrans_sold_price
    End Get
    Set(ByVal Value As Double)
      dblclitrans_sold_price = Value
    End Set
  End Property


  Public Property clitrans_sold_price_type() As String
    Get
      Return strclitrans_sold_price_type
    End Get
    Set(ByVal Value As String)
      strclitrans_sold_price_type = Value
    End Set
  End Property


  Public Property clitrans_est_price() As Double
    Get
      Return dblclitrans_est_price
    End Get
    Set(ByVal Value As Double)
      dblclitrans_est_price = Value
    End Set
  End Property


  '**************************************************
  ' Constructors
  '**************************************************


  '   Default Constructor
  Public Sub New()
    clitrans_id = 0
    clitrans_jetnet_trans_id = 0
    clitrans_cliac_id = 0
    clitrans_jetnet_ac_id = 0
    clitrans_cliamod_id = 0
    clitrans_lifecycle = 0
    clitrans_ownership = ""
    clitrans_date = CDate("01/01/1900")
    strclitrans_type = ""
    clitrans_subject = ""
    clitrans_customer_note = ""
    clitrans_newac_flag = ""
    clitrans_internal_trans_flag = ""
    clitrans_date_listed = CDate("01/01/1900")
    clitrans_exclusive_flag = ""
    clitrans_asking_wordage = ""
    clitrans_asking_price = 0
    clitrans_ser_nbr = ""
    clitrans_reg_nbr = ""
    clitrans_country_of_registration = ""
    clitrans_airframe_total_hours = 0
    clitrans_airframe_total_landings = 0
    clitrans_aport_iata_code = ""
    clitrans_aport_icao_code = ""
    clitrans_aport_name = ""
    clitrans_aport_state = ""
    clitrans_aport_country = ""
    clitrans_aport_city = ""
    clitrans_aport_private = ""
    clitrans_action_date = CDate("01/01/1900")
    clitrans_sold_price = 0
    clitrans_sold_price_type = ""
    clitrans_est_price = 0
    clitrans_year_mfr = ""
    clitrans_deal_type = ""
    clitrans_value_description = ""


    clitrans_subcategory_code = ""
    clitrans_subcat_code_part1 = ""
    clitrans_subcat_code_part2 = ""
    clitrans_subcat_code_part3 = ""
    clitrans_retail_flag = "N"

  End Sub


  '   Parameter Based Constructor
  Public Sub New(ByVal aclitrans_retail_flag As String, ByVal aclitrans_subcat_code_part2 As String, ByVal aclitrans_subcat_code_part3 As String, ByVal aclitrans_subcat_code_part1 As String, ByVal aclitrans_id As Integer, ByVal aclitrans_value_description As String, ByVal aclitrans_jetnet_trans_id As Integer, ByVal aclitrans_cliac_id As Long, ByVal aclitrans_jetnet_ac_id As Long, ByVal aclitrans_cliamod_id As Integer, ByVal aclitrans_lifecycle As Integer, ByVal aclitrans_ownership As String, ByVal aclitrans_date As System.DateTime, ByVal aclitrans_subcategory_code As String, ByVal aclitrans_subject As String, ByVal aclitrans_customer_note As String, ByVal aclitrans_newac_flag As String, ByVal aclitrans_internal_trans_flag As String, ByVal aclitrans_date_listed As System.DateTime, ByVal aclitrans_exclusive_flag As String, ByVal aclitrans_asking_wordage As String, ByVal aclitrans_asking_price As Double, ByVal aclitrans_ser_nbr As String, ByVal aclitrans_reg_nbr As String, ByVal aclitrans_country_of_registration As String, ByVal aclitrans_airframe_total_hours As Integer, ByVal aclitrans_airframe_total_landings As Integer, ByVal aclitrans_aport_iata_code As String, ByVal aclitrans_aport_icao_code As String, ByVal aclitrans_aport_name As String, ByVal aclitrans_aport_state As String, ByVal aclitrans_aport_country As String, ByVal aclitrans_aport_city As String, ByVal aclitrans_aport_private As String, ByVal aclitrans_action_date As System.DateTime, ByVal aclitrans_sold_price As Double, ByVal aclitrans_sold_price_type As String, ByVal aclitrans_est_price As Double, ByVal aclitrans_type As String)

    clitrans_subcategory_code = aclitrans_subcategory_code
    clitrans_subcat_code_part1 = aclitrans_subcat_code_part1
    clitrans_subcat_code_part2 = aclitrans_subcat_code_part2
    clitrans_subcat_code_part3 = aclitrans_subcat_code_part3
    clitrans_retail_flag = aclitrans_retail_flag


    clitrans_id = aclitrans_id
    clitrans_value_description = aclitrans_value_description
    clitrans_jetnet_trans_id = aclitrans_jetnet_trans_id
    clitrans_cliac_id = aclitrans_cliac_id
    clitrans_jetnet_ac_id = aclitrans_jetnet_ac_id
    clitrans_cliamod_id = aclitrans_cliamod_id
    clitrans_lifecycle = aclitrans_lifecycle
    clitrans_ownership = aclitrans_ownership
    clitrans_date = aclitrans_date
    clitrans_subject = aclitrans_subject
    clitrans_customer_note = aclitrans_customer_note
    clitrans_newac_flag = aclitrans_newac_flag
    clitrans_internal_trans_flag = aclitrans_internal_trans_flag
    clitrans_date_listed = aclitrans_date_listed
    clitrans_exclusive_flag = aclitrans_exclusive_flag
    clitrans_asking_wordage = aclitrans_asking_wordage
    clitrans_asking_price = aclitrans_asking_price
    clitrans_ser_nbr = aclitrans_ser_nbr
    clitrans_reg_nbr = aclitrans_reg_nbr
    clitrans_country_of_registration = aclitrans_country_of_registration
    clitrans_airframe_total_hours = aclitrans_airframe_total_hours
    clitrans_airframe_total_landings = aclitrans_airframe_total_landings
    clitrans_aport_iata_code = aclitrans_aport_iata_code
    clitrans_aport_icao_code = aclitrans_aport_icao_code
    clitrans_aport_name = aclitrans_aport_name
    clitrans_aport_state = aclitrans_aport_state
    clitrans_aport_country = aclitrans_aport_country
    clitrans_aport_city = aclitrans_aport_city
    clitrans_aport_private = aclitrans_aport_private
    clitrans_action_date = aclitrans_action_date
    clitrans_sold_price = aclitrans_sold_price
    clitrans_sold_price_type = aclitrans_sold_price_type
    clitrans_est_price = aclitrans_est_price
    clitrans_type = aclitrans_type
  End Sub




  ' ***********************************************************************
  ' Methods
  ' ***********************************************************************


  ' Method name: ClassInfo
  ' Purpose: to generate a string with all assigned parameters
  ' Parameters: Client_Transactions
  ' Return: String with all assigned parameters
  ' Change Log
  '           06/29/2010 - Created By: Tom Jones
  Public Function ClassInfo(ByVal aClient_Transactions As clsClient_Transactions) As String
    Try
      Dim ClassInformation As String


      ClassInformation = " clitrans_id = " & aClient_Transactions.clitrans_id & vbNewLine & " clitrans_type = " & aClient_Transactions.clitrans_type & vbNewLine & " clitrans_jetnet_trans_id = " & aClient_Transactions.clitrans_jetnet_trans_id & vbNewLine & " clitrans_cliac_id = " & aClient_Transactions.clitrans_cliac_id & vbNewLine & " clitrans_cliamod_id = " & aClient_Transactions.clitrans_cliamod_id & vbNewLine & " clitrans_lifecycle = " & aClient_Transactions.clitrans_lifecycle & vbNewLine & " clitrans_ownership = " & aClient_Transactions.clitrans_ownership & vbNewLine & " clitrans_date = " & aClient_Transactions.clitrans_date & vbNewLine & " clitrans_subject = " & aClient_Transactions.clitrans_subject & vbNewLine & " clitrans_customer_note = " & aClient_Transactions.clitrans_customer_note & vbNewLine & " clitrans_newac_flag = " & aClient_Transactions.clitrans_newac_flag & vbNewLine & " clitrans_internal_trans_flag = " & aClient_Transactions.clitrans_internal_trans_flag & vbNewLine & " clitrans_date_listed = " & aClient_Transactions.clitrans_date_listed & vbNewLine & " clitrans_exclusive_flag = " & aClient_Transactions.clitrans_exclusive_flag & vbNewLine & " clitrans_asking_wordage = " & aClient_Transactions.clitrans_asking_wordage & vbNewLine & " clitrans_asking_price = " & aClient_Transactions.clitrans_asking_price & vbNewLine & " clitrans_ser_nbr = " & aClient_Transactions.clitrans_ser_nbr & vbNewLine & " clitrans_reg_nbr = " & aClient_Transactions.clitrans_reg_nbr & vbNewLine & " clitrans_country_of_registration = " & aClient_Transactions.clitrans_country_of_registration & vbNewLine & " clitrans_airframe_total_hours = " & aClient_Transactions.clitrans_airframe_total_hours & vbNewLine & " clitrans_airframe_total_landings = " & aClient_Transactions.clitrans_airframe_total_landings & vbNewLine & " clitrans_aport_iata_code = " & aClient_Transactions.clitrans_aport_iata_code & vbNewLine & " clitrans_aport_icao_code = " & aClient_Transactions.clitrans_aport_icao_code & vbNewLine & " clitrans_aport_name = " & aClient_Transactions.clitrans_aport_name & vbNewLine & " clitrans_aport_state = " & aClient_Transactions.clitrans_aport_state & vbNewLine & " clitrans_aport_country = " & aClient_Transactions.clitrans_aport_country & vbNewLine & " clitrans_aport_city = " & aClient_Transactions.clitrans_aport_city & vbNewLine & " clitrans_aport_private = " & aClient_Transactions.clitrans_aport_private & vbNewLine & " clitrans_action_date = " & aClient_Transactions.clitrans_action_date & vbNewLine & " clitrans_sold_price = " & aClient_Transactions.clitrans_sold_price & vbNewLine & " clitrans_sold_price_type = " & aClient_Transactions.clitrans_sold_price_type & vbNewLine & " clitrans_est_price = " & aClient_Transactions.clitrans_est_price & vbNewLine


      ' return the string
      Return ClassInformation
    Catch ex As Exception
      MsgBox("Error occured in classInfo. Class: clsClient_Transactions. Error:" & ex.Message)
      Return Nothing
    End Try
  End Function


End Class
