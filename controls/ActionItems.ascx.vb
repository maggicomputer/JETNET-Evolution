Imports System.IO
Partial Public Class ActionItems
  Inherits System.Web.UI.UserControl
  Dim aclsData_Temp As New Object
  Dim aTempTable, aTempTable2 As New DataTable 'Data Tables used
  Dim error_string As String
  Public Event FillCompanyDrop(ByVal con As Control)
  Public Event ac_searchClick(ByVal con As Control)
  Public Event Aircraft_Name_Changed(ByVal con As Control, ByVal FillModel As Boolean)
  Public Event company_name_changed(ByVal con As Control)
  Public Event company_searchClick(ByVal con As Control)
  Public Event contact_name_changed(ByVal con As Control)
  Public Event fill_drop(ByVal jetnet_ac As Integer, ByVal client_ac As Integer, ByVal jetnet_comp As Integer, ByVal client_comp As Integer, ByVal jetnet_contact As Integer, ByVal client_contact As Integer, ByVal jetnet_mod As Integer, ByVal client_mod As Integer, ByVal con As Control, ByVal FillModel As Boolean)
  Public Event edit_note(ByVal type As String, ByVal con As Control, ByVal idnum As Integer)
  Public Event remove_note_ev(ByVal idnum As Integer, ByVal con As Control, ByVal type As String)



