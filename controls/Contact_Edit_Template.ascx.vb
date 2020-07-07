Imports System.IO
Partial Public Class Contact_Edit_Template
  Inherits System.Web.UI.UserControl
  Public Event Fill_Phone(ByVal type1 As String, ByVal type2 As String, ByVal type3 As String, ByVal type4 As String, ByVal type5 As String, ByVal type6 As String, ByVal con As Control)
  Public Event loop_contacts(ByVal idnum_new As Integer, ByVal jetnet_id As Integer, ByVal contact_id As Integer, ByVal skip As Boolean, ByVal inactive As Boolean)
  Public Event get_insert_ac(ByVal jetnet_id As Integer, ByVal idnum_new As Integer, ByVal get_previous_ac As Boolean, ByVal insert_ac As Boolean, ByVal new_client_ac As Integer)
  Public aclsData_Temp As New clsData_Manager_SQL
  Public aTempTable, aTempTable2 As New DataTable 'Data Tables used
  Dim error_string As String = ""

#Region "Page Events"
  Public Sub TextValidate(ByVal source As Object, ByVal args As ServerValidateEventArgs)
    If Not IsNothing(source.controltovalidate) Then
      Dim c As TextBox = FindControl(source.controltovalidate)
      Dim q As String = Replace(source.controltovalidate, "phone", "type")
      Dim d As DropDownList = FindControl(q)
      If c.Text <> "" Then
        If d.SelectedValue = "" Then
          args.IsValid = False
        Else
          args.IsValid = True
        End If
      End If
    End If
  End Sub
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Me.Visible Then
      Try

        If Session.Item("crmUserLogon") <> True Then
          Response.Redirect("Default.aspx", False)
        End If
        ' setup the connection info
        aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
        aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase") 'CApplication.Item("crmJetnetDatabase")
        aclsData_Temp.class_error = ""


        '---------------------------------------------End Database Connection Stuff---------------------------------------------
        If Not IsNothing(Request.Item("comp_ID")) Then
          If IsNumeric(Request.Item("comp_ID")) Then
            Session.Item("ListingID") = Request.Item("comp_ID")
          End If
        End If

        If Not IsNothing(Request.Item("contact_ID")) Then
          If IsNumeric(Request.Item("contact_ID")) Then
            Session.Item("ContactID") = Request.Item("contact_ID")
          End If
        End If

        If Not IsNothing(Request.Item("source")) Then
          If Not IsNumeric(Request.Item("source")) Then
            Session.Item("ListingSource") = Request.Item("source")
          End If
        End If

        If Session.Item("isMobile") = True Then
          TextBox1.Width = 255
          If Not IsNothing(Request.Item("contact_id")) Then
            If Not String.IsNullOrEmpty(Request.Item("contact_id").ToString) Then
              Session.Item("ContactID") = Request.Item("contact_id").Trim
            End If
          End If
          mobile_close.Text = "<a href='mobile_details.aspx?type=1&comp_ID=" & Session.Item("ListingID") & "&contact_ID=" & Session.Item("ContactID") & "'><img src=""images/cancel.gif"" alt=""Cancel"" border=""0""/></a>"
        End If

        If Not Page.IsPostBack Then
          fill_contact_boxes()
        End If

        RaiseEvent Fill_Phone(type1.ID, type2.ID, type3.ID, type4.ID, type5.ID, type6.ID, Me)


        If Trim(Request("action")) = "new" Then
          edit_cont_tag.Text = "<h2 class=""mainHeading remove_margin""><strong>Contact</strong> Add</h2>"
          'add_to_Folder()
          If Not Session.Item("ContactID") Is Nothing Then
            If Not IsNothing(Request.Item("contact_id")) Then
              If Not String.IsNullOrEmpty(Request.Item("contact_id").ToString) Then
                If Trim(Request("createClient")) = "true" Then
                  fill_edit_data()
                End If
              End If
            End If
          End If
        ElseIf Trim(Request("action")) = "remove" Then
          If Not IsNothing(Request.Item("contact_id")) Then
            If Not String.IsNullOrEmpty(Request.Item("contact_id").ToString) Then
              If IsNumeric(Request.Item("contact_id").ToString) Then
                Remove(Request.Item("contact_id"))
              End If
            End If
          End If
          Else
            ' Set_Folder_Editing("comp")
            If Not Session.Item("ContactID") Is Nothing Then
              If Not Page.IsPostBack Then
                fill_edit_data()
              End If
            End If
          End If

          ''If Session.Item("ListingSource") = "JETNET" Then

          ''ElseIf Trim(Request("action")) <> "new" Then
          ''    add_folder_cbo.Visible = False
          ''End If
      Catch ex As Exception
        error_string = "Contact_Edit_Template.ascx.vb - Page Load() " & ex.Message
        LogError(error_string)
      End Try
    End If
  End Sub

