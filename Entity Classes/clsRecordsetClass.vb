Imports Microsoft.VisualBasic
Imports System.ComponentModel

' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/clsRecordsetClass.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:48a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: clsRecordsetClass.vb $
'
' ********************************************************************************

<System.Serializable(), FlagsAttribute()> Public Enum eRecordsetTypes As Integer

  NULL = 0

  aircraftSearchList = 1 ' acSearchListRecordset
  transSearchDetail = 2 ' TransactionDetailRecordset
  transSearchList = 4 ' TransactionListRecordset
  eventSearchDetail = 8 ' eventAirCompanyRecordset
  eventSearchList = 16 ' eventListRecordset

  modelVwRetailSale = 32 ' modelForSaleRecordset
  modelVwForSale = 64 ' modelEventsRecordset
  modelVwEvents = 128 ' modelRetailSaleRecordset

  fractVwProviderSales = 256 ' fractProviderSalesRecordset
  fractVwBackSales = 512 ' fractBackSalesRecordset 
  fractVwFleet = 1024 ' fractFleetRecordset

  locationVwAircraft = 2048 ' aircraftLocationRecordset

  leaseVwDueToExpire = 4096 ' leasesDueToExpireRs
  leaseVwExpired = 8192 ' leasesExpiredRs

  transVwDocuments = 16384 ' transactionDocumentsRs

  forsaleVwRetailSale = 32768 ' modelForSaleViewRecordset

  opCostsSearchList = 65536 ' opCostsReportRecordset

End Enum

<System.Serializable(), FlagsAttribute()> Public Enum eFromViewTypes As Integer

  NULL = 0

  A = 1 ' aircraft search list ( by ac_id)
  H = 2 ' Transaction search list ( by ac_id and journ_id )
  HC = 4 ' Transaction search list detail ( by serial no and ac_id )
  EC = 8 ' event search list and event detail list  (by serial and ac_id)

  DF = 16 ' model view ForSale list
  DE = 32 ' model view Events list
  DR = 64 ' model view RetailSale list

  FP = 128 ' fract view ProviderSales list 
  FB = 256 ' fract view BackSales list 
  FF = 512 ' fract view Fleet list

  AL = 1024 ' aircraft Location view list

  LD = 2048 ' lease view DueToExpire list

  LX = 4096 ' lease view Expired list
  TD = 8192 ' transaction Documents view list

  FV = 16384 ' modelForSale view list

  OP = 32768 ' opCosts Search List

End Enum

<System.Serializable(), FlagsAttribute()> Public Enum eProcessBrowseTypes As Integer

  NULL = 0

  bySerialNumber = 1 ' browse by serial number
  byAircraftID = 2 ' browse by aircraft ID
  byAircraftJournalID = 4 ' browse by aircraft ID and journal ID
  byAircraftModelID = 5 ' browse by aircraft ID and journal ID

End Enum

