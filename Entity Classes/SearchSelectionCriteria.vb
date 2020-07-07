Imports Microsoft.VisualBasic
Imports System.ComponentModel

' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/SearchSelectionCriteria.vb $
'$$Author: Mike $
'$$Date: 3/25/20 11:01a $
'$$Modtime: 3/25/20 10:35a $
'$$Revision: 5 $
'$$Workfile: SearchSelectionCriteria.vb $
'
' ********************************************************************************

<System.Serializable()> Public Class SearchSelectionCriteria
  'Enums
  Private _searchCriteriaStatusCode As eObjStatusCode '
  Private _searchCriteriaDetailError As eObjDetailErrorCode '

  'Integers
  Private _searchCriteriaEventMonths As Integer '
  Private _searchCriteriaEventDays As Integer '
  Private _searchCriteriaEventHours As Integer '
  Private _searchCriteriaEventMinutes As Integer '
  Private _searchCriteriaCompanyFleetValue As Integer '

  Private _searchCriteriaYachtEventMonths As Integer '
  Private _searchCriteriaYachtEventDays As Integer '
  Private _searchCriteriaYachtEventHours As Integer '
  Private _searchCriteriaYachtEventMinutes As Integer '
  Private _searchCriteriaViewModel As Integer

  'Long
  Private _searchCriteriaViewAC As Long


  'Strings
  Private _searchCriteriaSerNoStart As String '
  Private _searchCriteriaSerNoEnd As String '
  Private _searchCriteriaRegNo As String '
  Private _searchCriteriaMarketStatus As String '
  Private _searchCriteriaOwnership As String '
  Private _searchCriteriaLeaseStatus As String '
  Private _searchCriteriaPreviouslyOwned As String '

  Private _searchCriteriaWeightClass As String '
  Private _searchCriteriaManufacturerName As String '
  Private _searchCriteriaAcSize As String '

  Private _searchCriteriaType As String '
  Private _searchCriteriaMake As String '
  Private _searchCriteriaModel As String '
  Private _searchCriteriaLifeCycle As String '
  Private _searchCriteriaHistoryType As String '
  Private _searchCriteriaHistoryFromOperator As String '
  Private _searchCriteriaHistoryFromAnswer As String '
  Private _searchCriteriaHistoryToOperator As String '
  Private _searchCriteriaHistoryToAnswer As String '
  Private _searchCriteriaHistoryDateOperator As String '
  Private _searchCriteriaHistoryDate As String '
  Private _searchCriteriaEventSearchType As String
  Private _searchCriteriaEventCategory As String '
  Private _searchCriteriaEventType As String '
  Private _searchCriteriaViewFeatureString As String
  Private _searchCriteriaViewVariantString As String

  Private _searchCriteriaCompanyID As Long '
  Private _searchCriteriaCompanyName As String '
  Private _searchCriteriaCompanyNameQueryString As String '
  Private _searchCriteriaCompanyAgencyType As String '
  Private _searchCriteriaCompanyRelationshipsToAC As String '
  Private _searchCriteriaCompanyAddress As String '
  Private _searchCriteriaCompanyCity As String '

  Private _searchCriteriaCompanyEmail As String '
  Private _searchCriteriaCompanyPhone As String '

  Private _searchCriteriaCompanyPostalCode As String '
  Private _searchCriteriaCompanyBusinessType As String '
  Private _searchCriteriaCompanyContinent As String '
  Private _searchCriteriaCompanyContinentOrRegion As String '
  Private _searchCriteriaCompanyRegion As String '
  Private _searchCriteriaCompanyStateProvince As String '
  Private _searchCriteriaCompanyStateName As String
  Private _searchCriteriaCompanyTimezone As String '
  Private _searchCriteriaCompanyCertifications As String '

  Private _searchCriteriaHasCompanyLocationInfo As Boolean   ' 

  Private _searchCriteriaUseContinent As Boolean   ' toggle to search Continent/Region
  Private _searchCriteriaUseRegion As Boolean

  Private _searchCriteriaCompanyYachtFleet As String
  Private _searchCriteriaCompanyFleetOperator As String '
  Private _searchCriteriaCompanyFleetAnswer As String '

  Private _searchCriteriaCompanyContactID As Long '
  Private _searchCriteriaCompanyContactFirstName As String '
  Private _searchCriteriaCompanyContactLastName As String '
  Private _searchCriteriaCompanyContactEmail As String '
  Private _searchCriteriaCompanyContactPhone As String '
  Private _searchCriteriaCompanyContactTitle As String '

  'Operating Cost Variables
  Private _searchCriteriaOpCostsFuelBurnOperator As String
  Private _searchCriteriaOpCostsFuelBurn As String
  Private _searchCriteriaOpCostsTotalDirectCostsOperator As String
  Private _searchCriteriaOpCostsTotalDirectCosts As String
  Private _searchCriteriaOpCostsCurrency As String

  'Both Operating Cost and Performance Variables
  Private _searchCriteriaDisplayUnits As String
  Private _searchCriteriaDisplayMiles As String

  'Performance Specs Variables 
  Private _searchCriteriaPerfSpecsSLISAOperator As String
  Private _searchCriteriaPerfSpecsSLISA As String
  Private _searchCriteriaPerfSpecsMaxRangeNBAAOrTanksFullOperator As String
  Private _searchCriteriaPerfSpecsMaxRangeNBAAOrTanksFull As String
  Private _searchCriteriaPerfSpecsFuselageLengthOperator As String
  Private _searchCriteriaPerfSpecsFuselageLength As String
  Private _searchCriteriaPerfSpecsFuselageHeightOperator As String
  Private _searchCriteriaPerfSpecsFuselageHeight As String
  Private _searchCriteriaPerfSpecsWingSpanOrWidthOperator As String
  Private _searchCriteriaPerfSpecsWingSpanOrWidth As String
  Private _searchCriteriaPerfSpecsCrewOperator As String
  Private _searchCriteriaPerfSpecsCrew As String
  Private _searchCriteriaPerfSpecsPassengersOperator As String
  Private _searchCriteriaPerfSpecsPassengers As String
  Private _searchCriteriaPerfSpecsMaxTakeoffOperator As String
  Private _searchCriteriaPerfSpecsMaxTakeoff As String
  Private _searchCriteriaPerfSpecsNormalCruiseOperator As String
  Private _searchCriteriaPerfSpecsNormalCruise As String
  Private _searchCriteriaPerfSpecsFuelCapacityOperator As String
  Private _searchCriteriaPerfSpecsFuelCapacity As String


  'Yacht Listing Search Items:
  Private _searchCriteriaYachtName As String
  Private _searchCriteriaYachtFlagOption As String
  Private _searchCriteriaYachtLengthOperator As String
  Private _searchCriteriaYachtLengthValue As String
  Private _searchCriteriaYachtLengthStandard As String
  Private _searchCriteriaYachtClass As String
  Private _searchCriteriaYachtCallSign As String
  Private _searchCriteriaYachtYearDeliveredOperator As String
  Private _searchCriteriaYachtYearDelivered As String
  Private _searchCriteriaYachtYearManufacturedOperator As String
  Private _searchCriteriaYachtYearManufactured As String
  Private _searchCriteriaYachtAskingPriceOperator As String
  Private _searchCriteriaYachtAskingPrice As String
  Private _searchCriteriaYachtAskingPriceCurrency As String
  Private _searchCriteriaYachtDOMOperator As String
  Private _searchCriteriaYachtDOM As String
  Private _searchCriteriaYachtMarketStatus As String
  Private _searchCriteriaYachtLifecycle As String
  Private _searchCriteriaYachtTransactionDateOperator As String
  Private _searchCriteriaYachtTransactionDate As String
  Private _searchCriteriaYachtTransactionType As String
  Private _searchCriteriaYachtCategory As String
  Private _searchCriteriaYachtType As String
  Private _searchCriteriaYachtSaleCharterRestrictions As String
  'Booleans
  Private _searchCriteriaRegExactMatch As Boolean '
  Private _searchCriteriaSerDoNotSearchAlt As Boolean '
  Private _searchCriteriaDoNotSearchPrevRegNo As Boolean '
  Private _searchCriteriaHelicopterFlag As Boolean '
  Private _searchCriteriaBusinessFlag As Boolean '
  Private _searchCriteriaCommercialFlag As Boolean '
  Private _searchCriteriaYachtFlag As Boolean '
  Private _searchCriteriaYachtHasFilterFlag As Boolean
  Private _searchCriteriaRetailActivity As Boolean
  Private _searchCriteriaSalesOfNewAircraftOnly As Boolean '
  Private _searchCriteriaSalesOfUsedAircraftOnly As Boolean '
  Private _searchCriteriaCompanyNotInSelectedRelationship As Boolean '
  Private _searchCriteriaCompanyDisplayContactInfo As Boolean '
  Private _searchCriteriaCompanyOnlyAircraftSalesProfessionals As Boolean '

  Private _searchCriteriaCompanyDisplayInactiveCompanies As Boolean
  Private _searchCriteriaCompanyDisplayHiddenCompanies As Boolean

  Private _searchCriteriaCompanyDisplayInactiveContacts As Boolean
  Private _searchCriteriaCompanyDisplayHiddenContacts As Boolean

  Private _searchCriteriaExcludeInternalTransactions As Boolean

  Private _searchCriteriaYachtForSale As Boolean
  Private _searchCriteriaYachtForLease As Boolean
  Private _searchCriteriaYachtForCharter As Boolean
  Private _searchCriteriaYachtPreviousName As Boolean


  ' subscriber search items
  Private _searchCriteriaSub_user_id As String
  Private _searchCriteriaSub_login As String
  Private _searchCriteriaSub_id As Long
  Private _searchCriteriaSequence_number As Long
  Private _searchCriteriaService_code As String
  Private _searchCriteriaLast_login_date As String
  Private _searchCriteriaStart_date As String
  Private _searchCriteriaEnd_date As String
  Private _searchCriteriaLastHost As String

  Private _searchCriteriaAerodexFlag As Boolean
  Private _searchCriteriaDemoFlag As Boolean
  Private _searchCriteriaMarketingFlag As Boolean
  Private _searchCriteriaCRMFlag As Boolean
  Private _searchCriteriaSPIFlag As Boolean
  Private _searchCriteriaMobileFlag As Boolean
  Private _searchCriteriaLocalNotesFlag As Boolean
  Private _searchCriteriaCloudNotesFlag As Boolean
  Private _searchCriteriaNotesPlusFlag As Boolean
  Private _searchCriteriaActiveFlag As Boolean
  Private _searchCriteriaExpiredFlag As Boolean
  Private _searchCriteriaAdminFlag As Boolean

  Private _searchCriteriaParentSub As Boolean

  Private _searchCriteriaServicesString As String

  Private _searchCriteriaDisplayString As String
  Private _searchCriteriaQueryString As String


  'View Airport Operator Variables that need to be recalled from session. The view search class doesn't seem to be saved in session.
  'Adding variables for the Airport Operator View.
  Private _searchViewCriteriaOperatorDropdown As Long
  Private _searchViewCriteriaOperatorSelected As Long
  Private _searchViewCriteriaOperatorDropdown2 As Long

  Private _searchViewCriteriaAirportDropdown As Long
  Private _searchViewCriteriaAirportSelected As Long
  Private _searchViewCriteriaAirportDropdown2 As Long
  Private _searchViewCriteriaAirportFolderName As String
  Private _searchViewCriteriaOperatorFolderName As String
  Private _searchViewCriteriaAirportExcludeFolder As Boolean
  Private _searchViewCriteriaAircraftExcludeFolder As Boolean
  Private _searchViewCriteriaOperatorExcludeFolder As Boolean
  Private _searchViewCriteriaStartDate As String
  Private _searchViewCriteriaEndDate As String
  Private _searchViewCriteriaDefaultFolder As Boolean
  Private _searchViewCriteriaAircraftDropdown As Long
  Private _searchViewCriteriaAircraftDropdown2 As Long
  Private _searchViewCriteriaAircraftFolderName As String
  Private _searchViewCriteriaAirportCodes As String
  Private _searchViewCriteriaRegList As String
  Private _searchViewCriteriaBasedOn As String
  Private _searchViewCriteriaClearAirport As Boolean
  Private _searchViewCriteriaClearCompany As Boolean
