Imports System.IO
Partial Public Class Contact_Reference_Edit_Template
  Inherits System.Web.UI.UserControl
  Public aclsData_Temp As New clsData_Manager_SQL
  Dim aTempTable, aTempTable2 As New DataTable 'Data Tables used
  Public Event loop_contacts(ByVal idnum_new As Integer, ByVal jetnet_id As Integer, ByVal contact_id As Integer, ByVal skip As Boolean, ByVal inactive As Boolean)
  Dim error_string As String = ""
#Region "Page Events"
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Me.Visible Then
      Try

        aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
        aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase") 'CApplication.Item("crmJetnetDatabase")


        aclsData_Temp.class_error = ""
        If Trim(Request("action")) <> "new" Then
          If Trim(Request("type")) = "company" And Trim(Request("action")) = "edit" Then
            view_panel_table.Width = 1010
            sep.Visible = True
            sep1.Visible = True
          ElseIf Trim(Request("action")) <> "reference" Then

            Me.Visible = False
          End If
          If Trim(Request("remove")) <> "" Then

            Dim aclsDelete_Client_Aircraft_Reference_cliacref_id As New clsClient_Aircraft_Reference
            Dim idnum As Integer = CInt(Trim(Request("id")))
            If aclsData_Temp.Delete_Client_Aircraft_Reference_cliacref_id(idnum) = True Then
              System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location.reload();", True)
              System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
            Else
              If aclsData_Temp.class_error <> "" Then
                error_string = aclsData_Temp.class_error
                LogError("ContactCard.ascx.vb - remove_con() - " & error_string)
              End If
              display_error()
            End If

            '-----------------------------------------This is adding a contact to an aircraft code-----------------------------------------------
          ElseIf Session.Item("Listing") = 3 Then
            Dim idnum As Integer = Session.Item("ListingID")



            'adding code for the contact quick entry
            contactQuickEntry.Attributes.Add("onClick", "javascript:window.open('edit.aspx?action=quick&acID=" & idnum & "&acSource=" & Session("ListingSource") & "','_blank','scrollbars=yes,menubar=no,height=750,width=1110,resizable=yes,toolbar=no,location=no,status=no');")


            contact_ref_add.Visible = True
            ac_info_display.Text = Display_Jetnet_AC(idnum)
            If Not Page.IsPostBack Then
              'These need to be dynamic but we're running way too short on time
              contact_name.Items.Add(New ListItem("Please perform a contact search below", ""))
              priority.Items.Add(New ListItem("NONE", "3"))
              priority.Items.Add(New ListItem("PRIMARY", "1"))
              priority.Items.Add(New ListItem("SECONDARY", "2"))
              priority.Items.Add(New ListItem("OTHER", "3"))
              priority.SelectedValue = "3"
              relationship_con.Items.Add(New ListItem("NONE", ""))

              aTempTable = aclsData_Temp.Get_CRM_Client_Aircraft_Contact_Type()
              If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then
                  For Each r As DataRow In aTempTable.Rows
                    relationship_con.Items.Add(New ListItem(r("cliact_name"), r("cliact_type")))
                  Next
                End If
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("Contact_Reference_Edit_Template.ascx.vb - Page Load() - " & error_string)
                End If
                display_error()
              End If
            End If
          ElseIf Session("Listing") = 1 And Session("ListingSource") = "CLIENT" Then
            view_panel.Visible = True
            If Not Page.IsPostBack Then
              'These need to be dynamic but we're running way too short on time
              relationship.Items.Add(New ListItem("None", ""))
              aircraft_name.Items.Add(New ListItem("None Selected", 0))
              ac_priority.Items.Add(New ListItem("NONE", ""))
              ac_priority.Items.Add(New ListItem("PRIMARY", "1"))
              ac_priority.Items.Add(New ListItem("SECONDARY", "2"))
              ac_priority.Items.Add(New ListItem("OTHER", "3"))

              aTempTable = aclsData_Temp.Get_CRM_Client_Aircraft_Contact_Type()
              If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then
                  For Each r As DataRow In aTempTable.Rows
                    relationship.Items.Add(New ListItem(r("cliact_name"), r("cliact_type")))
                  Next
                End If
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("Contact_Reference_Edit_Template.ascx.vb - Page Load() - " & error_string)
                End If
                display_error()
              End If
            End If

            Dim idnum As Integer = Session.Item("ListingID")
            company_view_text.Text = Comp_Display(idnum, "CLIENT")
          End If
        End If
      Catch ex As Exception
        error_string = "Contact_Reference_Edit_Template.ascx.vb - Page Load() " & ex.Message
        LogError(error_string)
      End Try
    End If

  End Sub
