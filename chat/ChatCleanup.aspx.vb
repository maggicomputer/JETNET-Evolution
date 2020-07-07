' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/chat/ChatCleanup.aspx.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:44a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: ChatCleanup.aspx.vb $
'
' ********************************************************************************

Partial Public Class ChatCleanup
  Inherits System.Web.UI.Page

  Private roomID As Guid
  Private aliasID As Integer

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Try
      If Not IsNothing(Request.Item("rid")) Then
        If Not String.IsNullOrEmpty(Request.Item("rid").ToString.Trim) Then
          roomID = New Guid(Request.Item("rid").ToString)
        End If
      End If

      If Not IsNothing(Request.Item("aid")) Then
        If Not String.IsNullOrEmpty(Request.Item("aid").ToString.Trim) Then
          If IsNumeric(Request.Item("aid").ToString.Trim) Then
            aliasID = CInt(Request.Item("aid").ToString.Trim)
          End If
        End If
      End If

      If ChatManager.DeleteAliasFromCurrentChat(HttpContext.Current, aliasID) Then
        ChatManager.LeaveChatRoom(roomID, HttpContext.Current)
      End If

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Page_Load : ChatCleanup.aspx</b><br />" + ex.Message
    End Try

  End Sub

End Class