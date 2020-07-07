' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/chat/Logic/ChatManager.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:44a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: ChatManager.vb $
'
' ********************************************************************************

Public Class ChatManager

#Region "Message Management"

  Public Shared Function SendMessage(ByVal talker As tblTalker, ByVal message As String) As Boolean
    Try

      Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)

      Dim msgpool As New tblMessagePool()

      msgpool.message = message
      msgpool.SendTime = DateTime.Now
      msgpool.talkerID = talker.TalkerID

      db.tblMessagePools.InsertOnSubmit(msgpool)

      ' at the same time "log" the message to the "message history table"
      Dim msgHistory As New tblMessageHistory()

      msgHistory.messageBody = message
      msgHistory.messageDate = DateTime.Now
      msgHistory.talkerID = talker.tblSession.UID

      ' find the "other" talker  (will only work for "2" talkers per room )
      Dim otherTalker As NotifyUser = ChatManager.GetNotificationInfo(talker.ChatRoomID)

      If talker.tblSession.UID = otherTalker.ToUserUID Then
        msgHistory.talker1ID = otherTalker.FromUserUID
      ElseIf talker.tblSession.UID = otherTalker.FromUserUID Then
        msgHistory.talker1ID = otherTalker.ToUserUID
      End If

      msgHistory.talkerDeleted = -1

      db.tblMessageHistories.InsertOnSubmit(msgHistory)

      db.SubmitChanges()

      Return True
    Catch
      Return False
    End Try
  End Function

  Public Shared Function RecieveMessage(ByVal room As tblChatRoom) As List(Of tblMessagePool)

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)

    If Not IsNothing(room) Then
      If db.tblMessagePools.Count(Function(msg) room.tblTalkers.Contains(msg.tblTalker)) > 0 Then
        Return (From messages In db.tblMessagePools Where messages.tblTalker.ChatRoomID = room.ChatRoomID).ToList()
      Else
        Return Nothing
      End If
    Else
      Return Nothing
    End If

  End Function

  Private Shared Sub TryToDeleteChatMessageList(ByVal roomid As Guid)

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)
    Dim chatroom = GetChatRoom(roomid)
    If Not IsNothing(chatroom) Then
      If (From t In chatroom.tblTalkers Where t.CheckOutTime Is Nothing Select t).Count() = 0 Then
        Dim list = From m In db.tblMessagePools Where m.tblTalker.ChatRoomID = roomid
        db.tblMessagePools.DeleteAllOnSubmit(list)
        db.SubmitChanges()
      End If
    End If

  End Sub

#End Region

#Region "Historical Message Management"

  Public Shared Function GetHistoricalMessages(ByVal talkerID As Integer, ByVal talkerID1 As Integer) As List(Of tblMessageHistory)

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)

    Dim msgHistory = From h In db.tblMessageHistories Where (h.talkerID = talkerID Or h.talkerID = talkerID1) AndAlso (h.talker1ID = talkerID Or h.talker1ID = talkerID1) Order By h.messageDate Ascending
    Return msgHistory.ToList()

  End Function

#End Region

