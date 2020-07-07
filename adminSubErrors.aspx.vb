' ********************************************************************************
' Copyright 2004-19. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/adminSubErrors.aspx.vb $
'$$Author: Matt $
'$$Date: 6/10/20 2:47p $
'$$Modtime: 6/10/20 1:17p $
'$$Revision: 7 $
'$$Workfile: adminSubErrors.aspx.vb $
'
' ********************************************************************************

Partial Public Class adminSubErrors
  Inherits System.Web.UI.Page

  Protected localDatalayer As New admin_center_dataLayer

  Public Shared masterPage As New Object

  Private Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

    Try
      If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
        Me.MasterPageFile = "~/EvoStyles/EmptyCustomerAdminTheme.master"
        masterPage = DirectCast(Page.Master, EmptyCustomerAdminTheme)
      ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
        Me.MasterPageFile = "~/EvoStyles/EmptyHomebaseTheme.Master"
        masterPage = DirectCast(Page.Master, EmptyHomebaseTheme)
      End If

      masterPage.SetContainerClass("")

    Catch ex As Exception
      If Not IsNothing(masterPage) Then
        masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString + " : " + ex.Message.ToString.Trim)
      Else
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodInfo.GetCurrentMethod().ToString + "</b><br />" + ex.Message.ToString.Trim
      End If
    End Try

  End Sub

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    If Session.Item("crmUserLogon") <> True Then

      Response.Redirect("Default.aspx", True)

    Else

      Dim sErrorString As String = ""

      If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString),
                                                              HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString,
                                                              CLng(HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString),
                                                              CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)) Then
        Response.Redirect("Default.aspx", True)
      End If


      localDatalayer.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      localDatalayer.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
      localDatalayer.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
      localDatalayer.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
      localDatalayer.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

      localDatalayer.crmMasterConnectStr = HttpContext.Current.Application.Item("crmMasterDatabase").ToString.Trim

      Dim userEmailString As String = ""
      Dim user_sub_id As Long = 0
      Dim user_login As String = ""
      Dim records As Integer = 0

      If Not IsNothing(Request.Item("email")) Then
        If Not String.IsNullOrEmpty(Request.Item("email").ToString.Trim) Then
          userEmailString = Request.Item("email").ToString.ToUpper.Trim
        End If
      End If

      If Not IsNothing(Request.Item("sub_id")) Then
        If Not String.IsNullOrEmpty(Request.Item("sub_id").ToString.Trim) Then
          user_sub_id = Request.Item("sub_id").ToString.ToUpper.Trim
        End If
      End If

      If Not IsNothing(Request.Item("login")) Then
        If Not String.IsNullOrEmpty(Request.Item("login").ToString.Trim) Then
          user_login = Request.Item("login").ToString.ToUpper.Trim
        End If
      End If

      If Not IsNothing(Request.Item("records")) Then
        If Not String.IsNullOrEmpty(Request.Item("records").ToString.Trim) Then
          records = Request.Item("records").ToString.ToUpper.Trim
        End If
      End If

            Dim sDisplaySubscriberErrorList As String = ""

            If Trim(sum_by.SelectedValue) <> "" Then
                localDatalayer.displaySubscriberErrorList(userEmailString, sDisplaySubscriberErrorList, user_sub_id, user_login, records, sum_by.SelectedValue, days_drop.SelectedValue)
            Else
                localDatalayer.displaySubscriberErrorList(userEmailString, sDisplaySubscriberErrorList, user_sub_id, user_login, records, "", days_drop.SelectedValue)
            End If


            subscriber_data_list_display.Text = sDisplaySubscriberErrorList.Trim

      masterPage.SetPageTitle("Subscriber Activity List - " + userEmailString.ToLower.Trim)  ' sets the page title and page.text

    End If

  End Sub

End Class