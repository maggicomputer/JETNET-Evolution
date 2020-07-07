Partial Public Class Operating_Listing
  Inherits System.Web.UI.Page
  Dim aclsData_temp As New clsData_Manager_SQL
  Private sTypeMakeModelCtrlBaseName As String = "Aircraft"
  Public productCodeCount As Integer = 0
  Public isHeliOnlyProduct As Boolean = False
  Public bUsernameExists As Boolean = False

  Public Shared masterPage As New Object

  Private Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

    If Session.Item("isMobile") Then
      Me.MasterPageFile = "~/EvoStyles/MobileTheme.master"
      masterPage = DirectCast(Page.Master, MobileTheme)
    Else
      Me.MasterPageFile = "~/EvoStyles/EvoTheme.master"
      masterPage = DirectCast(Page.Master, EvoTheme)
    End If

  End Sub

  Private Sub Operating_Listing_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

    If Not IsNothing(Request.Item("restart")) Then
      If Not String.IsNullOrEmpty(Request.Item("restart").ToString) Then
        If Request.Item("restart") = "1" Then
          ResetPage()
        End If
      End If
    End If

    If Session.Item("crmUserLogon") <> True Then
      Response.Redirect("Default.aspx", False)
    Else

      'Setting up Chosen Select Dropdown:
      If Session.Item("isMobile") = True Then

        Dim dropdownString As New StringBuilder
        dropdownString.Append("$(""#metricToggle"").click(function() {")
        dropdownString.Append("$(""#standardTable"").hide();")
        dropdownString.Append("$(""#metricTable"").show();")
        dropdownString.Append("});")
        dropdownString.Append("$(""#imperialToggle"").click(function() {")
        dropdownString.Append("$(""#metricTable"").hide();")
        dropdownString.Append("$(""#standardTable"").show();")
        dropdownString.Append("});")

        dropdownString.Append("function swapChosenDropdowns() {")
        dropdownString.Append("$("".chosen-select"").chosen(""destroy"");")
        dropdownString.Append("$("".chosen-select"").chosen({ no_results_text: ""No results found."", disable_search_threshold: 10 });")
        dropdownString.Append("}")
        If Not Page.ClientScript.IsClientScriptBlockRegistered("chosenDropdowns") Then
          System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "chosenDropdowns", dropdownString.ToString, True)
        End If

        dropdownString = New StringBuilder
        dropdownString.Append(";swapChosenDropdowns();")

        System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "CreateDropdown", dropdownString.ToString, True)
      End If


      If Not Page.IsPostBack Then
        'This needs to be put in and loaded for now. Hopefully whenever the session variables are the same, this can go away.
        If Not Session.Item("localPreferences").loadUserSession("", CLng(Session.Item("localUser").crmSubSubID.ToString), Session.Item("localUser").crmUserLogin.ToString, CLng(Session.Item("localUser").crmSubSeqNo.ToString), CLng(Session.Item("localUser").crmUserContactID.ToString)) Then
          Response.Write("error in load preferences : ")
        End If

        'This will go ahead and set up the javascript control array. Not needed unless you're going to need the array (such as to find an amod ID index) before the search button is clicked
        'Generally you won't, but on the ac listing page, you use folders and the home page market tab

        commonEvo.fillAirframeArray("")
        commonEvo.fillAircraftTypeLableArray("")
        commonEvo.fillDefaultAirframeArray("")

        commonEvo.fillMfrNamesArray("")
        commonEvo.fillAircraftSizeArray("")

        If Page.Request.Form("project_search") = "Y" Then
          ClearSelections()
          Dim folderID As Long = 0
          Dim FoldersTableData As New DataTable
          Dim cfolderData As String = ""


          FolderInformation.Text = ""
          FolderInformation.Visible = False
          folderID = Page.Request.Form("project_id")

          FoldersTableData = masterPage.aclsData_Temp.GetEvolutionFolderssBySubscription(folderID, Session.Item("localUser").crmUserLogin, Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmSubSeqNo, "", 0, Nothing, "")
          If Not IsNothing(FoldersTableData) Then
            If FoldersTableData.Rows.Count > 0 Then
              cfolderData = FoldersTableData.Rows(0).Item("cfolder_data").ToString


              If FoldersTableData.Rows(0).Item("cfolder_method").ToString = "S" Then
                operating_costs_folder_name.Text = FoldersTableData.Rows(0).Item("cfolder_name").ToString
              End If
              If cfolderData <> "" Then
                'Fills up the applicable folder Information pulled from the cfolder data field
                DisplayFunctions.FillUpFolderInformation(opCostsTableSearch, close_current_folder, cfolderData, FolderInformation, FoldersTableData, False, False, False, False, False, Operating_Collapse_Panel, Operating_actions_submenu_dropdown, Nothing, Nothing, Nothing, "", False, True)
              End If
            End If
          End If
        End If
      End If

      masterPage.SetDefaultButtion(Me.operating_search.UniqueID)

    End If
  End Sub
  ''' <summary>
  ''' IMPORTANT NOTE:
  ''' THE FUNCTION INSIDE THE BUTTON CLICK NEEDS
  ''' THE FOREIGN EXCHANGE RATE. RIGHT NOW IT IS HARD CODED
  ''' TO ZERO. 
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Session.Item("crmUserLogon") <> True Then
      Response.Redirect("Default.aspx", False)
    Else

      aclsData_temp = New clsData_Manager_SQL
      aclsData_temp.JETNET_DB = Session.Item("jetnetClientDatabase")

      If Not Page.IsPostBack Then
        'Add help button text here: 7/20/15
        Operating_help_text.Text = clsGeneral.clsGeneral.CreateEvoHelpLink("Operating Costs")
      End If

      ''Setting up the page:
      ViewTMMDropDowns.setIsView(False)

      ViewTMMDropDowns.setShowWeightClass(True)
      ViewTMMDropDowns.setShowMfrNames(True)
      ViewTMMDropDowns.setShowAcSize(True)

      ViewTMMDropDowns.setListSize(8)
      ViewTMMDropDowns.setControlName(sTypeMakeModelCtrlBaseName)
      DisplayFunctions.FillUpSessionForMakeTypeModel(sTypeMakeModelCtrlBaseName, ViewTMMDropDowns)

      If Page.IsPostBack Then
        'Setting up the project search
        If Session.Item("isMobile") Then
          If Not String.IsNullOrEmpty(makeModelDynamic.SelectedValue) Then
            Dim ModelData As Array = Split(makeModelDynamic.SelectedValue, "|")
            If UBound(ModelData) = 3 Then
              HttpContext.Current.Session.Item("tabAircraftType") = commonEvo.FindIndexForFirstItem(UCase(ModelData(0)), crmWebClient.Constants.AIRFRAME_TYPE, ModelData(1), crmWebClient.Constants.AIRFRAME_FRAME).ToString()
              HttpContext.Current.Session.Item("tabAircraftMake") = commonEvo.FindIndexForFirstItem(UCase(ModelData(2)), crmWebClient.Constants.AIRFRAME_MAKE, ModelData(1), crmWebClient.Constants.AIRFRAME_FRAME)
              HttpContext.Current.Session.Item("tabAircraftModel") = commonEvo.FindIndexForItemByAmodID(CLng(ModelData(3)))
            End If
          End If
        End If
      End If


      ToggleHigherLowerBar(False)

      If Not Page.IsPostBack And Page.Request.Form("complete_search") <> "Y" Then
        Initial(True)
        ' Criteria_Bar2.Visible = False
      Else
        Initial(False)
      End If

      If HttpContext.Current.Session.Item("localPreferences").DefaultCurrency = 0 Then
        HttpContext.Current.Session.Item("localPreferences").DefaultCurrency = 9 'us dollar
        HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate = 0
      End If

      'Load Search Information:
      If Not Page.IsPostBack Then

        fill_currency_dropdown(0, IIf(Session.Item("isMobile") = True, mobileCurrency, currencyList), HttpContext.Current.Session.Item("localPreferences").DefaultCurrency)

        FillOutSearchParameters()


        If Session.Item("isMobile") = True Then

          us_standard.Text = "US"
          metric_standard.Text = "Metric"
          opcosts_make_model_panel.CssClass = "display_none"
          MobileSearchVisible.Visible = True


          If makeModelDynamic.SelectedValue = "" Then
            If Session.Item("localPreferences").DefaultModel > 0 Then
              HttpContext.Current.Session.Item("tabAircraftModel") = commonEvo.FindIndexForItemByAmodID(Session.Item("localPreferences").DefaultModel)
            Else
              If Session.Item("localPreferences").UserBusinessFlag = True Then
                If Session.Item("localPreferences").Tierlevel = eTierLevelTypes.TURBOS Then
                  HttpContext.Current.Session.Item("tabAircraftModel") = commonEvo.FindIndexForItemByAmodID(207) '- king air b200 
                Else 'Jets or ALL
                  HttpContext.Current.Session.Item("tabAircraftModel") = commonEvo.FindIndexForItemByAmodID(272)   ' challenger 300 - business jet
                End If
              ElseIf Session.Item("localPreferences").UserCommercialFlag = True Then
                HttpContext.Current.Session.Item("tabAircraftModel") = commonEvo.FindIndexForItemByAmodID(698) ' boeng bbj -  commercial jet 
              ElseIf Session.Item("localPreferences").UserHelicopterFlag = True Then
                HttpContext.Current.Session.Item("tabAircraftModel") = commonEvo.FindIndexForItemByAmodID(408) ' augusta westland aw139 - helicopter 
              End If
            End If
          End If

          DisplayFunctions.SingleModelLookupAndFill(makeModelDynamic, masterPage)

          If makeModelDynamic.SelectedValue <> "" Then
            makeModelDynamic_SelectedIndexChanged(makeModelDynamic, System.EventArgs.Empty)
          End If
        End If
      End If

      '    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
      '    ''''''''''''''''Some neat functions that might help'''''''''''''''''''''''''
      '    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' 
      '    'Pass the tab index of what you want highlighted on the bar.
      masterPage.Set_Active_Tab(6)
      'This will set page title.
      Me.Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Operating Cost Search Results")



      'Set up bars to display correctly.
      If Session.Item("isMobile") Then
      Else

        If Not Page.IsPostBack Then
          Dim FoldersTable As New DataTable

          'Fill Folders Table
          operating_folders_submenu_dropdown.Items.Clear()
          DisplayFunctions.AddEditFolderListOptionToFolderDropdown(operating_folders_submenu_dropdown, 11)
          FoldersTable = masterPage.aclsData_Temp.GetEvolutionFolderssBySubscription(0, Session.Item("localUser").crmUserLogin, Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmSubSeqNo, "", 11, Nothing, "")
          If Not IsNothing(FoldersTable) Then
            If FoldersTable.Rows.Count > 0 Then
              For Each r As DataRow In FoldersTable.Rows
                If Not IsDBNull(r("cfolder_data")) Then
                  Dim FolderDataString As Array
                  'this was added to parse out the real search query now that we're saving it
                  FolderDataString = Split(r("cfolder_data"), "THEREALSEARCHQUERY")
                  operating_folders_submenu_dropdown.Items.Add(New ListItem(r("cfolder_name").ToString, "javascript:ParseSpecsOperatingMarketForm('" & r("cfolder_id").ToString & "',false,true,false,'" & Replace(FolderDataString(0), "'", "\'") & "');"))
                End If
              Next
            End If
          End If
        End If
      End If
    End If

    us_standard.Attributes.Add("onclick", "muxStandard();toggleMetricLabels();")
    metric_standard.Attributes.Add("onclick", "muxMetric();toggleMetricLabels();")
    add_MuxDisplayUnits_Script(us_standard, metric_standard)
    add_ToggleMetric_Script(us_standard, Div0, nautical_miles, statute_miles)

    nautical_miles.Attributes.Add("onclick", "muxNauticalMiles();")
    statute_miles.Attributes.Add("onclick", "muxStatuteMiles();")
    add_MuxDisplayMiles_Script(nautical_miles, statute_miles)

  End Sub

  Public Sub ToggleHigherLowerBar(ByVal lower_bar As Boolean)

    If lower_bar = True Then
      OperatingPanelEx.Collapsed = True
      OperatingPanelEx.ClientState = True
      Operating_search_expand_text.Visible = False
      Operating_help_text.Visible = False
    End If

    'actions dropdown
    Operating_actions_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & Operating_actions_submenu_dropdown.ClientID & "', true);")
    Operating_actions_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & Operating_actions_submenu_dropdown.ClientID & "', false);")

    Operating_actions_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & Operating_actions_submenu_dropdown.ClientID & "', true);")
    Operating_actions_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & Operating_actions_submenu_dropdown.ClientID & "', false);")


    'folder dropdown
    operating_folders_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & operating_folders_submenu_dropdown.ClientID & "', true);")
    operating_folders_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & operating_folders_submenu_dropdown.ClientID & "', false);")

    operating_folders_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & operating_folders_submenu_dropdown.ClientID & "', true);")
    operating_folders_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & operating_folders_submenu_dropdown.ClientID & "', false);")


    If Session.Item("isMobile") Then
      Operating_actions_dropdown.CssClass = "display_none"
      Operating_actions_submenu_dropdown.CssClass = "display_none"
      operating_folders_dropdown.CssClass = "display_none"
      operating_folders_submenu_dropdown.CssClass = "display_none"
    End If
  End Sub

  Public Sub Initial(ByVal initial_page_load As Boolean)
    If initial_page_load = True Then
      OperatingPanelEx.Collapsed = False
      OperatingPanelEx.ClientState = False
      Operating_actions_dropdown.Visible = False

      Operating_actions_submenu_dropdown.Items.Add(New ListItem("Save As - New Folder", "javascript:SubMenuDrop(3,0, 'OPERATING COSTS');"))
      Operating_actions_submenu_dropdown.Items.Add(New ListItem("Custom Export", "javascript:SubMenuDrop(1,0,'OPERATING COST');"))
      Operating_actions_submenu_dropdown.Items.Add(New ListItem("JETNET Export/Report", "javascript:SubMenuDrop(5,0,'OPERATING COST');"))
      Operating_actions_submenu_dropdown.Items.Add(New ListItem("Summary", "javascript:SubMenuDrop(2,0,'OPERATING COSTS');"))


    Else
      OperatingPanelEx.Collapsed = True
      OperatingPanelEx.ClientState = True
      Operating_actions_dropdown.Visible = True

    End If

  End Sub

  Private Sub operating_search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles operating_search.Click
    Dim ModelsString As String = ""
    Dim MakeString As String = ""
    Dim TypeString As String = ""

    Dim WeightClassDDL As New Object
    Dim WeightClass As String = ""

    Dim ManufacturerStr As String = ""

    Dim AcSizeStr As String = ""

    Dim AirframeTypeString As String = ""
    Dim CombinedAirframeTypeString As String = ""

    Dim BuildSearchString As String = ""
    Dim FuelBurn As String = ""
    Dim FuelBurnOperator As String = ""
    Dim TotalCost As String = ""
    Dim TotalCostOperator As String = ""
    Dim Business As Boolean = False
    Dim Helicopter As Boolean = False
    Dim Commercial As Boolean = False

    'We're going to go ahead and set a new search class,
    'but if one exists in session, we're using that one first
    Dim NewSearchClass As New SearchSelectionCriteria
    If Not IsNothing(Session.Item("searchCriteria")) Then
      NewSearchClass = Session.Item("searchCriteria")
    End If


    If Not IsNothing(ViewTMMDropDowns.FindControl("ddlWeightClass")) Then
      WeightClassDDL = ViewTMMDropDowns.FindControl("ddlWeightClass")
    End If

    'Model/Make/Type String Building
    BuildSearchString += DisplayFunctions.GetMakeModelTypeFromCommonControl("", BuildSearchString,
                                                                            ModelsString, MakeString,
                                                                            TypeString, AirframeTypeString,
                                                                            CombinedAirframeTypeString,
                                                                            WeightClassDDL, WeightClass,
                                                                            ManufacturerStr, AcSizeStr,
                                                                            Business, Helicopter, Commercial)

    If Not String.IsNullOrEmpty(WeightClass.Trim) Then
      'Setting up The Weight Class in Session
      NewSearchClass.SearchCriteriaWeightClass = WeightClass
    End If

    If Not String.IsNullOrEmpty(ManufacturerStr.Trim) Then
      'Setting up The Weight Class in Session
      NewSearchClass.SearchCriteriaManufacturerName = ManufacturerStr
    End If

    If Not String.IsNullOrEmpty(AcSizeStr.Trim) Then
      'Setting up The ac size in Session
      NewSearchClass.SearchCriteriaAcSize = AcSizeStr
    End If

    'Fuel Burn 
    If Not String.IsNullOrEmpty(fuel_burn_txt.Text) Then
      FuelBurn = fuel_burn_txt.Text
      FuelBurnOperator = fuel_burn_operator_ddl.SelectedValue
      BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(fuel_burn_txt, "Fuel Burn " & FuelBurnOperator)
    End If

    'Total Operating
    If Not String.IsNullOrEmpty(total_direct_txt.Text) Then
      TotalCost = total_direct_txt.Text
      TotalCostOperator = total_direct_ddl.SelectedValue
      BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(total_direct_txt, "Total Direct Cost " & TotalCostOperator)
    End If

    'Saving values into session
    NewSearchClass.SearchCriteriaOpCostsFuelBurn = FuelBurn
    NewSearchClass.SearchCriteriaOpCostsFuelBurnOperator = FuelBurnOperator
    NewSearchClass.SearchCriteriaOpCostsTotalDirectCosts = TotalCost
    NewSearchClass.SearchCriteriaOpCostsTotalDirectCostsOperator = TotalCostOperator
    NewSearchClass.SearchCriteriaOpCostsCurrency = IIf(Session.Item("isMobile") = True, mobileCurrency.SelectedValue, currencyList.SelectedValue)

    HttpContext.Current.Session.Item("localPreferences").DefaultCurrency = currencyList.SelectedValue

    If metric_standard.Checked = True Then
      NewSearchClass.SearchCriteriaDisplayUnits = "M"
    Else
      NewSearchClass.SearchCriteriaDisplayUnits = ""
    End If

    If statute_miles.Checked = True Then
      NewSearchClass.SearchCriteriaDisplayMiles = "S"
    Else
      NewSearchClass.SearchCriteriaDisplayMiles = ""
    End If

    OperatingPanelEx.Collapsed = True
    OperatingPanelEx.ClientState = True

    'set up top text
    HttpContext.Current.Session.Item("SearchString") = BuildSearchString
    Initial(False)
    Session.Item("searchCriteria") = NewSearchClass
    OperatingSearch(MakeString, ModelsString, TypeString, AirframeTypeString, _
                    IIf(metric_standard.Checked, True, False), IIf(statute_miles.Checked, True, False), _
                    WeightClass, ManufacturerStr, AcSizeStr, _
                    FuelBurn, FuelBurnOperator, TotalCost, TotalCostOperator, Business, Helicopter, Commercial)

  End Sub

  Private Sub ResetPage()
    ClearSelections()
    Response.Redirect("Operating_Listing.aspx")
  End Sub

  Private Sub reset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles reset.Click
    ResetPage()
  End Sub

  Private Sub ClearSelections()
    'Clear out the Type/Make/Model Boxes Properly on Reset:
    HttpContext.Current.Session.Item("tabAircraftType") = ""
    HttpContext.Current.Session.Item("tabAircraftMake") = ""
    HttpContext.Current.Session.Item("tabAircraftModel") = ""
    HttpContext.Current.Session.Item("tabAircraftModelWeightClass") = ""
    HttpContext.Current.Session.Item("tabAircraftMfrNames") = ""
    HttpContext.Current.Session.Item("tabAircraftSize") = ""
    Session.Item("searchCriteria") = New SearchSelectionCriteria
  End Sub

  Private Sub Operating_Listing_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
    If Page.Request.Form("project_search") = "Y" Then
      'if either of these variables is passed, then go ahead and complete this search.
      operating_search_Click(operating_search, EventArgs.Empty)
    End If
  End Sub

  Private Sub FillOutSearchParameters()
    Try
      'Filling Back in the Search Criteria.
      'Operator Costs

      'Fuel Burn Operator
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaOpCostsFuelBurnOperator) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaOpCostsFuelBurnOperator) Then
          fuel_burn_operator_ddl.SelectedValue = Session.Item("searchCriteria").SearchCriteriaOpCostsFuelBurnOperator.ToString
        End If
      End If

      'Fuel Burn Value
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaOpCostsFuelBurn) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaOpCostsFuelBurn) Then
          fuel_burn_txt.Text = Session.Item("searchCriteria").SearchCriteriaOpCostsFuelBurn.ToString
        End If
      End If

      'Total Costs Operator
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaOpCostsTotalDirectCostsOperator) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaOpCostsTotalDirectCostsOperator) Then
          total_direct_ddl.SelectedValue = Session.Item("searchCriteria").SearchCriteriaOpCostsTotalDirectCostsOperator.ToString
        End If
      End If

      'Total Costs Value
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaOpCostsTotalDirectCosts) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaOpCostsTotalDirectCosts) Then
          total_direct_txt.Text = Session.Item("searchCriteria").SearchCriteriaOpCostsTotalDirectCosts.ToString
        End If
      End If

      'Currency
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaOpCostsCurrency) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaOpCostsCurrency) Then
          currencyList.SelectedValue = Session.Item("searchCriteria").SearchCriteriaOpCostsCurrency.ToString
        End If
      End If

      'Display Units
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaDisplayUnits) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaDisplayUnits) Then
          If Session.Item("searchCriteria").SearchCriteriaDisplayUnits.ToString.ToUpper.Contains("M") Then
            us_standard.Checked = False
            metric_standard.Checked = True
          Else
            us_standard.Checked = True
            metric_standard.Checked = False
          End If
        End If
      End If

      'Display Miles
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaDisplayMiles) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaDisplayMiles) Then
          If Session.Item("searchCriteria").SearchCriteriaDisplayMiles.ToString.ToUpper.Contains("S") Then
            nautical_miles.Checked = False
            statute_miles.Checked = True
          Else
            nautical_miles.Checked = True
            statute_miles.Checked = False
          End If
        End If
      End If

    Catch ex As Exception
      ' Masterpage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " : " & ex.Message.ToString)
    End Try
  End Sub

  Public Shared Function get_currency_info() As DataTable
    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT currency_id, currency_exchange_rate, currency_name, currency_exchange_rate_date FROM Currency WITH(NOLOCK)")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText + "<br /><br />get_currency_info() As DataTable</b><br />" + sQuery.ToString

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
      End Try

    Catch ex As Exception
      Return Nothing

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

  Public Sub fill_currency_dropdown(ByRef maxWidth As Long, ByRef lbCurrency As DropDownList, ByVal nSelectedCurrency As Integer)

    Dim results_table As New DataTable

    Try

      lbCurrency.Items.Clear()

      results_table = get_currency_info()

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          For Each r As DataRow In results_table.Rows

            If Not IsDBNull(r.Item("currency_name")) And Not String.IsNullOrEmpty(r.Item("currency_name").ToString.Trim) Then

              If (r.Item("currency_name").ToString.Length * crmWebClient.Constants._STARTCHARWIDTH) > maxWidth Then
                maxWidth = (r.Item("currency_name").ToString.Length * crmWebClient.Constants._STARTCHARWIDTH)
              End If

              lbCurrency.Items.Add(New ListItem(r.Item("currency_name").ToString, r.Item("currency_id").ToString))

              If Not String.IsNullOrEmpty(r.Item("currency_id").ToString.Trim) Then
                If IsNumeric(r.Item("currency_id").ToString) Then
                  If CInt(r.Item("currency_id").ToString) = nSelectedCurrency Then
                    lbCurrency.SelectedValue = nSelectedCurrency.ToString
                  End If
                End If
              End If

            End If

          Next
        End If
      End If

      If nSelectedCurrency = 0 Then
        lbCurrency.SelectedValue = "9" ' default to us dollar
      End If

      lbCurrency.Width = (maxWidth)

    Catch ex As Exception

      masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString + " : " + ex.Message.ToString)

    Finally

    End Try

    results_table = Nothing

  End Sub

  Private Sub makeModelDynamic_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles makeModelDynamic.SelectedIndexChanged
    Initial(True)

    If Not String.IsNullOrEmpty(makeModelDynamic.SelectedValue) Then
      Dim ModelData As Array = Split(makeModelDynamic.SelectedValue, "|")
      If UBound(ModelData) = 3 Then
        HttpContext.Current.Session.Item("tabAircraftType") = commonEvo.FindIndexForFirstItem(UCase(ModelData(0)), crmWebClient.Constants.AIRFRAME_TYPE, ModelData(1), crmWebClient.Constants.AIRFRAME_FRAME).ToString()
        HttpContext.Current.Session.Item("tabAircraftMake") = commonEvo.FindIndexForFirstItem(UCase(ModelData(2)), crmWebClient.Constants.AIRFRAME_MAKE, ModelData(1), crmWebClient.Constants.AIRFRAME_FRAME)
        HttpContext.Current.Session.Item("tabAircraftModel") = commonEvo.FindIndexForItemByAmodID(CLng(ModelData(3)))

        OperatingSearch(UCase(ModelData(2)), CLng(ModelData(3)), UCase(ModelData(0)), ModelData(1), IIf(metric_standard.Checked, True, False), IIf(statute_miles.Checked, True, False), _
                        "", "", "", "", "", "", "", Session.Item("localSubscription").crmBusiness_Flag, _
                        Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmCommercial_Flag)

      End If
    End If

  End Sub

  Private Sub mobileCurrency_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles mobileCurrency.SelectedIndexChanged
    currencyList.SelectedValue = mobileCurrency.SelectedValue
    makeModelDynamic_SelectedIndexChanged(makeModelDynamic, System.EventArgs.Empty)
  End Sub

  Public Sub OperatingSearch(ByVal Make_String As String, ByVal Model_String As String, ByVal Model_Type_String As String, ByVal Airframe_Type_String As String, _
                           ByVal UseMetric As Boolean, ByVal UseStatute As Boolean, ByVal WeightClass As String, ByVal ManufacturerName As String, ByVal AcSize As String, _
                           ByVal FuelBurn As String, ByVal FuelBurnOperator As String, ByVal TotalDirectCost As String, ByVal TotalDirectCostOperator As String, _
                           ByVal Business As Boolean, ByVal Helicopter As Boolean, ByVal Commercial As Boolean)
    Try
      container_operating_costs.CssClass = "performance_container_content"

      Dim Results_Table As New DataTable
      Dim ForeignExchangeRate As Double = 0
      Dim FuelBase As Double = 0
      Dim CurrencyName As String = ""
      Dim CurrencyDate As String = ""

      Dim OpSearchCriteria As New viewSelectionCriteriaClass
      Dim ListofACIDS As String = ""
      Dim DisplayMixedType As Boolean = False
      Dim ResultString As String = ""
      Dim tmpViewObj As New viewsDataLayer

      tmpViewObj.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      tmpViewObj.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim

      masterPage.SetStatusText(HttpContext.Current.Session.Item("SearchString"))

      Results_Table = OperatingCostSearch(Model_String, Model_Type_String, Airframe_Type_String, Make_String, _
                                          WeightClass, ManufacturerName, AcSize, _
                                          FuelBurn, FuelBurnOperator, TotalDirectCost, TotalDirectCostOperator, _
                                          Business, Helicopter, Commercial, UseMetric, UseStatute)

      Call commonLogFunctions.Log_User_Event_Data("UserSearch", "Operating Search: " & clsGeneral.clsGeneral.StripChars(clsGeneral.clsGeneral.stripHTML(Replace(HttpContext.Current.Session.Item("SearchString"), "<br />", " ")), False), Nothing, 0, 0, 0, 0, 0, 0, 0)

      If Not IsNothing(Results_Table) Then
        Session.Item("localUser").crmLatestRecordCount = Results_Table.Rows.Count
        If Results_Table.Rows.Count > 0 Then

          FuelBase = 0
          ForeignExchangeRate = 0


          If Results_Table.Rows.Count > 1 Then

            For Each r As DataRow In Results_Table.Rows
              If String.IsNullOrEmpty(ListofACIDS.Trim) Then
                ListofACIDS = r.Item("amod_id").ToString
              Else
                ListofACIDS += crmWebClient.Constants.cCommaDelim + r.Item("amod_id").ToString
              End If
            Next

            DisplayMixedType = commonEvo.check_for_multi_airframes(Results_Table)

            If Not DisplayMixedType Then
              OpSearchCriteria.ViewCriteriaAirframeTypeStr = Results_Table.Rows(0).Item("amod_airframe_type_code").ToString.ToUpper.Trim
            End If

            OpSearchCriteria.ViewCriteriaAmodID = -1 ' clear any single model id
            OpSearchCriteria.ViewCriteriaAmodIDArray = Split(ListofACIDS, crmWebClient.Constants.cCommaDelim)
          Else
            If Results_Table.Rows.Count = 1 Then
              OpSearchCriteria.ViewCriteriaAirframeTypeStr = Results_Table.Rows(0).Item("amod_airframe_type_code").ToString.ToUpper.Trim
              OpSearchCriteria.ViewCriteriaAmodID = CLng(Results_Table.Rows(0).Item("amod_id").ToString)
              OpSearchCriteria.ViewCriteriaAmodIDArray = Nothing ' clear any model list
            End If
          End If

          Dim MobileTempCurrency As String = ""

          OpSearchCriteria.ViewCriteriaUseStatuteMiles = UseStatute

          If Session.Item("isMobile") = True Then
            Session.Item("searchCriteria").SearchCriteriaOpCostsCurrency = mobileCurrency.SelectedValue
            Session.Item("localPreferences").DefaultCurrency = mobileCurrency.SelectedValue
            OpSearchCriteria.ViewCriteriaUseMetricValues = False 'DisplayMetric

            operating_listing_text.Text = "<h1>Operating Costs</h1><table id=""standardTable"" cellspacing=""0"" cellpadding=""0"" class='data_aircraft_grid cell_right performanceTable mobileWidth'>"
          Else

            OpSearchCriteria.ViewCriteriaUseMetricValues = UseMetric

            operating_listing_text.Text = "<table id=""outerCostsTable"" cellspacing=""0"" cellpadding=""0"" class=""opCostsTableListing"" border=""1"">"
          End If

          operating_listing_text.Text += "<tr>"


          tmpViewObj.views_display_operating_costs(OpSearchCriteria, True, ResultString)

          If Session.Item("isMobile") = True Then
            operating_listing_text.Text += Replace(Replace(Replace(ResultString, "&nbsp;", " "), "<br />", " "), "padding-left:5px;", "border:0px !important")
          Else
            operating_listing_text.Text += ResultString
          End If

          tmpViewObj.views_display_operating_costs(OpSearchCriteria, False, ResultString)

          If Session.Item("isMobile") = True Then
            operating_listing_text.Text += Replace(Replace(Replace(ResultString, "&nbsp;", " "), "<br />", " "), "padding-left:5px;", "border:0px !important")
          Else
            operating_listing_text.Text += ResultString
          End If

          operating_listing_text.Text += "</tr>"
          operating_listing_text.Text += "</table>"

          'Metric
          If Session.Item("isMobile") = True Then
            OpSearchCriteria.ViewCriteriaUseMetricValues = True 'DisplayMetric

            operating_listing_text.Text += "<table id=""metricTable"" style=""display:none"" cellspacing=""0"" cellpadding=""0"" class='data_aircraft_grid cell_right performanceTable mobileWidth'>"

            operating_listing_text.Text += "<tr>"

            tmpViewObj.views_display_operating_costs(OpSearchCriteria, True, ResultString)

            If Session.Item("isMobile") = True Then
              operating_listing_text.Text += Replace(Replace(Replace(ResultString, "&nbsp;", " "), "<br />", " "), "padding-left:5px;", "border:0px !important")
            Else
              operating_listing_text.Text += ResultString
            End If

            tmpViewObj.views_display_operating_costs(OpSearchCriteria, False, ResultString)

            If Session.Item("isMobile") = True Then
              operating_listing_text.Text += Replace(Replace(Replace(ResultString, "&nbsp;", " "), "<br />", " "), "padding-left:5px;", "border:0px !important")
            Else
              operating_listing_text.Text += ResultString
            End If

            operating_listing_text.Text += "</tr>"
            operating_listing_text.Text += "</table>"

          End If

        Else
          operating_listing_text.Text = "<br /><br /><p align='center' class='red_text'><b>Your search returned no results.</p><br /><br />"
        End If
      Else
        'There was an error here. Let's record it.
        'And that there was an error on the data side.
        masterPage.LogError("OperatingSearch (operating_listing.aspx): " & aclsData_temp.class_error)


        operating_attention.Text = "<br /><p class='padding'>We're sorry, an error has occurred during your search.</b></p><br /><br />"
        If (InStr(UCase(Session.Item("localUser").crmLocalUserName), "JETNET.COM") > 0) Or (InStr(UCase(Session.Item("localUser").crmLocalUserName), "MVINTECH.COM") > 0) Then
          operating_attention.Text += aclsData_temp.class_error
        End If

        aclsData_temp.class_error = ""


      End If
    Catch ex As Exception
      'There was an error here. Let's record it.
      'And that there was an error on the data side.
      masterPage.LogError("OperatingSearch (operating_listing.aspx): " & ex.Message)


      operating_attention.Text = "<br /><p class='padding'>We're sorry, an error has occurred during your search.</b></p><br /><br />"
      If (InStr(UCase(Session.Item("localUser").crmLocalUserName), "JETNET.COM") > 0) Or (InStr(UCase(Session.Item("localUser").crmLocalUserName), "MVINTECH.COM") > 0) Then
        operating_attention.Text += ex.Message
      End If

    End Try
  End Sub

  Private Function OperatingCostSearch(ByVal amod_ids As String, ByVal modelType As String, ByVal AirframeType As String, ByVal MakeString As String, _
                                      ByVal WeightClass As String, ByVal ManufacturerName As String, ByVal AcSize As String, _
                                      ByVal FuelBurn As String, ByVal FuelBurnOperator As String, _
                                      ByVal TotalDirectCost As String, ByVal TotalDirectCostOperator As String, _
                                      ByVal Business As Boolean, ByVal Helicopter As Boolean, ByVal Commercial As Boolean, ByVal UseMetric As Boolean, ByVal UseStatute As Boolean) As DataTable

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim TempTable As New DataTable
    Dim query As String = ""
    Dim sqlwhere As String = ""
    Try
      HttpContext.Current.Session.Item("OpCostsModelID") = 0
      HttpContext.Current.Session.Item("OpCostsModelList") = ""

      query = "SELECT * "
      query = query & " FROM Aircraft_Model WITH(NOLOCK) "

      If Not String.IsNullOrEmpty(WeightClass.Trim) Then
        If Not String.IsNullOrEmpty(sqlwhere.Trim) Then
          sqlwhere += " and "
        End If

        If WeightClass.Contains(Constants.cValueSeperator) Then
          sqlwhere += " (amod_weight_class IN ('" + WeightClass.Trim + "')) "
        Else
          sqlwhere += " (amod_weight_class = '" + WeightClass.Trim + "') "
        End If

      End If

      If Not String.IsNullOrEmpty(ManufacturerName.Trim) Then
        If Not String.IsNullOrEmpty(sqlwhere.Trim) Then
          sqlwhere += " and "
        End If

        If ManufacturerName.Contains(Constants.cValueSeperator) Then
          sqlwhere += " (amod_manufacturer_common_name IN ('" + ManufacturerName.Trim + "')) "
        Else
          sqlwhere += " (amod_manufacturer_common_name = '" + ManufacturerName.Trim + "') "
        End If

      End If

      If Not String.IsNullOrEmpty(AcSize.Trim) Then
        If Not String.IsNullOrEmpty(sqlwhere.Trim) Then
          sqlwhere += " and "
        End If

        If AcSize.Contains(Constants.cValueSeperator) Then
          sqlwhere += " (amod_jniq_size IN ('" + AcSize.Trim + "')) "
        Else
          sqlwhere += " (amod_jniq_size = '" + AcSize.Trim + "') "
        End If

      End If

      If FuelBurn <> "" Then
        If sqlwhere <> "" Then
          sqlwhere += " and "
        End If
        If Not UseMetric Then
          sqlwhere += " (amod_fuel_burn_rate " & clsGeneral.clsGeneral.PrepQueryString(FuelBurnOperator, FuelBurn, "Numeric", False, "amod_fuel_burn_rate", True) & ")"
        Else
          ' convert FuelBurn from Gallons to Liters
          Dim conversionConstant = ConversionFunctions.ReturnMetricConversonConstant("GAL")
          sqlwhere += " ( (amod_fuel_burn_rate * " + conversionConstant.ToString + ")" & clsGeneral.clsGeneral.PrepQueryString(FuelBurnOperator, FuelBurn, "Numeric", False, "amod_fuel_burn_rate", True) & ") "
        End If
      End If

      If TotalDirectCost <> "" Then
        If sqlwhere <> "" Then
          sqlwhere += " and "
        End If
        sqlwhere += " (amod_tot_hour_direct_cost " & clsGeneral.clsGeneral.PrepQueryString(TotalDirectCostOperator, TotalDirectCost, "Numeric", False, "amod_tot_hour_direct_cost", True) & ")"
      End If

      If amod_ids <> "" Then
        If sqlwhere <> "" Then
          sqlwhere += " and "
        End If
        sqlwhere += " amod_id in (" & amod_ids & ") "
        If InStr(amod_ids.ToString, ",") = 0 Then
          HttpContext.Current.Session.Item("OpCostsModelID") = amod_ids
          HttpContext.Current.Session.Item("OpCostsModelList") = ""
        Else
          HttpContext.Current.Session.Item("OpCostsModelID") = 0
          HttpContext.Current.Session.Item("OpCostsModelList") = amod_ids.ToString
        End If

      Else
        If modelType <> "" Then
          If sqlwhere <> "" Then
            sqlwhere += " and "
          End If
          sqlwhere += " amod_type_code in (" & modelType & ")"
        End If

        If AirframeType <> "" Then
          If sqlwhere <> "" Then
            sqlwhere += " and "
          End If
          sqlwhere += " amod_airframe_type_code in (" & AirframeType & ")"
        End If

        If MakeString <> "" Then
          If sqlwhere <> "" Then
            sqlwhere += " and "
          End If
          sqlwhere += " amod_make_name in (" & MakeString & ")"
        End If
      End If

      If Not Helicopter And Not Business And Not Commercial Then
        sqlwhere = " WHERE" + IIf(Not String.IsNullOrEmpty(sqlwhere.Trim), sqlwhere + Constants.cAndClause, sqlwhere + Constants.cSingleSpace) + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), True, True)
      Else
        sqlwhere = " WHERE" + IIf(Not String.IsNullOrEmpty(sqlwhere.Trim), sqlwhere + Constants.cAndClause, sqlwhere + Constants.cSingleSpace) + commonEvo.BuildProductCodeCheckWhereClause(Helicopter, Business, Commercial, False, False, True, True)
      End If

      HttpContext.Current.Session.Item("MasterAircraftOperatingCostWhere") = sqlwhere

      query += sqlwhere + " ORDER BY amod_make_name, amod_model_name"

      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, query.ToString)

      SqlConn.ConnectionString = Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 120

      SqlCommand.CommandText = query
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        TempTable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = TempTable.GetErrors()
      End Try
      ' End If
      Return TempTable
    Catch ex As Exception
      OperatingCostSearch = Nothing
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in OperatingCostSearch(ByVal amod_ids As String, ByVal modelType As String, ByVal AirframeType As String, ByVal MakeString As String, ByVal WeightClass As String, ByVal FuelBurn As Long, ByVal FuelBurnOperator As String, ByVal TotalDirectCost As Long, ByVal TotalDirectCostOperator As String) As DataTable SQL VERSION: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing

    End Try

  End Function

  Public Sub add_ToggleMetric_Script(ByVal cbSource As CheckBox, ByVal div0 As HtmlControls.HtmlGenericControl, _
                                     ByVal cbSource1 As CheckBox, ByVal cbSource2 As CheckBox)

    'Register the script block
    Dim sScptStr As StringBuilder = New StringBuilder()

    If Not Page.ClientScript.IsClientScriptBlockRegistered("tml-cb-onclick") Then

      sScptStr.Append("<script type=""text/javascript"">")
      sScptStr.Append(vbCrLf & "  function toggleMetricLabels() {")
      sScptStr.Append(vbCrLf & "    if (document.getElementById(""" + cbSource.ClientID.ToString + """).checked == true) {")
      sScptStr.Append(vbCrLf & "      document.getElementById(""" + div0.ClientID.ToString + """).innerHTML = ""<strong>FUEL BURN (Gallons/Hour)</strong>"";")
      sScptStr.Append(vbCrLf & "      document.getElementById(""" + cbSource1.ClientID.ToString + """).disabled = false;")
      sScptStr.Append(vbCrLf & "      document.getElementById(""" + cbSource2.ClientID.ToString + """).disabled = false;")
      sScptStr.Append(vbCrLf & "    }")
      sScptStr.Append(vbCrLf & "    else {")
      sScptStr.Append(vbCrLf & "      document.getElementById(""" + div0.ClientID.ToString + """).innerHTML = ""<strong>FUEL BURN (Liters/Hour)</strong>"";")
      sScptStr.Append(vbCrLf & "      document.getElementById(""" + cbSource1.ClientID.ToString + """).disabled = true;")
      sScptStr.Append(vbCrLf & "      document.getElementById(""" + cbSource2.ClientID.ToString + """).disabled = true;")
      sScptStr.Append(vbCrLf & "    }")
      sScptStr.Append(vbCrLf & "  }")
      sScptStr.Append(vbCrLf & "</script>")

      Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tml-cb-onclick", sScptStr.ToString, False)

    End If

    sScptStr = Nothing

  End Sub

  Public Sub add_MuxDisplayUnits_Script(ByVal cbSource1 As CheckBox, ByVal cbSource2 As CheckBox)

    'Register the script block
    Dim sScptStr As StringBuilder = New StringBuilder()

    If Not Page.ClientScript.IsClientScriptBlockRegistered("mdu-cb-onclick") Then

      sScptStr.Append("<script type=""text/javascript"">")
      sScptStr.Append(vbCrLf & "  function muxStandard() {")
      sScptStr.Append(vbCrLf & "    if (document.getElementById(""" + cbSource1.ClientID.ToString + """).checked == true) {")
      sScptStr.Append(vbCrLf & "      document.getElementById(""" + cbSource2.ClientID.ToString + """).checked = false;")
      sScptStr.Append(vbCrLf & "    }")
      sScptStr.Append(vbCrLf & "  }")
      sScptStr.Append(vbCrLf & "  function muxMetric() {")
      sScptStr.Append(vbCrLf & "    if (document.getElementById(""" + cbSource2.ClientID.ToString + """).checked == true) {")
      sScptStr.Append(vbCrLf & "      document.getElementById(""" + cbSource1.ClientID.ToString + """).checked = false;")
      sScptStr.Append(vbCrLf & "    }")
      sScptStr.Append(vbCrLf & "  }")
      sScptStr.Append(vbCrLf & "</script>")

      Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "mdu-cb-onclick", sScptStr.ToString, False)

    End If

    sScptStr = Nothing

  End Sub

  Public Sub add_MuxDisplayMiles_Script(ByVal cbSource1 As CheckBox, ByVal cbSource2 As CheckBox)

    'Register the script block
    Dim sScptStr As StringBuilder = New StringBuilder()

    If Not Page.ClientScript.IsClientScriptBlockRegistered("mdm-cb-onclick") Then

      sScptStr.Append("<script type=""text/javascript"">")
      sScptStr.Append(vbCrLf & "  function muxNauticalMiles() {")
      sScptStr.Append(vbCrLf & "    if (document.getElementById(""" + cbSource1.ClientID.ToString + """).checked == true) {")
      sScptStr.Append(vbCrLf & "      document.getElementById(""" + cbSource2.ClientID.ToString + """).checked = false;")
      sScptStr.Append(vbCrLf & "    }")
      sScptStr.Append(vbCrLf & "  }")
      sScptStr.Append(vbCrLf & "  function muxStatuteMiles() {")
      sScptStr.Append(vbCrLf & "    if (document.getElementById(""" + cbSource2.ClientID.ToString + """).checked == true) {")
      sScptStr.Append(vbCrLf & "      document.getElementById(""" + cbSource1.ClientID.ToString + """).checked = false;")
      sScptStr.Append(vbCrLf & "    }")
      sScptStr.Append(vbCrLf & "  }")
      sScptStr.Append(vbCrLf & "</script>")

      Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "mdm-cb-onclick", sScptStr.ToString, False)

    End If

    sScptStr = Nothing

  End Sub

End Class