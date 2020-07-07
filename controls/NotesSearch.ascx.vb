Imports System.IO
Imports crmWebClient.clsGeneral
Partial Public Class NotesSearch
  Inherits System.Web.UI.UserControl
  Public Event check_changed(ByVal sender As Object)
  Public Event ChangedListing(ByVal sender As Object, ByVal Listing_Type As String)
  Public Event Searched_Me(ByVal sender As Object, ByVal search_for As String, ByVal search_where As String, ByVal search_for_cbo As String, ByVal ActiveStatus As String, ByVal type_notes As String, ByVal orderby As String, ByVal reg_start_date As String, ByVal reg_end_date As String, ByVal ClientIDs As String, ByVal JetnetIDs As String, ByVal acSearchField As Integer, ByVal acSearchOperator As Integer, ByVal acSearchText As String, ByVal NoteCategory As Integer, ByVal OnlyModel As Boolean, ByVal OnlyAircraft As Boolean, ByVal FolderType As Long)
  Dim error_string As String = ""
  Dim aTempTable, aTempTable2 As New DataTable 'Data Tables used

#Region "Custom Events"
  Private Sub search_button_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles search_button.Click
    Search()
  End Sub

  Private Sub Search()
    'Event that's handled on the Master Page.
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Dim FolderTypeValue As Long = 3
    Try
      masterPage.PerformDatabaseAction = True

      Dim acSearchText As String = "" 'Default this to be blank.
      Dim acSearchOperator As Integer = 0 'We're going to default this to be 0, meaning if it's zero, we can ignore it.
      Dim acSearchField As Integer = 0 'The search field is blank.
      Dim noteCategory As Integer = 0
      Dim JETNET_IDs As String = ""
      Dim CLIENT_IDs As String = ""
      Dim Active_Status As String = "" 'document_status.SelectedValue
      Dim ModelsForSave As String = ""

      If masterPage.TypeOfListing = 16 And prospect_search_by_dropdown.SelectedValue = "1" Then
        For i = 0 To ac_prospect_list.Items.Count - 1
          Dim splitIds As Array = Split(ac_prospect_list.Items(i).Value, "|")
          If UBound(splitIds) = 1 Then
            If ac_prospect_list.SelectedValue = "All" Then
              If ac_prospect_list.Items(i).Value <> "All" Then
                ModelsForSave += "'" & ac_prospect_list.Items(i).Value & "',"
                If splitIds(1) <> 0 Then
                  If JETNET_IDs <> "" Then
                    JETNET_IDs += " , "
                  End If
                  JETNET_IDs += splitIds(1)
                ElseIf splitIds(0) <> 0 Then
                  If CLIENT_IDs <> "" Then
                    CLIENT_IDs += " , "
                  End If
                  CLIENT_IDs += splitIds(0)
                End If
              End If
            Else
              If ac_prospect_list.Items(i).Selected Then
                ModelsForSave += "'" & ac_prospect_list.Items(i).Value & "',"
                If ac_prospect_list.Items(i).Value <> "" Then
                  If splitIds(1) <> 0 Then
                    If JETNET_IDs <> "" Then
                      JETNET_IDs += " , "
                    End If
                    JETNET_IDs += splitIds(1)
                  ElseIf splitIds(0) <> 0 Then
                    If CLIENT_IDs <> "" Then
                      CLIENT_IDs += " , "
                    End If
                    CLIENT_IDs += splitIds(0)
                  End If
                End If
              End If
            End If
          End If
        Next
      ElseIf (prospect_search_by_dropdown.SelectedValue <> 0) Then 'this means that if the search isn't prospect and is instead note related.
        'In a function because the master page uses the same function to send to the export page.
        clsGeneral.clsGeneral.Figure_Out_Note_Search_Fields(ac_search_field, ac_search_field_operator, ac_search_field_text, acSearchField, acSearchOperator, acSearchText)
        For i = 0 To model_cbo.Items.Count - 1
          If model_cbo.Items(i).Selected Then
            If model_cbo.Items(i).Value <> "" Then
              ModelsForSave += "'" & model_cbo.Items(i).Value & "',"
            End If
          End If
        Next
      End If


      If masterPage.TypeOfListing = 16 Then
        If showInactiveProspect.Checked = False Then
          Active_Status = "A"
        End If
      Else
        If (Not String.IsNullOrEmpty(FolderType.SelectedValue) And Not String.IsNullOrEmpty(listOfFolders.SelectedValue)) Then
          Dim FolderDataTable As New DataTable
          Dim FieldToPoll As String = "ac"
          JETNET_IDs = ""
          CLIENT_IDs = ""

          Select Case FolderType.SelectedValue
            Case "2"
              FieldToPoll = "contact"
              FolderTypeValue = 2
            Case "1"
              FieldToPoll = "comp"
              FolderTypeValue = 1
          End Select

          FolderDataTable = masterPage.aclsData_Temp.Get_Client_Folder_Index(CLng(listOfFolders.SelectedValue))
          If Not IsNothing(FolderDataTable) Then
            If FolderDataTable.Rows.Count > 0 Then
              ' build an string of ac_ids
              For Each r As DataRow In FolderDataTable.Rows
                If Not IsDBNull(r("cfoldind_jetnet_" & FieldToPoll & "_id")) Then
                  If IsNumeric(r("cfoldind_jetnet_" & FieldToPoll & "_id")) Then
                    If r("cfoldind_jetnet_" & FieldToPoll & "_id") > 0 Then
                      If JETNET_IDs <> "" Then
                        JETNET_IDs += ","
                      End If
                      JETNET_IDs += r("cfoldind_jetnet_" & FieldToPoll & "_id").ToString
                    End If
                  End If
                End If

                If Not IsDBNull(r("cfoldind_client_" & FieldToPoll & "_id")) Then
                  If IsNumeric(r("cfoldind_client_" & FieldToPoll & "_id")) Then
                    If r("cfoldind_client_" & FieldToPoll & "_id") > 0 Then
                      If CLIENT_IDs <> "" Then
                        CLIENT_IDs += ","
                      End If
                      CLIENT_IDs += r("cfoldind_client_" & FieldToPoll & "_id").ToString
                    End If
                  End If
                End If
              Next
            Else
              JETNET_IDs += ""
              CLIENT_IDs += ""
            End If
          End If
        End If
      End If

      noteCategory = prospect_category.SelectedValue

      If ModelsForSave <> "" Then
        ModelsForSave = UCase(ModelsForSave.TrimEnd(","))
      End If


      SaveSessionForRecall(ModelsForSave, Active_Status, acSearchField, acSearchOperator, acSearchText)


      RaiseEvent Searched_Me(search_button, Trim(clsGeneral.clsGeneral.StripChars(search_for_txt.Text, True)), search_where.SelectedValue, search_for_cbo.SelectedValue, Active_Status, display_cbo.SelectedValue, order_bo.SelectedValue, ad_start_date.Text, ad_end_date.Text, CLIENT_IDs, JETNET_IDs, acSearchField, acSearchOperator, acSearchText, noteCategory, IIf(masterPage.TypeOfListing = 16, IIf(prospect_search_by_dropdown.SelectedValue = 2, True, False), False), IIf(masterPage.TypeOfListing = 16, IIf(prospect_search_by_dropdown.SelectedValue = 1, True, False), False), FolderTypeValue)
      masterPage.PerformDatabaseAction = False
    Catch ex As Exception
      error_string = "NotesSearch.ascx.vb - Search_Button_Click() - " & ex.Message
      masterPage.LogError(error_string)
    End Try
  End Sub
  Private Sub search_for_cbo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles search_for_cbo.SelectedIndexChanged
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Try
      'Event that's handled on the Master Page.
      RaiseEvent ChangedListing(e, search_for_cbo.SelectedValue)
    Catch ex As Exception
      error_string = "NotesSearch.ascx.vb - search_for_cbo_SelectedIndexChanged() - " & ex.Message
      masterPage.LogError(error_string)
    End Try
  End Sub
