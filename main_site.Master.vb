Imports System.IO
Imports crmWebClient.clsGeneral
Partial Public Class main_site
  Inherits System.Web.UI.MasterPage
  Public aclsData_Temp As New clsData_Manager_SQL
  Public aTempTable, aTempTable2 As New DataTable 'Data Tables used
  Private dtTable_List As DataTable
  Private booHeaderBar As Boolean
  Private intContactID As Integer
  Private intContactID_Other As Integer
  Private intOtherID As Integer
  Private strRecordCount As String
  Private strNameOfListingType As String
  Private strSubnodeMethod As String
  Private intID As Integer
  Private boolPerformDatabaseAction As Boolean
  Private blnShowJetnetClientOption As Boolean
  Private intMainLoc As Integer
  Private strNameofSubnode As String
  Private intSubNodeOfListing As Integer
  Private intFromTypeOfListing As Integer
  Private booNextBtn As Boolean
  Private booPrevious As Boolean
  Private blnIsJob As Boolean
  Private blnShowJetnetClient As Boolean
  Private boolIsSubnode As Boolean
  Private blnShowSearch As Boolean
  Private bAircraftSort_Company As Boolean
  Private dteDateOfAction As Date
  Private strSource As String
  Private strValuationLabelText As String
  Public Event BringResults()
  Public Event ClearResults()
  Public Event NextButton_Listing()
  Public Event SetPagerButtons()
  Public Event PreviousButton_Listing()
  Public Event resultsVisible()
  Public Event resultsInvisible()
  Public Event Swap_Columns()
  Public Event SendToTabs(ByVal show_jetnet As CheckBox)
  Public error_string As String = ""
  Public m_bIsTerminating As Boolean = False

  Public bEnableChat As Boolean
  Public script_version As String = ""

#Region "Page Load"
  Public Sub Write_Javascript_Out()
    javascript_text.Text = Write_JSCRIPT()
  End Sub
  Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

    '-------------------------------------------Database Connections--------------------------------------------------------------

    aclsData_Temp = New clsData_Manager_SQL
    aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
    aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase") 'CApplication.Item("crmJetnetDatabase")

    aclsData_Temp.class_error = ""

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
    SRef5.Path = "~/common/jquery.sidr.min.js"
    SMgr.Scripts.Add(SRef5)

    Dim SRef6 As ScriptReference = New ScriptReference()
    SRef6.Path = "~/common/jquery.cookie.js"
    SMgr.Scripts.Add(SRef6)

    Dim SRef7 As ScriptReference = New ScriptReference()
    SRef7.Path = "https://www.gstatic.com/charts/loader.js"
    SMgr.Scripts.Add(SRef7)

    If (CBool(My.Settings.enableChat)) Then

      ChatManager.CheckAndInitChat(False, bEnableChat)
      notifyChatUserPanel.Visible = False

      If bEnableChat Then

        Dim SvcRef As ServiceReference = New ServiceReference()
        SvcRef.Path = "~/chat/Services/chatServices.svc"

        SMgr.Services.Add(SvcRef)

        notifyChatUserPanel.Visible = True

      End If
    End If




    If Session.Item("crmUserLogon") <> True Then
      'error_string = "main_site.Master.vb - Page Init() - " & Request.ServerVariables("SCRIPT_NAME").ToString() & " - Session Timeout"
      'LogError(error_string)

      Response.Redirect("Default.aspx", False)
      HttpContext.Current.ApplicationInstance.CompleteRequest()
      m_bIsTerminating = True
    End If


    If Not Page.IsPostBack Then


      If Not IsNothing(Trim(Request("useFAAFlightData"))) Then
        If Not String.IsNullOrEmpty(Trim(Request("useFAAFlightData"))) Then
          Session.Item("useFAAFlightData") = Trim(Request("useFAAFlightData"))
        End If
      End If
    End If


    'added 5-3-2012
    'This is a check to make absolute sure that the jetnet database is filled and the app var hasn't died somehow.
    'if it died, I want to terminate the page immediately and send back to the default page to reset it and log in.
    If Application.Item("crmJetnetDatabase") = "" Then
      Response.Redirect("Default.aspx", True)
    End If

    Try
      If Request.QueryString.Item("debug") = "1" Then
        Application.Item("DebugFlag") = True
      Else
        Application.Item("DebugFlag") = False
      End If

      'Session.Item("localUser").crmUser_DebugText = ""

      'I want to display the database connection string, but I'm going to split it at ;Password for safety.
      Dim database_display As Array = Split(Application.Item("crmJetnetDatabase"), ";Password")

      If UBound(database_display) > 0 Then
        Session.Item("localUser").crmUser_DebugText += "<b>Jetnet Database Connection <em>Not displaying password</em></b>: " & database_display(0).ToString & "<br />"
      End If

      If InStr(Request.ServerVariables("SCRIPT_NAME").ToString(), "listing.aspx") > 0 Then
        TypeOfListing = 1
        FromTypeOfListing = 1
      ElseIf InStr(LCase(Request.ServerVariables("SCRIPT_NAME").ToString()), "listing_air.aspx") > 0 Then
        TypeOfListing = 3
        FromTypeOfListing = 3
      ElseIf InStr(LCase(Request.ServerVariables("SCRIPT_NAME").ToString()), "listing_jobs.aspx") > 0 Then
        TypeOfListing = 5
        FromTypeOfListing = 5
      ElseIf InStr(LCase(Request.ServerVariables("SCRIPT_NAME").ToString()), "listing_action.aspx") > 0 Then
        TypeOfListing = 4
        FromTypeOfListing = 4
      ElseIf InStr(LCase(Request.ServerVariables("SCRIPT_NAME").ToString()), "listing_wanted.aspx") > 0 Then
        TypeOfListing = 12
        FromTypeOfListing = 12
      ElseIf InStr(LCase(Request.ServerVariables("SCRIPT_NAME").ToString()), "listing_transaction.aspx") > 0 Then
        TypeOfListing = 8
        FromTypeOfListing = 8
      ElseIf InStr(LCase(Request.ServerVariables("SCRIPT_NAME").ToString()), "listing_notes.aspx") > 0 Then
        If Session.Item("Listing") = 16 Then
          TypeOfListing = 16
          FromTypeOfListing = 16
        Else
          TypeOfListing = 6
          FromTypeOfListing = 6
        End If
      ElseIf InStr(LCase(Request.ServerVariables("SCRIPT_NAME").ToString()), "listing_opportunities.aspx") > 0 Then
        TypeOfListing = 11
        FromTypeOfListing = 11
      ElseIf InStr(LCase(Request.ServerVariables("SCRIPT_NAME").ToString()), "home.aspx") > 0 Then
        TypeOfListing = 9
        FromTypeOfListing = 9
      ElseIf InStr(LCase(Request.ServerVariables("SCRIPT_NAME").ToString()), "market.aspx") > 0 Then
        TypeOfListing = 10
        FromTypeOfListing = 10
      ElseIf InStr(LCase(Request.ServerVariables("SCRIPT_NAME").ToString()), "listing_document.aspx") > 0 Then
        TypeOfListing = 7
        FromTypeOfListing = 7
      ElseIf InStr(LCase(Request.ServerVariables("SCRIPT_NAME").ToString()), "listing_contact.aspx") > 0 Then
        TypeOfListing = 2
        FromTypeOfListing = 2
      ElseIf InStr(LCase(Request.ServerVariables("SCRIPT_NAME").ToString()), "view.aspx") > 0 Then
        TypeOfListing = 13
        FromTypeOfListing = 13
      ElseIf InStr(LCase(Request.ServerVariables("SCRIPT_NAME").ToString()), "performance_specs.aspx") > 0 Or InStr(LCase(Request.ServerVariables("SCRIPT_NAME").ToString()), "op_costs.aspx") > 0 Then
        TypeOfListing = 14
        FromTypeOfListing = 14
      End If


      '---------------------------------------------End Database Connection Stuff---------------------------------------------
    Catch ex As Exception
      error_string = "main_site.Master.vb - Page Init() - " & ex.Message
      LogError(error_string)
    End Try
    'End If
  End Sub


  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try
      'Clears the session automatic refresh time to timeout. 
      Session.Item("AutomaticRefreshTime") = 0

      If Not Page.IsPostBack Then
        Session.Item("transaction_table") = Nothing
      End If

      'Display the default Company search panel
      Write_Javascript_Out()
      SetActiveTab()
      If IsSubNode Then
        airLink.Attributes.Remove("href")
        airLink.Attributes.Add("href", "/listing_air.aspx?clear=true")
        compLink.Attributes.Remove("href")
        compLink.Attributes.Add("href", "/listing.aspx?clear=true")
      End If
      If Not Page.IsPostBack Then
        Search_display()
        fill_bar()


        '---------------------------------------------This sets up the Background Image stuff--------------------------------------------------------
        If Not IsNothing(Session.Item("localUser").crmLocalUser_Background) Then
          If Session.Item("localUser").crmLocalUser_Background = "" Then
            If Not Page.IsPostBack Then
              aTempTable = aclsData_Temp.GetBackgroundImages()
              If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then
                  Session.Item("localUser").crmLocalUser_Background = aTempTable.Rows(0).Item("evoback_id")
                  Session.Item("localUser").crmLocalUser_Background_ID = aTempTable.Rows(0).Item("evoback_id")
                End If
              End If
            End If
          End If
        End If
        'Setting the background image.
        If Not IsNothing(Session.Item("localUser").crmLocalUser_background) Then
          If Session.Item("localUser").crmLocalUser_background <> "" Then
            BackgroundImage = "<img src=""" & Replace(HttpContext.Current.Session.Item("jetnetFullHostName").ToString.Trim, "https://", "http://") + Session.Item("ImagesVirtualPath") & "/background/" & Session.Item("localUser").crmLocalUser_background & ".jpg"" alt='' class=""bg_image"" />"
          End If
        End If
      End If


    Catch ex As Exception
      error_string = "main_site.Master.vb - Page Load() - " & ex.Message
      LogError(error_string)
    End Try
    'End If
  End Sub
#End Region
#Region "Properties"
  Public Property PerformDatabaseAction()
    Get
      Return boolPerformDatabaseAction
    End Get
    Set(ByVal value)
      boolPerformDatabaseAction = value
    End Set
  End Property
  Public Property ShowSearch() 'determines whether search is shown on display page
    Get
      Return blnShowSearch
    End Get
    Set(ByVal value)
      blnShowSearch = value
      Session.Item("ShowSearch") = value
    End Set
  End Property
  Public Property BackgroundImage() 'determines background image
    Get
      Return background_image.Text
    End Get
    Set(ByVal value)
      background_image.Text = value
    End Set
  End Property
  Public Property ShowJetnetClient() 'determines whether jetnet/client info is shown on ac details
    Get
      Return blnShowJetnetClient
    End Get
    Set(ByVal value)
      blnShowJetnetClient = value
    End Set
  End Property
  Public Property ShowJetnetClientOption() As Boolean
    Get
      Dim show_jetnet_client As CheckBox = SubNav1.FindControl("show_jetnet_client")
      Dim show_jetnet_lbl As Label = SubNav1.FindControl("show_jetnet_lbl")
      blnShowJetnetClientOption = show_jetnet_client.Visible
    End Get
    Set(ByVal value As Boolean)
      If Session.Item("localUser").crmEvo = True Then
        value = False
      End If
      blnShowJetnetClientOption = value
      If TypeOfListing = 3 Then
        Dim show_jetnet_client As CheckBox = SubNav1.FindControl("show_jetnet_client")
        Dim show_jetnet_lbl As Label = SubNav1.FindControl("show_jetnet_lbl")
        show_jetnet_client.Visible = value
        show_jetnet_lbl.Visible = value
      End If
      Dim switch As Image = SubNav1.FindControl("switch")
      switch.Visible = value

      If OtherID = 0 Or Session.Item("localUser").crmEvo = True Then
        switch.Visible = False
      End If
    End Set
  End Property
  'This sets the OTHER id. Meaning if there's a jetnet company and it has a corresponding CLIENT ID - this is it. And vice versa.
  Public Property OtherID()
    Get
      Return intOtherID

    End Get
    Set(ByVal value)
      intOtherID = value
      Session.Item("OtherID") = value

      If value <> 0 Then
        'Setting up the prospect label if the Other ID is set.
        If TypeOfListing = 3 Then
          If Session.Item("localSubscription").crmAerodexFlag = False Then
            If Not IsNothing(SubNav1.FindControl("gold_prospect_icon_label")) Then
              Dim gold_prospect_Icon_label As Label = SubNav1.FindControl("gold_prospect_icon_label")
              gold_prospect_Icon_label.Text = "<img src='/images/gold_prospect_icon.png' class='gold_icon help_cursor' alt='Prospect View' title='Launch Prospect View' onclick=""javascript:load('view_template.aspx?ViewID=18&" & IIf(TypeOfListing = 3, "ac_id=", "comp_id=") & IIf(ListingSource = "JETNET", ListingID, OtherID) & "&noMaster=false','','scrollbars=yes,menubar=no,height=700,width=1150,resizable=yes,toolbar=no,location=no,status=no');""/>"
            End If
          End If
        End If

        'Setting up the jetnet/client toggle button.
        If Not IsNothing(SubNav1.FindControl("switch_link_begin")) Then
          Dim switch_link_begin As Label = SubNav1.FindControl("switch_link_begin")
          Dim var As String = ""
          If TypeOfListing = 1 Then
            var = "comp_ID"
          ElseIf TypeOfListing = 3 Then
            var = "ac_ID"
          End If
          If ListingSource = "JETNET" Then
            switch_link_begin.Text = "<a class='switch' href='details.aspx?source=CLIENT&" & var & "=" & value & "&type=" & TypeOfListing
            If TypeOfListing = 1 And Listing_ContactID_Other <> 0 Then
              switch_link_begin.Text = switch_link_begin.Text & "&contact_ID=" & Listing_ContactID_Other
            End If
            switch_link_begin.Text = switch_link_begin.Text & "'>"
          Else
            switch_link_begin.Text = "<a class='switch' href='details.aspx?source=JETNET&" & var & "=" & value & "&type=" & TypeOfListing
            If TypeOfListing = 1 And Listing_ContactID_Other <> 0 Then
              switch_link_begin.Text = switch_link_begin.Text & "&contact_ID=" & Listing_ContactID_Other
            End If
            switch_link_begin.Text = switch_link_begin.Text & "'>"
          End If
        End If
      Else
        'if the listing source is client and there is no otherID (jetnet ID), then clear the prospect label for now.
        If ListingSource = "CLIENT" Then
          If Not IsNothing(SubNav1.FindControl("gold_prospect_icon_label")) Then
            Dim gold_prospect_Icon_label As Label = SubNav1.FindControl("gold_prospect_icon_label")
            gold_prospect_Icon_label.Text = ""
          End If
        End If
      End If

    End Set
  End Property
  'This sets the parent main location ID
  Public Property MainLocID()
    Get
      Return intMainLoc
    End Get
    Set(ByVal value)
      intMainLoc = value
      'Session.Item("MainLoc") = value
    End Set
  End Property
  'Set paging count
  Public Property SetRecordCount()
    Get
      Return strRecordCount
    End Get
    Set(ByVal value)
      strRecordCount = value
      record_count.Text = value
      record_count.Visible = True
    End Set
  End Property
  'Set Visibility on Header
  Public Property Header_Bar()
    Get
      Return booHeaderBar
    End Get
    Set(ByVal value)
      booHeaderBar = value
      If booHeaderBar = True Then
        bar_main_text.Visible = True
        record_holder.Visible = True
      Else
        bar_main_text.Visible = False
        record_holder.Visible = False
      End If
    End Set
  End Property
  'Set the visibility on buttons
  Public Property Next_Button_Visible() 'Allows us to set the next button to invisible if needed
    Get
      Return booNextBtn
    End Get
    Set(ByVal value)
      booNextBtn = value
      If booNextBtn = True Then
        Nex_top.Visible = True
        Nex_bottom.Visible = True
      Else
        Nex_top.Visible = False
        Nex_bottom.Visible = False
      End If
    End Set
  End Property
  Public Property Previous_Button_Visible() 'Allows us to set the previous button to invisible if needed
    Get
      Return booPrevious
    End Get
    Set(ByVal value)
      booPrevious = value
      If booPrevious = True Then
        Pre_top.Visible = True
        Pre_bottom.Visible = True
      Else
        Pre_top.Visible = False
        Pre_bottom.Visible = False
      End If
    End Set
  End Property
  Public Property DateOfActionItem() 'Date used for calendar
    Get
      Return dteDateOfAction
    End Get
    Set(ByVal value)
      dteDateOfAction = FormatDateTime(value, 2)
      Session("DayPilotCalendar1_startDate") = value
    End Set
  End Property
  Public Property Table_List() As DataTable 'This is the result list of a search in a datatable
    Get
      Return dtTable_List
    End Get
    Set(ByVal value As DataTable)
      dtTable_List = value
    End Set
  End Property
  Public Property TypeOfListing() As Integer 'This is for the main category ID. Company/Contact/AC, etc
    Get
      Return Session.Item("Listing")
    End Get
    Set(ByVal value As Integer)
      Session.Item("Listing") = value
    End Set
  End Property
  Public Property NameOfListingType() As String 'This for the main category text. Company/Contact/AC, etc
    Get
      Return strNameOfListingType
    End Get
    Set(ByVal value As String)
      strNameOfListingType = value
    End Set
  End Property
  Public Property NameOfSubnode() As String 'Subnode Text that's been clicked on
    Get
      Return strNameofSubnode
    End Get
    Set(ByVal value As String)
      strNameofSubnode = value
      Session.Item("SubnodeName") = value
    End Set
  End Property
  Public Property Subnode_Method() As String 'Subnode Method That's been Clicked on
    Get
      Return strSubnodeMethod
    End Get
    Set(ByVal value As String)
      strSubnodeMethod = value
      Session.Item("SubnodeMethod") = value
    End Set
  End Property
  Public Property IsSubNode() As Boolean
    Get
      Return boolIsSubnode
    End Get
    Set(ByVal value As Boolean)
      boolIsSubnode = value
      Session.Item("isSubnode") = value
    End Set
  End Property
  Public Property SubNodeOfListing() As Integer 'Subnode ID that's been clicked on
    Get
      Return intSubNodeOfListing
    End Get
    Set(ByVal value As Integer)
      intSubNodeOfListing = value
      Session.Item("Subnode") = value
    End Set
  End Property

  'Properties used to display page information
  Public Property ListingID() As Integer 'ID of listing
    Get
      Return intID
    End Get
    Set(ByVal value As Integer)
      intID = value
      Session.Item("ListingID") = value
    End Set
  End Property
  Public Property FromTypeOfListing() As Integer
    Get
      Return intFromTypeOfListing
    End Get
    Set(ByVal value As Integer)
      intFromTypeOfListing = value
      Session.Item("FromTypeOfListing") = value
    End Set
  End Property
  Public Property ListingSource() As String 'Jetnet/Client source
    Get
      Return strSource
    End Get
    Set(ByVal value As String)
      strSource = UCase(value)
      Session.Item("ListingSource") = UCase(value)
    End Set
  End Property
  Public Property Listing_ContactID() As Integer
    Get
      Return intContactID
    End Get
    Set(ByVal value As Integer)
      intContactID = value
      Session.Item("ContactID") = value
    End Set
  End Property
  Public Property Listing_IsJob() As Boolean
    Get
      Return blnIsJob
    End Get
    Set(ByVal value As Boolean)
      blnIsJob = value
      Session.Item("IsJob") = value
    End Set
  End Property

  Public Property Listing_ContactID_Other() As Integer
    Get
      Return intContactID_Other
    End Get
    Set(ByVal value As Integer)
      intContactID_Other = value
      Session.Item("ContactID_Other") = value
    End Set
  End Property
  Public Property AircraftSort_Company() As Boolean
    Get
      Return bAircraftSort_Company
    End Get
    Set(ByVal value As Boolean)
      bAircraftSort_Company = value
      Session.Item("AircraftSort_Company") = value
    End Set
  End Property
  Public Property SetAircraftValuationLink() As String
    Get
      Return strValuationLabelText
    End Get
    Set(ByVal value As String)
      strValuationLabelText = value
      'For right now - this only displays if you're on test or local.
      'If Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Or Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Then
      'And you're not aerodex
      If Session.Item("localSubscription").crmAerodexFlag = False Then
        'Start to find the label to create the valuation link.
        If Not IsNothing(SubNav1.FindControl("valuation_label")) Then
          Dim valuation_label As Label = SubNav1.FindControl("valuation_label")
          'Use the value as the link text.
          valuation_label.Text = value
        End If
      End If
      'End If
    End Set
  End Property
