Partial Public Class Email
  Inherits System.Web.UI.UserControl
  Dim aclsData_Temp As New Object
  Dim atemptable As New DataTable
  Dim error_string As String
  Public Event fill_drop(ByVal jetnet_ac As Integer, ByVal client_ac As Integer, ByVal jetnet_comp As Integer, ByVal client_comp As Integer, ByVal jetnet_contact As Integer, ByVal client_contact As Integer, ByVal jetnet_mod As Integer, ByVal client_mod As Integer, ByVal con As Control, ByVal FillModel As Boolean)
  Public Event edit_note(ByVal type As String, ByVal con As Control, ByVal idnum As Integer)
  Public Event Aircraft_Name_Changed(ByVal con As Control, ByVal FillModel As Boolean)
  Public Event company_name_changed(ByVal con As Control)
  Public Event contact_name_changed(ByVal con As Control)
  Public Event FillCompanyDrop(ByVal con As Control)
  Public Event ac_searchClick(ByVal con As Control)
  Public Event company_searchClick(ByVal con As Control)
  Public Event remove_note_ev(ByVal idnum As Integer, ByVal con As Control, ByVal type As String)

#Region "Page Load"
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Me.Visible Then
      Try

        aclsData_Temp = New clsData_Manager_SQL
        aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
        aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase") 'CApplication.Item("crmJetnetDatabase")
        aclsData_Temp.class_error = ""
        'A function that will set the other ids on Company/Contact.
        If Not Page.IsPostBack Then
          clsGeneral.clsGeneral.Set_IDS(aclsData_Temp)
        End If
        If Session.Item("isMobile") = True Then
          resize_function.Text = "<script type=""text/javascript"">function FitPic() { };</script>"
          mobile_view.Visible = True
          regular_view.Visible = False
          notes_edit.Width = 320
          contact_related.Width = 300
          mobile_style.Visible = True
          company_name.Width = 300
          aircraft_name.Width = 300
          body.Width = 300
          If Session.Item("Listing") = 1 Then
            mobile_close.Text = "<a href='mobile_details.aspx?type=1&comp_ID=" & Session.Item("ListingID") & "'><img src=""images/cancel.gif"" alt=""Cancel"" border=""0""/></a>"
          Else
            mobile_close.Text = "<a href='mobile_details.aspx?type=3&ac_ID=" & Session.Item("ListingID") & "'><img src=""images/cancel.gif"" alt=""Cancel"" border=""0""/></a>"
          End If
        End If

        'file_upload_area.Visible = True
        'If Session.Item("localSubscription").crmDocumentsFlag = True Then
        '  'file_upload_area.Visible = True
        '  store_document.Enabled = True
        'Else
        '  store_document.Enabled = False
        '  store_document.Checked = False
        '  store_document.ForeColor = Drawing.Color.LightGray

        'End If
        'Grabbing the source of what we're viewing. 
        Dim source As String = Session.Item("ListingSource")
        If Not Page.IsPostBack Then 'Make sure no post back issues. 
          Select Case Trim(Request("action")) 'Request URL variable that determines whether we're editing or viewing a note.
            Case "edit" 'Edit Mode for Notes. 
              Dim idnum As Integer = Trim(Request("id")) 'the ID of the note being edited. 
              atemptable = aclsData_Temp.Get_Local_Notes_Client_NoteID(idnum) 'Get all of the information of the note. 
              If Not IsNothing(atemptable) Then
                If atemptable.Rows.Count > 0 Then
                  notes_cat.SelectedValue = IIf(Not IsDBNull(atemptable.Rows(0).Item("lnote_notecat_key")), atemptable.Rows(0).Item("lnote_notecat_key"), 0)


                  Dim info As Array = Split(HttpUtility.HtmlDecode(atemptable.Rows(0).Item("lnote_note")), ":::")

                  If Not IsNothing(info(0)) Then
                    email_to.Text = info(0)
                    email_to.Enabled = False
                  End If
                  If Not IsNothing(info(1)) Then
                    email_cc.Text = info(1)
                    email_cc.Enabled = False
                  End If
                  If Not IsNothing(info(2)) Then
                    email_subject.Text = info(2)
                    email_subject.Enabled = False
                  End If
                  If Not IsNothing(info(3)) Then
                    body.Content = info(3)
                    body.Enabled = False
                  End If
                  If Not IsNothing(info(4)) Then
                    notes_edit.Text = info(4)
                    notes_edit.Enabled = False
                  End If
                  file_upload_area.Visible = False
                  If Not IsDBNull(atemptable.Rows(0).Item("lnote_document_name")) Then
                    If atemptable.Rows(0).Item("lnote_document_name") <> "" Then
                      existing_docs.Text = clsGeneral.clsGeneral.DisplayDocuments(atemptable.Rows(0).Item("lnote_document_name"), atemptable.Rows(0).Item("lnote_document_flag"), True, atemptable.Rows(0).Item("lnote_id"))
                      existing.Visible = True
                    Else
                      existing.Visible = False
                    End If
                  End If

                  'Just setting some of the textboxes/dropdown lists to be disabled so we cannot change them. 
                  company_name.Enabled = False
                  aircraft_name.Enabled = False
                  contact_name.Enabled = False
                  aircraft_related.Enabled = False
                  company_related.Enabled = False
                  notes_cat.Enabled = False
                  add_note.Visible = False
                  email_from_bcc.Visible = False
                  'Filling up all the IDS of the jetnet/client objects based on what's stored in the note ID. 
                  jetnet_ac.Text = atemptable.Rows(0).Item("lnote_jetnet_ac_id")
                  client_ac.Text = atemptable.Rows(0).Item("lnote_client_ac_id")
                  jetnet_comp.Text = atemptable.Rows(0).Item("lnote_jetnet_comp_id")
                  client_comp.Text = atemptable.Rows(0).Item("lnote_client_comp_id")
                  client_contact.Text = atemptable.Rows(0).Item("lnote_client_contact_id")
                  jetnet_contact.Text = atemptable.Rows(0).Item("lnote_jetnet_contact_id")
                  client_mod.Text = atemptable.Rows(0).Item("lnote_client_amod_id")
                  jetnet_mod.Text = atemptable.Rows(0).Item("lnote_jetnet_amod_id")
                  'email from is being shown as the local user class email address.
                  email_from.Text = Session.Item("localUser").crmLocalUserEmailAddress
                  'Fill all these dropdowns. This event fills all the dropdowns. Contact/Company/Aircraft.
                  'Same event runs for Action Items/Documents/Notes/Emails.
                  RaiseEvent fill_drop(atemptable.Rows(0).Item("lnote_jetnet_ac_id"), atemptable.Rows(0).Item("lnote_client_ac_id"), atemptable.Rows(0).Item("lnote_jetnet_comp_id"), atemptable.Rows(0).Item("lnote_client_comp_id"), atemptable.Rows(0).Item("lnote_jetnet_contact_id"), atemptable.Rows(0).Item("lnote_client_contact_id"), atemptable.Rows(0).Item("lnote_jetnet_amod_id"), atemptable.Rows(0).Item("lnote_client_amod_id"), Me, False)
                  'Select the category. 
                  notes_cat.SelectedValue = atemptable.Rows(0).Item("lnote_notecat_key")
                  remove_note.Visible = True

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
              'Setting up the IDs based on the company/aircraft we were just viewing. 
              Select Case Session.Item("Listing")
                Case 1 'Company
                  Select Case Session.Item("ListingSource")
                    Case "JETNET" 'Jetnet company
                      jetnet_comp_id = Session.Item("ListingID")
                      jetnet_comp.Text = Session.Item("ListingID")
                      If Session.Item("OtherID") <> 0 Then
                        client_comp_id = Session.Item("OtherID")
                      End If
                    Case "CLIENT" 'client company
                      client_comp_id = Session.Item("ListingID")
                      If Session.Item("OtherID") <> 0 Then
                        jetnet_comp_id = Session.Item("OtherID")
                      End If
                  End Select
                  'This happens if a contact ID is filled out. 
                  If Session.Item("ContactID") <> 0 Then 'unfortunatley we have to poll the database once again
                    'to figure out the contact id for the other side (if it exists)
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
                Case 3 'Aircraft IDs are being set. 
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
              'Same dropdown event that runs on the add new note. 
              RaiseEvent fill_drop(jetnet_ac_id, client_ac_id, jetnet_comp_id, client_comp_id, jetnet_contact_id, client_contact_id, jetnet_mod_id, client_mod_id, Me, False)
              'setting up IDs in text box. 
              jetnet_ac.Text = jetnet_ac_id
              client_ac.Text = client_ac_id
              jetnet_comp.Text = jetnet_comp_id
              client_comp.Text = client_comp_id
              jetnet_contact.Text = jetnet_contact_id
              client_contact.Text = client_contact_id
              If Session.Item("Listing") = 1 Then
                'This toggles the visibility and disables some of the company/contact dropdowns
                'they're already picked, don't let them select something new. 
                company_search_vis.Visible = False
                company_related.Visible = False
                company_name.Enabled = False
                company_related.Enabled = False
                contact_name.Enabled = False
                AC_Search_Vis.Visible = True 'You however can search for an AC if the ac isn't picked.
              Else
                add_note.Visible = False 'You cannot send email until you pick a company or a contact.
                'If the email is just set on an aircraft, there's no email address associated with it.
                AC_Search_Vis.Visible = False 'You cannot however search for an AC if the ac is picked.
                aircraft_name.Enabled = False
                aircraft_related.Enabled = False
              End If
              'fill in from label with user class email address.
              email_from.Text = Session.Item("localUser").crmLocalUserEmailAddress
              'changing image to send email image. 
              add_note.ImageUrl = "~/images/send_email.jpg"

          End Select
          If Session.Item("localUser").crmUserType = eUserTypes.RESEARCH Then
            remove_note.Visible = False
          End If
        End If
      Catch ex As Exception
        error_string = "Email.ascx.vb - Page_Load() - " & ex.Message
        clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
      End Try
    End If
  End Sub
