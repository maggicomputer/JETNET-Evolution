Imports System.IO
Imports crmWebClient.clsGeneral
Partial Public Class companySearch
  Inherits System.Web.UI.UserControl
  Public Event Searched_Me(ByVal sender As Object, ByVal subnode As String, ByVal search_for As String, ByVal search_where As String, ByVal search_for_cbo As String, ByVal status_cbo As String, ByVal subset As String, ByVal country As String, ByVal states As String, ByVal operator_type As String, ByVal show_all As String, ByVal special_field As String, ByVal special_field_text As String, ByVal special_field_view As Boolean, ByVal special_field_text As String, ByVal client_IDS As String, ByVal jetnet_IDS As String, ByVal companyCity As String, ByVal mergeLists As Boolean)
  Public Event ChangedListing(ByVal sender As Object, ByVal Listing_Type As String)
  Dim error_string As String = ""

  Dim atemptable As DataTable
#Region "Events"
  Public Sub search_Click()
    Dim companySearchString As String = ""
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Try
      company_search_attention.Text = ""
      'search_pnl.Height = 60

      'Event that's handled on the Master Page.
      'Clicking the button, so clear the subfolder.
      masterPage.NameOfSubnode = ""


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

      Dim phone As String = Trim(clsGeneral.clsGeneral.StripChars(company_phone_number.Text, True))
      Dim jetnet_IDS As String = ""
      Dim client_IDS As String = ""
      If phone <> "" Then
        Try
          phone = Replace(phone, ".", "_")
          'phone = Replace(phone, "-", "_")
          phone = Replace(phone, "(", "_")
          phone = Replace(phone, ")", "_")
          phone = "%" & phone & "%"

          atemptable = masterPage.aclsData_Temp.SearchPhoneNumbers(phone)
          '' check the state of the DataTable
          If Not IsNothing(atemptable) Then
            If atemptable.Rows.Count > 0 Then
              For Each r As DataRow In atemptable.Rows
                If r("source") = "JETNET" Then
                  jetnet_IDS = jetnet_IDS & r("pnum_comp_id") & ","
                Else
                  client_IDS = client_IDS & r("pnum_comp_id") & ","
                End If
              Next
            Else
            End If
          Else
            If masterPage.aclsData_Temp.class_error <> "" Then
              error_string = masterPage.aclsData_Temp.class_error
              masterPage.LogError("companysearch.ascx.vb - search button click() - " & error_string)
            End If
            masterPage.display_error()
          End If
          If jetnet_IDS <> "" Then
            jetnet_IDS = UCase(jetnet_IDS.TrimEnd(","))
          End If
          If client_IDS <> "" Then
            client_IDS = UCase(client_IDS.TrimEnd(","))
          End If
        Catch ex As Exception
          error_string = "companysearch.ascx.vb - search button click() - " & ex.Message
          masterPage.LogError(error_string)
        End Try
      End If

      If states <> "" Or country.SelectedValue <> "" Or city_textbox.Text <> "" Or (search_for_txt.Text <> "" Or show_all.Checked = True) Or special_field_txt.Text <> "" Or company_phone_number.Text <> "" Then
        masterPage.PerformDatabaseAction = True
        Session("search_company") = False & "@" & clsGeneral.clsGeneral.Get_Name_Search_String(search_for_txt.Text) & "@" & search_where.Text & "@" & search_for_cbo.Text & "@" & status_cbo.Text & "@" & subset.SelectedValue & "@" & country.SelectedValue & "@" & states & "@" & types_of_owners.SelectedValue & "@" & show_all.Checked & "@" & special_field_cbo.SelectedValue & "@" & clsGeneral.clsGeneral.StripChars(special_field_txt.Text, True) & "@" & special_field_view.Checked & "@" & special_field_cbo.Text & "@" & Trim(clsGeneral.clsGeneral.StripChars(company_phone_number.Text, True)) & "@" & Trim(clsGeneral.clsGeneral.StripChars(city_textbox.Text, True)) & "@" & MergeList.Checked.ToString
        RaiseEvent Searched_Me(search_button, False, clsGeneral.clsGeneral.Get_Name_Search_String(search_for_txt.Text), search_where.Text, search_for_cbo.Text, status_cbo.Text, subset.SelectedValue, country.SelectedValue, states, types_of_owners.SelectedValue, show_all.Checked, special_field_cbo.SelectedValue, Trim(clsGeneral.clsGeneral.StripChars(special_field_txt.Text, True)), special_field_view.Checked, special_field_cbo.Text, client_IDS, jetnet_IDS, Trim(clsGeneral.clsGeneral.StripChars(city_textbox.Text, True)), MergeList.Checked)
        masterPage.PerformDatabaseAction = False


      Else
        company_search_attention.Text = "<p align='center'>Please use more detailed search parameters.</p>"
        search_pnl.Height = 170
      End If
      masterPage.Write_Javascript_Out()
    Catch ex As Exception
      error_string = "CompanySearch.ascx.vb - Search_Button_Click() " & ex.Message
      masterPage.LogError(error_string)
    End Try
  End Sub
  Private Sub Advanced_Search_Company_Fill_In()
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Try


      If Session.Item("localUser").crmEvo = True Then 'If an EVO user
      Else
        activity_view.Visible = True

        fields.Visible = True
      End If

      location_search.Visible = True
      adv_search.Visible = False
      'search_pnl.Height = 150
      country.Items.Clear()
      clsGeneral.clsGeneral.Populate_Country(country, Nothing, masterPage)
      state.Items.Clear()
      state.Items.Add(New ListItem("ALL", ""))
      clsGeneral.clsGeneral.Populate_State(state, Nothing, masterPage)
    Catch ex As Exception
      error_string = "CompanySearch.ascx.vb - adv_search_click() " & ex.Message
      masterPage.LogError(error_string)
    End Try
  End Sub
  'Private Sub adv_search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles adv_search.Click
  '    Advanced_Search_Company_Fill_In()

  '    country.SelectedValue = ""

  'End Sub
  Private Sub search_for_cbo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles search_for_cbo.SelectedIndexChanged
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    'Event that's handled on the Master Page.
    'Response.Write(search_for_cbo.SelectedItem.Value)
    Try
      RaiseEvent ChangedListing(e, search_for_cbo.SelectedValue)
    Catch ex As Exception
      error_string = "CompanySearch.ascx.vb - search_for_cbo_SelectedIndexChanged() " & ex.Message
      masterPage.LogError(error_string)
    End Try
  End Sub