#End Region
#Region "Basic AC/Contact/Comp Display Functions"
  Function Comp_Display(ByVal x As Integer, ByVal y As String)

    'Company display information
    Dim contact_text As String = ""
    Comp_Display = ""
    Try
      aTempTable = aclsData_Temp.GetCompanyInfo_ID(x, y, 0)
      ' check the state of the DataTable
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          For Each R As DataRow In aTempTable.Rows
            contact_text = R("comp_name") & "<br />"
            contact_text = contact_text & R("comp_address1") & "<br />"
            contact_text = contact_text & R("comp_city") & ", " & R("comp_state") & " "
            contact_text = contact_text & R("comp_zip_code") & "<br />"
            contact_text = contact_text & R("comp_country") & "<br />"
            contact_text = contact_text & "<a href='mailto:" & R("comp_email_address") & "' class='non_special_link'>" & R("comp_email_address") & "</a>" & "<br />"
          Next
        Else '0 rows
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("Contact_Reference_Edit_Template.ascx.vb - Comp_Display() - " & error_string)
        End If
        display_error()
      End If
      Comp_Display = contact_text
    Catch ex As Exception
      error_string = "Contact_Reference_Edit_Template.ascx.vb - Comp_Display() - " & ex.Message
      LogError(error_string)
    End Try
  End Function
  Private Function Display_Jetnet_AC(ByVal x As Integer)
    Dim aError As String = ""
    Display_Jetnet_AC = ""
    Try
      Dim aircraft_text As String = ""
      If Session.Item("ListingSource") = "CLIENT" Then
        aTempTable = aclsData_Temp.Get_Clients_Aircraft(x)
        ' check the state of the DataTable
        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            aircraft_edit_text.Text = CommonAircraftFunctions.CreateHeaderLine(aTempTable.Rows(0).Item("cliamod_make_name"), aTempTable.Rows(0).Item("cliamod_model_name"), aTempTable.Rows(0).Item("cliaircraft_ser_nbr"), " Contact Reference")

            For Each R As DataRow In aTempTable.Rows
              If Not IsDBNull(R("cliaircraft_year_mfr")) Then
                If R("cliaircraft_year_mfr") <> "" Then
                  aircraft_text = "Year: " & R("cliaircraft_year_mfr") & "<br />"
                End If
              End If
              If Not IsDBNull(R("cliaircraft_reg_nbr")) Then
                If R("cliaircraft_reg_nbr") <> "" Then
                  aircraft_text = aircraft_text & "Reg #: " & R("cliaircraft_reg_nbr") & "<br />"
                End If
              End If
              If Not IsDBNull(R("cliaircraft_ser_nbr")) Then
                If R("cliaircraft_ser_nbr") <> "" Then
                  aircraft_text = aircraft_text & "Ser #: " & R("cliaircraft_ser_nbr") & "<br />"
                End If
              End If
              If Not IsDBNull(R("cliaircraft_forsale_flag")) Then
                If R("cliaircraft_forsale_flag") = "Y" Then
                  If Not IsDBNull(R("cliaircraft_status")) Then
                    aircraft_text = aircraft_text & "<b class='green'>" & R("cliaircraft_status")
                  End If
                  If Not IsDBNull(R("cliaircraft_status")) Then
                    If R("cliaircraft_delivery") <> "" Then
                      aircraft_text = aircraft_text & " - " & R("cliaircraft_delivery")
                    End If
                  End If

                  If Not IsDBNull(R("cliaircraft_asking_wordage")) Then
                    If R("cliaircraft_asking_wordage") <> "" Then
                      If R("cliaircraft_asking_wordage") = "Price" Then
                        If Not IsDBNull(R("cliaircraft_asking_price")) Then
                          aircraft_text = aircraft_text & " Asking: " & FormatCurrency(R("cliaircraft_asking_price"), 0)
                        End If
                      Else
                        aircraft_text = aircraft_text & " " & R("cliaircraft_asking_wordage")
                      End If
                    End If
                    aircraft_text = aircraft_text & "</b><br />"
                  End If
                End If
              End If
              If Not IsDBNull(R("cliaircraft_asking_wordage")) Then
                If R("cliaircraft_status") <> "" Then
                  Select Case R("cliaircraft_status")
                    Case "For Sale"
                    Case Else
                      aircraft_text = aircraft_text & R("cliaircraft_status") & "<br />"
                  End Select
                End If
              End If
            Next
            'contact_info.Text = aircraft_text
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("Contact_Reference_Edit_Template.ascx.vb - Display_Jetnet_AC() - " & error_string)
          End If
          display_error()
        End If
        Display_Jetnet_AC = aircraft_text
      Else
        '--------Basic Aircraft Left Card Display for the AC Information------------------------------------------------
        Try
          aTempTable = aclsData_Temp.GetJETNET_Aircraft_Details_AC_ID(x, aError)
          ' check the state of the DataTable
          If Not IsNothing(aTempTable) Then
            If aTempTable.Rows.Count > 0 Then
              aircraft_edit_text.Text = CommonAircraftFunctions.CreateHeaderLine(aTempTable.Rows(0).Item("amod_make_name"), aTempTable.Rows(0).Item("amod_model_name"), aTempTable.Rows(0).Item("ac_ser_nbr"), " Contact Reference")
              For Each R As DataRow In aTempTable.Rows
                If Not IsDBNull(R("ac_year_mfr")) Then
                  If R("ac_year_mfr") <> "" Then
                    aircraft_text = "Year: " & R("ac_year_mfr") & "<br />"
                  End If
                End If
                If Not IsDBNull(R("ac_year_dlv")) Then
                  If R("ac_year_dlv") <> "" Then
                    aircraft_text = aircraft_text & "Delivered: " & R("ac_year_dlv") & "<br />"
                  End If
                End If
                If Not IsDBNull(R("ac_reg_nbr")) Then
                  If R("ac_reg_nbr") <> "" Then
                    aircraft_text = aircraft_text & "Reg #: " & R("ac_reg_nbr") & "<br />"
                  End If
                End If

                If Not IsDBNull(R("ac_prev_reg_nbr")) Then
                  If R("ac_prev_reg_nbr") <> "" Then
                    aircraft_text = aircraft_text & "Previous Reg #: " & R("ac_prev_reg_nbr") & "<br />"
                  End If
                End If

                If Not IsDBNull(R("ac_ser_nbr")) Then
                  If R("ac_ser_nbr") <> "" Then
                    aircraft_text = aircraft_text & "Ser #: " & R("ac_ser_nbr") & "<br />"
                  End If
                End If
                If Not IsDBNull(R("ac_alt_ser_nbr")) Then
                  If R("ac_alt_ser_nbr") <> "" Then
                    aircraft_text = aircraft_text & "Alt. Ser #: " & R("ac_alt_ser_nbr") & "<br />"
                  End If
                End If
                If Not IsDBNull(R("ac_forsale_flag")) Then
                  If R("ac_forsale_flag") = "Y" Then
                    aircraft_text = aircraft_text & "<b class='green'>" & R("ac_status")
                    If Not IsDBNull(R("ac_delivery")) Then
                      If R("ac_delivery") <> "" Then
                        aircraft_text = aircraft_text & " - " & R("ac_delivery")
                      End If
                    End If
                    If Not IsDBNull(R("ac_asking_wordage")) Then
                      If R("ac_asking_wordage") <> "" Then
                        If R("ac_asking_wordage") = "Price" Then
                          If Not IsDBNull(R("ac_asking_price")) Then
                            aircraft_text = aircraft_text & " Asking: " & FormatCurrency(R("ac_asking_price"), 0)
                          End If
                        Else
                          aircraft_text = aircraft_text & " " & R("ac_asking_wordage")
                        End If
                      End If
                    End If
                    aircraft_text = aircraft_text & "</b><br />"
                  End If
                End If

                If Not IsDBNull(R("ac_status")) Then
                  If R("ac_status") <> "" Then
                    Select Case R("ac_status")
                      Case "For Sale"
                      Case Else
                        aircraft_text = aircraft_text & R("ac_status") & "<br />"
                    End Select
                  End If
                End If

                If Not IsDBNull(R("ac_lifecycle")) Then
                  Select Case R("ac_lifecycle")
                    Case "1"
                      aircraft_text = aircraft_text & "In Production<br />"
                    Case "2"
                      aircraft_text = aircraft_text & "New<br />"
                    Case "3"
                      aircraft_text = aircraft_text & "In Operation<br />"
                    Case "4"
                      aircraft_text = aircraft_text & "Retired<br />"
                  End Select
                End If
                Display_Jetnet_AC = aircraft_text
              Next
            Else
              '0 rows
            End If
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = aclsData_Temp.class_error
              LogError("Contact_Reference_Edit_Template.ascx.vb - Display_Jetnet_AC() - " & error_string)
            End If
            display_error()
          End If

        Catch ex As Exception
          error_string = "Contact_Reference_Edit_Template.ascx.vb - Display_Jetnet_AC() - " & ex.Message
          LogError(error_string)
        End Try
      End If
    Catch ex As Exception
      error_string = "Contact_Reference_Edit_Template.ascx.vb - Display_Jetnet_AC() - " & ex.Message
      LogError(error_string)
    End Try
  End Function
  Function contact_display(ByVal x As Integer, ByVal y As String)
    contact_display = ""
    Try
      aTempTable = aclsData_Temp.GetContacts_Details(x, y)
      Dim comp_id As Integer = 0
      Dim contact_text_right As String = ""
      ' check the state of the DataTable
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          For Each R As DataRow In aTempTable.Rows
            contact_text_right = R("contact_first_name") & " " & R("contact_middle_initial") & " " & R("contact_last_name") & "<br />"
            contact_text_right = contact_text_right & R("contact_title")
            aTempTable = aclsData_Temp.GetCompanyInfo_ID(R("contact_comp_id"), y, 0)
            If Not IsNothing(aTempTable) Then
              If aTempTable.Rows.Count > 0 Then
                For Each q As DataRow In aTempTable.Rows
                  contact_text_right = contact_text_right & "<br />(" & q("comp_name") & ")<br />"
                Next
              End If
            Else
              If aclsData_Temp.class_error <> "" Then
                error_string = aclsData_Temp.class_error
                LogError("Contact_Reference_Edit_Template.ascx.vb - Contact_Display() - " & error_string)
              End If
              display_error()
            End If
            contact_text_right = contact_text_right & "<br />" & "<a href='mailto:" & R("contact_email_address") & "' class='non_special_link'>" & R("contact_email_address") & "</a>"
            If UCase(y) = "CLIENT" Then
              contact_text_right = contact_text_right & R("contact_notes")
            End If
          Next
        Else
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("Contact_Reference_Edit_Template.ascx.vb - Contact_Display() - " & error_string)
        End If
        display_error()
      End If
      contact_display = contact_text_right

    Catch ex As Exception
      error_string = "Contact_Reference_Edit_Template.ascx.vb - Contact_Display() - " & ex.Message
      LogError(error_string)
    End Try
  End Function
