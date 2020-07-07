Imports System.IO
Partial Public Class Company_Edit_Template
  Inherits System.Web.UI.UserControl
  Public Event Fill_Phone(ByVal type1 As String, ByVal type2 As String, ByVal type3 As String, ByVal type4 As String, ByVal type5 As String, ByVal type6 As String, ByVal con As Control)
  Public Event loop_contacts(ByVal idnum_new As Integer, ByVal jetnet_id As Integer, ByVal contact_id As Integer, ByVal skip As Boolean, ByVal inactive As Boolean)
  Public Event get_insert_ac(ByVal jetnet_id As Integer, ByVal idnum_new As Integer, ByVal get_previous_ac As Boolean, ByVal insert_ac As Boolean, ByVal new_client_ac As Integer)
  Public aclsData_Temp As New clsData_Manager_SQL
  Public aTempTable, aTempTable2 As New DataTable 'Data Tables used
  Dim error_string As String = ""
#Region "Page Events"
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Me.Visible Then
      Try
        '-------------------------------------------Database Connections--------------------------------------------------------------
        If Session.Item("crmUserLogon") <> True Then
          Response.Redirect("Default.aspx", False)
        End If

        ' setup the connection info

        aclsData_Temp.class_error = ""
        aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
        aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase") 'CApplication.Item("crmJetnetDatabase")
        '---------------------------------------------End Database Connection Stuff---------------------------------------------

        If Not IsNothing(Request.Item("comp_ID")) Then
          If IsNumeric(Request.Item("comp_ID")) Then
            Session.Item("ListingID") = Request.Item("comp_ID")
          End If
        End If


        If Not IsNothing(Request.Item("source")) Then
          If Not IsNumeric(Request.Item("source")) Then
            Session.Item("ListingSource") = Request.Item("source")
          End If
        End If


        If Session.Item("isMobile") = True Then
          comp_description.Width = 200
          mobile_close.Text = "<a href='mobile_details.aspx?type=1&comp_ID=" & Session.Item("ListingID") & "'><img src=""images/cancel.gif"" alt=""Cancel"" border=""0""/></a>"
        End If


        If Trim(Request("remove")) = "true" Then

          Remove_Company()
        ElseIf Trim(Request("synch")) = "true" Then
          synch.Visible = True
          identify_main.Visible = False
          company_edit_table.Visible = False
          company_combine_table.Visible = False
          connect_company_table.Visible = False
        ElseIf Trim(Request("main_location")) = "true" Then
          identify_main.Visible = True
          company_edit_table.Visible = False
          company_combine_table.Visible = False
          connect_company_table.Visible = False
          Dim id As Integer = CInt(Session.Item("ListingID"))
          Dim source As String = Session.Item("ListingSource")
          child_company_text.Text = DisplayCompanyForCombine(id, source)
        ElseIf Trim(Request("connect")) = "true" Then
          company_edit_table.Visible = False
          identify_main.Visible = False
          company_combine_table.Visible = False
          connect_company_table.Visible = True
          If Not Page.IsPostBack Then
            connect_company_list.Items.Add(New ListItem("Please Search First", ""))
          End If
          Dim id As Integer = CInt(Session.Item("ListingID"))
          Dim source As String = Session.Item("ListingSource")
          Dim contact_text As String = ""

          connect_main_company.Text = DisplayCompanyForCombine(id, source)


          If Not Page.IsPostBack Then
            If Not String.IsNullOrEmpty(Session.Item("OtherID")) Then
              If Session.Item("OtherID") <> 0 Then
                connect_company_details.Text = DisplayCompanyForCombine(Session.Item("OtherID"), "JETNET")
                connect_remove.Visible = True
              End If
            End If
            aTempTable = aclsData_Temp.Get_Jetnet_Country()
            If Not IsNothing(aTempTable) Then
              If aTempTable.Rows.Count > 0 Then
                For Each r As DataRow In aTempTable.Rows
                  If Not IsDBNull(r("clicountry_name")) And Trim(r("clicountry_name")) <> "" Then
                    connect_country.Items.Add(New ListItem(CStr(r("clicountry_name")), CStr(r("clicountry_name"))))
                  End If
                Next
              End If
            Else
              If aclsData_Temp.class_error <> "" Then
                error_string = aclsData_Temp.class_error
                LogError("company_edit_template.aspx.vb - page load() - " & error_string)
              End If
              display_error()
            End If

            connect_country.Items.Add(New ListItem("ALL", ""))
            connect_country.SelectedValue = ""



            aTempTable = aclsData_Temp.Get_Jetnet_State()
            If Not IsNothing(aTempTable) Then
              If aTempTable.Rows.Count > 0 Then
                For Each r As DataRow In aTempTable.Rows
                  connect_state.Items.Add(New ListItem(CStr(r("client_state")), CStr(r("client_state_abbr"))))
                Next
              End If
            Else
              If aclsData_Temp.class_error <> "" Then
                error_string = aclsData_Temp.class_error
                LogError("company_edit_template.aspx.vb - page load() - " & error_string)
              End If
              display_error()
            End If
            connect_state.Items.Add(New ListItem("ALL", ""))
            connect_state.SelectedValue = ""
          End If
        ElseIf Trim(Request("combine")) = "true" Then
          company_edit_table.Visible = False
          identify_main.Visible = False
          connect_company_table.Visible = False
          If Not Page.IsPostBack Then
            combine_company_list.Items.Add(New ListItem("Please Search First", ""))
          End If
          Dim id As Integer = CInt(Session.Item("ListingID"))
          Dim source As String = Session.Item("ListingSource")
          Dim contact_text As String = ""
          company_combine_table.Visible = True

          company_combine_details.Text = DisplayCompanyForCombine(id, source)
        Else
          connect_company_table.Visible = False
          company_combine_table.Visible = False
          identify_main.Visible = False
          identify_main.Visible = False
          company_edit_table.Visible = True
          RaiseEvent Fill_Phone(type1.ID, type2.ID, type3.ID, type4.ID, type5.ID, type6.ID, Me)
          If Trim(Request("action")) <> "new" Then
            If Not Session.Item("ListingID") Is Nothing Then
              If Not Page.IsPostBack Then
                fill_edit_data()
              End If
            End If
          End If

          set_preferences()


          If Trim(Request("action")) <> "new" Then
            add_folder_cbo.Visible = False
          End If

          If Trim(Request("auto")) = "true" Then
            update_me()
            'Response.Write("auto create")
          ElseIf Trim(Request("action")) = "checkforcreation" Then
            Dim NoteType As String = "note"
            Dim returnView As Long = 18 'default prospect view

            Select Case Trim(Request("note_type"))
              Case "B"
                NoteType = "prospect"
              Case "P"
                NoteType = "action"
              Case Else
                NoteType = "note"
            End Select

            'Setting up a return view, this defaults to the prospect view up top.
            If Not String.IsNullOrEmpty(Trim(Request("returnView"))) Then
              If IsNumeric(Trim(Request("returnView"))) Then
                returnView = Trim(Request("returnView"))
              End If
            End If

            'This means we're dealing with company creation:
            'This also means that in order for the edit pages to work correctly, we need to set the session.
            Session.Item("Listing") = 1
            'This is a special case coming from the prospect view in which we need to check for the existance of a client company.
            'If there is one, we can redirect to add a note.
            'If there is not one, we send straight to the note page without adding a client company
            aTempTable = aclsData_Temp.GetCompanyInfo_JETNET_ID(Session.Item("ListingID"), "")
            '' check the state of the DataTable
            If Not IsNothing(aTempTable) Then
              If aTempTable.Rows.Count = 0 Then  'If there is not one, we send to the note page, without adding the client company - changed 5/6/2015.
                'Dim URL = "edit.aspx?comp_ID=" & Request.Item("comp_ID") & "&source=JETNET&" & IIf(Trim(Request("NoteID")) <> "", "NoteID=" & Trim(Request("NoteID")) & "&", "") & "ViewID=" & returnView & "&type=company&auto=true&from=view&note_type=" & NoteType & "&prospectACID=" & Trim(Request("prospectACID") & "&rememberTab=" & Trim(Request("rememberTab")))
                'Dim registerString As String = ""
                'registerString = "if (confirm(""Adding a note to a Jetnet Company forces Client Company Creation. Would you still like to add a note?"")) {"
                'registerString += " window.location.href='" & URL & "';"
                'registerString += " } else {"
                'registerString += " self.close(); "
                'registerString += " }"

                'Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "reloadPagetoEdit", registerString, True)

                Session.Item("Listing") = 3
                Session.Item("ListingID") = Trim(Request("prospectACID"))
                Session.Item("ListingSource") = "JETNET"

                Response.Redirect("edit_note.aspx?Prospect_Jetnet_Comp_ID=" & Request.Item("comp_ID") & "&source=JETNET&" & IIf(Trim(Request("NoteID")) <> "", "NoteID=" & Trim(Request("NoteID")) & "&", "") & "from=view&ac_ID=" & Trim(Request("prospectACID")) & "&type=" & NoteType & "&action=new&ViewID=" & returnView & "&refreshing=prospect&rememberTab=" & Trim(Request("rememberTab")), False)
                Context.ApplicationInstance.CompleteRequest()

              Else 'If there is one, we can redirect to add a note.
                Session.Item("Listing") = 3
                Session.Item("ListingID") = Trim(Request("prospectACID"))
                Session.Item("ListingSource") = "JETNET"

                Response.Redirect("edit_note.aspx?Prospect_Client_Comp_ID=" & aTempTable.Rows(0).Item("comp_id") & "&Prospect_Jetnet_Comp_ID=" & Request.Item("comp_ID") & "&source=JETNET&" & IIf(Trim(Request("NoteID")) <> "", "NoteID=" & Trim(Request("NoteID")) & "&", "") & "from=view&ac_ID=" & Trim(Request("prospectACID")) & "&type=" & NoteType & "&action=new&ViewID=" & returnView & "&refreshing=prospect&rememberTab=" & Trim(Request("rememberTab")), False)
                Context.ApplicationInstance.CompleteRequest()
              End If
            End If
          End If 'End checkforcreation.


        End If
      Catch ex As Exception
        error_string = "Company_Edit_Template.ascx.vb - Page Load() " & ex.Message
        LogError(error_string)
      End Try
    End If
  End Sub
