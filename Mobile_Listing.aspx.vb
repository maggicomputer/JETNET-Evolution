Partial Public Class Mobile_Listing
  Inherits System.Web.UI.Page

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    'AddHandler Master.Search_Click, AddressOf New_Search_Click
    'types, types.


    Select Case Master.TypeOfListing
      Case 1
        company_search.Visible = True
        company_results.Visible = True
        contact_search.Visible = False
        transaction_search.Visible = False
        market_search.Visible = False

        If Master.SubNodeOfListing <> 0 Then
          company_search_button_Click(Me, Nothing)
          company_search.Visible = False
        End If

        If Not IsNothing(Request.Item("redo_search")) Then
          If Request.Item("redo_search").ToString = "true" Then
            company_adv_search_Click(Me, Nothing)
            Dim search_pnl As New Panel
            Dim special_view As New CheckBox
            clsGeneral.clsGeneral.Company_Last_Search_Selection(company_search_for, company_subset, company_search_where, company_search_for, company_status_cbo, special_field_cbo, state, country, types_of_owners, show_all, special_field_txt, special_view, search_pnl, company_phone_number, Master, Nothing)
            company_search_button_Click(Me, Nothing)
          End If
        End If
      Case 8
        company_search.Visible = False
        company_results.Visible = False
        contact_search.Visible = False
        transaction_search.Visible = True
        market_search.Visible = False
        If Not Page.IsPostBack Then
          clsGeneral.clsGeneral.populate_models(transaction_model, False, aircraft_search, Master, Nothing, False)
          transaction_year_start.Items.Add(New ListItem("All", ""))
          clsGeneral.clsGeneral.Transaction_Contact_Type(relationships, Master, Nothing)
          clsGeneral.clsGeneral.Transaction_Category(transaction_trans_type_cbo, Master, Nothing)
          transaction_year_end.Items.Add(New ListItem("All", ""))
          transaction_year_start.Items.Add(New ListItem("All", ""))
          If Not Page.IsPostBack Then
            transaction_trans_type_cbo.SelectedValue = "Full Sale"
          End If

          For i As Integer = 2015 To 1957 Step -1
            transaction_year_start.Items.Add(New ListItem(i, i))
            transaction_year_end.Items.Add(New ListItem(i, i))
          Next
          transaction_year_start.SelectedValue = ""
          transaction_year_end.SelectedValue = ""
        End If
      Case 10
        company_search.Visible = False
        company_results.Visible = False
        contact_search.Visible = False
        transaction_search.Visible = False
        market_search.Visible = True
        If Not Page.IsPostBack Then
          clsGeneral.clsGeneral.populate_models(market_model, False, market_search, Master, Nothing, False)
          clsGeneral.clsGeneral.Market_Categories(categories, market_types, Master.aclsData_Temp, "")
        End If
      Case 2
        contact_search.Visible = True
        company_results.Visible = False
        company_search.Visible = False
        transaction_search.Visible = False
        market_search.Visible = False
        If Not Page.IsPostBack Then
          contact_ordered_by.Items.Add(New ListItem("First Name, Last Name", "1"))
          contact_ordered_by.Items.Add(New ListItem("Company Name", "2"))
          contact_ordered_by.Items.Add(New ListItem("Last Name, First Name", "3"))
          contact_status_cbo.Items.Add(New ListItem("All", "B"))
          contact_status_cbo.Items.Add(New ListItem("Active", "Y"))
          contact_status_cbo.Items.Add(New ListItem("Inactive", "N"))
          contact_status_cbo.SelectedValue = "Y"
        End If
        If Master.SubNodeOfListing <> 0 Then
          contact_search_button_Click(Me, Nothing)
          contact_search.Visible = False
        End If
        If Not IsNothing(Request.Item("redo_search")) Then
          If Request.Item("redo_search").ToString = "true" Then
            Dim search_for As New DropDownList
            clsGeneral.clsGeneral.Contact_Last_Search_Selection(contact_first_name, contact_last_name, contact_search_where, search_for, comp_name_txt, contact_status_cbo, contact_ordered_by, contact_subset, Master, Nothing)
            contact_search_button_Click(Me, Nothing)

          End If
        End If
      Case 3
        contact_search.Visible = False
        company_results.Visible = False
        company_search.Visible = False
        aircraft_search.Visible = True
        transaction_search.Visible = False
        market_search.Visible = False
        If Not Page.IsPostBack Then
          clsGeneral.clsGeneral.populate_models(model_cbo, False, aircraft_search, Master, Nothing, False)
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


          ac_sort.Items.Add(New ListItem("Make/Model/Ser#", "amod_make_name, amod_model_name, ac_ser_nbr_sort"))
          ac_sort.Items.Add(New ListItem("AFTT", "ac_airframe_tot_hrs"))
          ac_sort.Items.Add(New ListItem("Model", "amod_make_name asc, amod_model_name"))
          ac_sort.Items.Add(New ListItem("Year", "ac_year_mfr"))
          ac_sort.Items.Add(New ListItem("Serial Number", "ac_ser_nbr_sort"))
          ac_sort.Items.Add(New ListItem("Reg Number", "ac_reg_nbr"))
          ac_sort.Items.Add(New ListItem("Owner", "contact_last_name"))
          year_start.Items.Add(New ListItem("All", ""))
          year_end.Items.Add(New ListItem("All", ""))
          For i As Integer = 2015 To 1957 Step -1
            year_start.Items.Add(New ListItem(i, i))
            year_end.Items.Add(New ListItem(i, i))
          Next
          year_start.SelectedValue = ""
          year_end.SelectedValue = ""

        End If
        If Master.SubNodeOfListing <> 0 Then
          aircraft_search_button_Click(Me, Nothing)
          aircraft_search.Visible = False
        End If
        If Not IsNothing(Request.Item("redo_search")) Then
          If Request.Item("redo_search").ToString = "true" Then
            clsGeneral.clsGeneral.Aircraft_Last_Search_Selection(ac_state, state_text, aircraft_search, aircraft_search_for, aircraft_search_where, New DropDownList, model_cbo, market_status_cbo, ac_sort, ac_sort, ac_subset, airport_name, icao_code, iata_code, city, ac_country, ac_types_of_owners, on_lease, on_exclusive, year_start, year_end, aftt, Master, Nothing)
            aircraft_search_button_Click(Me, Nothing)

          End If
        End If
      Case 6, 7, 4, 11, 12

        notes_search.Visible = True
        transaction_search.Visible = False
        market_search.Visible = False
        If Not Page.IsPostBack Then
          If Master.TypeOfListing <> 11 Then
            clsGeneral.clsGeneral.populate_models(notes_model, False, aircraft_search, Master, Nothing, False)
          Else
            models_row.Visible = False
          End If
          clsGeneral.clsGeneral.Fill_User_Dropdown(display_cbo, "Notes", Master, Nothing)
          Select Case Master.TypeOfListing
            Case 11
              clsGeneral.clsGeneral.Fill_Opportunity_Category(notes_cat, Master.aTempTable, Master.aclsData_Temp)
              order_bo.Items.Add(New ListItem("Entry Date", "lnote_entry_date"))
              order_bo.Items.Add(New ListItem("Cash Value", "lnote_cash_value"))
            Case 4
              clsGeneral.clsGeneral.Fill_Note_Category(notes_cat, "N", Master, Nothing)
              order_bo.Items.Add(New ListItem("Date Scheduled", "lnote_schedule_date"))
              order_bo.Items.Add(New ListItem("Priority", "lnote_clipri_id"))
            Case 6
              clsGeneral.clsGeneral.Fill_Note_Category(notes_cat, "N", Master, Nothing)
              order_bo.Items.Add(New ListItem("Entry Date", "lnote_entry_date"))
              order_bo.Items.Add(New ListItem("Note Text", "lnote_note"))
            Case 7
              clsGeneral.clsGeneral.Fill_Note_Category(notes_cat, "Y", Master, Nothing)
              order_bo.Items.Add(New ListItem("Entry Date", "lnote_entry_date"))
              order_bo.Items.Add(New ListItem("Note Text", "lnote_note"))
            Case 12
              wanted_hide.Visible = False
              wanted_hide2.Visible = False
              wanted_hide3.Visible = False
          End Select
        End If
      Case Else
        Response.Redirect("sandbox.aspx")

    End Select
    Session.Item("FromTypeOfListing") = Master.TypeOfListing
    If Not IsNothing(Request.Item("show")) Then
      If Not String.IsNullOrEmpty(Request.Item("show").ToString) Then
        If Request.Item("show") = "folder" Then
          New_Folder_Search_Click()
        End If
      End If
    End If


  End Sub
