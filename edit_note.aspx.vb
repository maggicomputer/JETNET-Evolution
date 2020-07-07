Imports System.IO
Imports System
Imports System.Net.Mail

Partial Public Class edit_note
  Inherits System.Web.UI.Page
  Dim aclsData_Temp As New clsData_Manager_SQL
  Dim aTempTable, aTempTable2 As New DataTable 'Data Tables used
  Dim error_string As String = ""
  Private localDatalayer As viewsDataLayer
  Private searchCriteria As New viewSelectionCriteriaClass
  Public completed_or_open As String = ""
  Public LAST_SAVE_DATE As String



#Region "Page events"
  Private Sub export_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try
      aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
      aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase") 'CApplication.Item("crmJetnetDatabase")

      If Session.Item("crmUserLogon") <> True Then
        Response.Redirect("Default.aspx", False)
      End If

      Select Case Trim(Request("type"))
        Case "note", "prospect", "valuation"
          titleh.Text = "Note Maintenance - Marketplace Manager"
          Session.Item("NoteType") = Trim(Request("type"))
          Notes1.Visible = True
          If Trim(Request("type")) <> "valuation" Then
            heading.Text = Trim(Request("type")) & " Maintenance"
          Else
            titleh.Text = "Market Value Analysis - Marketplace Manager"
            heading.Text = "Market Value Analysis"
          End If
        Case "value_analysis"
          Session.Item("NoteType") = Trim(Request("type"))
          titleh.Text = "Enter Aircraft Value Estimate - Marketplace Manager"
          heading.Text = "Enter Aircraft Value Estimate"
          Notes1.Visible = True

          Dim pertaining_to As New DropDownList
          If Not IsNothing(Me.FindControl("pertaining_to")) Then
            pertaining_to = Me.FindControl("pertaining_to")
          Else
            pertaining_to.Items.Add(New ListItem("", 0))
          End If


          'If Trim(Request("id")) <> "" Then
          '  aTempTable = aclsData_Temp.Get_Open_Market_Valuation(0, Trim(Request("id")))
          '  If Not IsNothing(aTempTable) Then
          '    If aTempTable.Rows.Count > 0 Then
          '      If Not IsNothing(aTempTable) Then
          '        For Each r As DataRow In aTempTable.Rows
          '          'Me.notes_edit.Text = r("lnote_note")

          '          'Me.estval_type_of.SelectedValue = r("clival_type")

          '          'Me.estval_asking_price.Text = r("asking_price")
          '          'Me.estval_take_price.Text = r("take_price")
          '          'Me.estval_estimated_value.Text = r("sold_price")

          '          'Me.estval_aftt.Text = r("clival_aftt_hours")
          '          'Me.estval_total_landings.Text = r("clival_total_landings")

          '          '  pertaining_to.SelectedValue = r("lnote_user_name")
          '          '  Me.note_date.Text = r("lnote_entry_date")

          '          'Call Fill_All_DropDowns(r("lnote_jetnet_ac_id"), r("lnote_jetnet_comp_id"), r("lnote_jetnet_comp_id"), r("lnote_client_comp_id"), r("lnote_jetnet_contact_id"), r("lnote_client_contact_id"), r("lnote_jetnet_amod_id"), r("lnote_client_amod_id"), Me, True)

          '        Next
          '      End If
          '    End If
          '  End If
          'End If


        Case "documents"
          Session("NoteType") = "Documents"
          titleh.Text = "Document Maintenance - Marketplace Manager"
          Documents1.Visible = True
          heading.Text = "Load a Document"
        Case "action"
          Session.Item("NoteType") = "Action"
          titleh.Text = "Action Items Maintenance - Marketplace Manager"
          ActionItems1.Visible = True
          heading.Text = "Action Item Maintenance"
        Case "email"
          Session.Item("NoteType") = "Email"
          titleh.Text = "Write an Email - Marketplace Manager"
          Email1.Visible = True
          heading.Text = "Send an Email"
        Case "opportunity"
          Session.Item("NoteType") = "Opportunity"
          titleh.Text = "Opportunity Maintenance - Marketplace Manager"
          Opportunities1.Visible = True
          heading.Text = "Opportunity Maintenance"
        Case "wanted"
          Session.Item("NoteType") = "Wanted"
          titleh.Text = "Wanted Maintenance - Marketplace Manager"
          Wanted1.Visible = True
          heading.Text = "Wanted Maintenance"
        Case "document_display"

          If Not IsNothing(Request.Item("file")) And Not IsNothing(Request.Item("id")) Then
            If Not String.IsNullOrEmpty(Request.Item("file").ToString) And Not IsNothing(Request.Item("id")) Then
              Display_Document(Request.Item("file").ToString, Request.Item("id").ToString)
            End If
          End If
        Case "UPDATE_STATUS"

          Dim aclsLocal_Notes As New clsLocal_Notes
          Dim aTempTable2 As DataTable

          aTempTable2 = aclsData_Temp.Get_Local_Notes_Client_NoteID(Trim(Request("id")))

          ' check the state of the DataTable
          If Not IsNothing(aTempTable2) Then
            If aTempTable2.Rows.Count > 0 Then
              For Each r As DataRow In aTempTable2.Rows
                aclsLocal_Notes.lnote_jetnet_ac_id = r("lnote_jetnet_ac_id")
                aclsLocal_Notes.lnote_client_ac_id = r("lnote_client_ac_id")
                aclsLocal_Notes.lnote_jetnet_comp_id = r("lnote_jetnet_comp_id")
                aclsLocal_Notes.lnote_client_comp_id = r("lnote_client_comp_id")
                aclsLocal_Notes.lnote_client_contact_id = r("lnote_client_contact_id")
                aclsLocal_Notes.lnote_jetnet_contact_id = r("lnote_jetnet_contact_id")
                aclsLocal_Notes.lnote_clipri_ID = r("lnote_clipri_ID")
                aclsLocal_Notes.lnote_document_flag = r("lnote_document_flag")
                aclsLocal_Notes.lnote_entry_date = r("lnote_entry_date")


                If Not IsDBNull(r("lnote_note")) Then
                  If r("lnote_note") <> "" Then
                    aclsLocal_Notes.lnote_status = "A"
                    If Trim(Request("status")) = "C" Then
                      aclsLocal_Notes.lnote_note = HttpUtility.HtmlEncode(r("lnote_note") & " ** Completed **")
                    Else
                      aclsLocal_Notes.lnote_note = HttpUtility.HtmlEncode(r("lnote_note") & " ** Dismissed **")
                    End If
                  Else
                    aclsLocal_Notes.lnote_status = Trim(Request("status"))
                    aclsLocal_Notes.lnote_note = ""
                  End If
                Else
                  aclsLocal_Notes.lnote_status = Trim(Request("status"))
                  aclsLocal_Notes.lnote_note = ""
                End If

                aclsLocal_Notes.lnote_clipri_ID = r("lnote_clipri_ID")

                aclsLocal_Notes.lnote_id = r("lnote_id")
                aclsLocal_Notes.lnote_entry_date = r("lnote_entry_date")
                aclsLocal_Notes.lnote_action_date = Now() ' DB requires some value
                aclsLocal_Notes.lnote_user_login = r("lnote_user_login") ' DB requires a string value greater than 0
                aclsLocal_Notes.lnote_user_name = Left(Session.Item("localUser").crmLocalUserFirstName & " " & Session.Item("localUser").crmLocalUserLastName, 15)
                aclsLocal_Notes.lnote_notecat_key = r("lnote_notecat_key")
                aclsLocal_Notes.lnote_user_id = r("lnote_user_id")
                aclsLocal_Notes.lnote_schedule_start_date = r("lnote_schedule_start_date")
                aclsLocal_Notes.lnote_schedule_end_date = r("lnote_schedule_end_date")


                If aclsData_Temp.update_localNote(aclsLocal_Notes) = True Then
                  Dim url As String = "listing_action.aspx"
                  System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location = '" & url & "';", True)
                  System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)

                Else
                  If aclsData_Temp.class_error <> "" Then
                    error_string = aclsData_Temp.class_error
                    clsGeneral.clsGeneral.LogError("edit_note.aspx.vb - Page Load() - " & error_string, aclsData_Temp)
                  End If
                End If

              Next
            End If
          End If

      End Select
    Catch ex As Exception
      error_string = "edit_note.aspx.vb - Page Load() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