#Region "Constructors"
  Sub New()

    _searchCriteriaStatusCode = eObjStatusCode.NULL
    _searchCriteriaDetailError = eObjDetailErrorCode.NULL

    'Integers 
    _searchCriteriaEventMonths = 0
    _searchCriteriaEventDays = 0
    _searchCriteriaEventHours = 0
    _searchCriteriaEventMinutes = 0
    _searchCriteriaCompanyFleetValue = 0


    _searchCriteriaYachtEventMonths = 0
    _searchCriteriaYachtEventDays = 0
    _searchCriteriaYachtEventHours = 0
    _searchCriteriaYachtEventMinutes = 0

    'Strings
    _searchCriteriaSerNoStart = ""
    _searchCriteriaSerNoEnd = ""
    _searchCriteriaRegNo = ""
    _searchCriteriaMarketStatus = ""
    _searchCriteriaOwnership = ""
    _searchCriteriaLeaseStatus = ""
    _searchCriteriaPreviouslyOwned = ""
    _searchCriteriaWeightClass = ""
    _searchCriteriaManufacturerName = ""
    _searchCriteriaAcSize = ""
    _searchCriteriaType = ""
    _searchCriteriaMake = ""
    _searchCriteriaModel = ""
    _searchCriteriaLifeCycle = ""
    _searchCriteriaHistoryType = ""
    _searchCriteriaHistoryFromOperator = ""
    _searchCriteriaHistoryFromAnswer = ""
    _searchCriteriaHistoryToOperator = ""
    _searchCriteriaHistoryToAnswer = ""
    _searchCriteriaHistoryDateOperator = ""
    _searchCriteriaHistoryDate = ""
    _searchCriteriaEventSearchType = ""
    _searchCriteriaEventCategory = ""
    _searchCriteriaEventType = ""

    _searchCriteriaCompanyID = 0
    _searchCriteriaCompanyName = ""
    _searchCriteriaCompanyAgencyType = ""
    _searchCriteriaCompanyRelationshipsToAC = ""
    _searchCriteriaCompanyAddress = ""
    _searchCriteriaCompanyCity = ""
    _searchCriteriaCompanyPostalCode = ""
    _searchCriteriaCompanyEmail = ""
    _searchCriteriaCompanyPhone = ""

    _searchCriteriaCompanyBusinessType = ""
    _searchCriteriaCompanyContinent = ""
    _searchCriteriaCompanyContinentOrRegion = ""
    _searchCriteriaCompanyRegion = ""
    _searchCriteriaCompanyStateName = ""
    _searchCriteriaCompanyStateProvince = ""
    _searchCriteriaCompanyTimezone = ""

    _searchCriteriaCompanyFleetOperator = ""
    _searchCriteriaCompanyFleetAnswer = ""

    _searchCriteriaCompanyContactID = 0
    _searchCriteriaCompanyContactFirstName = ""
    _searchCriteriaCompanyContactLastName = ""
    _searchCriteriaCompanyContactEmail = ""
    _searchCriteriaCompanyContactPhone = ""
    _searchCriteriaCompanyContactTitle = ""

    'Operating Cost Variables
    _searchCriteriaOpCostsFuelBurnOperator = ""
    _searchCriteriaOpCostsFuelBurn = ""
    _searchCriteriaOpCostsTotalDirectCostsOperator = ""
    _searchCriteriaOpCostsTotalDirectCosts = ""
    _searchCriteriaOpCostsCurrency = ""

    'Both Operating Cost and Performance Variables
    _searchCriteriaDisplayUnits = ""
    _searchCriteriaDisplayMiles = ""

    'Performance Specs Variables
    _searchCriteriaPerfSpecsSLISAOperator = ""
    _searchCriteriaPerfSpecsSLISA = ""
    _searchCriteriaPerfSpecsMaxRangeNBAAOrTanksFullOperator = ""
    _searchCriteriaPerfSpecsMaxRangeNBAAOrTanksFull = ""
    _searchCriteriaPerfSpecsFuselageLengthOperator = ""
    _searchCriteriaPerfSpecsFuselageLength = ""
    _searchCriteriaPerfSpecsFuselageHeightOperator = ""
    _searchCriteriaPerfSpecsFuselageHeight = ""
    _searchCriteriaPerfSpecsWingSpanOrWidthOperator = ""
    _searchCriteriaPerfSpecsWingSpanOrWidth = ""
    _searchCriteriaPerfSpecsCrewOperator = ""
    _searchCriteriaPerfSpecsCrew = ""
    _searchCriteriaPerfSpecsPassengersOperator = ""
    _searchCriteriaPerfSpecsPassengers = ""
    _searchCriteriaPerfSpecsMaxTakeoffOperator = ""
    _searchCriteriaPerfSpecsMaxTakeoff = ""
    _searchCriteriaPerfSpecsNormalCruiseOperator = ""
    _searchCriteriaPerfSpecsNormalCruise = ""
    _searchCriteriaPerfSpecsFuelCapacityOperator = ""
    _searchCriteriaPerfSpecsFuelCapacity = ""


    'Booleans
    _searchCriteriaRegExactMatch = False
    _searchCriteriaSerDoNotSearchAlt = False
    _searchCriteriaDoNotSearchPrevRegNo = False
    _searchCriteriaHelicopterFlag = False
    _searchCriteriaBusinessFlag = False
    _searchCriteriaCommercialFlag = False
    _searchCriteriaYachtFlag = False
    _searchCriteriaYachtHasFilterFlag = False
    _searchCriteriaRetailActivity = False
    _searchCriteriaSalesOfNewAircraftOnly = False
    _searchCriteriaSalesOfUsedAircraftOnly = False
    _searchCriteriaCompanyNotInSelectedRelationship = False
    _searchCriteriaCompanyDisplayContactInfo = False
    _searchCriteriaCompanyOnlyAircraftSalesProfessionals = False

    _searchCriteriaCompanyDisplayInactiveCompanies = False
    _searchCriteriaCompanyDisplayHiddenCompanies = False
    _searchCriteriaCompanyDisplayInactiveContacts = False
    _searchCriteriaCompanyDisplayHiddenContacts = False


    _searchCriteriaHasCompanyLocationInfo = False
    _searchCriteriaUseContinent = True
    _searchCriteriaUseRegion = False

    ' subscriber search items
    _searchCriteriaSub_user_id = ""
    _searchCriteriaSub_login = ""
    _searchCriteriaSub_id = 0
    _searchCriteriaSequence_number = 0
    _searchCriteriaService_code = ""
    _searchCriteriaLast_login_date = ""
    _searchCriteriaStart_date = ""
    _searchCriteriaEnd_date = ""
    _searchCriteriaLastHost = ""

    _searchCriteriaAerodexFlag = False
    _searchCriteriaDemoFlag = False
    _searchCriteriaMarketingFlag = False
    _searchCriteriaCRMFlag = False
    _searchCriteriaSPIFlag = False
    _searchCriteriaMobileFlag = False
    _searchCriteriaLocalNotesFlag = False
    _searchCriteriaCloudNotesFlag = False
    _searchCriteriaNotesPlusFlag = False
    _searchCriteriaActiveFlag = False
    _searchCriteriaExpiredFlag = False
    _searchCriteriaAdminFlag = False

    _searchCriteriaParentSub = False

    ' query and display strings 
    _searchCriteriaDisplayString = ""
    _searchCriteriaQueryString = ""
    _searchCriteriaCompanyNameQueryString = ""
    _searchCriteriaServicesString = ""

    'Yacht items
    _searchCriteriaYachtName = ""
    _searchCriteriaYachtFlagOption = ""
    _searchCriteriaYachtLengthOperator = ""
    _searchCriteriaYachtLengthValue = ""
    _searchCriteriaYachtLengthStandard = ""
    _searchCriteriaYachtClass = ""
    _searchCriteriaYachtCallSign = ""
    _searchCriteriaYachtYearDeliveredOperator = ""
    _searchCriteriaYachtYearDelivered = ""
    _searchCriteriaYachtYearManufacturedOperator = ""
    _searchCriteriaYachtYearManufactured = ""
    _searchCriteriaYachtAskingPriceOperator = ""
    _searchCriteriaYachtAskingPrice = ""
    _searchCriteriaYachtAskingPriceCurrency = ""
    _searchCriteriaYachtDOMOperator = ""
    _searchCriteriaYachtDOM = ""
    _searchCriteriaYachtMarketStatus = ""
    _searchCriteriaYachtLifecycle = ""
    _searchCriteriaYachtTransactionDateOperator = ""
    _searchCriteriaYachtTransactionDate = ""
    _searchCriteriaYachtTransactionType = ""
    _searchCriteriaYachtCategory = ""
    _searchCriteriaYachtType = ""
    _searchCriteriaYachtSaleCharterRestrictions = ""
    _searchCriteriaYachtForSale = False
    _searchCriteriaYachtForLease = False
    _searchCriteriaYachtForCharter = False
    _searchCriteriaYachtPreviousName = False



    _searchViewCriteriaOperatorDropdown = 0
    _searchViewCriteriaOperatorSelected = 0
    _searchViewCriteriaOperatorDropdown2 = 0
    _searchViewCriteriaOperatorFolderName = ""
    _searchViewCriteriaOperatorExcludeFolder = False

    _searchViewCriteriaAirportDropdown = 0
    _searchViewCriteriaAirportSelected = 0
    _searchViewCriteriaAirportDropdown2 = 0
    _searchViewCriteriaAirportFolderName = ""
    _searchViewCriteriaAirportExcludeFolder = False
    _searchViewCriteriaStartDate = ""
    _searchViewCriteriaEndDate = ""
    _searchViewCriteriaDefaultFolder = False
    _searchViewCriteriaAircraftDropdown = 0
    _searchViewCriteriaAircraftDropdown2 = 0
    _searchViewCriteriaAircraftFolderName = ""
    _searchViewCriteriaRegList = ""
    _searchViewCriteriaAirportCodes = ""
    _searchViewCriteriaAircraftExcludeFolder = False
    _searchViewCriteriaBasedOn = "D"
    _searchViewCriteriaClearAirport = False
    _searchViewCriteriaClearCompany = False
  End Sub
#End Region

#Region "Enums"
  Public Property SearchCriteriaStatusCode() As eObjStatusCode
    Get
      Return _searchCriteriaStatusCode
    End Get
    Set(ByVal value As eObjStatusCode)
      _searchCriteriaStatusCode = value
    End Set
  End Property
  Public Property SearchCriteriaDetailError() As eObjDetailErrorCode
    Get
      Return _searchCriteriaDetailError
    End Get
    Set(ByVal value As eObjDetailErrorCode)
      _searchCriteriaDetailError = value
    End Set
  End Property