#Region "Contact Search"
  Private Sub contact_search_button_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles contact_search_button.Click
    Try
      Dim contact_link As Label = Master.FindControl("contacts_link")
      contact_link.BackColor = Drawing.Color.FromName("#8cc7dd")
      general_search_error.Text = ""
      Session("search_contact") = Trim(clsGeneral.clsGeneral.StripChars(contact_first_name.Text, True)) & "@" & clsGeneral.clsGeneral.StripChars(contact_last_name.Text, True) & "@" & contact_search_where.Text & "@@" & comp_name_txt.Text & "@" & contact_status_cbo.SelectedValue & "@" & contact_ordered_by.SelectedValue & "@" & contact_subset.SelectedValue & "@"

      Master.aTempTable = clsGeneral.clsGeneral.Fill_Contact(Master, Nothing, contact_first_name.Text, contact_last_name.Text, comp_name_txt.Text, contact_status_cbo.SelectedValue, contact_search_where.SelectedValue, contact_ordered_by.SelectedValue, contact_subset.SelectedValue, Master.SubNodeOfListing, "", "")
      If Not IsNothing(Master.aTempTable) Then
        If Master.aTempTable.Rows.Count > 0 Then
          Record_Count(Master.aTempTable.Rows.Count)
          contact_list.DataSource = Master.aTempTable
          contact_list.DataBind()
          contact_search.Visible = False
        Else
          No_Records(contact_list)
        End If
      Else

        If Master.aclsData_Temp.class_error <> "" Then
          Master.error_string = "mobile_listing.aspx.vb - contact_search_button_CLICK() - " & Master.aclsData_Temp.class_error

          clsGeneral.clsGeneral.LogError(Master.error_string, Master.aclsData_Temp)
        End If
      End If
    Catch ex As Exception
      Master.error_string = "mobile_listing.aspx.vb - contact_search_button_CLICK() - " & ex.Message
      clsGeneral.clsGeneral.LogError(Master.error_string, Master.aclsData_Temp)
    End Try
  End Sub
#End Region

  Public Sub No_Records(ByVal list As DataGrid)
    list.DataSource = Nothing
    list.DataBind()
    Record_Count(0)
    search_results_error.Text = "<p align='center'>Your search returned 0 results.</p>"
  End Sub
#Region "Notes Search"
  Private Sub notes_search_button_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles notes_search_button.Click
    Try
      search_results_error.Text = ""
      general_search_error.Text = ""

      Master.aTempTable = clsGeneral.clsGeneral.Fill_Notes_Actions_Documents(notes_start_date.Text, notes_end_date.Text, notes_search_txt.Text, notes_search_where.SelectedValue, notes_model, notes_cat.SelectedValue, display_cbo.SelectedValue, Master, Nothing, "", "", "", 0, 0, "", False, False, 3)
      If Not IsNothing(Master.aTempTable) Then
        If Master.aTempTable.Rows.Count > 0 Then
          Record_Count(Master.aTempTable.Rows.Count)
          If Master.TypeOfListing = 7 Then
            documents_list.DataSource = Master.aTempTable
            documents_list.DataBind()
          ElseIf Master.TypeOfListing = 4 Then
            action_list.DataSource = Master.aTempTable
            action_list.DataBind()
          ElseIf Master.TypeOfListing = 11 Then
            opportunity_list.DataSource = Master.aTempTable
            opportunity_list.DataBind()
          Else
            notes_list.DataSource = Master.aTempTable
            notes_list.DataBind()
          End If


          notes_search.Visible = False
        Else
          No_Records(notes_list)
        End If
      Else
        If Master.aclsData_Temp.class_error <> "" Then
          Master.error_string = "mobile_listing.aspx.vb - notes_search_button_CLICK() - " & Master.aclsData_Temp.class_error

          clsGeneral.clsGeneral.LogError(Master.error_string, Master.aclsData_Temp)
        End If
      End If

    Catch ex As Exception
      Master.error_string = "mobile_listing.aspx.vb - notes_search_button_CLICK() - " & ex.Message
      clsGeneral.clsGeneral.LogError(Master.error_string, Master.aclsData_Temp)
    End Try
  End Sub
#End Region
  Public Sub Record_Count(ByVal x As Integer)
    If Not IsNothing(Master.FindControl("record_count")) Then
      Dim record_count As Label = Master.FindControl("record_count")
      record_count.Text = "" & x & " Record(s) Returned"
      record_count.Visible = True
    End If

    If Not IsNothing(Master.FindControl("search_new")) Then
      Master.FindControl("search_new").Visible = True
      'Master.FindControl("new_search").= Drawing.Color.FromName("#8cc7dd")
    End If
  End Sub
