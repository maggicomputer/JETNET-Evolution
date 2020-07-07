' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/adminDeveloper.aspx.vb $
'$$Author: Mike $
'$$Date: 7/19/19 10:23a $
'$$Modtime: 7/19/19 6:05a $
'$$Revision: 3 $
'$$Workfile: adminDeveloper.aspx.vb $
'
' ********************************************************************************

Partial Public Class adminDeveloper
  Inherits System.Web.UI.Page

  Protected localDatalayer As New admin_center_dataLayer

  Private sProjectKey As String = "0"
  Private sProjectTitle As String = ""
  Private sProjectPriority As String = ""
  Private sProjectStaffName As String = ""
  Private sProjectTask As String = ""
  Private sProjectTaskStatus As String = ""
  Public Shared masterPage As New Object

  Private Sub adminDeveloper_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
    If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
      Me.MasterPageFile = "~/EvoStyles/CustomerAdminTheme.Master"
      masterPage = DirectCast(Page.Master, CustomerAdminTheme)
    ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
      Me.MasterPageFile = "~/EvoStyles/HomebaseTheme.Master"
      masterPage = DirectCast(Page.Master, HomebaseTheme)
    End If
  End Sub

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Dim bShowTaskDetails As Boolean = False
    Dim bAddNewTask As Boolean = False

    Dim developerCritera As New developerSelectionCriteriaClass

    Dim sDisplayDevelopmentProject As String = ""
    Dim sDisplayDevelopmentStaff As String = ""
    Dim sDisplayDevelopmentSummary As String = ""

    Dim sDisplayTaskDetails As String = ""
    Dim sDisplayAddNewTask As String = ""

    Dim sErrorString As String = ""

    If Session.Item("crmUserLogon") <> True Then

      Response.Redirect("Default.aspx", True)

    Else

      If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
        masterPage.Set_Active_Tab(5)
        Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Evolution Developer Center - Home")
      ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
        masterPage.Set_Active_Tab(6)
        Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Homebase Developer Center - Home")
      End If

      If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString), _
                                                              HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString, _
                                                              CLng(HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString), _
                                                              CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)) Then
        Response.Redirect("Default.aspx", True)
      End If

      If Not IsNothing(Request.Item("project")) Then
        If Not String.IsNullOrEmpty(Request.Item("project").ToString.Trim) Then
          sProjectKey = Request.Item("project").ToString.ToUpper.Trim
          bShowTaskDetails = True
        End If
      End If

      If Not IsNothing(Request.Item("priority")) Then
        If Not String.IsNullOrEmpty(Request.Item("priority").ToString.Trim) Then
          sProjectPriority = Request.Item("priority").ToString.ToUpper.Trim
          bShowTaskDetails = True
        End If
      End If

      If Not IsNothing(Request.Item("staffname")) Then
        If Not String.IsNullOrEmpty(Request.Item("staffname").ToString.Trim) Then
          sProjectStaffName = Request.Item("staffname").ToString.ToUpper.Trim
          bShowTaskDetails = True
        End If
      End If

      If Not IsNothing(Request.Item("status")) Then
        If Not String.IsNullOrEmpty(Request.Item("status").ToString.Trim) Then
          sProjectTaskStatus = Request.Item("status").ToString.ToUpper.Trim
          bShowTaskDetails = True
        End If
      End If

      If Not IsNothing(Request.Item("task")) Then
        If Not String.IsNullOrEmpty(Request.Item("task").ToString.Trim) Then
          sProjectTask = Request.Item("task").ToString.ToUpper.Trim

          If sProjectTask.ToLower.Contains("add") Then
            bAddNewTask = True
          End If

        End If
      End If

      developerCritera.DeveloperCriteriaProjectKey = CInt(sProjectKey)
      developerCritera.DeveloperCriteriaProjectTitle = ""
      developerCritera.DeveloperCriteriaProjectPriority = sProjectPriority
      developerCritera.DeveloperCriteriaProjectStaffName = sProjectStaffName
      developerCritera.DeveloperCriteriaProjectTask = sProjectTask
      developerCritera.DeveloperCriteriaProjectStatus = sProjectTaskStatus
      developerCritera.DeveloperCriteriaToggleDisplay = False

      localDatalayer.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      localDatalayer.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      localDatalayer.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
      localDatalayer.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
      localDatalayer.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

      'If HttpContext.Current.Session.Item("jetnetWebSiteType") = eWebSiteTypes.LOCAL Then
      localDatalayer.taskerConnectStr = crmWebHostClass.generateMYSQLConnectionString("www.aerowebtech.com", "jetnettasker", "aerowebtech", "moejive")
      'Else
      'localDatalayer.taskerConnectStr = crmWebHostClass.generateMYSQLConnectionString("172.20.100.2", "jetnettasker", "aerowebtech", "moejive")
      'End If

      If ((Not bShowTaskDetails) And (Not bAddNewTask)) Then

        TableCell_project_table.Visible = True

        localDatalayer.displayDevelopmentPriority(sDisplayDevelopmentProject)
        taskingByPriorityLbl.Text = sDisplayDevelopmentProject.Trim

        localDatalayer.displayDevelopmentStaff(sDisplayDevelopmentStaff)
        taskingByStaffLbl.Text = sDisplayDevelopmentStaff.Trim

        localDatalayer.displayDevelopmentSummary(sDisplayDevelopmentSummary)
        taskingSummaryLbl.Text = sDisplayDevelopmentSummary.Trim

        TableCell_add_task_table.Visible = False
        TableCell_details_table.Visible = False

        If IsPostBack And sProjectTask.ToLower.Contains("submit") Then

          If Not String.IsNullOrEmpty(projectKeyDDl.SelectedValue.Trim) Then
            developerCritera.DeveloperCriteriaProjectKey = CInt(projectKeyDDl.SelectedValue)
          End If

          developerCritera.DeveloperCriteriaProjectTitle = ""

          If Not String.IsNullOrEmpty(projectPriorityDDL.SelectedValue.Trim) Then
            developerCritera.DeveloperCriteriaProjectPriority = projectPriorityDDL.SelectedValue
          End If

          developerCritera.DeveloperCriteriaProjectEntryStaffName = staff_entry_name.Value
          developerCritera.DeveloperCriteriaProjectStaffName = staff_name.Value
          developerCritera.DeveloperCriteriaProjectStatus = task_status.Value
          developerCritera.DeveloperCriteriaProjectTaskTitle = task_title.Text
          developerCritera.DeveloperCriteriaProjectTaskDiscription = task_description.Text
          developerCritera.DeveloperCriteriaProjectFollowUp = task_follow_up.Text.Trim
          developerCritera.DeveloperCriteriaToggleDisplay = False

          localDatalayer.insertAddNewTask(developerCritera)

          addNewTaskLbl.Visible = True
        End If

      End If

      If bShowTaskDetails Then

        TableCell_details_table.Visible = True

        localDatalayer.displayTaskDetails(developerCritera, sDisplayTaskDetails)

        taskingDetailsLbl.Text = sDisplayTaskDetails.Trim

        TableCell_add_task_table.Visible = False
        TableCell_project_table.Visible = False

      End If

      If bAddNewTask Then

        localDatalayer.fill_project_key_dropdown(developerCritera, 0, projectKeyDDl)
        localDatalayer.fill_project_priority_dropdown(developerCritera, 0, projectPriorityDDL)

        TableCell_add_task_table.Visible = True '

        TableCell_details_table.Visible = False
        TableCell_project_table.Visible = False

      End If

    End If

  End Sub

End Class