Imports Microsoft.VisualBasic
Imports System.ComponentModel

<System.Serializable(), FlagsAttribute()> Public Enum eObjStatusCode As Integer

  NULL = 0
  SUCCESS = 1
  FAILURE = 2

End Enum

<System.Serializable(), FlagsAttribute()> Public Enum eObjDetailErrorCode As Integer

  NULL = 0
  SQL = 1
  WEB = 2
  SERVICE = 4
  FUNC_INPUT = 8
  NO_RECORDS = 16
  LOGON_BADUSERNAME = 32
  LOGON_BADPASSWORD = 64
  LOGON_BADSUBSCRIPTION = 128
  LOGON_BADSUBDATERANGE = 256
  DENY_ACCESS = 512
  GRANT_ACCESS = 1024
  NOT_IMPLEMENTED = 2048
  RUNTIME_ERROR = 4096
  FUNCTION_EXCEPTION = 8192

End Enum

<System.Serializable(), FlagsAttribute()> Public Enum eLogonTypes As Integer

  NULL = 0
  COOKIE = 1
  REGISTRY = 2
  DATABASE = 4
  TEXTFILE = 8

End Enum

<System.Serializable(), FlagsAttribute()> Public Enum eUserTypes As Integer

  NULL = 0
  USER = 1
  ADMINISTRATOR = 2
  GUEST = 4 'demo account
  RESEARCH = 5 'Research/Entry only
  MARKETING = 6
  MyNotesOnly = 7

End Enum

<System.Serializable(), FlagsAttribute()> Public Enum eDatabaseTypes As Integer

  NULL = 0
  LOCAL = 1
  LIVE = 2
  WEEKLY = 4
  BIWEEKLY = 8
  MONTHLY = 16
  TEST = 32
  STAR = 64

End Enum

<System.Serializable(), FlagsAttribute()> Public Enum eDatalayerTypes As Integer

  NULL = 0
  ACCESS = 1
  MYSQL = 2
  MSSQL = 4
  ORACLE = 8

End Enum

<System.Serializable(), FlagsAttribute()> Public Enum eWebSiteTypes As Integer

  NULL = 0
  LIVE = 1
  TEST = 2
  BETA = 4
  LOCAL = 8

End Enum


<System.Serializable(), FlagsAttribute()> Public Enum eWebHostTypes As Integer

  NULL = 0
  EVOLUTION = 1
  CRM = 2
  YACHT = 4
  ADMIN = 8
  ABI = 16
  HOMEBASE = 32

End Enum

<System.Serializable(), FlagsAttribute()> Public Enum eAbiWebHostTypes As Integer

  NULL = 0
  BUSINESSINDEX_COM = 1
  BUSINESSINDEX_NET = 2
  DEALERINDEX_COM = 4
  HELICOPTERINDEX_COM = 8

End Enum

<System.Serializable(), FlagsAttribute()> Public Enum eProductCodeTypes As Integer

  NULL = 0
  B = 1 ' Business     ** check tier level for make/model selection
  H = 2 ' Helicopters  ** ignore tier level
  C = 4 ' Commercial   ** check tier level for make/model selection
  R = 8 ' Regional     ** ignore tier level
  F = 16 ' Fortune 1000 ** not used
  A = 32 ' Aviation Business Index     ** ignore tier level
  P = 64 ' AirBP     ** ignore tier level
  S = 128 ' STAR Reports     ** ignore tier level
  I = 256 ' SPI View     ** ignore tier level
  Y = 512 ' Yacht     ** ignore tier level

End Enum

<System.Serializable(), FlagsAttribute()> Public Enum eTierLevelTypes As Integer

  NULL = 0
  JETS = 1
  TURBOS = 2
  ALL = 4

End Enum

<System.Serializable(), FlagsAttribute()> Public Enum eAirframeTypes As Integer

  NULL = 0
  FIXEDWING = 1
  ROTARY = 2

End Enum

<System.Serializable(), FlagsAttribute()> Public Enum eMakeTypes As Integer

  NULL = 0
  EXECUTIVE = 1
  JET = 2
  TURBO = 4
  PISTON = 8
  TURBINE = 16

End Enum

<System.Serializable(), FlagsAttribute()> Public Enum eYachtHullTypes As Integer

  NULL = 0
  MOTOR = 1
  SAIL = 2

End Enum

<System.Serializable(), FlagsAttribute()> Public Enum eYachtCategoryTypes As Integer

  NULL = 0
  GIGA = 1
  MEGA = 2
  SUPER = 4
  LUXURY = 8

End Enum

<System.Serializable(), FlagsAttribute()> Public Enum eSMSActivateTypes As Integer

  NULL = 0
  YES = 1
  NO = 2
  PENDING = 4
  WAIT = 8
  TEST = 16

End Enum

<System.Serializable(), FlagsAttribute()> Public Enum eListingView As Integer
  ' needs to have NULL = 0
  LISTING = 0
  GALLERY = 1
End Enum

<System.Serializable(), FlagsAttribute()> Public Enum eSubscriptionShareType As Integer
  NULL = 0
  MY_SUBSCRIPTION = 1
  MY_PARENT_SUBSCRIPTION = 2
  MY_PARENT_COMPANY = 3
End Enum

<System.Serializable(), FlagsAttribute()> Public Enum eNotesACSearchTypes As Integer

  NULL = 0
  SERIAL_OR_REGNO = 1
  SERIAL_ONLY = 2
  REGNO_ONLY = 4
  AIRCRAFT_ID = 8

End Enum

<System.Serializable(), FlagsAttribute()> Public Enum eNotesYTSearchTypes As Integer

  NULL = 0
  NAME_OR_CALLSIGN = 1
  CALLSIGN_ONLY = 2
  NAME_ONLY = 4
  YACHT_ID = 8

End Enum

<System.Serializable(), FlagsAttribute()> Public Enum eNotesACSearchOperator As Integer

  NULL = 0
  BEGINS = 1
  ANYWHERE = 2
  EQUALS = 4

End Enum

