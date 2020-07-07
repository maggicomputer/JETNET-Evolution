' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/home.aspx.vb $
'$$Author: Mike $
'$$Date: 6/14/20 1:48p $
'$$Modtime: 6/14/20 1:42p $
'$$Revision: 18 $
'$$Workfile: home.aspx.vb $
'
' ********************************************************************************

Imports System.IO

Partial Public Class home
  Inherits System.Web.UI.Page

  Dim aTempTable, aTempTable2 As New DataTable 'Data Tables used
  Dim error_string As String = ""
  Dim masterPage As New Object
  Dim COOKIECSSCLASS As String = ""
  Dim attributeAutoCompleteString As String = ""
  Dim number_of_months_divide As Integer = 6
  Dim localDataLayer As New viewsDataLayer
  Dim comp_functions As New CompanyFunctions

  Dim AttributeDynamicJquery As StringBuilder = New StringBuilder()
  Protected PostBackStr As String = ""

  Private Sub home_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
    Dim LargetabIndexChangedScript As StringBuilder = New StringBuilder()
    Dim SmalltabIndexChangedScript As StringBuilder = New StringBuilder()
    Dim MobileTabScript As StringBuilder = New StringBuilder()



    If Not IsNothing(HttpContext.Current.Session.Item("localPreferences").DefaultAnalysisMonths) Then
      If Trim(HttpContext.Current.Session.Item("localPreferences").DefaultAnalysisMonths) <> "" Then
        If IsNumeric(HttpContext.Current.Session.Item("localPreferences").DefaultAnalysisMonths) = True Then
          If HttpContext.Current.Session.Item("localPreferences").DefaultAnalysisMonths > 0 Then
            number_of_months_divide = HttpContext.Current.Session.Item("localPreferences").DefaultAnalysisMonths
          End If
        End If
      End If
    End If



    PostBackStr = Page.ClientScript.GetPostBackEventReference(Me, "")

    Dim EventToggleOnTimeChange As StringBuilder = New StringBuilder()
    If Session.Item("localUser").crmEvo <> True Then 'If a CRM user
      evo_scripts.Visible = False

      If Not Page.ClientScript.IsClientScriptBlockRegistered("LargeTab") Then
        LargetabIndexChangedScript.Append("<script type=""text/javascript"">")
        LargetabIndexChangedScript.Append(vbCrLf & "  function LargeTabActiveTabChanged(sender, e)")
        LargetabIndexChangedScript.Append(vbCrLf & "{")
        LargetabIndexChangedScript.Append(vbCrLf & "  createCookie('LargeHomeActiveTab', sender.get_activeTabIndex(), 365);")

        LargetabIndexChangedScript.Append(vbCrLf & " }")
        LargetabIndexChangedScript.Append(vbCrLf & "</script>")
        Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "LargeTab", LargetabIndexChangedScript.ToString, False)
      End If

    Else
      If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Or HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
        'This turns on the index tab, as well as creates checkboxes for all the attributes.
        DisplayIndexTabFromCache()
        index_tab.Visible = True
      End If

      'Adding some scripts in for the mobile version:

      'On the mobile site of the home page only (at least for testing purposes).
      If Session.Item("isMobile") Then
        main_home_tab_container.Visible = False
        small_home_container_tab.Visible = False
        'market_summary_tab.HeaderText = "Market" '"<i class=""fa fa-bar-chart"" title=""Market Overview""></i>"
        'quick_search_tab.Visible = False '.HeaderText = "<i class=""fa fa-search"" title=""Quick Search""></i>"
        'market_activity_tab.HeaderText = "Events" '"<i class=""fa fa-calendar"" title=""Recent Events""></i>"
        'reports_tab.Visible = False '.HeaderText = "<i class=""fa fa-file-text"" title=""Reports""></i>"
        'wanted_tab.HeaderText = "Wanteds" ' "<i class=""fa fa-plane"" title=""Recent Wanteds""></i>"
        'MyAnalytics.HeaderText = "Analytics" '"<i class=""fa fa-area-chart"" title=""My Analytics""></i>"
        'index_tab.Visible = False '.HeaderText = "<i class=""fa fa-info"" title=""Index""></i>"
        'action_item_tab.HeaderText = "Actions" '"<i class=""fa fa-check-square""  title=""Action Items""></i>"

        'If Not IsNothing(Trim(Request("search"))) Then
        '  If Not Page.ClientScript.IsClientScriptBlockRegistered("MobileTabScript") Then

        '    MobileTabScript.Append(vbCrLf & "function FigureMobileDisplayOut() {")


        '    If Trim(Request("search")) = "company" Then
        '      'searchControls.DefaultButton = searchCompany.UniqueID.ToString
        '      MobileTabScript.Append(vbCrLf & "document.getElementById(""AircraftQuickSearchHeader"").className = 'display_none';")
        '      MobileTabScript.Append(vbCrLf & "document.getElementById(""AircraftQuickSearchCell"").className = 'display_none';")
        '      MobileTabScript.Append(vbCrLf & "document.getElementById(""AircraftQuickSearchButton"").className = 'display_none';")
        '      MobileTabScript.Append(vbCrLf & "$('html, body').animate({")
        '      MobileTabScript.Append(vbCrLf & "scrollTop: $(""#CompanyQuickSearchCell"").offset().top")
        '      MobileTabScript.Append(vbCrLf & "}, 2000);")
        '    Else
        '      'searchControls.DefaultButton = searchAircraft.UniqueID.ToString
        '      MobileTabScript.Append(vbCrLf & "document.getElementById(""CompanyQuickSearchHeader"").className = 'display_none';")
        '      MobileTabScript.Append(vbCrLf & "document.getElementById(""CompanyQuickSearchCell"").className = 'display_none';")
        '      MobileTabScript.Append(vbCrLf & "document.getElementById(""CompanyQuickSearchButton"").className = 'display_none';")
        '      MobileTabScript.Append(vbCrLf & "$('html, body').animate({")
        '      MobileTabScript.Append(vbCrLf & "scrollTop: $(""#AircraftQuickSearchCell"").offset().top")
        '      MobileTabScript.Append(vbCrLf & "}, 2000);")
        '    End If

        '    MobileTabScript.Append(vbCrLf & "}")
        '    Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "MobileTabScript", MobileTabScript.ToString, True)

        '  End If
        'End If

      End If

      If Not Page.ClientScript.IsClientScriptBlockRegistered("LargeTab") Then
        LargetabIndexChangedScript.Append("<script type=""text/javascript"">")
        'Turn this off for those in IE below 8
        LargetabIndexChangedScript.Append(vbCrLf & "var IE7ORBELOW = false;")
        If Request.Browser.Browser.ToString = "IE" And Request.Browser.MajorVersion < 8 Then
          LargetabIndexChangedScript.Append(vbCrLf & "IE7ORBELOW = true;")
        End If

        If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then

          'This needs to be broken apart.
          'IE 8 and below doesn't use addEventListener, it uses attachEvent.
          'So we need to check for existence before running and then basically doing the same thing with slightly altered syntax based on whether addEventListener is actually available
          'or not.
          Dim JavascriptOnLoad As String = ""
          Dim TemporaryHolding As New StringBuilder

          TemporaryHolding.Append(vbCrLf & " SetUpVariableAutoComplete('#" & ModelDynamic.ClientID & "', '#" & searchAircraft.ClientID & "', '#" & searchCompany.ClientID & "','" & ___amod_id.ClientID & "'); ")

          'If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Or HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Then
          If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
            AttributeDynamicJquery.Append(vbCrLf & "$(""#" & ___generic_data_description.ClientID & """).autocomplete({")
            AttributeDynamicJquery.Append(vbCrLf & "  source: [ " & attributeAutoCompleteString & " ],")
            AttributeDynamicJquery.Append(vbCrLf & " select: function (a, b) {")
            AttributeDynamicJquery.Append(vbCrLf & " $(""#" & ___generic_data_description.ClientID & """).val(b.item.label);")
            AttributeDynamicJquery.Append(vbCrLf & " $(""#" & ___attributeID.ClientID & """).val(b.item.value);")
            AttributeDynamicJquery.Append(vbCrLf & " return false;")
            AttributeDynamicJquery.Append(vbCrLf & " },")
            AttributeDynamicJquery.Append(vbCrLf & "change: function(event, ui) {")

            AttributeDynamicJquery.Append(vbCrLf & "if (ui.item) {")
            AttributeDynamicJquery.Append(vbCrLf & "$(""#" & ___attributeID.ClientID & """).val(ui.item.value);")
            AttributeDynamicJquery.Append(vbCrLf & "} else {")
            AttributeDynamicJquery.Append(vbCrLf & "$(""#" & ___attributeID.ClientID & """).val('');")
            AttributeDynamicJquery.Append(vbCrLf & "}")
            AttributeDynamicJquery.Append(vbCrLf & "}")
            AttributeDynamicJquery.Append(vbCrLf & "});")

            'appending the jquery to run on window load to initialize the attribute autocomplete on the quick search
            TemporaryHolding.Append(AttributeDynamicJquery)
          End If
          'End If

          'Only run this script for the mobile version of the site.
          'If Session.Item("isMobile") Then
          '  If Not IsNothing(Trim(Request("search"))) Then
          '    TemporaryHolding.Append(vbCrLf & "FigureMobileDisplayOut();")
          '  End If
          'End If


          JavascriptOnLoad = vbCrLf & "if (window.addEventListener) {"
          JavascriptOnLoad += vbCrLf & " window.addEventListener(""load"", "

          JavascriptOnLoad += vbCrLf & "function () {"
          'function goes here.
          JavascriptOnLoad += TemporaryHolding.ToString
          JavascriptOnLoad += vbCrLf & "}, false); "

          JavascriptOnLoad += vbCrLf & "}" 'Else 
          JavascriptOnLoad += vbCrLf & "else {"

          JavascriptOnLoad += vbCrLf & " window.attachEvent(""load"","
          JavascriptOnLoad += vbCrLf & "function () {"
          'function goes here.
          JavascriptOnLoad += TemporaryHolding.ToString
          JavascriptOnLoad += vbCrLf & "});"

          JavascriptOnLoad += vbCrLf & "}" 'End if
          LargetabIndexChangedScript.Append(vbCrLf & JavascriptOnLoad)
        End If

        LargetabIndexChangedScript.Append(vbCrLf & "  function LargeTabActiveTabChanged(sender, e)")
        LargetabIndexChangedScript.Append(vbCrLf & "{")



        LargetabIndexChangedScript.Append(vbCrLf & "  createCookie('LargeHomeActiveTab', sender.get_activeTabIndex(), 1);")

        LargetabIndexChangedScript.Append(vbCrLf & " if (sender.get_activeTabIndex() == " & IIf(Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT, "2", "4") & ") {")

        LargetabIndexChangedScript.Append(vbCrLf & " $(function(){")
        If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
          '  LargetabIndexChangedScript.Append(vbCrLf & "Sys.Application.add_load(function() {SetUpVariableAutoComplete('#" & YachtModelDynamic.ClientID & "', '#" & searchYacht.ClientID & "', '#" & searchYachtCompany.ClientID & "','" & ___yt_model_id.ClientID & "'); }); ")
        Else

          LargetabIndexChangedScript.Append(vbCrLf & "Sys.Application.add_load(function() {SetUpVariableAutoComplete('#" & ModelDynamic.ClientID & "', '#" & searchAircraft.ClientID & "', '#" & searchCompany.ClientID & "','" & ___amod_id.ClientID & "');  ")

          ' If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Or HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Then
          If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
            'appending the jquery script to initialize the attribute dropdown whenever the tab swaps and causes a partial postback.
            LargetabIndexChangedScript.Append(AttributeDynamicJquery)
          End If
          'End If


          'Only run this script for the mobile version of the site.
          'If Session.Item("isMobile") Then
          '  If Not IsNothing(Trim(Request("search"))) Then
          '    LargetabIndexChangedScript.Append(vbCrLf & "FigureMobileDisplayOut();") 8
          '  End If
          'End If

          LargetabIndexChangedScript.Append(vbCrLf & "});")

        End If

        LargetabIndexChangedScript.Append(vbCrLf & " });")
        LargetabIndexChangedScript.Append(vbCrLf & " }")
        LargetabIndexChangedScript.Append(vbCrLf & "}")
        LargetabIndexChangedScript.Append(vbCrLf & "</script>")
        Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "LargeTab", LargetabIndexChangedScript.ToString, False)
      End If


      If Not Page.ClientScript.IsClientScriptBlockRegistered("SmallTab") Then
        SmalltabIndexChangedScript.Append("<script type=""text/javascript"">")
        SmalltabIndexChangedScript.Append(vbCrLf & "  function SmallTabActiveTabChanged(sender, e)")
        SmalltabIndexChangedScript.Append(vbCrLf & "{")
        SmalltabIndexChangedScript.Append(vbCrLf & "  createCookie('SmallHomeActiveTab', sender.get_activeTabIndex(), 1);")
        SmalltabIndexChangedScript.Append(vbCrLf & "}")
        SmalltabIndexChangedScript.Append(vbCrLf & "</script>")
        Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "SmallTab", SmalltabIndexChangedScript.ToString, False)
      End If

      If Not Page.ClientScript.IsClientScriptBlockRegistered("ToggleEventLoadOnTimeChange") Then
        EventToggleOnTimeChange.Append("<script type=""text/javascript"">")
        EventToggleOnTimeChange.Append(vbCrLf & "function ToggleEventLoadOnTimeChange() {")
        EventToggleOnTimeChange.Append(vbCrLf & "var timePanel = document.getElementById(""" & event_time_panel.ClientID & """);")
        EventToggleOnTimeChange.Append(vbCrLf & "var loadScreen = document.getElementById(""" & events_load.ClientID & """);")
        EventToggleOnTimeChange.Append(vbCrLf & "var loadLabel = document.getElementById(""" & event_listing_label.ClientID & """);")
        EventToggleOnTimeChange.Append(vbCrLf & "if (loadScreen != null){ loadScreen.className = 'display_block'};")
        EventToggleOnTimeChange.Append(vbCrLf & "if (loadLabel != null){ loadLabel.className = 'display_none' };")
        EventToggleOnTimeChange.Append(vbCrLf & "if (timePanel != null){ timePanel.className = 'display_none light_seafoam_green_header_color' };")

        EventToggleOnTimeChange.Append(vbCrLf & "}")
        EventToggleOnTimeChange.Append(vbCrLf & "</script>")
        Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ToggleEvent", EventToggleOnTimeChange.ToString, False)
      End If
    End If

    If Not Page.IsPostBack Then

      'Check first to see if the quick tab is being set.
      If Trim(Request("tab")) <> Nothing Then
        If Not String.IsNullOrEmpty(Trim(Request("tab"))) Then
          If Trim(Request("tab")) = "search" Then
            main_home_tab_container.ActiveTab = quick_search_tab
            Response.Cookies.Item("LargeHomeActiveTab").Value = main_home_tab_container.ActiveTabIndex
            Response.Redirect("home.aspx")
          End If
        End If
      End If
    End If


  End Sub

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try
      If Session.Item("crmUserLogon") <> True Then
        Response.Redirect("Default.aspx", False)
      Else


        If Trim(Request("events")) = "Y" Then
          folder_events_tab.Visible = True
        End If

        If Not IsPostBack Then
          ___operator_length.Items.Clear()
          DisplayFunctions.Fill_Dropdown("Numeric", ___operator_length, "")
        End If

        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br />start load home page data : " + Now.ToString + "<br />"
        Trace.Write("Start PageLoad Home.aspx" + Now.ToString)

        Dim today As Date = FormatDateTime(Now(), 2)
        Dim week As Integer = Weekday(today)
        Dim monthint As Integer = Month(today)
        Dim monthdis As String = MonthName(monthint)
        Dim weekdis As String = WeekdayName(week)
        Dim yeardis As Integer = Year(today)
        Dim daydis As Integer = Day(today)
        today_date.Text = weekdis & ", " & monthdis & " " & daydis & ", " & yeardis

        'This makes sure that the notes aren't cached.
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Cache.SetNoStore()
        Response.Expires = -1


        'Warning shown if you're a guest user. 
        If Session.Item("localUser").crmUserType = eUserTypes.GUEST Then
          demo_attention_label.Visible = True
        Else
          demo_attention_label.Visible = False
        End If

        Dim UserID As String = HttpContext.Current.Session.Item("localUser").crmLocalUserID.ToString

        If Session.Item("localUser").crmEvo = True Then 'If an EVO user

          Trace.Write("Start Load Preferences Home.aspx" + Now.ToString)

          Dim sErrorString As String = ""
          If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(Session.Item("localUser").crmSubSubID.ToString), Session.Item("localUser").crmUserLogin.ToString, CLng(Session.Item("localUser").crmSubSeqNo.ToString), CLng(Session.Item("localUser").crmUserContactID.ToString)) Then
            Response.Write("error in load preferences : " + sErrorString)
          End If

          Trace.Write("End Load Preferences Home.aspx" + Now.ToString)

          crmPanelVisibility.Visible = False ' toggle the visibility of this off
          crm_tab.Visible = False
          home_right_visible.Visible = False
          yacht_display_table.Visible = False
          UserID = Session.Item("localUser").crmUserCompanyID.ToString & Session.Item("localUser").crmUserContactID.ToString & Session.Item("localUser").crmSubSubID.ToString

          'this only shows up for the evo application.
          If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then

            If Session.Item("localUser").crmUser_Evo_MPM_Flag = True Then
              Me.my_mpm_tabpanel.Visible = True
            End If

            If (CBool(My.Settings.enableChat)) Then

              If HttpContext.Current.Session.Item("localPreferences").ChatEnabled Then
                labelListOfUsers.Text = ChatManager.DisplayUsersForChatFilter()
                labelListOfUsers.Visible = True
              End If

              chat_panel.Visible = True

            Else
              chat_panel.Visible = False
            End If

            Trace.Write("Begin Evo section Home.aspx" + Now.ToString)

            'Changed on 7/10/2015. This is going to check for the isMobile session variable
            'If you're in the evolution app and swap your masterpage accordingly.
            If Session.Item("isMobile") Then
              masterPage = DirectCast(Page.Master, MobileTheme)
            Else
              masterPage = DirectCast(Page.Master, EvoTheme)
            End If



            evo_display_table.Visible = True




            If Not Page.IsPostBack Then 'If the page isn't a postback
              'This basically loads the array into session.
              'You're going to need this if you're going into the view for the first time. Otherwise the make/models won't be loaded
              'correctly.
              commonEvo.fillAirframeArray("")
              commonEvo.fillAircraftTypeLableArray("")
              commonEvo.fillDefaultAirframeArray("")

              Dim HideSharedIndex As HttpCookie = Request.Cookies("hideShared")
              Dim HideHiddenIndex As HttpCookie = Request.Cookies("hideHidden")

              If Not IsNothing(HideSharedIndex) Then
                If HideSharedIndex.Value = "true" Then
                  hide_shared.Checked = True
                End If
              End If

              If Not IsNothing(HideHiddenIndex) Then
                If HideHiddenIndex.Value = "true" Then
                  show_hidden_folders.Checked = True
                End If
              End If

              'Fill/Check reports tab
              If Session.Item("isMobile") = False Then
                Display_Reports(custom_reports_label, False)

                'We need to go ahead and load the category options for the event tab:
                Dim EventCategory As New DataTable
                EventCategory = masterPage.aclsData_Temp.Market_Search_Category()
                If Not IsNothing(EventCategory) Then
                  If Not EventCategory.Rows.Count < 0 Then
                    For Each r As DataRow In EventCategory.Rows
                      If Not IsDBNull(r("apecat_category_group")) Then
                        event_category.Items.Add(New ListItem(r("apecat_category_group"), r("apecat_category_group")))
                      End If
                    Next
                  End If
                  event_category.SelectedValue = ""
                End If

                'Go ahead and fill the relationship dropdown if it's not filled (quick search tab)
                If company_relationship___cref_contact_type.Items.Count = 0 Then
                  Dim TempTable As New DataTable

                  TempTable = masterPage.aclsData_Temp.Get_Client_Aircraft_Contact_Type()

                  company_relationship___cref_contact_type.Items.Insert(0, New ListItem("All", ""))
                  company_relationship___cref_contact_type.Items.Insert(1, New ListItem("All Owners", "'00','97','17','08','16'"))
                  company_relationship___cref_contact_type.Items.Insert(2, New ListItem("All Operating Companies", "'Y'"))
                  company_relationship___cref_contact_type.Items.Insert(3, New ListItem("All Dealers, Brokers, Reps", "'93','98','99'"))

                  For Each r As DataRow In TempTable.Rows
                    If Not IsDBNull(r("cliact_name")) And Not IsDBNull(r("cliact_type")) Then
                      company_relationship___cref_contact_type.Items.Add(New ListItem(r("cliact_name"), "'" & r("cliact_type") & "'"))
                    End If
                  Next

                  TempTable.Dispose()

                  company_relationship___cref_contact_type.SelectedValue = ""
                End If

                'This is filling up the models for the select box on the quick search.
                If ModelDynamic.Items.Count = 0 Then
                  Dim TempTable As New DataTable
                  TempTable = masterPage.aclsData_Temp.GetAircraft_MakeModels("", "", Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmCommercial_Flag, Session.Item("localSubscription").crmJets_Flag, Session.Item("localSubscription").crmExecutive_Flag, Session.Item("localSubscription").crmTurboprops, "")
                  ModelDynamic.Items.Insert(0, New ListItem("", ""))
                  For Each r As DataRow In TempTable.Rows
                    If Not IsDBNull(r("amod_model_name")) And Not IsDBNull(r("amod_make_name")) Then
                      If Not IsDBNull(r("amod_id")) Then
                        ModelDynamic.Items.Add(New ListItem(r("amod_make_name").ToString & " " & r("amod_model_name").ToString, r("amod_make_name").ToString & "|" & r("amod_id")))
                        'Fill up the Models if they have not been filled
                      End If
                    End If
                  Next
                  TempTable.Dispose()
                End If



                'Display Recent Items.
                Create_Recent_Items(UserID, aircraft_recent, False, False, True, False, "Aircraft")
                Create_Recent_Items(UserID, company_recent, True, False, False, False, "Companies")
                Create_Recent_Items(UserID, contact_recent, False, True, False, False, "Contacts")

                'Display Folder Listing to the side
                GetFolderListing(aircraft_projects, company_projects, contact_projects, history_projects, event_projects, wanted_projects, Nothing, performance_specs_projects, operating_costs_projects, marketing_summary_projects, Nothing, Nothing, airport_projects, show_hidden_folders, values_projects, hide_shared)

              End If

            End If

            'No recent AC Activity/Company Activity, display a small note.
            If aircraft_recent.Visible = False And company_recent.Visible = False And contact_recent.Visible = False Then
              recent_aircraft_activity_evo.Text = "<p align=""center""><br />There is no recent activity.</p>"
              recent_aircraft_activity_evo.CssClass = "emphasis_text"
            End If
            'Set the active tab to the first one.
            masterPage.Set_Active_Tab(0)

            'toggle notices
            'b.	Also – you can remove the notices tab from the home page since it will always be in upper right.
            ' notices_tab.Visible = False

            'Defaulting to active tab index. This happens after we toggle visible on/off
            small_home_container_tab.ActiveTabIndex = 0

            'retaining home page information.
            'Let's figure out if a cookie is saved.
            Dim SmallHomePageTabIndex As HttpCookie = Request.Cookies("SmallHomeActiveTab")
            Dim LargeHomePageTabIndex As HttpCookie = Request.Cookies("LargeHomeActiveTab")


            If Not Page.IsPostBack Then


              If Session.Item("localSubscription").crmAerodexFlag Then
                wanted_tab.Visible = False
                MyAnalytics.Visible = False

                If Not IsNothing(LargeHomePageTabIndex) Then
                  If LargeHomePageTabIndex.Value = 3 Then
                    LargeHomePageTabIndex.Value = 0
                  End If
                End If
              End If

              If HttpContext.Current.Session.Item("localPreferences").AerodexStandard = True Then
                airport_tab.Visible = False
              End If


              If Session.Item("localSubscription").crmServerSideNotes_Flag = True Then
                Dim ExistsTable As New DataTable
                ExistsTable = masterPage.aclsData_Temp.Get_Client_User_By_Email_Address(Session.Item("localUser").crmLocalUserEmailAddress)

                If Not IsNothing(ExistsTable) Then
                  If ExistsTable.Rows.Count = 0 Then 'This means that the user needs to be inserted.
                    'Please insert the user here.
                    Session.Item("localUser").crmLocalUserID = masterPage.aclsData_Temp.Insert_Client_User_Return(Session.Item("localUser").crmLocalUserFirstName, Session.Item("localUser").crmLocalUserLastName, Session.Item("localUser").crmLocalUserName, "", "N", Session.Item("localUser").crmLocalUserEmailAddress, Now(), 0, 0, New Nullable(Of System.DateTime))
                  ElseIf ExistsTable.Rows.Count > 0 Then
                    Session.Item("localUser").crmLocalUserID = ExistsTable.Rows(0).Item("cliuser_id")
                  End If
                End If
              End If


              If Session.Item("isMobile") Then
                'This means we don't set the active tab other than being 0.
                'Unless we go ahead and click the button to select the search.
                If Not IsNothing(Trim(Request("search"))) Then
                  main_home_tab_container.ActiveTab = quick_search_tab 'default
                  If Not String.IsNullOrEmpty(Trim(Request("search"))) Then
                    ' main_home_tab_container.ActiveTab = quick_search_tab
                  ElseIf Session.Item("localSubscription").crmServerSideNotes_Flag = True Or Session.Item("localSubscription").crmCloudNotes_Flag = True Then
                    action_item_tab.Visible = True
                    main_home_tab_container.ActiveTab = action_item_tab
                  End If
                End If
              Else

                'If have notes, make the tab visible and set to active tab.
                If Session.Item("localSubscription").crmServerSideNotes_Flag = True Or Session.Item("localSubscription").crmCloudNotes_Flag = True Then
                  action_item_tab.Visible = True
                  main_home_tab_container.ActiveTab = action_item_tab
                Else 'Otherwise toggle off.
                  action_item_tab.Visible = False
                End If
                'If aerodex is true, toggle the summary name.
                If Session.Item("localSubscription").crmAerodexFlag = True Then
                  market_summary_tab.HeaderText = "Fleet Summary"
                  market_overview_tab_label.Text = "Fleet Summary"
                  'if no selected models, default to quick search tab.
                  If Session.Item("localUser").crmSelectedModels = "" Then
                    main_home_tab_container.ActiveTab = quick_search_tab
                  End If
                End If

                Dim TabOffset As Integer = 0
                Select Case Session.Item("BusinessSegment")
                  Case "DB"
                    main_home_tab_container.ActiveTab = market_summary_tab
                    market_summary_tab.Visible = True
                    market_activity_tab.Visible = True
                    wanted_tab.Visible = True
                    If Not IsNothing(LargeHomePageTabIndex) Then
                      If LargeHomePageTabIndex.Value >= 8 Then
                        TabOffset = 1
                      End If
                    End If
                  Case "FB"
                    market_overview_tab_label.Text = "Fleet Summary"
                    market_summary_tab.HeaderText = "Fleet Summary" 'In this case, we need the fleet summary tab to show up, not market summary.
                    airport_tab.Visible = True
                    market_summary_tab.Visible = True
                    market_activity_tab.Visible = True
                    wanted_tab.Visible = True

                    If Session.Item("localSubscription").crmAerodexFlag = False Then
                      main_home_tab_container.ActiveTab = airport_tab
                      'TabOffset = 4 'DB tabs/airports tab
                    End If
                  Case Else
                    main_home_tab_container.ActiveTab = quick_search_tab
                    TabOffset = 3
                    If Not IsNothing(LargeHomePageTabIndex) Then
                      If LargeHomePageTabIndex.Value >= 5 Then
                        TabOffset = 4
                      End If
                    End If
                End Select


                'If there is a cookie for the right hand tab container, set it as active.
                Try
                  If Not IsNothing(SmallHomePageTabIndex) Then
                    small_home_container_tab.ActiveTabIndex = SmallHomePageTabIndex.Value
                  End If

                Catch
                  'Default to originally set active tab if cookie setting is unavailable
                End Try
                Try
                  'If there is a cookie for the main tab container, set it as active.
                  If Not IsNothing(LargeHomePageTabIndex) Then
                    main_home_tab_container.ActiveTabIndex = LargeHomePageTabIndex.Value + TabOffset
                  End If
                Catch
                  'Default to originally set active tab if cookie setting is unavailable
                End Try
              End If
              'Run through the tab container changed event

              If Session.Item("isMobile") = False Then
                If Not Page.IsPostBack Then
                  main_home_tab_container_ActiveTabChanged(main_home_tab_container, EventArgs.Empty)
                End If
              End If

            End If

            Trace.Write("End Evo section Home.aspx" + Now.ToString)

          ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
            masterPage = DirectCast(Page.Master, CustomerAdminTheme)
            evo_display_table.Visible = False
            yacht_display_table.Visible = False

          ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
            Dim TempTable As New DataTable
            'Casting the master theme to correct type.
            masterPage = DirectCast(Page.Master, YachtTheme)

            '''''''''''''''''''''''''''''''''''''''''''''''
            ''''''''''''''''''''''''''''''''''''''''''''''''
            '''''''''Working on how to reuse this section in both yacht and ac side
            '''''''''''''''''''''''''''''''''''''''''''''''
            ''''''''''''''''''''''''''''''''''''''''''''''''
            'Setting tabs active
            Dim SmallHomePageTabIndex As HttpCookie = Request.Cookies("SmallHomeActiveTab")
            Dim LargeHomePageTabIndex As HttpCookie = Request.Cookies("LargeHomeActiveTab")
            'If there is a cookie for the right hand tab container, set it as active.
            If Not IsNothing(SmallHomePageTabIndex) Then
              yacht_small_tab.ActiveTabIndex = SmallHomePageTabIndex.Value
            End If
            'If there is a cookie for the main tab container, set it as active.
            If Not IsNothing(LargeHomePageTabIndex) Then
              yacht_summary_tab.ActiveTabIndex = LargeHomePageTabIndex.Value
            End If

            'Toggling display tables.
            evo_display_table.Visible = False
            yacht_display_table.Visible = True

            If Not Page.IsPostBack Then
              'Display Folder Listing to the side
              GetFolderListing(Nothing, yacht_company_projects, yacht_contact_projects, Nothing, Nothing, Nothing, yacht_projects, Nothing, Nothing, Nothing, yacht_history_projects, yacht_event_projects, Nothing, yacht_hidden_folders, Nothing, Nothing)

              'Displaying the Recent Items List.
              Create_Recent_Items(UserID, yacht_company_recent, True, False, False, False, "Companies")
              Create_Recent_Items(UserID, yacht_contact_recent, True, False, False, False, "Contacts")
              Create_Recent_Items(UserID, yacht_recent, False, False, False, True, "Yachts")

              'No recent AC Activity/Company Activity, display a small note.
              If yacht_company_recent.Visible = False And yacht_contact_recent.Visible = False And yacht_recent.Visible = False Then
                recent_aircraft_activity_yacht.Text = "<p align=""center""><br />There is no recent activity.</p>"
                recent_aircraft_activity_yacht.CssClass = "emphasis_text"
              End If

              'Fill out relationship

              If company_relationship___.Items.Count = 0 Then

                TempTable = masterPage.aclsData_Temp.Get_Yacht_Contact_Type(False)
                For Each r As DataRow In TempTable.Rows
                  If Not IsDBNull(r("yct_code")) And Not IsDBNull(r("yct_name")) Then
                    company_relationship___.Items.Add(New ListItem(r("yct_name"), "'" & r("yct_code") & "'"))
                  End If
                Next


                company_relationship___.Items.Insert(0, New ListItem("All", ""))
                company_relationship___.Items.Insert(1, New ListItem("All Central Agents", "'99','C1','C2','C3','C4','C5','C6'"))
                company_relationship___.Items.Insert(2, New ListItem("All Designers", "'Y1','Y2','Y3','Y0','Y9'"))
                company_relationship___.Items.Insert(3, New ListItem("All Owners", "'00','08'"))
                company_relationship___.SelectedValue = ""
              End If


              'Displaying Summary
              CreateYachtSummary()

              'Displaying News Listing 
              GetYachtNewsListing()

              If Not Page.IsPostBack Then
                'Added to rerun attributes only if it defaults to it on page load.
                yacht_summary_tab_ActiveTabChanged(yacht_summary_tab, EventArgs.Empty)
              End If

              'Fill Yacht Model Dropdown:
              'Fill up the Models if they have not been filled
              'If ModelDynamic.Items.Count = 0 Then
              '  TempTable = New DataTable
              '  TempTable = YachtFunctions.GetYachtBrandQuickSearch()
              '  YachtModelDynamic.Items.Insert(0, New ListItem("", ""))
              '  For Each r As DataRow In TempTable.Rows
              '    Dim YachtPrefix As String = ""

              '    If Not IsDBNull(r("ym_motor_type")) Then
              '      YachtPrefix = "[" & r("ym_motor_type") & "]"
              '    End If
              '    If Not IsDBNull(r("ycs_description")) Then
              '      YachtPrefix += "[" & r("ycs_description") & "]"
              '    End If

              '    If Not IsDBNull(r("ym_model_name")) And Not IsDBNull(r("ym_brand_name")) Then
              '      If Not IsDBNull(r("ym_model_id")) Then
              '        YachtModelDynamic.Items.Add(New ListItem(YachtPrefix & " - " & r("ym_brand_name").ToString & " " & r("ym_model_name").ToString, r("ym_brand_name").ToString & "|" & r("ym_model_id")))
              '      End If
              '    End If
              '  Next
              '  TempTable.Dispose()
              'End If

              If Session.Item("localSubscription").crmCloudNotes_Flag = True Then
                yacht_action_items.Visible = True
                'Display Action Items:
                Create_Evo_Action_Items(yacht_action_items_label)
              End If

            End If
          End If
        Else 'If a CRM user, show action items
          masterPage = DirectCast(Page.Master, main_site)

          masterPage.ListingID = 0
          masterPage.ListingSource = ""
          masterPage.TypeOfListing = 9
          masterPage.Search_display()
          masterPage.fill_bar()

          Session.Item("isMobile") = False
          Session("Results") = Nothing


          'display_table_crm.Visible = True
          crmPanelVisibility.Visible = True ' toggle the visibility of this on
          home_right_visible.Visible = True
          evo_display_table.Visible = False
          ' crm_tab.ActiveTab = action_panel
          crm_action_panel.Visible = True
          crm_time_panel.CssClass = "display_block light_seafoam_green_header_color"



          'Setting up the page for homepage, clearing the sessions
          If Not Page.IsPostBack Then

            Dim LargeHomePageTabIndex As HttpCookie = Request.Cookies("LargeHomeActiveTab")
            'If there is a cookie for the main tab container, set it as active.
            If Not IsNothing(LargeHomePageTabIndex) Then
              crm_tab.ActiveTabIndex = LargeHomePageTabIndex.Value
            End If



            'If we're an administrator, we go ahead and show these tabs
            'as well as run the admin queries.
            If Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR Then
              crm_user_activity_panel.Visible = True
              crm_client_db_panel.Visible = True
            Else 'otherwise we make sure they're off.
              crm_user_activity_panel.Visible = False
              crm_client_db_panel.Visible = False
            End If


            'We need to go ahead and load the category options for the event tab:
            Dim EventCategory As New DataTable
            EventCategory = masterPage.aclsData_Temp.Market_Search_Category()
            If Not IsNothing(EventCategory) Then
              If Not EventCategory.Rows.Count < 0 Then
                For Each r As DataRow In EventCategory.Rows
                  If Not IsDBNull(r("apecat_category_group")) Then
                    crm_event_category.Items.Add(New ListItem(r("apecat_category_group"), r("apecat_category_group")))
                  End If
                Next
              End If
              event_category.SelectedValue = ""
            End If

            'Filling up the recent events.
            recently_edited_viewed_companies()
            recently_edited_viewed_contacts()
            recently_edited_viewed_ac()
            recently_added_notes()

            crm_tab_ActiveTabChanged(crm_tab, System.EventArgs.Empty)
          End If

        End If
      End If

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "home.aspx.vb - Page Load() - " + ex.Message
    End Try

    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br />end load home page data : " + Now.ToString + "<br />"
    Trace.Write("End PageLoad Home.aspx" + Now.ToString)

  End Sub


  Private Sub FillClientDBTotals()

    Trace.Write("Start FillClientDBTotals Home.aspx" + Now.ToString)
    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br />Start FillClientDBTotals Home.aspx : " + Now.ToString + "<br />"

    Dim DBTotals As New DataTable
    DBTotals = Master.aclsData_Temp.GetMasterClientDBTotals()
    If Not IsNothing(DBTotals) Then
      client_database_label.Text = "<table width='100%' cellpadding='6' cellspacing='0' class='data_aircraft_grid'>"
      client_database_label.Text += "<tr class='header_row'><td align='center' valign='top' colspan='2'><b>CLIENT DATABASE RECORD SUMMARY</b></td></tr>"

      'Header Row
      client_database_label.Text += "<tr class='header_row'><td align='left' valign='top'><b>Data Table</b></td><td align='left' valign='top'><b># Records</b></td></tr>"
      If DBTotals.Rows.Count > 0 Then
        'Companies Row
        client_database_label.Text += "<tr><td align='left' valign='top'>Client Companies</td><td align='left' valign='top'>" & DBTotals.Rows(0).Item("company_count").ToString & "</td></tr>"
        'Contacts Row:
        client_database_label.Text += "<tr class='alt_row'><td align='left' valign='top'>Client Contacts</td><td align='left' valign='top'>" & DBTotals.Rows(0).Item("contact_count").ToString & "</td></tr>"
        'Client Aircraft:
        client_database_label.Text += "<tr><td align='left' valign='top'>Client Aircraft</td><td align='left' valign='top'>" & DBTotals.Rows(0).Item("ac_count").ToString & "</td></tr>"
        'Client Wanteds:
        client_database_label.Text += "<tr class='alt_row'><td align='left' valign='top'>Client Wanted</td><td align='left' valign='top'>" & DBTotals.Rows(0).Item("wanted_count").ToString & "</td></tr>"
        'Client Transactions:
        client_database_label.Text += "<tr><td align='left' valign='top'>Client Transactions</td><td align='left' valign='top'>" & DBTotals.Rows(0).Item("trans_count").ToString & "</td></tr>"
        'Client Notes:
        client_database_label.Text += "<tr class='alt_row'><td align='left' valign='top'>Client Notes</td><td align='left' valign='top'>" & DBTotals.Rows(0).Item("note_count").ToString & "</td></tr>"
        'Client Action Items:
        client_database_label.Text += "<tr><td align='left' valign='top'>Client Action Items</td><td align='left' valign='top'>" & DBTotals.Rows(0).Item("action_count").ToString & "</td></tr>"
        'Client Opportunities
        client_database_label.Text += "<tr class='alt_row'><td align='left' valign='top'>Client Opportunities</td><td align='left' valign='top'>" & DBTotals.Rows(0).Item("opp_count").ToString & "</td></tr>"

      End If
      client_database_label.Text += "</table>"
    End If

    Trace.Write("End FillClientDBTotals Home.aspx" + Now.ToString)
    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br />End FillClientDBTotals Home.aspx : " + Now.ToString + "<br />"

    Call create_client_aircraft_summary()



  End Sub

  Public Sub create_client_aircraft_summary()

    Dim htmlOut As New StringBuilder
    Dim CLIENT_AC As New DataTable
    Dim toggleRowColor As Boolean

    CLIENT_AC = masterPage.aclsData_Temp.GetMasterClientAircraft()
    If Not IsNothing(CLIENT_AC) Then
      ' mympm_label.Text = "<table width='100%' cellpadding='6' cellspacing='0' class='data_aircraft_grid'>"
      ' mympm_label.Text += "<tr class='header_row'><td align='center' valign='top' colspan='3'><b>CLIENT DATABASE AIRCRAFT RECORDS</b></td></tr>"
      'Header Row
      '  mympm_label.Text += "<tr class='header_row'><td align='left' valign='top'><b></b></td><td align='right' valign='top'><b>For Sale</b></td><td align='right' valign='top'><b>Not For Sale</b></td></tr>"



      'htmlOut.Append(comp_functions.NEW_build_style_page_full_spec(False, False, 998))

      htmlOut.Append("<div class=""valueSpec Simplistic grayTabContainer""><div class=""Box"">")

      htmlOut.Append("<table cellpadding='3' cellspacing=""0"" width=""100%"" class='formatTable blue'><thead>")

      htmlOut.Append("<tr><th valign=""middle"" class=""left"">Model Name</th>")
      htmlOut.Append("<th valign=""middle"" class=""right"">For Sale</th>")
      htmlOut.Append("<th valign=""middle"" class=""right"">Not For Sale</th></tr>")
      htmlOut.Append("</thead><tbody>")


      If CLIENT_AC.Rows.Count > 0 Then
        For Each r As DataRow In CLIENT_AC.Rows

          If Not toggleRowColor Then
            htmlOut.Append("<tr class=""alt_row"">")
            toggleRowColor = True
          Else
            htmlOut.Append("<tr>")
            toggleRowColor = False
          End If


          ' if we are on evo, we have to do something different 
          If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then

            If Not String.IsNullOrEmpty(r.Item("cliamod_make_name").ToString) Then
              htmlOut.Append("<td valign=""middle"" class=""left""><a class=""underline"" title='Client Records for " & r.Item("cliamod_make_name").ToString & " " & r.Item("cliamod_model_name").ToString & "' onclick=""javascript:load('fullTextSearch.aspx?q=" & r.Item("cliamod_make_name").ToString & " " & r.Item("cliamod_model_name").ToString & "&amod_id=" & r.Item("cliamod_jetnet_amod_id").ToString & "&client_only=Y','','scrollbars=yes,menubar=no,height=700,width=1250,resizable=yes,toolbar=no,location=no,status=no');""/>" & r.Item("cliamod_make_name").ToString & " " & r.Item("cliamod_model_name").ToString & "</a></td>")
            Else
              htmlOut.Append("<td>&nbsp;</td>")
            End If

            If Not String.IsNullOrEmpty(r.Item("fs_count").ToString) Then
              htmlOut.Append("<td valign=""middle"" class=""right""><a class=""underline"" title='" & r.Item("cliamod_make_name").ToString & " " & r.Item("cliamod_model_name").ToString & " Client Records For Sale' onclick=""javascript:load('fullTextSearch.aspx?q=" & r.Item("cliamod_make_name").ToString & " " & r.Item("cliamod_model_name").ToString & " for sale&client_only=Y&amod_id=" & r.Item("cliamod_jetnet_amod_id").ToString & "&for_sale=Y','','scrollbars=yes,menubar=no,height=700,width=1250,resizable=yes,toolbar=no,location=no,status=no');""/>" & r.Item("fs_count").ToString & "</a></td>")
            Else
              htmlOut.Append("<td>&nbsp;</td>")
            End If

            If Not String.IsNullOrEmpty(r.Item("nfs_count").ToString) Then
              htmlOut.Append("<td valign=""middle"" class=""right""><a class=""underline"" title='" & r.Item("cliamod_make_name").ToString & " " & r.Item("cliamod_model_name").ToString & " Client Records Not For Sale' onclick=""javascript:load('fullTextSearch.aspx?q=" & r.Item("cliamod_make_name").ToString & " " & r.Item("cliamod_model_name").ToString & " not for sale&client_only=Y&amod_id=" & r.Item("cliamod_jetnet_amod_id").ToString & "&for_sale=N','','scrollbars=yes,menubar=no,height=700,width=1250,resizable=yes,toolbar=no,location=no,status=no');""/>" & r.Item("nfs_count").ToString & "</a></td>")
            Else
              htmlOut.Append("<td>&nbsp;</td>")
            End If



          Else

            If Not String.IsNullOrEmpty(r.Item("cliamod_model_name").ToString) Then
              htmlOut.Append("<td valign=""middle"" class=""left""><a class=""underline"" title='Client Records for " & r.Item("cliamod_make_name").ToString & " " & r.Item("cliamod_model_name").ToString & "' href='/listing_air.aspx?runMarket=true&jetnetModelID=" & r.Item("cliamod_jetnet_amod_id") & "&show_only_client=Y'/>" & r.Item("cliamod_make_name").ToString & " " & r.Item("cliamod_model_name").ToString & "</a></td>")
            Else
              htmlOut.Append("<td>&nbsp;</td>")
            End If

            If Not String.IsNullOrEmpty(r.Item("fs_count").ToString) Then
              htmlOut.Append("<td valign=""middle"" class=""left""><a class=""underline"" title='" & r.Item("cliamod_make_name").ToString & " " & r.Item("cliamod_model_name").ToString & " Client Records For Sale' href='/listing_air.aspx?runMarket=true&jetnetModelID=" & r.Item("cliamod_jetnet_amod_id") & "&forSale=true&show_only_client=Y'/>" & r.Item("fs_count").ToString & "</a></td>")
            Else
              htmlOut.Append("<td>&nbsp;</td>")
            End If

            If Not String.IsNullOrEmpty(r.Item("comp_name").ToString) Then
              htmlOut.Append("<td valign=""middle"" class=""left""><a class=""underline"" title='" & r.Item("cliamod_make_name").ToString & " " & r.Item("cliamod_model_name").ToString & " Client Records Not For Sale' href='/listing_air.aspx?runMarket=true&jetnetModelID=" & r.Item("cliamod_jetnet_amod_id") & "&forSale=false&show_only_client=Y'>" & r.Item("nfs_count").ToString & "</a></td>")
            Else
              htmlOut.Append("<td>&nbsp;</td>")
            End If
          End If



          htmlOut.Append("</tr>")

          '' if we are on evo, we have to do something different 
          'If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
          '  mympm_label.Text += "<tr><td align='left' valign='top'><a class=""underline"" title='Client Records for " & r.Item("cliamod_make_name").ToString & " " & r.Item("cliamod_model_name").ToString & "' onclick=""javascript:load('fullTextSearch.aspx?q=" & r.Item("cliamod_make_name").ToString & " " & r.Item("cliamod_model_name").ToString & "&amod_id=" & r.Item("cliamod_jetnet_amod_id").ToString & "&client_only=Y','','scrollbars=yes,menubar=no,height=700,width=1250,resizable=yes,toolbar=no,location=no,status=no');""/>" & r.Item("cliamod_make_name").ToString & " " & r.Item("cliamod_model_name").ToString & "</a></td>"
          '  mympm_label.Text += "<td align='right' valign='top'><a class=""underline"" title='" & r.Item("cliamod_make_name").ToString & " " & r.Item("cliamod_model_name").ToString & " Client Records For Sale' onclick=""javascript:load('fullTextSearch.aspx?q=" & r.Item("cliamod_make_name").ToString & " " & r.Item("cliamod_model_name").ToString & " for sale&client_only=Y&amod_id=" & r.Item("cliamod_jetnet_amod_id").ToString & "&for_sale=Y','','scrollbars=yes,menubar=no,height=700,width=1250,resizable=yes,toolbar=no,location=no,status=no');""/>" & r.Item("fs_count").ToString & "</a></td>"
          '  mympm_label.Text += "<td align='right' valign='top'><a class=""underline"" title='" & r.Item("cliamod_make_name").ToString & " " & r.Item("cliamod_model_name").ToString & " Client Records Not For Sale' onclick=""javascript:load('fullTextSearch.aspx?q=" & r.Item("cliamod_make_name").ToString & " " & r.Item("cliamod_model_name").ToString & " not for sale&client_only=Y&amod_id=" & r.Item("cliamod_jetnet_amod_id").ToString & "&for_sale=N','','scrollbars=yes,menubar=no,height=700,width=1250,resizable=yes,toolbar=no,location=no,status=no');""/>" & r.Item("nfs_count").ToString & "</a></td>"
          '  mympm_label.Text += "</tr>"
          'Else
          '  mympm_label.Text += "<tr><td align='left' valign='top'><a class=""underline"" title='Client Records for " & r.Item("cliamod_make_name").ToString & " " & r.Item("cliamod_model_name").ToString & "' href='/listing_air.aspx?runMarket=true&jetnetModelID=" & r.Item("cliamod_jetnet_amod_id") & "&show_only_client=Y'/>" & r.Item("cliamod_make_name").ToString & " " & r.Item("cliamod_model_name").ToString & "</a></td>"
          '  mympm_label.Text += "<td align='right' valign='top'><a class=""underline"" title='" & r.Item("cliamod_make_name").ToString & " " & r.Item("cliamod_model_name").ToString & " Client Records For Sale' href='/listing_air.aspx?runMarket=true&jetnetModelID=" & r.Item("cliamod_jetnet_amod_id") & "&forSale=true&show_only_client=Y'/>" & r.Item("fs_count").ToString & "</a></td>"
          '  mympm_label.Text += "<td align='right' valign='top'><a class=""underline"" title='" & r.Item("cliamod_make_name").ToString & " " & r.Item("cliamod_model_name").ToString & " Client Records Not For Sale' href='/listing_air.aspx?runMarket=true&jetnetModelID=" & r.Item("cliamod_jetnet_amod_id") & "&forSale=false&show_only_client=Y'>" & r.Item("nfs_count").ToString & "</a></td>"
          '  mympm_label.Text += "</tr>"
          'End If


        Next
      End If


      htmlOut.Append("</tbody></table>")
      'htmlOut.Append("</td></tr></table>")
      htmlOut.Append("</div></div>")

    End If

    mympm_label.Text = htmlOut.ToString

  End Sub


  ''' <summary>
  ''' Builds a CRM admin tab that displays client user notes/actions/opps totals plus stats on the last six months.
  ''' </summary>
  ''' <remarks></remarks>
  Private Sub FillUserActivityPanel()
    Dim UserActivity As New DataTable
    Dim StartingNotesTable As New DataTable
    Dim Counter As Integer

    Trace.Write("Start FillUserActivityPanel Home.aspx" + Now.ToString)
    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br />Start FillUserActivityPanel Home.aspx : " + Now.ToString + "<br />"

    Dim css As String = ""
    UserActivity = masterPage.aclsData_Temp.GetMasterUserNoteCountList()

    user_activity_label.Text = "<table width='100%' cellpadding='6' cellspacing='0' class='data_aircraft_grid'>"
    user_activity_label.Text += "<tr class='header_row'>"
    user_activity_label.Text += "<td align='left' valign='top'><b>User</b></td>"
    'Last six months

    For Counter = 6 To 1 Step -1
      user_activity_label.Text += "<td align='left' valign='top'><b>" & MonthName(Month(DateAdd(DateInterval.Month, -Counter, Now()))) & "</b></td>"
    Next

    user_activity_label.Text += "<td align='left' valign='top'><b>Total Notes</b></td>"
    user_activity_label.Text += "<td align='left' valign='top'><b>Action Items</b></td>"
    user_activity_label.Text += "<td align='left' valign='top'><b>Opportunities</b></td>"
    user_activity_label.Text += "<td align='left' valign='top'><b>Exports</b></td>"
    user_activity_label.Text += "</tr>"

    'First let's fill with information with
    'What's returned from the query.
    If Not IsNothing(UserActivity) Then
      If UserActivity.Rows.Count > 0 Then
        For Each r As DataRow In UserActivity.Rows
          user_activity_label.Text += "<tr class='" & css & "'>"
          user_activity_label.Text += "<td align='left' valign='top'>" & r("cliuser_first_name") & " " & r("cliuser_last_name") & "</td>"
          user_activity_label.Text += "<td align='left' valign='top'>" & r("month_one_count").ToString & "</td>"
          user_activity_label.Text += "<td align='left' valign='top'>" & r("month_two_count").ToString & "</td>"
          user_activity_label.Text += "<td align='left' valign='top'>" & r("month_three_count").ToString & "</td>"
          user_activity_label.Text += "<td align='left' valign='top'>" & r("month_four_count").ToString & "</td>"
          user_activity_label.Text += "<td align='left' valign='top'>" & r("month_five_count").ToString & "</td>"
          user_activity_label.Text += "<td align='left' valign='top'>" & r("month_six_count").ToString & "</td>"
          user_activity_label.Text += "<td align='left' valign='top'>" & r("total_note_count").ToString & "</td>"
          user_activity_label.Text += "<td align='left' valign='top'>" & r("total_action_count").ToString & "</td>"
          user_activity_label.Text += "<td align='left' valign='top'>" & r("total_opp_count").ToString & "</td>"
          user_activity_label.Text += "<td align='left' valign='top'>0</td>"
          user_activity_label.Text += "</tr>"

          If css = "" Then
            css = "alt_row"
          Else
            css = ""
          End If
        Next
      End If
    Else
      If masterPage.aclsData_Temp.class_error <> "" Then
        error_string = masterPage.aclsData_Temp.class_error
        masterPage.LogError("home.aspx.vb - FillUserActivityPanel() - " & error_string)
      End If
      masterPage.display_error()
    End If
    UserActivity.Dispose()
    StartingNotesTable.Dispose()

    user_activity_label.Text += "</table>"

    Trace.Write("End FillUserActivityPanel Home.aspx" + Now.ToString)
    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br />End FillUserActivityPanel Home.aspx : " + Now.ToString + "<br />"

  End Sub
  ''' <summary>
  ''' This function creates aircraft analytics on the Evo Home Page.
  ''' </summary>
  ''' <param name="ResultsTable"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Private Function CreateAircraftAnalytics(ByVal ResultsTable As DataTable) As String
    Dim OutputString As String = ""
    Dim cssClass As String = ""

    If Not IsNothing(ResultsTable) Then
      If ResultsTable.Rows.Count > 0 Then
        OutputString = "<table width=""50%"" cellpadding=""3"" cellspacing=""0"" class=""data_aircraft_grid float_left fullWidthMobile"">"
        OutputString += "<caption>My Aircraft</caption>"
        OutputString += "<tr class=""header_row"">"
        OutputString += "<td align=""left"" valign=""top"" width='130'>"
        OutputString += "<b class=""title"">Make</b>"
        OutputString += "</td>"
        OutputString += "<td align=""right"" valign=""top"">"
        OutputString += " <b class=""title"">Ser #</b>"
        OutputString += "</td>"
        OutputString += "<td align=""right"" valign=""top"">"
        OutputString += "<b class=""title"">Reg #</b>"
        OutputString += " </td>"
        OutputString += "<td align=""right"" valign=""top"" colspan='2'>"
        OutputString += "<b class=""title"">Clicks<br/>Since Listed</b>"
        OutputString += " </td>"
        OutputString += "</tr>"

        For Each r As DataRow In ResultsTable.Rows
          OutputString += "<tr class='" & cssClass & "'>"
          OutputString += "<td align=""left"" valign=""top"">"
          '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          ''''''''''''''''''''''''MAKE NAME''''''''''''''''''''''''''''''''''''''
          OutputString += "" & r("amod_make_name").ToString & " "
          '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          ''''''''''''''''''''''''MODEL NAME'''''''''''''''''''''''''''''''''''''
          OutputString += "<a href=""#"" onclick=""javascript:load('DisplayModelDetail.aspx?id=" & r("amod_id").ToString & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;"">" & r("amod_model_name").ToString & "</a>"
          OutputString += "</td>"
          OutputString += "<td align=""right"" valign=""top"">"
          '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          ''''''''''''''''''''''''SERIAL NBR'''''''''''''''''''''''''''''''''''''
          OutputString += "<a href=""#"" onclick=""javascript:DisplayACDetailsWithAnalytics(" & r("ac_id").ToString & ");return false;"">" & r("ac_ser_no_full").ToString & "</a>"
          OutputString += "</td>"
          OutputString += "<td align=""right"" valign=""top"">"
          '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          ''''''''''''''''''''''''REGISTR NBR''''''''''''''''''''''''''''''''''''
          OutputString += r("ac_reg_no").ToString
          OutputString += "</td>"

          OutputString += "<td align=""right"" valign=""top"">"
          '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          '''''''''''''''''''''''''''''CLICKS''''''''''''''''''''''''''''''''''''
          OutputString += "<a href=""#"" onclick=""javascript:DisplayACDetailsWithAnalytics(" & r("ac_id").ToString & ");return false;"">" & r("tcount").ToString & "</a>"

          OutputString += "</td>"
          OutputString += "<td align=""right"" valign=""top"">"
          '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          '''''''''''''''''''''''''''''CLICKS''''''''''''''''''''''''''''''''''''
          OutputString += "<a href=""#"" onclick=""javascript:DisplayACDetailsWithAnalytics(" & r("ac_id").ToString & ");return false;""><img src='images/analytics.jpg' alt='Click to view Stats' border='0'/></a>"
          OutputString += "</td>"
          OutputString += "</tr>"

          If cssClass = "" Then
            cssClass = "alt_row"
          Else
            cssClass = ""
          End If
        Next

        OutputString += "</table>"
      Else
        OutputString = "<table width=""50%"" cellpadding=""3"" cellspacing=""0"" class=""data_aircraft_grid float_left fullWidthMobile""><caption>My Aircraft</caption><tr><td align='left' valign='top'><p align='left'>Welcome " & Session.Item("localUser").crmLocalUserFirstName.ToString & " " & Session.Item("localUser").crmLocalUserLastName.ToString & ".<br /><br />Aircraft Analytics are only displayed for owned or brokered aircraft. There are currently no aircraft analytics for your subscription.</td></tr></table>"
      End If
    Else
      'error logging here.
      Master.LogError("home.aspx.vb - CreateAircraftAnalytics() - " & masterPage.aclsData_Temp.class_error)
      'clear error for data layer class
      masterPage.aclsData_Temp.class_error = ""
    End If
    ResultsTable.Dispose()
    Return OutputString
  End Function
  ''' <summary>
  ''' This Creates the Analytics Tab.
  ''' </summary>
  ''' <remarks></remarks>
  Sub CreateAnalytics()


    Dim htmlOut As New StringBuilder
    Dim x As Integer = 0

    Dim bIsDealer As Boolean = False

    Dim ResultsTable As New DataTable
    Dim WindowLoadJavascript As String = ""
    Dim has_stats As Boolean = False
    MyAnalytics_listing_label.Text = ""

    ResultsTable = masterPage.aclsData_Temp.DisplayACAnalyticInfoBasedOnCompanyID(CLng(Session.Item("localUser").crmUserCompanyID.ToString), 0)
    MyAnalytics_listing_label.Text += CreateAircraftAnalytics(ResultsTable)

    bIsDealer = commonEvo.isDealerCompany(CLng(Session.Item("localUser").crmUserCompanyID.ToString), 0)

    ResultsTable = New DataTable
    ResultsTable = masterPage.aclsData_Temp.DisplayAnalyticInformationSummarizedByDate(CLng(Session.Item("localUser").crmUserCompanyID.ToString), 0, 0, bIsDealer, has_stats)

    If Not IsNothing(ResultsTable) Then

      If ResultsTable.Rows.Count > 0 Then

        MyAnalytics_listing_label.Text += crmWebClient.DisplayFunctions.CreateAnalyticsSummaryByDate(ResultsTable, masterPage, "MY COMPANY", "46", True, bIsDealer, has_stats)

        htmlOut.Append(" var data = new google.visualization.DataTable();" + vbCrLf)
        htmlOut.Append(" data.addColumn('string', 'Month');" + vbCrLf)
        htmlOut.Append(" data.addColumn('number', 'Evolution Clicks');" + vbCrLf)

        If bIsDealer And has_stats = True Then
          htmlOut.Append(" data.addColumn('number', 'Global Clicks');" + vbCrLf)
        End If

        htmlOut.Append(" data.addRows(" + ResultsTable.Rows.Count.ToString + ");" + vbCrLf)

        For Each r As DataRow In ResultsTable.Rows
          htmlOut.Append(" data.setCell(" + x.ToString + ", 0, '" + r.Item("YTMONTH").ToString + "-" + r.Item("YTYEAR").ToString + "');" + vbCrLf)
          htmlOut.Append(" data.setCell(" + x.ToString + ", 1, " + IIf(CLng(r.Item("tcount").ToString) > 0, FormatNumber(r.Item("tcount").ToString, 0, TriState.False, TriState.False, TriState.False).ToString, "0") + ");" + vbCrLf)

          If bIsDealer And has_stats = True Then
            htmlOut.Append(" data.setCell(" + x.ToString + ", 2, " + IIf(CLng(r.Item("gcount").ToString) > 0, FormatNumber(r.Item("gcount").ToString, 0, TriState.False, TriState.False, TriState.False).ToString, "0") + ");" + vbCrLf)
          End If

          x += 1

        Next

        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FillDataAnalytics", htmlOut.ToString, True)

        'Because below IE8 is special and doesn't use listeners in the same way as everywhere else
        'We have to check to see if the event listener is available first,
        'If not, we attachEvent and basically do the same thing.

        WindowLoadJavascript = "if (window.addEventListener) {"

        WindowLoadJavascript += " window.addEventListener(""load"", "
        WindowLoadJavascript += " function () {"
        WindowLoadJavascript += " google.charts.load('current', { packages: ['corechart'] });"
        WindowLoadJavascript += " google.charts.setOnLoadCallback(drawVisualization);"
        WindowLoadJavascript += "}"
        WindowLoadJavascript += ",false);"
        WindowLoadJavascript += "}" 'Else 
        WindowLoadJavascript += "else {"

        WindowLoadJavascript += " window.attachEvent(""load"","
        WindowLoadJavascript += " function () {"
        WindowLoadJavascript += " google.charts.load('current', { packages: ['corechart'] });"
        WindowLoadJavascript += " google.charts.setOnLoadCallback(drawVisualization);"
        WindowLoadJavascript += "}"
        WindowLoadJavascript += ");"
        WindowLoadJavascript += "}" 'End if


        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ToggleAnalytics", WindowLoadJavascript, True)
        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ToggleAnalyticsTabChange", "Sys.Application.add_load(function() {drawVisualization();});", True)

      End If

    End If

    ResultsTable.Dispose()

  End Sub
  ''' <summary>
  ''' Creates Action Items on Evo side
  ''' </summary>
  ''' <remarks></remarks>
  Sub Create_Evo_Action_Items(ByVal ActionItemsLabel As Label)

    Trace.Write("Start Create_Evo_Action_Items Home.aspx" + Now.ToString)
    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br />Start Create_Evo_Action_Items Home.aspx : " + Now.ToString + "<br />"

    Dim NumberOfActionDays As Integer = 0

    Select Case action_time.SelectedValue
      Case "5"
        NumberOfActionDays = 5
      Case "14"
        NumberOfActionDays = 14
      Case "30"
        NumberOfActionDays = 30
      Case Else
        NumberOfActionDays = 7
    End Select

    Dim notesTable As New DataTable
    If Session.Item("localSubscription").crmServerSideNotes_Flag = True Then
      notesTable = masterPage.aclsData_Temp.Get_Local_Notes_GetByUserIDStatusLessThanDate(Session.Item("localUser").crmLocalUserID, FormatDateTime(DateAdd(DateInterval.Day, NumberOfActionDays, Now()), DateFormat.ShortDate), "P")
      ActionItemsLabel.Text = DisplayFunctions.Display_Notes_Or_Actions(notesTable, masterPage.aclsData_Temp, True, True, True, False, True, False, True)
    ElseIf Session.Item("localSubscription").crmCloudNotes_Flag = True Then
      notesTable = masterPage.aclsData_Temp.Get_CloudNotes_GetByUserIDStatusLessThanDate(Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmUserLogin, FormatDateTime(DateAdd(DateInterval.Day, NumberOfActionDays, Now()), DateFormat.ShortDate), "P")
      ActionItemsLabel.Text = DisplayFunctions.Display_Notes_Or_Actions(notesTable, masterPage.aclsData_Temp, True, True, True, True, True, False, True)
    End If

    Trace.Write("End Create_Evo_Action_Items Home.aspx" + Now.ToString)
    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br />End Create_Evo_Action_Items Home.aspx : " + Now.ToString + "<br />"

  End Sub
  ''' <summary>
  ''' This sub is going to replace all of the recently edited/viewed functions.
  ''' </summary>
  ''' <remarks></remarks>
  Sub Create_Recent_Items(ByVal UserIDCompare As String, ByVal CompanyRecent As TreeView, ByVal Company As Boolean, ByVal Contact As Boolean, ByVal Aircraft As Boolean, ByVal Yacht As Boolean, ByVal CookieName As String)
    'let's trace this out first.
    'First when we come to the page
    'we need to:
    '1.) Access Request Cookies
    '2.) If use cookies:
    'a.) Grab the information from cookies
    'b.) If client: 
    'aa.) Branch off into a seperate function that alters the table and queries for the client aircraft.
    'c.) if jetnet:
    'aa.) Branch off into a seperate function that alters the table and queries for jetnet aircraft.
    '3.) If not use cookies and CRM client:
    '4.) query client DB for most recent edit.
    Dim CompanyRecentNode As New TreeNode
    CompanyRecentNode.Text = "Recently Viewed " & CookieName 'Companies"

    Trace.Write("Start Create_Recent_Items Home.aspx" + Now.ToString)
    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br />Start Create_Recent_Items Home.aspx : " + Now.ToString + "<br />"

    'Dim Recent_Aircraft_Table As New Table 'Table object to hold recent aircraft.
    Dim TR As New TableRow 'table row object
    Dim TD As New TableCell 'table cell object
    Dim count As Integer = 0
    Dim holding_count As Integer = 0
    Dim UsingCookies As Boolean = False 'boolean for whether or not to use cookies. It's worth noting that if the app is in EVO mode, if this turns false, nothing gets displayed.
    Dim _RecentCompanyCookies As HttpCookie = Request.Cookies(CookieName)
    Dim USER As String = ""

    If _RecentCompanyCookies IsNot Nothing Then
      USER = _RecentCompanyCookies("USER")
      'this line means that user of the cookie matches the user viewing
      If USER = CStr(UserIDCompare) Then
        If _RecentCompanyCookies("ID") <> "" Then 'this means there are IDs stored in the cookie
          UsingCookies = True
        End If
      End If
    End If

    If UsingCookies = True Then
      'if the application comes in cookie ready
      'Set up the following variables
      USER = _RecentCompanyCookies("USER")
      Dim STORED_IDS As String = _RecentCompanyCookies("ID")
      Dim STORED_SOURCE As String = _RecentCompanyCookies("SOURCE")
      Dim ARRAY_OF_SOURCE As Array = Split(STORED_SOURCE, "|")
      Dim ID_ARRAY As Array = Split(STORED_IDS, "|")
      Dim TOPNUMBER As Integer = UBound(ID_ARRAY)
      Dim Company_JetnetID_String As String = ""
      Dim Company_ClientID_String As String = ""
      Dim AC_JetnetID_String As String = ""
      Dim AC_ClientID_String As String = ""
      Dim Contact_JetnetID_String As String = ""
      Dim Contact_ClientID_String As String = ""
      Dim Yacht_JetnetID_String As String = ""

      Dim HoldTable As New DataTable
      'loop through the cookie arrays
      For i As Integer = 0 To TOPNUMBER
        If ID_ARRAY(i) <> "" Then
          holding_count = TOPNUMBER
          If holding_count = 0 Then
            holding_count = 1
          End If
          Select Case UCase(CookieName)
            Case "COMPANIES"
              Select Case ARRAY_OF_SOURCE(i)
                Case "JETNET"
                  If Company_JetnetID_String <> "" Then
                    Company_JetnetID_String += ", "
                  End If
                  Company_JetnetID_String += ID_ARRAY(i).ToString
                Case "CLIENT"
                  If Company_ClientID_String <> "" Then
                    Company_ClientID_String += ", "
                  End If
                  Company_ClientID_String += ID_ARRAY(i).ToString
              End Select
            Case "AIRCRAFT"
              Select Case ARRAY_OF_SOURCE(i)
                Case "JETNET"
                  If AC_JetnetID_String <> "" Then
                    AC_JetnetID_String += ", "
                  End If
                  AC_JetnetID_String += ID_ARRAY(i).ToString
                Case "CLIENT"
                  If AC_ClientID_String <> "" Then
                    AC_ClientID_String += ", "
                  End If
                  AC_ClientID_String += ID_ARRAY(i).ToString
              End Select
            Case "CONTACTS"
              Select Case ARRAY_OF_SOURCE(i)
                Case "JETNET"
                  If Contact_JetnetID_String <> "" Then
                    Contact_JetnetID_String += ", "
                  End If
                  Contact_JetnetID_String += ID_ARRAY(i).ToString
                Case "CLIENT"
                  If Contact_ClientID_String <> "" Then
                    Contact_ClientID_String += ", "
                  End If
                  Contact_ClientID_String += ID_ARRAY(i).ToString
              End Select
            Case "YACHTS"
              If Yacht_JetnetID_String <> "" Then
                Yacht_JetnetID_String += ", "
              End If
              Yacht_JetnetID_String += ID_ARRAY(i).ToString
          End Select
        End If
      Next
      Dim ClientTable As New DataTable

      Select Case UCase(CookieName)
        Case "COMPANIES"
          If Company_JetnetID_String <> "" Then
            'Run company query
            HoldTable = masterPage.aclsData_Temp.GetLimited_CompanyInfo_InClause(Company_JetnetID_String, "JETNET")
          End If
          If Company_ClientID_String <> "" Then
            'Run company query
            ClientTable = masterPage.aclsData_Temp.GetLimited_CompanyInfo_InClause(Company_ClientID_String, "CLIENT")
          End If

          HoldTable = combineNoteTables(HoldTable, ClientTable, False)

          If HoldTable.Rows.Count > 0 Then
            Display_Jetnet_Company(HoldTable, "JETNET", CompanyRecentNode, holding_count)
          End If
        Case "CONTACTS"

          If Contact_JetnetID_String <> "" Then
            HoldTable = masterPage.aclsData_Temp.GetContacts_Details_InClause(Contact_JetnetID_String, "JETNET")
          End If

          If Contact_ClientID_String <> "" Then
            If clsGeneral.clsGeneral.isCrmDisplayMode Then
              ClientTable = GetClient_Contact_NAME_InClause(Contact_ClientID_String)
            End If
          End If

          HoldTable = combineNoteTables(HoldTable, ClientTable, True)

          If HoldTable.Rows.Count > 0 Then
            Build_Recent_Contacts(HoldTable, "JETNET", New Table, False, CompanyRecentNode, True)
          End If

        Case "AIRCRAFT"

          If AC_JetnetID_String <> "" Then
            HoldTable = masterPage.aclsData_Temp.GetJETNET_AC_NAME_InClause(AC_JetnetID_String, "JETNET")
          End If
          If AC_ClientID_String <> "" Then
            If clsGeneral.clsGeneral.isCrmDisplayMode Then
              ClientTable = GetClient_AC_NAME_InClause(AC_ClientID_String, "")
            End If
          End If

          HoldTable = combineNoteTables(HoldTable, ClientTable, True)

          If HoldTable.Rows.Count > 0 Then
            Display_Jetnet_Aircraft(HoldTable, "JETNET", CompanyRecentNode, holding_count)
          End If

        Case "YACHTS"
          If Yacht_JetnetID_String <> "" Then
            HoldTable = masterPage.aclsData_Temp.DisplayYachtByIDClause(Yacht_JetnetID_String)
            Display_Jetnet_Yacht(HoldTable, CompanyRecentNode, holding_count)
          End If
      End Select
    ElseIf UsingCookies = False And Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then
      'Blank for right now
    End If

    If holding_count > 0 Then
      CompanyRecent.Nodes.Add(CompanyRecentNode)
      CompanyRecent.Visible = True
    End If

    Trace.Write("End Create_Recent_Items Home.aspx" + Now.ToString)
    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br />End Create_Recent_Items Home.aspx : " + Now.ToString + "<br />"

  End Sub
  Public Function GetClient_Contact_NAME_InClause(ByVal contactIDs As String) As DataTable
    Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
    Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
    Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing
    Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader : MySqlReader = Nothing
    Dim aTempTable As New DataTable
    Dim sql As String = ""

    Try

      sql = "SELECT clicontact_action_date AS contact_action_date, clicontact_comp_id AS contact_comp_id, clicontact_email_address AS contact_email_address, clicontact_first_name AS contact_first_name,"
      sql += " clicontact_id AS contact_id, clicontact_last_name AS "
      sql += " contact_last_name, clicontact_middle_initial AS contact_middle_initial, clicontact_sirname AS contact_sirname, clicomp_name as comp_name, "
      sql += " clicontact_suffix AS contact_suffix, clicontact_title AS contact_title, 'CLIENT' as source, 'CLIENT' as contact_type "
      sql += " FROM client_contact  "
      sql += " inner join client_company on clicontact_comp_id = clicomp_id "
      sql += " WHERE clicontact_id in (" & contactIDs & ") "

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>GetClient_Contact_NAME_InClause(ByVal contactIDs As String) As DataTable</b><br />" & sql


      MySqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")
      MySqlConn.Open()
      MySqlCommand.Connection = MySqlConn
      MySqlCommand.CommandType = CommandType.Text
      MySqlCommand.CommandTimeout = 60

      MySqlCommand.CommandText = sql
      MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        aTempTable.Load(MySqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = aTempTable.GetErrors()
      End Try
      Return aTempTable
    Catch ex As Exception
      GetClient_Contact_NAME_InClause = Nothing
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in GetClient_Contact_NAME_InClause(ByVal contactIDs As String) As DataTable: " & ex.Message
    Finally
      MySqlConn.Dispose()
      MySqlConn.Close()
      MySqlConn = Nothing
      MySqlCommand.Dispose()
      MySqlCommand = Nothing
    End Try

  End Function

  Public Function GetClient_AC_NAME_InClause(ByVal ac_id As String, ByRef aError As String) As DataTable
    Dim MySqlConn As New MySql.Data.MySqlClient.MySqlConnection
    Dim MySqlCommand As New MySql.Data.MySqlClient.MySqlCommand
    Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing
    Dim MySqlReader As MySql.Data.MySqlClient.MySqlDataReader : MySqlReader = Nothing
    Dim aTempTable As New DataTable
    Dim sql As String = ""

    Try

      sql = sql & "Select DISTINCT "
      sql = sql & "client_aircraft.cliaircraft_id as ac_id, 'CLIENT' as source,  "
      sql = sql & " client_aircraft.cliaircraft_ser_nbr as ac_ser_nbr, client_aircraft.cliaircraft_reg_nbr as ac_reg_nbr, "
      sql = sql & " client_aircraft.cliaircraft_year_mfr as ac_year_mfr, client_aircraft_model.cliamod_id as amod_id, "
      sql = sql & " client_aircraft_model.cliamod_make_name as amod_make_name, client_aircraft_model.cliamod_model_name as amod_model_name "
      sql = sql & " FROM client_aircraft INNER JOIN "
      sql = sql & " client_aircraft_model on client_aircraft.cliaircraft_cliamod_id = client_aircraft_model.cliamod_id "
      sql = sql & " WHERE cliaircraft_id in (" & ac_id & ")  "

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>GetClient_AC_NAME_InClause(ByVal ac_id As String, ByRef aError As String) As DataTable</b><br />" & sql


      MySqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetServerNotesDatabase")
      MySqlConn.Open()
      MySqlCommand.Connection = MySqlConn
      MySqlCommand.CommandType = CommandType.Text
      MySqlCommand.CommandTimeout = 60

      MySqlCommand.CommandText = sql
      MySqlReader = MySqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        aTempTable.Load(MySqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = aTempTable.GetErrors()
      End Try
      Return aTempTable
    Catch ex As Exception
      GetClient_AC_NAME_InClause = Nothing
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in GetClient_AC_NAME_InClause(ByVal ac_id As String, ByRef aError As String) As DataTable: " & ex.Message
    Finally
      MySqlConn.Dispose()
      MySqlConn.Close()
      MySqlConn = Nothing
      MySqlCommand.Dispose()
      MySqlCommand = Nothing
    End Try

  End Function

  Function combineNoteTables(ByVal jetnetTable As DataTable, ByVal clientTable As DataTable, ByVal addSource As Boolean) As DataTable
    Dim Column As New DataColumn
    Dim returnTable As New DataTable

    If addSource Then
      'Going ahead to add the Source to the Jetnet Datatable, let's you know what type of data you're displaying. 
      Column.DataType = System.Type.GetType("System.String")
      Column.DefaultValue = "JETNET"
      Column.Unique = False
      Column.ColumnName = "source"
      jetnetTable.Columns.Add(Column)
    End If

    returnTable = jetnetTable.Clone

    returnTable.Merge(jetnetTable)
    returnTable.Merge(clientTable)
    Return returnTable
  End Function

  ''' <summary>
  ''' This is a function that is going to display recently viewed yachts
  ''' </summary>
  ''' <param name="RecentYachtTable"></param>
  ''' <param name="YachtRecentNode"></param>
  ''' <remarks></remarks>
  Sub Display_Jetnet_Yacht(ByVal RecentYachtTable As DataTable, ByRef YachtRecentNode As TreeNode, ByRef holding_count As Integer)
    Dim COUNT As Integer = 0
    Dim SubNode As New TreeNode

    Try

      If Not IsNothing(RecentYachtTable) Then
        If RecentYachtTable.Rows.Count > 0 Then
          For Each r As DataRow In RecentYachtTable.Rows
            SubNode = New TreeNode
            SubNode.ImageUrl = "images/final.png"

            SubNode.Text = "<a " & DisplayFunctions.WriteYachtDetailsLink(r("yt_id"), False, "", "", "") & ">" & r("yt_yacht_name").ToString & "</a> Hull # " & r("yt_hull_mfr_nbr").ToString
            SubNode.Target = "new"
            YachtRecentNode.ChildNodes.Add(SubNode)

            COUNT = COUNT - 1
          Next
        End If
      Else
        If masterPage.aclsData_Temp.class_error <> "" Then
          error_string = masterPage.aclsData_Temp.class_error
          masterPage.LogError("home.aspx.vb - Display_Jetnet_Yacht(ByVal Yacht_ID As Long, ByRef YachtRecentNode As TreeNode, ByRef COUNT As Integer) - " & error_string)
        End If
        masterPage.display_error()
      End If
    Catch ex As Exception
      masterPage.LogError("home.aspx.vb - Display_Jetnet_Aircraft(ByVal AC_ID As Long, ByRef Recent_Aircraft_Table As Table, ByRef holding_count As Integer) - " & ex.Message)
    End Try

  End Sub
  ''' <summary>
  ''' This is part of the function that will take place of Recently_Edited_Viewed_AC
  ''' </summary>
  '''<param name="CompanySource"></param>
  ''' <param name="CompanyRecentNode"></param>
  ''' <param name="holding_count"></param>
  ''' <remarks></remarks>
  Sub Display_Jetnet_Company(ByVal RecentCompanyTable As DataTable, ByVal CompanySource As String, ByRef CompanyRecentNode As TreeNode, ByRef holding_count As Integer)
    Dim subnode As TreeNode
    Dim COUNT As Integer = 0
    Dim Link_Text As String = ""

    Trace.Write("Start Display_Jetnet_Company Home.aspx" + Now.ToString)
    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br />Start Display_Jetnet_Company Home.aspx : " + Now.ToString + "<br />"

    If Not IsNothing(RecentCompanyTable) Then
      If RecentCompanyTable.Rows.Count > 0 Then
        For i = 0 To RecentCompanyTable.Rows.Count - 1 Step 1
          subnode = New TreeNode
          subnode.ImageUrl = "images/final.png"

          Link_Text = CompanyFunctions.Display_Company_Information_For_Link(RecentCompanyTable, False, i)

          If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then
            subnode.Text = Link_Text
            subnode.NavigateUrl = "details.aspx?comp_ID=" & RecentCompanyTable.Rows(i).Item("comp_id") & "&type=1&source=" & CompanySource & ""
          Else
            subnode.Text = "<a onclick=""javascript:load('DisplayCompanyDetail.aspx?compid=" & RecentCompanyTable.Rows(i).Item("comp_id") & IIf(RecentCompanyTable.Rows(i).Item("source") = "CLIENT", "&source=CLIENT", "") & "','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;"" class=""cursor"">" & RecentCompanyTable.Rows(i).Item("comp_name")

            If Not IsDBNull(RecentCompanyTable.Rows(i).Item("comp_city")) Then
              If RecentCompanyTable.Rows(i).Item("comp_city") <> "" Then
                subnode.Text += " " & RecentCompanyTable.Rows(i).Item("comp_city").ToString
              End If
            End If
            If Not IsDBNull(RecentCompanyTable.Rows(i).Item("comp_state")) Then
              If RecentCompanyTable.Rows(i).Item("comp_state") <> "" Then
                subnode.Text += ", " & RecentCompanyTable.Rows(i).Item("comp_state").ToString
              End If
            End If

            subnode.Text += "</a>"
            subnode.NavigateUrl = "#"
            subnode.Target = "new"
          End If

          CompanyRecentNode.ChildNodes.Add(subnode)

          COUNT = COUNT - 1
        Next
      End If
    Else
      If masterPage.aclsData_Temp.class_error <> "" Then
        error_string = masterPage.aclsData_Temp.class_error
        masterPage.LogError("home.aspx.vb - Display_Jetnet_Aircraft(ByVal AC_ID As Long, ByRef Recent_Aircraft_Table As Table, ByRef holding_count As Integer) - " & error_string)
      End If
      masterPage.display_error()
    End If

    Trace.Write("End Display_Jetnet_Company Home.aspx" + Now.ToString)
    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br />End Display_Jetnet_Company Home.aspx : " + Now.ToString + "<br />"

  End Sub
  ''' <summary>
  ''' This is part of the function CreateRecentItems, it displays aircraft
  ''' </summary>
  ''' <param name="RecentACTable">aircraft ID</param>
  ''' <param name="ACSource"></param>
  ''' <param name="AircraftRecentNode">table to be modified</param>
  ''' <param name="holding_count"></param>
  ''' <remarks></remarks>
  Sub Display_Jetnet_Aircraft(ByVal RecentACTable As DataTable, ByVal ACSource As String, ByRef AircraftRecentNode As TreeNode, ByRef holding_count As Integer)
    Dim subnode As TreeNode
    Dim COUNT As Integer = 0
    Dim link_text As String = ""

    Trace.Write("Start Display_Jetnet_Aircraft Home.aspx" + Now.ToString)
    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br />Start Display_Jetnet_Aircraft Home.aspx : " + Now.ToString + "<br />"


    If Not IsNothing(RecentACTable) Then
      If RecentACTable.Rows.Count > 0 Then
        For i = 0 To RecentACTable.Rows.Count - 1 Step 1
          subnode = New TreeNode
          subnode.ImageUrl = "images/final.png"

          link_text = CommonAircraftFunctions.Display_Aircraft_Information_For_Link(RecentACTable, False, i)

          If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then
            subnode.Text = link_text
            subnode.NavigateUrl = "details.aspx?ac_ID=" & RecentACTable.Rows(i).Item("ac_id") & "&type=3&source=" & ACSource & ""
          Else
            subnode.Text = crmWebClient.DisplayFunctions.WriteDetailsLink(RecentACTable.Rows(i).Item("ac_id"), 0, 0, 0, True, link_text, "", IIf(RecentACTable.Rows(i).Item("source") = "CLIENT", "&source=CLIENT", ""))
            subnode.NavigateUrl = "#"
            subnode.Target = "new"
          End If

          AircraftRecentNode.ChildNodes.Add(subnode)

          COUNT = COUNT - 1
        Next
      End If
    Else
      If masterPage.aclsData_Temp.class_error <> "" Then
        error_string = masterPage.aclsData_Temp.class_error
        masterPage.LogError("home.aspx.vb - Display_Jetnet_Aircraft(ByVal AC_ID As Long, ByRef Recent_Aircraft_Table As Table, ByRef holding_count As Integer) - " & error_string)
      End If
      masterPage.display_error()
    End If

    Trace.Write("End Display_Jetnet_Aircraft Home.aspx" + Now.ToString)
    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br />End Display_Jetnet_Aircraft Home.aspx : " + Now.ToString + "<br />"

  End Sub

  Sub Build_Recent_Contacts(ByVal RecentContactTable As DataTable, ByVal Source As String, ByRef Recent_Contact_Table As Table, ByVal DisplayCRM As Boolean, ByRef ContactTreeNode As TreeNode, ByVal UseTreeView As Boolean)
    Dim TR As New TableRow
    Dim TD As New TableCell
    Dim TD_2 As New TableCell
    Dim DisplayLabel As New Label
    Dim subnode As New TreeNode
    Dim LinkText As String = ""

    Trace.Write("Start Build_Recent_Contacts Home.aspx" + Now.ToString)
    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br />Start Build_Recent_Contacts Home.aspx : " + Now.ToString + "<br />"


    If Not IsNothing(RecentContactTable) Then
      If RecentContactTable.Rows.Count > 0 Then
        For i = 0 To RecentContactTable.Rows.Count - 1 Step 1
          subnode = New TreeNode
          TD_2 = New TableCell
          DisplayLabel = New Label
          TR = New TableRow
          TD = New TableCell
          TD.Width = 18

          If UseTreeView = False Then
            TD.Text = "<img src='images/final.jpg' alt='" & RecentContactTable.Rows(i).Item("contact_first_name") & " " & RecentContactTable.Rows(i).Item("contact_last_name") & "' />"
            TD.VerticalAlign = VerticalAlign.Top
            LinkText = "<a href='details.aspx?comp_ID=" & RecentContactTable.Rows(i).Item("contact_comp_id") & "&contact_ID=" & RecentContactTable.Rows(i).Item("contact_id") & "&type=1&source=" & Source & "'>" & RecentContactTable.Rows(i).Item("contact_first_name") & " " & RecentContactTable.Rows(i).Item("contact_last_name") & ", " & IIf(Not IsDBNull(RecentContactTable.Rows(i).Item("comp_name")), RecentContactTable.Rows(i).Item("comp_name"), "") & "</a>"
            DisplayLabel.Text = LinkText

            'Add to the table
            TD_2.Controls.Add(DisplayLabel)
            TR.Controls.Add(TD)
            TR.Controls.Add(TD_2)
            Recent_Contact_Table.Controls.Add(TR)
          Else
            subnode.ImageUrl = "images/final.png"
            LinkText = DisplayFunctions.WriteDetailsLink(0, RecentContactTable.Rows(i).Item("contact_comp_id"), RecentContactTable.Rows(i).Item("contact_id"), 0, True, RecentContactTable.Rows(i).Item("contact_first_name") & " " & RecentContactTable.Rows(i).Item("contact_last_name") & ", " & IIf(Not IsDBNull(RecentContactTable.Rows(i).Item("comp_name")), RecentContactTable.Rows(i).Item("comp_name"), ""), "", IIf(RecentContactTable.Rows(i).Item("source") = "CLIENT", "&source=CLIENT", ""))
            subnode.Text = "" & LinkText
            subnode.NavigateUrl = "#"
            subnode.Target = "new"

            ContactTreeNode.ChildNodes.Add(subnode)
          End If
        Next
      End If


    Else
      If masterPage.aclsData_Temp.class_error <> "" Then
        error_string = masterPage.aclsData_Temp.class_error
        masterPage.LogError("home.aspx.vb - recently_edited_viewed_contacts() - " & error_string)
      End If
      masterPage.display_error()
    End If

    Trace.Write("End Build_Recent_Contacts Home.aspx" + Now.ToString)
    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br />End Build_Recent_Contacts Home.aspx : " + Now.ToString + "<br />"

  End Sub

  Sub Display_Client_Aircraft(ByVal AC_ID As Long, ByRef Recent_Aircraft_Table As Table, ByRef holding_count As Integer)
    Dim TR As New TableRow
    Dim TD As New TableCell
    Dim TD_2 As New TableCell
    Dim T_Label As New Label
    Dim COUNT As Integer = 0
    aTempTable = masterPage.aclsData_Temp.Get_Clients_Aircraft_Ser_Model(AC_ID)

    If Not IsNothing(aTempTable) Then
      If aTempTable.Rows.Count > 0 Then
        For Each r As DataRow In aTempTable.Rows
          Dim name_string As String = ""
          TR = New TableRow
          TD = New TableCell
          TD.Width = 18
          If Not IsDBNull(r("cliamod_make_name")) And Not IsDBNull(r("cliamod_model_name")) Then
            name_string = r("cliamod_make_name") & " " & r("cliamod_model_name")
          End If

          TD.Text = "<img src='images/final.jpg' alt='" & name_string & "' />"
          TD.VerticalAlign = VerticalAlign.Top
          TD_2 = New TableCell
          T_Label = New Label
          Dim link_text As String = ""
          If Not IsDBNull(r("cliaircraft_year_mfr")) And Not IsDBNull(r("cliaircraft_ser_nbr")) Then
            link_text = r("cliaircraft_year_mfr") & " " & name_string & ", Ser #" & r("cliaircraft_ser_nbr")
          End If
          If Not IsDBNull(r("cliaircraft_reg_nbr")) Then
            If r("cliaircraft_reg_nbr") <> "" Then
              link_text = link_text & ", Reg#" & r("cliaircraft_reg_nbr")
            End If
          End If
          T_Label.Text = "<a href='details.aspx?ac_ID=" & r("cliaircraft_id") & "&type=3&source=CLIENT'>" & link_text & "</a>"

          TD_2.Controls.Add(T_Label)
          TR.Controls.Add(TD)
          TR.Controls.Add(TD_2)
          Recent_Aircraft_Table.Controls.Add(TR)
          COUNT = COUNT - 1

        Next
      End If
    Else
      If masterPage.aclsData_Temp.class_error <> "" Then
        error_string = masterPage.aclsData_Temp.class_error
        masterPage.LogError("home.aspx.vb - Display_Client_Aircraft(ByVal AC_ID As Long, ByRef Recent_Aircraft_Table As Table, ByRef holding_count As Integer) - " & error_string)
      End If
      masterPage.display_error()
    End If
  End Sub

  Sub recently_edited_viewed_ac()
    Try
      'Setup Recently Edited AC
      Dim tr As New TableRow
      Dim td As New TableCell
      Dim td2 As New TableCell
      Dim linky As New Label

      Dim count As Integer = 0
      Dim holding_count As Integer = 0

      Dim _aircraftCookies As HttpCookie = Request.Cookies("aircraft")

      Dim recent_aircraft As New Table
      recent_aircraft.Width = Unit.Percentage(100)
      Dim use_cookie As Boolean = False
      If _aircraftCookies IsNot Nothing Then
        Dim user As String = _aircraftCookies("USER")
        If user = CStr(Session.Item("localUser").crmLocalUserID) Then 'Cookie is for this user!
          If _aircraftCookies("ID") <> "" Then
            use_cookie = True
          End If
        End If
      End If

      If use_cookie = True Then 'use recently edited.
        Dim user As String = _aircraftCookies("USER")
        Dim stored_id As String = _aircraftCookies("ID")
        Dim stored_source As String = _aircraftCookies("SOURCE")
        Dim source_array As Array = Split(stored_source, "|")
        Dim id_array As Array = Split(stored_id, "|")
        Dim topnumber As Integer = UBound(id_array)


        td = New TableCell
        td2 = New TableCell
        tr = New TableRow
        recent_aircraft.CssClass = "data_aircraft_grid" '"most_recent"
        recent_aircraft.CellPadding = 3
        td.ColumnSpan = 2
        tr.CssClass = "header_row"
        td.Text = "<b>Recently Viewed Aircraft</b>"
        tr.Controls.Add(td)
        recent_aircraft.Controls.Add(tr)

        For i As Integer = 0 To topnumber
          If id_array(i) <> "" Then
            holding_count = topnumber
            If holding_count = 0 Then
              holding_count = 1
            End If
            Select Case source_array(i)

              Case "CLIENT"
                aTempTable = masterPage.aclsData_Temp.Get_Clients_Aircraft_Ser_Model(id_array(i))

                If Not IsNothing(aTempTable) Then
                  If aTempTable.Rows.Count > 0 Then
                    For Each r As DataRow In aTempTable.Rows

                      tr = New TableRow
                      td = New TableCell
                      td.Width = 18
                      Dim name_string As String = ""

                      If Not IsDBNull(r("cliamod_make_name")) And Not IsDBNull(r("cliamod_model_name")) Then
                        name_string = r("cliamod_make_name") & " " & r("cliamod_model_name")
                      End If


                      td.Text = "<img src='images/final.jpg' alt='" & name_string & "' />"
                      td.VerticalAlign = VerticalAlign.Top
                      td2 = New TableCell
                      linky = New Label
                      Dim link_text As String = ""
                      If Not IsDBNull(r("cliaircraft_year_mfr")) And Not IsDBNull(r("cliaircraft_ser_nbr")) Then
                        link_text = r("cliaircraft_year_mfr") & " " & name_string & ", Ser #" & r("cliaircraft_ser_nbr")
                      End If
                      If Not IsDBNull(r("cliaircraft_reg_nbr")) Then
                        If r("cliaircraft_reg_nbr") <> "" Then
                          link_text = link_text & ", Reg#" & r("cliaircraft_reg_nbr")
                        End If
                      End If
                      linky.Text = "<a href='details.aspx?ac_ID=" & r("cliaircraft_id") & "&type=3&source=" & source_array(i) & "'>" & link_text & "</a>"

                      td2.Controls.Add(linky)
                      tr.Controls.Add(td)
                      tr.Controls.Add(td2)
                      recent_aircraft.Controls.Add(tr)
                      count = count - 1

                    Next
                  End If
                Else
                  If masterPage.aclsData_Temp.class_error <> "" Then
                    error_string = masterPage.aclsData_Temp.class_error
                    masterPage.LogError("home.aspx.vb - recently_edited_viewed_ac() - " & error_string)
                  End If
                  masterPage.display_error()
                End If

              Case "JETNET"
                aTempTable = masterPage.aclsData_Temp.GetJETNET_AC_NAME(id_array(i), "")
                If Not IsNothing(aTempTable) Then
                  If aTempTable.Rows.Count > 0 Then


                    tr = New TableRow
                    td = New TableCell
                    td.Width = 18
                    Dim name_string As String = ""

                    name_string = aTempTable.Rows(0).Item("amod_make_name") & " " & aTempTable.Rows(0).Item("amod_model_name")

                    td.Text = "<img src='images/final.jpg' alt='" & name_string & "' />"
                    td.VerticalAlign = VerticalAlign.Top
                    td2 = New TableCell
                    linky = New Label

                    Dim link_text As String = aTempTable.Rows(0).Item("ac_year_mfr") & " " & name_string & ", Ser #" & aTempTable.Rows(0).Item("ac_ser_nbr")
                    If Not IsDBNull(aTempTable.Rows(0).Item("ac_reg_nbr")) Then
                      If aTempTable.Rows(0).Item("ac_reg_nbr") <> "" Then
                        link_text = link_text & ", Reg#" & aTempTable.Rows(0).Item("ac_reg_nbr")
                      End If
                    End If

                    linky.Text = "<a href='details.aspx?ac_ID=" & aTempTable.Rows(0).Item("ac_id") & "&type=3&source=" & source_array(i) & "'>" & link_text & "</a>"

                    td2.Controls.Add(linky)
                    tr.Controls.Add(td)
                    tr.Controls.Add(td2)
                    recent_aircraft.Controls.Add(tr)
                    count = count - 1


                  End If
                Else
                  If masterPage.aclsData_Temp.class_error <> "" Then
                    error_string = masterPage.aclsData_Temp.class_error
                    masterPage.LogError("home.aspx.vb - recently_edited_viewed_ac() - " & error_string)
                  End If
                  masterPage.display_error()
                End If

            End Select
          End If
        Next

      Else
        aTempTable = masterPage.aclsData_Temp.Get_LatestClients_Aircraft_ByUserID(Session.Item("localUser").crmLocalUserID)

        td = New TableCell
        td2 = New TableCell
        tr = New TableRow
        recent_aircraft.CssClass = "data_aircraft_grid" '"most_recent"
        recent_aircraft.CellPadding = 3
        td.ColumnSpan = 2
        tr.CssClass = "header_row"
        td.Text = "<b>Recently Edited Aircraft</b>"
        tr.Controls.Add(td)
        recent_aircraft.Controls.Add(tr)

        If Not IsNothing(aTempTable) Then
          count = aTempTable.Rows.Count
          holding_count = aTempTable.Rows.Count
        End If
        If count > 5 Then
          count = 5
        End If

        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then

            If count <> 0 Then
              tr = New TableRow
              td = New TableCell
              td.Width = 18
              Dim name_string As String = ""
              aTempTable2 = masterPage.aclsData_Temp.Get_Clients_Aircraft_Model_amodID(aTempTable.Rows(0).Item("cliaircraft_cliamod_id"))
              If Not IsNothing(aTempTable2) Then
                If aTempTable2.Rows.Count > 0 Then
                  For Each q As DataRow In aTempTable2.Rows
                    name_string = q("cliamod_make_name") & " " & q("cliamod_model_name")
                  Next
                Else
                  name_string = ""
                End If
              Else
                If masterPage.aclsData_Temp.class_error <> "" Then
                  error_string = masterPage.aclsData_Temp.class_error
                  masterPage.LogError("home.aspx.vb - recently_edited_viewed_ac() - " & error_string)
                End If
                masterPage.display_error()
              End If

              td.Text = "<img src='images/final.jpg' alt='" & name_string & "' />"
              td.VerticalAlign = VerticalAlign.Top
              td2 = New TableCell
              linky = New Label

              Dim link_text As String = aTempTable.Rows(0).Item("cliaircraft_year_mfr") & " " & name_string & ", Ser #" & aTempTable.Rows(0).Item("cliaircraft_ser_nbr")
              If Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_reg_nbr")) Then
                If aTempTable.Rows(0).Item("cliaircraft_reg_nbr") <> "" Then
                  link_text = link_text & ", Reg#" & aTempTable.Rows(0).Item("cliaircraft_reg_nbr")
                End If
              End If

              linky.Text = "<a href='details.aspx?ac_ID=" & aTempTable.Rows(0).Item("cliaircraft_id") & "&type=3&source=CLIENT'>" & link_text & "</a>"

              td2.Controls.Add(linky)
              tr.Controls.Add(td)
              tr.Controls.Add(td2)
              recent_aircraft.Controls.Add(tr)
              count = count - 1
            End If

          End If
        Else
          If masterPage.aclsData_Temp.class_error <> "" Then
            error_string = masterPage.aclsData_Temp.class_error
            masterPage.LogError("home.aspx.vb - recently_edited_viewed_ac() - " & error_string)
          End If
          masterPage.display_error()
        End If
      End If

      If holding_count > 0 Then
        home_aircraft_txt.Controls.Add(recent_aircraft)
      End If
    Catch ex As Exception
      error_string = "home.aspx.vb - recently_edited_viewed_ac() - " & ex.Message
      masterPage.LogError(error_string)
    End Try

  End Sub

  Sub recently_edited_viewed_companies()
    Try
      '-----------------------------------------------------------------------------
      Dim tr As New TableRow
      Dim td As New TableCell
      Dim td2 As New TableCell
      Dim linky As New Label

      Dim count As Integer = 0
      Dim holding_count As Integer = 0

      'Are we using recently edited or recently viewed?

      Dim _companiesCookies As HttpCookie = Request.Cookies("companies")

      Dim stored_id As String = ""
      Dim stored_source As String = ""
      Dim use_cookie As Boolean = False
      If _companiesCookies IsNot Nothing Then
        Dim user As String = _companiesCookies("USER")
        If user = CStr(Session.Item("localUser").crmLocalUserID) Then 'Cookie is for this user!
          If _companiesCookies("ID") <> "" Then
            use_cookie = True
          End If
        End If
      End If

      If use_cookie = True Then 'use recently edited.

        stored_id = _companiesCookies("ID")
        stored_source = _companiesCookies("SOURCE")
        Dim source_array As Array = Split(stored_source, "|")

        Dim id_array As Array = Split(stored_id, "|")
        Dim topnumber As Integer = UBound(id_array)

        Dim recent_company As New Table
        recent_company.Width = Unit.Percentage(100)
        recent_company.CssClass = "data_aircraft_grid" '"most_recent"
        recent_company.CellPadding = 3

        tr.CssClass = "header_row"
        td.ColumnSpan = 2
        td.Text = "<b>Recently Viewed Companies</b>"
        tr.Controls.Add(td)
        recent_company.Controls.Add(tr)

        For i As Integer = 0 To topnumber

          If id_array(i) <> "" And source_array(i) <> "" Then
            aTempTable = masterPage.aclsData_Temp.GetLimited_CompanyInfo_ID(id_array(i), source_array(i), 0)

            ' check the state of the DataTable
            If Not IsNothing(aTempTable) Then
              count = aTempTable.Rows.Count
              holding_count = aTempTable.Rows.Count
            End If

            If count > 5 Then
              count = 5
            End If

            If holding_count = 0 Then
              holding_count = 1
            End If

            If Not IsNothing(aTempTable) Then
              If aTempTable.Rows.Count > 0 Then

                If count <> 0 Then
                  tr = New TableRow
                  td = New TableCell

                  td.Width = 18
                  td.Text = "<img src='images/final.jpg' alt='company' />"
                  td.VerticalAlign = VerticalAlign.Top
                  td2 = New TableCell
                  linky = New Label
                  linky.Text = "<a href='details.aspx?comp_ID=" & aTempTable.Rows(0).Item("comp_id") & "&type=1&source=" & source_array(i) & "'>" & aTempTable.Rows(0).Item("comp_name") & ", " & aTempTable.Rows(0).Item("comp_city") & " " & aTempTable.Rows(0).Item("comp_state") & "</a>"

                  td2.Controls.Add(linky)
                  tr.Controls.Add(td)
                  tr.Controls.Add(td2)
                  recent_company.Controls.Add(tr)
                  count = count - 1
                End If

              End If
            Else
              If masterPage.aclsData_Temp.class_error <> "" Then
                error_string = masterPage.aclsData_Temp.class_error
                masterPage.LogError("home.aspx.vb - recently_edited_viewed_companies() - " & error_string)
              End If
              masterPage.display_error()
            End If
          End If
        Next
        If holding_count > 0 Then
          home_companies_txt.Controls.Add(recent_company)
        End If

      Else
        'Setup Recently Edited Companies

        aTempTable = masterPage.aclsData_Temp.Get_Latest_Client_CompanyByUserID(Session.Item("localUser").crmLocalUserID)

        Dim recent_company As New Table
        recent_company.Width = Unit.Percentage(100)
        recent_company.CssClass = "data_aircraft_grid" '"most_recent"
        recent_company.CellPadding = 3

        td.ColumnSpan = 2
        tr.CssClass = "header_row"
        td.Text = "<b>Recently Edited Companies</b>"
        tr.Controls.Add(td)
        recent_company.Controls.Add(tr)

        ' check the state of the DataTable
        If Not IsNothing(aTempTable) Then
          count = aTempTable.Rows.Count
          holding_count = aTempTable.Rows.Count
        End If

        If count > 5 Then
          count = 5
        End If

        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then

            If count <> 0 Then
              tr = New TableRow
              td = New TableCell
              td.Width = 18
              td.Text = "<img src='images/final.jpg' alt='" & aTempTable.Rows(0).Item("comp_name") & "' />"
              td.VerticalAlign = VerticalAlign.Top
              td2 = New TableCell
              linky = New Label
              linky.Text = "<a href='details.aspx?comp_ID=" & aTempTable.Rows(0).Item("comp_id") & "&source=CLIENT&type=1'>" & aTempTable.Rows(0).Item("comp_name") & ", " & aTempTable.Rows(0).Item("comp_city") & " " & aTempTable.Rows(0).Item("comp_state") & "</a>"

              td2.Controls.Add(linky)
              tr.Controls.Add(td)
              tr.Controls.Add(td2)
              recent_company.Controls.Add(tr)
              count = count - 1
            End If

          End If
        Else
          If masterPage.aclsData_Temp.class_error <> "" Then
            error_string = masterPage.aclsData_Temp.class_error
            masterPage.LogError("home.aspx.vb - recently_edited_viewed_companies() - " & error_string)
          End If
          masterPage.display_error()
        End If

        If holding_count > 0 Then
          home_companies_txt.Controls.Add(recent_company)
        End If

      End If
    Catch ex As Exception
      error_string = "home.aspx.vb - recently_edited_viewed_companies() - " & ex.Message
      masterPage.LogError(error_string)
    End Try
  End Sub

  Sub recently_edited_viewed_contacts()
    Try
      '-------------------------------------------------------------------------------------------------------------------------------------------
      'Setup Recently Edited Contacts
      '-----------------------------------------------------------------------------
      Dim TR As New TableRow
      Dim TD As New TableCell
      Dim TD_2 As New TableCell
      Dim DisplayLabel As New Label

      Dim count As Integer = 0
      Dim holding_count As Integer = 0

      Dim _contactsCookies As HttpCookie = Request.Cookies("contacts")
      Dim recent_contact As New Table
      recent_contact.Width = Unit.Percentage(100)
      Dim use_cookie As Boolean = False
      If _contactsCookies IsNot Nothing Then
        Dim user As String = _contactsCookies("USER")
        If user = CStr(Session.Item("localUser").crmLocalUserID) Then 'Cookie is for this user!
          If _contactsCookies("ID") <> "" Then
            use_cookie = True
          End If
        End If
      End If

      If use_cookie = True Then 'use recently edited.

        Dim stored_id As String = _contactsCookies("ID")
        Dim stored_source As String = _contactsCookies("SOURCE")
        Dim source_array As Array = Split(stored_source, "|")

        Dim id_array As Array = Split(stored_id, "|")
        Dim topnumber As Integer = UBound(id_array)
        Dim JetnetIDString As String = ""
        Dim ClientIDString As String = ""
        Dim HoldTable As New DataTable

        TD = New TableCell
        TD_2 = New TableCell
        TR = New TableRow
        recent_contact.CssClass = "data_aircraft_grid" 'most_recent"
        recent_contact.CellPadding = 3
        TD.ColumnSpan = 2
        TR.CssClass = "header_row"
        TD.Text = "<b>Recently Viewed Contacts</b>"
        TR.Controls.Add(TD)
        recent_contact.Controls.Add(TR)


        For i As Integer = 0 To topnumber
          holding_count = topnumber
          If holding_count = 0 Then
            holding_count = 1
          End If
          If id_array(i) <> "" And source_array(i) <> "" Then
            Dim id_set As Array = Split(id_array(i), ",")
            Dim source_set As Array = Split(source_array(i), ",")
            Select Case UCase(source_set(0))
              Case "JETNET"
                If JetnetIDString <> "" Then
                  JetnetIDString += ", "
                End If
                JetnetIDString += id_set(0).ToString
              Case "CLIENT"
                If ClientIDString <> "" Then
                  ClientIDString += ", "
                End If
                ClientIDString += id_set(0).ToString

            End Select
          End If
        Next

        If JetnetIDString <> "" Then
          HoldTable = masterPage.aclsData_Temp.GetContacts_Details_InClause(JetnetIDString, "JETNET")
          Build_Recent_Contacts(HoldTable, "JETNET", recent_contact, True, New TreeNode, False)
        End If
        If ClientIDString <> "" Then
          HoldTable = masterPage.aclsData_Temp.GetContacts_Details_InClause(ClientIDString, "CLIENT")
          Build_Recent_Contacts(HoldTable, "CLIENT", recent_contact, True, New TreeNode, False)
        End If

      Else
        TD = New TableCell
        TD_2 = New TableCell
        TR = New TableRow

        aTempTable = masterPage.aclsData_Temp.GetLatestContactByUserID(Session.Item("localUser").crmLocalUserID)
        recent_contact.CssClass = "data_aircraft_grid" ' "most_recent"
        recent_contact.CellPadding = 3
        TD.ColumnSpan = 2
        TR.CssClass = "header_row"
        TD.Text = "<b>Recently Edited Contacts</b>"
        TR.Controls.Add(TD)
        recent_contact.Controls.Add(TR)

        ' check the state of the DataTable
        If Not IsNothing(aTempTable) Then
          count = aTempTable.Rows.Count
          holding_count = aTempTable.Rows.Count
        End If
        If count > 5 Then
          count = 5
        End If

        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then

            If count <> 0 Then
              TR = New TableRow
              TD = New TableCell
              TD.Width = 18

              Dim con_comp_name As String = ""
              aTempTable2 = masterPage.aclsData_Temp.GetLimited_CompanyInfo_ID(aTempTable.Rows(0).Item("contact_comp_id"), "CLIENT", 0)
              If Not IsNothing(aTempTable2) Then
                If aTempTable2.Rows.Count > 0 Then
                  For Each z As DataRow In aTempTable2.Rows
                    con_comp_name = z("comp_name")
                  Next
                End If
              End If
              TD.VerticalAlign = VerticalAlign.Top
              TD_2 = New TableCell
              DisplayLabel = New Label

              TD.Text = "<img src='images/final.jpg' alt='" & aTempTable.Rows(0).Item("contact_first_name") & " " & aTempTable.Rows(0).Item("contact_last_name") & "' />"
              DisplayLabel.Text = "<a href='details.aspx?comp_ID=" & aTempTable.Rows(0).Item("contact_comp_id") & "&type=1&source=CLIENT&contact_ID=" & aTempTable.Rows(0).Item("contact_id") & "&'>" & aTempTable.Rows(0).Item("contact_first_name") & " " & aTempTable.Rows(0).Item("contact_last_name") & ", " & con_comp_name

              TD_2.Controls.Add(DisplayLabel)
              TR.Controls.Add(TD)
              TR.Controls.Add(TD_2)
              recent_contact.Controls.Add(TR)
              count = count - 1
            End If

          End If
        Else
          If masterPage.aclsData_Temp.class_error <> "" Then
            error_string = masterPage.aclsData_Temp.class_error
            masterPage.LogError("home.aspx.vb - recently_edited_viewed_contacts() - " & error_string)
          End If
          masterPage.display_error()
        End If
      End If
      If holding_count > 0 Then
        home_contacts_txt.Controls.Add(recent_contact)
      End If
    Catch ex As Exception
      error_string = "home.aspx.vb - recently_edited_viewed_contacts() - " & ex.Message
      masterPage.LogError(error_string)
    End Try
  End Sub

  Sub recently_added_notes()
    Try
      '-------------------------------------------------------------------------------------------------------------------------------------------
      'Setup Recently Edited Contacts
      '-----------------------------------------------------------------------------
      Dim tr As New TableRow
      Dim td As New TableCell
      Dim td2 As New TableCell

      Dim count As Integer = 0
      Dim holding_count As Integer = 0
      Dim recent_notes As New Table
      Dim _notesCookies As HttpCookie = Request.Cookies("notes")
      Dim use_cookie As Boolean = False
      If _notesCookies IsNot Nothing Then
        Dim user As String = _notesCookies("USER")
        If user = CStr(Session.Item("localUser").crmLocalUserID) Then 'Cookie is for this user!
          If _notesCookies("ID") <> "" Then
            use_cookie = True
          End If
        End If
      End If

      If use_cookie = True Then 'use recently edited.
        td = New TableCell
        td2 = New TableCell
        tr = New TableRow
        recent_notes.CssClass = "data_aircraft_grid" '"most_recent"
        recent_notes.Width = Unit.Percentage(100)
        recent_notes.CellPadding = 3
        td.ColumnSpan = 2
        tr.CssClass = "header_row"
        td.Text = "<b>Recently Viewed Notes</b>"
        tr.Controls.Add(td)
        recent_notes.Controls.Add(tr)

        Dim stored_id As String = _notesCookies("ID")
        Dim id_array As Array = Split(stored_id, "|")
        Dim topnumber As Integer = UBound(id_array)


        For i As Integer = 0 To topnumber
          If id_array(i) <> "" Then
            aTempTable = masterPage.aclsData_Temp.Get_Local_Notes_Client_NoteID(id_array(i))
            ' check the state of the DataTable
            If Not IsNothing(aTempTable) Then
              count = aTempTable.Rows.Count
              holding_count = holding_count + aTempTable.Rows.Count
            Else
              If masterPage.aclsData_Temp.class_error <> "" Then
                error_string = masterPage.aclsData_Temp.class_error
                masterPage.LogError("home.aspx.vb - Error Get_Local_Notes_Client_NoteID(" & id_array(i) & ") - " & error_string)
              End If
              masterPage.display_error()
            End If
            If count > 5 Then
              count = 5
            End If

            If Not IsNothing(aTempTable) Then
              If aTempTable.Rows.Count > 0 Then
                For Each r As DataRow In aTempTable.Rows
                  If count <> 0 Then
                    tr = New TableRow
                    td = New TableCell
                    td.Width = 18

                    td.Text = "<img src='images/final.jpg' alt='" & Server.HtmlDecode(r("lnote_note")) & "' />"
                    td.VerticalAlign = VerticalAlign.Top
                    td2 = New TableCell
                    Dim note_text As New Label
                    td2.VerticalAlign = VerticalAlign.Top
                    note_text.Text = "<a href='#' style='text-decoration:none;font-weight:100;' onclick=""javascript:window.open('edit_note.aspx?action=edit&amp;type=note&amp;id=" & r("lnote_id") & "','','scrollbars=yes,menubar=no,height=400,width=860,resizable=yes,toolbar=no,location=no,status=no');"">" & Server.HtmlDecode(Left(r("lnote_note"), 75)) & "...</a>"

                    Dim ds As New DataTable
                    Dim cal_string As String = ""
                    Dim callink As New Label
                    Dim magimage As New ImageButton
                    ' Dim fly As New OboutInc.Flyout2.Flyout
                    Dim flyout_show As Boolean = True
                    If r("lnote_client_comp_id") <> 0 Or r("lnote_jetnet_comp_id") <> 0 Then
                      If r("lnote_jetnet_comp_id") <> 0 Then
                        ds = masterPage.aclsData_Temp.GetLimited_CompanyInfo_ID(r("lnote_jetnet_comp_id"), "JETNET", 0)
                      Else
                        ds = masterPage.aclsData_Temp.GetLimited_CompanyInfo_ID(r("lnote_client_comp_id"), "CLIENT", 0)
                      End If

                      If Not IsNothing(ds) Then
                        If ds.Rows.Count > 0 Then
                          cal_string = "<a href='details.aspx?comp_ID=" & ds.Rows(0).Item("comp_id") & "&type=1&source=" & ds.Rows(0).Item("source") & "'>(<em style='color:#5b5e65;'>" & ds.Rows(0).Item("comp_name") & ", " & ds.Rows(0).Item("comp_city") & " " & ds.Rows(0).Item("comp_state") & "</em>)"
                          callink.Text = cal_string


                          If Not Page.IsPostBack Then
                            Dim temporaryText As String = ""

                            If r("lnote_client_comp_id") > 0 Then
                              temporaryText = clsGeneral.clsGeneral.stripHTML(Replace(Replace(masterPage.createAnAddressPopOut(r("lnote_client_comp_id"), "CLIENT"), "<br />", vbNewLine), "<BR />", vbNewLine))
                              magimage.ID = "Mag" & r("lnote_client_comp_id") & r("lnote_id")
                            Else
                              temporaryText = clsGeneral.clsGeneral.stripHTML(Replace(Replace(masterPage.createAnAddressPopOut(r("lnote_jetnet_comp_id"), "JETNET"), "<br />", vbNewLine), "<BR />", vbNewLine))
                              magimage.ID = "Mag" & r("lnote_jetnet_comp_id") & r("lnote_id")
                            End If


                            magimage.ImageUrl = "~/images/magnify.png"
                            magimage.AlternateText = temporaryText
                            magimage.ToolTip = temporaryText

                            'fly.AttachTo = "Mag" & r("lnote_client_comp_id") & r("lnote_id")
                            'fly.Position = OboutInc.Flyout2.PositionStyle.TOP_RIGHT
                            'fly.Align = OboutInc.Flyout2.AlignStyle.TOP
                            'fly.FlyingEffect = OboutInc.Flyout2.FlyingEffectStyle.TOP_RIGHT
                            'fly.FadingEffect = "true"
                            'Dim flytext As New Label

                            'flytext.Text = clsGeneral.clsGeneral.MouseOverTextStart & masterPage.createAnAddressPopOut(r("lnote_client_comp_id"), "CLIENT") & clsGeneral.clsGeneral.MouseOverTextEnd
                            'fly.Controls.Add(flytext)

                          End If
                        Else
                          flyout_show = False
                        End If
                      Else
                        flyout_show = False
                      End If
                    Else
                      flyout_show = False
                    End If

                    td2.Controls.Add(note_text)
                    td2.Controls.Add(callink)
                    If flyout_show = True Then
                      td2.Controls.Add(magimage)
                      ' td2.Controls.Add(fly)
                    End If
                    callink = New Label
                    If r("lnote_client_ac_id") <> 0 Or r("lnote_jetnet_ac_id") <> 0 Then
                      If r("lnote_jetnet_ac_id") <> 0 Then

                        cal_string = masterPage.what_ac(r("lnote_jetnet_ac_id"), r("lnote_client_ac_id"), 2)
                        cal_string = "<a href='details.aspx?ac_ID=" & r("lnote_jetnet_ac_id") & "&type=3&source=JETNET'>(<em style='color:#5b5e65;'>" & Replace(cal_string, "<br />", " - ") & "</em>)</a>"
                        callink.Text = cal_string
                      Else
                        cal_string = masterPage.what_ac(r("lnote_jetnet_ac_id"), r("lnote_client_ac_id"), 1)
                        cal_string = cal_string & masterPage.what_ac(r("lnote_jetnet_ac_id"), r("lnote_client_ac_id"), 2)
                        cal_string = "<a href='details.aspx?ac_ID=" & r("lnote_client_ac_id") & "&type=3&source=CLIENT'>(<em style='color:#5b5e65;'>" & Replace(cal_string, "<br />", " - ") & "</em>)</a>"
                        callink.Text = cal_string

                      End If
                    End If



                    td2.Controls.Add(callink)
                    tr.Controls.Add(td)
                    tr.Controls.Add(td2)
                    recent_notes.Controls.Add(tr)
                    count = count - 1
                  End If
                Next
              End If
            Else
              If masterPage.aclsData_Temp.class_error <> "" Then
                error_string = masterPage.aclsData_Temp.class_error
                masterPage.LogError("home.aspx.vb - Error Get_Local_Notes_Client_NoteID(" & id_array(i) & ") - " & error_string)
              End If
              masterPage.display_error()
            End If
          End If
        Next
        If holding_count > 0 Then
          home_notes_txt.Controls.Add(recent_notes)
        End If
      Else
        td = New TableCell
        td2 = New TableCell
        tr = New TableRow
        recent_notes.CssClass = "data_aircraft_grid" '"most_recent"
        recent_notes.CellPadding = 3
        recent_notes.Width = Unit.Percentage(100)
        td.ColumnSpan = 2
        tr.CssClass = "header_row"
        td.Text = "<b>Recently Edited Notes</b>"
        tr.Controls.Add(td)
        recent_notes.Controls.Add(tr)

        Dim usedate As String = DateAdd(DateInterval.Day, -10, Now())

        usedate = Year(usedate) & "-" & Month(usedate) & "-" & Day(usedate)

        aTempTable = masterPage.aclsData_Temp.Get_Local_Notes_Client_LastestByUser(Session.Item("localUser").crmLocalUserID, usedate, "A")

        ' check the state of the DataTable
        If Not IsNothing(aTempTable) Then
          count = aTempTable.Rows.Count
          holding_count = aTempTable.Rows.Count
        End If
        If count > 5 Then
          count = 5
        End If

        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            For Each r As DataRow In aTempTable.Rows
              If count <> 0 Then
                tr = New TableRow
                td = New TableCell
                td.Width = 18

                td.Text = "<img src='images/final.jpg' alt='" & Server.HtmlDecode(r("lnote_note")) & "' />"
                td.VerticalAlign = VerticalAlign.Top
                td2 = New TableCell
                Dim note_text As New Label
                td2.VerticalAlign = VerticalAlign.Top
                note_text.Text = "<a href='#' style='text-decoration:none;font-weight:100;' onclick=""javascript:window.open('edit_note.aspx?action=edit&amp;type=note&amp;id=" & r("lnote_id") & "','','scrollbars=yes,menubar=no,height=400,width=860,resizable=yes,toolbar=no,location=no,status=no');"">" & Server.HtmlDecode(Left(r("lnote_note"), 75)) & "...</a>"

                Dim ds As New DataTable
                If r("lnote_jetnet_comp_id") <> 0 Then
                  ds = masterPage.aclsData_Temp.GetLimited_CompanyInfo_ID(r("lnote_jetnet_comp_id"), "JETNET", 0)
                Else
                  ds = masterPage.aclsData_Temp.GetLimited_CompanyInfo_ID(r("lnote_client_comp_id"), "CLIENT", 0)
                End If


                Dim cal_string As String = ""
                Dim callink As New Label

                If Not IsNothing(ds) Then
                  If ds.Rows.Count > 0 Then
                    cal_string = "<a href='details.aspx?comp_ID=" & ds.Rows(0).Item("comp_id") & "&source=" & ds.Rows(0).Item("source") & "&type=1'>(<em style='color:#5b5e65;'>" & ds.Rows(0).Item("comp_name") & ", " & ds.Rows(0).Item("comp_city") & " " & ds.Rows(0).Item("comp_state") & "</em>)</a>"
                    callink.Text = cal_string
                  End If
                End If
                td2.Controls.Add(note_text)
                td2.Controls.Add(callink)

                callink = New Label
                If r("lnote_client_ac_id") <> 0 Or r("lnote_jetnet_ac_id") <> 0 Then
                  If r("lnote_jetnet_ac_id") <> 0 Then


                    cal_string = masterPage.what_ac(r("lnote_jetnet_ac_id"), r("lnote_client_ac_id"), 2)

                    cal_string = "<a href='details.aspx?ac_ID=" & r("lnote_jetnet_ac_id") & "&source=JETNET&type=3'>(<em style='color:#5b5e65;'>" & Replace(cal_string, "<br />", " - ") & "</em>)</a>"
                    callink.Text = cal_string


                  Else

                    cal_string = "<a href='details.aspx?ac_ID=" & r("lnote_jetnet_ac_id") & "&source=JETNET&type=3'>" & masterPage.what_ac(r("lnote_jetnet_ac_id"), r("lnote_client_ac_id"), 2) & "</a>"
                    cal_string = "(<em style='color:#5b5e65;'>" & Replace(cal_string, "<br />", " - ") & "</em>)"
                    callink.Text = cal_string

                  End If
                End If

                td2.Controls.Add(callink)

                tr.Controls.Add(td)
                tr.Controls.Add(td2)
                recent_notes.Controls.Add(tr)
                count = count - 1
              End If
            Next
          End If
        Else
          If masterPage.aclsData_Temp.class_error <> "" Then
            error_string = masterPage.aclsData_Temp.class_error
            masterPage.LogError("home.aspx.vb - aclsData_Temp.Get_Local_Notes_Client_LastestByUser(" & Session.Item("localUser").crmLocalUserID & ", " & usedate & ", ""A"") - " & error_string)
          End If
          masterPage.display_error()
        End If


        If holding_count > 0 Then
          home_notes_txt.Controls.Add(recent_notes)
        End If
      End If
    Catch ex As Exception
      error_string = "home.aspx.vb - Recently_Added_Notes() Exception - " & ex.Message
      masterPage.LogError(error_string)
    End Try

  End Sub

  Private Sub GetFolderListing(ByRef AircraftProjects As TreeView, ByRef CompanyProjects As TreeView,
                               ByRef ContactProjects As TreeView, ByRef HistoryProjects As TreeView,
                               ByRef EventProjects As TreeView, ByRef WantedProjects As TreeView,
                               ByRef YachtProjects As TreeView, ByRef PerformanceSpecsProjects As TreeView,
                               ByVal OperatingCostsProjects As TreeView, ByVal MarketingSummariesProjects As TreeView,
                               ByVal YachtHistoryProjects As TreeView, ByVal YachtEventProjects As TreeView,
                               ByRef AirportProjects As TreeView, ByRef ShowHiddenFolders As CheckBox, ByRef ValuesProjects As TreeView, ByRef hideSharedFolders As CheckBox)

    Dim FoldersType As New DataTable
    Dim FoldersTable As New DataTable 'holds folder datatable.
    'how about I do this first:
    'I will initialize each tree view with a starting main node.
    'Aircraft Main Node:
    Dim AircraftMainNode As New TreeNode
    AircraftMainNode.Text = "<b class='upperHeader tiny_text'>Aircraft Folders</b>"
    'Company Main Node:
    Dim CompanyMainNode As New TreeNode
    CompanyMainNode.Text = "<b class='upperHeader tiny_text'>Company Folders</b>"
    'CompanyMainNode.Expanded = False
    'Contact Main Node:
    Dim ContactMainNode As New TreeNode
    ContactMainNode.Text = "<b class='upperHeader tiny_text'>Contact Folders</b>"
    'History Main Node:
    Dim HistoryMainNode As New TreeNode
    HistoryMainNode.Text = "<b class='upperHeader tiny_text'>History Folders</b>"
    'HistoryMainNode.Expanded = False

    'Event Main Node:
    Dim EventMainNode As New TreeNode
    EventMainNode.Text = "<b class='upperHeader tiny_text'>Event Folders</b>"

    'Wanteds Main Node:
    Dim WantedMainNode As New TreeNode
    WantedMainNode.Text = "<b class='upperHeader tiny_text'>Wanteds Folders</b>"

    'Performance Specs Main Node:
    Dim PerformanceMainNode As New TreeNode
    PerformanceMainNode.Text = "<b class='upperHeader tiny_text'>Performance Specs Folders</b>"

    'Operating Costs Main Node:
    Dim OperatingMainNode As New TreeNode
    OperatingMainNode.Text = "<b class='upperHeader tiny_text'>Operating Costs Folders</b>"

    'Marketing Summary Main Node:
    Dim MarketingMainNode As New TreeNode
    MarketingMainNode.Text = "<b class='upperHeader tiny_text'>Market Summary Folders</b>"

    'Yacht Main Node:
    Dim YachtMainNode As New TreeNode
    YachtMainNode.Text = "<b class='upperHeader tiny_text'>Yacht Folders</b>"

    'Yacht History Main Node:
    Dim YachtHistoryMainNode As New TreeNode
    YachtHistoryMainNode.Text = "<b class='upperHeader tiny_text'>History Folders</b>"

    'Yacht Event Main Node:
    Dim YachtEventMainNode As New TreeNode
    YachtEventMainNode.Text = "<b class='upperHeader tiny_text'>Event Folders</b>"

    'Airport Main Node:
    Dim AirportMainNode As New TreeNode
    AirportMainNode.Text = "<b class='upperHeader tiny_text'>Airport Folders</b>"

    'Values main node
    Dim ValuesMainNode As New TreeNode
    ValuesMainNode.Text = "<b class='upperHeader tiny_text'>Values Folders</b>"

    If Not IsNothing(hideSharedFolders) Then

      If hideSharedFolders.Checked = True Then
        'dont get the shared data 
        FoldersTable = masterPage.aclsData_Temp.GetEvolutionFolderssBySubscription(0, Session.Item("localUser").crmUserLogin, Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmSubSeqNo, "N", 0, Nothing, "")
      Else
        FoldersTable = masterPage.aclsData_Temp.GetEvolutionFolderssBySubscription(0, Session.Item("localUser").crmUserLogin, Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmSubSeqNo, "", 0, Nothing, "")
      End If

    End If

    If Not IsNothing(FoldersTable) Then
      If FoldersTable.Rows.Count > 0 Then
        For Each r As DataRow In FoldersTable.Rows
          If Not IsDBNull(r("cfolder_data")) Or r("cfttpe_name").ToString.ToLower.Contains("airport") Then
            'Declaring what's common with each subnode
            Dim SubNode As New TreeNode
            Dim ShowFolder As Boolean = False 'Toggles the visibility of hidden folders whether on or off. 

            If ShowHiddenFolders.Checked = True Then 'if the checkbox is checked, show either hidden and unhidden
              ShowFolder = True
            Else
              If r("cfolder_hide_flag").ToString = "Y" Then 'if the hidden flag is set
                ShowFolder = False 'and checkbox is not checked, go ahead and hide the
              Else
                ShowFolder = True 'otherwise go ahead and display them.
              End If
            End If

            'If hideSharedFolders.Checked = False Then 'if the checkbox is not checked, show all
            '  ' leave hoever it was 
            'Else
            '  If r("cfolder_share").ToString = "Y" Then 'if it is checked, and it is shared, dont show 

            '    ShowFolder = False 'and checkbox is not checked, go ahead and hide the 

            '  Else
            '    ' leave hoever it was 
            '  End If
            'End If 


            If ShowFolder = True Then 'main toggle for visibility.

              Dim FolderDataString As Array = Nothing
              Dim FolderRawDataString As String = ""

              If Not IsDBNull(r("cfolder_data")) Then

                'this was added to parse out the real search query now that we're saving it
                FolderDataString = Split(r("cfolder_data"), "THEREALSEARCHQUERY")
                FolderRawDataString = r("cfolder_data").ToString.Trim
              End If
              SubNode.Text = r("cfolder_name").ToString

              SubNode.Value = Replace(r("cfolder_data").ToString, "'", "\'")

              SubNode.ImageUrl = DisplayFunctions.ReturnFolderImage(r("cfolder_method").ToString, r("cfolder_hide_flag").ToString, r("cfolder_share").ToString)

              'set tooltip as description.
              SubNode.ImageToolTip = r("cfolder_description").ToString

              Select Case r("cfttpe_name").ToString.ToLower
                Case "values"
                  If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = True Then
                    If Not String.IsNullOrEmpty(FolderRawDataString.Trim) Then
                      SubNode.NavigateUrl = "javascript:ParseViewFolders('" & r("cfolder_id").ToString & "',27,'" + IIf(Not IsNothing(FolderDataString), Replace(FolderDataString(0), "',", "\'"), "") + "','true');ChangeTheMouseCursorOnItemParentDocument('cursor_default');"
                      ValuesMainNode.ChildNodes.Add(SubNode)
                    End If
                  End If
                Case "yacht history"
                  SubNode.NavigateUrl = "javascript:ParseYachtSpecialFolders('" & r("cfolder_id").ToString & "',true,false,'" + IIf(Not IsNothing(FolderDataString), Replace(FolderDataString(0), "'", "\'"), "") + "');"
                  YachtHistoryMainNode.ChildNodes.Add(SubNode)
                Case "yacht events"
                  SubNode.NavigateUrl = "javascript:ParseYachtSpecialFolders('" & r("cfolder_id").ToString & "',false,true,'" + IIf(Not IsNothing(FolderDataString), Replace(FolderDataString(0), "'", "\'"), "") + "');"
                  YachtEventMainNode.ChildNodes.Add(SubNode)
                Case "market summaries"
                  SubNode.NavigateUrl = "javascript:ParseSpecsOperatingMarketForm('" & r("cfolder_id").ToString & "',false,false,true,'" + IIf(Not IsNothing(FolderDataString), Replace(FolderDataString(0), "'", "\'"), "") + "');"
                  MarketingMainNode.ChildNodes.Add(SubNode)
                Case "operating costs"
                  SubNode.NavigateUrl = "javascript:ParseSpecsOperatingMarketForm('" & r("cfolder_id").ToString & "',false,true,false,'" + IIf(Not IsNothing(FolderDataString), Replace(FolderDataString(0), "'", "\'"), "") + "');"
                  OperatingMainNode.ChildNodes.Add(SubNode)
                Case "performance specs"
                  SubNode.NavigateUrl = "javascript:ParseSpecsOperatingMarketForm('" & r("cfolder_id").ToString & "',true,false,false,'" + IIf(Not IsNothing(FolderDataString), Replace(FolderDataString(0), "'", "\'"), "") + "');"
                  PerformanceMainNode.ChildNodes.Add(SubNode)
                Case "aircraft"
                  If Not String.IsNullOrEmpty(FolderRawDataString.Trim) Then
                    SubNode.NavigateUrl = "javascript:ParseForm('" & r("cfolder_id").ToString & "', false,false,false, false, false, '" + IIf(Not IsNothing(FolderDataString), Replace(FolderDataString(0), "'", "\'"), "") + "');"
                  Else
                    SubNode.NavigateUrl = "javascript:alert('This folder contains no information.');"
                  End If

                  AircraftMainNode.ChildNodes.Add(SubNode)
                Case "yacht"
                  If Not String.IsNullOrEmpty(FolderRawDataString.Trim) Then
                    SubNode.NavigateUrl = "javascript:ParseForm('" & r("cfolder_id").ToString & "', false,false,false,false,true, '" + IIf(Not IsNothing(FolderDataString), Replace(FolderDataString(0), "'", "\'"), "") + "');"
                  Else
                    SubNode.NavigateUrl = "javascript:alert('This folder contains no information.');"
                  End If

                  YachtMainNode.ChildNodes.Add(SubNode)
                Case "company"
                  If Not String.IsNullOrEmpty(FolderRawDataString.Trim) Then
                    SubNode.NavigateUrl = "javascript:ParseForm('" & r("cfolder_id").ToString & "', false,false,true,false,false, '" + IIf(Not IsNothing(FolderDataString), Replace(FolderDataString(0), "'", "\'"), "") + "');"
                  Else
                    SubNode.NavigateUrl = "javascript:alert('This folder contains no information.');"
                  End If

                  CompanyMainNode.ChildNodes.Add(SubNode)
                Case "contact"
                  If Not String.IsNullOrEmpty(FolderRawDataString.Trim) Then
                    SubNode.NavigateUrl = "javascript:ParseForm('" & r("cfolder_id").ToString & "', false,false,true,false,false, '" + IIf(Not IsNothing(FolderDataString), Replace(FolderDataString(0), "'", "\'"), "") + "');"
                  Else
                    SubNode.NavigateUrl = "javascript:alert('This folder contains no information.');"
                  End If

                  ContactMainNode.ChildNodes.Add(SubNode)
                Case "history"
                  If Not String.IsNullOrEmpty(FolderRawDataString.Trim) Then
                    SubNode.NavigateUrl = "javascript:ParseForm('" & r("cfolder_id").ToString & "',true" & ",false,false, false,false,'" + IIf(Not IsNothing(FolderDataString), Replace(FolderDataString(0), "'", "\'"), "") + "');"
                  Else
                    SubNode.NavigateUrl = "javascript:alert('This folder contains no information.');"
                  End If

                  HistoryMainNode.ChildNodes.Add(SubNode)
                Case "events"

                  If Not String.IsNullOrEmpty(FolderRawDataString.Trim) Then
                    SubNode.NavigateUrl = "javascript:ParseForm('" & r("cfolder_id").ToString & "',false, true,false, false,false, " & " '" + IIf(Not IsNothing(FolderDataString), Replace(FolderDataString(0), "'", "\'"), "") + "');"
                  Else
                    SubNode.NavigateUrl = "javascript:alert('This folder contains no information.');"
                  End If
                  EventMainNode.ChildNodes.Add(SubNode)
                Case "wanteds"
                  If Not String.IsNullOrEmpty(FolderRawDataString.Trim) Then
                    SubNode.NavigateUrl = "javascript:ParseForm('" & r("cfolder_id").ToString & "', false,false,false,true,false, '" + IIf(Not IsNothing(FolderDataString), Replace(FolderDataString(0), "'", "\'"), "") + "');"
                  Else
                    SubNode.NavigateUrl = "javascript:alert('This folder contains no information.');"
                  End If

                  WantedMainNode.ChildNodes.Add(SubNode)
                Case "airport"
                  'If Not String.IsNullOrEmpty(FolderRawDataString.Trim) Then
                  SubNode.NavigateUrl = "javascript:load('staticFolderEditor.aspx?folderID=" + r("cfolder_id").ToString + "&airport=true&fromHome=true','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"
                  'Else
                  'SubNode.NavigateUrl = "javascript:alert('This folder contains no information.');"
                  'End If

                  AirportMainNode.ChildNodes.Add(SubNode)
              End Select
            End If
          End If
        Next
      Else


        no_projects.Visible = True
        no_projects.CssClass = "emphasis_text"
      End If


      'Aircraft Folders
      If Not IsNothing(AircraftProjects) Then
        AircraftProjects.Nodes.Clear()
        'CrmClientDisplayFolders(AircraftMainNode, 3)
        AircraftProjects.Nodes.Add(AircraftMainNode)
        AircraftProjects.ExpandAll()
      End If

      'Company Folders
      If Not IsNothing(CompanyProjects) Then
        CompanyProjects.Nodes.Clear()

        CrmClientDisplayFolders(CompanyMainNode, 1)
        CompanyProjects.Nodes.Add(CompanyMainNode)

        CompanyProjects.ExpandAll()
      End If

      'Contact Folders
      If Not IsNothing(ContactProjects) Then
        ContactProjects.Nodes.Clear()
        ContactProjects.Nodes.Add(ContactMainNode)
        ContactProjects.ExpandAll()
      End If

      'Wanted Folders
      If Not IsNothing(WantedProjects) Then
        WantedProjects.Nodes.Clear()
        WantedProjects.Nodes.Add(WantedMainNode)
        WantedProjects.ExpandAll()
      End If

      'History Folders
      If Not IsNothing(HistoryProjects) Then
        HistoryProjects.Nodes.Clear()
        HistoryProjects.Nodes.Add(HistoryMainNode)
        HistoryProjects.ExpandAll()
      End If

      'Event Folders
      If Not IsNothing(EventProjects) Then
        EventProjects.Nodes.Clear()
        EventProjects.Nodes.Add(EventMainNode)
        EventProjects.ExpandAll()
      End If

      'Performance Specs Folders
      If Not IsNothing(PerformanceSpecsProjects) Then
        PerformanceSpecsProjects.Nodes.Clear()
        PerformanceSpecsProjects.Nodes.Add(PerformanceMainNode)
        PerformanceSpecsProjects.ExpandAll()
      End If

      'Operating Costs Folders
      If Not IsNothing(OperatingCostsProjects) Then
        OperatingCostsProjects.Nodes.Clear()
        OperatingCostsProjects.Nodes.Add(OperatingMainNode)
        OperatingCostsProjects.ExpandAll()
      End If

      'Operating Costs Folders
      If Not IsNothing(MarketingSummariesProjects) Then
        MarketingSummariesProjects.Nodes.Clear()
        MarketingSummariesProjects.Nodes.Add(MarketingMainNode)
        MarketingSummariesProjects.ExpandAll()
      End If

      'Yacht Folder Projects
      If Not IsNothing(YachtProjects) Then
        YachtProjects.Nodes.Clear()
        YachtProjects.Nodes.Add(YachtMainNode)
        YachtProjects.ExpandAll()
      End If

      'Yacht History Folder Projects
      If Not IsNothing(YachtHistoryProjects) Then
        YachtHistoryProjects.Nodes.Clear()
        YachtHistoryProjects.Nodes.Add(YachtHistoryMainNode)
        YachtHistoryProjects.ExpandAll()
      End If

      'Yacht Event Folder Projects
      If Not IsNothing(YachtEventProjects) Then
        YachtEventProjects.Nodes.Clear()
        YachtEventProjects.Nodes.Add(YachtEventMainNode)
        YachtEventProjects.ExpandAll()
      End If

      'Airport Folder Projects
      If Not IsNothing(AirportProjects) Then
        AirportProjects.Nodes.Clear()
        AirportProjects.Nodes.Add(AirportMainNode)
        AirportProjects.ExpandAll()
      End If

      If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = True Then
        If Not IsNothing(ValuesProjects) Then
          ValuesProjects.Nodes.Clear()
          ValuesProjects.Nodes.Add(ValuesMainNode)
          ValuesProjects.ExpandAll()
          values_folder_container.CssClass = "aircraft_folder"
        End If
      End If


      FoldersTable = New DataTable
    Else
      If masterPage.aclsData_Temp.class_error <> "" Then
        masterPage.LogError("home.aspx.vb - GetFoldersListing() - " & masterPage.aclsData_Temp.class_error)
        masterPage.aclsData_Temp.class_error = ""
      End If
    End If
  End Sub

  Sub CrmClientDisplayFolders(ByRef mainNode As TreeNode, ByVal folderType As Integer)
    If clsGeneral.clsGeneral.isCrmDisplayMode Then
      mainNode.ChildNodes.Clear()
      Dim foldersTable As New DataTable
      Dim UserTableCheck As DataTable
      foldersTable = masterPage.aclsData_Temp.GetEvolutionFolderssBySubscription(0, Session.Item("localUser").crmUserLogin, Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmSubSeqNo, "", folderType, Nothing, "")

      UserTableCheck = masterPage.aclsData_Temp.Get_Client_User_By_Email_Address(Session.Item("localUser").crmLocalUserEmailAddress)
      If Not IsNothing(UserTableCheck) Then
        If UserTableCheck.Rows.Count > 0 Then
          foldersTable.Merge(masterPage.aclsdata_temp.Get_Client_Folders_Complete(folderType, UserTableCheck.Rows(0).Item("cliuser_id")))

          If Not IsNothing(foldersTable) Then
            If foldersTable.Rows.Count > 0 Then
              'Sort Together:
              Dim SortView As New DataView
              SortView = foldersTable.DefaultView
              SortView.Sort = "cfolder_name"
              foldersTable = SortView.ToTable()

              For Each r As DataRow In foldersTable.Rows
                Dim subnode As New TreeNode
                If (show_hidden_folders.Checked = False And r("cfolder_hide_flag") = "N") Or (show_hidden_folders.Checked = True) Then
                  If (hide_shared.Checked = True And r("cfolder_share") = "N") Or (hide_shared.Checked = False) Then
                    If r("source") = "CLIENT" Then
                      subnode.Text = "<span style=""display:block;min-width:230px;background-color: #ffece7 !important;padding:3px 3px 5px 0px;margin-bottom:2px;"">" & r("cfolder_name").ToString & "</span>"
                      subnode.Value = "value"
                      subnode.ImageUrl = DisplayFunctions.ReturnFolderImage(r("cfolder_method").ToString, r("cfolder_hide_flag").ToString, r("cfolder_share").ToString)


                      If Not IsDBNull(r("cfolder_data")) Then
                        subnode.NavigateUrl = "javascript:ParseCLIENTForm('" & r("cfolder_id").ToString & "',false,false," & IIf(folderType = 1, "true", "false") & ",false,false,'" & IIf(folderType = 3, clsGeneral.clsGeneral.translateClientAircraftToJetnet(r("cfolder_data").ToString), r("cfolder_data").ToString) & "');"
                      Else 'This is an index
                        'We need to look up the index information.
                        Dim FolderIndex As New DataTable
                        Dim ClientFolderString As String = ""
                        Dim FolderString As String = ""
                        Dim folderTypeString As String = "ac"
                        Select Case folderType
                          Case 1
                            folderTypeString = "comp"
                          Case 3
                            folderTypeString = "ac"
                        End Select
                        FolderIndex = masterPage.aclsData_Temp.Get_Client_Folder_Index(r("cfolder_id"))
                        For Each q As DataRow In FolderIndex.Rows
                          If q("cfoldind_jetnet_" & folderTypeString & "_id") > 0 Then
                            If FolderString <> "" Then
                              FolderString += ","
                            End If
                            FolderString += q("cfoldind_jetnet_" & folderTypeString & "_id").ToString
                          ElseIf q("cfoldind_client_" & folderTypeString & "_id") > 0 Then
                            If ClientFolderString <> "" Then
                              ClientFolderString += ","
                            End If
                            ClientFolderString += q("cfoldind_client_" & folderTypeString & "_id").ToString
                          End If
                        Next


                        If FolderString <> "" Or ClientFolderString <> "" Then
                          subnode.NavigateUrl = "javascript:ParseCLIENTForm('" & r("cfolder_id").ToString & "',false,false," & IIf(folderType = 1, "true", "false") & ",false,false,'COMPARE_" & folderTypeString & "_id=Equals" & IIf(FolderString <> "", "!~!" & folderTypeString & "_id=" & FolderString, "!~!" & folderTypeString & "_id=0") & IIf(ClientFolderString <> "", "!~!cli" & folderTypeString & "_id=" & ClientFolderString, "!~!cli" & folderTypeString & "_id=0") & "');"
                        Else
                          subnode.NavigateUrl = "javascript:alert('This folder contains no information.');"
                        End If
                      End If
                    Else
                      Dim FolderDataString As Array = Nothing
                      Dim FolderRawDataString As String = ""

                      If Not IsDBNull(r("cfolder_data")) Then
                        FolderDataString = Split(r("cfolder_data"), "THEREALSEARCHQUERY")
                        FolderRawDataString = r("cfolder_data").ToString.Trim
                      End If
                      subnode.Text = r("cfolder_name").ToString
                      subnode.Value = Replace(r("cfolder_data").ToString, "'", "\'")
                      subnode.ImageUrl = DisplayFunctions.ReturnFolderImage(r("cfolder_method").ToString, r("cfolder_hide_flag").ToString, r("cfolder_share").ToString)

                      'set tooltip as description.
                      subnode.ImageToolTip = r("cfolder_description").ToString
                      If Not String.IsNullOrEmpty(FolderRawDataString.Trim) Then
                        subnode.NavigateUrl = "javascript:ParseForm('" & r("cfolder_id").ToString & "', false,false," & IIf(folderType = 1, "true", "false") & ",false,false, '" + IIf(Not IsNothing(FolderDataString), Replace(FolderDataString(0), "'", "\'"), "") + "');"
                      Else
                        subnode.NavigateUrl = "javascript:alert('This folder contains no information.');"
                      End If
                    End If
                    mainNode.ChildNodes.Add(subnode)
                  End If
                End If
              Next
            End If
          End If

        End If
      End If

    End If
  End Sub

  Private Sub GetEventsListing(ByVal EventLabelToPopulate As Label, ByVal CRM As Boolean, ByVal TimeRD As RadioButtonList, ByVal CategoryRD As RadioButtonList)
    Dim cssClass As String = ""
    Dim EventCategory As String = ""
    Dim ResultsTable As New DataTable
    Dim EventNumberOfDays As Integer = -7
    Dim EventDate As String = ""
    Dim temp_string As String = ""
    EventLabelToPopulate.Text = ""

    Trace.Write("Start GetEventsListing Home.aspx" + Now.ToString)
    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br />Start GetEventsListing Home.aspx : " + Now.ToString + "<br />"

    Select Case TimeRD.SelectedValue
      Case "30"
        EventNumberOfDays = -30
      Case "90"
        EventNumberOfDays = -90
      Case "1"
        EventNumberOfDays = -1
      Case Else
        EventNumberOfDays = -7
    End Select

    EventCategory = CategoryRD.SelectedValue

    EventDate = Month(DateAdd(DateInterval.Day, EventNumberOfDays, Now())) & "/" & Day(DateAdd(DateInterval.Day, EventNumberOfDays, Now())) & "/" & Year(DateAdd(DateInterval.Day, EventNumberOfDays, Now()))


    'Session.Item("localUser").crmSelectedModels()
    ResultsTable = masterPage.aclsData_Temp.HomePageGetEventsListing(EventDate, Session.Item("localUser").crmSelectedModels, EventCategory)

    If Not IsNothing(ResultsTable) Then
      If ResultsTable.Rows.Count > 0 Then
        temp_string = "<table width=""100%"" cellpadding=""3"" cellspacing=""0"" class=""" & IIf(CRM, "data_aircraft_grid", "data_aircraft_grid") & """><tr class=""" & IIf(CRM, "header_row", "header_row") & """>"
        temp_string += "<td align=""left"" valign=""top"" " & IIf(Session.Item("isMobile"), "", "width='130'") & ">"
        temp_string += "<b class=""title"">Make</b>"
        temp_string += "</td>"
        temp_string += "<td align=""right"" valign=""top"">"
        temp_string += "<b class=""title"">Year</b>"
        temp_string += "</td>"
        temp_string += "<td align=""left"" valign=""top"" width='35' class=""mobile_display_off_cell"">"
        temp_string += " <b class=""title"">Ser #</b>"
        temp_string += "</td>"
        temp_string += "<td align=""left"" valign=""top"" class=""mobile_display_off_cell"">"
        temp_string += "<b class=""title"">Reg #</b>"
        temp_string += " </td>"
        temp_string += " <td align=""left"" valign=""top"" width='130' class=""mobile_display_off_cell""><b class=""title"">Activity</b></td>"
        temp_string += "<td align=""left"" valign=""top"" class=""mobile_display_off_cell""><b class=""title"">Description</b></td>"

        'mobile only display fields.
        temp_string += "<td align=""left"" valign=""top"" class=""mobile_display_on_cell""><b class=""title"">Ser #</b><br /><b class=""title"">Reg #</b></td>"
        temp_string += "<td align=""left"" valign=""top"" class=""mobile_display_on_cell""><b class=""title"">Activity</b><br /><b class=""title"">Description</b></td>"

        temp_string += "</tr>"

        EventLabelToPopulate.Text += temp_string
        temp_string = ""

        For Each r As DataRow In ResultsTable.Rows
          temp_string = "<tr class='" & cssClass & "'><td align=""left"" valign=""top"">"
          '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          ''''''''''''''''''''''''MAKE NAME''''''''''''''''''''''''''''''''''''''
          temp_string += "" & r("amod_make_name").ToString & " "
          '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          ''''''''''''''''''''''''MODEL NAME'''''''''''''''''''''''''''''''''''''
          If CRM = False Then
            temp_string += DisplayFunctions.WriteModelLink(r("amod_id"), r("amod_model_name"), True)
          Else
            temp_string += r("amod_model_name").ToString
          End If

          temp_string += "</td><td align=""left"" valign=""top"">"
          '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          ''''''''''''''''''''''''AC YEAR''''''''''''''''''''''''''''''''''''''''
          temp_string += r("ac_year").ToString
          temp_string += "</td><td align=""left"" valign=""top"" class=""mobile_display_off_cell"">"
          '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          ''''''''''''''''''''''''SERIAL NBR'''''''''''''''''''''''''''''''''''''
          If CRM = False Then
            temp_string += crmWebClient.DisplayFunctions.WriteDetailsLink(r("ac_id"), 0, 0, 0, True, r("ac_ser_no_full").ToString, "", "")
          Else
            temp_string += "<a href='details.aspx?ac_ID=" & r("ac_id") & "&type=3&source=JETNET'>"
            temp_string += r("ac_ser_no_full").ToString
            temp_string += "</a>"
          End If

          temp_string += "</td><td align=""left"" valign=""top"" class=""mobile_display_off_cell"">"
          '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          ''''''''''''''''''''''''REGISTR NBR''''''''''''''''''''''''''''''''''''
          temp_string += r("ac_reg_no").ToString
          temp_string += "</td>"
          '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          ''''''''''''''''''''''''ENTRY DATE'''''''''''''''''''''''''''''''''''''
          temp_string += "<td align=""left"" valign=""top"" class=""mobile_display_off_cell"">"
          temp_string += r("priorev_entry_date").ToString
          temp_string += "</td><td align=""left"" valign=""top"" class=""mobile_display_off_cell"">"
          '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          ''''''''''''''''''''''''SUBJECT''''''''''''''''''''''''''''''''''''''''
          If Not IsDBNull(r("priorev_subject")) Then
            If Not String.IsNullOrEmpty(r("priorev_subject")) Then
              temp_string += "" & r("priorev_subject").ToString & " "
            End If
          End If
          '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          ''''''''''''''''''''''''DESCRIPTION''''''''''''''''''''''''''''''''''''
          If Not IsDBNull(r("priorev_description")) Then
            If Not String.IsNullOrEmpty(r("priorev_description")) Then
              temp_string += "<span class=""tiny"">[" & r("priorev_description").ToString & "]</span>"
            End If
          End If

          temp_string += "</td><td align=""left"" valign=""top"" class=""mobile_display_on_cell"">"
          '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          ''''''''''''''''''''''''SERIAL NBR'''''''''''''''''''''''''''''''''''''
          If CRM = False Then
            temp_string += crmWebClient.DisplayFunctions.WriteDetailsLink(r("ac_id"), 0, 0, 0, True, r("ac_ser_no_full").ToString, "", "")
          Else
            temp_string += "<a href='details.aspx?ac_ID=" & r("ac_id") & "&type=3&source=JETNET'>"
            temp_string += r("ac_ser_no_full").ToString
            temp_string += "</a>"
          End If
          temp_string += "<br /><br />"
          '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          ''''''''''''''''''''''''REGISTR NBR''''''''''''''''''''''''''''''''''''
          temp_string += r("ac_reg_no").ToString

          temp_string += " </td><td align=""left"" valign=""top"" class=""mobile_display_on_cell"">"
          temp_string += r("priorev_entry_date").ToString
          temp_string += "<br /><br />"
          '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          ''''''''''''''''''''''''SUBJECT''''''''''''''''''''''''''''''''''''''''
          If Not IsDBNull(r("priorev_subject")) Then
            If Not String.IsNullOrEmpty(r("priorev_subject")) Then
              temp_string += "" & r("priorev_subject").ToString & " "
            End If
          End If
          '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          ''''''''''''''''''''''''DESCRIPTION''''''''''''''''''''''''''''''''''''
          If Not IsDBNull(r("priorev_description")) Then
            If Not String.IsNullOrEmpty(r("priorev_description")) Then
              temp_string += "<span class=""tiny"">[" & r("priorev_description").ToString & "]</span>"
            End If
          End If
          temp_string += " </td></tr>"

          If cssClass = "" Then
            cssClass = "alt_row"
          Else
            cssClass = ""
          End If

          EventLabelToPopulate.Text += temp_string
          temp_string = ""

        Next

        EventLabelToPopulate.Text += "</table>"
      Else
        If Session.Item("localUser").crmSelectedModels <> "" Then
          EventLabelToPopulate.CssClass = "padding" ' emphasis_text"
          EventLabelToPopulate.Text = "<br /><p align='center'>There are applicable events with these parameters.</p>"
        Else
          event_time_panel.CssClass = "display_none light_seafoam_green_header_color toggleSmallScreen"
          EventLabelToPopulate.CssClass = "padding" ' emphasis_text"
          '"<br /><p align='center'>There are no events currently.</p>"
          If CRM = False Then
            EventLabelToPopulate.Text = "<p align='left'>Welcome " & Session.Item("localUser").crmLocalUserFirstName.ToString & " " & Session.Item("localUser").crmLocalUserLastName.ToString & ".<br />To customize the default """ & market_summary_tab.HeaderText.ToString & """, ""Events"" and ""Wanted"" tabs, <a href='#' onclick=""javascript:window.open('Preferences.aspx','','scrollbars=yes,menubar=no,height=800,width=1150,resizable=yes,toolbar=no,location=no,status=no');"">select your preferred models using this link</a>."
          Else
            EventLabelToPopulate.Text = "<br /><p align='center'>There are no events currently.</p>"
          End If
        End If
      End If
    Else
      'error logging here.
      Master.LogError("home.aspx.vb - GetEventsListing() - " & masterPage.aclsData_Temp.class_error)
      'clear error for data layer class
      masterPage.aclsData_Temp.class_error = ""
    End If

    Trace.Write("End GetEventsListing Home.aspx" + Now.ToString)
    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br />End GetEventsListing Home.aspx : " + Now.ToString + "<br />"

  End Sub

  Private Sub GetFolderEventsListing(ByVal EventLabelToPopulate As Label, ByVal CRM As Boolean, ByVal TimeRD As RadioButtonList, ByVal CategoryRD As RadioButtonList)
    Dim cssClass As String = ""
    Dim EventCategory As String = ""
    Dim ResultsTable As New DataTable
    Dim EventNumberOfDays As Integer = -7
    Dim EventDate As String = ""

    Dim orig_temp_time As Integer = 0
    Dim temp_time As Integer = 0
    Dim this_span As String = ""
    Dim temp_days As Integer = 0
    Dim temp_hours As Integer = 0
    Dim temp_minutes As Integer = 0
    Dim temp_months As Integer = 0
    Dim ts As System.TimeSpan
    Dim date_is_added As Boolean = False
    Dim temp_last_process As String = ""
    Dim temp_date As String = ""
    Dim next_Date As String = ""
    Dim next_Date_print As String = ""
    Dim last_date_print As String = ""

    EventLabelToPopulate.Text = ""

    Trace.Write("Start GetFolderEventsListing Home.aspx" + Now.ToString)
    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br />Start GetEventsListing Home.aspx : " + Now.ToString + "<br />"

    Select Case TimeRD.SelectedValue
      Case "30"
        EventNumberOfDays = -30
      Case "90"
        EventNumberOfDays = -90
      Case "1"
        EventNumberOfDays = -1
      Case Else
        EventNumberOfDays = -7
    End Select

    EventCategory = CategoryRD.SelectedValue


    EventDate = Month(DateAdd(DateInterval.Day, EventNumberOfDays, Now())) & "/" & Day(DateAdd(DateInterval.Day, EventNumberOfDays, Now())) & "/" & Year(DateAdd(DateInterval.Day, EventNumberOfDays, Now()))


    'Session.Item("localUser").crmSelectedModels()
    ResultsTable = masterPage.aclsData_Temp.HomePageGetFolderEventsListing(EventDate, Session.Item("localUser").crmSelectedModels, EventCategory, Session.Item("localUser").crmUserLogin, Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmSubSeqNo)

    If Not IsNothing(ResultsTable) Then
      If ResultsTable.Rows.Count > 0 Then
        EventLabelToPopulate.Text = "<table width=""100%"" cellpadding=""2"" cellspacing=""0"" class=""" & IIf(CRM, "data_aircraft_grid", "data_aircraft_grid") & """><tr class=""" & IIf(CRM, "header_row", "header_row") & """>"
        EventLabelToPopulate.Text += "<td align=""left"" valign=""top"" width='210'>"
        EventLabelToPopulate.Text += "<b class=""title"">Event Name - Description</b>"
        EventLabelToPopulate.Text += "</td>"
        EventLabelToPopulate.Text += "<td align=""left"" valign=""top"" width='95' class=""mobile_display_off_cell"">"
        EventLabelToPopulate.Text += "<b class=""title"">Last Run</b>"
        EventLabelToPopulate.Text += " </td>"
        EventLabelToPopulate.Text += "<td align=""left"" valign=""top"" width='95' class=""mobile_display_off_cell"">"
        EventLabelToPopulate.Text += "<b class=""title"">Scheduled</b>"
        EventLabelToPopulate.Text += " </td>"
        EventLabelToPopulate.Text += " <td align=""left"" valign=""top"" width='170' class=""mobile_display_off_cell"">"
        EventLabelToPopulate.Text += "<b class=""title"">Send To</b>"
        EventLabelToPopulate.Text += "</td>"
        EventLabelToPopulate.Text += "<td align=""left"" valign=""top""  width='110' class=""mobile_display_off_cell"">"
        EventLabelToPopulate.Text += " <b class=""title"">Run Every</b>"
        EventLabelToPopulate.Text += "</td>"
        EventLabelToPopulate.Text += "</tr>"

        For Each r As DataRow In ResultsTable.Rows

          date_is_added = False

          EventLabelToPopulate.Text += "<tr class='" & cssClass & "'>"
          EventLabelToPopulate.Text += "<td align=""left"" valign=""top"">"
          If Not IsDBNull(r("cfolder_name")) Then
            EventLabelToPopulate.Text += "<b class=""title"">" & r("cfolder_name").ToString & "</b>"
          End If
          If Not IsDBNull(r("cfolder_description")) Then
            If Trim(r("cfolder_description").ToString) <> "" Then
              EventLabelToPopulate.Text += " - " & Left(r("cfolder_description").ToString, 90)
              If Len(r("cfolder_description")) > 100 Then
                EventLabelToPopulate.Text += "..."
              End If
            End If
          End If
          EventLabelToPopulate.Text += "&nbsp;</td>"


          If Not IsDBNull(r("cfolder_jetnet_run_last_process_date")) Then
            temp_last_process = r("cfolder_jetnet_run_last_process_date").ToString
          End If


          last_date_print = Trim(Replace(Replace(temp_last_process, " AM", ""), " PM", ""))
          last_date_print = Left(Trim(last_date_print), Len(Trim(last_date_print)) - 3)


          EventLabelToPopulate.Text += "<td align=""left"" valign=""top"" class=""mobile_display_off_cell"">"
          If Trim(last_date_print) <> "" Then
            EventLabelToPopulate.Text += last_date_print
          Else
            EventLabelToPopulate.Text += "&nbsp;"
          End If

          EventLabelToPopulate.Text += "</td>"



          If Not IsDBNull(r("cfolder_jetnet_run_freq_in_mins")) Then
            orig_temp_time = CInt(r("cfolder_jetnet_run_freq_in_mins"))


            temp_months = CInt(orig_temp_time / 43829)
            If temp_months > 1 Then
              If CInt(temp_months * 43829) > CInt(orig_temp_time) Then ' rounded bad
                temp_months = temp_months - 1
              End If

              temp_time = CInt(orig_temp_time - (temp_months * 43829))
            Else
              temp_time = orig_temp_time
            End If

            'temp_date = DateAdd(DateInterval.Minute, orig_temp_time, CDate(temp_last_process))

            ' temp_date = DateDiff(DateInterval.Minute, CDate(temp_last_process), CDate(temp_date))

            ts = New System.TimeSpan(0, temp_time, 0)
            temp_days = ts.Days
            temp_hours = ts.Hours
            temp_minutes = ts.Minutes

            If CInt(temp_days) >= 30 Then
              temp_months = CInt(temp_days / 30)

              If CInt(temp_months * 30) > CInt(temp_days) Then
                temp_months = temp_months - 1
              End If


              temp_days = CInt(temp_days - CInt(temp_months * 30)) ' it rounded bad
            End If


            'If CInt(temp_time) > temp_min_day Then ' minutes in a day
            '  temp_days = CInt(temp_time / temp_min_day)

            '  temp_temp = CInt(temp_min_day * temp_days)

            '  If CInt(temp_temp) > CInt(orig_temp_time) Then
            '    temp_days = (temp_days - 1) ' it rounded bad
            '  End If

            '  If temp_days >= 30 Then

            '    temp_month = CInt(temp_days / 30)
            '    temp_temp = CInt(temp_min_day * temp_days)

            '    If CInt(temp_temp) > CInt(orig_temp_time) Then

            '    End If

            '    If CInt(30 * temp_days) > CInt(temp_month) Then

            '    End If

            '  End If

            'ElseIf CInt(temp_time) >= 60 Then
            '  temp_hours = CInt(temp_time / 60)

            'ElseIf CInt(temp_time) < 60 Then


            'End If

            next_Date = DateAdd(DateInterval.Month, temp_months, CDate(temp_last_process))
            next_Date = DateAdd(DateInterval.Day, temp_days, CDate(next_Date))
            next_Date = DateAdd(DateInterval.Hour, temp_hours, CDate(next_Date))
            next_Date = DateAdd(DateInterval.Minute, temp_minutes, CDate(next_Date))

            next_Date_print = Trim(Replace(Replace(next_Date, " AM", ""), " PM", ""))
            next_Date_print = Left(Trim(next_Date_print), Len(Trim(next_Date_print)) - 3)

          Else
            next_Date_print = "<A href='#'>Schedule</a>"
          End If

          EventLabelToPopulate.Text += "<td align=""left"" valign=""top"" class=""mobile_display_off_cell"">"
          If Trim(next_Date_print) <> "" Then
            EventLabelToPopulate.Text += next_Date_print
          Else
            EventLabelToPopulate.Text += "&nbsp;"
          End If
          ' this is correct, but not correct due to how we are adding up numbers 
          'If Not IsDBNull(r("NEXTRUN")) Then
          '  EventLabelToPopulate.Text += "--" & r("NEXTRUN").ToString
          'End If
          EventLabelToPopulate.Text += "</td>"

          EventLabelToPopulate.Text += "<td align=""left"" valign=""top"" class=""mobile_display_off_cell"">"
          If Not IsDBNull(r("cfolder_jetnet_run_reply_username")) Then
            EventLabelToPopulate.Text += r("cfolder_jetnet_run_reply_username").ToString
          End If
          If Not IsDBNull(r("cfolder_jetnet_run_reply_email")) Then
            EventLabelToPopulate.Text += "<br><a href='mailto:" & r("cfolder_jetnet_run_reply_email").ToString & "'>" & r("cfolder_jetnet_run_reply_email").ToString & "</a> "
          End If
          EventLabelToPopulate.Text += "&nbsp;</td>"


          EventLabelToPopulate.Text += "<td align=""left"" valign=""top"" class=""mobile_display_off_cell"">"

          If Not IsDBNull(r("cfolder_jetnet_run_freq_in_mins")) Then

            If temp_months > 0 Then
              EventLabelToPopulate.Text += temp_months.ToString & " Months"
              date_is_added = True
            End If

            If temp_days > 0 Then
              If date_is_added = True Then
                EventLabelToPopulate.Text += ", "
              End If
              EventLabelToPopulate.Text += temp_days.ToString & " Days"
              date_is_added = True
            End If

            If temp_hours > 0 Then
              If date_is_added = True Then
                EventLabelToPopulate.Text += ", "
              End If
              EventLabelToPopulate.Text += temp_hours.ToString & " Hours"
              date_is_added = True
            End If

            If temp_minutes > 0 Then
              If date_is_added = True Then
                EventLabelToPopulate.Text += ", "
              End If
              EventLabelToPopulate.Text += temp_minutes.ToString & " Min"
              date_is_added = True
            End If

          End If

          EventLabelToPopulate.Text += "&nbsp;</td>"

          EventLabelToPopulate.Text += "</tr>"

          If cssClass = "" Then
            cssClass = "alt_row"
          Else
            cssClass = ""
          End If
        Next

        EventLabelToPopulate.Text += "</table>"
      Else
        If Session.Item("localUser").crmSelectedModels <> "" Then
          EventLabelToPopulate.CssClass = "padding" ' emphasis_text"
          EventLabelToPopulate.Text = "<br /><p align='center'>There are applicable events with these parameters.</p>"
        Else
          event_time_panel.CssClass = "display_none light_seafoam_green_header_color toggleSmallScreen"
          EventLabelToPopulate.CssClass = "padding" ' emphasis_text"
          '"<br /><p align='center'>There are no events currently.</p>"
          If CRM = False Then
            EventLabelToPopulate.Text = "<p align='left'>Welcome " & Session.Item("localUser").crmLocalUserFirstName.ToString & " " & Session.Item("localUser").crmLocalUserLastName.ToString & ".<br />To customize the default """ & market_summary_tab.HeaderText.ToString & """, ""Events"" and ""Wanted"" tabs, <a href='#' onclick=""javascript:window.open('Preferences.aspx','','scrollbars=yes,menubar=no,height=800,width=1150,resizable=yes,toolbar=no,location=no,status=no');"">select your preferred models using this link</a>."
          Else
            EventLabelToPopulate.Text = "<br /><p align='center'>There are no events currently.</p>"
          End If
        End If
      End If
    Else
      'error logging here.
      Master.LogError("home.aspx.vb - GetEventsListing() - " & masterPage.aclsData_Temp.class_error)
      'clear error for data layer class
      masterPage.aclsData_Temp.class_error = ""
    End If

    Trace.Write("End GetEventsListing Home.aspx" + Now.ToString)
    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br />End GetEventsListing Home.aspx : " + Now.ToString + "<br />"

  End Sub

  Private Sub toggleSalesEvo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles toggleSalesEvo.Click
    Dim cssClass As String = ""
    Dim ResultsTable As New DataTable
    Dim Temp_String As String = ""
    SalesSumamry(market_listing_label, False, cssClass, ResultsTable, Temp_String)
    Recent_SalesSumamry(market_listing_label, False, cssClass, ResultsTable, Temp_String)
    main_home_update_panel.Update()
    toggleSalesEvo.Visible = False

    System.Web.UI.ScriptManager.RegisterStartupScript(Me.main_home_update_panel, Me.GetType(), "cursorDefault", vbCrLf & "javascript:ChangeTheMouseCursorOnItemParentDocument('cursor_default');", True)


  End Sub

  Private Sub toggleSales_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles toggleSales.Click
    Dim cssClass As String = ""
    Dim ResultsTable As New DataTable
    Dim Temp_String As String = ""
    SalesSumamry(crm_market_overview, True, cssClass, ResultsTable, Temp_String)
    crm_update_panel.Update()
    toggleSales.Visible = False
  End Sub

  Public Sub SalesSumamry(ByRef marketLabel As Label, ByRef crmView As Boolean, ByRef cssClass As String, ByRef ResultsTable As DataTable, ByRef temp_string As String)
    Dim acObject As New marketSummaryObjAircraft()
    Dim mktSummaryFunctions As New marketSummaryFunctions()

    mktSummaryFunctions.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
    mktSummaryFunctions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
    mktSummaryFunctions.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
    mktSummaryFunctions.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
    mktSummaryFunctions.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

    Dim sRefLink As String = ""
    Dim sRefTitle As String = ""
    Dim tmpTransLink As String = ""
    Dim tmpAcDetails As String = ""
    Dim nSaleCount As Integer = 0
    Dim nSalePriceCount As Integer = 0
    Dim nSalePriceDisplayCount As Integer = 0
    Dim total_sale_count As Long = 0
    Dim total_sales As Long = 0
    Dim total_sales_w_price As Long = 0
    Dim total_displayable_sales_w_price As Long = 0
    Dim total_percent As Double = 0.0
    Dim total_low_ask As Long = 99000000
    Dim total_high_ask As Long = 0
    Dim total_low As Long = 99000000
    Dim total_high As Long = 0
    Dim total_avg As Long = 0
    Dim total_avg_sale As Long = 0


    ' If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM = False Then

    ' ADDED IN MSW - 8-8-16 ---- SPI SECTION-----------------------------------
    cssClass = ""
    ResultsTable = masterPage.aclsData_Temp.HomePageGetMarketSummaryListing_SPI(Session.Item("localUser").crmSelectedModels, number_of_months_divide)
    If Not IsNothing(ResultsTable) Then
      If ResultsTable.Rows.Count > 0 Then


        marketLabel.Text += "<table width=""100%"" cellpadding=""3"" cellspacing=""0"" class=""data_aircraft_grid"">"


        marketLabel.Text += "<tr class=""header_row"">"
        marketLabel.Text += "<td align=""center"" valign=""top"" colspan='11'>"
        marketLabel.Text += "<b class=""title"">RECENT RETAIL (PRE-OWNED) SALES – LAST " & number_of_months_divide & " MONTHS</b>"
        marketLabel.Text += "</td>"
        marketLabel.Text += "</tr>"


        marketLabel.Text += "<tr class=""header_row"">"
        If crmView Then
          marketLabel.Text += "<td align=""right"" valign=""top"" width=""16"">"
          marketLabel.Text += "<b class=""title"">&nbsp;</b>"
          marketLabel.Text += "</td>"
        Else
          If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag Then
            ' If Not CRMView Then
            marketLabel.Text += "<td align=""right"" valign=""top"" width=""16"">"
            marketLabel.Text += "<b class=""title"">&nbsp;</b>"
            marketLabel.Text += "</td>"
            '  End If
          End If
        End If

        marketLabel.Text += "<td align=""left"" valign=""top""><b class=""title"">Make/Model</b></td>"

        marketLabel.Text += "<td align=""right"" valign=""top"" nowrap='nowrap'>"
        'marketLabel.Text += "<b class=""title"">AC IN OPERATION</b>"
        marketLabel.Text += "<span class=""help_cursor"" title=""BASED ON RETAIL SALES (LAST 365 Days)""><b class=""title"">RETAIL<br/>SALES</b></span>"
        marketLabel.Text += "</td>"

        marketLabel.Text += "<td align=""right"" valign=""top"">"
        marketLabel.Text += "<span class=""help_cursor"" title=""BASED ON RETAIL SALES (LAST 365 Days)""><b class=""title"">SALES W PRICE /DISPLAYABLE</b>"
        marketLabel.Text += "</span></td>"

        marketLabel.Text += "<td align=""right"" valign=""top"" nowrap='nowrap'><b class=""title"">Percent<br/>w Price</b></td>"


        marketLabel.Text += "<td align=""right"" valign=""top""><b class=""title"">Low Asking</b></td>"
        marketLabel.Text += "<td align=""right"" valign=""top""><b class=""title"">Avg Asking</b></td>"
        marketLabel.Text += "<td align=""right"" valign=""top""><b class=""title"">High Asking</b></td>"

        marketLabel.Text += "<td align=""right"" valign=""top""><b class=""title"">Low Sale</b></td>"
        marketLabel.Text += "<td align=""right"" valign=""top""><b class=""title"">Avg Sale</b></td>"
        marketLabel.Text += "<td align=""right"" valign=""top""><b class=""title"">High Sale</b></td>"
        marketLabel.Text += "</tr>"

        For Each r As DataRow In ResultsTable.Rows

          temp_string += "<tr class='" & cssClass & "'>"

          If crmView = True Then
            temp_string += "<td align=""left"" valign=""middle"" width=""16"">"
            'Dim ValueTable As New DataTable
            'ValueTable = Master.aclsData_Temp.Get_Client_Value_By_Model(r("amod_id"))
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '''''''''''''''''''''''''LINK TO VALUE''''''''''''''''''''''''''''''''''
            'If Not IsNothing(ValueTable) Then

            '  If ValueTable.Rows.Count > 0 Then
            '    temp_string += "<img src=""images/current_value.png"" alt="""" alt='Launch Values View' class='help_cursor' title='Launch Market Valuation' onclick=""javascript:load('view_template.aspx?ViewID=19&noteID=" & ValueTable.Rows(0).Item("lnote_id") & "&amod_ID=" & r("amod_id") & "&noMaster=false','','scrollbars=yes,menubar=no,height=700,width=1250,resizable=yes,toolbar=no,location=no,status=no');""/>"
            '  Else

            '    Dim FakeTable As New DataTable

            '    'Okay let's fake in a link here by getting a client aircraft.
            '    FakeTable = Master.aclsData_Temp.Get_Client_Aircraft_By_Model(CLng(r("amod_id").ToString))
            '    If Not IsNothing(FakeTable) Then
            '      If FakeTable.Rows.Count > 0 Then
            '        temp_string += "<img src=""images/current_value.png"" alt="""" alt='Launch Values View' class='help_cursor' title='Launch Market Valuation' onclick=""javascript:load('edit_note.aspx?action=new&amp;type=valuation&amp;cat_key=0&amp;ac_ID=" & FakeTable.Rows(0).Item("cliaircraft_jetnet_ac_id") & "&source=JETNET&listing=true&amp;refreshing=view&amod_ID=" & r("amod_id") & "&temporary=true" & "','','scrollbars=yes,menubar=no,height=700,width=1250,resizable=yes,toolbar=no,location=no,status=no');""/>"
            '      End If
            '    End If
            '  End If
            'End If

            'ValueTable.Dispose()
            temp_string += "<img src=""images/current_value.png"" alt="""" alt='Launch  Market Summary' class='help_cursor values_icon_width' title='Launch Market Summary' onclick=""javascript:load('view_template.aspx?noMaster=false&ViewID=1&ViewName=Model Market Summary&amod_id=" & r("amod_id").ToString & "&activetab=2" & "','','scrollbars=yes,menubar=no,height=700,width=1250,resizable=yes,toolbar=no,location=no,status=no');""/>"
            temp_string += "</td>"

          ElseIf HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = True Then

            If Not crmView Then
              temp_string += "<td align=""left"" valign=""middle"" width=""16"">"
              temp_string += "<img src=""images/current_value.png"" alt="""" alt='Value View' class='help_cursor values_icon_width' title='Launch Values View' onclick=""javascript:load('view_template.aspx?ViewID=27&amod_id=" & r("amod_id") & "','','scrollbars=yes,menubar=no,height=700,width=1250,resizable=yes,toolbar=no,location=no,status=no');""/>"
              temp_string += "</td>"
            End If

          End If


          temp_string += "<td align=""left"" valign=""middle"" nowrap='nowrap'>"
          temp_string += "" & r("amod_make_name").ToString & " "
          temp_string += DisplayFunctions.WriteModelLink(r("amod_id"), r("amod_model_name"), True)
          temp_string += "&nbsp;</td>"

          'temp_string += "<td align=""right"" valign=""middle"">"
          'If crmView Then
          '  If Not IsDBNull(r("INOP")) Then
          '    temp_string += "" & r("INOP").ToString & ""
          '  End If
          'Else
          '  If Not IsDBNull(r("INOP")) Then
          '    temp_string += "<a href='#' onclick=""javascript:document.body.style.cursor = 'wait';SubmitForm('" & r("amod_id") & "','3','N','N','" & r("amod_type_code").ToString & "|" & r("amod_airframe_type_code").ToString & "','" & r("amod_make_name") & "');"">" & r("INOP").ToString & "</a>"
          '  End If
          'End If

          'temp_string += "&nbsp;</td>" 


          Dim salePriceString As String = ""

          ' column for "ac with prices"
          If Not IsDBNull(r("SALECOUNT")) Then
            If Not String.IsNullOrEmpty(r("SALECOUNT").ToString.Trim) Then
              If IsNumeric(r("SALECOUNT").ToString) Then
                nSaleCount = CInt(r("SALECOUNT").ToString)
              End If
            End If
          End If

          sRefLink = "javascript:ParseForm('0',true,false,false,false,false,'"

          ' ac info

          acObject.ModelsString = r("amod_id").ToString
          acObject.MakeString = r("amod_make_name").ToString
          acObject.TypeString = r("amod_type_code").ToString
          acObject.AirframeTypeString = r("amod_airframe_type_code").ToString
          acObject.CombinedAirframeTypeString = ""

          '' loop through the inUserProductCode and create the Where Clause  
          'For nloop = 0 To UBound(Session.Item("localPreferences").ProductCode)

          '  Select Case Session.Item("localPreferences").ProductCode(nloop)
          '    Case eProductCodeTypes.H
          '      acObject.bHasHelicopter = True
          '    Case eProductCodeTypes.B, eProductCodeTypes.S, eProductCodeTypes.I
          '      acObject.bHasBusiness = True
          '    Case eProductCodeTypes.C
          '      acObject.bHasCommercial = True
          '  End Select
          'Next

          tmpAcDetails = mktSummaryFunctions.make_linkback_aircraftInfo(acObject)

          If Not String.IsNullOrEmpty(tmpAcDetails.Trim) Then
            sRefLink += tmpAcDetails.Trim + "!~!"
          End If

          ' transaction date (range)
          sRefLink += "journ_date_operator=Between!~!journ_date=" + mktSummaryFunctions.make_linkback_dateRange("", False, True, number_of_months_divide) + "!~!"

          ' transaction type  AND NOT (journ_subcat_code_part3 IN (?DB?,?DS?,?FI?,?FY?,?IT?,?MF?,?RE?,?CC?,?LS?,?RM?))
          tmpTransLink = mktSummaryFunctions.make_linkback_transactionInfo("WS", True, True, True, "jcat_used_retail_sales_flag equals ?Y? AND journ_newac_flag equals ?N?").Trim

          If Not String.IsNullOrEmpty(tmpTransLink) Then
            sRefLink += tmpTransLink + "!~!"
          End If

          sRefLink += "clearSelection=true!~!fromHomePage=true');"

          sRefTitle = IIf(CBool(HttpContext.Current.Application.Item("DebugFlag").ToString), " title=""" + sRefLink.Trim + """", " title=""Click to view Retail Sales""")






          If nSaleCount > 0 Then
            If crmView Then
              salePriceString += "<td align=""right"" valign=""middle"">" + FormatNumber(nSaleCount, 0, True, False, True) + "&nbsp;"
            Else
              salePriceString += "<td align=""right"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink.Trim + """" + sRefTitle + ">" + FormatNumber(nSaleCount, 0, True, False, True) + "</a>&nbsp;"
            End If
            total_sales = total_sales + nSaleCount
          Else
            salePriceString += "<td align=""right"" valign=""middle"">0&nbsp;"
          End If

          'make sales into its own column
          salePriceString += "</td>"
          salePriceString += "<td align=""right"" valign=""middle"">"

          ' column for "ac with prices count"
          If Not IsDBNull(r("SALEPRICECOUNT")) Then
            If Not String.IsNullOrEmpty(r("SALEPRICECOUNT").ToString.Trim) Then
              If IsNumeric(r("SALEPRICECOUNT").ToString) Then
                nSalePriceCount = CInt(r("SALEPRICECOUNT").ToString)
              End If
            End If
          End If

          sRefLink = "javascript:ParseForm('0',true,false,false,false,false,'"

          ' ac info
          If Not String.IsNullOrEmpty(tmpAcDetails.Trim) Then
            sRefLink += tmpAcDetails.Trim + "!~!"
          End If

          ' transaction date (range)
          sRefLink += "journ_date_operator=Between!~!journ_date=" + mktSummaryFunctions.make_linkback_dateRange("", False, True, number_of_months_divide) + "!~!"

          ' transaction type
          tmpTransLink = mktSummaryFunctions.make_linkback_transactionInfo("WS", True, True, True, "jcat_used_retail_sales_flag equals ?Y? AND journ_newac_flag equals ?N? AND ac_sale_price > 0").Trim

          If Not String.IsNullOrEmpty(tmpTransLink) Then
            sRefLink += tmpTransLink + "!~!"
          End If

          sRefLink += "clearSelection=true!~!fromHomePage=true');"

          sRefTitle = IIf(CBool(HttpContext.Current.Application.Item("DebugFlag").ToString), " title=""" + sRefLink.Trim + """", " title=""Click to view Retail Sales with Prices""")

          If nSalePriceCount > 0 Then
            If crmView Then
              salePriceString += FormatNumber(nSalePriceCount, 0, True, False, True) + "&nbsp;/&nbsp;"
              total_sales_w_price = total_sales_w_price + nSalePriceCount
            Else
              salePriceString += "<a class=""underline cursor"" onclick=""" + sRefLink.Trim + """" + sRefTitle + ">" + FormatNumber(nSalePriceCount, 0, True, False, True) + "</a>&nbsp;/&nbsp;"
              total_sales_w_price = total_sales_w_price + nSalePriceCount
            End If
          Else
            salePriceString += "0&nbsp;/&nbsp;"
          End If

          ' column for "ac with prices count display"
          If Not IsDBNull(r("SALEPRICEDISPLAYCOUNT")) Then
            If Not String.IsNullOrEmpty(r("SALEPRICEDISPLAYCOUNT").ToString.Trim) Then
              If IsNumeric(r("SALEPRICEDISPLAYCOUNT").ToString) Then
                nSalePriceDisplayCount = CInt(r("SALEPRICEDISPLAYCOUNT").ToString)
                total_displayable_sales_w_price = total_displayable_sales_w_price + nSalePriceDisplayCount
              End If
            End If
          End If

          sRefLink = "javascript:ParseForm('0',true,false,false,false,false,'"

          ' ac info
          If Not String.IsNullOrEmpty(tmpAcDetails.Trim) Then
            sRefLink += tmpAcDetails.Trim + "!~!"
          End If

          ' transaction date (range)
          sRefLink += "journ_date_operator=Between!~!journ_date=" + mktSummaryFunctions.make_linkback_dateRange("", False, True, number_of_months_divide) + "!~!"

          ' transaction type
          tmpTransLink = mktSummaryFunctions.make_linkback_transactionInfo("WS", True, True, True, "jcat_used_retail_sales_flag equals ?Y? AND journ_newac_flag equals ?N? AND ac_sale_price > 0 AND ac_sale_price_display_flag equals ?Y?").Trim

          If Not String.IsNullOrEmpty(tmpTransLink) Then
            sRefLink += tmpTransLink + "!~!"
          End If

          sRefLink += "clearSelection=true!~!fromHomePage=true');"

          sRefTitle = IIf(CBool(HttpContext.Current.Application.Item("DebugFlag").ToString), " title=""" + sRefLink.Trim + """", " title=""Click to view Retail Sales with Displayable Prices""")

          If nSalePriceDisplayCount > 0 Then
            If crmView Then
              salePriceString += FormatNumber(nSalePriceDisplayCount, 0, True, False, True) + "</td>"
            Else
              salePriceString += "<a class=""underline cursor"" onclick=""" + sRefLink.Trim + """" + sRefTitle + ">" + FormatNumber(nSalePriceDisplayCount, 0, True, False, True) + "</a></td>"
            End If
          Else
            salePriceString += "0</td>"
          End If

          temp_string += salePriceString.Trim


          temp_string += "<td align=""right"" valign=""middle"">"
          If nSaleCount > 0 Then
            temp_string += FormatNumber(CDbl((nSalePriceCount / nSaleCount) * 100), 1)
          Else
            temp_string += "0.00"
          End If
          temp_string += "%&nbsp;</td>"



          temp_string += "<td align=""right"" valign=""middle"">"
          If Not IsDBNull(r("LOWASKING")) Then
            temp_string += FormatNumber((r("LOWASKING") / 1000), 0) & "k"
            If (r("LOWASKING") / 1000) < total_low_ask Then
              total_low_ask = FormatNumber((r("LOWASKING") / 1000), 0)
            End If
          End If
          temp_string += "&nbsp;</td>"

          temp_string += "<td align=""right"" valign=""middle"">"
          If Not IsDBNull(r("AVGASKING")) Then
            temp_string += FormatNumber((r("AVGASKING") / 1000), 0) & "k"
            '  total_avg = total_avg + (r("AVGASKING") * nSalePriceCount)
          End If
          temp_string += "&nbsp;</td>"

          temp_string += "<td align=""right"" valign=""middle"">"
          If Not IsDBNull(r("HIGHASKING")) Then
            temp_string += FormatNumber((r("HIGHASKING") / 1000), 0) & "k"
            If (r("HIGHASKING") / 1000) > total_high_ask Then
              total_high_ask = FormatNumber((r("HIGHASKING") / 1000), 0)
            End If
          End If
          temp_string += "&nbsp;</td>"


          If (crmView And HttpContext.Current.Session.Item("localSubscription").crmSalesPriceIndex_Flag = True) Or (HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag = True) Then
            temp_string += "<td align=""right"" valign=""middle"">"
            If Not IsDBNull(r("LOWSALE")) Then
              temp_string += FormatNumber((r("LOWSALE") / 1000), 0) & "k"
              If (r("LOWSALE") / 1000) < total_low Then
                total_low = FormatNumber((r("LOWSALE") / 1000), 0)
              End If
            End If
            temp_string += "&nbsp;</td>"

            temp_string += "<td align=""right"" valign=""middle"">"
            If Not IsDBNull(r("AVGSALE")) Then
              temp_string += "<font color='red'>" & FormatNumber((r("AVGSALE") / 1000), 0) & "k</font>"
              total_avg_sale = total_avg_sale + (r("AVGSALE") * nSalePriceCount)
            End If
            temp_string += "&nbsp;</td>"

            temp_string += "<td align=""right"" valign=""middle"">"
            If Not IsDBNull(r("HIGHSALE")) Then
              temp_string += FormatNumber((r("HIGHSALE") / 1000), 0) & "k"
              If (r("HIGHSALE") / 1000) > total_high Then
                total_high = FormatNumber((r("HIGHSALE") / 1000), 0)
              End If
            Else
            End If
            temp_string += "&nbsp;</td>"
          Else
            temp_string += "<td align=""right"" valign=""middle"">"
            If Not IsDBNull(r("LOWSALE")) Then
              ' temp_string += "<A href='' alt='VALUES Subscribers Only' title='VALUES Subscribers Only'>###,###k</a>"
              temp_string += "<A href='#' alt='Available to VALUES Subscribers Only' title='Available to VALUES Subscribers Only'>$$$</a>"
            End If
            temp_string += "&nbsp;</td>"

            temp_string += "<td align=""right"" valign=""middle"">"
            If Not IsDBNull(r("AVGSALE")) Then
              temp_string += "<A href='#' alt='Available to VALUES Subscribers Only' title='Available to VALUES Subscribers Only'>$$$</a>"
            End If
            temp_string += "&nbsp;</td>"

            temp_string += "<td align=""right"" valign=""middle"">"
            If Not IsDBNull(r("HIGHSALE")) Then
              temp_string += "<A href='#' alt='Available to VALUES Subscribers Only' title='Available to VALUES Subscribers Only'>$$$</a>"
            End If
            temp_string += "&nbsp;</td>"
          End If



          temp_string += "</tr>"
          If cssClass = "" Then
            cssClass = "alt_row"
          Else
            cssClass = ""
          End If

          marketLabel.Text += temp_string
          temp_string = ""
        Next

        temp_string = "<tr>"
        temp_string += "<td align='right' colspan='2'><b>Totals</b></td>"
        temp_string += "<td align='right'>" & FormatNumber(total_sales, 0) & "&nbsp;</td>"
        temp_string += "<td align='right'>" & FormatNumber(total_sales_w_price, 0) & " / " & FormatNumber(total_displayable_sales_w_price, 0) & "&nbsp;</td>"

        total_percent = ((total_sales_w_price / total_sales) * 100)
        temp_string += "<td align='right'>" & FormatNumber(total_percent, 1) & "%&nbsp;</td>"


        temp_string += "<td align='right'>" & FormatNumber(total_low_ask, 0) & "k&nbsp;</td>"
        temp_string += "<td align='right'>N/A&nbsp;</td>"
        temp_string += "<td align='right'>" & FormatNumber(total_high_ask, 0) & "k&nbsp;</td>"

        If total_low = 99000000 Then
          temp_string += "<td align='right'>N/A&nbsp;</td>"
        Else
          temp_string += "<td align='right'>" & FormatNumber(total_low, 0) & "k&nbsp;</td>"
        End If


        If total_sales_w_price > 0 Then
          total_avg_sale = (total_avg_sale / total_sales_w_price)
          total_avg_sale = (total_avg_sale / 1000)
        End If

        If total_avg_sale = 0 Then
          temp_string += "<td align='right'><font color='red'>N/A&nbsp;</font></td>"
        Else
          temp_string += "<td align='right'><font color='red'>" & FormatNumber(total_avg_sale, 0) & "k&nbsp;</font></td>"
        End If

        If total_high = 0 Then
          temp_string += "<td align='right'>N/A&nbsp;</td>"
        Else
          temp_string += "<td align='right'>" & FormatNumber(total_high, 0) & "k&nbsp;</td>"
        End If



        temp_string += "</tr>"

        marketLabel.Text += temp_string
        marketLabel.Text += "</table>"

      Else
        ''error logging here. 
        'Master.LogError("home.aspx.vb - GetMarketListing() - " & masterPage.aclsData_Temp.class_error)
        'clear error for data layer class
        masterPage.aclsData_Temp.class_error = ""
      End If
    End If
  End Sub

  Public Sub Recent_SalesSumamry(ByRef marketLabel As Label, ByRef crmView As Boolean, ByRef cssClass As String, ByRef ResultsTable As DataTable, ByRef temp_string As String)
    Dim acObject As New marketSummaryObjAircraft()
    Dim mktSummaryFunctions As New marketSummaryFunctions()

    mktSummaryFunctions.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
    mktSummaryFunctions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
    mktSummaryFunctions.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
    mktSummaryFunctions.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
    mktSummaryFunctions.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

    Dim sRefLink As String = ""
    Dim sRefTitle As String = ""
    Dim tmpTransLink As String = ""
    Dim tmpAcDetails As String = ""
    Dim nSaleCount As Integer = 0
    Dim nSalePriceCount As Integer = 0
    Dim nSalePriceDisplayCount As Integer = 0
    Dim total_sale_count As Long = 0
    Dim total_sales As Long = 0
    Dim total_sales_w_price As Long = 0
    Dim total_displayable_sales_w_price As Long = 0
    Dim total_percent As Double = 0.0
    Dim total_low_ask As Long = 99000000
    Dim total_high_ask As Long = 0
    Dim total_low As Long = 99000000
    Dim total_high As Long = 0
    Dim total_avg As Long = 0
    Dim total_avg_sale As Long = 0


    ' ADDED IN MSW - 8-8-16 ---- SPI SECTION-----------------------------------
    cssClass = ""
    ResultsTable = masterPage.aclsData_Temp.HomePageGetMarketSummaryListing_RECENT(Session.Item("localUser").crmSelectedModels, number_of_months_divide)
    If Not IsNothing(ResultsTable) Then
      If ResultsTable.Rows.Count > 0 Then

        marketLabel.Text += "<table width=""100%"" cellpadding=""3"" cellspacing=""0"" class=""data_aircraft_grid"">"

        marketLabel.Text += "<tr class=""header_row"">"
        marketLabel.Text += "<td align=""center"" valign=""top"" colspan='11'>"
        marketLabel.Text += "<b class=""title"">RECENT RETAIL (PRE-OWNED) SALES – LAST " & number_of_months_divide & " MONTHS - WITHOUT SALE PRICES</b>"
        marketLabel.Text += "</td>"
        marketLabel.Text += "</tr>"

        marketLabel.Text += "<tr class=""header_row"">"

        marketLabel.Text += "<td align=""left"" valign=""top""><b class=""title"">MAKE/MODEL</b></td>"
        marketLabel.Text += "<td align=""left"" valign=""top""><b class=""title"">DATE</b></td>"
        marketLabel.Text += "<td align=""left"" valign=""top""><b class=""title"">TRANSACTION INFO</b></td>"
        marketLabel.Text += "<td align=""left"" valign=""top""><b class=""title"">ASKING</b></td>"
        marketLabel.Text += "<td align=""left"" valign=""top"" nowrap='nowrap'><b class=""title"">SALE PRICE</b></td>"

        marketLabel.Text += "</tr>"

        temp_string = ""
        For Each r As DataRow In ResultsTable.Rows

          temp_string += "<tr class='" & cssClass & "'>"
          temp_string += "<td align=""left"" valign=""middle"" nowrap='nowrap'>"
          temp_string += "" & r("amod_make_name").ToString & " "
          temp_string += DisplayFunctions.WriteModelLink(r("amod_id"), r("amod_model_name"), True) & " "
          temp_string += "<a href='DisplayAircraftDetail.aspx?acid=" & r("ac_id") & "&jid=" & r("journ_id") & "' target='_blank'>" & r("ac_ser_no_full") & "</a>"
          temp_string += "&nbsp;</td>"

          If Not IsDBNull(r("journ_date")) Then
            temp_string += "<td align='right'>" & r("journ_date") & "&nbsp;&nbsp;</td>"
          Else
            temp_string += "<td align='right'>&nbsp;&nbsp;</td>"
          End If

          If Not IsDBNull(r("journ_subject")) Then
            temp_string += "<td align='left'>" & r("journ_subject") & "&nbsp;</td>"
          Else
            temp_string += "<td align='left'>&nbsp;</td>"
          End If

          If Not IsDBNull(r("ac_asking")) Then
            If r.Item("ac_asking").ToString.ToLower.Trim.Trim.Contains("price") Then
              temp_string += "<td align='right'>$" & FormatNumber((r("ac_asking_price") / 1000), 0) & "k&nbsp;&nbsp;</td>"
            Else
              temp_string += "<td align='right'>M/O&nbsp&nbsp;</td>"
            End If
          Else
            temp_string += "<td align='right'>OFFMKT&nbsp;&nbsp;</td>"
          End If

          temp_string += "<td align='left'>"
          temp_string += "<a href='#' onclick=""javascript:window.open('SendSalesTransaction.aspx?sendSales=true&ModelID=" & r("amod_id").ToString & "&jID=" & r("journ_id").ToString & "&acid=" & r("ac_id").ToString & "','','scrollbars=yes,menubar=no,height=438,width=800,resizable=yes,toolbar=no,location=no,status=no');"" class='gray_text'>ENTER</a>"
          ' temp_string += "<a href='#' onclick=""javascript:window.open('\',\'\',\'scrollbars=yes,menubar=no,height=438,width=800,resizable=yes,toolbar=no,location=no,status=no\');return false;\"" class=\'gray_text\'>ENTER</a>"
          temp_string += "&nbsp;</td>"

          temp_string += "</tr>"
          If cssClass = "" Then
            cssClass = "alt_row"
          Else
            cssClass = ""
          End If
        Next

        marketLabel.Text += temp_string
        marketLabel.Text += "</table>"

      Else
        ''error logging here. 
        'Master.LogError("home.aspx.vb - GetMarketListing() - " & masterPage.aclsData_Temp.class_error)
        'clear error for data layer class
        masterPage.aclsData_Temp.class_error = ""
      End If
    End If
  End Sub

  Private Sub GetFleetSummaryListing(ByVal MarketLabel As Label, ByVal CRMView As Boolean)

    Dim cssClass As String = ""
    Dim ResultsTable As New DataTable


    ResultsTable = masterPage.aclsData_Temp.HomePageGetFleetSummaryListing(Session.Item("localUser").crmSelectedModels)
    If Not IsNothing(ResultsTable) Then
      If ResultsTable.Rows.Count > 0 Then
        MarketLabel.Text = "<table width=""100%"" cellpadding=""3"" cellspacing=""0"" class=""data_aircraft_grid""><tr class=""header_row"">"
        MarketLabel.Text += "<td align=""left"" valign=""top"">"
        MarketLabel.Text += "<b class=""title"">Make</b>"
        MarketLabel.Text += "</td>"
        MarketLabel.Text += "<td align=""right"" valign=""top"">"
        MarketLabel.Text += "<b class=""title"">in production</b>"
        MarketLabel.Text += "</td>"
        MarketLabel.Text += "<td align=""right"" valign=""top"">"
        MarketLabel.Text += "<b class=""title"">at mfr</b>"
        MarketLabel.Text += " </td>"
        MarketLabel.Text += "<td align=""right"" valign=""top"">"
        MarketLabel.Text += " <b class=""title"">in operation</b>"
        MarketLabel.Text += "</td>"
        MarketLabel.Text += "<td align=""right"" valign=""top"">"
        MarketLabel.Text += "<b class=""title"">stored</b>"
        MarketLabel.Text += " </td>"
        MarketLabel.Text += " <td align=""right"" valign=""top"">"
        MarketLabel.Text += "<b class=""title"">retired</b>"
        MarketLabel.Text += "</td>"
        MarketLabel.Text += "</tr>"

        For Each r As DataRow In ResultsTable.Rows
          '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

          MarketLabel.Text += "<tr class='" & cssClass & "'>"
          '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          ''''''''''''''''''''''''MAKE'''''''''''''''''''''''''''''''''''''''''''
          MarketLabel.Text += "<td align=""left"" valign=""top"">"
          MarketLabel.Text += "" & r("amod_make_name").ToString & " "
          MarketLabel.Text += DisplayFunctions.WriteModelLink(r("amod_id"), r("amod_model_name"), True) ' "<a href=""#"" onclick=""javascript:load('DisplayModelDetail.aspx?id=" & r("amod_id").ToString & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;"">" & r("amod_model_name").ToString & "</a>"
          MarketLabel.Text += "</td>"

          '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          '''''''''''''''''''''''''IN PRODUCTION'''''''''''''''''''''''''''''''''
          MarketLabel.Text += "<td align=""right"" valign=""top"">"
          MarketLabel.Text += "<a href='#' onclick=""javascript:document.body.style.cursor = 'wait';SubmitForm('" & r("amod_id") & "','1','N','N','" & r("amod_type_code").ToString & "|" & r("amod_airframe_type_code").ToString & "','" & r("amod_make_name") & "');"">" & r("inprodcount").ToString & "</a>"
          MarketLabel.Text += "</a></td>"
          ''''''''''''''''''''''''AT MFR ''''''''''''''''''''''''''''''''''''''''
          MarketLabel.Text += "<td align=""right"" valign=""top"">"
          MarketLabel.Text += "<a href='#' onclick=""javascript:document.body.style.cursor = 'wait';SubmitForm('" & r("amod_id") & "','2','N','N','" & r("amod_type_code").ToString & "|" & r("amod_airframe_type_code").ToString & "','" & r("amod_make_name") & "');"">" & r("mfrcount").ToString & "</a>"
          MarketLabel.Text += "</td>"
          '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          ''''''''''''''''''''''''IN OP COUNT''''''''''''''''''''''''''''''''''''
          MarketLabel.Text += "<td align=""right"" valign=""top"">"
          MarketLabel.Text += "<a href='#' onclick=""javascript:document.body.style.cursor = 'wait';SubmitForm('" & r("amod_id") & "','3','N','N','" & r("amod_type_code").ToString & "|" & r("amod_airframe_type_code").ToString & "','" & r("amod_make_name") & "');"">" & r("inopcount").ToString & "</a>"
          MarketLabel.Text += "</td>"
          ''''''''''''''''''''''''STORED'''''''''''''''''''''''''''''''''''''''''
          MarketLabel.Text += "<td align=""right"" valign=""top"">"
          MarketLabel.Text += "<a href='#' onclick=""javascript:document.body.style.cursor = 'wait';SubmitForm('" & r("amod_id") & "','5','N','N','" & r("amod_type_code").ToString & "|" & r("amod_airframe_type_code").ToString & "','" & r("amod_make_name") & "');"">" & r("storedcount").ToString & "</a>"
          MarketLabel.Text += "</td>"
          '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          '''''''''''''''''''''''''RETIRED'''''''''''''''''''''''''''''''''''''''
          MarketLabel.Text += "<td align=""right"" valign=""top"">"
          MarketLabel.Text += "<a href='#' onclick=""javascript:document.body.style.cursor = 'wait';SubmitForm('" & r("amod_id") & "','4','N','N','" & r("amod_type_code").ToString & "|" & r("amod_airframe_type_code").ToString & "','" & r("amod_make_name") & "');"">" & r("retiredcount").ToString & "</a>"
          MarketLabel.Text += "</td>"

          MarketLabel.Text += "</tr>"
          If cssClass = "" Then
            cssClass = "alt_row"
          Else
            cssClass = ""
          End If
        Next

        MarketLabel.Text += "</table>"
      Else
        MarketLabel.CssClass = "padding"
        If CRMView = True Then
          MarketLabel.Text = "<p align='left'>Welcome " & Session.Item("localUser").crmLocalUserFirstName.ToString & " " & Session.Item("localUser").crmLocalUserLastName.ToString & ".<br />To customize the default  """ & market_summary_tab.HeaderText.ToString & """, ""Events"" and ""Wanted"" tabs, <a href='#' onclick=""javascript:window.open('myCRM.aspx','','scrollbars=yes,menubar=no,height=800,width=1150,resizable=yes,toolbar=no,location=no,status=no');"">select your preferred models using this link</a>."
        Else
          MarketLabel.Text = "<p align='left'>Welcome " & Session.Item("localUser").crmLocalUserFirstName.ToString & " " & Session.Item("localUser").crmLocalUserLastName.ToString & ".<br />To customize the default  """ & market_summary_tab.HeaderText.ToString & """, ""Events"" and ""Wanted"" tabs, <a href='#' onclick=""javascript:window.open('Preferences.aspx?activetab=2','','scrollbars=yes,menubar=no,height=800,width=1150,resizable=yes,toolbar=no,location=no,status=no');"">select your preferred models using this link</a>."
        End If

      End If

    Else
      ''error logging here. 
      Master.LogError("home.aspx.vb - GetFleetSummaryListing() - " & masterPage.aclsData_Temp.class_error)
      'clear error for data layer class
      masterPage.aclsData_Temp.class_error = ""
    End If
  End Sub

  Private Sub home_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
    If Session.Item("localUser").crmEvo = True Then
      If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
        Me.MasterPageFile = "~/EvoStyles/EvoTheme.master"

        'Changed on 7/10/2015. This is going to check for the isMobile session variable
        'If you're in the evolution app and swap your masterpage accordingly.
        If Session.Item("isMobile") = True Then
          Me.MasterPageFile = "~/EvoStyles/MobileTheme.master"
        End If

      ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
        Me.MasterPageFile = "~/EvoStyles/YachtTheme.master"
      ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
        Me.MasterPageFile = "~/EvoStyles/CustomerAdminTheme.master"
      End If
    End If
  End Sub

  Public Sub change_hidden_folder(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles show_hidden_folders.CheckedChanged

    If sender.id.ToString = "show_hidden_folders" Or sender.id.ToString = "hide_shared" Then
      GetFolderListing(aircraft_projects, company_projects, contact_projects, history_projects, event_projects, wanted_projects, Nothing, performance_specs_projects, operating_costs_projects, marketing_summary_projects, Nothing, Nothing, airport_projects, show_hidden_folders, values_projects, hide_shared)
      folder_update_panel.Update()
    Else
      GetFolderListing(Nothing, yacht_company_projects, yacht_contact_projects, Nothing, Nothing, Nothing, yacht_projects, Nothing, Nothing, Nothing, yacht_history_projects, yacht_event_projects, Nothing, yacht_hidden_folders, Nothing, Nothing)
      yacht_folder_update.Update()
    End If
  End Sub

  Private Sub crm_calendar_timeframe_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles crm_calendar_timeframe.SelectedIndexChanged
    crm_tab_ActiveTabChanged(crm_tab, System.EventArgs.Empty)
  End Sub

  Private Sub crm_tab_ActiveTabChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles crm_tab.ActiveTabChanged
    'Go through each one of the tabs.
    Select Case crm_tab.ActiveTab.ID.ToString
      Case "crm_action_panel"
        Dim DateString As String = FormatDateTime(DateAdd(DateInterval.Day, 8, Now()), 2)
        'Action Item Panel.
        'Going and filling out the action items table.

        Select Case crm_calendar_timeframe.SelectedValue
          Case "1"
            DateString = FormatDateTime(DateAdd(DateInterval.Day, 2, Now()), 2)
          Case "31"
            DateString = FormatDateTime(DateAdd(DateInterval.Day, 32, Now()), 2)
          Case Else
            DateString = FormatDateTime(DateAdd(DateInterval.Day, 8, Now()), 2)
        End Select

        clsGeneral.clsGeneral.Upcoming_ActionItems(main_calendar_txt, Nothing, masterPage, DateString)
        'don't show, nothing here by default.
        main_calendar_txt.Visible = True
        today_date.Visible = True
      Case "crm_market_overview_panel"
        If Session.Item("localSubscription").crmAerodexFlag = True Then
          GetFleetSummaryListing(crm_market_overview, True)
        Else
          masterPage.aclsData_Temp.GetMarketListing(crm_market_overview, True, number_of_months_divide, market_summary_tab, toggleSalesEvo, toggleSales)
        End If

      Case "crm_event_panel"
        'events panel
        'Filling up the CRM Events.
        If crm_event_listing.Text = "" Then
          GetEventsListing(crm_event_listing, True, crm_event_time, crm_event_category)
        End If
      Case "crm_wanteds_panel"
        'wanted panel
        'Filling up the Wanted Events.
        If crm_wanted_label.Text = "" Then
          Display_Wanteds(crm_wanted_label, True)
        End If
      Case "crm_user_activity_panel"
        'user activity panel
        If Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR Then
          FillUserActivityPanel()
        End If
      Case "crm_client_db_panel"
        'user client db panel
        If Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR Then
          FillClientDBTotals()
        End If
    End Select

    crm_update_panel.Update()
  End Sub

  Private Sub main_home_tab_container_ActiveTabChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles main_home_tab_container.ActiveTabChanged

    Dim temp_refresh As String = ""


    If main_home_tab_container.ActiveTab.ID.ToString = "market_summary_tab" Then
      temp_refresh = HttpContext.Current.Session.Item("localPreferences").DefaultAnalysisMonths
      If Trim(Session.Item("localUser").crmSelectedModels) <> "" Then
        temp_refresh = temp_refresh & "_" & Session.Item("localUser").crmSelectedModels()
      End If

      If market_listing_label.Text = "" Or (Trim(temp_refresh) <> Trim(Session("Last_Market_Refresh"))) Then

        If (Trim(temp_refresh) <> Trim(Session("Last_Market_Refresh"))) Then
          market_load.Visible = True
        End If
        If Session.Item("localSubscription").crmAerodexFlag = True Or Session.Item("BusinessSegment") = "FB" Then
          GetFleetSummaryListing(market_listing_label, False)
        Else
          masterPage.aclsData_Temp.GetMarketListing(market_listing_label, False, number_of_months_divide, market_summary_tab, toggleSalesEvo, toggleSales)
        End If
        'market_listing_update_panel.Update()
      End If

      Session("Last_Market_Refresh") = temp_refresh
      market_load.Visible = False
    ElseIf main_home_tab_container.ActiveTab.ID.ToString = "market_activity_tab" Then
      event_time_panel.CssClass = "display_block light_seafoam_green_header_color toggleSmallScreen"
      events_load.CssClass = "display_none"
      event_listing_label.CssClass = "display_block"

      If event_listing_label.Text = "" Then
        GetEventsListing(event_listing_label, False, event_time, event_category)
        'event_listing_update_panel.Update()
      End If
      event_time.Attributes.Add("onClick", "ToggleEventLoadOnTimeChange()")
      event_category.Attributes.Add("onClick", "ToggleEventLoadOnTimeChange()")

    ElseIf main_home_tab_container.ActiveTab.ID.ToString = "wanted_tab" Then
      If wanted_listing_label.Text = "" Then
        Display_Wanteds(wanted_listing_label, False)
      End If
      wanteds_load.Visible = False
    ElseIf main_home_tab_container.ActiveTab.ID.ToString = "MyAnalytics" Then
      'If MyAnalytics_listing_label.Text = "" Then
      CreateAnalytics()
      'myactions_update_panel.Update()
      'End If
      analytics_load.Visible = False
    ElseIf main_home_tab_container.ActiveTab.ID.ToString = "action_item_tab" Then
      If evo_action_items.Text = "" Then
        Create_Evo_Action_Items(evo_action_items)
      End If
      actions_load.Visible = False

    ElseIf main_home_tab_container.ActiveTab.ID.ToString = "quick_search_tab" Then
      ' Session.Item("localSubscription").crmAerodexFlag = True

      If Session.Item("localSubscription").crmAerodexFlag = True Then
        market_status_label.Visible = False
        ___market.Visible = False
      End If


      System.Web.UI.ScriptManager.RegisterStartupScript(Me.main_home_update_panel, Me.GetType(), "testTab", AttributeDynamicJquery.ToString & vbCrLf & "SetUpVariableAutoComplete('#" & ModelDynamic.ClientID & "', '#" & searchAircraft.ClientID & "', '#" & searchCompany.ClientID & "','" & ___amod_id.ClientID & "');", True)

    ElseIf main_home_tab_container.ActiveTab.ID.ToString = "reports_tab" Then
      'this is already loaded when the page is loaded. It's preloaded because
      'The query answer depends on whether they have the tab or not.
    ElseIf main_home_tab_container.ActiveTab.ID.ToString = "folder_events_tab" Then


      Me.folder_events_tab_text.Visible = True

      '  event_time_panel.CssClass = "display_block light_seafoam_green_header_color toggleSmallScreen"
      'events_folder_load.CssClass = "display_none"
      folder_events_tab_text.CssClass = "display_block"

      GetFolderEventsListing(folder_events_tab_text, False, event_time, event_category)

      ' event_time.Attributes.Add("onClick", "ToggleEventLoadOnTimeChange()")
      ' event_category.Attributes.Add("onClick", "ToggleEventLoadOnTimeChange()")
      ' GetEventsListing(event_listing_label, False, event_time, event_category)
      Me.event_folder_div.Visible = False
      Me.folder_events_tab_text.CssClass = "display_block"
      ' event_time_panel.CssClass = "display_block light_seafoam_green_header_color toggleSmallScreen"
      ' main_home_update_panel.Update()
    ElseIf main_home_tab_container.ActiveTab.ID.ToString = "airport_tab" Then
      If String.IsNullOrEmpty(airportsTable.Text.Trim) Then
        Display_Airports(airportsTable)
      Else
        System.Web.UI.ScriptManager.RegisterStartupScript(Me.main_home_update_panel, Me.main_home_update_panel.GetType(), "CreateAirportsTable", "$(document).ready(function() { CreateTheDatatable('aPortInnerTable','aPortDataTable','aPortjQueryTable'); });", True)
      End If

    ElseIf main_home_tab_container.ActiveTab.ID.ToString = "index_tab" Then
      If index_tab_label.Text = "" Then
        AdvancedQueryResults.LoopThroughHomeIndexTabToTurnOnCheckboxes(False, False, True, masterPage.aclsData_Temp, "AREA", index_tab)
        index_tab_label.Text = Now.ToString
      End If
      index_wait_div.Visible = False
      indexPanel.CssClass = ""
    ElseIf main_home_tab_container.ActiveTab.ID.ToString = "my_mpm_tabpanel" Then
      Call create_client_aircraft_summary()
      Me.mympm_div.Visible = False
    End If
    main_home_update_panel.Update()

    '02/12/2016
    'can you drop in a Subscription_Install_Log record for each time users click on a home page tab?
    'msg_type = "UserClickHome"
    'Message = "Tab [tab name]"

    If Page.IsPostBack Then 'This means called only when clicked.
      Call commonLogFunctions.Log_User_Event_Data("UserClickHome", "Tab: " & main_home_tab_container.ActiveTab.HeaderText, Nothing, 0, 0, 0, 0, 0, 0, 0, 0, "")
    End If


  End Sub

  Private Sub Display_Airports(ByVal airportsTable As Label)

    Me.MyAirports_Load.Visible = True

    Dim searchCriteria_test As New viewSelectionCriteriaClass


    Dim airport_fbo_functions As New airport_fbo_view_functions
    airport_fbo_functions.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
    airport_fbo_functions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
    airport_fbo_functions.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
    airport_fbo_functions.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
    airport_fbo_functions.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

    months_choice.Visible = True
    my_airports_label.Visible = True
    my_airports_label.Text = "Flight Arrivals Based On"

    'Replacing the function call to fill this up with the subscription FAA last date. 
    Session("Last_FAA_DATE") = Session.Item("localSubscription").crmSubinst_FAA_data_date

    Dim defaultAirportString As String = ""
    Dim defaultAirportFolderID As Long = 0
    Dim results_table As New DataTable

    results_table = commonEvo.returnAirportFolderName(True, False, 17)

    If Not IsNothing(results_table) Then
      If results_table.Rows.Count > 0 Then

        For Each r As DataRow In results_table.Rows

          If Not (IsDBNull(r.Item("cfolder_id"))) Then
            If IsNumeric(r.Item("cfolder_id").ToString) Then
              defaultAirportFolderID = CLng(r.Item("cfolder_id").ToString)
            End If
          End If

        Next

      End If

    End If

    If Not IsNothing(results_table) Then
      results_table.Clear()
    End If
    results_table = commonEvo.returnAirportFolderContents(defaultAirportFolderID)

    If Not IsNothing(results_table) Then
      If results_table.Rows.Count > 0 Then

        For Each r As DataRow In results_table.Rows

          If Not (IsDBNull(r.Item("aport_id"))) Then
            If IsNumeric(r.Item("aport_id").ToString) Then

              If String.IsNullOrEmpty(defaultAirportString.Trim) Then
                defaultAirportString = r.Item("aport_id").ToString
              Else
                defaultAirportString += Constants.cCommaDelim + r.Item("aport_id").ToString
              End If

            End If
          End If

        Next

      End If

    End If

    Dim sAirportHTMLtable As String = ""

    If Not String.IsNullOrEmpty(defaultAirportString.Trim) Then
      Call airport_fbo_functions.GET_USER_AIRPORTS_top_function(defaultAirportString, sAirportHTMLtable, Nothing, "view", Me.months_choice.SelectedValue, Session("Last_FAA_DATE"), True)
    End If

    If Not String.IsNullOrEmpty(sAirportHTMLtable.Trim) Then
      my_airports_label.Visible = False
      ' months_choice.Visible = False
    End If

    ' add the 'click here' header on it either way 

    If defaultAirportFolderID > 0 Then
      airportsTable.Text = "To modify your airport list below click <a href=""staticFolderEditor.aspx?folderID=" + defaultAirportFolderID.ToString.Trim + "&airport=true&default=true&fromHome=true"" target=""_blank"">HERE</a>"
    End If

    airportsTable.Text += "<br/><br/>"
    airportsTable.Text += sAirportHTMLtable.Trim

    System.Web.UI.ScriptManager.RegisterStartupScript(Me.main_home_update_panel, Me.main_home_update_panel.GetType(), "CreateAirportsTable", "$(document).ready(function() { CreateTheDatatable('aPortInnerTable','aPortDataTable','aPortjQueryTable'); });", True)

    MyAirports_Load.Visible = False

  End Sub

  Private Sub Display_Wanteds(ByVal wantedLabel As Label, ByVal CRM As Boolean)
    Dim cssClass As String = ""
    Dim Results_Table As New DataTable
    Dim StartDate As String = Month(DateAdd(DateInterval.Day, -90, Now())) & "/" & Day(DateAdd(DateInterval.Day, -90, Now())) & "/" & Year(DateAdd(DateInterval.Day, -90, Now()))
    Dim temp_string As String = ""

    'Only run this if there are selected models.
    If Session.Item("localUser").crmSelectedModels <> "" Then
      If CRM = True Then
        StartDate = Year(DateAdd(DateInterval.Day, -90, Now())) & "-" & Month(DateAdd(DateInterval.Day, -90, Now())) & "-" & Day(DateAdd(DateInterval.Day, -90, Now()))
        Results_Table = masterPage.aclsData_Temp.Return_Wanted(0, "", 0, Session.Item("localUser").crmSelectedModels, StartDate, "", "", "JC", 0)
      Else
        Results_Table = masterPage.aclsData_Temp.Return_Wanted_Evo(0, "", 0, Session.Item("localUser").crmSelectedModels, StartDate, "", "", "J", 0, "", "", "", "", "", "", Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmCommercial_Flag, Session.Item("localSubscription").crmHelicopter_Flag)
      End If
    End If
    If Not IsNothing(Results_Table) Then
      If Results_Table.Rows.Count > 0 Then

        wantedLabel.Text = "<table width=""100%"" cellpadding=""3"" cellspacing=""0""  class=""" & IIf(CRM, "data_aircraft_grid", "data_aircraft_grid") & """><tr class=""" & IIf(CRM, "header_row", "header_row") & """>"
        If CRM Then
          wantedLabel.Text += "<td align=""right"" valign=""top"" width='5'></td>"
        End If

        'These are mobile display only rows:
        wantedLabel.Text += "<td align=""left"" valign=""top"" width='215' class=""mobile_display_on_cell"">"
        wantedLabel.Text += "<b class=""title"">Make</b><br /><b class=""title"">Interested Party</b>"
        wantedLabel.Text += "</td>"
        wantedLabel.Text += "<td align=""left"" valign=""top"" width='215' class=""mobile_display_on_cell"">"
        wantedLabel.Text += " <b class=""title"">Year Range</b><br /><b class=""title"">Max Price</b>"
        wantedLabel.Text += "</td>"
        'End only mobile display rows.

        wantedLabel.Text += "<td align=""left"" valign=""top"" width='130' class=""mobile_display_off_cell""><b class=""title"">Make</b></td>"
        wantedLabel.Text += "<td align=""right"" valign=""top"" width='70'><b class=""title"">Date Listed</b></td>"
        If CRM = False Then
          wantedLabel.Text += "<td align=""right"" valign=""top"" width='5'></td>"
        End If
        wantedLabel.Text += "<td align=""left"" valign=""top"" width='215' class=""mobile_display_off_cell""> <b class=""title"">Interested Party</b></td>"
        wantedLabel.Text += "<td align=""left"" valign=""top"" width='70' class=""mobile_display_off_cell""><b class=""title"">Year Range</b> </td>"
        wantedLabel.Text += " <td align=""left"" valign=""top"" width='60' class=""mobile_display_off_cell""><b class=""title"">Max Price</b><br /></td>"
        wantedLabel.Text += "<td align=""left"" valign=""top"" width='50'><b class=""title"">Max AFTT</b></td>"
        wantedLabel.Text += "<td align=""left"" valign=""top""><b class=""title"">Damage</b></td>"
        wantedLabel.Text += "</tr>"

        For Each r As DataRow In Results_Table.Rows
          temp_string += "<tr class='" & cssClass & "'>"
          If CRM Then
            temp_string += "<td align=""left"" valign=""middle"" width='5'>" & IIf(r("source").ToString = "JETNET", "<img src='images/evo.png' alt='Jetnet Record' width='15' />", "<img src='images/client.png' alt='Client Record' width='15' />") & "</td>"
          End If

          'These are mobile display only rows:
          temp_string += "<td align=""left"" valign=""top"" width='215' class=""mobile_display_on_cell"">"

          If CRM = False Then
            temp_string += r("amod_make_name").ToString & " "
            temp_string += DisplayFunctions.WriteModelLink(r("amwant_amod_id"), "" + r("amod_model_name"), True)
          Else

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ''''''''''''''''''''''''MAKE NAME''''''''''''''''''''''''''''''''''''''
            temp_string += "" & r("amod_make_name").ToString & " "
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ''''''''''''''''''''''''MODEL NAME'''''''''''''''''''''''''''''''''''''
            temp_string += r("amod_model_name")
          End If

          temp_string += "<br /><br />"
          If CRM = False Then
            temp_string += crmWebClient.DisplayFunctions.WriteDetailsLink(0, r("comp_id"), 0, 0, True, r("comp_name").ToString, "", "")
          Else
            temp_string += r("comp_name").ToString
          End If

          temp_string += "</td><td align=""left"" valign=""top"" width='215' class=""mobile_display_on_cell"">"
          '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          ''''''''''''''''''''''''Year Range''''''''''''''''''''''''''''''''''''
          temp_string += r("amwant_start_year").ToString & "-" & r("amwant_end_year").ToString & "<br /><br />"
          temp_string += crmWebClient.clsGeneral.clsGeneral.no_zero(r("amwant_max_price").ToString, "", True)

          temp_string += "</td>"
          'End only mobile display rows.

          temp_string += "<td align=""left"" valign=""top"" class=""mobile_display_off_cell"">"
          If CRM = False Then
            temp_string += r("amod_make_name").ToString & " "
            temp_string += DisplayFunctions.WriteModelLink(r("amwant_amod_id"), "" + r("amod_model_name"), True)
          Else

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ''''''''''''''''''''''''MAKE NAME''''''''''''''''''''''''''''''''''''''
            temp_string += "" & r("amod_make_name").ToString & " "
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ''''''''''''''''''''''''MODEL NAME'''''''''''''''''''''''''''''''''''''
            temp_string += r("amod_model_name")
          End If


          temp_string += "</td><td align=""right"" valign=""top"">"
          '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          ''''''''''''''''''''''''Date Listed''''''''''''''''''''''''''''''''''''''''
          If CRM = False Then
            temp_string += "<a href=""#"" onclick=""javascript:load('WantedDetails.aspx?id=" & r("amwant_id").ToString & "','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;"">"
          End If
          temp_string += clsGeneral.clsGeneral.datenull(r("amwant_listed_date").ToString)
          If CRM = False Then
            temp_string += "</a>"
          End If
          temp_string += "</td>"
          If CRM = False Then
            temp_string += "<td align=""right"" valign=""top""></td>"
          End If
          temp_string += "<td align=""left"" valign=""top"" class=""mobile_display_off_cell"">"
          '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          ''''''''''''''''''''''''Interested Party'''''''''''''''''''''''''''''''''''''
          If CRM = False Then
            temp_string += crmWebClient.DisplayFunctions.WriteDetailsLink(0, r("comp_id"), 0, 0, True, r("comp_name").ToString, "", "")
          Else
            temp_string += r("comp_name").ToString
          End If
          temp_string += "</td><td align=""left"" valign=""top"" class=""mobile_display_off_cell"">"
          '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          ''''''''''''''''''''''''Year Range''''''''''''''''''''''''''''''''''''
          temp_string += r("amwant_start_year").ToString & "-" & r("amwant_end_year").ToString
          temp_string += "</td>"
          '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          ''''''''''''''''''''''''max price'''''''''''''''''''''''''''''''''''''
          temp_string += "<td align=""left"" valign=""top"" class=""mobile_display_off_cell"">"
          temp_string += crmWebClient.clsGeneral.clsGeneral.no_zero(r("amwant_max_price").ToString, "", True)
          temp_string += "</td><td align=""left"" valign=""top"">"
          '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          ''''''''''''''''''''''''max aftt''''''''''''''''''''''''''''''''''''''''
          temp_string += r("amwant_max_aftt").ToString & " "
          temp_string += "</td>"
          '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          '''''''''''''''''''''''''''DAMAGE''''''''''''''''''''''''''''''''''''''
          temp_string += "<td align=""left"" valign=""top"">"

          temp_string += "</td></tr>"

          If cssClass = "" Then
            cssClass = "alt_row"
          Else
            cssClass = ""
          End If


          wantedLabel.Text += temp_string
          temp_string = ""
        Next

        wantedLabel.Text += "</table>"
      Else
        If Session.Item("localUser").crmSelectedModels <> "" Then
          wantedLabel.CssClass = "padding" ' emphasis_text"
          wantedLabel.Text = "<br /><p align='center'>There are applicable listings with these parameters.</p>"
        Else
          wantedLabel.CssClass = "padding" ' emphasis_text"
          wantedLabel.Text = "<p align='left'>Welcome " & Session.Item("localUser").crmLocalUserFirstName.ToString & " " & Session.Item("localUser").crmLocalUserLastName.ToString & ".<br />To customize the default """ & market_summary_tab.HeaderText.ToString & """, ""Events"" and ""Wanted"" tabs, <a href='#' onclick=""javascript:window.open('Preferences.aspx','','scrollbars=yes,menubar=no,height=800,width=1150,resizable=yes,toolbar=no,location=no,status=no');"">select your preferred models using this link</a>."

        End If
      End If
    Else

      wantedLabel.CssClass = "padding" ' emphasis_text"
      wantedLabel.Text = "<p align='left'>Welcome " & Session.Item("localUser").crmLocalUserFirstName.ToString & " " & Session.Item("localUser").crmLocalUserLastName.ToString & ".<br />To customize the default """ & market_summary_tab.HeaderText.ToString & """, ""Events"" and ""Wanted"" tabs, <a href='#' onclick=""javascript:window.open('Preferences.aspx','','scrollbars=yes,menubar=no,height=800,width=1150,resizable=yes,toolbar=no,location=no,status=no');"">select your preferred models using this link</a>."
    End If
    wanted_results.Visible = True

  End Sub

  Private Sub months_choice_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles months_choice.SelectedIndexChanged

    Display_Airports(airportsTable)
    main_home_update_panel.Update()

  End Sub

  Private Sub event_time_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles event_time.SelectedIndexChanged, event_category.SelectedIndexChanged, crm_event_category.SelectedIndexChanged, crm_event_time.SelectedIndexChanged



    If Session.Item("localUser").crmEvo = True Then 'If a CRM user

      GetEventsListing(event_listing_label, False, event_time, event_category)
      events_load.CssClass = "display_none"
      event_listing_label.CssClass = "display_block"
      event_time_panel.CssClass = "display_block light_seafoam_green_header_color toggleSmallScreen"
      main_home_update_panel.Update()

    Else
      GetEventsListing(crm_event_listing, True, crm_event_time, crm_event_category)
      crm_event_listing.CssClass = "display_block"
      crm_time_panel.CssClass = "display_block light_seafoam_green_header_color"
      crm_event_update_panel.Update()

    End If
  End Sub

  Private Sub QuickSearchClick(ByVal controlsToBeSearchedPanel As Panel, ByVal senderID As String, ByVal updatePanelToBeUpdated As UpdatePanel)

    Dim JavaScript As String = ""
    Dim searchCompanyList As Boolean = False
    Dim searchYachtList As Boolean = False

    ''7/23/2015
    ''We're going to add a small check to overwrite the sender ID. Basically if any of the company information is filled out
    ''We're searching there instead, but only on evolution application.
    '01/13/2016
    'This was commented out down below. I edited it to make it to where
    'if you typed in a contact first name, last name or email address it
    'would automatically search company/display contact information.
    If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
      If company_contact_first___contact_first_name.Text <> "" Then
        senderID = "searchCompany"
        JavaScript = "company_contact_info=true!~!"
      End If
      If company_contact_last___contact_last_name.Text <> "" Then
        senderID = "searchCompany"
        JavaScript = "company_contact_info=true!~!"
      End If
      'If company_name___comp_name.Text <> "" Then
      '  senderID = "searchCompany"
      'End If
      If company_email_address___comp_email_address.Text <> "" Then
        senderID = "searchCompany"
        JavaScript = "company_contact_info=true!~!"
      End If
    End If


    If senderID.ToString = "searchCompany" Or senderID.ToString = "searchYachtCompany" Then
      searchCompanyList = True
    ElseIf senderID.ToString = "searchYacht" Then
      searchYachtList = True
    ElseIf senderID.ToString = "searchAircraft" Then
      searchCompanyList = False
    End If

    For Each c As Control In controlsToBeSearchedPanel.Controls
      If Not IsNothing(c) Then
        Dim ActualID As String = ""
        Dim IDArray As Array
        'We split the ID. 0 is company, 1 is aircraft.
        IDArray = Split(c.ID, "___")
        If UBound(IDArray) = 1 Then
          If searchCompanyList Then
            ActualID = IDArray(0)
            'If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
            '    ActualID = Replace(ActualID, "yt_", "")
            'End If
          Else
            ActualID = IDArray(1)
          End If

          If TypeOf c Is TextBox Then
            Dim TempObject As TextBox = c
            If Not String.IsNullOrEmpty(TempObject.Text) Then
              If ActualID <> "" Then
                If JavaScript <> "" Then
                  JavaScript += "!~!"
                End If

                If ActualID = "amod_id" Or ActualID = "yt_model_id" Then
                  Dim ans As String = HttpUtility.UrlDecode(TempObject.Text)
                  Dim ModelMake As Array = Split(ans, "|")
                  If UBound(ModelMake) = 1 Then
                    JavaScript += IIf(searchYachtList = True, "ym_brand_name=", "amod_make_name=") & ModelMake(0)
                    JavaScript += "!~!"
                  End If
                  JavaScript += ActualID & "=" & ModelMake(1)
                ElseIf ActualID = "attributeID" Then
                  'here we go. This is for the quick search autocomplete details textbox.
                  'If they happen to pick an autocomplete variable (from the attribute list)
                  'Then we're going to basically build a custom string to check the boxes on the aircraft search.
                  'The two boxes reside on the attributes tab - one under alphabetic and one under area.
                  'Technically only the Attribute_(id goes here) box needs to be checked - but having both checked looks nicer.
                  JavaScript += "Attribute_" & TempObject.Text & "=true!~!" & "Ignore_" & TempObject.Text & "=true"

                ElseIf ActualID = "generic_data_description" Then
                  If ___attributeID.Text = "" Then
                    'I am adding this catch. If they picked an attribute from the list and the ID of it is filled in this textbox
                    'That means we no longer care what's in this box. The only way we do care is if they typed in something on their own
                    'and the box is blank.
                    JavaScript += ActualID & "=" & TempObject.Text
                  End If
                Else
                  JavaScript += ActualID & "=" & TempObject.Text
                End If

              End If
            End If
          ElseIf TypeOf c Is DropDownList Then
            Dim TempObject As DropDownList = c
            If Not String.IsNullOrEmpty(TempObject.SelectedValue) Then
              If ActualID <> "" Then
                If JavaScript <> "" Then
                  JavaScript += "!~!"
                End If

                JavaScript += ActualID & "=" & TempObject.SelectedValue
              End If
            End If
          ElseIf TypeOf c Is CheckBox Then
            Dim TempObject As CheckBox = c
            If (TempObject.Checked) Then
              If ActualID <> "" Then
                If JavaScript <> "" Then
                  JavaScript += "!~!"
                End If

                JavaScript += ActualID & "=" & TempObject.Checked.ToString.ToLower
              End If
            End If
          End If
        End If
      End If
    Next

    If JavaScript <> "" Then
      If searchCompanyList Then
        JavaScript = "javascript:ParseForm('0', false,false,true,false,false, '" & Replace(JavaScript, "'", "\'") & "');"
      ElseIf searchYachtList Then
        JavaScript = "javascript:ParseForm('0', false,false,false,false,true, '" & Replace(JavaScript, "'", "\'") & "');"
      Else
        JavaScript = "javascript:ParseForm('0', false,false,false,false,false, '" & Replace(JavaScript, "'", "\'") & "');"
      End If


      'If Session.Item("isMobile") Then
      '  JavaScript += "FigureMobileDisplayOut();document.body.style.cursor='wait';"
      'End If
      'JavaScript = "alert('" & Replace(JavaScript, "'", "\'") & "');"
      'Clear Saved Session.
      Session.Item("MasterAircraftWhere") = ""
      Session.Item("MasterAircraftFrom") = ""
      Session.Item("MasterAircraftSelect") = ""
      Session.Item("MasterAircraftSort") = ""
      clsGeneral.clsGeneral.ClearSavedSelection()

      System.Web.UI.ScriptManager.RegisterClientScriptBlock(updatePanelToBeUpdated, Me.GetType(), "parsingForm", "" & JavaScript & "", True)
      updatePanelToBeUpdated.Update()
    End If
  End Sub

  Public Sub convert_us_to_metric(ByVal us_value As Double, ByRef metric As Double)

    metric = CDbl(us_value * 0.3048)

  End Sub ' convert_metric_to_us1

  Private Sub YachtQuickSearchClickHandler(ByVal sender As Object, ByVal e As System.EventArgs) Handles searchYacht.Click, searchYachtCompany.Click
    QuickSearchClick(yacht_search_control_panel, sender.id, Me.yacht_update_panel)
  End Sub

  Private Sub AircraftQuickSearchHandler(ByVal sender As Object, ByVal e As System.EventArgs) Handles searchAircraft.Click, searchCompany.Click
    QuickSearchClick(searchControls, sender.id, Me.main_home_update_panel)
  End Sub

#Region "Yacht Home Page Queries"
  Private Sub GetYachtNewsListing()
    Dim cssClass As String = ""
    Dim ResultsTable As New DataTable
    Dim URL As String = ""
    Dim ShowLink As Boolean = True
    yacht_latest_news_label.Text = ""

    ResultsTable = masterPage.aclsData_Temp.ListOfYachtNews(0)

    If Not IsNothing(ResultsTable) Then
      If ResultsTable.Rows.Count > 0 Then
        yacht_latest_news_label.Text = "<table width=""100%"" cellpadding=""3"" cellspacing=""0"" class=""data_aircraft_grid""><tr class=""header_row"">"
        yacht_latest_news_label.Text += "<td align=""left"" valign=""top"" width='130'>"
        yacht_latest_news_label.Text += "<b class=""title"">Latest News</b>"
        yacht_latest_news_label.Text += "</td>"
        yacht_latest_news_label.Text += "</tr>"

        For Each r As DataRow In ResultsTable.Rows
          yacht_latest_news_label.Text += "<tr>"
          yacht_latest_news_label.Text += "<td align=""left"" valign=""top"" class=""light_seafoam_green_header_color"">"

          If InStr(r("ytnews_web_address").ToString, "http://") > 0 Then
            URL = r("ytnews_web_address").ToString()
          ElseIf InStr(r("ytnews_web_address").ToString, "https://") > 0 Then
            URL = r("ytnews_web_address").ToString()
          Else
            URL = "http://" & r("ytnews_web_address").ToString
          End If


          If InStr(URL, "><") > 0 Then
            URL = Left(URL, InStr(URL, "><") - 1)
          End If


          URL = Replace(URL, "https//", "https://")

          If Not IsDBNull(r("ytnews_web_address")) Then
            If String.IsNullOrEmpty(r("ytnews_web_address")) Then
              ShowLink = False
            End If
          End If

          '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          ''''''''''''''''''''''''Title''''''''''''''''''''''''''''''''''''''

          yacht_latest_news_label.Text += IIf(ShowLink, "<a href=""" & URL & """ target=""new"" class=""small_to_medium_text"">", "<span class=""small_to_medium_text""") & "<b>" & r("ytnews_title").ToString & "</b>" & IIf(ShowLink, "</a>", "</span>") & " - "
          '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          ''''''''''''''''''''''''Date'''''''''''''''''''''''''''''''''''''

          yacht_latest_news_label.Text += FormatDateTime(r("ytnews_date"), 2)

          yacht_latest_news_label.Text += "</td>"
          yacht_latest_news_label.Text += "</tr>"
          yacht_latest_news_label.Text += "<tr class='" & cssClass & "'>"
          yacht_latest_news_label.Text += "<td align=""left"" valign=""top"">"

          yacht_latest_news_label.Text += "<p>" & IIf(r("ytnews_description").ToString.Length > 200, Left(r("ytnews_description").ToString, 200) & "...", r("ytnews_description").ToString)

          If (Not IsDBNull(r("ytnewssrc_name"))) Then
            If Not String.IsNullOrEmpty(r("ytnewssrc_name")) Then
              yacht_latest_news_label.Text += "[<a href=""" & URL & """ target=""new""><em>" & r("ytnewssrc_name").ToString & "</em></a>]"
            End If
          End If

          yacht_latest_news_label.Text += "</p></td>"
          yacht_latest_news_label.Text += "</tr>"

          If cssClass = "" Then
            cssClass = "alt_row"
          Else
            cssClass = ""
          End If
        Next

        yacht_latest_news_label.Text += "</table>"
      Else
        yacht_latest_news_label.CssClass = "padding" ' emphasis_text"
        yacht_latest_news_label.Text = "<br /><p align='center'>There is no latest news.</p>"
      End If
    Else
      'error logging here.
      Master.LogError("home.aspx.vb - GetYachtNewsListing() - " & masterPage.aclsData_Temp.class_error)
      'clear error for data layer class
      masterPage.aclsData_Temp.class_error = ""
    End If
  End Sub
  Private Sub CreateYachtSummary()
    Dim YachtTable As New DataTable
    Dim localDatalayer As New yachtViewDataLayer

    localDatalayer.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
    'Create table for Yachts.
    yacht_summary_label.Text = "<table class=""data_aircraft_grid"" width=""100%"" cellpadding=""4"" cellspacing=""0"">"
    yacht_summary_label.Text += "<tr class=""light_seafoam_green_header_color"">"
    yacht_summary_label.Text += "<td align=""left"" valign=""top""><b>MOTOR YACHTS</b></td>"
    yacht_summary_label.Text += "<td align=""left"" valign=""top""><b>SAILING YACHTS</b></td>"
    yacht_summary_label.Text += "</tr>"
    yacht_summary_label.Text += "<tr>"

    'Motor Yacht Summary
    'Data Information for Motor Yachts
    YachtTable = New DataTable
    YachtTable = localDatalayer.get_yachts_by_type(New yachtViewSelectionCriteria, "M")
    yacht_summary_label.Text += "<td align=""left"" valign=""top"">" & YachtFunctions.DisplaySummaryByYachtType(YachtTable) & "</td>"


    'Sailing Yacht Summary
    'Data Information for Sailing Yachts
    YachtTable = New DataTable
    YachtTable = localDatalayer.get_yachts_by_type(New yachtViewSelectionCriteria, "S")
    yacht_summary_label.Text += "<td align=""left"" valign=""top"">" & YachtFunctions.DisplaySummaryByYachtType(YachtTable) & "</td>"

    yacht_summary_label.Text += "</tr>"
    yacht_summary_label.Text += "</table>"

  End Sub
#End Region

#Region "Index Tab for Evo/Yacht"
  'Public Sub Display_Index_Tab(ByVal CRM As Boolean)
  '  'If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Or HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Or HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
  '  Dim aclsData_Temp As New clsData_Manager_SQL
  '  Dim MainContent As New ContentPlaceHolder
  '  Dim Find As New LinkButton
  '  Dim Find_Top As New LinkButton
  '  Dim ProductType As String = "Aircraft"
  '  Dim TemporaryTable As New DataTable 'Holds the topics attributes so we can use them to fill the autocomplete dropdown
  '  Dim DynamicIndexPanel As Panel = indexPanel
  '  Dim DynamicIndexTab As AjaxControlToolkit.TabPanel = index_tab
  '  Dim DynamicTabContainer As AjaxControlToolkit.TabContainer = main_home_tab_container

  '  If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
  '    ProductType = "Yacht"
  '    DynamicIndexPanel = yachtIndexPanel
  '    DynamicIndexTab = yacht_index_tab
  '    DynamicTabContainer = yacht_summary_tab
  '  End If

  '  Find.Text = "Find " & ProductType
  '  Find.CssClass = "gray_button float_right margin_4"
  '  Find.OnClientClick = "document.body.style.cursor='wait';"
  '  AddHandler Find.Command, AddressOf Me.Find_Command

  '  Find_Top.Text = "Find " & ProductType
  '  Find_Top.CssClass = "gray_button float_right margin_4"
  '  Find_Top.OnClientClick = "document.body.style.cursor='wait';"
  '  AddHandler Find_Top.Command, AddressOf Me.Find_Command

  '  aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase")
  '  aclsData_Temp.client_DB = Session.Item("jetnetServerNotesDatabase")

  '  If Not IsNothing(Page.Master.FindControl("ContentPlaceHolder1")) Then
  '    MainContent = TryCast(Page.Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
  '  End If

  '  DynamicIndexTab.Visible = True

  '  Dim TemporaryContainer As New Panel
  '  AdvancedQueryResults.BuildAttributeTextAndDropdown(TemporaryContainer, MainContent.ClientID, DynamicTabContainer.ID, DynamicIndexTab.ID, Page, False)
  '  DynamicIndexPanel.Controls.AddAt(0, TemporaryContainer)

  '  DynamicIndexPanel.Controls.AddAt(1, Find_Top)

  '  TemporaryContainer = New Panel
  '  TemporaryContainer.ID = "letter_display"

  '  TemporaryTable = AdvancedQueryResults.BuildTopicAreaPanel(TemporaryContainer, True, False, "LETTER", "TOPIC", MainContent.ClientID, DynamicTabContainer.ID, DynamicIndexTab.ID, Page, False, aclsData_Temp)
  '  DynamicIndexPanel.Controls.AddAt(2, TemporaryContainer)

  '  TemporaryContainer = New Panel
  '  TemporaryContainer.ID = "area_display"
  '  TemporaryContainer.Attributes.Add("style", "display:none;")
  '  AdvancedQueryResults.BuildTopicAreaPanel(TemporaryContainer, False, True, "AREA", "TOPIC", MainContent.ClientID, DynamicTabContainer.ID, DynamicIndexTab.ID, Page, False, aclsData_Temp)
  '  DynamicIndexPanel.Controls.AddAt(3, TemporaryContainer)



  '  DynamicIndexPanel.Controls.AddAt(4, Find)


  '  'Filling up the attribute source
  '  If attributeAutoCompleteString = "" Then
  '    If Not IsNothing(TemporaryTable) Then
  '      If TemporaryTable.Rows.Count > 0 Then

  '        For Each r As DataRow In TemporaryTable.Rows
  '          If attributeAutoCompleteString <> "" Then
  '            attributeAutoCompleteString += ","
  '          End If
  '          attributeAutoCompleteString += "{label: """ & r("TOPIC").ToString & """, value: """ & r("TOPID").ToString & """}"
  '        Next

  '      End If
  '    End If
  '  End If
  '  ' End If
  'End Sub

  Protected Sub Find_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs)
    Dim searchPanel As New Panel
    Dim DynamicIndexTab As AjaxControlToolkit.TabPanel = index_tab
    Dim DynamicPanel As Panel = indexPanel

    If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
      DynamicIndexTab = yacht_index_tab
      DynamicPanel = indexPanel
    End If

    If Not IsNothing(indexPanel.FindControl("letter_display")) Then
      searchPanel = DynamicPanel.FindControl("letter_display")
    End If

    AttributeQuickSearchJavascriptBuilder(DynamicIndexTab.Controls)

    If AttributeJavascript <> "" Then

      If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
        AttributeJavascript = "javascript:ParseForm('0', false,false,false,false,true, '" & Replace(AttributeJavascript, "'", "\'") & "');"
      Else
        AttributeJavascript = "javascript:ParseForm('0', false,false,false,false,false, '" & Replace(AttributeJavascript, "'", "\'") & "');"
      End If

      'If Session.Item("isMobile") Then
      '  AttributeJavascript += "FigureMobileDisplayOut();document.body.style.cursor='wait';"
      'End If

      'AttributeJavascript = "alert('" & AttributeJavascript & "');"
      'Clear Saved Session.
      clsGeneral.clsGeneral.ClearSavedSelection()
      Session.Item("MasterAircraftWhere") = ""
      Session.Item("MasterAircraftFrom") = ""
      Session.Item("MasterAircraftSelect") = ""
      Session.Item("MasterAircraftSort") = ""

      Session.Item("MasterYachtWhere") = ""
      Session.Item("MasterYachtFrom") = ""
      Session.Item("MasterYachtSelect") = ""
      Session.Item("MasterYachtSort") = ""

      If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.yacht_update_panel, Me.GetType(), "parsingForm", "" & AttributeJavascript & "", True)
        Me.yacht_update_panel.Update()
      Else
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me.main_home_update_panel, Me.GetType(), "parsingForm", "" & AttributeJavascript & "", True)
        Me.main_home_update_panel.Update()
      End If

    End If

  End Sub

  Dim AttributeJavascript As String = ""
  Private Sub AttributeQuickSearchJavascriptBuilder(ByVal controlToLoop As ControlCollection)
    For Each ctrl As Control In controlToLoop
      If TypeOf ctrl Is CheckBox Then

        If DirectCast(ctrl, CheckBox).Checked = True Then
          Dim TempObject As CheckBox = ctrl
          Dim ActualID As String = ""
          Dim IDArray As Array
          IDArray = Split(ctrl.ID, "___")
          If UBound(IDArray) = 1 Then
            ActualID = IDArray(1)
          End If


          If (TempObject.Checked) Then
            If ActualID <> "" Then
              If AttributeJavascript <> "" Then
                AttributeJavascript += "!~!"
              End If

              AttributeJavascript += ActualID & "=" & TempObject.Checked.ToString.ToLower
            End If
          End If


        End If
      ElseIf TypeOf ctrl Is DropDownList Then
        Dim TempObject As DropDownList = ctrl
        Dim ActualID As String = ""

        If AttributeJavascript <> "" Then
          AttributeJavascript += "!~!"
        End If

        AttributeJavascript += ctrl.ID & "=" & TempObject.SelectedValue
      End If

      If ctrl.Controls.Count > 0 Then
        AttributeQuickSearchJavascriptBuilder(ctrl.Controls)
      End If
    Next

  End Sub

  Private Sub DisplayIndexTabFromCache()
    Dim TemporaryContainer As New Panel
    Dim MainContent As New ContentPlaceHolder
    If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
      ' masterPage = DirectCast(Page.Master, EvoTheme)
      If Session.Item("isMobile") Then
        masterPage = DirectCast(Page.Master, MobileTheme)
      Else
        masterPage = DirectCast(Page.Master, EvoTheme)
      End If
    ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
      masterPage = DirectCast(Page.Master, YachtTheme)
    End If

    If Not IsNothing(Page.Master.FindControl("ContentPlaceHolder1")) Then
      MainContent = TryCast(Page.Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
    End If

    'This actually fills up the cached attribute.
    AdvancedQueryResults.FillCachedAttributeDataset(masterPage.aclsData_Temp)

    If Not IsNothing(HttpContext.Current.Cache("CachedAttributeDataset")) Then
      Dim Find As New LinkButton
      Dim Find_Top As New LinkButton
      Dim ProductType As String = "Aircraft"
      Dim TemporaryTable As New DataTable 'Holds the topics attributes so we can use them to fill the autocomplete dropdown
      Dim DynamicIndexPanel As Panel = indexPanel
      Dim DynamicIndexTab As AjaxControlToolkit.TabPanel = index_tab
      Dim DynamicTabContainer As AjaxControlToolkit.TabContainer = main_home_tab_container

      If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.YACHT Then
        ProductType = "Yacht"
        DynamicIndexPanel = yachtIndexPanel
        DynamicIndexTab = yacht_index_tab
        DynamicTabContainer = yacht_summary_tab
      End If

      Find.Text = "Find " & ProductType
      Find.CssClass = "gray_button float_right margin_4"
      Find.OnClientClick = "document.body.style.cursor='wait';"
      AddHandler Find.Command, AddressOf Me.Find_Command

      Find_Top.Text = "Find " & ProductType
      Find_Top.CssClass = "gray_button float_right margin_4"
      Find_Top.OnClientClick = "document.body.style.cursor='wait';"
      AddHandler Find_Top.Command, AddressOf Me.Find_Command

      If Not IsNothing(Page.Master.FindControl("ContentPlaceHolder1")) Then
        MainContent = TryCast(Page.Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
      End If

      DynamicIndexTab.Visible = True

      AdvancedQueryResults.BuildAttributeTextAndDropdown(TemporaryContainer, MainContent.ClientID, DynamicTabContainer.ID, DynamicIndexTab.ID, Page, False, main_home_update_panel)
      DynamicIndexPanel.Controls.AddAt(0, TemporaryContainer)

      DynamicIndexPanel.Controls.AddAt(1, Find_Top)

      'TemporaryContainer = New Panel
      'TemporaryContainer.ID = "letter_display"

      'TemporaryTable = AdvancedQueryResults.BuildInitialCachedCheckboxesOnHomePage(HttpContext.Current.Cache("CachedAttributeDataset").tables(0), TemporaryContainer, True, False, "LETTER", "TOPIC", MainContent.ClientID, main_home_tab_container.ID, index_tab.ID, Me, False, masterPage.aclsData_Temp, main_home_update_panel)
      'DynamicIndexPanel.Controls.AddAt(2, TemporaryContainer)

      TemporaryContainer = New Panel
      TemporaryContainer.ID = "area_display"
      'TemporaryContainer.Attributes.Add("style", "display:none;")
      TemporaryTable = AdvancedQueryResults.BuildInitialCachedCheckboxesOnHomePage(HttpContext.Current.Cache("CachedAttributeDataset").tables(0), TemporaryContainer, False, True, "AREA", "TOPIC", MainContent.ClientID, main_home_tab_container.ID, index_tab.ID, Me, False, masterPage.aclsData_Temp, main_home_update_panel)

      DynamicIndexPanel.Controls.AddAt(2, TemporaryContainer)

      DynamicIndexPanel.Controls.AddAt(3, Find)

      'Filling up the attribute source
      If attributeAutoCompleteString = "" Then
        If Not IsNothing(TemporaryTable) Then
          If TemporaryTable.Rows.Count > 0 Then

            For Each r As DataRow In TemporaryTable.Rows
              If attributeAutoCompleteString <> "" Then
                attributeAutoCompleteString += ","
              End If
              attributeAutoCompleteString += "{label: """ & r("TOPIC").ToString & """, value: """ & r("TOPID").ToString & """}"
            Next

          End If
        End If
      End If

    End If
  End Sub

#End Region

#Region "Reports Tab for Evo"
  Public Sub Display_Reports(ByVal Reports_Label As Label, ByVal CRM As Boolean)

    Dim cssClass As String = ""
    Dim Results_Table As New DataTable
    Dim headerName As String = ""
    Results_Table = GetReportsForSubscription(Session.Item("localUser").crmSubSubID.ToString, True, True, False)
    If Not IsNothing(Results_Table) Then
      If Results_Table.Rows.Count > 0 Then

        reports_tab.Visible = True
        Reports_Label.Text = "<table width=""100%"" cellpadding=""3"" cellspacing=""0""  class=""" & IIf(CRM, "data_aircraft_grid", "data_aircraft_grid") & """>"

        Reports_Label.Text += "<tr>"
        Reports_Label.Text += "<td align=""left"" valign=""top""><p class=""red_text remove_margin padding_table"">"

        If Session.Item("localUser").crmDemoUserFlag = False Then
          Reports_Label.Text += "The following prepackaged reports automatically export to Excel and some may be time consuming to generate.<br>Click on the report title below to run."
        Else
          Reports_Label.Text += "The following prepackaged reports automatically export to Excel for non-demo users only."
        End If

        Reports_Label.Text += "</p></td>"
        Reports_Label.Text += "</tr>"

        For Each r As DataRow In Results_Table.Rows

          If headerName <> r("sqlrep_type").ToString Then
            Reports_Label.Text += "<tr class=""" & IIf(CRM, "header_row", "header_row") & """>"

            Reports_Label.Text += "<td align=""left"" valign=""top"">"
            Reports_Label.Text += "<b class=""title"">" & r("sqlrep_type").ToString & "</b>"
            Reports_Label.Text += "</td>"
            Reports_Label.Text += "</tr>"
          End If

          Reports_Label.Text += "<tr class='" & cssClass & "'>"

          Reports_Label.Text += "<td align=""left"" valign=""top"">"

          '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          ''''''''''''''''''''''''Report Name/Description''''''''''''''''''''''''''''''''''''''
          If Session.Item("localUser").crmDemoUserFlag = False Then
            Reports_Label.Text += "<a href=""export.aspx?repID=" & r("sqlrep_id").ToString & "&type_of=" & r("sqlrep_type").ToString & """ target=""new"">" & r("sqlrep_title").ToString & "</a> - "
          Else
            Reports_Label.Text += "<strong class=""text_underline"">" & r("sqlrep_title").ToString & "</strong> - "
          End If
          '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          ''''''''''''''''''''''''Report Description''''''''''''''''''''''''''''''''''''''''
          Reports_Label.Text += r("sqlrep_description").ToString

          Reports_Label.Text += "</td>"

          Reports_Label.Text += "</tr>"

          If cssClass = "" Then
            cssClass = "alt_row"
          Else
            cssClass = ""
          End If

          headerName = r("sqlrep_type").ToString
        Next

        Reports_Label.Text += "</table>"
      Else
        reports_tab.Visible = False
      End If
    Else

      Reports_Label.CssClass = "padding" ' emphasis_text"

    End If
    custom_reports_results.Visible = True

  End Sub

  Public Function GetReportsForSubscription(ByVal subscriptionID As Long, ByVal SubscriptionLevel As Boolean, ByVal AllLevel As Boolean, ByVal JetnetLevel As Boolean) As DataTable
    Dim sql As String = ""
    Dim sqlWhere As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable
    Dim open_paren As String = "("

    Try

      'Opening Connection
      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()

      sql = "Select * from [Homebase].jetnet_ra.dbo.SQL_Report where "

      'Subscription = customer based reports only available to specific customers by subscription id
      If SubscriptionLevel Then
        'where (sqlrep_level=’Subscription’ and sqlrep_sub_id = mySUB) 
        sqlWhere += " (sqlrep_sub_id = @subscriptionID and sqlrep_level='Subscription') "
      End If

      'All – customer based reports available to all customers
      If AllLevel Then
        'or (sqlrep_level=’All’ and sqlrep_sub_id = 0)
        If sqlWhere <> "" Then
          sqlWhere += " or "
        End If
        sqlWhere += " (sqlrep_level='All' and sqlrep_sub_id = 0) "
      End If

      'JETNET = jetnet only report
      If JetnetLevel Then
        If sqlWhere <> "" Then
          sqlWhere += " or "
        End If
        sqlWhere += " (sqlrep_level='JETNET' and sqlrep_sub_id = 0) "
      End If

      sql += "(" + sqlWhere + ")"

      If Session.Item("localSubscription").crmBusiness_Flag = True Or Session.Item("localSubscription").crmCommercial_Flag = True Or Session.Item("localSubscription").crmHelicopter_Flag = True Or Session.Item("localSubscription").crmYacht_Flag = True Then
        sql &= " and "
      End If

      If Session.Item("localSubscription").crmBusiness_Flag = True Then
        sql &= " " & open_paren & " sqlrep_product_business_flag = 'Y' "
        open_paren = " or "
      End If

      If Session.Item("localSubscription").crmCommercial_Flag = True Then
        sql &= " " & open_paren & " sqlrep_product_commercial_flag = 'Y' "
        open_paren = " or "
      End If


      If Session.Item("localSubscription").crmHelicopter_Flag = True Then
        sql &= " " & open_paren & " sqlrep_product_helicopter_flag = 'Y' "
        open_paren = " or "
      End If

      If Session.Item("localSubscription").crmYacht_Flag = True Then
        sql &= " " & open_paren & " sqlrep_product_yacht_flag = 'Y' "
        open_paren = " or "
      End If



      If Session.Item("localSubscription").crmBusiness_Flag = True Or Session.Item("localSubscription").crmCommercial_Flag = True Or Session.Item("localSubscription").crmHelicopter_Flag = True Or Session.Item("localSubscription").crmYacht_Flag = True Then
        sql &= ")"
      End If


      If Session.Item("localPreferences").AerodexFlag = True Then
        sql &= " and sqlrep_aerodex_flag = 'Y' "
      End If

      sql += " order by sqlrep_type, sqlrep_title"

      'save to session query debug string.
      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sql.ToString)

      Dim SqlCommand As New SqlClient.SqlCommand(sql, SqlConn)


      SqlCommand.Parameters.AddWithValue("subscriptionID", subscriptionID)

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

      SqlCommand.Dispose()
      SqlCommand = Nothing


      Return atemptable
    Catch ex As Exception
      GetReportsForSubscription = Nothing
      ' Me.class_error = "Error in GetReportsForSubscription(ByVal subscriptionID As Long) As DataTable: SQL VERSION " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing



    End Try

  End Function

#End Region

  Private Sub yacht_summary_tab_ActiveTabChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles yacht_summary_tab.ActiveTabChanged
    'yacht_index_tab
    If yacht_summary_tab.ActiveTab.ID.ToString = "yacht_index_tab" Then
      If yacht_index_tab_label.Text = "" Then
        'AdvancedQueryResults.LoopThroughHomeIndexTabToTurnOnCheckboxes(False, True, False, masterPage.aclsData_Temp, "LETTER", yacht_index_tab)
        AdvancedQueryResults.LoopThroughHomeIndexTabToTurnOnCheckboxes(False, False, True, masterPage.aclsData_Temp, "AREA", yacht_index_tab)
        yacht_index_tab_label.Text = Now.ToString
      End If
      yacht_index_wait_div.Visible = False
      yachtIndexPanel.CssClass = ""
    End If
    yacht_update_panel.Update()
  End Sub

  Private Sub action_time_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles action_time.SelectedIndexChanged

    Create_Evo_Action_Items(evo_action_items)
    actions_load.Visible = False
    main_home_update_panel.Update()

  End Sub


End Class





