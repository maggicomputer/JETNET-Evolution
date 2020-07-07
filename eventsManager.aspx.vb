' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/eventsManager.aspx.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:38a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: eventsManager.aspx.vb $
'
' ********************************************************************************

Partial Public Class eventsManager
  Inherits System.Web.UI.Page

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Dim sErrorString As String = ""

    If Session.Item("crmUserLogon") <> True Then

      Response.Redirect("Default.aspx", False)

    Else

      If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(Session.Item("localUser").crmSubSubID.ToString), Session.Item("localUser").crmUserLogin.ToString, CLng(Session.Item("localUser").crmSubSeqNo.ToString), CLng(Session.Item("localUser").crmUserContactID.ToString)) Then
        Response.Write("error in load event manager : " + sErrorString)
      End If

      Dim fSubins_platform_os As String = commonEvo.getBrowserCapabilities(Request.Browser)

      Master.SetPageTitle("JETNET - Event Manager")  ' sets the page title and page.text

    End If

  End Sub

End Class