#End Region

#Region "Long"

  Public Property SearchViewCriteriaAircraftDropdown() As Long
    Get
      Return _searchViewCriteriaAircraftDropdown
    End Get
    Set(ByVal value As Long)
      _searchViewCriteriaAircraftDropdown = value
    End Set
  End Property
  Public Property SearchViewCriteriaAircraftDropdown2() As Long
    Get
      Return _searchViewCriteriaAircraftDropdown2
    End Get
    Set(ByVal value As Long)
      _searchViewCriteriaAircraftDropdown2 = value
    End Set
  End Property

  Public Property SearchCriteriaViewAC() As Long
    Get
      Return _searchCriteriaViewAC
    End Get
    Set(ByVal value As Long)
      _searchCriteriaViewAC = value
    End Set
  End Property

  Public Property SearchViewCriteriaOperatorDropdown() As Long
    Get
      Return _searchViewCriteriaOperatorDropdown
    End Get
    Set(ByVal value As Long)
      _searchViewCriteriaOperatorDropdown = value
    End Set
  End Property
  Public Property SearchViewCriteriaOperatorSelected() As Long
    Get
      Return _searchViewCriteriaOperatorSelected
    End Get
    Set(ByVal value As Long)
      _searchViewCriteriaOperatorSelected = value
    End Set
  End Property
  Public Property SearchViewCriteriaOperatorDropdown2() As Long
    Get
      Return _searchViewCriteriaOperatorDropdown2
    End Get
    Set(ByVal value As Long)
      _searchViewCriteriaOperatorDropdown2 = value
    End Set
  End Property
  Public Property SearchViewCriteriaAirportDropdown() As Long
    Get
      Return _searchViewCriteriaAirportDropdown
    End Get
    Set(ByVal value As Long)
      _searchViewCriteriaAirportDropdown = value
    End Set
  End Property
  Public Property SearchViewCriteriaAirportSelected() As Long
    Get
      Return _searchViewCriteriaAirportSelected
    End Get
    Set(ByVal value As Long)
      _searchViewCriteriaAirportSelected = value
    End Set
  End Property
  Public Property SearchViewCriteriaAirportDropdown2() As Long
    Get
      Return _searchViewCriteriaAirportDropdown2
    End Get
    Set(ByVal value As Long)
      _searchViewCriteriaAirportDropdown2 = value
    End Set
  End Property

#End Region