#Region "Notification Management"

  Public Shared Function NotifyChatUser(ByVal context As HttpContext, ByVal [alias] As String, ByVal AliasID As Integer, ByVal ChatRoomID As Guid) As Boolean

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)
    Dim notify As New tblNotify()

    Try

      ' get current session from db
      Dim session = GetMySession(context)

      ' before we "notify" user of a new chat check to see if "i just dropped my end of the chat"
      ' there should still be a "notification" in the "table"

      Dim oldNotification = GetMyPreviousNotification(session.UserAlias, session.UID, [alias], AliasID)

      If oldNotification IsNot Nothing Then
        ' notification is still in table the "talker I was just chatting with" should still be in the "room"

        ' see if the "talker I was just chatting with" is still in the "previous room"
        Dim talkerList As List(Of tblTalker) = GetRoomTalkerList(oldNotification.ChatRoomID)
        Dim result As New List(Of RoomTalker)()

        If talkerList IsNot Nothing Then
          If talkerList.Count > 0 Then

            For Each talker As tblTalker In talkerList
              result.Add(New RoomTalker(talker, context))
            Next

          End If
        End If

        ' ok should have a "list" of room talkers lets see if they match the "toUserUID" and "toUserAlias"
        If result.Count > 0 Then
          For Each rt As RoomTalker In result
            If rt.TalkerSessionUID = oldNotification.toUserUID And rt.TalkerAlias = oldNotification.toUserAlias Then
              ' the "talker I was just chatting with" is still in the room just re-join chat room
              ' set accepted flag to "A" so notification stays around for the "chat header block"

              Dim notifyAccept As tblNotify = db.tblNotifies.SingleOrDefault(Function(n) n.ChatRoomID = oldNotification.ChatRoomID)

              If Not IsNothing(notifyAccept) Then
                notifyAccept.notifyStatus = "A"
                db.SubmitChanges()
              End If

              Return False

            End If
          Next
        End If

      Else

        ' lets see if i was all ready in a chat with this [alias] if i was ... just recconnect to previous room
        ' if user is still in the room
        Dim stillHasSession = GetUserSession(context, [alias], AliasID)
        Dim previousNotification = GetOtherPreviousNotification(stillHasSession.UserAlias, stillHasSession.UID, session.UserAlias, session.UID)

        If previousNotification IsNot Nothing Then
          ' notification is still in table the "talker I was just chatting with" should still be in the "room"

          ' see if the [alias] is the "talker I was just chatting with" is still in the "previous room"
          Dim talkerList As List(Of tblTalker) = GetRoomTalkerList(previousNotification.ChatRoomID)
          Dim result As New List(Of RoomTalker)()

          If talkerList IsNot Nothing Then
            If talkerList.Count > 0 Then

              For Each talker As tblTalker In talkerList
                result.Add(New RoomTalker(talker, context))
              Next

            End If
          End If

          ' ok should have a "list" of room talkers lets see if they match the "fromUserUID" and "fromUserAlias"
          If result.Count > 0 Then
            For Each rt As RoomTalker In result
              If rt.TalkerSessionUID = previousNotification.fromUserUID And rt.TalkerAlias = previousNotification.fromUserAlias Then
                ' the "talker I was just chatting with" is still in the room just re-join chat room
                ' set accepted flag to "A" so notification stays around for the "chat header block"

                Dim notifyAccept As tblNotify = db.tblNotifies.SingleOrDefault(Function(n) n.ChatRoomID = previousNotification.ChatRoomID)

                If Not IsNothing(notifyAccept) Then
                  notifyAccept.notifyStatus = "A"
                  db.SubmitChanges()
                End If

                Return False

              End If

            Next
          End If

        End If

      End If

      Dim hasSession = GetUserSession(context, [alias], AliasID)

      ' check to see if this user has a session in the db 
      If hasSession IsNot Nothing Then

        notify.ChatRoomID = ChatRoomID

        notify.toUserUID = hasSession.UID
        notify.toUserAlias = hasSession.UserAlias

        notify.fromUserAlias = session.UserAlias
        notify.fromUserUID = session.UID
        notify.notifyStatus = "N"

        notify.notifiedTime = Now()

        db.tblNotifies.InsertOnSubmit(notify)
        db.SubmitChanges()

      End If

      Return True

    Catch ex As Exception

      Return False

    End Try

  End Function

  Public Shared Function UserHasNotifications(ByVal [alias] As String, ByVal AliasID As Integer) As tblNotify

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)

    Dim notifyUser As tblNotify = Nothing

    Try

      notifyUser = db.tblNotifies.SingleOrDefault(Function(n) n.toUserUID = AliasID AndAlso n.toUserAlias.ToLower.Trim = [alias].ToLower.Trim AndAlso n.notifyStatus = "N")

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>UserHasNotifications(ByVal [alias] As String, ByVal AliasID As Integer) As tblNotify</b><br />" + ex.Message

    End Try

    Return notifyUser

  End Function

  Public Shared Function UserHasBackNotification(ByVal [alias] As String, ByVal AliasID As Integer, ByVal myAlias As String, ByVal myAliasID As Integer) As tblNotify

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)

    Dim notifyUser As tblNotify = Nothing

    Try

      notifyUser = db.tblNotifies.SingleOrDefault(Function(n) n.toUserUID = AliasID AndAlso n.toUserAlias.ToLower.Trim = [alias].ToLower.Trim AndAlso n.fromUserUID = myAliasID AndAlso n.fromUserAlias.ToLower.Trim = myAlias.ToLower.Trim AndAlso (n.notifyStatus = "U" Or n.notifyStatus = "Y"))

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>UserHasBackNotification(ByVal [alias] As String, ByVal AliasID As Integer) As tblNotify</b><br />" + ex.Message

    End Try

    Return notifyUser

  End Function

  ' used to get the "chatFrom" and "chatWith" info from notification for display on "chat window"
  Public Shared Function GetNotificationInfo(ByVal ChatRoomID As Guid) As NotifyUser

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)

    Dim notifyUser As tblNotify = Nothing

    Try

      notifyUser = db.tblNotifies.SingleOrDefault(Function(n) n.ChatRoomID = ChatRoomID)

      If notifyUser IsNot Nothing Then
        Return New NotifyUser(notifyUser)
      End If

    Catch ex As Exception

    End Try

    Return Nothing

  End Function

  ' used to get the "previous notification" by sessionUID
  Public Shared Function GetMyPreviousNotification(ByVal myAlias As String, ByVal mySessionUID As Integer, ByVal otherAlias As String, ByVal otherSessionUID As Integer) As tblNotify

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)

    Dim notifyUser As tblNotify = Nothing

    Try

      notifyUser = db.tblNotifies.SingleOrDefault(Function(n) n.fromUserUID = mySessionUID AndAlso n.fromUserAlias.Trim.ToLower = myAlias.Trim.ToLower AndAlso n.toUserUID = otherSessionUID AndAlso n.toUserAlias.Trim.ToLower = otherAlias.Trim.ToLower)

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>GetMyPreviousNotification(ByVal myAlias As String, ByVal mySessionUID As Integer, ByVal otherAlias As String, ByVal otherSessionUID As Integer) As tblNotify</b><br />" + ex.Message

    End Try

    Return notifyUser

  End Function

  Public Shared Function GetOtherPreviousNotification(ByVal otherAlias As String, ByVal otherSessionUID As Integer, ByVal myAlias As String, ByVal mySessionUID As Integer) As tblNotify

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)

    Dim notifyUser As tblNotify = Nothing

    Try

      notifyUser = db.tblNotifies.SingleOrDefault(Function(n) n.fromUserUID = otherSessionUID AndAlso n.fromUserAlias.Trim.ToLower = otherAlias.Trim.ToLower AndAlso n.toUserUID = mySessionUID AndAlso n.toUserAlias.Trim.ToLower = myAlias.Trim.ToLower)

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>GetOtherPreviousNotification(ByVal otherAlias As String, ByVal otherSessionUID As Integer, ByVal myAlias As String, ByVal mySessionUID As Integer) As tblNotify</b><br />" + ex.Message

    End Try

    Return notifyUser

  End Function

  ' used to get the "old Notifications" by [alias] and AliasID
  Public Shared Function GetOldNotifications(ByVal [alias] As String, ByVal AliasID As Integer) As List(Of tblNotify)

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)

    Dim rsl = From n In db.tblNotifies Where (n.fromUserUID = AliasID AndAlso n.fromUserAlias.ToLower.Trim = [alias].ToLower.Trim AndAlso ((n.tblSession.onLineTime.Date = DateTime.Now.AddMinutes(-20).Date) And (n.tblSession.onLineTime.TimeOfDay < DateTime.Now.AddMinutes(-20).TimeOfDay)))
    Return rsl.ToList()

  End Function

  ' used to get the "previous chatRoom" by [alias] and AliasID
  Public Shared Function GetPreviousChatRoom(ByVal context As HttpContext, ByVal [alias] As String, ByVal AliasID As Integer) As tblNotify

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)

    Dim notifyUser As tblNotify = Nothing

    ' get current session from db  
    Dim session = GetMySession(context)

    Try

      ' did I send notification get the room
      notifyUser = GetMyPreviousNotification(session.UserAlias, session.UID, [alias], AliasID)
      If notifyUser IsNot Nothing Then
        Return notifyUser
      Else ' did the alias send the notification get the room
        notifyUser = GetOtherPreviousNotification([alias], AliasID, session.UserAlias, session.UID)
        If notifyUser IsNot Nothing Then
          Return notifyUser
        End If
      End If

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>GetPreviousChatRoom(ByVal [alias] As String, ByVal AliasID As Integer) As tblNotify</b><br />" + ex.Message

    End Try

    Return Nothing

  End Function

  Public Shared Function SetNotificationStatus(ByVal notifyID As Integer, ByVal bAccepted As Boolean, ByVal ChatRoomID As Guid) As Boolean

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)

    Try

      If bAccepted Then

        ' accept chat and set status to Y
        Dim notifyAccept As tblNotify = db.tblNotifies.SingleOrDefault(Function(n) n.NotifyID = notifyID AndAlso n.ChatRoomID = ChatRoomID)

        If notifyAccept IsNot Nothing Then
          notifyAccept.notifyStatus = "Y"
          db.SubmitChanges()
        End If

      Else

        ' send "back" notification that user is "unavailable" to chat at this time
        Dim notifyDecline As tblNotify = db.tblNotifies.SingleOrDefault(Function(n) n.NotifyID = notifyID AndAlso n.ChatRoomID = ChatRoomID)

        If notifyDecline IsNot Nothing Then
          notifyDecline.notifyStatus = "U"
          db.SubmitChanges()
        End If

      End If

      Return True

    Catch
      Return False
    End Try

  End Function

  Public Shared Sub DeleteUserNotifications(ByVal ChatRoomID As Guid, ByVal bAccepted As Boolean)

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)

    Try

      If bAccepted Then

        ' set accepted flag to "A" so notification stays around for the "chat header block"
        Dim notifyAccept As tblNotify = db.tblNotifies.SingleOrDefault(Function(n) n.ChatRoomID = ChatRoomID)

        If Not IsNothing(notifyAccept) Then
          notifyAccept.notifyStatus = "A"
          db.SubmitChanges()
        End If

      Else

        ' before we close chat window if a "talker" is still "active" have keep notification
        ' to re-establish chat session again ...
        ' see if any "talkers" are still in the "current room"
        Dim talkerList As List(Of tblTalker) = GetRoomTalkerList(ChatRoomID)
        Dim result As New List(Of RoomTalker)()

        If talkerList IsNot Nothing Then
          If talkerList.Count > 0 Then

            For Each talker As tblTalker In talkerList
              result.Add(New RoomTalker(talker, HttpContext.Current))
            Next

          End If
        End If

        If result.Count > 0 Then

          ' set accepted flag to "H" so notification stays around for untill "all users have left the room"
          Dim notifyUpdate As tblNotify = db.tblNotifies.SingleOrDefault(Function(n) n.ChatRoomID = ChatRoomID)

          If Not IsNothing(notifyUpdate) Then

            ' if the status is all ready set to "H" hold then don't update
            If Not notifyUpdate.notifyStatus.ToString.ToUpper.Contains("H") Then
              notifyUpdate.notifyStatus = "H"
              db.SubmitChanges()
            End If

          End If

        Else

          ' don't set accept flag just delete the notification then delete the "temp" chat room
          Dim notifyDelete As tblNotify = db.tblNotifies.SingleOrDefault(Function(n) n.ChatRoomID = ChatRoomID)

          If Not IsNothing(notifyDelete) Then
            db.tblNotifies.DeleteOnSubmit(notifyDelete)
            db.SubmitChanges()
          End If

          ' before we can "delete" the chat room we have to "delete" any "talkers" still left in room
          RemoveAllTalkersFromRoom(ChatRoomID)

          ' now delete chat room
          DeleteChatRoom(ChatRoomID)

        End If

      End If

      Return

    Catch
      Return
    End Try

  End Sub