#End Region
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

  Private Sub connect_country_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles connect_country.SelectedIndexChanged
    Try
      If connect_country.SelectedValue = "United States" Then
        connect_state.Visible = True
        state_connect_label.Visible = True
      Else
        connect_state.Visible = False
        state_connect_label.Visible = False
        connect_state.SelectedValue = ""

      End If
    Catch ex As Exception
      Dim masterPage As main_site = DirectCast(Page.Master, main_site)
      error_string = "Company_edit_Template.ascx.vb - connect_country_SelectedIndexChanged() " & ex.Message
      masterPage.LogError(error_string)
    End Try
  End Sub


  Function DisplayCompanyForCombine(ByVal id As Integer, ByVal source As String) As String
    DisplayCompanyForCombine = ""
    Dim contact_text As String = ""
    Try

      aTempTable = aclsData_Temp.GetCompanyInfo_ID(id, source, 0)
      ' check the state of the DataTable
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then

          For Each R As DataRow In aTempTable.Rows
            contact_text = contact_text & "<b>" & R("comp_name") & "</b><br />"
            If R("comp_address1") <> "" Then
              contact_text = contact_text & R("comp_address1") & "<br />"
            End If
            If Not IsDBNull(R("comp_address2")) Then
              If R("comp_address2") <> "" Then
                contact_text = contact_text & R("comp_address2") & "<br />"
              End If
            End If
            contact_text = contact_text & R("comp_city") & ", " & R("comp_state") & " "
            contact_text = contact_text & R("comp_zip_code") & "<br />"
            contact_text = contact_text & R("comp_country") & "<br />"
          Next
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("Company_Edit_Template.ascx.vb - Page Load() - " & error_string)
        End If
        display_error()
      End If
    Catch ex As Exception
      error_string = "Company_Edit_Template.ascx.vb - Page Load() " & ex.Message
      LogError(error_string)
    End Try
    DisplayCompanyForCombine = contact_text
  End Function

  Public Sub set_preferences()
    Try
      aTempTable = aclsData_Temp.Get_Client_Preferences()
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          For Each r As DataRow In aTempTable.Rows
            If Not IsDBNull(r("clipref_category1_use")) Then
              If r("clipref_category1_use") = "Y" Then
                comp_cat1.Visible = True
                comp_cat1_text.Visible = True
                comp_cat1_text.Text = CStr(IIf(Not IsDBNull(r("clipref_category1_name")), r("clipref_category1_name"), ""))
              Else
                comp_cat1.Visible = False
                comp_cat1_text.Visible = False
                comp_cat1_text.Text = CStr(IIf(Not IsDBNull(r("clipref_category1_name")), r("clipref_category1_name"), ""))
              End If
            End If

            If Not IsDBNull(r("clipref_category2_use")) Then
              If r("clipref_category2_use") = "Y" Then
                comp_cat2.Visible = True
                comp_cat2_text.Visible = True
                comp_cat2_text.Text = CStr(IIf(Not IsDBNull(r("clipref_category2_name")), r("clipref_category2_name"), ""))
              Else
                comp_cat2.Visible = False
                comp_cat2_text.Visible = False
                comp_cat2_text.Text = CStr(IIf(Not IsDBNull(r("clipref_category2_name")), r("clipref_category2_name"), ""))
              End If
            End If

            If Not IsDBNull(r("clipref_category3_use")) Then
              If r("clipref_category3_use") = "Y" Then
                comp_cat3.Visible = True
                comp_cat3_text.Visible = True
                comp_cat3_text.Text = CStr(IIf(Not IsDBNull(r("clipref_category3_name")), r("clipref_category3_name"), ""))
              Else
                comp_cat3.Visible = False
                comp_cat3_text.Visible = False
                comp_cat3_text.Text = CStr(IIf(Not IsDBNull(r("clipref_category3_name")), r("clipref_category3_name"), ""))
              End If
            End If

            If Not IsDBNull(r("clipref_category4_use")) Then
              If r("clipref_category4_use") = "Y" Then
                comp_cat4.Visible = True
                comp_cat4_text.Visible = True
                comp_cat4_text.Text = CStr(IIf(Not IsDBNull(r("clipref_category4_name")), r("clipref_category4_name"), ""))
              Else
                comp_cat4.Visible = False
                comp_cat4_text.Visible = False
                comp_cat4_text.Text = CStr(IIf(Not IsDBNull(r("clipref_category4_name")), r("clipref_category4_name"), ""))
              End If
            End If

            If Not IsDBNull(r("clipref_category5_use")) Then
              If r("clipref_category5_use") = "Y" Then
                comp_cat5.Visible = True
                comp_cat5_text.Visible = True
                comp_cat5_text.Text = CStr(IIf(Not IsDBNull(r("clipref_category5_name")), r("clipref_category5_name"), ""))
              Else
                comp_cat5.Visible = False
                comp_cat5_text.Visible = False
                comp_cat5_text.Text = CStr(IIf(Not IsDBNull(r("clipref_category5_name")), r("clipref_category5_name"), ""))

              End If
            End If
          Next
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("Company_Edit_Template.ascx.vb - Set_Preferences() - " & error_string)
        End If
        display_error()
      End If
    Catch ex As Exception
      error_string = "Company_Edit_Template.ascx.vb - Set_Preferences() " & ex.Message
      LogError(error_string)
    End Try
  End Sub