#End Region
#Region "Save Note"
  ''' <summary>
  ''' Add Note Button Click calls add note in edit_note.aspx
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub add_note_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles add_note.Click
    Try
      If body.Content <> "" Then
        If Not (Page.IsValid) Then
          'So this way if the page doesn't validate, it doesn't run. 
        Else
          Select Case Trim(Request("action")) 'only save new email, well send it and save it. 
            Case "new" 'new case
              RaiseEvent edit_note("email", Me, 0) 'raise event on edit_note.aspx.vb page. 
              If Session.Item("isMobile") = True Then 'If this is taking place on mobile version, redirect them here.
                Response.Redirect("Mobile_Details.aspx?type=" & Session.Item("Listing") & "&comp_id=" & Session.Item("ListingID") & "&source=" & Session.Item("ListingSource") & "&added=note", False)
              End If
          End Select
        End If
      Else
        return_error.Text = "<p align='center'>Please enter Email Body Text.</p>"
      End If

    Catch ex As Exception
      error_string = "Email.ascx.vb - add_note_Click() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
#End Region
#Region "Deals with dropdown changing, visibility changing based on search type"
  ''' <summary>
  ''' Related to company Aircraft toggle
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub aircraft_related_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles aircraft_related.CheckedChanged
    Try
      If aircraft_related.Checked = True Then
        ac_search.Visible = False
      Else
        ac_search.Visible = True
      End If
    Catch ex As Exception
      error_string = "Email.ascx.vb - aircraft_related_CheckedChanged() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  ''' <summary>
  ''' Toggles contact search - search inactive for now
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub contact_related_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles contact_related.CheckedChanged
    Try
      If contact_related.Checked = True Then
        contact_search.Visible = False
      Else
        contact_search.Visible = True
      End If
    Catch ex As Exception
      error_string = "Email.ascx.vb - contact_related_CheckedChanged() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  ''' <summary>
  ''' Company related to aircraft toggle search
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub company_related_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles company_related.CheckedChanged
    Try
      If company_related.Checked = True Then
        company_search.Visible = False
      Else
        company_search.Visible = True
      End If
    Catch ex As Exception
      error_string = "Email.ascx.vb - company_related_CheckedChanged() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  ''' <summary>
  ''' event on selected index change of aircraft name
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub aircraft_name_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles aircraft_name.SelectedIndexChanged
    Try
      If ac_search.Visible = True Then
        ac_search.Visible = True
        AC_Search_Vis.Visible = False
        aircraft_related.Visible = False
      End If
      RaiseEvent Aircraft_Name_Changed(Me, False)
    Catch ex As Exception
      error_string = "Email.ascx.vb - aircraft_name_SelectedIndexChanged() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  ''' <summary>
  ''' toggle ac search
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub AC_Search_Vis_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AC_Search_Vis.Click
    Try
      ac_search.Visible = True
      AC_Search_Vis.Visible = False
      aircraft_related.Visible = False
      company_search.Visible = False
      company_search_vis.Visible = True
    Catch ex As Exception
      error_string = "Email.ascx.vb - AC_Search_Vis_Click() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  ''' <summary>
  ''' toggle company search
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub company_search_vis_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles company_search_vis.Click
    Try
      company_search.Visible = True
      company_search_vis.Visible = False
      ac_search.Visible = False
      ac_search_text.Visible = True
      AC_Search_Vis.Visible = True
    Catch ex As Exception
      error_string = "Email.ascx.vb - company_search_vis_Click() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  ''' <summary>
  ''' toggle contact search
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub contact_search_vis_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles contact_search_vis.Click
    Try
      contact_search.Visible = True
      contact_search_vis.Visible = False
    Catch ex As Exception
      error_string = "Email.ascx.vb - contact_search_vis_Click() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  ''' <summary>
  ''' event on ac search button click
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub ac_search_button_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ac_search_button.Click
    Try
      ac_search.Visible = True
      AC_Search_Vis.Visible = False
      aircraft_related.Visible = False
      RaiseEvent ac_searchClick(Me)
    Catch ex As Exception
      error_string = "Email.ascx.vb - ac_search_button_Click() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  ''' <summary>
  ''' event on company search button click
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub company_search_button_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles company_search_button.Click
    Try
      company_search.Visible = True
      company_search_vis.Visible = False
      RaiseEvent company_searchClick(Me)
    Catch ex As Exception
      error_string = "Email.ascx.vb - company_search_button_Click() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  ''' <summary>
  ''' company name selected index change
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub company_name_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles company_name.SelectedIndexChanged
    Try
      company_search.Visible = False
      company_search_vis.Visible = True
      RaiseEvent company_name_changed(Me)
    Catch ex As Exception
      error_string = "Email.ascx.vb - company_name_SelectedIndexChanged() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  ''' <summary>
  ''' contact name selected index change
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub contact_name_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles contact_name.SelectedIndexChanged
    Try
      If company_search.Visible = True Then
        company_search.Visible = True
        company_search_vis.Visible = False
      End If

      RaiseEvent contact_name_changed(Me)
    Catch ex As Exception
      error_string = "Email.ascx.vb - contact_name_SelectedIndexChanged() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
#End Region
  ''' <summary>
  ''' Remove Note Click Function. This button click raises an event found on edit_note.aspx. Basically removes the note and remnants
  ''' </summary>
  ''' <param name="sender">button</param>
  ''' <param name="e">button args</param>
  ''' <remarks></remarks>
  Private Sub remove_note_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles remove_note.Click
    If Session.Item("localUser").crmUserType <> eUserTypes.RESEARCH Then
      Dim idnum As Integer = 0
      Try
        idnum = Trim(Request("id"))
      Catch ex As Exception
        error_string = "Email.ascx.vb - remove_note_Click() - " & ex.Message
        clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
      End Try
      RaiseEvent remove_note_ev(idnum, Me, "email")
    End If
  End Sub

End Class