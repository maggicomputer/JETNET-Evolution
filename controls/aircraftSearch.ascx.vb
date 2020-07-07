Imports crmWebClient.clsGeneral
Partial Public Class aircraftSearch
  Inherits System.Web.UI.UserControl
  Public Event ChangedListing(ByVal sender As Object, ByVal Listing_Type As String)
  Public Event Searched_Me(ByVal ByValsubnode As Boolean, ByVal search_for As String, ByVal search_where As String, ByVal search_for_cbo As String, ByVal model_cbo As String, ByVal market_status_cbo As String, ByVal sort As String, ByVal sort_how As String, ByVal subset As String, ByVal airport_name As String, ByVal icao_code As String, ByVal iata_code As String, ByVal city As String, ByVal country As String, ByVal state As String, ByVal types_of_owners As String, ByVal on_lease As String, ByVal on_exclusive As String, ByVal year_to As String, ByVal year_from As String, ByVal search_field As String, ByVal lifecycle As String, ByVal ownership As String, ByVal CustomField1 As String, ByVal CustomField2 As String, ByVal CustomField3 As String, ByVal CustomField4 As String, ByVal CustomField5 As String, ByVal CustomField6 As String, ByVal CustomField7 As String, ByVal CustomField8 As String, ByVal CustomField9 As String, ByVal CustomField10 As String, ByVal NoteSearch As Integer, ByVal AircraftNoteDate As String, ByVal exclude As Boolean)
  Public Event check_changed(ByVal sender As Object)
  Dim aTempTable, aTempTable2, TempTable As New DataTable 'Data Tables used
  Dim error_string As String = ""

#Region "Functions"
#End Region
#Region "Custom Events"
  Private Sub search_button_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles search_button.Click
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    'Event that's handled on the Master Page.
    'Clicking the button, so clear the subfolder.
    masterPage.NameOfSubnode = ""

    Click_Search()
  End Sub
  'Private Sub search_for_cbo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles search_for_cbo.SelectedIndexChanged
  '    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
  '    Try
  '        'Event that's handled on the Master Page.
  '        RaiseEvent ChangedListing(e, search_for_cbo.SelectedValue)
  '    Catch ex As Exception
  '        error_string = "AircraftSearch.ascx.vb - search_for_cbo_SelectedIndexChanged() " & ex.Message
  '        masterPage.LogError(error_string)
  '    End Try
  'End Sub 
