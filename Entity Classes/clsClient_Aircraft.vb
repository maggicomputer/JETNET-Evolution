Option Strict On
Option Explicit On


' ***********************************************************************
' Class Comments Section
' ***********************************************************************
' Class name: clsClient_Aircraft
' Purpose: Configure all properties and methods for clsClient_Aircraft
' Parameters: None
' Return: None
' Change Log
'           06/21/2010 - Created By: Tom Jones


Public Class clsClient_Aircraft



    '**************************************************
    ' Private variable declarations
    '**************************************************
    Private intcliaircraft_id As Integer
    Private intcliaircraft_cliamod_id As Integer
    Private strcliaircraft_ser_nbr As String
    Private strcliaircraft_reg_nbr As String
    Private strcliaircraft_year_mfr As String
    Private strcliaircraft_forsale_flag As String
    Private strcliaircraft_status As String
    Private strcliaircraft_exclusive_flag As String
    Private strcliaircraft_asking_wordage As String
    Private dblcliaircraft_asking_price As Double
    Private strcliaircraft_lease_flag As String
    Private strcliaircraft_delivery As String
    Private intcliaircraft_user_id As Integer
    Private datcliaircraft_action_date As Nullable(Of System.DateTime)
    Private intcliaircraft_jetnet_ac_id As Integer
    Private intcliaircraft_lifecycle As Integer
    Private strcliaircraft_ownership As String
    Private strcliaircraft_usage As String
    Private strcliaircraft_alt_ser_nbr As String
    Private strcliaircraft_prev_reg_nbr As String
    Private strcliaircraft_country_of_registration As String
    Private strcliaircraft_new_flag As String
    Private strcliaircraft_year_dlv As String
    Private datcliaircraft_date_purchased As Nullable(Of System.DateTime)
    Private datcliaircraft_date_listed As Nullable(Of System.DateTime)
    Private intcliaircraft_airframe_maintenance_program As Integer
    Private intcliaircraft_airframe_maintenance_tracking_program As Integer

    Private intcliaircraft_engine_maintenance_program As Integer
    Private intcliaircraft_engine_management_program As Integer

    Private intcliaircraft_airframe_total_hours As Nullable(of Integer)
    Private intcliaircraft_airframe_total_landings As Nullable(Of Integer)
    Private datcliaircraft_date_engine_times_as_of As Nullable(Of System.DateTime)
    Private strcliaircraft_aport_iata_code As String
    Private strcliaircraft_aport_icao_code As String
    Private strcliaircraft_aport_name As String
    Private strcliaircraft_aport_state As String
    Private strcliaircraft_aport_country As String
    Private strcliaircraft_aport_city As String
    Private strcliaircraft_aport_private As String
    Private strcliaircraft_apu_model_name As String
    Private strcliaircraft_apu_ser_nbr As String
    Private strcliaircraft_picture_exist_flag As String
    Private strcliaircraft_ac_maintained As String
    Private intcliaircraft_apu_ttsn_hours As Integer
    Private intcliaircraft_apu_tsoh_hours As Integer
    Private intcliaircraft_apu_tshi_hours As Integer
    Private strcliaircraft_apu_maintance_program As String
    Private strcliaircraft_damage_flag As String
    Private strcliaircraft_damage_history_notes As String
    Private dblcliaircraft_interior_rating As Double
    Private dblcliaircraft_est_price As Double
    Private dblcliaircraft_broker_price As Double
    Private strcliaircraft_interior_month_year As String
    Private strcliaircraft_interior_doneby_name As String
    Private strcliaircraft_interior_config_name As String
    Private dblcliaircraft_exterior_rating As Double
    Private strcliaircraft_exterior_month_year As String
    Private strcliaircraft_exterior_doneby_name As String
    Private intcliaircraft_passenger_count As Integer
    Private strcliaircraft_confidential_notes As String
    Private strcliaircraft_ser_nbr_sort As String
    Private strcliaircraft_value_description As String = ""
    Private strcliaircraft_custom_1 As String = ""
    Private strcliaircraft_custom_2 As String = ""
    Private strcliaircraft_custom_3 As String = ""
    Private strcliaircraft_custom_4 As String = ""
    Private strcliaircraft_custom_5 As String = ""
    Private strcliaircraft_custom_6 As String = ""
    Private strcliaircraft_custom_7 As String = ""
    Private strcliaircraft_custom_8 As String = ""
    Private strcliaircraft_custom_9 As String = ""
    Private strcliaircraft_custom_10 As String = ""

    '**************************************************
    ' Setters and getters
    '**************************************************