#Region "Fills Existing Editing Data for Company"
  Private Sub fill_edit_data()
    Dim id As Integer = CInt(Session.Item("ListingID"))
    Dim source As String = Session.Item("ListingSource")
    ' ---------------------------------------------Fill The Company Data--------------------------------------------------
    Try

      If id <> 0 And source <> "" Then
        aTempTable = aclsData_Temp.GetCompanyInfo_ID(id, source, 0)
      Else
        aTempTable = New DataTable
      End If
      ' check the state of the DataTable
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then

          For Each R As DataRow In aTempTable.Rows
            If Session.Item("ListingSource") = "CLIENT" Then
              deleteFunction.Visible = True
              If Not IsDBNull(R("comp_user_id")) Then
                aTempTable2 = aclsData_Temp.Get_Client_User(CInt(R("comp_user_id")))
                If Not IsNothing(aTempTable2) Then
                  If aTempTable2.Rows.Count > 0 Then

                    For Each q As DataRow In aTempTable2.Rows
                      update_text.Text = "Last Updated: " & R("comp_action_date") & "     By: " & q("cliuser_first_name") & " " & q("cliuser_last_name")
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

              Dim status As String = CStr(IIf(Not IsDBNull(R("clicomp_status")), R("clicomp_status"), ""))
              main_loc.Text = CInt(IIf(Not IsDBNull(R("clicomp_mainloc_comp_id")), R("clicomp_mainloc_comp_id"), "0"))
              If status = "Y" Then
                company_active.Selected = True
              Else
                company_inactive.Selected = True
              End If
            End If
            comp_address.Text = CStr(IIf(Not IsDBNull(R("comp_address1")), R("comp_address1"), ""))
            comp_address2.Text = CStr(IIf(Not IsDBNull(R("comp_address2")), R("comp_address2"), ""))
            comp_city.Text = CStr(IIf(Not IsDBNull(R("comp_city")), R("comp_city"), ""))
            comp_zip.Text = CStr(IIf(Not IsDBNull(R("comp_zip_code")), R("comp_zip_code"), ""))
            comp_name.Text = CStr(IIf(Not IsDBNull(R("comp_name")), R("comp_name"), ""))
            companyLabelHeader.Text = "<h2 class=""mainHeading remove_margin""><strong>" & CStr(IIf(Not IsDBNull(R("comp_name")), R("comp_name"), "")) & "</strong> Edit</h2>"
            'Response.Write(clsGeneral.clsGeneral.Get_Name_Search_String(comp_name.Text))

            comp_state.Text = CStr(IIf(Not IsDBNull(R("comp_state")), R("comp_state"), ""))
            comp_country.Text = CStr(IIf(Not IsDBNull(R("comp_country")), R("comp_country"), ""))
            comp_alt_name.Text = CStr(IIf(Not IsDBNull(R("comp_alternate_name")), R("comp_alternate_name"), ""))
            comp_email.Text = CStr(IIf(Not IsDBNull(R("comp_email_address")), R("comp_email_address"), ""))
            comp_web.Text = CStr(IIf(Not IsDBNull(R("comp_web_address")), R("comp_web_address"), ""))

            If source = "CLIENT" Then
              jetnet_comp_id.Text = CStr(IIf(Not IsDBNull(R("jetnet_comp_id")), R("jetnet_comp_id"), ""))
              comp_description.Text = CStr(IIf(Not IsDBNull(R("clicomp_description")), R("clicomp_description"), ""))
              comp_cat1.Text = CStr(IIf(Not IsDBNull(R("clicomp_category1")), R("clicomp_category1"), ""))
              comp_cat2.Text = CStr(IIf(Not IsDBNull(R("clicomp_category2")), R("clicomp_category2"), ""))
              comp_cat3.Text = CStr(IIf(Not IsDBNull(R("clicomp_category3")), R("clicomp_category3"), ""))
              comp_cat4.Text = CStr(IIf(Not IsDBNull(R("clicomp_category4")), R("clicomp_category4"), ""))
              comp_cat5.Text = CStr(IIf(Not IsDBNull(R("clicomp_category5")), R("clicomp_category5"), ""))
            End If

            Dim agen As String = CStr(IIf(Not IsDBNull(R("comp_agency_type")), R("comp_agency_type"), ""))

            If agen = "C" Then
              civilian.Selected = True
            ElseIf agen = "G" Then
              government.Selected = True
            ElseIf agen = "O" Then
              other.Selected = True
            Else
              unknown.Selected = True
            End If

          Next
        Else
          ' Response.Write("No Rows")
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("Company_Edit_Template.ascx.vb - fill_edit_data() - " & error_string)
        End If
        display_error()
      End If

      '---------------------------------------------Fill the Phone Data----------------------------------------------------
      If id <> 0 And source <> "" Then
        aTempTable = aclsData_Temp.GetPhoneNumbers(id, 0, source, 0)
      Else
        aTempTable = New DataTable
      End If
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
        Else
          'no rows
        End If

      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("Company_Edit_Template.ascx.vb - fill_edit_data() - " & error_string)
        End If
        display_error()
      End If

      'Determine if the categories need to show up. 

      aTempTable = aclsData_Temp.Get_Client_Preferences()
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          For Each r As DataRow In aTempTable.Rows
            If Not IsDBNull(r("clipref_category1_use")) Then
              If r("clipref_category1_use") = "Y" Then
                comp_cat1.Visible = True
                comp_cat1_text.Visible = True
                comp_cat1_text.Text = CStr(IIf(Not IsDBNull(r("clipref_category1_name")), r("clipref_category1_name") & "<br />", ""))
                comp_cat1_text.CssClass = "margined"
              Else
                comp_cat1.Visible = False
                comp_cat1_text.Visible = False
              End If
            End If

            If Not IsDBNull(r("clipref_category2_use")) Then
              If r("clipref_category2_use") = "Y" Then
                comp_cat2.Visible = True
                comp_cat2_text.Visible = True
                comp_cat2_text.Text = CStr(IIf(Not IsDBNull(r("clipref_category2_name")), r("clipref_category2_name") & "<br />", ""))
                comp_cat2_text.CssClass = "margined"
              Else
                comp_cat2.Visible = False
                comp_cat2_text.Visible = False
              End If
            End If

            If Not IsDBNull(r("clipref_category3_use")) Then
              If r("clipref_category3_use") = "Y" Then
                comp_cat3.Visible = True
                comp_cat3_text.Visible = True
                comp_cat3_text.Text = CStr(IIf(Not IsDBNull(r("clipref_category3_name")), r("clipref_category3_name") & "<br />", ""))
                comp_cat3_text.CssClass = "margined"
              Else
                comp_cat3.Visible = False
                comp_cat3_text.Visible = False
              End If
            End If

            If Not IsDBNull(r("clipref_category4_use")) Then
              If r("clipref_category4_use") = "Y" Then
                comp_cat4.Visible = True
                comp_cat4_text.Visible = True
                comp_cat4_text.Text = CStr(IIf(Not IsDBNull(r("clipref_category4_name")), r("clipref_category4_name") & "<br />", ""))
                comp_cat4_text.CssClass = "margined"
              Else
                comp_cat4.Visible = False
                comp_cat4_text.Visible = False
              End If
            End If

            If Not IsDBNull(r("clipref_category5_use")) Then
              If r("clipref_category5_use") = "Y" Then
                comp_cat5.Visible = True
                comp_cat5_text.Visible = True
                comp_cat5_text.Text = CStr(IIf(Not IsDBNull(r("clipref_category5_name")), r("clipref_category5_name") & "<br />", ""))
                comp_cat5_text.CssClass = "margined"
              Else
                comp_cat5.Visible = False
                comp_cat5_text.Visible = False
              End If
            End If
          Next
        End If
      End If
    Catch ex As Exception
      error_string = "Company_Edit_Template.ascx.vb - Fill_Edit_Data() " & ex.Message
      LogError(error_string)
    End Try
  End Sub
#End Region
#Region "Event Runs when trying to add/update company"
  Public Sub updateFunction_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles updateFunction.Click
    If Page.IsValid Then
      If UCase(comp_country.Text) = "USA" Or UCase(comp_country.Text) = "U.S.A." Or UCase(comp_country.Text) = "U.S.A" Or UCase(comp_country.Text) = "US" Then
        comp_country.Text = "United States"
      End If
      update_me()
      If Trim(Request("from")) <> "homePage" Then
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
      End If
    End If
  End Sub

  Public Sub update_me()
    Try

      Dim idnum As Integer
      If Trim(Request("action")) = "new" Then
        Session.Item("ListingID") = 0
        Session.Item("ListingSource") = ""
      End If
      If Not Session.Item("ListingID") Is Nothing Then
        idnum = Session.Item("ListingID")
      Else
        idnum = 0
      End If


      Dim source As String = Session.Item("ListingSource")
      Select Case source '1.) First Check and see if this is a client record or a contact record.
        Case "CLIENT" '2.) If client record, update client record. Remove phone numbers, add new phone numbers.
          Update_Client_Company(jetnet_comp_id.Text, idnum, True, True, True)
        Case "JETNET" 'If jetnet record, check and see if the jetnet record company ID already exists in the database.
          Dim errored As String = ""
          aTempTable = aclsData_Temp.GetCompanyInfo_JETNET_ID(idnum, errored)
          '' check the state of the DataTable
          If Not IsNothing(aTempTable) Then
            If aTempTable.Rows.Count = 0 Then 'If it does not, insert the company using the data provided.
              Insert_Company(idnum, True, True, True, True, True)
              'Response.Write("insert")
            Else 'If it does, go to 2 and update using the ID number we found from poling the database.
              Update_Client_Company(idnum, aTempTable.Rows(0).Item("comp_id"), True, True, True)
            End If
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = aclsData_Temp.class_error
              LogError("Company_Edit_Template.ascx.vb - update_me() - " & error_string)
            End If
            display_error()
          End If
        Case Else
          Insert_Company(0, True, True, False, False, False)
      End Select

      If Session.Item("isMobile") = True Then
        Response.Redirect("Mobile_Details.aspx?type=" & Session.Item("Listing") & "&comp_id=" & Session.Item("ListingID") & "&source=" & Session.Item("ListingSource") & "&edited=company", False)
      End If

    Catch ex As Exception
      error_string = "Company_Edit_Template.ascx.vb - update_me() " & ex.Message
      LogError(error_string)
    End Try
  End Sub