#End Region

#Region "Fill Contact/Edit Data"
  Public Sub fill_contact_boxes()
    Try
      sirname.Items.Add(New ListItem("None", ""))
      sirname.Items.Add(New ListItem("Ambas.", "Ambas."))
      sirname.Items.Add(New ListItem("Bishop", "Bishop"))
      sirname.Items.Add(New ListItem("Brig", "Brig"))
      sirname.Items.Add(New ListItem("Capt.", "Capt."))
      sirname.Items.Add(New ListItem("Cmdr.", "Cmdr."))
      sirname.Items.Add(New ListItem("Cmdt.", "Cmdt."))
      sirname.Items.Add(New ListItem("Col.", "Col."))
      sirname.Items.Add(New ListItem("Comm.", "Comm."))
      sirname.Items.Add(New ListItem("Corporal", "Corporal"))
      sirname.Items.Add(New ListItem("Deputy", "Deputy"))
      sirname.Items.Add(New ListItem("Dr.", "Dr."))
      sirname.Items.Add(New ListItem("Duke", "Duke"))
      sirname.Items.Add(New ListItem("Eng.", "Eng."))
      sirname.Items.Add(New ListItem("Gen.", "Gen."))
      sirname.Items.Add(New ListItem("HE", "HE"))
      sirname.Items.Add(New ListItem("HE Sheikh", "HE Sheikh"))
      sirname.Items.Add(New ListItem("HH Sheikh", "HH Sheikh"))
      sirname.Items.Add(New ListItem("Hon.", "Hon."))
      sirname.Items.Add(New ListItem("HRH.", "HRH."))

      sirname.Items.Add(New ListItem("Ing.", "Ing."))
      sirname.Items.Add(New ListItem("Jdge", "Jdge"))

      sirname.Items.Add(New ListItem("King", "King"))
      sirname.Items.Add(New ListItem("Lic.", "Lic."))
      sirname.Items.Add(New ListItem("Lord", "Lord"))
      sirname.Items.Add(New ListItem("Lt.", "Lt."))
      sirname.Items.Add(New ListItem("LtCol.", "LtCol."))
      sirname.Items.Add(New ListItem("LtGen.", "LtGen."))
      sirname.Items.Add(New ListItem("Ltn.", "Ltn."))
      sirname.Items.Add(New ListItem("Major", "Major"))
      sirname.Items.Add(New ListItem("Mr.", "Mr."))

      sirname.Items.Add(New ListItem("Mrs.", "Mrs."))
      sirname.Items.Add(New ListItem("Ms.", "Ms."))
      sirname.Items.Add(New ListItem("Pastor", "Pastor"))
      sirname.Items.Add(New ListItem("Pres", "Pres"))

      sirname.Items.Add(New ListItem("Prince", "Prince"))
      sirname.Items.Add(New ListItem("Prof.", "Prof."))
      sirname.Items.Add(New ListItem("Rear Admr.", "Rear Admr."))
      sirname.Items.Add(New ListItem("Reverend", "Reverend"))

      sirname.Items.Add(New ListItem("Senator", "Senator"))

      sirname.Items.Add(New ListItem("Sgt.", "Sgt."))
      sirname.Items.Add(New ListItem("Sheikh", "Sheikh"))
      sirname.Items.Add(New ListItem("Sir", "Sir"))
      sirname.Items.Add(New ListItem("Sult.", "Sult."))

      suffix.Items.Add(New ListItem("None", ""))
      suffix.Items.Add(New ListItem("CFA", "CFA"))

      suffix.Items.Add(New ListItem("Esq.", "Esq."))
      suffix.Items.Add(New ListItem("II", "II"))
      suffix.Items.Add(New ListItem("III", "III"))
      suffix.Items.Add(New ListItem("VI.", "VI"))
      suffix.Items.Add(New ListItem("Jr.", "Jr."))
      suffix.Items.Add(New ListItem("Sr.", "Sr."))
      suffix.Items.Add(New ListItem("V.", "V"))
    Catch ex As Exception
      error_string = "Company_Edit_Template.ascx.vb - fill_contact_boxes() " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Public Function fill_edit_data()

    Dim source As String = Session.Item("ListingSource")
    Dim ID As Integer = Session.Item("ContactID")

    '--------------------------------------Fill the Contact/Jobseeker Data-------------------------------------------
    Try
      Dim comp_id_int As Integer = 0
      If ID <> 0 And source <> "" Then
        aTempTable = aclsData_Temp.GetContacts_Details(ID, source)
      Else
        aTempTable = New DataTable
      End If

      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then

          For Each r As DataRow In aTempTable.Rows

            If source = "CLIENT" Then

              company_id.Text = r("contact_comp_id")
              jetnet_contact_id.Text = r("contact_jetnet_contact_id")

              If Not IsDBNull(r("clicontact_user_id")) Then
                aTempTable = aclsData_Temp.Get_Client_User(CInt(r("clicontact_user_id")))
                If Not IsNothing(aTempTable) Then
                  If aTempTable.Rows.Count > 0 Then
                    For Each q As DataRow In aTempTable.Rows
                      update_text.Text = "Last Updated: " & r("contact_action_date") & "     By: " & q("cliuser_first_name") & " " & q("cliuser_last_name")
                    Next
                  End If
                Else
                  If aclsData_Temp.class_error <> "" Then
                    error_string = aclsData_Temp.class_error
                    LogError("Company_Edit_Template.ascx.vb - fill_edit_data() - " & error_string)
                  End If
                  display_error()
                End If
              End If
              Dim status As String = CStr(IIf(Not IsDBNull(r("clicontact_status")), r("clicontact_status"), ""))
              If status = "Y" Then
                contact_active.Selected = True
              Else
                contact_inactive.Selected = True
              End If
            End If

            sirname.SelectedValue = CStr(IIf(Not IsDBNull(r("contact_sirname")), r("contact_sirname"), ""))
            firstname.Text = CStr(IIf(Not IsDBNull(r("contact_first_name")), r("contact_first_name"), ""))
            middle.Text = CStr(IIf(Not IsDBNull(r("contact_middle_initial")), r("contact_middle_initial"), ""))
            lastname.Text = CStr(IIf(Not IsDBNull(r("contact_last_name")), r("contact_last_name"), ""))

            edit_cont_tag.Text = "<h2 class=""mainHeading remove_margin""><strong>" & firstname.Text & " " & middle.Text & " " & lastname.Text & "</strong>" & IIf(Trim(Request("createClient")) = "true", " CREATE", " EDIT") & "</h2>"

            suffix.SelectedValue = CStr(IIf(Not IsDBNull(r("contact_suffix")), r("contact_suffix"), ""))
            contact_title.Text = CStr(IIf(Not IsDBNull(r("contact_title")), r("contact_title"), ""))
            Email.Text = CStr(IIf(Not IsDBNull(r("contact_email_address")), r("contact_email_address"), ""))
            comp_id.Text = CStr(IIf(Not IsDBNull(r("contact_comp_id")), r("contact_comp_id"), ""))
            comp_id_int = r("contact_comp_id")

            If source = "CLIENT" Then
              pref.Text = CStr(IIf(Not IsDBNull(r("contact_preferred_name")), r("contact_preferred_name"), ""))
              TextBox1.Text = CStr(IIf(Not IsDBNull(r("contact_notes")), r("contact_notes"), ""))

              If Not IsDBNull(r("contact_email_list")) Then
                If r("contact_email_list") = "Y" Then
                  CheckBox1.Checked = True
                End If
              End If
            End If
            sirname.TabIndex = 0
          Next
          '-----------------------------------Fill the Contact Phone Numbers--------------------------------------------
          aTempTable = aclsData_Temp.GetPhoneNumbers(comp_id_int, ID, source, 0)
          '' check the state of the DataTable
          If Not IsNothing(aTempTable) Then
            If aTempTable.Rows.Count > 0 Then
              ' set it to the datagrid 
              Dim x As Integer = 1
              For Each q As DataRow In aTempTable.Rows
                If x = 1 Then
                  type1.SelectedValue = Trim(q("pnum_type"))
                  phone1.Text = q("pnum_number")
                ElseIf x = 2 Then
                  type2.SelectedValue = q("pnum_type")
                  phone2.Text = q("pnum_number")
                ElseIf x = 3 Then
                  type3.SelectedValue = q("pnum_type")
                  phone3.Text = q("pnum_number")
                ElseIf x = 4 Then
                  type4.SelectedValue = q("pnum_type")
                  phone4.Text = q("pnum_number")
                ElseIf x = 5 Then
                  type5.SelectedValue = q("pnum_type")
                  phone5.Text = q("pnum_number")
                ElseIf x = 6 Then
                  type6.SelectedValue = q("pnum_type")
                  phone6.Text = q("pnum_number")
                End If
                x = x + 1
              Next
            End If
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = aclsData_Temp.class_error
              LogError("Company_Edit_Template.ascx.vb - fill_edit_data() - " & error_string)
            End If
            display_error()
          End If
        Else
          comp_id.Text = Session.Item("ListingID")
          comp_id.ReadOnly = True
          edit_cont_tag.Text = "<h2 class=""mainHeading remove_margin""><strong>CONTACT</strong> ADD</h2>"
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("Company_Edit_Template.ascx.vb - fill_edit_data() - " & error_string)
        End If
        display_error()
      End If
    Catch ex As Exception
      error_string = "Company_Edit_Template.ascx.vb - fill_edit_data() " & ex.Message
      LogError(error_string)
    End Try
    fill_edit_data = ""
  End Function
