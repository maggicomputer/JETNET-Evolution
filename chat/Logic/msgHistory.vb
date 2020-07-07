' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/chat/Logic/msgHistory.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:44a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: msgHistory.vb $
'
' ********************************************************************************

Imports System.Runtime.Serialization

<DataContract()> _
Public Class msgHistory
  <DataMember()> _
  Public Property messageID() As Integer
    Get
      Return m_messageID
    End Get
    Private Set(ByVal value As Integer)
      m_messageID = value
    End Set
  End Property
  Private m_messageID As Integer

  <DataMember()> _
  Public Property messageBody() As String
    Get
      Return m_messageBody
    End Get
    Private Set(ByVal value As String)
      m_messageBody = value
    End Set
  End Property
  Private m_messageBody As String

  <DataMember()> _
  Public Property messageDate() As Date
    Get
      Return m_messageDate
    End Get
    Private Set(ByVal value As Date)
      m_messageDate = value
    End Set
  End Property
  Private m_messageDate As Date

  <DataMember()> _
  Public Property talkerUID() As Integer
    Get
      Return m_talkerUID
    End Get
    Private Set(ByVal value As Integer)
      m_talkerUID = value
    End Set
  End Property
  Private m_talkerUID As Integer

  <DataMember()> _
  Public Property talkerUserName() As String
    Get
      Return m_talkerUserName
    End Get
    Private Set(ByVal value As String)
      m_talkerUserName = value
    End Set
  End Property
  Private m_talkerUserName As String

  <DataMember()> _
  Public Property talkerContactID() As String
    Get
      Return m_talkerContactID
    End Get
    Private Set(ByVal value As String)
      m_talkerContactID = value
    End Set
  End Property
  Private m_talkerContactID As String

  <DataMember()> _
  Public Property talkerDeleted() As Integer
    Get
      Return m_talkerDeleted
    End Get
    Private Set(ByVal value As Integer)
      m_talkerDeleted = value
    End Set
  End Property
  Private m_talkerDeleted As Integer

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

  Public Sub New(ByVal hMessage As tblMessageHistory, ByVal context As HttpContext)

    If hMessage IsNot Nothing Then

      messageID = hMessage.messageID
      messageBody = hMessage.messageBody
      messageDate = hMessage.messageDate

      talkerUID = hMessage.talkerID
      talkerUserName = hMessage.tblSession1.FriendlyName
      talkerContactID = hMessage.tblSession1.contactID

      talkerDeleted = hMessage.talkerDeleted

      IsFriend = (hMessage.tblSession1.SessionID <> context.Session.SessionID)

    End If

  End Sub

End Class