#End Region
#Region "Functions that Insert/Update Existing/NonExisting Companies"
  Function Insert_Company(ByVal jetnet_id As Integer, ByVal insert_comp As Boolean, ByVal insert_phone As Boolean, ByVal insert_contacts As Boolean, ByVal insert_contact_phone As Boolean, ByVal insert_ac As Boolean)
    Insert_Company = ""
    Try
      Dim startdate As String
      Dim status As String = "Y"
      If company_inactive.Selected Then
        status = "N"
      End If
      Dim aclsClient_Company As New clsClient_Company
      aclsClient_Company.clicomp_user_id = Session.Item("localUser").crmLocalUserID
      aclsClient_Company.clicomp_name = Trim(comp_name.Text)
      aclsClient_Company.clicomp_alternate_name_type = ""
      aclsClient_Company.clicomp_alternate_name = Trim(comp_alt_name.Text)
      aclsClient_Company.clicomp_address1 = Trim(comp_address.Text)
      aclsClient_Company.clicomp_address2 = Trim(comp_address2.Text)
      aclsClient_Company.clicomp_city = Trim(comp_city.Text)
      aclsClient_Company.clicomp_state = Trim(comp_state.Text)
      aclsClient_Company.clicomp_zip_code = Trim(comp_zip.Text)
      aclsClient_Company.clicomp_country = Trim(comp_country.Text)
      aclsClient_Company.clicomp_agency_type = Trim(comp_agency_type.SelectedValue)
      aclsClient_Company.clicomp_web_address = Trim(comp_web.Text)
      aclsClient_Company.clicomp_email_address = Trim(comp_email.Text)
      aclsClient_Company.clicomp_status = status
      startdate = Now()
      aclsClient_Company.clicomp_date_updated = startdate
      startdate = Year(startdate) & "-" & Month(startdate) & "-" & (Day(startdate)) & " " & FormatDateTime(startdate, 4) & ":" & Second(startdate)
      aclsClient_Company.clicomp_jetnet_comp_id = jetnet_id
      aclsClient_Company.clicomp_description = Trim(comp_description.Text)
      aclsClient_Company.clicomp_category1 = Trim(comp_cat1.Text)
      aclsClient_Company.clicomp_category2 = Trim(comp_cat2.Text)

      aclsClient_Company.clicomp_category3 = Trim(comp_cat3.Text)
      aclsClient_Company.clicomp_category4 = Trim(comp_cat4.Text)
      aclsClient_Company.clicomp_category5 = Trim(comp_cat5.Text)

      'Response.Write(aclsClient_Company.ClassInfo(aclsClient_Company))

      If insert_comp = True Then
        Dim idnum_new As Integer
        If aclsData_Temp.Insert_Client_Company(aclsClient_Company) = True Then
          'This basically inserts the client company.
          'Next if it inserted alright, we're polling the client company based on the jetnet ID.
          'This gives us our ID of the new company we just inserted. 
          'unless it has no jetnet ID, so let's take care of that

          If jetnet_id = 0 Then
            aTempTable = aclsData_Temp.Get_Insert_Client_Company(comp_name.Text, startdate, status)
            If Not IsNothing(aTempTable) Then 'not nothing
              If aTempTable.Rows.Count > 0 Then
                idnum_new = aTempTable.Rows(0).Item("comp_id")
              End If
            Else
              If aclsData_Temp.class_error <> "" Then
                error_string = aclsData_Temp.class_error
                LogError("Company_Edit_Template.ascx.vb - Insert_Company() - " & error_string)
              End If
              display_error()
            End If
          Else
            aTempTable = aclsData_Temp.GetCompanyInfo_JETNET_ID(jetnet_id, "")
            If Not IsNothing(aTempTable) Then 'not nothing
              idnum_new = aTempTable.Rows(0).Item("comp_id")
            Else
              If aclsData_Temp.class_error <> "" Then
                error_string = aclsData_Temp.class_error
                LogError("Company_Edit_Template.ascx.vb - Insert_Company() - " & error_string)
              End If
              display_error()
            End If

            'After the company gets inserted, this function 
            'will update the previous notes associated with the jetnet company to now be associated with the
            'client company
            'However this is stuck inside the if jetnet_id <> 0 if/then.
            'It does not make sense to run it in the other case.
            aclsData_Temp.UpdateCompanyNotesWithClientID(idnum_new, jetnet_id)

          End If




          'add to subfolder!
          'If add_folder_cbo.SelectedValue <> 0 Then
          '    AddToSubFolder(idnum_new)
          'End If


          If insert_phone = True Then
            'Next we call the function that phone numbers inserts them as new ones. 
            'Make sure we pass the new idnum_new
            remove_company_insert_company(idnum_new, False)
          End If

          status = True
          If company_inactive.Selected = False Then
            status = False
          End If
          If insert_contacts = True Then
            'loop through all the contacts
            RaiseEvent loop_contacts(idnum_new, jetnet_id, 0, True, status)
          End If

          If insert_ac = True Then
            'Next we need to poll the database for AC References
            RaiseEvent get_insert_ac(jetnet_id, idnum_new, True, True, 0)
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("Company_Edit_Template.ascx.vb - Insert_Company() - " & error_string)
          End If
          display_error()
        End If
        Session.Item("OtherID") = jetnet_id
        Session.Item("ListingID") = idnum_new
        Session.Item("ListingSource") = "CLIENT"

 

        If Trim(Request("auto")) = "true" Then
          If LCase(Trim(Request("from"))) <> "view" Then 'And LCase(Trim(Request("note"))) <> "prospect" Then
            Dim url As String = "edit_note.aspx?action=new&type=" & Trim(Request("note_type")) & "'"

            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location = 'details.aspx';", True)
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.location = '" & url & ";", True)
          ElseIf LCase(Trim(Request("from"))) = "view" Then
            Session.Item("OtherID") = 0
            Session.Item("ListingID") = Trim(Request("prospectACID"))
            Session.Item("ListingSource") = "JETNET"
            Session.Item("Listing") = 3

            Response.Redirect("edit_note.aspx?Prospect_Client_Comp_ID=" & idnum_new & "&Prospect_Jetnet_Comp_ID=" & jetnet_id & "&source=JETNET&ac_ID=" & Trim(Request("prospectACID")) & "&type=" & Trim(Request("note_type")) & "&action=new&refreshing=prospect&ViewID=" & Trim(Request("ViewID")) & "&from=view&" & IIf(Trim(Request("NoteID")) <> "", "NoteID=" & Trim(Request("NoteID")) & "&", "") & "rememberTab=" & Trim(Request("rememberTab")), False)
            Context.ApplicationInstance.CompleteRequest()


          End If

        ElseIf Trim(Request("from")) = "companyDetails" Then
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_WindowNow", "window.opener.location = 'DisplayCompanyDetail.aspx?compid=" & idnum_new.ToString & "&source=CLIENT';", True)
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
        ElseIf Trim(Request("from")) = "homePage" Then
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_WindowNow", "window.location.href = 'DisplayCompanyDetail.aspx?compid=" & idnum_new.ToString & "&source=CLIENT';", True)
        Else
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location = 'details.aspx';", True)
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
        End If

      End If

    Catch ex As Exception
      error_string = "Company_Edit_Template.ascx.vb - Insert_Company() " & ex.Message
      LogError(error_string)
    End Try
  End Function

  Function Update_Client_Company(ByVal jetnet_id As Integer, ByVal id As Integer, ByVal client_record As Boolean, ByVal remove_phone As Boolean, ByVal insert_phone As Boolean)
    Update_Client_Company = ""

    Try
      If client_record = True Then
        'First we update the client record. 
        Dim startdate As String
        startdate = Now()
        startdate = Year(startdate) & "-" & Month(startdate) & "-" & Day(startdate) & " " & TimeValue(Now())
        Dim status As String = "Y"

        If company_inactive.Selected Then
          status = "N"
        End If

        Dim aclsClient_Company As New clsClient_Company

        aclsClient_Company.clicomp_user_id = Session.Item("localUser").crmLocalUserID
        aclsClient_Company.clicomp_id = id
        aclsClient_Company.clicomp_name = Trim(comp_name.Text)
        aclsClient_Company.clicomp_alternate_name_type = ""
        aclsClient_Company.clicomp_alternate_name = Trim(comp_alt_name.Text)
        aclsClient_Company.clicomp_address1 = Trim(comp_address.Text)
        aclsClient_Company.clicomp_address2 = Trim(comp_address2.Text)
        aclsClient_Company.clicomp_city = Trim(comp_city.Text)
        aclsClient_Company.clicomp_state = Trim(comp_state.Text)
        aclsClient_Company.clicomp_zip_code = Trim(comp_zip.Text)
        aclsClient_Company.clicomp_country = Trim(comp_country.Text)
        aclsClient_Company.clicomp_agency_type = Trim(comp_agency_type.SelectedValue)
        aclsClient_Company.clicomp_web_address = Trim(comp_web.Text)
        aclsClient_Company.clicomp_email_address = Trim(comp_email.Text)
        aclsClient_Company.clicomp_date_updated = startdate
        ' set to zero for now since i'm updating a client record
        aclsClient_Company.clicomp_jetnet_comp_id = jetnet_id
        aclsClient_Company.clicomp_status = status
        aclsClient_Company.clicomp_description = Trim(comp_description.Text)

        aclsClient_Company.clicomp_category1 = Trim(comp_cat1.Text)
        aclsClient_Company.clicomp_category2 = Trim(comp_cat2.Text)

        aclsClient_Company.clicomp_category3 = Trim(comp_cat3.Text)
        aclsClient_Company.clicomp_category4 = Trim(comp_cat4.Text)
        aclsClient_Company.clicomp_category5 = Trim(comp_cat5.Text)
        aclsClient_Company.clicomp_mainloc_comp_id = CInt(IIf(IsNumeric(main_loc.Text), main_loc.Text, 0))
        'This is weird, we're going to need to keep the jetnet comp_id linked to the contact, right?

        'Response.Write(aclsClient_Company.ClassInfo(aclsClient_Company))

        'Then once that is done we're removing the client phone numbers associated with this company. 
        If aclsData_Temp.Update_Client_Company(aclsClient_Company) = True Then
          'If you have the okay to insert a phone number
          'Remove Phone is the okay to remove the other numbers previous. 
          If insert_phone = True Then
            remove_company_insert_company(id, remove_phone)
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error

            LogError("Company_Edit_Template.ascx.vb - Update_Client_Company() - " & error_string)
          End If
          display_error()
        End If

      End If

      If Trim(Request("from")) = "companyDetails" Then
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_WindowNow", "window.opener.location = 'DisplayCompanyDetail.aspx?compid=" & id.ToString & "&source=" & Session.Item("ListingSource") & "';", True)
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
      Else
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_WindowNow", "window.opener.location = 'details.aspx';", True)
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
      End If
    Catch ex As Exception
      error_string = "Company_Edit_Template.ascx.vb - Update_Client_Company() - " & ex.Message
      LogError(error_string)
    End Try
  End Function
  'This removes the company phone numbers and inserts them if we need to.
  Function remove_company_insert_company(ByVal idnum As String, ByVal remove As Boolean) As String
    remove_company_insert_company = ""
    Try

      If remove = True Then
        Dim aInt As Integer
        aInt = aclsData_Temp.DeletePhoneNumbers_compID(idnum)
        ' check the state of the DataTable
        If aInt > 0 Then
          'inserting the c phone numbers:
        End If
        display_error()
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
          aclsClient_Phone_Numbers.clipnum_contact_id = 0
          run = True
        ElseIf x = 2 And phone2.Text <> "" Then
          aclsClient_Phone_Numbers.clipnum_type = type2.SelectedValue
          aclsClient_Phone_Numbers.clipnum_number = phone2.Text
          aclsClient_Phone_Numbers.clipnum_comp_id = idnum
          aclsClient_Phone_Numbers.clipnum_contact_id = 0
          run = True
        ElseIf x = 3 And phone3.Text <> "" Then
          aclsClient_Phone_Numbers.clipnum_type = type3.SelectedValue
          aclsClient_Phone_Numbers.clipnum_number = phone3.Text
          aclsClient_Phone_Numbers.clipnum_comp_id = idnum
          aclsClient_Phone_Numbers.clipnum_contact_id = 0
          run = True
        ElseIf x = 4 And phone4.Text <> "" Then
          aclsClient_Phone_Numbers.clipnum_type = type4.SelectedValue
          aclsClient_Phone_Numbers.clipnum_number = phone4.Text
          aclsClient_Phone_Numbers.clipnum_comp_id = idnum
          aclsClient_Phone_Numbers.clipnum_contact_id = 0
          run = True
        ElseIf x = 5 And phone5.Text <> "" Then
          aclsClient_Phone_Numbers.clipnum_type = type5.SelectedValue
          aclsClient_Phone_Numbers.clipnum_number = phone5.Text
          aclsClient_Phone_Numbers.clipnum_comp_id = idnum
          aclsClient_Phone_Numbers.clipnum_contact_id = 0
          run = True
        ElseIf x = 6 And phone6.Text <> "" Then
          aclsClient_Phone_Numbers.clipnum_type = type6.SelectedValue
          aclsClient_Phone_Numbers.clipnum_number = phone6.Text
          aclsClient_Phone_Numbers.clipnum_comp_id = idnum
          aclsClient_Phone_Numbers.clipnum_contact_id = 0
          run = True
        End If
        If run = True Then
          If aclsData_Temp.Insert_Client_PhoneNumbers(aclsClient_Phone_Numbers) = True Then
            ' Response.Write("insert phone")
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = aclsData_Temp.class_error
              LogError("Company_Edit_Template.ascx.vb - remove_company_insert_company() - " & error_string)
            End If
            display_error()
          End If

        End If
      Next x
    Catch ex As Exception
      error_string = "Company_Edit_Template.ascx.vb - remove_company_insert_company() - " & ex.Message
      LogError(error_string)
    End Try
  End Function
