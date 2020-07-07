' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/chat/Logic/currentChat.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:44a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: currentChat.vb $
'
' ********************************************************************************
Imports System.Runtime.Serialization

<DataContract()> _
Public Class currentChat

  <DataMember()> _
  Public Property ChatID() As Integer
    Get
      Return m_ChatID
    End Get
    Private Set(ByVal value As Integer)
      m_ChatID = value
    End Set
  End Property
  Private m_ChatID As Integer

  <DataMember()> _
  Public Property SessionUID() As Integer
    Get
      Return m_SessionUID
    End Get
    Private Set(ByVal value As Integer)
      m_SessionUID = value
    End Set
  End Property
  Private m_SessionUID As Integer

  <DataMember()> _
  Public Property AliasUID() As Integer
    Get
      Return m_AliasUID
    End Get
    Private Set(ByVal value As Integer)
      m_AliasUID = value
    End Set
  End Property
  Private m_AliasUID As Integer

  Public Property ChatTimeStamp() As DateTime
    Get
      Return m_ChatTimeStamp
    End Get
    Private Set(ByVal value As DateTime)
      m_ChatTimeStamp = value
    End Set
  End Property
  Private m_ChatTimeStamp As DateTime

  Public Sub New(ByVal currentChat As tblCurrentChat)

    If currentChat IsNot Nothing Then

      ChatID = currentChat.ChatID
      SessionUID = currentChat.chatSessionUID
      AliasUID = currentChat.chatAliasSessionUID
      ChatTimeStamp = currentChat.chatStartTime

    End If

  End Sub

End Class
