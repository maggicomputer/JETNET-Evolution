Imports System.IO
Imports crmWebClient.clsGeneral
Partial Public Class Aircraft_Edit_Transactions_Tab
  Inherits System.Web.UI.UserControl
  Dim aclsData_Temp As New clsData_Manager_SQL
  Dim aTempTable, aTempTable2 As New DataTable 'Data Tables used
  Public Event loop_contacts(ByVal idnum_new As Integer, ByVal jetnet_id As Integer, ByVal contact_id As Integer, ByVal skip As Boolean, ByVal inactive As Boolean)
  Dim error_string As String = ""
  Dim ValueViewRecentSalesJetnetID As Long = 0
  Dim ValueViewRecentSalesClientID As Long = 0
  Dim cleared_sale As Boolean = False
  Dim AssumeID As Long = 0
#Region "Page Events"

  Public Sub RemoveTransaction()
    aircraft_edit.Visible = False
    Dim orgid As Integer = CInt((Trim(Request("cli_trans"))))
    If aclsData_Temp.Delete_Client_Transactions(orgid) = 1 Then

      aTempTable = aclsData_Temp.Get_Client_Transactions_aircraft_reference_clitcref_client_trans_id(orgid)

      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          For Each r As DataRow In aTempTable.Rows
            aclsData_Temp.Delete_Client_Transactions_aircraft_reference(r("clitcref_id"))
          Next
        End If
      End If
      System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location.reload();", True)
      System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
    End If
  End Sub
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    If Session.Item("crmUserLogon") <> True Then
      Response.Redirect("Default.aspx", False)
    Else
      If Me.Visible Then
        Try

          If Not String.IsNullOrEmpty(Trim(Request("acID"))) Then
            If IsNumeric(Trim(Request("acID"))) Then
              Session.Item("ListingID") = Trim(Request("acID"))
            End If
          End If

          Me.share_box.Visible = True


          If Not String.IsNullOrEmpty(Trim(Request("activetab"))) Then
            Session.Item("ViewActiveTab") = Trim(Request("activetab"))
          End If

          If Not String.IsNullOrEmpty(Trim(Request("source"))) Then
            Session.Item("ListingSource") = Trim(Request("source"))
          End If

          aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
          aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase") 'CApplication.Item("crmJetnetDatabase")

          'If HttpContext.Current.Session.Item("localUser").crmUser_Evo_MPM_Flag = True Then
          '  If aclsData_Temp.client_DB = "" Then
          '    aclsData_Temp.client_DB = HttpContext.Current.Session.Item("localPreferences").ServerNotesDatabaseConn
          '  End If
          'End If

          If Trim(Request("remove")) = "true" Then
            RemoveTransaction()
          Else
            If Trim(Request("new")) = "true" Then
              insert_row.Visible = False
              reference_info.Visible = False
            Else
              If Trim(Request("add")) = "ref" Then
                ref_adding.Visible = True
              End If
              reference_info.Visible = True
              insert_row.Visible = True

            End If


            If Not Page.IsPostBack Then
              bind_data()
            End If


            If Trim(Request("auto_trans")) = "true" Then
              If Trim(Request("from")) = "view" Then
                If Trim(Request("source")) = "JETNET" Then
                  If IsNumeric(Trim(Request("jac_id"))) Then
                    ValueViewRecentSalesJetnetID = Request("jac_id")

                    Dim TemporaryClientAC As New DataTable
                    If ValueViewRecentSalesJetnetID > 0 Then
                      TemporaryClientAC = aclsData_Temp.Get_Client_Aircraft_JETNET_AC(ValueViewRecentSalesJetnetID)
                      If Not IsNothing(TemporaryClientAC) Then
                        If TemporaryClientAC.Rows.Count > 0 Then
                          ValueViewRecentSalesClientID = TemporaryClientAC.Rows(0).Item("cliaircraft_id")
                        End If
                      End If
                    End If
                  End If
                Else
                  If IsNumeric(Trim(Request("compare_ac_id"))) Then
                    ValueViewRecentSalesClientID = Request("compare_ac_id")
                    Dim TemporaryJetnetAC As New DataTable
                    If ValueViewRecentSalesClientID > 0 Then
                      TemporaryJetnetAC = aclsData_Temp.Get_Clients_Aircraft(ValueViewRecentSalesClientID)
                      If Not IsNothing(TemporaryJetnetAC) Then
                        If TemporaryJetnetAC.Rows.Count > 0 Then
                          ValueViewRecentSalesJetnetID = TemporaryJetnetAC.Rows(0).Item("cliaircraft_jetnet_ac_id")
                        End If
                      End If
                    End If
                  End If

                End If
              End If



              update_Click()

            End If
          End If
        Catch ex As Exception
          error_string = "Aircraft_Edit_Template.ascx.vb - Page_Load() - " & ex.Message
          LogError(error_string)
        End Try
      End If
    End If
  End Sub
