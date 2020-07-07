' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/chat/Logic/NotifyUser.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:44a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: NotifyUser.vb $
'
' ********************************************************************************

Imports System.Runtime.Serialization

<DataContract()> _
Public Class NotifyUser

  <DataMember()> _
  Public Property NotifyStatus() As String
    Get
      Return m_notifyStatus
    End Get
    Private Set(ByVal value As String)
      m_notifyStatus = value
    End Set
  End Property
  Private m_notifyStatus As String

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
  Public Property FromAlias() As String
    Get
      Return m_fromAlias
    End Get
    Private Set(ByVal value As String)
      m_fromAlias = value
    End Set
  End Property
  Private m_fromAlias As String

  <DataMember()> _
  Public Property FromUserUID() As Integer
    Get
      Return m_fromUserUID
    End Get
    Private Set(ByVal value As Integer)
      m_fromUserUID = value
    End Set
  End Property
  Private m_fromUserUID As Integer

  <DataMember()> _
  Public Property FromUserName() As String
    Get
      Return m_FromUserName
    End Get
    Private Set(ByVal value As String)
      m_FromUserName = value
    End Set
  End Property
  Private m_FromUserName As String

  <DataMember()> _
  Public Property FromUserCompanyName() As String
    Get
      Return m_FromUserCompanyName
    End Get
    Private Set(ByVal value As String)
      m_FromUserCompanyName = value
    End Set
  End Property
  Private m_FromUserCompanyName As String

  <DataMember()> _
  Public Property FromUserContactID() As String
    Get
      Return m_FromUserContactID
    End Get
    Private Set(ByVal value As String)
      m_FromUserContactID = value
    End Set
  End Property
  Private m_FromUserContactID As String

  <DataMember()> _
  Public Property FromUserCompanyID() As String
    Get
      Return m_FromUserCompanyID
    End Get
    Private Set(ByVal value As String)
      m_FromUserCompanyID = value
    End Set
  End Property
  Private m_FromUserCompanyID As String

  <DataMember()> _
  Public Property ToAlias() As String
    Get
      Return m_toAlias
    End Get
    Private Set(ByVal value As String)
      m_toAlias = value
    End Set
  End Property
  Private m_toAlias As String

  <DataMember()> _
  Public Property ToUserUID() As Integer
    Get
      Return m_toUserUID
    End Get
    Private Set(ByVal value As Integer)
      m_toUserUID = value
    End Set
  End Property
  Private m_toUserUID As Integer

  <DataMember()> _
  Public Property ToUserName() As String
    Get
      Return m_ToUserName
    End Get
    Private Set(ByVal value As String)
      m_ToUserName = value
    End Set
  End Property
  Private m_ToUserName As String

  <DataMember()> _
  Public Property ToUserCompanyName() As String
    Get
      Return m_ToUserCompanyName
    End Get
    Private Set(ByVal value As String)
      m_ToUserCompanyName = value
    End Set
  End Property
  Private m_ToUserCompanyName As String

  <DataMember()> _
  Public Property ToUserContactID() As String
    Get
      Return m_ToUserContactID
    End Get
    Private Set(ByVal value As String)
      m_ToUserContactID = value
    End Set
  End Property
  Private m_ToUserContactID As String

  <DataMember()> _
  Public Property ToUserCompanyID() As String
    Get
      Return m_ToUserCompanyID
    End Get
    Private Set(ByVal value As String)
      m_ToUserCompanyID = value
    End Set
  End Property
  Private m_ToUserCompanyID As String

  <DataMember()> _
  Public Property NotifyID() As Integer
    Get
      Return m_NotifyID
    End Get
    Private Set(ByVal value As Integer)
      m_NotifyID = value
    End Set
  End Property
  Private m_NotifyID As Integer

  Public Property NotifyTimeStamp() As DateTime
    Get
      Return m_NotifyTimeStamp
    End Get
    Private Set(ByVal value As DateTime)
      m_NotifyTimeStamp = value
    End Set
  End Property
  Private m_NotifyTimeStamp As DateTime

  Public Sub New(ByVal notify As tblNotify)

    If notify IsNot Nothing Then

      NotifyID = notify.NotifyID
      NotifyTimeStamp = notify.notifiedTime
      NotifyStatus = notify.notifyStatus
      RoomID = notify.ChatRoomID

      ToUserUID = notify.toUserUID
      ToAlias = notify.toUserAlias

      ToUserName = notify.tblSession1.FriendlyName
      ToUserCompanyName = notify.tblSession1.ComapnyName
      ToUserCompanyID = notify.tblSession1.companyID
      ToUserContactID = notify.tblSession1.contactID

      FromUserUID = notify.fromUserUID
      FromAlias = notify.fromUserAlias

      FromUserName = notify.tblSession.FriendlyName
      FromUserCompanyName = notify.tblSession.ComapnyName
      FromUserCompanyID = notify.tblSession.companyID
      FromUserContactID = notify.tblSession.contactID

    End If

  End Sub

End Class