#End Region

#Region "ChatRoom Management"

  Public Shared Function CreateChatRoom(ByVal roomName As String, ByVal password As String, ByVal isLock As Boolean, ByVal maxUserNumber As Integer, ByVal needPassword As Boolean) As Guid

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)
    Dim room As New tblChatRoom()

    room.ChatRoomID = Guid.NewGuid()
    room.ChatRoomName = roomName
    room.ChatRoomPassword = password
    room.IsLock = isLock
    room.MaxUserNumber = maxUserNumber
    room.NeedPassword = needPassword

    db.tblChatRooms.InsertOnSubmit(room)
    db.SubmitChanges()
    Return room.ChatRoomID

  End Function

  Public Shared Function DeleteChatRoom(ByVal roomid As Guid) As Boolean

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)
    Dim bResult As Boolean = False

    Try

      Dim room As tblChatRoom = db.tblChatRooms.SingleOrDefault(Function(r) r.ChatRoomID = roomid)

      If room IsNot Nothing Then
        Try
          db.tblChatRooms.DeleteOnSubmit(room)
          db.SubmitChanges()
          bResult = True
        Catch sqlError As SqlClient.SqlException
          Return bResult
        End Try
      End If

    Catch
      Return bResult
    End Try

    Return bResult

  End Function

  Public Shared Function GetChatRoom(ByVal roomid As Guid) As tblChatRoom

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)
    Return db.tblChatRooms.SingleOrDefault(Function(r) r.ChatRoomID = roomid)

  End Function

  Public Shared Function IsRoomFull(ByVal roomID As Guid) As Boolean
    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)
    Dim rsl = db.tblChatRooms.SingleOrDefault(Function(room) room.ChatRoomID = roomID)

    If rsl IsNot Nothing Then
      Return rsl.MaxUserNumber = (From t In rsl.tblTalkers Where t.CheckOutTime Is Nothing Select t).Count()
    Else
      Return False
    End If

  End Function

  Public Shared Function GetChatRoomList() As List(Of tblChatRoom)
    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)
    Try
      Return db.tblChatRooms.ToList()
    Catch ex As Exception
      Return Nothing
    End Try
  End Function

  Public Shared Function JoinChatRoom(ByVal ChatRoomID As Guid, ByVal context As HttpContext) As Boolean

    ' check and see if this room is full
    If Not IsRoomFull(ChatRoomID) Then

      Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)

      ' get surrent session from db
      Dim session As tblSession = GetMySession(context)

      ' check to see if this user has a session in the db if they dont have a session then return false
      If session Is Nothing Then
        Return False
      End If

      If db.tblTalkers.Count(Function(t) t.ChatRoomID = ChatRoomID AndAlso t.SessionID = session.UID AndAlso t.CheckOutTime Is Nothing) > 0 Then
        ' cheeck to see if this "talker" is still in this room (CheckOutTime = Nothing), if still here return false
        Return False
      ElseIf db.tblTalkers.Count(Function(t) t.ChatRoomID = ChatRoomID AndAlso t.SessionID = session.UID AndAlso t.CheckOutTime IsNot Nothing) > 0 Then
        ' cheeck to see if this "talker" is still in this room (CheckOutTime <> Nothing), if still here update CheckOutTime = Nothing
        ' and return true
        Dim previousTalker As tblTalker = db.tblTalkers.SingleOrDefault(Function(t) t.ChatRoomID = ChatRoomID AndAlso t.SessionID = session.UID)

        If previousTalker IsNot Nothing Then
          previousTalker.CheckOutTime = Nothing
          db.SubmitChanges()
        End If

        Return True
      Else
        ' this talker was not in the room ... add this "talker" to the room
        Dim talker As New tblTalker()
        talker.ChatRoomID = ChatRoomID
        talker.CheckInTime = DateTime.Now
        talker.CheckOutTime = Nothing
        talker.SessionID = session.UID
        db.tblTalkers.InsertOnSubmit(talker)
        db.SubmitChanges()
        Return True
      End If
    Else
      Return False
    End If

  End Function

  Public Shared Function FindTalker(ByVal ChatRoomID As Guid, ByVal context As HttpContext) As tblTalker

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)
    Dim rsl = db.tblTalkers.FirstOrDefault(Function(t) t.ChatRoomID = ChatRoomID AndAlso t.SessionID = GetMySession(context).UID)
    Return rsl

  End Function

  Public Shared Function FindOtherTalker(ByVal ChatRoomID As Guid, ByVal context As HttpContext) As tblTalker

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)
    Dim rsl = db.tblTalkers.FirstOrDefault(Function(t) t.ChatRoomID = ChatRoomID AndAlso t.SessionID <> GetMySession(context).UID)
    Return rsl

  End Function

  Public Shared Function RemoveTalkerFromRoom(ByVal ChatRoomID As Guid, ByVal context As HttpContext) As Boolean

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)

    Try

      ' get the current session
      Dim session As tblSession = ChatManager.GetMySession(context)

      If session IsNot Nothing Then

        ' need to have the current "key" record for delete
        Dim deleteTalker As tblTalker = db.tblTalkers.SingleOrDefault(Function(t) t.ChatRoomID = ChatRoomID AndAlso t.SessionID = session.UID)

        If deleteTalker IsNot Nothing Then
          db.tblTalkers.DeleteOnSubmit(deleteTalker)
          db.SubmitChanges()
        End If

        Return True

      Else
        Return False
      End If

    Catch
      Return False
    End Try

  End Function

  Public Shared Function RemoveAllTalkersFromRoom(ByVal ChatRoomID As Guid) As Boolean

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)

    Try

      If ChatRoomID <> Guid.Empty Then

        Dim list = From t In db.tblTalkers Where t.ChatRoomID = ChatRoomID
        db.tblTalkers.DeleteAllOnSubmit(list)
        db.SubmitChanges()

        Return True

      Else
        Return False
      End If

    Catch
      Return False
    End Try

  End Function

  Public Shared Function GetRoomTalkerList(ByVal ChatRoomID As Guid) As List(Of tblTalker)

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)
    Dim rsl = From d In db.tblTalkers Where d.CheckOutTime Is Nothing AndAlso d.ChatRoomID = ChatRoomID
    Return rsl.ToList()

  End Function

  Public Shared Sub LeaveChatRoom(ByVal ChatRoomID As Guid, ByVal context As HttpContext)
    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)

    ' get the current session
    Dim session As tblSession = ChatManager.GetMySession(context)

    If session IsNot Nothing Then

      ' get the "talker" from this "room" who's session matches this session and hasnt left the room
      Dim talker = db.tblTalkers.FirstOrDefault(Function(t) t.ChatRoomID = ChatRoomID AndAlso t.SessionID = session.UID AndAlso t.CheckOutTime Is Nothing)

      If talker IsNot Nothing Then
        talker.CheckOutTime = DateTime.Now
        db.SubmitChanges()
      End If

      TryToDeleteChatMessageList(ChatRoomID)

      RemoveTalkerFromRoom(ChatRoomID, context)

      DeleteUserNotifications(ChatRoomID, False)

    End If

  End Sub

