
' ********************************************************************************
' Copyright 2004-19. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/deliveries_view_functions.vb $
'$$Author: Matt $
'$$Date: 4/07/20 3:00p $
'$$Modtime: 4/07/20 11:19a $
'$$Revision: 19 $
'$$Workfile: deliveries_view_functions.vb $
'
' ********************************************************************************

<System.Serializable()> Public Class deliveries_view_functions

  Private bIncludeLeases As Boolean
  Private aError As String
  Private clientConnectString As String
  Private adminConnectString As String

  Private starConnectString As String
  Private cloudConnectString As String
  Private serverConnectString As String

  Sub New()

    bIncludeLeases = False

    aError = ""
    clientConnectString = ""
    adminConnectString = ""

    starConnectString = ""
    cloudConnectString = ""
    serverConnectString = ""

  End Sub

  Public Property includeLeases() As Boolean
    Get
      includeLeases = bIncludeLeases
    End Get
    Set(ByVal value As Boolean)
      bIncludeLeases = value
    End Set
  End Property

  Public Property class_error() As String
    Get
      class_error = aError
    End Get
    Set(ByVal value As String)
      aError = value
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

  Public Function get_deliveries_datatable(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT YEAR(journ_date) AS tYear, MONTH(journ_date) AS tMonth, count(*) AS tCount")
      sQuery.Append(" FROM Journal WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft WITH(NOLOCK) ON journ_ac_id = ac_id AND journ_id = ac_journ_id")
      sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id")
      sQuery.Append(" WHERE " + IIf(bIncludeLeases, "(journ_subcategory_code LIKE('WS%') OR journ_subcategory_code LIKE('L%'))", "journ_subcategory_code LIKE('WS%')") + " AND right(journ_subcategory_code,4) <> 'CORR' AND right(journ_subcategory_code,2) <> 'IT'")
      sQuery.Append(" AND journ_newac_flag = 'Y' AND journ_internal_trans_flag = 'N'")

      sQuery.Append(Constants.cAndClause + "(YEAR(journ_date) >= YEAR(CONVERT(DATETIME,'" + DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now()).ToString + "',102))")
      sQuery.Append(Constants.cAndClause + " YEAR(journ_date) <= YEAR(getdate()))")

      If Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
        Dim tmpStr As String = ""

        ' flatten out amodID array ...
        For x As Integer = 0 To UBound(searchCriteria.ViewCriteriaAmodIDArray)
          If String.IsNullOrEmpty(tmpStr) Then
            tmpStr = searchCriteria.ViewCriteriaAmodIDArray(x)
          Else
            tmpStr += Constants.cCommaDelim + searchCriteria.ViewCriteriaAmodIDArray(x)
          End If
        Next

        sQuery.Append(Constants.cAndClause + "amod_id IN (" + tmpStr.Trim + ")")
      ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      ElseIf searchCriteria.ViewCriteriaSecondAmodID > -1 Then
        sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaSecondAmodID.ToString)
      ElseIf searchCriteria.ViewCriteriaThirdAmodID > -1 Then
        sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaThirdAmodID.ToString)
      ElseIf Not IsNothing(searchCriteria.ViewCriteriaMakeIDArray) Then
        sQuery.Append(Constants.cAndClause + "amod_make_name IN ('" + searchCriteria.ViewCriteriaAircraftMake.ToUpper.Replace(Constants.cCommaDelim, Constants.cValueSeperator).Trim + "')")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftType.Trim) Then

        If searchCriteria.ViewCriteriaAirframeType <> Constants.VIEW_HELICOPTERS Then
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code = 'F'")
        Else
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code = 'R'")
        End If

        sQuery.Append(Constants.cAndClause + "amod_type_code = '" + searchCriteria.ViewCriteriaAircraftType.Trim + "'")

      End If

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
      Else
        sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, False))
      End If

      sQuery.Append(" GROUP BY YEAR(journ_date), MONTH(journ_date)")
      sQuery.Append(" ORDER BY YEAR(journ_date), MONTH(journ_date)")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + sQuery.ToString

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
        aError = "Error in " + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + " load datatable " + constrExc.Message
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in " + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + " " + ex.Message
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

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

  Public Sub views_display_deliveries_line_chart(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_scriptString As String, ByRef out_htmlString As String, ByVal graphID As Integer)
    Dim htmlOut As New StringBuilder
    Dim scriptOut As New StringBuilder
    Dim results_table As New DataTable

    Dim sYear As String = ""

    Dim sMonthArray() As String = Split("1,2,3,4,5,6,7,8,9,10,11,12", ",")
    Dim sYearArray() As String = Nothing

    Dim afiltered_Rows As DataRow() = Nothing

        Dim sRunningTotal(,) As Integer = Nothing
        Dim x As Integer = 0
        Dim y As Integer = 0

        Try

      results_table = get_deliveries_datatable(searchCriteria)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    If graphID = 40 Then
                        ' scriptOut.Append("   {" + vbCrLf)
                        'scriptOut.Append(" alert('drawVisualization" + graphID.ToString + "');" + vbCrLf)  
                        scriptOut.Append(" var data" & graphID & " = new google.visualization.DataTable();" + vbCrLf)
                        scriptOut.Append(" data" & graphID & ".addColumn('string', 'Month');" + vbCrLf)
                    Else
                        scriptOut.Append("function drawVisualization" + graphID.ToString + "() ")
                        scriptOut.Append("   {" + vbCrLf)
                        'scriptOut.Append(" alert('drawVisualization" + graphID.ToString + "');" + vbCrLf)  
                        scriptOut.Append(" var data = new google.visualization.DataTable();" + vbCrLf)
                        scriptOut.Append(" data.addColumn('string', 'Month');" + vbCrLf)
                    End If




                    For Each r As DataRow In results_table.Rows

                        If Not commonEvo.inMyArray(sYear.Split(","), r.Item("tyear").ToString) Then

                            If String.IsNullOrEmpty(sYear.Trim) Then
                                sYear = r.Item("tyear").ToString
                            Else
                                sYear += "," + r.Item("tyear").ToString
                            End If


                            If graphID = 40 Then
                                scriptOut.Append(" data" & graphID & ".addColumn('number', '" + r.Item("tyear").ToString.Trim + "');" + vbCrLf)
                            Else
                                scriptOut.Append(" data.addColumn('number', '" + r.Item("tyear").ToString.Trim + "');" + vbCrLf)
                            End If
                        End If

                    Next

                    sYearArray = sYear.Split(",")

                    If graphID = 40 Then
                        '  scriptOut.Append(" data" & graphID & ".addRows([")

                        scriptOut.Append(" data" & graphID & ".addRows(12);" + vbCrLf)

                    Else
                        scriptOut.Append(" data.addRows([")
                    End If


                    Dim nYear As Integer = 0
                    ReDim sRunningTotal(sYearArray.GetUpperBound(0), 1)
                    sRunningTotal(nYear, 1) = 0

                    For Each strMO As String In sMonthArray

                        nYear = 0
                        y = 0

                        If graphID = 40 Then
                            ' scriptOut.Append(IIf(CInt(strMO.Trim) > 1, ", ['" + strMO.Trim + "'", " ['" + strMO.Trim + "'"))

                            '  scriptOut.Append(" data" & graphID & ".setCell(" + x.ToString + ", 0, '" & strMO.ToString)

                            ' htmlOut.Append(" data" & graphID & ".setCell(" + x.ToString + ", 1, " + IIf(CLng(r.Item("tcount").ToString) > 0, FormatNumber(r.Item("tcount").ToString, 0, TriState.False, TriState.False, TriState.False).ToString, "0") + ");" + vbCrLf)
                            '  If y = 0 Then
                            scriptOut.Append(" data" & graphID & ".setCell(" & x & ", " & y & ", '" & strMO.ToString)
                            'scriptOut.Append(" - " & strYR.ToString + "');" + vbCrLf)
                            scriptOut.Append("');" + vbCrLf)
                            '    End If
                        Else
                            scriptOut.Append(IIf(CInt(strMO.Trim) > 1, ", ['" + strMO.Trim + "'", " ['" + strMO.Trim + "'"))
                        End If

                        y = 1

                        For Each strYR As String In sYearArray
                            afiltered_Rows = results_table.Select("tmonth = " + strMO.Trim + " AND tyear = " + strYR.Trim, "")


                            If afiltered_Rows.Count > 0 Then
                                For Each r As DataRow In afiltered_Rows

                                    If graphID = 40 Then
                                        '-------------------------------------------------------------------------------- 

                                        If Not IsDBNull(r.Item("tcount")) Then
                                            If Not String.IsNullOrEmpty(r.Item("tcount").ToString.Trim) Then

                                                If CLng(r.Item("tcount").ToString) > 0 Then
                                                    sRunningTotal(nYear, 1) += CInt(r.Item("tcount").ToString)
                                                    '  scriptOut.Append("," + sRunningTotal(nYear, 1).ToString)

                                                    scriptOut.Append(" data" & graphID & ".setCell(" + x.ToString + ", " & y & ", " + IIf(CLng(sRunningTotal(nYear, 1).ToString) > 0, FormatNumber(sRunningTotal(nYear, 1).ToString, 0, TriState.False, TriState.False, TriState.False).ToString, "0") + ");" + vbCrLf)
                                                Else
                                                    scriptOut.Append("," + sRunningTotal(nYear, 1).ToString)
                                                    htmlOut.Append(" data" & graphID & ".setCell(" + x.ToString + ", " & y & ", " + IIf(CLng(sRunningTotal(nYear, 1).ToString) > 0, FormatNumber(sRunningTotal(nYear, 1).ToString, 0, TriState.False, TriState.False, TriState.False).ToString, "0") + ");" + vbCrLf)
                                                End If

                                                Exit For

                                            Else
                                                '' scriptOut.Append("," + sRunningTotal(nYear, 1).ToString)
                                                scriptOut.Append(" data" & graphID & ".setCell(" + x.ToString + ", 1, " + IIf(CLng(sRunningTotal(nYear, 1).ToString) > 0, FormatNumber(sRunningTotal(nYear, 1).ToString, 0, TriState.False, TriState.False, TriState.False).ToString, "0") + ");" + vbCrLf)
                                                Exit For
                                            End If

                                        Else
                                            'scriptOut.Append("," + sRunningTotal(nYear, 1).ToString)
                                            scriptOut.Append(" data" & graphID & ".setCell(" + x.ToString + ", 1, " + IIf(CLng(sRunningTotal(nYear, 1).ToString) > 0, FormatNumber(sRunningTotal(nYear, 1).ToString, 0, TriState.False, TriState.False, TriState.False).ToString, "0") + ");" + vbCrLf)
                                            Exit For
                                        End If
                                        '--------------------------------------------------------------------------------
                                    Else
                                        If Not IsDBNull(r.Item("tcount")) Then
                                            If Not String.IsNullOrEmpty(r.Item("tcount").ToString.Trim) Then

                                                If CLng(r.Item("tcount").ToString) > 0 Then
                                                    sRunningTotal(nYear, 1) += CInt(r.Item("tcount").ToString)
                                                    scriptOut.Append("," + sRunningTotal(nYear, 1).ToString)
                                                Else
                                                    scriptOut.Append("," + sRunningTotal(nYear, 1).ToString)
                                                End If

                                                Exit For

                                            Else
                                                scriptOut.Append("," + sRunningTotal(nYear, 1).ToString)
                                                Exit For
                                            End If

                                        Else
                                            scriptOut.Append("," + sRunningTotal(nYear, 1).ToString)
                                            Exit For
                                        End If
                                    End If

                                Next

                            Else
                                If graphID = 40 Then
                                    scriptOut.Append(" data" & graphID & ".setCell(" + x.ToString + ", " & y & ",  " & sRunningTotal(nYear, 1) & ");" + vbCrLf)

                                    ' scriptOut.Append("," + sRunningTotal(nYear, 1).ToString)
                                Else
                                    scriptOut.Append("," + sRunningTotal(nYear, 1).ToString)
                                End If
                            End If


                            y += 1
                            nYear += 1

                        Next

                        If graphID = 40 Then
                        Else
                            scriptOut.Append("]")
                        End If

                        x += 1
                    Next

                    If graphID = 40 Then
                    Else
                        scriptOut.Append("]);" + vbCrLf)
                    End If


                    If graphID = 40 Then
                        ' scriptOut.Append("}" + vbCrLf)
                    Else
                        scriptOut.Append("var options = { " + vbCrLf)
                        scriptOut.Append("  chartArea:{width:'70%',height:'80%'}," + vbCrLf)
                        scriptOut.Append("  hAxis: { title: 'Month'," + vbCrLf)
                        scriptOut.Append("           textStyle: { color: '#01579b', fontSize: 14, fontName:  'Arial', bold: true, italic: true }, " + vbCrLf)
                        scriptOut.Append("           titleTextStyle: { color: '#01579b', fontSize: 14, fontName:  'Arial', bold: false, italic: true }" + vbCrLf)
                        scriptOut.Append("         }," + vbCrLf)
                        scriptOut.Append("  vAxis: { title: 'Deliveries'," + vbCrLf)
                        scriptOut.Append("           textStyle: { color: '#1a237e', fontSize: 14, bold: true }," + vbCrLf)
                        scriptOut.Append("           titleTextStyle: { color: '#1a237e', fontSize: 16, bold: true }" + vbCrLf)
                        scriptOut.Append("        }," + vbCrLf)
                        scriptOut.Append("  title:'Cumulative Deliveries'," + vbCrLf)
                        scriptOut.Append("  smoothLine:false," + vbCrLf)
                        scriptOut.Append("  legendFontSize:12," + vbCrLf)
                        scriptOut.Append("  tooltipFontSize:9," + vbCrLf)
                        scriptOut.Append("  legend:'top'," + vbCrLf)
                        scriptOut.Append("  colors: ['black','red', 'blue', 'green', 'orange', 'yellow']" + vbCrLf)
                        scriptOut.Append("};" + vbCrLf)

                        scriptOut.Append(" var chart = new google.visualization.LineChart(document.getElementById('visualization" + graphID.ToString + "'));" + vbCrLf)
                        scriptOut.Append(" chart.draw(data, options);" + vbCrLf)
                        scriptOut.Append("}" + vbCrLf)
                    End If
                End If

            End If

            If Not String.IsNullOrEmpty(scriptOut.ToString.Trim) Then
                htmlOut.Append("<table id=""deliveries_line_chart"" width=""100%"" height=""400"" cellspacing=""0"" cellpadding=""2"">")
                htmlOut.Append("<tr><td valign=""top"" align=""left""><div id='visualization" + graphID.ToString + "' style=""text-align:center; width:100%; height:400px;""></div></td></tr>")
                htmlOut.Append("</table>" + vbCrLf)
            Else
                htmlOut.Append("<table id=""deliveries_line_chart"" width=""100%"" height=""400"" cellspacing=""0"" cellpadding=""2"">")
                htmlOut.Append("<tr><td valign=""top"" align=""left""><div style=""text-align:center; width:100%; height:400px;"">No Deliveries at this time, for this Make/Model ...</div></td></tr>")
                htmlOut.Append("</table>" + vbCrLf)
            End If

        Catch ex As Exception

      aError = "Error in " + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + " " + ex.Message
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    Finally

    End Try

    'return resulting html string
    out_scriptString = scriptOut.ToString
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Sub views_display_deliveries_bar_chart(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_scriptString As String, ByRef out_htmlString As String, ByVal graphID As Integer)

    Dim htmlOut As New StringBuilder
    Dim scriptOut As New StringBuilder
    Dim results_table As New DataTable

    Dim sYear As String = ""

    Dim sMonthArray() As String = Split("1,2,3,4,5,6,7,8,9,10,11,12", ",")
    Dim sYearArray() As String = Nothing

    Dim afiltered_Rows As DataRow() = Nothing

    Try

      results_table = get_deliveries_datatable(searchCriteria)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          scriptOut.Append("function drawVisualization" + graphID.ToString + "() {" + vbCrLf)
          'scriptOut.Append(" alert('drawVisualization" + graphID.ToString + "');" + vbCrLf)  
          scriptOut.Append(" var data = new google.visualization.DataTable();" + vbCrLf)

          scriptOut.Append(" data.addColumn('string', 'Month');" + vbCrLf)

          For Each r As DataRow In results_table.Rows

            If Not commonEvo.inMyArray(sYear.Split(","), r.Item("tyear").ToString) Then

              If String.IsNullOrEmpty(sYear.Trim) Then
                sYear = r.Item("tyear").ToString
              Else
                sYear += "," + r.Item("tyear").ToString
              End If

              scriptOut.Append(" data.addColumn('number', '" + r.Item("tyear").ToString.Trim + "');" + vbCrLf)

            End If

          Next

          sYearArray = sYear.Split(",")

          scriptOut.Append(" data.addRows([")

          For Each strMO As String In sMonthArray

            scriptOut.Append(IIf(CInt(strMO.Trim) > 1, ", ['" + strMO.Trim + "'", " ['" + strMO.Trim + "'"))

            For Each strYR As String In sYearArray

              afiltered_Rows = results_table.Select("tmonth = " + strMO.Trim + " AND tyear = " + strYR.Trim, "")

              If afiltered_Rows.Count > 0 Then

                For Each r As DataRow In afiltered_Rows

                  If Not IsDBNull(r.Item("tcount")) Then
                    If Not String.IsNullOrEmpty(r.Item("tcount").ToString.Trim) Then

                      If CLng(r.Item("tcount").ToString) > 0 Then
                        scriptOut.Append("," + r.Item("tcount").ToString)
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
          scriptOut.Append("  chartArea:{width:'70%',height:'80%'}," + vbCrLf)
          scriptOut.Append("  hAxis: { title: 'Month'," + vbCrLf)
          scriptOut.Append("           textStyle: { color: '#01579b', fontSize: 14, fontName:  'Arial', bold: true, italic: true }, " + vbCrLf)
          scriptOut.Append("           titleTextStyle: { color: '#01579b', fontSize: 14, fontName:  'Arial', bold: false, italic: true }" + vbCrLf)
          scriptOut.Append("         }," + vbCrLf)
          scriptOut.Append("  vAxis: { title: 'Deliveries'," + vbCrLf)
          scriptOut.Append("           textStyle: { color: '#1a237e', fontSize: 14, bold: true }," + vbCrLf)
          scriptOut.Append("           titleTextStyle: { color: '#1a237e', fontSize: 16, bold: true }" + vbCrLf)
          scriptOut.Append("        }," + vbCrLf)
          scriptOut.Append("  title:'Monthly Deliveries'," + vbCrLf)
          scriptOut.Append("  legendFontSize:12," + vbCrLf)
          scriptOut.Append("  tooltipFontSize:9," + vbCrLf)
          scriptOut.Append("  legend:'top'," + vbCrLf)
          scriptOut.Append("  colors: ['black','red', 'blue', 'green', 'orange', 'yellow']" + vbCrLf)
          scriptOut.Append("};" + vbCrLf)


          scriptOut.Append(" var chart = new google.visualization.ColumnChart(document.getElementById('visualization" + graphID.ToString + "'));" + vbCrLf)
          scriptOut.Append(" chart.draw(data, options);" + vbCrLf)
          scriptOut.Append("}" + vbCrLf)

        End If

      End If

      If Not String.IsNullOrEmpty(scriptOut.ToString.Trim) Then
        htmlOut.Append("<table id=""deliveries_bar_chart"" width=""100%"" height=""400"" cellspacing=""0"" cellpadding=""2"">")
        htmlOut.Append("<tr><td valign=""top"" align=""left""><div id='visualization" + graphID.ToString + "' style=""text-align:center; width:100%; height:400px;""></div></td></tr>")
        htmlOut.Append("</table>" + vbCrLf)
      Else
        htmlOut.Append("<table id=""deliveries_bar_chart"" width=""100%"" height=""400"" cellspacing=""0"" cellpadding=""2"">")
        htmlOut.Append("<tr><td valign=""top"" align=""left""><div style=""text-align:center; width:100%; height:400px;"">No Deliveries at this time, for this Make/Model ...</div></td></tr>")
        htmlOut.Append("</table>" + vbCrLf)
      End If

    Catch ex As Exception

      aError = "Error in " + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + " " + ex.Message
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    Finally

    End Try

    'return resulting html string
    out_scriptString = scriptOut.ToString
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_top_model_deliveries_datatable(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT amod_make_name, amod_model_name, amod_id, count(*) AS tCount")
      sQuery.Append(" FROM Journal WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft WITH(NOLOCK) ON journ_ac_id = ac_id AND journ_id = ac_journ_id")
      sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id")
      sQuery.Append(" WHERE " + IIf(bIncludeLeases, "(journ_subcategory_code LIKE('WS%') OR journ_subcategory_code LIKE('L%'))", "journ_subcategory_code LIKE('WS%')") + " AND right(journ_subcategory_code,4) <> 'CORR' AND right(journ_subcategory_code,2) <> 'IT'")
      sQuery.Append(" AND journ_newac_flag = 'Y' AND journ_internal_trans_flag = 'N'")

      sQuery.Append(Constants.cAndClause + "(YEAR(journ_date) >= YEAR(CONVERT(DATETIME,'" + DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now()).ToString + "',102))")
      sQuery.Append(Constants.cAndClause + " YEAR(journ_date) <= YEAR(getdate()))")

      If Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
        Dim tmpStr As String = ""

        ' flatten out amodID array ...
        For x As Integer = 0 To UBound(searchCriteria.ViewCriteriaAmodIDArray)
          If String.IsNullOrEmpty(tmpStr) Then
            tmpStr = searchCriteria.ViewCriteriaAmodIDArray(x)
          Else
            tmpStr += Constants.cCommaDelim + searchCriteria.ViewCriteriaAmodIDArray(x)
          End If
        Next

        sQuery.Append(Constants.cAndClause + "amod_id IN (" + tmpStr.Trim + ")")
      ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      ElseIf searchCriteria.ViewCriteriaSecondAmodID > -1 Then
        sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaSecondAmodID.ToString)
      ElseIf searchCriteria.ViewCriteriaThirdAmodID > -1 Then
        sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaThirdAmodID.ToString)
      ElseIf Not IsNothing(searchCriteria.ViewCriteriaMakeIDArray) Then
        sQuery.Append(Constants.cAndClause + "amod_make_name IN ('" + searchCriteria.ViewCriteriaAircraftMake.ToUpper.Replace(Constants.cCommaDelim, Constants.cValueSeperator).Trim + "')")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftType.Trim) Then

        If searchCriteria.ViewCriteriaAirframeType <> Constants.VIEW_HELICOPTERS Then
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code = 'F'")
        Else
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code = 'R'")
        End If

        sQuery.Append(Constants.cAndClause + "amod_type_code = '" + searchCriteria.ViewCriteriaAircraftType.Trim + "'")

      End If


      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
      Else
        sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, False))
      End If

      sQuery.Append(" GROUP BY amod_make_name, amod_model_name, amod_id")
      sQuery.Append(" ORDER BY count(*) DESC")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + sQuery.ToString

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
        aError = "Error in " + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + " load datatable " + constrExc.Message
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in " + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + " " + ex.Message
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

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

  Public Sub views_display_top_deliveries_models(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal bHasMaster As Boolean)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Dim toggleRowColor As Boolean = False

    Dim sTmpTitle As String = ""
    Dim sTitle As String = ""
    Dim sRefLink As String = ""
    Dim sRefTitle As String = ""

    Try

      results_table = get_top_model_deliveries_datatable(searchCriteria)

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        If String.IsNullOrEmpty(sTmpTitle.Trim) Then
          sTmpTitle = commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, "") + " : "
        End If
      ElseIf searchCriteria.ViewCriteriaMakeAmodID > -1 Then
        If String.IsNullOrEmpty(sTmpTitle.Trim) Then
          sTmpTitle = commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaMakeAmodID, True, "") + " : "
        Else
          sTmpTitle += " - " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaMakeAmodID, True, "") + " : "
        End If
      End If

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          sTitle = sTmpTitle + results_table.Rows.Count.ToString + " MODELS DELIVERED"

          If results_table.Rows.Count > 15 Then
            htmlOut.Append("<div valign=""top"" style=""height:370px; overflow: auto;"">")
          End If

          htmlOut.Append("<table id=""deliveriesModelsInnerTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")
          htmlOut.Append("<tr><td align=""center"" valign=""middle"" class=""header"" colspan=""2"">" + sTitle + "</td></tr>")

          For Each r As DataRow In results_table.Rows

            If Not toggleRowColor Then
              htmlOut.Append("<tr class='alt_row'>")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor='white'>")
              toggleRowColor = False
            End If

            htmlOut.Append("<td valign=""middle"" align=""left"">")

            sRefLink = "view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName.Trim
            sRefLink += IIf(bHasMaster = False, "&noMaster=false", "")
            sRefLink += "&amod_id=" + r.Item("amod_id").ToString

            sRefTitle = IIf(HttpContext.Current.Application.Item("DebugFlag").ToString, " title=""" + sRefLink.Trim + """", " title=""Click to view model""")

            htmlOut.Append("<a href=""" + sRefLink + """" + sRefTitle + ">" + r.Item("amod_make_name").ToString + "&nbsp;/&nbsp;" + r.Item("amod_model_name").ToString.Trim + "</a>")

            htmlOut.Append("</td><td valign=""middle"" align=""left"">")
            htmlOut.Append(r.Item("tCount").ToString)
            htmlOut.Append("</td></tr>")

          Next

          htmlOut.Append("</table>")

          If results_table.Rows.Count > 15 Then
            htmlOut.Append("</div>")
          End If

        Else
          htmlOut.Append("<table id='deliveriesModelsInnerTable' width='100%' cellpadding='2' cellspacing='0'><tr><td valign=""top"" align=""left""><br/>No Data Available</td></tr></table>")
        End If
      Else
        htmlOut.Append("<table id='deliveriesModelsInnerTable' width='100%' cellpadding='2' cellspacing='0'><tr><td valign=""top"" align=""left""><br/>No Data Available</td></tr></table>")
      End If

    Catch ex As Exception

      aError = "Error in " + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + " " + ex.Message
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    Finally

    End Try

    out_htmlString = htmlOut.ToString()
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_latest_deliveries(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT ac_id, ac_ser_no_full, ac_reg_no, amod_make_name, amod_model_name, amod_id, journ_subject, journ_date")
      sQuery.Append(" FROM Journal WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft WITH(NOLOCK) ON journ_ac_id = ac_id AND journ_id = ac_journ_id")
      sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id")
      sQuery.Append(" WHERE " + IIf(bIncludeLeases, "(journ_subcategory_code LIKE('WS%') OR journ_subcategory_code LIKE('L%'))", "journ_subcategory_code LIKE('WS%')") + " AND right(journ_subcategory_code,4) <> 'CORR' AND right(journ_subcategory_code,2) <> 'IT'")
      sQuery.Append(" AND journ_newac_flag = 'Y' AND journ_internal_trans_flag = 'N'")

      If searchCriteria.ViewCriteriaAmodID = -1 Then
        sQuery.Append(Constants.cAndClause + "(journ_date >= CONVERT(DATETIME, '" + DateAdd("d", (-1) * 60, Now()).ToString + "',102)")
        sQuery.Append(Constants.cAndClause + " journ_date <= CONVERT(DATETIME, '" + Now.ToString + "',102))")
      Else
        sQuery.Append(Constants.cAndClause + "(YEAR(journ_date) >= YEAR(CONVERT(DATETIME,'" + DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now()).ToString + "',102))")
        sQuery.Append(Constants.cAndClause + " YEAR(journ_date) <= YEAR(getdate()))")
      End If

      If Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
        Dim tmpStr As String = ""

        ' flatten out amodID array ...
        For x As Integer = 0 To UBound(searchCriteria.ViewCriteriaAmodIDArray)
          If String.IsNullOrEmpty(tmpStr) Then
            tmpStr = searchCriteria.ViewCriteriaAmodIDArray(x)
          Else
            tmpStr += Constants.cCommaDelim + searchCriteria.ViewCriteriaAmodIDArray(x)
          End If
        Next

        sQuery.Append(Constants.cAndClause + "amod_id IN (" + tmpStr.Trim + ")")
      ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      ElseIf searchCriteria.ViewCriteriaSecondAmodID > -1 Then
        sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaSecondAmodID.ToString)
      ElseIf searchCriteria.ViewCriteriaThirdAmodID > -1 Then
        sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaThirdAmodID.ToString)
      ElseIf Not IsNothing(searchCriteria.ViewCriteriaMakeIDArray) Then
        sQuery.Append(Constants.cAndClause + "amod_make_name IN ('" + searchCriteria.ViewCriteriaAircraftMake.ToUpper.Replace(Constants.cCommaDelim, Constants.cValueSeperator).Trim + "')")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftType.Trim) Then

        If searchCriteria.ViewCriteriaAirframeType <> Constants.VIEW_HELICOPTERS Then
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code = 'F'")
        Else
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code = 'R'")
        End If

        sQuery.Append(Constants.cAndClause + "amod_type_code = '" + searchCriteria.ViewCriteriaAircraftType.Trim + "'")

      End If

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
      Else
        sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, False))
      End If

      sQuery.Append(" ORDER BY journ_date DESC, amod_make_name, amod_model_name")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + sQuery.ToString

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
        aError = "Error in " + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + " load datatable " + constrExc.Message
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in " + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + " " + ex.Message
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

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

  Public Sub views_display_latest_deliveries(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Dim toggleRowColor As Boolean = False

    Dim sTmpTitle As String = ""
    Dim sTitle As String = ""

    Try

      results_table = get_latest_deliveries(searchCriteria)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          If results_table.Rows.Count > 15 Then
            htmlOut.Append("<div valign=""top"" style=""height:370px; overflow: auto;"">")
          End If

          htmlOut.Append("<table id=""latestDeliveriesInnerTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")
          If searchCriteria.ViewCriteriaAmodID = -1 Then
            htmlOut.Append("<tr><td align=""center"" valign=""middle"" class=""header"" colspan=""2"">LATEST DELIVERIES (LAST 60 DAYS)</td></tr>")
          Else
            htmlOut.Append("<tr><td align=""center"" valign=""middle"" class=""header"" colspan=""2"">LATEST DELIVERIES</td></tr>")
          End If

          For Each r As DataRow In results_table.Rows

            If Not toggleRowColor Then
              htmlOut.Append("<tr class='alt_row'>")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor='white'>")
              toggleRowColor = False
            End If

            htmlOut.Append("<td valign=""top"" align=""left"" width=""10%"">" + FormatDateTime(r.Item("journ_date").ToString, DateFormat.ShortDate) + "</td>")
            htmlOut.Append("<td valign=""top"" align=""left"">")

            If searchCriteria.ViewCriteriaAmodID = -1 Then
              htmlOut.Append("<em>" + r.Item("amod_make_name").ToString + " " + r.Item("amod_model_name").ToString + "</em> ")
            End If

            If Not IsDBNull(r.Item("ac_ser_no_full")) And Not String.IsNullOrEmpty(r.Item("ac_ser_no_full").ToString) Then
              htmlOut.Append("SN: <a class='underline' onclick=""JavaScript:load('DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "&jid=0','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title=""Display Aircraft Details"">")
              htmlOut.Append(r.Item("ac_ser_no_full").ToString + "</a>" + IIf(searchCriteria.ViewCriteriaAmodID = -1, "<br />", " "))
            End If

            If Not IsDBNull(r.Item("journ_subject")) And Not String.IsNullOrEmpty(r.Item("journ_subject").ToString) Then
              htmlOut.Append(r.Item("journ_subject").ToString)
            End If

            htmlOut.Append("</td></tr>")

          Next

          htmlOut.Append("</table>")

          If results_table.Rows.Count > 15 Then
            htmlOut.Append("</div>")
          End If

        Else
          htmlOut.Append("<table id='latestDeliveriesInnerTable' width='100%' cellpadding='2' cellspacing='0'><tr><td valign=""middle"" align=""left""><br/>No Data Available</td></tr></table>")
        End If
      Else
        htmlOut.Append("<table id='latestDeliveriesInnerTable' width='100%' cellpadding='2' cellspacing='0'><tr><td valign=""middle"" align=""left""><br/>No Data Available</td></tr></table>")
      End If

    Catch ex As Exception

      aError = "Error in " + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + " " + ex.Message
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    Finally

    End Try

    out_htmlString = htmlOut.ToString()
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_deliveries_by_type_datatable(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT amod_airframe_type_code, afmt_code, afmt_description, count(*) AS tCount")
      sQuery.Append(" FROM Journal WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft WITH(NOLOCK) ON journ_ac_id = ac_id AND journ_id = ac_journ_id")
      sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id")
      sQuery.Append(" INNER JOIN Airframe_Make_Type WITH(NOLOCK) ON afmt_airframetype = amod_airframe_type_code and afmt_airframemaketype = amod_type_code")
      sQuery.Append(" WHERE " + IIf(bIncludeLeases, "(journ_subcategory_code LIKE('WS%') OR journ_subcategory_code LIKE('L%'))", "journ_subcategory_code LIKE('WS%')") + " AND right(journ_subcategory_code,4) <> 'CORR' AND right(journ_subcategory_code,2) <> 'IT'")
      sQuery.Append(" AND journ_newac_flag = 'Y' AND journ_internal_trans_flag = 'N'")

      sQuery.Append(Constants.cAndClause + "(YEAR(journ_date) >= YEAR(CONVERT(DATETIME,'" + DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now()).ToString + "',102))")
      sQuery.Append(Constants.cAndClause + " YEAR(journ_date) <= YEAR(getdate()))")

      sQuery.Append(Constants.cAndClause + "afmt_code IN ('E','T','J','P')")

      If Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
        Dim tmpStr As String = ""

        ' flatten out amodID array ...
        For x As Integer = 0 To UBound(searchCriteria.ViewCriteriaAmodIDArray)
          If String.IsNullOrEmpty(tmpStr) Then
            tmpStr = searchCriteria.ViewCriteriaAmodIDArray(x)
          Else
            tmpStr += Constants.cCommaDelim + searchCriteria.ViewCriteriaAmodIDArray(x)
          End If
        Next

        sQuery.Append(Constants.cAndClause + "amod_id IN (" + tmpStr.Trim + ")")
      ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      ElseIf searchCriteria.ViewCriteriaSecondAmodID > -1 Then
        sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaSecondAmodID.ToString)
      ElseIf searchCriteria.ViewCriteriaThirdAmodID > -1 Then
        sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaThirdAmodID.ToString)
      ElseIf Not IsNothing(searchCriteria.ViewCriteriaMakeIDArray) Then
        sQuery.Append(Constants.cAndClause + "amod_make_name IN ('" + searchCriteria.ViewCriteriaAircraftMake.ToUpper.Replace(Constants.cCommaDelim, Constants.cValueSeperator).Trim + "')")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftType.Trim) Then

        If searchCriteria.ViewCriteriaAirframeType <> Constants.VIEW_HELICOPTERS Then
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code = 'F'")
        Else
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code = 'R'")
        End If

        sQuery.Append(Constants.cAndClause + "amod_type_code = '" + searchCriteria.ViewCriteriaAircraftType.Trim + "'")

      End If

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
      Else
        sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, False))
      End If

      sQuery.Append(" GROUP BY amod_airframe_type_code, afmt_code, afmt_description")
      sQuery.Append(" ORDER BY amod_airframe_type_code, count(*) DESC")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + sQuery.ToString

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
        aError = "Error in " + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + " load datatable " + constrExc.Message
        HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in " + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + " " + ex.Message
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

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

  Public Sub views_display_deliveries_by_type(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Dim toggleRowColor As Boolean = False

    Dim sTmpTitle As String = ""
    Dim sTitle As String = ""

    Try

      results_table = get_deliveries_by_type_datatable(searchCriteria)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          If results_table.Rows.Count > 15 Then
            htmlOut.Append("<div valign=""top"" style=""height:370px; overflow: auto;"">")
          End If

          htmlOut.Append("<table id=""deliveriesByTypeInnerTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")
          htmlOut.Append("<tr><td align=""center"" valign=""middle"" class=""header"" colspan=""2"">DELIVERIES BY TYPE</td></tr>")

          For Each r As DataRow In results_table.Rows

            If Not toggleRowColor Then
              htmlOut.Append("<tr class='alt_row'>")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor='white'>")
              toggleRowColor = False
            End If

            htmlOut.Append("<td valign=""middle"" align=""left"">")

            If r.Item("amod_airframe_type_code").ToString.ToUpper.Contains("F") Then
              htmlOut.Append("Fixed Wing&nbsp;/&nbsp;")
            Else
              htmlOut.Append("Rotary&nbsp;/&nbsp;")
            End If

            htmlOut.Append(r.Item("afmt_description").ToString)

            htmlOut.Append("</td><td valign=""middle"" align=""left"">")
            htmlOut.Append(r.Item("tCount").ToString)
            htmlOut.Append("</td></tr>")

          Next

          htmlOut.Append("</table>")

          If results_table.Rows.Count > 15 Then
            htmlOut.Append("</div>")
          End If

        Else
          htmlOut.Append("<table id='deliveriesByTypeInnerTable' width='100%' cellpadding='2' cellspacing='0'><tr><td valign=""middle"" align=""left""><br/>No Data Available</td></tr></table>")
        End If
      Else
        htmlOut.Append("<table id='deliveriesByTypeInnerTable' width='100%' cellpadding='2' cellspacing='0'><tr><td valign=""middle"" align=""left""><br/>No Data Available</td></tr></table>")
      End If

    Catch ex As Exception

      aError = "Error in " + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + " " + ex.Message
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    Finally

    End Try

    out_htmlString = htmlOut.ToString()
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_deliveries_operational_trends(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim temptable As New DataTable
    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sQuery = New StringBuilder()

    Try


      sQuery.Append("SELECT amod_make_name, amod_model_name, amod_id,")
      sQuery.Append(" YEAR(getdate())-4 as YEAR1YEAR,")
      sQuery.Append(" (SELECT TOP 1 mtrend_lifecycle_3_count FROM Aircraft_Model_Trend WITH(NOLOCK) WHERE mtrend_year=YEAR(getdate())-4 AND mtrend_month = 1 AND mtrend_amod_id = amod_id) AS YEAR1VAL,")
      sQuery.Append(" YEAR(getdate())-3 as YEAR2YEAR,")
      sQuery.Append(" (SELECT TOP 1 mtrend_lifecycle_3_count FROM Aircraft_Model_Trend WITH(NOLOCK) WHERE mtrend_year=YEAR(getdate())-3 AND mtrend_month = 1 AND mtrend_amod_id = amod_id) AS YEAR2VAL,")
      sQuery.Append(" YEAR(getdate())-2 as YEAR3YEAR,")
      sQuery.Append(" (SELECT TOP 1 mtrend_lifecycle_3_count FROM Aircraft_Model_Trend WITH(NOLOCK) WHERE mtrend_year=YEAR(getdate())-2 AND mtrend_month = 1 AND mtrend_amod_id = amod_id) AS YEAR3VAL,")
      sQuery.Append(" YEAR(getdate())-1 as YEAR4YEAR,")
      sQuery.Append(" (SELECT TOP 1 mtrend_lifecycle_3_count FROM Aircraft_Model_Trend WITH(NOLOCK) WHERE mtrend_year=YEAR(getdate())-1 AND mtrend_month = 1 AND mtrend_amod_id = amod_id) AS YEAR4VAL,")
      sQuery.Append(" YEAR(getdate()) as YEAR5YEAR,")
      sQuery.Append(" (case when (SELECT TOP 1 mtrend_lifecycle_3_count FROM Aircraft_Model_Trend WITH(NOLOCK) WHERE mtrend_year=YEAR(getdate()) AND mtrend_month = 1 AND mtrend_amod_id = amod_id) IS NULL")
      sQuery.Append(" then (select COUNT(*) from Aircraft with (NOLOCK) where ac_journ_id = 0 and ac_lifecycle_stage=3 and ac_amod_id = amod_id) else")
      sQuery.Append(" (SELECT TOP 1 mtrend_lifecycle_3_count FROM Aircraft_Model_Trend WITH(NOLOCK) WHERE mtrend_year=YEAR(getdate()) AND mtrend_month = 1 AND mtrend_amod_id = amod_id)  end) AS YEAR5VAL")
      sQuery.Append(" FROM Aircraft_Model WITH(NOLOCK)")
      sQuery.Append(" WHERE ")

      If Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
        Dim tmpStr As String = ""

        ' flatten out amodID array ...
        For x As Integer = 0 To UBound(searchCriteria.ViewCriteriaAmodIDArray)
          If String.IsNullOrEmpty(tmpStr) Then
            tmpStr = searchCriteria.ViewCriteriaAmodIDArray(x)
          Else
            tmpStr += Constants.cCommaDelim + searchCriteria.ViewCriteriaAmodIDArray(x)
          End If
        Next

        sQuery.Append("amod_id IN (" + tmpStr.Trim + ")")
      ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append("amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      ElseIf searchCriteria.ViewCriteriaSecondAmodID > -1 Then
        sQuery.Append("amod_id = " + searchCriteria.ViewCriteriaSecondAmodID.ToString)
      ElseIf searchCriteria.ViewCriteriaThirdAmodID > -1 Then
        sQuery.Append("amod_id = " + searchCriteria.ViewCriteriaThirdAmodID.ToString)
      ElseIf Not IsNothing(searchCriteria.ViewCriteriaMakeIDArray) Then
        sQuery.Append("amod_make_name IN ('" + searchCriteria.ViewCriteriaAircraftMake.ToUpper.Replace(Constants.cCommaDelim, Constants.cValueSeperator).Trim + "')")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append("amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftType.Trim) Then
        sQuery.Append("amod_type_code = '" + searchCriteria.ViewCriteriaAircraftType.Trim + "'")
      End If

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), True, False))
      Else
        sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      End If

      SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
      SqlConn.Open()
      SqlCommand.Connection = SqlConn


      clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

      SqlCommand.CommandText = sQuery.ToString
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        temptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = temptable.GetErrors()
      End Try

    Catch ex As Exception

      Return Nothing
      aError = "Error in " + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + " " + ex.Message
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    Finally
      SqlReader = Nothing

      SqlConn.Dispose()
      SqlConn.Close()
      SqlConn = Nothing

      SqlCommand.Dispose()
      SqlCommand = Nothing
    End Try

    Return temptable

  End Function

  Public Sub views_display_deliveries_operational_trends_graph(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_scriptString As String, ByRef out_htmlString As String, ByVal graphID As Integer)

    Dim htmlOut As New StringBuilder
    Dim scriptOut As New StringBuilder
    Dim results_table As New DataTable

    Try

      results_table = get_deliveries_operational_trends(searchCriteria)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          scriptOut.Append("function drawVisualization" + graphID.ToString + "() {" + vbCrLf)
          'scriptOut.Append(" alert('drawVisualization" + graphID.ToString + "');" + vbCrLf)  
          scriptOut.Append(" var data = new google.visualization.DataTable();" + vbCrLf)

          scriptOut.Append(" data.addColumn('string', 'Year');" + vbCrLf)
          scriptOut.Append(" data.addColumn('number', 'In Operation');" + vbCrLf)

          scriptOut.Append(" data.addRows([" + vbCrLf)


          For Each r As DataRow In results_table.Rows

            For x As Integer = 1 To 5

              scriptOut.Append(IIf(x > 1, ",['", "['"))

              If Not IsDBNull(r.Item("YEAR" + x.ToString + "YEAR")) Then
                If Not String.IsNullOrEmpty(r.Item("YEAR" + x.ToString + "YEAR").ToString.Trim) Then

                  If CLng(r.Item("YEAR" + x.ToString + "YEAR").ToString) > 0 Then
                    scriptOut.Append(r.Item("YEAR" + x.ToString + "YEAR").ToString + "'")
                  Else
                    scriptOut.Append("0'")
                  End If

                Else
                  scriptOut.Append("0'")
                End If

              Else
                scriptOut.Append("0'")
              End If

              If Not IsDBNull(r.Item("YEAR" + x.ToString + "VAL")) Then
                If Not String.IsNullOrEmpty(r.Item("YEAR" + x.ToString + "VAL").ToString.Trim) Then

                  If CLng(r.Item("YEAR" + x.ToString + "VAL").ToString) > 0 Then
                    scriptOut.Append("," + r.Item("YEAR" + x.ToString + "VAL").ToString + "]")
                  Else
                    scriptOut.Append(",0]")
                  End If

                Else
                  scriptOut.Append(",0]")
                End If

              Else
                scriptOut.Append(",0]")
              End If

            Next

          Next

          scriptOut.Append("]);" + vbCrLf)

          scriptOut.Append("var options = { " + vbCrLf)
          scriptOut.Append("  chartArea:{width:'74%',height:'75%'}," + vbCrLf)
          scriptOut.Append("  hAxis: { title: 'Year'," + vbCrLf)
          scriptOut.Append("           textStyle: { color: '#01579b', fontSize: 14, fontName:  'Arial', bold: true, italic: true }, " + vbCrLf)
          scriptOut.Append("           titleTextStyle: { color: '#01579b', fontSize: 14, fontName:  'Arial', bold: false, italic: true }" + vbCrLf)
          scriptOut.Append("         }," + vbCrLf)
          scriptOut.Append("  vAxis: { title: 'In Operation'," + vbCrLf)
          scriptOut.Append("           textStyle: { color: '#1a237e', fontSize: 14, bold: true }," + vbCrLf)
          scriptOut.Append("           titleTextStyle: { color: '#1a237e', fontSize: 16, bold: true }" + vbCrLf)
          scriptOut.Append("        }," + vbCrLf)
          scriptOut.Append("  smoothLine:true," + vbCrLf)
          scriptOut.Append("  legend:'none'," + vbCrLf)
          scriptOut.Append("  colors: ['black','red', 'blue', 'green', 'orange']" + vbCrLf)
          scriptOut.Append("};" + vbCrLf)


          scriptOut.Append(" var chart = new google.visualization.LineChart(document.getElementById('visualization" + graphID.ToString + "'));" + vbCrLf)
          scriptOut.Append(" chart.draw(data, options);" + vbCrLf)
          scriptOut.Append("}" + vbCrLf)

        End If

      End If

      If Not String.IsNullOrEmpty(scriptOut.ToString.Trim) Then
        htmlOut.Append("<table id=""modeloperationalTrendsTable"" width=""100%"" cellspacing=""0"" cellpadding=""4"">")
        htmlOut.Append("<tr><td valign=""top"" align=""center""><strong>In Operation Aircraft (Last 5 Years)</strong>")
        htmlOut.Append("<tr><td valign=""top"" align=""left""><div id='visualization" + graphID.ToString + "' style=""height:295px;""></div></td></tr>")
        htmlOut.Append("</table>" + vbCrLf)
      Else
        htmlOut.Append("<table id=""modeloperationalTrendsTable"" width=""100%"" cellspacing=""0"" cellpadding=""4"">")
        htmlOut.Append("<tr><td valign=""top"" align=""center""><strong>In Operation Aircraft (Last 5 Years)</strong>")
        htmlOut.Append("<tr><td valign=""middle"" align=""center"">No In Operation Aircraft Data at this time, for this Make/Model ...</td></tr>")
        htmlOut.Append("</table>" + vbCrLf)
      End If

    Catch ex As Exception

      aError = "Error in " + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + " " + ex.Message
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    Finally

    End Try

    'return resulting html string
    out_scriptString = scriptOut.ToString
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function build_deliveries_history_link(ByRef searchCriteria As viewSelectionCriteriaClass) As String
    Dim htmlOut As New StringBuilder

    Dim sRefLink As String = ""
    Dim sRefTitle As String = ""
    Dim tmpTransLink As String = ""
    Dim tmpAcDetails As String = ""

    Dim nTmpIndex As Long = -1

    Dim sAirFrame As String = ""
    Dim sAirType As String = ""
    Dim sMake As String = ""
    Dim sModel As String = ""
    Dim sUsage As String = ""
    sRefLink = "javascript:ParseForm('0',true,false,false,false,false,'"

    Try

      Dim mktSummaryFunctions As New marketSummaryFunctions

      If searchCriteria.ViewCriteriaAmodID > -1 Then ' String.Join(crmWebClient.Constants.cCommaDelim, ColumnSet)
        Dim acObject As New marketSummaryObjAircraft

        If IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
          acObject.ModelsString = searchCriteria.ViewCriteriaAmodID.ToString()
        Else
          acObject.ModelsString = String.Join(Constants.cCommaDelim, searchCriteria.ViewCriteriaAmodIDArray)
        End If

        acObject.MakeString = searchCriteria.ViewCriteriaAircraftMake
        acObject.TypeString = searchCriteria.ViewCriteriaAircraftType
        acObject.AirframeTypeString = IIf(searchCriteria.ViewCriteriaAirframeType <> Constants.VIEW_HELICOPTERS, "F", "R")
        acObject.CombinedAirframeTypeString = ""

        tmpAcDetails = mktSummaryFunctions.make_linkback_aircraftInfo(acObject)

      End If

      If Not String.IsNullOrEmpty(tmpAcDetails.Trim) Then
        sRefLink += tmpAcDetails.Trim + "!~!"
      End If

      ' transaction date (range)
      sRefLink += "journ_date_operator=Between!~!journ_date=" + Format(CDate("01/01/" + DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now()).Year.ToString), "MM/dd/yyyy") + ":" + Format(Now, "MM/dd/yyyy") + "!~!"

      ' transaction type
      HttpContext.Current.Session.Item("marketNewUsed") = "new"
      tmpTransLink = mktSummaryFunctions.make_linkback_transactionInfo(IIf(bIncludeLeases, "WS,L", "WS"), True, True, True, "journ_subcategory_code <> ?CORR?").Trim

      If Not String.IsNullOrEmpty(tmpTransLink) Then
        sRefLink += tmpTransLink + "!~!"
      End If

      sRefLink += "clearSelection=true!~!fromMarketSummary=false');"

      sRefTitle = IIf(HttpContext.Current.Application.Item("DebugFlag").ToString, " title=""" + sRefLink.Trim + """", " title=""Click to view Transactions""")

      htmlOut.Append("<a class=""button-darker"" href=""" + sRefLink + """" + sRefTitle + ">View Transactions</a>")

    Catch ex As Exception

      aError = "Error in " + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + " " + ex.Message
      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message

    Finally

    End Try

    Return htmlOut.ToString()

  End Function

End Class
