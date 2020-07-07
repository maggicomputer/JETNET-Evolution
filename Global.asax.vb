' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Global.asax.vb $
'$$Author: Mike $
'$$Date: 6/16/20 8:29p $
'$$Modtime: 6/16/20 7:55p $
'$$Revision: 19 $
'$$Workfile: Global.asax.vb $
'
' ********************************************************************************
Imports System.Web.SessionState

Public Class Global_asax
  Inherits System.Web.HttpApplication

  Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)

    Application.Item("DebugFlag") = False
    Application.Item("DebugLocalFlag") = False

    ' connection strings 
    ' any session can overwrite the connections for the application thats why they are only defined "per session"
    Application.Item("crmMasterDatabase") = ""
    Application.Item("crmClientDatabase") = ""
    Application.Item("crmActiveDatabase") = ""
    Application.Item("crmHistoryDatabase") = ""
    Application.Item("crmJetnetDatabase") = ""
    Application.Item("crmJetnetServerNotes") = ""
    Application.Item("crmUserLogonCount") = 0

    Application.Item("crmClientSiteData") = New crmWebHostClass

    ' client database names
    Application.Item("masterDatabase") = "jetnet_ra"
    Application.Item("weeklyDatabase") = "jetnet_ra_weekly"
    Application.Item("biweeklyDatabase") = "jetnet_ra_biweekly"
    Application.Item("monthlyDatabase") = "jetnet_ra_monthly"
    Application.Item("starDatabase") = "star_reports"
    Application.Item("testDatabase") = "jetnet_ra_test"

    Application.Item("Application_Version") = Constants.ApplicationVariable.EVO

    'System.Net.ServicePointManager.SecurityProtocol = ' Net.SecurityProtocolType.Tls12 Or Net.SecurityProtocolType.Tls11

  End Sub

  Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)

    Session.Item("jetnetAdminDatabase") = ""
    Session.Item("jetnetClientDatabase") = ""
    Session.Item("jetnetStarDatabase") = ""
    Session.Item("jetnetServerNotesDatabase") = ""
    Session.Item("jetnetCloudNotesDatabase") = ""

    Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO ' default to evo change as needed
    Session.Item("jetnetAppName") = ""

    Session.Item("DataAsOfDate") = ""

    Session.Item("jetnetWebSiteType") = eWebSiteTypes.NULL
    Session.Item("jetnetWebHostType") = eWebHostTypes.NULL

    Session.Item("jetnetAutoLogonCookie") = ""
    Session.Item("jetnetUserGuid") = ""
    Session.Item("jetnetFullHostName") = ""

    Session.Item("localUser") = New crmLocalUserClass

    Session.Item("localSubscription") = New crmSubscriptionClass
    ' just used for preferences page (and other items never added to the above class)

    Session.Item("localPreferences") = New clsSubscriptionClass

    Session.Item("searchCriteria") = New SearchSelectionCriteria

    Session.Item("homebaseUserClass") = New homebaseUserClass

    Session.Item("marketGraphData") = Nothing

    Session.Item("useFAAFlightData") = "FAA"

    ' New Path Session Vars
    ' all paths are relitive to the root folder site is in ...
    Session.Item("MarketSummaryFolderVirtualPath") = "tempfiles"
    Session.Item("ImagesVirtualPath") = "images"
    Session.Item("DocumentFolderVirtualPath") = "documents"
    Session.Item("HelpDocumentFolderVirtualPath") = "help/documents"
    Session.Item("AircraftPicturesFolderVirtualPath") = "pictures/aircraft"
    Session.Item("ModelPicturesFolderVirtualPath") = "pictures/model"
    Session.Item("AccountRepPicturesFolderVirtualPath") = "pictures/accountrep"
    Session.Item("CompanyPicturesFolderVirtualPath") = "pictures/company"
    Session.Item("ContactPicturesFolderVirtualPath") = "pictures/contact"
    Session.Item("YachtPicturesFolderVirtualPath") = "pictures/yacht"
    Session.Item("YachtModelPicturesFolderVirtualPath") = "pictures/yacht_model"
    Session.Item("ModelVideosFolderVirtualPath") = "mpeg"
    Session.Item("FAAPDFFolderVirtualPath") = "FAAPDF/LIBRARY"
    Session.Item("NTSBFolderVirtualPath") = "NTSB/LIBRARY"
    Session.Item("337FolderVirtualPath") = "337/LIBRARY"

    Session.Item("ABIPhotosFolderVirtualPath") = "pictures"

    Session.Item("masterRecordID") = ""
    Session.Item("masterRecordError") = ""
    Session.Item("crmUserType") = ""
    Session.Item("crmUserRegistered") = False
    Session.Item("crmUserLogon") = False
    Session.Item("rebuildCookieLogon") = False
    Session.Item("SubscriptionLogOn") = ""
    Session.Item("SubscriptionInstallDate") = ""
    Session.Item("SubscriptionLastAccess") = ""
    Session.Item("MasterAircraftSelect") = ""
    Session.Item("MasterAircraftFrom") = ""
    Session.Item("MasterAircraftWhere") = ""
    Session.Item("MasterAircraftSort") = ""
    Session.Item("AircraftSort_Company") = False

    Session.Item("tabAircraftType") = ""
    Session.Item("tabAircraftMake") = ""
    Session.Item("tabAircraftModel") = ""
    Session.Item("tabAircraftModelWeightClass") = ""
    Session.Item("tabAircraftMfrNames") = ""
    Session.Item("tabAircraftSize") = ""

    Session.Item("viewAircraftType") = ""
    Session.Item("viewAircraftMake") = ""
    Session.Item("viewAircraftModel") = ""
    Session.Item("viewAircraftModelWeightClass") = ""
    Session.Item("viewAircraftMfrNames") = ""
    Session.Item("viewAircraftSize") = ""

    Session.Item("UserDefaultFlag") = False

    ' These are used to remember each check box(s) state
    Session.Item("chkCommercialFilter") = False
    Session.Item("chkRegionalFilter") = False
    Session.Item("chkBusinessFilter") = False
    Session.Item("chkHelicopterFilter") = False
    'Session.Item("chkAirBPFilter") = False
    'Session.Item("chkABIFilter") = False
    'Session.Item("chkSTARFilter") = False
    'Session.Item("chkSPIFilter") = False
    'Session.Item("chkYachtFilter") = False

    ' These are used to remember which filter(s) are applied
    Session.Item("hasCommercialFilter") = False
    Session.Item("hasRegionalFilter") = False
    Session.Item("hasBusinessFilter") = False
    Session.Item("hasHelicopterFilter") = False
    'Session.Item("hasAirBPFilter") = False
    'Session.Item("hasABIFilter") = False
    'Session.Item("hasSTARFilter") = False
    'Session.Item("hasSPIFilter") = False
    'Session.Item("hasYachtFilter") = False

    ' Global flag for model filter 
    Session.Item("hasModelFilter") = False
    Session.Item("lastModelFilter") = ""

    Session.Item("lastModelFilter") = ""
    Session.Item("lastView") = -1
    Session.Item("starReportTab_ReportID") = -1

    ' session arrays to hold type/make/model dropdown list values
    Session.Item("AircraftTypeLableArray") = Nothing
    Session.Item("AirframeAmodArray") = Nothing
    Session.Item("AirframeArray") = Nothing

    'Session arrays to hold mfrNames and aircraft sizes ddl values
    Session.Item("AircraftMfrNamesArray") = Nothing
    Session.Item("AircraftSizeArray") = Nothing

    'session arrays to hold default airframes
    Session.Item("DefaultAirframeArray") = Nothing

    ' session arrays to hold region/content/country/state/timezone dropdown list values
    Session.Item("ContinentArray") = Nothing
    Session.Item("RegionArray") = Nothing
    Session.Item("TimeZoneArray") = Nothing

    ' session variables for region/content/country/state/timezone dropdowns
    Session.Item("hasCompanyTimeZones") = True
    Session.Item("hasViewTimeZones") = True

    ' session arrays to hold category/brand/model dropdown list values
    Session.Item("YachtYmodArray") = Nothing
    Session.Item("YachtCategoryLableArray") = Nothing
    Session.Item("YachtArray") = Nothing

    ' session variables for events Category/Type dropdowns
    Session.Item("EventCategoryArray") = Nothing

    ' session variables to hold event selections
    Session.Item("eventCatType") = ""
    Session.Item("eventCatCode") = ""
    Session.Item("eventType") = "AIRCRAFT"

    ' session variables to hold yacht selections
    Session.Item("tabYachtCategory") = "" ' not used

    Session.Item("tabYachtBrand") = ""
    Session.Item("tabYachtModel") = ""
    Session.Item("tabYachtSize") = ""
    Session.Item("tabYachtType") = ""

    Session.Item("viewYachtCategory") = ""

    Session.Item("viewYachtBrand") = ""
    Session.Item("viewYachtModel") = ""
    Session.Item("viewYachtSize") = ""
    Session.Item("viewYachtType") = ""

    Session.Item("viewRegionOrContinent") = "Continent"
    Session.Item("viewRegion") = ""
    Session.Item("viewCountry") = ""
    Session.Item("viewState") = ""
    Session.Item("viewTimeZone") = ""

    Session.Item("viewDocsStartDate") = FormatDateTime(DateAdd(DateInterval.Month, (-1 * 6), CDate(Now.ToShortDateString)).ToString, DateFormat.ShortDate)
    Session.Item("viewDocsEndDate") = Now.ToShortDateString

    Session.Item("baseRegionOrContinent") = "Continent"
    Session.Item("baseRegion") = ""
    Session.Item("baseCountry") = ""
    Session.Item("baseState") = ""

    Session.Item("companyRegionOrContinent") = "Continent"
    Session.Item("companyRegion") = ""
    Session.Item("companyCountry") = ""
    Session.Item("companyState") = ""
    Session.Item("companyTimeZone") = ""

    Session.Item("fuelPriceBase") = 0
    Session.Item("homebasefuelPrice") = 0
    Session.Item("localfuelPrice") = 0

    Session.Item("OpCostsModelID") = -1
    Session.Item("OpCostsModelList") = ""
    Session.Item("OpCostsBaseFileName") = ""

    Session.Item("localUserID") = 0
    Session.Item("localUser").crmUserSelectedModel = -1

    Session.Item("saveInCookie") = CBool(My.Settings.UseCookies)   ' currently unused
    Session.Item("saveInDatabase") = CBool(My.Settings.UseDatabase) ' currently unused

    Session.Item("isMobile") = False
    Session.Item("isEVOLOGGING") = False

    Session.Item("lastNoteID") = 0
    Session.Item("nSelectedNoteID") = 0

    Session.Item("nSelectedNoteCRMUserID") = 0
    Session.Item("sNewNoteString") = ""
    Session.Item("sNewNoteDate") = ""
    Session.Item("nSelectedNoteUserID") = ""
    Session.Item("sSelectedNoteUserName") = ""
    Session.Item("bSelectedNoteCancel") = False
    Session.Item("bNoteError") = False

    Session.Item("lastReminderID") = 0
    Session.Item("nSelectedReminderID") = 0

    Session.Item("nSelectedReminderCRMUserID") = 0
    Session.Item("sSelectedReminderEntryDate") = ""
    Session.Item("sNewReminderString") = ""
    Session.Item("sNewReminderDate") = ""
    Session.Item("sNewRemindertatus") = ""
    Session.Item("nSelectedReminderUserID") = ""
    Session.Item("sSelectedReminderUserName") = ""
    Session.Item("bSelectedReminderCancel") = False
    Session.Item("bReminderError") = False

    Session.Item("bIsNote") = False
    Session.Item("bIsUpdate") = False

    ' market summary Session Vars
    Session.Item("marketStartDate") = ""
    Session.Item("marketEndDate") = ""
    Session.Item("marketTimeScale") = "Months"
    Session.Item("marketScaleSets") = 6
    Session.Item("marketSumDirection") = ""
    Session.Item("marketSumType") = ""
    Session.Item("marketWeightClass") = ""
    Session.Item("marketNewUsed") = ""
    Session.Item("marketSummaryBaseFileName") = ""

    Session.Item("BusinessSegment") = ""
    Session.Item("ShowCondensedAcFormat") = "showCondensedAcFormat"

    'Added 8/2/2012 to set up defaults for new fields on user class.
    Session.Item("localUser").crmLocalUser_Background = ""
    Session.Item("localUser").crmUserRecsPerPage = 25
    Session.Item("localUser").crmUserAircraftRelationship = ""
    Session.Item("localUser").crmUserDefaultModels = ""

    Session.Item("localSubscription").crmSubscriptionID = 0
    Session.Item("localSubscription").crmMaxUserCount = 0

    Session.Item("localSubscription").crmSubDetailError = eObjDetailErrorCode.NULL
    Session.Item("localSubscription").crmSubStatusCode = eObjStatusCode.NULL
    Session.Item("localSubscription").crmDatabaseType = eDatabaseTypes.LIVE

    ' grab the server domain name
    Application.Item("crmClientSiteData").crmClientHostName = HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME").ToString.ToUpper.Trim

    ' grab the server domain path
    Application.Item("crmClientSiteData").crmClientHostPath = HttpContext.Current.Request.ServerVariables.Item("PATH_INFO").ToString.ToUpper.Trim

    'not used takes value from web.config file
    If Session.Item("saveInCookie") Then
      Session.Item("localSubscription").crmLogonType = eLogonTypes.COOKIE
    End If

    ' not used takes value from web.config file
    If Session.Item("saveInDatabase") Then
      Session.Item("localSubscription").crmLogonType = eLogonTypes.DATABASE
    End If

    ' not used
    If Not Session.Item("saveInDatabase") And Not Session.Item("saveInCookie") Then
      Session.Item("localSubscription").crmLogonType = eLogonTypes.REGISTRY
    End If

    ' gets the instance id of the current domain from IIS
    If Not String.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables.Item("INSTANCE_ID").ToString) Then
      Application.Item("crmClientSiteData").crmWebInstanceID = CInt(HttpContext.Current.Request.ServerVariables.Item("INSTANCE_ID").ToString)
    Else
      Application.Item("crmClientSiteData").crmWebInstanceID = CInt(0)
    End If

    ' currenly not used (setting for CRM to run from a local version or server version crm master database takes value from web.config file)
    Application.Item("crmClientSiteData").crmClientStandAloneMode = CBool(My.Settings.IsStandaloneMode)

    ' determine what site we are in and set the application variable Item("crmClientSiteData").crmWebHostType based on host name or path
    If Application.Item("crmClientSiteData").crmClientHostPath.ToString.ToUpper.Contains("TEST") Or Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("TEST") Then
      Application.Item("crmClientSiteData").WebSiteType = eWebSiteTypes.TEST
    ElseIf Application.Item("crmClientSiteData").crmClientHostPath.ToString.ToUpper.Contains("BETA") Or Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("BETA") Then
      Application.Item("crmClientSiteData").WebSiteType = eWebSiteTypes.BETA
    Else
      Application.Item("crmClientSiteData").WebSiteType = eWebSiteTypes.LIVE
    End If

    ' settings for running site from developers local machine
    If Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("LOCALHOST") Or
       Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("NEWEVONET") Or
       Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("YACHTSITE") Or
       Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("EVOADMIN") Or
       Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("CRMWEBCLIENT") Or
       Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("AVIATIONINDEX") Or
       Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("LOCALMOBILE") Or
       Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("LOCALHOMEBASE") Or
       Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("LOCALEVOLUTION") Or
       Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("LOCALYACHT") Or
       Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("LOCALADMIN") Or
      Application.Item("crmClientSiteData").crmClientHostName.contains("LOCALGLOBAL") Then
      Application.Item("crmClientSiteData").WebSiteType = eWebSiteTypes.LOCAL
    End If

    ' set the main data layer settings for crm takes value from web.config file
    Select Case My.Settings.WhatDataLayer.ToUpper.Trim
      Case "ACCESS"
        Session.Item("localSubscription").crmDataLayerType = eDatalayerTypes.ACCESS
        Application.Item("crmClientSiteData").crmWebDataLayerType = eDatalayerTypes.ACCESS
      Case "MSSQL"
        Session.Item("localSubscription").crmDataLayerType = eDatalayerTypes.MSSQL
        Application.Item("crmClientSiteData").crmWebDataLayerType = eDatalayerTypes.MSSQL
      Case "MYSQL"
        Session.Item("localSubscription").crmDataLayerType = eDatalayerTypes.MYSQL
        Application.Item("crmClientSiteData").crmWebDataLayerType = eDatalayerTypes.MYSQL
    End Select

    ' flag to switch sql server from live to backup (takes value from web.config file)
    Dim useBackupSQL As Boolean = CBool(My.Settings.useBackupSQL_SRV.ToString)
    ' flag to use local sql server or live sql server when running site locally (takes value from web.config file)
    Dim hasLocalSQL As Boolean = CBool(My.Settings.hasLocalSQLServer.ToString)

    ' set all the application variables and session variables based on what what "web domain" code runs on
    If Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("HOMEBASE") Then

      ' set the settings for in-house www.homebase.com [www.homebase.com] webhost (LIVE or TEST)

      Select Case Application.Item("crmClientSiteData").WebSiteType
        Case eWebSiteTypes.LIVE

          Application.Item("crmClientSiteData").AutoLogonCookie = "AutoLogonLiveInHouse"
          Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.LIVE_INHOUSE_MSSQL
          Application.Item("crmClientSiteData").crmWebHostType = eWebHostTypes.HOMEBASE
          Session.Item("localPreferences").AppUserName = "HOMEBASE"

          Application.Item("Application_Version") = Constants.ApplicationVariable.HOMEBASE
          Session.Item("localUser").crmGUID = "HMB-" + Guid.NewGuid().ToString
          Session.Item("localUser").crmEvo = True

        Case eWebSiteTypes.TEST

          Application.Item("crmClientSiteData").AutoLogonCookie = "AutoLogonTestInHouse"
          Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.TEST_INHOUSE_MSSQL
          Application.Item("crmClientSiteData").crmWebHostType = eWebHostTypes.HOMEBASE
          Session.Item("localPreferences").AppUserName = "HOMEBASE"

          Application.Item("Application_Version") = Constants.ApplicationVariable.HOMEBASE
          Session.Item("localUser").crmGUID = "HMB-" + Guid.NewGuid().ToString
          Session.Item("localUser").crmEvo = True

        Case eWebSiteTypes.LOCAL

          If hasLocalSQL Then
            ' if has local sql server use local default sql connection
            Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LOCAL_MSSQL
          Else
            If Not useBackupSQL Then
              ' if doesnt have local sql server use live default sql connection
              Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.TEST_INHOUSE_MSSQL
            Else
              ' if doesnt have local sql server use backup default sql connection
              Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.TEST_INHOUSE_MSSQL
            End If
          End If

          Application.Item("crmClientSiteData").AutoLogonCookie = "AutoLogonLocalInHouse"
          Application.Item("crmClientSiteData").crmWebHostType = eWebHostTypes.HOMEBASE
          Session.Item("localPreferences").AppUserName = "HOMEBASELOCAL"

          Application.Item("Application_Version") = Constants.ApplicationVariable.HOMEBASE
          Session.Item("localUser").crmGUID = "HMB-" + Guid.NewGuid().ToString
          Session.Item("localUser").crmEvo = True

      End Select

    ElseIf Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("INTERNALEVOLUTION.COM") Then

      ' set the settings for INTERNALEVOLUTION.COM [www.internalevolution.com] webhost (LIVE)
      If hasLocalSQL Then
        ' if has local sql server use local default sql connection
        Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LOCAL_MSSQL
      Else
        If Not useBackupSQL Then
          ' if doesnt have local sql server use live default sql connection
          Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.TEST_LOCAL_MSSQL
        Else
          ' if doesnt have local sql server use backup default sql connection
          Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.TEST_LOCAL_MSSQL_BK
        End If
      End If

      Application.Item("crmClientSiteData").AutoLogonCookie = "AutoLogonLiveInHouse"
      Application.Item("crmClientSiteData").crmWebHostType = eWebHostTypes.EVOLUTION
      Session.Item("localPreferences").AppUserName = "EVOLIVE"

      Application.Item("Application_Version") = Constants.ApplicationVariable.EVO
      Session.Item("localUser").crmGUID = "EVO-" + Guid.NewGuid().ToString
      Session.Item("localUser").crmEvo = True

    ElseIf Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("JETNETEVO2.COM") Then

      ' set the settings for JETNETEVO2.COM [www.jetnetweb.com] webhost (LIVE)

      Application.Item("crmClientSiteData").AutoLogonCookie = "AutoLogonLiveRealWorld"

      If hasLocalSQL Then
        ' if has local sql server use local default sql connection
        Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LOCAL_MSSQL
      Else
        If Not useBackupSQL Then
          ' if doesnt have local sql server use live default sql connection
          Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LIVE_MSSQL
        Else
          ' if doesnt have local sql server use backup default sql connection
          Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LIVE_MSSQL_BK
        End If
      End If

      Application.Item("crmClientSiteData").crmWebHostType = eWebHostTypes.EVOLUTION
      Session.Item("localPreferences").AppUserName = "EVOLIVE"

      Application.Item("Application_Version") = Constants.ApplicationVariable.EVO
      Session.Item("localUser").crmGUID = "EVO-" + Guid.NewGuid().ToString
      Session.Item("localUser").crmEvo = True

    ElseIf Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("JETNETTEST.COM") Then

      ' set the settings for JETNETTEST.COM [www.jetnetweb.com] webhost (TEST)

      Application.Item("crmClientSiteData").AutoLogonCookie = "AutoLogonTestRealWorld"

      If hasLocalSQL Then
        ' if has local sql server use local default sql connection
        Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LOCAL_MSSQL
      Else
        If Not useBackupSQL Then
          ' if doesnt have local sql server use test default sql connection
          Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.TEST_LIVE_MSSQL
        Else
          ' if doesnt have local sql server use backup test default sql connection
          Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.TEST_LIVE_MSSQL_BK
        End If
      End If

      Application.Item("crmClientSiteData").crmWebHostType = eWebHostTypes.EVOLUTION
      Session.Item("localPreferences").AppUserName = "EVOTEST"

      Application.Item("Application_Version") = Constants.ApplicationVariable.EVO
      Session.Item("localUser").crmGUID = "EVO-" + Guid.NewGuid().ToString
      Session.Item("localUser").crmEvo = True

    ElseIf Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("JETNETEVOLUTION.COM") Then

      ' set the settings for JETNETEVOLUTION.COM [www.jetnetevolution.com] webhost (LIVE) (current devlopment site)

      Application.Item("crmClientSiteData").AutoLogonCookie = "AutoLogonLiveRealWorld"

      If hasLocalSQL Then
        ' if has local sql server use local default sql connection
        Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LOCAL_MSSQL
      Else
        If Not useBackupSQL Then
          ' if doesnt have local sql server use live default sql connection
          Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LIVE_MSSQL
        Else
          ' if doesnt have local sql server use backup default sql connection
          Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LIVE_MSSQL_BK
        End If
      End If

      Session.Item("localPreferences").AppUserName = "EVOLIVEDOTNET"
      Application.Item("crmClientSiteData").crmWebHostType = eWebHostTypes.EVOLUTION

      Application.Item("Application_Version") = Constants.ApplicationVariable.EVO
      Session.Item("localUser").crmGUID = "EVO-" + Guid.NewGuid().ToString
      Session.Item("localUser").crmEvo = True

    ElseIf Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("LOCALHOST") Then

      ' set the settings for LOCALHOST [amanda's machine, matt's machine] webhost (LOCAL)

      Application.Item("crmClientSiteData").AutoLogonCookie = "AutoLogonLocal"

      If hasLocalSQL Then
        ' if has local sql server use local default sql connection
        Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LOCAL_MSSQL
      Else
        If Not useBackupSQL Then
          ' if doesnt have local sql server use live default sql connection
          Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.TEST_LOCAL_MSSQL
        Else
          ' if doesnt have local sql server use backup default sql connection
          Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.TEST_LOCAL_MSSQL_BK
        End If
      End If

      Session.Item("localPreferences").AppUserName = "EVOLOCAL"
      Application.Item("crmClientSiteData").crmWebHostType = eWebHostTypes.EVOLUTION

      Application.Item("Application_Version") = Constants.ApplicationVariable.EVO
      Session.Item("localUser").crmGUID = "EVO-" + Guid.NewGuid().ToString
      Session.Item("localUser").crmEvo = True

    ElseIf Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("NEWEVONET") Then

      ' set the settings for NEWEVONET [mvintech07] webhost (LOCAL)

      Application.Item("crmClientSiteData").AutoLogonCookie = "AutoLogonLocal"

      If hasLocalSQL Then
        ' if has local sql server use local default sql connection
        Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LOCAL_MSSQL
      Else
        If Not useBackupSQL Then
          ' if doesnt have local sql server use live default sql connection
          Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.TEST_LOCAL_MSSQL
        Else
          ' if doesnt have local sql server use backup default sql connection
          Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.TEST_LOCAL_MSSQL_BK
        End If
      End If

      Session.Item("localPreferences").AppUserName = "EVOLOCAL"
      Application.Item("crmClientSiteData").crmWebHostType = eWebHostTypes.EVOLUTION

      Application.Item("Application_Version") = Constants.ApplicationVariable.EVO
      Session.Item("localUser").crmGUID = "EVO-" + Guid.NewGuid().ToString
      Session.Item("localUser").crmEvo = True

    ElseIf Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("MOBILE") Then

      ' set the settings for JETNETEVOMOBILE.COM [www.jetnetevomobile.com, www.testevolutionmobile.com, mobile.jetnet.com] webhost (LIVE) (current devlopment site)

      Application.Item("crmClientSiteData").AutoLogonCookie = "AutoLogonLiveRealWorld"

      If Application.Item("crmClientSiteData").WebSiteType <> eWebSiteTypes.LOCAL Then

        Session.Item("localPreferences").AppUserName = "EVOMOBILE"

        If hasLocalSQL Then
          ' if has local sql server use local default sql connection
          Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LOCAL_MSSQL
        Else
          If Not useBackupSQL Then
            ' if doesnt have local sql server use live default sql connection
            Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LIVE_MSSQL
          Else
            ' if doesnt have local sql server use backup default sql connection
            Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LIVE_MSSQL_BK
          End If
        End If

      Else

        Session.Item("localPreferences").AppUserName = "EVOLOCAL"

        If hasLocalSQL Then
          ' if has local sql server use local default sql connection
          Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LOCAL_MSSQL
        Else
          If Not useBackupSQL Then
            ' if doesnt have local sql server use live default sql connection
            Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.TEST_LOCAL_MSSQL
          Else
            ' if doesnt have local sql server use backup default sql connection
            Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.TEST_LOCAL_MSSQL_BK
          End If
        End If

      End If

      Application.Item("crmClientSiteData").crmWebHostType = eWebHostTypes.EVOLUTION

      Application.Item("Application_Version") = Constants.ApplicationVariable.EVO
      Session.Item("localUser").crmGUID = "EVO-" + Guid.NewGuid().ToString
      Session.Item("localUser").crmEvo = True

      Session.Item("isMobile") = True

    ElseIf Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Trim.Contains("YACHT") Then

      ' set the settings for YACHT-SPOT.COM [www.yacht-spotonline.com] webhost (LIVE)

      Application.Item("crmClientSiteData").AutoLogonCookie = "AutoLogonLiveYacht"

      If Application.Item("crmClientSiteData").WebSiteType <> eWebSiteTypes.LOCAL Then

        Session.Item("localPreferences").AppUserName = "EVOLIVEDOTNET"

        If hasLocalSQL Then
          ' if has local sql server use local default sql connection
          Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LOCAL_MSSQL
        Else
          If Not useBackupSQL Then
            ' if doesnt have local sql server use live default sql connection
            Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LIVE_MSSQL
          Else
            ' if doesnt have local sql server use backup default sql connection
            Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LIVE_MSSQL_BK
          End If
        End If

      Else

        Session.Item("localPreferences").AppUserName = "EVOLIVE"

        If hasLocalSQL Then
          ' if has local sql server use local default sql connection
          Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LOCAL_MSSQL
        Else
          If Not useBackupSQL Then
            ' if doesnt have local sql server use live default sql connection
            Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.TEST_LOCAL_MSSQL
          Else
            ' if doesnt have local sql server use backup default sql connection
            Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.TEST_LOCAL_MSSQL_BK
          End If
        End If

      End If

      Application.Item("crmClientSiteData").crmWebHostType = eWebHostTypes.YACHT

      Application.Item("Application_Version") = Constants.ApplicationVariable.YACHT
      Session.Item("localUser").crmGUID = "YCT-" + Guid.NewGuid().ToString
      Session.Item("localUser").crmEvo = True

    ElseIf Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Trim.Contains("ADMIN") Then

      ' set the settings for EVOLUTIONADMIN.COM [www.evolutionadmin.com] webhost (LIVE)

      Application.Item("crmClientSiteData").AutoLogonCookie = ""

      If Application.Item("crmClientSiteData").WebSiteType <> eWebSiteTypes.LOCAL Then

        Session.Item("localPreferences").AppUserName = "EVOLIVEDOTNET"

        If hasLocalSQL Then
          ' if has local sql server use local default sql connection
          Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LOCAL_MSSQL
        Else
          If Not useBackupSQL Then
            ' if doesnt have local sql server use live default sql connection
            Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LIVE_MSSQL
          Else
            ' if doesnt have local sql server use backup default sql connection
            Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LIVE_MSSQL_BK
          End If
        End If

      Else

        Session.Item("localPreferences").AppUserName = "EVOLOCAL"

        If hasLocalSQL Then
          ' if has local sql server use local default sql connection
          Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LOCAL_MSSQL
        Else
          If Not useBackupSQL Then
            ' if doesnt have local sql server use live default sql connection
            Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.TEST_LOCAL_MSSQL
          Else
            ' if doesnt have local sql server use backup default sql connection
            Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.TEST_LOCAL_MSSQL_BK
          End If
        End If

      End If

      Application.Item("crmClientSiteData").crmWebHostType = eWebHostTypes.ADMIN

      Application.Item("Application_Version") = Constants.ApplicationVariable.CUSTOMER_CENTER
      Session.Item("localUser").crmGUID = "ADM-" + Guid.NewGuid().ToString
      Session.Item("localUser").crmEvo = True

    ElseIf Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Trim.Contains("CRM") Then

      ' set the settings for CRM.COM [jetnetcrm2.jetnet.com] webhost (LIVE or BETA or TEST or LOCAL)

      Application.Item("crmClientSiteData").AutoLogonCookie = ""

      If Application.Item("crmClientSiteData").WebSiteType <> eWebSiteTypes.LOCAL Then

        Select Case Application.Item("crmClientSiteData").WebSiteType
          Case eWebSiteTypes.LIVE
            Session.Item("localPreferences").AppUserName = "CRMLIVE"
          Case eWebSiteTypes.BETA
            Session.Item("localPreferences").AppUserName = "CRMBETA"
          Case eWebSiteTypes.TEST
            Session.Item("localPreferences").AppUserName = "CRMTEST"
        End Select

        If hasLocalSQL Then
          ' if has local sql server use local default sql connection
          Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LOCAL_MSSQL
        Else
          If Not useBackupSQL Then
            ' if doesnt have local sql server use live default sql connection
            Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LIVE_MSSQL
          Else
            ' if doesnt have local sql server use backup default sql connection
            Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LIVE_MSSQL_BK
          End If
        End If

      Else

        Session.Item("localPreferences").AppUserName = "CRMLOCAL"

        If hasLocalSQL Then
          ' if has local sql server use local default sql connection
          Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LOCAL_MSSQL
        Else
          If Not useBackupSQL Then
            ' if doesnt have local sql server use live default sql connection
            Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.TEST_LOCAL_MSSQL
          Else
            ' if doesnt have local sql server use backup default sql connection
            Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.TEST_LOCAL_MSSQL_BK
          End If
        End If

      End If

      Application.Item("crmClientSiteData").crmWebHostType = eWebHostTypes.CRM

      Application.Item("Application_Version") = Constants.ApplicationVariable.CRM
      Session.Item("localUser").crmGUID = "CRM-" + Guid.NewGuid().ToString
      Session.Item("localUser").crmEvo = False

    ElseIf Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Trim.Contains("INDEX") Then

      Application.Item("crmClientSiteData").AutoLogonCookie = ""

      If Application.Item("crmClientSiteData").WebSiteType <> eWebSiteTypes.LOCAL Then

        Session.Item("localPreferences").AppUserName = "ABILIVE"

        If hasLocalSQL Then
          ' if has local sql server use local default sql connection
          Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LOCAL_MSSQL
        Else
          If Not useBackupSQL Then
            ' if doesnt have local sql server use live default sql connection
            Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LIVE_MSSQL
          Else
            ' if doesnt have local sql server use backup default sql connection
            Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LIVE_MSSQL_BK
          End If
        End If

      Else

        Session.Item("localPreferences").AppUserName = "ABITEST"

        If hasLocalSQL Then
          ' if has local sql server use local default sql connection
          Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LOCAL_MSSQL
        Else
          If Not useBackupSQL Then
            ' if doesnt have local sql server use live default sql connection
            Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.TEST_LOCAL_MSSQL
          Else
            ' if doesnt have local sql server use backup default sql connection
            Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.TEST_LOCAL_MSSQL_BK
          End If
        End If

      End If

      Application.Item("crmClientSiteData").crmWebHostType = eWebHostTypes.ABI

      Application.Item("Application_Version") = Constants.ApplicationVariable.ABI
      Session.Item("localUser").crmGUID = "ABI-" + Guid.NewGuid().ToString
      Session.Item("localUser").crmEvo = True

    ElseIf Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Trim.Contains("GLOBAL") Then

      Application.Item("crmClientSiteData").AutoLogonCookie = ""

      Select Case Application.Item("crmClientSiteData").WebSiteType
        Case eWebSiteTypes.LIVE
          Session.Item("localPreferences").AppUserName = "ABILIVE"
        Case eWebSiteTypes.BETA
          Session.Item("localPreferences").AppUserName = "ABIBETA"
        Case eWebSiteTypes.TEST
          Session.Item("localPreferences").AppUserName = "ABITEST"
        Case eWebSiteTypes.LOCAL
          Session.Item("localPreferences").AppUserName = "ABILOCAL"
      End Select

      If Application.Item("crmClientSiteData").WebSiteType <> eWebSiteTypes.LOCAL Then

        If hasLocalSQL Then
          ' if has local sql server use local default sql connection
          Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LOCAL_MSSQL
        Else
          If Not useBackupSQL Then
            ' if doesnt have local sql server use live default sql connection
            Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LIVE_MSSQL
          Else
            ' if doesnt have local sql server use backup default sql connection
            Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LIVE_MSSQL_BK
          End If
        End If

      Else

        If hasLocalSQL Then
          ' if has local sql server use local default sql connection
          Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LOCAL_MSSQL
        Else
          If Not useBackupSQL Then
            ' if doesnt have local sql server use live default sql connection
            Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.TEST_LOCAL_MSSQL
          Else
            ' if doesnt have local sql server use backup default sql connection
            Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.TEST_LOCAL_MSSQL_BK
          End If
        End If

      End If

      Application.Item("crmClientSiteData").crmWebHostType = eWebHostTypes.ABI

      Application.Item("Application_Version") = Constants.ApplicationVariable.ABI
      Session.Item("localUser").crmGUID = "ABI-" + Guid.NewGuid().ToString
      Session.Item("localUser").crmEvo = True

    Else

      ' if no HOSTNAMES match default to live evo site
      ' set the settings for www.jetnetevolution.COM [www.jetnetevolution.com] webhost (LIVE)

      If Application.Item("crmClientSiteData").WebSiteType <> eWebSiteTypes.LOCAL Then

        If hasLocalSQL Then
          ' if has local sql server use local default sql connection
          Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LOCAL_MSSQL
        Else
          If Not useBackupSQL Then
            ' if doesnt have local sql server use live default sql connection
            Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LIVE_MSSQL
          Else
            ' if doesnt have local sql server use backup default sql connection
            Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LIVE_MSSQL_BK
          End If
        End If

      Else

        If hasLocalSQL Then
          ' if has local sql server use local default sql connection
          Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.DEFAULT_LOCAL_MSSQL
        Else
          If Not useBackupSQL Then
            ' if doesnt have local sql server use live default sql connection
            Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.TEST_LOCAL_MSSQL
          Else
            ' if doesnt have local sql server use backup default sql connection
            Application.Item("crmClientSiteData").AdminDatabaseConn = My.Settings.TEST_LOCAL_MSSQL_BK
          End If
        End If

      End If

      Application.Item("crmClientSiteData").AutoLogonCookie = "AutoLogonLiveRealWorld"

      Application.Item("crmClientSiteData").crmWebHostType = eWebHostTypes.EVOLUTION
      Session.Item("localPreferences").AppUserName = "EVOLIVE"

      Application.Item("Application_Version") = Constants.ApplicationVariable.EVO
      Session.Item("localUser").crmGUID = "EVO-" + Guid.NewGuid().ToString
      Session.Item("localUser").crmEvo = True

    End If

    ' this sets the full host name based on host machine its running
    ' the ClientFullHostName is used when generating reports with links in them
    ' so the link resolves properly from the users machine or from the pdf generator www.jetnetevomobile.com

    If (Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Substring(0, 3).Contains("WWW")) Then

      If (Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("JETNETEVOLUTION.COM") Or
          Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("TESTJETNETEVOLUTION.COM") Or
          Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("MOBILE.JETNET.COM") Or
          Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("JETNETEVOMOBILE.COM") Or
          Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("LOCALEVOLUTION.COM")) Then
        Application.Item("crmClientSiteData").ClientFullHostName = "https://" + Application.Item("crmClientSiteData").crmClientHostName.ToLower.Trim + "/"
      Else
        Application.Item("crmClientSiteData").ClientFullHostName = "http://" + Application.Item("crmClientSiteData").crmClientHostName.ToLower.Trim + "/"
      End If

    Else

      If (Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("JETNETEVOLUTION.COM") Or
          Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("TESTJETNETEVOLUTION.COM") Or
          Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("MOBILE.JETNET.COM") Or
          Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("JETNETEVOMOBILE.COM") Or
          Application.Item("crmClientSiteData").crmClientHostName.ToString.ToUpper.Contains("LOCALEVOLUTION.COM")) Then
        Application.Item("crmClientSiteData").ClientFullHostName = "https://www." + Application.Item("crmClientSiteData").crmClientHostName.ToString.ToLower.Trim + "/"
      Else
        Application.Item("crmClientSiteData").ClientFullHostName = "http://www" + Application.Item("crmClientSiteData").crmClientHostName.ToLower.Trim + "/"
      End If

    End If

    If Application.Item("crmClientSiteData").WebSiteType = eWebSiteTypes.LOCAL Then

      If hasLocalSQL Then
        ' if has local sql server use local test default cloud sql connection
        Application.Item("crmClientSiteData").CloudDatabaseConn = My.Settings.TEST_CLOUD_LOCAL_MSSQL
      Else
        If Not useBackupSQL Then
          ' if has local sql server use local test default cloud sql connection
          Application.Item("crmClientSiteData").CloudDatabaseConn = My.Settings.TEST_CLOUD_LOCAL_MSSQL
        Else
          ' if has local sql server use local backup test default cloud sql connection
          Application.Item("crmClientSiteData").CloudDatabaseConn = My.Settings.TEST_CLOUD_LOCAL_MSSQL_BK
        End If
      End If

    Else

      If Not useBackupSQL Then
        ' if doesnt have local sql server use live default cloud sql connection
        Application.Item("crmClientSiteData").CloudDatabaseConn = My.Settings.CLOUD_LIVE_MSSQL
      Else
        ' if doesnt have local sql server use backup live default cloud sql connection
        Application.Item("crmClientSiteData").CloudDatabaseConn = My.Settings.CLOUD_LIVE_MSSQL_BK
      End If

    End If

    ' set admin and cloud note connection strings for this session
    Session.Item("jetnetAdminDatabase") = Application.Item("crmClientSiteData").AdminDatabaseConn.ToString
    Session.Item("jetnetCloudNotesDatabase") = Application.Item("crmClientSiteData").CloudDatabaseConn.ToString

    Session.Item("jetnetAppVersion") = CType(Application.Item("Application_Version"), Constants.ApplicationVariable)
    Session.Item("jetnetAppName") = Session.Item("localPreferences").AppUserName.ToString

    Session.Item("jetnetWebSiteType") = CType(Application.Item("crmClientSiteData").WebSiteType, crmWebClient.eWebSiteTypes)
    Session.Item("jetnetWebHostType") = CType(Application.Item("crmClientSiteData").crmWebHostType, crmWebClient.eWebHostTypes)

    Session.Item("jetnetAutoLogonCookie") = Application.Item("crmClientSiteData").AutoLogonCookie.ToString
    Session.Item("jetnetUserGuid") = Session.Item("localUser").crmGUID.ToString

    Session.Item("jetnetFullHostName") = Application.Item("crmClientSiteData").ClientFullHostName.ToString

    Session.Item("webSiteInstanceID") = CInt(Application.Item("crmClientSiteData").crmWebInstanceID.ToString)

    Dim showOffLineScreen = CBool(My.Settings.showOffLine)

    If showOffLineScreen Then
      Response.Redirect("offLine.aspx", True)
    End If

    'CRM part of the login
    If Session.Item("localUser").crmEvo = False Then

      Dim sQuery As String = ""
      Dim bFoundConnectionData As Boolean = False

      Dim strServerName As String = ""
      Dim strDatabase As String = ""
      Dim strUserID As String = ""
      Dim strPassWD As String = ""

      Dim strActServerName As String = ""
      Dim strActDatabase As String = ""
      Dim strActUserID As String = ""
      Dim strActPassWD As String = ""

      Dim strHistServerName As String = ""
      Dim strHistDatabase As String = ""
      Dim strHistUserID As String = ""
      Dim strHistPassWD As String = ""

      Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
      Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
      Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader
      Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

      If Application.Item("crmClientSiteData").crmClientStandAloneMode Then
        ' connect to local client database for data connections for this host
        sQuery = "SELECT * FROM Client_Register WHERE client_webDataLayer = '" + Application.Item("crmClientSiteData").dataLayerTypeName(Application.Item("crmClientSiteData").crmWebDataLayerType)
        sQuery += "' AND client_webHostName = '" + Application.Item("crmClientSiteData").crmClientHostName + "' AND client_webSiteType = '"
        sQuery += Application.Item("crmClientSiteData").webSiteTypeName(Session.Item("jetnetWebSiteType")) + "'"
        sQuery += " AND client_regType = 'C' AND client_webInstanceID = " + Session.Item("webSiteInstanceID").ToString
      Else
        ' if NOT in standalone mode connect to master database for data connections for this host 
        sQuery = "SELECT * FROM Client_Register_Master WHERE client_webDataLayer = '" + Application.Item("crmClientSiteData").dataLayerTypeName(Application.Item("crmClientSiteData").crmWebDataLayerType)
        sQuery += "' AND client_webHostName = '" + Application.Item("crmClientSiteData").crmClientHostName + "' AND client_webSiteType = '"
        sQuery += Application.Item("crmClientSiteData").webSiteTypeName(Session.Item("jetnetWebSiteType")) + "'"
        sQuery += " AND client_regType = 'C' AND client_webInstanceID = " + Session.Item("webSiteInstanceID").ToString
      End If

      Try

        If CBool(HttpContext.Current.Application.Item("crmClientSiteData").crmClientStandAloneMode.ToString) Then
          ' if crm runs in standalone use local crm default master database connection
          MySqlConn.ConnectionString = My.Settings.DEFAULT_LOCAL_MYSQL
        Else
          If CBool(My.Settings.IsDebugMode) Then
            ' if crm runs NOT in standalone but is in debug mode use debug crm default master database connection
            MySqlConn.ConnectionString = My.Settings.DEFAULT_LIVE_MYSQL2_DEBUG
          Else

            If HttpContext.Current.Session.Item("jetnetWebSiteType") <> eWebSiteTypes.LOCAL Then
              If HttpContext.Current.Session.Item("jetnetFullHostName").ToString.ToUpper.Contains("JETNET14") Then
                ' if crm runs NOT in standalone but is NOT in debug mode and we are running on "JETNET14" use debug crm default master database connection
                MySqlConn.ConnectionString = My.Settings.DEFAULT_LIVE_MYSQL2_DEBUG.ToString
              Else
                ' if crm runs NOT in standalone but NOT not in debug mode use crm default master database connection
                MySqlConn.ConnectionString = My.Settings.DEFAULT_LIVE_MYSQL2.ToString
              End If
            Else
              ' if crm runs NOT in standalone but is NOT in debug mode BUT is LOCAL SITE use debug crm default master database connection
              MySqlConn.ConnectionString = My.Settings.DEFAULT_LIVE_MYSQL2_DEBUG.ToString
            End If

          End If
        End If

        Application.Item("crmMasterDatabase") = MySqlConn.ConnectionString.ToString

        MySqlConn.Open()

        MySqlCommand.Connection = MySqlConn
        MySqlCommand.CommandType = CommandType.Text
        MySqlCommand.CommandTimeout = 80

        MySqlCommand.CommandText = sQuery
        MySqlReader = MySqlCommand.ExecuteReader()

        If MySqlReader.HasRows Then

          MySqlReader.Read()

          If Not (IsDBNull(MySqlReader("client_regID"))) Then
            Session.Item("masterRecordID") = MySqlReader.Item("client_regID").ToString
          End If

          If Not (IsDBNull(MySqlReader("client_dbActiveHost"))) Then
            strActServerName = MySqlReader.Item("client_dbActiveHost").ToString
          End If

          If Not (IsDBNull(MySqlReader("client_dbActiveDatabase"))) Then
            strActDatabase = MySqlReader.Item("client_dbActiveDatabase").ToString
          End If

          If Not (IsDBNull(MySqlReader("client_dbActiveUID"))) Then
            strActUserID = MySqlReader.Item("client_dbActiveUID").ToString
          End If

          If Not (IsDBNull(MySqlReader("client_dbActivePWD"))) Then
            strActPassWD = MySqlReader.Item("client_dbActivePWD").ToString
          End If

          If Not (IsDBNull(MySqlReader.Item("client_regName"))) Then
            Session.Item("localUser").crmUser_RegName = MySqlReader.Item("client_regName").ToString
          End If

          If Not (IsDBNull(MySqlReader("client_dbTransactHost"))) Then
            strHistServerName = MySqlReader.Item("client_dbTransactHost").ToString
          End If

          If Not (IsDBNull(MySqlReader("client_dbTransactDatabase"))) Then
            strHistDatabase = MySqlReader.Item("client_dbTransactDatabase").ToString
          End If

          If Not (IsDBNull(MySqlReader("client_dbTransactUID"))) Then
            strHistUserID = MySqlReader.Item("client_dbTransactUID").ToString
          End If

          If Not (IsDBNull(MySqlReader("client_dbTransactPWD"))) Then
            strHistPassWD = MySqlReader.Item("client_dbTransactPWD").ToString
          End If

          If Not (IsDBNull(MySqlReader("client_dbHost"))) Then
            strServerName = MySqlReader.Item("client_dbHost").ToString
          End If

          If Not (IsDBNull(MySqlReader("client_dbDatabase"))) Then
            strDatabase = MySqlReader.Item("client_dbDatabase").ToString
          End If

          If Not (IsDBNull(MySqlReader("client_dbUID"))) Then
            strUserID = MySqlReader.Item("client_dbUID").ToString
          End If

          If Not (IsDBNull(MySqlReader("client_dbPWD"))) Then
            strPassWD = MySqlReader.Item("client_dbPWD").ToString
          End If

          bFoundConnectionData = True

          MySqlReader.Close()

        End If 'MySqlReader.HasRows 

        MySqlReader.Dispose()

      Catch MySqlException

        Session.Item("masterRecordError") = MySqlException.Message

      Finally

        MySqlCommand.Dispose()
        MySqlConn.Close()
        MySqlConn.Dispose()

      End Try

      If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Then
        If strServerName.Trim.Contains("172.30.5.47") Then
          strServerName = "jetnetcrm2.jetnet.com" ' or 192.69.4.165
        Else
          strServerName = "jetnetcrm.jetnet.com" ' or 192.69.4.159
        End If
      End If


      ' if we found connection data set the crm database connection strings
      If bFoundConnectionData Then
        Application.Item("crmClientSiteData").crmClientDatabaseConn = Application.Item("crmClientSiteData").generateMYSQLConnectionString(strServerName, strDatabase, strUserID, strPassWD)
        Application.Item("crmClientSiteData").crmActiveDatabaseConn = Application.Item("crmClientSiteData").generateMYSQLConnectionString(strActServerName, strActDatabase, strActUserID, strActPassWD)
        Application.Item("crmClientSiteData").crmHistoryDatabaseConn = Application.Item("crmClientSiteData").generateMYSQLConnectionString(strHistServerName, strHistDatabase, strHistUserID, strHistPassWD)
      End If

      If Not String.IsNullOrEmpty(Application.Item("crmClientSiteData").crmClientDatabaseConn.ToString) Then
        ' check again to make sure crm client database connection string is not blank use local crm client database connection (from crm master database)
        Application.Item("crmClientDatabase") = Application.Item("crmClientSiteData").crmClientDatabaseConn.ToString
      Else

        Application.Item("crmClientDatabase") = Application.Item("crmMasterDatabase")

      End If

    End If ' set up for crm only

    ' set debug flags when starting site
    If Request.QueryString.Item("debug") = "1" Then
      Application.Item("DebugFlag") = True
    Else

      If Request.QueryString.Item("debuglocal") = "1" Then
        Application.Item("DebugLocalFlag") = True
      Else
        Application.Item("DebugLocalFlag") = False
      End If

      Application.Item("DebugFlag") = False

    End If

  End Sub

  Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
    ' Fires at the beginning of each request
  End Sub

  Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
    ' Fires upon attempting to authenticate the use
  End Sub

  Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
    ' Fires when an error occurs
    'Server.Transfer("/errorPages/genericError.aspx")

  End Sub

  Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
    ' Fires when the session ends

    If Application.Item("crmUserLogonCount") < 2 Then
      Application.Item("crmUserLogonCount") = 0
    Else
      Application.Item("crmUserLogonCount") = Application.Item("crmUserLogonCount") - 1
    End If

    Session.Contents.RemoveAll()

  End Sub

  Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
    ' Fires when the application ends
    Application.Contents.RemoveAll()
  End Sub

End Class