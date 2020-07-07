
' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/homebaseCompanyProspect.aspx.vb $
'$$Author: Mike $
'$$Date: 8/15/19 12:43p $
'$$Modtime: 8/15/19 12:43p $
'$$Revision: 2 $
'$$Workfile: homebaseCompanyProspect.aspx.vb $
'
' ********************************************************************************

Partial Public Class homebaseCompanyProspect
  Inherits System.Web.UI.Page

  Public Shared masterPage As New Object

  Private Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

    Try
      If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
        Me.MasterPageFile = "~/EvoStyles/EmptyCustomerAdminTheme.master"
        masterPage = DirectCast(Page.Master, EmptyCustomerAdminTheme)
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

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

  End Sub

End Class