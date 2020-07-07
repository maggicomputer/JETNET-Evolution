Imports Microsoft.VisualBasic
Imports System.ComponentModel

' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/marketSummaryFunctions.vb $
'$$Author: Mike $
'$$Date: 5/28/20 5:34p $
'$$Modtime: 5/28/20 3:54p $
'$$Revision: 8 $
'$$Workfile: marketSummaryFunctions.vb $
'
' ********************************************************************************

<System.Serializable()> Public Class marketSummaryObjAircraft

  Private _modelsString As String
  Private _makeString As String
  Private _typeString As String
  Private _airframeTypeString As String
  Private _combinedAirframeTypeString As String

  Private _weightString As String
  Private _mfrNamesString As String
  Private _sizeString As String

  Private _bHasHelicopter As Boolean
  Private _bHasBusiness As Boolean
  Private _bHasCommercial As Boolean


  Sub New()

    _modelsString = ""
    _makeString = ""
    _typeString = ""

    _airframeTypeString = ""
    _combinedAirframeTypeString = ""

    _weightString = ""
    _mfrNamesString = ""
    _sizeString = ""

    _bHasHelicopter = False
    _bHasBusiness = False
    _bHasCommercial = False

  End Sub

  Public Property ModelsString() As String
    Get
      ModelsString = _modelsString
    End Get
    Set(ByVal value As String)
      _modelsString = value
    End Set
  End Property

  Public Property MakeString() As String
    Get
      MakeString = _makeString
    End Get
    Set(ByVal value As String)
      _makeString = value
    End Set
  End Property

  Public Property TypeString() As String
    Get
      TypeString = _typeString
    End Get
    Set(ByVal value As String)
      _typeString = value
    End Set
  End Property

  Public Property AirframeTypeString() As String
    Get
      AirframeTypeString = _airframeTypeString
    End Get
    Set(ByVal value As String)
      _airframeTypeString = value
    End Set
  End Property

  Public Property CombinedAirframeTypeString() As String
    Get
      CombinedAirframeTypeString = _combinedAirframeTypeString
    End Get
    Set(ByVal value As String)
      _combinedAirframeTypeString = value
    End Set
  End Property

  Public Property WeightString() As String
    Get
      WeightString = _weightString
    End Get
    Set(ByVal value As String)
      _weightString = value
    End Set
  End Property

  Public Property MfrNamesString() As String
    Get
      MfrNamesString = _mfrNamesString
    End Get
    Set(ByVal value As String)
      _mfrNamesString = value
    End Set
  End Property

  Public Property AcsizeString() As String
    Get
      AcsizeString = _sizeString
    End Get
    Set(ByVal value As String)
      _sizeString = value
    End Set
  End Property

  Public Property bHasHelicopter() As Boolean
    Get
      bHasHelicopter = _bHasHelicopter
    End Get
    Set(ByVal value As Boolean)
      _bHasHelicopter = value
    End Set
  End Property

  Public Property bHasBusiness() As Boolean
    Get
      bHasBusiness = _bHasBusiness
    End Get
    Set(ByVal value As Boolean)
      _bHasBusiness = value
    End Set
  End Property

  Public Property bHasCommercial() As Boolean
    Get
      bHasCommercial = _bHasCommercial
    End Get
    Set(ByVal value As Boolean)
      _bHasCommercial = value
    End Set
  End Property

End Class

<System.Serializable()> Public Class marketSummaryFunctions

  Private aError As String
  Private clientConnectString As String
  Private adminConnectString As String

  Private starConnectString As String
  Private cloudConnectString As String
  Private serverConnectString As String
  Private marketFile As System.IO.StreamWriter
  Private marketFile_wHeader As System.IO.StreamWriter


  Sub New()

    aError = ""
    clientConnectString = ""
    adminConnectString = ""

    starConnectString = ""
    cloudConnectString = ""
    serverConnectString = ""
    marketFile = Nothing

  End Sub

  Public Property class_error() As String
    Get
      class_error = aError
    End Get
    Set(ByVal value As String)
      aError = value
    End Set
  End Property

  Public Property marketSummaryFile() As System.IO.StreamWriter
    Get
      marketSummaryFile = marketFile
    End Get
    Set(ByVal value As System.IO.StreamWriter)
      marketFile = value
    End Set
  End Property
  Public Property marketSummaryFile2() As System.IO.StreamWriter
    Get
      marketSummaryFile2 = marketFile_wHeader
    End Get
    Set(ByVal value As System.IO.StreamWriter)
      marketFile_wHeader = value
    End Set
  End Property

#Region "database_connection_strings"

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