#Region "Strings"
    Public Property cliaircraft_ser_nbr_sort() As String
        Get
            Return strcliaircraft_ser_nbr_sort
        End Get
        Set(ByVal Value As String)
            strcliaircraft_ser_nbr_sort = Value
        End Set
    End Property
    Public Property cliaircraft_ser_nbr() As String
        Get
            Return strcliaircraft_ser_nbr
        End Get
        Set(ByVal Value As String)
            strcliaircraft_ser_nbr = Value
        End Set
    End Property
    Public Property cliaircraft_reg_nbr() As String
        Get
            Return strcliaircraft_reg_nbr
        End Get
        Set(ByVal Value As String)
            strcliaircraft_reg_nbr = Value
        End Set
    End Property
    Public Property cliaircraft_year_mfr() As String
        Get
            Return strcliaircraft_year_mfr
        End Get
        Set(ByVal Value As String)
            strcliaircraft_year_mfr = Value
        End Set
    End Property
    Public Property cliaircraft_forsale_flag() As String
        Get
            Return strcliaircraft_forsale_flag
        End Get
        Set(ByVal Value As String)
            strcliaircraft_forsale_flag = Value
        End Set
    End Property
    Public Property cliaircraft_status() As String
        Get
            Return strcliaircraft_status
        End Get
        Set(ByVal Value As String)
            strcliaircraft_status = Value
        End Set
    End Property
    Public Property cliaircraft_exclusive_flag() As String
        Get
            Return strcliaircraft_exclusive_flag
        End Get
        Set(ByVal Value As String)
            strcliaircraft_exclusive_flag = Value
        End Set
    End Property
    Public Property cliaircraft_asking_wordage() As String
        Get
            Return strcliaircraft_asking_wordage
        End Get
        Set(ByVal Value As String)
            strcliaircraft_asking_wordage = Value
        End Set
    End Property
    Public Property cliaircraft_ac_maintained() As String
        Get
            Return strcliaircraft_ac_maintained
        End Get
        Set(ByVal Value As String)
            strcliaircraft_ac_maintained = Value
        End Set
    End Property
    Public Property cliaircraft_lease_flag() As String
        Get
            Return strcliaircraft_lease_flag
        End Get
        Set(ByVal Value As String)
            strcliaircraft_lease_flag = Value
        End Set
    End Property
    Public Property cliaircraft_delivery() As String
        Get
            Return strcliaircraft_delivery
        End Get
        Set(ByVal Value As String)
            strcliaircraft_delivery = Value
        End Set
    End Property
    Public Property cliaircraft_ownership() As String
        Get
            Return strcliaircraft_ownership
        End Get
        Set(ByVal Value As String)
            strcliaircraft_ownership = Value
        End Set
    End Property
    Public Property cliaircraft_usage() As String
        Get
            Return strcliaircraft_usage
        End Get
        Set(ByVal Value As String)
            strcliaircraft_usage = Value
        End Set
    End Property
    Public Property cliaircraft_alt_ser_nbr() As String
        Get
            Return strcliaircraft_alt_ser_nbr
        End Get
        Set(ByVal Value As String)
            strcliaircraft_alt_ser_nbr = Value
        End Set
    End Property
    Public Property cliaircraft_prev_reg_nbr() As String
        Get
            Return strcliaircraft_prev_reg_nbr
        End Get
        Set(ByVal Value As String)
            strcliaircraft_prev_reg_nbr = Value
        End Set
    End Property
    Public Property cliaircraft_country_of_registration() As String
        Get
            Return strcliaircraft_country_of_registration
        End Get
        Set(ByVal Value As String)
            strcliaircraft_country_of_registration = Value
        End Set
    End Property
    Public Property cliaircraft_new_flag() As String
        Get
            Return strcliaircraft_new_flag
        End Get
        Set(ByVal Value As String)
            strcliaircraft_new_flag = Value
        End Set
    End Property
    Public Property cliaircraft_year_dlv() As String
        Get
            Return strcliaircraft_year_dlv
        End Get
        Set(ByVal Value As String)
            strcliaircraft_year_dlv = Value
        End Set
    End Property
    Public Property cliaircraft_aport_iata_code() As String
        Get
            Return strcliaircraft_aport_iata_code
        End Get
        Set(ByVal Value As String)
            strcliaircraft_aport_iata_code = Value
        End Set
    End Property
    Public Property cliaircraft_aport_icao_code() As String
        Get
            Return strcliaircraft_aport_icao_code
        End Get
        Set(ByVal Value As String)
            strcliaircraft_aport_icao_code = Value
        End Set
    End Property
    Public Property cliaircraft_aport_name() As String
        Get
            Return strcliaircraft_aport_name
        End Get
        Set(ByVal Value As String)
            strcliaircraft_aport_name = Value
        End Set
    End Property
    Public Property cliaircraft_aport_state() As String
        Get
            Return strcliaircraft_aport_state
        End Get
        Set(ByVal Value As String)
            strcliaircraft_aport_state = Value
        End Set
    End Property
    Public Property cliaircraft_aport_country() As String
        Get
            Return strcliaircraft_aport_country
        End Get
        Set(ByVal Value As String)
            strcliaircraft_aport_country = Value
        End Set
    End Property
    Public Property cliaircraft_aport_city() As String
        Get
            Return strcliaircraft_aport_city
        End Get
        Set(ByVal Value As String)
            strcliaircraft_aport_city = Value
        End Set
    End Property
    Public Property cliaircraft_aport_private() As String
        Get
            Return strcliaircraft_aport_private
        End Get
        Set(ByVal Value As String)
            strcliaircraft_aport_private = Value
        End Set
    End Property
    Public Property cliaircraft_apu_model_name() As String
        Get
            Return strcliaircraft_apu_model_name
        End Get
        Set(ByVal Value As String)
            strcliaircraft_apu_model_name = Value
        End Set
    End Property
    Public Property cliaircraft_apu_ser_nbr() As String
        Get
            Return strcliaircraft_apu_ser_nbr
        End Get
        Set(ByVal Value As String)
            strcliaircraft_apu_ser_nbr = Value
        End Set
    End Property
    Public Property cliaircraft_apu_maintance_program() As String
        Get
            Return strcliaircraft_apu_maintance_program
        End Get
        Set(ByVal Value As String)
            strcliaircraft_apu_maintance_program = Value
        End Set
    End Property


    Public Property cliaircraft_damage_flag() As String
        Get
            Return strcliaircraft_damage_flag
        End Get
        Set(ByVal Value As String)
            strcliaircraft_damage_flag = Value
        End Set
    End Property


    Public Property cliaircraft_damage_history_notes() As String
        Get
            Return strcliaircraft_damage_history_notes
        End Get
        Set(ByVal Value As String)
            strcliaircraft_damage_history_notes = Value
        End Set
    End Property

    Public Property cliaircraft_interior_month_year() As String
        Get
            Return strcliaircraft_interior_month_year
        End Get
        Set(ByVal Value As String)
            strcliaircraft_interior_month_year = Value
        End Set
    End Property


    Public Property cliaircraft_interior_doneby_name() As String
        Get
            Return strcliaircraft_interior_doneby_name
        End Get
        Set(ByVal Value As String)
            strcliaircraft_interior_doneby_name = Value
        End Set
    End Property


    Public Property cliaircraft_interior_config_name() As String
        Get
            Return strcliaircraft_interior_config_name
        End Get
        Set(ByVal Value As String)
            strcliaircraft_interior_config_name = Value
        End Set
    End Property
    Public Property cliaircraft_exterior_month_year() As String
        Get
            Return strcliaircraft_exterior_month_year
        End Get
        Set(ByVal Value As String)
            strcliaircraft_exterior_month_year = Value
        End Set
    End Property


    Public Property cliaircraft_exterior_doneby_name() As String
        Get
            Return strcliaircraft_exterior_doneby_name
        End Get
        Set(ByVal Value As String)
            strcliaircraft_exterior_doneby_name = Value
        End Set
    End Property

    Public Property cliaircraft_confidential_notes() As String
        Get
            Return strcliaircraft_confidential_notes
        End Get
        Set(ByVal Value As String)
            strcliaircraft_confidential_notes = Value
        End Set
    End Property

    Public Property cliaircraft_picture_exist_flag() As String
        Get
            Return strcliaircraft_picture_exist_flag
        End Get
        Set(ByVal value As String)
            strcliaircraft_picture_exist_flag = value
        End Set
    End Property

    Public Property cliaircraft_value_description() As String
        Get
            Return strcliaircraft_value_description
        End Get
        Set(ByVal value As String)
            strcliaircraft_value_description = value
        End Set
    End Property


    Public Property cliaircraft_custom_1() As String
        Get
            Return strcliaircraft_custom_1
        End Get
        Set(ByVal Value As String)
            strcliaircraft_custom_1 = Value
        End Set
    End Property
    Public Property cliaircraft_custom_2() As String
        Get
            Return strcliaircraft_custom_2
        End Get
        Set(ByVal Value As String)
            strcliaircraft_custom_2 = Value
        End Set
    End Property
    Public Property cliaircraft_custom_3() As String
        Get
            Return strcliaircraft_custom_3
        End Get
        Set(ByVal Value As String)
            strcliaircraft_custom_3 = Value
        End Set
    End Property
    Public Property cliaircraft_custom_4() As String
        Get
            Return strcliaircraft_custom_4
        End Get
        Set(ByVal Value As String)
            strcliaircraft_custom_4 = Value
        End Set
    End Property
    Public Property cliaircraft_custom_5() As String
        Get
            Return strcliaircraft_custom_5
        End Get
        Set(ByVal Value As String)
            strcliaircraft_custom_5 = Value
        End Set
    End Property
    Public Property cliaircraft_custom_6() As String
        Get
            Return strcliaircraft_custom_6
        End Get
        Set(ByVal Value As String)
            strcliaircraft_custom_6 = Value
        End Set
    End Property
    Public Property cliaircraft_custom_7() As String
        Get
            Return strcliaircraft_custom_7
        End Get
        Set(ByVal Value As String)
            strcliaircraft_custom_7 = Value
        End Set
    End Property
    Public Property cliaircraft_custom_8() As String
        Get
            Return strcliaircraft_custom_8
        End Get
        Set(ByVal Value As String)
            strcliaircraft_custom_8 = Value
        End Set
    End Property
    Public Property cliaircraft_custom_9() As String
        Get
            Return strcliaircraft_custom_9
        End Get
        Set(ByVal Value As String)
            strcliaircraft_custom_9 = Value
        End Set
    End Property
    Public Property cliaircraft_custom_10() As String
        Get
            Return strcliaircraft_custom_10
        End Get
        Set(ByVal Value As String)
            strcliaircraft_custom_10 = Value
        End Set
    End Property