#Region "Integers"
  Public Property SearchCriteriaViewModel() As Integer
    Get
      Return _searchCriteriaViewModel
    End Get
    Set(ByVal value As Integer)
      _searchCriteriaViewModel = value
    End Set
  End Property

  ''' <summary>
  ''' Event Months Search Field.
  ''' Found on Event Search Listing.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaEventMonths() As Integer
    Get
      Return _searchCriteriaEventMonths
    End Get
    Set(ByVal value As Integer)
      _searchCriteriaEventMonths = value
    End Set
  End Property

  ''' <summary>
  ''' Event Days Search Field.
  ''' Found on Event Search Listing.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaEventDays() As Integer
    Get
      Return _searchCriteriaEventDays
    End Get
    Set(ByVal value As Integer)
      _searchCriteriaEventDays = value
    End Set
  End Property
  ''' <summary>
  ''' Event Hours Search Field.
  ''' Found on Event Search Listing.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaEventHours() As Integer
    Get
      Return _searchCriteriaEventHours
    End Get
    Set(ByVal value As Integer)
      _searchCriteriaEventHours = value
    End Set
  End Property
  ''' <summary>
  ''' Event Minutes Search Field.
  ''' Found on Event Search Listing.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaEventMinutes() As Integer
    Get
      Return _searchCriteriaEventMinutes
    End Get
    Set(ByVal value As Integer)
      _searchCriteriaEventMinutes = value
    End Set
  End Property
  ''' <summary>
  ''' Company Fleet Value.
  ''' Found on Company Search Listing
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaCompanyFleetValue() As Integer
    Get
      Return _searchCriteriaCompanyFleetValue
    End Get
    Set(ByVal value As Integer)
      _searchCriteriaCompanyFleetValue = value
    End Set
  End Property

  Public Property SearchCriteriaCompanyID() As Integer
    Get
      Return _searchCriteriaCompanyID
    End Get
    Set(ByVal value As Integer)
      _searchCriteriaCompanyID = value
    End Set
  End Property

  Public Property SearchCriteriaCompanyContactID() As Integer
    Get
      Return _searchCriteriaCompanyContactID
    End Get
    Set(ByVal value As Integer)
      _searchCriteriaCompanyContactID = value
    End Set
  End Property


  ''' <summary>
  ''' Event Months Search Field.
  ''' Found on Yacht Event Search Listing.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaYachtEventMonths() As Integer
    Get
      Return _searchCriteriaYachtEventMonths
    End Get
    Set(ByVal value As Integer)
      _searchCriteriaYachtEventMonths = value
    End Set
  End Property
  ''' <summary>
  ''' Event Days Search Field.
  ''' Found on Yacht Event Search Listing.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaYachtEventDays() As Integer
    Get
      Return _searchCriteriaYachtEventDays
    End Get
    Set(ByVal value As Integer)
      _searchCriteriaYachtEventDays = value
    End Set
  End Property
  ''' <summary>
  ''' Event Hours Search Field.
  ''' Found on Yacht Event Search Listing.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaYachtEventHours() As Integer
    Get
      Return _searchCriteriaYachtEventHours
    End Get
    Set(ByVal value As Integer)
      _searchCriteriaYachtEventHours = value
    End Set
  End Property
  ''' <summary>
  ''' Event Minutes Search Field.
  ''' Found on Yacht Event Search Listing.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaYachtEventMinutes() As Integer
    Get
      Return _searchCriteriaYachtEventMinutes
    End Get
    Set(ByVal value As Integer)
      _searchCriteriaYachtEventMinutes = value
    End Set
  End Property
#End Region

#Region "Strings"
  Public Property SearchViewCriteriaRegList() As String
    Get
      Return _searchViewCriteriaRegList
    End Get
    Set(ByVal value As String)
      _searchViewCriteriaRegList = value
    End Set
  End Property
  Public Property SearchViewCriteriaAirportCodes() As String
    Get
      Return _searchViewCriteriaAirportCodes
    End Get
    Set(ByVal value As String)
      _searchViewCriteriaAirportCodes = value
    End Set
  End Property
  Public Property SearchViewCriteriaStartDate() As String
    Get
      Return _searchViewCriteriaStartDate
    End Get
    Set(ByVal value As String)
      _searchViewCriteriaStartDate = value
    End Set
  End Property
  Public Property SearchViewCriteriaEndDate() As String
    Get
      Return _searchViewCriteriaEndDate
    End Get
    Set(ByVal value As String)
      _searchViewCriteriaEndDate = value
    End Set
  End Property
  Public Property SearchViewCriteriaBasedOn() As String
    Get
      Return _searchViewCriteriaBasedOn
    End Get
    Set(ByVal value As String)
      _searchViewCriteriaBasedOn = value
    End Set
  End Property
  Public Property SearchViewCriteriaAircraftFolderName() As String
    Get
      Return _searchViewCriteriaAircraftFolderName
    End Get
    Set(ByVal value As String)
      _searchViewCriteriaAircraftFolderName = value
    End Set
  End Property

  Public Property SearchViewCriteriaOperatorFolderName() As String
    Get
      Return _searchViewCriteriaOperatorFolderName
    End Get
    Set(ByVal value As String)
      _searchViewCriteriaOperatorFolderName = value
    End Set
  End Property
  Public Property SearchViewCriteriaAirportFolderName() As String
    Get
      Return _searchViewCriteriaAirportFolderName
    End Get
    Set(ByVal value As String)
      _searchViewCriteriaAirportFolderName = value
    End Set
  End Property
  ''' <summary>
  ''' SLI SA Operator on Performance Specs
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaPerfSpecsSLISAOperator() As String
    Get
      Return _searchCriteriaPerfSpecsSLISAOperator
    End Get
    Set(ByVal value As String)
      _searchCriteriaPerfSpecsSLISAOperator = value
    End Set
  End Property
  ''' <summary>
  ''' View Features List (selectable features on value view)
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaViewFeatureString() As String
    Get
      Return _searchCriteriaViewFeatureString
    End Get
    Set(ByVal value As String)
      _searchCriteriaViewFeatureString = value
    End Set
  End Property
  ''' <summary>
  ''' View Variant Models List (Value View)
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaViewVariantString() As String
    Get
      Return _searchCriteriaViewVariantString
    End Get
    Set(ByVal value As String)
      _searchCriteriaViewVariantString = value
    End Set
  End Property

  ''' <summary>
  ''' SLI SA value on Performance Specs
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaPerfSpecsSLISA() As String
    Get
      Return _searchCriteriaPerfSpecsSLISA
    End Get
    Set(ByVal value As String)
      _searchCriteriaPerfSpecsSLISA = value
    End Set
  End Property

  ''' <summary>
  ''' Max Range (either NBAA IFR or Tanks Full) Operator on performance specs
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaPerfSpecsMaxRangeNBAAOrTanksFullOperator() As String
    Get
      Return _searchCriteriaPerfSpecsMaxRangeNBAAOrTanksFullOperator
    End Get
    Set(ByVal value As String)
      _searchCriteriaPerfSpecsMaxRangeNBAAOrTanksFullOperator = value
    End Set
  End Property
  ''' <summary>
  ''' Max Range (either NBAA IFR or Tanks Full) value on performance specs
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaPerfSpecsMaxRangeNBAAOrTanksFull() As String
    Get
      Return _searchCriteriaPerfSpecsMaxRangeNBAAOrTanksFull
    End Get
    Set(ByVal value As String)
      _searchCriteriaPerfSpecsMaxRangeNBAAOrTanksFull = value
    End Set
  End Property
  ''' <summary>
  ''' Fuselage Length Operator on Performance Specs
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaPerfSpecsFuselageLengthOperator() As String
    Get
      Return _searchCriteriaPerfSpecsFuselageLengthOperator
    End Get
    Set(ByVal value As String)
      _searchCriteriaPerfSpecsFuselageLengthOperator = value
    End Set
  End Property
  ''' <summary>
  ''' Fuselage Length Value on Performance Specs
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaPerfSpecsFuselageLength() As String
    Get
      Return _searchCriteriaPerfSpecsFuselageLength
    End Get
    Set(ByVal value As String)
      _searchCriteriaPerfSpecsFuselageLength = value
    End Set
  End Property

  ''' <summary>
  ''' Fuselage Height Operator on Performance Specs
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaPerfSpecsFuselageHeightOperator() As String
    Get
      Return _searchCriteriaPerfSpecsFuselageHeightOperator
    End Get
    Set(ByVal value As String)
      _searchCriteriaPerfSpecsFuselageHeightOperator = value
    End Set
  End Property
  ''' <summary>
  ''' Fuselage Height Value on Performance Specs
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaPerfSpecsFuselageHeight() As String
    Get
      Return _searchCriteriaPerfSpecsFuselageHeight
    End Get
    Set(ByVal value As String)
      _searchCriteriaPerfSpecsFuselageHeight = value
    End Set
  End Property

  ''' <summary>
  ''' Performance Specs - Wing Span (Or Width) Operator
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaPerfSpecsWingSpanOrWidthOperator() As String
    Get
      Return _searchCriteriaPerfSpecsWingSpanOrWidthOperator
    End Get
    Set(ByVal value As String)
      _searchCriteriaPerfSpecsWingSpanOrWidthOperator = value
    End Set
  End Property
  ''' <summary>
  ''' Performance Specs - Wing Span (Or Width) Value
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaPerfSpecsWingSpanOrWidth() As String
    Get
      Return _searchCriteriaPerfSpecsWingSpanOrWidth
    End Get
    Set(ByVal value As String)
      _searchCriteriaPerfSpecsWingSpanOrWidth = value
    End Set
  End Property

  ''' <summary>
  ''' Crew Operator on Performance Specs
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaPerfSpecsCrewOperator() As String
    Get
      Return _searchCriteriaPerfSpecsCrewOperator
    End Get
    Set(ByVal value As String)
      _searchCriteriaPerfSpecsCrewOperator = value
    End Set
  End Property
  ''' <summary>
  ''' Crew Value on Performance Specs
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaPerfSpecsCrew() As String
    Get
      Return _searchCriteriaPerfSpecsCrew
    End Get
    Set(ByVal value As String)
      _searchCriteriaPerfSpecsCrew = value
    End Set
  End Property

  ''' <summary>
  ''' Passenger Operator on Performance Specs
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaPerfSpecsPassengersOperator() As String
    Get
      Return _searchCriteriaPerfSpecsPassengersOperator
    End Get
    Set(ByVal value As String)
      _searchCriteriaPerfSpecsPassengersOperator = value
    End Set
  End Property
  ''' <summary>
  ''' Passenger Value on Performance Specs
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaPerfSpecsPassengers() As String
    Get
      Return _searchCriteriaPerfSpecsPassengers
    End Get
    Set(ByVal value As String)
      _searchCriteriaPerfSpecsPassengers = value
    End Set
  End Property

  ''' <summary>
  ''' Max Takeoff Operator on Performance Specs
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaPerfSpecsMaxTakeoffOperator() As String
    Get
      Return _searchCriteriaPerfSpecsMaxTakeoffOperator
    End Get
    Set(ByVal value As String)
      _searchCriteriaPerfSpecsMaxTakeoffOperator = value
    End Set
  End Property

  ''' <summary>
  ''' Max Takeoff Value on Performance Specs
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaPerfSpecsMaxTakeoff() As String
    Get
      Return _searchCriteriaPerfSpecsMaxTakeoff
    End Get
    Set(ByVal value As String)
      _searchCriteriaPerfSpecsMaxTakeoff = value
    End Set
  End Property


  ''' <summary>
  ''' Normal Cruise Operator on Performance Specs
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaPerfSpecsNormalCruiseOperator() As String
    Get
      Return _searchCriteriaPerfSpecsNormalCruiseOperator
    End Get
    Set(ByVal value As String)
      _searchCriteriaPerfSpecsNormalCruiseOperator = value
    End Set
  End Property
  ''' <summary>
  ''' Normal Cruise Value on Performance Specs
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaPerfSpecsNormalCruise() As String
    Get
      Return _searchCriteriaPerfSpecsNormalCruise
    End Get
    Set(ByVal value As String)
      _searchCriteriaPerfSpecsNormalCruise = value
    End Set
  End Property
  ''' <summary>
  ''' FuelCapacity Operator on Performance Specs
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaPerfSpecsFuelCapacityOperator() As String
    Get
      Return _searchCriteriaPerfSpecsFuelCapacityOperator
    End Get
    Set(ByVal value As String)
      _searchCriteriaPerfSpecsFuelCapacityOperator = value
    End Set
  End Property
  ''' <summary>
  ''' FuelCapacity Value on Performance Specs
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaPerfSpecsFuelCapacity() As String
    Get
      Return _searchCriteriaPerfSpecsFuelCapacity
    End Get
    Set(ByVal value As String)
      _searchCriteriaPerfSpecsFuelCapacity = value
    End Set
  End Property
  ''' <summary>
  ''' Display Units on the Op Costs/Performance Specs page
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaDisplayUnits() As String
    Get
      Return _searchCriteriaDisplayUnits
    End Get
    Set(ByVal value As String)
      _searchCriteriaDisplayUnits = value
    End Set
  End Property

  ''' <summary>
  ''' Display Miles on the Op Costs/Performance Specs page
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaDisplayMiles() As String
    Get
      Return _searchCriteriaDisplayMiles
    End Get
    Set(ByVal value As String)
      _searchCriteriaDisplayMiles = value
    End Set
  End Property

  ''' <summary>
  ''' Saves Fuel Burn Operator on the Op Costs page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaOpCostsFuelBurnOperator() As String
    Get
      Return _searchCriteriaOpCostsFuelBurnOperator
    End Get
    Set(ByVal value As String)
      _searchCriteriaOpCostsFuelBurnOperator = value
    End Set
  End Property
  ''' <summary>
  ''' Fuel Burn Value on Op Costs page
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaOpCostsFuelBurn() As String
    Get
      Return _searchCriteriaOpCostsFuelBurn
    End Get
    Set(ByVal value As String)
      _searchCriteriaOpCostsFuelBurn = value
    End Set
  End Property

  ''' <summary>
  ''' Total Direct Costs Operator on Op Costs page
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaOpCostsTotalDirectCostsOperator() As String
    Get
      Return _searchCriteriaOpCostsTotalDirectCostsOperator
    End Get
    Set(ByVal value As String)
      _searchCriteriaOpCostsTotalDirectCostsOperator = value
    End Set
  End Property
  ''' <summary>
  ''' Total Direct Costs value on Op Costs page
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaOpCostsTotalDirectCosts() As String
    Get
      Return _searchCriteriaOpCostsTotalDirectCosts
    End Get
    Set(ByVal value As String)
      _searchCriteriaOpCostsTotalDirectCosts = value
    End Set
  End Property
  ''' <summary>
  ''' Currency on Op Costs Page
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaOpCostsCurrency() As String
    Get
      Return _searchCriteriaOpCostsCurrency
    End Get
    Set(ByVal value As String)
      _searchCriteriaOpCostsCurrency = value
    End Set
  End Property

  ''' <summary>
  ''' LifeCycle Field.
  ''' Found On: Aircraft Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaLifeCycle() As String
    Get
      Return _searchCriteriaLifeCycle
    End Get
    Set(ByVal value As String)
      _searchCriteriaLifeCycle = value
    End Set
  End Property
  ''' <summary>
  ''' Model Search Field.
  ''' Found on Aircraft, Wanted, History, Performance Specs, Operating Costs, Events Listing.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaModel() As String
    Get
      Return _searchCriteriaModel
    End Get
    Set(ByVal value As String)
      _searchCriteriaModel = value
    End Set
  End Property
  ''' <summary>
  ''' Make Search Field.
  ''' Found on Aircraft, Wanted, History, Performance Specs, Operating Costs, Events Listing Pages.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaMake() As String
    Get
      Return _searchCriteriaMake
    End Get
    Set(ByVal value As String)
      _searchCriteriaMake = value
    End Set
  End Property
  ''' <summary>
  ''' Type Search Field. 
  ''' Found on Aircraft, Wanted, History, Performance Specs, Operating Costs, Events Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaType() As String
    Get
      Return _searchCriteriaType
    End Get
    Set(ByVal value As String)
      _searchCriteriaType = value
    End Set
  End Property
  ''' <summary>
  ''' Weight Class Search Field.
  ''' Found on Aircraft, Wanted, History, Performance Specs, Operating Costs, Events and Market Summary Listing Pages.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaWeightClass() As String
    Get
      Return _searchCriteriaWeightClass
    End Get
    Set(ByVal value As String)
      _searchCriteriaWeightClass = value
    End Set
  End Property

  ''' <summary>
  ''' Manufacturer Name Search Field.
  ''' Found on Aircraft, Wanted, History, Performance Specs, Operating Costs, Events and Market Summary Listing Pages.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaManufacturerName() As String
    Get
      Return _searchCriteriaManufacturerName
    End Get
    Set(ByVal value As String)
      _searchCriteriaManufacturerName = value
    End Set
  End Property

  ''' <summary>
  ''' Aircraft Size Search Field.  
  ''' Found on Aircraft, Wanted, History, Performance Specs, Operating Costs, Events and Market Summary Listing Pages.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaAcSize() As String
    Get
      Return _searchCriteriaAcSize
    End Get
    Set(ByVal value As String)
      _searchCriteriaAcSize = value
    End Set
  End Property

  ''' <summary>
  ''' Previously Owned Search Field.
  ''' Found on Aircraft Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaPreviouslyOwned() As String
    Get
      Return _searchCriteriaPreviouslyOwned
    End Get
    Set(ByVal value As String)
      _searchCriteriaPreviouslyOwned = value
    End Set
  End Property
  ''' <summary>
  ''' Lease Status Search Field.
  ''' Found on Aircraft Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaLeaseStatus() As String
    Get
      Return _searchCriteriaLeaseStatus
    End Get
    Set(ByVal value As String)
      _searchCriteriaLeaseStatus = value
    End Set
  End Property
  ''' <summary>
  ''' Ownership Search Field.
  ''' Found On Aircraft Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaOwnership() As String
    Get
      Return _searchCriteriaOwnership
    End Get
    Set(ByVal value As String)
      _searchCriteriaOwnership = value
    End Set
  End Property
  ''' <summary>
  ''' Serial Number Start Field. 
  ''' Found On:
  ''' Aircraft, Events, History Listing Pages.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaSerNoStart() As String
    Get
      Return _searchCriteriaSerNoStart
    End Get
    Set(ByVal value As String)
      _searchCriteriaSerNoStart = value
    End Set
  End Property
  ''' <summary>
  ''' Serial Number End Field.
  ''' Found On:
  ''' Aircraft, Events, History Listing Pages.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaSerNoEnd() As String
    Get
      Return _searchCriteriaSerNoEnd
    End Get
    Set(ByVal value As String)
      _searchCriteriaSerNoEnd = value
    End Set
  End Property
  ''' <summary>
  ''' Registration Number Field. 
  ''' Found On: Aircraft, History, Events Listing Pages.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaRegNo() As String
    Get
      Return _searchCriteriaRegNo
    End Get
    Set(ByVal value As String)
      _searchCriteriaRegNo = value
    End Set
  End Property
  ''' <summary>
  ''' Market Status Search Field
  ''' Found On: Aircraft Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaMarketStatus() As String
    Get
      Return _searchCriteriaMarketStatus
    End Get
    Set(ByVal value As String)
      _searchCriteriaMarketStatus = value
    End Set
  End Property

  ''' <summary>
  ''' History Type Search Field.
  ''' Found on History Listing.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaHistoryType() As String
    Get
      Return _searchCriteriaHistoryType
    End Get
    Set(ByVal value As String)
      _searchCriteriaHistoryType = value
    End Set
  End Property
  ''' <summary>
  ''' History From Operator Search Field.
  ''' Found on History Listing.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaHistoryFromOperator() As String
    Get
      Return _searchCriteriaHistoryFromOperator
    End Get
    Set(ByVal value As String)
      _searchCriteriaHistoryFromOperator = value
    End Set
  End Property
  ''' <summary>
  ''' History From Answer Search Field.
  ''' Found on History Listing.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaHistoryFromAnswer() As String
    Get
      Return _searchCriteriaHistoryFromAnswer
    End Get
    Set(ByVal value As String)
      _searchCriteriaHistoryFromAnswer = value
    End Set
  End Property
  ''' <summary>
  ''' History To Operator Search Field.
  ''' Found on History Listing.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaHistoryToOperator() As String
    Get
      Return _searchCriteriaHistoryToOperator
    End Get
    Set(ByVal value As String)
      _searchCriteriaHistoryToOperator = value
    End Set
  End Property
  ''' <summary>
  ''' History From Answer Search Field.
  ''' Found on History Listing.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaHistoryToAnswer() As String
    Get
      Return _searchCriteriaHistoryToAnswer
    End Get
    Set(ByVal value As String)
      _searchCriteriaHistoryToAnswer = value
    End Set
  End Property
  ''' <summary>
  ''' History Date Operator Search Field.
  ''' Found on History Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaHistoryDateOperator() As String
    Get
      Return _searchCriteriaHistoryDateOperator
    End Get
    Set(ByVal value As String)
      _searchCriteriaHistoryDateOperator = value
    End Set
  End Property
  ''' <summary>
  ''' History Date Search Field.
  ''' Found on History Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaHistoryDate() As String
    Get
      Return _searchCriteriaHistoryDate
    End Get
    Set(ByVal value As String)
      _searchCriteriaHistoryDate = value
    End Set
  End Property

  ''' <summary>
  ''' Event Search Type, Aircraft, Wanted, Company Search
  ''' Found on Event Listing
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaEventSearchType() As String
    Get
      Return _searchCriteriaEventSearchType
    End Get
    Set(ByVal value As String)
      _searchCriteriaEventSearchType = value
    End Set
  End Property
  ''' <summary>
  ''' Event Category Search Field.
  ''' Found on Event Listing.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaEventCategory() As String
    Get
      Return _searchCriteriaEventCategory
    End Get
    Set(ByVal value As String)
      _searchCriteriaEventCategory = value
    End Set
  End Property
  ''' <summary>
  ''' Event Type Search Field.
  ''' Found on Event Listing.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaEventType() As String
    Get
      Return _searchCriteriaEventType
    End Get
    Set(ByVal value As String)
      _searchCriteriaEventType = value
    End Set
  End Property

  ''' <summary>
  ''' Company Name Search Field.
  ''' Found on Company Listing.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaCompanyName() As String
    Get
      Return _searchCriteriaCompanyName
    End Get
    Set(ByVal value As String)
      _searchCriteriaCompanyName = value
    End Set
  End Property
  ''' <summary>
  ''' Company Agency Type Search Field.
  ''' Found on Company Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaCompanyAgencyType() As String
    Get
      Return _searchCriteriaCompanyAgencyType
    End Get
    Set(ByVal value As String)
      _searchCriteriaCompanyAgencyType = value
    End Set
  End Property

  ''' <summary>
  ''' Company Relationship to Aircraft Search Field.
  ''' Found on Company Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaCompanyRelationshipsToAC() As String
    Get
      Return _searchCriteriaCompanyRelationshipsToAC
    End Get
    Set(ByVal value As String)
      _searchCriteriaCompanyRelationshipsToAC = value
    End Set
  End Property

  ''' <summary>
  ''' Company Address Search Field.
  ''' Found on Company Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaCompanyAddress() As String
    Get
      Return _searchCriteriaCompanyAddress
    End Get
    Set(ByVal value As String)
      _searchCriteriaCompanyAddress = value
    End Set
  End Property

  ''' <summary>
  ''' Company City Search Field.
  ''' Found on Company Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaCompanyCity() As String
    Get
      Return _searchCriteriaCompanyCity
    End Get
    Set(ByVal value As String)
      _searchCriteriaCompanyCity = value
    End Set
  End Property

  ''' <summary>
  ''' Company Postal Code Search Field.
  ''' Found on Company Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaCompanyPostalCode() As String
    Get
      Return _searchCriteriaCompanyPostalCode
    End Get
    Set(ByVal value As String)
      _searchCriteriaCompanyPostalCode = value
    End Set
  End Property
  ''' <summary>
  ''' Company Email Search Field.
  ''' Found on Company Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaCompanyEmail() As String
    Get
      Return _searchCriteriaCompanyEmail
    End Get
    Set(ByVal value As String)
      _searchCriteriaCompanyEmail = value
    End Set
  End Property

  ''' <summary>
  ''' Company Phone Search Field.
  ''' Found on Company Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaCompanyPhone() As String
    Get
      Return _searchCriteriaCompanyPhone
    End Get
    Set(ByVal value As String)
      _searchCriteriaCompanyPhone = value
    End Set
  End Property

  ''' <summary>
  ''' Company Business Type Search Field.
  ''' Found on Company Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaCompanyBusinessType() As String
    Get
      Return _searchCriteriaCompanyBusinessType
    End Get
    Set(ByVal value As String)
      _searchCriteriaCompanyBusinessType = value
    End Set
  End Property

  ''' <summary>
  ''' Company Continent Search Field.
  ''' Found on Company Listing Page, Aircraft Advanced Search.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaCompanyContinent() As String
    Get
      Return _searchCriteriaCompanyContinent
    End Get
    Set(ByVal value As String)
      _searchCriteriaCompanyContinent = value
    End Set
  End Property

  ''' <summary>
  ''' Company Continent or Region Choice Search Field.
  ''' Found on Company Listing Page, Aircraft Advanced Search.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaCompanyContinentOrRegion() As String
    Get
      Return _searchCriteriaCompanyContinentOrRegion
    End Get
    Set(ByVal value As String)
      _searchCriteriaCompanyContinentOrRegion = value
    End Set
  End Property

  ''' <summary>
  ''' Company Region Search Field.
  ''' Found on Company Listing Page, Aircraft Advanced Search.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaCompanyRegion() As String
    Get
      Return _searchCriteriaCompanyRegion
    End Get
    Set(ByVal value As String)
      _searchCriteriaCompanyRegion = value
    End Set
  End Property
  ''' <summary>
  ''' Company State Name Search Field.
  ''' Found on Company Listing Page, Aircraft Advanced Search.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaCompanyStateName() As String
    Get
      Return _searchCriteriaCompanyStateName
    End Get
    Set(ByVal value As String)
      _searchCriteriaCompanyStateName = value
    End Set
  End Property
  ''' <summary>
  ''' Company State Province Search Field.
  ''' Found on Company Listing Page, Aircraft Advanced Search.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaCompanyStateProvince() As String
    Get
      Return _searchCriteriaCompanyStateProvince
    End Get
    Set(ByVal value As String)
      _searchCriteriaCompanyStateProvince = value
    End Set
  End Property

  ''' <summary>
  ''' Company Timezone Search Field.
  ''' Found on Company Listing Page, Aircraft Advanced Search.   
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaCompanyTimezone() As String
    Get
      Return _searchCriteriaCompanyTimezone
    End Get
    Set(ByVal value As String)
      _searchCriteriaCompanyTimezone = value
    End Set
  End Property

  Public Property SearchCriteriaCompanyCertifications() As String
    Get
      Return _searchCriteriaCompanyCertifications
    End Get
    Set(ByVal value As String)
      _searchCriteriaCompanyCertifications = value
    End Set
  End Property
  ''' <summary>
  ''' Company Yacht Fleet Dropdown.
  ''' Found on Company Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaCompanyYachtFleet() As String
    Get
      Return _searchCriteriaCompanyYachtFleet
    End Get
    Set(ByVal value As String)
      _searchCriteriaCompanyYachtFleet = value
    End Set
  End Property


  ''' <summary>
  ''' Company Fleet Operator Search Field.
  ''' Found on Company Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaCompanyFleetOperator() As String
    Get
      Return _searchCriteriaCompanyFleetOperator
    End Get
    Set(ByVal value As String)
      _searchCriteriaCompanyFleetOperator = value
    End Set
  End Property

  ''' <summary>
  ''' Company Fleet Answer Search Field.
  ''' Found on Company Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaCompanyFleetAnswer() As String
    Get
      Return _searchCriteriaCompanyFleetAnswer
    End Get
    Set(ByVal value As String)
      _searchCriteriaCompanyFleetAnswer = value
    End Set
  End Property
  ''' <summary>
  ''' Company Contact First Name Search Field.
  ''' Found on Company Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaCompanyContactFirstName() As String
    Get
      Return _searchCriteriaCompanyContactFirstName
    End Get
    Set(ByVal value As String)
      _searchCriteriaCompanyContactFirstName = value
    End Set
  End Property
  ''' <summary>
  ''' Company Contact Last Name Search Field.
  ''' Found on Company Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaCompanyContactLastName() As String
    Get
      Return _searchCriteriaCompanyContactLastName
    End Get
    Set(ByVal value As String)
      _searchCriteriaCompanyContactLastName = value
    End Set
  End Property

  ''' <summary>
  ''' Company Email Search Field.
  ''' Found on Company Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaCompanyContactEmail() As String
    Get
      Return _searchCriteriaCompanyContactEmail
    End Get
    Set(ByVal value As String)
      _searchCriteriaCompanyContactEmail = value
    End Set
  End Property

  ''' <summary>
  ''' Company Phone Search Field.
  ''' Found on Company Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaCompanyContactPhone() As String
    Get
      Return _searchCriteriaCompanyContactPhone
    End Get
    Set(ByVal value As String)
      _searchCriteriaCompanyContactPhone = value
    End Set
  End Property

  ''' <summary>
  ''' Company Contact Title Search Field.
  ''' Found on Company Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaCompanyContactTitle() As String
    Get
      Return _searchCriteriaCompanyContactTitle
    End Get
    Set(ByVal value As String)
      _searchCriteriaCompanyContactTitle = value
    End Set
  End Property

  ''' <summary>
  ''' Yacht Name Search Field
  ''' Found on Yacht Listing page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaYachtName() As String
    Get
      Return _searchCriteriaYachtName
    End Get
    Set(ByVal value As String)
      _searchCriteriaYachtName = value
    End Set
  End Property

  ''' <summary>
  ''' Yacht Flag Search Field
  ''' Found on Yacht Listing Page
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaYachtFlagOption() As String
    Get
      Return _searchCriteriaYachtFlagOption
    End Get
    Set(ByVal value As String)
      _searchCriteriaYachtFlagOption = value
    End Set
  End Property


  ''' <summary>
  ''' Yacht Length Operator Search Field.
  ''' Found on Yacht Listing Page
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaYachtLengthOperator() As String
    Get
      Return _searchCriteriaYachtLengthOperator
    End Get
    Set(ByVal value As String)
      _searchCriteriaYachtLengthOperator = value
    End Set
  End Property

  ''' <summary>
  ''' Yacht Length Value Search Field.
  ''' Found on Yacht Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaYachtLengthValue() As String
    Get
      Return _searchCriteriaYachtLengthValue
    End Get
    Set(ByVal value As String)
      _searchCriteriaYachtLengthValue = value
    End Set
  End Property

  ''' <summary>
  ''' Yacht Length Metric/US Value
  ''' Found on Yacht Listing Page
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaYachtLengthStandard() As String
    Get
      Return _searchCriteriaYachtLengthStandard
    End Get
    Set(ByVal value As String)
      _searchCriteriaYachtLengthStandard = value
    End Set
  End Property

  ''' <summary>
  ''' Yacht Class Field
  ''' Found on Yacht Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaYachtClass() As String
    Get
      Return _searchCriteriaYachtClass
    End Get
    Set(ByVal value As String)
      _searchCriteriaYachtClass = value
    End Set
  End Property

  ''' <summary>
  ''' Yacht Call Sign.
  ''' Yacht Listing Search Field.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaYachtCallSign() As String
    Get
      Return _searchCriteriaYachtCallSign
    End Get
    Set(ByVal value As String)
      _searchCriteriaYachtCallSign = value
    End Set
  End Property

  ''' <summary>
  ''' Yacht Year Delivered Operator Field.
  ''' Found Yacht Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaYachtYearDeliveredOperator() As String
    Get
      Return _searchCriteriaYachtYearDeliveredOperator
    End Get
    Set(ByVal value As String)
      _searchCriteriaYachtYearDeliveredOperator = value
    End Set
  End Property

  ''' <summary>
  ''' Yacht Year Delivered Field.
  ''' Found Yacht Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaYachtYearDelivered() As String
    Get
      Return _searchCriteriaYachtYearDelivered
    End Get
    Set(ByVal value As String)
      _searchCriteriaYachtYearDelivered = value
    End Set
  End Property

  ''' <summary>
  ''' Yacht Year Manufactured Operator Search Field.
  ''' Found on Yacht Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaYachtYearManufacturedOperator() As String
    Get
      Return _searchCriteriaYachtYearManufacturedOperator
    End Get
    Set(ByVal value As String)
      _searchCriteriaYachtYearManufacturedOperator = value
    End Set
  End Property

  ''' <summary>
  ''' Yacht Year Manufactured Search Field.
  ''' Found on Yacht Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaYachtYearManufactured() As String
    Get
      Return _searchCriteriaYachtYearManufactured
    End Get
    Set(ByVal value As String)
      _searchCriteriaYachtYearManufactured = value
    End Set
  End Property

  ''' <summary>
  ''' Yacht Asking Price Operator.
  ''' Found on Yacht Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaYachtAskingPriceOperator() As String
    Get
      Return _searchCriteriaYachtAskingPriceOperator
    End Get
    Set(ByVal value As String)
      _searchCriteriaYachtAskingPriceOperator = value
    End Set
  End Property

  ''' <summary>
  ''' Yacht Asking Price.
  ''' Found on Yacht Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaYachtAskingPrice() As String
    Get
      Return _searchCriteriaYachtAskingPrice
    End Get
    Set(ByVal value As String)
      _searchCriteriaYachtAskingPrice = value
    End Set
  End Property

  ''' <summary>
  ''' Currency for Yacht Asking Price.
  ''' Found on Yacht Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaYachtAskingPriceCurrency() As String
    Get
      Return _searchCriteriaYachtAskingPriceCurrency
    End Get
    Set(ByVal value As String)
      _searchCriteriaYachtAskingPriceCurrency = value
    End Set
  End Property


  ''' <summary>
  ''' Yacht Days on Market Operator.
  ''' Found on Yacht Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaYachtDOMOperator() As String
    Get
      Return _searchCriteriaYachtDOMOperator
    End Get
    Set(ByVal value As String)
      _searchCriteriaYachtDOMOperator = value
    End Set
  End Property

  ''' <summary>
  ''' Yacht Days on Market.
  ''' Found on Yacht Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaYachtDOM() As String
    Get
      Return _searchCriteriaYachtDOM
    End Get
    Set(ByVal value As String)
      _searchCriteriaYachtDOM = value
    End Set
  End Property

  ''' <summary>
  ''' Yacht Market Status 
  ''' Found on Yacht Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaYachtMarketStatus() As String
    Get
      Return _searchCriteriaYachtMarketStatus
    End Get
    Set(ByVal value As String)
      _searchCriteriaYachtMarketStatus = value
    End Set
  End Property

  ''' <summary>
  ''' Yacht Lifecycle
  ''' Found on Yacht Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaYachtLifecycle() As String
    Get
      Return _searchCriteriaYachtLifecycle
    End Get
    Set(ByVal value As String)
      _searchCriteriaYachtLifecycle = value
    End Set
  End Property

  ''' <summary>
  ''' Yacht Transaction Date Operator
  ''' Found on Yacht History Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaYachtTransactionDateOperator() As String
    Get
      Return _searchCriteriaYachtTransactionDateOperator
    End Get
    Set(ByVal value As String)
      _searchCriteriaYachtTransactionDateOperator = value
    End Set
  End Property
  ''' <summary>
  ''' Yacht Transaction Date 
  ''' Found on Yacht History Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaYachtTransactionDate() As String
    Get
      Return _searchCriteriaYachtTransactionDate
    End Get
    Set(ByVal value As String)
      _searchCriteriaYachtTransactionDate = value
    End Set
  End Property

  ''' <summary>
  ''' Yacht Transaction Type 
  ''' Found on Yacht History Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaYachtTransactionType() As String
    Get
      Return _searchCriteriaYachtTransactionType
    End Get
    Set(ByVal value As String)
      _searchCriteriaYachtTransactionType = value
    End Set
  End Property

  ''' <summary>
  ''' Yacht Category
  ''' Found on Yacht Event Listing Page. 
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaYachtCategory() As String
    Get
      Return _searchCriteriaYachtCategory
    End Get
    Set(ByVal value As String)
      _searchCriteriaYachtCategory = value
    End Set
  End Property

  ''' <summary>
  ''' Yacht Type
  ''' Found on Yacht Event Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaYachtType() As String
    Get
      Return _searchCriteriaYachtType
    End Get
    Set(ByVal value As String)
      _searchCriteriaYachtType = value
    End Set
  End Property

  Public Property SearchCriteriaYachtSaleCharterRestrictions() As String
    Get
      Return _searchCriteriaYachtSaleCharterRestrictions
    End Get
    Set(ByVal value As String)
      _searchCriteriaYachtSaleCharterRestrictions = value
    End Set
  End Property

#End Region

#Region "Booleans"
  Public Property SearchViewCriteriaClearCompany() As Boolean
    Get
      Return _searchViewCriteriaClearCompany
    End Get
    Set(ByVal value As Boolean)
      _searchViewCriteriaClearCompany = value
    End Set
  End Property
  Public Property SearchViewCriteriaClearAirport() As Boolean
    Get
      Return _searchViewCriteriaClearAirport
    End Get
    Set(ByVal value As Boolean)
      _searchViewCriteriaClearAirport = value
    End Set
  End Property
  Public Property SearchViewCriteriaDefaultFolder() As Boolean
    Get
      Return _searchViewCriteriaDefaultFolder
    End Get
    Set(ByVal value As Boolean)
      _searchViewCriteriaDefaultFolder = value
    End Set
  End Property
  Public Property SearchViewCriteriaAirportExcludeFolder() As Boolean
    Get
      Return _searchViewCriteriaAirportExcludeFolder
    End Get
    Set(ByVal value As Boolean)
      _searchViewCriteriaAirportExcludeFolder = value
    End Set
  End Property

  Public Property SearchViewCriteriaOperatorExcludeFolder() As Boolean
    Get
      Return _searchViewCriteriaOperatorExcludeFolder
    End Get
    Set(ByVal value As Boolean)
      _searchViewCriteriaOperatorExcludeFolder = value
    End Set
  End Property
  Public Property SearchViewCriteriaAircraftExcludeFolder() As Boolean
    Get
      Return _searchViewCriteriaAircraftExcludeFolder
    End Get
    Set(ByVal value As Boolean)
      _searchViewCriteriaAircraftExcludeFolder = value
    End Set
  End Property

  Public Property SearchCriteriaAdminFlag() As Boolean
    Get
      Return _searchCriteriaAdminFlag
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaAdminFlag = value
    End Set
  End Property

  Public Property SearchCriteriaCompanyDisplayInactiveCompanies() As Boolean
    Get
      Return _searchCriteriaCompanyDisplayInactiveCompanies
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaCompanyDisplayInactiveCompanies = value
    End Set
  End Property

  Public Property SearchCriteriaCompanyDisplayHiddenCompanies() As Boolean
    Get
      Return _searchCriteriaCompanyDisplayHiddenCompanies
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaCompanyDisplayHiddenCompanies = value
    End Set
  End Property

  Public Property SearchCriteriaCompanyDisplayInactiveContacts() As Boolean
    Get
      Return _searchCriteriaCompanyDisplayInactiveContacts
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaCompanyDisplayInactiveContacts = value
    End Set
  End Property

  Public Property SearchCriteriaCompanyDisplayHiddenContacts() As Boolean
    Get
      Return _searchCriteriaCompanyDisplayHiddenContacts
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaCompanyDisplayHiddenContacts = value
    End Set
  End Property


  ''' <summary>
  ''' Registration Exact Match Search Field.  
  ''' Found on: Aircraft, History and Events Listing Pages.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaRegExactMatch() As Boolean
    Get
      Return _searchCriteriaRegExactMatch
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaRegExactMatch = value
    End Set
  End Property

  '
  ''' <summary>
  ''' Checkbox to exclude internal transactions  
  ''' Found on: History Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaExcludeInternalTransactions() As Boolean
    Get
      Return _searchCriteriaExcludeInternalTransactions
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaExcludeInternalTransactions = value
    End Set
  End Property

  ''' <summary>
  ''' Do Not Search Alternate Serial Number Search Field.
  ''' Found on: Aircraft, History and Events Listing Pages.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaSerDoNotSearchAlt() As Boolean
    Get
      Return _searchCriteriaSerDoNotSearchAlt
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaSerDoNotSearchAlt = value
    End Set
  End Property
  ''' <summary>
  ''' Do Not Search Previous Registration Number Search Field.
  ''' Found on: Aircraft, History and Events Listing Pages.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaDoNotSearchPrevRegNo() As Boolean
    Get
      Return _searchCriteriaDoNotSearchPrevRegNo
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaDoNotSearchPrevRegNo = value
    End Set
  End Property
  ''' <summary>
  ''' Helcopter Flag Search Field.
  ''' Found on: Aircraft, History, Company, Performance Specs, Operating Costs, Events, Market Summary and Wanted Listing Pages. 
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaHelicopterFlag() As Boolean
    Get
      Return _searchCriteriaHelicopterFlag
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaHelicopterFlag = value
    End Set
  End Property
  ''' <summary>
  ''' Business Flag Search Field.
  ''' Found on: Aircraft, History, Company, Performance Specs, Operating Costs, Events, Market Summary and Wanted Listing Pages.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaBusinessFlag() As Boolean
    Get
      Return _searchCriteriaBusinessFlag
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaBusinessFlag = value
    End Set
  End Property
  ''' <summary>
  ''' Commercial Flag Search Field.
  ''' Found on: Aircraft, History, Company, Performance Specs, Operating Costs, Events, Market Summary and Wanted Listing Pages.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaCommercialFlag() As Boolean
    Get
      Return _searchCriteriaCommercialFlag
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaCommercialFlag = value
    End Set
  End Property

  Public Property SearchCriteriaYachtFlag() As Boolean
    Get
      Return _searchCriteriaYachtFlag
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaYachtFlag = value
    End Set
  End Property


  Public Property SearchCriteriaYachtHasFilterFlag() As Boolean
    Get
      Return _searchCriteriaYachtHasFilterFlag
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaYachtHasFilterFlag = value
    End Set
  End Property

  ''' <summary>
  ''' History Retail Activity Flag. 
  ''' Found on History Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaRetailActivity() As Boolean
    Get
      Return _searchCriteriaRetailActivity
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaRetailActivity = value
    End Set
  End Property
  ''' <summary>
  ''' History Sales Of New Aircraft Only Search Field.
  ''' Found On: History Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaSalesOfNewAircraftOnly() As Boolean
    Get
      Return _searchCriteriaSalesOfNewAircraftOnly
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaSalesOfNewAircraftOnly = value
    End Set
  End Property

  ''' <summary>
  ''' History Sales Of Used Aircraft Only Search Field.
  ''' Found On: History Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaSalesOfUsedAircraftOnly() As Boolean
    Get
      Return _searchCriteriaSalesOfUsedAircraftOnly
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaSalesOfUsedAircraftOnly = value
    End Set
  End Property
  ''' <summary>
  ''' Company Not In Selected Aircraft Relationship Search Field.
  ''' Found on Company Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaCompanyNotInSelectedRelationship() As Boolean
    Get
      Return _searchCriteriaCompanyNotInSelectedRelationship
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaCompanyNotInSelectedRelationship = value
    End Set
  End Property

  ''' <summary>
  ''' Company Display Contact Info Search Field.
  ''' Found on Company Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaCompanyDisplayContactInfo() As Boolean
    Get
      Return _searchCriteriaCompanyDisplayContactInfo
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaCompanyDisplayContactInfo = value
    End Set
  End Property
  ''' <summary>
  ''' Company Only Aircraft Sales Professionals Search Field.
  ''' Found on Company Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaCompanyOnlyAircraftSalesProfessionals() As Boolean
    Get
      Return _searchCriteriaCompanyOnlyAircraftSalesProfessionals
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaCompanyOnlyAircraftSalesProfessionals = value
    End Set
  End Property

  ''' <summary>
  ''' Yacht For Sale Flag.
  ''' Found on Yacht Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaYachtForSale() As Boolean
    Get
      Return _searchCriteriaYachtForSale
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaYachtForSale = value
    End Set
  End Property

  ''' <summary>
  ''' Yacht For Lease.
  ''' Found on Yacht Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaYachtForLease() As Boolean
    Get
      Return _searchCriteriaYachtForLease
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaYachtForLease = value
    End Set
  End Property

  ''' <summary>
  ''' Yacht For Charter. Found on Yacht Listing Page.
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaYachtForCharter() As Boolean
    Get
      Return _searchCriteriaYachtForCharter
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaYachtForCharter = value
    End Set
  End Property

  ''' <summary>
  ''' Yacht Previous Name Found on yacht listing page
  ''' </summary>
  ''' <value></value>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Property SearchCriteriaYachtPreviousName() As Boolean
    Get
      Return _searchCriteriaYachtPreviousName
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaYachtPreviousName = value
    End Set
  End Property


  Public Property SearchCriteriaHasCompanyLocationInfo() As Boolean
    Get
      Return _searchCriteriaHasCompanyLocationInfo
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaHasCompanyLocationInfo = value
    End Set
  End Property

  Public Property SearchCriteriaUseContinent() As Boolean
    Get
      Return _searchCriteriaUseContinent
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaUseContinent = value
    End Set
  End Property

  Public Property SearchCriteriaUseRegion() As Boolean
    Get
      Return _searchCriteriaUseRegion
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaUseRegion = value
    End Set
  End Property

