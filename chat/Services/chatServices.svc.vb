' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/chat/Services/chatServices.svc.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:45a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: chatServices.svc.vb $
'
' ********************************************************************************

Imports System.ServiceModel
Imports System.ServiceModel.Activation
Imports System.ServiceModel.Web

<ServiceContract([Namespace]:="http://crmWebClient", SessionMode:=SessionMode.Allowed)> _
<AspNetCompatibilityRequirements(RequirementsMode:=AspNetCompatibilityRequirementsMode.Allowed)> _
Public Class chatServices

  <OperationContract()> _
  Public Sub CreateChatRoom(ByVal useralias As String, ByVal roomName As String, ByVal password As String, ByVal maxUser As Integer, ByVal needPassword As Boolean)

    If maxUser < 2 Then
      maxUser = 2
    End If

    Dim roomid As Guid = ChatManager.CreateChatRoom(roomName, password, False, maxUser, needPassword)

  End Sub

  <OperationContract()> _
  Public Function newChatRoom(ByVal roomName As String, ByVal password As String, ByVal maxUser As Integer, ByVal needPassword As Boolean)

    If maxUser < 2 Then
      maxUser = 2
    End If

    Dim roomID As Guid = Nothing

    roomID = ChatManager.CreateChatRoom(roomName, password, False, maxUser, needPassword)

    Return roomID

  End Function

  <OperationContract()> _
  Public Function JoinChatRoom(ByVal roomid As String) As ChatRoom
    Dim rid As Guid
    'If Guid.TryParse(roomid, rid) Then
    If MyTryParse(roomid, rid) Then
      ChatManager.JoinChatRoom(rid, HttpContext.Current)

      Return New ChatRoom(rid)
    Else
      Return Nothing
    End If

  End Function

  <OperationContract()> _
  Public Function ChangeChatSession(ByVal sessionGUID As String, ByVal [alias] As String, ByVal bEnable As Boolean, ByVal bChangeSub As Boolean) As Boolean

    Try

      Return ChatManager.UpdateChatStatus(sessionGUID, [alias], bEnable, bChangeSub)

    Catch
      Return False
    End Try

  End Function

  <OperationContract()> _
  Public Function UpdateChatSession() As Boolean

    Try

      ' update this users session info if there is a session in the table
      Return ChatManager.UpdateSession(HttpContext.Current)

    Catch
      Return False
    End Try

  End Function

  <OperationContract()> _
  Public Function NotifyChatUser(ByVal [alias] As String, ByVal AliasID As Integer, ByVal roomID As String) As String

    Dim rid As Guid

    Try

      If MyTryParse(roomID, rid) Then
        If ChatManager.NotifyChatUser(HttpContext.Current, [alias], AliasID, rid) Then
          Return roomID
        Else

          ' might have to 
          Dim oldNotification = ChatManager.GetPreviousChatRoom(HttpContext.Current, [alias], AliasID)

          If oldNotification IsNot Nothing Then
            Return oldNotification.ChatRoomID.ToString
          End If

          Return roomID

        End If
      Else
        Return ""
      End If

    Catch
      Return ""
    End Try

  End Function

  <OperationContract()> _
  Public Function InChatWithUser(ByVal [alias] As String, ByVal AliasID As Integer) As String

    Dim bReturnValue As String = ""

    Dim bSessionTalker As Boolean = False
    Dim bAliasTalker As Boolean = False

    Try
      ' get the current session
      Dim session As tblSession = ChatManager.GetMySession(HttpContext.Current)

      ' first check if I started a previous chat
      Dim oldNotification = ChatManager.GetMyPreviousNotification(session.UserAlias, session.UID, [alias], AliasID)

      If oldNotification IsNot Nothing Then

        bReturnValue = oldNotification.ChatRoomID.ToString

      Else

        Dim list As List(Of tblChatRoom) = ChatManager.GetChatRoomList()
        Dim roomList As New List(Of ChatRoom)()

        For Each room As tblChatRoom In list
          roomList.Add(New ChatRoom(room.ChatRoomID))
        Next

        ' lets get a list of chat rooms
        If roomList IsNot Nothing Then

          For Each room As ChatRoom In roomList

            ' check each room and get each talker from the room 

            Dim talkerList As List(Of tblTalker) = ChatManager.GetRoomTalkerList(room.RoomID)
            Dim result As New List(Of RoomTalker)()

            If talkerList IsNot Nothing Then
              If talkerList.Count > 0 Then

                For Each talker As tblTalker In talkerList
                  result.Add(New RoomTalker(talker, HttpContext.Current))
                Next

              End If
            End If

            If result.Count > 0 Then
              For Each rt As RoomTalker In result

                ' see if I am a "talker" in this room
                If rt.TalkerSessionUID = session.UID And rt.TalkerAlias.ToLower = session.UserAlias Then
                  bSessionTalker = True
                End If

                ' see if the alias is a "talker" in this room
                If rt.TalkerSessionUID = AliasID And rt.TalkerAlias.ToLower = [alias].ToLower Then
                  bAliasTalker = True
                End If

              Next
            End If

            'if I am a talker in this room and the "alias" is also a talker in this room
            ' all ready in a chat with this alias so quit checking
            If bSessionTalker And bAliasTalker Then
              Exit For
            End If

            ' reset flags to check next room
            bSessionTalker = False
            bAliasTalker = False

          Next

        End If

      End If

    Catch
      bReturnValue = ""
    End Try

    Return bReturnValue

  End Function

  <OperationContract()> _
  Public Function HasChatNotifications() As NotifyUser

    ' get the current session
    Dim session As tblSession = ChatManager.GetMySession(HttpContext.Current)

    Dim localNotifyUser As tblNotify = Nothing

    If Not session Is Nothing Then

      localNotifyUser = ChatManager.UserHasNotifications(session.UserAlias, session.UID)

    End If

    Return New NotifyUser(localNotifyUser)

  End Function

  <OperationContract()> _
  Public Function GetBackNotifications(ByVal [alias] As String, ByVal AliasID As Integer) As NotifyUser

    ' get the current session
    Dim session As tblSession = ChatManager.GetMySession(HttpContext.Current)

    Dim localNotifyUser As tblNotify = Nothing

    If Not session Is Nothing Then

      localNotifyUser = ChatManager.UserHasBackNotification([alias], AliasID, session.UserAlias, session.UID)

    End If

    Return New NotifyUser(localNotifyUser)

  End Function

  <OperationContract()> _
  Public Function GetUserSessions() As List(Of chatSession)

    Dim result As New List(Of chatSession)()

    Dim userList As List(Of tblSession) = ChatManager.CurrentUserSessions(HttpContext.Current)

    For Each session As tblSession In userList
      result.Add(New chatSession(session))
    Next

    Return result

  End Function

  <OperationContract()> _
  Public Function GetCommunityList() As List(Of communityUser)

    Dim result As New List(Of communityUser)()

    Dim userList As List(Of tblCommunityList) = ChatManager.AllCommunityListUsers(HttpContext.Current)

    For Each communityListUser As tblCommunityList In userList
      result.Add(New communityUser(communityListUser))
    Next

    Return result

  End Function

  <OperationContract()> _
  Public Function DeleteCommunityListUser(ByVal [alias] As String, ByVal AliasID As Integer) As Boolean

    Try
      Return ChatManager.DeleteUserFromCommunityList(HttpContext.Current, [alias], AliasID)
    Catch
      Return False
    End Try

  End Function

  <OperationContract()> _
  Public Function AddUserCommunityList(ByVal [alias] As String, ByVal AliasID As Integer) As Boolean

    Try
      Return ChatManager.AddUserToCommunityList(HttpContext.Current, [alias], AliasID)
    Catch
      Return False
    End Try

  End Function

  <OperationContract()> _
  Public Function IsUserOnCommunityList(ByVal [alias] As String, ByVal AliasID As Integer) As Boolean

    Try
      Return ChatManager.IsUserOnCommunityList(HttpContext.Current, [alias], AliasID)
    Catch
      Return False
    End Try

  End Function

  <OperationContract()> _