#End Region

  Public Sub Click_Search()
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Dim model_list As New ListBox
    ac_search_attention.Text = ""

    If Session.Item("localUser").crmEvo = True Then 'If an EVO user
      model_list = model
    Else
      If model_cbo.Visible = True Then
        model_list = model_cbo
      Else
        model_list = model
      End If
    End If


    Try
      'We need to add a small check in here. If you fill in any of the custom fields, it defaults to a client search.
      If custom_pref_text1.Text <> "" Or custom_pref_text2.Text <> "" Or custom_pref_text3.Text <> "" Or custom_pref_text4.Text <> "" Or custom_pref_text5.Text <> "" Or custom_pref_text6.Text <> "" Or custom_pref_text7.Text <> "" Or custom_pref_text8.Text <> "" Or custom_pref_text9.Text <> "" Or custom_pref_text10.Text <> "" Then
        subset.SelectedValue = "C"
      End If

      Dim models As String = ""
      For i = 0 To model_list.Items.Count - 1
        If model_list.Items(i).Selected Then
          If model_list.Items(i).Value <> "" Then
            models = models & "'" & model_list.Items(i).Value & "',"
          End If
        End If
      Next

      If models <> "" Then
        models = UCase(models.TrimEnd(","))
      End If


      Dim states As String = ""
      For i = 0 To state.Items.Count - 1
        If state.Items(i).Selected Then
          If state.Items(i).Value <> "" Then
            states = states & "'" & state.Items(i).Value & "',"
          End If
        End If
      Next

      If states <> "" Then
        states = UCase(states.TrimEnd(","))
      End If


      Dim countries As String = ""
      For i = 0 To country.Items.Count - 1
        If country.Items(i).Selected Then
          If country.Items(i).Value <> "" Then
            countries += "'" & country.Items(i).Value & "',"
          End If
        End If
      Next

      If countries <> "" Then
        countries = UCase(countries.TrimEnd(","))
      End If

      Dim sortstring As String = ""
      Dim sortby As Array = Split(sort_by_cbo.SelectedValue, ",")
      For x = 0 To UBound(sortby)
        sortstring = sortstring & sortby(x) & " " & sort_method_cbo.SelectedValue & ","
      Next

      If sortstring <> "" Then
        sortstring = UCase(sortstring.TrimEnd(","))
      End If

      If masterPage.IsSubNode = True Or search_for_txt.Text <> "" Or model_list.SelectedValue <> "" Or airport_name.Text <> "" Or iata_code.Text <> "" Or icao_code.Text <> "" Or city.Text <> "" Or countries <> "" Then
        ac_search_attention.Text = ""

        Session("search_aircraft") = Trim(clsGeneral.clsGeneral.StripChars(search_for_txt.Text, True)) & "@" & search_where.SelectedValue & "@" & 3 & "@" & models & "@" & market_status_cbo.SelectedValue & "@" & sort_by_cbo.SelectedValue & "@" & sort_method_cbo.SelectedValue & "@" & subset.SelectedValue & "@" & Trim(clsGeneral.clsGeneral.StripChars(airport_name.Text, True)) & "@" & Trim(clsGeneral.clsGeneral.StripChars(icao_code.Text, True)) & "@" & Trim(clsGeneral.clsGeneral.StripChars(iata_code.Text, True)) & "@" & Trim(clsGeneral.clsGeneral.StripChars(city.Text, True)) & "@" & clsGeneral.clsGeneral.StripChars(countries, True) & "@" & states & "@" & types_of_owners.SelectedValue & "@" & on_lease.SelectedValue & "@" & on_exclusive.SelectedValue & "@" & year_start.SelectedValue & "@" & year_end.SelectedValue & "@" & aftt.Checked & "@" & search_field.SelectedValue & "@" & ac_lifecycle_dropdown.SelectedValue & "@" & ac_ownership_type.SelectedValue & "@" & clsGeneral.clsGeneral.StripChars(custom_pref_text1.Text, True) & "@" & clsGeneral.clsGeneral.StripChars(custom_pref_text2.Text, True) & "@" & clsGeneral.clsGeneral.StripChars(custom_pref_text3.Text, True) & "@" & clsGeneral.clsGeneral.StripChars(custom_pref_text4.Text, True) & "@" & clsGeneral.clsGeneral.StripChars(custom_pref_text5.Text, True) & "@" & clsGeneral.clsGeneral.StripChars(custom_pref_text6.Text, True) & "@" & clsGeneral.clsGeneral.StripChars(custom_pref_text7.Text, True) & "@" & clsGeneral.clsGeneral.StripChars(custom_pref_text8.Text, True) & "@" & clsGeneral.clsGeneral.StripChars(custom_pref_text9.Text, True) & "@" & clsGeneral.clsGeneral.StripChars(custom_pref_text10.Text, True) & "@" & clsGeneral.clsGeneral.StripChars(aircraftNotes.SelectedValue, True) & "@" & clsGeneral.clsGeneral.StripChars(notesDate.Text, True) & "@" & MergeList.Checked
        RaiseEvent Searched_Me(IIf(masterPage.Subnode_Method <> "A", IIf(masterPage.IsSubNode, True, False), False), Trim(clsGeneral.clsGeneral.StripChars(search_for_txt.Text, True)), search_where.SelectedValue, 3, models, market_status_cbo.SelectedValue, sortstring, sort_method_cbo.SelectedValue, subset.SelectedValue, Trim(clsGeneral.clsGeneral.StripChars(airport_name.Text, True)), Trim(clsGeneral.clsGeneral.StripChars(icao_code.Text, True)), Trim(clsGeneral.clsGeneral.StripChars(iata_code.Text, True)), Trim(clsGeneral.clsGeneral.StripChars(city.Text, True)), countries, states, types_of_owners.SelectedValue, on_lease.SelectedValue, on_exclusive.SelectedValue, year_start.SelectedValue, year_end.SelectedValue, search_field.SelectedValue, ac_lifecycle_dropdown.SelectedValue, ac_ownership_type.SelectedValue, clsGeneral.clsGeneral.StripChars(custom_pref_text1.Text, True), clsGeneral.clsGeneral.StripChars(custom_pref_text2.Text, True), clsGeneral.clsGeneral.StripChars(custom_pref_text3.Text, True), clsGeneral.clsGeneral.StripChars(custom_pref_text4.Text, True), clsGeneral.clsGeneral.StripChars(custom_pref_text5.Text, True), clsGeneral.clsGeneral.StripChars(custom_pref_text6.Text, True), clsGeneral.clsGeneral.StripChars(custom_pref_text7.Text, True), clsGeneral.clsGeneral.StripChars(custom_pref_text8.Text, True), clsGeneral.clsGeneral.StripChars(custom_pref_text9.Text, True), clsGeneral.clsGeneral.StripChars(custom_pref_text10.Text, True), clsGeneral.clsGeneral.StripChars(aircraftNotes.SelectedValue, True), clsGeneral.clsGeneral.StripChars(notesDate.Text, True), MergeList.Checked)
        masterPage.PerformDatabaseAction = False
      Else
        ac_search_attention.Text = "<p align='center'>Please use more detailed search parameters.</p>"
      End If

      masterPage.Write_Javascript_Out()


      If aircraftNotes.SelectedValue = "0" Then
        placerHold.Attributes.Remove("class")
        aircraftNotesDateToggle.Attributes.Add("class", "display_none")
      Else
        aircraftNotesDateToggle.Attributes.Remove("class")
        placerHold.Attributes.Add("class", "display_none")
      End If
      PanelCollapseEx.Collapsed = True
      PanelCollapseEx.ClientState = "true"

    Catch ex As Exception
      error_string = "AircraftSearch.ascx.vb - search_button_click() " & ex.Message
      masterPage.LogError(error_string)
    End Try
  End Sub
  Private Sub Advanced_Search_Fill_In_Click()
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Try
      'toggle custom fields categories on
      search_pnl.Height = 345
      advanced_search_categories.Visible = True
      'fill them up
      ToggleCustomFields(masterPage)

      base.Visible = True
      base2.Visible = True
      basecountry.Visible = True
      'base3.Visible = True
      base1.Visible = True
      adv_search.Visible = False
      country.Items.Add(New ListItem("ALL", ""))

      aTempTable = masterPage.aclsData_Temp.Get_Jetnet_Country()
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          For Each r As DataRow In aTempTable.Rows
            If Not IsDBNull(r("clicountry_name")) And Trim(r("clicountry_name")) <> "" Then
              country.Items.Add(New ListItem(CStr(r("clicountry_name")), CStr(r("clicountry_name"))))
            End If
          Next
        End If
      Else
        If masterPage.aclsData_Temp.class_error <> "" Then
          error_string = masterPage.aclsData_Temp.class_error
          masterPage.LogError("listing.aspx.vb - fill_CBO() - " & error_string)
        End If
        masterPage.display_error()
      End If


      'country.SelectedValue = ""
      aTempTable = masterPage.aclsData_Temp.Get_Jetnet_State()
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          For Each r As DataRow In aTempTable.Rows
            state.Items.Add(New ListItem(CStr(r("client_state")), CStr(r("client_state_abbr"))))
          Next
        End If
      Else
        If masterPage.aclsData_Temp.class_error <> "" Then
          error_string = masterPage.aclsData_Temp.class_error
          masterPage.LogError("listing.aspx.vb - fill_CBO() - " & error_string)
        End If
        masterPage.display_error()
      End If
    Catch ex As Exception
      error_string = "aircraftSearch.ascx.vb - adv_search_click() " & ex.Message
      masterPage.LogError(error_string)
    End Try
  End Sub

  Private Sub adv_search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles adv_search.Click
    Try
      Advanced_Search_Fill_In_Click()
    Catch ex As Exception
      Dim masterPage As main_site = DirectCast(Page.Master, main_site)
      error_string = "aircraftSearch.ascx.vb - adv_search_click() " & ex.Message
      masterPage.LogError(error_string)
    End Try
  End Sub