#Region "Page Load"
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Me.Visible Then

      aclsData_Temp = New clsData_Manager_SQL
      aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
      aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase") 'CApplication.Item("crmJetnetDatabase")
      aclsData_Temp.class_error = ""

      If Session.Item("isMobile") = True Then
        resize_function.Text = "<script type=""text/javascript"">function FitPic() { };</script>"
        'mobile_view.Visible = True
        'regular_view.Visible = False
        notes_edit.Width = 240
        contact_related.Width = 300
        company_name.Width = 300
        aircraft_name.Width = 300
        contact_name.Width = 300
        'mobile_close.Text = "<img src=""images/cancel.gif"" alt=""Cancel"" border=""0""  onClick='history.go(-1)'/>"
      End If

      Dim masterpage As New main_site
      Dim source As String = Session.Item("ListingSource")
      Dim parent As Integer = Session.Item("Listing")
      Dim aError As String = ""
      If Not Page.IsPostBack Then 'This function will set the other IDs so that they'll be filled correctly based on what's passed to the page. 
        clsGeneral.clsGeneral.Set_IDS(aclsData_Temp)
      End If

      If Not Page.IsPostBack Then
        Select Case Trim(Request("action"))
          Case "edit" 'Edit Mode for Notes. 
            Try
              upload_area.Visible = False
              Dim idnum As Integer = CInt(Trim(Request("id")))
              aTempTable = aclsData_Temp.Get_Local_Notes_Client_NoteID(idnum)
              If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then
                  If Not IsDBNull(aTempTable.Rows(0).Item("lnote_schedule_start_date")) Then
                    If IsDate(aTempTable.Rows(0).Item("lnote_schedule_start_date")) Then
                      dated.Text = FormatDateTime(aTempTable.Rows(0).Item("lnote_schedule_start_date"), 2)
                    End If
                  End If

                  notes_edit.Text = HttpUtility.HtmlDecode(aTempTable.Rows(0).Item("lnote_note"))
                  jetnet_ac.Text = aTempTable.Rows(0).Item("lnote_jetnet_ac_id")
                  client_ac.Text = aTempTable.Rows(0).Item("lnote_client_ac_id")
                  jetnet_comp.Text = aTempTable.Rows(0).Item("lnote_jetnet_comp_id")
                  client_comp.Text = aTempTable.Rows(0).Item("lnote_client_comp_id")
                  client_contact.Text = aTempTable.Rows(0).Item("lnote_client_contact_id")
                  jetnet_contact.Text = aTempTable.Rows(0).Item("lnote_jetnet_contact_id")
                  client_mod.Text = aTempTable.Rows(0).Item("lnote_client_amod_id")
                  jetnet_mod.Text = aTempTable.Rows(0).Item("lnote_jetnet_amod_id")
                  'add_note.ImageUrl = "~/images/edit.jpg"

                  'Event that fills the DropDown for the Action Items. All of them. COmpany, Contact, Aircraft, Time, etc. 
                  RaiseEvent fill_drop(aTempTable.Rows(0).Item("lnote_jetnet_ac_id"), aTempTable.Rows(0).Item("lnote_client_ac_id"), aTempTable.Rows(0).Item("lnote_jetnet_comp_id"), aTempTable.Rows(0).Item("lnote_client_comp_id"), aTempTable.Rows(0).Item("lnote_jetnet_contact_id"), aTempTable.Rows(0).Item("lnote_client_contact_id"), aTempTable.Rows(0).Item("lnote_jetnet_amod_id"), aTempTable.Rows(0).Item("lnote_client_amod_id"), Me, False)

                  'This is the category that was selected
                  notes_cat.SelectedValue = aTempTable.Rows(0).Item("lnote_notecat_key")

                  'Filling in the date.
                  If Not IsDBNull(aTempTable.Rows(0).Item("lnote_schedule_start_date")) Then
                    Dim offset As Date = aTempTable.Rows(0).Item("lnote_schedule_start_date")
                    offset = DateAdd("h", Session("timezone_offset"), offset)
                    Try
                      time.SelectedValue = (FormatDateTime(offset, 4))
                    Catch
                    End Try
                  End If
                  'Filling in the user ID
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
                End If
              End If
              'This will allow the user to remove an action item. The image button associated with that function.
              removeNoteLB.Visible = True

              If Session.Item("localUser").crmUserType = eUserTypes.RESEARCH Then
                add_noteLB.Visible = False
              End If
            Catch ex As Exception
              error_string = "Setting up the form for editing an Action Item - " & ex.Message
              clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
            End Try
          Case "new"
            Try
              Dim jetnet_ac_id As Integer = 0
              Dim client_ac_id As Integer = 0
              Dim jetnet_comp_id As Integer = 0
              Dim client_comp_id As Integer = 0
              Dim jetnet_contact_id As Integer = 0
              Dim client_contact_id As Integer = 0
              Dim jetnet_mod_id As Integer = 0
              Dim client_mod_id As Integer = 0
              'Setting up a new form,
              'All of the IDs associated
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


                  'Adding a special check to see what happens.
                  If LCase(Trim(Request("from"))) = "view" Then
                    If Not IsNothing(Trim(Request("Prospect_Client_Comp_ID"))) Then
                      If IsNumeric(Trim(Request("Prospect_Client_Comp_ID"))) Then
                        client_comp_id = Trim(Request("Prospect_Client_Comp_ID"))
                      End If
                    End If
                    If Not IsNothing(Trim(Request("Prospect_Jetnet_Comp_ID"))) Then
                      If IsNumeric(Trim(Request("Prospect_Jetnet_Comp_ID"))) Then
                        jetnet_comp_id = Trim(Request("Prospect_Jetnet_Comp_ID"))
                      End If
                    End If
                  End If
              End Select

              'Even that fills in the dropdowns based on what's passed to the page.
              RaiseEvent fill_drop(jetnet_ac_id, client_ac_id, jetnet_comp_id, client_comp_id, jetnet_contact_id, client_contact_id, jetnet_mod_id, client_mod_id, Me, False)
              'Filling in the hidden ID boxes. 
              jetnet_ac.Text = jetnet_ac_id
              client_ac.Text = client_ac_id
              jetnet_comp.Text = jetnet_comp_id
              client_comp.Text = client_comp_id
              jetnet_contact.Text = jetnet_contact_id
              client_contact.Text = client_contact_id
              'Pertaining to filled in with the current user. 
              Try
                pertaining_to.SelectedValue = Session.Item("localUser").crmLocalUserID
              Catch
              End Try
              'Toggling visibility of panels
              AC_Search_Vis.Visible = True
              company_search_vis.Visible = True
              company_related.Visible = False
              removeNoteLB.Visible = False
            Catch ex As Exception
              error_string = "Setting up the form for a new Action Item - " & ex.Message
              clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
            End Try

        End Select
        notes_edit.Focus()
        If Session.Item("localUser").crmUserType = eUserTypes.RESEARCH Then
          removeNoteLB.Visible = False
        End If
      End If
    End If
  End Sub