#End Region
#Region "Custom Events"
  Public Sub FillTreeView()
    TreeNav.Make_TreeList()
  End Sub
  Public Sub fill_bar()
    Try
      Dim operations_text As Label = SubNav1.FindControl("operations_text")
      Dim show_jetnet_client As CheckBox = SubNav1.FindControl("show_jetnet_client")
      Dim show_jetnet_lbl As Label = SubNav1.FindControl("show_jetnet_lbl")
      operations_text.Text = "" 'clear

      If TypeOfListing = 3 Then
        If OtherID = 0 Then
          show_jetnet_client.Visible = False
          show_jetnet_lbl.Visible = False
        Else
          show_jetnet_client.Visible = True
          show_jetnet_lbl.Visible = True
        End If
      End If
      If (Session("ListingID") = 0) Then

        If Trim(Session("export_info")) <> "" Then
          'If market_search.Visible = True Or WantedSearch.Visible = True Then
          'ElseIf ContactSearch.Visible = True Or TransactionSearch.Visible = True Or DocumentSearch.Visible = True Or OpportunitiesSearch.Visible = True Or TypeOfListing = 16 Or TypeOfListing = 14 And Session.Item("localUser").crmEvo <> True Then 'If not an EVO user Then
          '  operations_text.Text = "<td align='left' valign='middle'><a href='#' rel='anylinkmenu2' class='menuanchorclass'><img src='images/spacer.gif' alt='' border='0' />Actions</a></td>"
          'ElseIf Session.Item("localUser").crmEvo <> True Then 'If not an EVO userThen
          '  If TypeOfListing <> 16 Then
          '    operations_text.Text = "<td align='left' valign='middle'>"
          '    operations_text.Text += "<a href='#' rel='anylinkmenu1' class='menuanchorclass'><img src='images/spacer.gif' alt='' border='0' />Edit</a></td><td align='left' valign='middle><a href='#' rel='anylinkmenu2' class='menuanchorclass'><img src='images/spacer.gif' alt='' border='0' />Actions</a>"
          '    operations_text.Text += "</td>"
          '  End If
          'ElseIf Session.Item("localUser").crmEvo = True Then 'if an evo user
          '  If WantedSearch.Visible = True Or aircraftSearch.Visible = True Or companySearch.Visible = True Then
          '    operations_text.Text = "<td align='left' valign='middle'><a href='#' rel='anylinkmenu2' class='menuanchorclass'><img src='images/spacer.gif' alt='' border='0' />Actions</a></td>"
          '  End If
          'End If

          'If aircraftSearch.Visible Or companySearch.Visible Or ContactSearch.Visible Then
          operations_text.Text = "<td align='left' valign='middle'>"
          operations_text.Text += "<a href='#' rel='anylinkmenu2' class='menuanchorclass'><img src='images/spacer.gif' alt='' border='0' />Edit</a></td>"
          operations_text.Text += "</td>"
          'Add in the actions menu item
          If TypeOfListing <> 16 And market_search.Visible = False Then
            operations_text.Text += "<td align='left' valign='middle'>"
            operations_text.Text += "<a href='#' rel='anylinkmenu3' class='menuanchorclass'><img src='images/spacer.gif' alt='' border='0' />Actions</a></td>"
            'End If
          End If

        Else
          'If ContactSearch.Visible = False And TransactionSearch.Visible = False And market_search.Visible = False And DocumentSearch.Visible = False And WantedSearch.Visible = False And TypeOfListing <> 14 Then
          operations_text.Text = "<td align='left' valign='middle'><a href='#' rel='anylinkmenu2' class='menuanchorclass'><img src='images/spacer.gif' alt='' border='0' />Edit</a></a></td>"
          'ElseIf ContactSearch.Visible = True Then
          '  operations_text.Text = "<td align='left' valign='middle'><a href='#' rel='anylinkmenu3' class='menuanchorclass'><img src='images/spacer.gif' alt='' border='0' />Edit</a></a></td>"
          'End If

        End If
      ElseIf Session.Item("ListingID") <> 0 Then
        Select Case Session("ListingSource")
          Case "JETNET"
            Select Case CLng(Session("Listing"))
              Case 1
                If OtherID = 0 And Session.Item("localUser").crmEvo <> True Then 'If not an EVO userThen
                  operations_text.Text = "<td align='left' valign='middle'><a href='#' rel='anylinkmenu2' class='menuanchorclass'><img src='images/spacer.gif' alt='' border='0' />Edit</a></td><td align='left' valign='middle'><a href='#' rel='anylinkmenu3' class='menuanchorclass'><img src='images/spacer.gif' alt='' border='0' />Actions</a></td>"
                ElseIf OtherID <> 0 And Session.Item("localUser").crmEvo <> True Then 'If not an EVO user
                  operations_text.Text = "<td align='left' valign='middle'><a href='#' rel='anylinkmenu2' class='menuanchorclass'><img src='images/spacer.gif' alt='' border='0' />Edit</a></td><td align='left' valign='middle'><a href='#' rel='anylinkmenu3' class='menuanchorclass'><img src='images/spacer.gif' alt='' border='0' />Actions</a></td>"
                End If
              Case 3
                operations_text.Text = "<td align='left' valign='middle'><a href='#' rel='anylinkmenu2' class='menuanchorclass'><img src='images/spacer.gif' alt='' border='0' />Edit</a></td><td align='left' valign='middle'><a href='#' rel='anylinkmenu3' class='menuanchorclass'><img src='images/spacer.gif' alt='' border='0' />Actions</a></td>"
            End Select
          Case "CLIENT"
            Select Case CLng(Session("Listing"))

              Case 1
                operations_text.Text = "<td align='left' valign='middle'><a href='#' rel='anylinkmenu2' class='menuanchorclass'><img src='images/spacer.gif' alt='' border='0' />Edit</a></td><td align='left' valign='middle'><a href='#' rel='anylinkmenu3' class='menuanchorclass'><img src='images/spacer.gif' alt='' border='0' />Actions</a></td>"
              Case 3
                operations_text.Text = "<td align='left' valign='middle'><a href='#' rel='anylinkmenu2' class='menuanchorclass'><img src='images/spacer.gif' alt='' border='0' />Edit</a></td><td align='left' valign='middle'><a href='#' rel='anylinkmenu3' class='menuanchorclass'><img src='images/spacer.gif' alt='' border='0' />Actions</a></td>"
            End Select

        End Select
      Else
        operations_text.Text = "<td align='left' valign='middle'><a href='#' rel='anylinkmenu2' class='menuanchorclass'><img src='images/spacer.gif' alt='' border='0' />Edit</a></td>"
      End If

      If Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR Then
        operations_text.Text = "<td align='left' valign='middle'><a href=""#"" rel=""anylinkmenu_sub2"" class=""menuanchorclass"">Admin</a></td>" & operations_text.Text
      End If

    Catch ex As Exception
      error_string = "main_site.Master.vb - fill_bar() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub

  Private Sub SubNav1_Show_Both_Jetnet_Client_AC_Tabs(ByVal show_jetnet As System.Web.UI.WebControls.CheckBox) Handles SubNav1.Show_Both_Jetnet_Client_AC_Tabs
    RaiseEvent SendToTabs(show_jetnet)
  End Sub

  Sub default_models_check_changed(ByVal c As Control)
    Dim model_cbo As ListBox = c.FindControl("model_cbo")
    'Response.Write(model_cbo.SelectionMode)
    Dim sel_mode As String = model_cbo.SelectionMode 'this is important. Comments will explain more below.
    Dim make As ListBox = c.FindControl("make")
    Dim model As ListBox = c.FindControl("model")
    Dim model_type As CheckBoxList = c.FindControl("model_type")
    Dim type As ListBox = c.FindControl("type")
    Dim model_evo_swap As Label = c.FindControl("model_evo_swap")
    Dim default_models As CheckBox = c.FindControl("default_models")
    Dim model_list As ListBox = c.FindControl("model_cbo")
    Dim TypeDataTable As New DataTable
    Dim TypeDataHold As New DataTable
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Dim TempTable As New DataTable
    Dim selected As Boolean = False

    If default_models.Checked = True Then
      model_evo_swap.Visible = False
      'If the selection mode is single, it won't really matter what the default model checked is, the selected parameter
      'of populate_models function will always be false. Basically that the default selection shows up
      'but it's not selected. This will happen because you can only select one with the mode.
      If UCase(sel_mode) = "0" Then ' 0 = single, 1 = double
        selected = False
      Else
        selected = True
      End If
      make.Visible = False
      model.Visible = False
      type.Visible = False
      model_cbo.Visible = True
      model_type.Visible = False
      clsGeneral.clsGeneral.populate_models(model_list, True, c, Nothing, Me, selected)
    Else
      '    make.Visible = True
      '    model.Visible = True
      '    type.Visible = True
      '    model_cbo.Visible = False
      '    model_evo_swap.Visible = True
      '    model_type.Visible = True
      '    clsGeneral.clsGeneral.Getting_Type_Listbox_Set(TypeDataTable, model_type, MasterPage, TempTable, TypeDataHold, type)
      clsGeneral.clsGeneral.populate_models(model_list, False, c, Nothing, Me, False)
    End If
  End Sub
  'Fires when the dropdown box index is changed. Changes the search_display. 
  Private Sub companySearch_ChangedListing(ByVal sender As Object, ByVal Listing_Type As String) Handles companySearch.ChangedListing, DocumentSearch.ChangedListing, ActionItemsSearch.ChangedListing, aircraftSearch.ChangedListing, NotesSearch.ChangedListing, ContactSearch.ChangedListing, JobsSearch.ChangedListing, OpportunitiesSearch.ChangedListing, TransactionSearch.ChangedListing, market_search.ChangedListing
    Try
      TypeOfListing = Listing_Type
      SubNodeOfListing = Listing_Type
      NameOfSubnode = ""
      Redirect_Based_On_Type()
    Catch ex As Exception
      error_string = "main_site.Master.vb - companySearch_ChangedListing() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub

  '--------------------------------------------------Custom Events-------------------------------------------------------------
  Private Sub companySearch_Searched_Me(ByVal sender As Object, ByVal subnode As String, ByVal search_for As String, ByVal search_where As String, ByVal search_for_cbo As String, ByVal status_cbo As String, ByVal subset As String, ByVal country As String, ByVal states As String, ByVal operator_type As String, ByVal show_all As String, ByVal special_field As String, ByVal special_field_text As String, ByVal special_field_view As Boolean, ByVal special_field_column As String, ByVal client_IDS As String, ByVal jetnet_IDS As String, ByVal companyCity As String, ByVal mergeLists As Boolean) Handles companySearch.Searched_Me
    Try
      Fill_Company(subnode, search_for, search_where, search_for_cbo, status_cbo, subset, country, states, operator_type, show_all, special_field, special_field_text, special_field_view, special_field_column, client_IDS, jetnet_IDS, companyCity, mergeLists)
    Catch ex As Exception
      error_string = "main_site.Master.vb - companySearch_Searched_Me() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Private Sub ContactSearch_Searched_me(ByVal sender As Object, ByVal search_first As String, ByVal search_last As String, ByVal search_where As String, ByVal search_for_cbo As String, ByVal company_name As String, ByVal status_cbo As String, ByVal ordered_by As String, ByVal subset As String, ByVal email_address As String, ByVal phone As String) Handles ContactSearch.Searched_me
    Try
      Fill_Contact(False, search_first, search_last, search_where, company_name, status_cbo, ordered_by, subset, email_address, phone)
    Catch ex As Exception
      error_string = "main_site.Master.vb - ContactSearch_Searched_me() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub

  Private Sub aircraftSearch_Searched_Me(ByVal subnode As Boolean, ByVal search_for As String, ByVal search_where As String, ByVal search_for_cbo As String, ByVal model_cbo As String, ByVal market_status_cbo As String, ByVal sort As String, ByVal sort_how As String, ByVal subset As String, ByVal airport_name As String, ByVal icao_code As String, ByVal iata_code As String, ByVal city As String, ByVal country As String, ByVal state As String, ByVal types_of_owners As String, ByVal on_exclusive As String, ByVal on_lease As String, ByVal year_start As String, ByVal year_end As String, ByVal search_field As String, ByVal lifecycle As String, ByVal ownership As String, ByVal CustomField1 As String, ByVal CustomField2 As String, ByVal CustomField3 As String, ByVal CustomField4 As String, ByVal CustomField5 As String, ByVal CustomField6 As String, ByVal CustomField7 As String, ByVal CustomField8 As String, ByVal CustomField9 As String, ByVal CustomField10 As String, ByVal AircraftNotesSearch As Integer, ByVal AircraftNoteDate As String, ByVal MergeLists As Boolean) Handles aircraftSearch.Searched_Me
    Try
      If (Trim(Request("show_only_client")) = "Y" Or Trim(Request("forSale")) <> "") And Not IsPostBack Then

        If Trim(Request("show_only_client")) = "Y" And Trim(Request("forSale")) = "true" Then
          Fill_Aircraft(subnode, search_for, search_where, search_for_cbo, model_cbo, "For Sale", sort, sort_how, "", "", "C", airport_name, icao_code, iata_code, city, country, state, types_of_owners, on_exclusive, on_lease, year_start, year_end, search_field, lifecycle, ownership, CustomField1, CustomField2, CustomField3, CustomField4, CustomField5, CustomField6, CustomField7, CustomField8, CustomField9, CustomField10, AircraftNotesSearch, AircraftNoteDate, MergeLists)
        ElseIf Trim(Request("show_only_client")) = "Y" And Trim(Request("forSale")) = "false" Then
          Fill_Aircraft(subnode, search_for, search_where, search_for_cbo, model_cbo, "Not For Sale", sort, sort_how, "", "", "C", airport_name, icao_code, iata_code, city, country, state, types_of_owners, on_exclusive, on_lease, year_start, year_end, search_field, lifecycle, ownership, CustomField1, CustomField2, CustomField3, CustomField4, CustomField5, CustomField6, CustomField7, CustomField8, CustomField9, CustomField10, AircraftNotesSearch, AircraftNoteDate, MergeLists)
        ElseIf Trim(Request("show_only_client")) = "Y" Then
          Fill_Aircraft(subnode, search_for, search_where, search_for_cbo, model_cbo, "", sort, sort_how, "", "", "C", airport_name, icao_code, iata_code, city, country, state, types_of_owners, on_exclusive, on_lease, year_start, year_end, search_field, lifecycle, ownership, CustomField1, CustomField2, CustomField3, CustomField4, CustomField5, CustomField6, CustomField7, CustomField8, CustomField9, CustomField10, AircraftNotesSearch, AircraftNoteDate, MergeLists)
        ElseIf Trim(Request("forSale")) = "true" Then
          Fill_Aircraft(subnode, search_for, search_where, search_for_cbo, model_cbo, "For Sale", sort, sort_how, "", "", "JC", airport_name, icao_code, iata_code, city, country, state, types_of_owners, on_exclusive, on_lease, year_start, year_end, search_field, lifecycle, ownership, CustomField1, CustomField2, CustomField3, CustomField4, CustomField5, CustomField6, CustomField7, CustomField8, CustomField9, CustomField10, AircraftNotesSearch, AircraftNoteDate, MergeLists)
        ElseIf Trim(Request("forSale")) = "false" Then
          Fill_Aircraft(subnode, search_for, search_where, search_for_cbo, model_cbo, "Not For Sale", sort, sort_how, "", "", "JC", airport_name, icao_code, iata_code, city, country, state, types_of_owners, on_exclusive, on_lease, year_start, year_end, search_field, lifecycle, ownership, CustomField1, CustomField2, CustomField3, CustomField4, CustomField5, CustomField6, CustomField7, CustomField8, CustomField9, CustomField10, AircraftNotesSearch, AircraftNoteDate, MergeLists)
        Else
          Fill_Aircraft(subnode, search_for, search_where, search_for_cbo, model_cbo, market_status_cbo, sort, sort_how, "", "JC", subset, airport_name, icao_code, iata_code, city, country, state, types_of_owners, on_exclusive, on_lease, year_start, year_end, search_field, lifecycle, ownership, CustomField1, CustomField2, CustomField3, CustomField4, CustomField5, CustomField6, CustomField7, CustomField8, CustomField9, CustomField10, AircraftNotesSearch, AircraftNoteDate, MergeLists)
        End If
      Else
        Fill_Aircraft(subnode, search_for, search_where, search_for_cbo, model_cbo, market_status_cbo, sort, sort_how, "", "JC", subset, airport_name, icao_code, iata_code, city, country, state, types_of_owners, on_exclusive, on_lease, year_start, year_end, search_field, lifecycle, ownership, CustomField1, CustomField2, CustomField3, CustomField4, CustomField5, CustomField6, CustomField7, CustomField8, CustomField9, CustomField10, AircraftNotesSearch, AircraftNoteDate, MergeLists)
      End If
    Catch ex As Exception
      error_string = "main_site.Master.vb - aircraftSearch_Searched_Me() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Private Sub NotesSearch_Searched_Me(ByVal sender As Object, ByVal search_for As String, ByVal search_where As String, ByVal search_for_cbo As String, ByVal ActiveStatus As String, ByVal type_notes As String, ByVal orderby As String, ByVal reg_start As String, ByVal reg_end As String, ByVal clientIds As String, ByVal JetnetIds As String, ByVal acSearchField As Integer, ByVal acSearchOperator As Integer, ByVal acSearchText As String, ByVal NoteCategory As Integer, ByVal OnlyModel As Boolean, ByVal OnlyAircraft As Boolean, ByVal FolderType As Long) Handles NotesSearch.Searched_Me
    Try
      Fill_Notes(search_for, search_where, ActiveStatus, type_notes, orderby, reg_start, reg_end, clientIds, JetnetIds, acSearchField, acSearchOperator, acSearchText, NoteCategory, OnlyModel, OnlyAircraft, FolderType)
    Catch ex As Exception
      error_string = "main_site.Master.vb - NotesSearch_Searched_Me() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Private Sub ActionItemsSearch_Searched_Me(ByVal sender As Object, ByVal search_for As String, ByVal search_where As String, ByVal search_for_cbo As String, ByVal view_by As String, ByVal type_notes As String, ByVal orderby As String, ByVal start As String, ByVal reg_start As String, ByVal reg_end As String) Handles ActionItemsSearch.Searched_Me
    Try
      If UCase(view_by) = "WEEK" Then
        If start <> "" Then
          DateOfActionItem = start
        End If
        Fill_DayPilotCalendar1("Week")
        RaiseEvent resultsInvisible()
      ElseIf UCase(view_by) = "MONTH" Then
        If start <> "" Then
          DateOfActionItem = start
        End If
        Fill_DayPilotCalendar1("Month")
        RaiseEvent resultsInvisible()
      ElseIf UCase(view_by) = "DAY" Then
        If start <> "" Then
          DateOfActionItem = start
        End If
        Fill_DayPilotCalendar1("Day")
        RaiseEvent resultsInvisible()
      ElseIf UCase(view_by) = "LIST" Then

        DayPilotCalendar1.Visible = False
        Fill_Action("", search_for, search_where, view_by, type_notes, orderby, reg_start, reg_end)
        record_count.Visible = True
        RaiseEvent resultsVisible()
      End If
    Catch ex As Exception
      error_string = "main_site.Master.vb - ActionItemsSearch_Searched_Me() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Private Sub DocumentsSearch_Searched_Me(ByVal sender As Object, ByVal search_for As String, ByVal search_where As String, ByVal search_for_cbo As String, ByVal model_cbo As String, ByVal display_cbo As String, ByVal order_by As String, ByVal category As Integer, ByVal start_date As String, ByVal end_date As String) Handles DocumentSearch.Searched_Me
    Try
      Fill_Documents(search_for, search_where, model_cbo, display_cbo, order_by, category, start_date, end_date)
    Catch ex As Exception
      error_string = "main_site.Master.vb - DocumentsSearch_Searched_Me() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Private Sub OpportunitiesSearch_Searched_Me(ByVal sender As Object, ByVal search_for As String, ByVal search_where As String, ByVal search_for_cbo As String, ByVal display_cbo As String, ByVal start_date As String, ByVal category As Integer, ByVal end_date As String, ByVal status As String) Handles OpportunitiesSearch.Searched_Me
    Try
      Fill_Opportunities(search_for, search_where, display_cbo, category, start_date, end_date, status)
    Catch ex As Exception
      error_string = "main_site.Master.vb - DocumentsSearch_Searched_Me() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Private Sub TransactionSearch_Searched_me(ByVal sender As Object, ByVal search As String, ByVal search_where As String, ByVal search_for_cbo As String, ByVal model_cbo As String, ByVal subset As String, ByVal trans_type As String, ByVal start_date As String, ByVal end_date As String, ByVal relationships As String, ByVal year_start As String, ByVal year_end As String, ByVal internal As String, ByVal awaiting As Boolean) Handles TransactionSearch.Searched_me
    Try
      Fill_Transactions(search, search_where, search_for_cbo, model_cbo, subset, trans_type, start_date, end_date, relationships, year_start, year_end, internal, awaiting)
    Catch ex As Exception
      error_string = "main_site.Master.vb -TransactionsSearch_Searched_Me() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub

  Private Sub market_search_check_changed(ByVal sender As Object) Handles DocumentSearch.check_changed, OpportunitiesSearch.check_changed, market_search.check_changed, TransactionSearch.check_changed, aircraftSearch.check_changed, NotesSearch.check_changed, WantedSearch.check_changed
    default_models_check_changed(sender)
  End Sub
  Private Sub market_search_Market_Searched_me(ByVal sender As Object, ByVal model_cbo As ListBox, ByVal start_date As Integer, ByVal categories As ListBox, ByVal types As ListBox, ByVal start As String, ByVal end_date As String) Handles market_search.Market_Searched_me
    Try
      Fill_Market(model_cbo, start_date, categories, types, start, end_date)
    Catch ex As Exception
      error_string = "main_site.Master.vb -TransactionsSearch_Market_Searched_Me() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Private Sub wanted_search_Wanted_Searched_me(ByVal sender As Object, ByVal model_cbo As ListBox, ByVal start_date As String, ByVal end_date As String, ByVal interested_party As String, ByVal subset As String) Handles WantedSearch.Wanted_Searched_me
    Fill_Wanted(model_cbo, start_date, end_date, interested_party, subset)
  End Sub
  Private Sub Calendar_Changed_Date(ByVal ActionDate As String) Handles Calendar.Changed_Date
    Try
      FromTypeOfListing = 4
      DateOfActionItem = ActionDate
      TypeOfListing = 4
      NameOfSubnode = "Action Items"
      SubNodeOfListing = 4
      Redirect_Based_On_Type()
    Catch ex As Exception
      error_string = "main_site.Master.vb - Calendar_Changed_Date() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
#End Region
#Region "Public Functions"
  '-----------------------------------------------------Public Functions--------------------------------------------------------
  Public Sub LogError(ByVal ex As String)
    aclsData_Temp.LogError(Application.Item("crmClientSiteData").crmClientHostName, ex, DateTime.Now.ToString())
  End Sub
  Function DisplayAFTT(ByVal val As String, ByVal transaction As Boolean, ByVal actualVal As Object) As String
    DisplayAFTT = ""
    Dim aftt As New CheckBox
    If transaction Then
      aftt = TransactionSearch.FindControl("aftt")
    Else
      aftt = aircraftSearch.FindControl("aftt")
    End If

    If Not IsDBNull(actualVal) Then
      If actualVal > 0 Then
        If aftt.Checked = True Then
          DisplayAFTT = val
        Else
          DisplayAFTT = ""
        End If
      Else
        DisplayAFTT = ""
      End If
    Else
      DisplayAFTT = ""
    End If
  End Function
  Public Function display_error()
    '------------------------------Function that Creates a Javascript Error if the data manager class errors-----------
    display_error = ""
    If aclsData_Temp.class_error <> "" Then
      System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Error", "alert('" & Replace(aclsData_Temp.class_error, "'", " \'") & "');", True)
    End If
    aclsData_Temp.class_error = ""
  End Function
  '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
  ' Method name: Fill_DayPilotCalendar1
  ' Purpose: to fill the DayPilotCalendar1 calendar
  ' Parameters: compID
  ' Return: 
  '       DatTable
  ' Change Log
  '           3/22/2010    - Created By: Tom Jones
  ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
  Public Function Fill_DayPilotCalendar1(ByVal type_list As String) As Boolean
    Try
      Dim actionitemdate As String
      actionitemdate = (FormatDateTime(DateOfActionItem, DateFormat.ShortDate))
      DayPilotCalendar1.Visible = True
      record_count.Visible = False
      Previous_Button_Visible = False
      Next_Button_Visible = False
      Fill_DayPilotCalendar1 = False
      btnDayPilotCalendar_Next.Visible = True
      btnDayPilotCalendar_Previous.Visible = True
      ' create a string to hold the enddate
      Dim DayPilotCalendar1_EndDate As String = ""
      ' create a datatable to hold the search results
      Dim dtDayPilotCalendar1 As New DataTable
      ' set the end date
      Dim aDay As Integer = Day(DateOfActionItem) + 7
      Dim aMonth As String = ""
      Dim aYear As String = ""
      If CStr(DateOfActionItem) = "12:00:00 AM" Then
        DateOfActionItem = CStr(Now())
      End If
      If CStr(DateOfActionItem) = "" Then

        DateOfActionItem = CStr(Now())
        DateOfActionItem = DateOfActionItem.ToString.TrimEnd
        If type_list = "Week" Then
          DayPilotCalendar1_EndDate = Month(DateAdd(DateInterval.Day, 7, System.DateTime.Now)) & "/" & Day(DateAdd(DateInterval.Day, 7, System.DateTime.Now)) & "/" & Year(DateAdd(DateInterval.Day, 7, System.DateTime.Now))
        ElseIf type_list = "Month" Then

          DayPilotCalendar1_EndDate = Month(DateAdd(DateInterval.Month, 1, System.DateTime.Now)) & "/" & Day(DateAdd(DateInterval.Month, 1, System.DateTime.Now)) & "/" & Year(DateAdd(DateInterval.Month, 1, System.DateTime.Now))
        Else
          DayPilotCalendar1_EndDate = Month(DateAdd(DateInterval.Day, 1, System.DateTime.Now)) & "/" & Day(DateAdd(DateInterval.Day, 1, System.DateTime.Now)) & "/" & Year(DateAdd(DateInterval.Day, 1, System.DateTime.Now))
        End If

      Else
        If type_list = "Week" Then
          DayPilotCalendar1_EndDate = Month(DateAdd(DateInterval.Day, 7, DateOfActionItem)) & "/" & Day(DateAdd(DateInterval.Day, 7, DateOfActionItem)) & "/" & Year(DateAdd(DateInterval.Day, 7, DateOfActionItem))
        ElseIf type_list = "Month" Then
          DayPilotCalendar1_EndDate = Month(DateAdd(DateInterval.Month, 1, DateOfActionItem)) & "/" & Day(DateAdd(DateInterval.Month, 1, DateOfActionItem)) & "/" & Year(DateAdd(DateInterval.Month, 1, DateOfActionItem))
        Else
          DayPilotCalendar1_EndDate = Month(DateAdd(DateInterval.Day, 1, DateOfActionItem)) & "/" & Day(DateAdd(DateInterval.Day, 1, DateOfActionItem)) & "/" & Year(DateAdd(DateInterval.Day, 1, DateOfActionItem))
        End If
        ' DayPilotCalendar1_EndDate = aMonth & "/" & aDay & "/" & aYear
      End If
      ' check to see if DayPilotCalendar1_EndDate is a real date
      If Not IsDate(DayPilotCalendar1_EndDate) Then
        ' run a loop until we get a real date by moving aDay back by 
        Do While Not IsDate(DayPilotCalendar1_EndDate)
          ' move the day back by one
          aDay = aDay - 1
          ' reset the DayPilotCalendar1_EndDate to be checked at the top of the loop
          DayPilotCalendar1_EndDate = DateAdd(DateInterval.Day, 7, Now())
        Loop
      End If
      ' fill the datatable based on the date range
      dtDayPilotCalendar1 = aclsData_Temp.Get_Local_Notes_Schedule_Date_CalDisplay(CStr(DateOfActionItem), DayPilotCalendar1_EndDate, Session("timezone_offset"))

      If Not IsNothing(dtDayPilotCalendar1) Then
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("listing.aspx.vb - Fill_DayPilotCalendar1() - " & error_string)
        End If
        display_error()
      End If
      ' setup the number of days to display
      Select Case type_list
        Case "Week"
          DayPilotCalendar1.Days = 7
          DayPilotCalendar1.Visible = True
        Case "Day"
          DayPilotCalendar1.Days = 1
          DayPilotCalendar1.Visible = True
        Case "Month"
          DayPilotCalendar1.Days = 31
          DayPilotCalendar1.Visible = True

      End Select
      ' check the state of the datatable
      If Not IsNothing(dtDayPilotCalendar1) Then
        If dtDayPilotCalendar1.Rows.Count >= 0 Then
          ' set the back color
          DayPilotCalendar1.ShowToolTip = False
          DayPilotCalendar1.BackColor = Drawing.ColorTranslator.FromHtml("#D5E6F0")
          ' set the nonebusinessdays color
          DayPilotCalendar1.NonBusinessBackColor = Drawing.ColorTranslator.FromHtml("#B2D0F4")
          ' set the Hourbordercolor 'F0F0F0
          DayPilotCalendar1.HourBorderColor = Drawing.ColorTranslator.FromHtml("#F0F0F0")
          ' set the HourHalfBorderColor
          DayPilotCalendar1.HourHalfBorderColor = Drawing.ColorTranslator.FromHtml("#F0F0F0")
          ' set the bordercolor
          DayPilotCalendar1.BorderColor = Drawing.ColorTranslator.FromHtml("#F0F0F0")
          ' set the dataTextField
          DayPilotCalendar1.DataTextField = "lnote_note"
          ' set tthe DataValueField
          DayPilotCalendar1.DataValueField = "lnote_id"
          ' set the start date for the calenddar control
          DayPilotCalendar1.StartDate = CStr(DateOfActionItem)
          ' set the start field, from the notes table
          DayPilotCalendar1.DataStartField = "lnote_schedule_start_date"
          ' set the end field from the notes table
          DayPilotCalendar1.DataEndField = "lnote_schedule_end_date"
          ' set the data source
          DayPilotCalendar1.DataSource = dtDayPilotCalendar1
          ' bind the data
          DayPilotCalendar1.DataBind()
          Fill_DayPilotCalendar1 = True

          For Each x As DataRow In dtDayPilotCalendar1.Rows
            Dim showdate As String = Hour(CDate(x("lnote_schedule_start_date")))

            'If InStr(x("lnote_schedule_start_date"), "PM") Then
            '    Response.Write(showdate & "PM")
            'ElseIf InStr(x("lnote_schedule_start_date"), "AM") Then
            '    Response.Write(showdate & "AM")
            'End If

          Next
        Else
          ' Response.Write("Error in Fill_DayPilotCalendar1: datatable is nothing")
          ' Fill_DayPilotCalendar1 = False
          ' set the back color

          DayPilotCalendar1.BackColor = Drawing.ColorTranslator.FromHtml("#D5E6F0")
          ' set the nonebusinessdays color
          DayPilotCalendar1.NonBusinessBackColor = Drawing.ColorTranslator.FromHtml("#B2D0F4")
          ' set the Hourbordercolor 'F0F0F0
          DayPilotCalendar1.HourBorderColor = Drawing.ColorTranslator.FromHtml("#F0F0F0")
          ' set the HourHalfBorderColor
          DayPilotCalendar1.HourHalfBorderColor = Drawing.ColorTranslator.FromHtml("#F0F0F0")
          ' set the bordercolor
          DayPilotCalendar1.BorderColor = Drawing.ColorTranslator.FromHtml("#F0F0F0")
          ' set the dataTextField
          DayPilotCalendar1.DataTextField = "lnote_note"
          ' set tthe DataValueField
          DayPilotCalendar1.DataValueField = "lnote_id"
          ' set the start date for the calenddar control
          DayPilotCalendar1.StartDate = CStr(DateOfActionItem)
          ' set the start field, from the notes table
          DayPilotCalendar1.DataStartField = "lnote_schedule_start_date"
          ' set the end field from the notes table
          DayPilotCalendar1.DataEndField = "lnote_schedule_end_date"
          Fill_DayPilotCalendar1 = True
          For Each x As DataRow In dtDayPilotCalendar1.Rows
            Dim showdate As String = Hour(CDate(x("lnote_schedule_start_date")))

            'If InStr(x("lnote_schedule_start_date"), "PM") Then
            '    Response.Write(showdate & "PM")
            'ElseIf InStr(x("lnote_schedule_start_date"), "AM") Then
            '    Response.Write(showdate & "AM")
            'End If

          Next
        End If
      Else
        ' Response.Write("Error in Fill_DayPilotCalendar1: datatable is nothing")
        'Fill_DayPilotCalendar1 = False
        ' set the back color
        DayPilotCalendar1.BackColor = Drawing.ColorTranslator.FromHtml("#D5E6F0")
        ' set the nonebusinessdays color
        DayPilotCalendar1.NonBusinessBackColor = Drawing.ColorTranslator.FromHtml("#B2D0F4")
        ' set the Hourbordercolor 'F0F0F0
        DayPilotCalendar1.HourBorderColor = Drawing.ColorTranslator.FromHtml("#F0F0F0")
        ' set the HourHalfBorderColor
        DayPilotCalendar1.HourHalfBorderColor = Drawing.ColorTranslator.FromHtml("#F0F0F0")
        ' set the bordercolor
        DayPilotCalendar1.BorderColor = Drawing.ColorTranslator.FromHtml("#F0F0F0")
        ' set the dataTextField
        DayPilotCalendar1.DataTextField = "lnote_note"
        ' set tthe DataValueField
        DayPilotCalendar1.DataValueField = "lnote_id"
        ' set the start date for the calenddar control
        DayPilotCalendar1.StartDate = CStr(DateOfActionItem)
        ' set the start field, from the notes table
        DayPilotCalendar1.DataStartField = "lnote_schedule_start_date"
        ' set the end field from the notes table
        DayPilotCalendar1.DataEndField = "lnote_schedule_end_date"
        Fill_DayPilotCalendar1 = True
      End If
      ' DayPilotCalendar1.EventClickJavaScript() = "DayPilotCalendar1_EventClick"
      'Session("DayPilotCalendar1_startDate") = ""
    Catch ex As Exception
      Fill_DayPilotCalendar1 = False
      error_string = "main_site.Master.vb - Fill_DayPilotCalendar1() - " & ex.Message
      LogError(error_string)
    End Try
  End Function
#Region "These functions deal with the jobs listing page exclusively. This is how we parse the info to not double show"
  Public keep As Integer = 0
  Public change As Boolean = False
  Public run_total = 1
  Function evalme(ByVal y As Integer)
    evalme = ""
    Try
      'This takes a contact ID. If the job seeker has already been shown, this function filters out some of the info so you don't have to rewrite it. 

      ''If run_total = 3 Then
      If y <> keep Then
        change = True
        keep = y
      Else
        keep = y
        change = False
      End If
      'Else
      'run_total = run_total + 1
      'End If
    Catch ex As Exception
      error_string = "main_site.Master.vb - evalme() - " & ex.Message
      LogError(error_string)
    End Try

  End Function
  Function showme(ByVal x As String)
    showme = ""
    Try
      'This one is going to have to be looked at later.
      'This takes a job seeker ID. If the job seeker has already been shown, this function filters out some of the info so you don't have to rewrite it. 
      If Not IsDBNull(x) Then
        If change = True Then
          showme = x
        Else
          showme = ""
        End If

      End If
    Catch ex As Exception
      error_string = "main_site.Master.vb - showme() - " & ex.Message
      LogError(error_string)
    End Try
  End Function
  Function NoteAttachedACComp(ByVal x As Object, ByVal y As Object, ByVal z As Integer, ByVal q As String, ByVal contact_id As Integer, ByVal jetnetorclientnote As String)
    NoteAttachedACComp = ""
    Try
      If Not IsDBNull(jetnetorclientnote) Then
        If jetnetorclientnote = "JETNET" Then
          If Not IsDBNull(x) And Not IsDBNull(y) Then
            Dim da As String = Month(Now()) & "/" & Day(Now()) & "/" & Year(Now())
            Dim order_by As String = ""

            If q = "P" Then
              order_by = "lnote_schedule_start_date desc"
            Else
              order_by = "lnote_entry_date desc"
            End If

            x = x.ToString
            y = y.ToString

            aTempTable = New DataTable
            If z = 2 Then 'ac
              aTempTable = aclsData_Temp.DUAL_Notes_LIMIT("AC", x, q, UCase(y), da, order_by, 1)
            Else
              aTempTable = aclsData_Temp.DUAL_Notes_LIMIT("COMP", x, q, UCase(y), da, order_by, 1)

              'If q = "P" Then
              '    If UCase(y) = "CLIENT" Then
              '        aTempTable = .Get_CLIENT_RECENT_COMPANY_ACTION(da, x)
              '    Else
              '        aTempTable = .Get_JETNET_RECENT_COMPANY_ACTION(da, x)
              '    End If
              'Else

              '    If UCase(y) = "CLIENT" Then
              '        aTempTable = .Get_Local_Notes_Client_Comp(x, q)
              '    Else
              '        aTempTable = .Get_Local_Notes_JETNET_Comp(x, q)
              '    End If
              'End If
            End If

            If Not IsNothing(aTempTable) Then
              If aTempTable.Rows.Count >= 1 Then
                If contact_id = 0 Then
                  If q = "A" Then
                    NoteAttachedACComp = "<img src='images/document.png' alt='Notes Attached to Aircraft' border='0' />"
                  Else
                    NoteAttachedACComp = "<img src='images/red_pin.png' alt='Action Items Attached to Company' border='0' />"
                  End If
                Else
                  For Each r As DataRow In aTempTable.Rows
                    If r("lnote_client_contact_id") = contact_id Then

                      If q = "A" Then
                        NoteAttachedACComp = "<img src='images/document.png' alt='Notes Attached to Aircraft' border='0' />"
                      Else
                        NoteAttachedACComp = "<img src='images/red_pin.png' alt='Action Items Attached to Company' border='0' />"
                      End If

                    End If
                  Next
                End If

              End If
            Else
              If aclsData_Temp.class_error <> "" Then
                error_string = aclsData_Temp.class_error
                LogError("main_site.Master.vb - NoteAttachedACComp() - " & error_string)
              End If
              display_error()
            End If

          End If
        Else
          If q = "A" Then
            NoteAttachedACComp = "<img src='images/document.png' alt='Notes Attached to Aircraft' border='0' />"
          Else
            NoteAttachedACComp = "<img src='images/red_pin.png' alt='Action Items Attached to Company' border='0' />"
          End If
        End If
      End If
    Catch ex As Exception
      error_string = "main_site.Master.vb - NoteAttachedACComp() - " & ex.Message
      LogError(error_string)
    End Try
  End Function
  Public eventactext As String = ""
  Function ViewPriorityEventsClient(ByVal eventstring As Object, ByVal acID As Integer) As String
    ViewPriorityEventsClient = ""
    'If PerformDatabaseAction = True Then
    eventactext = ""
    If Not IsDBNull(eventstring) Then
      eventstring = eventstring.ToString
      'If eventstring <> "CLIENT" Then
      'eventactext = eventstring
      'Else
      Dim answer As String = ""
      If acID > 0 Then
        answer = aclsData_Temp.PriorityEventsClient(acID, Session.Item("localSubscription").crmAerodexFlag)
        If Not IsNothing(aTempTable) Then
          If answer <> "" Then
            eventactext = answer
          Else
            eventactext = ""
          End If
        End If

      End If
      ' End If
    End If
  End Function
  Public noteactext As String = ""
  Function ViewNoteAttachedACComp(ByVal x As Object, ByVal y As Object, ByVal z As Integer, ByVal q As String, ByVal contact_id As Integer, ByVal jetnetorclientnote As Object)
    ViewNoteAttachedACComp = ""
    noteactext = ""
    'x = id
    'y = source
    'z = 2 aircraft, 1 company
    'q = status 
    Dim da As String = Month(Now()) & "/" & Day(Now()) & "/" & Year(Now())
    aTempTable = Nothing
    Try
      If Not IsDBNull(jetnetorclientnote) Then
        If jetnetorclientnote = "JETNET" Then
          If Not IsDBNull(x) Then
            x = x.ToString
            y = IIf(Not IsDBNull(y), y, "JETNET")

            Dim lnote_order As String = "lnote_entry_date desc"
            If q = "A" Then
              lnote_order = "lnote_entry_date desc "
            Else
              lnote_order = "lnote_schedule_start_date asc "
            End If

            If z = 2 Then 'ac
              aTempTable = aclsData_Temp.DUAL_Notes_LIMIT("AC", x, q, UCase(y), da, lnote_order, 1)
            Else
              aTempTable = aclsData_Temp.DUAL_Notes_LIMIT("COMP", x, q, UCase(y), da, lnote_order, 1, "", contact_id)
            End If
          End If

          Dim note_text As String = ""
          If Not IsNothing(aTempTable) Then
            If aTempTable.Rows.Count >= 1 Then
              If contact_id = 0 Then
                If q = "P" Then
                  note_text = "<strong>" & DateAdd("h", Session("timezone_offset"), aTempTable.Rows(0).Item("lnote_schedule_start_date")) & " "
                Else
                  note_text = "<strong>" & DateAdd("h", Session("timezone_offset"), aTempTable.Rows(0).Item("lnote_entry_date")) & " "
                End If

                If Not IsDBNull(aTempTable.Rows(0).Item("lnote_user_name")) Then
                  If aTempTable.Rows(0).Item("lnote_user_name") <> "" Then
                    note_text = note_text & "(<em>Entered by: " & aTempTable.Rows(0).Item("lnote_user_name") & "</em>)"
                  End If
                End If
                note_text = note_text & "</strong><br />"

                note_text = note_text & aTempTable.Rows(0).Item("lnote_note")
              Else
                For Each r As DataRow In aTempTable.Rows
                  If (aTempTable.Rows(0).Item("lnote_jetnet_contact_id") = contact_id) Or (aTempTable.Rows(0).Item("lnote_client_contact_id") = contact_id) Then

                    If q = "P" Then
                      note_text = "<strong>" & DateAdd("h", Session("timezone_offset"), r("lnote_schedule_start_date")) & " "
                    Else
                      note_text = "<strong>" & DateAdd("h", Session("timezone_offset"), r("lnote_entry_date")) & " "
                    End If

                    If Not IsDBNull(r("lnote_user_name")) Then
                      If r("lnote_user_name") <> "" Then
                        note_text = note_text & "(<em>Entered by: " & r("lnote_user_name") & "</em>)"
                      End If
                    End If
                    note_text = note_text & "</strong><br />"

                    note_text = note_text & Server.HtmlDecode(r("lnote_note"))
                  End If
                Next
              End If

            Else
              If aclsData_Temp.class_error <> "" Then
                error_string = aclsData_Temp.class_error
                LogError("main_site.Master.vb - ViewNoteAttachedACComp() - " & error_string)
              End If
              display_error()
            End If
            'ViewNoteAttachedACComp = note_text
            noteactext = note_text
          End If
        Else
          'ViewNoteAttachedACComp = jetnetorclientnote
          noteactext = jetnetorclientnote
        End If
      End If
    Catch ex As Exception
      error_string = "main_site.Master.vb - ViewNoteAttachedACComp() - " & ex.Message
      LogError(error_string)
    End Try

  End Function
