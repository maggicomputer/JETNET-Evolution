Imports System.IO
Partial Public Class Submenu_Edit_Template
  Inherits System.Web.UI.UserControl
  Dim aclsData_Temp As New clsData_Manager_SQL
  Dim aTempTable, aTempTable2 As New DataTable 'Data Tables used
  Public Event close_me()
  Dim error_string As String = ""
#Region "Page Events"

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    If Me.Visible Then
      Try
        aclsData_Temp.client_DB = HttpContext.Current.Session.Item("jetnetServerNotesDatabase") 'CApplication.Item("crmClientDatabase")
        aclsData_Temp.JETNET_DB = Session.Item("jetnetClientDatabase") 'CApplication.Item("crmJetnetDatabase")

        ' setup the connection info

        aclsData_Temp.class_error = ""
        If Not Page.IsPostBack Then
          If Trim(Request("type")) = "edit" Then
            Start_Bad_Data_Fix() 'check for bad sorts
            add_folder_panl.Visible = False
            bind_data()
          ElseIf Trim(Request("type")) = "add_active" Then
            Dim Folder_ID As Long = 0

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'I am not convinced that I want to keep this on this page.
            'I would much rather put it on the folder maintenance page, so that way everything would be kept on one page
            'with one upkeep.
            'However in an attempt not to mess up the current working version
            'Yet still come up with a workable prototype
            'I've gone ahead and moved it here.
            Select Case Trim(Request("action"))
              Case "aifolder"
                cfolder_type_of_folder.Text = "3"
                foldertypeStringLabel.Text = "Aircraft"
                folder_type_label.Text = "Aircraft"
                cfolder_method.Text = "A"
              Case "cyfolder"
                cfolder_type_of_folder.Text = "1"
                foldertypeStringLabel.Text = "Company"
                folder_type_label.Text = "Company"
                cfolder_method.Text = "A"
              Case "ctfolder"
                cfolder_type_of_folder.Text = "2"
                foldertypeStringLabel.Text = "Contact"
                folder_type_label.Text = "Contact"
                cfolder_method.Text = "A"
              Case "trfolder"
                cfolder_type_of_folder.Text = "8"
                foldertypeStringLabel.Text = "Transactions"
                folder_type_label.Text = "Transactions"
                cfolder_method.Text = "A"
            End Select

            Dim QueryRebuild As String = ""

            add_folder_panl.Visible = False
            add_list_to_folder.Visible = False
            edit_folder.Visible = False
            add_active_folder.Visible = True
            folder_submit_button.Focus()

            For Each name As String In Request.Form.AllKeys
              If name <> "FOLDER_ID" Then
                Dim value As String = Request.Form(name)
                QueryRebuild += name & "=" & value & "!~!"
              Else
                Folder_ID = Request.Form(name)
                cfolder_id.Text = Folder_ID

                If Folder_ID > 0 Then
                  folder_submit_button.Text = "Update Folder"
                  Dim FolderTable As New DataTable

                  FolderTable = aclsData_Temp.Get_Client_Folders_ByID(CInt(Session.Item("localUser").crmLocalUserID), Folder_ID)
                  If FolderTable.Rows.Count > 0 Then
                    If FolderTable.Rows(0).Item("cfolder_cliuser_id") = CInt(Session.Item("localUser").crmLocalUserID) Then
                      cfolder_description.Text = IIf(Not IsDBNull(FolderTable.Rows(0).Item("cfolder_description")), FolderTable.Rows(0).Item("cfolder_description").ToString, "")
                      cfolder_name.Text = IIf(Not IsDBNull(FolderTable.Rows(0).Item("cfolder_name")), FolderTable.Rows(0).Item("cfolder_name").ToString, "")
                      cfolder_sort1.Text = IIf(Not IsDBNull(FolderTable.Rows(0).Item("cfolder_sort1")), FolderTable.Rows(0).Item("cfolder_sort1").ToString, "")
                      cfolder_sort2.Text = IIf(Not IsDBNull(FolderTable.Rows(0).Item("cfolder_sort2")), FolderTable.Rows(0).Item("cfolder_sort2").ToString, "")
                      cfolder_share.Checked = IIf((FolderTable.Rows(0).Item("cfolder_share").ToString = "Y"), True, False)
                      cfolder_hide.Checked = IIf((FolderTable.Rows(0).Item("cfolder_hide_flag").ToString = "Y"), True, False)
                    Else
                      add_folder_table.Visible = False
                      add_active_shared.Visible = True
                    End If
                  End If
                End If
              End If
            Next
            QueryRebuild = QueryRebuild.TrimEnd("!~!")
            cfolder_data.Text = QueryRebuild


            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
          ElseIf Trim(Request("type")) = "add_folderAuto" Then
            add_folder_panl.Visible = True
            add_folder_cbo.Items.Add(New ListItem("Aircraft Folder", 3))
            add_list_to_folder.Visible = False
            edit_folder.Visible = False

            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType(), "FitPage", "window.resizeTo(460, 400);self.focus();", True)
          ElseIf Trim(Request("type")) = "add_list" Then
            add_folder_panl.Visible = False
            add_list_to_folder.Visible = True
            edit_folder.Visible = False
            aTempTable2 = aclsData_Temp.Get_Client_Folders(CInt(Session.Item("localUser").crmLocalUserID), "Y", Session.Item("Listing"))
            If Not IsNothing(aTempTable2) Then
              If aTempTable2.Rows.Count > 0 Then
                For Each m As DataRow In aTempTable2.Rows
                  add_list_folder_cbo.Items.Add(New ListItem(m("cfolder_name"), m("cfolder_id")))
                Next
              End If
            End If
            add_list_folder_cbo.Items.Add(New ListItem("Please Select a Folder", ""))

            If IsNumeric(Trim(Request("auto"))) Then
              add_list_folder_cbo.SelectedValue = Trim(Request("auto"))
              add_to_folder_btn_Click(Nothing, Nothing)
            End If
          End If
        End If
      Catch ex As Exception
        error_string = "Submenu_Edit_Template.ascx.vb - Page_Load() - " & ex.Message
        clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
      End Try
    End If
  End Sub