#End Region
#Region "Data Grid Functions"
  Sub client_bind_data()
    Try
      Dim cli_trans_id As Integer = 0
      Try
        cli_trans_id = CInt(Trim(Request("cli_trans")))
      Catch
        cli_trans_id = 0
      End Try

      removeButton.Visible = True
      aTempTable2 = aclsData_Temp.Get_Client_Transactions_aircraft_reference_clitcref_client_trans_id(cli_trans_id)
      If Not IsNothing(aTempTable2) Then
        If aTempTable2.Rows.Count > 0 Then
          datagrid2.DataSource = aTempTable2
          datagrid2.DataBind()

          'If q("clitcref_contact_type") = "00" Or q("clitcref_contact_type") = "36" Or q("clitcref_contact_type") = "96" Then
          '    buyer_info.Text = (displayCompanyContact(q("clitcref_client_comp_id"), q("clitcref_client_contact_id"), True))
          'End If

          'If q("clitcref_contact_type") = "95" Then
          '    seller_info.Text = (displayCompanyContact(q("clitcref_client_comp_id"), q("clitcref_client_contact_id"), True))
          'End If

          'If q("clitcref_contact_type") = "62" Then
          '    reg_info.Text = (displayCompanyContact(q("clitcref_client_comp_id"), q("clitcref_client_contact_id"), True))
          'End If


        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - client_bind_data() - " & error_string)
        End If
        displayError()
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Transaction_Tab.ascx.vb - client_bind_data() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Public Sub MyDataGrid_Update(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
    Try
      Dim company As DropDownList = e.Item.FindControl("company")
      Dim company_array As Array = Split(company.SelectedValue, "|")
      'company_array(0) is company ID
      'company_array(1) is source

      Dim client_trans_id As Integer = CInt(Trim(Request("cli_trans")))
      Dim contact As DropDownList = e.Item.FindControl("contact")
      Dim contact_array As Array = Split(contact.SelectedValue, "|")
      'contact_array(0) is contact ID
      'contact_array(1) is source

      Dim company_id As Integer = 0
      company_id = company_array(0)

      Dim contact_id As Integer = 0
      Try
        contact_id = contact_array(0)
      Catch
        contact_id = 0
      End Try

      Dim ref_id As TextBox = e.Item.FindControl("id")
      Dim contact_type As DropDownList = e.Item.FindControl("contact_type")

      'Response.Write(contact_type.SelectedValue & "!!!!!!!!!!!")
      'First we have to determine if the company is a client record or jetnet record.
      If company_array(1) = "JETNET" Then
        Dim errored As String = ""
        'We have to go through a perform a bazillion checks here. 
        '3.) Take that jetnet company ID and poll it against our client database. 
        aTempTable = aclsData_Temp.GetCompanyInfo_JETNET_ID(company_id, errored)

        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then

            'Now check and see if the client company ID exists in the transaction table for this record
            aTempTable2 = aclsData_Temp.Get_Client_Transactions_Company(aTempTable.Rows(0).Item("comp_id"), client_trans_id)

            Dim datatable As New DataTable
            datatable = aTempTable2.Clone
            'Filter that transaction company table based only on the ones with that transaction ID. 
            'This really needs to match the CLIENT TRANSACTION ID NOT JETNET. MUST CHANGE
            Dim afileterd As DataRow() = aTempTable2.Select("clitcomp_trans_id = '" & journ_id.Text & "' ", "clitcomp_id")

            For Each z As DataRow In afileterd
              datatable.ImportRow(z)
            Next

            If datatable.Rows.Count > 0 Then
              'this is if the client company exists in the transaction record
              'Response.Write("<br />Don't make a copy of this!!!!!" & datatable.Rows(0).Item("clitcomp_id") & "<br />")
            Else
              aTempTable2 = aclsData_Temp.Get_Client_Transactions_Company(aTempTable.Rows(0).Item("comp_id"), client_trans_id)
              '---------------------------CLIENT TRANSACTION COMPANY-----------------------------------------
              If Not IsNothing(aTempTable2) Then
                If aTempTable2.Rows.Count = 0 Then
                  'This means that a client copy of this already exists in the database.
                  ' Response.Write("<br />Not found!!! Make a copy!! " & aTempTable.Rows(0).Item("comp_id") & "<br />")
                  Dim Client_Company_ID As Integer = aTempTable.Rows(0).Item("comp_id")
                  '---------4.) Store all of the info for that transaction company in the client transaction company database.
                  Fill_Transaction_Company_FromJETNET(Client_Company_ID, journ_id.Text, company_array(0), client_trans_id, "JETNET")
                  '---------5.) Store all of the related contacts to that company in the transaction related database.
                  Fill_Transaction_Contacts_FromJETNET(Client_Company_ID, journ_id.Text, company_array(0), client_trans_id, "JETNET")
                Else
                  Dim Client_Company_ID As Integer = aTempTable2.Rows(0).Item("clitcomp_id")
                End If

              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - MyDataGrid_Update() - " & error_string)
                End If
                displayError()
              End If
            End If
          Else
            ' Response.Write("<br />Not here!" & "<br />")

            '-----If the client copy doesn't exist
            '---------4.) Store all of the information for the transaction company into the client COMPANY database.
            '---------4.) b. Store all of the related company phone numbers to that transaction in the client phone number database.
            Dim Client_Company_ID As Integer = 0
            Client_Company_ID = Fill_Client_Company_FromJETNET(company_id, journ_id.Text)
            'Client_Company_ID = 11838 'hard coded for now for testing and coding purposes
            '---------5.) Store all of the information for that transaction company into the client company TRANSACTION database.
            '---------5.) b. Store all of the related company phone numbers to that transaction in the phone number transaction database.
            Fill_Transaction_Company_FromJETNET(Client_Company_ID, journ_id.Text, company_id, client_trans_id, "JETNET")
            '---------6.) Store all of the information for the related contacts to that company into the client CONTACT database. 
            '---------6.) b. Also add the contact phone numbers into the client database. 
            Fill_Client_Contacts_FromJETNET(company_id, Client_Company_ID, journ_id.Text)
            '---------7.) Store all of the information for the related contacts to that company into the client contact TRANSACTION database.
            Fill_Transaction_Contacts_FromJETNET(Client_Company_ID, journ_id.Text, company_id, client_trans_id, "JETNET")
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - MyDataGrid_Update() - " & error_string)
          End If
          displayError()
        End If


      End If

      Fill_SINGLE_AC_Transaction_Reference(contact_type.SelectedValue, company_id, contact_id, ref_id.Text, True, False, client_trans_id)
      client_bind_data()
    Catch ex As Exception
      error_string = "Aircraft_Edit_Transaction_Tab.ascx.vb - MyDataGrid_Update() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Sub MyDataGrid_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
    Try
      datagrid2.EditItemIndex = CInt(E.Item.ItemIndex)
      client_bind_data()
      datagrid2.DataBind()
      new_row.Visible = False
      insert_row.Visible = False
      buttons.Visible = False

    Catch ex As Exception
      error_string = "Aircraft_Edit_Transaction_Tab.ascx.vb - MyDataGrid_Edit() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub

  Sub MyDataGrid_Cancel(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
    Try
      datagrid2.EditItemIndex = -1

      client_bind_data()
      datagrid2.DataBind()
      new_row.Visible = False
      insert_row.Visible = True
      buttons.Visible = True
    Catch ex As Exception
      error_string = "Aircraft_Edit_Transaction_Tab.ascx.vb - MyDataGrid_Edit() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub

  Public Sub MyDataGrid_Delete(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
    Try
      Dim id As TextBox = e.Item.FindControl("id")
      Dim id_num As Integer = CInt(id.Text)
      Dim orgid As Integer = CInt((Trim(Request("cli_trans"))))
      Dim comp_id As Integer = 0
      'Dim contact_id As Integer = 0
      'first figure out what this holds.
      aTempTable = aclsData_Temp.Get_Client_Transactions_aircraft_reference_clitcref_id(id_num)

      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          comp_id = aTempTable.Rows(0).Item("clitcref_client_comp_id")
          'contact_id = aTempTable.Rows(0).Item("clitcref_client_contact_id")
        End If
      End If

      'Response.Write(comp_id & " " & contact_id)
      aTempTable = aclsData_Temp.Get_Client_Transactions_aircraft_reference_Company_Exists(orgid, comp_id)
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count = 1 Then
          'need to remove old company.
          If aclsData_Temp.Delete_Client_Transactions_Company(comp_id, orgid) = 1 Then
            'done
          End If

          'If contact_id <> 0 Then
          If aclsData_Temp.Delete_Client_Transactions_Contact_ByCompany(orgid, comp_id) = 1 Then
            'done
          End If
          'End If
        End If
      End If
      If aclsData_Temp.Delete_Client_Transactions_aircraft_reference(id_num) = 1 Then
        client_bind_data()
      End If

      new_row.Visible = False
      insert_row.Visible = True
      buttons.Visible = True

    Catch ex As Exception
      error_string = "Aircraft_Edit_Transaction_Tab.ascx.vb - MyDataGrid_Delete() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Private Sub datagrid2_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles datagrid2.ItemDataBound
    Try
      Dim comp_info As String = ""
      Dim displayContact As String = ""

      Dim cli_trans_id As Integer = 0
      If Trim(Request("cli_trans")) <> "" Then
        cli_trans_id = CInt(Trim(Request("cli_trans")))
      End If
      Dim sel As TextBox = e.Item.FindControl("type_hidden")
      If Not IsNothing(e.Item.FindControl("contact_type")) Then
        Dim ddl As DropDownList = e.Item.FindControl("contact_type")
        aTempTable = aclsData_Temp.Get_Client_Aircraft_Contact_Type

        For Each r As DataRow In aTempTable.Rows
          ddl.Items.Add(New ListItem(r("cliact_name"), r("cliact_type")))
        Next
        ddl.SelectedValue = sel.Text
      End If

      Dim contact As New TextBox
      Dim company As New TextBox
      Dim cont_info As String = ""
      If Not IsNothing(e.Item.FindControl("contact_hidden")) Then
        contact = e.Item.FindControl("contact_hidden")

        '--------------------------------------------------CLIENT CONTACT TABLE-----------------------------------------------------------------------------
        aTempTable = aclsData_Temp.Get_Client_Transactions_Contact_ContactID(contact.Text, cli_trans_id)
        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            For Each q In aTempTable.Rows
              If Not IsDBNull(q("clitcontact_sirname")) Then
                cont_info = cont_info & q("clitcontact_sirname") & " "
              End If
              If Not IsDBNull(q("clitcontact_first_name")) Then
                cont_info = cont_info & q("clitcontact_first_name") & " "
              End If
              If Not IsDBNull(q("clitcontact_middle_initial")) Then
                cont_info = cont_info & q("clitcontact_middle_initial") & " "
              End If
              If Not IsDBNull(q("clitcontact_last_name")) Then
                cont_info = cont_info & q("clitcontact_last_name") & " "
              End If
              If Not IsDBNull(q("clitcontact_title")) Then
                cont_info = cont_info & "- " & q("clitcontact_title") & " "
              End If
            Next
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - datagrid2_ItemDataBound() - " & error_string)
          End If
          displayError()
        End If
      End If

      If Not IsNothing(e.Item.FindControl("company_hidden")) Then
        company = e.Item.FindControl("company_hidden")
        aTempTable2 = aclsData_Temp.Get_Client_Transactions_Company(company.Text, cli_trans_id)
        '---------------------------CLIENT TRANSACTION COMPANY-----------------------------------------
        If Not IsNothing(aTempTable2) Then
          If aTempTable2.Rows.Count > 0 Then
            For Each q In aTempTable2.Rows
              If Not IsDBNull(q("clitcomp_name")) Then
                comp_info = comp_info & q("clitcomp_name") & " "
              End If
              If Not IsDBNull(q("clitcomp_city")) Then
                comp_info = comp_info & q("clitcomp_city") & " "
              End If
              If Not IsDBNull(q("clitcomp_state")) Then
                comp_info = comp_info & q("clitcomp_state") & " "
              End If

            Next
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - datagrid2_ItemDataBound() - " & error_string)
          End If
          displayError()
        End If

      End If


      If Not IsNothing(e.Item.FindControl("company")) Then
        Dim company_list As DropDownList = e.Item.FindControl("company")
        company_list.Items.Add(New ListItem(comp_info & "(CLIENT record)", company.Text & "|CLIENT"))
        company_list.SelectedValue = company.Text & "|CLIENT"
      End If

      comp_info = ""

      If Not IsNothing(e.Item.FindControl("contact")) Then
        Dim contact_list As DropDownList = e.Item.FindControl("contact")
        contact_list.Items.Add(New ListItem(cont_info, contact.Text))

        contact_list.SelectedValue = contact.Text
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Transaction_Tab.ascx.vb - datagrid2_ItemDataBound() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Sub swap_company(ByVal sender As Object, ByVal e As EventArgs)
    Try
      Dim ddllist As DropDownList = CType(sender, DropDownList)
      Dim cell As TableCell = CType(ddllist.Parent, TableCell)
      Dim item As DataGridItem = CType(cell.Parent, DataGridItem)
      Dim contact_name As DropDownList = item.Cells(3).FindControl("contact")


      Dim company_name As New DropDownList
      company_name = item.Cells(2).FindControl("company")

      Dim info As Array = Split(company_name.SelectedValue, "|")


      Dim comp_id As Integer = CInt(info(0))
      contact_name.Items.Clear()
      aTempTable = aclsData_Temp.GetContacts(comp_id, info(1), "Y", 0)
      ' check the state of the DataTable
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          contact_name.Items.Clear()
          contact_name.Items.Add(New ListItem("NOT SELECTED", ""))
          For Each r As DataRow In aTempTable.Rows
            If r("contact_title") <> "" Then
              contact_name.Items.Add(New ListItem(CStr(r("contact_title") & " - " & r("contact_sirname") & " " & r("contact_first_name") & " " & r("contact_last_name") & " (" & r("contact_type") & " record) "), r("contact_id") & "|" & r("contact_type")))
            Else
              contact_name.Items.Add(New ListItem(CStr(r("contact_sirname") & " " & r("contact_first_name") & " " & r("contact_last_name") & " (" & r("contact_type") & " record)"), r("contact_id") & "|" & r("contact_type")))
            End If
          Next
        Else
          contact_name.Items.Add(New ListItem("NO ASSOCIATED CONTACTS", ""))
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - test() - " & error_string)
        End If
        displayError()
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Transaction_Tab.ascx.vb - test() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub

  Sub insert_row_change(ByVal sender As Object, ByVal e As EventArgs)
    Try
      Dim contact_name As ListBox = row_contact
      Dim company_name As New ListBox
      company_name = row_company

      If company_name.SelectedValue <> "" Then
        If company_name.SelectedValue <> "|" Then
          contact_drop.Visible = True
          Dim info As Array = Split(company_name.SelectedValue, "|")


          Dim comp_id As Integer = CInt(info(0))
          contact_name.Items.Clear()
          aTempTable = aclsData_Temp.GetContacts(comp_id, info(1), "Y", 0)
          ' check the state of the DataTable
          If Not IsNothing(aTempTable) Then
            If aTempTable.Rows.Count > 0 Then
              contact_name.Items.Clear()
              Dim c_title As String = ""
              contact_name.Items.Add(New ListItem("NOT SELECTED", ""))
              For Each r As DataRow In aTempTable.Rows
                If Not IsDBNull(r("contact_title")) Then
                  c_title = r("contact_title")
                End If
                If r("contact_title") <> "" Then
                  contact_name.Items.Add(New ListItem(CStr(r("contact_title") & " - " & r("contact_sirname") & " " & r("contact_first_name") & " " & r("contact_last_name") & " (" & r("contact_type") & " record) "), r("contact_id") & "|" & r("contact_type")))
                Else
                  contact_name.Items.Add(New ListItem(CStr(r("contact_sirname") & " " & r("contact_first_name") & " " & r("contact_last_name") & " (" & r("contact_type") & " record)"), r("contact_id") & "|" & r("contact_type")))
                End If
              Next
            Else
              contact_name.Items.Add(New ListItem("NO ASSOCIATED CONTACTS", ""))
            End If
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = aclsData_Temp.class_error
              LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - insert_row_change() - " & error_string)
            End If
            displayError()
          End If
        End If
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Transaction_Tab.ascx.vb - insert_row_change() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Sub dispDetails(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
    Try
      Select Case (e.CommandName)
        Case "search"
          Dim company_search_panel As Panel = e.Item.FindControl("company_search_panel")
          company_search_panel.Visible = True
        Case "search_me"

          Dim company_name As DropDownList = e.Item.FindControl("company")

          Dim Named As TextBox = e.Item.FindControl("Name")
          aTempTable = aclsData_Temp.Company_Search(clsGeneral.clsGeneral.Get_Name_Search_String(Named.Text) & "%", "Y", "JC", "", "", "", Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmCommercial_Flag, "", "", "", "", "", "", "")
          company_name.Items.Clear()
          company_name.Items.Add(New ListItem("NONE SELECTED", "|"))
          If Not IsNothing(aTempTable) Then
            If aTempTable.Rows.Count > 0 Then
              For Each r As DataRow In aTempTable.Rows
                company_name.Items.Add(New ListItem(CStr(r("comp_name") & " (" & r("source") & " record)"), r("comp_id") & "|" & r("source")))
              Next
            Else ' 0 rows
            End If
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = aclsData_Temp.class_error
              LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - dispDetails() - " & error_string)
            End If
            displayError()
          End If
      End Select
    Catch ex As Exception
      error_string = "Aircraft_Edit_Transaction_Tab.ascx.vb - dispDetails() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
#End Region
#Region "Bind Data To Grid"
  Public Sub SetUpModels()
    'Fill up Client Aircraft Models
    aTempTable2 = aclsData_Temp.Get_Clients_Aircraft_Model()
    If Not IsNothing(aTempTable2) Then
      If aTempTable2.Rows.Count > 0 Then
        For Each q As DataRow In aTempTable2.Rows
          model_cbo.Items.Add(New ListItem(CStr(q("cliamod_make_name") & " " & q("cliamod_model_name")), q("cliamod_id")))
        Next
      End If
    Else
      If aclsData_Temp.class_error <> "" Then
        error_string = aclsData_Temp.class_error
        LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - bind_data() - " & error_string)
      End If
      displayError()
    End If
  End Sub
  Public Sub FillUpJetnetModelID(ByVal modelID As Long, ByVal jetnetID As Long)

    'Filling up model info if there's a client aircraft ID.
    If jetnetID > 0 Then
      Dim ModelInformation As New DataTable
      Dim SelectedModelID As Long = 0
      'Check for Jetnet model:
      ModelInformation = aclsData_Temp.Get_Clients_Aircraft_Model_ByJETNETAmod(modelID)
      journ_jetnet_amod_id.Text = modelID
      If Not IsNothing(ModelInformation) Then
        If ModelInformation.Rows.Count > 0 Then
          SelectedModelID = ModelInformation.Rows(0).Item("cliamod_id")
        Else
          ModelInformation = New DataTable
          Dim ModelData As New clsClient_Aircraft_Model
          'This model doesn't exist, so we have to get the model data from the jetnet database.
          ModelInformation = aclsData_Temp.Get_JETNET_Aircraft_Model_amodID(modelID)
          If Not IsNothing(ModelInformation) Then
            If ModelInformation.Rows.Count > 0 Then
              ModelData.cliamod_make_name = CStr(IIf(Not IsDBNull(ModelInformation.Rows(0).Item("amod_make_name")), ModelInformation.Rows(0).Item("amod_make_name"), ""))
              ModelData.cliamod_model_name = CStr(IIf(Not IsDBNull(ModelInformation.Rows(0).Item("amod_model_name")), ModelInformation.Rows(0).Item("amod_model_name"), ""))
              ModelData.cliamod_manufacturer_name = CStr(IIf(Not IsDBNull(ModelInformation.Rows(0).Item("amod_manufacturer_name")), ModelInformation.Rows(0).Item("amod_manufacturer_name"), ""))
              ModelData.cliamod_make_type = CStr(IIf(Not IsDBNull(ModelInformation.Rows(0).Item("amod_make_type")), ModelInformation.Rows(0).Item("amod_make_type"), ""))
              ModelData.cliamod_airframe_type = CStr(IIf(Not IsDBNull(ModelInformation.Rows(0).Item("amod_airframe_type")), ModelInformation.Rows(0).Item("amod_airframe_type"), ""))
              ModelData.cliamod_jetnet_amod_id = ModelInformation.Rows(0).Item("amod_id")
              SelectedModelID = aclsData_Temp.Insert_Client_Aircraft_Model(ModelData)
            End If
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = aclsData_Temp.class_error
              LogError("Aircraft_Edit_Transaction_Tab - bind_data() - " & error_string & " AC ID " & Session.Item("ListingID"))
            End If
          End If
        End If
      End If


      SetUpModels()
      model_cbo.SelectedValue = SelectedModelID

      SetUpModelInformation()
    End If
  End Sub
  Public Sub SetUpModelInformation()
    model_cbo.Enabled = False

    'Fill up the Model Information based on selection
    If model_cbo.SelectedValue <> "" Then
      aTempTable2 = aclsData_Temp.Get_Clients_Aircraft_Model_amodID(model_cbo.SelectedValue)
      If Not IsNothing(aTempTable2) Then
        If aTempTable2.Rows.Count > 0 Then
          For Each q As DataRow In aTempTable2.Rows
            ac_make.Text = q("cliamod_make_name")
            ac_model.Text = q("cliamod_model_name")
            ac_manu_name.Text = q("cliamod_manufacturer_name")
            ac_make_type.Text = q("cliamod_make_type")
            airframe_type.Text = q("cliamod_airframe_type")
          Next

        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - bind_data() - " & error_string)
        End If
        displayError()
      End If
    End If
  End Sub
  Public Sub bind_data()
    Try
      Dim ClientId As Long = 0
      Dim JetnetID As Long = 0

      Dim trans_id As Long = 0
      Dim cli_trans_id As Long = 0

      Dim str_mod As String = ""
      Dim ownership As String = ""
      Dim lifecycle As String = ""
      Dim serno As String = ""
      Dim show_asking As Boolean = False
      Dim temp_number As Integer = 0

  


      If Session.Item("ListingSource") = "CLIENT" Then
        Dim FindJetnetID As New DataTable
        ClientId = Session.Item("ListingID")
        client_ac_id.Text = ClientId
        FindJetnetID = aclsData_Temp.Get_Clients_Aircraft(ClientId)
        If Not IsNothing(FindJetnetID) Then
          If FindJetnetID.Rows.Count > 0 Then
            JetnetID = FindJetnetID.Rows(0).Item("cliaircraft_jetnet_ac_id")
            journ_jetnet_amod_id.Text = FindJetnetID.Rows(0).Item("cliamod_jetnet_amod_id")
          End If
        End If
      Else
        Dim FindClientID As New DataTable
        JetnetID = Session.Item("ListingID")
        FindClientID = aclsData_Temp.Get_Client_Aircraft_JETNET_AC(JetnetID)
        If Not IsNothing(FindClientID) Then
          If FindClientID.Rows.Count > 0 Then
            ClientId = FindClientID.Rows(0).Item("cliaircraft_id")
            client_ac_id.Text = ClientId
          End If
        End If

      End If


      If Not IsNothing(Trim(Request("trans"))) Then
        If Not String.IsNullOrEmpty(Trim(Request("trans"))) Then
          If IsNumeric(Trim(Request("trans"))) Then
            trans_id = Trim(Request("trans"))

            'This means a jetnet transaction:

            If Not IsNothing(Trim(Request("assumeID"))) Then
              If Not String.IsNullOrEmpty(Trim(Request("assumeID"))) Then
                If IsNumeric(Trim(Request("assumeID"))) Then
                  AssumeID = Trim(Request("assumeID"))
                End If
              End If
            End If
          End If
        End If
      End If

      If Not IsNothing(Trim(Request("cli_trans"))) Then
        If Not String.IsNullOrEmpty(Trim(Request("cli_trans"))) Then
          If IsNumeric(Trim(Request("cli_trans"))) Then
            cli_trans_id = Trim(Request("cli_trans"))
          End If
        End If
      End If


      'Filling up Year Manufacturer
      For d = 1950 To (Year(Now()) + 1)
        year_mfr.Items.Add(New ListItem(CStr(d), d))
      Next



      'Filling up the transaction type
      aTempTable2 = aclsData_Temp.Get_Transaction_DealType()
      If Not IsNothing(aTempTable2) Then
        If aTempTable2.Rows.Count > 0 Then
          For Each q As DataRow In aTempTable2.Rows
            deal_type.Items.Add(New ListItem(CStr(q("clitdeal_type_name")), q("clitdeal_type_name")))
          Next
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - deal type() - " & error_string)
        End If
        displayError()
      End If


      'Filling up model info if there's a client aircraft ID.
      If ClientId > 0 Then
        SetUpModels()

        aTempTable = aclsData_Temp.Get_Clients_Aircraft(ClientId)
        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then

            For Each R As DataRow In aTempTable.Rows
              model_cbo.SelectedValue = R("cliaircraft_cliamod_id")
              'These values are only defaulted if you click add new transaction.
              ownership = CStr(IIf(Not IsDBNull(R("cliaircraft_ownership")), R("cliaircraft_ownership"), ""))
              lifecycle = CStr(IIf(Not IsDBNull(R("cliaircraft_lifecycle")), R("cliaircraft_lifecycle"), ""))
              serno = CStr(IIf(Not IsDBNull(R("cliaircraft_ser_nbr")), R("cliaircraft_ser_nbr"), ""))
              year_mfr.SelectedValue = CStr(IIf(Not IsDBNull(R("cliaircraft_year_mfr")), R("cliaircraft_year_mfr"), ""))
            Next
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - bind_data() - " & error_string)
          End If
          displayError()
        End If

        SetUpModelInformation()
      End If


      aTempTable = aclsData_Temp.Get_Client_Transactions_Category

      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          Dim distinctTable As DataTable = aTempTable.DefaultView.ToTable(True, "clitcat_type")
          For Each q As DataRow In distinctTable.Rows
            If Not IsDBNull(q("clitcat_type")) Then
              typed.Items.Add(New ListItem(q("clitcat_type"), q("clitcat_type")))
            End If
          Next
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - bind_data() - " & error_string)
        End If
        displayError()
      End If


      typed.SelectedValue = "Full Sale"
      date_listed.Text = Now()

      If Trim(Request("new")) = "true" Then
        serial_nbr.Text = serno
        lifecycle_list.SelectedValue = lifecycle
        ownership_list.SelectedValue = ownership
      End If


      If cli_trans_id <> 0 Then

        client_bind_data()

        aTempTable = aclsData_Temp.Get_Client_Client_Transactions(CInt(cli_trans_id), CInt(trans_id))
        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then



            If ClientId = 0 Then
              'This means  there is no client aircraft, so we have to fill the model information down here.
              SetUpModels()
              model_cbo.SelectedValue = aTempTable.Rows(0).Item("clitrans_cliamod_id")
              SetUpModelInformation()
            End If

            For Each r As DataRow In aTempTable.Rows

              title_change.Text = CommonAircraftFunctions.CreateHeaderLine(ac_make.Text, ac_model.Text, r("clitrans_ser_nbr"), " TRANSACTION EDIT")

              If Not IsDBNull(r("clitrans_airframe_total_hours")) Then
                airframe_total_hours.Text = r("clitrans_airframe_total_hours")
              End If

              If Not IsDBNull(r("clitrans_value_description")) Then
                clitrans_value_description_text.Text = r("clitrans_value_description")
              End If

              If Not IsDBNull(r("clitrans_subcategory_code")) Then
                clitrans_subcategory_code.Text = r("clitrans_subcategory_code")
              End If

              If Not IsDBNull(r("clitrans_subcat_code_part1")) Then
                clitrans_subcat_code_part1.Text = r("clitrans_subcat_code_part1")
              End If

              If Not IsDBNull(r("clitrans_subcat_code_part2")) Then
                clitrans_subcat_code_part2.Text = r("clitrans_subcat_code_part2")
              End If

              If Not IsDBNull(r("clitrans_subcat_code_part3")) Then
                clitrans_subcat_code_part3.Text = r("clitrans_subcat_code_part3")
              End If

              If Not IsDBNull(r("clitrans_retail_flag")) Then
                clitrans_retail_flag_rad.SelectedValue = r("clitrans_retail_flag")
              End If


              If Not IsDBNull(r("clitrans_airframe_total_landings")) Then
                airframe_total_landings.Text = r("clitrans_airframe_total_landings")
              End If
              If Not IsDBNull(r("clitrans_aport_city")) Then
                aiport_city.Text = r("clitrans_aport_city")
              End If
              If Not IsDBNull(r("clitrans_deal_type")) Then
                deal_type.Text = r("clitrans_deal_type")
              End If
              If Not IsDBNull(r("clitrans_year_mfr")) Then
                year_mfr.Text = r("clitrans_year_mfr")
              End If
              If Not IsDBNull(r("clitrans_aport_country")) Then
                airport_country.Text = r("clitrans_aport_country")
              End If
              If Not IsDBNull(r("clitrans_aport_country")) Then
                iata_code.Text = r("clitrans_aport_iata_code")
              End If
              If Not IsDBNull(r("clitrans_aport_icao_code")) Then
                icao_code.Text = r("clitrans_aport_icao_code")
              End If
              If Not IsDBNull(r("clitrans_aport_name")) Then
                airport_name.Text = r("clitrans_aport_name")
              End If
              If Not IsDBNull(r("clitrans_asking_price")) Then

                asking.Text = FormatNumber(CInt(r("clitrans_asking_price")), 2)


              End If
              If Not IsDBNull(r("clitrans_asking_wordage")) Then
                asking_wordage.SelectedValue = r("clitrans_asking_wordage")

                If Not Page.IsPostBack Then
                  ' Response.Write(r("clitrans_asking_wordage") & "!!")
                  If r("clitrans_asking_wordage") <> "" Then
                    'for_sale_second.Visible = True
                    'for_sale_first.Visible = True
                    price_vis.Visible = True
                    'asking_wordage.Visible = True
                    'asking_lbl.Visible = True
                    date_listed_panel.Visible = True
                    for_sale.SelectedValue = "Y"
                  End If
                End If
              End If
              If Not IsDBNull(r("clitrans_country_of_registration")) Then
                country_reg.Text = r("clitrans_country_of_registration")
              End If
              If Not IsDBNull(r("clitrans_customer_note")) Then
                customer_note.Text = r("clitrans_customer_note")
              End If

              If Not IsDBNull(r("clitrans_date_listed")) Then
                If r("clitrans_date_listed") <> "0001-01-01 00:00:00" And r("clitrans_date_listed") <> "1/1/1900" Then
                  If IsDate(r("clitrans_date_listed")) Then
                    date_listed.Text = FormatDateTime(r("clitrans_date_listed"), DateFormat.ShortDate)
                  End If
                Else
                  date_listed.Text = ""
                End If
              End If
              If Not IsDBNull(r("clitrans_est_price")) Then
                estimated_price.Text = FormatNumber(CInt(r("clitrans_est_price")), 2)
              End If
              If Not IsDBNull(r("clitrans_exclusive_flag")) Then
                ac_exclusive.SelectedValue = r("clitrans_exclusive_flag")
              End If
              If Not IsDBNull(r("clitrans_internal_trans_flag")) Then
                ac_internal.SelectedValue = r("clitrans_internal_trans_flag")
              End If
              If Not IsDBNull(r("clitrans_jetnet_trans_id")) Then
                journ_id.Text = r("clitrans_jetnet_trans_id")
              End If
              If Not IsDBNull(r("clitrans_lifecycle")) Then
                lifecycle_list.SelectedValue = r("clitrans_lifecycle")
              End If
              If Not IsDBNull(r("clitrans_newac_flag")) Then
                new_list.SelectedValue = r("clitrans_newac_flag")
              End If
              If Not IsDBNull(r("clitrans_ownership")) Then
                ownership_list.SelectedValue = r("clitrans_ownership")
              End If
              If Not IsDBNull(r("clitrans_reg_nbr")) Then
                reg_nbr.Text = r("clitrans_reg_nbr")
              End If
              If Not IsDBNull(r("clitrans_ser_nbr")) Then
                serial_nbr.Text = r("clitrans_ser_nbr")
              End If
              If Not IsDBNull(r("clitrans_sold_price")) Then

                sold_price.Text = FormatNumber(CInt(r("clitrans_sold_price")), 2)

              End If
              If Not IsDBNull(r("clitrans_sold_price_type")) Then
                sold_price_type.SelectedValue = r("clitrans_sold_price_type")
              End If
              'If Not IsDBNull(r("clitrans_subcategory_code")) Then
              '    subcategory.text = r("clitrans_subcategory_code")
              'End If



              typed.SelectedValue = r("clitrans_type")

              If Not IsDBNull(r("clitrans_jetnet_ac_id")) Then
                jetnet_ac_id.Text = r("clitrans_jetnet_ac_id")
              End If

              If Not IsDBNull(r("clitrans_subject")) Then
                subject.Text = r("clitrans_subject")
              End If
              If Not IsDBNull(r("clitrans_jetnet_trans_id")) Then
                journ_id.Text = r("clitrans_jetnet_trans_id")
              End If
              If Not IsDBNull(r("clitrans_cliac_id")) Then
                journ_ac_id.Text = r("clitrans_cliac_id")
              End If
              If Not IsDBNull(r("clitrans_date")) Then
                journ_date.Text = r("clitrans_date")
              End If
              If Not IsDBNull(r("clitrans_date")) Then
                If IsDate(r("clitrans_date")) Then
                  trans_date.Text = FormatDateTime(r("clitrans_date"), DateFormat.ShortDate)
                End If
              End If

              If aclsData_Temp.CHECK_IF_TRANS_RECORD_EXISTS(journ_id.Text) = True Then
                asking.Attributes.Add("onchange", "showPopup(this.value,1);")
                removeButton.OnClientClick = "return showPopup(this.value,2);"

                Me.share_label_box.Text = "THIS TRANSACTION DATA HAS ALREADY BEEN SUBMITTED TO JETNET. TO UPDATE CHECK THE BOX BELOW BEFORE SAVING."
              Else
                Me.share_label_box.Text = "I understand that by checking the box below that the value related data saved regarding this transaction will be sent to JETNET for use and display within JETNET’s products including display of the sale price for this specific serial numbered aircraft. JETNET WILL NOT display the source data reported as part of this submittal process unless required to do so by court order or otherwise by law. <a href='#' onclick=""javascript:window.open('/help/documents/661.pdf ','_blank','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');"">Learn More</a>."
              End If

            Next
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("Aircraft_Edit_Transaction_Tab.ascx.vb -bind_data() - " & error_string)
          End If
          displayError()
        End If
        Dim ddl As DropDownList = row_contact_type
        aTempTable = aclsData_Temp.Get_CRM_Client_Aircraft_Contact_Type()
        insert_row.Visible = True
        For Each r As DataRow In aTempTable.Rows
          ddl.Items.Add(New ListItem(r("cliact_name"), r("cliact_type")))
        Next


      ElseIf trans_id <> 0 Then
        '00/36 is owner
        '95 is selle
        '62 is registered owner
        insert_row.Visible = False
        aTempTable2 = aclsData_Temp.Get_JETNET_Transactions_Aircraft_Reference_TransID(trans_id)
        If Not IsNothing(aTempTable2) Then
          If aTempTable2.Rows.Count > 0 Then
            datagrid1.DataSource = aTempTable2
            datagrid1.DataBind()
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - bind_data() - " & error_string)
          End If
          displayError()
        End If


        aTempTable = aclsData_Temp.Get_JETNET_Transactions_transID(CLng(trans_id))
        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then

            For Each r As DataRow In aTempTable.Rows
              journ_id.Text = CInt(trans_id)
              journ_ac_id.Text = r("journ_ac_id")

              If aclsData_Temp.CHECK_IF_TRANS_RECORD_EXISTS(journ_id.Text) = True Then
                Me.share_label_box.Text = "THIS TRANSACTION DATA HAS ALREADY BEEN SUBMITTED TO JETNET. TO UPDATE CHECK THE BOX BELOW BEFORE SAVING."
              Else
                Me.share_label_box.Text = "I understand that by checking the box below that the value related data saved regarding this transaction will be sent to JETNET for use and display within JETNET’s products including display of the sale price for this specific serial numbered aircraft. JETNET WILL NOT display the source data reported as part of this submittal process unless required to do so by court order or otherwise by law. <a href='#' onclick=""javascript:window.open('/help/documents/661.pdf ','_blank','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');"">Learn More</a>."
              End If



              FillUpJetnetModelID(r("ac_amod_id"), JetnetID)
              title_change.Text = CommonAircraftFunctions.CreateHeaderLine(ac_make.Text, ac_model.Text, r("ac_ser_no_full"), " TRANSACTION EDIT")

              Try
                lifecycle_list.SelectedValue = r("ac_lifecycle_stage")
              Catch
              End Try
              Try
                ownership_list.SelectedValue = r("ac_ownership_type")
              Catch
              End Try
              journ_date.Text = r("journ_date")

              If Not IsDBNull(r("journ_subcategory_code")) Then
                subcategory.Text = r("journ_subcategory_code")
                clitrans_subcategory_code.Text = r("journ_subcategory_code")
              End If

              If Not IsDBNull(r("journ_subcat_code_part1")) Then
                clitrans_subcat_code_part1.Text = r("journ_subcat_code_part1")
              End If

              If Not IsDBNull(r("journ_subcat_code_part2")) Then
                clitrans_subcat_code_part2.Text = r("journ_subcat_code_part2")
              End If


              If Not IsDBNull(r("journ_subcat_code_part3")) Then
                'default to N
                clitrans_retail_flag_rad.SelectedValue = "N"
                clitrans_subcat_code_part3.Text = r("journ_subcat_code_part3")
                clitrans_retail_flag_rad.SelectedValue = clsGeneral.clsGeneral.TransSetRetailFlag((r("journ_subcat_code_part3")))
                'retailTransToggle.Attributes.Add("style", "display:none;")
              End If

              If Not IsDBNull(r("journ_ac_id")) Then
                jetnet_ac_id.Text = r("journ_ac_id")
              End If

              aTempTable = aclsData_Temp.Get_Client_Transactions_Category_Code(r("journ_subcategory_code"))

              If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then
                  For Each q As DataRow In aTempTable.Rows
                    typed.SelectedValue = q("clitcat_type")
                  Next
                End If
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - dispDetails() - " & error_string)
                End If
                displayError()
              End If

              If Not IsDBNull(r("journ_customer_note")) Then
                customer_note.Text = r("journ_customer_note")
              End If

              If Not IsDBNull(r("journ_date")) Then
                trans_date.Text = FormatDateTime(r("journ_date"), DateFormat.ShortDate)
              End If

              Try
                new_list.SelectedValue = r("journ_newac_flag")
              Catch
              End Try

              Try
                ac_internal.SelectedValue = r("journ_internal_trans_flag")
              Catch
              End Try

              If Not IsDBNull(r("ac_list_date")) Then
                date_listed.Text = r("ac_list_date")
              End If

              Try
                ac_exclusive.SelectedValue = r("ac_exclusive_flag")
              Catch
              End Try
              If Not IsDBNull(r("ac_ser_no_full")) Then
                serial_nbr.Text = r("ac_ser_no_full")
              End If
              If Not IsDBNull(r("ac_reg_no")) Then
                reg_nbr.Text = r("ac_reg_no")
              End If
              If Not IsDBNull(r("trans_year_mfr")) Then
                year_mfr.Text = r("trans_year_mfr")
              End If

              If Not IsDBNull(r("ac_country_of_registration")) Then
                country_reg.Text = r("ac_country_of_registration")
              End If
              If Not IsDBNull(r("ac_airframe_tot_hrs")) Then
                airframe_total_hours.Text = r("ac_airframe_tot_hrs")
              End If
              If Not IsDBNull(r("ac_airframe_tot_landings")) Then
                airframe_total_landings.Text = r("ac_airframe_tot_landings")
              End If
              If Not IsDBNull(r("ac_aport_iata_code")) Then
                iata_code.Text = r("ac_aport_iata_code")
              End If
              If Not IsDBNull(r("ac_aport_icao_code")) Then
                icao_code.Text = r("ac_aport_icao_code")
              End If
              If Not IsDBNull(r("ac_aport_name")) Then
                airport_name.Text = r("ac_aport_name")
              End If
              If Not IsDBNull(r("ac_aport_state")) Then
                airport_state.Text = r("ac_aport_state")
              End If
              If Not IsDBNull(r("ac_aport_country")) Then
                airport_country.Text = r("ac_aport_country")
              End If
              If Not IsDBNull(r("ac_aport_city")) Then
                aiport_city.Text = r("ac_aport_city")
              End If
              If Not IsDBNull(r("ac_aport_private")) Then
                Try
                  airport_private.SelectedValue = r("ac_aport_private")
                Catch
                End Try
              End If
              Try

                subject.Text = r("journ_subject")
              Catch
              End Try

              If Not IsDBNull(r("ac_asking")) Then
                asking_wordage.SelectedValue = r("ac_asking")
                'Response.Write(r("ac_asking") & "!!!")
                If Not Page.IsPostBack Then
                  If r("ac_asking") <> "" Then
                    'for_sale_second.Visible = True
                    'for_sale_first.Visible = True
                    price_vis.Visible = True
                    date_listed_panel.Visible = True
                    'asking_wordage.Visible = True
                    'asking_lbl.Visible = True
                    for_sale.SelectedValue = "Y"
                  End If
                End If
              End If

              'ac_asking 
              show_asking = False
              If HttpContext.Current.Session.Item("localSubscription").crmSalesPriceIndex_Flag = True Then
                show_asking = True
              ElseIf Not IsDBNull(r("ac_asking")) Then
                If r("ac_asking") = "Price" Then
                  show_asking = True
                End If
              End If

              If show_asking = True Then
                If HttpContext.Current.Session.Item("localSubscription").crmSalesPriceIndex_Flag = True Then
                  If Not IsDBNull(r("ac_asking")) Then
                    If (Trim(r("ac_asking")) = "Make Offer" Or Trim(r("ac_asking")) = "") Then
                      ask_lbl.Text = "<A href='' alt='Reported Asking Price Displayed with Permission from Source' title='Reported Asking Price Displayed with Permission from Source'>Asking Price</a>"
                    End If
                  End If
                End If

                If Not IsDBNull(r("ac_asking_price")) Then
                  asking.Text = FormatNumber(CInt(r("ac_asking_price")), 2)
                End If
              End If

              If HttpContext.Current.Session.Item("localSubscription").crmSalesPriceIndex_Flag = True Then
                If Not IsDBNull(r("ac_sale_price_display_flag")) Then
                  If Trim(r.Item("ac_sale_price_display_flag").ToString) = "Y" Then
                    sold_lbl.Text = "<A href='' alt='Reported Sale Price Displayed with Permission from Source' title='Reported Sale Price Displayed with Permission from Source'>Sold Price</a>"

                    If Not IsDBNull(r("ac_sale_price")) Then
                      temp_number = CInt(r("ac_sale_price"))
                      temp_number = CInt(temp_number / 10000) ' divide by ten thousand, which will cut down last variable
                      temp_number = FormatNumber((temp_number * 10000), 0)

                      sold_price.Text = FormatNumber(CInt(temp_number), 2)
                    End If


                  End If
                End If
              End If

            Next

            If AssumeID > 0 Then
              Dim acOverwriteTable As New DataTable
              acOverwriteTable = aclsData_Temp.Get_Clients_Aircraft(AssumeID)
              If Not IsNothing(acOverwriteTable) Then
                If acOverwriteTable.Rows.Count > 0 Then
                  For Each R As DataRow In acOverwriteTable.Rows
                    If Not IsDBNull(R("cliaircraft_asking_wordage")) Then
                      asking_wordage.SelectedValue = R("cliaircraft_asking_wordage")

                      If R("cliaircraft_asking_wordage") = "Price" Then
                        price_vis.Visible = True
                      Else
                        price_vis.Visible = False
                      End If

                    End If
                      If Not IsDBNull(R("cliaircraft_est_price")) Then
                        estimated_price.Text = CStr(IIf(Not IsDBNull(R("cliaircraft_est_price")), FormatNumber(R("cliaircraft_est_price"), 2), ""))
                      End If
                      If Not IsDBNull(R("cliaircraft_asking_price")) Then
                        asking.Text = CStr(IIf(Not IsDBNull(R("cliaircraft_asking_price")), FormatNumber(R("cliaircraft_asking_price"), 2), ""))
                    End If
                    If Not IsDBNull(R("cliaircraft_jetnet_ac_id")) Then
                      hiddenJetnetAssumeIDRedirect.Text = R("cliaircraft_jetnet_ac_id")
                    End If
                    If Not IsDBNull(R("cliaircraft_value_description")) Then
                      clitrans_value_description_text.Text = R("cliaircraft_value_description").ToString
                    End If

                    If Not IsDBNull(R("cliaircraft_airframe_total_hours")) Then
                      airframe_total_hours.Text = R("cliaircraft_airframe_total_hours")
                    End If
                    If Not IsDBNull(R("cliaircraft_airframe_total_landings")) Then
                      airframe_total_landings.Text = R("cliaircraft_airframe_total_landings")
                    End If

                  Next
                End If
              End If
            End If
          End If
          If typed.SelectedValue = "Full Sale" And InStr(Trim(subject.Text), "On Market") = 0 And InStr(Trim(subject.Text), "Off Market") = 0 Then
            share_box.Visible = True
          Else
            share_box.Visible = False
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - bind_data() - " & error_string)
          End If
          displayError()
        End If

      ElseIf Trim(Request("new")) = "true" Then
        If Session.Item("ListingSource") = "JETNET" Then
          Dim ModelInformation As New DataTable

          ModelInformation = aclsData_Temp.GetJETNET_AC_NAME(JetnetID, "")
          If Not IsNothing(ModelInformation) Then
            If ModelInformation.Rows.Count > 0 Then
              FillUpJetnetModelID(ModelInformation.Rows(0).Item("amod_id"), JetnetID)
              jetnet_ac_id.Text = JetnetID
            End If
          End If
        End If
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Transaction_Tab.ascx.vb - bind_data() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
#End Region
#Region "Update Event"
  Private Sub update_Click() Handles updateButton.Click




    Try
      Dim cli_trans_id = 0
      Try
        cli_trans_id = CInt(Trim(Request("cli_trans")))
      Catch
        cli_trans_id = 0
      End Try



      If cli_trans_id <> 0 Then
        'UPDATE CLIENT TRANSACTION
        Dim aclsUpdate_Client_Transactions As New clsClient_Transactions
        aclsUpdate_Client_Transactions.clitrans_action_date = Now()
        aclsUpdate_Client_Transactions.clitrans_id = cli_trans_id

        If (airframe_total_hours.Text) <> "" Then
          aclsUpdate_Client_Transactions.clitrans_airframe_total_hours = airframe_total_hours.Text
        End If
        If (airframe_total_landings.Text) <> "" Then
          aclsUpdate_Client_Transactions.clitrans_airframe_total_landings = airframe_total_landings.Text
        End If
        If (aiport_city.Text) <> "" Then
          aclsUpdate_Client_Transactions.clitrans_aport_city = aiport_city.Text
          aclsUpdate_Client_Transactions.clitrans_aport_city = Replace(aclsUpdate_Client_Transactions.clitrans_aport_city, "'", "''")
        End If

        If clitrans_value_description_text.Text <> "" Then
          aclsUpdate_Client_Transactions.clitrans_value_description = clitrans_value_description_text.Text
        End If

        If (airport_country.Text) <> "" Then
          aclsUpdate_Client_Transactions.clitrans_aport_country = airport_country.Text
          aclsUpdate_Client_Transactions.clitrans_aport_country = Replace(aclsUpdate_Client_Transactions.clitrans_aport_country, "'", "''")
        End If

        If IsNumeric(jetnet_ac_id.Text) Then
          aclsUpdate_Client_Transactions.clitrans_jetnet_ac_id = jetnet_ac_id.Text
        End If

        If (year_mfr.SelectedValue) <> "" Then
          aclsUpdate_Client_Transactions.clitrans_year_mfr = year_mfr.SelectedValue
        End If
        If (deal_type.SelectedValue) <> "" Then
          aclsUpdate_Client_Transactions.clitrans_deal_type = deal_type.SelectedValue
        End If
        If (iata_code.Text) <> "" Then
          aclsUpdate_Client_Transactions.clitrans_aport_iata_code = iata_code.Text
        End If
        If (icao_code.Text) <> "" Then
          aclsUpdate_Client_Transactions.clitrans_aport_icao_code = icao_code.Text
        End If
        If (airport_name.Text) <> "" Then
          aclsUpdate_Client_Transactions.clitrans_aport_name = airport_name.Text
          aclsUpdate_Client_Transactions.clitrans_aport_name = Replace(aclsUpdate_Client_Transactions.clitrans_aport_name, "'", "''")
        End If

        Dim asking_price As Integer = 0
        Try
          asking_price = CInt(clsGeneral.clsGeneral.FormatMKDollarValue(asking.Text))
        Catch
          asking_price = 0
        End Try


        aclsUpdate_Client_Transactions.clitrans_asking_price = asking_price

        If (asking_wordage.SelectedValue) <> "" Then
          aclsUpdate_Client_Transactions.clitrans_asking_wordage = asking_wordage.SelectedValue
        End If


        'If Session.Item("ListingSource") = "CLIENT" Then
        aclsUpdate_Client_Transactions.clitrans_cliac_id = client_ac_id.Text 'Session.Item("ListingID")
        'End If

        If (model_cbo.SelectedValue) <> "" Then
          aclsUpdate_Client_Transactions.clitrans_cliamod_id = model_cbo.SelectedValue
        End If
        If (country_reg.Text) <> "" Then
          aclsUpdate_Client_Transactions.clitrans_country_of_registration = country_reg.Text
        End If
        If (customer_note.Text) <> "" Then
          aclsUpdate_Client_Transactions.clitrans_customer_note = customer_note.Text
        End If

        aclsUpdate_Client_Transactions.clitrans_date = trans_date.Text
        If (date_listed.Text) <> "" Then
          aclsUpdate_Client_Transactions.clitrans_date_listed = date_listed.Text
        End If
        Try
          asking_price = CInt(clsGeneral.clsGeneral.FormatMKDollarValue(estimated_price.Text))
        Catch
          asking_price = 0
        End Try

        If (typed.SelectedValue) <> "" Then
          aclsUpdate_Client_Transactions.clitrans_type = typed.SelectedValue
        End If

        aclsUpdate_Client_Transactions.clitrans_est_price = asking_price

        If (asking_wordage.SelectedValue) <> "" Then
          aclsUpdate_Client_Transactions.clitrans_asking_wordage = Left(asking_wordage.SelectedValue, 10)
        End If

        If (ac_exclusive.SelectedValue) <> "" Then
          aclsUpdate_Client_Transactions.clitrans_exclusive_flag = ac_exclusive.SelectedValue
        End If
        If (ac_internal.SelectedValue) <> "" Then
          aclsUpdate_Client_Transactions.clitrans_internal_trans_flag = ac_internal.SelectedValue
        End If

        aclsUpdate_Client_Transactions.clitrans_jetnet_trans_id = journ_id.Text

        If (lifecycle_list.SelectedValue) <> "" Then
          aclsUpdate_Client_Transactions.clitrans_lifecycle = lifecycle_list.SelectedValue
        End If
        If (new_list.SelectedValue) <> "" Then
          aclsUpdate_Client_Transactions.clitrans_newac_flag = new_list.SelectedValue
        End If
        If (ownership_list.SelectedValue) <> "" Then
          aclsUpdate_Client_Transactions.clitrans_ownership = ownership_list.SelectedValue
        End If
        If (reg_nbr.Text) <> "" Then
          aclsUpdate_Client_Transactions.clitrans_reg_nbr = reg_nbr.Text
        End If
        If (serial_nbr.Text) <> "" Then
          aclsUpdate_Client_Transactions.clitrans_ser_nbr = serial_nbr.Text
        End If
        Try
          asking_price = CInt(clsGeneral.clsGeneral.FormatMKDollarValue(sold_price.Text))
        Catch
          asking_price = 0
        End Try
        aclsUpdate_Client_Transactions.clitrans_sold_price = asking_price

        If (sold_price_type.Text) <> "" Then
          aclsUpdate_Client_Transactions.clitrans_sold_price_type = sold_price_type.Text
        End If

        'New fields added
        aclsUpdate_Client_Transactions.clitrans_subcat_code_part1 = clitrans_subcat_code_part1.Text
        aclsUpdate_Client_Transactions.clitrans_subcat_code_part2 = clitrans_subcat_code_part2.Text
        aclsUpdate_Client_Transactions.clitrans_subcat_code_part3 = clitrans_subcat_code_part3.Text
        aclsUpdate_Client_Transactions.clitrans_subcategory_code = clitrans_subcategory_code.Text
        aclsUpdate_Client_Transactions.clitrans_retail_flag = clitrans_retail_flag_rad.SelectedValue

        If (subject.Text) <> "" Then
          aclsUpdate_Client_Transactions.clitrans_subject = Replace(subject.Text, "'", "''")
        End If

        ' if we have already updated a client transaction 
        If Me.send_check.Checked = True Then
          cleared_sale = False
          Call aclsData_Temp.Insert_Into_Aircraft_Value(aclsUpdate_Client_Transactions, journ_id.Text, cleared_sale, "transaction")
          ' If cleared_sale = True Then
          '   Call SetUpPopupModal()
          'End If
        End If



        If aclsData_Temp.Update_Client_Transactions(aclsUpdate_Client_Transactions) = 1 Then
          If Trim(Request("from")) = "aircraftDetails" Then
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location.reload();", True)
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
          ElseIf Trim(Request("from")) <> "view" Then
            If Session.Item("Listing") = 1 Then
              Session.Item("company_active_tab") = 1
            End If
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "javascript: var URLlink = ''; if (window.opener.location.pathname.toUpperCase().search('LISTING') == 1){URLlink = '?redo_search=true'}; window.opener.location.href=window.opener.location.pathname + URLlink;", True)
          Else
            'System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "javascript: var URLlink = ''; if (window.opener.location.pathname.toUpperCase().search('WEBSOURCE') == 1){if (window.opener.opener.location.search.indexOf('expand=true') != -1) {window.opener.opener.location = window.opener.opener.location + '&expand=true';} else {window.opener.opener.location = window.opener.opener.location;} } else {window.opener.location.href = window.opener.location.href;}", True)

            If Trim(Request("from")) = "view" And Trim(Request("extra_amod")) <> "" Then

              Dim url As String = "view_template.aspx?viewID=1&noMaster=false&activetab=2&ViewName=Model Market Summary&amod_id=" & Trim(Request("amod_id"))

              url &= "&extra_amod=" & Trim(Request("extra_amod"))

              System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_WindowParent", "javascript:window.opener.location.href = '" & url & "';", True)

            Else
              System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "javascript:window.opener.location.reload(true);", True)
            End If


          End If

          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
        End If
      Else
        'INSERT JETNET TRANSACTION
        'This is the big one. 
        Dim aclsInsert_Client_Transactions As New clsClient_Transactions
        aclsInsert_Client_Transactions.clitrans_action_date = Now()

        If (airframe_total_hours.Text) <> "" Then
          aclsInsert_Client_Transactions.clitrans_airframe_total_hours = airframe_total_hours.Text
        End If
        If (airframe_total_landings.Text) <> "" Then
          aclsInsert_Client_Transactions.clitrans_airframe_total_landings = airframe_total_landings.Text
        End If
        If (aiport_city.Text) <> "" Then
          aclsInsert_Client_Transactions.clitrans_aport_city = aiport_city.Text
        End If

        If IsNumeric(jetnet_ac_id.Text) Then
          aclsInsert_Client_Transactions.clitrans_jetnet_ac_id = jetnet_ac_id.Text
        End If

        If clitrans_value_description_text.Text <> "" Then
          aclsInsert_Client_Transactions.clitrans_value_description = clitrans_value_description_text.Text
        End If

        If (year_mfr.SelectedValue) <> "" Then
          aclsInsert_Client_Transactions.clitrans_year_mfr = year_mfr.SelectedValue
        End If
        If (deal_type.SelectedValue) <> "" Then
          aclsInsert_Client_Transactions.clitrans_deal_type = deal_type.SelectedValue
        End If

        If (airport_country.Text) <> "" Then
          aclsInsert_Client_Transactions.clitrans_aport_country = airport_country.Text
        End If
        If (iata_code.Text) <> "" Then
          aclsInsert_Client_Transactions.clitrans_aport_iata_code = iata_code.Text
        End If
        If (icao_code.Text) <> "" Then
          aclsInsert_Client_Transactions.clitrans_aport_icao_code = icao_code.Text
        End If
        If (airport_name.Text) <> "" Then
          aclsInsert_Client_Transactions.clitrans_aport_name = airport_name.Text
        End If

        Dim asking_price As Integer = 0
        Try
          asking_price = CInt(clsGeneral.clsGeneral.FormatMKDollarValue(asking.Text))
        Catch
          asking_price = 0
        End Try
        aclsInsert_Client_Transactions.clitrans_asking_price = asking_price

        If (asking_wordage.SelectedValue) <> "" Then
          aclsInsert_Client_Transactions.clitrans_asking_wordage = Left(asking_wordage.SelectedValue, 10)
        End If

        If Trim(Request("auto_trans")) = "true" And Trim(Request("from")) = "view" Then
          aclsInsert_Client_Transactions.clitrans_cliac_id = ValueViewRecentSalesClientID
        Else
          'If Session.Item("ListingSource") = "JETNET" Then
          If AssumeID > 0 Then
            aclsInsert_Client_Transactions.clitrans_cliac_id = 0
          Else
            aclsInsert_Client_Transactions.clitrans_cliac_id = client_ac_id.Text 'Session.Item("OtherID")
            'Else
            '  aclsInsert_Client_Transactions.clitrans_cliac_id = Session.Item("ListingID")
            'End If
          End If
        End If
        If (model_cbo.SelectedValue) <> "" Then
          aclsInsert_Client_Transactions.clitrans_cliamod_id = model_cbo.SelectedValue
        End If
        If (country_reg.Text) <> "" Then
          aclsInsert_Client_Transactions.clitrans_country_of_registration = country_reg.Text
        End If
        If (customer_note.Text) <> "" Then
          aclsInsert_Client_Transactions.clitrans_customer_note = customer_note.Text
        End If

        aclsInsert_Client_Transactions.clitrans_date = trans_date.Text
        If (date_listed.Text) <> "" Then
          aclsInsert_Client_Transactions.clitrans_date_listed = date_listed.Text
        End If
        Try
          asking_price = CInt(clsGeneral.clsGeneral.FormatMKDollarValue(estimated_price.Text))
        Catch
          asking_price = 0
        End Try
        aclsInsert_Client_Transactions.clitrans_est_price = asking_price

        If (ac_exclusive.SelectedValue) <> "" Then
          aclsInsert_Client_Transactions.clitrans_exclusive_flag = ac_exclusive.SelectedValue
        End If
        If (ac_internal.SelectedValue) <> "" Then
          aclsInsert_Client_Transactions.clitrans_internal_trans_flag = ac_internal.SelectedValue
        End If

        If Trim(Request("new")) <> "true" Then
          aclsInsert_Client_Transactions.clitrans_jetnet_trans_id = journ_id.Text
        End If

        If (lifecycle_list.SelectedValue) <> "" Then
          aclsInsert_Client_Transactions.clitrans_lifecycle = lifecycle_list.SelectedValue
        End If
        If (new_list.SelectedValue) <> "" Then
          aclsInsert_Client_Transactions.clitrans_newac_flag = new_list.SelectedValue
        End If
        If (ownership_list.SelectedValue) <> "" Then
          aclsInsert_Client_Transactions.clitrans_ownership = ownership_list.SelectedValue
        End If
        If (reg_nbr.Text) <> "" Then
          aclsInsert_Client_Transactions.clitrans_reg_nbr = reg_nbr.Text
        End If
        If (serial_nbr.Text) <> "" Then
          aclsInsert_Client_Transactions.clitrans_ser_nbr = serial_nbr.Text
        End If

        'New fields added
        aclsInsert_Client_Transactions.clitrans_subcat_code_part1 = clitrans_subcat_code_part1.Text
        aclsInsert_Client_Transactions.clitrans_subcat_code_part2 = clitrans_subcat_code_part2.Text
        aclsInsert_Client_Transactions.clitrans_subcat_code_part3 = clitrans_subcat_code_part3.Text
        aclsInsert_Client_Transactions.clitrans_subcategory_code = clitrans_subcategory_code.Text
        aclsInsert_Client_Transactions.clitrans_retail_flag = clitrans_retail_flag_rad.SelectedValue

        Try
          asking_price = CInt(clsGeneral.clsGeneral.FormatMKDollarValue(sold_price.Text))
        Catch
          asking_price = 0
        End Try
        aclsInsert_Client_Transactions.clitrans_sold_price = asking_price

        If (sold_price_type.Text) <> "" Then
          aclsInsert_Client_Transactions.clitrans_sold_price_type = sold_price_type.Text
        End If
        If (typed.SelectedValue) <> "" Then
          aclsInsert_Client_Transactions.clitrans_type = typed.SelectedValue
        End If

        If (subject.Text) <> "" Then
          aclsInsert_Client_Transactions.clitrans_subject = subject.Text
        End If
        Dim return_id As Integer = aclsData_Temp.Insert_Client_Transactions(aclsInsert_Client_Transactions)
        Dim client_trans_id As Integer = return_id
        If Trim(Request("new")) <> "true" Then
          If return_id <> 0 Then
            'Really quick to test a function.


            '1.) First we take the jetnet transaction ID.

            '2.) Then we take this ID and figure out what's stored in the transaction company table.

            '00/36 is owner
            '95 is seller
            '62 is registered owner
            Dim errored As String = ""
            aTempTable = aclsData_Temp.Get_JETNET_Transactions_Aircraft_Reference_TransID(journ_id.Text)

            If Not IsNothing(aTempTable) Then
              If aTempTable.Rows.Count > 0 Then
                '        'For each company there:
                For Each q As DataRow In aTempTable.Rows

                  '3.) Take that jetnet company ID and poll it against our client database. 
                  aTempTable = aclsData_Temp.GetCompanyInfo_JETNET_ID(q("tacref_comp_id"), errored)

                  If Not IsNothing(aTempTable) Then
                    If aTempTable.Rows.Count > 0 Then

                      'Now check and see if the client company ID exists in the transaction table for this record
                      aTempTable2 = aclsData_Temp.Get_Client_Transactions_Company(aTempTable.Rows(0).Item("comp_id"), client_trans_id)

                      Dim datatable As New DataTable
                      datatable = aTempTable2.Clone
                      'Filter that transaction company table based only on the ones with that transaction ID. 
                      'This really needs to match the CLIENT TRANSACTION ID NOT JETNET. MUST CHANGE
                      Dim afileterd As DataRow() = aTempTable2.Select("clitcomp_trans_id = '" & journ_id.Text & "' ", "clitcomp_id")

                      For Each z As DataRow In afileterd
                        datatable.ImportRow(z)
                      Next

                      If datatable.Rows.Count > 0 Then
                        'this is if the client company exists in the transaction record
                        ' Response.Write("<br />Don't make a copy of this!!!!!" & datatable.Rows(0).Item("clitcomp_id") & "<br />")
                      Else
                        'This means that a client copy of this already exists in the database.
                        'Response.Write("<br />Not found!!! Make a copy!! " & aTempTable.Rows(0).Item("comp_id") & "<br />")
                        Dim Client_Company_ID As Integer = aTempTable.Rows(0).Item("comp_id")
                        '---------4.) Store all of the info for that transaction company in the client transaction company database.
                        Fill_Transaction_Company(Client_Company_ID, journ_id.Text, q("tacref_comp_id"), client_trans_id)
                        '---------5.) Store all of the related contacts to that company in the transaction related database.
                        Fill_Transaction_Contacts(Client_Company_ID, journ_id.Text, q("tacref_comp_id"), client_trans_id)

                      End If
                    Else
                      'Response.Write("<br />Not here!" & "<br />")

                      '-----If the client copy doesn't exist
                      '---------4.) Store all of the information for the transaction company into the client COMPANY database.
                      '---------4.) b. Store all of the related company phone numbers to that transaction in the client phone number database.
                      Dim Client_Company_ID As Integer = 0
                      Client_Company_ID = Fill_Client_Company(q("tacref_comp_id"), journ_id.Text)
                      'Client_Company_ID = 11838 'hard coded for now for testing and coding purposes
                      '---------5.) Store all of the information for that transaction company into the client company TRANSACTION database.
                      '---------5.) b. Store all of the related company phone numbers to that transaction in the phone number transaction database.
                      Fill_Transaction_Company(Client_Company_ID, journ_id.Text, q("tacref_comp_id"), client_trans_id)
                      '---------6.) Store all of the information for the related contacts to that company into the client CONTACT database. 
                      '---------6.) b. Also add the contact phone numbers into the client database. 
                      Fill_Client_Contacts(q("tacref_comp_id"), Client_Company_ID, journ_id.Text)
                      '---------7.) Store all of the information for the related contacts to that company into the client contact TRANSACTION database.
                      Fill_Transaction_Contacts(Client_Company_ID, journ_id.Text, q("tacref_comp_id"), client_trans_id)
                    End If
                  Else
                    If aclsData_Temp.class_error <> "" Then
                      error_string = aclsData_Temp.class_error
                      LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - Update_Click() - " & error_string)
                    End If
                    displayError()
                  End If
                Next
                'This has to take place after the loop because it does all at once. 
                '---------10.) Store all of the Client ac reference stuff.
                'Fill_AC_Reference(journ_id.Text)
                '---------11.) Store all of the Transaction ac reference stuff.
                Fill_AC_Transaction_Reference(journ_id.Text, client_trans_id)
              End If
            Else
              If aclsData_Temp.class_error <> "" Then
                error_string = aclsData_Temp.class_error
                LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - Update_Click() - " & error_string)
              End If
              displayError()
            End If


            If Me.send_check.Checked = True Then
              cleared_sale = False
              Call aclsData_Temp.Insert_Into_Aircraft_Value(aclsInsert_Client_Transactions, journ_id.Text, cleared_sale, "transaction")
              ' If cleared_sale = True Then
              '   Call SetUpPopupModal()
              ' End If
            End If

            'For each company there: //means note from Rick//

            '////If we lookup the jetnet company id in our client company table and don’t find a record in our client database,
            ', then we make a new client company record and use that company id on all the transaction records that we store.
            'Use the same approach for contacts as for the company.///


            '3.) Take that jetnet company ID and poll it against our client database. 
            '----- If it's there (meaning it has a client copy)
            '---------4.) Store all of the info for that transaction company in the client transaction company database.
            '---------5.) Store all of the related contacts to that company in the transaction related database.
            '---------6.) Store all of the related phone numbers to that transaction.
            '---------7.) Store all of the transaction_ac_reference stuff to map.

            '-----If the client copy doesn't exist
            '---------4.) Store all of the information for the transaction company into the client COMPANY database.
            '---------5.) Store all of the information for that transaction company into the client company TRANSACTION database.
            '---------6.) Store all of the information for the related contacts to that company into the client CONTACT database.
            '---------7.) Store all of the information for the related contacts to that company into the client contact TRANSACTION database.
            '---------8.) Store all of the related phone numbers to that transaction in the phone number database.
            '---------9.) Store all of the related phone numbers to that transaction in the client phone number database.
            '---------10.) Store all of the transaction ac reference stuff.
          End If
        End If

        If Trim(Request("cli_trans")) <> "" Then
          If Trim(Request("from")) <> "view" Then
            If Session.Item("Listing") = 1 Then
              Session.Item("company_active_tab") = 1
            End If
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window2", "javascript: var URLlink = ''; if (window.opener.location.pathname.toUpperCase().search('LISTING') == 1){URLlink = '?redo_search=true'}; window.opener.location.href=window.opener.location.pathname + URLlink;", True)
          Else
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_WindowParent", "window.opener.location.href = window.opener.location.href;", True)
          End If

          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
        ElseIf Trim(Request("from")) = "aircraftDetails" Then
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location.href='/DisplayAircraftDetail.aspx?acid=" & Trim(Request("acID")) & IIf(Trim(Request("source")) <> "", "&source=" & Trim(Request("source")), "") & "&jid=" & client_trans_id.ToString & "&tsource=CLIENT';", True)
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
        Else
          Dim ref As String = ""
          Dim ConfirmDialog As String = ""
          If AssumeID > 0 Then
            'If Trim(Request("assumeID")) <> "" Then
            '  If IsNumeric(Trim(Request("assumeID"))) Then
            'One more thing, we need to actually remove this aircraft since we're moving it to a transaction.
            aclsData_Temp.Delete_Client_Aircraft(AssumeID)

            If InStr(Trim(Request("opener")).ToString.ToLower, "details.aspx") > 0 Then
              System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_WindowAutoID", "window.opener.opener.location.href = 'details.aspx?type=3&source=JETNET&ac_id=" & hiddenJetnetAssumeIDRedirect.Text & "';", True)
            Else
              System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_WindowAutoID", "window.opener.opener.location.href = '/view_template.aspx?noMaster=false&viewID=1&ViewName=Model Market Summary&amod_id=" & journ_jetnet_amod_id.Text & IIf(Not String.IsNullOrEmpty(Trim(Request("extra_amod"))), "&extra_amod=" & Trim(Request("extra_amod")), "") & "';", True)
            End If


            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "window.opener.close();", True)
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window22", "window.close();", True)
            'End If
          ElseIf Trim(Request("auto_trans")) = "true" And Trim(Request("from")) = "view" Then

            Dim url As String = "view_template.aspx?viewID=19&noteID=" & Trim(Request("viewNOTEID")) & "&acID=" & Trim(Request("acID")) & "&noMaster=false&activetab=" & Trim(Request("activetab")) & "&ac_type=" & Trim(Request("ac_type")) & "&created_client=Y&source=" & Trim(Request("source"))
            If Not IsNothing(Trim(Request("jac_id"))) Then
              url &= "&jac_id=" & Trim(Request("jac_id"))
            End If

            If Not IsNothing(Trim(Request("extra_amod"))) Then
              url &= "&extra_amod=" & Trim(Request("extra_amod"))
            End If

            url &= "&compare_ac_id=" & ValueViewRecentSalesClientID.ToString

            If Not IsNothing(Trim(Request("sold_current"))) Then
              url &= "&sold_current=" & Trim(Request("sold_current"))
            End If

            url &= "&trans_id=" & client_trans_id

            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_WindowParent", "window.location.href = '" & url & "';", True)
          Else

            If Trim(Request("from")) <> "view" Then
              System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window2", "javascript: var URLlink = ''; if (window.opener.location.pathname.toUpperCase().search('LISTING') == 1){URLlink = '?redo_search=true'}; window.opener.location.href=window.opener.location.pathname + URLlink;", True)
            Else
              'System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_WindowParent", "window.opener.location.href = window.opener.location.href;", True)
              ' System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "javascript: var URLlink = ''; if (window.opener.location.pathname.toUpperCase().search('WEBSOURCE') == 1){if (window.opener.opener.location.search.indexOf('expand=true') != -1) {window.opener.opener.location = window.opener.opener.location + '&expand=true';} else {window.opener.opener.location = window.opener.opener.location;} } else {window.opener.location.href = window.opener.location.href;}", True)
              If Not String.IsNullOrEmpty(Trim(Request("viewType"))) Then
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "openerRefresh", "window.opener.location.href = window.opener.location.href;", True)
              Else
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_WindowAutoID", "window.opener.location.href = '/view_template.aspx?noMaster=false&activetab=" & Trim(Request("activetab")) & "&viewID=" & Trim(Request("viewID")) & IIf(Trim(Request("viewID")) <> "19", "&amod_id=" & journ_jetnet_amod_id.Text, "") & IIf(Not String.IsNullOrEmpty(Trim(Request("extra_amod"))), "&extra_amod=" & Trim(Request("extra_amod")), "") & IIf(Not String.IsNullOrEmpty(Trim(Request("noteID"))), "&noteID=" & Trim(Request("noteID")), "") & "';", True)
              End If


            End If

            If Trim(Request("trans")) = "" Then
              ref = "add=ref"
            End If




            ConfirmDialog = "if (confirm(""Your client transaction has been saved. If you are done with changes to this transaction and would like to continue working click OK or click Cancel to return to the transaction to make additional edits or modify Seller, Purchaser, etc."")) {"
            ConfirmDialog += "self.close();"
            ConfirmDialog += "} else {"

            ConfirmDialog += "window.location = 'edit.aspx?" & IIf(Trim(Request("from")) = "view", "from=view&", "") & IIf(Trim(Request("activetab")) <> "", "activetab=" & Trim(Request("activetab")) & "&", "") & "action=edit&" & IIf(Trim(Request("acID")) <> "", "acID=" & Trim(Request("acID")) & "&source=" & Trim(Request("source")) & "&", "") & "type=transaction&" & ref & "&trans=" & journ_id.Text & "&cli_trans=" & client_trans_id & "';"

            ConfirmDialog += "}"


            System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "ConfirmDialog", ConfirmDialog, True) '"window.location = 'edit.aspx?" & IIf(Trim(Request("from")) = "view", "from=view&", "") & "action=edit&" & IIf(Trim(Request("acIzzD")) <> "", "acID=" & Trim(Request("acID")) & "&source=" & Trim(Request("source")) & "&", "") & "type=transaction&" & ref & "&trans=" & journ_id.Text & "&cli_trans=" & client_trans_id & "';", True)
            'System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
          End If
        End If

      End If

    Catch ex As Exception
      error_string = "Aircraft_Edit_Transaction_Tab.ascx.vb - Update_Click() - " & ex.Message
      LogError(error_string)
    End Try

  End Sub

    Private Sub SetUpPopupModal()

        includeJqueryTheme.Text = "<link rel=""Stylesheet"" type=""text/css"" href=""//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css"" />"
        includeJqueryTheme.Visible = True

        If Not Page.ClientScript.IsClientScriptBlockRegistered("popups") Then
            Dim modalScript As StringBuilder = New StringBuilder()
            Dim modalPostbackScript As StringBuilder = New StringBuilder()

            modalPostbackScript.Append(" $(function(){")
            modalPostbackScript.Append("Sys.Application.add_load(function() {")

            modalPostbackScript.Append("jQuery(""#evoSidedialog"").dialog({")
            modalPostbackScript.Append("autoOpen: false,")
            modalPostbackScript.Append("show: {")
            modalPostbackScript.Append("effect: ""fade"",")
            modalPostbackScript.Append("duration: 500")
            modalPostbackScript.Append("},")
            modalPostbackScript.Append("modal: true,")
            modalPostbackScript.Append("dialogClass: ""welcomeUserPopup"",")
            modalPostbackScript.Append("minHeight: 130,")
            modalPostbackScript.Append("maxHeight: 130,")
            modalPostbackScript.Append("maxWidth: 750,")
            modalPostbackScript.Append("minWidth: 750,")
            modalPostbackScript.Append("draggable: false,")
            modalPostbackScript.Append("closeText:""X""")
            modalPostbackScript.Append("});")
            modalPostbackScript.Append("$(""#"""").click(function() {")
            modalPostbackScript.Append("jQuery(""#evoSidedialog"").dialog(""open"");")
            modalPostbackScript.Append("});")

            modalPostbackScript.Append("jQuery(""#yachtSidedialog"").dialog({")
            modalPostbackScript.Append("autoOpen: false,")
            modalPostbackScript.Append("show: {")
            modalPostbackScript.Append("effect: ""fade"",")
            modalPostbackScript.Append("duration: 500")
            modalPostbackScript.Append("},")
            modalPostbackScript.Append("modal: true,")
            modalPostbackScript.Append("dialogClass: ""welcomeUserPopup"",")
            modalPostbackScript.Append("minHeight: 130,")
            modalPostbackScript.Append("maxHeight: 130,")
            modalPostbackScript.Append("maxWidth: 750,")
            modalPostbackScript.Append("minWidth: 750,")
            modalPostbackScript.Append("draggable: false,")
            modalPostbackScript.Append("closeText:""X""")
            modalPostbackScript.Append("});")

            modalPostbackScript.Append("$(""#"""").click(function() {")
            modalPostbackScript.Append("jQuery(""#yachtSidedialog"").dialog(""open"");")
            modalPostbackScript.Append("});")

            modalPostbackScript.Append("});")
            'Add before final closing, not needed
            modalScript.Append(Replace(modalPostbackScript.ToString, "Sys.Application.add_load(function() {", ""))


            modalPostbackScript.Append("});")
            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "popupsPost", modalPostbackScript.ToString, True)
            ' Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "popupsPost", modalPostbackScript.ToString, True)
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "popups", " window.onload = function() {" & modalScript.ToString & "};", True)

        End If
    End Sub
#End Region
#Region "Functions for Transactions"
    Private Sub asking_wordage_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles asking_wordage.SelectedIndexChanged
    Try
      If asking_wordage.Text = "Price" Then
        price_vis.Visible = True
        'for_sale_second.Visible = True
        'for_sale_first.Visible = True
      Else
        'for_sale_second.Visible = False
        'for_sale_first.Visible = False
        price_vis.Visible = False
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Transaction_Tab.ascx.vb - asking_wordage_SelectedIndexChanged() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Private Sub for_sale_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles for_sale.SelectedIndexChanged
    Try
      If for_sale.SelectedValue = "Y" Then
        date_listed_panel.Visible = True
        'asking_lbl.Visible = True
        'asking_wordage.Visible = True
        'date_listed_panel.Visible = True
        'for_sale_first.Visible = True
      Else
        date_listed.Text = ""
        date_listed_panel.Visible = False
        'asking_lbl.Visible = False
        'asking_wordage.Visible = False
        'date_listed_panel.Visible = False
        'for_sale_first.Visible = False
        'for_sale_second.Visible = False
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Transaction_Tab.ascx.vb - for_sale_SelectedIndexChanged() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub
  Function displayCompany(ByVal company As Integer, ByVal client As Boolean)
    displayCompany = ""
    Try
      If Trim(Request("cli_trans")) = "" Then
        client = False
      Else
        client = True
      End If
      Dim cli_trans_id As Integer = 0
      If Trim(Request("cli_trans")) <> "" Then
        cli_trans_id = CInt(Trim(Request("cli_trans")))
      End If
      Dim comp_info As String = ""
      aTempTable2 = New DataTable
      Dim trans_id As Integer = 0
      If Trim(Request("trans")) <> "" Then
        trans_id = CInt(Trim(Request("trans")))
      End If
      If client = True Then
        aTempTable2 = aclsData_Temp.Get_Client_Transactions_Company(company, cli_trans_id)
        '---------------------------CLIENT TRANSACTION COMPANY-----------------------------------------
        If Not IsNothing(aTempTable2) Then
          If aTempTable2.Rows.Count > 0 Then
            For Each q In aTempTable2.Rows
              comp_info = ""
              'GridView1.DataSource = aTempTable2
              'GridView1.DataBind()

              If Not IsDBNull(q("clitcomp_name")) Then
                comp_info = comp_info & q("clitcomp_name") & " "
              End If
              If Not IsDBNull(q("clitcomp_city")) Then
                comp_info = comp_info & q("clitcomp_city") & " "
              End If
              If Not IsDBNull(q("clitcomp_state")) Then
                comp_info = comp_info & q("clitcomp_state") & " "
              End If

            Next
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - DisplayCompany() - " & error_string)
          End If
          displayError()
        End If
      Else
        aTempTable2 = aclsData_Temp.Get_JETNET_Transactions_Company(company, trans_id)
        'JETNET TRANSACTION COMPANY----------------------------------------------------------------------
        If Not IsNothing(aTempTable2) Then
          If aTempTable2.Rows.Count > 0 Then
            For Each q In aTempTable2.Rows
              comp_info = ""
              'GridView1.DataSource = aTempTable2
              'GridView1.DataBind()

              If Not IsDBNull(q("tcomp_name")) Then
                comp_info = comp_info & q("tcomp_name") & " "
              End If
              If Not IsDBNull(q("tcomp_city")) Then
                comp_info = comp_info & q("tcomp_city") & " "
              End If
              If Not IsDBNull(q("tcomp_state")) Then
                comp_info = comp_info & q("tcomp_state") & " "
              End If

            Next
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - DisplayCompany() - " & error_string)
          End If
          displayError()
        End If

      End If
      displayCompany = displayCompany & comp_info
    Catch ex As Exception
      error_string = "Aircraft_Edit_Transaction_Tab.ascx.vb - displayCompany() - " & ex.Message
      LogError(error_string)
    End Try
  End Function
  Function whatRelationship(ByVal rel As Object)
    whatRelationship = ""
    Try
      If Not IsDBNull(rel) Then
        If IsNumeric(rel) Then
          aTempTable = aclsData_Temp.Get_Client_Aircraft_Contact_Type(rel)
          If Not IsNothing(aTempTable) Then
            If aTempTable.Rows.Count > 0 Then
              For Each q In aTempTable.Rows
                If Not IsDBNull(q("cliact_name")) Then
                  whatRelationship = q("cliact_name")
                End If
              Next
            End If
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = aclsData_Temp.class_error
              LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - WhatRelationship() - " & error_string)
            End If
            displayError()
          End If

        End If
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Transaction_Tab.ascx.vb - whatRelationship() - " & ex.Message
      LogError(error_string)
    End Try
  End Function
  Function displayContact(ByVal contact As Integer, ByVal client As Boolean)
    displayContact = ""
    Try
      If Trim(Request("cli_trans")) = "" Then
        client = False
      Else
        client = True
      End If

      Dim cli_trans_id As Integer = 0
      If Trim(Request("cli_trans")) <> "" Then
        cli_trans_id = CInt(Trim(Request("cli_trans")))
      End If
      Dim comp_info As String = ""
      aTempTable2 = New DataTable
      Dim trans_id As Integer = 0
      If Trim(Request("trans")) <> "" Then
        trans_id = CInt(Trim(Request("trans")))
      End If
      If client = True Then
        '--------------------------------------------------CLIENT CONTACT TABLE-----------------------------------------------------------------------------
        aTempTable = aclsData_Temp.Get_Client_Transactions_Contact_ContactID(contact, cli_trans_id)
        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            For Each q In aTempTable.Rows
              displayContact = ""
              If Not IsDBNull(q("clitcontact_sirname")) Then
                displayContact = displayContact & q("clitcontact_sirname") & " "
              End If
              If Not IsDBNull(q("clitcontact_first_name")) Then
                displayContact = displayContact & q("clitcontact_first_name") & " "
              End If
              If Not IsDBNull(q("clitcontact_middle_initial")) Then
                displayContact = displayContact & q("clitcontact_middle_initial") & " "
              End If
              If Not IsDBNull(q("clitcontact_last_name")) Then
                displayContact = displayContact & q("clitcontact_last_name") & " <br />"
              End If
              If Not IsDBNull(q("clitcontact_title")) Then
                displayContact = displayContact & q("clitcontact_title") & "<br /> "
              End If
              If Not IsDBNull(q("clitcontact_email_address")) Then
                displayContact = displayContact & q("clitcontact_email_address")
              End If

            Next
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - DisplayContact() - " & error_string)
          End If
          displayError()
        End If

      Else
        '------------------------------------JETNET CONTACT TABLE-------------------------------------------------------------------------------------

        aTempTable = aclsData_Temp.Get_JETNET_Transactions_Contact_ContactID(contact, trans_id)
        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            For Each q In aTempTable.Rows
              displayContact = ""
              If Not IsDBNull(q("tcontact_sirname")) Then
                displayContact = displayContact & q("tcontact_sirname") & " "
              End If
              If Not IsDBNull(q("tcontact_first_name")) Then
                displayContact = displayContact & q("tcontact_first_name") & " "
              End If
              If Not IsDBNull(q("tcontact_middle_initial")) Then
                displayContact = displayContact & q("tcontact_middle_initial") & " "
              End If
              If Not IsDBNull(q("tcontact_last_name")) Then
                displayContact = displayContact & q("tcontact_last_name") & " <br />"
              End If
              If Not IsDBNull(q("tcontact_title")) Then
                displayContact = displayContact & q("tcontact_title") & "<br /> "
              End If
              If Not IsDBNull(q("tcontact_email_address")) Then
                displayContact = displayContact & q("tcontact_email_address")
              End If

            Next
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - DisplayContact() - " & error_string)
          End If
          displayError()
        End If
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Transaction_Tab.ascx.vb - displayContact() - " & ex.Message
      LogError(error_string)
    End Try
    displayContact = displayContact
  End Function
  Function Fill_Client_Company(ByVal jetnet_comp_id As Integer, ByVal trans_id As Integer) As Integer
    'This function fills the client company based on a transaction record.
    Fill_Client_Company = 0
    Try
      Dim errored As String = ""
      Dim startdate As String = ""
      Dim comp_name As String = ""
      Dim aclsClient_Company As New clsClient_Company
      aTempTable = aclsData_Temp.Get_JETNET_Transactions_Company(jetnet_comp_id, trans_id)
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          For Each q As DataRow In aTempTable.Rows
            aclsClient_Company.clicomp_user_id = Session.Item("localUser").crmLocalUserID
            aclsClient_Company.clicomp_name = CStr(IIf(Not IsDBNull(q("tcomp_name")), q("tcomp_name"), ""))
            comp_name = CStr(IIf(Not IsDBNull(q("tcomp_name")), q("tcomp_name"), ""))
            aclsClient_Company.clicomp_alternate_name_type = CStr(IIf(Not IsDBNull(q("tcomp_alternate_name_type")), q("tcomp_alternate_name_type"), ""))
            aclsClient_Company.clicomp_alternate_name = CStr(IIf(Not IsDBNull(q("tcomp_alternate_name")), q("tcomp_alternate_name"), ""))
            aclsClient_Company.clicomp_address1 = CStr(IIf(Not IsDBNull(q("tcomp_address1")), q("tcomp_address1"), ""))
            aclsClient_Company.clicomp_address2 = CStr(IIf(Not IsDBNull(q("tcomp_address2")), q("tcomp_address2"), ""))
            aclsClient_Company.clicomp_city = CStr(IIf(Not IsDBNull(q("tcomp_city")), q("tcomp_city"), ""))
            aclsClient_Company.clicomp_state = CStr(IIf(Not IsDBNull(q("tcomp_state")), q("tcomp_state"), ""))
            aclsClient_Company.clicomp_zip_code = CStr(IIf(Not IsDBNull(q("tcomp_zip_code")), q("tcomp_zip_code"), ""))
            aclsClient_Company.clicomp_country = CStr(IIf(Not IsDBNull(q("tcomp_country")), q("tcomp_country"), ""))
            aclsClient_Company.clicomp_agency_type = CStr(IIf(Not IsDBNull(q("tcomp_agency_type")), q("tcomp_agency_type"), ""))
            aclsClient_Company.clicomp_web_address = CStr(IIf(Not IsDBNull(q("tcomp_web_address")), q("tcomp_web_address"), ""))
            aclsClient_Company.clicomp_email_address = CStr(IIf(Not IsDBNull(q("tcomp_email_address")), q("tcomp_email_address"), ""))
            aclsClient_Company.clicomp_status = "Y"
            startdate = Now()
            aclsClient_Company.clicomp_date_updated = startdate
            startdate = Year(startdate) & "-" & Month(startdate) & "-" & (Day(startdate)) & " " & FormatDateTime(startdate, 4) & ":" & Second(startdate)
            aclsClient_Company.clicomp_jetnet_comp_id = q("tcomp_id")
          Next
          Dim new_comp_id As Integer = 0
          If aclsData_Temp.Insert_Client_Company(aclsClient_Company) = True Then
            aTempTable = aclsData_Temp.Get_Insert_Client_Company(comp_name, startdate, "Y")
            If Not IsNothing(aTempTable) Then 'not nothing
              If aTempTable.Rows.Count > 0 Then
                Fill_Client_Company = aTempTable.Rows(0).Item("comp_id")
                new_comp_id = aTempTable.Rows(0).Item("comp_id")
              End If
            Else
              If aclsData_Temp.class_error <> "" Then
                error_string = aclsData_Temp.class_error
                LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_Client_Company() - " & error_string)
              End If
              displayError()
            End If
          End If

          aTempTable2 = aclsData_Temp.Get_JETNET_Transactions_PhoneNbrs_compID(jetnet_comp_id, trans_id)
          '' check the state of the DataTable
          If Not IsNothing(aTempTable2) Then
            If aTempTable2.Rows.Count > 0 Then
              Dim aclsClient_Phone_Numbers As New clsClient_Phone_Numbers
              For Each r As DataRow In aTempTable2.Rows
                aclsClient_Phone_Numbers.clipnum_type = CStr(IIf(Not IsDBNull(r("tpnum_type")), r("tpnum_type"), ""))
                aclsClient_Phone_Numbers.clipnum_number = CStr(IIf(Not IsDBNull(r("tpnum_number")), r("tpnum_number"), ""))
                aclsClient_Phone_Numbers.clipnum_comp_id = new_comp_id
                aclsClient_Phone_Numbers.clipnum_contact_id = 0

                If aclsData_Temp.Insert_Client_PhoneNumbers(aclsClient_Phone_Numbers) = True Then
                  ' Response.Write("insert phone")
                Else
                  If aclsData_Temp.class_error <> "" Then
                    error_string = aclsData_Temp.class_error
                    LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_Client_Company() - " & error_string)
                  End If
                  displayError()
                End If
              Next
            End If
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = aclsData_Temp.class_error
              LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_Client_Company() - " & error_string)
            End If
            displayError()
          End If
        End If
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_Client_Company() - " & ex.Message
      LogError(error_string)
    End Try
  End Function
  Function Fill_Client_Company_FromJETNET(ByVal jetnet_comp_id As Integer, ByVal trans_id As Integer) As Integer
    Try
      'This function fills the client company based on a jetnet record.
      Fill_Client_Company_FromJETNET = 0
      Dim errored As String = ""
      Dim startdate As String = ""
      Dim comp_name As String = ""
      Dim aclsClient_Company As New clsClient_Company
      aTempTable = aclsData_Temp.GetCompanyInfo_ID(jetnet_comp_id, "JETNET", 0)
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          For Each q As DataRow In aTempTable.Rows
            aclsClient_Company.clicomp_user_id = Session.Item("localUser").crmLocalUserID
            aclsClient_Company.clicomp_name = CStr(IIf(Not IsDBNull(q("comp_name")), q("comp_name"), ""))
            comp_name = CStr(IIf(Not IsDBNull(q("comp_name")), q("comp_name"), ""))
            aclsClient_Company.clicomp_alternate_name_type = CStr(IIf(Not IsDBNull(q("comp_alternate_name_type")), q("comp_alternate_name_type"), ""))
            aclsClient_Company.clicomp_alternate_name = CStr(IIf(Not IsDBNull(q("comp_alternate_name")), q("comp_alternate_name"), ""))
            aclsClient_Company.clicomp_address1 = CStr(IIf(Not IsDBNull(q("comp_address1")), q("comp_address1"), ""))
            aclsClient_Company.clicomp_address2 = CStr(IIf(Not IsDBNull(q("comp_address2")), q("comp_address2"), ""))
            aclsClient_Company.clicomp_city = CStr(IIf(Not IsDBNull(q("comp_city")), q("comp_city"), ""))
            aclsClient_Company.clicomp_state = CStr(IIf(Not IsDBNull(q("comp_state")), q("comp_state"), ""))
            aclsClient_Company.clicomp_zip_code = CStr(IIf(Not IsDBNull(q("comp_zip_code")), q("comp_zip_code"), ""))
            aclsClient_Company.clicomp_country = CStr(IIf(Not IsDBNull(q("comp_country")), q("comp_country"), ""))
            aclsClient_Company.clicomp_agency_type = CStr(IIf(Not IsDBNull(q("comp_agency_type")), q("comp_agency_type"), ""))
            aclsClient_Company.clicomp_web_address = CStr(IIf(Not IsDBNull(q("comp_web_address")), q("comp_web_address"), ""))
            aclsClient_Company.clicomp_email_address = CStr(IIf(Not IsDBNull(q("comp_email_address")), q("comp_email_address"), ""))
            aclsClient_Company.clicomp_status = "Y"
            startdate = Now()
            aclsClient_Company.clicomp_date_updated = startdate
            startdate = Year(startdate) & "-" & Month(startdate) & "-" & (Day(startdate)) & " " & FormatDateTime(startdate, 4) & ":" & Second(startdate)
            aclsClient_Company.clicomp_jetnet_comp_id = q("comp_id")
          Next
          Dim new_comp_id As Integer = 0
          If aclsData_Temp.Insert_Client_Company(aclsClient_Company) = True Then
            aTempTable = aclsData_Temp.Get_Insert_Client_Company(comp_name, startdate, "Y")
            If Not IsNothing(aTempTable) Then 'not nothing
              If aTempTable.Rows.Count > 0 Then
                Fill_Client_Company_FromJETNET = aTempTable.Rows(0).Item("comp_id")
                new_comp_id = aTempTable.Rows(0).Item("comp_id")
              End If
            Else
              If aclsData_Temp.class_error <> "" Then
                error_string = aclsData_Temp.class_error
                LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_Client_Company_FromJETNET() - " & error_string)
              End If
              displayError()
            End If
          End If

          aTempTable2 = aclsData_Temp.GetPhoneNumbers(jetnet_comp_id, 0, "JETNET", 0)
          '' check the state of the DataTable
          If Not IsNothing(aTempTable2) Then
            If aTempTable2.Rows.Count > 0 Then
              Dim aclsClient_Phone_Numbers As New clsClient_Phone_Numbers
              For Each r As DataRow In aTempTable2.Rows
                aclsClient_Phone_Numbers.clipnum_type = CStr(IIf(Not IsDBNull(r("pnum_type")), r("pnum_type"), ""))
                aclsClient_Phone_Numbers.clipnum_number = CStr(IIf(Not IsDBNull(r("pnum_number")), r("pnum_number"), ""))
                aclsClient_Phone_Numbers.clipnum_comp_id = new_comp_id
                aclsClient_Phone_Numbers.clipnum_contact_id = 0

                If aclsData_Temp.Insert_Client_PhoneNumbers(aclsClient_Phone_Numbers) = True Then
                  ' Response.Write("insert phone")
                Else
                  If aclsData_Temp.class_error <> "" Then
                    error_string = aclsData_Temp.class_error
                    LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_Client_Company_FromJETNET() - " & error_string)
                  End If
                  displayError()
                End If
              Next
            End If
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = aclsData_Temp.class_error
              LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_Client_Company_FromJETNET() - " & error_string)
            End If
            displayError()
          End If
        End If
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_Client_Company_FromJETNET() - " & ex.Message
      LogError(error_string)
    End Try
  End Function
  Function Fill_Transaction_Company(ByVal client_comp_id As Integer, ByVal trans_id As Integer, ByVal jetnet_comp_id As Integer, ByVal client_trans_id As Integer) As Integer
    'This fills the company based on the jetnet transaction record
    Fill_Transaction_Company = 0
    Try

      aTempTable2 = aclsData_Temp.Get_Client_Transactions_Company(client_comp_id, client_trans_id)
      If aTempTable2.Rows.Count = 0 Then 'added this check because if the company already exists in the transaction table, we don't want to add it again. 


        Dim aclsClient_Transactions_Company As New clsClient_Transactions_Company

        aTempTable = aclsData_Temp.Get_JETNET_Transactions_Company(jetnet_comp_id, trans_id)
        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            For Each q As DataRow In aTempTable.Rows
              aclsClient_Transactions_Company.clitcomp_id = client_comp_id
              aclsClient_Transactions_Company.clitcomp_action_date = Now()
              aclsClient_Transactions_Company.clitcomp_address1 = CStr(IIf(Not IsDBNull(q("tcomp_address1")), q("tcomp_address1"), ""))
              aclsClient_Transactions_Company.clitcomp_address2 = CStr(IIf(Not IsDBNull(q("tcomp_address2")), q("tcomp_address2"), ""))
              aclsClient_Transactions_Company.clitcomp_agency_type = CStr(IIf(Not IsDBNull(q("tcomp_agency_type")), q("tcomp_agency_type"), ""))
              aclsClient_Transactions_Company.clitcomp_alternate_name = CStr(IIf(Not IsDBNull(q("tcomp_alternate_name")), q("tcomp_alternate_name"), ""))
              aclsClient_Transactions_Company.clitcomp_alternate_name_type = CStr(IIf(Not IsDBNull(q("tcomp_alternate_name_type")), q("tcomp_alternate_name_type"), ""))
              aclsClient_Transactions_Company.clitcomp_city = CStr(IIf(Not IsDBNull(q("tcomp_city")), q("tcomp_city"), ""))
              aclsClient_Transactions_Company.clitcomp_country = CStr(IIf(Not IsDBNull(q("tcomp_country")), q("tcomp_country"), ""))
              aclsClient_Transactions_Company.clitcomp_email_address = CStr(IIf(Not IsDBNull(q("tcomp_email_address")), q("tcomp_email_address"), ""))
              aclsClient_Transactions_Company.clitcomp_name = CStr(IIf(Not IsDBNull(q("tcomp_name")), q("tcomp_name"), ""))
              aclsClient_Transactions_Company.clitcomp_state = CStr(IIf(Not IsDBNull(q("tcomp_state")), q("tcomp_state"), ""))
              aclsClient_Transactions_Company.clitcomp_trans_id = client_trans_id
              aclsClient_Transactions_Company.clitcomp_web_address = CStr(IIf(Not IsDBNull(q("tcomp_web_address")), q("tcomp_web_address"), ""))
              aclsClient_Transactions_Company.clitcomp_zip_code = CStr(IIf(Not IsDBNull(q("tcomp_zip_code")), q("tcomp_zip_code"), ""))
            Next
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_Transaction_Company() - " & error_string)
          End If
          displayError()
        End If


        Fill_Transaction_Company = aclsData_Temp.Insert_Client_Transactions_Company(aclsClient_Transactions_Company)

        aTempTable2 = aclsData_Temp.Get_JETNET_Transactions_PhoneNbrs_compID(jetnet_comp_id, trans_id)
        '' check the state of the DataTable
        If Not IsNothing(aTempTable2) Then
          If aTempTable2.Rows.Count > 0 Then
            For Each m As DataRow In aTempTable2.Rows
              Dim aclsClient_Transactions_Phone_Numbers As New clsClient_Transactions_Phone_Numbers
              aclsClient_Transactions_Phone_Numbers.clitpnum_comp_id = client_comp_id
              aclsClient_Transactions_Phone_Numbers.clitpnum_contact_id = 0
              aclsClient_Transactions_Phone_Numbers.clitpnum_number = CStr(IIf(Not IsDBNull(m("tpnum_number")), m("tpnum_number"), ""))
              aclsClient_Transactions_Phone_Numbers.clitpnum_trans_id = trans_id
              aclsClient_Transactions_Phone_Numbers.clitpnum_type = CStr(IIf(Not IsDBNull(m("tpnum_type")), m("tpnum_type"), ""))
              If aclsData_Temp.Insert_Client_Transactions_PhoneNbrs(aclsClient_Transactions_Phone_Numbers) = True Then
                '  Response.Write("insert contact phone Number<br />")
              Else
                'Response.Write("Update Client Contact Fail")
              End If
            Next
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_Transaction_Company() - " & error_string)
          End If
          displayError()
        End If
      End If

    Catch ex As Exception
      error_string = "Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_Transaction_Company() - " & ex.Message
      LogError(error_string)
    End Try
  End Function
  Function Fill_Transaction_Company_FromJETNET(ByVal client_comp_id As Integer, ByVal trans_id As Integer, ByVal jetnet_comp_id As Integer, ByVal client_trans_id As Integer, ByVal source As String) As Integer
    Fill_Transaction_Company_FromJETNET = 0
    Try
      'This fills everything based on the jetnet record (not from transaction table)
      Dim aclsClient_Transactions_Company As New clsClient_Transactions_Company

      aTempTable = aclsData_Temp.GetCompanyInfo_ID(jetnet_comp_id, source, 0)
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          For Each q As DataRow In aTempTable.Rows
            aclsClient_Transactions_Company.clitcomp_id = client_comp_id
            aclsClient_Transactions_Company.clitcomp_action_date = Now()
            aclsClient_Transactions_Company.clitcomp_address1 = CStr(IIf(Not IsDBNull(q("comp_address1")), q("comp_address1"), ""))
            aclsClient_Transactions_Company.clitcomp_address2 = CStr(IIf(Not IsDBNull(q("comp_address2")), q("comp_address2"), ""))
            aclsClient_Transactions_Company.clitcomp_agency_type = CStr(IIf(Not IsDBNull(q("comp_agency_type")), q("comp_agency_type"), ""))
            aclsClient_Transactions_Company.clitcomp_alternate_name = CStr(IIf(Not IsDBNull(q("comp_alternate_name")), q("comp_alternate_name"), ""))
            aclsClient_Transactions_Company.clitcomp_alternate_name_type = CStr(IIf(Not IsDBNull(q("comp_alternate_name_type")), q("comp_alternate_name_type"), ""))
            aclsClient_Transactions_Company.clitcomp_city = CStr(IIf(Not IsDBNull(q("comp_city")), q("comp_city"), ""))
            aclsClient_Transactions_Company.clitcomp_country = CStr(IIf(Not IsDBNull(q("comp_country")), q("comp_country"), ""))
            aclsClient_Transactions_Company.clitcomp_email_address = CStr(IIf(Not IsDBNull(q("comp_email_address")), q("comp_email_address"), ""))
            aclsClient_Transactions_Company.clitcomp_name = CStr(IIf(Not IsDBNull(q("comp_name")), q("comp_name"), ""))
            aclsClient_Transactions_Company.clitcomp_state = CStr(IIf(Not IsDBNull(q("comp_state")), q("comp_state"), ""))
            aclsClient_Transactions_Company.clitcomp_trans_id = client_trans_id
            aclsClient_Transactions_Company.clitcomp_web_address = CStr(IIf(Not IsDBNull(q("comp_web_address")), q("comp_web_address"), ""))
            aclsClient_Transactions_Company.clitcomp_zip_code = CStr(IIf(Not IsDBNull(q("comp_zip_code")), q("comp_zip_code"), ""))
          Next
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_Transaction_Company_FromJETNET() - " & error_string)
        End If
        displayError()
      End If

      Fill_Transaction_Company_FromJETNET = aclsData_Temp.Insert_Client_Transactions_Company(aclsClient_Transactions_Company)

      aTempTable2 = aclsData_Temp.GetPhoneNumbers(jetnet_comp_id, 0, source, 0)
      '' check the state of the DataTable
      If Not IsNothing(aTempTable2) Then
        If aTempTable2.Rows.Count > 0 Then
          For Each m As DataRow In aTempTable2.Rows
            Dim aclsClient_Transactions_Phone_Numbers As New clsClient_Transactions_Phone_Numbers
            aclsClient_Transactions_Phone_Numbers.clitpnum_comp_id = client_comp_id
            aclsClient_Transactions_Phone_Numbers.clitpnum_contact_id = 0
            aclsClient_Transactions_Phone_Numbers.clitpnum_number = CStr(IIf(Not IsDBNull(m("pnum_number")), m("pnum_number"), ""))
            aclsClient_Transactions_Phone_Numbers.clitpnum_trans_id = trans_id
            aclsClient_Transactions_Phone_Numbers.clitpnum_type = CStr(IIf(Not IsDBNull(m("pnum_type")), m("pnum_type"), ""))
            If aclsData_Temp.Insert_Client_Transactions_PhoneNbrs(aclsClient_Transactions_Phone_Numbers) = True Then
              '  Response.Write("insert contact phone Number<br />")
            Else
              'Response.Write("Update Client Contact Fail")
            End If
          Next
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_Transaction_Company_FromJETNET() - " & error_string)
        End If
        displayError()
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_Transaction_Company_FromJETNET() - " & ex.Message
      LogError(error_string)
    End Try
  End Function
  Function Fill_Client_Contacts(ByVal jetnet_comp_id As Integer, ByVal client_comp_id As Integer, ByVal trans_id As Integer) As Integer
    Try
      aTempTable = aclsData_Temp.Get_JETNET_Transactions_Contact_compID(jetnet_comp_id, trans_id)

      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then

          'This loops through all of the contacts. 
          For Each r As DataRow In aTempTable.Rows

            Dim aclsClient_Contact As New clsClient_Contact
            aclsClient_Contact.clicontact_user_id = Session.Item("localUser").crmLocalUserID
            aclsClient_Contact.clicontact_sirname = CStr(IIf(Not IsDBNull(r("tcontact_sirname")), r("tcontact_sirname"), ""))
            aclsClient_Contact.clicontact_first_name = CStr(IIf(Not IsDBNull(r("tcontact_first_name")), r("tcontact_first_name"), ""))
            aclsClient_Contact.clicontact_middle_initial = CStr(IIf(Not IsDBNull(r("tcontact_middle_initial")), r("tcontact_middle_initial"), ""))
            aclsClient_Contact.clicontact_last_name = CStr(IIf(Not IsDBNull(r("tcontact_last_name")), r("tcontact_last_name"), ""))
            aclsClient_Contact.clicontact_suffix = CStr(IIf(Not IsDBNull(r("tcontact_suffix")), r("tcontact_suffix"), ""))
            aclsClient_Contact.clicontact_title = CStr(IIf(Not IsDBNull(r("tcontact_title")), r("tcontact_title"), ""))
            aclsClient_Contact.clicontact_email_address = CStr(IIf(Not IsDBNull(r("tcontact_email_address")), r("tcontact_email_address"), ""))
            aclsClient_Contact.clicontact_date_updated = Now()
            aclsClient_Contact.clicontact_jetnet_contact_id = r("tcontact_id")
            aclsClient_Contact.clicontact_comp_id = client_comp_id
            aclsClient_Contact.clicontact_status = "Y"
            Dim contact_id_new As Integer


            'This attempts to insert this contact record. 
            If aclsData_Temp.Insert_Client_Contact(aclsClient_Contact) = True Then
              'not done yet. Now we have to get the phone numbers based on the contact and insert them.
              'First we need to get the contact id of what we just inserted.. 
              'Have to get the new contact ID 
              aTempTable2 = aclsData_Temp.GetContactInfo_JETNET_ID(r("tcontact_id"), "Y")
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
                  LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_Client_Contacts() - " & error_string)
                End If
                displayError()
              End If

              'Inserting new contact phone numbers. 
              aTempTable2 = aclsData_Temp.Get_JETNET_Transactions_PhoneNbrs_contactID(r("tcontact_id"), trans_id)
              If Not IsNothing(aTempTable2) Then
                If aTempTable2.Rows.Count > 0 Then
                  For Each q As DataRow In aTempTable2.Rows
                    Dim aclsClient_Phone_Numbers As New clsClient_Phone_Numbers
                    aclsClient_Phone_Numbers.clipnum_type = CStr(IIf(Not IsDBNull(q("tpnum_type")), q("tpnum_type"), ""))
                    aclsClient_Phone_Numbers.clipnum_number = CStr(IIf(Not IsDBNull(q("tpnum_number")), q("tpnum_number"), ""))
                    aclsClient_Phone_Numbers.clipnum_comp_id = client_comp_id
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
                  LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_Client_Contacts() - " & error_string)
                End If
                displayError()
              End If
            End If
          Next
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_Client_Contacts() - " & error_string)
        End If
        displayError()
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_Client_Contacts() - " & ex.Message
      LogError(error_string)
    End Try
  End Function
  Function Fill_Client_Contacts_FromJETNET(ByVal jetnet_comp_id As Integer, ByVal client_comp_id As Integer, ByVal trans_id As Integer) As Integer
    'This is where we have to add the contacts that were linked with this company but we have to get htem from the jetnet table, not the jetnet trans table
    Try
      aTempTable = aclsData_Temp.GetContacts(jetnet_comp_id, "JETNET", "Y", 0)

      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then

          'This loops through all of the contacts. 
          For Each r As DataRow In aTempTable.Rows

            Dim aclsClient_Contact As New clsClient_Contact
            aclsClient_Contact.clicontact_user_id = Session.Item("localUser").crmLocalUserID
            aclsClient_Contact.clicontact_sirname = CStr(IIf(Not IsDBNull(r("contact_sirname")), r("contact_sirname"), ""))
            aclsClient_Contact.clicontact_first_name = CStr(IIf(Not IsDBNull(r("contact_first_name")), r("contact_first_name"), ""))
            aclsClient_Contact.clicontact_middle_initial = CStr(IIf(Not IsDBNull(r("contact_middle_initial")), r("contact_middle_initial"), ""))
            aclsClient_Contact.clicontact_last_name = CStr(IIf(Not IsDBNull(r("contact_last_name")), r("contact_last_name"), ""))
            aclsClient_Contact.clicontact_suffix = CStr(IIf(Not IsDBNull(r("contact_suffix")), r("contact_suffix"), ""))
            aclsClient_Contact.clicontact_title = CStr(IIf(Not IsDBNull(r("contact_title")), r("contact_title"), ""))
            aclsClient_Contact.clicontact_email_address = CStr(IIf(Not IsDBNull(r("contact_email_address")), r("contact_email_address"), ""))
            aclsClient_Contact.clicontact_date_updated = Now()
            aclsClient_Contact.clicontact_jetnet_contact_id = r("contact_id")
            aclsClient_Contact.clicontact_comp_id = client_comp_id
            aclsClient_Contact.clicontact_status = "Y"
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
                  LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_Client_Contacts_FromJetnet() - " & error_string)
                End If
                displayError()
              End If

              'Inserting new contact phone numbers. 
              aTempTable2 = aclsData_Temp.GetContact_PhoneNumbers(r("contact_id"))
              If Not IsNothing(aTempTable2) Then
                If aTempTable2.Rows.Count > 0 Then
                  For Each q As DataRow In aTempTable2.Rows
                    Dim aclsClient_Phone_Numbers As New clsClient_Phone_Numbers
                    aclsClient_Phone_Numbers.clipnum_type = CStr(IIf(Not IsDBNull(q("pnum_type")), q("pnum_type"), ""))
                    aclsClient_Phone_Numbers.clipnum_number = CStr(IIf(Not IsDBNull(q("pnum_number")), q("pnum_number"), ""))
                    aclsClient_Phone_Numbers.clipnum_comp_id = client_comp_id
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
                  LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_Client_Contacts_FromJetnet() - " & error_string)
                End If
                displayError()
              End If
            End If
          Next
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_Client_Contacts_FromJetnet() - " & error_string)
        End If
        displayError()
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_Client_Contacts_FromJetnet() - " & ex.Message
      LogError(error_string)
    End Try
  End Function
  Function Fill_Transaction_Contacts(ByVal client_comp_id As Integer, ByVal trans_id As Integer, ByVal jetnet_comp_id As Integer, ByVal client_trans_id As Integer) As Integer
    Fill_Transaction_Contacts = 0
    Try
      'This is where we have to add the contacts that were already linked with this company. 
      'Make sure to use the jetnet_id id. This is important because we're using jetnet ID to get the existing contacts. 

      aTempTable = aclsData_Temp.Get_JETNET_Transactions_Contact_compID(jetnet_comp_id, trans_id)

      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then

          'This loops through all of the contacts. 
          For Each r As DataRow In aTempTable.Rows
            '


            Dim contact_id_new As Integer = 0
            If r("tcontact_id") <> 0 Then
              aTempTable = aclsData_Temp.GetContactInfo_JETNET_ID(r("tcontact_id"), "Y")
              If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then
                  For Each f As DataRow In aTempTable.Rows
                    contact_id_new = f("contact_id")
                  Next 'this loops through contact ID record
                Else 'rows = 0 
                End If
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_Transaction_Contacts() - " & error_string)
                End If
                displayError()
              End If
            End If

            aTempTable2 = aclsData_Temp.Get_Client_Transactions_Contact_ContactID(contact_id_new, client_trans_id)
            If aTempTable2.Rows.Count = 0 Then 'added this check because if the contact already exists in the transaction table, we don't want to add it again. 

              Dim aclsclient_transactions_contact As New clsclient_transactions_contact
              aclsclient_transactions_contact.clitcontact_sirname = CStr(IIf(Not IsDBNull(r("tcontact_sirname")), r("tcontact_sirname"), ""))
              aclsclient_transactions_contact.clitcontact_first_name = CStr(IIf(Not IsDBNull(r("tcontact_first_name")), r("tcontact_first_name"), ""))
              aclsclient_transactions_contact.clitcontact_middle_initial = CStr(IIf(Not IsDBNull(r("tcontact_middle_initial")), r("tcontact_middle_initial"), ""))
              aclsclient_transactions_contact.clitcontact_last_name = CStr(IIf(Not IsDBNull(r("tcontact_last_name")), r("tcontact_last_name"), ""))
              aclsclient_transactions_contact.clitcontact_suffix = CStr(IIf(Not IsDBNull(r("tcontact_suffix")), r("tcontact_suffix"), ""))
              aclsclient_transactions_contact.clitcontact_title = CStr(IIf(Not IsDBNull(r("tcontact_title")), r("tcontact_title"), ""))
              aclsclient_transactions_contact.clitcontact_email_address = CStr(IIf(Not IsDBNull(r("tcontact_email_address")), r("tcontact_email_address"), ""))
              aclsclient_transactions_contact.clitcontact_action_date = Now()
              aclsclient_transactions_contact.clitcontact_trans_id = client_trans_id
              aclsclient_transactions_contact.clitcontact_comp_id = client_comp_id
              aclsclient_transactions_contact.clitcontact_id = contact_id_new


              ''This attempts to insert this contact record. 
              If aclsData_Temp.Insert_Client_Transactions_Contact(aclsclient_transactions_contact) = True Then

                '    'Inserting new contact phone numbers. 

                aTempTable2 = aclsData_Temp.Get_JETNET_Transactions_PhoneNbrs_contactID(contact_id_new, trans_id)
                If Not IsNothing(aTempTable2) Then
                  If aTempTable2.Rows.Count > 0 Then
                    For Each q As DataRow In aTempTable2.Rows
                      Dim aclsClient_Transactions_Phone_Numbers As New clsClient_Transactions_Phone_Numbers
                      aclsClient_Transactions_Phone_Numbers.clitpnum_comp_id = client_comp_id
                      aclsClient_Transactions_Phone_Numbers.clitpnum_contact_id = contact_id_new
                      aclsClient_Transactions_Phone_Numbers.clitpnum_number = CStr(IIf(Not IsDBNull(q("tpnum_number")), q("tpnum_number"), ""))
                      aclsClient_Transactions_Phone_Numbers.clitpnum_trans_id = trans_id
                      aclsClient_Transactions_Phone_Numbers.clitpnum_type = CStr(IIf(Not IsDBNull(q("tpnum_type")), q("tpnum_type"), ""))
                      If aclsData_Temp.Insert_Client_Transactions_PhoneNbrs(aclsClient_Transactions_Phone_Numbers) = True Then
                        '  Response.Write("insert contact phone Number<br />")
                      Else
                        'Response.Write("Update Client Contact Fail")
                      End If
                    Next 'This loops through new contact phone numbers


                  Else
                    If aclsData_Temp.class_error <> "" Then
                      error_string = aclsData_Temp.class_error
                      LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_Transaction_Contacts() - " & error_string)
                    End If
                    displayError()
                  End If
                Else
                  If aclsData_Temp.class_error <> "" Then
                    error_string = aclsData_Temp.class_error
                    LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_Transaction_Contacts() - " & error_string)
                  End If
                  displayError()
                End If
              End If


            End If
          Next
        End If
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_Transaction_Contacts() - " & ex.Message
      LogError(error_string)
    End Try

  End Function
  Function Fill_Transaction_Contacts_FromJETNET(ByVal client_comp_id As Integer, ByVal trans_id As Integer, ByVal jetnet_comp_id As Integer, ByVal client_trans_id As Integer, ByVal source As String) As Integer
    Fill_Transaction_Contacts_FromJETNET = 0
    Try
      'This is where we have to add the contacts that were linked with this company but we have to get htem from the jetnet table, not the jetnet trans table 
      'Make sure to use the jetnet_id id. This is important because we're using jetnet ID to get the existing contacts. 
      aTempTable = aclsData_Temp.GetContacts(jetnet_comp_id, source, "Y", 0)

      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then

          'This loops through all of the contacts. 
          For Each r As DataRow In aTempTable.Rows
            '

            Dim contact_id_new As Integer = 0
            If r("contact_id") <> 0 And source = "JETNET" Then
              aTempTable = aclsData_Temp.GetContactInfo_JETNET_ID(r("contact_id"), "Y")
              If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then
                  For Each f As DataRow In aTempTable.Rows
                    contact_id_new = f("contact_id")
                  Next 'this loops through contact ID record
                Else 'rows = 0 
                End If
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_Transaction_Contacts_FromJETNET() - " & error_string)
                End If
                displayError()
              End If
            Else
              contact_id_new = r("contact_id")
            End If

            Dim aclsclient_transactions_contact As New clsclient_transactions_contact
            aclsclient_transactions_contact.clitcontact_sirname = CStr(IIf(Not IsDBNull(r("contact_sirname")), r("contact_sirname"), ""))
            aclsclient_transactions_contact.clitcontact_first_name = CStr(IIf(Not IsDBNull(r("contact_first_name")), r("contact_first_name"), ""))
            aclsclient_transactions_contact.clitcontact_middle_initial = CStr(IIf(Not IsDBNull(r("contact_middle_initial")), r("contact_middle_initial"), ""))
            aclsclient_transactions_contact.clitcontact_last_name = CStr(IIf(Not IsDBNull(r("contact_last_name")), r("contact_last_name"), ""))
            aclsclient_transactions_contact.clitcontact_suffix = CStr(IIf(Not IsDBNull(r("contact_suffix")), r("contact_suffix"), ""))
            aclsclient_transactions_contact.clitcontact_title = CStr(IIf(Not IsDBNull(r("contact_title")), r("contact_title"), ""))
            aclsclient_transactions_contact.clitcontact_email_address = CStr(IIf(Not IsDBNull(r("contact_email_address")), r("contact_email_address"), ""))
            aclsclient_transactions_contact.clitcontact_action_date = Now()
            aclsclient_transactions_contact.clitcontact_trans_id = client_trans_id
            aclsclient_transactions_contact.clitcontact_comp_id = client_comp_id
            aclsclient_transactions_contact.clitcontact_id = contact_id_new


            ''This attempts to insert this contact record. 
            If aclsData_Temp.Insert_Client_Transactions_Contact(aclsclient_transactions_contact) = True Then

              '    'Inserting new contact phone numbers. 
              aTempTable2 = aclsData_Temp.GetContact_PhoneNumbers(r("contact_id"))
              If Not IsNothing(aTempTable2) Then
                If aTempTable2.Rows.Count > 0 Then
                  For Each q As DataRow In aTempTable2.Rows
                    Dim aclsClient_Transactions_Phone_Numbers As New clsClient_Transactions_Phone_Numbers
                    aclsClient_Transactions_Phone_Numbers.clitpnum_comp_id = client_comp_id
                    aclsClient_Transactions_Phone_Numbers.clitpnum_contact_id = contact_id_new
                    aclsClient_Transactions_Phone_Numbers.clitpnum_number = CStr(IIf(Not IsDBNull(q("pnum_number")), q("pnum_number"), ""))
                    aclsClient_Transactions_Phone_Numbers.clitpnum_trans_id = trans_id
                    aclsClient_Transactions_Phone_Numbers.clitpnum_type = CStr(IIf(Not IsDBNull(q("pnum_type")), q("pnum_type"), ""))
                    If aclsData_Temp.Insert_Client_Transactions_PhoneNbrs(aclsClient_Transactions_Phone_Numbers) = True Then
                      '  Response.Write("insert contact phone Number<br />")
                    Else
                      'Response.Write("Update Client Contact Fail")
                    End If
                  Next 'This loops through new contact phone numbers


                Else
                  If aclsData_Temp.class_error <> "" Then
                    error_string = aclsData_Temp.class_error
                    LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_Transaction_Contacts_FromJETNET() - " & error_string)
                  End If
                  displayError()
                End If
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_Transaction_Contacts_FromJETNET() - " & error_string)
                End If
                displayError()
              End If
            End If
          Next
        End If
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_Transaction_Contacts_FromJETNET() - " & ex.Message
      LogError(error_string)
    End Try

  End Function
  Function Fill_AC_Reference(ByVal trans_id As Integer) As Integer
    Try
      aTempTable2 = aclsData_Temp.Get_JETNET_Transactions_Aircraft_Reference_TransID(trans_id)
      If Not IsNothing(aTempTable2) Then
        If aTempTable2.Rows.Count > 0 Then
          For Each q As DataRow In aTempTable2.Rows
            Dim aclsInsert_Client_Aircraft_Reference As New clsClient_Aircraft_Reference
            Dim comp_new As Integer = 0
            If q("tacref_comp_id") <> 0 Then
              aTempTable = aclsData_Temp.GetCompanyInfo_JETNET_ID(CStr(q("tacref_comp_id")), "")
              If Not IsNothing(aTempTable) Then 'not nothing
                comp_new = aTempTable.Rows(0).Item("comp_id")
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_AC_Reference() - " & error_string)
                End If
                displayError()
              End If
            End If

            aclsInsert_Client_Aircraft_Reference.cliacref_comp_id = comp_new
            aclsInsert_Client_Aircraft_Reference.cliacref_cliac_id = 0

            Dim contact_id_new As Integer = 0
            If q("tacref_contact_id") <> 0 Then
              aTempTable2 = aclsData_Temp.GetContactInfo_JETNET_ID(q("tacref_contact_id"), "Y")
              If Not IsNothing(aTempTable2) Then
                If aTempTable2.Rows.Count > 0 Then
                  For Each f As DataRow In aTempTable2.Rows
                    contact_id_new = f("contact_id")
                  Next 'this loops through contact ID record
                Else 'rows = 0 
                End If
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_AC_Reference() - " & error_string)
                End If
                displayError()
              End If
            End If

            aclsInsert_Client_Aircraft_Reference.cliacref_contact_type = CStr(IIf(Not IsDBNull(q("tacref_contact_type")), q("tacref_contact_type"), ""))
            aclsInsert_Client_Aircraft_Reference.cliacref_contact_id = contact_id_new
            aclsInsert_Client_Aircraft_Reference.cliacref_jetnet_ac_id = CStr(IIf(Not IsDBNull(q("tacref_ac_id")), q("tacref_ac_id"), ""))
            aclsInsert_Client_Aircraft_Reference.cliacref_cliac_id = Session.Item("ListingID")
            aclsInsert_Client_Aircraft_Reference.cliacref_operator_flag = CStr(IIf(Not IsDBNull(q("tacref_operator_flag")), q("tacref_operator_flag"), ""))
            aclsInsert_Client_Aircraft_Reference.cliacref_owner_percentage = CStr(IIf(Not IsDBNull(q("tacref_owner_percentage")), q("tacref_owner_percentage"), "0"))
            aclsInsert_Client_Aircraft_Reference.cliacref_business_type = CStr(IIf(Not IsDBNull(q("tacref_business_type")), q("tacref_business_type"), ""))
            aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_expires = Now()
            aclsInsert_Client_Aircraft_Reference.cliacref_date_fraction_purchased = Now()
            If aclsData_Temp.Insert_Client_Aircraft_Reference(aclsInsert_Client_Aircraft_Reference) = True Then
            Else
              If aclsData_Temp.class_error <> "" Then
                error_string = aclsData_Temp.class_error
                LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_AC_Reference() - " & error_string)
              End If
              displayError()
            End If
          Next
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_AC_Reference() - " & error_string)
        End If
        displayError()
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_AC_Reference() - " & ex.Message
      LogError(error_string)
    End Try
  End Function
  Function Fill_AC_Transaction_Reference(ByVal trans_id As Integer, ByVal client_trans_id As Integer) As Integer
    Fill_AC_Transaction_Reference = 0
    Try
      aTempTable2 = aclsData_Temp.Get_JETNET_Transactions_Aircraft_Reference_TransID(trans_id)
      If Not IsNothing(aTempTable2) Then
        If aTempTable2.Rows.Count > 0 Then
          For Each q As DataRow In aTempTable2.Rows

            Dim aclsClient_Transaction_Aircraft_Reference As New clsClient_Transactions_Sircraft_Reference
            Dim comp_new As Integer = 0
            If q("tacref_comp_id") <> 0 Then
              aTempTable = aclsData_Temp.GetCompanyInfo_JETNET_ID(CStr(q("tacref_comp_id")), "")
              If Not IsNothing(aTempTable) Then 'not nothing
                If aTempTable.Rows.Count > 0 Then
                  comp_new = aTempTable.Rows(0).Item("comp_id")
                End If
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_AC_Transaction_Reference() - " & error_string)
                End If
                displayError()
              End If
            End If

            aclsClient_Transaction_Aircraft_Reference.clitcref_client_comp_id = comp_new


            Dim contact_id_new As Integer = 0
            Dim view_contact As Integer = q("tacref_contact_id")
            If q("tacref_contact_id") <> 0 Then
              aTempTable = aclsData_Temp.GetContactInfo_JETNET_ID(q("tacref_contact_id"), "Y")
              If Not IsNothing(aTempTable) Then
                If aTempTable.Rows.Count > 0 Then
                  For Each f As DataRow In aTempTable.Rows
                    contact_id_new = f("contact_id")
                  Next 'this loops through contact ID record
                Else 'rows = 0 
                End If
              Else
                If aclsData_Temp.class_error <> "" Then
                  error_string = aclsData_Temp.class_error
                  LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_AC_Transaction_Reference() - " & error_string)
                End If
                displayError()
              End If
            End If

            aclsClient_Transaction_Aircraft_Reference.clitcref_id = q("tacref_id")
            aclsClient_Transaction_Aircraft_Reference.clitcref_contact_type = CStr(IIf(Not IsDBNull(q("tacref_contact_type")), q("tacref_contact_type"), ""))
            '/////////////////////////QUESTION QUESTION QUESTION QUESTION QUESTION
            aclsClient_Transaction_Aircraft_Reference.clitcref_client_contact_id = contact_id_new
            aclsClient_Transaction_Aircraft_Reference.clitcref_client_ac_id = CStr(IIf(Not IsDBNull(q("tacref_ac_id")), q("tacref_ac_id"), ""))
            aclsClient_Transaction_Aircraft_Reference.clitcref_operator_flag = CStr(IIf(Not IsDBNull(q("tacref_operator_flag")), q("tacref_operator_flag"), ""))
            aclsClient_Transaction_Aircraft_Reference.clitcref_owner_percentage = CStr(IIf(Not IsDBNull(q("tacref_owner_percentage")), q("tacref_owner_percentage"), "0"))
            aclsClient_Transaction_Aircraft_Reference.clitcref_business_type = CStr(IIf(Not IsDBNull(q("tacref_business_type")), q("tacref_business_type"), ""))
            aclsClient_Transaction_Aircraft_Reference.clitcref_date_fraction_expires = Now()
            aclsClient_Transaction_Aircraft_Reference.clitcref_client_trans_id = client_trans_id
            If aclsData_Temp.Insert_Client_Transactions_aircraft_reference(aclsClient_Transaction_Aircraft_Reference) = True Then
            Else
            End If
          Next
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_AC_Transaction_Reference() - " & error_string)
        End If
        displayError()
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_AC_Transaction_Reference() - " & ex.Message
      LogError(error_string)
    End Try
  End Function

  Function Fill_SINGLE_AC_Transaction_Reference(ByVal contact_type As String, ByVal comp_id As Integer, ByVal contact_id As Integer, ByVal client_ref_id As Integer, ByVal update As Boolean, ByVal insert As Boolean, ByVal client_trans_id As Integer) As Integer
    Fill_SINGLE_AC_Transaction_Reference = 0
    Try
      If update = True Then
        If aclsData_Temp.Delete_Client_Transactions_aircraft_reference(client_ref_id) = 1 Then
          '  Response.Write("removed")
        End If
      End If

      aTempTable = aclsData_Temp.Get_Client_Transactions_aircraft_reference_highestID
      If Not IsNothing(aTempTable) Then 'not nothing
        If aTempTable.Rows.Count > 0 Then
          client_ref_id = aTempTable.Rows(0).Item("clitcref_id")
          client_ref_id = client_ref_id + 1
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_SINGLE_AC_Transaction_Reference() - " & error_string)
        End If
        displayError()
      End If



      Dim aclsClient_Transaction_Aircraft_Reference As New clsClient_Transactions_Sircraft_Reference
      Dim comp_new As Integer = 0
      If comp_id <> 0 Then
        aTempTable = aclsData_Temp.GetCompanyInfo_JETNET_ID(CStr(comp_id), "")
        If Not IsNothing(aTempTable) Then 'not nothing
          If aTempTable.Rows.Count > 0 Then
            comp_new = aTempTable.Rows(0).Item("comp_id")
          Else
            comp_new = comp_id
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_SINGLE_AC_Transaction_Reference() - " & error_string)
          End If
          displayError()
        End If
      End If

      aclsClient_Transaction_Aircraft_Reference.clitcref_client_comp_id = comp_new


      Dim contact_id_new As Integer = 0
      If contact_id <> 0 Then
        aTempTable = aclsData_Temp.GetContactInfo_JETNET_ID(contact_id, "Y")
        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            For Each f As DataRow In aTempTable.Rows
              contact_id_new = f("contact_id")
            Next 'this loops through contact ID record
          Else 'rows = 0 
            contact_id_new = contact_id
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_SINGLE_AC_Transaction_Reference() - " & error_string)
          End If
          displayError()
        End If
      End If

      aclsClient_Transaction_Aircraft_Reference.clitcref_id = client_ref_id
      aclsClient_Transaction_Aircraft_Reference.clitcref_contact_type = contact_type
      aclsClient_Transaction_Aircraft_Reference.clitcref_client_contact_id = contact_id_new
      aclsClient_Transaction_Aircraft_Reference.clitcref_client_ac_id = 0
      aclsClient_Transaction_Aircraft_Reference.clitcref_operator_flag = ""
      aclsClient_Transaction_Aircraft_Reference.clitcref_owner_percentage = 0
      aclsClient_Transaction_Aircraft_Reference.clitcref_business_type = ""

      aclsClient_Transaction_Aircraft_Reference.clitcref_date_fraction_expires = Now()
      aclsClient_Transaction_Aircraft_Reference.clitcref_client_trans_id = client_trans_id

      If comp_id <> 0 Then 'And client_ref_id <> 0 Then
        If aclsData_Temp.Insert_Client_Transactions_aircraft_reference(aclsClient_Transaction_Aircraft_Reference) = True Then
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = aclsData_Temp.class_error
            LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_SINGLE_AC_Transaction_Reference() - " & error_string & client_ref_id & "|" & comp_id & "|" & contact_id)
          End If
          displayError()
        End If
      Else
        attention.Text = "There was an error inserting your transaction. Please try again."
      End If

    Catch ex As Exception
      error_string = "Aircraft_Edit_Transaction_Tab.ascx.vb - Fill_SINGLE_AC_Transaction_Reference() - " & ex.Message
      LogError(error_string)
    End Try
  End Function


  Private Sub comp_search_row_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles comp_search_row.Click
    Try
      company_search_panel_row.Visible = True
      comp_search_row.Visible = False
    Catch ex As Exception
      error_string = "Aircraft_Edit_Transaction_Tab.ascx.vb - comp_search_row_Click() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub

  Private Sub insert_row_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles insert_row.Click
    Try
      new_row.Visible = True
      insert_row.Visible = False
      buttons.Visible = False
    Catch ex As Exception
      error_string = "Aircraft_Edit_Transaction_Tab.ascx.vb - insert_row_Click() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub

  Private Sub company_search_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles company_search_button.Click
    Try
      Dim company_name As ListBox = row_company

      Dim Named As TextBox = row_Name
      company_search_panel_row.Visible = False
      comp_search_row.Visible = True
      row_company.Visible = True
      aTempTable = aclsData_Temp.Company_Search(clsGeneral.clsGeneral.Get_Name_Search_String(Named.Text) & "%", "Y", "JC", "", "", "", Session.Item("localSubscription").crmHelicopter_Flag, Session.Item("localSubscription").crmBusiness_Flag, Session.Item("localSubscription").crmCommercial_Flag, "", "", "", "", "", "", "")
      company_name.Items.Clear()
      company_name.Items.Add(New ListItem("NONE SELECTED", ""))
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          For Each r As DataRow In aTempTable.Rows
            'address, city, state, country
            Dim address_string As String = ""
            If Not IsDBNull(r("comp_address1")) Then
              If r("comp_address1") <> "" Then
                address_string = r("comp_address1") & " "
              End If
            End If
            If Not IsDBNull(r("comp_city")) Then
              If r("comp_city") <> "" Then
                address_string = address_string & r("comp_city") & " "
              End If
            End If
            If Not IsDBNull(r("comp_state")) Then
              If r("comp_state") <> "" Then
                address_string = address_string & r("comp_state") & " "
              End If
            End If
            If Not IsDBNull(r("comp_country")) Then
              If r("comp_country") <> "" Then
                address_string = address_string & r("comp_country")
              End If
            End If
            company_name.Items.Add(New ListItem(CStr(r("comp_name") & " " & address_string & "(" & r("source") & " record)"), r("comp_id") & "|" & r("source")))
          Next
        Else ' 0 rows
        End If
      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = aclsData_Temp.class_error
          LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - company_search_button_Click() - " & error_string)
        End If
        displayError()
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Transaction_Tab.ascx.vb - company_search_button_Click() - " & ex.Message
      LogError(error_string)
    End Try
  End Sub

  Private Sub save_row_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles save_row.Click
    Try
      If Page.IsValid Then
        Dim company As ListBox = row_company
        Dim company_array As Array = Split(company.SelectedValue, "|")
        'company_array(0) is company ID
        'company_array(1) is source

        Dim client_trans_id As Integer = IIf(IsNumeric(Trim(Request("cli_trans"))), CInt(Trim(Request("cli_trans"))), 0)
        Dim contact As ListBox = row_contact
        Dim contact_array As Array = Split(contact.SelectedValue, "|")
        'contact_array(0) is contact ID
        'contact_array(1) is source

        Dim company_id As Integer = 0
        Try
          company_id = IIf(IsNumeric(company_array(0)), company_array(0), 0)
        Catch
          company_id = 0
        End Try
        Dim contact_id As Integer = 0
        Try
          contact_id = IIf(IsNumeric(contact_array(0)), contact_array(0), 0)
        Catch
          contact_id = 0
        End Try


        Dim contact_type As DropDownList = row_contact_type

        'Response.Write(contact_type.SelectedValue & "!!!!!!!!!!!")
        'First we have to determine if the company is a client record or jetnet record.
        If company_array(1) = "JETNET" Then
          Dim errored As String = ""
          'We have to go through a perform a bazillion checks here. 
          '3.) Take that jetnet company ID and poll it against our client database. 
          aTempTable = aclsData_Temp.GetCompanyInfo_JETNET_ID(company_id, errored)

          If Not IsNothing(aTempTable) Then
            If aTempTable.Rows.Count > 0 Then

              'Now check and see if the client company ID exists in the transaction table for this record
              aTempTable2 = aclsData_Temp.Get_Client_Transactions_Company(aTempTable.Rows(0).Item("comp_id"), client_trans_id)

              Dim datatable As New DataTable
              datatable = aTempTable2.Clone
              'Filter that transaction company table based only on the ones with that transaction ID. 
              'This really needs to match the CLIENT TRANSACTION ID NOT JETNET. MUST CHANGE
              Dim afileterd As DataRow() = aTempTable2.Select("clitcomp_trans_id = '" & journ_id.Text & "' ", "clitcomp_id")

              For Each z As DataRow In afileterd
                datatable.ImportRow(z)
              Next

              If datatable.Rows.Count > 0 Then
                'this is if the client company exists in the transaction record
                'Response.Write("<br />Don't make a copy of this!!!!!" & datatable.Rows(0).Item("clitcomp_id") & "<br />")
              Else
                aTempTable2 = aclsData_Temp.Get_Client_Transactions_Company(aTempTable.Rows(0).Item("comp_id"), client_trans_id)
                '---------------------------CLIENT TRANSACTION COMPANY-----------------------------------------
                If Not IsNothing(aTempTable2) Then
                  If aTempTable2.Rows.Count = 0 Then
                    'This means that a client copy of this already exists in the database.
                    ' Response.Write("<br />Not found!!! Make a copy!! " & aTempTable.Rows(0).Item("comp_id") & "<br />")
                    Dim Client_Company_ID As Integer = aTempTable.Rows(0).Item("comp_id")
                    '---------4.) Store all of the info for that transaction company in the client transaction company database.
                    Fill_Transaction_Company_FromJETNET(Client_Company_ID, journ_id.Text, company_array(0), client_trans_id, "JETNET")
                    '---------5.) Store all of the related contacts to that company in the transaction related database.
                    Fill_Transaction_Contacts_FromJETNET(Client_Company_ID, journ_id.Text, company_array(0), client_trans_id, "JETNET")
                  Else
                    Dim Client_Company_ID As Integer = aTempTable2.Rows(0).Item("clitcomp_id")
                  End If

                Else
                  If aclsData_Temp.class_error <> "" Then
                    error_string = aclsData_Temp.class_error
                    LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - save_row_click() - " & error_string)
                  End If
                  displayError()
                End If
              End If
            Else
              ' Response.Write("<br />Not here!" & "<br />")

              '-----If the client copy doesn't exist
              '---------4.) Store all of the information for the transaction company into the client COMPANY database.
              '---------4.) b. Store all of the related company phone numbers to that transaction in the client phone number database.
              Dim Client_Company_ID As Integer = 0
              Client_Company_ID = Fill_Client_Company_FromJETNET(company_id, journ_id.Text)
              'Client_Company_ID = 11838 'hard coded for now for testing and coding purposes
              '---------5.) Store all of the information for that transaction company into the client company TRANSACTION database.
              '---------5.) b. Store all of the related company phone numbers to that transaction in the phone number transaction database.
              Fill_Transaction_Company_FromJETNET(Client_Company_ID, journ_id.Text, company_id, client_trans_id, "JETNET")
              '---------6.) Store all of the information for the related contacts to that company into the client CONTACT database. 
              '---------6.) b. Also add the contact phone numbers into the client database. 
              Fill_Client_Contacts_FromJETNET(company_id, Client_Company_ID, journ_id.Text)
              '---------7.) Store all of the information for the related contacts to that company into the client contact TRANSACTION database.
              Fill_Transaction_Contacts_FromJETNET(Client_Company_ID, journ_id.Text, company_id, client_trans_id, "JETNET")
            End If
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = aclsData_Temp.class_error
              LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - save_row_click() - " & error_string)
            End If
            displayError()
          End If


        Else
          aTempTable2 = aclsData_Temp.Get_Client_Transactions_Company(company_id, client_trans_id)
          '---------------------------CLIENT TRANSACTION COMPANY-----------------------------------------
          If Not IsNothing(aTempTable2) Then
            If aTempTable2.Rows.Count = 0 Then
              ' Response.Write("<br />Not here!" & "<br />")
              'Need to check and see - check and see
              '-----If the client copy doesn't exist
              '---------4.) Store all of the information for the transaction company into the client COMPANY database.
              '---------4.) b. Store all of the related company phone numbers to that transaction in the client phone number database.
              Dim Client_Company_ID As Integer = 0
              Client_Company_ID = company_id
              'Client_Company_ID = 11838 'hard coded for now for testing and coding purposes
              '---------5.) Store all of the information for that transaction company into the client company TRANSACTION database.
              '---------5.) b. Store all of the related company phone numbers to that transaction in the phone number transaction database.
              Fill_Transaction_Company_FromJETNET(Client_Company_ID, journ_id.Text, company_id, client_trans_id, "CLIENT")
              Fill_Transaction_Contacts_FromJETNET(Client_Company_ID, journ_id.Text, company_id, client_trans_id, "CLIENT")
            Else
              'check for jetnet company id.

              Dim atemptable4 As New DataTable
              atemptable4 = aclsData_Temp.GetCompanyInfo_ID(company_id, "CLIENT", 0)


              Dim atemptable3 As New DataTable
              If contact_id <> 0 Then
                atemptable3 = aclsData_Temp.Get_Client_Transactions_Contact_ContactID(contact_id, client_trans_id)
                If Not IsNothing(atemptable3) Then
                  If atemptable3.Rows.Count > 0 Then
                  Else
                    Fill_Transaction_Contacts_FromJETNET(company_id, journ_id.Text, company_id, client_trans_id, "CLIENT")
                  End If

                End If
              End If
            End If
          Else
            If aclsData_Temp.class_error <> "" Then
              error_string = aclsData_Temp.class_error
              LogError("Aircraft_Edit_Transaction_Tab.ascx.vb - save_row_click() - " & error_string)
            End If
            displayError()
          End If
        End If

        Fill_SINGLE_AC_Transaction_Reference(row_contact_type.SelectedValue, company_id, contact_id, 0, False, True, client_trans_id)

        new_row.Visible = False
        insert_row.Visible = True
        client_bind_data()
        row_company.Items.Clear()
        row_contact.Items.Clear()
        contact_drop.Visible = False
        comp_search_row.Visible = False
        row_company.Visible = False
        company_search_panel_row.Visible = True
      End If
    Catch ex As Exception
      error_string = "Aircraft_Edit_Transaction_Tab.ascx.vb - save_row_click() - " & ex.Message
      LogError(error_string)
    End Try


  End Sub
#End Region

  '-----------------------------------------------------Public Functions--------------------------------------------------------
  Public Function displayError()
    '------------------------------Function that Creates a Javascript Error if the data manager class errors-----------
    displayError = ""
    If aclsData_Temp.class_error <> "" Then
      '  System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Error", "alert('" & Replace(aclsData_Temp.class_error, "'", " \'") & "');", True)
    End If
    aclsData_Temp.class_error = ""
  End Function

  Public Sub LogError(ByVal ex As String)
    aclsData_Temp.LogError(Application.Item("crmClientSiteData").crmClientHostName, ex, DateTime.Now.ToString())
  End Sub

  Protected Sub cancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cancel.Click
    new_row.Visible = False
    insert_row.Visible = True
    buttons.Visible = True
  End Sub

  Private Sub remove_Click() Handles removeButton.Click
    RemoveTransaction()
  End Sub


  Private Sub typed_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles typed.SelectedIndexChanged
    Select Case UCase(typed.SelectedValue)
      Case "FRACTIONAL SALE"
        clitrans_subcat_code_part1.Text = "FS"
      Case "FULL SALE"
        clitrans_subcat_code_part1.Text = "WS"
      Case "SHARE SALE"
        clitrans_subcat_code_part1.Text = "SS"
      Case "DELIVERY POSITION SALE"
        clitrans_subcat_code_part1.Text = "DP"
      Case "SEIZURE"
        clitrans_subcat_code_part1.Text = "SZ"
      Case "LEASE"
        clitrans_subcat_code_part1.Text = "LS"
      Case "FORECLOSURE"
        clitrans_subcat_code_part1.Text = "FC"
    End Select

  End Sub
End Class