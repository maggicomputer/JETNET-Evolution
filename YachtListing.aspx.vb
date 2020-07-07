Partial Public Class YachtListing
  Inherits System.Web.UI.Page
  Dim PageNumber As Integer = 1
  Dim PageSort As String = ""
  Dim MarketEvent As Boolean = False
  Dim History As Boolean = False
  Dim ErrorReportingTypeString As String = "Yacht"
  Dim Query_Class_Array As New ArrayList()
  Dim SelectedDataGrid As New DataGrid
  Dim SelectedDataList As New DataList
  Public Shared masterPage As New Object

  Private Sub YachtListing_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

    Me.location_tab.Visible = False

    Dim YachtMarketCategory As DataTable
    If Session.Item("crmUserLogon") <> True Then
      Response.Redirect("Default.aspx", False)
    Else
      Try



        BuildAdvancedSearch()

        If MarketEvent Then
          SelectedDataGrid = YachtEventsDataGrid
          SelectedDataList = YachtDataList
        ElseIf History Then
          SelectedDataGrid = HistoryDataGrid
          SelectedDataList = HistoryDataList
        Else
          SelectedDataGrid = YachtDataGrid
          SelectedDataList = YachtDataList
        End If

        If Not Page.IsPostBack Then
          'If the page is on the market event, 
          'we need to go ahead and fill the yacht Market category.
          If MarketEvent Then
            YachtMarketCategory = Master.aclsData_Temp.ListOfYachtMarketCategories
            clsGeneral.clsGeneral.Populate_Listbox(YachtMarketCategory, market_category, "ypec_category", "ypec_category", False)
            actions_submenu_dropdown.Items.Add(New ListItem("Save As Folder", "javascript:SubMenuYachtDrop(3,0, 'YACHT EVENTS');"))
            actions_submenu_dropdown.Items.Add(New ListItem("Custom Export", "javascript:SubMenuYachtDrop(1,0, 'YACHTEVENTS');"))
            actions_submenu_dropdown.Items.Add(New ListItem("YachtSpot Export/Report", "javascript:SubMenuYachtDrop(5,0, 'YACHTEVENTS');"))
          ElseIf History Then
            actions_submenu_dropdown.Items.Add(New ListItem("Save As Folder", "javascript:SubMenuYachtDrop(3,0, 'YACHT HISTORY');"))
            actions_submenu_dropdown.Items.Add(New ListItem("Custom Export", "javascript:SubMenuYachtDrop(1,0, 'YACHTHISTORY');"))
            actions_submenu_dropdown.Items.Add(New ListItem("YachtSpot Export/Report", "javascript:SubMenuYachtDrop(5,0, 'YACHTHISTORY');"))
          Else
            actions_submenu_dropdown.Items.Add(New ListItem("Save As Folder", "javascript:SubMenuYachtDrop(3,0, 'YACHT');"))

            actions_submenu_dropdown.Items.Add(New ListItem("Custom Export", "javascript:SubMenuYachtDrop(1,0, 'YACHT');"))
            actions_submenu_dropdown.Items.Add(New ListItem("YachtSpot Export/Report", "javascript:SubMenuYachtDrop(5,0, 'YACHT');"))
          End If


          'Fill operators
          DisplayFunctions.Fill_Dropdown("Year", operator_year_dlv, "")
          DisplayFunctions.Fill_Dropdown("Year", operator_year_mfr, "")
          DisplayFunctions.Fill_Dropdown("Numeric", operator_length, "")
          DisplayFunctions.Fill_Dropdown("Numeric", operator_asking_price, "")
          DisplayFunctions.Fill_Dropdown("Numeric", operator_days_on_market, "")
          FillCurrencyDropdowns(price_range_currency)

          ' DisplayFunctions.Fill_Dropdown("Numeric", operator_charter_price, "")
          ' FillCurrencyDropdowns(charter_price_currency)

          Dim TempTable As New DataTable
          'Fill Status
          TempTable = Master.aclsData_Temp.ListOfYachtStagesStatusCombined()
          Populate_Dual_Stages_Listbox(TempTable, yt_lifecycle_id, "yl_lifecycle_name", "yls_lifecycle_status", "yl_lifecyle_id", "yls_lifecycle_status", " - ", False)

          'Fill engine manufacturer
          TempTable = YachtFunctions.GetYachtEngineManufacturers()
          clsGeneral.clsGeneral.Populate_Listbox(TempTable, yt_engine_manufacturer, "comp_name", "comp_id", False)

          'Fill Yacht Charter Locations:
          TempTable = YachtFunctions.GetYachtCharterLocations()
          clsGeneral.clsGeneral.Populate_Listbox(TempTable, yt_confidential_notes, "ycloc_name", "ycloc_keywords", False)

          'fill yacht compliance type
          TempTable = YachtFunctions.GetYachtComplianceTypes()
          clsGeneral.clsGeneral.Populate_Dropdown(TempTable, yt_compliance_type, "yct_type", "yct_id", False)
          'Fill Yacht Class:


          'ycst_code, ycst_society_name
          TempTable = Master.aclsData_Temp.Get_Yacht_Class()
          clsGeneral.clsGeneral.Populate_Dropdown(TempTable, yacht_class, "ycst_society_name", "ycst_code", False)

          'Fill out registration country
          TempTable = Master.aclsData_Temp.Get_Jetnet_Country()
          clsGeneral.clsGeneral.Populate_Dropdown(TempTable, country_registration, "clicountry_name", "clicountry_name", False)
          'Fill out business type
          TempTable = New DataTable
          TempTable = Master.aclsData_Temp.Get_Jetnet_Business_Type()
          clsGeneral.clsGeneral.Populate_Listbox(TempTable, cref_business_type, "cbus_name", "cbus_type", False)
          'Fill out contact title
          TempTable = New DataTable
          TempTable = Master.aclsData_Temp.Get_Jetnet_Contact_Title_Group()
          clsGeneral.clsGeneral.Populate_Listbox(TempTable, contact_title, "ctitleg_group_name", "ctitleg_group_name", False)

          'TempTable = New DataTable
          'TempTable = Master.aclsData_Temp.Get_Jetnet_Category_Size()
          'clsGeneral.clsGeneral.Populate_Dropdown(TempTable, model_length_class, "ycs_description", "ycs_description", False)

          'SELECT * FROM yacht_category_size WITH (NOLOCK) 


          'Fill out relationship
          TempTable = New DataTable
          yr_contact_type.Items.Clear()

          TempTable = Master.aclsData_Temp.Get_Yacht_Contact_Type(History)
          clsGeneral.clsGeneral.Populate_Listbox(TempTable, yr_contact_type, "yct_name", "yct_code", True)
          yr_contact_type.Items.RemoveAt(0)
          yr_contact_type.Items.Insert(0, New ListItem("All", ""))
          yr_contact_type.Items.Insert(1, New ListItem("All Central Agents", "'99','C1','C2','C3','C4','C5','C6'"))
          yr_contact_type.Items.Insert(2, New ListItem("All Designers", "'Y1','Y2','Y3','Y0','Y9'"))
          yr_contact_type.Items.Insert(3, New ListItem("All Owners", "'00','08'"))

          yr_contact_type.SelectedValue = ""

          TempTable = Nothing

          ym_mfr_comp_id.Items.Clear()
          TempTable = YachtFunctions.Get_Yacht_Brand_And_Manufacturer()
          Populate_Dual_Stages_Listbox(TempTable, ym_mfr_comp_id, "comp_name", "ym_brand_name", "comp_id", "ym_brand_name", " / ", False)

          brand_listbox.Items.Add(New ListItem("Please Select a Manufacturer", ""))


          If Yacht_Criteria.Visible = True Then

            'This needs to be put in and loaded for now. Hopefully whenever the session variables are the same, this can go away.
            If Not Session.Item("localPreferences").loadUserSession("", CLng(Session.Item("localUser").crmSubSubID.ToString), Session.Item("localUser").crmUserLogin.ToString, CLng(Session.Item("localUser").crmSubSeqNo.ToString), CLng(Session.Item("localUser").crmUserContactID.ToString)) Then
              Response.Write("error in load preferences : ")
            End If


            ''Filling the yacht array in case it hasn't been filled yet.
            commonEvo.fillYachtArray("")
            commonEvo.fillYachtCategoryLableArray("")

            If Page.Request.Form("complete_search") = "Y" Then
              Dim cFolderData As String = ""

              'This is going to be moved to DisplayFunctions.RefillUpFolderInformation, however
              'I need to get it working first.
              'Afterwards it will be moved but if it's not working I don't want it to affect other pages yet.
              If Page.Request.Form("ym_category_size") <> "" Then
                Dim tempArray As Array = Split(Page.Request.Form("ym_category_size"), "|")

                If UBound(tempArray) > 0 Then
                  HttpContext.Current.Session.Item("tabYachtType") = commonEvo.FindYachtIndexForFirstItem(tempArray(0), crmWebClient.Constants.LOCYACHT_CATEGORY, tempArray(1), crmWebClient.Constants.LOCYACHT_MOTOR)
                  HttpContext.Current.Session.Item("tabYachtSize") = commonEvo.FindYachtIndexForFirstItem(tempArray(0), crmWebClient.Constants.LOCYACHT_CATEGORY, tempArray(1), crmWebClient.Constants.LOCYACHT_MOTOR)
                End If
              End If

              If Page.Request.Form("ym_brand_name") <> "" Then
                Dim TemporaryCategoryHold As String = ""

                'Setting the brand.
                HttpContext.Current.Session.Item("tabYachtBrand") = commonEvo.FindYachtIndexForFirstItem(Page.Request.Form("ym_brand_name"), crmWebClient.Constants.LOCYACHT_BRAND)
                'After I set the brand, I need to go ahead and set the category for the brand.
                commonEvo.ReturnYachtModelDataFromIndex(CLng(HttpContext.Current.Session.Item("tabYachtBrand")), "", TemporaryCategoryHold, "", "")
                HttpContext.Current.Session.Item("tabYachtCategory") = commonEvo.FindYachtIndexForFirstItem(TemporaryCategoryHold, crmWebClient.Constants.LOCYACHT_CATEGORY, Page.Request.Form("ym_brand_name"), crmWebClient.Constants.LOCYACHT_BRAND)

              End If

              LoopThroughAndCompleteYachtTextBoxFillup(cFolderData)

            End If

            If Page.Request.Form("project_search") = "Y" Then
              Dim folderID As Long = 0
              Dim FoldersTableData As New DataTable
              Dim cfolderData As String = ""

              us_standard.Checked = False

              FolderInformation.Text = ""
              FolderInformation.Visible = False
              folderID = Page.Request.Form("project_id")

              If folderID <> 0 Then
                FoldersTableData = Master.aclsData_Temp.GetEvolutionFolderssBySubscription(folderID, Session.Item("localUser").crmUserLogin, Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmSubSeqNo, "", 0, Nothing, "")
                If Not IsNothing(FoldersTableData) Then
                  If FoldersTableData.Rows.Count > 0 Then
                    cfolderData = FoldersTableData.Rows(0).Item("cfolder_data").ToString
                    If FoldersTableData.Rows(0).Item("cfolder_method").ToString = "S" Then
                      folder_name.Text = FoldersTableData.Rows(0).Item("cfolder_name").ToString
                      static_folder.Text = "true"
                    End If

                    If cfolderData <> "" Then
                      'Fills up the applicable folder Information pulled from the cfolder data field
                      DisplayFunctions.FillUpFolderInformation(Table2, close_current_folder, cfolderData, FolderInformation, FoldersTableData, False, History, MarketEvent, False, True, Collapse_Panel, actions_submenu_dropdown, company_contact_tab, StaticFolderNewSearchLabel, Control_Panel, "")
                    End If


                    If MarketEvent Then
                      If market_category.SelectedValue <> "" Then
                        'Fill the Type box.
                        market_category_SelectedIndexChanged(market_category, EventArgs.Empty)
                      End If
                    End If

                    'Added 12/10/15. Refilling up static tab panels.
                    DisplayFunctions.RefillUpFolderInformation(False, cfolderData, Collapse_Panel, general_tab)
                    DisplayFunctions.RefillUpFolderInformation(False, cfolderData, Collapse_Panel, location_tab)
                    DisplayFunctions.RefillUpFolderInformation(False, cfolderData, Collapse_Panel, hull_tab)
                    DisplayFunctions.RefillUpFolderInformation(False, cfolderData, Collapse_Panel, maintenance)
                    DisplayFunctions.RefillUpFolderInformation(False, cfolderData, Collapse_Panel, charter_tab)
                    'The attributes panel needs to be refilled
                    'This just runs through to find the specific controls on the attributes panel.
                    DisplayFunctions.RefillUpFolderInformation(False, cfolderData, Collapse_Panel, AttributesPanel)


                  End If
                End If
              Else 'If we don't have a folder ID, look to the request variables. This happens when a quick search or summary is performed.
                LoopThroughAndCompleteYachtTextBoxFillup(cfolderData)
              End If

            End If
          End If
        End If
      Catch ex As Exception
        Master.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " (" & ErrorReportingTypeString & "): " & ex.Message)
      End Try


    End If
  End Sub

  Private Sub FillCurrencyDropdowns(ByVal CurrencyDropDownList As DropDownList)
    Dim CurrencyTable As New DataTable
    CurrencyTable = Master.aclsData_Temp.FillCurrencyTable()

    If Not IsNothing(CurrencyTable) Then
      If CurrencyTable.Rows.Count > 0 Then
        For Each r As DataRow In CurrencyTable.Rows
          If Not IsDBNull(r("currency_name")) Then
            If Not IsDBNull(r("currency_id")) Then
              CurrencyDropDownList.Items.Add(New ListItem(r("currency_name").ToString, r("currency_id").ToString))
            End If
          End If
        Next

        CurrencyDropDownList.SelectedValue = "9"   ' us dollar
      End If
    End If
  End Sub

  Private Sub LoopThroughAndCompleteYachtTextBoxFillup(ByVal cfolderData As String)
    For Each name As String In Request.Form.AllKeys 'This will loop through all the keys.
      If name <> "project_id" And name <> "project_search" Then
        'This test is going to check and see if the control actually exists of the request variable before passing it along.
        Dim value As String = Request.Form(name)
        Dim cont As Object = Collapse_Panel.FindControl(name)
        If cfolderData <> "" Then
          cfolderData += "!~!"
        End If
        cfolderData += name & "=" & value
      End If
    Next

    ' cfolderData = Replace(cfolderData, "for_sale=Y", "for_sale=true")
    If cfolderData <> "" Then
      DisplayFunctions.RefillUpFolderInformation(False, cfolderData, Collapse_Panel, Nothing)

      'The attributes panel needs to be refilled
      'This just runs through to find the specific controls on the attributes panel.
      DisplayFunctions.RefillUpFolderInformation(False, cfolderData, Collapse_Panel, AttributesPanel)
    End If
  End Sub

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Dim FoldersTable As New DataTable

    If HttpContext.Current.Session.Item("jetnetWebSiteType") = crmWebClient.eWebSiteTypes.LOCAL Then
      imgDisplayFolder.Text = "https://www.testjetnetevolution.com/pictures/yacht"
    Else
      imgDisplayFolder.Text = HttpContext.Current.Session.Item("jetnetFullHostName").ToString + HttpContext.Current.Session.Item("YachtPicturesFolderVirtualPath")
    End If


    viewCCSTDropDowns.setIsBase(False)
    viewCCSTDropDowns.setIsView(False)
    viewCCSTDropDowns.setFirstControl(True)
    viewCCSTDropDowns.setListSize(6)
    viewCCSTDropDowns.setShowInactiveCountries(False)

    If Not Page.IsPostBack Then

      folders_submenu_dropdown.Items.Clear()

      FoldersTable = Master.aclsData_Temp.GetEvolutionFolderssBySubscription(0, Session.Item("localUser").crmUserLogin, Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmSubSeqNo, "", IIf(History = True, 14, IIf(MarketEvent = True, 15, 10)), Nothing, "")
      DisplayFunctions.AddEditFolderListOptionToFolderDropdown(folders_submenu_dropdown, IIf(History = True, 14, IIf(MarketEvent = True, 15, 10)))

      If Not IsNothing(FoldersTable) Then
        If FoldersTable.Rows.Count > 0 Then
          For Each r As DataRow In FoldersTable.Rows
            If Not IsDBNull(r("cfolder_data")) Then
              Dim FolderDataString As Array
              'this was added to parse out the real search query now that we're saving it

              FolderDataString = Split(r("cfolder_data"), "THEREALSEARCHQUERY")

              If History Then
                folders_submenu_dropdown.Items.Add(New ListItem(r("cfolder_name").ToString, "javascript:ParseYachtSpecialFolders('" & r("cfolder_id").ToString & "',true,false,'" & Replace(FolderDataString(0), "'", "\'") & "');"))
              ElseIf MarketEvent Then
                folders_submenu_dropdown.Items.Add(New ListItem(r("cfolder_name").ToString, "javascript:ParseYachtSpecialFolders('" & r("cfolder_id").ToString & "',false,true,'" & Replace(FolderDataString(0), "'", "\'") & "');"))
              Else
                folders_submenu_dropdown.Items.Add(New ListItem(r("cfolder_name").ToString, "javascript:ParseForm('" & r("cfolder_id").ToString & "', false, false,false, false,true,'" & Replace(FolderDataString(0), "'", "\'") & "');"))
              End If

              'folders_submenu_dropdown.Items.Add(New ListItem(r("cfolder_name").ToString, "javascript:alert('This folder contains no information');"))
            End If
          Next
        End If
      End If
    End If
    FoldersTable.Dispose()
    FoldersTable = Nothing

    'This toggles the lower and higher bar of the page depending on if it's showing or not
    ToggleHigherLowerBar(False)
    'Setting the page up for the non-search version. This will be toggled in the search function
    If Not Page.IsPostBack Then
      Initial(True)
    End If


    tabTSBMDropDowns.setControlName("Yacht")
    FillUpSessionForYachtTypeSizeBrandModel("tab", "Yacht")

    If MarketEvent = False And History = False Then
      Master.Set_Active_Tab(2)
      'This will set page title.
      Me.Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Yacht Search Results")
      event_yacht_row.Visible = False
      event_table.Visible = False
      aerodex_toggle.Visible = True
      lifecycle_cell.Visible = True
      event_toggle_on.Visible = False
      aerodex_toggle_checkboxes.Visible = True

      tabTSBMDropDowns.setListSize(21)

      'viewCBMDropDowns.setListSize(21)

    ElseIf History = True Then
      Master.Set_Active_Tab(4)
      'This will set page title.
      Me.Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("History Search Results")
      event_yacht_row.Visible = False
      event_table.Visible = False
      aerodex_toggle.Visible = False
      price_range_toggle.Visible = False
      lifecycle_cell.Visible = False
      event_toggle_on.Visible = False
      aerodex_toggle_checkboxes.Visible = False
      history_toggle_on.Visible = True

      tabTSBMDropDowns.setListSize(16)
      'viewCBMDropDowns.setListSize(16)

      event_box.CssClass = "transaction_search_box"
    ElseIf MarketEvent = True Then
      Master.Set_Active_Tab(5)
      'This will set page title.
      Me.Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Yacht Event Search Results")
      'events_type_of_search.Visible = True

      tabTSBMDropDowns.setListSize(26)
      'viewCBMDropDowns.setListSize(26)

      event_yacht_row.Visible = True
      event_table.Visible = True
      event_box.CssClass = "event_search_box"
      aerodex_toggle.Visible = False
      lifecycle_cell.Visible = False
      aerodex_toggle_checkboxes.Visible = False
      event_toggle_on.Visible = True


    End If
    If Not Page.IsPostBack Then
      If Yacht_Criteria.Visible = True Then
        'Load Search Information:
        Dim TempFolderID As Integer = 0
        If Page.Request.Form("project_search") = "Y" Then
          If Page.Request.Form("project_id") <> 0 Then
            TempFolderID = Page.Request.Form("project_id")
          End If
        End If

        If TempFolderID = 0 Then
          If Page.Request.Form("clearSelection") = "true" Then
          Else
            FillOutSearchParameters()
          End If
        End If
      End If
    End If

  End Sub

  ''' <summary>
  ''' Next/previous button clicks function.
  ''' </summary>
  ''' <param name="next_"></param>
  ''' <param name="prev_"></param>
  ''' <param name="next_all"></param>
  ''' <param name="prev_all"></param>
  ''' <param name="goToPage"></param>
  ''' <param name="pageNumber"></param>
  ''' <remarks></remarks>
  Public Sub MovePage(ByVal next_ As Boolean, ByVal prev_ As Boolean, ByVal next_all As Boolean, ByVal prev_all As Boolean, ByVal goToPage As Boolean, ByVal pageNumber As Integer)
    Try
      Dim holdTable As New DataTable
      Dim StartCount As Integer = 0
      Dim EndCount As Integer = 0
      Dim RecordsPerPage As Integer = 0
      If Session.Item("localUser").crmUserRecsPerPage <> 0 Then
        RecordsPerPage = Session.Item("localUser").crmUserRecsPerPage
      End If




      If Not IsNothing(Session.Item("Yacht_Master")) Then
        holdTable = Session.Item("Yacht_Master")
        Initial(False)
        DisplayFunctions.MovePage(StartCount, EndCount, SelectedDataGrid, SelectedDataList, holdTable, next_, prev_, next_all, prev_all, goToPage, pageNumber)
        SetPagingButtons(IIf(StartCount = 1, False, True), IIf(holdTable.Rows.Count = EndCount, False, True))

        record_count.Text = "Showing " & StartCount & " - " & IIf(holdTable.Rows.Count <= RecordsPerPage, holdTable.Rows.Count, IIf((RecordsPerPage + StartCount) <= holdTable.Rows.Count, IIf(StartCount = 1, RecordsPerPage, RecordsPerPage + StartCount), holdTable.Rows.Count))
        record_count2.Text = record_count.Text
      End If


    Catch ex As Exception
      'Some More Error Catching.
      Master.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " : " & ex.Message.ToString)
    End Try

  End Sub


  Public Sub Yacht_Search(ByVal model_string As String, ByVal forsale_flag As String, _
                          ByVal ForLease_Flag As String, ByVal ForCharter_Flag As String, _
                          ByVal MFRStart As String, ByVal MFREnd As String, _
                          ByVal CallSign As String, ByVal LifeCycleStage_String As String, _
                          ByVal Status As String, ByVal Ownership_String As String, _
                          ByVal PreviouslyOwned_Flag As String, ByVal model_type_string As String, _
                          ByVal make_string As String, ByVal brandString As String, _
                          ByVal yearString As String, ByVal CategorySizeString As String, _
                          ByVal MotorSizeString As String, ByVal PageNumber As String, _
                          ByVal PageSort As String, ByVal bindFromSession As Boolean, _
                          ByVal yacht_name_search As String, ByVal BuildSearchString As String, _
                          ByVal yachtIDs As String, ByVal yachtModels As String, _
                          ByVal RegisteredCountryFlag As String, ByVal yearDlv As String, _
                          ByVal yachtClass As String, ByVal PreviousName As Boolean, _
                          ByVal CompanyCountriesString As String, _
                          ByVal CompanyTimeZoneString As String, ByVal CompanyContinentString As String, _
                          ByVal CompanyRegionString As String, ByVal CompanyStateName As String, _
                          ByVal DynamicQueryString As String, ByVal helipad_check As String, _
                          ByVal BrandMFR_String As String, ByVal JournalDate As String, ByVal JournalTransType As String, _
                          ByVal useAltHullMFR As Boolean, ByVal US_Waters_Flag As String, _
                          ByVal yachtAskingPrice As String, ByVal yachtAskingPriceCurrency As String, ByVal yachtAskingPriceOperator As String)

    Dim Results_Table As New DataTable

    Try
      Dim Paging_Table As New DataTable
      Dim RecordsPerPage As Integer = 0


      If Session.Item("localUser").crmUserRecsPerPage <> 0 Then
        RecordsPerPage = Session.Item("localUser").crmUserRecsPerPage
      End If

      If bindFromSession = True And Not IsNothing(Session.Item("Yacht_Master")) Then
        Results_Table = Session.Item("Yacht_Master")
      Else
        Results_Table = EvolutionYachtListingPageQuery(forsale_flag, ForLease_Flag, _
                                                      ForCharter_Flag, MFRStart, _
                                                      MFREnd, CallSign, _
                                                      LifeCycleStage_String, brandString, _
                                                      yearString, CategorySizeString, _
                                                      MotorSizeString, PageSort, _
                                                      yacht_name_search, yachtIDs, _
                                                      yachtModels, Ownership_String, _
                                                      Status, RegisteredCountryFlag, _
                                                      yearDlv, yachtClass, _
                                                      PreviousName, _
                                                      CompanyCountriesString, CompanyTimeZoneString, _
                                                      CompanyContinentString, CompanyRegionString, _
                                                      CompanyStateName, DynamicQueryString, _
                                                      helipad_check, BrandMFR_String, _
                                                      History, JournalDate, JournalTransType, _
                                                      useAltHullMFR, US_Waters_Flag, _
                                                      yachtAskingPrice, yachtAskingPriceCurrency, yachtAskingPriceOperator)
        Session.Item("Yacht_Master") = Results_Table
      End If

      HttpContext.Current.Session.Item("SearchString") = BuildSearchString

      Master.SetStatusText(HttpContext.Current.Session.Item("SearchString"))


      If Not IsNothing(Results_Table) Then
        Session.Item("localUser").crmLatestRecordCount = Results_Table.Rows.Count
        If Results_Table.Rows.Count > 0 Then
          'This is basically saying that if the datagrid isn't visible, don't fill it
          If SelectedDataGrid.Visible = True Then
            SelectedDataGrid.DataSource = Results_Table
            SelectedDataGrid.PageSize = RecordsPerPage
            SelectedDataGrid.DataBind()
            If Session.Item("localSubscription").crmServerSideNotes_Flag Or Session.Item("localSubscription").crmCloudNotes_Flag Then
              If Session.Item("localUser").crmDisplayNoteTag Then
                If History Then
                  SelectedDataGrid.Columns(7).Visible = True
                Else
                  SelectedDataGrid.Columns(8).Visible = True
                End If

              End If
            End If
          End If


          'This is basically saying that if the datagrid isn't visible, don't fill it
          If SelectedDataList.Visible = True Then
            'We need to add the paging to this for now since the datalist doesn't natively support paging. 
            'For right now, we clone the results table (getting the schema) then filter based on the ac_count field (added during query)
            'This will allow us to bind based on the paging table.
            Paging_Table = Results_Table.Clone
            Dim afiltered_Client As DataRow() = Results_Table.Select("comp_count <= " & RecordsPerPage, "")
            For Each atmpDataRow_Client In afiltered_Client
              Paging_Table.ImportRow(atmpDataRow_Client)
            Next

            SelectedDataList.DataSource = Paging_Table
            SelectedDataList.DataBind()
          End If


          criteria_results.Text = Results_Table.Rows.Count & " Results"
          record_count.Text = "Showing 1 - " & IIf(Results_Table.Rows.Count <= RecordsPerPage, Results_Table.Rows.Count, RecordsPerPage)
          record_count2.Text = record_count.Text
          'This will fill up the dropdown bar with however many pages.
          'This will fill up the dropdown bar with however many pages.
          If Results_Table.Rows.Count > RecordsPerPage Then
            Fill_Page_To_To_Dropdown(Math.Ceiling(Results_Table.Rows.Count / RecordsPerPage))
            SetPagingButtons(False, True)
          Else
            Fill_Page_To_To_Dropdown(1)
            SetPagingButtons(False, False)
          End If


          Paging_Table = Nothing
        Else
          record_count.Text = "0 Records"
          record_count2.Text = record_count.Text
          criteria_results.Text = "No Results"
          SelectedDataGrid.DataSource = New DataTable
          SelectedDataGrid.DataBind()
          SelectedDataList.DataSource = New DataTable
          SelectedDataList.DataBind()
          SetPagingButtons(False, False)
          yacht_attention.Text = "<br /><p class='padding'><b>No Yacht Found. Please refine your search and try again.</b></p><br /><br />"
        End If

      End If
    Catch ex As Exception

    End Try
  End Sub

  Public Sub Event_Search(ByVal model_string As String, ByVal forsale_flag As String, _
                          ByVal ForLease_Flag As String, ByVal ForCharter_Flag As String, _
                          ByVal MFRStart As String, ByVal MFREnd As String, _
                          ByVal CallSign As String, ByVal LifeCycleStage_String As String, _
                          ByVal Status As String, ByVal Ownership_String As String, _
                          ByVal PreviouslyOwned_Flag As String, ByVal model_type_string As String, _
                          ByVal make_string As String, ByVal brandString As String, _
                          ByVal yearString As String, ByVal CategorySizeString As String, _
                          ByVal MotorSizeString As String, ByVal PageNumber As String, _
                          ByVal PageSort As String, ByVal bindFromSession As Boolean, _
                          ByVal yacht_name_search As String, ByVal BuildSearchString As String, _
                          ByVal yachtIDs As String, ByVal yachtModels As String, _
                          ByVal MarketCategory As String, ByVal MarketType As String, _
                          ByVal StartDate As String, ByVal RegisteredCountryFlag As String, _
                          ByVal yearDlv As String, ByVal yachtClass As String, _
                          ByVal PreviousName As Boolean, _
                          ByVal DynamicQueryString As String, ByVal useAltHullMFR As Boolean, _
                          ByVal EventTransactionSearch As Boolean, ByVal US_Waters_Flag As String, _
                          ByVal CompanyCountriesString As String, ByVal CompanyTimeZoneString As String, _
                          ByVal CompanyContinentString As String, ByVal CompanyRegionString As String, _
                          ByVal CompanyStateName As String, ByVal BrandMFR_String As String, _
                          ByVal yachtAskingPrice As String, ByVal yachtAskingPriceCurrency As String, ByVal yachtAskingPriceOperator As String)

    Dim Results_Table As New DataTable
    Try
      Dim Paging_Table As New DataTable
      Dim RecordsPerPage As Integer = 0

      If Session.Item("localUser").crmUserRecsPerPage <> 0 Then
        RecordsPerPage = Session.Item("localUser").crmUserRecsPerPage
      End If

      If bindFromSession = True And Not IsNothing(Session.Item("Yacht_Master")) Then
        Results_Table = Session.Item("Yacht_Master")
      Else
        Results_Table = EvolutionYachtEventListingPageQuery(forsale_flag, ForLease_Flag, _
                                                           ForCharter_Flag, MFRStart, _
                                                           MFREnd, CallSign, _
                                                           LifeCycleStage_String, brandString, _
                                                           yearString, CategorySizeString, _
                                                           MotorSizeString, PageSort, _
                                                           yacht_name_search, yachtIDs, _
                                                           yachtModels, MarketCategory, _
                                                           MarketType, StartDate, _
                                                           Ownership_String, Status, _
                                                           RegisteredCountryFlag, yearDlv, _
                                                           PreviousName, yachtClass, _
                                                           useAltHullMFR, EventTransactionSearch, _
                                                           US_Waters_Flag, DynamicQueryString, _
                                                           CompanyCountriesString, CompanyTimeZoneString, _
                                                           CompanyContinentString, CompanyRegionString, _
                                                           CompanyStateName, BrandMFR_String, _
                                                           yachtAskingPrice, yachtAskingPriceCurrency, yachtAskingPriceOperator)
        Session.Item("Yacht_Master") = Results_Table
      End If

      HttpContext.Current.Session.Item("SearchString") = BuildSearchString

      Master.SetStatusText(HttpContext.Current.Session.Item("SearchString"))


      If Not IsNothing(Results_Table) Then
        Session.Item("localUser").crmLatestRecordCount = Results_Table.Rows.Count
        If Results_Table.Rows.Count > 0 Then
          'This is basically saying that if the datagrid isn't visible, don't fill it
          If YachtEventsDataGrid.Visible = True Then
            YachtEventsDataGrid.DataSource = Results_Table
            YachtEventsDataGrid.PageSize = RecordsPerPage
            YachtEventsDataGrid.DataBind()
          End If

          criteria_results.Text = Results_Table.Rows.Count & " Results"
          record_count.Text = "Showing 1 - " & IIf(Results_Table.Rows.Count <= RecordsPerPage, Results_Table.Rows.Count, RecordsPerPage)
          record_count2.Text = record_count.Text
          'This will fill up the dropdown bar with however many pages.
          'This will fill up the dropdown bar with however many pages.
          If Results_Table.Rows.Count > RecordsPerPage Then
            Fill_Page_To_To_Dropdown(Math.Ceiling(Results_Table.Rows.Count / RecordsPerPage))
            SetPagingButtons(False, True)
          Else
            Fill_Page_To_To_Dropdown(1)
            SetPagingButtons(False, False)
          End If


          Paging_Table = Nothing
        Else
          record_count.Text = "0 Records"
          record_count2.Text = record_count.Text
          criteria_results.Text = "No Results"
          YachtEventsDataGrid.DataSource = New DataTable
          YachtEventsDataGrid.DataBind()

          SetPagingButtons(False, False)
          yacht_attention.Text = "<br /><p class='padding'><b>No Yacht Found. Please refine your search and try again.</b></p><br /><br />"
        End If

      End If
    Catch ex As Exception

    End Try
  End Sub

  ''' <summary>
  ''' This runs on the initial load of the page. It'll toggle off some of the paging elements and things we don't need displayed if we're first coming into the page.
  ''' </summary>
  ''' <param name="initial_page_load"></param>
  ''' <remarks></remarks>
  Public Sub Initial(ByVal initial_page_load As Boolean)
    If initial_page_load = True Then
      criteria_results.Visible = False
      sort_by_text.Visible = False
      sort_by_dropdown.Visible = False
      view_dropdown.Visible = False
      actions_dropdown.Visible = False
      paging.Visible = False
      paging2.Visible = False
      PanelCollapseEx.Collapsed = False
      PanelCollapseEx.ClientState = False


    Else
      PanelCollapseEx.Collapsed = True
      PanelCollapseEx.ClientState = True
      criteria_results.Visible = True
      sort_by_text.Visible = True
      sort_by_dropdown.Visible = True
      If MarketEvent = False Then 'This means that the event listing does not have a switch to gallery/listing dropdown
        'because on the events tab, there isn't a gallery listing.
        view_dropdown.Visible = True
      End If
      actions_dropdown.Visible = True
      paging.Visible = True
      paging2.Visible = True

      per_page_text.Visible = True
      per_page_dropdown_.Visible = True
    End If
  End Sub
  '''' <summary>
  '''' Toggles the bar whether it's the high bar or the low bar. This sets up the javascript for the bulleted lists as well.
  '''' </summary>
  '''' <param name="lower_bar"></param>
  '''' <remarks></remarks>
  Public Sub ToggleHigherLowerBar(ByVal lower_bar As Boolean)
    sort_submenu_dropdown.Items.Clear()

    'setting the javascript of the menus
    'folders:
    folders_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & folders_submenu_dropdown.ClientID & "', true);")
    folders_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & folders_submenu_dropdown.ClientID & "', false);")

    folders_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & folders_submenu_dropdown.ClientID & "', true);")
    folders_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & folders_submenu_dropdown.ClientID & "', false);")


    'sort dropdown
    sort_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & sort_submenu_dropdown.ClientID & "', true);")
    sort_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & sort_submenu_dropdown.ClientID & "', false);")

    sort_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & sort_submenu_dropdown.ClientID & "', true);")
    sort_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & sort_submenu_dropdown.ClientID & "', false);")

    'Fill up sort menu dynamically:
    sort_submenu_dropdown.Items.Add("Year")
    sort_submenu_dropdown.Items.Add("Name")
    sort_submenu_dropdown.Items.Add("Hull #")
    sort_submenu_dropdown.Items.Add("Brand/Model")
    sort_submenu_dropdown.Items.Add("Model")
    If MarketEvent Then
      sort_submenu_dropdown.Items.Add("Date DESC")
      sort_submenu_dropdown.Items.Add("Date ASC")
    Else
      sort_submenu_dropdown.Items.Add("LOA")
    End If

    'page dropdown
    per_page_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & per_page_submenu_dropdown.ClientID & "', true);")
    per_page_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & per_page_submenu_dropdown.ClientID & "', false);")

    per_page_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & per_page_submenu_dropdown.ClientID & "', true);")
    per_page_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & per_page_submenu_dropdown.ClientID & "', false);")

    'go to dropdown
    go_to_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & go_to_submenu_dropdown.ClientID & "', true);")
    go_to_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & go_to_submenu_dropdown.ClientID & "', false);")

    go_to_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & go_to_submenu_dropdown.ClientID & "', true);")
    go_to_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & go_to_submenu_dropdown.ClientID & "', false);")


    'view dropdown
    view_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & view_submenu_dropdown.ClientID & "', true);")
    view_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & view_submenu_dropdown.ClientID & "', false);")

    view_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & view_submenu_dropdown.ClientID & "', true);")
    view_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & view_submenu_dropdown.ClientID & "', false);")

    'actions dropdown
    actions_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & actions_submenu_dropdown.ClientID & "', true);")
    actions_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & actions_submenu_dropdown.ClientID & "', false);")

    actions_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & actions_submenu_dropdown.ClientID & "', true);")
    actions_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & actions_submenu_dropdown.ClientID & "', false);")

    If lower_bar = True Then
      PanelCollapseEx.Enabled = False
      Collapse_Panel.Visible = False
      search_expand_text.Visible = False
      help_text.Visible = False
      sort_by_text.Visible = False
      sort_by_dropdown.Visible = False
      view_dropdown_.Visible = False

    Else
      per_page_dropdown_.Visible = False
      per_page_text.Visible = False
      go_to_dropdown_.Visible = False
      go_to_text.Visible = False
    End If
  End Sub

  Public Sub SetPageNumber(Optional ByVal selectedLI As Integer = 0)
    PageNumber = selectedLI
  End Sub
  ''' <summary>
  ''' Small function to swap classes of listing view dropdown on ac listing page.
  ''' </summary>
  ''' <param name="showtype"></param>
  ''' <remarks></remarks>
  Sub SwitchGalleryListing(ByVal showtype As Integer)
    Select Case showtype
      Case 0
        view_dropdown.Items.Clear()
        view_dropdown.Items.Add(New ListItem("", ""))
        view_dropdown.CssClass = "ul_top listing_view_bullet"
        AlterListing(0, 0)
        Session.Item("localUser").crmACListingView = eListingView.LISTING
      Case 1
        view_dropdown.Items.Clear()
        view_dropdown.Items.Add(New ListItem("", ""))
        view_dropdown.CssClass = "ul_top thumnail_view_bullet"
        AlterListing(1, 0)
        Session.Item("localUser").crmACListingView = eListingView.GALLERY
    End Select

  End Sub
  Public Sub SetPageSort(Optional ByVal selectedLI As String = "")
    Try
      Select Case selectedLI
        Case "Year"
          PageSort = " yt_year_mfr"
        Case "Name"
          PageSort = " yt_yacht_name"
        Case "Hull #"
          PageSort = " yt_hull_mfr_nbr"
          PageSort = " dbo.ConvertToNumeric(yt_hull_mfr_nbr) ASC, yt_hull_mfr_nbr ASC "
        Case "Brand/Model"
          PageSort = " ym_brand_name, ym_model_name, yt_hull_mfr_nbr"
        Case "LOA"
          PageSort = " Yacht.yt_length_overall_meters"
        Case "Date DESC"
          PageSort = " ype_entered_date desc"
        Case "Date ASC"
          PageSort = " ype_entered_date ASC"
        Case Else
          PageSort = " ym_model_name"
      End Select

    Catch ex As Exception
      Master.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " (Yacht): " & ex.Message)
    End Try
  End Sub
  ''' <summary>
  ''' Click part of the dropdown list, switch the submenu bullet with the main bullet
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Public Sub submenu_dropdown_Click(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.BulletedListEventArgs)
    Try
      Dim selectedLI As New ListItem
      selectedLI = sender.Items(e.Index)
      If sender.id.ToString = "sort_submenu_dropdown" Then
        sort_dropdown.Items.Clear()
        sort_dropdown.Items.Add(New ListItem(selectedLI.Text, ""))
        SetPageSort(selectedLI.Text)
        search_Click(search, EventArgs.Empty)
      ElseIf sender.id.ToString = "view_submenu_dropdown" Then
        SwitchGalleryListing(e.Index)
        search_Click(search, EventArgs.Empty, True)
      ElseIf sender.id.ToString = "go_to_submenu_dropdown" Then
        go_to_dropdown.Items.Clear()
        go_to_dropdown.Items.Add(New ListItem(selectedLI.Text, ""))

        SetPageNumber(CInt(selectedLI.Text))
        MovePage(False, False, False, False, True, PageNumber)

      ElseIf sender.id.ToString = "per_page_submenu_dropdown" Then
        per_page_dropdown.Items.Clear()
        per_page_dropdown.Items.Add(New ListItem(selectedLI.Text & " ", selectedLI.Text))
        Session.Item("localUser").crmUserRecsPerPage = CInt(selectedLI.Value)
        MovePage(False, False, False, False, False, PageNumber)

      End If

    Catch ex As Exception
      Master.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " (Yacht): " & ex.Message)
    End Try
  End Sub


  ''' <summary>
  ''' Runs and calls an event on search click. This calls an event which is then handled by the main.aspx page.
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub search_Click(ByVal sender As Object, ByVal e As System.EventArgs, Optional ByVal LoadFromSession As Boolean = False) Handles search.Click
    PanelCollapseEx.Collapsed = True
    PanelCollapseEx.ClientState = True

    Dim US_Waters_Flag As String = ""
    Dim ForSale_Flag As String = ""
    Dim ForLease_Flag As String = ""
    Dim ForCharter_Flag As String = ""
    Dim HullMFR_Start As String = ""
    Dim HullMFR_End As String = ""
    Dim UseAltHullMFR As Boolean = False

    Dim BuildSearchString As String = ""
    Dim LifeCycleStage_String As String = ""
    Dim LifeCycleStatus_String As String = ""
    Dim type_String As String = ""
    Dim size_String As String = ""
    Dim Status As String = ""
    Dim Ownership_String As String = ""
    Dim PreviouslyOwned_Flag As String = ""


    Dim model_type_string As String = ""
    Dim PreviousName As Boolean = False
    Dim brand_string As String = ""
    Dim make_string As String = ""
    Dim model_string As String = ""
    Dim YearString As String = ""
    Dim YearDlv As String = ""
    Dim YachtClass As String = ""
    Dim yacht_name As String = ""
    Dim YachtIDs As String = ""
    Dim YachtCountryRegistered As String = ""
    Dim CallSign As String = ""

    'Event Listing Variables
    Dim EventTypeOfSearch As String = ""
    Dim MarketCategory As String = ""
    Dim MarketType As String = ""
    Dim StartDate As Date = Now()
    Dim Months As Integer = 0
    Dim Days As Integer = 0
    Dim Hours As Integer = 0
    Dim Minutes As Integer = 0
    Dim UseDefaultDate As Boolean = True
    Dim DynamicQueryString As String = ""
    Dim TotalCountHold As Integer = 0
    Dim Counter As Integer = 0
    Dim EventTransactionSearch As Boolean = False

    'Continent/Region/State/Timezone
    Dim CompanyRegionString As String = ""
    Dim CompanyContinentString As String = ""
    Dim CompanyTimeZoneString As String = ""
    Dim CompanyCountriesString As String = ""
    Dim CompanyStateName As String = ""

    Dim helipad_checked As String = ""

    Dim MFR_String As String = "" 'Used as a variable to get the information out of the listbox.
    Dim BrandMFR_String As String = "" 'Used as a variable that's passed to the dataquery function. This allows us to store the OR clause that we need.

    Dim JournalDate As String = ""
    Dim JournalDateOperator As String = ""
    Dim JournalTransType As String = ""
    Dim temp_metric As String = ""
    Dim ComplianceTypeText As String = ""

    Dim EngineManufacturerString As String = ""
    Dim EngineModelString As String = ""

    Dim yachtBrandNameSearch As String = ""

    'Setting up the search criteria class to save fields in session. 9/30/15
    Dim NewSearchClass As New SearchSelectionCriteria

    yacht_attention.Text = ""
    Initial(False)

    'Life Cycle Building
    LifeCycleStage_String = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(yt_lifecycle_id, False, 0, True)


    'We still need to go through and loop to get the ID/stage seperated
    Dim LifeCycleStage As Array = Split(LifeCycleStage_String, ",")
    Dim FullLifeCycleString As String = ""
    If LifeCycleStage_String <> "" Then

      'Saving LifeCycle in Session before we clear this variable to perform the ID/stage seperation. This is because we need to save the selection, not necessarily what we're searching on.
      NewSearchClass.SearchCriteriaYachtLifecycle = LifeCycleStage_String

      LifeCycleStage_String = ""
      For counting As Integer = 0 To UBound(LifeCycleStage)
        Dim splitFinalAnswer As Array = Split(LifeCycleStage(counting), "|")

        If FullLifeCycleString <> "" Then
          FullLifeCycleString += " or "
        End If

        If Trim(UCase(splitFinalAnswer(1))) = "OUT OF SERVICE" Then
          FullLifeCycleString += " (yt_lifecycle_id = '" & splitFinalAnswer(0) & "' and upper(yt_lifecycle_status) like ('%" & splitFinalAnswer(1) & "%')  ) "
        Else
          FullLifeCycleString += " (yt_lifecycle_id = '" & splitFinalAnswer(0) & "' and upper(yt_lifecycle_status) in ('" & splitFinalAnswer(1) & "')  ) "
        End If


      Next
      BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(yt_lifecycle_id, "Lifecycle")
    End If

    'Ownership Building
    'Ownership_String = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(ownership, True, 0, True)
    'If Ownership_String <> "" Then
    '  BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(ownership, "Ownership")
    'End If

    'Yt Compliance Type Text String:
    ComplianceTypeText = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(yt_compliance_type, False, 0, True)
    If ComplianceTypeText <> "" Then
      BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(yt_compliance_type, "Compliance Type")
    End If

    'This sets up the Year Mfr in the query class, builds with the advanced search string.
    If yt_year_mfr.Text <> "" Then
      Dim QueryData As New AdvancedQueryResults
      QueryData.FieldName = "yt_year_mfr"
      QueryData.OperatorChoice = operator_year_mfr.SelectedValue
      QueryData.DataType = "Year"
      QueryData.SearchValue = yt_year_mfr.Text
      QueryData.FieldDisplay = "Year Mfr"
      QueryData.SpecialConsideration = False
      Query_Class_Array.Add(QueryData)

      'Saving the year MFR in session
      NewSearchClass.SearchCriteriaYachtYearManufactured = yt_year_mfr.Text
      'Saving the year MFR operator in session
      NewSearchClass.SearchCriteriaYachtYearManufacturedOperator = operator_year_mfr.Text
    End If

    'This sets up the year delivered in the query class, meaning it builds with the advanced search string.
    If yt_year_dlv.Text <> "" Then
      Dim QueryData As New AdvancedQueryResults
      QueryData.FieldName = "yt_year_dlv"
      QueryData.OperatorChoice = operator_year_dlv.SelectedValue
      QueryData.DataType = "Year"
      QueryData.SearchValue = yt_year_dlv.Text
      QueryData.FieldDisplay = "Year Dlv"
      QueryData.SpecialConsideration = False
      Query_Class_Array.Add(QueryData)

      'Saving the year DLV in session
      NewSearchClass.SearchCriteriaYachtYearDelivered = yt_year_dlv.Text
      'Saving the year DLV operator in session
      NewSearchClass.SearchCriteriaYachtYearDeliveredOperator = operator_year_dlv.SelectedValue
    End If


    'This sets up the yacht length in the query class, meaning it builds with the advanced search string.
    If length_to.Text.ToString.Trim <> "" Then
      Dim QueryData As New AdvancedQueryResults
      QueryData.FieldName = "yt_length_overall_meters"
      QueryData.OperatorChoice = operator_length.SelectedValue
      QueryData.DataType = "Numeric"

      'Saving yacht length in session
      NewSearchClass.SearchCriteriaYachtLengthValue = length_to.Text
      'Saving yacht length US/Metric value
      NewSearchClass.SearchCriteriaYachtLengthStandard = IIf(us_standard.Checked, "US", "METRIC")
      'Saving Yacht Length Operator 
      NewSearchClass.SearchCriteriaYachtLengthOperator = operator_length.SelectedValue

      If us_standard.Checked Then
        If InStr(length_to.Text, ":") > 0 Then
          Dim metricAnswer As Array = Split(length_to.Text, ":")
          Dim FirstValue As Long = 0
          Dim SecondValue As Long = 0
          If UBound(metricAnswer) = 1 Then
            convert_us_to_metric(CDbl(metricAnswer(0)), FirstValue)
            convert_us_to_metric(CDbl(metricAnswer(1)), SecondValue)
          End If

          temp_metric = FirstValue & ":" & SecondValue
        Else
          convert_us_to_metric(CDbl(length_to.Text), temp_metric)
          temp_metric = Replace(FormatNumber(temp_metric, 2), ",", "")
        End If

      Else
        temp_metric = length_to.Text
      End If

      QueryData.SearchValue = temp_metric
      QueryData.FieldDisplay = "Length (in " & IIf(us_standard.Checked, "Meters", "Meters") & ")" 'Displays in the up top listbox as meters because of field being used to search
      QueryData.SpecialConsideration = False
      Query_Class_Array.Add(QueryData)
    End If


    If Trim(price_range.Text) <> "" Then
      'Dim QueryData As New AdvancedQueryResults
      'QueryData.FieldName = "yt_asking_price"
      'QueryData.OperatorChoice = operator_asking_price.SelectedValue
      'QueryData.DataType = "Numeric"

      'QueryData.SearchValue = Replace(price_range.Text, ",", "")
      'QueryData.FieldDisplay = "Asking Price"
      'QueryData.SpecialConsideration = False
      'Query_Class_Array.Add(QueryData)

      'Saving Yacht Asking Price
      NewSearchClass.SearchCriteriaYachtAskingPrice = Replace(price_range.Text, ",", "")
      'Saving Yacht Asking Currency
      NewSearchClass.SearchCriteriaYachtAskingPriceCurrency = price_range_currency.Text
      'Saving Yacht Asking Operator
      NewSearchClass.SearchCriteriaYachtAskingPriceOperator = operator_asking_price.SelectedValue

    End If

    'Days on market
    If Trim(days_on_market.Text) <> "" Then
      Dim QueryData As New AdvancedQueryResults
      QueryData.FieldName = "DATEDIFF(d,yt_forsale_list_date,getdate())"
      QueryData.OperatorChoice = operator_days_on_market.SelectedValue
      QueryData.DataType = "Numeric"

      QueryData.SearchValue = days_on_market.Text
      QueryData.FieldDisplay = "Days on Market"
      QueryData.SpecialConsideration = False
      Query_Class_Array.Add(QueryData)

      'Saving Yacht Days on Market
      NewSearchClass.SearchCriteriaYachtDOM = days_on_market.Text
      'Saving Yacht Days Operator
      NewSearchClass.SearchCriteriaYachtDOMOperator = operator_days_on_market.SelectedValue
    End If

    'For sale flag checked?
    If for_sale.Checked Then
      ForSale_Flag = "Y"
      BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(for_sale, "For Sale")
      'Saving for sale
      NewSearchClass.SearchCriteriaYachtForSale = for_sale.Checked
    End If

    'For lease flag checked?
    If for_lease.Checked Then
      ForLease_Flag = "Y"
      BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(for_lease, "For Lease")
      'Saving for lease 
      NewSearchClass.SearchCriteriaYachtForLease = for_lease.Checked
    End If


    'For charter flag checked?
    If for_charter.Checked Then
      ForCharter_Flag = "Y"
      BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(for_charter, "For Charter")
      'Saving for charter
      NewSearchClass.SearchCriteriaYachtForCharter = for_charter.Checked
    End If



    If US_waters.SelectedValue <> "" Then
      US_Waters_Flag = US_waters.SelectedValue
      BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(US_waters, "For Sale/Charter Restriction")
      'Saving for For Sale/Charter Restriction
      NewSearchClass.SearchCriteriaYachtSaleCharterRestrictions = US_waters.SelectedValue
    End If

    'Yacht Flag
    If country_registration.SelectedValue <> "" Then
      YachtCountryRegistered = country_registration.SelectedValue
      BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(country_registration, "Country of Registration")

      'saving yacht Flag
      NewSearchClass.SearchCriteriaYachtFlagOption = country_registration.SelectedValue
    End If

    Status = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(yt_market, True, 0, True)
    If Status <> "" Then
      BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(yt_market, "Status")

      'Saving status
      NewSearchClass.SearchCriteriaYachtMarketStatus = Status
    End If


    'Hull MFR from
    If Not String.IsNullOrEmpty(hull_MFR_from.Text) Then
      HullMFR_Start = hull_MFR_from.Text
      BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(HullMFR_Start, "Hull MFR ID Start")
    End If

    'Hull MFR to
    If Not String.IsNullOrEmpty(hull_MFR_to.Text) Then
      HullMFR_End = hull_MFR_to.Text
      BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(HullMFR_End, "Hull MFR ID End")
    End If

    'Use alternate hull MFR
    If search_alt_hull.Checked Then
      UseAltHullMFR = True
      BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(HullMFR_End, "Use Alternate Hull #")
    End If


    'If Not String.IsNullOrEmpty(yt_call_sign.Text) Then
    '  CallSign = yt_call_sign.Text
    '  BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(yt_call_sign, "Call Sign")
    'End If


    If Not String.IsNullOrEmpty(yt_call_sign.Text) Then
      'Saving   'CallSign in Session
      NewSearchClass.SearchCriteriaYachtCallSign = CallSign

      CallSign = Trim(yt_call_sign.Text)
      CallSign = CallSign.TrimEnd(";")
      CallSign = Replace(CallSign, ";", ",")

      CallSign = clsGeneral.clsGeneral.CleanUserData(CallSign, Constants.cEmptyString, Constants.cCommaDelim, True)

      If InStr(CallSign, ",") > 0 Then
        Dim QueryData As New AdvancedQueryResults
        QueryData.FieldName = " yt_radio_call_sign"
        QueryData.OperatorChoice = "Begins With"

        QueryData.DataType = "String"
        QueryData.SearchValue = CallSign
        QueryData.FieldDisplay = "Call Sign"
        Query_Class_Array.Add(QueryData)
        CallSign = ""
      End If

      'CallSign String Building Textual Display
      BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(yt_call_sign, "Call Sign")
    End If


    'Yacht name search
    If Trim(yacht_name_search.Text) <> "" Then
      yacht_name = yacht_name_search.Text
      yacht_name = Replace(yacht_name, "'", "''")
      BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(yacht_name_search, "Name")

      'Savign yacht name
      NewSearchClass.SearchCriteriaYachtName = yacht_name
    End If

    'Yacht ID / Yacht Folder
    If yt_id.Text <> "" Then
      YachtIDs = clsGeneral.clsGeneral.StripChars(yt_id.Text, True)
      BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(yt_id, "Yacht ID(s)")
      If folder_name.Text <> "" Then
        BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(folder_name, "Folder")
      End If
    End If

    If ypn_previous_name.Checked Then
      PreviousName = True
      BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(ypn_previous_name, "Search Previous Name(s)")
      'Save Previous Name
      NewSearchClass.SearchCriteriaYachtPreviousName = ypn_previous_name.Checked
    End If

    If yacht_class.SelectedValue <> "" Then
      YachtClass = yacht_class.SelectedValue
      BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(yacht_class, "Classification")
      'Saving Yacht Class
      NewSearchClass.SearchCriteriaYachtClass = YachtClass
    End If


    'We need to use an OR here instead of an in clause, which means we need to change the way we pull this information.
    'This is going to pull a piped list with mfr|brand.
    MFR_String = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(ym_mfr_comp_id, False, 0, True)

    'Check to see if anything was selected.
    If MFR_String <> "" Then
      'Split the commas (multiple selected items in listbox..
      Dim MFRBrandArray As Array = Split(MFR_String, ",")
      'Make sure this is initialized at empty.
      BrandMFR_String = ""
      'For each item in the splt array.
      For counting As Integer = 0 To UBound(MFRBrandArray)
        Dim EachMFRBrandItemArray As Array = Split(MFRBrandArray(counting), "|") 'Split the piping of each individual item.
        If UBound(EachMFRBrandItemArray) = 1 Then
          'This means that both items are there.
          If BrandMFR_String <> "" Then
            BrandMFR_String += " or " 'We need an or there if the clause is already established.
          End If

          BrandMFR_String += " ( "
          'These need to be escaped.###
          BrandMFR_String += " ym_brand_name = '" & clsGeneral.clsGeneral.StripChars(EachMFRBrandItemArray(1).ToString, False) & "' and ym_mfr_comp_id = '" & clsGeneral.clsGeneral.StripChars(EachMFRBrandItemArray(0).ToString, False) & "' "
          BrandMFR_String += " ) "
        End If

      Next
      'This is going to go ahead and build the search string display that the top of the page.
      'It takes the select box and fills it up with MFR/Brand
      BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(ym_mfr_comp_id, "Manufacturer/Brand")
      If BrandMFR_String <> "" Then
        BrandMFR_String = " (" & BrandMFR_String & ")"
      End If
    End If



    If History Then
      If Not String.IsNullOrEmpty(journ_date_operator.SelectedValue) Then
        JournalDateOperator = journ_date_operator.SelectedValue
        NewSearchClass.SearchCriteriaYachtTransactionDateOperator = JournalDateOperator
      End If

      If Not String.IsNullOrEmpty(journ_date.Text) Then
        NewSearchClass.SearchCriteriaYachtTransactionDate = journ_date.Text
        If IsDate(journ_date.Text) Then
          JournalDate = Month(journ_date.Text) & "/" & Day(journ_date.Text) & "/" & Year(journ_date.Text)
          If Not String.IsNullOrEmpty(journ_date_operator.SelectedValue) Then
            JournalDate = clsGeneral.clsGeneral.PrepQueryString(JournalDateOperator, JournalDate, "Date", False, "", True)
          Else
            JournalDate = ""
          End If
          BuildSearchString += "Journal Date " & JournalDateOperator & " " & journ_date.Text & "<br />"
        ElseIf JournalDateOperator = "Between" And InStr(journ_date.Text, ":") Then
          JournalDate = clsGeneral.clsGeneral.PrepQueryString(JournalDateOperator, journ_date.Text, "Date", False, "", True)
          BuildSearchString += "Journal Date " & JournalDateOperator & " " & Replace(journ_date.Text, ":", " and ") & "<br />"

        End If
      End If

      If Not String.IsNullOrEmpty(journ_trans_type.SelectedValue) Then

        JournalTransType = journ_trans_type.SelectedValue
        NewSearchClass.SearchCriteriaYachtTransactionType = JournalTransType

        BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(journ_trans_type, "Transaction Type")

      End If

      '
    End If

    GetYachtTypeSizeBrandModelFromCommonControl("tab", "Yacht", type_String, size_String, brand_string, model_string, BuildSearchString)

    'This function grabs the all the region information from the locaton control for Company Side
    DisplayFunctions.GetRegionInfoFromCommonControl("Company", BuildSearchString, CompanyCountriesString, CompanyTimeZoneString, CompanyContinentString, CompanyRegionString, CompanyStateName)


    'Response.Write()

    'Look through the market event only information
    If MarketEvent Then
      DisplayFunctions.ToGrabTheEventOnlyInformation(MarketEvent, EventTypeOfSearch, MarketCategory, MarketType, Months, Days, Hours, Minutes, UseDefaultDate, StartDate, BuildSearchString, New SearchSelectionCriteria, market_category, market_type, New RadioButtonList, events_months, event_days, event_hours, event_minutes)

      NewSearchClass.SearchCriteriaYachtCategory = MarketCategory
      NewSearchClass.SearchCriteriaYachtType = MarketType
      NewSearchClass.SearchCriteriaYachtEventDays = Days
      NewSearchClass.SearchCriteriaYachtEventMonths = Months
      NewSearchClass.SearchCriteriaYachtEventHours = Hours
      NewSearchClass.SearchCriteriaYachtEventMinutes = Minutes
      If InStr(MarketCategory, "TRANSACTION") > 0 Then
        EventTransactionSearch = True
      End If
    End If

    If Trim(Request("h")) = "1" Then
      HttpContext.Current.Session.Item("IS_YACHT_HISTORY") = "Y"
    Else
      HttpContext.Current.Session.Item("IS_YACHT_HISTORY") = "N"
    End If


    If comp_not_in_selected.Checked = True Then
      DynamicQueryString = AdvancedQueryResults.BuildDynamicString(Master.aclsData_Temp, Query_Class_Array, yacht_advanced_search, BuildSearchString, New TextBox, New TextBox, New TextBox, New TextBox, TotalCountHold, Counter, False, "YACHT", Nothing, Nothing, True)
    Else
      DynamicQueryString = AdvancedQueryResults.BuildDynamicString(Master.aclsData_Temp, Query_Class_Array, yacht_advanced_search, BuildSearchString, New TextBox, New TextBox, New TextBox, New TextBox, TotalCountHold, Counter, False, "YACHT", Nothing, Nothing)
    End If




    EngineManufacturerString = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(yt_engine_manufacturer, False, 0, True)
    EngineModelString = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(yt_engine_model, False, 0, True)

    If EngineManufacturerString <> "" Then
      If DynamicQueryString <> "" Then
        DynamicQueryString += " and "
      End If
      BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(yt_engine_manufacturer, "Engine Manufacturer")

      DynamicQueryString += " yt_id in (select distinct ye_yt_id from Yacht_Engines with (NOLOCK) "
      DynamicQueryString += " inner join Yacht_Engine_Models with (NOLOCK) on ye_engine_model_id = yem_engine_model_id "
      DynamicQueryString += " inner join Company with (NOLOCK) on yem_engine_mfr_comp_id = comp_id and comp_journ_id = 0 "
      DynamicQueryString += " where ( ye_journ_id = 0 "
      DynamicQueryString += " and comp_id in (" & EngineManufacturerString & ") "

      If EngineModelString <> "" Then
        DynamicQueryString += " and yem_engine_model_id in (" & EngineModelString & ") "
        BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(yt_engine_model, "Engine Model")
      End If
      DynamicQueryString += " )) "

    End If


    If search_yt_mfr_brand.Text <> "" Then
      yachtBrandNameSearch = clsGeneral.clsGeneral.StripChars(search_yt_mfr_brand.Text, False)
    End If

    If yachtBrandNameSearch <> "" Then
      If DynamicQueryString <> "" Then
        DynamicQueryString += " and "
      End If

      BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(search_yt_mfr_brand, "Search Mfr/Brand/Model Name")

      DynamicQueryString += " (ym_brand_name like ('%" & yachtBrandNameSearch & "%') or ym_model_name like ('%" & yachtBrandNameSearch & "%')"
      DynamicQueryString += " or ym_mfr_comp_id in (select distinct comp_id from Company with (NOLOCK) "
      DynamicQueryString += " where comp_name like '%" & yachtBrandNameSearch & "%' and comp_journ_id=0 "
      DynamicQueryString += " and comp_product_yacht_flag='Y'))"
    End If

    Dim CompanyWhereString As String = ""
    If CompanyRegionString <> "" Then
      If CompanyStateName <> "" Then
        CompanyWhereString = AdvancedQueryResults.BuildRegionWhereString("state_name", "comp_country", Master.aclsData_Temp, CompanyStateName, CompanyCountriesString, CompanyRegionString)

        If DynamicQueryString <> "" Then
          DynamicQueryString += " and (" & CompanyWhereString & ")"
        Else
          DynamicQueryString += "(" & CompanyWhereString & ")"
        End If
        CompanyStateName = ""
        CompanyCountriesString = ""
        CompanyRegionString = ""
      End If
    End If

    'Saving Search to session  

    Session.Item("searchCriteria") = NewSearchClass

    If MarketEvent = False Then
      Yacht_Search(model_string, ForSale_Flag, _
                   ForLease_Flag, ForCharter_Flag, _
                   HullMFR_Start, HullMFR_End, _
                   CallSign, FullLifeCycleString, _
                   Status, Ownership_String, _
                   PreviouslyOwned_Flag, model_type_string, _
                   make_string, brand_string, _
                   YearString, size_String, _
                   type_String, PageNumber, _
                   PageSort, LoadFromSession, _
                   yacht_name, BuildSearchString, _
                   YachtIDs, model_string, _
                   YachtCountryRegistered, YearDlv, _
                   YachtClass, PreviousName, _
                   CompanyCountriesString, _
                   CompanyTimeZoneString, CompanyContinentString, _
                   CompanyRegionString, CompanyStateName, _
                   DynamicQueryString, helipad_checked, _
                   BrandMFR_String, JournalDate, JournalTransType, _
                   UseAltHullMFR, US_Waters_Flag, _
                   NewSearchClass.SearchCriteriaYachtAskingPrice, _
                   NewSearchClass.SearchCriteriaYachtAskingPriceCurrency, _
                   NewSearchClass.SearchCriteriaYachtAskingPriceOperator)
    Else
      Event_Search(model_string, ForSale_Flag, _
                   ForLease_Flag, ForCharter_Flag, _
                   HullMFR_Start, HullMFR_End, _
                   CallSign, FullLifeCycleString, _
                   Status, Ownership_String, _
                   PreviouslyOwned_Flag, model_type_string, _
                   make_string, brand_string, _
                   YearString, size_String, _
                   type_String, PageNumber, _
                   PageSort, LoadFromSession, _
                   yacht_name, BuildSearchString, _
                   YachtIDs, model_string, _
                   MarketCategory, MarketType, _
                   Format(CDate(StartDate), "MM/dd/yyyy hh:mm:ss tt"), YachtCountryRegistered, _
                   YearDlv, YachtClass, _
                   PreviousName, _
                   DynamicQueryString, UseAltHullMFR, _
                   EventTransactionSearch, US_Waters_Flag, _
                   CompanyCountriesString, CompanyTimeZoneString, _
                   CompanyContinentString, CompanyRegionString, _
                   CompanyStateName, BrandMFR_String, _
                   NewSearchClass.SearchCriteriaYachtAskingPrice, _
                   NewSearchClass.SearchCriteriaYachtAskingPriceCurrency, _
                   NewSearchClass.SearchCriteriaYachtAskingPriceOperator)
    End If

    'This has to be ran after the search to rebuild the attributes tab.

    Dim MainContent As New ContentPlaceHolder
    If Not IsNothing(Page.Master.FindControl("ContentPlaceHolder1")) Then
      MainContent = TryCast(Page.Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
    End If

    AttrTab.Controls.Remove(AttributesPanel)

    Dim newPanel As New Panel
    AdvancedQueryResults.DealWithAttributeTab(MainContent.ClientID, newPanel, AttrTab, yacht_advanced_search, Me, Master.aclsData_Temp, yacht_attention)
    AttrTab.Controls.Add(newPanel)

  End Sub

  Public Sub convert_metric_to_us1(ByRef us_value As Double, ByVal metric As Double)

    Dim english As Double = 0.0
    Dim feet As Integer = 0
    Dim inches As Integer = 0


    english = (metric * 3.28084)
    feet = Int(english)
    inches = (english - feet) * 12
    inches = FormatNumber(inches, 0)

    If inches = 12 Then
      feet = feet + 1
      inches = 0
    End If


    us_value = CDbl(Trim(feet & "." & inches))

  End Sub ' convert_metric_to_us1

  Public Sub convert_us_to_metric(ByVal us_value As Double, ByRef metric As String)

    metric = CDbl(us_value * 0.3048)

  End Sub ' convert_metric_to_us1

  Public Sub Fill_Page_To_To_Dropdown(ByVal pageNumber As Integer)
    go_to_submenu_dropdown.Items.Clear()
    For x = 1 To pageNumber
      go_to_submenu_dropdown.Items.Add(New ListItem(x, x))
    Next
  End Sub

  Private Sub next__Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles next_.Click, previous.Click, next_all.Click, previous_all.Click, next_2.Click, previous2.Click, next_all2.Click, previous_all2.Click
    If sender.id.ToString = "next_" Or sender.id.ToString = "next_2" Then
      MovePage(True, False, False, False, False, 0)
    ElseIf sender.id.ToString = "previous" Or sender.id.ToString = "previous2" Then
      MovePage(False, True, False, False, False, 0)
    ElseIf sender.id.ToString = "next_all" Or sender.id.ToString = "next_all2" Then
      MovePage(False, False, True, False, False, 0)
    ElseIf sender.id.ToString = "previous_all" Or sender.id.ToString = "previous_all2" Then
      MovePage(False, False, False, True, False, 0)
    End If
  End Sub
  ''' <summary>
  ''' Alter Listing is an event that toggles datagrid/datalist on/off
  ''' Error reporting is included.
  ''' </summary>
  ''' <param name="TypeOfListing"></param>
  ''' <remarks></remarks>
  Public Sub AlterListing(ByVal TypeOfListing As Integer, ByVal RecordAmount As Integer)
    Try

      Select Case TypeOfListing
        Case 0 'Listing Display
          SelectedDataList.Visible = False
          SelectedDataGrid.Visible = True
        Case 1 'Image Display
          SelectedDataList.Visible = True
          SelectedDataGrid.Visible = False
      End Select

      SelectedDataList.Dispose()
      SelectedDataGrid.Dispose()

    Catch ex As Exception
      Master.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " (Yacht): " & ex.Message)
    End Try
  End Sub
  ''' <summary>
  ''' Toggles visibility of next/prev
  ''' </summary>
  ''' <param name="back_page"></param>
  ''' <param name="next_page"></param>
  ''' <remarks></remarks>
  Public Sub SetPagingButtons(ByVal back_page As Boolean, ByVal next_page As Boolean)

    previous_all.Visible = back_page
    previous_all2.Visible = back_page


    previous.Visible = back_page
    previous2.Visible = back_page

    next_all.Visible = next_page
    next_all2.Visible = next_page

    next_.Visible = next_page
    next_2.Visible = next_page

  End Sub

  Private Sub YachtListing_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
    If Not Page.IsPostBack Then
      If Yacht_Criteria.Visible = True Then

        If MarketEvent Then
          SwitchGalleryListing(0)
        Else
          SwitchGalleryListing(Session.Item("localUser").crmACListingView)
        End If
      End If
    End If



    If Page.Request.Form("project_search") = "Y" Or Page.Request.Form("complete_search") = "Y" Then
      search_Click(search, EventArgs.Empty)
    End If
  End Sub

  Private Sub YachtListing_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
    If Not IsNothing(Request.Item("e")) Then
      If Not String.IsNullOrEmpty(Request.Item("e").ToString) Then
        MarketEvent = True
        ErrorReportingTypeString = "Events"
      End If
    End If

    If Not IsNothing(Request.Item("h")) Then
      If Not String.IsNullOrEmpty(Request.Item("h").ToString) Then
        History = True
        ErrorReportingTypeString = "History"
      End If
    End If

    If Not IsNothing(Request.Item("restart")) Then
      If Not String.IsNullOrEmpty(Request.Item("restart").ToString) Then
        If Request.Item("restart") = "1" Then
          Reset_Page()
        End If
      End If
    End If
    If Page.Request.Form("complete_search") = "Y" Or Page.Request.Form("project_search") = "Y" Then
      ClearSavedSelection()
    End If

    Me.MasterPageFile = "~/EvoStyles/YachtTheme.master"
    masterPage = DirectCast(Page.Master, YachtTheme)

  End Sub

  Public Sub Reset_Page()
    ClearSavedSelection()
    If MarketEvent Then
      Response.Redirect("YachtListing.aspx?e=1")
    ElseIf History Then
      Response.Redirect("YachtListing.aspx?h=1")
    Else
      Response.Redirect("YachtListing.aspx")
    End If

  End Sub

  Public Sub ClearSavedSelection()
    Session.Item("tabYachtModel") = ""
    Session.Item("tabYachtBrand") = ""
    Session.Item("tabYachtCategory") = ""

    Session.Item("tabYachtSize") = ""
    Session.Item("tabYachtType") = ""

    Session.Item("MasterYachtWhere") = ""
    Session.Item("MasterYachtFrom") = ""
    Session.Item("MasterYachtSelect") = ""
    Session.Item("MasterYachtSort") = ""

    HttpContext.Current.Session.Item("companyRegion") = ""
    HttpContext.Current.Session.Item("companyRegionOrContinent") = "continent"
    HttpContext.Current.Session.Item("companyTimeZone") = ""
    HttpContext.Current.Session.Item("companyCountry") = ""
    HttpContext.Current.Session.Item("companyState") = ""

    'Clear the search class/reset it
    Session.Item("searchCriteria") = New SearchSelectionCriteria
  End Sub

  ''' <summary>
  ''' Fills out Search Parameters 
  ''' </summary>
  ''' <remarks></remarks>
  Private Sub FillOutSearchParameters()
    Try
      'Filling Back in the Search Criteria.

      'All Pages

      'Yacht Name Search Criteria.
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaYachtName) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaYachtName) Then
          yacht_name_search.Text = Session.Item("searchCriteria").SearchCriteriaYachtName.ToString
        End If
      End If

      'Yacht Name Search Criteria.
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaYachtName) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaYachtName) Then
          yacht_name_search.Text = Session.Item("searchCriteria").SearchCriteriaYachtName.ToString
        End If
      End If

      'Yacht Previous Name Checked
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaYachtPreviousName) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaYachtPreviousName) Then
          ypn_previous_name.Checked = Session.Item("searchCriteria").SearchCriteriaYachtPreviousName
        End If
      End If

      'Charter for Sale restriction
      If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaYachtSaleCharterRestrictions) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaYachtSaleCharterRestrictions) Then
          US_waters.SelectedValue = Session.Item("searchCriteria").SearchCriteriaYachtSaleCharterRestrictions
        End If
      End If


      'Flag
      If Not IsNothing(Session.Item("searchCriteria").searchCriteriaYachtFlagOption) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").searchCriteriaYachtFlagOption) Then
          country_registration.SelectedValue = Session.Item("searchCriteria").searchCriteriaYachtFlagOption
        End If
      End If

      'Length Operator
      If Not IsNothing(Session.Item("searchCriteria").searchCriteriaYachtLengthOperator) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").searchCriteriaYachtLengthOperator) Then
          operator_length.SelectedValue = Session.Item("searchCriteria").searchCriteriaYachtLengthOperator
        End If
      End If

      'Length
      If Not IsNothing(Session.Item("searchCriteria").searchCriteriaYachtLengthValue) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").searchCriteriaYachtLengthValue) Then
          length_to.Text = Session.Item("searchCriteria").searchCriteriaYachtLengthValue
        End If
      End If

      'Standard
      If Not IsNothing(Session.Item("searchCriteria").searchCriteriaYachtLengthStandard) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").searchCriteriaYachtLengthStandard) Then
          If Session.Item("searchCriteria").searchCriteriaYachtLengthStandard = "US" Then
            us_standard.Checked = True
            metric_standard.Checked = False
          Else
            us_standard.Checked = False
            metric_standard.Checked = True
          End If
        End If
      End If

      'Yacht class
      If Not IsNothing(Session.Item("searchCriteria").searchCriteriaYachtClass) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").searchCriteriaYachtClass) Then
          yacht_class.SelectedValue = Session.Item("searchCriteria").searchCriteriaYachtClass
        End If
      End If


      'Call Sign
      If Not IsNothing(Session.Item("searchCriteria").searchCriteriaYachtCallSign) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").searchCriteriaYachtCallSign) Then
          yt_call_sign.Text = Session.Item("searchCriteria").searchCriteriaYachtCallSign
        End If
      End If

      'Year Delivered Operator
      If Not IsNothing(Session.Item("searchCriteria").searchCriteriaYachtYearDeliveredOperator) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").searchCriteriaYachtYearDeliveredOperator) Then
          operator_year_dlv.SelectedValue = Session.Item("searchCriteria").searchCriteriaYachtYearDeliveredOperator
        End If
      End If

      'Year Delivered 
      If Not IsNothing(Session.Item("searchCriteria").searchCriteriaYachtYearDelivered) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").searchCriteriaYachtYearDelivered) Then
          yt_year_dlv.Text = Session.Item("searchCriteria").searchCriteriaYachtYearDelivered
        End If
      End If

      'Year MFR Operator
      If Not IsNothing(Session.Item("searchCriteria").searchCriteriaYachtYearManufacturedOperator) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").searchCriteriaYachtYearManufacturedOperator) Then
          operator_year_mfr.SelectedValue = Session.Item("searchCriteria").searchCriteriaYachtYearManufacturedOperator
        End If
      End If

      'Year MFR
      If Not IsNothing(Session.Item("searchCriteria").searchCriteriaYachtYearManufactured) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").searchCriteriaYachtYearManufactured) Then
          yt_year_mfr.Text = Session.Item("searchCriteria").searchCriteriaYachtYearManufactured
        End If
      End If

      'For Sale
      If Not IsNothing(Session.Item("searchCriteria").searchCriteriaYachtForSale) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").searchCriteriaYachtForSale) Then
          If (Session.Item("searchCriteria").searchCriteriaYachtForSale) Then
            for_sale.Checked = True
          End If
        End If
      End If

      'For Lease
      If Not IsNothing(Session.Item("searchCriteria").searchCriteriaYachtForLease) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").searchCriteriaYachtForLease) Then
          If (Session.Item("searchCriteria").searchCriteriaYachtForLease) Then
            for_lease.Checked = True
          End If
        End If
      End If

      'For Charter
      If Not IsNothing(Session.Item("searchCriteria").searchCriteriaYachtForCharter) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").searchCriteriaYachtForCharter) Then
          If (Session.Item("searchCriteria").searchCriteriaYachtForCharter) Then
            for_charter.Checked = True
          End If
        End If
      End If

      'Asking Price Operator
      If Not IsNothing(Session.Item("searchCriteria").searchCriteriaYachtAskingPriceOperator) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").searchCriteriaYachtAskingPriceOperator) Then
          operator_asking_price.SelectedValue = Session.Item("searchCriteria").searchCriteriaYachtAskingPriceOperator
        End If
      End If

      'Asking Price 
      If Not IsNothing(Session.Item("searchCriteria").searchCriteriaYachtAskingPrice) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").searchCriteriaYachtAskingPrice) Then
          price_range.Text = Session.Item("searchCriteria").searchCriteriaYachtAskingPrice
        End If
      End If

      'Asking Price Currency
      If Not IsNothing(Session.Item("searchCriteria").searchCriteriaYachtAskingPriceCurrency) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").searchCriteriaYachtAskingPriceCurrency) Then
          price_range_currency.SelectedValue = Session.Item("searchCriteria").searchCriteriaYachtAskingPriceCurrency
        End If
      End If

      'DOM Operator
      If Not IsNothing(Session.Item("searchCriteria").searchCriteriaYachtDOMOperator) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").searchCriteriaYachtDOMOperator) Then
          operator_days_on_market.SelectedValue = Session.Item("searchCriteria").searchCriteriaYachtDOMOperator
        End If
      End If

      'DOM
      If Not IsNothing(Session.Item("searchCriteria").searchCriteriaYachtDOM) Then
        If Not String.IsNullOrEmpty(Session.Item("searchCriteria").searchCriteriaYachtDOM) Then
          days_on_market.Text = Session.Item("searchCriteria").searchCriteriaYachtDOM
        End If
      End If

      '---------------------------------------------------------------------------------------------
      If MarketEvent Then
        'Event Months Only.
        If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaYachtEventMonths) Then
          If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaYachtEventMonths) Then
            If Session.Item("searchCriteria").SearchCriteriaYachtEventMonths <> 0 Then
              events_months.Text = Session.Item("searchCriteria").SearchCriteriaYachtEventMonths
            End If
          End If
        End If
        'Event Days Only.
        If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaYachtEventDays) Then
          If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaYachtEventDays) Then
            If Session.Item("searchCriteria").SearchCriteriaYachtEventDays <> 0 Then
              event_days.Text = Session.Item("searchCriteria").SearchCriteriaYachtEventDays
            Else
              'This is a special case added for what happens when your event session day isn't there and the textbox defaults to 1.
              'We need to run a check against all other variables. If there's anything in any of those other months/minutes/hours besides 0, then
              'we need to clear this out and make it 0.
              If (Session.Item("searchCriteria").SearchCriteriaYachtEventMonths <> 0 Or Session.Item("searchCriteria").SearchCriteriaYachtEventHours <> 0 Or Session.Item("searchCriteria").SearchCriteriaYachtEventMinutes <> 0) Then
                event_days.Text = "0"
              End If
            End If
          End If
        End If
        'Event Hours Only.
        If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaYachtEventHours) Then
          If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaYachtEventHours) Then
            If Session.Item("searchCriteria").SearchCriteriaYachtEventHours <> 0 Then
              event_hours.Text = Session.Item("searchCriteria").SearchCriteriaYachtEventHours
            End If
          End If
        End If
        'Event Minutes Only.
        If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaYachtEventMinutes) Then
          If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaYachtEventMinutes) Then
            If Session.Item("searchCriteria").SearchCriteriaYachtEventMinutes <> 0 Then
              event_minutes.Text = Session.Item("searchCriteria").SearchCriteriaYachtEventMinutes
            End If
          End If
        End If

        'Event Category
        If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaYachtCategory) Then
          If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaYachtCategory) Then
            Dim EventCategorySelection As Array
            EventCategorySelection = Split(Replace(Session.Item("searchCriteria").SearchCriteriaYachtCategory, "'", ""), ",")
            market_category.SelectedIndex = -1 'This will remove any previously selected items in the listbox, such as the selection of all
            'that the page defaults to.
            For EventCategorySelectionCount = 0 To UBound(EventCategorySelection)
              For ListBoxCount As Integer = 0 To market_category.Items.Count() - 1
                If UCase(market_category.Items(ListBoxCount).Value) = UCase(EventCategorySelection(EventCategorySelectionCount)) Then
                  market_category.Items(ListBoxCount).Selected = True
                End If
              Next
            Next
            market_category_SelectedIndexChanged(market_category, EventArgs.Empty)
          End If
        End If
        'Event Types
        If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaYachtType) Then
          If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaYachtType) Then
            Dim EventTypeSelection As Array
            EventTypeSelection = Split(Replace(Session.Item("searchCriteria").SearchCriteriaYachtType, "'", ""), ",")
            market_type.SelectedIndex = -1 'This will remove any previously selected items in the listbox, such as the selection of all
            'that the page defaults to.
            For EventTypeSelectionCount = 0 To UBound(EventTypeSelection)
              For ListBoxCount As Integer = 0 To market_type.Items.Count() - 1
                If UCase(market_type.Items(ListBoxCount).Value) = UCase(EventTypeSelection(EventTypeSelectionCount)) Then
                  market_type.Items(ListBoxCount).Selected = True
                End If
              Next
            Next
          End If
        End If
      End If
      '---------------------------------------------------------------------------------------------
      If History = True Then

        If Not IsNothing(Session.Item("searchCriteria").searchCriteriaYachtTransactionDateOperator) Then
          If Not String.IsNullOrEmpty(Session.Item("searchCriteria").searchCriteriaYachtTransactionDateOperator) Then
            journ_date_operator.SelectedValue = Session.Item("searchCriteria").searchCriteriaYachtTransactionDateOperator
          End If
        End If

        If Not IsNothing(Session.Item("searchCriteria").searchCriteriaYachtTransactionDate) Then
          If Not String.IsNullOrEmpty(Session.Item("searchCriteria").searchCriteriaYachtTransactionDate) Then
            journ_date.Text = Session.Item("searchCriteria").searchCriteriaYachtTransactionDate
          End If
        End If

        If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaYachtTransactionType) Then
          If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaYachtTransactionType) Then
            journ_trans_type.SelectedValue = Session.Item("searchCriteria").SearchCriteriaYachtTransactionType
          End If
        End If

      End If
      '---------------------------------------------------------------------------------------------
      If MarketEvent = False And History = False Then

        'Market Status Search Criteria.
        If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaYachtMarketStatus) Then
          If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaYachtMarketStatus) Then
            Dim MarketSelection As Array
            MarketSelection = Split(Replace(Session.Item("searchCriteria").SearchCriteriaYachtMarketStatus, "'", ""), ",")
            yt_market.SelectedIndex = -1 'This will remove any previously selected items in the listbox, such as the selection of all
            'that the page defaults to.
            For MarketSelectionCount = 0 To UBound(MarketSelection)
              For ListBoxCount As Integer = 0 To yt_market.Items.Count() - 1
                If UCase(yt_market.Items(ListBoxCount).Value) = UCase(MarketSelection(MarketSelectionCount)) Then
                  yt_market.Items(ListBoxCount).Selected = True
                End If
              Next
            Next
          End If
        End If

        'Life Cycle
        If Not IsNothing(Session.Item("searchCriteria").SearchCriteriaYachtLifecycle) Then
          If Not String.IsNullOrEmpty(Session.Item("searchCriteria").SearchCriteriaYachtLifecycle) Then
            Dim LifeCycleSelection As Array
            LifeCycleSelection = Split(Replace(Session.Item("searchCriteria").SearchCriteriaYachtLifecycle, "'", ""), ",")
            yt_lifecycle_id.SelectedIndex = -1 'This will remove any previously selected items in the listbox, such as the selection of all
            'that the page defaults to.
            For LifeCycleSelectionCount = 0 To UBound(LifeCycleSelection)
              For ListBoxCount As Integer = 0 To yt_lifecycle_id.Items.Count() - 1
                If UCase(yt_lifecycle_id.Items(ListBoxCount).Value) = UCase(LifeCycleSelection(LifeCycleSelectionCount)) Then
                  yt_lifecycle_id.Items(ListBoxCount).Selected = True
                End If
              Next
            Next
          End If
        End If


      End If
    Catch ex As Exception
      Master.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " (" & ErrorReportingTypeString & "): " & ex.Message)
    End Try
  End Sub

  Private Sub market_category_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles market_category.SelectedIndexChanged
    Dim MarketCategory As String = ""
    Dim MarketType As New DataTable
    If market_category.SelectedValue <> "" Then
      MarketCategory = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(market_category, True, 0, False)

      MarketType = Master.aclsData_Temp.ListOfYachtMarketTypes(MarketCategory)
      clsGeneral.clsGeneral.Populate_Listbox(MarketType, market_type, "ypec_category_name", "ypec_category_name", False)


    End If
  End Sub


  Private Sub BuildAdvancedSearch()
    Dim MainContent As New ContentPlaceHolder
    'Added 9/10/2015.
    'This is going to set up the master page content holder so we can go ahead and reference it later on for the date range picker.
    If Not IsNothing(Page.Master.FindControl("ContentPlaceHolder1")) Then
      MainContent = TryCast(Page.Master.FindControl("ContentPlaceHolder1"), ContentPlaceHolder)
    End If

    Dim TemporaryTable As New DataTable
    Dim Counter As Integer = 1
    Dim SubCounter As Integer = 0
    Dim TempPanel As New AjaxControlToolkit.TabPanel
    '-- TAB IDS *********************************
    '-- 40 = Maintenance
    '-- 43 = Interior
    '-- 36 = Exterior
    '-- 37 = Equipment
    '-- 39 = Bridge
    '-- 35 = Systems
    '-- 38 = Power

    TemporaryTable = Master.aclsData_Temp.ListofTabsForCustomSearch("Yacht")
    If Not IsNothing(TemporaryTable) Then
      If TemporaryTable.Rows.Count > 0 Then

        For Each r As DataRow In TemporaryTable.Rows
          If UCase(r("cefstab_sub_name").ToString) <> "COMPANY/CONTACT" And UCase(r("cefstab_sub_name").ToString) <> "CHARTER" Then

            Dim TemporaryFields As New DataTable

            Dim Tab As New Table
            Dim TR As New TableRow
            Dim TD As New TableCell
            Dim TD_2 As New TableCell
            Dim TDTEXT As New TextBox
            Dim TDSELECT As New DropDownList
            Dim TDSELECTCOMPARISON As New DropDownList
            Dim TD_3 As New TableCell
            Dim TD_4 As New TableCell
            Dim TD_5 As New TableCell 'For Validation Message.
            Dim LB As New Label
            Dim Display_Block As String = ""
            Dim cssClass As String = ""
            Dim SetUpLinkOutStart As String = ""
            Dim SetUpLinkOutEnd As String = ""


            'Validation Controls.
            Dim NumberCustom As New RegularExpressionValidator
            Dim CustomValidation As New CustomValidator

            TempPanel = New AjaxControlToolkit.TabPanel
            Tab.Width = Unit.Percentage(100D)
            Tab.CssClass = "data_aircraft_grid"
            Tab.CellPadding = 5

            TempPanel.ID = "TAB" & r("cefstab_id")
            TempPanel.HeaderText = r("cefstab_sub_name")
            TempPanel.Visible = True

            TemporaryFields = Master.aclsData_Temp.ListofTabsFieldsBasedonTabID(r("cefstab_id"))

            If Not IsNothing(TemporaryFields) Then



              If TemporaryFields.Rows.Count > 0 Then

                TR.CssClass = "header_row"
                TD.Text = "<b>Field</b>"
                TD.Width = Unit.Percentage(20D)
                TD_2.Text = "<b>Condition</b>"
                TD_2.Width = Unit.Percentage(15D)
                TD_3.Text = "<b>Value</b>"
                TD_3.Width = Unit.Percentage(15D)
                TD_4.Text = "<b>Format</b>"



                TD_4.Width = Unit.Percentage(38D)
                TD_5.Width = Unit.Percentage(12D)

                TR.Controls.Add(TD)
                TR.Controls.Add(TD_2)
                TR.Controls.Add(TD_3)
                TR.Controls.Add(TD_4)
                TR.Controls.Add(TD_5)
                Tab.Controls.Add(TR)

                SubCounter = 0
                Display_Block = ""

                For Each q As DataRow In TemporaryFields.Rows
                  'If q("cef_display") = "Boarding Accommodations" Then
                  'This is the block that shows the group heading. 
                  If Display_Block <> q("cefsblk_name").ToString Then
                    If UCase(r("cefstab_sub_name").ToString) = "HULL/DIMENSIONS" And UCase(q("cefsblk_name").ToString) = "HULL" Then
                    Else
                      TR = New TableRow
                      TR.CssClass = "header_row"
                      TD = New TableCell

                      If cssClass = "" Then
                        cssClass = "alt_row"
                      Else
                        cssClass = ""
                      End If


                      TD = New TableCell
                      TD.CssClass = "data_aircraft_grid_cell light_seafoam_green_header_color"
                      'TD.BackColor = System.Drawing.ColorTranslator.FromHtml("#e7eeeb")
                      TD.ColumnSpan = 5
                      TD.Text = "<b>" & q("cefsblk_name").ToString & "</b>"
                      TR.Controls.Add(TD)
                      Tab.Controls.Add(TR)
                    End If
                    Display_Block = q("cefsblk_name").ToString
                  End If

                  LB = New Label
                  TR = New TableRow
                  TR.CssClass = cssClass
                  TD = New TableCell
                  TD_2 = New TableCell
                  TD_3 = New TableCell
                  TD_4 = New TableCell
                  TD_5 = New TableCell

                  CustomValidation = New CustomValidator
                  NumberCustom = New RegularExpressionValidator
                  TDTEXT = New TextBox
                  TDSELECT = New DropDownList
                  TDSELECTCOMPARISON = New DropDownList
                  SetUpLinkOutStart = ""
                  SetUpLinkOutEnd = ""


                  'If there's a definition, switch this table cell class to a help cursor
                  'Also sets the tooltip.
                  If (q("cef_definition").ToString <> "") Then
                    TD.CssClass = "help_cursor"
                    TD.ToolTip = q("cef_definition").ToString
                  End If

                  If Not IsDBNull(q("cef_link")) Then
                    If Not String.IsNullOrEmpty(Trim(q("cef_link"))) Then
                      SetUpLinkOutStart = "<a href=""#"" onclick=""javascript:load('" & q("cef_link") & "','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;"">"
                      SetUpLinkOutEnd = "</a>"
                    End If
                  End If
                  'This displays the text in the first textbox, but also displays a magnifying glass (smaller than picture) if there is a definition (mouseover)
                  TD.Text = SetUpLinkOutStart & q("cef_display").ToString & SetUpLinkOutEnd & ": " & IIf(q("cef_definition").ToString <> "", "&nbsp;<img src='images/magnify_small.png' width='9' alt='" & q("cef_definition").ToString & "' title='" & q("cef_definition").ToString & "' />", "")


                  'Adds the tablecell
                  TR.Controls.Add(TD)

                  'Comparison Field

                  TDSELECTCOMPARISON.ID = "COMPARE_" + AdvancedQueryResults.EscapeSpecialCharactersInSearchIDs(q.Item("cef_evo_field_name").ToString).Trim


                  'Dim rgx As New Regex("\s+")
                  'TDSELECTCOMPARISON.ID = rgx.Replace(TDSELECTCOMPARISON.ID, " ")
                  TDSELECTCOMPARISON.Width = Unit.Percentage(100D)
                  TDSELECTCOMPARISON = DisplayFunctions.Fill_Dropdown(q("cef_field_type").ToString, TDSELECTCOMPARISON, q("cef_values").ToString)

                  'This piece of code is going to check and see if the values are set for this form item.
                  'If they are, it's going to set an attribute.
                  'This is going to reference a javascript function
                  'in common_functions.js.
                  'This function is going to set an onchange event on a select dropdown.
                  'It will then watch and if the select box comparison is changed to empty, 
                  'It will go ahead and clear the associated textbox value.
                  'You can pass the type of input on the third parameter of the javascript function.
                  'In this case it is textarea because we're setting up multiline (text area) boxes.
                  If String.IsNullOrEmpty(q("cef_values").ToString) Then

                    TDSELECTCOMPARISON.Attributes.Add("onChange", "javascript:ClearAssociatedBox($(this).find(':selected').val(),'" + AdvancedQueryResults.EscapeSpecialCharactersInSearchIDs(q.Item("cef_evo_field_name").ToString).Trim + "', 'textarea');")

                  End If


                  'This check will see whether or not the dropdown only has equals. If it does, we're going to go ahead
                  'And add a label that will basically say equals. The way we tell if the dropdown only has equals
                  'is because the function up above sets the css class to display_none if it is equals.
                  If TDSELECTCOMPARISON.CssClass = "display_none" Then
                    LB.Text = TDSELECTCOMPARISON.SelectedItem.Text
                    LB.CssClass = "lighter_gray_text"
                    TD_2.Controls.Add(LB)
                  ElseIf TDSELECTCOMPARISON.CssClass = "display_none includes" Then
                    LB.Text = "Enter Search Term"
                    LB.CssClass = "lighter_gray_text"
                    TD_2.Controls.Add(LB)
                  End If

                  'Fill in the session saved value.
                  DisplayFunctions.SelectInformation(TDSELECTCOMPARISON, Session.Item("Advanced-" & TDSELECTCOMPARISON.ID))


                  TD_2.Controls.Add(TDSELECTCOMPARISON)
                  TR.Controls.Add(TD_2)

                  If Not IsDBNull(q("cef_values")) Then
                    If q("cef_values").ToString <> "" Then
                      Dim TempHold As Array = Nothing
                      TDSELECT.ID = q("cef_evo_field_name").ToString 'q("cef_field_type").ToString & "-" & q("cef_id") & "-" & q("cef_evo_field_name").ToString
                      TDSELECT.ValidationGroup = q("cef_field_type").ToString
                      TempHold = Split(q("cef_values"), ",")
                      TDSELECT.Items.Add(New ListItem("", ""))
                      'This tooltip is set as the display on purpose.
                      'When we are looping through them, instead of adding yet another pipe in the ID
                      'Using the tooltip is an easy way to get the Textual Display of this Field.
                      TDSELECT.ToolTip = q("cef_display").ToString


                      For x = 0 To UBound(TempHold)
                        TDSELECT.Items.Add(New ListItem(Replace(Trim(TempHold(x)), "&#44;", ","), IIf(UCase(q("cef_field_type").ToString) = "CHAR", Left(Trim(TempHold(x)), 1), Trim(TempHold(x)))))
                      Next


                      TDSELECT.Width = Unit.Percentage(100D)

                      'Fill in the session saved value.
                      DisplayFunctions.SelectInformation(TDSELECT, Session.Item("Advanced-" & TDSELECT.ID))

                      'If just on the case of the project search
                      'Well, really only if we're doing a summary call back
                      'if the session is set, but there's a request variable
                      'we override it.
                      'On a summary call back - we go ahead and don't clear the session
                      'regular projects we do.
                      If Page.Request.Form("project_search") = "Y" Then
                        Dim temp As String = Request.Form(TDSELECT.ID)
                        If temp <> "" Then
                          TDSELECT.SelectedValue = Request.Form(TDSELECT.ID)
                        End If
                      End If


                      TD_3.Controls.Add(TDSELECT)

                      TD_4.Text = DisplayFunctions.DisplayFormatRules("Dropdown")
                    End If
                  End If


                  If TDSELECT.ID = "" Then 'use a textbox

                    TDTEXT.ID = AdvancedQueryResults.EscapeSpecialCharactersInSearchIDs(q.Item("cef_evo_field_name").ToString).Trim

                    TDTEXT.Width = Unit.Percentage(99D)
                    TDTEXT.ValidationGroup = q("cef_field_type").ToString


                    'This tooltip is set as the display on purpose.
                    'When we are looping through them, instead of adding yet another pipe in the ID
                    'Using the tooltip is an easy way to get the Textual Display of this Field.
                    TDTEXT.ToolTip = q("cef_display").ToString
                    TDTEXT.TextMode = TextBoxMode.MultiLine
                    TDTEXT.Height = Unit.Pixel(12)
                    TDTEXT.Rows = 1

                    'Fill in the session saved value.
                    If Not IsNothing(Session.Item("Advanced-" & TDTEXT.ID)) Then
                      If Not String.IsNullOrEmpty(Session.Item("Advanced-" & TDTEXT.ID)) Then
                        TDTEXT.Text = Session.Item("Advanced-" & TDTEXT.ID)
                      End If
                    End If
                    'Fill in the session saved value.
                    DisplayFunctions.SelectInformation(TDTEXT, Session.Item("Advanced-" & TDTEXT.ID))


                    'If just on the case of the project search
                    'Well, really only if we're doing a summary call back
                    'if the session is set, but there's a request variable
                    'we override it.
                    'On a summary call back - we go ahead and don't clear the session
                    'regular projects we do.
                    If Page.Request.Form("project_search") = "Y" Then
                      Dim temp As String = Request.Form(TDTEXT.ID)
                      If temp <> "" Then
                        TDTEXT.Text = Request.Form(TDTEXT.ID)
                      End If
                    End If

                    TD_3.Controls.Add(TDTEXT)

                    'This validation is only going in for numeric fields for right now.
                    'This is a work in progress and a test, I don't want to add them all at once,
                    'but would rather work with one at a time.
                    If q("cef_field_type").ToString = "Numeric" Or q("cef_field_type").ToString = "Year" Then
                      If q("cef_display").ToString <> "Days on Market" Then
                        NumberCustom.ID = "VALIDATE_" & q("cef_id").ToString 'Replace(Replace(Replace(Replace(q("cef_display").ToString, " ", ""), "#", "Num"), ")", ""), "(", "")
                        NumberCustom.ErrorMessage = "*Incorrect Format"
                        'NumberCustom.Font.Size = Unit.
                        NumberCustom.Font.Bold = True
                        NumberCustom.ValidationGroup = "Numeric"
                        NumberCustom.ControlToValidate = TDTEXT.ID
                        NumberCustom.SetFocusOnError = True
                        NumberCustom.ValidationExpression = "^[\d,:\s\n]+$"
                        NumberCustom.Text = "*Incorrect Format"
                        NumberCustom.Display = ValidatorDisplay.Static
                        NumberCustom.Enabled = True
                        TD_5.Controls.Add(NumberCustom)
                      End If
                    ElseIf q("cef_field_type").ToString = "Date" Then
                      CustomValidation.ID = "VALIDATE_" & q("cef_id").ToString '& Replace(Replace(Replace(Replace(q("cef_display").ToString, " ", ""), "#", "Num"), ")", ""), "(", "")
                      CustomValidation.ErrorMessage = "*Incorrect Format"
                      'NumberCustom.Font.Size = Unit.
                      CustomValidation.Font.Bold = True
                      CustomValidation.ValidationGroup = "Numeric"
                      CustomValidation.ControlToValidate = TDTEXT.ID
                      CustomValidation.SetFocusOnError = True
                      CustomValidation.ClientValidationFunction = "validateDate"
                      CustomValidation.Text = "*Incorrect Format"
                      CustomValidation.Display = ValidatorDisplay.Static
                      CustomValidation.Enabled = True
                      TD_5.Controls.Add(CustomValidation)
                    End If

                    If LB.Text <> "Enter Search Term" Then
                      TD_4.Text = DisplayFunctions.DisplayFormatRules(q("cef_field_type").ToString)
                    End If

                  End If

                  TR.Controls.Add(TD_3)
                  TD_4.CssClass = "lighter_gray_text"
                  TD_4.ToolTip = q("cef_definition").ToString
                  TR.Controls.Add(TD_4)

                  TR.Controls.Add(TD_5) 'Validator
                  Tab.Controls.Add(TR)
                  SubCounter += 1
                  ' End If
                Next




              End If

            End If

            If UCase(r("cefstab_sub_name").ToString) = "GENERAL" Then
              general_tab.Controls.Add(Tab)
            ElseIf UCase(r("cefstab_sub_name").ToString) = "HULL/DIMENSIONS" Then
              hull_tab.Controls.Add(Tab)
            ElseIf UCase(r("cefstab_sub_name").ToString) = "LOCATION" Then
              location_tab.Controls.Add(Tab)
            ElseIf UCase(r("cefstab_sub_name").ToString) = "MAINTENANCE" Then
              maintenance_dynamic_panel.Controls.Add(Tab)
            ElseIf UCase(r("cefstab_sub_name").ToString) = "POWER" Then
              power_dynamic_panel.Controls.Add(Tab)
            Else
              TempPanel.Controls.Add(Tab)
              yacht_advanced_search.Controls.AddAt(Counter, TempPanel)
            End If



            Tab.Dispose()
            TempPanel.Dispose()


            Counter += 1

          End If
        Next

        AdvancedQueryResults.DealWithAttributeTab(MainContent.ClientID, AttributesPanel, AttrTab, yacht_advanced_search, Page, Master.aclsData_Temp, yacht_attention)

      End If
    End If

  End Sub

  Private Sub reset_form_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles reset_form.Click
    'This goes through and finds all the advanced search items and clears it.
    Dim I As Integer = 0
    Dim L As Integer = Session.Contents.Count
    Dim keyName As String

    For I = L - 1 To 0 Step -1
      If TypeOf (Session.Contents.Item(I)) Is String Then
        If InStr(Session.Contents.Keys(I).ToString(), "Advanced-") > 0 Then

          keyName = Session.Contents.Keys(I).ToString()
          Session.Remove(keyName)
        End If
      End If
    Next
  End Sub

  Private Sub for_charter_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles for_charter.CheckedChanged
    If for_charter.Checked Then
      charter_tab.Visible = True
      yacht_advanced_search.ActiveTab = charter_tab
    Else
      charter_tab.Visible = False
      yacht_advanced_search.ActiveTab = company_contact_tab
    End If

  End Sub

  Public Shared Sub Populate_Dual_Stages_Listbox(ByVal tempTable As DataTable, ByVal lb As ListBox, ByVal fieldtext As String, ByVal SecondFieldText As String, ByVal fieldvalue As String, ByVal secondFieldValue As String, ByVal Seperator As String, ByVal quotes As Boolean)
    lb.Items.Clear()
    lb.Items.Add(New ListItem("All", ""))
    If Not IsNothing(tempTable) Then
      If tempTable.Rows.Count > 0 Then
        For Each r As DataRow In tempTable.Rows
          If Not IsDBNull(r(fieldtext)) And Trim(r(fieldvalue)) <> "" Then
            If Not IsDBNull(r(SecondFieldText)) And Trim(r(secondFieldValue)) <> "" Then
              lb.Items.Add(New ListItem(CStr(Replace(r(fieldtext), "Manufacturer", "MFR")) & Seperator & CStr(Replace(r(SecondFieldText), "Manufacturer", "MFR")), IIf(quotes = True, "'" & CStr(r(fieldvalue)) & "|" & CStr(r(secondFieldValue)) & "'", CStr(r(fieldvalue)) & "|" & CStr(r(secondFieldValue)))))
            End If
          End If
        Next
      End If
    End If
    lb.SelectedValue = ""
  End Sub

  Private Sub yt_engine_manufacturer_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles yt_engine_manufacturer.SelectedIndexChanged
    Dim CompanyIDString As String = ""
    CompanyIDString = clsGeneral.clsGeneral.ExtractSelectedStringFromListboxDropdown(yt_engine_manufacturer, False, 0, True)

    Dim resultsTable As New DataTable
    resultsTable = YachtFunctions.GetEngineModelFromManufacturer(CompanyIDString)

    clsGeneral.clsGeneral.Populate_Listbox(resultsTable, yt_engine_model, "yem_engine_model", "yem_engine_model_id", False)
  End Sub

  Private Function GetYachtTypeSizeBrandModelFromCommonControl(ByVal TypeofControl As String, ByVal sYachtCategoryModelCtrlBaseName As String, ByRef YachtType As String, ByRef YachtSize As String, ByRef YachtBrand As String, ByRef YachtModel As String, ByRef BuildSearchString As String)

    Dim sType As String = ""
    Dim sSize As String = ""
    Dim sBrand As String = ""
    Dim sModel As String = ""

    '----------------------------------------------------------------------------------------------------
    '---------------------------------------Yacht Type-----------------------------------------------
    '----------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------
    Dim VariableYachtType As String = ""
    Dim HoldString As String = ""

    If HttpContext.Current.Request.Form("complete_search") = "Y" Or HttpContext.Current.Request.Form("project_search") = "Y" Then
      VariableYachtType = HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Type")
    Else
      HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Type") = ""
      If Not IsNothing(HttpContext.Current.Request.Item("cbo" + sYachtCategoryModelCtrlBaseName + "Type")) Then
        VariableYachtType = HttpContext.Current.Request.Item("cbo" + sYachtCategoryModelCtrlBaseName + "Type")
      End If
    End If
    '------------------------------------------------------------------------------------------------
    'Splitting The Type to set up the string for the where clause and the search text display.
    If Not IsNothing(VariableYachtType) Then
      If Not String.IsNullOrEmpty(VariableYachtType.Trim) Then
        If Not VariableYachtType.ToString.ToLower.Contains("all") Then

          HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Type") = VariableYachtType

          Dim TypeArray As Array = Split(HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Type"), ",")

          For MultipleTypeCount = 0 To UBound(TypeArray)
            Dim TempTypeHold As String = CStr(TypeArray(MultipleTypeCount))
            Dim ModelIDHold As Long = 0
            ModelIDHold = commonEvo.ReturnYachtModelIDForItemIndex(CLng(TempTypeHold))

            If commonEvo.ReturnYachtModelDataFromIndex(CLng(TempTypeHold), sType, sSize, sBrand, sModel) Then
              If Not String.IsNullOrEmpty(HoldString.Trim) Then
                HoldString += ", "
              End If
              HoldString += "'" & sType & "'"
            End If

            sType = ""
            sSize = ""
            sBrand = ""
            sModel = ""

          Next

          YachtType = HoldString

          BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(Replace(HoldString, ",", ", "), sYachtCategoryModelCtrlBaseName & " Motor(s)")

        Else
          HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Type") = ""
        End If
      Else
        HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Type") = ""
      End If
    End If

    '----------------------------------------------------------------------------------------------------
    '---------------------------------------Yacht Size-----------------------------------------------
    '----------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------
    Dim VariableYachtSize As String = ""
    HoldString = ""

    If HttpContext.Current.Request.Form("complete_search") = "Y" Or HttpContext.Current.Request.Form("project_search") = "Y" Then
      VariableYachtSize = HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Size")
    Else
      HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Size") = ""
      If Not IsNothing(HttpContext.Current.Request.Item("cbo" + sYachtCategoryModelCtrlBaseName + "Size")) Then
        VariableYachtSize = HttpContext.Current.Request.Item("cbo" + sYachtCategoryModelCtrlBaseName + "Size")
      End If
    End If
    '------------------------------------------------------------------------------------------------
    'Splitting The Category to set up the string for the where clause and the search text display.
    If Not IsNothing(VariableYachtSize) Then
      If Not String.IsNullOrEmpty(VariableYachtSize.Trim) Then
        If Not VariableYachtSize.ToString.ToLower.Contains("all") Then

          HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Size") = VariableYachtSize

          Dim SizeArray As Array = Split(HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Size"), ",")

          For MultipleSizeCount = 0 To UBound(SizeArray)
            Dim TempCategoryHold As String = CStr(SizeArray(MultipleSizeCount))
            Dim ModelIDHold As Long = 0
            ModelIDHold = commonEvo.ReturnYachtModelIDForItemIndex(CLng(TempCategoryHold))
            If commonEvo.ReturnYachtModelDataFromIndex(CLng(TempCategoryHold), sType, sSize, sBrand, sModel) Then
              If Not String.IsNullOrEmpty(HoldString.Trim) Then
                HoldString += ", "
              End If
              HoldString += "'" & sSize & "'"
            End If

            sType = ""
            sSize = ""
            sBrand = ""
            sModel = ""

          Next

          YachtSize = HoldString

          BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(Replace(YachtSize, "'", ""), sYachtCategoryModelCtrlBaseName & " Size(s)")

        Else
          HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Size") = ""
        End If
      Else
        HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Size") = ""
      End If
    End If

    '----------------------------------------------------------------------------------------------------
    '-----------------------------------------Yacht Brand------------------------------------------------
    '----------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------
    Dim VariableYachtBrand As String = ""
    HoldString = ""
    If HttpContext.Current.Request.Form("complete_search") = "Y" Or HttpContext.Current.Request.Form("project_search") = "Y" Then
      VariableYachtBrand = HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Brand")
    Else
      HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Brand") = ""
      If Not IsNothing(HttpContext.Current.Request.Item("cbo" + sYachtCategoryModelCtrlBaseName + "Brand")) Then
        VariableYachtBrand = HttpContext.Current.Request.Item("cbo" + sYachtCategoryModelCtrlBaseName + "Brand")
      End If
    End If

    '------------------------------------------------------------------------------------------------
    'Splitting The Brand to set up the string for the where clause and the search text display.
    If Not IsNothing(VariableYachtBrand) Then

      If Not String.IsNullOrEmpty(VariableYachtBrand.Trim) Then
        If Not VariableYachtBrand.ToString.ToLower.Contains("all") Then

          HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Brand") = VariableYachtBrand

          Dim BrandArray As Array = Split(HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Brand"), ",")

          For MultipleBrandCount = 0 To UBound(BrandArray)
            Dim TempBrandHold As String = CStr(BrandArray(MultipleBrandCount))
            Dim ModelIDHold As Long = 0
            ModelIDHold = commonEvo.ReturnYachtModelIDForItemIndex(CLng(TempBrandHold))
            If commonEvo.ReturnYachtModelDataFromIndex(CLng(TempBrandHold), sType, sSize, sBrand, sModel) Then
              If Not String.IsNullOrEmpty(HoldString.Trim) Then
                HoldString += ", "
              End If
              HoldString += "'" & sBrand & "'"
            End If

            sType = ""
            sSize = ""
            sBrand = ""
            sModel = ""

          Next

          YachtBrand = HoldString

          BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(Replace(YachtBrand, "'", ""), sYachtCategoryModelCtrlBaseName & " Brand(s)")
        Else
          HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Brand") = ""
        End If
      Else
        HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Brand") = ""
      End If
    End If
    '----------------------------------------------------------------------------------------------------
    '-----------------------------------------Yacht Model------------------------------------------------
    '----------------------------------------------------------------------------------------------------
    '----------------------------------------------------------------------------------------------------
    Dim VariableYachtModel As String = ""
    HoldString = ""

    If HttpContext.Current.Request.Form("complete_search") = "Y" Or HttpContext.Current.Request.Form("project_search") = "Y" Then
      VariableYachtModel = HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Model")
    Else
      HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Model") = ""
      If Not IsNothing(HttpContext.Current.Request.Item("cbo" + sYachtCategoryModelCtrlBaseName + "Model")) Then
        VariableYachtModel = HttpContext.Current.Request.Item("cbo" + sYachtCategoryModelCtrlBaseName + "Model")
      End If
    End If
    '------------------------------------------------------------------------------------------------
    'Splitting The Model to set up the string for the where clause and the search text display.
    If Not IsNothing(VariableYachtModel) Then
      If Not String.IsNullOrEmpty(VariableYachtModel.Trim) Then
        If Not VariableYachtModel.ToString.ToLower.Contains("all") Then

          HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Model") = VariableYachtModel

          Dim ModelArray As Array = Split(HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Model"), ",")

          For MultipleModelCount = 0 To UBound(ModelArray)

            Dim TempModelHold As String = CStr(ModelArray(MultipleModelCount))
            Dim ModelIDHold As Long = 0
            ModelIDHold = commonEvo.ReturnYachtModelIDForItemIndex(CLng(TempModelHold))
            If commonEvo.ReturnYachtModelDataFromIndex(CLng(TempModelHold), sType, sSize, sBrand, sModel) Then
              If Not String.IsNullOrEmpty(HoldString.Trim) Then
                HoldString += ", "
              End If
              HoldString += "'" & sModel & "'"
            End If

          Next

          YachtModel = HoldString

          BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(Replace(HoldString, "'", ""), sYachtCategoryModelCtrlBaseName & " Model(s)")

        Else
          HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Model") = ""
        End If
      Else
        HttpContext.Current.Session.Item(TypeofControl & sYachtCategoryModelCtrlBaseName & "Model") = ""
      End If
    End If


    Return BuildSearchString
  End Function

  Private Sub FillUpSessionForYachtTypeSizeBrandModel(ByVal PageOrigin As String, ByVal sTypeYachtBaseName As String)

    ' because these values are needed on this page they need to match the control names in the control
    ' so the request header picks up the right values
    If Not IsNothing(HttpContext.Current.Request.Item("cbo" + sTypeYachtBaseName + "Type")) Then
      If Not String.IsNullOrEmpty(HttpContext.Current.Request.Item("cbo" + sTypeYachtBaseName + "Type").ToString) Then
        If Not HttpContext.Current.Request.Item("cbo" + sTypeYachtBaseName + "Type").ToString.ToLower.Contains("all") Then
          HttpContext.Current.Session.Item(PageOrigin + sTypeYachtBaseName + "Type") = HttpContext.Current.Request.Item("cbo" + sTypeYachtBaseName + "Type").ToString.Trim
        Else
          HttpContext.Current.Session.Item(PageOrigin + sTypeYachtBaseName + "Type") = ""
          HttpContext.Current.Session.Item(PageOrigin + sTypeYachtBaseName + "Size") = ""
          HttpContext.Current.Session.Item(PageOrigin + sTypeYachtBaseName + "Brand") = ""
          HttpContext.Current.Session.Item(PageOrigin + sTypeYachtBaseName + "Model") = ""
        End If
      End If
    End If

    If Not IsNothing(HttpContext.Current.Request.Item("cbo" + sTypeYachtBaseName + "Size")) Then
      If Not String.IsNullOrEmpty(HttpContext.Current.Request.Item("cbo" + sTypeYachtBaseName + "Size").ToString) Then
        If Not HttpContext.Current.Request.Item("cbo" + sTypeYachtBaseName + "Size").ToString.ToLower.Contains("all") Then
          HttpContext.Current.Session.Item(PageOrigin + sTypeYachtBaseName + "Size") = HttpContext.Current.Request.Item("cbo" + sTypeYachtBaseName + "Size").ToString.Trim
        Else
          HttpContext.Current.Session.Item(PageOrigin + sTypeYachtBaseName + "Size") = ""
          HttpContext.Current.Session.Item(PageOrigin + sTypeYachtBaseName + "Brand") = ""
          HttpContext.Current.Session.Item(PageOrigin + sTypeYachtBaseName + "Model") = ""
        End If
      End If
    End If

    If Not IsNothing(HttpContext.Current.Request.Item("cbo" + sTypeYachtBaseName + "Brand")) Then
      If Not String.IsNullOrEmpty(HttpContext.Current.Request.Item("cbo" + sTypeYachtBaseName + "Brand").ToString) Then
        If Not HttpContext.Current.Request.Item("cbo" + sTypeYachtBaseName + "Brand").ToString.ToLower.Contains("all") Then
          HttpContext.Current.Session.Item(PageOrigin + sTypeYachtBaseName + "Brand") = HttpContext.Current.Request.Item("cbo" + sTypeYachtBaseName + "Brand").ToString.Trim
        Else
          HttpContext.Current.Session.Item(PageOrigin + sTypeYachtBaseName + "Brand") = ""
          HttpContext.Current.Session.Item(PageOrigin + sTypeYachtBaseName + "Model") = ""
        End If
      End If
    End If

    If Not IsNothing(HttpContext.Current.Request.Item("cbo" + sTypeYachtBaseName + "Model")) Then
      If Not String.IsNullOrEmpty(HttpContext.Current.Request.Item("cbo" + sTypeYachtBaseName + "Model").ToString) Then
        If Not HttpContext.Current.Request.Item("cbo" + sTypeYachtBaseName + "Model").ToString.ToLower.Contains("all") Then
          HttpContext.Current.Session.Item(PageOrigin + sTypeYachtBaseName + "Model") = HttpContext.Current.Request.Item("cbo" + sTypeYachtBaseName + "Model").ToString.Trim
        Else
          HttpContext.Current.Session.Item(PageOrigin + sTypeYachtBaseName + "Model") = ""
        End If
      End If
    End If

  End Sub

  Public Function EvolutionYachtListingPageQuery(ByVal Forsale_Flag As String, ByVal ForLease_Flag As String, _
                                               ByVal ForCharter_Flag As String, ByVal MfrStart As String, _
                                               ByVal MfrEnd As String, ByVal CallSign As String, _
                                               ByVal LifecycleStage As String, ByVal brandString As String, _
                                               ByVal yearString As String, ByVal CategorySize As String, _
                                               ByVal MotorSize As String, ByVal PageSort As String, _
                                               ByVal yacht_name_search As String, ByVal yachtIDs As String, _
                                               ByVal yachtModels As String, ByVal Ownership As String, _
                                               ByVal YachtStatus As String, ByVal RegisteredCountryFlag As String, _
                                               ByVal yearDlv As String, ByVal yachtClass As String, _
                                               ByVal PreviousName As Boolean, _
                                               ByVal CompanyCountriesString As String, ByVal CompanyTimeZoneString As String, _
                                               ByVal CompanyContinentString As String, ByVal CompanyRegionString As String, _
                                               ByVal CompanyStateName As String, ByVal DynamicQueryStringGenerated As String, _
                                               ByVal helipad_check As String, ByVal BrandMFR_String As String, _
                                               ByVal History As Boolean, ByVal JournalDate As String, ByVal JournalTransType As String, _
                                               ByVal useAltHullMFR As Boolean, ByVal US_Waters_Flag As String, _
                                               ByVal yachtAskingPrice As String, ByVal yachtAskingPriceCurrency As String, ByVal yachtAskingPriceOperator As String) As DataTable

    '
    Dim Query As String = ""
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable
    Dim where_string As String = ""
    Dim QueryFrom As String = ""
    Dim count As Integer = 1
    Dim temp_refit_string As String = ""
    Dim temp_refit_string_start As String = ""
    Dim temp_refit_string_mid As String = ""
    Dim temp_refit_string_end As String = ""
    Dim refit_start_year As String = ""
    Dim refitt_end_year As String = ""
    Dim temp_spot As Integer = 0
    Dim temp_count As Integer = 0
    Dim refit_select As String = "STUFF((SELECT ';'+yd_description FROM Yacht_Details WITH (NOLOCK) WHERE (yd_yt_id = yt_id) AND (yd_type = 'Maintenance') AND (yd_name = 'Refit') FOR XML PATH('')),1,1,'')  like"
    Dim string_years As String = ""
    Dim BrokenrefittYears As Array = Nothing
    Dim i As Integer = 0


    atemptable.Columns.Add("yt_id")
    atemptable.Columns.Add("yt_year_mfr")
    atemptable.Columns.Add("yt_yacht_name")
    atemptable.Columns.Add("yt_hull_mfr_nbr")
    atemptable.Columns.Add("ym_brand_name")
    atemptable.Columns.Add("ym_model_name")
    atemptable.Columns.Add("ym_category_size")
    atemptable.Columns.Add("ym_submotor_type")
    atemptable.Columns.Add("ytpic_id")
    atemptable.Columns.Add("ycs_description")
    atemptable.Columns.Add("yt_forsale_status")
    atemptable.Columns.Add("yt_length_overall_meters")
    'atemptable.Columns.Add("comp_name")
    'atemptable.Columns.Add("comp_id")
    atemptable.Columns.Add("yt_forsale_list_date")
    atemptable.Columns.Add("yt_asking_price")
    atemptable.Columns.Add("yt_forsale_flag")
    atemptable.Columns.Add("ym_motor_type")
    atemptable.Columns.Add("yt_for_lease_flag")
    atemptable.Columns.Add("yt_for_charter_flag")
    atemptable.Columns.Add("yt_journ_id")
    atemptable.Columns.Add("yt_asking_price_wordage")
    atemptable.Columns.Add("yl_lifecycle_name")
    atemptable.Columns.Add("yls_lifecycle_status")

    Dim yt_count As DataColumn = New DataColumn("comp_count", Type.GetType("System.Int64"))
    yt_count.AutoIncrement = True
    yt_count.AutoIncrementSeed = 1
    atemptable.Columns.Add(yt_count)

    HttpContext.Current.Session.Item("MasterYacht") = "" 'Whole Search
    HttpContext.Current.Session.Item("MasterYachtSelect") = "" 'Select Only
    HttpContext.Current.Session.Item("MasterYachtFrom") = "" 'From Only
    HttpContext.Current.Session.Item("MasterYachtWhere") = "" 'Where Only
    HttpContext.Current.Session.Item("MasterYachtSort") = "" 'Sort Only

    Try

      Query = "SELECT DISTINCT "

      If History = True Then
        Query += "journ_date, journ_subcategory_code, journ_subject, "
      End If

      Query += "yl_lifecycle_name,yls_lifecycle_status, yt_id, yt_yacht_name, "
      Query += "yt_hull_mfr_nbr, ym_brand_name, ym_model_name, ym_category_size, "
      Query += "ym_submotor_type, ycs_description, yt_length_overall_meters, yt_year_mfr, "
      Query += "yt_forsale_status, yt_length_overall_meters, yt_forsale_list_date, "
      Query += "yt_for_charter_flag, yt_forsale_flag, ym_motor_type, yt_for_lease_flag, "
      Query += "yt_asking_price, yt_journ_id, yt_asking_price_wordage, "

      If History = True Then
        Query += "0 AS ytpic_id "
      Else
        Query += "(SELECT TOP 1 ytpic_id FROM yacht_pictures WHERE ytpic_yt_id = yt_id AND ytpic_journ_id = yt_journ_id AND ytpic_hide_flag = 'N' ORDER BY ytpic_seq_no asc) AS ytpic_id "
      End If

      HttpContext.Current.Session.Item("MasterYachtSelect") = Query

      QueryFrom = "FROM Yacht WITH(NOLOCK) "

      QueryFrom += "LEFT OUTER JOIN yacht_reference WITH(NOLOCK) ON yr_yt_id = yt_id AND yr_journ_id = yt_journ_id AND yr_contact_type <> '71' "
      QueryFrom += "LEFT OUTER JOIN company WITH(NOLOCK) ON comp_id = yr_comp_id AND comp_journ_id = yr_journ_id "
      QueryFrom += "LEFT OUTER JOIN [State] WITH(NOLOCK) ON state_code = comp_state AND state_country = comp_country "
      QueryFrom += "LEFT OUTER JOIN contact WITH(NOLOCK) ON contact_id = yr_contact_id AND contact_comp_id = yr_comp_id AND contact_journ_id = yr_journ_id "
      QueryFrom += "INNER JOIN yacht_model WITH(NOLOCK) ON ym_model_id = yt_model_id "
      QueryFrom += "INNER JOIN yacht_category_size WITH(NOLOCK) ON ycs_category_size = ym_category_size AND ycs_motor_type = ym_motor_type "

      If Not String.IsNullOrEmpty(CompanyContinentString.Trim) Then
        QueryFrom += "LEFT OUTER JOIN Country WITH(NOLOCK) ON comp_country = country_name "
      End If

      If PreviousName Then
        QueryFrom += "LEFT OUTER JOIN Yacht_Previous_Names WITH(NOLOCK) ON ypn_yt_id = yt_id "
      End If

      QueryFrom += "LEFT OUTER JOIN yacht_contact_type WITH(NOLOCK) ON yct_code = yr_contact_type "

      If History Then
        QueryFrom += "INNER JOIN Journal WITH(NOLOCK) ON  yt_journ_id = journ_id and yt_id = journ_yacht_id "
      Else
        QueryFrom += "LEFT OUTER JOIN yacht_pictures WITH(NOLOCK) ON ytpic_yt_id = yt_id and ytpic_journ_id = yt_journ_id  and ytpic_hide_flag = 'N' and ytpic_seq_no = 1 "
      End If

      QueryFrom += "LEFT OUTER JOIN Yacht_Lifecycle WITH(NOLOCK) ON yt_lifecycle_id = yl_lifecyle_id "
      QueryFrom += "LEFT OUTER JOIN Yacht_Lifecycle_Status WITH(NOLOCK) ON yt_lifecycle_id = yl_lifecyle_id AND yt_lifecycle_status = yls_lifecycle_status"

      HttpContext.Current.Session.Item("MasterYachtFrom") = QueryFrom

      Query += QueryFrom

      If History = False Then
        where_string = " (yt_journ_id = 0) "
      End If

      ' where_string = " (yr_contact_type = '00' or yr_contact_type is null) "

      'History Only Fields 
      If History Then
        If JournalDate <> "" Then
          If Not String.IsNullOrEmpty(where_string.Trim) Then
            where_string += " and "
          End If
          where_string += " journ_date " & JournalDate & ""
        End If
      End If

      If History Then
        If JournalTransType <> "" Then
          If Not String.IsNullOrEmpty(where_string.Trim) Then
            where_string += " and "
          End If
          where_string += " journ_subcat_code_part1 = '" & JournalTransType & "'"
        End If
      End If

      'For sale flag:
      If Trim(Forsale_Flag) <> "" Then
        If Not String.IsNullOrEmpty(where_string.Trim) Then
          where_string += " and "
        End If
        If Trim(Forsale_Flag) = "Y" Then
          where_string += "yt_forsale_flag = 'Y' "
          'Else
          '    where_string += "yt_forsale_flag = 'N' "
        End If
      End If

      'If Trim(yacht_searched_id) <> "" Then
      '  If IsNumeric(yacht_searched_id) Then
      '    If Not String.IsNullOrEmpty(where_string.Trim) Then
      '      where_string += " and "
      '    End If
      '    where_string += " yt_id = '" & Trim(yacht_searched_id) & "' "
      '  End If
      'End If

      'Registered Country Flag:
      If Trim(RegisteredCountryFlag) <> "" Then
        ' If Trim(RegisteredCountryFlag) = "Y" Then
        If Not String.IsNullOrEmpty(where_string.Trim) Then
          where_string += " and "
        End If
        where_string += " yt_registered_country_flag = '" & RegisteredCountryFlag & "' "
        'Else
        '    where_string += " yt_registered_country_flag = 'N' "
        'End If
      End If

      'Charter/For Sale Restrictions
      If Trim(US_Waters_Flag) <> "" Then
        If Not String.IsNullOrEmpty(where_string.Trim) Then
          where_string += " and "
        End If
        If Trim(US_Waters_Flag) = "Y" Then
          where_string += " yt_not_in_usa_water = 'Y' "
        ElseIf Trim(US_Waters_Flag) = "N" Then
          where_string += " yt_not_in_usa_water <> 'Y' "
        End If
      End If

      'For lease flag:
      If Trim(ForLease_Flag) <> "" Then
        If Not String.IsNullOrEmpty(where_string.Trim) Then
          where_string += " and "
        End If
        If Trim(ForLease_Flag) = "Y" Then
          where_string += "yt_for_lease_flag = 'Y' "
          'Else
          '    where_string += "yt_for_lease_flag = 'N' "
        End If
      End If
      'Status 
      If Trim(YachtStatus) <> "" Then
        Dim SwapOperator As String = " and "
        If Not String.IsNullOrEmpty(where_string.Trim) Then
          where_string += " and "
        End If

        where_string += " ( "
        If InStr(YachtStatus, "NOT FOR SALE") > 0 Then
          where_string += "yt_forsale_flag = 'N' "

          YachtStatus = Replace(YachtStatus, "'NOT FOR SALE'", "")
          YachtStatus = YachtStatus.TrimEnd(",")
          SwapOperator = " or "
          If YachtStatus <> "" Then
            where_string += " or "
          End If
        End If

        If YachtStatus <> "" Then
          If InStr(Trim(YachtStatus), "'AVAILABLE'") > 0 Then
            'added MSW - 8-8-16
            If Trim(YachtStatus) = "'AVAILABLE'" Then
              where_string += " (yt_forsale_flag = 'Y' or yt_for_lease_flag = 'Y' or yt_for_charter_flag = 'Y')  "
            Else
              where_string += " (yt_forsale_status in (" & YachtStatus & ") or (yt_forsale_flag = 'Y' or yt_for_lease_flag = 'Y' or yt_for_charter_flag = 'Y')) "
            End If

          ElseIf InStr(Trim(YachtStatus), "'NOT AVAILABLE'") > 0 Then
            where_string += " (yt_forsale_flag = 'N' and yt_for_lease_flag = 'N' and yt_for_charter_flag = 'N')  "
          ElseIf InStr(Trim(YachtStatus), "'NOT FOR CHARTER'") > 0 Then
            where_string += " (yt_for_charter_flag = 'N')  "
          Else
            where_string += " yt_forsale_status in (" & YachtStatus & ") "
          End If
          ' and 
        End If
        where_string += " ) "
      End If
      'Charter flag:
      If Trim(ForCharter_Flag) <> "" Then
        If Not String.IsNullOrEmpty(where_string.Trim) Then
          where_string += " and "
        End If
        If Trim(ForCharter_Flag) = "Y" Then
          where_string += " yt_for_charter_flag = 'Y' "
          'Else
          '    where_string += " yt_for_charter_flag = 'N' "
        End If
      End If

      'lifecycle stage
      If LifecycleStage <> "" Then
        If Not String.IsNullOrEmpty(where_string.Trim) Then
          where_string += " and "
        End If
        where_string += " (" & LifecycleStage & ")"
      End If

      'Ownership 
      If Ownership <> "" Then
        If Not String.IsNullOrEmpty(where_string.Trim) Then
          where_string += " and "
        End If
        where_string += " yt_ownership_type in (" & Ownership & ")"
      End If
      'yacht model ID
      If yachtIDs <> "" Then
        If Not String.IsNullOrEmpty(where_string.Trim) Then
          where_string += " and "
        End If
        where_string += " yt_id in (" & yachtIDs & ") "
      End If

      'If Trim(CentralAgent) <> "" Then
      '    If Trim(CentralAgent) = "Y" Then
      '        where_string = where_string & "AND yt_central_agent_flag = 'Y' "
      '    Else
      '        where_string = where_string & "AND yt_central_agent_flag = 'N' "
      '    End If
      'End If

      ' for the search on the general tab
      If Trim(BrandMFR_String) <> "" Then
        If Not String.IsNullOrEmpty(where_string.Trim) Then
          where_string += " and "
        End If
        where_string += BrandMFR_String
      End If

      'yacht brands
      If Trim(brandString) <> "" Then
        If Not String.IsNullOrEmpty(where_string.Trim) Then
          where_string += " AND "
        End If
        where_string += " ym_brand_name IN (" + brandString + ") "
      End If

      'yacht models
      If Trim(yachtModels) <> "" Then
        If Not String.IsNullOrEmpty(where_string.Trim) Then
          where_string += " AND "
        End If
        where_string += " ym_model_name IN (" + yachtModels + ") "
      End If

      'yacht motor 
      If Trim(MotorSize) <> "" Then
        If Not String.IsNullOrEmpty(where_string.Trim) Then
          where_string += " AND "
        End If
        where_string += " ym_motor_type IN (" + MotorSize + ") "
      End If

      'yacht category size
      If Trim(CategorySize) <> "" Then
        If Not String.IsNullOrEmpty(where_string.Trim) Then
          where_string += " AND "
        End If
        where_string += " ym_category_size IN (" + CategorySize + ") "
      End If

      'yacht year
      If Trim(yearString) <> "" Then
        If Not String.IsNullOrEmpty(where_string.Trim) Then
          where_string += " and "
        End If
        where_string += " yt_year_mfr = '" & yearString & "' "
      End If
      'yacht year dlv
      If Trim(yearDlv) <> "" Then
        If Not String.IsNullOrEmpty(where_string.Trim) Then
          where_string += " and "
        End If
        where_string += " yt_year_dlv = '" & yearDlv & "' "
      End If


      'yacht mfr start
      If Trim(MfrStart) <> "" Then
        If Not String.IsNullOrEmpty(where_string.Trim) Then
          where_string += " and "
        End If
        where_string += " ( yt_hull_mfr_nbr like '" & MfrStart & "%' "

        If useAltHullMFR Then
          where_string += " or yt_alt_hull_mfr_nbr  like '" & MfrStart & "%' ) "
        Else
          where_string += " ) "
        End If
      End If
      'mfr end
      If Trim(MfrEnd) <> "" Then
        If Not String.IsNullOrEmpty(where_string.Trim) Then
          where_string += " and "
        End If
        where_string += " ( yt_hull_mfr_nbr like '%" & MfrEnd & "' "

        If useAltHullMFR Then
          where_string += " or yt_alt_hull_mfr_nbr like '%" & MfrEnd & "' ) "
        Else
          where_string += " ) "
        End If
      End If


      'call sign
      If Trim(CallSign) <> "" Then
        If Not String.IsNullOrEmpty(where_string.Trim) Then
          where_string += " and "
        End If
        where_string += " yt_radio_call_sign = '" & CallSign & "' "
      End If


      'name
      If Trim(yacht_name_search) <> "" Then
        If Not String.IsNullOrEmpty(where_string.Trim) Then
          where_string += " and "
        End If
        where_string += " ( (yt_yacht_name like '" & yacht_name_search & "%' or yt_yacht_name_search like '" & yacht_name_search & "%')  "
        'previous name
        If PreviousName Then
          If Not String.IsNullOrEmpty(where_string.Trim) Then
            where_string += " or "
          End If
          where_string += " ypn_previous_name like '" & yacht_name_search & "%' "
        End If
        where_string += " )"
      End If

      If Trim(yachtClass) <> "" Then
        If Not String.IsNullOrEmpty(where_string.Trim) Then
          where_string += " and "
        End If
        where_string += "  yt_class_id ='" & yachtClass & "'"
      End If


      If CompanyTimeZoneString <> "" Then
        If Not String.IsNullOrEmpty(where_string.Trim) Then
          where_string += " and "
        End If

        where_string += " comp_timezone in (SELECT tzone_name FROM Timezone where tzone_id in (" & CompanyTimeZoneString & ")) "
      End If


      'Continent
      If CompanyContinentString <> "" Then
        If Not String.IsNullOrEmpty(where_string.Trim) Then
          where_string += " AND"
        End If
        where_string += " country_continent_name in (" & CompanyContinentString & ") "
      End If

      ' check the state
      If CompanyStateName <> "" Then
        If Not String.IsNullOrEmpty(where_string.Trim) Then
          where_string += " AND "
        End If
        where_string += " state_name IN (" & CompanyStateName & ")"
      End If


      ''----------- LENGTH TO AND FROM SEARCH------------------------

      'If length_from.ToString.Trim <> "" And length_from.ToString.Trim <> "0" Then
      '  If Not String.IsNullOrEmpty(where_string.Trim) Then
      '    where_string += " AND "
      '  End If
      '  where_string += " yt_length_overall_meters >= " & length_from.ToString.Trim & " "
      'End If

      'If length_to.ToString.Trim <> "" And length_to.ToString.Trim <> "0" Then
      '  If Not String.IsNullOrEmpty(where_string.Trim) Then
      '    where_string += " AND "
      '  End If
      '  where_string += " yt_length_overall_meters <= " & length_to.ToString.Trim & " "
      'End If
      '----------- LENGTH TO AND FROM SEARCH------------------------

      If Not String.IsNullOrEmpty(yachtAskingPrice.Trim) Then

        Dim sTmpStr As String = ""
        Dim exchangeRate As Double = 0.0
        Dim sCurrencyName As String = ""

        If Not yachtAskingPriceCurrency.Contains("9") Then

          ' if not us dollar get the "conversion" traslation
          exchangeRate = commonEvo.GetForeignExchangeRate(CInt(yachtAskingPriceCurrency), sCurrencyName, "")

          If yachtAskingPrice.Contains(":") Then
            Dim tmpPriceArray() As String = Split(yachtAskingPrice, ":")
            yachtAskingPrice = (CDbl(tmpPriceArray(0)) * exchangeRate).ToString + ":" + (CDbl(tmpPriceArray(1)) * exchangeRate).ToString
          Else
            yachtAskingPrice = (CDbl(yachtAskingPrice) * exchangeRate).ToString
          End If


        End If

        If yachtAskingPrice.Contains(":") Or yachtAskingPriceOperator.ToLower.Contains("between") Then

          Dim tmpPriceArray() As String = Split(yachtAskingPrice, ":")
          ' split the asking price and generate "between" clause
          If tmpPriceArray.Length > 1 Then
            sTmpStr = IIf(Not String.IsNullOrEmpty(where_string.Trim), " AND ", "") + IIf(exchangeRate = 0, "yt_asking_price BETWEEN ", "(yt_asking_price) BETWEEN ") + tmpPriceArray(0).Trim + " AND " + tmpPriceArray(1).Trim + " "
          Else
            sTmpStr = IIf(Not String.IsNullOrEmpty(where_string.Trim), " AND ", "") + IIf(exchangeRate = 0, "yt_asking_price ", "(yt_asking_price) ") + Constants.cEq + tmpPriceArray(0).Trim + " "
          End If

        Else

          Select Case (yachtAskingPriceOperator.ToLower)

            Case "equals"
              sTmpStr = IIf(Not String.IsNullOrEmpty(where_string.Trim), " AND ", "") + IIf(exchangeRate = 0, "yt_asking_price ", "(yt_asking_price) ") + Constants.cEq + yachtAskingPrice.Trim + " "
            Case "greater than"
              sTmpStr = IIf(Not String.IsNullOrEmpty(where_string.Trim), " AND ", "") + IIf(exchangeRate = 0, "yt_asking_price ", "(yt_asking_price) ") + Constants.cGt + yachtAskingPrice.Trim + " "
            Case "less than"
              sTmpStr = IIf(Not String.IsNullOrEmpty(where_string.Trim), " AND ", "") + IIf(exchangeRate = 0, "yt_asking_price ", "(yt_asking_price) ") + Constants.cLt + yachtAskingPrice.Trim + " "

          End Select

        End If

        where_string += sTmpStr

      End If

      If Not String.IsNullOrEmpty(helipad_check.Trim) Then
        If Not String.IsNullOrEmpty(where_string.Trim) Then
          where_string += " AND "
        End If
        where_string += "yt_helipad = 'Y' "
      End If


      ' check the country
      If Not String.IsNullOrEmpty(CompanyCountriesString.Trim) Then
        If Not String.IsNullOrEmpty(where_string.Trim) Then
          where_string += " AND "
        End If

        where_string += "comp_country in (" + CompanyCountriesString.Trim + ") "
      End If


      'If Trim(MFR_String) <> "" Then
      '  If Not String.IsNullOrEmpty(where_string.Trim) Then
      '    where_string += " AND "
      '  End If

      '  where_string += " ym_mfr_comp_id in (" & Replace(MFR_String, "'", "") & ") "
      'End If


      ''regions
      If Not String.IsNullOrEmpty(CompanyRegionString.Trim) Then

        If Not String.IsNullOrEmpty(where_string.Trim) Then
          where_string += " AND "
        End If

        where_string += " comp_country in (select distinct geographic_country_name FROM geographic with (NOLOCK) where geographic_region_name in (" + CompanyRegionString.Trim + ")) "

        If CompanyStateName = "" Then
          where_string += " and (state_name in (select distinct state_name FROM geographic with (NOLOCK) inner join State with (NOLOCK) on state_code=geographic_state_code and state_country=geographic_country_name where geographic_region_name in (" + CompanyRegionString.Trim + ")) or state_name is null) "
        End If

      End If


      If Not String.IsNullOrEmpty(DynamicQueryStringGenerated.Trim) Then

        temp_refit_string = DynamicQueryStringGenerated

        temp_spot = 0
        temp_spot = InStr(Trim(temp_refit_string), refit_select)
        If temp_spot > 0 Then
          temp_refit_string_start = Left(Trim(temp_refit_string), temp_spot - 1)

          temp_refit_string_end = Right(Trim(temp_refit_string), Len(Trim(temp_refit_string)) - temp_spot - 175)

          temp_spot = InStr(Trim(temp_refit_string_end), "%")
          If temp_spot > 0 Then
            temp_refit_string_end = Right(Trim(temp_refit_string_end), Len(Trim(temp_refit_string_end)) - temp_spot)
          End If

          temp_spot = InStr(Trim(temp_refit_string_end), "%")
          If temp_spot > 0 Then
            temp_refit_string_mid = Left(Trim(temp_refit_string_end), temp_spot - 1)
            temp_refit_string_end = Right(Trim(temp_refit_string_end), Len(Trim(temp_refit_string_end)) - temp_spot - 1) ' 1 extra to get rid of tick
          End If

          If Trim(temp_refit_string_mid) <> "" Then
            If InStr(Trim(temp_refit_string_mid), ":") > 0 Then
              refit_start_year = Left(Trim(temp_refit_string_mid), InStr(Trim(temp_refit_string_mid), ":") - 1)
              refitt_end_year = Right(Trim(temp_refit_string_mid), Len(Trim(temp_refit_string_mid)) - InStr(Trim(temp_refit_string_mid), ":"))
              temp_refit_string = " ("

              For i = CInt(refit_start_year) To CInt(refitt_end_year)

                If i = CInt(refit_start_year) Then
                  temp_refit_string &= " (" & refit_select & " '%" & i & "%') "
                Else
                  temp_refit_string &= " or  (" & refit_select & " '%" & i & "%') "
                End If

              Next
              temp_refit_string &= " )"

              DynamicQueryStringGenerated = temp_refit_string_start & " " & temp_refit_string & " " & temp_refit_string_end

            End If
          End If

          'temp_refit_string

        End If

        If Not String.IsNullOrEmpty(where_string.Trim) Then
          where_string += " AND "
        End If

        where_string += DynamicQueryStringGenerated.Trim

      End If


      If InStr(Trim(where_string), "AND (yd_type = 'Equipment') AND (yd_name = 'Miscellaneous')") > 0 Then
        where_string = Replace(Trim(where_string), "AND (yd_type = 'Equipment') AND (yd_name = 'Miscellaneous')", "AND (yd_type in ('Equipment', 'Amenities')) AND (yd_name = 'Miscellaneous')")
      End If



      If Not String.IsNullOrEmpty(where_string.Trim) Then
        Query += " WHERE " + where_string
        HttpContext.Current.Session.Item("MasterYachtWhere") = " WHERE " + where_string
      End If

      If Not String.IsNullOrEmpty(PageSort.Trim) Then
        Query += " ORDER BY " + PageSort
        HttpContext.Current.Session.Item("MasterYachtSort") = " ORDER BY " + PageSort
      Else
        Query += " ORDER BY ym_brand_name "
        HttpContext.Current.Session.Item("MasterYachtSort") = " ORDER BY ym_brand_name"
      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b style='color:#ff0000;'>EvolutionYachtListingPageQuery(...) As DataTable</b><br />" & Query
      HttpContext.Current.Session.Item("MasterYacht") = Query

      Call commonLogFunctions.Log_User_Event_Data("UserSearch", "Yacht Search: " & clsGeneral.clsGeneral.StripChars(where_string, False), Nothing, 0, 0, 0, 0, 0, 0, 0)

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      SqlCommand.CommandText = Query
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

    Catch ex As Exception

      atemptable = Nothing

      Dim previousException As String = ex.Message.Trim

      Try

        If Not IsNothing(masterPage) Then
          masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
        Else
          HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.ToString.Trim
        End If
      Catch ex2 As Exception

        commonLogFunctions.forceLogError("ERROR", System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

      End Try

    Finally

      SqlReader = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

    End Try

    Return atemptable

  End Function

  Public Function EvolutionYachtEventListingPageQuery(ByVal Forsale_Flag As String, ByVal ForLease_Flag As String, _
                                                      ByVal ForCharter_Flag As String, ByVal MfrStart As String, _
                                                      ByVal MfrEnd As String, ByVal CallSign As String, ByVal LifecycleStage As String, _
                                                      ByVal brandString As String, ByVal yearString As String, _
                                                      ByVal CategorySize As String, ByVal MotorSize As String, _
                                                      ByVal PageSort As String, ByVal yacht_name_search As String, _
                                                      ByVal yachtIDs As String, ByVal yachtModels As String, _
                                                      ByVal MarketCategory As String, ByVal MarketType As String, _
                                                      ByVal StartDate As String, ByVal Ownership As String, _
                                                      ByVal YachtStatus As String, ByVal RegisteredCountryFlag As String, _
                                                      ByVal yearDlv As String, ByVal PreviousName As Boolean, _
                                                      ByVal YachtClass As String, _
                                                      ByVal useAltHullMFR As Boolean, ByVal EventTransactionSearch As Boolean, _
                                                      ByVal US_Waters_Flag As String, ByVal DynamicQueryStringGenerated As String, _
                                                      ByVal CompanyCountriesString As String, ByVal CompanyTimeZoneString As String, _
                                                      ByVal CompanyContinentString As String, ByVal CompanyRegionString As String, _
                                                      ByVal CompanyStateName As String, ByVal BrandMFR_String As String, _
                                                      ByVal yachtAskingPrice As String, ByVal yachtAskingPriceCurrency As String, ByVal yachtAskingPriceOperator As String) As DataTable
    Dim Query As String = ""

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim atemptable As New DataTable
    Dim where_string As String = ""
    Dim Query_From As String = ""

    Dim comp_count As DataColumn = New DataColumn("comp_count", Type.GetType("System.Int64"))
    comp_count.AutoIncrement = True
    comp_count.AutoIncrementSeed = 1

    atemptable.Columns.Add("ype_id")
    atemptable.Columns.Add("ype_subject")
    atemptable.Columns.Add("ype_description")
    atemptable.Columns.Add("ype_comp_id")
    atemptable.Columns.Add("ype_contact_id")
    atemptable.Columns.Add("ype_entered_date")
    atemptable.Columns.Add("ype_yt_id") 'Added Yacht ID per email 3/27/2014

    atemptable.Columns.Add("ym_brand_name")
    atemptable.Columns.Add("ym_model_name")
    atemptable.Columns.Add("yt_yacht_name")
    atemptable.Columns.Add("yt_year_mfr")
    atemptable.Columns.Add("yt_radio_call_sign")
    atemptable.Columns.Add("yt_hull_mfr_nbr")


    Try

      HttpContext.Current.Session.Item("MasterYachtEvents") = "" 'Whole Search
      HttpContext.Current.Session.Item("MasterYachtEventsWhere") = "" 'Where Only
      HttpContext.Current.Session.Item("MasterYachtEventsFrom") = "" 'From Variable Only

      HttpContext.Current.Session.Item("MasterYachtFrom") = Nothing 'MAKE IT nothing, so we know it was an events search

      Query = " select distinct ype_id, ype_yt_id, yt_id, ype_subject as apev_subject, "
      Query += "  cast(ype_description as varchar(1000)) as apev_description, ype_comp_id, ype_contact_id, "
      Query += " ype_entered_date as apev_action_date , ype_entered_date as apev_entry_date,"
      Query += " ym_brand_name, ym_model_name, yt_yacht_name, yt_year_mfr, yt_radio_call_sign,yt_hull_mfr_nbr"

      Query_From = " from Yacht_Priority_Events WITH(NOLOCK) "
      Query_From += " inner join Yacht_Priority_Events_Category WITH(NOLOCK) on ype_category_code=ypec_category_code"
      Query_From += " inner join Yacht WITH(NOLOCK)  on ype_yt_id = yt_id and yt_journ_id=0"


      Query_From += "  left outer join yacht_reference WITH(NOLOCK) on yr_yt_id = yt_id  AND yr_journ_id = yt_journ_id "
      Query_From += "  left outer join company  WITH(NOLOCK) on comp_id = yr_comp_id and comp_journ_id = yr_journ_id "
      Query_From += "  left outer join contact  WITH(NOLOCK) on contact_id = yr_contact_id  and contact_comp_id = yr_comp_id and contact_journ_id = yr_journ_id "


      If CompanyStateName <> "" Then
        Query_From += "  LEFT OUTER JOIN [State] WITH(NOLOCK) on state_code = comp_state and state_country=comp_country "
      End If
      If CompanyContinentString <> "" Then
        Query_From += " left outer join Country with (NOLOCK) on comp_country=country_name "
      End If

      If PreviousName Then
        Query_From += " left outer join Yacht_Previous_Names WITH(NOLOCK) "
        Query_From += " on ypn_yt_id = yt_id"
      End If



      Query_From += " inner join Yacht_Model WITH(NOLOCK)  on yt_model_id = ym_model_id "
      HttpContext.Current.Session.Item("MasterYachtEventsFrom") = Query_From

      Query += Query_From
      'For sale Flag:
      If Trim(Forsale_Flag) <> "" Then
        If where_string <> "" Then
          where_string += " and "
        End If
        If Trim(Forsale_Flag) = "Y" Then
          where_string = where_string & " yt_forsale_flag = 'Y' "
          'Else
          '    where_string = where_string & " yt_forsale_flag = 'N' "
        End If
      End If
      'Lease flag:
      If Trim(ForLease_Flag) <> "" Then
        If where_string <> "" Then
          where_string += " and "
        End If
        If Trim(ForLease_Flag) = "Y" Then
          where_string = where_string & " yt_for_lease_flag = 'Y' "
          'Else
          '    where_string = where_string & " yt_for_lease_flag = 'N' "
        End If
      End If
      'Country registered flag:
      If Trim(RegisteredCountryFlag) <> "" Then
        'If Trim(RegisteredCountryFlag) = "Y" Then
        If where_string <> "" Then
          where_string += " and "
        End If
        where_string += " yt_registered_country_flag = '" & RegisteredCountryFlag & "' "
        'Else
        '    where_string += " yt_registered_country_flag = 'N' "
        'End If
      End If
      'Charter flag:
      If Trim(ForCharter_Flag) <> "" Then
        If where_string <> "" Then
          where_string += " and "
        End If
        If Trim(ForCharter_Flag) = "Y" Then
          where_string = where_string & " yt_for_charter_flag = 'Y' "
          'Else
          '    where_string = where_string & " yt_for_charter_flag = 'N' "
        End If
      End If

      If CompanyTimeZoneString <> "" Then
        If where_string <> "" Then
          where_string += " and "
        End If

        where_string += " comp_timezone in (SELECT tzone_name FROM Timezone where tzone_id in (" & CompanyTimeZoneString & ")) "
      End If


      'Continent
      If CompanyContinentString <> "" Then
        If where_string <> "" Then
          where_string += " AND"
        End If
        where_string += " country_continent_name in (" & CompanyContinentString & ") "
      End If


      ' check the country
      If CompanyCountriesString <> "" Then
        If where_string <> "" Then
          where_string += " AND "
        End If

        where_string += " comp_country in (" & CompanyCountriesString & ") "
      End If

      ' check the state
      If CompanyStateName <> "" Then
        If where_string <> "" Then
          where_string += " AND "
        End If
        where_string += " state_name IN (" & CompanyStateName & ")"
      End If
      If CompanyRegionString <> "" Then
        If where_string <> "" Then
          where_string += " AND "
        End If
        where_string += " comp_country in (select distinct geographic_country_name FROM geographic with (NOLOCK) where geographic_region_name in (" & CompanyRegionString & ")) "

        If CompanyStateName = "" Then
          where_string += " and (state_name in (select distinct state_name FROM geographic with (NOLOCK) inner join State with (NOLOCK) on state_code=geographic_state_code and state_country=geographic_country_name where geographic_region_name in (" & CompanyRegionString & ")) or state_name is null) "
        End If
      End If

      ' for the search on the general tab
      If Trim(BrandMFR_String) <> "" Then
        If where_string <> "" Then
          where_string += " and "
        End If
        where_string += BrandMFR_String
      End If

      'Status 
      If Trim(YachtStatus) <> "" Then
        If where_string <> "" Then
          where_string += " and "
        End If
        where_string += " yt_forsale_status in (" & YachtStatus & ") "
      End If

      'Regional/For Sale Restrictions
      If Trim(US_Waters_Flag) <> "" Then
        If where_string <> "" Then
          where_string += " and "
        End If
        If Trim(US_Waters_Flag) = "Y" Then
          where_string += " yt_not_in_usa_water = 'Y' "
        ElseIf Trim(US_Waters_Flag) = "N" Then
          where_string += " yt_not_in_usa_water <> 'Y' "
        End If
      End If


      'The market category
      If MarketCategory <> "" Then
        If where_string <> "" Then
          where_string += " and "
        End If
        where_string += "  ypec_category in (" & MarketCategory & ") "
      End If
      'The market category type.
      If MarketType <> "" Then
        If where_string <> "" Then
          where_string += " and "
        End If
        where_string += "  ypec_category_name in (" & MarketType & ") "
      End If
      'Start Date
      If StartDate <> "" Then
        If where_string <> "" Then
          where_string += " and "
        End If
        where_string += " ype_entered_date >= '" & StartDate & "'"
      End If
      'Ownership 
      If Ownership <> "" Then
        If where_string <> "" Then
          where_string += " and "
        End If
        where_string += " yt_ownership_type in (" & Ownership & ")"
      End If
      'Lifecycle stage
      If LifecycleStage <> "" Then
        If where_string <> "" Then
          where_string += " and "
        End If
        where_string += " (" & LifecycleStage & ")"

      End If
      'Yacht IDs in the case of static folders
      If yachtIDs <> "" Then
        If where_string <> "" Then
          where_string += " and "
        End If
        where_string += " yt_id in (" & yachtIDs & ") "
      End If

      'If Trim(CentralAgent) <> "" Then
      '    If where_string <> "" Then
      '        where_string += " and " 
      '    End If
      '    If Trim(CentralAgent) = "Y" Then
      '        where_string = where_string & " yt_central_agent_flag = 'Y' "
      '    Else
      '        where_string = where_string & " yt_central_agent_flag = 'N' "
      '    End If
      'End If
      If Trim(YachtClass) <> "" Then
        If where_string <> "" Then
          where_string += " and "
        End If
        where_string += "  yt_class_id ='" & YachtClass & "'"
      End If

      'Yacht Brands
      If Trim(brandString) <> "" Then
        If where_string <> "" Then
          where_string += " AND "
        End If
        where_string = where_string & " ym_brand_name IN (" + brandString + ") "
      End If

      'yacht models
      If Trim(yachtModels) <> "" Then
        If where_string <> "" Then
          where_string += " AND "
        End If
        where_string += " ym_model_name IN (" + yachtModels + ") "
      End If

      'yacht motor 
      If Trim(MotorSize) <> "" Then
        If where_string <> "" Then
          where_string += " AND "
        End If
        where_string += " ym_motor_type IN (" + MotorSize + ") "
      End If

      'yacht category size
      If Trim(CategorySize) <> "" Then
        If where_string <> "" Then
          where_string += " AND "
        End If
        where_string += " ym_category_size IN (" + CategorySize + ") "
      End If

      'Yacht Year String
      If Trim(yearString) <> "" Then
        If where_string <> "" Then
          where_string += " and "
        End If
        where_string = where_string & " yt_year_mfr = '" & yearString & "' "
      End If
      'yacht year dlv
      If Trim(yearDlv) <> "" Then
        If where_string <> "" Then
          where_string += " and "
        End If
        where_string += " yt_year_dlv = '" & yearDlv & "' "
      End If

      'Yacht MFR start
      If Trim(MfrStart) <> "" Then
        If where_string <> "" Then
          where_string += " and "
        End If
        where_string += " ( yt_hull_mfr_nbr like '" & MfrStart & "%' "

        If useAltHullMFR Then
          where_string += " or yt_alt_hull_mfr_nbr  like '" & MfrStart & "%' ) "
        Else
          where_string += " ) "
        End If
      End If

      'Yacht MFR end
      If Trim(MfrEnd) <> "" Then
        If where_string <> "" Then
          where_string += " and "
        End If
        where_string += " ( yt_hull_mfr_nbr like '%" & MfrEnd & "' "

        If useAltHullMFR Then
          where_string += " or yt_alt_hull_mfr_nbr like '%" & MfrEnd & "' ) "
        Else
          where_string += " ) "
        End If
      End If


      'Yacht Call Sign
      If Trim(CallSign) <> "" Then
        If where_string <> "" Then
          where_string += " and "
        End If
        where_string = where_string & " yt_radio_call_sign = '" & CallSign & "' "
      End If

      'Yacht Name Search
      If Trim(yacht_name_search) <> "" Then
        If where_string <> "" Then
          where_string += " and "
        End If
        where_string += " (yt_yacht_name like '" & yacht_name_search & "%' "
        'previous name
        If PreviousName Then
          If where_string <> "" Then
            where_string += " or "
          End If
          where_string += " ypn_previous_name like '" & yacht_name_search & "%' "
        End If
        where_string += " )"
      End If

      If Not String.IsNullOrEmpty(yachtAskingPrice.Trim) Then

        Dim sTmpStr As String = ""
        Dim exchangeRate As Double = 0.0
        Dim sCurrencyName As String = ""

        If Not yachtAskingPriceCurrency.Contains("9") Then

          ' if not us dollar get the "conversion" translation
          exchangeRate = commonEvo.GetForeignExchangeRate(CInt(yachtAskingPriceCurrency), sCurrencyName, "")

          If yachtAskingPrice.Contains(":") Then
            Dim tmpPriceArray() As String = Split(yachtAskingPrice, ":")
            yachtAskingPrice = (CDbl(tmpPriceArray(0)) * exchangeRate).ToString + ":" + (CDbl(tmpPriceArray(1)) * exchangeRate).ToString
          Else
            yachtAskingPrice = (CDbl(yachtAskingPrice) * exchangeRate).ToString
          End If


        End If

        If yachtAskingPrice.Contains(":") Or yachtAskingPriceOperator.ToLower.Contains("between") Then

          Dim tmpPriceArray() As String = Split(yachtAskingPrice, ":")
          ' split the asking price and generate "between" clause
          If tmpPriceArray.Length > 1 Then
            sTmpStr = IIf(Not String.IsNullOrEmpty(where_string.Trim), " AND ", "") + IIf(exchangeRate = 0, "yt_asking_price BETWEEN ", "(yt_asking_price * " + exchangeRate.ToString + ") BETWEEN ") + tmpPriceArray(0).Trim + " AND " + tmpPriceArray(1).Trim + " "
          Else
            sTmpStr = IIf(Not String.IsNullOrEmpty(where_string.Trim), " AND ", "") + IIf(exchangeRate = 0, "yt_asking_price ", "(yt_asking_price * " + exchangeRate.ToString + ") ") + Constants.cEq + tmpPriceArray(0).Trim + " "
          End If

        Else

          Select Case (yachtAskingPriceOperator.ToLower)

            Case "equals"
              sTmpStr = IIf(Not String.IsNullOrEmpty(where_string.Trim), " AND ", "") + IIf(exchangeRate = 0, "yt_asking_price ", "(yt_asking_price * " + exchangeRate.ToString + ") ") + Constants.cEq + yachtAskingPrice.Trim + " "
            Case "greater than"
              sTmpStr = IIf(Not String.IsNullOrEmpty(where_string.Trim), " AND ", "") + IIf(exchangeRate = 0, "yt_asking_price ", "(yt_asking_price * " + exchangeRate.ToString + ") ") + Constants.cGt + yachtAskingPrice.Trim + " "
            Case "less than"
              sTmpStr = IIf(Not String.IsNullOrEmpty(where_string.Trim), " AND ", "") + IIf(exchangeRate = 0, "yt_asking_price ", "(yt_asking_price * " + exchangeRate.ToString + ") ") + Constants.cLt + yachtAskingPrice.Trim + " "

          End Select

        End If

        where_string += sTmpStr

      End If

      If Not String.IsNullOrEmpty(DynamicQueryStringGenerated.Trim) Then
        If Not String.IsNullOrEmpty(where_string.Trim) Then
          where_string += " and "
        End If
        where_string += DynamicQueryStringGenerated
      End If

      If Not String.IsNullOrEmpty(where_string.Trim) Then

        where_string += " AND ype_hide_flag = 'N' "

        Query += " WHERE " + where_string
        HttpContext.Current.Session.Item("MasterYachtEventsWhere") = " WHERE " + where_string
      End If

      If Not String.IsNullOrEmpty(PageSort.Trim) Then
        Query += " ORDER BY " + PageSort
        HttpContext.Current.Session.Item("MasterYachtSort") = " ORDER BY " + PageSort
      Else
        Query += " ORDER BY ym_brand_name "
        HttpContext.Current.Session.Item("MasterYachtSort") = " ORDER BY ym_brand_name"
      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b style='color:#ff0000;'>EvolutionYachtEventListingPageQuery(...) As DataTable</b><br />" & Query
      HttpContext.Current.Session.Item("MasterYachtEvents") = Query

      Call commonLogFunctions.Log_User_Event_Data("UserSearch", "Yacht Event Search: " & clsGeneral.clsGeneral.StripChars(where_string, False), Nothing, 0, 0, 0, 0, 0, 0, 0)

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      SqlCommand.CommandText = Query
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
      End Try

    Catch ex As Exception

      atemptable = Nothing

      Dim previousException As String = ex.Message.Trim

      Try

        If Not IsNothing(masterPage) Then
          masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.Trim)
        Else
          HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " (" + ErrorReportingTypeString.Trim + "): " + ex.Message.ToString.Trim
        End If
      Catch ex2 As Exception

        commonLogFunctions.forceLogError("ERROR", System.Reflection.MethodInfo.GetCurrentMethod().ToString.Trim + " (" + ErrorReportingTypeString.Trim + "): Previous Exception Thrown[" + previousException.Trim + "] : Exception Thrown[" + ex2.Message.Trim + "]")

      End Try

    Finally
      SqlReader = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

    End Try

    Return atemptable

  End Function

End Class