#End Region

#Region "subscriber_search_items"

  Public Property SearchCriteriaSub_user_id() As String
    Get
      Return _searchCriteriaSub_user_id
    End Get
    Set(ByVal value As String)
      _searchCriteriaSub_user_id = value
    End Set
  End Property

  Public Property SearchCriteriaSub_login() As String
    Get
      Return _searchCriteriaSub_login
    End Get
    Set(ByVal value As String)
      _searchCriteriaSub_login = value
    End Set
  End Property

  Public Property SearchCriteriaSub_id() As Long
    Get
      Return _searchCriteriaSub_id
    End Get
    Set(ByVal value As Long)
      _searchCriteriaSub_id = value
    End Set
  End Property

  Public Property SearchCriteriaSequence_number() As Long
    Get
      Return _searchCriteriaSequence_number
    End Get
    Set(ByVal value As Long)
      _searchCriteriaSequence_number = value
    End Set
  End Property

  Public Property SearchCriteriaService_code() As String
    Get
      Return _searchCriteriaService_code
    End Get
    Set(ByVal value As String)
      _searchCriteriaService_code = value
    End Set
  End Property

  Public Property SearchCriteriaLast_login_date() As String
    Get
      Return _searchCriteriaLast_login_date
    End Get
    Set(ByVal value As String)
      _searchCriteriaLast_login_date = value
    End Set
  End Property

  Public Property SearchCriteriaStart_date() As String
    Get
      Return _searchCriteriaStart_date
    End Get
    Set(ByVal value As String)
      _searchCriteriaStart_date = value
    End Set
  End Property

  Public Property SearchCriteriaEnd_date() As String  '
    Get
      Return _searchCriteriaEnd_date
    End Get
    Set(ByVal value As String)
      _searchCriteriaEnd_date = value
    End Set
  End Property


  Public Property SearchCriteriaLastHost() As String  '_searchCriteriaServicesString
    Get
      Return _searchCriteriaLastHost
    End Get
    Set(ByVal value As String)
      _searchCriteriaLastHost = value
    End Set
  End Property

  Public Property SearchCriteriaServices() As String  '
    Get
      Return _searchCriteriaServicesString
    End Get
    Set(ByVal value As String)
      _searchCriteriaServicesString = value
    End Set
  End Property

  Public Property SearchCriteriaAerodexFlag() As Boolean
    Get
      Return _searchCriteriaAerodexFlag
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaAerodexFlag = value
    End Set
  End Property

  Public Property SearchCriteriaDemoFlag() As Boolean
    Get
      Return _searchCriteriaDemoFlag
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaDemoFlag = value
    End Set
  End Property

  Public Property SearchCriteriaMarketingFlag() As Boolean
    Get
      Return _searchCriteriaMarketingFlag
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaMarketingFlag = value
    End Set
  End Property

  Public Property SearchCriteriaCRMFlag() As Boolean
    Get
      Return _searchCriteriaCRMFlag
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaCRMFlag = value
    End Set
  End Property

  Public Property SearchCriteriaSPIFlag() As Boolean
    Get
      Return _searchCriteriaSPIFlag
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaSPIFlag = value
    End Set
  End Property

  Public Property SearchCriteriaMobileFlag() As Boolean
    Get
      Return _searchCriteriaMobileFlag
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaMobileFlag = value
    End Set
  End Property

  Public Property SearchCriteriaLocalNotesFlag() As Boolean
    Get
      Return _searchCriteriaLocalNotesFlag
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaLocalNotesFlag = value
    End Set
  End Property

  Public Property SearchCriteriaCloudNotesFlag() As Boolean
    Get
      Return _searchCriteriaCloudNotesFlag
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaCloudNotesFlag = value
    End Set
  End Property

  Public Property SearchCriteriaNotesPlusFlag() As Boolean
    Get
      Return _searchCriteriaNotesPlusFlag
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaNotesPlusFlag = value
    End Set
  End Property

  Public Property SearchCriteriaActiveFlag() As Boolean
    Get
      Return _searchCriteriaActiveFlag
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaActiveFlag = value
    End Set
  End Property

  Public Property SearchCriteriaExpiredFlag() As Boolean
    Get
      Return _searchCriteriaExpiredFlag
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaExpiredFlag = value
    End Set
  End Property

  Public Property SearchCriteriaParentSub() As Boolean
    Get
      Return _searchCriteriaParentSub
    End Get
    Set(ByVal value As Boolean)
      _searchCriteriaParentSub = value
    End Set
  End Property

