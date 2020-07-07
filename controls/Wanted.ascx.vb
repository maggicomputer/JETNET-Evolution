Partial Public Class Wanted
  Inherits System.Web.UI.UserControl
  Dim aclsData_Temp As New Object
  Dim aTempTable, aTempTable2 As New DataTable 'Data Tables used
  Public Event Aircraft_Name_Changed(ByVal con As Control, ByVal FillModel As Boolean)
  Public Event company_name_changed(ByVal con As Control)
  Public Event contact_name_changed(ByVal con As Control)
  Public Event FillCompanyDrop(ByVal con As Control)
  Public Event ac_searchClick(ByVal con As Control)
  Public Event company_searchClick(ByVal con As Control)
  Public Event fill_drop(ByVal jetnet_ac As Integer, ByVal client_ac As Integer, ByVal jetnet_comp As Integer, ByVal client_comp As Integer, ByVal jetnet_contact As Integer, ByVal client_contact As Integer, ByVal jetnet_mod As Integer, ByVal client_mod As Integer, ByVal con As Control, ByVal FillModel As Boolean)
  Public Event edit_note(ByVal type As String, ByVal con As Control, ByVal idnum As Integer)
  Public Event remove_note_ev(ByVal idnum As Integer, ByVal con As Control, ByVal type As String)
  Dim error_string As String = ""

