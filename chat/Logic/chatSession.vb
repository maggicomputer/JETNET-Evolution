' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/chat/Logic/chatSession.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:44a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: chatSession.vb $
'
' ********************************************************************************

Imports System.Runtime.Serialization

<DataContract()> _
Public Class chatSession
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
  Public Property SessionID() As String
    Get
      Return m_SessionID
    End Get
    Private Set(ByVal value As String)
      m_SessionID = value
    End Set
  End Property
  Private m_SessionID As String

  <DataMember()> _
  Public Property SessionIP() As String
    Get
      Return m_SessionIP
    End Get
    Private Set(ByVal value As String)
      m_SessionIP = value
    End Set
  End Property
  Private m_SessionIP As String

  <DataMember()> _
  Public Property SessionAlias() As String
    Get
      Return m_SessionAlias
    End Get
    Private Set(ByVal value As String)
      m_SessionAlias = value
    End Set
  End Property
  Private m_SessionAlias As String

  <DataMember()> _
  Public Property SessionFriendlyName() As String
    Get
      Return m_SessionFriendlyName
    End Get
    Private Set(ByVal value As String)
      m_SessionFriendlyName = value
    End Set
  End Property
  Private m_SessionFriendlyName As String

  <DataMember()> _
  Public Property SessionComapnyName() As String
    Get
      Return m_SessionComapnyName
    End Get
    Private Set(ByVal value As String)
      m_SessionComapnyName = value
    End Set
  End Property
  Private m_SessionComapnyName As String

  <DataMember()> _
  Public Property SessionSubscriptionID() As Long
    Get
      Return m_SessionSubID
    End Get
    Private Set(ByVal value As Long)
      m_SessionSubID = value
    End Set
  End Property
  Private m_SessionSubID As Long

  <DataMember()> _
Public Property SessionUserID() As String
    Get
      Return m_SessionUserID
    End Get
    Private Set(ByVal value As String)
      m_SessionUserID = value
    End Set
  End Property
  Private m_SessionUserID As String

  <DataMember()> _
  Public Property SessionSequenceNum() As Integer
    Get
      Return m_SessionSequenceNum
    End Get
    Private Set(ByVal value As Integer)
      m_SessionSequenceNum = value
    End Set
  End Property
  Private m_SessionSequenceNum As Integer

  Public Property SessionContactID() As Long
    Get
      Return m_SessionContactID
    End Get
    Private Set(ByVal value As Long)
      m_SessionContactID = value
    End Set
  End Property
  Private m_SessionContactID As Long

  Public Property SessionCompanyID() As Long
    Get
      Return m_SessionCompanyID
    End Get
    Private Set(ByVal value As Long)
      m_SessionCompanyID = value
    End Set
  End Property
  Private m_SessionCompanyID As Long

  Public Property SessionOnLine() As Date
    Get
      Return m_SessionOnLine
    End Get
    Private Set(ByVal value As Date)
      m_SessionOnLine = value
    End Set
  End Property
  Private m_SessionOnLine As Date

  Public Sub New(ByVal session As tblSession)

    SessionUID = session.UID
    SessionID = session.SessionID
    SessionIP = session.IP
    SessionAlias = session.UserAlias

    SessionFriendlyName = session.FriendlyName
    SessionComapnyName = session.ComapnyName

    SessionSubscriptionID = session.subscriptionID
    SessionUserID = session.userID
    SessionSequenceNum = session.sequenceNum
    SessionContactID = session.contactID
    SessionCompanyID = session.companyID

    SessionOnLine = session.onLineTime

  End Sub

End Class