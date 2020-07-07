Partial Public Class EmptyEvoTheme
  Inherits System.Web.UI.MasterPage
  Public aclsData_Temp As clsData_Manager_SQL
  Public bEnableChat As Boolean
  Public bChangeChat As Boolean
  Dim isHomebase As Boolean = False
  Dim isUserLogedIn As Boolean = False
  Dim isforgotToken As Boolean = False

  Public script_version As String = ""
  'Edits per call with Rick: 10/16
  'Removed the Empty Session Refresh.
  'Added login check back in.
  Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

    If Not IsNothing(HttpContext.Current.Request("homebase")) Then
      If Not String.IsNullOrEmpty(HttpContext.Current.Request("homebase").ToString.Trim) Then
        isHomebase = IIf(HttpContext.Current.Request("homebase").ToString.Trim.Contains("Y"), True, False)
      End If
    End If

    If Not IsNothing(HttpContext.Current.Session.Item("crmUserLogon")) Then
      If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("crmUserLogon").ToString.Trim) Then
        isUserLogedIn = CBool(HttpContext.Current.Session.Item("crmUserLogon").ToString.Trim)
      End If
    End If


    If Not IsNothing(HttpContext.Current.Request("forgotToken")) Then
      If Not String.IsNullOrEmpty(HttpContext.Current.Request("forgotToken").ToString.Trim) Then
        isforgotToken = True
      End If
    End If

    If Not IsNothing(HttpContext.Current.Request.Item("securityToken")) Then
      If Not String.IsNullOrEmpty(HttpContext.Current.Request.Item("securityToken").ToString.Trim) Then
        ValidateAPI(HttpContext.Current.Request.Item("securityToken").ToString.Trim)
      End If
    End If

        'If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Or HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Then
        SetContainerClass("container MaxWidthRemove") 'set full width page
        'End If


        aclsData_Temp = New clsData_Manager_SQL
    aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase")
    aclsData_Temp.client_DB = Session.Item("jetnetServerNotesDatabase")

    'Clears the debug string on each refresh.
    'Session.Item("localUser").crmUser_DebugText = ""

    Dim SMgr As ScriptManager
    If ScriptManager.GetCurrent(Page) Is Nothing Then
      Throw New Exception("ScriptManager not found.")
    Else
      SMgr = ScriptManager.GetCurrent(Page)
    End If

    script_version = My.Settings.SCRIPT_VERSION.ToString

    Dim link As HtmlLink = New HtmlLink()
    link.Attributes.Add("type", "text/css")
    link.Attributes.Add("rel", "stylesheet")
    link.Attributes.Add("href", "/EvoStyles/stylesheets/additional_styles.css" + script_version)
    Page.Header.Controls.Add(link)

    Dim link1 As HtmlLink = New HtmlLink()
    link1.Attributes.Add("type", "text/css")
    link1.Attributes.Add("rel", "stylesheet")
    link1.Attributes.Add("href", "/EvoStyles/stylesheets/tableThemes.css" + script_version)
    Page.Header.Controls.Add(link1)

    Dim link2 As HtmlLink = New HtmlLink()
    link2.Attributes.Add("type", "text/css")
    link2.Attributes.Add("rel", "stylesheet")
    link2.Attributes.Add("href", "/EvoStyles/stylesheets/header_styles.css" + script_version)
    Page.Header.Controls.Add(link2)

    Dim SRef As ScriptReference = New ScriptReference()
    SRef.Path = "https://cdnjs.cloudflare.com/ajax/libs/jquery/3.4.1/jquery.min.js"
    SMgr.Scripts.Add(SRef)

    Dim SRef1 As ScriptReference = New ScriptReference()
    SRef1.Path = "https://code.jquery.com/ui/1.12.1/jquery-ui.js"
    SMgr.Scripts.Add(SRef1)

    Dim SRef2 As ScriptReference = New ScriptReference()
    SRef2.Path = "https://cdnjs.cloudflare.com/ajax/libs/jquery-migrate/3.1.0/jquery-migrate.min.js"
    SMgr.Scripts.Add(SRef2)

    Dim SRef3 As ScriptReference = New ScriptReference()
    SRef3.Path = "~/common/jquery.select-to-autocomplete.min.js"
    SMgr.Scripts.Add(SRef3)

    Dim SRef4 As ScriptReference = New ScriptReference()
    SRef4.Path = "~/common/common_functions.js" + script_version
    SMgr.Scripts.Add(SRef4)

    Dim SRef5 As ScriptReference = New ScriptReference()
    SRef5.Path = "~/common/jquery.number.min.js"
    SMgr.Scripts.Add(SRef5)

    Dim SRef6 As ScriptReference = New ScriptReference()
    SRef6.Path = "~/common/date.js"
    SMgr.Scripts.Add(SRef6)

    Dim SRef7 As ScriptReference = New ScriptReference()
    SRef7.Path = "~/common/daterangepicker.jQuery.js" + script_version
    SMgr.Scripts.Add(SRef7)

    Dim SRef8 As ScriptReference = New ScriptReference()
    SRef8.Path = "~/common/header_scripts.js" + script_version
    SMgr.Scripts.Add(SRef8)

    Dim SRef9 As ScriptReference = New ScriptReference()
    SRef9.Path = "~/common/chosen.jquery.min.js"
    SMgr.Scripts.Add(SRef9)

    Dim SRef10 As ScriptReference = New ScriptReference()
    SRef10.Path = "~/common/jQDateRangeSlider-min.js"
    SMgr.Scripts.Add(SRef10)

    Dim SRef11 As ScriptReference = New ScriptReference()
    SRef11.Path = "~/abiFiles/js/superfish.min.js"
    SMgr.Scripts.Add(SRef11)

    Dim SRef12 As ScriptReference = New ScriptReference()
    SRef12.Path = "https://www.gstatic.com/charts/loader.js"
    SMgr.Scripts.Add(SRef12)

    Dim SRef13 = New ScriptReference()
    SRef13.Path = "~/common/jquery.slicknav.min.js"
    SMgr.Scripts.Add(SRef13)

    If Session.Item("isMobile") = True Then

      Dim link3 As HtmlLink = New HtmlLink()
      link3.Attributes.Add("type", "text/css")
      link3.Attributes.Add("rel", "stylesheet")
      link3.Attributes.Add("href", "https://cdn.datatables.net/responsive/2.1.1/css/responsive.dataTables.min.css")
      Page.Header.Controls.Add(link3)

      Dim SRef14 = New ScriptReference()
      SRef14.Path = "https://cdn.datatables.net/responsive/2.1.1/js/dataTables.responsive.min.js"
      SMgr.Scripts.Add(SRef14)

      homeButton.Visible = True

      Dim link4 As HtmlLink = New HtmlLink()
      link4.Attributes.Add("type", "text/css")
      link4.Attributes.Add("rel", "stylesheet")
      link4.Attributes.Add("href", "/EvoStyles/stylesheets/additional_mobile_styles.css" + script_version)
      Page.Header.Controls.Add(link4)

    End If

    If (CBool(My.Settings.enableChat)) And Not isHomebase And Not isforgotToken Then

      ChatManager.CheckAndInitChat(False, bEnableChat)

      If bEnableChat Or HttpContext.Current.Request.Path.ToString.Trim.ToLower.Contains("preferences.aspx") Then

        Dim SRef15 As ScriptReference = New ScriptReference()
        SRef15.Path = "~/chat/Scripts/chatUtility.js"
        SMgr.Scripts.Add(SRef15)

        Dim SRef16 As ScriptReference = New ScriptReference()
        SRef16.Path = "~/chat/Scripts/chatWithUser.js"
        SMgr.Scripts.Add(SRef16)

        Dim SvcRef As ServiceReference = New ServiceReference()
        SvcRef.Path = "~/chat/Services/chatServices.svc"
        SMgr.Services.Add(SvcRef)

      End If
    End If

    'WelcomeContainer.Attributes.Remove("class")
    'WelcomeContainer.Attributes.Add("class", "sixteen columns headerHeight")
    'belowWelcomeContainer.Attributes.Add("class", "headerHeightPadding")
    'logo.CssClass = "evolution_logo_standalone"

    Select Case Session.Item("jetnetAppVersion")
      Case Constants.ApplicationVariable.YACHT
        logo.ImageUrl = "~/images/JETNET_YachtSpot.png" 'swap logo
        logo.Attributes.Add("style", "filter: invert(1);height:35px !important;padding-top:20px !important;")
      Case Constants.ApplicationVariable.CUSTOMER_CENTER
        logo.ImageUrl = "~/images/JETNET_EvoAdmin_Outlines.png"
        logo.Attributes.Add("style", "filter: invert(1);")
      Case Constants.ApplicationVariable.CRM
        logo.Attributes.Add("style", "padding-top:1px !important;")
        logo.ImageUrl = "~/images/JETNET_MarketplaceMan.png" 'swap logo
        If Session.Item("localSubscription").crmSalesPriceIndex_Flag Then
          logo.ImageUrl = "~/images/MPM_Values.png"
        End If
      Case Else

        If Session.Item("isMobile") = False Then
          ShowHelpfulHint()
          setUpHotJar()

        End If


        googleAnalyticsScript.Visible = True
        If Session.Item("localSubscription").crmAerodexFlag Then
          If Session.Item("localSubscription").crmProductCode = "H" Then
            logo.ImageUrl = "~/images/JETNET_Rotodex.png" 'swap logo
          Else
            If UCase(Session.Item("localSubscription").crmFrequency) = "LIVE" Then
              logo.ImageUrl = "~/images/JETNET_AerodexElite_FINAL.png" 'swap logo
            Else
              logo.ImageUrl = "~/images/JETNET_Aerodex.png" 'swap logo
            End If
          End If
        Else
          If Session.Item("localSubscription").crmProductCode = "H" Then
            logo.ImageUrl = "~/images/JETNET_EvoMarketplace_White.png"
          Else
            'This has been changed
            logo.ImageUrl = "~/images/JETNET_EvoMarketplace_White.png" 'swap logo
            If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag Then
              If Session.Item("isMobile") = False Then
                logo.ImageUrl = "~/images/JETNET_MarketplaceValues.png"
                logo.CssClass = "evolution_ValuesLogo"
              End If
              If HttpContext.Current.Session.Item("localUser").crmUser_Evo_MPM_Flag Then 'This means users have MPM and values
                logo.ImageUrl = "~/images/JETNET_MPMandValues.png"
              End If
            Else
              If HttpContext.Current.Session.Item("localUser").crmUser_Evo_MPM_Flag Then 'This means users have MPM and not values
                logo.ImageUrl = "~/images/JETNET_MPMOnly.png"
              End If
            End If
          End If
        End If

    End Select
  End Sub

  Private Sub setUpHotJar()
    If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Or HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Then
      hotjarScriptTestEvo.Visible = True 'This enables the hotjar script on evo.
      hotjarScriptLiveEvo.Visible = False
    Else
      hotjarScriptLiveEvo.Visible = True
      hotjarScriptTestEvo.Visible = False
    End If
  End Sub

  Private Sub hintUpdateButton_Click(sender As Object, e As EventArgs) Handles hintUpdateButton.Click
    'hintTextUpdate.Text = hintTextUpdate.Text
    Dim popupDateInsert As String = Format(Now(), "MM/dd/yyyy")

    Dim subID As Long = 0
    Dim login As String = ""
    Dim seqNo As Long = 0
    Dim noteID As Long = 0

    If Not IsNothing(HttpContext.Current.Session.Item("localUser").crmSubSubID) Then
      If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString.Trim) Then
        If IsNumeric(HttpContext.Current.Session.Item("localUser").crmSubSubID) Then
          subID = CLng(HttpContext.Current.Session.Item("localUser").crmSubSubID)
        End If
      End If
    End If

    If Not IsNothing(HttpContext.Current.Session.Item("localUser").crmUserLogin) Then
      If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localUser").crmUserLogin.Trim) Then
        login = HttpContext.Current.Session.Item("localUser").crmUserLogin.Trim
      End If
    End If

    If Not IsNothing(HttpContext.Current.Session.Item("localUser").crmSubSeqNo) Then
      If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString.Trim) Then
        If IsNumeric(HttpContext.Current.Session.Item("localUser").crmSubSeqNo) Then
          seqNo = CLng(HttpContext.Current.Session.Item("localUser").crmSubSeqNo)
        End If
      End If
    End If

    If Not String.IsNullOrEmpty(hintTextUpdate.Text.Trim) Then
      If IsNumeric(hintTextUpdate.Text) Then
        noteID = CLng(hintTextUpdate.Text)
      End If
    End If

    aclsData_Temp.InsertIntoSubscriptionNotifications(subID, login, seqNo, noteID, popupDateInsert, "R")

  End Sub

  Private Sub hintUpdateClickLink_Click(sender As Object, e As EventArgs) Handles hintUpdateClickLink.Click
    'hintTextUpdate.Text = hintTextUpdate.Text
    Dim popupDateInsert As String = Format(Now(), "MM/dd/yyyy")
    Dim subID As Long = 0
    Dim login As String = ""
    Dim seqNo As Long = 0
    Dim noteID As Long = 0

    If Not IsNothing(HttpContext.Current.Session.Item("localUser").crmSubSubID) Then
      If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString.Trim) Then
        If IsNumeric(HttpContext.Current.Session.Item("localUser").crmSubSubID) Then
          subID = CLng(HttpContext.Current.Session.Item("localUser").crmSubSubID)
        End If
      End If
    End If

    If Not IsNothing(HttpContext.Current.Session.Item("localUser").crmUserLogin) Then
      If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localUser").crmUserLogin.Trim) Then
        login = HttpContext.Current.Session.Item("localUser").crmUserLogin.Trim
      End If
    End If

    If Not IsNothing(HttpContext.Current.Session.Item("localUser").crmSubSeqNo) Then
      If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString.Trim) Then
        If IsNumeric(HttpContext.Current.Session.Item("localUser").crmSubSeqNo) Then
          seqNo = CLng(HttpContext.Current.Session.Item("localUser").crmSubSeqNo)
        End If
      End If
    End If

    If Not String.IsNullOrEmpty(hintTextUpdate.Text.Trim) Then
      If IsNumeric(hintTextUpdate.Text) Then
        noteID = CLng(hintTextUpdate.Text)
      End If
    End If

    aclsData_Temp.InsertIntoSubscriptionNotifications(subID, login, seqNo, noteID, popupDateInsert, "C")

  End Sub

  Public Sub ShowHelpfulHint()
    Dim pageName As String = ""
    Dim ShowView As Boolean = False
    Dim ViewIDStr As String = ""
    DisplayFunctions.ShowHelpfulHintPage(pageName, ShowView, ViewIDStr) 'Figures out the parameters based on what page you're on.
    If Not String.IsNullOrEmpty(pageName) Or ShowView = True Then
      DisplayFunctions.DisplayHelpfulHint(pageName, ShowView, ViewIDStr, hintText, aclsData_Temp, hintPopupHolder, hintTextUpdate) 'Actually displays the hint.
    End If
  End Sub

  Public Sub SetContainerClass(ByVal ClassName As String)
    pageSizing.Attributes.Remove("class")
    pageSizing.Attributes.Add("class", ClassName)
  End Sub
  Public Sub AddExtraButtons(ByVal ExtraButtonsString As String)
    extra_buttons.Text = (ExtraButtonsString)
  End Sub

  Public Sub SetFormClass(ByVal className As String)
    aspnetForm.Attributes.Add("class", className)
  End Sub
  Public Sub UpdateHelpLink(ByVal url As String)
    updateHelp.Text = "<a href=""" & url & """ target=""blank""><img src=""images/help-circle.svg"" alt=""Help"" /></a>"
  End Sub
  Public Sub RemoveLine()
    fixedBar.Attributes.Add("class", "FixedHeaderBar noBorder")
    belowWelcomeContainer.Attributes.Add("class", "headerHeightPadding noBorder")
  End Sub

  'Private Sub Configure_Background()
  '    Dim TempTable As New DataTable
  '    Trace.Write("Start Configure_Background EvoTheme.Master.vb" + Now.ToString)

  '    If Not IsNothing(Session.Item("localUser").crmLocalUser_Background_ID) Then
  '        If Session.Item("localUser").crmLocalUser_Background_ID = 0 Then 'this means the background is random, this needs to be looked up once, and then set.
  '            If Not Page.IsPostBack Then
  '                TempTable = aclsData_Temp.GetBackgroundImages()
  '                If Not IsNothing(TempTable) Then
  '                    If TempTable.Rows.Count > 0 Then
  '                        Session.Item("localUser").crmLocalUser_Background_ID = TempTable.Rows(0).Item("evoback_id")
  '                    End If
  '                End If
  '            End If
  '        End If
  '    End If
  '    If Not IsNothing(Session.Item("localUser").crmLocalUser_Background_ID) Then
  '        If Session.Item("localUser").crmLocalUser_Background_ID <> 0 Then
  '            background_image.Text = "<img src=""images/background/" + Session.Item("localUser").crmLocalUser_Background_ID.ToString + ".jpg"" alt="""" class=""bg_image"" />"
  '        End If
  '    End If
  '    Trace.Write("End Configure_Background EmptyEvoTheme.Master.vb" + Now.ToString)

  ' End Sub


  Public Sub Swap_Logo_Image(ByVal url As String)
    logo.ImageUrl = url
  End Sub
  ''' <summary>
  ''' This is blank until the logging is put in
  ''' </summary>
  ''' <param name="ex"></param>
  ''' <remarks></remarks>
  Public Sub LogError(ByVal ex As String)
    'Error Logging Function
    Call commonLogFunctions.Log_User_Event_Data("UserError", Replace(Request.Url.AbsolutePath, "/", "") & " : " & Replace(ex, "'", "''"), Nothing, 0, 0, 0, 0, 0)
  End Sub

  Public Sub SetPageText(ByVal text As String)
    PageText.Text = text
  End Sub

  Public Sub SetPageTitle(ByVal title As String)
    PageText.Text = title
    Me.Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title(title)
  End Sub

  'This is going to remove the background image.
  'If you ever need a very basic page with no added styles
  'Public Sub RemoveBackgroundImage(ByVal removeImage As Boolean)
  '    If removeImage Then
  '        background_image.Text = ""
  '    Else
  '        If Not IsNothing(Session.Item("localUser").crmLocalUser_Background_ID) Then
  '            If Session.Item("localUser").crmLocalUser_Background_ID <> 0 Then
  '                background_image.Text = "<img src=""images/background/" + Session.Item("localUser").crmLocalUser_Background_ID.ToString + ".jpg"" alt="""" class=""bg_image"" />"
  '            End If
  '        End If
  '    End If
  'End Sub

  'If you need a quick function to allow the masterpage to just be a blank screen
  'Use this and pass true. If you need to reset it, pass false.
  Public Sub RemoveAllStyleElements(ByVal removeStyle As Boolean)
    'RemoveBackgroundImage(removeStyle)
    RemoveSizes(removeStyle)
    removeFooter(removeStyle)
    RemoveBackgroundColor(removeStyle)
    RemoveLogo(removeStyle)
    TurnOffPageHeader(removeStyle)
    WelcomeContainer.Attributes.Remove("class")
    belowWelcomeContainer.Attributes.Remove("class")
    fixedBar.Visible = False
  End Sub

  'This function allows you to remove the logo from this master page.
  'Or put it back, if for some reason you need to do so.
  Private Sub RemoveLogo(ByVal removeItem As Boolean)
    If removeItem Then
      logo.Visible = False
    Else
      logo.Visible = False
    End If
  End Sub

  'This will toggle the page header if it isn't needed.
  Public Sub TurnOffPageHeader(ByVal removeItem As Boolean)
    If removeItem Then
      PageText.Visible = False
    Else
      PageText.Visible = False
    End If
  End Sub
  'This function will allow you to toggle off/on the footer.
  Private Sub removeFooter(ByVal removeItem As Boolean)
    footer.Attributes.Remove("class")
    footer.Attributes.Add("class", IIf(removeItem, "display_none", "sixteen columns footer"))
  End Sub

  'This footer will get rid of the white background color of the page.
  'or put it back on depending on parameter
  Private Sub RemoveBackgroundColor(ByRef removeColor As Boolean)
    bodyTag.Attributes.Remove("class")
    bodyTag.Attributes.Add("class", IIf(removeColor, "noBackgroundColor", ""))
  End Sub

  'This function will get rid of the css grid styling that's used on this page
  'Or put it back, allowing you to toggle. 
  Public Sub RemoveSizes(ByVal removeClass As Boolean)
    If removeClass Then
      container_white_background_div.Attributes.Remove("class")
      pageSizing.Attributes.Remove("class")
    Else
      container_white_background_div.Attributes.Add("class", "sixteen columns white_background_color content_border")
      pageSizing.Attributes.Add("class", "container")
    End If
  End Sub
  Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Not (isUserLogedIn Or isHomebase Or isforgotToken) Then
      Response.Redirect("Default.aspx", True)
    Else
      If clsGeneral.clsGeneral.CheckForBotActivity(aclsData_Temp, Page.IsPostBack) Then
        'Configure_Background()
      Else 'Check for Bot Activity returned false, ship them to user verification

        Response.Redirect("UserVerification.aspx")
      End If
    End If

  End Sub

  Public Shared Function ValidateAPI(ByVal apiToken As String) As Boolean
    Dim responseAPI As Boolean = False
    Dim responseData As New DataTable
    Dim UserName As String = ""
    Dim Password As String = ""
    If Not IsNothing(apiToken) Then
      If Not String.IsNullOrEmpty(apiToken) Then
        apiToken = clsGeneral.clsGeneral.DecodeBase64(Trim(apiToken))

        responseData = CheckAPIToken(apiToken)
        If Not IsNothing(responseData) Then
          If responseData.Rows.Count = 0 Then
            HttpContext.Current.Session.Contents.Clear()
            HttpContext.Current.Session.Abandon()
            HttpContext.Current.Session.Item("Listing") = ""
            HttpContext.Current.Session.Item("Subnode") = ""
            HttpContext.Current.Session.Item("ID") = ""
            HttpContext.Current.Response.Redirect("/default.aspx?api_error=true")
          ElseIf responseData.Rows.Count > 0 Then
            If Not IsDBNull(responseData.Rows(0).Item("apiact_email_address")) Then
              If Not String.IsNullOrEmpty(responseData.Rows(0).Item("apiact_email_address")) Then
                UserName = responseData.Rows(0).Item("apiact_email_address")
                UserName = clsGeneral.clsGeneral.EncodeBase64(UserName)
              End If
            End If
            If Not IsDBNull(responseData.Rows(0).Item("apiact_password")) Then
              If Not String.IsNullOrEmpty(responseData.Rows(0).Item("apiact_password")) Then
                Password = responseData.Rows(0).Item("apiact_password")
                Password = clsGeneral.clsGeneral.EncodeBase64(Password)
              End If
            End If

            If Not String.IsNullOrEmpty(UserName) And Not String.IsNullOrEmpty(Password) Then
              Dim url_string As String = "logout.aspx?apiLog=true&2=" & UserName & "&1=" & Password

              Dim pageName As String = LCase(HttpContext.Current.Request.Url.ToString)

              If pageName.Contains("displaycompanydetail.aspx") Then

                url_string += "&type=company&id=" & Trim(HttpContext.Current.Request("compid")) & "&jid=" & Trim(HttpContext.Current.Request("jid")) & "&source=" & Trim(HttpContext.Current.Request("source"))
              ElseIf pageName.Contains("displayaircraftdetail.aspx") Then
                url_string += "&type=ac&id=" & Trim(HttpContext.Current.Request("acid")) & "&jid=" & Trim(HttpContext.Current.Request("jid")) & "&source=" & Trim(HttpContext.Current.Request("source"))
              ElseIf pageName.Contains("displaycontactdetail.aspx") Then
                url_string += "&type=contact&id=" & Trim(HttpContext.Current.Request("conid")) & "&compid=" & Trim(HttpContext.Current.Request("compid")) & "&jid=" & Trim(HttpContext.Current.Request("jid")) & "&source=" & Trim(HttpContext.Current.Request("source"))
              End If

              HttpContext.Current.Response.Redirect(url_string)
            End If
          End If
        End If

      End If
    End If

  End Function

  Public Shared Function CheckAPIToken(ByVal APIToken As String)
    Dim sql As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable
    Dim PermissionsClause As String = ""

    Try
      If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("jetnetAdminDatabase")) Then

        'Changes from Rick on 5/14/19

        '1.	Modify the < 1 to be < 8
        '2.	Remove the entire not exists criteria.
        'So now it will basically be just checking to see if this token was good in the last 8 hours. If so it should work. 


        sql = "select top 1 * "
        sql += " from API_Activity_Log with (NOLOCK)"
        sql += " where DateDiff(Hour, apiact_request_date, GETDATE()) < 8"
        sql += " and apiact_call_token = @apiToken "
        ' sql += " and not exists ("
        'sql += " select distinct b.apiact_id from API_Activity_Log b with (NOLOCK) "
        'sql += " where b.apiact_request_date > API_Activity_Log.apiact_request_date And b.apiact_id > API_Activity_Log.apiact_id "
        'sql += " and b.apiact_contact_id = API_Activity_Log.apiact_contact_id and b.apiact_call_token <> API_Activity_Log.apiact_call_token)"

        sql += " order by apiact_id desc "


        SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase")
        SqlConn.Open()

        clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "EmptyEvoTheme.Master.vb", sql.ToString)

        Dim SqlCommand As New SqlClient.SqlCommand(sql, SqlConn)

        SqlCommand.Parameters.AddWithValue("@apiToken", APIToken)

        SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

        Try
          atemptable.Load(SqlReader)
        Catch constrExc As System.Data.ConstraintException
          Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        End Try

        SqlCommand.Dispose()
        SqlCommand = Nothing
      End If

      Return atemptable
    Catch ex As Exception
      CheckAPIToken = Nothing
      'Me.class_error = "Error in Get_Company_Relationships(ByVal comp_id As Integer): SQL VERSION " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing


    End Try

  End Function

End Class