#End Region
#Region "Update/Insert Contact Functions/Phone Numbers"

  Private Sub updateButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles updateButton.Click
    Try
      If Page.IsValid Then
        aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
        aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase") 'CApplication.Item("crmJetnetDatabase")
        Dim idnum As Integer
        If Not Session.Item("ContactID") Is Nothing Then
          idnum = Session.Item("ContactID")
        Else
          idnum = 0
        End If

        Dim source As String = Session.Item("ListingSource")
        '        'Job seeker, Contact. 
        '        '1.) First check and see if this is a client record or a contact record.
        '        '2.) If client record, update it. Remove phone numbers, add new phone numbers.
        '        '3.) If jetnet record, poll the contact's company information.
        '        '4.) Insert the contact's company information.
        '        '5.) Insert the contact's company phone information.
        '        '6.) Insert the contact information.
        '        '7.) Insert the OTHER company contacts information besides the person you just added.
        '        '8.) Insert the contacts phone numbers.
        '        '9.) Insert the aircraft references. 

        If Trim(Request("action")) <> "new" Then
          If idnum <> 0 Then
            Select Case source  '1.) First check and see if this is a client record or a contact record.
              Case "CLIENT" '2.) If client record, update it. Remove phone numbers, add new phone numbers.
                Update_Contact(idnum, True, True, True)
              Case "JETNET" 'If jetnet record, poll the contact's company information.
                Insert_Contact(idnum, True, True, True, True, True, True)
            End Select
          End If
        Else
          'new contact
          insert_only_contact()
          Session("new_contact") = ""
        End If

        If Session.Item("isMobile") = True Then
          Response.Redirect("Mobile_Details.aspx?type=" & Session.Item("Listing") & "&comp_id=" & Session.Item("ListingID") & "&&contact_ID=" & Session.Item("ContactID") & "&source=" & Session.Item("ListingSource") & "&edited=company", False)
        End If
        If Trim(Request("from")) = "contactDetails" Then
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_WindowNow", "window.opener.location.reload();", True)
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
        Else
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location = 'details.aspx';", True)
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
        End If
      End If
    Catch ex As Exception
      error_string = "Contact_Edit_Template.ascx.vb - Update_Click() " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Function Insert_Contact(ByVal jetnet_id As Integer, ByVal insert_company As Boolean, ByVal insert_phone As Boolean, ByVal insert_other_contacts As Boolean, ByVal insert_contact_phone As Boolean, ByVal insert_cont As Boolean, ByVal insert_ac_company As Boolean)
    Insert_Contact = ""

    If insert_company = True Then
      'First I'll need to grab the contact company information.

      'First we need to add this company that the jetnet record is associated with to the client database. So this polls the database
      'and inserts it as needed. 
      'This is where you have to insert the jetnet company into the client company so everything links up. 
      'Make sure to use the comp_id.text

      Try
        aTempTable = aclsData_Temp.GetCompanyInfo_ID(comp_id.Text, "JETNET", 0)
        If Not IsNothing(aTempTable) Then 'not nothing
          Dim aclsClient_Company As New clsClient_Company
          Dim comp_id As Integer = 0
          For Each r As DataRow In aTempTable.Rows
            aclsClient_Company.clicomp_user_id = Session.Item("localUser").crmLocalUserID
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

          Dim idnum_new As Integer
          'inserting that info into the database. 
          Dim carry_on As Boolean = False
          aTempTable2 = aclsData_Temp.GetCompanyInfo_JETNET_ID(comp_id, "")
          If Not IsNothing(aTempTable2) Then  'This jetnet record isn't in a company record yet, so let's insert it.
            If aclsData_Temp.Insert_Client_Company(aclsClient_Company) = True Then
              carry_on = True
            End If
          Else
            carry_on = True
            'already exists
          End If

          If carry_on = True Then
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
                          LogError("Contact_Edit_Template.ascx.vb - Insert_Contact() - " & error_string)
                        End If
                        display_error()
                      End If
                    Next 'for each in get phone numbers
                  End If
                Else
                  If aclsData_Temp.class_error <> "" Then
                    error_string = aclsData_Temp.class_error
                    LogError("Contact_Edit_Template.ascx.vb - Insert_Contact() - " & error_string)
                  End If
                  display_error()
                End If
              Next 'For each row in get company info

              RaiseEvent get_insert_ac(jetnet_id, idnum_new, True, True, 0)
              'This is where I have to get all the other contacts from the jetnet company!!! Besides the one
              'That we have the id for!

              Dim status As Boolean = True
              '=========================
              If Not contact_inactive.Selected Then
                status = False
              End If

              RaiseEvent loop_contacts(idnum_new, comp_id, jetnet_id, True, status)

              '====================


              If insert_cont = True Then

                Dim active As String = "Y"

                If status = True Then
                  status = "N"
                End If


                'This is where I insert that last one.
                Dim aclsClient_Contact As New clsClient_Contact
                aclsClient_Contact.clicontact_preferred_name = pref.Text
                If CheckBox1.Checked = True Then
                  aclsClient_Contact.clicontact_email_list = "Y"
                Else
                  aclsClient_Contact.clicontact_email_list = "N"
                End If
                aclsClient_Contact.clicontact_user_id = Session.Item("localUser").crmLocalUserID
                aclsClient_Contact.clicontact_notes = Trim(TextBox1.Text)
                aclsClient_Contact.clicontact_sirname = Trim(sirname.SelectedValue)
                aclsClient_Contact.clicontact_first_name = Trim(firstname.Text)
                aclsClient_Contact.clicontact_middle_initial = Trim(middle.Text)
                aclsClient_Contact.clicontact_last_name = Trim(lastname.Text)
                aclsClient_Contact.clicontact_suffix = Trim(suffix.SelectedValue)
                aclsClient_Contact.clicontact_title = Trim(contact_title.Text)
                aclsClient_Contact.clicontact_email_address = Trim(Email.Text)
                aclsClient_Contact.clicontact_date_updated = Now()
                aclsClient_Contact.clicontact_status = active
                ' set to 0 since this is a Client record
                aclsClient_Contact.clicontact_jetnet_contact_id = jetnet_id
                aclsClient_Contact.clicontact_comp_id = idnum_new
                Dim contact_id_new As Integer
                Try
                  'Now finally we insert the contact. 
                  If aclsData_Temp.Insert_Client_Contact(aclsClient_Contact) = True Then
                    '  Response.Write("Insert Client Contact Success")
                    'And closes the form and sends the user on their way. 
                    aTempTable = aclsData_Temp.GetContactInfo_JETNET_ID(jetnet_id, "Y")
                    If Not IsNothing(aTempTable) Then 'not nothing
                      'Insert the new phone numbers
                      contact_id_new = aTempTable.Rows(0).Item("contact_id")
                      remove_contact_insert_contact_numbers(aTempTable.Rows(0).Item("contact_comp_id"), aTempTable.Rows(0).Item("contact_id"), False)
                    Else
                      If aclsData_Temp.class_error <> "" Then
                        error_string = aclsData_Temp.class_error
                        LogError("Contact_Edit_Template.ascx.vb - Insert_Contact() - " & error_string)
                      End If
                      display_error()
                    End If

                    'AddToSubFolder(contact_id_new)
                    'Get all the AC information.

                    aTempTable2 = aclsData_Temp.Get_Aircraft_Reference_JETNET_contactID(jetnet_id)
                    If Not IsNothing(aTempTable2) Then
                      If aTempTable2.Rows.Count > 0 Then
                        For Each q As DataRow In aTempTable2.Rows
                          Dim aclsInsert_Client_Aircraft_Reference As New clsClient_Aircraft_Reference

                          aclsInsert_Client_Aircraft_Reference.cliacref_comp_id = idnum_new
                          aclsInsert_Client_Aircraft_Reference.cliacref_contact_type = CStr(q("acref_contact_type"))
                          aclsInsert_Client_Aircraft_Reference.cliacref_contact_id = contact_id_new
                          aclsInsert_Client_Aircraft_Reference.cliacref_jetnet_ac_id = CStr(q("acref_ac_id"))
                          aclsInsert_Client_Aircraft_Reference.cliacref_operator_flag = CStr(q("acref_operator_flag"))
                          aclsInsert_Client_Aircraft_Reference.cliacref_owner_percentage = IIf(Not IsDBNull(q("acref_owner_percentage")), q("acref_owner_percentage"), "0")
                          aclsInsert_Client_Aircraft_Reference.cliacref_business_type = CStr(q("acref_business_type"))
                          aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_expires = Now()
                          aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_purchased = Now()

                          If aclsData_Temp.Insert_Client_Aircraft_Reference(aclsInsert_Client_Aircraft_Reference) = True Then
                            Session.Item("ListingID") = idnum_new
                            Session.Item("ContactID") = contact_id_new
                            Session.Item("ListingSource") = "CLIENT"
                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location = 'details.aspx';", True)
                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
                          Else
                            If aclsData_Temp.class_error <> "" Then
                              error_string = aclsData_Temp.class_error
                              LogError("Contact_Edit_Template.ascx.vb - Insert_Contact() - " & error_string)
                            End If
                            display_error()
                          End If
                        Next
                      End If
                    Else
                      If aclsData_Temp.class_error <> "" Then
                        error_string = aclsData_Temp.class_error
                        LogError("Contact_Edit_Template.ascx.vb - Insert_Contact() -  " & error_string)
                      End If
                      display_error()
                    End If

                  Else
                    If aclsData_Temp.class_error <> "" Then
                      error_string = aclsData_Temp.class_error
                      LogError("Contact_Edit_Template.ascx.vb - Insert_Contact() - " & error_string)
                    End If
                    display_error()
                  End If
                Catch ex As Exception
                  Response.Write("Error in client contact insert: " & ex.Message)
                End Try
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("Contact_Edit_Template.ascx.vb - Insert_Contact() - " & error_string)
                End If
                display_error()
              End If 'If table returning the comp ID isn't nothing

            End If

          End If 'if client company got inserted
        End If

      Catch ex As Exception
        error_string = "Contact_Edit_Template.ascx.vb - Insert_Contact() " & ex.Message
        LogError(error_string)
      End Try
    End If
  End Function
  Function Update_Contact(ByVal id As Integer, ByVal client_record As Boolean, ByVal remove_phone As Integer, ByVal insert_phone As Integer)
    'This is our little update_contact function.

    Update_Contact = ""
    Try
      Dim status As String = "Y"

      If contact_inactive.Selected = True Then
        status = "N"
      End If
      Dim aclsClient_Contact As New clsClient_Contact

      If client_record = True Then
        aclsClient_Contact.clicontact_preferred_name = pref.Text
        If CheckBox1.Checked = True Then
          aclsClient_Contact.clicontact_email_list = "Y"
        Else
          aclsClient_Contact.clicontact_email_list = "N"

        End If
        aclsClient_Contact.clicontact_user_id = Session.Item("localUser").crmLocalUserID
        aclsClient_Contact.clicontact_notes = Trim(TextBox1.Text)
        aclsClient_Contact.clicontact_id = id
        aclsClient_Contact.clicontact_comp_id = comp_id.Text
        aclsClient_Contact.clicontact_sirname = Trim(sirname.SelectedValue)
        aclsClient_Contact.clicontact_first_name = Trim(firstname.Text)
        aclsClient_Contact.clicontact_middle_initial = Trim(middle.Text)
        aclsClient_Contact.clicontact_last_name = Trim(lastname.Text)
        aclsClient_Contact.clicontact_suffix = Trim(suffix.SelectedValue)
        aclsClient_Contact.clicontact_title = Trim(contact_title.Text)
        aclsClient_Contact.clicontact_email_address = Trim(Email.Text)
        aclsClient_Contact.clicontact_status = status
        aclsClient_Contact.clicontact_date_updated = Now()
        aclsClient_Contact.clicontact_jetnet_contact_id = jetnet_contact_id.Text

        If aclsData_Temp.Update_Client_Contact(aclsClient_Contact) = True Then
          'this is updated record for client. phone numbers deleted and readded. company not touched. 
          If insert_phone = True Then
            remove_contact_insert_contact_numbers(comp_id.Text, id, remove_phone)
          End If
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location.reload();", True)
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("Contact_Edit_Template.ascx.vb - Update_Contact() - " & error_string)
          End If
          display_error()
        End If
      End If

    Catch ex As Exception
      error_string = "Update_Contact.ascx.vb - Page Init() " & ex.Message
      LogError(error_string)
    End Try
  End Function
  'This is a function that removes the contact ID numbers if wanted and inserts new ones. 
  Function remove_contact_insert_contact_numbers(ByVal idnum As String, ByVal client As String, ByVal remove As Boolean) As String
    remove_contact_insert_contact_numbers = ""
    Try

      If remove = True Then
        Dim aInt As Integer
        aInt = aclsData_Temp.DeletePhoneNumbers_contactID(client, idnum)
        ' check the state of the DataTable
        If aInt > 0 Then
          'inserting the c phone numbers:
        End If
      End If

      'Then we go ahead and reinsert the phone numbers. 
      Dim run As Boolean = False
      For x = 0 To 7
        run = False
        Dim aclsClient_Phone_Numbers As New clsClient_Phone_Numbers
        If x = 1 And phone1.Text <> "" Then
          aclsClient_Phone_Numbers.clipnum_type = type1.SelectedValue
          aclsClient_Phone_Numbers.clipnum_number = phone1.Text
          aclsClient_Phone_Numbers.clipnum_comp_id = idnum
          aclsClient_Phone_Numbers.clipnum_contact_id = client
          run = True
        ElseIf x = 2 And phone2.Text <> "" Then
          aclsClient_Phone_Numbers.clipnum_type = type2.SelectedValue
          aclsClient_Phone_Numbers.clipnum_number = phone2.Text
          aclsClient_Phone_Numbers.clipnum_comp_id = idnum
          aclsClient_Phone_Numbers.clipnum_contact_id = client
          run = True
        ElseIf x = 3 And phone3.Text <> "" Then
          aclsClient_Phone_Numbers.clipnum_type = type3.SelectedValue
          aclsClient_Phone_Numbers.clipnum_number = phone3.Text
          aclsClient_Phone_Numbers.clipnum_comp_id = idnum
          aclsClient_Phone_Numbers.clipnum_contact_id = client
          run = True
        ElseIf x = 4 And phone4.Text <> "" Then
          aclsClient_Phone_Numbers.clipnum_type = type4.SelectedValue
          aclsClient_Phone_Numbers.clipnum_number = phone4.Text
          aclsClient_Phone_Numbers.clipnum_comp_id = idnum
          aclsClient_Phone_Numbers.clipnum_contact_id = client
          run = True
        ElseIf x = 5 And phone5.Text <> "" Then
          aclsClient_Phone_Numbers.clipnum_type = type5.SelectedValue
          aclsClient_Phone_Numbers.clipnum_number = phone5.Text
          aclsClient_Phone_Numbers.clipnum_comp_id = idnum
          aclsClient_Phone_Numbers.clipnum_contact_id = client
          run = True
        ElseIf x = 6 And phone6.Text <> "" Then
          aclsClient_Phone_Numbers.clipnum_type = type6.SelectedValue
          aclsClient_Phone_Numbers.clipnum_number = phone6.Text
          aclsClient_Phone_Numbers.clipnum_comp_id = idnum
          aclsClient_Phone_Numbers.clipnum_contact_id = client
          run = True
        End If
        If run = True Then
          If aclsData_Temp.Insert_Client_PhoneNumbers(aclsClient_Phone_Numbers) = True Then
            'Response.Write("insert")
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = aclsData_Temp.class_error
              LogError("Contact_Edit_Template.ascx.vb - remove_contact_insert_contact_numbers() - " & error_string)
            End If
            display_error()
          End If
        End If
      Next x
    Catch ex As Exception
      error_string = "Contact_Edit_Template.ascx.vb - remove_contact_insert_contact_numbers() " & ex.Message
      LogError(error_string)
    End Try
  End Function
  Function insert_only_contact()
    insert_only_contact = ""
    Dim startdate As String
    Dim status As String = "Y"
    Dim idnum_new As Integer = CInt(Session.Item("ListingID"))

    Dim aclsClient_Contact As New clsClient_Contact
    Try
      aclsClient_Contact.clicontact_user_id = Session.Item("localUser").crmLocalUserID
      aclsClient_Contact.clicontact_sirname = sirname.SelectedValue
      aclsClient_Contact.clicontact_first_name = firstname.Text
      aclsClient_Contact.clicontact_middle_initial = middle.Text
      aclsClient_Contact.clicontact_last_name = lastname.Text
      aclsClient_Contact.clicontact_suffix = suffix.SelectedValue
      aclsClient_Contact.clicontact_title = contact_title.Text
      aclsClient_Contact.clicontact_email_address = Email.Text
      startdate = Now()
      aclsClient_Contact.clicontact_date_updated = startdate
      aclsClient_Contact.clicontact_notes = Trim(TextBox1.Text)
      startdate = Year(startdate) & "-" & Month(startdate) & "-" & (Day(startdate)) & " " & FormatDateTime(startdate, 4) & ":" & Second(startdate)


      ' set to 0 since this is a Client record
      aclsClient_Contact.clicontact_jetnet_contact_id = 0

      If Not IsNothing(Request.Item("contact_id")) Then
        If Not String.IsNullOrEmpty(Request.Item("contact_id").ToString) Then
          If Trim(Request("createClient")) = "true" Then
            aclsClient_Contact.clicontact_jetnet_contact_id = Request.Item("contact_id")
          End If
        End If
      End If

      aclsClient_Contact.clicontact_comp_id = idnum_new
      Dim contact_id_new As Integer
      'Now finally we insert the contact. 
      If aclsData_Temp.Insert_Client_Contact(aclsClient_Contact) = True Then
        'Response.Write("Insert Client Contact Success")
        'And closes the form and sends the user on their way. 
        aTempTable = aclsData_Temp.Get_Insert_Client_Contact(idnum_new, startdate, "Y")
        If Not IsNothing(aTempTable) Then 'not nothing
          'Insert the new phone numbers
          contact_id_new = aTempTable.Rows(0).Item("contact_id")
          remove_contact_insert_contact_numbers(aTempTable.Rows(0).Item("contact_comp_id"), aTempTable.Rows(0).Item("contact_id"), False)
          '?contact_ID=423542&comp_ID=277109&type=1&source=JETNET#
          If Trim(Request("from")) = "contactDetails" Then
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location.reload();", True)
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
          Else

            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location = 'details.aspx?type=1&source=CLIENT&comp_ID=" & idnum_new.ToString & "&contact_ID=" & contact_id_new & "';", True)
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("Contact_Edit_Template.ascx.vb - insert_only_contact - " & error_string)
          End If
          display_error()
        End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("Contact_Edit_Template.ascx.vb - insert_only_contact - " & error_string)
          End If
          display_error()
        End If

    Catch ex As Exception
      error_string = "Contact_Edit_Template.ascx.vb - Page Init() " & ex.Message
      LogError(error_string)
    End Try

  End Function