<System.Serializable()> Public Class crmWebHostClass

  Private _crmWebHostStatusCode As eObjStatusCode
  Private _crmWebHostDetailError As eObjDetailErrorCode

  Private _crmWebHostType As eWebHostTypes
  Private _WebSiteType As eWebSiteTypes

  Private _crmWebInstanceID As Integer
  Private _crmWebDatalayerType As eDatalayerTypes

  ' keep for crm? 
  Private _crmActiveDatabaseConn As String
  Private _crmHistoryDatabaseConn As String
  Private _crmClientDatabaseConn As String

  ' should only need one admin connection per serverinstance
  Private _AdminDatabaseConn As String
  Private _CloudDatabaseConn As String

  Private _ClientFullHostName As String

  Private _DebugFlag As Boolean

  Private _SessionRefreshMin As Integer
  Private _SessionMaxRefreshMin As Integer

  Private _SessionMaxRefreshCount As Integer

  Private _AutoLogonCookie As String

  Private _crmClientHostName As String
  Private _crmClientHostPath As String

  ' never used
  Private _crmClientStandAloneMode As Boolean

  Sub New()

    _crmWebHostStatusCode = eObjStatusCode.NULL
    _crmWebHostDetailError = eObjDetailErrorCode.NULL
    _crmWebHostType = eWebHostTypes.NULL
    _WebSiteType = eWebSiteTypes.NULL

    _crmWebInstanceID = 0
    _crmWebDatalayerType = eDatalayerTypes.NULL

    _crmActiveDatabaseConn = ""
    _crmHistoryDatabaseConn = ""
    _crmClientDatabaseConn = ""

    _crmClientHostName = ""
    _crmClientHostPath = ""

    _crmClientStandAloneMode = False

    _AdminDatabaseConn = ""
    _CloudDatabaseConn = ""

    _ClientFullHostName = ""

    _DebugFlag = False

    _SessionRefreshMin = 10
    _SessionMaxRefreshMin = 120
    _SessionMaxRefreshCount = ((_SessionMaxRefreshMin / _SessionRefreshMin) - 1)

    _AutoLogonCookie = ""

  End Sub

  Public Shared Function generateMYSQLConnectionString(ByVal hostName As String, ByVal dataBase As String, ByVal userID As String, ByVal passWD As String) As String

    Return "Connect Timeout=90;Allow User Variables=True;Default Command Timeout=3600;Persist Security Info=True;server=" + hostName.Trim + ";User Id=" + userID.Trim + ";password=" + passWD.Trim + ";database=" + dataBase.Trim

  End Function

  Public Shared Function generateMSSQLConnectionString(ByVal hostName As String, ByVal dataBase As String, ByVal userID As String, ByVal passWD As String) As String

    Return "server=" + hostName.Trim + ";initial catalog=" + dataBase.Trim + ";Persist Security Info=False;User Id=" + userID.Trim + ";Password=" + passWD.Trim + ";"

  End Function

  Public Shared Function generateACCESSConnectionString(ByVal hostName As String, ByVal dataBase As String, ByVal userID As String, ByVal passWD As String) As String

    Return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Chr(34).ToString + hostName.Trim + dataBase.Trim + Chr(34).ToString

  End Function

  Public Function webSiteHostName(ByVal siteType As eWebHostTypes) As String

    Dim webSiteTypes As Type = GetType(eWebHostTypes)

    Return [Enum].GetName(webSiteTypes, siteType)

  End Function

  Public Function webSiteTypeName(ByVal hostType As eWebSiteTypes) As String

    Dim WebHostTypes As Type = GetType(eWebSiteTypes)

    Return [Enum].GetName(WebHostTypes, hostType)

  End Function

  Public Function dataLayerTypeName(ByVal dataLayerType As eDatalayerTypes) As String

    Dim dataLayerTypes As Type = GetType(eDatalayerTypes)

    Return [Enum].GetName(dataLayerTypes, dataLayerType)

  End Function

  Public Property crmWebHostStatustCode() As eObjStatusCode
    Get
      Return _crmWebHostStatusCode
    End Get
    Set(ByVal value As eObjStatusCode)
      _crmWebHostStatusCode = value
    End Set
  End Property

  Public Property crmWebHostDetailError() As eObjDetailErrorCode
    Get
      Return _crmWebHostDetailError
    End Get
    Set(ByVal value As eObjDetailErrorCode)
      _crmWebHostDetailError = value
    End Set
  End Property

  Public Property crmWebHostType() As eWebHostTypes
    Get
      Return _crmWebHostType
    End Get
    Set(ByVal value As eWebHostTypes)
      _crmWebHostType = value
    End Set
  End Property

  Public Property WebSiteType() As eWebSiteTypes
    Get
      Return _WebSiteType
    End Get
    Set(ByVal value As eWebSiteTypes)
      _WebSiteType = value
    End Set
  End Property

  Public Property crmActiveDatabaseConn() As String
    Get
      Return _crmActiveDatabaseConn
    End Get
    Set(ByVal value As String)
      _crmActiveDatabaseConn = value
    End Set
  End Property

  Public Property crmHistoryDatabaseConn() As String
    Get
      Return _crmHistoryDatabaseConn
    End Get
    Set(ByVal value As String)
      _crmHistoryDatabaseConn = value
    End Set
  End Property

  Public Property crmClientDatabaseConn() As String
    Get
      Return _crmClientDatabaseConn
    End Get
    Set(ByVal value As String)
      _crmClientDatabaseConn = value
    End Set
  End Property

  Public Property crmClientHostName() As String
    Get
      Return _crmClientHostName
    End Get
    Set(ByVal value As String)
      _crmClientHostName = value
    End Set
  End Property

  Public Property crmClientHostPath() As String
    Get
      Return _crmClientHostPath
    End Get
    Set(ByVal value As String)
      _crmClientHostPath = value
    End Set
  End Property ' 

  Public Property crmClientStandAloneMode() As Boolean
    Get
      Return _crmClientStandAloneMode
    End Get
    Set(ByVal value As Boolean)
      _crmClientStandAloneMode = value
    End Set
  End Property

  Public Property crmWebDatalayerType() As eDatalayerTypes
    Get
      Return _crmWebDatalayerType
    End Get
    Set(ByVal value As eDatalayerTypes)
      _crmWebDatalayerType = value
    End Set
  End Property

  Public Property ClientFullHostName() As String
    Get
      Return _ClientFullHostName
    End Get
    Set(ByVal value As String)
      _ClientFullHostName = value
    End Set
  End Property

  Public Property AdminDatabaseConn() As String
    Get
      Return _AdminDatabaseConn
    End Get
    Set(ByVal value As String)
      _AdminDatabaseConn = value
    End Set
  End Property

  Public Property CloudDatabaseConn() As String
    Get
      Return _CloudDatabaseConn
    End Get
    Set(ByVal value As String)
      _CloudDatabaseConn = value
    End Set
  End Property

  Public Property crmWebInstanceID() As Integer
    Get
      Return _crmWebInstanceID
    End Get
    Set(ByVal value As Integer)
      _crmWebInstanceID = value
    End Set
  End Property

  Public ReadOnly Property SessionRefreshMin() As Integer
    Get
      Return _SessionRefreshMin
    End Get
  End Property

  Public ReadOnly Property SessionMaxRefreshMin() As Integer
    Get
      Return _SessionMaxRefreshMin
    End Get
  End Property

  Public ReadOnly Property sessionMaxRefreshCount() As Integer
    Get
      Return _SessionMaxRefreshCount
    End Get
  End Property

  Public Property DebugFlag() As Boolean
    Get
      Return _DebugFlag
    End Get
    Set(ByVal value As Boolean)
      _DebugFlag = value
    End Set
  End Property

  Public Property AutoLogonCookie() As String
    Get
      Return _AutoLogonCookie
    End Get
    Set(ByVal value As String)
      _AutoLogonCookie = value
    End Set
  End Property

End Class