#End Region

#Region "query and display strings"
  Public Property SearchCriteriaDisplayString() As String
    Get
      Return _searchCriteriaDisplayString
    End Get
    Set(ByVal value As String)
      _searchCriteriaDisplayString = value
    End Set
  End Property
  Public Property SearchCriteriaQueryString() As String
    Get
      Return _searchCriteriaQueryString
    End Get
    Set(ByVal value As String)
      _searchCriteriaQueryString = value
    End Set
  End Property
  Public Property SearchCriteriaCompanyNameQueryString() As String
    Get
      Return _searchCriteriaCompanyNameQueryString
    End Get
    Set(ByVal value As String)
      _searchCriteriaCompanyNameQueryString = value
    End Set
  End Property
#End Region

#Region "Display Class"
  Public Function DisplaySearchClass() As String

    DisplaySearchClass = "Session.Item(""searchCriteria"").SearchCriteriaStatusCode: " & _searchCriteriaStatusCode & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaDetailError: " & _searchCriteriaDetailError & "<br />"

    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaViewAC: " & _searchCriteriaViewAC.ToString & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaViewModel: " & _searchCriteriaViewModel.ToString & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaViewVariantString: " & _searchCriteriaViewVariantString & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaModel: " & _searchCriteriaModel & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaMake: " & _searchCriteriaMake & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaType: " & _searchCriteriaType & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaWeightClass: " & _searchCriteriaWeightClass & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaManufacturerName: " & _searchCriteriaManufacturerName & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaAcSize: " & _searchCriteriaAcSize & "<br />"

    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaRegExactMatch: " & _searchCriteriaRegExactMatch & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaSerDoNotSearchAlt: " & _searchCriteriaSerDoNotSearchAlt & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaDoNotSearchPrevRegNo: " & _searchCriteriaDoNotSearchPrevRegNo & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaBusinessFlag: " & _searchCriteriaBusinessFlag & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaCommercialFlag: " & _searchCriteriaCommercialFlag & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaHelicopterFlag: " & _searchCriteriaHelicopterFlag & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaViewFeatureString: " & _searchCriteriaViewFeatureString & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").searchCriteriaYachtHasFilterFlag: " & _searchCriteriaYachtHasFilterFlag & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaSerNoStart: " & _searchCriteriaSerNoStart & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaSerNoEnd: " & _searchCriteriaSerNoEnd & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaRegNo: " & _searchCriteriaRegNo & "<br />"

    DisplaySearchClass += "<br />AC Listing Page Only:<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaLifeCycle: " & _searchCriteriaLifeCycle & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaPreviouslyOwned: " & _searchCriteriaPreviouslyOwned & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaLeaseStatus: " & _searchCriteriaLeaseStatus & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaOwnership: " & _searchCriteriaOwnership & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaMarketStatus: " & _searchCriteriaMarketStatus & "<br />"

    DisplaySearchClass += "<br />History Listing Page Only:<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaHistoryType: " & _searchCriteriaHistoryType & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaHistoryFromOperator: " & _searchCriteriaHistoryFromOperator & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaHistoryFromAnswer: " & _searchCriteriaHistoryFromAnswer & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaHistoryToOperator: " & _searchCriteriaHistoryToOperator & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaHistoryToAnswer: " & _searchCriteriaHistoryToAnswer & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaHistoryDateOperator: " & _searchCriteriaHistoryDateOperator & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaHistoryDate: " & _searchCriteriaHistoryDate & "<br />"

    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaRetailActivity: " & _searchCriteriaRetailActivity & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaSalesOfNewAircraftOnly: " & _searchCriteriaSalesOfNewAircraftOnly & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaSalesOfUsedAircraftOnly: " & _searchCriteriaSalesOfUsedAircraftOnly & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaExcludeInternalTransactions: " & _searchCriteriaExcludeInternalTransactions & "<br />"

    DisplaySearchClass += "<br />Event Listing Page Only:<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaEventSearchType: " & _searchCriteriaEventSearchType & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaEventCategory: " & _searchCriteriaEventCategory & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaEventType: " & _searchCriteriaEventType & "<br />"

    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaEventMonths: " & _searchCriteriaEventMonths & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaEventDays: " & _searchCriteriaEventDays & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaEventHours: " & _searchCriteriaEventHours & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaEventMinutes: " & _searchCriteriaEventMinutes & "<br />"

    DisplaySearchClass += "<br />Company Listing Page or Subscriber Listing Page:<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaCompanyID: " & _searchCriteriaCompanyID.ToString & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaCompanyName: " & _searchCriteriaCompanyName & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaCompanyAgencyType: " & _searchCriteriaCompanyAgencyType & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaCompanyRelationshipsToAC: " & _searchCriteriaCompanyRelationshipsToAC & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaCompanyAddress: " & _searchCriteriaCompanyAddress & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaCompanyCity: " & _searchCriteriaCompanyCity & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaCompanyEmail: " & _searchCriteriaCompanyEmail & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaCompanyPhone: " & _searchCriteriaCompanyPhone & "<br />"

    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaCompanyPostalCode: " & _searchCriteriaCompanyPostalCode & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaCompanyBusinessType: " & _searchCriteriaCompanyBusinessType & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaCompanyContinent: " & _searchCriteriaCompanyContinent & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaCompanyContinentOrRegion: " & _searchCriteriaCompanyContinentOrRegion & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaCompanyRegion: " & _searchCriteriaCompanyRegion & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaCompanyStateProvince: " & _searchCriteriaCompanyStateProvince & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaCompanyTimezone: " & _searchCriteriaCompanyTimezone & "<br />"
    '
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaCompanyYachtFleet: " & _searchCriteriaCompanyYachtFleet & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaCompanyFleetOperator: " & _searchCriteriaCompanyFleetOperator & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaCompanyFleetAnswer: " & _searchCriteriaCompanyFleetAnswer & "<br />"

    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaCompanyContactID: " & _searchCriteriaCompanyContactID.ToString & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaCompanyContactFirstName: " & _searchCriteriaCompanyContactFirstName & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaCompanyContactLastName: " & _searchCriteriaCompanyContactLastName & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaCompanyContactEmail: " & _searchCriteriaCompanyContactEmail & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaCompanyContactPhone: " & _searchCriteriaCompanyContactPhone & "<br />"

    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaCompanyContactTitle: " & _searchCriteriaCompanyContactTitle & "<br />"

    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaCompanyNotInSelectedRelationship: " & _searchCriteriaCompanyNotInSelectedRelationship & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaCompanyDisplayContactInfo: " & _searchCriteriaCompanyDisplayContactInfo & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaCompanyNotInSelectedRelationship: " & _searchCriteriaCompanyNotInSelectedRelationship & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaCompanyOnlyAircraftSalesProfessionals: " & _searchCriteriaCompanyOnlyAircraftSalesProfessionals & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaCompanyFleetValue: " & _searchCriteriaCompanyFleetValue & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaCompanyDisplayInactiveCompanies: " & _searchCriteriaCompanyDisplayInactiveCompanies & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaCompanyDisplayHiddenCompanies: " & _searchCriteriaCompanyDisplayHiddenCompanies & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaCompanyDisplayInactiveContacts: " & _searchCriteriaCompanyDisplayInactiveContacts & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaCompanyDisplayHiddenContacts: " & _searchCriteriaCompanyDisplayHiddenContacts & "<br />"


    DisplaySearchClass += "<br />Operating Costs Page Only:<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaOpCostsFuelBurnOperator: " & _searchCriteriaOpCostsFuelBurnOperator & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaOpCostsFuelBurn: " & _searchCriteriaOpCostsFuelBurn & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaOpCostsTotalDirectCostsOperator: " & _searchCriteriaOpCostsTotalDirectCostsOperator & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaOpCostsTotalDirectCosts: " & _searchCriteriaOpCostsTotalDirectCosts & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaOpCostsCurrency: " & _searchCriteriaOpCostsCurrency & "<br />"

    DisplaySearchClass += "<br />Both Performance and Operating Costs:<br />" '
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaDisplayUnits: " & _searchCriteriaDisplayUnits & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaDisplayMiles: " & _searchCriteriaDisplayMiles & "<br />"

    DisplaySearchClass += "<br />Performance Specs Page Only:<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaPerfSpecsSLISAOperator: " & _searchCriteriaPerfSpecsSLISAOperator & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaPerfSpecsSLISA: " & _searchCriteriaPerfSpecsSLISA & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaPerfSpecsMaxRangeNBAAOrTanksFullOperator: " & _searchCriteriaPerfSpecsMaxRangeNBAAOrTanksFullOperator & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaPerfSpecsMaxRangeNBAAOrTanksFull: " & _searchCriteriaPerfSpecsMaxRangeNBAAOrTanksFull & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaPerfSpecsFuselageLengthOperator: " & _searchCriteriaPerfSpecsFuselageLengthOperator & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaPerfSpecsFuselageLength: " & _searchCriteriaPerfSpecsFuselageLength & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaPerfSpecsFuselageHeightOperator: " & _searchCriteriaPerfSpecsFuselageHeightOperator & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaPerfSpecsFuselageHeight: " & _searchCriteriaPerfSpecsFuselageHeight & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaPerfSpecsWingSpanOrWidthOperator: " & _searchCriteriaPerfSpecsWingSpanOrWidthOperator & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaPerfSpecsWingSpanOrWidth: " & _searchCriteriaPerfSpecsWingSpanOrWidth & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaPerfSpecsCrewOperator: " & _searchCriteriaPerfSpecsCrewOperator & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaPerfSpecsCrew: " & _searchCriteriaPerfSpecsCrew & "<br />"

    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaPerfSpecsPassengersOperator: " & _searchCriteriaPerfSpecsPassengersOperator & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaPerfSpecsPassengers: " & _searchCriteriaPerfSpecsPassengers & "<br />"


    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaPerfSpecsMaxTakeoffOperator: " & _searchCriteriaPerfSpecsMaxTakeoffOperator & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaPerfSpecsMaxTakeoff: " & _searchCriteriaPerfSpecsMaxTakeoff & "<br />"

    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaPerfSpecsNormalCruiseOperator: " & _searchCriteriaPerfSpecsNormalCruiseOperator & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaPerfSpecsNormalCruise: " & _searchCriteriaPerfSpecsNormalCruise & "<br />"

    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaPerfSpecsFuelCapacityOperator: " & _searchCriteriaPerfSpecsFuelCapacityOperator & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaPerfSpecsFuelCapacity: " & _searchCriteriaPerfSpecsFuelCapacity & "<br />"

    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaDisplayString: " & _searchCriteriaDisplayString & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaQueryString: " & _searchCriteriaQueryString & "<br />"

    DisplaySearchClass += "<br />Yacht Page Only:<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaYachtName:" & _searchCriteriaYachtName & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaYachtPreviousName:" & _searchCriteriaYachtPreviousName.ToString & "<br />"

    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaYachtFlagOption:" & _searchCriteriaYachtFlagOption & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaYachtLengthOperator:" & _searchCriteriaYachtLengthOperator & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaYachtLengthValue:" & _searchCriteriaYachtLengthValue & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaYachtLengthStandard:" & _searchCriteriaYachtLengthStandard & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaYachtClass:" & _searchCriteriaYachtClass & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaYachtCallSign:" & _searchCriteriaYachtCallSign & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaYachtYearDeliveredOperator:" & _searchCriteriaYachtYearDeliveredOperator & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaYachtYearDelivered:" & _searchCriteriaYachtYearDelivered & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaYachtYearManufacturedOperator:" & _searchCriteriaYachtYearManufacturedOperator & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaYachtYearManufactured :" & _searchCriteriaYachtYearManufactured & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaYachtAskingPriceOperator:" & _searchCriteriaYachtAskingPriceOperator & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaYachtAskingPrice:" & _searchCriteriaYachtAskingPrice & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaYachtAskingPriceCurrency:" & _searchCriteriaYachtAskingPriceCurrency & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaYachtDOMOperator:" & _searchCriteriaYachtDOMOperator & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaYachtDOM:" & _searchCriteriaYachtDOM & "<br />"

    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaYachtMarketStatus:" & _searchCriteriaYachtMarketStatus & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaYachtLifecycle:" & _searchCriteriaYachtLifecycle & "<br />"


    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaYachtForSale:" & _searchCriteriaYachtForSale.ToString & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaYachtForLease:" & _searchCriteriaYachtForLease.ToString & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaYachtForCharter:" & _searchCriteriaYachtForCharter.ToString & "<br />"

    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaYachtTransactionDate:" & _searchCriteriaYachtTransactionDate.ToString & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaYachtTransactionDateOperator:" & _searchCriteriaYachtTransactionDateOperator.ToString & "<br />"

    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaYachtCategory:" & _searchCriteriaYachtCategory.ToString & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaYachtType:" & _searchCriteriaYachtType.ToString & "<br />"

    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaYachtEventMonths:" & _searchCriteriaYachtEventMonths.ToString & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaYachtEventDays:" & _searchCriteriaYachtEventDays.ToString & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaYachtEventHours:" & _searchCriteriaYachtEventHours.ToString & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaYachtEventMinutes:" & _searchCriteriaYachtEventMinutes.ToString & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchCriteriaYachtSaleCharterRestrictions:" & _searchCriteriaYachtSaleCharterRestrictions.ToString & "<br />"


    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchViewCriteriaOperatorDropdown:" & _searchViewCriteriaOperatorDropdown.ToString & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchViewCriteriaOperatorSelected:" & _searchViewCriteriaOperatorSelected.ToString & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchViewCriteriaOperatorDropdown2:" & _searchViewCriteriaOperatorDropdown2.ToString & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchViewCriteriaOperatorFolderName:" & _searchViewCriteriaOperatorFolderName.ToString & "<br />"


    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchViewCriteriaAirportDropdown:" & _searchViewCriteriaAirportDropdown.ToString & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchViewCriteriaAirportSelected:" & _searchViewCriteriaAirportSelected.ToString & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchViewCriteriaAirportDropdown2:" & _searchViewCriteriaAirportDropdown2.ToString & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchViewCriteriaAirportFolderName:" & _searchViewCriteriaAirportFolderName.ToString & "<br />"

    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchViewCriteriaAircraftDropdown:" & _searchViewCriteriaAircraftDropdown.ToString & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchViewCriteriaAircraftDropdown2:" & _searchViewCriteriaAircraftDropdown2.ToString & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchViewCriteriaAircraftFolderName:" & _searchViewCriteriaAircraftFolderName.ToString & "<br />"

    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchViewCriteriaAirportExcludeFolder:" & _searchViewCriteriaAirportExcludeFolder.ToString & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchViewCriteriaOperatorExcludeFolder:" & _searchViewCriteriaOperatorExcludeFolder.ToString & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchViewCriteriaAircraftExcludeFolder:" & _searchViewCriteriaAircraftExcludeFolder.ToString & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchViewCriteriaStartDate:" & _searchViewCriteriaStartDate.ToString & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchViewCriteriaEndDate:" & _searchViewCriteriaEndDate.ToString & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchViewCriteriaAirportCodes:" & _searchViewCriteriaAirportCodes.ToString & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchViewCriteriaRegList:" & _searchViewCriteriaRegList.ToString & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchViewCriteriaDefaultFolder:" & _searchViewCriteriaDefaultFolder.ToString & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchViewCriteriaBasedOn:" & _searchViewCriteriaBasedOn.ToString & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchViewCriteriaClearAirport:" & _searchViewCriteriaClearAirport.ToString & "<br />"
    DisplaySearchClass += "Session.Item(""searchCriteria"").SearchViewCriteriaClearCompany:" & _searchViewCriteriaClearCompany.ToString & "<br />"

  End Function
#End Region

End Class