#End Region
#Region "Add Contact Functions"
  Private Sub searching_cont_ref_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles searching_cont_ref.Click, contact_search_ref_2.Click
    Try
      Dim searched As String = "C"
      Dim jetnet_IDS As String = ""
      Dim client_IDS As String = ""
      If input.SelectedValue = "jetnet_input" Then
        searched = "J"
      Else
        searched = "C"
      End If
      'searched = "C"
      Dim contact As Boolean = True
      Dim client_string As String = ""
      Dim jetnet_string As String = ""

      If last.Text = "" And first.Text = "" And sender.id = "searching_cont_ref" Then
        contact = False
        client_string = "CLICOMP_NAME AS ""COMP_NAME"",clicomp_id as ""contact_comp_id"", clicomp_address1 as comp_address1, clicomp_state as comp_state, 'CLIENT' as contact_type, clicomp_city as comp_city, clicomp_country as comp_country"
        jetnet_string = "COMP_NAME,comp_id as ""contact_comp_id"", comp_address1, comp_address2, comp_state, comp_city, comp_country, 'JETNET' as contact_type "
        aTempTable = aclsData_Temp.Export_All("COMP_NAME", client_string, jetnet_string, True, contact, False, False, searched, "", "", "%" & clsGeneral.clsGeneral.Get_Name_Search_String(company.Text) & "%", "Y", "", "", "", False, "%" & clsGeneral.clsGeneral.StripChars(first.Text, True) & "%", "%" & clsGeneral.clsGeneral.StripChars(last.Text, True) & "%", "", "", "2", "%", "", "", "", "", "", "", "", "", "", "", Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmJets_Flag, Session.Item("localSubscription").crmExecutive_Flag, Session.Item("localSubscription").crmTurboprops, Session.Item("localSubscription").crmCommercial_Flag, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "")
        'contact_name_vis.Visible = True


        comp_name.Items.Clear()
        'contact_name.Items.Add(New ListItem("NONE SELECTED", ""))

        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            For Each r As DataRow In aTempTable.Rows
              customize_reference.Visible = True
              comp_name_vis.Visible = True
              comp_name.Visible = True
              contact_ref_add_errormsg.Text = ""
              comp_name.Items.Add(New ListItem(CStr(r("comp_name") & " - " & r("comp_city") & " (" & r("comp_country") & ") (" & r("contact_type") & ")"), "0|" & r("contact_type") & "|" & r("contact_comp_id")))

            Next

          Else
            contact_ref_add_errormsg.Text = "<p align='center'>No Companies were found with these parameters.</p>"
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("Contact_Reference_Edit_Template.ascx.vb - searching_cont_ref_Click()  - " & error_string)
          End If
          display_error()
        End If
      Else
        contact = True
        client_string = "CLICOMP_NAME AS ""COMP_NAME"",clicomp_id as ""contact_comp_id"", CLICONTACT_SIRNAME AS ""CONTACT_SIRNAME"",CLICONTACT_FIRST_NAME AS ""CONTACT_FIRST_NAME"",clicontact_id as ""contact_id"",'CLIENT' as contact_type, CLICONTACT_LAST_NAME AS ""CONTACT_LAST_NAME"",CLICONTACT_TITLE AS ""CONTACT_TITLE"",CLICONTACT_EMAIL_ADDRESS AS ""CONTACT_EMAIL_ADDRESS"""
        jetnet_string = "COMP_NAME,comp_id as ""contact_comp_id"", CONTACT_SIRNAME,CONTACT_FIRST_NAME,CONTACT_LAST_NAME, CONTACT_TITLE,CONTACT_EMAIL_ADDRESS, contact_id, 'JETNET' as contact_type "

        If company.Text <> "" Then
          aTempTable = aclsData_Temp.Export_All("COMP_NAME", client_string, jetnet_string, True, contact, False, False, searched, "", "", "%" & clsGeneral.clsGeneral.StripChars(company.Text, True) & "%", "Y", "", "", "", False, "%" & clsGeneral.clsGeneral.StripChars(first.Text, True) & "%", "%" & clsGeneral.clsGeneral.StripChars(last.Text, True) & "%", "", "", "2", "%", "", "", "", "", "", "", "", "", "", "", Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmJets_Flag, Session.Item("localSubscription").crmExecutive_Flag, Session.Item("localSubscription").crmTurboprops, Session.Item("localSubscription").crmCommercial_Flag, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "")
        Else
          first.Text = existing_first.Text
          last.Text = existing_last.Text
          If jetnet_comp_id.Text <> "" Then
            jetnet_IDS = "'" & jetnet_comp_id.Text & "'"
            searched = "J"
          Else
            client_IDS = "'" & client_comp_id.Text & "'"
            searched = "C"
          End If
          aTempTable = aclsData_Temp.Export_All("COMP_NAME", client_string, jetnet_string, True, contact, False, False, searched, "", "", "%%", "Y", "", "", "", False, "%" & clsGeneral.clsGeneral.StripChars(first.Text, True) & "%", "%" & clsGeneral.clsGeneral.StripChars(last.Text, True) & "%", "", "", "2", "%", "", "", "", "", "", "", "", "", "", "", Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmJets_Flag, Session.Item("localSubscription").crmExecutive_Flag, Session.Item("localSubscription").crmTurboprops, Session.Item("localSubscription").crmCommercial_Flag, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", jetnet_IDS, client_IDS, "", "", "")
        End If

        contact_name_vis.Visible = True
        customize_reference.Visible = True

        contact_name.Items.Clear()
        'contact_name.Items.Add(New ListItem("NONE SELECTED", ""))

        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            For Each r As DataRow In aTempTable.Rows
              If Not IsDBNull(r("contact_title")) Then
                If r("contact_title") <> "" Then
                  contact_name.Items.Add(New ListItem(CStr(r("contact_title") & " - " & r("contact_sirname") & " " & r("contact_first_name") & " " & r("contact_last_name") & " (" & r("comp_name") & ")"), r("contact_id") & "|" & r("contact_type") & "|" & r("contact_comp_id")))
                Else
                  contact_name.Items.Add(New ListItem(CStr(r("contact_sirname") & " " & r("contact_first_name") & " " & r("contact_last_name") & " (" & r("comp_name") & ")"), r("contact_id") & "|" & r("contact_type") & "|" & r("contact_comp_id")))
                End If
              Else
                contact_name.Items.Add(New ListItem(CStr(r("contact_sirname") & " " & r("contact_first_name") & " " & r("contact_last_name") & " (" & r("comp_name") & ")"), r("contact_id") & "|" & r("contact_type") & "|" & r("contact_comp_id")))
              End If

            Next

          Else
            '0 rows
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("Contact_Reference_Edit_Template.ascx.vb - searching_cont_ref_Click() - " & error_string)
          End If
          display_error()
        End If

      End If

    Catch ex As Exception
      error_string = "Contact_Reference_Edit_Template.ascx.vb - searching_cont_ref_Click() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Private Sub contact_name_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles contact_name.SelectedIndexChanged
    Try
      contact_ref_id.Text = contact_name.SelectedValue
      customize_relationship.Visible = True
      Dim whatis As Array = Split(contact_ref_id.Text, "|")
      If whatis(1) = "JETNET" Then
        contact_info.Text = contact_display(IIf(CStr(whatis(0)) = "", 0, whatis(0)), "JETNET")
      Else
        contact_info.Text = contact_display(IIf(CStr(whatis(0)) = "", 0, whatis(0)), "CLIENT")
      End If
    Catch ex As Exception
      error_string = "Contact_Reference_Edit_Template.ascx.vb - contact_name_SelectedIndexChanged() " & ex.Message
      LogError(error_string)
    End Try
  End Sub
#End Region
#Region "Add contact reference to aircraft function"
  Private Sub add_cont_ref_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles add_cont_ref.Click
    'Here's a different one. We need to take a contact.
    'Add the company, add the other contacts, add the contact. Add the reference.
    Try
      Dim whatis As Array = Split(contact_ref_id.Text, "|")
      Dim cont As Boolean = True
      If whatis(1) = "JETNET" Then
        Dim idnum_new As Integer
        Dim jetnet_id As Integer = whatis(2)
        Dim contact_id As Integer = whatis(0)

        Try
          aTempTable = aclsData_Temp.GetCompanyInfo_JETNET_ID(whatis(2), "")
          If Not IsNothing(aTempTable) Then 'not nothing
            If aTempTable.Rows.Count > 0 Then
              idnum_new = aTempTable.Rows(0).Item("comp_id")
              cont = False
              If contact_id <> 0 Then
                aTempTable2 = aclsData_Temp.GetContactInfo_JETNET_ID(contact_id, "Y")
                If Not IsNothing(aTempTable2) Then 'not nothing
                  If aTempTable2.Rows.Count > 0 Then
                    contact_id = aTempTable2.Rows(0).Item("contact_id")
                  End If
                End If
              End If
            End If
          End If

          If cont = True Then
            aTempTable = aclsData_Temp.GetCompanyInfo_ID(whatis(2), "JETNET", 0)
            If Not IsNothing(aTempTable) Then 'not nothing
              'This jetnet record isn't in a company record yet, so let's insert it.
              Dim aclsClient_Company As New clsClient_Company
              Dim comp_id As Integer = 0
              For Each r As DataRow In aTempTable.Rows
                If Not IsDBNull(r("comp_name")) Then
                  aclsClient_Company.clicomp_name = r("comp_name")
                End If
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

              'inserting that info into the database. 
              If aclsData_Temp.Insert_Client_Company(aclsClient_Company) = True Then
                'This means that the company information got stored correctly.

                aTempTable = aclsData_Temp.GetCompanyInfo_JETNET_ID(comp_id, "")
                If Not IsNothing(aTempTable) Then 'not nothing
                  For Each r As DataRow In aTempTable.Rows

                    aTempTable2 = aclsData_Temp.GetPhoneNumbers(comp_id, 0, "JETNET", 0)
                    If Not IsNothing(aTempTable) Then
                      If aTempTable.Rows.Count > 0 Then
                        For Each q As DataRow In aTempTable2.Rows
                          Dim aclsClient_Phone_Numbers As New clsClient_Phone_Numbers
                          idnum_new = r("comp_id")
                          aclsClient_Phone_Numbers.clipnum_type = q("pnum_type")
                          aclsClient_Phone_Numbers.clipnum_number = q("pnum_number")
                          aclsClient_Phone_Numbers.clipnum_comp_id = r("comp_id") 'This is the comp_id of the new company we just inserted.
                          aclsClient_Phone_Numbers.clipnum_contact_id = 0
                          If aclsData_Temp.Insert_Client_PhoneNumbers(aclsClient_Phone_Numbers) = True Then
                            ' Response.Write("insert contact phone Number<br />")
                          Else
                            If aclsData_Temp.class_error <> "" Then
                              error_string = aclsData_Temp.class_error
                              LogError("Contact_Reference_Edit_Template.ascx.vb - add_cont_ref_Click() - " & error_string)
                            End If
                            display_error()
                          End If
                        Next 'for each in get phone numbers
                      End If
                    Else
                      If aclsData_Temp.class_error <> "" Then
                        error_string = aclsData_Temp.class_error
                        LogError("Contact_Reference_Edit_Template.ascx.vb - add_cont_ref_Click() - " & error_string)
                      End If
                      display_error()
                    End If
                  Next 'For each row in get company info
                End If
              End If
            End If

            RaiseEvent loop_contacts(idnum_new, jetnet_id, contact_id, False, False)

            Dim aclsClient_Contact As New clsClient_Contact

            aTempTable = aclsData_Temp.GetContacts_Details(contact_id, "JETNET")
            If Not IsNothing(aTempTable) Then
              If aTempTable.Rows.Count > 0 Then

                For Each r As DataRow In aTempTable.Rows

                  aclsClient_Contact = New clsClient_Contact
                  'This is where I insert that last one.
                  aclsClient_Contact.clicontact_sirname = CStr(r("contact_sirname"))
                  aclsClient_Contact.clicontact_first_name = CStr(r("contact_first_name"))
                  aclsClient_Contact.clicontact_middle_initial = CStr(r("contact_middle_initial"))
                  aclsClient_Contact.clicontact_last_name = CStr(r("contact_last_name"))
                  aclsClient_Contact.clicontact_suffix = CStr(r("contact_suffix"))
                  aclsClient_Contact.clicontact_title = CStr(r("contact_title"))
                  aclsClient_Contact.clicontact_email_address = CStr(r("contact_email_address"))
                  aclsClient_Contact.clicontact_date_updated = Now()
                  aclsClient_Contact.clicontact_status = "Y"
                  ' set to 0 since this is a Client record
                  aclsClient_Contact.clicontact_jetnet_contact_id = contact_id
                  aclsClient_Contact.clicontact_comp_id = idnum_new
                  Dim contact_id_new As Integer
                  Try
                    'Now finally we insert the contact. 
                    If aclsData_Temp.Insert_Client_Contact(aclsClient_Contact) = True Then
                      '  Response.Write("Insert Client Contact Success")
                      'And closes the form and sends the user on their way. 
                      aTempTable2 = aclsData_Temp.GetContactInfo_JETNET_ID(contact_id, "Y")
                      If Not IsNothing(aTempTable2) Then 'not nothing
                        'Insert the new phone numbers
                        If aTempTable2.Rows.Count > 0 Then
                          contact_id_new = aTempTable2.Rows(0).Item("contact_id")
                        End If
                      Else
                        If aclsData_Temp.class_error <> "" Then
                          error_string = aclsData_Temp.class_error
                          LogError("Contact_Reference_Edit_Template.ascx.vb - add_cont_ref_Click() - " & error_string)
                        End If
                        display_error()
                      End If
                    End If

                    Dim aclsInsert_Client_Aircraft_Reference As New clsClient_Aircraft_Reference

                    aclsInsert_Client_Aircraft_Reference.cliacref_comp_id = idnum_new
                    aclsInsert_Client_Aircraft_Reference.cliacref_contact_type = relationship_con.SelectedValue
                    aclsInsert_Client_Aircraft_Reference.cliacref_contact_id = contact_id_new
                    If Session.Item("ListingSource") = "JETNET" Then
                      aclsInsert_Client_Aircraft_Reference.cliacref_jetnet_ac_id = Session.Item("ListingID")
                      aclsInsert_Client_Aircraft_Reference.cliacref_cliac_id = 0
                    ElseIf Session.Item("ListingSource") = "CLIENT" Then
                      aclsInsert_Client_Aircraft_Reference.cliacref_jetnet_ac_id = 0
                      aclsInsert_Client_Aircraft_Reference.cliacref_cliac_id = Session.Item("ListingID")
                    End If

                    aclsInsert_Client_Aircraft_Reference.cliacref_operator_flag = ""
                    aclsInsert_Client_Aircraft_Reference.cliacref_owner_percentage = "0"
                    aclsInsert_Client_Aircraft_Reference.cliacref_contact_priority = CInt(priority.SelectedValue)
                    aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_expires = Now()
                    aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_purchased = Now()
                    aclsInsert_Client_Aircraft_Reference.cliacref_business_type = ""

                    If aclsData_Temp.Insert_Client_Aircraft_Reference(aclsInsert_Client_Aircraft_Reference) = True Then
                      ref_two_add.Text = "Your Reference has been Added."
                      System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location.reload();", True)
                      System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
                    Else
                      If aclsData_Temp.class_error <> "" Then
                        error_string = aclsData_Temp.class_error
                        LogError("Contact_Reference_Edit_Template.ascx.vb - add_cont_ref_Click() - " & error_string)
                      End If
                      display_error()
                    End If

                  Catch ex As Exception
                    error_string = "Contact_Reference_Edit_Template.ascx.vb - add_cont_ref_Click() - " & ex.Message
                    LogError(error_string)
                  End Try
                Next
              Else
                Dim aclsInsert_Client_Aircraft_Reference As New clsClient_Aircraft_Reference

                aclsInsert_Client_Aircraft_Reference.cliacref_comp_id = idnum_new
                aclsInsert_Client_Aircraft_Reference.cliacref_contact_type = relationship_con.SelectedValue
                aclsInsert_Client_Aircraft_Reference.cliacref_contact_id = 0
                If Session.Item("ListingSource") = "JETNET" Then
                  aclsInsert_Client_Aircraft_Reference.cliacref_jetnet_ac_id = Session.Item("ListingID")
                  aclsInsert_Client_Aircraft_Reference.cliacref_cliac_id = 0
                ElseIf Session.Item("ListingSource") = "CLIENT" Then
                  aclsInsert_Client_Aircraft_Reference.cliacref_jetnet_ac_id = 0
                  aclsInsert_Client_Aircraft_Reference.cliacref_cliac_id = Session.Item("ListingID")
                End If

                aclsInsert_Client_Aircraft_Reference.cliacref_operator_flag = ""
                aclsInsert_Client_Aircraft_Reference.cliacref_owner_percentage = "0"
                aclsInsert_Client_Aircraft_Reference.cliacref_contact_priority = CInt(priority.SelectedValue)
                aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_expires = Now()
                aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_purchased = Now()
                aclsInsert_Client_Aircraft_Reference.cliacref_business_type = ""

                If aclsData_Temp.Insert_Client_Aircraft_Reference(aclsInsert_Client_Aircraft_Reference) = True Then
                  ref_two_add.Text = "Your Reference has been Added."
                  System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location.reload();", True)
                  System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
                Else
                  If aclsData_Temp.class_error <> "" Then
                    error_string = aclsData_Temp.class_error
                    LogError("Contact_Reference_Edit_Template.ascx.vb - add_cont_ref_Click() - " & error_string)
                  End If
                  display_error()
                End If

              End If
            End If
          ElseIf cont = False Then

            'company already exists already exists
            Dim aclsInsert_Client_Aircraft_Reference As New clsClient_Aircraft_Reference

            aclsInsert_Client_Aircraft_Reference.cliacref_comp_id = idnum_new
            aclsInsert_Client_Aircraft_Reference.cliacref_contact_type = relationship_con.SelectedValue
            aclsInsert_Client_Aircraft_Reference.cliacref_contact_id = contact_id
            If Session.Item("ListingSource") = "JETNET" Then
              aclsInsert_Client_Aircraft_Reference.cliacref_jetnet_ac_id = Session.Item("ListingID")
              aclsInsert_Client_Aircraft_Reference.cliacref_cliac_id = 0
            ElseIf Session.Item("ListingSource") = "CLIENT" Then
              aclsInsert_Client_Aircraft_Reference.cliacref_jetnet_ac_id = 0
              aclsInsert_Client_Aircraft_Reference.cliacref_cliac_id = Session.Item("ListingID")
            End If

            aclsInsert_Client_Aircraft_Reference.cliacref_operator_flag = ""
            aclsInsert_Client_Aircraft_Reference.cliacref_owner_percentage = "0"
            aclsInsert_Client_Aircraft_Reference.cliacref_contact_priority = CInt(priority.SelectedValue)
            aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_expires = Now()
            aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_purchased = Now()
            aclsInsert_Client_Aircraft_Reference.cliacref_business_type = ""

            If aclsData_Temp.Insert_Client_Aircraft_Reference(aclsInsert_Client_Aircraft_Reference) = True Then
              ref_two_add.Text = "Your Reference has been Added."
              System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location.reload();", True)
              System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
            Else
              If aclsData_Temp.class_error <> "" Then
                error_string = aclsData_Temp.class_error
                LogError("Contact_Reference_Edit_Template.ascx.vb - add_cont_ref_Click() - " & error_string)
              End If
              display_error()
            End If
          End If
        Catch ex As Exception
          error_string = "Contact_Reference_Edit_Template.ascx.vb - add_cont_ref_Click() - " & ex.Message
          LogError(error_string)
        End Try

      Else
        Dim aclsInsert_Client_Aircraft_Reference As New clsClient_Aircraft_Reference
        aclsInsert_Client_Aircraft_Reference.cliacref_comp_id = whatis(2)
        aclsInsert_Client_Aircraft_Reference.cliacref_contact_type = relationship_con.SelectedValue

        aclsInsert_Client_Aircraft_Reference.cliacref_contact_id = IIf(CStr(whatis(0)) = "", 0, whatis(0))


        If Session.Item("ListingSource") = "JETNET" Then
          aclsInsert_Client_Aircraft_Reference.cliacref_jetnet_ac_id = Session.Item("ListingID")
          aclsInsert_Client_Aircraft_Reference.cliacref_cliac_id = 0
        ElseIf Session.Item("ListingSource") = "CLIENT" Then
          aclsInsert_Client_Aircraft_Reference.cliacref_jetnet_ac_id = 0
          aclsInsert_Client_Aircraft_Reference.cliacref_cliac_id = Session.Item("ListingID")
        End If
        aclsInsert_Client_Aircraft_Reference.cliacref_operator_flag = ""
        aclsInsert_Client_Aircraft_Reference.cliacref_owner_percentage = "0"
        aclsInsert_Client_Aircraft_Reference.cliacref_contact_priority = priority.SelectedValue
        aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_expires = Now()
        aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_purchased = Now()
        aclsInsert_Client_Aircraft_Reference.cliacref_business_type = ""

        If aclsData_Temp.Insert_Client_Aircraft_Reference(aclsInsert_Client_Aircraft_Reference) = True Then
          ref_two_add.Text = "Your Reference has been Added."
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location.reload();", True)
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("Contact_Reference_Edit_Template.ascx.vb - add_cont_ref_Click() - " & error_string)
          End If
          display_error()
        End If

      End If
    Catch ex As Exception
      error_string = "Contact_Reference_Edit_Template.ascx.vb - add_cont_ref_Click() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
#End Region
#Region "Add AC to client Company function"
  Private Sub ac_search_button_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ac_search_button.Click
    Try
      TableRow2.Visible = True
      '5
      Dim source As String = ""
      Dim ser As String = ""
      Dim reg As String = ""
      Dim yearm As String = ""
      Dim id As Integer = 0
      aTempTable = aclsData_Temp.AC_Search_New("AMOD_MAKE_NAME ASC, AMOD_MODEL_NAME ASC, AC_SER_NBR_SORT ASC", "C", "all", "", "", "%" & ac_sear.Text & "%", "", "", "", "", "", "", "", "", "", Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmJets_Flag, Session.Item("localSubscription").crmExecutive_Flag, Session.Item("localSubscription").crmTurboprops, Session.Item("localSubscription").crmCommercial_Flag, Session.Item("localSubscription").crmAerodexFlag, "", "", "", "", "5", "2", "", "", "", "", "", "", "", "", "", "", "", "", 0, False)
      aircraft_name.Items.Clear()
      aircraft_name.Items.Add(New ListItem("None Selected", 0))
      aircraft_name.SelectedValue = 0
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          For Each r As DataRow In aTempTable.Rows
            If Not IsDBNull(r("source")) Then
              source = r("source")
            Else
              source = r("other_source")
            End If
            If Not IsDBNull(r("ac_id")) Then
              id = r("ac_id")
            Else
              id = r("other_ac_id")
            End If
            If Not IsDBNull(r("ac_year_mfr")) Then
              yearm = r("ac_year_mfr")
            Else
              yearm = r("other_ac_year_mfr")
            End If
            If Not IsDBNull(r("ac_ser_nbr")) Then
              ser = r("ac_ser_nbr")
            Else
              ser = r("other_ac_ser_nbr")
            End If

            If Not IsDBNull(r("ac_reg_nbr")) Then
              reg = r("ac_reg_nbr")
            Else
              reg = r("other_ac_reg_nbr")
            End If


            If r("ac_reg_nbr").ToString <> "" Then
              aircraft_name.Items.Add(New ListItem(CStr(yearm & " " & r("amod_make_name") & " " & r("amod_model_name") & " Ser #:" & ser & " Reg#:" & reg), source & "|" & id))
            Else
              aircraft_name.Items.Add(New ListItem(CStr(yearm & " " & r("amod_make_name") & " " & r("amod_model_name") & " Ser #:" & clsGeneral.clsGeneral.stripHTML(ser)), source & "|" & id))
            End If
          Next
        Else
          '0 rows
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("Contact_Reference_Edit_Template.ascx.vb - ac_search_button_Click() - " & error_string)
        End If
        display_error()
      End If
    Catch ex As Exception
      error_string = "Contact_Reference_Edit_Template.ascx.vb - ac_search_button_Click() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub

  Private Sub add_ref_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles add_ref.Click
    Dim aclsInsert_Client_Aircraft_Reference As New clsClient_Aircraft_Reference
    Dim idnum As Integer = Session.Item("ListingID")

    Dim cont As Integer
    Try
      cont = Session.Item("ContactID")
    Catch
      cont = 0
    End Try

    Dim jetnet As Integer = 0
    Dim client As Integer = 0
    Dim ar As Array = Split(UCase(aircraft_name.SelectedValue), "|")
    If ar(0) = "JETNET" Then
      jetnet = ar(1)
    ElseIf ar(0) = "CLIENT" Then
      client = ar(1)
    End If
    aclsInsert_Client_Aircraft_Reference.cliacref_comp_id = idnum
    aclsInsert_Client_Aircraft_Reference.cliacref_contact_type = relationship.SelectedValue
    aclsInsert_Client_Aircraft_Reference.cliacref_contact_id = cont
    aclsInsert_Client_Aircraft_Reference.cliacref_jetnet_ac_id = jetnet
    aclsInsert_Client_Aircraft_Reference.cliacref_cliac_id = client
    aclsInsert_Client_Aircraft_Reference.cliacref_operator_flag = ""
    aclsInsert_Client_Aircraft_Reference.cliacref_owner_percentage = "0"
    aclsInsert_Client_Aircraft_Reference.cliacref_contact_priority = IIf(IsNumeric(ac_priority.SelectedValue), ac_priority.SelectedValue, 0)
    aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_expires = Now()
    aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_purchased = Now()

    If aclsData_Temp.Insert_Client_Aircraft_Reference(aclsInsert_Client_Aircraft_Reference) = True Then
      ref_update.Text = "Your Reference has been Added."
      System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location = 'details.aspx';", True)
      System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
    Else
      display_error()
    End If

  End Sub
#End Region
  Public Function display_error()
    '------------------------------Function that Creates a Javascript Error if the data manager class errors-----------
    display_error = ""
    If aclsData_Temp.class_error <> "" Then
      System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Error", "alert('" & Replace(aclsData_Temp.class_error, "'", " \'") & "');", True)
    End If
    aclsData_Temp.class_error = ""
  End Function
  Public Sub LogError(ByVal ex As String)
    aclsData_Temp.LogError(Application.Item("crmClientSiteData").crmClientHostName, ex, DateTime.Now.ToString())
  End Sub

  Private Sub relationship_con_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles relationship_con.SelectedIndexChanged
    If relationship_con.SelectedValue <> "" Then
      add_cont_ref.Visible = True
    Else
      add_cont_ref.Visible = False
    End If
  End Sub

  Private Sub comp_name_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles comp_name.SelectedIndexChanged
    ' second_existing_company_panel.Visible = True

    first_name_vis.Visible = True
    last_name_vis.Visible = True
    first.Visible = True
    last.Visible = True
    contact_ref_id.Text = comp_name.SelectedValue
    customize_relationship.Visible = True
    Dim the_answer As Array = Split(comp_name.SelectedValue, "|")
    contact_name.Items.Clear()
    contact_info.Text = ""

    If comp_name.SelectedValue <> "" Then
      If UBound(the_answer) = 2 Then
        '"0|" & r("source") & "|" & r("contact_comp_id")
        Select Case the_answer(1).ToString
          Case "CLIENT"
            client_comp_id.Text = the_answer(2).ToString
            jetnet_comp_id.Text = ""
          Case "JETNET"
            jetnet_comp_id.Text = the_answer(2).ToString
            client_comp_id.Text = ""
        End Select
      End If
    End If
  End Sub

  Private Sub aircraft_name_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles aircraft_name.SelectedIndexChanged
    If aircraft_name.SelectedValue <> "0" Then
      company_ac_add.Visible = True
    Else
      company_ac_add.Visible = False
      add_ref.Visible = False
    End If

  End Sub

  Private Sub relationship_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles relationship.SelectedIndexChanged
    If relationship.SelectedValue <> "" Then
      add_ref.Visible = True
    Else
      add_ref.Visible = False
    End If
  End Sub

  Private Sub existing_company_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles existing_company_button.Click
    Dim idnum As Integer = Session.Item("ListingID")
    client_comp_id.Text = ""
    jetnet_comp_id.Text = ""
    contact_ref_id.Text = ""
    comp_name.Items.Clear()

    If idnum <> 0 Then
      ac_ref_instructions.Text = "Please pick choose the subset of existing companies."
      existing_company_panel.Visible = True
      search_company_panel.Visible = False
      customize_reference.Visible = True

    End If

  End Sub

  Private Sub search_company_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles search_company_button.Click
    ac_ref_instructions.Text = "Please perform a search for a contact or company first."
    search_company_panel.Visible = True
    existing_company_panel.Visible = False
    second_existing_company_panel.Visible = False
    customize_reference.Visible = False
    comp_name.Items.Clear()

    client_comp_id.Text = ""
    jetnet_comp_id.Text = ""
    contact_ref_id.Text = ""
    'company name visibility
    comp_name_vis.Visible = False
    comp_name.Visible = False
  End Sub

  Private Sub existing_subset_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles existing_subset.SelectedIndexChanged
    Dim distinct_table As New DataTable
    Dim distinct_table_view As New DataView
    Dim whole_data As New DataTable
    Dim tempTable As New DataTable
    aTempTable2 = New DataTable
    Dim idnum As Integer = Session.Item("ListingID")
    Dim searched As String = "CLIENT"
    If existing_subset.SelectedValue = "jetnet_input" Then
      searched = "JETNET"
    End If
    'company name visibility
    comp_name_vis.Visible = True
    comp_name.Visible = True
    comp_name.Items.Clear()
    If idnum <> 0 Then
      ac_ref_instructions.Text = "Please pick choose from the list of companies."
      'Here is what's going to happen.
      'When we click existing company button, we need to say, "Hey, let's figure out what source we have,
      'then we need to figure out what existing companies we have. We can do this by taking the aircraft contact datatables,
      'and making a unique dataview

      whole_data.Columns.Add("comp_id")
      whole_data.Columns.Add("comp_name")
      whole_data.Columns.Add("comp_city")
      whole_data.Columns.Add("comp_country")
      whole_data.Columns.Add("contact_type")
      whole_data.Columns.Add("contact_comp_id")
      whole_data.Columns.Add("source")

      Select Case UCase(Session("ListingSource"))
        Case "JETNET"
          'client
          tempTable = New DataTable
          aTempTable2 = New DataTable

          tempTable = aclsData_Temp.Get_Aircraft_Reference_Client_JetnetacID_Full_Details(idnum)

          'jetnet
          aTempTable2 = aclsData_Temp.Get_Aircraft_Reference_Jetnet_acID(idnum, 0)


          For Each r As DataRow In tempTable.Rows
            If r("source").ToString = searched Then
              Dim newCustomersRow As DataRow = whole_data.NewRow()

              newCustomersRow("comp_id") = r("comp_id").ToString
              newCustomersRow("comp_name") = r("comp_name").ToString
              newCustomersRow("comp_city") = r("comp_city").ToString
              newCustomersRow("comp_country") = r("comp_country").ToString
              newCustomersRow("contact_type") = r("acref_contact_type").ToString
              newCustomersRow("contact_comp_id") = r("comp_id").ToString
              newCustomersRow("source") = r("source").ToString
              whole_data.Rows.Add(newCustomersRow)
              whole_data.AcceptChanges()
            End If
          Next

          For Each r As DataRow In aTempTable2.Rows
            Dim newCustomersRow As DataRow = whole_data.NewRow()
            If r("source").ToString = searched Then
              newCustomersRow("comp_id") = r("acref_comp_id").ToString
              newCustomersRow("comp_name") = r("comp_name").ToString
              newCustomersRow("comp_city") = r("comp_city").ToString
              newCustomersRow("comp_country") = r("comp_country").ToString
              newCustomersRow("contact_type") = r("act_name").ToString
              newCustomersRow("contact_comp_id") = r("acref_comp_id").ToString
              newCustomersRow("source") = r("source").ToString
              whole_data.Rows.Add(newCustomersRow)
              whole_data.AcceptChanges()
            End If
          Next
        Case "CLIENT"
          aTempTable2 = New DataTable

          aTempTable2 = aclsData_Temp.Get_Aircraft_Reference_Client_acID_Full_Details(idnum)

          For Each r As DataRow In aTempTable2.Rows
            If searched = "CLIENT" Then
              Dim newCustomersRow As DataRow = whole_data.NewRow()

              newCustomersRow("comp_id") = r("comp_id").ToString
              newCustomersRow("comp_name") = r("comp_name").ToString
              newCustomersRow("comp_city") = r("comp_city").ToString
              newCustomersRow("comp_country") = r("comp_country").ToString
              newCustomersRow("contact_type") = r("acref_contact_type").ToString
              newCustomersRow("contact_comp_id") = r("comp_id").ToString
              newCustomersRow("source") = "CLIENT"
              whole_data.Rows.Add(newCustomersRow)
              whole_data.AcceptChanges()
            End If
          Next
      End Select
      distinct_table_view = whole_data.DefaultView

      distinct_table = distinct_table_view.ToTable(True, "comp_name", "comp_city", "comp_country", "source", "contact_comp_id")

      For Each r As DataRow In distinct_table.Rows
        If r("source") = searched Then
          comp_name.Items.Add(New ListItem(CStr(r("comp_name") & " - " & r("comp_city") & " (" & r("comp_country") & ") " & " (" & r("source") & ")"), "0|" & r("source") & "|" & r("contact_comp_id")))
        End If
      Next
    End If
  End Sub
End Class