#End Region
#Region "Integers"
    Public Property cliaircraft_id() As Integer
        Get
            Return intcliaircraft_id
        End Get
        Set(ByVal Value As Integer)
            intcliaircraft_id = Value
        End Set
    End Property
    Public Property cliaircraft_cliamod_id() As Integer
        Get
            Return intcliaircraft_cliamod_id
        End Get
        Set(ByVal Value As Integer)
            intcliaircraft_cliamod_id = Value
        End Set
    End Property
    Public Property cliaircraft_jetnet_ac_id() As Integer
        Get
            Return intcliaircraft_jetnet_ac_id
        End Get
        Set(ByVal Value As Integer)
            intcliaircraft_jetnet_ac_id = Value
        End Set
    End Property
    Public Property cliaircraft_lifecycle() As Integer
        Get
            Return intcliaircraft_lifecycle
        End Get
        Set(ByVal Value As Integer)
            intcliaircraft_lifecycle = Value
        End Set
    End Property
    Public Property cliaircraft_airframe_maintenance_program() As Integer
        Get
            Return intcliaircraft_airframe_maintenance_program
        End Get
        Set(ByVal Value As Integer)
            intcliaircraft_airframe_maintenance_program = Value
        End Set
    End Property
    Public Property cliaircraft_airframe_maintenance_tracking_program() As Integer
        Get
            Return intcliaircraft_airframe_maintenance_tracking_program
        End Get
        Set(ByVal Value As Integer)
            intcliaircraft_airframe_maintenance_tracking_program = Value
        End Set
    End Property

    Public Property cliaircraft_engine_maintenance_program() As Integer
        Get
            Return intcliaircraft_engine_maintenance_program
        End Get
        Set(ByVal Value As Integer)
            intcliaircraft_engine_maintenance_program = Value
        End Set
    End Property
    Public Property cliaircraft_engine_management_program() As Integer
        Get
            Return intcliaircraft_engine_management_program
        End Get
        Set(ByVal Value As Integer)
            intcliaircraft_engine_management_program = Value
        End Set
    End Property


    Public Property cliaircraft_apu_ttsn_hours() As Integer
        Get
            Return intcliaircraft_apu_ttsn_hours
        End Get
        Set(ByVal Value As Integer)
            intcliaircraft_apu_ttsn_hours = Value
        End Set
    End Property
    Public Property cliaircraft_apu_tsoh_hours() As Integer
        Get
            Return intcliaircraft_apu_tsoh_hours
        End Get
        Set(ByVal Value As Integer)
            intcliaircraft_apu_tsoh_hours = Value
        End Set
    End Property
    Public Property cliaircraft_apu_tshi_hours() As Integer
        Get
            Return intcliaircraft_apu_tshi_hours
        End Get
        Set(ByVal Value As Integer)
            intcliaircraft_apu_tshi_hours = Value
        End Set
    End Property
    Public Property cliaircraft_passenger_count() As Integer
        Get
            Return intcliaircraft_passenger_count
        End Get
        Set(ByVal Value As Integer)
            intcliaircraft_passenger_count = Value
        End Set
    End Property
