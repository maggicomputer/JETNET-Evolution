' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/chat/ChatTestPopUp.aspx.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:44a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: ChatTestPopUp.aspx.vb $
'
' ********************************************************************************

Partial Public Class ChatTestPopUp
  Inherits System.Web.UI.Page

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Try
    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Page_Load : ChatTestPopUp.aspx</b><br />" + ex.Message
    End Try

  End Sub

End Class