#End Region

#Region "Chat Session Management"

  Public Shared Function GetMySession(ByVal context As HttpContext) As tblSession

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)

    Dim session As tblSession = Nothing

    Try

      session = db.tblSessions.SingleOrDefault(Function(s) s.SessionID = context.Session.SessionID)

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>GetMySession(ByVal context As HttpContext) As tblSession</b><br />" + ex.Message

    End Try

    Return session

  End Function

  Public Shared Function SessionExist(ByVal context As HttpContext) As Boolean
    Return GetMySession(context) IsNot Nothing
  End Function

  Public Shared Function GetUserSession(ByVal context As HttpContext, ByVal [alias] As String, ByVal AliasID As Integer) As tblSession

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)

    Dim session As tblSession = Nothing

    Try

      session = db.tblSessions.SingleOrDefault(Function(s) s.SessionID <> context.Session.SessionID AndAlso s.UserAlias.ToLower.Trim = [alias].ToLower.Trim AndAlso s.UID = AliasID)

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>GetUserSession(ByVal context As HttpContext, ByVal [alias] As String, ByVal AliasID As Integer) As tblSession</b><br />" + ex.Message

    End Try

    Return session

  End Function

  Public Shared Function CurrentUserSessions(ByVal context As HttpContext) As List(Of tblSession)

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)

    Dim session = GetMySession(context)

    ' if sessionID doesn't match and user alias doesn't match and online time is greater than 10 minutes ago pickup user 
    '                                          check onLineTime.Date = now.AddMinutes(-10).Date              check onLineTime.TimeOfDay = now.AddMinutes(-10).TimeOfDay
    Dim rsl = From s In db.tblSessions Where (s.SessionID <> session.SessionID AndAlso s.UserAlias.ToLower.Trim <> session.UserAlias.ToLower.Trim AndAlso _
                                              ((s.onLineTime.Date = DateTime.Now.AddMinutes(-10).Date) And (s.onLineTime.TimeOfDay > DateTime.Now.AddMinutes(-10).TimeOfDay)))

    Return rsl.ToList()

  End Function

  Public Shared Function AllUserSessions(ByVal context As HttpContext) As List(Of tblSession)

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)

    Dim session = GetMySession(context)

    ' if sessionID doesn't match and user alias doesn't match gets list of all other users sessions
    Dim rsl = From s In db.tblSessions Where (s.SessionID <> session.SessionID AndAlso s.UserAlias.ToLower.Trim <> session.UserAlias.ToLower.Trim)

    Return rsl.ToList()

  End Function

  Public Shared Function isUserOnLine(ByVal [alias] As String, ByVal AliasID As Integer) As Boolean

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)

    ' if online time is greater than 10 minutes ago and alias and ID matches user is on line                               check onLineTime.Date = now.AddMinutes(-10).Date              check onLineTime.TimeOfDay = now.AddMinutes(-10).TimeOfDay
    Dim session = db.tblSessions.FirstOrDefault(Function(s) s.UserAlias.ToLower.Trim = [alias].ToLower.Trim AndAlso s.UID = AliasID AndAlso ((s.onLineTime.Date = DateTime.Now.AddMinutes(-10).Date) And (s.onLineTime.TimeOfDay > DateTime.Now.AddMinutes(-10).TimeOfDay)))

    Return session IsNot Nothing

  End Function

  Public Shared Function CreateSession(ByVal context As HttpContext, ByVal [alias] As String) As Boolean

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)

    Try

      If String.IsNullOrEmpty([alias]) Then ' if userAlias is empty dont add user to session
        Return False
      End If

      Dim subID As String = context.Session.Item("localUser").crmSubSubID.ToString.Trim
      Dim login As String = context.Session.Item("localUser").crmUserLogin.ToString.Trim
      Dim seqNO As String = context.Session.Item("localUser").crmSubSeqNo.ToString.Trim
      Dim UsrID As String = context.Session.Item("localUser").crmUserContactID.ToString.Trim
      Dim CompID As String = context.Session.Item("localUser").crmUserCompanyID.ToString.Trim

      Dim hasSession = db.tblSessions.SingleOrDefault(Function(s) s.UserAlias.ToLower.Trim = [alias].ToLower.Trim AndAlso s.companyID = CompID AndAlso s.contactID = UsrID)

      If hasSession Is Nothing Then

        Dim session As New tblSession()

        session.SessionID = context.Session.SessionID
        session.IP = context.Request.UserHostAddress

        session.UserAlias = [alias]

        ' takes local user and adds alias and local user info to session table
        session.subscriptionID = subID
        session.userID = login
        session.sequenceNum = seqNO
        session.contactID = UsrID
        session.companyID = CompID

        ' add friendly name and look up company name when createing session
        session.FriendlyName = context.Session.Item("localUser").crmLocalUserFirstName.ToString.Trim + " " + context.Session.Item("localUser").crmLocalUserLastName.ToString.Trim

        session.ComapnyName = commonEvo.get_company_name_fromID(CLng(CompID), 0, False, True, "").Trim

        session.onLineTime = DateTime.Now

        db.tblSessions.InsertOnSubmit(session)
        db.SubmitChanges()

        Return True
      Else
        Return False
      End If

    Catch
      Return False
    End Try

  End Function

  Public Shared Function UpdateSession(ByVal context As HttpContext) As Boolean

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)

    Try

      Dim subID As String = context.Session.Item("localUser").crmSubSubID.ToString.Trim
      Dim login As String = context.Session.Item("localUser").crmUserLogin.ToString.Trim
      Dim seqNO As String = context.Session.Item("localUser").crmSubSeqNo.ToString.Trim
      Dim UsrID As String = context.Session.Item("localUser").crmUserContactID.ToString.Trim
      Dim CompID As String = context.Session.Item("localUser").crmUserCompanyID.ToString.Trim

      Dim mySession = db.tblSessions.SingleOrDefault(Function(s) s.subscriptionID = subID AndAlso s.userID = Login AndAlso s.sequenceNum = seqNO AndAlso s.companyID = CompID AndAlso s.contactID = UsrID)

      If mySession IsNot Nothing Then

        mySession.SessionID = context.Session.SessionID.Trim
        mySession.IP = context.Request.UserHostAddress.Trim

        mySession.FriendlyName = context.Session.Item("localUser").crmLocalUserFirstName.ToString.Trim + " " + context.Session.Item("localUser").crmLocalUserLastName.ToString.Trim
        mySession.ComapnyName = commonEvo.get_company_name_fromID(CLng(CompID), 0, False, True, "").Trim

        mySession.onLineTime = DateTime.Now

        db.SubmitChanges()
        Return True
      Else
        Return False
      End If
    Catch
      Return False
    End Try

  End Function

  Public Shared Function ChangeSessionSubscription(ByVal context As HttpContext, ByVal [alias] As String) As Boolean

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)

    Try

      Dim subID As String = context.Session.Item("localUser").crmSubSubID.ToString.Trim
      Dim login As String = context.Session.Item("localUser").crmUserLogin.ToString.Trim
      Dim seqNO As String = context.Session.Item("localUser").crmSubSeqNo.ToString.Trim

      Dim UsrID As String = context.Session.Item("localUser").crmUserContactID.ToString.Trim
      Dim CompID As String = context.Session.Item("localUser").crmUserCompanyID.ToString.Trim

      Dim mySession = db.tblSessions.SingleOrDefault(Function(s) s.UserAlias.ToLower.Trim = [alias].ToLower.Trim AndAlso s.companyID = CompID AndAlso s.contactID = UsrID)

      If mySession IsNot Nothing Then

        mySession.SessionID = context.Session.SessionID
        mySession.IP = context.Request.UserHostAddress

        mySession.subscriptionID = subID
        mySession.userID = login
        mySession.sequenceNum = seqNO

        mySession.onLineTime = DateTime.Now

        db.SubmitChanges()

        Return True
      Else
        Return False
      End If

    Catch
      Return False
    End Try

  End Function

  Public Shared Function LogMySessionOff(ByVal context As HttpContext) As Boolean

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)

    Try

      Dim mySession = db.tblSessions.SingleOrDefault(Function(s) s.SessionID = context.Session.SessionID)

      If mySession IsNot Nothing Then

        mySession.onLineTime = DateAdd(DateInterval.Minute, -10, Now())
        db.SubmitChanges()

        Return True
      Else
        Return False
      End If
    Catch
      Return False
    End Try

  End Function

  Public Shared Function DeleteSession(ByVal context As HttpContext, ByVal [alias] As String) As Boolean

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)

    Try

      Dim session As tblSession = db.tblSessions.SingleOrDefault(Function(s) s.SessionID = context.Session.SessionID AndAlso s.UserAlias.ToLower.Trim = [alias].ToLower.Trim)

      ' delete messages
      ' delete talkers
      ' delete notifications

      db.tblSessions.DeleteOnSubmit(session)
      db.SubmitChanges()

      Return True
    Catch
      Return False
    End Try

  End Function

  Public Shared Function DeleteAllSessions(ByVal context As HttpContext) As Boolean

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)

    Try

      Dim list = From s In db.tblSessions Where s.SessionID IsNot Nothing
      db.tblSessions.DeleteAllOnSubmit(list)
      db.SubmitChanges()

      Return True

    Catch
      Return False
    End Try

  End Function

  Public Shared Sub CheckAndInitChat(ByVal bCleanUpOldChatInfo As Boolean, ByRef bEnableChat As Boolean, Optional ByRef txtAlias As String = "", Optional ByRef txtAliasID As Integer = 0)

    Dim sErrorString As String = ""

    If Not HttpContext.Current.Session.Item("localPreferences").loadUserSession(sErrorString, CLng(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString), HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString, CLng(HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString), CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)) Then
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "error in load preferences : " + sErrorString
    End If

    bEnableChat = HttpContext.Current.Session.Item("localPreferences").ChatEnabled

    If bEnableChat Then

      Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)

      Dim subID As String = HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString.Trim
      Dim login As String = HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString.Trim
      Dim seqNO As String = HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString.Trim
      Dim UsrID As String = HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString.Trim
      Dim CompID As String = HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString.Trim

      ' check that this subscription matches
      Dim mySession = db.tblSessions.SingleOrDefault(Function(s) s.subscriptionID = subID AndAlso s.userID = login AndAlso s.sequenceNum = seqNO AndAlso s.companyID = CompID AndAlso s.contactID = UsrID)

      If mySession IsNot Nothing Then

        txtAlias = mySession.UserAlias
        txtAliasID = mySession.UID

        ' if chat flag is enabled but updatesession fails then return false
        If Not ChatManager.UpdateSession(HttpContext.Current) Then
          bEnableChat = False
          Return
        End If
      Else
        bEnableChat = False
        Return
      End If

      If bCleanUpOldChatInfo Then

        ' now clean up any "previous chat items"
        ' check and see "if I notified any users in previous session" from any "notification" over 20 minutes old
        ' if i did delete "notification(s), talker(s), and room(s)" 

        Dim result As New List(Of NotifyUser)()

        Dim notifyList As List(Of tblNotify) = ChatManager.GetOldNotifications(mySession.UserAlias, mySession.UID)

        If notifyList IsNot Nothing Then

          For Each notify As tblNotify In notifyList
            result.Add(New NotifyUser(notify))
          Next

          Dim RoomID As New Guid

          For Each nu As NotifyUser In result

            RoomID = nu.RoomID

            Dim notifyDelete As tblNotify = db.tblNotifies.SingleOrDefault(Function(n) n.ChatRoomID = RoomID)

            If Not IsNothing(notifyDelete) Then
              db.tblNotifies.DeleteOnSubmit(notifyDelete)
              db.SubmitChanges()
            End If

            ' before we can "delete" the chat room we have to "delete" any "talkers" still left in room
            ChatManager.RemoveAllTalkersFromRoom(RoomID)

            ' now delete chat room
            ChatManager.DeleteChatRoom(RoomID)

          Next

        End If ' notifyList IsNot Nothing

        ' next clean up any "curent chat records"
        DeleteAllFromCurrentChat(HttpContext.Current)

      End If ' bCleanUpOldChatInfo

    End If ' bEnableChat

  End Sub

  Public Shared Function userEnabledChat(ByVal companyID As Long, ByVal contactID As Long, ByVal txtalias As String, ByRef txtAliasID As Integer) As Boolean

    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConnection As New System.Data.SqlClient.SqlConnection
    Dim lDataReader As System.Data.SqlClient.SqlDataReader = Nothing
    Dim sQuery As String = ""

    Dim bResult As Boolean = False

    sQuery = "SELECT TOP 1 subins_chat_flag, tblSession.UID AS txtAliasID, sub_id, subins_login, subins_seq_no FROM View_JETNET_Customers WITH(NOLOCK)"
    sQuery += " INNER JOIN tblSession ON ( tblSession.subscriptionID = sub_id and tblSession.companyID = sub_comp_id and tblSession.contactID = subins_contact_id AND tblSession.UserAlias = contact_email_address)"
    sQuery += " WHERE sub_comp_id = " + companyID.ToString + " AND subins_contact_id = " + contactID.ToString + " AND contact_email_address = '" + txtalias.Trim + "'"

    Try

      txtAliasID = 0

      SqlConnection.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConnection.Open()

      SqlCommand.Connection = SqlConnection
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = sQuery

      lDataReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      If lDataReader.HasRows Then

        Do While lDataReader.Read()

          If Not IsDBNull(lDataReader.Item("subins_chat_flag")) Then

            If Not String.IsNullOrEmpty(lDataReader.Item("subins_chat_flag").ToString.Trim) Then

              bResult = IIf(lDataReader.Item("subins_chat_flag").ToString.ToUpper.Contains("Y"), True, False)

            End If

          End If

          If bResult Then

            If Not IsDBNull(lDataReader.Item("txtAliasID")) Then

              If Not String.IsNullOrEmpty(lDataReader.Item("txtAliasID").ToString.Trim) Then

                txtAliasID = CInt(lDataReader.Item("txtAliasID").ToString.Trim)
              End If

            End If

            Exit Do

          End If

        Loop

      End If

    Catch SqlException

      SqlConnection.Dispose()
      SqlCommand.Dispose()

    Finally

      SqlCommand.Dispose()
      SqlConnection.Close()
      SqlConnection.Dispose()

    End Try

    lDataReader = Nothing
    SqlCommand = Nothing
    SqlConnection = Nothing

    Return bResult

  End Function

  Public Shared Function checkForOtherChatSubscriptions(ByVal userAlias As String, ByRef nPreviousSubID As Long, ByRef nPreviousLogin As String, ByRef nPreviousSeqNo As Long) As Boolean

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing
    Dim SqlReader As SqlClient.SqlDataReader : SqlReader = Nothing

    Dim bResult As Boolean = False
    Dim results_table As New DataTable

    Dim nCurrentSubID As Long = CLng(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString)

    Dim UsrID As String = CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)
    Dim CompID As String = CLng(HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString)

    nPreviousSubID = 0

    Try

      ' first "check and see if this user has "chat" turned on on another subscription
      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      sQuery.Append(" SELECT TOP 1 subins_chat_flag, sub_id, subins_login, subins_seq_no FROM View_JETNET_Customers")
      sQuery.Append(" WHERE (subins_chat_flag = 'Y' AND subins_contact_id = " + UsrID.ToString)
      sQuery.Append(" AND sub_comp_id = " + CompID.ToString)
      sQuery.Append(" AND contact_email_address = '" + userAlias + "')")

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        results_table.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = results_table.GetErrors()
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in CheckChatStatus load datatable</b><br /> " + constrExc.Message
      End Try

      SqlReader.Close()

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          For Each r As DataRow In results_table.Rows

            If Not IsDBNull(r.Item("sub_id")) Then
              If Not String.IsNullOrEmpty(r.Item("sub_id").ToString.Trim) Then
                nPreviousSubID = CLng(r.Item("sub_id").ToString)
              End If
            End If

            If Not IsDBNull(r.Item("subins_login")) Then
              If Not String.IsNullOrEmpty(r.Item("subins_login").ToString.Trim) Then
                nPreviousLogin = r.Item("subins_login").ToString.Trim
              End If
            End If

            If Not IsDBNull(r.Item("subins_seq_no")) Then
              If Not String.IsNullOrEmpty(r.Item("subins_seq_no").ToString.Trim) Then
                nPreviousSeqNo = CLng(r.Item("subins_seq_no").ToString)
              End If
            End If

          Next

        End If

      End If

      If nCurrentSubID <> nPreviousSubID Then
        bResult = True
      End If

    Catch ex As Exception

    Finally

      SqlReader = Nothing

      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing

    End Try

    Return bResult

  End Function

  Public Shared Function UpdateChatStatus(ByVal userGUID As String, ByVal userAlias As String, ByVal bEnable As Boolean, ByVal bChangeSub As Boolean) As Boolean

    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim bResult As Boolean = False

    Try

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      If bChangeSub Then

        Dim nOtherSubID As Long = 0
        Dim nOtherLogon As String = ""
        Dim nOtherSeqNo As Long = 0

        Dim bHasOtherChatOn = checkForOtherChatSubscriptions(userAlias, nOtherSubID, nOtherLogon, nOtherSeqNo)

        If (bHasOtherChatOn And nOtherSubID > 0) Then
          ' clear the other "subscriptions" chat flag
          sQuery.Append("UPDATE Subscription_Install SET subins_web_action_date = NULL, subins_chat_flag = 'N'")
          sQuery.Append(" WHERE (subins_sub_id = " + nOtherSubID.ToString)
          sQuery.Append(" AND subins_contact_id = " + HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)
          sQuery.Append(" AND subins_seq_no = " + nOtherSeqNo.ToString)
          sQuery.Append(" AND subins_login = '" + nOtherLogon.Trim + "')")

          HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /> UpdateChatStatus(ByVal userGUID As String, ByVal userAlias As String, ByVal bEnable As Boolean, ByVal bChangeSub As Boolean) As Boolean</b><br />" + sQuery.ToString

          Try
            SqlCommand.CommandText = sQuery.ToString
            SqlCommand.ExecuteNonQuery()
            HttpContext.Current.Session.Item("bHadOtherChatSubscription") = Nothing
          Catch SqlException
            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in UpdateChatStatus ExecuteNonQuery{" + sQuery.ToString + "} :" + SqlException.Message
            Return False
          End Try

        End If

        sQuery = New StringBuilder()

      End If

      ' "enable/disable" chat
      If bEnable Then
        sQuery.Append("UPDATE Subscription_Install SET subins_web_action_date = NULL, subins_chat_flag = 'Y'")
      Else
        sQuery.Append("UPDATE Subscription_Install SET subins_web_action_date = NULL, subins_chat_flag = 'N'")
      End If

      sQuery.Append(" WHERE (subins_session_guid = '" + userGUID.Trim + "') AND (subins_sub_id = " + HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString)
      sQuery.Append(" AND subins_contact_id = " + HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)
      sQuery.Append(" AND subins_seq_no = " + HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString)
      sQuery.Append(" AND subins_login = '" + HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString.Trim + "')")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /> UpdateChatStatus(ByVal userGUID As String, ByVal userAlias As String, ByVal bEnable As Boolean, ByVal bChangeSub As Boolean) As Boolean</b><br />" + sQuery.ToString

      Try
        SqlCommand.CommandText = sQuery.ToString
        SqlCommand.ExecuteNonQuery()
      Catch SqlException
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in UpdateChatStatus ExecuteNonQuery{" + sQuery.ToString + "} :" + SqlException.Message
      End Try

      If bEnable Then

        If bChangeSub Then

          If ChangeSessionSubscription(HttpContext.Current, userAlias) Then
            bResult = True
          End If

        Else

          If Not CreateSession(HttpContext.Current, userAlias) Then
            ' if user already has a session just update current session
            UpdateSession(HttpContext.Current)
          End If

          bResult = True

        End If

      Else
        ' when disable keep the user but turn "off" chat flag
        'DeleteSession(HttpContext.Current, userAlias)

        bResult = True

      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in  UpdateChatStatus(ByVal userGUID As String, ByVal userAlias As String, ByVal bEnable As Boolean, ByVal bChangeSub As Boolean) As Boolean " + ex.Message

    Finally

      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing

    End Try

    commonLogFunctions.Log_User_Event_Data("UpdateChatStatus", "User Updated Chat Status GUID : " + userGUID, Nothing, , , , CLng(HttpContext.Current.Session.Item("localUser").crmUserCompanyID.ToString), CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString))

    Return bResult

  End Function

  Public Shared Function getSubscriberContactAlias() As String

    Dim tempTable As New DataTable
    Dim tmpPrefobj As New preferencesDataLayer

    Dim sSubscriberAlias As String = ""

    tmpPrefobj.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

    Try
      tempTable = tmpPrefobj.ReturnUserDetailsAndImage(HttpContext.Current.Session.Item("localUser").crmUserContactID)

      If Not IsNothing(tempTable) Then
        If tempTable.Rows.Count > 0 Then

          For Each r As DataRow In tempTable.Rows

            If Not (IsDBNull(r.Item("contact_email_address"))) And Not String.IsNullOrEmpty(r.Item("contact_email_address").ToString.Trim) Then
              sSubscriberAlias = r.Item("contact_email_address").ToString.Trim
            End If

          Next

        End If

      End If

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in getSubscriberContactAlias() As String " + ex.Message
    End Try

    tempTable = Nothing
    tmpPrefobj = Nothing

    Return sSubscriberAlias

  End Function