#End Region
#Region "Add Submenu Folder Function"
  Protected Sub add_sub_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles add_sub.Click
    Try
      'Add a folder
      Dim id As Integer = CInt(add_folder_cbo.SelectedValue)
      Dim name As String = folder_name.Text
      Dim newFolderID As Long = 0
      newFolderID = aclsData_Temp.Insert_Into_Client_Folder(id, name, CInt(Session.Item("localUser").crmLocalUserID), "N", "N", 2, 1)

      If newFolderID > 0 Then
        add_list_folder_cbo.Items.Add(New ListItem(name, newFolderID.ToString))
        add_list_folder_cbo.SelectedValue = newFolderID
        Session.Item("FromTypeOfListing") = id 'added to retain listing ID that we came from on a search if the type is changed
        Session.Item("Listing") = id
        Session.Item("isSubnode") = True
        Session.Item("SubnodeName") = name
        Session.Item("Subnode") = newFolderID
        Session.Item("SubnodeMethod") = ""

        Session("Results") = Nothing
        Session("search_company") = Nothing
        Session("search_contact") = Nothing
        Session("search_aircraft") = Nothing
        Session("search_transaction") = Nothing

        add_to_folder_btn_Click(Nothing, Nothing)

      Else
        If aclsData_Temp.class_error <> "" Then
          error_string = "Submenu_Edit_Template.ascx.vb - add_sub_Click() - " & aclsData_Temp.class_error
          clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        End If
      End If
    Catch ex As Exception
      error_string = "Submenu_Edit_Template.ascx.vb - add_sub_Click() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
