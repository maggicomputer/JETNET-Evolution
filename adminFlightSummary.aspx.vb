
' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/adminFlightSummary.aspx.vb $
'$$Author: Mike $
'$$Date: 6/11/20 5:12p $
'$$Modtime: 6/11/20 4:04p $
'$$Revision: 8 $
'$$Workfile: adminFlightSummary.aspx.vb $
'
' ********************************************************************************

Partial Public Class adminFlightSummary
  Inherits System.Web.UI.Page

  Private sTask As String = ""

  Public Shared masterPage As New Object

  Private Sub adminFlightSummary_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit

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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " (adminFlightSummary_PreInit): " + ex.Message.ToString.Trim
      End If
    End Try

  End Sub

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Dim sErrorString As String = ""
    Dim graphID As Integer = 1

    Dim htmlFlightSummaryGraph As String = ""
    Dim htmlFlightSummaryGraphScript As String = ""
    Dim htmlFlightSummaryFunctionScript As String = ""

    If Session.Item("crmUserLogon") <> True Then

      Response.Redirect("Default.aspx", True)

    Else
      Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Admin Flight Data Summary")

      If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString), _
                                                            HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString, _
                                                            CLng(HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString), _
                                                            CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)) Then
        Response.Redirect("Default.aspx", True)
      End If

      masterPage.Set_Active_Tab(10)

      If Not IsNothing(Request.Item("task")) Then
        If Not String.IsNullOrEmpty(Request.Item("task").ToString.Trim) Then
          sTask = Request.Item("task").ToString.ToUpper.Trim
        End If
      End If

      flightSummaryDataDatelbl.Text = "<div align=""left"" valign=""top"" style=""height:400px; overflow: auto; padding-right:8px;""><p>" + generateFlightSummaryDateTable() + "</p></div>"

      generateFlightSummaryDataGraph(htmlFlightSummaryFunctionScript, htmlFlightSummaryGraph, graphID)

      htmlFlightSummaryGraphScript = vbCrLf + "<script type=""text/javascript"">" + vbCrLf
      htmlFlightSummaryGraphScript += "$(document).ready(function(){" + vbCrLf
      htmlFlightSummaryGraphScript += " drawVisualization" + graphID.ToString + "();" + vbCrLf
      htmlFlightSummaryGraphScript += "});" + vbCrLf
      htmlFlightSummaryGraphScript += htmlFlightSummaryFunctionScript.Trim
      htmlFlightSummaryGraphScript += "</script>" + vbCrLf

      System.Web.UI.ScriptManager.RegisterStartupScript(flight_summary_panel, Me.GetType(), "showFlightSummaryGraph" + graphID.ToString, htmlFlightSummaryGraphScript, False)

      flightSummaryDataGraphlbl.Text = htmlFlightSummaryGraph

    End If

  End Sub

  Public Function getFlightSummaryDataDatesDataTable() As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      'select distinct  year(ffd_date), month(ffd_date), day(ffd_date) , (cast(month(ffd_date) as CHAR(4))+ '-'+cast(day(ffd_date) as CHAR(2))) as MONTHDAY,
      'sum(case when ffd_data_source='FAA-LIVE' then 1 else 0 end) as FAALIVE,
      'sum(case when ffd_data_source='FAA-FOIA' then 1 else 0 end) as FAAFOIA,
      'sum(case when ffd_data_source='F-AWARE' then 1 else 0 end) as FAWARE,
      'COUNT(*) as FLIGHTS
      'from View_Flights with (NOLOCK)
      'where ffd_date >= '1/1/2018'
      'group by  year(ffd_date),month(ffd_date), day(ffd_date), (cast(month(ffd_date) as CHAR(4))+ '-'+cast(day(ffd_date) as CHAR(2)))
      'order by year(ffd_date),month(ffd_date), day(ffd_date)

      sQuery.Append("SELECT DISTINCT MONTH(ffd_date) as [MONTH], DAY(ffd_date) as [DAY], YEAR(ffd_date) as [YEAR], ")
      'sQuery.Append(" (cast(month(ffd_date) as CHAR(4)) + '-' + cast(day(ffd_date) as CHAR(2))) as MONTHDAY,")
      sQuery.Append(" sum(case when ffd_data_source='FAA-LIVE' then 1 else 0 end) as FAALIVE,")
      sQuery.Append(" sum(case when ffd_data_source='FAA-FOIA' then 1 else 0 end) as FAAFOIA,")
      sQuery.Append(" sum(case when ffd_data_source='F-AWARE' then 1 else 0 end) as FAWARE,")
      sQuery.Append(" COUNT(*) as TOTALFLTS")
      sQuery.Append(" FROM View_Flights WITH(NOLOCK)")

      sQuery.Append(" WHERE ffd_date >= '01/01/" + (Year(Now()) - 1).ToString + "'")

      sQuery.Append(" GROUP BY MONTH(ffd_date), DAY(ffd_date),  YEAR(ffd_date) ")
      'sQuery.Append(" , (cast(month(ffd_date) as CHAR(4)) + '-' + cast(day(ffd_date) as CHAR(2)))")
      sQuery.Append(" ORDER BY  YEAR(ffd_date) asc, MONTH(ffd_date), DAY(ffd_date)")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getUserSummaryDatesDataTable() As DataTable</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim

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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getUserSummaryDatesDataTable load datatable</b><br /> " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getUserSummaryDatesDataTable() As DataTable</b><br />" + ex.Message

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

  Public Function generateFlightSummaryDateTable() As String

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Try

      results_table = getFlightSummaryDataDatesDataTable()

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<table border=""1"" cellpadding=""2"" cellspacing=""0"" width=""100%"">")

          ' first add the report title
          htmlOut.Append("<tr><td align=""left"" valign=""middle"" colspan=""" + results_table.Columns.Count.ToString + """><b>Flight Data Source</b></td></tr>")

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

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in generateUserSummaryTable() " + ex.Message

    Finally

    End Try

    'return resulting html string
    Return htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Function

  Public Sub generateFlightSummaryDataGraph(ByRef out_scriptString As String, ByRef out_htmlString As String, ByVal graphID As Integer)

    Dim htmlOut As New StringBuilder
    Dim scriptOut As New StringBuilder
    Dim results_table As New DataTable

    Dim x As Integer = 0

    Dim sMonth As String = ""

    Dim sDayArray() As String = Split("1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31", ",")
    Dim sMonthArray() As String = Nothing
    Dim sYearArray() As String = Split("" & Year(Now()) - 1 & "," & Year(Now()) & "", ",")

    Dim afiltered_Rows As DataRow() = Nothing
    Dim bFirstTime = True

    Try

      results_table = getFlightSummaryDataDatesDataTable()

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          scriptOut.Append("google.charts.load('current', {packages: ['corechart']});" + vbCrLf)
          scriptOut.Append("google.charts.setOnLoadCallback(drawVisualization" + graphID.ToString + ");" + vbCrLf)
          scriptOut.Append("function drawVisualization" + graphID.ToString + "() {" + vbCrLf)
          'scriptOut.Append(" alert('drawVisualization" + graphID.ToString + "');" + vbCrLf)  google.charts.setOnLoadCallback(drawChart);
          scriptOut.Append(" var data = new google.visualization.DataTable();" + vbCrLf)

          scriptOut.Append(" data.addColumn('string', 'Date');" + vbCrLf)

          For Each r As DataRow In results_table.Rows

            If Not commonEvo.inMyArray(sMonth.Split(","), r.Item("MONTH").ToString) Then

              If String.IsNullOrEmpty(sMonth.Trim) Then
                sMonth = r.Item("MONTH").ToString
              Else
                sMonth += "," + r.Item("MONTH").ToString
              End If

            End If


          Next

          sMonthArray = sMonth.Split(",")

          scriptOut.Append(" data.addColumn('number', 'LIVE');" + vbCrLf)
          scriptOut.Append(" data.addColumn('number', 'FOIA');" + vbCrLf)
          scriptOut.Append(" data.addColumn('number', 'FLTWARE');" + vbCrLf)
          scriptOut.Append(" data.addColumn('number', 'Total');" + vbCrLf)

          scriptOut.Append(" data.addRows([")

          For Each strYear As String In sYearArray

            For Each strMO As String In sMonthArray

              For Each strDay As String In sDayArray

                afiltered_Rows = results_table.Select("MONTH = " + strMO.Trim + " AND DAY = " + strDay.Trim + " AND YEAR = " + strYear.Trim, "")

                If afiltered_Rows.Count > 0 Then

                  scriptOut.Append(IIf(Not bFirstTime, ", ['" + strMO.Trim + "-" + strDay.Trim + "-" + strYear.Trim + "'", " ['" + strMO.Trim + "-" + strDay.Trim + "-" + strYear.Trim + "'"))

                  For Each r As DataRow In afiltered_Rows

                    If Not IsDBNull(r.Item("FAALIVE")) Then
                      If Not String.IsNullOrEmpty(r.Item("FAALIVE").ToString.Trim) Then

                        If CLng(r.Item("FAALIVE").ToString) > 0 Then
                          scriptOut.Append("," + r.Item("FAALIVE").ToString)
                        Else
                          scriptOut.Append(",0")
                        End If

                      Else
                        scriptOut.Append(",0")
                      End If

                    Else
                      scriptOut.Append(",0")
                    End If

                    If Not IsDBNull(r.Item("FAAFOIA")) Then
                      If Not String.IsNullOrEmpty(r.Item("FAAFOIA").ToString.Trim) Then

                        If CLng(r.Item("FAAFOIA").ToString) > 0 Then
                          scriptOut.Append("," + r.Item("FAAFOIA").ToString)
                        Else
                          scriptOut.Append(",0")
                        End If


                      Else
                        scriptOut.Append(",0")
                      End If

                    Else
                      scriptOut.Append(",0")
                    End If

                    If Not IsDBNull(r.Item("FAWARE")) Then
                      If Not String.IsNullOrEmpty(r.Item("FAWARE").ToString.Trim) Then

                        If CLng(r.Item("FAWARE").ToString) > 0 Then
                          scriptOut.Append("," + r.Item("FAWARE").ToString)
                        Else
                          scriptOut.Append(",0")
                        End If

                      Else
                        scriptOut.Append(",0")
                      End If

                    Else
                      scriptOut.Append(",0")
                    End If

                    If Not IsDBNull(r.Item("TOTALFLTS")) Then
                      If Not String.IsNullOrEmpty(r.Item("TOTALFLTS").ToString.Trim) Then

                        If CLng(r.Item("TOTALFLTS").ToString) > 0 Then
                          scriptOut.Append("," + r.Item("TOTALFLTS").ToString)
                        Else
                          scriptOut.Append(",0")
                        End If
                      Else
                        scriptOut.Append(",0")
                      End If

                    Else
                      scriptOut.Append(",0")
                    End If

                  Next ' DataRow

                  bFirstTime = False

                  scriptOut.Append("]")

                End If ' afiltered_Rows.Count > 0 Then

              Next ' strDay

            Next ' strMO
          Next

          scriptOut.Append("]);" + vbCrLf)

          scriptOut.Append("var options = { " + vbCrLf)
          scriptOut.Append("  chartArea:{width:'80%',height:'75%'}," + vbCrLf)
          scriptOut.Append("  hAxis: { title: 'Date'," + vbCrLf)
          scriptOut.Append("           textStyle: { color: '#01579b', fontSize: 14, fontName:  'Arial', bold: true, italic: true }, " + vbCrLf)
          scriptOut.Append("           titleTextStyle: { color: '#01579b', fontSize: 14, fontName:  'Arial', bold: false, italic: true }" + vbCrLf)
          scriptOut.Append("         }," + vbCrLf)
          scriptOut.Append("  vAxis: { title: 'Records'," + vbCrLf)
          scriptOut.Append("           textStyle: { color: '#1a237e', fontSize: 14, bold: true }," + vbCrLf)
          scriptOut.Append("           titleTextStyle: { color: '#1a237e', fontSize: 16, bold: true }" + vbCrLf)
          scriptOut.Append("        }," + vbCrLf)
          scriptOut.Append("  smoothLine:false," + vbCrLf)
          scriptOut.Append("  legend:'top'," + vbCrLf)
          scriptOut.Append("  colors: ['black','red', 'blue', 'green', 'orange']" + vbCrLf)
          scriptOut.Append("};" + vbCrLf)


          scriptOut.Append(" var chart = new google.visualization.LineChart(document.getElementById('visualization" + graphID.ToString + "'));" + vbCrLf)
          scriptOut.Append(" chart.draw(data, options);" + vbCrLf)
          scriptOut.Append("}" + vbCrLf)

        End If

      End If

      If Not String.IsNullOrEmpty(scriptOut.ToString.Trim) Then
        htmlOut.Append("<table id=""flightSummaryTable"" width=""100%"" cellspacing=""0"" cellpadding=""4"">")
        htmlOut.Append("<tr><td valign=""top"" align=""left""><div id='visualization" + graphID.ToString + "' style=""height:395px;""></div></td></tr>")
        htmlOut.Append("</table>" + vbCrLf)
      Else
        htmlOut.Append("<table id=""flightSummaryTable"" width=""100%"" cellspacing=""0"" cellpadding=""4"">")
        htmlOut.Append("<tr><td valign=""middle"" align=""center"">No Flight Summary Data at this time ...</td></tr>")
        htmlOut.Append("</table>" + vbCrLf)
      End If

    Catch ex As Exception

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in generateUserSummaryDataGraph(ByRef out_scriptString As String, ByRef out_htmlString As String, ByVal graphID As Integer) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_scriptString = scriptOut.ToString
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

End Class