#End Region

#Region "Community List Management"

  Public Shared Function IsUserOnCommunityList(ByVal context As HttpContext, ByVal [alias] As String, ByVal AliasID As Integer) As Boolean
    Return GetCommunityListUser(context, [alias], AliasID) IsNot Nothing
  End Function

  Public Shared Function GetCommunityListUser(ByVal context As HttpContext, ByVal [alias] As String, ByVal AliasID As Integer) As tblCommunityList

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)
    Dim session = GetMySession(context)

    Dim communityUser As tblCommunityList = db.tblCommunityLists.SingleOrDefault(Function(l) l.SessionUID = session.UID AndAlso l.BuddyAlias.ToLower.Trim = [alias].ToLower.Trim AndAlso l.BuddyUID = AliasID)

    Return communityUser

  End Function

  Public Shared Function AddUserToCommunityList(ByVal context As HttpContext, ByVal [alias] As String, ByVal AliasID As Integer) As Boolean
    ' will need sessionUID of the "buddy" to get exact match
    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)

    Try

      Dim newListUser As New tblCommunityList()

      Dim mySession = GetMySession(context)

      If String.IsNullOrEmpty([alias]) Then ' if userAlias is empty dont add user to session
        Return False
      End If

      newListUser.SessionUID = mySession.UID
      newListUser.SessionAlias = mySession.UserAlias

      Dim hasSession = db.tblSessions.SingleOrDefault(Function(s) s.UserAlias.ToLower.Trim = [alias].ToLower.Trim AndAlso s.UID = AliasID)

      newListUser.BuddyUID = hasSession.UID
      newListUser.BuddyAlias = hasSession.UserAlias

      newListUser.BlockAlias = "N"
      newListUser.IgnoreAlias = "N"
      newListUser.IncludeAlias = "Y"

      db.tblCommunityLists.InsertOnSubmit(newListUser)
      db.SubmitChanges()
      Return True
    Catch
      Return False
    End Try
  End Function

  Public Shared Function DeleteUserFromCommunityList(ByVal context As HttpContext, ByVal [alias] As String, ByVal AliasID As Integer) As Boolean

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)

    Try

      Dim session = GetMySession(context)

      Dim list As tblCommunityList = db.tblCommunityLists.SingleOrDefault(Function(l) l.SessionUID = session.UID AndAlso l.BuddyAlias.ToLower.Trim = [alias].ToLower.Trim AndAlso l.BuddyUID = AliasID)

      db.tblCommunityLists.DeleteOnSubmit(list)
      db.SubmitChanges()

      Return True
    Catch
      Return False
    End Try

  End Function

  Public Shared Function CurrentCommunityListUsers(ByVal context As HttpContext) As List(Of tblCommunityList)

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)

    Dim session = GetMySession(context)

    ' if sessionUID match and UserAlias match and online time is greater than 10 minutes ago pickup online community users 
    '                                          check onLineTime.Date = now.AddMinutes(-10).Date              check onLineTime.TimeOfDay = now.AddMinutes(-10).TimeOfDay
    Dim cml = From l In db.tblCommunityLists Where (l.SessionUID = session.UID AndAlso l.SessionAlias.ToLower.Trim = session.UserAlias.ToLower.Trim AndAlso l.IncludeAlias = "Y" AndAlso _
                                              ((l.tblSession.onLineTime.Date = DateTime.Now.AddMinutes(-10).Date) And (l.tblSession.onLineTime.TimeOfDay > DateTime.Now.AddMinutes(-10).TimeOfDay))) Order By l.tblSession.FriendlyName Ascending

    Return cml.ToList()

  End Function

  Public Shared Function AllCommunityListUsers(ByVal context As HttpContext) As List(Of tblCommunityList)

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)

    Dim session = GetMySession(context)

    ' if sessionUID match and UserAlias match pickup all community users 
    Dim cml = From l In db.tblCommunityLists Where (l.SessionUID = session.UID AndAlso l.SessionAlias.ToLower.Trim = session.UserAlias.ToLower.Trim AndAlso l.IncludeAlias = "Y") Order By l.tblSession.FriendlyName Ascending

    Return cml.ToList()

  End Function

  Public Shared Function DisplayUsersForChatFilter() As String

    Dim cssClass As String = "alt_row_light"
    Dim sScptStr As StringBuilder = New StringBuilder()

    sScptStr.Append("<ul class=""commentlist"" id=""UserUL"">")

    Dim result As New List(Of chatSession)()

    Dim userList As List(Of tblSession) = ChatManager.AllUserSessions(HttpContext.Current)

    For Each session As tblSession In userList ' get me a list of all chat user sessions
      If Not IsUserOnCommunityList(HttpContext.Current, session.UserAlias, session.UID) Then
        ' only add users "not" on my community list
        result.Add(New chatSession(session))
      End If
    Next

    If result.Count > 0 Then

      For Each s As chatSession In result
        sScptStr.Append("<li style=""display:none""><a href=""#"" class="""" style=""padding-right:4px;""><strong>" + s.SessionFriendlyName.Trim + "</strong></a>")
        sScptStr.Append("<company>" + s.SessionComapnyName.Trim + "</company>, <email>" + s.SessionAlias + "</email>")
        sScptStr.Append("<img src=""/images/addcompare.png"" title= ""Add " + s.SessionFriendlyName.Trim + " to my JETNET Community Chat list""")
        sScptStr.Append(" alt= ""Add " + s.SessionFriendlyName.Trim + " to my JETNET Community Chat list""")
        sScptStr.Append(" class=""float_right"" onclick='fnAddCommunityUserNotify(""" + s.SessionAlias + """," + s.SessionUID.ToString + ");return false;' /></li>")
        'Using inline style on purpose - this will allow jquery to manipulate it using show/fade
      Next

    End If

    sScptStr.Append("</ul><div class=""clearfix""></div>")

    Return sScptStr.ToString

  End Function

#End Region

#Region "Current Chat Management"

  Public Shared Function IsUserInCurrentChat(ByVal context As HttpContext, ByVal AliasUID As Integer) As Boolean
    Return GetCurrentChatUser(context, AliasUID) IsNot Nothing
  End Function

  Public Shared Function GetCurrentChatUser(ByVal context As HttpContext, ByVal AliasUID As Integer) As tblCurrentChat

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)
    Dim session = GetMySession(context)

    Dim userAlias As tblCurrentChat = db.tblCurrentChats.SingleOrDefault(Function(l) l.chatSessionUID = session.UID AndAlso l.chatAliasSessionUID = AliasUID)

    Return userAlias

  End Function

  Public Shared Function AddAliasToCurrentChat(ByVal context As HttpContext, ByVal AliasUID As Integer) As Boolean
    ' will need sessionUID of the "buddy" to get exact match
    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)

    Try

      Dim newAlias As New tblCurrentChat()

      Dim mySession = GetMySession(context)

      If AliasUID < 1 Then ' if AliasUID is less than 1 dont add to list
        Return False
      End If

      newAlias.chatSessionUID = mySession.UID
      newAlias.chatAliasSessionUID = AliasUID
      newAlias.chatStartTime = Now()

      db.tblCurrentChats.InsertOnSubmit(newAlias)
      db.SubmitChanges()
      Return True
    Catch
      Return False
    End Try
  End Function

  Public Shared Function DeleteAliasFromCurrentChat(ByVal context As HttpContext, ByVal AliasUID As Integer) As Boolean

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)

    Try

      Dim session = GetMySession(context)

      Dim userAlias As tblCurrentChat = db.tblCurrentChats.SingleOrDefault(Function(l) l.chatSessionUID = session.UID AndAlso l.chatAliasSessionUID = AliasUID)

      db.tblCurrentChats.DeleteOnSubmit(userAlias)
      db.SubmitChanges()

      Return True
    Catch
      Return False
    End Try

  End Function

  Public Shared Function DeleteAllFromCurrentChat(ByVal context As HttpContext) As Boolean

    Dim db As New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)

    Try

      Dim mySession As tblSession = GetMySession(context)

      If mySession IsNot Nothing Then

        Dim list = From c In db.tblCurrentChats Where c.chatSessionUID = mySession.UID
        db.tblCurrentChats.DeleteAllOnSubmit(list)
        db.SubmitChanges()

        Return True
      Else
        Return False
      End If

    Catch
      Return False
    End Try

  End Function

#End Region

End Class
