' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/AssetInsight.aspx.vb $
'$$Author: Mike $
'$$Date: 11/16/19 3:05p $
'$$Modtime: 11/16/19 2:35p $
'$$Revision: 3 $
'$$Workfile: AssetInsight.aspx.vb $
'
' ********************************************************************************

Partial Public Class AssetInsight
  Inherits System.Web.UI.Page

  Public Shared masterPage As New Object
  Private nAircraftID As Long = 0
  Private value_label As String = "eValue"
  Private value_color As String = "#078fd7"
  Private temp_ac_dlv_year As Integer = 0
  Private temp_ac_mfr_year As Integer = 0
  Private temp_make_model As String = ""
  Private temp_ser_no As String = ""
  Private AircraftModel_JETNET As Integer = 0
  Private afmv_source_id As String = ""
  Private show_mapped As Boolean = False
  Private afmvAirframeHours As Long = 0


  Private Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

    Try

      If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
        Me.MasterPageFile = "~/EvoStyles/EmptyCustomerAdminTheme.master"
        masterPage = DirectCast(Page.Master, EmptyCustomerAdminTheme)
      ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.EVO Then
        Me.MasterPageFile = "~/EvoStyles/EmptyEvoTheme.master"
        masterPage = DirectCast(Page.Master, EmptyEvoTheme)
      ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CRM Then
        Me.MasterPageFile = "~/EvoStyles/EmptyEvoTheme.master"
        masterPage = DirectCast(Page.Master, EmptyEvoTheme)
      End If

    Catch ex As Exception
      If Not IsNothing(masterPage) Then
        masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString & " (Page_PreInit): " & ex.Message.ToString)
      Else
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " (Page_PreInit): " + ex.Message.ToString.Trim
      End If
    End Try

  End Sub

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Dim sErrorString As String = ""
    Dim show_customer As Boolean = False
    Dim temp_aftt As String = ""

    HttpContext.Current.Session.Item("crmUserLogon") = True

    If Not IsNothing(Request.Item("acid")) Then
      If Not String.IsNullOrEmpty(Request.Item("acid").ToString.Trim) Then
        If IsNumeric(Request.Item("acid").ToString) Then
          nAircraftID = CLng(Request.Item("acid").ToString.Trim)
        End If
      End If
    End If


    If Trim(Request("mapping")) = "Y" Then
      show_mapped = True
    Else
      show_mapped = False
    End If


    If Trim(Request("customer")) = "Y" Then
      show_customer = True
    Else
      show_customer = False
    End If


    '  AIAClbl.Text = "Asset Insight Aircraft Data for acid:" + nAircraftID.ToString

    If show_customer = True Then
      Dim AircraftTable As New DataTable
      AircraftTable = CommonAircraftFunctions.BuildReusableTable(nAircraftID, 0, "JETNET", "", Master.aclsData_Temp, False, 0, "JETNET")

      If Not IsNothing(AircraftTable) Then
        If AircraftTable.Rows.Count > 0 Then

          'Aircraft Model ID
          AircraftModel_JETNET = AircraftTable.Rows(0).Item("jetnet_amod_id")

          'Grabbing Ser #
          If Not IsDBNull(AircraftTable.Rows(0).Item("ac_ser_nbr")) Then
            temp_ser_no = AircraftTable.Rows(0).Item("ac_ser_nbr")
          End If

          'Grabbing the year DLV from datatable
          If Not IsDBNull(AircraftTable.Rows(0).Item("ac_year")) Then
            temp_ac_dlv_year = AircraftTable.Rows(0).Item("ac_year")
          End If

          If Not IsDBNull(AircraftTable.Rows(0).Item("ac_mfr_year")) Then
            temp_ac_mfr_year = AircraftTable.Rows(0).Item("ac_mfr_year")
          End If

          If IsNumeric(temp_ac_dlv_year) And IsNumeric(temp_ac_mfr_year) Then
            displayYearCompare()
          End If

          'Grabbing the make/model name from datatable.
          If Not (IsDBNull(AircraftTable.Rows(0).Item("amod_make_name")) And Not IsDBNull(AircraftTable.Rows(0).Item("amod_model_name"))) Then
            temp_make_model = AircraftTable.Rows(0).Item("amod_make_name").ToString & " " & AircraftTable.Rows(0).Item("amod_model_name").ToString
          End If

          'Function to Create Main Header Line.
          AIAClbl.Text = CommonAircraftFunctions.CreateHeaderLine(AircraftTable.Rows(0).Item("amod_make_name"), AircraftTable.Rows(0).Item("amod_model_name"), AircraftTable.Rows(0).Item("ac_ser_nbr"), "<span class=""float_right padding_right"">eValue Summary</span>")

          'Setting up Identification Block.
          ac_block.Text = CommonAircraftFunctions.Build_Identification_Block("blue", False, "", "100%", "100%", 0, AircraftTable, "JETNET", 0, nAircraftID, Master.aclsData_Temp, New CheckBox, New CheckBox, HttpContext.Current.Session.Item("localPreferences").AerodexFlag, 0, False, True)

          'Function to Control Status Block.
          ac_status.Text = CommonAircraftFunctions.Build_Status_Block(nAircraftID, 0, New DataTable, AircraftTable, False, HttpContext.Current.Session.Item("localPreferences").AerodexFlag, 0, "100%", "100%", "blue", "", Master.aclsData_Temp, New TextBox, passCheckboxForAsking, New CheckBox, New CheckBox, "", "", New TextBox, False, False, False, "", True, New Button, False, True)
          generateResidualValue(nAircraftID, AircraftModel_JETNET)
        End If
      End If


      'temp_ac_dlv_year = Get_AC_DLV_YEAR(nAircraftID)
      'temp_make_model = Get_Model_Name(AircraftModel_JETNET)
      'Call get_current_avg_evalues(evalue_label.Text)
      ' AIAClbl.Text &= ", Asset ID: " & afmv_source_id
      '  AIAClbl.Text &= "<h2 class=""mainHeading padded_left""><strong>" & temp_make_model & "</strong> SN#: " & temp_ser_no & "</h2>"



      generate_aftt_avg(nAircraftID, AirframeEnginesLbl.Text, factors.Text)
      generate_sale_price_differences(nAircraftID, recent_sales.Text)
      DisplaySalesComparables(AircraftModel_JETNET)


    End If

    DisplayMaintenanceCoverageBox(nAircraftID, maint_coverage.Text)

    ' get airframe and inspections
    generateAssetInsightAirframeInspections(nAircraftID, "", InspectionsLbl.Text, temp_aftt)

    ' get modifications "features"
    generateModificationsFeaturesTable(nAircraftID, ModificationsLbl.Text)





    Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("eValue Summary")
    masterPage.SetPageTitle("eValue Summary")

  End Sub
  Private Sub DisplayYearCompare()
    If (temp_ac_dlv_year) > 0 Then
      If (temp_ac_mfr_year) > 0 Then
        If temp_ac_mfr_year < temp_ac_dlv_year Then
          Dim differenceYear As Integer = 0
          differenceYear = temp_ac_dlv_year - temp_ac_mfr_year
          aircraftYearCompareText.Text = "<p>This Aircraft may have lower eValue due to being manufactured <strong>" & differenceYear.ToString & " year(s)</strong> prior to delivery.</p>"
        End If

      End If
    End If

  End Sub
  Public Sub DisplaySalesComparables(ByVal modelID As Long)
    Dim resultsTable As New DataTable
    Dim resultsText As New StringBuilder
    sales_comparables_label.CssClass = ""

    resultsTable = getSalesComparableDataTable(modelID)
    If Not IsNothing(resultsTable) Then
      If resultsTable.Rows.Count > 0 Then
        resultsText.Append("<div class=""Box"">")
        resultsText.Append("<table width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""formatTable blue"">")
        resultsText.Append("<tr class=""noBorder""><td align=""left"" valign=""top"" colspan=""5""><div class=""subHeader"">JETNET SALES (LAST 365 DAYS)</div></td></tr>")
        resultsText.Append("<tr>")
        resultsText.Append("<td align=""left"" valign=""top"">Ser #</td>")
        resultsText.Append("<td align=""left"" valign=""top"">Date</td>")
        resultsText.Append("<td align=""left"" valign=""top"">DLV Year</td>")
        resultsText.Append("<td align=""left"" valign=""top"">AFTT</td>")
        resultsText.Append("<td align=""left"" valign=""top"">Sale Price</td>")
        resultsText.Append("</tr>")

        For Each r As DataRow In resultsTable.Rows

          resultsText.Append("<tr>")
          'ser #
          resultsText.Append("<td align=""left"" valign=""top"">")
          resultsText.Append(DisplayFunctions.WriteDetailsLink(r("ac_id"), 0, 0, r("ac_journ_id"), True, r("SERNO"), "", ""))
          resultsText.Append("</td>")

          ' reg#
          resultsText.Append("<td align=""left"" valign=""top"">")
          If Not IsDBNull(r("SALEDATE")) Then
            resultsText.Append(clsGeneral.clsGeneral.TwoPlaceYear(r("SALEDATE")))
          End If
          resultsText.Append("</td>")


          'dlv year
          resultsText.Append("<td align=""left"" valign=""top"">")
          If Not IsDBNull(r("DLVYEAR")) Then
            resultsText.Append(r("DLVYEAR"))
          End If
          resultsText.Append("</td>")

          'airframe hours
          resultsText.Append("<td align=""left"" valign=""top"">")
          If Not IsDBNull(r("AIRFRAMEHRS")) Then
            resultsText.Append(r("AIRFRAMEHRS"))
          End If
          resultsText.Append("</td>")


          'sale price
          resultsText.Append("<td align=""left"" valign=""top"">")
          If Not IsDBNull(r("SALEPRICE")) Then
            resultsText.Append(clsGeneral.clsGeneral.ConvertIntoThousands(r("SALEPRICE")))
          End If
          resultsText.Append("</td>")




          resultsText.Append("</tr>")
        Next



        resultsText.Append("</table>")
        resultsText.Append("</div>")

        sales_comparables_label.Text = resultsText.ToString

      End If
    End If
  End Sub

  Public Function getSalesComparableDataTable(ByVal modelID As Long) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try



      sQuery.Append("SELECT distinct journ_date as SALEDATE, ac_id, ac_journ_id, ac_ser_no_full as SERNO, ac_mfr_year as MFRYEAR,ac_year as DLVYEAR,")
      sQuery.Append(" ac_reg_no as REGNO,  ")
      sQuery.Append(" ac_sale_price as SALEPRICE, ac_airframe_tot_hrs as AIRFRAMEHRS")
      sQuery.Append(" from journal WITH(NOLOCK) ")
      sQuery.Append(" inner join Aircraft WITH (NOLOCK) on ac_id=journ_ac_id and ac_journ_id = journ_id")
      sQuery.Append(" inner join Aircraft_Model with (NOLOCK) on amod_id = ac_amod_id")
      'Model ID here
      sQuery.Append(" WHERE amod_id = " & modelID.ToString)
      sQuery.Append(" AND NOT (journ_subcat_code_part3 IN ('DB','DS','FI','FY','IT','MF','RE','CC','LS','RM')) ")
      sQuery.Append(" AND (journ_subcat_code_part1 = 'WS') AND (journ_internal_trans_flag = 'N') and journ_newac_flag='N'")
      sQuery.Append(" AND journ_date >= GETDATE() - 365 ")
      sQuery.Append(" AND (ac_sale_price_display_flag = 'Y')")
      'Year dlv -1/+1

      If IsNumeric(temp_ac_dlv_year) Then
        If temp_ac_dlv_year > 0 Then
          sQuery.Append(" and ac_year between '" & (temp_ac_dlv_year - 1).ToString & "' and '" & (temp_ac_dlv_year + 1).ToString & "'")
        End If
      End If

      If IsNumeric(afmvAirframeHours) Then
        If afmvAirframeHours > 0 Then
          sQuery.Append(" and ac_est_airframe_hrs between " & (afmvAirframeHours - 1000).ToString & " and " & (afmvAirframeHours + 1000).ToString & "") ' changed from ac_airframe_tot_hrs   to ac_est_airframe_hrs
        End If
      End If

      '''''''''''''Product codes here
      sQuery.Append(" " & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(HttpContext.Current.Session.Item("localSubscription"), False, True))
      sQuery.Append(" ORDER BY journ_date DESC")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getSalesComparableDataTable(ByVal modelID As Long) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = Session.Item("jetnetClientDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 240

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getAssetInsightResearchDataTable load datatable</b><br /> " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getAssetInsightResearchDataTable(ByVal nAircraftID As Long) As DataTable</b><br />" + ex.Message

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

  Public Function getAssetInsightResearchDataTable(ByVal nAircraftID As Long) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT aiinspect_description AS ITEM, aiinspect_hours AS HOURS, aiinspect_cycles AS CYCLES, aiinspect_date AS DATE,")
      If show_mapped = True Then
        sQuery.Append(" aimaint_jetnet_item_id as mapped_id,  ")
      End If
      sQuery.Append(" (CASE WHEN aiinspect_item_id < 100 THEN 'AIRFRAME/ENGINES TIMES' ELSE 'INSPECTIONS' END) AS DATATYPE,")
      sQuery.Append(" (CASE WHEN aiinspect_verified_flag = 'Y' THEN 'VERIFIED' ELSE 'ASSUMED' END) AS STATUS")
      sQuery.Append(" FROM Asset_Insight_Aircraft_Inspections WITH(NOLOCK)")
      sQuery.Append(" LEFT OUTER JOIN Asset_Insight_Maintenance_Item WITH(NOLOCK) ON aimaint_description = aiinspect_description")
      sQuery.Append(" INNER JOIN Aircraft_Flat WITH(NOLOCK) ON aiinspect_ac_id = ac_id AND ac_journ_id = 0")

      sQuery.Append(" WHERE ac_id = " + nAircraftID.ToString)
      sQuery.Append(" ORDER BY  DATATYPE asc, ITEM asc, aiinspect_item_id")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getAssetInsightResearchDataTable(ByVal nAircraftID As Long) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = Session.Item("jetnetClientDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 240

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getAssetInsightResearchDataTable load datatable</b><br /> " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getAssetInsightResearchDataTable(ByVal nAircraftID As Long) As DataTable</b><br />" + ex.Message

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

  Public Sub generateAssetInsightAirframeInspections(ByVal nAircraftID As Long, ByRef sAirframeTable As String, ByRef sInspectionsTable As String, ByVal text_from_avg As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim dataType As String = ""
    Dim bold_line As Boolean = False
    Dim bold_whole_line As Boolean = False
    Dim show_customer As Boolean = False
    Dim has_data As Boolean = False
    Dim data_exists As Boolean = False

    Try

      If Trim(Request("customer")) = "Y" Then
        show_customer = True
      Else
        show_customer = False
      End If



      results_table = getAssetInsightResearchDataTable(nAircraftID)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          If show_customer = True Then
            htmlOut.Append("<br/><table  cellpadding=""4"" cellspacing=""0"">")

            ' first add the report title
            htmlOut.Append("<tr><td align=""left"" valign=""middle"" colspan=""" + results_table.Columns.Count.ToString + """><strong>AIRFRAME/ENGINE TIMES</strong></td></tr>")
            ' second generate the header based off the column names in the datatable 
            ' htmlOut.Append("<tr><td align=""left"">ITEM</td><td>C/W</td></tr>")
          Else
            htmlOut.Append("<table border=""1"" cellpadding=""2"" cellspacing=""0"">")

            ' first add the report title
            htmlOut.Append("<tr><td align=""left"" valign=""middle"" colspan=""" + results_table.Columns.Count.ToString + """><strong>AIRFRAME/ENGINE TIMES</strong></td></tr>")
            ' second generate the header based off the column names in the datatable
            htmlOut.Append("<tr bgcolor=""#CCCCCC"">")

            For Each c As DataColumn In results_table.Columns

              If c.ColumnName.Contains("ITEM") Or c.ColumnName.Contains("HOURS") Or c.ColumnName.Contains("CYCLES") Or c.ColumnName.Contains("STATUS") Then
                htmlOut.Append("<td align=""left"">" + c.ColumnName.ToUpper.Replace("CCOUNT", "COUNT").Trim + "</td>")
              End If

            Next

            htmlOut.Append("</tr>")
          End If







          ' second display the report data based off the column names in the datatable
          For Each r As DataRow In results_table.Rows


            If show_customer = True Then
              If r.Item("DATATYPE").ToString.Contains("AIRFRAME/ENGINES TIMES") Then
                If Trim(r.Item("ITEM")) <> "Airframe Planned" Then
                  has_data = False
                  htmlOut.Append("<tr>")
                  htmlOut.Append("<td align=""left"" valign=""top"">" & Replace(r.Item("ITEM"), "Current", "") & "</td>")
                  htmlOut.Append("<td align=""left"" valign=""top"">")

                  If Not IsDBNull(r.Item("HOURS")) Then
                    If Trim(r.Item("HOURS")) <> "0" Then
                      htmlOut.Append("" & FormatNumber(r.Item("HOURS"), 0) & " Hrs")
                      has_data = True
                    End If
                  End If

                  If Not IsDBNull(r.Item("CYCLES")) Then
                    If Trim(r.Item("CYCLES")) <> "0" Then
                      If has_data = True Then
                        htmlOut.Append(", ")
                      End If
                      htmlOut.Append("" & FormatNumber(r.Item("CYCLES"), 0) & " Cycles")
                    End If
                  End If

                  htmlOut.Append("</tr>")

                  If Trim(r.Item("ITEM")) = "Airframe Current" Then
                    htmlOut.Append(text_from_avg)
                  End If

                End If
              End If
            Else
              htmlOut.Append("<tr>")


              ' ramble through each "column name" and display data
              For Each c As DataColumn In results_table.Columns

                bold_line = False
                If show_mapped = True Then
                  If Not IsDBNull(r.Item("mapped_id")) Then
                    bold_line = True
                  End If
                End If

                If bold_line = True Then
                  If r.Item("DATATYPE").ToString.Contains("AIRFRAME/ENGINES TIMES") Then
                    If c.ColumnName.Contains("ITEM") Or c.ColumnName.Contains("HOURS") Or c.ColumnName.Contains("CYCLES") Or c.ColumnName.Contains("STATUS") Then
                      htmlOut.Append("<td align=""left"" valign=""top""><strong>" + r.Item(c.ColumnName).ToString.Trim + "</strong></td>")
                    End If
                  End If
                Else
                  If r.Item("DATATYPE").ToString.Contains("AIRFRAME/ENGINES TIMES") Then
                    If c.ColumnName.Contains("ITEM") Or c.ColumnName.Contains("HOURS") Or c.ColumnName.Contains("CYCLES") Or c.ColumnName.Contains("STATUS") Then
                      htmlOut.Append("<td align=""left"" valign=""top"">" + r.Item(c.ColumnName).ToString.Trim + "</td>")
                    End If
                  End If
                End If

              Next

              htmlOut.Append("</tr>")
            End If


          Next

          htmlOut.Append("</table>")

          sAirframeTable = htmlOut.ToString

          htmlOut = New StringBuilder





          If show_customer = True Then
            htmlOut.Append("<div class=""Box removeLeftMargin""><table width=""100%"" cellpadding=""0"" cellspacing=""0"" class=""formatTable blue"">")
            ' first add the report title
            htmlOut.Append("<tr class=""noBorder""><td align=""left"" valign=""middle"" colspan=""" + results_table.Columns.Count.ToString + """><span class=""subHeader"">INSPECTION ASSUMPTIONS</span></td></tr>")
            ' second generate the header based off the column names in the datatable
            htmlOut.Append("<tr><td align=""left"" width='300'><strong>ITEM</strong></td><td><strong>C/W</strong></td></tr>")

          Else
            htmlOut.Append("<table border=""1"" cellpadding=""2"" cellspacing=""0"">")
            ' first add the report title
            htmlOut.Append("<tr><td align=""left"" valign=""middle"" colspan=""" + results_table.Columns.Count.ToString + """><strong>INSPECTIONS</strong></td></tr>")
            ' second generate the header based off the column names in the datatable
            htmlOut.Append("<tr bgcolor=""#CCCCCC"">")
          End If



          For Each c As DataColumn In results_table.Columns

            If show_customer = True Then
            Else
              If Not c.ColumnName.Contains("DATATYPE") Then
                If Trim(UCase(c.ColumnName)) <> "MAPPED_ID" Then
                  htmlOut.Append("<td align=""left"">" + c.ColumnName.ToUpper.Replace("CCOUNT", "COUNT").Trim + "</td>")
                End If
              End If
            End If
 
          Next
          htmlOut.Append("</tr>")

          ' second display the report data based off the column names in the datatable
          For Each r As DataRow In results_table.Rows
            has_data = False
            data_exists = False

            If show_customer = True Then
              If r.Item("DATATYPE").ToString.Contains("INSPECTIONS") Then
                ' ITEM, aiinspect_hours AS HOURS, aiinspect_cycles AS CYCLES, aiinspect_date AS DATE,
                If Not IsDBNull(r.Item("ITEM")) Then
                  If Trim(r.Item("ITEM")) <> "" Then

                    If Not IsDBNull(r.Item("HOURS")) Then
                      If Trim(r.Item("HOURS")) <> "0" Then
                        data_exists = True
                      End If
                    End If

                    If Not IsDBNull(r.Item("CYCLES")) Then
                      If Trim(r.Item("CYCLES")) <> "0" Then
                        data_exists = True
                      End If
                    End If

                    If Not IsDBNull(r.Item("DATE")) Then
                      If Trim(r.Item("DATE")) <> "" Then
                        data_exists = True
                      End If
                    End If


                    If data_exists = True Then

                      htmlOut.Append("<tr>")

                      If show_mapped = True Then
                        If Not IsDBNull(r.Item("mapped_id")) Then
                          If r.Item("mapped_id") > 0 Then
                            htmlOut.Append("<td align=""left"" valign=""top"">" & r.Item("ITEM") & "</td>")
                          Else
                            htmlOut.Append("<td align=""left"" valign=""top""><font color='red'>" & r.Item("ITEM") & "</font></td>")
                          End If 
                        Else
                          htmlOut.Append("<td align=""left"" valign=""top"">" & r.Item("ITEM") & "</td>")
                        End If
                      Else
                        htmlOut.Append("<td align=""left"" valign=""top"">" & r.Item("ITEM") & "</td>")
                      End If


                        htmlOut.Append("<td align=""left"" valign=""top"">")

                        If Not IsDBNull(r.Item("HOURS")) Then
                          If Trim(r.Item("HOURS")) <> "0" Then
                            htmlOut.Append("" & FormatNumber(r.Item("HOURS"), 0) & " Hrs")
                            has_data = True
                          End If
                        End If

                        If Not IsDBNull(r.Item("CYCLES")) Then
                          If Trim(r.Item("CYCLES")) <> "0" Then
                            If has_data = True Then
                              htmlOut.Append(", ")
                            End If
                            htmlOut.Append("" & FormatNumber(r.Item("CYCLES"), 0) & " Cycles")
                          End If
                        End If

                        If Not IsDBNull(r.Item("DATE")) Then
                          If has_data = True Then
                            htmlOut.Append(", ")
                          End If
                          htmlOut.Append("" & FormatDateTime(r.Item("DATE"), DateFormat.ShortDate) & " ")
                        End If

                        htmlOut.Append("")

                        htmlOut.Append("</td>")
                        htmlOut.Append("</tr>")
                      End If
                    End If
                  End If
                End If
              Else
                htmlOut.Append("<tr>")
              End If


              bold_whole_line = False
              ' ramble through each "column name" and display data
              For Each c As DataColumn In results_table.Columns
                If show_customer = True Then
                Else
                  bold_line = False
                  If show_mapped = True Then
                    If Not IsDBNull(r.Item("mapped_id")) Then
                      bold_line = True
                    Else
                      If IsDBNull(r.Item(c.ColumnName)) Then
                      ElseIf Trim(r.Item(c.ColumnName)) = "Paint - Last Time Aircraft was Painted" Or Trim(r.Item(c.ColumnName)) = "Interior Replacement" Then
                        bold_whole_line = True
                      End If
                    End If
                  End If
                  If Trim(UCase(c.ColumnName)) <> "MAPPED_ID" Then
                    If bold_line = True Or bold_whole_line = True Then
                      If r.Item("DATATYPE").ToString.Contains("INSPECTIONS") Then
                        If Not c.ColumnName.Contains("DATATYPE") Then

                          If IsDate(r.Item(c.ColumnName).ToString) Then
                            htmlOut.Append("<td align=""left"" valign=""top""><strong>" + FormatDateTime(r.Item(c.ColumnName).ToString.Trim, DateFormat.ShortDate) + "</strong></td>")
                          Else
                            htmlOut.Append("<td align=""left"" valign=""top""><strong>" + r.Item(c.ColumnName).ToString.Trim + "</strong></td>")
                          End If
                        End If
                      End If
                    Else
                      If r.Item("DATATYPE").ToString.Contains("INSPECTIONS") Then
                        If Not c.ColumnName.Contains("DATATYPE") Then

                          If IsDate(r.Item(c.ColumnName).ToString) Then
                            htmlOut.Append("<td align=""left"" valign=""top"">" + FormatDateTime(r.Item(c.ColumnName).ToString.Trim, DateFormat.ShortDate) + "</td>")
                          Else
                            htmlOut.Append("<td align=""left"" valign=""top"">" + r.Item(c.ColumnName).ToString.Trim + "</td>")
                          End If
                        End If
                      End If
                    End If
                  End If
                End If

              Next

              If show_customer = True Then
              Else
                htmlOut.Append("</tr>")
              End If


          Next

          htmlOut.Append("</table>")
          If show_customer = True Then
            htmlOut.Append("</div>")
          End If
          sInspectionsTable = htmlOut.ToString

        End If

      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in generateAssetInsightAirframeInspections(ByVal nAircraftID As Long, ByRef sAirframeTable As String, ByRef sInspectionsTable As String) " + ex.Message

    Finally

    End Try

    htmlOut = Nothing
    results_table = Nothing

  End Sub
  Public Function get_aftt_vs_model_aftt_avg(ByVal nAircraftID As Long) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try


      sQuery.Append("select  afmv_airframe_hrs,afmv_source_id, ")
      sQuery.Append(" afmv_quality, afmv_atc, afmv_atfc, afmv_exposure, afmv_lower, afmv_upper, afmv_equity, ")
      sQuery.Append(" (select AVG(afmv_airframe_hrs) from Aircraft_FMV f2 with (NOLOCK) ")
      sQuery.Append(" inner join Aircraft a2 on a2.ac_id = f2.afmv_ac_id and a2.ac_journ_id = 0  ")
      sQuery.Append(" where  a2.ac_year = Aircraft.ac_year and a2.ac_amod_id = aircraft.ac_amod_id and f2.afmv_airframe_hrs > 0 and f2.afmv_latest_flag = 'Y') as avg_aftt, ")

      sQuery.Append(" (select min(afmv_airframe_hrs) from Aircraft_FMV f2 with (NOLOCK)   ")
      sQuery.Append(" inner join Aircraft a2 on a2.ac_id = f2.afmv_ac_id and a2.ac_journ_id = 0  ")
      sQuery.Append(" where  a2.ac_year = Aircraft.ac_year and a2.ac_amod_id = aircraft.ac_amod_id and f2.afmv_airframe_hrs > 0 and f2.afmv_latest_flag = 'Y') as min_aftt, ")

      sQuery.Append(" (select max(afmv_airframe_hrs) from Aircraft_FMV f2 with (NOLOCK)   ")
      sQuery.Append(" inner join Aircraft a2 on a2.ac_id = f2.afmv_ac_id and a2.ac_journ_id = 0  ")
      sQuery.Append(" where  a2.ac_year = Aircraft.ac_year and a2.ac_amod_id = aircraft.ac_amod_id and f2.afmv_airframe_hrs > 0  and f2.afmv_latest_flag = 'Y') as max_aftt ")

      sQuery.Append(" from Aircraft_FMV with (NOLOCK) ")
      sQuery.Append(" inner join Aircraft with (NOLOCK) on ac_id = afmv_ac_id and ac_journ_id = 0  ")
      sQuery.Append(" where afmv_ac_id = " & nAircraftID & " and afmv_latest_flag = 'Y' ")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>get_aftt_vs_model_aftt_avg(ByVal nAircraftID As Long) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = Session.Item("jetnetClientDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 240

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getAssetInsightResearchDataTable2 load datatable</b><br /> " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getAssetInsightResearchDataTable2(ByVal nAircraftID As Long) As DataTable</b><br />" + ex.Message

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
  Public Function get_maint_coverage_select(ByVal nAircraftID As Long) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try


      sQuery.Append("select aiaccov_item_id, aiaccov_description from Asset_Insight_Aircraft_Coverage with (NOLOCK) ")
      sQuery.Append(" where aiaccov_ac_id = " & nAircraftID & " ")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>get_aftt_vs_model_aftt_avg(ByVal nAircraftID As Long) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = Session.Item("jetnetClientDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 240

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getAssetInsightResearchDataTable2 load datatable</b><br /> " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getAssetInsightResearchDataTable2(ByVal nAircraftID As Long) As DataTable</b><br />" + ex.Message

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

  Public Function get_sale_price_differences(ByVal nAircraftID As Long) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try


      sQuery.Append("  select  afmv_value,   ")
      sQuery.Append(" (select avg(afmv_value) from Aircraft_FMV f2 with (NOLOCK)  ")
      sQuery.Append(" inner join Aircraft a2 on a2.ac_id = f2.afmv_ac_id and a2.ac_journ_id = 0  ")
      sQuery.Append("       where(a2.ac_year = Aircraft.ac_year And a2.ac_amod_id = aircraft.ac_amod_id And afmv_value > 0) ")
      sQuery.Append(" and afmv_latest_flag = 'Y') as avg_evalue, ")
      sQuery.Append(" (select AVG(ac_sale_price) from Aircraft a3 with (NOLOCK)  ")
      sQuery.Append(" inner join Journal with (NOLOCK) on journ_id = a3.ac_journ_id ")
      sQuery.Append("       where(a3.ac_amod_id = aircraft.ac_amod_id) ")
      sQuery.Append(" and journ_subcat_code_part1='WS' AND journ_internal_trans_flag='N'  ")
      sQuery.Append(" and journ_subcat_code_part3 NOT IN ('DB','DS','FI','FY','IT','MF','RE','CC','LS', 'RM')  ")
      sQuery.Append(" and a3.ac_year = Aircraft.ac_year and journ_date > GETDATE() - 365) as avg_sale, ")

      sQuery.Append(" (select AVG(ac_sale_price) from Aircraft a3 with (NOLOCK)  ")
      sQuery.Append(" inner join Journal with (NOLOCK) on journ_id = a3.ac_journ_id ")
      sQuery.Append("      where(a3.ac_amod_id = aircraft.ac_amod_id) ")
      sQuery.Append(" and journ_subcat_code_part1='WS' AND journ_internal_trans_flag='N'  ")
      sQuery.Append(" and journ_subcat_code_part3 NOT IN ('DB','DS','FI','FY','IT','MF','RE','CC','LS', 'RM')  ")
      sQuery.Append(" and a3.ac_year = Aircraft.ac_year and a3.ac_id = aircraft.ac_id and journ_date > GETDATE() - 365 ")
      sQuery.Append(" ) as my_avg_sale, ")


      sQuery.Append(" (select top 1 a3.ac_sale_price  from Aircraft a3 with (NOLOCK)   ")
      sQuery.Append(" inner join Journal with (NOLOCK) on journ_id = a3.ac_journ_id  ")
      sQuery.Append("       where(a3.ac_amod_id = aircraft.ac_amod_id)  ")
      sQuery.Append(" and journ_subcat_code_part1='WS' AND journ_internal_trans_flag='N'   ")
      sQuery.Append(" and journ_subcat_code_part3 NOT IN ('DB','DS','FI','FY','IT','MF','RE','CC','LS', 'RM')   ")
      sQuery.Append(" and a3.ac_year = Aircraft.ac_year and a3.ac_id = aircraft.ac_id and a3.ac_sale_price > 0   ")
      sQuery.Append(" order by journ_date desc   ")
      sQuery.Append(" ) as last_sale,   ")

      sQuery.Append(" (select top 1 journ_date  from Aircraft a3 with (NOLOCK)   ")
      sQuery.Append(" inner join Journal with (NOLOCK) on journ_id = a3.ac_journ_id  ")
      sQuery.Append("       where(a3.ac_amod_id = aircraft.ac_amod_id)  ")
      sQuery.Append(" and journ_subcat_code_part1='WS' AND journ_internal_trans_flag='N'   ")
      sQuery.Append(" and journ_subcat_code_part3 NOT IN ('DB','DS','FI','FY','IT','MF','RE','CC','LS', 'RM')   ")
      sQuery.Append(" and a3.ac_year = Aircraft.ac_year and a3.ac_id = aircraft.ac_id and a3.ac_sale_price > 0   ")
      sQuery.Append(" order by journ_date desc   ")
      sQuery.Append(" ) as last_sale_date   ")

      sQuery.Append(" from Aircraft_FMV with (NOLOCK)    ")
      sQuery.Append(" inner join Aircraft with (NOLOCK) on ac_id = afmv_ac_id and ac_journ_id = 0    ")

      sQuery.Append(" where afmv_ac_id = " & nAircraftID & " and afmv_latest_flag = 'Y' ")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>get_sale_price_differences(ByVal nAircraftID As Long) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = Session.Item("jetnetClientDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 240

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in get_sale_price_differences load datatable</b><br /> " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in get_sale_price_differences(ByVal nAircraftID As Long) As DataTable</b><br />" + ex.Message

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

  Public Function getAssetInsightResearchDataTable2(ByVal nAircraftID As Long) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT aiacmod_description AS ITEM, aiacmod_installed_flag AS INSTALLED,")

      If show_mapped = True Then
        sQuery.Append(" case when aimodif_acatt_id > 0 then 'Y' else 'N' end  as is_mapped ,  ")
      Else
        sQuery.Append(" 'N'  as is_mapped  ,  ")
      End If

      sQuery.Append(" (CASE WHEN aiacmod_verified_flag = 'Y' THEN 'VERIFIED' ELSE 'ASSUMED' END) AS STATUS ")
      sQuery.Append(" FROM Asset_Insight_Aircraft_Modifications WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft_Flat WITH(NOLOCK) ON aiacmod_ac_id = ac_id AND ac_journ_id = 0")
      If show_mapped = True Then
        sQuery.Append("  left outer join Asset_Insight_Modifications with (NOLOCK) on  aimodif_item_id  = aiacmod_ac_item_id ")
      End If

      sQuery.Append(" WHERE aiacmod_ac_id = " + nAircraftID.ToString)
      sQuery.Append(" ORDER BY aiacmod_description")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getAssetInsightResearchDataTable2(ByVal nAircraftID As Long) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = Session.Item("jetnetClientDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 240

      SqlCommand.CommandText = sQuery.ToString

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getAssetInsightResearchDataTable2 load datatable</b><br /> " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getAssetInsightResearchDataTable2(ByVal nAircraftID As Long) As DataTable</b><br />" + ex.Message

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

  Public Sub get_current_avg_evalues(ByRef temp_label As String)


    Dim utilization_functions As New utilization_view_functions
    Dim FoundEval As Boolean = False
    'Dim current_month_table As New DataTable
    'Dim comp_functions As New CompanyFunctions




    'temp_label = "<table>"

    '



    'searchCriteria.ViewCriteriaAmodID = 0
    'current_month_table = utilization_functions.get_current_month_assett_summary(searchCriteria)
    'If Not IsNothing(current_month_table) Then
    '  If current_month_table.Rows.Count > 0 Then
    '    For Each r As DataRow In current_month_table.Rows

    '      If Not IsDBNull(r("afmv_source_id")) Then
    '        afmv_source_id = r("afmv_source_id")
    '      End If

    '      If Not IsDBNull(r("AVGVALUE")) Then
    '        temp_label &= comp_functions.create_value_with_label("<a href=""#"" onclick=""javascript:load('/help/documents/809.pdf','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;""><font color='" & value_color & "'>" & value_label & "</font></a>", "<font color='" & value_color & "'>$" & FormatNumber((r("AVGVALUE") / 1000), 0) & "k</font>", True, False, 0, "")
    '      End If

    '    Next
    '  End If
    'End If

    '' set it for the 2nd selection 
    'searchCriteria.ViewCriteriaAmodID = AircraftModel_JETNET
    'current_month_table.Clear()
    '' get rid of the ac id so it just does model 
    'current_month_table = utilization_functions.get_current_month_assett_summary(searchCriteria, temp_ac_dlv_year)
    'If Not IsNothing(current_month_table) Then
    '  If current_month_table.Rows.Count > 0 Then
    '    For Each r As DataRow In current_month_table.Rows
    '      If Not IsDBNull(r("AVGVALUE")) Then

    '        If Trim(temp_label) = "" Then
    '          temp_label &= comp_functions.create_value_with_label("<font color='" & value_color & "'>(<a href='' title='Avg " & temp_ac_dlv_year & " " & temp_make_model & " " & value_label & "' alt='Avg " & temp_ac_dlv_year & " " & temp_make_model & " " & value_label & "'><font color='" & value_color & "'>Avg/Year:</font></a></font>", "<font color='" & value_color & "'>$" & FormatNumber((r("AVGVALUE") / 1000), 0) & "k)</font>", True, False, 0, "")
    '        Else
    '          temp_label = Replace(temp_label, "</tr>", "<td>&nbsp;&nbsp;&nbsp;&nbsp;<font color='" & value_color & "'>(<a href='' title='Avg " & temp_ac_dlv_year & " " & temp_make_model & " " & value_label & "' alt='Avg " & temp_ac_dlv_year & " " & temp_make_model & " " & value_label & "'><font color='" & value_color & "'>Avg/Year:</font></a> $" & FormatNumber((r("AVGVALUE") / 1000), 0) & "k)</font></td></tr>")
    '        End If

    '      End If
    '    Next
    '  End If
    'End If

    'temp_label &= "</table>"

  End Sub
  Public Sub generate_sale_price_differences(ByVal nAircraftID As Long, ByRef sale_price_page As String)

    Dim results_table As New DataTable

    Dim htmlOut As New StringBuilder

    Try


      results_table = get_sale_price_differences(nAircraftID)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then


          htmlOut.Append("<br/><table cellpadding=""4"" cellspacing=""0"">")

          ' first add the report title
          htmlOut.Append("<tr><td align=""left"" valign=""middle"" colspan=""" + results_table.Columns.Count.ToString + """><strong>SALE PRICE DIFFERENCES</strong></td></tr>")

          For Each r As DataRow In results_table.Rows

            If Not IsDBNull(r.Item("last_sale")) Then
              If Not IsDBNull(r.Item("last_sale_date")) Then
                htmlOut.Append("<tr><td align='left' valign='top'>LAST SALE (" & r.Item("last_sale_date") & ")</td><td align=""left"" valign=""top"">$" & FormatNumber(r.Item("last_sale"), 0) & "</td></tr>")

                Dim dateCompare As Date = r.Item("last_sale_date")
                Dim dateOneYear As Date = Format(DateAdd(DateInterval.Year, -1, Now()), "MM/dd/yyyy")
                If dateCompare >= dateOneYear Then
                  aircraftSalesText.Text = "<p>Aircraft sold for " & clsGeneral.clsGeneral.ConvertIntoThousands(r.Item("last_sale")) & " on " & clsGeneral.clsGeneral.TwoPlaceYear(r.Item("last_sale_date")) & ".</p>"
                End If
              Else
                htmlOut.Append("<tr><td align='left' valign='top'>LAST SALE</td><td align=""left"" valign=""top"">$" & FormatNumber(r.Item("last_sale"), 0) & "</td></tr>")
              End If
            End If


            If Not IsDBNull(r.Item("my_avg_sale")) Then
              htmlOut.Append("<tr><td align='left' valign='top'>AC SALE AVG LAST YEAR</td><td align=""left"" valign=""top"">$" & FormatNumber(r.Item("my_avg_sale"), 0) & "</td></tr>")
            End If

            If Not IsDBNull(r.Item("avg_sale")) Then
              htmlOut.Append("<tr><td align='left' valign='top'>AVG DLV YEAR SALE LAST YEAR</td><td align=""left"" valign=""top"">$" & FormatNumber(r.Item("avg_sale"), 0) & "</td></tr>")

              If Not IsDBNull(r.Item("afmv_value")) Then
                Dim TenPercent As Double = r.Item("afmv_value") * 0.1
                If (r.Item("avg_sale") >= (r.Item("afmv_value") - TenPercent)) And (r.Item("avg_sale") <= (r.Item("afmv_value") + TenPercent)) Then
                  aircraftDeliveryText.Text = "<p>Average sale price for this fleet (by delivery year) was " & clsGeneral.clsGeneral.ConvertIntoThousands(r.Item("avg_sale")) & ".</p>"
                End If
              End If
            End If




          Next

          htmlOut.Append("</table>")

        End If
      End If

      sale_price_page = htmlOut.ToString


    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in generate_aftt_avg " + ex.Message
    End Try

  End Sub
  Public Sub DisplayMaintenanceCoverageBox(ByVal nAircraftID As Long, ByRef pageLabelText As String)
    Dim FieldDisplay As String = ""
    Dim resultsStringbu As New StringBuilder
    Dim comp_functions As New CompanyFunctions
    Dim airframeFound As Boolean = False
    Dim engineFound As Boolean = False


    Dim results_table As New DataTable

    Dim htmlOut As New StringBuilder

    Try


      results_table = get_maint_coverage_select(nAircraftID)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<div class=""Box""><table border=""0"" width=""100%"" cellpadding=""2"" cellspacing=""0"" class=""formatTable blue large"">")
          htmlOut.Append("<tr class=""noBorder""><td align=""left"" colspan='2'><span class=""subHeader"">MAINTENANCE COVERAGE</span></td></tr>")


          For Each r As DataRow In results_table.Rows

            If Not IsDBNull(r.Item("aiaccov_item_id")) Then
              If UCase(Trim(r.Item("aiaccov_item_id"))) = "AIRFRAME" Then
                airframeFound = True
                htmlOut.Append(comp_functions.create_value_with_label("Airframe Program", r.Item("aiaccov_description"), True, True, 0, ""))
              ElseIf UCase(Trim(r.Item("aiaccov_item_id"))) = "ENGINES" Then
                htmlOut.Append(comp_functions.create_value_with_label("Engine Program", r.Item("aiaccov_description"), True, True, 0, ""))
                engineFound = True
              Else
                htmlOut.Append(comp_functions.create_value_with_label(r.Item("aiaccov_item_id"), r.Item("aiaccov_description"), True, True, 0, ""))
              End If



            End If

          Next

          If airframeFound = False Then
            htmlOut.Append(comp_functions.create_value_with_label("Airframe Program", "<span class=""red_text"">Unknown</span>", True, True, 0, ""))
            aircraftAirframeMaintenanceText.Text = "<p>No airframe maintenance program was indicated at the time of the estimate.</p>"
          End If

          If engineFound = False Then
            htmlOut.Append(comp_functions.create_value_with_label("Engine Program", "<span class=""red_text"">Unknown</span>", True, True, 0, ""))
            aircraftEngineMaintenanceText.Text = "<p>No engine maintenance program was indicated at the time of the estimate.</p>"
          End If
          htmlOut.Append("</table></div>")

        End If
      End If


      pageLabelText = htmlOut.ToString
    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in generate_aftt_avg " + ex.Message
    End Try

  End Sub
  Public Sub generate_maint_coverage(ByVal nAircraftID As Long, ByRef page_Text As String)

    Dim results_table As New DataTable

    Dim htmlOut As New StringBuilder

    Try


      results_table = get_maint_coverage_select(nAircraftID)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<div class=""Box""><table border=""0"" width=""100%"" cellpadding=""2"" cellspacing=""0"" class=""formatTable blue large"">")
          htmlOut.Append("<tr class=""noBorder""><td align=""left"" colspan='2'><span class=""subHeader"">MAINTENANCE COVERAGE</span></td></tr>")

          If show_mapped = True Then
            htmlOut.Append("<tr bgcolor=""#CCCCCC""><td align=""left"">ITEM</td><td>DESCRIPTION</td></tr>")
          Else
            htmlOut.Append("<tr><td align=""left"">ITEM</td><td>DESCRIPTION</td></tr>")
          End If


          '  first add the report title


          For Each r As DataRow In results_table.Rows

            If Not IsDBNull(r.Item("aiaccov_item_id")) Then

              If show_mapped = True Then
                htmlOut.Append("<tr valign='top'><td align='left' valign='top'><strong>" & r.Item("aiaccov_item_id") & "</strong></td><td align=""left"" valign=""top""><strong>")
                If Not IsDBNull(r.Item("aiaccov_description")) Then
                  htmlOut.Append("" & r.Item("aiaccov_description") & "")
                End If
                htmlOut.Append("&nbsp;</strong></td></tr>")
              Else
                htmlOut.Append("<tr valign='top'><td align='left' valign='top'>" & r.Item("aiaccov_item_id") & "</td><td align=""left"" valign=""top"">")
                If Not IsDBNull(r.Item("aiaccov_description")) Then
                  htmlOut.Append("" & r.Item("aiaccov_description") & "")
                End If
                htmlOut.Append("&nbsp;</td></tr>")
              End If


            End If

          Next

          htmlOut.Append("</table></div>")

        End If
      End If


      page_Text = htmlOut.ToString


    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in generate_aftt_avg " + ex.Message
    End Try


  End Sub
  Public Sub generateResidualValue(ByVal aircraftID As Long, ByVal modelID As Long)
    residual_label.Visible = True

    Dim utilization_functions As New utilization_view_functions
    utilization_functions.adminConnectStr = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
    utilization_functions.clientConnectStr = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString.Trim
    utilization_functions.starConnectStr = HttpContext.Current.Session.Item("jetnetStarDatabase").ToString.Trim
    utilization_functions.serverConnectStr = HttpContext.Current.Session.Item("jetnetServerNotesDatabase").ToString.Trim
    utilization_functions.cloudConnectStr = HttpContext.Current.Session.Item("jetnetCloudNotesDatabase").ToString.Trim

    residual_label.Text = "<div class=""Box""><table class=""formatTable blue large"" width=""100%""><tr class=""noBorder""><td align=""left"" valign=""top"">"

    residual_label.Text += "<span class=""subHeader"">RESIDUAL VALUES BY DLV YEAR</span></td></tr><tr><td align=""left"" valign=""top"">"

    Call utilization_functions.FillAssettInsightGraphs("RESIDUALAC", modelID, residual_label.Text, graphUpdateResidual, 1, aircraftID, 0, 220, temp_ac_dlv_year, True, True, True, "", "N", "", "", "", "", "", "", "", "", "", "", "", True, True, False)

    residual_label.Text += "</td></tr></table></div>"


  End Sub
  Public Function generateAFTTGauge(ByVal afttTable As DataTable, ByVal currentAFFTT As Object, ByVal minAFTT As Object, ByVal avgAFTT As Object, ByVal maxAFTT As Object) As StringBuilder
    Dim htmlOut As New StringBuilder
    Dim jsScr As New StringBuilder

    afttGauge.CssClass = ""
    jsScr.Append(" function initGauge_AFTT() { ")

    jsScr.Append(" var gauge = new RadialGauge({ renderTo:  'afttCount',")
    jsScr.Append(" width: 275, height: 275, units: false,")
    jsScr.Append(" fontTitleSize: ""34"",")
    jsScr.Append(" fontTitle:""Arial"",")
    jsScr.Append("colorTitle:  '#4f5050',")

    jsScr.Append(" title: """ & currentAFFTT.ToString & """, ")
    jsScr.Append("  startAngle: 90, SweepAngle: 180, valueBox: false, ticksAngle: 180, exactTicks: true, ")
    jsScr.Append("  minValue: " & minAFTT.ToString & ",  maxValue: " & maxAFTT.ToString & ",")
    jsScr.Append(" majorTicks: false, minorTicks: 0,strokeTicks: false,")
    jsScr.Append(" colorUnits: ""#000000"",")
    jsScr.Append(" fontUnitsSize: ""30"",")
    jsScr.Append("highlights: false,animation: false,")
    jsScr.Append("barWidth: 25,")
    jsScr.Append("barProgress: true,")
    jsScr.Append("colorBarProgress:  '#078fd7',")
    jsScr.Append("needle: false,")
    jsScr.Append("colorBar:  '#eee',")
    jsScr.Append("colorStrokeTicks: '#fff',")
    jsScr.Append("numbersMargin: -18,")
    jsScr.Append("  colorPlate: ""rgba(0,0,0,0)"",") 'Make background transparent.
    jsScr.Append("    borderShadowWidth: 0,")
    jsScr.Append("    borders: false,")
    jsScr.Append("    value: " & currentAFFTT.ToString & ",")
    jsScr.Append("}).draw();")


    jsScr.Append(" };initGauge_AFTT();")

    If IsNumeric(avgAFTT) Then
      If IsNumeric(currentAFFTT) Then
        If currentAFFTT >= avgAFTT Then
          If currentAFFTT = maxAFTT Then
            aircraftAFTTCompareText.Text = "<p>Aircraft has the <strong>highest</strong> (" & currentAFFTT.ToString & ") airframe hours for this delivery year.</p>"
          Else
            aircraftAFTTCompareText.Text = "<p>Aircraft has <strong>higher</strong> (" & currentAFFTT.ToString & ") than average (" & avgAFTT.ToString & ") airframe hours for this delivery year.</p>"
          End If

        ElseIf currentAFFTT < avgAFTT Then
          If currentAFFTT = minAFTT Then
            aircraftAFTTCompareText.Text = "<p>Aircraft has the <strong>lowest</strong> (" & currentAFFTT.ToString & ") airframe hours for this delivery year.</p>"
          Else
            aircraftAFTTCompareText.Text = "<p>Aircraft has <strong>lower</strong> (" & currentAFFTT.ToString & ") than average (" & avgAFTT.ToString & ") airframe hours for this delivery year.</p>"
          End If
          End If
      End If
    End If

    System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType, "gaugeString", jsScr.ToString, True)


    Return htmlOut
  End Function

  Public Function generateEvalueGauge(ByVal canvasName As String, ByVal value As Double) As StringBuilder
    Dim htmlOut As New StringBuilder
    Dim jsScr As New StringBuilder
    evalueGauges.CssClass = ""

    jsScr.Append(" function initGauge_" & canvasName & "() { ")

    jsScr.Append(" var gauge = new RadialGauge({ renderTo:  '" & canvasName & "',")
    jsScr.Append(" width: 265, height: 265, units: false,")
    jsScr.Append(" fontTitleSize: ""34"",")
    jsScr.Append(" fontTitle:""Arial"",")
    jsScr.Append("colorTitle:  '#4f5050',")

    If Len(Trim(value)) > 3 Then
      jsScr.Append(" title: """ & FormatNumber(value, 2).ToString & """, ")
    Else
      jsScr.Append(" title: """ & value.ToString & """, ")
    End If


    jsScr.Append("  startAngle: 90, SweepAngle: 180, valueBox: false, ticksAngle: 180, exactTicks: true, ")
    If LCase(canvasName) = "aftccount" Then
      jsScr.Append("  minValue: -5, ")
    Else
      jsScr.Append("  minValue: 0, ")
    End If

    jsScr.Append("  maxValue: 10,")
    jsScr.Append(" majorTicks: false, minorTicks: 0,strokeTicks: false,")
    jsScr.Append(" colorUnits: ""#000000"",")
    jsScr.Append(" fontUnitsSize: ""30"",")
    jsScr.Append("highlights: false,animation: false,")
    jsScr.Append("barWidth: 25,")
    jsScr.Append("barProgress: true,")


    If value >= 6 Then
      jsScr.Append("colorBarProgress:  '#2ce427',")
    ElseIf value >= 5.5 And value < 6 Then
      jsScr.Append("colorBarProgress:  '#62cc14',")
    ElseIf value >= 5 And value < 5.5 Then
      jsScr.Append("colorBarProgress:  '#cad310',")
    ElseIf value >= 4.5 And value < 5 Then
      jsScr.Append("colorBarProgress:  '#e9c43b',")
    ElseIf value >= 4 And value < 4.5 Then
      jsScr.Append("colorBarProgress:  '#e88d23',")
    ElseIf value < 4 Then
      jsScr.Append("colorBarProgress:  '#ff0000',")
    End If

    jsScr.Append("needle: false,")
    jsScr.Append("colorBar:  '#eee',")
    jsScr.Append("colorStrokeTicks: '#fff',")
    jsScr.Append("numbersMargin: -18,")
    jsScr.Append("  colorPlate: ""rgba(0,0,0,0)"",") 'Make background transparent.
    jsScr.Append("    borderShadowWidth: 0,")
    jsScr.Append("    borders: false,")
    jsScr.Append("    value: " & value.ToString & ",")
    jsScr.Append("}).draw();")


    jsScr.Append(" };initGauge_" & canvasName & "();")

    System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType, "gaugeString" & canvasName, jsScr.ToString, True)


    Return htmlOut
  End Function
  Public Sub generate_aftt_avg(ByVal nAircraftID As Long, ByRef aftt_page As String, ByRef temp_factors As String)

    Dim results_table As New DataTable

    Dim htmlOut As New StringBuilder
    Dim htmlout2 As New StringBuilder
    factors.Visible = False

    Try


      results_table = get_aftt_vs_model_aftt_avg(nAircraftID)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          'Gauge
          htmlOut = generateAFTTGauge(results_table, results_table.Rows(0).Item("afmv_airframe_hrs"), results_table.Rows(0).Item("min_aftt"), results_table.Rows(0).Item("avg_aftt"), results_table.Rows(0).Item("max_aftt"))
          ' 
          If Not IsDBNull(results_table.Rows(0).Item("afmv_airframe_hrs")) Then
            afmvAirframeHours = results_table.Rows(0).Item("afmv_airframe_hrs")
          End If

          htmlout2.Append("<br/><table cellpadding=""4"" cellspacing=""0"">")
          htmlout2.Append("<tr><td align=""left"" colspan='2'><strong>ASSET INSIGHT FACTORS</strong></td></tr>")


          'htmlOut.Append("<table cellpadding=""4"" cellspacing=""0"">")

          ' first add the report title
          '  htmlOut.Append("<tr><td align=""left"" valign=""middle"" colspan=""" + results_table.Columns.Count.ToString + """><strong>AVG DLV YEAR</strong></td></tr>")

          For Each r As DataRow In results_table.Rows

            If Not IsDBNull(r.Item("min_aftt")) Then
              htmlout2.Append("<tr valign='top'><td align='left' valign='top'>LOW DLV YEAR AFTT</td><td align=""left"" valign=""top"">" & FormatNumber(r.Item("min_aftt"), 0) & " Hrs</td></tr>")
            End If

            If Not IsDBNull(r.Item("avg_aftt")) Then
              htmlout2.Append("<tr valign='top'><td align='left' valign='top'>AVG DLV YEAR AFTT</td><td align=""left"" valign=""top"">" & FormatNumber(r.Item("avg_aftt"), 0) & " Hrs</td></tr>")
            End If

            If Not IsDBNull(r.Item("max_aftt")) Then
              htmlout2.Append("<tr valign='top'><td align='left' valign='top'>HIGH DLV YEAR AFTT</td><td align=""left"" valign=""top"">" & FormatNumber(r.Item("max_aftt"), 0) & " Hrs</td></tr>")
            End If 


            '------------------------------------------------ 
            If Not IsDBNull(r.Item("afmv_quality")) Then
              htmlout2.Append("<tr valign='top'><td align='left' valign='top'>QUALITY</td><td align=""left"" valign=""top"">" & r.Item("afmv_quality") & "</td></tr>")
              generateEvalueGauge("qualityCount", r.Item("afmv_quality"))

              generateQualityText(r.Item("afmv_quality"))
            End If

            If Not IsDBNull(r.Item("afmv_atc")) Then
              'htmlout2.Append("<tr valign='top'><td align='left' valign='top'>ATC</td><td align=""left"" valign=""top"">" & r.Item("afmv_atc") & "</td></tr>")
              generateEvalueGauge("atcCount", r.Item("afmv_atc"))

              If r.Item("afmv_atc") > 5.5 Then
                aircraftATCText.Text = "<p>This aircraft has received an <strong class=""green_text"">outstanding</strong> quality rating (" & FormatNumber(r.Item("afmv_atc").ToString, 2) & ") based on its technical condition relative to maintenance.</p>"
              ElseIf r.Item("afmv_atc") < 4 Then
                aircraftATCText.Text = "<p>This aircraft has received a <strong class=""red_text"">below average</strong> quality rating (" & FormatNumber(r.Item("afmv_atc").ToString, 2) & ") based on its technical condition relative to maintenance.</p>"
              End If
            End If

            If Not IsDBNull(r.Item("afmv_atfc")) Then
              'htmlout2.Append("<tr valign='top'><td align='left' valign='top'>AFTC</td><td align=""left"" valign=""top"">" & r.Item("afmv_atfc") & "</td></tr>")
              generateEvalueGauge("aftcCount", r.Item("afmv_atfc"))
              If r.Item("afmv_atfc") > 5.5 Then
                aircraftATCText.Text = "<p>This aircraft has received an <strong class=""green_text"">outstanding</strong> quality rating (" & FormatNumber(r.Item("afmv_atfc").ToString, 2) & ") based on its technical condition relative to cost.</p>"
              ElseIf r.Item("afmv_atfc") < 4 Then
                aircraftATCText.Text = "<p>This aircraft has received a <strong class=""red_text"">below average</strong> quality rating (" & FormatNumber(r.Item("afmv_atfc").ToString, 2) & ") based on its technical condition relative to cost.</p>"
              End If
            End If



            If Not IsDBNull(r.Item("afmv_exposure")) Then
              maintenance_exposure_label.Text = clsGeneral.clsGeneral.ConvertIntoThousands(r.Item("afmv_exposure"))
              'htmlout2.Append("<tr valign='top'><td align='left' valign='top'>EXPOSURE</td><td align=""left"" valign=""top"">$" & FormatNumber(r.Item("afmv_exposure"), 0) & "</td></tr>")
            End If

            If Not IsDBNull(r.Item("afmv_source_id")) Then
              analysis_id_label.Text = results_table.Rows(0).Item("afmv_source_id").ToString
            End If

            If Not IsDBNull(r.Item("afmv_lower")) Then
              factors.Visible = True
              model_comparables_label.Text = clsGeneral.clsGeneral.ConvertIntoThousands(r.Item("afmv_lower"))
              htmlout2.Append("<tr valign='top'><td align='left' valign='top'>LOWER</td><td align=""left"" valign=""top"">$" & FormatNumber(r.Item("afmv_lower"), 0) & "</td></tr>")
            End If

            If Not IsDBNull(r.Item("afmv_upper")) Then
              factors.Visible = True
              If Not String.IsNullOrEmpty(model_comparables_label.Text) Then
                model_comparables_label.Text += " - "
              End If
              model_comparables_label.Text += clsGeneral.clsGeneral.ConvertIntoThousands(r.Item("afmv_upper"))
              htmlout2.Append("<tr valign='top'><td align='left' valign='top'>UPPER</td><td align=""left"" valign=""top"">$" & FormatNumber(r.Item("afmv_upper"), 0) & "</td></tr>")
            End If

            If Not IsDBNull(r.Item("afmv_equity")) Then
              factors.Visible = True
              htmlout2.Append("<tr valign='top'><td align='left' valign='top'>EQUITY</td><td align=""left"" valign=""top"">$" & FormatNumber(r.Item("afmv_equity"), 0) & "</td></tr>")
            End If

          Next

          ' htmlOut.Append("</table>")

        End If


        htmlout2.Append("</table>")


      End If


      aftt_page = htmlOut.ToString
      temp_factors = htmlout2.ToString

    Catch ex As Exception
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in generate_aftt_avg " + ex.Message
    End Try

  End Sub

  Private Sub generateQualityText(ByVal quality As Double)

    quality = FormatNumber(quality, 2)

    If quality >= 6 Then
      aircraftQualityText.Text = "<p>This aircraft has received an <strong class=""green_text"">outstanding</strong> quality rating (" & quality.ToString & ") based on its technical condition relative to maintenance and cost.</p>"
    ElseIf quality >= 5.5 And quality < 6 Then
      aircraftQualityText.Text = "<p>This aircraft has received an <strong class=""excellent_text"">excellent</strong> quality rating (" & quality.ToString & ") based on its technical condition relative to maintenance and cost.</p>"
    ElseIf quality >= 5 And quality < 5.5 Then
      aircraftQualityText.Text = "<p>This aircraft has received a <strong class=""verygood_text"">very good</strong> quality rating (" & quality.ToString & ") based on its technical condition relative to maintenance and cost.</p>"
    ElseIf quality >= 4.5 And quality < 5 Then
      aircraftQualityText.Text = "<p>This aircraft has received a <strong class=""good_text"">good</strong> quality rating (" & quality.ToString & ") based on its technical condition relative to maintenance and cost.</p>"
    ElseIf quality >= 4 And quality < 4.5 Then
      aircraftQualityText.Text = "<p>This aircraft has received an <strong class=""average_text"">average</strong> quality rating (" & quality.ToString & ") based on its technical condition relative to maintenance and cost.</p>"
    ElseIf quality < 4 Then
      aircraftQualityText.Text = "<p>This aircraft has received a <strong class=""red_text"">below average</strong> quality rating (" & quality.ToString & ") based on its technical condition relative to maintenance and cost.</p>"
    End If
  End Sub

  Public Sub generateModificationsFeaturesTable(ByVal nAircraftID As Long, ByRef sModificationsTable As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder
    Dim show_customer As Boolean = False
    Dim htmlOut_not As New StringBuilder

    Try


      If Trim(Request("customer")) = "Y" Then
        show_customer = True
      Else
        show_customer = False
      End If



      results_table = getAssetInsightResearchDataTable2(nAircraftID)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          If show_customer = True Then
            htmlOut.Append("<div class=""Box  removeLeftMargin""><table cellpadding=""0"" cellspacing=""0"" width='100%' class=""formatTable blue"">")
          Else
            htmlOut.Append("<table border=""1"" cellpadding=""2"" cellspacing=""0"">")
          End If

          ' first add the report title
          htmlOut.Append("<tr class=""noBorder""><td align=""left"" valign=""middle"" colspan=""" + results_table.Columns.Count.ToString + """><span class=""subHeader"">FEATURE ASSUMPTIONS</span></td></tr>")

          If show_customer = True Then
          Else
            ' second generate the header based off the column names in the datatable
            htmlOut.Append("<tr bgcolor=""#CCCCCC"">")
            For Each c As DataColumn In results_table.Columns
              If Trim(UCase(c.ColumnName)) <> "IS_MAPPED" Then
                htmlOut.Append("<td align=""left"">" + c.ColumnName.ToUpper.Replace("CCOUNT", "COUNT").Trim + "</td>")
              End If
            Next
            htmlOut.Append("</tr>")
          End If

          If show_customer = True Then
            htmlOut.Append("<tr class=""display_none"" id=""equippedRow""><td align=""left"" valign=""top""><strong>EQUIPPED WITH:</strong></td></tr>")
            htmlOut_not.Append("<tr class=""display_none"" id=""notequippedRow""><td align=""left"" valign=""top"">&nbsp;</td></tr><tr><td align=""left"" valign=""top""><strong>NOT EQUIPPED WITH OR REPORTED:</strong></td></tr>")
          End If

          Dim showEquipped As Boolean = False
          Dim showNotEquipped As Boolean = False
          ' second display the report data based off the column names in the datatable
          For Each r As DataRow In results_table.Rows

            If show_mapped = True And r.Item("is_mapped") = "N" Then
              If show_customer = True Then
                If Not IsDBNull(r.Item("INSTALLED")) Then
                  If Trim(r.Item("INSTALLED")) = "Y" Then
                    If Not IsDBNull(r.Item("ITEM")) Then
                      showEquipped = True
                      htmlOut.Append("<tr><td align=""left"" valign=""top""><font color='red'>" & Trim(r.Item("ITEM")) & "</font></td></tr>")
                    End If
                  Else
                    If Not IsDBNull(r.Item("ITEM")) Then
                      showNotEquipped = True
                      htmlOut_not.Append("<tr><td align=""left"" valign=""top""><font color='red'>" & Trim(r.Item("ITEM")) & "</font></td></tr>")
                    End If
                  End If
                End If
              Else
                htmlOut.Append("<tr>")
              End If
            Else
              If show_customer = True Then
                If Not IsDBNull(r.Item("INSTALLED")) Then
                  If Trim(r.Item("INSTALLED")) = "Y" Then
                    If Not IsDBNull(r.Item("ITEM")) Then
                      showEquipped = True
                      htmlOut.Append("<tr><td align=""left"" valign=""top"">" & Trim(r.Item("ITEM")) & "</td></tr>")
                    End If
                  Else
                    If Not IsDBNull(r.Item("ITEM")) Then
                      showNotEquipped = True
                      htmlOut_not.Append("<tr><td align=""left"" valign=""top"">" & Trim(r.Item("ITEM")) & "</td></tr>")
                    End If
                  End If
                End If
              Else
                htmlOut.Append("<tr>")
              End If
            End If
 
            

            ' ramble through each "column name" and display data
            For Each c As DataColumn In results_table.Columns

              If show_customer = True Then

              Else
                If show_mapped = True Then
                  If Trim(UCase(c.ColumnName)) <> "IS_MAPPED" Then
                    If r.Item("is_mapped") = "Y" Then
                      htmlOut.Append("<td align=""left"" valign=""top""><strong>" + r.Item(c.ColumnName).ToString.Trim + "</strong></td>")
                    Else
                      htmlOut.Append("<td align=""left"" valign=""top"">" + r.Item(c.ColumnName).ToString.Trim + "</td>")
                    End If
                  End If
                Else
                  htmlOut.Append("<td align=""left"" valign=""top"">" + r.Item(c.ColumnName).ToString.Trim + "</td>")
                End If
              End If

            Next


            If show_customer = True Then
            Else
              htmlOut.Append("</tr>")
            End If


          Next

          If show_customer = True Then
            htmlOut.Append(htmlOut_not)
          End If

          If showEquipped Then
            htmlOut.Replace(" class=""display_none"" id=""equippedRow""", " class=""noBorder"" id=""equippedRow""")
          End If


          If showNotEquipped Then
            htmlOut.Replace(" class=""display_none"" id=""notequippedRow""", " class=""noBorder"" id=""notequippedRow""")
          End If

          htmlOut.Append("</table>")

          If show_customer = True Then
            htmlOut.Append("</div>")
          End If

          sModificationsTable = htmlOut.ToString
        ElseIf results_table.Rows.Count = 0 Then
          modBox.Visible = False
          inspectionClass.Attributes.Remove("class")
          inspectionClass.Attributes.Add("class", "twelve columns")
        End If

      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in generateModificationsFeaturesTable(ByVal nAircraftID As Long, ByRef sModificationsTable As String) " + ex.Message

    Finally

    End Try

    'return resulting html string
    htmlOut = Nothing
    results_table = Nothing

  End Sub

















































  '------------ MAYBE TO BE SPLIT OUT LATER 
  Public Function Get_Model_Name(ByVal amod_id As Integer) As String
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlDataReader As System.Data.SqlClient.SqlDataReader : SqlDataReader = Nothing

    Dim tmpStr As String : tmpStr = ""

    Dim Query As String : Query = ""
    Query = "SELECT DISTINCT amod_make_name, amod_model_name FROM Aircraft_Model WITH(NOLOCK) WHERE amod_id = " + amod_id.ToString

    '    Query &= " " + commonEvo.GenerateProductCodeSelectionQuery(Session.Item("localSubscription"), False, True)
    Query = Query & clsGeneral.clsGeneral.GenerateProductCodeSelectionQuery_CRM(Session.Item("localSubscription"), False, False)

    Try

      'Select Case Application.Item("webHostObject").evoWebHostType
      'Case eWebSiteTypes.LOCAL
      'SqlConn.ConnectionString = My.Settings.DEFAULT_LOCAL_MSSQL
      '  Case Else
      SqlConn.ConnectionString = Session.Item("localPreferences").UserDatabaseConn
      'End Select

      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = Query.ToString

      SqlDataReader = SqlCommand.ExecuteReader()

      If SqlDataReader.HasRows Then
        SqlDataReader.Read()
        If Not (IsDBNull(SqlDataReader("amod_make_name")) And Not IsDBNull(SqlDataReader("amod_model_name"))) Then
          tmpStr = SqlDataReader("amod_make_name").ToString & " " & SqlDataReader("amod_model_name").ToString
        End If
      End If
      SqlDataReader.Close()
      SqlDataReader = Nothing
    Catch SqlException

      SqlConn.Dispose()
      SqlCommand.Dispose()

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Get_Model_Name: " & SqlException.Message

    Finally

      SqlCommand.Dispose()
      SqlConn.Close()
      SqlConn.Dispose()

    End Try

    Return tmpStr

  End Function

  Public Function Get_AC_DLV_YEAR(ByVal ac_id As Integer) As String
    Dim SqlException As System.Data.SqlClient.SqlException : SqlException = Nothing
    Dim SqlCommand As New System.Data.SqlClient.SqlCommand
    Dim SqlConn As New System.Data.SqlClient.SqlConnection
    Dim SqlDataReader As System.Data.SqlClient.SqlDataReader : SqlDataReader = Nothing

    Dim tmpStr As String : tmpStr = "0"

    Dim Query As String : Query = ""
    Query = "SELECT DISTINCT ac_year FROM Aircraft WITH(NOLOCK) WHERE ac_journ_id = 0 and ac_id = " + ac_id.ToString

    Try

      'Select Case Application.Item("webHostObject").evoWebHostType
      'Case eWebSiteTypes.LOCAL
      'SqlConn.ConnectionString = My.Settings.DEFAULT_LOCAL_MSSQL
      '  Case Else
      SqlConn.ConnectionString = Session.Item("localPreferences").UserDatabaseConn
      'End Select

      SqlConn.Open()

      SqlCommand.Connection = SqlConn
      SqlCommand.CommandTimeout = 1000
      SqlCommand.CommandText = Query.ToString

      SqlDataReader = SqlCommand.ExecuteReader()

      If SqlDataReader.HasRows Then
        SqlDataReader.Read()
        If Not (IsDBNull(SqlDataReader("ac_year"))) Then
          tmpStr = SqlDataReader("ac_year").ToString
        End If
      End If
      SqlDataReader.Close()
      SqlDataReader = Nothing
    Catch SqlException

      SqlConn.Dispose()
      SqlCommand.Dispose()

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in Get_AC_MFR_YEAR: " & SqlException.Message

    Finally

      SqlCommand.Dispose()
      SqlConn.Close()
      SqlConn.Dispose()

    End Try

    Return tmpStr

  End Function








End Class