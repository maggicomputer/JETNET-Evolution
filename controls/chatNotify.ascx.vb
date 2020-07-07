' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /CRM Evolution/controls/chatNotify.ascx.vb $
'$$Author: Mike $
'$$Date: 3/17/15 12:31p $
'$$Modtime: 3/17/15 12:30p $
'$$Revision: 13 $
'$$Workfile: chatNotify.ascx.vb $
'
' ********************************************************************************

Partial Public Class chatNotify
  Inherits System.Web.UI.UserControl

  Public bEnableChat As Boolean = False
  Public fullHostname As String = ""
  Public txtAlias As String = ""
  Public txtAliasID As Long = 0
  Public userSessionGUID As String = ""
  Public bChatChangeSub As Boolean = False

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    ChatManager.CheckAndInitChat(False, bEnableChat, txtAlias, txtAliasID)
    fullHostname = HttpContext.Current.Session.Item("jetnetFullHostName")
    userSessionGUID = HttpContext.Current.Session.Item("localPreferences").SessionGUID.ToString.Trim

    If Not bEnableChat Then
      txtAlias = ChatManager.getSubscriberContactAlias()
      bChatChangeSub = ChatManager.checkForOtherChatSubscriptions(txtAlias, 0, "", 0)
    End If

  End Sub

End Class