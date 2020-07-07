
' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/homeMenuEditor.aspx.vb $
'$$Author: Amanda $
'$$Date: 9/06/19 3:57p $
'$$Modtime: 9/06/19 3:49p $
'$$Revision: 9 $
'$$Workfile: homeMenuEditor.aspx.vb $
'
' ********************************************************************************

Public Class homeMenuEditor
    Inherits System.Web.UI.Page
    Dim UniquePageTable As New DataTable
    Public Shared masterPage As New Object
    Protected localDatalayer As New admin_center_dataLayer
    Dim level As String = "Main"
    Dim parentR As String = ""
    Private Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

        Try
            If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
                Me.MasterPageFile = "~/EvoStyles/CustomerAdminTheme.master"
                masterPage = DirectCast(Page.Master, CustomerAdminTheme)
            ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
                Me.MasterPageFile = "~/EvoStyles/EmptyHomebaseTheme.Master"
                masterPage = DirectCast(Page.Master, EmptyHomebaseTheme)
            ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
                Me.MasterPageFile = "~/EvoStyles/EmptyEvoTheme.master"
                masterPage = DirectCast(Page.Master, EmptyEvoTheme)
            End If

        Catch ex As Exception
            If Not IsNothing(masterPage) Then
                masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString + " : " + ex.Message.ToString.Trim)
            Else
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodInfo.GetCurrentMethod().ToString + "</b><br />" + ex.Message.ToString.Trim
            End If
        End Try

    End Sub
    Public Function CreateALink(ByVal ItemName As String, ByVal PageName As String, ByVal Count As Long, ByVal DisplayName As Object) As String
        Dim returnString As String = ""
        Dim cssBold As String = ""
        If Count > 0 Then
            cssBold = "bold"
        End If

        returnString += "<a href=""/homeMenuEditor.aspx?level=" & Server.UrlEncode(ItemName) & "&parent=" & Server.UrlEncode(PageName) & """ class=""blue " & cssBold & """>"
        If Not IsDBNull(DisplayName) Then
            returnString += DisplayName
        End If
        returnString += "</a>"

        Return returnString
    End Function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        localDatalayer.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
        localDatalayer.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
        localDatalayer.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
        localDatalayer.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
        localDatalayer.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

        If Not IsNothing(Trim(Request("level"))) Then
            If Not String.IsNullOrEmpty(Trim(Request("level"))) Then
                level = Server.UrlDecode(Trim(Request("level")))
            End If
        End If
        If Not IsNothing(Trim(Request("parent"))) Then
            If Not String.IsNullOrEmpty(Trim(Request("parent"))) Then
                parentR = Server.UrlDecode(Trim(Request("parent")))
            End If
        End If


        UniquePageTable = localDatalayer.UniquePage()
        new_menutree_page_name.Items.Add(New ListItem("Main", "Main"))

        If Not Page.IsPostBack Then
            If Not IsNothing(uniquePageTable) Then
                If uniquePageTable.Rows.Count > 0 Then
                    For Each r As DataRow In uniquePageTable.Rows
                        new_menutree_page_name.Items.Add(New ListItem(r("menutree_page_name"), r("menutree_page_name")))
                    Next
                End If
            End If
        End If

        Try
            new_menutree_page_name.SelectedValue = level
        Catch ex As Exception
            new_menutree_page_name.SelectedValue = "Main"
        End Try


        If Not Page.IsPostBack Then
            DisplayLower(parentR, level)
        End If
    End Sub
    Public Function FolderClassDisplay(ByVal level As Long) As String

        Dim returnString As String = ""
        If level > 0 Then
            returnString = "<img src=""/images/folder-2x.png"" alt=""Drag Folder Here"" />"
        Else
            returnString = "<img src=""/images/file-2x.png"" alt=""Drag Folder Here"" />"
        End If

        Return returnString
    End Function


    Sub Save(ByVal Sender As Object, ByVal e As AjaxControlToolkit.ReorderListCommandEventArgs)
        If Page.IsValid Then
            Dim menutree_page_name As DropDownList = e.Item.FindControl("menutree_page_name")
            Dim menutree_item_name As TextBox = e.Item.FindControl("menutree_item_name")
            Dim menutree_display_name As TextBox = e.Item.FindControl("menutree_display_name")
            Dim menutree_display_url As TextBox = e.Item.FindControl("menutree_display_url")
            Dim menutree_admin_flag As CheckBox = CType(e.Item.FindControl("menutree_admin_flag"), CheckBox)
            Dim menutree_status As DropDownList = e.Item.FindControl("menutree_status")
            Dim menutree_description As TextBox = e.Item.FindControl("menutree_description")
            Dim old_menutree_item_name As Label = e.Item.FindControl("old_menutree_item_name")
            Dim menutree_id As TextBox = e.Item.FindControl("id")
            MenuTreeUpdate(menutree_status.SelectedValue, menutree_display_url.Text, menutree_item_name.Text, menutree_page_name.SelectedValue, menutree_id.Text, menutree_description.Text, menutree_display_name.Text, IIf(menutree_admin_flag.Checked, "Y", "N"))


        End If
    End Sub

    Public Function MenuTreeUpdate(ByVal menutree_status As String, ByVal menutree_display_url As String, ByVal menutree_item_name As String, ByVal menutree_page_name As String, ByVal menuTree_ID As Long, ByVal menutree_description As String, ByVal menutree_display_name As String, ByVal menutree_admin_flag As String) As Boolean
        Dim Query As String = ""
        'Dim SqlConn As New SqlClient.SqlConnection
        'Dim SqlReader As SqlClient.SqlDataReader
        'Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try
            'SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            'SqlConn.Open()
            Query = "update menu_tree set menutree_status = @menutree_status, menutree_admin_flag = @menutree_admin_flag,  menutree_description = @menutree_description, "
            Query += " menutree_display_url = @menutree_display_url, menutree_display_name = @menutree_display_name, menutree_item_name = @menutree_item_name, menutree_page_name = @menutree_page_name, "
            Query += "  where menutree_id = @menutree_id limit 1"



            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, Query.ToString)

            'Dim SqlCommand As New Sql.Data.MySqlClient.MySqlCommand(Query, SqlConn)
            'SqlCommand.Parameters.AddWithValue("@menutree_status", menutree_status)

            'SqlCommand.Parameters.AddWithValue("@menutree_admin_flag", menutree_admin_flag)
            'SqlCommand.Parameters.AddWithValue("@menutree_description", menutree_description)

            'SqlCommand.Parameters.AddWithValue("@menutree_display_url", menutree_display_url)
            'SqlCommand.Parameters.AddWithValue("@menutree_display_name", menutree_display_name)
            'SqlCommand.Parameters.AddWithValue("@menutree_item_name", menutree_item_name)
            'SqlCommand.Parameters.AddWithValue("@menutree_page_name", menutree_page_name)

            'SqlCommand.Parameters.AddWithValue("menutree_id", menuTree_ID)

            'SqlCommand.ExecuteNonQuery()

            Return True
            'SqlCommand.Dispose()
            'SqlCommand = Nothing
        Catch ex As Exception
            'Me.class_error = Me.class_error & "Error in " & System.Reflection.MethodBase.GetCurrentMethod().Name.ToString & ": " & ex.Message & "<br />"
            Return False
        Finally
            'kill everything
            'SqlReader = Nothing

            'SqlConn.Dispose()
            'SqlConn.Close()
            'SqlConn = Nothing


        End Try
    End Function

    Public Function isCheckedItem(isChecked As Object) As Boolean
        Dim returnBool As Boolean = False
        If Not IsDBNull(isChecked) Then
            If isChecked.ToString.ToUpper = "Y" Then
                returnBool = True
            End If
        End If
        Return returnBool
    End Function

    Public Sub DisplayLower(ByVal PageToExample As String, ByVal level As String)
        Dim returnTable As New DataTable
        If level <> "Main" Then
            returnTable = localDatalayer.MenuFilter(level, "", "")
        Else
            returnTable = localDatalayer.MenuFilter("Main", "", "")
        End If
        If Not IsNothing(returnTable) Then
            Reorder_ListR.DataSource = returnTable
            Reorder_ListR.DataBind()
        End If

    End Sub

    Sub Save_Row(ByVal sender As Object, ByVal e As AjaxControlToolkit.ReorderListCommandEventArgs)
        If e.CommandName = "Edit" Then
            edit(sender, e) 'Sets up the edit form.
        ElseIf e.CommandName = "Save" Then 'Save the edits
            Save(sender, e)
            'ElseIf e.CommandName = "Delete" Then
            '    Delete(sender, e) 'Sets up the deletion
        ElseIf e.CommandName = "Cancel" Then
            Cancel(sender) 'Cancelling.
        End If

    End Sub

    Public Sub Cancel(ByVal sender As Object)
        Reorder_ListR.ShowInsertItem = False
        Reorder_ListR.EditItemIndex = -1
        'Rebinding
        BindData()
    End Sub

    Sub Edit(ByVal Sender As Object, ByVal e As AjaxControlToolkit.ReorderListCommandEventArgs)
        Reorder_ListR.ShowInsertItem = False
        Reorder_ListR.EditItemIndex = CInt(e.Item.ItemIndex)

        BindData()
        Dim mID As TextBox = CType(e.Item.FindControl("id"), TextBox)

        accessID.Text = mID.Text

    End Sub
    Private Sub Reorder_ListR_ItemDataBound(ByVal sender As Object, ByVal e As AjaxControlToolkit.ReorderListItemEventArgs) Handles Reorder_ListR.ItemDataBound
        Dim pageDropdown As DropDownList = CType(e.Item.FindControl("menutree_page_name"), DropDownList)

        Dim pageDropdownCurrent As TextBox = CType(e.Item.FindControl("menutree_page_name_current"), TextBox)

        Dim name As TextBox = CType(e.Item.FindControl("menutree_item_name"), TextBox)



        pageDropdown.Items.Add(New ListItem(pageDropdownCurrent.Text, pageDropdownCurrent.Text)) 'Just for safety so something is there if the page doesn't fill.
        If Not IsNothing(uniquePageTable) Then
            If uniquePageTable.Rows.Count > 0 Then
                For Each r As DataRow In uniquePageTable.Rows
                    pageDropdown.Items.Add(New ListItem(r("menutree_page_name"), r("menutree_page_name")))
                Next
            End If
        End If


        Try
            pageDropdown.SelectedValue = pageDropdownCurrent.Text
        Catch ex As Exception
        End Try


    End Sub

    Protected Sub Reorder_ListR_ItemReorder(sender As Object, e As AjaxControlToolkit.ReorderListItemReorderEventArgs)
        Dim dataTabled As New DataTable


        dataTabled = localDatalayer.MenuFilter(level, "", "")
        ReorderFolderList(dataTabled, e)
        dataTabled = dataTabled
    End Sub
    ''' <summary>
    ''' Function that runs to reorder the folder list, it goes ahead and accepts a datatable
    ''' </summary>
    ''' <param name="datatable"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' 
    Sub ReorderFolderList(ByVal datatable As DataTable, ByVal e As AjaxControlToolkit.ReorderListItemReorderEventArgs)

        Dim oldIndex As Integer = e.OldIndex
        Dim newIndex As Integer = e.NewIndex
        Dim newPriorityOrder As Integer = CInt(datatable.Rows(newIndex)("menutree_order"))

        If newIndex > oldIndex Then
            'item moved down
            For i As Integer = oldIndex + 1 To newIndex
                Dim propertyId As Integer = CInt(datatable.Rows(i)("menutree_id"))
                If propertyId <> -1 Then
                    datatable.Rows(i)("menutree_order") = CInt(datatable.Rows(i)("menutree_order")) - 1
                    'menuDataQueries.MenuTreeUpdateSort(propertyId, CInt(datatable.Rows(i)("menutree_order")), MenuTreeSite)
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Reorder - id: " & propertyId.ToString & " order: " & CInt(datatable.Rows(i)("menutree_order"))
                End If
            Next
        Else
            'item moved up
            For i As Integer = oldIndex - 1 To newIndex Step -1
                Dim propertyId As Integer = datatable.Rows(i)("menutree_id")
                If propertyId <> -1 Then
                    datatable.Rows(i)("menutree_order") = CInt(datatable.Rows(i)("menutree_order")) + 1
                    'menuDataQueries.MenuTreeUpdateSort(propertyId, CInt(datatable.Rows(i)("menutree_order")), MenuTreeSite)
                    HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Reorder - id: " & propertyId.ToString & " order: " & CInt(datatable.Rows(i)("menutree_order"))
                End If
            Next
        End If

        'Finally, update the priority for origional row            
        Dim id As Integer = CInt(datatable.Rows(oldIndex)("menutree_id"))
        If id <> -1 Then
            'menuDataQueries.MenuTreeUpdateSort(id, newPriorityOrder, MenuTreeSite)
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Reorder - id: " & id.ToString & " order: " & newPriorityOrder
        End If



    End Sub

    Sub BindData()
        Dim returnString As String = ""
        DisplayLower(parentR, level)
        'DisplayMenu(parentR, returnString, "first", level)
    End Sub

End Class