#Region "Events dealing with Company Search"
  Private Sub company_search_button_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles company_search_button.Click
    Try
      general_search_error.Text = ""
      general_search_error.Text = ""
      Dim companies_link As Label = Master.FindControl("companies_link")
      companies_link.BackColor = Drawing.Color.FromName("#8cc7dd")

      If company_search_for.Text <> "" Or Master.SubNodeOfListing <> 0 Or company_phone_number.Text <> "" Then

        Dim subnode As Boolean = False
        If Master.SubNodeOfListing <> 0 Then
          subnode = True
        End If
        If Not IsNothing(Master.FindControl("new_search")) Then
          Master.FindControl("new_search").Visible = True
        End If
        general_search_error.Text = ""

        Session("search_company") = "@" & Trim(clsGeneral.clsGeneral.StripChars(company_search_for.Text, True)) & "@" & company_search_where.Text & "@@" & company_status_cbo.Text & "@" & company_subset.SelectedValue & "@" & country.SelectedValue & "@" & types_of_owners.SelectedValue & "@@FALSE@@FALSE@" & IIf(special_field_txt.Text <> "", True, False) & "@@" & Trim(clsGeneral.clsGeneral.StripChars(company_phone_number.Text, True))

        'figure out ids for phone #
        Dim phone As String = Trim(clsGeneral.clsGeneral.StripChars(company_phone_number.Text, True))
        Dim jetnet_IDS As String = ""
        Dim client_IDS As String = ""
        If phone <> "" Then
          Try
            Master.aTempTable = Master.aclsData_Temp.SearchPhoneNumbers(phone)
            '' check the state of the DataTable
            If Not IsNothing(Master.aTempTable) Then
              If Master.aTempTable.Rows.Count > 0 Then
                For Each r As DataRow In Master.aTempTable.Rows
                  If r("source") = "CLIENT" Then
                    jetnet_IDS = jetnet_IDS & r("pnum_comp_id") & ","
                  Else
                    client_IDS = client_IDS & r("pnum_comp_id") & ","
                  End If
                Next
              Else
              End If
            Else
              If Master.aclsData_Temp.class_error <> "" Then
                Master.error_string = "mobile_listing.aspx.vb - company search button - last search click() - " & Master.aclsData_Temp.class_error
                clsGeneral.clsGeneral.LogError(Master.error_string, Master.aclsData_Temp)
              End If
            End If
            If jetnet_IDS <> "" Then
              jetnet_IDS = UCase(jetnet_IDS.TrimEnd(","))
              subnode = True
            End If
            If client_IDS <> "" Then
              client_IDS = UCase(client_IDS.TrimEnd(","))
              subnode = True
            End If
          Catch ex As Exception
            Master.error_string = "companysearch.ascx.vb - last search click() - " & ex.Message
            clsGeneral.clsGeneral.LogError(Master.error_string, Master.aclsData_Temp)
          End Try
        End If

        Master.aTempTable = clsGeneral.clsGeneral.Fill_Company(subnode, "" & clsGeneral.clsGeneral.Get_Name_Search_String(company_search_for.Text) & "", company_search_where.Text, "", company_status_cbo.SelectedValue, company_subset.SelectedValue, country.SelectedValue, types_of_owners.SelectedValue, "", "", "", False, "", Master, Nothing, Master.SubNodeOfListing, state, client_IDS, jetnet_IDS, "", False)


        If Not IsNothing(Master.aTempTable) Then
          If Master.aTempTable.Rows.Count > 0 Then
            Record_Count(Master.aTempTable.Rows.Count)
            company_list.DataSource = Master.aTempTable
            company_list.DataBind()
            company_search.Visible = False
          Else
            No_Records(company_list)
          End If
        Else
          If Master.aclsData_Temp.class_error <> "" Then
            Master.error_string = "mobile_listing.aspx.vb - company_search_button_CLICK() - " & Master.aclsData_Temp.class_error
            clsGeneral.clsGeneral.LogError(Master.error_string, Master.aclsData_Temp)
          End If
        End If
      Else
        general_search_error.Text = "<p align='center'>Please include a General Search Term in order to perform a company search.</p>"
      End If
    Catch ex As Exception
      Master.error_string = "mobile_listing.aspx.vb - company_search_button_CLICK() - " & ex.Message
      clsGeneral.clsGeneral.LogError(Master.error_string, Master.aclsData_Temp)
    End Try
  End Sub
  Private Sub country_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ac_country.SelectedIndexChanged, country.SelectedIndexChanged

    Try
      If sender.SelectedValue = "United States" Then
        state.Visible = True
        ac_state.Visible = True
        state_text.Visible = True
      Else
        state.Visible = False
        ac_state.Visible = False
        ac_state.SelectedValue = ""
        state.SelectedValue = ""
      End If
    Catch ex As Exception
      Master.error_string = "CompanySearch.ascx.vb - country_SelectedIndexChanged() " & ex.Message
      clsGeneral.clsGeneral.LogError(Master.error_string, Master.aclsData_Temp)
    End Try
  End Sub
  Private Sub subset_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles company_subset.SelectedIndexChanged
    Try
      If company_subset.SelectedValue = "C" Then
        show_all.Visible = True
      Else
        show_all.Visible = False
      End If
    Catch ex As Exception
      Master.error_string = "CompanySearch.ascx.vb - subset_SelectedIndexChanged() " & ex.Message
      clsGeneral.clsGeneral.LogError(Master.error_string, Master.aclsData_Temp)
    End Try
  End Sub
  Private Sub Select_Special_Field(ByVal select_string As String)
    Try
      If select_string <> "" Then
        special_field_txt.Visible = True
      Else
        special_field_txt.Visible = False
      End If
    Catch ex As Exception
      Master.error_string = "CompanySearch.ascx.vb - special_field_cbo_SelectedIndexChanged() " & ex.Message
      clsGeneral.clsGeneral.LogError(Master.error_string, Master.aclsData_Temp)
    End Try
  End Sub
  Private Sub special_field_cbo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles special_field_cbo.SelectedIndexChanged
    Select_Special_Field(special_field_cbo.SelectedValue)
  End Sub

  Public Sub New_Folder_Search_Click()
    aircraft_search.Visible = False
    contact_search.Visible = False
    company_search.Visible = False
    company_folders.Visible = True

    If Not IsNothing(Master.FindControl("search_new")) Then
      Master.FindControl("search_new").Visible = True
      'Master.FindControl("new_search").= Drawing.Color.FromName("#8cc7dd")
    End If
    If Not IsNothing(Master.FindControl("folder_search")) Then
      Master.FindControl("folder_search").Visible = False
      'Master.FindControl("new_search").= Drawing.Color.FromName("#8cc7dd")
    End If
  End Sub
  Private Sub company_adv_search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles company_adv_search.Click
    activity_view.Visible = True
    location_search.Visible = True
    'subset_search.Visible = True
    fields.Visible = True
    location_search.Visible = True
    types_search.Visible = True
    company_adv_search.Visible = False
    clsGeneral.clsGeneral.Populate_Country(country, Master, Nothing)
    clsGeneral.clsGeneral.Populate_State(state, Master, Nothing)
    clsGeneral.clsGeneral.Populate_Company_Category(special_field_cbo, Master, Nothing)
  End Sub
#End Region