#Region "common_market_summary_functions"

  Public Function Get_Quarter_Select(ByVal in_Quarter As String) As String

    Dim tmpQuarterSelect As String = ""

    Select Case in_Quarter.ToLower.Trim
      Case "Q1"
        tmpQuarterSelect = crmWebClient.Constants.cAndClause + "(month(journ_date) >= 1 and month(journ_date) <= 3)"
      Case "Q2"
        tmpQuarterSelect = crmWebClient.Constants.cAndClause + "(month(journ_date) > 3 and month(journ_date) <= 6)"
      Case "Q3"
        tmpQuarterSelect = crmWebClient.Constants.cAndClause + "(month(journ_date) > 6 and month(journ_date) <= 9)"
      Case "Q4"
        tmpQuarterSelect = crmWebClient.Constants.cAndClause + "(month(journ_date) > 9 and month(journ_date) <= 12)"
    End Select

    Return tmpQuarterSelect

  End Function

  Public Function Get_Quarter_For_Month_Server(ByVal in_Month As Integer) As String

    Dim tmpQuarter As String = ""

    Select Case in_Month

      Case 1, 2, 3
        tmpQuarter = "Q1"
      Case 4, 5, 6
        tmpQuarter = "Q2"
      Case 7, 8, 9
        tmpQuarter = "Q3"
      Case 10, 11, 12
        tmpQuarter = "Q4"

    End Select

    Return tmpQuarter

  End Function

  Public Function Get_FirstMonth_For_Quarter_Server(ByVal in_Quarter As String) As Integer

    Dim tmpQuarter As Integer = 0

    Select Case in_Quarter.ToLower.Trim

      Case "q1"
        tmpQuarter = 1
      Case "q2"
        tmpQuarter = 4
      Case "q3"
        tmpQuarter = 7
      Case "q4"
        tmpQuarter = 10

    End Select

    Return tmpQuarter

  End Function

  Public Function ReturnDefaultSummaryRange_Server(ByVal sTimeScale As String) As Integer

    Dim tmpRange As Integer = 0

    Select Case (sTimeScale.ToLower.Trim)
      Case "years"
        tmpRange = 5
      Case "months"
        tmpRange = 6
      Case "days"
        tmpRange = 15
      Case "quarters"
        tmpRange = 4
      Case Else
        tmpRange = 6
    End Select

    Return tmpRange

  End Function

  Public Function ReturnTotalRange_Server(ByVal sTimeScale As String) As Integer

    Dim tmpRange As Integer = 0

    Select Case (sTimeScale.ToLower.Trim)
      Case "years"
        tmpRange = 10
      Case "months"
        tmpRange = 12
      Case "days"
        tmpRange = 31
      Case "quarters"
        tmpRange = 12
      Case Else
        tmpRange = 12
    End Select

    Return tmpRange

  End Function

  Public Function GetTransTypeName(ByVal inTransCode As String) As String

    Dim tmpCodeName As String = "Unknown"

    Select Case inTransCode.ToUpper.Trim
      Case "WS"
        tmpCodeName = "Full Sales"
      Case "WO"
        tmpCodeName = "Written Off"
      Case "OM"
        tmpCodeName = "Off Markets"
      Case "MA"
        tmpCodeName = "On Markets"
      Case "DP"
        tmpCodeName = "Delivery Positions"
      Case "FS"
        tmpCodeName = "Fractional Sales"
      Case "SS"
        tmpCodeName = "Share Sales"
      Case "FC"
        tmpCodeName = "Foreclosures"
      Case "L"
        tmpCodeName = "Leases"
      Case "SZ"
        tmpCodeName = "Seizures"
    End Select

    Return tmpCodeName

  End Function

  Public Function LinkTransTypeName(ByVal inTransCode As String) As String

    Dim tmpCodeName As String = "Unknown"

    Select Case inTransCode.ToUpper.Trim
      Case "WS"
        tmpCodeName = "Whole"
      Case "FS"
        tmpCodeName = "Fractional"
      Case "SS"
        tmpCodeName = "Share"
      Case "DP"
        tmpCodeName = "Delivery Position"
      Case "FC"
        tmpCodeName = "Foreclosures"
      Case "L", "LA", "LN", "LO", "LS", "LX", "LT"
        tmpCodeName = "Leases"
      Case "SZ"
        tmpCodeName = "Seizures"
      Case "OM"
        tmpCodeName = "Off Markets"
      Case "MA"
        tmpCodeName = "On Markets"
      Case "WO"
        tmpCodeName = "Written Off"
    End Select

    Return tmpCodeName

  End Function

  Public Function Get_Lease_Type(ByVal inLeaseType As String) As String

    Dim tmpCodeName As String = "Unknown"

    Select Case inLeaseType.ToUpper.Trim
      Case "LA"
        tmpCodeName = "Lease Available, Still Available"
      Case "LN"
        tmpCodeName = "Lease Not Available, Now Available"
      Case "LO"
        tmpCodeName = "Lease Available, Now Not Available"
      Case "LS"
        tmpCodeName = "Lease"
      Case "LX"
        tmpCodeName = "Lease Expired"
      Case "LT"
        tmpCodeName = "Lease Not Available, Still Not Available"
      Case "L"
        tmpCodeName = "Leases"
    End Select

    Return tmpCodeName

  End Function

  Public Function WriteLineToBoth(ByVal inLine As String, ByVal inFile As System.IO.StreamWriter, Optional ByVal inFile2 As System.IO.StreamWriter = Nothing) As String

    inFile.WriteLine(Replace(Replace(inLine, "<a class=""underline cursor""", "<temp class=""underline cursor"""), "</a>", "</temp>"))
    If Not IsNothing(inFile2) Then
      inFile2.WriteLine(Replace(Replace(inLine, "<a class=""underline cursor""", "<temp class=""underline cursor"""), "</a>", "</temp>"))
    End If

    Return inLine

  End Function

  Public Sub WriteLineToFile(ByVal inLine As String, ByVal inFile As System.IO.StreamWriter, Optional ByVal inFile2 As System.IO.StreamWriter = Nothing)

    If Not IsNothing(inFile) Then
      inFile.WriteLine(Replace(Replace(inLine, "<a class=""underline cursor""", "<temp class=""underline cursor"""), "</a>", "</temp>"))
    End If

    If Not IsNothing(inFile2) Then
      inFile2.WriteLine(Replace(Replace(inLine, "<a class=""underline cursor""", "<temp class=""underline cursor"""), "</a>", "</temp>"))
    End If

  End Sub

  Public Function return_next_previous_date(ByVal sDirection As String, ByVal sTimeScale As String, ByVal in_Date As Date) As Date

    Dim dtReturn As Date = Now()

    Select Case sDirection.ToLower

      Case "next"

        Select Case sTimeScale.ToLower
          Case "years"
            dtReturn = DateAdd("yyyy", 1, in_Date)
          Case "months"
            dtReturn = DateAdd("m", 1, in_Date)
          Case "days"
            dtReturn = DateAdd("d", 1, in_Date)
          Case "quarters"
            dtReturn = DateAdd("q", 1, in_Date)
        End Select

      Case "previous"

        Select Case sTimeScale.ToLower
          Case "years"
            dtReturn = DateAdd("yyyy", -1, in_Date)
          Case "months"
            dtReturn = DateAdd("m", -1, in_Date)
          Case "days"
            dtReturn = DateAdd("d", -1, in_Date)
          Case "quarters"
            dtReturn = DateAdd("q", -1, in_Date)
        End Select

    End Select

    Return dtReturn

  End Function

  Public Sub set_summary_date_range(ByRef dtEndDate As String, ByRef dtStartDate As String, ByVal sTimeScale As String, ByRef nScaleSets As Integer, ByVal isHeliOnly As Boolean)

    Dim dtTempDate As Date = Nothing
    Dim nDisplayRange As Integer = 0
    Dim nMaxRange As Integer = 0
    Dim bShiftStartDate As Boolean = False
    Dim dtSelected As Date = Nothing

    Dim dtMonthBottom As Date = CDate("10/01/1989")
    Dim dtYearBottom As Date = CDate("1/01/1990")

    Dim dtHeliMonthBottom As Date = CDate("01/01/2006")
    Dim dtHeliYearBottom As Date = CDate("01/01/2006")

    If String.IsNullOrEmpty(dtStartDate.Trim) Then

      Select Case sTimeScale.ToLower
        Case "years"
          dtSelected = DateAdd("yyyy", (-1 * (nScaleSets)), CDate(dtEndDate))
        Case "months"
          dtSelected = DateAdd("m", (-1 * (nScaleSets)), CDate(dtEndDate))
        Case "quarters"
          dtSelected = DateAdd("q", (-1 * (nScaleSets)), CDate(Get_FirstMonth_For_Quarter_Server(Get_Quarter_For_Month_Server(Month(CDate(dtEndDate)))).ToString + "/01/" + Year(CDate(dtEndDate)).ToString))
        Case "days"
          dtSelected = DateAdd("d", (-1 * (nScaleSets)), CDate(dtEndDate))
      End Select

    Else
      dtSelected = dtStartDate
    End If

    If Not String.IsNullOrEmpty(dtSelected.ToString) Then

      ' we need to check to make sure we dont go past our bottom limit
      Select Case sTimeScale.ToLower

        Case "years", "quarters"
          If isHeliOnly Then
            If Year(dtHeliYearBottom) >= Year(dtSelected) And Month(dtHeliYearBottom) >= Month(dtSelected) Then
              dtSelected = dtHeliYearBottom
            End If
          Else
            If Year(dtYearBottom) >= Year(dtSelected) And Month(dtYearBottom) >= Month(dtSelected) Then
              dtSelected = dtYearBottom
            End If
          End If
        Case Else
          If isHeliOnly Then
            If Year(dtHeliYearBottom) >= Year(dtSelected) And Month(dtHeliYearBottom) >= Month(dtSelected) Then
              dtSelected = dtHeliYearBottom
            End If
          Else
            If Year(dtYearBottom) >= Year(dtSelected) And Month(dtYearBottom) >= Month(dtSelected) Then
              dtSelected = dtYearBottom
            End If
          End If

      End Select

      dtStartDate = dtSelected

    End If

    If Not String.IsNullOrEmpty(dtSelected) Then

      If sTimeScale.ToLower = "months" Then

        dtTempDate = DateAdd("m", nScaleSets, CDate(dtStartDate))

        If dtTempDate < dtEndDate Then
          dtEndDate = CDate(Month(dtTempDate).ToString + "/01/" + Year(dtTempDate).ToString)
        Else
          dtEndDate = CDate(Month(dtEndDate).ToString + "/01/" + Year(dtEndDate).ToString)
        End If

        nDisplayRange = DateDiff("m", CDate(dtStartDate), CDate(dtEndDate))
        nMaxRange = ReturnTotalRange_Server(sTimeScale)

        If nDisplayRange < nScaleSets Then
          nScaleSets = nDisplayRange
        Else
          If nDisplayRange < nMaxRange Then
            nScaleSets = nDisplayRange
          Else
            nScaleSets = nMaxRange
          End If
        End If

        dtStartDate = CDate(Month(dtSelected).ToString + "/01/" + Year(dtSelected).ToString)

      End If

      If sTimeScale.ToLower = "years" Then

        dtTempDate = DateAdd("yyyy", nScaleSets, CDate(dtStartDate))

        If dtTempDate < dtEndDate Then
          dtEndDate = CDate("01/01/" + Year(dtTempDate).ToString)
        Else
          dtEndDate = CDate("01/01/" + Year(dtEndDate).ToString)
        End If

        nDisplayRange = DateDiff("yyyy", CDate(dtStartDate), CDate(dtEndDate))
        nMaxRange = ReturnTotalRange_Server(sTimeScale)

        If nDisplayRange < nScaleSets Then
          nScaleSets = nDisplayRange
        Else
          If nDisplayRange < nMaxRange Then
            nScaleSets = nDisplayRange
          Else
            nScaleSets = nMaxRange
          End If
        End If

        dtStartDate = CDate("01/01/" + Year(dtSelected).ToString)

      End If

      If sTimeScale.ToLower = "quarters" Then

        dtTempDate = DateAdd("q", nScaleSets, CDate(dtStartDate))

        If dtTempDate < dtEndDate Then
          dtEndDate = CDate(Month(dtTempDate).ToString + "/01/" + Year(dtTempDate).ToString)
        Else
          dtEndDate = CDate(Month(dtEndDate).ToString + "/01/" + Year(dtEndDate).ToString)
        End If

        nDisplayRange = DateDiff("q", CDate(dtStartDate), CDate(dtEndDate))
        nMaxRange = ReturnTotalRange_Server(sTimeScale)

        If nDisplayRange < nScaleSets Then
          nScaleSets = nDisplayRange
        Else
          If nDisplayRange < nMaxRange Then
            nScaleSets = nDisplayRange
          Else
            nScaleSets = nMaxRange
          End If
        End If

        dtStartDate = CDate(Month(dtSelected).ToString + "/01/" + Year(dtSelected).ToString)

      End If

      If sTimeScale.ToLower = "days" Then

        dtTempDate = DateAdd("d", nScaleSets, CDate(dtStartDate))

        If dtTempDate < dtEndDate Then
          dtEndDate = dtTempDate
        End If

        nDisplayRange = DateDiff("d", CDate(dtStartDate), CDate(dtEndDate))
        nMaxRange = ReturnTotalRange_Server(sTimeScale)

        If nDisplayRange < nScaleSets Then
          nScaleSets = nDisplayRange
        Else
          If nDisplayRange < nMaxRange Then
            nScaleSets = nDisplayRange
          Else
            nScaleSets = nMaxRange
          End If
        End If

      End If

    End If

  End Sub

  Public Function generate_timescale_headers(ByRef htmlout As String, ByVal dtStartDate As Date, ByVal dtEndDate As Date, ByVal sTimeScale As String, ByVal isShowTx As Boolean, Optional ByVal isRetail As Boolean = False) As String

    Dim inHeaderString = New StringBuilder()
    Dim Separator As String = ""
    Dim dtTmpDate As Date = Nothing

    Try

      ' SET UP TIMESCALE HEADERS FOR AVAILABLE SUMMARIES
      Select Case sTimeScale.ToLower

        Case "years"

          For xLoop As Integer = Year(dtStartDate) To Year(dtEndDate) - 1

            htmlout += WriteLineToBoth("<td align=""center"" valign=""middle"">" + xLoop.ToString + "</td>", marketFile, marketFile_wHeader)
            inHeaderString.Append(Separator + xLoop.ToString)

            If Not isShowTx Then
              If (xLoop > Year(dtStartDate) And xLoop < Year(dtEndDate)) Then
                htmlout += WriteLineToBoth("<td align=""center"" valign=""middle"">%Chg</td>", marketFile, marketFile_wHeader)
              End If
            End If

            If Not isShowTx Then
              If Not isRetail Then
                htmlout += "<td rowspan=""24"">&nbsp;</td>"
              Else
                htmlout += "<td rowspan=""6"">&nbsp;</td>"
              End If
            End If

            Separator = crmWebClient.Constants.cCommaDelim

          Next

        Case "quarters"

          dtTmpDate = CDate(Month(dtStartDate).ToString + "/01/" + Year(dtStartDate).ToString)

          For xLoop As Integer = 0 To DateDiff("q", dtStartDate, dtEndDate) - 1

            htmlout += WriteLineToBoth("<td align=""center"" valign=""middle"">" + Get_Quarter_For_Month_Server(Month(dtTmpDate)) + "/" + Year(dtTmpDate).ToString + "</td>", marketFile, marketFile_wHeader)
            inHeaderString.Append(Separator + Get_Quarter_For_Month_Server(Month(dtTmpDate)) + "/" + Year(dtTmpDate).ToString)

            If Not isShowTx Then
              If (xLoop > 0) And (xLoop < DateDiff("q", dtStartDate, dtEndDate)) Then
                htmlout += WriteLineToBoth("<td align=""center"" valign=""middle"">%Chg</td>", marketFile, marketFile_wHeader)
              End If
            End If

            If Not isShowTx Then
              If Not isRetail Then
                htmlout += "<td rowspan=""24"">&nbsp;</td>"
              Else
                htmlout += "<td rowspan=""6"">&nbsp;</td>"
              End If
            End If

            Separator = crmWebClient.Constants.cCommaDelim

            dtTmpDate = DateAdd("q", 1, dtTmpDate)

          Next

        Case "months"

          dtTmpDate = dtStartDate

          For xLoop = 0 To DateDiff("m", dtStartDate, dtEndDate) - 1

            htmlout += WriteLineToBoth("<td align=""center"" valign=""middle"">" + Month(dtTmpDate).ToString + "/" + Year(dtTmpDate).ToString + "</td>", marketFile, marketFile_wHeader)
            inHeaderString.Append(Separator + Month(dtTmpDate).ToString + "/" + Year(dtTmpDate).ToString)

            If Not isShowTx Then
              If (xLoop > 0 And xLoop < DateDiff("m", dtStartDate, dtEndDate)) Then
                htmlout += WriteLineToBoth("<td align=""center"" valign=""middle"">%Chg</td>", marketFile, marketFile_wHeader)
              End If
            End If

            If Not isShowTx Then
              If Not isRetail Then
                htmlout += "<td rowspan=""24"">&nbsp;</td>"
              Else
                htmlout += "<td rowspan=""6"">&nbsp;</td>"
              End If
            End If

            Separator = crmWebClient.Constants.cCommaDelim

            dtTmpDate = DateAdd("m", 1, dtTmpDate)
          Next

        Case "days"

          dtTmpDate = dtStartDate

          For xLoop = 0 To DateDiff("d", dtStartDate, dtEndDate) - 1

            htmlout += WriteLineToBoth("<td align=""center"" valign=""middle"">" + Day(dtTmpDate).ToString + "</td>", marketFile, marketFile_wHeader)
            inHeaderString.Append(Separator + Day(dtTmpDate).ToString)

            If Not isShowTx Then
              If (xLoop > 0 And xLoop < DateDiff("d", dtStartDate, dtEndDate)) Then
                htmlout += WriteLineToBoth("<td align=""center"" valign=""middle"">%Chg</td>", marketFile, marketFile_wHeader)
              End If
            End If

            If Not isShowTx Then
              If Not isRetail Then
                htmlout += "<td rowspan=""24"">&nbsp;</td>"
              Else
                htmlout += "<td rowspan=""6"">&nbsp;</td>"
              End If
            End If

            Separator = crmWebClient.Constants.cCommaDelim

            dtTmpDate = DateAdd("d", 1, dtTmpDate)

          Next

      End Select

    Catch ex As Exception

    End Try

    Return inHeaderString.ToString

  End Function

  Public Function WriteColumn(ByVal inValue As Double, ByVal bShowDollarSgn As Boolean, ByVal bDontGroupDigits As Boolean, Optional ByVal bShowPrecision As Boolean = False) As String

    ' Format the columns printed with or without decimals
    Dim htmlout As String = ""

    Try

      If HttpContext.Current.Session.Item("marketTimeScale").ToString.ToLower.Contains("months") Then
        If bShowPrecision Then
          If bShowDollarSgn Then
            htmlout = "<td nowrap=""nowrap"" align=""right"" valign=""middle"">" + FormatCurrency(inValue, 0, False, False, True) + "</td>"
          ElseIf bDontGroupDigits Then
            htmlout = "<td nowrap=""nowrap"" align=""right"" valign=""middle"">" + FormatNumber(inValue, 2, False, False, False) + "</td>"
          Else
            htmlout = "<td nowrap=""nowrap"" align=""right"" valign=""middle"">" + FormatNumber(inValue, 2, True, False, True) + "</td>"
          End If
        Else
          If bShowDollarSgn Then
            htmlout = "<td nowrap=""nowrap"" align=""right"" valign=""middle"">" + FormatCurrency(inValue, 0, False, False, True) + "</td>"
          ElseIf bDontGroupDigits Then
            htmlout = "<td nowrap=""nowrap"" align=""right"" valign=""middle"">" + FormatNumber(inValue, 0, False, False, False) + "</td>"
          Else
            htmlout = "<td nowrap=""nowrap"" align=""right"" valign=""middle"">" + FormatNumber(inValue, 0, True, False, True) + "</td>"
          End If
        End If
      Else
        If bShowDollarSgn Then
          htmlout = "<td nowrap=""nowrap"" align=""right"" valign=""middle"">" + FormatCurrency(inValue, 0, False, False, True) + "</td>"
        ElseIf bDontGroupDigits Then
          htmlout = "<td nowrap=""nowrap"" align=""right"" valign=""middle"">" + FormatNumber(inValue, 2, False, False, False) + "</td>"
        Else
          htmlout = "<td nowrap=""nowrap"" align=""right"" valign=""middle"">" + FormatNumber(inValue, 2, True, False, True) + "</td>"
        End If
      End If

    Catch ex As Exception
      Return "<td nowrap=""nowrap"" align=""center"" valign=""middle"">ERROR</td>"
    End Try

    Return htmlout

  End Function

  Public Function createSimpleReportTitle(ByVal isHeliOnly As Boolean, ByVal isNewUsed As String) As String

    Dim tArray As Array = Nothing

    Dim sWeightClassTitle As String = ""
    Dim sAircraftTypeTitle As String = ""
    Dim sAircraftMakeModelTitle As String = ""

    Dim sAirFrame As String = ""
    Dim sAirType As String = ""
    Dim sMake As String = ""
    Dim sModel As String = ""
    Dim sUsage As String = ""

    Dim hadSeletedItem As Boolean = False
    Dim hadHeliItem As Boolean = False

    Dim strSimpleReportTitle As String = "Market Summary for"

    If Not HttpContext.Current.Session.Item("marketWeightClass").ToString.ToUpper.Contains("ALL") Then

      If HttpContext.Current.Session.Item("marketWeightClass").ToString.ToUpper.Contains("V") Then
        sWeightClassTitle = " Weight Class : Very Light Jet"
      End If

      If HttpContext.Current.Session.Item("marketWeightClass").ToString.ToUpper.Contains("L") Then
        sWeightClassTitle = " Weight Class : Light"
      End If

      If HttpContext.Current.Session.Item("marketWeightClass").ToString.ToUpper.Contains("M") Then
        sWeightClassTitle = " Weight Class : Medium"
      End If

      If HttpContext.Current.Session.Item("marketWeightClass").ToString.ToUpper.Contains("H") Then
        sWeightClassTitle = " Weight Class : Heavy"
      End If

    End If

    If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("tabAircraftType").ToString.Trim) Then

      tArray = Split(HttpContext.Current.Session.Item("tabAircraftType"), crmWebClient.Constants.cMultiDelim)
      If Not IsNothing(tArray) Then

        For nloop As Integer = 0 To UBound(tArray)

          If commonEvo.ReturnModelDataFromIndex(tArray(nloop), sAirFrame, sAirType, sMake, sModel, sUsage) Then

            Select Case (sAirType)
              Case "E"
                If Not String.IsNullOrEmpty(sAircraftTypeTitle.Trim) Then
                  sAircraftTypeTitle += " and Jet Airliners"
                Else
                  sAircraftTypeTitle = "Jet Airliners"
                End If

              Case "J"
                If Not String.IsNullOrEmpty(sAircraftTypeTitle.Trim) Then
                  sAircraftTypeTitle += " and Business Jets"
                Else
                  sAircraftTypeTitle = "Business Jets"
                End If

              Case "T"

                If sAirFrame = "R" Then

                  hadHeliItem = True

                  If Not String.IsNullOrEmpty(sAircraftTypeTitle.Trim) Then
                    sAircraftTypeTitle += " and Turbines"
                  Else
                    sAircraftTypeTitle = "Turbines"
                  End If
                End If

                If sAirFrame = "F" Then
                  If Not String.IsNullOrEmpty(sAircraftTypeTitle.Trim) Then
                    sAircraftTypeTitle += " and Turbo Props"
                  Else
                    sAircraftTypeTitle = "Turbo Props"
                  End If
                End If

              Case "P"
                If Not String.IsNullOrEmpty(sAircraftTypeTitle.Trim) Then
                  sAircraftTypeTitle += " and Pistons"
                Else
                  sAircraftTypeTitle = "Pistons"
                End If

                If sAirFrame = "R" Then
                  hadHeliItem = True
                End If

            End Select

          End If
        Next
      End If

      If UBound(tArray) = 0 Then
        sAircraftTypeTitle = "All " + sAircraftTypeTitle
      End If

    End If

    If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("tabAircraftMake").ToString.Trim) And Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("tabAircraftModel").ToString.Trim) Then

      tArray = Split(HttpContext.Current.Session.Item("tabAircraftModel"), Constants.cCommaDelim)
      If Not IsNothing(tArray) Then

        For nloop As Integer = 0 To UBound(tArray)

          If commonEvo.ReturnModelDataFromIndex(tArray(nloop), sAirFrame, sAirType, sMake, sModel, sUsage) Then

            If Not String.IsNullOrEmpty(sAircraftMakeModelTitle.Trim) Then
              sAircraftMakeModelTitle += " and " + sMake + " / " + sModel
            Else
              sAircraftMakeModelTitle = sMake + " / " + sModel
            End If

          End If
        Next
      End If

      If UBound(tArray) = 0 Then
        sAircraftTypeTitle = "All " + sAircraftMakeModelTitle
      End If

    Else

      If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("tabAircraftMake").ToString.Trim) Then

        tArray = Split(HttpContext.Current.Session.Item("tabAircraftMake"), Constants.cCommaDelim)
        If Not IsNothing(tArray) Then

          For nloop As Integer = 0 To UBound(tArray)

            If commonEvo.ReturnModelDataFromIndex(tArray(nloop), sAirFrame, sAirType, sMake, sModel, sUsage) Then

              If Not String.IsNullOrEmpty(sAircraftMakeModelTitle.Trim) Then
                sAircraftMakeModelTitle += " and " + sMake
              Else
                sAircraftMakeModelTitle = sMake
              End If

            End If
          Next
        End If

        If UBound(tArray) = 0 Then
          sAircraftTypeTitle = "All " + sAircraftMakeModelTitle
        End If

      End If

    End If

    If isHeliOnly Or hadHeliItem Then
      strSimpleReportTitle += crmWebClient.Constants.cSingleSpace + "Helicopters"
      hadSeletedItem = True
    End If

    If Not String.IsNullOrEmpty(sAircraftTypeTitle.Trim) And String.IsNullOrEmpty(sAircraftMakeModelTitle.Trim) Then
      strSimpleReportTitle += crmWebClient.Constants.cSingleSpace + sAircraftTypeTitle
      hadSeletedItem = True
    End If

    If Not String.IsNullOrEmpty(sAircraftMakeModelTitle.Trim) Then
      strSimpleReportTitle += crmWebClient.Constants.cSingleSpace + sAircraftMakeModelTitle
      hadSeletedItem = True
    End If

    If Not String.IsNullOrEmpty(sWeightClassTitle.Trim) Then
      strSimpleReportTitle += crmWebClient.Constants.cSingleSpace + sWeightClassTitle
      hadSeletedItem = True
    End If

    If Not String.IsNullOrEmpty(isNewUsed.Trim) And Not isNewUsed.ToUpper.Contains("ALL") Then
      hadSeletedItem = True
      strSimpleReportTitle += crmWebClient.Constants.cSingleSpace + isNewUsed
    End If

    If isNewUsed.ToUpper.Contains("ALL") And Not hadSeletedItem Then
      strSimpleReportTitle += crmWebClient.Constants.cSingleSpace + isNewUsed
    End If

    Return strSimpleReportTitle

  End Function

  Public Sub fill_startdate_dropdown(ByVal isHeliOnly As Boolean, ByRef start_date_dropdown As DropDownList)

    Dim dtMonthSeed As Date = CDate("10/01/1989")
    Dim dtYearSeed As Date = CDate("1/01/1990")

    Dim dtHeliMonthSeed As Date = CDate("01/01/2006")
    Dim dtHeliYearSeed As Date = CDate("01/01/2006")

    Dim zloop As Integer = 0

    Dim dtSelected As Date = Nothing

    Const MAXARRAYDIM = 1
    Const displayITEM = 0
    Const displayITEMNAME = 1

    Dim dtStartDate As Date = Nothing
    Dim dtEndDate As Date = Nothing

    Dim displayArray(,) As String = Nothing

    Try

      If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("marketStartDate").ToString) Then
        dtSelected = CDate(HttpContext.Current.Session.Item("marketStartDate").ToString)
      Else
        dtSelected = Now()
      End If

      If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("marketEndDate").ToString) Then
        dtEndDate = CDate(HttpContext.Current.Session.Item("marketEndDate").ToString)
      Else
        dtEndDate = Month(Now()).ToString + "/01/" + Year(Now()).ToString
      End If

      start_date_dropdown.Items.Clear()

      Select Case (HttpContext.Current.Session.Item("marketTimeScale").ToString.ToLower)
        Case "years"

          ' 2006,2005,2004,2003,2002,2001
          If isHeliOnly Then
            dtStartDate = dtHeliYearSeed
          Else
            dtStartDate = dtYearSeed
          End If

          ReDim displayArray(DateDiff("yyyy", dtStartDate, dtEndDate) - 1, MAXARRAYDIM)

          ' find total years up to the year before the current year
          For yLoop As Integer = Year(dtStartDate) To Year(dtEndDate) - 1
            displayArray(zloop, displayITEM) = CDate("01/01/" + yLoop.ToString).ToString
            displayArray(zloop, displayITEMNAME) = yLoop.ToString
            zloop += 1
          Next

          ' reverse the string for display
          ' 2001,2002,2003,2004,2005,2006,...

          zloop = 0
          For yLoop As Integer = UBound(displayArray) To LBound(displayArray) Step -1

            start_date_dropdown.Items.Add(New ListItem(displayArray(yLoop, displayITEMNAME), displayArray(yLoop, displayITEM)))

            If Not String.IsNullOrEmpty(dtSelected) Then
              If CDate(displayArray(yLoop, displayITEM)) = CDate("01/01/" + Year(dtSelected).ToString) Then
                start_date_dropdown.Items(zloop).Selected = True
              End If
            End If

            zloop += 1

          Next

        Case "months"

          ' 3/2006,2/2006,1/2006,12/2005,11/2005,10/2005,....

          If isHeliOnly Then
            dtStartDate = dtHeliMonthSeed
          Else
            dtStartDate = dtMonthSeed
          End If

          Dim nTmpMonth As Date = dtStartDate

          ReDim displayArray(DateDiff("m", dtStartDate, dtEndDate) - 1, MAXARRAYDIM)

          ' find total months up to the month before the current month
          For yLoop As Integer = 0 To DateDiff("m", dtStartDate, dtEndDate) - 1

            displayArray(zloop, displayITEM) = nTmpMonth.ToShortDateString
            displayArray(zloop, displayITEMNAME) = Month(nTmpMonth).ToString + "/" + Year(nTmpMonth).ToString

            nTmpMonth = DateAdd("m", 1, nTmpMonth)

            zloop += 1
          Next

          zloop = 0
          ' reverse the string for display
          ' 10/2005,11/2005,12/2005,1/2006,2/2006,3/2006
          For yLoop As Integer = UBound(displayArray) To LBound(displayArray) Step -1

            start_date_dropdown.Items.Add(New ListItem(displayArray(yLoop, displayITEMNAME), displayArray(yLoop, displayITEM)))

            If Not String.IsNullOrEmpty(dtSelected) Then
              If CDate(displayArray(yLoop, displayITEM)) = CDate(Month(dtSelected).ToString + "/01/" + Year(dtSelected).ToString) Then
                start_date_dropdown.Items(zloop).Selected = True
              End If
            End If

            zloop += 1

          Next

        Case "days"

          If isHeliOnly Then
            dtStartDate = dtHeliMonthSeed
          Else
            dtStartDate = dtMonthSeed
          End If

          Dim nTmpDay As Date = dtStartDate

          ReDim displayArray(DateDiff("d", dtStartDate, dtEndDate) - 1, MAXARRAYDIM)

          'find total days up to the day before the current day
          For yLoop As Integer = 0 To DateDiff("d", dtStartDate, dtEndDate) - 1

            displayArray(zloop, displayITEM) = nTmpDay.ToShortDateString
            displayArray(zloop, displayITEMNAME) = nTmpDay.ToShortDateString

            nTmpDay = DateAdd("d", 1, nTmpDay)

            zloop += 1
          Next

          zloop = 0
          'reverse the string for display
          '3/4/2006,3/5/2006,3/6/2006,3/7/2006,3/8/2006,3/9/2006,3/10/2006,...

          For yLoop As Integer = UBound(displayArray) To LBound(displayArray) Step -1

            start_date_dropdown.Items.Add(New ListItem(displayArray(yLoop, displayITEMNAME), displayArray(yLoop, displayITEM)))

            If Not String.IsNullOrEmpty(dtSelected) Then
              If CDate(displayArray(yLoop, displayITEM)) = CDate(dtSelected) Then
                start_date_dropdown.Items(zloop).Selected = True
              End If
            End If

            zloop += 1

          Next

        Case "quarters"

          If isHeliOnly Then
            dtStartDate = dtHeliYearSeed
          Else
            dtStartDate = dtYearSeed
          End If

          ReDim displayArray(DateDiff("q", dtStartDate, dtEndDate) - 1, MAXARRAYDIM)

          Dim dtTmpQuarter As Date = CDate(Month(dtStartDate).ToString + "/01/" + Year(dtStartDate).ToString)

          ' find total quarters up to the quarter before the current quarter                                    
          For yLoop As Integer = 0 To DateDiff("q", dtStartDate, dtEndDate) - 1

            displayArray(zloop, displayITEM) = Month(dtTmpQuarter).ToString + "/01/" + Year(dtTmpQuarter).ToString
            displayArray(zloop, displayITEMNAME) = Get_Quarter_For_Month_Server(Month(dtTmpQuarter)) + "/" + Year(dtTmpQuarter).ToString

            dtTmpQuarter = DateAdd("q", 1, dtTmpQuarter)

            zloop += 1
          Next

          zloop = 0
          ' reverse the string for display
          ' 4Q 04, 1Q 05, 2Q 05, 3Q 05, 4Q 05, 1Q 06 

          For yLoop As Integer = UBound(displayArray) To LBound(displayArray) Step -1

            start_date_dropdown.Items.Add(New ListItem(displayArray(yLoop, displayITEMNAME), displayArray(yLoop, displayITEM)))

            If Not String.IsNullOrEmpty(dtSelected) Then
              If CDate(displayArray(yLoop, displayITEM)) = CDate(Month(dtSelected).ToString + "/01/" + Year(dtSelected).ToString) Then
                start_date_dropdown.Items(zloop).Selected = True
              End If
            End If

            zloop += 1

          Next

      End Select

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in fill_startdate_dropdown(ByVal dtStartDate As Date, ByVal dtEndDate As Date, ByVal sSelTimeScale As String, ByVal isHeliOnly As Boolean, ByRef start_date_dropdown As DropDownList)" + ex.Message

    End Try

    displayArray = Nothing

  End Sub

  Public Function get_business_types() As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT cbus_type, cbus_name FROM Company_Business_Type WITH(NOLOCK) WHERE cbus_aircraft_flag = 'Y'")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_business_types() As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        aError = "Error in get_business_types load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_business_types() As DataTable " + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return atemptable

  End Function

  Public Sub FillBusinessTypeArray(ByRef arrBusinessTypes(,) As String)

    Dim results_table As New DataTable
    Dim nCounter As Integer = 0

    Try

      results_table = get_business_types()

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          ReDim arrBusinessTypes(results_table.Rows.Count - 1, 1)

          For Each r As DataRow In results_table.Rows
            arrBusinessTypes(nCounter, 0) = r.Item("cbus_type").ToString.Trim.ToUpper
            arrBusinessTypes(nCounter, 1) = r.Item("cbus_name").ToString.Trim
            nCounter += 1
          Next

        Else
          ReDim arrBusinessTypes(0, 0)
          arrBusinessTypes(0, 0) = ""
        End If

      Else
        ReDim arrBusinessTypes(0, 0)
        arrBusinessTypes(0, 0) = ""
      End If

    Catch ex As Exception

      aError = "Error in FillBusinessTypeArray(ByRef arrBusinessTypes(,) As String) " + ex.Message

    Finally

    End Try

    results_table = Nothing

  End Sub

  Public Function business_type_name(ByVal inBusinessCode As String, ByVal arrBusinessTypes(,) As String) As String

    Dim sbus_name = ""

    If Not String.IsNullOrEmpty(inBusinessCode.Trim) Then
      For xLoop As Integer = 0 To UBound(arrBusinessTypes)

        If Not String.IsNullOrEmpty(arrBusinessTypes(xLoop, 0).Trim) Then
          If arrBusinessTypes(xLoop, 0).Trim.ToLower.Contains(inBusinessCode.ToLower.Trim) Then
            sbus_name = arrBusinessTypes(xLoop, 1)
            Exit For
          End If
        End If

      Next

    End If

    If String.IsNullOrEmpty(sbus_name.Trim) Then
      sbus_name = "Not Specified [" + inBusinessCode + "]"
    End If

    Return sbus_name

  End Function

  Public Function make_linkback_dateRange(ByVal ColumnSetValue As String, ByVal bIsTotal As Boolean, Optional ByVal bIsHomePage As Boolean = False, Optional ByVal use_month_timeframe As Integer = 0) As String

    Dim market_date_span As String = ""
    Dim sTempTime As String = ""

    If Not bIsHomePage Then

      If Not bIsTotal Then

        If Not IsNothing(HttpContext.Current.Session.Item("marketTimeScale")) Then
          If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("marketTimeScale").ToString.Trim) Then

            Select Case (HttpContext.Current.Session.Item("marketTimeScale").ToString.ToLower.Trim)
              Case "years"
                market_date_span = "01/01/" + ColumnSetValue.Substring(ColumnSetValue.IndexOf("/") + 1, 4)
                market_date_span += ":"
                sTempTime = DateAdd(DateInterval.Year, 1, CDate("01/01/" + ColumnSetValue.Substring(ColumnSetValue.IndexOf("/") + 1, 4)))
                market_date_span += Format(DateAdd(DateInterval.Day, -1, CDate(sTempTime)), "MM/dd/yyyy")
              Case "months"
                market_date_span = ColumnSetValue.Substring(0, IIf(ColumnSetValue.IndexOf("/") = 1, 1, 2)) + "/01/" + ColumnSetValue.Substring(ColumnSetValue.IndexOf("/") + 1, 4)
                market_date_span += ":"
                sTempTime = DateAdd(DateInterval.Month, 1, CDate(ColumnSetValue.Substring(0, IIf(ColumnSetValue.IndexOf("/") = 1, 1, 2)) + "/01/" + ColumnSetValue.Substring(ColumnSetValue.IndexOf("/") + 1, 4)))
                market_date_span += Format(DateAdd(DateInterval.Day, -1, CDate(sTempTime)), "MM/dd/yyyy")
              Case "quarters"
                market_date_span = Get_FirstMonth_For_Quarter_Server(ColumnSetValue.Substring(0, IIf(ColumnSetValue.IndexOf("/") = 1, 1, 2))).ToString + "/01/" + ColumnSetValue.Substring(ColumnSetValue.IndexOf("/") + 1, 4)
                market_date_span += ":"
                sTempTime = DateAdd(DateInterval.Quarter, 1, CDate(Get_FirstMonth_For_Quarter_Server(ColumnSetValue.Substring(0, IIf(ColumnSetValue.IndexOf("/") = 1, 1, 2))).ToString + "/01/" + ColumnSetValue.Substring(ColumnSetValue.IndexOf("/") + 1, 4)))
                market_date_span += Format(DateAdd(DateInterval.Day, -1, CDate(sTempTime)), "MM/dd/yyyy")
              Case "days"
                market_date_span = ColumnSetValue.Trim
                market_date_span += ":"
                market_date_span += Format(DateAdd(DateInterval.Day, 1, CDate(ColumnSetValue.Trim)), "MM/dd/yyyy")

            End Select

          End If
        End If

      Else

        ' check for start date, if we have start date add to timeSpan Format(DateAdd(DateInterval.Day, 1, CDate(ColumnSetValue.Trim)), "MM/dd/yyyy")
        ' check for end date, if we have end date add it to timeSpan
        ' if end date is null or empty clear timeSpan and use default
        If Not IsNothing(HttpContext.Current.Session.Item("marketStartDate")) Then
          If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("marketStartDate").ToString.Trim) Then

            market_date_span = FormatDateTime(CDate(HttpContext.Current.Session.Item("marketStartDate").ToString.Trim), DateFormat.ShortDate)

            If Not IsNothing(HttpContext.Current.Session.Item("marketEndDate")) Then
              If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("marketEndDate").ToString.Trim) Then
                market_date_span += ":"
                sTempTime = FormatDateTime(CDate(HttpContext.Current.Session.Item("marketEndDate").ToString.Trim), DateFormat.ShortDate)
                market_date_span += Format(DateAdd(DateInterval.Day, -1, CDate(sTempTime)), "MM/dd/yyyy")
              Else
                market_date_span = ""
              End If
            Else
              market_date_span = ""
            End If

          End If
        End If

      End If

      If String.IsNullOrEmpty(market_date_span) Then ' default the linkback timespan to "previous month"
        Dim tmpSpanDate As Date = DateAdd(DateInterval.Month, -1, Now)
        sTempTime = Now.Month.ToString + "/01/" + Now.Year.ToString
        market_date_span = tmpSpanDate.Month.ToString + "/01/" + tmpSpanDate.Year.ToString + ":" + Format(DateAdd(DateInterval.Day, -1, CDate(sTempTime)), "MM/dd/yyyy")
      End If

    Else

      If String.IsNullOrEmpty(market_date_span) Then '  linkback timespan to "previous year"
        Dim tmpSpanDate As Date
        If use_month_timeframe > 0 Then
          tmpSpanDate = DateAdd(DateInterval.Month, -use_month_timeframe, Now)
        Else
          tmpSpanDate = DateAdd(DateInterval.Year, -1, Now)
        End If  
        sTempTime = Now.ToShortDateString
        market_date_span = tmpSpanDate.ToShortDateString + ":" + Now.ToShortDateString 'Format(DateAdd(DateInterval.Day, -1, CDate(sTempTime)), "MM/dd/yyyy")
      End If

    End If

    Return market_date_span

  End Function

  Public Function make_linkback_aircraftInfo(ByVal localACSelection As marketSummaryObjAircraft) As String

    Dim nTmpIndex As Long = -1
    Dim nTmpModelID As Long = -1

    Dim sAirFrame As String = ""
    Dim sAirType As String = ""
    Dim sMake As String = ""
    Dim sModel As String = ""
    Dim sUsage As String = ""

    Dim market_aircraft_info As String = ""

    ' add product filters
    market_aircraft_info = "chkHelicopterFilterID=" + localACSelection.bHasHelicopter.ToString.ToLower
    market_aircraft_info += "!~!chkBusinessFilterID=" + localACSelection.bHasBusiness.ToString.ToLower
    market_aircraft_info += "!~!chkCommercialFilterID=" + localACSelection.bHasCommercial.ToString.ToLower

    If (localACSelection.bHasHelicopter Or localACSelection.bHasBusiness Or localACSelection.bHasCommercial) Then
      market_aircraft_info += "!~!hasModelFilterID=True"
    End If

    ' check for model id first
    If Not IsNothing(HttpContext.Current.Request.Item("amod_id")) Then
      If Not String.IsNullOrEmpty(HttpContext.Current.Request.Item("amod_id").ToString) Then

        nTmpModelID = CLng(HttpContext.Current.Request.Item("amod_id").ToString.Trim)

        If nTmpModelID > -1 Then
          nTmpIndex = commonEvo.FindIndexForItemByAmodID(nTmpModelID)
          commonEvo.ReturnModelDataFromIndex(nTmpIndex, sAirFrame, sAirType, sMake, sModel, sUsage)
        End If

        market_aircraft_info += "!~!cboAircraftTypeID=" + sAirType + Constants.cSvrDataSeperator + sAirFrame
        market_aircraft_info += "!~!cboAircraftMakeID=" + sMake.ToUpper.Trim
        market_aircraft_info += "!~!cboAircraftModelID=" + nTmpModelID.ToString

        Return market_aircraft_info

      End If

    End If

    If Not IsNothing(localACSelection) Then
      ' clean out any "ticks" from the "type/make/model" selections

      If Not String.IsNullOrEmpty(localACSelection.TypeString.Trim) Then

        market_aircraft_info += "!~!cboAircraftTypeID=" + localACSelection.TypeString.Replace(Constants.cSingleQuote, Constants.cEmptyString).Trim

        If Not String.IsNullOrEmpty(localACSelection.AirframeTypeString.Trim) Then
          market_aircraft_info += Constants.cSvrDataSeperator + localACSelection.AirframeTypeString.Replace(Constants.cSingleQuote, Constants.cEmptyString).ToUpper.Trim
        End If

      Else

        If Not String.IsNullOrEmpty(localACSelection.AirframeTypeString.Trim) Then
          market_aircraft_info += "!~!cboAircraftTypeID=" + localACSelection.AirframeTypeString.Replace(Constants.cSingleQuote, Constants.cEmptyString).ToUpper.Trim
        End If

      End If

      If Not String.IsNullOrEmpty(localACSelection.MakeString.Trim) Then

        Dim tmpMakeArr As Array = Split(localACSelection.MakeString.Replace(Constants.cSingleQuote, Constants.cEmptyString), Constants.cCommaDelim)
        Dim tmpMakeStr As String = ""

        market_aircraft_info += "!~!cboAircraftMakeID="

        For Each tmpMake As String In tmpMakeArr
          If String.IsNullOrEmpty(tmpMakeStr) Then
            tmpMakeStr = tmpMake.ToUpper.Trim + Constants.cSvrDataSeperator + commonEvo.ReturnAmodIDForItemIndex(commonEvo.FindIndexForFirstItem(tmpMake, Constants.AIRFRAME_MAKE)).ToString
          Else
            tmpMakeStr += Constants.cDymDataSeperator + tmpMake.ToUpper.Trim + Constants.cSvrDataSeperator + commonEvo.ReturnAmodIDForItemIndex(commonEvo.FindIndexForFirstItem(tmpMake, Constants.AIRFRAME_MAKE)).ToString
          End If
        Next

        market_aircraft_info += tmpMakeStr.Trim
      End If

      If Not String.IsNullOrEmpty(localACSelection.ModelsString.Trim) Then
        market_aircraft_info += "!~!cboAircraftModelID=" + localACSelection.ModelsString.Replace(Constants.cCommaDelim, Constants.cDymDataSeperator).Trim
      End If ' 

      If Not String.IsNullOrEmpty(localACSelection.WeightString.Trim) Then

        Dim tmpWeightArr As Array = Split(localACSelection.WeightString.Replace(Constants.cSingleQuote, Constants.cEmptyString), Constants.cCommaDelim)
        Dim tmpWeightStr As String = ""

        market_aircraft_info += "!~!ddlweightclass="

        For Each tmpWeight As String In tmpWeightArr
          If String.IsNullOrEmpty(tmpWeightStr) Then
            tmpWeightStr = tmpWeight.ToUpper.Trim
          Else
            tmpWeightStr += Constants.cDymDataSeperator + tmpWeight.ToUpper.Trim
          End If
        Next

        market_aircraft_info += tmpWeightStr.Trim
      End If

      If Not String.IsNullOrEmpty(localACSelection.MfrNamesString.Trim) Then

        Dim tmpMfrNamesArr As Array = Split(localACSelection.MfrNamesString.Replace(Constants.cSingleQuote, Constants.cEmptyString), Constants.cCommaDelim)
        Dim tmpMfrNamesStr As String = ""

        market_aircraft_info += "!~!ddlMfrNameID="

        For Each tmpMfrName As String In tmpMfrNamesArr
          If String.IsNullOrEmpty(tmpMfrNamesStr) Then
            tmpMfrNamesStr = tmpMfrName.ToUpper.Trim
          Else
            tmpMfrNamesStr += Constants.cDymDataSeperator + tmpMfrName.ToUpper.Trim
          End If
        Next

        market_aircraft_info += tmpMfrNamesStr.Trim
      End If

      If Not String.IsNullOrEmpty(localACSelection.AcsizeString.Trim) Then

        Dim tmpSizeCatArr As Array = Split(localACSelection.AcsizeString.Replace(Constants.cSingleQuote, Constants.cEmptyString), Constants.cCommaDelim)
        Dim tmpSizeCatStr As String = ""

        market_aircraft_info += "!~!ddlSizeCatID="

        For Each tmpSizeCat As String In tmpSizeCatArr
          If String.IsNullOrEmpty(tmpSizeCatStr) Then
            tmpSizeCatStr = tmpSizeCat.ToUpper.Trim
          Else
            tmpSizeCatStr += Constants.cDymDataSeperator + tmpSizeCat.ToUpper.Trim
          End If
        Next

        market_aircraft_info += tmpSizeCatStr.Trim
      End If

    End If

    Return market_aircraft_info

  End Function

  Public Function make_linkback_transactionInfo(ByVal sCurrentTransType As String, ByVal bNoFrom As Boolean, _
                                                ByVal bAddInternal As Boolean, ByVal bExcludeInternalTx As Boolean, _
                                                Optional ByVal sAppendToMarketWhere As String = "", Optional ByVal sFullTransType As String = "", Optional ByVal bTransTotal As Boolean = False) As String

    Dim market_trans_info As String = ""
    Dim bUseAnd As Boolean = False


    If Not String.IsNullOrEmpty(sCurrentTransType) Then

      If Not (sCurrentTransType.Substring(0, 2).ToLower.Contains("ma") Or sCurrentTransType.Substring(0, 2).ToLower.Contains("om") Or sCurrentTransType.Substring(0, 2).ToLower.Contains("wo")) Then

        If sCurrentTransType.Contains(",") Then ' special case from deliveries view
          Dim subcatcode() As String = sCurrentTransType.Split(",")
          market_trans_info = "journ_subcat_code_part1=" + LinkTransTypeName(subcatcode(0).ToUpper.Trim)
          market_trans_info += "##" + LinkTransTypeName(subcatcode(1).ToUpper.Trim)

        Else
          market_trans_info = "journ_subcat_code_part1=" + LinkTransTypeName(sCurrentTransType.Substring(0, 2).ToUpper.Trim)
        End If

        If bTransTotal Then

          If sCurrentTransType.Length > 2 And Not bNoFrom Then

            market_trans_info += IIf(Not String.IsNullOrEmpty(market_trans_info.Trim), "!~!", "") + "journ_subcat_code_part2_operator=From!~!journ_subcat_code_part2=" + sCurrentTransType.Substring(2, 2).ToUpper.Trim

          ElseIf sFullTransType.Length > 2 And Not bNoFrom Then

            market_trans_info += IIf(Not String.IsNullOrEmpty(market_trans_info.Trim), "!~!", "") + "journ_subcat_code_part2_operator=From!~!journ_subcat_code_part2=" + sFullTransType.Substring(2, 2).ToUpper.Trim

          Else

            If bNoFrom Then
              market_trans_info += IIf(Not String.IsNullOrEmpty(market_trans_info.Trim), "!~!", "") + "journ_subcat_code_part2_operator=From!~!journ_subcat_code_part2="
            End If

          End If

        Else

          If sCurrentTransType.Length > 2 Then

            If bNoFrom Then
              market_trans_info += IIf(Not String.IsNullOrEmpty(market_trans_info.Trim), "!~!", "") + "journ_subcat_code_part2_operator=From!~!journ_subcat_code_part2="
            Else
              market_trans_info += IIf(Not String.IsNullOrEmpty(market_trans_info.Trim), "!~!", "") + "journ_subcat_code_part2_operator=From!~!journ_subcat_code_part2=" + sCurrentTransType.Substring(2, 2).ToUpper.Trim
            End If

          End If

          If sCurrentTransType.Length > 4 Then
            market_trans_info += IIf(Not String.IsNullOrEmpty(market_trans_info.Trim), "!~!", "") + "journ_subcat_code_part3_operator=To!~!journ_subcat_code_part3=" + sCurrentTransType.Substring(sCurrentTransType.Length - 2, 2).ToUpper.Trim
          End If

        End If

      Else
        Select Case sCurrentTransType.Substring(0, 2).ToLower
          Case "om" ' off markets
            market_trans_info = "off_markets=true"
          Case "ma" ' on markets
            market_trans_info = "on_markets=true"
          Case "wo" ' written off
            market_trans_info = "written_off=true"
        End Select
      End If

    End If

    If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("marketNewUsed").ToString.Trim) Then

      If HttpContext.Current.Session.Item("marketNewUsed").ToString.Trim.ToUpper.Contains("NEW") Then

        market_trans_info += IIf(Not String.IsNullOrEmpty(market_trans_info.Trim), "!~!", "") + "journ_newac_flag=true"

      ElseIf HttpContext.Current.Session.Item("marketNewUsed").ToString.Trim.ToUpper.Contains("USED") Then

        market_trans_info += IIf(Not String.IsNullOrEmpty(market_trans_info.Trim), "!~!", "") + "jcat_used_retail_sales_flag=true"

      End If

    End If

    If bAddInternal Then

      If bExcludeInternalTx Then
        market_trans_info += IIf(Not String.IsNullOrEmpty(market_trans_info.Trim), "!~!", "") + "sMarketAddToWhereClause=journ_internal_trans_flag equals ?N?" + IIf(Not String.IsNullOrEmpty(sAppendToMarketWhere), Constants.cAndClause + sAppendToMarketWhere.Trim, "")
      Else
        market_trans_info += IIf(Not String.IsNullOrEmpty(market_trans_info.Trim), "!~!", "") + "sMarketAddToWhereClause=journ_internal_trans_flag equals ?Y?" + IIf(Not String.IsNullOrEmpty(sAppendToMarketWhere), Constants.cAndClause + sAppendToMarketWhere.Trim, "")
      End If

      bUseAnd = True

    End If

    If bUseAnd Then
      market_trans_info += Constants.cAndClause + "(jcat_category_code equals ?AH?)"
      market_trans_info += Constants.cAndClause + "(journ_subcategory_code <> ?FSPEND?)"
      market_trans_info += Constants.cAndClause + "(journ_subcategory_code <> ?BIS?)"
      market_trans_info += Constants.cAndClause + "(journ_subcategory_code <> ?CNAME?)"
      market_trans_info += Constants.cAndClause + "(journ_subcategory_code <> ?ACDOC?)"
    Else
      market_trans_info += IIf(Not String.IsNullOrEmpty(market_trans_info.Trim), "!~!", "") + "sMarketAddToWhereClause=(jcat_category_code equals ?AH?)"
      market_trans_info += Constants.cAndClause + "(journ_subcategory_code <> ?FSPEND?)"
      market_trans_info += Constants.cAndClause + "(journ_subcategory_code <> ?BIS?)"
      market_trans_info += Constants.cAndClause + "(journ_subcategory_code <> ?CNAME?)"
      market_trans_info += Constants.cAndClause + "(journ_subcategory_code <> ?ACDOC?)"
    End If

    Return market_trans_info

  End Function