<System.Serializable()> Public Class crmSubscriptionClass
  Private _crmSubinst_last_login_date As Nullable(Of System.DateTime)
  Private _crmSubinst_last_logout_date As Nullable(Of System.DateTime)
  Private _crmSubinst_last_session_date As Nullable(Of System.DateTime)
  Private _crmSubinst_FAA_data_date As Nullable(Of System.DateTime)

  Private _crmSubStatusCode As eObjStatusCode
  Private _crmSubDetailError As eObjDetailErrorCode
  Private _crmDatalayerType As eDatalayerTypes
  Private _crmDatabaseType As eDatabaseTypes
  Private _crmLogonType As eLogonTypes

  Private _crmSubscriptionID As Integer
  Private _crmMaxUserCount As Integer
  Private _crmAerodexFlag As Boolean
  Private _crmTierlevel As String
  Private _crmProductCode As String
  Private _crmFrequency As String
  Private _crmDocumentsFlag As Boolean

  Private _crmBusiness_Flag As Boolean
  Private _crmHelicopter_Flag As Boolean
  Private _crmCommercial_Flag As Boolean
  Private _crmYacht_Flag As Boolean

  Private _crmJets_Flag As Boolean
  Private _crmExecutive_Flag As Boolean
  Private _crmTurboprops As Boolean

  Private _crmStar_Reports_Flag As Boolean
  Private _crmSalesPriceIndex_Flag As Boolean
  Private _crmAppraiser_Flag As Boolean

  Private _crmServerSideNotes_Flag As Boolean
  Private _crmCloudNotes_Flag As Boolean
  Private _crmRegID As Long = 0
  Private _crmServerSideDBName As String = ""
  Private _crmSubscriptionShareType As eSubscriptionShareType
  Private _crmCloudNotesDBName As String = ""
  Private _crmMarketingFlag As Boolean

  Private _crmServiceCode As String
  Private _crmServiceName As String

  Sub New()

    _crmSubStatusCode = eObjStatusCode.NULL
    _crmSubDetailError = eObjDetailErrorCode.NULL
    _crmDatabaseType = eDatabaseTypes.NULL
    _crmDatalayerType = eDatalayerTypes.NULL
    _crmLogonType = eLogonTypes.NULL
    _crmMaxUserCount = 0
    _crmAerodexFlag = False
    _crmTierlevel = ""
    _crmProductCode = ""
    _crmFrequency = ""
    _crmDocumentsFlag = False


    _crmBusiness_Flag = False
    _crmHelicopter_Flag = False
    _crmCommercial_Flag = False
    _crmYacht_Flag = False

    _crmJets_Flag = False
    _crmExecutive_Flag = False
    _crmTurboprops = False

    _crmStar_Reports_Flag = False
    _crmSalesPriceIndex_Flag = False
    _crmAppraiser_Flag = False
    _crmServerSideNotes_Flag = False
    _crmServerSideDBName = ""
    _crmSubscriptionShareType = eSubscriptionShareType.MY_SUBSCRIPTION
    _crmRegID = 0
    'extended class based on my preferences page.
    _crmMarketingFlag = False
    _crmServiceCode = ""
    _crmServiceName = ""


  End Sub

  Public Function DisplaySubscription() As String
    DisplaySubscription = "Session.Item(""localSubscription"").crmSubinst_last_login_date As Nullable(Of System.DateTime): " & _crmSubinst_last_login_date & "<br />"
    DisplaySubscription += "Session.Item(""localSubscription"").crmSubinst_last_logout_date As Nullable(Of System.DateTime): " & _crmSubinst_last_logout_date & "<br />"
    DisplaySubscription += "Session.Item(""localSubscription"").crmSubinst_last_session_date As Nullable(Of System.DateTime): " & _crmSubinst_last_session_date & "<br />"
    DisplaySubscription += "Session.Item(""localSubscription"").crmSubinst_FAA_data_date As Nullable(Of System.DateTime): " & _crmSubinst_FAA_data_date & "<br />"


    DisplaySubscription += "Session.Item(""localSubscription"").crmSubStatusCode As eObjStatusCode: " & _crmSubStatusCode & "<br />"
    DisplaySubscription += "Session.Item(""localSubscription"").crmSubDetailError As eObjDetailErrorCode: " & _crmSubDetailError & "<br />"
    DisplaySubscription += "Session.Item(""localSubscription"").crmDatalayerType As eDatalayerTypes: " & _crmDatalayerType & "<br />"
    DisplaySubscription += "Session.Item(""localSubscription"").crmDatabaseType As eDatabaseTypes: " & _crmDatabaseType & "<br />"
    DisplaySubscription += "Session.Item(""localSubscription"").crmLogonType As eLogonTypes: " & _crmLogonType & "<br />"

    DisplaySubscription += "Session.Item(""localSubscription"").crmSubscriptionID As Integer: " & _crmSubscriptionID & "<br />"
    DisplaySubscription += "Session.Item(""localSubscription"").crmMaxUserCount As Integer: " & _crmMaxUserCount & "<br />"
    DisplaySubscription += "Session.Item(""localSubscription"").crmAerodexFlag As Boolean: " & _crmAerodexFlag & "<br />"
    DisplaySubscription += "Session.Item(""localSubscription"").crmTierlevel As String: " & _crmTierlevel & "<br />"
    DisplaySubscription += "Session.Item(""localSubscription"").crmProductCode As String: " & _crmProductCode & "<br />"
    DisplaySubscription += "Session.Item(""localSubscription"").crmFrequency As String: " & _crmFrequency & "<br />"
    DisplaySubscription += "Session.Item(""localSubscription"").crmDocumentsFlag As Boolean: " & _crmDocumentsFlag & "<br />"

    DisplaySubscription += "Session.Item(""localSubscription"").crmBusiness_Flag As Boolean: " & _crmBusiness_Flag & "<br />"
    DisplaySubscription += "Session.Item(""localSubscription"").crmHelicopter_Flag As Boolean: " & _crmHelicopter_Flag & "<br />"
    DisplaySubscription += "Session.Item(""localSubscription"").crmCommercial_Flag As Boolean: " & _crmCommercial_Flag & "<br />"
    DisplaySubscription += "Session.Item(""localSubscription"").crmYacht_Flag As Boolean: " & _crmYacht_Flag & "<br />"

    DisplaySubscription += "Session.Item(""localSubscription"").crmJets_Flag As Boolean: " & _crmJets_Flag & "<br />"
    DisplaySubscription += "Session.Item(""localSubscription"").crmExecutive_Flag As Boolean: " & _crmExecutive_Flag & "<br />"
    DisplaySubscription += "Session.Item(""localSubscription"").crmTurboprops As Boolean: " & _crmTurboprops & "<br />"

    DisplaySubscription += "Session.Item(""localSubscription"").crmStar_Reports_Flag As Boolean: " & _crmStar_Reports_Flag & "<br />"
    DisplaySubscription += "Session.Item(""localSubscription"").crmSalesPriceIndex_Flag As Boolean: " & _crmSalesPriceIndex_Flag & "<br />"
    DisplaySubscription += "Session.Item(""localSubscription"").crmAppraiser_Flag As Boolean: " & _crmAppraiser_Flag & "<br />"
    DisplaySubscription += "<br />Standard Cloud Notes<br />"
    DisplaySubscription += "Session.Item(""localSubscription"").crmCloudNotes_Flag As Boolean: " & _crmCloudNotes_Flag & "<br />"
    DisplaySubscription += "Session.Item(""localSubscription"").crmCloudNotesDBName As String: " & _crmCloudNotesDBName & "<br />"

    DisplaySubscription += "<br />Cloud Notes Plus<br />"
    DisplaySubscription += "Session.Item(""localSubscription"").crmServerSideNotes_Flag As Boolean: " & _crmServerSideNotes_Flag & "<br />"
    DisplaySubscription += "Session.Item(""localSubscription"").crmServerSideDBName As String: " & _crmServerSideDBName & "<br />"

    DisplaySubscription += "<br />"
    DisplaySubscription += "Session.Item(""localSubscription"").crmRegID As String: " & _crmRegID & "<br />"
    'extended class based on my preferences page.
    DisplaySubscription += "Session.Item(""localSubscription"").crmServiceCode As String : " & _crmServiceCode & "<br />"
    DisplaySubscription += "Session.Item(""localSubscription"").crmServiceName As String : " & _crmServiceName & "<br />"

    DisplaySubscription += "Session.Item(""localSubscription"").crmMarketingFlag as Boolean : " & _crmMarketingFlag & "<br />"
    DisplaySubscription += "Session.Item(""localSubscription"").crmSubscriptionShareType as Boolean : " & _crmSubscriptionShareType & "<br />"

  End Function


  Public Function dataBaseTypeName(ByVal dbType As eDatabaseTypes) As String

    Dim tmpStr As String = ""
    Dim dbTypes As Type = GetType(eDatabaseTypes)

    Return [Enum].GetName(dbTypes, dbType)

  End Function

  Public Function dataLayerTypeName(ByVal dataLayerType As eDatalayerTypes) As String

    Dim tmpStr As String = ""
    Dim dataLayerTypes As Type = GetType(eDatalayerTypes)

    Return [Enum].GetName(dataLayerTypes, dataLayerType)

  End Function

  Public Property crmSubinst_last_login_date() As Nullable(Of System.DateTime)
    Get
      Return _crmSubinst_last_login_date
    End Get
    Set(ByVal value As Nullable(Of System.DateTime))
      _crmSubinst_last_login_date = value
    End Set
  End Property

  Public Property crmSubinst_FAA_data_date() As Nullable(Of System.DateTime)
    Get
      Return _crmSubinst_FAA_data_date
    End Get
    Set(ByVal value As Nullable(Of System.DateTime))
      _crmSubinst_FAA_data_date = value
    End Set
  End Property


  Public Property crmSubinst_last_logout_date() As Nullable(Of System.DateTime)
    Get
      Return _crmSubinst_last_logout_date
    End Get
    Set(ByVal value As Nullable(Of System.DateTime))
      _crmSubinst_last_logout_date = value
    End Set
  End Property

  Public Property crmSubinst_last_session_date() As Nullable(Of System.DateTime)
    Get
      Return _crmSubinst_last_session_date
    End Get
    Set(ByVal value As Nullable(Of System.DateTime))
      _crmSubinst_last_session_date = value
    End Set
  End Property

  Public Property crmSubStatusCode() As eObjStatusCode
    Get
      Return _crmSubStatusCode
    End Get
    Set(ByVal value As eObjStatusCode)
      _crmSubStatusCode = value
    End Set
  End Property

  Public Property crmSubDetailError() As eObjDetailErrorCode
    Get
      Return _crmSubDetailError
    End Get
    Set(ByVal value As eObjDetailErrorCode)
      _crmSubDetailError = value
    End Set
  End Property

  Public Property crmLogonType() As eLogonTypes
    Get
      Return _crmLogonType
    End Get
    Set(ByVal value As eLogonTypes)
      _crmLogonType = value
    End Set
  End Property

  Public Property crmDatabaseType() As eDatabaseTypes
    Get
      Return _crmDatabaseType
    End Get
    Set(ByVal value As eDatabaseTypes)
      _crmDatabaseType = value
    End Set
  End Property

  Public Property crmDataLayerType() As eDatalayerTypes
    Get
      Return _crmDatalayerType
    End Get
    Set(ByVal value As eDatalayerTypes)
      _crmDatalayerType = value
    End Set
  End Property

  Public Property crmSubscriptionID() As Integer
    Get
      Return _crmSubscriptionID
    End Get
    Set(ByVal value As Integer)
      _crmSubscriptionID = value
    End Set
  End Property

  Public Property crmMaxUserCount() As Integer
    Get
      Return _crmMaxUserCount
    End Get
    Set(ByVal value As Integer)
      _crmMaxUserCount = value
    End Set
  End Property

  Public Property crmAerodexFlag() As Boolean
    Get
      Return _crmAerodexFlag
    End Get
    Set(ByVal value As Boolean)
      _crmAerodexFlag = value
    End Set
  End Property

  Public Property crmMarketingFlag() As Boolean
    Get
      Return _crmMarketingFlag
    End Get
    Set(ByVal value As Boolean)
      _crmMarketingFlag = value
    End Set
  End Property


  Public Property crmTierlevel() As String
    Get
      Return _crmTierlevel
    End Get
    Set(ByVal value As String)
      _crmTierlevel = value
    End Set
  End Property

  Public Property crmFrequency() As String
    Get
      Return _crmFrequency
    End Get
    Set(ByVal value As String)
      _crmFrequency = value
    End Set
  End Property

  Public Property crmProductCode() As String
    Get
      Return _crmProductCode
    End Get
    Set(ByVal value As String)
      _crmProductCode = value
    End Set
  End Property



  Public Property crmDocumentsFlag() As Boolean
    Get
      Return _crmDocumentsFlag
    End Get
    Set(ByVal value As Boolean)
      _crmDocumentsFlag = value
    End Set
  End Property


  Public Property crmBusiness_Flag() As Boolean
    Get
      Return _crmBusiness_Flag
    End Get
    Set(ByVal value As Boolean)
      _crmBusiness_Flag = value
    End Set
  End Property

  Public Property crmHelicopter_Flag() As Boolean
    Get
      Return _crmHelicopter_Flag
    End Get
    Set(ByVal value As Boolean)
      _crmHelicopter_Flag = value
    End Set
  End Property

  Public Property crmCommercial_Flag() As Boolean
    Get
      Return _crmCommercial_Flag
    End Get
    Set(ByVal value As Boolean)
      _crmCommercial_Flag = value
    End Set
  End Property
  Public Property crmYacht_Flag() As Boolean
    Get
      Return _crmYacht_Flag
    End Get
    Set(ByVal value As Boolean)
      _crmYacht_Flag = value
    End Set
  End Property
  Public Property crmJets_Flag() As Boolean
    Get
      Return _crmJets_Flag
    End Get
    Set(ByVal value As Boolean)
      _crmJets_Flag = value
    End Set
  End Property

  Public Property crmExecutive_Flag() As Boolean
    Get
      Return _crmExecutive_Flag
    End Get
    Set(ByVal value As Boolean)
      _crmExecutive_Flag = value
    End Set
  End Property

  Public Property crmTurboprops() As Boolean
    Get
      Return _crmTurboprops
    End Get
    Set(ByVal value As Boolean)
      _crmTurboprops = value
    End Set
  End Property

  Public Property crmStar_Reports_Flag() As Boolean
    Get
      Return _crmStar_Reports_Flag
    End Get
    Set(ByVal value As Boolean)
      _crmStar_Reports_Flag = value
    End Set
  End Property
  Public Property crmAppraiser_Flag() As Boolean
    Get
      Return _crmAppraiser_Flag
    End Get
    Set(ByVal value As Boolean)
      _crmAppraiser_Flag = value
    End Set
  End Property

  Public Property crmSalesPriceIndex_Flag() As Boolean
    Get
      Return _crmSalesPriceIndex_Flag
    End Get
    Set(ByVal value As Boolean)
      _crmSalesPriceIndex_Flag = value
    End Set
  End Property

  Public Property crmServerSideNotes_Flag() As Boolean
    Get
      Return _crmServerSideNotes_Flag
    End Get
    Set(ByVal value As Boolean)
      _crmServerSideNotes_Flag = value
    End Set
  End Property
  Public Property crmCloudNotes_Flag() As Boolean
    Get
      Return _crmCloudNotes_Flag
    End Get
    Set(ByVal value As Boolean)
      _crmCloudNotes_Flag = value
    End Set
  End Property
  Public Property crmServerSideDBName() As String
    Get
      Return _crmServerSideDBName
    End Get
    Set(ByVal value As String)
      _crmServerSideDBName = value
    End Set
  End Property
  Public Property crmSubscriptionShareType() As eSubscriptionShareType
    Get
      Return _crmSubscriptionShareType
    End Get
    Set(ByVal value As eSubscriptionShareType)
      _crmSubscriptionShareType = value
    End Set
  End Property

  Public Property crmCloudNotesDBName() As String
    Get
      Return _crmCloudNotesDBName
    End Get
    Set(ByVal value As String)
      _crmCloudNotesDBName = value
    End Set
  End Property
  Public Property crmRegID() As Long
    Get
      Return _crmRegID
    End Get
    Set(ByVal value As Long)
      _crmRegID = value
    End Set
  End Property

  Public Property crmServiceCode() As String
    Get
      Return _crmServiceCode
    End Get
    Set(ByVal value As String)
      _crmServiceCode = value
    End Set
  End Property

  Public Property crmServiceName() As String
    Get
      Return _crmServiceName
    End Get
    Set(ByVal value As String)
      _crmServiceName = value
    End Set
  End Property

