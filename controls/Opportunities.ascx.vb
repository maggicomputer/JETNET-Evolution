Imports System.IO
Partial Public Class Opportunities
  Inherits System.Web.UI.UserControl
  Dim aclsData_Temp As New Object
  Dim aTempTable, aTempTable2 As New DataTable 'Data Tables used
  Public Event ac_searchClick(ByVal con As Control)
  Public Event Aircraft_Name_Changed(ByVal con As Control, ByVal FillModel As Boolean)
  Public Event company_name_changed(ByVal con As Control)
  Public Event company_searchClick(ByVal con As Control)
  Public Event contact_name_changed(ByVal con As Control)
  Public Event FillCompanyDrop(ByVal con As Control)
  Public Event fill_drop(ByVal jetnet_ac As Integer, ByVal client_ac As Integer, ByVal jetnet_comp As Integer, ByVal client_comp As Integer, ByVal jetnet_contact As Integer, ByVal client_contact As Integer, ByVal jetnet_mod As Integer, ByVal client_mod As Integer, ByVal con As Control, ByVal FillModel As Boolean)
  Public Event edit_note(ByVal type As String, ByVal con As Control, ByVal idnum As Integer)
  Dim error_string As String = ""
  Public Event remove_note_ev(ByVal idnum As Integer, ByVal con As Control, ByVal type As String)