#End Region

  Private Sub AddToSubFolder(ByVal idnum As Integer)
    Try
      Dim source As String = "CLIENT"
      Dim ftype As Integer = Session.Item("Listing")
      Dim contact As Integer = 0
      Dim selectedvalue As String = add_folder_cbo.SelectedValue

      If Session.Item("Listing_ContactID") <> 0 Then
        contact = CInt(Session.Item("Listing_ContactID"))
        idnum = CInt(Session.Item("Listing_ContactID"))
      End If


      Dim errored As String = ""
      Select Case ftype
        Case 1
          If contact = 0 Then
            If source = "JETNET" Then
              If aclsData_Temp.Insert_Into_Client_Folder_Index(selectedvalue, 0, idnum, 0, 0, 0, 0, 0, errored) = 1 Then
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("Company_Edit_Template.ascx.vb - AddToSubFolder() - " & error_string)
                End If
                display_error()
              End If
            ElseIf source = "CLIENT" Then
              If aclsData_Temp.Insert_Into_Client_Folder_Index(selectedvalue, 0, 0, 0, 0, idnum, 0, 0, errored) = 1 Then
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("Company_Edit_Template.ascx.vb - AddToSubFolder() - " & error_string)
                End If
                display_error()
              End If
            End If
          Else
            If source = "JETNET" Then
              If aclsData_Temp.Insert_Into_Client_Folder_Index(selectedvalue, 0, 0, contact, 0, 0, 0, 0, errored) = 1 Then
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("Company_Edit_Template.ascx.vb - AddToSubFolder() - " & error_string)
                End If
                display_error()
              End If
            ElseIf source = "CLIENT" Then
              If aclsData_Temp.Insert_Into_Client_Folder_Index(selectedvalue, 0, 0, 0, 0, 0, contact, 0, errored) = 1 Then
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("Company_Edit_Template.ascx.vb - AddToSubFolder() - " & error_string)
                End If
                display_error()
              End If
            End If
          End If

        Case 3

          If source = "JETNET" Then
            If aclsData_Temp.Insert_Into_Client_Folder_Index(selectedvalue, idnum, 0, 0, 0, 0, 0, 0, errored) = 1 Then
            Else
              If aclsData_Temp.class_error <> "" Then
                error_string = aclsData_Temp.class_error
                LogError("Company_Edit_Template.ascx.vb - AddToSubFolder() - " & error_string)
              End If
              display_error()
            End If
          ElseIf source = "CLIENT" Then
            If aclsData_Temp.Insert_Into_Client_Folder_Index(selectedvalue, 0, 0, 0, idnum, 0, 0, 0, errored) = 1 Then
            Else
              If aclsData_Temp.class_error <> "" Then
                error_string = aclsData_Temp.class_error
                LogError("Company_Edit_Template.ascx.vb - AddToSubFolder() - " & error_string)
              End If
              display_error()
            End If

          End If
      End Select
    Catch ex As Exception
      error_string = "Company_Edit_Template.ascx.vb - AddToSubFolder() - " & ex.Message
      LogError(error_string)
    End Try
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

  Private Sub search_combine_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles search_combine.Click
    Try
      Dim contact_text As String = ""
      aTempTable = aclsData_Temp.Company_Search("%" & clsGeneral.clsGeneral.Get_Name_Search_String(search_combine_text.Text) & "%", "Y", "C", "", "", "", Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmCommercial_Flag, "", "", "", "", "", "", "")
      combine_company_list.Items.Clear()
      combine_company_list.Items.Add(New ListItem("NONE SELECTED", "|"))
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          combine_company_list.Enabled = True
          For Each r As DataRow In aTempTable.Rows
            contact_text = r("comp_name") & " - "
            If r("comp_address1") <> "" Then
              contact_text = contact_text & r("comp_address1") & " "
            End If
            If Not IsDBNull(r("comp_address2")) Then
              If r("comp_address2") <> "" Then
                contact_text = contact_text & r("comp_address2") & " "
              End If
            End If
            contact_text = contact_text & r("comp_city") & ", " & r("comp_state") & " "
            contact_text = contact_text & r("comp_zip_code") & " "
            contact_text = contact_text & r("comp_country")

            If r("comp_id") = CInt(Session("ListingID")) Then

            Else
              combine_company_list.Items.Add(New ListItem(contact_text, r("comp_id") & "|" & r("source")))
            End If
          Next
        Else ' 0 rows
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("company_edit_template.aspx.vb - search_combine_Click() - " & error_string)
        End If
        display_error()
      End If
    Catch ex As Exception
      error_string = "company_edit_template.aspx.vb - search_combine_Click() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub

  Private Sub combine_company_list_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles combine_company_list.SelectedIndexChanged
    Try
      If combine_company_list.SelectedValue <> "" Then
        Dim info As Array = Split(combine_company_list.SelectedValue, "|")
        If UBound(info) <> 1 Then
          Attention.Text = "<p align='center'>Please select a company to combine.</p>"
        Else
          combining_company_details.Text = DisplayCompanyForCombine(info(0), info(1))
        End If
      Else
        Attention.Text = "<p align='center'>Please select a company to combine.</p>"
      End If
      combine_me.Visible = True
    Catch ex As Exception
      error_string = "company_edit_template.aspx.vb - combine_company_list_SelectedIndexChanged() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub

  Private Sub combine_me_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles combine_me.Click
    'Combining Companies.
    Try
      If combine_company_list.SelectedValue <> "" Then
        Dim info As Array = Split(combine_company_list.SelectedValue, "|")
        ' Response.Write("combine " & Session("ListingID") & " and " & info(0))



        '1.) Search for all info(0) company notes.
        '2.) If Found, Replace all lnote_company_client_id with session("ListingID")

        aTempTable = aclsData_Temp.Get_Local_Notes_Client_Company(info(0))

        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            For Each r As DataRow In aTempTable.Rows

              Dim aclsLocal_Notes As New clsLocal_Notes
              aclsLocal_Notes.lnote_jetnet_comp_id = r("lnote_jetnet_comp_id")
              aclsLocal_Notes.lnote_client_comp_id = Session("ListingID")
              aclsLocal_Notes.lnote_client_contact_id = 0
              aclsLocal_Notes.lnote_jetnet_contact_id = r("lnote_jetnet_contact_id")
              aclsLocal_Notes.lnote_clipri_ID = r("lnote_clipri_ID")
              aclsLocal_Notes.lnote_document_flag = r("lnote_document_flag")
              aclsLocal_Notes.lnote_note = CStr(IIf(Not IsDBNull(r("lnote_note")), r("lnote_note"), ""))
              aclsLocal_Notes.lnote_id = r("lnote_id")
              aclsLocal_Notes.lnote_action_date = Now()
              aclsLocal_Notes.lnote_user_login = CStr(IIf(Not IsDBNull(r("lnote_user_login")), r("lnote_user_login"), ""))
              aclsLocal_Notes.lnote_user_name = CStr(IIf(Not IsDBNull(r("lnote_user_name")), r("lnote_user_name"), ""))
              aclsLocal_Notes.lnote_notecat_key = r("lnote_notecat_key")
              aclsLocal_Notes.lnote_user_id = r("lnote_user_id")
              aclsLocal_Notes.lnote_jetnet_ac_id = r("lnote_jetnet_ac_id")
              aclsLocal_Notes.lnote_client_ac_id = r("lnote_client_ac_id")
              aclsLocal_Notes.lnote_entry_date = r("lnote_entry_date")
              aclsLocal_Notes.lnote_status = r("lnote_status")
              aclsLocal_Notes.lnote_schedule_start_date = CStr(IIf(Not IsDBNull(r("lnote_schedule_start_date")), r("lnote_schedule_start_date"), "1/1/1900"))
              aclsLocal_Notes.lnote_schedule_end_date = CStr(IIf(Not IsDBNull(r("lnote_schedule_end_date")), r("lnote_schedule_end_date"), "1/1/1900"))


              If aclsData_Temp.update_localNote(aclsLocal_Notes) = True Then
                '  Response.Write("Update Class Info: " & aclsLocal_Notes.ClassInfo(aclsLocal_Notes) & "<br /><hr />")
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("edit_note.aspx.vb - update_note() - " & error_string)
                End If
                display_error()
              End If

            Next
          End If
        End If

        '3.) Search for all info(0) AC References.
        aTempTable = aclsData_Temp.Get_Client_Aircraft_Reference_CompID(info(0))

        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            For Each r As DataRow In aTempTable.Rows

              Dim aclsInsert_Client_Aircraft_Reference As New clsClient_Aircraft_Reference
              aclsInsert_Client_Aircraft_Reference.cliacref_comp_id = Session("ListingID")
              aclsInsert_Client_Aircraft_Reference.cliacref_cliac_id = r("cliacref_cliac_id")

              aclsInsert_Client_Aircraft_Reference.cliacref_id = r("cliacref_id")


              aclsInsert_Client_Aircraft_Reference.cliacref_contact_type = CStr(IIf(Not IsDBNull(r("cliacref_contact_type")), r("cliacref_contact_type"), ""))
              aclsInsert_Client_Aircraft_Reference.cliacref_contact_id = 0
              aclsInsert_Client_Aircraft_Reference.cliacref_jetnet_ac_id = r("cliacref_jetnet_ac_id")
              aclsInsert_Client_Aircraft_Reference.cliacref_cliac_id = r("cliacref_cliac_id")
              aclsInsert_Client_Aircraft_Reference.cliacref_operator_flag = CStr(IIf(Not IsDBNull(r("cliacref_operator_flag")), r("cliacref_operator_flag"), ""))
              aclsInsert_Client_Aircraft_Reference.cliacref_owner_percentage = CStr(IIf(Not IsDBNull(r("cliacref_owner_percentage")), r("cliacref_owner_percentage"), "0"))
              aclsInsert_Client_Aircraft_Reference.cliacref_business_type = CStr(IIf(Not IsDBNull(r("cliacref_business_type")), r("cliacref_business_type"), ""))
              aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_expires = Now()
              aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_purchased = Now()

              If aclsData_Temp.Update_Client_Aircraft_Reference(aclsInsert_Client_Aircraft_Reference) = True Then
                ' Response.Write("Update Class Info: " & aclsInsert_Client_Aircraft_Reference.ClassInfo(aclsInsert_Client_Aircraft_Reference) & "<br /><hr />")
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("Company_Edit_Template.ascx.vb - combine_me_Click() - " & error_string)
                End If
                display_error()
              End If


            Next
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("Company_Edit_Template.ascx.vb - combine_me_Click() - " & error_string)
          End If
          display_error()
        End If

        '5.) Search for all info(0) contacts.

        aTempTable = aclsData_Temp.GetContacts(info(0), "CLIENT", "Y", 0)
        ' check the state of the DataTable
        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            For Each r As DataRow In aTempTable.Rows

              aTempTable2 = aclsData_Temp.GetContacts_Details(r("contact_id"), "CLIENT")
              If Not IsNothing(aTempTable2) Then
                If aTempTable2.Rows.Count > 0 Then
                  For Each q As DataRow In aTempTable2.Rows

                    Dim aclsClient_Contact As New clsClient_Contact
                    'GridView1.DataSource = aTempTable2
                    'GridView1.DataBind()
                    aclsClient_Contact.clicontact_preferred_name = CStr(IIf(Not IsDBNull(q("contact_preferred_name")), q("contact_preferred_name"), ""))
                    aclsClient_Contact.clicontact_email_list = CStr(IIf(Not IsDBNull(q("contact_email_list")), q("contact_email_list"), ""))
                    aclsClient_Contact.clicontact_user_id = Session.Item("localUser").crmLocalUserID
                    aclsClient_Contact.clicontact_notes = CStr(IIf(Not IsDBNull(q("contact_notes")), q("contact_notes"), ""))
                    aclsClient_Contact.clicontact_id = q("contact_id")
                    aclsClient_Contact.clicontact_comp_id = Session("ListingID")
                    aclsClient_Contact.clicontact_sirname = CStr(IIf(Not IsDBNull(q("contact_sirname")), q("contact_sirname"), ""))
                    aclsClient_Contact.clicontact_first_name = CStr(IIf(Not IsDBNull(q("contact_first_name")), q("contact_first_name"), ""))
                    aclsClient_Contact.clicontact_middle_initial = CStr(IIf(Not IsDBNull(q("contact_middle_initial")), q("contact_middle_initial"), ""))
                    aclsClient_Contact.clicontact_last_name = CStr(IIf(Not IsDBNull(q("contact_last_name")), q("contact_last_name"), ""))
                    aclsClient_Contact.clicontact_suffix = CStr(IIf(Not IsDBNull(q("contact_suffix")), q("contact_suffix"), ""))
                    aclsClient_Contact.clicontact_title = CStr(IIf(Not IsDBNull(q("contact_title")), q("contact_title"), ""))
                    aclsClient_Contact.clicontact_email_address = CStr(IIf(Not IsDBNull(q("contact_email_address")), q("contact_email_address"), ""))
                    aclsClient_Contact.clicontact_status = CStr(IIf(Not IsDBNull(q("clicontact_status")), q("clicontact_status"), ""))
                    aclsClient_Contact.clicontact_date_updated = Now()
                    aclsClient_Contact.clicontact_jetnet_contact_id = q("contact_jetnet_contact_id")

                    If aclsData_Temp.Update_Client_Contact(aclsClient_Contact) = True Then
                      'this is updated record for client. phone numbers deleted and readded. company not touched. 

                      'remove_contact_insert_contact_numbers(comp_id.Text, ID, remove_phone)
                      aTempTable2 = aclsData_Temp.GetPhoneNumbers(info(0), r("contact_id"), "CLIENT", 0)
                      '' check the state of the DataTable
                      If Not IsNothing(aTempTable2) Then
                        If aTempTable2.Rows.Count > 0 Then
                          ' set it to the datagrid 
                          For Each t As DataRow In aTempTable2.Rows
                            Dim aclsClient_Phone_Numbers As New clsClient_Phone_Numbers
                            aclsClient_Phone_Numbers.clipnum_type = CStr(IIf(Not IsDBNull(t("pnum_type")), t("pnum_type"), ""))
                            aclsClient_Phone_Numbers.clipnum_number = CStr(IIf(Not IsDBNull(t("pnum_number")), t("pnum_number"), ""))
                            aclsClient_Phone_Numbers.clipnum_id = t("clipnum_id")
                            aclsClient_Phone_Numbers.clipnum_comp_id = Session("ListingID")
                            aclsClient_Phone_Numbers.clipnum_contact_id = r("contact_id")
                            If aclsData_Temp.Update_Client_PhoneNumbers(aclsClient_Phone_Numbers) = True Then
                              'Response.Write("insert")
                            Else
                              If aclsData_Temp.class_error <> "" Then
                                error_string = aclsData_Temp.class_error
                                LogError("Company_Edit_Template.ascx.vb - combine_me_Click() - " & error_string)
                              End If
                              display_error()
                            End If
                          Next
                        End If
                      End If

                    Else
                      If aclsData_Temp.class_error <> "" Then
                        error_string = aclsData_Temp.class_error
                        LogError("Company_Edit_Template.ascx.vb - combine_me_Click() - " & error_string)
                      End If
                      display_error()
                    End If
                  Next
                End If
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("Company_Edit_Template.ascx.vb - combine_me_Click() - " & error_string)
                End If
                display_error()
              End If
            Next
          End If

        End If

        '6.) If found, replace all contact_comp_id with session("listingID")


        '7) Delete client company 

        If aclsData_Temp.Delete_Client_Company(info(0)) = True Then
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location= 'details.aspx';", True)
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
        End If

      End If
    Catch ex As Exception
      error_string = "company_edit_template.aspx.vb - combine_me_click() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub

  Private Sub connect_company_search_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles connect_company_search.Click, parent_search.Click
    Try
      Dim active As New DropDownList
      Dim acttext As New TextBox
      Dim attLab As New Label
      Dim client_string As String = ""
      Dim jetnet_string As String = ""
      client_string = "CLICOMP_NAME AS ""COMP_NAME"",clicomp_id as ""comp_id"", clicomp_address1 as comp_address1, clicomp_address2 as comp_address2, clicomp_state as comp_state, 'CLIENT' as source, clicomp_city as comp_city, clicomp_country as comp_country, clicomp_zip_code as comp_zip_code "
      jetnet_string = ""

      If Trim(Request("connect")) = "true" Then
        active = connect_company_list
        acttext = connect_company
        attLab = Attention_connect
        aTempTable = aclsData_Temp.Company_Search("%" & clsGeneral.clsGeneral.Get_Name_Search_String(acttext.Text) & "%", "Y", "J", connect_country.Text, connect_state.Text, "", Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmCommercial_Flag, "", "", "", "", connect_zip.Text, connect_city.Text, connect_address.Text)
      ElseIf Trim(Request("main_location")) = "true" Then
        active = parent_list
        acttext = parent_search_text

        aTempTable = aclsData_Temp.Export_All("COMP_NAME", client_string, jetnet_string, True, False, False, False, "C", "", "", "%" & clsGeneral.clsGeneral.StripChars(parent_search_text.Text, True) & "%", "Y", "", "", "", False, "%%", "%%", "", "", "2", "%", "", "", "", "", "", "", "", "", "", "", Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmJets_Flag, Session.Item("localSubscription").crmExecutive_Flag, Session.Item("localSubscription").crmTurboprops, Session.Item("localSubscription").crmCommercial_Flag, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "")

      End If

      Dim contact_text As String = ""

      active.Items.Clear()
      active.Items.Add(New ListItem("NONE SELECTED", "|"))
      active.Enabled = True

      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          For Each r As DataRow In aTempTable.Rows
            If Not IsDBNull(r("comp_name")) Then
              contact_text = r("comp_name") & " - "
            End If
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

            If r("comp_id") = CInt(Session("ListingID")) Then

            Else
              active.Items.Add(New ListItem(contact_text, r("comp_id") & "|" & r("source")))
            End If
          Next
        Else ' 0 rows
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("company_edit_template.aspx.vb - connect_company_search_Click() - " & error_string)
        End If
        display_error()
      End If
    Catch ex As Exception
      error_string = "company_edit_template.aspx.vb - connect_company_search_Click() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub

  Private Sub connect_company_list_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles connect_company_list.SelectedIndexChanged, parent_list.SelectedIndexChanged
    Try
      Dim active As New DropDownList
      Dim acttext As New Label
      Dim lab As New Label
      If Trim(Request("connect")) = "true" Then
        active = connect_company_list
        acttext = connect_company_details
        lab = Attention_connect
      ElseIf Trim(Request("main_location")) = "true" Then
        active = parent_list
        acttext = parent_company_details

      End If


      If active.SelectedValue <> "" Then
        Dim info As Array = Split(active.SelectedValue, "|")
        If UBound(info) <> 1 Then
          lab.Text = "<p align='center'>Please select a company to relate.</p>"
        Else
          acttext.Text = DisplayCompanyForCombine(info(0), info(1))
        End If
      Else
        lab.Text = "<p align='center'>Please select a company to relate.</p>"
      End If
      If Trim(Request("connect")) = "true" Then
        connect_me.Visible = True
      ElseIf Trim(Request("main_location")) = "true" Then
        add_parent.Visible = True
      End If

    Catch ex As Exception
      error_string = "company_edit_template.aspx.vb - combine_company_list_SelectedIndexChanged() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub


  Private Sub connect_me_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles connect_me.Click, add_parent.Click
    Try
      Dim aclsClient_Company As New clsClient_Company
      Dim id As Integer = CInt(Session.Item("ListingID"))
      Dim active As New DropDownList
      Dim acttext As New Label
      If Trim(Request("connect")) = "true" Then
        active = connect_company_list
        acttext = Attention_connect
      ElseIf Trim(Request("main_location")) = "true" Then
        active = parent_list
        acttext = attention_parent
      End If


      If active.SelectedValue <> "" And active.SelectedValue <> "|" Then
        Dim info As Array = Split(active.SelectedValue, "|")
        If UBound(info) <> 1 Then
          acttext.Text = "<p align='center'>Please select a company to relate.</p>"
        Else
          aTempTable = aclsData_Temp.GetCompanyInfo_ID(id, "CLIENT", 0)
          ' check the state of the DataTable
          If Not IsNothing(aTempTable) Then
            If aTempTable.Rows.Count > 0 Then

              For Each R As DataRow In aTempTable.Rows
                aclsClient_Company.clicomp_user_id = Session.Item("localUser").crmLocalUserID
                aclsClient_Company.clicomp_id = id

                aclsClient_Company.clicomp_name = CStr(IIf(Not IsDBNull(R("comp_name")), R("comp_name"), ""))
                aclsClient_Company.clicomp_alternate_name_type = CStr(IIf(Not IsDBNull(R("comp_alternate_name_type")), R("comp_alternate_name_type"), ""))
                aclsClient_Company.clicomp_alternate_name = CStr(IIf(Not IsDBNull(R("comp_alternate_name")), R("comp_alternate_name"), ""))
                aclsClient_Company.clicomp_address1 = CStr(IIf(Not IsDBNull(R("comp_address1")), R("comp_address1"), ""))
                aclsClient_Company.clicomp_address2 = CStr(IIf(Not IsDBNull(R("comp_address2")), R("comp_address2"), ""))
                aclsClient_Company.clicomp_city = CStr(IIf(Not IsDBNull(R("comp_city")), R("comp_city"), ""))
                aclsClient_Company.clicomp_state = CStr(IIf(Not IsDBNull(R("comp_state")), R("comp_state"), ""))
                aclsClient_Company.clicomp_zip_code = CStr(IIf(Not IsDBNull(R("comp_zip_code")), R("comp_zip_code"), ""))
                aclsClient_Company.clicomp_country = CStr(IIf(Not IsDBNull(R("comp_country")), R("comp_country"), ""))
                aclsClient_Company.clicomp_agency_type = CStr(IIf(Not IsDBNull(R("comp_agency_type")), R("comp_agency_type"), ""))
                aclsClient_Company.clicomp_web_address = CStr(IIf(Not IsDBNull(R("comp_web_address")), R("comp_web_address"), ""))
                aclsClient_Company.clicomp_email_address = CStr(IIf(Not IsDBNull(R("comp_email_address")), R("comp_email_address"), ""))
                aclsClient_Company.clicomp_date_updated = Now()
                ' set to zero for now since i'm updating a client record

                aclsClient_Company.clicomp_status = CStr(IIf(Not IsDBNull(R("clicomp_status")), R("clicomp_status"), ""))
                aclsClient_Company.clicomp_description = CStr(IIf(Not IsDBNull(R("clicomp_description")), R("clicomp_description"), ""))

                aclsClient_Company.clicomp_category1 = CStr(IIf(Not IsDBNull(R("clicomp_category1")), R("clicomp_category1"), ""))
                aclsClient_Company.clicomp_category2 = CStr(IIf(Not IsDBNull(R("clicomp_category2")), R("clicomp_category2"), ""))

                aclsClient_Company.clicomp_category3 = CStr(IIf(Not IsDBNull(R("clicomp_category3")), R("clicomp_category3"), ""))
                aclsClient_Company.clicomp_category4 = CStr(IIf(Not IsDBNull(R("clicomp_category4")), R("clicomp_category4"), ""))
                aclsClient_Company.clicomp_category5 = CStr(IIf(Not IsDBNull(R("clicomp_category5")), R("clicomp_category5"), ""))
                'This is weird, we're going to need to keep the jetnet comp_id linked to the contact, right?
                If Trim(Request("connect")) = "true" Then
                  aclsClient_Company.clicomp_jetnet_comp_id = CInt(info(0))
                  aclsClient_Company.clicomp_mainloc_comp_id = CInt(IIf(Not IsDBNull(R("clicomp_mainloc_comp_id")), R("clicomp_mainloc_comp_id"), 0))
                  If aclsData_Temp.COMP_Update_Note_When_Jetnet_Made_Client(id, info(0)) = 1 Then
                  End If
                ElseIf Trim(Request("main_location")) = "true" Then
                  aclsClient_Company.clicomp_mainloc_comp_id = CInt(info(0))
                  aclsClient_Company.clicomp_jetnet_comp_id = CInt(IIf(Not IsDBNull(R("jetnet_comp_id")), R("jetnet_comp_id"), 0))
                End If

                'Then once that is done we're removing the client phone numbers associated with this company. 
                If aclsData_Temp.Update_Client_Company(aclsClient_Company) = True Then
                  System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location= 'details.aspx';", True)
                  System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
                Else
                  If aclsData_Temp.class_error <> "" Then
                    error_string = aclsData_Temp.class_error
                    LogError("Company_Edit_Template.ascx.vb - connect_me_Click() - " & error_string)
                  End If
                  display_error()
                End If
              Next
            End If
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = aclsData_Temp.class_error
              LogError("Company_Edit_Template.ascx.vb - connect_me_Click() - " & error_string)
            End If
            display_error()
          End If
        End If
      Else
        acttext.Text = "<p align='center'>Please select a company to relate.</p>"
      End If
    Catch ex As Exception
      error_string = "Company_Edit_Template.ascx.vb - connect_me_Click() " & ex.Message
      LogError(error_string)
    End Try
  End Sub

  Private Sub connect_remove_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles connect_remove.Click
    Try
      Dim aclsClient_Company As New clsClient_Company
      Dim id As Integer = CInt(Session.Item("ListingID"))

      aTempTable = aclsData_Temp.GetCompanyInfo_ID(id, "CLIENT", 0)
      ' check the state of the DataTable
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then

          For Each R As DataRow In aTempTable.Rows
            aclsClient_Company.clicomp_user_id = Session.Item("localUser").crmLocalUserID
            aclsClient_Company.clicomp_id = id
            aclsClient_Company.clicomp_name = CStr(IIf(Not IsDBNull(R("comp_name")), R("comp_name"), ""))
            aclsClient_Company.clicomp_alternate_name_type = CStr(IIf(Not IsDBNull(R("comp_alternate_name_type")), R("comp_alternate_name_type"), ""))
            aclsClient_Company.clicomp_alternate_name = CStr(IIf(Not IsDBNull(R("comp_alternate_name")), R("comp_alternate_name"), ""))
            aclsClient_Company.clicomp_address1 = CStr(IIf(Not IsDBNull(R("comp_address1")), R("comp_address1"), ""))
            aclsClient_Company.clicomp_address2 = CStr(IIf(Not IsDBNull(R("comp_address2")), R("comp_address2"), ""))
            aclsClient_Company.clicomp_city = CStr(IIf(Not IsDBNull(R("comp_city")), R("comp_city"), ""))
            aclsClient_Company.clicomp_state = CStr(IIf(Not IsDBNull(R("comp_state")), R("comp_state"), ""))
            aclsClient_Company.clicomp_zip_code = CStr(IIf(Not IsDBNull(R("comp_zip_code")), R("comp_zip_code"), ""))
            aclsClient_Company.clicomp_country = CStr(IIf(Not IsDBNull(R("comp_country")), R("comp_country"), ""))
            aclsClient_Company.clicomp_agency_type = CStr(IIf(Not IsDBNull(R("comp_agency_type")), R("comp_agency_type"), ""))
            aclsClient_Company.clicomp_web_address = CStr(IIf(Not IsDBNull(R("comp_web_address")), R("comp_web_address"), ""))
            aclsClient_Company.clicomp_email_address = CStr(IIf(Not IsDBNull(R("comp_email_address")), R("comp_email_address"), ""))
            aclsClient_Company.clicomp_date_updated = Now()
            ' set to zero for now since i'm updating a client record
            aclsClient_Company.clicomp_jetnet_comp_id = 0
            aclsClient_Company.clicomp_mainloc_comp_id = CInt(IIf(Not IsDBNull(R("clicomp_mainloc_comp_id")), R("clicomp_mainloc_comp_id"), 0))
            aclsClient_Company.clicomp_status = CStr(IIf(Not IsDBNull(R("clicomp_status")), R("clicomp_status"), ""))
            aclsClient_Company.clicomp_description = CStr(IIf(Not IsDBNull(R("clicomp_description")), R("clicomp_description"), ""))

            aclsClient_Company.clicomp_category1 = CStr(IIf(Not IsDBNull(R("clicomp_category1")), R("clicomp_category1"), ""))
            aclsClient_Company.clicomp_category2 = CStr(IIf(Not IsDBNull(R("clicomp_category2")), R("clicomp_category2"), ""))

            aclsClient_Company.clicomp_category3 = CStr(IIf(Not IsDBNull(R("clicomp_category3")), R("clicomp_category3"), ""))
            aclsClient_Company.clicomp_category4 = CStr(IIf(Not IsDBNull(R("clicomp_category4")), R("clicomp_category4"), ""))
            aclsClient_Company.clicomp_category5 = CStr(IIf(Not IsDBNull(R("clicomp_category5")), R("clicomp_category5"), ""))
            'This is weird, we're going to need to keep the jetnet comp_id linked to the contact, right?


            'Then once that is done we're removing the client phone numbers associated with this company. 
            If aclsData_Temp.Update_Client_Company(aclsClient_Company) = True Then
              System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location= 'details.aspx';", True)
              System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
            Else
              If aclsData_Temp.class_error <> "" Then
                error_string = aclsData_Temp.class_error
                LogError("Company_Edit_Template.ascx.vb - connect_remove_Click() - " & error_string)
              End If
              display_error()
            End If
          Next
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("Company_Edit_Template.ascx.vb - connect_remove_Click() - " & error_string)
        End If
        display_error()
      End If

    Catch ex As Exception
      error_string = "Company_Edit_Template.ascx.vb - connect_me_Click() " & ex.Message
      LogError(error_string)
    End Try
  End Sub


  Private Sub Remove_Company()
    Session.Item("Listing") = 1
    Session.Item("FromTypeOfListing") = 1
    If Not IsNothing(Session.Item("ListingID")) Then
      If Not String.IsNullOrEmpty(Session.Item("ListingID").ToString) Then
        If IsNumeric(Session.Item("ListingID")) Then
          aclsData_Temp.Remove_Client_Company(Session.Item("ListingID"))
        End If
      End If
    End If
    Session.Item("ListingID") = 0
    Dim url As String = "listing.aspx?removed=true"
    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location = '" & url & "';", True)
    System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
  End Sub

  Private Sub synchronize_button_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles synchronize_button.Click
    Try
      Dim itemCount As Integer
      Dim updated_string As String = "" 'list of items updated
      itemCount = synch_list.Items.Count
      For i = 0 To (itemCount - 1)
        If synch_list.Items(i).Selected Then
          Select Case synch_list.Items(i).Value
            Case "General/Location/Status"
              updated_string = "General/Location/Status, "
              'update the general company record. 
              Synch_Location(Session.Item("ListingID"), Session.Item("OtherID"))
            Case "Phone Numbers"
              updated_string = updated_string & "Phone Numbers, "
              Synch_Phone(Session.Item("ListingID"), Session.Item("OtherID"))
            Case "Contacts"
              updated_string = updated_string & "Contacts, "
              Synch_Contacts(Session.Item("ListingID"), Session.Item("OtherID"))
          End Select
        End If
      Next i
      If updated_string <> "" Then
        updated_string = Trim(updated_string)
        updated_string = UCase(updated_string.TrimEnd(","))
      End If
      synch_note.Text = "<p class='alert_box'>Your record has been updated in the following areas: " & updated_string & "</p>"

      System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location.reload();", True)
    Catch ex As Exception
      error_string = "Company Edit Template Synchronize Button Click() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Private Sub synch_list_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles synch_list.SelectedIndexChanged
    synchronize_button.Visible = False
    Dim itemCount As Integer
    itemCount = synch_list.Items.Count
    For i = 0 To (itemCount - 1)
      If synch_list.Items(i).Selected Then
        synchronize_button.Visible = True
      End If
    Next i
  End Sub