End Class

<System.Serializable()> Public Class crmLocalUserClass
  Private _crmActiveX As Boolean
  Private _crmLocalSideNotes_Flag As Boolean
  Private _crmEnableNotes As Boolean



  Private _crmUserStatusCode As eObjStatusCode
  Private _crmUserDetailError As eObjDetailErrorCode

  Private _crmUserType As eUserTypes
  Private _crmUserLogin As String
  Private _crmSubUserID As String
  Private _crmSubPswdID As String
  Private _crmSubSeqNo As Long
  Private _crmEvo As Boolean

  'appened 1/137/13 
  Private _crmGUID As String

  'appended 8/2/12 
  Private _crmUser_aircraft_relationship As String 'Default Aircraft Relationship - Defaults to All Owners!

  'appended 1/12/12
  Private _crmUser_RegName As String
  Private _crmUser_DebugText As String

  Private _crmSubscriptionCode As String
  Private _crmSecurityToken As String

  Private _crmSubInstallDate As Date
  Private _crmSubAccessDate As Date

  Private _crmSubStartDate As Date
  Private _crmSubEndDate As Date

  Private _crmSubNoEndDate As Boolean

  Private _crmSubSubID As Long
  Private _crmSubParentID

  Private _crmUserCompanyID As Long
  Private _crmUserCompanyName As String
  Private _crmUserContactID As Long

  Private _crmLocalUserID As Long
  Private _crmUserSelectedModel As Long

  Private _crmLocalUserName As String
  Private _crmLocalUserPswd As String

  Private _crmLocalUserFirstName As String
  Private _crmLocalUserLastName As String
  Private _crmLocalUserEmailAddress As String

  'appended 7/2012 Background image for CRM
  Private _crmLocalUser_Background As String 'background image for CRM/EVO Name.

  'appended 3/20/13 Background ID
  Private _crmLocalUser_Background_ID As Long 'background ID
  Private _crmSelectedView As Integer
  Private _crmMobileFlag As Boolean
  Private _crmMobileNumber As String
  Private _crmDefaultProject As String
  Private _crmEmailReplyname As String
  Private _crmEmailReplyAddress As String
  Private _crmEmailFormat As String
  Private _crmCellService As String
  Private _crmCellCarrierID As Long
  Private _crmSMSStatus As String
  Private _crmCellEvents As String
  Private _crmSelectedModels As String
  Private _crmSMSSelectedModels As String
  Private _crmLocalDbFile As String
  Private _crmDisplayNoteTag As Boolean
  Private _crmPlatformOS As String
  Private _crmDontShowPics As Boolean
  Private _crmAllowExport_Flag As Boolean
  Private _crmAllowProjects_Flag As Boolean
  Private _crmAllowEmailRequest As Boolean
  Private _crmAllowEventRequest As Boolean
  Private _crmAllowTextMessage As Boolean
  Private _crmDemoUserFlag As Boolean
  Private _crmSubscriberNotices As Boolean
  Private _crmACListingView As eListingView
  Private _crmCompanyListingView As eListingView

  'appended 8/2/12 
  Private _crmUser_recs_per_page As Integer 'Default Records per Page - Defaults to 25!
  Private _crmLocalUser_Default_Models As String 'user level default models
  'added 3/8/13
  Private _crmMaxClientExport As Integer
  Private _crmLatestRecordSearch As Long

  'added 4/7/15
  'Prefix for Temporary Files
  Private _crmUserTemporaryFilePrefix As String

  'added 5/4/15
  'A boolean that allows the CRM to autolog aircraft changes as notes.
  Private _crmUser_Autolog_Flag As Boolean

  'Added 2/22/18
  Private _crmUser_Evo_MPM_User_Flag As Boolean

  'added 8/28/15
  'This is a session variable to store the attributes (area) into a datatable per user.
  Private _crmUserAttributesAreaTable As DataTable
  'This is a session variable to store the attributes (letter) into a datatable per user.
  Private _crmUserAttributesLetterTable As DataTable

  'Added 9/25/2015
  'This is a session variable to store the subins_default_airports field for Default Airports.
  Private _crmUserDefaultAirports As String

  'Added 11/11/16 for user level spi flag
  Private _crmUser_SPI_Flag As Boolean

  Private _crmUser_Evalues_CSS As String = "evalue_blue"
  Private _crmUser_CRM_Database_Not_Available As Boolean = False
  Private _crmUser_API_Login As Boolean = False
  ' Returns the input string encoded to base64
  Public Function EncodeBase64(ByVal input As String) As String
    Dim strBytes() As Byte = System.Text.Encoding.UTF8.GetBytes(input)
    Return System.Convert.ToBase64String(strBytes)
  End Function

  ' Returns the input string decoded from base64
  Public Function DecodeBase64(ByVal input As String) As String
    Dim strBytes() As Byte = System.Convert.FromBase64String(input)
    Return System.Text.Encoding.UTF8.GetChars(strBytes)
  End Function

  Public Sub New()
    _crmUser_Evo_MPM_User_Flag = False
    _crmUserStatusCode = eObjStatusCode.NULL
    _crmUserDetailError = eObjDetailErrorCode.NULL
    _crmUser_SPI_Flag = False
    _crmUser_API_Login = False

    _crmGUID = ""
    _crmUserType = eUserTypes.NULL
    _crmUserLogin = ""
    _crmUser_DebugText = ""
    _crmSubUserID = ""
    _crmSubPswdID = ""
    _crmUser_RegName = ""
    _crmEvo = False
    _crmSubInstallDate = Today()
    _crmSubAccessDate = Today()

    _crmSubStartDate = Today()
    _crmSubEndDate = Today()
    _crmSubSeqNo = 0
    _crmSubSubID = 0
    _crmSubParentID = 0
    _crmSubNoEndDate = False

    _crmSubscriptionCode = ""
    _crmSecurityToken = ""

    _crmUserSelectedModel = 0
    _crmUserCompanyID = 0
    _crmUserCompanyName = ""
    _crmUserContactID = 0

    _crmLocalUserID = 0
    _crmMaxClientExport = 0
    _crmLocalUserName = ""
    _crmLocalUserPswd = ""

    _crmLocalUserFirstName = ""
    _crmLocalUserLastName = ""
    _crmLocalUserEmailAddress = ""
    _crmLocalUser_Background = ""
    _crmLocalUser_Background_ID = 0
    _crmUser_aircraft_relationship = ""

    _crmSelectedView = 0
    _crmMobileFlag = False
    _crmMobileNumber = ""
    _crmDefaultProject = ""
    _crmEmailReplyname = ""
    _crmEmailReplyAddress = ""
    _crmEmailFormat = ""
    _crmCellService = ""
    _crmCellCarrierID = 0
    _crmSMSStatus = 0
    _crmCellEvents = ""
    _crmSelectedModels = ""
    _crmSMSSelectedModels = ""
    _crmPlatformOS = ""
    _crmLocalDbFile = ""
    _crmDisplayNoteTag = False
    _crmPlatformOS = ""
    _crmDontShowPics = False
    _crmAllowExport_Flag = False
    _crmAllowProjects_Flag = False
    _crmAllowEmailRequest = False
    _crmAllowEventRequest = False
    _crmAllowTextMessage = False
    _crmDemoUserFlag = False

    _crmUser_Evalues_CSS = "evalue_blue"

    _crmACListingView = eListingView.GALLERY
    _crmCompanyListingView = eListingView.LISTING
    _crmLocalSideNotes_Flag = False
    _crmEnableNotes = False
    _crmActiveX = False
    _crmSubscriberNotices = False
    _crmLatestRecordSearch = 0

    _crmUserTemporaryFilePrefix = ""
    _crmUser_Autolog_Flag = False
    _crmUserAttributesAreaTable = New DataTable
    _crmUserAttributesLetterTable = New DataTable
    _crmUserDefaultAirports = ""
    _crmUser_CRM_Database_Not_Available = False
  End Sub

  Public Function DisplayUser() As String
    DisplayUser = "Session.Item(""localUser"").crmUserStatusCode As eObjStatusCode: " & _crmUserStatusCode & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmUserDetailError As eObjDetailErrorCode: " & _crmUserDetailError & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmUserType As eUserTypes: " & _crmUserType & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmUserLogin As String: " & _crmUserLogin & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmSubUserID As String: " & _crmSubUserID & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmSubPswdID As String: " & _crmSubPswdID & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmSubSeqNo As Long: " & _crmSubSeqNo & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmEvo As Boolean: " & _crmEvo & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmUser_SPI_Flag As String: " & _crmUser_SPI_Flag.ToString & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmGUID As String: " & _crmGUID & "<br />"

    DisplayUser += "Session.Item(""localUser"").crmUserAircraftRelationship As String 'Default Aircraft Relationship - Defaults to All Owners!: " & _crmUser_aircraft_relationship & "<br />"

    DisplayUser += "Session.Item(""localUser"").crmUser_RegName As String: " & _crmUser_RegName & "<br />"

    DisplayUser += "Session.Item(""localUser"").crmSubscriptionCode As String: " & _crmSubscriptionCode & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmSecurityToken As String: " & _crmSecurityToken & "<br />"

    DisplayUser += "Session.Item(""localUser"").crmSubInstallDate As Date: " & _crmSubInstallDate & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmSubAccessDate As Date: " & _crmSubAccessDate & "<br />"

    DisplayUser += "Session.Item(""localUser"").crmSubStartDate As Date: " & _crmSubStartDate & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmSubEndDate As Date: " & _crmSubEndDate & "<br />"

    DisplayUser += "Session.Item(""localUser"").crmSubNoEndDate As Boolean: " & _crmSubNoEndDate & "<br />"

    DisplayUser += "Session.Item(""localUser"").crmSubSubID As Long: " & _crmSubSubID & "<br />"

    DisplayUser += "Session.Item(""localUser"").crmSubParentID As Long: " & _crmSubParentID & "<br />"

    DisplayUser += "Session.Item(""localUser"").crmCellEvents As String: " & _crmCellEvents & "<br />"

    DisplayUser += "Session.Item(""localUser"").crmUserCompanyID As Long: " & _crmUserCompanyID & "<br />"

    DisplayUser += "Session.Item(""localUser"").crmMaxClientExport As Integer: " & _crmMaxClientExport & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmLatestRecordCount As Integer: " & _crmLatestRecordSearch & "<br />"


    DisplayUser += "Session.Item(""localUser"").crmUserCompanyName As String: " & _crmUserCompanyName & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmUserContactID As Long: " & _crmUserContactID & "<br />"

    DisplayUser += "Session.Item(""localUser"").crmLocalUserID As Long: " & _crmLocalUserID & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmUserSelectedModel As Long: " & _crmUserSelectedModel & "<br />"

    DisplayUser += "Session.Item(""localUser"").crmLocalUserName As String: " & _crmLocalUserName & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmLocalUserPswd As String: " & _crmLocalUserPswd & "<br />"

    DisplayUser += "Session.Item(""localUser"").crmLocalUserFirstName As String: " & _crmLocalUserFirstName & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmLocalUserLastName As String: " & _crmLocalUserLastName & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmLocalUserEmailAddress As String: " & _crmLocalUserEmailAddress & "<br />"

    DisplayUser += "Session.Item(""localUser"").crmLocalUser_Background As String 'background image for CRM/Evo Background Name.: " & _crmLocalUser_Background & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmLocalUser_Background_ID As Long 'Evo Background ID#.: " & _crmLocalUser_Background_ID & "<br />"

    DisplayUser += "Session.Item(""localUser"").crmUserRecsPerPage As Integer 'Default Records per Page - Defaults to 25!: " & _crmUser_recs_per_page & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmUserDefaultModels As String 'user level default models (on the CRM side, holds the CRM model IDs): " & _crmLocalUser_Default_Models & "<br />"

    DisplayUser += "Session.Item(""localUser"").crmSMSSelectedModels As String 'user level default models: " & _crmSMSSelectedModels & "<br />"

    DisplayUser += "Session.Item(""localUser"").crmSelectedView As Integer : " & _crmSelectedView & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmMobileFlag As Boolean : " & _crmMobileFlag & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmMobileNumber As String : " & _crmMobileNumber & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmDefaultProject As String : " & _crmDefaultProject & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmEmailReplyname As String : " & _crmEmailReplyname & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmEmailReplyAddress As String: " & _crmEmailReplyAddress & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmEmailFormat As String: " & _crmEmailFormat & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmCellService As String : " & _crmCellService & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmCellCarrierID As Long : " & _crmCellCarrierID & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmSMSStatus As String : " & _crmSMSStatus & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmSMSSelectedModels As Boolean : " & _crmSMSSelectedModels & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmSelectedModels As String : " & _crmSelectedModels & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmPlatformOS as string: " & _crmPlatformOS & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmLocalDbFile As String : " & _crmLocalDbFile & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmDisplayNoteTag As Boolean : " & _crmDisplayNoteTag & "<br />"

    DisplayUser += "Session.Item(""localUser"").crmPlatformOS As String : " & _crmPlatformOS & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmDontShowPics As Boolean : " & _crmDontShowPics & " <- Value (To be removed when references are gone)<br />"
    DisplayUser += "Session.Item(""localUser"").crmAllowExport_Flag As Boolean : " & _crmAllowExport_Flag & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmAllowProjects_Flag As Boolean : " & _crmAllowProjects_Flag & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmAllowEmailRequest As Boolean : " & _crmAllowEmailRequest & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmAllowEventRequest As Boolean : " & _crmAllowEventRequest & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmAllowTextMessage As Boolean : " & _crmAllowTextMessage & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmDemoUserFlag As Boolean : " & _crmDemoUserFlag & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmLocalSideNotes_Flag As Boolean: " & _crmLocalSideNotes_Flag & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmUser_Evo_MPM_Flag As Boolean: " & _crmUser_Evo_MPM_User_Flag & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmActiveX As Boolean: " & _crmActiveX & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmEnableNotes As Boolean: " & _crmEnableNotes & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmSubscriberNotices As Boolean: " & _crmSubscriberNotices & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmACListingView As Boolean: " & _crmACListingView & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmCompanyListingView As Boolean: " & _crmCompanyListingView & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmUserTemporaryFilePrefix As String: " & _crmUserTemporaryFilePrefix & "<br />"
    DisplayUser += "Session.Item(""localUser"").crmUser_Autolog_Flag As String: " & _crmUser_Autolog_Flag & "<br />"

    DisplayUser += "Session.Item(""localUser"").crmUser_Evalues_CSS As String: " & _crmUser_Evalues_CSS.ToString & "<br />"

    DisplayUser += "Session.Item(""localUser"").crmUser_CRM_Database_Not_Available As Boolean: " & _crmUser_CRM_Database_Not_Available.ToString & "<br />"

    DisplayUser += "Session.Item(""localUser"").crmUser_API_Login As Boolean: " & _crmUser_API_Login.ToString & "<br />"

    If Not IsNothing(crmUserAttributeAreaDatatable) Then
      DisplayUser += "Session.Item(""localUser"").crmUserAttributeAreaDatatable As Datatable: The row count is: " & crmUserAttributeAreaDatatable.Rows.Count & "<br />"
    Else
      DisplayUser += "Session.Item(""localUser"").crmUserAttributeAreaDatatable As Datatable: Nothing<br />"
    End If

    If Not IsNothing(crmUserAttributeLetterDatatable) Then
      DisplayUser += "Session.Item(""localUser"").crmUserAttributeLetterDatatable As Datatable: The row count is: " & crmUserAttributeLetterDatatable.Rows.Count & "<br />"
    Else
      DisplayUser += "Session.Item(""localUser"").crmUserAttributeLetterDatatable As Datatable: Nothing<br />"
    End If

    DisplayUser += "Session.Item(""localUser"").crmUserDefaultAirports As String: " & _crmUserDefaultAirports & "<br />"
  End Function



  Public Property crmUser_API_Login() As Boolean
    Get
      Return _crmUser_API_Login
    End Get
    Set(ByVal value As Boolean)
      _crmUser_API_Login = value
    End Set
  End Property
  Public Property crmUser_CRM_Database_Not_Available() As Boolean
    Get
      Return _crmUser_CRM_Database_Not_Available
    End Get
    Set(ByVal value As Boolean)
      _crmUser_CRM_Database_Not_Available = value
    End Set
  End Property

  Public Property crmUser_Evalues_CSS() As String
    Get
      Return _crmUser_Evalues_CSS
    End Get
    Set(ByVal value As String)
      _crmUser_Evalues_CSS = value
    End Set
  End Property


  Public Property crmUser_Evo_MPM_Flag() As Boolean
    Get
      Return _crmUser_Evo_MPM_User_Flag
    End Get
    Set(ByVal value As Boolean)
      _crmUser_Evo_MPM_User_Flag = value
    End Set
  End Property
  Public Property crmUser_SPI_Flag() As Boolean
    Get
      Return _crmUser_SPI_Flag
    End Get
    Set(ByVal value As Boolean)
      _crmUser_SPI_Flag = value
    End Set
  End Property
  Public Property crmUser_Autolog_Flag() As Boolean
    Get
      Return _crmUser_Autolog_Flag
    End Get
    Set(ByVal value As Boolean)
      _crmUser_Autolog_Flag = value
    End Set
  End Property

  Public Property crmLocalSideNotes_Flag() As Boolean
    Get
      Return _crmLocalSideNotes_Flag
    End Get
    Set(ByVal value As Boolean)
      _crmLocalSideNotes_Flag = value
    End Set
  End Property
  Public Property crmActiveX() As Boolean
    Get
      Return _crmActiveX
    End Get
    Set(ByVal value As Boolean)
      _crmActiveX = value
    End Set
  End Property
  Public Property crmEnableNotes() As Boolean
    Get
      Return _crmEnableNotes
    End Get
    Set(ByVal value As Boolean)
      _crmEnableNotes = value
    End Set
  End Property
  Public Property crmDemoUserFlag() As Boolean
    Get
      Return _crmDemoUserFlag
    End Get
    Set(ByVal value As Boolean)
      _crmDemoUserFlag = value
    End Set
  End Property
  Public Property crmAllowExport_Flag() As Boolean
    Get
      Return _crmAllowExport_Flag
    End Get
    Set(ByVal value As Boolean)
      _crmAllowExport_Flag = value
    End Set
  End Property
  Public Property crmSubscriberNotices() As Boolean
    Get
      Return _crmSubscriberNotices
    End Get
    Set(ByVal value As Boolean)
      _crmSubscriberNotices = value
    End Set
  End Property
  Public Property crmAllowProjects_Flag() As Boolean
    Get
      Return _crmAllowProjects_Flag
    End Get
    Set(ByVal value As Boolean)
      _crmAllowProjects_Flag = value
    End Set
  End Property
  Public Property crmAllowEmailRequest() As Boolean
    Get
      Return _crmAllowEmailRequest
    End Get
    Set(ByVal value As Boolean)
      _crmAllowEmailRequest = value
    End Set
  End Property
  Public Property crmAllowTextMessage() As Boolean
    Get
      Return _crmAllowTextMessage
    End Get
    Set(ByVal value As Boolean)
      _crmAllowTextMessage = value
    End Set
  End Property
  Public Property crmAllowEventRequest() As Boolean
    Get
      Return _crmAllowEventRequest
    End Get
    Set(ByVal value As Boolean)
      _crmAllowEventRequest = value
    End Set
  End Property
  Public Property crmUserStatusCode() As eObjStatusCode
    Get
      Return _crmUserStatusCode
    End Get
    Set(ByVal value As eObjStatusCode)
      _crmUserStatusCode = value
    End Set
  End Property
  Public Property crmUserDetailError() As eObjDetailErrorCode
    Get
      Return _crmUserDetailError
    End Get
    Set(ByVal value As eObjDetailErrorCode)
      _crmUserDetailError = value
    End Set
  End Property

  Public Property crmUserType() As eUserTypes
    Get
      Return _crmUserType
    End Get
    Set(ByVal value As eUserTypes)
      _crmUserType = value
    End Set
  End Property
  Public Property crmACListingView() As eListingView
    Get
      ' wouldnt you want to get from the cookie and set to the cookie?
      Return _crmACListingView
    End Get
    Set(ByVal value As eListingView)
      _crmACListingView = value

      HttpContext.Current.Response.Cookies("ACListingView").Values("VIEW") = value
      HttpContext.Current.Response.Cookies("ACListingView").Values("USER") = _crmUserCompanyID.ToString & _crmUserContactID.ToString & _crmSubSubID.ToString
      HttpContext.Current.Response.Cookies("ACListingView").Expires = DateTime.Now.AddDays(10)
    End Set
  End Property

  Public Property crmCompanyListingView() As eListingView
    Get
      Return _crmCompanyListingView
    End Get
    Set(ByVal value As eListingView)
      _crmCompanyListingView = value

      HttpContext.Current.Response.Cookies("CompanyListingView").Values("VIEW") = value
      HttpContext.Current.Response.Cookies("CompanyListingView").Values("USER") = _crmUserCompanyID.ToString & _crmUserContactID.ToString & _crmSubSubID.ToString
      HttpContext.Current.Response.Cookies("CompanyListingView").Expires = DateTime.Now.AddDays(10)
    End Set
  End Property
  'holds the list of default CRM model IDs.
  Public Property crmUserDefaultModels() As String
    Get
      Return _crmLocalUser_Default_Models
    End Get
    Set(ByVal value As String)
      _crmLocalUser_Default_Models = value
    End Set
  End Property
  'holds the default Aircraft Relationships
  Public Property crmUserAircraftRelationship() As String
    Get
      Return _crmUser_aircraft_relationship
    End Get
    Set(ByVal value As String)
      _crmUser_aircraft_relationship = value
    End Set
  End Property
  'holds the GUID
  Public Property crmGUID() As String
    Get
      Return _crmGUID
    End Get
    Set(ByVal value As String)
      _crmGUID = value
    End Set
  End Property
  Public Property crmUserLogin() As String
    Get
      Return _crmUserLogin
    End Get
    Set(ByVal value As String)
      _crmUserLogin = value
    End Set
  End Property
  Public Property crmEvo() As Boolean
    Get
      Return _crmEvo
    End Get
    Set(ByVal value As Boolean)
      _crmEvo = value
    End Set
  End Property
  Public Property crmUser_DebugText() As String
    Get
      Return _crmUser_DebugText
    End Get
    Set(ByVal value As String)
      _crmUser_DebugText = value
    End Set
  End Property

  Public Property crmUser_RegName() As String
    Get
      Return _crmUser_RegName
    End Get
    Set(ByVal value As String)
      _crmUser_RegName = value
    End Set
  End Property

  Public Property crmSubUserID() As String
    Get
      Return _crmSubUserID
    End Get
    Set(ByVal value As String)
      _crmSubUserID = value
    End Set
  End Property

  Public Property crmSubPswdID() As String
    Get
      Return _crmSubPswdID
    End Get
    Set(ByVal value As String)
      _crmSubPswdID = value
    End Set
  End Property

  Public Property crmSubInstallDate() As Date
    Get
      Return _crmSubInstallDate
    End Get
    Set(ByVal value As Date)
      _crmSubInstallDate = value
    End Set
  End Property

  Public Property crmSubAccessDate() As Date
    Get
      Return _crmSubAccessDate
    End Get
    Set(ByVal value As Date)
      _crmSubAccessDate = value
    End Set
  End Property

  Public Property crmSubStartDate() As Date
    Get
      Return _crmSubStartDate
    End Get
    Set(ByVal value As Date)
      _crmSubStartDate = value
    End Set
  End Property

  Public Property crmSubEndDate() As Date
    Get
      Return _crmSubEndDate
    End Get
    Set(ByVal value As Date)
      _crmSubEndDate = value
    End Set
  End Property

  Public Property crmSubNoEndDate() As Boolean
    Get
      Return _crmSubNoEndDate
    End Get
    Set(ByVal value As Boolean)
      _crmSubNoEndDate = value
    End Set
  End Property

  Public Property crmSubscriptionCode() As String
    Get
      Return _crmSubscriptionCode
    End Get
    Set(ByVal value As String)
      _crmSubscriptionCode = value
    End Set
  End Property

  Public Property crmSecurityToken() As String
    Get
      Return _crmSecurityToken
    End Get
    Set(ByVal value As String)
      _crmSecurityToken = value
    End Set
  End Property

  Public Property crmSubSeqNo() As Long
    Get
      Return _crmSubSeqNo
    End Get
    Set(ByVal value As Long)
      _crmSubSeqNo = value
    End Set
  End Property

  Public Property crmUserSelectedModel() As Long
    Get
      Return _crmUserSelectedModel
    End Get
    Set(ByVal value As Long)
      _crmUserSelectedModel = value
    End Set
  End Property

  Public Property crmMaxClientExport() As Integer
    Get
      Return _crmMaxClientExport
    End Get
    Set(ByVal value As Integer)
      _crmMaxClientExport = value
    End Set
  End Property


  Public Property crmSubSubID() As Long
    Get
      Return _crmSubSubID
    End Get
    Set(ByVal value As Long)
      _crmSubSubID = value
    End Set
  End Property

  Public Property crmSubParentID() As Long
    Get
      Return _crmSubParentID
    End Get
    Set(ByVal value As Long)
      _crmSubParentID = value
    End Set
  End Property


  Public Property crmUserCompanyName() As String
    Get
      Return _crmUserCompanyName
    End Get
    Set(ByVal value As String)
      _crmUserCompanyName = value
    End Set
  End Property
  Public Property crmPlatformOS() As String
    Get
      Return _crmPlatformOS
    End Get
    Set(ByVal value As String)
      _crmPlatformOS = value
    End Set
  End Property
  Public Property crmDontShowPics() As Boolean
    Get
      Return _crmDontShowPics
    End Get
    Set(ByVal value As Boolean)
      _crmDontShowPics = value
    End Set
  End Property

  Public Property crmUserCompanyID() As Long
    Get
      Return _crmUserCompanyID
    End Get
    Set(ByVal value As Long)
      _crmUserCompanyID = value
    End Set
  End Property

  Public Property crmUserContactID() As Long
    Get
      Return _crmUserContactID
    End Get
    Set(ByVal value As Long)
      _crmUserContactID = value
    End Set
  End Property

  Public Property crmLocalUserID() As Long
    Get
      Return _crmLocalUserID
    End Get
    Set(ByVal value As Long)
      _crmLocalUserID = value
    End Set
  End Property

  Public Property crmLocalUserName() As String
    Get
      Return _crmLocalUserName
    End Get
    Set(ByVal value As String)
      _crmLocalUserName = value
    End Set
  End Property

  Public Property crmLocalUserPswd() As String
    Get
      Return _crmLocalUserPswd
    End Get
    Set(ByVal value As String)
      _crmLocalUserPswd = value
    End Set
  End Property

  Public Property crmLocalUserFirstName() As String
    Get
      Return _crmLocalUserFirstName
    End Get
    Set(ByVal value As String)
      _crmLocalUserFirstName = value
    End Set
  End Property

  Public Property crmLocalUserLastName() As String
    Get
      Return _crmLocalUserLastName
    End Get
    Set(ByVal value As String)
      _crmLocalUserLastName = value
    End Set
  End Property

  Public Property crmLocalUserEmailAddress() As String
    Get
      Return _crmLocalUserEmailAddress
    End Get
    Set(ByVal value As String)
      _crmLocalUserEmailAddress = value
    End Set
  End Property

  Public Property crmLocalUser_Background() As String
    Get
      Return _crmLocalUser_Background
    End Get
    Set(ByVal value As String)
      _crmLocalUser_Background = value
    End Set
  End Property

  Public Property crmLocalUser_Background_ID() As Long
    Get
      Return _crmLocalUser_Background_ID
    End Get
    Set(ByVal value As Long)
      _crmLocalUser_Background_ID = value
    End Set
  End Property
  Public Property crmSelectedView() As Integer
    Get
      Return _crmSelectedView
    End Get
    Set(ByVal value As Integer)
      _crmSelectedView = value
    End Set
  End Property
  Public Property crmMobileFlag() As Boolean
    Get
      Return _crmMobileFlag
    End Get
    Set(ByVal value As Boolean)
      _crmMobileFlag = value
    End Set
  End Property
  Public Property crmMobileNumber() As String
    Get
      Return _crmMobileNumber
    End Get
    Set(ByVal value As String)
      _crmMobileNumber = value
    End Set
  End Property
  Public Property crmDefaultProject() As String
    Get
      Return _crmDefaultProject
    End Get
    Set(ByVal value As String)
      _crmDefaultProject = value
    End Set
  End Property
  Public Property crmEmailReplyname() As String
    Get
      Return _crmEmailReplyname
    End Get
    Set(ByVal value As String)
      _crmEmailReplyname = value
    End Set
  End Property
  Public Property crmEmailReplyAddress() As String
    Get
      Return _crmEmailReplyAddress
    End Get
    Set(ByVal value As String)
      _crmEmailReplyAddress = value
    End Set
  End Property
  Public Property crmEmailFormat() As String
    Get
      Return _crmEmailFormat
    End Get
    Set(ByVal value As String)
      _crmEmailFormat = value
    End Set
  End Property
  Public Property crmCellService() As String
    Get
      Return _crmCellService
    End Get
    Set(ByVal value As String)
      _crmCellService = value
    End Set
  End Property
  Public Property crmSMSSelectedModels() As String
    Get
      Return _crmSMSSelectedModels
    End Get
    Set(ByVal value As String)
      _crmSMSSelectedModels = value
    End Set
  End Property
  'On the CRM side and on the Evo side, it holds the default model IDS that correspond with the jetnet IDs
  Public Property crmSelectedModels() As String
    Get
      Return _crmSelectedModels
    End Get
    Set(ByVal value As String)
      _crmSelectedModels = value
    End Set
  End Property
  Public Property crmLocalDbFile() As String
    Get
      Return _crmLocalDbFile
    End Get
    Set(ByVal value As String)
      _crmLocalDbFile = value
    End Set
  End Property
  Public Property crmCellCarrierID() As Long
    Get
      Return _crmCellCarrierID
    End Get
    Set(ByVal value As Long)
      _crmCellCarrierID = value
    End Set
  End Property
  Public Property crmSMSStatus() As String
    Get
      Return _crmSMSStatus
    End Get
    Set(ByVal value As String)
      _crmSMSStatus = value
    End Set
  End Property
  Public Property crmCellEvents() As String
    Get
      Return _crmCellEvents
    End Get
    Set(ByVal value As String)
      _crmCellEvents = value
    End Set
  End Property

  Public Property crmDisplayNoteTag() As Boolean
    Get
      Return _crmDisplayNoteTag
    End Get
    Set(ByVal value As Boolean)
      _crmDisplayNoteTag = value
    End Set
  End Property

  Public Property crmUserRecsPerPage() As Integer
    Get
      Return _crmUser_recs_per_page
    End Get
    Set(ByVal value As Integer)
      _crmUser_recs_per_page = value
    End Set
  End Property

  Public Property crmLatestRecordCount() As Integer
    Get
      Return _crmLatestRecordSearch
    End Get
    Set(ByVal value As Integer)
      _crmLatestRecordSearch = value
    End Set
  End Property


  Public Property crmUserTemporaryFilePrefix() As String
    Get
      Return _crmUserTemporaryFilePrefix
    End Get
    Set(ByVal value As String)
      _crmUserTemporaryFilePrefix = value
    End Set
  End Property
  Public Property crmUserAttributeAreaDatatable() As DataTable
    Get
      Return _crmUserAttributesAreaTable
    End Get
    Set(ByVal value As DataTable)
      _crmUserAttributesAreaTable = value
    End Set
  End Property

  Public Property crmUserAttributeLetterDatatable() As DataTable
    Get
      Return _crmUserAttributesLetterTable
    End Get
    Set(ByVal value As DataTable)
      _crmUserAttributesLetterTable = value
    End Set
  End Property

  Public Property crmUserDefaultAirports() As String
    Get
      Return _crmUserDefaultAirports
    End Get
    Set(ByVal value As String)
      _crmUserDefaultAirports = value
    End Set
  End Property
End Class
