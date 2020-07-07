Imports Microsoft.VisualBasic
Imports System.ComponentModel

' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/clsSubscriptionCls.vb $
'$$Author: Mike $
'$$Date: 11/05/19 9:14a $
'$$Modtime: 11/05/19 9:01a $
'$$Revision: 7 $
'$$Workfile: clsSubscriptionCls.vb $
'
' ********************************************************************************

<System.Serializable()> Public Class clsSubscriptionClass

  Private _SubStatusCode As eObjStatusCode
  Private _SubDetailError As eObjDetailErrorCode

  Private _DatalayerType As eDatalayerTypes
  Private _DatabaseType As eDatabaseTypes
  Private _LogonType As eLogonTypes

  Private _UserType As eUserTypes

  Private _SessionGUID As String

  Private _LastLoginDate As Date
  Private _LastSessionDate As Date
  Private _LastLogoutDate As Date

  Private _Login As String
  Private _UserID As String
  Private _SeqNo As Long
  Private _SubID As Long
  Private _crmUserID As Long
  Private _SubCompName As String
  Private _OsBrowser As String

  Private _ServiceCode As String
  Private _ServiceName As String
  Private _ProductCode() As eProductCodeTypes
  Private _ProductName As String

  Private _Tierlevel As eTierLevelTypes

  ' flags for turning off options
  Private _AerodexFlag As Boolean

  Private _DemoFlag As Boolean
  Private _MarketingFlag As Boolean
  Private _EnableNotesFlag As Boolean
  Private _ExportReportFlag As Boolean
  Private _EmailRequestFlag As Boolean
  Private _EventRequestFlag As Boolean
  Private _SaveProjectsFlag As Boolean
  Private _EnableTextFlag As Boolean

  Private _SMSActiveFlag As Boolean
  Private _SMSActivationStatus As eSMSActivateTypes
  Private _MobleWebStatus As Boolean
  Private _SmsPhoneNumber As String
  Private _SmsProviderName As String
  Private _SmsProviderID As Long
  Private _SmsSelectedEvents As String
  Private _SmsSelectedModels As String

  Private _bHasNotifications As Boolean

  Private _UserBusinessFlag As Boolean
  Private _UserHelicoptersFlag As Boolean
  Private _UserRegionalFlag As Boolean
  Private _UserCommercialFlag As Boolean
  Private _UserAirBPFlag As Boolean
  Private _UserABIFlag As Boolean
  Private _isUserStarRptFlag As Boolean
  Private _isUserSPIViewFlag As Boolean
  Private _isUserYachtFlag As Boolean

  Private _isHeliOnlyProduct As Boolean
  Private _isCommercialOnlyProduct As Boolean
  Private _isBusinessOnlyProduct As Boolean
  Private _isYachtOnlyProduct As Boolean

  Private _ShowNoteOnACList As Boolean
  Private _HasServerNotes As Boolean
  Private _HasCloudNotes As Boolean
  Private _ShowNotes As Boolean
  Private _ShowReminders As Boolean

  Private _ServerNotesDatabaseName As String

  Private _ServerNotesDatabaseConn As String

  Private _UserDatabaseConn As String
  Private _STARDatabaseConn As String

  Private _CloudNotesDatabaseName As String

  Private _AppUserName As String

  Private _UserContactID As Long
  Private _UserCompanyID As Long

  Private _DefaultHomeView As Long
  Private _DefaultModelID As Long

  Private _DefaultCompanyType As String
  Private _CompanyType As String
  Private _PageBackground As String

  Private _UserPageSize As Long

  Private _UserEmailReplyToName As String
  Private _UserEmailReplyToAddress As String
  Private _UserEmailDefaultFormat As String

  Private _UseStandardOrMetric As String
  Private _UseMetricValue As Boolean
  Private _UseStatuteMile As Boolean

  Private _DefaultCurrency As Integer
  Private _CurrencyExchangeRate As Double

  Private _userBrowserType As String

  Private _userDefaultModelList As String

  Private _UserChatEnabled As Boolean

  Private _MaxAllowedCustomExport As Integer

  Private _ShareByCompanyFlag As Boolean
  Private _ShareByParentSubFlag As Boolean
  Private _UserAdminFlag As Boolean

  Private _businessSegment As String
  Private _defaultAnalysisMonths As Integer

  Private _AerodexStandard As Boolean
  Private _AerodexElite As Boolean

  Private _ShowListingsOnGlobal As Boolean
  Private _HasGlobalRecord As Boolean

  Sub New()

    _SubStatusCode = eObjStatusCode.NULL
    _SubDetailError = eObjDetailErrorCode.NULL
    _DatabaseType = eDatabaseTypes.NULL
    _DatalayerType = eDatalayerTypes.NULL
    _LogonType = eLogonTypes.NULL
    _UserType = eUserTypes.NULL
    _SessionGUID = ""

    _AerodexFlag = False
    _LastLoginDate = Today()
    _LastSessionDate = Today()
    _LastLogoutDate = Today()

    _Login = ""
    _UserID = ""
    _SeqNo = 0
    _SubID = 0
    _crmUserID = 0
    _SubCompName = ""
    _OsBrowser = ""

    _ServiceCode = ""
    _ServiceName = ""
    _ProductCode = Nothing
    _ProductName = ""

    _Tierlevel = eTierLevelTypes.NULL

    ' flags for turning off options
    _HasServerNotes = False
    _HasCloudNotes = False

    _ShowNotes = False
    _ShowReminders = False
    _DemoFlag = False
    _MarketingFlag = False
    _EnableNotesFlag = False
    _ExportReportFlag = False
    _EmailRequestFlag = False
    _EventRequestFlag = False
    _SaveProjectsFlag = False
    _EnableTextFlag = False

    _SMSActiveFlag = False
    _SMSActivationStatus = eSMSActivateTypes.NULL
    _MobleWebStatus = False
    _SmsPhoneNumber = ""
    _SmsProviderName = ""
    _SmsProviderID = 0
    _SmsSelectedEvents = ""
    _SmsSelectedModels = ""

    _bHasNotifications = False

    _UserBusinessFlag = False
    _UserHelicoptersFlag = False
    _UserRegionalFlag = False
    _UserCommercialFlag = False
    _UserAirBPFlag = False
    _UserABIFlag = False
    _isUserStarRptFlag = False
    _isUserSPIViewFlag = False
    _isUserYachtFlag = False

    _isHeliOnlyProduct = False
    _isCommercialOnlyProduct = False
    _isBusinessOnlyProduct = False
    _isYachtOnlyProduct = False

    _ShowNoteOnACList = False
    _ServerNotesDatabaseName = ""
    _ServerNotesDatabaseConn = ""

    _CloudNotesDatabaseName = ""

    _UserDatabaseConn = ""
    _STARDatabaseConn = ""

    _AppUserName = ""

    _UserContactID = 0
    _UserCompanyID = 0

    _DefaultHomeView = 0
    _DefaultModelID = 0

    _DefaultCompanyType = ""
    _CompanyType = ""

    _PageBackground = ""

    _UserPageSize = 10

    _UserEmailReplyToName = ""
    _UserEmailReplyToAddress = ""
    _UserEmailDefaultFormat = "HTML"

    _UseStandardOrMetric = "standard"
    _UseMetricValue = False
    _UseStatuteMile = False

    _DefaultCurrency = 9
    _CurrencyExchangeRate = 0.0

    _userBrowserType = ""

    _userDefaultModelList = ""

    _UserChatEnabled = False

    _MaxAllowedCustomExport = 0

    _ShareByCompanyFlag = False
    _ShareByParentSubFlag = False
    _UserAdminFlag = False

    _businessSegment = ""
    _defaultAnalysisMonths = 0

    _AerodexStandard = False
    _AerodexElite = False

    _ShowListingsOnGlobal = False
    _HasGlobalRecord = False

  End Sub

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

  Public Property SubStatusCode() As eObjStatusCode
    Get
      Return _SubStatusCode
    End Get
    Set(ByVal value As eObjStatusCode)
      _SubStatusCode = value
    End Set
  End Property

  Public Property SubDetailError() As eObjDetailErrorCode
    Get
      Return _SubDetailError
    End Get
    Set(ByVal value As eObjDetailErrorCode)
      _SubDetailError = value
    End Set
  End Property

  Public Property LogonType() As eLogonTypes
    Get
      Return _LogonType
    End Get
    Set(ByVal value As eLogonTypes)
      _LogonType = value
    End Set
  End Property

  Public Property DatabaseType() As eDatabaseTypes
    Get
      Return _DatabaseType
    End Get
    Set(ByVal value As eDatabaseTypes)
      _DatabaseType = value
    End Set
  End Property

  Public Property DataLayerType() As eDatalayerTypes
    Get
      Return _DatalayerType
    End Get
    Set(ByVal value As eDatalayerTypes)
      _DatalayerType = value
    End Set
  End Property

  Public Property SessionGUID() As String
    Get
      Return _SessionGUID
    End Get
    Set(ByVal value As String)
      _SessionGUID = value
    End Set
  End Property

  Public Property AerodexFlag() As Boolean
    Get
      Return _AerodexFlag
    End Get
    Set(ByVal value As Boolean)
      _AerodexFlag = value
    End Set
  End Property

  Public Property LastLoginDate() As Date
    Get
      Return _LastLoginDate
    End Get
    Set(ByVal value As Date)
      _LastLoginDate = value
    End Set
  End Property

  Public Property LastSessionDate() As Date
    Get
      Return _LastSessionDate
    End Get
    Set(ByVal value As Date)
      _LastSessionDate = value
    End Set
  End Property

  Public Property LastLogoutDate() As Date
    Get
      Return _LastLogoutDate
    End Get
    Set(ByVal value As Date)
      _LastLogoutDate = value
    End Set
  End Property

  Public Property Login() As String
    Get
      Return _Login
    End Get
    Set(ByVal value As String)
      _Login = value
    End Set
  End Property

  Public Property UserID() As String
    Get
      Return _UserID
    End Get
    Set(ByVal value As String)
      _UserID = value
    End Set
  End Property

  Public Property SeqNo() As Long
    Get
      Return _SeqNo
    End Get
    Set(ByVal value As Long)
      _SeqNo = value
    End Set
  End Property

  Public Property SubID() As Long
    Get
      Return _SubID
    End Get
    Set(ByVal value As Long)
      _SubID = value
    End Set
  End Property

  Public Property crmUserID() As Long  '_OsPlatform 
    Get
      Return _crmUserID
    End Get
    Set(ByVal value As Long)
      _crmUserID = value
    End Set
  End Property

  Public Property SubCompanyName() As String
    Get
      Return _SubCompName
    End Get
    Set(ByVal value As String)
      _SubCompName = value
    End Set
  End Property

  Public Property OsBrowser() As String
    Get
      Return _OsBrowser
    End Get
    Set(ByVal value As String)
      _OsBrowser = value
    End Set
  End Property

  Public Property ServiceCode() As String
    Get
      Return _ServiceCode
    End Get
    Set(ByVal value As String)
      _ServiceCode = value
    End Set
  End Property

  Public Property ServiceName() As String
    Get
      Return _ServiceName
    End Get
    Set(ByVal value As String)
      _ServiceName = value
    End Set
  End Property

  Public Property ProductCode() As eProductCodeTypes()
    Get
      Return _ProductCode
    End Get
    Set(ByVal value As eProductCodeTypes())
      _ProductCode = value
    End Set
  End Property

  Public Property ProductName() As String
    Get
      Return _ProductName
    End Get
    Set(ByVal value As String)
      _ProductName = value
    End Set
  End Property

  Public Property Tierlevel() As eTierLevelTypes
    Get
      Return _Tierlevel
    End Get
    Set(ByVal value As eTierLevelTypes)
      _Tierlevel = value
    End Set
  End Property

  Public Property HasCloudNotes() As Boolean
    Get
      Return _HasCloudNotes
    End Get
    Set(ByVal value As Boolean)
      _HasCloudNotes = value
    End Set
  End Property

  Public Property HasServerNotes() As Boolean
    Get
      Return _HasServerNotes
    End Get
    Set(ByVal value As Boolean)
      _HasServerNotes = value
    End Set
  End Property

  Public Property DemoFlag() As Boolean
    Get
      Return _DemoFlag
    End Get
    Set(ByVal value As Boolean)
      _DemoFlag = value
    End Set
  End Property

  Public Property MarketingFlag() As Boolean
    Get
      Return _MarketingFlag
    End Get
    Set(ByVal value As Boolean)
      _MarketingFlag = value
    End Set
  End Property

  Public Property EnableNotesFlag() As Boolean
    Get
      Return _EnableNotesFlag
    End Get
    Set(ByVal value As Boolean)
      _EnableNotesFlag = value
    End Set
  End Property

  Public Property ExportReportFlag() As Boolean
    Get
      Return _ExportReportFlag
    End Get
    Set(ByVal value As Boolean)
      _ExportReportFlag = value
    End Set
  End Property

  Public Property EmailRequestFlag() As Boolean
    Get
      Return _EmailRequestFlag
    End Get
    Set(ByVal value As Boolean)
      _EmailRequestFlag = value
    End Set
  End Property

  Public Property EventRequestFlag() As Boolean
    Get
      Return _EventRequestFlag
    End Get
    Set(ByVal value As Boolean)
      _EventRequestFlag = value
    End Set
  End Property

  Public Property SaveProjectsFlag() As Boolean
    Get
      Return _SaveProjectsFlag
    End Get
    Set(ByVal value As Boolean)
      _SaveProjectsFlag = value
    End Set
  End Property

  Public Property EnableTextFlag() As Boolean
    Get
      Return _EnableTextFlag
    End Get
    Set(ByVal value As Boolean)
      _EnableTextFlag = value
    End Set
  End Property

  Public Property SMSActiveFlag() As Boolean
    Get
      Return _SMSActiveFlag
    End Get
    Set(ByVal value As Boolean)
      _SMSActiveFlag = value
    End Set
  End Property

  Public Property SMSActivationStatus() As eSMSActivateTypes
    Get
      Return _SMSActivationStatus
    End Get
    Set(ByVal value As eSMSActivateTypes)
      _SMSActivationStatus = value
    End Set
  End Property

  Public Property MobleWebStatus() As Boolean
    Get
      Return _MobleWebStatus
    End Get
    Set(ByVal value As Boolean)
      _MobleWebStatus = value
    End Set
  End Property

  Public Property bHasNotifications() As Boolean
    Get
      Return _bHasNotifications
    End Get
    Set(ByVal value As Boolean)
      _bHasNotifications = value
    End Set
  End Property

  Public Property UserHelicopterFlag() As Boolean
    Get
      Return _UserHelicoptersFlag
    End Get
    Set(ByVal value As Boolean)
      _UserHelicoptersFlag = value
    End Set
  End Property

  Public Property UserBusinessFlag() As Boolean
    Get
      Return _UserBusinessFlag
    End Get
    Set(ByVal value As Boolean)
      _UserBusinessFlag = value
    End Set
  End Property

  Public Property UserRegionalFlag() As Boolean
    Get
      Return _UserRegionalFlag
    End Get
    Set(ByVal value As Boolean)
      _UserRegionalFlag = value
    End Set
  End Property

  Public Property UserCommercialFlag() As Boolean
    Get
      Return _UserCommercialFlag
    End Get
    Set(ByVal value As Boolean)
      _UserCommercialFlag = value
    End Set
  End Property

  Public Property UserAirBPFlag() As Boolean
    Get
      Return _UserAirBPFlag
    End Get
    Set(ByVal value As Boolean)
      _UserAirBPFlag = value
    End Set
  End Property

  Public Property UserABIFlag() As Boolean
    Get
      Return _UserABIFlag
    End Get
    Set(ByVal value As Boolean)
      _UserABIFlag = value
    End Set
  End Property

  Public Property UserStarRptFlag() As Boolean
    Get
      Return _isUserStarRptFlag
    End Get
    Set(ByVal value As Boolean)
      _isUserStarRptFlag = value
    End Set
  End Property

  Public Property UserSPIViewFlag() As Boolean
    Get
      Return _isUserSPIViewFlag
    End Get
    Set(ByVal value As Boolean)
      _isUserSPIViewFlag = value
    End Set
  End Property

  Public Property UserYachtFlag() As Boolean
    Get
      Return _isUserYachtFlag
    End Get
    Set(ByVal value As Boolean)
      _isUserYachtFlag = value
    End Set
  End Property

  Public Property isYachtOnlyProduct() As Boolean
    Get
      Return _isYachtOnlyProduct
    End Get
    Set(ByVal value As Boolean)
      _isYachtOnlyProduct = value
    End Set
  End Property

  Public Property isHeliOnlyProduct() As Boolean
    Get
      Return _isHeliOnlyProduct
    End Get
    Set(ByVal value As Boolean)
      _isHeliOnlyProduct = value
    End Set
  End Property

  Public Property isCommercialOnlyProduct() As Boolean
    Get
      Return _isCommercialOnlyProduct
    End Get
    Set(ByVal value As Boolean)
      _isCommercialOnlyProduct = value
    End Set
  End Property

  Public Property isBusinessOnlyProduct() As Boolean
    Get
      Return _isBusinessOnlyProduct
    End Get
    Set(ByVal value As Boolean)
      _isBusinessOnlyProduct = value
    End Set
  End Property

  Public Property ShowNotes() As Boolean
    Get
      Return _ShowNotes
    End Get
    Set(ByVal value As Boolean)
      _ShowNotes = value
    End Set
  End Property

  Public Property ShowReminders() As Boolean
    Get
      Return _ShowReminders
    End Get
    Set(ByVal value As Boolean)
      _ShowReminders = value
    End Set
  End Property

  Public Property ShowNoteOnACList() As Boolean
    Get
      Return _ShowNoteOnACList
    End Get
    Set(ByVal value As Boolean)
      _ShowNoteOnACList = value
    End Set
  End Property '

  Public Property UserDatabaseConn() As String
    Get
      Return _UserDatabaseConn
    End Get
    Set(ByVal value As String)
      _UserDatabaseConn = value
    End Set
  End Property

  Public Property STARDatabaseConn() As String
    Get
      Return _STARDatabaseConn
    End Get
    Set(ByVal value As String)
      _STARDatabaseConn = value
    End Set
  End Property

  Public Property UserContactID() As Long
    Get
      Return _UserContactID
    End Get
    Set(ByVal value As Long)
      _UserContactID = value
    End Set
  End Property

  Public Property UserCompanyID() As Long
    Get
      Return _UserCompanyID
    End Get
    Set(ByVal value As Long)
      _UserCompanyID = value
    End Set
  End Property

  Public Property DefaultHomeView() As Long
    Get
      Return _DefaultHomeView
    End Get
    Set(ByVal value As Long)
      _DefaultHomeView = value
    End Set
  End Property

  Public Property DefaultModel() As Long
    Get
      Return _DefaultModelID
    End Get
    Set(ByVal value As Long)
      _DefaultModelID = value
    End Set
  End Property

  Public Property ServerNotesDatabaseConn() As String
    Get
      Return _ServerNotesDatabaseConn
    End Get
    Set(ByVal value As String)
      _ServerNotesDatabaseConn = value
    End Set
  End Property

  Public Property ServerNotesDatabaseName() As String
    Get
      Return _ServerNotesDatabaseName
    End Get
    Set(ByVal value As String)
      _ServerNotesDatabaseName = value
    End Set
  End Property

  Public Property CloudNotesDatabaseName() As String
    Get
      Return _CloudNotesDatabaseName
    End Get
    Set(ByVal value As String)
      _CloudNotesDatabaseName = value
    End Set
  End Property

  Public Property DefaultCompType() As String
    Get
      Return _DefaultCompanyType
    End Get
    Set(ByVal value As String)
      _DefaultCompanyType = value
    End Set
  End Property

  Public Property CompType() As String
    Get
      Return _CompanyType
    End Get
    Set(ByVal value As String)
      _CompanyType = value
    End Set
  End Property

  Public Property PageBackground() As String
    Get
      Return _PageBackground
    End Get
    Set(ByVal value As String)
      _PageBackground = value
    End Set
  End Property

  Public Property AppUserName() As String
    Get
      Return _AppUserName
    End Get
    Set(ByVal value As String)
      _AppUserName = value
    End Set
  End Property

  Public Property UserPageSize() As Long
    Get
      Return _UserPageSize
    End Get
    Set(ByVal value As Long)
      _UserPageSize = value
    End Set
  End Property

  Public Property SmsPhoneNumber() As String
    Get
      Return _SmsPhoneNumber
    End Get
    Set(ByVal value As String)
      _SmsPhoneNumber = value
    End Set
  End Property

  Public Property SmsProviderName() As String
    Get
      Return _SmsProviderName
    End Get
    Set(ByVal value As String)
      _SmsProviderName = value
    End Set
  End Property

  Public Property SmsProviderID() As Long
    Get
      Return _SmsProviderID
    End Get
    Set(ByVal value As Long)
      _SmsProviderID = value
    End Set
  End Property

  Public Property SmsSelectedEvents() As String
    Get
      Return _SmsSelectedEvents
    End Get
    Set(ByVal value As String)
      _SmsSelectedEvents = value
    End Set
  End Property

  Public Property SmsSelectedModels() As String
    Get
      Return _SmsSelectedModels
    End Get
    Set(ByVal value As String)
      _SmsSelectedModels = value
    End Set
  End Property

  Public Property UserEmailReplyToName() As String
    Get
      Return _UserEmailReplyToName
    End Get
    Set(ByVal value As String)
      _UserEmailReplyToName = value
    End Set
  End Property

  Public Property UserEmailReplyToAddress() As String
    Get
      Return _UserEmailReplyToAddress
    End Get
    Set(ByVal value As String)
      _UserEmailReplyToAddress = value
    End Set
  End Property

  Public Property UserEmailDefaultFormat() As String
    Get
      Return _UserEmailDefaultFormat
    End Get
    Set(ByVal value As String)
      _UserEmailDefaultFormat = value
    End Set
  End Property

  Public Property UserBrowserType() As String
    Get
      Return _userBrowserType
    End Get
    Set(ByVal value As String)
      _userBrowserType = value
    End Set
  End Property

  Public Property UseStandardOrMetric() As String
    Get
      Return _UseStandardOrMetric
    End Get
    Set(ByVal value As String)
      _UseStandardOrMetric = value
    End Set
  End Property

  Public Property UserDefaultModelList() As String
    Get
      Return _userDefaultModelList
    End Get
    Set(ByVal value As String)
      _userDefaultModelList = value
    End Set
  End Property

  Public Property UseMetricValues() As Boolean
    Get
      Return _UseMetricValue
    End Get
    Set(ByVal value As Boolean)
      _UseMetricValue = value
    End Set
  End Property

  Public Property UseStatuteMile() As Boolean
    Get
      Return _UseStatuteMile
    End Get
    Set(ByVal value As Boolean)
      _UseStatuteMile = value
    End Set
  End Property

  Public Property DefaultCurrency() As Integer  '
    Get
      Return _DefaultCurrency
    End Get
    Set(ByVal value As Integer)
      _DefaultCurrency = value
    End Set
  End Property

  Public Property CurrencyExchangeRate() As Double
    Get
      Return _CurrencyExchangeRate
    End Get
    Set(ByVal value As Double)
      _CurrencyExchangeRate = value
    End Set
  End Property

  Public Property ChatEnabled() As Boolean
    Get
      Return _UserChatEnabled
    End Get
    Set(ByVal value As Boolean)
      _UserChatEnabled = value
    End Set
  End Property

  Public Property ShareByCompanyFlag() As Boolean
    Get
      Return _ShareByCompanyFlag
    End Get
    Set(ByVal value As Boolean)
      _ShareByCompanyFlag = value
    End Set
  End Property

  Public Property ShareByParentSubFlag() As Boolean
    Get
      Return _ShareByParentSubFlag
    End Get
    Set(ByVal value As Boolean)
      _ShareByParentSubFlag = value
    End Set
  End Property

  Public Property UserAdminFlag() As Boolean
    Get
      Return _UserAdminFlag
    End Get
    Set(ByVal value As Boolean)
      _UserAdminFlag = value
    End Set
  End Property

  Public Property MaxAllowedCustomExport() As Integer
    Get
      Return _MaxAllowedCustomExport
    End Get
    Set(ByVal value As Integer)
      _MaxAllowedCustomExport = value
    End Set
  End Property

  Public Property BusinessSegment() As String
    Get
      Return _businessSegment
    End Get
    Set(ByVal value As String)
      _businessSegment = value
    End Set
  End Property

  Public Property DefaultAnalysisMonths() As Integer
    Get
      Return _defaultAnalysisMonths
    End Get
    Set(ByVal value As Integer)
      _defaultAnalysisMonths = value
    End Set
  End Property

  Public Property AerodexStandard() As Boolean
    Get
      Return _AerodexStandard
    End Get
    Set(ByVal value As Boolean)
      _AerodexStandard = value
    End Set
  End Property

  Public Property AerodexElite() As Boolean
    Get
      Return _AerodexElite
    End Get
    Set(ByVal value As Boolean)
      _AerodexElite = value
    End Set
  End Property

  Public Property ShowListingsOnGlobal() As Boolean
    Get
      Return _ShowListingsOnGlobal
    End Get
    Set(ByVal value As Boolean)
      _ShowListingsOnGlobal = value
    End Set
  End Property

  Public Property HasGlobalRecord() As Boolean
    Get
      Return _HasGlobalRecord
    End Get
    Set(ByVal value As Boolean)
      _HasGlobalRecord = value
    End Set
  End Property

  Public Function loadUserSession(ByRef outError As String, ByVal l_subscriptionID As Long, ByVal s_userID As String,
                                  ByVal l_sequence_no As Long, ByVal l_user_contactID As Long) As Boolean


    Dim bResult As Boolean = True

    Dim strDatabaseHost As String = ""
    Dim strServerName As String = ""
    Dim strDatabase As String = ""
    Dim strUserID As String = ""
    Dim strPassWD As String = ""
    Dim continue_MPM As Boolean = False
    Dim results_table As DataTable = Nothing

    Dim strProductList As String = ""
    Dim sProductList() As eProductCodeTypes = Nothing

    Try

      If String.IsNullOrEmpty(HttpContext.Current.Session.Item("localPreferences").SessionGUID) Then

        results_table = getSessionSubscriptionInfo()

        If Not IsNothing(results_table) Then

          If results_table.Rows.Count > 0 Then

            For Each r As DataRow In results_table.Rows

              HttpContext.Current.Session.Item("localPreferences").SessionGUID = r.Item("subins_session_guid").ToString.Trim

              HttpContext.Current.Session.Item("localPreferences").SeqNo = CInt(r.Item("subins_seq_no").ToString)
              HttpContext.Current.Session.Item("localPreferences").SubID = CLng(r.Item("sub_id").ToString)
              HttpContext.Current.Session.Item("localPreferences").UserID = r.Item("subins_login").ToString.Trim

              HttpContext.Current.Session.Item("localPreferences").Login = HttpContext.Current.Session.Item("localPreferences").UserID.trim + "-" + r.Item("sublogin_password").ToString.Trim

              HttpContext.Current.Session.Item("localPreferences").UserContactID = CLng(r.Item("subins_contact_id").ToString)
              HttpContext.Current.Session.Item("localPreferences").UserCompanyID = CLng(r.Item("sub_comp_id").ToString)

              HttpContext.Current.Session.Item("localPreferences").crmUserID = CLng(r.Item("sub_server_side_crm_regid").ToString)

              HttpContext.Current.Session.Item("localPreferences").LogonType = eLogonTypes.DATABASE
              HttpContext.Current.Session.Item("localPreferences").DatalayerType = eDatalayerTypes.MSSQL

              If Not (IsDBNull(r.Item("serfreqan_sqlserver_name"))) Then
                strServerName = r.Item("serfreqan_sqlserver_name").ToString.Trim
              Else
                If CBool(My.Settings.useBackupSQL_SRV.ToString) Then
                  strServerName = My.Settings.BKUP_SQL_SRV_NAME
                Else
                  strServerName = My.Settings.LIVE_SQL_SRV_NAME
                End If
              End If

              If Not (IsDBNull(r.Item("serfreqan_database_name"))) Then
                strDatabase = r.Item("serfreqan_database_name").ToString.Trim
              Else
                strDatabase = HttpContext.Current.Application.Item("masterDatabase").ToString
              End If

              If Not (IsDBNull(r.Item("serfreqan_user_id"))) Then
                strUserID = r.Item("serfreqan_user_id").ToString.Trim
              Else
                strUserID = "sa"
              End If

              If Not (IsDBNull(r.Item("serfreqan_password"))) Then
                strPassWD = r.Item("serfreqan_password").ToString.Trim
              Else
                strPassWD = "krw32n89"
              End If

              If CType(HttpContext.Current.Session.Item("jetnetWebHostType"), crmWebClient.eWebHostTypes) <> eWebHostTypes.HOMEBASE Then
                HttpContext.Current.Session.Item("localPreferences").UserDatabaseConn = crmWebHostClass.generateMSSQLConnectionString(strServerName, strDatabase, strUserID, strPassWD)
              Else
                HttpContext.Current.Session.Item("localPreferences").UserDatabaseConn = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
              End If

              HttpContext.Current.Session.Item("localPreferences").STARDatabaseConn = crmWebHostClass.generateMSSQLConnectionString(strServerName, HttpContext.Current.Application.Item("starDatabase"), "evolution", "vbs73az8")

              If HttpContext.Current.Session.Item("jetnetWebSiteType") <> eWebSiteTypes.LOCAL Then

                Select Case (strDatabase.Trim.ToLower)

                  Case HttpContext.Current.Application.Item("masterDatabase").ToString
                    HttpContext.Current.Session.Item("localPreferences").DatabaseType = eDatabaseTypes.LIVE
                  Case HttpContext.Current.Application.Item("weeklyDatabase").ToString
                    HttpContext.Current.Session.Item("localPreferences").DatabaseType = eDatabaseTypes.WEEKLY
                  Case HttpContext.Current.Application.Item("biweeklyDatabase").ToString
                    HttpContext.Current.Session.Item("localPreferences").DatabaseType = eDatabaseTypes.BIWEEKLY
                  Case HttpContext.Current.Application.Item("monthlyDatabase").ToString
                    HttpContext.Current.Session.Item("localPreferences").DatabaseType = eDatabaseTypes.MONTHLY
                  Case HttpContext.Current.Application.Item("starDatabase").ToString
                    HttpContext.Current.Session.Item("localPreferences").DatabaseType = eDatabaseTypes.STAR
                  Case HttpContext.Current.Application.Item("testDatabase").ToString
                    HttpContext.Current.Session.Item("localPreferences").DatabaseType = eDatabaseTypes.TEST
                  Case Else
                    HttpContext.Current.Session.Item("localPreferences").DatabaseType = eDatabaseTypes.LIVE

                End Select

              Else

                Select Case (strDatabase.Trim.ToLower)

                  Case HttpContext.Current.Application.Item("masterDatabase").ToString
                    HttpContext.Current.Session.Item("localPreferences").DatabaseType = eDatabaseTypes.LIVE
                  Case HttpContext.Current.Application.Item("weeklyDatabase").ToString
                    HttpContext.Current.Session.Item("localPreferences").DatabaseType = eDatabaseTypes.WEEKLY
                  Case HttpContext.Current.Application.Item("biweeklyDatabase").ToString
                    HttpContext.Current.Session.Item("localPreferences").DatabaseType = eDatabaseTypes.BIWEEKLY
                  Case HttpContext.Current.Application.Item("monthlyDatabase").ToString
                    HttpContext.Current.Session.Item("localPreferences").DatabaseType = eDatabaseTypes.MONTHLY
                  Case HttpContext.Current.Application.Item("starDatabase").ToString
                    HttpContext.Current.Session.Item("localPreferences").DatabaseType = eDatabaseTypes.STAR
                  Case HttpContext.Current.Application.Item("testDatabase").ToString
                    HttpContext.Current.Session.Item("localPreferences").DatabaseType = eDatabaseTypes.TEST
                  Case Else
                    HttpContext.Current.Session.Item("localPreferences").DatabaseType = eDatabaseTypes.LOCAL

                End Select

              End If

              ' session timestamp items
              If Not (IsDBNull(r.Item("subins_last_login_date"))) Then
                HttpContext.Current.Session.Item("localPreferences").LastLoginDate = CDate(r.Item("subins_last_login_date").ToString)
              End If

              If Not (IsDBNull(r.Item("subins_last_session_date"))) Then
                HttpContext.Current.Session.Item("localPreferences").LastSessionDate = CDate(r.Item("subins_last_session_date").ToString)
              End If

              If Not (IsDBNull(r.Item("subins_last_logout_date"))) Then
                HttpContext.Current.Session.Item("localPreferences").LastLogoutDate = CDate(r.Item("subins_last_logout_date").ToString)
              End If

              ' used to overide aircraft notes
              HttpContext.Current.Session.Item("localPreferences").EnableNotesFlag = IIf(r.Item("sublogin_allow_local_notes_flag").ToString.ToLower.Trim = "y", True, False)

              If HttpContext.Current.Session.Item("localPreferences").EnableNotesFlag Then

                HttpContext.Current.Session.Item("localPreferences").HasServerNotes = IIf(r.Item("sub_server_side_notes_flag").ToString.ToLower.Trim = "y", True, False)
                HttpContext.Current.Session.Item("localPreferences").HasCloudNotes = IIf(r.Item("sub_cloud_notes_flag").ToString.ToLower.Trim = "y", True, False)
                HttpContext.Current.Session.Item("localPreferences").CloudNotesDatabaseName = r.Item("sub_cloud_notes_database").ToString.Trim

                HttpContext.Current.Session.Item("localPreferences").ShowNotes = False
                HttpContext.Current.Session.Item("localPreferences").ShowNoteOnACList = IIf(r.Item("subins_display_note_tag_on_aclist_flag").ToString.ToLower.Trim = "y", True, False)

                If HttpContext.Current.Session.Item("localPreferences").HasServerNotes Then

                  If HttpContext.Current.Session.Item("localUser").crmUser_CRM_Database_Not_Available = True Then 'if this gets set, the connection to the client register master has previously thrown an exception. So we're going to stop

                    HttpContext.Current.Session.Item("localPreferences").HasServerNotes = False 'connecting and set the notes connections to false.
                    HttpContext.Current.Session.Item("localSubscription").crmServerSideNotes_Flag = False

                  Else
                    strServerName = ""
                    strDatabase = ""
                    strUserID = ""
                    strPassWD = ""

                    HttpContext.Current.Session.Item("localPreferences").ShowNotes = True

                    ' look up client database info from CRM client_register_master table

                    Dim SqlConn As New SqlClient.SqlConnection
                    Dim SqlCommand As New SqlClient.SqlCommand
                    Dim SqlReader As SqlClient.SqlDataReader
                    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

                    Dim sQuery As String = "SELECT * FROM client_register_master WHERE client_regID = " + HttpContext.Current.Session.Item("localPreferences").crmUserID.ToString

                    Try

                      If CBool(HttpContext.Current.Application.Item("crmClientSiteData").crmClientStandAloneMode.ToString) Then
                        ' if crm runs in standalone use local crm default master database connection
                        SqlConn.ConnectionString = My.Settings.DEFAULT_LOCAL_MSSQL.ToString
                      Else
                        If CBool(My.Settings.IsDebugMode) Then

                          ' if crm runs NOT in standalone but is in debug mode use debug crm default master database connection
                          If CBool(My.Settings.useBackupSQL_SRV.ToString) Then
                            SqlConn.ConnectionString = My.Settings.TEST_CRM_CENTRAL_BK.ToString
                          Else
                            SqlConn.ConnectionString = My.Settings.TEST_CRM_CENTRAL.ToString
                          End If

                        Else

                          If HttpContext.Current.Session.Item("jetnetWebSiteType") <> eWebSiteTypes.LOCAL Then
                            If HttpContext.Current.Session.Item("jetnetFullHostName").ToString.ToUpper.Contains("JETNET14") Then
                              ' if crm runs NOT in standalone but is NOT in debug mode and we are running on "JETNET14" use debug crm default master database connection
                              If CBool(My.Settings.useBackupSQL_SRV.ToString) Then
                                SqlConn.ConnectionString = My.Settings.TEST_CRM_CENTRAL_BK.ToString
                              Else
                                SqlConn.ConnectionString = My.Settings.TEST_CRM_CENTRAL.ToString
                              End If
                            Else
                              ' if crm runs NOT in standalone but NOT not in debug mode use crm default master database connection
                              If CBool(My.Settings.useBackupSQL_SRV.ToString) Then
                                SqlConn.ConnectionString = My.Settings.LIVE_CRM_CENTRAL_BK.ToString
                              Else
                                SqlConn.ConnectionString = My.Settings.LIVE_CRM_CENTRAL.ToString
                              End If
                            End If
                          Else
                            ' if crm runs NOT in standalone but is NOT in debug mode BUT is LOCAL SITE use debug crm default master database connection
                            If CBool(My.Settings.useBackupSQL_SRV.ToString) Then
                              SqlConn.ConnectionString = My.Settings.TEST_CRM_CENTRAL_BK.ToString
                            Else
                              SqlConn.ConnectionString = My.Settings.TEST_CRM_CENTRAL.ToString
                            End If
                          End If

                        End If
                      End If

                      'If String.IsNullOrEmpty(HttpContext.Current.Application.Item("crmMasterDatabase").ToString.Trim) Then
                      'HttpContext.Current.Application.Item("crmMasterDatabase") = SqlConn.ConnectionString.ToString
                      'End If

                      If (HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Or HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL) Then
                        commonLogFunctions.forceLogError("CONNECT", "CRM Central Connection Used: client_register_master ")
                      End If

                      SqlConn.Open()

                      SqlCommand.Connection = SqlConn
                      SqlCommand.CommandType = CommandType.Text
                      SqlCommand.CommandTimeout = 30


                      SqlCommand.CommandText = sQuery
                      SqlReader = SqlCommand.ExecuteReader()

                      If SqlReader.HasRows Then

                        SqlReader.Read()

                        If Not (IsDBNull(SqlReader("client_dbHost"))) Then
                          strDatabaseHost = SqlReader.Item("client_dbHost")
                        End If

                        If Not (IsDBNull(SqlReader("client_dbDatabase"))) Then
                          strDatabase = SqlReader.Item("client_dbDatabase")
                        End If

                        If Not (IsDBNull(SqlReader("client_dbUID"))) Then
                          strUserID = SqlReader.Item("client_dbUID")
                        End If

                        If Not (IsDBNull(SqlReader("client_dbPWD"))) Then
                          strPassWD = SqlReader.Item("client_dbPWD")
                        End If

                        If Not (IsDBNull(SqlReader("client_regCustomer_Type"))) Then
                          If Trim(SqlReader("client_regCustomer_Type")) <> "SERVERDB" Then
                            continue_MPM = True
                          Else
                            HttpContext.Current.Session.Item("localUser").crmUser_Evo_MPM_Flag = False
                          End If
                        Else
                          HttpContext.Current.Session.Item("localUser").crmUser_Evo_MPM_Flag = False
                        End If

                        SqlReader.Close()

                      End If 'MySqlReader.HasRows 

                      SqlReader.Dispose()

                    Catch SqlException
                      commonLogFunctions.forceLogError("ERROR", System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim + " CRM Central User Exception Thrown[" + SqlException.Message.Trim + "]")
                      HttpContext.Current.Session.Item("localUser").crmUser_CRM_Database_Not_Available = True
                      SqlConn.Dispose()
                      SqlCommand.Dispose()


                    Finally

                      SqlConn.Close()
                      SqlCommand.Dispose()
                      SqlConn.Dispose()

                    End Try

                    If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Then
                      If strDatabaseHost.Trim.Contains("172.30.5.47") Then
                        strDatabaseHost = "jetnetcrm2.jetnet.com" ' or 192.69.4.165
                      Else
                        strDatabaseHost = "jetnetcrm.jetnet.com" ' or 192.69.4.159
                      End If
                    End If

                    ' generate connection string for crm server notes connection
                    HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseName = r.Item("sub_server_side_dbase_name").ToString.Trim
                    HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseConn = crmWebHostClass.generateMYSQLConnectionString(strDatabaseHost, strDatabase, strUserID, strPassWD)

                    ' verify server notes connection, "turn off" server notes if connection fails ...
                    If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseConn.ToString.Trim) And HttpContext.Current.Session.Item("localUser").crmUser_CRM_Database_Not_Available = False Then

                      Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
                      Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
                      Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader
                      Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

                      Try

                        If (HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Or HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL) Then
                          commonLogFunctions.forceLogError("CONNECT", "CRM Central Connection Used: client_user ")
                        End If


                        MySqlConn = Nothing
                        MySqlConn = New MySql.Data.MySqlClient.MySqlConnection

                        MySqlConn.ConnectionString = HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseConn.ToString.Trim
                        MySqlConn.Open()

                        If MySqlConn.State = ConnectionState.Open Then

                          ' ADDED IN MSW - 2/22/18
                          If continue_MPM Then
                            sQuery = "SELECT * FROM client_user WHERE cliuser_login = '" & HttpContext.Current.Session.Item("localUser").crmLocalUserName.ToString.Trim & "' and  cliuser_password <> '' and cliuser_active_flag='Y' "

                            MySqlCommand.Connection = MySqlConn
                            MySqlCommand.CommandType = CommandType.Text
                            MySqlCommand.CommandTimeout = 30

                            MySqlCommand.CommandText = sQuery
                            MySqlReader = MySqlCommand.ExecuteReader()

                            If MySqlReader.HasRows Then

                              MySqlReader.Read()
                              HttpContext.Current.Session.Item("localUser").crmUser_Evo_MPM_Flag = True
                            End If

                            MySqlReader.Close()

                          End If

                        Else
                          HttpContext.Current.Session.Item("localPreferences").HasServerNotes = False
                        End If

                      Catch MySqlException
                        commonLogFunctions.forceLogError("ERROR", System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim + " CRM Central User Exception Thrown[" + MySqlException.Message.Trim + "]")

                        MySqlConn.Dispose()
                        HttpContext.Current.Session.Item("localPreferences").HasServerNotes = False

                      Finally


                        MySqlConn.Close()
                        MySqlCommand.Dispose()
                        MySqlConn.Dispose()

                      End Try

                    End If
                  End If
                Else

                  If String.IsNullOrEmpty(HttpContext.Current.Application.Item("crmMasterDatabase").ToString.Trim) Then

                    If CBool(HttpContext.Current.Application.Item("crmClientSiteData").crmClientStandAloneMode.ToString) Then
                      ' if crm runs in standalone use local crm default master database connection
                      HttpContext.Current.Application.Item("crmMasterDatabase") = My.Settings.DEFAULT_LOCAL_MYSQL.ToString
                    Else
                      If CBool(My.Settings.IsDebugMode) Then
                        ' if crm runs NOT in standalone but is in debug mode use debug crm default master database connection
                        HttpContext.Current.Application.Item("crmMasterDatabase") = My.Settings.DEFAULT_LIVE_MYSQL2_DEBUG.ToString
                      Else

                        If HttpContext.Current.Session.Item("jetnetWebSiteType") <> eWebSiteTypes.LOCAL Then
                          If HttpContext.Current.Session.Item("jetnetFullHostName").ToString.ToUpper.Contains("JETNET14") Then
                            ' if crm runs NOT in standalone but is NOT in debug mode and we are running on "JETNET14" use debug crm default master database connection
                            HttpContext.Current.Application.Item("crmMasterDatabase") = My.Settings.DEFAULT_LIVE_MYSQL2_DEBUG.ToString
                          Else
                            ' if crm runs NOT in standalone but NOT not in debug mode use crm default master database connection
                            HttpContext.Current.Application.Item("crmMasterDatabase") = My.Settings.DEFAULT_LIVE_MYSQL2.ToString
                          End If
                        Else
                          ' if crm runs NOT in standalone but is NOT in debug mode BUT is LOCAL SITE use debug crm default master database connection
                          HttpContext.Current.Application.Item("crmMasterDatabase") = My.Settings.DEFAULT_LIVE_MYSQL2_DEBUG.ToString
                        End If

                      End If
                    End If

                  End If
                End If

              Else

                If String.IsNullOrEmpty(HttpContext.Current.Application.Item("crmMasterDatabase").ToString.Trim) Then

                  If CBool(HttpContext.Current.Application.Item("crmClientSiteData").crmClientStandAloneMode.ToString) Then
                    ' if crm runs in standalone use local crm default master database connection
                    HttpContext.Current.Application.Item("crmMasterDatabase") = My.Settings.DEFAULT_LOCAL_MYSQL.ToString
                  Else
                    If CBool(My.Settings.IsDebugMode) Then
                      ' if crm runs NOT in standalone but is in debug mode use debug crm default master database connection
                      HttpContext.Current.Application.Item("crmMasterDatabase") = My.Settings.DEFAULT_LIVE_MYSQL2_DEBUG.ToString
                    Else

                      If HttpContext.Current.Session.Item("jetnetWebSiteType") <> eWebSiteTypes.LOCAL Then
                        If HttpContext.Current.Session.Item("jetnetFullHostName").ToString.ToUpper.Contains("JETNET14") Then
                          ' if crm runs NOT in standalone but is NOT in debug mode and we are running on "JETNET14" use debug crm default master database connection
                          HttpContext.Current.Application.Item("crmMasterDatabase") = My.Settings.DEFAULT_LIVE_MYSQL2_DEBUG.ToString
                        Else
                          ' if crm runs NOT in standalone but NOT not in debug mode use crm default master database connection
                          HttpContext.Current.Application.Item("crmMasterDatabase") = My.Settings.DEFAULT_LIVE_MYSQL2.ToString
                        End If
                      Else
                        ' if crm runs NOT in standalone but is NOT in debug mode BUT is LOCAL SITE use debug crm default master database connection
                        HttpContext.Current.Application.Item("crmMasterDatabase") = My.Settings.DEFAULT_LIVE_MYSQL2_DEBUG.ToString
                      End If

                    End If
                  End If
                End If

              End If

              If CType(HttpContext.Current.Session.Item("jetnetWebHostType"), crmWebClient.eWebHostTypes) = eWebHostTypes.CRM Then
                If String.IsNullOrEmpty(HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseConn.ToString.Trim) Then
                  HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseConn = HttpContext.Current.Application.Item("crmClientDatabase")
                End If
              End If


              ' fill in user product flags
              HttpContext.Current.Session.Item("localPreferences").UserHelicopterFlag = IIf(r.Item("sub_helicopters_flag").ToString.ToLower.Trim = "y", True, False)
              HttpContext.Current.Session.Item("localPreferences").UserBusinessFlag = IIf(r.Item("sub_business_aircraft_flag").ToString.ToLower.Trim = "y", True, False)
              HttpContext.Current.Session.Item("localPreferences").UserCommercialFlag = IIf(r.Item("sub_commerical_flag").ToString.ToLower.Trim = "y", True, False)
              HttpContext.Current.Session.Item("localPreferences").UserRegionalFlag = IIf(r.Item("sub_regional_flag").ToString.ToLower.Trim = "y", True, False)
              'HttpContext.Current.Session.Item("localPreferences").UserAirBPFlag = IIf(r.Item("sub_airbp_flag").ToString.ToLower.Trim = "y", True, False)
              HttpContext.Current.Session.Item("localPreferences").UserABIFlag = IIf(r.Item("sub_abi_flag").ToString.ToLower.Trim = "y", True, False)
              HttpContext.Current.Session.Item("localPreferences").UserStarRptFlag = IIf(r.Item("sub_starreports_flag").ToString.ToLower.Trim = "y", True, False)
              HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = IIf(r.Item("sublogin_values_flag").ToString.ToLower.Trim = "y", True, False)
              HttpContext.Current.Session.Item("localPreferences").UserYachtFlag = IIf(r.Item("sub_yacht_flag").ToString.ToLower.Trim = "y", True, False)

              HttpContext.Current.Session.Item("localPreferences").isHeliOnlyProduct = HttpContext.Current.Session.Item("localPreferences").UserHelicopterFlag And
                                                                                Not (HttpContext.Current.Session.Item("localPreferences").UserBusinessFlag Or
                                                                                HttpContext.Current.Session.Item("localPreferences").UserCommercialFlag Or
                                                                                HttpContext.Current.Session.Item("localPreferences").UserRegionalFlag Or
                                                                                HttpContext.Current.Session.Item("localPreferences").UserYachtFlag)

              HttpContext.Current.Session.Item("localPreferences").isCommercialOnlyProduct = HttpContext.Current.Session.Item("localPreferences").UserCommercialFlag And
                                                                      Not (HttpContext.Current.Session.Item("localPreferences").UserBusinessFlag Or
                                                                      HttpContext.Current.Session.Item("localPreferences").UserHelicopterFlag Or
                                                                      HttpContext.Current.Session.Item("localPreferences").UserRegionalFlag Or
                                                                      HttpContext.Current.Session.Item("localPreferences").UserYachtFlag)

              HttpContext.Current.Session.Item("localPreferences").isBusinessOnlyProduct = HttpContext.Current.Session.Item("localPreferences").UserBusinessFlag And
                                                                      Not (HttpContext.Current.Session.Item("localPreferences").UserHelicopterFlag Or
                                                                      HttpContext.Current.Session.Item("localPreferences").UserCommercialFlag Or
                                                                      HttpContext.Current.Session.Item("localPreferences").UserRegionalFlag Or
                                                                      HttpContext.Current.Session.Item("localPreferences").UserYachtFlag)

              HttpContext.Current.Session.Item("localPreferences").isYachtOnlyProduct = HttpContext.Current.Session.Item("localPreferences").UserYachtFlag And
                                                        Not (HttpContext.Current.Session.Item("localPreferences").UserHelicopterFlag Or
                                                        HttpContext.Current.Session.Item("localPreferences").UserCommercialFlag Or
                                                        HttpContext.Current.Session.Item("localPreferences").UserRegionalFlag Or
                                                        HttpContext.Current.Session.Item("localPreferences").UserBusinessFlag)

              If CType(HttpContext.Current.Session.Item("jetnetWebHostType"), crmWebClient.eWebHostTypes) <> eWebHostTypes.HOMEBASE Then

                If HttpContext.Current.Session.Item("localPreferences").UserHelicopterFlag Then
                  If String.IsNullOrEmpty(strProductList) Then
                    strProductList = Constants.PRODUCT_TYPE_H
                  Else
                    strProductList = strProductList + Constants.cCommaDelim + Constants.PRODUCT_TYPE_H
                  End If
                End If

                If HttpContext.Current.Session.Item("localPreferences").UserBusinessFlag Then
                  If String.IsNullOrEmpty(strProductList) Then
                    strProductList = Constants.PRODUCT_TYPE_B
                  Else
                    strProductList = strProductList + Constants.cCommaDelim + Constants.PRODUCT_TYPE_B
                  End If
                End If

                If HttpContext.Current.Session.Item("localPreferences").UserCommercialFlag Then
                  If String.IsNullOrEmpty(strProductList) Then
                    strProductList = Constants.PRODUCT_TYPE_C
                  Else
                    strProductList = strProductList + Constants.cCommaDelim + Constants.PRODUCT_TYPE_C
                  End If
                End If

                If HttpContext.Current.Session.Item("localPreferences").UserRegionalFlag Then
                  If String.IsNullOrEmpty(strProductList) Then
                    strProductList = Constants.PRODUCT_TYPE_R
                  Else
                    strProductList = strProductList + Constants.cCommaDelim + Constants.PRODUCT_TYPE_R
                  End If
                End If

                If HttpContext.Current.Session.Item("localPreferences").UserYachtFlag Then
                  If String.IsNullOrEmpty(strProductList) Then
                    strProductList = Constants.PRODUCT_TYPE_Y
                  Else
                    strProductList = strProductList + Constants.cCommaDelim + Constants.PRODUCT_TYPE_Y
                  End If
                End If

              Else
                strProductList = Constants.PRODUCT_TYPE_H + Constants.cCommaDelim + Constants.PRODUCT_TYPE_B + Constants.cCommaDelim + Constants.PRODUCT_TYPE_C + Constants.cCommaDelim + Constants.PRODUCT_TYPE_Y
              End If

              Dim tmpProdArray As String() = Split(strProductList, Constants.cCommaDelim)

              ReDim sProductList(UBound(tmpProdArray))

              For z As Integer = 0 To UBound(tmpProdArray)
                Select Case tmpProdArray(z).ToString.ToUpper.Trim
                  Case Constants.PRODUCT_TYPE_B
                    sProductList(z) = eProductCodeTypes.B
                  Case Constants.PRODUCT_TYPE_H
                    sProductList(z) = eProductCodeTypes.H
                  Case Constants.PRODUCT_TYPE_C
                    sProductList(z) = eProductCodeTypes.C
                  Case Constants.PRODUCT_TYPE_R
                    sProductList(z) = eProductCodeTypes.R
                  Case Constants.PRODUCT_TYPE_F
                    sProductList(z) = eProductCodeTypes.F
                  Case Constants.PRODUCT_TYPE_A
                    sProductList(z) = eProductCodeTypes.A
                  Case Constants.PRODUCT_TYPE_P
                    sProductList(z) = eProductCodeTypes.P
                  Case Constants.PRODUCT_TYPE_S
                    sProductList(z) = eProductCodeTypes.S
                  Case Constants.PRODUCT_TYPE_I
                    sProductList(z) = eProductCodeTypes.I
                  Case Constants.PRODUCT_TYPE_Y
                    sProductList(z) = eProductCodeTypes.Y

                End Select
              Next

              HttpContext.Current.Session.Item("localPreferences").ProductCode = sProductList
              tmpProdArray = Nothing
              sProductList = Nothing

              If Not IsDBNull(r.Item("subins_default_amod_id")) Then
                If CLng(r.Item("subins_default_amod_id").ToString) > 0 Then
                  HttpContext.Current.Session.Item("localPreferences").DefaultModel = CLng(r.Item("subins_default_amod_id").ToString)
                Else
                  HttpContext.Current.Session.Item("localPreferences").DefaultModel = -1
                End If
              Else
                HttpContext.Current.Session.Item("localPreferences").DefaultModel = -1
              End If

              If Not IsDBNull(r.Item("subins_default_models")) Then
                HttpContext.Current.Session.Item("localPreferences").UserDefaultModelList = r.Item("subins_default_models").ToString
              End If

              HttpContext.Current.Session.Item("localPreferences").MarketingFlag = IIf(r.Item("sub_marketing_flag").ToString.ToLower.Trim = "y", True, False)
              HttpContext.Current.Session.Item("localPreferences").AerodexFlag = IIf(r.Item("sub_aerodex_flag").ToString.ToLower.Trim = "y", True, False)
              HttpContext.Current.Session.Item("localPreferences").DemoFlag = IIf(r.Item("sublogin_demo_flag").ToString.ToLower.Trim = "y", True, False)
              HttpContext.Current.Session.Item("localPreferences").UserAdminFlag = IIf(r.Item("subins_admin_flag").ToString.ToLower.Trim = "y", True, False)

              If HttpContext.Current.Session.Item("localPreferences").AerodexFlag Then
                If HttpContext.Current.Session.Item("localPreferences").DatabaseType = eDatabaseTypes.WEEKLY Then
                  HttpContext.Current.Session.Item("localPreferences").AerodexStandard = True
                Else
                  HttpContext.Current.Session.Item("localPreferences").AerodexElite = True
                End If
              End If

              HttpContext.Current.Session.Item("localPreferences").ShareByCompanyFlag = IIf(r.Item("sub_share_by_comp_id_flag").ToString.ToLower.Trim = "y", True, False)
              HttpContext.Current.Session.Item("localPreferences").ShareByParentSubFlag = IIf(r.Item("sub_share_by_parent_sub_id_flag").ToString.ToLower.Trim = "y", True, False)

              HttpContext.Current.Session.Item("localPreferences").ExportReportFlag = IIf(r.Item("sublogin_allow_export_flag").ToString.ToLower.Trim = "y", True, False)
              HttpContext.Current.Session.Item("localPreferences").SaveProjectsFlag = IIf(r.Item("sublogin_allow_projects_flag").ToString.ToLower.Trim = "y", True, False)
              HttpContext.Current.Session.Item("localPreferences").EmailRequestFlag = IIf(r.Item("sublogin_allow_email_request_flag").ToString.ToLower.Trim = "y", True, False)
              HttpContext.Current.Session.Item("localPreferences").EventRequestFlag = IIf(r.Item("sublogin_allow_event_request_flag").ToString.ToLower.Trim = "y", True, False)
              HttpContext.Current.Session.Item("localPreferences").EnableTextFlag = IIf(r.Item("sublogin_allow_text_message_flag").ToString.ToLower.Trim = "y", True, False)

              HttpContext.Current.Session.Item("localPreferences").MobleWebStatus = IIf(r.Item("subins_evo_mobile_flag").ToString.ToLower.Trim = "y", True, False)

              Select Case (r.Item("sub_busair_tier_level").ToString)
                Case "1"
                  HttpContext.Current.Session.Item("localPreferences").Tierlevel = eTierLevelTypes.JETS
                Case "2"
                  HttpContext.Current.Session.Item("localPreferences").Tierlevel = eTierLevelTypes.TURBOS
                Case Else
                  HttpContext.Current.Session.Item("localPreferences").Tierlevel = eTierLevelTypes.ALL
              End Select

              HttpContext.Current.Session.Item("localPreferences").UserEmailReplyToName = r.Item("subins_email_replyname").ToString
              HttpContext.Current.Session.Item("localPreferences").UserEmailReplyToAddress = r.Item("subins_email_replyaddress").ToString
              HttpContext.Current.Session.Item("localPreferences").UserEmailDefaultFormat = r.Item("subins_email_default_format").ToString

              HttpContext.Current.Session.Item("localPreferences").SmsPhoneNumber = r.Item("subins_cell_number").ToString

              If HttpContext.Current.Session.Item("localPreferences").EnableTextFlag Then ' only load the values if the flag is true

                If Not (IsDBNull(r.Item("subins_cell_service"))) Then
                  HttpContext.Current.Session.Item("localPreferences").SmsProviderName = r.Item("subins_cell_service").ToString.Trim
                Else
                  HttpContext.Current.Session.Item("localPreferences").SmsProviderName = ""
                End If

                If Not (IsDBNull(r.Item("subins_cell_carrier_id"))) Then
                  HttpContext.Current.Session.Item("localPreferences").SmsProviderID = CLng(r.Item("subins_cell_carrier_id").ToString)
                Else
                  HttpContext.Current.Session.Item("localPreferences").SmsProviderID = 0
                End If

                If Not (IsDBNull(r.Item("subins_sms_events"))) Then
                  HttpContext.Current.Session.Item("localPreferences").SmsSelectedEvents = r.Item("subins_sms_events").ToString.Trim
                Else
                  HttpContext.Current.Session.Item("localPreferences").SmsSelectedEvents = ""
                End If

                If Not (IsDBNull(r.Item("subins_smstxt_models"))) Then
                  HttpContext.Current.Session.Item("localPreferences").SmsSelectedModels = r.Item("subins_smstxt_models").ToString.Trim
                Else
                  HttpContext.Current.Session.Item("localPreferences").SmsSelectedModels = ""
                End If

                If r.Item("subins_smstxt_active_flag").ToString.Trim = Constants.SMS_ACTIVATE_YES Then
                  HttpContext.Current.Session.Item("localPreferences").SMSActiveFlag = True
                Else
                  HttpContext.Current.Session.Item("localPreferences").SMSActiveFlag = False
                End If

                Select Case (r.Item("subins_smstxt_active_flag").ToString.Trim)
                  Case Constants.SMS_ACTIVATE_YES
                    HttpContext.Current.Session.Item("localPreferences").SMSActivationStatus = eSMSActivateTypes.YES
                  Case Constants.SMS_ACTIVATE_NO
                    HttpContext.Current.Session.Item("localPreferences").SMSActivationStatus = eSMSActivateTypes.NO
                  Case Constants.SMS_ACTIVATE_PENDING
                    HttpContext.Current.Session.Item("localPreferences").SMSActivationStatus = eSMSActivateTypes.PENDING
                  Case Constants.SMS_ACTIVATE_WAIT
                    HttpContext.Current.Session.Item("localPreferences").SMSActivationStatus = eSMSActivateTypes.WAIT
                  Case Constants.SMS_ACTIVATE_TEST
                    HttpContext.Current.Session.Item("localPreferences").SMSActivationStatus = eSMSActivateTypes.TEST
                  Case Else
                    HttpContext.Current.Session.Item("localPreferences").SMSActivationStatus = eSMSActivateTypes.NO
                End Select

              End If

              ' addtional subscription values
              HttpContext.Current.Session.Item("localPreferences").SubCompanyName = commonEvo.get_company_name_fromID(CLng(HttpContext.Current.Session.Item("localPreferences").UserCompanyID.ToString), 0, False, True, "")
              HttpContext.Current.Session.Item("localPreferences").DefaultCompType = r.Item("subins_aircraft_tab_relationship_to_ac_default").ToString.Trim

              ' get users default homepage view
              If Not (IsDBNull(r.Item("subins_evoview_id"))) Then
                HttpContext.Current.Session.Item("localPreferences").DefaultHomeView = CLng(r.Item("subins_evoview_id").ToString.Trim)
              Else
                HttpContext.Current.Session.Item("localPreferences").DefaultHomeView = 0
              End If

              ' get users session page size
              HttpContext.Current.Session.Item("localPreferences").UserPageSize = CLng(r.Item("subins_nbr_rec_per_page").ToString.Trim)

              ' get max export / report output
              HttpContext.Current.Session.Item("localPreferences").MaxAllowedCustomExport = CInt(r.Item("sub_max_allowed_custom_export").ToString.Trim)

              ' get users service code
              HttpContext.Current.Session.Item("localPreferences").ServiceCode = r.Item("sub_serv_code").ToString.Trim
              HttpContext.Current.Session.Item("localPreferences").ServiceName = r.Item("serv_name").ToString.Trim

              HttpContext.Current.Session.Item("localPreferences").ChatEnabled = IIf(r.Item("subins_chat_flag").ToString.ToLower.Trim = "y", True, False)

              HttpContext.Current.Session.Item("localPreferences").BusinessSegment = r.Item("subins_business_type_code").ToString.Trim

              If Not (IsDBNull(r.Item("subins_platform_os"))) Then
                HttpContext.Current.Session.Item("localPreferences").OsBrowser = r.Item("subins_platform_os").ToString.Trim
              Else
                HttpContext.Current.Session.Item("localPreferences").OsBrowser = "other  unknown"
              End If

              ' get max export / report output
              HttpContext.Current.Session.Item("localPreferences").DefaultAnalysisMonths = CInt(r.Item("subins_default_analysis_months").ToString.Trim)

            Next

            ' COMMENTED OUT MSW - THIS WILL BE SET DIFFERENTLY IN PREFERENCES AREA NOW 

            '' go get subscribers GLOBAL listing info and set flag ShowListingsOnGlobal
            'Dim atemptable As New DataTable
            'Dim tQuery = New StringBuilder()

            'Dim SqlConn As New SqlClient.SqlConnection
            'Dim SqlCommand As New SqlClient.SqlCommand
            'Dim SqlReader As SqlClient.SqlDataReader
            'Dim SqlException As SqlClient.SqlException : SqlException = Nothing

            'Try

            '  tQuery.Append("SELECT abicserv_id, abicserv_end_date FROM ABI_Company_service WITH(NOLOCK)")
            '  tQuery.Append(" WHERE abicserv_serv_code = 'ACLIST' AND abicserv_status = 'A' AND abicserv_start_date <= GETDATE()")
            '  tQuery.Append(" AND abicserv_comp_id = " + HttpContext.Current.Session.Item("localPreferences").UserCompanyID.ToString)

            '  HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>loadUserSession()</b><br />" + tQuery.ToString

            '  SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            '  SqlConn.Open()
            '  SqlCommand.Connection = SqlConn
            '  SqlCommand.CommandType = CommandType.Text
            '  SqlCommand.CommandTimeout = 60

            '  SqlCommand.CommandText = tQuery.ToString
            '  SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            '  Try
            '    atemptable.Load(SqlReader)
            '  Catch constrExc As System.Data.ConstraintException
            '    Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
            '    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in loadUserSession load datatable" + constrExc.Message
            '  End Try

            '  If atemptable.Rows.Count > 0 Then

            '    HttpContext.Current.Session.Item("localPreferences").HasGlobalRecord = True

            '    If IsDBNull(atemptable.Rows(0).Item("abicserv_end_date")) Then
            '      HttpContext.Current.Session.Item("localPreferences").ShowListingsOnGlobal = True
            '    Else
            '      If Not String.IsNullOrEmpty(atemptable.Rows(0).Item("abicserv_end_date").ToString) Then
            '        HttpContext.Current.Session.Item("localPreferences").ShowListingsOnGlobal = False
            '      End If
            '    End If
            '  Else
            '    HttpContext.Current.Session.Item("localPreferences").ShowListingsOnGlobal = False
            '    HttpContext.Current.Session.Item("localPreferences").HasGlobalRecord = False
            '  End If

            'Catch SqlException

            '  outError = SqlException.Message
            '  bResult = False

            '  HttpContext.Current.Session.Item("localPreferences").SubStatusCode = eObjStatusCode.FAILURE
            '  HttpContext.Current.Session.Item("localPreferences").SubDetailError = eObjDetailErrorCode.SQL
            '  HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in loadUserSession(ByRef outError As String, ByVal l_subscriptionID As Long, ByVal s_userID As String, ByVal l_sequence_no As Long, ByVal l_user_contactID As Long) As Boolean" + SqlException.Message

            'Finally

            '  SqlReader = Nothing

            '  SqlConn.Dispose()
            '  SqlConn.Close()
            '  SqlConn = Nothing

            '  SqlCommand.Dispose()
            '  SqlCommand = Nothing
            '  tQuery = Nothing

            'End Try

            ' ADDED IN MSW - 7/2/18 ------------- fixed by MJM 11/21/18
            If HttpContext.Current.Session.Item("localPreferences").UserABIFlag Then
              HttpContext.Current.Session.Item("localPreferences").ShowListingsOnGlobal = True
            Else
              HttpContext.Current.Session.Item("localPreferences").ShowListingsOnGlobal = False
            End If

            'set the user databases for this session
            HttpContext.Current.Session.Item("jetnetClientDatabase") = HttpContext.Current.Session.Item("localPreferences").UserDatabaseConn.ToString.Trim
            HttpContext.Current.Session.Item("jetnetStarDatabase") = HttpContext.Current.Session.Item("localPreferences").STARDatabaseConn.ToString.Trim
            HttpContext.Current.Session.Item("jetnetServerNotesDatabase") = HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseConn.ToString.Trim

            ' set the session variable
            HttpContext.Current.Session.Item("BusinessSegment") = HttpContext.Current.Session.Item("localPreferences").BusinessSegment.ToString.Trim

            HttpContext.Current.Session.Item("localPreferences").SubStatusCode = eObjStatusCode.SUCCESS
            HttpContext.Current.Session.Item("localPreferences").SubDetailError = eObjDetailErrorCode.NULL

          End If

        End If

      End If

    Catch ex As Exception

      outError = ex.Message
      bResult = False
      HttpContext.Current.Session.Item("localPreferences").SubStatusCode = eObjStatusCode.FAILURE
      HttpContext.Current.Session.Item("localPreferences").SubDetailError = eObjDetailErrorCode.SQL

    Finally

      results_table = Nothing

    End Try

    Return bResult

  End Function

  Public Function DisplayPreferences() As String

    Dim sOutputString = New StringBuilder()

    sOutputString.Append("Session.Item(""localPreferences"").SubStatusCode As eObjStatusCode: " + _SubStatusCode.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").SubDetailError As eObjDetailErrorCode: " + _SubDetailError.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").DatabaseType As eDatabaseTypes: " + dataBaseTypeName(_DatabaseType) + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").DatalayerType As eDatalayerTypes: " + dataLayerTypeName(_DatalayerType) + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").LogonType As eLogonTypes: " + _LogonType.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").UserType As eUserTypes: " + _UserType.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").SessionGUID As String: " + _SessionGUID.Trim + "<br />")

    sOutputString.Append("Session.Item(""localPreferences"").LastLoginDate As Date: " + _LastLoginDate.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").LastSessionDate As Date: " + _LastSessionDate.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").LastLogoutDate As Date: " + _LastLogoutDate.ToString + "<br />")

    sOutputString.Append("Session.Item(""localPreferences"").Login As String: " + _Login.Trim + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").UserID As String: " + _UserID.Trim + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").SeqNo As Long: " + _SeqNo.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").SubID As Long: " + _SubID.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").crmUserID As Long: " + _crmUserID.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").SubCompName As String: " + _SubCompName.Trim + "<br />")

    sOutputString.Append("Session.Item(""localPreferences"").ServiceCode As String: " + _ServiceCode.Trim + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").ServiceName As String: " + _ServiceName.Trim + "<br />")

    Dim tmpProductCodeStr As String = ""

    If IsArray(_ProductCode) And Not IsNothing(_ProductCode) Then

      ' loop through the inUserProductCode and create the Where Clause  
      For nloop = 0 To UBound(_ProductCode)

        Select Case _ProductCode(nloop)
          Case eProductCodeTypes.B
            If String.IsNullOrEmpty(tmpProductCodeStr) Then
              tmpProductCodeStr = Constants.PRODUCT_TYPE_B
            Else
              tmpProductCodeStr += Constants.cCommaDelim + Constants.PRODUCT_TYPE_B
            End If
          Case eProductCodeTypes.H
            If String.IsNullOrEmpty(tmpProductCodeStr) Then
              tmpProductCodeStr = Constants.PRODUCT_TYPE_H
            Else
              tmpProductCodeStr += Constants.cCommaDelim + Constants.PRODUCT_TYPE_H
            End If
          Case eProductCodeTypes.C
            If String.IsNullOrEmpty(tmpProductCodeStr) Then
              tmpProductCodeStr = Constants.PRODUCT_TYPE_C
            Else
              tmpProductCodeStr += Constants.cCommaDelim + Constants.PRODUCT_TYPE_C
            End If
          Case eProductCodeTypes.R
            If String.IsNullOrEmpty(tmpProductCodeStr) Then
              tmpProductCodeStr = Constants.PRODUCT_TYPE_R
            Else
              tmpProductCodeStr += Constants.cCommaDelim + Constants.PRODUCT_TYPE_R
            End If
          Case eProductCodeTypes.F
            If String.IsNullOrEmpty(tmpProductCodeStr) Then
              tmpProductCodeStr = Constants.PRODUCT_TYPE_F
            Else
              tmpProductCodeStr += Constants.cCommaDelim + Constants.PRODUCT_TYPE_F
            End If
          Case eProductCodeTypes.A
            If String.IsNullOrEmpty(tmpProductCodeStr) Then
              tmpProductCodeStr = Constants.PRODUCT_TYPE_A
            Else
              tmpProductCodeStr += Constants.cCommaDelim + Constants.PRODUCT_TYPE_A
            End If
          Case eProductCodeTypes.P
            If String.IsNullOrEmpty(tmpProductCodeStr) Then
              tmpProductCodeStr = Constants.PRODUCT_TYPE_P
            Else
              tmpProductCodeStr += Constants.cCommaDelim + Constants.PRODUCT_TYPE_P
            End If
          Case eProductCodeTypes.S
            If String.IsNullOrEmpty(tmpProductCodeStr) Then
              tmpProductCodeStr = Constants.PRODUCT_TYPE_S
            Else
              tmpProductCodeStr += Constants.cCommaDelim + Constants.PRODUCT_TYPE_S
            End If
          Case eProductCodeTypes.I
            If String.IsNullOrEmpty(tmpProductCodeStr) Then
              tmpProductCodeStr = Constants.PRODUCT_TYPE_I
            Else
              tmpProductCodeStr += Constants.cCommaDelim + Constants.PRODUCT_TYPE_I
            End If
          Case eProductCodeTypes.Y
            If String.IsNullOrEmpty(tmpProductCodeStr) Then
              tmpProductCodeStr = Constants.PRODUCT_TYPE_Y
            Else
              tmpProductCodeStr += Constants.cCommaDelim + Constants.PRODUCT_TYPE_Y
            End If
        End Select
      Next
    End If

    sOutputString.Append("Session.Item(""localPreferences"").ProductCode() As eProductCodeTypes: " + tmpProductCodeStr.Trim + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").ProductName As String: " + _ProductName.Trim + "<br />")

    sOutputString.Append("Session.Item(""localPreferences"").Tierlevel As eTierLevelTypes: " + _Tierlevel.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").BusinessSegment As String: " + _businessSegment.ToString + "<br />")

    ' flags for turning off options 
    sOutputString.Append("Session.Item(""localPreferences"").AerodexFlag As Boolean: " + _AerodexFlag.ToString + "<br />")

    sOutputString.Append("Session.Item(""localPreferences"").DemoFlag As Boolean: " + _DemoFlag.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").MarketingFlag As Boolean: " + _MarketingFlag.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").EnableNotesFlag As Boolean: " + _EnableNotesFlag.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").ExportReportFlag As Boolean: " + _ExportReportFlag.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").EmailRequestFlag As Boolean: " + _EmailRequestFlag.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").EventRequestFlag As Boolean: " + _EventRequestFlag.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").SaveProjectsFlag As Boolean: " + _SaveProjectsFlag.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").EnableTextFlag As Boolean: " + _EnableTextFlag.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").UserAdminFlag As Boolean: " + _UserAdminFlag.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").ShareByCompanyFlag As Boolean: " + _ShareByCompanyFlag.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").ShareByParentSubFlag As Boolean: " + _ShareByParentSubFlag.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").ChatEnabled As Boolean: " + _UserChatEnabled.ToString + "<br />")

    sOutputString.Append("Session.Item(""localPreferences"").SMSActiveFlag As Boolean: " + _SMSActiveFlag.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").SMSActivationStatus As eSMSActivateTypes: " + _SMSActivationStatus.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").MobleWebStatus As Boolean: " + _MobleWebStatus.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").SmsPhoneNumber As String: " + _SmsPhoneNumber.Trim + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").SmsProviderName As String: " + _SmsProviderName.Trim + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").SmsProviderID As Long: " + _SmsProviderID.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").SmsSelectedEvents As String: " + _SmsSelectedEvents.Trim + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").SmsSelectedModels As String: " + _SmsSelectedModels.Trim + "<br />")

    sOutputString.Append("Session.Item(""localPreferences"").bHasNotifications As Boolean: " + _bHasNotifications.ToString + "<br />")

    sOutputString.Append("Session.Item(""localPreferences"").UserBusinessFlag As Boolean: " + _UserBusinessFlag.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").UserHelicoptersFlag As Boolean: " + _UserHelicoptersFlag.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").UserRegionalFlag As Boolean: " + _UserRegionalFlag.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").UserCommercialFlag As Boolean: " + _UserCommercialFlag.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").UserAirBPFlag As Boolean: " + _UserAirBPFlag.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").UserABIFlag As Boolean: " + _UserABIFlag.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").isUserStarRptFlag As Boolean: " + _isUserStarRptFlag.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").isUserSPIViewFlag As Boolean: " + _isUserSPIViewFlag.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").isUserYachtFlag As Boolean: " + _isUserYachtFlag.ToString + "<br />")

    sOutputString.Append("Session.Item(""localPreferences"").isHeliOnlyProduct As Boolean: " + _isHeliOnlyProduct.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").isCommercialOnlyProduct As Boolean: " + _isCommercialOnlyProduct.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").isBusinessOnlyProduct As Boolean: " + _isBusinessOnlyProduct.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").isYachtOnlyProduct As Boolean: " + _isYachtOnlyProduct.ToString + "<br />")

    sOutputString.Append("Session.Item(""localPreferences"").AerodexStandard As Boolean: " + _AerodexStandard.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").AerodexElite As Boolean: " + _AerodexElite.ToString + "<br />")

    sOutputString.Append("Session.Item(""localPreferences"").ShowNoteOnACList As Boolean: " + _ShowNoteOnACList.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").HasServerNotes As Boolean: " + _HasServerNotes.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").HasCloudNotes As Boolean: " + _HasCloudNotes.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").ShowNotes As Boolean: " + _ShowNotes.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").ShowReminders As Boolean: " + _ShowReminders.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").ShowListingsOnGlobal As Boolean: " + _ShowListingsOnGlobal.ToString + "<br />")

    sOutputString.Append("Session.Item(""localPreferences"").ServerNotesDatabaseName As String: " + _ServerNotesDatabaseName.Trim + "<br />")

    sOutputString.Append("Session.Item(""localPreferences"").ServerNotesDatabaseConn As String: " + _ServerNotesDatabaseConn.Trim + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").CloudNotesDatabaseName As String: " + _CloudNotesDatabaseName.Trim + "<br />")

    Dim database_display As Array = Split(_UserDatabaseConn, ";Password")
    sOutputString.Append("Session.Item(""localPreferences"").UserDatabaseConn As String: " + database_display(0).ToString.Trim + "<br />")

    database_display = Split(_STARDatabaseConn, ";Password")
    sOutputString.Append("Session.Item(""localPreferences"").STARDatabaseConn As String: " + database_display(0).ToString.Trim + "<br />")

    sOutputString.Append("Session.Item(""localPreferences"").AppUserName As String: " + _AppUserName.Trim + "<br />")

    sOutputString.Append("Session.Item(""localPreferences"").UserContactID As Long: " + _UserContactID.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").UserCompanyID As Long: " + _UserCompanyID.ToString + "<br />")

    sOutputString.Append("Session.Item(""localPreferences"").DefaultHomeView As Long: " + _DefaultHomeView.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").DefaultModelID As Long: " + _DefaultModelID.ToString + "<br />")

    sOutputString.Append("Session.Item(""localPreferences"").DefaultCompanyType As String: " + _DefaultCompanyType.Trim + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").CompanyType As String: " + _CompanyType.Trim + "<br />")

    sOutputString.Append("Session.Item(""localPreferences"").PageBackground As String: " + _PageBackground.Trim + "<br />")

    sOutputString.Append("Session.Item(""localPreferences"").UserPageSize As Long: " + _UserPageSize.ToString + "<br />")

    sOutputString.Append("Session.Item(""localPreferences"").UserEmailReplyToName As String: " + _UserEmailReplyToName.Trim + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").UserEmailReplyToAddress As String: " + _UserEmailReplyToAddress.Trim + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").UserEmailDefaultFormat As String: " + _UserEmailDefaultFormat.Trim + "<br />")

    sOutputString.Append("Session.Item(""localPreferences"").UseStandardOrMetric As String: " + _UseStandardOrMetric.Trim + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").UseMetricValue As Boolean: " + _UseMetricValue.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").UseStatuteMile As Boolean: " + _UseStatuteMile.ToString + "<br />")

    sOutputString.Append("Session.Item(""localPreferences"").DefaultCurrency As Long: " + _DefaultCurrency.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").CurrencyExchangeRate As Double: " + CurrencyExchangeRate.ToString + "<br />")

    sOutputString.Append("Session.Item(""localPreferences"").MaxAllowedCustomExport As Integer: " + _MaxAllowedCustomExport.ToString + "<br />")
    sOutputString.Append("Session.Item(""localPreferences"").OsBrowser As string: " + _OsBrowser.Trim + "<br />")

    Return sOutputString.ToString

    sOutputString = Nothing

  End Function

  Public Shared Function getSessionSubscriptionInfo() As DataTable
    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT Service.serv_name, Service.serv_database_name, Service.serv_active_flag, Subscription.sub_id, Subscription.sub_comp_id, Subscription.sub_contact_id,")
      sQuery.Append(" Subscription.sub_serv_code, Subscription.sub_start_date, Subscription.sub_end_date, Subscription.sub_tech_id, Subscription.sub_marketing_flag,")
      sQuery.Append(" Subscription.sub_nbr_days_expire, Subscription.sub_business_aircraft_flag, Subscription.sub_busair_tier_level, Subscription.sub_helicopters_flag,")
      sQuery.Append(" Subscription.sub_commerical_flag, Subscription.sub_regional_flag, Subscription.sub_aerodex_flag, Subscription.sub_frequency, Subscription.sub_nbr_of_installs,")
      sQuery.Append(" Subscription.sub_contract_amount, Subscription.sub_abi_flag, Subscription.sub_starreports_flag, Subscription.sub_server_side_notes_flag,")
      sQuery.Append(" Subscription.sub_yacht_flag, Subscription.sub_server_side_dbase_name, Subscription.sub_server_side_crm_regid,")
      sQuery.Append(" Subscription.sub_cloud_notes_flag, Subscription.sub_cloud_notes_database, Subscription.sub_parent_sub_id, Subscription.sub_web_action_date,")
      sQuery.Append(" Subscription.sub_share_by_parent_sub_id_flag, Subscription.sub_share_by_comp_id_flag, Subscription.sub_max_allowed_custom_export,")
      sQuery.Append(" Subscription_Login.sublogin_password, Subscription_Login.sublogin_contact_id, Subscription_Login.sublogin_active_flag, Subscription_Login.sublogin_demo_flag,")
      sQuery.Append(" Subscription_Login.sublogin_nbr_of_installs, Subscription_Login.sublogin_allow_export_flag, Subscription_Login.sublogin_allow_local_notes_flag,")
      sQuery.Append(" Subscription_Login.sublogin_allow_projects_flag, Subscription_Login.sublogin_allow_email_request_flag, Subscription_Login.sublogin_allow_event_request_flag,")
      sQuery.Append(" Subscription_Login.sublogin_bypass_active_x_registry_flag, Subscription_Login.sublogin_allow_text_message_flag, Subscription_Login.sublogin_values_flag,")
      sQuery.Append(" Subscription_Login.sublogin_web_action_date, Subscription_Install.subins_login, Subscription_Install.subins_seq_no, Subscription_Install.subins_platform_name,")
      sQuery.Append(" Subscription_Install.subins_platform_os, Subscription_Install.subins_install_date, Subscription_Install.subins_access_date,")
      sQuery.Append(" Subscription_Install.subins_active_flag, Subscription_Install.subins_web_action_date, Subscription_Install.subins_local_db_flag,")
      sQuery.Append(" Subscription_Install.subins_local_db_file, Subscription_Install.subins_webpage_timeout, Subscription_Install.subins_activex_flag,")
      sQuery.Append(" Subscription_Install.subins_autocheck_tservice, Subscription_Install.subins_terminal_service, Subscription_Install.subins_email_replyname,")
      sQuery.Append(" Subscription_Install.subins_email_replyaddress, Subscription_Install.subins_email_default_format, Subscription_Install.subins_default_airports,")
      sQuery.Append(" Subscription_Install.subins_aircraft_tab_relationship_to_ac_default, Subscription_Install.subins_contract_amount, Subscription_Install.subins_use_cookie_flag,")
      sQuery.Append(" Subscription_Install.subins_display_note_tag_on_aclist_flag, Subscription_Install.subins_evoview_id, Subscription_Install.subins_cell_number,")
      sQuery.Append(" Subscription_Install.subins_cell_service, Subscription_Install.subins_smstxt_models, Subscription_Install.subins_cell_carrier_id,")
      sQuery.Append(" Subscription_Install.subins_smstxt_active_flag, Subscription_Install.subins_mobile_active_date, Subscription_Install.subins_default_amod_id, Subscription_Install.subins_default_analysis_months,")
      sQuery.Append(" Subscription_Install.subins_evo_mobile_flag, Subscription_Install.subins_sms_events, Subscription_Install.subins_contact_id, Subscription_Install.subins_business_type_code, ")
      sQuery.Append(" Subscription_Install.subins_last_login_date, Subscription_Install.subins_last_logout_date, Subscription_Install.subins_last_session_date,")
      sQuery.Append(" Subscription_Install.subins_background_image_id, Subscription_Install.subins_nbr_rec_per_page, Subscription_Install.subins_session_guid, Subscription_Install.subins_default_models,")
      sQuery.Append(" Subscription_Install.subins_admin_flag, Subscription_Install.subins_chat_flag, Service_Frequency_AppName.serfreqan_sqlserver_name, Service_Frequency_AppName.serfreqan_database_name,")
      sQuery.Append(" Service_Frequency_AppName.serfreqan_user_id, Service_Frequency_AppName.serfreqan_password, Service_Frequency_AppName.serfreqan_appname")
      sQuery.Append(" FROM Service WITH (NOLOCK) INNER JOIN")
      sQuery.Append(" Subscription WITH (NOLOCK) ON Subscription.sub_serv_code = Service.serv_code INNER JOIN")
      sQuery.Append(" Service_Frequency_AppName WITH (NOLOCK) ON Subscription.sub_serv_code = Service.serv_code AND")
      sQuery.Append(" Subscription.sub_frequency = Service_Frequency_AppName.serfreqan_frequency INNER JOIN")
      sQuery.Append(" Subscription_Login WITH (NOLOCK) ON Subscription.sub_id = Subscription_Login.sublogin_sub_id INNER JOIN")
      sQuery.Append(" Subscription_Install WITH (NOLOCK) ON Subscription_Install.subins_sub_id = Subscription.sub_id AND")
      sQuery.Append(" Subscription_Login.sublogin_login = Subscription_Install.subins_login")

      ' use HttpContext.Current.Session.Item("localSubscription").crmSubscriptionID when not on EVO
      If HttpContext.Current.Session.Item("jetnetWebHostType") = eWebHostTypes.CRM Then
        sQuery.Append(" WHERE (Subscription_Install.subins_sub_id = " + HttpContext.Current.Session.Item("localSubscription").crmSubscriptionID.ToString.Trim + ") AND")
      Else ' use HttpContext.Current.Session.Item("localUser").crmSubSubID when on EVO
        sQuery.Append(" WHERE (Subscription_Install.subins_sub_id = " + HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString.Trim + ") AND")
      End If

      sQuery.Append(" (Subscription_Install.subins_seq_no = " + HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString.Trim + ") AND")
      sQuery.Append(" (Subscription_Install.subins_contact_id = " + HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString.Trim + ") AND")

      ' use HttpContext.Current.Session.Item("CRMJetnetUserName") when not on EVO
      If HttpContext.Current.Session.Item("jetnetWebHostType") = eWebHostTypes.CRM Then
        sQuery.Append(" (Subscription_Install.subins_login = '" + HttpContext.Current.Session.Item("CRMJetnetUserName").ToString.Trim + "') AND")
      Else ' use HttpContext.Current.Session.Item("localUser").crmUserLogin when on EVO
        sQuery.Append(" (Subscription_Install.subins_login = '" + HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString.Trim + "') AND")
      End If

      sQuery.Append(" (Service_Frequency_AppName.serfreqan_appname = '" + HttpContext.Current.Session.Item("jetnetAppName").ToString + "')")

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, GetType(clsSubscriptionClass).FullName, sQuery.ToString)

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in getSessionSubscriptionInfo load datatable" + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in getSessionSubscriptionInfo() As DataTable" + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return atemptable

  End Function

End Class