#Region "Synch Functions"
  Private Sub Synch_Phone(ByVal client_id As Integer, ByVal jetnet_id As Integer)
    Dim aInt As Integer = 0
    aInt = aclsData_Temp.DeletePhoneNumbers_compID(client_id)
    ' check the state of the DataTable
    If aInt > 0 Then
      'inserting the c phone numbers:
    End If
    display_error()

    Dim aclsClient_Phone_Numbers As New clsClient_Phone_Numbers

    Dim atemptable As New DataTable
    atemptable = aclsData_Temp.GetPhoneNumbers(jetnet_id, 0, "JETNET", 0)
    For Each r As DataRow In atemptable.Rows
      aclsClient_Phone_Numbers.clipnum_type = IIf(Not IsDBNull(r("pnum_type")), r("pnum_type"), "")
      aclsClient_Phone_Numbers.clipnum_number = IIf(Not IsDBNull(r("pnum_number")), r("pnum_number"), "")
      aclsClient_Phone_Numbers.clipnum_comp_id = client_id
      aclsClient_Phone_Numbers.clipnum_contact_id = IIf(Not IsDBNull(r("pnum_contact_id")), r("pnum_contact_id"), 0)

      If aclsData_Temp.Insert_Client_PhoneNumbers(aclsClient_Phone_Numbers) = True Then
        ' Response.Write("insert phone")
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("Company_Edit_Template.ascx.vb - remove_company_insert_company() - " & error_string)
        End If
        display_error()
      End If
    Next
  End Sub
  Private Sub Synch_Contacts(ByVal client_id As Integer, ByVal jetnet_id As Integer)
    If aclsData_Temp.Remove_Client_Contact_ByCompany(client_id) = 1 Then
      'Carry on with the copying. 
      RaiseEvent loop_contacts(client_id, jetnet_id, 0, True, False)
    End If
  End Sub
  Private Sub Synch_Location(ByVal client_id As Integer, ByVal jetnet_id As Integer)
    Dim atemptable As New DataTable
    atemptable = aclsData_Temp.GetCompanyInfo_ID(jetnet_id, "JETNET", 0)
    For Each r As DataRow In atemptable.Rows
      Dim aclsClient_Company As New clsClient_Company
      aclsClient_Company.clicomp_user_id = Session.Item("localUser").crmLocalUserID
      aclsClient_Company.clicomp_id = client_id
      aclsClient_Company.clicomp_name = IIf(Not IsDBNull(r("comp_name")), r("comp_name"), "")
      aclsClient_Company.clicomp_alternate_name_type = IIf(Not IsDBNull(r("comp_alternate_name_type")), r("comp_alternate_name_type"), "")
      aclsClient_Company.clicomp_alternate_name = IIf(Not IsDBNull(r("comp_alternate_name")), r("comp_alternate_name"), "")
      aclsClient_Company.clicomp_address1 = IIf(Not IsDBNull(r("comp_address1")), r("comp_address1"), "")
      aclsClient_Company.clicomp_address2 = IIf(Not IsDBNull(r("comp_address2")), r("comp_address2"), "")
      aclsClient_Company.clicomp_city = IIf(Not IsDBNull(r("comp_city")), r("comp_city"), "")
      aclsClient_Company.clicomp_state = IIf(Not IsDBNull(r("comp_state")), r("comp_state"), "")
      aclsClient_Company.clicomp_zip_code = IIf(Not IsDBNull(r("comp_zip_code")), r("comp_zip_code"), "")
      aclsClient_Company.clicomp_country = IIf(Not IsDBNull(r("comp_country")), r("comp_country"), "")
      aclsClient_Company.clicomp_agency_type = IIf(Not IsDBNull(r("comp_agency_type")), r("comp_agency_type"), "")
      aclsClient_Company.clicomp_web_address = IIf(Not IsDBNull(r("comp_web_address")), r("comp_web_address"), "")
      aclsClient_Company.clicomp_email_address = IIf(Not IsDBNull(r("comp_email_address")), r("comp_email_address"), "")
      aclsClient_Company.clicomp_date_updated = Now()
      aclsClient_Company.clicomp_jetnet_comp_id = jetnet_id
      aclsClient_Company.clicomp_status = "Y"

      aTempTable2 = aclsData_Temp.GetCompanyInfo_ID(client_id, "CLIENT", 0)
      For Each q As DataRow In aTempTable2.Rows

        aclsClient_Company.clicomp_description = IIf(Not IsDBNull(q("clicomp_description")), q("clicomp_description"), "")

        aclsClient_Company.clicomp_category1 = IIf(Not IsDBNull(q("clicomp_category1")), q("clicomp_category1"), "")
        aclsClient_Company.clicomp_category2 = IIf(Not IsDBNull(q("clicomp_category2")), q("clicomp_category2"), "")

        aclsClient_Company.clicomp_category3 = IIf(Not IsDBNull(q("clicomp_category3")), q("clicomp_category3"), "")
        aclsClient_Company.clicomp_category4 = IIf(Not IsDBNull(q("clicomp_category4")), q("clicomp_category4"), "")
        aclsClient_Company.clicomp_category5 = IIf(Not IsDBNull(q("clicomp_category5")), q("clicomp_category5"), "")
      Next

      aclsClient_Company.clicomp_mainloc_comp_id = 0

      If aclsData_Temp.Update_Client_Company(aclsClient_Company) = True Then
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("Company_Edit_Template.ascx.vb - Synch Location() - " & error_string)
        End If
        display_error()
      End If

    Next
  End Sub
#End Region

  Private Sub deleteFunction_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles deleteFunction.Click
    Remove_Company()
  End Sub
End Class