#End Region

#Region "Double"
    Public Property cliaircraft_asking_price() As Double
        Get
            Return dblcliaircraft_asking_price
        End Get
        Set(ByVal Value As Double)
            dblcliaircraft_asking_price = Value
        End Set
    End Property
    Public Property cliaircraft_est_price() As Double
        Get
            Return dblcliaircraft_est_price
        End Get
        Set(ByVal Value As Double)
            dblcliaircraft_est_price = Value
        End Set
    End Property
    Public Property cliaircraft_broker_price() As Double
        Get
            Return dblcliaircraft_broker_price
        End Get
        Set(ByVal Value As Double)
            dblcliaircraft_broker_price = Value
        End Set
    End Property
    Public Property cliaircraft_user_id() As Integer
        Get
            Return intcliaircraft_user_id
        End Get
        Set(ByVal Value As Integer)
            intcliaircraft_user_id = Value
        End Set
    End Property
#End Region

#Region "Nullable/DateTime"
    Public Property cliaircraft_action_date() As Nullable(Of System.DateTime)
        Get
            Return datcliaircraft_action_date
        End Get
        Set(ByVal Value As Nullable(Of System.DateTime))
            datcliaircraft_action_date = Value
        End Set
    End Property
    Public Property cliaircraft_date_purchased() As Nullable(Of System.DateTime)
        Get
            Return datcliaircraft_date_purchased
        End Get
        Set(ByVal Value As Nullable(Of System.DateTime))
            datcliaircraft_date_purchased = Value
        End Set
    End Property
    Public Property cliaircraft_date_listed() As Nullable(Of System.DateTime)
        Get
            Return datcliaircraft_date_listed
        End Get
        Set(ByVal Value As Nullable(Of System.DateTime))
            datcliaircraft_date_listed = Value
        End Set
    End Property
    Public Property cliaircraft_date_engine_times_as_of() As Nullable(Of System.DateTime)
        Get
            Return datcliaircraft_date_engine_times_as_of
        End Get
        Set(ByVal Value As Nullable(Of System.DateTime))
            datcliaircraft_date_engine_times_as_of = Value
        End Set
    End Property
#End Region

#Region "Nullable/Integer"
    Public Property cliaircraft_airframe_total_hours() As Nullable(Of Integer)
        Get
            Return intcliaircraft_airframe_total_hours
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            intcliaircraft_airframe_total_hours = Value
        End Set
    End Property
    Public Property cliaircraft_airframe_total_landings() As Nullable(Of Integer)
        Get
            Return intcliaircraft_airframe_total_landings
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            intcliaircraft_airframe_total_landings = Value
        End Set
    End Property
#End Region

#Region "Double"

    Public Property cliaircraft_interior_rating() As Double
        Get
            Return dblcliaircraft_interior_rating
        End Get
        Set(ByVal Value As Double)
            dblcliaircraft_interior_rating = Value
        End Set
    End Property

    Public Property cliaircraft_exterior_rating() As Double
        Get
            Return dblcliaircraft_exterior_rating
        End Get
        Set(ByVal Value As Double)
            dblcliaircraft_exterior_rating = Value
        End Set
    End Property