#Region "Events dealing with Advanced Airport Search"
  Private Sub countryLB_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles country.SelectedIndexChanged
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Try
      Dim ShowStates As Boolean = False

      For i = 0 To country.Items.Count - 1
        If country.Items(i).Selected Then
          If country.Items(i).Value = "United States" Then
            ShowStates = True
          End If
        End If
      Next

      If ShowStates Then
        state.Visible = True
        state_text.Visible = True
        search_pnl.Height = FigureOutSearchPanelHeight() + 60
      Else
        state_text.Visible = False
        state.Visible = False
        state.SelectedValue = ""
        search_pnl.Height = FigureOutSearchPanelHeight() + 60
      End If

    Catch ex As Exception
      error_string = "AircraftSearch.ascx.vb - country_SelectedIndexChanged() " & ex.Message
      masterPage.LogError(error_string)
    End Try
  End Sub
  'Private Sub country_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles country.SelectedIndexChanged
  '  Dim masterPage As main_site = DirectCast(Page.Master, main_site)
  '  Try
  '    If country.SelectedValue = "United States" Then
  '      state.Visible = True
  '      state_text.Visible = True
  '      search_pnl.Height = FigureOutSearchPanelHeight() + 60

  '    Else
  '      state_text.Visible = False
  '      state.Visible = False
  '      state.SelectedValue = ""
  '      search_pnl.Height = FigureOutSearchPanelHeight() + 10
  '    End If
  '  Catch ex As Exception
  '    error_string = "AircraftSearch.ascx.vb - country_SelectedIndexChanged() " & ex.Message
  '    masterPage.LogError(error_string)
  '  End Try
  'End Sub
