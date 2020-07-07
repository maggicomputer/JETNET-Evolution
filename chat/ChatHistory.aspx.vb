' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /CRM Evolution/chat/ChatHistory.aspx.vb $
'$$Author: Mike $
'$$Date: 3/17/15 12:31p $
'$$Modtime: 3/17/15 12:30p $
'$$Revision: 13 $
'$$Workfile: ChatHistory.aspx.vb $
'
' ********************************************************************************

Partial Public Class ChatHistory

  Inherits System.Web.UI.Page

  Public TalkerID As String = ""
  Public TalkerID1 As String = ""

  Public bEnableChat As Boolean
  Private roomID As Guid

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    ChatManager.CheckAndInitChat(False, bEnableChat)

    If Not IsNothing(Request.Item("rid")) Then
      If Not String.IsNullOrEmpty(Request.Item("rid").ToString.Trim) Then
        roomID = New Guid(Request.Item("rid").ToString)
      End If
    End If

    Dim notifyInfo = ChatManager.GetNotificationInfo(roomID)

    If Not IsNothing(notifyInfo) Then
      TalkerID = notifyInfo.FromUserUID.ToString
      TalkerID1 = notifyInfo.ToUserUID.ToString
    End If

  End Sub

End Class