#Region "Page Events"
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Me.Visible Then
      Try

        aclsData_Temp = New clsData_Manager_SQL

        aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
        aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase") 'CApplication.Item("crmJetnetDatabase")


        aclsData_Temp.class_error = ""


        Dim parent As Integer = Session.Item("Listing")
        Dim aError As String = ""
        Dim source As String = Session.Item("ListingSource")
        If Session.Item("isMobile") = True Then
          ' mobile_view.Visible = True
          'regular_view.Visible = False
          notes_edit.Width = 220
          contact_related.Width = 300
          company_name.Width = 300
          aircraft_name.Width = 300
          contact_name.Width = 300
          'notes_title.Width = 220
          'mobile_close.Text = "<img src=""images/cancel.gif"" alt=""Cancel"" border=""0""  onClick='history.go(-1)'/>"
        End If

        If Not Page.IsPostBack Then
          Select Case Trim(Request("action"))
            Case "edit" 'Edit Mode for Notes. 

              Dim idnum As Integer = Trim(Request("id"))
              removeNoteLB.Visible = True
              'edit_table.Visible = True
              aTempTable = aclsData_Temp.Get_Local_Notes_Client_NoteID(idnum)
              If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then

                  If Not IsDBNull(aTempTable.Rows(0).Item("lnote_schedule_start_date")) Then
                    If IsDate(aTempTable.Rows(0).Item("lnote_schedule_start_date")) Then
                      dated.Text = FormatDateTime(aTempTable.Rows(0).Item("lnote_schedule_start_date"), 2)
                    End If
                  End If
                  If Not IsDBNull(aTempTable.Rows(0).Item("lnote_cash_value")) Then

                    opp_cash.Text = aTempTable.Rows(0).Item("lnote_cash_value")

                  End If
                  If Not IsDBNull(aTempTable.Rows(0).Item("lnote_capture_percentage")) Then

                    capt_per.SelectedValue = aTempTable.Rows(0).Item("lnote_capture_percentage")

                  End If
                  If Not IsDBNull(aTempTable.Rows(0).Item("lnote_opportunity_status")) Then
                    ' If IsDate(aTempTable.Rows(0).Item("lnote_opportunity_status")) Then
                    opp_status.SelectedValue = aTempTable.Rows(0).Item("lnote_opportunity_status")
                    'End If
                  End If
                  If Not IsDBNull(aTempTable.Rows(0).Item("lnote_note")) Then
                    If InStr(HttpUtility.HtmlDecode(aTempTable.Rows(0).Item("lnote_note")), " ::: ") Then
                      Dim text As Array = Split(HttpUtility.HtmlDecode(aTempTable.Rows(0).Item("lnote_note")), " ::: ")
                      notes_edit.Text = text(1)
                      notes_title.Text = text(0)
                    Else
                      notes_edit.Text = HttpUtility.HtmlDecode(aTempTable.Rows(0).Item("lnote_note"))
                    End If
                  End If
                  jetnet_ac.Text = aTempTable.Rows(0).Item("lnote_jetnet_ac_id")
                  client_ac.Text = aTempTable.Rows(0).Item("lnote_client_ac_id")
                  jetnet_comp.Text = aTempTable.Rows(0).Item("lnote_jetnet_comp_id")
                  client_comp.Text = aTempTable.Rows(0).Item("lnote_client_comp_id")
                  client_contact.Text = aTempTable.Rows(0).Item("lnote_client_contact_id")
                  jetnet_contact.Text = aTempTable.Rows(0).Item("lnote_jetnet_contact_id")
                  client_mod.Text = aTempTable.Rows(0).Item("lnote_client_amod_id")
                  jetnet_mod.Text = aTempTable.Rows(0).Item("lnote_jetnet_amod_id")



                  RaiseEvent fill_drop(aTempTable.Rows(0).Item("lnote_jetnet_ac_id"), aTempTable.Rows(0).Item("lnote_client_ac_id"), aTempTable.Rows(0).Item("lnote_jetnet_comp_id"), aTempTable.Rows(0).Item("lnote_client_comp_id"), aTempTable.Rows(0).Item("lnote_jetnet_contact_id"), aTempTable.Rows(0).Item("lnote_client_contact_id"), aTempTable.Rows(0).Item("lnote_jetnet_amod_id"), aTempTable.Rows(0).Item("lnote_client_amod_id"), Me, False)
                  notes_opp.SelectedValue = aTempTable.Rows(0).Item("lnote_notecat_key")
                  If Not IsDBNull(aTempTable.Rows(0).Item("lnote_user_id")) Then
                    pertaining_to.SelectedValue = aTempTable.Rows(0).Item("lnote_user_id")

                  End If


                  If Session.Item("localUser").crmUserType = eUserTypes.RESEARCH Then
                    add_noteLB.Visible = False
                  End If

                End If
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  clsGeneral.clsGeneral.LogError("Opportunities.ascx.vb - Page_Load() - " & error_string, aclsData_Temp)
                End If
              End If
            Case "new"
              Dim jetnet_ac_id As Integer = 0
              Dim client_ac_id As Integer = 0
              Dim jetnet_comp_id As Integer = 0
              Dim client_comp_id As Integer = 0
              Dim jetnet_contact_id As Integer = 0
              Dim client_contact_id As Integer = 0
              Dim jetnet_mod_id As Integer = 0
              Dim client_mod_id As Integer = 0
              Select Case Session.Item("Listing")
                Case 1 'Company
                  Select Case Session.Item("ListingSource")
                    Case "JETNET"
                      jetnet_comp_id = Session.Item("ListingID")
                      jetnet_comp.Text = Session.Item("ListingID")
                      If Session.Item("OtherID") <> 0 Then
                        client_comp_id = Session.Item("OtherID")
                      End If
                    Case "CLIENT"
                      client_comp_id = Session.Item("ListingID")
                      If Session.Item("OtherID") <> 0 Then
                        jetnet_comp_id = Session.Item("OtherID")
                      End If
                  End Select
                  If Session.Item("ContactID") <> 0 Then
                    Select Case Session.Item("ListingSource")
                      Case "JETNET"
                        jetnet_contact_id = Session.Item("ContactID")
                        'We need to get the client contact ID if it exists for this!
                        Dim atemptable = aclsData_Temp.GetContactInfo_JETNET_ID(Session.Item("ContactID"), "Y")
                        If Not IsNothing(atemptable) Then
                          If atemptable.rows.count > 0 Then
                            client_contact_id = atemptable.rows(0).item("contact_id")
                          End If
                        End If

                      Case "CLIENT"
                        client_contact_id = Session.Item("ContactID")
                        Dim atemptable = aclsData_Temp.GetContacts_Details(Session.Item("ContactID"), "CLIENT")
                        If Not IsNothing(atemptable) Then
                          If atemptable.rows.count > 0 Then
                            jetnet_contact_id = atemptable.rows(0).item("contact_jetnet_contact_id")
                          End If
                        End If
                    End Select
                  End If
                Case 3 'Aircraft
                  Select Case Session.Item("ListingSource")
                    Case "JETNET"
                      jetnet_ac_id = Session.Item("ListingID")
                      If Session.Item("OtherID") <> 0 Then
                        client_ac_id = Session.Item("OtherID")
                      End If
                    Case "CLIENT"
                      client_ac_id = Session.Item("ListingID")
                      If Session.Item("OtherID") <> 0 Then
                        jetnet_ac_id = Session.Item("OtherID")
                      End If
                  End Select
              End Select
              RaiseEvent fill_drop(jetnet_ac_id, client_ac_id, jetnet_comp_id, client_comp_id, jetnet_contact_id, client_contact_id, jetnet_mod_id, client_mod_id, Me, False)
              jetnet_ac.Text = jetnet_ac_id
              client_ac.Text = client_ac_id
              jetnet_comp.Text = jetnet_comp_id
              client_comp.Text = client_comp_id
              jetnet_contact.Text = jetnet_contact_id
              client_contact.Text = client_contact_id
              AC_Search_Vis.Visible = False
              ac_search.Visible = True
              company_search_vis.Visible = True
              company_related.Visible = False
              Try
                pertaining_to.SelectedValue = Session.Item("localUser").crmLocalUserID
              Catch
              End Try

          End Select

          If Session.Item("localUser").crmUserType = eUserTypes.RESEARCH Then
            removeNoteLB.Visible = False
          End If
        End If
      Catch ex As Exception
        error_string = "Opportunities.ascx.vb - Page_Load() - " & ex.Message
        clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
      End Try
    End If
  End Sub
