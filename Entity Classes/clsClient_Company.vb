Option Strict On
Option Explicit On


' ***********************************************************************
' Class Comments Section
' ***********************************************************************
' Class name: clsClient_Company
' Purpose: Configure all properties and methods for clsClient_Company
' Parameters: None
' Return: None
' Change Log
'           1/21/2010 - Created By: Tom Jones


Public Class clsClient_Company



    '**************************************************
    ' Private variable declarations
    '**************************************************
    Private intclicomp_id As Integer
    Private strclicomp_name As String
    Private strclicomp_search_name As String
    Private strclicomp_alternate_name_type As String
    Private strclicomp_alternate_name As String
    Private strclicomp_address1 As String
    Private strclicomp_address2 As String
    Private strclicomp_city As String
    Private strclicomp_state As String
    Private strclicomp_zip_code As String
    Private strclicomp_country As String
    Private strclicomp_agency_type As String
    Private strclicomp_web_address As String
    Private strclicomp_email_address As String
    Private strclicomp_date_updated As String
    Private intclicomp_jetnet_comp_id As Integer
    Private strclicomp_status As String
    Private strclicomp_description As String
    Private strclicomp_category1 As String
    Private strclicomp_category2 As String
    Private strclicomp_category3 As String
    Private strclicomp_category4 As String
    Private strclicomp_category5 As String
    Private strclicomp_business_type_name As String
    Private intclicomp_user_id As Integer
    Private intclicomp_mainloc_comp_id As Nullable(Of Integer)

    '**************************************************
    ' Setters and getters
    '**************************************************
    Public Property clicomp_id() As Integer
        Get
            Return intclicomp_id
        End Get
        Set(ByVal Value As Integer)
            intclicomp_id = Value
        End Set
    End Property

    Public Property clicomp_search_name() As String
        Get
            Return strclicomp_search_name
        End Get
        Set(ByVal Value As String)
            strclicomp_search_name = Value
        End Set
    End Property

    Public Property clicomp_name() As String
        Get
            Return strclicomp_name
        End Get
        Set(ByVal Value As String)
            strclicomp_name = Value
        End Set
    End Property

    Public Property clicomp_business_type_name() As String
        Get
            Return strclicomp_business_type_name
        End Get
        Set(ByVal Value As String)
            strclicomp_business_type_name = Value
        End Set
    End Property


    Public Property clicomp_alternate_name_type() As String
        Get
            Return strclicomp_alternate_name_type
        End Get
        Set(ByVal Value As String)
            strclicomp_alternate_name_type = Value
        End Set
    End Property


    Public Property clicomp_alternate_name() As String
        Get
            Return strclicomp_alternate_name
        End Get
        Set(ByVal Value As String)
            strclicomp_alternate_name = Value
        End Set
    End Property


    Public Property clicomp_address1() As String
        Get
            Return strclicomp_address1
        End Get
        Set(ByVal Value As String)
            strclicomp_address1 = Value
        End Set
    End Property


    Public Property clicomp_address2() As String
        Get
            Return strclicomp_address2
        End Get
        Set(ByVal Value As String)
            strclicomp_address2 = Value
        End Set
    End Property


    Public Property clicomp_city() As String
        Get
            Return strclicomp_city
        End Get
        Set(ByVal Value As String)
            strclicomp_city = Value
        End Set
    End Property


    Public Property clicomp_state() As String
        Get
            Return strclicomp_state
        End Get
        Set(ByVal Value As String)
            strclicomp_state = Value
        End Set
    End Property


    Public Property clicomp_zip_code() As String
        Get
            Return strclicomp_zip_code
        End Get
        Set(ByVal Value As String)
            strclicomp_zip_code = Value
        End Set
    End Property


    Public Property clicomp_country() As String
        Get
            Return strclicomp_country
        End Get
        Set(ByVal Value As String)
            strclicomp_country = Value
        End Set
    End Property


    Public Property clicomp_agency_type() As String
        Get
            Return strclicomp_agency_type
        End Get
        Set(ByVal Value As String)
            strclicomp_agency_type = Value
        End Set
    End Property


    Public Property clicomp_web_address() As String
        Get
            Return strclicomp_web_address
        End Get
        Set(ByVal Value As String)
            strclicomp_web_address = Value
        End Set
    End Property


    Public Property clicomp_email_address() As String
        Get
            Return strclicomp_email_address
        End Get
        Set(ByVal Value As String)
            strclicomp_email_address = Value
        End Set
    End Property


    Public Property clicomp_date_updated() As String
        Get
            Return strclicomp_date_updated
        End Get
        Set(ByVal Value As String)
            strclicomp_date_updated = Value
        End Set
    End Property


    Public Property clicomp_jetnet_comp_id() As Integer
        Get
            Return intclicomp_jetnet_comp_id
        End Get
        Set(ByVal Value As Integer)
            intclicomp_jetnet_comp_id = Value
        End Set
    End Property


    Public Property clicomp_status() As String
        Get
            Return strclicomp_status
        End Get
        Set(ByVal Value As String)
            strclicomp_status = Value
        End Set
    End Property
    Public Property clicomp_description() As String
        Get
            Return strclicomp_description
        End Get
        Set(ByVal Value As String)
            strclicomp_description = Value
        End Set
    End Property
    Public Property clicomp_category1() As String
        Get
            Return strclicomp_category1
        End Get
        Set(ByVal Value As String)
            strclicomp_category1 = Value
        End Set
    End Property
    Public Property clicomp_category2() As String
        Get
            Return strclicomp_category2
        End Get
        Set(ByVal Value As String)
            strclicomp_category2 = Value
        End Set
    End Property
    Public Property clicomp_category3() As String
        Get
            Return strclicomp_category3
        End Get
        Set(ByVal Value As String)
            strclicomp_category3 = Value
        End Set
    End Property
    Public Property clicomp_category4() As String
        Get
            Return strclicomp_category4
        End Get
        Set(ByVal Value As String)
            strclicomp_category4 = Value
        End Set
    End Property
    Public Property clicomp_category5() As String
        Get
            Return strclicomp_category5
        End Get
        Set(ByVal Value As String)
            strclicomp_category5 = Value
        End Set
    End Property

    Public Property clicomp_user_id() As Integer
        Get
            Return intclicomp_user_id
        End Get
        Set(ByVal Value As Integer)
            intclicomp_user_id = Value
        End Set
    End Property
    Public Property clicomp_mainloc_comp_id() As Nullable(Of Integer)
        Get
            Return intclicomp_mainloc_comp_id
        End Get
        Set(ByVal Value As Nullable(Of Integer))
            intclicomp_mainloc_comp_id = Value
        End Set
    End Property



    '**************************************************
    ' Constructors
    '**************************************************


    '   Default Constructor
    Public Sub New()
        clicomp_id = 0
        clicomp_name = ""
        clicomp_alternate_name_type = ""
        clicomp_alternate_name = ""
        clicomp_address1 = ""
        clicomp_address2 = ""
        clicomp_city = ""
        clicomp_state = ""
        clicomp_zip_code = ""
        clicomp_country = ""
        clicomp_agency_type = ""
        clicomp_web_address = ""
        clicomp_email_address = ""
        clicomp_date_updated = ""
        clicomp_jetnet_comp_id = 0
        clicomp_status = "Y"
        clicomp_description = ""
        clicomp_category1 = ""
        clicomp_category2 = ""
        clicomp_category3 = ""
        clicomp_category4 = ""
        clicomp_category5 = ""
        clicomp_business_type_name = ""
        clicomp_user_id = 0
    End Sub


    '   Parameter Based Constructor
    Public Sub New(ByVal aclicomp_id As Integer, ByVal aclicomp_mainloc_comp_id As Integer, ByVal aclicomp_name As String, ByVal aclicomp_alternate_name_type As String, ByVal aclicomp_alternate_name As String, ByVal aclicomp_address1 As String, ByVal aclicomp_address2 As String, ByVal aclicomp_city As String, ByVal aclicomp_state As String, ByVal aclicomp_zip_code As String, ByVal aclicomp_country As String, ByVal aclicomp_agency_type As String, ByVal aclicomp_web_address As String, ByVal aclicomp_email_address As String, ByVal aclicomp_date_updated As String, ByVal aclicomp_jetnet_comp_id As Integer, ByVal aclicomp_status As String, ByVal aclicomp_user_id As Integer, ByVal aclicomp_description As String, ByVal aclicomp_category1 As String, ByVal aclicomp_category2 As String, ByVal aclicomp_category3 As String, ByVal aclicomp_category4 As String, ByVal aclicomp_category5 As String, ByVal aclicomp_business_type_name As String)
        clicomp_id = aclicomp_id
        clicomp_name = aclicomp_name
        clicomp_alternate_name_type = aclicomp_alternate_name_type
        clicomp_alternate_name = aclicomp_alternate_name
        clicomp_address1 = aclicomp_address1
        clicomp_address2 = aclicomp_address2
        clicomp_city = aclicomp_city
        clicomp_state = aclicomp_state
        clicomp_zip_code = aclicomp_zip_code
        clicomp_country = aclicomp_country
        clicomp_agency_type = aclicomp_agency_type
        clicomp_web_address = aclicomp_web_address
        clicomp_email_address = aclicomp_email_address
        clicomp_date_updated = aclicomp_date_updated
        clicomp_jetnet_comp_id = aclicomp_jetnet_comp_id
        clicomp_status = aclicomp_status
        clicomp_user_id = aclicomp_user_id
        clicomp_description = aclicomp_description
        clicomp_category1 = aclicomp_category1
        clicomp_category1 = aclicomp_category2
        clicomp_category3 = aclicomp_category3
        clicomp_category4 = aclicomp_category4
        clicomp_category5 = aclicomp_category5
        clicomp_mainloc_comp_id = aclicomp_mainloc_comp_id
        clicomp_business_type_name = aclicomp_business_type_name
    End Sub




    ' ***********************************************************************
    ' Methods
    ' ***********************************************************************


    ' Method name: ClassInfo
    ' Purpose: to generate a string with all assigned parameters
    ' Parameters: Client_Company
    ' Return: String with all assigned parameters
    ' Change Log
    '           1/21/2010 - Created By: Tom Jones
    Public Function ClassInfo(ByVal aClient_Company As clsClient_Company) As String
        Try
            Dim ClassInformation As String


            ClassInformation = " clicomp_id = " & aClient_Company.clicomp_id & " clicomp_name = " & aClient_Company.clicomp_name & " clicomp_alternate_name_type = " & aClient_Company.clicomp_alternate_name_type & " clicomp_alternate_name = " & aClient_Company.clicomp_alternate_name & " clicomp_address1 = " & aClient_Company.clicomp_address1 & " clicomp_address2 = " & aClient_Company.clicomp_address2 & " clicomp_city = " & aClient_Company.clicomp_city & " clicomp_state = " & aClient_Company.clicomp_state & " clicomp_zip_code = " & aClient_Company.clicomp_zip_code & " clicomp_country = " & aClient_Company.clicomp_country & " clicomp_agency_type = " & aClient_Company.clicomp_agency_type & " clicomp_web_address = " & aClient_Company.clicomp_web_address & " clicomp_email_address = " & aClient_Company.clicomp_email_address & " clicomp_date_updated = " & aClient_Company.clicomp_date_updated & " clicomp_jetnet_comp_id = " & aClient_Company.clicomp_jetnet_comp_id & vbNewLine & " clicomp_status " & aClient_Company.clicomp_status & " clicomp_user_id " & aClient_Company.clicomp_user_id


            ' return the string
            Return ClassInformation
        Catch ex As Exception
            'MsgBox("Error occured in classInfo. Class: clsClient_Company. Error:" & ex.Message)
            Return Nothing
        End Try
    End Function


End Class
