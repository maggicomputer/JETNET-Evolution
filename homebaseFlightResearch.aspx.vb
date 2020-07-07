' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/homebaseFlightResearch.aspx.vb $
'$$Author: Mike $
'$$Date: 6/11/20 5:12p $
'$$Modtime: 6/11/20 5:07p $
'$$Revision: 5 $
'$$Workfile: homebaseFlightResearch.aspx.vb $
'
' ********************************************************************************

Partial Public Class homebaseFlightResearch
  Inherits System.Web.UI.Page

  Private sRegNumber As String = ""
  Private sTask As String = ""

  Public Shared masterPage As New Object

  Private Sub homebaseFlightResearch_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit

    Try

      If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER Then
        Me.MasterPageFile = "~/EvoStyles/CustomerAdminTheme.Master"
        masterPage = DirectCast(Page.Master, CustomerAdminTheme)
      ElseIf Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
        Me.MasterPageFile = "~/EvoStyles/HomebaseTheme.Master"
        masterPage = DirectCast(Page.Master, HomebaseTheme)
      End If

    Catch ex As Exception
      If Not IsNothing(masterPage) Then
        masterPage.LogError(System.Reflection.MethodInfo.GetCurrentMethod().ToString + " : " + ex.Message.ToString)
      Else
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " (homebaseFlightResearch_PreRender): " + ex.Message.ToString.Trim
      End If
    End Try

  End Sub

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Dim sErrorString As String = ""
    Dim graphID As Integer = 1

    Dim htmlFlightArrivalGraph As String = ""
    Dim htmlFlightArrivalGraphScript As String = ""
    Dim htmlFlightArrivalFunctionScript As String = ""

    If Session.Item("crmUserLogon") <> True Then

      Response.Redirect("Default.aspx", True)

    Else

      If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
        Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Homebase Flight Research - Home")
        Master.Set_Active_Tab(8)
      Else
        Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Admin Flight Research - Home")
      End If

      If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString), _
                                                            HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString, _
                                                            CLng(HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString), _
                                                            CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)) Then
        Response.Redirect("Default.aspx", True)
      End If

      If Not IsNothing(Request.Item("task")) Then
        If Not String.IsNullOrEmpty(Request.Item("task").ToString.Trim) Then
          sTask = Request.Item("task").ToString.ToUpper.Trim
        End If
      End If

      faaDataDatelbl.Text = generateFAADateTable()

      generateFAADataGraph(htmlFlightArrivalFunctionScript, htmlFlightArrivalGraph, graphID)

      htmlFlightArrivalGraphScript = vbCrLf + "<script type=""text/javascript"">" + vbCrLf
      htmlFlightArrivalGraphScript += "$(document).ready(function(){" + vbCrLf
      htmlFlightArrivalGraphScript += " drawVisualization" + graphID.ToString + "();" + vbCrLf
      htmlFlightArrivalGraphScript += "});" + vbCrLf
      htmlFlightArrivalGraphScript += htmlFlightArrivalFunctionScript.Trim
      htmlFlightArrivalGraphScript += "</script>" + vbCrLf

      System.Web.UI.ScriptManager.RegisterStartupScript(faa_flight_panel, Me.GetType(), "showUtilizationGraph" + graphID.ToString, htmlFlightArrivalGraphScript, False)

      faaDataGraphlbl.Text = htmlFlightArrivalGraph

      If IsPostBack And sTask.ToLower.Contains("run") And Not String.IsNullOrEmpty(reg_no.Text.Trim) Then
        sRegNumber = reg_no.Text

        If Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE Then
          Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title(reg_no.Text.Trim + " - Homebase Flight Research - " + WeekdayName(Weekday(Today)).ToString + ", " + MonthName(Month(Today)).ToString + " " + Day(Today).ToString + ", " + Year(Today).ToString)
          Master.Set_Active_Tab(8)
        Else
          Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title(reg_no.Text.Trim + " - Admin Flight Research - " + WeekdayName(Weekday(Today)).ToString + ", " + MonthName(Month(Today)).ToString + " " + Day(Today).ToString + ", " + Year(Today).ToString)
        End If

        FAAResearchDetailsLbl.Text = generateFAAFlightResearch(sRegNumber)

        AircraftDetailsLbl.Text = generateAircraftDetails(sRegNumber)

        flightAwareLink.Text = "<a class=""underline cursor"" href=""https://flightaware.com/live/flight/" + sRegNumber.Trim + """ target=""new"" title=""View On Flight Aware""><strong>View On Flight Aware</strong></a>"

      End If

    End If

  End Sub

  Public Function getFAAFlightResearchDataTable(ByVal sRegNumber As String) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT DISTINCT fxfa_aircraftid AS REGNO,")
      sQuery.Append(" fxfa_departurePointAirport AS DEPARTFROM,")
      sQuery.Append(" fxfa_etdTime AS DEPARTTIME,")
      sQuery.Append(" fxfa_arrivalPointAirport AS ARRIVETO,")
      sQuery.Append(" fxfa_etaTime AS ARRIVETIME,")
      sQuery.Append(" (SELECT TOP 1 fxfp_flightAircraftSpecs FROM FAA_XML_Flight_Plan WITH(NOLOCK)")
      sQuery.Append("  WHERE fxfa_aircraftId = fxfp_aircraftid AND fxfa_gufi = fxfp_gufi) AS MODELCODE,")
      sQuery.Append(" datediff(minute,fxfa_etdTime,fxfa_etaTime) AS FLIGHTTIME")
      sQuery.Append(" FROM FAA_XML_Flight_Arrivals WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft_Reg_No WITH(NOLOCK) ON fxfa_aircraftId = acreg_reg_no_search")
      sQuery.Append(" WHERE fxfa_etatype='ACTUAL'")
      sQuery.Append(" AND fxfa_arrivalPointAirport <> '' AND fxfa_departurePointAirport <> ''")
      sQuery.Append(" AND fxfa_aircraftId = '" + sRegNumber.Trim + "'")
      sQuery.Append(" ORDER BY fxfa_etdTime DESC")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getFAAFlightResearchDataTable(ByVal sRegNumber As String) As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = "database=jetnet_ra_faa;server=10.10.254.60;User Id=FAA_Apps;Password=JN_FAA_SQL06!;Connection Timeout=500"

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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getFAAFlightResearchDataTable load datatable</b><br /> " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getFAAFlightResearchDataTable(ByVal sRegNumber As String) As DataTable</b><br />" + ex.Message

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

  Public Function generateFAAFlightResearch(ByVal sRegNumber As String) As String

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Try

      results_table = getFAAFlightResearchDataTable(sRegNumber)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<table border=""1"" cellpadding=""2"" cellspacing=""0"">")

          ' first add the report title
          htmlOut.Append("<tr><td align=""left"" valign=""middle"" colspan=""" + results_table.Columns.Count.ToString + """><b>Flight Data For " + sRegNumber.Trim + "</b></td></tr>")

          ' second generate the header based off the column names in the datatable
          htmlOut.Append("<tr bgcolor=""#CCCCCC"">")
          For Each c As DataColumn In results_table.Columns
            htmlOut.Append("<td align=""left"">" + c.ColumnName.ToUpper.Replace("CCOUNT", "COUNT").Trim + "</td>")
          Next
          htmlOut.Append("</tr>")

          ' second display the report data based off the column names in the datatable
          For Each r As DataRow In results_table.Rows

            htmlOut.Append("<tr>")

            ' ramble through each "column name" and display data
            For Each c As DataColumn In results_table.Columns
              htmlOut.Append("<td align=""left"" valign=""top"">" + r.Item(c.ColumnName).ToString.Trim + "</td>")
            Next

            htmlOut.Append("</tr>")

          Next

          htmlOut.Append("</table>")

        End If

      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in generateFAAFlightResearch(ByVal sRegNumber As String) " + ex.Message

    Finally

    End Try

    'return resulting html string
    Return htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Function

  Public Function GetAllAircraftInfo_dataTable(ByVal sRegNumber As String) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    'Dim SqlException As SqlClient.SqlException

    Try

      sQuery.Append("SELECT DISTINCT amod_make_name AS MAKE, amod_model_name AS MODEL, amod_airframe_type_code AS AIRFRAME, amod_type_code AS TYPE, ac_ser_no_full AS SERIALNO, ac_id AS ACID")
      sQuery.Append(" FROM Aircraft WITH(NOLOCK) INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id")
      sQuery.Append(" WHERE ac_reg_no_search = '" + sRegNumber.Trim + "' AND ac_journ_id = 0")

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 90

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()

        Return Nothing

      End Try

    Catch ex As Exception

      Return Nothing

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return atemptable

  End Function

  Public Function generateAircraftDetails(ByVal sRegNumber As String) As String

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Try

      results_table = GetAllAircraftInfo_dataTable(sRegNumber)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count = 1 Then

          htmlOut.Append("<table border=""1"" cellpadding=""2"" cellspacing=""0"">")

          ' first add the report title
          htmlOut.Append("<tr><td align=""left"" valign=""middle"" colspan=""" + results_table.Columns.Count.ToString + """><b>Aircraft Details For " + sRegNumber.Trim + "</b></td></tr>")

          ' second generate the header based off the column names in the datatable
          htmlOut.Append("<tr bgcolor=""#CCCCCC"">")
          For Each c As DataColumn In results_table.Columns
            htmlOut.Append("<td align=""left"">" + c.ColumnName.ToUpper.Replace("CCOUNT", "COUNT").Trim + "</td>")
          Next
          htmlOut.Append("</tr>")

          ' second display the report data based off the column names in the datatable
          For Each r As DataRow In results_table.Rows

            htmlOut.Append("<tr>")

            ' ramble through each "column name" and display data
            For Each c As DataColumn In results_table.Columns
              htmlOut.Append("<td align=""left"" valign=""top"">" + r.Item(c.ColumnName).ToString.Trim + "</td>")
            Next

            htmlOut.Append("</tr>")

          Next

          htmlOut.Append("</table>")

        Else

          htmlOut.Append("<table border=""1"" cellpadding=""2"" cellspacing=""0"">")
          htmlOut.Append("<tr><td align=""left"" valign=""top""><strong>MORE THAN ONE Aircraft Matched " + sRegNumber.Trim + "</strong></td></tr>")
          htmlOut.Append("</table>")

        End If

      Else

        htmlOut.Append("<table border=""1"" cellpadding=""2"" cellspacing=""0"">")
        htmlOut.Append("<tr><td align=""left"" valign=""top""><strong>No Aircraft Matched " + sRegNumber.Trim + "</strong></td></tr>")
        htmlOut.Append("</table>")

      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in generateAircraftDetails(ByVal sRegNumber As String) " + ex.Message

    Finally

    End Try

    'return resulting html string
    Return htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Function

  Public Function getFAADataDatesDataTable(Optional ByVal bFromGraphFunction As Boolean = False) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT DISTINCT MONTH(fxfa_etaTime) as [MONTH], DAY(fxfa_etaTime) as [DAY], COUNT(*) AS ARRIVALS,")
      sQuery.Append(" (SELECT COUNT(*) FROM FAA_XML_Flight_Arrivals b WITH(NOLOCK) WHERE b.fxfa_etatype='ACTUAL' AND MONTH(b.fxfa_etaTime) = MONTH(a.fxfa_etaTime)")
      sQuery.Append(" AND DAY(b.fxfa_etaTime) = DAY(a.fxfa_etaTime)")
      sQuery.Append(" AND b.fxfa_arrivalPointAirport <> '' AND b.fxfa_departurePointAirport <> ''")
      sQuery.Append(" AND b.fxfa_processed_status like '%FAA Flight Data Added%') AS ADDED")
      sQuery.Append(" FROM FAA_XML_Flight_Arrivals a WITH(NOLOCK)")
      sQuery.Append(" WHERE fxfa_etatype='ACTUAL' AND fxfa_arrivalPointAirport <> '' AND fxfa_departurePointAirport <> ''")

      If bFromGraphFunction Then
        sQuery.Append(" AND (DATEDIFF(m, fxfa_etaTime, GETDATE()) < 1)")
      Else
        sQuery.Append(" AND fxfa_etaTime >= GETDATE()-7")
      End If
      sQuery.Append(" GROUP BY MONTH(fxfa_etaTime), DAY(fxfa_etaTime)")
      sQuery.Append(" ORDER BY MONTH(fxfa_etaTime), DAY(fxfa_etaTime)")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getFAADataDatesDataTable() As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = "database=jetnet_ra_faa;server=10.10.254.60;User Id=FAA_Apps;Password=JN_FAA_SQL06!;Connection Timeout=500"

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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getFAADataDatesDataTable load datatable</b><br /> " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getFAADataDatesDataTable() As DataTable</b><br />" + ex.Message

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

  Public Function generateFAADateTable() As String

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Try

      results_table = getFAADataDatesDataTable()

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<table border=""1"" cellpadding=""2"" cellspacing=""0"">")

          ' first add the report title
          htmlOut.Append("<tr><td align=""left"" valign=""middle"" colspan=""" + results_table.Columns.Count.ToString + """><b>FAA Data Dates</b></td></tr>")

          ' second generate the header based off the column names in the datatable
          htmlOut.Append("<tr bgcolor=""#CCCCCC"">")
          For Each c As DataColumn In results_table.Columns
            htmlOut.Append("<td align=""left"">" + c.ColumnName.ToUpper.Replace("CCOUNT", "COUNT").Trim + "</td>")
          Next
          htmlOut.Append("</tr>")

          ' second display the report data based off the column names in the datatable
          For Each r As DataRow In results_table.Rows

            htmlOut.Append("<tr>")

            ' ramble through each "column name" and display data
            For Each c As DataColumn In results_table.Columns
              htmlOut.Append("<td align=""left"" valign=""top"">" + r.Item(c.ColumnName).ToString.Trim + "</td>")
            Next

            htmlOut.Append("</tr>")

          Next

          htmlOut.Append("</table>")

        End If

      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in generateFAADateTable() " + ex.Message

    Finally

    End Try

    'return resulting html string
    Return htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Function

  Public Sub generateFAADataGraph(ByRef out_scriptString As String, ByRef out_htmlString As String, ByVal graphID As Integer)

    Dim htmlOut As New StringBuilder
    Dim scriptOut As New StringBuilder
    Dim results_table As New DataTable

    Dim x As Integer = 0

    Dim sWeek As String = ""
    Dim sTmpDay As String = ""

    Dim sDayArray() As String = Split("1,2,3,4,5,6,7", ",")
    Dim sWeekArray() As String = Nothing

    Dim afiltered_Rows As DataRow() = Nothing

    Try

      results_table = getFAADataDatesDataTable(True)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          scriptOut.Append("google.charts.load('current', {packages: ['corechart']});" + vbCrLf)
          scriptOut.Append("google.charts.setOnLoadCallback(drawVisualization" + graphID.ToString + ");" + vbCrLf)
          scriptOut.Append("function drawVisualization" + graphID.ToString + "() {" + vbCrLf)
          'scriptOut.Append(" alert('drawVisualization" + graphID.ToString + "');" + vbCrLf)  
          scriptOut.Append(" var data = new google.visualization.DataTable();" + vbCrLf)

          scriptOut.Append(" data.addColumn('number', 'Day');" + vbCrLf)

          For Each r As DataRow In results_table.Rows

            Select Case CInt(r.Item("DAY").ToString)

              Case Is < 8
                If Not commonEvo.inMyArray(sWeek.Split(","), "1") Then

                  If String.IsNullOrEmpty(sWeek.Trim) Then
                    sWeek = "1"
                  Else
                    sWeek += "," + "1"
                  End If

                  scriptOut.Append(" data.addColumn('number', 'Arrivals Wk 1');" + vbCrLf)

                End If

              Case Is < 15
                If Not commonEvo.inMyArray(sWeek.Split(","), "2") Then

                  If String.IsNullOrEmpty(sWeek.Trim) Then
                    sWeek = "2"
                  Else
                    sWeek += "," + "2"
                  End If

                  scriptOut.Append(" data.addColumn('number', 'Arrivals Wk 2');" + vbCrLf)

                End If

              Case Is < 22
                If Not commonEvo.inMyArray(sWeek.Split(","), "3") Then

                  If String.IsNullOrEmpty(sWeek.Trim) Then
                    sWeek = "3"
                  Else
                    sWeek += "," + "3"
                  End If

                  scriptOut.Append(" data.addColumn('number', 'Arrivals Wk 3');" + vbCrLf)

                End If

              Case Is < 29
                If Not commonEvo.inMyArray(sWeek.Split(","), "4") Then

                  If String.IsNullOrEmpty(sWeek.Trim) Then
                    sWeek = "4"
                  Else
                    sWeek += "," + "4"
                  End If

                  scriptOut.Append(" data.addColumn('number', 'Arrivals Wk 4');" + vbCrLf)

                End If

              Case Is < 32
                If Not commonEvo.inMyArray(sWeek.Split(","), "5") Then

                  If String.IsNullOrEmpty(sWeek.Trim) Then
                    sWeek = "5"
                  Else
                    sWeek += "," + "5"
                  End If

                  scriptOut.Append(" data.addColumn('number', 'Arrivals Wk 5');" + vbCrLf)

                End If

            End Select

          Next

          sWeekArray = sWeek.Split(",")

          scriptOut.Append(" data.addRows([")

          For Each strDay As String In sDayArray

            scriptOut.Append(IIf(CInt(strDay.Trim) > 1, ", [" + strDay.Trim, " [" + strDay.Trim))

            For Each strWk As String In sWeekArray

              Select Case CInt(strWk.Trim)

                Case 1
                  sTmpDay = (CInt(strDay) + 0).ToString
                Case 2
                  sTmpDay = (CInt(strDay) + 7).ToString
                Case 3
                  sTmpDay = (CInt(strDay) + 14).ToString
                Case 4
                  sTmpDay = (CInt(strDay) + 21).ToString
                Case 5
                  sTmpDay = (CInt(strDay) + 28).ToString

              End Select

              afiltered_Rows = results_table.Select("DAY = " + sTmpDay.Trim, "")

              If afiltered_Rows.Count > 0 Then

                For Each r As DataRow In afiltered_Rows

                  If Not IsDBNull(r.Item("ARRIVALS")) Then
                    If Not String.IsNullOrEmpty(r.Item("ARRIVALS").ToString.Trim) Then

                      If CLng(r.Item("ARRIVALS").ToString) > 0 Then
                        scriptOut.Append("," + r.Item("ARRIVALS").ToString)
                      Else
                        scriptOut.Append(",0")
                      End If

                      Exit For

                    Else
                      scriptOut.Append(",0")
                      Exit For
                    End If

                  Else
                    scriptOut.Append(",0")
                    Exit For
                  End If

                Next

              Else
                scriptOut.Append(",0")
              End If

            Next

            scriptOut.Append("]")

          Next

          scriptOut.Append("]);" + vbCrLf)

          scriptOut.Append("var options = { " + vbCrLf)
          scriptOut.Append("  chartArea:{width:'80%',height:'75%'}," + vbCrLf)
          scriptOut.Append("  hAxis: { title: 'Day of week'," + vbCrLf)
          scriptOut.Append("           textStyle: { color: '#01579b', fontSize: 14, fontName:  'Arial', bold: true, italic: true }, " + vbCrLf)
          scriptOut.Append("           titleTextStyle: { color: '#01579b', fontSize: 14, fontName:  'Arial', bold: false, italic: true }" + vbCrLf)
          scriptOut.Append("         }," + vbCrLf)
          scriptOut.Append("  vAxis: { title: 'Arrivals'," + vbCrLf)
          scriptOut.Append("           textStyle: { color: '#1a237e', fontSize: 14, bold: true }," + vbCrLf)
          scriptOut.Append("           titleTextStyle: { color: '#1a237e', fontSize: 16, bold: true }" + vbCrLf)
          scriptOut.Append("        }," + vbCrLf)
          scriptOut.Append("  smoothLine:true," + vbCrLf)
          scriptOut.Append("  legend:'top'," + vbCrLf)
          scriptOut.Append("  colors: ['black','red', 'blue', 'green', 'orange']" + vbCrLf)
          scriptOut.Append("};" + vbCrLf)


          scriptOut.Append(" var chart = new google.visualization.LineChart(document.getElementById('visualization" + graphID.ToString + "'));" + vbCrLf)
          scriptOut.Append(" chart.draw(data, options);" + vbCrLf)
          scriptOut.Append("}" + vbCrLf)

        End If

      End If

      If Not String.IsNullOrEmpty(scriptOut.ToString.Trim) Then
        htmlOut.Append("<table id=""flightActivityTable"" width=""100%"" cellspacing=""0"" cellpadding=""4"">")
        htmlOut.Append("<tr><td valign=""top"" align=""left""><div id='visualization" + graphID.ToString + "' style=""height:295px;""></div></td></tr>")
        htmlOut.Append("</table>" + vbCrLf)
      Else
        htmlOut.Append("<table id=""flightActivityTable"" width=""100%"" cellspacing=""0"" cellpadding=""4"">")
        htmlOut.Append("<tr><td valign=""middle"" align=""center"">No Flight Utilization Data at this time, for this Make/Model ...</td></tr>")
        htmlOut.Append("</table>" + vbCrLf)
      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in generateFAADataGraph(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, Optional ByVal faa_date As String = """") " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_scriptString = scriptOut.ToString
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

End Class