#Region "Events dealing with Aircraft Search"
  Function DisplayAFTT(ByVal val As String) As String
    DisplayAFTT = ""
    Dim aftt As CheckBox = aircraft_search.FindControl("aftt")
    If aftt.Checked = True Then
      DisplayAFTT = val
    Else
      DisplayAFTT = ""
    End If
  End Function
  Private Sub Advanced_Search_Fill_In_Click()
    Try
      base.Visible = True
      base2.Visible = True
      base3.Visible = True
      base1.Visible = True
      base4.Visible = True
      base5.Visible = True
      aircraft_adv_search.Visible = False
      Master.aTempTable = Master.aclsData_Temp.Get_Jetnet_Country()
      clsGeneral.clsGeneral.Populate_Country(ac_country, Master, Nothing)
      clsGeneral.clsGeneral.Populate_State(ac_state, Master, Nothing)

    Catch ex As Exception
      Master.error_string = "mobile_listing.aspx.vb - adv_search_click() " & ex.Message
      clsGeneral.clsGeneral.LogError(Master.error_string, Master.aclsData_Temp)
    End Try
  End Sub

  Private Sub adv_search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles aircraft_adv_search.Click
    Try
      Advanced_Search_Fill_In_Click()
    Catch ex As Exception
      Master.error_string = "mobile_listing.aspx.vb - adv_search_click() " & ex.Message
      clsGeneral.clsGeneral.LogError(Master.error_string, Master.aclsData_Temp)
    End Try
  End Sub
  Dim count As Integer = 0
  Public color As String = "container_grid"
  Dim linked As New Label
  Dim lab As New Label
  Dim cont As New Label
  Dim but As New ImageButton
  Dim fly As New OboutInc.Flyout2.Flyout
  Dim container As New Panel
  Private Sub Results_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles trans_list.ItemDataBound

    'If Not IsNothing(e.Item.Cells(23)) Then
    '    'e.Item.Cells(23).Text = "Relationships"
    'End If

    Dim text_string As String = ""
    Dim text_string2 As String = ""
    Dim text_string3 As String = ""
    Dim id As String() = Split("", "|")
    ''Response.Write(e.Item.Cells(2).Text & "<br />")
    Dim act_name As String() = Split("", "|")
    Dim act_name_id As String() = Split("", "|")
    Dim perc As String() = Split("", "|")
    Dim cont_id As String() = Split("", "|")
    Dim td As New TableCell
    Dim maxrow As Integer = 0
    Dim display As Integer = 0
    Dim rowadd As Integer = 0
    If Master.TypeOfListing = 8 Then
      rowadd = 15
      maxrow = 32
      id = Split(e.Item.Cells(0).Text, "|")
      act_name = Split(e.Item.Cells(2).Text, "|")
      act_name_id = Split(e.Item.Cells(32).Text, "|")
      perc = Split("", "|")
      cont_id = Split(e.Item.Cells(1).Text, "|")
      display = 1 'transaction
    ElseIf Master.TypeOfListing = 3 Then
      rowadd = 7
      maxrow = 26
      id = Split(e.Item.Cells(1).Text, "|")
      act_name = Split(e.Item.Cells(8).Text, "|")
      perc = Split(e.Item.Cells(9).Text, "|")
      cont_id = Split(e.Item.Cells(3).Text, "|")
      display = 2 'aircraft
    End If


    but.ImageUrl = "~/images/magnify.png"
    but.OnClientClick = "return false;"

    Dim text As New Label
    Dim fly_text As String = ""


    If InStr(UCase(Request.ServerVariables("SCRIPT_NAME").ToString()), "MOBILE_LISTING.ASPX") > 0 Or InStr(UCase(Request.ServerVariables("SCRIPT_NAME").ToString()), "LISTING_TRANSACTION.ASPX") > 0 Then

      If Not IsNothing(e.Item.Cells(maxrow)) Then

        If Trim(e.Item.Cells(3).Text) <> "&nbsp;" Then
          For j = 0 To UBound(id)

            'account sorting
            Dim run_through As Boolean = True
            Dim show_broker As Boolean = False
            If display = 2 Then
              'If Session.Item("types_of_owners") = "" Then
              '    Session.Item("types_of_owners") = "all"
              'End If
              Select Case Session.Item("types_of_owners")
                Case "all"
                  Select Case act_name(j)
                    Case "Previous Owner", "Fractional Owner", "Owner", "Co-Owner", "Program Holder", "Exclusive Broker"
                      run_through = True
                    Case Else
                      run_through = False
                  End Select

                Case "whole"
                  Select Case act_name(j)
                    Case "Owner", "Previous Owner", "Exclusive Broker"
                      run_through = True
                    Case Else
                      run_through = False
                  End Select
                Case "operators"
                  Select Case act_name(j)
                    Case "Aircraft Management Company", "Charter Company", "Flight Department", "Hangar", "Lessee", "Managing Company", "Operator", "Program Manager", "Sublesee", "Exclusive Broker"
                      run_through = True
                    Case Else
                      run_through = False
                  End Select
                Case Else
                  show_broker = True
                  run_through = True
              End Select
            ElseIf display = 1 Then

              Dim ar As String() = Split(Session.Item("transaction_owners"), ",")
              run_through = False


              For t = 0 To UBound(ar)
                If run_through = False Then
                  If ar(t) = act_name_id(j) Then
                    run_through = True
                  Else
                    run_through = False
                  End If
                End If
              Next


            End If

            If cont_id(j) = "" Then
              cont_id(j) = 0
            End If
            If id(j) = "" Then
              id(j) = 0
            End If
            Dim add_me As Boolean = True
            If show_broker = False And act_name(j) = "Exclusive Broker" Then
              add_me = False
            End If


            Dim r As String = "JETNET"
            'If r = "JETNET" Then

            If run_through = True Then
              Dim ac As String = e.Item.Cells(4).Text
              'If ac = "10427" Or ac = "10426" Then
              '    ac = "here!!!!!!"
              'End If
              '3 is transaction, 2 is aircraft
              If (e.Item.Cells(2).Text = "JETNET" Or e.Item.Cells(2).Text = "CLIENT") Or (e.Item.Cells(3).Text = "JETNET" Or e.Item.Cells(3).Text = "CLIENT") Then
                'counter = counter + 1
                'Response.Write(counter & "!!!!!")
                If id(j) <> 0 Then

                  Dim comp_name As Array = Split("", "|")
                  Dim comp_address As Array = Split("", "|")
                  Dim comp_address2 As Array = Split("", "|")
                  Dim comp_city As Array = Split("", "|")
                  Dim comp_state As Array = Split("", "|")
                  Dim comp_country As Array = Split("", "|")
                  Dim comp_zip_code As Array = Split("", "|")
                  Dim comp_email_address As Array = Split("", "|")
                  Dim comp_web_address As Array = Split("", "|")

                  Dim contact_first_name As Array = Split("", "|")
                  Dim contact_last_name As Array = Split("", "|")
                  Dim contact_middle_initial As Array = Split("", "|")
                  Dim contact_title As Array = Split("", "|")
                  Dim contact_preferred_name As Array = Split("", "|")
                  Dim contact_notes As Array = Split("", "|")
                  Dim contact_email_address As Array = Split("", "|")
                  Dim contact_type_id As Array = Split("", "|")
                  Dim comp_source As Array = Split("", "|")
                  Dim client_exists = False
                  Dim source As String = ""

                  If display = 2 Then
                    'ac starts at 20
                    comp_name = Split(e.Item.Cells(10).Text, "|")
                    comp_address = Split(e.Item.Cells(11).Text, "|")
                    comp_address2 = Split(e.Item.Cells(12).Text, "|")
                    comp_city = Split(e.Item.Cells(13).Text, "|")
                    comp_state = Split(e.Item.Cells(14).Text, "|")
                    comp_country = Split(e.Item.Cells(15).Text, "|")
                    comp_zip_code = Split(e.Item.Cells(16).Text, "|")
                    comp_email_address = Split(e.Item.Cells(17).Text, "|")
                    comp_web_address = Split(e.Item.Cells(18).Text, "|")



                    contact_first_name = Split(e.Item.Cells(19).Text, "|")
                    contact_last_name = Split(e.Item.Cells(20).Text, "|")
                    contact_middle_initial = Split(e.Item.Cells(21).Text, "|")
                    contact_title = Split(e.Item.Cells(22).Text, "|")
                    contact_preferred_name = Split(e.Item.Cells(23).Text, "|")
                    contact_notes = Split(e.Item.Cells(24).Text, "|")
                    contact_email_address = Split(e.Item.Cells(25).Text, "|")


                    comp_source = Split(e.Item.Cells(26).Text, "|")
                    If InStr(e.Item.Cells(26).Text, "CLIENT") > 0 Then
                      client_exists = True
                    End If
                  Else
                    'trans starts 16
                    comp_name = Split(e.Item.Cells(16).Text, "|")
                    comp_address = Split(e.Item.Cells(17).Text, "|")
                    comp_address2 = Split(e.Item.Cells(18).Text, "|")
                    comp_city = Split(e.Item.Cells(19).Text, "|")
                    comp_state = Split(e.Item.Cells(20).Text, "|")
                    comp_country = Split(e.Item.Cells(21).Text, "|")
                    comp_zip_code = Split(e.Item.Cells(22).Text, "|")
                    comp_email_address = Split(e.Item.Cells(23).Text, "|")
                    comp_web_address = Split(e.Item.Cells(24).Text, "|")

                    contact_first_name = Split(e.Item.Cells(25).Text, "|")
                    contact_last_name = Split(e.Item.Cells(26).Text, "|")
                    contact_middle_initial = Split(e.Item.Cells(27).Text, "|")
                    contact_title = Split(e.Item.Cells(28).Text, "|")
                    contact_preferred_name = Split(e.Item.Cells(29).Text, "|")
                    contact_notes = Split(e.Item.Cells(30).Text, "|")
                    contact_email_address = Split(e.Item.Cells(31).Text, "|")
                    contact_type_id = Split(e.Item.Cells(32).Text, "|")
                  End If
                  If comp_name(j) <> "" Then
                    Dim address_string As String = ""
                    Dim lng_address_string As String = ""
                    Dim phone_text As String = ""
                    Dim contact_phone_text As String = ""
                    Dim font_color As String = ""
                    fly = New OboutInc.Flyout2.Flyout
                    linked = New Label
                    lab = New Label
                    but = New ImageButton
                    address_string = ""
                    text = New Label
                    cont = New Label
                    container = New Panel
                    'container.BorderColor = Drawing.Color.Red
                    'container.BorderWidth = 1
                    ' Response.Write(comp_name(j) & "1<br /><br />")

                    'linked.CommandName = "comp_details_from_ac"
                    'linked.ID = "details_view_com" & j & CInt(id(j))
                    'AddHandler linked.Click, AddressOf dispDetails_link
                    'linked.CommandArgument = CInt(id(j)) & "|" & e.Item.Cells(3).Text

                    text_string3 = comp_name(j)
                    If display = 2 Then
                      If comp_source(j) = "JETNET" Then
                        If color = "container_grid" Then
                          color = "container_grid_alt"
                        Else
                          color = "container_grid"
                        End If
                        font_color = "#023657"
                        source = "JETNET"
                      ElseIf comp_source(j) = "CJETNET" Then
                        If color = "container_grid" Then
                          color = "container_grid_alt"
                        Else
                          color = "container_grid"
                        End If
                        font_color = "#023657"
                        client_exists = True
                        source = "CLIENT"
                      ElseIf comp_source(j) = "JCLIENT" Then
                        If color = "container_grid_client" Then
                          color = "container_grid_alt_client"
                        Else
                          color = "container_grid_client"
                        End If
                        client_exists = True
                        source = "CLIENT"
                        font_color = "#7a3733"
                      ElseIf comp_source(j) = "CLIENT" Then
                        If color = "container_grid_client" Then
                          color = "container_grid_alt_client"
                        Else
                          color = "container_grid_client"
                        End If
                        source = "CLIENT"
                        font_color = "#7a3733"
                      End If
                    Else

                      If color = "container_grid" Then
                        color = "container_grid_alt"
                      Else
                        color = "container_grid"
                      End If
                      font_color = "#023657"
                    End If

                    lng_address_string = ""
                    lng_address_string = lng_address_string & "<strong style='font-size:14px;color:#" & font_color & ";'>" & comp_name(j) & "</strong><br />"
                    If comp_address(j) <> "" Then
                      lng_address_string = lng_address_string & comp_address(j) & "<br />"
                    End If
                    If comp_address2(j) <> "" Then
                      lng_address_string = lng_address_string & " " & comp_address2(j) & "<br />"
                    End If
                    If comp_city(j) <> "" Then
                      address_string = address_string & comp_city(j) & ","
                      lng_address_string = lng_address_string & comp_city(j) & ","
                    End If
                    If comp_state(j) <> "" Then
                      address_string = address_string & " " & comp_state(j)
                      lng_address_string = lng_address_string & " " & comp_state(j) & "<br />"
                    End If
                    If comp_zip_code(j) <> "" Then
                      lng_address_string = lng_address_string & " " & comp_zip_code(j) & "<br />"
                    End If
                    If comp_country(j) <> "" Then
                      address_string = address_string & " " & comp_country(j)
                      lng_address_string = lng_address_string & " " & comp_country(j) & "<br />"
                    End If
                    If comp_email_address(j) <> "" Then
                      lng_address_string = lng_address_string & "<br /><a href='mailto:" & comp_email_address(j) & "'>" & comp_email_address(j) & "</a>"
                    End If
                    If comp_web_address(j) <> "" Then
                      If InStr(comp_web_address(j), "http://") = 0 Then
                        lng_address_string = lng_address_string & "<br /><a href='http://" & comp_web_address(j) & "' target='_new'>" & comp_web_address(j) & "</a>"
                      Else
                        lng_address_string = lng_address_string & "<br /><a href='" & comp_web_address(j) & "' target='_new'>" & comp_web_address(j) & "</a>"
                      End If
                    End If
                    'linked.CommandArgument = CInt(id(j)) & "|" & e.Item.Cells(3).Text

                    If client_exists = True Then
                      comp_source(j) = "JETNET"
                    End If

                    container.CssClass = color
                    If display = 2 Then
                      linked.Text = "<span style='font-size:10px;'><a href='mobile_details.aspx?comp_ID=" & CInt(id(j)) & "&source=" & source & "&type=1'>" & text_string3 & " (<em>" & act_name(j) & clsGeneral.clsGeneral.showpercent(perc(j), act_name(j)) & "</em>)</a></span>"
                    Else
                      linked.Text = "<span style='font-size:10px;'><a href='mobile_details.aspx?comp_ID=" & CInt(id(j)) & "&source=" & e.Item.Cells(3).Text & "&type=1'>" & text_string3 & " (<em>" & act_name(j) & "</em>)</a></span>"
                    End If
                    text_string2 = "<span style='font-size:9px;'><i>" & address_string & "</i></span>"
                    lab.Text = "<br />" & text_string2
                    If display = 2 Then
                      cont.Text = "<br clear='all' /><span style='font-size:10px;'><a href='mobile_details.aspx?comp_ID=" & CInt(id(j)) & "&contact_ID=" & CInt(cont_id(j)) & "&source=" & source & "&type=1'>" & contact_first_name(j) & " " & contact_last_name(j) & "</a></span>"
                    Else
                      cont.Text = "<br clear='all' /><span style='font-size:10px;'><a href='mobile_details.aspx?comp_ID=" & CInt(id(j)) & "&contact_ID=" & CInt(cont_id(j)) & "&source=" & e.Item.Cells(3).Text & "&type=1'>" & contact_first_name(j) & " " & contact_last_name(j) & "</a></span>"
                    End If
                    'cont.CommandName = "cont_details_from_ac"

                    'cont.CommandArgument = CInt(cont_id(j)) & "|" & e.Item.Cells(3).Text & "|" & CInt(id(j))
                    'AddHandler cont.Click, AddressOf dispDetails_link

                    Dim contact_text As String = ""
                    'set up contact mouseover display

                    If Not contact_first_name(j) = "" Then
                      contact_text = "<strong style='font-size:14px;color:#67A0D9;'>" & contact_first_name(j)
                    End If
                    If Not contact_middle_initial(j) = "" Then
                      contact_text = contact_text & " " & contact_middle_initial(j)
                    End If
                    If Not contact_last_name(j) = "" Then
                      contact_text = contact_text & " " & contact_last_name(j) & "</strong><br />"
                    End If
                    If Not contact_title(j) = "" Then
                      contact_text = contact_text & contact_title(j) & " <br />"
                    End If
                    If Not contact_email_address(j) = "" Then
                      contact_text = contact_text & "<a href='mailto:" & contact_email_address(j) & "' class='non_special_link'>" & contact_email_address(j) & "</a>"
                    End If

                    If text_string3 <> "" Then
                      'If Not Page.IsPostBack Then

                      If Not Page.IsPostBack Then
                        count = count + 1
                        'Response.Write("I am touching database! " & Master.Search & "<br />")


                        'Query for phone numbers. 
                        Master.aTempTable = Master.aclsData_Temp.GetPhoneNumbers(id(j), 0, e.Item.Cells(3).Text, 0)
                        If Not IsNothing(Master.aTempTable) Then

                          If Master.aTempTable.Rows.Count > 0 Then
                            For Each q As DataRow In Master.aTempTable.Rows
                              If q("pnum_contact_id") <> 0 Then
                                contact_phone_text = contact_phone_text & q("pnum_type") & ": " & q("pnum_number") & "<br />"
                              Else
                                phone_text = phone_text & q("pnum_type") & ": " & q("pnum_number") & "<br />"
                              End If
                            Next
                          End If
                        End If

                        ' Session.Item("NotRealPostBack") = false 'do not rework database connections
                      End If
                    End If
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
                    but.ID = "Button" & j & id(j)
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
                    fly.AttachTo = "Button" & j & id(j)
                    fly.Controls.Add(text)
                    If add_me = True Then
                      'container.Controls.Add(but)
                      container.Controls.Add(lab)
                      ' e.Item.Cells(rowadd).Controls.Add(but)
                      ' e.Item.Cells(rowadd).Controls.Add(lab)
                    End If
                    'If display = 2 Then
                    '    If act_name(j) = "Exclusive Broker" Then
                    '        Dim ex As Label = e.Item.Cells(15).FindControl("popup_ex")
                    '        Dim flyout1 As OboutInc.Flyout2.Flyout = e.Item.Cells(16).FindControl("Flyout1")
                    '        Dim str As String = ex.Text
                    '        ' ex.Text = "<img src='images/purple_arrow.gif' alt='Exclusive' width='25'/>"
                    '        flyout1.Controls.Clear()
                    '        flyout1.Controls.Add(text)
                    '    End If
                    'End If
                  Else
                    add_me = False ' no company to add :(
                    'change add me to false so a company doesn't get added.
                    'fixed 6/14/2011
                  End If

                  If add_me = True Then
                    container.Controls.Add(cont)
                    ' e.Item.Cells(rowadd).Controls.Add(cont)
                  End If
                  If add_me = True Then
                    'container.Controls.Add(fly)
                    'e.Item.Cells(rowadd).Controls.Add(fly)
                    Dim pan As Panel = e.Item.Cells(rowadd).FindControl("company_hold")
                    pan.Controls.Add(container)
                    'e.Item.Cells(rowadd).Controls.Add(container)
                  End If

                  'Next

                End If
              End If
            End If
          Next
        End If
      End If
    End If

  End Sub

  Function Aircraft_Item_Databound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) As String
    Aircraft_Item_Databound = ""
    Dim lng_address_string As String = ""
    Dim comp_name As String = ""
    Dim comp_source As String = ""
    Dim comp_address As String = ""
    Dim comp_address2 As String = ""
    Dim comp_zip_code As String = ""
    Dim comp_country As String = ""
    Dim comp_city As String = ""
    Dim comp_state As String = ""
    Dim comp_web_address As String = ""
    Dim comp_email_address As String = ""
    Dim comp_id As Integer = 0
    Dim office_phone As String = ""
    Dim office_fax As String = ""

    Dim contact_email_address As String = ""
    Dim contact_first_name As String = ""
    Dim contact_last_name As String = ""
    Dim contact_middle_initial As String = ""
    Dim contact_title As String = ""

    Dim perc As String = ""
    Dim act_name As String = ""
    Dim text_string2 As String = ""
    Dim address_string As String = ""
    Dim text_string3 As String = ""
    Dim phone_text As String = ""
    Dim contact_text As String = ""
    Dim short_contact_text As String = ""
    Dim font_color As String = ""
    Dim source As String = ""
    Dim fly_text As String = ""
    Dim client_exists As Boolean = True
    Dim text As New Label
    Dim operator_string As String = Session.Item("types_of_owners")

    'If operator_string = "" Then
    '    operator_string = "all"
    'End If

    Dim combined_table As New DataTable
    Dim first_table As New DataTable
    Dim second_table As New DataTable

    Dim counter As Integer = 0
    If Trim(e.Item.Cells(1).Text) <> "&nbsp;" Then
      If Not IsNothing(e.Item.Cells(8)) Then

        'Response.Write(e.Item.Cells(1).Text & " source<br />")
        'Response.Write(e.Item.Cells(2).Text & " other source<br />")
        'Response.Write(e.Item.Cells(3).Text & " ac id<br />")
        'Response.Write(e.Item.Cells(4).Text & " other ac ID<br />")
        Dim source_1 As String = IIf(e.Item.Cells(1).Text <> "&nbsp;", e.Item.Cells(1).Text, Nothing)
        Dim source_2 As String = IIf(e.Item.Cells(2).Text <> "&nbsp;", e.Item.Cells(2).Text, Nothing)
        Dim ac_id_1 As String = IIf(e.Item.Cells(3).Text <> "&nbsp;", e.Item.Cells(3).Text, Nothing)
        Dim ac_id_2 As String = IIf(e.Item.Cells(4).Text <> "&nbsp;", e.Item.Cells(4).Text, Nothing)
        If Not IsNothing(ac_id_2) And Not IsNothing(source_2) Then
          first_table = Master.aclsData_Temp.Aircraft_Listing_Company_Display(e.Item.Cells(4).Text, e.Item.Cells(2).Text, operator_string)

          If IsNothing(first_table) Then
            If Master.aclsData_Temp.class_error <> "" Then
              Master.error_string = Master.aclsData_Temp.class_error
              clsGeneral.clsGeneral.LogError("listing.aspx.vb - 1. Generation of Company Listings on Aircraft Search() - " & Master.error_string, Master.aclsData_Temp)
            End If
            Master.display_error()
          End If

        End If


        If Not IsNothing(ac_id_1) And Not IsNothing(source_1) Then
          If (IsNumeric(e.Item.Cells(3).Text)) And ((e.Item.Cells(1).Text = "CLIENT" Or e.Item.Cells(1).Text = "JETNET")) Then
            second_table = Master.aclsData_Temp.Aircraft_Listing_Company_Display(e.Item.Cells(3).Text, e.Item.Cells(1).Text, operator_string)
            If IsNothing(second_table) Then
              If Master.aclsData_Temp.class_error <> "" Then
                Master.error_string = Master.aclsData_Temp.class_error
                clsGeneral.clsGeneral.LogError("listing.aspx.vb - 2. Generation of Company Listings on Aircraft Search() - " & Master.error_string, Master.aclsData_Temp)
              End If
              Master.display_error()
            End If

          End If
        End If

        If first_table.Rows.Count > 0 Then 'if the first table has rows..
          combined_table = first_table 'we default to setting the display table as the first table, just in case there is no second table.
          If second_table.Rows.Count > 0 Then 'if the second table has rows
            combined_table = Master.aclsData_Temp.Combine_Jetnet_Client_Company_Listing_Display(first_table, second_table) 'then we send to a combining and distinct function
          End If
        ElseIf second_table.Rows.Count > 0 Then 'if the first table doesn't have rows, does the second?
          combined_table = second_table 'yes, it has rows.
        End If

        If Not IsNothing(combined_table) Then
          If combined_table.Rows.Count > 0 Then
            For Each company As DataRow In combined_table.Rows
              fly = New OboutInc.Flyout2.Flyout
              linked = New Label
              lab = New Label
              but = New ImageButton
              address_string = ""
              lng_address_string = ""
              short_contact_text = ""
              contact_text = ""
              phone_text = ""
              text = New Label
              cont = New Label
              container = New Panel


              counter = counter + 1
              comp_name = IIf(Not IsDBNull(company("comp_name")), company("comp_name"), "")
              comp_source = IIf(Not IsDBNull(company("source")), company("source"), "")
              comp_address = IIf(Not IsDBNull(company("comp_address")), company("comp_address"), "")
              comp_address2 = IIf(Not IsDBNull(company("comp_address2")), company("comp_address2"), "")
              comp_zip_code = IIf(Not IsDBNull(company("comp_zip_code")), company("comp_zip_code"), "")
              comp_country = IIf(Not IsDBNull(company("comp_country")), company("comp_country"), "")
              comp_city = IIf(Not IsDBNull(company("comp_city")), company("comp_city"), "")
              comp_state = IIf(Not IsDBNull(company("comp_state")), company("comp_state"), "")
              comp_web_address = IIf(Not IsDBNull(company("comp_web_address")), company("comp_web_address"), "")
              comp_email_address = IIf(Not IsDBNull(company("comp_email_address")), company("comp_email_address"), "")
              comp_id = IIf(Not IsDBNull(company("comp_id")), company("comp_id"), 0)
              perc = IIf(Not IsDBNull(company("percentage")), company("percentage"), "")
              act_name = IIf(Not IsDBNull(company("actype_name")), company("actype_name"), "")
              office_phone = IIf(Not IsDBNull(company("comp_office_phone")), company("comp_office_phone"), "")
              office_fax = IIf(Not IsDBNull(company("comp_fax_phone")), company("comp_fax_phone"), "")

              contact_email_address = IIf(Not IsDBNull(company("contact_email_address")), company("contact_email_address"), "")
              contact_first_name = IIf(Not IsDBNull(company("contact_first_name")), company("contact_first_name"), "")
              contact_last_name = IIf(Not IsDBNull(company("contact_last_name")), company("contact_last_name"), "")
              contact_middle_initial = IIf(Not IsDBNull(company("contact_middle_initial")), company("contact_middle_initial"), "")
              contact_title = IIf(Not IsDBNull(company("contact_title")), company("contact_title"), "")



              text_string3 = comp_name

              If comp_source = "JETNET" Then
                If color = "container_grid" Then
                  color = "container_grid_alt"
                Else
                  color = "container_grid"
                End If
                font_color = "#023657"
                source = "JETNET"
              ElseIf comp_source = "CJETNET" Then
                If color = "container_grid" Then
                  color = "container_grid_alt"
                Else
                  color = "container_grid"
                End If
                font_color = "#023657"
                client_exists = True
                source = "CLIENT"
              ElseIf comp_source = "JCLIENT" Then
                If color = "container_grid_client" Then
                  color = "container_grid_alt_client"
                Else
                  color = "container_grid_client"
                End If
                client_exists = True
                source = "CLIENT"
                font_color = "#7a3733"
              ElseIf comp_source = "CLIENT" Then
                If color = "container_grid_client" Then
                  color = "container_grid_alt_client"
                Else
                  color = "container_grid_client"
                End If
                source = "CLIENT"
                font_color = "#7a3733"
              End If

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

              If client_exists = True Then
                comp_source = "JETNET"
              End If

              If Not contact_first_name = "" Then
                contact_text = "<strong style='font-size:14px;color:#67A0D9;'>" & contact_first_name
                short_contact_text = "<br />" & contact_first_name
              End If
              If Not contact_middle_initial = "" Then
                contact_text = contact_text & " " & contact_middle_initial
                short_contact_text = short_contact_text & " " & contact_middle_initial
              End If
              If Not contact_last_name = "" Then
                contact_text = contact_text & " " & contact_last_name & "</strong><br />"
                short_contact_text = short_contact_text & " " & contact_last_name
              End If


              If Not contact_title = "" Then
                contact_text = contact_text & contact_title & " <br />"

              End If


              If Not contact_email_address = "" Then
                contact_text = contact_text & "<a href='mailto:" & contact_email_address & "' class='non_special_link'>" & contact_email_address & "</a>"
              End If


              If office_phone <> "" Then
                phone_text = phone_text & "Office: " & office_phone & "<br />"
              End If

              If office_fax <> "" Then
                phone_text = phone_text & "Fax: " & office_fax & "<br />"
              End If

              If phone_text <> "" Then
                phone_text = "<br /><br /><strong style='font-size:14px;color:#67A0D9;'>COMPANY PHONE NUMBERS</strong><br />" & phone_text
              End If

              container.CssClass = color

              linked.Text = "<span style='font-size:10px;'><a href='details.aspx?comp_ID=" & comp_id & "&source=" & source & "&type=1'>" & text_string3 & " (<em>" & act_name & clsGeneral.clsGeneral.showpercent(perc, act_name) & "</em>)</a> " & short_contact_text & " </span>"
              container.Controls.Add(linked)
              text_string2 = "<span style='font-size:9px;'><i>" & address_string & "</i></span>"

              container.Controls.Add(linked)
              'e.Item.Cells(rowadd).Controls.Add(linked)

              but.ID = "Button" & counter & comp_id
              but.ImageUrl = "~/images/magnify.png"
              but.OnClientClick = "return false;"

              fly.Align = OboutInc.Flyout2.AlignStyle.TOP
              fly.Position = OboutInc.Flyout2.PositionStyle.TOP_RIGHT
              fly.FlyingEffect = OboutInc.Flyout2.FlyingEffectStyle.TOP_RIGHT
              fly.FadingEffect = True
              fly_text = clsGeneral.clsGeneral.MouseOverTextStart()
              fly_text = fly_text & UCase(lng_address_string)


              fly_text = fly_text & UCase(phone_text)

              If contact_text <> "" Then
                fly_text = fly_text & "<br /><br />" & UCase(contact_text)
              End If

              fly_text = fly_text & clsGeneral.clsGeneral.MouseOverTextEnd()
              text.Text = fly_text
              fly.AttachTo = "Button" & counter & comp_id
              fly.Controls.Add(text)



              'If act_name = "Exclusive Broker" Then
              '    Dim ex As Label = e.Item.Cells(17).FindControl("popup_ex")
              '    Dim flyout1 As OboutInc.Flyout2.Flyout = e.Item.Cells(17).FindControl("Flyout1")
              '    Dim str As String = ex.Text
              '    ' ex.Text = "<img src='images/purple_arrow.gif' alt='Exclusive' width='25'/>"
              '    flyout1.Controls.Clear()
              '    flyout1.Controls.Add(text)
              'Else
              container.Controls.Add(but)
              container.Controls.Add(lab)

              container.Controls.Add(cont)

              container.Controls.Add(fly)

              Dim pan As Panel = e.Item.Cells(7).FindControl("company_hold")
              pan.Controls.Add(container)
              'End If


            Next
          End If
        End If
      End If
    End If
  End Function

  Private Sub aircraft_search_button_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles aircraft_search_button.Click
    Session.Item("types_of_owners") = ac_types_of_owners.SelectedValue
    If Not IsNothing(Master.FindControl("new_search")) Then
      Master.FindControl("new_search").Visible = True
    End If
    search_results_error.Text = ""
    general_search_error.Text = ""
    Dim aircraft_link As Label = Master.FindControl("aircraft_link")
    aircraft_link.BackColor = Drawing.Color.FromName("#8cc7dd")
    Try
      Dim subnode As Boolean = False
      If Master.SubNodeOfListing <> 0 Then
        subnode = True
      End If
      Dim models As String = ""
      For i = 0 To model_cbo.Items.Count - 1
        If model_cbo.Items(i).Selected Then
          If model_cbo.Items(i).Value <> "" Then
            models = models & "'" & model_cbo.Items(i).Value & "',"
          End If
        End If
      Next

      If models <> "" Then
        models = UCase(models.TrimEnd(","))
      End If
      Session.Item("models_export") = models

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

      Session("search_aircraft") = Trim(clsGeneral.clsGeneral.StripChars(aircraft_search_for.Text, True)) & "@" & aircraft_search_where.SelectedValue & "@@" & models & "@" & market_status_cbo.SelectedValue & "@" & ac_sort.SelectedValue & "@" & sort_method_cbo.SelectedValue & "@" & ac_subset.SelectedValue & "@" & Trim(clsGeneral.clsGeneral.StripChars(airport_name.Text, True)) & "@" & Trim(clsGeneral.clsGeneral.StripChars(icao_code.Text, True)) & "@" & Trim(clsGeneral.clsGeneral.StripChars(iata_code.Text, True)) & "@" & Trim(clsGeneral.clsGeneral.StripChars(city.Text, True)) & "@" & clsGeneral.clsGeneral.StripChars(country.SelectedValue, True) & "@" & states & "@" & types_of_owners.SelectedValue & "@" & on_lease.SelectedValue & "@" & on_exclusive.SelectedValue & "@" & year_start.SelectedValue & "@" & year_end.SelectedValue & "@" & aftt.Checked & "@" & "@"
      Master.aTempTable = clsGeneral.clsGeneral.Fill_Aircraft(Master, Nothing, ac_sort.SelectedValue, ac_subset.SelectedValue, ac_types_of_owners.SelectedValue, aircraft_search_for.Text, market_status_cbo.Text, airport_name.Text, icao_code.Text, iata_code.Text, city.Text, country.Text, on_exclusive.SelectedValue, on_lease.SelectedValue, year_start.Text, year_end.Text, aircraft_search_where.Text, model_cbo, subnode, states, "", "", "", "", "", "", "", "", "", "", "", "", "", 0, "", False)
      If Not IsNothing(Master.aTempTable) Then
        If Master.aTempTable.Rows.Count > 0 Then
          Record_Count(Master.aTempTable.Rows.Count)
          ac_list.DataSource = Master.aTempTable
          ac_list.DataBind()

          search_results_error.Text = ""
          aircraft_search.Visible = False
        Else
          No_Records(ac_list)
        End If
      Else
        If Master.aclsData_Temp.class_error <> "" Then
          Master.error_string = "mobile_listing.aspx.vb - aircraft_search_button_CLICK() - " & Master.aclsData_Temp.class_error

          clsGeneral.clsGeneral.LogError(Master.error_string, Master.aclsData_Temp)
        End If
      End If
    Catch ex As Exception
      Master.error_string = "mobile_listing.aspx.vb - aircraft_search_button_CLICK() - " & ex.Message
      clsGeneral.clsGeneral.LogError(Master.error_string, Master.aclsData_Temp)
    End Try
  End Sub