#End Region

  Private Sub default_models_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles default_models.CheckedChanged

    RaiseEvent check_changed(Me)
  End Sub

  Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

    If Not Page.IsPostBack Then
      If Not String.IsNullOrEmpty(Trim(Request("runMarket"))) Then
        If Trim(Request("runMarket")) = "true" Then
          If Not String.IsNullOrEmpty(Trim(Request("jetnetModelID"))) Then
            Dim masterPage As main_site = DirectCast(Page.Master, main_site)
            masterPage.FromTypeOfListing = 3 'added to retain listing ID that we came from on a search if the type is changed
            masterPage.TypeOfListing = 3
            masterPage.IsSubNode = False
            masterPage.NameOfSubnode = "Aircraft"
            masterPage.SubNodeOfListing = 3
            masterPage.Subnode_Method = ""
          End If
        End If
      End If
    End If

    Dim SwapPageScript As StringBuilder = New StringBuilder()
    If Not Page.ClientScript.IsClientScriptBlockRegistered("Toggle") Then
      SwapPageScript.Append("<script type=""text/javascript"">")

      SwapPageScript.Append(vbCrLf & "function toggleNotesDateToggle(aircraftSearchDropdown) {")
      SwapPageScript.Append(vbCrLf & " if (aircraftSearchDropdown.value != 0) { ")
      SwapPageScript.Append(vbCrLf & "$(""#" & aircraftNotesDateToggle.ClientID & """).removeClass(""display_none"");")
      SwapPageScript.Append(vbCrLf & "$(""#" & placerHold.ClientID & """).prop(""class"", ""display_none"");")
      SwapPageScript.Append(vbCrLf & "} else {")
      SwapPageScript.Append(vbCrLf & "$(""#" & aircraftNotesDateToggle.ClientID & """).prop(""class"", ""display_none"");")
      SwapPageScript.Append(vbCrLf & "$(""#" & placerHold.ClientID & """).removeClass(""display_none"");")
      SwapPageScript.Append(vbCrLf & "}")
      SwapPageScript.Append(vbCrLf & "}")
      SwapPageScript.Append("</script>")
      System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Toggle", SwapPageScript.ToString, False)

    End If

    search_where.Attributes.Add("onChange", "javascript:createCookie('searchWhere', this.options[this.selectedIndex].innerHTML, 356);")

  End Sub
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
    If Me.Visible = True Then
      If Session.Item("crmUserLogon") = True Then
        Dim masterPage As main_site = DirectCast(Page.Master, main_site)

        ' If Not Page.IsPostBack Then
        If Trim(Request("clear")) = "true" Then

          masterPage.FromTypeOfListing = 3 'added to retain listing ID that we came from on a search if the type is changed
          masterPage.TypeOfListing = 3
          masterPage.IsSubNode = False
          masterPage.NameOfSubnode = "Aircraft"
          masterPage.SubNodeOfListing = 3
          masterPage.Subnode_Method = ""

          masterPage.Table_List = Nothing
          Session("Results") = Nothing
          Session("search_company") = Nothing
          Session("search_contact") = Nothing
          Session("search_aircraft") = Nothing
          Session("search_transaction") = Nothing
          Response.Redirect("/listing_air.aspx")
        End If
        'End If


        'Small addition to the aftt checkbox.
        'We started saving this in a simple cookie
        'If the cookie doesn't exist
        'It just defaults to checked.
        Dim _afttCookies As HttpCookie = Request.Cookies("aftt")
        If Not IsNothing(_afttCookies) Then
          If _afttCookies.Value = True Then
            aftt.Checked = True
          Else
            aftt.Checked = False
          End If
        Else
          aftt.Checked = True
        End If


        search_for_txt.Focus()
        Dim TypeDataTable As New DataTable
        Dim TypeDataHold As New DataTable
        Dim strFunds As String = ""
        Dim default_vis As Boolean = True
        Dim research As Boolean = False


        If Session.Item("localUser").crmEvo <> True Then 'If an EVO user
          If Not IsNothing(Trim(Request("redo_search"))) Then
            If Trim(Request("redo_search")) = "true" Then
              research = True
            End If
          End If
        End If


        'Querying the Database and keeping this information so we only have to do it once. 
        If Not Page.IsPostBack Then
          'we're going to set the default
          'of the relationship dropdown here. 
          If Not IsDBNull(Session.Item("localUser").crmUserAircraftRelationship) Then
            If Session.Item("localUser").crmUserAircraftRelationship <> "" Then
              types_of_owners.SelectedIndex = types_of_owners.Items.IndexOf(types_of_owners.Items.FindByText(Session.Item("localUser").crmUserAircraftRelationship))
            End If
          End If

          If Session.Item("localUser").crmEvo = True Then 'If an EVO user
            'This bit of code basically ensures that you can run the CRM in evo mode, basically ignoring the crm client side.
            subset_label.Text = "" 'On Lease?:"
            'lease_text.Visible = False
            subset.Visible = False

            clsGeneral.clsGeneral.Getting_Type_Listbox_Set(TypeDataTable, model_type, masterPage, TempTable, TypeDataHold, type)
          Else
            If Session.Item("localUser").crmEvo <> True Then 'If an EVO user
              If masterPage.IsSubNode = False Then 'This model box only runs if a user isn't looking at a folder. If they are looking at a folder, the model box fills up below, after we get the cfolder data from the database.
                Try
                  clsGeneral.clsGeneral.populate_models(model_cbo, IIf(research = True, False, True), Me, Nothing, masterPage, IIf(research = True, False, True))
                Catch ex As Exception
                  error_string = "aircraft - fill_CBO() Model Dropdown Filling - " & ex.Message
                  masterPage.LogError(error_string)
                End Try
              End If
            End If

            'This bit of code checks to see if the value of the model box is blank or if we're running a back to listing search.
            'If it's a back to listing search, the default model is not checked.
            If model_cbo.SelectedValue <> "" Or research = True Or masterPage.IsSubNode = False Then
              default_models.Checked = True
            Else
              default_models.Checked = False
            End If


            'Toggling the model swap to show what's applicable for CRM.
            model_cbo.Visible = True
            model_evo_swap.Visible = False
            model_type.Visible = False
            default_models.Visible = True


          End If
        End If
        If Not Page.IsPostBack Then


          ' If Session.Item("localUser").crmEvo = True Then 'If an EVO user
          'Just a simple check to get rid of all the checkboxes we can't have.
          If Session.Item("localSubscription").crmHelicopter_Flag <> True Then
            model_type.Items.Remove(model_type.Items.FindByValue("Helicopter"))
          End If
          If Session.Item("localSubscription").crmBusiness_Flag <> True Then
            model_type.Items.Remove(model_type.Items.FindByValue("Business"))
          End If
          If Session.Item("localSubscription").crmCommercial_Flag <> True Then
            model_type.Items.Remove(model_type.Items.FindByValue("Commercial"))
          End If
          'End If

          If Not IsNothing(Request.Item("removed")) Then
            If Not String.IsNullOrEmpty(Request.Item("removed").ToString) Then
              If Request.Item("removed").Trim = "true" Then
                ac_search_attention.Text = "<p align='center'>Your Aircraft has been removed.</p>"
              End If
            End If
          End If
          If Not IsNothing(Request.Item("ac_not_exist")) Then
            If Not String.IsNullOrEmpty(Request.Item("ac_not_exist").ToString) Then
              If Request.Item("ac_not_exist").Trim = "true" Then
                ac_search_attention.Text = "<p align='center'>This Aircraft no longer exists.</p>"
              End If
            End If
          End If

          'Filling the market status drop down.
          market_status_cbo.Items.Add(New ListItem("All", ""))
          market_status_cbo.Items.Add(New ListItem("Deal", "Deal"))
          market_status_cbo.Items.Add(New ListItem("For Sale", "For Sale"))
          market_status_cbo.Items.Add(New ListItem("For Sale/Best Deal", "For Sale/Best Deal"))
          market_status_cbo.Items.Add(New ListItem("For Sale Exclusive", "For Sale Exclusive"))
          market_status_cbo.Items.Add(New ListItem("For Sale/Lease", "For Sale/Lease"))
          market_status_cbo.Items.Add(New ListItem("For Sale/Off Market", "For Sale/Off Market"))
          market_status_cbo.Items.Add(New ListItem("For Sale/Possible", "For Sale/Possible"))
          market_status_cbo.Items.Add(New ListItem("For Sale/Trade", "For Sale/Trade"))
          market_status_cbo.Items.Add(New ListItem("For Sale/Share", "For Sale/Share"))
          market_status_cbo.Items.Add(New ListItem("Lease", "Lease"))
          market_status_cbo.Items.Add(New ListItem("Not For Sale", "Not For Sale"))
          market_status_cbo.Items.Add(New ListItem("Other", "Other"))
          market_status_cbo.Items.Add(New ListItem("Sale Pending", "Sale Pending"))
          market_status_cbo.Items.Add(New ListItem("Unconfirmed", "Unconfirmed"))

          masterPage.special_sort_by_cbo.Items.Add(New ListItem("Make/Model/Ser#", "amod_make_name, amod_model_name, ac_ser_nbr_sort"))
          sort_by_cbo.Items.Add(New ListItem("Make/Model/Ser#", "amod_make_name, amod_model_name, ac_ser_nbr_sort"))
          masterPage.special_sort_by_cbo.Items.Add(New ListItem("AFTT", "ac_airframe_tot_hrs"))
          sort_by_cbo.Items.Add(New ListItem("AFTT", "ac_airframe_tot_hrs"))
          masterPage.special_sort_by_cbo.Items.Add(New ListItem("Model", "amod_make_name, amod_model_name"))
          sort_by_cbo.Items.Add(New ListItem("Model", "amod_make_name, amod_model_name"))
          masterPage.special_sort_by_cbo.Items.Add(New ListItem("Year", "ac_year_mfr"))
          sort_by_cbo.Items.Add(New ListItem("Year", "ac_year_mfr"))
          masterPage.special_sort_by_cbo.Items.Add(New ListItem("Serial Number", "ac_ser_nbr_sort"))
          sort_by_cbo.Items.Add(New ListItem("Serial Number", "ac_ser_nbr_sort"))
          masterPage.special_sort_by_cbo.Items.Add(New ListItem("Reg Number", "ac_reg_nbr"))
          sort_by_cbo.Items.Add(New ListItem("Reg Number", "ac_reg_nbr"))
          masterPage.special_sort_by_cbo.Items.Add(New ListItem("Company Name", "comp_name"))
          sort_by_cbo.Items.Add(New ListItem("Company Name", "comp_name"))
          masterPage.special_sort_by_cbo.Items.Add(New ListItem("Date Listed", "ac_date_listed"))
          sort_by_cbo.Items.Add(New ListItem("Date Listed", "ac_date_listed"))
          masterPage.special_sort_by_cbo.Items.Add(New ListItem("Asking Price", "ac_asking_price_sort"))
          sort_by_cbo.Items.Add(New ListItem("Asking Price", "ac_asking_price_sort"))
          clsGeneral.clsGeneral.Year_Range_DropDownFill(year_start, 1975, 2015)
          clsGeneral.clsGeneral.Year_Range_DropDownFill(year_end, 1975, 2015)


          If Session.Item("localSubscription").crmAerodexFlag = True Then
            market_status_cbo.Visible = False
            market_status_lbl.Text = ""
            exclusive.Visible = True
            on_exclusive.Visible = True
            exclusive_label.Visible = True
            exclusive_cell_label.Visible = False
            on_lease.Visible = False
            year_from_label.Text = "Year From: "
            market_status_cell.Visible = False
            market_status_dropdown_cell.Visible = False
            types_of_owners_cell.ColumnSpan = "3"
            types_of_owners_cell.HorizontalAlign = HorizontalAlign.Left
            types_of_owners.Width = Unit.Pixel(350)
            subset_label.Width = Unit.Pixel(80)
          End If

          search_where.Items.Clear()
          search_where.Items.Add(New ListItem("Begins With", "1"))

          search_where.Items.Add(New ListItem("Anywhere", "2"))
          search_where.Items.Add(New ListItem("Equals", "3"))

          Dim _searchWhereCookies As HttpCookie = Request.Cookies("searchWhere")
          If Not IsNothing(_searchWhereCookies) Then
            If _searchWhereCookies.Value = "Anywhere" Then
              search_where.SelectedValue = "2"
            ElseIf _searchWhereCookies.Value = "Equals" Then
              search_where.SelectedValue = "3"
            Else
              search_where.SelectedValue = "1"
            End If
          Else
            search_where.SelectedValue = "1"
          End If

          If Not IsNothing(Session("search_aircraft")) Then
            If Not String.IsNullOrEmpty(Session("search_aircraft").ToString) And research = True Then
              default_vis = False
            End If
          End If


          'Let's try to refill up the aircraft folders.
          Dim FolderTable As New DataTable
          Dim cfolderData As String = ""
          Dim AlreadyRanSearch As Boolean = False
          If masterPage.IsSubNode = True Then
            NewSearch.Visible = True
            default_models.Checked = False
            cfolderData = clsGeneral.clsGeneral.ReturnCfolderData(masterPage, FolderTable)
            'This is going to populate the model box for those who are viewing a folder.
            clsGeneral.clsGeneral.populate_models(model_cbo, False, Me, Nothing, masterPage, False)
            If cfolderData = "" Then
              'This means that this is an index folder, so we raise the event differently
              'This used to be called in the listing.aspx page, however with the addition of static folders
              'I opted to move it here so it would more closely mirror the evo.net way of running folders.
              masterPage.Fill_Aircraft(True, "", 2, "", "", "", sort_by_cbo.SelectedValue, sort_method_cbo.SelectedValue, "", "", "JC", "", "", "", "", "", "", Session.Item("types_of_owners"), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 0, "", False)
              PanelCollapseEx.Collapsed = True
              PanelCollapseEx.ClientState = "true"
            ElseIf cfolderData <> "" Then
              If InStr(cfolderData, "state") > 0 Or InStr(cfolderData, "airport") > 0 Or InStr(cfolderData, "iata") > 0 Or InStr(cfolderData, "icao") > 0 Or InStr(cfolderData, "country") > 0 Or InStr(cfolderData, "city") > 0 Or InStr(cfolderData, "custom") > 0 Then
                adv_search_Click(adv_search, System.EventArgs.Empty)
              End If


              'Fills up the applicable folder Information pulled from the cfolder data field
              DisplayFunctions.FillUpFolderInformation(New Table, New Label, cfolderData, New Label, FolderTable, True, False, False, False, False, search_pnl, New BulletedList, Nothing, Nothing, Nothing)


              'Automatically running the search
              Click_Search()
              AlreadyRanSearch = True

              masterPage.IsSubNode = False
              masterPage.SubNodeOfListing = 0
              masterPage.NameOfSubnode = ""
            End If
          End If


          If research = True Then
            If Not IsNothing(Session("search_aircraft")) Then
              If Not String.IsNullOrEmpty(Session("search_aircraft").ToString) Then
                If AlreadyRanSearch = False Then
                  Last_Search() 'fill last search and perform
                End If
                default_vis = False
                default_models.Checked = False
              End If
            End If
          End If

          If Not Page.IsPostBack Then
            If Not String.IsNullOrEmpty(Trim(Request("runMarket"))) Then
              If Trim(Request("runMarket")) = "true" Then
                If Not String.IsNullOrEmpty(Trim(Request("jetnetModelID"))) Then
                  model_cbo.Items.Clear()
                  default_models.Checked = False
                  clsGeneral.clsGeneral.populate_models(model_cbo, False, Me, Nothing, masterPage, False)

                  For j As Integer = 0 To model_cbo.Items.Count() - 1
                    'We need to split the listbox value and compare it to the split value 0
                    Dim models As Array = Split(model_cbo.Items(j).Value, "|")
                    If models(0) = Trim(Request("jetnetModelID")) Then
                      model_cbo.Items(j).Selected = True
                    Else
                      model_cbo.Items(j).Selected = False
                    End If
                  Next

                End If
                If Not String.IsNullOrEmpty(Trim(Request("forSale"))) Then
                  market_status_cbo.SelectedValue = "For Sale"
                End If

                If Not String.IsNullOrEmpty(Trim(Request("inOp"))) Then
                  ac_lifecycle_dropdown.SelectedValue = "3"
                End If

                If Not String.IsNullOrEmpty(Trim(Request("exclusive"))) Then
                  on_lease.SelectedValue = "Y"
                End If

                Click_Search()
              End If
            End If
          End If


      End If

    End If
    End If
  End Sub

  Private Sub Last_Search()
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Try

      Dim ac_search As Array = Split(Session("search_aircraft"), "@")

      ac_search(3) = Replace(ac_search(3), "'", "")
      ac_search(13) = Replace(ac_search(13), "'", "")
      Dim models As Array = Split(ac_search(3), ",")
      Dim states As Array = Split(ac_search(13), ",")
      Dim countryArray As Array = Split(ac_search(12), ",")

      'Custom fields are between 23-32
      If ac_search(8) <> "" Or ac_search(9) <> "" Or ac_search(10) <> "" Or ac_search(11) <> "" Or ac_search(12) <> "" Or ac_search(23) <> "" Or ac_search(24) <> "" Or ac_search(25) <> "" Or ac_search(26) <> "" Or ac_search(27) <> "" Or ac_search(28) <> "" Or ac_search(29) <> "" Or ac_search(30) <> "" Or ac_search(31) <> "" Or ac_search(32) <> "" Then
        'set the advanced search clicky option.
        Advanced_Search_Fill_In_Click()
      End If

      'aircraft notes search is 33
      If Not String.IsNullOrEmpty(ac_search(33)) Then
        If IsNumeric(ac_search(33)) Then
          aircraftNotes.SelectedValue = ac_search(33)
        End If
      End If

      'Aircraft notes date is 34
      If Not String.IsNullOrEmpty(ac_search(34)) Then
        notesDate.Text = ac_search(34)
      End If

      'Exclude Jetnet is 35
      If Not String.IsNullOrEmpty(ac_search(35)) Then
        MergeList.Checked = IIf(ac_search(35) = True, True, False)
      End If

      If ac_search(13) <> "" Then
        state.Visible = True
        state_text.Visible = True
        search_pnl.Height = 305
      End If

      'Search text
      search_for_txt.Text = ac_search(0)
      'Search where (everywhere, begins with.)
      search_where.SelectedValue = ac_search(1)
      'search_for_cbo.SelectedValue = ac_search(2)


      'refil the models 
      For x = 0 To UBound(models)
        '  Response.Write(models(x) & "<br />")
        For j As Integer = 0 To model_cbo.Items.Count() - 1
          If model_cbo.Items(0).Selected = True Then
            model_cbo.Items(0).Selected = False
          End If
          Dim mode As String = UCase(model_cbo.Items(j).Value)
          Dim et As String = UCase(models(x))
          If UCase(model_cbo.Items(j).Value) = UCase(models(x)) Then
            model_cbo.Items(j).Selected = True
          Else
          End If
        Next
      Next

      'refill the states
      For x = 0 To UBound(states)
        For j As Integer = 0 To state.Items.Count() - 1
          Dim mode As String = UCase(state.Items(j).Value)
          Dim et As String = UCase(states(x))
          If UCase(state.Items(j).Value) = UCase(states(x)) Then
            state.Items(j).Selected = True
          Else
          End If
        Next
      Next

      'Market Status
      market_status_cbo.SelectedValue = ac_search(4)
      'Sort Method, Ascending, Descending.
      sort_method_cbo.SelectedValue = ac_search(6)
      'Sort by, meaning the field names we're sorting by.
      sort_by_cbo.SelectedValue = ac_search(5)
      'Subset, the datasubset we're searching.
      subset.SelectedValue = ac_search(7)
      'Airport name
      airport_name.Text = ac_search(8)
      'Icao code
      icao_code.Text = ac_search(9)
      'Iata code
      iata_code.Text = ac_search(10)
      'City
      city.Text = ac_search(11)
      'Country

      'country.SelectedValue = ac_search(12)
      'refill the Country
      country.SelectedValue = -1
      For x = 0 To UBound(countryArray)
        For j As Integer = 0 To country.Items.Count() - 1
          Dim mode As String = UCase(country.Items(j).Value)
          Dim et As String = UCase(countryArray(x))
          If UCase(country.Items(j).Value) = UCase(countryArray(x)) Then
            country.Items(j).Selected = True
          Else
          End If
        Next
      Next


      'Different owners to list.
      types_of_owners.SelectedValue = ac_search(14)
      'On lease
      on_lease.SelectedValue = ac_search(15)
      'On exclusive
      on_exclusive.SelectedValue = ac_search(16)
      'Year range start
      year_start.SelectedValue = ac_search(17)
      'Year range end
      year_end.SelectedValue = ac_search(18)
      'AC lifecycle.
      ac_lifecycle_dropdown.SelectedValue = ac_search(21)
      'Ownership Type
      ac_ownership_type.SelectedValue = ac_search(22)


      'Advanced search fields
      custom_pref_text1.Text = ac_search(23)
      custom_pref_text2.Text = ac_search(24)
      custom_pref_text3.Text = ac_search(25)
      custom_pref_text4.Text = ac_search(26)
      custom_pref_text5.Text = ac_search(27)
      custom_pref_text6.Text = ac_search(28)
      custom_pref_text7.Text = ac_search(29)
      custom_pref_text8.Text = ac_search(30)
      custom_pref_text9.Text = ac_search(31)
      custom_pref_text10.Text = ac_search(32)

      'AFTT on/off
      Try
        aftt.Checked = ac_search(19)
      Catch
        aftt.Checked = False
      End Try

      'raise search event!
      Click_Search()
    Catch ex As Exception
      error_string = "aircraftSearch.ascx.vb - click search() " & ex.Message
      masterPage.LogError(error_string)
    End Try
  End Sub
  Private Sub type_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles type.SelectedIndexChanged
    clsGeneral.clsGeneral.Type_Selected_Index_Changed(make, type, Page.IsPostBack)
  End Sub


  Private Sub make_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles make.SelectedIndexChanged
    clsGeneral.clsGeneral.Make_Selected_Index_Changed(model, make, type)
  End Sub



  Private Sub model_type_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles model_type.SelectedIndexChanged
    clsGeneral.clsGeneral.Model_Type_Selected_Index_Changed(type, model_type)
  End Sub

  Private Function FigureOutSearchPanelHeight() As Double
    Dim currentHeight As Double = 280
    If custom_pref_name1.Visible = True Then
      currentHeight += 30
    End If
    If custom_pref_name3.Visible = True Then
      currentHeight += 30
    End If
    If custom_pref_name5.Visible = True Then
      currentHeight += 30
    End If
    If custom_pref_name7.Visible = True Then
      currentHeight += 30
    End If
    If custom_pref_name9.Visible = True Then
      currentHeight += 30
    End If


    Return currentHeight
  End Function

  Private Sub ToggleCustomFields(ByVal masterpage As main_site)
    Try
      Dim currentHeight As Double = search_pnl.Height.Value
      aTempTable = masterpage.aclsData_Temp.Get_Client_Preferences()
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          For Each r As DataRow In aTempTable.Rows
            If Not IsDBNull(r("clipref_ac_custom_1_use")) Then
              If r("clipref_ac_custom_1_use") = "Y" Then
                currentHeight += 30
                custom_pref_name1.Visible = True
                custom_pref_text1.Visible = True
                custom_pref_name1.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_1")), r("clipref_ac_custom_1"), "")) & ":"
              Else
                custom_pref_name1.Visible = False
                custom_pref_text1.Visible = False
                custom_pref_name1.Text = ""
              End If
            End If

            If Not IsDBNull(r("clipref_ac_custom_2_use")) Then
              If r("clipref_ac_custom_2_use") = "Y" Then
                custom_pref_name2.Visible = True
                custom_pref_text2.Visible = True
                custom_pref_name2.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_2")), r("clipref_ac_custom_2"), "")) & ":"
              Else
                custom_pref_name2.Visible = False
                custom_pref_text2.Visible = False
                custom_pref_name2.Text = ""
              End If
            End If

            If Not IsDBNull(r("clipref_ac_custom_3_use")) Then
              If r("clipref_ac_custom_3_use") = "Y" Then
                currentHeight += 30
                custom_pref_name3.Visible = True
                custom_pref_text3.Visible = True
                custom_pref_name3.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_3")), r("clipref_ac_custom_3"), "")) & ":"
              Else
                custom_pref_name3.Visible = False
                custom_pref_text3.Visible = False
                custom_pref_name3.Text = ""
              End If
            End If

            If Not IsDBNull(r("clipref_ac_custom_4_use")) Then
              If r("clipref_ac_custom_4_use") = "Y" Then
                custom_pref_name4.Visible = True
                custom_pref_text4.Visible = True
                custom_pref_name4.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_4")), r("clipref_ac_custom_4"), "")) & ":"
              Else
                custom_pref_name4.Visible = False
                custom_pref_text4.Visible = False
                custom_pref_name4.Text = ""
              End If
            End If


            If Not IsDBNull(r("clipref_ac_custom_5_use")) Then
              If r("clipref_ac_custom_5_use") = "Y" Then
                currentHeight += 30
                custom_pref_name5.Visible = True
                custom_pref_text5.Visible = True
                custom_pref_name5.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_5")), r("clipref_ac_custom_5"), "")) & ":"
              Else
                custom_pref_name5.Visible = False
                custom_pref_text5.Visible = False
                custom_pref_name5.Text = ""
              End If
            End If


            If Not IsDBNull(r("clipref_ac_custom_6_use")) Then
              If r("clipref_ac_custom_6_use") = "Y" Then
                custom_pref_name6.Visible = True
                custom_pref_text6.Visible = True
                custom_pref_name6.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_6")), r("clipref_ac_custom_6"), "")) & ":"
              Else
                custom_pref_name6.Visible = False
                custom_pref_text6.Visible = False
                custom_pref_name6.Text = ""
              End If
            End If

            If Not IsDBNull(r("clipref_ac_custom_7_use")) Then
              If r("clipref_ac_custom_7_use") = "Y" Then
                currentHeight += 30
                custom_pref_name7.Visible = True
                custom_pref_text7.Visible = True
                custom_pref_name7.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_7")), r("clipref_ac_custom_7"), "")) & ":"
              Else
                custom_pref_name7.Visible = False
                custom_pref_text7.Visible = False
                custom_pref_name7.Text = ""
              End If
            End If


            If Not IsDBNull(r("clipref_ac_custom_8_use")) Then
              If r("clipref_ac_custom_8_use") = "Y" Then
                custom_pref_name8.Visible = True
                custom_pref_text8.Visible = True
                custom_pref_name8.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_8")), r("clipref_ac_custom_8"), "")) & ":"
              Else
                custom_pref_name8.Visible = False
                custom_pref_text8.Visible = False
                custom_pref_name8.Text = ""
              End If
            End If

            If Not IsDBNull(r("clipref_ac_custom_9_use")) Then
              If r("clipref_ac_custom_9_use") = "Y" Then
                currentHeight += 30
                custom_pref_name9.Visible = True
                custom_pref_text9.Visible = True
                custom_pref_name9.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_9")), r("clipref_ac_custom_9"), "")) & ":"
              Else
                custom_pref_name9.Visible = False
                custom_pref_text9.Visible = False
                custom_pref_name9.Text = ""
              End If
            End If

            If Not IsDBNull(r("clipref_ac_custom_10_use")) Then
              If r("clipref_ac_custom_10_use") = "Y" Then

                custom_pref_name10.Visible = True
                custom_pref_text10.Visible = True
                custom_pref_name10.Text = CStr(IIf(Not IsDBNull(r("clipref_ac_custom_10")), r("clipref_ac_custom_10"), "")) & ":"
              Else
                custom_pref_name10.Visible = False
                custom_pref_text10.Visible = False
                custom_pref_name10.Text = ""
              End If
            End If
          Next
        End If
      Else
        If masterpage.aclsData_Temp.class_error <> "" Then
          error_string = masterpage.aclsData_Temp.class_error
          masterpage.LogError("AircraftSearch.ascx.vb - ToggleCustomFields() - " & error_string)
        End If
        masterpage.display_error()
      End If

      If search_pnl.Height.Value <> currentHeight Then
        currentHeight += 10 'buffer for custom fields header.
        search_pnl.Height = currentHeight
      Else
        advanced_search_categories.Visible = False 'toggle custom fields off
      End If

    Catch ex As Exception
      error_string = "AircraftSearch.ascx.vb - ToggleCustomFields() " & ex.Message
      masterpage.LogError(error_string)
    End Try
  End Sub




End Class