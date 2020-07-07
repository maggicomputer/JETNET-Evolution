Imports Microsoft.VisualBasic
Imports System.ComponentModel

' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/opCostsClass.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:49a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: opCostsClass.vb $
'
' ********************************************************************************

<System.Serializable(), FlagsAttribute()> Public Enum eOpCostsTypes As Integer

  NULL = 0

  'bySerialNumber = 1 ' browse by serial number
  'byAircraftID = 2 ' browse by aircraft ID
  'byAircraftJournalID = 4 ' browse by aircraft ID and journal ID

End Enum

<System.Serializable()> Public Class opCostsClass

  Private _evoOpCostsStatusCode As eObjStatusCode
  Private _evoOpCostsDetailError As eObjDetailErrorCode

  Private _evoMakeModelName As String
  Private _evoCurrencyName As String
  Private _evoCurrencySymbol As String
  Private _evoCurrencyDate As String

  Private _modelID As Long

  Private _fuelGalCost As Double
  Private _fuelAddCost As Double
  Private _fuelBurnRate As Double
  Private _calcFuelTotalCost As Double

  Private _maintLaborCost As Double
  Private _maintPartsCost As Double
  Private _maintLaborCostManHour As Double
  Private _maintPartsCostManHour As Double

  Private _calcMaintTotalCost As Double

  Private _maintEngineCost As Double
  Private _maintThrustCost As Double

  Private _miscLandParkCost As Double
  Private _miscCrewCost As Double
  Private _miscSupplyCost As Double
  Private _calcMiscFlightTotalCost As Double

  Private _calcTotalDirCostHour As Double
  Private _avgBlockSpeed As Double
  Private _calcTotalCostPerMile As Double

  Private _captSalaryCost As Double
  Private _coPilotSalaryCost As Double
  Private _benefitsCost As Double
  Private _calcCrewTotalCost As Double

  Private _hangarCost As Double

  Private _insuranceHullCost As Double
  Private _insuranceLiabilityCost As Double
  Private _calcInsuranceTotalCost As Double

  Private _miscTrainCost As Double
  Private _miscModernCost As Double
  Private _miscNavCost As Double
  Private _calcMiscTotalCost As Double

  Private _depreciationCost As Double
  Private _calcTotalFixedCosts As Double

  Private _number_of_seats As Integer
  Private _annualMiles As Integer
  Private _calcAnnualHrs As Integer

  Private _calcTotalDirCostYR As Double
  Private _calcTotalFixedDirect As Double

  Private _calcCostPerHourFixDir As Double
  Private _calcCostPerMileFixDir As Double
  Private _calcCostPerSeatFixDir As Double

  Private _calcNoDepTotalCost As Double
  Private _variableTotalCost As Double
  Private _calcCostPerHourNoDep As Double
  Private _calcCostPerMileNoDep As Double
  Private _calcCostPerSeatNoDep As Double

  Sub New()

    _evoOpCostsStatusCode = eObjStatusCode.NULL
    _evoOpCostsDetailError = eObjDetailErrorCode.NULL

    _evoMakeModelName = ""
    _evoCurrencyName = ""
    _evoCurrencySymbol = ""
    _evoCurrencyDate = ""

    _modelID = 0

    _fuelGalCost = 0.0
    _fuelAddCost = 0.0
    _fuelBurnRate = 0.0
    _calcFuelTotalCost = 0.0

    _maintLaborCost = 0.0
    _maintPartsCost = 0.0
    _maintLaborCostManHour = 0.0
    _maintPartsCostManHour = 0.0
    _calcMaintTotalCost = 0.0

    _maintEngineCost = 0.0
    _maintThrustCost = 0.0

    _miscLandParkCost = 0.0
    _miscCrewCost = 0.0
    _miscSupplyCost = 0.0
    _calcMiscFlightTotalCost = 0.0

    _calcTotalDirCostHour = 0.0
    _avgBlockSpeed = 0.0
    _calcTotalCostPerMile = 0.0

    _captSalaryCost = 0.0
    _coPilotSalaryCost = 0.0
    _benefitsCost = 0.0
    _calcCrewTotalCost = 0.0

    _hangarCost = 0.0

    _insuranceHullCost = 0.0
    _insuranceLiabilityCost = 0.0
    _calcInsuranceTotalCost = 0.0

    _miscTrainCost = 0.0
    _miscModernCost = 0.0
    _miscNavCost = 0.0
    _calcMiscTotalCost = 0.0

    _depreciationCost = 0.0

    _calcTotalFixedCosts = 0.0

    _number_of_seats = 0
    _annualMiles = 0
    _calcAnnualHrs = 0

    _calcTotalDirCostYR = 0.0
    _calcTotalFixedDirect = 0.0
    _calcCostPerHourFixDir = 0.0
    _calcCostPerMileFixDir = 0.0
    _calcCostPerSeatFixDir = 0.0

    _calcNoDepTotalCost = 0.0
    _variableTotalCost = 0.0
    _calcCostPerHourNoDep = 0.0
    _calcCostPerMileNoDep = 0.0
    _calcCostPerSeatNoDep = 0.0

  End Sub

  Public Property evoOpCostsStatusCode() As eObjStatusCode
    Get
      Return _evoOpCostsStatusCode
    End Get
    Set(ByVal value As eObjStatusCode)
      _evoOpCostsStatusCode = value
    End Set
  End Property

  Public Property evoOpCostsDetailError() As eObjDetailErrorCode
    Get
      Return _evoOpCostsDetailError
    End Get
    Set(ByVal value As eObjDetailErrorCode)
      _evoOpCostsDetailError = value
    End Set
  End Property

  Public Property evoModelID() As Long
    Get
      Return _modelID
    End Get
    Set(ByVal value As Long)
      _modelID = value
    End Set
  End Property

  Public Property evoMakeModelName() As String
    Get
      Return _evoMakeModelName
    End Get
    Set(ByVal value As String)
      _evoMakeModelName = value
    End Set
  End Property

  Public Property fuelGalCost() As Double
    Get
      Return _fuelGalCost
    End Get
    Set(ByVal value As Double)
      _fuelGalCost = value
    End Set
  End Property

  Public Property fuelAddCost() As Double
    Get
      Return _fuelAddCost
    End Get
    Set(ByVal value As Double)
      _fuelAddCost = value
    End Set
  End Property

  Public Property fuelBurnRate() As Double
    Get
      Return _fuelBurnRate
    End Get
    Set(ByVal value As Double)
      _fuelBurnRate = value
    End Set
  End Property

  Public Property calcFuelTotalCost() As Double
    Get
      Return _calcFuelTotalCost
    End Get
    Set(ByVal value As Double)
      _calcFuelTotalCost = value
    End Set
  End Property

  Public Property maintLaborCost() As Double
    Get
      Return _maintLaborCost
    End Get
    Set(ByVal value As Double)
      _maintLaborCost = value
    End Set
  End Property

  Public Property maintPartsCost() As Double
    Get
      Return _maintPartsCost
    End Get
    Set(ByVal value As Double)
      _maintPartsCost = value
    End Set
  End Property

  Public Property maintLaborCostManHour() As Double
    Get
      Return _maintLaborCostManHour
    End Get
    Set(ByVal value As Double)
      _maintLaborCostManHour = value
    End Set
  End Property

  Public Property maintPartsCostManHour() As Double
    Get
      Return _maintPartsCostManHour
    End Get
    Set(ByVal value As Double)
      _maintPartsCostManHour = value
    End Set
  End Property
  Public Property calcMaintTotalCost() As Double
    Get
      Return _calcMaintTotalCost
    End Get
    Set(ByVal value As Double)
      _calcMaintTotalCost = value
    End Set
  End Property

  Public Property maintEngineCost() As Double
    Get
      Return _maintEngineCost
    End Get
    Set(ByVal value As Double)
      _maintEngineCost = value
    End Set
  End Property

  Public Property maintThrustCost() As Double
    Get
      Return _maintThrustCost
    End Get
    Set(ByVal value As Double)
      _maintThrustCost = value
    End Set
  End Property

  Public Property miscLandParkCost() As Double
    Get
      Return _miscLandParkCost
    End Get
    Set(ByVal value As Double)
      _miscLandParkCost = value
    End Set
  End Property

  Public Property miscCrewCost() As Double
    Get
      Return _miscCrewCost
    End Get
    Set(ByVal value As Double)
      _miscCrewCost = value
    End Set
  End Property

  Public Property miscSupplyCost() As Double
    Get
      Return _miscSupplyCost
    End Get
    Set(ByVal value As Double)
      _miscSupplyCost = value
    End Set
  End Property

  Public Property calcMiscFlightTotalCost() As Double
    Get
      Return _calcMiscFlightTotalCost
    End Get
    Set(ByVal value As Double)
      _calcMiscFlightTotalCost = value
    End Set
  End Property

  Public Property calcTotalDirCostHour() As Double
    Get
      Return _calcTotalDirCostHour
    End Get
    Set(ByVal value As Double)
      _calcTotalDirCostHour = value
    End Set
  End Property

  Public Property avgBlockSpeed() As Double
    Get
      Return _avgBlockSpeed
    End Get
    Set(ByVal value As Double)
      _avgBlockSpeed = value
    End Set
  End Property

  Public Property calcTotalCostPerMile() As Double
    Get
      Return _calcTotalCostPerMile
    End Get
    Set(ByVal value As Double)
      _calcTotalCostPerMile = value
    End Set
  End Property

  Public Property captSalaryCost() As Double
    Get
      Return _captSalaryCost
    End Get
    Set(ByVal value As Double)
      _captSalaryCost = value
    End Set
  End Property

  Public Property coPilotSalaryCost() As Double
    Get
      Return _coPilotSalaryCost
    End Get
    Set(ByVal value As Double)
      _coPilotSalaryCost = value
    End Set
  End Property

  Public Property benefitsCost() As Double
    Get
      Return _benefitsCost
    End Get
    Set(ByVal value As Double)
      _benefitsCost = value
    End Set
  End Property

  Public Property calcCrewTotalCost() As Double
    Get
      Return _calcCrewTotalCost
    End Get
    Set(ByVal value As Double)
      _calcCrewTotalCost = value
    End Set
  End Property

  Public Property hangarCost() As Double
    Get
      Return _hangarCost
    End Get
    Set(ByVal value As Double)
      _hangarCost = value
    End Set
  End Property

  Public Property insuranceHullCost() As Double
    Get
      Return _insuranceHullCost
    End Get
    Set(ByVal value As Double)
      _insuranceHullCost = value
    End Set
  End Property

  Public Property insuranceLiabilityCost() As Double
    Get
      Return _insuranceLiabilityCost
    End Get
    Set(ByVal value As Double)
      _insuranceLiabilityCost = value
    End Set
  End Property

  Public Property calcInsuranceTotalCost() As Double
    Get
      Return _calcInsuranceTotalCost
    End Get
    Set(ByVal value As Double)
      _calcInsuranceTotalCost = value
    End Set
  End Property

  Public Property miscTrainCost() As Double
    Get
      Return _miscTrainCost
    End Get
    Set(ByVal value As Double)
      _miscTrainCost = value
    End Set
  End Property

  Public Property miscModernCost() As Double
    Get
      Return _miscModernCost
    End Get
    Set(ByVal value As Double)
      _miscModernCost = value
    End Set
  End Property

  Public Property miscNavCost() As Double
    Get
      Return _miscNavCost
    End Get
    Set(ByVal value As Double)
      _miscNavCost = value
    End Set
  End Property

  Public Property calcMiscTotalCost() As Double
    Get
      Return _calcMiscTotalCost
    End Get
    Set(ByVal value As Double)
      _calcMiscTotalCost = value
    End Set
  End Property

  Public Property depreciationCost() As Double
    Get
      Return _depreciationCost
    End Get
    Set(ByVal value As Double)
      _depreciationCost = value
    End Set
  End Property

  Public Property calcTotalFixedCosts() As Double
    Get
      Return _calcTotalFixedCosts
    End Get
    Set(ByVal value As Double)
      _calcTotalFixedCosts = value
    End Set
  End Property

  Public Property numberOfSeats() As Integer
    Get
      Return _number_of_seats
    End Get
    Set(ByVal value As Integer)
      _number_of_seats = value
    End Set
  End Property

  Public Property annualMiles() As Integer
    Get
      Return _annualMiles
    End Get
    Set(ByVal value As Integer)
      _annualMiles = value
    End Set
  End Property

  Public Property calcAnnualHrs() As Integer
    Get
      Return _calcAnnualHrs
    End Get
    Set(ByVal value As Integer)
      _calcAnnualHrs = value
    End Set
  End Property

  Public Property calcTotalDirCostYR() As Double
    Get
      Return _calcTotalDirCostYR
    End Get
    Set(ByVal value As Double)
      _calcTotalDirCostYR = value
    End Set
  End Property

  Public Property calcTotalFixedDirect() As Double
    Get
      Return _calcTotalFixedDirect
    End Get
    Set(ByVal value As Double)
      _calcTotalFixedDirect = value
    End Set
  End Property '

  Public Property calcCostPerHourFixDir() As Double
    Get
      Return _calcCostPerHourFixDir
    End Get
    Set(ByVal value As Double)
      _calcCostPerHourFixDir = value
    End Set
  End Property

  Public Property calcCostPerMileFixDir() As Double
    Get
      Return _calcCostPerMileFixDir
    End Get
    Set(ByVal value As Double)
      _calcCostPerMileFixDir = value
    End Set
  End Property

  Public Property calcCostPerSeatFixDir() As Double
    Get
      Return _calcCostPerSeatFixDir
    End Get
    Set(ByVal value As Double)
      _calcCostPerSeatFixDir = value
    End Set
  End Property

  Public Property variableTotalCost() As Double
    Get
      Return _variableTotalCost
    End Get
    Set(ByVal value As Double)
      _variableTotalCost = value
    End Set
  End Property
  Public Property calcNoDepTotalCost() As Double
    Get
      Return _calcNoDepTotalCost
    End Get
    Set(ByVal value As Double)
      _calcNoDepTotalCost = value
    End Set
  End Property

  Public Property calcCostPerHourNoDep() As Double
    Get
      Return _calcCostPerHourNoDep
    End Get
    Set(ByVal value As Double)
      _calcCostPerHourNoDep = value
    End Set
  End Property

  Public Property calcCostPerMileNoDep() As Double
    Get
      Return _calcCostPerMileNoDep
    End Get
    Set(ByVal value As Double)
      _calcCostPerMileNoDep = value
    End Set
  End Property

  Public Property calcCostPerSeatNoDep() As Double
    Get
      Return _calcCostPerSeatNoDep
    End Get
    Set(ByVal value As Double)
      _calcCostPerSeatNoDep = value
    End Set
  End Property

  Public Property evoCurrencyName() As String
    Get
      Return _evoCurrencyName
    End Get
    Set(ByVal value As String)
      _evoCurrencyName = value
    End Set
  End Property

  Public Property evoCurrencySymbol() As String
    Get
      Return _evoCurrencySymbol
    End Get
    Set(ByVal value As String)
      _evoCurrencySymbol = value
    End Set
  End Property

  Public Property evoCurrencyDate() As String
    Get
      Return _evoCurrencyDate
    End Get
    Set(ByVal value As String)
      _evoCurrencyDate = value
    End Set
  End Property
End Class
