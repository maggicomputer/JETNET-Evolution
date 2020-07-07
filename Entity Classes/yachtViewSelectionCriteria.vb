Imports Microsoft.VisualBasic
Imports System.ComponentModel

' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/yachtViewSelectionCriteria.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:50a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: yachtViewSelectionCriteria.vb $
'
' ********************************************************************************

<System.Serializable()> Public Class yachtViewSelectionCriteria

  Private _yachtViewCriteriaStatusCode As eObjStatusCode
  Private _yachtViewCriteriaDetailError As eObjDetailErrorCode

  Private _yachtViewID As Long
  Private _yachtViewName As String

  ' available yacht views selection variables
  Private _yachtViewCriteriaYmodID As Long
  Private _yachtViewCriteriaYmodIDArray As Array
  Private _yachtViewCriteriaBrandID As Long
  Private _yachtViewCriteriaBrandIDArray As Array
  Private _yachtViewCriteriaCategoryID As Long
  Private _yachtViewCriteriaCategoryIDArray As Array

  Private _yachtViewCriteriaYachtMotor As String
  Private _yachtViewCriteriaYachtCategory As String
  Private _yachtViewCriteriaYachtBrand As String
  Private _yachtViewCriteriaYachtModel As String

  Private _yachtViewCriteriaMotorType As Integer

  ' company location variables
  Private _yachtViewCriteriaHasCompanyLocationInfo As Boolean   ' 

  Private _yachtViewCriteriaUseContinent As Boolean   ' toggle to search Continent/Region
  Private _yachtViewCriteriaUseRegion As Boolean

  Private _yachtViewCriteriaContinent As String
  Private _yachtViewCriteriaContinentArray As Array

  Private _yachtViewCriteriaCountry As String
  Private _yachtViewCriteriaCountryArray As Array

  Private _yachtViewCriteriaState As String
  Private _yachtViewCriteriaStateArray As Array

  Private _yachtViewCriteriaCity As String
  Private _yachtViewCriteriaCityArray As Array

  Private _yachtViewCriteriaTimeZone As String
  Private _yachtViewCriteriaTimeZoneArray As Array

  Private _yachtViewCriteriaCountryHasStates As Boolean

  Private _yachtViewCriteriaNoteID As Long
  Private _yachtViewCriteriaNoteUserID As Long
  Private _yachtViewCriteriaNoteCompanyID As Long
  Private _yachtViewCriteriaNoteAircraftID As Long

  Private _yachtViewCriteriaNoteClientID As Long

  Private _yachtViewCriteriaNoteField As String
  Private _yachtViewCriteriaNoteTextValue As String

  Private _yachtViewCriteriaNoteYTSearchTextValue As String
  Private _yachtViewCriteriaNoteYTSearchOperator As Integer
  Private _yachtViewCriteriaNoteYTSearchField As Integer

  Private _yachtViewCriteriaNoteStartDate As String
  Private _yachtViewCriteriaNoteEndDate As String

  Private _yachtViewCriteriaNoteEntryDate As String

  Private _yachtViewCriteriaNoteScheduleStartDate As String
  Private _yachtViewCriteriaNoteScheduleEndDate As String

  Private _yachtViewCriteriaNoteOrderBy As String
  Private _yachtViewCriteriaNoteDocsAttached As String
  Private _yachtViewCriteriaNoteType As String

  Private _yachtViewCriteriaGetAllNotes As Boolean

  Private _yachtViewCriteriaSubID As Long
  Private _yachtViewCriteriaCRMuserID As Long
  Private _yachtViewCriteriaLogin As String

  Private _yachtViewCriteriaIsReport As Boolean

  Private _yachtViewCriteriaYachtID As Long
  Private _yachtViewCriteriaCompanyID As Long


  Sub New()

    _yachtViewCriteriaStatusCode = eObjStatusCode.NULL
    _yachtViewCriteriaDetailError = eObjDetailErrorCode.NULL

    _yachtViewCriteriaYmodID = -1
    _yachtViewCriteriaYmodIDArray = Nothing

    _yachtViewCriteriaBrandID = -1
    _yachtViewCriteriaBrandIDArray = Nothing

    _yachtViewCriteriaCategoryID = -1
    _yachtViewCriteriaCategoryIDArray = Nothing

    _yachtViewCriteriaYachtMotor = ""
    _yachtViewCriteriaYachtCategory = ""
    _yachtViewCriteriaYachtBrand = ""
    _yachtViewCriteriaYachtModel = ""

    _yachtViewCriteriaMotorType = crmWebClient.Constants.VIEW_ALLHULLTYPES

    _yachtViewCriteriaHasCompanyLocationInfo = False

    _yachtViewCriteriaUseContinent = True
    _yachtViewCriteriaUseRegion = False

    _yachtViewCriteriaContinent = ""
    _yachtViewCriteriaCountry = ""
    _yachtViewCriteriaState = ""
    _yachtViewCriteriaCity = ""
    _yachtViewCriteriaTimeZone = ""

    _yachtViewCriteriaContinentArray = Nothing
    _yachtViewCriteriaCountryArray = Nothing
    _yachtViewCriteriaStateArray = Nothing
    _yachtViewCriteriaCityArray = Nothing
    _yachtViewCriteriaTimeZoneArray = Nothing

    _yachtViewCriteriaCountryHasStates = False

    _yachtViewCriteriaNoteID = 0
    _yachtViewCriteriaNoteUserID = 0
    _yachtViewCriteriaNoteCompanyID = 0
    _yachtViewCriteriaNoteAircraftID = 0

    _yachtViewCriteriaNoteClientID = 0

    _yachtViewCriteriaNoteField = ""
    _yachtViewCriteriaNoteTextValue = ""

    _yachtViewCriteriaNoteYTSearchTextValue = ""
    _yachtViewCriteriaNoteYTSearchOperator = 0
    _yachtViewCriteriaNoteYTSearchField = 0

    _yachtViewCriteriaNoteStartDate = ""
    _yachtViewCriteriaNoteEndDate = ""

    _yachtViewCriteriaNoteEntryDate = ""

    _yachtViewCriteriaNoteScheduleStartDate = ""
    _yachtViewCriteriaNoteScheduleEndDate = ""

    _yachtViewCriteriaNoteOrderBy = ""
    _yachtViewCriteriaNoteDocsAttached = ""
    _yachtViewCriteriaNoteType = ""

    _yachtViewCriteriaGetAllNotes = False

    _yachtViewCriteriaSubID = 0
    _yachtViewCriteriaCRMuserID = 0
    _yachtViewCriteriaLogin = ""

    _yachtViewCriteriaIsReport = False

    _yachtViewCriteriaYachtID = 0
    _yachtViewCriteriaCompanyID = 0

    _yachtViewID = 0
    _yachtViewName = ""

  End Sub

  Public Property YachtViewSelectionCriteriaStatusCode() As eObjStatusCode
    Get
      Return _yachtViewCriteriaStatusCode
    End Get
    Set(ByVal value As eObjStatusCode)
      _yachtViewCriteriaStatusCode = value
    End Set
  End Property

  Public Property YachtViewSelectionCriteriaDetailError() As eObjDetailErrorCode
    Get
      Return _yachtViewCriteriaDetailError
    End Get
    Set(ByVal value As eObjDetailErrorCode)
      _yachtViewCriteriaDetailError = value
    End Set
  End Property

  Public Property YachtViewCriteriaYmodID() As Long
    Get
      Return _yachtViewCriteriaYmodID
    End Get
    Set(ByVal value As Long)
      _yachtViewCriteriaYmodID = value
    End Set
  End Property

  Public Property YachtViewCriteriaYmodIDArray() As Array
    Get
      Return _yachtViewCriteriaYmodIDArray
    End Get
    Set(ByVal value As Array)
      _yachtViewCriteriaYmodIDArray = value
    End Set
  End Property

  Public Property YachtViewCriteriaBrandID() As Long
    Get
      Return _yachtViewCriteriaBrandID
    End Get
    Set(ByVal value As Long)
      _yachtViewCriteriaBrandID = value
    End Set
  End Property

  Public Property YachtViewCriteriaBrandIDArray() As Array
    Get
      Return _yachtViewCriteriaBrandIDArray
    End Get
    Set(ByVal value As Array)
      _yachtViewCriteriaBrandIDArray = value
    End Set
  End Property


  Public Property YachtViewCriteriaCategoryID() As Long
    Get
      Return _yachtViewCriteriaCategoryID
    End Get
    Set(ByVal value As Long)
      _yachtViewCriteriaCategoryID = value
    End Set
  End Property

  Public Property YachtViewCriteriaCategoryIDArray() As Array
    Get
      Return _yachtViewCriteriaCategoryIDArray
    End Get
    Set(ByVal value As Array)
      _yachtViewCriteriaCategoryIDArray = value
    End Set
  End Property

  Public Property YachtViewCriteriaYachtMotor() As String
    Get
      Return _yachtViewCriteriaYachtMotor
    End Get
    Set(ByVal value As String)
      _yachtViewCriteriaYachtMotor = value
    End Set
  End Property

  Public Property YachtViewCriteriaYachtCategory() As String
    Get
      Return _yachtViewCriteriaYachtCategory
    End Get
    Set(ByVal value As String)
      _yachtViewCriteriaYachtCategory = value
    End Set
  End Property

  Public Property YachtViewCriteriaYachtBrand() As String
    Get
      Return _yachtViewCriteriaYachtBrand
    End Get
    Set(ByVal value As String)
      _yachtViewCriteriaYachtBrand = value
    End Set
  End Property

  Public Property YachtViewCriteriaYachtModel() As String
    Get
      Return _yachtViewCriteriaYachtModel
    End Get
    Set(ByVal value As String)
      _yachtViewCriteriaYachtModel = value
    End Set
  End Property

  Public Property YachtViewCriteriaMotorType() As Integer
    Get
      Return _yachtViewCriteriaMotorType
    End Get
    Set(ByVal value As Integer)
      _yachtViewCriteriaMotorType = value
    End Set
  End Property

  Public Property YachtViewCriteriaIsReport() As Boolean
    Get
      Return _yachtViewCriteriaIsReport
    End Get
    Set(ByVal value As Boolean)
      _yachtViewCriteriaIsReport = value
    End Set
  End Property

  Public Property YachtViewCriteriaYachtID() As Long  '
    Get
      Return _yachtViewCriteriaYachtID
    End Get
    Set(ByVal value As Long)
      _yachtViewCriteriaYachtID = value
    End Set
  End Property

  Public Property YachtViewCriteriaNoteYTSearchTextValue() As String
    Get
      Return _yachtViewCriteriaNoteYTSearchTextValue
    End Get
    Set(ByVal value As String)
      _yachtViewCriteriaNoteYTSearchTextValue = value
    End Set
  End Property

  Public Property YachtViewCriteriaNoteYTSearchOperator() As Integer
    Get
      Return _yachtViewCriteriaNoteYTSearchOperator
    End Get
    Set(ByVal value As Integer)
      _yachtViewCriteriaNoteYTSearchOperator = value
    End Set
  End Property

  Public Property YachtViewCriteriaNoteYTSearchField() As Integer
    Get
      Return _yachtViewCriteriaNoteYTSearchField
    End Get
    Set(ByVal value As Integer)
      _yachtViewCriteriaNoteYTSearchField = value
    End Set
  End Property

  Public Property YachtViewCriteriaNoteField() As String
    Get
      Return _yachtViewCriteriaNoteField
    End Get
    Set(ByVal value As String)
      _yachtViewCriteriaNoteField = value
    End Set
  End Property

  Public Property YachtViewCriteriaNoteTextValue() As String
    Get
      Return _yachtViewCriteriaNoteTextValue
    End Get
    Set(ByVal value As String)
      _yachtViewCriteriaNoteTextValue = value
    End Set
  End Property

  Public Property YachtViewCriteriaNoteStartDate() As String
    Get
      Return _yachtViewCriteriaNoteStartDate
    End Get
    Set(ByVal value As String)
      _yachtViewCriteriaNoteStartDate = value
    End Set
  End Property

  Public Property YachtViewCriteriaNoteEndDate() As String
    Get
      Return _yachtViewCriteriaNoteEndDate
    End Get
    Set(ByVal value As String)
      _yachtViewCriteriaNoteEndDate = value
    End Set
  End Property

  Public Property YachtViewCriteriaNoteEntryDate() As String
    Get
      Return _yachtViewCriteriaNoteEntryDate
    End Get
    Set(ByVal value As String)
      _yachtViewCriteriaNoteEntryDate = value
    End Set
  End Property

  Public Property YachtViewCriteriaNoteScheduleStartDate() As String
    Get
      Return _yachtViewCriteriaNoteScheduleStartDate
    End Get
    Set(ByVal value As String)
      _yachtViewCriteriaNoteScheduleStartDate = value
    End Set
  End Property

  Public Property YachtViewCriteriaNoteScheduleEndDate() As String
    Get
      Return _yachtViewCriteriaNoteScheduleEndDate
    End Get
    Set(ByVal value As String)
      _yachtViewCriteriaNoteScheduleEndDate = value
    End Set
  End Property

  Public Property YachtViewCriteriaNoteOrderBy() As String
    Get
      Return _yachtViewCriteriaNoteOrderBy
    End Get
    Set(ByVal value As String)
      _yachtViewCriteriaNoteOrderBy = value
    End Set
  End Property

  Public Property YachtViewCriteriaNoteDocsAttached() As String
    Get
      Return _yachtViewCriteriaNoteDocsAttached
    End Get
    Set(ByVal value As String)
      _yachtViewCriteriaNoteDocsAttached = value
    End Set
  End Property

  Public Property YachtViewCriteriaNoteType() As String
    Get
      Return _yachtViewCriteriaNoteType
    End Get
    Set(ByVal value As String)
      _yachtViewCriteriaNoteType = value
    End Set
  End Property

  Public Property YachtViewCriteriaNoteID() As Long
    Get
      Return _yachtViewCriteriaNoteID
    End Get
    Set(ByVal value As Long)
      _yachtViewCriteriaNoteID = value
    End Set
  End Property

  Public Property YachtViewCriteriaNoteUserID() As Long
    Get
      Return _yachtViewCriteriaNoteUserID
    End Get
    Set(ByVal value As Long)
      _yachtViewCriteriaNoteUserID = value
    End Set
  End Property

  Public Property YachtViewCriteriaNoteCompanyID() As Long
    Get
      Return _yachtViewCriteriaNoteCompanyID
    End Get
    Set(ByVal value As Long)
      _yachtViewCriteriaNoteCompanyID = value
    End Set
  End Property

  Public Property YachtViewCriteriaNoteAircraftID() As Long
    Get
      Return _yachtViewCriteriaNoteAircraftID
    End Get
    Set(ByVal value As Long)
      _yachtViewCriteriaNoteAircraftID = value
    End Set
  End Property

  Public Property YachtViewCriteriaNoteClientID() As Long
    Get
      Return _yachtViewCriteriaNoteClientID
    End Get
    Set(ByVal value As Long)
      _yachtViewCriteriaNoteClientID = value
    End Set
  End Property

  Public Property YachtViewCriteriaGetAllNotes() As Boolean
    Get
      Return _yachtViewCriteriaGetAllNotes
    End Get
    Set(ByVal value As Boolean)
      _yachtViewCriteriaGetAllNotes = value
    End Set
  End Property

  Public Property YachtViewCriteriaSubID() As Long
    Get
      Return _yachtViewCriteriaSubID
    End Get
    Set(ByVal value As Long)
      _yachtViewCriteriaSubID = value
    End Set
  End Property

  Public Property YachtViewCriteriaCRMuserID() As Long
    Get
      Return _yachtViewCriteriaCRMuserID
    End Get
    Set(ByVal value As Long)
      _yachtViewCriteriaCRMuserID = value
    End Set
  End Property

  Public Property YachtViewCriteriaLogin() As String
    Get
      Return _yachtViewCriteriaLogin
    End Get
    Set(ByVal value As String)
      _yachtViewCriteriaLogin = value
    End Set
  End Property

  Public Property YachtViewCriteriaContinentArray() As Array
    Get
      Return _yachtViewCriteriaContinentArray
    End Get
    Set(ByVal value As Array)
      _yachtViewCriteriaContinentArray = value
    End Set
  End Property

  Public Property YachtViewCriteriaCountryArray() As Array
    Get
      Return _yachtViewCriteriaCountryArray
    End Get
    Set(ByVal value As Array)
      _yachtViewCriteriaCountryArray = value
    End Set
  End Property

  Public Property YachtViewCriteriaStateArray() As Array
    Get
      Return _yachtViewCriteriaStateArray
    End Get
    Set(ByVal value As Array)
      _yachtViewCriteriaStateArray = value
    End Set
  End Property

  Public Property YachtViewCriteriaCityArray() As Array
    Get
      Return _yachtViewCriteriaCityArray
    End Get
    Set(ByVal value As Array)
      _yachtViewCriteriaCityArray = value
    End Set
  End Property

  Public Property YachtViewCriteriaTimeZoneArray() As Array
    Get
      Return _yachtViewCriteriaTimeZoneArray
    End Get
    Set(ByVal value As Array)
      _yachtViewCriteriaTimeZoneArray = value
    End Set
  End Property

  'company location properties
  Public Property YachtViewCriteriaHasCompanyLocationInfo() As Boolean
    Get
      Return _yachtViewCriteriaHasCompanyLocationInfo
    End Get
    Set(ByVal value As Boolean)
      _yachtViewCriteriaHasCompanyLocationInfo = value
    End Set
  End Property

  Public Property YachtViewCriteriaUseContinent() As Boolean
    Get
      Return _yachtViewCriteriaUseContinent
    End Get
    Set(ByVal value As Boolean)
      _yachtViewCriteriaUseContinent = value
    End Set
  End Property

  Public Property YachtViewCriteriaUseRegion() As Boolean
    Get
      Return _yachtViewCriteriaUseRegion
    End Get
    Set(ByVal value As Boolean)
      _yachtViewCriteriaUseRegion = value
    End Set
  End Property

  Public Property YachtViewCriteriaContinent() As String
    Get
      Return _yachtViewCriteriaContinent
    End Get
    Set(ByVal value As String)
      _yachtViewCriteriaContinent = value
    End Set
  End Property

  Public Property YachtViewCriteriaCountry() As String
    Get
      Return _yachtViewCriteriaCountry
    End Get
    Set(ByVal value As String)
      _yachtViewCriteriaCountry = value
    End Set
  End Property

  Public Property YachtViewCriteriaState() As String
    Get
      Return _yachtViewCriteriaState
    End Get
    Set(ByVal value As String)
      _yachtViewCriteriaState = value
    End Set
  End Property

  Public Property YachtViewCriteriaCity() As String
    Get
      Return _yachtViewCriteriaCity
    End Get
    Set(ByVal value As String)
      _yachtViewCriteriaCity = value
    End Set
  End Property

  Public Property YachtViewCriteriaTimeZone() As String
    Get
      Return _yachtViewCriteriaTimeZone
    End Get
    Set(ByVal value As String)
      _yachtViewCriteriaTimeZone = value
    End Set
  End Property

  Public Property YachtViewCriteriaCountryHasStates() As Boolean
    Get
      Return _yachtViewCriteriaCountryHasStates
    End Get
    Set(ByVal value As Boolean)
      _yachtViewCriteriaCountryHasStates = value
    End Set
  End Property

  Public Property YachtViewCriteriaCompanyID() As Long
    Get
      Return _yachtViewCriteriaCompanyID
    End Get
    Set(ByVal value As Long)
      _yachtViewCriteriaCompanyID = value
    End Set
  End Property

  Public Property YachtViewID() As Long
    Get
      Return _yachtViewID
    End Get
    Set(ByVal value As Long)
      _yachtViewID = value
    End Set
  End Property

  Public Property YachtViewName() As String
    Get
      Return _yachtViewName
    End Get
    Set(ByVal value As String)
      _yachtViewName = value
    End Set
  End Property

End Class