#End Region
#Region "Deals with dropdown changing, search style change"
  Private Sub AC_Search_Vis_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AC_Search_Vis.Click
    Try
      ac_search.Visible = True
      AC_Search_Vis.Visible = False
      aircraft_related.Visible = False
      company_search.Visible = False
      company_search_vis.Visible = True
    Catch ex As Exception
      error_string = "Opportunities.ascx.vb - AC_Search_Vis_Click() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Private Sub company_search_vis_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles company_search_vis.Click
    Try
      company_search.Visible = True
      company_search_vis.Visible = False
      ac_search.Visible = False
      ac_search_text.Visible = True
      AC_Search_Vis.Visible = True
    Catch ex As Exception
      error_string = "Opportunities.ascx.vb - company_search_vis_Click() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Private Sub contact_search_vis_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles contact_search_vis.Click
    Try
      contact_search.Visible = True
      contact_search_vis.Visible = False
    Catch ex As Exception
      error_string = "Opportunities.ascx.vb - company_search_vis_Click() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Private Sub ac_search_button_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ac_search_button.Click
    Try
      ac_search.Visible = True
      AC_Search_Vis.Visible = False
      aircraft_related.Visible = False
      RaiseEvent ac_searchClick(Me)
    Catch ex As Exception
      error_string = "Opportunities.ascx.vb - ac_search_button_Click() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Private Sub company_search_button_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles company_search_button.Click
    Try
      company_search.Visible = True
      company_search_vis.Visible = False
      RaiseEvent company_searchClick(Me)
    Catch ex As Exception
      error_string = "Opportunities.ascx.vb - company_search_button_Click() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Private Sub company_name_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles company_name.SelectedIndexChanged
    Try
      'company_search.Visible = True
      'company_search_vis.Visible = False
      RaiseEvent company_name_changed(Me)
    Catch ex As Exception
      error_string = "Opportunities.ascx.vb - company_name_SelectedIndexChanged() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Private Sub contact_name_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles contact_name.SelectedIndexChanged
    Try
      If company_search.Visible = True Then
        company_search.Visible = True
        company_search_vis.Visible = False
      End If

      RaiseEvent contact_name_changed(Me)
    Catch ex As Exception
      error_string = "Opportunities.ascx.vb - contact_name_SelectedIndexChanged() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Private Sub aircraft_related_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles aircraft_related.CheckedChanged
    Try
      If aircraft_related.Checked = True Then
        ac_search.Visible = False
      Else
        ac_search.Visible = True
      End If
    Catch ex As Exception
      error_string = "Opportunities.ascx.vb - aircraft_related_CheckedChanged() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Private Sub contact_related_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles contact_related.CheckedChanged
    Try
      If contact_related.Checked = True Then
        contact_search.Visible = False
      Else
        contact_search.Visible = True
      End If
    Catch ex As Exception
      error_string = "Opportunities.ascx.vb - contact_related_CheckedChanged() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Private Sub company_related_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles company_related.CheckedChanged
    Try
      If company_related.Checked = True Then
        company_search.Visible = False
      Else
        company_search.Visible = True
      End If
    Catch ex As Exception
      error_string = "Opportunities.ascx.vb - company_related_CheckedChanged() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Private Sub aircraft_name_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles aircraft_name.SelectedIndexChanged
    Try
      If ac_search.Visible = True Then
        ac_search.Visible = True
        AC_Search_Vis.Visible = False
        aircraft_related.Visible = False
      End If
      RaiseEvent Aircraft_Name_Changed(Me, False)
    Catch ex As Exception
      error_string = "Opportunities.ascx.vb - aircraft_name_SelectedIndexChanged() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
