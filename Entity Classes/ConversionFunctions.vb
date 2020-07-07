Public Class ConversionFunctions

  Public Shared Function ConvertCostGallonToCostLiter(ByVal dCostGallon As Double) As Double

    Dim dCostLiter As Double = 0.0

    If CDbl(dCostGallon) > 0.0 Then
      dCostLiter = CDbl(dCostGallon) * 0.26417
    End If

    Return dCostLiter

  End Function ' ConvertCostGallonToCostLiter

  Public Shared Function ConvertNauticalMileToMeter(ByVal nmMile As Double) As Double

    Dim dMeter As Double = 0.0

    If CDbl(nmMile) > 0.0 Then
      dMeter = CDbl(nmMile) * 1852
    End If

    Return dMeter

  End Function ' ConvertNauticalMileToMeter

  Public Shared Function ConvertMeterToNauticalMile(ByVal lMeter As Double) As Double

    Dim dNMMile As Double = 0.0

    If CDbl(lMeter) > 0.0 Then
      dNMMile = CDbl(lMeter) / 1852
    End If

    Return dNMMile

  End Function ' ConvertMeterToNauticalMile

  Public Shared Function ConvertStatuteMileToNauticalMile(ByVal stMile As Double) As Double

    Dim nmMile As Double = 0.0

    If CDbl(stMile) > 0.0 Then
      nmMile = CDbl(stMile) * 0.86898
    End If

    Return nmMile

  End Function ' ConvertStatuteMileToNauticalMile

  Public Shared Function ConvertNauticalMileToStatuteMile(ByVal nmMile As Double) As Double

    Dim stMile As Double = 0.0

    If CDbl(nmMile) > 0.0 Then
      stMile = CDbl(nmMile) * 1.1515
    End If

    Return stMile

  End Function ' ConvertNauticalMileToStatuteMile

  Public Shared Function ConvertFeetToMeter(ByVal lFeet As Double) As Double

    Dim dMeter As Double = 0.0

    If CDbl(lFeet) > 0.0 Then
      dMeter = CDbl(lFeet) * 0.3048
    End If

    Return dMeter

  End Function ' ConvertFeetToMeter

  Public Shared Function ConvertMeterToFeet(ByVal lMeter As Double) As Double

    Dim dFeet As Double = 0.0

    If CDbl(lMeter) > 0.0 Then
      dFeet = CDbl(lMeter) * 3.2808399
    End If

    Return dFeet

  End Function ' ConvertMeterToFeet

  Public Shared Function ConvertNauticalMileToKilometer(ByVal lNMile As Double) As Double

    Dim dKilometer As Double = 0.0

    If CDbl(lNMile) > 0.0 Then
      dKilometer = CDbl(lNMile) * 1.852
    End If

    Return dKilometer

  End Function ' ConvertNauticalMileToKilometer

  Public Shared Function ConvertKilometerToNauticalMile(ByVal lKilometer As Double) As Double

    Dim dNMile As Double = 0.0

    If CDbl(lKilometer) > 0.0 Then
      dNMile = CDbl(lKilometer) * 0.53995
    End If

    Return dNMile

  End Function ' ConvertKilometerToNauticalMile

  Public Shared Function ConvertKilometerToMile(ByVal lKilometer As Double) As Double

    Dim dMile As Double = 0.0

    If CDbl(lKilometer) > 0.0 Then
      dMile = CDbl(lKilometer) * 0.62137
    End If

    Return dMile

  End Function ' ConvertKilometerToMile

  Public Shared Function ConvertKilometerToStatuteMile(ByVal lKilometer As Double) As Double

    Dim dSMile As Double = 0.0

    If CDbl(lKilometer) > 0.0 Then
      dSMile = CDbl(lKilometer) * 0.62137
    End If

    Return dSMile

  End Function ' ConvertKilometerToStatuteMile

  Public Shared Function ConvertStatuteMileToKilometer(ByVal lSMile As Double) As Double

    Dim dKilometer As Double = 0.0

    If CDbl(lSMile) > 0.0 Then
      dKilometer = CDbl(lSMile) * 1.609344
    End If

    Return dKilometer

  End Function ' ConvertStatuteMileToKilometer

  Public Shared Function ConvertMileToKilometer(ByVal lMile As Double) As Double

    Dim dKilometer As Double = 0.0

    If CDbl(lMile) > 0.0 Then
      dKilometer = CDbl(lMile) * 1.609344
    End If

    Return dKilometer

  End Function ' ConvertMileToKilometer

  Public Shared Function ConvertKnotsToKPH(ByVal lKnots As Double) As Double ' Knots To Kilometers Per Hour

    Dim dKPH As Double = 0.0

    If CDbl(lKnots) > 0.0 Then
      dKPH = CDbl(lKnots) * 1.852
    End If

    Return dKPH

  End Function ' ConvertKnotsToKPH 

  Public Shared Function ConvertKPHToKnots(ByVal lKPH As Double) As Double ' Kilometers Per Hour To Knots

    Dim dKnots As Double = 0.0

    If CDbl(lKPH) > 0.0 Then
      dKnots = CDbl(lKPH) * 0.53995
    End If

    Return dKnots

  End Function ' ConvertKPHToKnots 

  Public Shared Function ConvertFPMToMPS(ByVal lFPM As Double) As Double ' Feet Per Minute to Meters Per Second

    Dim dMPS As Double = 0.0

    If CDbl(lFPM) > 0.0 Then
      dMPS = ((CDbl(lFPM) * 0.3048) / 60)
    End If

    Return dMPS

  End Function ' ConvertFPMToMPS

  Public Shared Function ConvertMPSToFPM(ByVal lMPS As Double) As Double ' Meters Per Second To Feet Per Minute

    Dim dFPM As Double = 0.0

    If CDbl(lMPS) > 0.0 Then
      dFPM = ((CDbl(lMPS) * 3.281) * 60)
    End If

    Return dFPM

  End Function ' ConvertMPSToFPM

  Public Shared Function ConvertPSIToHG(ByVal lPSI As Double) As Double ' Pounds Per Square Inch To Milimeter of Mercury (torr)

    Dim dHG As Double = 0.0

    If CDbl(lPSI) > 0.0 Then
      dHG = CDbl(lPSI) * 51.72
    End If

    Return dHG

  End Function ' ConvertPSIToHG

  Public Shared Function ConvertHGToPSI(ByVal lHG As Double) As Double ' Milimeter of Mercury (torr) To Pounds Per Square Inch

    Dim dPSI As Double = 0.0

    If CDbl(lHG) > 0.0 Then
      dPSI = CDbl(lHG) * 0.01934
    End If

    Return dPSI

  End Function ' ConvertHGToPSI

  Public Shared Function ConvertPoundToKilogram(ByVal lPounds As Double) As Double

    Dim dKilo As Double = 0.0

    If CDbl(lPounds) > 0.0 Then
      dKilo = CDbl(lPounds) * 0.4536
    End If

    Return dKilo

  End Function ' ConvertPoundToKilogram

  Public Shared Function ConvertKilogramToPound(ByVal lKilo As Double) As Double

    Dim dPound As Double = 0.0

    If CDbl(lKilo) > 0.0 Then
      dPound = CDbl(lKilo) * 2.205
    End If

    Return dPound

  End Function ' ConvertKilogramToPound

  Public Shared Function ConvertGallonToLiter(ByVal lGallon As Double) As Double

    Dim dLiter As Double = 0.0

    If CDbl(lGallon) > 0.0 Then
      dLiter = CDbl(lGallon) * 3.7854
    End If

    Return dLiter

  End Function ' ConvertGallonToLiter 

  Public Shared Function ConvertLiterToGallon(ByVal lLiter As Double) As Double

    Dim dGallon As Double = 0.0

    If CDbl(lLiter) > 0.0 Then
      dGallon = CDbl(lLiter) * 0.26417
    End If

    Return dGallon

  End Function ' ConvertLiterToGallon

  Public Shared Function ConvertHPToMetricHP(ByVal lHorsepower As Double) As Double

    Dim dMetrichorsepower As Double = 0.0

    If CDbl(lHorsepower) > 0.0 Then
      dMetrichorsepower = CDbl(lHorsepower) * 1.000001
    End If

    Return dMetrichorsepower

  End Function ' ConvertHPToMetricHP 

  Public Shared Function ConvertMetricHPToHP(ByVal lMetricHorsepower As Double) As Double

    Dim dHorsepower As Double = 0.0

    If CDbl(lMetricHorsepower) > 0.0 Then
      dHorsepower = CDbl(lMetricHorsepower) * 0.9999995
    End If

    Return dHorsepower

  End Function ' ConvertMetricHPToHP

  Public Shared Function ConvertCubicFeetToCubicMeter(ByVal lCubicFeet As Double) As Double

    Dim dCubicMeter As Double = 0.0

    If CDbl(lCubicFeet) > 0.0 Then
      dCubicMeter = CDbl(lCubicFeet) * 0.02831685
    End If

    Return dCubicMeter

  End Function ' ConvertFeetToMeter

  Public Shared Function ConvertCubicMeterToCubicFeet(ByVal lCubicMeter As Double) As Double

    Dim dCubicFeet As Double = 0.0

    If CDbl(lCubicMeter) > 0.0 Then
      dCubicFeet = CDbl(lCubicMeter) * 35.3146667
    End If

    Return dCubicFeet

  End Function ' ConvertMeterToFeet

  Public Shared Function TranslateStatuteToNauticalMilesLong(ByVal in_StrToTranslate As String) As String

    Select Case (in_StrToTranslate.ToUpper)

      Case "SM"
        Return "Statute Mile"
      Case "NM"
        Return "Nautical Mile"
      Case Else
        Return UCase(in_StrToTranslate)

    End Select

  End Function

  Public Shared Function ConvertStatuteToNauticalMiles(ByVal in_convertWhat As String, ByVal in_valToConvert As Double) As Double

    Select Case (in_convertWhat.ToUpper)

      Case "NM"
        Return CDbl(ConvertNauticalMileToStatuteMile(in_valToConvert))
      Case "SM"
        Return CDbl(ConvertStatuteMileToNauticalMile(in_valToConvert))
      Case Else
        Return CDbl(in_valToConvert)

    End Select

  End Function

  Public Shared Function TranslateUSMetricUnitsLong(ByVal in_StrToTranslate As String) As String

    Select Case (in_StrToTranslate.ToUpper)

      Case "FT"
        Return "Meter"
      Case "NM"
        Return "Kilometer"
      Case "M"
        Return "Kilometer"
      Case "SM"
        Return "Kilometer"
      Case "KN"
        Return "Kilometers Per Hour"
      Case "FPM"
        Return "Meters Per Second"
      Case "PSI"
        Return "Milimeter of Mercury"
      Case "LB"
        Return "Kilogram"
      Case "GAL"
        Return "Liter"
      Case "HP"
        Return "Metric Horsepower"
      Case Else
        Return UCase(in_StrToTranslate)

    End Select

  End Function

  Public Shared Function TranslateUSMetricUnitsShort(ByVal in_StrToTranslate As String) As String

    Select Case (in_StrToTranslate.ToUpper)

      Case "FT"
        Return "m"
      Case "NM"
        Return "km"
      Case "M"
        Return "km"
      Case "SM"
        Return "km"
      Case "KN"
        Return "kph"
      Case "FPM"
        Return "mps"
      Case "PSI"
        Return "torr"
      Case "LBS"
        Return "kg"
      Case "GAL"
        Return "ltr"
      Case "HP"
        Return "mhp"
      Case "CBFT"
        Return "m3"
      Case Else
        Return UCase(in_StrToTranslate)

    End Select

  End Function

  Public Shared Function ConvertUSToMetricValue(ByVal in_convertWhat As String, ByVal in_valToConvert As Double) As Double

    Select Case (in_convertWhat.ToUpper)

      Case "FT"
        Return CDbl(ConvertFeetToMeter(in_valToConvert))
      Case "NM"
        Return CDbl(ConvertNauticalMileToKilometer(in_valToConvert))
      Case "M"
        Return CDbl(ConvertMileToKilometer(in_valToConvert))
      Case "SM"
        Return CDbl(ConvertStatuteMileToKilometer(in_valToConvert))
      Case "KN"
        Return CDbl(ConvertKnotsToKPH(in_valToConvert))
      Case "FPM"
        Return CDbl(ConvertFPMToMPS(in_valToConvert))
      Case "PSI"
        Return CDbl(ConvertPSIToHG(in_valToConvert))
      Case "LBS"
        Return CDbl(ConvertPoundToKilogram(in_valToConvert))
      Case "GAL"
        Return CDbl(ConvertGallonToLiter(in_valToConvert))
      Case "PPG"
        Return CDbl(ConvertCostGallonToCostLiter(in_valToConvert))
      Case "CBFT"
        Return CDbl(ConvertCubicFeetToCubicMeter(in_valToConvert))
      Case Else
        Return CDbl(in_valToConvert)

    End Select

  End Function

  Public Shared Function ConvertMetricToUSValue(ByVal in_convertWhat As String, ByVal in_valToConvert As Double) As Double

    Select Case (in_convertWhat.ToUpper)

      Case "M"
        Return CDbl(ConvertMeterToFeet(in_valToConvert))
      Case "NK"
        Return CDbl(ConvertKilometerToNauticalMile(in_valToConvert))
      Case "K"
        Return CDbl(ConvertKilometerToMile(in_valToConvert))
      Case "SK"
        Return CDbl(ConvertKilometerToStatuteMile(in_valToConvert))
      Case "KPH"
        Return CDbl(ConvertKPHToKnots(in_valToConvert))
      Case "MPS"
        Return CDbl(ConvertMPSToFPM(in_valToConvert))
      Case "MMHG"
        Return CDbl(ConvertHGToPSI(in_valToConvert))
      Case "KG"
        Return CDbl(ConvertKilogramToPound(in_valToConvert))
      Case "L"
        Return CDbl(ConvertLiterToGallon(in_valToConvert))
      Case "CBFT"
        Return CDbl(ConvertCubicMeterToCubicFeet(in_valToConvert))
      Case Else
        Return CDbl(in_valToConvert)

    End Select

  End Function

  Public Shared Function ConvertUSToForeignCurrency(ByVal in_ExchangeRate, ByVal in_valToConvert) As Double

    Return Math.Round(CDbl(CDbl(in_ExchangeRate) * CDbl(in_valToConvert)), 3)

  End Function

  Public Shared Function Truncate(ByVal value As Double, ByVal precision As Integer) As String

    If precision < 0 Then
      Throw New ArgumentOutOfRangeException("Precision cannot be less than zero")
    End If

    Dim result As String = value.ToString()
    Dim dot As Integer = result.IndexOf(".")

    If dot < 0 Then
      Return result
    End If

    Dim newLength As Integer = dot + precision + 1
    If newLength = dot + 1 Then
      newLength -= 1
    End If

    If newLength > result.Length Then
      newLength = result.Length
    End If

    Return result.Substring(0, newLength)

  End Function

  Public Shared Function ReturnMetricConversonConstant(ByVal in_convertWhat As String) As Double

    Select Case (in_convertWhat.ToUpper)

      Case "FT"
        Return CDbl(0.3048)
      Case "NM"
        Return CDbl(1.852)
      Case "M"
        Return CDbl(1.609344)
      Case "SM"
        Return CDbl(1.609344)
      Case "KN"
        Return CDbl(1.852)
      Case "PSI"
        Return CDbl(51.72)
      Case "LBS"
        Return CDbl(0.4536)
      Case "GAL"
        Return CDbl(3.7854)
      Case "PPG"
        Return CDbl(0.26417)
      Case "CBFT"
        Return CDbl(0.02831685)
      Case Else
        Return CDbl(0.0)

    End Select

  End Function

  Public Shared Function ReturnNauticalStatuteConversonConstant(ByVal in_convertWhat As String) As Double

    Select Case (in_convertWhat.ToUpper)

      Case "NM"
        Return CDbl(0.896)
      Case "SM"
        Return CDbl(1.1515)
      Case Else
        Return CDbl(0.0)

    End Select

  End Function

End Class