#End Region

 



  

 



    '**************************************************
    ' Constructors
    '**************************************************


    '   Default Constructor
    Public Sub New()
        cliaircraft_id = 0
        cliaircraft_picture_exist_flag = "N"
        cliaircraft_cliamod_id = 0
        cliaircraft_ser_nbr = ""
        cliaircraft_reg_nbr = ""
        cliaircraft_year_mfr = ""
        cliaircraft_forsale_flag = ""
        cliaircraft_status = ""
        cliaircraft_exclusive_flag = ""
        cliaircraft_asking_wordage = ""
        cliaircraft_asking_price = 0.0
        cliaircraft_lease_flag = ""
        cliaircraft_delivery = ""
        cliaircraft_user_id = 0
        cliaircraft_action_date = CDate("1/1/1900")
        cliaircraft_jetnet_ac_id = 0
        cliaircraft_lifecycle = 0
        cliaircraft_ownership = ""
        cliaircraft_usage = ""
        cliaircraft_alt_ser_nbr = ""
        cliaircraft_prev_reg_nbr = ""
        cliaircraft_country_of_registration = ""
        cliaircraft_new_flag = ""
        cliaircraft_year_dlv = ""
        cliaircraft_ac_maintained = ""
        'cliaircraft_date_purchased = CDate("1/1/1900")
        'cliaircraft_date_listed = CDate("1/1/1900")
        cliaircraft_airframe_maintenance_program = 0
        cliaircraft_airframe_maintenance_tracking_program = 0
        cliaircraft_engine_maintenance_program = 0
        cliaircraft_engine_management_program = 0
        'cliaircraft_airframe_total_hours = 0
        'cliaircraft_airframe_total_landings = 0
        'cliaircraft_date_engine_times_as_of = CDate("1/1/1900")
        cliaircraft_aport_iata_code = ""
        cliaircraft_aport_icao_code = ""
        cliaircraft_aport_name = ""
        cliaircraft_aport_state = ""
        cliaircraft_aport_country = ""
        cliaircraft_aport_city = ""
        cliaircraft_aport_private = ""
        cliaircraft_apu_model_name = ""
        cliaircraft_apu_ser_nbr = ""
        cliaircraft_apu_ttsn_hours = 0
        cliaircraft_apu_tsoh_hours = 0
        cliaircraft_apu_tshi_hours = 0
        cliaircraft_apu_maintance_program = ""
        cliaircraft_damage_flag = ""
        cliaircraft_damage_history_notes = ""
        cliaircraft_interior_rating = 0.0
        cliaircraft_interior_month_year = ""
        cliaircraft_interior_doneby_name = ""
        cliaircraft_interior_config_name = ""
        cliaircraft_exterior_rating = 0.0
        cliaircraft_exterior_month_year = ""
        cliaircraft_exterior_doneby_name = ""
        cliaircraft_passenger_count = 0
        cliaircraft_confidential_notes = ""
        cliaircraft_est_price = 0.0
        cliaircraft_broker_price = 0.0
        cliaircraft_ser_nbr_sort = ""
        cliaircraft_value_description = ""

        cliaircraft_custom_1 = ""
        cliaircraft_custom_2 = ""
        cliaircraft_custom_3 = ""
        cliaircraft_custom_4 = ""
        cliaircraft_custom_5 = ""
        cliaircraft_custom_6 = ""
        cliaircraft_custom_7 = ""
        cliaircraft_custom_8 = ""
        cliaircraft_custom_9 = ""
        cliaircraft_custom_10 = ""
    End Sub


    '   Parameter Based Constructor
    Public Sub New(ByVal acliaircraft_id As Integer, ByVal acliaircraft_ac_maintained As String, ByVal acliaircraft_cliamod_id As Integer, ByVal acliaircraft_ser_nbr As String, ByVal acliaircraft_reg_nbr As String, ByVal acliaircraft_year_mfr As String, ByVal acliaircraft_forsale_flag As String, ByVal acliaircraft_status As String, ByVal acliaircraft_exclusive_flag As String, ByVal acliaircraft_asking_wordage As String, ByVal acliaircraft_asking_price As Double, ByVal acliaircraft_lease_flag As String, ByVal acliaircraft_delivery As String, ByVal acliaircraft_user_id As Integer, ByVal acliaircraft_action_date As System.DateTime, ByVal acliaircraft_jetnet_ac_id As Integer, ByVal acliaircraft_lifecycle As Integer, ByVal acliaircraft_ownership As String, ByVal acliaircraft_usage As String, ByVal acliaircraft_alt_ser_nbr As String, ByVal acliaircraft_prev_reg_nbr As String, ByVal acliaircraft_country_of_registration As String, ByVal acliaircraft_new_flag As String, ByVal acliaircraft_year_dlv As String, ByVal acliaircraft_date_purchased As System.DateTime, ByVal acliaircraft_date_listed As System.DateTime, ByVal acliaircraft_airframe_maintenance_program As Integer, ByVal acliaircraft_airframe_maintenance_tracking_program As Integer, ByVal acliaircraft_airframe_total_hours As Integer, ByVal acliaircraft_airframe_total_landings As Integer, ByVal acliaircraft_date_engine_times_as_of As System.DateTime, ByVal acliaircraft_aport_iata_code As String, ByVal acliaircraft_aport_icao_code As String, ByVal acliaircraft_aport_name As String, ByVal acliaircraft_aport_state As String, ByVal acliaircraft_aport_country As String, ByVal acliaircraft_aport_city As String, ByVal acliaircraft_aport_private As String, ByVal acliaircraft_apu_model_name As String, ByVal acliaircraft_apu_ser_nbr As String, ByVal acliaircraft_apu_ttsn_hours As Integer, ByVal acliaircraft_apu_tsoh_hours As Integer, ByVal acliaircraft_apu_tshi_hours As Integer, ByVal acliaircraft_apu_maintance_program As String, ByVal acliaircraft_damage_flag As String, ByVal acliaircraft_damage_history_notes As String, ByVal acliaircraft_interior_rating As Double, ByVal acliaircraft_interior_month_year As String, ByVal acliaircraft_interior_doneby_name As String, ByVal acliaircraft_interior_config_name As String, ByVal acliaircraft_exterior_rating As Double, ByVal acliaircraft_exterior_month_year As String, ByVal acliaircraft_exterior_doneby_name As String, ByVal acliaircraft_passenger_count As Integer, ByVal acliaircraft_confidential_notes As String, ByVal acliaircraft_est_price As Double, ByVal acliaircraft_broker_price As Double, ByVal acliaircraft_ser_nbr_sort As String, ByVal acliaircraft_value_description As String, ByVal acliaircraft_custom_1 As String, ByVal acliaircraft_custom_2 As String, ByVal acliaircraft_custom_3 As String, ByVal acliaircraft_custom_4 As String, ByVal acliaircraft_custom_5 As String, ByVal acliaircraft_custom_6 As String, ByVal acliaircraft_custom_7 As String, ByVal acliaircraft_custom_8 As String, ByVal acliaircraft_custom_9 As String, ByVal acliaircraft_custom_10 As String, ByVal acliaircraft_engine_maintenance_program As Integer, ByVal acliaircraft_engine_management_program As Integer)
        cliaircraft_id = acliaircraft_id
        cliaircraft_cliamod_id = acliaircraft_cliamod_id
        cliaircraft_ser_nbr = acliaircraft_ser_nbr
        cliaircraft_reg_nbr = acliaircraft_reg_nbr
        cliaircraft_year_mfr = acliaircraft_year_mfr
        cliaircraft_forsale_flag = acliaircraft_forsale_flag
        cliaircraft_status = acliaircraft_status
        cliaircraft_exclusive_flag = acliaircraft_exclusive_flag
        cliaircraft_asking_wordage = acliaircraft_asking_wordage
        cliaircraft_asking_price = acliaircraft_asking_price
        cliaircraft_lease_flag = acliaircraft_lease_flag
        cliaircraft_delivery = acliaircraft_delivery
        cliaircraft_user_id = acliaircraft_user_id
        cliaircraft_action_date = acliaircraft_action_date
        cliaircraft_jetnet_ac_id = acliaircraft_jetnet_ac_id
        cliaircraft_lifecycle = acliaircraft_lifecycle
        cliaircraft_ownership = acliaircraft_ownership
        cliaircraft_usage = acliaircraft_usage
        cliaircraft_ac_maintained = acliaircraft_ac_maintained
        cliaircraft_alt_ser_nbr = acliaircraft_alt_ser_nbr
        cliaircraft_prev_reg_nbr = acliaircraft_prev_reg_nbr
        cliaircraft_country_of_registration = acliaircraft_country_of_registration
        cliaircraft_new_flag = acliaircraft_new_flag
        cliaircraft_year_dlv = acliaircraft_year_dlv
        cliaircraft_date_purchased = acliaircraft_date_purchased
        cliaircraft_date_listed = acliaircraft_date_listed
        cliaircraft_airframe_maintenance_program = acliaircraft_airframe_maintenance_program
        cliaircraft_airframe_maintenance_tracking_program = acliaircraft_airframe_maintenance_tracking_program


        cliaircraft_engine_maintenance_program = acliaircraft_engine_maintenance_program
        cliaircraft_engine_management_program = acliaircraft_engine_management_program

        cliaircraft_airframe_total_hours = acliaircraft_airframe_total_hours
        cliaircraft_airframe_total_landings = acliaircraft_airframe_total_landings
        cliaircraft_date_engine_times_as_of = acliaircraft_date_engine_times_as_of
        cliaircraft_aport_iata_code = acliaircraft_aport_iata_code
        cliaircraft_aport_icao_code = acliaircraft_aport_icao_code
        cliaircraft_aport_name = acliaircraft_aport_name
        cliaircraft_aport_state = acliaircraft_aport_state
        cliaircraft_aport_country = acliaircraft_aport_country
        cliaircraft_aport_city = acliaircraft_aport_city
        cliaircraft_aport_private = acliaircraft_aport_private
        cliaircraft_apu_model_name = acliaircraft_apu_model_name
        cliaircraft_apu_ser_nbr = acliaircraft_apu_ser_nbr
        cliaircraft_apu_ttsn_hours = acliaircraft_apu_ttsn_hours
        cliaircraft_apu_tsoh_hours = acliaircraft_apu_tsoh_hours
        cliaircraft_apu_tshi_hours = acliaircraft_apu_tshi_hours
        cliaircraft_apu_maintance_program = acliaircraft_apu_maintance_program
        cliaircraft_damage_flag = acliaircraft_damage_flag
        cliaircraft_damage_history_notes = acliaircraft_damage_history_notes
        cliaircraft_interior_rating = acliaircraft_interior_rating
        cliaircraft_interior_month_year = acliaircraft_interior_month_year
        cliaircraft_interior_doneby_name = acliaircraft_interior_doneby_name
        cliaircraft_interior_config_name = acliaircraft_interior_config_name
        cliaircraft_exterior_rating = acliaircraft_exterior_rating
        cliaircraft_exterior_month_year = acliaircraft_exterior_month_year
        cliaircraft_exterior_doneby_name = acliaircraft_exterior_doneby_name
        cliaircraft_passenger_count = acliaircraft_passenger_count
        cliaircraft_confidential_notes = acliaircraft_confidential_notes
        cliaircraft_est_price = acliaircraft_est_price
        cliaircraft_broker_price = acliaircraft_broker_price
        cliaircraft_ser_nbr_sort = acliaircraft_ser_nbr_sort
        cliaircraft_value_description = acliaircraft_value_description



        cliaircraft_custom_1 = acliaircraft_custom_1
        cliaircraft_custom_2 = acliaircraft_custom_2
        cliaircraft_custom_3 = acliaircraft_custom_3
        cliaircraft_custom_4 = acliaircraft_custom_4
        cliaircraft_custom_5 = acliaircraft_custom_5
        cliaircraft_custom_6 = acliaircraft_custom_6
        cliaircraft_custom_7 = acliaircraft_custom_7
        cliaircraft_custom_8 = acliaircraft_custom_8
        cliaircraft_custom_9 = acliaircraft_custom_9
        cliaircraft_custom_10 = acliaircraft_custom_10
    End Sub




    ' ***********************************************************************
    ' Methods
    ' ***********************************************************************


    ' Method name: ClassInfo
    ' Purpose: to generate a string with all assigned parameters
    ' Parameters: Client_Aircraft
    ' Return: String with all assigned parameters
    ' Change Log
    '           06/21/2010 - Created By: Tom Jones
    Public Function ClassInfo(ByVal aClient_Aircraft As clsClient_Aircraft) As String
        Try
            Dim ClassInformation As String


            ClassInformation = " cliaircraft_broker_price = " & aClient_Aircraft.cliaircraft_broker_price & vbNewLine & " cliaircraft_id = " & aClient_Aircraft.cliaircraft_id & vbNewLine & " cliaircraft_cliamod_id = " & aClient_Aircraft.cliaircraft_cliamod_id & vbNewLine & " cliaircraft_ser_nbr = " & aClient_Aircraft.cliaircraft_ser_nbr & vbNewLine & " cliaircraft_reg_nbr = " & aClient_Aircraft.cliaircraft_reg_nbr & vbNewLine & " cliaircraft_year_mfr = " & aClient_Aircraft.cliaircraft_year_mfr & vbNewLine & " cliaircraft_forsale_flag = " & aClient_Aircraft.cliaircraft_forsale_flag & vbNewLine & " cliaircraft_status = " & aClient_Aircraft.cliaircraft_status & vbNewLine & " cliaircraft_exclusive_flag = " & aClient_Aircraft.cliaircraft_exclusive_flag & vbNewLine & " cliaircraft_asking_wordage = " & aClient_Aircraft.cliaircraft_asking_wordage & vbNewLine & " cliaircraft_asking_price = " & aClient_Aircraft.cliaircraft_asking_price & vbNewLine & " cliaircraft_lease_flag = " & aClient_Aircraft.cliaircraft_lease_flag & vbNewLine & " cliaircraft_delivery = " & aClient_Aircraft.cliaircraft_delivery & vbNewLine & " cliaircraft_user_id = " & aClient_Aircraft.cliaircraft_user_id & vbNewLine & " cliaircraft_action_date = " & aClient_Aircraft.cliaircraft_action_date & vbNewLine & " cliaircraft_jetnet_ac_id = " & aClient_Aircraft.cliaircraft_jetnet_ac_id & vbNewLine & " cliaircraft_lifecycle = " & aClient_Aircraft.cliaircraft_lifecycle & vbNewLine & " cliaircraft_ownership = " & aClient_Aircraft.cliaircraft_ownership & vbNewLine & " cliaircraft_usage = " & aClient_Aircraft.cliaircraft_usage & vbNewLine & " cliaircraft_alt_ser_nbr = " & aClient_Aircraft.cliaircraft_alt_ser_nbr & vbNewLine & " cliaircraft_prev_reg_nbr = " & aClient_Aircraft.cliaircraft_prev_reg_nbr & vbNewLine & " cliaircraft_country_of_registration = " & aClient_Aircraft.cliaircraft_country_of_registration & vbNewLine & " cliaircraft_new_flag = " & aClient_Aircraft.cliaircraft_new_flag & vbNewLine & " cliaircraft_year_dlv = " & aClient_Aircraft.cliaircraft_year_dlv & vbNewLine & " cliaircraft_date_purchased = " & aClient_Aircraft.cliaircraft_date_purchased & vbNewLine & " cliaircraft_date_listed = " & aClient_Aircraft.cliaircraft_date_listed & vbNewLine & " cliaircraft_airframe_maintenance_program = " & aClient_Aircraft.cliaircraft_airframe_maintenance_program & vbNewLine & " cliaircraft_airframe_maintenance_tracking_program = " & aClient_Aircraft.cliaircraft_airframe_maintenance_tracking_program & vbNewLine & " cliaircraft_airframe_total_hours = " & aClient_Aircraft.cliaircraft_airframe_total_hours & vbNewLine & " cliaircraft_airframe_total_landings = " & aClient_Aircraft.cliaircraft_airframe_total_landings & vbNewLine & " cliaircraft_date_engine_times_as_of = " & aClient_Aircraft.cliaircraft_date_engine_times_as_of & vbNewLine & " cliaircraft_aport_iata_code = " & aClient_Aircraft.cliaircraft_aport_iata_code & vbNewLine & " cliaircraft_aport_icao_code = " & aClient_Aircraft.cliaircraft_aport_icao_code & vbNewLine & " cliaircraft_aport_name = " & aClient_Aircraft.cliaircraft_aport_name & vbNewLine & " cliaircraft_aport_state = " & aClient_Aircraft.cliaircraft_aport_state & vbNewLine & " cliaircraft_aport_country = " & aClient_Aircraft.cliaircraft_aport_country & vbNewLine & " cliaircraft_aport_city = " & aClient_Aircraft.cliaircraft_aport_city & vbNewLine & " cliaircraft_aport_private = " & aClient_Aircraft.cliaircraft_aport_private & vbNewLine & " cliaircraft_apu_model_name = " & aClient_Aircraft.cliaircraft_apu_model_name & vbNewLine & " cliaircraft_apu_ser_nbr = " & aClient_Aircraft.cliaircraft_apu_ser_nbr & vbNewLine & " cliaircraft_apu_ttsn_hours = " & aClient_Aircraft.cliaircraft_apu_ttsn_hours & vbNewLine & " cliaircraft_apu_tsoh_hours = " & aClient_Aircraft.cliaircraft_apu_tsoh_hours & vbNewLine & " cliaircraft_apu_tshi_hours = " & aClient_Aircraft.cliaircraft_apu_tshi_hours & vbNewLine & " cliaircraft_apu_maintance_program = " & aClient_Aircraft.cliaircraft_apu_maintance_program & vbNewLine & " cliaircraft_damage_flag = " & aClient_Aircraft.cliaircraft_damage_flag & vbNewLine & " cliaircraft_damage_history_notes = " & aClient_Aircraft.cliaircraft_damage_history_notes & vbNewLine & " cliaircraft_interior_rating = " & aClient_Aircraft.cliaircraft_interior_rating & vbNewLine & " cliaircraft_interior_month_year = " & aClient_Aircraft.cliaircraft_interior_month_year & vbNewLine & " cliaircraft_interior_doneby_name = " & aClient_Aircraft.cliaircraft_interior_doneby_name & vbNewLine & " cliaircraft_interior_config_name = " & aClient_Aircraft.cliaircraft_interior_config_name & vbNewLine & " cliaircraft_exterior_rating = " & aClient_Aircraft.cliaircraft_exterior_rating & vbNewLine & " cliaircraft_exterior_month_year = " & aClient_Aircraft.cliaircraft_exterior_month_year & vbNewLine & " cliaircraft_exterior_doneby_name = " & aClient_Aircraft.cliaircraft_exterior_doneby_name & vbNewLine & " cliaircraft_passenger_count = " & aClient_Aircraft.cliaircraft_passenger_count & vbNewLine & " cliaircraft_confidential_notes = " & aClient_Aircraft.cliaircraft_confidential_notes & vbNewLine & " cliaircraft_value_description = " & aClient_Aircraft.cliaircraft_value_description & vbNewLine
            ClassInformation += " cliaircraft_custom_1 = " & aClient_Aircraft.cliaircraft_custom_1 & vbNewLine
            ClassInformation += " cliaircraft_custom_2 = " & aClient_Aircraft.cliaircraft_custom_2 & vbNewLine
            ClassInformation += " cliaircraft_custom_3 = " & aClient_Aircraft.cliaircraft_custom_3 & vbNewLine
            ClassInformation += " cliaircraft_custom_4 = " & aClient_Aircraft.cliaircraft_custom_4 & vbNewLine
            ClassInformation += " cliaircraft_custom_5 = " & aClient_Aircraft.cliaircraft_custom_5 & vbNewLine
            ClassInformation += " cliaircraft_custom_6 = " & aClient_Aircraft.cliaircraft_custom_6 & vbNewLine
            ClassInformation += " cliaircraft_custom_7 = " & aClient_Aircraft.cliaircraft_custom_7 & vbNewLine
            ClassInformation += " cliaircraft_custom_8 = " & aClient_Aircraft.cliaircraft_custom_8 & vbNewLine
            ClassInformation += " cliaircraft_custom_9 = " & aClient_Aircraft.cliaircraft_custom_9 & vbNewLine
            ClassInformation += " cliaircraft_custom_10 = " & aClient_Aircraft.cliaircraft_custom_10 & vbNewLine

            ' return the string
            Return ClassInformation
        Catch ex As Exception
            MsgBox("Error occured in classInfo. Class: clsClient_Aircraft. Error:" & ex.Message)
            Return Nothing
        End Try
    End Function


End Class
