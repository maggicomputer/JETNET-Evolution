Imports Microsoft.VisualBasic
Imports System.ComponentModel

' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/generateOpCostsReport.vb $
'$$Author: Mike $
'$$Date: 6/19/19 8:49a $
'$$Modtime: 6/18/19 6:12p $
'$$Revision: 2 $
'$$Workfile: generateOpCostsReport.vb $
'
' ********************************************************************************

<System.Serializable()> Public Class generateOpCostsReport

  Private aError As String
  Private sRptTitle As String
  Private sCurrencyName As String
  Private sCurrencySymbol As String
  Private sCurrencyDate As String
  Private bChangeCurrency As Boolean
  Private nOpCostsModelID As Long
  Private sOpCostsModelIDList As String
  Private sOpCostsFileName As String

  Private convertedOpCostsData() As opCostsClass

  Sub New()

    aError = ""

    sRptTitle = ""
    sCurrencyName = ""
    sCurrencySymbol = ""
    sCurrencyDate = ""
    bChangeCurrency = False
    nOpCostsModelID = -1
    sOpCostsModelIDList = ""
    sOpCostsFileName = ""

    convertedOpCostsData = Nothing

    Dim sErrorString As String = ""

    If Not HttpContext.Current.Session.Item("localPreferences").loadUserSession(sErrorString, CLng(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString), HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString, CLng(HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString), CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)) Then
      aError = "error in load preferences : " + sErrorString
    End If

    If Not IsNothing(HttpContext.Current.Session.Item("OpCostsModelID")) Then
      If CLng(HttpContext.Current.Session.Item("OpCostsModelID").ToString) > -1 Then
        nOpCostsModelID = CLng(HttpContext.Current.Session.Item("OpCostsModelID").ToString)
      End If
    End If

    If Not IsNothing(HttpContext.Current.Session.Item("OpCostsModelList")) Then
      If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("OpCostsModelList").ToString) Then
        sOpCostsModelIDList = HttpContext.Current.Session.Item("OpCostsModelList").ToString
      End If
    End If

    Dim subscriptionInfo As String = HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString + "_" + HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString.Trim + "_" + HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString + "_"

    sOpCostsFileName = subscriptionInfo + "export_of_current_operating_costs_list" + IIf(HttpContext.Current.Session.Item("localPreferences").UseMetricValues, "_5_metric", IIf(HttpContext.Current.Session.Item("localPreferences").UseStatuteMile, "_5_standard", "_5_nautical"))

    sOpCostsFileName = commonEvo.GenerateFileName(sOpCostsFileName, ".xls", False)

    HttpContext.Current.Session.Item("OpCostsBaseFileName") = ""
    HttpContext.Current.Session.Item("OpCostsBaseFileName") = sOpCostsFileName.Replace(".xls", "") ' just clean off file extension

    If HttpContext.Current.Session.Item("homebasefuelPrice") = 0 Then
      HttpContext.Current.Session.Item("homebasefuelPrice") = CDbl(commonEvo.Get_Homebase_Fuel_Price())
    End If

    If CDbl(HttpContext.Current.Session.Item("localfuelPrice")) > 0 Then
      HttpContext.Current.Session.Item("fuelPriceBase") = CDbl(HttpContext.Current.Session.Item("localfuelPrice").ToString)
    ElseIf CDbl(HttpContext.Current.Session.Item("homebasefuelPrice")) > 0 Then
      HttpContext.Current.Session.Item("fuelPriceBase") = CDbl(HttpContext.Current.Session.Item("homebasefuelPrice").ToString)
    End If

    If CInt(HttpContext.Current.Session.Item("localPreferences").DefaultCurrency.ToString) <> 9 Then ' 9 = us dollar

      HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate = commonEvo.GetForeignExchangeRate(CInt(HttpContext.Current.Session.Item("localPreferences").DefaultCurrency.ToString), sCurrencyName, sCurrencyDate)

      sRptTitle = sCurrencyName.Trim

      If Not String.IsNullOrEmpty(sCurrencyDate) Then
        sRptTitle &= " (" + HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString + ") as of " + FormatDateTime(sCurrencyDate, vbShortDate)
      Else
        sRptTitle &= " (" + HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString + ")"
      End If

      If sCurrencyName.ToLower.Contains("euro") Then
        sCurrencySymbol = crmWebClient.Constants.cEuroSymbol
      ElseIf sCurrencyName.ToLower.Contains("dollar") Then
        sCurrencySymbol = crmWebClient.Constants.cDollarSymbol
      ElseIf sCurrencyName.ToLower.Contains("pound") Then
        sCurrencySymbol = crmWebClient.Constants.cPoundSymbol
      Else
        sCurrencySymbol = crmWebClient.Constants.cEmptyString
      End If

    Else

      HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate = 0
      sCurrencySymbol = crmWebClient.Constants.cDollarSymbol
      sCurrencyName = "Dollar (US)"
      sCurrencyDate = Now().ToShortDateString

      sRptTitle &= Trim("Dollar (US)")

    End If

    If CDbl(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString) > 0 Then
      bChangeCurrency = True
    End If

  End Sub

  Public Property class_error() As String
    Get
      class_error = aError
    End Get
    Set(ByVal value As String)
      aError = value
    End Set
  End Property

  Public Property useMetricValues() As Boolean
    Get
      useMetricValues = HttpContext.Current.Session.Item("localPreferences").UseMetricValues
    End Get
    Set(ByVal value As Boolean)
      HttpContext.Current.Session.Item("localPreferences").UseMetricValues = value
    End Set
  End Property

  Public Property useStatuteMiles() As Boolean
    Get
      useStatuteMiles = HttpContext.Current.Session.Item("localPreferences").UseStatuteMiles
    End Get
    Set(ByVal value As Boolean)
      HttpContext.Current.Session.Item("localPreferences").UseStatuteMiles = value
    End Set
  End Property

  Public Function generate_excel_output() As Boolean

    Dim dataTableToload As DataTable = Nothing
    Dim dataSetToProcess As New DataSet

    Try

      If Not loadReportDataTable(dataTableToload, convertedOpCostsData) Then
        aError += "error in generateOpCostsReport.vb : did not load op costs recordset"
      End If

      If Not processOpCostsDataSet(dataTableToload, convertedOpCostsData) Then
        aError += "error in generateOpCostsReport.vb : did not load class with data from recordset"
      End If

      If Not convertOpCostsClassToDataset(convertedOpCostsData, dataSetToProcess) Then
        aError += "error in generateOpCostsReport.vb : did not convert class to recordset"
      End If

      If Not outputOpcostsResults(dataSetToProcess) Then
        aError += "error in generateOpCostsReport.vb : did not output results"
      End If

      dataTableToload = Nothing
      dataSetToProcess = Nothing

    Catch ex As Exception

      aError += "error in generate_excel_output() : " + ex.Message
      Return False

    End Try

    Return True

  End Function

  Private Function outputOpcostsResults(ByVal ds As DataSet) As Boolean

    ' will output .xls and .html file formats
    Try

      Dim reportFolder As String = HttpContext.Current.Server.MapPath(HttpContext.Current.Session.Item("MarketSummaryFolderVirtualPath").ToString)
      Dim reportDisplayFolder As String = HttpContext.Current.Session.Item("MarketSummaryFolderVirtualPath").ToString
      Dim sFileName As String = ""

      Dim subscriptionInfo As String = HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString + "_" + HttpContext.Current.Session.Item("localUser").crmUserLogin + "_" + HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString + "_"

      Dim twExcel As XmlTextWriter
      Dim twHtml As XmlTextWriter

      If HttpContext.Current.Session.Item("debug") Then

        ' generate new filename for temp file
        sFileName = commonEvo.GenerateFileName(subscriptionInfo + "tmpExcel_XML", ".xml", False)

        twExcel = New XmlTextWriter(reportFolder + "\" + sFileName, System.Text.Encoding.UTF8)
        twExcel.Formatting = Formatting.Indented
        twExcel.Indentation = 2
        twExcel.WriteStartDocument()

        If HttpContext.Current.Session.Item("localPreferences").UseMetricValues Then
          twExcel.WriteProcessingInstruction("xml-stylesheet", "type='text/xsl' href='" + HttpContext.Current.Session.Item("jetnetFullHostName").ToString.Trim + reportDisplayFolder + "/OperatingCostsEXCELmetric.xslt'")
        Else
          If HttpContext.Current.Session.Item("localPreferences").UseStatuteMile Then
            twExcel.WriteProcessingInstruction("xml-stylesheet", "type='text/xsl' href='" + HttpContext.Current.Session.Item("jetnetFullHostName").ToString.Trim + reportDisplayFolder + "/OperatingCostsEXCELstandard.xslt'")
          Else
            twExcel.WriteProcessingInstruction("xml-stylesheet", "type='text/xsl' href='" + HttpContext.Current.Session.Item("jetnetFullHostName").ToString.Trim + reportDisplayFolder + "/OperatingCostsEXCELnautical.xslt'")
          End If
        End If

        ' write the dataset out to xml file
        ds.WriteXml(twExcel)

        twExcel.Close()

        ' generate new filename for temp file
        sFileName = commonEvo.GenerateFileName(subscriptionInfo + "tmpHtml_XML", ".xml", False)

        twHtml = New XmlTextWriter(reportFolder + "\" + sFileName, System.Text.Encoding.UTF8)
        twHtml.Formatting = Formatting.Indented
        twHtml.Indentation = 2
        twHtml.WriteStartDocument()

        If HttpContext.Current.Session.Item("localPreferences").UseMetricValues Then
          twHtml.WriteProcessingInstruction("xml-stylesheet", "type='text/xsl' href='" + HttpContext.Current.Session.Item("jetnetFullHostName").ToString.Trim + reportDisplayFolder + "/OperatingCostsHTMLmetric.xslt'")
        Else
          If HttpContext.Current.Session.Item("localPreferences").UseStatuteMile Then
            twHtml.WriteProcessingInstruction("xml-stylesheet", "type='text/xsl' href='" + HttpContext.Current.Session.Item("jetnetFullHostName").ToString.Trim + reportDisplayFolder + "/OperatingCostsHTMLstandard.xslt'")
          Else
            twHtml.WriteProcessingInstruction("xml-stylesheet", "type='text/xsl' href='" + HttpContext.Current.Session.Item("jetnetFullHostName").ToString.Trim + reportDisplayFolder + "/OperatingCostsHTMLnautical.xslt'")
          End If
        End If

        ' write the dataset out to xml file
        ds.WriteXml(twHtml)

        twHtml.Close()

      End If

      'Second, transform the DataSet XML and save it to a file.
      Dim xmlDoc As XmlDataDocument = New XmlDataDocument(ds)

      ' Create the XsltSettings object with script enabled. 
      Dim settings As New XsltSettings(False, True)
      Dim xslTran As New XslCompiledTransform()

      ' Execute the transform. 
      If HttpContext.Current.Session.Item("localPreferences").UseMetricValues Then
        xslTran.Load(reportFolder + "\" + "OperatingCostsEXCELmetric.xslt", settings, New XmlUrlResolver())
      Else
        If HttpContext.Current.Session.Item("localPreferences").UseStatuteMile Then
          xslTran.Load(reportFolder + "\" + "OperatingCostsEXCELstandard.xslt", settings, New XmlUrlResolver())
        Else
          xslTran.Load(reportFolder + "\" + "OperatingCostsEXCELnautical.xslt", settings, New XmlUrlResolver())
        End If
      End If

      sFileName = sOpCostsFileName

      twExcel = New XmlTextWriter(reportFolder + "\" + sFileName, System.Text.Encoding.UTF8)
      twExcel.Formatting = Formatting.Indented
      twExcel.Indentation = 2
      twExcel.WriteStartDocument()

      xslTran.Transform(xmlDoc, twExcel)

      twExcel.Close()

      ' Execute the transform. 
      If HttpContext.Current.Session.Item("localPreferences").UseMetricValues Then
        xslTran.Load(reportFolder + "\" + "OperatingCostsHTMLmetric.xslt", settings, New XmlUrlResolver())
      Else
        If HttpContext.Current.Session.Item("localPreferences").UseStatuteMile Then
          xslTran.Load(reportFolder + "\" + "OperatingCostsHTMLstandard.xslt", settings, New XmlUrlResolver())
        Else
          xslTran.Load(reportFolder + "\" + "OperatingCostsHTMLnautical.xslt", settings, New XmlUrlResolver())
        End If
      End If

      sFileName = commonEvo.GenerateFileName(sOpCostsFileName, ".html", True)

      twHtml = New XmlTextWriter(reportFolder + "\" + sFileName, System.Text.Encoding.UTF8)
      twHtml.Formatting = Formatting.Indented
      twHtml.Indentation = 2

      twHtml.WriteStartDocument()
      twHtml.WriteDocType("html", "-//w3c//dtd html 4.0 transitional//en", Nothing, Nothing)

      xslTran.Transform(xmlDoc, twHtml)

      twHtml.Close()

      twExcel = Nothing
      twHtml = Nothing

    Catch ex As Exception

      aError += "error in outputOpcostsResults(ByVal ds As DataSet) As Boolean : " + ex.Message
      Return False

    End Try

    Return True

  End Function

  Private Function generate_direct_cost_values(ByRef inDataSetRow As DataRow, ByRef opCostsRow As opCostsClass) As Boolean

    Try

      opCostsRow.evoCurrencyName = sCurrencyName
      opCostsRow.evoCurrencySymbol = sCurrencySymbol
      opCostsRow.evoCurrencyDate = sCurrencyDate

      If Not IsDBNull(inDataSetRow.Item("amod_id")) Then
        opCostsRow.evoModelID = CDbl(inDataSetRow.Item("amod_id").ToString)
      End If

      If Not String.IsNullOrEmpty(inDataSetRow.Item("amod_make_name").ToString) Then
        opCostsRow.evoMakeModelName = inDataSetRow.Item("amod_make_name").ToString
      End If
      If Not String.IsNullOrEmpty(inDataSetRow.Item("amod_model_name").ToString) Then
        opCostsRow.evoMakeModelName &= " " + inDataSetRow.Item("amod_model_name").ToString
      End If

      If HttpContext.Current.Session.Item("localPreferences").UseMetricValues Then

        If Not IsDBNull(inDataSetRow.Item("amod_fuel_gal_cost")) And CDbl(HttpContext.Current.Session.Item("fuelPriceBase")) = 0 Then
          If CDbl(inDataSetRow.Item("amod_fuel_gal_cost").ToString) Then
            opCostsRow.fuelGalCost = CDbl(ConversionFunctions.ConvertUSToMetricValue("PPG", CDbl(inDataSetRow.Item("amod_fuel_gal_cost").ToString)))
          End If
        Else
          If CDbl(HttpContext.Current.Session.Item("fuelPriceBase").ToString) > 0 Then
            opCostsRow.fuelGalCost = CDbl(ConversionFunctions.ConvertUSToMetricValue("PPG", CDbl(HttpContext.Current.Session.Item("fuelPriceBase").ToString)))
          End If
        End If

        If Not IsDBNull(inDataSetRow.Item("amod_fuel_add_cost")) Then
          opCostsRow.fuelAddCost = CDbl(ConversionFunctions.ConvertUSToMetricValue("PPG", CDbl(inDataSetRow.Item("amod_fuel_add_cost").ToString)))
        End If

        If Not IsDBNull(inDataSetRow.Item("amod_fuel_burn_rate")) Then
          opCostsRow.fuelBurnRate = CDbl(ConversionFunctions.ConvertUSToMetricValue("GAL", CDbl(inDataSetRow.Item("amod_fuel_burn_rate").ToString)))
        End If

        If bChangeCurrency Then
          opCostsRow.fuelGalCost = CDbl(ConversionFunctions.ConvertUSToForeignCurrency(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate, opCostsRow.fuelGalCost))
        End If

        If bChangeCurrency Then
          opCostsRow.fuelAddCost = CDbl(ConversionFunctions.ConvertUSToForeignCurrency(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate, opCostsRow.fuelAddCost))
        End If

        ' calculate totals
        opCostsRow.calcFuelTotalCost = CDbl(Math.Round(CDbl((opCostsRow.fuelGalCost + opCostsRow.fuelAddCost) * opCostsRow.fuelBurnRate), 2))

      Else

        If Not IsDBNull(inDataSetRow.Item("amod_fuel_gal_cost")) And CDbl(HttpContext.Current.Session.Item("fuelPriceBase")) = 0 Then
          If CDbl(inDataSetRow.Item("amod_fuel_gal_cost").ToString) Then
            opCostsRow.fuelGalCost = CDbl(inDataSetRow.Item("amod_fuel_gal_cost").ToString)
          End If
        Else
          If CDbl(HttpContext.Current.Session.Item("fuelPriceBase").ToString) > 0 Then
            opCostsRow.fuelGalCost = CDbl(HttpContext.Current.Session.Item("fuelPriceBase").ToString)
          End If
        End If

        If Not IsDBNull(inDataSetRow.Item("amod_fuel_add_cost")) Then
          opCostsRow.fuelAddCost = CDbl(inDataSetRow.Item("amod_fuel_add_cost").ToString)
        End If

        If Not IsDBNull(inDataSetRow.Item("amod_fuel_burn_rate")) Then
          opCostsRow.fuelBurnRate = CDbl(inDataSetRow.Item("amod_fuel_burn_rate").ToString)
        End If

        If bChangeCurrency Then
          opCostsRow.fuelGalCost = CDbl(ConversionFunctions.ConvertUSToForeignCurrency(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate, opCostsRow.fuelGalCost))
        End If

        If bChangeCurrency Then
          opCostsRow.fuelAddCost = CDbl(ConversionFunctions.ConvertUSToForeignCurrency(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate, opCostsRow.fuelAddCost))
        End If

        ' calculate totals
        opCostsRow.calcFuelTotalCost = CDbl(Math.Round(CDbl((opCostsRow.fuelGalCost + opCostsRow.fuelAddCost) * opCostsRow.fuelBurnRate), 2))

      End If


      If Not IsDBNull(inDataSetRow.Item("amod_maint_lab_cost")) Then
        opCostsRow.maintLaborCost = CDbl(inDataSetRow.Item("amod_maint_lab_cost").ToString)
      End If

      If Not IsDBNull(inDataSetRow.Item("amod_maint_parts_cost")) Then
        opCostsRow.maintPartsCost = CDbl(inDataSetRow.Item("amod_maint_parts_cost").ToString)
      End If

      If Not IsDBNull(inDataSetRow.Item("amod_maint_labor_cost_man_hours_multiplier")) Then
        opCostsRow.maintLaborCostManHour = CDbl(inDataSetRow.Item("amod_maint_labor_cost_man_hours_multiplier").ToString) * opCostsRow.maintLaborCost
      End If

      If Not IsDBNull(inDataSetRow.Item("amod_maint_parts_cost_man_hours_multiplier")) Then
        opCostsRow.maintPartsCostManHour = CDbl(inDataSetRow.Item("amod_maint_parts_cost_man_hours_multiplier").ToString) * opCostsRow.maintPartsCost
      End If

      ' calculate totals
      opCostsRow.calcMaintTotalCost = CDbl(Math.Round(CDbl(opCostsRow.maintLaborCost + opCostsRow.maintPartsCost), 2))

      If bChangeCurrency Then
        opCostsRow.calcMaintTotalCost = CDbl(Math.Round(ConversionFunctions.ConvertUSToForeignCurrency(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString, opCostsRow.calcMaintTotalCost), 2))
      End If

      If bChangeCurrency Then
        opCostsRow.maintLaborCost = CDbl(Math.Round(ConversionFunctions.ConvertUSToForeignCurrency(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString, opCostsRow.maintLaborCost), 2))
      End If

      If bChangeCurrency Then
        opCostsRow.maintPartsCost = CDbl(Math.Round(ConversionFunctions.ConvertUSToForeignCurrency(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString, opCostsRow.maintPartsCost), 2))
      End If

      If Not IsDBNull(inDataSetRow.Item("amod_engine_ovh_cost")) Then
        opCostsRow.maintEngineCost = CDbl(inDataSetRow.Item("amod_engine_ovh_cost").ToString)
      End If

      If bChangeCurrency Then
        opCostsRow.maintEngineCost = CDbl(Math.Round(ConversionFunctions.ConvertUSToForeignCurrency(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString, opCostsRow.maintEngineCost), 2))
      End If

      If Not IsDBNull(inDataSetRow.Item("amod_thrust_rev_ovh_cost")) Then
        opCostsRow.maintThrustCost = CDbl(inDataSetRow.Item("amod_thrust_rev_ovh_cost").ToString)
      End If

      If bChangeCurrency Then
        opCostsRow.maintThrustCost = CDbl(Math.Round(ConversionFunctions.ConvertUSToForeignCurrency(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString, opCostsRow.maintThrustCost), 2))
      End If

      If Not IsDBNull(inDataSetRow.Item("amod_land_park_cost")) Then
        opCostsRow.miscLandParkCost = CDbl(inDataSetRow.Item("amod_land_park_cost").ToString)
      End If

      If Not IsDBNull(inDataSetRow.Item("amod_crew_exp_cost")) Then
        opCostsRow.miscCrewCost = CDbl(inDataSetRow.Item("amod_crew_exp_cost").ToString)
      End If

      If Not IsDBNull(inDataSetRow.Item("amod_supplies_cost")) Then
        opCostsRow.miscSupplyCost = CDbl(inDataSetRow.Item("amod_supplies_cost").ToString)
      End If

      ' calculate totals
      opCostsRow.calcMiscFlightTotalCost = CDbl(Math.Round(CDbl(opCostsRow.miscLandParkCost + opCostsRow.miscCrewCost + opCostsRow.miscSupplyCost), 2))

      If bChangeCurrency Then
        opCostsRow.calcMiscFlightTotalCost = CDbl(Math.Round(ConversionFunctions.ConvertUSToForeignCurrency(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString, opCostsRow.calcMiscFlightTotalCost), 2))
      End If

      If bChangeCurrency Then
        opCostsRow.miscLandParkCost = CDbl(Math.Round(ConversionFunctions.ConvertUSToForeignCurrency(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString, opCostsRow.miscLandParkCost), 2))
      End If

      If bChangeCurrency Then
        opCostsRow.miscCrewCost = CDbl(Math.Round(ConversionFunctions.ConvertUSToForeignCurrency(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString, opCostsRow.miscCrewCost), 2))
      End If

      If bChangeCurrency Then
        opCostsRow.miscSupplyCost = CDbl(Math.Round(ConversionFunctions.ConvertUSToForeignCurrency(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString, opCostsRow.miscSupplyCost), 2))
      End If

      ' calculate totals
      opCostsRow.calcTotalDirCostHour = CDbl(opCostsRow.calcFuelTotalCost + opCostsRow.calcMaintTotalCost + opCostsRow.calcMiscFlightTotalCost + opCostsRow.maintEngineCost + opCostsRow.maintThrustCost)

      If Not IsDBNull(inDataSetRow.Item("amod_avg_block_speed")) Then
        If HttpContext.Current.Session.Item("localPreferences").UseMetricValues Then
          opCostsRow.avgBlockSpeed = CDbl(ConversionFunctions.ConvertUSToMetricValue("SM", CDbl(inDataSetRow.Item("amod_avg_block_speed").ToString)))
        Else
          If HttpContext.Current.Session.Item("localPreferences").UseStatuteMile Then
            opCostsRow.avgBlockSpeed = CDbl(inDataSetRow.Item("amod_avg_block_speed").ToString)
          Else
            opCostsRow.avgBlockSpeed = CDbl(ConversionFunctions.ConvertStatuteToNauticalMiles("SM", CDbl(inDataSetRow.Item("amod_avg_block_speed").ToString)))
          End If
        End If
      End If

      If opCostsRow.avgBlockSpeed > 0 Then
        opCostsRow.calcTotalCostPerMile = CDbl(opCostsRow.calcTotalDirCostHour / opCostsRow.avgBlockSpeed)
      End If

    Catch ex As Exception
      aError += "error in generate_direct_cost_values(ByRef inDataSetRow As DataRow, ByRef opCostsRow As opCostsClass) As Boolean : " + ex.Message
      Return False
    End Try

    Return True

  End Function

  Private Function generate_annual_fixed_values(ByRef inDataSetRow As DataRow, ByRef opCostsRow As opCostsClass) As Boolean

    Try

      If Not IsDBNull(inDataSetRow.Item("amod_capt_salary_cost")) Then
        opCostsRow.captSalaryCost = CDbl(inDataSetRow.Item("amod_capt_salary_cost").ToString)
      End If

      If Not IsDBNull(inDataSetRow.Item("amod_cpilot_salary_cost")) Then
        opCostsRow.coPilotSalaryCost = CDbl(inDataSetRow.Item("amod_cpilot_salary_cost").ToString)
      End If

      If Not IsDBNull(inDataSetRow.Item("amod_crew_benefit_cost")) Then
        opCostsRow.benefitsCost = CDbl(inDataSetRow.Item("amod_crew_benefit_cost").ToString)
      End If

      ' calculate totals
      opCostsRow.calcCrewTotalCost = CDbl(Math.Round(CDbl(opCostsRow.captSalaryCost + opCostsRow.coPilotSalaryCost + opCostsRow.benefitsCost), 0))

      If bChangeCurrency Then
        opCostsRow.calcCrewTotalCost = CDbl(Math.Round(ConversionFunctions.ConvertUSToForeignCurrency(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString, opCostsRow.calcCrewTotalCost), 0))
      End If

      If bChangeCurrency Then
        opCostsRow.coPilotSalaryCost = CDbl(Math.Round(ConversionFunctions.ConvertUSToForeignCurrency(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString, opCostsRow.coPilotSalaryCost), 2))
      End If

      If bChangeCurrency Then
        opCostsRow.captSalaryCost = CDbl(Math.Round(ConversionFunctions.ConvertUSToForeignCurrency(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString, opCostsRow.captSalaryCost), 2))
      End If

      If bChangeCurrency Then
        opCostsRow.benefitsCost = CDbl(Math.Round(ConversionFunctions.ConvertUSToForeignCurrency(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString, opCostsRow.benefitsCost), 2))
      End If

      If Not IsDBNull(inDataSetRow.Item("amod_hangar_cost")) Then
        opCostsRow.hangarCost = CDbl(inDataSetRow.Item("amod_hangar_cost").ToString)
      End If

      If bChangeCurrency Then
        opCostsRow.hangarCost = CDbl(Math.Round(ConversionFunctions.ConvertUSToForeignCurrency(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString, opCostsRow.hangarCost), 0))
      End If

      If Not IsDBNull(inDataSetRow.Item("amod_hull_insurance_cost")) Then
        opCostsRow.insuranceHullCost = CDbl(inDataSetRow.Item("amod_hull_insurance_cost").ToString)
      End If

      If Not IsDBNull(inDataSetRow.Item("amod_liability_insurance_cost")) Then
        opCostsRow.insuranceLiabilityCost = CDbl(inDataSetRow.Item("amod_liability_insurance_cost").ToString)
      End If

      ' calculate totals
      opCostsRow.calcInsuranceTotalCost = CDbl(Math.Round(CDbl(opCostsRow.insuranceHullCost + opCostsRow.insuranceLiabilityCost), 0))

      If bChangeCurrency Then
        opCostsRow.calcInsuranceTotalCost = CDbl(Math.Round(ConversionFunctions.ConvertUSToForeignCurrency(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString, opCostsRow.calcInsuranceTotalCost), 0))
      End If

      If bChangeCurrency Then
        opCostsRow.insuranceHullCost = CDbl(Math.Round(ConversionFunctions.ConvertUSToForeignCurrency(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString, opCostsRow.insuranceHullCost), 2))
      End If

      If bChangeCurrency Then
        opCostsRow.insuranceLiabilityCost = CDbl(Math.Round(ConversionFunctions.ConvertUSToForeignCurrency(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString, opCostsRow.insuranceLiabilityCost), 2))
      End If

      If Not IsDBNull(inDataSetRow.Item("amod_misc_train_cost")) Then
        opCostsRow.miscTrainCost = CDbl(inDataSetRow.Item("amod_misc_train_cost").ToString)
      End If

      If Not IsDBNull(inDataSetRow.Item("amod_misc_modern_cost")) Then
        opCostsRow.miscModernCost = CDbl(inDataSetRow.Item("amod_misc_modern_cost").ToString)
      End If

      If Not IsDBNull(inDataSetRow.Item("amod_misc_naveq_cost")) Then
        opCostsRow.miscNavCost = CDbl(inDataSetRow.Item("amod_misc_naveq_cost").ToString)
      End If

      ' calculate totals
      opCostsRow.calcMiscTotalCost = CDbl(Math.Round(CDbl(opCostsRow.miscTrainCost + opCostsRow.miscModernCost + opCostsRow.miscNavCost), 0))

      If bChangeCurrency Then
        opCostsRow.calcMiscTotalCost = CDbl(Math.Round(ConversionFunctions.ConvertUSToForeignCurrency(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString, opCostsRow.calcMiscTotalCost), 0))
      End If

      If bChangeCurrency Then
        opCostsRow.miscTrainCost = CDbl(Math.Round(ConversionFunctions.ConvertUSToForeignCurrency(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString, opCostsRow.miscTrainCost), 2))
      End If

      If bChangeCurrency Then
        opCostsRow.miscModernCost = CDbl(Math.Round(ConversionFunctions.ConvertUSToForeignCurrency(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString, opCostsRow.miscModernCost), 2))
      End If

      If bChangeCurrency Then
        opCostsRow.miscNavCost = CDbl(Math.Round(ConversionFunctions.ConvertUSToForeignCurrency(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString, opCostsRow.miscNavCost), 2))
      End If

      If Not IsDBNull(inDataSetRow.Item("amod_deprec_cost")) Then
        opCostsRow.depreciationCost = CDbl(inDataSetRow.Item("amod_deprec_cost").ToString)
      End If

      If bChangeCurrency Then
        opCostsRow.depreciationCost = CDbl(Math.Round(ConversionFunctions.ConvertUSToForeignCurrency(HttpContext.Current.Session.Item("localPreferences").CurrencyExchangeRate.ToString, opCostsRow.depreciationCost), 0))
      End If

      ' calculate totals
      opCostsRow.calcTotalFixedCosts = CDbl(Math.Round(CDbl(opCostsRow.calcCrewTotalCost + opCostsRow.hangarCost + opCostsRow.calcInsuranceTotalCost + opCostsRow.calcMiscTotalCost + opCostsRow.depreciationCost), 0))

    Catch ex As Exception
      aError += "error in generate_annual_fixed_values(ByRef inDataSetRow As DataRow, ByRef opCostsRow As opCostsClass) As Boolean : " + ex.Message
      Return False
    End Try

    Return True

  End Function

  Private Function generate_annual_budget_values(ByRef inDataSetRow As DataRow, ByRef opCostsRow As opCostsClass) As Boolean

    Try

      If Not IsDBNull(inDataSetRow.Item("amod_number_of_seats")) Then
        If (CLng(inDataSetRow.Item("amod_number_of_seats").ToString) > 0) Then
          opCostsRow.numberOfSeats = CInt(inDataSetRow.Item("amod_number_of_seats").ToString)
        End If
      End If

      If Not IsDBNull(inDataSetRow.Item("amod_annual_miles")) Then
        opCostsRow.annualMiles = CInt(inDataSetRow.Item("amod_annual_miles").ToString)

        If HttpContext.Current.Session.Item("localPreferences").UseMetricValues Then
          opCostsRow.annualMiles = CDbl(Math.Round(ConversionFunctions.ConvertUSToMetricValue("M", CDbl(opCostsRow.annualMiles)), 0))
        Else
          If Not HttpContext.Current.Session.Item("localPreferences").UseStatuteMile Then
            opCostsRow.annualMiles = CDbl(Math.Round(ConversionFunctions.ConvertStatuteToNauticalMiles("SM", CDbl(opCostsRow.annualMiles)), 0))
          End If
        End If
      End If

      ' calculate totals
      If opCostsRow.avgBlockSpeed > 0 Then
        opCostsRow.calcAnnualHrs = CDbl(Math.Round(CDbl(opCostsRow.annualMiles / opCostsRow.avgBlockSpeed), 0))
      End If

      If opCostsRow.calcAnnualHrs > 0 Then
        opCostsRow.calcTotalDirCostYR = Math.Round(CDbl(opCostsRow.calcTotalDirCostHour * opCostsRow.calcAnnualHrs), 3)
      End If

      opCostsRow.calcTotalFixedDirect = Math.Round(CDbl(opCostsRow.calcTotalDirCostYR + opCostsRow.calcTotalFixedCosts), 3)

      If opCostsRow.calcAnnualHrs > 0 Then
        opCostsRow.calcCostPerHourFixDir = Math.Round(CDbl(opCostsRow.calcTotalFixedDirect / opCostsRow.calcAnnualHrs), 3)
      End If

      If opCostsRow.annualMiles > 0 Then
        opCostsRow.calcCostPerMileFixDir = Math.Round(CDbl(opCostsRow.calcTotalFixedDirect / opCostsRow.annualMiles), 3)
      End If

      If opCostsRow.numberOfSeats > 0 Then
        opCostsRow.calcCostPerSeatFixDir = Math.Round(CDbl(opCostsRow.calcCostPerMileFixDir / opCostsRow.numberOfSeats), 3)
      End If

      opCostsRow.calcNoDepTotalCost = Math.Round(CDbl(opCostsRow.calcTotalFixedDirect - opCostsRow.depreciationCost), 3)

      If opCostsRow.calcAnnualHrs > 0 Then
        opCostsRow.calcCostPerHourNoDep = Math.Round(CDbl(opCostsRow.calcNoDepTotalCost / opCostsRow.calcAnnualHrs), 3)
      End If

      If opCostsRow.annualMiles > 0 Then
        opCostsRow.calcCostPerMileNoDep = Math.Round(CDbl(opCostsRow.calcNoDepTotalCost / opCostsRow.annualMiles), 3)
      End If

      If opCostsRow.numberOfSeats > 0 Then
        opCostsRow.calcCostPerSeatNoDep = Math.Round(CDbl(opCostsRow.calcCostPerMileNoDep / opCostsRow.numberOfSeats), 3)
      End If

      If Not IsDBNull(inDataSetRow.Item("amod_variable_costs")) Then
        opCostsRow.variableTotalCost = CDbl(inDataSetRow.Item("amod_variable_costs").ToString)
      End If

    Catch ex As Exception
      aError += "error in generate_annual_budget_values(ByRef inDataSetRow As DataRow, ByRef opCostsRow As opCostsClass) As Boolean : " + ex.Message
      Return False
    End Try

    Return True
  End Function

  Private Function get_op_costs_info() As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT * FROM Aircraft_Model WITH(NOLOCK) INNER JOIN Aircraft_Type WITH(NOLOCK) ON amod_type_code = atype_code")
      sQuery.Append(" WHERE ")

      If Not String.IsNullOrEmpty(sOpCostsModelIDList.Trim) Then
        sQuery.Append("amod_id IN (" + sOpCostsModelIDList.Trim + ")")
      ElseIf nOpCostsModelID > -1 Then
        sQuery.Append("amod_id = " + nOpCostsModelID.ToString)
      End If

      sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True))

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_op_costs_info() As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString
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
        aError += "Error in get_op_costs_info load datatable" + constrExc.Message
      End Try

    Catch ex As Exception

      aError += "error in get_op_costs_info() As DataTable : " + ex.Message
      Return Nothing

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

  Private Function loadReportDataTable(ByRef dtOpCosts As DataTable, ByRef outOpCostsArray() As opCostsClass) As Boolean

    Dim results_table As New DataTable
    Dim bReturnValue As Boolean = False

    Try

      dtOpCosts = get_op_costs_info()
      If Not IsNothing(dtOpCosts) Then
        If dtOpCosts.Rows.Count > 0 Then ' we have 1 or more aircraft
          ' set the size of the array to match recordset count
          ReDim outOpCostsArray(dtOpCosts.Rows.Count - 1)
          bReturnValue = True
        End If
      End If


    Catch ex As Exception ' catch exe exception
      aError += "error in loadReportDataTable(ByRef dtOpCosts As DataTable, ByRef outOpCostsArray() As opCostsClass) As Boolean : " + ex.Message
    End Try

    Return bReturnValue

  End Function

  Private Function processOpCostsDataSet(ByRef dtOpCosts As DataTable, ByRef outOpCostsArray() As opCostsClass) As Boolean

    Try

      If Not IsNothing(dtOpCosts) Then
        If dtOpCosts.Rows.Count > 0 Then

          For a As Integer = 0 To dtOpCosts.Rows.Count - 1

            outOpCostsArray(a) = New opCostsClass

            generate_direct_cost_values(dtOpCosts.Rows(a), outOpCostsArray(a)) ' DIRECTCOST
            generate_annual_fixed_values(dtOpCosts.Rows(a), outOpCostsArray(a)) ' ANNUALFIXED
            generate_annual_budget_values(dtOpCosts.Rows(a), outOpCostsArray(a)) ' ANNUALBUDGET

          Next

        End If
      End If

    Catch ex As Exception
      aError += "error in processOpCostsDataSet(ByRef dtOpCosts As DataTable, ByRef outOpCostsArray() As opCostsClass) As Boolean : " + ex.Message
      Return False
    End Try

    Return True

  End Function

  Private Function convertOpCostsClassToDataset(ByRef outOpCostsArray() As opCostsClass, ByRef dsOpCosts As DataSet) As Boolean

    Try

      ' create Customer table 
      Dim opCostsDT As DataTable = New DataTable("opCosts")

      opCostsDT.Columns.Add("amodId", Type.GetType("System.Int32"))
      opCostsDT.Columns.Add("modelName", Type.GetType("System.String"))

      opCostsDT.Columns.Add("styleLink", Type.GetType("System.String"))

      opCostsDT.Columns.Add("exchangeRateDate", Type.GetType("System.String"))
      opCostsDT.Columns.Add("currencySymbol", Type.GetType("System.String"))

      opCostsDT.Columns.Add("fuelGalCost", Type.GetType("System.String"))
      opCostsDT.Columns.Add("fuelAddCost", Type.GetType("System.String"))
      opCostsDT.Columns.Add("fuelBurnRate", Type.GetType("System.String"))
      opCostsDT.Columns.Add("fuelTotalCost", Type.GetType("System.String"))

      opCostsDT.Columns.Add("maintLaborCost", Type.GetType("System.String"))
      opCostsDT.Columns.Add("maintPartsCost", Type.GetType("System.String"))
      opCostsDT.Columns.Add("maintLaborCostManHour", Type.GetType("System.String"))
      opCostsDT.Columns.Add("maintPartsCostManHour", Type.GetType("System.String"))
      opCostsDT.Columns.Add("maintTotalCost", Type.GetType("System.String"))

      opCostsDT.Columns.Add("maintEngineCost", Type.GetType("System.String"))
      opCostsDT.Columns.Add("maintThrustCost", Type.GetType("System.String"))

      opCostsDT.Columns.Add("miscLandParkCost", Type.GetType("System.String"))
      opCostsDT.Columns.Add("miscCrewCost", Type.GetType("System.String"))
      opCostsDT.Columns.Add("miscSupplyCost", Type.GetType("System.String"))
      opCostsDT.Columns.Add("miscFlightTotalCost", Type.GetType("System.String"))

      opCostsDT.Columns.Add("totalDirCostHour", Type.GetType("System.String"))
      opCostsDT.Columns.Add("avgBlockSpeed", Type.GetType("System.String"))
      opCostsDT.Columns.Add("totalCostPerMile", Type.GetType("System.String"))

      opCostsDT.Columns.Add("captSalaryCost", Type.GetType("System.String"))
      opCostsDT.Columns.Add("coPilotSalaryCost", Type.GetType("System.String"))
      opCostsDT.Columns.Add("benefitsCost", Type.GetType("System.String"))
      opCostsDT.Columns.Add("captSalaryCostRaw", Type.GetType("System.String"))
      opCostsDT.Columns.Add("coPilotSalaryCostRaw", Type.GetType("System.String"))
      opCostsDT.Columns.Add("benefitsCostRaw", Type.GetType("System.String"))
      opCostsDT.Columns.Add("crewTotalCost", Type.GetType("System.String"))

      opCostsDT.Columns.Add("hangarCost", Type.GetType("System.String"))

      opCostsDT.Columns.Add("insuranceHullCost", Type.GetType("System.String"))
      opCostsDT.Columns.Add("insuranceLiabilityCost", Type.GetType("System.String"))
      opCostsDT.Columns.Add("insuranceHullCostRaw", Type.GetType("System.String"))
      opCostsDT.Columns.Add("insuranceLiabilityCostRaw", Type.GetType("System.String"))
      opCostsDT.Columns.Add("insuranceTotalCost", Type.GetType("System.String"))

      opCostsDT.Columns.Add("miscTrainCost", Type.GetType("System.String"))
      opCostsDT.Columns.Add("miscModernCost", Type.GetType("System.String"))
      opCostsDT.Columns.Add("miscNavCost", Type.GetType("System.String"))
      opCostsDT.Columns.Add("miscTrainCostRaw", Type.GetType("System.String"))
      opCostsDT.Columns.Add("miscModernCostRaw", Type.GetType("System.String"))
      opCostsDT.Columns.Add("miscNavCostRaw", Type.GetType("System.String"))
      opCostsDT.Columns.Add("miscTotalCost", Type.GetType("System.String"))

      opCostsDT.Columns.Add("depreciationCost", Type.GetType("System.String"))
      opCostsDT.Columns.Add("depreciationCostRaw", Type.GetType("System.String"))
      opCostsDT.Columns.Add("totalFixedCosts", Type.GetType("System.String"))
      opCostsDT.Columns.Add("variableTotalCost", Type.GetType("System.String"))

      opCostsDT.Columns.Add("numberOfSeats", Type.GetType("System.String"))
      opCostsDT.Columns.Add("annualMiles", Type.GetType("System.String"))
      opCostsDT.Columns.Add("annualHrs", Type.GetType("System.String"))

      opCostsDT.Columns.Add("totalDirCostYR", Type.GetType("System.String"))

      opCostsDT.Columns.Add("totalFixedDirect", Type.GetType("System.String"))

      opCostsDT.Columns.Add("costPerHourFixDir", Type.GetType("System.String"))
      opCostsDT.Columns.Add("costPerMileFixDir", Type.GetType("System.String"))
      opCostsDT.Columns.Add("costPerSeatFixDir", Type.GetType("System.String"))

      opCostsDT.Columns.Add("noDepTotalCost", Type.GetType("System.String"))

      opCostsDT.Columns.Add("costPerHourNoDep", Type.GetType("System.String"))
      opCostsDT.Columns.Add("costPerMileNoDep", Type.GetType("System.String"))
      opCostsDT.Columns.Add("costPerSeatNoDep", Type.GetType("System.String"))

      opCostsDT.PrimaryKey = New DataColumn() {opCostsDT.Columns("amodId")}

      dsOpCosts.Tables.Add(opCostsDT)

      For a As Integer = 0 To UBound(outOpCostsArray)

        Dim newOpCostsRow As DataRow = dsOpCosts.Tables("opCosts").NewRow()

        newOpCostsRow("amodId") = outOpCostsArray(a).evoModelID
        newOpCostsRow("modelName") = outOpCostsArray(a).evoMakeModelName

        If CInt(HttpContext.Current.Session.Item("localPreferences").DefaultCurrency.ToString) <> 9 Then ' 9 = us dollar
          newOpCostsRow("exchangeRateDate") = sRptTitle
        Else
          newOpCostsRow("exchangeRateDate") = "Dollar (US)"
        End If

        newOpCostsRow("currencySymbol") = sCurrencySymbol

        newOpCostsRow("styleLink") = HttpContext.Current.Session.Item("MarketSummaryFolderVirtualPath").ToString + "/OperatingCostsHTML.css"

        ' DIRECTCOST
        newOpCostsRow("fuelGalCost") = FormatNumber(outOpCostsArray(a).fuelGalCost, 2, True, False, True).ToString
        newOpCostsRow("fuelAddCost") = FormatNumber(outOpCostsArray(a).fuelAddCost, 2, True, False, True).ToString
        newOpCostsRow("fuelBurnRate") = FormatNumber(outOpCostsArray(a).fuelBurnRate, 2, True, False, False).ToString
        newOpCostsRow("fuelTotalCost") = FormatNumber(outOpCostsArray(a).calcFuelTotalCost, 2, True, False, True).ToString

        newOpCostsRow("maintLaborCost") = FormatNumber(outOpCostsArray(a).maintLaborCost, 2, True, False, True).ToString
        newOpCostsRow("maintPartsCost") = FormatNumber(outOpCostsArray(a).maintPartsCost, 2, True, False, True).ToString
        newOpCostsRow("maintLaborCostManHour") = ConversionFunctions.Truncate(outOpCostsArray(a).maintLaborCostManHour, 2)
        newOpCostsRow("maintPartsCostManHour") = ConversionFunctions.Truncate(outOpCostsArray(a).maintPartsCostManHour, 2)
        newOpCostsRow("maintTotalCost") = FormatNumber(outOpCostsArray(a).calcMaintTotalCost, 2, True, False, True).ToString

        newOpCostsRow("maintEngineCost") = FormatNumber(outOpCostsArray(a).maintEngineCost, 2, True, False, True).ToString
        newOpCostsRow("maintThrustCost") = FormatNumber(outOpCostsArray(a).maintThrustCost, 2, True, False, True).ToString

        newOpCostsRow("miscLandParkCost") = FormatNumber(outOpCostsArray(a).miscLandParkCost, 2, True, False, True).ToString
        newOpCostsRow("miscCrewCost") = FormatNumber(outOpCostsArray(a).miscCrewCost, 2, True, False, True).ToString
        newOpCostsRow("miscSupplyCost") = FormatNumber(outOpCostsArray(a).miscSupplyCost, 2, True, False, True).ToString
        newOpCostsRow("miscFlightTotalCost") = FormatNumber(outOpCostsArray(a).calcMiscFlightTotalCost, 2, True, False, True).ToString

        newOpCostsRow("totalDirCostHour") = FormatNumber(outOpCostsArray(a).calcTotalDirCostHour, 2, True, False, True).ToString
        newOpCostsRow("avgBlockSpeed") = FormatNumber(outOpCostsArray(a).avgBlockSpeed, 0, True, False, True).ToString
        newOpCostsRow("totalCostPerMile") = FormatNumber(ConversionFunctions.Truncate(outOpCostsArray(a).calcTotalCostPerMile, 0), 2, True, False, True).ToString

        ' ANNUALFIXED
        newOpCostsRow("captSalaryCostRaw") = FormatNumber(outOpCostsArray(a).captSalaryCost, 2, True, False, True).ToString
        newOpCostsRow("coPilotSalaryCostRaw") = FormatNumber(outOpCostsArray(a).coPilotSalaryCost, 2, True, False, True).ToString
        newOpCostsRow("benefitsCostRaw") = FormatNumber(outOpCostsArray(a).benefitsCost, 2, True, False, True).ToString

        newOpCostsRow("captSalaryCost") = FormatNumber(outOpCostsArray(a).captSalaryCost, 0, True, False, True).ToString
        newOpCostsRow("coPilotSalaryCost") = FormatNumber(outOpCostsArray(a).coPilotSalaryCost, 0, True, False, True).ToString
        newOpCostsRow("benefitsCost") = FormatNumber(outOpCostsArray(a).benefitsCost, 0, True, False, True).ToString

        newOpCostsRow("crewTotalCost") = FormatNumber(outOpCostsArray(a).calcCrewTotalCost, 0, True, False, True).ToString

        newOpCostsRow("hangarCost") = FormatNumber(outOpCostsArray(a).hangarCost, 0, True, False, True).ToString

        newOpCostsRow("insuranceHullCostRaw") = FormatNumber(outOpCostsArray(a).insuranceHullCost, 2, True, False, True).ToString
        newOpCostsRow("insuranceLiabilityCostRaw") = FormatNumber(outOpCostsArray(a).insuranceLiabilityCost, 2, True, False, True).ToString

        newOpCostsRow("insuranceHullCost") = FormatNumber(outOpCostsArray(a).insuranceHullCost, 0, True, False, True).ToString
        newOpCostsRow("insuranceLiabilityCost") = FormatNumber(outOpCostsArray(a).insuranceLiabilityCost, 0, True, False, True).ToString
        newOpCostsRow("insuranceTotalCost") = FormatNumber(outOpCostsArray(a).calcInsuranceTotalCost, 0, True, False, True).ToString

        newOpCostsRow("miscTrainCostRaw") = FormatNumber(outOpCostsArray(a).miscTrainCost, 2, True, False, True).ToString
        newOpCostsRow("miscModernCostRaw") = FormatNumber(outOpCostsArray(a).miscModernCost, 2, True, False, True).ToString
        newOpCostsRow("miscNavCostRaw") = FormatNumber(outOpCostsArray(a).miscNavCost, 2, True, False, True).ToString

        newOpCostsRow("miscTrainCost") = FormatNumber(outOpCostsArray(a).miscTrainCost, 0, True, False, True).ToString
        newOpCostsRow("miscModernCost") = FormatNumber(outOpCostsArray(a).miscModernCost, 0, True, False, True).ToString
        newOpCostsRow("miscNavCost") = FormatNumber(outOpCostsArray(a).miscNavCost, 0, True, False, True).ToString
        newOpCostsRow("miscTotalCost") = FormatNumber(outOpCostsArray(a).calcMiscTotalCost, 0, True, False, True).ToString

        newOpCostsRow("depreciationCostRaw") = FormatNumber(outOpCostsArray(a).depreciationCost, 2, True, False, True).ToString

        newOpCostsRow("depreciationCost") = FormatNumber(outOpCostsArray(a).depreciationCost, 0, True, False, True).ToString
        newOpCostsRow("totalFixedCosts") = FormatNumber(outOpCostsArray(a).calcTotalFixedCosts, 0, True, False, True).ToString
        newOpCostsRow("variableTotalCost") = FormatNumber(outOpCostsArray(a).variableTotalCost, 0, True, False, True).ToString

        ' ANNUALBUDGET
        newOpCostsRow("numberOfSeats") = FormatNumber(outOpCostsArray(a).numberOfSeats, 0, False, False, False).ToString
        newOpCostsRow("annualMiles") = FormatNumber(outOpCostsArray(a).annualMiles, 0, False, False, True).ToString
        newOpCostsRow("annualHrs") = FormatNumber(outOpCostsArray(a).calcAnnualHrs, 0, False, False, False).ToString

        newOpCostsRow("totalDirCostYR") = FormatNumber(ConversionFunctions.Truncate(outOpCostsArray(a).calcTotalDirCostYR, 0), 0, False, False, True)

        newOpCostsRow("totalFixedDirect") = FormatNumber(ConversionFunctions.Truncate(outOpCostsArray(a).calcTotalFixedDirect, 0), 0, False, False, True)
        newOpCostsRow("costPerHourFixDir") = FormatNumber(outOpCostsArray(a).calcCostPerHourFixDir, 0, True, False, True).ToString
        newOpCostsRow("costPerMileFixDir") = FormatNumber(outOpCostsArray(a).calcCostPerMileFixDir, 2, True, False, True).ToString
        newOpCostsRow("costPerSeatFixDir") = FormatNumber(outOpCostsArray(a).calcCostPerSeatFixDir, 2, True, False, True).ToString

        newOpCostsRow("noDepTotalCost") = FormatNumber(ConversionFunctions.Truncate(outOpCostsArray(a).calcNoDepTotalCost, 0), 0, False, False, True)
        newOpCostsRow("costPerHourNoDep") = FormatNumber(outOpCostsArray(a).calcCostPerHourNoDep, 0, True, False, True).ToString
        newOpCostsRow("costPerMileNoDep") = FormatNumber(outOpCostsArray(a).calcCostPerMileNoDep, 2, True, False, True).ToString
        newOpCostsRow("costPerSeatNoDep") = FormatNumber(outOpCostsArray(a).calcCostPerSeatNoDep, 2, True, False, True).ToString

        dsOpCosts.Tables("opCosts").Rows.Add(newOpCostsRow)

      Next

    Catch ex As Exception
      aError += "error in convertOpCostsClassToDataset(ByRef outOpCostsArray() As opCostsClass, ByRef dsOpCosts As DataSet) As Boolean : " + ex.Message
      Return False
    End Try

    Return True

  End Function

End Class