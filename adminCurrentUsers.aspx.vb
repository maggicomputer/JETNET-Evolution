' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/adminCurrentUsers.aspx.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:36a $
'$$Modtime: 6/18/19 6:11p $
'$$Revision: 2 $
'$$Workfile: adminCurrentUsers.aspx.vb $
'
' ********************************************************************************

Partial Public Class adminCurrentUsers
  Inherits System.Web.UI.Page

  Protected localDatalayer As New admin_center_dataLayer
  Protected localCriteria As New onLineUsersSelectionCriteriaClass

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Dim temp_log_data As String = ""
    Dim temp_main_data As String = ""
    Dim sTabTitle As String = ""
    Dim bHasTitle As Boolean = False

    Dim contact_info As String = ""

    Dim sErrorString As String = ""

    If Session.Item("crmUserLogon") <> True Then

      Response.Redirect("Default.aspx", True)

    Else

      If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString), _
                                                              HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString, _
                                                              CLng(HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString), _
                                                              CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)) Then
        Response.Redirect("Default.aspx", True)
      End If

      localDatalayer.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      localDatalayer.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      localDatalayer.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
      localDatalayer.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
      localDatalayer.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

      localDatalayer.crmMasterConnectStr = HttpContext.Current.Application.Item("crmMasterDatabase").ToString.Trim

      Master.Set_Active_Tab(1)

      If Not IsNothing(Request.Item("show")) Then
        If Not String.IsNullOrEmpty(Request.Item("show").Trim) Then
          If IsNumeric(Request.Item("show").Trim) Then
            localCriteria.OnLineCriteriaNumberToShow = CLng(Request.Item("show").Trim)
          End If
        End If
      End If

      If Not IsNothing(Request.Item("search")) Then
        If Not String.IsNullOrEmpty(Request.Item("search").Trim) And String.IsNullOrEmpty(name_search.Text.Trim) Then
          localCriteria.OnLineCriteriaSearchItem = Request.Item("search").Trim
        End If
      End If

      If Not IsNothing(Request.Item("bus_type")) Then
        If Not String.IsNullOrEmpty(Request.Item("bus_type").Trim) Then
          localCriteria.OnLineCriteriaBusType = Request.Item("bus_type").Trim
        End If
      End If

      If Not IsNothing(Request.Item("browser")) Then
        If Not String.IsNullOrEmpty(Request.Item("browser").Trim) Then
          localCriteria.OnLineCriteriaByBrowser = IIf(Request.Item("browser").Trim.Contains("Y"), True, False)
        End If
      End If

      If Not IsNothing(Request.Item("new")) Then
        If Not String.IsNullOrEmpty(Request.Item("new").Trim) Then
          localCriteria.OnLineCriteriaByNew = IIf(Request.Item("new").Trim.Contains("Y"), True, False)
        End If
      End If

      If Not IsNothing(Request.Item("productCode")) Then
        If Not String.IsNullOrEmpty(Request.Item("productCode").Trim) Then
          If Request.Item("productCode").Trim.ToUpper.Contains("B,C,H,Y") Then
            localCriteria.OnLineCriteriaProductCode = ""
          Else
            localCriteria.OnLineCriteriaProductCode = Request.Item("productCode").Trim
          End If
        End If
      End If

      If Not IsNothing(Request.Item("type")) Then
        If Not String.IsNullOrEmpty(Request.Item("type").Trim) Then
          localCriteria.OnLineCriteriaPlatformType = Request.Item("type").Trim
        End If
      End If

      If Not IsNothing(Request.Item("service")) Then
        If Not String.IsNullOrEmpty(Request.Item("service").Trim) Then
          localCriteria.OnLineCriteriaService = Request.Item("service").Trim
        End If
      End If

      If Not IsNothing(Request.Item("freq")) Then
        If Not String.IsNullOrEmpty(Request.Item("freq").Trim) Then
          localCriteria.OnLineCriteriaFrequency = Request.Item("freq").Trim
        End If
      End If

      If Not IsNothing(Request.Item("order")) Then
        If Not String.IsNullOrEmpty(Request.Item("order").Trim) Then
          localCriteria.OnLineCriteriaOrderBy = Request.Item("order").Trim
        End If
      End If

      If Not IsNothing(Request.Item("info")) Then
        If Not String.IsNullOrEmpty(Request.Item("info").Trim) Then
          localCriteria.OnLineCriteriaInfo = Request.Item("info").Trim
        End If
      End If

      If Not IsNothing(Request.Item("id")) Then
        If Not String.IsNullOrEmpty(Request.Item("id").Trim) Then
          If IsNumeric(Request.Item("id").Trim) Then
            localCriteria.OnLineCriteriaCompanyID = CLng(Request.Item("id").Trim)
          End If
        End If
      End If

      If Not IsNothing(Request.Item("col")) Then
        If Not String.IsNullOrEmpty(Request.Item("col").Trim) Then
          localCriteria.OnLineCriteriaSelectedItem = Request.Item("col").Trim
        End If
      End If

      If Not IsNothing(Request.Item("server")) Then
        If Not String.IsNullOrEmpty(Request.Item("server").Trim) Then
          localCriteria.OnLineCriteriaServer = Request.Item("server").Trim
        End If
      End If

      If Not IsNothing(Request.Item("user_id")) Then
        If Not String.IsNullOrEmpty(Request.Item("user_id").Trim) Then
          If IsNumeric(Request.Item("user_id").Trim) Then
            localCriteria.OnLineCriteriaContactID = CLng(Request.Item("user_id").Trim)
          End If
        End If
      End If

      all_user_panel.Visible = False
      all_user_text.Text = ""

      If Not String.IsNullOrEmpty(localCriteria.OnLineCriteriaBusType.Trim) Then

        localDatalayer.displayAdminCompanyByBusinessType(localCriteria, temp_main_data)

      ElseIf localCriteria.OnLineCriteriaByBrowser Then

        localDatalayer.displayAdminUsersByBrowser(localCriteria, sTabTitle, temp_main_data)

      ElseIf localCriteria.OnLineCriteriaByNew Then

        localDatalayer.displayAdminCRMUsers(localCriteria, temp_main_data)

      Else

        localDatalayer.displayAdminCurrentUsers(localCriteria, sTabTitle, temp_main_data)

        If localCriteria.OnLineCriteriaContactID > 0 Then
          localDatalayer.displayAdminUserLog(localCriteria, msg_type.Text.Trim, temp_log_data)
        ElseIf localCriteria.OnLineCriteriaCompanyID > 0 Then

          localDatalayer.displayAdminUserLog(localCriteria, msg_type.Text.Trim, temp_log_data)

          all_user_panel.Visible = True
          localDatalayer.displayAdminCompanyUsers(localCriteria, sTabTitle, all_user_text.Text)
        End If

      End If

      'if there is log data, add the show last month link 
      If Not String.IsNullOrEmpty(temp_log_data.Trim) Then
        temp_log_data += "<br /><a href=""adminCurrentUsers.aspx?show=7&freq=" + localCriteria.OnLineCriteriaFrequency.Trim + "&user_id=" + localCriteria.OnLineCriteriaContactID.ToString.Trim + "&id=" + localCriteria.OnLineCriteriaCompanyID.ToString.Trim + "&col=" + localCriteria.OnLineCriteriaSelectedItem.Trim + "&order=" + localCriteria.OnLineCriteriaOrderBy.Trim + "&info=" + localCriteria.OnLineCriteriaInfo.Trim + "&search=" + localCriteria.OnLineCriteriaSearchItem.Trim + """>"
        temp_log_data += "Show Last 7 days</a>"
        temp_log_data += "<br /><a href=""adminCurrentUsers.aspx?show=30&freq=" + localCriteria.OnLineCriteriaFrequency.Trim + "&user_id=" + localCriteria.OnLineCriteriaContactID.ToString.Trim + "&id=" + localCriteria.OnLineCriteriaCompanyID.ToString.Trim + "&col=" + localCriteria.OnLineCriteriaSelectedItem.Trim + "&order=" + localCriteria.OnLineCriteriaOrderBy.Trim + "&info=" + localCriteria.OnLineCriteriaInfo.Trim + "&search=" + localCriteria.OnLineCriteriaSearchItem.Trim + """>"
        temp_log_data += "Show Last 30 days</a>"
        temp_log_data += "<br /><a href=""adminCurrentUsers.aspx?show=60&freq=" + localCriteria.OnLineCriteriaFrequency.Trim + "&user_id=" + localCriteria.OnLineCriteriaContactID.ToString.Trim + "&id=" + localCriteria.OnLineCriteriaCompanyID.ToString.Trim + "&col=" + localCriteria.OnLineCriteriaSelectedItem.Trim + "&order=" + localCriteria.OnLineCriteriaOrderBy.Trim + "&info=" + localCriteria.OnLineCriteriaInfo.Trim + "&search=" + localCriteria.OnLineCriteriaSearchItem.Trim + """>"
        temp_log_data += "Show Last 60 days</a>"
      End If

      If Not String.IsNullOrEmpty(sTabTitle.Trim) Then
        left_side_panel.HeaderText = sTabTitle.Trim
        bHasTitle = True
      End If

      If Not bHasTitle Then
        If localCriteria.OnLineCriteriaByBrowser Then
          If localCriteria.OnLineCriteriaPlatformType.Trim.Contains("B") Then
            left_side_panel.HeaderText = "Users Currenty On Browser : " + localCriteria.OnLineCriteriaInfo.Trim
          Else
            left_side_panel.HeaderText = "Users Currenty On Platform : " + localCriteria.OnLineCriteriaInfo.Trim
          End If
        ElseIf localCriteria.OnLineCriteriaByNew Then
          left_side_panel.HeaderText = "MPM Users Logged In"
        ElseIf Not String.IsNullOrEmpty(localCriteria.OnLineCriteriaBusType.Trim) Then
          left_side_panel.HeaderText = commonEvo.GetCompanyBusinessTypeName(localCriteria.OnLineCriteriaBusType).Trim + " Companies"
        End If
      End If

      bottom_tab_panel.HeaderText = "Log Data"
      right_side_text.Text = ""

      If localCriteria.OnLineCriteriaCompanyID = -1 And localCriteria.OnLineCriteriaContactID = -1 Then
        right_tab_container.Visible = False
        bottom_tab_container.Visible = False
        left_side_text.Text = temp_main_data
      Else

        ' if there is a user_id then do that, else, it is a company 
        If localCriteria.OnLineCriteriaContactID > 0 Then

          right_tab_container.Visible = True
          bottom_tab_panel.HeaderText = "Contact Log Data"

          Dim tempTable As New DataTable
          Dim tmpPrefobj As New preferencesDataLayer

          tmpPrefobj.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
          tempTable = tmpPrefobj.ReturnUserDetailsAndImage(localCriteria.OnLineCriteriaContactID)

          If Not IsNothing(tempTable) Then
            If tempTable.Rows.Count > 0 Then

              contact_info = IIf(Not IsDBNull(tempTable.Rows(0).Item("contact_sirname")), IIf(Not String.IsNullOrEmpty(tempTable.Rows(0).Item("contact_sirname").ToString.Trim), tempTable.Rows(0).Item("contact_sirname").ToString.Trim + "&nbsp;", ""), "")
              contact_info += IIf(Not IsDBNull(tempTable.Rows(0).Item("contact_first_name")), tempTable.Rows(0).Item("contact_first_name").ToString.Trim + "&nbsp;", "")
              contact_info += IIf(Not IsDBNull(tempTable.Rows(0).Item("contact_middle_initial")), IIf(Not String.IsNullOrEmpty(tempTable.Rows(0).Item("contact_middle_initial").ToString.Trim), tempTable.Rows(0).Item("contact_middle_initial").ToString.Trim + ".&nbsp;", ""), "")
              contact_info += IIf(Not IsDBNull(tempTable.Rows(0).Item("contact_last_name")), tempTable.Rows(0).Item("contact_last_name").ToString.Trim, "")

              ContactFunctions.Display_Contact_Details(tempTable, Me.right_side_text, 0, 0, Master, False, False)

            End If
          End If

        Else
          bottom_tab_panel.HeaderText = "Company Log Data"
        End If

        crmWebClient.CompanyFunctions.Fill_Information_Tab(left_side_panel, left_side_text, Master, localCriteria.OnLineCriteriaCompanyID, 0, "", invisible_label, left_tab_container, invisible_label, invisible_label, False)

        right_side_text.Text = temp_main_data
        bottom_tab_text.Text = temp_log_data

      End If

    End If

  End Sub

End Class