#Region "Page Events"
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Me.Visible Then
      Try

        aclsData_Temp = New clsData_Manager_SQL

        If Session.Item("isMobile") = True Then
          'mobile_view.Visible = True
          'regular_view.Visible = False
          notes_edit.Width = 320
          contact_related.Width = 300
          mobile_style.Visible = True
          company_name.Width = 300
          aircraft_name.Width = 300
          'mobile_close.Text = "<img src=""images/cancel.gif"" alt=""Cancel"" border=""0""  onClick='history.go(-1)'/>"
        End If

        aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
        aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase") 'CApplication.Item("crmJetnetDatabase")
        aclsData_Temp.class_error = ""

        If Not Page.IsPostBack Then
          clsGeneral.clsGeneral.Set_IDS(aclsData_Temp)
        End If

        Dim source As String = Session.Item("ListingSource")



        If Not Page.IsPostBack Then

          clsGeneral.clsGeneral.Year_Range_DropDownFill(wanted_year_start, 1975, 2015)
          clsGeneral.clsGeneral.Year_Range_DropDownFill(wanted_year_end, 1975, 2015)

          Select Case Trim(Request("action"))
            Case "edit" 'Edit Mode for Notes. 
              upload_area.Visible = False
              Dim idnum As Integer = Trim(Request("id"))

              edit_table.Visible = True
              removeNoteLB.Visible = True


              aTempTable = aclsData_Temp.Get_Local_Notes_Client_NoteID(idnum)
              If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then
                  notes_cat.SelectedValue = aTempTable.Rows(0).Item("lnote_notecat_key")
                  notes_edit.Text = HttpUtility.HtmlDecode(aTempTable.Rows(0).Item("lnote_note"))
                  jetnet_ac.Text = aTempTable.Rows(0).Item("lnote_jetnet_ac_id")
                  client_ac.Text = aTempTable.Rows(0).Item("lnote_client_ac_id")
                  jetnet_comp.Text = aTempTable.Rows(0).Item("lnote_jetnet_comp_id")
                  client_comp.Text = aTempTable.Rows(0).Item("lnote_client_comp_id")
                  client_contact.Text = aTempTable.Rows(0).Item("lnote_client_contact_id")
                  jetnet_contact.Text = aTempTable.Rows(0).Item("lnote_jetnet_contact_id")
                  client_mod.Text = aTempTable.Rows(0).Item("lnote_client_amod_id")
                  jetnet_mod.Text = aTempTable.Rows(0).Item("lnote_jetnet_amod_id")
                  wanted_year_start.SelectedValue = aTempTable.Rows(0).Item("lnote_wanted_start_year").ToString
                  wanted_year_end.SelectedValue = aTempTable.Rows(0).Item("lnote_wanted_end_year").ToString
                  wanted_damage_cur.Text = aTempTable.Rows(0).Item("lnote_wanted_damage_cur").ToString
                  wanted_damage_hist.Text = aTempTable.Rows(0).Item("lnote_wanted_damage_hist").ToString
                  wanted_max_aftt.Text = aTempTable.Rows(0).Item("lnote_wanted_max_aftt").ToString
                  wanted_max_price.Text = aTempTable.Rows(0).Item("lnote_wanted_max_price").ToString
                  note_date.Text = aTempTable.Rows(0).Item("lnote_schedule_start_date").ToString

                  'add_note.ImageUrl = "~/images/edit.jpg"

                  RaiseEvent fill_drop(aTempTable.Rows(0).Item("lnote_jetnet_ac_id"), aTempTable.Rows(0).Item("lnote_client_ac_id"), aTempTable.Rows(0).Item("lnote_jetnet_comp_id"), aTempTable.Rows(0).Item("lnote_client_comp_id"), aTempTable.Rows(0).Item("lnote_jetnet_contact_id"), aTempTable.Rows(0).Item("lnote_client_contact_id"), aTempTable.Rows(0).Item("lnote_jetnet_amod_id"), aTempTable.Rows(0).Item("lnote_client_amod_id"), Me, False)

                  notes_cat.SelectedValue = aTempTable.Rows(0).Item("lnote_notecat_key")
                  If Not IsDBNull(aTempTable.Rows(0).Item("lnote_user_id")) Then
                    Try
                      If aTempTable.Rows(0).Item("lnote_user_id") = 0 Then
                        pertaining_to.SelectedValue = Session.Item("localUser").crmLocalUserID
                      Else
                        pertaining_to.SelectedValue = aTempTable.Rows(0).Item("lnote_user_id")
                      End If
                    Catch
                    End Try
                  Else
                    pertaining_to.SelectedValue = Session.Item("localUser").crmLocalUserID
                  End If
                  Dim timed As String = ""
                  Try
                    Dim offset As Date = aTempTable.Rows(0).Item("lnote_entry_date")
                    offset = DateAdd("h", Session("timezone_offset"), offset)
                    timed = Format(offset, "HH:00")
                    time.SelectedValue = CStr(timed)
                  Catch ex As Exception
                    error_string = "Wanted.ascx.vb - Page_Load() - " & ex.Message
                    clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
                  End Try

                  note_date.Text = FormatDateTime(aTempTable.Rows(0).Item("lnote_entry_date"), 2)
                  curprev.SelectedValue = "P"
                  If Session.Item("localUser").crmUserType = eUserTypes.RESEARCH Then
                    add_noteLB.Visible = False
                  End If
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

              curprev.Items.Add(New ListItem("Current Date", "N"))
              curprev.SelectedValue = "N"
              current.Visible = True
              current.Text = Now()
              note_date.Visible = False
              note_date_image.Visible = False
              time.Visible = False
              time.SelectedValue = "09:00"
              AC_Search_Vis.Visible = False
              company_search_vis.Visible = True
              company_related.Visible = False
              removeNoteLB.Visible = False
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
        error_string = "Wanted.ascx.vb - Page_Load() - " & ex.Message
        clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
      End Try
    End If
  End Sub