#End Region

  Public Sub Remove(ByVal id As Long)
    Dim companyID As Long = company_id.Text
    'check to see if it has notes attached.
    aTempTable = aclsData_Temp.Get_Local_Notes_Client_Contact(id)

    If Not IsNothing(aTempTable) Then
      If aTempTable.Rows.Count > 0 Then
        For Each r As DataRow In aTempTable.Rows
          Dim aclsLocal_Notes As New clsLocal_Notes
          'if there are references, update them to zero
          aclsLocal_Notes.lnote_jetnet_ac_id = r("lnote_jetnet_ac_id")
          aclsLocal_Notes.lnote_client_ac_id = r("lnote_client_ac_id")
          aclsLocal_Notes.lnote_jetnet_comp_id = r("lnote_jetnet_comp_id")
          aclsLocal_Notes.lnote_client_comp_id = r("lnote_client_comp_id")
          aclsLocal_Notes.lnote_client_contact_id = 0
          aclsLocal_Notes.lnote_jetnet_contact_id = r("lnote_jetnet_contact_id")
          aclsLocal_Notes.lnote_clipri_ID = r("lnote_clipri_ID")
          aclsLocal_Notes.lnote_document_flag = r("lnote_document_flag")
          aclsLocal_Notes.lnote_entry_date = r("lnote_entry_date")
          aclsLocal_Notes.lnote_status = r("lnote_status")
          aclsLocal_Notes.lnote_clipri_ID = r("lnote_clipri_ID")

          aclsLocal_Notes.lnote_note = r("lnote_note")
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
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = aclsData_Temp.class_error
            End If
          End If
        Next

      End If
    Else
      If aclsData_Temp.class_error <> "" Then
        error_string = aclsData_Temp.class_error
      End If
    End If

    'Check on AC References.
    aTempTable = aclsData_Temp.Get_Client_Aircraft_Reference_ContactID(id)

    If Not IsNothing(aTempTable) Then
      If aTempTable.Rows.Count > 0 Then
        For Each r As DataRow In aTempTable.Rows

          'Response.Write(r("cliacref_id") & "<br />")

          Dim aclsInsert_Client_Aircraft_Reference As New clsClient_Aircraft_Reference
          aclsInsert_Client_Aircraft_Reference.cliacref_comp_id = r("cliacref_comp_id")
          aclsInsert_Client_Aircraft_Reference.cliacref_cliac_id = r("cliacref_cliac_id")

          aclsInsert_Client_Aircraft_Reference.cliacref_id = r("cliacref_id")
          aclsInsert_Client_Aircraft_Reference.cliacref_contact_type = r("cliacref_contact_type")
          aclsInsert_Client_Aircraft_Reference.cliacref_contact_id = 0
          aclsInsert_Client_Aircraft_Reference.cliacref_jetnet_ac_id = r("cliacref_jetnet_ac_id")
          aclsInsert_Client_Aircraft_Reference.cliacref_cliac_id = r("cliacref_cliac_id")
          aclsInsert_Client_Aircraft_Reference.cliacref_operator_flag = r("cliacref_operator_flag")
          aclsInsert_Client_Aircraft_Reference.cliacref_owner_percentage = r("cliacref_owner_percentage")
          aclsInsert_Client_Aircraft_Reference.cliacref_business_type = r("cliacref_business_type")
          aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_expires = Now()
          aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_purchased = Now()

          If aclsData_Temp.Update_Client_Aircraft_Reference(aclsInsert_Client_Aircraft_Reference) = True Then
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = aclsData_Temp.class_error
            End If
          End If
        Next
      End If
    Else
      If aclsData_Temp.class_error <> "" Then
        error_string = aclsData_Temp.class_error
      End If
    End If

    If aclsData_Temp.Delete_Client_Contact(id) = 1 Then
      If Trim(Request("from")) = "contactDetails" Then
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location.href = '/DisplayCompanyDetail.aspx?compid=" & companyID.ToString & "&source=CLIENT';", True)
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
      Else
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location.reload();", True)
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
      End If
    End If
  End Sub

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


  Private Sub deleteFunction_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles deleteFunction.Click
    If Not IsNothing(Request.Item("contact_id")) Then
      If Not String.IsNullOrEmpty(Request.Item("contact_id").ToString) Then
        If IsNumeric(Request.Item("contact_id").ToString) Then
          Remove(Request.Item("contact_id"))
        End If
      End If
    End If
  End Sub
End Class