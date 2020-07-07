' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/chat/Logic/RoomTalker.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:44a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: RoomTalker.vb $
'
' ********************************************************************************

Imports System.Runtime.Serialization

<DataContract()> _
Public Class RoomTalker
  <DataMember()> _
  Public Property TalkerAlias() As String
    Get
      Return m_TalkerAlias
    End Get
    Private Set(ByVal value As String)
      m_TalkerAlias = value
    End Set
  End Property
  Private m_TalkerAlias As String

  <DataMember()> _
  Public Property TalkerAliasName() As String
    Get
      Return m_TalkerAliasName
    End Get
    Private Set(ByVal value As String)
      m_TalkerAliasName = value
    End Set
  End Property
  Private m_TalkerAliasName As String

  <DataMember()> _
  Public Property TalkerSession() As String
    Get
      Return m_TalkerSession
    End Get
    Private Set(ByVal value As String)
      m_TalkerSession = value
    End Set
  End Property
  Private m_TalkerSession As String

  <DataMember()> _
Public Property TalkerSessionUID() As String
    Get
      Return m_TalkerSessionUID
    End Get
    Private Set(ByVal value As String)
      m_TalkerSessionUID = value
    End Set
  End Property
  Private m_TalkerSessionUID As String

  <DataMember()> _
  Public Property TalkerIP() As String
    Get
      Return m_TalkerIP
    End Get
    Private Set(ByVal value As String)
      m_TalkerIP = value
    End Set
  End Property
  Private m_TalkerIP As String

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

  Public Sub New(ByVal talker As tblTalker, ByVal context As HttpContext)
    TalkerAlias = talker.tblSession.UserAlias
    TalkerAliasName = talker.tblSession.FriendlyName
    TalkerIP = talker.tblSession.IP
    TalkerSession = talker.tblSession.SessionID
    TalkerSessionUID = talker.tblSession.UID
    IsFriend = (TalkerSession <> context.Session.SessionID)
  End Sub
End Class
