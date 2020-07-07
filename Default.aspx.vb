' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Default.aspx.vb $
'$$Author: Mike $
'$$Date: 6/30/20 3:44p $
'$$Modtime: 6/30/20 3:05p $
'$$Revision: 26 $
'$$Workfile: Default.aspx.vb $
'
' ********************************************************************************

Partial Public Class _Default_aspx

  Inherits System.Web.UI.Page


  Public txtAlias As String = ""
  Public bEnableChat As Boolean

  Public bShowCalendar As Boolean

  Public bIsTestSite As Boolean = False

  Public AutoLoginVariable As Boolean = False
  Public AutoSwapApplicationVariable As Boolean = False
  Public script_version As String = ""

  Private Sub _Default_aspx_Init(sender As Object, e As EventArgs) Handles Me.Init

    Try

      script_version = My.Settings.SCRIPT_VERSION.ToString

      Dim link As HtmlLink = New HtmlLink()
      link.Attributes.Add("type", "text/css")
      link.Attributes.Add("rel", "stylesheet")
      link.Attributes.Add("href", "/EvoStyles/stylesheets/additional_styles.css" + script_version)
      Page.Header.Controls.Add(link)

      Dim link1 As HtmlLink = New HtmlLink()
      link1.Attributes.Add("type", "text/css")
      link1.Attributes.Add("rel", "stylesheet")
      link1.Attributes.Add("href", "/EvoStyles/stylesheets/header_styles.css" + script_version)
      Page.Header.Controls.Add(link1)

      If Session.Item("isMobile") = True Then

        Dim link2 As HtmlLink = New HtmlLink()
        link2.Attributes.Add("type", "text/css")
        link2.Attributes.Add("rel", "stylesheet")
        link2.Attributes.Add("href", "/EvoStyles/stylesheets/additional_mobile_styles.css" + script_version)
        Page.Header.Controls.Add(link2)

      End If

    Catch ex As Exception
      commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : Exception Thrown[" + ex.Message.Trim + "]")

    End Try

  End Sub

  Private Sub _Default_aspx_PreLoad(sender As Object, e As EventArgs) Handles Me.PreLoad

    Try

      If (Session.Item("jetnetAppVersion") <> Constants.ApplicationVariable.HOMEBASE) And (Session.Item("jetnetAppVersion") <> Constants.ApplicationVariable.CUSTOMER_CENTER) Then

        Dim url_string As String = "logout.aspx?badip=true"
        Dim UserIPAddress As String = ""
        Dim sQuery As String = ""

        Dim recCount As Integer = 0

        Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
        Dim SqlCommand As New System.Data.SqlClient.SqlCommand
        Dim SqlConnection As New System.Data.SqlClient.SqlConnection
        Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing

        Try

          Dim sConnectionStr As String = ""

          If Not IsNothing(HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR")) Then
            If Not String.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR").ToString.Trim) Then
              UserIPAddress = HttpContext.Current.Request.ServerVariables("HTTP_X_FORWARDED_FOR").ToString.Trim
            End If
          End If

          If String.IsNullOrEmpty(UserIPAddress.Trim) Then
            If Not IsNothing(HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")) Then
              If Not String.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables("REMOTE_ADDR").ToString.Trim) Then
                UserIPAddress = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR").ToString.Trim
              End If
            End If
          End If

          sQuery = "SELECT count(*) AS blockAddress FROM Monitor_TCPIP WITH (NOLOCK) WHERE mtip_TCPIP = '" + UserIPAddress + "' AND mtip_monitor_flag = 'B'"

          Dim useBackupSQL As Boolean = CBool(My.Settings.useBackupSQL_SRV.ToString)

          If Not IsNothing(HttpContext.Current.Application.Item("crmClientSiteData").AdminDatabaseConn) Then

            If Not String.IsNullOrEmpty(HttpContext.Current.Application.Item("crmClientSiteData").AdminDatabaseConn.ToString.Trim) Then
              sConnectionStr = HttpContext.Current.Application.Item("crmClientSiteData").AdminDatabaseConn.ToString.Trim
            Else

              If HttpContext.Current.Application.Item("crmClientSiteData").WebSiteType <> eWebSiteTypes.LOCAL Then
                If Not useBackupSQL Then
                  ' if doesnt have local sql server use live default sql connection
                  sConnectionStr = My.Settings.DEFAULT_LIVE_MSSQL
                Else
                  ' if doesnt have local sql server use backup default sql connection
                  sConnectionStr = My.Settings.DEFAULT_LIVE_MSSQL_BK
                End If
              Else
                If Not useBackupSQL Then
                  ' if doesnt have local sql server use live default sql connection
                  sConnectionStr = My.Settings.TEST_LOCAL_MSSQL
                Else
                  ' if doesnt have local sql server use backup default sql connection
                  sConnectionStr = My.Settings.TEST_LOCAL_MSSQL_BK
                End If
              End If

            End If

          Else

            If HttpContext.Current.Application.Item("crmClientSiteData").WebSiteType <> eWebSiteTypes.LOCAL Then
              If Not useBackupSQL Then
                ' if doesnt have local sql server use live default sql connection
                sConnectionStr = My.Settings.DEFAULT_LIVE_MSSQL
              Else
                ' if doesnt have local sql server use backup default sql connection
                sConnectionStr = My.Settings.DEFAULT_LIVE_MSSQL_BK
              End If
            Else
              If Not useBackupSQL Then
                ' if doesnt have local sql server use live default sql connection
                sConnectionStr = My.Settings.TEST_LOCAL_MSSQL
              Else
                ' if doesnt have local sql server use backup default sql connection
                sConnectionStr = My.Settings.TEST_LOCAL_MSSQL_BK
              End If
            End If

          End If

          SqlConnection.ConnectionString = sConnectionStr
          SqlConnection.Open()

          SqlCommand.Connection = SqlConnection
          SqlCommand.CommandTimeout = 1000
          SqlCommand.CommandText = sQuery

          lDataReader = SqlCommand.ExecuteReader()

          If lDataReader.HasRows Then

            lDataReader.Read()

            If Not IsDBNull(lDataReader.Item("blockAddress")) Then
              If Not String.IsNullOrEmpty(lDataReader.Item("blockAddress").ToString) Then
                recCount = CInt(lDataReader.Item("blockAddress").ToString.Trim)
              End If
            End If

          End If

          lDataReader.Close()

          If recCount = 1 Then
            commonLogFunctions.Log_User_Event_Data("UserError", "Unauthorized IP")
            Response.Redirect(url_string, False)
          End If

        Catch SqlException

          commonLogFunctions.Log_User_Event_Data("UserError", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : Exception Thrown[" + SqlException.Message.Trim + "]")

          SqlConnection.Dispose()
          SqlCommand.Dispose()

        Finally

          SqlCommand.Dispose()
          SqlConnection.Close()
          SqlConnection.Dispose()

        End Try

      End If

    Catch ex As Exception
      commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : Exception Thrown[" + ex.Message.Trim + "]")

    End Try

  End Sub

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Dim bHasNoBlankAcFieldsCookie As Boolean = False
    Dim bShowBlankAcFields As Boolean = False

    Try

      If Application.Item("crmClientSiteData").WebSiteType <> eWebSiteTypes.LIVE Then
        bIsTestSite = True
      End If

      If Application.Item("DebugFlag") Then

        Dim sDebugText As New StringBuilder

        sDebugText.Append("<div style=""background-color: white;filter: alpha(opacity=70); opacity: 0.7; padding: 5px; width:40%; "">host name : " + HttpContext.Current.Application.Item("crmClientSiteData").crmClientHostName.ToString)
        sDebugText.Append("<br />app path : " + HttpContext.Current.Application.Item("crmClientSiteData").crmClientHostPath.ToString)
        sDebugText.Append("<br />app name : " + HttpContext.Current.Session.Item("localPreferences").AppUserName.ToString)
        sDebugText.Append("<br />site type : " + HttpContext.Current.Application.Item("crmClientSiteData").webSiteTypeName(HttpContext.Current.Session.Item("jetnetWebSiteType")).ToString)
        sDebugText.Append("<br />site host type : " + HttpContext.Current.Application.Item("crmClientSiteData").webSiteHostName(HttpContext.Current.Application.Item("crmClientSiteData").crmWebHostType).ToString)
        sDebugText.Append("<br />Application_Version : " + HttpContext.Current.Session.Item("jetnetAppVersion").ToString)
        sDebugText.Append("<br />full client host name : " + Application.Item("crmClientSiteData").ClientFullHostName.ToString)
        sDebugText.Append("<br />web instance ID : " + HttpContext.Current.Application.Item("crmClientSiteData").crmWebInstanceID.ToString)

        sDebugText.Append("<br />user session guid : " + HttpContext.Current.Session.Item("localUser").crmGUID)
        sDebugText.Append("<br />is evo : " + HttpContext.Current.Session.Item("localUser").crmEvo.ToString)
        sDebugText.Append("<br />host machine : " + HttpContext.Current.Request.ServerVariables.Item("LOCAL_ADDR").ToString.ToUpper.Trim)
        sDebugText.Append("<br />crm frequency : " + Session.Item("localSubscription").crmFrequency)
        sDebugText.Append("<br />crm master connect ID : " + HttpContext.Current.Session.Item("masterRecordID").ToString)
        sDebugText.Append("<br />crm master connect error : " + HttpContext.Current.Session.Item("masterRecordError").ToString)
        sDebugText.Append("<br />crm master db : " + clsGeneral.clsGeneral.ParseOutPasswordForDBDisplay(Application.Item("crmMasterDatabase").ToString))
        sDebugText.Append("<br />crm client db : " + clsGeneral.clsGeneral.ParseOutPasswordForDBDisplay(Application.Item("crmClientDatabase").ToString))

        sDebugText.Append("<br />AutoAPILoginVariable : " + Session.Item("localUser").crmUser_API_Login.ToString)
        sDebugText.Append("<br />mobile site : " + Session.Item("isMobile").ToString)

        sDebugText.Append("</div>")

        debugTextLbl.Visible = True
        debugTextLbl.Text = sDebugText.ToString()   'AutoAPILoginVariable Session.Item("isMobile")

        sDebugText = Nothing

      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br />start load default.aspx page data : " + Now.ToString + "<br />"
      Trace.Write("Start PageLoad Default.aspx" + Now.ToString)

      If Not IsNothing(HttpContext.Current.Request.Item("whatBrowser")) Then
        If Not String.IsNullOrEmpty(HttpContext.Current.Request.Item("whatBrowser").ToString.Trim) Then
          HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br />os / browser type : " + HttpContext.Current.Request.Item("whatBrowser").ToString + "<br />"
        End If
      End If

      If HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME").ToString.ToUpper.Trim.Contains("GLOBAL") Or
        HttpContext.Current.Request.ServerVariables.Item("SERVER_NAME").ToString.ToUpper.Trim.Contains("INDEX") Then
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /> redirect to abi<br />"
        abi_functions.ABI_Redirect()
      End If

      If Not IsNothing(HttpContext.Current.Request.Item("api_error")) Then
        If Not String.IsNullOrEmpty(HttpContext.Current.Request.Item("api_error").ToString.Trim) Then
          lbl_inactive.Text = "<p style=""margin-left:0px; text-align:center;"">Your session token is invalid.</p>"
        End If
      End If

      bShowBlankAcFields = commonEvo.getUserShowBlankACFields(Session.Item("ShowCondensedAcFormat"), bHasNoBlankAcFieldsCookie)

      If bHasNoBlankAcFieldsCookie Then
        HttpContext.Current.Response.Cookies(HttpContext.Current.Session.Item("ShowCondensedAcFormat").ToString).Value = "N"
        HttpContext.Current.Response.Cookies(HttpContext.Current.Session.Item("ShowCondensedAcFormat").ToString).Expires = DateTime.Now.AddDays(300)
      End If

      'these are strictly style elements. 
      If Session.Item("localUser").crmEvo = True Then
        Dim SmallHomePageTabIndex As HttpCookie = Request.Cookies("SmallHomeActiveTab")
        Dim LargeHomePageTabIndex As HttpCookie = Request.Cookies("LargeHomeActiveTab")

        If IsNothing(SmallHomePageTabIndex) Then
          Response.Cookies("SmallHomeActiveTab").Expires = DateTime.Now.AddYears(-30)
          Response.Cookies("SmallHomeActiveTab").Value = "0"
        End If

        If IsNothing(LargeHomePageTabIndex) Then
          Response.Cookies("LargeHomeActiveTab").Expires = DateTime.Now.AddYears(-30)
          Response.Cookies("LargeHomeActiveTab").Value = "0"
        End If

        If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Or Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Or Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Or Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
          ' If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Or HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Then
          If Not IsNothing(Trim(Request("swap"))) Then
            If Trim(Request("swap")) = "true" Then
              AutoSwapApplicationVariable = True
            End If
          End If
          'End If
        End If

        Session.Item("localUser").crmUser_API_Login = False
        If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
          HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br />AutoAPILoginVariable default to:" & Session.Item("localUser").crmUser_API_Login.ToString
          '  If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Or HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Then
          If Not IsNothing(Request("apiLog")) Then
            If Trim(Request("apiLog")) = "true" Then
              Session.Item("localUser").crmUser_API_Login = True
              HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br />Session.Item(""localUser"").crmUser_API_Login set to true."
            End If
          End If
          ' End If
        End If

        Evo_Styles.Visible = True
        CRM_Styles.Visible = False
        CRM_Logo_Text.Visible = False 'turn off large CRM Text
        'header_div.Attributes.Remove("class")
        'header_div.Attributes.Add("class", "sixteen columns headerHeight")
        'belowWelcomeContainer.Attributes.Add("class", "headerHeightPadding home")
        'logo.CssClass = "evolution_logo home"

        If (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT) Then
          regular_page_information.CssClass = "yachtSpecific"
          logo.ImageUrl = "~/images/JETNET_YachtSpot.png" 'swap logo

          background_image.ImageUrl = "~/images/background/31.jpg"

          Page.Header.Title = "Welcome to JETNET Yacht Spot"

          welcome_to_text.Text = "Welcome to YachtSpot <span>Market Intelligence for the Luxury Yacht Industry.</span>"
          welcome_paragraph.Text = "<p>YachtSpot is a subscription service for authorized users only. Unauthorized use of this service may result in both financial and legal actions. If you have questions regarding your subscription status or would like to inquire about subscribing contact <a href='mailto:yachtspot@jetnet.com'>yachtspot@jetnet.com</a> or 1-(800)-553-8638.</p>"

          'If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Or HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Then
          If Not IsNothing(Trim(Request("swap"))) Then
            If Trim(Request("swap")) = "true" Then
              AutoSwapApplicationVariable = True
            End If
          End If
          'End If

        ElseIf (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO) Then
          HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br/>AutoAPILoginVariable : " & Session.Item("localUser").crmUser_API_Login.ToString
          Dim SMgr As ScriptManager
          If ScriptManager.GetCurrent(Page) Is Nothing Then
            Throw New Exception("ScriptManager not found.")
          Else
            SMgr = ScriptManager.GetCurrent(Page)
          End If


          Dim SRef As ScriptReference = New ScriptReference()
          SRef.Path = "https://cdnjs.cloudflare.com/ajax/libs/jquery/3.4.1/jquery.min.js"
          SMgr.Scripts.Add(SRef)

          Dim SRef1 As ScriptReference = New ScriptReference()
          SRef1.Path = "https://code.jquery.com/ui/1.12.1/jquery-ui.js"
          SMgr.Scripts.Add(SRef1)

          Dim SRef2 As ScriptReference = New ScriptReference()
          SRef2.Path = "https://cdnjs.cloudflare.com/ajax/libs/jquery-migrate/3.1.0/jquery-migrate.min.js"
          SMgr.Scripts.Add(SRef2)

          'Adding Common Javascript File For Folder Redirect:
          '9/30/15
          Dim SRef3 As ScriptReference = New ScriptReference()
          SRef3.Path = "~/common/common_functions.js" + script_version
          SMgr.Scripts.Add(SRef3)

          logo.ImageUrl = "~/images/JETNET_EvoMarketplace.png" 'swap logo
          logo.CssClass = "evolution_logo home"

          background_image.ImageUrl = "~/images/background/login.jpg"

          Page.Header.Title = "Welcome to JETNET Evolution"

          welcome_to_text.Text = "Welcome to JETNET Evolution <span>Market Intelligence for the Aviation Industry</span>"
          welcome_paragraph.Text = "<p>Evolution is a subscription service for authorized users only. Unauthorized use of this service may result in both financial and legal actions. If you have questions regarding your subscription status or would like to inquire about subscribing contact <a href='mailto:customerservice@jetnet.com'>customerservice@jetnet.com</a> or 1-(800)-553-8638.</p>"

          If Session.Item("isMobile") = True Then
            logo.CssClass += "logoMobileOffset"
            logo.ImageUrl = "~/images/JETNET_EvoMarketplace_Mobile.png"
            background_image.Visible = True
            fixedBar.Visible = True
          Else
            setUpHotJar()
          End If

        ElseIf (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Then

          logo.ImageUrl = "~/images/JETNET_EvoAdmin_Outlines.png" 'swap logo

          background_image.ImageUrl = "~/images/background/21.jpg"

          Page.Header.Title = "Welcome to JETNET Administration"

          welcome_to_text.Text = "Welcome to JETNET Evolution Administration <span>Configuration and Monitoring of the JETNET family of web sites.</span>"
          welcome_to_text.ForeColor = Drawing.Color.Black
          welcome_paragraph.Text = "<p>Evolution is a subscription service for authorized users only. Unauthorized use of this service may result in both financial and legal actions. If you have questions regarding your subscription status or would like to inquire about subscribing contact <a href='mailto:customerservice@jetnet.com'>customerservice@jetnet.com</a> or 1-(800)-553-8638.</p>"
          welcome_paragraph.ForeColor = Drawing.Color.Black

        ElseIf (Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE) Then

          logo.ImageUrl = "~/images/homebase.png" 'swap logo

          background_image.ImageUrl = "~/images/background/21.jpg"

          Page.Header.Title = "Welcome to JETNET Homebase"

          welcome_to_text.Text = "Welcome to JETNET Homebase <span>Internal JETNET Research web site.</span>"
          welcome_to_text.ForeColor = Drawing.Color.Black
          welcome_paragraph.Text = "<p>Please contact <a href='mailto:customerservice@jetnet.com'>customerservice@jetnet.com</a> or 1-(800)-553-8638. if you have issues with your subscription</p>"
          welcome_paragraph.ForeColor = Drawing.Color.Black

        End If

      Else
        Evo_Styles.Visible = False
        CRM_Styles.Visible = True

        logo.ImageUrl = "~/images/JETNET_MarketplaceMan.png" 'swap logo 
        'logo.CssClass = "logo_image"

        background_image.ImageUrl = "~/images/background/10.jpg"

        Page.Header.Title = "Welcome to Marketplace Manager"

        welcome_to_text.Text = "Welcome to Evolution Marketplace Manager"
        welcome_paragraph.Text = "<p>The Marketplace Manager is a subscription service for authorized users only. Unauthorized use of this service may result in both financial and legal actions. If you have questions regarding your subscription status or would like to inquire about subscribing contact <a href='mailto:customerservice@jetnet.com'>customerservice@jetnet.com</a> or 1-(800)-553-8638.</p>"

      End If

      logonUser.Visible = True

      If Not IsPostBack Then

        current_jetnet_events.Visible = False

        If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
          current_jetnet_events.Visible = True
          displayJetnetCalendar(current_jetnet_events.Text)
          ' current_jetnet_events.CssClass = "display_none"
        End If

        'This is the registration and validation control display.
        If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then
          Call checkClientMachine(CInt(Application.Item("crmClientSiteData").crmWebInstanceID.ToString))
        End If

        Call getUserCookies()

        If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then
          '//Amanda 7/18/11.
          '//All I'm doing right here is adding a simple check to see if 
          'mobile = 1 in query string. If it does, I'll set the isMobile flag. 
          If Request.QueryString.Item("mobile") = "1" Then
            Session.Item("isMobile") = True
          Else
            Session.Item("isMobile") = False
          End If
        End If

        '//Amanda 8/24/11.
        '//All I'm doing right here is adding a simple check to see if 
        'inactive = true, then display error message
        If Request.QueryString.Item("inactive") = "true" Then
          lbl_inactive.Text = "<p style=""text-align:center;"">Your session has been logged out due to inactivity.</p>"
        End If

        If Session.Item("isMobile") = False Then '//Amanda - 7/18/11. Toggling the visibility of two 
          'controls based on what type of system the user is viewing. 
          regular_page_information.Visible = True
          mobile_page_information.Visible = False
          mobile_resize.Visible = False
        Else
          ' size the eula for the phone ...
          'eulaAgreement.Width = Unit.Percentage(80)
          max_user_warning.Width = Unit.Percentage(80)
          'MPE1.RepositionMode = AjaxControlToolkit.ModalPopupRepositionMode.RepositionOnWindowResize
          eulaAgreement.Attributes.Remove("style")
          eulaAgreement.Attributes.Add("style", "display:none")
          If Session.Item("localUser").crmEvo = True Then
          Else
            max_user_warning.CssClass = "modalPopupCSS"
            eulaAgreement.CssClass = "modalPopupCSS"
            mobile_resize.Visible = True
            regular_page_information.Visible = False
            mobile_page_information.Visible = True
          End If
        End If

        If Not String.IsNullOrEmpty(Session.Item("localUser").crmUser_RegName) Then
          logon_add_info_lbl.Text = Session.Item("jetnetAppVersion") & ": " & Session.Item("localUser").crmUser_RegName
        End If

        'Evolution Only - Autologin
        If Session.Item("localUser").crmEvo = True Then

          If AutoLoginVariable = True Then
            logonUser.AutoLoginClick()
            '  Me.logonUser.LoginButton_Click()
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br/>user clicked auto log on: " + Now.ToString + "<br />"
          ElseIf Session.Item("localUser").crmUser_API_Login = True Then 'This does the same thing as autoswapapplicationvariable down below but I wanted to make them seperate statements
            'incase the need came up to differentiate. Plus this way it's easier to read.
            logonUser.AutoLoginFromOtherApplication()
          ElseIf AutoSwapApplicationVariable = True Then

            'Auto login if swapping from yacht spot or evo to the other one.
            'A variable named swap must be passed - also defaults to using the autologin if it's set first for the yacht side.
            'Only on test for now
            logonUser.AutoLoginFromOtherApplication()

          End If
        End If
      Else

      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br />end load default.aspx page data : " + Now.ToString + "<br />"

      Trace.Write("End PageLoad Default.aspx" + Now.ToString)

    Catch ex As Exception
      commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : Exception Thrown[" + ex.Message.Trim + "]")

    End Try

  End Sub

  Private Sub setUpHotJar()

    Try

      If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Or HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Then
        hotjarScriptTestEvo.Visible = True 'This enables the hotjar script on evo.
        hotjarScriptLiveEvo.Visible = False
      Else
        hotjarScriptLiveEvo.Visible = True
        hotjarScriptTestEvo.Visible = False
      End If

    Catch ex As Exception
      commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : Exception Thrown[" + ex.Message.Trim + "]")

    End Try

  End Sub

  Private Sub getUserCookies()
    Dim nCount As Integer
    Dim tmpUserId As String = "''"
    Dim tmpPassWord As String = "''"

    nCount = 0
    Try

      If Not IsNothing(Request.Cookies.Item("crmUserName")) Then
        If Request.Cookies.Item("crmUserName").Values.Count > 0 Then
          For nCount = 0 To Request.Cookies.Item("crmUserName").Values.Count - 1
            If Not IsNothing(Request.Cookies.Item("crmUserName").Values.GetKey(0)) Then

              If nCount = 0 Then

                logonUser.UserName.Text = Request.Cookies.Item("crmUserName").Values.Item(0).ToString.Trim
                logonUser.Password.Attributes.Item("value") = Session.Item("localUser").DecodeBase64(Request.Cookies.Item("crmUserPassword").Values.Item(0).ToString).ToLower.Trim
                logonUser.RememberMe.Checked = True

                'only perform check for Evo.
                If Session.Item("localUser").crmEvo = True Then
                  If Not IsNothing(Request.Cookies.Item(Application.Item("crmClientSiteData").AutoLogonCookie)) Then
                    If IIf((Request.Cookies.Item(Application.Item("crmClientSiteData").AutoLogonCookie).Values.Item(0).ToString.ToLower.Trim = "y") Or (Request.Cookies.Item(Application.Item("crmClientSiteData").AutoLogonCookie).Values.Item(0).ToString.ToLower.Trim = "true"), True, False) Then
                      logonUser.AutoLogin.Checked = True
                      AutoLoginVariable = True
                    Else
                      AutoLoginVariable = False
                    End If
                  Else
                    logonUser.AutoLogin.Checked = False
                    AutoLoginVariable = False
                  End If
                End If

              End If 'nCount = 0 Then

            Else ' see if there is just a single cookie

              logonUser.UserName.Text = Request.Cookies.Item("crmUserName").Item("").ToString.Trim
              logonUser.Password.Attributes.Item("value") = Session.Item("localUser").DecodeBase64(Request.Cookies.Item("crmUserPassword").Item("").ToString).ToLower.Trim
              logonUser.RememberMe.Checked = True

              'only perform check for Evo.
              If Session.Item("localUser").crmEvo = True Then
                If Not IsNothing(Request.Cookies.Item(Application.Item("crmClientSiteData").AutoLogonCookie)) Then
                  If IIf((Request.Cookies.Item(Application.Item("crmClientSiteData").AutoLogonCookie).Values.Item("").ToString.ToLower.Trim = "y") Or (Request.Cookies.Item(Application.Item("crmClientSiteData").AutoLogonCookie).Values.Item("").ToString.ToLower.Trim = "true"), True, False) Then
                    logonUser.AutoLogin.Checked = True
                    AutoLoginVariable = True
                  Else
                    AutoLoginVariable = False
                  End If
                End If
              Else
                logonUser.AutoLogin.Checked = False
                AutoLoginVariable = False
              End If

            End If ' Not IsNothing(Request.Cookies.Item("crmUserName").Values.GetKey(0)) Then

          Next ' nCount	

        End If ' Request.Cookies.Item("crmUserName").Values.Count > 0 Then

      End If '  Not IsNothing(Request.Cookies.Item("crmUserName")) 

    Catch ex As Exception
      commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : Exception Thrown[" + ex.Message.Trim + "]")

    End Try

  End Sub

  Private Sub checkClientMachine(ByVal inClientID As Integer)

    Dim DisplayLogin As Boolean = True
    Dim LoginErrorString As String = ""
    Dim objlocalUser As New crmLocalUserClass

    Dim nCount As Integer = 0

    Dim sQuery As String = ""
    Dim sQuery1 As String = ""

    Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
    Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
    Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader
    Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      ' log local crm user if client all ready registered
      ' when ever a user loads this page we must check for a local install

      If Session.Item("localUser").crmEvo = False Then

        If eDatalayerTypes.MYSQL Then
          If Application.Item("crmClientSiteData").crmClientStandAloneMode Then
            ' connect to local client database for registration info
            sQuery = "SELECT * FROM Client_Register WHERE client_regSecurityToken IS NOT NULL AND client_regType = 'C' AND client_regStatus = 'Y'"
            sQuery += " AND client_webDataLayer = '" + Application.Item("crmClientSiteData").dataLayerTypeName(Application.Item("crmClientSiteData").crmWebDataLayerType)
            sQuery += "' AND client_webHostName = '" + Application.Item("crmClientSiteData").crmClientHostName + "' and client_webSiteType = '"
            sQuery += Application.Item("crmClientSiteData").webSiteTypeName(Session.Item("jetnetWebSiteType")) + "' AND client_webInstanceID = " + Session.Item("webSiteInstanceID").ToString
          Else
            ' if NOT in standalone mode connect to master database registration info 
            sQuery = "SELECT * FROM Client_Register_Master WHERE client_regSecurityToken IS NOT NULL AND client_regType = 'C' AND client_regStatus = 'Y'"
            sQuery += " AND client_webDataLayer = '" + Application.Item("crmClientSiteData").dataLayerTypeName(Application.Item("crmClientSiteData").crmWebDataLayerType)
            sQuery += "' AND client_webHostName = '" + Application.Item("crmClientSiteData").crmClientHostName + "' and client_webSiteType = '"
            sQuery += Application.Item("crmClientSiteData").webSiteTypeName(Session.Item("jetnetWebSiteType")) + "' AND client_webInstanceID = " + Session.Item("webSiteInstanceID").ToString
          End If
        Else
          If Application.Item("crmClientSiteData").crmClientStandAloneMode Then
            ' connect to local client database for registration info
            sQuery = "SELECT * FROM Client_Register WHERE client_regSecurityToken NOT IS NULL AND client_regType = 'C' AND client_regStatus = 'Y'"
            sQuery += " AND client_webDataLayer = '" + Application.Item("crmClientSiteData").dataLayerTypeName(Application.Item("crmClientSiteData").crmWebDataLayerType)
            sQuery += "' AND client_webHostName = '" + Application.Item("crmClientSiteData").crmClientHostName + "' and client_webSiteType = '"
            sQuery += Application.Item("crmClientSiteData").webSiteTypeName(Session.Item("jetnetWebSiteType")) + "' AND client_webInstanceID = " + Session.Item("webSiteInstanceID").ToString
          Else
            ' if NOT in standalone mode connect to master database registration info 
            sQuery = "SELECT * FROM Client_Register_Master WHERE client_regSecurityToken NOT IS NULL AND client_regType = 'C' AND client_regStatus = 'Y'"
            sQuery += " AND client_webDataLayer = '" + Application.Item("crmClientSiteData").dataLayerTypeName(Application.Item("crmClientSiteData").crmWebDataLayerType)
            sQuery += "' AND client_webHostName = '" + Application.Item("crmClientSiteData").crmClientHostName + "' and client_webSiteType = '"
            sQuery += Application.Item("crmClientSiteData").webSiteTypeName(Session.Item("jetnetWebSiteType")) + "' AND client_webInstanceID = " + Session.Item("webSiteInstanceID").ToString
          End If
        End If

        If eDatalayerTypes.MYSQL Then
          If Application.Item("crmClientSiteData").crmClientStandAloneMode Then
            ' connect to local client database for registration info
            sQuery1 = "SELECT * FROM Client_Register WHERE client_regSecurityToken IS NOT NULL AND client_regType = 'C' AND client_regStatus = 'Y'  AND client_webHostName = '" + Application.Item("crmClientSiteData").crmClientHostName + "' AND client_webInstanceID = " + Session.Item("webSiteInstanceID").ToString
          Else
            ' if NOT in standalone mode connect to master database registration info 
            sQuery1 = "SELECT * FROM Client_Register_Master WHERE client_regSecurityToken IS NOT NULL AND client_regType = 'C' AND client_regStatus = 'Y'  AND client_webHostName = '" + Application.Item("crmClientSiteData").crmClientHostName + "' AND client_webInstanceID = " + Session.Item("webSiteInstanceID").ToString
          End If
        Else
          If Application.Item("crmClientSiteData").crmClientStandAloneMode Then
            ' connect to local client database for registration info
            sQuery1 = "SELECT * FROM Client_Register WHERE client_regSecurityToken NOT IS NULL AND client_regType = 'C' AND client_regStatus = 'Y'  AND client_webHostName = '" + Application.Item("crmClientSiteData").crmClientHostName + "' AND client_webInstanceID = " + Session.Item("webSiteInstanceID").ToString
          Else
            ' if NOT in standalone mode connect to master database registration info 
            sQuery1 = "SELECT * FROM Client_Register_Master WHERE client_regSecurityToken NOT IS NULL AND client_regType = 'C' AND client_regStatus = 'Y'  AND client_webHostName = '" + Application.Item("crmClientSiteData").crmClientHostName + "' AND client_webInstanceID = " + Session.Item("webSiteInstanceID").ToString
          End If
        End If

        Try

          If Application.Item("crmClientSiteData").crmClientStandAloneMode Then
            MySqlConn.ConnectionString = Application.Item("crmClientDatabase").ToString.Trim
          Else
            MySqlConn.ConnectionString = HttpContext.Current.Application.Item("crmMasterDatabase").ToString.Trim
          End If

          MySqlConn.Open()

          MySqlCommand.Connection = MySqlConn
          MySqlCommand.CommandType = CommandType.Text
          MySqlCommand.CommandTimeout = 60

          MySqlCommand.CommandText = sQuery
          MySqlReader = MySqlCommand.ExecuteReader()

          If Not MySqlReader.HasRows Then
            MySqlReader.Close()
            MySqlCommand.CommandText = sQuery1
            MySqlReader = MySqlCommand.ExecuteReader()
          End If

          If Not MySqlReader.HasRows Then
            DisplayLogin = False
            LoginErrorString = "We're sorry, this is no longer a valid Marketplace Manager subscription. Please contact us to reinstate it."
          End If

          If MySqlReader.HasRows Then

            MySqlReader.Read()

            If Not (IsDBNull(MySqlReader("client_regSecurityToken"))) Then
              Session.Item("localUser").crmSecurityToken = MySqlReader.Item("client_regSecurityToken").ToString.Trim
            End If

            If Not (IsDBNull(MySqlReader("client_reg_sale_price_flag"))) Then
              Session.Item("localSubscription").crmSalesPriceIndex_Flag = IIf(MySqlReader("client_reg_sale_price_flag").ToString.ToLower.Trim = "y", True, False)
            End If

            If Not (IsDBNull(MySqlReader("client_reg_appraiser_flag"))) Then
              Session.Item("localSubscription").crmAppraiser_Flag = IIf(MySqlReader("client_reg_appraiser_flag").ToString.ToLower.Trim = "y", True, False)
            End If

            If Not (IsDBNull(MySqlReader("client_regSub_ID"))) Then
              Session.Item("localSubscription").crmSubscriptionID = MySqlReader.Item("client_regSub_ID")
            End If

            If Not (IsDBNull(MySqlReader("client_regSubscriptionCode"))) Then
              Session.Item("localUser").crmSubscriptionCode = objlocalUser.DecodeBase64(MySqlReader.Item("client_regSubscriptionCode").ToString.Trim)
            End If

            If Not (IsDBNull(MySqlReader("client_regInstallDate"))) Then
              Session.Item("localUser").crmSubInstallDate = objlocalUser.DecodeBase64(MySqlReader.Item("client_regInstallDate").ToString.Trim)
            End If

            If Not (IsDBNull(MySqlReader("client_regAccessDate"))) Then
              Session.Item("localUser").crmSubAccessDate = objlocalUser.DecodeBase64(MySqlReader.Item("client_regAccessDate").ToString.Trim)
            End If

            If Not (IsDBNull(MySqlReader("client_webUserLimit"))) Then
              Session.Item("localSubscription").crmMaxUserCount = CLng(MySqlReader.Item("client_webUserLimit"))
            Else
              Session.Item("localSubscription").crmMaxUserCount = 1
            End If

            If Not (IsDBNull(MySqlReader("client_regAerodexFlag"))) Then

              If MySqlReader.Item("client_regAerodexFlag").ToString.ToUpper.Trim = "Y" Then
                Session.Item("localSubscription").crmAerodexFlag = True
              Else
                Session.Item("localSubscription").crmAerodexFlag = False
              End If
            Else
              Session.Item("localSubscription").crmAerodexFlag = False
            End If

            If Not (IsDBNull(MySqlReader("client_regTierLevel"))) Then
              Session.Item("localSubscription").crmTierlevel = MySqlReader.Item("client_regTierLevel").ToString.ToUpper.Trim
            End If

            If Not (IsDBNull(MySqlReader("client_regProductCode"))) Then
              Session.Item("localSubscription").crmProductCode = MySqlReader.Item("client_regProductCode").ToString.ToUpper.Trim
            End If

            If Not (IsDBNull(MySqlReader("client_regFrequency"))) Then
              Session.Item("localSubscription").crmFrequency = MySqlReader.Item("client_regFrequency").ToString.ToUpper.Trim
            End If

            If Not (IsDBNull(MySqlReader("client_regDocumentsFlag"))) Then
              If MySqlReader.Item("client_regDocumentsFlag").ToString.ToUpper.Trim = "Y" Then
                Session.Item("localSubscription").crmDocumentsFlag = True
              Else
                Session.Item("localSubscription").crmDocumentsFlag = False
              End If
            Else
              Session.Item("localSubscription").crmDocumentsFlag = False
            End If

            MySqlReader.Close()

          End If 'MySqlReader.HasRows 

          MySqlReader.Dispose()

        Catch MySqlException

          commonLogFunctions.Log_User_Event_Data("UserError", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : MySqlException Thrown[" + MySqlException.Message.Trim + "]")

          MySqlConn.Dispose()
          MySqlCommand.Dispose()

        Finally

          MySqlConn.Close()
          MySqlCommand.Dispose()
          MySqlConn.Dispose()

        End Try

        HttpContext.Current.Session.Item("SubscriptionLogOn") = HttpContext.Current.Session.Item("localUser").crmSubscriptionCode
        HttpContext.Current.Session.Item("SubscriptionInstallDate") = HttpContext.Current.Session.Item("localUser").crmSubInstallDate
        HttpContext.Current.Session.Item("SubscriptionLastAccess") = HttpContext.Current.Session.Item("localUser").crmSubAccessDate

        ' if there is a local install needed for (crm only)
        If Not (String.IsNullOrEmpty(Session.Item("localUser").crmSubscriptionCode)) And Not (String.IsNullOrEmpty(Session.Item("localUser").crmSecurityToken)) Then

          Dim jetnet_query As String = ""

          Dim atemptable As New DataTable
          Dim update_client As Boolean = False
          Dim tier_level_jetnet As String = ""
          Dim product_code_jetnet As String = ""
          Dim aerodex_jetnet As Boolean = False

          jetnet_query = " SELECT top 1 sublogin_sub_id, sub_nbr_of_installs, subins_login,subins_seq_no, sub_starreports_flag,  "
          jetnet_query = jetnet_query & " sub_comp_id, subins_contact_id, subins_last_login_date,sub_start_date, sub_end_date, "
          jetnet_query = jetnet_query & " sub_business_aircraft_flag,sub_helicopters_flag,sub_commerical_flag, subins_last_logout_date, sub_yacht_flag, "
          jetnet_query = jetnet_query & " sub_aerodex_flag,sub_busair_tier_level, sub_frequency, sub_sale_price_flag, subins_last_session_date, "
          jetnet_query = jetnet_query & " sub_server_side_notes_flag, sub_server_side_dbase_name, sub_server_side_crm_regid "
          jetnet_query = jetnet_query & " FROM Subscription WITH(NOLOCK)"
          jetnet_query = jetnet_query & " INNER JOIN Subscription_Login WITH(NOLOCK) ON (sublogin_sub_id = sub_id)"
          jetnet_query = jetnet_query & " INNER JOIN Subscription_Install WITH(NOLOCK) ON (subins_sub_id = sub_id) and (sublogin_login=subins_login) "
          jetnet_query = jetnet_query & " WHERE sublogin_active_flag='Y'"
          jetnet_query = jetnet_query & " and sub_start_date <= '" & Month(Now()) & "/" & Day(Now()) & "/" & Year(Now()) & "'"
          jetnet_query = jetnet_query & " and (sub_end_date >= '" & Month(Now()) & "/" & Day(Now()) & "/" & Year(Now()) & "' or sub_end_date is NULL)"
          jetnet_query = jetnet_query & " and sublogin_sub_id = '" & Session.Item("localSubscription").crmSubscriptionID & "' "

          Try

            SqlConn.ConnectionString = HttpContext.Current.Application.Item("crmClientSiteData").AdminDatabaseConn

            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60


            SqlCommand.CommandText = jetnet_query
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
              atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
              Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
            End Try

            If Not IsNothing(atemptable) Then
              If atemptable.Rows.Count = 0 Then
                DisplayLogin = False
                LoginErrorString = "We're sorry, this Marketplace Manager Subscription is no longer valid. Please contact us to reinstate it."
              ElseIf atemptable.Rows.Count > 0 Then
                'Session.Item("CRMJetnetUserName") = ""
                If Not IsDBNull(atemptable.Rows(0).Item("subins_login")) Then
                  Session.Item("CRMJetnetUserName") = atemptable.Rows(0).Item("subins_login")
                End If

                Session.Item("localUser").crmSubAccessDate = Now().ToString

                If Not IsDBNull(atemptable.Rows(0).Item("sub_start_date")) Then
                  Session.Item("localUser").crmSubStartDate = CDate(atemptable.Rows(0).Item("sub_start_date"))
                End If

                If Not IsDBNull(atemptable.Rows(0).Item("sub_end_date")) Then
                  Session.Item("localUser").crmSubEndDate = CDate(atemptable.Rows(0).Item("sub_end_date"))
                Else
                  Session.Item("localUser").crmSubNoEndDate = True
                End If

                If Not IsDBNull(atemptable.Rows(0).Item("subins_seq_no")) Then
                  Session.Item("localUser").crmSubSeqNo = atemptable.Rows(0).Item("subins_seq_no")
                End If

                If Not IsDBNull(atemptable.Rows(0).Item("sub_comp_id")) Then
                  Session.Item("localUser").crmUserCompanyID = atemptable.Rows(0).Item("sub_comp_id")
                End If

                If Not IsDBNull(atemptable.Rows(0).Item("subins_contact_id")) Then
                  Session.Item("localUser").crmUserContactID = atemptable.Rows(0).Item("subins_contact_id")
                End If

                If Not IsDBNull(atemptable.Rows(0).Item("sub_nbr_of_installs")) Then
                  Session.Item("localSubscription").crmMaxUserCount = atemptable.Rows(0).Item("sub_nbr_of_installs")
                End If

                If Not IsDBNull(atemptable.Rows(0).Item("sub_busair_tier_level")) Then
                  If atemptable.Rows(0).Item("sub_busair_tier_level") = "1" Then
                    tier_level_jetnet = "J"
                  ElseIf atemptable.Rows(0).Item("sub_busair_tier_level") = "2" Then
                    tier_level_jetnet = "T"
                  ElseIf atemptable.Rows(0).Item("sub_busair_tier_level") = "3" Then
                    tier_level_jetnet = "ALL"
                  End If
                End If

                If Not IsDBNull(atemptable.Rows(0).Item("sub_busair_tier_level")) Then
                  Session.Item("localSubscription").crmTierlevel = tier_level_jetnet
                End If


                '''''''''''''''''''''''''''''''''''product code''''''''''''''''''''''''''''''''''''
                'unfortunately we store product codes in one field in the crm master database..
                'and three in fields in the jetnet database..

                If atemptable.Rows(0).Item("sub_helicopters_flag") = "Y" Then
                  product_code_jetnet = "H"
                End If

                If atemptable.Rows(0).Item("sub_business_aircraft_flag") = "Y" Then
                  If product_code_jetnet <> "" Then
                    product_code_jetnet = product_code_jetnet & ","
                  End If
                  product_code_jetnet = product_code_jetnet & "B"
                End If

                If atemptable.Rows(0).Item("sub_commerical_flag") = "Y" Then
                  If product_code_jetnet <> "" Then
                    product_code_jetnet = product_code_jetnet & ","
                  End If
                  product_code_jetnet = product_code_jetnet & "C"
                End If


                If atemptable.Rows(0).Item("sub_yacht_flag") = "Y" Then
                  If product_code_jetnet <> "" Then
                    product_code_jetnet = product_code_jetnet & ","
                  End If
                  product_code_jetnet = product_code_jetnet & "Y"
                End If

                Session.Item("localSubscription").crmProductCode = product_code_jetnet

                If Not IsDBNull(atemptable.Rows(0).Item("sub_frequency")) Then
                  Session.Item("localSubscription").crmFrequency = UCase(Left(atemptable.Rows(0).Item("sub_frequency"), 1)) & LCase(Mid(atemptable.Rows(0).Item("sub_frequency"), 2))
                End If

                If Not IsDBNull(atemptable.Rows(0).Item("sub_aerodex_flag")) Then
                  If atemptable.Rows(0).Item("sub_aerodex_flag") = "Y" Then
                    aerodex_jetnet = True
                  Else
                    aerodex_jetnet = False
                  End If
                Else
                  aerodex_jetnet = False
                End If

                Session.Item("localSubscription").crmAerodexFlag = aerodex_jetnet

              End If
            End If

            SqlReader.Close()

          Catch SqlException
            commonLogFunctions.Log_User_Event_Data("UserError", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : SqlException Thrown[" + SqlException.Message.Trim + "]")

          Finally
            SqlReader = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

          End Try

          sQuery = ""

          If Application.Item("crmClientSiteData").crmClientStandAloneMode Then
            ' connect to local client database for data connections for this host
            sQuery = "UPDATE Client_Register SET client_regSubscriptionCode = '" + Session.Item("localUser").EncodeBase64(Session.Item("localUser").crmSubscriptionCode) + "',"
            sQuery += " client_regSecurityToken = '" + Session.Item("localUser").crmSecurityToken + "',"
            sQuery += " client_regInstallDate = '" + Session.Item("localUser").EncodeBase64(Session.Item("localUser").crmSubInstallDate) + "',"
            sQuery += " client_regAccessDate = '" + Session.Item("localUser").EncodeBase64(Session.Item("localUser").crmSubAccessDate) + "',"
            sQuery += " client_webUserLimit = " + Session.Item("localSubscription").crmMaxUserCount.ToString + ","
            sQuery += " client_webInstanceID = " + Session.Item("webSiteInstanceID").ToString + ","
            sQuery += " client_regStatus = 'Y',"
            If Session.Item("localSubscription").crmAerodexFlag Then
              sQuery += " client_regAerodexFlag = 'Y',"
            Else
              sQuery += " client_regAerodexFlag = 'N',"
            End If
            sQuery += " client_regFrequency = '" + Session.Item("localSubscription").crmFrequency + "',"
            sQuery += " client_regTierLevel = '" + Session.Item("localSubscription").crmTierlevel + "',"
            sQuery += " client_regProductCode = '" + Session.Item("localSubscription").crmProductCode + "',"
            sQuery += " client_regJetnetCompanyID = " + Session.Item("localUser").crmUserCompanyID.ToString + ","
            sQuery += " client_webCurrentUsers = 0"
            sQuery += " WHERE client_webDataLayer = '" + Application.Item("crmClientSiteData").dataLayerTypeName(Application.Item("crmClientSiteData").crmWebDataLayerType)
            sQuery += "' AND client_webHostName = '" + Application.Item("crmClientSiteData").crmClientHostName + "' AND client_webSiteType = '"
            sQuery += Application.Item("crmClientSiteData").webSiteTypeName(Session.Item("jetnetWebSiteType")) + "' AND client_webInstanceID = " + Session.Item("webSiteInstanceID").ToString
            sQuery += " AND client_regType = 'C' AND client_regStatus = 'Y' limit 1"

          Else
            ' if NOT in standalone mode connect to master database for data connections for this host 
            sQuery = "UPDATE Client_Register_Master SET client_regSubscriptionCode = '" + Session.Item("localUser").EncodeBase64(Session.Item("localUser").crmSubscriptionCode) + "',"
            sQuery += " client_regSecurityToken = '" + Session.Item("localUser").crmSecurityToken + "',"
            sQuery += " client_regInstallDate = '" + Session.Item("localUser").EncodeBase64(Session.Item("localUser").crmSubInstallDate) + "',"
            sQuery += " client_regAccessDate = '" + Session.Item("localUser").EncodeBase64(Session.Item("localUser").crmSubAccessDate) + "',"
            sQuery += " client_webUserLimit = " + Session.Item("localSubscription").crmMaxUserCount.ToString + ","
            sQuery += " client_webInstanceID = " + Session.Item("webSiteInstanceID").ToString + ","
            sQuery += " client_regStatus = 'Y',"
            If Session.Item("localSubscription").crmAerodexFlag Then
              sQuery += " client_regAerodexFlag = 'Y',"
            Else
              sQuery += " client_regAerodexFlag = 'N',"
            End If
            sQuery += " client_regFrequency = '" + Session.Item("localSubscription").crmFrequency + "',"
            sQuery += " client_regTierLevel = '" + Session.Item("localSubscription").crmTierlevel + "',"
            sQuery += " client_regProductCode = '" + Session.Item("localSubscription").crmProductCode + "',"
            sQuery += " client_regJetnetCompanyID = " + Session.Item("localUser").crmUserCompanyID.ToString + ","
            sQuery += " client_webCurrentUsers = 0"
            sQuery += " WHERE client_webDataLayer = '" + Application.Item("crmClientSiteData").dataLayerTypeName(Application.Item("crmClientSiteData").crmWebDataLayerType)
            sQuery += "' AND client_webHostName = '" + Application.Item("crmClientSiteData").crmClientHostName + "' AND client_webSiteType = '"
            sQuery += Application.Item("crmClientSiteData").webSiteTypeName(Session.Item("jetnetWebSiteType")) + "' AND client_webInstanceID = " + Session.Item("webSiteInstanceID").ToString
            sQuery += " AND client_regType = 'C' AND client_regStatus = 'Y' limit 1"
          End If

          Try

            If Application.Item("crmClientSiteData").crmClientStandAloneMode Then
              MySqlConn.ConnectionString = Application.Item("crmClientDatabase")
            Else
              'If CBool(My.Settings.IsDebugMode) = True Then
              MySqlConn.ConnectionString = HttpContext.Current.Application.Item("crmMasterDatabase")
              'Else
              ' MySqlConn.ConnectionString = My.Settings.DEFAULT_LIVE_MYSQL.ToString
              ' End If
            End If

            MySqlConn.Open()

            MySqlCommand.Connection = MySqlConn
            MySqlCommand.CommandType = CommandType.Text
            MySqlCommand.CommandTimeout = 60

            MySqlCommand.CommandText = sQuery
            MySqlCommand.ExecuteNonQuery()

          Catch MySqlException

            commonLogFunctions.Log_User_Event_Data("UserError", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : MySqlException Thrown[" + MySqlException.Message.Trim + "]")

            MySqlConn.Dispose()
            MySqlCommand.Dispose()

          Finally

            MySqlCommand.Dispose()
            MySqlConn.Close()
            MySqlConn.Dispose()

          End Try


          If DisplayLogin = False Then
            lbl_inactive.Text = LoginErrorString
            lbl_inactive.BackColor = Drawing.Color.White
            lbl_inactive.CssClass = "display_block padding"
            logonUser.Visible = False
          Else
            logonUser.Visible = True
          End If
        Else 'Checking for a valid security token.
          If DisplayLogin = False Then
            lbl_inactive.BackColor = Drawing.Color.White
            lbl_inactive.CssClass = "display_block padding"
            lbl_inactive.Text = LoginErrorString
            logonUser.Visible = False
          End If
        End If

      End If

    Catch ex As Exception
      commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : Exception Thrown[" + ex.Message.Trim + "]")

    End Try

  End Sub

  Private Sub logonUser_UserLogonStatus(ByVal sender As Object, ByVal e As System.EventArgs) Handles logonUser.UserLogonStatus

    'let's figure this out!
    'figure out what database to use!
    Dim atemptable As New DataTable
    Dim aclsData_Temp As New clsData_Manager_SQL

    Try

      Dim hasLocalSQL As Boolean = CBool(My.Settings.hasLocalSQLServer.ToString)
      Dim useBackupSQL As Boolean = CBool(My.Settings.useBackupSQL_SRV.ToString)
      Dim bEnableEULA As Boolean = CBool(My.Settings.enableEULA.ToString)

      Dim continue_login As Boolean = False

      Dim login_date As New Nullable(Of System.DateTime)
      Dim logout_date As New Nullable(Of System.DateTime)
      Dim session_date As New Nullable(Of System.DateTime)
      Dim current_time As New Nullable(Of System.DateTime)

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br/>success logon get user connection start : " + Now.ToString + "<br />"

      If Response.IsClientConnected = True Then

        If Application.Item("crmClientSiteData").crmWebHostType = eWebHostTypes.HOMEBASE And Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Then ' TEMP SETUP FOR LOCAL TESTING
          Session.Item("localSubscription").crmFrequency = "test"
        End If

        atemptable = aclsData_Temp.Getting_Database_Connection(Session.Item("localSubscription").crmFrequency)

        If Not IsNothing(atemptable) Then
          If atemptable.Rows.Count > 0 Then

            If Session.Item("jetnetWebSiteType") <> eWebSiteTypes.LOCAL Then
              Application.Item("crmJetnetDatabase") = "Data Source=" + atemptable.Rows(0).Item("connectserver").ToString + ";Initial Catalog=" + atemptable.Rows(0).Item("connectdb").ToString + ";Persist Security Info=True;MultipleActiveResultSets=True;Asynchronous Processing=True;User ID=" + atemptable.Rows(0).Item("connectuser").ToString + ";Password=" + atemptable.Rows(0).Item("connectpw").ToString
            Else ' site is running from local machine (if localmachine has database use it, if not use one of the live sql databases)
              If hasLocalSQL Then
                Application.Item("crmJetnetDatabase") = My.Settings.DEFAULT_LOCAL_MSSQL.ToString
              Else
                If Application.Item("crmClientSiteData").crmWebHostType = eWebHostTypes.HOMEBASE Then
                  Application.Item("crmJetnetDatabase") = "Data Source=" + atemptable.Rows(0).Item("connectserver").ToString + ";Initial Catalog=" + atemptable.Rows(0).Item("connectdb").ToString + ";Persist Security Info=True;MultipleActiveResultSets=True;Asynchronous Processing=True;User ID=" + atemptable.Rows(0).Item("connectuser").ToString + ";Password=" + atemptable.Rows(0).Item("connectpw").ToString
                Else
                  If Not useBackupSQL Then
                    Application.Item("crmJetnetDatabase") = "Data Source=tcp:172.30.5.39,1433;Initial Catalog=" + atemptable.Rows(0).Item("connectdb").ToString + ";Persist Security Info=False;MultipleActiveResultSets=True;Asynchronous Processing=True;User ID=" + atemptable.Rows(0).Item("connectuser").ToString + ";Password=" + atemptable.Rows(0).Item("connectpw").ToString
                  Else
                    Application.Item("crmJetnetDatabase") = "Data Source=tcp:172.30.5.42,1433;Initial Catalog=" + atemptable.Rows(0).Item("connectdb").ToString + ";Persist Security Info=False;MultipleActiveResultSets=True;Asynchronous Processing=True;User ID=" + atemptable.Rows(0).Item("connectuser").ToString + ";Password=" + atemptable.Rows(0).Item("connectpw").ToString
                  End If

                End If
              End If
            End If

          Else ' if we cant find user connection default to live connections

            If hasLocalSQL Then
              Application.Item("crmJetnetDatabase") = My.Settings.DEFAULT_LOCAL_MSSQL
            Else
              If Not useBackupSQL Then
                Application.Item("crmJetnetDatabase") = My.Settings.DEFAULT_LIVE_MSSQL
              Else
                Application.Item("crmJetnetDatabase") = My.Settings.DEFAULT_LIVE_MSSQL_BK
              End If
            End If

          End If
        End If

        'Setting the application variable into session.
        HttpContext.Current.Session.Item("jetnetClientDatabase") = Application.Item("crmJetnetDatabase")

        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br/>success logon get user connection end : " + Now.ToString + "<br />"

        'Me.validateUser.Visible = Truec
        logonUser.Visible = True

        HttpContext.Current.Session.Item("crmUserLogon") = True

        current_time = DateAdd(DateInterval.Minute, -10, Now())

        If HttpContext.Current.Session.Item("localUser").crmEvo = True Then

          If Not IsDBNull(HttpContext.Current.Session.Item("localSubscription").crmSubinst_last_session_date) Then
            session_date = HttpContext.Current.Session.Item("localSubscription").crmSubinst_last_session_date
          End If
          If Not IsDBNull(HttpContext.Current.Session.Item("localSubscription").crmSubinst_last_login_date) Then
            login_date = HttpContext.Current.Session.Item("localSubscription").crmSubinst_last_login_date
          End If
          If Not IsDBNull(HttpContext.Current.Session.Item("localSubscription").crmSubinst_last_logout_date) Then
            logout_date = HttpContext.Current.Session.Item("localSubscription").crmSubinst_last_logout_date
          End If

          If HttpContext.Current.Session.Item("localUser").crmUser_API_Login = False Then
            If session_date <= current_time Then
              continue_login = True
            ElseIf logout_date = session_date Then
              continue_login = True
            ElseIf IsNothing(session_date) Then
              continue_login = True
            Else
              continue_login = False
            End If
          Else
            continue_login = True
          End If

        Else

          Dim ReturnTable As New DataTable
          Dim timezoneTable As New DataTable
          Dim returnInt As New Integer

          '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          'appended on 1/17/2013
          'first rule of business, we need to check and see if there even is a row in the database for this user.
          'We'll poll this by basically checking the crm_central client DB
          ReturnTable = aclsData_Temp.Get_CRM_Central_Client_User(CInt(Session.Item("localUser").crmLocalUserID), CLng(Session.Item("masterRecordID")))

          If Not IsNothing(ReturnTable) Then
            If ReturnTable.Rows.Count > 0 Then
              'This means that the row is there, so we're all good, we do not need to insert one.
              'However we do not need to update the GUID here, it comes later if they decide to actually log in!

              If Not IsDBNull(ReturnTable.Rows(0).Item("cliuser_jetnet_contact_id")) Then
                If Not String.IsNullOrEmpty(ReturnTable.Rows(0).Item("cliuser_jetnet_contact_id").ToString.Trim) Then
                  If IsNumeric(ReturnTable.Rows(0).Item("cliuser_jetnet_contact_id").ToString) Then

                    If CLng(ReturnTable.Rows(0).Item("cliuser_jetnet_contact_id").ToString) > 0 Then

                      aclsData_Temp.client_DB = Application.Item("crmClientDatabase")

                      returnInt = aclsData_Temp.Update_CRM_Central_Client_User(CInt(Session.Item("localUser").crmLocalUserID), CLng(Session.Item("masterRecordID")), HttpContext.Current.Session.Item("CRMJetnetUserName").ToString.Trim, Session.Item("localUser").crmSubSeqNo, Session.Item("localUser").crmUserContactID)

                      Session.Item("isEVOLOGGING") = True

                    ElseIf CLng(ReturnTable.Rows(0).Item("cliuser_jetnet_contact_id").ToString) = 0 Then
                      ' lets see if we can map an EVO user to the MPM user
                      atemptable = New DataTable
                      Dim sQuery = New StringBuilder()

                      Dim SqlConn As New SqlClient.SqlConnection
                      Dim SqlCommand As New SqlClient.SqlCommand
                      Dim SqlReader As SqlClient.SqlDataReader
                      Dim SqlException As SqlClient.SqlException : SqlException = Nothing

                      Try

                        sQuery.Append("SELECT DISTINCT sublogin_login, subins_seq_no, subins_contact_id FROM View_JETNET_Customers")
                        sQuery.Append(" WHERE contact_email_address = '" + Session.Item("localUser").crmLocalUserEmailAddress.ToString + "'")
                        sQuery.Append(" AND sub_id = " + HttpContext.Current.Session.Item("localSubscription").crmSubscriptionID.ToString)

                        SqlConn.ConnectionString = HttpContext.Current.Application.Item("crmClientSiteData").AdminDatabaseConn

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
                        End Try

                        If Not IsNothing(atemptable) Then

                          If atemptable.Rows.Count > 0 Then

                            For Each r As DataRow In atemptable.Rows

                              If Not IsDBNull(atemptable.Rows(0).Item("sublogin_login")) Then
                                HttpContext.Current.Session.Item("CRMJetnetUserName") = atemptable.Rows(0).Item("sublogin_login")
                              End If

                              If Not IsDBNull(r.Item("subins_seq_no")) Then
                                Session.Item("localUser").crmSubSeqNo = r.Item("subins_seq_no")
                              End If

                              If Not IsDBNull(r.Item("subins_contact_id")) Then
                                Session.Item("localUser").crmUserContactID = r.Item("subins_contact_id")
                              End If

                            Next

                            aclsData_Temp.client_DB = Application.Item("crmClientDatabase")

                            returnInt = aclsData_Temp.Update_CRM_Central_Client_User(CInt(Session.Item("localUser").crmLocalUserID), CLng(Session.Item("masterRecordID")), HttpContext.Current.Session.Item("CRMJetnetUserName").ToString.Trim, Session.Item("localUser").crmSubSeqNo, Session.Item("localUser").crmUserContactID)

                            ReturnTable = aclsData_Temp.Get_CRM_Central_Client_User(CInt(Session.Item("localUser").crmLocalUserID), CLng(Session.Item("masterRecordID")))

                            Session.Item("isEVOLOGGING") = True

                          End If

                        End If

                        SqlReader.Close()

                      Catch SqlException
                        commonLogFunctions.Log_User_Event_Data("UserError", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : MySqlException Thrown[" + SqlException.Message.Trim + "]")

                      Finally
                        SqlReader = Nothing

                        SqlConn.Dispose()
                        SqlConn.Close()
                        SqlConn = Nothing

                        SqlCommand.Dispose()
                        SqlCommand = Nothing

                      End Try

                    End If

                  End If ' IsNumeric(ReturnTable.Rows(0).Item("cliuser_jetnet_contact_id").ToString) Then
                End If ' Not String.IsNullOrEmpty(ReturnTable.Rows(0).Item("cliuser_jetnet_contact_id").ToString.Trim) Then
              End If ' Not IsDBNull(ReturnTable.Rows(0).Item("cliuser_jetnet_contact_id")) Then

            Else

              'this means that the row is not there. We are not all good. We need to insert one now.
              'It's okay to  insert the guid here.
              returnInt = aclsData_Temp.Insert_Blank_CRM_Central_Client_User(CInt(Session.Item("localUser").crmLocalUserID), CLng(Session.Item("masterRecordID")), Session.Item("localUser").crmGUID, Session.Item("localUser").crmLocalUserFirstName, Session.Item("localUser").crmLocalUserLastName, Session.Item("localUser").crmUserLogin, Session.Item("localUser").crmLocalUserEmailAddress)

              ReturnTable = aclsData_Temp.Get_CRM_Central_Client_User(CInt(Session.Item("localUser").crmLocalUserID), CLng(Session.Item("masterRecordID")))

              If ReturnTable.Rows.Count > 0 Then
                'This means that the row is there, so we're all good, we do not need to insert one.
                'However we do not need to update the GUID here, it comes later if they decide to actually log in!

                If Not IsDBNull(ReturnTable.Rows(0).Item("cliuser_jetnet_contact_id")) Then
                  If Not String.IsNullOrEmpty(ReturnTable.Rows(0).Item("cliuser_jetnet_contact_id").ToString.Trim) Then
                    If IsNumeric(ReturnTable.Rows(0).Item("cliuser_jetnet_contact_id").ToString) Then

                      If CLng(ReturnTable.Rows(0).Item("cliuser_jetnet_contact_id").ToString) > 0 Then

                        aclsData_Temp.client_DB = Application.Item("crmClientDatabase")

                        returnInt = aclsData_Temp.Update_CRM_Central_Client_User(CInt(Session.Item("localUser").crmLocalUserID), CLng(Session.Item("masterRecordID")), HttpContext.Current.Session.Item("CRMJetnetUserName").ToString.Trim, Session.Item("localUser").crmSubSeqNo, Session.Item("localUser").crmUserContactID)

                        Session.Item("isEVOLOGGING") = True

                      ElseIf CLng(ReturnTable.Rows(0).Item("cliuser_jetnet_contact_id").ToString) = 0 Then
                        ' lets see if we can map an EVO user to the MPM user
                        atemptable = New DataTable
                        Dim sQuery = New StringBuilder()

                        Dim SqlConn As New SqlClient.SqlConnection
                        Dim SqlCommand As New SqlClient.SqlCommand
                        Dim SqlReader As SqlClient.SqlDataReader
                        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

                        Try

                          sQuery.Append("SELECT DISTINCT sublogin_login, subins_seq_no, subins_contact_id FROM View_JETNET_Customers")
                          sQuery.Append(" WHERE contact_email_address = '" + Session.Item("localUser").crmLocalUserEmailAddress.ToString + "'")
                          sQuery.Append(" AND sub_id = " + HttpContext.Current.Session.Item("localSubscription").crmSubscriptionID.ToString)

                          SqlConn.ConnectionString = HttpContext.Current.Application.Item("crmClientSiteData").AdminDatabaseConn

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
                          End Try

                          If Not IsNothing(atemptable) Then

                            If atemptable.Rows.Count > 0 Then

                              If Not IsDBNull(atemptable.Rows(0).Item("sublogin_login")) Then
                                HttpContext.Current.Session.Item("CRMJetnetUserName") = atemptable.Rows(0).Item("sublogin_login")
                              End If

                              If Not IsDBNull(atemptable.Rows(0).Item("subins_seq_no")) Then
                                Session.Item("localUser").crmSubSeqNo = atemptable.Rows(0).Item("subins_seq_no")
                              End If

                              If Not IsDBNull(atemptable.Rows(0).Item("subins_contact_id")) Then
                                Session.Item("localUser").crmUserContactID = atemptable.Rows(0).Item("subins_contact_id")
                              End If

                              aclsData_Temp.client_DB = Application.Item("crmClientDatabase")

                              returnInt = aclsData_Temp.Update_CRM_Central_Client_User(CInt(Session.Item("localUser").crmLocalUserID), CLng(Session.Item("masterRecordID")), HttpContext.Current.Session.Item("CRMJetnetUserName").ToString.Trim, Session.Item("localUser").crmSubSeqNo, Session.Item("localUser").crmUserContactID)

                              ReturnTable = aclsData_Temp.Get_CRM_Central_Client_User(CInt(Session.Item("localUser").crmLocalUserID), CLng(Session.Item("masterRecordID")))

                              Session.Item("isEVOLOGGING") = True

                            End If

                          End If

                          SqlReader.Close()

                        Catch SqlException
                          commonLogFunctions.Log_User_Event_Data("UserError", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : MySqlException Thrown[" + SqlException.Message.Trim + "]")

                        Finally
                          SqlReader = Nothing

                          SqlConn.Dispose()
                          SqlConn.Close()
                          SqlConn = Nothing

                          SqlCommand.Dispose()
                          SqlCommand = Nothing

                        End Try

                      End If

                    End If ' IsNumeric(ReturnTable.Rows(0).Item("cliuser_jetnet_contact_id").ToString) Then
                  End If ' Not String.IsNullOrEmpty(ReturnTable.Rows(0).Item("cliuser_jetnet_contact_id").ToString.Trim) Then
                End If ' Not IsDBNull(ReturnTable.Rows(0).Item("cliuser_jetnet_contact_id")) Then

              End If
            End If

          End If


          '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          'Then each time a user logs in, we check the current users record in the client user table to determine if we think this user is 
          'still logged in elsewhere (based on how current the timedate stamp is and whether we know they were logged out. 
          'If we determine we think they are still logged in then we can ask the user if they want to login and terminate their 
          'previous login or cancel from the login process.
          'Check client_user table for current user login and retrieve last session date and last logout date.  
          '?	If last logout date is = last session date then just login as normal. This means that the user logged out normally to close last session so user is ok to login again.
          '?	If last logout date is < last session date or last logout date is NULL then there is a potential that the user is still logged in elsewhere. 
          '•	Check if last session date is within TIMEOUTPERIOD from current date/time then assume that user is still logged in.
          '•	Check if last session date is not within TIMEOUTPERIOD from current date/time then assume that the user closed his browser without logging out and is not currently online so user is ok to login again.
          '?	If user is still logged in then:
          '•	Tell the user that they system has detected that the system has detected that they are currently connected to the CRM via another session and ask if they desire to terminate the previous session and login anyways or cancel to leave previous session active.  If they choose to login anyways, then follow rules for user is ok to login (below).  If they choose to cancel the login then write a note to error log “User XXXXX cancelled login due to previous session.” And do not set any variables and take user back to login page.
          'String to grab User session/login information
          'Dim update_string As String = "select cliuser_last_session_date, cliuser_admin_flag, cliuser_end_date, cliuser_last_login_date, cliuser_last_logout_date from client_user WHERE (cliuser_id = " & CInt(Session.Item("localUser").crmLocalUserID) & ") limit 1"


          If ReturnTable.Rows.Count > 0 Then
            ''If MySqlReader.HasRows Then
            'MySqlReader.Read()
            If Not IsDBNull(ReturnTable.Rows(0).Item("cliuser_last_session_date")) Then
              session_date = ReturnTable.Rows(0).Item("cliuser_last_session_date")
            End If
            If Not IsDBNull(ReturnTable.Rows(0).Item("cliuser_last_login_date")) Then
              login_date = ReturnTable.Rows(0).Item("cliuser_last_login_date")
            End If
            If Not IsDBNull(ReturnTable.Rows(0).Item("cliuser_last_logout_date")) Then
              logout_date = ReturnTable.Rows(0).Item("cliuser_last_logout_date")
            End If
            'If Not IsDBNull(ReturnTable.Rows(0).Item("cliuser_admin_flag")) Then
            '    Session.Item("cliuserFlag") = ReturnTable.Rows(0).Item("cliuser_admin_flag")
            'End If
            'End If
          End If
          'Figuring out if user is logged in elsewhere.
          'True means continue login, they're okay
          'False means stop and show popup
          'If session_date >= current_time Then
          '    continue_login = True
          'Else
          '    If logout_date = session_date Or session_date >= current_time Then
          '        continue_login = True
          '    ElseIf (logout_date < session_date) Or IsNothing(logout_date) Then
          '        If session_date <= Now() And session_date >= current_time Then
          '            'Verify that the system will log you in without a message if the last session active date is over 10 minutes old
          '            continue_login = False
          '        Else
          '            continue_login = True
          '        End If
          '    End If
          'End If

          If session_date <= current_time Then
            continue_login = True
          ElseIf logout_date = session_date Then
            continue_login = True
          ElseIf IsNothing(session_date) Then
            continue_login = True
          Else
            continue_login = False
          End If

        End If

        If CType(Session.Item("jetnetWebHostType"), crmWebClient.eWebHostTypes) = eWebHostTypes.EVOLUTION Or
           CType(Session.Item("jetnetWebHostType"), crmWebClient.eWebHostTypes) = eWebHostTypes.YACHT Then

          If bEnableEULA Then

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br/>EVO/YACHT site success logon get eula start : " + Now.ToString + "<br />"
            Trace.Write("EVO/YACHT site success logon get eula start : " + Now.ToString)

            Dim sEulaText As String = ""
            Dim nEulaID As Long = 0
            Dim sEulaDate As String = ""

            ' get current eula
            commonEvo.Get_Current_Eula(nEulaID, sEulaDate, sEulaText)

            ' check to see if this user has "accepted" current eula
            If Not commonEvo.Check_Subscription_Eula(nEulaID) And Session.Item("localUser").crmUser_API_Login = False Then

              sEulaText = sEulaText.Replace("[DATETIME]", Now.ToShortDateString + vbCrLf)
              sEulaText = sEulaText.Replace("[COMPANYNAME]", commonEvo.get_company_name_fromID(CLng(HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString), 0, False, True, ""))
              Dim tempName = commonEvo.get_contact_info_fromID(CLng(HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString), CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString), 0, False, False, False, "")

              If Not String.IsNullOrEmpty(tempName) Then
                Dim nIndex As Integer = tempName.IndexOf("<br />")
                sEulaText = sEulaText.Replace("[CONTACTNAME]", tempName.Substring(0, nIndex).Trim)
              Else ' set generic user name
                sEulaText = sEulaText.Replace("[CONTACTNAME]", HttpContext.Current.Application.Item("crmClientSiteData").webSiteHostName(HttpContext.Current.Session.Item("jetnetWebHostType")).ToString + " USER")
              End If

              eulaText.Text = sEulaText
              eulaAgreement.CssClass = "modalPopup"
              If Session.Item("isMobile") = True Then
                eulaAgreement.CssClass = "modalPopupCSS"
              End If
              btnAccept.Focus()

              MPE1.Show() 'this shows the ajax modal popupcontrol and kills command connection

            Else
              HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br/>AutoAPILoginVariable1: " & Session.Item("localUser").crmUser_API_Login.ToString
              If continue_login = False Then

                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br/>EVO/YACHT site success logon accepted 'previous' eula 'Previous login' start : " + Now.ToString + "<br />"
                Trace.Write("EVO/YACHT site success logon accepted 'previous' eula 'Previous login' start : " + Now.ToString)

                max_user_warning.CssClass = "modalPopup"

                MPE.Show() 'this shows the ajax modal popupcontrol and kills command connection
                ' MPE.Enabled = False
                If Session.Item("isMobile") = True Then '//Amanda - 7/18/11. Toggling the visibility of two 
                  If Session.Item("localUser").crmEvo = False Then
                    ' size the eula for the phone ...
                    eulaAgreement.CssClass = "modalPopupCSS"
                    eulaAgreement.Width = 200

                    max_user_warning.Width = 150
                    max_user_warning.CssClass = "modalPopupCSS"
                    mobile_resize.Visible = True
                    regular_page_information.Visible = False
                    mobile_page_information.Visible = True
                  End If
                End If

              Else
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br/>AutoAPILoginVariable2: " & Session.Item("localUser").crmUser_API_Login.ToString
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br/>EVO/YACHT site success logon accepted 'previous' eula no 'Previous login' start : " + Now.ToString + "<br />"
                Trace.Write("EVO/YACHT site success logon accepted 'previous' eula no 'Previous login' start : " + Now.ToString)

                Update_User_Session()

              End If ' continue login

            End If ' check for eula 

          Else ' if eula is disabled

            If continue_login = False Then

              HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br/>EVO/YACHT site success logon eula *disabled* 'Previous login' start : " + Now.ToString + "<br />"
              Trace.Write("EVO/YACHT site success logon eula *disabled* 'Previous login' start : " + Now.ToString)

              max_user_warning.CssClass = "modalPopup"

              MPE.Show() 'this shows the ajax modal popupcontrol and kills command connection
              ' MPE.Enabled = False
              If Session.Item("isMobile") = True Then '//Amanda - 7/18/11. Toggling the visibility of two 
                If Session.Item("localUser").crmEvo = False Then
                  ' size the eula for the phone ...
                  eulaAgreement.CssClass = "modalPopupCSS"
                  ' eulaAgreement.Width = 200

                  max_user_warning.Width = 150
                  max_user_warning.CssClass = "modalPopupCSS"
                  mobile_resize.Visible = True
                  regular_page_information.Visible = False
                  mobile_page_information.Visible = True
                End If
              End If

            Else

              HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br/>EVO/YACHT site success logon eula *disabled* no 'Previous login' start : " + Now.ToString + "<br />"
              Trace.Write("EVO/YACHT site success logon eula *disabled* no 'Previous login' start : " + Now.ToString)

              Update_User_Session()

            End If ' continue login

          End If

        Else ' crm or admin site

          If continue_login = False Then

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br/>CRM/ADMIN/HOMEMASE site success 'Previous login' start : " + Now.ToString + "<br />"
            Trace.Write("CRM/ADMIN/HOMEMASE site success 'Previous login' start : " + Now.ToString)

            max_user_warning.CssClass = "modalPopup"

            MPE.Show() 'this shows the ajax modal popupcontrol and kills command connection
            ' MPE.Enabled = False
            If Session.Item("isMobile") = True Then '//Amanda - 7/18/11. Toggling the visibility of two 

              ' size the eula for the phone ...
              eulaAgreement.CssClass = "modalPopupCSS"
              'eulaAgreement.Width = 200

              max_user_warning.Width = 150
              max_user_warning.CssClass = "modalPopupCSS"
              mobile_resize.Visible = True
              regular_page_information.Visible = False
              mobile_page_information.Visible = True

            End If

          Else

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br/>CRM/ADMIN/HOMEMASE site success 'Previous login' start : " + Now.ToString + "<br />"
            Trace.Write("CRM/ADMIN/HOMEMASE site success no 'Previous login' start : " + Now.ToString)

            If CType(Session.Item("jetnetWebHostType"), crmWebClient.eWebHostTypes) = eWebHostTypes.CRM Then
              aclsData_Temp.Insert_CRM_Event("LOGIN", Application.Item("crmClientSiteData").crmClientHostName, "This user has logged in.", Session.Item("localUser").crmLocalUserName)
            End If

            Update_User_Session()

          End If ' continue login

        End If ' is evo or yacht site

      End If ' is client connected

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br/>end login : " + Now.ToString + "<br />"
      Trace.Write("End UserLogonStatus Default.aspx" + Now.ToString)

    Catch ex As Exception
      commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : Exception Thrown[" + ex.Message.Trim + "]")

    End Try

  End Sub

  Private Sub logonUser_UserLogonFailed(ByVal sender As Object, ByVal e As System.EventArgs) Handles logonUser.UserLogonFailed

    Try

      logonUser.Visible = True
      HttpContext.Current.Session.Item("crmUserLogon") = False

    Catch ex As Exception
      commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : Exception Thrown[" + ex.Message.Trim + "]")

    End Try

  End Sub

  Public Sub Update_User_Session()

    Dim aclsTemp As New clsData_Manager_SQL
    Dim timezoneTable As New DataTable
    Dim clientExport As New DataTable

    Try

      ' re-generate the "session GUID" after each successfull login  
      Select Case CType(Session.Item("jetnetWebHostType"), crmWebClient.eWebHostTypes)

        Case eWebHostTypes.HOMEBASE
          Session.Item("localUser").crmGUID = "HMB-" + Guid.NewGuid().ToString

        Case eWebHostTypes.ADMIN
          Session.Item("localUser").crmGUID = "ADM-" + Guid.NewGuid().ToString

        Case eWebHostTypes.ABI
          Session.Item("localUser").crmGUID = "ABI-" + Guid.NewGuid().ToString

        Case eWebHostTypes.CRM
          Session.Item("localUser").crmGUID = "CRM-" + Guid.NewGuid().ToString

        Case eWebHostTypes.YACHT
          Session.Item("localUser").crmGUID = "YCT-" + Guid.NewGuid().ToString

        Case Else
          Session.Item("localUser").crmGUID = "EVO-" + Guid.NewGuid().ToString

      End Select

      If Session.Item("localUser").crmUser_API_Login = True Then
        Dim apiTable As New DataTable
        apiTable = GetAPIPreviousGuid(HttpContext.Current.Session.Item("localUser").crmUserLogin, HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmSubSeqNo)

        If Not IsNothing(apiTable) Then
          If apiTable.Rows.Count > 0 Then
            If Not IsDBNull(apiTable.Rows(0).Item("subins_session_guid")) Then
              Session.Item("localUser").crmGUID = apiTable.Rows(0).Item("subins_session_guid")
            End If
          End If
        End If

      End If

      Session.Item("jetnetUserGuid") = Session.Item("localUser").crmGUID.ToString

      Dim dtLastLoginDate As System.DateTime
      Dim dtLastSessionDate As System.DateTime
      Dim strDate As System.DateTime

      dtLastLoginDate = Now()
      dtLastSessionDate = dtLastLoginDate
      strDate = FormatDateTime(dtLastLoginDate, vbGeneralDate)
      Session.Item("TimeStamp") = Format(strDate, "yyyy-MM-dd H:mm:ss")

      If Session.Item("localUser").crmEvo = True Then

        If CType(Session.Item("jetnetWebHostType"), crmWebClient.eWebHostTypes) = eWebHostTypes.EVOLUTION Or
           CType(Session.Item("jetnetWebHostType"), crmWebClient.eWebHostTypes) = eWebHostTypes.YACHT Or
           CType(Session.Item("jetnetWebHostType"), crmWebClient.eWebHostTypes) = eWebHostTypes.HOMEBASE Then
          commonEvo.cleanTempFilesDirectory()
        End If

        'this only shows up for the evo WEBSITE.
        If CType(Session.Item("jetnetWebHostType"), crmWebClient.eWebHostTypes) = eWebHostTypes.EVOLUTION Then
          If (CBool(My.Settings.enableChat)) Then
            ChatManager.CheckAndInitChat(True, bEnableChat)
          End If
        End If

        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br/>start update user session : " + Now.ToString + "<br />"
        Trace.Write("Start Update_User_Session Default.aspx" + Now.ToString)

        aclsTemp.Update_Evo_Sub_Dates("main_login", strDate, HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmUserLogin, HttpContext.Current.Session.Item("localUser").crmSubSeqNo, HttpContext.Current.Session.Item("localUser").crmGUID)

        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br/>update user dates end : " + Now.ToString + "<br />"

        aclsTemp.JETNET_DB = Session.Item("jetnetClientDatabase")

        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br/>check faa date start : " + Now.ToString + "<br />"

        ''Checking the FAA date.
        Dim FAATable As New DataTable
        FAATable = aclsTemp.Get_FAA_Date()

        If Not IsNothing(FAATable) Then
          If FAATable.Rows.Count > 0 Then
            If Not IsDBNull(FAATable.Rows(0).Item("MaxDate")) Then
              If Not String.IsNullOrEmpty(FAATable.Rows(0).Item("MaxDate").ToString.Trim) Then
                HttpContext.Current.Session.Item("localSubscription").crmSubinst_FAA_data_date = FAATable.Rows(0).Item("MaxDate").ToString.Trim
              End If
            End If
          End If

          FAATable.Dispose()

        End If

        FAATable = Nothing

        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br/>check faa date end : " + Now.ToString + "<br />"

        commonEvo.FindDataAsOfDate()

      Else
        aclsTemp.client_DB = Application.Item("crmClientDatabase") 'set client database
        aclsTemp.Update_GUID_Central_Client_User(CInt(Session.Item("localUser").crmLocalUserID), CLng(Session.Item("masterRecordID")), Session.Item("localUser").crmGUID, Session.Item("localUser").crmLocalUserFirstName, Session.Item("localUser").crmLocalUserLastName, Session.Item("localUser").crmUserLogin, Session.Item("localUser").crmLocalUserEmailAddress)
        aclsTemp.CRM_Central_Update_Client_User_Dates(CInt(Session.Item("localUser").crmLocalUserID), CLng(Session.Item("masterRecordID")), "Y", "main_login", Now())

        aclsTemp.JETNET_DB = Application.Item("crmJetnetDatabase")
        'Setting flight data to be shown as FAA.   Session.Item("localSubscription").crmSubscriptionID
        Session.Item("useFAAFlightData") = "FAA"

        If Session.Item("isEVOLOGGING") Then
          aclsTemp.Update_Evo_Sub_Dates("main_login", strDate, HttpContext.Current.Session.Item("localSubscription").crmSubscriptionID, HttpContext.Current.Session.Item("CRMJetnetUserName").ToString.Trim, HttpContext.Current.Session.Item("localUser").crmSubSeqNo, HttpContext.Current.Session.Item("localUser").crmGUID)
        End If

        ''Checking the FAA date for CRM.
        Dim FAATable As New DataTable
        FAATable = aclsTemp.Get_FAA_Date()

        If Not IsNothing(FAATable) Then
          If FAATable.Rows.Count > 0 Then
            If Not IsDBNull(FAATable.Rows(0).Item("MaxDate")) Then
              HttpContext.Current.Session.Item("localSubscription").crmSubinst_FAA_data_date = FAATable.Rows(0).Item("MaxDate")
            End If
          End If
        End If

        FAATable.Dispose()


        'we need to set the timezone here.
        Try
          timezoneTable = aclsTemp.Get_Client_Timezone(CInt(Session("timezone")))
          If Not IsNothing(timezoneTable) Then
            If timezoneTable.Rows.Count > 0 Then
              For Each r As DataRow In timezoneTable.Rows
                Session("timezone_offset") = r("clitzone_time_vs_eastern")
              Next
            Else
              Session("timezone_offset") = 0
            End If
          End If
        Catch ex As Exception
          Session("timezone_offset") = 0
        End Try
        'we need to set the max client export here.
        Try
          clientExport = aclsTemp.Get_Client_Preferences()
          If Not IsNothing(clientExport) Then
            If clientExport.Rows.Count > 0 Then
              For Each r As DataRow In clientExport.Rows
                If Not IsDBNull(r("clipref_max_client_export")) Then
                  Session.Item("localUser").crmMaxClientExport = IIf(IsNumeric(r("clipref_max_client_export")), r("clipref_max_client_export"), 0)
                End If

              Next
            End If
          End If

          If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then
            'Place to put the clear temp files on CRM.
            crmWebClient.clsGeneral.clsGeneral.CleanTemporaryFilesWithPrefix()
          End If

        Catch ex As Exception
          Session.Item("localUser").crmMaxClientExport = 0
        End Try
      End If


      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br/>end update user session : " + Now.ToString + "<br />"
      Trace.Write("End Update_User_Session Default.aspx" + Now.ToString)

      If Session.Item("isMobile") = False Then

        Select Case Session.Item("jetnetAppVersion")
          Case Constants.ApplicationVariable.HOMEBASE
            If AutoSwapApplicationVariable Then
              'We need to check for URL
              RouteHomeBaseAdminAutoLoginURL()
            Else
              Response.Redirect("homebaseHome.aspx", False)
            End If

          Case Constants.ApplicationVariable.CUSTOMER_CENTER

            If AutoSwapApplicationVariable Then
              RouteHomeBaseAdminAutoLoginURL()
            Else
              Response.Redirect("adminHome.aspx", False)
            End If


          Case Constants.ApplicationVariable.EVO
            Dim HomePageURL As String = "/home.aspx"
            'If Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Or Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Then
            HomePageURL = "/home_tile.aspx"
            'End If

            Dim PopupFolder As New DataTable
            Dim ShowPopup As Boolean = False
            Dim PopupLink As String = ""
            Dim popupDate As String = Format(DateAdd(DateInterval.Day, -90, Now()), "MM/dd/yyyy")
            Dim popupDateInsert As String = Format(Now(), "MM/dd/yyyy")

            Session.Item("localUser").crmSubscriberNotices = False 'We're going to add this in, it was being set in the other welcome user control alerts. Defaulted to false, swapped to true only if the notice query returns something.
            'If Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Or Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Then
            PopupFolder = aclsTemp.Get_Jetnet_Notifications_Popup("R", HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmUserLogin, HttpContext.Current.Session.Item("localUser").crmSubSeqNo, popupDate)

            If Not IsNothing(PopupFolder) Then
              If PopupFolder.Rows.Count > 0 Then 'This user has a notification:
                Session.Item("localUser").crmSubscriberNotices = True 'This means they have a notice.
                includeJqueryTheme.Text = "<link rel=""Stylesheet"" type=""text/css"" href=""//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css"" />"
                includeJqueryTheme.Visible = True
                notificationText.Text += "<div class=""popupOverflow"">"
                If Not IsDBNull(PopupFolder.Rows(0).Item("evonot_description")) Then
                  notificationText.Text += "<p>" & PopupFolder.Rows(0).Item("evonot_description").ToString & "</p>"
                Else
                  notificationText.Text += "<p>" & PopupFolder.Rows(0).Item("evonot_announcement").ToString & "</p>"
                End If
                'Default
                'PopupLink = "onclick=""javascript:load('help.aspx?id=" & PopupFolder.Rows(0).Item("evonot_id").ToString & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');"" class=""float_right"">"
                'If Not IsDBNull(PopupFolder.Rows(0).Item("evonot_video")) Then
                '  PopupLink += "View Video"
                'Else
                '  PopupLink += "View More Details"
                'End If
                'PopupLink += "</a>"

                'If Not IsDBNull(PopupFolder.Rows(0).Item("evonot_doc_link")) Then
                '  If Not String.IsNullOrEmpty(PopupFolder.Rows(0).Item("evonot_doc_link")) Then
                '    'resetting this variable if a document is there:
                '    If InStr(PopupFolder.Rows(0).Item("evonot_doc_link"), "http://") > 0 Then
                '      PopupLink = "onclick=""javascript:load('" & PopupFolder.Rows(0).Item("evonot_doc_link").ToString & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');"" class=""float_right"">View Document</a>"
                '    Else
                '      PopupLink = "onclick=""javascript:load('" & "http://" & PopupFolder.Rows(0).Item("evonot_doc_link").ToString & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');"" class=""float_right"">View Document</a>"
                '    End If
                '  End If
                'End If
                notificationText.Text += "</div>"

                ShowPopup = True
                Dim modalPostbackScript As StringBuilder = New StringBuilder()
                modalPostbackScript.Append("jQuery(""#evoNotificationdialog"").dialog({")
                modalPostbackScript.Append("autoOpen: true,")
                modalPostbackScript.Append("show: {")
                modalPostbackScript.Append("effect: ""fade"",")
                modalPostbackScript.Append("duration: 500")
                modalPostbackScript.Append("},")
                modalPostbackScript.Append("modal: true,")
                modalPostbackScript.Append("dialogClass: ""welcomeUserPopup homePagePopup"",")
                modalPostbackScript.Append("minHeight: 300,")
                modalPostbackScript.Append("maxHeight: 300,")
                modalPostbackScript.Append("maxWidth: 800,")
                modalPostbackScript.Append("minWidth: 800,")
                modalPostbackScript.Append("draggable: false,")
                modalPostbackScript.Append("close: function( event, ui ) {window.location = """ & HomePageURL & """;document.body.style.cursor = 'wait';},")
                modalPostbackScript.Append("closeText:""X""")
                modalPostbackScript.Append("});")

                'Last thing needed to be done before popup displayed - marking as viewed:
                aclsTemp.InsertIntoSubscriptionNotifications(Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmUserLogin, Session.Item("localUser").crmSubSeqNo, PopupFolder.Rows(0).Item("evonot_id"), popupDateInsert, "R")
                'Now we show the popup
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "PopupRedirect", "window.onload = function() {" & modalPostbackScript.ToString & "}", True)
              End If
            End If

            Dim returnString As String = ""
            Dim FolderTable As New DataTable
            Dim FolderID As Long = 0
            FolderTable = GetEvolutionDefaultFolder(HttpContext.Current.Session.Item("localUser").crmUserLogin, HttpContext.Current.Session.Item("localUser").crmSubSubID, HttpContext.Current.Session.Item("localUser").crmSubSeqNo)
            If Not IsNothing(FolderTable) Then
              If FolderTable.Rows.Count > 0 Then
                For Each r As DataRow In FolderTable.Rows
                  FolderID = r("cfolder_id")
                  returnString = clsGeneral.clsGeneral.LookupDefaultFolder(aclsTemp, FolderID)
                  If returnString <> "" Then
                    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "FolderRedirect", "window.onload = function() {var loadScreen = document.getElementById(""" & folder_load.ClientID & """);if (loadScreen != null){ loadScreen.className = 'display_block'};document.body.style.cursor = 'wait';" & returnString & "}", True)
                  End If
                Next
              End If
            End If



            If returnString = "" And ShowPopup = False And Session.Item("localUser").crmUser_API_Login = False Then
              If Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Or Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Then
                Dim PageToRedirect As String = Trim(Request("pageURL"))
                If Not String.IsNullOrEmpty(PageToRedirect) Then
                  Response.Redirect(PageToRedirect, False)
                Else
                  Response.Redirect(HomePageURL, False)
                End If
              Else
                Response.Redirect(HomePageURL, False)
              End If
            ElseIf Session.Item("localUser").crmUser_API_Login = True Then
              Dim urlString As String = ""

              If Not Session.Item("localPreferences").loadUserSession("", CLng(Session.Item("localUser").crmSubSubID.ToString), Session.Item("localUser").crmUserLogin.ToString, CLng(Session.Item("localUser").crmSubSeqNo.ToString), CLng(Session.Item("localUser").crmUserContactID.ToString)) Then
                Response.Write("error in load preferences : ")
              End If


              Select Case Trim(Request("type"))
                Case "company"
                  urlString = "DisplayCompanyDetail.aspx?compid=" & Trim(Request("id"))
                  If Not IsNothing(Trim(Request("jid"))) Then
                    If Not String.IsNullOrEmpty(Trim(Request("jid"))) Then
                      urlString += "&jid=" & Trim(Request("jid"))
                    End If
                  End If

                  If Not IsNothing(Trim(Request("source"))) Then
                    If Not String.IsNullOrEmpty(Trim(Request("source"))) Then
                      urlString += "&source=" & Trim(Request("source"))
                    End If
                  End If


                  commonLogFunctions.InsertAPILog(HttpContext.Current.Session.Item("localUser").crmGUID, HttpContext.Current.Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmUserCompanyID, Session.Item("localUser").crmUserContactID, Session.Item("localUser").crmLocalUserName, Session.Item("localUser").crmLocalUserPswd, "Evolution Display Company", CLng(Trim(Request("id"))), 0, 0, 0, "ACCESS TO EVOLUTION FROM API")

                Case "ac"
                  urlString = "DisplayAircraftDetail.aspx?acid=" & Trim(Request("id"))

                  If Not IsNothing(Trim(Request("jid"))) Then
                    If Not String.IsNullOrEmpty(Trim(Request("jid"))) Then
                      urlString += "&jid=" & Trim(Request("jid"))
                    End If
                  End If

                  If Not IsNothing(Trim(Request("source"))) Then
                    If Not String.IsNullOrEmpty(Trim(Request("source"))) Then
                      urlString += "&source=" & Trim(Request("source"))
                    End If
                  End If


                  commonLogFunctions.InsertAPILog(HttpContext.Current.Session.Item("localUser").crmGUID, HttpContext.Current.Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmUserCompanyID, Session.Item("localUser").crmUserContactID, Session.Item("localUser").crmLocalUserName, Session.Item("localUser").crmLocalUserPswd, "Evolution Display Aircraft", 0, CLng(Trim(Request("id"))), 0, 0, "ACCESS TO EVOLUTION FROM API")

                Case "contact"
                  Dim CompanyID As Long = 0
                  Dim ContactID As Long = 0
                  Dim JournalID As Long = 0
                  Dim CRMSource As String = ""

                  If Not IsNothing(Trim(Request("compid"))) Then
                    If Not String.IsNullOrEmpty(Trim(Request("compid"))) Then
                      If IsNumeric(Trim(Request("compid"))) Then
                        CompanyID = Trim(Request("compid"))
                      End If
                    End If
                  End If

                  If Not IsNothing(Trim(Request("id"))) Then
                    If Not String.IsNullOrEmpty(Trim(Request("id"))) Then
                      ContactID = Trim(Request("id"))
                    End If
                  End If

                  If Not IsNothing(Trim(Request("jid"))) Then
                    If Not String.IsNullOrEmpty(Trim(Request("jid"))) Then
                      If IsNumeric(Trim(Request("jid"))) Then
                        JournalID = Trim(Request("jid"))
                      End If
                    End If
                  End If

                  If Not IsNothing(Trim(Request("source"))) Then
                    If Not String.IsNullOrEmpty(Trim(Request("source"))) Then
                      CRMSource = Trim(Request("source"))
                    End If
                  End If

                  If CompanyID = 0 Then
                    'Figure out company ID
                    FigureOutCompanyID(CRMSource, ContactID, JournalID, aclsTemp, CompanyID)
                  End If

                  urlString = "DisplayContactDetail.aspx?compid=" & CompanyID.ToString & "&conid=" & ContactID

                  If JournalID > 0 Then
                    urlString += "&jid=" & JournalID.ToString
                  End If
                  If CRMSource <> "" Then
                    urlString += "&source=" & CRMSource
                  End If

                  commonLogFunctions.InsertAPILog(HttpContext.Current.Session.Item("localUser").crmGUID, HttpContext.Current.Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmUserCompanyID, Session.Item("localUser").crmUserContactID, Session.Item("localUser").crmLocalUserName, Session.Item("localUser").crmLocalUserPswd, "Evolution Display Contact", CLng(ContactID), 0, CLng(ContactID), 0, "ACCESS TO EVOLUTION FROM API")
                Case Else
                  urlString = HomePageURL
              End Select

              Response.Redirect(urlString, False)

            End If
          Case Else
            Response.Redirect("home.aspx", False)
        End Select

      Else '//Amanda - Just added a simple change in redirect for mobile users - 7/18/11

        If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then
          Response.Redirect("sandbox.aspx", False)
        Else
          Response.Redirect("home.aspx", False)
        End If

      End If

    Catch ex As Exception
      commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : Exception Thrown[" + ex.Message.Trim + "]")

    End Try

  End Sub

  Private Sub RouteHomeBaseAdminAutoLoginURL()

    Try

      Dim PageToRedirect As String = Trim(Request("pageURL"))

      Dim defaultHomePage As String = ""

      If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
        defaultHomePage = "homebaseHome.aspx"
      Else
        defaultHomePage = "adminHome.aspx"
      End If

      If Not String.IsNullOrEmpty(PageToRedirect) Then
        Response.Redirect(PageToRedirect, False)
      Else
        Response.Redirect(defaultHomePage, False)
      End If

    Catch ex As Exception
      commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : Exception Thrown[" + ex.Message.Trim + "]")

    End Try

  End Sub

  Private Sub FigureOutCompanyID(ByVal crmSource As String, ByVal contactID As Long, ByVal journalID As Long, ByVal aclsData_Temp As clsData_Manager_SQL, ByRef CompanyID As Long)

    Dim ContactTable As New DataTable

    Try

      If crmSource <> "CLIENT" Then
        ContactTable = aclsData_Temp.ReturnContactInformationACDetails(journalID, contactID)
        If Not IsNothing(ContactTable) Then
          If ContactTable.Rows.Count > 0 Then
            CompanyID = ContactTable.Rows(0).Item("contact_comp_id")
          End If
        End If
      End If

    Catch ex As Exception
      commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : Exception Thrown[" + ex.Message.Trim + "]")

    End Try

    ContactTable = Nothing

  End Sub

  Private Function GetAPIPreviousGuid(ByVal login_variable As String, ByVal sub_id As Long, ByVal seqNo As Long) As DataTable
    Dim atemptable As New DataTable
    Dim sQuery As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      If sub_id <> 0 And login_variable <> "" And seqNo <> 0 Then
        sQuery += "select subins_session_guid from Subscription_Install with (NOLOCK)"
        sQuery += " where subins_sub_id = " & sub_id
        sQuery += " and subins_login='" & Trim(login_variable) & "'"
        sQuery += " and subins_seq_no=" & seqNo


        SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
        SqlConn.Open()
        SqlCommand.Connection = SqlConn


        SqlCommand.CommandText = sQuery
        SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
        SqlCommand.CommandType = CommandType.Text
        SqlCommand.CommandTimeout = 60


        Try
          atemptable.Load(SqlReader)
        Catch constrExc As System.Data.ConstraintException
          Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        End Try
      End If
    Catch ex As Exception
      GetAPIPreviousGuid = Nothing
      commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : Exception Thrown[" + ex.Message.Trim + "]")
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

  Private Function GetEvolutionDefaultFolder(ByVal login_variable As String, ByVal sub_id As Long, ByVal seqNo As Long) As DataTable
    Dim atemptable As New DataTable
    Dim sQuery As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      If sub_id <> 0 And login_variable <> "" And seqNo <> 0 Then
        sQuery = ""
        sQuery += "select top 1 * from Client_Folder with (NOLOCK)"
        sQuery += " where cfolder_sub_id = " & sub_id
        sQuery += " and cfolder_login='" & Trim(login_variable) & "'"
        sQuery += " and cfolder_seq_no=" & seqNo
        sQuery += " and cfolder_default_flag='Y'"

        SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
        SqlConn.Open()
        SqlCommand.Connection = SqlConn


        SqlCommand.CommandText = sQuery
        SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
        SqlCommand.CommandType = CommandType.Text
        SqlCommand.CommandTimeout = 60


        Try
          atemptable.Load(SqlReader)
        Catch constrExc As System.Data.ConstraintException
          Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        End Try
      End If
    Catch ex As Exception
      GetEvolutionDefaultFolder = Nothing
      commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : Exception Thrown[" + ex.Message.Trim + "]")
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

  Private Sub Okay_Button_ModalPopup(ByVal sender As Object, ByVal e As System.EventArgs) Handles OkButton.Click

    Try

      'If they say okay, log the login.
      If Session.Item("localUser").crmEvo = True Then
        Dim aclsData_Temp As New clsData_Manager_SQL
        Call commonLogFunctions.Log_User_Event_Data("UserLogoutForced", "User Clicked OK and logged out a different login", Nothing)
      End If

      Update_User_Session()

    Catch ex As Exception
      commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : Exception Thrown[" + ex.Message.Trim + "]")

    End Try

  End Sub

  Private Sub Accept_Button_ModalPopup(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccept.Click

    Try

      'If they say okay, log the login.
      If Session.Item("localUser").crmEvo = True Then

        commonEvo.Update_Subscription_Eula("Y")

        Dim aclsData_Temp As New clsData_Manager_SQL
        Call commonLogFunctions.Log_User_Event_Data("EULAAccepted", "User Clicked ACCEPT and accepted current eula agreement", Nothing)

      End If

      Update_User_Session()

    Catch ex As Exception
      commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : Exception Thrown[" + ex.Message.Trim + "]")

    End Try

  End Sub

  Private Sub Decline_Button_ModalPopup(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDecline.Click

    Try

      'If they say okay, log the login.
      If Session.Item("localUser").crmEvo = True Then

        commonEvo.Update_Subscription_Eula("N")

        Dim aclsData_Temp As New clsData_Manager_SQL
        Call commonLogFunctions.Log_User_Event_Data("EULADeclined", "User Clicked DECLINE and declined current eula agreement", Nothing)

      End If

    Catch ex As Exception
      commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : Exception Thrown[" + ex.Message.Trim + "]")

    End Try

  End Sub

  Public Function getJetnetCalendarDataTable() As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT evonot_release_date, evonot_title, evonot_announcement FROM Evolution_Notifications WITH(NOLOCK)")
      sQuery.Append(" WHERE evonot_release_type = 'JC' AND evonot_active_flag='Y' AND evonot_release_date >= GETDATE()")
      sQuery.Append(" ORDER BY evonot_release_date ASC")

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 120

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

    Catch ex As Exception

      commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : Exception Thrown[" + ex.Message.Trim + "]")

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

  Public Sub displayJetnetCalendar(ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim tmpString As String = ""

    Try

      htmlOut.Append("<div class=""calendarTable"">")

      results_table = getJetnetCalendarDataTable()

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then
          evoCalendar.Visible = True
          If Not Page.ClientScript.IsStartupScriptRegistered("ToggleClickScript") Then
            Dim ToggleClick As New StringBuilder

            ToggleClick.Append("$(function(){")
            ToggleClick.Append(" $('#calendarPopup').click(function() {")
            ToggleClick.Append("if ($(""#" & current_jetnet_events.ClientID & """).is("":hidden"")) {")
            ToggleClick.Append("$(""#" & current_jetnet_events.ClientID & """).show();")
            ToggleClick.Append("} else {")
            ToggleClick.Append("$(""#" & current_jetnet_events.ClientID & """).hide();")
            ToggleClick.Append("}")
            ToggleClick.Append("});")
            ToggleClick.Append("});")
            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ToggleClickScript", ToggleClick.ToString, True)
          End If

          htmlOut.Append("<table id=""defaultJetnetCalendarTable"" width=""100%"" cellpadding=""3"" cellspacing=""0"">")
          htmlOut.Append("<tr><td align=""center"" valign=""middle"" height=""28"" colspan=""2""><em>Email <a href=""mailto:training@jetnet.com"">training@jetnet.com</a> to sign up for JETNET training.</em></td></tr>")
          htmlOut.Append("<tr><td align=""center"" valign=""middle"" colspan=""2"">")
          htmlOut.Append("<div style=""text-align: center; padding: 2px; height: 100px; overflow: auto;"">")
          htmlOut.Append("<table id=""defaultJetnetCalendarTable"" width=""100%"" cellpadding=""3"" cellspacing=""0"">")

          For Each r As DataRow In results_table.Rows

            htmlOut.Append("<tr>")
            htmlOut.Append("<td align=""left"" valign=""middle"" style=""padding-left:4px; font-size: 12px;"">&#8226;&nbsp;")
            htmlOut.Append("<strong>" + FormatDateTime(r.Item("evonot_release_date").ToString.Trim, DateFormat.LongDate).Trim + "</strong>")
            htmlOut.Append("&nbsp;-&nbsp;" + HttpContext.Current.Server.HtmlEncode(r.Item("evonot_title").ToString.Trim) + "")

            'tmpString = ""

            'If Not IsDBNull(r.Item("evonot_announcement")) Then
            '  If Not String.IsNullOrEmpty(r.Item("evonot_announcement").ToString.Trim) Then
            '    tmpString = "&nbsp;-&nbsp;" + r.Item("evonot_announcement").ToString.Replace("<p>", "").Replace("</p>", "").Trim
            '  End If
            'End If

            'htmlOut.Append(IIf(Not String.IsNullOrEmpty(tmpString), tmpString, ""))
            htmlOut.Append("</td>")

            htmlOut.Append("</tr>")

          Next

          htmlOut.Append("</table></div></td></tr></table>")

        Else
          htmlOut.Append("<table id=""defaultJetnetCalendarTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr><td valign=""top"" align=""left""><br/>No Calendar Items To Display</td></tr></table>")
        End If
      Else
        htmlOut.Append("<table id=""defaultJetnetCalendarTable"" width=""100%"" cellpadding=""2"" cellspacing=""0""><tr><td valign=""top"" align=""left""><br/>No Calendar Items To Display</td></tr></table>")
      End If

      htmlOut.Append("</div>")

    Catch ex As Exception
      commonLogFunctions.forceLogError("ERROR", Reflection.MethodBase.GetCurrentMethod().ToString.Trim + " : Exception Thrown[" + ex.Message.Trim + "]")

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

End Class