#End Region
#Region "Events dealing with Advanced Country Search"
  Private Sub country_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles country.SelectedIndexChanged
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Try
      If country.SelectedValue = "United States" Then
        state.Visible = True
        search_pnl.Height = 200
      Else
        state.Visible = False
        state.SelectedValue = ""
        search_pnl.Height = 155
      End If
    Catch ex As Exception
      error_string = "CompanySearch.ascx.vb - country_SelectedIndexChanged() " & ex.Message
      masterPage.LogError(error_string)
    End Try
  End Sub
#End Region

  Private Sub subset_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles subset.SelectedIndexChanged
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Try
      If subset.SelectedValue = "C" Then
        show_all.Visible = True

        If country.SelectedValue = "United States" Then
          search_pnl.Height = 220
        Else
          search_pnl.Height = 175
        End If

      Else
        show_all.Visible = False
      End If
    Catch ex As Exception
      error_string = "CompanySearch.ascx.vb - subset_SelectedIndexChanged() " & ex.Message
      masterPage.LogError(error_string)
    End Try
  End Sub
  Private Sub Select_Special_Field(ByVal select_string As String)
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Try
      If select_string <> "" Then
        special_field_txt.Visible = True
        Flyout1.Visible = True
        Button1.Visible = True
        special_field_view.Visible = True
        special_field_view.Text = "Display " & special_field_cbo.SelectedItem.Text & " column in search results"
      Else
        special_field_txt.Visible = False
        Flyout1.Visible = False
        Button1.Visible = False
        special_field_view.Visible = False
      End If
    Catch ex As Exception
      error_string = "CompanySearch.ascx.vb - special_field_cbo_SelectedIndexChanged() " & ex.Message
      masterPage.LogError(error_string)
    End Try
  End Sub
  Private Sub special_field_cbo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles special_field_cbo.SelectedIndexChanged
    Select_Special_Field(special_field_cbo.SelectedValue)
  End Sub

  Private Sub Last_Search()
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Try
      Dim subnode As Boolean = False
      Dim comp_search As Array = Split(Session("search_company"), "@")
      Dim states As Array = Split(comp_search(7), ",")
      Dim statestr As String = ""

      search_for_txt.Text = Trim(comp_search(1))
      search_where.Text = comp_search(2)
      search_for_cbo.Text = comp_search(3)
      status_cbo.Text = comp_search(4)
      special_field_cbo.SelectedValue = comp_search(10)
      If comp_search(6) <> "" Or comp_search(7) <> "" Or comp_search(8) <> "" Or comp_search(9) <> "" Or comp_search(10) <> "" Or comp_search(11) <> "" Or comp_search(12) <> "" Or comp_search(13) <> "" Then
        Advanced_Search_Company_Fill_In() 'make advanced search visible
        'is the special field selected?
        Select_Special_Field(comp_search(10))
      End If

      If Replace(comp_search(7), "'", "") <> "" Then
        state.Visible = True
        search_pnl.Height = 190
        state.SelectedValue = -1 ' deselect all
        For x = 0 To UBound(states)
          'state.Items(2).Selected = True
          For j As Integer = 0 To state.Items.Count - 1
            Dim mode As String = UCase(state.Items(j).Value)
            Dim et As String = Replace(UCase(states(x)), "'", "")
            If mode = et Then
              state.Items(j).Selected = True
            Else
            End If
          Next
        Next
      End If

      subset.SelectedValue = comp_search(5)
      country.SelectedValue = comp_search(6)
      types_of_owners.SelectedValue = comp_search(8)
      show_all.Checked = comp_search(9)
      special_field_cbo.SelectedValue = comp_search(10)
      special_field_txt.Text = Trim(comp_search(11))
      special_field_view.Checked = comp_search(12)
      special_field_cbo.Text = comp_search(13)
      company_phone_number.Text = comp_search(14)
      city_textbox.Text = comp_search(15)

      'figure out ids for phone #
      Dim phone As String = Trim(clsGeneral.clsGeneral.StripChars(company_phone_number.Text, True))
      Dim jetnet_IDS As String = ""
      Dim client_IDS As String = ""
      If phone <> "" Then
        phone = Replace(phone, ".", "%")
        'phone = Replace(phone, "-", "%")
        phone = Replace(phone, "(", "%")
        phone = Replace(phone, ")", "%")
        phone = "%" & phone & "%"
        Try
          atemptable = masterPage.aclsData_Temp.SearchPhoneNumbers(phone)
          '' check the state of the DataTable
          If Not IsNothing(atemptable) Then
            If atemptable.Rows.Count > 0 Then
              For Each r As DataRow In atemptable.Rows
                If r("source") = "CLIENT" Then
                  client_IDS = client_IDS & r("pnum_comp_id") & ","
                Else
                  jetnet_IDS = jetnet_IDS & r("pnum_comp_id") & ","
                End If
              Next
            Else
            End If
          Else
            If masterPage.aclsData_Temp.class_error <> "" Then
              error_string = masterPage.aclsData_Temp.class_error
              masterPage.LogError("companysearch.ascx.vb - last search click() - " & error_string)
            End If
            masterPage.display_error()
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
          error_string = "companysearch.ascx.vb - last search click() - " & ex.Message
          masterPage.LogError(error_string)
        End Try
      End If

      RaiseEvent Searched_Me(Me, subnode, clsGeneral.clsGeneral.StripChars(search_for_txt.Text, True), search_where.Text, search_for_cbo.Text, status_cbo.Text, subset.SelectedValue, country.SelectedValue, comp_search(7), types_of_owners.SelectedValue, show_all.Checked, special_field_cbo.SelectedValue, clsGeneral.clsGeneral.StripChars(special_field_txt.Text, True), special_field_view.Checked, special_field_cbo.Text, client_IDS, jetnet_IDS, Trim(clsGeneral.clsGeneral.StripChars(city_textbox.Text, True)), MergeList.Checked)
    Catch ex As Exception
      error_string = "CompanySearch.ascx.vb - Last_Search() " & ex.Message
      masterPage.LogError(error_string)
    End Try
  End Sub

  Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    If Not Page.IsPostBack Then
      status_cbo.Items.Clear()
      status_cbo.Items.Add(New ListItem("All", "B"))
      status_cbo.Items.Add(New ListItem("Active", "Y"))
      status_cbo.Items.Add(New ListItem("Inactive", "N"))
      status_cbo.SelectedValue = "Y"


      'If Session.Item("jetnetWebSiteType") = eWebSiteTypes.TEST Or Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Then
      city_label.Visible = True
      city_textbox.Visible = True
      'End If


      Dim search_in As DropDownList = search_for_cbo

      search_in.Items.Clear()
      search_in.Items.Add(New ListItem("COMPANY", "1"))
      search_in.Items.Add(New ListItem("CONTACT", "2"))
      search_in.Items.Add(New ListItem("AIRCRAFT", "3"))
      search_in.Items.Add(New ListItem("ACTION ITEMS", "4"))
      search_in.Items.Add(New ListItem("NOTES", "6"))
      search_in.Items.Add(New ListItem("OPPORTUNITIES", "7"))
      search_in.Items.Add(New ListItem("TRANSACTIONS", "8"))
      search_in.Items.Add(New ListItem("MARKET", "10"))

      If Not Page.IsPostBack Then
        Try
          search_in.SelectedValue = 1
        Catch
        End Try
      End If

    End If
  End Sub
  Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Me.Visible Then
      If Session.Item("crmUserLogon") = True Then
        Dim masterPage As main_site = DirectCast(Page.Master, main_site)
        Try
          search_for_txt.Focus()
          If Session.Item("localUser").crmEvo = True Then 'If an EVO user
            status_cbo.Visible = False
            special_field_txt.Visible = False
            special_field_cbo.Visible = False
            special_field_view.Visible = False
            search_pnl.Height = 105
            search_pnl_table.Height = 105
            fields.Visible = False
            activity_view.Visible = False
          End If

          If Trim(Request("clear")) = "true" Then

            masterPage.FromTypeOfListing = 1 'added to retain listing ID that we came from on a search if the type is changed
            masterPage.TypeOfListing = 1
            masterPage.IsSubNode = False
            masterPage.NameOfSubnode = "Company"
            masterPage.SubNodeOfListing = 1
            masterPage.Subnode_Method = ""

            masterPage.Table_List = Nothing
            Session("Results") = Nothing
            Session("search_company") = Nothing
            Session("search_contact") = Nothing
            Session("search_aircraft") = Nothing
            Session("search_transaction") = Nothing
            Response.Redirect("/listing.aspx")
          End If

          'only if not post back
          If Not Page.IsPostBack Then

            Advanced_Search_Company_Fill_In()
            country.SelectedValue = ""
            Dim category As DropDownList = special_field_cbo
            category.Items.Add(New ListItem("NONE", ""))

            If Session.Item("localUser").crmEvo <> True Then
              atemptable = masterPage.aclsData_Temp.Get_Client_Preferences()
              If Not IsNothing(atemptable) Then
                If atemptable.Rows.Count > 0 Then
                  ' For Each r As DataRow In aTempTable.Rows
                  If Not IsDBNull(atemptable.Rows(0).Item("clipref_category1_use")) Then
                    If atemptable.Rows(0).Item("clipref_category1_use") = "Y" Then
                      category.Items.Add(New ListItem(atemptable.Rows(0).Item("clipref_category1_name"), "clicomp_category1"))
                    Else

                    End If
                  End If

                  If Not IsDBNull(atemptable.Rows(0).Item("clipref_category2_use")) Then
                    If atemptable.Rows(0).Item("clipref_category2_use") = "Y" Then
                      category.Items.Add(New ListItem(atemptable.Rows(0).Item("clipref_category2_name"), "clicomp_category2"))
                    Else

                    End If
                  End If

                  If Not IsDBNull(atemptable.Rows(0).Item("clipref_category3_use")) Then
                    If atemptable.Rows(0).Item("clipref_category3_use") = "Y" Then
                      category.Items.Add(New ListItem(atemptable.Rows(0).Item("clipref_category3_name"), "clicomp_category3"))
                    Else

                    End If
                  End If

                  If Not IsDBNull(atemptable.Rows(0).Item("clipref_category4_use")) Then
                    If atemptable.Rows(0).Item("clipref_category4_use") = "Y" Then
                      category.Items.Add(New ListItem(atemptable.Rows(0).Item("clipref_category4_name"), "clicomp_category4"))
                    Else

                    End If
                  End If

                  If Not IsDBNull(atemptable.Rows(0).Item("clipref_category5_use")) Then
                    If atemptable.Rows(0).Item("clipref_category5_use") = "Y" Then
                      category.Items.Add(New ListItem(atemptable.Rows(0).Item("clipref_category5_name"), "clicomp_category5"))
                    Else

                    End If
                  End If
                  'Next
                End If
              Else
                If masterPage.aclsData_Temp.class_error <> "" Then
                  error_string = masterPage.aclsData_Temp.class_error
                  masterPage.LogError("listing.aspx.vb - fill_CBO() - " & error_string)
                End If
                masterPage.display_error()
              End If
            End If

            Dim research As Boolean = False
            If Not IsNothing(Trim(Request("redo_search"))) Then
              If Trim(Request("redo_search")) = "true" Then
                research = True
              End If
            End If


            'Let's try to refill up the Company folders.
            Dim cfolderData As String = ""
            Dim FolderTable As New DataTable
            If masterPage.IsSubNode = True Then
              cfolderData = clsGeneral.clsGeneral.ReturnCfolderData(masterPage, FolderTable)

              If cfolderData = "" Then
                masterPage.Fill_Company(True, "", 2, "", "B", "JC", "", "", "", "", "", "", False, "", "", "", "", false)
              ElseIf cfolderData <> "" Then
                'Fills up the applicable folder Information pulled from the cfolder data field
                DisplayFunctions.FillUpFolderInformation(New Table, New Label, cfolderData, New Label, FolderTable, True, False, False, False, False, search_pnl, New BulletedList, Nothing, Nothing, Nothing)
                'Make sure state listbox shows if something is selected:
                If state.SelectedValue <> "" Then
                  state.Visible = True
                  search_pnl.Height = 200
                End If
                'Automatically running the search
                search_Click()

                masterPage.IsSubNode = False
                masterPage.SubNodeOfListing = 0
                masterPage.NameOfSubnode = ""
              End If
            End If



            If research = True Then
              If Not IsNothing(Session("search_company")) Then
                If Not String.IsNullOrEmpty(Session("search_company").ToString) Then
                  Last_Search() 'fill last search and perform
                End If
              End If
            End If

          End If
        Catch ex As Exception
          error_string = "CompanyCard.ascx.vb - Page Init() " & ex.Message
          masterPage.LogError(error_string)
        End Try
      End If
    End If
  End Sub


End Class