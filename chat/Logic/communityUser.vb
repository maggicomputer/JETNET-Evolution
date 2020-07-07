' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/chat/Logic/communityUser.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:44a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: communityUser.vb $
'
' ********************************************************************************
Imports System.Runtime.Serialization

<DataContract()> _
Public Class communityUser

  <DataMember()> _
  Public Property BuddyID() As Integer
    Get
      Return m_BuddyID
    End Get
    Private Set(ByVal value As Integer)
      m_BuddyID = value
    End Set
  End Property
  Private m_BuddyID As Integer

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
  Public Property BuddyUID() As Integer
    Get
      Return m_BuddyUID
    End Get
    Private Set(ByVal value As Integer)
      m_BuddyUID = value
    End Set
  End Property
  Private m_BuddyUID As Integer

  <DataMember()> _
  Public Property BuddyAlias() As String
    Get
      Return m_BuddyAlias
    End Get
    Private Set(ByVal value As String)
      m_BuddyAlias = value
    End Set
  End Property
  Private m_BuddyAlias As String

  <DataMember()> _
  Public Property BuddyName() As String
    Get
      Return m_BuddyName
    End Get
    Private Set(ByVal value As String)
      m_BuddyName = value
    End Set
  End Property
  Private m_BuddyName As String

  <DataMember()> _
  Public Property BuddyComapnyName() As String
    Get
      Return m_BuddyComapnyName
    End Get
    Private Set(ByVal value As String)
      m_BuddyComapnyName = value
    End Set
  End Property
  Private m_BuddyComapnyName As String

  <DataMember()> _
  Public Property BlockBuddy() As Boolean
    Get
      Return m_BlockBuddy
    End Get
    Private Set(ByVal value As Boolean)
      m_BlockBuddy = value
    End Set
  End Property
  Private m_BlockBuddy As Boolean

  <DataMember()> _
  Public Property IgnoreBuddy() As Boolean
    Get
      Return m_IgnoreBuddy
    End Get
    Private Set(ByVal value As Boolean)
      m_IgnoreBuddy = value
    End Set
  End Property
  Private m_IgnoreBuddy As Boolean

  <DataMember()> _
  Public Property IncludeBuddy() As Boolean
    Get
      Return m_IncludeBuddy
    End Get
    Private Set(ByVal value As Boolean)
      m_IncludeBuddy = value
    End Set
  End Property
  Private m_IncludeBuddy As Boolean

  <DataMember()> _
  Public Property IsOnline() As Boolean
    Get
      Return m_IsOnline
    End Get
    Private Set(ByVal value As Boolean)
      m_IsOnline = value
    End Set
  End Property
  Private m_IsOnline As Boolean

  Public Sub New(ByVal communityUser As tblCommunityList)

    BuddyID = communityUser.buddyID

    SessionUID = communityUser.SessionUID
    SessionAlias = communityUser.SessionAlias

    BuddyUID = communityUser.BuddyUID
    BuddyAlias = communityUser.BuddyAlias

    BuddyName = communityUser.tblSession.FriendlyName
    BuddyComapnyName = communityUser.tblSession.ComapnyName

    BlockBuddy = IIf(communityUser.BlockAlias.ToString.Contains("Y"), True, False)
    IgnoreBuddy = IIf(communityUser.IgnoreAlias.ToString.Contains("Y"), True, False)
    IncludeBuddy = IIf(communityUser.IncludeAlias.ToString.Contains("Y"), True, False)

    IsOnline = IIf(DateDiff(DateInterval.Minute, communityUser.tblSession.onLineTime, Now()) < 10, True, False)

  End Sub

End Class
