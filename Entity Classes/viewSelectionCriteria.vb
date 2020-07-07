Imports Microsoft.VisualBasic
Imports System.ComponentModel

' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/viewSelectionCriteria.vb $
'$$Author: Matt $
'$$Date: 3/16/20 10:54a $
'$$Modtime: 3/10/20 4:31p $
'$$Revision: 3 $
'$$Workfile: viewSelectionCriteria.vb $
'
' ********************************************************************************

<System.Serializable()> Public Class viewSelectionCriteriaClass

  Private _viewCriteriaStatusCode As eObjStatusCode
  Private _viewCriteriaDetailError As eObjDetailErrorCode

  ' available views selection variables
  Private _viewCriteriaAmodID As Long
  Private _viewCriteriaAmodIDArray As Array
  Private _viewTempAmodID As Long

  Private _viewCriteriaAmodID1 As String  ' used for second model selection
  Private _viewCriteriaAmodID2 As String  ' used for third model selection

  Private _viewCriteriaMakeID As Long
  Private _viewCriteriaMakeIDArray As Array

  Private _viewCriteriaTypeID As Long
  Private _viewCriteriaTypeIDArray As Array

  Private _viewCriteriaWeightClass As String

  ' company location variables
  Private _viewCriteriaHasCompanyLocationInfo As Boolean   ' 

  Private _viewCriteriaUseContinent As Boolean   ' toggle to search Continent/Region
  Private _viewCriteriaUseRegion As Boolean

  Private _ViewCriteriaContinent As String
  Private _viewCriteriaContinentArray As Array

  Private _viewCriteriaCountry As String
  Private _viewCriteriaCountryArray As Array

  Private _viewCriteriaState As String
  Private _viewCriteriaStateArray As Array

  Private _viewCriteriaCity As String
  Private _viewCriteriaCityArray As Array

  Private _viewCriteriaTimeZone As String
  Private _viewCriteriaTimeZoneArray As Array

  Private _viewCriteriaCountryHasStates As Boolean

  ' aircraft base location variables
  Private _viewCriteriaHasBaseLocationInfo As Boolean   ' 

  Private _viewCriteriaBaseUseContinent As Boolean ' toggle to search Continent/Region
  Private _viewCriteriaBaseUseRegion As Boolean

  Private _ViewCriteriaBaseContinent As String
  Private _viewCriteriaBaseContinentArray As Array

  Private _viewCriteriaBaseCountry As String
  Private _viewCriteriaBaseCountryArray As Array

  Private _viewCriteriaBaseState As String
  Private _viewCriteriaBaseStateArray As Array

  Private _viewCriteriaBaseCity As String
  Private _viewCriteriaBaseCityArray As Array

  Private _viewCriteriaBaseCountryHasStates As Boolean


  Private _viewCriteriaProductType As Integer
  Private _viewCriteriaAirframeType As Integer

  Private _viewCriteriaTimeSpan As Integer

  Private _viewCriteriaAirframeTypeStr As String
  Private _viewCriteriaAircraftType As String
  Private _viewCriteriaAircraftMake As String
  Private _viewCriteriaAircraftModel As String
  Private _viewCriteriaAircraftModel1 As String ' used for second model selection (model name)
  Private _viewCriteriaAircraftModel2 As String ' used for third model selection (model name)

  ' available views properties
  Private _viewCriteriaLocationViewType As Integer
  Private _viewCriteriaLocationViewSort As Integer

  Private _viewCriteriaUseMetricValues As Boolean
  Private _viewCriteriaUseStatuteMiles As Boolean

  Private _viewCriteriaSortBy As String
  Private _viewCriteriaIsReport As Boolean
  Private _viewCriteriaNoLocalNotes As Boolean ' used as general flag(user has no cloud notes)(still could have server notes)
  Private _viewCriteriaShowInternal As Boolean

  ' available views tempory values
  Private _viewCriteriaAircraftRange As Double

  Private _viewCriteriaHeliRangeTanksFull As Double
  Private _viewCriteriaHeliRangeSeatsFull As Double
  Private _viewCriteriaAircraftFieldLength As Long

  Private _viewCriteriaAircraftID As Long
  Private _viewCriteriaYachtID As Long
  Private _viewCriteriaJournalID As Long
  Private _viewCriteriaCompanyID As Long
  Private _viewTempCompanyID As Long

  Private _viewCriteriaSubID As Long
  Private _viewCriteriaCRMuserID As Long
  Private _viewCriteriaLogin As String

  Private _viewCriteriaGetExclusive As Boolean
  Private _viewCriteriaGetOperator As Boolean

  Private _viewCriteriaDocumentsStartDate As String
  Private _viewCriteriaDocumentsEndDate As String

  Private _viewCriteriaDocumentsTxType As String
  Private _viewCriteriaDocumentType As String

  Private _viewCriteriaAirportIATA As String
  Private _viewCriteriaAirportICAO As String
  Private _viewCriteriaAirportName As String
  Private _viewCriteriaAirportLatitude As Double
  Private _viewCriteriaAirportLongitude As Double

  Private _viewCriteriaEngineName As String

  Private _viewCriteriaFractionalProgramID As Long
  Private _viewCriteriaFractionalProgramName As String

  ' star report variables
  Private _viewCriteriaStarReportID As Integer
  Private _viewCriteriaStarReportType As String
  Private _viewCriteriaStarReportDate As String
  Private _viewCriteriaStarReportCatagory As String

  ' star report variables
  Private _viewCriteriaSPIWeightClass As String
  Private _viewCriteriaSPIAirframe As Integer

  Private _viewCriteriaSPIYearSld1 As String
  Private _viewCriteriaSPIselectedYearSld1 As String
  Private _viewCriteriaSPIYearSld2 As String
  Private _viewCriteriaSPIYearQtr1 As String
  Private _viewCriteriaSPIYearQtr1Name As String

  Private _viewCriteriaStarReportYear As String
  Private _viewCriteriaStarReportPrefix As String
  Private _viewCriteriaStarReportSuffix As String

  Private _viewCriteriaHasHelicopterFlag As Boolean
  Private _viewCriteriaHasBusinessFlag As Boolean
  Private _viewCriteriaHasCommercialFlag As Boolean
  Private _viewCriteriaHasRegionalFlag As Boolean
  Private _viewCriteriaHasYachtFlag As Boolean

  Private _viewID As Long
  Private _viewName As String

  Private _viewCriteriaNoteID As Long
  Private _viewCriteriaNoteUserID As Long
  Private _viewCriteriaNoteCompanyID As Long
  Private _viewCriteriaNoteAircraftID As Long

  Private _viewCriteriaNoteClientID As Long

  Private _viewCriteriaNoteField As String
  Private _viewCriteriaNoteTextValue As String

  Private _viewCriteriaNoteACSearchTextValue As String
  Private _viewCriteriaNoteACSearchOperator As Integer
  Private _viewCriteriaNoteACSearchField As Integer

  Private _viewCriteriaNoteStartDate As String
  Private _viewCriteriaNoteEndDate As String

  Private _viewCriteriaNoteEntryDate As String

  Private _viewCriteriaNoteScheduleStartDate As String
  Private _viewCriteriaNoteScheduleEndDate As String

  Private _viewCriteriaNoteOrderBy As String
  Private _viewCriteriaNoteDocsAttached As String
  Private _viewCriteriaNoteType As String

  Private _viewCriteriaGetAllNotes As Boolean

  '4 Variables Added for CRM Version of Model Market Summary.
  'These variables store the slider information for Year/AFTT ranges.
  Private _viewCriteriaYearStart As Integer
  Private _viewCriteriaYearEnd As Integer
  Private _viewCriteriaAFTTStart As Long
  Private _viewCriteriaAFTTEnd As Long

  Private _viewCriteriaFolderID As Long
    Private _viewCriteriaFolderName As String

    Private _viewCriteriaInOperation As String ' ADDED MSW - 3/10/20

    'Private _viewCriteria

    Sub New()

    _viewCriteriaStatusCode = eObjStatusCode.NULL
    _viewCriteriaDetailError = eObjDetailErrorCode.NULL

    _viewCriteriaAmodID = -1
    _viewCriteriaAmodID1 = -1
    _viewCriteriaAmodID2 = -1

    _viewTempAmodID = -1
    _viewCriteriaMakeID = -1
    _viewCriteriaTypeID = -1

    _viewCriteriaWeightClass = ""

    _viewCriteriaHasCompanyLocationInfo = False

    _viewCriteriaUseContinent = True
    _viewCriteriaUseRegion = False

    _ViewCriteriaContinent = ""
    _viewCriteriaCountry = ""
    _viewCriteriaState = ""
    _viewCriteriaCity = ""
    _viewCriteriaTimeZone = ""

    _viewCriteriaContinentArray = Nothing
    _viewCriteriaCountryArray = Nothing
    _viewCriteriaStateArray = Nothing
    _viewCriteriaCityArray = Nothing
    _viewCriteriaTimeZoneArray = Nothing

    _viewCriteriaCountryHasStates = False

    _viewCriteriaHasBaseLocationInfo = False

    _viewCriteriaBaseUseContinent = True
    _viewCriteriaBaseUseRegion = False

    _ViewCriteriaBaseContinent = ""
    _viewCriteriaBaseCountry = ""
    _viewCriteriaBaseState = ""
    _viewCriteriaBaseCity = ""

    _viewCriteriaBaseContinentArray = Nothing
    _viewCriteriaBaseCountryArray = Nothing
    _viewCriteriaBaseStateArray = Nothing
    _viewCriteriaBaseCityArray = Nothing

    _viewCriteriaBaseCountryHasStates = False

    _viewCriteriaLocationViewType = crmWebClient.Constants.LOCATION_VIEW_BASE
    _viewCriteriaLocationViewSort = crmWebClient.Constants.LOCATION_SORT_COUNTRY

    _viewCriteriaProductType = crmWebClient.Constants.PRODUCT_CODE_ALL
    _viewCriteriaAirframeType = crmWebClient.Constants.VIEW_ALLAIRFRAME

    _viewCriteriaTimeSpan = 0

    _viewCriteriaAirframeTypeStr = ""
    _viewCriteriaAircraftType = ""
    _viewCriteriaAircraftMake = ""
    _viewCriteriaAircraftModel = ""
    _viewCriteriaAircraftModel1 = ""
    _viewCriteriaAircraftModel2 = ""

    _viewCriteriaUseMetricValues = False
    _viewCriteriaUseStatuteMiles = False

    _viewCriteriaSortBy = ""
    _viewCriteriaIsReport = False

    _viewCriteriaAircraftRange = 0.0

    _viewCriteriaHeliRangeTanksFull = 0.0
    _viewCriteriaHeliRangeSeatsFull = 0.0

    _viewCriteriaAircraftFieldLength = 0

    _viewCriteriaAircraftID = 0
    _viewCriteriaYachtID = 0
    _viewCriteriaJournalID = 0
    _viewCriteriaCompanyID = 0
    _viewTempCompanyID = 0

    _viewCriteriaGetExclusive = False
    _viewCriteriaGetOperator = False

    _viewCriteriaNoLocalNotes = False
    _viewCriteriaShowInternal = False

    _viewCriteriaDocumentsStartDate = ""
    _viewCriteriaDocumentsEndDate = ""

    _viewCriteriaDocumentsTxType = ""
    _viewCriteriaDocumentType = ""

    _viewCriteriaEngineName = ""

    _viewCriteriaAirportIATA = ""
    _viewCriteriaAirportICAO = ""
    _viewCriteriaAirportName = ""
    _viewCriteriaAirportLatitude = 0.0
    _viewCriteriaAirportLongitude = 0.0

    _viewCriteriaFractionalProgramID = 0
    _viewCriteriaFractionalProgramName = ""

    _viewID = 0
    _viewName = ""

    _viewCriteriaStarReportID = -1
    _viewCriteriaStarReportType = ""
    _viewCriteriaStarReportDate = ""
    _viewCriteriaStarReportCatagory = ""

    _viewCriteriaStarReportYear = ""
    _viewCriteriaStarReportPrefix = ""
    _viewCriteriaStarReportSuffix = ""

    _viewCriteriaSPIWeightClass = ""

    _viewCriteriaSPIYearSld1 = Now.Year.ToString
    _viewCriteriaSPIselectedYearSld1 = ""
    _viewCriteriaSPIYearSld2 = Now.Year.ToString
    _viewCriteriaSPIYearQtr1 = "1"
    _viewCriteriaSPIYearQtr1Name = ""

    _viewCriteriaSPIAirframe = 0

    '_viewCriteria
    _viewCriteriaAmodIDArray = Nothing
    _viewCriteriaMakeIDArray = Nothing
    _viewCriteriaTypeIDArray = Nothing

    _viewCriteriaHasHelicopterFlag = False
    _viewCriteriaHasBusinessFlag = False
    _viewCriteriaHasCommercialFlag = False
    _viewCriteriaHasRegionalFlag = False
    _viewCriteriaHasYachtFlag = False

    _viewCriteriaNoteID = 0
    _viewCriteriaNoteUserID = 0
    _viewCriteriaNoteCompanyID = 0
    _viewCriteriaNoteAircraftID = 0

    _viewCriteriaNoteClientID = 0

    _viewCriteriaNoteField = ""
    _viewCriteriaNoteTextValue = ""

    _viewCriteriaNoteACSearchTextValue = ""
    _viewCriteriaNoteACSearchOperator = 0
    _viewCriteriaNoteACSearchField = 0

    _viewCriteriaNoteStartDate = ""
    _viewCriteriaNoteEndDate = ""

    _viewCriteriaNoteEntryDate = ""

    _viewCriteriaNoteScheduleStartDate = ""
    _viewCriteriaNoteScheduleEndDate = ""

    _viewCriteriaNoteOrderBy = ""
    _viewCriteriaNoteDocsAttached = ""
    _viewCriteriaNoteType = ""

    _viewCriteriaGetAllNotes = False

    _viewCriteriaSubID = 0
    _viewCriteriaCRMuserID = 0
    _viewCriteriaLogin = ""


    _viewCriteriaYearStart = 0
    _viewCriteriaYearEnd = 0
    _viewCriteriaAFTTStart = 0
    _viewCriteriaAFTTEnd = 0

    _viewCriteriaFolderID = 0
        _viewCriteriaFolderName = ""


        _viewCriteriaInOperation = "" ' ADDED MSW - 3/10/20

    End Sub

  Public Property ViewSelectionCriteriaStatusCode() As eObjStatusCode
    Get
      Return _viewCriteriaStatusCode
    End Get
    Set(ByVal value As eObjStatusCode)
      _viewCriteriaStatusCode = value
    End Set
  End Property

  Public Property ViewSelectionCriteriaDetailError() As eObjDetailErrorCode
    Get
      Return _viewCriteriaDetailError
    End Get
    Set(ByVal value As eObjDetailErrorCode)
      _viewCriteriaDetailError = value
    End Set
  End Property

  Public Property ViewCriteriaAmodID() As Long
    Get
      Return _viewCriteriaAmodID
    End Get
    Set(ByVal value As Long)
      _viewCriteriaAmodID = value
    End Set
  End Property

  Public Property ViewTempAmodID() As Long
    Get
      Return _viewTempAmodID
    End Get
    Set(ByVal value As Long)
      _viewTempAmodID = value
    End Set
  End Property

  Public Property ViewCriteriaMakeAmodID() As Long
    Get
      Return _viewCriteriaMakeID
    End Get
    Set(ByVal value As Long)
      _viewCriteriaMakeID = value
    End Set
  End Property

  Public Property ViewCriteriaTypeAmodID() As Long
    Get
      Return _viewCriteriaTypeID
    End Get
    Set(ByVal value As Long)
      _viewCriteriaTypeID = value
    End Set
  End Property

  Public Property ViewCriteriaSecondAmodID() As Long
    Get
      Return _viewCriteriaAmodID1
    End Get
    Set(ByVal value As Long)
      _viewCriteriaAmodID1 = value
    End Set
  End Property

  Public Property ViewCriteriaThirdAmodID() As Long
    Get
      Return _viewCriteriaAmodID2
    End Get
    Set(ByVal value As Long)
      _viewCriteriaAmodID2 = value
    End Set
  End Property

  Public Property ViewCriteriaProductType() As Integer
    Get
      Return _viewCriteriaProductType
    End Get
    Set(ByVal value As Integer)
      _viewCriteriaProductType = value
    End Set
  End Property

  Public Property ViewCriteriaTimeSpan() As Integer
    Get
      Return _viewCriteriaTimeSpan
    End Get
    Set(ByVal value As Integer)
      _viewCriteriaTimeSpan = value
    End Set
  End Property

  Public Property ViewCriteriaAirframeType() As Integer
    Get
      Return _viewCriteriaAirframeType
    End Get
    Set(ByVal value As Integer)
      _viewCriteriaAirframeType = value
    End Set
  End Property

  Public Property ViewCriteriaAirframeTypeStr() As String
    Get
      Return _viewCriteriaAirframeTypeStr
    End Get
    Set(ByVal value As String)
      _viewCriteriaAirframeTypeStr = value
    End Set
  End Property

  Public Property ViewCriteriaAircraftType() As String
    Get
      Return _viewCriteriaAircraftType
    End Get
    Set(ByVal value As String)
      _viewCriteriaAircraftType = value
    End Set
  End Property

  Public Property ViewCriteriaAircraftMake() As String
    Get
      Return _viewCriteriaAircraftMake
    End Get
    Set(ByVal value As String)
      _viewCriteriaAircraftMake = value
    End Set
  End Property

  Public Property ViewCriteriaAircraftModel() As String
    Get
      Return _viewCriteriaAircraftModel
    End Get
    Set(ByVal value As String)
      _viewCriteriaAircraftModel = value
    End Set
  End Property

  Public Property ViewCriteriaSecondModel() As String
    Get
      Return _viewCriteriaAircraftModel1
    End Get
    Set(ByVal value As String)
      _viewCriteriaAircraftModel1 = value
    End Set
  End Property

  Public Property ViewCriteriaThirdModel() As String '
    Get
      Return _viewCriteriaAircraftModel2
    End Get
    Set(ByVal value As String)
      _viewCriteriaAircraftModel2 = value
    End Set
  End Property

  Public Property ViewCriteriaUseMetricValues() As Boolean
    Get
      Return _viewCriteriaUseMetricValues
    End Get
    Set(ByVal value As Boolean)
      _viewCriteriaUseMetricValues = value
    End Set
  End Property

  Public Property ViewCriteriaUseStatuteMiles() As Boolean
    Get
      Return _viewCriteriaUseStatuteMiles
    End Get
    Set(ByVal value As Boolean)
      _viewCriteriaUseStatuteMiles = value
    End Set
  End Property

  Public Property ViewCriteriaSortBy() As String
    Get
      Return _viewCriteriaSortBy
    End Get
    Set(ByVal value As String)
      _viewCriteriaSortBy = value
    End Set
  End Property

  Public Property ViewCriteriaIsReport() As Boolean
    Get
      Return _viewCriteriaIsReport
    End Get
    Set(ByVal value As Boolean)
      _viewCriteriaIsReport = value
    End Set
  End Property

  Public Property ViewCriteriaAircraftID() As Long
    Get
      Return _viewCriteriaAircraftID
    End Get
    Set(ByVal value As Long)
      _viewCriteriaAircraftID = value
    End Set
  End Property

  Public Property ViewCriteriaYachtID() As Long  '
    Get
      Return _viewCriteriaYachtID
    End Get
    Set(ByVal value As Long)
      _viewCriteriaYachtID = value
    End Set
  End Property

  Public Property ViewCriteriaJournalID() As Long
    Get
      Return _viewCriteriaJournalID
    End Get
    Set(ByVal value As Long)
      _viewCriteriaJournalID = value
    End Set
  End Property

  Public Property ViewCriteriaCompanyID() As Long
    Get
      Return _viewCriteriaCompanyID
    End Get
    Set(ByVal value As Long)
      _viewCriteriaCompanyID = value
    End Set
  End Property

  Public Property ViewTempCompanyID() As Long
    Get
      Return _viewTempCompanyID
    End Get
    Set(ByVal value As Long)
      _viewTempCompanyID = value
    End Set
  End Property

  Public Property ViewCriteriaGetExclusive() As Boolean
    Get
      Return _viewCriteriaGetExclusive
    End Get
    Set(ByVal value As Boolean)
      _viewCriteriaGetExclusive = value
    End Set
  End Property

  Public Property ViewCriteriaGetOperator() As Boolean
    Get
      Return _viewCriteriaGetOperator
    End Get
    Set(ByVal value As Boolean)
      _viewCriteriaGetOperator = value
    End Set
  End Property

  Public Property ViewCriteriaAircraftRange() As Double
    Get
      Return _viewCriteriaAircraftRange
    End Get
    Set(ByVal value As Double)
      _viewCriteriaAircraftRange = value
    End Set
  End Property

  Public Property ViewCriteriaHeliRangeTanksFull() As Double
    Get
      Return _viewCriteriaHeliRangeTanksFull
    End Get
    Set(ByVal value As Double)
      _viewCriteriaHeliRangeTanksFull = value
    End Set
  End Property

  Public Property ViewCriteriaHeliRangeSeatsFull() As Double
    Get
      Return _viewCriteriaHeliRangeSeatsFull
    End Get
    Set(ByVal value As Double)
      _viewCriteriaHeliRangeSeatsFull = value
    End Set
  End Property

  Public Property ViewCriteriaAircraftFieldLength() As Long
    Get
      Return _viewCriteriaAircraftFieldLength
    End Get
    Set(ByVal value As Long)
      _viewCriteriaAircraftFieldLength = value
    End Set
  End Property

  Public Property ViewCriteriaNoLocalNotes() As Boolean
    Get
      Return _viewCriteriaNoLocalNotes
    End Get
    Set(ByVal value As Boolean)
      _viewCriteriaNoLocalNotes = value
    End Set
  End Property

  Public Property ViewCriteriaShowInternal() As Boolean
    Get
      Return _viewCriteriaShowInternal
    End Get
    Set(ByVal value As Boolean)
      _viewCriteriaShowInternal = value
    End Set
  End Property

  'company location properties
  Public Property ViewCriteriaHasCompanyLocationInfo() As Boolean
    Get
      Return _viewCriteriaHasCompanyLocationInfo
    End Get
    Set(ByVal value As Boolean)
      _viewCriteriaHasCompanyLocationInfo = value
    End Set
  End Property

  Public Property ViewCriteriaUseContinent() As Boolean
    Get
      Return _viewCriteriaUseContinent
    End Get
    Set(ByVal value As Boolean)
      _viewCriteriaUseContinent = value
    End Set
  End Property

  Public Property ViewCriteriaUseRegion() As Boolean
    Get
      Return _viewCriteriaUseRegion
    End Get
    Set(ByVal value As Boolean)
      _viewCriteriaUseRegion = value
    End Set
  End Property

  Public Property ViewCriteriaContinent() As String
    Get
      Return _ViewCriteriaContinent
    End Get
    Set(ByVal value As String)
      _ViewCriteriaContinent = value
    End Set
  End Property

  Public Property ViewCriteriaCountry() As String
    Get
      Return _viewCriteriaCountry
    End Get
    Set(ByVal value As String)
      _viewCriteriaCountry = value
    End Set
  End Property

  Public Property ViewCriteriaState() As String
    Get
      Return _viewCriteriaState
    End Get
    Set(ByVal value As String)
      _viewCriteriaState = value
    End Set
  End Property

  Public Property ViewCriteriaCity() As String
    Get
      Return _viewCriteriaCity
    End Get
    Set(ByVal value As String)
      _viewCriteriaCity = value
    End Set
  End Property

  Public Property ViewCriteriaTimeZone() As String
    Get
      Return _viewCriteriaTimeZone
    End Get
    Set(ByVal value As String)
      _viewCriteriaTimeZone = value
    End Set
  End Property

  Public Property ViewCriteriaCountryHasStates() As Boolean
    Get
      Return _viewCriteriaCountryHasStates
    End Get
    Set(ByVal value As Boolean)
      _viewCriteriaCountryHasStates = value
    End Set
  End Property

  ' aircraft base location properties
  Public Property ViewCriteriaHasBaseLocationInfo() As Boolean
    Get
      Return _viewCriteriaHasBaseLocationInfo
    End Get
    Set(ByVal value As Boolean)
      _viewCriteriaHasBaseLocationInfo = value
    End Set
  End Property

  Public Property ViewCriteriaBaseUseContinent() As Boolean
    Get
      Return _viewCriteriaBaseUseContinent
    End Get
    Set(ByVal value As Boolean)
      _viewCriteriaBaseUseContinent = value
    End Set
  End Property

  Public Property ViewCriteriaBaseUseRegion() As Boolean
    Get
      Return _viewCriteriaBaseUseRegion
    End Get
    Set(ByVal value As Boolean)
      _viewCriteriaBaseUseRegion = value
    End Set
  End Property

  Public Property ViewCriteriaBaseContinent() As String
    Get
      Return _ViewCriteriaContinent
    End Get
    Set(ByVal value As String)
      _ViewCriteriaBaseContinent = value
    End Set
  End Property

  Public Property ViewCriteriaBaseCountry() As String
    Get
      Return _viewCriteriaBaseCountry
    End Get
    Set(ByVal value As String)
      _viewCriteriaBaseCountry = value
    End Set
  End Property

  Public Property ViewCriteriaBaseState() As String
    Get
      Return _viewCriteriaBaseState
    End Get
    Set(ByVal value As String)
      _viewCriteriaBaseState = value
    End Set
  End Property

  Public Property ViewCriteriaBaseCity() As String
    Get
      Return _viewCriteriaBaseCity
    End Get
    Set(ByVal value As String)
      _viewCriteriaBaseCity = value
    End Set
  End Property

  Public Property ViewCriteriaBaseCountryHasStates() As Boolean
    Get
      Return _viewCriteriaBaseCountryHasStates
    End Get
    Set(ByVal value As Boolean)
      _viewCriteriaBaseCountryHasStates = value
    End Set
  End Property

  Public Property ViewCriteriaDocumentsStartDate() As String
    Get
      Return _viewCriteriaDocumentsStartDate
    End Get
    Set(ByVal value As String)
      _viewCriteriaDocumentsStartDate = value
    End Set
  End Property

  Public Property ViewCriteriaDocumentsEndDate() As String
    Get
      Return _viewCriteriaDocumentsEndDate
    End Get
    Set(ByVal value As String)
      _viewCriteriaDocumentsEndDate = value
    End Set
  End Property

  Public Property ViewCriteriaAirportIATA() As String
    Get
      Return _viewCriteriaAirportIATA
    End Get
    Set(ByVal value As String)
      _viewCriteriaAirportIATA = value
    End Set
  End Property

  Public Property ViewCriteriaAirportICAO() As String
    Get
      Return _viewCriteriaAirportICAO
    End Get
    Set(ByVal value As String)
      _viewCriteriaAirportICAO = value
    End Set
  End Property

  Public Property ViewCriteriaEngineName() As String
    Get
      Return _viewCriteriaEngineName
    End Get
    Set(ByVal value As String)
      _viewCriteriaEngineName = value
    End Set
  End Property

  Public Property ViewCriteriaSPIWeightClass() As String
    Get
      Return _viewCriteriaSPIWeightClass
    End Get
    Set(ByVal value As String)
      _viewCriteriaSPIWeightClass = value
    End Set
  End Property

  Public Property ViewCriteriaSPIYearSld1() As String
    Get
      Return _viewCriteriaSPIYearSld1
    End Get
    Set(ByVal value As String)
      _viewCriteriaSPIYearSld1 = value
    End Set
  End Property

  Public Property ViewCriteriaSPIselectedYearSld1() As String
    Get
      Return _viewCriteriaSPIselectedYearSld1
    End Get
    Set(ByVal value As String)
      _viewCriteriaSPIselectedYearSld1 = value
    End Set
  End Property

  Public Property ViewCriteriaSPIYearSld2() As String
    Get
      Return _viewCriteriaSPIYearSld2
    End Get
    Set(ByVal value As String)
      _viewCriteriaSPIYearSld2 = value
    End Set
  End Property

  Public Property ViewCriteriaSPIYearQtr1() As String
    Get
      Return _viewCriteriaSPIYearQtr1
    End Get
    Set(ByVal value As String)
      _viewCriteriaSPIYearQtr1 = value
    End Set
  End Property

  Public Property ViewCriteriaSPIYearQtr1Name() As String
    Get
      Return _viewCriteriaSPIYearQtr1Name
    End Get
    Set(ByVal value As String)
      _viewCriteriaSPIYearQtr1Name = value
    End Set
  End Property

  Public Property viewCriteriaSPIAirframe() As String
    Get
      Return _viewCriteriaSPIAirframe
    End Get
    Set(ByVal value As String)
      _viewCriteriaSPIAirframe = value
    End Set
  End Property

  Public Property ViewCriteriaLocationViewType() As Integer
    Get
      Return _viewCriteriaLocationViewType
    End Get
    Set(ByVal value As Integer)
      _viewCriteriaLocationViewType = value
    End Set
  End Property

  Public Property ViewCriteriaLocationViewSort() As Integer
    Get
      Return _viewCriteriaLocationViewSort
    End Get
    Set(ByVal value As Integer)
      _viewCriteriaLocationViewSort = value
    End Set
  End Property

  Public Property ViewCriteriaFractionalProgramID() As Long
    Get
      Return _viewCriteriaFractionalProgramID
    End Get
    Set(ByVal value As Long)
      _viewCriteriaFractionalProgramID = value
    End Set
  End Property

  Public Property ViewCriteriaFractionalProgramName() As String
    Get
      Return _viewCriteriaFractionalProgramName
    End Get
    Set(ByVal value As String)
      _viewCriteriaFractionalProgramName = value
    End Set
  End Property

  Public Property ViewID() As Long
    Get
      Return _viewID
    End Get
    Set(ByVal value As Long)
      _viewID = value
    End Set
  End Property

  Public Property ViewName() As String
    Get
      Return _viewName
    End Get
    Set(ByVal value As String)
      _viewName = value
    End Set
  End Property

  Public Property ViewCriteriaStarReportID() As Integer
    Get
      Return _viewCriteriaStarReportID
    End Get
    Set(ByVal value As Integer)
      _viewCriteriaStarReportID = value
    End Set
  End Property

  Public Property ViewCriteriaStarReportType() As String
    Get
      Return _viewCriteriaStarReportType
    End Get
    Set(ByVal value As String)
      _viewCriteriaStarReportType = value
    End Set
  End Property

  Public Property ViewCriteriaStarReportDate() As String
    Get
      Return _viewCriteriaStarReportDate
    End Get
    Set(ByVal value As String)
      _viewCriteriaStarReportDate = value
    End Set
  End Property

  Public Property ViewCriteriaStarReportCatagory() As String
    Get
      Return _viewCriteriaStarReportCatagory
    End Get
    Set(ByVal value As String)
      _viewCriteriaStarReportCatagory = value
    End Set
  End Property

  Public Property ViewCriteriaStarReportYear() As String
    Get
      Return _viewCriteriaStarReportYear
    End Get
    Set(ByVal value As String)
      _viewCriteriaStarReportYear = value
    End Set
  End Property

  Public Property ViewCriteriaStarReportPrefix() As String
    Get
      Return _viewCriteriaStarReportPrefix
    End Get
    Set(ByVal value As String)
      _viewCriteriaStarReportPrefix = value
    End Set
  End Property

  Public Property ViewCriteriaStarReportSuffix() As String
    Get
      Return _viewCriteriaStarReportSuffix
    End Get
    Set(ByVal value As String)
      _viewCriteriaStarReportSuffix = value
    End Set
  End Property

  Public Property ViewCriteriaWeightClass() As String
    Get
      Return _viewCriteriaWeightClass
    End Get
    Set(ByVal value As String)
      _viewCriteriaWeightClass = value
    End Set
  End Property

  Public Property ViewCriteriaAmodIDArray() As Array
    Get
      Return _viewCriteriaAmodIDArray
    End Get
    Set(ByVal value As Array)
      _viewCriteriaAmodIDArray = value
    End Set
  End Property

  Public Property ViewCriteriaMakeIDArray() As Array
    Get
      Return _viewCriteriaMakeIDArray
    End Get
    Set(ByVal value As Array)
      _viewCriteriaMakeIDArray = value
    End Set
  End Property

  Public Property ViewCriteriaTypeIDArray() As Array
    Get
      Return _viewCriteriaTypeIDArray
    End Get
    Set(ByVal value As Array)
      _viewCriteriaTypeIDArray = value
    End Set
  End Property

  Public Property ViewCriteriaAirportName() As String
    Get
      Return _viewCriteriaAirportName
    End Get
    Set(ByVal value As String)
      _viewCriteriaAirportName = value
    End Set
  End Property

  Public Property ViewCriteriaAirportLatitude() As Double
    Get
      Return _viewCriteriaAirportLatitude
    End Get
    Set(ByVal value As Double)
      _viewCriteriaAirportLatitude = value
    End Set
  End Property

  Public Property ViewCriteriaAirportLongitude() As Double
    Get
      Return _viewCriteriaAirportLongitude
    End Get
    Set(ByVal value As Double)
      _viewCriteriaAirportLongitude = value
    End Set
  End Property

  Public Property ViewCriteriaHasHelicopterFlag() As Boolean
    Get
      Return _viewCriteriaHasHelicopterFlag
    End Get
    Set(ByVal value As Boolean)
      _viewCriteriaHasHelicopterFlag = value
    End Set
  End Property

  Public Property ViewCriteriaHasBusinessFlag() As Boolean
    Get
      Return _viewCriteriaHasBusinessFlag
    End Get
    Set(ByVal value As Boolean)
      _viewCriteriaHasBusinessFlag = value
    End Set
  End Property

  Public Property ViewCriteriaHasCommercialFlag() As Boolean
    Get
      Return _viewCriteriaHasCommercialFlag
    End Get
    Set(ByVal value As Boolean)
      _viewCriteriaHasCommercialFlag = value
    End Set
  End Property

  Public Property ViewCriteriaHasRegionalFlag() As Boolean
    Get
      Return _viewCriteriaHasRegionalFlag
    End Get
    Set(ByVal value As Boolean)
      _viewCriteriaHasRegionalFlag = value
    End Set
  End Property

  Public Property ViewCriteriaHasYachtFlag() As Boolean
    Get
      Return _viewCriteriaHasYachtFlag
    End Get
    Set(ByVal value As Boolean)
      _viewCriteriaHasYachtFlag = value
    End Set
  End Property

  Public Property ViewCriteriaContinentArray() As Array
    Get
      Return _viewCriteriaContinentArray
    End Get
    Set(ByVal value As Array)
      _viewCriteriaContinentArray = value
    End Set
  End Property

  Public Property ViewCriteriaCountryArray() As Array
    Get
      Return _viewCriteriaCountryArray
    End Get
    Set(ByVal value As Array)
      _viewCriteriaCountryArray = value
    End Set
  End Property

  Public Property ViewCriteriaStateArray() As Array
    Get
      Return _viewCriteriaStateArray
    End Get
    Set(ByVal value As Array)
      _viewCriteriaStateArray = value
    End Set
  End Property

  Public Property ViewCriteriaCityArray() As Array
    Get
      Return _viewCriteriaCityArray
    End Get
    Set(ByVal value As Array)
      _viewCriteriaCityArray = value
    End Set
  End Property

  Public Property ViewCriteriaTimeZoneArray() As Array
    Get
      Return _viewCriteriaTimeZoneArray
    End Get
    Set(ByVal value As Array)
      _viewCriteriaTimeZoneArray = value
    End Set
  End Property

  Public Property ViewCriteriaBaseContinentArray() As Array
    Get
      Return _viewCriteriaBaseContinentArray
    End Get
    Set(ByVal value As Array)
      _viewCriteriaBaseContinentArray = value
    End Set
  End Property

  Public Property ViewCriteriaBaseCountryArray() As Array
    Get
      Return _viewCriteriaBaseCountryArray
    End Get
    Set(ByVal value As Array)
      _viewCriteriaBaseCountryArray = value
    End Set
  End Property

  Public Property ViewCriteriaBaseStateArray() As Array
    Get
      Return _viewCriteriaBaseStateArray
    End Get
    Set(ByVal value As Array)
      _viewCriteriaBaseStateArray = value
    End Set
  End Property

  Public Property ViewCriteriaBaseCityArray() As Array
    Get
      Return _viewCriteriaBaseCityArray
    End Get
    Set(ByVal value As Array)
      _viewCriteriaBaseCityArray = value
    End Set
  End Property

  Public Property ViewCriteriaDocumentsTxType() As String
    Get
      Return _viewCriteriaDocumentsTxType
    End Get
    Set(ByVal value As String)
      _viewCriteriaDocumentsTxType = value
    End Set
  End Property

  Public Property ViewCriteriaDocumentType() As String '
    Get
      Return _viewCriteriaDocumentType
    End Get
    Set(ByVal value As String)
      _viewCriteriaDocumentType = value
    End Set
  End Property

  Public Property ViewCriteriaNoteACSearchTextValue() As String
    Get
      Return _viewCriteriaNoteACSearchTextValue
    End Get
    Set(ByVal value As String)
      _viewCriteriaNoteACSearchTextValue = value
    End Set
  End Property

  Public Property ViewCriteriaNoteACSearchOperator() As Integer
    Get
      Return _viewCriteriaNoteACSearchOperator
    End Get
    Set(ByVal value As Integer)
      _viewCriteriaNoteACSearchOperator = value
    End Set
  End Property

  Public Property ViewCriteriaNoteACSearchField() As Integer
    Get
      Return _viewCriteriaNoteACSearchField
    End Get
    Set(ByVal value As Integer)
      _viewCriteriaNoteACSearchField = value
    End Set
  End Property
 
  Public Property ViewCriteriaNoteField() As String
    Get
      Return _viewCriteriaNoteField
    End Get
    Set(ByVal value As String)
      _viewCriteriaNoteField = value
    End Set
  End Property

  Public Property ViewCriteriaNoteTextValue() As String
    Get
      Return _viewCriteriaNoteTextValue
    End Get
    Set(ByVal value As String)
      _viewCriteriaNoteTextValue = value
    End Set
  End Property

  Public Property ViewCriteriaNoteStartDate() As String
    Get
      Return _viewCriteriaNoteStartDate
    End Get
    Set(ByVal value As String)
      _viewCriteriaNoteStartDate = value
    End Set
  End Property

  Public Property ViewCriteriaNoteEndDate() As String
    Get
      Return _viewCriteriaNoteEndDate
    End Get
    Set(ByVal value As String)
      _viewCriteriaNoteEndDate = value
    End Set
  End Property

  Public Property ViewCriteriaNoteEntryDate() As String
    Get
      Return _viewCriteriaNoteEntryDate
    End Get
    Set(ByVal value As String)
      _viewCriteriaNoteEntryDate = value
    End Set
  End Property

  Public Property ViewCriteriaNoteScheduleStartDate() As String
    Get
      Return _viewCriteriaNoteScheduleStartDate
    End Get
    Set(ByVal value As String)
      _viewCriteriaNoteScheduleStartDate = value
    End Set
  End Property

  Public Property ViewCriteriaNoteScheduleEndDate() As String
    Get
      Return _viewCriteriaNoteScheduleEndDate
    End Get
    Set(ByVal value As String)
      _viewCriteriaNoteScheduleEndDate = value
    End Set
  End Property

  Public Property ViewCriteriaNoteOrderBy() As String
    Get
      Return _viewCriteriaNoteOrderBy
    End Get
    Set(ByVal value As String)
      _viewCriteriaNoteOrderBy = value
    End Set
  End Property

  Public Property ViewCriteriaNoteDocsAttached() As String
    Get
      Return _viewCriteriaNoteDocsAttached
    End Get
    Set(ByVal value As String)
      _viewCriteriaNoteDocsAttached = value
    End Set
  End Property

  Public Property ViewCriteriaNoteType() As String
    Get
      Return _viewCriteriaNoteType
    End Get
    Set(ByVal value As String)
      _viewCriteriaNoteType = value
    End Set
  End Property

  Public Property ViewCriteriaNoteID() As Long
    Get
      Return _viewCriteriaNoteID
    End Get
    Set(ByVal value As Long)
      _viewCriteriaNoteID = value
    End Set
  End Property

  Public Property ViewCriteriaNoteUserID() As Long
    Get
      Return _viewCriteriaNoteUserID
    End Get
    Set(ByVal value As Long)
      _viewCriteriaNoteUserID = value
    End Set
  End Property

  Public Property ViewCriteriaNoteCompanyID() As Long
    Get
      Return _viewCriteriaNoteCompanyID
    End Get
    Set(ByVal value As Long)
      _viewCriteriaNoteCompanyID = value
    End Set
  End Property

  Public Property ViewCriteriaNoteAircraftID() As Long
    Get
      Return _viewCriteriaNoteAircraftID
    End Get
    Set(ByVal value As Long)
      _viewCriteriaNoteAircraftID = value
    End Set
  End Property

  Public Property ViewCriteriaNoteClientID() As Long
    Get
      Return _viewCriteriaNoteClientID
    End Get
    Set(ByVal value As Long)
      _viewCriteriaNoteClientID = value
    End Set
  End Property

  Public Property ViewCriteriaGetAllNotes() As Boolean
    Get
      Return _viewCriteriaGetAllNotes
    End Get
    Set(ByVal value As Boolean)
      _viewCriteriaGetAllNotes = value
    End Set
  End Property

  Public Property ViewCriteriaSubID() As Long
    Get
      Return _viewCriteriaSubID
    End Get
    Set(ByVal value As Long)
      _viewCriteriaSubID = value
    End Set
  End Property

  Public Property ViewCriteriaCRMuserID() As Long
    Get
      Return _viewCriteriaCRMuserID
    End Get
    Set(ByVal value As Long)
      _viewCriteriaCRMuserID = value
    End Set
  End Property

  Public Property ViewCriteriaLogin() As String
    Get
      Return _viewCriteriaLogin
    End Get
    Set(ByVal value As String)
      _viewCriteriaLogin = value
    End Set
  End Property

  'CRM Model Summary Properties:
  Public Property ViewCriteriaAFTTStart() As Long
    Get
      Return _viewCriteriaAFTTStart
    End Get
    Set(ByVal value As Long)
      _viewCriteriaAFTTStart = value
    End Set
  End Property

  Public Property ViewCriteriaAFTTEnd() As Long
    Get
      Return _viewCriteriaAFTTEnd
    End Get
    Set(ByVal value As Long)
      _viewCriteriaAFTTEnd = value
    End Set
  End Property

  Public Property ViewCriteriaYearStart() As Integer
    Get
      Return _viewCriteriaYearStart
    End Get
    Set(ByVal value As Integer)
      _viewCriteriaYearStart = value
    End Set
  End Property

  Public Property ViewCriteriaYearEnd() As Integer
    Get
      Return _viewCriteriaYearEnd
    End Get
    Set(ByVal value As Integer)
      _viewCriteriaYearEnd = value
    End Set
  End Property

  Public Property ViewCriteriaFolderID() As Long
    Get
      Return _viewCriteriaFolderID
    End Get
    Set(ByVal value As Long)
      _viewCriteriaFolderID = value
    End Set
  End Property

    Public Property ViewCriteriaFolderName() As String
        Get
            Return _viewCriteriaFolderName
        End Get
        Set(ByVal value As String)
            _viewCriteriaFolderName = value
        End Set
    End Property

    Public Property viewCriteriaInOperation() As String
        Get
            Return _viewCriteriaInOperation
        End Get
        Set(ByVal value As String)
            _viewCriteriaInOperation = value
        End Set
    End Property


    'Public Sub stub(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

    '  Dim results_table As New DataTable 
    '  Dim htmlOut As New StringBuilder
    '  Dim strOut As New StringBuilder
    '  Dim toggleRowColor As Boolean = False

    '  Try

    '    results_table = get_function(searchCriteria)

    '    If Not IsNothing(results_table) Then

    '      If results_table.Rows.Count > 0 Then

    '        For Each r As DataRow In results_table.Rows
    '          If Not toggleRowColor Then
    '            htmlOut.Append("<tr class='alt_row'>")
    '            toggleRowColor = True
    '          Else
    '            htmlOut.Append("<tr bgcolor='white'>")
    '            toggleRowColor = False
    '          End If

    '        Next
    '      Else

    '      End If
    '    Else

    '    End If

    '  Catch ex As Exception

    '    aError = "Error in stub(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    '  Finally

    '  End Try

    '  'return resulting html string
    '  out_htmlString = htmlOut.ToString
    '  htmlOut = Nothing
    '  strOut = Nothing
    '  results_table = Nothing

    'End Sub

End Class

