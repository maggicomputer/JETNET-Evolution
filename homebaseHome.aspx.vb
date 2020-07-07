' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /CRM Evolution/homebaseHome.aspx.vb $
'$$Author: Mike $
'$$Date: 3/29/16 11:28a $
'$$Modtime: 3/29/16 11:28a $
'$$Revision: 1 $
'$$Workfile: homebaseHome.aspx.vb $
'
' ********************************************************************************

Partial Public Class homebaseHome
  Inherits System.Web.UI.Page

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Dim sErrorString As String = ""

    If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString), _
                                                      HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString, _
                                                      CLng(HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString), _
                                                      CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)) Then
      Response.Redirect("Default.aspx", True)

    End If

    Master.Set_Active_Tab(0)

    Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("JETNET Homebase - Home")

  End Sub

End Class