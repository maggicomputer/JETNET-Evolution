
' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/MarketSummary.aspx.vb $
'$$Author: Mike $
'$$Date: 5/28/20 5:34p $
'$$Modtime: 5/28/20 4:49p $
'$$Revision: 4 $
'$$Workfile: MarketSummary.aspx.vb $
'
' ********************************************************************************

Partial Public Class MarketSummary

  Inherits System.Web.UI.Page

  Public productCodeCount As Integer = 0
  Public isHeliOnlyProduct As Boolean = False
  Public isBusinessOnlyProduct As Boolean = False
  Public isCommercialOnlyProduct As Boolean = False
  Private bHasHelicopterFilter As Boolean = False
  Private bHasBusinessFilter As Boolean = False
  Private bHasCommercialFilter As Boolean = False

  Private bPreviousSummary As Boolean = False

  Dim ErrorReportingTypeString As String = "Market"
  Private sTypeMakeModelCtrlBaseName As String = "Aircraft"
  Private sMarketDateCtrlBaseName As String = ""

  Public ModelsString As String = ""
  Public MakeString As String = ""
  Public TypeString As String = ""
  Public AirframeTypeString As String = ""
  Public CombinedAirframeTypeString As String = ""

  Public WeightClassDDL As DropDownList = Nothing
  Public WeightClassStr As String = ""

  Public ManufacturerStr As String = ""

  Public AcSizeStr As String = ""

  Public BuildSearchString As String = ""

  Public TransactionTypeLBString As String = ""
  Public TransactionNotFromLBString As String = ""
  Public TransactionFromLBString As String = ""
  Public TransactionNotToLBString As String = ""
  Public TransactionToLBString As String = ""

  Const sSiteStyleSheet As String = "common\marketSummary.css"
  Const sMasterTransList As String = "WS,OM,MA,WO,DP,FS,SS,FC,L,SZ"

  Private sTransList As String = "WS,OM,MA,WO,DP,FS,SS,FC,L,SZ"

  Private dtMonthBottom As Date = CDate("10/01/1989")
  Private dtYearBottom As Date = CDate("01/01/1990")

  Private dtHeliMonthBottom As Date = CDate("01/01/2006")
  Private dtHeliYearBottom As Date = CDate("01/01/2006")

  Public sTimeScale As String = "Months" 'default market summary values
  Public nScaleSets As Integer = 6 'default market summary values

  Public nEndScale As Integer = 12 'default market summary values

  Private nTotalRecords As Integer = 0
  Public nMarketModelID As Long = -1

  Private sMarketSumFileName As String = ""
  Private sMarketSumFileName_wHeader As String = ""
  Private marketFile As System.IO.StreamWriter
  Private marketFile_wHeader As System.IO.StreamWriter


  Private aBusinessTypesArray(,) As String = Nothing

  Private fSubins_platform_os As String = ""

  Private localFunctions As marketSummaryFunctions

  Const graphMAXITEMS = 42

  Private localGraphArray() As marketGraphData
  Private localACSelection As marketSummaryObjAircraft

  Public linkInfo As String = ""
  Dim LookupDataSet As New DataSet

  Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

    If Not IsNothing(Request.Item("restart")) Then
      If Not String.IsNullOrEmpty(Request.Item("restart").ToString) Then
        If Request.Item("restart") = "1" Then
          reset_Click(reset, System.EventArgs.Empty)
        End If
      End If
    End If

    If Session.Item("crmUserLogon") <> True Then
      Response.Redirect("Default.aspx", False)
    Else

      If Not Page.IsPostBack Then
        'This needs to be put in and loaded for now. Hopefully whenever the session variables are the same, this can go away.
        If Not Session.Item("localPreferences").loadUserSession("", CLng(Session.Item("localUser").crmSubSubID.ToString), Session.Item("localUser").crmUserLogin.ToString, CLng(Session.Item("localUser").crmSubSeqNo.ToString), CLng(Session.Item("localUser").crmUserContactID.ToString)) Then
          Response.Write("error in load preferences : ")
        End If

        'This will go ahead and set up the javascript control array. Not needed unless you're going to need the array (such as to find an amod ID index) before the search button is clicked
        'Generally you won't, but on the ac listing page, you use folders and the home page market tab
        Dim localDataLayer As New viewsDataLayer
        'This basically loads the array into session.
        commonEvo.fillAirframeArray("")
        commonEvo.fillAircraftTypeLableArray("")
        commonEvo.fillDefaultAirframeArray("")

        commonEvo.fillMfrNamesArray("")
        commonEvo.fillAircraftSizeArray("")

        isHeliOnlyProduct = HttpContext.Current.Session.Item("localPreferences").isHeliOnlyProduct
        isBusinessOnlyProduct = HttpContext.Current.Session.Item("localPreferences").isBusinessOnlyProduct
        isCommercialOnlyProduct = HttpContext.Current.Session.Item("localPreferences").isCommercialOnlyProduct

        If isHeliOnlyProduct Then
          HttpContext.Current.Session.Item("hasModelFilter") = True
          Session.Item("hasHelicopterFilter") = True
        ElseIf isBusinessOnlyProduct Then
          HttpContext.Current.Session.Item("hasModelFilter") = True
          Session.Item("hasBusinessFilter") = True
        ElseIf isCommercialOnlyProduct Then
          HttpContext.Current.Session.Item("hasModelFilter") = True
          Session.Item("hasCommercialFilter") = True
        ElseIf productCodeCount = 2 Then
          HttpContext.Current.Session.Item("hasModelFilter") = True

          If Session.Item("localPreferences").UserHelicopterFlag And Session.Item("localPreferences").UserBusinessFlag Then
            Session.Item("hasBusinessFilter") = True
            Session.Item("hasHelicopterFilter") = False
          End If

          If Session.Item("localPreferences").UserBusinessFlag And Session.Item("localPreferences").UserCommercialFlag Then
            Session.Item("hasBusinessFilter") = True
            Session.Item("hasCommercialFilter") = False
          End If

          If Session.Item("localPreferences").UserHelicopterFlag And Session.Item("localPreferences").UserCommercialFlag Then
            Session.Item("hasHelicopterFilter") = True
            Session.Item("hasCommercialFilter") = False
          End If

        ElseIf productCodeCount > 2 Then

          HttpContext.Current.Session.Item("hasModelFilter") = True

          Session.Item("hasHelicopterFilter") = False
          Session.Item("hasBusinessFilter") = True
          Session.Item("hasCommercialFilter") = False

        End If

        Master.aclsData_Temp.FillCacheLookups()

        'Initializing Cache Dataset
        If Not IsNothing(Cache("CacheLookups")) Then
          LookupDataSet = Cache("CacheLookups")
        End If

        If Not IsNothing(LookupDataSet.Tables(0)) Then
          clsGeneral.clsGeneral.Populate_Listbox(LookupDataSet.Tables(0), transaction_from_lb, "cbus_name", "cbus_type", False)
        End If

        If Not IsNothing(LookupDataSet.Tables(0)) Then
          clsGeneral.clsGeneral.Populate_Listbox(LookupDataSet.Tables(0), transaction_to_lb, "cbus_name", "cbus_type", False)
        End If


        'Setting up the project search
        If Page.Request.Form("project_search") = "Y" Then
          'ClearSelections()
          Dim folderID As Long = 0
          Dim FoldersTableData As New DataTable
          Dim cfolderData As String = ""


          FolderInformation.Text = ""
          FolderInformation.Visible = False
          folderID = Page.Request.Form("project_id")

          FoldersTableData = Master.aclsData_Temp.GetEvolutionFolderssBySubscription(folderID, Session.Item("localUser").crmUserLogin, Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmSubSeqNo, "", 0, Nothing, "")
          If Not IsNothing(FoldersTableData) Then
            If FoldersTableData.Rows.Count > 0 Then
              cfolderData = FoldersTableData.Rows(0).Item("cfolder_data").ToString


              If FoldersTableData.Rows(0).Item("cfolder_method").ToString = "S" Then
                folder_name.Text = FoldersTableData.Rows(0).Item("cfolder_name").ToString
              End If
              If cfolderData <> "" Then
                'Fills up the applicable folder Information pulled from the cfolder data field
                DisplayFunctions.FillUpFolderInformation(Table2, close_current_folder, cfolderData, FolderInformation, FoldersTableData, False, False, False, False, False, Collapse_Panel, actions_submenu_dropdown, Nothing, Nothing, Nothing, "", False, False, True)
              End If
            End If

          End If
        End If
      End If
    End If
  End Sub

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Dim temp_selected As String = ""
    Dim sErrorString As String = ""

    Try

      If Session.Item("crmUserLogon") <> True Then
        Response.Redirect("Default.aspx", False)
      Else

        If Not IsNothing(Request.Item("previousSummary")) Then
          If Not String.IsNullOrEmpty(Request.Item("previousSummary").ToString) Then
            If CBool(Request.Item("previousSummary").ToString) Then
              bPreviousSummary = True
            End If
          End If
        End If

        PanelCollapseEx.Collapsed = False
        PanelCollapseEx.ClientState = "False"

        ' set up array to hold graph data from market summary
        ReDim localGraphArray(graphMAXITEMS - 1)

        localACSelection = New marketSummaryObjAircraft

        localFunctions = New marketSummaryFunctions
        localFunctions.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

        localFunctions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
        localFunctions.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
        localFunctions.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
        localFunctions.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

        fSubins_platform_os = commonEvo.getBrowserCapabilities(Request.Browser)


        localFunctions.FillBusinessTypeArray(aBusinessTypesArray)

        'DisplayFunctions.FillUpSessionForMakeTypeModel(sTypeMakeModelCtrlBaseName, ViewTMMDropDowns)


        'This is going to set the control with different parameter variables.
        ViewTMMDropDowns.setIsView(False)
        ViewTMMDropDowns.setShowWeightClass(True)
        ViewTMMDropDowns.setListSize(16)
        ViewTMMDropDowns.setControlName(sTypeMakeModelCtrlBaseName)
        ViewTMMDropDowns.setShowMfrNames(True)
        ViewTMMDropDowns.setShowAcSize(True)

        Dim bControlPanelSelection As Boolean = False
        fill_session_variables(bControlPanelSelection)

        MarketSummaryPickDateID.setIsView(False)
        MarketSummaryPickDateID.setControlName(sMarketDateCtrlBaseName)
        MarketSummaryPickDateID.setValues(Session.Item("marketTimeScale").ToString, Session.Item("marketStartDate").ToString, CInt(Session.Item("marketScaleSets").ToString))

        'Setting the active tab (the navigation link)
        Master.Set_Active_Tab(8)
        'This will set page title.
        Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Market Summary Search Results")

        'Sets up the page for an initial load, this is true if no search is performed (initial load), false if searched on.
        scp_tr_2.Visible = False
        scp_tr_3.Visible = False

        'Set up bars to display correctly.
        If Not Page.IsPostBack Then
          Dim FoldersTable As New DataTable

          'Fill Folders Table
          folders_submenu_dropdown.Items.Clear()
          DisplayFunctions.AddEditFolderListOptionToFolderDropdown(folders_submenu_dropdown, 13)
          FoldersTable = Master.aclsData_Temp.GetEvolutionFolderssBySubscription(0, Session.Item("localUser").crmUserLogin, Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmSubSeqNo, "", 13, Nothing, "")
          If Not IsNothing(FoldersTable) Then
            If FoldersTable.Rows.Count > 0 Then
              For Each r As DataRow In FoldersTable.Rows
                If Not IsDBNull(r("cfolder_data")) Then
                  Dim FolderDataString As Array
                  'this was added to parse out the real search query now that we're saving it
                  FolderDataString = Split(r("cfolder_data"), "THEREALSEARCHQUERY")
                  folders_submenu_dropdown.Items.Add(New ListItem(r("cfolder_name").ToString, "javascript:ParseSpecsOperatingMarketForm('" & r("cfolder_id").ToString & "',false,false,true,'" & Replace(FolderDataString(0), "'", "\'") & "');"))
                End If
              Next
            End If
          End If
        End If


        ' if amod_id has data that overides type/make/model selections
        If Not IsNothing(Request.Item("amod_id")) Then
          If Not String.IsNullOrEmpty(Request.Item("amod_id").ToString) Then

            nMarketModelID = CLng(Request.Item("amod_id").ToString.Trim)

            Session.Item("tabAircraftModel") = commonEvo.FindIndexForItemByAmodID(nMarketModelID)
            Session.Item("tabAircraftMake") = Session.Item("tabAircraftModel")
            Session.Item("tabAircraftType") = Session.Item("tabAircraftModel")

          End If
        End If

        If Not IsPostBack And nMarketModelID = -1 Then
          If bPreviousSummary Then
            summary_search_Click(sender, e)
            bPreviousSummary = False
          Else
            HttpContext.Current.Session.Item("MasterMarketAvailableSummary") = ""
            HttpContext.Current.Session.Item("MasterMarketTransactionSummary") = ""
            Initial(True)
          End If
        Else

          If bPreviousSummary Then
            bPreviousSummary = False
          End If

          HttpContext.Current.Session.Item("MasterMarketAvailableSummary") = ""
          HttpContext.Current.Session.Item("MasterMarketTransactionSummary") = ""
          If bControlPanelSelection Or nMarketModelID > -1 Then
            summary_search_Click(sender, e)
          End If

        End If


        SetUpJavascriptDropDown() 'Setting up the javascript bulleted list dropdowns.

        'add_nextPrevious_Script()
        chkAvailableID.Attributes.Add("onclick", "updateMarketCheckBoxWarning();")
        chkTransactionsID.Attributes.Add("onclick", "updateMarketCheckBoxWarning();")
        add_marketCheckBoxWarning_Script(chkAvailableID, chkTransactionsID)

        chkNewToMarketID.Attributes.Add("onclick", "updateNewToMarket();")
        chkUsedMarketID.Attributes.Add("onclick", "updateUsedMarket();")
        add_marketCheckBoxNewUsed_Script(chkNewToMarketID, chkUsedMarketID)

        'This will go ahead and display it on the master page.
        Master.SetStatusText(HttpContext.Current.Session.Item("SearchString").ToString)

      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in page_load()" + ex.Message

    Finally

    End Try

  End Sub

  Private Sub reset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles reset.Click
    ResetPage()
  End Sub

  Private Sub ResetPage()
    ClearSelections()
    Response.Redirect("MarketSummary.aspx")
  End Sub

  Private Sub ClearSelections()
    'Clear out the Type/Make/Model Boxes Properly on Reset:
    HttpContext.Current.Session.Item("tabAircraftType") = ""
    HttpContext.Current.Session.Item("tabAircraftMake") = ""
    HttpContext.Current.Session.Item("tabAircraftModel") = ""
    HttpContext.Current.Session.Item("tabAircraftModelWeightClass") = ""
    HttpContext.Current.Session.Item("tabAircraftMfrNames") = ""
    HttpContext.Current.Session.Item("tabAircraftSize") = ""


    HttpContext.Current.Session.Item("hasModelFilter") = True

    Session.Item("chkHelicopterFilter") = False
    Session.Item("chkBusinessFilter") = True
    Session.Item("chkCommercialFilter") = False

    Session.Item("searchCriteria") = New SearchSelectionCriteria
  End Sub

  Private Sub Initial(ByVal initial_page_load As Boolean)

    If initial_page_load Then

      criteria_results.Visible = False
      sort_by_text.Visible = False
      sort_by_dropdown.Visible = False
      actions_dropdown.Visible = False
      paging.Visible = False

    Else

      criteria_results.Visible = True
      sort_by_text.Visible = False
      sort_by_dropdown.Visible = False
      actions_dropdown.Visible = True
      paging.Visible = False

    End If

  End Sub

  Private Sub SetUpJavascriptDropDown()
    'setting the javascript of the menus
    'sort
    sort_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" + sort_submenu_dropdown.ClientID + "', true);")
    sort_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" + sort_submenu_dropdown.ClientID + "', false);")

    sort_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" + sort_submenu_dropdown.ClientID + "', true);")
    sort_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" + sort_submenu_dropdown.ClientID + "', false);")

    'actions dropdown
    actions_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" + actions_submenu_dropdown.ClientID + "', true);")
    actions_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" + actions_submenu_dropdown.ClientID + "', false);")

    actions_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" + actions_submenu_dropdown.ClientID + "', true);")
    actions_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" + actions_submenu_dropdown.ClientID + "', false);")

    'folder dropdown
    folders_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & folders_submenu_dropdown.ClientID & "', true);")
    folders_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & folders_submenu_dropdown.ClientID & "', false);")

    folders_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & folders_submenu_dropdown.ClientID & "', true);")
    folders_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & folders_submenu_dropdown.ClientID & "', false);")

  End Sub

  Private Sub add_nextPrevious_Script()

    'Register the script block
    Dim sScptStr As StringBuilder = New StringBuilder()

    If Not Page.ClientScript.IsClientScriptBlockRegistered("nextPrevious_Script") Then

      sScptStr.Append("<script type=""text/javascript"">")
      sScptStr.Append(vbCrLf + "  function setPrevious() {")
      sScptStr.Append(vbCrLf + "    document.getElementById(""marketSumDirectionID"").value = ""previous"";")
      sScptStr.Append(vbCrLf + "    document.getElementById(""marketControlPanelSelectionID"").value = ""true"";")
      sScptStr.Append(vbCrLf + "    return true;")
      sScptStr.Append(vbCrLf + "  }")
      sScptStr.Append(vbCrLf + "  function setNext() {")
      sScptStr.Append(vbCrLf + "    document.getElementById(""marketSumDirectionID"").value = ""next"";")
      sScptStr.Append(vbCrLf + "    document.getElementById(""marketControlPanelSelectionID"").value = ""true"";")
      sScptStr.Append(vbCrLf + "    return true;")
      sScptStr.Append(vbCrLf + "  }")
      sScptStr.Append(vbCrLf + "</script>")

      Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "nextPrevious_Script", sScptStr.ToString, False)

    End If

    sScptStr = Nothing


  End Sub

  Private Sub add_marketCheckBoxWarning_Script(ByVal cbSource As CheckBox, ByVal cbSource1 As CheckBox)

    'Register the script block
    Dim sScptStr As StringBuilder = New StringBuilder()

    If Not Page.ClientScript.IsClientScriptBlockRegistered("checkBoxWarning_Script") Then

      sScptStr.Append("<script type=""text/javascript"">")
      sScptStr.Append(vbCrLf + "  function updateMarketCheckBoxWarning() {")
      sScptStr.Append(vbCrLf + "    if ((!document.getElementById(""" + cbSource1.ClientID.ToString + """).checked) && (!document.getElementById(""" + cbSource.ClientID.ToString + """).checked)) {")
      sScptStr.Append(vbCrLf + "      document.getElementById(""WarningID"").style.visibility = ""visible"";")
      sScptStr.Append(vbCrLf + "    }")
      sScptStr.Append(vbCrLf + "    else {")
      sScptStr.Append(vbCrLf + "      document.getElementById(""WarningID"").style.visibility = ""hidden"";")
      sScptStr.Append(vbCrLf + "    }")
      sScptStr.Append(vbCrLf + "    return true;")
      sScptStr.Append(vbCrLf + "  }")
      sScptStr.Append(vbCrLf + "</script>")

      Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "checkBoxWarning_Script", sScptStr.ToString, False)

    End If

    sScptStr = Nothing

  End Sub

  Private Sub add_marketCheckBoxNewUsed_Script(ByVal cbSource As CheckBox, ByVal cbSource1 As CheckBox)

    'Register the script block
    Dim sScptStr As StringBuilder = New StringBuilder()

    If Not Page.ClientScript.IsClientScriptBlockRegistered("checkNewUsed_Script") Then

      sScptStr.Append("<script type=""text/javascript"">")
      sScptStr.Append(vbCrLf + "  function updateNewToMarket() {")
      sScptStr.Append(vbCrLf + "    if (document.getElementById(""" + cbSource.ClientID.ToString + """).checked == true) {")
      sScptStr.Append(vbCrLf + "      document.getElementById(""" + cbSource1.ClientID.ToString + """).checked = false;")
      sScptStr.Append(vbCrLf + "    }")
      sScptStr.Append(vbCrLf + "    return true;")
      sScptStr.Append(vbCrLf + "  }")
      sScptStr.Append(vbCrLf + "  function updateUsedMarket() {")
      sScptStr.Append(vbCrLf + "    if (document.getElementById(""" + cbSource1.ClientID.ToString + """).checked == true) {")
      sScptStr.Append(vbCrLf + "      document.getElementById(""" + cbSource.ClientID.ToString + """).checked = false;")
      sScptStr.Append(vbCrLf + "    }")
      sScptStr.Append(vbCrLf + "    return true;")
      sScptStr.Append(vbCrLf + "  }")
      sScptStr.Append(vbCrLf + "</script>")

      Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "checkNewUsed_Script", sScptStr.ToString, False)

    End If

    sScptStr = Nothing

  End Sub

  Private Sub fill_session_variables(ByRef bControlPanelSelection As Boolean)

    Dim tmpEndDate As String = ""
    Dim tmpStartDate As String = ""
    Dim tmpSpanRange As Integer = 0

    Dim bTimeSpanChanged As Boolean = False

    Try

      If Not IsNothing(ViewTMMDropDowns.FindControl("ddlWeightClass")) Then
        WeightClassDDL = ViewTMMDropDowns.FindControl("ddlWeightClass")
        HttpContext.Current.Session.Item("tabAircraftModelWeightClass") = WeightClassDDL.SelectedValue
      End If

      'Model/Make/Type String Building, this function is going to pull those information from the type make model control, plus build the search string
      'All you need to do is call it.
      'Unfortunately because of how this page is set up, we have to go ahead and grab session chkFilters before we run this control.
      bHasBusinessFilter = HttpContext.Current.Session.Item("hasBusinessFilter")
      bHasCommercialFilter = HttpContext.Current.Session.Item("hasCommercialFilter")
      bHasHelicopterFilter = HttpContext.Current.Session.Item("hasHelicopterFilter")

      BuildSearchString += GetMARKETMakeModelTypeFromCommonControl("", BuildSearchString, ModelsString, MakeString, TypeString, AirframeTypeString, CombinedAirframeTypeString,
                            WeightClassDDL, WeightClassStr, ManufacturerStr, AcSizeStr, bHasBusinessFilter, bHasHelicopterFilter, bHasCommercialFilter)

      localACSelection.ModelsString = ModelsString
      localACSelection.MakeString = MakeString
      localACSelection.TypeString = TypeString
      localACSelection.AirframeTypeString = AirframeTypeString
      localACSelection.CombinedAirframeTypeString = CombinedAirframeTypeString
      localACSelection.WeightString = WeightClassStr
      localACSelection.MfrNamesString = ManufacturerStr
      localACSelection.AcsizeString = AcSizeStr

      linkInfo = localFunctions.make_linkback_aircraftInfo(localACSelection)

      'I included a couple of examples of functions that might be useful.
      'This first one will get all the selected information from a listbox. We'll take transaction_from_lb as an example.
      'The first parameter of the function is the listbox name, the second is whether the resulting string needs to be in quotes.
      'So like if you have 'Challenger', 'Astra' = that would be true, and 300, 372 would be false.
      'The third and fourth parameters work together. If you store a piped string in the listbox because you need two values, like J|F and you only need the J slot, you'd pass 1 and false
      'Generally it'll just be 0 and true though which treats it like a flat value.

      TransactionTypeLBString = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(transaction_type_lb, True, 0, True)

      TransactionFromLBString = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(transaction_from_lb, True, 0, True)
      TransactionToLBString = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(transaction_to_lb, True, 0, True)

      'Here's another useful function - it involves the top center textual display after to run a search, that displays what you searched on.
      If Not String.IsNullOrEmpty(TransactionTypeLBString.Trim) Then
        BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(transaction_type_lb, " Transaction ")
      End If
      If Not String.IsNullOrEmpty(TransactionFromLBString.Trim) Then
        BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(transaction_from_lb, transaction_from.SelectedValue.ToString + " Aircraft Contact type ")
      End If
      If Not String.IsNullOrEmpty(TransactionToLBString.Trim) Then
        BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(transaction_to_lb, transaction_to.SelectedValue.ToString + " Aircraft Contact type ")
      End If

      'This will go ahead and set the buildsearchstring to a session variable:
      HttpContext.Current.Session.Item("SearchString") = BuildSearchString

      If chkTransactionsID.Checked Then
        hasTransactionSummary.Text = "Include"
      Else
        hasTransactionSummary.Text = "Does not include"
      End If

      'If Not IsNothing(Request.Item("marketControlPanelSelection")) Then
      '  If Not String.IsNullOrEmpty(Request.Item("marketControlPanelSelection").ToString.Trim) Then
      '    bControlPanelSelection = CBool(Request.Item("marketControlPanelSelection").ToString)
      '  End If
      'End If

      If scp_tr_2.Visible Then

        If Not String.IsNullOrEmpty(summary_type_DropDownList.SelectedValue) Then
          If Not summary_type_DropDownList.SelectedValue.ToLower.Contains(Session.Item("marketSumType").ToString.ToLower) Then
            Session.Item("marketSumType") = summary_type_DropDownList.SelectedValue
            bControlPanelSelection = True
          Else
            Session.Item("marketSumType") = "trans_type"
          End If
        End If

        If Not String.IsNullOrEmpty(tx_types_DropDownList.SelectedValue) Then
          If Not tx_types_DropDownList.SelectedValue.ToLower.Contains(TransactionTypeLBString.Replace(crmWebClient.Constants.cSingleQuote, "").ToLower.Trim) Then
            sTransList = tx_types_DropDownList.SelectedValue.ToLower
            bControlPanelSelection = True
          End If
        End If

        If Not String.IsNullOrEmpty(time_scale_DropDownList.SelectedValue) Then
          If Not time_scale_DropDownList.SelectedValue.ToLower.Contains(Session.Item("marketTimeScale").ToString.ToLower) Then
            sTimeScale = time_scale_DropDownList.SelectedValue
            nScaleSets = localFunctions.ReturnDefaultSummaryRange_Server(sTimeScale)
            bControlPanelSelection = True
            bTimeSpanChanged = True
          Else
            sTimeScale = Session.Item("marketTimeScale").ToString.ToLower
          End If
        End If

        If Not String.IsNullOrEmpty(range_span_DropDownList.SelectedValue) Then
          If Not range_span_DropDownList.SelectedValue.ToLower.Contains(Session.Item("marketScaleSets").ToString.ToLower) Then

            nScaleSets = CInt(range_span_DropDownList.SelectedValue)

            If nScaleSets > localFunctions.ReturnTotalRange_Server(sTimeScale) Then
              nScaleSets = localFunctions.ReturnDefaultSummaryRange_Server(sTimeScale)
            End If

            bControlPanelSelection = True

          Else
            If Not bTimeSpanChanged Then ' use the session time scale if we havent changed timespan
              nScaleSets = Session.Item("marketScaleSets")
            End If
          End If
        End If

        If Not String.IsNullOrEmpty(start_date_DropDownList.SelectedValue) Then
          If CDate(start_date_DropDownList.SelectedValue) <> CDate(Session.Item("marketStartDate")) Then
            tmpStartDate = start_date_DropDownList.SelectedValue
            bControlPanelSelection = True
          Else
            If Not bTimeSpanChanged Then ' use the session start date if we havent changed timespan
              tmpStartDate = Session.Item("marketStartDate").ToString
            End If
          End If
        End If

      End If

      If Not bControlPanelSelection Then
        If Not IsNothing(Request.Item("chkHelicopterFilter")) Then
          If Not String.IsNullOrEmpty(Request.Item("chkHelicopterFilter").ToString) Then
            bHasHelicopterFilter = CBool(Request.Item("chkHelicopterFilter").ToString.Trim)

          End If
        End If

        If Not IsNothing(Request.Item("chkBusinessFilter")) Then
          If Not String.IsNullOrEmpty(Request.Item("chkBusinessFilter").ToString) Then
            bHasBusinessFilter = CBool(Request.Item("chkBusinessFilter").ToString.Trim)
          End If
        End If

        If Not IsNothing(Request.Item("chkCommercialFilter")) Then
          If Not String.IsNullOrEmpty(Request.Item("chkCommercialFilter").ToString) Then
            bHasCommercialFilter = CBool(Request.Item("chkCommercialFilter").ToString.Trim)
          End If
        End If

        If Not isHeliOnlyProduct Then ' check the filters to see if they have picked only heli
          isHeliOnlyProduct = bHasHelicopterFilter And Not (bHasBusinessFilter Or bHasCommercialFilter)
        End If

        localACSelection.bHasHelicopter = bHasHelicopterFilter
        localACSelection.bHasCommercial = bHasCommercialFilter
        localACSelection.bHasBusiness = bHasBusinessFilter

        ' set the timescale for the market summaries
        If Not IsNothing(Request.Item("cbo" + sMarketDateCtrlBaseName + "TimeScale")) Then
          If Not String.IsNullOrEmpty(Request.Item("cbo" + sMarketDateCtrlBaseName + "TimeScale").ToString.Trim) Then
            sTimeScale = Request("cbo" + sMarketDateCtrlBaseName + "TimeScale").ToString.Trim
          End If
        Else
          If Page.Request.Form("project_search") = "Y" Or bPreviousSummary Then
            If Not IsNothing(Session.Item("marketTimeScale")) Then
              If Not String.IsNullOrEmpty(Session.Item("marketTimeScale")) Then
                sTimeScale = Session.Item("marketTimeScale")
              End If
            End If
          End If
        End If

        ' set the # of timescale sets for the market summaries
        If Not IsNothing(Request.Item("cbo" + sMarketDateCtrlBaseName + "RangeSpan")) Then
          If Not String.IsNullOrEmpty(Request.Item("cbo" + sMarketDateCtrlBaseName + "RangeSpan").ToString.Trim) Then
            If IsNumeric(Request.Item("cbo" + sMarketDateCtrlBaseName + "RangeSpan").ToString.Trim) And CInt(Request.Item("cbo" + sMarketDateCtrlBaseName + "RangeSpan").ToString.Trim) Then

              nScaleSets = CInt(Request.Item("cbo" + sMarketDateCtrlBaseName + "RangeSpan").ToString.Trim)

              If nScaleSets > localFunctions.ReturnTotalRange_Server(sTimeScale) Then
                nScaleSets = localFunctions.ReturnDefaultSummaryRange_Server(sTimeScale)
              End If

            End If
          End If
        Else
          If Page.Request.Form("project_search") = "Y" Or bPreviousSummary Then
            If Not IsNothing(Session.Item("marketScaleSets")) Then
              If Not String.IsNullOrEmpty(Session.Item("marketScaleSets")) Then

                nScaleSets = Session.Item("marketScaleSets")

                If nScaleSets > localFunctions.ReturnTotalRange_Server(sTimeScale) Then
                  nScaleSets = localFunctions.ReturnDefaultSummaryRange_Server(sTimeScale)
                End If
              End If
            End If
          End If
        End If

        If Not IsNothing(Request.Item("cbo" + sMarketDateCtrlBaseName + "StartDate")) Then
          If Not String.IsNullOrEmpty(Request.Item("cbo" + sMarketDateCtrlBaseName + "StartDate").ToString.Trim) Then

            If Not IsNothing(Request.Item("marketSumDirection")) Then ' not used at the moment
              If Not String.IsNullOrEmpty(Request.Item("marketSumDirection").ToString.Trim) Then

                tmpStartDate = localFunctions.return_next_previous_date(Request.Item("marketSumDirection").ToString, sTimeScale, CDate(Session.Item("marketStartDate").ToString)).ToShortDateString

                ' we need to check to make sure we dont go past our bottom data limits
                Select Case sTimeScale.ToLower.Trim

                  Case "years", "quarters"
                    If isHeliOnlyProduct Then
                      If Year(dtHeliYearBottom) >= Year(CDate(Session.Item("marketStartDate"))) And Month(dtHeliYearBottom) >= Month(CDate(Session.Item("marketStartDate"))) Then
                        tmpStartDate = dtHeliYearBottom.ToString
                      End If
                    Else
                      If Year(dtYearBottom) >= Year(CDate(Session.Item("marketStartDate"))) And Month(dtYearBottom) >= Month(CDate(Session.Item("marketStartDate"))) Then
                        tmpStartDate = dtYearBottom.ToString
                      End If
                    End If

                  Case Else
                    If isHeliOnlyProduct Then
                      If Year(dtHeliMonthBottom) >= Year(CDate(Session.Item("marketStartDate"))) And Month(dtHeliMonthBottom) >= Month(CDate(Session.Item("marketStartDate"))) Then
                        tmpStartDate = dtHeliMonthBottom.ToString
                      End If
                    Else
                      If Year(dtMonthBottom) >= Year(CDate(Session.Item("marketStartDate"))) And Month(dtMonthBottom) >= Month(CDate(Session.Item("marketStartDate"))) Then
                        tmpStartDate = dtMonthBottom.ToString
                      End If
                    End If

                End Select

              Else

                tmpStartDate = Request.Item("cbo" + sMarketDateCtrlBaseName + "StartDate").ToString.Trim

              End If

            Else

              tmpStartDate = Request.Item("cbo" + sMarketDateCtrlBaseName + "StartDate").ToString.Trim

            End If

          End If
        Else
          If Page.Request.Form("project_search") = "Y" Or bPreviousSummary Then
            If Not IsNothing(Session.Item("marketStartDate")) Then
              If Not String.IsNullOrEmpty(Session.Item("marketStartDate")) Then
                tmpStartDate = Session.Item("marketStartDate")
              End If
            End If
          End If
        End If

      End If

      tmpEndDate = Now().ToShortDateString

      tmpSpanRange = nScaleSets

      localFunctions.set_summary_date_range(tmpEndDate, tmpStartDate, sTimeScale, tmpSpanRange, isHeliOnlyProduct)

      nScaleSets = tmpSpanRange

      Session.Item("marketEndDate") = tmpEndDate
      Session.Item("marketStartDate") = tmpStartDate

      Session.Item("marketTimeScale") = sTimeScale
      Session.Item("marketScaleSets") = nScaleSets

      If String.IsNullOrEmpty(Session.Item("marketSumType").ToString.Trim) Then  ' if nothing chosen default to Seller/Purchaser
        Session.Item("marketSumType") = "trans_type"
      End If

      'If Not String.IsNullOrEmpty(Session.Item("marketEndDate").ToString.Trim) Then
      '  If sTimeScale.ToLower = "years" Then
      '    If Year(CDate(Session.Item("marketEndDate"))) = Year(Now()) Then
      '      marketSumDirectionNext.Visible = False
      '    Else
      '      marketSumDirectionNext.Visible = True
      '    End If
      '  Else
      '    If Year(CDate(Session.Item("marketEndDate"))) = Year(Now()) And Month(CDate(Session.Item("marketEndDate"))) = Month(Now()) Then
      '      marketSumDirectionNext.Visible = False
      '    Else
      '      marketSumDirectionNext.Visible = True
      '    End If
      '  End If
      'Else
      '  marketSumDirectionNext.Visible = False
      'End If

      nEndScale = localFunctions.ReturnTotalRange_Server(sTimeScale)

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in fill_session_variables(ByRef bControlPanelSelection As Boolean)" + ex.Message
    End Try

  End Sub

  Private Function postBackScript() As String
    Dim scriptOut As New StringBuilder

    'scriptOut.Append("alert(""run fillStartDateJS and refreshTypeMakeModelByCheckBox script and product check boxes"");")

    'scriptOut.Append(" document.getElementById(""ctl00_WelcomeUser1_CRM_Logo_Text"").value = ""<div class=""current_status_div"">" + BuildSearchString + "</div>"";")
    'scriptOut.Append("alert(document.getElementById(""ctl00_WelcomeUser1_CRM_Logo_Text"").value);")

    If Not Session.Item("localPreferences").UserHelicopterFlag Then
      scriptOut.Append("if ((typeof(document.getElementById(""chkHelicopterFilterID"")) != ""undefined"") && (document.getElementById(""chkHelicopterFilterID"") != null)) {" + vbCrLf)
      scriptOut.Append(" document.getElementById(""chkHelicopterFilterID"").style.visibility = ""hidden"";" + vbCrLf)
      scriptOut.Append("}" + vbCrLf)
    End If

    If Not Session.Item("localPreferences").UserBusinessFlag Then
      scriptOut.Append("if ((typeof(document.getElementById(""chkBusinessFilterID"")) != ""undefined"") && (document.getElementById(""chkBusinessFilterID"") != null)) {" + vbCrLf)
      scriptOut.Append(" document.getElementById(""chkBusinessFilterID"").style.visibility = ""hidden"";" + vbCrLf)
      scriptOut.Append("}" + vbCrLf)
    End If

    If Not Session.Item("localPreferences").UserCommercialFlag Then
      scriptOut.Append("if ((typeof(document.getElementById(""chkCommercialFilterID"")) != ""undefined"") && (document.getElementById(""chkCommercialFilterID"") != null)) {" + vbCrLf)
      scriptOut.Append(" document.getElementById(""chkCommercialFilterID"").style.visibility = ""hidden"";" + vbCrLf)
      scriptOut.Append("}" + vbCrLf)
    End If

    If Not Session.Item("localPreferences").UserRegionalFlag Then
      scriptOut.Append("if ((typeof(document.getElementById(""chkRegionalFilterID"")) != ""undefined"") && (document.getElementById(""chkRegionalFilterID"") != null)) {" + vbCrLf)
      scriptOut.Append(" document.getElementById(""chkRegionalFilterID"").style.visibility = ""hidden"";" + vbCrLf)
      scriptOut.Append("}" + vbCrLf)
    End If

    'scriptOut.Append(" updateMarketCheckBoxWarning();")
    scriptOut.Append(" $(document).ready(function(){fillStartDateJS(""refresh"", " + isHeliOnlyProduct.ToString.ToLower + "," + isBusinessOnlyProduct.ToString.ToLower + "," + isCommercialOnlyProduct.ToString.ToLower + ");});" + vbCrLf)
    scriptOut.Append(" $(document).ready(function(){refreshTypeMakeModelByCheckBox("""", """", " + isHeliOnlyProduct.ToString.ToLower + "," + productCodeCount.ToString + ");});" + vbCrLf)
    scriptOut.Append(" var market_selection = """ + linkInfo.Replace(Constants.cDymDataSeperator, Constants.cSvrStringSeperator).Trim + """;" + vbCrLf)

    Return scriptOut.ToString

    scriptOut = Nothing

  End Function

  Private Sub summary_search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles summary_search.Click

    'This will run the actual search.
    Try

      'Collapse the search panel:
      PanelCollapseEx.Collapsed = True
      PanelCollapseEx.ClientState = "True"

      'This is going to run after the search, it will toggle the actions dropdown in the search bar on or off
      Initial(False)

      ' create the output file when user "clicks" search

      ' Clear the session variable that holds the array on each run
      Session.Item("marketGraphData") = Nothing

      For x As Integer = 0 To graphMAXITEMS - 1
        localGraphArray(x) = New marketGraphData
      Next

      If IsNothing(Session.Item("marketGraphData")) Then
        If Not IsNothing(localGraphArray) And IsArray(localGraphArray) Then
          Session.Item("marketGraphData") = localGraphArray
        End If
      End If

      Dim subscriptionInfo As String = Session.Item("localUser").crmSubSubID.ToString + "_" + Session.Item("localUser").crmUserLogin.ToString.Trim + "_" + Session.Item("localUser").crmSubSeqNo.ToString + "_"
      Dim sReportTitle As String = "export_of_current_market_summary_list_17"

      sMarketSumFileName = commonEvo.GenerateFileName(subscriptionInfo + sReportTitle, ".html", False)
      sMarketSumFileName_wHeader = sMarketSumFileName

      HttpContext.Current.Session.Item("marketSummaryBaseFileName") = ""
      HttpContext.Current.Session.Item("marketSummaryBaseFileName") = sMarketSumFileName.Replace(".html", "") ' just clean off file extension

      Dim sMarketFilePath As String = HttpContext.Current.Server.MapPath(Session.Item("MarketSummaryFolderVirtualPath").ToString) + "\" + sMarketSumFileName
      Dim sMarketFilePath_wheader As String = HttpContext.Current.Server.MapPath(Session.Item("MarketSummaryFolderVirtualPath").ToString) + "\" + sMarketSumFileName_wHeader

      sMarketFilePath_wheader = Replace(sMarketFilePath_wheader, ".html", "_wHeader.html")

      marketFile = System.IO.File.CreateText(sMarketFilePath)
      marketFile_wHeader = System.IO.File.CreateText(sMarketFilePath_wheader)

      ' Read our site style sheet and dump it to the top of the file
      Dim sServerMapPath As String = HttpContext.Current.Server.MapPath(sSiteStyleSheet)

      Dim txtFile = System.IO.File.OpenText(sServerMapPath)
      Dim txtFileLine As String = ""

      If Not txtFile.EndOfStream Then

        localFunctions.WriteLineToFile("<style>", marketFile, marketFile_wHeader)

        Do While txtFile.EndOfStream <> True

          txtFileLine = txtFile.ReadLine
          localFunctions.WriteLineToFile(txtFileLine, marketFile, marketFile_wHeader)

        Loop

        localFunctions.WriteLineToFile("</style>", marketFile, marketFile_wHeader)

        txtFile.Close()

      End If

      txtFile = Nothing

      localFunctions.marketSummaryFile = marketFile
      localFunctions.marketSummaryFile2 = marketFile_wHeader

      Dim comp_functions As New CompanyFunctions
      Dim comp_address As String = ""
      Dim comp_logo As String = ""
      Dim word_width As String = "110%"
      Dim pdf_html_width As String = "95%"


      Call comp_functions.CompanyInformationHeaderWithPNG(0, "", comp_address, comp_logo, Nothing, logo_check, False)

      localFunctions.WriteLineToFile(comp_functions.NEW_build_full_spec_page_header(0, "", comp_address, comp_logo, 0, 0, False, word_width, pdf_html_width, Nothing, Nothing, Nothing, "", False), Nothing, marketFile_wHeader)


      summary_control_panel()
      summary_control_block.Visible = True

      If chkAvailableID.Checked Then

        available_summary_block.Visible = True
        available_summary.Text = build_available_summary()

        Dim tmpOutput = build_retail_summary()

        If Not String.IsNullOrEmpty(tmpOutput.Trim) Then

          If HttpContext.Current.Session.Item("localPreferences").UserSPIViewFlag Then
            available_summary.Text += tmpOutput
          End If

        End If

      Else
        available_summary_block.Visible = False
        available_summary.Text = ""
      End If

      If chkTransactionsID.Checked Then

        Dim tmpOutput = build_transaction_summary()

        If Not String.IsNullOrEmpty(tmpOutput.Trim) Then
          transaction_summary_block.Visible = True
          transaction_summary.Text = tmpOutput
        Else
          transaction_summary_block.Visible = True
          transaction_summary.Text = "<div style='text-align:center;'><div style=""width: 100%; overflow: auto; vertical-align: top;""><font color=""red"">** No Transactions Found **</font></div></div>"
        End If

      Else
        transaction_summary_block.Visible = False
        transaction_summary.Text = ""
      End If

      marketFile.Close()
      marketFile = Nothing

      marketFile_wHeader.Close()
      marketFile_wHeader = Nothing

      ' create excel version of report
      Dim sExcelFileName = HttpContext.Current.Server.MapPath(Session.Item("MarketSummaryFolderVirtualPath").ToString) + "\" + HttpContext.Current.Session.Item("marketSummaryBaseFileName") + ".xls"
      System.IO.File.Copy(sMarketFilePath, sExcelFileName)

      commonLogFunctions.Log_User_Event_Data("UserSearch", "Summary Search: " + clsGeneral.clsGeneral.StripChars(clsGeneral.clsGeneral.stripHTML(Replace(HttpContext.Current.Session.Item("SearchString"), "<br />", " ")), False), Nothing, 0, 0, 0, 0, 0, 0, 0)

      'After you preform the search, you'll have some text display to deal with.
      criteria_results.Text = "Market Summary Result(s)"
      record_count.Text = "" 'if no paging, I would set to blank

      'I'm not sure if this will ever have/need paging, however if it does, this toggles buttons. If it doesn't, you can get rid of the buttons
      SetPagingButtons(False, False)

      System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "updateMarketControls", postBackScript, True)

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in summary_search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles summary_search.Click" + ex.Message

      ' close the file on any exception
      marketFile = Nothing

    End Try

  End Sub

  Private Sub SetPagingButtons(ByVal back_page As Boolean, ByVal next_page As Boolean)
    previous_all.Visible = back_page
    previous.Visible = back_page

    next_all.Visible = next_page
    next_.Visible = next_page

  End Sub

  Private Sub summary_control_panel()

    Dim tmpString As String = ""
    Dim itemCount As Integer = 0

    Try

      scp_tr_2.Visible = True

      localFunctions.WriteLineToFile("<div style='text-align:center;'>", marketFile, marketFile_wHeader)

      ' SET UP TABLE FOR MARKET SUMMARY OPTIONS
      localFunctions.WriteLineToFile("<table border='1' cellpadding='2' cellspacing='0' bordercolor='#949494' width='100%'>", marketFile, marketFile_wHeader)
      localFunctions.WriteLineToFile("<tr><td colspan='12' align='center' valign='middle'>", marketFile, marketFile_wHeader)

      If chkNewToMarketID.Checked Then
        tmpString = "Sales of New Aircraft Only"
      ElseIf chkUsedMarketID.Checked Then
        tmpString = "Sales of Used Aircraft Only"
      Else
        tmpString = "All Aircraft"
      End If

      simpleReportTitle.Text = localFunctions.createSimpleReportTitle(isHeliOnlyProduct, tmpString)

      localFunctions.WriteLineToFile("<font size=""2.5""><b>" + simpleReportTitle.Text + "</b></font>", marketFile, marketFile_wHeader)

      localFunctions.WriteLineToFile("</td></tr></table>", marketFile, marketFile_wHeader)
      localFunctions.WriteLineToFile("</div>", marketFile, marketFile_wHeader)

      ' fill up "time frame" listbox based on market timeScale
      If Not String.IsNullOrEmpty(Session.Item("marketStartDate").ToString.Trim) Then

        localFunctions.fill_startdate_dropdown(isHeliOnlyProduct, start_date_DropDownList)

      End If

      'tmpString = Session.Item("marketTimeScale").ToString.Replace("s", Constants.cEmptyString)

      'marketSumDirectionPrevious.Text = "shift time span back one " + tmpString.ToLower
      'marketSumDirectionNext.Text = "&nbsp;or&nbsp;shift time span forward one " + tmpString.ToLower

      'marketSumDirectionPrevious.PostBackUrl = "MarketSummary.aspx"
      'marketSumDirectionNext.PostBackUrl = "MarketSummary.aspx"

      ' SET THE TYPE OF MARKET SUMMARY DESIRED (WHAT WILL WE SUMMARIZE BY)
      If chkTransactionsID.Checked Then

        scp_tr_3.Visible = True

        If Not String.IsNullOrEmpty(Session.Item("marketSumType").ToString.Trim) Then
          tmpString = Session.Item("marketSumType").ToString.Trim
        Else
          tmpString = "Trans_Type"
        End If

        summary_type_DropDownList.SelectedValue = tmpString.ToLower

        Dim sFullTransList() As String = sMasterTransList.Split(Constants.cCommaDelim)
        tx_types_DropDownList.Items.Clear()

        For iLoop As Integer = 0 To UBound(sFullTransList)

          If iLoop = 0 Then
            tx_types_DropDownList.Items.Add(New ListItem("All", sMasterTransList))

            If String.IsNullOrEmpty(TransactionTypeLBString) And sTransList = sMasterTransList Then
              tx_types_DropDownList.Items(itemCount).Selected = True
            End If

            itemCount += 1

          End If

          tx_types_DropDownList.Items.Add(New ListItem(localFunctions.GetTransTypeName(sFullTransList(iLoop)), sFullTransList(iLoop).Trim))

          If sTransList.ToUpper.Contains(sFullTransList(iLoop)) And sTransList <> sMasterTransList Then
            tx_types_DropDownList.Items(itemCount).Selected = True
          End If

          If Not String.IsNullOrEmpty(TransactionTypeLBString) And (sTransList = sMasterTransList) Then

            If TransactionTypeLBString.ToUpper.Contains(tx_types_DropDownList.Items(itemCount).Value.ToUpper) Then
              tx_types_DropDownList.Items(itemCount).Selected = True
            End If

          End If

          itemCount += 1

        Next

      Else

        scp_tr_3.Visible = False

      End If ' if transaction summary

      time_scale_DropDownList.SelectedValue = Session.Item("marketTimeScale").ToString.ToLower

      ' DISPLAY TIMESCALE INCREMENT OPTIONS

      itemCount = 0
      range_span_DropDownList.Items.Clear()
      For nTempScale As Integer = 1 To nEndScale

        range_span_DropDownList.Items.Add(New ListItem(nTempScale.ToString, nTempScale.ToString))

        If nTempScale = Session("marketScaleSets") Then
          range_span_DropDownList.Items(itemCount).Selected = True
        End If

        itemCount += 1
      Next

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in summary_control_panel()" + ex.Message
    End Try

  End Sub

  Private Function build_available_summary() As String

    Dim SqlException As System.Data.SqlClient.SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand

    Dim sqlDT_available_summary As New DataTable
    Dim sqlDR_available_summary As System.Data.SqlClient.SqlDataReader = Nothing

    Dim htmlOut = New StringBuilder()
    Dim sQuery As New StringBuilder()

    Dim sColSpan = CStr(nScaleSets * 3 - 1 + 3)

    Dim sReportColSpan = CStr(nScaleSets * 2 + 1)

    Dim sAvailHeaderString As String = ""

    Dim YearMonth As String = ""
    Dim sYearMonthValue As String = ""

    Dim summaryMonth As Integer = 0
    Dim summaryYear As Integer = 0

    Dim columnSetMonth As Integer = 0
    Dim columnSetYear As Integer = 0
    Dim columnQuarterMonth As String = ""

    Try

      SqlConn.ConnectionString = Session.Item("jetnetClientDatabase").ToString.Trim

      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text     '
      SqlCommand.CommandTimeout = 60

      ' make the available summaries
      sQuery.Append("SELECT amod_make_name, amod_model_name, Aircraft_Model_Trend.*,  AC14.ac14_forsale_avg_dom as AVGDOM ")
      sQuery.Append(" FROM Aircraft_Model_Trend WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) on mtrend_amod_id = amod_id")
      sQuery.Append(" LEFT OUTER JOIN star_reports.dbo.Aircraft_14 AS AC14 ON AC14.ac14_amod_id = mtrend_amod_id AND YEAR(AC14.ac14_start_date) = mtrend_year AND MONTH(AC14.ac14_start_date) = mtrend_month")
      sQuery.Append(" WHERE amod_id = mtrend_amod_id")

      If Not String.IsNullOrEmpty(AirframeTypeString.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_airframe_type_code IN (" + AirframeTypeString.Trim + ")")
      End If

      If Not String.IsNullOrEmpty(TypeString.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_type_code IN (" + TypeString.Trim + ")")
      End If

      If Not String.IsNullOrEmpty(MakeString.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_make_name IN (" + MakeString.Trim + ")")
      End If

      If nMarketModelID > -1 Then
        sQuery.Append(Constants.cAndClause + "amod_id = " + nMarketModelID.ToString)
      ElseIf Not String.IsNullOrEmpty(ModelsString.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_id IN (" + ModelsString.Trim + ")")
      End If

      ' now add weight class
      If Not WeightClassStr.ToUpper.Contains("ALL") Then

        If Not String.IsNullOrEmpty(WeightClassStr.Trim) Then
          If WeightClassStr.Contains(Constants.cCommaDelim) Then
            sQuery.Append(Constants.cAndClause + "amod_weight_class IN ('" + WeightClassStr.Replace(Constants.cCommaDelim, Constants.cValueSeperator).Trim + "')")
          Else
            sQuery.Append(Constants.cAndClause + "amod_weight_class = '" + WeightClassStr.Trim + "'")
          End If
        End If

      End If

      ' now add Mfr Names
      If Not ManufacturerStr.ToUpper.Contains("ALL") Then

        If Not String.IsNullOrEmpty(ManufacturerStr.Trim) Then

          If ManufacturerStr.Contains(Constants.cValueSeperator) Then
            sQuery.Append(Constants.cAndClause + "amod_manufacturer_common_name IN ('" + ManufacturerStr.Trim + "')")
          Else
            sQuery.Append(Constants.cAndClause + "amod_manufacturer_common_name = '" + ManufacturerStr.Trim + "'")
          End If

        End If

      End If

      ' now add ac sizes
      If Not AcSizeStr.ToUpper.Contains("ALL") Then

        If Not String.IsNullOrEmpty(AcSizeStr.Trim) Then

          If AcSizeStr.Contains(Constants.cValueSeperator) Then
            sQuery.Append(Constants.cAndClause + "amod_jniq_size IN ('" + AcSizeStr.Trim + "')")
          Else
            sQuery.Append(Constants.cAndClause + "amod_jniq_size = '" + AcSizeStr.Trim + "'")
          End If

        End If

      End If

      If Not bHasHelicopterFilter And Not bHasBusinessFilter And Not bHasCommercialFilter Then
        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True))
      Else
        sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(bHasHelicopterFilter, bHasBusinessFilter, bHasCommercialFilter, False, False, False, True))
      End If

      ' PASSED CRITERIA
      If Session.Item("marketTimeScale").ToString.ToLower.Contains("months") Then

        If Not String.IsNullOrEmpty(Session.Item("marketStartDate").ToString.Trim) And Not String.IsNullOrEmpty(Session.Item("marketEndDate").ToString.Trim) Then

          If Year(CDate(Session.Item("marketStartDate").ToString)) = Year(CDate(Session.Item("marketEndDate").ToString)) Then

            sQuery.Append(Constants.cAndClause + "(((mtrend_year = year(CONVERT(DATETIME, '" + Session.Item("marketStartDate").ToString.Trim + "',102)))")
            sQuery.Append(Constants.cAndClause + "(mtrend_month >= month(CONVERT(DATETIME, '" + Session.Item("marketStartDate").ToString.Trim + "',102))))")
            sQuery.Append(Constants.cAndClause + "((mtrend_year = year(CONVERT(DATETIME, '" + Session.Item("marketEndDate").ToString.Trim + "',102)))")
            sQuery.Append(Constants.cAndClause + "(mtrend_month <= month(CONVERT(DATETIME, '" + Session.Item("marketEndDate").ToString.Trim + "',102)))))")

          Else

            sQuery.Append(Constants.cAndClause + "(((mtrend_year = year(CONVERT(DATETIME, '" + Session.Item("marketStartDate").ToString.Trim + "',102)))")
            sQuery.Append(Constants.cAndClause + "(mtrend_month >= month(CONVERT(DATETIME, '" + Session.Item("marketStartDate").ToString.Trim + "',102))))")
            sQuery.Append(Constants.cOrClause + "((mtrend_year = year(CONVERT(DATETIME, '" + Session.Item("marketEndDate").ToString.Trim + "',102)))")
            sQuery.Append(Constants.cAndClause + "(mtrend_month <= month(CONVERT(DATETIME, '" + Session.Item("marketEndDate").ToString.Trim + "',102)))))")

          End If

        End If

      End If

      If Session.Item("marketTimeScale").ToString.ToLower.Contains("years") Or Session.Item("marketTimeScale").ToString.ToLower.Contains("quarters") Then

        If Not String.IsNullOrEmpty(Session.Item("marketStartDate").ToString.Trim) Then
          sQuery.Append(Constants.cAndClause + "mtrend_year >= year(CONVERT(DATETIME, '" + Session.Item("marketStartDate").ToString.Trim + "',102))")
        End If

        If Not String.IsNullOrEmpty(Session.Item("marketEndDate").ToString.Trim) Then
          sQuery.Append(Constants.cAndClause + "mtrend_year <= year(CONVERT(DATETIME, '" + Session.Item("marketEndDate").ToString.Trim + "',102))")
        End If

      End If

      If Not bHasHelicopterFilter And Not bHasBusinessFilter And Not bHasCommercialFilter Then
        sQuery.Append(" " + commonEvo.MakeMarketProductCodeClause(HttpContext.Current.Session.Item("localPreferences"), False))
      Else
        sQuery.Append(" " + commonEvo.BuildMarketProductCodeCheckWhereClause(bHasHelicopterFilter, bHasBusinessFilter, bHasCommercialFilter, False, False, False))
      End If

      sQuery.Append(" ORDER BY mtrend_year, mtrend_month")

      If Not bPreviousSummary Then
        HttpContext.Current.Session.Item("MasterMarketAvailableSummary") = sQuery.ToString
      Else
        sQuery = Nothing
        sQuery = New StringBuilder
        sQuery.Append(HttpContext.Current.Session.Item("MasterMarketAvailableSummary").ToString)
      End If

      HttpContext.Current.Session.Item("MasterMarketSummary") = sQuery.ToString

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>build_available_summary()</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      sqlDR_available_summary = SqlCommand.ExecuteReader()

      Try
        sqlDT_available_summary.Load(sqlDR_available_summary)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = sqlDT_available_summary.GetErrors()
      End Try

      sqlDR_available_summary.Close()
      sqlDR_available_summary.Dispose()

      If sqlDT_available_summary.Rows.Count > 0 Then

        ' div tag for available summary report
        htmlOut.Append(localFunctions.WriteLineToBoth("<div style='text-align:center;'>", marketFile, marketFile_wHeader))

        'If Session("userBrowserType") = "saf" Then
        '  htmlOut.Append("<div style='width: 99%; text-align:center; overflow-x: scroll;'>")
        'Else
        '  htmlOut.Append("<div style='width: 99%; text-align:center; overflow-x: auto;'>")
        'End If

        htmlOut.Append("<div style=""width: 100%; overflow: auto; vertical-align: top;"">")

        nTotalRecords = sqlDT_available_summary.Rows.Count

        htmlOut.Append(localFunctions.WriteLineToBoth("<table class='data_aircraft_grid' border='1' cellpadding='2' cellspacing='0' bordercolor='#949494' width='100%'>", marketFile, marketFile_wHeader))

        If Not isHeliOnlyProduct Then
          localFunctions.WriteLineToFile("<tr><td colspan='" + sReportColSpan + "' align='center' valign='middle'><font size='2.5'><b>Market Summary (Aircraft Available For Sale)", marketFile, marketFile_wHeader)
          htmlOut.Append("<tr class=""header_row""><td colspan='" + sColSpan + "' align='center' valign='middle'><font size='2.5'><b>Market Summary (Aircraft Available For Sale)")
        Else
          localFunctions.WriteLineToFile("<tr><td colspan='" + sReportColSpan + "' align='center' valign='middle'><font size='2.5'><b>Market Summary (Helicopters Available For Sale)", marketFile, marketFile_wHeader)
          htmlOut.Append("<tr class=""header_row""><td colspan='" + sColSpan + "' align='center' valign='middle'><font size='2.5'><b>Market Summary (Helicopters Available For Sale)")
        End If

        htmlOut.Append(localFunctions.WriteLineToBoth("</b></font></td></tr><tr class=""header_row"">", marketFile, marketFile_wHeader))

        If Session.Item("marketTimeScale").ToString.ToLower.Contains("months") Then
          htmlOut.Append(localFunctions.WriteLineToBoth("<td>&nbsp;</td>", marketFile, marketFile_wHeader))
        ElseIf Session.Item("marketTimeScale").ToString.ToLower.Contains("years") Then
          htmlOut.Append(localFunctions.WriteLineToBoth("<td>AVERAGES ARE YEARLY - Over Time Period</td>", marketFile, marketFile_wHeader))
        ElseIf Session.Item("marketTimeScale").ToString.ToLower.Contains("quarters") Then
          htmlOut.Append(localFunctions.WriteLineToBoth("<td>AVERAGES ARE QUARTERLY - Over Time Period</td>", marketFile, marketFile_wHeader))
        Else
          htmlOut.Append(localFunctions.WriteLineToBoth("<td>AVERAGES ARE MONTHLY - Over Time Period</td>", marketFile, marketFile_wHeader))
        End If

        ' SET UP TIMESCALE HEADERS FOR AVAILABLE SUMMARIES
        Dim tmpHtmlOut As String = ""
        sAvailHeaderString = localFunctions.generate_timescale_headers(tmpHtmlOut, CDate(Session.Item("marketStartDate").ToString), CDate(Session.Item("marketEndDate").ToString), Session.Item("marketTimeScale"), False)

        htmlOut.Append(tmpHtmlOut)

        ' CREATE ARRAYS FOR HEADER AND EACH TRANSACTION TYPE
        Dim ColumnSet() As String = sAvailHeaderString.Split(Constants.cCommaDelim)

        Dim nArrayLength As Integer = ColumnSet.Length - 1

        Dim AV_For_Sale(nArrayLength) As Double
        Dim AV_For_Sale_Count(nArrayLength) As Double

        Dim AV_In_Operation_Fleet(nArrayLength) As Double
        Dim AV_In_Operation_Fleet_Count(nArrayLength) As Double

        Dim AV_In_Operation_Fleet_For_Sale(nArrayLength) As Double
        Dim AV_In_Operation_Fleet_For_Sale_Count(nArrayLength) As Double

        Dim AV_End_User(nArrayLength) As Double
        Dim AV_End_User_Count(nArrayLength) As Double

        Dim AV_End_User_Exc(nArrayLength) As Double
        Dim AV_End_User_Exc_Count(nArrayLength) As Double

        Dim AV_Dealer(nArrayLength) As Double
        Dim AV_Dealer_Count(nArrayLength) As Double

        Dim AV_Domestic(nArrayLength) As Double
        Dim AV_Domestic_Count(nArrayLength) As Double

        Dim AV_International(nArrayLength) As Double
        Dim AV_International_Count(nArrayLength) As Double

        Dim AV_Asking_Price_Total(nArrayLength) As Double
        Dim AV_Asking_Price_Count(nArrayLength) As Double

        Dim AV_Asking_High(nArrayLength) As Double
        Dim AV_Asking_Low(nArrayLength) As Double

        Dim AV_Asking_Make_Offer(nArrayLength) As Double
        Dim AV_Asking_Make_Offer_Count(nArrayLength) As Double

        Dim AV_Avg_Year_Total(nArrayLength) As Double
        Dim AV_Avg_Year_Count(nArrayLength) As Double

        Dim AV_Avg_Airframe_TT_Total(nArrayLength) As Double
        Dim AV_Avg_Airframe_TT_Count(nArrayLength) As Double

        Dim AV_Avg_Engine_TT_Total(nArrayLength) As Double
        Dim AV_Avg_Engine_TT_Count(nArrayLength) As Double

        Dim AV_New_To_Market(nArrayLength) As Double
        Dim AV_New_To_Market_Count(nArrayLength) As Double

        Dim AV_DOM(nArrayLength) As Double
        Dim AV_DOM_COUNT(nArrayLength) As Double

        Dim YearMonth_Count(nArrayLength) As Double

        ' not used at this time
        Dim AV_Delivery_Position(nArrayLength) As Double
        Dim AV_Lease(nArrayLength) As Double
        Dim AV_Fractional(nArrayLength) As Double

        For iLoop As Integer = 0 To UBound(ColumnSet)
          AV_For_Sale(iLoop) = 0
          AV_For_Sale_Count(iLoop) = 0
          AV_In_Operation_Fleet(iLoop) = 0
          AV_In_Operation_Fleet_Count(iLoop) = 0
          AV_In_Operation_Fleet_For_Sale(iLoop) = 0
          AV_In_Operation_Fleet_For_Sale_Count(iLoop) = 0
          AV_End_User(iLoop) = 0
          AV_End_User_Count(iLoop) = 0
          AV_End_User_Exc(iLoop) = 0
          AV_End_User_Exc_Count(iLoop) = 0
          AV_Dealer(iLoop) = 0
          AV_Dealer_Count(iLoop) = 0
          AV_Domestic(iLoop) = 0
          AV_Domestic_Count(iLoop) = 0
          AV_International(iLoop) = 0
          AV_International_Count(iLoop) = 0
          AV_Asking_Price_Total(iLoop) = 0
          AV_Asking_Price_Count(iLoop) = 0
          AV_Asking_High(iLoop) = 0
          AV_Asking_Low(iLoop) = 0
          AV_Asking_Make_Offer(iLoop) = 0
          AV_Asking_Make_Offer_Count(iLoop) = 0
          AV_Avg_Year_Total(iLoop) = 0
          AV_Avg_Year_Count(iLoop) = 0
          AV_Avg_Airframe_TT_Total(iLoop) = 0
          AV_Avg_Airframe_TT_Count(iLoop) = 0
          AV_Avg_Engine_TT_Total(iLoop) = 0
          AV_Avg_Engine_TT_Count(iLoop) = 0
          AV_New_To_Market(iLoop) = 0
          AV_New_To_Market_Count(iLoop) = 0
          YearMonth_Count(iLoop) = 0
          AV_DOM(iLoop) = 0
          AV_DOM_COUNT(iLoop) = 0

          ' not used at this time
          AV_Delivery_Position(iLoop) = 0
          AV_Lease(iLoop) = 0
          AV_Fractional(iLoop) = 0

        Next

        htmlOut.Append(localFunctions.WriteLineToBoth("<td align='center' valign='middle'>AVERAGE</td>", marketFile, marketFile_wHeader))

        For Each Row As DataRow In sqlDT_available_summary.Rows

          summaryMonth = CInt(Row.Item("mtrend_month").ToString)
          summaryYear = CInt(Row.Item("mtrend_year").ToString)
          sYearMonthValue = Trim(summaryYear.ToString + "-" + summaryMonth.ToString)

          For iLoop As Integer = 0 To UBound(ColumnSet)

            Select Case sTimeScale.ToLower
              Case "years"
                columnSetMonth = CInt(1)
                columnSetYear = CInt(Right(ColumnSet(iLoop), 4))
              Case "quarters"
                columnQuarterMonth = Left(ColumnSet(iLoop), InStr(1, ColumnSet(iLoop), "/") - 1)
                columnSetYear = CInt(Right(ColumnSet(iLoop), 4))
              Case Else
                columnSetMonth = CInt(Left(ColumnSet(iLoop), InStr(1, ColumnSet(iLoop), "/") - 1))
                columnSetYear = CInt(Right(ColumnSet(iLoop), 4))
            End Select

            Select Case sTimeScale.ToLower
              Case "years"
                If summaryYear = columnSetYear Then

                  If YearMonth <> sYearMonthValue Then
                    YearMonth = sYearMonthValue
                    YearMonth_Count(iLoop) += 1
                  End If

                  localFunctions.Store_Available_Totals(Row, iLoop, AV_For_Sale, AV_For_Sale_Count, AV_In_Operation_Fleet, AV_In_Operation_Fleet_Count, AV_In_Operation_Fleet_For_Sale, AV_In_Operation_Fleet_For_Sale_Count,
                                                        AV_End_User, AV_End_User_Count, AV_End_User_Exc, AV_End_User_Exc_Count, AV_Dealer, AV_Dealer_Count, AV_Domestic, AV_Domestic_Count, AV_International, AV_International_Count,
                                                        AV_Asking_Price_Total, AV_Asking_Price_Count, AV_Asking_High, AV_Asking_Low, AV_Asking_Make_Offer, AV_Asking_Make_Offer_Count, AV_Avg_Year_Total, AV_Avg_Year_Count,
                                                        AV_Avg_Airframe_TT_Total, AV_Avg_Airframe_TT_Count, AV_Avg_Engine_TT_Total, AV_Avg_Engine_TT_Count, AV_New_To_Market, AV_New_To_Market_Count, AV_Delivery_Position, AV_Lease, AV_Fractional, AV_DOM, AV_DOM_COUNT)

                End If

              Case "quarters"
                If summaryYear = columnSetYear Then
                  If localFunctions.Get_Quarter_For_Month_Server(summaryMonth) = columnQuarterMonth Then

                    If YearMonth <> sYearMonthValue Then
                      YearMonth = sYearMonthValue
                      YearMonth_Count(iLoop) += 1
                    End If

                    localFunctions.Store_Available_Totals(Row, iLoop, AV_For_Sale, AV_For_Sale_Count, AV_In_Operation_Fleet, AV_In_Operation_Fleet_Count, AV_In_Operation_Fleet_For_Sale, AV_In_Operation_Fleet_For_Sale_Count,
                                                          AV_End_User, AV_End_User_Count, AV_End_User_Exc, AV_End_User_Exc_Count, AV_Dealer, AV_Dealer_Count, AV_Domestic, AV_Domestic_Count, AV_International, AV_International_Count,
                                                          AV_Asking_Price_Total, AV_Asking_Price_Count, AV_Asking_High, AV_Asking_Low, AV_Asking_Make_Offer, AV_Asking_Make_Offer_Count, AV_Avg_Year_Total, AV_Avg_Year_Count,
                                                          AV_Avg_Airframe_TT_Total, AV_Avg_Airframe_TT_Count, AV_Avg_Engine_TT_Total, AV_Avg_Engine_TT_Count, AV_New_To_Market, AV_New_To_Market_Count, AV_Delivery_Position, AV_Lease, AV_Fractional, AV_DOM, AV_DOM_COUNT)

                  End If
                End If

              Case "months"
                If summaryYear = columnSetYear Then
                  If summaryMonth = columnSetMonth Then

                    If YearMonth <> sYearMonthValue Then
                      YearMonth = sYearMonthValue
                      YearMonth_Count(iLoop) += 1
                    End If

                    localFunctions.Store_Available_Totals(Row, iLoop, AV_For_Sale, AV_For_Sale_Count, AV_In_Operation_Fleet, AV_In_Operation_Fleet_Count, AV_In_Operation_Fleet_For_Sale, AV_In_Operation_Fleet_For_Sale_Count,
                                                          AV_End_User, AV_End_User_Count, AV_End_User_Exc, AV_End_User_Exc_Count, AV_Dealer, AV_Dealer_Count, AV_Domestic, AV_Domestic_Count, AV_International, AV_International_Count,
                                                          AV_Asking_Price_Total, AV_Asking_Price_Count, AV_Asking_High, AV_Asking_Low, AV_Asking_Make_Offer, AV_Asking_Make_Offer_Count, AV_Avg_Year_Total, AV_Avg_Year_Count,
                                                          AV_Avg_Airframe_TT_Total, AV_Avg_Airframe_TT_Count, AV_Avg_Engine_TT_Total, AV_Avg_Engine_TT_Count, AV_New_To_Market, AV_New_To_Market_Count, AV_Delivery_Position, AV_Lease, AV_Fractional, AV_DOM, AV_DOM_COUNT)

                  End If
                End If

            End Select

          Next ' iLoop

        Next

        htmlOut.Append(localFunctions.Print_Available_Summaries(CDate(Session("marketStartDate").ToString), CDate(Session("marketEndDate").ToString), sAvailHeaderString, sColSpan, ColumnSet, YearMonth_Count, AV_For_Sale, AV_For_Sale_Count, AV_In_Operation_Fleet, AV_In_Operation_Fleet_Count, AV_In_Operation_Fleet_For_Sale, AV_In_Operation_Fleet_For_Sale_Count, AV_End_User, AV_End_User_Count, AV_End_User_Exc, AV_End_User_Exc_Count, AV_Dealer, AV_Dealer_Count, AV_Domestic, AV_Domestic_Count, AV_International, AV_International_Count, AV_Asking_Price_Total, AV_Asking_Price_Count, AV_Asking_High, AV_Asking_Low, AV_Asking_Make_Offer, AV_Asking_Make_Offer_Count, AV_Avg_Year_Total, AV_Avg_Year_Count, AV_Avg_Airframe_TT_Total, AV_Avg_Airframe_TT_Count, AV_Avg_Engine_TT_Total, AV_Avg_Engine_TT_Count, AV_New_To_Market, AV_New_To_Market_Count, AV_Delivery_Position, AV_Lease, AV_Fractional, AV_DOM, AV_DOM_COUNT))

        Dim tmpString = Session.Item("marketTimeScale").ToString.Replace("s", Constants.cEmptyString)

        localFunctions.WriteLineToFile("<tr><td colspan='" + sReportColSpan + "' align='center' valign='middle'><font size='1.5'>All figures calculated at end of " + tmpString + " (%Chg)=Percent change from previous " + tmpString + ".</font></td></tr>", marketFile, marketFile_wHeader)
        localFunctions.WriteLineToFile("<tr><td colspan='" + sReportColSpan + "' align='center' valign='middle'><font size='1.5'>NYA=Not Yet Available, NC=Not Calculated.", marketFile, marketFile_wHeader)

        htmlOut.Append("<tr class=""header_row""><td colspan='" + sColSpan + "' align='center' valign='middle'><font size='1.5'>All figures calculated at end of " + tmpString + " (%Chg)=Percent change from previous " + tmpString + ".</font></td></tr>")
        htmlOut.Append("<tr class=""header_row""><td colspan='" + sColSpan + "' align='center' valign='middle'><font size='1.5'>NYA=Not Yet Available, NC=Not Calculated.")

        htmlOut.Append(localFunctions.WriteLineToBoth("&nbsp; Average column includes total for final " + tmpString + " since all data was reported.</font></th></tr></table>", marketFile, marketFile_wHeader))

        htmlOut.Append("</div>")

        ' div tag for available summary report
        htmlOut.Append(localFunctions.WriteLineToBoth("</div>", marketFile, marketFile_wHeader))
        localFunctions.WriteLineToFile("<br/>", marketFile, marketFile_wHeader)

        htmlOut.Append("<hr width='100%' size='1' style='margin:10px 0px 10px 0px;'>")

      End If

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in build_available_summary()" + ex.Message
    Finally

      sqlDT_available_summary.Dispose()
      sqlDT_available_summary = Nothing

      SqlConn.Close()
      SqlConn.Dispose()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing

    End Try

    Return htmlOut.ToString

  End Function

  Private Function build_transaction_summary() As String

    Dim SqlException As System.Data.SqlClient.SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand

    Dim sqlDT_trans_summary As New DataTable
    Dim sqlDR_trans_summary As System.Data.SqlClient.SqlDataReader = Nothing

    Dim htmlOut = New StringBuilder()
    Dim sQuery As New StringBuilder()

    Dim sColSpan = CStr(nScaleSets * 3 - 1 + 3)
    Dim sReportColSpan = CStr(nScaleSets * 2 + 1)

    Dim columnSetMonth As Integer = 0
    Dim columnSetYear As Integer = 0
    Dim columnQuarterMonth As String = ""

    Dim sTransHeaderString As String = ""

    Dim nTransFromTotal As Double = 0.0
    Dim nTransGroupTotal As Double = 0.0
    Dim nTransTotal As Double = 0.0
    Dim nTotalProcessed As Integer = 0

    Dim sCurrentTransType As String = ""
    Dim sPreviousTransType As String = ""

    Dim sTrans_Group As String = ""
    Dim sTrans_To As String = ""
    Dim sPrevious_Trans_Group As String = ""
    Dim sLast_From As String = ""

    Dim Source_Name As String = ""
    Dim Destination_Name As String = ""
    Dim sTransactionClass As String = ""

    Dim fAc_amod_id As Integer = 0
    Dim fAc_journ_id As Long = 0
    Dim fJourn_newac_flag As String = ""
    Dim fAc_list_date As String = ""
    Dim fAc_year As String = ""
    Dim fAc_mfr_year As String = ""
    Dim fJourn_date As Date = Nothing
    Dim fJourn_subcategory_code As String = ""
    Dim fJcat_subcategory_name As String = ""
    Dim fAmod_make_name As String = ""
    Dim fAmod_model_name As String = ""
    Dim fAc_asking As String = ""

    Dim TransClasses() As String = Nothing

    Dim sTransactionCode As String = ""
    Dim FilterString As String = ""
    Dim SortString As String = ""

    Dim localACInfo As New marketSummaryObjAircraft

    Try

      SqlConn.ConnectionString = Session.Item("jetnetClientDatabase").ToString.Trim

      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text     '
      SqlCommand.CommandTimeout = 60

      ' SELECT ALL THE TRANSACTIONS FOR THE TIMEFRAME SPECIFIED
      sQuery.Append("SELECT amod_id, ac_journ_id, journ_newac_flag, ac_list_date, ac_year, ac_mfr_year, journ_date, journ_subcategory_code, jcat_subcategory_name,")
      sQuery.Append(" amod_make_name, amod_model_name, ac_asking, ac_asking_price, ac_airframe_tot_hrs, ac_airframe_tot_landings,")
      sQuery.Append(" ac_product_business_flag, ac_product_helicopter_flag, ac_product_commercial_flag,")
      sQuery.Append(" LEFT(journ_subcategory_code,2) AS trans_cat,")
      sQuery.Append(" SUBSTRING(journ_subcategory_code,3,2) AS trans_source,")
      sQuery.Append(" RIGHT(journ_subcategory_code,2) AS trans_destination")

      'sQuery.Append(" FROM Aircraft WITH(NOLOCK), Aircraft_Model WITH(NOLOCK), Journal WITH(NOLOCK), Journal_Category WITH(NOLOCK)")
      'sQuery.Append(" WHERE ac_amod_id = amod_id AND ac_journ_id = journ_id AND journ_subcategory_code = jcat_subcategory_code")
      'sQuery.Append(" AND jcat_category_code <> 'MS' AND RIGHT(journ_subcategory_code, 4) <> 'CORR'") '

      sQuery.Append(" FROM View_Aircraft_History_Flat WITH(NOLOCK)")
      sQuery.Append(" WHERE jcat_category_code <> 'MS' AND RIGHT(journ_subcategory_code, 4) <> 'CORR'")

      If Not String.IsNullOrEmpty(AirframeTypeString.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_airframe_type_code IN (" + AirframeTypeString.Trim + ")")
      End If

      If Not String.IsNullOrEmpty(TypeString.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_type_code IN (" + TypeString.Trim + ")")
      End If

      If Not String.IsNullOrEmpty(MakeString.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_make_name IN (" + MakeString.Trim + ")")
      End If

      If nMarketModelID > -1 Then
        sQuery.Append(Constants.cAndClause + "amod_id = " + nMarketModelID.ToString)
      ElseIf Not String.IsNullOrEmpty(ModelsString.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_id IN (" + ModelsString.Trim + ")")
      End If

      localACInfo.ModelsString = ModelsString
      localACInfo.MakeString = MakeString
      localACInfo.TypeString = TypeString
      localACInfo.AirframeTypeString = AirframeTypeString
      localACInfo.CombinedAirframeTypeString = ""
      localACInfo.WeightString = WeightClassStr
      localACInfo.MfrNamesString = ManufacturerStr
      localACInfo.AcsizeString = AcSizeStr

      ' now add weight class
      If Not WeightClassStr.ToUpper.Contains("ALL") Then

        If Not String.IsNullOrEmpty(WeightClassStr.Trim) Then
          If WeightClassStr.Contains(Constants.cCommaDelim) Then
            sQuery.Append(Constants.cAndClause + "amod_weight_class IN ('" + WeightClassStr.Replace(Constants.cCommaDelim, Constants.cValueSeperator).Trim + "')")
          Else
            sQuery.Append(Constants.cAndClause + "amod_weight_class = '" + WeightClassStr.Trim + "'")
          End If
        End If

      End If

      ' now add Mfr Names
      If Not ManufacturerStr.ToUpper.Contains("ALL") Then

        If Not String.IsNullOrEmpty(ManufacturerStr.Trim) Then

          If ManufacturerStr.Contains(Constants.cValueSeperator) Then
            sQuery.Append(Constants.cAndClause + "amod_manufacturer_common_name IN ('" + ManufacturerStr.Trim + "')")
          Else
            sQuery.Append(Constants.cAndClause + "amod_manufacturer_common_name = '" + ManufacturerStr.Trim + "'")
          End If

        End If

      End If

      ' now add ac sizes
      If Not AcSizeStr.ToUpper.Contains("ALL") Then

        If Not String.IsNullOrEmpty(AcSizeStr.Trim) Then

          If AcSizeStr.Contains(Constants.cValueSeperator) Then
            sQuery.Append(Constants.cAndClause + "amod_jniq_size IN ('" + AcSizeStr.Trim + "')")
          Else
            sQuery.Append(Constants.cAndClause + "amod_jniq_size = '" + AcSizeStr.Trim + "'")
          End If

        End If

      End If

      HttpContext.Current.Session.Item("marketNewUsed") = ""

      ' new to market
      If chkNewToMarketID.Checked Then
        sQuery.Append(Constants.cAndClause + "journ_newac_flag = 'Y' AND journ_internal_trans_flag = 'N' AND journ_subcat_code_part2 NOT LIKE 'IT'")
        HttpContext.Current.Session.Item("marketNewUsed") = "new"
      End If

      If chkUsedMarketID.Checked Then
        sQuery.Append(Constants.cAndClause + "journ_newac_flag = 'N' AND journ_internal_trans_flag = 'N' AND journ_subcat_code_part2 NOT LIKE 'IT'")
        HttpContext.Current.Session.Item("marketNewUsed") = "used"
      End If

      If Not bHasHelicopterFilter And Not bHasBusinessFilter And Not bHasCommercialFilter Then
        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
      Else
        sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(bHasHelicopterFilter, bHasBusinessFilter, bHasCommercialFilter, False, False, False, False))
      End If

      sQuery.Append(Constants.cAndClause + "(jcat_category_code = 'AH')")
      sQuery.Append(Constants.cAndClause + "(journ_subcategory_code <> 'FSPEND')")
      sQuery.Append(Constants.cAndClause + "(journ_subcategory_code <> 'BIS')")
      sQuery.Append(Constants.cAndClause + "(journ_subcategory_code <> 'CNAME')")
      sQuery.Append(Constants.cAndClause + "(journ_subcategory_code <> 'ACDOC')")

      If sTransList.ToUpper <> sMasterTransList Then

        sQuery.Append(Constants.cAndClause + "(journ_subcategory_code LIKE '" + sTransList.ToUpper.Trim + "%')")

      Else

        If Not String.IsNullOrEmpty(TransactionTypeLBString.Trim) Then
          sQuery.Append(Constants.cAndClause + "(journ_subcat_code_part1 IN (" + TransactionTypeLBString.Replace(Constants.cMultiDelim, Constants.cValueSeperator).ToUpper.Trim + "))")
        End If

        If Not transaction_from.SelectedValue.ToString.Contains("not") Then
          If Not String.IsNullOrEmpty(TransactionFromLBString.Trim) Then
            sQuery.Append(Constants.cAndClause + "(journ_subcat_code_part2 IN (" + TransactionFromLBString.ToUpper.Trim + "))")
          End If
        Else
          If Not String.IsNullOrEmpty(TransactionFromLBString.Trim) Then
            sQuery.Append(Constants.cAndClause + Constants.cNot + "(journ_subcat_code_part2 IN (" + TransactionFromLBString.ToUpper.Trim + "))")
          End If
        End If

        If Not transaction_to.SelectedValue.ToString.Contains("not") Then
          If Not String.IsNullOrEmpty(TransactionToLBString.Trim) Then
            sQuery.Append(Constants.cAndClause + "(journ_subcat_code_part3 IN (" + TransactionToLBString.ToUpper.Trim + "))")
          End If
        Else
          If Not String.IsNullOrEmpty(TransactionToLBString.Trim) Then
            sQuery.Append(Constants.cAndClause + Constants.cNot + "(journ_subcat_code_part3 IN (" + TransactionToLBString.ToUpper.Trim + "))")
          End If
        End If

      End If

      If Not String.IsNullOrEmpty(Session.Item("marketStartDate").ToString.Trim) Then
        sQuery.Append(Constants.cAndClause + "(journ_date >= CONVERT(DATETIME, '" + Session.Item("marketStartDate").ToString.Trim + "',102))")
      End If

      If Not String.IsNullOrEmpty(Session.Item("marketEndDate").ToString.Trim) Then
        sQuery.Append(Constants.cAndClause + "(journ_date < CONVERT(DATETIME, '" + Session.Item("marketEndDate").ToString.Trim + "',102))")
      End If

      If Session.Item("marketSumType").ToString.Trim.ToLower.Contains("trans_destination") Then
        sQuery.Append(" ORDER BY trans_cat, trans_destination, journ_date")
      Else
        sQuery.Append(" ORDER BY trans_source, jcat_subcategory_name, journ_subcategory_code, journ_date")
      End If

      If Not bPreviousSummary Then
        HttpContext.Current.Session.Item("MasterMarketTransactionSummary") = sQuery.ToString
      Else
        sQuery = Nothing
        sQuery = New StringBuilder
        sQuery.Append(HttpContext.Current.Session.Item("MasterMarketTransactionSummary").ToString)
      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>build_transaction_summary()</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      sqlDR_trans_summary = SqlCommand.ExecuteReader()

      Try
        sqlDT_trans_summary.Load(sqlDR_trans_summary)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = sqlDT_trans_summary.GetErrors()
      End Try

      sqlDR_trans_summary.Close()
      sqlDR_trans_summary.Dispose()

      If sqlDT_trans_summary.Rows.Count > 0 Then

        ' div tag for transaction summary report
        htmlOut.Append(localFunctions.WriteLineToBoth("<div style='text-align:center;'>", marketFile, marketFile_wHeader))

        htmlOut.Append("<div style=""width: 100%; overflow: auto; vertical-align: top;"">")

        If Not String.IsNullOrEmpty(TransactionTypeLBString.Trim) Then
          TransClasses = Split(TransactionTypeLBString.Replace(Constants.cSingleQuote, Constants.cEmptyString), Constants.cCommaDelim)
        Else
          TransClasses = Split(sTransList, Constants.cCommaDelim)
        End If

        ' create arrays for header and each transaction type
        Dim ColumnSet() As String = Nothing

        Dim nArrayLength As Integer = 0

        Dim TransValues() As Double = Nothing
        Dim GroupValues() As Double = Nothing
        Dim AskingAvgValues() As Double = Nothing
        Dim AskingTotValues() As Double = Nothing
        Dim AskingHighValues() As Double = Nothing
        Dim AskingLowValues() As Double = Nothing
        Dim MakeOffValues() As Double = Nothing
        Dim YearAvgValues() As Long = Nothing
        Dim DaysOnTotValues() As Double = Nothing
        Dim DaysOnAvgValues() As Double = Nothing
        Dim OffMarketValues() As Double = Nothing
        Dim OnMarketValues() As Double = Nothing
        Dim WithdrawnValues() As Double = Nothing
        Dim SectionValues() As Double = Nothing
        Dim ITValues() As Double = Nothing
        Dim NewToMktValues() As Double = Nothing

        Dim afiltered_Transactions As DataRow() = Nothing

        For ClassNum As Integer = 0 To UBound(TransClasses)

          sTransactionCode = TransClasses(ClassNum).ToLower.Trim

          If Not sTransactionCode.Substring(0, 1).ToUpper.Contains("L") Then
            sTransactionClass = localFunctions.GetTransTypeName(sTransactionCode)
          Else
            sTransactionClass = localFunctions.Get_Lease_Type(sTransactionCode)
          End If

          Select Case sTransactionCode.ToUpper.Trim
            Case "WS"
              FilterString = "journ_subcategory_code LIKE 'WS%'"
              Source_Name = "Seller"
              Destination_Name = "Purchaser"

            Case "WO"
              FilterString = "(journ_subcategory_code LIKE 'WU%' OR journ_subcategory_code LIKE 'WO%' OR journ_subcategory_code LIKE 'WF%')"
              Source_Name = sTimeScale + " >>"
              Destination_Name = " - "

            Case "OM"
              FilterString = "journ_subcategory_code LIKE 'OM%'"
              Source_Name = sTimeScale + " >>"
              Destination_Name = " - "

            Case "MA"
              FilterString = "journ_subcategory_code LIKE 'MA%'"
              Source_Name = sTimeScale + " >>"
              Destination_Name = " - "

            Case "DP"
              FilterString = "journ_subcategory_code LIKE 'DP%'"
              Source_Name = "Seller"
              Destination_Name = "Purchaser"

            Case "FS"
              FilterString = "journ_subcategory_code LIKE 'FS%'"
              Source_Name = "Seller"
              Destination_Name = "Purchaser"

            Case "SS"
              FilterString = "journ_subcategory_code LIKE 'SS%'"
              Source_Name = "Seller"
              Destination_Name = "Purchaser"

            Case "FC"
              FilterString = "journ_subcategory_code LIKE 'FC%'"
              Source_Name = "Previous Owner"
              Destination_Name = "Foreclosed By"

            Case "L", "LA", "LX", "LN", "LO", "LS", "LT"
              FilterString = "journ_subcategory_code LIKE '" + sTransactionCode + "%'"
              Source_Name = "Lessor"
              Destination_Name = "Lessee"

            Case "SZ"
              FilterString = "journ_subcategory_code LIKE 'SZ%'"
              Source_Name = "Previous Owner"
              Destination_Name = "Seized By"

          End Select

          If Session.Item("marketSumType").ToString.Trim.ToLower.Contains("trans_destination") Then
            SortString = "trans_cat, trans_destination, journ_date"
          Else
            SortString = "trans_source, jcat_subcategory_name, journ_subcategory_code, journ_date"
          End If

          ' CREATE NEW TABLE EACH TRANSACTION CLASSIFICATION
          htmlOut.Append(localFunctions.WriteLineToBoth("<table class='data_aircraft_grid' border='1' cellpadding='2' cellspacing='0' bordercolor='#949494' width='100%'>", marketFile, marketFile_wHeader))

          If Not String.IsNullOrEmpty(sTransactionClass) Then
            localFunctions.WriteLineToFile("<tr><td colspan=""" + sReportColSpan + """ align=""center"" valign=""middle""><font size=""2.5""><b>Transaction Summary " + sTransactionClass, marketFile, marketFile_wHeader)
            htmlOut.Append("<tr class=""header_row""><td colspan=""" + sColSpan + """ align=""center"" valign=""middle""><font size=""2.5""><b>Transaction Summary " + sTransactionClass)
          Else
            localFunctions.WriteLineToFile("<tr><td colspan=""" + sReportColSpan + """ align=""center"" valign=""middle""><font size=""2.5""><b>Transaction Summary", marketFile, marketFile_wHeader)
            htmlOut.Append("<tr class=""header_row""><td colspan=""" + sColSpan + """ align=""center"" valign=""middle""><font size=""2.5""><b>Transaction Summary")
          End If

          htmlOut.Append(localFunctions.WriteLineToBoth("</b></font></td></tr>", marketFile, marketFile_wHeader))

          If Session.Item("marketSumType").ToString.Trim.ToLower.Contains("trans_source") Then
            htmlOut.Append(localFunctions.WriteLineToBoth("<tr class=""header_row""><td align=""left"" valign=""middle"" nowrap=""nowrap"">" + Source_Name + "</td>", marketFile, marketFile_wHeader))
          End If

          If Session.Item("marketSumType").ToString.Trim.ToLower.Contains("trans_destination") Then
            If sTransactionCode.ToLower = "ma" Or sTransactionCode.ToLower = "om" Or sTransactionCode.ToLower = "wo" Then
              htmlOut.Append(localFunctions.WriteLineToBoth("<tr class=""header_row""><td align=""left"" valign=""middle"" nowrap=""nowrap"">" + Source_Name + "</td>", marketFile, marketFile_wHeader))
            Else
              htmlOut.Append(localFunctions.WriteLineToBoth("<tr class=""header_row""><td align=""left"" valign=""middle"" nowrap=""nowrap"">" + Destination_Name + "</td>", marketFile, marketFile_wHeader))
            End If
          End If

          If Session.Item("marketSumType").ToString.Trim.ToLower.Contains("trans_type") Then
            If sTransactionCode.ToLower = "ma" Or sTransactionCode.ToLower = "om" Or sTransactionCode.ToLower = "wo" Then
              htmlOut.Append(localFunctions.WriteLineToBoth("<tr class=""header_row""><td align=""left"" valign=""middle"" nowrap=""nowrap"">" + Source_Name + "</td>", marketFile, marketFile_wHeader))
            Else
              htmlOut.Append(localFunctions.WriteLineToBoth("<tr class=""header_row""><td align=""left"" valign=""middle"" nowrap=""nowrap"">" + Source_Name + "</td><td>" + Destination_Name + "</td>", marketFile, marketFile_wHeader))
            End If
          End If

          ' set up timescale headers for transaction summary
          Dim tmpHtmlOut As String = ""
          sTransHeaderString = localFunctions.generate_timescale_headers(tmpHtmlOut, CDate(Session.Item("marketStartDate").ToString), CDate(Session.Item("marketEndDate").ToString), Session.Item("marketTimeScale"), True, False)

          htmlOut.Append(tmpHtmlOut)

          ColumnSet = sTransHeaderString.Split(Constants.cCommaDelim)

          nArrayLength = ColumnSet.Length - 1

          ReDim TransValues(nArrayLength)
          ReDim GroupValues(nArrayLength)
          ReDim AskingAvgValues(nArrayLength)
          ReDim AskingTotValues(nArrayLength)
          ReDim AskingHighValues(nArrayLength)
          ReDim AskingLowValues(nArrayLength)
          ReDim MakeOffValues(nArrayLength)
          ReDim YearAvgValues(nArrayLength)
          ReDim DaysOnTotValues(nArrayLength)
          ReDim DaysOnAvgValues(nArrayLength)
          ReDim OffMarketValues(nArrayLength)
          ReDim OnMarketValues(nArrayLength)
          ReDim WithdrawnValues(nArrayLength)
          ReDim SectionValues(nArrayLength)
          ReDim ITValues(nArrayLength)
          ReDim NewToMktValues(nArrayLength)

          For iLoop As Integer = 0 To UBound(ColumnSet)
            TransValues(iLoop) = 0
            GroupValues(iLoop) = 0
            AskingAvgValues(iLoop) = 0
            AskingTotValues(iLoop) = 0
            AskingHighValues(iLoop) = 0
            AskingLowValues(iLoop) = 0
            MakeOffValues(iLoop) = 0
            YearAvgValues(iLoop) = 0
            DaysOnAvgValues(iLoop) = 0
            DaysOnTotValues(iLoop) = 0
            OffMarketValues(iLoop) = 0
            OnMarketValues(iLoop) = 0
            WithdrawnValues(iLoop) = 0
            SectionValues(iLoop) = 0
            ITValues(iLoop) = 0
            NewToMktValues(iLoop) = 0
          Next

          htmlOut.Append(localFunctions.WriteLineToBoth("<td align=""center"" valign=""middle"" nowrap=""nowrap"">TOTAL</td></tr>", marketFile, marketFile_wHeader))

          nTransGroupTotal = 0
          nTransFromTotal = 0
          nTransTotal = 0

          sLast_From = ""

          afiltered_Transactions = sqlDT_trans_summary.Select(FilterString, SortString)

          If afiltered_Transactions.Length > 0 Then

            For Each Row As DataRow In afiltered_Transactions

              If Not IsDBNull(Row.Item("journ_date")) Then
                If Not String.IsNullOrEmpty(Row.Item("journ_date").ToString.Trim) Then
                  fJourn_date = CDate(Row.Item("journ_date").ToString.Trim)
                End If
              End If

              If Not IsDBNull(Row.Item("journ_subcategory_code")) Then
                If Not String.IsNullOrEmpty(Row.Item("journ_subcategory_code").ToString.Trim) Then
                  fJourn_subcategory_code = Row.Item("journ_subcategory_code").ToString
                End If
              End If

              If Not IsDBNull(Row.Item("jcat_subcategory_name")) Then
                If Not String.IsNullOrEmpty(Row.Item("jcat_subcategory_name").ToString.Trim) Then
                  fJcat_subcategory_name = Row.Item("jcat_subcategory_name").ToString
                End If
              End If

              If Not IsDBNull(Row.Item("ac_journ_id")) Then
                If Not String.IsNullOrEmpty(Row.Item("ac_journ_id").ToString.Trim) Then
                  fAc_journ_id = CLng(Row.Item("ac_journ_id").ToString)
                End If
              End If

              nTotalProcessed += 1

              ' do we have a new transaction type
              If sCurrentTransType <> fJourn_subcategory_code Then

                If Not String.IsNullOrEmpty(sCurrentTransType.Trim) And Session.Item("marketSumType").ToString.Trim.ToLower.Contains("trans_type") Then
                  htmlOut.Append(localFunctions.WriteLineToBoth(localFunctions.Print_Transaction_Type_Totals(localACSelection, ColumnSet, TransValues, sTransactionCode, sTrans_To,
                                                                                                             sLast_From, sCurrentTransType, nTransGroupTotal, aBusinessTypesArray), marketFile, marketFile_wHeader))
                End If

                sPreviousTransType = sCurrentTransType
                sCurrentTransType = fJourn_subcategory_code

                If Session.Item("marketSumType").ToString.Trim.ToLower.Contains("trans_source") Or Session.Item("marketSumType").ToString.Trim.ToLower.Contains("trans_type") Then
                  If sTransactionCode.ToLower = "ma" Or sTransactionCode.ToLower = "om" Or sTransactionCode.ToLower = "wo" Then
                    sTrans_Group = fJourn_subcategory_code.Substring(0, 2)
                  Else
                    sTrans_Group = localFunctions.business_type_name(fJourn_subcategory_code.Substring(2, 2), aBusinessTypesArray)
                  End If
                End If

                If Session.Item("marketSumType").ToString.Trim.ToLower.Contains("trans_destination") Then
                  If sTransactionCode.ToLower = "ma" Or sTransactionCode.ToLower = "om" Or sTransactionCode.ToLower = "wo" Then
                    sTrans_Group = fJourn_subcategory_code.Substring(0, 2)
                  Else
                    sTrans_Group = localFunctions.business_type_name(fJourn_subcategory_code.Substring(fJourn_subcategory_code.Length - 2, 2), aBusinessTypesArray)
                  End If
                End If

                ' DO WE HAVE A NEW TRANSACTION FROM
                If (sTrans_Group <> sPrevious_Trans_Group) Then

                  If sTransactionCode.ToLower = "ma" Or sTransactionCode.ToLower = "om" Or sTransactionCode.ToLower = "wo" Then
                    htmlOut.Append(localFunctions.WriteLineToBoth(localFunctions.Print_Transaction_Group_Totals(localACSelection, ColumnSet, GroupValues, sTransactionCode, sCurrentTransType,
                                                                                                                nTransFromTotal, sColSpan, aBusinessTypesArray), marketFile, marketFile_wHeader))
                  Else
                    htmlOut.Append(localFunctions.WriteLineToBoth(localFunctions.Print_Transaction_Group_Totals(localACSelection, ColumnSet, GroupValues, sTransactionCode, sPreviousTransType,
                                                                                                                nTransFromTotal, sColSpan, aBusinessTypesArray), marketFile, marketFile_wHeader))
                  End If

                  ' NOT FIRST TRANSACTION FROM
                  sPrevious_Trans_Group = sTrans_Group
                  nTransFromTotal = 0

                End If   ' NEW TRANS FROM

                If sTransactionCode.ToLower = "ma" Or sTransactionCode.ToLower = "om" Or sTransactionCode.ToLower = "wo" Then
                  sTrans_To = fJourn_subcategory_code.Substring(0, 2)
                Else
                  sTrans_To = localFunctions.business_type_name(fJourn_subcategory_code.Substring(fJourn_subcategory_code.Length - 2, 2), aBusinessTypesArray)
                End If

                nTransGroupTotal = 0

              End If

              For iLoop As Integer = 0 To UBound(ColumnSet)

                Select Case sTimeScale.ToLower
                  Case "years"
                    columnSetMonth = CInt(1)
                    columnSetYear = CInt(Right(ColumnSet(iLoop), 4))
                  Case "quarters"
                    columnQuarterMonth = Left(ColumnSet(iLoop), InStr(1, ColumnSet(iLoop), "/") - 1)
                    columnSetYear = CInt(Right(ColumnSet(iLoop), 4))
                  Case Else
                    columnSetMonth = CInt(Left(ColumnSet(iLoop), InStr(1, ColumnSet(iLoop), "/") - 1))
                    columnSetYear = CInt(Right(ColumnSet(iLoop), 4))
                End Select

                Select Case sTimeScale.ToLower
                  Case "years"
                    If Year(fJourn_date) = columnSetYear Then
                      localFunctions.Store_Trans_Totals(Row, iLoop, sCurrentTransType, TransValues, GroupValues, SectionValues, DaysOnTotValues, DaysOnAvgValues,
                                                        AskingTotValues, AskingAvgValues, AskingHighValues, AskingLowValues, MakeOffValues, YearAvgValues, NewToMktValues, ITValues)
                    End If

                  Case "quarters"
                    If Year(fJourn_date) = columnSetYear Then
                      If localFunctions.Get_Quarter_For_Month_Server(Month(fJourn_date)) = columnQuarterMonth Then
                        localFunctions.Store_Trans_Totals(Row, iLoop, sCurrentTransType, TransValues, GroupValues, SectionValues, DaysOnTotValues, DaysOnAvgValues,
                                                          AskingTotValues, AskingAvgValues, AskingHighValues, AskingLowValues, MakeOffValues, YearAvgValues, NewToMktValues, ITValues)
                      End If
                    End If

                  Case "months"
                    If Year(fJourn_date) = columnSetYear Then
                      If Month(fJourn_date) = columnSetMonth Then
                        localFunctions.Store_Trans_Totals(Row, iLoop, sCurrentTransType, TransValues, GroupValues, SectionValues, DaysOnTotValues, DaysOnAvgValues,
                                                          AskingTotValues, AskingAvgValues, AskingHighValues, AskingLowValues, MakeOffValues, YearAvgValues, NewToMktValues, ITValues)
                      End If
                    End If

                  Case "days"
                    If DateDiff("D", fJourn_date, CDate(ColumnSet(iLoop))) = 0 Then
                      localFunctions.Store_Trans_Totals(Row, iLoop, sCurrentTransType, TransValues, GroupValues, SectionValues, DaysOnTotValues, DaysOnAvgValues,
                                                        AskingTotValues, AskingAvgValues, AskingHighValues, AskingLowValues, MakeOffValues, YearAvgValues, NewToMktValues, ITValues)
                    End If
                End Select
              Next

              If Not sCurrentTransType.Substring(sCurrentTransType.Length - 2, 2).ToUpper.Contains("IT") Then
                nTransGroupTotal += 1
                nTransTotal += 1
                nTransFromTotal += 1
              End If

            Next

            If Session.Item("marketSumType").ToString.Trim.ToLower.Contains("trans_type") Then
              htmlOut.Append(localFunctions.WriteLineToBoth(localFunctions.Print_Transaction_Type_Totals(localACSelection, ColumnSet, TransValues, sTransactionCode,
                                                                                                         sTrans_To, sLast_From, sCurrentTransType, nTransGroupTotal, aBusinessTypesArray), marketFile, marketFile_wHeader))
            End If

            ' TOTALS FOR GROUP TOTALS
            htmlOut.Append(localFunctions.WriteLineToBoth(localFunctions.Print_Transaction_Group_Totals(localACSelection, ColumnSet, GroupValues, sTransactionCode,
                                                                                                        sCurrentTransType, nTransFromTotal, sColSpan, aBusinessTypesArray), marketFile, marketFile_wHeader))

            htmlOut.Append(localFunctions.WriteLineToBoth(localFunctions.Print_Transaction_Section_Totals(localACSelection, ColumnSet, TransValues, SectionValues,
                                                                                                          ITValues, DaysOnTotValues, DaysOnAvgValues, AskingTotValues,
                                                                                                          AskingAvgValues, AskingHighValues, AskingLowValues, MakeOffValues,
                                                                                                          YearAvgValues, NewToMktValues, sTransactionClass, sTransactionCode, sCurrentTransType,
                                                                                                          nTransTotal, sTransHeaderString, sColSpan), marketFile, marketFile_wHeader))

            ' CLEAR THE ARRAY OF VALUES
            For iLoop As Integer = 0 To UBound(ColumnSet)
              TransValues(iLoop) = 0
              GroupValues(iLoop) = 0
              AskingAvgValues(iLoop) = 0
              AskingTotValues(iLoop) = 0
              AskingHighValues(iLoop) = 0
              AskingLowValues(iLoop) = 0
              MakeOffValues(iLoop) = 0
              YearAvgValues(iLoop) = 0
              DaysOnAvgValues(iLoop) = 0
              DaysOnTotValues(iLoop) = 0
              OffMarketValues(iLoop) = 0
              OnMarketValues(iLoop) = 0
              WithdrawnValues(iLoop) = 0
              SectionValues(iLoop) = 0
              ITValues(iLoop) = 0
              NewToMktValues(iLoop) = 0
            Next

          Else
            localFunctions.WriteLineToFile("<tr><td colspan=""" + sReportColSpan + """ align=""center"" valign=""middle"">No Transactions Found", marketFile, marketFile_wHeader)
            htmlOut.Append("<tr class=""header_row""><td colspan=""" + sColSpan + """ align=""center"" valign=""middle"">No Transactions Found")
            htmlOut.Append(localFunctions.WriteLineToBoth("</td></tr>", marketFile, marketFile_wHeader))
          End If

          htmlOut.Append(localFunctions.WriteLineToBoth("</table><br />", marketFile, marketFile_wHeader))

          sCurrentTransType = ""
          sPreviousTransType = ""
          sTrans_Group = ""
          sTrans_To = ""
          sPrevious_Trans_Group = ""
          nTransGroupTotal = 0
          nTransFromTotal = 0
          nTransTotal = 0

        Next ' ClassNum = 0 To UBound(TransClasses)  

        htmlOut.Append("</div>")

        ' div tag for available summary report
        htmlOut.Append(localFunctions.WriteLineToBoth("</div>", marketFile, marketFile_wHeader))

      End If

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br />Error in build_transaction_summary()" + ex.Message
    Finally

      sqlDT_trans_summary.Dispose()
      sqlDT_trans_summary = Nothing

      SqlConn.Close()
      SqlConn.Dispose()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing

    End Try

    Return htmlOut.ToString

  End Function

  Private Function build_retail_summary() As String

    Dim SqlException As System.Data.SqlClient.SqlException = Nothing
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand

    Dim sqlDT_retail_summary As New DataTable
    Dim sqlDR_retail_summary As System.Data.SqlClient.SqlDataReader = Nothing

    Dim htmlOut = New StringBuilder()
    Dim sQuery As New StringBuilder()

    Dim sColSpan = CStr(nScaleSets * 3 - 1 + 3)

    Dim sReportColSpan = CStr(nScaleSets * 2 + 1)

    Dim sAvailHeaderString As String = ""

    Dim YearMonth As String = ""
    Dim sYearMonthValue As String = ""

    Dim summaryMonth As Integer = 0
    Dim summaryYear As Integer = 0

    Dim columnSetMonth As Integer = 0
    Dim columnSetYear As Integer = 0
    Dim columnQuarterMonth As String = ""

    Try

      SqlConn.ConnectionString = Session.Item("jetnetClientDatabase").ToString.Trim

      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = System.Data.CommandType.Text     '
      SqlCommand.CommandTimeout = 60

      ' make the available summaries



      Select Case sTimeScale.ToLower
        Case "years"
          sQuery.Append("SELECT DATEPART(year, journ_date) AS tYear, ")
        Case "quarters"
          sQuery.Append("SELECT DATEPART(year, journ_date) AS tYear, DATEPART(quarter, journ_date) AS tMonth,")
        Case Else
          sQuery.Append("SELECT DATEPART(year, journ_date) AS tYear, DATEPART(month, journ_date) AS tMonth,")
      End Select

      sQuery.Append(" MIN(ac_sale_price) AS dLowSelling, AVG(ac_sale_price) AS dAvgSelling, MAX(ac_sale_price) AS dHighSelling, COUNT(*) AS nSalePricecount")
      sQuery.Append(" FROM Aircraft WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id")
      sQuery.Append(" INNER JOIN Journal WITH(NOLOCK) ON journ_id = ac_journ_id AND journ_ac_id = ac_id")
      sQuery.Append(" WHERE NOT (journ_subcat_code_part3 IN ('DB','DS','FI','FY','IT','MF','RE','CC','LS', 'RM'))")
      sQuery.Append(" AND (journ_subcat_code_part1 = 'WS') AND (journ_internal_trans_flag = 'N')")
      sQuery.Append(" AND (ac_sale_price IS NOT NULL) AND (ac_sale_price <> 0)")

      If Not String.IsNullOrEmpty(AirframeTypeString.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_airframe_type_code IN (" + AirframeTypeString.Trim + ")")
      End If

      If Not String.IsNullOrEmpty(TypeString.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_type_code IN (" + TypeString.Trim + ")")
      End If

      If Not String.IsNullOrEmpty(MakeString.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_make_name IN (" + MakeString.Trim + ")")
      End If

      If nMarketModelID > -1 Then
        sQuery.Append(Constants.cAndClause + "amod_id = " + nMarketModelID.ToString)
      ElseIf Not String.IsNullOrEmpty(ModelsString.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_id IN (" + ModelsString.Trim + ")")
      End If

      ' now add weight class
      If Not WeightClassStr.ToUpper.Contains("ALL") Then

        If Not String.IsNullOrEmpty(WeightClassStr.Trim) Then
          If WeightClassStr.Contains(Constants.cCommaDelim) Then
            sQuery.Append(Constants.cAndClause + "amod_weight_class IN ('" + WeightClassStr.Replace(Constants.cCommaDelim, Constants.cValueSeperator).Trim + "')")
          Else
            sQuery.Append(Constants.cAndClause + "amod_weight_class = '" + WeightClassStr.Trim + "'")
          End If
        End If

      End If

      ' now add Mfr Names
      If Not ManufacturerStr.ToUpper.Contains("ALL") Then

        If Not String.IsNullOrEmpty(ManufacturerStr.Trim) Then

          If ManufacturerStr.Contains(Constants.cValueSeperator) Then
            sQuery.Append(Constants.cAndClause + "amod_manufacturer_common_name IN ('" + ManufacturerStr.Trim + "')")
          Else
            sQuery.Append(Constants.cAndClause + "amod_manufacturer_common_name = '" + ManufacturerStr.Trim + "'")
          End If

        End If

      End If

      ' now add ac sizes
      If Not AcSizeStr.ToUpper.Contains("ALL") Then

        If Not String.IsNullOrEmpty(AcSizeStr.Trim) Then

          If AcSizeStr.Contains(Constants.cValueSeperator) Then
            sQuery.Append(Constants.cAndClause + "amod_jniq_size IN ('" + AcSizeStr.Trim + "')")
          Else
            sQuery.Append(Constants.cAndClause + "amod_jniq_size = '" + AcSizeStr.Trim + "'")
          End If

        End If

      End If

      If Not bHasHelicopterFilter And Not bHasBusinessFilter And Not bHasCommercialFilter Then
        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True))
      Else
        sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(bHasHelicopterFilter, bHasBusinessFilter, bHasCommercialFilter, False, False, False, True))
      End If

      If Not String.IsNullOrEmpty(Session.Item("marketStartDate").ToString.Trim) Then
        sQuery.Append(Constants.cAndClause + "(journ_date >= CONVERT(DATETIME, '" + Session.Item("marketStartDate").ToString.Trim + "',102))")
      End If

      If Not String.IsNullOrEmpty(Session.Item("marketEndDate").ToString.Trim) Then
        sQuery.Append(Constants.cAndClause + "(journ_date < CONVERT(DATETIME, '" + Session.Item("marketEndDate").ToString.Trim + "',102))")
      End If



      Select Case sTimeScale.ToLower
        Case "years"
          sQuery.Append(" GROUP BY DATEPART(year, journ_date) ")
          sQuery.Append(" ORDER BY DATEPART(year, journ_date) ASC ")
        Case "quarters"
          sQuery.Append(" GROUP BY DATEPART(year, journ_date), DATEPART(quarter, journ_date)")
          sQuery.Append(" ORDER BY DATEPART(year, journ_date) ASC, DATEPART(quarter, journ_date) ASC")
        Case Else
          sQuery.Append(" GROUP BY DATEPART(year, journ_date), DATEPART(month, journ_date)")
          sQuery.Append(" ORDER BY DATEPART(year, journ_date) ASC, DATEPART(month, journ_date) ASC")
      End Select




      If Not bPreviousSummary Then
        HttpContext.Current.Session.Item("MasterMarketRetailSummary") = sQuery.ToString
      Else
        sQuery = Nothing
        sQuery = New StringBuilder
        sQuery.Append(HttpContext.Current.Session.Item("MasterMarketRetailSummary").ToString)
      End If

      HttpContext.Current.Session.Item("MasterMarketRetailSummary") = sQuery.ToString

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>build_retail_summary()</b><br />" + sQuery.ToString

      SqlCommand.CommandText = sQuery.ToString
      sqlDR_retail_summary = SqlCommand.ExecuteReader()

      Try
        sqlDT_retail_summary.Load(sqlDR_retail_summary)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = sqlDT_retail_summary.GetErrors()
      End Try

      sqlDR_retail_summary.Close()
      sqlDR_retail_summary.Dispose()

      If sqlDT_retail_summary.Rows.Count > 0 Then

        ' div tag for available summary report
        htmlOut.Append(localFunctions.WriteLineToBoth("<div style='text-align:center;'>", marketFile, marketFile_wHeader))

        'If Session("userBrowserType") = "saf" Then
        '  htmlOut.Append("<div style='width: 99%; text-align:center; overflow-x: scroll;'>")
        'Else
        '  htmlOut.Append("<div style='width: 99%; text-align:center; overflow-x: auto;'>")
        'End If

        htmlOut.Append("<div style=""width: 100%; overflow: auto; vertical-align: top;"">")

        nTotalRecords = sqlDT_retail_summary.Rows.Count

        htmlOut.Append(localFunctions.WriteLineToBoth("<table class='data_aircraft_grid' border='1' cellpadding='2' cellspacing='0' bordercolor='#949494' width='100%'>", marketFile, marketFile_wHeader))

        If Not isHeliOnlyProduct Then
          localFunctions.WriteLineToFile("<tr><td colspan='" + sReportColSpan + "' align='center' valign='middle'><font size='2.5'><b>Retail Sales Market Summary", marketFile, marketFile_wHeader)
          htmlOut.Append("<tr class=""header_row""><td colspan='" + sColSpan + "' align='center' valign='middle'><font size='2.5'><b>Retail Sales Market Summary")
        Else
          localFunctions.WriteLineToFile("<tr><td colspan='" + sReportColSpan + "' align='center' valign='middle'><font size='2.5'><b>Retail Sales Market Summary", marketFile, marketFile_wHeader)
          htmlOut.Append("<tr class=""header_row""><td colspan='" + sColSpan + "' align='center' valign='middle'><font size='2.5'><b>Retail Sales Market Summary")
        End If

        htmlOut.Append(localFunctions.WriteLineToBoth("</b></font></td></tr><tr class=""header_row"">", marketFile, marketFile_wHeader))

        If Session.Item("marketTimeScale").ToString.ToLower.Contains("months") Then
          htmlOut.Append(localFunctions.WriteLineToBoth("<td>&nbsp;</td>", marketFile, marketFile_wHeader))
        Else

          If Session.Item("marketTimeScale").ToString.ToLower.Contains("months") Then
            htmlOut.Append(localFunctions.WriteLineToBoth("<td>&nbsp;</td>", marketFile, marketFile_wHeader))
          ElseIf Session.Item("marketTimeScale").ToString.ToLower.Contains("years") Then
            htmlOut.Append(localFunctions.WriteLineToBoth("<td>AVERAGES ARE YEARLY - Over Time Period</td>", marketFile, marketFile_wHeader))
          ElseIf Session.Item("marketTimeScale").ToString.ToLower.Contains("quarters") Then
            htmlOut.Append(localFunctions.WriteLineToBoth("<td>AVERAGES ARE QUARTERLY - Over Time Period</td>", marketFile, marketFile_wHeader))
          Else
            htmlOut.Append(localFunctions.WriteLineToBoth("<td>AVERAGES ARE MONTHLY - Over Time Period</td>", marketFile, marketFile_wHeader))
          End If
        End If

        ' SET UP TIMESCALE HEADERS FOR AVAILABLE SUMMARIES
        Dim tmpHtmlOut As String = ""
        sAvailHeaderString = localFunctions.generate_timescale_headers(tmpHtmlOut, CDate(Session.Item("marketStartDate").ToString), CDate(Session.Item("marketEndDate").ToString), Session.Item("marketTimeScale"), False, True)

        htmlOut.Append(tmpHtmlOut)

        ' CREATE ARRAYS FOR HEADER AND EACH TRANSACTION TYPE
        Dim ColumnSet() As String = sAvailHeaderString.Split(Constants.cCommaDelim)

        Dim nArrayLength As Integer = ColumnSet.Length - 1

        Dim dLowSelling(nArrayLength) As Double
        Dim dLowSelling_Count(nArrayLength) As Double

        Dim dAvgSelling(nArrayLength) As Double
        Dim dAvgSelling_Count(nArrayLength) As Double

        Dim dHighSelling(nArrayLength) As Double
        Dim dHighSelling_Count(nArrayLength) As Double

        Dim nSpCount(nArrayLength) As Double
        Dim nSpCount_Count(nArrayLength) As Double

        Dim YearMonth_Count(nArrayLength) As Double

        For iLoop As Integer = 0 To UBound(ColumnSet)

          dLowSelling(iLoop) = 0
          dLowSelling_Count(iLoop) = 0

          dAvgSelling(iLoop) = 0
          dAvgSelling_Count(iLoop) = 0

          dHighSelling(iLoop) = 0
          dHighSelling_Count(iLoop) = 0

          nSpCount(iLoop) = 0
          nSpCount_Count(iLoop) = 0

          YearMonth_Count(iLoop) = 0

        Next

        htmlOut.Append(localFunctions.WriteLineToBoth("<td align='center' valign='middle'>AVERAGE</td>", marketFile, marketFile_wHeader))

        For Each Row As DataRow In sqlDT_retail_summary.Rows

          Select Case sTimeScale.ToLower
            Case "years"
              summaryYear = CInt(Row.Item("tYear").ToString)
              sYearMonthValue = Trim(summaryYear.ToString)
            Case "quarters"
              summaryMonth = CInt(Row.Item("tMonth").ToString)      ' tQuarter
              summaryYear = CInt(Row.Item("tYear").ToString)
              sYearMonthValue = Trim(summaryYear.ToString + "-" + summaryMonth.ToString)
            Case Else
              summaryMonth = CInt(Row.Item("tMonth").ToString)
              summaryYear = CInt(Row.Item("tYear").ToString)
              sYearMonthValue = Trim(summaryYear.ToString + "-" + summaryMonth.ToString)
          End Select



          For iLoop As Integer = 0 To UBound(ColumnSet)

            Select Case sTimeScale.ToLower
              Case "years"
                columnSetMonth = CInt(1)
                columnSetYear = CInt(Right(ColumnSet(iLoop), 4))
              Case "quarters"
                columnQuarterMonth = Left(ColumnSet(iLoop), InStr(1, ColumnSet(iLoop), "/") - 1)      ' changed to be month - based even though its quarters 
                'columnSetYear = CInt(Right(ColumnSet(iLoop), 4))
                'columnSetMonth = Left(ColumnSet(iLoop), InStr(1, ColumnSet(iLoop), "/") - 1)
                columnSetYear = CInt(Right(ColumnSet(iLoop), 4))
              Case Else
                columnSetMonth = CInt(Left(ColumnSet(iLoop), InStr(1, ColumnSet(iLoop), "/") - 1))
                columnSetYear = CInt(Right(ColumnSet(iLoop), 4))
            End Select

            Select Case sTimeScale.ToLower
              Case "years"
                If summaryYear = columnSetYear Then

                  If YearMonth <> sYearMonthValue Then
                    YearMonth = sYearMonthValue
                    YearMonth_Count(iLoop) += 1
                  End If

                  localFunctions.Store_Retail_Totals(Row, iLoop, dLowSelling, dLowSelling_Count, dAvgSelling, dAvgSelling_Count, dHighSelling, dHighSelling_Count, nSpCount, nSpCount_Count)

                End If

              Case "quarters"
                If summaryYear = columnSetYear Then
                  If summaryMonth = Replace(columnQuarterMonth, "Q", "") Then
                    '  If localFunctions.Get_Quarter_For_Month_Server(summaryMonth) = columnQuarterMonth Then   ' commented out .. changed quarter to be done in the query - MSW  - 4/25/19

                    If YearMonth <> sYearMonthValue Then
                      YearMonth = sYearMonthValue
                      YearMonth_Count(iLoop) += 1
                    End If

                    localFunctions.Store_Retail_Totals(Row, iLoop, dLowSelling, dLowSelling_Count, dAvgSelling, dAvgSelling_Count, dHighSelling, dHighSelling_Count, nSpCount, nSpCount_Count)

                    'End If
                  End If
                End If

              Case "months"
                If summaryYear = columnSetYear Then
                  If summaryMonth = columnSetMonth Then

                    If YearMonth <> sYearMonthValue Then
                      YearMonth = sYearMonthValue
                      YearMonth_Count(iLoop) += 1
                    End If

                    localFunctions.Store_Retail_Totals(Row, iLoop, dLowSelling, dLowSelling_Count, dAvgSelling, dAvgSelling_Count, dHighSelling, dHighSelling_Count, nSpCount, nSpCount_Count)

                  End If
                End If

            End Select

          Next ' iLoop

        Next

        htmlOut.Append(localFunctions.Print_Retail_Summaries(CDate(Session("marketStartDate").ToString), CDate(Session("marketEndDate").ToString), sAvailHeaderString, sColSpan, ColumnSet, YearMonth_Count,
                                                                dLowSelling, dLowSelling_Count, dAvgSelling, dAvgSelling_Count, dHighSelling, dHighSelling_Count, nSpCount, nSpCount_Count))

        Dim tmpString = Session.Item("marketTimeScale").ToString.Replace("s", Constants.cEmptyString)

        localFunctions.WriteLineToFile("<tr><td colspan='" + sReportColSpan + "' align='center' valign='middle'><font size='1.5'>All figures calculated at end of " + tmpString + " (%Chg)=Percent change from previous " + tmpString + ".</font></td></tr>", marketFile, marketFile_wHeader)
        localFunctions.WriteLineToFile("<tr><td colspan='" + sReportColSpan + "' align='center' valign='middle'><font size='1.5'>NYA=Not Yet Available, NC=Not Calculated.", marketFile, marketFile_wHeader)

        htmlOut.Append("<tr class=""header_row""><td colspan='" + sColSpan + "' align='center' valign='middle'><font size='1.5'>All figures calculated at end of " + tmpString + " (%Chg)=Percent change from previous " + tmpString + ".</font></td></tr>")
        htmlOut.Append("<tr class=""header_row""><td colspan='" + sColSpan + "' align='center' valign='middle'><font size='1.5'>NYA=Not Yet Available, NC=Not Calculated.")

        htmlOut.Append(localFunctions.WriteLineToBoth("&nbsp; Average column includes total for final " + tmpString + " since all data was reported.</font></td></tr></table>", marketFile, marketFile_wHeader))

        htmlOut.Append("</div>")

        ' div tag for available summary report
        htmlOut.Append(localFunctions.WriteLineToBoth("</div>", marketFile, marketFile_wHeader))
        localFunctions.WriteLineToFile("<br/>", marketFile, marketFile_wHeader)

        htmlOut.Append("<hr width='100%' size='1' style='margin:10px 0px 10px 0px;'>")

      End If

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in build_retail_summary()" + ex.Message
    Finally

      sqlDT_retail_summary.Dispose()
      sqlDT_retail_summary = Nothing

      SqlConn.Close()
      SqlConn.Dispose()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing

    End Try

    Return htmlOut.ToString

  End Function

  Private Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
    If Page.Request.Form("project_search") = "Y" Then
      'if either of these variables is passed, then go ahead and complete this search.
      summary_search_Click(summary_search, EventArgs.Empty)
    End If
  End Sub

  Private Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

    If Session.Item("crmUserLogon") <> True Then
      Response.Redirect("Default.aspx", False)
    Else

      If Not IsNothing(Request.Item("restart")) Then
        If Not String.IsNullOrEmpty(Request.Item("restart").ToString) Then
          If Request.Item("restart") = "1" Then
            ClearSelections()
          End If
        End If
      End If

      productCodeCount = DisplayFunctions.ReturnProductCodeCount(productCodeCount)

    End If

  End Sub

  Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

    Try

      'This will go ahead and display it on the master page.
      Master.SetStatusText(HttpContext.Current.Session.Item("SearchString").ToString)

    Catch ex As Exception

      Dim previousException As String = ex.Message.Trim

      Try

        If Not IsNothing(Master) Then
          Master.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
        Else
          HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.ToString.Trim
        End If
      Catch ex2 As Exception

        commonLogFunctions.forceLogError("ERROR", System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

      End Try
    End Try

  End Sub

  Public Function GetMARKETMakeModelTypeFromCommonControl(ByVal sTypeMakeModelCtrlBaseName As String, ByVal BuildSearchString As String,
                                                          ByRef ModelsString As String, ByRef MakeString As String, ByRef TypeString As String, ByRef AirframeTypeString As String,
                                                          ByRef CombinedAirframeTypeString As String, ByRef WeightClassDDL As Object, ByRef WeightClassStr As String,
                                                          ByRef ManufacturerStr As String, ByRef AcSizeStr As String,
                                                          Optional ByRef Business As Boolean = False, Optional ByRef Helicopter As Boolean = False, Optional ByRef Commercial As Boolean = False) As String
    Dim sAirframeType As String = ""
    Dim sAirType As String = ""
    Dim sMake As String = ""
    Dim sModel As String = ""
    Dim sUsage As String = ""
    Dim sAirFrame As String = ""

    Dim ModelTextDisplay As String = ""

    '-------------------------------------------------------------

    'BUSINESS CHECKBOX
    Dim VariableBusiness As Boolean = Business
    If HttpContext.Current.Request.Form("complete_search") = "Y" Or HttpContext.Current.Request.Form("project_search") = "Y" Then
      'VariableBusiness = HttpContext.Current.Session.Item("hasBusinessFilter")
    Else
      If Not IsNothing(HttpContext.Current.Request.Item("chkBusinessFilter")) Then
        VariableBusiness = HttpContext.Current.Request.Item("chkBusinessFilter")
      End If
    End If

    'Added a small check. If their business flag is false, this is always false no matter. 
    If HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = False Then
      VariableBusiness = False
    End If

    HttpContext.Current.Session.Item("hasBusinessFilter") = VariableBusiness
    Business = VariableBusiness

    '-------------------------------------------------------------
    'COMMERCIAL CHECKBOX
    Dim VariableCommercial As Boolean = Commercial
    If HttpContext.Current.Request.Form("complete_search") = "Y" Or HttpContext.Current.Request.Form("project_search") = "Y" Then
      'VariableCommercial = HttpContext.Current.Session.Item("hasCommercialFilter")
    Else
      If Not IsNothing(HttpContext.Current.Request.Item("chkCommercialFilter")) Then
        VariableCommercial = HttpContext.Current.Request.Item("chkCommercialFilter")
      End If
    End If

    'Added a small check. If their Commercial flag is false, this is always false no matter. 
    If HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = False Then
      VariableCommercial = False
    End If

    HttpContext.Current.Session.Item("hasCommercialFilter") = VariableCommercial
    Commercial = VariableCommercial

    '-------------------------------------------------------------
    'HELICOPTER CHECKBOX
    Dim VariableHelicopter As Boolean = Helicopter
    If HttpContext.Current.Request.Form("complete_search") = "Y" Or HttpContext.Current.Request.Form("project_search") = "Y" Then
      'VariableHelicopter = HttpContext.Current.Session.Item("hasHelicopterFilter")
    Else
      If Not IsNothing(HttpContext.Current.Request.Item("chkHelicopterFilter")) Then
        VariableHelicopter = HttpContext.Current.Request.Item("chkHelicopterFilter")
      End If
    End If

    'Added a small check. If their Helicopter flag is false, this is always false no matter. 
    If HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = False Then
      VariableHelicopter = False
    End If

    Helicopter = VariableHelicopter
    HttpContext.Current.Session.Item("hasHelicopterFilter") = VariableHelicopter

    ''''''''''''''''''''''''''''''''''' 
    'Here's one more small check.
    'We check to see if all three are false.
    'If they are, we set whatever's set up in session.
    If Business = False And Helicopter = False And Commercial = False Then
      'Setting up Business in Session
      Business = HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag
      'Setting up Helicopter in Session
      Helicopter = HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag
      'Setting up Commercial in session
      Commercial = HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag
    End If


    '----------------
    'TYPE 
    Dim VariableType As String = ""
    If HttpContext.Current.Request.Form("complete_search") = "Y" Or HttpContext.Current.Request.Form("project_search") = "Y" Or bPreviousSummary Then
      VariableType = HttpContext.Current.Session.Item("tabAircraftType")
    Else
      If Not IsNothing(HttpContext.Current.Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "AircraftType")) Then
        VariableType = HttpContext.Current.Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "AircraftType")
      End If
    End If

    If Not IsNothing(VariableType) Then
      If Not String.IsNullOrEmpty(VariableType.ToString) Then
        If Not VariableType.ToString.ToLower.Contains("all") Then
          HttpContext.Current.Session.Item("tabAircraftType") = VariableType.ToString.Trim

          Dim TypeArray As Array = Split(HttpContext.Current.Session.Item("tabAircraftType"), ",")
          For MultipleModelCount = 0 To UBound(TypeArray)
            Dim CurrentModelCount As Long = CLng(TypeArray(MultipleModelCount))

            commonEvo.ReturnModelDataFromIndex(CurrentModelCount, sAirframeType, sAirType, sMake, sModel, sUsage)

            If TypeString <> "" Then
              TypeString += ", "
              AirframeTypeString += ", "
              CombinedAirframeTypeString += ","
            End If

            TypeString += "'" + sAirType + "'"
            AirframeTypeString += "'" + sAirframeType + "'"
            CombinedAirframeTypeString += sAirType + "|" + sAirframeType
          Next
          BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(Replace(TypeString, "'", ""), "Type(s)")
          BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(Replace(AirframeTypeString, "'", ""), "Airframe Type(s)")
        Else
          HttpContext.Current.Session.Item("tabAircraftModel") = ""
          HttpContext.Current.Session.Item("tabAircraftMake") = ""
          HttpContext.Current.Session.Item("tabAircraftType") = ""
        End If
      End If
    End If

    '-----------------
    'MAKE
    Dim VariableMake As String = ""
    If HttpContext.Current.Request.Form("complete_search") = "Y" Or HttpContext.Current.Request.Form("project_search") = "Y" Or bPreviousSummary Then
      VariableMake = HttpContext.Current.Session.Item("tabAircraftMake")
    Else
      If Not IsNothing(HttpContext.Current.Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "AircraftMake")) Then
        VariableMake = HttpContext.Current.Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "AircraftMake")
      End If
    End If
    If Not IsNothing(VariableMake) Then
      If Not String.IsNullOrEmpty(VariableMake.ToString) Then
        If Not VariableMake.ToString.ToLower.Contains("all") Then
          HttpContext.Current.Session.Item("tabAircraftMake") = VariableMake.ToString.Trim

          Dim MakeArray As Array = Split(HttpContext.Current.Session.Item("tabAircraftMake"), ",")
          For MultipleModelCount = 0 To UBound(MakeArray)
            Dim CurrentModelCount As Long = CLng(MakeArray(MultipleModelCount))

            commonEvo.ReturnModelDataFromIndex(CurrentModelCount, sAirframeType, sAirType, sMake, sModel, sUsage)

            If MakeString <> "" Then
              MakeString += ", "
            End If

            MakeString += "'" & Replace(sMake, "'", "''") & "'"
          Next
          BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(Replace(MakeString, "'", ""), "Make(s)")

        Else
          HttpContext.Current.Session.Item("tabAircraftModel") = ""
          HttpContext.Current.Session.Item("tabAircraftMake") = ""
        End If
      End If
    End If

    '-----------------
    'MODEL
    Dim VariableModel As String = ""
    If HttpContext.Current.Request.Form("complete_search") = "Y" Or HttpContext.Current.Request.Form("project_search") = "Y" Or bPreviousSummary Then
      VariableModel = HttpContext.Current.Session.Item("tabAircraftModel")
    Else
      If Not IsNothing(HttpContext.Current.Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "AircraftModel")) Then
        VariableModel = HttpContext.Current.Request.Item("cbo" + sTypeMakeModelCtrlBaseName + "AircraftModel")
      End If
    End If

    If Not IsNothing(VariableModel) Then
      If Not String.IsNullOrEmpty(VariableModel.ToString) Then
        If Not VariableModel.ToString.ToLower.Contains("all") Then
          HttpContext.Current.Session.Item("tabAircraftModel") = VariableModel.ToString.Trim

          Dim ModelArray As Array = Split(HttpContext.Current.Session.Item("tabAircraftModel"), ",")

          For MultipleModelCount = 0 To UBound(ModelArray)
            Dim CurrentModelCount As Long = CLng(ModelArray(MultipleModelCount))

            commonEvo.ReturnModelDataFromIndex(CurrentModelCount, sAirframeType, sAirType, sMake, sModel, sUsage)

            If ModelsString <> "" Then
              ModelsString += ","
              ModelTextDisplay += ", "
            End If

            ModelsString += commonEvo.ReturnAmodIDForItemIndex(CurrentModelCount).ToString
            ModelTextDisplay += sModel
          Next

          BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(ModelTextDisplay, "Model(s)")

        Else
          HttpContext.Current.Session.Item("tabAircraftModel") = ""
        End If
      End If
    End If

    '----------------- 
    'WEIGHT CLASS
    Dim VariableWeight As String = ""
    Dim displayWeightClassString As String = ""

    If HttpContext.Current.Request.Form("complete_search") = "Y" Or HttpContext.Current.Request.Form("project_search") = "Y" Or bPreviousSummary Then
      VariableWeight = HttpContext.Current.Session.Item("marketWeightClass")
    Else
      If Not IsNothing(HttpContext.Current.Session.Item("marketWeightClass")) Then
        If TypeOf WeightClassDDL Is DropDownList Then
          VariableWeight = WeightClassDDL.selectedValue
        End If
      End If
    End If

    If Not IsNothing(VariableWeight) Then
      If Not String.IsNullOrEmpty(VariableWeight.Trim) Then
        If Not VariableWeight.ToLower.Contains("all") Then

          For Each li In WeightClassDDL.Items
            If li.Selected Then

              If String.IsNullOrEmpty(WeightClassStr.Trim) Then
                WeightClassStr = li.Value.ToString.Trim
                displayWeightClassString = commonEvo.TranslateAcWeightClass(li.Value.ToString.Trim)
              Else
                WeightClassStr += Constants.cCommaDelim + li.Value.ToString.Trim
                displayWeightClassString += Constants.cCommaDelim + commonEvo.TranslateAcWeightClass(li.Value.ToString.Trim)
              End If

            End If
          Next

          ' first add the "tick" for the text
          WeightClassStr = WeightClassStr.Replace(Constants.cCommaDelim, Constants.cValueSeperator)

          BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(displayWeightClassString, "Weight Class")
        Else
          HttpContext.Current.Session.Item("tabAircraftModelWeightClass") = ""
        End If
      End If
    End If

    'Manufacturer
    Dim VariableManufacturer As String = ""
    If HttpContext.Current.Request.Form("complete_search") = "Y" Or HttpContext.Current.Request.Form("project_search") = "Y" Then
      VariableManufacturer = HttpContext.Current.Session.Item("tabAircraftMfrNames")
    Else
      If Not IsNothing(HttpContext.Current.Request.Item("ddlMfrName")) Then
        VariableManufacturer = HttpContext.Current.Request.Item("ddlMfrName")
      End If
    End If

    If Not IsNothing(VariableManufacturer) Then
      If Not String.IsNullOrEmpty(VariableManufacturer.Trim) Then
        If Not VariableManufacturer.ToString.ToLower.Contains("all") Then

          HttpContext.Current.Session.Item("tabAircraftMfrNames") = VariableManufacturer.ToString.Trim

          Dim MfrArray As Array = Split(HttpContext.Current.Session.Item("tabAircraftMfrNames"), ",")

          For mfrNameCount = 0 To UBound(MfrArray)

            If String.IsNullOrEmpty(ManufacturerStr.Trim) Then
              ManufacturerStr = MfrArray(mfrNameCount)
            Else
              ManufacturerStr += Constants.cCommaDelim + MfrArray(mfrNameCount)
            End If

          Next

          Dim displayMfrString As String = ManufacturerStr

          ' first add the "tick" for the text
          ManufacturerStr = ManufacturerStr.Replace(Constants.cCommaDelim, Constants.cValueSeperator)

          BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(displayMfrString, "Manufacturer")
        Else
          HttpContext.Current.Session.Item("tabAircraftMfrNames") = ""
        End If
      End If
    End If

    'Ac Size 
    Dim VariableAcSize As String = ""
    Dim displayAcSizeString As String = ""

    If HttpContext.Current.Request.Form("complete_search") = "Y" Or HttpContext.Current.Request.Form("project_search") = "Y" Then
      VariableAcSize = HttpContext.Current.Session.Item("tabAircraftSize")
    Else
      If Not IsNothing(HttpContext.Current.Request.Item("ddlSizeCat")) Then
        VariableAcSize = HttpContext.Current.Request.Item("ddlSizeCat")
      End If
    End If

    If Not IsNothing(VariableAcSize) Then
      If Not String.IsNullOrEmpty(VariableAcSize.Trim) Then
        If Not VariableAcSize.ToString.ToLower.Contains("all") Then

          HttpContext.Current.Session.Item("tabAircraftSize") = VariableAcSize.ToString.Trim

          Dim AcSizeArray As Array = Split(HttpContext.Current.Session.Item("tabAircraftSize"), ",")

          For acSizeCount = 0 To UBound(AcSizeArray)

            If String.IsNullOrEmpty(AcSizeStr.Trim) Then
              AcSizeStr = AcSizeArray(acSizeCount)
              displayAcSizeString = commonEvo.TranslateAcSizes(AcSizeArray(acSizeCount))
            Else
              AcSizeStr += Constants.cCommaDelim + AcSizeArray(acSizeCount)
              displayAcSizeString += Constants.cCommaDelim + commonEvo.TranslateAcSizes(AcSizeArray(acSizeCount))
            End If

          Next


          ' first add the "tick" for the text
          AcSizeStr = AcSizeStr.Replace(Constants.cCommaDelim, Constants.cValueSeperator)

          BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(displayAcSizeString, "Size")
        Else
          HttpContext.Current.Session.Item("tabAircraftSize") = ""
        End If
      End If
    End If

    Return BuildSearchString

  End Function

End Class