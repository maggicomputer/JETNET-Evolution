Imports System.IO
Partial Public Class SubNav
  Inherits System.Web.UI.UserControl
  'Public Event AddToFolder(ByVal selectedvalue As Integer, ByVal remove As Boolean)
  Dim selvalue As Integer
  Dim table, atemptable2, aTempTable As DataTable
  Dim error_string As String = ""
  Public Event Show_Both_Jetnet_Client_AC_Tabs(ByVal show_jetnet As CheckBox)

  Private Sub add_folder_cbo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles add_folder_cbo.SelectedIndexChanged
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Try
      selvalue = add_folder_cbo.SelectedValue
    Catch ex As Exception
      error_string = "SubNav.ascx.vb - add_folder_cbo_SelectedIndexChanged() - " & ex.Message
      masterPage.LogError(error_string)
    End Try
  End Sub
  Public Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try
      If Session.Item("crmUserLogon") <> True Then
        Response.Redirect("Default.aspx", False)
      Else

        If Not Page.IsPostBack Then
          If Me.Visible Then
            Dim masterPage As main_site = DirectCast(Page.Master, main_site)
            Try



              'toggle back to listing button
              If Not IsNothing(Session("Results")) And (Session("ListingID") <> 0) Then
                back_visible.Visible = True
              Else
                back_visible.Visible = False
              End If

              If Session.Item("localUser").crmEvo = True Then 'If an EVO user
                ' Me.Visible = False
                If masterPage.TypeOfListing <> 12 Then
                  operations_text.Text = ""
                End If
                selected_item_menu.Visible = False
                my_companies.Visible = False
                my_contacts.Visible = False
                toggle_evo_js.Text = ""
              Else

                'Special change for just safari, making the dropdowns work
                If Request.UserAgent.IndexOf("AppleWebKit") > 0 Then
                  Request.Browser.Adapters.Clear()
                End If

                If InStr(UCase(Request.ServerVariables("SCRIPT_NAME").ToString()), "LISTING_AIR.ASPX") > 0 Then
                  selected_item_menu.Visible = True
                ElseIf InStr(UCase(Request.ServerVariables("SCRIPT_NAME").ToString()), "LISTING.ASPX") > 0 Then
                  my_companies.Visible = True
                ElseIf InStr(UCase(Request.ServerVariables("SCRIPT_NAME").ToString()), "LISTING_CONTACT.ASPX") > 0 Then
                  my_contacts.Visible = True
                End If
                new_search.Visible = False
                switch.Width = 0
                switch.ImageUrl = "~/images/spacer.gif"

                If Not Page.IsPostBack Then
                  If masterPage.ListingID = 0 Then
                    switch.Visible = False
                  ElseIf masterPage.OtherID = 0 Then
                    switch.Visible = False
                  Else
                    switch.Visible = True
                  End If
                  If masterPage.ListingSource = "CLIENT" Then
                    switch.ImageUrl = "~/images/switch_jetnet.jpg"
                    switch.Width = 30
                    switch.CssClass = "switch_icon"
                  ElseIf masterPage.ListingSource = "JETNET" Then
                    switch.ImageUrl = "~/images/switch_client.jpg"
                    switch.Width = 30
                    switch.CssClass = "switch_icon"
                  End If

                  If masterPage.ListingID > 0 Then
                    If Session.Item("localSubscription").crmAerodexFlag = False Then
                      'If we have a listing ID, go ahead and check if we're on an aircraft/company list. If we are, go ahead and display the prospect view Icon.
                      If masterPage.TypeOfListing = 3 Then 'Or masterPage.TypeOfListing = 1 Then
                        gold_prospect_icon_label.Visible = True
                        gold_prospect_icon_label.Text = "<img src='/images/gold_prospect_icon.png' alt='Prospect View' class='gold_icon help_cursor' title='Launch Prospect View' onclick=""javascript:load('view_template.aspx?ViewID=18&" & IIf(masterPage.TypeOfListing = 3, "ac_id=", "comp_id=") & IIf(masterPage.ListingSource = "JETNET", masterPage.ListingID, masterPage.OtherID) & "&noMaster=false','','scrollbars=yes,menubar=no,height=700,width=1150,resizable=yes,toolbar=no,location=no,status=no');""/>"
                      End If
                    Else
                      valuation_label.Visible = False
                    End If
                  End If
                End If
                If masterPage.ListingID <> 0 And masterPage.ShowSearch <> True Then
                  new_search.Visible = True
                End If

                If my_companies.Visible = True Then
                  'show shared folers!
                  atemptable2 = masterPage.aclsData_Temp.Get_Client_Folders_Shared("Y", 1, True)
                  If Not IsNothing(atemptable2) Then
                    If atemptable2.Rows.Count > 0 Then
                      For Each r As DataRow In atemptable2.Rows
                        If UCase(r("cfolder_name")) = "MY COMPANIES" Then
                        Else
                          If r("cfolder_method") <> "A" Then
                            my_companies.Items(0).ChildItems.Add(New MenuItem("Save Selections to " & r("cfolder_name"), "5|" & r("cfolder_id")))
                          End If
                        End If
                      Next
                    End If
                  Else
                    If masterPage.aclsData_Temp.class_error <> "" Then
                      error_string = masterPage.aclsData_Temp.class_error
                      masterPage.LogError("SubNav.ascx.vb - Page_Load() - " & error_string)
                    End If
                    masterPage.display_error()
                  End If

                  atemptable2 = masterPage.aclsData_Temp.Get_Client_Folders_NonShared(CInt(Session.Item("localUser").crmLocalUserID), "N", 1, True)
                  If Not IsNothing(atemptable2) Then
                    If atemptable2.Rows.Count > 0 Then
                      For Each r As DataRow In atemptable2.Rows
                        If UCase(r("cfolder_name")) = "MY COMPANIES" Then
                        Else
                          If r("cfolder_method") <> "A" Then
                            my_companies.Items(0).ChildItems.Add(New MenuItem("Save Selections to " & r("cfolder_name"), "5|" & r("cfolder_id")))
                          End If
                        End If
                      Next
                    End If
                  Else
                    If masterPage.aclsData_Temp.class_error <> "" Then
                      error_string = masterPage.aclsData_Temp.class_error
                      masterPage.LogError("SubNav.ascx.vb - Page_Load() - " & error_string)
                    End If
                    masterPage.display_error()
                  End If
                ElseIf my_contacts.Visible = True Then
                  'show shared folers!
                  atemptable2 = masterPage.aclsData_Temp.Get_Client_Folders_Shared("Y", 2, True)
                  If Not IsNothing(atemptable2) Then
                    If atemptable2.Rows.Count > 0 Then
                      For Each r As DataRow In atemptable2.Rows
                        If UCase(r("cfolder_name")) = "MY CONTACTS" Then
                        Else
                          If r("cfolder_method") <> "A" Then
                            my_contacts.Items(0).ChildItems.Add(New MenuItem("Save Selections to " & r("cfolder_name"), "5|" & r("cfolder_id")))
                          End If
                        End If
                      Next
                    End If
                  Else
                    If masterPage.aclsData_Temp.class_error <> "" Then
                      error_string = masterPage.aclsData_Temp.class_error
                      masterPage.LogError("SubNav.ascx.vb - Page_Load() - " & error_string)
                    End If
                    masterPage.display_error()
                  End If

                  atemptable2 = masterPage.aclsData_Temp.Get_Client_Folders_NonShared(CInt(Session.Item("localUser").crmLocalUserID), "N", 2, True)
                  If Not IsNothing(atemptable2) Then
                    If atemptable2.Rows.Count > 0 Then
                      For Each r As DataRow In atemptable2.Rows
                        If UCase(r("cfolder_name")) = "MY CONTACTS" Then
                        Else
                          If r("cfolder_method") <> "A" Then
                            my_contacts.Items(0).ChildItems.Add(New MenuItem("Save Selections to " & r("cfolder_name"), "5|" & r("cfolder_id")))
                          End If
                        End If
                      Next
                    End If
                  Else
                    If masterPage.aclsData_Temp.class_error <> "" Then
                      error_string = masterPage.aclsData_Temp.class_error
                      masterPage.LogError("SubNav.ascx.vb - Page_Load() - " & error_string)
                    End If
                    masterPage.display_error()
                  End If
                ElseIf selected_item_menu.Visible = True Then
                  'show shared folers!
                  atemptable2 = masterPage.aclsData_Temp.Get_Client_Folders_Shared("Y", 3, True)
                  If Not IsNothing(atemptable2) Then
                    If atemptable2.Rows.Count > 0 Then
                      For Each r As DataRow In atemptable2.Rows
                        If UCase(r("cfolder_name")) = "MY AIRCRAFT" Then
                        Else
                          If r("cfolder_method") <> "A" Then
                            selected_item_menu.Items(0).ChildItems.Add(New MenuItem("Save Selections to " & r("cfolder_name"), "5|" & r("cfolder_id")))
                          End If
                        End If
                      Next
                    End If

                  Else
                    If masterPage.aclsData_Temp.class_error <> "" Then
                      error_string = masterPage.aclsData_Temp.class_error
                      masterPage.LogError("SubNav.ascx.vb - Page_Load() - " & error_string)
                    End If
                    masterPage.display_error()
                  End If

                  atemptable2 = masterPage.aclsData_Temp.Get_Client_Folders_NonShared(CInt(Session.Item("localUser").crmLocalUserID), "N", 3, True)
                  If Not IsNothing(atemptable2) Then
                    If atemptable2.Rows.Count > 0 Then
                      For Each r As DataRow In atemptable2.Rows
                        If UCase(r("cfolder_name")) = "MY AIRCRAFT" Then
                        Else
                          If r("cfolder_method") <> "A" Then
                            selected_item_menu.Items(0).ChildItems.Add(New MenuItem("Save Selections to " & r("cfolder_name"), "5|" & r("cfolder_id")))
                          End If
                        End If
                      Next
                    End If
                  Else
                    If masterPage.aclsData_Temp.class_error <> "" Then
                      error_string = masterPage.aclsData_Temp.class_error
                      masterPage.LogError("SubNav.ascx.vb - Page_Load() - " & error_string)
                    End If
                    masterPage.display_error()
                  End If
                  Dim NewFolder As New MenuItem
                  NewFolder.NavigateUrl = "/edit.aspx?action=folder&type=add_folderAuto&folderType=3"
                  NewFolder.Text = "Save Selections to New Folder"
                  NewFolder.Value = "6"
                  NewFolder.Target = "new"
                  selected_item_menu.Items(0).ChildItems.Add(NewFolder)
                End If
                If masterPage.NameOfSubnode <> "" Then
                  If masterPage.Subnode_Method <> "A" Then
                    If masterPage.NameOfSubnode <> masterPage.NameOfListingType Then
                      selected_item_menu.Items(0).ChildItems.Add(New MenuItem("Remove Selected From " & masterPage.NameOfSubnode, 6))
                      my_companies.Items(0).ChildItems.Add(New MenuItem("Remove Selected From " & masterPage.NameOfSubnode, 6))
                      my_contacts.Items(0).ChildItems.Add(New MenuItem("Remove Selected From " & masterPage.NameOfSubnode, 6))
                    Else
                      selected_item_menu.Items(0).ChildItems.Remove(New MenuItem("Remove Selected From " & masterPage.NameOfSubnode, 6))
                      my_companies.Items(0).ChildItems.Remove(New MenuItem("Remove Selected From " & masterPage.NameOfSubnode, 6))
                      my_contacts.Items(0).ChildItems.Remove(New MenuItem("Remove Selected From " & masterPage.NameOfSubnode, 6))
                    End If
                  End If
                End If
              End If
            Catch ex As Exception
              error_string = "SubNav.ascx.vb - add_folder_cbo_SelectedIndexChanged() - " & ex.Message
              masterPage.LogError(error_string)
            End Try
          End If
        End If
      End If
   Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "SubNav.ascx.vb - Page Load() - " + ex.Message
    End Try
  End Sub


  Private Sub selected_item_menu_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles selected_item_menu.MenuItemClick, my_companies.MenuItemClick, my_contacts.MenuItemClick
    Dim masterPage As main_site = DirectCast(Page.Master, main_site)
    Try



      'select all -  check all boxes on screen 

      'clear selections - uncheck all boxes


      'temporary list = selected boxes - clear after added to folder so only checked save


      'my aircraft folder is just like an Aircraft. default. Never display my aircraft folder. 

      'remove selected from my aircraft - only on my aircraft. 

      'remove selected from my aircraft (my folder) only shows up on folder. No folder displayed - menu option
      'not there. Repaints page when done with remove selected. 

      'view only selected aircraft - give it a try. collapse and expand.

      Dim cookie_name As String = ""
      Dim cookieString As String = ""
      Dim folder_name As String = ""
      Dim atemptable As DataTable = Session("Results")


      Select Case masterPage.TypeOfListing
        Case 1
          cookie_name = "companies_marked"
          folder_name = "My Companies"
        Case 2
          cookie_name = "contacts_marked"
          folder_name = "My Contacts"
        Case 3
          cookie_name = "aircraft_marked"
          folder_name = "My Aircraft"
      End Select
      Select Case e.Item.Value
        Case "1" 'sselect all companies
          Select Case masterPage.TypeOfListing
            Case 3
              If Not IsNothing(atemptable) Then
                If atemptable.Rows.Count > 0 Then
                  For Each r As DataRow In atemptable.Rows
                    If Not IsDBNull(r("ac_id")) And Not IsDBNull(r("source")) Then
                      cookieString = cookieString & (r("ac_id") & "#" & r("source") & "|")
                    End If
                    If Not IsDBNull(r("other_ac_id")) And Not IsDBNull(r("other_source")) Then
                      cookieString = cookieString & (r("other_ac_id") & "#" & r("other_source") & "|")
                    End If

                  Next
                End If
              Else
                If masterPage.aclsData_Temp.class_error <> "" Then
                  error_string = masterPage.aclsData_Temp.class_error
                  masterPage.LogError("SubNav.ascx.vb - selected_item_menu_MenuItemClick() - " & error_string)
                End If
                masterPage.display_error()
              End If
            Case 1
              If Not IsNothing(atemptable) Then
                If atemptable.Rows.Count > 0 Then
                  For Each r As DataRow In atemptable.Rows
                    cookieString = cookieString & (r("comp_id") & "#" & r("source") & "|")
                  Next
                End If
              Else
                If masterPage.aclsData_Temp.class_error <> "" Then
                  error_string = masterPage.aclsData_Temp.class_error
                  masterPage.LogError("SubNav.ascx.vb - selected_item_menu_MenuItemClick() - " & error_string)
                End If
                masterPage.display_error()
              End If
            Case 2
              If Not IsNothing(atemptable) Then
                If atemptable.Rows.Count > 0 Then
                  For Each r As DataRow In atemptable.Rows
                    cookieString = cookieString & (r("contact_id") & "#" & r("contact_type") & "|")
                  Next
                End If
              Else
                If masterPage.aclsData_Temp.class_error <> "" Then
                  error_string = masterPage.aclsData_Temp.class_error
                  masterPage.LogError("SubNav.ascx.vb - selected_item_menu_MenuItemClick() - " & error_string)
                End If
                masterPage.display_error()
              End If
          End Select
          Dim aCookie As HttpCookie = Request.Cookies(cookie_name)


          If cookieString <> "" Then
            cookieString = UCase(cookieString.TrimEnd("|"))
          End If
          If aCookie IsNot Nothing Then
            If aCookie.Value <> "" Then
              cookieString = aCookie.Value & "|" & cookieString
            Else
              cookieString = cookieString
            End If
            Response.Cookies(cookie_name).Value = cookieString
          Else
            aCookie = New HttpCookie(cookie_name)
            aCookie.Value = cookieString
            HttpContext.Current.Response.Cookies.Add(aCookie)
          End If
          masterPage.PerformDatabaseAction = True
          Response.Redirect(Replace(Request.Url.ToString, "?redo_search=true", "") & "?redo_search=true", False)
          HttpContext.Current.ApplicationInstance.CompleteRequest()
          masterPage.m_bIsTerminating = True

        Case "2" 'save to my AC
          masterPage.PerformDatabaseAction = True
          masterPage.mark_all_selected_items(cookie_name, folder_name)
          masterPage.PerformDatabaseAction = False
        Case "6" 'remove all selected
          masterPage.PerformDatabaseAction = True
          masterPage.remove_all_selected_items(cookie_name, folder_name)
          masterPage.PerformDatabaseAction = False
        Case "4"
          'Response.Write("Clear Selections (View All Aircraft)")
          ' Case InStr(e.Item.Value, "5|") > 0
          'Response.Write("Save Selections to Folder")

          '   System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "Error", "load('edit.aspx?action=folder&type=add_list','scrollbars=yes,menubar=no,height=30,width=400,resizable=yes,toolbar=no,location=no,status=no');", True)
        Case "6"
          'Create new folder:
          'Response.Redirect("listing_air.aspx?redo_search=true")
        Case Else
          If InStr(e.Item.Value, "5|") > 0 Then
            Dim spli As String() = Split(e.Item.Value, "|")
            Response.Redirect("edit.aspx?action=folder&type=add_list&auto=" & spli(1), False)
          End If
          'Response.Write("There was an Error During Menu Selection")
      End Select
    Catch ex As Exception
      error_string = "Submenu_Edit_Template.ascx.vb - selected_item_menu_MenuItemClick() - " & ex.Message
      masterPage.LogError(error_string)
    End Try
  End Sub

  Private Sub show_jetnet_client_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles show_jetnet_client.CheckedChanged
    'These means that you show jetnet and client data. An event has to fire, get captured by the details page, get sent to the aircraft tabs
    'to rerun the change tab function.
    RaiseEvent Show_Both_Jetnet_Client_AC_Tabs(show_jetnet_client)
  End Sub
End Class