#End Region
#Region "Functions that figure out what ac, what contact, what model, what user, what note category, or what company based on ID"
  Function createANoteAddressPopOut(ByVal x As Object, ByVal y As Object) As String
    createANoteAddressPopOut = ""
    Try
      If Not IsDBNull(x) And Not IsDBNull(y) Then
        If x <> 0 Then
          createANoteAddressPopOut = createAnAddressPopOut(x, "JETNET")
        Else
          createANoteAddressPopOut = createAnAddressPopOut(y, "CLIENT")
        End If
      End If
    Catch ex As Exception
      error_string = "main_site.Master.vb - createANoteAddressPopOut() - " & ex.Message
      LogError(error_string)
    End Try
  End Function
  Public company_listing_text As String = ""
  Function createAnAddressPopOut(ByVal x As Object, ByVal y As Object) As String
    createAnAddressPopOut = ""
    company_listing_text = ""
    'If PerformDatabaseAction = True Then
    Try
      Dim address As String = ""
      Dim address_hold As String = ""
      Dim description As String = ""
      y = UCase(y)

      If Not IsDBNull(x) And Not IsDBNull(y) Then
        aTempTable = aclsData_Temp.GetCompanyInfo_ID(x, y, 0)
      Else
        aTempTable = New DataTable
      End If
      ' check the state of the DataTable
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then

          For Each R As DataRow In aTempTable.Rows
            If Not IsDBNull(R("comp_name")) Then
              address_hold = "<strong style='font-size:14px;color:#67A0D9;'>" & R("comp_name") & "</strong><br />"
            End If
            company_listing_text = "<a href='details.aspx?source=" & y & "&comp_ID=" & x & "&type=1'>" & Replace(Replace(address_hold, "<strong style='font-size:14px;color:#67A0D9;'>", ""), "</strong>", "") & "</a>"

            If Not IsDBNull(R("comp_address1")) Then
              If Trim(R("comp_address1")) <> "" Then
                address = address & R("comp_address1") & "<br />"
              End If
            End If
            If Not IsDBNull(R("comp_address2")) Then
              If Trim(R("comp_address2")) <> "" Then
                address = address & R("comp_address2") & "<br />"
              End If
            End If

            If Not IsDBNull(R("comp_city")) Then
              If Trim(R("comp_city")) <> "" Then
                address = address & R("comp_city") & ", "
              End If
            End If
            If Not IsDBNull(R("comp_state")) Then
              If Trim(R("comp_state")) <> "" Then
                address = address & R("comp_state") & " "
              End If
            End If

            If Not IsDBNull(R("comp_zip_code")) Then
              If Trim(R("comp_zip_code")) <> "" Then
                address = address & R("comp_zip_code") & "<br />"
              Else
              End If
            Else
            End If

            If Not IsDBNull(R("comp_country")) Then
              If Trim(R("comp_country")) <> "" Then
                address = address & R("comp_country") & "<br />"
              Else
              End If
            Else
            End If

            If Not IsDBNull(R("comp_email_address")) Then
              If Trim(R("comp_email_address")) <> "" Then
                address = address & "<a href='mailto:" & R("comp_email_address") & "' class='non_special_link'>" & R("comp_email_address") & "</a>"
              Else
              End If
            Else
            End If
            company_listing_text = company_listing_text & address

            address = address_hold & address
            'If y = "CLIENT" Then
            '    If Not IsDBNull(R("clicomp_description")) Then
            '        If Trim(R("clicomp_description")) <> "" Then
            '            description = "<strong>Description:</strong> " & R("clicomp_description") & "<br />"
            '        Else
            '        End If
            '    Else
            '    End If
            'End If

          Next

          '------Phone Company Information Left Card Display----------------------------------------------------------------------
          Try

            aTempTable = aclsData_Temp.GetPhoneNumbers(x, 0, y, 0)
            '' check the state of the DataTable
            If Not IsNothing(aTempTable) Then
              If aTempTable.Rows.Count > 0 Then
                address = address & "<br /><strong style='font-size:12px;color:#4d7997;'>Phone Numbers</strong><br />"
                ' set it to the datagrid 
                For Each q As DataRow In aTempTable.Rows
                  address = address & q("pnum_type") & ": " & q("pnum_number") & "<br />"
                Next
              Else
                'rows = 0
                address = address & ""
              End If
            Else
              If aclsData_Temp.class_error <> "" Then
                error_string = aclsData_Temp.class_error
                LogError("main_site.Master.vb - createAnAddressPopOut() - " & error_string)
              End If
              display_error()
            End If
          Catch ex As Exception
            error_string = "main_site.Master.vb - createAnAddressPopOut() - " & ex.Message
            LogError(error_string)
          End Try

        Else
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("main_site.Master.vb - createAnAddressPopOut() - " & error_string)
        End If
        display_error()
      End If

      If address <> "" Then
        address = UCase(address.TrimEnd("<br />"))
      End If

      If description <> "" Then
        createAnAddressPopOut = address & "<br /><br />" & description
      Else
        createAnAddressPopOut = address
      End If
    Catch ex As Exception
      error_string = "main_site.Master.vb - createAnAddressPopOut() - " & ex.Message
      LogError(error_string)
    End Try
    'End If
  End Function
  Function createANoteContactPopOut(ByVal x As Object, ByVal y As Object, ByVal z As Object, ByVal q As Object) As String
    createANoteContactPopOut = ""
    Try
      If Not IsDBNull(x) And Not IsDBNull(y) And Not IsDBNull(z) And Not IsDBNull(q) Then
        If x <> 0 Then
          createANoteContactPopOut = createAContactPopOut(x, "JETNET", z)
        Else
          createANoteContactPopOut = createAContactPopOut(y, "CLIENT", q)
        End If
      End If
    Catch ex As Exception
      error_string = "main_site.Master.vb - createANoteContactPopOut() - " & ex.Message
      LogError(error_string)
    End Try
  End Function
  Function createaNoteACPopOut(ByVal x As Object, ByVal y As Object) As String
    createaNoteACPopOut = ""
    Try
      If Not IsDBNull(x) And Not IsDBNull(y) Then
        If x <> 0 Then
          createaNoteACPopOut = createanACPopOut(x, "JETNET")
        Else
          createaNoteACPopOut = createanACPopOut(y, "CLIENT")
        End If
      End If
    Catch ex As Exception
      error_string = "main_site.Master.vb - createaNoteACPopOut() - " & ex.Message
      LogError(error_string)
    End Try
  End Function
  Public aircraft_listing_text As String = ""
  Function createanACPopOut(ByVal idnum As Integer, ByVal source As String) As String
    Dim aircraft_text As String = ""
    aircraft_listing_text = ""
    Try
      If UCase(source) = "CLIENT" Then
        aTempTable = aclsData_Temp.Get_Clients_Aircraft(idnum)
        ' check the state of the DataTable
        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            Dim r As DataRow = aTempTable.Rows(0)

            aTempTable2 = aclsData_Temp.Get_Clients_Aircraft_Model_amodID(r("cliaircraft_cliamod_id"))
            If Not IsNothing(aTempTable2) Then
              If aTempTable2.Rows.Count > 0 Then
                For Each q As DataRow In aTempTable2.Rows
                  aircraft_text = q("cliamod_make_name") & " " & q("cliamod_model_name") & "<br />"
                Next
              End If
            Else
              If aclsData_Temp.class_error <> "" Then
                error_string = aclsData_Temp.class_error
                LogError("main_site.Master.vb - createanACPopOut() - " & error_string)
              End If
              display_error()
            End If


            If Not IsDBNull(r("cliaircraft_year_mfr")) Then
              If r("cliaircraft_year_mfr") <> "" Then
                aircraft_text = aircraft_text & "Year: " & r("cliaircraft_year_mfr") & "<br />"
              End If
            End If
            If Not IsDBNull(r("cliaircraft_reg_nbr")) Then
              If r("cliaircraft_reg_nbr") <> "" Then
                aircraft_text = aircraft_text & "Reg #: " & r("cliaircraft_reg_nbr") & "<br />"
              End If
            End If

            If Not IsDBNull(r("cliaircraft_ser_nbr")) Then
              If r("cliaircraft_ser_nbr") <> "" Then
                aircraft_text = aircraft_text & "Ser #: " & r("cliaircraft_ser_nbr") & "<br />"
              End If
            End If

            aircraft_listing_text = "<a href='details.aspx?source=CLIENT&ac_ID=" & idnum & "&type=3'>" & aircraft_text & "</a>"

            If Not IsDBNull(r("cliaircraft_date_purchased")) Then
              If CStr(r("cliaircraft_date_purchased")) <> "1/1/1900" Then
                aircraft_text = aircraft_text & "</b>Purchased: " & r("cliaircraft_date_purchased") & "<br />"
              End If
            End If

            If Session.Item("localSubscription").crmAerodexFlag = True Then

            Else
              If r("cliaircraft_forsale_flag") = "Y" Then
                aircraft_text = aircraft_text & "<b class='green'>" & r("cliaircraft_status")
                If Not IsDBNull(r("cliaircraft_delivery")) Then
                  If r("cliaircraft_delivery") <> "" Then
                    aircraft_text = aircraft_text & " - " & r("cliaircraft_delivery")
                  End If
                End If
                If Not IsDBNull(r("cliaircraft_asking_wordage")) Then
                  If r("cliaircraft_asking_wordage") <> "" Then
                    If r("cliaircraft_asking_wordage") = "Price" Then
                      If Not IsDBNull(r("cliaircraft_asking_price")) Then
                        Dim asking_price As String = clsGeneral.clsGeneral.no_zero(r("cliaircraft_asking_price"), "", True)
                        If asking_price <> "" Then
                          aircraft_text = aircraft_text & " Asking: " & asking_price
                        End If

                      End If
                    Else
                      aircraft_text = aircraft_text & " " & r("cliaircraft_asking_wordage")
                    End If
                  End If

                End If
                aircraft_text = aircraft_text & "</b><br />"
              End If


              If Not IsDBNull(r("cliaircraft_est_price")) Then
                Dim take_price As String = clsGeneral.clsGeneral.no_zero(r("cliaircraft_est_price"), "", True)
                If take_price <> "" Then
                  aircraft_text = aircraft_text & "Take Price: " & take_price & "<br />"
                End If
              End If

              If Not IsDBNull(r("cliaircraft_date_listed")) Then
                Dim date_listed As String = clsGeneral.clsGeneral.datenull(r("cliaircraft_date_listed"))
                If date_listed <> "" Then
                  aircraft_text = aircraft_text & "List Date: " & date_listed & "<br />"
                End If
              End If


              If Not IsDBNull(r("cliaircraft_date_listed")) Then
                Dim date_listed As String = clsGeneral.clsGeneral.datenull(r("cliaircraft_date_listed"))
                If date_listed <> "" Then
                  aircraft_text = aircraft_text & clsGeneral.clsGeneral.trans_date_diff(Now(), r("cliaircraft_date_listed"), 2) & "<br />"
                End If
              End If


              If Not IsDBNull(r("cliaircraft_status")) Then
                If r("cliaircraft_status") <> "" Then
                  Select Case r("cliaircraft_status")
                    Case "For Sale"
                    Case Else
                      aircraft_text = aircraft_text & r("cliaircraft_status") & "<br />"
                  End Select
                End If
              End If
              If Not IsDBNull(r("cliaircraft_exclusive_flag")) Then
                If r("cliaircraft_exclusive_flag") <> "" Then
                  aircraft_text = aircraft_text & clsGeneral.clsGeneral.yes_no(r("cliaircraft_exclusive_flag"), "exclusive")
                End If
              End If
              If Not IsDBNull(r("cliaircraft_lease_flag")) Then
                If r("cliaircraft_lease_flag") <> "" Then
                  aircraft_text = aircraft_text & clsGeneral.clsGeneral.yes_no(r("cliaircraft_lease_flag"), "leased")
                End If
              End If
            End If

          End If
        End If
      ElseIf UCase(source) = "JETNET" Then
        aTempTable = aclsData_Temp.GetJETNET_Aircraft_Details_AC_ID(idnum, "")
        ' check the state of the DataTable
        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            Dim r As DataRow = aTempTable.Rows(0)
            aircraft_text = aircraft_text & r("amod_make_name") & " " & r("amod_model_name") & "<br />"
            If Not IsDBNull(r("ac_year_mfr")) Then
              If r("ac_year_mfr") <> "" Then
                aircraft_text = aircraft_text & "Year: " & r("ac_year_mfr") & "<br />"
              End If
            End If
            If Not IsDBNull(r("ac_year_dlv")) Then
              aircraft_text = aircraft_text & "Delivered: " & r("ac_year_dlv") & "<br />"
            End If
            If Not IsDBNull(r("ac_date_purchased")) Then
              aircraft_text = aircraft_text & "Purchased: " & r("ac_date_purchased") & "<br />"
            End If
            If Not IsDBNull(r("ac_reg_nbr")) Then
              If r("ac_reg_nbr") <> "" Then
                aircraft_text = aircraft_text & "Reg #: " & r("ac_reg_nbr") & "<br />"
              End If
            End If
            If Not IsDBNull(r("ac_prev_reg_nbr")) Then
              If r("ac_prev_reg_nbr") <> "" Then
                aircraft_text = aircraft_text & "Previous Reg #: " & r("ac_prev_reg_nbr") & "<br />"
              End If
            End If
            If Not IsDBNull(r("ac_ser_nbr")) Then
              If r("ac_ser_nbr") <> "" Then
                aircraft_text = aircraft_text & "Ser #: " & r("ac_ser_nbr") & "<br />"
              End If
            End If
            If Not IsDBNull(r("ac_alt_ser_nbr")) Then
              If r("ac_alt_ser_nbr") <> "" Then
                aircraft_text = aircraft_text & "Alt. Ser #: " & r("ac_alt_ser_nbr") & "<br />"
              End If
            End If

            aircraft_listing_text = "<a href='details.aspx?source=JETNET&ac_ID=" & idnum & "&type=3'>" & aircraft_text & "</a>"
            If Session.Item("localSubscription").crmAerodexFlag = True Then

            Else
              If r("ac_forsale_flag") = "Y" Then
                aircraft_text = aircraft_text & "<b class='green'>" & r("ac_status")
                If Not IsDBNull(r("ac_delivery")) Then
                  If r("ac_delivery") <> "" Then
                    aircraft_text = aircraft_text & " - " & r("ac_delivery")
                  End If
                End If
                If Not IsDBNull(r("ac_asking_wordage")) Then
                  If r("ac_asking_wordage") <> "" Then
                    If r("ac_asking_wordage") = "Price" Then
                      If Not IsDBNull(r("ac_asking_price")) Then
                        Dim asking_price As String = clsGeneral.clsGeneral.no_zero(r("ac_asking_price"), "", True)
                        If asking_price <> "" Then
                          aircraft_text = aircraft_text & " Asking: " & asking_price
                        End If
                      End If
                    Else
                      aircraft_text = aircraft_text & " " & r("ac_asking_wordage")
                    End If
                  End If
                End If
                aircraft_text = aircraft_text & "</b><br />"
              End If


              If Not IsDBNull(r("ac_date_listed")) Then
                Dim date_listed As String = clsGeneral.clsGeneral.datenull(r("ac_date_listed"))
                If date_listed <> "" Then
                  aircraft_text = aircraft_text & "List Date: " & date_listed & "<br />"
                End If
              End If


              If Not IsDBNull(r("ac_date_listed")) Then
                Dim date_listed As String = clsGeneral.clsGeneral.datenull(r("ac_date_listed"))
                If date_listed <> "" Then
                  aircraft_text = aircraft_text & clsGeneral.clsGeneral.trans_date_diff(Now(), r("ac_date_listed"), 2) & "<br />"
                End If
              End If

              If Not IsDBNull(r("ac_status")) Then
                If r("ac_status") <> "" Then
                  Select Case r("ac_status")
                    Case "For Sale"
                    Case Else
                      aircraft_text = aircraft_text & r("ac_status") & "<br />"
                  End Select
                End If
              End If
            End If

            If Not IsDBNull(r("ac_lifecycle")) Then
              Select Case r("ac_lifecycle")
                Case "1"
                  aircraft_text = aircraft_text & "In Production<br />"
                Case "2"
                  aircraft_text = aircraft_text & "New<br />"
                Case "3"
                  aircraft_text = aircraft_text & "In Operation<br />"
                Case "4"
                  aircraft_text = aircraft_text & "Retired<br />"
              End Select
            End If
            aircraft_text = aircraft_text & "Airport: "
            If Not IsDBNull(r("ac_aport_iata_code")) Then
              If r("ac_aport_iata_code") <> "" Then
                aircraft_text = aircraft_text & " - " & r("ac_aport_iata_code")
              End If
            End If
            If Not IsDBNull(r("ac_aport_icao_code")) Then
              If r("ac_aport_icao_code") <> "" Then
                aircraft_text = aircraft_text & " - " & r("ac_aport_icao_code")
              End If
            End If
            If Not IsDBNull(r("ac_aport_name")) Then
              If r("ac_aport_name") <> "" Then
                aircraft_text = aircraft_text & " - " & r("ac_aport_name")
              End If
            End If
            If Not IsDBNull(r("ac_aport_state")) Then
              If r("ac_aport_state") <> "" Then
                aircraft_text = aircraft_text & " - " & r("ac_aport_state")
              End If
            End If
            If Not IsDBNull(r("ac_aport_country")) Then
              If r("ac_aport_country") <> "" Then
                aircraft_text = aircraft_text & " - " & r("ac_aport_country")
              End If
            End If
            If Not IsDBNull(r("ac_aport_city")) Then
              If r("ac_aport_city") <> "" Then
                aircraft_text = aircraft_text & " - " & r("ac_aport_city")
              End If
            End If
            If Not IsDBNull(r("ac_aport_private")) Then
              If r("ac_aport_private") = "Y" Then
                aircraft_text = aircraft_text & " <em>(Private Airport)</em><br />"
              Else
                aircraft_text = aircraft_text & " <em>(Non-Private Airport)</em><br />"
              End If
            End If
            If Session.Item("localSubscription").crmAerodexFlag = True Then

            Else
              aircraft_text = aircraft_text & clsGeneral.clsGeneral.yes_no(r("ac_exclusive_flag"), "previous")
            End If
            If Not IsDBNull(r("ac_ownership")) Then
              If r("ac_ownership") <> "" Then
                Select Case r("ac_ownership")
                  Case "W"
                    aircraft_text = aircraft_text & "Wholly Owned<br />"
                  Case "F"
                    aircraft_text = aircraft_text & "Fractionally Owned<br />"
                  Case "C"
                    aircraft_text = aircraft_text & "Co-Owned<br />"
                End Select
              End If
            End If
            If Session.Item("localSubscription").crmAerodexFlag = True Then

            Else
              If r("ac_exclusive_flag") <> "" Then
                aircraft_text = aircraft_text & clsGeneral.clsGeneral.yes_no(r("ac_exclusive_flag"), "exclusive")
              End If
              If r("ac_lease_flag") <> "" Then
                aircraft_text = aircraft_text & clsGeneral.clsGeneral.yes_no(r("ac_lease_flag"), "leased")
                Try
                  If r("ac_lease_flag") = "Y" Then
                    aTempTable2 = aclsData_Temp.GetAircraft_Lease_acID_ExpFlag(idnum, "N", 0)
                    ' check the state of the DataTable
                    If Not IsNothing(aTempTable2) Then
                      If aTempTable2.Rows.Count > 0 Then
                        For Each q As DataRow In aTempTable2.Rows
                          aircraft_text = aircraft_text & ": "
                          If Not IsDBNull(q("aclease_term")) Then
                            aircraft_text = aircraft_text & "Term " & q("aclease_term")
                          End If
                          If Not IsDBNull(q("aclease_date_expiration")) Then
                            aircraft_text = aircraft_text & " Expires " & q("aclease_date_expiration")
                          End If
                          If Not IsDBNull(q("aclease_note")) Then
                            If q("aclease_note") <> "" Then
                              aircraft_text = aircraft_text & " - " & q("aclease_note")
                            End If
                          End If
                          aircraft_text = aircraft_text & "</span>"
                        Next
                      Else '0 rows
                      End If
                    Else
                      If aclsData_Temp.class_error <> "" Then
                        error_string = aclsData_Temp.class_error
                        LogError("main_site.Master.vb - createanACPopOut() - " & error_string)
                      End If
                      display_error()
                    End If
                  End If
                Catch ex As Exception
                  error_string = "main_site.Master.vb - createanACPopOut() - " & ex.Message
                  LogError(error_string)
                End Try
              End If
            End If

          Else
            ' 0 rows
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("main_site.Master.vb - createanACPopOut() - " & error_string)
          End If
          display_error()
        End If
      End If

    Catch ex As Exception

      error_string = "main_site.Master.vb - createanACPopOut() - " & ex.Message
      LogError(error_string)
    End Try

    Return aircraft_text

  End Function

  Function createAContactPopOut(ByVal x As Object, ByVal y As Object, ByVal z As Object) As String
    'x = contact id, y = type, z = comp id
    createAContactPopOut = ""
    Try
      Dim contact_text As String = ""
      If Not IsDBNull(x) And Not IsDBNull(y) Then
        y = UCase(y)
        Try

          aTempTable = aclsData_Temp.GetContacts_Details(x, y)
          ' check the state of the DataTable
          If Not IsNothing(aTempTable) Then
            If aTempTable.Rows.Count > 0 Then
              For Each R As DataRow In aTempTable.Rows
                If Not IsDBNull(R("contact_first_name")) Then
                  contact_text = "<strong style='font-size:14px;color:#67A0D9;'>" & R("contact_first_name")
                End If
                If Not IsDBNull(R("contact_middle_initial")) Then
                  contact_text = contact_text & " " & R("contact_middle_initial")
                End If
                If Not IsDBNull(R("contact_last_name")) Then
                  contact_text = contact_text & " " & R("contact_last_name") & "</strong><br />"
                End If
                If y = "CLIENT" Then
                  If Not IsDBNull(R("contact_preferred_name")) Then
                    If Trim(R("contact_preferred_name")) <> "" Then
                      contact_text = contact_text & "Preferred Name: " & R("contact_preferred_name") & "<br />"
                    End If
                  End If
                End If
                If Not IsDBNull(R("contact_title")) Then
                  contact_text = contact_text & R("contact_title") & " <br />"
                End If
                If Not IsDBNull(R("contact_email_address")) Then
                  contact_text = contact_text & "<a href='mailto:" & R("contact_email_address") & "' class='non_special_link'>" & R("contact_email_address") & "</a>"
                End If

                If y = "CLIENT" Then
                  If Not IsDBNull(R("contact_notes")) Then
                    contact_text = contact_text & "<br /><em>" & R("contact_notes") & "</em>"
                  End If
                End If
              Next
            Else
              'zero rows
            End If
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = aclsData_Temp.class_error
              LogError("main_site.Master.vb - createAContactPopOut() - " & error_string)
            End If
            display_error()
          End If

        Catch ex As Exception
          error_string = "main_site.Master.vb - createAContactPopOut() - " & ex.Message
          LogError(error_string)
        End Try
        '------Contact Information Phone Numbers(if a contact ID is clicked display)----------------------------------------------------------------------
        Try
          aTempTable = aclsData_Temp.GetPhoneNumbers(z, x, y, 0)
          '' check the state of the DataTable
          If Not IsNothing(aTempTable) Then
            If aTempTable.Rows.Count > 0 Then
              ' set it to the datagrid 
              contact_text = contact_text & "<br /><strong style='font-size:12px;color:#4d7997;'>Phone Numbers</strong><br />"
              For Each q As DataRow In aTempTable.Rows
                contact_text = contact_text & q("pnum_type") & ": " & q("pnum_number") & "<br />"
              Next
            Else
            End If
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = aclsData_Temp.class_error
              LogError("main_site.Master.vb - createAContactPopOut() - " & error_string)
            End If
            display_error()
          End If

        Catch ex As Exception
          error_string = "main_site.Master.vb - createAContactPopOut() - " & ex.Message
          LogError(error_string)
        End Try
      End If
      If contact_text <> "" Then
        contact_text = UCase(contact_text.TrimEnd("<br>"))
      End If
      If contact_text <> "" Then
        contact_text = UCase(contact_text.TrimEnd("<br />"))
      End If

      Return contact_text
    Catch ex As Exception
      error_string = "main_site.Master.vb - createAContactPopOut() - " & ex.Message
      LogError(error_string)
    End Try

  End Function


  Public ContactPhone As String = ""
  Function createAContactPopOutPhone(ByVal x As Object, ByVal y As Object, ByVal z As Object) As String
    createAContactPopOutPhone = ""
    ContactPhone = ""
    If PerformDatabaseAction = True Then
      createAContactPopOutPhone = ""
      ContactPhone = ""
      Try
        If Not IsDBNull(x) And Not IsDBNull(y) Then
          y = UCase(y)
          '------Contact Information Phone Numbers(if a contact ID is clicked display)----------------------------------------------------------------------
          Try
            aTempTable = aclsData_Temp.GetPhoneNumbers(z, x, y, 0)
            '' check the state of the DataTable
            If Not IsNothing(aTempTable) Then
              If aTempTable.Rows.Count > 0 Then
                ' set it to the datagrid 

                ContactPhone = ContactPhone & "<strong style='font-size:12px;color:#4d7997;'>Phone Numbers</strong><br />"
                For Each q As DataRow In aTempTable.Rows
                  ContactPhone = ContactPhone & q("pnum_type") & ": " & q("pnum_number") & "<br />"
                Next
              Else
              End If
            Else
              If aclsData_Temp.class_error <> "" Then
                error_string = aclsData_Temp.class_error
                LogError("main_site.Master.vb - createAContactPopOut() - " & error_string)
              End If
              display_error()
            End If

          Catch ex As Exception
            error_string = "main_site.Master.vb - createAContactPopOut() - " & ex.Message
            LogError(error_string)
          End Try
        End If
        If ContactPhone <> "" Then
          ContactPhone = UCase(ContactPhone.TrimEnd("<br>"))
        End If
        If ContactPhone <> "" Then
          ContactPhone = UCase(ContactPhone.TrimEnd("<br />"))
        End If

        ' Return ContactPhone
      Catch ex As Exception
        error_string = "main_site.Master.vb - createAContactPopOut() - " & ex.Message
        LogError(error_string)
      End Try
    End If
  End Function
  Public broker As Boolean = False
  Public broker_text As String = ""
  Function createExclusiveBroker(ByVal sernum As Object, ByVal jetnet_ac As Object, ByVal source As Object, ByVal id As Object, ByVal flag As Object, ByVal flag2 As Object) As String
    ' If PerformDatabaseAction = True Then
    broker_text = ""
    broker = False
    Dim strContact As String = ""
    broker = False
    Try

      Dim compare As String = "N"

      If Not IsDBNull(flag) Then
        compare = flag
      ElseIf Not IsDBNull(flag2) Then
        compare = flag2
      End If
      If Not IsDBNull(compare) Then
        If compare = "Y" Then
          broker = True
        Else
          broker = False
        End If
      End If
    Catch ex As Exception
      error_string = "main_site.Master.vb - createExclusiveBroker() - " & ex.Message
      LogError(error_string)
    End Try

    If Not String.IsNullOrEmpty(Session.Item("localSubscription").crmAerodexFlag) Then
      If Session.Item("localSubscription").crmAerodexFlag = True Then
        broker_text = ""
        broker = False
      Else
        broker_text = strContact
      End If
    End If
    createExclusiveBroker = ""
    'End If
  End Function

  Function what_model(ByVal x As Object, ByVal y As Object) As String
    what_model = ""
    Try
      Dim compare_string As Integer = 0
      Dim compare_string2 As Integer = 0
      If IsDBNull(x) Then
      Else
        compare_string = x
      End If
      If IsDBNull(y) Then
      Else
        compare_string2 = y
      End If

      If compare_string2 <> 0 Then
        aTempTable = aclsData_Temp.Get_Clients_Aircraft_Model_amodID(CInt(y))
        'old = aTempTable2 = GetAircraft_MakeModel(CInt(y), Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmCommercial_Flag, Session.Item("localSubscription").crmJets_Flag, Session.Item("localSubscription").crmExecutive_Flag, Session.Item("localSubscription").crmTurboprops)
      ElseIf compare_string <> 0 Then
        aTempTable = aclsData_Temp.Get_JETNET_Aircraft_Model_amodID(CInt(x))
        'old =  aTempTable2 = 'GetAircraft_MakeModel(CInt(x), Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmCommercial_Flag, Session.Item("localSubscription").crmJets_Flag, Session.Item("localSubscription").crmExecutive_Flag, Session.Item("localSubscription").crmTurboprops)
      End If
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then

          For Each r As DataRow In aTempTable.Rows
            If compare_string2 <> 0 Then
              what_model = r("cliamod_make_name") & " " & Left(r("cliamod_model_name"), 15)
            Else
              what_model = r("amod_make_name") & " " & Left(r("amod_model_name"), 15)
            End If
          Next
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("main_site.Master.vb - what_model() - " & error_string)
        End If
        display_error()
      End If
    Catch ex As Exception
      error_string = "main_site.Master.vb - what_model() - " & ex.Message
      LogError(error_string)
    End Try
  End Function
  Function what_ac(ByVal jetnet As Integer, ByVal client As Integer, ByVal show As Integer) As String
    'This function takes what AC and determines what ac is associated with this ID. 
    what_ac = ""
    'If PerformDatabaseAction = True Then
    Try
      Dim aircraft_text As String = ""
      If jetnet <> 0 Then
        Dim aError As String = ""
        aTempTable = New DataTable
        aTempTable = aclsData_Temp.GetJETNET_AC_NAME(jetnet, aError)
        ' check the state of the DataTable
        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            For Each R As DataRow In aTempTable.Rows
              If show = 2 Then
                aircraft_text = R("amod_make_name") & " " & R("amod_model_name") & "<br />"
                If Not IsDBNull(R("ac_ser_nbr")) Then
                  If R("ac_ser_nbr") <> "" Then
                    aircraft_text += "<a href='details.aspx?ac_ID=" & jetnet & "&type=3&source=JETNET'>Ser #: " & R("ac_ser_nbr") & "</a><br />"
                  End If
                End If
                If Not IsDBNull(R("ac_year_mfr")) Then
                  If R("ac_year_mfr") <> "" Then
                    aircraft_text = aircraft_text & R("ac_year_mfr") & " "
                  End If
                End If
                If Not IsDBNull(R("ac_reg_nbr")) Then
                  If R("ac_reg_nbr") <> "" Then
                    aircraft_text = aircraft_text & " Reg #: " & R("ac_reg_nbr")
                  End If
                End If
              End If
              what_ac = aircraft_text
            Next
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("main_site.Master.vb - what_ac() - " & error_string)
          End If
          display_error()
        End If
      ElseIf client <> 0 Then
        aTempTable = New DataTable
        aTempTable = aclsData_Temp.Get_Clients_Aircraft_Ser_Model(client)
        ' check the state of the DataTable
        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            For Each R As DataRow In aTempTable.Rows

              If show = 2 Then

                If Not IsDBNull(R("cliamod_make_name")) And Not IsDBNull(R("cliamod_model_name")) Then
                  aircraft_text = R("cliamod_make_name") & " " & R("cliamod_model_name") & "<br />"
                End If

                If Not IsDBNull(R("cliaircraft_ser_nbr")) Then
                  If R("cliaircraft_ser_nbr") <> "" Then
                    aircraft_text += "<a href='details.aspx?ac_ID=" & client & "&type=3&source=CLIENT'>Ser #: " & R("cliaircraft_ser_nbr") & "</a><br />"
                  End If
                End If

                If Not IsDBNull(R("cliaircraft_year_mfr")) Then
                  If R("cliaircraft_year_mfr") <> "" Then
                    aircraft_text = aircraft_text & "Year: " & R("cliaircraft_year_mfr") & "<br />"
                  End If
                End If
                If Not IsDBNull(R("cliaircraft_reg_nbr")) Then
                  If R("cliaircraft_reg_nbr") <> "" Then
                    aircraft_text = aircraft_text & "Reg #: " & R("cliaircraft_reg_nbr") & "<br />"
                  End If
                End If
              End If
              what_ac = aircraft_text
            Next
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("main_site.Master.vb - what_ac() - " & error_string)
          End If
          display_error()
        End If
      End If
    Catch ex As Exception
      error_string = "main_site.Master.vb - what_ac() - " & ex.Message
      LogError(error_string)
    End Try
    'End If
  End Function
  Function what_comp(ByVal jetnet As Integer, ByVal client As Integer, ByVal part As Integer) As String
    'This function takes what company and source and displays what company id associated with the number
    what_comp = ""
    ' If PerformDatabaseAction = True Then
    Try
      Dim source As String = "JETNET"
      Dim idnum As Integer = 0
      Dim contact_text As String = ""
      If jetnet <> 0 Then
        source = "JETNET"
        idnum = jetnet
      ElseIf client <> 0 Then
        source = "CLIENT"
        idnum = client
      End If
      aTempTable = aclsData_Temp.GetCompanyInfo_ID(idnum, source, 0)
      ' check the state of the DataTable
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          For Each R As DataRow In aTempTable.Rows
            If part = 1 Then
              contact_text = "<b>" & R("comp_name") & "</b><br />"
            Else
              contact_text = contact_text & R("comp_address1") & "<br />"
              contact_text = contact_text & R("comp_city") & ", " & R("comp_state") & " "
              contact_text = contact_text & R("comp_zip_code") & "<br />"
              contact_text = contact_text & R("comp_country") & "<br />"
              contact_text = contact_text & "<a href='mailto:" & R("comp_email_address") & "' class='non_special_link'>" & R("comp_email_address") & "</a>"
            End If
            what_comp = contact_text
          Next
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("main_site.Master.vb - what_comp() - " & error_string)
        End If
        display_error()
      End If
      Return what_comp
    Catch ex As Exception
      error_string = "main_site.Master.vb - what_comp() - " & ex.Message
      LogError(error_string)
    End Try
    ' End If
  End Function
  Function what_comp_short(ByVal jetnet As Integer, ByVal client As Integer, ByVal part As Integer) As String
    'This function takes what company and source and displays what company id associated with the number
    what_comp_short = ""
    Try
      Dim source As String = "JETNET"
      Dim idnum As Integer = 0
      Dim contact_text As String = ""
      If jetnet <> 0 Then
        source = "JETNET"
        idnum = jetnet
      ElseIf client <> 0 Then
        source = "CLIENT"
        idnum = client
      End If
      aTempTable = aclsData_Temp.GetCompanyInfo_ID(idnum, source, 0)
      ' check the state of the DataTable
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          For Each R As DataRow In aTempTable.Rows
            If part = 1 Then
              contact_text = "<b>" & R("comp_name") & "</b><br />"
            Else
              contact_text = contact_text & R("comp_city") & ", " & R("comp_state") & " "
              contact_text = contact_text & R("comp_country") & "<br />"
            End If
            what_comp_short = contact_text
          Next
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("main_site.Master.vb - what_comp_short() - " & error_string)
        End If
        display_error()
      End If
      Return what_comp_short
    Catch ex As Exception
      error_string = "main_site.Master.vb - what_comp_short() - " & ex.Message
      LogError(error_string)
    End Try
  End Function
  Function what_contact(ByVal jetnet As Integer, ByVal client As Integer) As String
    'This function takes the contact id/source and displays what contact the number is associated with.
    what_contact = ""
    Try
      Dim source As String = "JETNET"
      Dim idnum As Integer = 0
      Dim contact_text As String = ""
      If jetnet <> 0 Then
        source = "JETNET"
        idnum = jetnet
      ElseIf client <> 0 Then
        source = "CLIENT"
        idnum = client
      End If
      aTempTable = aclsData_Temp.GetContacts_Details(idnum, source)
      Dim comp_id As Integer = 0
      ' check the state of the DataTable
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          For Each R As DataRow In aTempTable.Rows

            If Not IsDBNull(R("contact_first_name")) Then
              what_contact = R("contact_first_name") & " "
            End If

            If Not IsDBNull(R("contact_middle_initial")) Then
              what_contact += R("contact_middle_initial") & " "
            End If

            If Not IsDBNull(R("contact_last_name")) Then
              what_contact += R("contact_last_name") & " "
            End If

            what_contact += "<br />"

            If source = "CLIENT" Then
              If Not IsDBNull(R("contact_preferred_name")) Then
                what_contact = what_contact & "Preferred Name: " & R("contact_preferred_name") & "<br />"
              End If
            End If

            If Not IsDBNull(R("contact_title")) Then
              what_contact += R("contact_title")
            End If

            If Not IsDBNull(R("contact_email_address")) Then
              what_contact += " <br />" & "<a href='mailto:" & R("contact_email_address") & "' class='non_special_link'>" & R("contact_email_address") & "</a>"
            End If

            If source = "CLIENT" Then
              If Not IsDBNull(R("contact_notes")) Then
                what_contact = what_contact & "<br />" & R("contact_notes") & "<br />"
              End If
            End If
          Next
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("main_site.Master.vb - what_contact() - " & error_string)
        End If
        display_error()
      End If
      Return what_contact
    Catch ex As Exception
      error_string = "main_site.Master.vb - what_contact() - " & ex.Message
      LogError(error_string)
    End Try
  End Function
  Function what_contact_short(ByVal jetnet As Integer, ByVal client As Integer) As String
    'This function takes the contact id/source and displays what contact the number is associated with.
    what_contact_short = ""
    Try
      Dim source As String = "JETNET"
      Dim idnum As Integer = 0
      Dim contact_text As String = ""
      If jetnet <> 0 Then
        source = "JETNET"
        idnum = jetnet
      ElseIf client <> 0 Then
        source = "CLIENT"
        idnum = client
      End If
      aTempTable = aclsData_Temp.GetContacts_Details(idnum, source)
      Dim comp_id As Integer = 0
      ' check the state of the DataTable
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          For Each R As DataRow In aTempTable.Rows
            what_contact_short = R("contact_first_name") & " " & R("contact_middle_initial") & " " & R("contact_last_name") & "<br />"
          Next
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("main_site.Master.vb - what_contact_shortt() - " & error_string)
        End If
        display_error()
      End If
      Return what_contact_short
    Catch ex As Exception
      error_string = "main_site.Master.vb - what_contact_short() - " & ex.Message
      LogError(error_string)
    End Try
  End Function
  Function whatRelationship(ByVal rel As Object)
    whatRelationship = ""
    Try
      If Not IsDBNull(rel) Then
        If IsNumeric(rel) Then
          aTempTable = aclsData_Temp.Get_Client_Aircraft_Contact_Type(rel)
          If Not IsNothing(aTempTable) Then
            If aTempTable.Rows.Count > 0 Then
              For Each q In aTempTable.Rows
                If Not IsDBNull(q("cliact_name")) Then
                  whatRelationship = q("cliact_name")
                End If
              Next
            End If
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = aclsData_Temp.class_error
              LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - WhatRelationship() - " & error_string)
            End If
            display_error()
          End If

        End If
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Transaction_Tab.ascx.vb - whatRelationship() - " & ex.Message
      LogError(error_string)
    End Try
  End Function
  Function what_user(ByVal x As Object) As String

    what_user = ""
    If PerformDatabaseAction = True Or Not Page.IsPostBack Then
      Try
        If IsDBNull(x) Then
        Else
          If IsNumeric(x) Then
            aTempTable = aclsData_Temp.Get_Client_User(CInt(x))
            If Not IsNothing(aTempTable) Then
              If aTempTable.Rows.Count > 0 Then
                For Each r As DataRow In aTempTable.Rows
                  what_user = r("cliuser_first_name") & " " & Left(r("cliuser_last_name"), 15)
                Next
              End If
            Else
              If aclsData_Temp.class_error <> "" Then
                error_string = aclsData_Temp.class_error
                LogError("main_site.Master.vb - what_user() - " & error_string)
              End If
              display_error()
            End If
          Else
            Return x.ToString
          End If
        End If
      Catch ex As Exception
        error_string = "main_site.Master.vb - what_user() - " & ex.Message
        LogError(error_string)
      End Try
    End If

  End Function
  Function what_cat(ByVal x As Integer) As String
    what_cat = ""
    If PerformDatabaseAction = True Or Not Page.IsPostBack Then
      Try
        If Not IsDBNull(x) Then
          aTempTable = aclsData_Temp.Get_Client_Note_Category
          If Not IsNothing(aTempTable) Then
            If aTempTable.Rows.Count > 0 Then
              For Each R As DataRow In aTempTable.Rows
                'notecat_key, notecat_name
                If x = R("notecat_key") Then
                  what_cat = R("notecat_name")
                End If
              Next
            End If
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = aclsData_Temp.class_error
              LogError("main_site.Master.vb - what_cat() - " & error_string)
            End If
            display_error()
            what_cat = x
          End If
        End If
      Catch ex As Exception
        error_string = "main_site.Master.vb - what_cat() - " & ex.Message
        LogError(error_string)
      End Try
    End If
  End Function
  Function what_opportunity_cat(ByVal x As Integer, ByVal DisplayCategoryHeader As Boolean) As String
    what_opportunity_cat = ""
    If PerformDatabaseAction = True Or Not Page.IsPostBack Then
      Try
        If Not IsDBNull(x) Then
          aTempTable = aclsData_Temp.Get_Opportunity_Categories_ID(x)
          If Not IsNothing(aTempTable) Then
            If aTempTable.Rows.Count > 0 Then
              For Each R As DataRow In aTempTable.Rows
                'notecat_key, notecat_name
                If x = R("oppcat_key") Then
                  what_opportunity_cat = R("oppcat")
                End If
              Next
            End If
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = aclsData_Temp.class_error
              LogError("main_site.Master.vb - what_opportunity_cat() - " & error_string)
            End If
            display_error()
            what_opportunity_cat = x
          End If
        End If

        If what_opportunity_cat <> "" Then
          If DisplayCategoryHeader Then
            what_opportunity_cat = "Category: " & what_opportunity_cat
          End If
        End If
      Catch ex As Exception
        error_string = "main_site.Master.vb - what_opportunity_cat() - " & ex.Message
        LogError(error_string)
      End Try
    End If
  End Function
  Function what_comp_short_trans(ByVal jetnet As Integer, ByVal client As Integer, ByVal part As Integer, ByVal trans As Integer) As String
    'This function takes what company and source and displays what company id associated with the number
    what_comp_short_trans = ""
    Try
      Dim source As String = "JETNET"
      Dim idnum As Integer = 0
      Dim contact_text As String = ""
      If jetnet <> 0 Then
        source = "JETNET"
        idnum = jetnet
        aTempTable = aclsData_Temp.Get_JETNET_Transactions_Company(idnum, trans)

        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            For Each R As DataRow In aTempTable.Rows
              If part = 1 Then
                contact_text = "<b>" & R("tcomp_name") & "</b><br />"
              Else
                If Not IsDBNull(R("tcomp_name")) Then
                  contact_text = "<strong style='font-size:14px;color:#67A0D9;'>" & R("tcomp_name") & "</strong><br />"
                End If
                If Not IsDBNull(R("tcomp_address1")) Then
                  If Trim(R("tcomp_address1")) <> "" Then
                    contact_text = contact_text & R("tcomp_address1") & "<br />"
                  End If
                End If
                If Not IsDBNull(R("tcomp_address2")) Then
                  If Trim(R("tcomp_address2")) <> "" Then
                    contact_text = contact_text & R("tcomp_address2") & "<br />"
                  End If
                End If

                If Not IsDBNull(R("tcomp_city")) Then
                  If Trim(R("tcomp_city")) <> "" Then
                    contact_text = contact_text & R("tcomp_city") & ", "
                  End If
                End If

                If Not IsDBNull(R("tcomp_state")) Then
                  If Trim(R("tcomp_state")) <> "" Then
                    contact_text = contact_text & R("tcomp_state") & " "
                  End If
                End If

                If Not IsDBNull(R("tcomp_zip_code")) Then
                  If Trim(R("tcomp_zip_code")) <> "" Then
                    contact_text = contact_text & R("tcomp_zip_code") & "<br />"
                  Else
                  End If
                Else
                End If

                If Not IsDBNull(R("tcomp_country")) Then
                  If Trim(R("tcomp_country")) <> "" Then
                    contact_text = contact_text & R("tcomp_country") & "<br />"
                  Else
                  End If
                Else
                End If

              End If
              what_comp_short_trans = contact_text
            Next
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("main_site.Master.vb - what_comp_short_trans() - " & error_string)
          End If
          display_error()
        End If
        Return what_comp_short_trans

      ElseIf client <> 0 Then
        source = "CLIENT"
        idnum = client

        aTempTable = aclsData_Temp.Get_Client_Transactions_Company(idnum, trans)

        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            For Each R As DataRow In aTempTable.Rows
              If part = 1 Then
                contact_text = "<b>" & R("clitcomp_name") & "</b><br />"
              Else
                If Not IsDBNull(R("clitcomp_name")) Then
                  contact_text = "<strong style='font-size:14px;color:#67A0D9;'>" & R("clitcomp_name") & "</strong><br />"
                End If
                If Not IsDBNull(R("clitcomp_address1")) Then
                  If Trim(R("clitcomp_address1")) <> "" Then
                    contact_text = contact_text & R("clitcomp_address1") & "<br />"
                  End If
                End If
                If Not IsDBNull(R("clitcomp_address2")) Then
                  If Trim(R("clitcomp_address2")) <> "" Then
                    contact_text = contact_text & R("clitcomp_address2") & "<br />"
                  End If
                End If

                If Not IsDBNull(R("clitcomp_city")) Then
                  If Trim(R("clitcomp_city")) <> "" Then
                    contact_text = contact_text & R("clitcomp_city") & ", "
                  End If
                End If

                If Not IsDBNull(R("clitcomp_state")) Then
                  If Trim(R("clitcomp_state")) <> "" Then
                    contact_text = contact_text & R("clitcomp_state") & " "
                  End If
                End If

                If Not IsDBNull(R("clitcomp_zip_code")) Then
                  If Trim(R("clitcomp_zip_code")) <> "" Then
                    contact_text = contact_text & R("clitcomp_zip_code") & "<br />"
                  Else
                  End If
                Else
                End If

                If Not IsDBNull(R("clitcomp_country")) Then
                  If Trim(R("clitcomp_country")) <> "" Then
                    contact_text = contact_text & R("clitcomp_country") & "<br />"
                  Else
                  End If
                Else
                End If

              End If
              what_comp_short_trans = contact_text
            Next
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("main_site.Master.vb - what_comp_short_trans() - " & error_string)
          End If
          display_error()
        End If
        Return what_comp_short_trans
      End If


    Catch ex As Exception
      error_string = "main_site.Master.vb - what_comp_short_trans() - " & ex.Message
      LogError(error_string)
    End Try
  End Function

  Function Build_Aircraft_Company_Listings() As Panel
    Build_Aircraft_Company_Listings = New Panel
    Dim color As String = "container_grid"
    Dim linked As New Label
    Dim lab As New Label
    Dim cont As New Label
    Dim but As New ImageButton
    Dim fly As New OboutInc.Flyout2.Flyout
    Dim container As New Panel
    Dim add_me As Boolean = True
    Dim text As New Label
    Dim fly_text As String = ""
    Dim address_string As String = ""
    Dim lng_address_string As String = ""
    Dim phone_text As String = ""
    Dim contact_phone_text As String = ""
    Dim font_color As String = ""
    Dim comp_name As String = ""
    Dim comp_address As String = ""
    Dim comp_address2 As String = ""
    Dim comp_city As String = ""
    Dim comp_state As String = ""
    Dim comp_country As String = ""
    Dim comp_zip_code As String = ""
    Dim comp_email_address As String = ""
    Dim comp_web_address As String = ""
    Dim contact_first_name As String = ""
    Dim contact_last_name As String = ""
    Dim contact_middle_initial As String = ""
    Dim contact_title As String = ""
    Dim contact_preferred_name As String = ""
    Dim contact_notes As String = ""
    Dim contact_email_address As String = ""
    Dim contact_type_id As String = ""
    Dim comp_source As String = ""
    Dim source As String = ""
    Dim text_string3 As String = ""
    Dim text_string2 As String = ""
    Dim act_name As String = ""
    Dim perc As String = ""
    Dim cont_id As Integer = 0
    Dim id As Integer = 0
    Dim j As Integer = 0
    fly = New OboutInc.Flyout2.Flyout
    linked = New Label
    lab = New Label
    but = New ImageButton
    address_string = ""
    text = New Label
    cont = New Label
    container = New Panel


    lng_address_string = ""
    lng_address_string = lng_address_string & "<strong style='font-size:14px;color:#" & font_color & ";'>" & comp_name & "</strong><br />"
    If comp_address <> "" Then
      lng_address_string = lng_address_string & comp_address & "<br />"
    End If
    If comp_address2 <> "" Then
      lng_address_string = lng_address_string & " " & comp_address2 & "<br />"
    End If
    If comp_city <> "" Then
      address_string = address_string & comp_city & ","
      lng_address_string = lng_address_string & comp_city & ","
    End If
    If comp_state <> "" Then
      address_string = address_string & " " & comp_state
      lng_address_string = lng_address_string & " " & comp_state & "<br />"
    End If
    If comp_zip_code <> "" Then
      lng_address_string = lng_address_string & " " & comp_zip_code & "<br />"
    End If
    If comp_country <> "" Then
      address_string = address_string & " " & comp_country
      lng_address_string = lng_address_string & " " & comp_country & "<br />"
    End If
    If comp_email_address <> "" Then
      lng_address_string = lng_address_string & "<br /><a href='mailto:" & comp_email_address & "'>" & comp_email_address & "</a>"
    End If
    If comp_web_address <> "" Then
      If InStr(comp_web_address, "http://") = 0 Then
        lng_address_string = lng_address_string & "<br /><a href='http://" & comp_web_address & "' target='_new'>" & comp_web_address & "</a>"
      Else
        lng_address_string = lng_address_string & "<br /><a href='" & comp_web_address & "' target='_new'>" & comp_web_address & "</a>"
      End If
    End If


    container.CssClass = color
    linked.Text = "<span style='font-size:10px;'><a href='details.aspx?comp_ID=" & CInt(id) & "&source=" & source & "&type=1'>" & text_string3 & " (<em>" & act_name & clsGeneral.clsGeneral.showpercent(perc, act_name) & "</em>)</a></span>"


    text_string2 = "<span style='font-size:9px;'><i>" & address_string & "</i></span>"

    cont.Text = "<br clear='all' /><span style='font-size:10px;'><a href='details.aspx?comp_ID=" & CInt(id) & "&contact_ID=" & CInt(cont_id) & "&source=" & source & "&type=1'>" & contact_first_name & " " & contact_last_name & "</a></span>"


    Dim contact_text As String = ""
    'set up contact mouseover display

    If Not contact_first_name = "" Then
      contact_text = "<strong style='font-size:14px;color:#67A0D9;'>" & contact_first_name
    End If
    If Not contact_middle_initial = "" Then
      contact_text = contact_text & " " & contact_middle_initial
    End If
    If Not contact_last_name = "" Then
      contact_text = contact_text & " " & contact_last_name & "</strong><br />"
    End If
    If Not contact_title = "" Then
      contact_text = contact_text & contact_title & " <br />"
    End If
    If Not contact_email_address = "" Then
      contact_text = contact_text & "<a href='mailto:" & contact_email_address & "' class='non_special_link'>" & contact_email_address & "</a>"
    End If

    'If text_string3 <> "" Then
    '    aTempTable = Master.aclsData_Temp.GetPhoneNumbers(ID(j), 0, e.Item.Cells(3).Text,0)
    '    If Not IsNothing(aTempTable) Then

    '        If aTempTable.Rows.Count > 0 Then
    '            For Each q As DataRow In aTempTable.Rows
    '                If q("pnum_contact_id") <> 0 Then
    '                    contact_phone_text = contact_phone_text & q("pnum_type") & ": " & q("pnum_number") & "<br />"
    '                Else
    '                    phone_text = phone_text & q("pnum_type") & ": " & q("pnum_number") & "<br />"
    '                End If
    '            Next
    '        End If
    '    End If
    'End If
    If contact_phone_text <> "" Then
      contact_phone_text = "<br /><br /><strong style='font-size:14px;color:#67A0D9;'>CONTACT PHONE NUMBERS</strong><br />" & contact_phone_text
    End If

    If phone_text <> "" Then
      phone_text = "<br /><br /><strong style='font-size:14px;color:#67A0D9;'>COMPANY PHONE NUMBERS</strong><br />" & phone_text
    End If

    If add_me = True Then
      container.Controls.Add(linked)
      'e.Item.Cells(rowadd).Controls.Add(linked)
    End If
    but.ID = "Button" & j & id
    but.ImageUrl = "~/images/magnify.png"
    but.OnClientClick = "return false;"

    fly.Align = OboutInc.Flyout2.AlignStyle.TOP
    fly.Position = OboutInc.Flyout2.PositionStyle.TOP_RIGHT
    fly.FlyingEffect = OboutInc.Flyout2.FlyingEffectStyle.TOP_RIGHT
    fly.FadingEffect = True
    fly_text = clsGeneral.clsGeneral.MouseOverTextStart()
    fly_text = fly_text & UCase(lng_address_string)

    ' If cont_id(j) = 0 Then
    'phone now
    fly_text = fly_text & UCase(phone_text)
    'End If
    If contact_text <> "" Then
      fly_text = fly_text & "<br /><br />" & UCase(contact_text)
    End If
    ' If cont_id(j) <> 0 Then
    'phone now
    fly_text = fly_text & UCase(contact_phone_text)
    'End If

    fly_text = fly_text & clsGeneral.clsGeneral.MouseOverTextEnd()
    text.Text = fly_text
    fly.AttachTo = "Button" & j & id
    fly.Controls.Add(text)
    If add_me = True Then
      container.Controls.Add(but)
      container.Controls.Add(lab)
      ' e.Item.Cells(rowadd).Controls.Add(but)
      ' e.Item.Cells(rowadd).Controls.Add(lab)
    End If
    'If act_name = "Exclusive Broker" Then
    '    Dim ex As Label = e.Item.Cells(15).FindControl("popup_ex")
    '    Dim flyout1 As OboutInc.Flyout2.Flyout = e.Item.Cells(16).FindControl("Flyout1")
    '    Dim str As String = ex.Text
    '    ' ex.Text = "<img src='images/purple_arrow.gif' alt='Exclusive' width='25'/>"
    '    flyout1.Controls.Clear()
    '    flyout1.Controls.Add(text)
    'End If

    'Else
    'add_me = False ' no company to add :(
    ''change add me to false so a company doesn't get added.
    ''fixed 6/14/2011
    'End If

    If add_me = True Then
      container.Controls.Add(cont)
      ' e.Item.Cells(rowadd).Controls.Add(cont)
    End If
    If add_me = True Then
      container.Controls.Add(fly)
      'e.Item.Cells(rowadd).Controls.Add(fly)
      Dim pan As New Panel
      pan.Controls.Add(container)
      'e.Item.Cells(rowadd).Controls.Add(container)
      Return pan
    End If
  End Function
  Function Market_Client_AC_Return(ByVal jetnet_ID As Integer) As String
    If jetnet_ID <> 0 Then
      aTempTable = aclsData_Temp.CHECKFORClient_Aircraft_JETNET_AC(jetnet_ID)
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          Market_Client_AC_Return = "<a href='details.aspx?ac_ID=" & aTempTable.Rows(0).Item("cliaircraft_id") & "&type=3&source=CLIENT'><img src='images/client_aircraft.png' alt='Client Aircraft Associated with this Record' border='0'/></a>"
        Else
          Market_Client_AC_Return = ""
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("main_site.Master.vb - Market_Client_AC_Return() - " & error_string)
        End If
        display_error()
        Market_Client_AC_Return = ""

      End If
    Else
      Market_Client_AC_Return = ""
    End If
  End Function




  Function PrepopulatedAddressPopOut(ByVal compID As Object, ByVal compSource As Object, ByVal comp_name As Object, ByVal comp_address1 As Object, ByVal comp_address2 As Object, ByVal comp_city As Object, ByVal comp_state As Object, ByVal comp_zip_code As Object, ByVal comp_country As Object, ByVal description As Object, ByVal comp_email_address As Object, ByVal Phone As Object, ByVal Fax As Object) As String
    PrepopulatedAddressPopOut = ""
    company_listing_text = ""

    Try
      Dim address As String = ""
      Dim address_hold As String = ""
      'If Not IsDBNull(compSource) Then
      '    compSource = UCase(compSource)
      'End If


      If Not IsDBNull(comp_name) Then
        address_hold = "<strong style='font-size:14px;color:#67A0D9;'>" & comp_name & "</strong><br />"
      End If

      'company_listing_text = "<a href='details.aspx?source=" & compSource & "&comp_ID=" & compID & "&type=1'>" & Replace(Replace(address_hold, "<strong style='font-size:14px;color:#67A0D9;'>", ""), "</strong>", "") & "</a>"

      If Not IsDBNull(comp_address1) Then
        If Trim(comp_address1) <> "" Then
          address += comp_address1 & "<br />"
        End If
      End If
      If Not IsDBNull(comp_address2) Then
        If Trim(comp_address2) <> "" Then
          address += comp_address2 & "<br />"
        End If
      End If

      If Not IsDBNull(comp_city) Then
        If Trim(comp_city) <> "" Then
          address += comp_city & ", "
        End If
      End If
      If Not IsDBNull(comp_state) Then
        If Trim(comp_state) <> "" Then
          address += comp_state & " "
        End If
      End If

      If Not IsDBNull(comp_zip_code) Then
        If Trim(comp_zip_code) <> "" Then
          address += comp_zip_code & "<br />"
        End If
      End If

      If Not IsDBNull(comp_country) Then
        If Trim(comp_country) <> "" Then
          address += comp_country
        End If
      End If

      If Not IsDBNull(comp_email_address) Then
        If Trim(comp_email_address) <> "" Then
          address += "<a href='mailto:" & comp_email_address & "' class='non_special_link'>" & comp_email_address & "</a>"
        End If
      End If
      'company_listing_text = company_listing_text & address

      address = address_hold & address & "<br />"

      '------Phone Company Information Left Card Display----------------------------------------------------------------------

      If Not IsDBNull(Phone) And Not IsDBNull(Fax) Then
        address = address & "<br /><strong style='font-size:12px;color:#4d7997;'>Phone Numbers</strong><br />"
      End If

      If Not IsDBNull(Phone) Then
        If Trim(Phone) <> "" Then
          address = address & "Office : " & Phone & "<br />"
        End If
      End If

      If Not IsDBNull(Fax) Then
        If Trim(Fax) <> "" Then
          address = address & "Fax : " & Fax & "<br />"
        End If
      End If


      If address <> "" Then
        address = UCase(address.TrimEnd("<br />"))
      End If

      If Not IsDBNull(description) Then
        PrepopulatedAddressPopOut = address & "<br /><br />" & description
      Else
        PrepopulatedAddressPopOut = address
      End If


    Catch ex As Exception
      error_string = "main_site.Master.vb - createAnAddressPopOut() - " & ex.Message
      LogError(error_string)
    End Try

  End Function


  Function PrepoluatedAircraftPopout(ByVal acID As Object, ByVal amod_make_name As Object, ByVal amod_model_name As Object, ByVal ac_year_mfr As Object, ByVal ac_reg_nbr As Object, ByVal ac_ser_nbr As Object, ByVal ac_date_purchased As Object, ByVal ac_forsale_flag As Object, ByVal ac_status As Object, ByVal ac_delivery As Object, ByVal ac_asking_wordage As Object, ByVal ac_asking_price As Object, ByVal ac_est_price As Object, ByVal ac_date_listed As Object, ByVal ac_exclusive_flag As Object, ByVal ac_lease_flag As Object) As String
    Dim aircraft_text As String = ""
    aircraft_listing_text = ""
    Try

      'Make/Model
      If Not IsDBNull(amod_make_name) And Not IsDBNull(amod_model_name) Then
        aircraft_text = amod_make_name & " " & amod_model_name & "<br />"
      End If
      'Year MFR
      If Not IsDBNull(ac_year_mfr) Then
        If ac_year_mfr <> "" Then
          aircraft_text += "Year: " & ac_year_mfr & "<br />"
        End If
      End If

      If Not IsDBNull(ac_reg_nbr) Then
        If ac_reg_nbr <> "" Then
          aircraft_text += "Reg #: " & ac_reg_nbr & "<br />"
        End If
      End If

      If Not IsDBNull(ac_ser_nbr) Then
        If ac_ser_nbr <> "" Then
          aircraft_text += "Ser #: " & ac_ser_nbr & "<br />"
        End If
      End If

      aircraft_listing_text = "<a href='details.aspx?source=CLIENT&ac_ID=" & acID & "&type=3'>" & aircraft_text & "</a>"

      If Not IsDBNull(ac_date_purchased) Then
        If CStr(ac_date_purchased) <> "1/1/1900" Then
          aircraft_text += "</b>Purchased: " & ac_date_purchased & "<br />"
        End If
      End If



      If Session.Item("localSubscription").crmAerodexFlag = False Then
        If Not IsDBNull(ac_forsale_flag) Then
          If ac_forsale_flag = "Y" Then
            aircraft_text = aircraft_text & "<b class='green'>" & ac_status
            If Not IsDBNull(ac_delivery) Then
              If ac_delivery <> "" Then
                aircraft_text = aircraft_text & " - " & ac_delivery
              End If
            End If
            If Not IsDBNull(ac_asking_wordage) Then
              If ac_asking_wordage <> "" Then
                If ac_asking_wordage = "Price" Then
                  If Not IsDBNull(ac_asking_price) Then
                    Dim asking_price As String = clsGeneral.clsGeneral.no_zero(ac_asking_price, "", True)
                    If asking_price <> "" Then
                      aircraft_text = aircraft_text & " Asking: " & asking_price
                    End If
                  End If
                Else
                  aircraft_text = aircraft_text & " " & ac_asking_wordage
                End If
              End If

            End If
            aircraft_text = aircraft_text & "</b><br />"
          End If
        End If

        If Not IsDBNull(ac_est_price) Then
          Dim take_price As String = clsGeneral.clsGeneral.no_zero(ac_est_price, "", True)
          If take_price <> "" Then
            aircraft_text = aircraft_text & "Take Price: " & take_price & "<br />"
          End If
        End If

        If Not IsDBNull(ac_date_listed) Then
          Dim date_listed As String = clsGeneral.clsGeneral.datenull(ac_date_listed)
          If date_listed <> "" Then
            aircraft_text = aircraft_text & "List Date: " & date_listed & "<br />"
          End If
        End If

        If Not IsDBNull(ac_date_listed) Then
          Dim date_listed As String = clsGeneral.clsGeneral.datenull(ac_date_listed)
          If date_listed <> "" Then
            aircraft_text = aircraft_text & clsGeneral.clsGeneral.trans_date_diff(Now(), ac_date_listed, 2) & "<br />"
          End If
        End If

        If Not IsDBNull(ac_status) Then
          If ac_status <> "For Sale" Then
            aircraft_text = aircraft_text & ac_status & "<br />"
          End If
        End If


        If Not IsDBNull(ac_exclusive_flag) Then
          If ac_exclusive_flag <> "" Then
            aircraft_text = aircraft_text & clsGeneral.clsGeneral.yes_no(ac_exclusive_flag, "exclusive")
          End If
        End If

        If Not IsDBNull(ac_lease_flag) Then
          If ac_lease_flag <> "" Then
            aircraft_text = aircraft_text & clsGeneral.clsGeneral.yes_no(ac_lease_flag, "leased")
          End If
        End If
      End If

    Catch ex As Exception

      error_string = "main_site.Master.vb - createanACPopOut() - " & ex.Message
      LogError(error_string)
    End Try

    Return aircraft_text

  End Function

#End Region
  ''' <summary>
  ''' This javascript function builds the javascript dropdown menu for the pages. 
  ''' </summary>
  ''' <returns>string</returns>
  ''' <remarks></remarks>
  Public Function Write_JSCRIPT() As String
    Write_JSCRIPT = ""
    Try
      If Session.Item("crmUserLogon") = True Then
        '-----------------------------------------This Function Randomly Generates the Javascript Menu for the Dropdown Menu------
        Write_JSCRIPT = "<script type='text/javascript'>" & vbNewLine
        'Write_JSCRIPT = Write_JSCRIPT & "function load(x,y) {" & vbNewLine
        'Write_JSCRIPT = Write_JSCRIPT & "window.open(x,'test',y);" & vbNewLine & "}" & vbNewLine
        Write_JSCRIPT = Write_JSCRIPT & vbNewLine & "var anylinkmenu_view={divclass:'anylinkmenuwide', inlinestyle:'', linktarget:''}" & vbNewLine
        Write_JSCRIPT = Write_JSCRIPT & "anylinkmenu_view.items=[" & vbNewLine
        Write_JSCRIPT = Write_JSCRIPT & "[""Model Market Summary"", ""javascript:load('view_template.aspx?ViewID=1&noMaster=false','','scrollbars=yes,menubar=no,height=700,width=1150,resizable=yes,toolbar=no,location=no,status=no');""] //no comma following last entry!" & vbNewLine
        Write_JSCRIPT = Write_JSCRIPT & "]" & vbNewLine

        Dim EditMenuString As String = ""
        Dim FolderMenuEditString As String = ""
        FolderMenuEditString = "[""Edit Company Folders"", ""javascript:load('edit.aspx?action=cyfolder&type=edit','','scrollbars=yes,menubar=no,height=600,width=600,resizable=yes,toolbar=no,location=no,status=no');""], //no comma following last entry!" & vbNewLine
        FolderMenuEditString += "[""Edit Contacts Folders"", ""javascript:load('edit.aspx?action=ctfolder&type=edit','','scrollbars=yes,menubar=no,height=600,width=600,resizable=yes,toolbar=no,location=no,status=no');""], //no comma following last entry!" & vbNewLine
        FolderMenuEditString += "[""Edit Aircraft Folders"", ""javascript:load('edit.aspx?action=aifolder&type=edit','','scrollbars=yes,menubar=no,height=600,width=600,resizable=yes,toolbar=no,location=no,status=no');""], //no comma following last entry!" & vbNewLine
        FolderMenuEditString += "[""Edit History Folders"", ""javascript:load('edit.aspx?action=trfolder&type=edit','','scrollbars=yes,menubar=no,height=600,width=600,resizable=yes,toolbar=no,location=no,status=no');""] //no comma following last entry!" & vbNewLine



        'ADMIN
        Write_JSCRIPT = Write_JSCRIPT & vbNewLine & "var anylinkmenu_sub2={divclass:'anylinkmenu', inlinestyle:'', linktarget:''}" & vbNewLine
        Write_JSCRIPT = Write_JSCRIPT & "anylinkmenu_sub2.items=[" & vbNewLine
        Write_JSCRIPT = Write_JSCRIPT & "[""Users"", ""javascript:load('edit.aspx?action=user','','scrollbars=yes,menubar=no,height=550,width=860,resizable=yes,toolbar=no,location=no,status=no');""]," & vbNewLine

        Write_JSCRIPT = Write_JSCRIPT & "[""Export Log"", ""javascript:load('edit.aspx?action=view_logs','','scrollbars=yes,menubar=no,height=550,width=860,resizable=yes,toolbar=no,location=no,status=no');""], //no comma following last entry!" & vbNewLine
        Write_JSCRIPT = Write_JSCRIPT & "[""DEBUG"", ""javascript:load('debug.aspx','','scrollbars=yes,menubar=no,height=600,width=600,resizable=yes,toolbar=no,location=no,status=no');""] //no comma following last entry!" & vbNewLine
        Write_JSCRIPT = Write_JSCRIPT & "]" & vbNewLine

        Dim old As String = ""
        Write_JSCRIPT = Write_JSCRIPT & vbNewLine & "var anylinkmenu_sub4={divclass:'anylinkmenuwidest', inlinestyle:'', linktarget:''}" & vbNewLine
        Write_JSCRIPT = Write_JSCRIPT & "anylinkmenu_sub4.items=[" & vbNewLine


        clsGeneral.clsGeneral.FillCachedHelpMenu(aclsData_Temp)
        If Not IsNothing(Cache("CachedHelpMenu")) Then

          ' evotop_name, evonot_title, evonot_doc_link, evonot_id 
          If Session.Item("localUser").crmEvo <> True Then
            If Not IsNothing(Cache("CachedHelpMenu").tables(0)) Then
              aTempTable = Cache("CachedHelpMenu").tables(0)
              If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then
                  For Each r As DataRow In aTempTable.Rows
                    If r("evotop_name") <> old Then
                      Write_JSCRIPT = Write_JSCRIPT & "[""<div style='background-color:#4d4d4d;color:#ffffff;'>" & r("evotop_name") & "</div>"", ""#""], //no comma following last entry!" & vbNewLine
                    End If
                    If Not IsDBNull(r("evonot_doc_link")) Then
                      If InStr(Trim(r("evonot_doc_link")), "http://") = 0 Then
                        Write_JSCRIPT = Write_JSCRIPT & "[""&nbsp;&nbsp;&nbsp;" & r("evonot_title") & """, ""javascript:load('http://" & r("evonot_doc_link") & "','')""], //no comma following last entry!" & vbNewLine
                      Else
                        Write_JSCRIPT = Write_JSCRIPT & "[""&nbsp;&nbsp;&nbsp;" & r("evonot_title") & """, ""javascript:load('" & r("evonot_doc_link") & "','')""], //no comma following last entry!" & vbNewLine
                      End If
                    End If
                    old = r("evotop_name")
                  Next

                End If
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("main_site.Master.vb - get_help_topics() - " & error_string)
                End If
                display_error()
              End If
            End If


            Write_JSCRIPT = Write_JSCRIPT & "[""<div style='background-color:#4d4d4d;color:#ffffff;'>LATEST RELEASES</div>"", ""#""], //no comma following last entry!" & vbNewLine
            If Not IsNothing(Cache("CachedHelpMenu").tables(1)) Then
              aTempTable = Cache("CachedHelpMenu").tables(1)
              If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then
                  For Each r As DataRow In aTempTable.Rows
                    If Not IsDBNull(r("evonot_doc_link")) Then
                      If InStr(Trim(r("evonot_doc_link")), "http://") = 0 Then
                        Write_JSCRIPT = Write_JSCRIPT & "[""&nbsp;&nbsp;&nbsp;" & r("evonot_title") & """, ""javascript:load('http://" & r("evonot_doc_link") & "','','')""], //no comma following last entry!" & vbNewLine
                      Else
                        Write_JSCRIPT = Write_JSCRIPT & "[""&nbsp;&nbsp;&nbsp;" & r("evonot_title") & """, ""javascript:load('" & r("evonot_doc_link") & "','','')""], //no comma following last entry!" & vbNewLine
                      End If
                    End If
                  Next
                End If
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("main_site.Master.vb - get_help_topics() - " & error_string)
                End If
                display_error()
              End If


              If Right(Trim(Write_JSCRIPT), 1) = "," Then
                ' then we have to get rid of 
                Write_JSCRIPT = Left(Trim(Write_JSCRIPT), Len(Trim(Write_JSCRIPT)) - 1)
              End If
              Write_JSCRIPT = Write_JSCRIPT & "]" & vbNewLine

            End If
          End If
        End If






        Select Case TypeOfListing
          Case 14
            Write_JSCRIPT = Write_JSCRIPT & vbNewLine & "var anylinkmenu3={divclass:'anylinkmenu', inlinestyle:'', linktarget:''}" & vbNewLine
            Write_JSCRIPT = Write_JSCRIPT & "anylinkmenu3.items=[" & vbNewLine
            Write_JSCRIPT = Write_JSCRIPT & "[""Custom Export"", ""javascript:load('export_creator.aspx?','','scrollbars=yes,menubar=no,height=560,width=1400,resizable=yes,toolbar=no,location=no,status=no');""] //no comma following last entry!" & vbNewLine
            Write_JSCRIPT = Write_JSCRIPT & "]" & vbNewLine
          Case 10

          Case 2

            'EditMenuString = vbNewLine & "var anylinkmenu2={divclass:'anylinkmenu', inlinestyle:'', linktarget:''}" & vbNewLine
            'EditMenuString += "anylinkmenu2.items=[" & vbNewLine
            EditMenuString = "[""New Company"", ""javascript:load('edit.aspx?action=new&type=company','','scrollbars=yes,menubar=no,height=620,width=1030,resizable=yes,toolbar=no,location=no,status=no');""], //no comma following last entry!" & vbNewLine
            EditMenuString += "[""Contact Quick Entry"", ""javascript:load('edit.aspx?action=quick','','scrollbars=yes,menubar=no,height=750,width=1110,resizable=yes,toolbar=no,location=no,status=no');""]" & vbNewLine
            If OtherID <> 0 Then
              EditMenuString += "[""New Contact"", ""javascript:load('edit.aspx?action=new&parent=2','','scrollbars=yes,menubar=no,height=600,width=860,resizable=yes,toolbar=no,location=no,status=no');""] //no comma following last entry!" & vbNewLine
            End If

            Write_JSCRIPT = Write_JSCRIPT & vbNewLine & "var anylinkmenu3={divclass:'anylinkmenu', inlinestyle:'', linktarget:''}" & vbNewLine
            Write_JSCRIPT = Write_JSCRIPT & "anylinkmenu3.items=[" & vbNewLine
            Write_JSCRIPT = Write_JSCRIPT & "[""Save As - New Folder"", ""javascript:crmSaveFolder('ContactSearch','ctfolder',0);""]" & vbNewLine

            If IsSubNode Then
              If Subnode_Method = "A" Then
                Write_JSCRIPT = Write_JSCRIPT & ",[""Save Current Folder"", ""javascript:crmSaveFolder('ContactSearch','ctfolder'," & SubNodeOfListing & ");""]" & vbNewLine
              End If
            End If


            Dim search_first As String = CType(ContactSearch.FindControl("first_name"), TextBox).Text
            Dim search_last As String = CType(ContactSearch.FindControl("last_name"), TextBox).Text
            Dim search_where As String = CType(ContactSearch.FindControl("search_where"), DropDownList).SelectedValue
            Dim company_name As String = CType(ContactSearch.FindControl("comp_name_txt"), TextBox).Text
            Dim status_cbo As String = CType(ContactSearch.FindControl("status_cbo"), DropDownList).SelectedValue
            Dim ordered_by As String = CType(ContactSearch.FindControl("ordered_by"), DropDownList).SelectedValue
            Dim email_address As String = CType(ContactSearch.FindControl("comp_email_address"), TextBox).Text
            Dim subset As String = CType(ContactSearch.FindControl("subset"), DropDownList).SelectedValue
            Dim phone As String = CType(ContactSearch.FindControl("phone"), TextBox).Text

            Dim sub_text As String = ""
            If NameOfListingType <> NameOfSubnode Then
              sub_text = NameOfSubnode
            End If


            Write_JSCRIPT = Write_JSCRIPT & ",['Custom Export', ""javascript:load('export_creator.aspx?all=false&cphn=" & phone & "&snt=" & Replace(sub_text, "'", "") & "&sn=" & SubNodeOfListing & "&fn=" & Replace(search_first, "'", "\\'") & "&ln=" & Replace(search_last, "'", "\\'") & "&sw=" & search_where & "&cn=" & clsGeneral.clsGeneral.Get_Name_Search_String(company_name) & "&st=" & status_cbo & "&or=" & ordered_by & "&su=" & subset & "&cem=" & Replace(email_address, "'", "\\'") & "','','scrollbars=yes,menubar=no,height=560,width=1400,resizable=yes,toolbar=no,location=no,status=no');""] //no comma following last entry!" & vbNewLine
            Write_JSCRIPT = Write_JSCRIPT & "]" & vbNewLine
          Case 3
            If ListingID = 0 Then

              'EditMenuString = vbNewLine & "var anylinkmenu2={divclass:'anylinkmenu', inlinestyle:'', linktarget:''}" & vbNewLine
              'EditMenuString += "anylinkmenu2.items=[" & vbNewLine
              EditMenuString = "[""New Aircraft"", ""javascript:load('edit.aspx?action=new&type=aircraft','','scrollbars=yes,menubar=no,height=600,width=960,resizable=yes,toolbar=no,location=no,status=no');""] //no comma following last entry!" & vbNewLine

              Write_JSCRIPT = Write_JSCRIPT & vbNewLine & "var anylinkmenu3={divclass:'anylinkmenu', inlinestyle:'', linktarget:''}" & vbNewLine
              Write_JSCRIPT = Write_JSCRIPT & "anylinkmenu3.items=[" & vbNewLine
              Write_JSCRIPT = Write_JSCRIPT & "[""Save As - New Folder"", ""javascript:crmSaveFolder('aircraftSearch','aifolder',0);""]" & vbNewLine

              If IsSubNode Then
                If Subnode_Method = "A" Then
                  Write_JSCRIPT = Write_JSCRIPT & ",[""Save Current Folder"", ""javascript:crmSaveFolder('aircraftSearch','aifolder'," & SubNodeOfListing & ");""]" & vbNewLine
                End If
              End If

              Dim search As String = CType(aircraftSearch.FindControl("search_for_txt"), TextBox).Text
              Dim search_where As String = CType(aircraftSearch.FindControl("search_where"), DropDownList).SelectedValue
              Dim model_cbo As ListBox = aircraftSearch.FindControl("model_cbo")
              Dim market_status_cbo As String = CType(aircraftSearch.FindControl("market_status_cbo"), DropDownList).SelectedValue
              Dim subset As String = CType(aircraftSearch.FindControl("subset"), DropDownList).SelectedValue
              Dim airport_name As String = CType(aircraftSearch.FindControl("airport_name"), TextBox).Text
              Dim icao_code As String = CType(aircraftSearch.FindControl("icao_code"), TextBox).Text
              Dim iata_code As String = CType(aircraftSearch.FindControl("iata_code"), TextBox).Text
              Dim city As String = CType(aircraftSearch.FindControl("city"), TextBox).Text
              Dim country_cbo As ListBox = CType(aircraftSearch.FindControl("country"), ListBox)
              Dim state As ListBox = aircraftSearch.FindControl("state")
              Dim types_of_owners As String = CType(aircraftSearch.FindControl("types_of_owners"), DropDownList).SelectedValue
              Dim on_exclusive As String = CType(aircraftSearch.FindControl("on_exclusive"), DropDownList).SelectedValue
              Dim on_lease As String = CType(aircraftSearch.FindControl("on_lease"), DropDownList).SelectedValue
              Dim year_start As String = CType(aircraftSearch.FindControl("year_start"), DropDownList).SelectedValue
              Dim year_end As String = CType(aircraftSearch.FindControl("year_end"), DropDownList).SelectedValue
              Dim lifecycle As String = CType(aircraftSearch.FindControl("ac_lifecycle_dropdown"), DropDownList).SelectedValue
              Dim ownership As String = CType(aircraftSearch.FindControl("ac_ownership_type"), DropDownList).SelectedValue

              'Start of custom fields.
              Dim CustomField1 As String = CType(aircraftSearch.FindControl("custom_pref_text1"), TextBox).Text
              Dim CustomField2 As String = CType(aircraftSearch.FindControl("custom_pref_text2"), TextBox).Text
              Dim CustomField3 As String = CType(aircraftSearch.FindControl("custom_pref_text3"), TextBox).Text
              Dim CustomField4 As String = CType(aircraftSearch.FindControl("custom_pref_text4"), TextBox).Text
              Dim CustomField5 As String = CType(aircraftSearch.FindControl("custom_pref_text5"), TextBox).Text
              Dim CustomField6 As String = CType(aircraftSearch.FindControl("custom_pref_text6"), TextBox).Text
              Dim CustomField7 As String = CType(aircraftSearch.FindControl("custom_pref_text7"), TextBox).Text
              Dim CustomField8 As String = CType(aircraftSearch.FindControl("custom_pref_text8"), TextBox).Text
              Dim CustomField9 As String = CType(aircraftSearch.FindControl("custom_pref_text9"), TextBox).Text
              Dim CustomField10 As String = CType(aircraftSearch.FindControl("custom_pref_text10"), TextBox).Text

              'Notes Search? With/Without/With and Without
              Dim notesSearch As String = CType(aircraftSearch.FindControl("aircraftNotes"), DropDownList).SelectedValue
              Dim notesSearchDate As String = CType(aircraftSearch.FindControl("notesDate"), TextBox).Text
              Dim NotesSearchString As String = ""

              Dim CustomFieldString As String = ""
              'Custom Fields string:
              If CustomField1 <> "" Then
                CustomFieldString = "&c1=" & CustomField1
              End If
              If CustomField2 <> "" Then
                CustomFieldString += "&c2=" & CustomField2
              End If
              If CustomField3 <> "" Then
                CustomFieldString += "&c3=" & CustomField3
              End If
              If CustomField4 <> "" Then
                CustomFieldString += "&c4=" & CustomField4
              End If
              If CustomField5 <> "" Then
                CustomFieldString += "&c5=" & CustomField5
              End If
              If CustomField6 <> "" Then
                CustomFieldString += "&c6=" & CustomField6
              End If
              If CustomField7 <> "" Then
                CustomFieldString += "&c7=" & CustomField7
              End If
              If CustomField8 <> "" Then
                CustomFieldString += "&c8=" & CustomField8
              End If
              If CustomField9 <> "" Then
                CustomFieldString += "&c9=" & CustomField9
              End If
              If CustomField10 <> "" Then
                CustomFieldString += "&c10=" & CustomField10
              End If

              If notesSearch > 0 Then
                NotesSearchString += "&nss=" & notesSearch.ToString
              End If

              If notesSearchDate <> "" Then
                NotesSearchString += "&and=" & notesSearchDate
              End If

              Dim models As String = ""

              If Not IsNothing(model_cbo) Then
                models = model_cbo.ClientID
              End If

              Dim states As String = ""
              For i = 0 To state.Items.Count - 1
                If state.Items(i).Selected Then
                  If state.Items(i).Value <> "" Then
                    states = states & "" & state.Items(i).Value & ","
                  End If
                End If
              Next

              If states <> "" Then
                states = UCase(states.TrimEnd(","))
              End If

              Dim country As String = ""
              For i = 0 To country_cbo.Items.Count - 1
                If country_cbo.Items(i).Selected Then
                  If country_cbo.Items(i).Value <> "" Then
                    country += "" & country_cbo.Items(i).Value & ","
                  End If
                End If
              Next

              If states <> "" Then
                states = UCase(states.TrimEnd(","))
              End If


              Dim sub_text As String = ""
              If NameOfListingType <> NameOfSubnode Then
                If Subnode_Method <> "A" Then
                  sub_text = NameOfSubnode
                End If
              End If


              If Session.Item("localUser").crmEvo = True Then
                Write_JSCRIPT = Write_JSCRIPT & ",[""Custom Export"", ""javascript:load('evo_exporter.aspx?ys=" & year_start & "&ye=" & year_end & "&ex=" & on_exclusive & "&le=" & on_lease & "&snt=" & sub_text & "&sn=" & SubNodeOfListing & "&sta=" & states & "&se=" & search & "&sw=" & search_where & "&m=" & models & "&ms=" & market_status_cbo & "&su=" & subset & "&an=" & airport_name & "&ic=" & icao_code & "&ia=" & iata_code & "&ci=" & city & "&co=" & country & "&ow=" & types_of_owners & "','','scrollbars=yes,menubar=no,height=800,width=1060,resizable=yes,toolbar=no,location=no,status=no');""] //no comma following last entry!" & vbNewLine

              Else
                Write_JSCRIPT = Write_JSCRIPT & ",[""Custom Export"", ""javascript:load('export_creator.aspx?lcs=" & lifecycle & "&ot=" & ownership & "&ys=" & year_start & "&ye=" & year_end & "&ex=" & on_exclusive & "&le=" & on_lease & "&snt=" & sub_text & "&sn=" & SubNodeOfListing & "&sta=" & states & "&se=" & search & "&sw=" & search_where & "&m=" & models & "&ms=" & market_status_cbo & "&su=" & subset & "&an=" & airport_name & "&ic=" & icao_code & "&ia=" & iata_code & "&ci=" & city & "&co=" & country & "&ow=" & types_of_owners & "" & CustomFieldString & NotesSearchString & "','','scrollbars=yes,menubar=no,height=560,width=1400,resizable=yes,toolbar=no,location=no,status=no');""] //no comma following last entry!" & vbNewLine
              End If


              Write_JSCRIPT = Write_JSCRIPT & "]" & vbNewLine
            Else

              'EditMenuString = vbNewLine & "var anylinkmenu2={divclass:'anylinkmenuwide', inlinestyle:'', linktarget:''}" & vbNewLine
              'EditMenuString += "anylinkmenu2.items=[" & vbNewLine

              EditMenuString = "[""New Aircraft"", ""javascript:load('edit.aspx?action=new&type=aircraft','','scrollbars=yes,menubar=no,height=600,width=960,resizable=yes,toolbar=no,location=no,status=no');""], //no comma following last entry!" & vbNewLine
              EditMenuString += "[""New Note"", ""javascript:load('edit_note.aspx?type=note&action=new','','scrollbars=yes,menubar=no,height=400,width=860,resizable=yes,toolbar=no,location=no,status=no');""], //no comma following last entry!" & vbNewLine

              If OtherID = 0 And ListingSource = "JETNET" Then
                EditMenuString += "[""New Transaction"", ""javascript:load('edit.aspx?action=edit&type=aircraft&trans=&auto_ac=true','','scrollbars=yes,menubar=no,height=900,width=860,resizable=yes,toolbar=no,location=no,status=no');""], //no comma following last entry!" & vbNewLine
              Else
                EditMenuString += "[""New Transaction"", ""javascript:load('edit.aspx?action=edit&type=transaction&new=true','','scrollbars=yes,menubar=no,height=900,width=860,resizable=yes,toolbar=no,location=no,status=no');""], //no comma following last entry!" & vbNewLine
              End If

              EditMenuString += "[""New Action Item"", ""javascript:load('edit_note.aspx?type=action&action=new','','scrollbars=yes,menubar=no,height=400,width=860,resizable=yes,toolbar=no,location=no,status=no');""], //no comma following last entry!" & vbNewLine
              Dim ValueURL As String = ""
              Dim ValueConfirmation As String = ""

              If SetAircraftValuationLink <> "" Then
                ValueConfirmation = "if (confirm('There is already an open market value analysis record for this Aircraft. Are you sure you want to create another one?')){"
              End If
              'This means there is no open value analysis:
              'If the listing source is client, then we can go ahead and send them directly to the note.
              If ListingSource = "CLIENT" Then
                ValueURL = "javascript: " & ValueConfirmation & "load('edit_note.aspx?action=new&amp;type=valuation&amp;cat_key=0&amp;refreshing=view','','scrollbars=yes,menubar=no,height=700,width=1250,resizable=yes,toolbar=no,location=no,status=no');" & IIf(SetAircraftValuationLink <> "", "};", "")
              Else 'Otherwise if the listing source is JETNET and other ID is blank, then we need to go ahead and ask them to create a client aircraft record.
                If OtherID = 0 Then
                  ValueURL = "javascript: " & ValueConfirmation & "CreateValuationRecord('edit.aspx?action=edit&type=aircraft&ac_ID=" & ListingID & "&source=JETNET&redirect=tovalue');" & IIf(SetAircraftValuationLink <> "", "};", "")
                Else 'Otherwise other ID exists and client record is there.
                  ValueURL = "javascript: " & ValueConfirmation & "load('edit_note.aspx?action=new&amp;type=valuation&amp;cat_key=0&amp;refreshing=view','','scrollbars=yes,menubar=no,height=700,width=1250,resizable=yes,toolbar=no,location=no,status=no');" & IIf(SetAircraftValuationLink <> "", "};", "")
                End If
              End If
              EditMenuString += "[""New Market Value Analysis"", """ & ValueURL & """] //no comma following last entry!" & vbNewLine


              If ListingSource = "CLIENT" Then
                EditMenuString += ",[""Edit Aircraft"", ""javascript:load('edit.aspx?action=edit&type=aircraft&ac_ID=" & ListingID & "&source=" & ListingSource & "','','scrollbars=yes,menubar=no,height=600,width=1030,resizable=yes,toolbar=no,location=no,status=no');""], //no comma following last entry!" & vbNewLine
                EditMenuString += "[""Synchronize Client AC"", ""javascript:load('edit.aspx?synch=true&type=aircraft&ac_ID=" & ListingID & "&source=" & ListingSource & "','','scrollbars=yes,menubar=no,height=460,width=400,resizable=yes,toolbar=no,location=no,status=no');""], //no comma following last entry!" & vbNewLine
                If Session.Item("ListingID") <> 0 Then
                  Dim Aircraft_Info As String = ""
                  aTempTable = aclsData_Temp.Client_Aircraft_Ser_Model(Session.Item("ListingID"))
                  If Not IsNothing(aTempTable) Then
                    If aTempTable.Rows.Count > 0 Then
                      Aircraft_Info = (aTempTable.Rows(0).Item("cliamod_make_name") & " " & aTempTable.Rows(0).Item("cliamod_model_name")) & " Serial # " & aTempTable.Rows(0).Item("cliaircraft_ser_nbr")
                    End If
                  Else
                    If aclsData_Temp.class_error <> "" Then
                      error_string = aclsData_Temp.class_error
                      LogError("main_site.Master.vb - Get_Clients_Aircraft() - " & error_string)
                    End If
                    display_error()
                  End If
                  aTempTable.Dispose()
                  EditMenuString += "[""Remove Client Aircraft"", ""javascript:if(confirm('Are you finished with your changes and would like to remove your " & Aircraft_Info & " Client Aircraft Record?'))javascript:load('edit.aspx?remove=true&type=aircraft&ac_ID=" & ListingID & "&source=" & ListingSource & "','scrollbars=yes,menubar=no,height=460,width=400,resizable=yes,toolbar=no,location=no,status=no');""] //no comma following last entry!" & vbNewLine
                End If
              End If

              Write_JSCRIPT = Write_JSCRIPT & vbNewLine & "var anylinkmenu3={divclass:'anylinkmenuwidest', inlinestyle:'', linktarget:''}" & vbNewLine
              Write_JSCRIPT = Write_JSCRIPT & "anylinkmenu3.items=[" & vbNewLine

              Write_JSCRIPT = Write_JSCRIPT & "[""Export Spec Sheet"", ""javascript:load('print_spec.aspx?ac_ID=" & ListingID & "&type=" & ListingSource & "','','scrollbars=yes,menubar=no,height=170,width=650,resizable=yes,toolbar=no,location=no,status=no');""] //no comma following last entry!" & vbNewLine


              Dim ValTable As New DataTable
              If ListingSource = "CLIENT" Then
                ValTable = aclsData_Temp.GetComparableValuesExcludingCertainAC(ListingID)
              Else 'If the listing source is jetnet, we check to see if the other ID exists
                'If it does, we use that one.
                ValTable = aclsData_Temp.GetComparableValuesExcludingCertainAC(OtherID)
              End If

              'Write_JSCRIPT = Write_JSCRIPT & vbNewLine & "var anylinkmenuvalue={divclass:'anylinkmenuwidest', inlinestyle:'', linktarget:''}" & vbNewLine
              'Write_JSCRIPT = Write_JSCRIPT & "anylinkmenuvalue.items=[" & vbNewLine

              If ValTable.Rows.Count > 0 Then
                For Each r As DataRow In ValTable.Rows
                  If ListingSource = "JETNET" Then
                    If OtherID = 0 Then
                      Write_JSCRIPT = Write_JSCRIPT & ",[""Add to " & r("cliamod_make_name").ToString & " " & r("cliamod_model_name").ToString & " Ser #" & r("cliaircraft_ser_nbr").ToString & " valuation "", ""javascript:load('edit.aspx?action=edit&type=aircraft&ac_ID=" & ListingID.ToString & "&source=" & ListingSource.ToString & "&auto_ac=true&from=view&viewNOTEID=" & r("lnote_id").ToString & "&activetab=1&ac_type=JETNET','scrollbars=yes,menubar=no,height=700,width=1250,resizable=yes,toolbar=no,location=no,status=no');""]" & vbNewLine
                    Else
                      Write_JSCRIPT = Write_JSCRIPT & ",[""Add to " & r("cliamod_make_name").ToString & " " & r("cliamod_model_name").ToString & " Ser #" & r("cliaircraft_ser_nbr").ToString & " valuation "", ""javascript:load('edit.aspx?action=edit&type=aircraft&j_ac_id=" & ListingID.ToString & "&ac_ID=" & OtherID.ToString & "&source=" & ListingSource.ToString & "&from=view&addValueOnly=true&viewNOTEID=" & r("lnote_id").ToString & "&activetab=1&ac_type=JETNET','scrollbars=yes,menubar=no,height=700,width=1250,resizable=yes,toolbar=no,location=no,status=no');""]" & vbNewLine

                    End If
                  Else
                    Write_JSCRIPT = Write_JSCRIPT & ",[""Add to " & r("cliamod_make_name").ToString & " " & r("cliamod_model_name").ToString & " Ser #" & r("cliaircraft_ser_nbr").ToString & " valuation "", ""javascript:load('edit.aspx?action=edit&type=aircraft&j_ac_id=" & OtherID & "&ac_ID=" & ListingID.ToString & "&source=" & ListingSource.ToString & "&from=view&addValueOnly=true&viewNOTEID=" & r("lnote_id").ToString & "&activetab=1&ac_type=JETNET','scrollbars=yes,menubar=no,height=700,width=1250,resizable=yes,toolbar=no,location=no,status=no');""]" & vbNewLine
                  End If
                Next

                'Write_JSCRIPT = Write_JSCRIPT & "]" & vbNewLine
                'Else
                '  Write_JSCRIPT = Write_JSCRIPT & "[""<em>No Available Actions</em>"", ""javascript:alert('No Available Actions at this time');""]" & vbNewLine
                '  Write_JSCRIPT = Write_JSCRIPT & "]" & vbNewLine
              End If


              Write_JSCRIPT = Write_JSCRIPT & "]" & vbNewLine


            End If

          Case 4

            'EditMenuString = vbNewLine & "var anylinkmenu2={divclass:'anylinkmenu', inlinestyle:'', linktarget:''}" & vbNewLine
            'EditMenuString += "anylinkmenu2.items=[" & vbNewLine
            EditMenuString = "[""New Action Item"" , ""javascript:load('edit_note.aspx?type=action&action=new','','scrollbars=yes,menubar=no,height=400,width=860,resizable=yes,toolbar=no,location=no,status=no');""] //no comma following last entry!" & vbNewLine

            Write_JSCRIPT = Write_JSCRIPT & vbNewLine & "var anylinkmenu3={divclass:'anylinkmenu', inlinestyle:'', linktarget:''}" & vbNewLine
            Write_JSCRIPT = Write_JSCRIPT & "anylinkmenu3.items=[" & vbNewLine


            Dim search As TextBox = ActionItemsSearch.FindControl("search_for_txt")
            Dim start As TextBox = ActionItemsSearch.FindControl("ad_start_date")
            Dim ended As TextBox = ActionItemsSearch.FindControl("ad_end_date")
            Dim user As DropDownList = ActionItemsSearch.FindControl("display_cbo")
            Dim search_where As DropDownList = ActionItemsSearch.FindControl("search_where")
            Dim order_by As DropDownList = ActionItemsSearch.FindControl("order_bo")

            Write_JSCRIPT = Write_JSCRIPT & "[""Custom Export"", ""javascript:load('export_creator.aspx?no=" & order_by.SelectedValue & "&sw=" & search_where.SelectedValue & "&nt=P&en=" & ended.Text & "&st=" & start.Text & "&sf=" & search.Text & "&us=" & IIf(user.SelectedValue = "", HttpContext.Current.Session.Item("localUser").crmLocalUserID, user.SelectedValue) & "&m=','','scrollbars=yes,menubar=no,height=560,width=1400,resizable=yes,toolbar=no,location=no,status=no');""] //no comma following last entry!" & vbNewLine
            Write_JSCRIPT = Write_JSCRIPT & "]" & vbNewLine


          Case 5
            'EditMenuString = vbNewLine & "var anylinkmenu2={divclass:'anylinkmenu', inlinestyle:'', linktarget:''}" & vbNewLine
            'EditMenuString += "anylinkmenu2.items=[" & vbNewLine
            EditMenuString = "[""New Job Seeker"", ""javascript:load('http://www.jetadvisors.com/development/admin/seeker_submittal.asp?new=true&crm=true','','scrollbars=yes,menubar=no,height=800,width=860,resizable=yes,toolbar=no,location=no,status=no');""] //no comma following last entry!" & vbNewLine

            Write_JSCRIPT = Write_JSCRIPT & vbNewLine & "var anylinkmenu3={divclass:'anylinkmenu', inlinestyle:'', linktarget:'new'}" & vbNewLine
            Write_JSCRIPT = Write_JSCRIPT & "anylinkmenu3.items=[" & vbNewLine
            Write_JSCRIPT = Write_JSCRIPT & "[""Custom Export"", ""export.aspx""] //no comma following last entry!" & vbNewLine
            Write_JSCRIPT = Write_JSCRIPT & "]" & vbNewLine
          Case 6
            'EditMenuString = vbNewLine & "var anylinkmenu2={divclass:'anylinkmenu', inlinestyle:'', linktarget:''}" & vbNewLine
            'EditMenuString += "anylinkmenu2.items=[" & vbNewLine
            EditMenuString = "[""New Note"", ""javascript:load('edit_note.aspx?type=note&action=new','','scrollbars=yes,menubar=no,height=400,width=860,resizable=yes,toolbar=no,location=no,status=no');""] //no comma following last entry!" & vbNewLine

            Write_JSCRIPT = Write_JSCRIPT & vbNewLine & "var anylinkmenu3={divclass:'anylinkmenu', inlinestyle:'', linktarget:''}" & vbNewLine
            Write_JSCRIPT = Write_JSCRIPT & "anylinkmenu3.items=[" & vbNewLine
            Dim search As TextBox = NotesSearch.FindControl("search_for_txt")
            Dim start As TextBox = NotesSearch.FindControl("ad_start_date")
            Dim ended As TextBox = NotesSearch.FindControl("ad_end_date")
            Dim user As DropDownList = NotesSearch.FindControl("display_cbo")
            Dim search_where As DropDownList = NotesSearch.FindControl("search_where")
            Dim order_by As DropDownList = NotesSearch.FindControl("order_bo")

            'Ac search fields.
            'Added 5/8/14
            Dim acSearchFieldOperatorDrop As New DropDownList
            Dim acSearchOperator As Integer = 0
            Dim acSearchFieldDrop As New DropDownList
            Dim acSearchField As Integer = 0
            Dim acSearchTextBox As New TextBox
            Dim acSearchText As String = ""
            Dim NoteCategory As Integer
            'New fields for folder
            Dim FolderType As Long = 3
            Dim FolderID As Long = 0
            If Not IsNothing(NotesSearch.FindControl("FolderType")) Then
              If IsNumeric(CType(NotesSearch.FindControl("FolderType"), DropDownList).SelectedValue) Then
                FolderType = CType(NotesSearch.FindControl("FolderType"), DropDownList).SelectedValue
              End If
            End If
            If Not IsNothing(NotesSearch.FindControl("listOfFolders")) Then
              If IsNumeric(CType(NotesSearch.FindControl("listOfFolders"), DropDownList).SelectedValue) Then
                FolderID = CType(NotesSearch.FindControl("listOfFolders"), DropDownList).SelectedValue
              End If
            End If


            If Not IsNothing(NotesSearch.FindControl("prospect_category")) Then
              If IsNumeric(CType(NotesSearch.FindControl("prospect_category"), DropDownList).SelectedValue) Then
                NoteCategory = CType(NotesSearch.FindControl("prospect_category"), DropDownList).SelectedValue
              End If
            End If


            'Set up the ac search field operator
            If Not IsNothing(NotesSearch.FindControl("ac_search_field_operator")) Then
              acSearchFieldOperatorDrop = NotesSearch.FindControl("ac_search_field_operator")
            End If
            'Set up the ac search field 
            If Not IsNothing(NotesSearch.FindControl("ac_search_field")) Then
              acSearchFieldDrop = NotesSearch.FindControl("ac_search_field")
            End If
            'Set up the ac search text field  
            If Not IsNothing(NotesSearch.FindControl("ac_search_field_text")) Then
              acSearchTextBox = NotesSearch.FindControl("ac_search_field_text")
            End If


            'Figure out the values
            clsGeneral.clsGeneral.Figure_Out_Note_Search_Fields(acSearchFieldDrop, acSearchFieldOperatorDrop, acSearchTextBox, acSearchField, acSearchOperator, acSearchText)



            Write_JSCRIPT = Write_JSCRIPT & "[""Custom Export"", ""javascript:load('export_creator.aspx?" & IIf(FolderType > 0, "pft=" & FolderType.ToString & "&", "") & IIf(FolderID > 0, "pfi=" & FolderID.ToString & "&", "") & "no=" & order_by.SelectedValue & "&sw=" & search_where.SelectedValue & "&nt=A&en=" & ended.Text & "&st=" & start.Text & "&sf=" & search.Text & "&us=" & user.SelectedValue & "&acOp=" & acSearchOperator & "&acSF=" & acSearchField & "&acST=" & acSearchText & "&ca=" & NoteCategory.ToString & "','','scrollbars=yes,menubar=no,height=560,width=1400,resizable=yes,toolbar=no,location=no,status=no');""] //no comma following last entry!" & vbNewLine
            Write_JSCRIPT = Write_JSCRIPT & "]" & vbNewLine
          Case 7
            Write_JSCRIPT = Write_JSCRIPT & vbNewLine & "var anylinkmenu3={divclass:'anylinkmenu', inlinestyle:'', linktarget:''}" & vbNewLine

            Write_JSCRIPT = Write_JSCRIPT & "anylinkmenu3.items=[" & vbNewLine
            Write_JSCRIPT = Write_JSCRIPT & "[""Custom Export"", ""javascript:load('export.aspx','','scrollbars=yes,menubar=no,height=560,width=910,resizable=yes,toolbar=no,location=no,status=no');""] //no comma following last entry!" & vbNewLine
            Write_JSCRIPT = Write_JSCRIPT & "]" & vbNewLine
          Case 16 'Prospects
            Write_JSCRIPT = Write_JSCRIPT & vbNewLine & "var anylinkmenu3={divclass:'anylinkmenu', inlinestyle:'', linktarget:''}" & vbNewLine
            Write_JSCRIPT = Write_JSCRIPT & "anylinkmenu3.items=[" & vbNewLine

            Dim search As TextBox = NotesSearch.FindControl("search_for_txt")
            Dim start As TextBox = NotesSearch.FindControl("ad_start_date")
            Dim ended As TextBox = NotesSearch.FindControl("ad_end_date")
            Dim user As DropDownList = NotesSearch.FindControl("display_cbo")
            Dim search_where As DropDownList = NotesSearch.FindControl("search_where")
            Dim order_by As DropDownList = NotesSearch.FindControl("order_bo")
            Dim ShowInactive As CheckBox = NotesSearch.FindControl("ShowInactiveProspect")

            Dim ProspectCategory As DropDownList = NotesSearch.FindControl("prospect_category")
            Dim ProspectTypeSearch As DropDownList = NotesSearch.FindControl("prospect_search_by_dropdown")
            Dim ProspectAcBox As New ListBox

            If ProspectTypeSearch.SelectedValue = 1 Then
              ProspectAcBox = NotesSearch.FindControl("ac_prospect_list")
            End If

            Dim oppStatus As String = "A"
            If ShowInactive.Checked = True Then
              oppStatus = ""
            End If

            Write_JSCRIPT = Write_JSCRIPT & "[""Custom Export"", ""javascript:load('export_creator.aspx?acp=" & ProspectAcBox.ClientID & "&amp;pty=" & ProspectTypeSearch.SelectedValue & "&amp;ca=" & ProspectCategory.SelectedValue & "&amp;opp=" & oppStatus & "&amp;no=" & order_by.SelectedValue & "&sw=" & search_where.SelectedValue & "&nt=B&en=" & ended.Text & "&st=" & start.Text & "&sf=" & search.Text & "&us=" & user.SelectedValue & "','','scrollbars=yes,menubar=no,height=560,width=1400,resizable=yes,toolbar=no,location=no,status=no');""] //no comma following last entry!" & vbNewLine
            Write_JSCRIPT = Write_JSCRIPT & "]" & vbNewLine

          Case 11
            'EditMenuString = vbNewLine & "var anylinkmenu2={divclass:'anylinkmenu', inlinestyle:'', linktarget:''}" & vbNewLine
            'EditMenuString += "anylinkmenu2.items=[" & vbNewLine
            EditMenuString = "[""New Opportunity"", ""javascript:load('edit_note.aspx?type=opportunity&action=new','','scrollbars=yes,menubar=no,height=400,width=860,resizable=yes,toolbar=no,location=no,status=no');""] //no comma following last entry!" & vbNewLine



            Dim search As TextBox = OpportunitiesSearch.FindControl("search_for_txt")
            Dim start As TextBox = OpportunitiesSearch.FindControl("ad_start_date")
            Dim ended As TextBox = OpportunitiesSearch.FindControl("ad_end_date")
            Dim user As DropDownList = OpportunitiesSearch.FindControl("display_cbo")
            Dim search_where As DropDownList = OpportunitiesSearch.FindControl("search_where")
            Dim opp As DropDownList = OpportunitiesSearch.FindControl("opportunity_status")
            Dim cat As DropDownList = OpportunitiesSearch.FindControl("notes_cat")
            Write_JSCRIPT = Write_JSCRIPT & vbNewLine & "var anylinkmenu3={divclass:'anylinkmenu', inlinestyle:'', linktarget:''}" & vbNewLine

            Write_JSCRIPT = Write_JSCRIPT & "anylinkmenu3.items=[" & vbNewLine
            Write_JSCRIPT = Write_JSCRIPT & "[""Custom Export"", ""javascript:load('export_creator.aspx?opp=" & opp.SelectedValue & "&ca=" & cat.SelectedValue & "&sw=" & search_where.SelectedValue & "&nt=O&en=" & ended.Text & "&st=" & start.Text & "&sf=" & search.Text & "&us=" & user.SelectedValue & "&m=','','scrollbars=yes,menubar=no,height=560,width=1400,resizable=yes,toolbar=no,location=no,status=no');""] //no comma following last entry!" & vbNewLine
            Write_JSCRIPT = Write_JSCRIPT & "]" & vbNewLine


          Case 8
            'EditMenuString = vbNewLine & "var anylinkmenu2={divclass:'anylinkmenu', inlinestyle:'', linktarget:''}" & vbNewLine
            'EditMenuString += "anylinkmenu2.items=[" & vbNewLine


            Write_JSCRIPT = Write_JSCRIPT & vbNewLine & "var anylinkmenu3={divclass:'anylinkmenu', inlinestyle:'', linktarget:''}" & vbNewLine
            Write_JSCRIPT = Write_JSCRIPT & "anylinkmenu3.items=[" & vbNewLine
            Write_JSCRIPT = Write_JSCRIPT & "[""Save As - New Folder"", ""javascript:crmSaveFolder('TransactionSearch','trfolder',0);""]," & vbNewLine
            If IsSubNode Then
              If Subnode_Method = "A" Then
                Write_JSCRIPT = Write_JSCRIPT & "[""Save Current Folder"", ""javascript:crmSaveFolder('TransactionSearch','trfolder'," & SubNodeOfListing & ");""]," & vbNewLine
              End If
            End If

            Dim model_cbo As ListBox = TransactionSearch.FindControl("model_cbo")
            Dim trans_type_cbo As DropDownList = TransactionSearch.FindControl("trans_type_cbo")
            Dim datad As DropDownList = TransactionSearch.FindControl("subset")
            Dim search As TextBox = TransactionSearch.FindControl("search_for_txt")
            Dim search_where As DropDownList = TransactionSearch.FindControl("search_where")
            Dim year_start As String = CType(TransactionSearch.FindControl("year_start"), DropDownList).SelectedValue
            Dim year_end As String = CType(TransactionSearch.FindControl("year_end"), DropDownList).SelectedValue

            Dim internal As DropDownList = TransactionSearch.FindControl("internal_trans")

            Dim awaiting As CheckBox = TransactionSearch.FindControl("awaiting")
            Dim aval As String = ""
            If awaiting.Checked = True Then
              aval = "Y"
            Else
              aval = "N"
            End If


            Dim start As TextBox = TransactionSearch.FindControl("start_date_txt")
            Dim ended As TextBox = TransactionSearch.FindControl("end_date_txt")


            Dim models As String = ""
            Write_JSCRIPT = Write_JSCRIPT & "[""Custom Export"", ""javascript:load('export_creator.aspx?ad=" & aval & "&in=" & internal.SelectedValue & "&tys=" & year_start & "&tye=" & year_end & "&se=" & search.Text & "&sw=" & search_where.SelectedValue & "&m=" & models & "&t=" & trans_type_cbo.SelectedValue & "&s=" & start.Text & "&e=" & ended.Text & "&d=" & datad.SelectedValue & "','','scrollbars=yes,menubar=no,height=560,width=1400,resizable=yes,toolbar=no,location=no,status=no');""] //no comma following last entry!" & vbNewLine
            Write_JSCRIPT = Write_JSCRIPT & "]" & vbNewLine
          Case Else
            If ListingID = 0 Then

              'EditMenuString = vbNewLine & "var anylinkmenu2={divclass:'anylinkmenu', inlinestyle:'', linktarget:''}" & vbNewLine
              'EditMenuString += "anylinkmenu2.items=[" & vbNewLine
              EditMenuString = "[""New Company"", ""javascript:load('edit.aspx?action=new&type=company','','scrollbars=yes,menubar=no,height=620,width=1030,resizable=yes,toolbar=no,location=no,status=no');""], //no comma following last entry!" & vbNewLine
              EditMenuString += "[""Contact Quick Entry"", ""javascript:load('edit.aspx?action=quick','','scrollbars=yes,menubar=no,height=750,width=1110,resizable=yes,toolbar=no,location=no,status=no');""]" & vbNewLine

              Write_JSCRIPT = Write_JSCRIPT & vbNewLine & "var anylinkmenu3={divclass:'anylinkmenu', inlinestyle:'', linktarget:''}" & vbNewLine
              Write_JSCRIPT = Write_JSCRIPT & "anylinkmenu3.items=[" & vbNewLine
              Write_JSCRIPT = Write_JSCRIPT & "[""Save As - New Folder"", ""javascript:crmSaveFolder('companySearch','cyfolder',0);""]" & vbNewLine

              If IsSubNode Then
                If Subnode_Method = "A" Then
                  Write_JSCRIPT = Write_JSCRIPT & ",[""Save Current Folder"", ""javascript:crmSaveFolder('companySearch','cyfolder'," & SubNodeOfListing & ");""]" & vbNewLine
                End If
              End If

              Dim state As ListBox = companySearch.FindControl("state")
              Dim owners As DropDownList = companySearch.FindControl("types_of_owners")
              Dim country As DropDownList = companySearch.FindControl("country")
              Dim subset As DropDownList = companySearch.FindControl("subset")
              Dim search As TextBox = companySearch.FindControl("search_for_txt")
              Dim search_where As DropDownList = companySearch.FindControl("search_where")
              Dim special_field_cbo As DropDownList = companySearch.FindControl("special_field_cbo")
              Dim special_field_txt As TextBox = companySearch.FindControl("special_field_txt")
              Dim show_all As CheckBox = companySearch.FindControl("show_all")
              Dim status As DropDownList = companySearch.FindControl("status_cbo")
              Dim companyCity As New TextBox
              Dim companyPhone As New TextBox

              If Not IsNothing(companySearch.FindControl("company_phone_number")) Then
                companyPhone = companySearch.FindControl("company_phone_number")
              End If

              If Not IsNothing(companySearch.FindControl("city_textbox")) Then
                companyCity = companySearch.FindControl("city_textbox")
              End If

              Dim states As String = ""
              For i = 0 To state.Items.Count - 1
                If state.Items(i).Selected Then
                  If state.Items(i).Value <> "" Then
                    states = states & "" & state.Items(i).Value & ","
                  End If
                End If
              Next

              If states <> "" Then
                states = UCase(states.TrimEnd(","))
              End If

              Dim sub_text As String = ""
              If NameOfListingType <> NameOfSubnode Then
                sub_text = NameOfSubnode
              End If

              If Session.Item("localUser").crmEvo = True Then
                Write_JSCRIPT = Write_JSCRIPT & ",[""Custom Export"", ""javascript:load('evo_exporter.aspx?snt=" & sub_text & "&st=" & status.SelectedValue & "&sn=" & SubNodeOfListing & "&all=" & show_all.Checked & "&sp_txt=" & special_field_txt.Text & "&sp_cbo=" & special_field_cbo.SelectedValue & "&state=" & states & "&owners=" & owners.SelectedValue & "&country=" & country.SelectedValue & "&su=" & subset.SelectedValue & "&search=" & clsGeneral.clsGeneral.Get_Name_Search_String(search.Text) & "&sw=" & search_where.SelectedValue & "','','scrollbars=yes,menubar=no,height=800,width=1060,resizable=yes,toolbar=no,location=no,status=no');""] //no comma following last entry!" & vbNewLine
              Else
                Write_JSCRIPT = Write_JSCRIPT & ",[""Custom Export"", ""javascript:load('export_creator.aspx?ccp=" & companyPhone.Text & "&ccs=" & companyCity.Text & "&snt=" & sub_text & "&st=" & status.SelectedValue & "&sn=" & SubNodeOfListing & "&all=" & show_all.Checked & "&sp_txt=" & special_field_txt.Text & "&sp_cbo=" & special_field_cbo.SelectedValue & "&state=" & states & "&owners=" & owners.SelectedValue & "&country=" & country.SelectedValue & "&su=" & subset.SelectedValue & "&search=" & clsGeneral.clsGeneral.Get_Name_Search_String(search.Text) & "&sw=" & search_where.SelectedValue & "','','scrollbars=yes,menubar=no,height=560,width=1400,resizable=yes,toolbar=no,location=no,status=no');""] //no comma following last entry!" & vbNewLine
              End If

              Write_JSCRIPT = Write_JSCRIPT & "]" & vbNewLine
            Else

              If Listing_IsJob = False Then
                'EditMenuString = vbNewLine & "var anylinkmenu2={divclass:'anylinkmenuwide', inlinestyle:'', linktarget:''}" & vbNewLine
                'EditMenuString += "anylinkmenu2.items=[" & vbNewLine
                EditMenuString = "[""New Company"", ""javascript:load('edit.aspx?action=new&type=company','','scrollbars=yes,menubar=no,height=620,width=1030,resizable=yes,toolbar=no,location=no,status=no');""], //no comma following last entry!" & vbNewLine

                If ListingSource = "CLIENT" Then
                  EditMenuString += "[""Aircraft Relationship"", ""javascript:load('edit.aspx?action=reference','','scrollbars=yes,menubar=no,height=400,width=800,resizable=yes,toolbar=no,location=no,status=no');""], //no comma following last entry!" & vbNewLine
                End If

                EditMenuString += "[""New Action Item"", ""javascript:load('edit_note.aspx?type=action&action=new','','scrollbars=yes,menubar=no,height=450,width=860,resizable=yes,toolbar=no,location=no,status=no');""], //no comma following last entry!" & vbNewLine
                EditMenuString += "[""Edit Company"", ""javascript:load('edit.aspx?type=company&action=edit&comp_ID=" & ListingID & "&source=" & ListingSource & "','','scrollbars=yes,menubar=no,height=620,width=1050,resizable=yes,toolbar=no,location=no,status=no');""] //no comma following last entry!" & vbNewLine

                If ListingSource = "CLIENT" Then
                  EditMenuString += ",[""Identify Main Location"", ""javascript:load('edit.aspx?type=company&main_location=true&comp_ID=" & ListingID & "&source=" & ListingSource & "','','scrollbars=yes,menubar=no,height=500,width=1010,resizable=yes,toolbar=no,location=no,status=no');""] //no comma following last entry!" & vbNewLine

                  EditMenuString += ",[""Combine Companies"", ""javascript:load('edit.aspx?type=company&combine=true&comp_ID=" & ListingID & "&source=" & ListingSource & "','','scrollbars=yes,menubar=no,height=500,width=1010,resizable=yes,toolbar=no,location=no,status=no');""] //no comma following last entry!" & vbNewLine
                  If OtherID = 0 Then
                    EditMenuString += ",[""Relate to JETNET Company"", ""javascript:load('edit.aspx?type=company&connect=true&comp_ID=" & ListingID & "&source=" & ListingSource & "','','scrollbars=yes,menubar=no,height=500,width=1010,resizable=yes,toolbar=no,location=no,status=no');""] //no comma following last entry!" & vbNewLine
                  Else
                    EditMenuString += ",[""Synchronize Company"", ""javascript:load('edit.aspx?type=company&synch=true&comp_ID=" & ListingID & "&source=" & ListingSource & "','','scrollbars=yes,menubar=no,height=500,width=1010,resizable=yes,toolbar=no,location=no,status=no');""] //no comma following last entry!" & vbNewLine

                    EditMenuString += ",[""Remove/Update JETNET Relation"", ""javascript:load('edit.aspx?type=company&connect=true&comp_ID=" & ListingID & "&source=" & ListingSource & "','','scrollbars=yes,menubar=no,height=500,width=1010,resizable=yes,toolbar=no,location=no,status=no');""], //no comma following last entry!" & vbNewLine
                    If ListingID <> 0 Then
                      Dim company_Info As String = ""

                      aTempTable = aclsData_Temp.GetCompanyInfo_ID(ListingID, "CLIENT", 0)
                      If Not IsNothing(aTempTable) Then
                        If aTempTable.Rows.Count > 0 Then
                          company_Info = (aTempTable.Rows(0).Item("comp_name"))
                          '   company_email = aTempTable.Rows(0).Item("comp_email_address")
                        End If
                      Else
                        If aclsData_Temp.class_error <> "" Then
                          error_string = aclsData_Temp.class_error
                          LogError("main_site.Master.vb - GetCompanyInfo_ID() - " & error_string)
                        End If
                        display_error()
                      End If
                      aTempTable.Dispose()
                      EditMenuString += "[""Remove Client Company"", ""javascript:if(confirm('Are you finished with your changes and would like to remove your " & company_Info & " Client Company Record?'))javascript:load('edit.aspx?remove=true&type=company&comp_ID=" & ListingID & "&source=" & ListingSource & "','scrollbars=yes,menubar=no,height=460,width=400,resizable=yes,toolbar=no,location=no,status=no');""] //no comma following last entry!" & vbNewLine
                    End If
                  End If
                End If

              Else
                'EditMenuString = vbNewLine & "var anylinkmenu2={divclass:'anylinkmenu', inlinestyle:'', linktarget:''}" & vbNewLine
                'EditMenuString += "anylinkmenu2.items=[" & vbNewLine
                EditMenuString = "[""Edit Job Seeker"", ""javascript:load('http://www.jetadvisors.com/development/admin/seeker_submittal.asp?id=" & Trim(Request("job")) & "&crm=true','','scrollbars=yes,menubar=no,height=800,width=860,resizable=yes,toolbar=no,location=no,status=no');""] //no comma following last entry!" & vbNewLine
                EditMenuString += "]" & vbNewLine
              End If

              Write_JSCRIPT = Write_JSCRIPT & vbNewLine & "var anylinkmenu3={divclass:'anylinkmenu', inlinestyle:'', linktarget:'new'}" & vbNewLine
              Write_JSCRIPT = Write_JSCRIPT & "anylinkmenu3.items=[" & vbNewLine
              Write_JSCRIPT = Write_JSCRIPT & "[""Company to Outlook"", ""export.aspx?outlook=true&parent=1&amp;source=" & ListingSource & "&amp;id=" & ListingID & """] //no comma following last entry!" & vbNewLine

              If ListingSource = "CLIENT" Then
                '    aTempTable = aclsData_Temp.Get_Client_JETNET_AC(ListingID, "ac_id ASC", Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmCommercial_Flag, Session.Item("localSubscription").crmJets_Flag, Session.Item("localSubscription").crmExecutive_Flag, Session.Item("localSubscription").crmTurboprops)
              ElseIf ListingSource = "JETNET" Then
                '   aTempTable = aclsData_Temp.GetAircraft_Listing_compid(ListingID, Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmCommercial_Flag, Session.Item("localSubscription").crmJets_Flag, Session.Item("localSubscription").crmExecutive_Flag, Session.Item("localSubscription").crmTurboprops, 0, Session.Item("localSubscription").crmAerodexFlag)
              End If

              If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count <> 0 Then

                  Write_JSCRIPT = Write_JSCRIPT & ",[""AC list to Excel"", ""export.aspx?ac_list=true""] //no comma following last entry!" & vbNewLine

                End If
              End If
              aTempTable = Nothing

              If Not Listing_ContactID = 0 Then
                Write_JSCRIPT = Write_JSCRIPT & ",[""Contact to Outlook"", ""export.aspx?outlook=true&parent=2&amp;source=" & ListingSource & "&amp;id=" & Listing_ContactID & """] //no comma following last entry!" & vbNewLine
              End If

              Write_JSCRIPT = Write_JSCRIPT & "]" & vbNewLine
            End If

        End Select

        Write_JSCRIPT += vbNewLine & "var anylinkmenu2={divclass:'anylinkmenuwide', inlinestyle:'', linktarget:''}" & vbNewLine
        Write_JSCRIPT += "anylinkmenu2.items=[" & vbNewLine
        If EditMenuString <> "" Then
          Write_JSCRIPT += EditMenuString + ","
        End If
        Write_JSCRIPT += "" + FolderMenuEditString
        Write_JSCRIPT += "]"

        Write_JSCRIPT = Write_JSCRIPT & "</script>"
      End If
    Catch ex As Exception
      error_string = "main_site.Master.vb - Write_JSCRIPT() - " & ex.Message
      LogError(error_string)
    End Try
  End Function
  Public Function what_cat(ByVal x As Integer, ByVal y As String, ByVal reverse As Boolean) As String
    what_cat = x
    Try
      If Not IsDBNull(x) Then
        aTempTable = aclsData_Temp.Get_Client_Note_Category

        If reverse <> True Then
          If Not IsNothing(aTempTable) Then
            If aTempTable.Rows.Count > 0 Then
              For Each R As DataRow In aTempTable.Rows
                'notecat_key, notecat_name
                If x = R("notecat_key") Then
                  what_cat = R("notecat_name")
                End If
              Next
            Else
            End If
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = aclsData_Temp.class_error
              LogError("main_site.Master.vb - what_cat() - " & error_string)
            End If
            display_error()
            what_cat = x
          End If
        Else
          If Not IsNothing(aTempTable) Then
            If aTempTable.Rows.Count > 0 Then
              For Each R As DataRow In aTempTable.Rows
                'notecat_key, notecat_name
                If UCase(y) = UCase(R("notecat_name")) Then
                  what_cat = R("notecat_key")
                End If
              Next
            Else
            End If
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = aclsData_Temp.class_error
              LogError("main_site.Master.vb - what_cat() - " & error_string)
            End If
            display_error()
            what_cat = x
          End If
        End If
      End If

      what_cat = UCase(what_cat)
    Catch ex As Exception
      error_string = "main_site.Master.vb - what_cat() - " & ex.Message
      LogError(error_string)
    End Try

  End Function
  Function add_comp_name(ByVal q As Integer, ByVal show As Integer, ByVal source As String)

    'This adds the company name for notes and action display
    add_comp_name = ""
    If TypeOfListing <> 1 Then
      '---------------------------Aircraft Contact Information-----------------------------------------------------
      Try
        Dim strContact As String = ""
        ' get the contact info
        Dim compID As Integer = q
        aTempTable = aclsData_Temp.GetCompanyInfo_ID(compID, source, 0)

        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            For Each r As DataRow In aTempTable.Rows
              If show = 2 Then
                strContact = " (<em>"
                If Not (IsDBNull(r("comp_name"))) Then
                  If r("comp_name") <> "" Then
                    strContact = strContact & "" & r("comp_name") & " "
                  End If
                End If
                If Not (IsDBNull(r("comp_city"))) Then
                  If r("comp_city") <> "" Then
                    strContact = strContact & r("comp_city") & " "
                  End If
                End If
                If Not (IsDBNull(r("comp_state"))) Then
                  If r("comp_state") <> "" Then
                    strContact = strContact & r("comp_state") & " "
                  End If
                End If
                If Not (IsDBNull(r("comp_country"))) Then
                  If r("comp_country") <> "" Then
                    strContact = strContact & r("comp_country")
                  End If
                End If
                strContact = " - " & strContact & "</em>)"
              End If
              add_comp_name = strContact
            Next
          Else ' 0 rows
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("main_site.Master.vb - add_comp_name() - " & error_string)
          End If
          display_error()
        End If
      Catch ex As Exception
        error_string = "main_site.Master.vb - add_comp_name() - " & ex.Message
        LogError(error_string)
      End Try
    Else
      If show = 2 Then
        add_comp_name = " (<em>" & source & " Company</em>)"
      End If
    End If

  End Function
  Function add_ac_name(ByVal idnum As Integer, ByVal show As Integer, ByVal source As String)
    'This adds the aircraft name for notes and action display
    add_ac_name = ""
    Try
      If source = "JETNET" Then
        If TypeOfListing <> 3 Then
          Dim aircraft_text As String = ""
          Dim aError As String = ""
          aTempTable = aclsData_Temp.GetJETNET_AC_NAME(idnum, aError)
          ' check the state of the DataTable
          If Not IsNothing(aTempTable) Then
            If aTempTable.Rows.Count > 0 Then
              For Each R As DataRow In aTempTable.Rows

                'check for flags
                aircraft_text = ""
                If show = 2 Then
                  aircraft_text = " (<em>"
                  If Not IsDBNull(R("ac_year_mfr")) Then
                    If R("ac_year_mfr") <> "" Then
                      aircraft_text = aircraft_text & R("ac_year_mfr") & " "
                    End If
                  End If
                  aircraft_text = aircraft_text & R("amod_make_name") & " " & R("amod_model_name") & " - "
                  If Not IsDBNull(R("ac_reg_nbr")) Then
                    If R("ac_reg_nbr") <> "" Then
                      aircraft_text = aircraft_text & "Reg #: " & R("ac_reg_nbr") & " - "
                    End If
                  End If
                  add_ac_name = aircraft_text & "</em>)"
                End If

                'If show = 1 Then
                If Not IsDBNull(R("ac_ser_nbr")) Then
                  If R("ac_ser_nbr") <> "" Then
                    aircraft_text = aircraft_text & "Ser #:" & R("ac_ser_nbr") & "</em>)"
                  End If
                End If
                'End If
                add_ac_name = aircraft_text
              Next
            Else ' 0 rows
            End If
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = aclsData_Temp.class_error
              LogError("main_site.Master.vb - add_ac_name() - " & error_string)
            End If
            display_error()
          End If
        Else
          If show = 2 Then
            add_ac_name = " (<em>" & source & " AC</em>)"
          End If
        End If
      Else
        If TypeOfListing <> 3 Then
          Dim aircraft_text As String = ""
          Dim aError As String = ""
          aTempTable = aclsData_Temp.Get_Clients_Aircraft(idnum)
          ' check the state of the DataTable
          If Not IsNothing(aTempTable) Then
            If aTempTable.Rows.Count > 0 Then
              For Each R As DataRow In aTempTable.Rows
                aircraft_text = ""
                If show = 2 Then
                  aircraft_text = " (<em>"
                  If Not IsDBNull(R("cliaircraft_year_mfr")) Then
                    If R("cliaircraft_year_mfr") <> "" Then
                      aircraft_text = aircraft_text & "Year: " & R("cliaircraft_year_mfr") & " "
                    End If
                  End If
                  If Not IsDBNull(R("cliaircraft_reg_nbr")) Then
                    If R("cliaircraft_reg_nbr") <> "" Then
                      aircraft_text = aircraft_text & "Reg #: " & R("cliaircraft_reg_nbr") & "  "
                    End If
                  End If
                End If

                'If show = 1 Then
                If Not IsDBNull(R("cliaircraft_ser_nbr")) Then
                  If R("cliaircraft_ser_nbr") <> "" Then
                    aircraft_text = aircraft_text & "Ser #:" & R("cliaircraft_ser_nbr") & "</em>)"
                  End If
                End If
                'End If
                add_ac_name = aircraft_text
              Next
            Else ' 0 rows
            End If
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = aclsData_Temp.class_error
              LogError("main_site.Master.vb - add_ac_name() - " & error_string)
            End If
            display_error()
          End If
        Else
          If show = 2 Then
            add_ac_name = " (<em>" & source & " AC</em>)"
          End If
        End If
      End If
    Catch ex As Exception
      error_string = "main_site.Master.vb - add_ac_name() - " & ex.Message
      LogError(error_string)
    End Try
  End Function
  Public Function GenerateActionItemStartDate(ByRef StartDate As Object) As String
    Dim returnString As String = ""
    If Not IsNothing(StartDate) Then
      If Not IsDBNull(StartDate) Then
        If IsDate(StartDate) Then
          returnString = DateAdd("h", Session("timezone_offset"), FormatDateTime(StartDate))
        End If
      End If
    End If
    Return returnString
  End Function
#End Region
#Region "Public Subs"
#Region "My Aircraft (company, etc) menu dropdown subs"
  Public Sub remove_all_selected_items(ByVal cookie_name As String, ByVal folder_name As String)
    Try
      Dim _acmarked As HttpCookie = Request.Cookies(cookie_name)
      Dim client_ids As String = ""
      Dim jetnet_ids As String = ""
      Dim jetnet_ac_id As Integer = 0
      Dim client_ac_id As Integer = 0
      Dim jetnet_comp_id As Integer = 0
      Dim client_comp_id As Integer = 0
      Dim jetnet_contact_id As Integer = 0
      Dim client_contact_id As Integer = 0

      If _acmarked IsNot Nothing Then
        Dim _acmarked_val As String = Request.Cookies(cookie_name).Value
        If _acmarked_val <> "" Then
          Dim arrayed As Array = Split(_acmarked_val, "|")
          Dim my_aircraft_folder As Integer = 0
          my_aircraft_folder = SubNodeOfListing
          For x = 0 To UBound(arrayed)
            If arrayed(x) <> "" Then
              Dim list_ids As Array = Split(arrayed(x), "#")
              Select Case list_ids(1)
                Case "CLIENT"
                  Select Case TypeOfListing
                    Case 1
                      client_comp_id = list_ids(0)
                      jetnet_comp_id = 0
                    Case 3
                      client_ac_id = list_ids(0)
                      jetnet_ac_id = 0
                  End Select
                Case "JETNET"
                  Select Case TypeOfListing
                    Case 1
                      jetnet_comp_id = list_ids(0)
                      client_comp_id = 0
                    Case 3
                      jetnet_ac_id = list_ids(0)
                      client_ac_id = 0
                  End Select
              End Select

              aTempTable = aclsData_Temp.Get_ClientFolderIndex_Search(my_aircraft_folder, jetnet_ac_id, jetnet_comp_id, jetnet_contact_id, client_ac_id, client_comp_id, client_contact_id)
              If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count <> 0 Then
                  If aclsData_Temp.Delete_Client_Folder_Index(CInt(aTempTable.Rows(0).Item("cfoldind_id")), CInt(my_aircraft_folder)) = 1 Then
                  End If
                End If
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("main_site.Master.vb - remove_all_selected_items() - " & error_string)
                End If
                display_error()
              End If
            End If
          Next
          '
        End If
      End If
      Response.Cookies(cookie_name).Value = ""
      Response.Redirect(Request.Url.ToString, False)
      HttpContext.Current.ApplicationInstance.CompleteRequest()
      m_bIsTerminating = True
    Catch ex As Exception
      error_string = "main_site.Master.vb - remove_all_selected_items() - " & ex.Message
      LogError(error_string)
    End Try

  End Sub
  Public Sub mark_all_selected_items(ByVal cookie_name As String, ByVal folder_name As String)
    Try
      Dim _acmarked As HttpCookie = Request.Cookies(cookie_name)
      Dim client_ids As String = ""
      Dim jetnet_ids As String = ""
      Dim jetnet_ac_id As Integer = 0
      Dim client_ac_id As Integer = 0
      Dim jetnet_comp_id As Integer = 0
      Dim client_comp_id As Integer = 0
      Dim jetnet_contact_id As Integer = 0
      Dim client_contact_id As Integer = 0

      If _acmarked IsNot Nothing Then
        Dim _acmarked_val As String = Request.Cookies(cookie_name).Value
        Dim arrayed As Array = Split(_acmarked_val, "|")
        Dim my_aircraft_folder As Integer = 0
        'Get myaircraft subnode, or creates the my (whatever) folder if it doesn't exist
        aTempTable = aclsData_Temp.Get_Client_Folders_MyAircraft(folder_name, CInt(Session.Item("localUser").crmLocalUserID), False)
        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then 'gets subnode of exists
            my_aircraft_folder = aTempTable.Rows(0).Item("cfolder_id")
          Else
            'create my  folder
            If aclsData_Temp.Insert_Into_Client_Folder(TypeOfListing, folder_name, CInt(Session.Item("localUser").crmLocalUserID), "N", "N", 2, 1) <> 0 Then
              aTempTable2 = aclsData_Temp.Get_Client_Folders_MyAircraft(folder_name, CInt(Session.Item("localUser").crmLocalUserID), False)
              If Not IsNothing(aTempTable2) Then
                If aTempTable2.Rows.Count > 0 Then
                  my_aircraft_folder = aTempTable2.Rows(0).Item("cfolder_id")
                Else
                  If aclsData_Temp.class_error <> "" Then
                    error_string = aclsData_Temp.class_error
                    LogError("main_site.Master.vb - mark_all_selected_items() - " & error_string)
                  End If
                  display_error()
                End If
              End If
            End If
          End If
        End If
        For x = 0 To UBound(arrayed)
          If arrayed(x) <> "" Then
            Dim list_ids As Array = Split(arrayed(x), "#")
            Select Case list_ids(1)
              Case "CLIENT"
                Select Case TypeOfListing
                  Case 1
                    client_comp_id = list_ids(0)
                    jetnet_comp_id = 0
                  Case 2
                    client_contact_id = list_ids(0)
                    jetnet_contact_id = 0
                  Case 3
                    client_ac_id = list_ids(0)
                    jetnet_ac_id = 0
                End Select
              Case "JETNET"
                Select Case TypeOfListing
                  Case 1
                    jetnet_comp_id = list_ids(0)
                    client_comp_id = 0
                  Case 2
                    jetnet_contact_id = list_ids(0)
                    client_contact_id = 0
                  Case 3
                    jetnet_ac_id = list_ids(0)
                    client_ac_id = 0
                End Select
            End Select
            aTempTable = aclsData_Temp.Get_ClientFolderIndex_Search(my_aircraft_folder, jetnet_ac_id, jetnet_comp_id, jetnet_contact_id, client_ac_id, client_comp_id, client_contact_id)
            If Not IsNothing(aTempTable) Then
              If aTempTable.Rows.Count = 0 Then
                If aclsData_Temp.Insert_Into_Client_Folder_Index(my_aircraft_folder, jetnet_ac_id, jetnet_comp_id, jetnet_contact_id, client_ac_id, client_comp_id, client_contact_id, 0, "") = 1 Then
                Else
                  If aclsData_Temp.class_error <> "" Then
                    error_string = aclsData_Temp.class_error
                    LogError("main_site.Master.vb - mark_all_selected_items() - " & error_string)
                  End If
                  display_error()
                End If
              End If
            End If
          End If
        Next

        Response.Cookies(cookie_name).Value = ""
      End If
      TreeNav.Make_TreeView()
      'Response.Redirect(Request.Url.ToString, False)
    Catch ex As Exception
      error_string = "main_site.Master.vb - mark_all_selected_items() - " & ex.Message
      LogError(error_string)
    End Try

  End Sub
  Public Function IsInCookie(ByVal what_box As String) As String
    IsInCookie = """"
    Dim cookie_name As String = ""
    Select Case TypeOfListing
      Case 1
        cookie_name = "companies_marked"
      Case 2
        cookie_name = "contacts_marked"
      Case 3
        cookie_name = "aircraft_marked"
    End Select
    Dim _acmarked As HttpCookie = Request.Cookies(cookie_name)
    If _acmarked IsNot Nothing Then
      what_box = UCase(what_box)
      Dim _acmarked_val As String = "|" & Request.Cookies(cookie_name).Value & "|"
      If what_box <> "#" Then
        If InStr(_acmarked_val, "|" & what_box & "|") > 0 Then
          IsInCookie = """ checked= 'yes'"
        Else
          IsInCookie = """"
        End If
      ElseIf what_box = "#" Then
        IsInCookie = "display:none;"
      End If
    ElseIf what_box = "#" Then
      IsInCookie = "display:none;"
    End If
  End Function
#End Region
  Sub addOtherID(ByVal c As ImageButton)
    'Try
    '    Dim switch_view As ImageButton
    '    'switch_view_text = SubNav1.FindControl("switch_view_text")
    '    'switch_view_text.Controls.Add(c)
    '    switch_view = SubNav1.FindControl("switch")
    '    switch_view.ImageUrl = c.ImageUrl
    '    switch_view.
    'Catch ex As Exception
    '    error_string = "main_site.Master.vb - addOtherID() - " & ex.Message
    '    LogError(error_string)
    'End Try
  End Sub
  Public Sub Search_display()
    Try
      Select Case TypeOfListing
        Case 14, 15
          WantedSearch.Visible = False
          SubNav1.Visible = False 'True
          record_holder.Visible = False
          NameOfListingType = "" '"Main"
          companySearch.Visible = False
          aircraftSearch.Visible = False
          ContactSearch.Visible = False
          NotesSearch.Visible = False
          ActionItemsSearch.Visible = False
          JobsSearch.Visible = False
          DocumentSearch.Visible = False
          OpportunitiesSearch.Visible = False
          TransactionSearch.Visible = False
          DayPilotCalendar1.Visible = False
          btnDayPilotCalendar_Previous.Visible = False
          btnDayPilotCalendar_Next.Visible = False
          market_search.Visible = False
        Case 9, 13
          WantedSearch.Visible = False
          SubNav1.Visible = True
          record_holder.Visible = False
          NameOfListingType = "" '"Main"
          companySearch.Visible = False
          aircraftSearch.Visible = False
          ContactSearch.Visible = False
          NotesSearch.Visible = False
          ActionItemsSearch.Visible = False
          JobsSearch.Visible = False
          DocumentSearch.Visible = False
          OpportunitiesSearch.Visible = False
          TransactionSearch.Visible = False
          DayPilotCalendar1.Visible = False
          btnDayPilotCalendar_Previous.Visible = False
          btnDayPilotCalendar_Next.Visible = False
          market_search.Visible = False
        Case 11
          WantedSearch.Visible = False
          NameOfListingType = "Opportunities"
          companySearch.Visible = False
          aircraftSearch.Visible = False
          OpportunitiesSearch.Visible = True
          ContactSearch.Visible = False
          NotesSearch.Visible = False
          ActionItemsSearch.Visible = False
          JobsSearch.Visible = False
          DocumentSearch.Visible = False
          TransactionSearch.Visible = False
          DayPilotCalendar1.Visible = False
          btnDayPilotCalendar_Previous.Visible = False
          btnDayPilotCalendar_Next.Visible = False
          market_search.Visible = False
        Case 12
          NameOfListingType = "Wanteds"
          companySearch.Visible = False
          aircraftSearch.Visible = False
          OpportunitiesSearch.Visible = False
          ContactSearch.Visible = False
          NotesSearch.Visible = False
          ActionItemsSearch.Visible = False
          JobsSearch.Visible = False
          DocumentSearch.Visible = False
          TransactionSearch.Visible = False
          DayPilotCalendar1.Visible = False
          btnDayPilotCalendar_Previous.Visible = False
          btnDayPilotCalendar_Next.Visible = False
          market_search.Visible = False
          WantedSearch.Visible = True
        Case 1
          WantedSearch.Visible = False
          NameOfListingType = "Company"
          OpportunitiesSearch.Visible = False
          companySearch.Visible = True
          aircraftSearch.Visible = False
          ContactSearch.Visible = False
          NotesSearch.Visible = False
          ActionItemsSearch.Visible = False
          JobsSearch.Visible = False
          DocumentSearch.Visible = False
          TransactionSearch.Visible = False
          DayPilotCalendar1.Visible = False
          btnDayPilotCalendar_Previous.Visible = False
          btnDayPilotCalendar_Next.Visible = False
          market_search.Visible = False
        Case 2
          WantedSearch.Visible = False
          NameOfListingType = "Contact"
          companySearch.Visible = False
          OpportunitiesSearch.Visible = False
          aircraftSearch.Visible = False
          ContactSearch.Visible = True
          NotesSearch.Visible = False
          ActionItemsSearch.Visible = False
          JobsSearch.Visible = False
          DocumentSearch.Visible = False
          TransactionSearch.Visible = False
          DayPilotCalendar1.Visible = False
          btnDayPilotCalendar_Previous.Visible = False
          btnDayPilotCalendar_Next.Visible = False
          market_search.Visible = False
        Case 3
          WantedSearch.Visible = False
          NameOfListingType = "Aircraft"
          'If InStr(UCase(Request.ServerVariables("SCRIPT_NAME").ToString()), "SANDBOX.ASPX") > 0 Then
          '    Nex_top.OnClientClick = "next_prev();"
          '    Nex_bottom.OnClientClick = "next_prev();"
          '    Pre_top.OnClientClick = "next_prev();"
          '    Pre_bottom.OnClientClick = "next_prev();"
          'End If
          OpportunitiesSearch.Visible = False
          aircraftSearch.Visible = True
          companySearch.Visible = False
          ContactSearch.Visible = False
          NotesSearch.Visible = False
          TransactionSearch.Visible = False
          ActionItemsSearch.Visible = False
          JobsSearch.Visible = False
          DocumentSearch.Visible = False
          DayPilotCalendar1.Visible = False
          btnDayPilotCalendar_Previous.Visible = False
          btnDayPilotCalendar_Next.Visible = False
          market_search.Visible = False
        Case 4
          WantedSearch.Visible = False
          NameOfListingType = "Action Items"
          aircraftSearch.Visible = False
          OpportunitiesSearch.Visible = False
          companySearch.Visible = False
          ContactSearch.Visible = False
          NotesSearch.Visible = False
          TransactionSearch.Visible = False
          ActionItemsSearch.Visible = True
          JobsSearch.Visible = False
          DocumentSearch.Visible = False
          market_search.Visible = False
        Case 5
          WantedSearch.Visible = False
          NameOfListingType = "Jobs"
          JobsSearch.Visible = False
          aircraftSearch.Visible = False
          companySearch.Visible = False
          OpportunitiesSearch.Visible = False
          ContactSearch.Visible = False
          NotesSearch.Visible = False
          ActionItemsSearch.Visible = False
          JobsSearch.Visible = True
          TransactionSearch.Visible = False
          DocumentSearch.Visible = False
          DayPilotCalendar1.Visible = False
          btnDayPilotCalendar_Previous.Visible = False
          btnDayPilotCalendar_Next.Visible = False
          market_search.Visible = False
        Case 6, 16
          WantedSearch.Visible = False
          If TypeOfListing = 16 Then
            NameOfListingType = "Aircraft Prospects"
          Else
            NameOfListingType = "Notes"
          End If

          JobsSearch.Visible = False
          OpportunitiesSearch.Visible = False
          TransactionSearch.Visible = False
          aircraftSearch.Visible = False
          companySearch.Visible = False
          ContactSearch.Visible = False
          JobsSearch.Visible = False
          NotesSearch.Visible = True
          ActionItemsSearch.Visible = False
          DayPilotCalendar1.Visible = False
          DocumentSearch.Visible = False
          btnDayPilotCalendar_Previous.Visible = False
          btnDayPilotCalendar_Next.Visible = False
          market_search.Visible = False
        Case 7
          WantedSearch.Visible = False
          NameOfListingType = "Documents"
          JobsSearch.Visible = False
          aircraftSearch.Visible = False
          companySearch.Visible = False
          OpportunitiesSearch.Visible = False
          ContactSearch.Visible = False
          JobsSearch.Visible = False
          TransactionSearch.Visible = False
          NotesSearch.Visible = False
          ActionItemsSearch.Visible = False
          DayPilotCalendar1.Visible = False
          DocumentSearch.Visible = True
          btnDayPilotCalendar_Previous.Visible = False
          btnDayPilotCalendar_Next.Visible = False
          market_search.Visible = False
        Case 8
          WantedSearch.Visible = False
          NameOfListingType = "Transactions"
          TransactionSearch.Visible = True
          aircraftSearch.Visible = False
          OpportunitiesSearch.Visible = False
          companySearch.Visible = False
          ContactSearch.Visible = False
          NotesSearch.Visible = False
          ActionItemsSearch.Visible = False
          JobsSearch.Visible = False
          DocumentSearch.Visible = False
          DayPilotCalendar1.Visible = False
          btnDayPilotCalendar_Previous.Visible = False
          btnDayPilotCalendar_Next.Visible = False
          market_search.Visible = False
        Case 10
          WantedSearch.Visible = False
          NameOfListingType = "Market Activity"
          market_search.Visible = True
          TransactionSearch.Visible = False
          aircraftSearch.Visible = False
          companySearch.Visible = False
          ContactSearch.Visible = False
          NotesSearch.Visible = False
          OpportunitiesSearch.Visible = False
          ActionItemsSearch.Visible = False
          JobsSearch.Visible = False
          DocumentSearch.Visible = False
          DayPilotCalendar1.Visible = False
          btnDayPilotCalendar_Previous.Visible = False
          btnDayPilotCalendar_Next.Visible = False
      End Select

      'This is going to turn OFF the search for the details page :) 

      If ListingID <> 0 And ShowSearch <> True Then
        companySearch.Visible = False
        aircraftSearch.Visible = False
        ContactSearch.Visible = False
        NotesSearch.Visible = False
        ActionItemsSearch.Visible = False
        JobsSearch.Visible = False
        DocumentSearch.Visible = False
        TransactionSearch.Visible = False
        DayPilotCalendar1.Visible = False
        btnDayPilotCalendar_Previous.Visible = False
        btnDayPilotCalendar_Next.Visible = False
        market_search.Visible = False
      End If
      'Update the Title Text on listing page
      If NameOfSubnode = "" Then
        bar_main_text.Text = NameOfListingType
      ElseIf NameOfListingType <> NameOfSubnode Then
        bar_main_text.Text = NameOfListingType & " > " & NameOfSubnode
      Else
        bar_main_text.Text = NameOfListingType
      End If
    Catch ex As Exception
      error_string = "main_site.Master.vb - search_display() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Public Sub CheckVisibilityForJetnetClient()
    Try
      Dim show_jetnet_client As CheckBox = SubNav1.FindControl("show_jetnet_client")
      If show_jetnet_client.Checked = True Then
        ShowJetnetClient = True
      Else
        ShowJetnetClient = False
      End If
    Catch ex As Exception
      error_string = "main_site.Master.vb - CheckVisibilityForJetnetClient() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
#End Region
#Region "Private Functions"
  Public Sub Redirect_Based_On_Type()
    Try
      Select Case TypeOfListing
        Case 1
          Response.Redirect("listing.aspx", False)
          HttpContext.Current.ApplicationInstance.CompleteRequest()
          m_bIsTerminating = True
        Case 2
          Response.Redirect("listing_contact.aspx", False)
          HttpContext.Current.ApplicationInstance.CompleteRequest()
          m_bIsTerminating = True
        Case 3
          Response.Redirect("listing_air.aspx", False)
          HttpContext.Current.ApplicationInstance.CompleteRequest()
          m_bIsTerminating = True
        Case 4
          Response.Redirect("listing_action.aspx", False)
          HttpContext.Current.ApplicationInstance.CompleteRequest()
          m_bIsTerminating = True
        Case 5
          Response.Redirect("listing_jobs.aspx", False)
          HttpContext.Current.ApplicationInstance.CompleteRequest()
          m_bIsTerminating = True
        Case 6, 16
          Response.Redirect("listing_notes.aspx", False)
          HttpContext.Current.ApplicationInstance.CompleteRequest()
          m_bIsTerminating = True
        Case 7
          Response.Redirect("listing_document.aspx", False)
          HttpContext.Current.ApplicationInstance.CompleteRequest()
          m_bIsTerminating = True
        Case 8
          Response.Redirect("listing_transaction.aspx", False)
          HttpContext.Current.ApplicationInstance.CompleteRequest()
          m_bIsTerminating = True
          'Response.Redirect("listing_contact.aspx")
        Case 9
          Response.Redirect("home.aspx", False)
          HttpContext.Current.ApplicationInstance.CompleteRequest()
          m_bIsTerminating = True
        Case 10
          Response.Redirect("market.aspx", False)
          HttpContext.Current.ApplicationInstance.CompleteRequest()
          m_bIsTerminating = True
        Case 11
          Response.Redirect("listing_opportunities.aspx", False)
          HttpContext.Current.ApplicationInstance.CompleteRequest()
          m_bIsTerminating = True
        Case 12
          Response.Redirect("listing_wanted.aspx", False)
          HttpContext.Current.ApplicationInstance.CompleteRequest()
          m_bIsTerminating = True
        Case 13
          Response.Redirect("View.aspx", False)
          'javascript:load('view_template.aspx?ViewID=1&noMaster=false','','scrollbars=yes,menubar=no,height=700,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;
          HttpContext.Current.ApplicationInstance.CompleteRequest()
          m_bIsTerminating = True
        Case 14
          Response.Redirect("performance_specs.aspx", False)
          HttpContext.Current.ApplicationInstance.CompleteRequest()
          m_bIsTerminating = True
        Case 15
          Response.Redirect("op_costs.aspx", False)
          HttpContext.Current.ApplicationInstance.CompleteRequest()
          m_bIsTerminating = True
      End Select
    Catch ex As Exception
      error_string = "main_site.Master.vb - Redirect_Based_On_Type() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
#End Region
#Region "Private Subs"
  Protected Sub btnDayPilotCalendar_Calendar_Next(ByVal sender As Object, ByVal e As EventArgs) Handles btnDayPilotCalendar_Next.Click
    Try
      If DayPilotCalendar1.Days = 7 Then
        DayPilotCalendar1.StartDate = DayPilotCalendar1.StartDate.AddDays(7)
        DateOfActionItem = DayPilotCalendar1.StartDate
        TypeOfListing = 4
        NameOfSubnode = "Action Items"
        SubNodeOfListing = 4
        Fill_DayPilotCalendar1("Week")
      ElseIf DayPilotCalendar1.Days = 1 Then
        DayPilotCalendar1.StartDate = DayPilotCalendar1.StartDate.AddDays(1)
        DateOfActionItem = DayPilotCalendar1.StartDate
        TypeOfListing = 4
        NameOfSubnode = "Action Items"
        SubNodeOfListing = 4
        Fill_DayPilotCalendar1("Day")
      Else
        DayPilotCalendar1.StartDate = DayPilotCalendar1.StartDate.AddMonths(1)
        DateOfActionItem = DayPilotCalendar1.StartDate
        TypeOfListing = 4
        NameOfSubnode = "Action Items"
        SubNodeOfListing = 4
        Fill_DayPilotCalendar1("Month")
      End If
    Catch ex As Exception
      error_string = "main_site.Master.vb - btnDayPilotCalendar_Calendar_Next() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Protected Sub btnDayPilotCalendar_Calenar_Previous_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDayPilotCalendar_Previous.Click
    Try
      If DayPilotCalendar1.Days = 7 Then
        DayPilotCalendar1.StartDate = DayPilotCalendar1.StartDate.AddDays(-7)
        DateOfActionItem = DayPilotCalendar1.StartDate
        TypeOfListing = 4
        Fill_DayPilotCalendar1("Week")
      Else
        DayPilotCalendar1.StartDate = DayPilotCalendar1.StartDate.AddDays(-1)
        DateOfActionItem = DayPilotCalendar1.StartDate
        TypeOfListing = 4
        Fill_DayPilotCalendar1("Day")
      End If
    Catch ex As Exception
      error_string = "main_site.Master.vb - btnDayPilotCalendar_Calenar_Previous_Click() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Sub PagerButtonClick(ByVal sender As Object, ByVal e As EventArgs) Handles Pre_top.Click
    'This deals with the paging. 
    Try
      Dim currentrecord As Integer = 0
      Dim arg As String = sender.CommandArgument

      Select Case arg
        Case "Next"
          RaiseEvent NextButton_Listing()
        Case "Prev"
          RaiseEvent PreviousButton_Listing()
        Case Else
          'Results.CurrentPageIndex = Convert.ToInt32(arg)
      End Select
    Catch ex As Exception
      error_string = "main_site.Master.vb - PagerButtonClick() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Sub Set_Pager_Buttons()
    'Determines whether the buttons show up or not. 
    RaiseEvent SetPagerButtons()
  End Sub
#End Region
#Region "Search Functions "
  Public Sub Fill_Jobs(ByVal type As Integer)
    RaiseEvent resultsVisible()
    Try
      Dim aError As String = ""
      Select Case type
        Case 1
          'Pending
          aTempTable = aclsData_Temp.GetClient_JobSeeker_status("P", aError)
          If aError <> "" Then
            LogError("main_site.Master.vb - Fill_Jobs() GetClient_JobSeeker_status - " & aError)
          End If
        Case 2
          'Pilots
          aTempTable = aclsData_Temp.GetClient_JobSeeker_Type("P", aError)
          If aError <> "" Then
            LogError("main_site.Master.vb - Fill_Jobs() GetClient_JobSeeker_Type - " & aError)
          End If
        Case 3
          'Mechanics
          aTempTable = aclsData_Temp.GetClient_JobSeeker_Type("M", aError)
          If aError <> "" Then
            LogError("main_site.Master.vb - Fill_Jobs() GetClient_JobSeeker_Type - " & aError)
          End If
        Case Else
          aTempTable = aclsData_Temp.GetClient_JobSeeker
      End Select
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          Table_List = aTempTable
          RaiseEvent BringResults()
          error_results.Text = ""
        Else
          Table_List = New DataTable
          error_results.Text = "<p align='center'>0 Records have been found. Please try again.</p>"
          RaiseEvent BringResults()
        End If
      Else
        'Nothing was Returned
        error_results.Text = "<p align='center'>0 Records have been found. Please try again.</p>"
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("main_site.Master.vb - Fill_Jobs() - " & error_string)
        End If
        display_error()
        aclsData_Temp.class_error = ""
      End If
    Catch ex As Exception
      error_string = "main_site.Master.vb - Fill_Jobs() - " & ex.Message
      LogError(error_string)
    End Try

  End Sub
  Public Sub Fill_Documents(ByVal search_for As String, ByVal search_where As String, ByVal model_cbo As String, ByVal display_cbo As String, ByVal order_by As String, ByVal category As Integer, ByVal start_date As String, ByVal end_date As String)
    RaiseEvent resultsVisible()
    Try
      If start_date <> "" Then
        start_date = Year(start_date) & "-" & Month(start_date) & "-" & Day(start_date)
      End If
      If end_date <> "" Then
        Dim _end_date As Date = CDate(end_date)
        _end_date = DateAdd(DateInterval.Day, 1, _end_date)
        end_date = Year(_end_date) & "-" & Month(_end_date) & "-" & Day(_end_date)
      Else
        end_date = Year(DateAdd(DateInterval.Day, 1, Now())) & "-" & Month(DateAdd(DateInterval.Day, 1, Now())) & "-" & Day(DateAdd(DateInterval.Day, 1, Now()))
      End If

      Dim jetnet_model_id As String = ""
      Dim client_model_id As String = ""
      If model_cbo <> "" Then
        model_cbo = Replace(model_cbo, "'", "")
        Dim model_sets As Array = Split(model_cbo, ",")
        For x = 0 To UBound(model_sets)
          Dim model_info As Array = Split(model_sets(x), "|")
          If x = 0 Then
            jetnet_model_id = "'"
            client_model_id = "'"
          End If
          jetnet_model_id = jetnet_model_id & model_info(0)
          client_model_id = client_model_id & model_info(4)
          If x <> UBound(model_sets) Then
            jetnet_model_id = jetnet_model_id & "','"
            client_model_id = client_model_id & "','"
          Else
            jetnet_model_id = jetnet_model_id & "'"
            client_model_id = client_model_id & "'"
          End If
        Next
      End If

      Dim user As Integer = 999
      If display_cbo <> "" Then
        user = CInt(display_cbo)
      End If
      If user = 999 Then
        user = CInt(Session.Item("localUser").crmLocalUserID)
      End If
      If search_for = "" Then 'No search parameters
        aTempTable = aclsData_Temp.GetLocal_Notes_Extender_Opportunities("F", "", jetnet_model_id, client_model_id, category, start_date, end_date, display_cbo)
      Else
        Dim search As String = "%" & search_for & "%" 'Default to this type of search
        If search_where = 1 Then 'This means that parentheses is on both sides, search feature. 
          search = "%" & search_for & "%"
        ElseIf search_where = 2 Then
          search = search_for & "%"
        End If
        If model_cbo <> "" Then
          Dim model As Array = Split(Trim(model_cbo), "|")
          If model(3) = "CLIENT" Then
            aTempTable = aclsData_Temp.GetLocal_Notes_Extender_Opportunities("F", search, jetnet_model_id, client_model_id, category, start_date, end_date, display_cbo)
          Else
            aTempTable = aclsData_Temp.GetLocal_Notes_Extender_Opportunities("F", search, jetnet_model_id, client_model_id, category, start_date, end_date, display_cbo)
          End If
        Else
          aTempTable = aclsData_Temp.GetLocal_Notes_Extender_Opportunities("F", search, jetnet_model_id, client_model_id, category, start_date, end_date, display_cbo)
        End If
      End If
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          Table_List = aTempTable
          RaiseEvent BringResults()
          error_results.Text = ""
        Else
          Table_List = New DataTable
          error_results.Text = "<p align='center'>0 Records have been found. Please try again.</p>"
          RaiseEvent BringResults()

        End If
      Else
        'Nothing was Returned
        error_results.Text = "<p align='center'>0 Records have been found. Please try again.</p>"
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("main_site.Master.vb - Fill_Documents() - " & error_string)
        End If
        display_error()
        aclsData_Temp.class_error = ""
      End If
    Catch ex As Exception
      error_string = "main_site.Master.vb - Fill_Documents() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Public Sub Fill_Action(ByVal search_date As String, ByVal search_for As String, ByVal search_where As String, ByVal document_status As String, ByVal display_cbo As String, ByVal order_by As String, ByVal reg_start As String, ByVal reg_end As String)
    RaiseEvent resultsVisible()
    btnDayPilotCalendar_Previous.Visible = False
    btnDayPilotCalendar_Next.Visible = False
    DayPilotCalendar1.Visible = False
    Try

      If reg_end <> "" Then
        If IsDate(reg_end) Then
          reg_end = Month(reg_end) & "/" & Day(reg_end) & "/" & Year(reg_end)
        End If
      End If
      If reg_start <> "" Then
        If IsDate(reg_start) Then
          reg_start = Month(reg_start) & "/" & Day(reg_start) & "/" & Year(reg_start)
        End If
      End If
      If search_date <> "" Then
        If IsDate(search_date) Then
          search_date = Month(search_date) & "/" & Day(search_date) & "/" & Year(search_date)
        End If
      End If
      Dim filter_me As Boolean = False
      Dim user As Integer = 999
      If display_cbo <> "" Then
        user = CInt(display_cbo)
      End If
      If user = 999 Then
        user = CInt(Session.Item("localUser").crmLocalUserID)
      End If
      If document_status = "" Then
        document_status = "B"
      End If
      filter_me = False

      If reg_start <> "" Then
        filter_me = True
      End If

      If search_for = "" Then 'No search parameters

        If search_date <> "" Then
          aTempTable = aclsData_Temp.Get_Local_Notes_Action_Date(CStr(search_date), CStr(DateAdd(DateInterval.Hour, 23, DateOfActionItem)))
        Else
          aTempTable = aclsData_Temp.Get_Local_Notes_Status("P", user, "B")
        End If
      Else
        Dim search As String = "%" & search_for & "%" 'Default to this type of search
        If search_where = 1 Then 'This means that parentheses is on both sides, search feature. 
          search = "%" & search_for & "%"
        ElseIf search_where = 2 Then
          search = search_for & "%"
        End If
        aTempTable = aclsData_Temp.Get_Local_Notes_Action_Item(search, "A", order_by, user)
      End If

      If filter_me = True Then

        'Filtering the table based on dates.
        aTempTable2 = aTempTable.Clone
        ' create a datarow to filter in the rows by make_name
        Dim afileterd As DataRow()

        If reg_end <> "" Then
          afileterd = aTempTable.Select("lnote_schedule_start_date >= '" & reg_start & "' and lnote_schedule_start_date <= '" & reg_end & "'", "lnote_schedule_start_date asc")
        Else
          afileterd = aTempTable.Select("lnote_schedule_start_date >= '" & reg_start & "' ", "lnote_schedule_start_date asc")
        End If
        ' create another datarow to import the filtered info
        For Each atmpDataRow As DataRow In afileterd
          aTempTable2.ImportRow(atmpDataRow)
        Next

        aTempTable = aTempTable2
      End If


      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          Table_List = aTempTable
          RaiseEvent BringResults()
          error_results.Text = ""
        Else
          Table_List = New DataTable
          error_results.Text = "<p align='center'>0 Records have been found. Please try again.</p>"
          RaiseEvent BringResults()
        End If
      Else
        'Nothing was Returned
        error_results.Text = "<p align='center'>0 Records have been found. Please try again.</p>"
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("main_site.Master.vb - Fill_Action() - " & error_string)
        End If
        display_error()
        aclsData_Temp.class_error = ""
      End If
    Catch ex As Exception
      error_string = "main_site.Master.vb - Fill_Action() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Public Sub Fill_Notes(ByVal search_for As String, ByVal search_where As String, ByVal ActiveStatus As String, ByVal display_cbo As String, ByVal order_by As String, ByVal reg_start As String, ByVal reg_end As String, ByVal clientIds As String, ByVal jetnetIds As String, ByVal acSearchField As Integer, ByVal acSearchOperator As Integer, ByVal acSearchText As String, ByVal NoteCategory As String, ByVal OnlyModel As Boolean, ByVal OnlyAircraft As Boolean, ByVal FolderType As Long)
    RaiseEvent resultsVisible()
    Try
      Dim user As Integer = 999
      If display_cbo <> "" Then
        user = CInt(display_cbo)
      End If
      If user = 999 Then
        user = CInt(Session.Item("localUser").crmLocalUserID)
      End If


      'Dim filter_me As Boolean = False
      Dim notes_model As New ListBox

      If Session.Item("localUser").crmEvo = True Then 'If an EVO user
        notes_model = NotesSearch.FindControl("model")
      Else
        If NotesSearch.FindControl("model_cbo").Visible = True Then
          notes_model = NotesSearch.FindControl("model_cbo")
        Else
          notes_model = NotesSearch.FindControl("model")
        End If
      End If


      aTempTable = clsGeneral.clsGeneral.Fill_Notes_Actions_Documents(reg_start, reg_end, search_for, search_where, notes_model, NoteCategory, user, Nothing, Me, ActiveStatus, clientIds, jetnetIds, acSearchField, acSearchOperator, acSearchText, OnlyModel, OnlyAircraft, FolderType)

      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then

          If TypeOfListing = 16 Then
            aTempTable = aclsData_Temp.Notes_FillupSearch(aTempTable, order_by)
          ElseIf TypeOfListing = 6 Then

            'Company Tables.
            aTempTable.Columns.Add("comp_name")
            aTempTable.Columns.Add("comp_address1")
            aTempTable.Columns.Add("comp_address2")
            aTempTable.Columns.Add("comp_city")
            aTempTable.Columns.Add("comp_state")
            aTempTable.Columns.Add("comp_country")
            aTempTable.Columns.Add("comp_zip_code")
            aTempTable.Columns.Add("comp_id")
            aTempTable.Columns.Add("comp_description")
            aTempTable.Columns.Add("comp_email_address")
            aTempTable.Columns.Add("comp_source")
            aTempTable.Columns.Add("comp_phone_office")
            aTempTable.Columns.Add("comp_phone_fax")

            'Aircraft
            aTempTable.Columns.Add("amod_model_name")
            aTempTable.Columns.Add("amod_make_name")
            aTempTable.Columns.Add("ac_ser_nbr")
            aTempTable.Columns.Add("ac_reg_nbr")
            aTempTable.Columns.Add("ac_year_mfr")
            aTempTable.Columns.Add("ac_id")
            aTempTable.Columns.Add("ac_source")

            aTempTable.Columns.Add("ac_date_purchased")
            aTempTable.Columns.Add("ac_forsale_flag")
            aTempTable.Columns.Add("ac_status")
            aTempTable.Columns.Add("ac_delivery")
            aTempTable.Columns.Add("ac_asking_wordage")
            aTempTable.Columns.Add("ac_asking_price")
            aTempTable.Columns.Add("ac_est_price")
            aTempTable.Columns.Add("ac_date_listed")
            aTempTable.Columns.Add("ac_exclusive_flag")
            aTempTable.Columns.Add("ac_lease_flag")



          End If
          Table_List = aTempTable
          RaiseEvent BringResults()
          error_results.Text = ""
        Else
          Table_List = New DataTable
          error_results.Text = "<p align='center'>0 Records have been found. Please try again.</p>"
          RaiseEvent BringResults()
        End If
      Else
        'Nothing was Returned
        error_results.Text = "<p align='center'>0 Records have been found. Please try again.</p>"
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("main_site.Master.vb - Fill_Notes() - " & error_string)
        End If
        display_error()
        aclsData_Temp.class_error = ""
      End If
    Catch ex As Exception
      error_string = "main_site.Master.vb - Fill_Notes() - " & ex.Message
      LogError(error_string)
    End Try

  End Sub

  Public Sub Fill_Opportunities(ByVal search_for As String, ByVal search_where As String, ByVal display_cbo As String, ByVal category As Integer, ByVal start_date As String, ByVal end_date As String, ByVal status As String)
    RaiseEvent resultsVisible()
    Try
      Dim user As Integer = 999
      If display_cbo <> "" Then
        user = CInt(display_cbo)
      End If
      If user = 999 Then
        user = CInt(Session.Item("localUser").crmLocalUserID)
      End If

      aTempTable = clsGeneral.clsGeneral.Fill_Notes_Actions_Documents(start_date, end_date, search_for, search_where, New ListBox, category, user, Nothing, Me, status, "", "", 0, 0, "", False, False, 3)

      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          Table_List = aTempTable
          RaiseEvent BringResults()
          error_results.Text = ""
        Else
          Table_List = New DataTable
          error_results.Text = "<p align='center'>0 Records have been found. Please try again.</p>"
          RaiseEvent BringResults()
        End If
      Else
        'Nothing was Returned
        error_results.Text = "<p align='center'>0 Records have been found. Please try again.</p>"
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("main_site.Master.vb - Fill_Notes() - " & error_string)
        End If
        display_error()
        aclsData_Temp.class_error = ""
      End If
    Catch ex As Exception
      error_string = "main_site.Master.vb - Fill_Notes() - " & ex.Message
      LogError(error_string)
    End Try

  End Sub
  ''' <summary>
  ''' Function for Contact Search Event
  ''' </summary>
  ''' <param name="subnode">subnode true/false?</param>
  ''' <param name="SQL_Contact_Name">Contact First Name</param>
  ''' <param name="SQL_Last_Name">Contact Last Name</param>
  ''' <param name="search_how">Begins with/Anywhere</param>
  ''' <param name="comp_name_text">Company Name Text Box</param>
  ''' <param name="status">Active/Inactive</param>
  ''' <param name="sort">Sort By</param> 
  ''' <param name="subset">Data Subset?</param>
  ''' <remarks></remarks>
  Public Sub Fill_Contact(ByVal subnode As Boolean, ByVal SQL_Contact_Name As String, ByVal SQL_Last_Name As String, ByVal search_how As String, ByVal comp_name_text As String, ByVal status As String, ByVal sort As String, ByVal subset As String, ByVal email_address As String, ByVal phone As String)
    'x is the search string.
    RaiseEvent resultsVisible()
    'Subnode is whether or not we're going to display the information for a subfolder. Example:
    'Under Company - there is a folder called Hot Leads. If subnode is true - hot leads information will display.
    'If it's false, the regular search will take place. 
    Dim arComp_ids_JETNET As String = ""
    Dim arComp_ids_CLIENT As String = ""
    Try

      If subnode = False Then
        aTempTable = clsGeneral.clsGeneral.Fill_Contact(Nothing, Me, SQL_Contact_Name, SQL_Last_Name, comp_name_text, status, search_how, sort, subset, 0, email_address, phone)
      Else
        aTempTable = clsGeneral.clsGeneral.Fill_Contact(Nothing, Me, SQL_Contact_Name, SQL_Last_Name, comp_name_text, status, search_how, sort, subset, SubNodeOfListing, email_address, phone)
      End If


      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          Table_List = aTempTable
          RaiseEvent BringResults()
          error_results.Text = ""
        Else
          Table_List = New DataTable
          error_results.Text = "<p align='center'>0 Records have been found. Please try again.</p>"
          RaiseEvent BringResults()
        End If
      Else
        'Nothing was Returned
        error_results.Text = "<p align='center'>0 Records have been found. Please try again.</p>"
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("main_site.Master.vb - Fill_Contact() - " & error_string)
        End If
        display_error()
        aclsData_Temp.class_error = ""
      End If
    Catch ex As Exception
      error_string = "main_site.Master.vb - Fill_Contact() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Public Sub Fill_Company(ByVal subnode As Boolean, ByVal search_for As String, ByVal search_where As String, ByVal search_for_cbo As String, ByVal status_cbo As String, ByVal subset As String, ByVal country As String, ByVal states As String, ByVal operator_type As String, ByVal show_all As String, ByVal special_field As String, ByVal special_field_text As String, ByVal special_field_view As Boolean, ByVal special_field_column As String, ByVal client_IDS As String, ByVal jetnet_IDS As String, ByVal companyCity As String, ByVal mergeLists As Boolean)
    RaiseEvent resultsVisible()
    Try
      'x is the search string.
      'Subnode is whether or not we're going to display the information for a subfolder. Example:
      'Under Company - there is a folder called Hot Leads. If subnode is true - hot leads information will display.
      'If it's false, the regular search will take place. 
      'Y is a determining factor. More often than not it will be 1. That means
      'When things are searched - they'll be a % sign on either side. Example: where comp_name like "%test%"
      'When y is 2 - they'll only be a % sign at the end. Example: where comp_name like "A%". This is for company
      'Letter/number buttons.

      Dim state As ListBox = companySearch.FindControl("state")
      aTempTable = clsGeneral.clsGeneral.Fill_Company(subnode, "" & search_for & "", search_where, search_for, status_cbo, subset, country, operator_type, show_all, special_field, special_field_text, special_field_view, special_field_column, Nothing, Me, SubNodeOfListing, state, client_IDS, jetnet_IDS, companyCity, mergeLists)

      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          'GridView1.DataSource = aTempTable
          'GridView1.DataBind()
          Table_List = aTempTable
          If special_field_view = True Then
            RaiseEvent Swap_Columns()
          End If
          RaiseEvent BringResults()
          error_results.Text = ""
        Else
          Table_List = New DataTable
          error_results.Text = "<p align='center'>0 Records have been found. Please try again.</p>"
          RaiseEvent BringResults()
        End If
      Else
        'Nothing was Returned
        error_results.Text = "<p align='center'>0 Records have been found. Please try again.</p>"
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("main_site.Master.vb - Fill_Company() - " & error_string)
        End If
        display_error()
        aclsData_Temp.class_error = ""
      End If

    Catch ex As Exception
      error_string = "main_site.Master.vb - Fill_Company() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Public Sub Fill_Aircraft(ByVal subnode As Boolean, ByVal search_for As String, ByVal begins_with As String, ByVal search_in As String, ByVal model As String, ByVal market_status As String, ByVal sort As String, ByVal sort_how As String, ByVal jetnet_ids As String, ByVal client_ids As String, ByVal subset As String, ByVal airport_name As String, ByVal icao_code As String, ByVal iata_code As String, ByVal city As String, ByVal country As String, ByVal state As String, ByVal types_of_owners As String, ByVal on_exclusive As String, ByVal on_lease As String, ByVal year_start As String, ByVal year_end As String, ByVal search_field As String, ByVal lifecycle As String, ByVal ownership As String, ByVal CustomField1 As String, ByVal CustomField2 As String, ByVal CustomField3 As String, ByVal CustomField4 As String, ByVal CustomField5 As String, ByVal CustomField6 As String, ByVal CustomField7 As String, ByVal CustomField8 As String, ByVal CustomField9 As String, ByVal CustomField10 As String, ByVal AircraftNotesSearch As Integer, ByVal AircraftNoteDate As String, ByVal MergeLists As Boolean)
    RaiseEvent resultsVisible()

    Try
      Session.Item("types_of_owners") = types_of_owners 'Set for the owners display

      Dim model_list As New ListBox

      If Session.Item("localUser").crmEvo = True Then 'If an EVO user
        model_list = aircraftSearch.FindControl("model")
      Else
        If aircraftSearch.FindControl("model_cbo").Visible = True Then
          model_list = aircraftSearch.FindControl("model_cbo")
        Else
          model_list = aircraftSearch.FindControl("model")
        End If
      End If

      aTempTable = clsGeneral.clsGeneral.Fill_Aircraft(Nothing, Me, sort, subset, types_of_owners, search_for, market_status, airport_name, icao_code, iata_code, city, country, on_exclusive, on_lease, year_start, year_end, begins_with, model_list, subnode, state, search_field, lifecycle, ownership, CustomField1, CustomField2, CustomField3, CustomField4, CustomField5, CustomField6, CustomField7, CustomField8, CustomField9, CustomField10, AircraftNotesSearch, AircraftNoteDate, MergeLists)
      jump_to.Items.Clear()
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          Table_List = aTempTable
          RaiseEvent BringResults()
          error_results.Text = ""
          sortByDynamic.Visible = True
        Else
          Table_List = New DataTable
          error_results.Text = "<p align='center' class='red'>0 Records have been found. Please try again.</p>"
          RaiseEvent BringResults()
        End If
      Else
        'Nothing was Returned
        error_results.Text = "<p align='center'>0 Records have been found. Please try again.</p>"
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("main_site.Master.vb - Fill_Aircraft() - " & error_string)
        End If
        display_error()
        aclsData_Temp.class_error = ""
      End If
    Catch ex As Exception
      error_string = "main_site.Master.vb - Fill_Aircraft() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Public Sub Fill_Market(ByVal model_cbo As ListBox, ByVal start_date As Integer, ByVal categories As ListBox, ByVal types As ListBox, ByVal start As String, ByVal end_date As String)
    Try
      RaiseEvent resultsVisible()
      'Dim start_date_field As String = ""
      'If start_date <> 0 Then
      '    Select Case start_date
      '        Case 7 'Week
      '            start_date_field = DateAdd(DateInterval.Day, -7, Now())
      '        Case 31 'Month
      '            start_date_field = DateAdd(DateInterval.Month, -1, Now())
      '        Case 93 'Three Month
      '            start_date_field = DateAdd(DateInterval.Month, -3, Now())
      '        Case 186 'Six Month
      '            start_date_field = DateAdd(DateInterval.Month, -6, Now())
      '        Case 279 'Nine Months
      '            start_date_field = DateAdd(DateInterval.Month, -9, Now())
      '        Case 365 'Twelve Months
      '            start_date_field = DateAdd(DateInterval.Month, -12, Now())
      '    End Select

      '    start_date_field = Year(start_date_field) & "-" & Month(start_date_field) & "-" & Day(start_date_field)
      'End If

      If (start_date = 0) And (model_cbo.SelectedValue = "") And (start = "") And (end_date = "") Then
        error_results.Text = "Please choose either the model or enter a date range"
      Else
        'Dim jetnet_model_id_hold As String = ""
        Try
          error_results.Text = ""

          'Dim jetnet_model_id As Integer = 0
          'If model_cbo <> "" Then
          '    Dim arrayed As Array = Split(model_cbo, ",")

          '    For count = 0 To UBound(arrayed)
          '        Dim model_info As Array = Split(arrayed(count), "|")
          '        jetnet_model_id_hold = jetnet_model_id_hold & "'" & model_info(0) & "',"
          '    Next

          '    If jetnet_model_id_hold <> "" Then
          '        jetnet_model_id_hold = UCase(jetnet_model_id_hold.TrimEnd(","))
          '    End If

          'Else
          '    jetnet_model_id_hold = ""
          'End If

          'aTempTable = New DataTable

          aTempTable = clsGeneral.clsGeneral.Fill_Market(Nothing, Me, model_cbo, categories, types, start_date, start, end_date)



          If Not IsNothing(aTempTable) Then
            If aTempTable.Rows.Count > 0 Then
              Table_List = aTempTable
              RaiseEvent BringResults()
              error_results.Text = ""
            Else
              Table_List = New DataTable
              error_results.Text = "<p align='center'>0 Records have been found. Please try again.</p>"
              RaiseEvent BringResults()
            End If
          Else
            'Nothing was Returned
            error_results.Text = "<p align='center'>0 Records have been found. Please try again.</p>"
            If aclsData_Temp.class_error <> "" Then
              error_string = aclsData_Temp.class_error
              LogError("main_site.Master.vb - Fill_Market() - " & error_string)
            End If
            display_error()
            aclsData_Temp.class_error = ""
          End If
        Catch ex As Exception
          error_string = "main_site.Master.vb - Fill_Market() - " & ex.Message
          LogError(error_string)
        End Try

      End If
    Catch ex As Exception
      error_string = "main_site.Master.vb - Fill_Market() - " & ex.Message
      LogError(error_string)
    End Try

  End Sub
  Public Sub Fill_Transactions(ByVal search As String, ByVal search_where As String, ByVal search_for_cbo As String, ByVal model_cbo As String, ByVal subset As String, ByVal trans_type As String, ByVal start_date As String, ByVal end_date As String, ByVal relationships As String, ByVal year_start As String, ByVal year_end As String, ByVal internal As String, ByVal awaiting As Boolean)
    Try
      Dim transaction_model As New ListBox
      If Session.Item("localUser").crmEvo = True Then 'If an EVO user
        transaction_model = TransactionSearch.FindControl("model")
      Else
        If TransactionSearch.FindControl("model_cbo").Visible = True Then
          transaction_model = TransactionSearch.FindControl("model_cbo")
        Else
          transaction_model = TransactionSearch.FindControl("model")
        End If
      End If
      Dim errored As Boolean = False
      If start_date = "" And end_date = "" And search = "" And transaction_model.SelectedValue = "" Then
        aTempTable = New DataTable
        errored = True
      Else
        aTempTable = clsGeneral.clsGeneral.Fill_Transactions(start_date, end_date, transaction_model, search, search_where, internal, awaiting, trans_type, subset, year_start, year_end, Nothing, Me)

      End If
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          Table_List = aTempTable
          RaiseEvent BringResults()
          error_results.Text = ""
        Else
          Table_List = New DataTable
          If errored = False Then
            error_results.Text = "<p align='center'>0 Records have been found. Please try again.</p>"
          Else
            error_results.Text = "<p align='center'>Please use more detailed search parameters.</p>"
          End If

          RaiseEvent BringResults()
        End If
      Else
        'Nothing was Returned
        error_results.Text = "<p align='center'>0 Records have been found. Please try again.</p>"
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("main_site.Master.vb - Fill_Transactions() - " & error_string)
        End If
        display_error()
        aclsData_Temp.class_error = ""
      End If


    Catch ex As Exception
      error_string = "main_site.Master.vb - Fill_Transactions() - " & ex.Message
      LogError(error_string)
    End Try

  End Sub
  Public Sub Fill_Wanted(ByVal model_cbo As ListBox, ByVal start_date As String, ByVal end_date As String, ByVal interested_party As String, ByVal subset As String)
    Try

      Dim errored As Boolean = False
      If start_date = "" And end_date = "" And interested_party = "" And model_cbo.SelectedValue = "" Then
        aTempTable = New DataTable
        errored = True
      Else
        aTempTable = clsGeneral.clsGeneral.Fill_Wanteds(Nothing, Me, model_cbo, start_date, end_date, interested_party, subset)

      End If
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          Table_List = aTempTable
          RaiseEvent BringResults()
          error_results.Text = ""
        Else
          Table_List = New DataTable
          If errored = False Then
            error_results.Text = "<p align='center'>0 Records have been found. Please try again.</p>"
          Else
            error_results.Text = "<p align='center'>Please use more detailed search parameters.</p>"
          End If

          RaiseEvent BringResults()
        End If
      Else
        'Nothing was Returned
        error_results.Text = "<p align='center'>0 Records have been found. Please try again.</p>"
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("main_site.Master.vb - Fill_Wanteds() - " & error_string)
        End If
        display_error()
        aclsData_Temp.class_error = ""
      End If


    Catch ex As Exception
      error_string = "main_site.Master.vb - Fill_Wanteds() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
#End Region
#Region "Tree View Events/Functions"
  Private Sub Clicked_Tree(ByVal sender As Object, ByVal type As Integer, ByVal parent As Integer, ByVal text As String, ByVal existsSubNode As Boolean, ByVal subnodeMethod As String) Handles TreeNav.Clicked_Me
    Try
      FromTypeOfListing = parent 'added to retain listing ID that we came from on a search if the type is changed
      TypeOfListing = parent
      IsSubNode = existsSubNode
      NameOfSubnode = text
      SubNodeOfListing = type
      Subnode_Method = subnodeMethod

      Table_List = Nothing
      Session("Results") = Nothing
      Session("search_company") = Nothing
      Session("search_contact") = Nothing
      Session("search_aircraft") = Nothing
      Session("search_transaction") = Nothing
      Redirect_Based_On_Type()
    Catch ex As Exception
      error_string = "main_site.Master.vb - Clicked_Tree() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
#End Region




  Private Sub Page_PreviousButton_Listing() Handles Me.PreviousButton_Listing

  End Sub
  Public Sub SetActiveTab()
    If InStr(HttpContext.Current.Request.Url.AbsoluteUri, "home.aspx") > 0 Then
      homeLink.Attributes.Add("class", "specialHeaderTab selectedTab")
    ElseIf InStr(HttpContext.Current.Request.Url.AbsoluteUri, "listing_air.aspx") > 0 Then
      airLink.Attributes.Add("class", "specialHeaderTab selectedTab")
    ElseIf InStr(HttpContext.Current.Request.Url.AbsoluteUri, "listing.aspx") > 0 Then
      compLink.Attributes.Add("class", "specialHeaderTab selectedTab")
    ElseIf InStr(HttpContext.Current.Request.Url.AbsoluteUri, "listing_transaction.aspx") > 0 Then
      transLink.Attributes.Add("class", "specialHeaderTab selectedTab")
    ElseIf InStr(HttpContext.Current.Request.Url.AbsoluteUri, "market.aspx") > 0 Then
      marketLink.Attributes.Add("class", "specialHeaderTab selectedTab")
    End If
  End Sub
  Private Sub special_sort_by_cbo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles special_sort_by_cbo.SelectedIndexChanged, special_sort_method_cbo.SelectedIndexChanged
    Dim runSearch As Boolean = False

    If Not IsNothing(Session("Results")) Then
      aTempTable = Session("Results")
      Dim SortMethod As String = ""
      Dim Filtered_DV As New DataView(aTempTable)

      SortMethod = special_sort_by_cbo.SelectedValue + " " + special_sort_method_cbo.SelectedValue

      Filtered_DV.Sort = SortMethod
      Select Case UCase(SortMethod) 'Throwing this through a sort just for the aircraft page, all the others should be fine as is.
        Case "AC_DATE_LISTED DESC"
          Filtered_DV.Sort = "OTHER_AC_DATE_LISTED DESC"
        Case "AC_DATE_LISTED ASC"
          Filtered_DV.Sort = "OTHER_AC_DATE_LISTED ASC"
        Case "AC_AIRFRAME_TOT_HRS DESC"
          Filtered_DV.Sort = "OTHER_AC_AIRFRAME_TOT_HRS DESC"
        Case "AC_AIRFRAME_TOT_HRS ASC"
          Filtered_DV.Sort = "OTHER_AC_AIRFRAME_TOT_HRS ASC"
        Case "AC_AIRFRAME_TOT_HRS ASC"
          Filtered_DV.Sort = "OTHER_AC_AIRFRAME_TOT_HRS ASC"
        Case "AMOD_MAKE_NAME DESC, AMOD_MODEL_NAME DESC, AC_SER_NBR_SORT DESC"
          Filtered_DV.Sort = "AMOD_MAKE_NAME DESC, AMOD_MODEL_NAME DESC, AC_SER_NBR_SORT DESC"
        Case "AMOD_MAKE_NAME ASC, AMOD_MODEL_NAME ASC, AC_SER_NBR_SORT ASC"
          Filtered_DV.Sort = "AMOD_MAKE_NAME ASC, AMOD_MODEL_NAME ASC, AC_SER_NBR_SORT ASC"
        Case "AC_YEAR_MFR DESC"
          Filtered_DV.Sort = "OTHER_AC_YEAR_MFR DESC, AC_SER_NBR_SORT ASC"
        Case "AC_YEAR_MFR ASC"
          Filtered_DV.Sort = "OTHER_AC_YEAR_MFR ASC, AC_SER_NBR_SORT ASC"
        Case "AC_SER_NBR_SORT DESC"
          Filtered_DV.Sort = "AC_SER_NBR_SORT DESC"
        Case "AC_SER_NBR_SORT ASC"
          Filtered_DV.Sort = "AC_SER_NBR_SORT ASC"
        Case "AC_REG_NBR DESC"
          Filtered_DV.Sort = "OTHER_AC_REG_NBR DESC"
        Case "AC_REG_NBR ASC"
          Filtered_DV.Sort = "OTHER_AC_REG_NBR ASC"
        Case "AC_ASKING_PRICE DESC"
          Filtered_DV.Sort = "OTHER_AC_ASKING_PRICE DESC"
        Case "AC_ASKING_PRICE ASC"
          Filtered_DV.Sort = "OTHER_AC_ASKING_PRICE ASC"
        Case "COMP_NAME ASC", "COMP_NAME DESC"
          runSearch = True
        Case Else
          Filtered_DV.Sort = UCase(SortMethod)
      End Select


      If runSearch = False Then
        aTempTable = Filtered_DV.ToTable

        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            Table_List = aTempTable
            RaiseEvent BringResults()
            error_results.Text = ""
          Else
            Table_List = New DataTable
            error_results.Text = "<p align='center'>0 Records have been found. Please try again.</p>"
            RaiseEvent BringResults()
          End If
        Else
          'Nothing was Returned
          error_results.Text = "<p align='center'>0 Records have been found. Please try again.</p>"
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("main_site.Master.vb - Fill_Jobs() - " & error_string)
          End If
          display_error()
          aclsData_Temp.class_error = ""
        End If
      ElseIf runSearch = True Then
        'Rerun the search.
        Dim SortDropdown As New DropDownList

        If Not IsNothing(aircraftSearch.FindControl("sort_by_cbo")) Then
          If TypeOf aircraftSearch.FindControl("sort_by_cbo") Is DropDownList Then
            SortDropdown = aircraftSearch.FindControl("sort_by_cbo")
            SortDropdown.SelectedValue = special_sort_by_cbo.SelectedValue
          End If
        End If

        If Not IsNothing(aircraftSearch.FindControl("sort_method_cbo")) Then
          If TypeOf aircraftSearch.FindControl("sort_method_cbo") Is DropDownList Then
            SortDropdown = New DropDownList
            SortDropdown = aircraftSearch.FindControl("sort_method_cbo")
            SortDropdown.SelectedValue = special_sort_method_cbo.SelectedValue
          End If
        End If

        aircraftSearch.Click_Search()

      End If
    End If
  End Sub
End Class