#End Region
#Region "DataGrid Functions"
  Sub MyDataGrid_Edit(ByVal Sender As Object, ByVal E As DataGridCommandEventArgs)
    Try
      datagrid_details.EditItemIndex = CInt(E.Item.ItemIndex)
      bind_data()
      datagrid_details.DataBind()
    Catch ex As Exception
      error_string = "Submenu_Edit_Template.ascx.vb - MyDataGrid_Edit() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Public Sub MyDataGrid_Delete(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
    Try
      Dim id As TextBox = e.Item.FindControl("id_hidden")
      Dim typed As TextBox = e.Item.FindControl("type_hidden")
      Dim user As TextBox = e.Item.FindControl("user_hidden")
      Dim name As TextBox = e.Item.FindControl("name_hidden")

      If aclsData_Temp.Remove_Client_Folders(CInt(id.Text), CInt(typed.Text), name.Text, CInt(user.Text)) = 1 Then
        bind_data()
        feedback.Text = "Your folder has been removed."
      End If

    Catch ex As Exception
      error_string = "Submenu_Edit_Template.ascx.vb -  MyDataGrid_Delete() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
    'Response.Write("name: " & name.Text & " id: " & id.Text & " type: " & typed.Text & " user: " & user.Text)
  End Sub
  Public Sub MyDataGrid_Cancel(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
    Try
      bind_data()
      datagrid_details.EditItemIndex = -1
      feedback.Text = ""
    Catch ex As Exception
      error_string = "Submenu_Edit_Template.ascx.vb - MyDataGrid_Cancel() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Public Sub MyDataGrid_Update(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
    Try
      Dim id As TextBox = e.Item.FindControl("id_hidden")
      Dim typed As TextBox = e.Item.FindControl("type_hidden")
      Dim user As TextBox = e.Item.FindControl("user_hidden")
      Dim name As TextBox = e.Item.FindControl("name")
      Dim share As CheckBox = e.Item.FindControl("cfolder_share")
      Dim cfolder_sort2 As TextBox = e.Item.FindControl("cfolder_sort2")

      Dim cfolder_method As New TextBox
      If Not IsNothing(e.Item.FindControl("method")) Then
        cfolder_method = e.Item.FindControl("method")
      End If

      Dim cfolder_data As New TextBox
      If Not IsNothing(e.Item.FindControl("method")) Then
        cfolder_data = e.Item.FindControl("data")
      End If

      Dim cfolder_description As New TextBox
      If Not IsNothing(e.Item.FindControl("description")) Then
        cfolder_description = e.Item.FindControl("description")
      End If

      Dim cfolder_share As String = ""
      Dim cfolder_sort1 As Integer = 0

      If share.Checked = True Then
        cfolder_share = "Y"
        cfolder_sort1 = 1
      Else
        cfolder_share = "N"
        cfolder_sort1 = 2
      End If
      Dim cfolder_hide_flag As String = "N"

      Dim returned As Integer = aclsData_Temp.Update_Client_Folders(CInt(typed.Text), name.Text, CInt(user.Text), cfolder_share, cfolder_hide_flag, cfolder_sort1, cfolder_sort2.Text, cfolder_data.Text, cfolder_method.Text, cfolder_description.Text, CInt(id.Text))

      If returned = 1 Then
        bind_data()
        datagrid_details.EditItemIndex = -1
        feedback.Text = "Your folder has been updated."
      End If
    Catch ex As Exception
      error_string = "Submenu_Edit_Template.ascx.vb - MyDataGrid_Update() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
    'Response.Write("name: " & name.Text & "hidden name" & hidden_name.Text & " id: " & id.Text & " type: " & typed.Text & " user: " & user.Text)
  End Sub

  Sub Start_Bad_Data_Fix()
    Try
      Dim fixed_sort As Integer = 0
      'need to start of fixing sort field 1
      aTempTable = aclsData_Temp.Client_Folder_BadSort()
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then
          For Each r As DataRow In aTempTable.Rows
            If r("cfolder_share") = "Y" Then
              fixed_sort = 1
            Else
              fixed_sort = 2
            End If
            UpdateSortOrderField(r("cfolder_id"), fixed_sort, 1)
            '  Response.Write(r("cfolder_id") & "!! bad sort!! fixed = " & fixed_sort & "<br />")
          Next
        End If
      End If

      Dim typeOfFolder As Integer = 0
      If Not IsNothing(Request.Item("action")) Then
        If Not String.IsNullOrEmpty(Request.Item("action").ToString) Then
          Select Case Request.Item("action").ToString
            Case "cyfolder"
              typeOfFolder = 1
            Case "ctfolder"
              typeOfFolder = 2
            Case "aifolder"
              typeOfFolder = 3
          End Select
        End If
      End If
      'This has to run on first page load to check of the data is bad.
      'First we need to check the sort field 2
      aTempTable = aclsData_Temp.Get_Client_Folders_Shared("Y", typeOfFolder, True)
      Checking_For_Bad_Sort(aTempTable)
      'Response.Write("<hr />")
      aTempTable = aclsData_Temp.Get_Client_Folders_NonShared(CInt(Session.Item("localUser").crmLocalUserID), "N", typeOfFolder, True)
      Checking_For_Bad_Sort(aTempTable)
    Catch ex As Exception
      error_string = "Submenu_Edit_Template.ascx.vb - Fix_Data_If_Bad() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try

  End Sub
  Sub Checking_For_Bad_Sort(ByVal atemptable As DataTable)
    Dim old_sort As Integer = -1
    Dim bad_data As Boolean = False
    If Not IsNothing(atemptable) Then
      If atemptable.Rows.Count > 0 Then
        For Each r As DataRow In atemptable.Rows
          If old_sort = r("cfolder_sort2") Then
            bad_data = True
          End If
          old_sort = r("cfolder_sort2")
        Next
      End If
    Else
      If aclsData_Temp.class_error <> "" Then
        error_string = "Submenu_Edit_Template.ascx.vb - bind_data() - " & aclsData_Temp.class_error
        clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
      End If
    End If
    If bad_data = True Then
      Finally_Fixing_Data_Sort(atemptable)
    Else 'table is okay
      ' Response.Write("table is okay")
    End If
  End Sub

  Sub Finally_Fixing_Data_Sort(ByVal atemptable)
    ' Response.Write("fix table!!<br />")
    Dim x As Integer = 0
    If Not IsNothing(atemptable) Then
      If atemptable.Rows.Count > 0 Then
        For Each r As DataRow In atemptable.Rows
          UpdateSortOrderField(r("cfolder_id"), x, 2)
          x = x + 1
        Next
      End If
    End If
  End Sub
  Sub bind_data()
    Try
      Dim typeOfFolder As Integer = 0
      If Not IsNothing(Request.Item("action")) Then
        If Not String.IsNullOrEmpty(Request.Item("action").ToString) Then
          Select Case Request.Item("action").ToString
            Case "cyfolder"
              typeOfFolder = 1
              label_header.Text = "<h4 align='right'>Edit Company Subfolders</h4>"
              add_new.Text = "New Company Folder"
            Case "ctfolder"
              typeOfFolder = 2
              label_header.Text = "<h4 align='right'>Edit Contact Subfolders</h4>"
              add_new.Text = "New Contact Folder"
            Case "aifolder"
              typeOfFolder = 3
              label_header.Text = "<h4 align='right'>Edit Aircraft Subfolders</h4>"
              add_new.Text = "New Aircraft Folder"
            Case "trfolder"
              typeOfFolder = 8
              label_header.Text = "<h4 align='right'>Edit History Subfolders</h4>"
              add_new.Text = "New Aircraft Folder"
          End Select
        End If
      End If

      aTempTable = aclsData_Temp.Get_Client_Folders_Shared("Y", typeOfFolder, True)
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then

          Session("remember_share") = aTempTable
          ReorderList2.DataSource = aTempTable
          If Session.Item("localUser").crmUserType = eUserTypes.ADMINISTRATOR Then
            ReorderList2.AllowReorder = True
          Else
            ReorderList2.AllowReorder = False
          End If
          ReorderList2.DataBind()

          feedback.Text = ""
        Else
          ReorderList2.DataSource = New DataTable
          ReorderList2.DataBind()
        End If
      Else
        ReorderList2.DataSource = New DataTable
        ReorderList2.DataBind()
        If aclsData_Temp.class_error <> "" Then
          error_string = "Submenu_Edit_Template.ascx.vb - bind_data() - " & aclsData_Temp.class_error
          clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        End If
      End If


      aTempTable = aclsData_Temp.Get_Client_Folders_NonShared(CInt(Session.Item("localUser").crmLocalUserID), "N", typeOfFolder, True)
      If Not IsNothing(aTempTable) Then
        If aTempTable.Rows.Count > 0 Then

          Session("remember") = aTempTable
          ReorderList1.DataSource = aTempTable
          ReorderList1.DataBind()

          feedback.Text = ""

          'End If
        Else
          ReorderList1.DataSource = New DataTable
          ReorderList1.DataBind()
        End If
      Else
        ReorderList1.DataSource = New DataTable
        ReorderList1.DataBind()
        If aclsData_Temp.class_error <> "" Then
          error_string = "Submenu_Edit_Template.ascx.vb - bind_data() - " & aclsData_Temp.class_error
          clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
        End If
      End If
    Catch ex As Exception
      error_string = "Submenu_Edit_Template.ascx.vb - bind_data() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Function WhatFolder(ByVal x As Object)
    WhatFolder = ""
    Try
      If Not IsDBNull(x) Then
        aTempTable = aclsData_Temp.Get_Client_Folder_Type
        If Not IsNothing(aTempTable) Then
          If aTempTable.Rows.Count > 0 Then
            For Each q As DataRow In aTempTable.Rows
              If x.ToString = q("cftype_id") Then
                WhatFolder = q("cfttpe_name")
              End If
            Next
          End If
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = "Submenu_Edit_Template.ascx.vb - WhatFolder() - " & aclsData_Temp.class_error
            clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
          End If
        End If
      End If
    Catch ex As Exception
      error_string = "Submenu_Edit_Template.ascx.vb - WhatFolder() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Function
#End Region
  Public Function Display_Popup(ByVal tcount As Object) As String
    Display_Popup = ""
    If Not IsDBNull(tcount) Then
      Display_Popup = "if(!confirm('The folder that you wish to delete contains " & tcount & " record(s). Do you still want to delete?'))return false;"
    End If
  End Function


  Public Function ToggleNewFolderIcon(ByVal share As String, ByVal cfolder_method As Object, ByVal cfolder_hide_flag As Object, ByVal cfolder_share As Object) As String
    Dim strClass As String = ""
    Dim BaseCss As String = "dragHandleShareHide"

    If share = "Y" Then
      BaseCss = "dragHandle"
    End If
    strClass = "<div class=""" & BaseCss & """ style=""background-image:url('../" & DisplayFunctions.ReturnFolderImage(cfolder_method.ToString, cfolder_hide_flag.ToString, cfolder_share.ToString) & "') !important;"" /></div>"

    Return strClass
  End Function
  Private Sub add_to_folder_btn_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles add_to_folder_btn.Click
    Try
      Dim cookie_name As String = ""
      Dim save_name As String = ""
      Select Case Session.Item("Listing")
        Case 1
          save_name = "Company(s)"
          cookie_name = "companies_marked"
        Case 2
          save_name = "Contact(s)"
          cookie_name = "contacts_marked"
        Case 3
          save_name = "Aircraft"
          cookie_name = "aircraft_marked"
      End Select
      Dim _acmarked As HttpCookie = Request.Cookies(cookie_name)
      Dim client_ids As String = ""
      Dim jetnet_ids As String = ""
      Dim jetnet_ac_id As Integer = 0
      Dim client_ac_id As Integer = 0
      Dim jetnet_comp_id As Integer = 0
      Dim client_comp_id As Integer = 0
      Dim jetnet_contact_id As Integer = 0
      Dim client_contact_id As Integer = 0

      If _acmarked IsNot Nothing Then
        Dim _acmarked_val As String = Request.Cookies(cookie_name).Value
        Dim arrayed As Array = Split(_acmarked_val, "|")
        Dim my_aircraft_folder As Integer = add_list_folder_cbo.SelectedValue
        For x = 0 To UBound(arrayed)
          If arrayed(x) <> "" Then
            Dim list_ids As Array = Split(arrayed(x), "#")
            Select Case list_ids(1)
              Case "CLIENT"
                Select Case Session.Item("Listing")
                  Case 1
                    client_comp_id = list_ids(0)
                    jetnet_comp_id = 0
                  Case 2
                    client_contact_id = list_ids(0)
                    jetnet_contact_id = 0
                  Case 3
                    client_ac_id = list_ids(0)
                    jetnet_ac_id = 0
                End Select
              Case "JETNET"
                Select Case Session.Item("Listing")
                  Case 1
                    jetnet_comp_id = list_ids(0)
                    client_comp_id = 0
                  Case 2
                    jetnet_contact_id = list_ids(0)
                    client_contact_id = 0
                  Case 3
                    jetnet_ac_id = list_ids(0)
                    client_ac_id = 0
                End Select
            End Select
            aTempTable = aclsData_Temp.Get_ClientFolderIndex_Search(my_aircraft_folder, jetnet_ac_id, jetnet_comp_id, jetnet_contact_id, client_ac_id, client_comp_id, client_contact_id)
            If Not IsNothing(aTempTable) Then
              If aTempTable.Rows.Count = 0 Then
                If aclsData_Temp.Insert_Into_Client_Folder_Index(my_aircraft_folder, jetnet_ac_id, jetnet_comp_id, jetnet_contact_id, client_ac_id, client_comp_id, client_contact_id, 0, "") = 1 Then
                Else
                  If aclsData_Temp.class_error <> "" Then
                    error_string = "main_site.Master.vb - mark_all_selected_items() - " & aclsData_Temp.class_error
                    clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
                  End If
                End If
              End If
            End If
          End If
        Next

        Response.Cookies(cookie_name).Value = ""
      End If
      Response.Cookies("aircraft_marked").Value = ""
      If IsNumeric(Trim(Request("auto"))) Then
        If Session("Listing") = 1 Then
          Response.Redirect("listing.aspx?redo_search=true", False)
        ElseIf Session("Listing") = 2 Then
          Response.Redirect("listing_contact.aspx?redo_search=true", False)
        Else
          Response.Redirect("listing_air.aspx?redo_search=true", False)
        End If
      Else
        If Trim(Request("type")) = "add_folderAuto" Then
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location.href = 'listing_air.aspx';", True)
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alert", "alert('Your " & save_name & " has been saved');", True)
        Else

          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location.reload();", True)
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
          System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "alert", "alert('Your " & save_name & " has been saved');", True)
        End If
      End If
    Catch ex As Exception
      error_string = "Submenu_Edit_Template.ascx.vb - add_to_folder_btn_Click() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub

  Sub Edit_Shared(ByVal Sender As Object, ByVal e As AjaxControlToolkit.ReorderListCommandEventArgs)
    Try
      Sender.ShowInsertItem = False
      Sender.EditItemIndex = CInt(e.Item.ItemIndex)
      bind_data()
    Catch ex As Exception
      error_string = "submenu_Edit_Template.ascx.vb - MyDataGrid_Edit_Shared() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Public Sub Cancel_Shared(ByVal sender As Object, ByVal e As AjaxControlToolkit.ReorderListCommandEventArgs)
    Try
      'bind_data()
      ReorderList1.ShowInsertItem = False
      ReorderList1.EditItemIndex = -1
      ReorderList2.ShowInsertItem = False
      ReorderList2.EditItemIndex = -1
      bind_data()
    Catch ex As Exception
      error_string = "submenu_Edit_Template.ascx.vb - MyDataGrid_Edit() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Public Sub Delete_Shared(ByVal sender As Object, ByVal e As AjaxControlToolkit.ReorderListCommandEventArgs)
    Try
      Dim id As TextBox = e.Item.FindControl("id")

      If aclsData_Temp.Remove_Client_Folders_ByPrimaryKeyOnly(CInt(id.Text)) = 1 Then
        bind_data()
        feedback.Text = "Your folder has been removed."
      End If
    Catch ex As Exception
      error_string = "submenu_Edit_Template.ascx.vb - Delete() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub

  Sub Save_Row_Shared(ByVal sender As Object, ByVal e As AjaxControlToolkit.ReorderListCommandEventArgs)
    If e.CommandName = "Save" Then

      Dim id As TextBox = e.Item.FindControl("id")
      Dim cfolder_name As TextBox = e.Item.FindControl("new_name")
      Dim cfolder_share As CheckBox = e.Item.FindControl("new_share")
      Dim cfolder_hide_flag As CheckBox = e.Item.FindControl("new_hide")

      Dim returned As Integer = aclsData_Temp.Update_Selective_Client_Folders(cfolder_name.Text, IIf(cfolder_share.Checked = True, "Y", "N"), IIf(cfolder_hide_flag.Checked = True, "Y", "N"), CInt(id.Text)) ' aclsData_Temp.Update_Client_Folders(CInt(cfolder_cftype_id.Text), cfolder_name.Text, CInt(cfolder_cliuser_id.Text), IIf(cfolder_share.Checked = True, "Y", "N"), IIf(cfolder_hide_flag.Checked = True, "Y", "N"), cfolder_sort1, cfolder_sort2.Text, cfolder_data.Text, cfolder_method.Text, cfolder_description.Text, CInt(id.Text))
      ReorderList1.EditItemIndex = -1
      ReorderList2.EditItemIndex = -1
      bind_data()
    ElseIf e.CommandName = "Insert" Then
      Try
        Dim typeOfFolder As Integer = 0
        'Add a folder
        Select Case Request.Item("action").ToString
          Case "cyfolder"
            typeOfFolder = 1
          Case "ctfolder"
            typeOfFolder = 2
          Case "aifolder"
            typeOfFolder = 3
        End Select


        Dim cfolder_share As CheckBox = e.Item.FindControl("new_share")
        Dim cfolder_hide_flag As CheckBox = e.Item.FindControl("new_hide")
        Dim cfolder_name As TextBox = e.Item.FindControl("new_name")

        If aclsData_Temp.Insert_Into_Client_Folder(typeOfFolder, Left(Replace(cfolder_name.Text, "'", "\'"), 100), CInt(Session.Item("localUser").crmLocalUserID), IIf(cfolder_share.Checked = True, "Y", "N"), IIf(cfolder_hide_flag.Checked = True, "Y", "N"), 2, 1) <> 0 Then
          ReorderList1.ShowInsertItem = False
          ReorderList2.ShowInsertItem = False
          bind_data()
        Else
          If aclsData_Temp.class_error <> "" Then
            error_string = "Submenu_Edit_Template.ascx.vb - save row() - " & aclsData_Temp.class_error
            clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
          End If
        End If
      Catch ex As Exception
        error_string = "Submenu_Edit_Template.ascx.vb - save row() - " & ex.Message
        clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
      End Try
    End If

  End Sub
  Private Sub add_new_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles add_new.Click
    ReorderList1.ShowInsertItem = True
    bind_data()
  End Sub


  Protected Sub reorderSort_ItemReorder(ByVal sender As Object, ByVal e As AjaxControlToolkit.ReorderListItemReorderEventArgs) Handles ReorderList1.ItemReorder
    Dim dataTable As DataTable = DirectCast(Session("remember"), DataTable)
    reorder_me(dataTable, e)
  End Sub


  Protected Sub reorderSort_ItemReorderShare(ByVal sender As Object, ByVal e As AjaxControlToolkit.ReorderListItemReorderEventArgs) Handles ReorderList2.ItemReorder
    Try
      Dim dataTable As DataTable = DirectCast(Session("remember_share"), DataTable)
      reorder_me(dataTable, e)
    Catch ex As Exception
      error_string = "submenu_Edit_Template.ascx.vb - shared reorder() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub

  Sub reorder_me(ByVal datatable As DataTable, ByVal e As AjaxControlToolkit.ReorderListItemReorderEventArgs)
    Try
      Dim oldIndex As Integer = e.OldIndex
      Dim newIndex As Integer = e.NewIndex
      Dim newPriorityOrder As Integer = CInt(datatable.Rows(newIndex)("cfolder_sort2"))

      If newIndex > oldIndex Then
        'item moved down
        For i As Integer = oldIndex + 1 To newIndex
          Dim propertyId As Integer = CInt(datatable.Rows(i)("cfolder_id"))
          If propertyId <> -1 Then
            datatable.Rows(i)("cfolder_sort2") = CInt(datatable.Rows(i)("cfolder_sort2")) - 1
            UpdateSortOrderField(propertyId, CInt(datatable.Rows(i)("cfolder_sort2")), 2)
          End If
        Next
      Else
        'item moved up
        For i As Integer = oldIndex - 1 To newIndex Step -1
          Dim propertyId As Integer = datatable.Rows(i)("cfolder_id")
          If propertyId <> -1 Then
            datatable.Rows(i)("cfolder_sort2") = CInt(datatable.Rows(i)("cfolder_sort2")) + 1
            UpdateSortOrderField(propertyId, CInt(datatable.Rows(i)("cfolder_sort2")), 2)
          End If
        Next
      End If

      'Finally, update the priority for origional row            
      Dim id As Integer = CInt(datatable.Rows(oldIndex)("cfolder_id"))
      If id <> -1 Then
        UpdateSortOrderField(id, newPriorityOrder, 2)
      End If
      Session("remember_share") = datatable
      bind_data()
    Catch ex As Exception
      error_string = "submenu_Edit_Template.ascx.vb - reorder_me() - " & ex.Message
      clsGeneral.clsGeneral.LogError(error_string, aclsData_Temp)
    End Try
  End Sub
  Private Sub UpdateSortOrderField(ByVal pId As Integer, ByVal newPriorityOrder As Integer, ByVal sort_field As Integer)
    'Response.Write(pId & " -- New ID, new order " & newPriorityOrder & "<br />")
    Dim returned As Integer
    If sort_field = 2 Then
      returned = aclsData_Temp.Update_Client_Folders_Sort(Math.Abs(newPriorityOrder), 0, pId, sort_field)
    ElseIf sort_field = 1 Then
      returned = aclsData_Temp.Update_Client_Folders_Sort(0, Math.Abs(newPriorityOrder), pId, sort_field)
    End If
  End Sub

  ''' <summary>
  ''' This button runs whenever you want to add an active folder from a search.
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub folder_submit_button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles folder_submit_button.Click
    Dim ReturnedID As Long = 0
    Dim pageURL As String = ""
    Select Case cfolder_type_of_folder.Text
      Case "1"
        pageURL = "listing.aspx"
      Case "2"
        pageURL = "listing_contact.aspx"
      Case "3"
        pageURL = "listing_air.aspx"
      Case "8"
        pageURL = "listing_transaction.aspx"
    End Select
    If cfolder_id.Text = 0 Then
      ReturnedID = aclsData_Temp.Insert_Into_Client_Active_Folder(cfolder_type_of_folder.Text, Left(Replace(cfolder_name.Text, "'", "\'"), 100), CInt(Session.Item("localUser").crmLocalUserID), IIf(cfolder_share.Checked, "Y", "N"), IIf(cfolder_hide.Checked, "Y", "N"), 2, 1, clsGeneral.clsGeneral.StripChars(cfolder_method.Text, True), Replace(cfolder_data.Text, "'", "''"), Replace(cfolder_description.Text, "'", "\'"))
      If ReturnedID > 0 Then
        Session.Item("isSubnode") = True
        Session.Item("Subnode") = ReturnedID
        Session.Item("SubnodeName") = clsGeneral.clsGeneral.StripChars(cfolder_name.Text, True)
        Session.Item("SubnodeMethod") = "A"
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location='" & pageURL & "';", True)
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
      Else
        active_folder_attention.Text = "There are an error inserting your folder."
        active_folder_attention.Visible = True
      End If
    Else
      ReturnedID = aclsData_Temp.Update_Client_Folders(CInt(cfolder_type_of_folder.Text), Left(cfolder_name.Text, 100), CInt(Session.Item("localUser").crmLocalUserID), IIf(cfolder_share.Checked = True, "Y", "N"), IIf(cfolder_hide.Checked = True, "Y", "N"), cfolder_sort1.Text, cfolder_sort2.Text, cfolder_data.Text, cfolder_method.Text, cfolder_description.Text, CInt(cfolder_id.Text))
      If ReturnedID > 0 Then
        Session.Item("isSubnode") = True
        Session.Item("Subnode") = cfolder_id.Text
        Session.Item("SubnodeName") = clsGeneral.clsGeneral.StripChars(cfolder_name.Text, True)
        Session.Item("SubnodeMethod") = "A"
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Refresh_Window", "window.opener.location='" & pageURL & "';", True)
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Close_Window", "self.close();", True)
      Else
        active_folder_attention.Text = "There are an error updating your folder."
        active_folder_attention.Visible = True
      End If
    End If
  End Sub


End Class