#End Region
#Region "Save Opportunity"
  Private Sub add_note_Click() Handles add_noteLB.Click
    Try
      Select Case Trim(Request("action"))
        Case "edit"
          If Session.Item("localUser").crmUserType <> eUserTypes.RESEARCH Then
            Dim idnum As Integer = Trim(Request("id"))
            RaiseEvent edit_note("opportunity", Me, idnum)
            If Session.Item("isMobile") = True Then
              If Session.Item("Listing") = 11 Then
                Response.Redirect("Mobile_Listing.aspx?type=11", False)
              Else
                Response.Redirect("Mobile_Details.aspx?type=" & Session.Item("Listing") & "&comp_id=" & Session.Item("ListingID") & "&source=" & Session.Item("ListingSource") & "&edited=opportunities", False)
              End If
            End If
          End If
        Case "new"
          RaiseEvent edit_note("opportunity", Me, 0)
          If Session.Item("isMobile") = True Then
            If Session.Item("Listing") = 11 Then
              Response.Redirect("Mobile_Listing.aspx?type=11", False)
            Else
              Response.Redirect("Mobile_Details.aspx?type=" & Session.Item("Listing") & "&comp_id=" & Session.Item("ListingID") & "&source=" & Session.Item("ListingSource") & "&edited=opportunities", False)
            End If
          End If
      End Select
    Catch ex As Exception
      error_string = "Opportunities.ascx.vb - add_note_click() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
