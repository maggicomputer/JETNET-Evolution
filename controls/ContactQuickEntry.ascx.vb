Partial Public Class ContactQuickEntry
  Inherits System.Web.UI.UserControl
  Dim aclsData_Temp As New clsData_Manager_SQL
  Dim Error_String As String = ""
  Public Event Fill_Phone(ByVal type1 As String, ByVal type2 As String, ByVal type3 As String, ByVal type4 As String, ByVal type5 As String, ByVal type6 As String, ByVal con As Control)
  Dim AcID As Long = 0
  Dim AcSource As String = ""
  Dim AircraftRelationship As Boolean = False

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Session.Item("crmUserLogon") <> True Then
      Response.Redirect("Default.aspx", False)
    End If

    aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase") 'CApplication.Item("crmJetnetDatabase")
    aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")

    If Not Page.IsPostBack Then
      RaiseEvent Fill_Phone(type1.ID, type2.ID, type3.ID, cphone_type1.ID, cphone_type2.ID, cphone_type3.ID, Me)
    End If

    If Not IsNothing(Trim(Request("acID"))) Then
      If Not String.IsNullOrEmpty(Trim(Request("acID"))) Then
        AcID = Trim(Request("acID"))
      End If
    End If

    If Not IsNothing(Trim(Request("acSource"))) Then
      If Not String.IsNullOrEmpty(Trim(Request("acSource"))) Then
        AcSource = Trim(Request("acSource"))
      End If
    End If


    If AcID > 0 And AcSource <> "" Then
      Dim aTempTable As New DataTable
      Dim itemName As String = ""
      AircraftRelationship = True
      attach_note_to_aircraft_panel.Visible = True
      addAircraftNote.CssClass = "display_none"
      aircraft_prospects_checkbox.Checked = False
      attach_note_to_aircraft.Text = "Attach note to this aircraft?"
      contactTypeAircraft.Visible = True
      attach_note_to_aircraft.AutoPostBack = False

      If UCase(AcSource) = "CLIENT" Then
        aTempTable = aclsData_Temp.Get_Clients_Aircraft(AcID)
        itemName = "cliaircraft_cliamod_id"
      ElseIf UCase(AcSource) = "JETNET" Then
        aTempTable = aclsData_Temp.GetJETNET_Aircraft_Details_AC_ID(AcID, "")
        itemName = "ac_amod_id"
      End If


      If Not Page.IsPostBack Then
        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            'Do a lookup to get the model ID
            aircraft_note.Items.Add(New ListItem("", AcID & "|" & AcSource & "|" & aTempTable.Rows(0).Item(itemName)))
            aircraft_note.SelectedValue = AcID & "|" & AcSource & "|" & aTempTable.Rows(0).Item(itemName)
            Aircraft_Note_Changed()
          End If
        End If

        'Finally fill up the contact type
        aTempTable = aclsData_Temp.Get_CRM_Client_Aircraft_Contact_Type()
        If Not IsNothing(aTempTable) Then
          contactRelationship.Items.Add(New ListItem("Please Select One", ""))
          If aTempTable.Rows.Count > 0 Then
            For Each r As DataRow In aTempTable.Rows
              contactRelationship.Items.Add(New ListItem(r("cliact_name"), r("cliact_type")))
            Next
          End If

          contactRelationship.SelectedValue = ""
        Else
          If aclsData_Temp.class_error <> "" Then
            Error_String = aclsData_Temp.class_error
            LogError("ContactQuickEntry.aspx.vb - Page Load() - " & Error_String)
          End If
        End If
      End If

    End If
  End Sub
  Public Sub TextValidate(ByVal source As Object, ByVal args As ServerValidateEventArgs)
    If Not IsNothing(source.controltovalidate) Then
      Dim c As TextBox = FindControl(source.controltovalidate)
      Dim q As String = Replace(source.controltovalidate, "phone", "type")
      Dim d As DropDownList = FindControl(q)
      If c.Text <> "" Then
        If Not IsNothing(d) Then
          If d.SelectedValue = "" Then
            args.IsValid = False
          Else
            args.IsValid = True
          End If
        End If
      End If
    End If
  End Sub

  Private Sub company_instructions_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles company_instructions.SelectedIndexChanged
    If company_instructions.SelectedValue = "auto" Then
      ToggleContactCompany(False, "#888888")
    ElseIf company_instructions.SelectedValue = "enter_new" Then
      ToggleContactCompany(True, "#000000")
    ElseIf company_instructions.SelectedValue = "search" Then
      ToggleContactCompany(True, "#000000")
    End If
  End Sub

  Private Sub ToggleContactCompany(ByVal visible As Boolean, ByVal color As String)
    Dim cssClass_style As String = ""
    Dim opposite_style As String = ""
    If visible = False Then
      cssClass_style = "display_none"
    Else
      opposite_style = "display_none"
    End If
    'company toggle
    comp_name_label.ForeColor = System.Drawing.ColorTranslator.FromHtml(color)
    comp_name.CssClass = cssClass_style
    comp_name_lbl.CssClass = opposite_style
    comp_name_lbl.Text = comp_name.Text

    'company address
    comp_address_label.ForeColor = System.Drawing.ColorTranslator.FromHtml(color)
    comp_address.CssClass = cssClass_style
    comp_address_lbl.CssClass = opposite_style
    'company city
    comp_city_label.ForeColor = System.Drawing.ColorTranslator.FromHtml(color)
    comp_city.CssClass = cssClass_style
    comp_city_lbl.CssClass = opposite_style
    'company state
    comp_state.CssClass = cssClass_style
    comp_state_lbl.CssClass = opposite_style
    'company zip
    comp_zip.CssClass = cssClass_style
    comp_zip_lbl.CssClass = opposite_style
    'company country
    comp_country_label.ForeColor = System.Drawing.ColorTranslator.FromHtml(color)
    comp_country.CssClass = cssClass_style
    comp_country_lbl.CssClass = opposite_style
    'company email
    comp_email_label.ForeColor = System.Drawing.ColorTranslator.FromHtml(color)
    comp_email.CssClass = cssClass_style
    comp_email_lbl.CssClass = opposite_style
    comp_email_lbl.Text = comp_email.Text

    'company phone labels
    comp_phone_label.ForeColor = System.Drawing.ColorTranslator.FromHtml(color)
    comp_phone_type_label.ForeColor = System.Drawing.ColorTranslator.FromHtml(color)

    'comp phone 1
    cphone1.CssClass = cssClass_style
    cphone1_lbl.CssClass = opposite_style
    cphone1_lbl.Text = cphone1.Text

    cphone_type1.CssClass = cssClass_style
    cphone_type1_lbl.CssClass = opposite_style
    cphone_type1_lbl.Text = cphone_type1.Text

    'comp phone 2
    cphone2.CssClass = cssClass_style
    cphone2_lbl.CssClass = opposite_style
    cphone2_lbl.Text = cphone2.Text

    cphone_type2.CssClass = cssClass_style
    cphone_type2_lbl.CssClass = opposite_style
    cphone_type2_lbl.Text = cphone_type2.Text

    'comp phone 3
    cphone3.CssClass = cssClass_style
    cphone3_lbl.CssClass = opposite_style
    cphone3_lbl.Text = cphone3.Text

    cphone_type3.CssClass = cssClass_style
    cphone_type3_lbl.CssClass = opposite_style
    cphone_type3_lbl.Text = cphone_type3.Text
  End Sub

  Private Sub save_quick_entry_Click() Handles save_quick_entryLB.Click
    If Page.IsValid Then
      Dim clsCompany As New clsClient_Company
      Dim clsClient As New clsClient_Contact
      Dim clsNote As New clsLocal_Notes

      Dim startdate As String = ""
      Dim aTempTable As New DataTable
      Dim holdTable As New DataTable
      Dim filterTable As New DataTable

      'Building the Company Class.
      clsCompany.clicomp_name = comp_name.Text
      clsCompany.clicomp_address1 = comp_address.Text
      clsCompany.clicomp_city = comp_city.Text
      clsCompany.clicomp_state = comp_state.Text
      clsCompany.clicomp_zip_code = comp_zip.Text
      clsCompany.clicomp_email_address = comp_email.Text
      startdate = Now()
      clsCompany.clicomp_date_updated = startdate
      startdate = Year(startdate) & "-" & Month(startdate) & "-" & (Day(startdate)) & " " & FormatDateTime(startdate, 4) & ":" & Second(startdate)

      'The function will take care of replacing single quotes, it will take care of trimming the data so it fits 
      If aclsData_Temp.Insert_Client_Company(clsCompany) = True Then
        aTempTable = aclsData_Temp.Get_Insert_Client_Company(clsCompany.clicomp_name, startdate, "Y")
        If Not IsNothing(aTempTable) Then 'not nothing
          If aTempTable.Rows.Count > 0 Then
            'The company that just got inserted - this is its ID.
            clsCompany.clicomp_id = IIf(Not IsDBNull(aTempTable.Rows(0).Item("comp_id")), aTempTable.Rows(0).Item("comp_id"), 0)
            'let's insert the phone #'s for Companies
            Insert_Company_Phone_Numbers(clsCompany.clicomp_id)

            'Let's build the Contact Class Now.
            clsClient.clicontact_comp_id = clsCompany.clicomp_id
            clsClient.clicontact_date_updated = startdate
            clsClient.clicontact_email_address = Email.Text
            clsClient.clicontact_first_name = firstname.Text
            clsClient.clicontact_middle_initial = middle.Text
            clsClient.clicontact_last_name = lastname.Text
            clsClient.clicontact_title = contact_title.Text
            clsClient.clicontact_notes = contact_notes.Text

            'After creation of contact class
            'insert into the database
            If aclsData_Temp.Insert_Client_Contact(clsClient) = True Then
              aTempTable = aclsData_Temp.Get_Insert_Client_Contact(clsCompany.clicomp_id, startdate, "Y")
              If Not IsNothing(aTempTable) Then
                'grabbing the contact ID
                clsClient.clicontact_id = aTempTable.Rows(0).Item("contact_id")

                'This is going to check to see if we have an Aircraft ID from the page, meaning we're trying to add a reference.
                'We'll need both acSource and acID to do this.

                If AircraftRelationship Then
                  'This means we're going to be adding an Aircraft Relationship.
                  Dim aclsInsert_Client_Aircraft_Reference As New clsClient_Aircraft_Reference

                  aclsInsert_Client_Aircraft_Reference.cliacref_comp_id = clsClient.clicontact_comp_id
                  aclsInsert_Client_Aircraft_Reference.cliacref_contact_type = contactRelationship.SelectedValue
                  aclsInsert_Client_Aircraft_Reference.cliacref_contact_id = clsClient.clicontact_id
                  If AcSource = "JETNET" Then
                    aclsInsert_Client_Aircraft_Reference.cliacref_jetnet_ac_id = AcID
                    aclsInsert_Client_Aircraft_Reference.cliacref_cliac_id = 0
                  ElseIf AcSource = "CLIENT" Then
                    aclsInsert_Client_Aircraft_Reference.cliacref_jetnet_ac_id = 0
                    aclsInsert_Client_Aircraft_Reference.cliacref_cliac_id = AcID
                  End If

                  aclsInsert_Client_Aircraft_Reference.cliacref_operator_flag = ""
                  aclsInsert_Client_Aircraft_Reference.cliacref_owner_percentage = "0"
                  aclsInsert_Client_Aircraft_Reference.cliacref_contact_priority = IIf(IsNumeric(contactRelationshipPriority.SelectedValue), CInt(contactRelationshipPriority.SelectedValue), 0)
                  aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_expires = Now()
                  aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_purchased = Now()
                  aclsInsert_Client_Aircraft_Reference.cliacref_business_type = ""

                  If aclsData_Temp.Insert_Client_Aircraft_Reference(aclsInsert_Client_Aircraft_Reference) = True Then
                    'Relationship has been added.
                  End If
                End If

                'insert contact phone #s
                Insert_Contact_Phone_Numbers(clsClient.clicontact_id, clsCompany.clicomp_id)

                'check for note flag
                If enter_as_note.Checked = True Then
                  'Even though enter as note is checked (it's defaulted to checked) - go
                  'ahead and make sure that the actual note text isn't empty.
                  'If it isn't, we can save the note, if not we won't save the blank note.
                  If Not String.IsNullOrEmpty(contact_notes.Text) Then
                    'create note class
                    clsNote.lnote_client_contact_id = clsClient.clicontact_id
                    clsNote.lnote_client_comp_id = clsCompany.clicomp_id
                    clsNote.lnote_note = contact_notes.Text
                    clsNote.lnote_status = "A"
                    clsNote.lnote_notecat_key = 23

                    'Saving the jetnet ac ID
                    If IsNumeric(jetnet_ac.Text) Then
                      clsNote.lnote_jetnet_ac_id = jetnet_ac.Text
                    End If

                    'Saving the client ac ID
                    If IsNumeric(client_ac.Text) Then
                      clsNote.lnote_client_ac_id = client_ac.Text
                    End If

                    'Saving the client model ID
                    If IsNumeric(client_mod.Text) Then
                      clsNote.lnote_client_amod_id = client_mod.Text
                    End If

                    'Saving the jetnet model ID
                    If IsNumeric(jetnet_mod.Text) Then
                      clsNote.lnote_jetnet_amod_id = jetnet_mod.Text
                    End If


                    holdTable = aclsData_Temp.Get_Client_NOT_Note_Document_Category("Y")
                    If Not IsNothing(holdTable) Then
                      If holdTable.Rows.Count > 0 Then
                        filterTable = holdTable.Clone

                        ' create a datarow to filter in the rows by category
                        Dim afileterd As DataRow() = holdTable.Select("notecat_name = 'General'", "")
                        ' create another datarow to import the filtered info
                        For Each atmpDataRow As DataRow In afileterd
                          filterTable.ImportRow(atmpDataRow)
                        Next

                        If Not IsNothing(filterTable) Then
                          If filterTable.Rows.Count > 0 Then
                            clsNote.lnote_notecat_key = filterTable.Rows(0).Item("notecat_key")
                          End If
                        End If
                      End If
                    End If

                    clsNote.lnote_entry_date = Now() ' DB requires some value
                    clsNote.lnote_action_date = Now() ' DB requires some value
                    clsNote.lnote_clipri_ID = 1
                    clsNote.lnote_user_login = Session.Item("localUser").crmLocalUserID ' DB requires a string value greater than 0
                    clsNote.lnote_user_name = Left(Session.Item("localUser").crmLocalUserFirstName & " " & Session.Item("localUser").crmLocalUserLastName, 15)
                    clsNote.lnote_user_id = Session.Item("localUser").crmLocalUserID ' DB requires a string value greater than 0
                    aclsData_Temp.Insert_Note(clsNote)

                    'Adding prospect if it was checked
                    If Session.Item("localSubscription").crmAerodexFlag = False Then
                      If attach_prospect_aircraft.Checked Then
                        clsNote.lnote_status = "B"
                        clsNote.lnote_opportunity_status = "A"
                        aclsData_Temp.Insert_Note(clsNote)
                      End If
                    End If
                  End If
                End If

                If Trim(Request("from")) = "homePage" Then
                  If AircraftRelationship = False Then
                    'Refreshing the page to the correct details page
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.location.href = '/DisplayContactDetail.aspx?conid=" & clsClient.clicontact_id & "&compid=" & clsCompany.clicomp_id & "&source=CLIENT';", True)
                  Else
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.location.href='/DisplayAircraftDetail.aspx?acid=" & AcID & "&source=" & AcSource & "';", True)
                  End If
                Else

                  If AircraftRelationship = False Then
                    'Refreshing the page to the correct details page
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location = 'details.aspx?=contact_ID=" & clsClient.clicontact_id & "&comp_ID=" & clsCompany.clicomp_id & "&type=1&source=CLIENT';", True)
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
                  Else
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.opener.location='details.aspx?ac_ID=" & AcID & "&type=3&source=" & AcSource & "';", True)
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "CloseWindowParent", "window.opener.close();", True)
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
                  End If
                End If
              Else
                If aclsData_Temp.class_error <> "" Then
                  LogError("ContactQuickEntry.ascx.vb - SaveQuickEntry Click - " & aclsData_Temp.class_error)
                End If
                displayError()
              End If
            Else
              If aclsData_Temp.class_error <> "" Then
                LogError("ContactQuickEntry.ascx.vb - SaveQuickEntry Click - " & aclsData_Temp.class_error)
              End If
              displayError()
            End If


          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            LogError("ContactQuickEntry.ascx.vb - Save Click() - " & aclsData_Temp.class_error)
          End If
          displayError()
        End If
      End If
    End If
  End Sub
  Private Sub Insert_Company_Phone_Numbers(ByVal comp_id As Long)
    Dim clsPhone As New clsClient_Phone_Numbers
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'Insert First set of Phone #s
    If cphone1.Text <> "" And cphone_type1.SelectedValue <> "" Then
      clsPhone.clipnum_comp_id = comp_id
      clsPhone.clipnum_contact_id = 0
      clsPhone.clipnum_number = cphone1.Text
      clsPhone.clipnum_type = cphone_type1.SelectedValue
      aclsData_Temp.Insert_Client_PhoneNumbers(clsPhone)
    End If
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'Insert Second set of Phone #s
    clsPhone = New clsClient_Phone_Numbers
    If cphone2.Text <> "" And cphone_type2.SelectedValue <> "" Then
      clsPhone.clipnum_comp_id = comp_id
      clsPhone.clipnum_contact_id = 0
      clsPhone.clipnum_number = cphone2.Text
      clsPhone.clipnum_type = cphone_type2.SelectedValue
      aclsData_Temp.Insert_Client_PhoneNumbers(clsPhone)
    End If
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'Insert third set of Phone #s
    clsPhone = New clsClient_Phone_Numbers
    If cphone3.Text <> "" And cphone_type3.SelectedValue <> "" Then
      clsPhone.clipnum_comp_id = comp_id
      clsPhone.clipnum_contact_id = 0
      clsPhone.clipnum_number = cphone3.Text
      clsPhone.clipnum_type = cphone_type3.SelectedValue
      aclsData_Temp.Insert_Client_PhoneNumbers(clsPhone)
    End If
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
  End Sub
  Private Sub Insert_Contact_Phone_Numbers(ByVal contact_id As Long, ByVal comp_id As Long)
    Dim clsPhone As New clsClient_Phone_Numbers
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'Insert First set of Phone #s
    If phone1.Text <> "" And type1.SelectedValue <> "" Then
      clsPhone.clipnum_comp_id = comp_id
      clsPhone.clipnum_contact_id = contact_id
      clsPhone.clipnum_number = phone1.Text
      clsPhone.clipnum_type = type1.SelectedValue
      aclsData_Temp.Insert_Client_PhoneNumbers(clsPhone)
    End If
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'Insert Second set of Phone #s
    clsPhone = New clsClient_Phone_Numbers
    If phone2.Text <> "" And type2.SelectedValue <> "" Then
      clsPhone.clipnum_comp_id = comp_id
      clsPhone.clipnum_contact_id = contact_id
      clsPhone.clipnum_number = phone2.Text
      clsPhone.clipnum_type = type2.SelectedValue
      aclsData_Temp.Insert_Client_PhoneNumbers(clsPhone)
    End If
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'Insert third set of Phone #s
    clsPhone = New clsClient_Phone_Numbers
    If phone3.Text <> "" And type3.SelectedValue <> "" Then
      clsPhone.clipnum_comp_id = comp_id
      clsPhone.clipnum_contact_id = contact_id
      clsPhone.clipnum_number = phone3.Text
      clsPhone.clipnum_type = type3.SelectedValue
      aclsData_Temp.Insert_Client_PhoneNumbers(clsPhone)
    End If
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
  End Sub

  'Toggles the aircraft attach note panel visibility when deciding whether to attach the contact note to an aircraft as well.
  Private Sub attach_note_to_aircraft_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles attach_note_to_aircraft.CheckedChanged

    If attach_note_to_aircraft.Checked Then
      attach_note_to_aircraft_panel.Visible = True
      'We need to check on displaying the prospects (since that's default)
      Fill_Up_Aircraft_Prospects()
    ElseIf attach_note_to_aircraft.Checked = False Then
      attach_note_to_aircraft_panel.Visible = False


      'We should clear these out to ensure that no left over ac gets picked.
      jetnet_ac.Text = "0"
      client_ac.Text = "0"
      jetnet_mod.Text = "0"
      client_mod.Text = "0"
      attach_prospect_aircraft.Visible = False
      attach_prospect_aircraft.Checked = False

      'Default dropdown to nothing
      aircraft_note.Items.Clear()
      aircraft_note.Items.Add(New ListItem("Please search for an Aircraft", "0||0"))

    End If

  End Sub

  '-----------------------------------------------------Public Functions--------------------------------------------------------
  Public Function displayError()
    '------------------------------Function that Creates a Javascript Error if the data manager class errors-----------
    displayError = ""
    If aclsData_Temp.class_error <> "" Then
      System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Error", "alert('" & Replace(aclsData_Temp.class_error, "'", " \'") & "');", True)
    End If
    aclsData_Temp.class_error = ""
  End Function

  Public Sub LogError(ByVal ex As String)
    aclsData_Temp.LogError(Application.Item("crmClientSiteData").crmClientHostName, ex, DateTime.Now.ToString())
  End Sub

  'Toggles the visibility of the aircraft search box, depending on whether they want to view prospects or search for aircraft
  Private Sub aircraft_search_checkbox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles aircraft_search_checkbox.CheckedChanged
    'We should clear these out to ensure that no left over ac gets picked.
    jetnet_ac.Text = "0"
    client_ac.Text = "0"
    jetnet_mod.Text = "0"
    client_mod.Text = "0"
    attach_prospect_aircraft.Visible = False
    attach_prospect_aircraft.Checked = False

    If aircraft_search_checkbox.Checked Then
      aircraft_display_search_panel.Visible = True
      aircraft_information.Text = ""
      aircraft_display_information_panel.Visible = False

      'Default dropdown to nothing
      aircraft_note.Items.Clear()
      aircraft_note.Items.Add(New ListItem("Please search for an Aircraft", "0||0"))
    Else
      aircraft_display_search_panel.Visible = False
    End If
  End Sub

  'Function that fills up Aircraft Prospects from list
  Private Sub Fill_Up_Aircraft_Prospects()
    If attach_note_to_aircraft_panel.Visible = True Then
      If aircraft_prospects_checkbox.Checked = True Then
        'Default dropdown to nothing/clear
        aircraft_note.Items.Clear()
        aircraft_note.Items.Add(New ListItem("Please select an Aircraft Prospect", "0||0"))
        Dim acTable As DataTable = aclsData_Temp.BuildACProspectList("")
        If Not IsNothing(acTable) Then
          For Each r As DataRow In acTable.Rows
            Dim ACString As String = ""
            ACString = r("amod_make_name") & " " & r("amod_model_name")
            ACString += IIf(Not IsDBNull(r("ac_ser_nbr")), " Ser #:" & r("ac_ser_nbr") & " ", "")
            ACString += IIf(Not IsDBNull(r("ac_reg_nbr")), "Reg #" & r("ac_reg_nbr"), "")

            If r("lnote_client_ac_id") > 0 Then
              aircraft_note.Items.Add(New ListItem(ACString, r("lnote_client_ac_id") & "|CLIENT|" & r("amod_id")))
            Else
              aircraft_note.Items.Add(New ListItem(ACString, r("lnote_jetnet_ac_id") & "|JETNET|" & r("amod_id")))
            End If

          Next
        End If
      End If
    End If
  End Sub


  Public Sub SearchAircraftButton()
    Dim TemporaryTable As New DataTable
    Dim SQL_Aircraft_Make_Model As String
    Try
      If Page.IsPostBack Then
        'Just a small check to make sure that the serial # isn't blank even though there's client validation
        If serial_number_text.Text <> "" Then


          'Set up the search parameters.
          SQL_Aircraft_Make_Model = "%" & clsGeneral.clsGeneral.StripChars(serial_number_text.Text, False) & "%"
          'Run through the datalayer and bring the table back.
          TemporaryTable = aclsData_Temp.AC_Search_New("AMOD_MAKE_NAME ASC, AMOD_MODEL_NAME ASC, AC_SER_NBR_SORT ASC", "JC", "all", "", "", SQL_Aircraft_Make_Model, "", "", "", "", "", "", "", "", "", Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmJets_Flag, Session.Item("localSubscription").crmExecutive_Flag, Session.Item("localSubscription").crmTurboprops, Session.Item("localSubscription").crmCommercial_Flag, Session.Item("localSubscription").crmAerodexFlag, "", "", "", "", "5", "2", "", "", "", "", "", "", "", "", "", "", "", "", 0, False)

          'Clear dropdown in anticipation of data fill.
          aircraft_note.Items.Clear()
          aircraft_note.Items.Add(New ListItem("Please Select an Aircraft", "0||0"))

          'Let's default clear these just so there's nothing hanging left over.
          jetnet_ac.Text = "0"
          client_ac.Text = "0"
          jetnet_mod.Text = "0"
          client_mod.Text = "0"
          attach_prospect_aircraft.Visible = False
          attach_prospect_aircraft.Checked = False

          aircraft_information.Text = ""
          aircraft_display_information_panel.Visible = False

          If Not IsNothing(TemporaryTable) Then
            If TemporaryTable.Rows.Count > 0 Then
              For Each r As DataRow In TemporaryTable.Rows
                Dim ser As String = ""
                'This parses the serial number by removing the html in the datatable.
                'Since this is the same datatable that's returned in the aircraft search, the link is already built in
                'so we'll need to remove it. 
                If Not IsDBNull(r("ac_ser_nbr")) Then
                  ser = Regex.Replace(r("ac_ser_nbr"), "<.*?>", "")
                ElseIf Not IsDBNull(r("other_ac_ser_nbr")) Then
                  ser = Regex.Replace(r("other_ac_ser_nbr"), "<.*?>", "")
                End If

                aircraft_note.Items.Add(New ListItem(IIf(Not IsDBNull(r("ac_year_mfr")), r("ac_year_mfr"), r("other_ac_year_mfr")) & " " & r("amod_make_name") & " " & r("amod_model_name") & " Ser #:" & ser & " Reg#:" & IIf(Not IsDBNull(r("ac_reg_nbr")), r("ac_reg_nbr"), r("other_ac_reg_nbr")) & " (" & r("source") & " record)", r("ac_id") & "|" & r("source") & "|" & r("ac_amod_id")))
              Next
            Else
              'If there are no results, display message to user.
              aircraft_note.Items.Clear()
              aircraft_note.Items.Add(New ListItem("No Search Results", "0||0"))
            End If
          Else
            'If there is an error, display no results, clear previous dropdown.
            aircraft_note.Items.Clear()
            aircraft_note.Items.Add(New ListItem("No Search Results", "0||0"))
            'Record data error.
            If aclsData_Temp.class_error <> "" Then
              Error_String = aclsData_Temp.class_error
              clsGeneral.clsGeneral.LogError("ContactQuickEntry.ascx.vb - SearchAircraftButton Data() - " & Error_String, aclsData_Temp)
            End If
          End If
        End If
      End If
    Catch ex As Exception
      'If there are no results, display message to user.
      aircraft_note.Items.Clear()
      aircraft_note.Items.Add(New ListItem("No Search Results", "0||0"))
      'Log Error.
      Error_String = "ContactQuickEntry.ascx.vb -SearchAircraftButton() Function - " & ex.Message
      LogError(Error_String)
    End Try
  End Sub


  'This is what happens if the aircraft note dropdown selected index is changed.
  Private Sub Aircraft_Note_Changed() Handles aircraft_note.SelectedIndexChanged
    Try
      Dim AircraftInformation As New DataTable
      Dim ModelInformation As New DataTable
      Dim Aircraft_Model As String = ""

      Dim Aircraft_Data As New clsClient_Aircraft
      Dim typed() As String

      'Let's default clear these:
      jetnet_ac.Text = "0"
      client_ac.Text = "0"
      jetnet_mod.Text = "0"
      client_mod.Text = "0"
      attach_prospect_aircraft.Visible = False
      attach_prospect_aircraft.Checked = False
      aircraft_display_information_panel.Visible = True
      aircraft_information.Text = ""
      aircraft_display_search_panel.Visible = False
      aircraft_search_checkbox.Checked = False

      If aircraft_note.SelectedValue = "0" Or aircraft_note.SelectedValue = "0||0" Then
        jetnet_ac.Text = "0"
        client_ac.Text = "0"
        jetnet_mod.Text = "0"
        client_mod.Text = "0"
      ElseIf aircraft_note.SelectedValue <> "0" Then
        'Check for aerodex then go ahead and toggle the prospect checkbox on if they have an aircraft selected.
        If Session.Item("localSubscription").crmAerodexFlag = False Then
          attach_prospect_aircraft.Visible = True
        End If

        typed = Split(aircraft_note.SelectedValue, "|")
        If UCase(typed(1)) = "JETNET" Then
          jetnet_ac.Text = typed(0)
          jetnet_mod.Text = typed(2)

          'Fill up the aircraft Datatable.
          AircraftInformation = aclsData_Temp.Get_Client_Aircraft_JETNET_AC(typed(0))


          If Not IsNothing(AircraftInformation) Then
            If AircraftInformation.Rows.Count > 0 Then
              client_ac.Text = AircraftInformation.Rows(0).Item("cliaircraft_id")

              'Aircraft_Model = (aTempTable.Rows(0).Item("cliamod_make_name") & " " & aTempTable.Rows(0).Item("cliamod_model_name"))
              Aircraft_Data = clsGeneral.clsGeneral.Create_Aircraft_Class(AircraftInformation, "cliaircraft")
              Aircraft_Data.cliaircraft_id = AircraftInformation.Rows(0).Item("cliaircraft_id")

              aircraft_information.Text = "<br />" & clsGeneral.clsGeneral.Build_Aircraft_Display(Aircraft_Data, True, False, False)
            Else 'unfortunately have to repoll the database for the ac display information because it still exists evne if it's not a client. 
              clsGeneral.clsGeneral.Display_Jetnet_Aircraft_Label(aircraft_information, Aircraft_Data, typed(0), aclsData_Temp, jetnet_mod)
              client_ac.Text = "0" ' clear this
            End If 'just because there's no ac doesn't mean no model.


            'This grabs the client model information from the database by checking based on the jetnet amod ID
            ModelInformation = aclsData_Temp.Get_Clients_Aircraft_Model_ByJETNETAmod(typed(2))
            If Not IsNothing(ModelInformation) Then
              If ModelInformation.Rows.Count > 0 Then
                'Sets the box.
                client_mod.Text = ModelInformation.Rows(0).Item("cliamod_id")
              End If
            Else 'There is no matching model
              client_mod.Text = "0"
            End If
          Else
            'Aircraft information error.
            If aclsData_Temp.class_error <> "" Then
              Error_String = aclsData_Temp.class_error
              clsGeneral.clsGeneral.LogError("ContactQuickEntry.ascx.vb - Aircraft_Note SelectedIndexChanged J AircraftInformation Data() - " & Error_String, aclsData_Temp)
            End If
          End If
        Else
          client_ac.Text = typed(0)

          'Search the client AC to bring back the jetnet ac ID
          AircraftInformation = aclsData_Temp.Get_Clients_Aircraft(typed(0))
          If Not IsNothing(AircraftInformation) Then
            If AircraftInformation.Rows.Count > 0 Then
              jetnet_ac.Text = AircraftInformation.Rows(0).Item("cliaircraft_jetnet_ac_id")

              'get the jetnet_model if there is any:
              If IsNumeric(typed(2)) Then
                'Set the client model ID
                client_mod.Text = typed(2)
                'Check the database for the client model's jetnet ID.
                ModelInformation = aclsData_Temp.Get_Clients_Aircraft_Model_amodID(typed(2))
                If Not IsNothing(ModelInformation) Then
                  If ModelInformation.Rows.Count > 0 Then
                    'Set the jetnet model ID.
                    jetnet_mod.Text = ModelInformation.Rows(0).Item("cliamod_jetnet_amod_id")
                  End If
                End If
              End If

              'Display the aircraft information.
              clsGeneral.clsGeneral.Display_Jetnet_Aircraft_Label(aircraft_information, Aircraft_Data, AircraftInformation.Rows(0).Item("cliaircraft_jetnet_ac_id"), aclsData_Temp, jetnet_mod)

              'If there's no jetnet aircraft ID
              If AircraftInformation.Rows(0).Item("cliaircraft_jetnet_ac_id") = 0 Then
                'we clear the jetnet ac text
                jetnet_ac.Text = "0"
                'But we still have to display the client information 
                Aircraft_Model = (AircraftInformation.Rows(0).Item("cliamod_make_name") & " " & AircraftInformation.Rows(0).Item("cliamod_model_name"))
                Aircraft_Data = clsGeneral.clsGeneral.Create_Aircraft_Class(AircraftInformation, "cliaircraft")
                Aircraft_Data.cliaircraft_id = AircraftInformation.Rows(0).Item("cliaircraft_id")

                aircraft_information.Text = Aircraft_Model & "<br />" & clsGeneral.clsGeneral.Build_Aircraft_Display(Aircraft_Data, True, False, False)

              End If
            End If
          Else
            'Aircraft information error.
            If aclsData_Temp.class_error <> "" Then
              Error_String = aclsData_Temp.class_error
              clsGeneral.clsGeneral.LogError("ContactQuickEntry.ascx.vb - Aircraft_Note SelectedIndexChanged C AircraftInformation Data() - " & Error_String, aclsData_Temp)
            End If
          End If

        End If

      End If


    Catch ex As Exception
      Error_String = "ContactQuickEntry.ascx.vb - aircraft_name_changed() - " & ex.Message
      clsGeneral.clsGeneral.LogError(Error_String, aclsData_Temp)
    End Try
  End Sub

  'This is what happens when they click the aircraft prospects checkbox.
  Private Sub aircraft_prospects_checkbox_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles aircraft_prospects_checkbox.CheckedChanged
    'We should clear these out to ensure that no left over ac gets picked.
    jetnet_ac.Text = "0"
    client_ac.Text = "0"
    jetnet_mod.Text = "0"
    client_mod.Text = "0"
    attach_prospect_aircraft.Visible = False
    attach_prospect_aircraft.Checked = False

    If aircraft_prospects_checkbox.Checked Then
      Fill_Up_Aircraft_Prospects()
    End If
  End Sub
End Class