<System.Serializable()> Public Class clsRecordsetClass

  Private _RecordsetStatusCode As eObjStatusCode
  Private _RecordsetDetailError As eObjDetailErrorCode
  Private _RecordsetType As eRecordsetTypes
  Private _FromViewType As eFromViewTypes
  Private _RecordsetDatalayerType As eDatalayerTypes
  Private _RecordsetDatabaseType As eDatabaseTypes
  Private _RecordsetConnString As String
  Private _RecordsetQueryString As String
  Private _RecordCount As Integer
  Private _BrowseRecordCount As Integer
  Private _BrowseBy As eProcessBrowseTypes
  Private _BrowseDataSet As DataSet

  Sub New()

    _RecordsetStatusCode = eObjStatusCode.NULL
    _RecordsetDetailError = eObjDetailErrorCode.NULL

    _RecordsetType = eRecordsetTypes.NULL
    _FromViewType = eFromViewTypes.NULL
    _BrowseBy = eProcessBrowseTypes.NULL

    _RecordsetDatabaseType = eDatabaseTypes.NULL
    _RecordsetDatalayerType = eDatalayerTypes.NULL

    _RecordsetConnString = ""
    _RecordsetQueryString = ""
    _RecordCount = 0
    _BrowseRecordCount = 0

    _BrowseDataSet = Nothing

  End Sub

  Public Function RecordsetTypeName(ByVal rsTypeName As eRecordsetTypes) As String

    Dim tmpStr As String = ""
    Dim recordsetNameTypes As Type = GetType(eRecordsetTypes)

    Return [Enum].GetName(recordsetNameTypes, rsTypeName)

  End Function

  Public Function RecordsetBrowseBy(ByVal browseByTypeName As eProcessBrowseTypes) As String

    Dim tmpStr As String = ""
    Dim browseByNameTypes As Type = GetType(eProcessBrowseTypes)

    Return [Enum].GetName(browseByNameTypes, browseByTypeName)

  End Function

  Public Function RecordsetFromViewTypeName(ByVal fromViewTypeName As eFromViewTypes) As String

    Dim tmpStr As String = ""
    Dim fromViewNameTypes As Type = GetType(eFromViewTypes)

    Return [Enum].GetName(fromViewNameTypes, fromViewTypeName)

  End Function

  Public Function RecordsetDataBaseTypeName(ByVal dbType As eDatabaseTypes) As String

    Dim tmpStr As String = ""
    Dim dbTypes As Type = GetType(eDatabaseTypes)

    Return [Enum].GetName(dbTypes, dbType)

  End Function

  Public Function RecordsetDataLayerTypeName(ByVal dataLayerType As eDatalayerTypes) As String

    Dim tmpStr As String = ""
    Dim dataLayerTypes As Type = GetType(eDatalayerTypes)

    Return [Enum].GetName(dataLayerTypes, dataLayerType)

  End Function

  Public Property RecordsetStatusCode() As eObjStatusCode
    Get
      Return _RecordsetStatusCode
    End Get
    Set(ByVal value As eObjStatusCode)
      _RecordsetStatusCode = value
    End Set
  End Property

  Public Property RecordsetDetailError() As eObjDetailErrorCode
    Get
      Return _RecordsetDetailError
    End Get
    Set(ByVal value As eObjDetailErrorCode)
      _RecordsetDetailError = value
    End Set
  End Property

  Public Property RecordsetType() As eRecordsetTypes
    Get
      Return _RecordsetType
    End Get
    Set(ByVal value As eRecordsetTypes)
      _RecordsetType = value
    End Set
  End Property

  Public Property RecordsetfromView() As eFromViewTypes
    Get
      Return _FromViewType
    End Get
    Set(ByVal value As eFromViewTypes)
      _FromViewType = value
    End Set
  End Property

  Public Property BrowseRecordsetBy() As eProcessBrowseTypes
    Get
      Return _BrowseBy
    End Get
    Set(ByVal value As eProcessBrowseTypes)
      _BrowseBy = value
    End Set
  End Property

  Public Property RecordsetDatabaseType() As eDatabaseTypes
    Get
      Return _RecordsetDatabaseType
    End Get
    Set(ByVal value As eDatabaseTypes)
      _RecordsetDatabaseType = value
    End Set
  End Property

  Public Property RecordsetDatalayerType() As eDatalayerTypes
    Get
      Return _RecordsetDatalayerType
    End Get
    Set(ByVal value As eDatalayerTypes)
      _RecordsetDatalayerType = value
    End Set
  End Property

  Public Property RecordsetConnString() As String
    Get
      Return _RecordsetConnString
    End Get
    Set(ByVal value As String)
      _RecordsetConnString = value
    End Set
  End Property

  Public Property RecordsetQueryString() As String
    Get
      Return _RecordsetQueryString
    End Get
    Set(ByVal value As String)
      _RecordsetQueryString = value
    End Set
  End Property

  Public Property RecordCount() As Integer
    Get
      Return _RecordCount
    End Get
    Set(ByVal value As Integer)
      _RecordCount = value
    End Set
  End Property

  Public Property BrowseRecordCount() As Integer
    Get
      Return _BrowseRecordCount
    End Get
    Set(ByVal value As Integer)
      _BrowseRecordCount = value
    End Set
  End Property

  Public Property BrowseDataSet() As DataSet
    Get
      Return _BrowseDataSet
    End Get
    Set(ByVal value As DataSet)
      _BrowseDataSet = value
    End Set
  End Property

End Class