#End Region

  ''' <summary>
  ''' Used for asp.net validator to check and make sure date is okay
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="args"></param>
  ''' <remarks></remarks>
  Public Sub checkDate(ByVal sender As Object, ByVal args As ServerValidateEventArgs)

    If args.Value > Now() Then
      args.IsValid = False
      Exit Sub
    End If
    args.IsValid = True
  End Sub


  ''' <summary>
  ''' Remove Note Click Function. This button click raises an event found on edit_note.aspx. Basically removes the note and remnants
  ''' </summary>
  ''' <remarks></remarks>
  Private Sub remove_note_Click() Handles removeNoteLB.Click
    If Session.Item("localUser").crmUserType <> eUserTypes.RESEARCH Then
      Dim idnum As Integer = 0
      Try
        idnum = Trim(Request("id"))
      Catch ex As Exception
        error_string = "Opportunities.ascx.vb - remove_note_Click() - " & ex.Message
        clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
      End Try
      RaiseEvent remove_note_ev(idnum, Me, "opportunities")
    End If
  End Sub

  Private Sub visible_all_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles visible_all.Click
    If notes_opp.Visible = False Then
      notes_opp.Visible = True
      cat_name.Visible = False
      cat_insert.Visible = False
      category_edit.Visible = True
      visible_all.Text = "Add Category"
    Else
      cat_insert.Visible = True
      category_edit.Visible = False
      notes_opp.Visible = False
      cat_name.Visible = True
      visible_all.Text = "Cancel"
    End If
  End Sub


  Private Sub cat_insert_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cat_insert.Click
    If cat_name.Text <> "" Then

      If aclsData_Temp.Insert_Opportunity_Categories(cat_name.Text) = 1 Then
        attention.Text = "<p align='left'>Your category has been added.</p>"
        cat_name.Visible = False
        notes_opp.Visible = True
        notes_opp.Items.Clear()
        cat_name.Visible = False
        cat_insert.Visible = False
        category_edit.Visible = True
        visible_all.Text = "Add Category"

        'rebind the roll
        If Not IsNothing(Name.FindControl("notes_opp")) Then
          clsGeneral.clsGeneral.Fill_Opportunity_Category(Name.FindControl("notes_opp"), aTempTable, aclsData_Temp)
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          clsGeneral.clsGeneral.LogError("edit_note.ascx.vb -Fill_All_DropDowns() - " & error_string, aclsData_Temp)
        End If
      End If
    End If
  End Sub

  Private Sub FillOppCatEdit()
    Dim TemporaryTable As New DataTable
    TemporaryTable = aclsData_Temp.Get_Opportunity_Categories()
    If Not IsNothing(TemporaryTable) Then
      If TemporaryTable.Rows.Count > 0 Then
        datagrid_details.DataSource = TemporaryTable
        datagrid_details.DataBind()
      End If
    End If
  End Sub

  Private Sub category_edit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles category_edit.Click
    FillOppCatEdit()
    attention.Text = ""
    oppPanel.Visible = False
    categoryEditTable.Visible = True
  End Sub

  Private Sub ResumeOpportunities_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ResumeOpportunities.Click
    oppPanel.Visible = True
    categoryEditTable.Visible = False

    notes_opp.Visible = True
    notes_opp.Items.Clear()
    'rebind the roll
    If Not IsNothing(Name.FindControl("notes_opp")) Then
      clsGeneral.clsGeneral.Fill_Opportunity_Category(Name.FindControl("notes_opp"), aTempTable, aclsData_Temp)
    End If
  End Sub

#Region "Datagrid Events"
  Public Sub MyDataGrid_Update(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
    Try
      Dim id As TextBox = e.Item.FindControl("id")
      Dim name As TextBox = e.Item.FindControl("name")

      If aclsData_Temp.Update_Opportunity_Category(name.Text, id.Text) = 1 Then
        datagrid_details.EditItemIndex = -1
        FillOppCatEdit()
        opp_updated.Text = "<p align=""center"">Your information has been edited.</p>"
      End If
    Catch ex As Exception
      error_string = "Opportunities.ascx.vb - MyDataGrid_Update() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Public Sub MyDataGrid_Delete(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
    Try
      Dim id As TextBox = e.Item.FindControl("id")

      Dim ac_hidden As TextBox = e.Item.FindControl("ac_hidden")

      If aclsData_Temp.Delete_Client_Aircraft_Details(id.Text, ac_hidden.Text) = 1 Then
        FillOppCatEdit()
        opp_updated.Text = "<p align=""center"">Your information has been removed.</p>"
      End If
    Catch ex As Exception
      error_string = "Opportunities.ascx.vb - MyDataGrid_Delete() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Public Sub MyDataGrid_Cancel(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
    Try
      datagrid_details.EditItemIndex = -1
      FillOppCatEdit()

      opp_updated.Text = ""
    Catch ex As Exception
      error_string = "Opportunities.ascx.vb - MyDataGrid_Cancel() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Sub MyDataGrid_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
    Try
      datagrid_details.EditItemIndex = CInt(E.Item.ItemIndex)
      FillOppCatEdit()
      opp_updated.Text = ""
    Catch ex As Exception
      error_string = "Opportunities.ascx.vb - MyDataGrid_Edit() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
#End Region
 

End Class