#End Region
  Private Sub type_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles type.SelectedIndexChanged
    clsGeneral.clsGeneral.Type_Selected_Index_Changed(make, type, Page.IsPostBack)
  End Sub

  Private Sub SaveSessionForRecall(ByRef ModelsForSave As String, ByRef Active_Status As String, ByRef acSearchField As Integer, ByRef acSearchOperator As Integer, ByRef acSearchText As String)
    'First we save the search text
    Session("search_note") = Trim(clsGeneral.clsGeneral.StripChars(search_for_txt.Text, True)) & "@"
    'Then we save the search text operator (begins with, etc)
    Session("search_note") += search_where.SelectedValue & "@"
    'Then we save the type of search
    Session("search_note") += search_for_cbo.SelectedValue & "@"
    'Prospect active status
    Session("search_note") += Active_Status & "@"
    'Then we save the my notes/person's notes
    Session("search_note") += display_cbo.SelectedValue.ToString & "@"
    'Then we save the order by 
    Session("search_note") += order_bo.SelectedValue & "@"
    'Start date
    Session("search_note") += ad_start_date.Text & "@"
    'End Date
    Session("search_note") += ad_end_date.Text & "@"
    'Aircraft search field
    Session("search_note") += acSearchField & "@"
    'Aircraft search operator
    Session("search_note") += acSearchOperator.ToString & "@"
    'Aircraft search text
    Session("search_note") += acSearchText.ToString & "@"
    'models
    Session("search_note") += ModelsForSave & "@"
    'Category
    Session("search_note") += prospect_category.SelectedValue & "@"
    'Type Of Search
    Session("search_note") += prospect_search_by_dropdown.SelectedValue & "@"
    'FolderType
    Session("search_note") += FolderType.SelectedValue & "@"
    'ChosenFolder
    Session("search_note") += listOfFolders.SelectedValue & "@"
  End Sub

  Private Sub RecallSessionForSearch(ByRef TypeOfListing As Integer)
    If Not IsNothing(Session("search_note")) Then
      If Not String.IsNullOrEmpty(Session("search_note")) Then
        Dim SearchText As Array = Split(Session("search_note"), "@")

        'The first variable that's been saved is the 
        'Search for txt.
        If UBound(SearchText) >= 0 Then
          search_for_txt.Text = SearchText(0)
        End If

        'Then we fill in the search where.
        If UBound(SearchText) >= 1 Then
          search_where.SelectedValue = SearchText(1)
        End If

        'Type of 
        If UBound(SearchText) >= 2 Then
          search_for_cbo.SelectedValue = SearchText(2)
        End If

        'active status
        If UBound(SearchText) >= 3 Then
          If SearchText(3) = "I" Then
            showInactiveProspect.Checked = True
          End If
        End If

        'Person's notes/your notes
        If UBound(SearchText) >= 4 Then
          display_cbo.SelectedValue = SearchText(4)
        End If

        'Order by
        If UBound(SearchText) >= 5 Then
          order_bo.SelectedValue = SearchText(5)
        End If

        'Start date
        If UBound(SearchText) >= 6 Then
          ad_start_date.Text = SearchText(6)
        End If

        'End date
        If UBound(SearchText) >= 7 Then
          ad_end_date.Text = SearchText(7)
        End If

        'Aircraft search field
        If UBound(SearchText) >= 8 Then
          ac_search_field.Text = SearchText(8)
        End If

        'Aircraft search operator
        If UBound(SearchText) >= 9 Then
          ac_search_field_operator.SelectedValue = SearchText(9)
        End If

        'Aircraft search text
        If UBound(SearchText) >= 10 Then
          ac_search_field_text.Text = SearchText(10)
        End If

        'Prospect Category.
        If TypeOfListing = 16 Then
          If UBound(SearchText) >= 12 Then
            prospect_category.SelectedValue = SearchText(12)
          End If

          'Prospect search by
          If UBound(SearchText) >= 13 Then
            prospect_search_by_dropdown.SelectedValue = SearchText(13)
          End If

        Else
          'Note folder
          If UBound(SearchText) >= 14 Then
            FolderType.SelectedValue = SearchText(14)
            FolderType_SelectedIndexChanged(FolderType, System.EventArgs.Empty)
          End If
          If UBound(SearchText) >= 15 Then
            listOfFolders.SelectedValue = SearchText(15)
          End If
        End If




        'models
        If UBound(SearchText) >= 11 Then
          If Not String.IsNullOrEmpty(SearchText(11)) Then

            'Replacing single quotes first
            SearchText(11) = Replace(SearchText(11), "'", "")

            Dim ListBoxToFill As New ListBox
            Dim models As Array = Split(SearchText(11), ",")

            'Check search by models option
            search_by_models.Checked = True

            'Figure out what listbox to use to refill.
            If TypeOfListing = 16 Then
              ListBoxToFill = ac_prospect_list
            Else
              ListBoxToFill = model_cbo
            End If


            'deselect all previous
            ListBoxToFill.SelectedValue = -1

            For x = 0 To UBound(models)
              For j As Integer = 0 To ListBoxToFill.Items.Count() - 1
                If UCase(ListBoxToFill.Items(j).Value) = UCase(models(x)) Then
                  ListBoxToFill.Items(j).Selected = True
                End If
              Next
            Next
          End If
        End If

        Search()
      End If
    End If
  End Sub


  Private Sub make_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles make.SelectedIndexChanged
    clsGeneral.clsGeneral.Make_Selected_Index_Changed(model, make, type)
  End Sub

  Private Sub default_models_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles default_models.CheckedChanged
    RaiseEvent check_changed(Me)
  End Sub

  Private Sub model_type_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles model_type.SelectedIndexChanged
    clsGeneral.clsGeneral.Model_Type_Selected_Index_Changed(type, model_type)
  End Sub
  Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Me.Visible Then
      If Session.Item("crmUserLogon") = True Then
        Dim research As Boolean = False
        search_for_txt.Focus()
        Dim masterPage As main_site = DirectCast(Page.Master, main_site)
        Dim TypeDataTable As New DataTable
        Dim TypeDataHold As New DataTable
        Dim temptable As New DataTable
        Dim default_vis As Boolean = True
        Try
          '---------------------------------------------End Database Connection Stuff---------------------------------------------
          If Not Page.IsPostBack Then
            'Querying the Database and keeping this information so we only have to do it once. 
            If Not Page.IsPostBack Then
              If Session.Item("localUser").crmEvo = True Then 'If an EVO user

                clsGeneral.clsGeneral.Getting_Type_Listbox_Set(TypeDataTable, model_type, masterPage, temptable, TypeDataHold, type)
                ''''''
              Else
                If Trim(Request("redo_search")) = "true" Then
                  research = True
                End If

                Try
                  clsGeneral.clsGeneral.populate_models(model_cbo, IIf(research = True, False, True), Me, Nothing, masterPage, default_vis)
                Catch ex As Exception
                  error_string = "wantedSearch - fill_CBO() Trans Model Dropdown Filling - " & ex.Message
                  masterPage.LogError(error_string)
                End Try

                If model_cbo.SelectedValue <> "" Then
                  default_models.Checked = True
                  search_by_models.Checked = True
                Else
                  default_models.Checked = False
                End If
                model_cbo.Visible = True
                model_evo_swap.Visible = False
                model_type.Visible = False
                default_models.Visible = True

                If search_by_models.Checked = False Then
                  default_models.Enabled = False
                  default_models.Checked = False
                  model_cbo.Enabled = False
                End If
                'Else
                '     clsGeneral.clsGeneral.Getting_Type_Listbox_Set(TypeDataTable, model_type, masterPage, temptable, TypeDataHold, type)
                ' End If
              End If

              'If Session.Item("localUser").crmEvo = True Then 'If an EVO user
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

            End If




            'Document Status for Notes
            document_status.Items.Add(New ListItem("All", "B"))
            document_status.Items.Add(New ListItem("Yes", "Y"))
            document_status.Items.Add(New ListItem("No", "N"))
            'My notes or all notes 
            If Me.include_inactives.Checked = True Then
              clsGeneral.clsGeneral.Fill_User_Dropdown(display_cbo, masterPage.NameOfListingType, Nothing, masterPage, True)
            Else
              clsGeneral.clsGeneral.Fill_User_Dropdown(display_cbo, masterPage.NameOfListingType, Nothing, masterPage)
            End If





            If masterPage.TypeOfListing = 16 Then
              'In this case I would like to default to “All Prospects” rather than my prospects.
              If HttpContext.Current.Session.Item("localUser").crmUserType <> eUserTypes.MyNotesOnly Then
                display_cbo.SelectedValue = 0
              Else
                display_cbo.SelectedValue = HttpContext.Current.Session.Item("localUser").crmLocalUserID
              End If

              'Order by
              order_bo.Items.Add(New ListItem("Make/Model/Serial #/Company", "amod_make_name, amod_model_name, ac_ser_nbr, comp_name"))
              order_bo.Items.Add(New ListItem("Company/Make/Model/Serial #", "comp_name, amod_make_name, amod_model_name, ac_ser_nbr"))
              prospect_ac_sort.Visible = True
              action_sort.Visible = False
              prospect_search_by.Visible = True
              prospect_category_row.Visible = True
              clsGeneral.clsGeneral.Fill_Opportunity_Category(prospect_category, aTempTable, masterPage.aclsData_Temp)


              If ac_prospect_list.Items.Count = 1 Then
                FillUpProspects()
              End If

              If prospect_search_by_dropdown.SelectedValue = "3" Then
                prospect_neither_holder.Visible = True
                prospect_ac_sort.Visible = False
                action_sort.Visible = False
                search_pnl.Height = Unit.Pixel(175)
                search_pnl_table.Height = Unit.Pixel(170)
              End If
              folderTypeRow.Visible = False
            Else
              prospect_category_row.Visible = True
              clsGeneral.clsGeneral.Fill_Note_Category(prospect_category, "N", Nothing, masterPage, "notecat_order")
              'Order by
              order_bo.Items.Add(New ListItem("Entry Date", "lnote_entry_date"))
              order_bo.Items.Add(New ListItem("Note Text ", "lnote_note"))
              folderTypeRow.Visible = True
            End If

            search_where.Items.Clear()
            search_for_cbo.Items.Clear()
            search_for_cbo.Items.Add(New ListItem("COMPANY", "1"))
            search_for_cbo.Items.Add(New ListItem("CONTACT", "2"))
            search_for_cbo.Items.Add(New ListItem("AIRCRAFT", "3"))
            search_for_cbo.Items.Add(New ListItem("ACTION ITEMS", "4"))
            search_for_cbo.Items.Add(New ListItem("NOTES", "6"))
            search_for_cbo.Items.Add(New ListItem("AIRCRAFT PROSPECTS", "16"))
            search_for_cbo.Items.Add(New ListItem("OPPORTUNITIES", "7"))
            search_for_cbo.Items.Add(New ListItem("TRANSACTIONS", "8"))
            search_for_cbo.Items.Add(New ListItem("MARKET", "10"))
            search_where.Items.Add(New ListItem("Begins With", "2"))

            search_where.Items.Add(New ListItem("Anywhere", "1"))
            If Not Page.IsPostBack Then
              Try
                search_for_cbo.SelectedValue = masterPage.TypeOfListing
                If research = True Then
                  RecallSessionForSearch(masterPage.TypeOfListing)
                End If
              Catch
              End Try
              If masterPage.TypeOfListing = 16 Then
                masterPage.Write_Javascript_Out()
              End If
            End If
          End If




        Catch ex As Exception
          error_string = "notesSearch.ascx.vb - Page Init() - " & ex.Message
          masterPage.LogError(error_string)
        End Try

        If Not IsPostBack Then
          display_cbo.Items.Clear()
          If Me.include_inactives.Checked = True Then
            clsGeneral.clsGeneral.Fill_User_Dropdown(display_cbo, "Notes", Nothing, masterPage, True)
          Else
            clsGeneral.clsGeneral.Fill_User_Dropdown(display_cbo, "Notes", Nothing, masterPage)
          End If
        End If

      End If
    End If

  

  End Sub

  Private Sub search_by_models_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles search_by_models.CheckedChanged

    If search_by_models.Checked = False Then
      default_models.Enabled = False
      default_models.Checked = False
      model_cbo.Enabled = False
      model_cbo.SelectedIndex = -1
    Else
      default_models.Enabled = True
      default_models.Checked = True
      model_cbo.Enabled = True
    End If
  End Sub

  'This sub fills up the prospects list.
  Public Sub FillUpProspects()
    ac_prospect_list.Items.Clear()
    ac_prospect_list.Items.Add(New ListItem("All", "All"))
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Dim acTable As DataTable = masterPage.aclsData_Temp.BuildACProspectList(IIf(showInactiveProspect.Checked, "", "A"))
    If Not IsNothing(acTable) Then
      For Each r As DataRow In acTable.Rows
        Dim ACString As String = ""
        ACString = r("amod_make_name") & " " & r("amod_model_name")
        ACString += IIf(Not IsDBNull(r("ac_ser_nbr")), " Ser #:" & r("ac_ser_nbr") & " ", "")
        ACString += IIf(Not IsDBNull(r("ac_reg_nbr")), "Reg #" & r("ac_reg_nbr"), "")

        ac_prospect_list.Items.Add(New ListItem(ACString, r("lnote_client_ac_id") & "|" & r("lnote_jetnet_ac_id")))
      Next
    End If
  End Sub
  'Toggles off/on whether or not we're showing inactive/active prospects
  Private Sub showInactiveProspect_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles showInactiveProspect.CheckedChanged
    If prospect_search_by_dropdown.SelectedValue = 1 Then
      FillUpProspects()
    End If
  End Sub

  Private Sub prospect_search_by_list_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles prospect_search_by_dropdown.SelectedIndexChanged
    If prospect_search_by_dropdown.SelectedValue = 1 Then
      prospect_ac_sort.Visible = True
      action_sort.Visible = False
      search_pnl.Height = Unit.Pixel(175)
      search_pnl_table.Height = Unit.Pixel(170)
      prospect_neither_holder.Visible = False

    ElseIf prospect_search_by_dropdown.SelectedValue = 2 Then
      prospect_ac_sort.Visible = False
      action_sort.Visible = True
      model_swap_cell.RowSpan = 5
      search_pnl.Height = Unit.Pixel(185)
      search_pnl_table.Height = Unit.Pixel(180)
      prospect_neither_holder.Visible = False
      action_sort_col_one.Visible = False
      action_sort_col_two.Visible = False
      ac_search_field.Visible = False
      ac_search_field_operator.Visible = False
      ac_search_field_text.Visible = False
    Else
      prospect_neither_holder.Visible = True
      prospect_ac_sort.Visible = False
      action_sort.Visible = False
      search_pnl.Height = Unit.Pixel(175)
      search_pnl_table.Height = Unit.Pixel(170)
    End If
    Search()
  End Sub

  Private Sub FolderType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles FolderType.SelectedIndexChanged
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Dim FolderList As New DataTable
    If FolderType.SelectedValue = "" Then
      listOfFolders.Enabled = False
      listOfFolders.CssClass = "display_disable"
      listOfFolders.Items.Clear()
      listOfFolders.Items.Add(New ListItem("N/A", ""))
    Else
      listOfFolders.Enabled = True
      listOfFolders.CssClass = ""

      'We're going to make sure that we only use predefined input from us. If for some reason
      'They pass gunk as the selected value, it'll just default to aircraft folders.
      Select Case FolderType.SelectedValue
        Case "1"
          FolderList = masterPage.aclsData_Temp.Get_Client_Folders(CInt(Session.Item("localUser").crmLocalUserID), "Y", 1)
        Case "2"
          FolderList = masterPage.aclsData_Temp.Get_Client_Folders(CInt(Session.Item("localUser").crmLocalUserID), "Y", 2)
        Case Else
          FolderList = masterPage.aclsData_Temp.Get_Client_Folders(CInt(Session.Item("localUser").crmLocalUserID), "Y", 3)
      End Select

      If Not IsNothing(FolderList) Then
        If FolderList.Rows.Count > 0 Then
          listOfFolders.Items.Clear()
          For Each r As DataRow In FolderList.Rows
            If Not IsDBNull(r("cfolder_method")) Then
              If r("cfolder_method") = "S" Then
                listOfFolders.Items.Add(New ListItem(r("cfolder_name"), r("cfolder_id")))
              End If
            End If
          Next
        End If
      End If

    End If
  End Sub
End Class