#End Region


#Region "Market Search"
  Private Sub categories_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles categories.SelectedIndexChanged
    clsGeneral.clsGeneral.Market_Type(categories, market_types, Master.aclsData_Temp, "")
  End Sub
  Private Sub search_market_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles search_market.Click

    Try

      search_results_error.Text = ""
      general_search_error.Text = ""
      Master.aTempTable = clsGeneral.clsGeneral.Fill_Market(Master, Nothing, market_model, categories, market_types, CInt(market_time.SelectedValue), "", "")

      If Not IsNothing(Master.aTempTable) Then
        If Master.aTempTable.Rows.Count > 0 Then
          Record_Count(Master.aTempTable.Rows.Count)
          search_results_error.Text = ""
          market_list.DataSource = Master.aTempTable
          market_results.DataBind()
          market_search.Visible = False
        Else
          No_Records(market_list)
        End If
      Else
        'Nothing was Returned
        search_results_error.Text = "<p align='center'>0 Records have been found. Please try again.</p>"
        If Master.aclsData_Temp.class_error <> "" Then
          Master.error_string = "main_site.Master.vb - Fill_Market() - " & Master.aclsData_Temp.class_error

          clsGeneral.clsGeneral.LogError(Master.error_string, Master.aclsData_Temp)
        End If
        Master.aclsData_Temp.class_error = ""
      End If
    Catch ex As Exception
      Master.error_string = "mobile_listing.aspx.vb - search market Click()- " & ex.Message
      clsGeneral.clsGeneral.LogError(Master.error_string, Master.aclsData_Temp)
    End Try
  End Sub
