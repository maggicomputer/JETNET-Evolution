' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/chat/Logic/ChatRoom.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:44a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: ChatRoom.vb $
'
' ********************************************************************************

Imports System.Runtime.Serialization

<DataContract()> _
Public Class ChatRoom

    <DataMember()> _
    Public Property RoomID() As Guid
        Get
            Return m_RoomID
        End Get
        Private Set(ByVal value As Guid)
            m_RoomID = value
        End Set
    End Property
    Private m_RoomID As Guid


    <DataMember()> _
    Public Property RoomName() As String
        Get
            Return m_RoomName
        End Get
        Private Set(ByVal value As String)
            m_RoomName = value
        End Set
    End Property
    Private m_RoomName As String


    <DataMember()> _
    Public Property MaxUser() As Integer
        Get
            Return m_MaxUser
        End Get
        Private Set(ByVal value As Integer)
            m_MaxUser = value
        End Set
    End Property
    Private m_MaxUser As Integer


    <DataMember()> _
    Public Property CurrentUser() As Integer
        Get
            Return m_CurrentUser
        End Get
        Private Set(ByVal value As Integer)
            m_CurrentUser = value
        End Set
    End Property
    Private m_CurrentUser As Integer

    Public Sub New(ByVal id As Guid)
    Dim db As SessionDBDataContext = New SessionDBDataContext(HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString)
        Dim room = db.tblChatRooms.SingleOrDefault(Function(r) r.ChatRoomID = id)
        If room IsNot Nothing Then
            RoomID = id
            RoomName = room.ChatRoomName
            MaxUser = room.MaxUserNumber
            CurrentUser = (From t In room.tblTalkers Where t.CheckOutTime Is Nothing Select t).Count()
        End If
    End Sub

End Class
