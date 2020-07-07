' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/chat/Logic/Message.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:44a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: Message.vb $
'
' ********************************************************************************

Imports System.Runtime.Serialization

<DataContract()> _
Public Class Message
  <DataMember()> _
  Public Property Talker() As String
    Get
      Return m_Talker
    End Get
    Private Set(ByVal value As String)
      m_Talker = value
    End Set
  End Property
  Private m_Talker As String

  <DataMember()> _
  Public Property TalkerName() As String
    Get
      Return m_TalkerName
    End Get
    Private Set(ByVal value As String)
      m_TalkerName = value
    End Set
  End Property
  Private m_TalkerName As String

  <DataMember()> _
  Public Property MessageData() As String
    Get
      Return m_MessageData
    End Get
    Private Set(ByVal value As String)
      m_MessageData = value
    End Set
  End Property
  Private m_MessageData As String

  <DataMember()> _
  Public Property SendTime() As DateTime
    Get
      Return m_SendTime
    End Get
    Private Set(ByVal value As DateTime)
      m_SendTime = value
    End Set
  End Property
  Private m_SendTime As DateTime

  <DataMember()> _
  Public Property IsFriend() As Boolean
    Get
      Return m_IsFriend
    End Get
    Private Set(ByVal value As Boolean)
      m_IsFriend = value
    End Set
  End Property
  Private m_IsFriend As Boolean

  Public Sub New(ByVal message__1 As tblMessagePool, ByVal session As HttpContext)
    Talker = message__1.tblTalker.tblSession.UserAlias
    TalkerName = message__1.tblTalker.tblSession.FriendlyName
    MessageData = message__1.message
    SendTime = message__1.SendTime
    IsFriend = (message__1.tblTalker.tblSession.SessionID <> session.Session.SessionID)
  End Sub
End Class