#End Region

#Region "Transaction Search"
  Private Sub search_transactions_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles search_transactions.Click
    Try
      If transaction_start_date_txt.Text <> "" Or transaction_end_date_txt.Text <> "" Or transaction_search_for_txt.Text <> "" Or transaction_model.SelectedValue <> "" Then
        search_results_error.Text = ""
        general_search_error.Text = ""
        Dim transaction_link As Label = Master.FindControl("transaction_link")
        transaction_link.BackColor = Drawing.Color.FromName("#8cc7dd")
        Dim rel As String = ""
        For i = 0 To relationships.Items.Count - 1
          If relationships.Items(i).Selected Then
            rel = rel & "" & relationships.Items(i).Value & ","
          End If
        Next

        If rel <> "" Then
          rel = UCase(rel.TrimEnd(","))
        End If
        Session.Item("transaction_owners") = rel

        Master.aTempTable = clsGeneral.clsGeneral.Fill_Transactions(transaction_start_date_txt.Text, transaction_end_date_txt.Text, transaction_model, transaction_search_for_txt.Text, transaction_search_where.Text, internal_trans.SelectedValue, awaiting.Checked, transaction_trans_type_cbo.SelectedValue, transaction_subset.SelectedValue, transaction_year_start.Text, transaction_year_end.Text, Master, Nothing)
        If Not IsNothing(Master.aTempTable) Then
          If Master.aTempTable.Rows.Count > 0 Then
            Record_Count(Master.aTempTable.Rows.Count)
            trans_list.DataSource = Master.aTempTable
            trans_list.DataBind()
            transaction_search.Visible = False
          Else
            No_Records(contact_list)
          End If
        Else
          If Master.aclsData_Temp.class_error <> "" Then
            Master.error_string = "mobile_listing.aspx.vb - trans_search_button_CLICK()  - " & Master.aclsData_Temp.class_error

            clsGeneral.clsGeneral.LogError(Master.error_string, Master.aclsData_Temp)
          End If
        End If
      Else
        search_results_error.Text = "<p align='center'>Please add more search parameters.</p>"
      End If
    Catch ex As Exception
      Master.error_string = "mobile_listing.aspx.vb - trans_search_button_CLICK() - " & ex.Message
      clsGeneral.clsGeneral.LogError(Master.error_string, Master.aclsData_Temp)
    End Try
  End Sub
#End Region
End Class