Public Function DeleteChatUserAlias(ByVal AliasID As Integer) As Boolean

    Try
      Return ChatManager.DeleteAliasFromCurrentChat(HttpContext.Current, AliasID)
    Catch
      Return False
    End Try

  End Function

  <OperationContract()> _
  Public Function AddChatUserAlias(ByVal AliasID As Integer) As Boolean

    Try
      Return ChatManager.AddAliasToCurrentChat(HttpContext.Current, AliasID)
    Catch
      Return False
    End Try

  End Function

  <OperationContract()> _
  Public Function IsUserInCurrentChat(ByVal AliasID As Integer) As Boolean

    Try
      Return ChatManager.IsUserInCurrentChat(HttpContext.Current, AliasID)
    Catch
      Return False
    End Try

  End Function

  <OperationContract()> _
  Public Function UpdateNotificationStatus(ByVal notifyID As Integer, ByVal bAccepted As Boolean, ByVal roomid As String) As Boolean

    Dim rid As Guid

    Try
      If MyTryParse(roomid, rid) Then
        Return ChatManager.SetNotificationStatus(notifyID, bAccepted, rid)
      Else
        Return False
      End If

    Catch
      Return False
    End Try

  End Function

  <OperationContract()> _
  Public Sub DeleteUserNotification(ByVal roomid As String, ByVal bAccepted As Boolean)

    Dim rid As Guid

    Try
      'If Guid.TryParse(roomid, rid) Then    
      If MyTryParse(roomid, rid) Then
        ChatManager.DeleteUserNotifications(rid, bAccepted)
      Else
        Return
      End If

    Catch
      Return
    End Try

  End Sub

  <OperationContract()> _
  Public Sub LeaveChatRoom(ByVal roomid As String)

    If roomid Is Nothing Then
      roomid = GetGUIDFromQuery(HttpContext.Current.Request.UrlReferrer.Query).ToString()
    End If

    Dim rid As Guid

    'If Guid.TryParse(roomid, rid) Then    
    If MyTryParse(roomid, rid) Then

      ChatManager.LeaveChatRoom(rid, HttpContext.Current)

    Else
      Return
    End If

  End Sub

  <OperationContract()> _
  Public Sub DeleteAllSessions()
    ChatManager.DeleteAllSessions(HttpContext.Current)
  End Sub

  <OperationContract()> _
  Public Sub LogSessionOff()
    ChatManager.LogMySessionOff(HttpContext.Current)
  End Sub

  <OperationContract()> _
  Public Sub DeleteChatRoom(ByVal roomID As String)

    If roomID Is Nothing Then
      roomID = GetGUIDFromQuery(HttpContext.Current.Request.UrlReferrer.Query).ToString()
    End If

    Dim rid As Guid
    'If Guid.TryParse(roomID, rim) Then
    If MyTryParse(roomID, rid) Then
      ChatManager.DeleteChatRoom(rid)
    End If

  End Sub

  <OperationContract()> _
  Public Function GetChatRoomList() As List(Of ChatRoom)

    Dim list As List(Of tblChatRoom) = ChatManager.GetChatRoomList()
    Dim result As New List(Of ChatRoom)()
    For Each room As tblChatRoom In list
      result.Add(New ChatRoom(room.ChatRoomID))
    Next
    Return result

  End Function

  <OperationContract()> _
  Public Function GetChatRoomInfo(ByVal roomID As String) As ChatRoom

    Dim rim As Guid

    'If Guid.TryParse(roomID, rim) Then
    If MyTryParse(roomID, rim) Then
      Return New ChatRoom(rim)
    Else
      Return Nothing
    End If

  End Function

  <OperationContract()> _
  Public Function IsRoomTalkerAvailable(ByVal roomID As String) As Boolean

    ' returns true if "a talker" with a session.UID that doesn't match current session
    ' is in the current chat room
    Dim result As New List(Of RoomTalker)()

    Dim rim As Guid

    ' get the current session
    Dim session As tblSession = ChatManager.GetMySession(HttpContext.Current)

    If Not IsNothing(session) Then

      If MyTryParse(roomID, rim) Then

        Dim talkerList As List(Of tblTalker) = ChatManager.GetRoomTalkerList(rim)

        For Each talker As tblTalker In talkerList
          If talker.tblSession.UID <> session.UID Then
            result.Add(New RoomTalker(talker, HttpContext.Current))
          End If

        Next

      End If

    End If

    Return result.Count > 0

  End Function

  <OperationContract()> _
  Public Function GetRoomTalkerList() As List(Of RoomTalker)

    Dim result As New List(Of RoomTalker)()

    Dim roomid As Guid = GetGUIDFromQuery(HttpContext.Current.Request.UrlReferrer.Query)

    If roomid <> Guid.Empty Then
      Dim talkerList As List(Of tblTalker) = ChatManager.GetRoomTalkerList(roomid)
      For Each talker As tblTalker In talkerList
        result.Add(New RoomTalker(talker, HttpContext.Current))
      Next
    End If

    Return result

  End Function

  <OperationContract()> _
  Public Function SendMessage(ByVal message As String) As Boolean

    Dim roomid As Guid = GetGUIDFromQuery(HttpContext.Current.Request.UrlReferrer.Query)

    If roomid <> Guid.Empty Then
      Dim talker As tblTalker = ChatManager.FindTalker(roomid, HttpContext.Current)
      ChatManager.SendMessage(talker, message)
      Return True
    Else

      Return False
    End If
  End Function

  <OperationContract()> _
  Public Function RecieveMessage() As List(Of Message)

    Dim result As New List(Of Message)()
    Dim roomid As Guid = GetGUIDFromQuery(HttpContext.Current.Request.UrlReferrer.Query)

    If roomid <> Guid.Empty Then
      Dim messageList As List(Of tblMessagePool) = ChatManager.RecieveMessage(ChatManager.GetChatRoom(roomid))

      If Not IsNothing(messageList) Then
        For Each msg As tblMessagePool In messageList
          result.Add(New Message(msg, HttpContext.Current))
        Next
      End If
    End If

    Return result

  End Function

  <OperationContract()> _
  Public Function GetHistoricalMessages(ByVal talkerID As Integer, ByVal talkerID1 As Integer) As List(Of msgHistory)

    Dim result As New List(Of msgHistory)()

    Dim historyList As List(Of tblMessageHistory) = ChatManager.GetHistoricalMessages(talkerID, talkerID1)

    For Each history As tblMessageHistory In historyList
      result.Add(New msgHistory(history, HttpContext.Current))
    Next

    Return result

  End Function

  Private Function GetGUIDFromQuery(ByVal query As String) As Guid
    Dim rim As Guid
    If String.IsNullOrEmpty(query) Then
      Return Guid.Empty
    End If

    Dim reg As New Regex("rid=([0-9a-z]{8}-[0-9a-z]{4}-[0-9a-z]{4}-[0-9a-z]{4}-[0-9a-z]{12})")

    Dim gid As String = reg.Match(query).Groups(1).Value
    'If Guid.TryParse(gid, rim) Then
    If MyTryParse(gid, rim) Then
      Return rim
    Else
      Return Guid.Empty
    End If

  End Function

  Private Function MyTryParse(ByVal inGuid As String, ByRef outGuid As Guid) As Boolean

    If String.IsNullOrEmpty(inGuid) Then
      outGuid = Guid.Empty
      Return False
    End If

    Try
      outGuid = New Guid(inGuid)
      Return True
    Catch ex As Exception
      outGuid = Guid.Empty
      Return False
    End Try

  End Function

End Class