#End Region
#Region "Deals with Dropdown Selected Changes/Detailed Search clicks"
  Private Sub aircraft_related_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles aircraft_related.CheckedChanged
    Try
      If aircraft_related.Checked = True Then
        ac_search.Visible = False
      Else
        ac_search.Visible = True
      End If
    Catch ex As Exception
      error_string = "Action Item Form - Aircraft Related CheckChanged - " & ex.Message
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
      error_string = "Action Item Form - Contact Related CheckChanged - " & ex.Message
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
      error_string = "Action Item Form - Company Related CheckChanged - " & ex.Message
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
      error_string = "Action Item Form - Aircraft Name SelectedIndexChanged- " & ex.Message
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
      error_string = "Action Item Form - AC_Search_Vis_Click - " & ex.Message
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
      error_string = "Action Item Form - company_search_vis_Click - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Private Sub contact_search_vis_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles contact_search_vis.Click
    Try
      contact_search.Visible = True
      contact_search_vis.Visible = False
    Catch ex As Exception
      error_string = "Action Item Form - contact_search_vis_Click - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Public Sub ac_search_button_Click() Handles ac_search_buttonLB.Click
    Try
      ac_search.Visible = True
      AC_Search_Vis.Visible = False
      aircraft_related.Visible = False
      RaiseEvent ac_searchClick(Me)
    Catch ex As Exception
      error_string = "Action Item Form - ac_search_button_Click - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Public Sub company_search_button_Click() Handles company_search_buttonLB.Click
    Try
      company_search.Visible = True
      company_search_vis.Visible = False
      RaiseEvent company_searchClick(Me)
    Catch ex As Exception
      error_string = "Action Item Form - company_search_button_Click - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Private Sub company_name_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles company_name.SelectedIndexChanged
    Try
      company_search.Visible = True
      company_search_vis.Visible = False
      RaiseEvent company_name_changed(Me)
    Catch ex As Exception
      error_string = "Action Item Form - company_name_SelectedIndexChanged - " & ex.Message
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
      error_string = "Action Item Form - contact_name_SelectedIndexChanged - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
#End Region
  Public Sub checkDatePrev(ByVal sender As Object, ByVal args As ServerValidateEventArgs)
    Dim dated As Date = FormatDateTime(Now(), 2)
    Dim dated2 As Date = args.Value
    If (dated2) < (dated) Then
      args.IsValid = False
      Exit Sub
    End If
    args.IsValid = True
  End Sub
#Region "Insert Note"
  Private Sub add_note_Click() Handles add_noteLB.Click
    Try
      If Not (Page.IsValid) Then

      Else
        Select Case Trim(Request("action"))
          Case "edit"
            If Session.Item("localUser").crmUserType <> eUserTypes.RESEARCH Then
              Dim idnum As Integer = Trim(Request("id"))
              RaiseEvent edit_note("action", Me, idnum)
              If Session.Item("isMobile") = True Then
                Response.Redirect("Mobile_Details.aspx?type=" & Session.Item("Listing") & "&comp_id=" & Session.Item("ListingID") & "&source=" & Session.Item("ListingSource") & "&edited=action", False)
              End If
            End If
          Case "new"
            RaiseEvent edit_note("action", Me, 0)
            If Session.Item("isMobile") = True Then
              Response.Redirect("Mobile_Details.aspx?type=" & Session.Item("Listing") & "&comp_id=" & Session.Item("ListingID") & "&source=" & Session.Item("ListingSource") & "&added=action", False)
            End If
        End Select
      End If
    Catch ex As Exception
      error_string = "Action Item Form - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try

  End Sub
#End Region


  Private Sub statused_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles statused.SelectedIndexChanged
    If statused.SelectedValue = "C" Then
      action_to_note_warning.Text = "Completing your Action Item will change the type to a Note."
      priority.SelectedValue = 3
      email_action.Visible = False
      priority_action.Visible = False
      CustomValidator1.Enabled = False
      CalendarExtender1.OnClientDateSelectionChanged = ""
    End If
  End Sub

  Private Sub remove_note_Click() Handles removeNoteLB.Click
    If Session.Item("localUser").crmUserType <> eUserTypes.RESEARCH Then
      Dim idnum As Integer = Trim(Request("id"))
      RaiseEvent remove_note_ev(idnum, Me, "action")
    End If
  End Sub

  Private Sub email_pertaining_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles email_pertaining.CheckedChanged
    If email_pertaining.Checked = True Then
      cc_row.Visible = True
    Else
      cc_row.Visible = False
    End If
  End Sub
End Class