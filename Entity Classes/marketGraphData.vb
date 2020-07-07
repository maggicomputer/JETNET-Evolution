Imports Microsoft.VisualBasic
Imports System.ComponentModel

' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/marketGraphData.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:49a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: marketGraphData.vb $
'
' ********************************************************************************

<System.Serializable(), FlagsAttribute()> Public Enum eGraphLinkType As Integer
  NULL = 0

  ' available market summary graph links
  AV_FORSALE = 1
  AV_FORSALE_EU = 2
  AV_FORSALE_BKR = 3
  AV_FORSALE_DLR = 4
  AV_FORSALE_DOM = 5
  AV_FORSALE_FOR = 6
  AV_AVG_ASKING = 7
  AV_HIGH_ASKING = 8
  AV_LOW_ASKING = 9
  AV_MAKE_OFFER = 10
  AV_AVG_YEAR = 11
  AV_AVG_AFTT = 12
  AV_AVG_ENTT = 13
  AV_NEW_TO_MARKET = 14

  ' transaction summary graph links
  WS_AVG_ASKING = 15
  WS_HIGH_ASKING = 16
  WS_LOW_ASKING = 17
  WS_NEW_SALES = 18
  WS_MAKE_OFFER = 19
  WS_AVG_YEAR = 20
  WS_AVG_DAYSONMARKET = 21

  WS_TOTAL_TX = 22
  WS_INTERNAL_TX = 23

  OM_TOTAL_TX = 24
  MA_TOTAL_TX = 25
  WO_TOTAL_TX = 26

  DP_TOTAL_TX = 27
  DP_INTERNAL_TX = 28

  FS_TOTAL_TX = 29
  FS_INTERNAL_TX = 30

  SS_TOTAL_TX = 31
  SS_INTERNAL_TX = 32

  FC_TOTAL_TX = 33
  FC_INTERNAL_TX = 34

  LS_TOTAL_TX = 35
  LS_INTERNAL_TX = 36

  SZ_TOTAL_TX = 37
  SZ_INTERNAL_TX = 38

  ' available market summary graph links
  AV_INOPERATION = 39
  AV_INOPERATIONFORSALE = 40

End Enum

<System.Serializable()> Public Class marketGraphData
  Private aError As String
  Private clientConnectString As String
  Private adminConnectString As String

  Private starConnectString As String
  Private cloudConnectString As String
  Private serverConnectString As String

  Private _mkt_link_ref As eGraphLinkType
  Private _mkt_x_title As String
  Private _mkt_y_title As String
  Private _mkt_x_data As String
  Private _mkt_y_data As String
  Private _mkt_graph_title As String

  Sub New()
    aError = ""
    clientConnectString = ""
    adminConnectString = ""

    starConnectString = ""
    cloudConnectString = ""
    serverConnectString = ""

    _mkt_link_ref = eGraphLinkType.NULL

    _mkt_x_title = ""
    _mkt_y_title = ""
    _mkt_x_data = ""
    _mkt_y_data = ""
    _mkt_graph_title = ""

  End Sub

#Region "database_connections"

  Public Property class_error() As String
    Get
      class_error = aError
    End Get
    Set(ByVal value As String)
      aError = value
    End Set
  End Property

  Public Property adminConnectStr() As String
    Get
      adminConnectStr = adminConnectString
    End Get
    Set(ByVal value As String)
      adminConnectString = value
    End Set
  End Property

  Public Property clientConnectStr() As String
    Get
      clientConnectStr = clientConnectString
    End Get
    Set(ByVal value As String)
      clientConnectString = value
    End Set
  End Property

  Public Property starConnectStr() As String
    Get
      starConnectStr = starConnectString
    End Get
    Set(ByVal value As String)
      starConnectString = value
    End Set
  End Property

  Public Property cloudConnectStr() As String
    Get
      cloudConnectStr = cloudConnectString
    End Get
    Set(ByVal value As String)
      cloudConnectString = value
    End Set
  End Property

  Public Property serverConnectStr() As String
    Get
      serverConnectStr = serverConnectString
    End Get
    Set(ByVal value As String)
      serverConnectString = value
    End Set
  End Property

#End Region

#Region "public_properties"

  Public Property marketGraph_LinkType() As eGraphLinkType
    Get
      Return _mkt_link_ref
    End Get
    Set(ByVal value As eGraphLinkType)
      _mkt_link_ref = value
    End Set
  End Property

  Public Property marketGraph_X_title() As String
    Get
      Return _mkt_x_title
    End Get
    Set(ByVal value As String)
      _mkt_x_title = value
    End Set
  End Property

  Public Property marketGraph_Y_title() As String
    Get
      Return _mkt_y_title
    End Get
    Set(ByVal value As String)
      _mkt_y_title = value
    End Set
  End Property

  Public Property marketGraph_X_data() As String
    Get
      Return _mkt_x_data
    End Get
    Set(ByVal value As String)
      _mkt_x_data = value
    End Set
  End Property

  Public Property marketGraph_Y_data() As String
    Get
      Return _mkt_y_data
    End Get
    Set(ByVal value As String)
      _mkt_y_data = value
    End Set
  End Property

  Public Property marketGraph_topTitle() As String
    Get
      Return _mkt_graph_title
    End Get
    Set(ByVal value As String)
      _mkt_graph_title = value
    End Set
  End Property

#End Region

End Class
