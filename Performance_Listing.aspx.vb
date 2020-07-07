Partial Public Class Performance_Listing
  Inherits System.Web.UI.Page
  Dim aclsData_temp As New clsData_Manager_SQL
  Private sTypeMakeModelCtrlBaseName As String = "Aircraft"
  Public productCodeCount As Integer = 0
  Public isHeliOnlyProduct As Boolean = False
  Public bUsernameExists As Boolean = False

  Dim masterPage As New Object

  Private Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

    If Session.Item("isMobile") Then
      Me.MasterPageFile = "~/EvoStyles/MobileTheme.master"
      masterPage = DirectCast(Page.Master, MobileTheme)
    Else
      Me.MasterPageFile = "~/EvoStyles/EvoTheme.master"
      masterPage = DirectCast(Page.Master, EvoTheme)
    End If

  End Sub

  Private Sub Performance_Listing_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

    Dim sErrorString As String = ""

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

      If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString), _
                                                             HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString, _
                                                             CLng(HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString), _
                                                             CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)) Then
        Response.Redirect("Default.aspx", True)
      End If

      'This code disables the form for commercial only user.
      Dim DisableCommercialUser As StringBuilder = New StringBuilder()

      If Session.Item("localSubscription").crmBusiness_Flag = False And Session.Item("localSubscription").crmHelicopter_Flag = False Then
        If Session.Item("localSubscription").crmCommercial_Flag = True Then
          If Not Page.ClientScript.IsClientScriptBlockRegistered("SwapPageDependingOnEventType") Then
            DisableCommercialUser.Append("<script type=""text/javascript"">")

            DisableCommercialUser.Append(vbCrLf & "$(document).ready(function() {")
            DisableCommercialUser.Append(vbCrLf & "$(""#" & perfSpecsTable.ClientID & """).find(""input,button,textarea,select"").attr(""disabled"", ""disabled"");")
            DisableCommercialUser.Append(vbCrLf & "$(""#" & perfSpecsTable.ClientID & """).prop(""class"", ""display_disable disableOpCosts"");")

            DisableCommercialUser.Append(vbCrLf & "$(""#" & perfSpecsTable.ClientID & """).find(""input,strong,textarea,select"").prop(""class"", ""display_disable"");")
            DisableCommercialUser.Append(vbCrLf & "$(""#" & perfspecs_make_model_panel.ClientID & """).prop(""class"", ""disabled_model_search_box searchBox nine columns"");")
            DisableCommercialUser.Append(vbCrLf & "$(""#" & performance_search.ClientID & """).prop(""class"", ""display_none"");")



            DisableCommercialUser.Append(vbCrLf & "$(""#" & reset.ClientID & """).prop(""class"", ""display_none"");")

            DisableCommercialUser.Append(vbCrLf & "});")
            DisableCommercialUser.Append("</script>")
            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "DisableCommercialUser", DisableCommercialUser.ToString, False)
          End If
        End If
      End If


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

        'This will go ahead and set up the javascript control array. Not needed unless you're going to need the array (such as to find an amod ID index) before the search button is clicked
        'Generally you won't, but on the ac listing page, you use folders and the home page market tab

        'This basically loads the array into session.
        commonEvo.fillAirframeArray("")
        commonEvo.fillAircraftTypeLableArray("")
        commonEvo.fillDefaultAirframeArray("")

        commonEvo.fillMfrNamesArray("")
        commonEvo.fillAircraftSizeArray("")

        'Setting up the project search
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
                performance_specs_folder_name.Text = FoldersTableData.Rows(0).Item("cfolder_name").ToString
              End If
              If cfolderData <> "" Then
                'Fills up the applicable folder Information pulled from the cfolder data field
                DisplayFunctions.FillUpFolderInformation(perfSpecsTable, close_current_folder, cfolderData, FolderInformation, FoldersTableData, False, False, False, False, False, Performance_Collapse_Panel, performance_actions_submenu_dropdown, Nothing, Nothing, Nothing, "", True)
              End If
            End If
          End If
        End If
      End If

      masterPage.SetDefaultButtion(Me.performance_search.UniqueID)

    End If
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

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    If Session.Item("crmUserLogon") <> True Then
      Response.Redirect("Default.aspx", False)
    Else

      aclsData_temp = New clsData_Manager_SQL
      aclsData_temp.JETNET_DB = Session.Item("jetnetClientDatabase")

      ToggleHigherLowerBar(False)
      If Not Page.IsPostBack And Page.Request.Form("complete_search") <> "Y" Then
        Initial(True)
      Else
        Initial(False)
      End If

      If Not Page.IsPostBack Then
        performance_help_text.Text = clsGeneral.clsGeneral.CreateEvoHelpLink("Performance Specs")
      End If

      ''Setting up the page:
      ViewTMMDropDowns.setIsView(False)

      ViewTMMDropDowns.setShowWeightClass(True)
      ViewTMMDropDowns.setShowMfrNames(True)
      ViewTMMDropDowns.setShowAcSize(True)

      ViewTMMDropDowns.setListSize(15)
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

      'Load Search Information:
      If Not Page.IsPostBack Then
        FillOutSearchParameters()
        If Session.Item("isMobile") Then
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

      ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
      ''''''''''''''''Some neat functions that might help'''''''''''''''''''''''''
      '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''' 
      'Pass the tab index of what you want highlighted on the bar.
      masterPage.Set_Active_Tab(5)
      'This will set page title.
      Me.Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Performance Search Results")


      'Set up bars to display correctly.
      If Not Page.IsPostBack Then
        Dim FoldersTable As New DataTable

        'Fill Folders Table
        performance_folders_submenu_dropdown.Items.Clear()
        DisplayFunctions.AddEditFolderListOptionToFolderDropdown(performance_folders_submenu_dropdown, 12)
        FoldersTable = masterPage.aclsData_Temp.GetEvolutionFolderssBySubscription(0, Session.Item("localUser").crmUserLogin, Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmSubSeqNo, "", 12, Nothing, "")
        If Not IsNothing(FoldersTable) Then
          If FoldersTable.Rows.Count > 0 Then
            For Each r As DataRow In FoldersTable.Rows
              If Not IsDBNull(r("cfolder_data")) Then
                Dim FolderDataString As Array
                'this was added to parse out the real search query now that we're saving it
                FolderDataString = Split(r("cfolder_data"), "THEREALSEARCHQUERY")
                performance_folders_submenu_dropdown.Items.Add(New ListItem(r("cfolder_name").ToString, "javascript:ParseSpecsOperatingMarketForm('" & r("cfolder_id").ToString & "',true,false,false,'" & Replace(FolderDataString(0), "'", "\'") & "');"))
              End If
            Next
          End If
        End If
      End If

    End If

    us_standard.Attributes.Add("onclick", "muxStandard();toggleMetricLabels();")
    metric_standard.Attributes.Add("onclick", "muxMetric();toggleMetricLabels();")
    add_MuxDisplayUnits_Script(us_standard, metric_standard)
    add_ToggleMetric_Script(us_standard, Div0, Div1, Div2, Div3, Div4, Div5, nautical_miles, statute_miles)

    nautical_miles.Attributes.Add("onclick", "muxNauticalMiles();toggleMilesLabels();")
    statute_miles.Attributes.Add("onclick", "muxStatuteMiles();toggleMilesLabels();")
    add_MuxDisplayMiles_Script(nautical_miles, statute_miles)
    add_ToggleNauticalMiles_Script(nautical_miles, Div1)

  End Sub

  Private Sub Performance_Spec_Search(ByVal Make_String As String, ByVal Model_String As String, ByVal Model_Type_String As String, ByVal Airframe_Type_String As String, _
                                      ByVal UseMetric As Boolean, ByVal UseStatute As Boolean, ByVal WeightClass As String, ByVal ManufacturerName As String, ByVal AcSize As String, _
                                      ByVal TakeOffSL As String, ByVal TakeOffSLOperator As String, ByVal MaxRange As String, ByVal MaxRangeOperator As String, _
                                      ByVal FuseLength As String, ByVal FuseLengthOperator As String, ByVal FuseHeight As String, ByVal FuseHeightOperator As String, _
                                      ByVal FuseWing As String, ByVal FuseWingOperator As String, ByVal Crew As String, ByVal CrewOperator As String, _
                                      ByVal Passengers As String, ByVal PassengersOperator As String, ByVal MaxTakeOff As String, ByVal MaxTakeOffOperator As String, _
                                      ByVal NormalCruise As String, ByVal NormalCruiseOperator As String, ByVal FuelCapacity As String, ByVal FuelCapacityOperator As String, _
                                      ByVal Helicopter As Boolean, ByVal Business As Boolean, ByVal Commercial As Boolean)
    Try
      Dim Results_Table As New DataTable
      performance_attention.Text = ""
      container_performance_listing.CssClass = "performance_container_content"

      Dim DisplayRType As Boolean = False
      Dim DisplayFType As Boolean = False
      Dim ListofACIDs As String = ""
      Dim DisplayMixedType As Boolean = False
      Dim ResultDisplayForListing As String = ""
      Dim PerformanceSearchCriteria As New viewSelectionCriteriaClass
      Dim tmpViewObj As New viewsDataLayer


      tmpViewObj.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      tmpViewObj.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim

      masterPage.SetStatusText(HttpContext.Current.Session.Item("SearchString"))

      Results_Table = PerformanceSpecSearch(Model_String, Model_Type_String, Airframe_Type_String, Make_String, _
                                            WeightClass, ManufacturerName, AcSize, TakeOffSL, TakeOffSLOperator, MaxRange, MaxRangeOperator, _
                                            FuseLength, FuseLengthOperator, FuseHeight, FuseHeightOperator, FuseWing, FuseWingOperator, _
                                            Crew, CrewOperator, Passengers, PassengersOperator, MaxTakeOff, MaxTakeOffOperator, _
                                            NormalCruise, NormalCruiseOperator, FuelCapacity, FuelCapacityOperator, _
                                            Helicopter, Business, Commercial, UseMetric, UseStatute)

      Call commonLogFunctions.Log_User_Event_Data("UserSearch", "Performance Search: " & clsGeneral.clsGeneral.StripChars(clsGeneral.clsGeneral.stripHTML(Replace(HttpContext.Current.Session.Item("SearchString"), "<br />", " ")), False), Nothing, 0, 0, 0, 0, 0, 0, 0)

      If Not IsNothing(Results_Table) Then

        Session.Item("localUser").crmLatestRecordCount = Results_Table.Rows.Count
        If Results_Table.Rows.Count > 0 Then

          If Results_Table.Rows.Count > 1 Then

            For Each r As DataRow In Results_Table.Rows
              If String.IsNullOrEmpty(ListofACIDs.Trim) Then
                ListofACIDs = r.Item("amod_id").ToString
              Else
                ListofACIDs += crmWebClient.Constants.cCommaDelim + r.Item("amod_id").ToString
              End If
            Next

            DisplayMixedType = commonEvo.check_for_multi_airframes(Results_Table)

            If Not DisplayMixedType Then
              PerformanceSearchCriteria.ViewCriteriaAirframeTypeStr = Results_Table.Rows(0).Item("amod_airframe_type_code").ToString.ToUpper.Trim
            End If

            PerformanceSearchCriteria.ViewCriteriaAmodID = -1 ' clear any single model id
            PerformanceSearchCriteria.ViewCriteriaAmodIDArray = Split(ListofACIDs, crmWebClient.Constants.cCommaDelim)
          Else
            If Results_Table.Rows.Count = 1 Then
              PerformanceSearchCriteria.ViewCriteriaAirframeTypeStr = Results_Table.Rows(0).Item("amod_airframe_type_code").ToString.ToUpper.Trim
              PerformanceSearchCriteria.ViewCriteriaAmodID = CLng(Results_Table.Rows(0).Item("amod_id").ToString)
              PerformanceSearchCriteria.ViewCriteriaAmodIDArray = Nothing ' clear any model list
            End If
          End If

          PerformanceSearchCriteria.ViewCriteriaUseMetricValues = UseMetric
          PerformanceSearchCriteria.ViewCriteriaUseStatuteMiles = UseStatute


          'This would just be for the variable for the search listing.
          performance_listing_text.Text = ""

          If Session.Item("isMobile") = True Then
            performance_listing_text.Text = "<h1>Performance Specs</h1>"
          End If

          performance_listing_text.Text += "<table cellspacing='0' cellpadding='0' class='data_aircraft_grid cell_right performanceTable mobileWidth' " & IIf(UseMetric, "id=""metricTable""", "id=""standardTable""") & ">"
          performance_listing_text.Text += "<tr>"

          tmpViewObj.views_display_performance_specs(False, "Html", True, DisplayMixedType, PerformanceSearchCriteria, ResultDisplayForListing)
          performance_listing_text.Text += ResultDisplayForListing

          tmpViewObj.views_display_performance_specs(False, "Html", False, DisplayMixedType, PerformanceSearchCriteria, ResultDisplayForListing)
          performance_listing_text.Text += ResultDisplayForListing

          'End Display for Search
          performance_listing_text.Text += "</tr>"
          performance_listing_text.Text += "</table>"

          If Session.Item("isMobile") = True Then
            'Second table toggle:
            'This would just be for the variable for the search listing.
            performance_listing_text.Text += "<table cellspacing='0' cellpadding='0' style=""display:none;"" class='data_aircraft_grid cell_right performanceTable mobileWidth' " & IIf(UseMetric, "id=""metricTable""", "id=""standardTable""") & ">"
            performance_listing_text.Text += "<tr>"
            PerformanceSearchCriteria.ViewCriteriaUseMetricValues = IIf(UseMetric, False, True) 'opposite

            tmpViewObj.views_display_performance_specs(False, "Html", True, DisplayMixedType, PerformanceSearchCriteria, ResultDisplayForListing)
            performance_listing_text.Text += ResultDisplayForListing

            tmpViewObj.views_display_performance_specs(False, "Html", False, DisplayMixedType, PerformanceSearchCriteria, ResultDisplayForListing)
            performance_listing_text.Text += ResultDisplayForListing

            'Reset
            PerformanceSearchCriteria.ViewCriteriaUseMetricValues = UseMetric
            'End Display for Search
            performance_listing_text.Text += "</tr>"
            performance_listing_text.Text += "</table>"

          End If


        Else
          performance_listing_text.Text = "<br /><br /><p align='center' class='red_text'><b>Your search returned no results.</p><br /><br />"
        End If
      Else
        'There was an error here. Let's record it.
        'And that there was an error on the data side.
        masterPage.LogError("Performance_Spec_Search (performance_listing.aspx): " & aclsData_temp.class_error)


        performance_attention.Text = "<br /><p class='padding'>We're sorry, an error has occurred during your search.</b></p><br /><br />"
        If (InStr(UCase(Session.Item("localUser").crmLocalUserName), "JETNET.COM") > 0) Or (InStr(UCase(Session.Item("localUser").crmLocalUserName), "MVINTECH.COM") > 0) Then
          performance_attention.Text += aclsData_temp.class_error
        End If

        aclsData_temp.class_error = ""


      End If

    Catch ex As Exception
      'There was an error here. Let's record it.
      'And that there was an error on the data side.
      masterPage.LogError("Performance_Spec_Search (performance_listing.aspx): " & ex.Message)


      performance_attention.Text = "<br /><p class='padding'>We're sorry, an error has occurred during your search.</b></p><br /><br />"
      If (InStr(UCase(Session.Item("localUser").crmLocalUserName), "JETNET.COM") > 0) Or (InStr(UCase(Session.Item("localUser").crmLocalUserName), "MVINTECH.COM") > 0) Then
        performance_attention.Text += ex.Message
      End If

    End Try

  End Sub

  Public Sub ToggleHigherLowerBar(ByVal lower_bar As Boolean)
    'setting the javascript of the menus
    If lower_bar = True Then
      PerformancePanelEx.Enabled = False
      PerformancePanelEx.Collapsed = True
      PerformancePanelEx.ClientState = True
      performance_search_expand_text.Visible = False
      performance_help_text.Visible = False
    End If

    'actions dropdown
    performance_actions_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & performance_actions_submenu_dropdown.ClientID & "', true);")
    performance_actions_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & performance_actions_submenu_dropdown.ClientID & "', false);")

    performance_actions_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & performance_actions_submenu_dropdown.ClientID & "', true);")
    performance_actions_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & performance_actions_submenu_dropdown.ClientID & "', false);")

    'folder dropdown
    performance_folders_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & performance_folders_submenu_dropdown.ClientID & "', true);")
    performance_folders_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & performance_folders_submenu_dropdown.ClientID & "', false);")

    performance_folders_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & performance_folders_submenu_dropdown.ClientID & "', true);")
    performance_folders_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & performance_folders_submenu_dropdown.ClientID & "', false);")


  End Sub

  Private Sub performance_search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles performance_search.Click
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


    Dim TakeOffSL As String = ""
    Dim TakeOffSLOperator As String = ""

    Dim MaxRange As String = ""
    Dim MaxRangeOperator As String = ""

    Dim FuseLength As String = ""
    Dim FuseLengthOperator As String = ""
    Dim FuseHeight As String = ""
    Dim FuseHeightOperator As String = ""
    Dim FuseWing As String = ""
    Dim FuseWingOperator As String = ""

    Dim Crew As String = ""
    Dim CrewOperator As String = ""
    Dim Passengers As String = ""
    Dim PassengersOperator As String = ""

    Dim MaxTakeOff As String = ""
    Dim MaxTakeOffOperator As String = ""

    Dim NormalCruise As String = ""
    Dim NormalCruiseOperator As String = ""

    Dim FuelCapacity As String = ""
    Dim FuelCapacityOperator As String = ""

    Dim Business As Boolean = False
    Dim Helicopter As Boolean = False
    Dim Commercial As Boolean = False

    'We're going to go ahead and set a new search class,
    'but if one exists in session, we're using that one first
    Dim NewSearchClass As SearchSelectionCriteria
    If Not IsNothing(HttpContext.Current.Session.Item("searchCriteria")) Then
      NewSearchClass = HttpContext.Current.Session.Item("searchCriteria")
    Else
      NewSearchClass = New SearchSelectionCriteria
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

    PerformancePanelEx.Collapsed = True
    PerformancePanelEx.ClientState = True

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

    'Fuselage Length
    If Not String.IsNullOrEmpty(fuselage_length_txt.Text) Then
      FuseLength = Replace(fuselage_length_txt.Text, ",", "")
      FuseLengthOperator = fuselage_length_ddl.SelectedValue
      BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(fuselage_length_txt, "Fuselage Length " & FuseLengthOperator)
    End If

    NewSearchClass.SearchCriteriaPerfSpecsFuselageLength = FuseLength
    NewSearchClass.SearchCriteriaPerfSpecsFuselageLengthOperator = FuseLengthOperator

    'Fuselage Height
    If Not String.IsNullOrEmpty(fuselage_height_txt.Text) Then
      FuseHeight = Replace(fuselage_height_txt.Text, ",", "")
      FuseHeightOperator = fuselage_height_ddl.SelectedValue
      BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(fuselage_height_txt, "Fuselage Height " & FuseHeightOperator)
    End If


    NewSearchClass.SearchCriteriaPerfSpecsFuselageHeight = FuseHeight
    NewSearchClass.SearchCriteriaPerfSpecsFuselageHeightOperator = FuseHeightOperator

    'Max Range
    If Not String.IsNullOrEmpty(maxrange_txt.Text) Then
      MaxRange = Replace(maxrange_txt.Text, ",", "")
      MaxRangeOperator = maxrange_ddl.SelectedValue
      If Helicopter Then
        BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(maxrange_txt, "Max Range Tanks Full " & MaxRangeOperator)
      End If
      If Commercial Or Business Then
        BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(maxrange_txt, "Max Range " & MaxRangeOperator)
      End If

    End If

    NewSearchClass.SearchCriteriaPerfSpecsMaxRangeNBAAOrTanksFull = MaxRange
    NewSearchClass.SearchCriteriaPerfSpecsMaxRangeNBAAOrTanksFullOperator = MaxRangeOperator

    'Fuselage Wing
    If Not String.IsNullOrEmpty(fuselage_wing_txt.Text) Then
      FuseWing = Replace(fuselage_wing_txt.Text, ",", "")
      FuseWingOperator = fuselage_wing_ddl.SelectedValue
      If Helicopter Then
        BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(fuselage_wing_txt, "Fuselage Width " & FuseWingOperator)
      End If
      If Commercial Or Business Then
        BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(fuselage_wing_txt, "Fuselage Wingspan " & FuseWingOperator)
      End If

    End If

    NewSearchClass.SearchCriteriaPerfSpecsWingSpanOrWidth = FuseWing
    NewSearchClass.SearchCriteriaPerfSpecsWingSpanOrWidthOperator = FuseWingOperator

    'SL ISA BFL
    If Not String.IsNullOrEmpty(takeoff_sl_txt.Text) Then
      TakeOffSL = Replace(takeoff_sl_txt.Text, ",", "")
      TakeOffSLOperator = takeoff_sl_ddl.SelectedValue
      BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(takeoff_sl_txt, "SL ISA BFL " & TakeOffSLOperator)
    End If

    NewSearchClass.SearchCriteriaPerfSpecsSLISA = TakeOffSL
    NewSearchClass.SearchCriteriaPerfSpecsSLISAOperator = TakeOffSLOperator

    'Crew
    If Not String.IsNullOrEmpty(crew_txt.Text) Then
      Crew = Replace(crew_txt.Text, ",", "")
      CrewOperator = crew_ddl.SelectedValue
      BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(crew_txt, "Crew " & CrewOperator)
    End If

    NewSearchClass.SearchCriteriaPerfSpecsCrew = Crew
    NewSearchClass.SearchCriteriaPerfSpecsCrewOperator = CrewOperator


    'Passengers
    If Not String.IsNullOrEmpty(passengers_txt.Text) Then
      Passengers = Replace(passengers_txt.Text, ",", "")
      PassengersOperator = passengers_ddl.SelectedValue
      BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(passengers_txt, "Passengers " & PassengersOperator)
    End If

    NewSearchClass.SearchCriteriaPerfSpecsPassengers = Passengers
    NewSearchClass.SearchCriteriaPerfSpecsPassengersOperator = PassengersOperator

    'Max Takeoff
    If Not String.IsNullOrEmpty(max_takeoff_txt.Text) Then
      MaxTakeOff = Replace(max_takeoff_txt.Text, ",", "")
      MaxTakeOffOperator = max_takeoff_ddl.SelectedValue
      BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(max_takeoff_txt, "Max Takeoff " & MaxTakeOffOperator)
    End If

    NewSearchClass.SearchCriteriaPerfSpecsMaxTakeoff = MaxTakeOff
    NewSearchClass.SearchCriteriaPerfSpecsMaxTakeoffOperator = MaxTakeOffOperator

    'Normal Cruise
    If Not String.IsNullOrEmpty(cruise_speed_txt.Text) Then
      NormalCruise = Replace(cruise_speed_txt.Text, ",", "")
      NormalCruiseOperator = cruise_speed_ddl.SelectedValue
      BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(cruise_speed_txt, "Normal Cruise " & NormalCruiseOperator)
    End If

    NewSearchClass.SearchCriteriaPerfSpecsNormalCruise = NormalCruise
    NewSearchClass.SearchCriteriaPerfSpecsNormalCruiseOperator = NormalCruiseOperator

    'Normal Cruise
    If Not String.IsNullOrEmpty(cruise_speed_txt.Text) Then
      NormalCruise = Replace(cruise_speed_txt.Text, ",", "")
      NormalCruiseOperator = cruise_speed_ddl.SelectedValue
      BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(cruise_speed_txt, "Normal Cruise " & NormalCruiseOperator)
    End If

    NewSearchClass.SearchCriteriaPerfSpecsNormalCruise = NormalCruise
    NewSearchClass.SearchCriteriaPerfSpecsNormalCruiseOperator = NormalCruiseOperator

    'Fuel Capacity
    If Not String.IsNullOrEmpty(fuel_capacity_txt.Text) Then
      FuelCapacity = Replace(fuel_capacity_txt.Text, ",", "")
      FuelCapacityOperator = fuel_capacity_ddl.SelectedValue
      BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(fuel_capacity_txt, "Fuel Capacity " & FuelCapacityOperator)
    End If

    NewSearchClass.SearchCriteriaPerfSpecsFuelCapacity = FuelCapacity
    NewSearchClass.SearchCriteriaPerfSpecsFuelCapacityOperator = FuelCapacityOperator

    If metric_standard.Checked Then
      NewSearchClass.SearchCriteriaDisplayUnits = "M"
    Else
      NewSearchClass.SearchCriteriaDisplayUnits = ""
    End If

    If statute_miles.Checked Then
      NewSearchClass.SearchCriteriaDisplayMiles = "S"
    Else
      NewSearchClass.SearchCriteriaDisplayMiles = ""
    End If

    'set up top text
    HttpContext.Current.Session.Item("SearchString") = BuildSearchString
    HttpContext.Current.Session.Item("searchCriteria") = NewSearchClass

    Initial(False)
    Performance_Spec_Search(MakeString, ModelsString, TypeString, AirframeTypeString, _
                            IIf(metric_standard.Checked, True, False), IIf(statute_miles.Checked, True, False), _
                            WeightClass, ManufacturerStr, AcSizeStr, TakeOffSL, TakeOffSLOperator, MaxRange, MaxRangeOperator, _
                            FuseLength, FuseLengthOperator, FuseHeight, FuseHeightOperator, FuseWing, FuseWingOperator, _
                            Crew, CrewOperator, Passengers, PassengersOperator, MaxTakeOff, MaxTakeOffOperator, _
                            NormalCruise, NormalCruiseOperator, FuelCapacity, FuelCapacityOperator, Helicopter, Business, Commercial)

  End Sub

  Public Sub Initial(ByVal initial_page_load As Boolean)

    If initial_page_load = True Then

      PerformancePanelEx.Collapsed = False
      PerformancePanelEx.ClientState = False
      performance_actions_dropdown.Visible = False

      performance_actions_submenu_dropdown.Items.Add(New ListItem("Save As - New Folder", "javascript:SubMenuDrop(3,0, 'PERFORMANCE SPECS');"))
      performance_actions_submenu_dropdown.Items.Add(New ListItem("Custom Export", "javascript:SubMenuDrop(1,0,'PERFORMANCE SPECS');"))
      performance_actions_submenu_dropdown.Items.Add(New ListItem("JETNET Export/Report", "javascript:SubMenuDrop(5,0,'PERFORMANCE SPECS');"))
      performance_actions_submenu_dropdown.Items.Add(New ListItem("Summary", "javascript:SubMenuDrop(2,0,'PERFORMANCE SPECS');"))


    Else

      PerformancePanelEx.Collapsed = True
      PerformancePanelEx.ClientState = True
      performance_actions_dropdown.Visible = True

    End If

    If Session.Item("isMobile") Then
      performance_actions_dropdown.CssClass = "display_none"
      performance_actions_submenu_dropdown.CssClass = "display_none"
      performance_folders_dropdown.CssClass = "display_none"
      performance_folders_submenu_dropdown.CssClass = "display_none"
    End If
  End Sub

  Private Sub ResetPage()
    ClearSelections()
    Response.Redirect("Performance_Listing.aspx")
  End Sub

  Private Sub reset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles reset.Click
    ResetPage()
  End Sub

  Private Sub FillOutSearchParameters()
    Try
      'Filling Back in the Search Criteria.
      'Performance Costs

      'SLI Operator
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaPerfSpecsSLISAOperator) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaPerfSpecsSLISAOperator) Then
          takeoff_sl_ddl.SelectedValue = Session.Item("searchCriteria").SearchCriteriaPerfSpecsSLISAOperator.ToString
        End If
      End If

      'SLI Value
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaPerfSpecsSLISA) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaPerfSpecsSLISA) Then
          takeoff_sl_txt.Text = Session.Item("searchCriteria").SearchCriteriaPerfSpecsSLISA.ToString
        End If
      End If

      'Max Range Operator
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaPerfSpecsMaxRangeNBAAOrTanksFullOperator) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaPerfSpecsMaxRangeNBAAOrTanksFullOperator) Then
          maxrange_ddl.SelectedValue = Session.Item("searchCriteria").SearchCriteriaPerfSpecsMaxRangeNBAAOrTanksFullOperator.ToString
        End If
      End If

      'Max Range Value
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaPerfSpecsMaxRangeNBAAOrTanksFull) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaPerfSpecsMaxRangeNBAAOrTanksFull) Then
          maxrange_txt.Text = Session.Item("searchCriteria").SearchCriteriaPerfSpecsMaxRangeNBAAOrTanksFull.ToString
        End If
      End If

      'Fuselage Length Operator
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaPerfSpecsFuselageLengthOperator) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaPerfSpecsFuselageLengthOperator) Then
          fuselage_length_ddl.SelectedValue = Session.Item("searchCriteria").SearchCriteriaPerfSpecsFuselageLengthOperator.ToString
        End If
      End If

      'Fuselage Length Value
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaPerfSpecsFuselageLength) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaPerfSpecsFuselageLength) Then
          fuselage_length_txt.Text = Session.Item("searchCriteria").SearchCriteriaPerfSpecsFuselageLength.ToString
        End If
      End If

      'Fuselage Height Operator
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaPerfSpecsFuselageHeightOperator) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaPerfSpecsFuselageHeightOperator) Then
          fuselage_height_ddl.SelectedValue = Session.Item("searchCriteria").SearchCriteriaPerfSpecsFuselageHeightOperator.ToString
        End If
      End If

      'Fuselage Height Value
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaPerfSpecsFuselageHeight) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaPerfSpecsFuselageHeight) Then
          fuselage_height_txt.Text = Session.Item("searchCriteria").SearchCriteriaPerfSpecsFuselageHeight.ToString
        End If
      End If

      'Fuselage Wing Span or Width Operator
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaPerfSpecsWingSpanOrWidthOperator) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaPerfSpecsWingSpanOrWidthOperator) Then
          fuselage_wing_ddl.SelectedValue = Session.Item("searchCriteria").SearchCriteriaPerfSpecsWingSpanOrWidthOperator.ToString
        End If
      End If

      'Fuselage Wing Span or Width Value
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaPerfSpecsWingSpanOrWidth) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaPerfSpecsWingSpanOrWidth) Then
          fuselage_wing_txt.Text = Session.Item("searchCriteria").SearchCriteriaPerfSpecsWingSpanOrWidth.ToString
        End If
      End If

      'Crew Operator
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaPerfSpecsCrewOperator) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaPerfSpecsCrewOperator) Then
          crew_ddl.SelectedValue = Session.Item("searchCriteria").SearchCriteriaPerfSpecsCrewOperator.ToString
        End If
      End If

      'Crew Value
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaPerfSpecsCrew) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaPerfSpecsCrew) Then
          crew_txt.Text = Session.Item("searchCriteria").SearchCriteriaPerfSpecsCrew.ToString
        End If
      End If

      'Passengers Operator
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaPerfSpecsPassengersOperator) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaPerfSpecsPassengersOperator) Then
          passengers_ddl.SelectedValue = Session.Item("searchCriteria").SearchCriteriaPerfSpecsPassengersOperator.ToString
        End If
      End If

      'Passengers Value
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaPerfSpecsPassengers) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaPerfSpecsPassengers) Then
          passengers_txt.Text = Session.Item("searchCriteria").SearchCriteriaPerfSpecsPassengers.ToString
        End If
      End If

      'Max Takeoff Operator
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaPerfSpecsMaxTakeoffOperator) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaPerfSpecsMaxTakeoffOperator) Then
          max_takeoff_ddl.SelectedValue = Session.Item("searchCriteria").SearchCriteriaPerfSpecsMaxTakeoffOperator.ToString
        End If
      End If

      'Max Takeoff Value
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaPerfSpecsMaxTakeoff) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaPerfSpecsMaxTakeoff) Then
          max_takeoff_txt.Text = Session.Item("searchCriteria").SearchCriteriaPerfSpecsMaxTakeoff.ToString
        End If
      End If

      'Normal Cruise Operator
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaPerfSpecsNormalCruiseOperator) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaPerfSpecsNormalCruiseOperator) Then
          cruise_speed_ddl.SelectedValue = Session.Item("searchCriteria").SearchCriteriaPerfSpecsNormalCruiseOperator.ToString
        End If
      End If

      'Normal Cruise Value
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaPerfSpecsNormalCruise) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaPerfSpecsNormalCruise) Then
          cruise_speed_txt.Text = Session.Item("searchCriteria").SearchCriteriaPerfSpecsNormalCruise.ToString
        End If
      End If

      'Display Units
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaDisplayUnits) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaDisplayUnits) Then
          If Session.Item("searchCriteria").SearchCriteriaDisplayUnits.ToString = "M" Then
            metric_standard.Checked = True
            us_standard.Checked = False
          Else
            metric_standard.Checked = False
            us_standard.Checked = True
          End If
        End If
      End If

      'Display Miles
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaDisplayMiles) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaDisplayMiles) Then
          If Session.Item("searchCriteria").SearchCriteriaDisplayMiles.ToString = "S" Then
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

  Private Sub Performance_Listing_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete

    If Page.Request.Form("project_search") = "Y" Then
      'if either of these variables is passed, then go ahead and complete this search.
      performance_search_Click(performance_search, EventArgs.Empty)
    End If
  End Sub

  Private Sub makeModelDynamic_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles makeModelDynamic.SelectedIndexChanged
    Initial(True)

    If Not String.IsNullOrEmpty(makeModelDynamic.SelectedValue) Then
      Dim ModelData As Array = Split(makeModelDynamic.SelectedValue, "|")
      If UBound(ModelData) = 3 Then
        HttpContext.Current.Session.Item("tabAircraftType") = commonEvo.FindIndexForFirstItem(UCase(ModelData(0)), crmWebClient.Constants.AIRFRAME_TYPE, ModelData(1), crmWebClient.Constants.AIRFRAME_FRAME).ToString()
        HttpContext.Current.Session.Item("tabAircraftMake") = commonEvo.FindIndexForFirstItem(UCase(ModelData(2)), crmWebClient.Constants.AIRFRAME_MAKE, ModelData(1), crmWebClient.Constants.AIRFRAME_FRAME)
        HttpContext.Current.Session.Item("tabAircraftModel") = commonEvo.FindIndexForItemByAmodID(CLng(ModelData(3)))

        Performance_Spec_Search(UCase(ModelData(2)), CLng(ModelData(3)), UCase(ModelData(0)), ModelData(1), _
                                IIf(metric_standard.Checked, True, False), IIf(statute_miles.Checked, True, False), _
                                "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", _
                                Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, _
                                Session.Item("localSubscription").crmCommercial_Flag)

      End If
    End If


  End Sub

  Private Function PerformanceSpecSearch(ByVal amod_ids As String, ByVal modelType As String, ByVal AirframeType As String, ByVal MakeString As String, _
                                         ByVal WeightClass As String, ByVal ManufacturerName As String, ByVal AcSize As String, _
                                         ByVal TakeOffSL As String, ByVal TakeOffSLOperator As String, ByVal MaxRange As String, _
                                         ByVal MaxRangeOperator As String, ByVal FuseLength As String, ByVal FuseLengthOperator As String, _
                                         ByVal FuseHeight As String, ByVal FuseHeightOperator As String, ByVal FuseWing As String, _
                                         ByVal FuseWingOperator As String, ByVal Crew As String, ByVal CrewOperator As String, _
                                         ByVal Passengers As String, ByVal PassengersOperator As String, ByVal MaxTakeOff As String, _
                                         ByVal MaxTakeOffOperator As String, ByVal NormalCruise As String, ByVal NormalCruiseOperator As String, _
                                         ByVal FuelCapacity As String, ByVal FuelCapacityOperator As String, _
                                         ByVal Helicopter As Boolean, ByVal Business As Boolean, ByVal Commercial As Boolean, ByVal UseMetric As Boolean, ByVal UseStatute As Boolean) As DataTable

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim TempTable As New DataTable
    Dim query As String = ""
    Dim sqlwhere As String = ""
    Try

      '  If amod_ids <> "" And performance Or performance = False Then
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

      'Takeoff ALI
      If TakeOffSL <> "" Then
        If sqlwhere <> "" Then
          sqlwhere += " and "
        End If
        If Not UseMetric Then
          sqlwhere += " (amod_takeoff_ali " & clsGeneral.clsGeneral.PrepQueryString(TakeOffSLOperator, TakeOffSL, "Numeric", False, "amod_takeoff_ali", True) & ") "
        Else
          ' convert TakeOffSL from feet to meters
          Dim conversionConstant = ConversionFunctions.ReturnMetricConversonConstant("FT")

          sqlwhere += " ( (amod_takeoff_ali * " + conversionConstant.ToString + ")" & clsGeneral.clsGeneral.PrepQueryString(TakeOffSLOperator, TakeOffSL, "Numeric", False, "amod_takeoff_ali", True) & ") "
        End If
      End If

      'Fuselage Length
      If FuseLength <> "" Then
        If sqlwhere <> "" Then
          sqlwhere += " and "
        End If
        If Not UseMetric Then
          sqlwhere += " (amod_fuselage_length " & clsGeneral.clsGeneral.PrepQueryString(FuseLengthOperator, FuseLength, "Numeric", False, "amod_fuselage_length", True) & ") "
        Else
          Dim conversionConstant = ConversionFunctions.ReturnMetricConversonConstant("FT")
          ' convert FuseLength from feet to meters
          sqlwhere += " ( (amod_fuselage_length * " + conversionConstant.ToString + ")" & clsGeneral.clsGeneral.PrepQueryString(FuseLengthOperator, FuseLength, "Numeric", False, "amod_fuselage_length", True) & ") "
        End If
      End If

      'Fuselage Height
      If FuseHeight <> "" Then
        If sqlwhere <> "" Then
          sqlwhere += " and "
        End If
        If Not UseMetric Then
          sqlwhere += " (amod_fuselage_height " & clsGeneral.clsGeneral.PrepQueryString(FuseHeightOperator, FuseHeight, "Numeric", False, "amod_fuselage_height", True) & ") "
        Else
          Dim conversionConstant = ConversionFunctions.ReturnMetricConversonConstant("FT")
          ' convert FuseHeight from feet to meters
          sqlwhere += " ( (amod_fuselage_height * " + conversionConstant.ToString + ")" & clsGeneral.clsGeneral.PrepQueryString(FuseHeightOperator, FuseHeight, "Numeric", False, "amod_fuselage_height", True) & ") "
        End If
      End If

      'Fuselage Wing
      If FuseWing <> "" Then
        If sqlwhere <> "" Then
          sqlwhere += " and "
        End If

        sqlwhere += " ( "

        If Commercial = True Or Business = True Then
          If Not UseMetric Then
            sqlwhere += " (amod_fuselage_wingspan " & clsGeneral.clsGeneral.PrepQueryString(FuseWingOperator, FuseWing, "Numeric", False, "amod_fuselage_wingspan", True) & ") "
          Else
            Dim conversionConstant = ConversionFunctions.ReturnMetricConversonConstant("FT")
            ' convert FuseWing from feet to meters
            sqlwhere += " ( (amod_fuselage_wingspan * " + conversionConstant.ToString + ")" & clsGeneral.clsGeneral.PrepQueryString(FuseWingOperator, FuseWing, "Numeric", False, "amod_fuselage_wingspan", True) & ") "
          End If
        End If

        If Helicopter = True Then
          If Commercial = True Or Business = True Then
            sqlwhere += " or "
          End If
          If Not UseMetric Then
            sqlwhere += " (amod_fuselage_width " & clsGeneral.clsGeneral.PrepQueryString(FuseWingOperator, FuseWing, "Numeric", False, "amod_fuselage_width", True) & ") "
          Else
            Dim conversionConstant = ConversionFunctions.ReturnMetricConversonConstant("FT")
            ' convert FuseWing from feet to meters
            sqlwhere += " ( (amod_fuselage_width * " + conversionConstant.ToString + ")" & clsGeneral.clsGeneral.PrepQueryString(FuseWingOperator, FuseWing, "Numeric", False, "amod_fuselage_width", True) & ") "
          End If
        End If
        sqlwhere += " ) "
      End If

      If MaxRange <> "" Then
        If sqlwhere <> "" Then
          sqlwhere += " and "
        End If

        sqlwhere += " ( "

        If Commercial = True Or Business = True Then
          If Not UseMetric Then
            If UseStatute Then
              sqlwhere += " (amod_max_range_miles " & clsGeneral.clsGeneral.PrepQueryString(MaxRangeOperator, MaxRange, "Numeric", False, "amod_max_range_miles", True) & ") "
            Else
              ' convert MaxRange from statute miles to nautical miles
              Dim conversionConstant = ConversionFunctions.ReturnNauticalStatuteConversonConstant("NM")
              sqlwhere += " ( (amod_max_range_miles * " + conversionConstant.ToString + ")" & clsGeneral.clsGeneral.PrepQueryString(MaxRangeOperator, MaxRange, "Numeric", False, "amod_max_range_miles", True) & ") "
            End If
          Else
            ' convert MaxRange from nautical miles to kilometers
            Dim conversionConstant = ConversionFunctions.ReturnMetricConversonConstant("NM")
            sqlwhere += " ( (amod_max_range_miles * " + conversionConstant.ToString + ")" & clsGeneral.clsGeneral.PrepQueryString(MaxRangeOperator, MaxRange, "Numeric", False, "amod_max_range_miles", True) & ") "
          End If
        End If

        If Helicopter = True Then
          If Commercial = True Or Business = True Then
            sqlwhere += " or "
          End If
          If Not UseMetric Then
            If UseStatute Then
              sqlwhere += " (amod_range_tanks_full " & clsGeneral.clsGeneral.PrepQueryString(MaxRangeOperator, MaxRange, "Numeric", False, "amod_range_tanks_full", True) & ") "
            Else
              ' convert MaxRange from statute miles to nautical miles
              Dim conversionConstant = ConversionFunctions.ReturnNauticalStatuteConversonConstant("NM")
              sqlwhere += " ( (amod_range_tanks_full * " + conversionConstant.ToString + ")" & clsGeneral.clsGeneral.PrepQueryString(MaxRangeOperator, MaxRange, "Numeric", False, "amod_range_tanks_full", True) & ") "
            End If
          Else
            ' convert MaxRange from nautical miles to kilometers
            Dim conversionConstant = ConversionFunctions.ReturnMetricConversonConstant("NM")
            sqlwhere += " ( (amod_range_tanks_full * " + conversionConstant.ToString + ")" & clsGeneral.clsGeneral.PrepQueryString(MaxRangeOperator, MaxRange, "Numeric", False, "amod_range_tanks_full", True) & ") "
          End If
        End If
        sqlwhere += " ) "
      End If

      'Crew
      If Crew <> "" Then
        If sqlwhere <> "" Then
          sqlwhere += " and "
        End If
        sqlwhere += " (amod_number_of_crew " & clsGeneral.clsGeneral.PrepQueryString(CrewOperator, Crew, "Numeric", False, "amod_number_of_crew", True) & ") "
      End If

      'Passengers
      If Passengers <> "" Then
        If sqlwhere <> "" Then
          sqlwhere += " and "
        End If
        sqlwhere += " (amod_number_of_passengers " & clsGeneral.clsGeneral.PrepQueryString(PassengersOperator, Passengers, "Numeric", False, "amod_number_of_passengers", True) & ") "
      End If

      'Max Takeoff
      If MaxTakeOff <> "" Then
        If sqlwhere <> "" Then
          sqlwhere += " and "
        End If
        If Not UseMetric Then
          sqlwhere += " (amod_max_takeoff_weight " & clsGeneral.clsGeneral.PrepQueryString(MaxTakeOffOperator, MaxTakeOff, "Numeric", False, "amod_max_takeoff_weight", True) & ") "
        Else
          ' convert MaxTakeOff from pounds to kilograms
          Dim conversionConstant = ConversionFunctions.ReturnMetricConversonConstant("LBS")
          sqlwhere += " ( (amod_max_takeoff_weight * " + conversionConstant.ToString + ")" & clsGeneral.clsGeneral.PrepQueryString(MaxTakeOffOperator, MaxTakeOff, "Numeric", False, "amod_max_takeoff_weight", True) & ") "
        End If
      End If

      'Normal Cruise
      If NormalCruise <> "" Then
        If sqlwhere <> "" Then
          sqlwhere += " and "
        End If
        If Not UseMetric Then
          sqlwhere += " (amod_cruis_speed " & clsGeneral.clsGeneral.PrepQueryString(NormalCruiseOperator, NormalCruise, "Numeric", False, "amod_cruis_speed", True) & ") "
        Else
          ' convert NormalCruise from knots per hour to kilometers per hour
          Dim conversionConstant = ConversionFunctions.ReturnMetricConversonConstant("KN")
          sqlwhere += " ( (amod_cruis_speed * " + conversionConstant.ToString + ")" & clsGeneral.clsGeneral.PrepQueryString(NormalCruiseOperator, NormalCruise, "Numeric", False, "amod_cruis_speed", True) & ") "
        End If
      End If

      'Fuel Capacity
      If FuelCapacity <> "" Then
        If sqlwhere <> "" Then
          sqlwhere += " and "
        End If
        If Not UseMetric Then
          sqlwhere += " (amod_fuel_cap_std_gal " & clsGeneral.clsGeneral.PrepQueryString(FuelCapacityOperator, FuelCapacity, "Numeric", False, "amod_fuel_cap_std_gal", True) & ") "
        Else
          ' convert FuelCapacity from pounds to kilograms
          Dim conversionConstant = ConversionFunctions.ReturnMetricConversonConstant("GAL")
          sqlwhere += " ( (amod_fuel_cap_std_gal * " + conversionConstant.ToString + ")" & clsGeneral.clsGeneral.PrepQueryString(FuelCapacityOperator, FuelCapacity, "Numeric", False, "amod_fuel_cap_std_gal", True) & ") "
        End If
      End If

      If amod_ids <> "" Then
        If sqlwhere <> "" Then
          sqlwhere += " and "
        End If
        sqlwhere += " amod_id in (" & amod_ids & ") "
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

      HttpContext.Current.Session.Item("MasterAircraftPerformanceSpecsWhere") = sqlwhere

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
      PerformanceSpecSearch = Nothing
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in PerformanceSpecSearch(ByVal amod_ids As String) As DataTable SQL VERSION: " & ex.Message
    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing

    End Try

  End Function

  Public Sub add_ToggleMetric_Script(ByVal cbSource As CheckBox, ByVal div0 As HtmlControls.HtmlGenericControl, ByVal div1 As HtmlControls.HtmlGenericControl, _
                                     ByVal div2 As HtmlControls.HtmlGenericControl, ByVal div3 As HtmlControls.HtmlGenericControl, _
                                     ByVal div4 As HtmlControls.HtmlGenericControl, ByVal div5 As HtmlControls.HtmlGenericControl, _
                                     ByVal cbSource1 As CheckBox, ByVal cbSource2 As CheckBox)

    'Register the script block
    Dim sScptStr As StringBuilder = New StringBuilder()

    If Not Page.ClientScript.IsClientScriptBlockRegistered("tml-cb-onclick") Then

      sScptStr.Append("<script type=""text/javascript"">")
      sScptStr.Append(vbCrLf & "  function toggleMetricLabels() {")
      sScptStr.Append(vbCrLf & "    if (document.getElementById(""" + cbSource.ClientID.ToString + """).checked == true) {")
      sScptStr.Append(vbCrLf & "      document.getElementById(""" + div0.ClientID.ToString + """).innerHTML = ""<strong>TAKEOFF PERFORMANCE (ft)</strong>"";")
      sScptStr.Append(vbCrLf & "      document.getElementById(""" + div1.ClientID.ToString + """).innerHTML = ""<strong>RANGE (nm)</strong>"";")
      sScptStr.Append(vbCrLf & "      document.getElementById(""" + div2.ClientID.ToString + """).innerHTML = ""<strong>FUSELAGE DIMENSIONS (ft)</strong>"";")
      sScptStr.Append(vbCrLf & "      document.getElementById(""" + div3.ClientID.ToString + """).innerHTML = ""<strong>WEIGHT (lbs)</strong>"";")
      sScptStr.Append(vbCrLf & "      document.getElementById(""" + div4.ClientID.ToString + """).innerHTML = ""<strong>CAPACITY (gal)</strong>"";")
      sScptStr.Append(vbCrLf & "      document.getElementById(""" + div5.ClientID.ToString + """).innerHTML = ""<strong>SPEED (kts)</strong>"";")
      sScptStr.Append(vbCrLf & "      document.getElementById(""" + cbSource1.ClientID.ToString + """).disabled = false;")
      sScptStr.Append(vbCrLf & "      document.getElementById(""" + cbSource2.ClientID.ToString + """).disabled = false;")
      sScptStr.Append(vbCrLf & "    }")
      sScptStr.Append(vbCrLf & "    else {")
      sScptStr.Append(vbCrLf & "      document.getElementById(""" + div0.ClientID.ToString + """).innerHTML = ""<strong>TAKEOFF PERFORMANCE (m)</strong>"";")
      sScptStr.Append(vbCrLf & "      document.getElementById(""" + div1.ClientID.ToString + """).innerHTML = ""<strong>RANGE (km)</strong>"";")
      sScptStr.Append(vbCrLf & "      document.getElementById(""" + div2.ClientID.ToString + """).innerHTML = ""<strong>FUSELAGE DIMENSIONS (m)</strong>"";")
      sScptStr.Append(vbCrLf & "      document.getElementById(""" + div3.ClientID.ToString + """).innerHTML = ""<strong>WEIGHT (k)</strong>"";")
      sScptStr.Append(vbCrLf & "      document.getElementById(""" + div4.ClientID.ToString + """).innerHTML = ""<strong>CAPACITY (ltr)</strong>"";")
      sScptStr.Append(vbCrLf & "      document.getElementById(""" + div5.ClientID.ToString + """).innerHTML = ""<strong>SPEED (kph)</strong>"";")
      sScptStr.Append(vbCrLf & "      document.getElementById(""" + cbSource1.ClientID.ToString + """).disabled = true;")
      sScptStr.Append(vbCrLf & "      document.getElementById(""" + cbSource2.ClientID.ToString + """).disabled = true;")
      sScptStr.Append(vbCrLf & "    }")
      sScptStr.Append(vbCrLf & "  }")
      sScptStr.Append(vbCrLf & "</script>")

      Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tml-cb-onclick", sScptStr.ToString, False)

    End If

    sScptStr = Nothing

  End Sub

  Public Sub add_ToggleNauticalMiles_Script(ByVal cbSource As CheckBox, ByVal div1 As HtmlControls.HtmlGenericControl)

    'Register the script block
    Dim sScptStr As StringBuilder = New StringBuilder()

    If Not Page.ClientScript.IsClientScriptBlockRegistered("tnm-cb-onclick") Then

      sScptStr.Append("<script type=""text/javascript"">")
      sScptStr.Append(vbCrLf & "  function toggleMilesLabels() {")
      sScptStr.Append(vbCrLf & "    if (document.getElementById(""" + cbSource.ClientID.ToString + """).checked == true) {")
      sScptStr.Append(vbCrLf & "      document.getElementById(""" + div1.ClientID.ToString + """).innerHTML = ""<strong>RANGE (nm)</strong>"";")
      sScptStr.Append(vbCrLf & "    }")
      sScptStr.Append(vbCrLf & "    else {")
      sScptStr.Append(vbCrLf & "      document.getElementById(""" + div1.ClientID.ToString + """).innerHTML = ""<strong>RANGE (sm)</strong>"";")
      sScptStr.Append(vbCrLf & "    }")
      sScptStr.Append(vbCrLf & "  }")
      sScptStr.Append(vbCrLf & "</script>")

      Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tnm-cb-onclick", sScptStr.ToString, False)

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