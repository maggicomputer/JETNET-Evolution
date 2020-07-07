Partial Public Class Wanted_Listing
  Inherits System.Web.UI.Page
  Dim aclsData_temp As New clsData_Manager_SQL
  Public productCodeCount As Integer = 0
  Public isHeliOnlyProduct As Boolean = False
  Dim TempTable As New DataTable
  Dim TypeDataTable As New DataTable
  Dim TypeDataHold As New DataTable
  Dim masterPage As New EvoTheme
  Dim PageNumber As Integer = 1
  Dim PageSort As String = ""

  Private sTypeMakeModelCtrlBaseName As String = "Aircraft"
  Public bUsernameExists As Boolean = False

  Private Sub Wanted_Listing_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

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

      ''Adding redirect because aerodex shouldn't be able to view this.
      If Session.Item("localSubscription").crmAerodexFlag Then
        Response.Redirect("home.aspx", False)
      Else

        If Not Page.IsPostBack Then
          'Add help button text here: 7/20/15
          wanted_help_text.Text = clsGeneral.clsGeneral.CreateEvoHelpLink("Wanted")


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
                  wanted_folder_name.Text = FoldersTableData.Rows(0).Item("cfolder_name").ToString
                End If
                If cfolderData <> "" Then
                  'Fills up the applicable folder Information pulled from the cfolder data field
                  DisplayFunctions.FillUpFolderInformation(Table4, close_current_folder, cfolderData, FolderInformation, FoldersTableData, False, False, False, True, False, wanted_Collapse_Panel, wanted_actions_submenu_dropdown)
                End If
              End If
            End If
          End If
        End If
      End If
    End If
  End Sub

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Dim FoldersTable As New DataTable
    If Session.Item("crmUserLogon") <> True Then
      Response.Redirect("Default.aspx", False)
    Else
      aclsData_temp = New clsData_Manager_SQL
      aclsData_temp.JETNET_DB = Session.Item("jetnetClientDatabase")


      ToggleHigherLowerBar(False)

      ViewTMMDropDowns.setIsView(False)

      ViewTMMDropDowns.setShowWeightClass(True)
      ViewTMMDropDowns.setShowMfrNames(True)
      ViewTMMDropDowns.setShowAcSize(True)

      ViewTMMDropDowns.setListSize(8)
      ViewTMMDropDowns.setControlName(sTypeMakeModelCtrlBaseName)

      'Added 7/01/2015.
      'This is going to set the paging item to be the one you have saved in session.
      If Not Page.IsPostBack Then
        If Wanted_Criteria.Visible = True Then
          DisplayFunctions.SetPagingItem(wanted_per_page_dropdown)
        End If
      End If

      DisplayFunctions.FillUpSessionForMakeTypeModel(sTypeMakeModelCtrlBaseName, ViewTMMDropDowns)

      If Not Page.IsPostBack And Page.Request.Form("complete_search") <> "Y" Then
        Initial(True)
      Else
        Initial(False)
      End If

      '    'Pass the tab index of what you want highlighted on the bar.
      Master.Set_Active_Tab(9)
      'This will set page title.
      Me.Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Wanted Search Results")


      'Set up bars to display correctly.
      If Not Page.IsPostBack Then

        'Fill Folders Table
        wanted_folders_submenu_dropdown.Items.Clear()
        DisplayFunctions.AddEditFolderListOptionToFolderDropdown(wanted_folders_submenu_dropdown, 9)
        FoldersTable = Master.aclsData_Temp.GetEvolutionFolderssBySubscription(0, Session.Item("localUser").crmUserLogin, Session.Item("localUser").crmSubSubID, Session.Item("localUser").crmSubSeqNo, "", 9, Nothing, "")
        If Not IsNothing(FoldersTable) Then
          If FoldersTable.Rows.Count > 0 Then
            For Each r As DataRow In FoldersTable.Rows
              If Not IsDBNull(r("cfolder_data")) Then
                Dim FolderDataString As Array
                'this was added to parse out the real search query now that we're saving it
                FolderDataString = Split(r("cfolder_data"), "THEREALSEARCHQUERY")

                If Replace(r("cfolder_data").ToString, "amwant_id=", "") <> "" Then
                  wanted_folders_submenu_dropdown.Items.Add(New ListItem(r("cfolder_name").ToString, "javascript:ParseForm('" & r("cfolder_id").ToString & "',false,false,false,true,false,'" & Replace(FolderDataString(0), "'", "\'") & "');"))
                Else
                  wanted_folders_submenu_dropdown.Items.Add(New ListItem(r("cfolder_name").ToString, "javascript:alert('This folder contains no information.');"))
                End If
              End If
            Next
          End If
        End If
      End If




      End If
  End Sub

  ''' <summary>
  ''' Sets page sort, works only for ac page.
  ''' </summary>
  ''' <param name="selectedLI"></param>
  ''' <remarks></remarks>
  Public Sub SetPageSort(Optional ByVal selectedLI As String = "")
    Select Case selectedLI
      Case "Make/Model"
        PageSort = " amod_make_name, amod_model_name "
      Case "Interested Party"
        PageSort = " comp_name "
      Case Else
        PageSort = " amwant_listed_date "
    End Select
  End Sub
  ''' <summary>
  ''' Sets dropdown page #
  ''' </summary>
  ''' <param name="selectedLI"></param>
  ''' <remarks></remarks>
  Public Sub SetPageNumber(Optional ByVal selectedLI As Integer = 0)
    PageNumber = selectedLI
  End Sub

  Public Sub submenu_dropdown_Click(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.BulletedListEventArgs)

    Dim selectedLI As New ListItem
    selectedLI = sender.Items(e.Index)
    If sender.id.ToString = "wanted_sort_submenu_dropdown" Then
      wanted_sort_dropdown.Items.Clear()
      wanted_sort_dropdown.Items.Add(New ListItem(selectedLI.Text, ""))
      SetPageSort(selectedLI.Text)
      wanted_search_Click(wanted_search, EventArgs.Empty)
    ElseIf sender.id.ToString = "wanted_per_page_submenu_dropdown" Then
      wanted_per_page_dropdown.Items.Clear()
      wanted_per_page_dropdown.Items.Add(New ListItem(selectedLI.Text & " ", selectedLI.Text))
      Session.Item("localUser").crmUserRecsPerPage = CInt(selectedLI.Value)
      MovePage(False, False, False, False, False, PageNumber)
    End If
  End Sub
    ''' <summary>
    ''' Runs the search
    ''' </summary>
    ''' <param name="ModelString"></param>
    ''' <param name="StartDate"></param>
    ''' <param name="EndDate"></param>
    ''' <param name="CompanyName"></param>
    ''' <param name="bindFromSession"></param>
    ''' <remarks></remarks>
    Public Sub WantedSearch(ByVal ModelString As String, ByVal MakeString As String, ByVal ModelType As String, ByVal AirframeType As String,
                          ByVal WeightClass As String, ByVal ManufacturerName As String, ByVal AcSize As String, ByVal StartDate As String, ByVal EndDate As String,
                          ByVal CompanyName As String, ByVal PageSort As String, ByVal WantedIDs As String,
                          ByVal Business As Boolean, ByVal Commercial As Boolean, ByVal Helicopter As Boolean, Optional ByVal bindFromSession As Boolean = False, Optional ByVal placed_by As String = "")

        Dim RecordsPerPage As Integer = 0
        Dim Results_Table As New DataTable
        If Session.Item("localUser").crmUserRecsPerPage <> 0 Then
            RecordsPerPage = Session.Item("localUser").crmUserRecsPerPage
        End If
        wanted_attention.Text = ""
        'If ModelString <> "" Then
        Master.SetStatusText(HttpContext.Current.Session.Item("SearchString"))

        If bindFromSession = True And Not IsNothing(Session.Item("Wanted_Master")) Then
            Results_Table = Session.Item("Wanted_Master")
        Else
            Results_Table = Return_Wanted_Evo(0, "", 0, ModelString, StartDate, EndDate, CompanyName, "J", 0, PageSort,
                                                      MakeString, ModelType, AirframeType, WeightClass, ManufacturerName, AcSize,
                                                      WantedIDs, Business, Commercial, Helicopter, placed_by)
            Session.Item("Wanted_Master") = Results_Table
        End If

        Call commonLogFunctions.Log_User_Event_Data("UserSearch", "Wanted Search: " & clsGeneral.clsGeneral.StripChars(clsGeneral.clsGeneral.stripHTML(Replace(HttpContext.Current.Session.Item("SearchString"), "<br />", " ")), False), Nothing, 0, 0, 0, 0, 0, 0, 0)

        If Not IsNothing(Results_Table) Then
            Session.Item("localUser").crmLatestRecordCount = Results_Table.Rows.Count
            If Results_Table.Rows.Count > 0 Then
                'Added 7/01/2015
                'This will reset the current page index to be 0 whenever a search is performed.
                'This resets the datagrid so that way if you're paging, it will start back at the first page.
                Results.CurrentPageIndex = 0

                Results.PageSize = RecordsPerPage
                Results.Visible = True
                Results.DataSource = Results_Table
                Results.DataBind()

                wanted_criteria_results.Text = Results_Table.Rows.Count & " Results"
                wanted_record_count.Text = "Showing 1 - " & IIf(Results_Table.Rows.Count <= RecordsPerPage, Results_Table.Rows.Count, RecordsPerPage)

                'This will fill up the dropdown bar with however many pages.
                If Results_Table.Rows.Count > RecordsPerPage Then
                    SetPagingButtons(False, True)
                Else
                    SetPagingButtons(False, False)
                End If

                wanted_PanelCollapseEx.Collapsed = True
                Results_Table = Nothing

            Else
                'Added 07/01/2015
                'This clears the current page index when no results are found.
                Results.CurrentPageIndex = 0

                Results.DataSource = New DataTable
                Results.DataBind()
                Results.Visible = False
                SetPagingButtons(False, False)

                wanted_attention.Text = "<br /><p class='padding'><b>No Wanteds Found. Please refine your search and try again.</b></p><br /><br />"

                wanted_criteria_results.Text = "0 Results"

                wanted_record_count.Text = "Showing 0 Results"

            End If
        End If
        '  End If
    End Sub

    ''' <summary>
    ''' move page, handles the moving of the page. Sends off to a function which sets the paging correctly.
    ''' </summary>
    ''' <param name="next_"></param>
    ''' <param name="prev_"></param>
    ''' <param name="next_all"></param>
    ''' <param name="prev_all"></param>
    ''' <param name="goToPage"></param>
    ''' <param name="pageNumber"></param>
    ''' <remarks></remarks>
    Public Sub MovePage(ByVal next_ As Boolean, ByVal prev_ As Boolean, ByVal next_all As Boolean, ByVal prev_all As Boolean, ByVal goToPage As Boolean, ByVal pageNumber As Integer)
    Dim holdTable As New DataTable
    Dim StartCount As Integer = 0
    Dim EndCount As Integer = 0
    Dim RecordsPerPage As Integer = 0
    If Session.Item("localUser").crmUserRecsPerPage <> 0 Then
      RecordsPerPage = Session.Item("localUser").crmUserRecsPerPage
    End If

    If Not IsNothing(Session.Item("Wanted_Master")) Then
      holdTable = Session.Item("Wanted_Master")
      Initial(False)
      DisplayFunctions.MovePage(StartCount, EndCount, Results, Nothing, holdTable, next_, prev_, next_all, prev_all, goToPage, pageNumber)
      SetPagingButtons(IIf(StartCount = 1, False, True), IIf(holdTable.Rows.Count = EndCount, False, True))

      wanted_record_count.Text = "Showing " & StartCount & " - " & IIf(holdTable.Rows.Count <= RecordsPerPage, holdTable.Rows.Count, IIf((RecordsPerPage + StartCount) <= holdTable.Rows.Count, RecordsPerPage + StartCount, holdTable.Rows.Count))

    End If
  End Sub

  ''' <summary>
  ''' Handles the next/previous button clicks.
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub next__Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles wanted_next_.Click, wanted_previous.Click, wanted_next_all.Click, wanted_previous_all.Click
    If sender.commandname.ToString = "next" Then
      MovePage(True, False, False, False, False, 0)
    ElseIf sender.commandname.ToString = "previous" Then
      MovePage(False, True, False, False, False, 0)
    ElseIf sender.commandname.ToString = "next_all" Then
      MovePage(False, False, True, False, False, 0)
    ElseIf sender.commandname.ToString = "previous_all" Then
      MovePage(False, False, False, True, False, 0)
    End If
  End Sub

  ''' <summary>
  ''' Search click button
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub wanted_search_Click(ByVal sender As Object, ByVal e As System.EventArgs, Optional ByVal LoadfromSession As Boolean = False) Handles wanted_search.Click
    Dim ModelsString As String = ""

    Dim WeightClassDDL As New Object
    Dim WeightClass As String = ""

    Dim ManufacturerStr As String = ""

    Dim AcSizeStr As String = ""

    Dim MakeString As String = ""
    Dim TypeString As String = ""
    Dim WantedIDs As String = ""
    Dim AirframeTypeString As String = ""
    Dim CombinedAirframeTypeString As String = ""

    Dim StartDate As String = ""
    Dim EndDate As String = ""
    Dim BuildSearchString As String = ""
    Dim Business As Boolean = False
    Dim Commercial As Boolean = False
    Dim Helicopter As Boolean = False

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

    'Start Date
    If IsDate(wanted_from.Text) Then
      StartDate = Year(wanted_from.Text) & "-" & Month(wanted_from.Text) & "-" & Day(wanted_from.Text)
      BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(StartDate, "Start Date")
    End If

    If amwant_id.Text <> "" Then
      WantedIDs = Trim(amwant_id.Text)
      BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(wanted_folder_name.Text, "Folder")
    End If

    'End Date
    If IsDate(wanted_to.Text) Then
      EndDate = Year(wanted_to.Text) & "-" & Month(wanted_to.Text) & "-" & Day(wanted_to.Text)
      BuildSearchString += DisplayFunctions.BuildSearchTextDisplay(EndDate, "End Date")
    End If

    HttpContext.Current.Session.Item("SearchString") = BuildSearchString

    Initial(False)
        'Calling Search.
        WantedSearch(ModelsString, MakeString, TypeString, AirframeTypeString, WeightClass, ManufacturerStr, AcSizeStr,
                 StartDate, EndDate, clsGeneral.clsGeneral.StripChars(wanted_interested.Text, True),
                 PageSort, WantedIDs, Business, Commercial, Helicopter, LoadfromSession, wanted_placed_by.SelectedValue)
    End Sub
  ''' <summary>
  ''' Fills page for dropdown.
  ''' </summary>
  ''' <param name="pageNumber"></param>
  ''' <remarks></remarks>
  Public Sub Fill_Page_To_To_Dropdown(ByVal pageNumber As Integer)

    wanted_go_to_submenu_dropdown.Items.Clear()
    For x = 1 To pageNumber
      wanted_go_to_submenu_dropdown.Items.Add(New ListItem(x, x))
    Next

  End Sub

  ''' <summary>
  ''' Toggles visibility of next/prev
  ''' </summary>
  ''' <param name="back_page"></param>
  ''' <param name="next_page"></param>
  ''' <remarks></remarks>
  Public Sub SetPagingButtons(ByVal back_page As Boolean, ByVal next_page As Boolean)
    wanted_previous_all.Visible = back_page
    wanted_previous.Visible = back_page

    wanted_next_all.Visible = next_page
    wanted_next_.Visible = next_page

  End Sub


  ''' <summary>
  ''' This runs on the initial load of the page. It'll toggle off some of the paging elements and things we don't need displayed if we're first coming into the page.
  ''' </summary>
  ''' <param name="initial_page_load"></param>
  ''' <remarks></remarks>
  Public Sub Initial(ByVal initial_page_load As Boolean)
    wanted_actions_submenu_dropdown.Items.Clear()
    wanted_actions_submenu_dropdown.Items.Add(New ListItem("Add Wanted", "javascript:load('enterWantedInfo.aspx','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');"))

    If initial_page_load = True Then

      wanted_criteria_results.Visible = False
      wanted_sort_by_text.Visible = False
      wanted_sort_by_dropdown.Visible = False
      'wanted_actions_dropdown.Visible = False
      wanted_paging.Visible = False
      wanted_per_page_text.Visible = False
      wanted_per_page_dropdown_.Visible = False
      wanted_go_to_text.Visible = False
      wanted_go_to_dropdown_.Visible = False

      wanted_PanelCollapseEx.Collapsed = False
      wanted_PanelCollapseEx.ClientState = False

    Else
      'This adds the other buttons to the wanted action dropdown that shouldn't be there on the initial load.
      wanted_actions_submenu_dropdown.Items.Add(New ListItem("Save As - New Folder", "javascript:SubMenuDrop(3,0, 'WANTED');"))
      wanted_actions_submenu_dropdown.Items.Add(New ListItem("Custom Export", "javascript:SubMenuDrop(1,0,'WANTED');"))
      wanted_actions_submenu_dropdown.Items.Add(New ListItem("JETNET Export/Report", "javascript:SubMenuDrop(5,0,'WANTED');"))
      wanted_actions_submenu_dropdown.Items.Add(New ListItem("Summary", "javascript:SubMenuDrop(2,0,'WANTED');"))

      wanted_criteria_results.Visible = True
      wanted_sort_by_text.Visible = True
      wanted_sort_by_dropdown.Visible = True
      wanted_actions_dropdown.Visible = True
      wanted_paging.Visible = True
      wanted_per_page_dropdown_.Visible = True
      wanted_PanelCollapseEx.Collapsed = True
      wanted_PanelCollapseEx.ClientState = True

      wanted_per_page_text.Visible = True
      wanted_per_page_dropdown_.Visible = True


    End If
  End Sub
  ''' <summary>
  ''' Toggles the bar whether it's the high bar or the low bar. This sets up the javascript for the bulleted lists as well.
  ''' </summary>
  ''' <param name="lower_bar"></param>
  ''' <remarks></remarks>
  Public Sub ToggleHigherLowerBar(ByVal lower_bar As Boolean)
    'setting the javascript of the menus

    wanted_sort_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & wanted_sort_submenu_dropdown.ClientID & "', true);")
    wanted_sort_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & wanted_sort_submenu_dropdown.ClientID & "', false);")

    wanted_sort_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & wanted_sort_submenu_dropdown.ClientID & "', true);")
    wanted_sort_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & wanted_sort_submenu_dropdown.ClientID & "', false);")

    'page dropdown
    wanted_per_page_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & wanted_per_page_submenu_dropdown.ClientID & "', true);")
    wanted_per_page_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & wanted_per_page_submenu_dropdown.ClientID & "', false);")

    wanted_per_page_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & wanted_per_page_submenu_dropdown.ClientID & "', true);")
    wanted_per_page_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & wanted_per_page_submenu_dropdown.ClientID & "', false);")

    'go to dropdown
    wanted_go_to_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & wanted_go_to_submenu_dropdown.ClientID & "', true);")
    wanted_go_to_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & wanted_go_to_submenu_dropdown.ClientID & "', false);")

    wanted_go_to_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & wanted_go_to_submenu_dropdown.ClientID & "', true);")
    wanted_go_to_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & wanted_go_to_submenu_dropdown.ClientID & "', false);")

    'folder dropdown
    wanted_folders_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & wanted_folders_submenu_dropdown.ClientID & "', true);")
    wanted_folders_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & wanted_folders_submenu_dropdown.ClientID & "', false);")

    wanted_folders_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & wanted_folders_submenu_dropdown.ClientID & "', true);")
    wanted_folders_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & wanted_folders_submenu_dropdown.ClientID & "', false);")



    'actions dropdown
    wanted_actions_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & wanted_actions_submenu_dropdown.ClientID & "', true);")
    wanted_actions_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & wanted_actions_submenu_dropdown.ClientID & "', false);")

    wanted_actions_submenu_dropdown.Attributes.Add("onmouseover", "javascript:ShowBar('" & wanted_actions_submenu_dropdown.ClientID & "', true);")
    wanted_actions_submenu_dropdown.Attributes.Add("onmouseout", "javascript:ShowBar('" & wanted_actions_submenu_dropdown.ClientID & "', false);")

    If lower_bar = True Then
      wanted_PanelCollapseEx.Enabled = True
      wanted_PanelCollapseEx.ClientState = True
      wanted_search_expand_text.Visible = False
      wanted_help_text.Visible = False
      wanted_sort_by_text.Visible = False
      wanted_sort_by_dropdown.Visible = False

    Else
      wanted_per_page_dropdown_.Visible = False
      wanted_per_page_text.Visible = False
      wanted_go_to_dropdown_.Visible = False
      wanted_go_to_text.Visible = False
    End If

  End Sub

  Private Sub Wanted_Listing_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
    If Page.Request.Form("project_search") = "Y" Then
      'if either of these variables is passed, then go ahead and complete this search.
      wanted_search_Click(wanted_search, EventArgs.Empty)
    End If
  End Sub

  Private Sub ResetPage()
    ClearSelections()
    Response.Redirect("Wanted_Listing.aspx")
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
  Private Sub reset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles reset.Click
    ResetPage()
  End Sub

  Private Sub Wanted_Listing_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
    Master.SetDefaultButtion(Me.wanted_search.UniqueID)
  End Sub

    Private Function Return_Wanted_Evo(ByVal compID As Long, ByVal companySource As String, ByVal otherID As Long, ByVal amodID As String,
                                     ByVal start_date As String, ByVal end_date As String, ByVal company_name As String, ByVal subset As String,
                                     ByVal JournalID As Long, ByVal PageSort As String, ByVal MakeString As String, ByVal ModelType As String, ByVal AirframeType As String,
                                     ByVal WeightClass As String, ByVal ManufacturerName As String, ByVal AcSize As String, ByVal WantedIDs As String,
                                     ByVal Business As Boolean, ByVal Commercial As Boolean, ByVal Helicopter As Boolean, ByVal placed_by As String) As DataTable
        Dim sql As String = ""
        Dim sqlwhere As String = ""
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim atemptable As New DataTable


        'Dim afileterd As DataRow()
        'Dim Return_Table As New DataTable

        Dim MySqlException As MySql.Data.MySqlClient.MySqlException : MySqlException = Nothing
        Dim jetnet_ID As Integer = 0
        Dim client_ID As Integer = 0
        Dim andQ As String
        atemptable.Columns.Add("source")
        atemptable.Columns.Add("lnote_id")
        'atemptable.Columns.Add("amwant_listed_date")

        Dim amwant_listed_date As DataColumn = atemptable.Columns.Add("amwant_listed_date", Type.GetType("System.DateTime"))
        amwant_listed_date.AllowDBNull = True
        atemptable.Columns.Add("amwant_amod_id")
        atemptable.Columns.Add("amod_make_name")
        atemptable.Columns.Add("comp_name")
        atemptable.Columns.Add("comp_id")
        atemptable.Columns.Add("amod_model_name")
        atemptable.Columns.Add("amwant_notes")
        atemptable.Columns.Add("amwant_start_year")
        atemptable.Columns.Add("amwant_end_year")
        atemptable.Columns.Add("amwant_max_price")
        atemptable.Columns.Add("amwant_max_aftt")
        atemptable.Columns.Add("contact_first_name")
        atemptable.Columns.Add("contact_last_name")
        atemptable.Columns.Add("contact_id")
        atemptable.Columns.Add("amwant_id")

        If companySource = "CLIENT" Then
            client_ID = compID
            jetnet_ID = otherID
        Else
            jetnet_ID = compID
            client_ID = otherID
        End If

        If jetnet_ID <> 0 And client_ID = 0 Then
            subset = "J"
        End If
        If client_ID <> 0 And jetnet_ID = 0 Then
            subset = "C"
        End If

        HttpContext.Current.Session.Item("MasterWanted") = "" 'Whole Search
        HttpContext.Current.Session.Item("MasterAircraftWantedFrom") = "" 'Where only
        HttpContext.Current.Session.Item("MasterAircraftWantedWhere") = "" 'From Only

        Try
            If HttpContext.Current.Session.Item("localUser").crmEvo = True Then
                subset = "J"
            End If


            If subset = "J" Or subset = "JC" Then
                sql = ""
                sql = "select amwant_listed_date, amwant_id,amwant_amod_id, amod_make_name, view_aircraft_model_wanted.comp_name, view_aircraft_model_wanted.comp_id, amod_model_name, amwant_notes, amwant_start_year, amwant_end_year,"
                sql = sql & " amwant_max_price, amwant_max_aftt, contact_first_name, contact_last_name, contact_id, 'JETNET' AS source, comp_product_helicopter_flag, comp_product_business_flag, comp_product_commercial_flag"
                sql = sql & " FROM view_aircraft_model_wanted WITH(NOLOCK) INNER JOIN company WITH(NOLOCK) ON view_aircraft_model_wanted.comp_id = company.comp_id AND company.comp_journ_id = 0"
                sql = sql & "  "

                HttpContext.Current.Session.Item("MasterAircraftWantedFrom") = " FROM view_aircraft_model_wanted WITH(NOLOCK) INNER JOIN company WITH(NOLOCK) ON view_aircraft_model_wanted.comp_id = company.comp_id AND company.comp_journ_id = 0"

                sql = sql & " WHERE "

                sqlwhere = " amwant_journ_id = " & JournalID & " AND (amwant_verified_date IS NOT NULL) "


                If Trim(placed_by) = "" Or Trim(placed_by) = "All" Then

                ElseIf Trim(placed_by) = "End User" Then
                    sqlwhere = sqlwhere & " and view_aircraft_model_wanted.comp_business_type='EU' "
                ElseIf Trim(placed_by) = "Dealer" Then
                    sqlwhere = sqlwhere & " and view_aircraft_model_wanted.comp_business_type='DB' "
                End If


                If jetnet_ID <> 0 Then
                    If sqlwhere <> "" Then
                        andQ = " and "
                    Else
                        andQ = ""
                    End If
                    sqlwhere = sqlwhere & andQ & " amwant_comp_id = " & jetnet_ID
                End If


                If Not String.IsNullOrEmpty(WeightClass.Trim) Then
                    If Not String.IsNullOrEmpty(sqlwhere.Trim) Then
                        andQ = " and "
                    Else
                        andQ = ""
                    End If

                    If WeightClass.Contains(Constants.cValueSeperator) Then
                        sqlwhere += andQ + "EXISTS (SELECT amod_id FROM Aircraft_Model WHERE amod_weight_class IN ('" + WeightClass.Trim + "'))"
                    Else
                        sqlwhere += andQ + "EXISTS (SELECT amod_id FROM Aircraft_Model WHERE amod_weight_class = '" + WeightClass.Trim + "')"
                    End If
                End If

                If Not String.IsNullOrEmpty(ManufacturerName.Trim) Then
                    If Not String.IsNullOrEmpty(sqlwhere.Trim) Then
                        andQ = " and "
                    Else
                        andQ = ""
                    End If

                    If ManufacturerName.Contains(Constants.cValueSeperator) Then
                        sqlwhere += andQ + "EXISTS (SELECT amod_id FROM Aircraft_Model WHERE amod_manufacturer_common_name IN ('" + ManufacturerName.Trim + "'))"
                    Else
                        sqlwhere += andQ + "EXISTS (SELECT amod_id FROM Aircraft_Model WHERE amod_manufacturer_common_name = '" + ManufacturerName.Trim + "')"
                    End If

                End If

                If Not String.IsNullOrEmpty(AcSize.Trim) Then
                    If Not String.IsNullOrEmpty(sqlwhere.Trim) Then
                        andQ = " and "
                    Else
                        andQ = ""
                    End If

                    If AcSize.Contains(Constants.cValueSeperator) Then
                        sqlwhere += andQ + "EXISTS (SELECT amod_id FROM Aircraft_Model WHERE (amod_jniq_size IN ('" + AcSize.Trim + "'))"
                    Else
                        sqlwhere += andQ + "EXISTS (SELECT amod_id FROM Aircraft_Model WHERE (amod_jniq_size = '" + AcSize.Trim + "')"
                    End If

                End If

                If WantedIDs <> "" Then
                    If sqlwhere <> "" Then
                        andQ = " and "
                    Else
                        andQ = ""
                    End If
                    sqlwhere = sqlwhere & andQ & " (amwant_id in (" & WantedIDs & ")) "
                End If

                If company_name <> "" Then
                    If sqlwhere <> "" Then
                        andQ = " and "
                    Else
                        andQ = ""
                    End If
                    sqlwhere = sqlwhere & andQ & " (company.comp_name like '%" & company_name & "%') "
                End If

                If start_date <> "" Then
                    If sqlwhere <> "" Then
                        andQ = " and "
                    Else
                        andQ = ""
                    End If
                    sqlwhere = sqlwhere & andQ & " (amwant_listed_date >= '" & start_date & "') "
                End If

                If end_date <> "" Then
                    If sqlwhere <> "" Then
                        andQ = " and "
                    Else
                        andQ = ""
                    End If
                    sqlwhere = sqlwhere & andQ & " (amwant_listed_date <= '" & end_date & "')"
                End If


                If amodID <> "" Then
                    If sqlwhere <> "" Then
                        sqlwhere += " and "
                    End If
                    sqlwhere += " amwant_amod_id in (" & amodID & ") "
                Else
                    If ModelType <> "" Then
                        If sqlwhere <> "" Then
                            sqlwhere += " and "
                        End If
                        sqlwhere += " amod_type_code in (" & ModelType & ")"
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

                ''Setting up the filtering
                Dim HoldClsSubscription As New crmSubscriptionClass

                HoldClsSubscription.crmAerodexFlag = HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag
                HoldClsSubscription.crmBusiness_Flag = Business
                HoldClsSubscription.crmCommercial_Flag = Commercial
                HoldClsSubscription.crmHelicopter_Flag = Helicopter
                HoldClsSubscription.crmJets_Flag = HttpContext.Current.Session.Item("localSubscription").crmJets_Flag
                HoldClsSubscription.crmTurboprops = HttpContext.Current.Session.Item("localSubscription").crmTurboprops
                HoldClsSubscription.crmExecutive_Flag = HttpContext.Current.Session.Item("localSubscription").crmExecutive_Flag


                sqlwhere += " " + clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HoldClsSubscription, False, False)

                sqlwhere += " " + commonEvo.MakeCompanyProductCodeClause(HttpContext.Current.Session.Item("localPreferences"), False)


                HttpContext.Current.Session.Item("MasterAircraftWantedWhere") = sqlwhere

                sql = sql & sqlwhere & " order by "
                If PageSort = "" Then
                    sql = sql & " amwant_listed_date desc"
                Else
                    sql = sql & PageSort & " asc"
                End If

                SqlConn.ConnectionString = Session.Item("jetnetClientDatabase").ToString.Trim
                SqlConn.Open()
                SqlCommand.Connection = SqlConn
                SqlCommand.CommandType = CommandType.Text
                SqlCommand.CommandTimeout = 60

                SqlCommand.CommandText = sql

                'Setting the WHOLE QUERY in a session variable
                'This will be used on the Folder Maintenance Page to save the query:
                HttpContext.Current.Session.Item("MasterWanted") = sql
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Return_Wanted_Evo(ByVal compID As Long, ByVal companySource As String, ByVal otherID As Long, ByVal amodID As String, ByVal start_date As String, ByVal end_date As String, ByVal company_name As String, ByVal subset As String, JournalID As long) As DataTabl <em>Jetnet Side</em></b><br />" & sql

                SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

                Do While SqlReader.Read()

                    Dim newCustomersRow As DataRow = atemptable.NewRow()
                    newCustomersRow("source") = SqlReader.Item("source")
                    newCustomersRow("amwant_amod_id") = SqlReader.Item("amwant_amod_id")

                    newCustomersRow("amwant_listed_date") = SqlReader.Item("amwant_listed_date")
                    newCustomersRow("amod_make_name") = SqlReader.Item("amod_make_name")
                    newCustomersRow("lnote_id") = 0
                    newCustomersRow("comp_name") = SqlReader.Item("comp_name")
                    newCustomersRow("comp_id") = SqlReader.Item("comp_id")
                    newCustomersRow("amod_model_name") = SqlReader.Item("amod_model_name")
                    newCustomersRow("amwant_notes") = SqlReader.Item("amwant_notes")

                    newCustomersRow("amwant_start_year") = SqlReader.Item("amwant_start_year")
                    newCustomersRow("amwant_end_year") = SqlReader.Item("amwant_end_year")
                    newCustomersRow("amwant_max_price") = SqlReader.Item("amwant_max_price")
                    newCustomersRow("amwant_max_aftt") = SqlReader.Item("amwant_max_aftt")

                    newCustomersRow("contact_first_name") = SqlReader.Item("contact_first_name")
                    newCustomersRow("contact_last_name") = SqlReader.Item("contact_last_name")
                    newCustomersRow("contact_id") = SqlReader.Item("contact_id")
                    newCustomersRow("amwant_id") = SqlReader.Item("amwant_id")

                    atemptable.Rows.Add(newCustomersRow)
                    atemptable.AcceptChanges()

                Loop

            End If

            Return atemptable

        Catch ex As Exception
            Return_Wanted_Evo = Nothing
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Return_Wanted_Evo(ByVal compID As Long, ByVal companySource As String, ByVal otherID As Long, ByVal amodID As String, ByVal start_date As String, ByVal end_date As String, ByVal company_name As String, ByVal subset As String, JournalID As long): SQL VERSION " & ex.Message
        Finally
            SqlReader = Nothing
            SqlConn.Dispose()

            SqlConn.Close()

            SqlConn = Nothing

            SqlCommand.Dispose()

            SqlCommand = Nothing
        End Try
        atemptable = Nothing
    End Function

End Class