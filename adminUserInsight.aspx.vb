
' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/adminUserInsight.aspx.vb $
'$$Author: Mike $
'$$Date: 7/18/19 9:14p $
'$$Modtime: 7/18/19 9:12p $
'$$Revision: 4 $
'$$Workfile: adminUserInsight.aspx.vb $
'
' ********************************************************************************

Partial Public Class adminUserInsight
  Inherits System.Web.UI.Page

  Private sTask As String = ""

  Public Shared masterPage As New Object

  Private Sub adminUserInsight_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit

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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += " (adminUserInsight_Init): " + ex.Message.ToString.Trim
      End If
    End Try

  End Sub

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Dim sErrorString As String = ""

    Dim graphID As Integer = 1

    Dim htmlUserSummaryGraphScript As String = ""
    Dim htmlUserSummaryGraph As String = ""
    Dim htmlUserSummaryFunctionScript As String = ""

    If Session.Item("crmUserLogon") <> True Then

      Response.Redirect("Default.aspx", True)

    Else
      Page.Header.Title = clsGeneral.clsGeneral.Set_Page_Title("Admin User Data Summary")

      If Not Session.Item("localPreferences").loadUserSession(sErrorString, CLng(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString),
                                                            HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString,
                                                            CLng(HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString),
                                                            CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)) Then
        Response.Redirect("Default.aspx", True)
      End If

      masterPage.Set_Active_Tab(12)

      If Not IsNothing(Request.Item("task")) Then
        If Not String.IsNullOrEmpty(Request.Item("task").ToString.Trim) Then
          sTask = Request.Item("task").ToString.ToUpper.Trim
        End If
      End If

      If Not String.IsNullOrEmpty(insightAreaddl.SelectedValue.ToString.Trim) Then

        user_insight_DataDatelbl.Text = "<div align=""left"" valign=""top"" style=""height:300px; overflow: auto; padding-right:8px;""><p>" + generateUserSummaryTable() + "</p></div>"

        generateUserSummaryDataGraph(htmlUserSummaryFunctionScript, htmlUserSummaryGraph, graphID)

        htmlUserSummaryGraphScript = vbCrLf + "<script type=""text/javascript"">" + vbCrLf '

        htmlUserSummaryGraphScript += "$(document).ready(function(){" + vbCrLf
        htmlUserSummaryGraphScript += " drawVisualization" + graphID.ToString + "();" + vbCrLf
        htmlUserSummaryGraphScript += " CloseLoadingMessage(""DivLoadingMessage"");" + vbCrLf
        htmlUserSummaryGraphScript += "});" + vbCrLf
        htmlUserSummaryGraphScript += htmlUserSummaryFunctionScript.Trim

        htmlUserSummaryGraphScript += "</script>" + vbCrLf

        System.Web.UI.ScriptManager.RegisterStartupScript(user_insight_panel, Me.GetType(), "showUserSummaryGraph" + graphID.ToString, htmlUserSummaryGraphScript.Trim, False)

        user_insight_DataGraphlbl.Text = htmlUserSummaryGraph.Trim

      End If

    End If

  End Sub


  Public Function getUserSummaryDatesDataTable() As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT DISTINCT COUNT(DISTINCT subins_contact_id) AS UNIQUEUSERS, COUNT(*) AS VISITS,")
      sQuery.Append(" (COUNT(*)/DATEDIFF(d,'1/1/2019', getdate())) AS AVGVISITSPERDAY,")

      sQuery.Append(" (")
      sQuery.Append("  ( SELECT COUNT(DISTINCT subins_contact_id + ' ' + datepart(y, subislog_date))")
      sQuery.Append("    FROM Subscription_Install_Log WITH(NOLOCK)")
      sQuery.Append("    INNER JOIN Subscription_Install WITH(NOLOCK) ON subislog_subid = subins_sub_id AND subislog_login = subins_login AND subislog_seq_no = subins_seq_no")
      sQuery.Append("    WHERE subislog_message LIKE(@subislog_message) AND subislog_date >= '1/1/" + (Year(Now())).ToString + "'")
      sQuery.Append("    AND subislog_subid NOT IN (SELECT DISTINCT sub_id FROM subscription WITH(NOLOCK) WHERE sub_comp_id = 135887)")
      sQuery.Append("   ) / DATEDIFF(d,'1/1/" + (Year(Now())).ToString + "', getdate())")
      sQuery.Append(" ) AS UNIQUEUSERSPERDAY")

      sQuery.Append(" FROM Subscription_Install_Log WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Subscription_Install WITH(NOLOCK) ON subislog_subid = subins_sub_id AND subislog_login = subins_login AND subislog_seq_no = subins_seq_no")

      sQuery.Append(" WHERE subislog_message LIKE(@subislog_message) AND subislog_date >= '1/1/" + (Year(Now())).ToString + "'")
      sQuery.Append(" AND subislog_subid NOT IN (SELECT DISTINCT sub_id FROM subscription WITH(NOLOCK) WHERE sub_comp_id = 135887)")

      SqlCommand.Parameters.AddWithValue("@subislog_message", "%" + insightAreaddl.SelectedValue.ToString.Trim + "%")

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

  Public Function generateUserSummaryTable() As String

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Try

      results_table = getUserSummaryDatesDataTable()

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append("<table border=""1"" cellpadding=""2"" cellspacing=""0"" width=""100%"">")

          ' first add the report title
          htmlOut.Append("<tr><td align=""left"" valign=""middle"" colspan=""" + results_table.Columns.Count.ToString + """><b>Summary for " + insightAreaddl.SelectedItem.ToString.Trim + "</b><em>( since 1/1/" + (Year(Now())).ToString + " )</em></td></tr>")

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

  Public Function getUserSummaryGraphDataTable() As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT DISTINCT MONTH(subislog_date) as [MONTH], DAY(subislog_date) as [DAY], YEAR(subislog_date) as [YEAR],")
      sQuery.Append(" COUNT(distinct subins_contact_id) as TOTAUSERS")
      sQuery.Append(" FROM Subscription_Install_Log WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Subscription_Install WITH(NOLOCK) ON subislog_subid = subins_sub_id AND subislog_login = subins_login AND subislog_seq_no = subins_seq_no")

      sQuery.Append(" WHERE subislog_message LIKE(@subislog_message) AND subislog_date >= '1/1/" + (Year(Now())).ToString + "'")
      sQuery.Append(" AND subislog_subid NOT IN (SELECT DISTINCT sub_id FROM subscription WITH(NOLOCK) WHERE sub_comp_id = 135887)")

      sQuery.Append(" GROUP BY MONTH(subislog_date), DAY(subislog_date),  YEAR(subislog_date)")
      sQuery.Append(" ORDER BY YEAR(subislog_date) asc, MONTH(subislog_date), DAY(subislog_date)")

      SqlCommand.Parameters.AddWithValue("@subislog_message", "%" + insightAreaddl.SelectedValue.ToString.Trim + "%")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>getUserSummaryGraphDataTable() As DataTable</b><br />" + sQuery.ToString

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
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getUserSummaryGraphDataTable load datatable</b><br /> " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>Error in getUserSummaryGraphDataTable() As DataTable</b><br />" + ex.Message

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

  Public Sub generateUserSummaryDataGraph(ByRef out_scriptString As String, ByRef out_htmlString As String, ByVal graphID As Integer)

    Dim htmlOut As New StringBuilder
    Dim scriptOut As New StringBuilder
    Dim results_table As New DataTable

    Dim x As Integer = 0

    Dim sMonth As String = ""

    Dim bFirstTime = True

    Dim sDayArray() As String = Split("1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31", ",")
    Dim sMonthArray() As String = Nothing

    Dim afiltered_Rows As DataRow() = Nothing

    Try

      results_table = getUserSummaryGraphDataTable()

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          scriptOut.Append("google.load('visualization', '1', { packages: ['corechart'] });" + vbCrLf)
          scriptOut.Append("function drawVisualization" + graphID.ToString + "() {" + vbCrLf)
          'scriptOut.Append(" alert('drawVisualization" + graphID.ToString + "');" + vbCrLf)
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

          scriptOut.Append(" data.addColumn('number', 'Clicks');" + vbCrLf)

          scriptOut.Append(" data.addRows([")

          For Each strMO As String In sMonthArray

            For Each strDay As String In sDayArray

              afiltered_Rows = results_table.Select("MONTH = " + strMO.Trim + " AND DAY = " + strDay.Trim, "")

              If afiltered_Rows.Count > 0 Then

                scriptOut.Append(IIf(Not bFirstTime, ", ['" + strMO.Trim + "-" + strDay.Trim + "'", " ['" + strMO.Trim + "-" + strDay.Trim + "'"))

                For Each r As DataRow In afiltered_Rows

                  If Not IsDBNull(r.Item("TOTAUSERS")) Then
                    If Not String.IsNullOrEmpty(r.Item("TOTAUSERS").ToString.Trim) Then

                      If CLng(r.Item("TOTAUSERS").ToString) > 0 Then
                        scriptOut.Append("," + r.Item("TOTAUSERS").ToString)
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

                scriptOut.Append("]")

                bFirstTime = False

              End If ' afiltered_Rows.Count > 0 Then

            Next ' strDay

          Next ' strMO

          scriptOut.Append("]);" + vbCrLf)

          scriptOut.Append("var options = { " + vbCrLf)
          scriptOut.Append("  chartArea:{width:'80%',height:'75%'}," + vbCrLf)
          scriptOut.Append("  hAxis: { title: 'Date'," + vbCrLf)
          scriptOut.Append("           textStyle: { color: '#01579b', fontSize: 14, fontName:  'Arial', bold: true, italic: true }, " + vbCrLf)
          scriptOut.Append("           titleTextStyle: { color: '#01579b', fontSize: 14, fontName:  'Arial', bold: false, italic: true }" + vbCrLf)
          scriptOut.Append("         }," + vbCrLf)
          scriptOut.Append("  vAxis: { title: 'Clicks'," + vbCrLf)
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
        htmlOut.Append("<table id=""userInsightSummaryTable"" width=""100%"" cellspacing=""0"" cellpadding=""4"">")
        htmlOut.Append("<tr><td valign=""top"" align=""left""><div id='visualization" + graphID.ToString + "' style=""height:395px;""></div></td></tr>")
        htmlOut.Append("</table>" + vbCrLf)
      Else
        htmlOut.Append("<table id=""userInsightSummaryTable"" width=""100%"" cellspacing=""0"" cellpadding=""4"">")
        htmlOut.Append("<tr><td valign=""middle"" align=""center"">No User Insight Summary Data at this time ...</td></tr>")
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