#End Region

#Region "available_summary_functions"

  Public Function Store_Available_Totals(ByRef adoRs As DataRow, _
                                  ByVal nIndex As Integer, _
                                  ByRef AV_For_Sale() As Double, _
                                  ByRef AV_For_Sale_Count() As Double, _
                                  ByRef AV_In_Operation_Fleet() As Double, _
                                  ByRef AV_In_Operation_Fleet_Count() As Double, _
                                  ByRef AV_In_Operation_Fleet_For_Sale() As Double, _
                                  ByRef AV_In_Operation_Fleet_For_Sale_Count() As Double, _
                                  ByRef AV_End_User() As Double, _
                                  ByRef AV_End_User_Count() As Double, _
                                  ByRef AV_End_User_Exc() As Double, _
                                  ByRef AV_End_User_Exc_Count() As Double, _
                                  ByRef AV_Dealer() As Double, _
                                  ByRef AV_Dealer_Count() As Double, _
                                  ByRef AV_Domestic() As Double, _
                                  ByRef AV_Domestic_Count() As Double, _
                                  ByRef AV_International() As Double, _
                                  ByRef AV_International_Count() As Double, _
                                  ByRef AV_Asking_Price_Total() As Double, _
                                  ByRef AV_Asking_Price_Count() As Double, _
                                  ByRef AV_Asking_High() As Double, _
                                  ByRef AV_Asking_Low() As Double, _
                                  ByRef AV_Asking_Make_Offer() As Double, _
                                  ByRef AV_Asking_Make_Offer_Count() As Double, _
                                  ByRef AV_Avg_Year_Total() As Double, _
                                  ByRef AV_Avg_Year_Count() As Double, _
                                  ByRef AV_Avg_Airframe_TT_Total() As Double, _
                                  ByRef AV_Avg_Airframe_TT_Count() As Double, _
                                  ByRef AV_Avg_Engine_TT_Total() As Double, _
                                  ByRef AV_Avg_Engine_TT_Count() As Double, _
                                  ByRef AV_New_To_Market() As Double, _
                                  ByRef AV_New_To_Market_Count() As Double, _
                                  ByRef AV_Delivery_Position() As Double, _
                                  ByRef AV_Lease() As Double, _
                                  ByRef AV_Fractional() As Double, _
                                  ByRef AV_DOM() As Double, _
                                  ByRef AV_DOM_COUNT() As Double)

    Dim fMtrend_total_aircraft_for_sale As Double = 0.0
    Dim fMtrend_lifecycle_3_count As Double = 0.0

    Dim fMtrend_end_user_count As Double = 0.0
    Dim fMtrend_euser_exclusive_count As Double = 0.0
    Dim fMtrend_dealer_owned_count As Double = 0.0
    Dim fMtrend_domestic_count As Double = 0.0
    Dim fMtrend_international_count As Double = 0.0

    ' If the total value and total count are available then use them
    Dim fMtrend_avail_asking_price_total As Double = 0.0
    Dim fMtrend_avail_asking_price_count As Double = 0.0
    Dim fMtrend_avg_asking_price As Double = 0.0
    Dim fMtrend_low_asking_price As Double = 0.0
    Dim fMtrend_high_asking_price As Double = 0.0
    Dim fMtrend_make_offer_count As Double = 0.0
    Dim fMtrend_avail_fractowr_count As Double = 0.0
    Dim fMtrend_avg_year_total As Double = 0.0
    Dim fMtrend_avg_year_count As Double = 0.0
    Dim fMtrend_avg_dom As Double = 0.0
    Dim fMtrend_avg_airframe_time As Double = 0.0
    Dim fMtrend_avg_engine_time As Double = 0.0
    Dim fMtrend_avail_lease_count As Double = 0.0
    Dim fMtrend_avail_new_onmarket_count As Double = 0.0
    Dim fMtrend_sold_new_onmarket_count As Double = 0.0
    Dim fMtrend_avail_dlvypos_count As Double = 0.0
    Dim nNewtoMarket As Double = 0.0

    Try

      If Not (IsDBNull(adoRs.Item("mtrend_total_aircraft_for_sale"))) Then
        fMtrend_total_aircraft_for_sale = CDbl(adoRs.Item("mtrend_total_aircraft_for_sale").ToString)
      Else
        fMtrend_total_aircraft_for_sale = 0.0
      End If

      If Not (IsDBNull(adoRs.Item("mtrend_lifecycle_3_count"))) Then
        fMtrend_lifecycle_3_count = CDbl(adoRs.Item("mtrend_lifecycle_3_count").ToString)
      Else
        fMtrend_lifecycle_3_count = 0.0
      End If

      If Not (IsDBNull(adoRs.Item("mtrend_end_user_count"))) Then
        fMtrend_end_user_count = CDbl(adoRs.Item("mtrend_end_user_count").ToString)
      Else
        fMtrend_end_user_count = 0.0
      End If

      If Not (IsDBNull(adoRs.Item("mtrend_euser_exclusive_count"))) Then
        fMtrend_euser_exclusive_count = CDbl(adoRs.Item("mtrend_euser_exclusive_count").ToString)
      Else
        fMtrend_euser_exclusive_count = 0.0
      End If

      If Not (IsDBNull(adoRs.Item("mtrend_dealer_owned_count"))) Then
        fMtrend_dealer_owned_count = CDbl(adoRs.Item("mtrend_dealer_owned_count").ToString)
      Else
        fMtrend_dealer_owned_count = 0.0
      End If

      If Not (IsDBNull(adoRs.Item("mtrend_domestic_count"))) Then
        fMtrend_domestic_count = CDbl(adoRs.Item("mtrend_domestic_count").ToString)
      Else
        fMtrend_domestic_count = 0.0
      End If

      If Not (IsDBNull(adoRs.Item("mtrend_international_count"))) Then
        fMtrend_international_count = CDbl(adoRs.Item("mtrend_international_count").ToString)
      Else
        fMtrend_international_count = 0.0
      End If

      If Not (IsDBNull(adoRs.Item("AVGDOM"))) Then
        fMtrend_avg_dom = CDbl(adoRs.Item("AVGDOM").ToString)
      Else
        fMtrend_avg_dom = 0.0
      End If

      If fMtrend_total_aircraft_for_sale <> 0 Then
        AV_For_Sale(nIndex) += fMtrend_total_aircraft_for_sale
        AV_For_Sale_Count(nIndex) += 1
      End If

      If fMtrend_end_user_count <> 0 Then
        AV_End_User(nIndex) += fMtrend_end_user_count
        AV_End_User_Count(nIndex) += 1
      End If

      If fMtrend_euser_exclusive_count <> 0 Then
        AV_End_User_Exc(nIndex) += fMtrend_euser_exclusive_count
        AV_End_User_Exc_Count(nIndex) += 1
      End If

      If fMtrend_dealer_owned_count <> 0 Then
        AV_Dealer(nIndex) += fMtrend_dealer_owned_count
        AV_Dealer_Count(nIndex) += 1
      End If

      If fMtrend_domestic_count <> 0 Then
        AV_Domestic(nIndex) += fMtrend_domestic_count
        AV_Domestic_Count(nIndex) += 1
      End If

      If fMtrend_international_count <> 0 Then
        AV_International(nIndex) += fMtrend_international_count
        AV_International_Count(nIndex) += 1
      End If

      If fMtrend_lifecycle_3_count <> 0 Then
        AV_In_Operation_Fleet(nIndex) += fMtrend_lifecycle_3_count
        AV_In_Operation_Fleet_Count(nIndex) += 1
      End If

      If fMtrend_lifecycle_3_count <> 0 And fMtrend_total_aircraft_for_sale <> 0 Then
        AV_In_Operation_Fleet_For_Sale(nIndex) = System.Math.Round(CDbl((AV_For_Sale(nIndex) / AV_In_Operation_Fleet(nIndex)) * 100), 2)
        AV_In_Operation_Fleet_For_Sale_Count(nIndex) += 1
      End If

      If Not (IsDBNull(adoRs.Item("mtrend_avail_asking_price_total"))) Then
        fMtrend_avail_asking_price_total = CDbl(adoRs.Item("mtrend_avail_asking_price_total").ToString)
      Else
        fMtrend_avail_asking_price_total = 0.0
      End If

      If Not (IsDBNull(adoRs.Item("mtrend_avail_asking_price_count"))) Then
        fMtrend_avail_asking_price_count = CDbl(adoRs.Item("mtrend_avail_asking_price_count").ToString)
      Else
        fMtrend_avail_asking_price_count = 0.0
      End If

      If Not (IsDBNull(adoRs.Item("mtrend_avg_asking_price"))) Then
        fMtrend_avg_asking_price = CDbl(adoRs.Item("mtrend_avg_asking_price").ToString)
      Else
        fMtrend_avg_asking_price = 0.0
      End If

      If Not (IsDBNull(adoRs.Item("mtrend_high_asking_price"))) Then
        fMtrend_high_asking_price = CDbl(adoRs.Item("mtrend_high_asking_price").ToString)
      Else
        fMtrend_high_asking_price = 0.0
      End If

      If Not (IsDBNull(adoRs.Item("mtrend_low_asking_price"))) Then
        fMtrend_low_asking_price = CDbl(adoRs.Item("mtrend_low_asking_price").ToString)
      Else
        fMtrend_low_asking_price = 0.0
      End If

      If Not (IsDBNull(adoRs.Item("mtrend_make_offer_count"))) Then
        fMtrend_make_offer_count = CDbl(adoRs.Item("mtrend_make_offer_count").ToString)
      Else
        fMtrend_make_offer_count = 0.0
      End If

      If Not (IsDBNull(adoRs.Item("mtrend_avg_year_total"))) Then
        fMtrend_avg_year_total = CDbl(adoRs.Item("mtrend_avg_year_total").ToString)
      Else
        fMtrend_avg_year_total = 0.0
      End If

      If Not (IsDBNull(adoRs.Item("mtrend_avg_year_count"))) Then
        fMtrend_avg_year_count = CDbl(adoRs.Item("mtrend_avg_year_count").ToString)
      Else
        fMtrend_avg_year_count = 0.0
      End If

      If Not (IsDBNull(adoRs.Item("mtrend_avg_airframe_time"))) Then
        fMtrend_avg_airframe_time = CDbl(adoRs.Item("mtrend_avg_airframe_time").ToString)
      Else
        fMtrend_avg_airframe_time = 0.0
      End If

      If Not (IsDBNull(adoRs.Item("mtrend_avg_engine_time"))) Then
        fMtrend_avg_engine_time = CDbl(adoRs.Item("mtrend_avg_engine_time").ToString)
      Else
        fMtrend_avg_engine_time = 0.0
      End If

      If Not (IsDBNull(adoRs.Item("mtrend_avail_new_onmarket_count"))) Then
        fMtrend_avail_new_onmarket_count = CDbl(adoRs.Item("mtrend_avail_new_onmarket_count").ToString)
      Else
        fMtrend_avail_new_onmarket_count = 0.0
      End If

      If Not (IsDBNull(adoRs.Item("mtrend_sold_new_onmarket_count"))) Then
        fMtrend_sold_new_onmarket_count = CDbl(adoRs.Item("mtrend_sold_new_onmarket_count").ToString)
      Else
        fMtrend_sold_new_onmarket_count = 0.0
      End If

      If Not (IsDBNull(adoRs.Item("mtrend_avail_dlvypos_count"))) Then
        fMtrend_avail_dlvypos_count = CDbl(adoRs.Item("mtrend_avail_dlvypos_count").ToString)
      Else
        fMtrend_avail_dlvypos_count = 0.0
      End If

      If Not (IsDBNull(adoRs.Item("mtrend_avail_lease_count"))) Then
        fMtrend_avail_lease_count = CDbl(adoRs.Item("mtrend_avail_lease_count").ToString)
      Else
        fMtrend_avail_lease_count = 0.0
      End If

      If Not (IsDBNull(adoRs.Item("mtrend_avail_fractowr_count"))) Then
        fMtrend_avail_fractowr_count = CDbl(adoRs.Item("mtrend_avail_fractowr_count").ToString)
      Else
        fMtrend_avail_fractowr_count = 0.0
      End If

      If fMtrend_avail_asking_price_total <> 0 And fMtrend_avail_asking_price_count <> 0 Then

        AV_Asking_Price_Total(nIndex) += fMtrend_avail_asking_price_total
        AV_Asking_Price_Count(nIndex) += fMtrend_avail_asking_price_count

      Else ' Use the Already Generated Asking Averages

        If fMtrend_avg_asking_price <> 0 Then
          AV_Asking_Price_Total(nIndex) += fMtrend_avg_asking_price
          AV_Asking_Price_Count(nIndex) += 1
        End If

      End If

      If CDbl(AV_Asking_High(nIndex)) < fMtrend_high_asking_price Then
        AV_Asking_High(nIndex) = fMtrend_high_asking_price
      End If

      If fMtrend_low_asking_price > 0 Then
        If CDbl(AV_Asking_Low(nIndex)) = 0 Or CDbl(AV_Asking_Low(nIndex)) > fMtrend_low_asking_price Then
          AV_Asking_Low(nIndex) = fMtrend_low_asking_price
        End If
      End If

      If fMtrend_make_offer_count <> 0 Then
        AV_Asking_Make_Offer(nIndex) += fMtrend_make_offer_count
        AV_Asking_Make_Offer_Count(nIndex) += 1
      End If

      If fMtrend_avg_year_total <> 0 Then
        AV_Avg_Year_Total(nIndex) += fMtrend_avg_year_total
        AV_Avg_Year_Count(nIndex) += fMtrend_avg_year_count
      End If

      If fMtrend_avg_airframe_time <> 0 Then
        AV_Avg_Airframe_TT_Total(nIndex) += fMtrend_avg_airframe_time
        AV_Avg_Airframe_TT_Count(nIndex) += 1
      End If

      If fMtrend_avg_engine_time <> 0 Then
        AV_Avg_Engine_TT_Total(nIndex) += fMtrend_avg_engine_time
        AV_Avg_Engine_TT_Count(nIndex) += 1
      End If

      'A/C NEW on Mkt
      If fMtrend_avail_new_onmarket_count <> 0 Then ' Use New Fields
        nNewtoMarket = fMtrend_avail_new_onmarket_count + fMtrend_sold_new_onmarket_count
      End If

      If nNewtoMarket <> 0 Then
        AV_New_To_Market(nIndex) += nNewtoMarket
        AV_New_To_Market_Count(nIndex) += 1
      End If

      If fMtrend_avg_dom <> 0 Then
        AV_DOM(nIndex) += fMtrend_avg_dom
        AV_DOM_COUNT(nIndex) += 1
      End If

      'Chg Mkt Invtry is a Calculated Field - Does NOT get Written To File
      'Chg Dlr Invtry is a Calculated Field - Does NOT get Written To File     

      AV_Delivery_Position(nIndex) += fMtrend_avail_dlvypos_count
      AV_Lease(nIndex) += fMtrend_avail_lease_count
      AV_Fractional(nIndex) += fMtrend_avail_fractowr_count

    Catch ex As Exception
      Return False
    End Try

    Return True

  End Function

  Public Function Print_Available_Summaries(ByVal dtStartDate As Date, _
                                     ByVal dtEndDate As Date, _
                                     ByVal sHeaderString As String, _
                                     ByVal sColSpan As String, _
                                     ByVal ColumnSet() As String, _
                                     ByVal YearMonth_Count() As Double, _
                                     ByVal AV_For_Sale() As Double, _
                                     ByVal AV_For_Sale_Count() As Double, _
                                     ByVal AV_In_Operation_Fleet() As Double, _
                                     ByVal AV_In_Operation_Fleet_Count() As Double, _
                                     ByVal AV_In_Operation_Fleet_For_Sale() As Double, _
                                     ByVal AV_In_Operation_Fleet_For_Sale_Count() As Double, _
                                     ByVal AV_End_User() As Double, _
                                     ByVal AV_End_User_Count() As Double, _
                                     ByVal AV_End_User_Exc() As Double, _
                                     ByVal AV_End_User_Exc_Count() As Double, _
                                     ByVal AV_Dealer() As Double, _
                                     ByVal AV_Dealer_Count() As Double, _
                                     ByVal AV_Domestic() As Double, _
                                     ByVal AV_Domestic_Count() As Double, _
                                     ByVal AV_International() As Double, _
                                     ByVal AV_International_Count() As Double, _
                                     ByVal AV_Asking_Price_Total() As Double, _
                                     ByVal AV_Asking_Price_Count() As Double, _
                                     ByVal AV_Asking_High() As Double, _
                                     ByVal AV_Asking_Low() As Double, _
                                     ByVal AV_Asking_Make_Offer() As Double, _
                                     ByVal AV_Asking_Make_Offer_Count() As Double, _
                                     ByVal AV_Avg_Year_Total() As Double, _
                                     ByVal AV_Avg_Year_Count() As Double, _
                                     ByVal AV_Avg_Airframe_TT_Total() As Double, _
                                     ByVal AV_Avg_Airframe_TT_Count() As Double, _
                                     ByVal AV_Avg_Engine_TT_Total() As Double, _
                                     ByVal AV_Avg_Engine_TT_Count() As Double, _
                                     ByVal AV_New_To_Market() As Double, _
                                     ByVal AV_New_To_Market_Count() As Double, _
                                     ByVal AV_Delivery_Position() As Double, _
                                     ByVal AV_Lease() As Double, _
                                     ByVal AV_Fractional() As Double, _
                                     ByVal AV_DOM() As Double, _
                                     ByVal AV_DOM_COUNT() As Double) As String
    Dim tmpData As String = ""
    Dim htmlOut = New StringBuilder()
    Dim sRefLink As String = ""

    Dim tmpMarketGraphData As marketGraphData = Nothing

    Try

      ' print total aircraft for sale
      htmlOut.Append(WriteLineToBoth("<tr>", marketFile, marketFile_wHeader))

      If HttpContext.Current.Session.Item("marketTimeScale").ToString.ToLower.Contains("years") Or HttpContext.Current.Session.Item("marketTimeScale").ToString.ToLower.Contains("quarters") Then

        tmpMarketGraphData = New marketGraphData

        tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.AV_FORSALE
        tmpMarketGraphData.marketGraph_topTitle = "Aircraft for Sale"

        tmpMarketGraphData.marketGraph_Y_title = "Average For Sale"

        For x As Integer = 0 To UBound(AV_For_Sale)
          If String.IsNullOrEmpty(tmpData) Then
            tmpData = FormatNumber((AV_For_Sale(x) / YearMonth_Count(x)), 0, False, False, False).ToString
          Else
            tmpData += crmWebClient.Constants.cCommaDelim + FormatNumber((AV_For_Sale(x) / YearMonth_Count(x)), 0, False, False, False).ToString
          End If
        Next

        tmpMarketGraphData.marketGraph_Y_data = tmpData
        tmpData = ""

        tmpMarketGraphData.marketGraph_X_title = "per Year"
        tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

        If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
          HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.AV_FORSALE) - 1) = tmpMarketGraphData
        End If

        sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.AV_FORSALE).ToString + "','marketSummaryGraph');"

        htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">Aircraft for Sale (Avg)</a></td>", marketFile, marketFile_wHeader))

      Else

        tmpMarketGraphData = New marketGraphData

        tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.AV_FORSALE
        tmpMarketGraphData.marketGraph_topTitle = "Aircraft for Sale"

        tmpMarketGraphData.marketGraph_Y_title = "For Sale"

        For x As Integer = 0 To UBound(AV_For_Sale)
          If String.IsNullOrEmpty(tmpData) Then
            tmpData = AV_For_Sale(x).ToString
          Else
            tmpData += crmWebClient.Constants.cCommaDelim + AV_For_Sale(x).ToString
          End If
        Next

        tmpMarketGraphData.marketGraph_Y_data = tmpData
        tmpData = ""

        tmpMarketGraphData.marketGraph_X_title = "per Month"
        tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

        If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
          HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.AV_FORSALE) - 1) = tmpMarketGraphData
        End If

        sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.AV_FORSALE).ToString + "','marketSummaryGraph');"
        htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">Aircraft for Sale</a></td>", marketFile, marketFile_wHeader))

      End If

      htmlOut.Append(Calculate_Available_Summaries(ColumnSet, AV_For_Sale, AV_For_Sale_Count, YearMonth_Count, dtEndDate, True, False, False, False, False))

      htmlOut.Append(WriteLineToBoth("</tr>", marketFile, marketFile_wHeader))

      ' print in operation fleet
      htmlOut.Append(WriteLineToBoth("<tr>", marketFile, marketFile_wHeader))

      If HttpContext.Current.Session.Item("marketTimeScale").ToString.ToLower.Contains("years") Or HttpContext.Current.Session.Item("marketTimeScale").ToString.ToLower.Contains("quarters") Then

        tmpMarketGraphData = New marketGraphData

        tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.AV_INOPERATION
        tmpMarketGraphData.marketGraph_topTitle = "In Operation Fleet"

        tmpMarketGraphData.marketGraph_Y_title = "Average In Operation Fleet"

        For x As Integer = 0 To UBound(AV_In_Operation_Fleet)
          If String.IsNullOrEmpty(tmpData) Then
            tmpData = FormatNumber((AV_In_Operation_Fleet(x) / YearMonth_Count(x)), 0, False, False, False).ToString
          Else
            tmpData += crmWebClient.Constants.cCommaDelim + FormatNumber((AV_In_Operation_Fleet(x) / YearMonth_Count(x)), 0, False, False, False).ToString
          End If
        Next

        tmpMarketGraphData.marketGraph_Y_data = tmpData
        tmpData = ""

        tmpMarketGraphData.marketGraph_X_title = "per Year"
        tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

        If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
          HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.AV_INOPERATION) - 1) = tmpMarketGraphData
        End If

        sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.AV_INOPERATION).ToString + "','marketSummaryGraph');"

        htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">In Operation Fleet (Avg)</a></td>", marketFile, marketFile_wHeader))

      Else

        tmpMarketGraphData = New marketGraphData

        tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.AV_INOPERATION
        tmpMarketGraphData.marketGraph_topTitle = "In Operation Fleet"

        tmpMarketGraphData.marketGraph_Y_title = "In Operation Fleet"

        For x As Integer = 0 To UBound(AV_In_Operation_Fleet)
          If String.IsNullOrEmpty(tmpData) Then
            tmpData = AV_In_Operation_Fleet(x).ToString
          Else
            tmpData += crmWebClient.Constants.cCommaDelim + AV_In_Operation_Fleet(x).ToString
          End If
        Next

        tmpMarketGraphData.marketGraph_Y_data = tmpData
        tmpData = ""

        tmpMarketGraphData.marketGraph_X_title = "per Month"
        tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

        If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
          HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.AV_INOPERATION) - 1) = tmpMarketGraphData
        End If

        sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.AV_INOPERATION).ToString + "','marketSummaryGraph');"
        htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">In Operation Fleet</a></td>", marketFile, marketFile_wHeader))

      End If

      htmlOut.Append(Calculate_Available_Summaries(ColumnSet, AV_In_Operation_Fleet, AV_In_Operation_Fleet_Count, YearMonth_Count, dtEndDate, True, False, False, False, False))

      htmlOut.Append(WriteLineToBoth("</tr>", marketFile, marketFile_wHeader))

      ' print in operation fleet for sale
      htmlOut.Append(WriteLineToBoth("<tr>", marketFile, marketFile_wHeader))

      If HttpContext.Current.Session.Item("marketTimeScale").ToString.ToLower.Contains("years") Or HttpContext.Current.Session.Item("marketTimeScale").ToString.ToLower.Contains("quarters") Then

        tmpMarketGraphData = New marketGraphData

        tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.AV_INOPERATIONFORSALE
        tmpMarketGraphData.marketGraph_topTitle = "In Operation Fleet for Sale"

        tmpMarketGraphData.marketGraph_Y_title = "Average In Operation For Sale"
        '' MATT COMMENTED OUT THE  / YearMonth_Count(x) IN THESE LINES, BELIEVES IT IS DIVIDING TWICE - 12/8/17
        For x As Integer = 0 To UBound(AV_In_Operation_Fleet_For_Sale)
          If String.IsNullOrEmpty(tmpData) Then
            tmpData = FormatNumber((AV_In_Operation_Fleet_For_Sale(x)), 0, False, False, False).ToString
          Else
            tmpData += crmWebClient.Constants.cCommaDelim + FormatNumber((AV_In_Operation_Fleet_For_Sale(x)), 0, False, False, False).ToString
          End If
        Next



        tmpMarketGraphData.marketGraph_Y_data = tmpData
        tmpData = ""

        tmpMarketGraphData.marketGraph_X_title = "per Year"
        tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

        If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
          HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.AV_INOPERATIONFORSALE) - 1) = tmpMarketGraphData
        End If

        sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.AV_INOPERATIONFORSALE).ToString + "','marketSummaryGraph');"

        htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">% In Operation for Sale (Avg)</a></td>", marketFile, marketFile_wHeader))

      Else

        tmpMarketGraphData = New marketGraphData

        tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.AV_INOPERATIONFORSALE
        tmpMarketGraphData.marketGraph_topTitle = "In Operation Fleet for Sale"

        tmpMarketGraphData.marketGraph_Y_title = "In Operation For Sale"

        For x As Integer = 0 To UBound(AV_In_Operation_Fleet_For_Sale)
          If String.IsNullOrEmpty(tmpData) Then
            tmpData = AV_In_Operation_Fleet_For_Sale(x).ToString
          Else
            tmpData += crmWebClient.Constants.cCommaDelim + AV_In_Operation_Fleet_For_Sale(x).ToString
          End If
        Next

        tmpMarketGraphData.marketGraph_Y_data = tmpData
        tmpData = ""

        tmpMarketGraphData.marketGraph_X_title = "per Month"
        tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

        If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
          HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.AV_INOPERATIONFORSALE) - 1) = tmpMarketGraphData
        End If

        sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.AV_INOPERATIONFORSALE).ToString + "','marketSummaryGraph');"
        htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">% In Operation for Sale</a></td>", marketFile, marketFile_wHeader))

      End If

      htmlOut.Append(Calculate_Available_Summaries(ColumnSet, AV_In_Operation_Fleet_For_Sale, AV_In_Operation_Fleet_For_Sale_Count, YearMonth_Count, dtEndDate, True, False, False, False, False, True, True))

      htmlOut.Append(WriteLineToBoth("</tr>", marketFile, marketFile_wHeader))
      htmlOut.Append("<tr height=""4""><td colspan=""" + sColSpan + """></td></tr>")

      ' print end user aircraft for sale
      htmlOut.Append(WriteLineToBoth("<tr>", marketFile, marketFile_wHeader))

      If HttpContext.Current.Session.Item("marketTimeScale").ToString.ToLower.Contains("years") Or HttpContext.Current.Session.Item("marketTimeScale").ToString.ToLower.Contains("quarters") Then

        tmpMarketGraphData = New marketGraphData

        tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.AV_FORSALE_EU
        tmpMarketGraphData.marketGraph_topTitle = "Aircraft For Sale by End User"

        tmpMarketGraphData.marketGraph_Y_title = "Average For Sale"

        For x As Integer = 0 To UBound(AV_End_User)
          If String.IsNullOrEmpty(tmpData) Then
            tmpData = FormatNumber((AV_End_User(x) / YearMonth_Count(x)), 0, False, False, False).ToString
          Else
            tmpData += crmWebClient.Constants.cCommaDelim + FormatNumber((AV_End_User(x) / YearMonth_Count(x)), 0, False, False, False).ToString
          End If
        Next

        tmpMarketGraphData.marketGraph_Y_data = tmpData
        tmpData = ""

        tmpMarketGraphData.marketGraph_X_title = "per Year"
        tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

        If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
          HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.AV_FORSALE_EU) - 1) = tmpMarketGraphData
        End If

        sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.AV_FORSALE_EU).ToString + "','marketSummaryGraph');"
        htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">For Sale by End User (Avg)</a></td>", marketFile, marketFile_wHeader))

      Else

        tmpMarketGraphData = New marketGraphData

        tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.AV_FORSALE_EU
        tmpMarketGraphData.marketGraph_topTitle = "Aircraft For Sale by End User"

        tmpMarketGraphData.marketGraph_Y_title = "For Sale"

        For x As Integer = 0 To UBound(AV_End_User)
          If String.IsNullOrEmpty(tmpData) Then
            tmpData = AV_End_User(x).ToString
          Else
            tmpData += crmWebClient.Constants.cCommaDelim + AV_End_User(x).ToString
          End If
        Next

        tmpMarketGraphData.marketGraph_Y_data = tmpData
        tmpData = ""

        tmpMarketGraphData.marketGraph_X_title = "per Month"
        tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

        If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
          HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.AV_FORSALE_EU) - 1) = tmpMarketGraphData
        End If

        sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.AV_FORSALE_EU).ToString + "','marketSummaryGraph');"
        htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">For Sale by End User</a></td>", marketFile, marketFile_wHeader))

      End If

      htmlOut.Append(Calculate_Available_Summaries(ColumnSet, AV_End_User, AV_End_User_Count, YearMonth_Count, dtEndDate, True, False, False, False, False))

      htmlOut.Append(WriteLineToBoth("</tr>", marketFile, marketFile_wHeader))

      ' print total end user exclusive
      htmlOut.Append(WriteLineToBoth("<tr>", marketFile, marketFile_wHeader))

      If HttpContext.Current.Session.Item("marketTimeScale").ToString.ToLower.Contains("years") Or HttpContext.Current.Session.Item("marketTimeScale").ToString.ToLower.Contains("quarters") Then

        tmpMarketGraphData = New marketGraphData

        tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.AV_FORSALE_BKR
        tmpMarketGraphData.marketGraph_topTitle = "Aircraft For Sale by Exclusive Broker"

        tmpMarketGraphData.marketGraph_Y_title = "Average For Sale"

        For x As Integer = 0 To UBound(AV_End_User_Exc)
          If String.IsNullOrEmpty(tmpData) Then
            tmpData = FormatNumber((AV_End_User_Exc(x) / YearMonth_Count(x)), 0, False, False, False).ToString
          Else
            tmpData += crmWebClient.Constants.cCommaDelim + FormatNumber((AV_End_User_Exc(x) / YearMonth_Count(x)), 0, False, False, False).ToString
          End If
        Next

        tmpMarketGraphData.marketGraph_Y_data = tmpData
        tmpData = ""

        tmpMarketGraphData.marketGraph_X_title = "per Year"
        tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

        If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
          HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.AV_FORSALE_BKR) - 1) = tmpMarketGraphData
        End If

        sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.AV_FORSALE_BKR).ToString + "','marketSummaryGraph');"
        htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">For Sale w/Exclusive Broker (Avg)</a></td>", marketFile, marketFile_wHeader))

      Else

        tmpMarketGraphData = New marketGraphData

        tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.AV_FORSALE_BKR
        tmpMarketGraphData.marketGraph_topTitle = "Aircraft For Sale by Exclusive Broker"

        tmpMarketGraphData.marketGraph_Y_title = "For Sale"

        For x As Integer = 0 To UBound(AV_End_User_Exc)
          If String.IsNullOrEmpty(tmpData) Then
            tmpData = AV_End_User_Exc(x).ToString
          Else
            tmpData += crmWebClient.Constants.cCommaDelim + AV_End_User_Exc(x).ToString
          End If
        Next

        tmpMarketGraphData.marketGraph_Y_data = tmpData
        tmpData = ""

        tmpMarketGraphData.marketGraph_X_title = "per Month"
        tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

        If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
          HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.AV_FORSALE_BKR) - 1) = tmpMarketGraphData
        End If

        sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.AV_FORSALE_BKR).ToString + "','marketSummaryGraph');"
        htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">For Sale w/Exclusive Broker</a></td>", marketFile, marketFile_wHeader))

      End If

      htmlOut.Append(Calculate_Available_Summaries(ColumnSet, AV_End_User_Exc, AV_End_User_Exc_Count, YearMonth_Count, dtEndDate, True, False, False, False, False))

      htmlOut.Append(WriteLineToBoth("</tr>", marketFile, marketFile_wHeader))

      ' print dealer aircraft for sale
      htmlOut.Append(WriteLineToBoth("<tr>", marketFile, marketFile_wHeader))

      If HttpContext.Current.Session.Item("marketTimeScale").ToString.ToLower.Contains("years") Or HttpContext.Current.Session.Item("marketTimeScale").ToString.ToLower.Contains("quarters") Then

        tmpMarketGraphData = New marketGraphData

        tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.AV_FORSALE_DLR
        tmpMarketGraphData.marketGraph_topTitle = "Aircraft For Sale by Dealer"

        tmpMarketGraphData.marketGraph_Y_title = "Average For Sale"

        For x As Integer = 0 To UBound(AV_Dealer)
          If String.IsNullOrEmpty(tmpData) Then
            tmpData = FormatNumber((AV_Dealer(x) / YearMonth_Count(x)), 0, False, False, False).ToString
          Else
            tmpData += crmWebClient.Constants.cCommaDelim + FormatNumber((AV_Dealer(x) / YearMonth_Count(x)), 0, False, False, False).ToString
          End If
        Next

        tmpMarketGraphData.marketGraph_Y_data = tmpData
        tmpData = ""

        tmpMarketGraphData.marketGraph_X_title = "per Year"
        tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

        If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
          HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.AV_FORSALE_DLR) - 1) = tmpMarketGraphData
        End If

        sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.AV_FORSALE_DLR).ToString + "','marketSummaryGraph');"
        htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">For Sale w/Dealer (Avg)</a></td>", marketFile, marketFile_wHeader))

      Else

        tmpMarketGraphData = New marketGraphData

        tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.AV_FORSALE_DLR
        tmpMarketGraphData.marketGraph_topTitle = "Aircraft For Sale by Dealer"

        tmpMarketGraphData.marketGraph_Y_title = "For Sale"

        For x As Integer = 0 To UBound(AV_Dealer)
          If String.IsNullOrEmpty(tmpData) Then
            tmpData = AV_Dealer(x).ToString
          Else
            tmpData += crmWebClient.Constants.cCommaDelim + AV_Dealer(x).ToString
          End If
        Next

        tmpMarketGraphData.marketGraph_Y_data = tmpData
        tmpData = ""

        tmpMarketGraphData.marketGraph_X_title = "per Month"
        tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

        If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
          HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.AV_FORSALE_DLR) - 1) = tmpMarketGraphData
        End If

        sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.AV_FORSALE_DLR).ToString + "','marketSummaryGraph');"
        htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">For Sale w/Dealer</a></td>", marketFile, marketFile_wHeader))

      End If

      htmlOut.Append(Calculate_Available_Summaries(ColumnSet, AV_Dealer, AV_Dealer_Count, YearMonth_Count, dtEndDate, True, False, False, False, False))

      htmlOut.Append(WriteLineToBoth("</tr>", marketFile, marketFile_wHeader))
      htmlOut.Append("<tr height=""4""><td colspan=""" + sColSpan + """></td></tr>")

      ' print domestic aircraft for sale
      htmlOut.Append(WriteLineToBoth("<tr>", marketFile, marketFile_wHeader))

      If HttpContext.Current.Session.Item("marketTimeScale").ToString.ToLower.Contains("years") Or HttpContext.Current.Session.Item("marketTimeScale").ToString.ToLower.Contains("quarters") Then

        tmpMarketGraphData = New marketGraphData

        tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.AV_FORSALE_DOM
        tmpMarketGraphData.marketGraph_topTitle = "Domestic Aircraft For Sale"

        tmpMarketGraphData.marketGraph_Y_title = "Average For Sale"

        For x As Integer = 0 To UBound(AV_Domestic)
          If String.IsNullOrEmpty(tmpData) Then
            tmpData = FormatNumber((AV_Domestic(x) / YearMonth_Count(x)), 0, False, False, False).ToString
          Else
            tmpData += crmWebClient.Constants.cCommaDelim + FormatNumber((AV_Domestic(x) / YearMonth_Count(x)), 0, False, False, False).ToString
          End If
        Next

        tmpMarketGraphData.marketGraph_Y_data = tmpData
        tmpData = ""

        tmpMarketGraphData.marketGraph_X_title = "per Year"
        tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

        If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
          HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.AV_FORSALE_DOM) - 1) = tmpMarketGraphData
        End If

        sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.AV_FORSALE_DOM).ToString + "','marketSummaryGraph');"
        htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">Domestic (Avg)</a></td>", marketFile, marketFile_wHeader))

      Else

        tmpMarketGraphData = New marketGraphData

        tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.AV_FORSALE_DOM
        tmpMarketGraphData.marketGraph_topTitle = "Domestic Aircraft For Sale"

        tmpMarketGraphData.marketGraph_Y_title = "For Sale"

        For x As Integer = 0 To UBound(AV_Domestic)
          If String.IsNullOrEmpty(tmpData) Then
            tmpData = AV_Domestic(x).ToString
          Else
            tmpData += crmWebClient.Constants.cCommaDelim + AV_Domestic(x).ToString
          End If
        Next

        tmpMarketGraphData.marketGraph_Y_data = tmpData
        tmpData = ""

        tmpMarketGraphData.marketGraph_X_title = "per Month"
        tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

        If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
          HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.AV_FORSALE_DOM) - 1) = tmpMarketGraphData
        End If

        sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.AV_FORSALE_DOM).ToString + "','marketSummaryGraph');"
        htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">Domestic</a></td>", marketFile, marketFile_wHeader))

      End If

      htmlOut.Append(Calculate_Available_Summaries(ColumnSet, AV_Domestic, AV_Domestic_Count, YearMonth_Count, dtEndDate, True, False, False, False, False))

      htmlOut.Append(WriteLineToBoth("</tr>", marketFile, marketFile_wHeader))

      ' PRINT INTERNATIONAL FOR SALE
      htmlOut.Append(WriteLineToBoth("<tr>", marketFile, marketFile_wHeader))

      If HttpContext.Current.Session.Item("marketTimeScale").ToString.ToLower.Contains("years") Or HttpContext.Current.Session.Item("marketTimeScale").ToString.ToLower.Contains("quarters") Then

        tmpMarketGraphData = New marketGraphData

        tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.AV_FORSALE_FOR
        tmpMarketGraphData.marketGraph_topTitle = "International Aircraft For Sale"

        tmpMarketGraphData.marketGraph_Y_title = "Average For Sale"

        For x As Integer = 0 To UBound(AV_International)
          If String.IsNullOrEmpty(tmpData) Then
            tmpData = FormatNumber((AV_International(x) / YearMonth_Count(x)), 0, False, False, False).ToString
          Else
            tmpData += crmWebClient.Constants.cCommaDelim + FormatNumber((AV_International(x) / YearMonth_Count(x)), 0, False, False, False).ToString
          End If
        Next

        tmpMarketGraphData.marketGraph_Y_data = tmpData
        tmpData = ""

        tmpMarketGraphData.marketGraph_X_title = "per Year"
        tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

        If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
          HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.AV_FORSALE_FOR) - 1) = tmpMarketGraphData
        End If

        sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.AV_FORSALE_FOR).ToString + "','marketSummaryGraph');"
        htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">International (Avg)</a></td>", marketFile, marketFile_wHeader))

      Else

        tmpMarketGraphData = New marketGraphData

        tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.AV_FORSALE_FOR
        tmpMarketGraphData.marketGraph_topTitle = "International Aircraft For Sale"

        tmpMarketGraphData.marketGraph_Y_title = "For Sale"

        For x As Integer = 0 To UBound(AV_International)
          If String.IsNullOrEmpty(tmpData) Then
            tmpData = AV_International(x).ToString
          Else
            tmpData += crmWebClient.Constants.cCommaDelim + AV_International(x).ToString
          End If
        Next

        tmpMarketGraphData.marketGraph_Y_data = tmpData
        tmpData = ""

        tmpMarketGraphData.marketGraph_X_title = "per Month"
        tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

        If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
          HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.AV_FORSALE_FOR) - 1) = tmpMarketGraphData
        End If

        sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.AV_FORSALE_FOR).ToString + "','marketSummaryGraph');"
        htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">International</a></td>", marketFile, marketFile_wHeader))

      End If

      htmlOut.Append(Calculate_Available_Summaries(ColumnSet, AV_International, AV_International_Count, YearMonth_Count, dtEndDate, True, False, False, False, False))

      htmlOut.Append(WriteLineToBoth("</tr>", marketFile, marketFile_wHeader))
      htmlOut.Append("<tr height=""4""><td colspan=""" + sColSpan + """></td></tr>")

      ' print asking average
      htmlOut.Append(WriteLineToBoth("<tr>", marketFile, marketFile_wHeader))

      tmpMarketGraphData = New marketGraphData

      tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.AV_AVG_ASKING
      tmpMarketGraphData.marketGraph_topTitle = "Average Asking Price"

      tmpMarketGraphData.marketGraph_Y_title = "Asking Price in Dollars"

      For x As Integer = 0 To UBound(AV_Asking_Price_Total)
        If String.IsNullOrEmpty(tmpData) Then
          tmpData = FormatNumber((AV_Asking_Price_Total(x) / AV_Asking_Price_Count(x)), 0, False, False, False).ToString
        Else
          tmpData += crmWebClient.Constants.cCommaDelim + FormatNumber((AV_Asking_Price_Total(x) / AV_Asking_Price_Count(x)), 0, False, False, False).ToString
        End If
      Next

      tmpMarketGraphData.marketGraph_Y_data = tmpData
      tmpData = ""

      tmpMarketGraphData.marketGraph_X_title = "per Month"
      tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

      If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
        HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.AV_AVG_ASKING) - 1) = tmpMarketGraphData
      End If

      sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.AV_AVG_ASKING).ToString + "','marketSummaryGraph');"
      htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">Average&nbsp;Asking&nbsp;Price</a></td>", marketFile, marketFile_wHeader))

      htmlOut.Append(Calculate_Available_Summaries(ColumnSet, AV_Asking_Price_Total, AV_Asking_Price_Count, YearMonth_Count, dtEndDate, False, True, True, True, False))

      htmlOut.Append(WriteLineToBoth("</tr>", marketFile, marketFile_wHeader))

      ' print asking high
      htmlOut.Append(WriteLineToBoth("<tr>", marketFile, marketFile_wHeader))

      tmpMarketGraphData = New marketGraphData

      tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.AV_HIGH_ASKING
      tmpMarketGraphData.marketGraph_topTitle = "High Asking Price"

      tmpMarketGraphData.marketGraph_Y_title = "Asking Price in Dollars"

      For x As Integer = 0 To UBound(AV_Asking_High)
        If String.IsNullOrEmpty(tmpData) Then
          tmpData = AV_Asking_High(x).ToString
        Else
          tmpData += crmWebClient.Constants.cCommaDelim + AV_Asking_High(x).ToString
        End If
      Next

      tmpMarketGraphData.marketGraph_Y_data = tmpData
      tmpData = ""

      tmpMarketGraphData.marketGraph_X_title = "per Month"
      tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

      If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
        HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.AV_HIGH_ASKING) - 1) = tmpMarketGraphData
      End If

      sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.AV_HIGH_ASKING).ToString + "','marketSummaryGraph');"
      htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">High&nbsp;Asking&nbsp;Price</a></td>", marketFile, marketFile_wHeader))

      htmlOut.Append(Calculate_Available_Summaries(ColumnSet, AV_Asking_High, Nothing, YearMonth_Count, dtEndDate, False, False, False, True, False))

      htmlOut.Append(WriteLineToBoth("</tr>", marketFile, marketFile_wHeader))

      ' print asking low
      htmlOut.Append(WriteLineToBoth("<tr>", marketFile, marketFile_wHeader))

      tmpMarketGraphData = New marketGraphData

      tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.AV_LOW_ASKING
      tmpMarketGraphData.marketGraph_topTitle = "Low Asking Price"

      tmpMarketGraphData.marketGraph_Y_title = "Asking Price in Dollars"

      For x As Integer = 0 To UBound(AV_Asking_Low)
        If String.IsNullOrEmpty(tmpData) Then
          tmpData = AV_Asking_Low(x).ToString
        Else
          tmpData += crmWebClient.Constants.cCommaDelim + AV_Asking_Low(x).ToString
        End If
      Next

      tmpMarketGraphData.marketGraph_Y_data = tmpData
      tmpData = ""

      tmpMarketGraphData.marketGraph_X_title = "per Month"
      tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

      If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
        HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.AV_LOW_ASKING) - 1) = tmpMarketGraphData
      End If

      sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.AV_LOW_ASKING).ToString + "','marketSummaryGraph');"
      htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">Low&nbsp;Asking&nbsp;Price</a></td>", marketFile, marketFile_wHeader))

      htmlOut.Append(Calculate_Available_Summaries(ColumnSet, AV_Asking_Low, Nothing, YearMonth_Count, dtEndDate, False, False, False, True, False))

      htmlOut.Append(WriteLineToBoth("</tr>", marketFile, marketFile_wHeader))

      ' print make offer
      htmlOut.Append(WriteLineToBoth("<tr>", marketFile, marketFile_wHeader))

      If HttpContext.Current.Session.Item("marketTimeScale").ToString.ToLower.Contains("years") Or HttpContext.Current.Session.Item("marketTimeScale").ToString.ToLower.Contains("quarters") Then

        tmpMarketGraphData = New marketGraphData

        tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.AV_MAKE_OFFER
        tmpMarketGraphData.marketGraph_topTitle = "Make Offer"

        tmpMarketGraphData.marketGraph_Y_title = "Average Make Offers"

        For x As Integer = 0 To UBound(AV_Asking_Make_Offer)
          If String.IsNullOrEmpty(tmpData) Then
            tmpData = FormatNumber((AV_Asking_Make_Offer(x) / YearMonth_Count(x)), 0, False, False, False).ToString
          Else
            tmpData += crmWebClient.Constants.cCommaDelim + FormatNumber((AV_Asking_Make_Offer(x) / YearMonth_Count(x)), 0, False, False, False).ToString
          End If
        Next

        tmpMarketGraphData.marketGraph_Y_data = tmpData
        tmpData = ""

        tmpMarketGraphData.marketGraph_X_title = "per Year"
        tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

        If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
          HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.AV_MAKE_OFFER) - 1) = tmpMarketGraphData
        End If

        sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.AV_MAKE_OFFER).ToString + "','marketSummaryGraph');"
        htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">Make Offer / Inquire (Avg)</a></td>", marketFile, marketFile_wHeader))

      Else

        tmpMarketGraphData = New marketGraphData

        tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.AV_MAKE_OFFER
        tmpMarketGraphData.marketGraph_topTitle = "Make Offer"

        tmpMarketGraphData.marketGraph_Y_title = "Make Offers"

        For x As Integer = 0 To UBound(AV_Asking_Make_Offer)
          If String.IsNullOrEmpty(tmpData) Then
            tmpData = AV_Asking_Make_Offer(x).ToString
          Else
            tmpData += crmWebClient.Constants.cCommaDelim + AV_Asking_Make_Offer(x).ToString
          End If
        Next

        tmpMarketGraphData.marketGraph_Y_data = tmpData
        tmpData = ""

        tmpMarketGraphData.marketGraph_X_title = "per Month"
        tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

        If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
          HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.AV_MAKE_OFFER) - 1) = tmpMarketGraphData
        End If

        sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.AV_MAKE_OFFER).ToString + "','marketSummaryGraph');"
        htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">Make Offer / Inquire</a></td>", marketFile, marketFile_wHeader))

      End If

      htmlOut.Append(Calculate_Available_Summaries(ColumnSet, AV_Asking_Make_Offer, AV_Asking_Make_Offer_Count, YearMonth_Count, dtEndDate, True, False, False, False, False))

      htmlOut.Append(WriteLineToBoth("</tr>", marketFile, marketFile_wHeader))

      htmlOut.Append("<tr height=""4""><td colspan=""" + sColSpan + """></td></tr>")

      ' print average year
      htmlOut.Append(WriteLineToBoth("<tr>", marketFile, marketFile_wHeader))

      tmpMarketGraphData = New marketGraphData

      tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.AV_AVG_YEAR
      tmpMarketGraphData.marketGraph_topTitle = "Average Year"

      tmpMarketGraphData.marketGraph_Y_title = "Average Year"

      For x As Integer = 0 To UBound(AV_Avg_Year_Total)
        If String.IsNullOrEmpty(tmpData) Then
          tmpData = FormatNumber((AV_Avg_Year_Total(x) / AV_Avg_Year_Count(x)), 0, False, False, False).ToString
        Else
          tmpData += crmWebClient.Constants.cCommaDelim + FormatNumber((AV_Avg_Year_Total(x) / AV_Avg_Year_Count(x)), 0, False, False, False).ToString
        End If
      Next

      tmpMarketGraphData.marketGraph_Y_data = tmpData
      tmpData = ""

      tmpMarketGraphData.marketGraph_X_title = "per Month"
      tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

      If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
        HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.AV_AVG_YEAR) - 1) = tmpMarketGraphData
      End If

      sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.AV_AVG_YEAR).ToString + "','marketSummaryGraph');"
      htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">Average Year</a></td>", marketFile, marketFile_wHeader))

      htmlOut.Append(Calculate_Available_Summaries(ColumnSet, AV_Avg_Year_Total, AV_Avg_Year_Count, YearMonth_Count, dtEndDate, False, True, False, False, True))

      htmlOut.Append(WriteLineToBoth("</tr>", marketFile, marketFile_wHeader))


      htmlOut.Append("<tr height=""4""><td colspan=""" + sColSpan + """></td></tr>")

      ' print average year
      htmlOut.Append(WriteLineToBoth("<tr>", marketFile, marketFile_wHeader))


      tmpMarketGraphData = New marketGraphData

      tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.AV_AVG_YEAR
      tmpMarketGraphData.marketGraph_topTitle = "Average Days on Market"

      tmpMarketGraphData.marketGraph_Y_title = "Average Days on Market"

      For x As Integer = 0 To UBound(AV_DOM)
        If String.IsNullOrEmpty(tmpData) Then
          tmpData = FormatNumber((AV_DOM(x) / AV_DOM_COUNT(x)), 0, False, False, False).ToString
        Else
          tmpData += crmWebClient.Constants.cCommaDelim + FormatNumber((AV_DOM(x) / AV_DOM_COUNT(x)), 0, False, False, False).ToString
        End If
      Next

      tmpMarketGraphData.marketGraph_Y_data = tmpData
      tmpData = ""

      tmpMarketGraphData.marketGraph_X_title = "per Month"
      tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

      If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
        HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.AV_AVG_YEAR) - 1) = tmpMarketGraphData
      End If

      sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.AV_AVG_YEAR).ToString + "','marketSummaryGraph');"
      htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">Average Days on Market</a></td>", marketFile, marketFile_wHeader))

      htmlOut.Append(Calculate_Available_Summaries(ColumnSet, AV_DOM, AV_DOM_COUNT, YearMonth_Count, dtEndDate, False, True, False, False, True))

      htmlOut.Append(WriteLineToBoth("</tr>", marketFile, marketFile_wHeader))



      ' print average airframe total time
      htmlOut.Append(WriteLineToBoth("<tr>", marketFile, marketFile_wHeader))

      tmpMarketGraphData = New marketGraphData

      tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.AV_AVG_AFTT
      tmpMarketGraphData.marketGraph_topTitle = "Average Airframe Total Time"

      tmpMarketGraphData.marketGraph_Y_title = "Airframe TT"

      For x As Integer = 0 To UBound(AV_Avg_Airframe_TT_Total)
        If String.IsNullOrEmpty(tmpData) Then
          tmpData = FormatNumber((AV_Avg_Airframe_TT_Total(x) / AV_Avg_Airframe_TT_Count(x)), 0, False, False, False).ToString
        Else
          tmpData += crmWebClient.Constants.cCommaDelim + FormatNumber((AV_Avg_Airframe_TT_Total(x) / AV_Avg_Airframe_TT_Count(x)), 0, False, False, False).ToString
        End If
      Next

      tmpMarketGraphData.marketGraph_Y_data = tmpData
      tmpData = ""

      tmpMarketGraphData.marketGraph_X_title = "per Month"
      tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

      If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
        HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.AV_AVG_AFTT) - 1) = tmpMarketGraphData
      End If

      sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.AV_AVG_AFTT).ToString + "','marketSummaryGraph');"
      htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">Average Airframe TT</a></td>", marketFile, marketFile_wHeader))

      htmlOut.Append(Calculate_Available_Summaries(ColumnSet, AV_Avg_Airframe_TT_Total, AV_Avg_Airframe_TT_Count, YearMonth_Count, dtEndDate, False, True, True, False, False))

      htmlOut.Append(WriteLineToBoth("</tr>", marketFile, marketFile_wHeader))

      ' print average engine total time
      htmlOut.Append(WriteLineToBoth("<tr>", marketFile, marketFile_wHeader))

      tmpMarketGraphData = New marketGraphData

      tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.AV_AVG_ENTT
      tmpMarketGraphData.marketGraph_topTitle = "Average Engine Total Time"

      tmpMarketGraphData.marketGraph_Y_title = "Engine TT"

      For x As Integer = 0 To UBound(AV_Avg_Engine_TT_Total)
        If String.IsNullOrEmpty(tmpData) Then
          tmpData = FormatNumber((AV_Avg_Engine_TT_Total(x) / AV_Avg_Engine_TT_Count(x)), 0, False, False, False).ToString
        Else
          tmpData += crmWebClient.Constants.cCommaDelim + FormatNumber((AV_Avg_Engine_TT_Total(x) / AV_Avg_Engine_TT_Count(x)), 0, False, False, False).ToString
        End If
      Next

      tmpMarketGraphData.marketGraph_Y_data = tmpData
      tmpData = ""

      tmpMarketGraphData.marketGraph_X_title = "per Month"
      tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

      If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
        HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.AV_AVG_ENTT) - 1) = tmpMarketGraphData
      End If

      sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.AV_AVG_ENTT).ToString + "','marketSummaryGraph');"
      htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">Average Engine TT</a></td>", marketFile, marketFile_wHeader))

      htmlOut.Append(Calculate_Available_Summaries(ColumnSet, AV_Avg_Engine_TT_Total, AV_Avg_Engine_TT_Count, YearMonth_Count, dtEndDate, False, True, True, False, False))

      htmlOut.Append(WriteLineToBoth("</tr>", marketFile, marketFile_wHeader))
      htmlOut.Append("<tr height=""4""><td colspan=""" + sColSpan + """></td></tr>")

      ' print new to market
      htmlOut.Append(WriteLineToBoth("<tr>", marketFile, marketFile_wHeader))

      If HttpContext.Current.Session.Item("marketTimeScale").ToString.ToLower.Contains("years") Or HttpContext.Current.Session.Item("marketTimeScale").ToString.ToLower.Contains("quarters") Then

        tmpMarketGraphData = New marketGraphData

        tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.AV_NEW_TO_MARKET
        tmpMarketGraphData.marketGraph_topTitle = "New to the market"

        tmpMarketGraphData.marketGraph_Y_title = "Aircraft new to market"

        For x As Integer = 0 To UBound(AV_New_To_Market) 'AV_New_To_Market_Count YearMonth_Count
          If String.IsNullOrEmpty(tmpData) Then
            tmpData = FormatNumber((AV_New_To_Market(x) / AV_New_To_Market_Count(x)), 0, False, False, False).ToString
          Else
            tmpData += crmWebClient.Constants.cCommaDelim + FormatNumber((AV_New_To_Market(x) / AV_New_To_Market_Count(x)), 0, False, False, False).ToString
          End If
        Next

        tmpMarketGraphData.marketGraph_Y_data = tmpData
        tmpData = ""

        tmpMarketGraphData.marketGraph_X_title = "per Year"
        tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

        If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
          HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.AV_NEW_TO_MARKET) - 1) = tmpMarketGraphData
        End If

        sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.AV_NEW_TO_MARKET).ToString + "','marketSummaryGraph');"

        Dim tmpString As String = "<td nowrap=""nowrap"" align=""left"" valign=""middle"">"
        tmpString += "<div title = ""HELP NOTE: Refers to the aircrcraft that are new to the market. Represents the number of aircraft that are for sale this period that were not for sale during the previous reporting period."">"
        tmpString += "<a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">Aircraft&nbsp;New&nbsp;To&nbsp;Market&nbsp;(Avg)</a></div></td>"
        htmlOut.Append(WriteLineToBoth(tmpString, marketFile, marketFile_wHeader))

      Else

        tmpMarketGraphData = New marketGraphData

        tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.AV_NEW_TO_MARKET
        tmpMarketGraphData.marketGraph_topTitle = "New to the market"

        tmpMarketGraphData.marketGraph_Y_title = "Aircraft new to market"

        For x As Integer = 0 To UBound(AV_New_To_Market)
          If String.IsNullOrEmpty(tmpData) Then
            tmpData = AV_New_To_Market(x).ToString
          Else
            tmpData += crmWebClient.Constants.cCommaDelim + AV_New_To_Market(x).ToString
          End If
        Next

        tmpMarketGraphData.marketGraph_Y_data = tmpData
        tmpData = ""

        tmpMarketGraphData.marketGraph_X_title = "per Month"
        tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

        If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
          HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.AV_NEW_TO_MARKET) - 1) = tmpMarketGraphData
        End If

        sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.AV_NEW_TO_MARKET).ToString + "','marketSummaryGraph');"

        Dim tmpString As String = "<td nowrap=""nowrap"" align=""left"" valign=""middle"">"
        tmpString += "<div title = ""HELP NOTE: Refers to the aircrcraft that are new to the market. Represents the number of aircraft that are for sale this period that were not for sale during the previous reporting period."">"
        tmpString += "<a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">Aircraft&nbsp;New&nbsp;To&nbsp;Market</a></div></td>"
        htmlOut.Append(WriteLineToBoth(tmpString, marketFile, marketFile_wHeader))

      End If

      htmlOut.Append(Calculate_Available_Summaries(ColumnSet, AV_New_To_Market, AV_New_To_Market_Count, YearMonth_Count, dtEndDate, True, False, False, False, False))

      htmlOut.Append(WriteLineToBoth("</tr>", marketFile, marketFile_wHeader))

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Print_Available_Summaries()" + ex.Message
    End Try

    Return htmlOut.ToString

  End Function

  Public Function Calculate_Available_Summaries(ByVal ColumnSet() As String, ByRef arrItemToCount() As Double, ByRef arrItemTotalCount() As Double, ByRef arrYearMonthCount() As Double, ByVal dtEndDate As Date, ByVal bUseTotal As Boolean, ByVal bUseAverage As Boolean, ByVal bShowPercentChg As Boolean, ByVal bShowDollarSgn As Boolean, ByVal bDontGroupDigits As Boolean, Optional ByVal bShowPrecision As Boolean = False, Optional ByVal is_percent_for_sale As Boolean = False)

    Dim htmlOut = New StringBuilder()

    Dim nFirstNum As Double = 0.0
    Dim nSecondNum As Double = 0.0
    Dim nAverageValue As Double = 0.0

    Dim nTotalItem As Double = 0.0
    Dim nTotalItemCount As Double = 0.0
    Dim nCalcPercent As Double = 0.0
    Dim nLastValue As Double = 0.0
    Dim nCurrentValue As Double = 0.0
    Dim nTotAvg As Double = 0.0

    Dim columnSetMonth As Integer = 0
    Dim columnSetYear As Integer = 0
    Dim columnQuarterMonth As String = ""

    Try

      For nIndex As Integer = 0 To UBound(ColumnSet)

        Select Case HttpContext.Current.Session.Item("marketTimeScale").ToString.ToLower
          Case "years"
            columnSetMonth = CInt(1)
            columnSetYear = CInt(Right(ColumnSet(nIndex), 4))
          Case "quarters"
            columnQuarterMonth = Left(ColumnSet(nIndex), InStr(1, ColumnSet(nIndex), "/") - 1)
            columnSetYear = CInt(Right(ColumnSet(nIndex), 4))
          Case Else
            columnSetMonth = CInt(Left(ColumnSet(nIndex), InStr(1, ColumnSet(nIndex), "/") - 1))
            columnSetYear = CInt(Right(ColumnSet(nIndex), 4))
        End Select

        If bUseTotal And Not IsNothing(arrItemTotalCount) Then

          If arrItemTotalCount(nIndex) > 0 Then

            If is_percent_for_sale = True And (HttpContext.Current.Session.Item("marketTimeScale").ToString.ToLower.Contains("quarters") Or HttpContext.Current.Session.Item("marketTimeScale").ToString.ToLower.Contains("year")) Then
              htmlOut.Append(WriteLineToBoth(WriteColumn((arrItemToCount(nIndex)), bShowDollarSgn, bDontGroupDigits, bShowPrecision), marketFile, marketFile_wHeader))

              If nIndex <> UBound(ColumnSet) Then
                nTotalItem += (arrItemToCount(nIndex))
                nTotalItemCount += arrItemTotalCount(nIndex)
              End If

            Else
              htmlOut.Append(WriteLineToBoth(WriteColumn((arrItemToCount(nIndex) / arrYearMonthCount(nIndex)), bShowDollarSgn, bDontGroupDigits, bShowPrecision), marketFile, marketFile_wHeader))

              If nIndex <> UBound(ColumnSet) Then
                nTotalItem += (arrItemToCount(nIndex) / arrYearMonthCount(nIndex))
                nTotalItemCount += arrItemTotalCount(nIndex)
              End If

            End If
            ' add dollar sign to prices

          Else
            ' if it is the last column don't print the 0, print nya only if
            ' the columnset(nindex) = year(dtenddate) and month(dtenddate)     

            If nIndex = UBound(ColumnSet) Then
              If Year(dtEndDate) = columnSetYear Then ' match year

                If Not HttpContext.Current.Session.Item("marketTimeScale").ToString.ToLower.Contains("quarters") Then

                  If Month(dtEndDate) = columnSetMonth Then ' match month
                    htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">NYA</td>", marketFile, marketFile_wHeader))
                  Else
                    htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">0</td>", marketFile, marketFile_wHeader))
                  End If

                Else

                  If Get_Quarter_For_Month_Server(Month(dtEndDate)) = columnQuarterMonth Then ' match quarter
                    htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">NYA</td>", marketFile, marketFile_wHeader))
                  Else
                    htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">0</td>", marketFile, marketFile_wHeader))
                  End If
                End If

              Else
                htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">0</td>", marketFile, marketFile_wHeader))
              End If
            Else
              htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">0</td>", marketFile, marketFile_wHeader))
            End If

          End If ' arrItemTotalCount(nIndex) > 0
        End If ' bUseTotal and Not IsNothing(arrItemTotalCount)  

        If Not bUseTotal And Not bUseAverage And IsNothing(arrItemTotalCount) Then

          If arrItemToCount(nIndex) > 0 Then

            If bShowDollarSgn Then
              htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">" + FormatCurrency(arrItemToCount(nIndex), 0, False, False, True) + "</td>", marketFile, marketFile_wHeader))
            ElseIf bDontGroupDigits Then
              htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">" + FormatNumber(arrItemToCount(nIndex), 0, False, False, False) + "</td>", marketFile, marketFile_wHeader))
            Else
              htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">" + FormatNumber(arrItemToCount(nIndex), 0, True, False, True) + "</td>", marketFile, marketFile_wHeader))
            End If

            If nIndex <> UBound(ColumnSet) Then
              nTotalItem += arrItemToCount(nIndex)
              nTotalItemCount += 1
            End If

          Else
            ' if it is the last column don't print the 0, print nya only if
            ' the columnset(nindex) = year(dtenddate) and month(dtenddate)     
            If nIndex = UBound(ColumnSet) Then
              If Year(dtEndDate) = columnSetYear Then ' match year
                If Not HttpContext.Current.Session.Item("marketTimeScale").ToString.ToLower.Contains("quarters") Then
                  If Month(dtEndDate) = columnSetMonth Then ' match month
                    htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">NYA</td>", marketFile, marketFile_wHeader))
                  Else
                    htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">0</td>", marketFile, marketFile_wHeader))
                  End If
                Else
                  If Get_Quarter_For_Month_Server(Month(dtEndDate)) = columnQuarterMonth Then ' match quarter
                    htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">NYA</td>", marketFile, marketFile_wHeader))
                  Else
                    htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">0</td>", marketFile, marketFile_wHeader))
                  End If
                End If
              Else
                htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">0</td>", marketFile, marketFile_wHeader))
              End If
            Else
              htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">0</td>", marketFile, marketFile_wHeader))
            End If ' nIndex = UBound(ColumnSet)

          End If ' arrItemToCount(nIndex) > 0

        End If ' Not bUseTotal and Not bUseAverage and IsNull(arrItemTotalCount)

        If bUseAverage And Not IsNothing(arrItemTotalCount) Then

          If arrItemTotalCount(nIndex) > 0 Then

            nAverageValue = (arrItemToCount(nIndex) / arrItemTotalCount(nIndex))

            If nIndex <> UBound(ColumnSet) Then
              nTotalItem += arrItemToCount(nIndex)
              nTotalItemCount += arrItemTotalCount(nIndex)
            End If
          Else
            nAverageValue = 0
          End If ' arrItemToCount(nIndex) > 0

          If nAverageValue > 0 Then

            If Not bShowPercentChg Then
              htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">" + FormatNumber(nAverageValue, 0, True, False, False) + "</td>", marketFile, marketFile_wHeader))
            ElseIf bShowDollarSgn Then
              htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">" + FormatCurrency(nAverageValue, 0, False, False, True) + "</td>", marketFile, marketFile_wHeader))
            ElseIf bDontGroupDigits Then
              htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">" + FormatNumber(nAverageValue, 0, False, False, False) + "</td>", marketFile, marketFile_wHeader))
            ElseIf bShowPercentChg Then
              htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">" + FormatNumber(nAverageValue, 0, True, False, True) + "</td>", marketFile, marketFile_wHeader))
            Else
              htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">" + FormatNumber(nAverageValue, 0, True, False, True) + "%</td>", marketFile, marketFile_wHeader))
            End If

          Else

            ' if it is the last column don't print the 0, print nya only if
            ' the columnset(nindex) = year(dtenddate) and month(dtenddate)     
            If nIndex = UBound(ColumnSet) Then

              If Year(dtEndDate) = columnSetYear Then ' match year
                If Not HttpContext.Current.Session.Item("marketTimeScale").ToString.ToLower.Contains("quarters") Then
                  If Month(dtEndDate) = columnSetMonth Then ' match month
                    htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">NYA</td>", marketFile, marketFile_wHeader))
                  Else
                    htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">0</td>", marketFile, marketFile_wHeader))
                  End If
                Else
                  If Get_Quarter_For_Month_Server(Month(dtEndDate)) = columnQuarterMonth Then ' match quarter
                    htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">NYA</td>", marketFile, marketFile_wHeader))
                  Else
                    htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">0</td>", marketFile, marketFile_wHeader))
                  End If
                End If
              Else
                htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">0</td>", marketFile, marketFile_wHeader))
              End If
            Else
              htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">0</td>", marketFile, marketFile_wHeader))
            End If ' nIndex = UBound(ColumnSet)

          End If

        End If ' bUseAverage and Not IsNull(arrItemTotalCount)

        ' IF WE ARE NOT ON THE FIRST OR LAST COLUMN THEN DISPLAY THE PERCENT         
        If nIndex > 0 And nIndex <= UBound(ColumnSet) Then

          If bUseTotal Then

            If arrYearMonthCount(nIndex) > 0 Then
              ' Sombody forgot to catch division by zero errors

              If arrYearMonthCount(nIndex) > 0 Then
                nCurrentValue = (arrItemToCount(nIndex) / arrYearMonthCount(nIndex))
              Else
                nCurrentValue = arrItemToCount(nIndex)
              End If

              If arrYearMonthCount(nIndex - 1) > 0 Then
                nLastValue = (arrItemToCount(nIndex - 1) / arrYearMonthCount(nIndex - 1))
              Else
                nLastValue = arrItemToCount(nIndex - 1)
              End If

              If nCurrentValue > 0 Then
                If arrItemToCount(nIndex - 1) > 0 Then
                  nCalcPercent = ((nCurrentValue - nLastValue) / nLastValue * 100)
                ElseIf arrItemToCount(nIndex - 1) = 0 Then
                  nCurrentValue = 0
                Else
                  nCalcPercent = (nCurrentValue * 100)
                End If
              End If

            Else
              nCurrentValue = 0
              nLastValue = 0
              nCalcPercent = 0
            End If

            If bDontGroupDigits Then
              htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">" + IIf(nCurrentValue > 0, FormatNumber(nCalcPercent, 2, False, False, False) + "%", "NC") + "</td>", marketFile, marketFile_wHeader))
            Else
              htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">" + IIf(nCurrentValue > 0, FormatNumber(nCalcPercent, 2, True, False, True) + "%", "NC") + "</td>", marketFile, marketFile_wHeader))
            End If

          End If ' bUseTotal

          If Not bUseTotal And Not bUseAverage Then

            If arrItemToCount(nIndex - 1) > 0 Then
              nCalcPercent = ((arrItemToCount(nIndex) - arrItemToCount(nIndex - 1)) / arrItemToCount(nIndex - 1)) * 100
            Else
              nCalcPercent = arrItemToCount(nIndex) * 100
            End If

            If bDontGroupDigits Then
              htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">" + FormatNumber(nCalcPercent, 2, False, False, False) + "%</td>", marketFile, marketFile_wHeader))
            Else
              htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">" + FormatNumber(nCalcPercent, 2, True, False, True) + "%</td>", marketFile, marketFile_wHeader))
            End If

          End If ' Not bUseTotal and Not bUseAverage 

          If bUseAverage Then

            If Not bShowPercentChg Then
              htmlOut.Append(WriteLineToBoth("<td>&nbsp;</td>", marketFile, marketFile_wHeader))
            Else

              nFirstNum = 0
              nSecondNum = 0

              If arrItemToCount(nIndex - 1) > 0 And arrItemTotalCount(nIndex - 1) > 0 Then

                If arrItemToCount(nIndex) > 0 And arrItemTotalCount(nIndex) > 0 Then
                  nFirstNum = (arrItemToCount(nIndex) / arrItemTotalCount(nIndex))
                End If

                nSecondNum = (arrItemToCount(nIndex - 1) / arrItemTotalCount(nIndex - 1))

                If nFirstNum > 0 Then
                  nCalcPercent = ((nFirstNum - nSecondNum) / nSecondNum) * 100
                End If

              Else

                If arrItemToCount(nIndex) > 0 And arrItemTotalCount(nIndex) > 0 Then
                  nFirstNum = (arrItemToCount(nIndex) / arrItemTotalCount(nIndex))
                  nCalcPercent = nFirstNum * 100
                End If

              End If

              If bDontGroupDigits Then
                htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">" + IIf(nFirstNum > 0, FormatNumber(nCalcPercent, 2, False, False, False) + "%", "NC") + "</td>", marketFile, marketFile_wHeader))
              Else
                htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">" + IIf(nFirstNum > 0, FormatNumber(nCalcPercent, 2, True, False, True) + "%", "NC") + "</td>", marketFile, marketFile_wHeader))
              End If

            End If ' Not bShowPercentChg then

          End If ' bUseAverage 

        End If ' nIndex > 0 and nIndex <> UBound(ColumnSet) 

      Next

      If nTotalItemCount > 0 Then
        If bUseTotal Then
          nTotAvg = nTotalItem / CDbl(UBound(ColumnSet))
        Else
          nTotAvg = nTotalItem / nTotalItemCount
        End If
      Else
        nTotAvg = nTotalItem
      End If

      If nTotalItem > 0 Then

        If bUseTotal Then
          htmlOut.Append(WriteLineToBoth(WriteColumn(nTotAvg, bShowDollarSgn, bDontGroupDigits), marketFile, marketFile_wHeader))
        End If

        If Not bUseTotal And Not bUseAverage Then
          If bShowDollarSgn Then
            htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">" + FormatCurrency(nTotAvg, 0, False, False, True) + "</td>", marketFile, marketFile_wHeader))
          ElseIf bDontGroupDigits Then
            htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">" + FormatNumber(nTotAvg, 0, False, False, False) + "</td>", marketFile, marketFile_wHeader))
          Else
            htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">" + FormatNumber(nTotAvg, 0, False, False, True) + "</td>", marketFile, marketFile_wHeader))
          End If
        End If

        If bUseAverage Then
          If Not bShowPercentChg Then
            If bShowDollarSgn Then
              htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">" + FormatCurrency(nTotAvg, 0, False, False, True) + "</td>", marketFile, marketFile_wHeader))
            ElseIf bDontGroupDigits Then
              htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">" + FormatNumber(nTotAvg, 0, False, False, False) + "</td>", marketFile, marketFile_wHeader))
            Else
              htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">" + FormatNumber(nTotAvg, 0, False, False, True) + "</td>", marketFile, marketFile_wHeader))
            End If
          Else
            htmlOut.Append(WriteLineToBoth(WriteColumn(nTotAvg, bShowDollarSgn, bDontGroupDigits), marketFile, marketFile_wHeader))
          End If
        End If

      Else

        If UBound(ColumnSet) > 0 Then
          htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">0</td>", marketFile, marketFile_wHeader))
        Else
          htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""right"" valign=""middle"">NC</td>", marketFile, marketFile_wHeader))
        End If

      End If

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Calculate_Available_Summaries()" + ex.Message
    End Try

    Return htmlOut.ToString

  End Function

#End Region

#Region "retail_summary_functions"

  Public Function Store_Retail_Totals(ByRef adoRs As DataRow, _
                                ByVal nIndex As Integer, _
                                ByRef dLowSelling() As Double, _
                                ByRef dLowSelling_Count() As Double, _
                                ByRef dAvgSelling() As Double, _
                                ByRef dAvgSelling_Count() As Double, _
                                ByRef dHighSelling() As Double, _
                                ByRef dHighSelling_Count() As Double, _
                                ByRef nSpCount() As Double, _
                                ByRef nSpCount_Count() As Double)


    Dim dlow_selling_count As Double = 0.0
    Dim davg_selling_count As Double = 0.0
    Dim dhigh_selling_count As Double = 0.0
    Dim nsp_count_count As Double = 0.0

    Try

      If Not (IsDBNull(adoRs.Item("dLowSelling"))) Then
        dlow_selling_count = CDbl(adoRs.Item("dLowSelling").ToString)
      Else
        dlow_selling_count = 0.0
      End If

      If Not (IsDBNull(adoRs.Item("dAvgSelling"))) Then
        davg_selling_count = CDbl(adoRs.Item("dAvgSelling").ToString)
      Else
        davg_selling_count = 0.0
      End If

      If Not (IsDBNull(adoRs.Item("dHighSelling"))) Then
        dhigh_selling_count = CDbl(adoRs.Item("dHighSelling").ToString)
      Else
        dhigh_selling_count = 0.0
      End If

      If Not (IsDBNull(adoRs.Item("nSalePricecount"))) Then
        nsp_count_count = CDbl(adoRs.Item("nSalePricecount").ToString)
      Else
        nsp_count_count = 0.0
      End If

      If dlow_selling_count <> 0 Then
        dLowSelling(nIndex) += dlow_selling_count
        dLowSelling_Count(nIndex) += 1
      End If

      If davg_selling_count <> 0 Then
        dAvgSelling(nIndex) += davg_selling_count
        dAvgSelling_Count(nIndex) += 1
      End If

      If dhigh_selling_count <> 0 Then
        dHighSelling(nIndex) += dhigh_selling_count
        dHighSelling_Count(nIndex) += 1
      End If

      If nsp_count_count <> 0 Then
        nSpCount(nIndex) += nsp_count_count
        nSpCount_Count(nIndex) += 1
      End If

    Catch ex As Exception
      Return False
    End Try

    Return True

  End Function

  Public Function Print_Retail_Summaries(ByVal dtStartDate As Date, _
                                     ByVal dtEndDate As Date, _
                                     ByVal sHeaderString As String, _
                                     ByVal sColSpan As String, _
                                     ByVal ColumnSet() As String, _
                                     ByVal YearMonth_Count() As Double, _
                                     ByVal dLowSelling() As Double, _
                                     ByVal dLowSelling_Count() As Double, _
                                     ByVal dAvgSelling() As Double, _
                                     ByVal dAvgSelling_Count() As Double, _
                                     ByVal dHighSelling() As Double, _
                                     ByVal dHighSelling_Count() As Double, _
                                     ByVal nSpCount() As Double, _
                                     ByVal nSpCount_Count() As Double) As String
    Dim tmpData As String = ""
    Dim htmlOut = New StringBuilder()
    Dim sRefLink As String = ""

    Dim tmpMarketGraphData As marketGraphData = Nothing

    Try

      ' print total aircraft Low Selling Price
      htmlOut.Append(WriteLineToBoth("<tr>", marketFile, marketFile_wHeader))

      If HttpContext.Current.Session.Item("marketTimeScale").ToString.ToLower.Contains("years") Or HttpContext.Current.Session.Item("marketTimeScale").ToString.ToLower.Contains("quarters") Then

        'tmpMarketGraphData = New marketGraphData

        'tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.AV_FORSALE
        'tmpMarketGraphData.marketGraph_topTitle = "Aircraft Low Selling Price"

        'tmpMarketGraphData.marketGraph_Y_title = "Average Low Selling Price"

        'For x As Integer = 0 To UBound(AV_For_Sale)
        '  If String.IsNullOrEmpty(tmpData) Then
        '    tmpData = FormatNumber((AV_For_Sale(x) / YearMonth_Count(x)), 0, False, False, False).ToString
        '  Else
        '    tmpData += crmWebClient.Constants.cCommaDelim + FormatNumber((AV_For_Sale(x) / YearMonth_Count(x)), 0, False, False, False).ToString
        '  End If
        'Next

        'tmpMarketGraphData.marketGraph_Y_data = tmpData
        'tmpData = ""

        'tmpMarketGraphData.marketGraph_X_title = "per Year"
        'tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

        'If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
        '  HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.AV_FORSALE) - 1) = tmpMarketGraphData
        'End If

        'sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.AV_FORSALE).ToString + "','marketSummaryGraph');"

        'htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">Aircraft Low Selling Price (Avg)</a></td>", marketFile, marketFile_wHeader))
        htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle"">Aircraft Low Selling Price</td>", marketFile, marketFile_wHeader))

      Else

        'tmpMarketGraphData = New marketGraphData

        'tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.AV_FORSALE
        'tmpMarketGraphData.marketGraph_topTitle = "Aircraft Low Selling Price"

        'tmpMarketGraphData.marketGraph_Y_title = "Low Selling Price"

        'For x As Integer = 0 To UBound(AV_For_Sale)
        '  If String.IsNullOrEmpty(tmpData) Then
        '    tmpData = AV_For_Sale(x).ToString
        '  Else
        '    tmpData += crmWebClient.Constants.cCommaDelim + AV_For_Sale(x).ToString
        '  End If
        'Next

        'tmpMarketGraphData.marketGraph_Y_data = tmpData
        'tmpData = ""

        'tmpMarketGraphData.marketGraph_X_title = "per Month"
        'tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

        'If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
        '  HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.AV_FORSALE) - 1) = tmpMarketGraphData
        'End If

        'sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.AV_FORSALE).ToString + "','marketSummaryGraph');"
        'htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">Aircraft Low Selling Price</a></td>", marketFile, marketFile_wHeader))
        htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle"">Aircraft Low Selling Price</td>", marketFile, marketFile_wHeader))

      End If

      htmlOut.Append(Calculate_Available_Summaries(ColumnSet, dLowSelling, dLowSelling_Count, YearMonth_Count, dtEndDate, True, False, False, False, False))

      htmlOut.Append(WriteLineToBoth("</tr>", marketFile, marketFile_wHeader))

      ' print aircraft Avg Selling Price
      htmlOut.Append(WriteLineToBoth("<tr>", marketFile, marketFile_wHeader))

      If HttpContext.Current.Session.Item("marketTimeScale").ToString.ToLower.Contains("years") Or HttpContext.Current.Session.Item("marketTimeScale").ToString.ToLower.Contains("quarters") Then

        'tmpMarketGraphData = New marketGraphData

        'tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.AV_INOPERATION
        'tmpMarketGraphData.marketGraph_topTitle = "Aircraft Avg Selling Price"

        'tmpMarketGraphData.marketGraph_Y_title = "Average Selling Price"

        'For x As Integer = 0 To UBound(AV_In_Operation_Fleet)
        '  If String.IsNullOrEmpty(tmpData) Then
        '    tmpData = FormatNumber((AV_In_Operation_Fleet(x) / YearMonth_Count(x)), 0, False, False, False).ToString
        '  Else
        '    tmpData += crmWebClient.Constants.cCommaDelim + FormatNumber((AV_In_Operation_Fleet(x) / YearMonth_Count(x)), 0, False, False, False).ToString
        '  End If
        'Next

        'tmpMarketGraphData.marketGraph_Y_data = tmpData
        'tmpData = ""

        'tmpMarketGraphData.marketGraph_X_title = "per Year"
        'tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

        'If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
        '  HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.AV_INOPERATION) - 1) = tmpMarketGraphData
        'End If

        'sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.AV_INOPERATION).ToString + "','marketSummaryGraph');"

        'htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">Aircraft Avg Selling Price (Avg)</a></td>", marketFile, marketFile_wHeader))
        htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle"">Aircraft Avg Selling Price</td>", marketFile, marketFile_wHeader))

      Else

        'tmpMarketGraphData = New marketGraphData

        'tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.AV_INOPERATION
        'tmpMarketGraphData.marketGraph_topTitle = "Aircraft Avg Selling Price"

        'tmpMarketGraphData.marketGraph_Y_title = "Average Selling Price"

        'For x As Integer = 0 To UBound(AV_In_Operation_Fleet)
        '  If String.IsNullOrEmpty(tmpData) Then
        '    tmpData = AV_In_Operation_Fleet(x).ToString
        '  Else
        '    tmpData += crmWebClient.Constants.cCommaDelim + AV_In_Operation_Fleet(x).ToString
        '  End If
        'Next

        'tmpMarketGraphData.marketGraph_Y_data = tmpData
        'tmpData = ""

        'tmpMarketGraphData.marketGraph_X_title = "per Month"
        'tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

        'If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
        '  HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.AV_INOPERATION) - 1) = tmpMarketGraphData
        'End If

        'sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.AV_INOPERATION).ToString + "','marketSummaryGraph');"
        'htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">Aircraft Avg Selling Price</a></td>", marketFile, marketFile_wHeader))
        htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle"">Aircraft Avg Selling Price</td>", marketFile, marketFile_wHeader))

      End If

      htmlOut.Append(Calculate_Available_Summaries(ColumnSet, dAvgSelling, dAvgSelling_Count, YearMonth_Count, dtEndDate, True, False, False, False, False))

      htmlOut.Append(WriteLineToBoth("</tr>", marketFile, marketFile_wHeader))

      ' print Aircraft High Selling Price
      htmlOut.Append(WriteLineToBoth("<tr>", marketFile, marketFile_wHeader))

      If HttpContext.Current.Session.Item("marketTimeScale").ToString.ToLower.Contains("years") Or HttpContext.Current.Session.Item("marketTimeScale").ToString.ToLower.Contains("quarters") Then

        'tmpMarketGraphData = New marketGraphData

        'tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.AV_INOPERATIONFORSALE
        'tmpMarketGraphData.marketGraph_topTitle = "Aircraft High Selling Price"

        'tmpMarketGraphData.marketGraph_Y_title = "Average High Selling Price"

        'For x As Integer = 0 To UBound(AV_In_Operation_Fleet_For_Sale)
        '  If String.IsNullOrEmpty(tmpData) Then
        '    tmpData = FormatNumber((AV_In_Operation_Fleet_For_Sale(x) / YearMonth_Count(x)), 0, False, False, False).ToString
        '  Else
        '    tmpData += crmWebClient.Constants.cCommaDelim + FormatNumber((AV_In_Operation_Fleet_For_Sale(x) / YearMonth_Count(x)), 0, False, False, False).ToString
        '  End If
        'Next

        'tmpMarketGraphData.marketGraph_Y_data = tmpData
        'tmpData = ""

        'tmpMarketGraphData.marketGraph_X_title = "per Year"
        'tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

        'If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
        '  HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.AV_INOPERATIONFORSALE) - 1) = tmpMarketGraphData
        'End If

        'sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.AV_INOPERATIONFORSALE).ToString + "','marketSummaryGraph');"

        'htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">Aircraft High Selling Price (Avg)</a></td>", marketFile, marketFile_wHeader))
        htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle"">Aircraft High Selling Price</td>", marketFile, marketFile_wHeader))

      Else

        'tmpMarketGraphData = New marketGraphData

        'tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.AV_INOPERATIONFORSALE
        'tmpMarketGraphData.marketGraph_topTitle = "Aircraft High Selling Price"

        'tmpMarketGraphData.marketGraph_Y_title = "Average High Selling Price"

        'For x As Integer = 0 To UBound(AV_In_Operation_Fleet_For_Sale)
        '  If String.IsNullOrEmpty(tmpData) Then
        '    tmpData = AV_In_Operation_Fleet_For_Sale(x).ToString
        '  Else
        '    tmpData += crmWebClient.Constants.cCommaDelim + AV_In_Operation_Fleet_For_Sale(x).ToString
        '  End If
        'Next

        'tmpMarketGraphData.marketGraph_Y_data = tmpData
        'tmpData = ""

        'tmpMarketGraphData.marketGraph_X_title = "per Month"
        'tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

        'If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
        '  HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.AV_INOPERATIONFORSALE) - 1) = tmpMarketGraphData
        'End If

        'sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.AV_INOPERATIONFORSALE).ToString + "','marketSummaryGraph');"
        'htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">Aircraft High Selling Price</a></td>", marketFile, marketFile_wHeader))
        htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle"">Aircraft High Selling Price</td>", marketFile, marketFile_wHeader))

      End If

      htmlOut.Append(Calculate_Available_Summaries(ColumnSet, dHighSelling, dHighSelling_Count, YearMonth_Count, dtEndDate, True, False, False, False, False))

      htmlOut.Append(WriteLineToBoth("</tr>", marketFile, marketFile_wHeader))
      htmlOut.Append("<tr height=""4""><td colspan=""" + sColSpan + """></td></tr>")

      ' print Aircraft Selling Price Count
      htmlOut.Append(WriteLineToBoth("<tr>", marketFile, marketFile_wHeader))

      If HttpContext.Current.Session.Item("marketTimeScale").ToString.ToLower.Contains("years") Or HttpContext.Current.Session.Item("marketTimeScale").ToString.ToLower.Contains("quarters") Then

        'tmpMarketGraphData = New marketGraphData

        'tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.AV_FORSALE_EU
        'tmpMarketGraphData.marketGraph_topTitle = "Aircraft Selling Price Count"

        'tmpMarketGraphData.marketGraph_Y_title = "Average Selling Price Count"

        'For x As Integer = 0 To UBound(AV_End_User)
        '  If String.IsNullOrEmpty(tmpData) Then
        '    tmpData = FormatNumber((AV_End_User(x) / YearMonth_Count(x)), 0, False, False, False).ToString
        '  Else
        '    tmpData += crmWebClient.Constants.cCommaDelim + FormatNumber((AV_End_User(x) / YearMonth_Count(x)), 0, False, False, False).ToString
        '  End If
        'Next

        'tmpMarketGraphData.marketGraph_Y_data = tmpData
        'tmpData = ""

        'tmpMarketGraphData.marketGraph_X_title = "per Year"
        'tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

        'If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
        '  HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.AV_FORSALE_EU) - 1) = tmpMarketGraphData
        'End If

        'sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.AV_FORSALE_EU).ToString + "','marketSummaryGraph');"
        'htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">Aircraft Selling Price Count (Avg)</a></td>", marketFile, marketFile_wHeader))
        htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle"">Aircraft Selling Price Count</td>", marketFile, marketFile_wHeader))

      Else

        'tmpMarketGraphData = New marketGraphData

        'tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.AV_FORSALE_EU
        'tmpMarketGraphData.marketGraph_topTitle = "Aircraft Selling Price Count"

        'tmpMarketGraphData.marketGraph_Y_title = "Selling Price Count"

        'For x As Integer = 0 To UBound(AV_End_User)
        '  If String.IsNullOrEmpty(tmpData) Then
        '    tmpData = AV_End_User(x).ToString
        '  Else
        '    tmpData += crmWebClient.Constants.cCommaDelim + AV_End_User(x).ToString
        '  End If
        'Next

        'tmpMarketGraphData.marketGraph_Y_data = tmpData
        'tmpData = ""

        'tmpMarketGraphData.marketGraph_X_title = "per Month"
        'tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

        'If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
        '  HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.AV_FORSALE_EU) - 1) = tmpMarketGraphData
        'End If

        'sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.AV_FORSALE_EU).ToString + "','marketSummaryGraph');"
        'htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">Aircraft Selling Price Count</a></td>", marketFile, marketFile_wHeader))
        htmlOut.Append(WriteLineToBoth("<td nowrap=""nowrap"" align=""left"" valign=""middle"">Aircraft Selling Price Count</td>", marketFile, marketFile_wHeader))
      End If

      htmlOut.Append(Calculate_Available_Summaries(ColumnSet, nSpCount, nSpCount_Count, YearMonth_Count, dtEndDate, True, False, False, False, False))

      htmlOut.Append(WriteLineToBoth("</tr>", marketFile, marketFile_wHeader))

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Print_Retail_Summaries()" + ex.Message
    End Try

    Return htmlOut.ToString

  End Function

#End Region

#Region "transaction_summary_functions"

  Public Function Store_Trans_Totals(ByRef in_adoRs As DataRow, _
                                     ByVal nIndex As Integer, _
                                     ByVal sCurrentTransType As String, _
                                     ByRef TransValues() As Double, _
                                     ByRef GroupValues() As Double, _
                                     ByRef SectionValues() As Double, _
                                     ByRef DaysOnTotValues() As Double, _
                                     ByRef DaysOnAvgValues() As Double, _
                                     ByRef AskingTotValues() As Double, _
                                     ByRef AskingAvgValues() As Double, _
                                     ByRef AskingHighValues() As Double, _
                                     ByRef AskingLowValues() As Double, _
                                     ByRef MakeOffValues() As Double, _
                                     ByRef YearAvgValues() As Long, _
                                     ByRef NewToMktValues() As Double, _
                                     ByRef ITValues() As Double) As Boolean


    Dim fJourn_newac_flag As String = ""
    Dim fJourn_date As String = ""
    Dim fAc_year As Long = 0
    Dim fAc_mfr_year As Long = 0
    Dim fAc_asking As String = ""
    Dim fAc_asking_price As Double = 0.0
    Dim fAc_list_date As String = ""

    Try

      If sCurrentTransType.Substring(sCurrentTransType.Length - 2, 2).ToUpper.Contains("IT") Then
        ITValues(nIndex) += 1
        Return True
      End If

      If Not IsDBNull(in_adoRs.Item("ac_list_date")) Then
        If Not String.IsNullOrEmpty(in_adoRs.Item("ac_list_date").ToString.Trim) Then
          fAc_list_date = CDate(in_adoRs.Item("ac_list_date").ToString).ToShortDateString
        End If
      End If

      If Not IsDBNull(in_adoRs.Item("journ_newac_flag")) Then
        If Not String.IsNullOrEmpty(in_adoRs.Item("journ_newac_flag").ToString.Trim) Then
          fJourn_newac_flag = in_adoRs.Item("journ_newac_flag").ToString.ToUpper.Trim
        End If
      End If

      If Not IsDBNull(in_adoRs.Item("journ_date")) Then
        If Not String.IsNullOrEmpty(in_adoRs.Item("journ_date").ToString.Trim) Then
          fJourn_date = CDate(in_adoRs.Item("journ_date").ToString).ToShortDateString
        End If
      End If

      If Not IsDBNull(in_adoRs.Item("ac_year")) Then
        If Not String.IsNullOrEmpty(in_adoRs.Item("ac_year").ToString.Trim) Then
          If IsNumeric(in_adoRs.Item("ac_year").ToString) Then
            fAc_year = CLng(in_adoRs.Item("ac_year").ToString)
          End If
        End If
      End If

      If Not IsDBNull(in_adoRs.Item("ac_mfr_year")) Then
        If Not String.IsNullOrEmpty(in_adoRs.Item("ac_mfr_year").ToString.Trim) Then
          If IsNumeric(in_adoRs.Item("ac_mfr_year").ToString) Then
            fAc_mfr_year = CLng(in_adoRs.Item("ac_mfr_year").ToString)
          End If
        End If
      End If

      If Not IsDBNull(in_adoRs.Item("ac_asking")) Then
        If Not String.IsNullOrEmpty(in_adoRs.Item("ac_asking").ToString.Trim) Then
          fAc_asking = in_adoRs.Item("ac_asking").ToString
        End If
      End If

      If Not IsDBNull(in_adoRs.Item("ac_asking_price")) Then
        If Not String.IsNullOrEmpty(in_adoRs.Item("ac_asking_price").ToString.Trim) Then
          fAc_asking_price = CDbl(in_adoRs.Item("ac_asking_price").ToString)
        End If
      End If

      TransValues(nIndex) += 1
      GroupValues(nIndex) += 1
      SectionValues(nIndex) += 1

      If fJourn_newac_flag.Contains("Y") Then
        NewToMktValues(nIndex) += 1
      End If

      If Not String.IsNullOrEmpty(fAc_list_date.Trim) Then
        If IsDate(fAc_list_date) Then

          Dim daysOnMarket As Long = DateDiff("d", CDate(fAc_list_date), CDate(fJourn_date))

          If daysOnMarket > 0 Then
            DaysOnTotValues(nIndex) += 1
            DaysOnAvgValues(nIndex) += CDbl(daysOnMarket)
          End If

        End If
      End If

      If fAc_asking.ToLower.Contains("price") Then
        If fAc_asking_price > 0 Then

          AskingTotValues(nIndex) += 1
          AskingAvgValues(nIndex) += fAc_asking_price

          If fAc_asking_price > AskingHighValues(nIndex) Then
            AskingHighValues(nIndex) = fAc_asking_price
          End If

          If fAc_asking_price < AskingLowValues(nIndex) Or AskingLowValues(nIndex) = 0 Then
            AskingLowValues(nIndex) = fAc_asking_price
          End If

        End If
      End If

      If fAc_asking_price = 0 Then
        MakeOffValues(nIndex) += 1
      End If

      If fAc_year > 0 Then
        YearAvgValues(nIndex) += fAc_year
      Else
        YearAvgValues(nIndex) += fAc_mfr_year
      End If

    Catch ex As Exception
      Return False
    End Try

    Return True

  End Function

  Public Function Print_Transaction_Group_Totals(ByVal localACSelection As marketSummaryObjAircraft, ByVal ColumnSet() As String, ByVal GroupValues() As Double, _
                                                 ByVal sTransCode As String, ByVal in_Trans_Type As String, ByVal nTransFromTotal As Double, _
                                                 ByVal sColSpan As String, ByVal arrBusinessTypes(,) As String) As String

    Dim htmlOut As New StringBuilder()
    Dim sRefLink As String = ""
    Dim sRefTitle As String = ""
    Dim tmpTransLink As String = ""

    Try

      Dim tmpAcDetails As String = make_linkback_aircraftInfo(localACSelection)

      If sTransCode.ToLower = "ma" Or sTransCode.ToLower = "om" Or sTransCode.ToLower = "wo" Then
        For xLoop As Integer = 0 To UBound(ColumnSet)
          GroupValues(xLoop) = 0
        Next

        Return ""
      End If

      If nTransFromTotal = 0 Then
        Return ""
      End If

      If HttpContext.Current.Session.Item("marketSumType").ToString.Trim.ToLower.Contains("trans_type") Then
        htmlOut.Append("<tr class=""header_row""><td align=""right"" valign=""middle"" colspan=""2"">TOTAL</td>")
      Else
        If HttpContext.Current.Session.Item("marketSumType").ToString.Trim.ToLower.Contains("trans_source") Then
          htmlOut.Append("<tr class=""header_row""><td align=""left"" valign=""middle"">" + business_type_name(in_Trans_Type.Substring(2, 2), arrBusinessTypes) + "</td>")
        Else
          htmlOut.Append("<tr class=""header_row""><td align=""left"" valign=""middle"">" + business_type_name(in_Trans_Type.Substring(in_Trans_Type.Length - 2, 2), arrBusinessTypes) + "</td>")
        End If
      End If

      For xLoop As Integer = 0 To UBound(ColumnSet)

        sRefLink = "javascript:ParseForm('0',true,false,false,false,false,'"

        If Not String.IsNullOrEmpty(tmpAcDetails.Trim) Then
          sRefLink += tmpAcDetails.Trim + "!~!"
        End If

        ' transaction date (range)
        sRefLink += "journ_date_operator=Between!~!journ_date=" + make_linkback_dateRange(ColumnSet(xLoop).ToString.Trim, False) + "!~!"

        ' transaction type
        tmpTransLink = make_linkback_transactionInfo(in_Trans_Type, False, True, True, "", "", True).Trim

        If Not String.IsNullOrEmpty(tmpTransLink) Then
          sRefLink += tmpTransLink + "!~!"
        End If

        sRefLink += "clearSelection=true!~!fromMarketSummary=true');"

        sRefTitle = IIf(CBool(HttpContext.Current.Application.Item("DebugFlag").ToString), " title=""" + sRefLink.Trim + """", " title=""Click to view aircraft list""")

        If GroupValues(xLoop) > 0 Then
          htmlOut.Append("<td align=""right"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink.Trim + """" + sRefTitle + ">" + FormatNumber(GroupValues(xLoop), 0, True, False, True) + "</a></td>")
        Else
          htmlOut.Append("<td align=""right"" valign=""middle"">0</td>")
        End If

      Next

      sRefLink = "javascript:ParseForm('0',true,false,false,false,false,'"

      If Not String.IsNullOrEmpty(tmpAcDetails.Trim) Then
        sRefLink += tmpAcDetails.Trim + "!~!"
      End If

      ' transaction date (range)
      sRefLink += "journ_date_operator=Between!~!journ_date=" + make_linkback_dateRange("", True) + "!~!"

      ' transaction type
      tmpTransLink = make_linkback_transactionInfo(in_Trans_Type, False, True, True, "", "", True).Trim

      If Not String.IsNullOrEmpty(tmpTransLink) Then
        sRefLink += tmpTransLink + "!~!"
      End If

      sRefLink += "clearSelection=true!~!fromMarketSummary=true');"

      sRefTitle = IIf(CBool(HttpContext.Current.Application.Item("DebugFlag").ToString), " title=""" + sRefLink.Trim + """", " title=""Click to view aircraft list""")

      If nTransFromTotal > 0 Then
        htmlOut.Append("<td align=""right"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink.Trim + """" + sRefTitle + ">" + FormatNumber(nTransFromTotal, 0, True, False, True) + "</a></td>")
      Else
        htmlOut.Append("<td align=""right"" valign=""middle"">0</td>")
      End If

      htmlOut.Append("</tr>")

      ' CLEAR THE ARRAY OF VALUES
      For xLoop As Integer = 0 To UBound(ColumnSet)
        GroupValues(xLoop) = 0
      Next

      ' INSERT A BLANK LINE AFTER TOTALS FOR FROM
      If HttpContext.Current.Session.Item("marketSumType").ToString.Trim.ToLower.Contains("trans_type") Then
        htmlOut.Append("<tr><td colspan=""" + sColSpan + """>&nbsp;</td></tr>")
      End If

    Catch ex As Exception
      Return ""
    End Try

    Return htmlOut.ToString
    htmlOut = Nothing

  End Function

  Public Function Print_Transaction_Type_Totals(ByVal localACSelection As marketSummaryObjAircraft, ByVal ColumnSet() As String, ByVal TransValues() As Double, _
                                                ByVal sTransCode As String, ByVal sTrans_To As String, ByRef sLast_From As String, ByVal sCurrentTransType As String, _
                                                ByVal nTransGroupTotal As Double, ByVal arrBusinessTypes(,) As String) As String

    Dim htmlOut As New StringBuilder()
    Dim sTrans_From As String = ""

    Dim sRefLink As String = ""
    Dim sRefTitle As String = ""
    Dim tmpTransLink As String = ""

    Try

      If sCurrentTransType.Substring(sCurrentTransType.Length - 2, 2).ToUpper.Contains("IT") Then
        Return ""
      End If

      If sTransCode.ToLower = "ma" Or sTransCode.ToLower = "om" Or sTransCode.ToLower = "wo" Then
        Return ""
      End If

      Dim tmpAcDetails As String = make_linkback_aircraftInfo(localACSelection)

      sTrans_From = business_type_name(sCurrentTransType.Substring(2, 2), arrBusinessTypes)

      If Not String.IsNullOrEmpty(sTrans_From) Then
        If sLast_From.Trim <> sTrans_From.Trim Then
          htmlOut.Append("<tr><td align=""left"" valign=""middle"" nowrap=""nowrap"">" + sTrans_From.Trim + "</td>")
        Else
          htmlOut.Append("<tr><td align=""left"" valign=""middle"" nowrap=""nowrap"">&nbsp;</td>")
        End If
      Else
        htmlOut.Append("<tr><td align=""left"" valign=""middle"" nowrap=""nowrap"">Unknown [" + sCurrentTransType.Substring(2, 2).Trim + "]</td>")
      End If

      sLast_From = sTrans_From

      'If Not sTransCode.Substring(0, 1).ToUpper.Trim.Contains("L") Then
      htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + sTrans_To + "</td>")
      'Else
      '  htmlOut.Append("<td align=""left"" valign=""middle"" nowrap=""nowrap"">" + sTrans_To + " (<em>" + Get_Lease_Type(sCurrentTransType.Substring(0, 2).ToUpper.Trim) + "<em>)</td>")
      'End If

      tmpTransLink = make_linkback_transactionInfo(sCurrentTransType, False, False, False).Trim

      ' WRITE THE TIMESCALE TOTALS FOR EACH TRANSACTION TYPE
      For xLoop As Integer = 0 To UBound(ColumnSet)

        sRefLink = "javascript:ParseForm('0',true,false,false,false,false,'"

        If Not String.IsNullOrEmpty(tmpAcDetails.Trim) Then
          sRefLink += tmpAcDetails.Trim + "!~!"
        End If

        ' transaction date (range)
        sRefLink += "journ_date_operator=Between!~!journ_date=" + make_linkback_dateRange(ColumnSet(xLoop).ToString.Trim, False) + "!~!"

        ' transaction type
        If Not String.IsNullOrEmpty(tmpTransLink) Then
          sRefLink += tmpTransLink + "!~!"
        End If

        sRefLink += "clearSelection=true!~!fromMarketSummary=true');"

        sRefTitle = IIf(CBool(HttpContext.Current.Application.Item("DebugFlag").ToString), " title=""" + sRefLink.Trim + """", " title=""Click to view aircraft list""")

        If TransValues(xLoop) > 0 Then
          htmlOut.Append("<td align=""right"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink.Trim + """" + sRefTitle + ">" + FormatNumber(TransValues(xLoop), 0, True, False, True) + "</a></td>")
        Else
          htmlOut.Append("<td align=""right"" valign=""middle"">0</td>")
        End If

      Next

      ' CLEAR THE TIMESCALE TOTALS FOR EACH TRANSACTION TYPE
      For xLoop As Integer = 0 To UBound(ColumnSet)
        TransValues(xLoop) = 0
      Next

      ' WRITE THE TOTAL TRANSACTIONS FOR THE TRANSACTION TYPE
      sRefLink = "javascript:ParseForm('0',true,false,false,false,false,'"

      If Not String.IsNullOrEmpty(tmpAcDetails.Trim) Then
        sRefLink += tmpAcDetails.Trim + "!~!"
      End If

      ' transaction date (range)
      sRefLink += "journ_date_operator=Between!~!journ_date=" + make_linkback_dateRange("", True) + "!~!"

      ' transaction type
      If Not String.IsNullOrEmpty(tmpTransLink) Then
        sRefLink += tmpTransLink + "!~!"
      End If

      sRefLink += "clearSelection=true!~!fromMarketSummary=true');"

      sRefTitle = IIf(CBool(HttpContext.Current.Application.Item("DebugFlag").ToString), " title=""" + sRefLink.Trim + """", " title=""Click to view aircraft list""")

      If nTransGroupTotal > 0 Then
        htmlOut.Append("<td align=""right"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink.Trim + """" + sRefTitle + ">" + FormatNumber(nTransGroupTotal, 0, True, False, True) + "</a></td>")
      Else
        htmlOut.Append("<td align=""right"" valign=""middle"">0</td>")
      End If

      htmlOut.Append("</tr>")

    Catch ex As Exception
      Return ""
    End Try

    Return htmlOut.ToString
    htmlOut = Nothing

  End Function

  Public Function Display_WholeSaleTotals(ByVal localACSelection As marketSummaryObjAircraft, ByVal ColumnSet() As String, _
                                   ByVal SectionValues() As Double, _
                                   ByRef DaysOnTotValues() As Double, _
                                   ByRef DaysOnAvgValues() As Double, _
                                   ByRef AskingTotValues() As Double, _
                                   ByRef AskingAvgValues() As Double, _
                                   ByRef AskingHighValues() As Double, _
                                   ByRef AskingLowValues() As Double, _
                                   ByRef MakeOffValues() As Double, _
                                   ByRef YearAvgValues() As Long, _
                                   ByRef NewToMktValues() As Double, _
                                   ByVal sTransactionClass As String, _
                                   ByVal sTransCode As String, _
                                   ByVal sHeaderString As String) As String

    Dim htmlOut As New StringBuilder()
    Dim tmpData As String = ""
    Dim sTrans_From As String = ""
    Dim sRefLink As String = ""
    Dim sRefTitle As String = ""
    Dim tmpTransLink As String = ""

    Dim tmpMarketGraphData As marketGraphData = Nothing

    Try

      Dim tmpAcDetails As String = make_linkback_aircraftInfo(localACSelection)

      ' print avg asking price
      htmlOut.Append("<tr class=""header_row"">")

      If HttpContext.Current.Session.Item("marketSumType").ToString.Trim.ToLower.Contains("trans_type") Then
        htmlOut.Append("<td>&nbsp;</td>")
      End If

      tmpMarketGraphData = New marketGraphData

      tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.WS_AVG_ASKING
      tmpMarketGraphData.marketGraph_topTitle = "Full Sale - Average Asking Price"

      tmpMarketGraphData.marketGraph_Y_title = "Aircraft Asking Price"

      For x As Integer = 0 To UBound(AskingAvgValues)
        If String.IsNullOrEmpty(tmpData) Then
          tmpData = FormatNumber((AskingAvgValues(x) / AskingTotValues(x)), 0, False, False, False).ToString
        Else
          tmpData += crmWebClient.Constants.cCommaDelim + FormatNumber((AskingAvgValues(x) / AskingTotValues(x)), 0, False, False, False).ToString
        End If
      Next

      tmpMarketGraphData.marketGraph_Y_data = tmpData
      tmpData = ""

      tmpMarketGraphData.marketGraph_X_title = "per Month"
      tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

      If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
        HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.WS_AVG_ASKING) - 1) = tmpMarketGraphData
      End If

      sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.WS_AVG_ASKING).ToString + "','marketSummaryGraph');"
      htmlOut.Append("<td align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">AVERAGE&nbsp;ASKING&nbsp;PRICE</a></td>")

      Dim TotAskingAvg As Double = 0
      Dim TotAskingNumber As Double = 0

      For xLoop = 0 To UBound(ColumnSet)
        If AskingTotValues(xLoop) > 0 Then
          htmlOut.Append("<td align=""right"" valign=""middle"">" + FormatCurrency((AskingAvgValues(xLoop) / AskingTotValues(xLoop)), 0, False, False, True) + "</td>")
          TotAskingAvg += AskingAvgValues(xLoop)
          TotAskingNumber += AskingTotValues(xLoop)
        Else
          htmlOut.Append("<td>&nbsp;</td>")
        End If
      Next

      If TotAskingNumber > 0 Then
        htmlOut.Append("<td align=""right"" valign=""middle"">" + FormatCurrency((TotAskingAvg / TotAskingNumber), 0, False, False, True) + "</td>")
      Else
        htmlOut.Append("<td align=""right"" valign=""middle"">&nbsp;</td>")
      End If

      htmlOut.Append("</tr>")

      ' print high asking price
      htmlOut.Append("<tr class=""header_row"">")
      If HttpContext.Current.Session.Item("marketSumType").ToString.Trim.ToLower.Contains("trans_type") Then
        htmlOut.Append("<td>&nbsp;</td>")
      End If

      tmpMarketGraphData = New marketGraphData

      tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.WS_HIGH_ASKING
      tmpMarketGraphData.marketGraph_topTitle = "Full Sale - High Asking Price"

      tmpMarketGraphData.marketGraph_Y_title = "Aircraft High Asking Price"

      For x As Integer = 0 To UBound(AskingHighValues)
        If String.IsNullOrEmpty(tmpData) Then
          tmpData = FormatNumber(AskingHighValues(x), 0, False, False, False).ToString
        Else
          tmpData += crmWebClient.Constants.cCommaDelim + FormatNumber(AskingHighValues(x), 0, False, False, False).ToString
        End If
      Next

      tmpMarketGraphData.marketGraph_Y_data = tmpData
      tmpData = ""

      tmpMarketGraphData.marketGraph_X_title = "per Month"
      tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

      If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
        HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.WS_HIGH_ASKING) - 1) = tmpMarketGraphData
      End If

      sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.WS_HIGH_ASKING).ToString + "','marketSummaryGraph');"
      htmlOut.Append("<td align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">HIGH&nbsp;ASKING&nbsp;PRICE</a></td>")

      Dim TotAskingHigh As Double = 0

      For xLoop = 0 To UBound(ColumnSet)
        If CDbl(AskingHighValues(xLoop)) > 0 Then

          sRefLink = "javascript:ParseForm('0',true,false,false,false,false,'"

          If Not String.IsNullOrEmpty(tmpAcDetails.Trim) Then
            sRefLink += tmpAcDetails.Trim + "!~!"
          End If

          ' transaction date (range)
          sRefLink += "journ_date_operator=Between!~!journ_date=" + make_linkback_dateRange(ColumnSet(xLoop).ToString.Trim, False) + "!~!"

          tmpTransLink = make_linkback_transactionInfo(sTransCode, False, True, True, "ac_asking_price equals " + AskingHighValues(xLoop).ToString.Trim)

          ' transaction type
          If Not String.IsNullOrEmpty(tmpTransLink) Then
            sRefLink += tmpTransLink + "!~!"
          End If

          sRefLink += "clearSelection=true!~!fromMarketSummary=true');"

          sRefTitle = IIf(CBool(HttpContext.Current.Application.Item("DebugFlag").ToString), " title=""" + sRefLink.Trim + """", " title=""Click to view aircraft list""")

          htmlOut.Append("<td align=""right"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """" + sRefTitle + ">" + FormatCurrency(AskingHighValues(xLoop), 0) + "</a></td>")

          If AskingHighValues(xLoop) > TotAskingHigh Then
            TotAskingHigh = AskingHighValues(xLoop)
          End If

        Else
          htmlOut.Append("<td align=""right"" valign=""middle"">&nbsp;</td>")
        End If
      Next

      sRefLink = "javascript:ParseForm('0',true,false,false,false,false,'"

      If Not String.IsNullOrEmpty(tmpAcDetails.Trim) Then
        sRefLink += tmpAcDetails.Trim + "!~!"
      End If

      ' transaction date (range)
      sRefLink += "journ_date_operator=Between!~!journ_date=" + make_linkback_dateRange("", True) + "!~!"

      If CDbl(TotAskingHigh) > 0 Then

        tmpTransLink = make_linkback_transactionInfo(sTransCode, False, True, True, "ac_asking_price equals " + TotAskingHigh.ToString.Trim)

        ' transaction type
        If Not String.IsNullOrEmpty(tmpTransLink) Then
          sRefLink += tmpTransLink + "!~!"
        End If

        sRefLink += "clearSelection=true!~!fromMarketSummary=true');"

        sRefTitle = IIf(CBool(HttpContext.Current.Application.Item("DebugFlag").ToString), " title=""" + sRefLink.Trim + """", " title=""Click to view aircraft list""")

        htmlOut.Append("<td align=""right"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """" + sRefTitle + ">" + FormatCurrency(TotAskingHigh, 0) + "</a></td>")
      Else
        htmlOut.Append("<td>&nbsp;</td>")
      End If

      htmlOut.Append("</tr>")

      ' print low asking price
      htmlOut.Append("<tr class=""header_row"">")
      If HttpContext.Current.Session.Item("marketSumType").ToString.Trim.ToLower.Contains("trans_type") Then
        htmlOut.Append("<td>&nbsp;</td>")
      End If

      tmpMarketGraphData = New marketGraphData

      tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.WS_LOW_ASKING
      tmpMarketGraphData.marketGraph_topTitle = "Full Sale - Low Asking Price"

      tmpMarketGraphData.marketGraph_Y_title = "Aircraft Low Asking Price"

      For x As Integer = 0 To UBound(AskingLowValues)
        If String.IsNullOrEmpty(tmpData) Then
          tmpData = FormatNumber(AskingLowValues(x), 0, False, False, False).ToString
        Else
          tmpData += crmWebClient.Constants.cCommaDelim + FormatNumber(AskingLowValues(x), 0, False, False, False).ToString
        End If
      Next

      tmpMarketGraphData.marketGraph_Y_data = tmpData
      tmpData = ""

      tmpMarketGraphData.marketGraph_X_title = "per Month"
      tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

      If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
        HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.WS_LOW_ASKING) - 1) = tmpMarketGraphData
      End If

      sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.WS_LOW_ASKING).ToString + "','marketSummaryGraph');"
      htmlOut.Append("<td align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">LOW&nbsp;ASKING&nbsp;PRICE</a></td>")

      Dim TotAskingLow As Double = 0

      For xLoop = 0 To UBound(ColumnSet)

        If CDbl(AskingLowValues(xLoop)) > 0 Then

          sRefLink = "javascript:ParseForm('0',true,false,false,false,false,'"

          If Not String.IsNullOrEmpty(tmpAcDetails.Trim) Then
            sRefLink += tmpAcDetails.Trim + "!~!"
          End If

          ' transaction date (range)
          sRefLink += "journ_date_operator=Between!~!journ_date=" + make_linkback_dateRange(ColumnSet(xLoop).ToString.Trim, False) + "!~!"

          tmpTransLink = make_linkback_transactionInfo(sTransCode, False, True, True, "ac_asking_price equals " + AskingLowValues(xLoop).ToString.Trim)

          ' transaction type
          If Not String.IsNullOrEmpty(tmpTransLink) Then
            sRefLink += tmpTransLink + "!~!"
          End If

          sRefLink += "clearSelection=true!~!fromMarketSummary=true');"

          sRefTitle = IIf(CBool(HttpContext.Current.Application.Item("DebugFlag").ToString), " title=""" + sRefLink.Trim + """", " title=""Click to view aircraft list""")

          htmlOut.Append("<td align=""right"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """" + sRefTitle + ">" + FormatCurrency(AskingLowValues(xLoop), 0) + "</a></td>")

          If AskingLowValues(xLoop) <= TotAskingLow Or TotAskingLow = 0 Then
            TotAskingLow = AskingLowValues(xLoop)
          End If

        Else
          htmlOut.Append("<td align=""right"" valign=""middle"">&nbsp;</td>")
        End If

      Next

      sRefLink = "javascript:ParseForm('0',true,false,false,false,false,'"

      If Not String.IsNullOrEmpty(tmpAcDetails.Trim) Then
        sRefLink += tmpAcDetails.Trim + "!~!"
      End If

      ' transaction date (range)
      sRefLink += "journ_date_operator=Between!~!journ_date=" + make_linkback_dateRange("", True) + "!~!"

      If CDbl(TotAskingLow) > 0 Then

        tmpTransLink = make_linkback_transactionInfo(sTransCode, False, True, True, "ac_asking_price equals " + TotAskingLow.ToString.Trim)

        ' transaction type
        If Not String.IsNullOrEmpty(tmpTransLink) Then
          sRefLink += tmpTransLink + "!~!"
        End If

        sRefLink += "clearSelection=true!~!fromMarketSummary=true');"

        sRefTitle = IIf(CBool(HttpContext.Current.Application.Item("DebugFlag").ToString), " title=""" + sRefLink.Trim + """", " title=""Click to view aircraft list""")

        htmlOut.Append("<td align=""right"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """" + sRefTitle + ">" + FormatCurrency(TotAskingLow, 0) + "</a></td>")

      Else
        htmlOut.Append("<td>&nbsp;</td>")
      End If

      htmlOut.Append("</tr>")

      ' print new to market totals
      htmlOut.Append("<tr class=""header_row"">")
      If HttpContext.Current.Session.Item("marketSumType").ToString.Trim.ToLower.Contains("trans_type") Then
        htmlOut.Append("<td>&nbsp;</td>")
      End If

      tmpMarketGraphData = New marketGraphData

      tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.WS_NEW_SALES
      tmpMarketGraphData.marketGraph_topTitle = "Full Sale - Aircraft New to Market"

      tmpMarketGraphData.marketGraph_Y_title = "New to Market"

      For x As Integer = 0 To UBound(NewToMktValues)
        If String.IsNullOrEmpty(tmpData) Then
          tmpData = FormatNumber(NewToMktValues(x), 0, False, False, False).ToString
        Else
          tmpData += crmWebClient.Constants.cCommaDelim + FormatNumber(NewToMktValues(x), 0, False, False, False).ToString
        End If
      Next

      tmpMarketGraphData.marketGraph_Y_data = tmpData
      tmpData = ""

      tmpMarketGraphData.marketGraph_X_title = "per Month"
      tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

      If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
        HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.WS_NEW_SALES) - 1) = tmpMarketGraphData
      End If

      sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.WS_NEW_SALES).ToString + "','marketSummaryGraph');"
      htmlOut.Append("<td align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">SALES&nbsp;OF<br />NEW&nbsp;AIRCRAFT</a></td>")

      Dim TotNewToMkt As Double = 0

      For xLoop = 0 To UBound(ColumnSet)
        If NewToMktValues(xLoop) > 0 Then   ' we have internal totals for this column

          sRefLink = "javascript:ParseForm('0',true,false,false,false,false,'"

          If Not String.IsNullOrEmpty(tmpAcDetails.Trim) Then
            sRefLink += tmpAcDetails.Trim + "!~!"
          End If

          ' transaction date (range)
          sRefLink += "journ_date_operator=Between!~!journ_date=" + make_linkback_dateRange(ColumnSet(xLoop).ToString.Trim, False) + "!~!"

          tmpTransLink = make_linkback_transactionInfo(sTransCode, False, True, True)

          ' transaction type
          If Not String.IsNullOrEmpty(tmpTransLink) Then
            sRefLink += tmpTransLink + "!~!"
          End If

          sRefLink += "journ_newac_flag=true" + "!~!"

          sRefLink += "clearSelection=true!~!fromMarketSummary=true');"

          sRefTitle = IIf(CBool(HttpContext.Current.Application.Item("DebugFlag").ToString), " title=""" + sRefLink.Trim + """", " title=""Click to view aircraft list""")

          htmlOut.Append("<td align=""right"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """" + sRefTitle + ">" + FormatNumber(NewToMktValues(xLoop), 0, True, False, True) + "</a></td>")
        Else
          htmlOut.Append("<td align=""right"" valign=""middle"">0</td>")
        End If

        TotNewToMkt += NewToMktValues(xLoop)

      Next

      sRefLink = "javascript:ParseForm('0',true,false,false,false,false,'"

      If Not String.IsNullOrEmpty(tmpAcDetails.Trim) Then
        sRefLink += tmpAcDetails.Trim + "!~!"
      End If

      ' transaction date (range)
      sRefLink += "journ_date_operator=Between!~!journ_date=" + make_linkback_dateRange("", True) + "!~!"

      If TotNewToMkt > 0 Then

        tmpTransLink = make_linkback_transactionInfo(sTransCode, False, True, True)

        ' transaction type
        If Not String.IsNullOrEmpty(tmpTransLink) Then
          sRefLink += tmpTransLink + "!~!"
        End If

        sRefLink += "journ_newac_flag=true" + "!~!"

        sRefLink += "clearSelection=true!~!fromMarketSummary=true');"

        sRefTitle = IIf(CBool(HttpContext.Current.Application.Item("DebugFlag").ToString), " title=""" + sRefLink.Trim + """", " title=""Click to view aircraft list""")

        htmlOut.Append("<td align=""right"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """" + sRefTitle + ">" + FormatNumber(TotNewToMkt, 0, True, False, True) + "</a></td>")
      Else
        htmlOut.Append("<td align=""right"" valign=""middle"">0</td>")
      End If

      htmlOut.Append("</tr>")

      ' print make offer totals
      htmlOut.Append("<tr class=""header_row"">")
      If HttpContext.Current.Session.Item("marketSumType").ToString.Trim.ToLower.Contains("trans_type") Then
        htmlOut.Append("<td>&nbsp;</td>")
      End If

      tmpMarketGraphData = New marketGraphData

      tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.WS_MAKE_OFFER
      tmpMarketGraphData.marketGraph_topTitle = "Full Sale - Make Offers"

      tmpMarketGraphData.marketGraph_Y_title = "Make Offers"

      For x As Integer = 0 To UBound(MakeOffValues)
        If String.IsNullOrEmpty(tmpData) Then
          tmpData = FormatNumber(MakeOffValues(x), 0, False, False, False).ToString
        Else
          tmpData += crmWebClient.Constants.cCommaDelim + FormatNumber(MakeOffValues(x), 0, False, False, False).ToString
        End If
      Next

      tmpMarketGraphData.marketGraph_Y_data = tmpData
      tmpData = ""

      tmpMarketGraphData.marketGraph_X_title = "per Month"
      tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

      If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
        HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.WS_MAKE_OFFER) - 1) = tmpMarketGraphData
      End If

      sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.WS_MAKE_OFFER).ToString + "','marketSummaryGraph');"
      htmlOut.Append("<td align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">MAKE&nbsp;OFFER</a></td>")

      Dim TotMakeOffer As Double = 0

      For xLoop = 0 To UBound(ColumnSet)
        If MakeOffValues(xLoop) > 0 Then   ' we have internal totals for this column
          sRefLink = "javascript:ParseForm('0',true,false,false,false,false,'"

          If Not String.IsNullOrEmpty(tmpAcDetails.Trim) Then
            sRefLink += tmpAcDetails.Trim + "!~!"
          End If

          ' transaction date (range)
          sRefLink += "journ_date_operator=Between!~!journ_date=" + make_linkback_dateRange(ColumnSet(xLoop).ToString.Trim, False) + "!~!"

          tmpTransLink = make_linkback_transactionInfo(sTransCode, False, True, True, "(ac_asking_price equals 0 OR ac_asking_price IS NULL)")

          ' transaction type
          If Not String.IsNullOrEmpty(tmpTransLink) Then
            sRefLink += tmpTransLink + "!~!"
          End If

          sRefLink += "clearSelection=true!~!fromMarketSummary=true');"

          sRefTitle = IIf(CBool(HttpContext.Current.Application.Item("DebugFlag").ToString), " title=""" + sRefLink.Trim + """", " title=""Click to view aircraft list""")

          htmlOut.Append("<td align=""right"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """" + sRefTitle + ">" + FormatNumber(MakeOffValues(xLoop), 0, True, False, True) + "</a></td>")
        Else
          htmlOut.Append("<td align=""right"" valign=""middle"">0</td>")
        End If

        TotMakeOffer += MakeOffValues(xLoop)

      Next

      sRefLink = "javascript:ParseForm('0',true,false,false,false,false,'"

      If Not String.IsNullOrEmpty(tmpAcDetails.Trim) Then
        sRefLink += tmpAcDetails.Trim + "!~!"
      End If

      ' transaction date (range)
      sRefLink += "journ_date_operator=Between!~!journ_date=" + make_linkback_dateRange("", True) + "!~!"

      If TotMakeOffer > 0 Then

        tmpTransLink = make_linkback_transactionInfo(sTransCode, False, True, True, "(ac_asking_price equals 0 OR ac_asking_price IS NULL)")

        ' transaction type
        If Not String.IsNullOrEmpty(tmpTransLink) Then
          sRefLink += tmpTransLink + "!~!"
        End If

        sRefLink += "clearSelection=true!~!fromMarketSummary=true');"

        sRefTitle = IIf(CBool(HttpContext.Current.Application.Item("DebugFlag").ToString), " title=""" + sRefLink.Trim + """", " title=""Click to view aircraft list""")

        htmlOut.Append("<td align=""right"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """" + sRefTitle + ">" + FormatNumber(TotMakeOffer, 0, True, False, True) + "</a></td>")
      Else
        htmlOut.Append("<td align=""right"" valign=""middle"">0</td>")
      End If

      htmlOut.Append("</tr>")

      ' print year average
      htmlOut.Append("<tr class=""header_row"">")
      If HttpContext.Current.Session.Item("marketSumType").ToString.Trim.ToLower.Contains("trans_type") Then
        htmlOut.Append("<td>&nbsp;</td>")
      End If

      tmpMarketGraphData = New marketGraphData

      tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.WS_AVG_YEAR
      tmpMarketGraphData.marketGraph_topTitle = "Full Sale - Average Aircraft Year"

      tmpMarketGraphData.marketGraph_Y_title = "Aircraft Year"

      For x As Integer = 0 To UBound(YearAvgValues)
        If String.IsNullOrEmpty(tmpData) Then
          tmpData = FormatNumber((YearAvgValues(x) / SectionValues(x)), 0, False, False, False).ToString
        Else
          tmpData += crmWebClient.Constants.cCommaDelim + FormatNumber((YearAvgValues(x) / SectionValues(x)), 0, False, False, False).ToString
        End If
      Next

      tmpMarketGraphData.marketGraph_Y_data = tmpData
      tmpData = ""

      tmpMarketGraphData.marketGraph_X_title = "per Month"
      tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

      If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
        HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.WS_AVG_YEAR) - 1) = tmpMarketGraphData
      End If

      sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.WS_AVG_YEAR).ToString + "','marketSummaryGraph');"
      htmlOut.Append("<td align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">AVERAGE&nbsp;YEAR</a></td>")

      Dim TotYearAvg As Double = 0
      Dim TotYearNumber As Double = 0

      For xLoop = 0 To UBound(ColumnSet)
        If SectionValues(xLoop) > 0 Then
          htmlOut.Append("<td align=""right"" valign=""middle"">" + FormatNumber(YearAvgValues(xLoop) / SectionValues(xLoop), 0, False, False, False) + "</td>")
          TotYearAvg += YearAvgValues(xLoop)
          TotYearNumber += SectionValues(xLoop)
        Else
          htmlOut.Append("<td>&nbsp;</td>")
        End If

      Next

      If TotYearNumber > 0 Then
        htmlOut.Append("<td align=""right"" valign=""middle"">" + FormatNumber(TotYearAvg / TotYearNumber, 0, False, False, False) + "</td>")
      Else
        htmlOut.Append("<td>&nbsp;</td>")
      End If

      htmlOut.Append("</tr>")

      ' print days on market
      htmlOut.Append("<tr class=""header_row"">")
      If HttpContext.Current.Session.Item("marketSumType").ToString.Trim.ToLower.Contains("trans_type") Then
        htmlOut.Append("<td>&nbsp;</td>")
      End If

      tmpMarketGraphData = New marketGraphData

      tmpMarketGraphData.marketGraph_LinkType = eGraphLinkType.WS_AVG_DAYSONMARKET
      tmpMarketGraphData.marketGraph_topTitle = "Full Sale - Average Days on Market"

      tmpMarketGraphData.marketGraph_Y_title = "Days on Market"

      For x As Integer = 0 To UBound(DaysOnAvgValues)
        If String.IsNullOrEmpty(tmpData) Then
          tmpData = FormatNumber((DaysOnAvgValues(x) / DaysOnTotValues(x)), 0, False, False, False).ToString
        Else
          tmpData += crmWebClient.Constants.cCommaDelim + FormatNumber((DaysOnAvgValues(x) / DaysOnTotValues(x)), 0, False, False, False).ToString
        End If
      Next

      tmpMarketGraphData.marketGraph_Y_data = tmpData
      tmpData = ""

      tmpMarketGraphData.marketGraph_X_title = "per Month"
      tmpMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

      If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
        HttpContext.Current.Session.Item("marketGraphData")(CInt(eGraphLinkType.WS_AVG_DAYSONMARKET) - 1) = tmpMarketGraphData
      End If

      sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + CInt(eGraphLinkType.WS_AVG_DAYSONMARKET).ToString + "','marketSummaryGraph');"
      htmlOut.Append("<td align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">AVERAGE&nbsp;DAYS<br />ON&nbsp;MARKET</a></td>")

      Dim TotOnMarketAvg As Double = 0
      Dim TotDaysRecords As Double = 0

      For xLoop As Integer = 0 To UBound(ColumnSet)
        If DaysOnTotValues(xLoop) > 0 Then
          htmlOut.Append("<td align=""right"" valign=""middle"">" + FormatNumber(DaysOnAvgValues(xLoop) / DaysOnTotValues(xLoop), 0, False, False, False) + "</td>")
          TotDaysRecords += DaysOnTotValues(xLoop)
        Else
          htmlOut.Append("<td>&nbsp;</td>")
        End If
        TotOnMarketAvg += DaysOnAvgValues(xLoop)
      Next

      If TotDaysRecords > 0 Then
        htmlOut.Append("<td align=""right"" valign=""middle"">" + FormatNumber(TotOnMarketAvg / TotDaysRecords, 0, False, False, False) + "</td>")
      Else
        htmlOut.Append("<td>&nbsp;</td>")
      End If

      htmlOut.Append("</tr>")

    Catch ex As Exception
      Return ""
    End Try

    Return htmlOut.ToString
    htmlOut = Nothing

  End Function

  Public Function Print_Transaction_Section_Totals(ByVal localACSelection As marketSummaryObjAircraft, ByVal ColumnSet() As String, _
                                                   ByVal TransValues() As Double, ByVal SectionValues() As Double, ByVal ITValues() As Double, _
                                                   ByVal DaysOnTotValues() As Double, ByVal DaysOnAvgValues() As Double, ByVal AskingTotValues() As Double, _
                                                   ByVal AskingAvgValues() As Double, ByVal AskingHighValues() As Double, ByVal AskingLowValues() As Double, _
                                                   ByVal MakeOffValues() As Double, ByVal YearAvgValues() As Long, ByVal NewToMktValues() As Double, _
                                                   ByVal sTransactionClass As String, ByVal sTransCode As String, ByVal sCurrentTransType As String, _
                                                   ByVal nTransTotal As Double, ByVal sHeaderString As String, ByVal sColSpan As String) As String

    Dim tmpData As String = ""

    Dim htmlOut As New StringBuilder()
    Dim sRefLink As String = ""
    Dim sRefTitle As String = ""
    Dim tmpTransLink As String = ""

    Dim totalMarketGraphData As marketGraphData = Nothing
    Dim totGraphID As Integer = 0
    Dim totGraphIndex As Integer = 0

    Dim internalMarketGraphData As marketGraphData = Nothing
    Dim intGraphID As Integer = 0
    Dim intGraphIndex As Integer = 0

    Try

      Dim tmpAcDetails As String = make_linkback_aircraftInfo(localACSelection)

      totalMarketGraphData = New marketGraphData
      internalMarketGraphData = New marketGraphData

      Select Case sTransCode.ToLower
        Case "ws"
          totalMarketGraphData.marketGraph_LinkType = eGraphLinkType.WS_TOTAL_TX
          internalMarketGraphData.marketGraph_LinkType = eGraphLinkType.WS_INTERNAL_TX

          totGraphID = CInt(eGraphLinkType.WS_TOTAL_TX)
          totGraphIndex = CInt(eGraphLinkType.WS_TOTAL_TX) - 1

          intGraphID = CInt(eGraphLinkType.WS_INTERNAL_TX)
          intGraphIndex = CInt(eGraphLinkType.WS_INTERNAL_TX) - 1

        Case "wo"
          totalMarketGraphData.marketGraph_LinkType = eGraphLinkType.WO_TOTAL_TX

          totGraphID = CInt(eGraphLinkType.WO_TOTAL_TX)
          totGraphIndex = CInt(eGraphLinkType.WO_TOTAL_TX) - 1

        Case "om"
          totalMarketGraphData.marketGraph_LinkType = eGraphLinkType.OM_TOTAL_TX

          totGraphID = CInt(eGraphLinkType.OM_TOTAL_TX)
          totGraphIndex = CInt(eGraphLinkType.OM_TOTAL_TX) - 1

        Case "ma"
          totalMarketGraphData.marketGraph_LinkType = eGraphLinkType.MA_TOTAL_TX

          totGraphID = CInt(eGraphLinkType.MA_TOTAL_TX)
          totGraphIndex = CInt(eGraphLinkType.MA_TOTAL_TX) - 1

        Case "dp"
          totalMarketGraphData.marketGraph_LinkType = eGraphLinkType.DP_TOTAL_TX
          internalMarketGraphData.marketGraph_LinkType = eGraphLinkType.DP_INTERNAL_TX

          totGraphID = CInt(eGraphLinkType.DP_TOTAL_TX)
          totGraphIndex = CInt(eGraphLinkType.DP_TOTAL_TX) - 1

          intGraphID = CInt(eGraphLinkType.DP_INTERNAL_TX)
          intGraphIndex = CInt(eGraphLinkType.DP_INTERNAL_TX) - 1

        Case "fs"
          totalMarketGraphData.marketGraph_LinkType = eGraphLinkType.FS_TOTAL_TX
          internalMarketGraphData.marketGraph_LinkType = eGraphLinkType.FS_INTERNAL_TX

          totGraphID = CInt(eGraphLinkType.FS_TOTAL_TX)
          totGraphIndex = CInt(eGraphLinkType.FS_TOTAL_TX) - 1

          intGraphID = CInt(eGraphLinkType.FS_INTERNAL_TX)
          intGraphIndex = CInt(eGraphLinkType.FS_INTERNAL_TX) - 1

        Case "ss"
          totalMarketGraphData.marketGraph_LinkType = eGraphLinkType.SS_TOTAL_TX
          internalMarketGraphData.marketGraph_LinkType = eGraphLinkType.SS_INTERNAL_TX

          totGraphID = CInt(eGraphLinkType.SS_TOTAL_TX)
          totGraphIndex = CInt(eGraphLinkType.SS_TOTAL_TX) - 1

          intGraphID = CInt(eGraphLinkType.SS_INTERNAL_TX)
          intGraphIndex = CInt(eGraphLinkType.SS_INTERNAL_TX) - 1

        Case "fc"
          totalMarketGraphData.marketGraph_LinkType = eGraphLinkType.FC_TOTAL_TX
          internalMarketGraphData.marketGraph_LinkType = eGraphLinkType.FC_INTERNAL_TX

          totGraphID = CInt(eGraphLinkType.FC_TOTAL_TX)
          totGraphIndex = CInt(eGraphLinkType.FC_TOTAL_TX) - 1

          intGraphID = CInt(eGraphLinkType.FC_INTERNAL_TX)
          intGraphIndex = CInt(eGraphLinkType.FC_INTERNAL_TX) - 1

        Case "l", "la", "lx", "ln", "lo", "ls", "lt"
          totalMarketGraphData.marketGraph_LinkType = eGraphLinkType.LS_TOTAL_TX
          internalMarketGraphData.marketGraph_LinkType = eGraphLinkType.LS_INTERNAL_TX

          totGraphID = CInt(eGraphLinkType.LS_TOTAL_TX)
          totGraphIndex = CInt(eGraphLinkType.LS_TOTAL_TX) - 1

          intGraphID = CInt(eGraphLinkType.LS_INTERNAL_TX)
          intGraphIndex = CInt(eGraphLinkType.LS_INTERNAL_TX) - 1

        Case "sz"
          totalMarketGraphData.marketGraph_LinkType = eGraphLinkType.SZ_TOTAL_TX
          internalMarketGraphData.marketGraph_LinkType = eGraphLinkType.SZ_INTERNAL_TX

          totGraphID = CInt(eGraphLinkType.SZ_TOTAL_TX)
          totGraphIndex = CInt(eGraphLinkType.SZ_TOTAL_TX) - 1

          intGraphID = CInt(eGraphLinkType.SZ_INTERNAL_TX)
          intGraphIndex = CInt(eGraphLinkType.SZ_INTERNAL_TX) - 1

      End Select

      totalMarketGraphData.marketGraph_topTitle = "Total Transactions for " + sTransactionClass

      totalMarketGraphData.marketGraph_Y_title = "Transactions"

      For x As Integer = 0 To UBound(SectionValues)
        If String.IsNullOrEmpty(tmpData) Then
          If SectionValues(x) > 0 Then
            tmpData = FormatNumber(SectionValues(x), 0, False, False, False).ToString
          Else
            tmpData = "0"
          End If
        Else
          If SectionValues(x) > 0 Then
            tmpData += crmWebClient.Constants.cCommaDelim + FormatNumber(SectionValues(x), 0, False, False, False).ToString
          Else
            tmpData += crmWebClient.Constants.cCommaDelim + "0"
          End If
        End If
      Next

      totalMarketGraphData.marketGraph_Y_data = tmpData
      tmpData = ""

      totalMarketGraphData.marketGraph_X_title = "per Month"
      totalMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

      If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
        HttpContext.Current.Session.Item("marketGraphData")(totGraphIndex) = totalMarketGraphData
      End If

      sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + totGraphID.ToString + "','marketSummaryGraph');"
      htmlOut.Append("<tr class=""header_row"">")
      htmlOut.Append("<td align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">TOTAL&nbsp;TRANSACTIONS</a></td>")

      If HttpContext.Current.Session.Item("marketSumType").ToString.Trim.ToLower.Contains("trans_type") Then
        If sTransCode.ToLower <> "ma" And sTransCode.ToLower <> "om" And sTransCode.ToLower <> "wo" Then
          htmlOut.Append("<td>&nbsp;</td>")
        End If
      End If


      For xLoop As Integer = 0 To UBound(SectionValues)
        sRefLink = "javascript:ParseForm('0',true,false,false,false,false,'"

        If Not String.IsNullOrEmpty(tmpAcDetails.Trim) Then
          sRefLink += tmpAcDetails.Trim + "!~!"
        End If

        ' transaction date (range)
        sRefLink += "journ_date_operator=Between!~!journ_date=" + make_linkback_dateRange(ColumnSet(xLoop).ToString.Trim, False) + "!~!"

        tmpTransLink = make_linkback_transactionInfo(sTransCode, True, True, True, "", sCurrentTransType, True).Trim

        ' transaction type
        If Not String.IsNullOrEmpty(tmpTransLink) Then
          sRefLink += tmpTransLink + "!~!"
        End If

        sRefLink += "clearSelection=true!~!fromMarketSummary=true');"
        sRefTitle = IIf(CBool(HttpContext.Current.Application.Item("DebugFlag").ToString), " title=""" + sRefLink.Trim + """", " title=""Click to view aircraft list""")

        If SectionValues(xLoop) > 0 Then
          htmlOut.Append("<td align=""right"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink.Trim + """" + sRefTitle + ">" + FormatNumber(SectionValues(xLoop), 0, True, False, True) + "</a></td>")
        Else
          htmlOut.Append("<td align=""right"" valign=""middle"">" + FormatNumber(TransValues(xLoop), 0, True, False, True) + "</td>")
        End If

      Next

      sRefLink = "javascript:ParseForm('0',true,false,false,false,false,'"

      If Not String.IsNullOrEmpty(tmpAcDetails.Trim) Then
        sRefLink += tmpAcDetails.Trim + "!~!"
      End If

      ' transaction date (range)
      sRefLink += "journ_date_operator=Between!~!journ_date=" + make_linkback_dateRange("", True) + "!~!"

      tmpTransLink = make_linkback_transactionInfo(sTransCode, True, True, True, "", sCurrentTransType, True).Trim

      ' transaction type
      If Not String.IsNullOrEmpty(tmpTransLink) Then
        sRefLink += tmpTransLink + "!~!"
      End If

      sRefLink += "clearSelection=true!~!fromMarketSummary=true');"
      sRefTitle = IIf(CBool(HttpContext.Current.Application.Item("DebugFlag").ToString), " title=""" + sRefLink.Trim + """", " title=""Click to view aircraft list""")

      If nTransTotal > 0 Then
        htmlOut.Append("<td align=""right"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink.Trim + """" + sRefTitle + ">" + FormatNumber(nTransTotal, 0, True, False, True) + "</a></td>")
      Else
        htmlOut.Append("<td align=""right"" valign=""middle"">0</td>")
      End If

      htmlOut.Append("</tr>")

      If sTransCode.ToLower = "ws" Then

        htmlOut.Append("<tr><td colspan=""" + sColSpan + """>&nbsp;</td></tr>")

        htmlOut.Append(Display_WholeSaleTotals(localACSelection, ColumnSet, SectionValues, DaysOnTotValues, DaysOnAvgValues, AskingTotValues, AskingAvgValues, AskingHighValues, AskingLowValues, MakeOffValues, YearAvgValues, NewToMktValues, sTransactionClass, sTransCode, sHeaderString))

        htmlOut.Append("<tr><td colspan=""" + sColSpan + """>&nbsp;</td></tr>")

      End If

      ' DON'T PRINT INTERNAL TOTALS IF ON,OFF MARKET OR WRITTEN OFF
      If sTransCode.ToLower <> "ma" And sTransCode.ToLower <> "om" And sTransCode.ToLower <> "wo" Then

        Dim tmpValue As Double = 0

        ' Check to see if we have any data for internals before we print them out      
        For x As Integer = 0 To UBound(ITValues)
          If ITValues(x) > 0 Then
            tmpValue += ITValues(x)
          End If
        Next

        If tmpValue > 0 Then

          htmlOut.Append("<tr><td colspan=""" + sColSpan + """>&nbsp;</td></tr>")
          htmlOut.Append("<tr class=""header_row"">")

          If HttpContext.Current.Session.Item("marketSumType").ToString.Trim.ToLower.Contains("trans_type") Then
            htmlOut.Append("<td>&nbsp;</td>")
          End If

          internalMarketGraphData.marketGraph_topTitle = "Internal Transactions for " + sTransactionClass

          internalMarketGraphData.marketGraph_Y_title = "Transactions"

          For x As Integer = 0 To UBound(ITValues)
            If String.IsNullOrEmpty(tmpData) Then
              If ITValues(x) > 0 Then
                tmpData = FormatNumber(ITValues(x), 0, False, False, False).ToString
              Else
                tmpData = "0"
              End If
            Else
              If ITValues(x) > 0 Then
                tmpData += crmWebClient.Constants.cCommaDelim + FormatNumber(ITValues(x), 0, False, False, False).ToString
              Else
                tmpData += crmWebClient.Constants.cCommaDelim + "0"
              End If
            End If
          Next

          internalMarketGraphData.marketGraph_Y_data = tmpData
          tmpData = ""

          internalMarketGraphData.marketGraph_X_title = "per Month"
          internalMarketGraphData.marketGraph_X_data = String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)

          If Not IsNothing(HttpContext.Current.Session.Item("marketGraphData")) And IsArray(HttpContext.Current.Session.Item("marketGraphData")) Then
            HttpContext.Current.Session.Item("marketGraphData")(intGraphIndex) = internalMarketGraphData
          End If

          sRefLink = "javascript:openSmallWindowJS('MarketSummaryGraphs.aspx?graphID=" + intGraphID.ToString + "','marketSummaryGraph');"
          htmlOut.Append("<td align=""left"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink + """ title=""Click to Graph This Line"">INTERNAL&nbsp;TRANSACTIONS</a></td>")

          Dim TotalIT As Double = 0
          For x As Integer = 0 To UBound(ITValues)

            If ITValues(x) > 0 Then

              sRefLink = "javascript:ParseForm('0',true,false,false,false,false,'"

              If Not String.IsNullOrEmpty(tmpAcDetails.Trim) Then
                sRefLink += tmpAcDetails.Trim + "!~!"
              End If

              ' transaction date (range)
              sRefLink += "journ_date_operator=Between!~!journ_date=" + make_linkback_dateRange(ColumnSet(x).ToString.Trim, False) + "!~!"

              tmpTransLink = make_linkback_transactionInfo(sTransCode.ToUpper, True, True, False, "", sCurrentTransType, True).Trim

              ' transaction type
              If Not String.IsNullOrEmpty(tmpTransLink) Then
                sRefLink += tmpTransLink + "!~!"
              End If

              sRefLink += "clearSelection=true!~!fromMarketSummary=true');"
              sRefTitle = IIf(CBool(HttpContext.Current.Application.Item("DebugFlag").ToString), " title=""" + sRefLink.Trim + """", " title=""Click to view aircraft list""")

              If CDbl(ITValues(x)) > 0 Then   ' we have internal totals for this column
                htmlOut.Append("<td align=""right"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink.Trim + """" + sRefTitle + ">" + FormatNumber(ITValues(x), 0, True, False, True) + "</a></td>")
              Else
                htmlOut.Append("<td align=""right"" valign=""middle"">0</td>")
              End If

              TotalIT += ITValues(x)

            Else
              htmlOut.Append("<td align=""right"" valign=""middle"">0</td>")
            End If

          Next

          sRefLink = "javascript:ParseForm('0',true,false,false,false,false,'"

          If Not String.IsNullOrEmpty(tmpAcDetails.Trim) Then
            sRefLink += tmpAcDetails.Trim + "!~!"
          End If

          ' transaction date (range)
          sRefLink += "journ_date_operator=Between!~!journ_date=" + make_linkback_dateRange("", True) + "!~!"

          tmpTransLink = make_linkback_transactionInfo(sTransCode.ToUpper, True, True, False, "", sCurrentTransType, True).Trim

          ' transaction type
          If Not String.IsNullOrEmpty(tmpTransLink) Then
            sRefLink += tmpTransLink + "!~!"
          End If

          sRefLink += "clearSelection=true!~!fromMarketSummary=true');"
          sRefTitle = IIf(CBool(HttpContext.Current.Application.Item("DebugFlag").ToString), " title=""" + sRefLink.Trim + """", " title=""Click to view aircraft list""")

          If TotalIT > 0 Then
            htmlOut.Append("<td align=""right"" valign=""middle""><a class=""underline cursor"" onclick=""" + sRefLink.Trim + """" + sRefTitle + ">" + FormatNumber(TotalIT, 0, True, False, True) + "</a></td>")
          Else
            htmlOut.Append("<td align=""right"" valign=""middle"">0</td>")
          End If

          htmlOut.Append("</tr>")

        End If

      End If   ' if OM or WO or MA don't print internals

    Catch ex As Exception
      Return ""
    End Try

    totalMarketGraphData = Nothing
    internalMarketGraphData = Nothing

    Return htmlOut.ToString
    htmlOut = Nothing

  End Function

#End Region

End Class