#End Region
#Region "Public Functions for Events"
  Function what_ac(ByVal jetnet As Integer, ByVal client As Integer, ByVal show As Integer) As String
    'This function takes what AC and determines what ac is associated with this ID. 
    what_ac = ""
    Try
      Dim aircraft_text As String = ""
      If jetnet <> 0 Then
        Dim aError As String = ""
        aTempTable = aclsData_Temp.GetJETNET_Aircraft_Details_AC_ID(jetnet, aError)
        ' check the state of the DataTable
        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            For Each R As DataRow In aTempTable.Rows
              If show = 2 Then
                If R("ac_year_mfr") <> "" Then
                  aircraft_text = R("ac_year_mfr") & " "
                End If
                aircraft_text = aircraft_text & R("amod_make_name") & " " & R("amod_model_name") & "<br />"
                If R("ac_reg_nbr") <> "" Then
                  aircraft_text = aircraft_text & " Reg #: " & R("ac_reg_nbr")
                End If
              End If
              If show = 1 Then
                If R("ac_ser_nbr") <> "" Then
                  aircraft_text = "<br />Ser #: " & R("ac_ser_nbr")
                End If
              End If
              what_ac = aircraft_text
              ' End If
            Next
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            clsGeneral.clsGeneral.LogError("main_site.Master.vb - what_ac() - " & error_string, aclsData_Temp)
          End If
          display_error()
        End If
      ElseIf client <> 0 Then
        aTempTable = aclsData_Temp.Get_Clients_Aircraft(client)
        ' check the state of the DataTable
        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            For Each R As DataRow In aTempTable.Rows

              If show = 2 Then
                aTempTable2 = aclsData_Temp.Get_Clients_Aircraft_Model_amodID(R("cliaircraft_cliamod_id"))
                If Not IsNothing(aTempTable2) Then
                  If aTempTable2.Rows.Count > 0 Then
                    For Each q As DataRow In aTempTable2.Rows
                      aircraft_text = q("cliamod_make_name") & " " & q("cliamod_model_name") & "<br />"
                    Next
                  End If
                End If
                If R("cliaircraft_year_mfr") <> "" Then
                  aircraft_text = aircraft_text & "Year: " & R("cliaircraft_year_mfr") & "<br />"
                End If
                If R("cliaircraft_reg_nbr") <> "" Then
                  aircraft_text = aircraft_text & "Reg #: " & R("cliaircraft_reg_nbr") & "<br />"
                End If
              End If
              If show = 1 Then
                If R("cliaircraft_ser_nbr") <> "" Then
                  aircraft_text = aircraft_text & "Ser #: " & R("cliaircraft_ser_nbr") & "<br />"
                End If
              End If
              what_ac = aircraft_text
            Next
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            clsGeneral.clsGeneral.LogError("main_site.Master.vb - what_ac() - " & error_string, aclsData_Temp)
          End If

        End If
      End If
    Catch ex As Exception
      error_string = "main_site.Master.vb - what_ac() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Function
  Function what_comp(ByVal jetnet As Integer, ByVal client As Integer, ByVal part As Integer) As String
    'This function takes what company and source and displays what company id associated with the number
    what_comp = ""
    Try
      Dim source As String = "JETNET"
      Dim idnum As Integer = 0
      Dim contact_text As String = ""
      If jetnet <> 0 Then
        source = "JETNET"
        idnum = jetnet
      ElseIf client <> 0 Then
        source = "CLIENT"
        idnum = client
      End If
      aTempTable = aclsData_Temp.GetCompanyInfo_ID(idnum, source, 0)
      ' check the state of the DataTable
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          For Each R As DataRow In aTempTable.Rows
            If part = 1 Then
              contact_text = "<b>" & R("comp_name") & "</b><br />"
            Else
              contact_text = contact_text & R("comp_address1") & "<br />"
              contact_text = contact_text & R("comp_city") & ", " & R("comp_state") & " "
              contact_text = contact_text & R("comp_zip_code") & "<br />"
              contact_text = contact_text & R("comp_country") & "<br />"

              Try

                aTempTable = aclsData_Temp.GetPhoneNumbers(idnum, 0, source, 0)
                '' check the state of the DataTable
                If Not IsNothing(aTempTable) Then
                  If aTempTable.Rows.Count > 0 Then
                    ' set it to the datagrid 
                    For Each q As DataRow In aTempTable.Rows
                      contact_text = contact_text & q("pnum_type") & ": " & q("pnum_number") & "<br />"
                    Next
                    contact_text = contact_text & "<br />"
                  Else
                    'rows = 0
                    contact_text = ""
                  End If
                Else
                  If aclsData_Temp.class_error <> "" Then
                    error_string = aclsData_Temp.class_error
                    clsGeneral.clsGeneral.LogError("edit_note.aspx.vb - add_note (Phone Number Display)() - " & error_string, aclsData_Temp)
                  End If
                End If
              Catch ex As Exception
                error_string = "edit_note.aspx.vb - add_note (Phone Number Display)() " & ex.Message
                clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
              End Try
            End If
            what_comp = contact_text
            'End If
          Next
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          clsGeneral.clsGeneral.LogError("main_site.Master.vb - what_comp() - " & error_string, aclsData_Temp)
        End If
        display_error()
      End If
      Return what_comp
    Catch ex As Exception
      error_string = "main_site.Master.vb - what_comp() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Function
  Function what_contact(ByVal jetnet As Integer, ByVal client As Integer) As String
    'This function takes the contact id/source and displays what contact the number is associated with.
    what_contact = ""
    Try
      Dim source As String = "JETNET"
      Dim idnum As Integer = 0
      Dim contact_text As String = ""
      If jetnet <> 0 Then
        source = "JETNET"
        idnum = jetnet
      ElseIf client <> 0 Then
        source = "CLIENT"
        idnum = client
      End If
      aTempTable = aclsData_Temp.GetContacts_Details(idnum, source)
      Dim comp_id As Integer = 0
      ' check the state of the DataTable
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          For Each R As DataRow In aTempTable.Rows
            what_contact = R("contact_first_name") & " " & R("contact_middle_initial") & " " & R("contact_last_name") & "<br />"
            Try

              aTempTable = aclsData_Temp.GetPhoneNumbers(R("contact_comp_id"), idnum, source, 0)
              '' check the state of the DataTable
              If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then
                  ' set it to the datagrid 
                  For Each q As DataRow In aTempTable.Rows
                    what_contact = what_contact & q("pnum_type") & ": " & q("pnum_number") & "<br />"
                  Next
                  what_contact = what_contact & "<br />"
                Else
                  'rows = 0
                  'contact_text = ""
                End If
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  clsGeneral.clsGeneral.LogError("edit_note.aspx.vb - add_note (Phone Number Display)() - " & error_string, aclsData_Temp)
                End If
              End If
            Catch ex As Exception
              error_string = "edit_note.aspx.vb - add_note (Phone Number Display)() " & ex.Message
              clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
            End Try

          Next
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          clsGeneral.clsGeneral.LogError("edit_note.aspx.vb - what_contact() - " & error_string, aclsData_Temp)
        End If
      End If
      Return what_contact
    Catch ex As Exception
      error_string = "edit_note.aspx.vb - what_contact() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Function
  Sub company_drop_fill(ByVal name As Control)
    Try
      If Not Page.IsPostBack Then
        Dim old As Integer
        Dim company_name As DropDownList = name.FindControl("company_name")
        Dim contact_name As DropDownList = name.FindControl("contact_name")
        Dim jetnet_comp As TextBox = name.FindControl("jetnet_comp")
        Dim client_comp As TextBox = name.FindControl("client_comp")
        Dim jetnet_ac As TextBox = name.FindControl("jetnet_ac")
        Dim client_ac As TextBox = name.FindControl("client_ac")

        Select Case Session.Item("Listing")
          Case 3
            If Session.Item("ListingSource") = "CLIENT" Then
              aTempTable = aclsData_Temp.Get_Aircraft_Reference_Client_acID(Session.Item("ListingID"))
              If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then
                  For Each r As DataRow In aTempTable.Rows
                    aTempTable2 = aclsData_Temp.GetCompanyInfo_ID(r("cliacref_comp_id"), "CLIENT", 0)
                    ' check the state of the DataTable
                    If Not IsNothing(aTempTable) Then
                      If aTempTable2.Rows.Count > 0 Then
                        For Each z As DataRow In aTempTable2.Rows
                          If old <> r("cliacref_comp_id") Then
                            If IsNothing(company_name.Items.FindByValue(r("cliacref_comp_id") & "|CLIENT")) Then
                              company_name.Items.Add(New ListItem(z("comp_name") & " (" & z("comp_state") & " - " & z("comp_country") & ")", r("cliacref_comp_id") & "|CLIENT"))
                            End If
                          End If
                          old = r("cliacref_comp_id")
                        Next
                      End If
                    Else
                      If aclsData_Temp.class_error <> "" Then
                        error_string = aclsData_Temp.class_error
                        clsGeneral.clsGeneral.LogError("edit_note.aspx.vb - company_drop_fill() - " & error_string, aclsData_Temp)
                      End If
                    End If
                  Next
                End If
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  clsGeneral.clsGeneral.LogError("edit_note.aspx.vb - company_drop_fill() - " & error_string, aclsData_Temp)
                End If
              End If

            Else
              aTempTable = aclsData_Temp.GetAircraft_Listing_wContacts(Session.Item("ListingID"), "JETNET")
              If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then
                  For Each r As DataRow In aTempTable.Rows
                    If old <> r("comp_id") Then
                      If IsNothing(company_name.Items.FindByValue(r("comp_id") & "|JETNET")) Then
                        company_name.Items.Add(New ListItem(r("comp_name") & " (" & r("comp_state") & " - " & r("comp_country") & ")", r("comp_id") & "|JETNET"))
                      End If
                    End If
                    old = r("comp_id")
                  Next
                End If
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  clsGeneral.clsGeneral.LogError("edit_note.aspx.vb - company_drop_fill() - " & error_string, aclsData_Temp)
                End If
              End If
            End If
          Case 1
            Try
              aTempTable = aclsData_Temp.GetCompanyInfo_ID(Session.Item("ListingID"), Session.Item("ListingSource"), 0)
              ' check the state of the DataTable
              If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then
                  For Each R As DataRow In aTempTable.Rows
                    If IsNothing(company_name.Items.FindByValue(R("comp_id") & "|" & R("source"))) Then
                      company_name.Items.Add(New ListItem(R("comp_name") & " (" & R("comp_state") & " - " & R("comp_country") & ")", R("comp_id") & "|" & R("source")))
                    End If
                    company_name.Visible = True
                  Next
                Else '0 rows
                End If
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  clsGeneral.clsGeneral.LogError("edit_note.aspx.vb - company_drop_fill() - " & error_string, aclsData_Temp)
                End If
              End If
            Catch ex As Exception
              error_string = "edit_note.aspx.vb - company_drop_fill() - " & ex.Message
              clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
            End Try
          Case Else
            If jetnet_comp.Text <> 0 Or client_comp.Text <> 0 Then

              If jetnet_comp.Text <> 0 Then
                aTempTable = aclsData_Temp.GetCompanyInfo_ID(jetnet_comp.Text, "JETNET", 0)
              Else
                aTempTable = aclsData_Temp.GetCompanyInfo_ID(client_comp.Text, "CLIENT", 0)
              End If
              Try

                ' check the state of the DataTable
                If Not IsNothing(aTempTable) Then
                  If aTempTable.Rows.Count > 0 Then
                    For Each R As DataRow In aTempTable.Rows
                      If IsNothing(company_name.Items.FindByValue(R("comp_id") & "|" & R("source"))) Then
                        company_name.Items.Add(New ListItem(R("comp_name") & " (" & R("comp_state") & " - " & R("comp_country") & ")", R("comp_id") & "|" & R("source")))
                      End If

                      'company_name.SelectedValue = R("comp_id") & "|" & R("source")
                      company_name.Visible = True
                    Next
                  Else '0 rows
                  End If
                Else
                  If aclsData_Temp.class_error <> "" Then
                    error_string = aclsData_Temp.class_error
                    clsGeneral.clsGeneral.LogError("edit_note.aspx.vb - company_drop_fill() - " & error_string, aclsData_Temp)
                  End If
                  display_error()
                End If
              Catch ex As Exception
                error_string = "edit_note.aspx.vb - company_drop_fill() - " & ex.Message
                LogError(error_string)
              End Try

            ElseIf client_ac.Text <> 0 Then


              aTempTable = aclsData_Temp.Get_Aircraft_Reference_Client_acID(client_ac.Text)
              If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then
                  For Each r As DataRow In aTempTable.Rows
                    aTempTable2 = aclsData_Temp.GetCompanyInfo_ID(r("cliacref_comp_id"), "CLIENT", 0)
                    ' check the state of the DataTable
                    If Not IsNothing(aTempTable) Then
                      If aTempTable2.Rows.Count > 0 Then
                        For Each z As DataRow In aTempTable2.Rows
                          If old <> r("cliacref_comp_id") Then
                            If IsNothing(company_name.Items.FindByValue(r("cliacref_comp_id") & "|CLIENT")) Then
                              company_name.Items.Add(New ListItem(z("comp_name") & " (" & z("comp_state") & " - " & z("comp_country") & ")", r("cliacref_comp_id") & "|CLIENT"))
                            End If
                          End If
                          old = r("cliacref_comp_id")
                        Next
                      End If
                    Else
                      If aclsData_Temp.class_error <> "" Then
                        error_string = aclsData_Temp.class_error
                        clsGeneral.clsGeneral.LogError("edit_note.aspx.vb - company_drop_fill() - " & error_string, aclsData_Temp)
                      End If
                    End If
                  Next
                End If
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  clsGeneral.clsGeneral.LogError("edit_note.aspx.vb - company_drop_fill() - " & error_string, aclsData_Temp)
                End If
              End If
            ElseIf jetnet_ac.Text <> 0 Then
              aTempTable = aclsData_Temp.GetAircraft_Listing_wContacts(jetnet_ac.Text, "JETNET")
              If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then
                  For Each r As DataRow In aTempTable.Rows
                    If old <> r("comp_id") Then
                      If IsNothing(company_name.Items.FindByValue(r("comp_id") & "|JETNET")) Then
                        company_name.Items.Add(New ListItem(r("comp_name") & " (" & r("comp_state") & " - " & r("comp_country"), r("comp_id") & "|JETNET"))
                      End If
                    End If
                    old = r("comp_id")
                  Next
                End If
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  clsGeneral.clsGeneral.LogError("edit_note.aspx.vb - company_drop_fill() - " & error_string, aclsData_Temp)
                End If
              End If
            End If
        End Select
      End If
    Catch ex As Exception
      error_string = "edit_note.aspx.vb - company_drop_fill() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
    Sub company_name_changed(ByVal name As Control)
        Try
            Dim Company_Data As New clsClient_Company
            Dim email_to As New Label 'This is setting email to as a label, a new label. In case on the other controls besides email
            'it doesn't exist.
            Dim email_address As String = "" 'here's an email address string to store the email address of the
            'selected companies.
            If Not IsNothing(name.FindControl("email_to")) Then 'here's email to control.
                'this only gets set if the control actually exists on the user control we're on.
                email_to = name.FindControl("email_to")
            End If

            Dim add_note As New ImageButton 'this is setting that image button as a button if it exists.
            If Not IsNothing(name.FindControl("add_note")) Then 'check to see if this exists on parent user control.
                add_note = name.FindControl("add_note") 'if it does, then set it to add note, instead of just a new button
            End If
            Dim company_name As DropDownList = name.FindControl("company_name")
            Dim jetnet_comp As TextBox = name.FindControl("jetnet_comp")
            Dim client_comp As TextBox = name.FindControl("client_comp")
            Dim contact_name As DropDownList = name.FindControl("contact_name")
            Dim company_info As Label = name.FindControl("company_info")
            If company_name.SelectedValue = "0" Or company_name.SelectedValue = "|" Then
                jetnet_comp.Text = "0"
                client_comp.Text = "0"
                company_info.Text = ""
                'it's blank? okay set to visible = false
                'basically this happens if no company is selected after a company is selected
                add_note.Visible = False
                email_to.Text = ""
            ElseIf company_name.SelectedValue <> "0" Then
                Dim info As Array = Split(company_name.SelectedValue, "|")
                If UBound(info) = 1 Then
                    If info(1) = "CLIENT" Then
                        aTempTable = aclsData_Temp.GetCompanyInfo_ID(info(0), info(1), 0)
                        If Not IsNothing(aTempTable) Then
                            If aTempTable.Rows.Count > 0 Then
                                Company_Data = New clsClient_Company
                                Company_Data = clsGeneral.clsGeneral.Create_Company_Class(aTempTable, "CLIENT", New DataTable)
                                company_info.Text = aTempTable.Rows(0).Item("comp_name") & "<br />" & clsGeneral.clsGeneral.Show_Company_Display(Company_Data, False)

                                jetnet_comp.Text = IIf(Not IsDBNull(aTempTable.Rows(0).Item("jetnet_comp_id")), aTempTable.Rows(0).Item("jetnet_comp_id"), 0)
                                email_address = IIf(Not IsDBNull(aTempTable.Rows(0).Item("comp_email_address")), aTempTable.Rows(0).Item("comp_email_address"), "")
                            End If
                        End If
                        aTempTable.Dispose()
                        client_comp.Text = info(0)
                    Else
                        aTempTable = aclsData_Temp.GetCompanyInfo_JETNET_ID(info(0), "")
                        If Not IsNothing(aTempTable) Then
                            If aTempTable.Rows.Count > 0 Then
                                client_comp.Text = IIf(Not IsDBNull(aTempTable.Rows(0).Item("comp_id")), aTempTable.Rows(0).Item("comp_id"), 0)
                                email_address = IIf(Not IsDBNull(aTempTable.Rows(0).Item("comp_email_address")), aTempTable.Rows(0).Item("comp_email_address"), "")

                                Company_Data = New clsClient_Company
                                Company_Data = clsGeneral.clsGeneral.Create_Company_Class(aTempTable, "CLIENT", New DataTable)
                                company_info.Text = aTempTable.Rows(0).Item("comp_name") & "<br />" & clsGeneral.clsGeneral.Show_Company_Display(Company_Data, False)


                            ElseIf aTempTable.Rows.Count = 0 Then
                                aTempTable2 = aclsData_Temp.GetCompanyInfo_ID(info(0), "JETNET", 0)
                                If Not IsNothing(aTempTable2) Then
                                    If aTempTable2.Rows.Count > 0 Then
                                        Company_Data = New clsClient_Company
                                        Company_Data = clsGeneral.clsGeneral.Create_Company_Class(aTempTable2, "JETNET", New DataTable)
                                        company_info.Text = aTempTable2.Rows(0).Item("comp_name") & "<br />" & clsGeneral.clsGeneral.Show_Company_Display(Company_Data, False)
                                    End If
                                End If
                            End If
                        End If
                        jetnet_comp.Text = info(0)
                        aTempTable.Dispose()
                    End If

                    If name.ID = "Email1" Then
                        If email_address.ToString <> "" Then 'Here we go.
                            'If this email address is actually not blank, 
                            'we're going to fill in the to label.
                            'we're also going to switch the visibility of 
                            'add note to true meaning they'll be able to send an email.
                            add_note.Visible = True
                            email_to.Text = email_address
                        Else 'it's blank? okay set to visible = false 
                            add_note.Visible = False
                            email_to.Text = ""
                        End If
                    End If

                    Dim comp_id As Integer = CInt(info(0))
                    contact_name.Items.Clear()
                    Dim c_title As String = ""
                    aTempTable = aclsData_Temp.GetContacts(comp_id, info(1), "Y", 0)
                    ' check the state of the DataTable
                    If Not IsNothing(aTempTable) Then
                        If aTempTable.Rows.Count > 0 Then
                            contact_name.Items.Clear()
                            contact_name.Items.Add(New ListItem("NOT SELECTED", ""))
                            For Each r As DataRow In aTempTable.Rows
                                c_title = IIf(Not IsDBNull(r("contact_title")), r("contact_title"), "")


                                If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Or Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
                                    If c_title <> "" Then
                                        contact_name.Items.Add(New ListItem(CStr(r("contact_title") & " - " & r("contact_sirname") & " " & r("contact_first_name") & " " & r("contact_last_name")), r("contact_id") & "|" & r("contact_type")))
                                    Else
                                        contact_name.Items.Add(New ListItem(CStr(r("contact_sirname") & " " & r("contact_first_name") & " " & r("contact_last_name")), r("contact_id") & "|" & r("contact_type")))
                                    End If
                                Else
                                    If c_title <> "" Then
                                        contact_name.Items.Add(New ListItem(CStr(r("contact_title") & " - " & r("contact_sirname") & " " & r("contact_first_name") & " " & r("contact_last_name") & " (" & r("contact_type") & " record) "), r("contact_id") & "|" & r("contact_type")))
                                    Else
                                        contact_name.Items.Add(New ListItem(CStr(r("contact_sirname") & " " & r("contact_first_name") & " " & r("contact_last_name") & " (" & r("contact_type") & " record)"), r("contact_id") & "|" & r("contact_type")))
                                    End If
                                End If


                            Next
                        Else
                            contact_name.Items.Add(New ListItem("NO ASSOCIATED CONTACTS", ""))
                        End If
                    Else
                        If aclsData_Temp.class_error <> "" Then
                            error_string = aclsData_Temp.class_error
                            clsGeneral.clsGeneral.LogError("edit_note.aspx.vb - company_name_changed() - " & error_string, aclsData_Temp)
                        End If
                        display_error()
                    End If
                End If
            End If
        Catch ex As Exception
            error_string = "edit_note.aspx.vb - company_name_changed() - " & ex.Message
            LogError(error_string)
        End Try
    End Sub
    Sub evoadmin_company_name_changed(ByVal name As Control)
        Try
            Dim Company_Data As New clsClient_Company
            Dim email_to As New Label 'This is setting email to as a label, a new label. In case on the other controls besides email
            'it doesn't exist.
            Dim email_address As String = "" 'here's an email address string to store the email address of the
            'selected companies.
            If Not IsNothing(name.FindControl("email_to")) Then 'here's email to control.
                'this only gets set if the control actually exists on the user control we're on.
                email_to = name.FindControl("email_to")
            End If

            Dim add_note As New ImageButton 'this is setting that image button as a button if it exists.
            If Not IsNothing(name.FindControl("add_note")) Then 'check to see if this exists on parent user control.
                add_note = name.FindControl("add_note") 'if it does, then set it to add note, instead of just a new button
            End If
            Dim company_name As DropDownList = name.FindControl("company_name")
            Dim jetnet_comp As TextBox = name.FindControl("jetnet_comp")
            Dim client_comp As TextBox = name.FindControl("client_comp")
            Dim contact_name As DropDownList = name.FindControl("contact_name")
            Dim company_info As Label = name.FindControl("company_info")


            If company_name.SelectedValue = "0" Or company_name.SelectedValue = "|" Then
                jetnet_comp.Text = "0"
                client_comp.Text = "0"
                company_info.Text = ""
                'it's blank? okay set to visible = false
                'basically this happens if no company is selected after a company is selected
                add_note.Visible = False
                email_to.Text = ""
            ElseIf company_name.SelectedValue <> "0" Then
                Dim info As Array = Split(company_name.SelectedValue, "|")
                If UBound(info) = 1 Then


                    aTempTable2 = aclsData_Temp.GetCompanyInfo_ID(info(0), "JETNET", 0)
                    If Not IsNothing(aTempTable2) Then
                        If aTempTable2.Rows.Count > 0 Then
                            Company_Data = New clsClient_Company
                            Company_Data = clsGeneral.clsGeneral.Create_Company_Class(aTempTable2, "JETNET", New DataTable)
                            company_info.Text = aTempTable2.Rows(0).Item("comp_name") & "<br />" & clsGeneral.clsGeneral.Show_Company_Display(Company_Data, False)
                        End If
                    End If


                    jetnet_comp.Text = info(0)
                    aTempTable.Dispose()

                    If name.ID = "Email1" Then
                        If email_address.ToString <> "" Then 'Here we go.
                            'If this email address is actually not blank, 
                            'we're going to fill in the to label.
                            'we're also going to switch the visibility of 
                            'add note to true meaning they'll be able to send an email.
                            add_note.Visible = True
                            email_to.Text = email_address
                        Else 'it's blank? okay set to visible = false 
                            add_note.Visible = False
                            email_to.Text = ""
                        End If
                    End If

                    Dim comp_id As Integer = CInt(info(0))
                    contact_name.Items.Clear()
                    Dim c_title As String = ""
                    aTempTable = aclsData_Temp.GetContacts(comp_id, info(1), "Y", 0)
                    ' check the state of the DataTable
                    If Not IsNothing(aTempTable) Then
                        If aTempTable.Rows.Count > 0 Then
                            contact_name.Items.Clear()
                            contact_name.Items.Add(New ListItem("NOT SELECTED", ""))
                            For Each r As DataRow In aTempTable.Rows
                                c_title = IIf(Not IsDBNull(r("contact_title")), r("contact_title"), "")


                                If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Or Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
                                    If c_title <> "" Then
                                        contact_name.Items.Add(New ListItem(CStr(r("contact_title") & " - " & r("contact_sirname") & " " & r("contact_first_name") & " " & r("contact_last_name")), r("contact_id") & "|" & r("contact_type")))
                                    Else
                                        contact_name.Items.Add(New ListItem(CStr(r("contact_sirname") & " " & r("contact_first_name") & " " & r("contact_last_name")), r("contact_id") & "|" & r("contact_type")))
                                    End If
                                Else
                                    If c_title <> "" Then
                                        contact_name.Items.Add(New ListItem(CStr(r("contact_title") & " - " & r("contact_sirname") & " " & r("contact_first_name") & " " & r("contact_last_name") & " (" & r("contact_type") & " record) "), r("contact_id") & "|" & r("contact_type")))
                                    Else
                                        contact_name.Items.Add(New ListItem(CStr(r("contact_sirname") & " " & r("contact_first_name") & " " & r("contact_last_name") & " (" & r("contact_type") & " record)"), r("contact_id") & "|" & r("contact_type")))
                                    End If
                                End If


                            Next
                        Else
                            contact_name.Items.Add(New ListItem("NO ASSOCIATED CONTACTS", ""))
                        End If
                    Else
                        If aclsData_Temp.class_error <> "" Then
                            error_string = aclsData_Temp.class_error
                            clsGeneral.clsGeneral.LogError("edit_note.aspx.vb - company_name_changed() - " & error_string, aclsData_Temp)
                        End If
                        display_error()
                    End If
                End If
            End If
        Catch ex As Exception
            error_string = "edit_note.aspx.vb - company_name_changed() - " & ex.Message
            LogError(error_string)
        End Try
    End Sub

    Private Sub aircraft_name_changed(ByVal name As Control, ByVal FillModel As Boolean)
    Try
      Dim Aircraft_Data As New clsClient_Aircraft
      Dim Aircraft_Model As String = ""
      Dim aircraft_name As DropDownList = name.FindControl("aircraft_name")
      Dim jetnet_ac As TextBox = name.FindControl("jetnet_ac")
      Dim client_ac As TextBox = name.FindControl("client_ac")
      Dim jetnet_mod As TextBox = name.FindControl("jetnet_mod")
      Dim client_mod As TextBox = name.FindControl("client_mod")
      Dim aircraft_info As Label = name.FindControl("aircraft_info")
      Dim typed() As String
      If name.ID <> "Wanted1" Then
        If aircraft_name.SelectedValue = "0" Or aircraft_name.SelectedValue = "0||0" Then
          jetnet_ac.Text = "0"
          client_ac.Text = "0"
          jetnet_mod.Text = "0"
          client_mod.Text = "0"
          aircraft_info.Text = ""
        ElseIf aircraft_name.SelectedValue <> "0" Then
          typed = Split(aircraft_name.SelectedValue, "|")
          If UCase(typed(1)) = "JETNET" Then
            jetnet_ac.Text = typed(0)
            jetnet_mod.Text = typed(2)
            aTempTable2 = aclsData_Temp.Get_Client_Aircraft_JETNET_AC(typed(0))
            If Not IsNothing(aTempTable2) Then
              If aTempTable2.Rows.Count > 0 Then
                client_ac.Text = aTempTable2.Rows(0).Item("cliaircraft_id")

                'Aircraft_Model = (aTempTable2.Rows(0).Item("cliamod_make_name") & " " & aTempTable2.Rows(0).Item("cliamod_model_name"))
                Aircraft_Data = clsGeneral.clsGeneral.Create_Aircraft_Class(aTempTable2, "cliaircraft")
                Aircraft_Data.cliaircraft_id = aTempTable2.Rows(0).Item("cliaircraft_id")

                aircraft_info.Text = Aircraft_Model & "<br />" & clsGeneral.clsGeneral.Build_Aircraft_Display(Aircraft_Data, True, False, True)
              Else 'unfortunately have to repoll the database for the ac display information because it still exists evne if it's not a client. 
                clsGeneral.clsGeneral.Display_Jetnet_Aircraft_Label(aircraft_info, Aircraft_Data, typed(0), aclsData_Temp, jetnet_mod)
                client_ac.Text = "0" ' clear this
              End If 'just because there's no ac doesn't mean no model.


              aTempTable = aclsData_Temp.Get_Clients_Aircraft_Model_ByJETNETAmod(typed(2))
              If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then
                  If Not IsNothing(aTempTable2) Then 'This only gets filled in if the up above ran (atemptable2 gets filled meaning it's got a client AC.)
                    If aTempTable2.Rows.Count > 0 Then 'The atemptable2 datatable doesn't get the model ID, however this one does, so I can fill it in after the fact and just append
                      'it to the top
                      Aircraft_Model = (aTempTable.Rows(0).Item("cliamod_make_name") & " " & aTempTable.Rows(0).Item("cliamod_model_name"))
                      aircraft_info.Text = Aircraft_Model & aircraft_info.Text
                    End If
                  End If
                  client_mod.Text = aTempTable.Rows(0).Item("cliamod_id")
                End If
              End If
            End If
          Else
            client_ac.Text = typed(0)

            'Search the client AC to bring back the jetnet ac ID
            aTempTable2 = aclsData_Temp.Get_Clients_Aircraft(typed(0))
            If Not IsNothing(aTempTable2) Then
              If aTempTable2.Rows.Count > 0 Then
                jetnet_ac.Text = aTempTable2.Rows(0).Item("cliaircraft_jetnet_ac_id")
                'get the jetnet_model
                clsGeneral.clsGeneral.Display_Jetnet_Aircraft_Label(aircraft_info, Aircraft_Data, aTempTable2.Rows(0).Item("cliaircraft_jetnet_ac_id"), aclsData_Temp, jetnet_mod)

                If aTempTable2.Rows(0).Item("cliaircraft_jetnet_ac_id") = 0 Then
                  'this means there's no jetnet ac so we still need to display based on client ac info
                  jetnet_ac.Text = "0"
                  Aircraft_Model = (aTempTable2.Rows(0).Item("cliamod_make_name") & " " & aTempTable2.Rows(0).Item("cliamod_model_name"))
                  Aircraft_Data = clsGeneral.clsGeneral.Create_Aircraft_Class(aTempTable2, "cliaircraft")
                  Aircraft_Data.cliaircraft_id = aTempTable2.Rows(0).Item("cliaircraft_id")
                  aircraft_info.Text = Aircraft_Model & "<br />" & clsGeneral.clsGeneral.Build_Aircraft_Display(Aircraft_Data, True, False, True)
                End If
              End If
            End If


            client_mod.Text = typed(2)
          End If

          ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          'Changed 5-14-2012
          ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          'Rick's Note:
          'click on notes and enter a note -while entering you decide to pick an aircraft once 
          'you pick the aircraft the company name will disappear. We need to make this so that
          ' when you originally 
          'came in from a company that the company name sticks
          ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

          If Session.Item("Listing") = 3 Then
            Dim company_name As DropDownList = name.FindControl("company_name")
            Dim contact_name As DropDownList = name.FindControl("contact_name")

            Dim jetnet_comp As TextBox = name.FindControl("jetnet_comp")
            Dim client_comp As TextBox = name.FindControl("client_comp")
            Dim client_contact As TextBox = name.FindControl("client_contact")
            Dim jetnet_contact As TextBox = name.FindControl("jetnet_contact")

            jetnet_comp.Text = "0"
            client_comp.Text = "0"
            client_contact.Text = "0"
            jetnet_contact.Text = "0"

            company_name.Items.Clear()
            contact_name.Items.Clear()

            If IsNothing(company_name.Items.FindByValue("|")) Then
              company_name.Items.Add(New ListItem("None Selected", "|"))
            End If

            company_name.SelectedValue = "|"
            contact_name.Items.Add(New ListItem("Please Select a Contact", "0|"))
            contact_name.SelectedValue = "0|"
            Dim company_info As Label = name.FindControl("company_info")

            company_info.Text = ""

            just_the_left_hand_dropdowns(jetnet_ac.Text, client_ac.Text, 0, 0, 0, 0, jetnet_mod.Text, client_mod.Text, name, FillModel)
          End If
        End If
      ElseIf name.ID = "Wanted1" Then
        If aircraft_name.SelectedValue <> "0||0" Then
          typed = Split(aircraft_name.SelectedValue, "|")
          jetnet_mod.Text = typed(0)
          client_mod.Text = typed(4)
        End If
      End If
    Catch ex As Exception
      error_string = "edit_note.aspx.vb - aircraft_name_changed() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Sub company_SearchClick(ByVal name As Control)
    Try
      Dim company_name As DropDownList = name.FindControl("company_name")
      Dim Named As TextBox = name.FindControl("Name")
      Dim first_name As TextBox = name.FindControl("first_name")
      Dim last_name As TextBox = name.FindControl("last_name")
      Dim email_address As TextBox = name.FindControl("email_address")
            Dim phone_number As TextBox = name.FindControl("phone_number")


            If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Or Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
                aTempTable = aclsData_Temp.Search_CompanysWithContacts("J", "" & Trim(clsGeneral.clsGeneral.StripChars(Named.Text, True)) & "", "" & Trim(clsGeneral.clsGeneral.StripChars(first_name.Text, True)) & "", "" & Trim(clsGeneral.clsGeneral.StripChars(last_name.Text, True)) & "", "" & Trim(clsGeneral.clsGeneral.StripChars(phone_number.Text, True)) & "", "" & Trim(clsGeneral.clsGeneral.StripChars(email_address.Text, True)) & "")
            Else
                aTempTable = aclsData_Temp.Search_CompanysWithContacts("JC", "" & Trim(clsGeneral.clsGeneral.StripChars(Named.Text, True)) & "", "" & Trim(clsGeneral.clsGeneral.StripChars(first_name.Text, True)) & "", "" & Trim(clsGeneral.clsGeneral.StripChars(last_name.Text, True)) & "", "" & Trim(clsGeneral.clsGeneral.StripChars(phone_number.Text, True)) & "", "" & Trim(clsGeneral.clsGeneral.StripChars(email_address.Text, True)) & "")
            End If





            'aTempTable = aclsData_Temp.Company_Search("%" & Trim(clsGeneral.clsGeneral.StripChars(Named.Text, True)) & "%", "Y", "JC", "", "", "", Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmCommercial_Flag, "", "", "", "", "", "", "")

            company_name.Items.Clear()
      If IsNothing(company_name.Items.FindByValue("|")) Then
        company_name.Items.Add(New ListItem("None Selected", "|"))
      End If

      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          For Each r As DataRow In aTempTable.Rows
            Dim contact_text As String = ""

            contact_text = r("comp_name") & " - "
            If Not IsDBNull(r("comp_address1")) Then
              If r("comp_address1") <> "" Then
                contact_text = contact_text & r("comp_address1") & " "
              End If
            End If
            If Not IsDBNull(r("comp_address2")) Then
              If r("comp_address2") <> "" Then
                contact_text = contact_text & r("comp_address2") & " "
              End If
            End If
            If Not IsDBNull(r("comp_city")) Then
              contact_text = contact_text & r("comp_city")
            End If
            If Not IsDBNull(r("comp_state")) Then
              contact_text = contact_text & ", " & r("comp_state") & " "
            End If
            If Not IsDBNull(r("comp_zip_code")) Then
              contact_text = contact_text & r("comp_zip_code") & " "
            End If
            If Not IsDBNull(r("comp_country")) Then
              contact_text = contact_text & r("comp_country")
            End If

                        If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Or Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
                            contact_text = contact_text
                        Else
                            contact_text = contact_text & " (" & r("source") & " Record)"
                        End If


                        If IsNothing(company_name.Items.FindByValue(r("comp_id") & "|" & r("source"))) Then
              company_name.Items.Add(New ListItem(contact_text, r("comp_id") & "|" & r("source")))
            End If

          Next
        Else ' 0 rows
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          clsGeneral.clsGeneral.LogError("edit_note.aspx.vb - company_SearchClick() - " & error_string, aclsData_Temp)
        End If
      End If
    Catch ex As Exception
      error_string = "edit_note.aspx.vb - company_SearchClick() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Private Sub ac_SearchClick(ByVal name As Control)
    Try
      If Page.IsPostBack Then
        Dim aircraft_name As DropDownList = name.FindControl("aircraft_name")
        Dim aircraft_related As CheckBox = name.FindControl("aircraft_related")
        Dim serial As TextBox = name.FindControl("serial")
        Dim SQL_Aircraft_Make_Model As String
        SQL_Aircraft_Make_Model = "%" & serial.Text & "%"
        aTempTable = aclsData_Temp.AC_Search("AMOD_MAKE_NAME ASC, AMOD_MODEL_NAME ASC, AC_SER_NBR_SORT ASC", "JC", "", "", "", SQL_Aircraft_Make_Model, "", "", "", "", "", "", "", "", "", Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmJets_Flag, Session.Item("localSubscription").crmExecutive_Flag, Session.Item("localSubscription").crmTurboprops, Session.Item("localSubscription").crmCommercial_Flag, Session.Item("localSubscription").crmAerodexFlag, "", "", "", "", "5", "2", "", "", "", "", "", "", "", "", "", "", "", "", 0, False)
        If Session.Item("NoteType") = "Opportunity" Then
          SQL_Aircraft_Make_Model = "%" & serial.Text & "%"
          aTempTable = aclsData_Temp.GetAircraft_MakeModels(SQL_Aircraft_Make_Model, SQL_Aircraft_Make_Model, Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmCommercial_Flag, Session.Item("localSubscription").crmJets_Flag, Session.Item("localSubscription").crmExecutive_Flag, Session.Item("localSubscription").crmTurboprops, "")
          aircraft_related.Visible = False
        End If

        aircraft_related.Visible = False
        aircraft_name.Items.Clear()
        aircraft_name.Items.Add(New ListItem("None Selected", 0))
        'aircraft_name.SelectedValue = 0
        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            For Each r As DataRow In aTempTable.Rows
              Dim ser As String = ""
              If Not IsDBNull(r("ac_ser_nbr")) Then
                ser = Regex.Replace(r("ac_ser_nbr"), "<.*?>", "")
              ElseIf Not IsDBNull(r("other_ac_ser_nbr")) Then
                ser = Regex.Replace(r("other_ac_ser_nbr"), "<.*?>", "")
              End If

              aircraft_name.Items.Add(New ListItem(IIf(Not IsDBNull(r("ac_year_mfr")), r("ac_year_mfr"), r("other_ac_year_mfr")) & " " & r("amod_make_name") & " " & r("amod_model_name") & " Ser #:" & ser & " Reg#:" & IIf(Not IsDBNull(r("ac_reg_nbr")), r("ac_reg_nbr"), r("other_ac_reg_nbr")) & " (" & r("source") & " record)", r("ac_id") & "|" & r("source") & "|" & r("ac_amod_id")))
            Next
          Else
            '0 rows
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            clsGeneral.clsGeneral.LogError("edit_note.aspx.vb - ac_SearchClick() - " & error_string, aclsData_Temp)
          End If
        End If
      End If
    Catch ex As Exception
      error_string = "edit_note.aspx.vb - ac_SearchClick() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Private Sub contact_name_changed(ByVal name As Control)
    Try
      Dim email_to As New Label 'This is setting email to as a label, a new label. In case on the other controls besides email
      'it doesn't exist.
      Dim email_address As String = "" 'here's an email address string to store the email address of the
      'selected companies.
      If Not IsNothing(name.FindControl("email_to")) Then 'here's email to control.
        'this only gets set if the control actually exists on the user control we're on.
        email_to = name.FindControl("email_to")
      End If

      Dim add_note As New ImageButton 'this is setting that image button as a button if it exists.
      If Not IsNothing(name.FindControl("add_note")) Then 'check to see if this exists on parent user control.
        add_note = name.FindControl("add_note") 'if it does, then set it to add note, instead of just a new button
      End If
      Dim contact_name As DropDownList = name.FindControl("contact_name")
      Dim jetnet_contact As TextBox = name.FindControl("jetnet_contact")
      Dim client_contact As TextBox = name.FindControl("client_contact")
      If contact_name.SelectedValue = "" Then
        jetnet_contact.Text = "0"
        client_contact.Text = "0"

      ElseIf contact_name.SelectedValue <> "" Then
        Dim cont_id As Array = Split(contact_name.SelectedValue, "|")
        Try
          If cont_id(1) = "JETNET" Then
            jetnet_contact.Text = cont_id(0)
            aTempTable = aclsData_Temp.GetContactInfo_JETNET_ID(cont_id(0), "Y")
            If Not IsNothing(aTempTable) Then
              If aTempTable.Rows.Count > 0 Then
                client_contact.Text = IIf(Not IsDBNull(aTempTable.Rows(0).Item("contact_id")), aTempTable.Rows(0).Item("contact_id"), 0)
                'contact email address being set. 
                email_address = IIf(Not IsDBNull(aTempTable.Rows(0).Item("contact_email_address")), aTempTable.Rows(0).Item("contact_email_address"), "")
              ElseIf name.ID = "Email1" Then
                'Sorry, we have to query again if the control is Email1.
                'This is to get the contact Details so we can get the email address.
                'Keep in mind that if there is a contact version of this contact
                'The client email address with automatically 
                'win over the jetnet version.
                aTempTable = aclsData_Temp.GetContacts_Details(cont_id(0), "JETNET")
                If Not IsNothing(aTempTable) Then
                  If aTempTable.Rows.Count > 0 Then
                    email_address = IIf(Not IsDBNull(aTempTable.Rows(0).Item("contact_email_address")), aTempTable.Rows(0).Item("contact_email_address"), "")
                  End If
                End If
              End If
            End If

          Else
            client_contact.Text = cont_id(0)
            aTempTable = aclsData_Temp.GetContacts_Details(cont_id(0), "CLIENT")
            If Not IsNothing(aTempTable) Then
              If aTempTable.Rows.Count > 0 Then
                jetnet_contact.Text = IIf(Not IsDBNull(aTempTable.Rows(0).Item("contact_jetnet_contact_id")), aTempTable.Rows(0).Item("contact_jetnet_contact_id"), 0)
                'contact email address being set. 
                email_address = IIf(Not IsDBNull(aTempTable.Rows(0).Item("contact_email_address")), aTempTable.Rows(0).Item("contact_email_address"), "")
              End If
              If name.ID = "Email1" Then
                'Sorry, we have to query again if the control is Email1.
                'This is to get the contact Details so we can get the email address.
                'Keep in mind that if there is a contact version of this contact
                'The client email address with automatically 
                'win over the jetnet version.
                aTempTable = aclsData_Temp.GetContacts_Details(cont_id(0), "CLIENT")
                If Not IsNothing(aTempTable) Then
                  If aTempTable.Rows.Count > 0 Then
                    email_address = IIf(Not IsDBNull(aTempTable.Rows(0).Item("contact_email_address")), aTempTable.Rows(0).Item("contact_email_address"), "")
                  End If
                End If
              End If
            End If

          End If
          If name.ID = "Email1" Then
            If email_address.ToString <> "" Then 'Here we go.
              'If this email address is actually not blank, 
              'we're going to fill in the to label.
              'we're also going to switch the visibility of 
              'add note to true meaning they'll be able to send an email.
              'This also means that if the contact email address is there, it'll overwrite the
              'company email address in the email_to label's text property. 
              add_note.Visible = True
              email_to.Text = email_address
            Else 'it's blank? okay set to visible = false 
              add_note.Visible = False
              email_to.Text = ""
            End If
          End If
        Catch ex As Exception
          error_string = "edit_note.aspx.vb - contact_name_changed() - " & ex.Message
          clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        End Try
      End If
    Catch ex As Exception
      error_string = "edit_note.aspx.vb - contact_name_changed() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Function what_Note_user(ByVal x As Object) As String
    what_Note_user = ""
    Try
      If IsDBNull(x) Then
      Else
        If IsNumeric(x) Then
          aTempTable = aclsData_Temp.Get_Client_User(CInt(x))
          If Not IsNothing(aTempTable) Then
            If aTempTable.Rows.Count > 0 Then
              For Each r As DataRow In aTempTable.Rows
                what_Note_user = r("cliuser_first_name") & " " & Left(r("cliuser_last_name"), 15)
              Next
            End If
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = aclsData_Temp.class_error
              clsGeneral.clsGeneral.LogError("edit_note.aspx.vb - what_Note_user() - " & error_string, aclsData_Temp)
            End If
            display_error()
          End If
        Else
          Return x.ToString
        End If
      End If
    Catch ex As Exception
      error_string = "edit_note.aspx.vb - what_Note_user() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Function
    ''' <summary>
    ''' Remove Note. Called as an event from all the user controls. This basically removes the note and document
    ''' </summary>
    ''' <param name="idnum"></param>
    ''' <param name="con"></param>
    ''' <remarks></remarks>
    ''' 
    Private Sub remove_note(ByVal idnum As Integer, ByVal con As Control, Optional ByRef type As String = "")
        Dim aclsLocal_Notes As New clsLocal_Notes
        Dim document_flag As String = ""
        Dim document_name As String = ""

        If Session.Item("Listing") = 3 Then
            If con.ID = "Documents1" Then
                Session.Item("ac_active_tab") = 13
            ElseIf con.ID = "ActionItems1" Then
                Session.Item("ac_active_tab") = 12
            ElseIf con.ID = "Notes1" Then
                If LCase(type) = "prospect" Then
                    Session.Item("ac_active_tab") = 15
                Else
                    Session.Item("ac_active_tab") = 11
                End If
            End If
        Else
            If con.ID = "Documents1" Then
                Session.Item("company_active_tab") = 5
            ElseIf con.ID = "ActionItems1" Then
                Session.Item("company_active_tab") = 4
            ElseIf con.ID = "Notes1" Then
                Session.Item("company_active_tab") = 3
            End If
        End If

        aclsLocal_Notes.lnote_id = idnum
        'Check to see if we need to remove document.
        If con.ID = "Documents1" Or con.ID = "Email1" Then 'make sure this note is deleted
            aTempTable2 = aclsData_Temp.Get_Local_Notes_Client_NoteID(idnum)
            '' check the state of the DataTable
            If Not IsNothing(aTempTable2) Then
                If aTempTable2.Rows.Count > 0 Then
                    document_flag = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("lnote_document_flag")), aTempTable2.Rows(0).Item("lnote_document_flag"), "")
                    document_name = IIf(Not IsDBNull(aTempTable2.Rows(0).Item("lnote_document_name")), aTempTable2.Rows(0).Item("lnote_document_name"), "")
                End If
            End If

            If document_flag = "L" Then 'document loaded"
                Try



                    If Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Then
                        Dim TheFile As FileInfo = New FileInfo(Server.MapPath("\Documents\") & document_name)
                        If TheFile.Exists Then
                            File.Delete(MapPath(".") & "\Documents\" & document_name)
                        Else
                            Throw New FileNotFoundException()
                        End If
                    Else
                        Dim file_location As String = Replace(LCase(Application.Item("crmClientSiteData").crmClientHostName()), "www.", "")
                        Dim TheFile As FileInfo = New FileInfo("D:\crmDocuments\" & file_location & "\" & document_name)
                        If TheFile.Exists Then
                            File.Delete("D:\crmDocuments\" & file_location & "\" & document_name)
                        Else
                            Throw New FileNotFoundException()
                        End If
                    End If



                Catch ex As FileNotFoundException
                    error_string = "edit_note.aspx.vb - remove_note(ByVal idnum As Integer) - " & ex.Message
                    LogError(error_string)
                Catch ex As Exception
                    error_string = "edit_note.aspx.vb - remove_note(ByVal idnum As Integer) - " & ex.Message
                    LogError(error_string)
                End Try
            End If
        End If
        If aclsData_Temp.Delete_LocalNote(aclsLocal_Notes) = True Then

            Dim acval_id As New Label
            If Not IsNothing(con.FindControl("acval_id")) Then
                acval_id = con.FindControl("acval_id")
            End If

            If Trim(Request("type")) = "value_analysis" Then
                Call aclsData_Temp.Delete_Client_Value_Comparable(Trim(Request("clival_id")))
                If Trim(acval_id.Text) <> "" Then
                    Call aclsData_Temp.Delete_Aircraft_Value(acval_id.Text)
                End If
            End If

            If Trim(Request("ViewID")) = "18" And Trim(Request("ac_ID")) = "" Then
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.$('#ctl00_ContentPlaceHolder1_View_Master1_crmProspectSearchButton').trigger('click');", True)
            Else
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location.href = window.opener.location.href;", True)
            End If

            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
        Else
            If aclsData_Temp.class_error <> "" Then
                error_string = aclsData_Temp.class_error
                clsGeneral.clsGeneral.LogError("edit_note.aspx.vb -  remove_note(ByVal idnum As Integer) - " & error_string, aclsData_Temp)
            End If
            display_error()
        End If
    End Sub

    ''' <summary>
    ''' Add Note, this function deals with:
    ''' Adding a Note
    ''' Adding an Action Item
    ''' Adding a Document
    ''' Sending an Email
    ''' And Updating all of the Above.
    ''' </summary>
    ''' <param name="type">Type of Note</param>
    ''' <param name="name">Name of Control</param>
    ''' <param name="note_ID">Note ID in the case of an update</param>
    ''' <remarks>In order to find some of the controls on the user controls (textboxes, etc) - I've passed the main user control
    ''' that is calling the function (Like Email1). This will allow me to use name.id to check what control it is and to 
    ''' access the controls by going name.findcontrol.</remarks>
    Private Sub Add_Note(ByVal type As String, ByVal name As Control, ByVal note_ID As Integer)

    Dim estval_asking_price As TextBox
    Dim estval_asking_price_value As String = ""
    Dim estval_take_price As TextBox
    Dim estval_take_price_value As String = ""
    Dim estval_estimated_value As TextBox
    Dim authorize_check_box As CheckBox
    Dim estval_estimated_value_value As String = ""
    Dim estval_aftt As TextBox
    Dim estval_aftt_value As String = ""
    Dim estval_total_landings As TextBox
    Dim estval_total_landings_value As String = ""
    Dim authorize_check As Boolean = False
    Dim targetDate As String = ""

    Try
      ''''''''''''''''''''''''''''''''''''''''''''
      'Adding an ability to add a prospect as a note.
      Dim AdditionalNoteClass As New clsLocal_Notes
      Dim AdditionalNote As New CheckBox
      If Not IsNothing(name.FindControl("add_note_automatically_checkbox")) Then
        AdditionalNote = name.FindControl("add_note_automatically_checkbox")
      End If

      If Not IsNothing(name.FindControl("targetdate")) Then
        Dim targetDateText As New TextBox
        targetDateText = name.FindControl("targetdate")
        targetDate = targetDateText.Text
      End If


      Dim type_of_est_value_drop As DropDownList
      Dim type_of_est_value As String = ""
      If Not IsNothing(name.FindControl("estval_type_of")) Then
        type_of_est_value_drop = name.FindControl("estval_type_of")
        type_of_est_value = type_of_est_value_drop.SelectedValue
      End If





      If Not IsNothing(name.FindControl("estval_asking_price")) Then
        estval_asking_price = name.FindControl("estval_asking_price")
        estval_asking_price_value = estval_asking_price.Text
      End If


      If Not IsNothing(name.FindControl("estval_take_price")) Then
        estval_take_price = name.FindControl("estval_take_price")
        estval_take_price_value = estval_take_price.Text
      End If


      If Not IsNothing(name.FindControl("estval_estimated_value")) Then
        estval_estimated_value = name.FindControl("estval_estimated_value")
        estval_estimated_value_value = estval_estimated_value.Text
      End If

      If Not IsNothing(name.FindControl("estval_aftt")) Then
        estval_aftt = name.FindControl("estval_aftt")
        estval_aftt_value = estval_aftt.Text
      End If


      If Not IsNothing(name.FindControl("estval_total_landings")) Then
        estval_total_landings = name.FindControl("estval_total_landings")
        estval_total_landings_value = estval_total_landings.Text
      End If

      If Not IsNothing(name.FindControl("authorize_check")) Then
        authorize_check_box = name.FindControl("authorize_check")
        authorize_check = authorize_check_box.Checked
      End If

      ''''''''''''''''''''''''''''''''''''''''''''
      ''''''''''''''''''''''''''''''''''''''''''''
      '''''''''declare''''''''''''''''''''''''''''
      '''''''''notes follow up action item''''''''
      '''''''''5-15-2012''''''''''''''''''''''''''
      ''''''''''''''''''''''''''''''''''''''''''''
      ''''''''''''''''''''''''''''''''''''''''''''
      Dim follow_up As CheckBox = name.FindControl("follow_up")
      Dim action_item_date As New TextBox
      If Not IsNothing(name.FindControl("action_item_time")) Then
        action_item_date = name.FindControl("action_item_date")
      End If

      Dim action_item_time As New DropDownList
      If Not IsNothing(name.FindControl("action_item_time")) Then
        action_item_time = name.FindControl("action_item_time")
      End If

      Dim action_item_subject As TextBox = name.FindControl("action_item_subject")


      'create a new note class
      Dim aclsLocalNote_FollowUp As New clsLocal_Notes

      ''''''''''''''''''''''''''''''''''''''''''''
      ''''''''''''''''''''''''''''''''''''''''''''
      '''''''''declare end''''''''''''''''''''''''
      '''''''''notes follow up action item''''''''
      '''''''''5-15-2012''''''''''''''''''''''''''
      ''''''''''''''''''''''''''''''''''''''''''''
      ''''''''''''''''''''''''''''''''''''''''''''

      Dim wanted_damage_cur As String = ""
      Dim wanted_damage_hist As String = ""
      Dim wanted_end_year As String = ""
      Dim wanted_start_year As String = ""
      Dim wanted_max_aftt As Nullable(Of Integer)
      Dim wanted_max_price As Nullable(Of Decimal)

      Dim email_from As String = Session.Item("localUser").crmLocalUserEmailAddress
      Dim email_bcc As String = ""
      Dim file_location As String = Replace(LCase(Application.Item("crmClientSiteData").crmClientHostName()), "www.", "")
      If Session.Item("Listing") = 3 Then
        If name.ID = "Documents1" Then
          Session.Item("ac_active_tab") = 13
        ElseIf name.ID = "ActionItems1" Then
          Session.Item("ac_active_tab") = 12
        ElseIf name.ID = "Notes1" Then
          If LCase(type) = "prospect" Then
            Session.Item("ac_active_tab") = 15
          ElseIf LCase(type) = "value_analysis" Then
            Session.Item("company_active_tab") = 16
          Else
            Session.Item("ac_active_tab") = 11
          End If
        End If
      Else
        If name.ID = "Documents1" Then
          Session.Item("company_active_tab") = 5
        ElseIf name.ID = "ActionItems1" Then
          Session.Item("company_active_tab") = 4
        ElseIf name.ID = "Opportunities1" Then
          Session.Item("company_active_tab") = 5
        ElseIf name.ID = "Wanted1" Then
          Session.Item("company_active_tab") = 6
        ElseIf name.ID = "Notes1" Then
          If LCase(type) = "prospect" Then
            Session.Item("company_active_tab") = 9
          Else
            Session.Item("company_active_tab") = 3
          End If
        End If
      End If
      'dropdown lists
      Dim company_name As DropDownList = name.FindControl("company_name")
      Dim notes_cat As New DropDownList
      Dim notes_title As New TextBox
      Dim cat As Integer = 0
      Dim opp_cat As Integer = 0
      If Not IsNothing(name.FindControl("notes_cat")) Then
        notes_cat = name.FindControl("notes_cat")
        cat = notes_cat.SelectedValue
      Else
        notes_cat.Items.Add(New ListItem("", 0))
      End If
      If UCase(type) = "PROSPECT" Or UCase(type) = "OPPORTUNITY" Then
        If Not IsNothing(name.FindControl("notes_opp")) Then
          notes_cat = name.FindControl("notes_opp")
          opp_cat = notes_cat.SelectedValue
        Else
          notes_cat.Items.Add(New ListItem("", 0))
        End If
      End If
      Dim check_document As Boolean = False
      Dim action_cc As New TextBox
      If Not IsNothing(name.FindControl("action_cc")) Then
        action_cc = name.FindControl("action_cc")
      End If

      If Not IsNothing(name.FindControl("wanted_damage_cur")) Then
        Dim damage_cur As RadioButtonList = name.FindControl("wanted_damage_cur")
        wanted_damage_cur = damage_cur.SelectedValue
      End If

      If Not IsNothing(name.FindControl("wanted_damage_hist")) Then
        Dim damage_hist As RadioButtonList = name.FindControl("wanted_damage_hist")
        wanted_damage_hist = damage_hist.SelectedValue
      End If

      If Not IsNothing(name.FindControl("wanted_year_end")) Then
        Dim wanted_end As DropDownList = name.FindControl("wanted_year_end")
        wanted_end_year = wanted_end.SelectedValue
      End If

      If Not IsNothing(name.FindControl("wanted_year_start")) Then
        Dim wanted_start As DropDownList = name.FindControl("wanted_year_start")
        wanted_start_year = wanted_start.SelectedValue
      End If

      If Not IsNothing(name.FindControl("wanted_max_aftt")) Then
        Dim wanted_aftt As TextBox = name.FindControl("wanted_max_aftt")
        If wanted_aftt.Text <> "" Then
          wanted_max_aftt = wanted_aftt.Text
        End If
      End If

      If Not IsNothing(name.FindControl("wanted_max_price")) Then

        Dim wanted_max As TextBox = name.FindControl("wanted_max_price")
        If wanted_max.Text <> "" Then
          wanted_max_price = wanted_max.Text
        End If
      End If

      If Not IsNothing(name.FindControl("store_document")) Then
        Dim store_doc As CheckBox = name.FindControl("store_document")
        check_document = store_doc.Checked
      Else
        check_document = False
      End If

      Dim opp_status As String = ""
      If Not IsNothing(name.FindControl("opp_status")) Then
        Dim store_doc As RadioButtonList = name.FindControl("opp_status")
        opp_status = store_doc.SelectedValue
      Else
        opp_status = ""
      End If
      Dim cash As Integer = 0
      If Not IsNothing(name.FindControl("opp_cash")) Then
        Dim opp_cash As TextBox = name.FindControl("opp_cash")
        If IsNumeric(opp_cash.Text) Then
          cash = opp_cash.Text
        End If
      Else
        cash = 0
      End If

      Dim percent As Integer = 0
      If Not IsNothing(name.FindControl("capt_per")) Then
        Dim capt_per As DropDownList = name.FindControl("capt_per")
        percent = capt_per.SelectedValue
      Else
        percent = 0
      End If
      If Not IsNothing(name.FindControl("notes_title")) Then
        notes_title = name.FindControl("notes_title")
      End If
      'Set up the email from BCC checkbox working 
      Dim email_from_bcc As New CheckBox

      If Not IsNothing(name.FindControl("email_from_bcc")) Then
        email_from_bcc = name.FindControl("email_from_bcc")
      End If

      If email_from_bcc.Checked = True Then
        email_bcc = Session.Item("localUser").crmLocalUserEmailAddress
      End If


      Dim contact_name As DropDownList = name.FindControl("contact_name")
      Dim aircraft_name As DropDownList = name.FindControl("aircraft_name")
      Dim pertaining_to As New DropDownList
      If Not IsNothing(name.FindControl("pertaining_to")) Then
        pertaining_to = name.FindControl("pertaining_to")
      Else
        pertaining_to.Items.Add(New ListItem("", 0))
      End If
      Dim priority As New DropDownList
      If Not IsNothing(name.FindControl("priority")) Then
        priority = name.FindControl("priority")
      End If
      If UCase(type) = "PROSPECT" Then
        priority = name.FindControl("priorityID")
      End If

      Dim time As New DropDownList
      If Not IsNothing(name.FindControl("time")) Then
        time = name.FindControl("time")
      End If


      'checkboxes
      'Checking to see if automatic prospect checkbox is checked, making sure type is note.
      Dim AddAutomaticProspect As New CheckBox
      If UCase(type) = "NOTE" Then
        AddAutomaticProspect = name.FindControl("add_prospect_automatically_checkbox")
      End If


      Dim company_related As CheckBox = name.FindControl("company_related")
      Dim aircraft_related As CheckBox = name.FindControl("aircraft_related")
      Dim email_pertaining As New CheckBox
      If name.ID = "ActionItems1" Or UCase(type) = "NOTE" Then
        email_pertaining = name.FindControl("email_pertaining")
      End If

      'textboxes
      Dim jetnet_contact As TextBox = name.FindControl("jetnet_contact")
      Dim client_contact As TextBox = name.FindControl("client_contact")
      Dim notes_edit As TextBox = name.FindControl("notes_edit")
      Dim jetnet_comp As TextBox = name.FindControl("jetnet_comp")
      Dim client_comp As TextBox = name.FindControl("client_comp")
      Dim jetnet_ac As TextBox = name.FindControl("jetnet_ac")
      Dim client_ac As TextBox = name.FindControl("client_ac")
      Dim jetnet_mod As TextBox = name.FindControl("jetnet_mod")
      Dim client_mod As TextBox = name.FindControl("client_mod")
      Dim dated As New TextBox
      If Not IsNothing(name.FindControl("dated")) Then
        dated = name.FindControl("dated")
      End If

      Dim note_date As New TextBox

      If name.ID = "Notes1" Or name.ID = "Documents1" Or name.ID = "Wanted1" Then
        note_date = name.FindControl("note_date")
      End If
      If name.ID = "Wanted1" Then
        email_pertaining = New CheckBox
      End If

      'radio button lists
      Dim statused As New RadioButtonList
      If Not IsNothing(name.FindControl("statused")) Then
        statused = name.FindControl("statused")
      End If

      'labels
      Dim existing_docs As New Label
      If Not IsNothing(name.FindControl("existing_docs")) Then
        existing_docs = name.FindControl("existing_docs")
      End If
      Dim current As New Label
      If Not IsNothing(name.FindControl("current")) Then
        current = name.FindControl("current")
      End If
      'fileupload
      Dim FileUpload1 As New FileUpload
      If Not IsNothing(name.FindControl("FileUpload1")) Then
        FileUpload1 = name.FindControl("FileUpload1")
      End If
      'Declare notes class
      Dim aclsLocal_Notes As New clsLocal_Notes
      Dim email_body As String = ""
      Dim email_subject As String = ""
      Dim email_to As String = ""
      Dim email_cc As String = ""

      Dim web_url As New TextBox
      Dim remote_document As New CheckBox

      Dim old_file As New TextBox
      If name.ID = "Documents1" Then
        web_url = Documents1.FindControl("remote_url")
        remote_document = Documents1.FindControl("stored_remotely")
        notes_title = Documents1.FindControl("notes_title")
        old_file = Documents1.FindControl("notes_old_document_title")
      End If

      Dim document_title As String = ""
      '& contact_name.Text & aircraft_name.Text
      Dim startdate As String = ""
      Dim enddated As String = ""
      Dim enddate As Date
      Dim new_id As Integer = 0
      Dim internal As String = "N"
      Dim retail As String = "N"
      Dim ViewToPDF As String = ""
      Dim document_name As String = ""
      Dim acval_id As Long = 0


      Dim offset As Integer = CInt(Session("timezone_offset")) * -1
      If UCase(type) = "PROSPECT" Then
        aclsLocal_Notes.lnote_capture_percentage = percent
        aclsLocal_Notes.lnote_cash_value = cash
        aclsLocal_Notes.lnote_opportunity_status = opp_status
        aclsLocal_Notes.lnote_notecat_key = opp_cat
      End If

      '--------------------------------------- ----------begin insertion -------------------------------------------------------
      'No jetnet company exists for this client company. 
      If jetnet_comp.Text = "0" Then
        aclsLocal_Notes.lnote_client_comp_id = client_comp.Text
        aclsLocal_Notes.lnote_jetnet_comp_id = 0
      Else 'Does a client company exists for this jetnet company? 
        aTempTable = aclsData_Temp.GetCompanyInfo_JETNET_ID(jetnet_comp.Text, "")
        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count = 0 Then 'No it doesn't, we actually need to make one.
            new_id = 0 'Fill_Company(jetnet_comp.Text)
          Else
            new_id = aTempTable.Rows(0).Item("comp_id") 'Yes it does, here it is.
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            clsGeneral.clsGeneral.LogError("edit_note.aspx.vb - add_note() - " & error_string, aclsData_Temp)
          End If
        End If
        'setting the class values correctly. 
        aclsLocal_Notes.lnote_client_comp_id = new_id
        aclsLocal_Notes.lnote_jetnet_comp_id = jetnet_comp.Text
      End If
      new_id = 0 'resetting the new id value
      If jetnet_contact.Text = "0" Then 'There's no jetnet contact for this contact
        aclsLocal_Notes.lnote_client_contact_id = client_contact.Text
        aclsLocal_Notes.lnote_jetnet_contact_id = 0
      Else 'Is there a client contact for this?
        aTempTable = aclsData_Temp.GetContactInfo_JETNET_ID(jetnet_contact.Text, "Y")
        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count = 0 Then
            new_id = 0 'no
          Else 'yes
            new_id = aTempTable.Rows(0).Item("contact_id")
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            clsGeneral.clsGeneral.LogError("edit_note.aspx.vb - add_note() - " & error_string, aclsData_Temp)
          End If
        End If 'setting the class values correctly. 
        aclsLocal_Notes.lnote_client_contact_id = new_id
        aclsLocal_Notes.lnote_jetnet_contact_id = jetnet_contact.Text
      End If
      'starting to fill the class in. 

      aclsLocal_Notes.lnote_document_flag = "N"
      If name.ID = "Documents1" Or UCase(type) = "PROSPECT" Then
        aclsLocal_Notes.lnote_note = HttpUtility.HtmlEncode(notes_title.Text) & " ::: " & HttpUtility.HtmlEncode(notes_edit.Text)

      ElseIf name.ID = "Email1" Then
        Dim email_to_txt As Label = name.FindControl("email_to")
        Dim email_cc_txt As TextBox = name.FindControl("email_cc")
        Dim email_subject_txt As TextBox = name.FindControl("email_subject")
        Dim email_body_txt As AjaxControlToolkit.HTMLEditor.Editor = name.FindControl("body")
        email_to = email_to_txt.Text
        email_cc = email_cc_txt.Text
        email_subject = email_subject_txt.Text
        email_body = email_body_txt.Content
        aclsLocal_Notes.lnote_note = HttpUtility.HtmlEncode(email_to_txt.Text) & " ::: " & HttpUtility.HtmlEncode(email_cc_txt.Text) & " ::: " & HttpUtility.HtmlEncode(email_subject_txt.Text) & " ::: " & HttpUtility.HtmlEncode(email_body_txt.Content) & " ::: " & HttpUtility.HtmlEncode(notes_edit.Text)
        aclsLocal_Notes.lnote_status = "E"
      ElseIf name.ID = "Email1" Then

      Else
        aclsLocal_Notes.lnote_note = HttpUtility.HtmlEncode(notes_edit.Text)
      End If
      aclsLocal_Notes.lnote_entry_date = Now() ' DB requires some value
      aclsLocal_Notes.lnote_action_date = Now() ' DB requires some value
      aclsLocal_Notes.lnote_user_login = Session.Item("localUser").crmLocalUserID ' DB requires a string value greater than 0
      aclsLocal_Notes.lnote_user_name = Left(Session.Item("localUser").crmLocalUserFirstName & " " & Session.Item("localUser").crmLocalUserLastName, 15)

      If opp_cat = 0 Then
        aclsLocal_Notes.lnote_notecat_key = cat
      End If

      aclsLocal_Notes.lnote_user_id = pertaining_to.SelectedValue

      aclsLocal_Notes.lnote_jetnet_ac_id = jetnet_ac.Text
      aclsLocal_Notes.lnote_client_ac_id = client_ac.Text

      'This is where we're going to add a small check very quick if the client_ac is 0, just to poll the database to see
      'if a client aircraft has been created since the page has loaded.
      If aclsLocal_Notes.lnote_jetnet_ac_id > 0 And aclsLocal_Notes.lnote_client_ac_id = 0 Then
        Dim CheckTable As New DataTable
        CheckTable = aclsData_Temp.CHECKFORClient_Aircraft_JETNET_AC(aclsLocal_Notes.lnote_jetnet_ac_id)
        If Not IsNothing(CheckTable) Then
          If CheckTable.Rows.Count > 0 Then
            aclsLocal_Notes.lnote_client_ac_id = CheckTable.Rows(0).Item("cliaircraft_id")
          End If
        End If
      End If

      aclsLocal_Notes.lnote_jetnet_amod_id = jetnet_mod.Text
      aclsLocal_Notes.lnote_client_amod_id = client_mod.Text

      If priority.SelectedValue = "" Then
        aclsLocal_Notes.lnote_clipri_ID = 1
      Else
        aclsLocal_Notes.lnote_clipri_ID = priority.SelectedValue
      End If

      If type = "action" Or statused.SelectedValue = "C" Then
        startdate = DateAdd("h", offset, CDate(dated.Text & " " & time.SelectedValue))
        enddate = DateAdd(DateInterval.Minute, 30, CDate(startdate))
        enddated = Year(enddate) & "-" & Month(enddate) & "-" & (Day(enddate)) & " " & FormatDateTime(enddate, 4) & ":" & Second(enddate)
        startdate = Year(startdate) & "-" & Month(startdate) & "-" & (Day(startdate)) & " " & FormatDateTime(startdate, 4) & ":" & Second(startdate)
        aclsLocal_Notes.lnote_schedule_start_date = startdate
        aclsLocal_Notes.lnote_schedule_end_date = enddated
      ElseIf type = "prospect" Then
        If Not String.IsNullOrEmpty(targetDate) Then
          aclsLocal_Notes.lnote_schedule_start_date = Year(targetDate) & "-" & Month(targetDate) & "-" & (Day(targetDate)) & " " & FormatDateTime(targetDate, 4) & ":" & Second(targetDate)
        End If
      End If

      If type = "action" And statused.SelectedValue <> "C" Then
        aclsLocal_Notes.lnote_status = statused.SelectedValue
      ElseIf type = "documents" Then
        aclsLocal_Notes.lnote_status = "F"

        If note_date.Text <> "" Then
          aclsLocal_Notes.lnote_entry_date = CDate(note_date.Text & " " & time.SelectedValue)
        ElseIf current.Text <> "" Then
          aclsLocal_Notes.lnote_entry_date = CDate(current.Text)
        End If
      Else 'note

        If name.ID = "Email1" Then
          aclsLocal_Notes.lnote_status = "E"
          aclsLocal_Notes.lnote_user_id = Session.Item("localUser").crmLocalUserID
        ElseIf name.ID = "Opportunities1" Then
          aclsLocal_Notes.lnote_user_id = pertaining_to.SelectedValue 'assigned to 
          aclsLocal_Notes.lnote_user_login = Session.Item("localUser").crmLocalUserID
          aclsLocal_Notes.lnote_status = "O"
        ElseIf UCase(type) = "PROSPECT" Then
          aclsLocal_Notes.lnote_status = "B"
          aclsLocal_Notes.lnote_user_name = Left(what_Note_user(pertaining_to.SelectedValue), 15)
        ElseIf UCase(type) = "VALUATION" Then
          aclsLocal_Notes.lnote_status = "V"
          'For some reason prospect uses statuses A/I but valuation uses statuses O/C. So this is just a small check
          'To turn the prospect status to a valuation status so that it will save correctly.
          If opp_status = "A" Then
            aclsLocal_Notes.lnote_opportunity_status = "O"
          ElseIf opp_status = "I" Then
            aclsLocal_Notes.lnote_opportunity_status = "C"
          Else
            aclsLocal_Notes.lnote_opportunity_status = opp_status
          End If
        ElseIf UCase(type) = "VALUE_ANALYSIS" Then
          aclsLocal_Notes.lnote_status = "D"
          aclsLocal_Notes.lnote_user_login = pertaining_to.SelectedValue
          aclsLocal_Notes.lnote_user_name = Left(what_Note_user(pertaining_to.SelectedValue), 15)

          If Trim(estval_asking_price_value) <> "" Then
            aclsLocal_Notes.lnote_estval_asking_price = estval_asking_price_value
          Else
            aclsLocal_Notes.lnote_estval_asking_price = 0
          End If

          If Trim(estval_take_price_value) <> "" Then
            aclsLocal_Notes.lnote_estval_take_price = estval_take_price_value
          Else
            aclsLocal_Notes.lnote_estval_take_price = 0
          End If

          If Trim(estval_estimated_value_value) <> "" Then
            aclsLocal_Notes.lnote_estval_estimated_value = estval_estimated_value_value
          Else
            aclsLocal_Notes.lnote_estval_estimated_value = 0
          End If

          If Trim(estval_aftt_value) <> "" Then
            aclsLocal_Notes.lnote_estval_aftt = estval_aftt_value
          Else
            aclsLocal_Notes.lnote_estval_aftt = 0
          End If

          If Trim(estval_total_landings_value) <> "" Then
            aclsLocal_Notes.lnote_estval_total_landings = estval_total_landings_value
          Else
            aclsLocal_Notes.lnote_estval_total_landings = 0
          End If

          aclsLocal_Notes.lnote_estval_type = type_of_est_value
          aclsLocal_Notes.lnote_note = HttpUtility.HtmlEncode(notes_edit.Text)
        Else
          aclsLocal_Notes.lnote_status = "A"
          aclsLocal_Notes.lnote_user_login = pertaining_to.SelectedValue
          aclsLocal_Notes.lnote_user_name = Left(what_Note_user(pertaining_to.SelectedValue), 15)
        End If


        If note_date.Text <> "" Then
          aclsLocal_Notes.lnote_entry_date = CDate(note_date.Text & " " & time.SelectedValue)
        ElseIf current.Text <> "" Then
          aclsLocal_Notes.lnote_entry_date = CDate(current.Text)
        End If

        If UCase(type) = "PROSPECT" Then
        Else
          aclsLocal_Notes.lnote_clipri_ID = 1
        End If

      End If
      If name.ID = "Documents1" Then
        aclsLocal_Notes.lnote_user_id = Session.Item("localUser").crmLocalUserID
        aclsLocal_Notes.lnote_user_login = Session.Item("localUser").crmLocalUserID
      End If
      If name.ID = "Wanted1" Then
        aclsLocal_Notes.lnote_wanted_damage_cur = wanted_damage_cur
        aclsLocal_Notes.lnote_wanted_damage_hist = wanted_damage_hist
        aclsLocal_Notes.lnote_wanted_end_year = wanted_end_year
        aclsLocal_Notes.lnote_wanted_start_year = wanted_start_year

        aclsLocal_Notes.lnote_wanted_max_aftt = wanted_max_aftt
        aclsLocal_Notes.lnote_wanted_max_price = wanted_max_price

        If aclsLocal_Notes.lnote_client_amod_id = 0 Then
          Dim model_table As New DataTable
          Dim model_id As Integer = 0
          'This is unfortunately where we need to insert a client model because one doesn't exist.
          If aclsLocal_Notes.lnote_jetnet_amod_id <> 0 Then
            model_table = aclsData_Temp.Get_JETNET_Aircraft_Model_amodID(aclsLocal_Notes.lnote_jetnet_amod_id)
            If Not IsNothing(model_table) Then
              If model_table.Rows.Count > 0 Then
                model_id = clsGeneral.clsGeneral.Create_Model(aclsData_Temp, model_table.Rows(0).Item("amod_airframe_type"), model_table.Rows(0).Item("amod_make_name"), model_table.Rows(0).Item("amod_make_type"), model_table.Rows(0).Item("amod_manufacturer_name"), model_table.Rows(0).Item("amod_model_name"), aclsLocal_Notes.lnote_jetnet_amod_id)
              End If
            End If
          End If
          aclsLocal_Notes.lnote_client_amod_id = model_id
        End If

        If note_date.Text <> "" Then
          aclsLocal_Notes.lnote_schedule_start_date = note_date.Text
        Else
          aclsLocal_Notes.lnote_schedule_start_date = Now()
        End If
        aclsLocal_Notes.lnote_status = "W"
      End If
      If name.ID = "ActionItems1" Or UCase(type) = "NOTE" Then
        If email_pertaining.Checked = True Then
          If UCase(type) = "NOTE" Then
            Build_Actions_Email(pertaining_to, jetnet_ac, client_ac, jetnet_comp, client_comp, jetnet_contact, client_contact, notes_edit, DateAdd("h", offset, CDate(action_item_date.Text & " " & action_item_time.SelectedValue)), enddate, action_cc)
          Else
            Build_Actions_Email(pertaining_to, jetnet_ac, client_ac, jetnet_comp, client_comp, jetnet_contact, client_contact, notes_edit, startdate, enddate, action_cc)
          End If
        End If
      End If

      If name.ID = "Email1" Then
        Send_Email(email_from, email_to, email_bcc, email_cc, email_subject, email_body, aclsData_Temp)
      End If
      '-------------------------------------------------------------document title -------------------------------------------------
      If type = "documents" And FileUpload1.FileName = "" Then 'remote doc or just a simple update without document upload

        If remote_document.Checked = True Then

          aclsLocal_Notes.lnote_document_flag = "R"
          aclsLocal_Notes.lnote_document_name = web_url.Text
        Else
          aclsLocal_Notes.lnote_document_flag = "L"
          document_title = Trim(Build_File_Name(jetnet_contact, client_contact, client_comp, jetnet_comp, client_ac, jetnet_ac)) & System.IO.Path.GetExtension(old_file.Text).ToLower()
          aclsLocal_Notes.lnote_document_name = clsGeneral.clsGeneral.StripChars(document_title, False)
          If Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Then
            File.Move(Server.MapPath("\Documents\") & old_file.Text, Server.MapPath("\Documents\") & aclsLocal_Notes.lnote_document_name)
          Else
            File.Move("D:\crmDocuments\" & file_location & "\" & old_file.Text, "D:\crmDocuments\" & file_location & "\" & aclsLocal_Notes.lnote_document_name)
          End If


        End If

        If note_ID <> 0 Then
          aclsLocal_Notes.lnote_id = note_ID
          aclsData_Temp.update_localNote(aclsLocal_Notes)
        Else
          aclsData_Temp.Insert_Note(aclsLocal_Notes)
        End If

        If Session.Item("Listing") <> 9 Then
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location.href='details.aspx';", True)
        ElseIf Session.Item("Listing") = 9 Then
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location.href='home.aspx';", True)
        End If

        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
      ElseIf FileUpload1.FileName <> "" And (type = "documents" Or check_document = True) Then
        Dim uploads As HttpFileCollection
        uploads = HttpContext.Current.Request.Files
        Dim text As String = ""
        Dim named As String = ""
        Dim count As Integer = 0
        Dim pass = True
        Dim old_file_name As String = ""
        Dim fileOK As Boolean = False
        Dim fileExtension As String
        fileExtension = System.IO.Path.GetExtension(FileUpload1.FileName).ToLower()
        'xls, powerpoint 
        Dim allowedExtensions As String() = _
          {".jpg", ".jpeg", ".png", ".gif", ".pdf", ".doc", ".docx", ".txt", ".xls", ".xlsx", ".ppt", ".pptx", ".pps", ".ppsx"}
        For i As Integer = 0 To allowedExtensions.Length - 1
          If fileExtension = allowedExtensions(i) Then
            fileOK = True
          End If
        Next
        If fileOK = True Then
          If note_ID <> 0 Then
            aTempTable = aclsData_Temp.Get_Local_Notes_Client_NoteID(note_ID)
            '' check the state of the DataTable
            If Not IsNothing(aTempTable) Then
              If aTempTable.Rows.Count > 0 Then
                old_file_name = aTempTable.Rows(0).Item("lnote_document_name")
              End If
            End If
          End If

          document_title = Build_File_Name(jetnet_contact, client_contact, client_comp, jetnet_comp, client_ac, jetnet_ac)

          document_title = document_title & fileExtension
          aclsLocal_Notes.lnote_document_name = document_title


          If note_ID = 0 Then
            aclsLocal_Notes.lnote_document_flag = "L"
            note_ID = aclsData_Temp.Insert_Note(aclsLocal_Notes)
          Else
            aclsLocal_Notes.lnote_id = note_ID
            document_title = note_ID & "-" & document_title
            aclsLocal_Notes.lnote_document_name = document_title
            aclsData_Temp.update_localNote(aclsLocal_Notes)

            Try
              Dim TheFile As FileInfo
              If Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Then
                TheFile = New FileInfo(Server.MapPath("\Documents\") & old_file_name)
              Else
                TheFile = New FileInfo("D:\crmDocuments\" & file_location & "\" & old_file_name)
              End If

              If TheFile.Exists Then
                If Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Then
                  File.Delete(MapPath(".") & "\Documents\" & old_file_name)
                Else
                  File.Delete("D:\crmDocuments\" & file_location & "\" & old_file_name)
                End If
              Else
                Throw New FileNotFoundException()
              End If

            Catch ex As FileNotFoundException
              error_string = "edit_note.aspx.vb - remove document - not found() - " & ex.Message
              LogError(error_string)
            Catch ex As Exception
              error_string = "edit_note.aspx.vb - remove document() - " & ex.Message
              LogError(error_string)
            End Try
          End If

          If note_ID <> 0 Then
            If pass = True Then
              Try
                document_title = note_ID & "-" & document_title
                document_title = clsGeneral.clsGeneral.StripChars(document_title, False)
                If Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Then
                  FileUpload1.PostedFile.SaveAs(Server.MapPath("\Documents\") & document_title)
                Else
                  FileUpload1.PostedFile.SaveAs("D:\crmDocuments\" & file_location & "\" & document_title)
                End If
                'reupdate because it saved

                aclsLocal_Notes.lnote_document_name = document_title
                aclsLocal_Notes.lnote_document_flag = "L"
                aclsLocal_Notes.lnote_id = note_ID
                aclsData_Temp.update_localNote(aclsLocal_Notes)
              Catch ex As Exception 'document not loaded, no update of data in row
                error_string = "edit_note.aspx.vb - add_note() - " & ex.Message
                LogError(error_string)
              End Try
            End If

            If Session.Item("Listing") <> 9 Then
              System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location.href='details.aspx';", True)
            ElseIf Session.Item("Listing") = 9 Then
              System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location.href='home.aspx';", True)
            End If

            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
          End If
        ElseIf fileOK = False Then
          'bad document
          If type = "documents" Then
            Dim attention As Label = Documents1.FindControl("attention")

            attention.Text = "<p align='center'>Please choose a different file format.</p><p align='center'>Acceptable formats are: .jpg, .jpeg, .png, .gif, .pdf, .doc, .txt, .xls, .xlsx, .ppt, .pptx, .pps, .ppsx</p>"
          End If
        End If

        '---------------------------------------Not a Document or a remotelt stored document!
      Else 'not document
        If note_ID = 0 Then
          note_ID = aclsData_Temp.Insert_Note(aclsLocal_Notes)

          If Session.Item("localSubscription").crmAerodexFlag = False Then
            If AddAutomaticProspect.Checked Then
              If UCase(type) = "NOTE" Then
                'Slightly ammending the notes class to resend it as a matching prospect if box is checked.
                aclsLocal_Notes.lnote_id = 0
                aclsLocal_Notes.lnote_status = "B"
                aclsLocal_Notes.lnote_opportunity_status = "A"
                aclsData_Temp.Insert_Note(aclsLocal_Notes)
              End If
            End If
            If AdditionalNote.Checked Then
              If UCase(type) = "PROSPECT" Then
                'Slightly ammending the notes class to resend it as a matching note (for prospect) is checked
                aclsLocal_Notes.lnote_id = 0
                aclsLocal_Notes.lnote_status = "A"
                aclsLocal_Notes.lnote_opportunity_status = ""
                aclsData_Temp.Insert_Note(aclsLocal_Notes)
              End If
            End If
          End If


          'update after insert of the note.
          'This will run if it's originally a valuation or if you're saving a valuation in addition to a note. 
          If aclsLocal_Notes.lnote_status = "V" Then
            aclsData_Temp.Insert_Client_Value_Comparable(note_ID, "F", aclsLocal_Notes.lnote_client_ac_id, 0, "P", aclsLocal_Notes.lnote_jetnet_ac_id)
          ElseIf UCase(type) = "VALUE_ANALYSIS" Then
            aclsData_Temp.Insert_Client_Value_Comparable(note_ID, aclsLocal_Notes.lnote_estval_type, aclsLocal_Notes.lnote_client_ac_id, 0, "P", aclsLocal_Notes.lnote_jetnet_ac_id, aclsLocal_Notes)

            If authorize_check = True Then
              Dim aclsUpdate_Client_Transactions As New clsClient_Transactions
              aclsUpdate_Client_Transactions.clitrans_airframe_total_hours = aclsLocal_Notes.lnote_estval_aftt
              aclsUpdate_Client_Transactions.clitrans_airframe_total_landings = aclsLocal_Notes.lnote_estval_total_landings
              aclsUpdate_Client_Transactions.clitrans_asking_price = aclsLocal_Notes.lnote_estval_asking_price
              aclsUpdate_Client_Transactions.clitrans_est_price = aclsLocal_Notes.lnote_estval_take_price
              aclsUpdate_Client_Transactions.clitrans_sold_price = aclsLocal_Notes.lnote_estval_estimated_value
              aclsUpdate_Client_Transactions.clitrans_type = aclsLocal_Notes.lnote_estval_type
              aclsUpdate_Client_Transactions.clitrans_action_date = Date.Now()
              aclsUpdate_Client_Transactions.clitrans_date = aclsLocal_Notes.lnote_entry_date
              aclsUpdate_Client_Transactions.clitrans_cliamod_id = aclsLocal_Notes.lnote_jetnet_amod_id
              aclsUpdate_Client_Transactions.clitrans_jetnet_ac_id = aclsLocal_Notes.lnote_jetnet_ac_id
              aclsUpdate_Client_Transactions.clitrans_value_description = aclsLocal_Notes.lnote_note


              Call aclsData_Temp.Insert_Into_Aircraft_Value(aclsUpdate_Client_Transactions, 0, False, "est_value")
            End If
          End If
        Else
          aclsLocal_Notes.lnote_id = note_ID
          document_title = "" 'note_ID & "-" & document_title
          aclsLocal_Notes.lnote_document_name = document_title




          'We need to run a check here for the value analysis:
          Dim SavedAFTT As String = ""
          If aclsLocal_Notes.lnote_status = "V" And aclsLocal_Notes.lnote_opportunity_status = "C" Then


            If Trim(Request("internal")) <> "" Then
              internal = Trim(Request("internal"))
            End If

            If Trim(Request("retail")) <> "" Then
              retail = Trim(Request("retail"))
            End If


            'HttpContext.Current.Session.Item("jetnetServerNotesDatabase") = CApplication.Item("crmClientDatabase")

            localDatalayer = New viewsDataLayer
            localDatalayer.clientConnectStr = Session.Item("localPreferences").UserDatabaseConn
            localDatalayer.starConnectStr = Session.Item("localPreferences").STARDatabaseConn
            localDatalayer.serverConnectStr = Session.Item("localPreferences").ServerNotesDatabaseConn

            Call crmViewDataLayer.get_valuation_details(note_ID, localDatalayer, Session("CLIENT_AC_ID"), Session("JETNET_AC_ID"), LAST_SAVE_DATE, completed_or_open, False, "")
            ViewToPDF = ViewToPDF & crmViewDataLayer.Build_Compare_Graphs(client_ac.Text, note_ID, False, internal, retail, localDatalayer, ANALYTICS_HISTORY, Server.MapPath("TempFiles"), completed_or_open, searchCriteria, Me.Page, Me.bottom_tab_update_panel, True, True, True, True, True, False, "")

            '  Response.AppendHeader("Content-Type", "application/msword")

            '  Response.AppendHeader("Content-disposition", "attachment; filename=COMPLETED_VALUE_ANALYSIS_FULL_PDF.doc")
            ' call the Build HTML Page
            ViewToPDF = ViewToPDF & "</body></html>"
            ' call the Output String to HTML file

            document_name = note_ID & "_COMPLETED_VALUE_ANALYSIS_FULL_PDF.html"
            If crmViewDataLayer.write_report_string_to_file(ViewToPDF, document_name) Then
              Call crmViewDataLayer.convert_to_pdf(document_name)
            End If


            ''--------------- CREATE SINGLE FOR MARKET STATUS---------------------
            ViewToPDF = ""
            Call crmViewDataLayer.get_valuation_details(note_ID, localDatalayer, Session("CLIENT_AC_ID"), Session("JETNET_AC_ID"), LAST_SAVE_DATE, completed_or_open, False, "")
            ViewToPDF = ViewToPDF & crmViewDataLayer.Build_Compare_Graphs(client_ac.Text, note_ID, False, internal, retail, localDatalayer, ANALYTICS_HISTORY, Server.MapPath("TempFiles"), completed_or_open, searchCriteria, Me.Page, Me.bottom_tab_update_panel, False, False, True, False, False, True, "")


            ViewToPDF = ViewToPDF & "</body></html>"
            ' call the Output String to HTML file

            document_name = note_ID & "_COMPLETED_MARKET_STATUS_PDF.html"
            If crmViewDataLayer.write_report_string_to_file(ViewToPDF, document_name) Then
              Call crmViewDataLayer.convert_to_pdf(document_name)
            End If
            ''--------------- CREATE SINGLE FOR MARKET STATUS---------------------


            '--------------- CREATE SINGLE FOR MARKET SURVEY---------------------
            ViewToPDF = ""
            Call crmViewDataLayer.get_valuation_details(note_ID, localDatalayer, Session("CLIENT_AC_ID"), Session("JETNET_AC_ID"), LAST_SAVE_DATE, completed_or_open, False, "")
            ViewToPDF = ViewToPDF & crmViewDataLayer.Build_Compare_Graphs(client_ac.Text, note_ID, False, internal, retail, localDatalayer, ANALYTICS_HISTORY, Server.MapPath("TempFiles"), completed_or_open, searchCriteria, Me.Page, Me.bottom_tab_update_panel, False, True, False, False, False, True, "")


            ViewToPDF = ViewToPDF & "</body></html>"
            ' call the Output String to HTML file

            document_name = note_ID & "_COMPLETED_MARKET_SURVEY_PDF.html"
            If crmViewDataLayer.write_report_string_to_file(ViewToPDF, document_name) Then
              Call crmViewDataLayer.convert_to_pdf(document_name)
            End If
            '--------------- CREATE SINGLE FOR MARKET SURVEY---------------------



            '--------------- CREATE SINGLE FOR MARKET TRENDS---------------------
            'Call crmViewDataLayer.get_valuation_details(note_ID, localDatalayer, Session("CLIENT_AC_ID"), Session("JETNET_AC_ID"), LAST_SAVE_DATE, completed_or_open)
            'ViewToPDF = ViewToPDF & crmViewDataLayer.Build_Compare_Graphs(client_ac.Text, note_ID, False, internal, retail, localDatalayer, ANALYTICS_HISTORY, Server.MapPath("TempFiles"), completed_or_open, searchCriteria, Me.Page, Me.bottom_tab_update_panel, False, True, False, False, False, True)


            'ViewToPDF = ViewToPDF & "</body></html>"
            '' call the Output String to HTML file

            'document_name = note_ID & "_COMPLETED_MARKET_SURVEY_PDF.html"
            'If crmViewDataLayer.write_report_string_to_file(ViewToPDF, document_name) Then
            '    Call crmViewDataLayer.convert_to_pdf(document_name)
            'End If
            '--------------- CREATE SINGLE FOR MARKET TRENDS---------------------


            clsGeneral.clsGeneral.SaveMarketComparables(aclsLocal_Notes, aclsData_Temp, SavedAFTT)


          End If

          aclsData_Temp.update_localNote(aclsLocal_Notes)

          If UCase(type) = "VALUE_ANALYSIS" Then
            aclsData_Temp.Update_Client_Value_Comparable(note_ID, aclsLocal_Notes.lnote_estval_type, aclsLocal_Notes.lnote_client_ac_id, 0, "P", aclsLocal_Notes.lnote_jetnet_ac_id, Trim(Request("clival_id")), aclsLocal_Notes)

            If authorize_check = True Then
              Dim aclsUpdate_Client_Transactions As New clsClient_Transactions
              aclsUpdate_Client_Transactions.clitrans_airframe_total_hours = aclsLocal_Notes.lnote_estval_aftt
              aclsUpdate_Client_Transactions.clitrans_airframe_total_landings = aclsLocal_Notes.lnote_estval_total_landings
              aclsUpdate_Client_Transactions.clitrans_asking_price = aclsLocal_Notes.lnote_estval_asking_price
              aclsUpdate_Client_Transactions.clitrans_est_price = aclsLocal_Notes.lnote_estval_take_price
              aclsUpdate_Client_Transactions.clitrans_sold_price = aclsLocal_Notes.lnote_estval_estimated_value
              aclsUpdate_Client_Transactions.clitrans_type = aclsLocal_Notes.lnote_estval_type
              aclsUpdate_Client_Transactions.clitrans_action_date = Date.Now()
              aclsUpdate_Client_Transactions.clitrans_date = aclsLocal_Notes.lnote_entry_date
              aclsUpdate_Client_Transactions.clitrans_cliamod_id = aclsLocal_Notes.lnote_jetnet_amod_id
              aclsUpdate_Client_Transactions.clitrans_jetnet_ac_id = aclsLocal_Notes.lnote_jetnet_ac_id
              aclsUpdate_Client_Transactions.clitrans_value_description = aclsLocal_Notes.lnote_note


              'acval_id = aclsData_Temp.Find_Aircraft_Value_ID(aclsUpdate_Client_Transactions, 0)

              Dim acval_id_label As New Label
              If Not IsNothing(name.FindControl("acval_id")) Then
                acval_id_label = name.FindControl("acval_id")
              End If

              If Trim(acval_id_label.Text) <> "" And Trim(acval_id_label.Text) <> "0" Then
                Call aclsData_Temp.Update_Aircraft_Value(aclsUpdate_Client_Transactions, 0, acval_id_label.Text)
              Else
                Call aclsData_Temp.Insert_Into_Aircraft_Value(aclsUpdate_Client_Transactions, 0, False, "est_value")
              End If

            End If
          End If

          If aclsLocal_Notes.lnote_status = "V" And aclsLocal_Notes.lnote_opportunity_status = "C" Then
            Response.Redirect("print_spec.aspx?ac_id=" & aclsLocal_Notes.lnote_client_ac_id & "&noteid=" & aclsLocal_Notes.lnote_id & "&sales_within_years=0&timeframe=18&sales_within_Aftt=0&use_only_used=Y&use_jetnet_data=Y&current_aftt=" & SavedAFTT & "&internal_flag=N&retail_flag=Y&fromClose=true", False)
            Context.ApplicationInstance.CompleteRequest()

          End If

        End If

        ''''''''''''''''''''''''''''''''''''''''''''
        ''''''''''''''''''''''''''''''''''''''''''''
        '''''''''fill in notes follow up''''''''''''
        '''''''''action(item) ''''''''''''''''''''''
        '''''''''5-15-2012''''''''''''''''''''''''''
        ''''''''''''''''''''''''''''''''''''''''''''
        ''''''''''''''''''''''''''''''''''''''''''''
        If Not IsNothing(follow_up) Then
          'this means that the follow up control does exist.
          If follow_up.Checked = True Then
            'this means that a follow up action item does exist.
            If Not IsNothing(action_item_subject) Then
              'fill in message
              aclsLocalNote_FollowUp.lnote_note = action_item_subject.Text
            End If
            'fill in time
            If Not IsNothing(action_item_date) And Not IsNothing(action_item_time) Then
              startdate = DateAdd("h", offset, CDate(action_item_date.Text & " " & action_item_time.SelectedValue))
              enddate = DateAdd(DateInterval.Minute, 30, CDate(startdate))
              enddated = Year(enddate) & "-" & Month(enddate) & "-" & (Day(enddate)) & " " & FormatDateTime(enddate, 4) & ":" & Second(enddate)
              startdate = Year(startdate) & "-" & Month(startdate) & "-" & (Day(startdate)) & " " & FormatDateTime(startdate, 4) & ":" & Second(startdate)
              aclsLocalNote_FollowUp.lnote_schedule_start_date = startdate
              aclsLocalNote_FollowUp.lnote_schedule_end_date = enddated
            End If

            'fill in status
            aclsLocalNote_FollowUp.lnote_status = "P"
            'category
            aclsLocalNote_FollowUp.lnote_notecat_key = aclsLocal_Notes.lnote_notecat_key
            'all company, model, ac, contact stuff
            aclsLocalNote_FollowUp.lnote_jetnet_ac_id = aclsLocal_Notes.lnote_jetnet_ac_id
            aclsLocalNote_FollowUp.lnote_client_ac_id = aclsLocal_Notes.lnote_client_ac_id

            aclsLocalNote_FollowUp.lnote_jetnet_comp_id = aclsLocal_Notes.lnote_jetnet_comp_id
            aclsLocalNote_FollowUp.lnote_client_comp_id = aclsLocal_Notes.lnote_client_comp_id

            aclsLocalNote_FollowUp.lnote_jetnet_contact_id = aclsLocal_Notes.lnote_jetnet_contact_id
            aclsLocalNote_FollowUp.lnote_client_contact_id = aclsLocal_Notes.lnote_client_contact_id

            aclsLocalNote_FollowUp.lnote_jetnet_amod_id = aclsLocal_Notes.lnote_jetnet_amod_id
            aclsLocalNote_FollowUp.lnote_client_amod_id = aclsLocal_Notes.lnote_client_amod_id

            'will never be an update, so id is 0
            aclsLocalNote_FollowUp.lnote_id = 0
            'priority ID
            aclsLocalNote_FollowUp.lnote_clipri_ID = aclsLocal_Notes.lnote_clipri_ID
            aclsLocalNote_FollowUp.lnote_user_id = aclsLocal_Notes.lnote_user_id
            aclsLocalNote_FollowUp.lnote_user_login = aclsLocal_Notes.lnote_user_login
            aclsLocalNote_FollowUp.lnote_user_name = aclsLocal_Notes.lnote_user_name
            aclsLocalNote_FollowUp.lnote_entry_date = Now()
            note_ID = aclsData_Temp.Insert_Note(aclsLocalNote_FollowUp)
          End If
        End If
        ''''''''''''''''''''''''''''''''''''''''''''
        ''''''''''''''''''''''''''''''''''''''''''''
        '''''''''end''''''''''''''''''''''''''''''''
        '''''''''notes follow up action item''''''''
        '''''''''5-15-2012''''''''''''''''''''''''''
        ''''''''''''''''''''''''''''''''''''''''''''
        ''''''''''''''''''''''''''''''''''''''''''''


        If Trim(Request("refreshing")) <> "prospect" And Trim(Request("refreshing")) <> "view" Then
          If Session.Item("Listing") <> 9 Then
            If Trim(Request("notesViewAll")) = "show" Then
              System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.$('#ctl00_ContentPlaceHolder1_searchButton').trigger('click');", True)
            Else
              If Trim(Request("from")) = "companyDetails" Or Trim(Request("from")) = "contactDetails" Then

                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location.href=window.opener.location.href;", True)

              Else
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "javascript: var URLlink = ''; if (window.opener.location.pathname.toUpperCase().search('LISTING') == 1){URLlink = '?redo_search=true'}; if (window.opener.location.pathname.toUpperCase().search('DISPLAYAIRCRAFTDETAIL') == 1){URLlink = '?acid=" & Trim(Request("ac_ID")) & "&jid=0&source=" & Trim(Request("source")) & "'};if (window.opener.location.pathname.toUpperCase().search('DISPLAYCOMPANYDETAIL') == 1){URLlink = '?compid=" & Trim(Request("comp_ID")) & "&jid=0&source=" & Trim(Request("source")) & "'};window.opener.location.href=window.opener.location.pathname + URLlink;", True)

              End If
            End If
          ElseIf Session.Item("Listing") = 9 Then
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location.href='home.aspx';", True)
          End If

          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)

        Else
          If IsNumeric(Trim(Request("rememberTab"))) Then
            Session.Item("selectedViewTab") = Trim(Request("rememberTab"))
          End If
          If aclsLocal_Notes.lnote_status = "V" Then
            If Trim(Request("nWin")) <> "1" Then
              System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.location.href='view_template.aspx?ViewID=19&noteID=" & note_ID & "&noMaster=false" & IIf(Trim(Request("amod_ID")) <> "", "&amod_ID=" & Trim(Request("amod_ID")), "") & "';", True)
            Else
              System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location.href='view_template.aspx?ViewID=19&noteID=" & note_ID & "&noMaster=false';", True)
              System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
            End If
          Else
            If Trim(Request("ViewID")) = "18" Then
              If Trim(Request("ac_ID")) = "" Or Trim(Request("ac_ID")) = "0" Then
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.$('#ctl00_ContentPlaceHolder1_View_Master1_crmProspectSearchButton').trigger('click');", True)
              Else
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location.href='view_template.aspx?ViewID=18&ac_id=" & Trim(Request("ac_ID")) & "&noMaster=false';", True) ''
              End If
            Else
              System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location.href='view_template.aspx?" & IIf(Trim(Request("NoteID")) <> "", "noteID=" & Trim(Request("NoteID")) & "&", "") & "ViewID=" & Trim(Request("ViewID")) & IIf(Trim(Request("viewModelID")) <> "", "&amod_id=" & Trim(Request("viewModelID")), "") & "&noMaster=false" & "';", True) ' IIf(IsNumeric(Trim(Request("rememberTab"))), "&activetab=" & Trim(Request("rememberTab")), "") & "';", True)
            End If

            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
          End If
        End If

      End If
    Catch ex As Exception
      error_string = "edit_note.aspx.vb - add_note() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  ''' <summary>
  '''Builds the Action Item Email that gets sent if requested upon creation.
  ''' </summary>
  ''' <param name="pertaining_to">Person the item is for</param>
  ''' <param name="jetnet_ac">Jetnet AC ID</param>
  ''' <param name="client_ac">Client AC ID</param>
  ''' <param name="jetnet_comp">Jetnet Company ID</param>
  ''' <param name="client_comp">Client Company ID</param>
  ''' <param name="jetnet_contact">Jetnet Contact ID</param>
  ''' <param name="client_contact">Client Contact ID</param>
  ''' <param name="notes_edit">Notes Field</param>
  ''' <param name="startdate">Start Date of Item</param>
  ''' <param name="enddated">End Date of Item</param>
  ''' <remarks></remarks>
  Sub Build_Actions_Email(ByVal pertaining_to As DropDownList, ByVal jetnet_ac As TextBox, ByVal client_ac As TextBox, ByVal jetnet_comp As TextBox, ByVal client_comp As TextBox, ByVal jetnet_contact As TextBox, ByVal client_contact As TextBox, ByVal notes_edit As TextBox, ByVal startdate As String, ByVal enddated As String, ByVal cc As TextBox)
    Dim Recepient As String = "Amanda@mvintech.com"
    Dim cc_string As String = cc.Text
    aTempTable = aclsData_Temp.Get_Client_User(pertaining_to.SelectedValue)
    If Not IsNothing(aTempTable) Then
      If aTempTable.Rows.Count > 0 Then
        For Each r As DataRow In aTempTable.Rows
          Recepient = r("cliuser_email_address")
        Next
      End If
    Else
      If aclsData_Temp.class_error <> "" Then
        error_string = aclsData_Temp.class_error
        clsGeneral.clsGeneral.LogError("edit_note.aspx.vb - add_note_click() - " & error_string, aclsData_Temp)
      End If
      display_error()
    End If

    Dim From As String = "Mandy@mvintech.com"
    aTempTable = aclsData_Temp.Get_Client_User(Session.Item("localUser").crmLocalUserID)
    If Not IsNothing(aTempTable) Then
      If aTempTable.Rows.Count > 0 Then
        For Each r As DataRow In aTempTable.Rows
          From = r("cliuser_email_address")
        Next
      End If
    Else
      If aclsData_Temp.class_error <> "" Then
        error_string = aclsData_Temp.class_error
        clsGeneral.clsGeneral.LogError("edit_note.aspx.vb - add_note_click() - " & error_string, aclsData_Temp)
      End If
    End If

    Dim Bcc As String = ""
    'Dim Cc As String = ""

    Dim comp_sub As String = ""
    Dim ac_sub As String = ""
    If client_comp.Text <> "0" Or jetnet_comp.Text <> "0" Then
      comp_sub = "(" & Replace(what_comp(jetnet_comp.Text, client_comp.Text, 1), "<b>", "") & ")"
      comp_sub = Replace(comp_sub, "</b>", "")
      comp_sub = Replace(comp_sub, "<br />", "")
    End If

    If client_ac.Text <> "0" Or jetnet_ac.Text <> "0" Then
      ac_sub = "(" & Replace(what_ac(jetnet_ac.Text, client_ac.Text, 2), "<br />", " ") & ")"
      ac_sub = Replace(ac_sub, "<b>", " ")
      ac_sub = Replace(ac_sub, "</b>", " ")
    End If


    Dim Subject As String = "Action Item " & comp_sub & " " & ac_sub
    Dim Body As String = ""
    If startdate <> "" And enddated <> "" Then
      Dim start_time As String = ""
      Dim end_time As String = ""
      start_time = Format(TimeValue(startdate), "hh:mm tt")

      Dim week As Integer = Weekday(startdate)
      Dim monthint As Integer = Month(startdate)
      Dim monthdis As String = MonthName(monthint)
      Dim weekdis As String = WeekdayName(week)
      Dim yeardis As Integer = Year(startdate)
      Dim daydis As Integer = Day(startdate)
      start_time = weekdis & ", " & monthdis & " " & daydis & ", " & yeardis & " at " & start_time
      Body = "<html>"
      Body = Body & "<head></head>"
      Body = Body & "<body>"
      Body = Body & "<table width='700' cellpadding='3' cellspacing='0'>"
      Body = Body & "<tr><td align='left' valign='top'>Action Date: " & start_time & "</td></tr>"
      Body = Body & "<tr><td align='left' valign='top'>"
      If client_comp.Text <> "0" Or jetnet_comp.Text <> "0" Then
        Body = Body & "<b>Company Information:</b><br />"
        Body = Body & Replace(what_comp(jetnet_comp.Text, client_comp.Text, 1), "<b>", "")
        Body = Body & what_comp(jetnet_comp.Text, client_comp.Text, 2)
      End If
      Body = Body & "</td>"
      Body = Body & "<td align='left' valign='top'>"
      If client_contact.Text <> "0" Or jetnet_contact.Text <> "0" Then
        Body = Body & "<b>Contact Information:</b><br />"
        Body = Body & what_contact(jetnet_contact.Text, client_contact.Text)
      End If
      Body = Body & "</td></tr>"
      If client_ac.Text <> "0" Or jetnet_ac.Text <> "0" Then
        Body = Body & "<tr><td align='left' valign='top'>"
        Body = Body & "<b>Aircraft Information:</b><br />"
        Body = Body & what_ac(jetnet_ac.Text, client_ac.Text, 2)
        Body = Body & what_ac(jetnet_ac.Text, client_ac.Text, 1)
        Body = Body & "</td></tr>"
      End If
      Body = Body & "<tr><td align='left' valign='top'>"
      Body = Body & "<b>Action Description:</b><br />" & notes_edit.Text & ".</td></tr>"
      Body = Body & "<tr><td align='left' valign='top'><b>Task Entered By:</b> " & Left(Session.Item("localUser").crmLocalUserFirstName & " " & Session.Item("localUser").crmLocalUserLastName, 15) & ".</td></tr>"
      Body = Body & "</table>"
      Body = Body & "</body>"
      Body = Body & "</html>"
    End If
    Send_Email("customerservice@jetnet.com", Recepient, Bcc, cc_string, Subject, Body, aclsData_Temp)

  End Sub
  ''' <summary>
  ''' One function that fills all of the dropdowns in every user control: Email1, Notes1, ActionItems1, Documents1 -
  ''' depending on what is being used at the time.
  ''' </summary>
  ''' <param name="jetnet_ac">Jetnet AC</param>
  ''' <param name="client_ac">Client AC</param>
  ''' <param name="jetnet_comp">Jetnet Company</param>
  ''' <param name="client_comp">Client Company</param>
  ''' <param name="jetnet_contact">Jetnet Contact</param>
  ''' <param name="client_contact">Client Contact</param>
  ''' <param name="jetnet_mod">Jetnet Model</param>
  ''' <param name="client_mod">Client Model</param>
  ''' <param name="name">Control being used</param>
  ''' <remarks></remarks>
  Public Sub Fill_All_DropDowns(ByVal jetnet_ac As Integer, ByVal client_ac As Integer, ByVal jetnet_comp As Integer, ByVal client_comp As Integer, ByVal jetnet_contact As Integer, ByVal client_contact As Integer, ByVal jetnet_mod As Integer, ByVal client_mod As Integer, ByVal name As Control, ByVal FillModel As Boolean)
    Dim old_id As Integer = 0
    Dim comp_name As String = ""
    Dim company_state As String = ""
    Dim company_country As String = ""
    Dim company_title As String = ""
    Dim company_info As Label = name.FindControl("company_info")
    Dim Company_Results As New DataTable
    Dim Preferences_Table As New DataTable
    Dim company_source As String = ""

    Dim title As String = ""
    Dim sirname As String = ""
    Dim first As String = ""
    Dim last As String = ""

    'dropdown lists
    Dim company_name As DropDownList = name.FindControl("company_name")
    Dim notes_cat As DropDownList = name.FindControl("notes_cat")
    Dim notes_opp As DropDownList = name.FindControl("notes_opp")
    Dim contact_name As DropDownList = name.FindControl("contact_name")
    Dim aircraft_name As DropDownList = name.FindControl("aircraft_name")
    Dim pertaining_to As DropDownList = name.FindControl("pertaining_to")
    Dim priority As DropDownList = name.FindControl("priority")
    Dim time As DropDownList = name.FindControl("time")
    Dim action_item_time As DropDownList = name.FindControl("action_item_time")

    Dim aircraft_info As Label = name.FindControl("aircraft_info")
    Dim Aircraft_Data As New clsClient_Aircraft
    Dim Aircraft_Model As String = ""
    Dim ac_year As String = ""
    Dim ac_ser As String = ""
    Dim ac_reg As String = ""
    Dim hold_category As Integer = 23

    Dim jetnet_amod As TextBox = name.FindControl("jetnet_mod")
    Dim client_amod As TextBox = name.FindControl("client_mod")

    If Not IsNothing(name.FindControl("priority")) Then
      'Filling Priority Table
      priority.Items.Add(New ListItem("High", "1"))
      priority.Items.Add(New ListItem("Medium", "2"))
      priority.Items.Add(New ListItem("Low", "3"))
    End If
    'Default Company/Aircraft/Contact
    aircraft_name.Items.Add(New ListItem("None Selected", "0||0"))
    If IsNothing(company_name.Items.FindByValue("|")) Then
      company_name.Items.Add(New ListItem("None Selected", "|"))
    End If
        'contact_name.Items.Add(New ListItem("Please Select a Company", ""))

        ' SKIP IT ALL EXCEPT LEFT SIDE 
        If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Or Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
            ' dont do - no mpm connection
        Else
            If Not IsNothing(name.FindControl("pertaining_to")) Then
            'Filling User Table up

            If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
                Dim ExistsTable As DataTable
                ExistsTable = aclsData_Temp.Get_Client_User_By_Email_Address(HttpContext.Current.Session.Item("localUser").crmLocalUserEmailAddress)
                If Not IsNothing(ExistsTable) Then
                    If ExistsTable.Rows.Count > 0 Then
                        HttpContext.Current.Session.Item("localUser").crmLocalUserID = ExistsTable.Rows(0).Item("cliuser_id")
                    End If
                End If
            End If

            If HttpContext.Current.Session.Item("localUser").crmUserType <> eUserTypes.MyNotesOnly Then
                    aTempTable = aclsData_Temp.Get_AllClientUser_Active("Y")
                    If Not IsNothing(aTempTable) Then
                        If aTempTable.Rows.Count > 0 Then
                            For Each q As DataRow In aTempTable.Rows
                                pertaining_to.Items.Add(New ListItem(q("cliuser_first_name") & " " & q("cliuser_last_name"), q("cliuser_id")))
                            Next
                        End If
                    Else
                        If aclsData_Temp.class_error <> "" Then
                            error_string = aclsData_Temp.class_error
                            clsGeneral.clsGeneral.LogError("edit_note.ascx.vb -Fill_All_DropDowns() - " & error_string, aclsData_Temp)
                        End If
                        display_error()
                    End If

                ElseIf HttpContext.Current.Session.Item("localUser").crmUserType = eUserTypes.MyNotesOnly Then
                    pertaining_to.Items.Add(New ListItem(Session.Item("localUser").crmLocalUserFirstName & " " & Session.Item("localUser").crmLocalUserLastName, HttpContext.Current.Session.Item("localUser").crmLocalUserID))
                End If
            End If




        If Not IsNothing(name.FindControl("notes_opp")) Then
                Dim TemporaryDropdownList As DropDownList = name.FindControl("notes_opp")
                TemporaryDropdownList.Items.Clear()
                clsGeneral.clsGeneral.Fill_Opportunity_Category(name.FindControl("notes_opp"), aTempTable, aclsData_Temp)
            End If
        If Not IsNothing(name.FindControl("time")) Then
            'Filling Time Table up
            aTempTable = aclsData_Temp.Get_Time_Table 'Time table fill up
            If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then
                    For Each q As DataRow In aTempTable.Rows
                        time.Items.Add(New ListItem(q("clitim_time"), FormatDateTime(q("clitim_time"), 4)))
                    Next
                End If
            Else
                If aclsData_Temp.class_error <> "" Then
                    error_string = aclsData_Temp.class_error
                    clsGeneral.clsGeneral.LogError("edit_note.ascx.vb -Fill_All_DropDowns() - " & error_string, aclsData_Temp)
                End If
            End If
        End If

        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'follow up action item on notes control
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If Not IsNothing(name.FindControl("action_item_time")) Then
      'Filling Time Table up
      aTempTable = aclsData_Temp.Get_Time_Table 'Time table fill up
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          For Each q As DataRow In aTempTable.Rows
            action_item_time.Items.Add(New ListItem(q("clitim_time"), FormatDateTime(q("clitim_time"), 4)))
          Next
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          clsGeneral.clsGeneral.LogError("edit_note.ascx.vb -Fill_All_DropDowns() - " & error_string, aclsData_Temp)
        End If
      End If
    End If
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'follow up action item on notes control
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        If Not IsNothing(name.FindControl("notes_cat")) Then
                'Filling Note Category Up. 
                If name.ID = "Documents1" Then
                    aTempTable = aclsData_Temp.Get_Client_Note_Document_Category("Y")
                ElseIf name.ID = "Notes1" Then
                    aTempTable = aclsData_Temp.Get_Client_NOT_Note_Document_Category("Y", "notecat_order")
                Else
                    aTempTable = aclsData_Temp.Get_Client_NOT_Note_Document_Category("Y")
                End If

                If Not IsNothing(aTempTable) Then
                    If aTempTable.Rows.Count > 0 Then
                        For Each z As DataRow In aTempTable.Rows
                            If InStr(UCase(z("notecat_name")), "GENERAL") > 0 Then
                                hold_category = z("notecat_key")
                            End If
                            notes_cat.Items.Add(New ListItem(z("notecat_name"), z("notecat_key")))

                        Next
                    End If
                Else
                    If aclsData_Temp.class_error <> "" Then
                        error_string = aclsData_Temp.class_error
                        clsGeneral.clsGeneral.LogError("edit_note.ascx.vb -Fill_All_DropDowns() - " & error_string, aclsData_Temp)
                    End If
                End If


                notes_cat.SelectedValue = hold_category
            End If


        End If


        just_the_left_hand_dropdowns(jetnet_ac, client_ac, jetnet_comp, client_comp, jetnet_contact, client_contact, jetnet_mod, client_mod, name, FillModel)
  End Sub
  Public Function just_the_left_hand_dropdowns(ByVal jetnet_ac As Integer, ByVal client_ac As Integer, ByVal jetnet_comp As Integer, ByVal client_comp As Integer, ByVal jetnet_contact As Integer, ByVal client_contact As Integer, ByVal jetnet_mod As Integer, ByVal client_mod As Integer, ByVal name As Control, ByVal FillModel As Boolean)
    just_the_left_hand_dropdowns = ""
    Dim old_id As Integer = 0
    Dim comp_name As String = ""
    Dim company_state As String = ""
    Dim company_country As String = ""
    Dim company_title As String = ""
    Dim company_info As Label = name.FindControl("company_info")
    Dim Company_Results As New DataTable
    Dim Preferences_Table As New DataTable
    Dim company_source As String = ""

    Dim title As String = ""
    Dim sirname As String = ""
    Dim first As String = ""
    Dim last As String = ""

    'dropdown lists
    Dim company_name As DropDownList = name.FindControl("company_name")
    Dim notes_cat As DropDownList = name.FindControl("notes_cat")
    Dim notes_opp As DropDownList = name.FindControl("notes_opp")
    Dim contact_name As DropDownList = name.FindControl("contact_name")
    Dim aircraft_name As DropDownList = name.FindControl("aircraft_name")

    'For sale blocks.
    Dim ac_sale As New RadioButtonList
    Dim ac_status_not_for_sale As New DropDownList
    Dim ac_status_for_sale As New DropDownList
    Dim CompareValidator1 As New CompareValidator
    Dim date_listed_panel As New Panel
    Dim date_listed As New TextBox
    Dim DOMlisted As New Label
    Dim DOMWord As New Label
    Dim est_label As New Label
    Dim cliaircraft_value_description_text As New TextBox
    Dim est_price As New TextBox
    Dim broker_price As New TextBox
    Dim broker_lbl As New Label
    Dim asking_price As New TextBox
    Dim asking_wordage As New DropDownList
    Dim ask_lbl As New Label


    SetReferenceSaleControls(name, ac_sale, ac_status_not_for_sale, ac_status_for_sale, CompareValidator1, date_listed_panel, date_listed, DOMlisted, DOMWord, est_label, cliaircraft_value_description_text, est_price, broker_price, broker_lbl, asking_price, asking_wordage, ask_lbl)


    Dim pertaining_to As DropDownList = name.FindControl("pertaining_to")
    Dim priority As DropDownList = name.FindControl("priority")
    Dim time As DropDownList = name.FindControl("time")

    Dim aircraft_info As Label = name.FindControl("aircraft_info")
    Dim Aircraft_Data As New clsClient_Aircraft
    Dim Aircraft_Model As String = ""
    Dim ac_year As String = ""
    Dim ac_ser As String = ""
    Dim ac_reg As String = ""
    Dim hold_category As Integer = 23

    Dim jetnet_amod As TextBox = name.FindControl("jetnet_mod")
    Dim client_amod As TextBox = name.FindControl("client_mod")
    Dim NotesModelName As New DropDownList
    If Not IsNothing(name.FindControl("model_name")) Then
      NotesModelName = name.FindControl("model_name")
    End If


    'First we're going to fill the aircraft_info.text 
    'Is there any AC ID?
    If jetnet_ac <> 0 Or client_ac <> 0 Then
      'Is there a jetnet AC Id?
      If client_ac <> 0 Then
        aTempTable = aclsData_Temp.Get_Clients_Aircraft(client_ac)
        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            Aircraft_Model = (aTempTable.Rows(0).Item("cliamod_make_name") & " " & aTempTable.Rows(0).Item("cliamod_model_name"))
            Aircraft_Data = clsGeneral.clsGeneral.Create_Aircraft_Class(aTempTable, "cliaircraft")
            Aircraft_Data.cliaircraft_id = client_ac
            Aircraft_Data.cliaircraft_value_description = IIf(Not IsDBNull(aTempTable.Rows(0).Item("cliaircraft_value_description")), aTempTable.Rows(0).Item("cliaircraft_value_description"), "")
            aircraft_name.Items.Add(New ListItem(Aircraft_Model & " Ser#" & Aircraft_Data.cliaircraft_ser_nbr & " Reg#" & Aircraft_Data.cliaircraft_reg_nbr, client_ac & "|CLIENT|" & aTempTable.Rows(0).Item("cliaircraft_cliamod_id")))
            aircraft_name.SelectedValue = client_ac & "|CLIENT|" & aTempTable.Rows(0).Item("cliaircraft_cliamod_id")
            client_amod.Text = aTempTable.Rows(0).Item("cliaircraft_cliamod_id")
            aTempTable2 = aclsData_Temp.Get_Clients_Aircraft_Model_amodID(aTempTable.Rows(0).Item("cliaircraft_cliamod_id"))
            If Not IsNothing(aTempTable2) Then
              If aTempTable2.Rows.Count > 0 Then
                jetnet_amod.Text = aTempTable2.Rows(0).Item("cliamod_jetnet_amod_id")
              End If
            End If
          End If
        End If
      ElseIf jetnet_ac <> 0 Then
        aTempTable = aclsData_Temp.GetJETNET_Aircraft_Details_AC_ID(jetnet_ac, "")
        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            Aircraft_Model = (aTempTable.Rows(0).Item("amod_make_name") & " " & aTempTable.Rows(0).Item("amod_model_name"))
            Aircraft_Data = clsGeneral.clsGeneral.Create_Aircraft_Class(aTempTable, "ac")
            Aircraft_Data.cliaircraft_id = jetnet_ac
            aircraft_name.Items.Add(New ListItem(Aircraft_Model & " Ser#" & Aircraft_Data.cliaircraft_ser_nbr & " Reg#" & Aircraft_Data.cliaircraft_reg_nbr, jetnet_ac & "|JETNET|" & aTempTable.Rows(0).Item("ac_amod_id")))
            aircraft_name.SelectedValue = jetnet_ac & "|JETNET|" & aTempTable.Rows(0).Item("ac_amod_id")
            jetnet_amod.Text = aTempTable.Rows(0).Item("ac_amod_id")
            aTempTable2 = aclsData_Temp.Get_Clients_Aircraft_Model_ByJETNETAmod(aTempTable.Rows(0).Item("ac_amod_id"))
            If Not IsNothing(aTempTable2) Then
              If aTempTable2.Rows.Count > 0 Then
                client_amod.Text = aTempTable2.Rows(0).Item("cliamod_id")
              End If
            End If

          End If
        End If
      End If
      Dim id_array As String = ""
      aircraft_info.Text = Aircraft_Model & "<br />" & clsGeneral.clsGeneral.Build_Aircraft_Display(Aircraft_Data, True, False, True)

      'We now need to fill out the for sale block.
      If name.ID = "Notes1" Then
        FillUpForSaleControls(Aircraft_Data, name, ac_sale, ac_status_not_for_sale, ac_status_for_sale, CompareValidator1, date_listed_panel, date_listed, DOMlisted, DOMWord, est_label, cliaircraft_value_description_text, est_price, broker_price, broker_lbl, asking_price, asking_wordage, ask_lbl)
      End If


      'Next we need to determine something. Is there a company ID? 
      'If there is not, then we really need to fill the company dropdown with the companies associated with the aircraft. 
      If jetnet_comp = 0 And client_comp = 0 Then
        'Fill with associated companies with aircraft
        'If jetnet_ac <> 0 Then
        'aTempTable = aclsData_Temp.GetAircraft_Listing_wContacts_WITHOUTID(jetnet_ac, "JETNET", "23414")
        'aTempTable = aclsData_Temp.Get_Aircraft_Reference_Client_JetnetacID_Full_Details(jetnet_ac)

        'Else
        If client_ac <> 0 Then
          aTempTable = aclsData_Temp.GetAircraft_Listing_wContacts(client_ac, "CLIENT")
        Else
          aTempTable = aclsData_Temp.GetAircraft_Listing_wContacts(jetnet_ac, "JETNET")
        End If

        'End If
        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            For Each r As DataRow In aTempTable.Rows

              If old_id = IIf(Not IsDBNull(r("comp_id")), r("comp_id"), 0) Then
              Else

                comp_name = IIf(Not IsDBNull(r("comp_name")), r("comp_name"), "")

                If Not IsDBNull(r("comp_jetnet_id")) Then
                  If r("comp_jetnet_id") <> 0 Then
                    id_array = id_array & r("comp_jetnet_id") & ","
                  End If
                End If

                company_country = IIf(Not IsDBNull(r("comp_country")), r("comp_country"), "")
                                company_state = IIf(Not IsDBNull(r("comp_state")), r("comp_state"), "")


                                If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Or Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
                                    company_title = comp_name & " (" & company_state & " " & company_country & ")"
                                Else
                                    company_title = comp_name & " (" & company_state & " " & company_country & " - " & r("source") & ")"
                                End If

                                If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Or Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
                                    If IsNothing(company_name.Items.FindByValue(r("comp_id"))) Then
                                        company_name.Items.Add(New ListItem(CStr(company_title), r("comp_id")))
                                    End If
                                Else
                                        If IsNothing(company_name.Items.FindByValue(r("comp_id") & "|" & r("source"))) Then
                                        company_name.Items.Add(New ListItem(CStr(company_title), r("comp_id") & "|" & r("source")))
                                    End If
                                    'If Not IsNothing(name.FindControl("email_to")) Then
                                    '    Dim email_to As TextBox = name.FindControl("email_to")
                                    '    email_to.Text = r("comp_email_address")
                                    'End If

                                    old_id = r("comp_id")
                                End If
                            End If




            Next
            id_array = id_array.TrimEnd(",")
          End If


        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            clsGeneral.clsGeneral.LogError("edit_note.aspx.vb - GetAircraft_Listing_wContacts() - " & error_string, aclsData_Temp)
          End If
        End If
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ''''''''''''''''''''''''''''''''''''check two just for jetnet AC.
        If jetnet_ac <> 0 Then
          'run the jetnet companies now.
          aTempTable = aclsData_Temp.GetAircraft_Listing_wContacts_WITHOUTID(jetnet_ac, "JETNET", id_array)
          If Not IsNothing(aTempTable) Then
            If aTempTable.Rows.Count > 0 Then
              For Each r As DataRow In aTempTable.Rows

                If old_id = IIf(Not IsDBNull(r("comp_id")), r("comp_id"), 0) Then
                Else

                  comp_name = IIf(Not IsDBNull(r("comp_name")), r("comp_name"), "")

                  company_country = IIf(Not IsDBNull(r("comp_country")), r("comp_country"), "")
                  company_state = IIf(Not IsDBNull(r("comp_state")), r("comp_state"), "")
                  company_title = comp_name & " (" & company_state & " " & company_country & " - " & r("source") & ")"

                  If IsNothing(company_name.Items.FindByValue(r("comp_id") & "|" & r("source"))) Then
                    company_name.Items.Add(New ListItem(CStr(company_title), r("comp_id") & "|" & r("source")))
                  End If
                  'If Not IsNothing(name.FindControl("email_to")) Then
                  '    Dim email_to As TextBox = name.FindControl("email_to")
                  '    email_to.Text = r("comp_email_address")
                  'End If

                  old_id = r("comp_id")
                End If

              Next
            End If

          End If
        End If
      End If
    End If
    'Alright!!
    'Next we need to figure out the company. This is the second most important. 
    If jetnet_comp <> 0 Or client_comp <> 0 Then
      Preferences_Table = Nothing

      If client_comp <> 0 Then
        Company_Results = aclsData_Temp.GetCompanyInfo_ID(client_comp, "CLIENT", 0)
        company_source = "CLIENT"
      ElseIf jetnet_comp <> 0 Then
        Company_Results = aclsData_Temp.GetCompanyInfo_ID(jetnet_comp, "JETNET", 0)
        company_source = "JETNET"
      End If

      ' check the state of the DataTable
      If Not IsNothing(Company_Results) Then
        If Company_Results.Rows.Count > 0 Then
          'Sets the variables for the company display
          Dim Company_Data As New clsClient_Company
          Company_Data = clsGeneral.clsGeneral.Create_Company_Class(Company_Results, company_source, Preferences_Table)
          'Builds the company Display
          company_info.Text = Company_Data.clicomp_name & " <br />" & clsGeneral.clsGeneral.Show_Company_Display(Company_Data, False)

                    If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Or Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
                        If IsNothing(company_name.Items.FindByValue(Company_Results.Rows(0).Item("comp_id"))) Then
                            company_name.Items.Add(New ListItem(Company_Data.clicomp_name & " (" & Company_Data.clicomp_state & " " & Company_Data.clicomp_country & ")", Company_Results.Rows(0).Item("comp_id")))
                        End If
                        company_name.SelectedValue = Company_Results.Rows(0).Item("comp_id")
                    Else
                        If IsNothing(company_name.Items.FindByValue(Company_Results.Rows(0).Item("comp_id") & "|" & company_source)) Then
                            company_name.Items.Add(New ListItem(Company_Data.clicomp_name & " (" & Company_Data.clicomp_state & " " & Company_Data.clicomp_country & " - " & company_source & ")", Company_Results.Rows(0).Item("comp_id") & "|" & company_source))
                        End If
                        company_name.SelectedValue = Company_Results.Rows(0).Item("comp_id") & "|" & company_source
                    End If


                    If Not IsNothing(name.FindControl("email_to")) Then
                            Dim email_to As Label = name.FindControl("email_to")
                            email_to.Text = Company_Results.Rows(0).Item("comp_email_address")
                        End If
                    End If
                Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          clsGeneral.clsGeneral.LogError("edit_note.ascx.vb - Fill_All_Dropdowns() - " & error_string, aclsData_Temp)
        End If
      End If

      'Aircraft Associated with Company! 
      'But only if no jetnet_ac/client_ac

      If jetnet_ac = 0 And client_ac = 0 Then
        If company_source = "CLIENT" Then
          aTempTable = aclsData_Temp.Get_Client_JETNET_AC(client_comp, "ac_id ASC", Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmCommercial_Flag, Session.Item("localSubscription").crmJets_Flag, Session.Item("localSubscription").crmExecutive_Flag, Session.Item("localSubscription").crmTurboprops)
        Else
          aTempTable = aclsData_Temp.GetAircraft_Listing_compid(jetnet_comp, Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmCommercial_Flag, Session.Item("localSubscription").crmJets_Flag, Session.Item("localSubscription").crmExecutive_Flag, Session.Item("localSubscription").crmTurboprops, 0, Session.Item("localSubscription").crmAerodexFlag)
        End If

        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            aircraft_name.Items.Clear()
            For Each r As DataRow In aTempTable.Rows
              ac_year = IIf(Not IsDBNull(r("ac_year_mfr")), r("ac_year_mfr"), "")
              ac_ser = IIf(Not IsDBNull(r("ac_ser_nbr")), r("ac_ser_nbr"), "")
              ac_reg = IIf(Not IsDBNull(r("ac_reg_nbr")), r("ac_reg_nbr"), "")
              aircraft_name.Items.Add(New ListItem(CStr(ac_year & " " & r("amod_make_name") & " " & r("amod_model_name") & " Ser #:" & ac_ser & " Reg#:" & ac_reg), r("ac_id") & "|" & r("source") & "|" & r("ac_amod_id")))
            Next
            If jetnet_ac = 0 And client_ac = 0 Then
              aircraft_name.Items.Add(New ListItem("Please Select an Aircraft", "0||0"))
              aircraft_name.SelectedValue = "0||0"
            End If
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            clsGeneral.clsGeneral.LogError("edit_note.aspx.vb - fill_all_dropdowns() - " & error_string, aclsData_Temp)
          End If
        End If
      End If
      'Contacts associated with company
      If company_source = "CLIENT" Then
        aTempTable = aclsData_Temp.GetContacts(client_comp, company_source, "Y", 0)
      Else
        aTempTable = aclsData_Temp.GetContacts(jetnet_comp, company_source, "Y", 0)
      End If
      ' check the state of the DataTable
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          contact_name.Items.Clear()
          For Each r As DataRow In aTempTable.Rows
            title = IIf(Not IsDBNull(r("contact_title")), r("contact_title"), "")
            sirname = IIf(Not IsDBNull(r("contact_sirname")), r("contact_sirname"), "")
            first = IIf(Not IsDBNull(r("contact_first_name")), r("contact_first_name"), "")
            last = IIf(Not IsDBNull(r("contact_last_name")), r("contact_last_name"), "")
                        If jetnet_contact = r("contact_id") Or client_contact = r("contact_id") Then
                            If Not IsNothing(name.FindControl("email_to")) Then
                                Dim email_to As Label = name.FindControl("email_to")
                                email_to.Text = r("contact_email_address")
                            End If
                        End If

                        If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Or Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
                            If title <> "" Then
                                contact_name.Items.Add(New ListItem(CStr(title & " - " & sirname & " " & first & " " & last), r("contact_id") & "|" & UCase(r("contact_type"))))
                            Else
                                contact_name.Items.Add(New ListItem(CStr(sirname & " " & first & " " & last), r("contact_id") & "|" & UCase(r("contact_type"))))
                            End If
                        Else
                            If title <> "" Then
                                contact_name.Items.Add(New ListItem(CStr(title & " - " & sirname & " " & first & " " & last & " (" & company_source & " record) "), r("contact_id") & "|" & UCase(r("contact_type"))))
                            Else
                                contact_name.Items.Add(New ListItem(CStr(sirname & " " & first & " " & last & " (" & company_source & " record)"), r("contact_id") & "|" & UCase(r("contact_type"))))
                            End If
                        End If


                    Next

          contact_name.Items.Add(New ListItem("Please Select a Contact", "0|"))
          If jetnet_contact <> 0 Then
            If Not IsNothing(contact_name.Items.FindByValue(jetnet_contact & "|JETNET")) Then
              contact_name.SelectedValue = jetnet_contact & "|JETNET"
            End If
          End If
          If client_contact <> 0 Then
            If Not IsNothing(contact_name.Items.FindByValue(client_contact & "|CLIENT")) Then
              contact_name.SelectedValue = client_contact & "|CLIENT"
            End If
          End If
          If jetnet_contact = 0 And client_contact = 0 Then
            contact_name.SelectedValue = "0|"
          End If


        Else
          contact_name.Items.Clear()
          contact_name.Items.Add(New ListItem("No Associated Contacts", "0|"))
          contact_name.SelectedValue = "0|"
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          clsGeneral.clsGeneral.LogError("edit_note.aspx.vb - fill_drop() - " & error_string, aclsData_Temp)
        End If
      End If
    End If
    If name.ID = "Wanted1" Or FillModel = True Then
      Dim CurrentDropdown As New DropDownList
      Dim hold_selection As String = "0||0"

      If name.ID = "Wanted1" Then
        CurrentDropdown = aircraft_name
      ElseIf FillModel Then
        CurrentDropdown = NotesModelName
      End If

      CurrentDropdown.Items.Clear()
      CurrentDropdown.Items.Add(New ListItem("None Selected", "0||0"))
      aTempTable = aclsData_Temp.Get_Combination_Models(Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmCommercial_Flag, Session.Item("localSubscription").crmJets_Flag, Session.Item("localSubscription").crmExecutive_Flag, Session.Item("localSubscription").crmTurboprops)
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          For Each q As DataRow In aTempTable.Rows
            'if these values aren't zero then there is a default selection
            If jetnet_amod.Text <> "0" Or client_amod.Text <> "0" Then
              If (jetnet_amod.Text = q("amod_id")) And (client_amod.Text = q("client_id")) Then
                hold_selection = q("amod_id") & "|" & q("amod_make_name") & "|" & q("amod_model_name") & "|" & q("source") & "|" & q("client_id")
              End If
            End If
            CurrentDropdown.Items.Add(New ListItem(CStr(q("amod_make_name") & " " & q("amod_model_name")), q("amod_id") & "|" & q("amod_make_name") & "|" & q("amod_model_name") & "|" & q("source") & "|" & q("client_id")))
          Next
          Try
            CurrentDropdown.SelectedValue = hold_selection
          Catch ex As Exception
            If aclsData_Temp.class_error <> "" Then
              error_string = aclsData_Temp.class_error
              clsGeneral.clsGeneral.LogError("edit_note.aspx.vb - select model - " & error_string, aclsData_Temp)
            End If
          End Try
        End If
      End If
    End If
  End Function
  ''' <summary>
  ''' 
  ''' Builds File Name for Document based on things the document is attached to.
  ''' Like company name, AC name, Contact name.
  ''' </summary>
  ''' <param name="jetnet_contact">Jetnet Contact</param>
  ''' <param name="client_contact">Client Contact</param>
  ''' <param name="client_comp">Client Company</param>
  ''' <param name="jetnet_comp">Jetnet Company</param>
  ''' <param name="client_ac">Client AC</param>
  ''' <param name="jetnet_ac">Jetnet AC</param>
  ''' <returns>Filename String</returns>
  ''' <remarks></remarks>
  Public Function Build_File_Name(ByVal jetnet_contact As TextBox, ByVal client_contact As TextBox, ByVal client_comp As TextBox, ByVal jetnet_comp As TextBox, ByVal client_ac As TextBox, ByVal jetnet_ac As TextBox) As String
    Dim document_title As String = ""
    Try
      'let's figure out the document title!
      If jetnet_ac.Text <> "0" Or client_ac.Text <> "0" Then
        If jetnet_ac.Text <> "0" Then
          aTempTable = aclsData_Temp.GetJETNET_Aircraft_Details_AC_ID(jetnet_ac.Text, "")
          If Not IsNothing(aTempTable) Then
            If aTempTable.Rows.Count > 0 Then
              document_title = document_title & aTempTable.Rows(0).Item("amod_make_name") & " " & aTempTable.Rows(0).Item("amod_model_name") & "-" & aTempTable.Rows(0).Item("ac_ser_nbr")
            End If
          End If
        ElseIf client_ac.Text <> "0" Then
          aTempTable = aclsData_Temp.Get_Clients_Aircraft(client_ac.Text)
          If Not IsNothing(aTempTable) Then
            If aTempTable.Rows.Count > 0 Then
              document_title = document_title & aTempTable.Rows(0).Item("cliamod_make_name") & " " & aTempTable.Rows(0).Item("cliamod_model_name") & "-" & aTempTable.Rows(0).Item("cliaircraft_ser_nbr")
            End If
          End If
        End If
      End If

      If jetnet_comp.Text <> "0" Or client_comp.Text <> "0" Then
        If jetnet_comp.Text <> "0" Then
          aTempTable = aclsData_Temp.GetCompanyInfo_ID(jetnet_comp.Text, "JETNET", 0)
        ElseIf client_comp.Text <> "0" Then
          aTempTable = aclsData_Temp.GetCompanyInfo_ID(client_comp.Text, "CLIENT", 0)
        End If

        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            If document_title <> "" Then
              document_title = document_title & "-"
            End If
            document_title = document_title & aTempTable.Rows(0).Item("comp_name")
          End If
        End If
      End If


      If jetnet_contact.Text <> "0" Or client_contact.Text <> "0" Then
        If jetnet_contact.Text <> "0" Then
          aTempTable = aclsData_Temp.GetContacts_Details(jetnet_contact.Text, "JETNET")
        ElseIf client_contact.Text <> "0" Then
          aTempTable = aclsData_Temp.GetContacts_Details(client_contact.Text, "CLIENT")
        End If

        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            If document_title <> "" Then
              document_title = document_title & "-"
            End If
            document_title = document_title & aTempTable.Rows(0).Item("contact_first_name") & " " & aTempTable.Rows(0).Item("contact_last_name")
          End If
        End If
      End If



      If Not IsDBNull(document_title) Then
        document_title = Replace(document_title, " ", "")
        document_title = Replace(document_title, ",", "")
        document_title = Replace(document_title, ",", "")
        document_title = Replace(document_title, ".", "")
      End If
    Catch ex As Exception
      error_string = "edit_note.aspx.vb - build_file_name - " & ex.Message
      LogError(error_string)
    End Try
    Build_File_Name = document_title
  End Function
  ''' <summary>
  ''' Sends Email from the Email Part of the CRM
  ''' </summary>
  ''' <param name="from">Person Sending, CRM User</param>
  ''' <param name="recepient">Person receiving</param>
  ''' <param name="bcc">Person blind copied</param>
  ''' <param name="cc">Person copied</param>
  ''' <param name="subject">Email Subject</param>
  ''' <param name="body">Email Body</param>
  ''' <remarks></remarks>
  Public Sub Send_Email(ByVal from As String, ByVal recepient As String, ByVal bcc As String, ByVal cc As String, ByVal subject As String, ByVal body As String, ByVal aclsData_Temp As clsData_Manager_SQL)
    Try
      '' Instantiate a new instance of MailMessage
      'Dim mMailMessage As New MailMessage()

      '' Set the sender address of the mail message
      'mMailMessage.From = New MailAddress(from)
      '' Set the recepient address of the mail message
      'mMailMessage.To.Add(New MailAddress(recepient))

      '' Check if the bcc value is nothing or an empty string
      'If Not bcc Is Nothing And bcc <> String.Empty Then
      '  ' Set the Bcc address of the mail message
      '  mMailMessage.Bcc.Add(New MailAddress(bcc))
      'End If

      '' Check if the cc value is nothing or an empty value
      'If Not cc Is Nothing And cc <> String.Empty Then
      '  ' Set the CC address of the mail message
      '  mMailMessage.CC.Add(New MailAddress(cc))
      'End If

      '' Set the subject of the mail message
      'mMailMessage.Subject = subject
      '' Set the body of the mail message
      'mMailMessage.Body = body

      aclsData_Temp.InsertCRMMailQueue("Evolution", Session.Item("localUser").crmLocalUserFirstName & " " & Session.Item("localUser").crmLocalUserLastName, from, "smtp.jetnet.com", "customerservice@jetnet.com", "cservice123", recepient, cc, bcc, subject, body, Session.Item("localUser").crmUserCompanyID, Session.Item("localSubscription").crmSubscriptionID, "MPM", Replace(UCase(Application.Item("crmClientSiteData").crmClientHostName), "WWW", ""))

      'fileupload

      'Dim attach As System.Net.Mail.Attachment
      'Here's where we attach the file that they uploaded, if they upload a file.
      'Dim FileUpload1 As New FileUpload
      'If Not IsNothing(Email1.FindControl("FileUpload1")) Then
      '  If Email1.Visible = True Then
      '    FileUpload1 = Email1.FindControl("FileUpload1")
      '    If Not IsNothing(FileUpload1) Then
      '      If FileUpload1.PostedFile.FileName <> "" Then
      '        mMailMessage.Attachments.Add(New Attachment(FileUpload1.PostedFile.InputStream, FileUpload1.FileName))
      '      End If
      '    End If
      '  End If
      'End If
      'mMailMessage.Attachments.Add(FileUpload1)
      ' Set the format of the mail message body as HTML
      'mMailMessage.IsBodyHtml = True
      '' Set the priority of the mail message to normal
      'mMailMessage.Priority = MailPriority.Normal

      '' Instantiate a new instance of SmtpClient
      'Dim mSmtpClient As New SmtpClient("localhost", 25)
      '' Send the mail message
      'mSmtpClient.Send(mMailMessage)

    Catch ex As Exception
      error_string = "edit_note.aspx.vb - SendEmail() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
#End Region
#Region "events"

  Private Sub Notes1_remove_note_ev(ByVal idnum As Integer, ByVal con As Control, ByVal type As String) Handles Notes1.remove_note_ev, Opportunities1.remove_note_ev, ActionItems1.remove_note_ev, Documents1.remove_note_ev, Email1.remove_note_ev, Wanted1.remove_note_ev

    remove_note(idnum, con, type)
  End Sub
  Private Sub Notes1_ac_searchClick(ByVal con As Control) Handles Opportunities1.ac_searchClick, Documents1.ac_searchClick, ActionItems1.ac_searchClick, Notes1.ac_searchClick, Wanted1.ac_searchClick, Email1.ac_searchClick
    ac_SearchClick(con)
  End Sub
  Private Sub Notes1_Aircraft_Name_Changed(ByVal con As Control, ByVal FillModel As Boolean) Handles Opportunities1.Aircraft_Name_Changed, Notes1.Aircraft_Name_Changed, ActionItems1.Aircraft_Name_Changed, Documents1.Aircraft_Name_Changed, Email1.Aircraft_Name_Changed, Wanted1.Aircraft_Name_Changed
    aircraft_name_changed(con, FillModel)
  End Sub
  Private Sub Notes1_company_name_changed(ByVal con As Control) Handles Opportunities1.company_name_changed, Notes1.company_name_changed, ActionItems1.company_name_changed, Documents1.company_name_changed, Email1.company_name_changed, Wanted1.company_name_changed

        If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Or Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
            evoadmin_company_name_changed(con)
        Else
            company_name_changed(con)
        End If

    End Sub
  Private Sub Notes1_company_searchClick(ByVal con As Control) Handles Opportunities1.company_searchClick, Notes1.company_searchClick, Documents1.company_searchClick, ActionItems1.company_searchClick, Email1.company_searchClick, Wanted1.company_searchClick
    company_SearchClick(con)
  End Sub
  Private Sub Notes1_contact_name_changed(ByVal con As Control) Handles Opportunities1.contact_name_changed, Notes1.contact_name_changed, ActionItems1.contact_name_changed, Documents1.contact_name_changed, Email1.contact_name_changed, Wanted1.contact_name_changed
    contact_name_changed(con)
  End Sub
  Private Sub Notes1_FillCompanyDrop(ByVal con As Control) Handles Opportunities1.FillCompanyDrop, Notes1.FillCompanyDrop, ActionItems1.FillCompanyDrop, Documents1.FillCompanyDrop, Email1.FillCompanyDrop, Wanted1.FillCompanyDrop
    company_drop_fill(con)
  End Sub
  Private Sub Opportunities1_fill_drop(ByVal jetnet_ac As Integer, ByVal client_ac As Integer, ByVal jetnet_comp As Integer, ByVal client_comp As Integer, ByVal jetnet_contact As Integer, ByVal client_contact As Integer, ByVal jetnet_mod As Integer, ByVal client_mod As Integer, ByVal con As Control, ByVal FillModel As Boolean) Handles Documents1.fill_drop, Notes1.fill_drop, ActionItems1.fill_drop, Email1.fill_drop, Opportunities1.fill_drop, Wanted1.fill_drop
    Fill_All_DropDowns(jetnet_ac, client_ac, jetnet_comp, client_comp, jetnet_contact, client_contact, client_mod, jetnet_mod, con, FillModel)
  End Sub
    Private Sub Opportunities1_update_note(ByVal type As String, ByVal con As Control, ByVal idnum As Integer) Handles Documents1.edit_note, Notes1.edit_note, ActionItems1.edit_note, Email1.edit_note, Opportunities1.edit_note, Wanted1.edit_note

        Add_Note(type, con, idnum)

    End Sub

#End Region
#Region "Error Handling for datamanager"
    Function display_error()
    display_error = ""
    If aclsData_Temp.class_error <> "" Then
      System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Error", "alert('" & Replace(Replace(aclsData_Temp.class_error, "'", ""), vbNewLine, "") & "');", True)
    End If
    aclsData_Temp.class_error = ""
  End Function
  Public Sub LogError(ByVal ex As String)
    aclsData_Temp.LogError(Application.Item("crmClientSiteData").crmClientHostName, ex, DateTime.Now.ToString())
  End Sub
#End Region
#Region "Repeated"
  '-------------------------------------------------------------------Important Note------------------------------------------------------------------------------
  '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
  '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
  '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
  '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
  'These need to only be on one page only. These show up on the edit.aspx page as well. Since they don't share a common page, I'm not sure how to share these functions between them.
  'But this will definitely need to be cleaned up using a class.
  Private Function Fill_Company(ByVal jetnet_id As Integer) As Integer
    Try
      aTempTable = aclsData_Temp.GetCompanyInfo_ID(jetnet_id, "JETNET", 0)
      If Not IsNothing(aTempTable) Then 'not nothing
        Dim aclsClient_Company As New clsClient_Company
        Dim comp_id As Integer = 0
        For Each r As DataRow In aTempTable.Rows
          aclsClient_Company.clicomp_user_id = Session.Item("localUser").crmLocalUserID
          aclsClient_Company.clicomp_name = r("comp_name")
          If Not IsDBNull(r("comp_alternate_name_type")) Then
            aclsClient_Company.clicomp_alternate_name_type = r("comp_alternate_name_type")
          End If
          If Not IsDBNull(r("comp_alternate_name")) Then
            aclsClient_Company.clicomp_alternate_name = r("comp_alternate_name")
          End If
          If Not IsDBNull(r("comp_address1")) Then
            aclsClient_Company.clicomp_address1 = r("comp_address1")
          End If
          If Not IsDBNull(r("comp_address2")) Then
            aclsClient_Company.clicomp_address2 = r("comp_address2")
          End If
          If Not IsDBNull(r("comp_city")) Then
            aclsClient_Company.clicomp_city = r("comp_city")
          End If
          If Not IsDBNull(r("comp_state")) Then
            aclsClient_Company.clicomp_state = r("comp_state")
          End If
          If Not IsDBNull(r("comp_zip_code")) Then
            aclsClient_Company.clicomp_zip_code = r("comp_zip_code")
          End If
          If Not IsDBNull(r("comp_country")) Then
            aclsClient_Company.clicomp_country = r("comp_country")
          End If
          If Not IsDBNull(r("comp_agency_type")) Then
            aclsClient_Company.clicomp_agency_type = r("comp_agency_type")
          End If
          If Not IsDBNull(r("comp_web_address")) Then
            aclsClient_Company.clicomp_web_address = r("comp_web_address")
          End If
          If Not IsDBNull(r("comp_email_address")) Then
            aclsClient_Company.clicomp_email_address = r("comp_email_address")
          End If
          aclsClient_Company.clicomp_date_updated = Now()
          aclsClient_Company.clicomp_jetnet_comp_id = r("comp_id")
          comp_id = r("comp_id")
        Next
        Dim insert_data As Boolean = True
        Dim idnum_new As Integer
        'inserting that info into the database. 
        Dim carry_on As Boolean = False
        aTempTable2 = aclsData_Temp.GetCompanyInfo_JETNET_ID(comp_id, "")
        If aTempTable2.Rows.Count = 0 Then 'This jetnet record isn't in a company record yet, so let's insert it.
          If aclsData_Temp.Insert_Client_Company(aclsClient_Company) = True Then
            carry_on = True
          End If
        Else
          'Doesn't need phone or contacts.
          insert_data = False
          carry_on = True
          'already exists don't add to database just swap ID
          comp_id = jetnet_id
        End If

        aTempTable = aclsData_Temp.GetCompanyInfo_JETNET_ID(comp_id, "")
        If Not IsNothing(aTempTable) Then 'not nothing
          For Each r As DataRow In aTempTable.Rows
            idnum_new = r("comp_id")
          Next
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            clsGeneral.clsGeneral.LogError("edit_note.ascx.vb - Fill_Company() - " & error_string, aclsData_Temp)
          End If
          display_error()
        End If

        If carry_on = True And insert_data = True Then
          'This means that the company information got stored correctly.

          aTempTable = aclsData_Temp.GetCompanyInfo_JETNET_ID(comp_id, "")
          If Not IsNothing(aTempTable) Then 'not nothing
            For Each r As DataRow In aTempTable.Rows
              idnum_new = r("comp_id")
              aTempTable2 = aclsData_Temp.GetPhoneNumbers(comp_id, 0, "JETNET", 0)
              If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then
                  For Each q As DataRow In aTempTable2.Rows
                    Dim aclsClient_Phone_Numbers As New clsClient_Phone_Numbers

                    aclsClient_Phone_Numbers.clipnum_type = q("pnum_type")
                    aclsClient_Phone_Numbers.clipnum_number = q("pnum_number")
                    aclsClient_Phone_Numbers.clipnum_comp_id = r("comp_id") 'This is the comp_id of the new company we just inserted.
                    aclsClient_Phone_Numbers.clipnum_contact_id = 0
                    If aclsData_Temp.Insert_Client_PhoneNumbers(aclsClient_Phone_Numbers) = True Then
                      ' Response.Write("insert contact phone Number<br />")
                    Else
                      If aclsData_Temp.class_error <> "" Then
                        error_string = aclsData_Temp.class_error
                        clsGeneral.clsGeneral.LogError("edit_note.ascx.vb - Fill_Company() - " & error_string, aclsData_Temp)
                      End If
                    End If
                  Next 'for each in get phone numbers
                End If
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  clsGeneral.clsGeneral.LogError("edit_note.ascx.vb - Fill_Company() - " & error_string, aclsData_Temp)
                End If
              End If
            Next 'For each row in get company info
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            clsGeneral.clsGeneral.LogError("edit_note.ascx.vb - Fill_Company() - " & error_string, aclsData_Temp)
          End If
        End If

        get_insert_ac(jetnet_id, idnum_new, True, True, 0)
        'This is where I have to get all the other contacts from the jetnet company!!! Besides the one
        'That we have the id for!

        Dim status As Boolean = False

        loop_contacts(idnum_new, comp_id, jetnet_id, False, status)

        '====================

        Fill_Company = idnum_new
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          clsGeneral.clsGeneral.LogError("edit_note.ascx.vb - Fill_Company() - " & error_string, aclsData_Temp)
        End If
        display_error()
      End If
    Catch ex As Exception
      error_string = "edit_note.ascx.vb - Fill_Company() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Function
  Function loop_contacts(ByVal idnum_new As Integer, ByVal jetnet_id As Integer, ByVal skip_one As Integer, ByVal add_ref_here As Boolean, ByVal inactive As String)
    loop_contacts = ""

    Dim status As String = "Y"

    If inactive = True Then
      status = "N"
    End If

    'After that's cleared, we need to take all of the contacts
    Try
      'This is where we have to add the contacts that were already linked with this company. 
      'Make sure to use the jetnet_id id. This is important because we're using jetnet ID to get the existing contacts. 

      aTempTable = aclsData_Temp.GetContacts(jetnet_id, "JETNET", "Y", 0)

      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then

          'This loops through all of the contacts. 
          For Each r As DataRow In aTempTable.Rows

            If r("contact_id") <> skip_one Then
              Dim aclsClient_Contact As New clsClient_Contact
              aclsClient_Contact.clicontact_user_id = Session.Item("localUser").crmLocalUserID
              If Not IsDBNull(r("contact_sirname")) Then
                aclsClient_Contact.clicontact_sirname = r("contact_sirname")
              End If
              If Not IsDBNull(r("contact_first_name")) Then
                aclsClient_Contact.clicontact_first_name = r("contact_first_name")
              End If
              If Not IsDBNull(r("contact_middle_initial")) Then
                aclsClient_Contact.clicontact_middle_initial = r("contact_middle_initial")
              End If
              If Not IsDBNull(r("contact_last_name")) Then
                aclsClient_Contact.clicontact_last_name = r("contact_last_name")
              End If
              If Not IsDBNull(r("contact_suffix")) Then
                aclsClient_Contact.clicontact_suffix = r("contact_suffix")
              End If
              If Not IsDBNull(r("contact_title")) Then
                aclsClient_Contact.clicontact_title = r("contact_title")
              End If
              If Not IsDBNull(r("contact_email_address")) Then
                aclsClient_Contact.clicontact_email_address = r("contact_email_address")
              End If

              aclsClient_Contact.clicontact_date_updated = Now()
              aclsClient_Contact.clicontact_jetnet_contact_id = r("contact_id")
              aclsClient_Contact.clicontact_comp_id = idnum_new
              aclsClient_Contact.clicontact_status = status
              Dim contact_id_new As Integer


              'This attempts to insert this contact record. 
              If aclsData_Temp.Insert_Client_Contact(aclsClient_Contact) = True Then
                'not done yet. Now we have to get the phone numbers based on the contact and insert them.
                'First we need to get the contact id of what we just inserted.. 
                'Have to get the new contact ID 
                aTempTable2 = aclsData_Temp.GetContactInfo_JETNET_ID(r("contact_id"), "Y")
                If Not IsNothing(aTempTable2) Then
                  If aTempTable2.Rows.Count > 0 Then
                    For Each q As DataRow In aTempTable2.Rows
                      contact_id_new = q("contact_id")
                    Next 'this loops through contact ID record
                  Else 'rows = 0 
                  End If
                Else
                  If aclsData_Temp.class_error <> "" Then
                    error_string = aclsData_Temp.class_error
                    clsGeneral.clsGeneral.LogError("edit_note.ascx.vb - loop_contacts() - " & error_string, aclsData_Temp)
                  End If
                End If

                'Inserting new contact phone numbers. 
                aTempTable2 = aclsData_Temp.GetContact_PhoneNumbers(r("contact_id"))
                If Not IsNothing(aTempTable2) Then
                  If aTempTable2.Rows.Count > 0 Then
                    For Each q As DataRow In aTempTable2.Rows
                      Dim aclsClient_Phone_Numbers As New clsClient_Phone_Numbers
                      If Not IsDBNull(q("pnum_type")) Then
                        aclsClient_Phone_Numbers.clipnum_type = q("pnum_type")
                      End If
                      If Not IsDBNull(q("pnum_number")) Then
                        aclsClient_Phone_Numbers.clipnum_number = q("pnum_number")
                      End If
                      aclsClient_Phone_Numbers.clipnum_comp_id = idnum_new
                      aclsClient_Phone_Numbers.clipnum_contact_id = contact_id_new
                      If aclsData_Temp.Insert_Client_PhoneNumbers(aclsClient_Phone_Numbers) = True Then
                        '  Response.Write("insert contact phone Number<br />")
                      Else
                        'Response.Write("Update Client Contact Fail")
                      End If
                    Next 'This loops through new contact phone numbers


                  Else ' rows = 0
                  End If
                Else
                  If aclsData_Temp.class_error <> "" Then
                    error_string = aclsData_Temp.class_error
                    clsGeneral.clsGeneral.LogError("edit_note.ascx.vb - loop_contacts() - " & error_string, aclsData_Temp)
                  End If
                End If

                'Get all the AC information.
                If add_ref_here = True Then
                  aTempTable2 = aclsData_Temp.Get_Aircraft_Reference_JETNET_contactID(r("contact_id"))
                  If Not IsNothing(aTempTable2) Then
                    If aTempTable2.Rows.Count > 0 Then
                      For Each q As DataRow In aTempTable2.Rows
                        Dim aclsInsert_Client_Aircraft_Reference As New clsClient_Aircraft_Reference

                        aclsInsert_Client_Aircraft_Reference.cliacref_comp_id = idnum_new
                        If Not IsDBNull(q("acref_contact_type")) Then
                          aclsInsert_Client_Aircraft_Reference.cliacref_contact_type = q("acref_contact_type")
                        End If
                        aclsInsert_Client_Aircraft_Reference.cliacref_contact_id = contact_id_new

                        aclsInsert_Client_Aircraft_Reference.cliacref_jetnet_ac_id = q("acref_ac_id")
                        If Not IsDBNull(q("acref_operator_flag")) Then
                          aclsInsert_Client_Aircraft_Reference.cliacref_operator_flag = q("acref_operator_flag")
                        End If
                        aclsInsert_Client_Aircraft_Reference.cliacref_owner_percentage = IIf(Not IsDBNull(q("acref_owner_percentage")), q("acref_owner_percentage"), "0")
                        If Not IsDBNull(q("acref_business_type")) Then
                          aclsInsert_Client_Aircraft_Reference.cliacref_business_type = q("acref_business_type")
                        End If
                        aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_expires = Now()
                        aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_purchased = Now()
                        If aclsData_Temp.Insert_Client_Aircraft_Reference(aclsInsert_Client_Aircraft_Reference) = True Then
                        Else
                          If aclsData_Temp.class_error <> "" Then
                            error_string = aclsData_Temp.class_error
                            clsGeneral.clsGeneral.LogError("edit_note.ascx.vb - loop_contacts() - " & error_string, aclsData_Temp)
                          End If
                        End If
                      Next
                    End If
                  Else
                    If aclsData_Temp.class_error <> "" Then
                      error_string = aclsData_Temp.class_error
                      clsGeneral.clsGeneral.LogError("edit_note.ascx.vb - loop_contacts() - " & error_string, aclsData_Temp)
                    End If
                  End If
                End If
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  clsGeneral.clsGeneral.LogError("edit_note.ascx.vb - loop_contacts() - " & error_string, aclsData_Temp)
                End If
              End If 'if client is inserted
            End If 'end skip
          Next ' This loops through all the contacts. 


        End If
      End If
    Catch ex As Exception
      error_string = "edit_note.ascx.vb - loop_contacts() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Function
  Function get_insert_ac(ByVal jetnet_id As Integer, ByVal idnum_new As Integer, ByVal get_previous_ac As Boolean, ByVal insert_ac As Boolean, ByVal new_client_ac As Integer)
    get_insert_ac = ""
    Try
      If get_previous_ac = True Then
        'Get the AC's from the old DB
      End If

      If insert_ac = True Then
        'Insert the AC information
      End If
      aTempTable2 = aclsData_Temp.Get_Aircraft_Reference_JETNET_compID(jetnet_id)
      If Not IsNothing(aTempTable2) Then
        If aTempTable2.Rows.Count > 0 Then
          For Each q As DataRow In aTempTable2.Rows
            Dim aclsInsert_Client_Aircraft_Reference As New clsClient_Aircraft_Reference


            aclsInsert_Client_Aircraft_Reference.cliacref_comp_id = idnum_new


            aclsInsert_Client_Aircraft_Reference.cliacref_cliac_id = new_client_ac


            aclsInsert_Client_Aircraft_Reference.cliacref_contact_type = CStr(q("acref_contact_type"))
            aclsInsert_Client_Aircraft_Reference.cliacref_contact_id = 0
            aclsInsert_Client_Aircraft_Reference.cliacref_jetnet_ac_id = CStr(q("acref_ac_id"))
            aclsInsert_Client_Aircraft_Reference.cliacref_operator_flag = CStr(q("acref_operator_flag"))
            aclsInsert_Client_Aircraft_Reference.cliacref_owner_percentage = IIf(Not IsDBNull(q("acref_owner_percentage")), q("acref_owner_percentage"), "0")
            aclsInsert_Client_Aircraft_Reference.cliacref_business_type = CStr(q("acref_business_type"))

            aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_expires = Now()
            aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_purchased = Now()

            If aclsData_Temp.Insert_Client_Aircraft_Reference(aclsInsert_Client_Aircraft_Reference) = True Then
            Else
              If aclsData_Temp.class_error <> "" Then
                error_string = aclsData_Temp.class_error
                clsGeneral.clsGeneral.LogError("edit_note.ascx.vb - get_insert_ac() - " & error_string, aclsData_Temp)
              End If
            End If
          Next
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          clsGeneral.clsGeneral.LogError("edit_note.ascx.vb - get_insert_ac() - " & error_string, aclsData_Temp)
        End If
        display_error()
      End If
    Catch ex As Exception
      error_string = "edit_note.ascx.vb - get_insert_ac() - " & ex.Message
      LogError(error_string)
    End Try
  End Function
#End Region
#Region "Documents"
  Public Sub Display_Document(ByVal file_name As String, ByVal id As String)
    If Application.Item("crmClientSiteData").WebSiteType = eWebSiteTypes.LOCAL Then
      Response.Redirect("Documents/" & file_name, False)
    Else
      Dim file_location As String = Replace(LCase(Application.Item("crmClientSiteData").crmClientHostName()), "www.", "")
      ' Specify the directories you want to manipulate.
      Dim path As String = "D:\crmDocuments\" & file_location & "\" & file_name
      Dim ext As String = System.IO.Path.GetExtension(path).ToLower()
      Dim path2 As String = Server.MapPath("\Documents\") & Session.Item("localUser").crmLocalUserID & "_" & id & "_tmp" & ext

      Try
        ' 'make sure path 2 doesn't exist first .
        If File.Exists(path2) Then
          File.Delete(path2)
        End If

        ' Copy the file.
        File.Copy(path, path2)

        Dim url As String = "Documents/" & Session.Item("localUser").crmLocalUserID & "_" & id & "_tmp" & ext

        Response.Redirect(url, False)
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)

      Catch e As Exception
        Response.Write("problem" & e.Message)
      End Try
    End If
  End Sub
#End Region


#Region "Notes Control - Regarding the Sale Panel"

  Sub SetReferenceSaleControls(ByRef name As Control, ByRef ac_sale As RadioButtonList, ByRef ac_status_not_for_sale As DropDownList, ByRef ac_status_for_sale As DropDownList, ByRef CompareValidator1 As CompareValidator, ByRef date_listed_panel As Panel, ByRef date_listed As TextBox, ByRef DOMlisted As Label, ByRef DOMWord As Label, ByRef est_label As Label, ByRef cliaircraft_value_description_text As TextBox, ByRef est_price As TextBox, ByRef broker_price As TextBox, ByRef broker_lbl As Label, ByRef asking_price As TextBox, ByRef asking_wordage As DropDownList, ByRef ask_lbl As Label)

    If Not IsNothing(name.FindControl("ac_sale")) Then
      ac_sale = name.FindControl("ac_sale")
    End If


    If Not IsNothing(name.FindControl("ac_status_not_for_sale")) Then
      ac_status_not_for_sale = name.FindControl("ac_status_not_for_sale")
    End If


    If Not IsNothing(name.FindControl("ac_status_for_sale")) Then
      ac_status_for_sale = name.FindControl("ac_status_for_sale")
    End If


    If Not IsNothing(name.FindControl("CompareValidator1")) Then
      CompareValidator1 = name.FindControl("CompareValidator1")
    End If

    If Not IsNothing(name.FindControl("date_listed_panel")) Then
      date_listed_panel = name.FindControl("date_listed_panel")
    End If


    If Not IsNothing(name.FindControl("date_listed")) Then
      date_listed = name.FindControl("date_listed")
    End If


    If Not IsNothing(name.FindControl("DOMListed")) Then
      DOMlisted = name.FindControl("DOMListed")
    End If


    If Not IsNothing(name.FindControl("DOMWord")) Then
      DOMWord = name.FindControl("DOMWord")
    End If


    If Not IsNothing(name.FindControl("est_label")) Then
      est_label = name.FindControl("est_label")
    End If


    If Not IsNothing(name.FindControl("cliaircraft_value_description_text")) Then
      cliaircraft_value_description_text = name.FindControl("cliaircraft_value_description_text")
    End If


    If Not IsNothing(name.FindControl("est_price")) Then
      est_price = name.FindControl("est_price")
    End If


    If Not IsNothing(name.FindControl("broker_price")) Then
      broker_price = name.FindControl("broker_price")
    End If


    If Not IsNothing(name.FindControl("broker_lbl")) Then
      broker_lbl = name.FindControl("broker_lbl")
    End If


    If Not IsNothing(name.FindControl("asking_price")) Then
      asking_price = name.FindControl("asking_price")
    End If


    If Not IsNothing(name.FindControl("asking_wordage")) Then
      asking_wordage = name.FindControl("asking_wordage")
    End If

    If Not IsNothing(name.FindControl("ask_lbl")) Then
      ask_lbl = name.FindControl("ask_lbl")
    End If

  End Sub

  Sub FillUpForSaleControls(ByVal Aircraft_Data As clsClient_Aircraft, ByVal name As Control, ByRef ac_sale As RadioButtonList, ByRef ac_status_not_for_sale As DropDownList, ByRef ac_status_for_sale As DropDownList, ByRef CompareValidator1 As CompareValidator, ByRef date_listed_panel As Panel, ByRef date_listed As TextBox, ByRef DOMlisted As Label, ByRef DOMWord As Label, ByRef est_label As Label, ByRef cliaircraft_value_description_text As TextBox, ByRef est_price As TextBox, ByRef broker_price As TextBox, ByRef broker_lbl As Label, ByRef asking_price As TextBox, ByRef asking_wordage As DropDownList, ByRef ask_lbl As Label)

    ac_sale.SelectedValue = Aircraft_Data.cliaircraft_forsale_flag

    If ac_sale.SelectedValue = "Y" Then
      ac_status_not_for_sale.Attributes.Add("style", "display:none;")
      ac_status_for_sale.Attributes.Add("style", "display:block;")
      ac_status_for_sale.Items.Add(New ListItem(Aircraft_Data.cliaircraft_status, Aircraft_Data.cliaircraft_status))
      ac_status_for_sale.SelectedValue = Aircraft_Data.cliaircraft_status

      If UCase(Trim(Aircraft_Data.cliaircraft_asking_wordage)) = "PRICE" Then
        asking_price.Text = FormatNumber(Aircraft_Data.cliaircraft_asking_price, 2)
        asking_price.Attributes.Add("style", "display:block;")
        ask_lbl.Attributes.Add("style", "display:block;")
        est_label.Attributes.Add("style", "display:block;")
        est_price.Attributes.Add("style", "display:block;")
        broker_price.Attributes.Add("style", "display:block;")
        broker_lbl.Attributes.Add("style", "display:block;")
        asking_wordage.Attributes.Add("style", "display:block;")
        date_listed_panel.Attributes.Add("style", "display:block;")
      End If

      CompareValidator1.Enabled = True
      date_listed_panel.Attributes.Add("style", "display:block;")
      est_label.Attributes.Add("style", "display:block;")
      est_price.Attributes.Add("style", "display:block;")
      broker_price.Attributes.Add("style", "display:block;")
      broker_lbl.Attributes.Add("style", "display:block;")
      broker_price.Text = Aircraft_Data.cliaircraft_broker_price
      est_price.Text = Aircraft_Data.cliaircraft_est_price

      cliaircraft_value_description_text.Text = Aircraft_Data.cliaircraft_value_description

      If Not IsDBNull(Aircraft_Data.cliaircraft_date_listed) Then
        If Not IsNothing(Aircraft_Data.cliaircraft_date_listed) Then
          date_listed.Text = CStr(IIf(((Aircraft_Data.cliaircraft_date_listed <> "12:00:00 AM")), Aircraft_Data.cliaircraft_date_listed, ""))
        End If
      End If

      If Not IsDBNull(Aircraft_Data.cliaircraft_date_listed) Then
        If Not IsNothing(Aircraft_Data.cliaircraft_date_listed) Then
          DOMlisted.Text = DateDiff(DateInterval.Day, CDate(Aircraft_Data.cliaircraft_date_listed), Now()) & " Days"
          DOMWord.Visible = True
        End If
      End If

      asking_wordage.SelectedValue = Trim(Aircraft_Data.cliaircraft_asking_wordage)

    ElseIf ac_sale.SelectedValue = "N" Then
      If Not IsDBNull(Aircraft_Data.cliaircraft_date_listed) Then
        If Not IsNothing(Aircraft_Data.cliaircraft_date_listed) Then
          date_listed.Text = CStr(IIf(((Aircraft_Data.cliaircraft_date_listed <> "12:00:00 AM")), Aircraft_Data.cliaircraft_date_listed, ""))
        End If
      End If

      ac_status_not_for_sale.Attributes.Add("style", "display:block;")
      ac_status_for_sale.Attributes.Add("style", "display:none;")
      ac_status_not_for_sale.Items.Add(New ListItem(Aircraft_Data.cliaircraft_status, Aircraft_Data.cliaircraft_status))
      ac_status_not_for_sale.SelectedValue = Aircraft_Data.cliaircraft_status

      cliaircraft_value_description_text.Text = Aircraft_Data.cliaircraft_value_description
      CompareValidator1.Enabled = False
      date_listed_panel.Attributes.Add("style", "display:none;")
      asking_price.Text = "0.00"
      est_price.Text = "0.00"
      broker_price.Text = "0.00"
    End If

  End Sub


#End Region
End Class