#End Region
#Region "Deals with dropdown changing, visibility changing based on search type"
  Private Sub aircraft_related_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles aircraft_related.CheckedChanged
    Try
      If aircraft_related.Checked = True Then
        ac_search.Visible = False
      Else
        ac_search.Visible = True
      End If
    Catch ex As Exception
      error_string = "Wanted.ascx.vb - aircraft_related_CheckedChanged() - " & ex.Message
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
      error_string = "Wanted.ascx.vb - contact_related_CheckedChanged() - " & ex.Message
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
      error_string = "Wanted.ascx.vb - company_related_CheckedChanged() - " & ex.Message
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
      error_string = "Wanted.ascx.vb - aircraft_name_SelectedIndexChanged() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Private Sub AC_Search_Vis_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AC_Search_Vis.Click
    Try
      ac_search.Visible = True
      AC_Search_Vis.Visible = False
      aircraft_related.Visible = False
      company_search.Visible = False
      company_search_vis.Visible = True
    Catch ex As Exception
      error_string = "Wanted.ascx.vb - AC_Search_Vis_Click() - " & ex.Message
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
      error_string = "Wanted.ascx.vb - company_search_vis_Click() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Private Sub contact_search_vis_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles contact_search_vis.Click
    Try
      contact_search.Visible = True
      contact_search_vis.Visible = False
    Catch ex As Exception
      error_string = "Wanted.ascx.vb - contact_search_vis_Click() - " & ex.Message
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
      error_string = "Wanted.ascx.vb - ac_search_button_Click() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Private Sub company_search_button_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles company_search_button.Click
    Try
      company_search.Visible = True
      company_search_vis.Visible = False
      RaiseEvent company_searchClick(Me)
    Catch ex As Exception
      error_string = "Wanted.ascx.vb - company_search_button_Click() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Private Sub company_name_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles company_name.SelectedIndexChanged
    Try
      company_search.Visible = False
      company_search_vis.Visible = True
      RaiseEvent company_name_changed(Me)
    Catch ex As Exception
      error_string = "Wanted.ascx.vb - company_name_SelectedIndexChanged() - " & ex.Message
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
      error_string = "Wanted.ascx.vb - contact_name_SelectedIndexChanged() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
#End Region
#Region "Save Note"
  Private Sub add_note_Click() Handles add_noteLB.Click
    Try
      If Not (Page.IsValid) Then

      Else
        Select Case Trim(Request("action"))
          Case "edit"
            If Session.Item("localUser").crmUserType <> eUserTypes.RESEARCH Then
              Dim idnum As Integer = Trim(Request("id"))
              RaiseEvent edit_note("note", Me, idnum)
              If Session.Item("isMobile") = True Then
                Response.Redirect("Mobile_Details.aspx?type=" & Session.Item("Listing") & "&comp_id=" & Session.Item("ListingID") & "&source=" & Session.Item("ListingSource") & "&added=note", False)
              End If
            End If
          Case "new"
            RaiseEvent edit_note("note", Me, 0)
            If Session.Item("isMobile") = True Then
              Response.Redirect("Mobile_Details.aspx?type=" & Session.Item("Listing") & "&comp_id=" & Session.Item("ListingID") & "&source=" & Session.Item("ListingSource") & "&added=note", False)
            End If
        End Select
      End If


    Catch ex As Exception
      error_string = "Wanted.ascx.vb - add_note_Click() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
#End Region

  Private Sub curprev_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles curprev.SelectedIndexChanged
    If curprev.SelectedValue = "P" Then
      current.Visible = False
      note_date.Visible = True
      note_date_image.Visible = True
      time.Visible = True
      note_date.Text = FormatDateTime(Now(), 2)
      time.SelectedValue = Format(Now(), "HH:00")
      RequiredFieldValidator1.Enabled = True
    Else
      current.Visible = True
      current.Text = Now()
      note_date.Visible = False
      note_date_image.Visible = False
      time.Visible = False
      RequiredFieldValidator1.Enabled = False
    End If
  End Sub
  Public Sub checkDate(ByVal sender As Object, ByVal args As ServerValidateEventArgs)

    If args.Value > Now() Then
      args.IsValid = False
      Exit Sub
    End If
    args.IsValid = True
  End Sub


  Private Sub remove_note_Click() Handles removeNoteLB.Click
    If Session.Item("localUser").crmUserType <> eUserTypes.RESEARCH Then
      Dim idnum As Integer = 0
      Try
        idnum = Trim(Request("id"))
      Catch ex As Exception
        error_string = "Notes.ascx.vb - remove_note_Click() - " & ex.Message
        clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
      End Try
      RaiseEvent remove_note_ev(idnum, Me, "wanted")
    End If
  End Sub

End Class