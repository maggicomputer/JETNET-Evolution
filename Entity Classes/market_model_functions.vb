Imports Microsoft.VisualBasic
Imports System.ComponentModel

' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/market_model_functions.vb $
'$$Author: Amanda $
'$$Date: 6/26/20 4:20p $
'$$Modtime: 6/26/20 4:12p $
'$$Revision: 8 $
'$$Workfile: market_model_functions.vb $
'
' ********************************************************************************

<System.Serializable()> Public Class market_model_functions

  Private aError As String
  Private clientConnectString As String
  Private adminConnectString As String

  Private starConnectString As String
  Private cloudConnectString As String
  Private serverConnectString As String

  Sub New()

    aError = ""
    clientConnectString = ""
    adminConnectString = ""

    starConnectString = ""
    cloudConnectString = ""
    serverConnectString = ""

  End Sub

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

#Region "market_model_functions"

  Public Function get_avg_price_by_month_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable
    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sSeperator As String = ""

    Try

      sQuery.Append("SELECT DISTINCT mtrend_year, mtrend_month, ((sum(cast(mtrend_avail_asking_price_total as float))/sum(cast(NULLIF([mtrend_avail_asking_price_count],0) as float)))) AS avgPrice")
      sQuery.Append(" FROM Aircraft_Model_Trend WITH(NOLOCK) INNER JOIN aircraft_model WITH(NOLOCK) ON mtrend_amod_id = amod_id")
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
        sSeperator = Constants.cAndClause
      ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append("amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
        sSeperator = Constants.cAndClause
      ElseIf searchCriteria.ViewCriteriaSecondAmodID > -1 Then
        sQuery.Append("amod_id = " + searchCriteria.ViewCriteriaSecondAmodID.ToString)
        sSeperator = Constants.cAndClause
      ElseIf searchCriteria.ViewCriteriaThirdAmodID > -1 Then
        sQuery.Append("amod_id = " + searchCriteria.ViewCriteriaThirdAmodID.ToString)
        sSeperator = Constants.cAndClause
      ElseIf Not IsNothing(searchCriteria.ViewCriteriaMakeIDArray) Then
        sQuery.Append("amod_make_name IN ('" + searchCriteria.ViewCriteriaAircraftMake.ToUpper.Replace(Constants.cCommaDelim, Constants.cValueSeperator).Trim + "')")
        sSeperator = Constants.cAndClause
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append("amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
        sSeperator = Constants.cAndClause
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftType.Trim) Then
        sQuery.Append("amod_type_code = '" + searchCriteria.ViewCriteriaAircraftType.Trim + "'")
        sSeperator = Constants.cAndClause
      End If

      Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
        Case Constants.VIEW_EXECUTIVE
          sQuery.Append(sSeperator + "amod_airframe_type_code='F' AND amod_type_code = 'E'")
          sSeperator = Constants.cAndClause
        Case Constants.VIEW_JETS
          sQuery.Append(sSeperator + "amod_airframe_type_code='F' AND amod_type_code = 'J'")
          sSeperator = Constants.cAndClause
        Case Constants.VIEW_TURBOPROPS
          sQuery.Append(sSeperator + "amod_airframe_type_code='F' AND amod_type_code = 'T'")
          sSeperator = Constants.cAndClause
        Case Constants.VIEW_PISTONS
          sQuery.Append(sSeperator + "amod_airframe_type_code='F' AND amod_type_code = 'P'")
          sSeperator = Constants.cAndClause
        Case Constants.VIEW_HELICOPTERS
          sQuery.Append(sSeperator + "amod_airframe_type_code='R' AND amod_type_code in ('T','P')")
          sSeperator = Constants.cAndClause
      End Select

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(sSeperator + commonEvo.MakeMarketProductCodeClause(HttpContext.Current.Session.Item("localPreferences"), True))
        sSeperator = Constants.cAndClause
      Else
        sQuery.Append(sSeperator + commonEvo.BuildMarketProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, True))
        sSeperator = Constants.cAndClause
      End If

      sQuery.Append(make_mtrend_year_query_string(searchCriteria.ViewCriteriaTimeSpan)) 
      
      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True))
      Else
        sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      End If

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaWeightClass.Trim) Then
        If searchCriteria.ViewCriteriaWeightClass.Contains(",") Then
          sQuery.Append(Constants.cAndClause + "amod_weight_class IN ('" + searchCriteria.ViewCriteriaWeightClass.ToUpper.Replace(Constants.cCommaDelim, Constants.cValueSeperator).Trim + "')")
        Else
          sQuery.Append(Constants.cAndClause + "amod_weight_class = '" + searchCriteria.ViewCriteriaWeightClass.ToUpper.Trim + "'")
        End If
      End If

      sQuery.Append(" GROUP BY mtrend_year, mtrend_month ORDER BY mtrend_year ASC, mtrend_month ASC")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_avg_price_by_month_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_avg_price_by_month_info load datatable" + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_avg_price_by_month_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable" + ex.Message

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
  Public Function make_mtrend_year_query_string(ByVal month_range As Integer) As String

    ' if however many months we go back are in the same year, then do it differently 
    If Year(DateAdd("m", (-1) * month_range, Now())) = Year(Now()) Then
      make_mtrend_year_query_string = (" AND ( ")

      make_mtrend_year_query_string &= ("(mtrend_month >= month(CONVERT(DATETIME, '" + DateAdd("m", (-1) * month_range, Now()).ToString + "',102)))")
      make_mtrend_year_query_string &= (" and (mtrend_year = year(CONVERT(DATETIME, '" + DateAdd("m", (-1) * month_range, Now()).ToString + "',102)))")

      make_mtrend_year_query_string &= (" ) ")
    Else
      make_mtrend_year_query_string = (" AND ( ")

      make_mtrend_year_query_string &= (" ((mtrend_year = year(CONVERT(DATETIME, '" + DateAdd("m", (-1) * month_range, Now()).ToString + "',102)))")
      make_mtrend_year_query_string &= (Constants.cAndClause & "(mtrend_month >= month(CONVERT(DATETIME, '" + DateAdd("m", (-1) * month_range, Now()).ToString + "',102))))")

      make_mtrend_year_query_string &= (" OR ")

      make_mtrend_year_query_string &= (" (mtrend_year > year(CONVERT(DATETIME, '" + DateAdd("m", (-1) * month_range, Now()).ToString + "',102)))")

      make_mtrend_year_query_string &= (" OR ((mtrend_year = year(CONVERT(DATETIME, '" + Now.ToString + "',102)))" + Constants.cAndClause + "(mtrend_month <= month(CONVERT(DATETIME, '" + Now.ToString + "',102))))")

      make_mtrend_year_query_string &= (" ) ")
    End If






  End Function

  Public Function get_actual_asking_price_by_month_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable
    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT ac_asking_price FROM aircraft WITH(NOLOCK) INNER JOIN aircraft_model WITH(NOLOCK) ON amod_id = ac_amod_id")
      sQuery.Append(" WHERE ac_forsale_flag = 'Y' AND ac_journ_id = 0 AND ac_asking_price <> ''")

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
        sQuery.Append(Constants.cAndClause + "amod_type_code = '" + searchCriteria.ViewCriteriaAircraftType.Trim + "'")
      End If

      Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
        Case Constants.VIEW_EXECUTIVE
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'E'")
        Case Constants.VIEW_JETS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'J'")
        Case Constants.VIEW_TURBOPROPS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'T'")
        Case Constants.VIEW_PISTONS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'P'")
        Case Constants.VIEW_HELICOPTERS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='R' AND amod_type_code in ('T','P')")
      End Select

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaWeightClass.Trim) Then
        If searchCriteria.ViewCriteriaWeightClass.Contains(",") Then
          sQuery.Append(Constants.cAndClause + "amod_weight_class IN ('" + searchCriteria.ViewCriteriaWeightClass.ToUpper.Replace(Constants.cCommaDelim, Constants.cValueSeperator).Trim + "')")
        Else
          sQuery.Append(Constants.cAndClause + "amod_weight_class = '" + searchCriteria.ViewCriteriaWeightClass.ToUpper.Trim + "'")
        End If
      End If

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True))
      Else
        sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_actual_asking_price_by_month_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_actual_asking_price_by_month_info load datatable" + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_actual_asking_price_by_month_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable" + ex.Message

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

  Public Sub views_display_avg_price_by_month_graph(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef AVG_PRICE_MONTH As DataVisualization.Charting.Chart, Optional ByRef google_chart_string As String = "", Optional ByVal make_larger_no_solds As Boolean = False, Optional ByVal values_avg_asking As Long = 0, Optional ByRef avg_price As String = "")

    Dim results_table As New DataTable

    Dim high_number As Double = 0.0
    Dim low_number As Double = 100000000.0
    Dim starting_point As Integer = 0
    Dim ending_point As Integer = 0
    Dim interval_point As Integer = 1

    Dim ac_asking_price As Double = 0.0
    Dim ac_asking_price_count As Integer = 0
    Dim temp_average As Double = 0.0
    Dim orig_avg As Double = 0
    Dim x As Integer = 0

    Dim bIsFirstLoop As Boolean = True

    Try

      results_table = get_avg_price_by_month_info(searchCriteria)

      AVG_PRICE_MONTH.Series.Clear()
      AVG_PRICE_MONTH.Series.Add("AVG_PRICE").ChartType = UI.DataVisualization.Charting.SeriesChartType.Spline
      AVG_PRICE_MONTH.Series("AVG_PRICE").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
      AVG_PRICE_MONTH.Series("AVG_PRICE").LabelForeColor = Drawing.Color.Blue
      AVG_PRICE_MONTH.ChartAreas("ChartArea1").AxisY.Title = "Avg Asking Price-In Thousands US$"
      AVG_PRICE_MONTH.Series("AVG_PRICE").Color = Drawing.Color.Blue
      AVG_PRICE_MONTH.Series("AVG_PRICE").BorderWidth = 1
      AVG_PRICE_MONTH.Series("AVG_PRICE").MarkerSize = 5
      AVG_PRICE_MONTH.Series("AVG_PRICE").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle
      AVG_PRICE_MONTH.Series("AVG_PRICE").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32

      If make_larger_no_solds = True Then
        AVG_PRICE_MONTH.Width = 400
        AVG_PRICE_MONTH.Height = 350
      Else
        AVG_PRICE_MONTH.Width = 300
        AVG_PRICE_MONTH.Height = 300
      End If

      If Not IsNothing(results_table) Then

        google_chart_string = " data1.addColumn('string', 'Serial#'); "
        google_chart_string += " data1.addColumn('number', 'Asking'); "
        google_chart_string += " data1.addRows(["

        If results_table.Rows.Count > 0 Then

          For Each r As DataRow In results_table.Rows

            If Not IsDBNull(r("mtrend_year")) Then
              If Not String.IsNullOrEmpty(r.Item("mtrend_year").ToString) Then

                If Not IsDBNull(r("mtrend_month")) Then
                  If Not String.IsNullOrEmpty(r.Item("mtrend_month").ToString) Then

                    If Not IsDBNull(r("avgPrice")) Then
                      If CDbl(r.Item("avgPrice").ToString) > 0 Then

                        temp_average = CDbl(r.Item("avgPrice").ToString)
                        orig_avg = CDbl(temp_average)
                        temp_average = CDbl(temp_average / 1000)

                        If high_number = 0 Or CDbl(temp_average) > high_number Then
                          high_number = Math.Round(temp_average, 2)
                        End If

                        If low_number = 0 Or CDbl(temp_average) < low_number Then
                          low_number = Math.Round(temp_average, 2)
                        End If

                        AVG_PRICE_MONTH.Series("AVG_PRICE").Points.AddXY((r.Item("mtrend_month").ToString + "-" + r.Item("mtrend_year").ToString), CDbl(temp_average))

                        If Not bIsFirstLoop Then
                          google_chart_string += ",['" + (r.Item("mtrend_month").ToString + "-" + r.Item("mtrend_year").ToString) + "', " + Replace(FormatNumber((CDbl(r.Item("avgPrice").ToString) / 1000), 1), ",", "") + "]"
                        Else
                          google_chart_string += "['" + (r.Item("mtrend_month").ToString + "-" + r.Item("mtrend_year").ToString) + "', " + Replace(FormatNumber((CDbl(r.Item("avgPrice").ToString) / 1000), 1), ",", "") + "]"
                        End If

                      Else

                        AVG_PRICE_MONTH.Series("AVG_PRICE").Points.AddXY((r.Item("mtrend_month").ToString + "-" + r.Item("mtrend_year").ToString), 0)

                        If Not bIsFirstLoop Then
                          google_chart_string += ",['" + (r.Item("mtrend_month").ToString + "-" + r.Item("mtrend_year").ToString) + "', 0]"
                        Else
                          google_chart_string += "['" + (r.Item("mtrend_month").ToString + "-" + r.Item("mtrend_year").ToString) + "', 0]"
                        End If

                      End If

                      temp_average = CDbl(temp_average * 1000)
                      If Trim(avg_price) <> "" Then 
                        avg_price &= "," & temp_average
                      Else 
                        avg_price = temp_average
                      End If

                      bIsFirstLoop = False
                    End If


                    x += 1

                  End If
                End If
              End If
            End If

          Next

        End If
      End If

      results_table = Nothing

      If values_avg_asking > 0 Then
        If Not bIsFirstLoop Then
          google_chart_string += ",['" & (Month(Date.Now) & "-" & Year(Date.Now())) & "', " & Replace(values_avg_asking, ",", "") & "]"
        Else
          google_chart_string += "['" & (Month(Date.Now) & "-" & Year(Date.Now())) & "', " & Replace(values_avg_asking, ",", "") & "]"
        End If
      Else

        If HttpContext.Current.Session.Item("localPreferences").DatabaseType <> eDatabaseTypes.MONTHLY Then

          results_table = get_actual_asking_price_by_month_info(searchCriteria)

          If Not IsNothing(results_table) Then

            If results_table.Rows.Count > 0 Then

              For Each r As DataRow In results_table.Rows

                ac_asking_price += CDbl(r.Item("ac_asking_price").ToString)
                ac_asking_price_count += 1

              Next

              ac_asking_price = CDbl(ac_asking_price / ac_asking_price_count)

              ac_asking_price = CDbl(ac_asking_price / 1000)

              AVG_PRICE_MONTH.Series("AVG_PRICE").Points.AddXY((Date.Now.Month.ToString + "-" + Date.Now.Year.ToString), ac_asking_price)
              AVG_PRICE_MONTH.Series("AVG_PRICE").Points(x).Color = Drawing.Color.Black
              AVG_PRICE_MONTH.Series("AVG_PRICE").Points(x).BorderDashStyle = DataVisualization.Charting.ChartDashStyle.Dash

              If ac_asking_price > high_number Then
                high_number = ac_asking_price
              End If

              If ac_asking_price < low_number Then
                low_number = ac_asking_price
              End If

              If Not bIsFirstLoop Then
                google_chart_string += ",['" & (Month(Date.Now) & "-" & Year(Date.Now())) & "', " & Replace(ac_asking_price, ",", "") & "]"
              Else
                google_chart_string += "['" & (Month(Date.Now) & "-" & Year(Date.Now())) & "', " & Replace(ac_asking_price, ",", "") & "]"
              End If

              If Trim(avg_price) <> "" Then
                avg_price &= "," & temp_average
              Else
                avg_price = temp_average
              End If


            End If
          End If


        End If
      End If

      If AVG_PRICE_MONTH.Series("AVG_PRICE").Points.Count < 1 Then
        AVG_PRICE_MONTH.Titles.Clear()
        AVG_PRICE_MONTH.Titles.Add("No Avg Asking Price")
      End If

      results_table = Nothing

      commonEvo.set_ranges_for_vsCharts(low_number, high_number, interval_point, starting_point, ending_point)

      'debug_output.Text += "HI: " + high_number.ToString + " LO: " + low_number.ToString + "<br />"
      'debug_output.Text += "EP: " + ending_point.ToString + " SP: " + IIf(starting_point > 0, starting_point, 0).ToString + " IP: " + interval_point.ToString + "<br />"
      'debug_output.Text += "SZ: " + CInt((high_number.ToString.Length + low_number.ToString.Length) / 2).ToString + " RG: " + Math.Abs(high_number - low_number).ToString + "<br />"

      AVG_PRICE_MONTH.ChartAreas("ChartArea1").AxisY.Maximum = ending_point
      AVG_PRICE_MONTH.ChartAreas("ChartArea1").AxisY.Minimum = IIf(starting_point > 0, starting_point, 0)
      AVG_PRICE_MONTH.ChartAreas("ChartArea1").AxisY.Interval = interval_point

    Catch ex As Exception

      aError = "Error in views_display_avg_price_by_month_graph(ByVal inModelID As Long, ByRef AVG_PRICE_MONTH As DataVisualization.Charting.Chart) " + ex.Message

    Finally

    End Try

  End Sub

  Public Function get_sold_per_month_info(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal in_bShowFuture As Boolean) As DataTable
    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("SELECT YEAR(journ_date) AS aYear, MONTH(journ_date) AS aMonth, count(*) AS aCount")
      sQuery.Append(" FROM Journal WITH(NOLOCK) INNER JOIN Aircraft WITH(NOLOCK) ON journ_ac_id = ac_id AND journ_id = ac_journ_id")
      sQuery.Append(" INNER JOIN aircraft_model WITH(NOLOCK) ON ac_amod_id = amod_id")

      sQuery.Append(" WHERE ((journ_date >= '" + Month(DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now())).ToString + "/01/" + Year(DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now())).ToString + "')")
      sQuery.Append(" AND (journ_date < '" + Now.Month.ToString)

      If in_bShowFuture Then
        sQuery.Append("/" + Now.Day.ToString + "/")
      Else
        sQuery.Append("/01/")
      End If

      sQuery.Append(Now.Year.ToString + "'))")

      sQuery.Append(" AND journ_subcategory_code like 'WS%' AND right(journ_subcategory_code,4) <> 'CORR' AND right(journ_subcategory_code,2) <> 'IT'")
      sQuery.Append("  and journ_internal_trans_flag = 'N' and  journ_subcat_code_part3 NOT IN ('DB','DS','FI','FY','IT','MF','RE','CC','LS', 'RM')   ")


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
        sQuery.Append(Constants.cAndClause + "amod_type_code = '" + searchCriteria.ViewCriteriaAircraftType.Trim + "'")
      End If

      Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
        Case Constants.VIEW_EXECUTIVE
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'E'")
        Case Constants.VIEW_JETS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'J'")
        Case Constants.VIEW_TURBOPROPS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'T'")
        Case Constants.VIEW_PISTONS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'P'")
        Case Constants.VIEW_HELICOPTERS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='R' AND amod_type_code in ('T','P')")
      End Select

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaWeightClass.Trim) Then
        If searchCriteria.ViewCriteriaWeightClass.Contains(",") Then
          sQuery.Append(Constants.cAndClause + "amod_weight_class IN ('" + searchCriteria.ViewCriteriaWeightClass.ToUpper.Replace(Constants.cCommaDelim, Constants.cValueSeperator).Trim + "')")
        Else
          sQuery.Append(Constants.cAndClause + "amod_weight_class = '" + searchCriteria.ViewCriteriaWeightClass.ToUpper.Trim + "'")
        End If
      End If

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True))
      Else
        sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      End If

      sQuery.Append(" GROUP BY year(journ_date), month(journ_date) ORDER BY year(journ_date), month(journ_date)")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_sold_per_month_info(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal in_bShowFuture As Boolean) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_sold_per_month_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_sold_per_month_info(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal in_bShowFuture As Boolean) As DataTable " + ex.Message

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

  Public Sub views_display_sold_per_month_graph(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal bShowFuture As Boolean, ByRef PER_MONTH_GRAPH As DataVisualization.Charting.Chart, Optional ByRef graph_string As String = "")

    Dim x As Integer = 0

    Dim high_number As Integer = 0
    Dim low_number As Integer = 0
    Dim current_month_to_show As Boolean = False
    Dim interval_point As Integer = 1
    Dim results_table As New DataTable
    Dim starting_point As Integer = 0
    Dim ending_point As Integer = 0

    Try

      results_table = get_sold_per_month_info(searchCriteria, bShowFuture)

      PER_MONTH_GRAPH.Series.Clear()
      PER_MONTH_GRAPH.Series.Add("PER_MONTH").ChartType = UI.DataVisualization.Charting.SeriesChartType.Spline
      PER_MONTH_GRAPH.Series("PER_MONTH").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
      PER_MONTH_GRAPH.Series("PER_MONTH").LabelForeColor = Drawing.Color.Blue
      PER_MONTH_GRAPH.ChartAreas("ChartArea1").AxisY.Title = "Aircraft Sold"
      PER_MONTH_GRAPH.Series("PER_MONTH").Color = Drawing.Color.Blue
      PER_MONTH_GRAPH.Series("PER_MONTH").BorderWidth = 1
      PER_MONTH_GRAPH.Series("PER_MONTH").MarkerSize = 5
      PER_MONTH_GRAPH.Series("PER_MONTH").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle
      PER_MONTH_GRAPH.Series("PER_MONTH").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
      PER_MONTH_GRAPH.Width = 300
      PER_MONTH_GRAPH.Height = 300

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          For Each r As DataRow In results_table.Rows
            If Not IsDBNull(r("aYear")) Then
              If Not String.IsNullOrEmpty(r.Item("aYear").ToString) Then

                If Not IsDBNull(r("aMonth")) Then
                  If Not String.IsNullOrEmpty(r.Item("aMonth").ToString) Then

                    If Not IsDBNull(r("aCount")) Then

                      If CDbl(r.Item("aCount").ToString) > 0 Then

                        If high_number = 0 Or CDbl(r.Item("aCount").ToString) > high_number Then
                          high_number = CDbl(r.Item("aCount").ToString)
                        End If

                        If low_number = 0 Or CDbl(r.Item("aCount")) < low_number Then
                          low_number = CDbl(r.Item("aCount").ToString)
                        End If

                        If CInt(r.Item("aMonth").ToString) = CInt(Now.Month.ToString) Then
                          current_month_to_show = True
                        End If

                        PER_MONTH_GRAPH.Series("PER_MONTH").Points.AddXY((r.Item("aMonth").ToString + "-" + r.Item("aYear").ToString), CDbl(r.Item("aCount").ToString))
                      Else
                        PER_MONTH_GRAPH.Series("PER_MONTH").Points.AddXY((r.Item("aMonth").ToString + "-" + r.Item("aYear").ToString), 0)
                      End If
                    Else
                      PER_MONTH_GRAPH.Series("PER_MONTH").Points.AddXY((r.Item("aMonth").ToString + "-" + r.Item("aYear").ToString), 0)
                    End If


                    If Trim(graph_string) <> "" Then
                      graph_string &= ", "
                    End If
                    graph_string += "['" & (r.Item("aMonth") & "-" & r.Item("aYear")) & "'," & r.Item("aCount") & " ]"



                    x += 1

                  End If
                End If
              End If
            End If

          Next

        End If
      End If

      graph_string = " data18.addColumn('string', 'Month/Year'); data18.addColumn('number', 'Average'); data18.addRows([ " & graph_string


      results_table = Nothing

      If bShowFuture And current_month_to_show Then
        PER_MONTH_GRAPH.Series("PER_MONTH").Points(x - 1).Color = Drawing.Color.Black
        PER_MONTH_GRAPH.Series("PER_MONTH").Points(x - 1).BorderDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
      End If

      'If high_number - low_number > 20 Then
      '  interval_point = 5
      'ElseIf high_number - high_number > 10 Then
      '  interval_point = 2
      'End If


      commonEvo.set_ranges_for_vsCharts(low_number, high_number, interval_point, starting_point, ending_point)

      PER_MONTH_GRAPH.ChartAreas("ChartArea1").AxisY.Maximum = ending_point
      PER_MONTH_GRAPH.ChartAreas("ChartArea1").AxisY.Minimum = IIf(starting_point > 0, starting_point, 0)
      PER_MONTH_GRAPH.ChartAreas("ChartArea1").AxisY.Interval = interval_point


      ' PER_MONTH_GRAPH.ChartAreas("ChartArea1").AxisY.Maximum = high_number + 1
      ' PER_MONTH_GRAPH.ChartAreas("ChartArea1").AxisY.Minimum = low_number - 1
      ' PER_MONTH_GRAPH.ChartAreas("ChartArea1").AxisY.Interval = interval_point

    Catch ex As Exception

      aError = "Error in views_display_avg_price_by_month_graph(ByVal inModelID As Long, ByRef AVG_PRICE_MONTH As DataVisualization.Charting.Chart) " + ex.Message

    Finally

    End Try

  End Sub
 
  Public Function get_average_days_on_market_info(ByRef searchCriteria As viewSelectionCriteriaClass, Optional ByVal use_nonsale_data As Boolean = False) As DataTable
    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sSeperator As String = ""

    Try

      If use_nonsale_data = True Then
        sQuery.Append("SELECT DISTINCT mtrend_year, mtrend_month, AC14.ac14_forsale_avg_dom as mtrend_avg_market_days")
        ' changed the name here so we dont have to change the next page  -->>
      Else
        sQuery.Append("SELECT DISTINCT mtrend_year, mtrend_month, mtrend_avg_market_days")
     End If
  
      sQuery.Append(" FROM Aircraft_Model_Trend WITH(NOLOCK) INNER JOIN aircraft_model WITH(NOLOCK) ON mtrend_amod_id = amod_id")
      sQuery.Append(" INNER JOIN star_reports.dbo.Aircraft_14 AS AC14 ON AC14.ac14_amod_id = mtrend_amod_id AND YEAR(AC14.ac14_start_date) = mtrend_year AND MONTH(AC14.ac14_start_date) = mtrend_month ")

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
        sSeperator = Constants.cAndClause
      ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append("amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
        sSeperator = Constants.cAndClause
      ElseIf searchCriteria.ViewCriteriaSecondAmodID > -1 Then
        sQuery.Append("amod_id = " + searchCriteria.ViewCriteriaSecondAmodID.ToString)
        sSeperator = Constants.cAndClause
      ElseIf searchCriteria.ViewCriteriaThirdAmodID > -1 Then
        sQuery.Append("amod_id = " + searchCriteria.ViewCriteriaThirdAmodID.ToString)
        sSeperator = Constants.cAndClause
      ElseIf Not IsNothing(searchCriteria.ViewCriteriaMakeIDArray) Then
        sQuery.Append("amod_make_name IN ('" + searchCriteria.ViewCriteriaAircraftMake.ToUpper.Replace(Constants.cCommaDelim, Constants.cValueSeperator).Trim + "')")
        sSeperator = Constants.cAndClause
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append("amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
        sSeperator = Constants.cAndClause
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftType.Trim) Then
        sQuery.Append("amod_type_code = '" + searchCriteria.ViewCriteriaAircraftType.Trim + "'")
        sSeperator = Constants.cAndClause
      End If

      Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
        Case Constants.VIEW_EXECUTIVE
          sQuery.Append(sSeperator + "amod_airframe_type_code='F' AND amod_type_code = 'E'")
          sSeperator = Constants.cAndClause
        Case Constants.VIEW_JETS
          sQuery.Append(sSeperator + "amod_airframe_type_code='F' AND amod_type_code = 'J'")
          sSeperator = Constants.cAndClause
        Case Constants.VIEW_TURBOPROPS
          sQuery.Append(sSeperator + "amod_airframe_type_code='F' AND amod_type_code = 'T'")
          sSeperator = Constants.cAndClause
        Case Constants.VIEW_PISTONS
          sQuery.Append(sSeperator + "amod_airframe_type_code='F' AND amod_type_code = 'P'")
          sSeperator = Constants.cAndClause
        Case Constants.VIEW_HELICOPTERS
          sQuery.Append(sSeperator + "amod_airframe_type_code='R' AND amod_type_code in ('T','P')")
          sSeperator = Constants.cAndClause
      End Select

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(sSeperator + commonEvo.MakeMarketProductCodeClause(HttpContext.Current.Session.Item("localPreferences"), True))
      Else
        sQuery.Append(sSeperator + commonEvo.BuildMarketProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, True))
      End If

      sQuery.Append(sSeperator + "(((mtrend_year >= year(CONVERT(DATETIME, '" + DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now()).ToString + "',102)))")
      sQuery.Append(Constants.cAndClause & "(mtrend_month >= month(CONVERT(DATETIME, '" + DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now()).ToString + "',102))))")

      Dim sClause As String = ""
      If Year(DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now())) <> Year(Now()) Then
        sClause = " OR "
      Else
        sClause = " AND "
      End If

      sQuery.Append(sClause & "((mtrend_year = year(CONVERT(DATETIME, '" + Now.ToString + "',102)))" + Constants.cAndClause + "(mtrend_month <= month(CONVERT(DATETIME, '" + Now.ToString + "',102)))))")

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True))
      Else
        sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      End If

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaWeightClass.Trim) Then
        If searchCriteria.ViewCriteriaWeightClass.Contains(",") Then
          sQuery.Append(Constants.cAndClause + "amod_weight_class IN ('" + searchCriteria.ViewCriteriaWeightClass.ToUpper.Replace(Constants.cCommaDelim, Constants.cValueSeperator).Trim + "')")
        Else
          sQuery.Append(Constants.cAndClause + "amod_weight_class = '" + searchCriteria.ViewCriteriaWeightClass.ToUpper.Trim + "'")
        End If
      End If

      If use_nonsale_data = True Then
        sQuery.Append(" GROUP BY mtrend_year, mtrend_month, AC14.ac14_forsale_avg_dom ORDER BY mtrend_year ASC, mtrend_month ASC")
      Else
        sQuery.Append(" GROUP BY mtrend_year, mtrend_month, mtrend_avg_market_days ORDER BY mtrend_year ASC, mtrend_month ASC")
      End If
      


      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_average_days_on_market_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_average_days_on_market_info load datatable" + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_average_days_on_market_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable" + ex.Message

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

  Public Function get_actual_average_days_on_market_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable
    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try
      sQuery.Append("SELECT DATEDIFF(d,ac_list_date,getdate()) AS daysOnMarket FROM aircraft WITH(NOLOCK) INNER JOIN aircraft_model WITH(NOLOCK) ON amod_id = ac_amod_id")
      sQuery.Append(" WHERE ac_forsale_flag = 'Y' AND ac_journ_id = 0 AND ac_list_date <> ''")

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
        sQuery.Append(Constants.cAndClause + "amod_type_code = '" + searchCriteria.ViewCriteriaAircraftType.Trim + "'")
      End If

      Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
        Case Constants.VIEW_EXECUTIVE
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'E'")
        Case Constants.VIEW_JETS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'J'")
        Case Constants.VIEW_TURBOPROPS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'T'")
        Case Constants.VIEW_PISTONS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'P'")
        Case Constants.VIEW_HELICOPTERS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='R' AND amod_type_code in ('T','P')")
      End Select

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaWeightClass.Trim) Then
        If searchCriteria.ViewCriteriaWeightClass.Contains(",") Then
          sQuery.Append(Constants.cAndClause + "amod_weight_class IN ('" + searchCriteria.ViewCriteriaWeightClass.ToUpper.Replace(Constants.cCommaDelim, Constants.cValueSeperator).Trim + "')")
        Else
          sQuery.Append(Constants.cAndClause + "amod_weight_class = '" + searchCriteria.ViewCriteriaWeightClass.ToUpper.Trim + "'")
        End If
      End If

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True))
      Else
        sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_actual_average_days_on_market_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_actual_average_days_on_market_info load datatable" + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_actual_average_days_on_market_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable" + ex.Message

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

  Public Sub views_display_average_days_on_market_graph(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef AVG_DAYS_ON As DataVisualization.Charting.Chart, Optional ByVal use_past_market_data As Boolean = False, Optional ByRef graph_string As String = "")

    Dim high_number As Integer = 0
    Dim low_number As Integer = 0
    Dim starting_point As Integer = 0
    Dim interval_point As Integer = 1

    Dim daysonmarket As Integer = 0
    Dim daysonmarket2 As Integer = 0
    Dim days As Integer = 0
    Dim x As Integer = 0

    Dim results_table As New DataTable

    Try
      results_table = get_average_days_on_market_info(searchCriteria, use_past_market_data)

      AVG_DAYS_ON.Series.Clear()
      AVG_DAYS_ON.Series.Add("AVG_DAYS").ChartType = UI.DataVisualization.Charting.SeriesChartType.Spline
      AVG_DAYS_ON.Series("AVG_DAYS").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
      AVG_DAYS_ON.Series("AVG_DAYS").LabelForeColor = Drawing.Color.Blue
      AVG_DAYS_ON.ChartAreas("ChartArea1").AxisY.Title = "Avg Days On Market"
      AVG_DAYS_ON.Series("AVG_DAYS").Color = Drawing.Color.Blue
      AVG_DAYS_ON.Series("AVG_DAYS").BorderWidth = 1
      AVG_DAYS_ON.Series("AVG_DAYS").MarkerSize = 5
      AVG_DAYS_ON.Series("AVG_DAYS").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle
      AVG_DAYS_ON.Series("AVG_DAYS").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
      AVG_DAYS_ON.Width = 300
      AVG_DAYS_ON.Height = 300

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          For Each r As DataRow In results_table.Rows
            If Not IsDBNull(r("mtrend_year")) Then
              If Not String.IsNullOrEmpty(r.Item("mtrend_year").ToString) Then

                If Not IsDBNull(r("mtrend_month")) Then
                  If Not String.IsNullOrEmpty(r.Item("mtrend_month").ToString) Then

                    If Not IsDBNull(r("mtrend_avg_market_days")) Then
                      If CDbl(r.Item("mtrend_avg_market_days").ToString) > 0 Then

                        If high_number = 0 Or CDbl(r.Item("mtrend_avg_market_days").ToString) > high_number Then
                          high_number = CDbl(r.Item("mtrend_avg_market_days").ToString)
                        End If

                        If low_number = 0 Or CDbl(r.Item("mtrend_avg_market_days")) < low_number Then
                          low_number = CDbl(r.Item("mtrend_avg_market_days").ToString)
                        End If

                        AVG_DAYS_ON.Series("AVG_DAYS").Points.AddXY((r.Item("mtrend_month").ToString + "-" + r.Item("mtrend_year").ToString), CDbl(r.Item("mtrend_avg_market_days").ToString))

                        If Trim(graph_string) <> "" Then
                          graph_string &= ", "
                        End If
                        graph_string += "['" & (r.Item("mtrend_month") & "-" & r.Item("mtrend_year")) & "'," & r.Item("mtrend_avg_market_days") & " ]"
 
                        x += 1

                      End If
                    End If

                  End If
                End If
              End If
            End If

          Next

        End If
      End If

      results_table = Nothing

      If HttpContext.Current.Session.Item("localPreferences").DatabaseType <> eDatabaseTypes.MONTHLY Then

        results_table = New DataTable
        results_table = get_actual_average_days_on_market_info(searchCriteria)

        If Not IsNothing(results_table) Then

          If results_table.Rows.Count > 0 Then

            For Each r As DataRow In results_table.Rows

              If Not IsDBNull(r("daysOnMarket")) Then
                If CLng(r.Item("daysOnMarket").ToString) > 0 Then


                  daysonmarket += 1
                  daysonmarket2 += CLng(r.Item("daysOnMarket").ToString)

                End If
              End If


            Next


            If daysonmarket > 0 Then
              days = System.Math.Round(CLng(daysonmarket2) / CLng(daysonmarket))
            Else
              days = System.Math.Round(CLng(daysonmarket2))
            End If

            If Trim(graph_string) <> "" Then
              graph_string &= ", "
            End If
            graph_string += "['" & (Month(Date.Now) & "-" & Year(Date.Now)) & "'," & days & " ]"



            If high_number = 0 Or CDbl(days) > high_number Then
              high_number = CDbl(days)
            End If

            If low_number = 0 Or CDbl(days) < low_number Then
              low_number = CDbl(days)
            End If

            AVG_DAYS_ON.Series("AVG_DAYS").Points.AddXY((Date.Now.Month.ToString + "-" + Date.Now.Year.ToString), CDbl(days))
            AVG_DAYS_ON.Series("AVG_DAYS").Points(x).Color = Drawing.Color.Black
            AVG_DAYS_ON.Series("AVG_DAYS").Points(x).BorderDashStyle = DataVisualization.Charting.ChartDashStyle.Dash

          End If
        End If

      End If


      graph_string = " data100.addColumn('string', 'Month/Year'); data100.addColumn('number', 'Average'); data100.addRows([ " & graph_string


      If AVG_DAYS_ON.Series("AVG_DAYS").Points.Count < 1 Then
        AVG_DAYS_ON.Titles.Clear()
        AVG_DAYS_ON.Titles.Add("No Avg Days on Market")
      End If

      results_table = Nothing

      If low_number > 200 Then
        starting_point = (low_number / 200) - 1
        starting_point = starting_point * 200
      Else
        starting_point = 0
      End If

      If low_number < 400 Then
        interval_point = 100
      Else
        interval_point = 200
      End If

      AVG_DAYS_ON.ChartAreas("ChartArea1").AxisY.Maximum = high_number + 100
      AVG_DAYS_ON.ChartAreas("ChartArea1").AxisY.Minimum = starting_point
      AVG_DAYS_ON.ChartAreas("ChartArea1").AxisY.Interval = interval_point

    Catch ex As Exception

      aError = "Error in views_display_average_days_on_market_graph(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef AVG_DAYS_ON As DataVisualization.Charting.Chart, ByRef avg_days_on_market As Integer) " + ex.Message

    Finally

    End Try

  End Sub

  Public Function get_for_sale_by_month_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable
    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sSeperator As String = ""

    Try

      sQuery.Append("SELECT DISTINCT mtrend_year, mtrend_month, SUM(mtrend_total_aircraft_for_sale) AS mtrend_total_aircraft_for_sale")
      sQuery.Append(" FROM Aircraft_Model_Trend WITH(NOLOCK) INNER JOIN aircraft_model WITH(NOLOCK) ON mtrend_amod_id = amod_id")
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
        sSeperator = Constants.cAndClause
      ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append("amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
        sSeperator = Constants.cAndClause
      ElseIf searchCriteria.ViewCriteriaSecondAmodID > -1 Then
        sQuery.Append("amod_id = " + searchCriteria.ViewCriteriaSecondAmodID.ToString)
        sSeperator = Constants.cAndClause
      ElseIf searchCriteria.ViewCriteriaThirdAmodID > -1 Then
        sQuery.Append("amod_id = " + searchCriteria.ViewCriteriaThirdAmodID.ToString)
        sSeperator = Constants.cAndClause
      ElseIf Not IsNothing(searchCriteria.ViewCriteriaMakeIDArray) Then
        sQuery.Append("amod_make_name IN ('" + searchCriteria.ViewCriteriaAircraftMake.ToUpper.Replace(Constants.cCommaDelim, Constants.cValueSeperator).Trim + "')")
        sSeperator = Constants.cAndClause
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append("amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
        sSeperator = Constants.cAndClause
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftType.Trim) Then
        sQuery.Append("amod_type_code = '" + searchCriteria.ViewCriteriaAircraftType.Trim + "'")
        sSeperator = Constants.cAndClause
      End If

      Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
        Case Constants.VIEW_EXECUTIVE
          sQuery.Append(sSeperator + "amod_airframe_type_code='F' AND amod_type_code = 'E'")
          sSeperator = Constants.cAndClause
        Case Constants.VIEW_JETS
          sQuery.Append(sSeperator + "amod_airframe_type_code='F' AND amod_type_code = 'J'")
          sSeperator = Constants.cAndClause
        Case Constants.VIEW_TURBOPROPS
          sQuery.Append(sSeperator + "amod_airframe_type_code='F' AND amod_type_code = 'T'")
          sSeperator = Constants.cAndClause
        Case Constants.VIEW_PISTONS
          sQuery.Append(sSeperator + "amod_airframe_type_code='F' AND amod_type_code = 'P'")
          sSeperator = Constants.cAndClause
        Case Constants.VIEW_HELICOPTERS
          sQuery.Append(sSeperator + "amod_airframe_type_code='R' AND amod_type_code in ('T','P')")
          sSeperator = Constants.cAndClause
      End Select

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(sSeperator + commonEvo.MakeMarketProductCodeClause(HttpContext.Current.Session.Item("localPreferences"), True))
        sSeperator = Constants.cAndClause
      Else
        sQuery.Append(sSeperator + commonEvo.BuildMarketProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, True))
        sSeperator = Constants.cAndClause
      End If

      ' ADDED THIS BLOCK IN, MSW - TIMEFRAME WAS BAD----------------------
      '------------------------------------------------------------------- 
      sQuery.Append(make_mtrend_year_query_string(searchCriteria.ViewCriteriaTimeSpan))
            '-------------------------------------------------------------------
            '-------------------------------------------------------------------


            'sQuery.Append(sSeperator + "(((mtrend_year >= year(CONVERT(DATETIME, '" + DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now()).ToString + "',102)))")
            'sQuery.Append(Constants.cAndClause + "(mtrend_month >= month(CONVERT(DATETIME, '" + DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now()).ToString + "',102))))")


            'Dim sClause As String = ""
            'If Year(DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now())) <> Year(Now()) Then
            '  sClause = " OR "
            'Else
            '  sClause = " AND "
            'End If

            'sQuery.Append(sClause + "((mtrend_year = year(CONVERT(DATETIME, '" + Now.ToString + "',102)))" + Constants.cAndClause + "(mtrend_month <= month(CONVERT(DATETIME, '" + Now.ToString + "',102)))))")

            If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaWeightClass.Trim) Then
                If searchCriteria.ViewCriteriaWeightClass.Contains(",") Then
                    sQuery.Append(Constants.cAndClause + "amod_weight_class IN ('" + searchCriteria.ViewCriteriaWeightClass.ToUpper.Replace(Constants.cCommaDelim, Constants.cValueSeperator).Trim + "')")
                Else
                    sQuery.Append(Constants.cAndClause + "amod_weight_class = '" + searchCriteria.ViewCriteriaWeightClass.ToUpper.Trim + "'")
                End If
            End If






            If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
                sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True))
            Else
                sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
            End If


            If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
                sQuery.Append("   And mtrend_product_type = ")
                sQuery.Append("( ")
                sQuery.Append("  Select top  1 ")
                sQuery.Append("    Case  ")
                sQuery.Append("  when a2.amod_product_business_flag = 'Y' then 'B' ")
                sQuery.Append("  when a2.amod_product_commercial_flag  = 'Y' then 'C' ")
                sQuery.Append("   when a2.amod_product_helicopter_flag  = 'Y' then 'H' ")
                sQuery.Append("  Else 'B' ")
                sQuery.Append("   End ")
                sQuery.Append("    From aircraft_model a2 with (NOLOCK) ")
                sQuery.Append(" Where a2.amod_id = aircraft_model.amod_id ")
                sQuery.Append(") ")
            ElseIf HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True And HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True Then
                sQuery.Append("   And mtrend_product_type = ")
                sQuery.Append("( ")
                sQuery.Append("  Select top  1 ")
                sQuery.Append("    Case  ")
                sQuery.Append("  when a2.amod_product_business_flag = 'Y' then 'B' ")
                sQuery.Append("  when a2.amod_product_commercial_flag  = 'Y' then 'C' ")
                sQuery.Append("   when a2.amod_product_helicopter_flag  = 'Y' then 'H' ")
                sQuery.Append("  Else 'B' ")
                sQuery.Append("   End ")
                sQuery.Append("    From aircraft_model a2 with (NOLOCK) ")
                sQuery.Append(" Where a2.amod_id = aircraft_model.amod_id ")
                sQuery.Append(") ")
            ElseIf HttpContext.Current.Session.Item("localSubscription").crmCommercial_Flag = True Then
                sQuery.Append("   And mtrend_product_type = 'C' ")
            ElseIf HttpContext.Current.Session.Item("localSubscription").crmBusiness_Flag = True Then
                sQuery.Append("   And mtrend_product_type = 'B' ")
            ElseIf HttpContext.Current.Session.Item("localSubscription").crmHelicopter_Flag = True Then
                sQuery.Append("   And mtrend_product_type = 'H' ")
            End If

            sQuery.Append(" GROUP BY mtrend_year, mtrend_month ORDER BY mtrend_year ASC, mtrend_month ASC")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_for_sale_by_month_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_for_sale_by_month_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_for_sale_by_month_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable" + ex.Message

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

  Public Function get_actual_for_sale_by_month_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable
    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Try

      sQuery.Append("Select count(distinct ac_id) As tcount FROM aircraft_flat With(NOLOCK)")
      sQuery.Append(" WHERE ac_forsale_flag = 'Y' AND ac_journ_id = 0")

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
        sQuery.Append(Constants.cAndClause + "amod_type_code = '" + searchCriteria.ViewCriteriaAircraftType.Trim + "'")
      End If

      Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
        Case Constants.VIEW_EXECUTIVE
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'E'")
        Case Constants.VIEW_JETS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'J'")
        Case Constants.VIEW_TURBOPROPS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'T'")
        Case Constants.VIEW_PISTONS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'P'")
        Case Constants.VIEW_HELICOPTERS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='R' AND amod_type_code in ('T','P')")
      End Select

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaWeightClass.Trim) Then
        If searchCriteria.ViewCriteriaWeightClass.Contains(",") Then
          sQuery.Append(Constants.cAndClause + "amod_weight_class IN ('" + searchCriteria.ViewCriteriaWeightClass.ToUpper.Replace(Constants.cCommaDelim, Constants.cValueSeperator).Trim + "')")
        Else
          sQuery.Append(Constants.cAndClause + "amod_weight_class = '" + searchCriteria.ViewCriteriaWeightClass.ToUpper.Trim + "'")
        End If
      End If

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True))
      Else
        sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_actual_for_sale_by_month_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_actual_for_sale_by_month_info load datatable" + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_actual_for_sale_by_month_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable" + ex.Message

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

  Public Sub views_display_for_sale_by_month_graph(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef FOR_SALE As DataVisualization.Charting.Chart, Optional ByVal make_larger_no_solds As Boolean = False, Optional ByRef graph_string As String = "", Optional ByVal values_for_sale As Long = 0, Optional ByRef months_string As String = "", Optional ByRef for_sale_string As String = "")

    Dim high_number As Double = 0.0
    Dim low_number As Double = 0.0
    Dim starting_point As Integer = 0
    Dim ending_point As Integer = 0
    Dim interval_point As Integer = 1 

    Dim results_table As New DataTable
    Dim x As Integer = 0

    Try

      FOR_SALE.Series.Clear()
      FOR_SALE.Series.Add("FOR_SALE")
      FOR_SALE.Series("FOR_SALE").ChartType = UI.DataVisualization.Charting.SeriesChartType.Spline
      FOR_SALE.Series("FOR_SALE").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
      FOR_SALE.ChartAreas("ChartArea1").AxisY.Title = "Aircraft For Sale"
      FOR_SALE.Series("FOR_SALE").Color = Drawing.Color.Blue
      FOR_SALE.Series("FOR_SALE").BorderWidth = 1
      FOR_SALE.Series("FOR_SALE").MarkerSize = 5
      FOR_SALE.Series("FOR_SALE").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle
      FOR_SALE.Series("FOR_SALE").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32

      If make_larger_no_solds = True Then
        FOR_SALE.Width = 400
        FOR_SALE.Height = 350
      Else
        FOR_SALE.Width = 300
        FOR_SALE.Height = 300
      End If


      results_table = get_for_sale_by_month_info(searchCriteria)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          For Each r As DataRow In results_table.Rows
            If Not IsDBNull(r("mtrend_year")) Then
              If Not String.IsNullOrEmpty(r.Item("mtrend_year").ToString.Trim) Then

                If Not IsDBNull(r("mtrend_month")) Then
                  If Not String.IsNullOrEmpty(r.Item("mtrend_month").ToString.Trim) Then

                    If Not IsDBNull(r("mtrend_total_aircraft_for_sale")) Then
                      If CDbl(r.Item("mtrend_total_aircraft_for_sale").ToString) > 0 Then

                        If high_number = 0 Or CDbl(r.Item("mtrend_total_aircraft_for_sale").ToString) > high_number Then
                          high_number = CDbl(r.Item("mtrend_total_aircraft_for_sale").ToString)
                        End If

                        If low_number = 0 Or CDbl(r.Item("mtrend_total_aircraft_for_sale")) < low_number Then
                          low_number = CDbl(r.Item("mtrend_total_aircraft_for_sale").ToString)
                        End If

                        FOR_SALE.Series("FOR_SALE").Points.AddXY((r.Item("mtrend_month").ToString + "-" + r.Item("mtrend_year").ToString), CDbl(r.Item("mtrend_total_aircraft_for_sale").ToString))

                        If Trim(graph_string) <> "" Then
                          graph_string &= ", "
                        End If
                        graph_string += "['" & (r.Item("mtrend_month") & "-" & r.Item("mtrend_year")) & "'," & r.Item("mtrend_total_aircraft_for_sale") & " ]"

                        If Trim(months_string) <> "" Then
                          months_string &= "," & r.Item("mtrend_month") & "-" & r.Item("mtrend_year")
                        Else
                          months_string = r.Item("mtrend_month") & "-" & r.Item("mtrend_year")
                        End If

                        If Trim(for_sale_string) <> "" Then
                          for_sale_string &= "," & r.Item("mtrend_total_aircraft_for_sale")
                        Else
                          for_sale_string = r.Item("mtrend_total_aircraft_for_sale")
                        End If

                        x += 1

                      End If
                    End If

                  End If
                End If
              End If
            End If

          Next

        End If
      End If

      results_table = Nothing
      results_table = New DataTable

      If values_for_sale <> 0 Then

        If Trim(graph_string) <> "" Then
          graph_string &= ", "
        End If
        graph_string += "['" & (Month(Date.Now()) & "-" & Year(Date.Now())) & "'," & values_for_sale & " ]"

      Else

        If HttpContext.Current.Session.Item("localPreferences").DatabaseType <> eDatabaseTypes.MONTHLY Then

          results_table = get_actual_for_sale_by_month_info(searchCriteria)

          If Not IsNothing(results_table) Then

            If results_table.Rows.Count > 0 Then

              For Each r As DataRow In results_table.Rows

                If high_number = 0 Or CDbl(r.Item("tcount").ToString) > high_number Then
                  high_number = CDbl(r.Item("tcount").ToString)
                End If

                If low_number = 0 Or CDbl(r.Item("tcount").ToString) < low_number Then
                  low_number = CDbl(r.Item("tcount").ToString)
                End If

                If CDbl(r.Item("tcount").ToString) > 0 Then
                  FOR_SALE.Series("FOR_SALE").Points.AddXY((Date.Now.Month.ToString + "-" + Date.Now.Year().ToString), CDbl(r.Item("tcount").ToString))
                  FOR_SALE.Series("FOR_SALE").Points(x).Color = Drawing.Color.Black
                  FOR_SALE.Series("FOR_SALE").Points(x).BorderDashStyle = DataVisualization.Charting.ChartDashStyle.Dash
                End If

                If Trim(graph_string) <> "" Then
                  graph_string &= ", "
                End If
                graph_string += "['" & (Month(Date.Now()) & "-" & Year(Date.Now())) & "'," & r.Item("tcount") & " ]"

                If Trim(months_string) <> "" Then
                  months_string = "," & r.Item("mtrend_month") & "-" & r.Item("mtrend_year")
                Else
                  months_string = r.Item("mtrend_month") & "-" & r.Item("mtrend_year")
                End If

                If Trim(for_sale_string) <> "" Then
                  for_sale_string = "," & r.Item("mtrend_total_aircraft_for_sale")
                Else
                  for_sale_string = r.Item("mtrend_total_aircraft_for_sale")
                End If 
              Next

            End If
          End If
        End If


        If FOR_SALE.Series("FOR_SALE").Points.Count < 1 Then
          FOR_SALE.Titles.Clear()
          FOR_SALE.Titles.Add("No Aircraft For Sale")
        End If
      End If


      graph_string = " data16.addColumn('string', 'Month/Year'); data16.addColumn('number', 'Average'); data16.addRows([ " & graph_string



      results_table = Nothing

      commonEvo.set_ranges_for_vsCharts(low_number, high_number, interval_point, starting_point, ending_point)

      FOR_SALE.ChartAreas("ChartArea1").AxisY.Maximum = ending_point
      FOR_SALE.ChartAreas("ChartArea1").AxisY.Minimum = IIf(starting_point > 0, starting_point, 0)
      FOR_SALE.ChartAreas("ChartArea1").AxisY.Interval = interval_point

    Catch ex As Exception

      aError = "Error in views_display_for_sale_by_month_graph(ByVal inModelID As Long, ByRef FOR_SALE As DataVisualization.Charting.Chart) " + ex.Message

    Finally

    End Try

  End Sub



    Public Sub views_display_fleet_market_summary(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_Build_FleetMarketSummary_text As String, ByRef out_GetMarketStatus As String, Optional ByRef out_AdminMarketStatus As String = "", Optional ByVal number_of_months_divide As Integer = 6, Optional ByRef values_for_sale As Long = 0, Optional ByRef values_dom As Long = 0, Optional ByRef values_avg_asking As Long = 0, Optional ByRef values_in_op As Double = 0, Optional ByRef values_total_inop As Long = 0, Optional ByRef ac_exclusive_sale As Integer = 0, Optional ByRef ac_lease As Integer = 0, Optional ByRef absorp_rate As Double = 0.0, Optional ByRef per As Double = 0, Optional ByRef per2 As Double = 0, Optional ByRef per3 As Double = 0, Optional ByRef forsaleavghigh As Double = 0.0, Optional ByRef forsaleavlow As Double = 0.0, Optional ByRef values_mfr_avg_low As Integer = 0, Optional ByRef values_mfr_avg_high As Integer = 0, Optional ByRef values_mfr_avg As Integer = 0, Optional ByRef daysonmarket_low As Integer = 0, Optional ByRef daysonmarket_high As Integer = 0, Optional ByRef days_avg As Integer = 0, Optional ByRef values_aftt_low As Long = 0, Optional ByRef values_aftt_high As Long = 0, Optional ByRef values_aftt_avg As Long = 0, Optional ByRef values_avg_asking_display As Double = 0, Optional ByRef ModelImage As String = "", Optional ByRef ac_id_array As Array = Nothing, Optional ByRef ac_asking_array As Array = Nothing, Optional ByVal array_count As Integer = 0, Optional ByRef landings_high As Double = 0, Optional ByRef landings_low As Double = 0, Optional ByRef landings_avg As Double = 0, Optional ByRef landings_sum As Double = 0, Optional ByRef landings_count As Long = 0)
        Dim bHasMaster As Boolean = True
        Dim fleetHtmlOut As New StringBuilder
        Dim marketHtmlOut As New StringBuilder

        Dim results_table As New DataTable

        Dim string_for_op_percentage = ""

        Dim avgyear As Integer = 0
        Dim avgyearcount As Integer = 0

        Dim totalcount As Integer = 0
        Dim totalInOpcount As Integer = 0
        Dim ac_for_sale As Integer = 0
        'Dim ac_exclusive_sale As Integer = 0
        ' Dim ac_lease As Integer = 0

        Dim w_owner As Integer = 0
        Dim s_owner As Integer = 0
        Dim f_owner As Integer = 0
        Dim o_stage As Integer = 0
        Dim t_stage As Integer = 0
        Dim th_stage As Integer = 0
        Dim f_stage As Integer = 0
        Dim values_mfr_count As Integer = 0
        Dim values_aftt_count As Long = 0
        'Dim daysonmarket As Integer = 0
        'Dim daysonmarket2 As Integer = 0
        'Dim days As Integer = 0

        Dim allhigh As Integer = 0
        Dim alllow As Integer = 0
        Dim all_aftt_low As Long = 0
        Dim all_aftt_high As Long = 0
        Dim us_reg As Integer = 0
        'Dim forsaleavghigh As Double = 0.0
        'Dim forsaleavlow As Double = 0.0

        'Dim per As Double = 0
        'Dim per2 As Double = 0
        'Dim per3 As Double = 0

        Dim sRefLink As String = ""
        Dim sAirFrame As String = ""
        Dim sAirType As String = ""
        Dim sMake As String = ""
        Dim sModel As String = ""
        Dim sUsage As String = ""
        ' Dim absorp_rate As Double = 0.0
        Dim w_asking_count As Integer = 0
        Dim total_days As Integer = 0
        Dim days_total_sum As Integer = 0
        Dim k As Integer = 0
        Dim client_asking As Long = 0

        Try

            If daysonmarket_low = 0 Then
                daysonmarket_low = 10000
            End If


            If Not IsNothing(HttpContext.Current.Request("noMaster")) Then
                If Not String.IsNullOrEmpty(HttpContext.Current.Request("noMaster")) Then
                    If Trim(HttpContext.Current.Request("noMaster")) = "false" Then
                        bHasMaster = False ' This otherwise defaults to false. All this means is that if this function is called from the view (that's seperately popped up in it's own window with the noMaster variable) that the 
                        'window closes and then parseForm runs. So you can tell the search is being performed.
                    End If
                End If
            End If

            results_table = commonEvo.get_fleet_market_summary_info(searchCriteria, IIf(searchCriteria.ViewID = 10, True, False), number_of_months_divide)

            absorp_rate = 0
            If Not IsNothing(results_table) Then
                If results_table.Rows.Count > 0 Then

                    For Each r As DataRow In results_table.Rows

                        If Not IsDBNull(r("daysonmarket")) Then
                            If CLng(r.Item("daysonmarket").ToString) > 0 Then
                                total_days += 1
                                ' daysonmarket2 += CLng(r.Item("daysonmarket").ToString)
                                ' changed msw - 12/18/18

                                days_total_sum = days_total_sum + CLng(r.Item("daysonmarket").ToString)

                                If CLng(r.Item("daysonmarket").ToString) > daysonmarket_high Then
                                    daysonmarket_high = CLng(r.Item("daysonmarket").ToString)
                                End If

                                If CLng(r.Item("daysonmarket").ToString) < daysonmarket_low Then
                                    daysonmarket_low = CLng(r.Item("daysonmarket").ToString)
                                End If


                            End If
                        End If


                        If Not IsDBNull(r("ac_airframe_tot_landings")) Then
                            If IsNumeric(r("ac_airframe_tot_landings").ToString) Then

                                If CInt(r("ac_airframe_tot_landings").ToString) > 0 Then

                                    If landings_high = 0 Or CInt(r.Item("ac_airframe_tot_landings").ToString) > landings_high Then
                                        landings_high = CInt(r.Item("ac_airframe_tot_landings").ToString)
                                    End If

                                    If landings_low = 0 Or CInt(r.Item("ac_airframe_tot_landings").ToString) < landings_low Then
                                        landings_low = CInt(r.Item("ac_airframe_tot_landings").ToString)
                                    End If

                                    landings_sum += CInt(r.Item("ac_airframe_tot_landings").ToString)

                                    landings_count += 1
                                End If
                            End If
                        End If


                        If Not IsDBNull(r("ac_mfr_year")) Then
                            If IsNumeric(r("ac_mfr_year").ToString) Then

                                If CInt(r("ac_mfr_year").ToString) > 0 Then

                                    If allhigh = 0 Or CInt(r.Item("ac_mfr_year").ToString) > allhigh Then
                                        allhigh = CInt(r.Item("ac_mfr_year").ToString)
                                    End If

                                    If alllow = 0 Or CInt(r.Item("ac_mfr_year").ToString) < alllow Then
                                        alllow = CInt(r.Item("ac_mfr_year").ToString)
                                    End If

                                    avgyear += CInt(r.Item("ac_mfr_year").ToString)
                                    avgyearcount += 1

                                End If
                            End If
                        End If



                        totalcount += 1

                        If r.Item("ac_lifecycle_stage").ToString = "3" And (r.Item("ac_ownership_type").ToString.ToUpper = "S" Or r.Item("ac_ownership_type").ToString.ToUpper = "F" Or r.Item("ac_ownership_type").ToString.ToUpper = "W") Then
                            totalInOpcount += 1
                        End If

                        If r.Item("ac_ownership_type").ToString.ToUpper = "W" And r.Item("ac_lifecycle_stage").ToString = "3" Then
                            w_owner += 1
                        End If

                        If r.Item("ac_ownership_type").ToString.ToUpper = "F" And r.Item("ac_lifecycle_stage").ToString = "3" Then
                            f_owner += 1
                        End If

                        If r.Item("ac_ownership_type").ToString.ToUpper = "S" And r.Item("ac_lifecycle_stage").ToString = "3" Then
                            s_owner += 1
                        End If

                        If r.Item("ac_lifecycle_stage").ToString = "1" Then
                            o_stage += 1
                        End If

                        If r.Item("ac_lifecycle_stage").ToString = "2" Then
                            t_stage += 1
                        End If

                        If r.Item("ac_lifecycle_stage").ToString = "3" And (r.Item("ac_ownership_type").ToString.ToUpper = "S" Or r.Item("ac_ownership_type").ToString.ToUpper = "F" Or r.Item("ac_ownership_type").ToString.ToUpper = "W") Then
                            th_stage += 1
                        End If

                        If r.Item("ac_lifecycle_stage").ToString = "4" Then
                            f_stage += 1
                        End If


                        If Not IsDBNull(r("ac_airframe_tot_Hrs")) Then
                            If IsNumeric(r("ac_airframe_tot_Hrs")) Then
                                If CInt(r("ac_airframe_tot_Hrs").ToString) > 0 Then
                                    If CInt(r("ac_airframe_tot_Hrs")) > CInt(all_aftt_high) Then
                                        all_aftt_high = CInt(r("ac_airframe_tot_Hrs"))
                                    End If

                                    If CInt(r("ac_airframe_tot_Hrs")) < CInt(all_aftt_low) Or all_aftt_low = 0 Then
                                        all_aftt_low = CInt(r("ac_airframe_tot_Hrs"))
                                    End If
                                End If
                            End If
                        End If

                        If r("ac_lifecycle_stage") = "3" Then
                            If Not IsDBNull(r("ac_country_of_registration")) Then
                                If Trim(r("ac_country_of_registration")) = "United States" Then
                                    us_reg = us_reg + 1
                                End If
                            End If
                        End If

                        client_asking = 0
                        If HttpContext.Current.Session.Item("localUser").crmUser_Evo_MPM_Flag = True Then
                            If array_count > 0 Then
                                For k = 0 To array_count - 1
                                    If ac_asking_array(k) > 0 And ac_id_array(k) = r("ac_id") Then
                                        client_asking = ac_asking_array(k)
                                    End If
                                    k = array_count
                                Next
                            End If
                        End If



                        If client_asking > 0 Then
                            ac_for_sale += 1
                            If forsaleavghigh = 0 Or CDbl(client_asking) > forsaleavghigh Then
                                forsaleavghigh = CDbl(client_asking)
                            End If

                            If forsaleavlow = 0 Or (CDbl(client_asking) < forsaleavlow) Then
                                forsaleavlow = CDbl(client_asking)
                            End If


                            values_avg_asking = values_avg_asking + CDbl(client_asking)
                            w_asking_count = w_asking_count + 1

                        ElseIf r.Item("ac_forsale_flag").ToString.ToUpper = "Y" Then

                            ac_for_sale += 1

                            If Not IsDBNull(r("ac_asking_price")) Then
                                If Not String.IsNullOrEmpty(r.Item("ac_asking_price").ToString) Then

                                    If CDbl(r.Item("ac_asking_price").ToString) > 0 Then

                                        If forsaleavghigh = 0 Or CDbl(r.Item("ac_asking_price").ToString) > forsaleavghigh Then
                                            forsaleavghigh = CDbl(r.Item("ac_asking_price").ToString)
                                        End If

                                        If forsaleavlow = 0 Or (CDbl(r.Item("ac_asking_price").ToString) < forsaleavlow) Then
                                            forsaleavlow = CDbl(r.Item("ac_asking_price").ToString)
                                        End If

                                        values_avg_asking = values_avg_asking + CDbl(r.Item("ac_asking_price").ToString)
                                        w_asking_count = w_asking_count + 1
                                    End If

                                End If
                            End If
                        End If


                        If r.Item("ac_forsale_flag").ToString.ToUpper = "Y" Then

                            If Not IsDBNull(r("ac_mfr_year")) Then
                                If IsNumeric(r("ac_mfr_year")) Then
                                    If CInt(r("ac_mfr_year").ToString) > 0 Then
                                        If CInt(r("ac_mfr_year")) > CInt(values_mfr_avg_high) Then
                                            values_mfr_avg_high = CInt(r("ac_mfr_year"))
                                        End If

                                        If CInt(r("ac_mfr_year")) < CInt(values_mfr_avg_low) Or values_mfr_avg_low = 0 Then
                                            values_mfr_avg_low = CInt(r("ac_mfr_year"))
                                        End If

                                        values_mfr_avg = values_mfr_avg + CInt(r("ac_mfr_year"))
                                        values_mfr_count = values_mfr_count + 1
                                    End If
                                End If
                            End If

                            If Not IsDBNull(r("ac_airframe_tot_Hrs")) Then
                                If IsNumeric(r("ac_airframe_tot_Hrs")) Then
                                    If CInt(r("ac_airframe_tot_Hrs").ToString) > 0 Then
                                        If CInt(r("ac_airframe_tot_Hrs")) > CInt(values_aftt_high) Then
                                            values_aftt_high = CInt(r("ac_airframe_tot_Hrs"))
                                        End If

                                        If CInt(r("ac_airframe_tot_Hrs")) < CInt(values_aftt_low) Or values_aftt_low = 0 Then
                                            values_aftt_low = CInt(r("ac_airframe_tot_Hrs"))
                                        End If

                                        values_aftt_avg = values_aftt_avg + CInt(r("ac_airframe_tot_Hrs"))
                                        values_aftt_count = values_aftt_count + 1
                                    End If
                                End If
                            End If
                        End If


                        If Not IsDBNull(r("SalesPerTimeframe")) Then
                            If IsNumeric(r("SalesPerTimeframe").ToString) Then
                                If r("SalesPerTimeframe") > 0 Then
                                    absorp_rate = r("SalesPerTimeframe")
                                End If
                            End If
                        End If

                        If Not IsDBNull(r("ac_exclusive_flag")) Then
                            If r.Item("ac_exclusive_flag").ToString.ToUpper = "Y" Then
                                ac_exclusive_sale += 1
                            End If
                        End If

                        If Not IsDBNull(r("ac_lease_flag")) Then
                            If r.Item("ac_lease_flag").ToString.ToUpper = "Y" Then
                                ac_lease += 1
                            End If
                        End If

                    Next

                End If
            End If



            If landings_sum > 0 Then
                landings_avg = (landings_sum / landings_count)
            End If


            If (forsaleavlow > 0) Then
                forsaleavlow = CDbl(forsaleavlow / 1000)
            End If

            If (forsaleavghigh > 0) Then
                forsaleavghigh = CDbl(forsaleavghigh / 1000)
            End If

            If (ac_for_sale > 0 And th_stage > 0) Then

                per = System.Math.Round(CDbl(ac_for_sale / th_stage * 100), 1)
                per2 = System.Math.Round(CDbl(ac_exclusive_sale / ac_for_sale * 100), 1)
                per3 = System.Math.Round(CDbl(ac_lease / th_stage * 100), 1)

                If total_days > 0 Then
                    days_avg = System.Math.Round(CLng(days_total_sum) / CLng(total_days))
                End If

            End If

            'If (alllow >= 0 And allhigh > 0) Then
            '  For i As Integer = alllow To allhigh
            '    avgyear += i
            '    avgyearcount += 1
            '  Next
            'End If

            If avgyear > 0 And avgyearcount > 0 Then
                avgyear = CLng(avgyear / avgyearcount)
            End If

            If values_aftt_count > 0 Then
                values_aftt_avg = CDbl(values_aftt_avg / values_aftt_count)
            End If

            If values_mfr_count > 0 Then
                values_mfr_avg = CDbl(values_mfr_avg / values_mfr_count)
            End If

            Dim nTmpIndex As Long = -1
            Dim nTmpModelID As Long = -1

            If searchCriteria.ViewCriteriaAmodID > -1 Then
                nTmpIndex = commonEvo.FindIndexForItemByAmodID(searchCriteria.ViewCriteriaAmodID)
                commonEvo.ReturnModelDataFromIndex(nTmpIndex, sAirFrame, sAirType, sMake, sModel, sUsage)
                nTmpModelID = searchCriteria.ViewCriteriaAmodID
            ElseIf searchCriteria.ViewCriteriaSecondAmodID > -1 Then
                nTmpIndex = commonEvo.FindIndexForItemByAmodID(searchCriteria.ViewCriteriaSecondAmodID)
                commonEvo.ReturnModelDataFromIndex(nTmpIndex, sAirFrame, sAirType, sMake, sModel, sUsage)
                nTmpModelID = searchCriteria.ViewCriteriaSecondAmodID
            ElseIf searchCriteria.ViewCriteriaThirdAmodID > -1 Then
                nTmpIndex = commonEvo.FindIndexForItemByAmodID(searchCriteria.ViewCriteriaThirdAmodID)
                commonEvo.ReturnModelDataFromIndex(nTmpIndex, sAirFrame, sAirType, sMake, sModel, sUsage)
                nTmpModelID = searchCriteria.ViewCriteriaThirdAmodID
            End If

            string_for_op_percentage = "&nbsp;<span class='tiny'>(" + FormatNumber(per, 1, TriState.False, TriState.False, TriState.True).ToString + "% of In Operation)</span>"

            If searchCriteria.ViewID <> 1 And searchCriteria.ViewID <> 11 Then
                ' start outer table
                fleetHtmlOut.Append("<table id='fleetTable' cellpadding='2' cellspacing='0' width='100%'" + IIf(HttpContext.Current.Session.Item("lastView") <> 16, " class='module'", "") + ">")
                fleetHtmlOut.Append("<tr>")

                ' Ownership table
                fleetHtmlOut.Append("<td align='right' valign='top' class='FleetMarket_Left_TD' width='50%'><table id='ownershipTable' cellspacing='0' cellpadding='2' width='100%' class='sub_table'>")
                fleetHtmlOut.Append("<tr class='aircraft_list'><td valign='middle' align='center' colspan='2'><strong>" + IIf(searchCriteria.ViewID = 10, "Charter", "") + "&nbsp;Ownership&nbsp;(In&nbsp;Operation)&nbsp;</strong></td></tr>")

                If w_owner > 0 Then

                    If nTmpModelID > -1 And searchCriteria.ViewID <> 10 Then
                        sRefLink = "<a class='underline cursor' onclick=""javascript:ParseForm('0',false,false,false,false,false,'cboAircraftTypeID=" + sAirType + Constants.cSvrDataSeperator + sAirFrame
                        sRefLink += "!~!cboAircraftMakeID=" + sMake.ToUpper.Trim
                        sRefLink += "!~!cboAircraftModelID=" + nTmpModelID.ToString
                        sRefLink += "!~!ac_ownership_type=W!~!clearSelection=true');"" title=""Click to view aircraft list"">"
                        sRefLink += FormatNumber(w_owner, 0, TriState.False, TriState.False, TriState.True).ToString + "</a>"
                    Else
                        sRefLink = FormatNumber(w_owner, 0, TriState.False, TriState.False, TriState.True).ToString
                    End If

                    fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left'><span class=""mobileLabel"">Whole:&nbsp;</span></td><td align='right'><span class=""mobileAnswer"">" + sRefLink.Trim + "</span></td></tr>")
                Else
                    fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left'><span class=""mobileLabel"">Whole:&nbsp;</span></td><td align='right'><span class=""mobileAnswer"">&nbsp;0</span></td></tr>")
                End If

                If s_owner > 0 Then

                    If nTmpModelID > -1 And searchCriteria.ViewID <> 10 Then
                        sRefLink = "<a class='underline cursor' onclick=""javascript:ParseForm('0',false,false,false,false,false,'cboAircraftTypeID=" + sAirType + Constants.cSvrDataSeperator + sAirFrame
                        sRefLink += "!~!cboAircraftMakeID=" + sMake.ToUpper.Trim
                        sRefLink += "!~!cboAircraftModelID=" + nTmpModelID.ToString
                        sRefLink += "!~!ac_ownership_type=S!~!clearSelection=true');"" title=""Click to view aircraft list"">"
                        sRefLink += FormatNumber(s_owner, 0, TriState.False, TriState.False, TriState.True).ToString + "</a>"
                    Else
                        sRefLink = FormatNumber(s_owner, 0, TriState.False, TriState.False, TriState.True).ToString
                    End If

                    fleetHtmlOut.Append("<tr><td valign='top' align='left'><span class=""mobileLabel"">Shared:&nbsp;</span></td><td align='right'><span class=""mobileAnswer"">&nbsp;" + sRefLink.Trim + "</span></td></tr>")
                Else
                    fleetHtmlOut.Append("<tr><td valign='top' align='left'><span class=""mobileLabel"">Shared:&nbsp;</span></td><td align='right'><span class=""mobileAnswer"">&nbsp;0</span></td></tr>")
                End If

                If f_owner > 0 Then
                    If nTmpModelID > -1 And searchCriteria.ViewID <> 10 Then
                        sRefLink = "<a class='underline cursor' onclick=""javascript:ParseForm('0',false,false,false,false,false,'cboAircraftTypeID=" + sAirType + Constants.cSvrDataSeperator + sAirFrame
                        sRefLink += "!~!cboAircraftMakeID=" + sMake.ToUpper.Trim
                        sRefLink += "!~!cboAircraftModelID=" + nTmpModelID.ToString
                        sRefLink += "!~!ac_ownership_type=F!~!clearSelection=true');"" title=""Click to view aircraft list"">"
                        sRefLink += FormatNumber(f_owner, 0, TriState.False, TriState.False, TriState.True).ToString + "</a>"
                    Else
                        sRefLink = FormatNumber(f_owner, 0, TriState.False, TriState.False, TriState.True).ToString
                    End If

                    fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left'><span class=""mobileLabel"">Fractional:&nbsp;</span></td><td align='right'><span class=""mobileAnswer"">&nbsp;" + sRefLink.Trim + "</span></td></tr>")
                Else
                    fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left'><span class=""mobileLabel"">Fractional:&nbsp;</span></td><td align='right'><span class=""mobileAnswer"">&nbsp;0</span></td></tr>")
                End If

                If totalInOpcount > 0 Then
                    If nTmpModelID > -1 And searchCriteria.ViewID <> 10 Then
                        sRefLink = "<a class='underline cursor' onclick=""javascript:ParseForm('0',false,false,false,false,false,'cboAircraftTypeID=" + sAirType + Constants.cSvrDataSeperator + sAirFrame
                        sRefLink += "!~!cboAircraftMakeID=" + sMake.ToUpper.Trim
                        sRefLink += "!~!cboAircraftModelID=" + nTmpModelID.ToString
                        sRefLink += "!~!ac_lifecycle_stage=3!~!ac_ownership_type=W,F,S!~!clearSelection=true');"" title=""Click to view aircraft list"">"
                        sRefLink += FormatNumber(totalInOpcount, 0, TriState.False, TriState.False, TriState.True).ToString + "</a>"
                    Else
                        sRefLink = FormatNumber(totalInOpcount, 0, TriState.False, TriState.False, TriState.True).ToString
                    End If
                    fleetHtmlOut.Append("<tr><td valign='top' align='left' nowrap='nowrap'><span class=""mobileLabel"">Total Aircraft:&nbsp;</span></td><td align='right'><span class=""mobileAnswer"">&nbsp;" + sRefLink.Trim + "</span></td></tr>")
                Else
                    fleetHtmlOut.Append("<tr><td valign='top' align='left' nowrap='nowrap'><span class=""mobileLabel"">Total Aircraft:&nbsp;</span></td><td align='right'><span class=""mobileAnswer"">&nbsp;0</span></td></tr>")
                End If

                If (alllow > 0) And (allhigh > 0) And (allhigh <> CInt(Now().Year)) Then
                    fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='center' class='border_bottom' colspan='2' nowrap='nowrap'><span class=""mobileLabel"">MFR Year Range</span> <span class=""mobileAnswer"">" + alllow.ToString + " - " + allhigh.ToString + "</span></td></tr>")
                ElseIf (alllow > 0) And (allhigh = CInt(Now().Year)) Then
                    fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='center' class='border_bottom' colspan='2' nowrap='nowrap'><span class=""mobileLabel"">MFR Year Range</span> <span class=""mobileAnswer"">" + alllow.ToString + " - To Present</span></td></tr>")
                Else
                    fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='center' class='border_bottom' colspan='2' nowrap='nowrap'><span class=""mobileLabel"">MFR Year Range</span>&nbsp;:&nbsp;<span class=""mobileAnswer"">N/A</span></td></tr>")
                End If

                fleetHtmlOut.Append("</table>")

                ' Fleet Info
                fleetHtmlOut.Append("</td><td align='left' width='50%' valign='top'>")

            End If
            If searchCriteria.ViewID <> 10 And searchCriteria.ViewID <> 1 And searchCriteria.ViewID <> 11 Then


                fleetHtmlOut.Append("<table id='lifeCycleTable' width='100%' cellspacing='0' cellpadding='2' class='sub_table'>")
                fleetHtmlOut.Append("<tr class='aircraft_list'><td valign='top' align='center' colspan='2'><strong>" & IIf(HttpContext.Current.Session.Item("isMobile"), "&nbsp;", "") & "Fleet By Life Cycle</strong></td></tr>")

                If o_stage > 0 Then
                    If nTmpModelID > -1 Then
                        sRefLink = "<a class='underline cursor' onclick=""javascript:ParseForm('0',false,false,false,false,false,'cboAircraftTypeID=" + sAirType + Constants.cSvrDataSeperator + sAirFrame
                        sRefLink += "!~!cboAircraftMakeID=" + sMake.ToUpper.Trim
                        sRefLink += "!~!cboAircraftModelID=" + nTmpModelID.ToString
                        sRefLink += "!~!ac_lifecycle_stage=1!~!clearSelection=true');"" title=""Click to view aircraft list"">"
                        sRefLink += FormatNumber(o_stage, 0, TriState.False, TriState.False, TriState.True).ToString + "</a>"
                    Else
                        sRefLink = FormatNumber(o_stage, 0, TriState.False, TriState.False, TriState.True).ToString
                    End If
                    fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' nowrap='nowrap'><span class=""mobileLabel"">In Production:&nbsp;</span></td><td align='right'><span class=""mobileAnswer"">&nbsp;" + sRefLink.Trim + "</span></td></tr>")
                Else
                    fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' nowrap='nowrap'><span class=""mobileLabel"">In Production:&nbsp;</span></td><td align='right'><span class=""mobileAnswer"">&nbsp;0</span></td></tr>")
                End If

                If t_stage > 0 Then
                    If nTmpModelID > -1 Then
                        sRefLink = "<a class='underline cursor' onclick=""javascript:ParseForm('0',false,false,false,false,false,'cboAircraftTypeID=" + sAirType + Constants.cSvrDataSeperator + sAirFrame
                        sRefLink += "!~!cboAircraftMakeID=" + sMake.ToUpper.Trim
                        sRefLink += "!~!cboAircraftModelID=" + nTmpModelID.ToString
                        sRefLink += "!~!ac_lifecycle_stage=2!~!clearSelection=true');"" title=""Click to view aircraft list"">"
                        sRefLink += FormatNumber(t_stage, 0, TriState.False, TriState.False, TriState.True).ToString + "</a>"
                    Else
                        sRefLink = FormatNumber(t_stage, 0, TriState.False, TriState.False, TriState.True).ToString
                    End If
                    fleetHtmlOut.Append("<tr><td valign='top' align='left'><span class=""mobileLabel"">At MFR:&nbsp;</span></td><td align='right'><span class=""mobileAnswer"">&nbsp;" + sRefLink.Trim + "</span></td></tr>")
                Else
                    fleetHtmlOut.Append("<tr><td valign='top' align='left'><span class=""mobileLabel"">At MFR:&nbsp;</span></td><td align='right'><span class=""mobileAnswer"">&nbsp;0</span></td></tr>")
                End If

                If th_stage > 0 Then
                    If nTmpModelID > -1 Then
                        sRefLink = "<a class='underline cursor' onclick=""javascript:ParseForm('0',false,false,false,false,false,'cboAircraftTypeID=" + sAirType + Constants.cSvrDataSeperator + sAirFrame
                        sRefLink += "!~!cboAircraftMakeID=" + sMake.ToUpper.Trim
                        sRefLink += "!~!cboAircraftModelID=" + nTmpModelID.ToString
                        sRefLink += "!~!ac_lifecycle_stage=3!~!clearSelection=true');"" title=""Click to view aircraft list"">"
                        sRefLink += FormatNumber(th_stage, 0, TriState.False, TriState.False, TriState.True).ToString + "</a>"
                    Else
                        sRefLink = FormatNumber(th_stage, 0, TriState.False, TriState.False, TriState.True).ToString
                    End If
                    fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' ><span class=""mobileLabel"">In Operation:&nbsp;</span></td><td align='right'><span class=""mobileAnswer"">&nbsp;" + sRefLink.Trim + "</span></td></tr>")
                Else
                    fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' ><span class=""mobileLabel"">In Operation:&nbsp;</span></td><td align='right'><span class=""mobileAnswer"">&nbsp;0</span></td></tr>")
                End If

                If f_stage > 0 Then
                    If nTmpModelID > -1 Then
                        sRefLink = "<a class='underline cursor' onclick=""javascript:ParseForm('0',false,false,false,false,false,'cboAircraftTypeID=" + sAirType + Constants.cSvrDataSeperator + sAirFrame
                        sRefLink += "!~!cboAircraftMakeID=" + sMake.ToUpper.Trim
                        sRefLink += "!~!cboAircraftModelID=" + nTmpModelID.ToString
                        sRefLink += "!~!ac_lifecycle_stage=4!~!clearSelection=true');"" title=""Click to view aircraft list"">"
                        sRefLink += FormatNumber(f_stage, 0, TriState.False, TriState.False, TriState.True).ToString + "</a>"
                    Else
                        sRefLink = FormatNumber(f_stage, 0, TriState.False, TriState.False, TriState.True).ToString
                    End If
                    fleetHtmlOut.Append("<tr><td valign='top' align='left'><span class=""mobileLabel"">Retired:&nbsp;</span></td><td align='right'><span class=""mobileAnswer"">&nbsp;" + sRefLink.Trim + "</span></td></tr>")
                Else
                    fleetHtmlOut.Append("<tr><td valign='top' align='left'><span class=""mobileLabel"">Retired:&nbsp;</span></td><td align='right'><span class=""mobileAnswer"">&nbsp;0</span></td></tr>")
                End If

                If totalcount > 0 Then
                    If nTmpModelID > -1 Then
                        sRefLink = "<a class='underline cursor' onclick=""javascript:ParseForm('0',false,false,false,false,false,'cboAircraftTypeID=" + sAirType + Constants.cSvrDataSeperator + sAirFrame
                        sRefLink += "!~!cboAircraftMakeID=" + sMake.ToUpper.Trim
                        sRefLink += "!~!cboAircraftModelID=" + nTmpModelID.ToString
                        sRefLink += "!~!clearSelection=true');"" title=""Click to view aircraft list"">"
                        sRefLink += FormatNumber(totalcount, 0, TriState.False, TriState.False, TriState.True).ToString + "</a>"
                    Else
                        sRefLink = FormatNumber(totalcount, 0, TriState.False, TriState.False, TriState.True).ToString
                    End If
                    fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' class='border_bottom'><span class=""mobileLabel"">Total Aircraft:&nbsp;</span></td><td class='border_bottom' align='right'><span class=""mobileAnswer"">&nbsp;" + sRefLink.Trim + "</span></td></tr>")
                Else
                    fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' class='border_bottom'><span class=""mobileLabel"">Total Aircraft:&nbsp;</span></td><td class='border_bottom' align='right'><span class=""mobileAnswer"">&nbsp;0</span></td></tr>")
                End If

                fleetHtmlOut.Append("</table>")

            ElseIf searchCriteria.ViewID <> 1 And searchCriteria.ViewID <> 11 Then

                results_table = commonEvo.get_fleet_market_summary_info(searchCriteria, False)

                If Not IsNothing(results_table) Then
                    If results_table.Rows.Count > 0 Then

                        For Each r As DataRow In results_table.Rows

                            If Not IsDBNull(r("daysonmarket")) Then
                                If CLng(r.Item("daysonmarket").ToString) > 0 Then
                                    total_days += 1
                                    days_total_sum = days_total_sum + CLng(r.Item("daysonmarket").ToString)

                                    If CLng(r.Item("daysonmarket").ToString) > daysonmarket_high Then
                                        daysonmarket_high = CLng(r.Item("daysonmarket").ToString)
                                    End If

                                    If CLng(r.Item("daysonmarket").ToString) < daysonmarket_low Then
                                        daysonmarket_low = CLng(r.Item("daysonmarket").ToString)
                                    End If

                                End If
                            End If

                            If Not IsDBNull(r("ac_mfr_year")) Then
                                If IsNumeric(r("ac_mfr_year").ToString) Then

                                    If CInt(r("ac_mfr_year").ToString) > 0 Then

                                        If allhigh = 0 Or CInt(r.Item("ac_mfr_year").ToString) > allhigh Then
                                            allhigh = CInt(r.Item("ac_mfr_year").ToString)
                                        End If

                                        If alllow = 0 Or CInt(r.Item("ac_mfr_year").ToString) < alllow Then
                                            alllow = CInt(r.Item("ac_mfr_year").ToString)
                                        End If

                                    End If
                                End If
                            End If

                            totalcount += 1

                            If r.Item("ac_lifecycle_stage").ToString = "3" And (r.Item("ac_ownership_type").ToString.ToUpper = "S" Or r.Item("ac_ownership_type").ToString.ToUpper = "F" Or r.Item("ac_ownership_type").ToString.ToUpper = "W") Then
                                totalInOpcount += 1
                            End If

                            If r.Item("ac_ownership_type").ToString.ToUpper = "W" And r.Item("ac_lifecycle_stage").ToString = "3" Then
                                w_owner += 1
                            End If

                            If r.Item("ac_ownership_type").ToString.ToUpper = "F" And r.Item("ac_lifecycle_stage").ToString = "3" Then
                                f_owner += 1
                            End If

                            If r.Item("ac_ownership_type").ToString.ToUpper = "S" And r.Item("ac_lifecycle_stage").ToString = "3" Then
                                s_owner += 1
                            End If

                            If r.Item("ac_lifecycle_stage").ToString = "1" Then
                                o_stage += 1
                            End If

                            If r.Item("ac_lifecycle_stage").ToString = "2" Then
                                t_stage += 1
                            End If

                            If r.Item("ac_lifecycle_stage").ToString = "3" And (r.Item("ac_ownership_type").ToString.ToUpper = "S" Or r.Item("ac_ownership_type").ToString.ToUpper = "F" Or r.Item("ac_ownership_type").ToString.ToUpper = "W") Then
                                th_stage += 1
                            End If

                            If r.Item("ac_lifecycle_stage").ToString = "4" Then
                                f_stage += 1
                            End If

                            If r.Item("ac_forsale_flag").ToString.ToUpper.Contains("Y") Then

                                ac_for_sale += 1

                                If Not IsDBNull(r("ac_asking_price")) Then
                                    If Not String.IsNullOrEmpty(r.Item("ac_asking_price").ToString) Then

                                        If CDbl(r.Item("ac_asking_price").ToString) > 0 Then

                                            If forsaleavghigh = 0 Or CDbl(r.Item("ac_asking_price").ToString) > forsaleavghigh Then
                                                forsaleavghigh = CDbl(r.Item("ac_asking_price").ToString)
                                            End If

                                            If forsaleavlow = 0 Or (CDbl(r.Item("ac_asking_price").ToString) < forsaleavlow) Then
                                                forsaleavlow = CDbl(r.Item("ac_asking_price").ToString)
                                            End If

                                            values_avg_asking = values_avg_asking + CDbl(r.Item("ac_asking_price").ToString)
                                            w_asking_count = w_asking_count + 1
                                        End If

                                    End If
                                End If
                            End If



                            If Not IsDBNull(r("ac_exclusive_flag")) Then
                                If r.Item("ac_exclusive_flag").ToString.ToUpper.Contains("Y") Then
                                    ac_exclusive_sale += 1
                                End If
                            End If

                            If Not IsDBNull(r("ac_lease_flag")) Then
                                If r.Item("ac_lease_flag").ToString.ToUpper.Contains("Y") Then
                                    ac_lease += 1
                                End If
                            End If

                        Next

                    End If
                End If

                If (forsaleavlow > 0) Then
                    forsaleavlow = CDbl(forsaleavlow / 1000)
                End If

                If (forsaleavghigh > 0) Then
                    forsaleavghigh = CDbl(forsaleavghigh / 1000)
                End If

                If (ac_for_sale > 0 And th_stage > 0) Then

                    per = System.Math.Round(CDbl(ac_for_sale / th_stage * 100), 1)
                    per2 = System.Math.Round(CDbl(ac_exclusive_sale / ac_for_sale * 100), 1)
                    per3 = System.Math.Round(CDbl(ac_lease / th_stage * 100), 1)

                    If total_days > 0 Then
                        days_avg = System.Math.Round(CLng(days_total_sum) / CLng(total_days))
                    End If

                End If

                If (alllow >= 0 And allhigh > 0) Then
                    For i As Integer = alllow To allhigh
                        avgyear += i
                        avgyearcount += 1
                    Next
                End If

                If avgyear > 0 And avgyearcount > 0 Then
                    avgyear = CLng(avgyear / avgyearcount)
                End If

                fleetHtmlOut.Append("<table id='ownershipTable' cellspacing='0' cellpadding='2' width='100%' class='sub_table'>")
                fleetHtmlOut.Append("<tr class='aircraft_list'><td valign='middle' align='center' colspan='2'><strong>Total&nbsp;Ownership&nbsp;(In&nbsp;Operation)&nbsp;</strong></td></tr>")

                If w_owner > 0 Then

                    If nTmpModelID > -1 Then
                        sRefLink = "<a class='underline cursor' onclick=""javascript:ParseForm('0',false,false,false,false,false,'cboAircraftTypeID=" + sAirType + Constants.cSvrDataSeperator + sAirFrame
                        sRefLink += "!~!cboAircraftMakeID=" + sMake.ToUpper.Trim
                        sRefLink += "!~!cboAircraftModelID=" + nTmpModelID.ToString
                        sRefLink += "!~!ac_ownership_type=W!~!clearSelection=true');"" title=""Click to view aircraft list"">"
                        sRefLink += FormatNumber(w_owner, 0, TriState.False, TriState.False, TriState.True).ToString + "</a>"
                    Else
                        sRefLink = FormatNumber(w_owner, 0, TriState.False, TriState.False, TriState.True).ToString
                    End If

                    fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left'>Whole:&nbsp;</td><td align='right'>" + sRefLink.Trim + "</td></tr>")
                Else
                    fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left'>Whole:&nbsp;</td><td align='right'>&nbsp;0</td></tr>")
                End If

                If s_owner > 0 Then

                    If nTmpModelID > -1 Then
                        sRefLink = "<a class='underline cursor' onclick=""javascript:ParseForm('0',false,false,false,false,false,'cboAircraftTypeID=" + sAirType + Constants.cSvrDataSeperator + sAirFrame
                        sRefLink += "!~!cboAircraftMakeID=" + sMake.ToUpper.Trim
                        sRefLink += "!~!cboAircraftModelID=" + nTmpModelID.ToString
                        sRefLink += "!~!ac_ownership_type=S!~!clearSelection=true');"" title=""Click to view aircraft list"">"
                        sRefLink += FormatNumber(s_owner, 0, TriState.False, TriState.False, TriState.True).ToString + "</a>"
                    Else
                        sRefLink = FormatNumber(s_owner, 0, TriState.False, TriState.False, TriState.True).ToString
                    End If

                    fleetHtmlOut.Append("<tr><td valign='top' align='left'>Shared:&nbsp;</td><td align='right'>&nbsp;" + sRefLink.Trim + "</td></tr>")
                Else
                    fleetHtmlOut.Append("<tr><td valign='top' align='left'>Shared:&nbsp;</td><td align='right'>&nbsp;0</td></tr>")
                End If

                If f_owner > 0 Then
                    If nTmpModelID > -1 Then
                        sRefLink = "<a class='underline cursor' onclick=""javascript:ParseForm('0',false,false,false,false,false,'cboAircraftTypeID=" + sAirType + Constants.cSvrDataSeperator + sAirFrame
                        sRefLink += "!~!cboAircraftMakeID=" + sMake.ToUpper.Trim
                        sRefLink += "!~!cboAircraftModelID=" + nTmpModelID.ToString
                        sRefLink += "!~!ac_ownership_type=F!~!clearSelection=true');"" title=""Click to view aircraft list"">"
                        sRefLink += FormatNumber(f_owner, 0, TriState.False, TriState.False, TriState.True).ToString + "</a>"
                    Else
                        sRefLink = FormatNumber(f_owner, 0, TriState.False, TriState.False, TriState.True).ToString
                    End If

                    fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left'>Fractional:&nbsp;</td><td align='right'>&nbsp;" + sRefLink.Trim + "</td></tr>")
                Else
                    fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left'>Fractional:&nbsp;</td><td align='right'>&nbsp;0</td></tr>")
                End If

                If totalInOpcount > 0 Then
                    If nTmpModelID > -1 Then
                        sRefLink = "<a class='underline cursor' onclick=""javascript:ParseForm('0',false,false,false,false,false,'cboAircraftTypeID=" + sAirType + Constants.cSvrDataSeperator + sAirFrame
                        sRefLink += "!~!cboAircraftMakeID=" + sMake.ToUpper.Trim
                        sRefLink += "!~!cboAircraftModelID=" + nTmpModelID.ToString
                        sRefLink += "!~!ac_lifecycle_stage=3!~!ac_ownership_type=W,F,S!~!clearSelection=true');"" title=""Click to view aircraft list"">"
                        sRefLink += FormatNumber(totalInOpcount, 0, TriState.False, TriState.False, TriState.True).ToString + "</a>"
                    Else
                        sRefLink = FormatNumber(totalInOpcount, 0, TriState.False, TriState.False, TriState.True).ToString
                    End If
                    fleetHtmlOut.Append("<tr><td valign='top' align='left' nowrap='nowrap'>Total Aircraft:&nbsp;</td><td align='right'>&nbsp;" + sRefLink.Trim + "</td></tr>")
                Else
                    fleetHtmlOut.Append("<tr><td valign='top' align='left' nowrap='nowrap'>Total Aircraft:&nbsp;</td><td align='right'>&nbsp;0</td></tr>")
                End If

                If (alllow > 0) And (allhigh > 0) And (allhigh <> CInt(Now().Year)) Then
                    fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='center' class='border_bottom' colspan='2' nowrap='nowrap'>MFR Year Range " + alllow.ToString + " - " + allhigh.ToString + "</td></tr>")
                ElseIf (alllow > 0) And (allhigh = CInt(Now().Year)) Then
                    fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='center' class='border_bottom' colspan='2' nowrap='nowrap'>MFR Year Range " + alllow.ToString + " - To Present</td></tr>")
                Else
                    fleetHtmlOut.Append("<tr class='alt_row'><td valign='top' align='center' class='border_bottom' colspan='2' nowrap='nowrap'>MFR Year Range&nbsp;:&nbsp;N/A</td></tr>")
                End If

                fleetHtmlOut.Append("</table>")




            End If
            'fleetHtmlOut.Append("</div>")
            fleetHtmlOut.Append("</td></tr></table>")


            marketHtmlOut.Append("<table width='100%' cellspacing='0' cellpadding='4' valign='top' class='sub_table formatTable  datagrid blue'>")
            If HttpContext.Current.Session.Item("isMobile") = False Then
                If Trim(HttpContext.Current.Session.Item("DataAsOfDate")) <> "" Then
                    marketHtmlOut.Append("<tr class='aircraft_list'><td valign='top' align='center' colspan='2'><strong class=""subHeader"">Market Status (as of " & FormatDateTime(HttpContext.Current.Session.Item("DataAsOfDate").ToString, DateFormat.ShortDate).Trim & ")</strong></td></tr>")
                Else
                    marketHtmlOut.Append("<tr class='aircraft_list'><td valign='top' align='center' colspan='2'><strong class=""subHeader"">Market Status (as of )</strong></td></tr>")
                End If
            End If

            values_for_sale = 0
            If ac_for_sale > 0 Then
                values_for_sale = ac_for_sale
                If nTmpModelID > -1 Then
                    sRefLink = ""
                    If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = False Then
                        sRefLink = "<a class='underline cursor' onclick=""javascript:ParseForm('0',false,false,false,false,false,'cboAircraftTypeID=" + sAirType + Constants.cSvrDataSeperator + sAirFrame
                        sRefLink += "!~!cboAircraftMakeID=" + sMake.ToUpper.Trim
                        sRefLink += "!~!cboAircraftModelID=" + nTmpModelID.ToString
                        sRefLink += "!~!market=For Sale!~!clearSelection=true');"" title=""Click to view aircraft list"">"
                    End If

                    sRefLink += FormatNumber(ac_for_sale, 0, TriState.False, TriState.False, TriState.True).ToString
                    If HttpContext.Current.Session.Item("localSubscription").crmAerodexFlag = False Then
                        sRefLink += "</a>"
                    End If
                Else
                    sRefLink = FormatNumber(ac_for_sale, 0, TriState.False, TriState.False, TriState.True).ToString
                End If
                marketHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' class='seperator'><span class=""mobileLabel"">For Sale&nbsp;</span></td><td valign='top' align='left' class='rightside'><span class=""mobileAnswer"">" + sRefLink.Trim + string_for_op_percentage + "</span></td></tr>")
            Else
                marketHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' class='seperator'><span class=""mobileLabel"">For Sale:&nbsp;</span></td><td align='left' class='rightside'><span class=""mobileAnswer"">0&nbsp;<span class='tiny'>(0% of For Sale)</span></span></td></tr>")
            End If

            If forsaleavlow > 0 Or forsaleavghigh > 0 Then
                marketHtmlOut.Append("<tr><td valign='top' align='left' class='seperator'><span class=""mobileLabel"">Asking Price Range:&nbsp;</span></td><td align='left' nowrap='nowrap' class='rightside'><span class=""mobileAnswer"">" + FormatCurrency(forsaleavlow, 0, False, True, True).ToString + "k - " + FormatCurrency(forsaleavghigh, 0, False, True, True).ToString + "k</span></td></tr>")
                out_AdminMarketStatus = "<span class=""li""><span class=""label"">For Sale Asking Price Range&nbsp;:&nbsp;</span>" + FormatCurrency(forsaleavlow, 0, False, True, True).ToString + "k - " + FormatCurrency(forsaleavghigh, 0, False, True, True).ToString + "k</span>"
            Else
                marketHtmlOut.Append("<tr><td valign='top' align='left' class='seperator'><span class=""mobileLabel"">Asking Price Range:&nbsp;</span></td><td align='left' class='rightside'><span class=""mobileAnswer"">No Asking Prices</span></td></tr>")
                out_AdminMarketStatus = "<span class=""li""><span class=""label"">For Sale Asking Price Range&nbsp;:&nbsp;</span>No For Sale Asking Prices to Display</span>"
            End If

            If Not HttpContext.Current.Session.Item("localPreferences").AerodexFlag Then
                If CLng(ac_exclusive_sale) > 0 Then
                    If nTmpModelID > -1 Then
                        sRefLink = "<a class='underline cursor' onclick=""javascript:ParseForm('0',false,false,false,false,false,'cboAircraftTypeID=" + sAirType + Constants.cSvrDataSeperator + sAirFrame
                        sRefLink += "!~!cboAircraftMakeID=" + sMake.ToUpper.Trim
                        sRefLink += "!~!cboAircraftModelID=" + nTmpModelID.ToString
                        sRefLink += "!~!market=For Sale on Exclusive!~!clearSelection=true');"" title=""Click to view aircraft list"">"
                        sRefLink += FormatNumber(ac_exclusive_sale, 0, TriState.False, TriState.False, TriState.True).ToString + "</a>"
                    Else
                        sRefLink = FormatNumber(ac_exclusive_sale, 0, TriState.False, TriState.False, TriState.True).ToString
                    End If
                    marketHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' class='seperator'><span class=""mobileLabel"">On Exclusive:&nbsp;</span></td><td align='left' class='rightside'><span class=""mobileAnswer"">" + sRefLink.Trim + " <span class='tiny'>(" + FormatNumber(per2, 1, TriState.False, TriState.False, TriState.True).ToString + "% For Sale on Exclusive)</span></span></td></tr>")
                Else
                    marketHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' class='seperator'><span class=""mobileLabel"">On Exclusive:&nbsp;</span></td><td align='left' class='rightside'><span class=""mobileAnswer"">0&nbsp;<span class='tiny'>(0% For Sale on Exclusive)</span></span></td></tr>")
                End If
            End If

            If avgyear > 0 Then
                marketHtmlOut.Append("<tr><td valign='top' align='left' class='seperator'><span class=""mobileLabel"">Avg MFG Year:&nbsp;</span></td><td align='left' class='rightside'><span class=""mobileAnswer"">" + FormatNumber(avgyear, 0, TriState.False, TriState.False, TriState.False).ToString + "</span></td></tr>")
            Else
                marketHtmlOut.Append("<tr><td valign='top' align='left' class='seperator'><span class=""mobileLabel"">Avg MFG Year:&nbsp;</span></td><td align='left' class='rightside'><span class=""mobileAnswer"">N/A</span></td></tr>")
            End If

            values_dom = 0
            If days_avg > 0 Then
                values_dom = FormatNumber(days_avg, 0)
                marketHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' class='seperator'><span class=""mobileLabel"">Avg Days on Market:&nbsp;</span></td><td align='left' class='rightside'><span class=""mobileAnswer"">" + FormatNumber(days_avg, 0, TriState.False, TriState.False, TriState.True).ToString + "</span></td></tr>")
            Else
                marketHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' class='seperator'><span class=""mobileLabel"">Avg Days on Market:&nbsp;</span></td><td align='left' class='rightside'><span class=""mobileAnswer"">N/A</span></td></tr>")
            End If

            If ac_lease > 0 Then
                If nTmpModelID > -1 Then
                    sRefLink = "<a class='underline cursor' onclick=""javascript:ParseForm('0',false,false,false,false,false,'cboAircraftTypeID=" + sAirType + Constants.cSvrDataSeperator + sAirFrame
                    sRefLink += "!~!cboAircraftMakeID=" + sMake.ToUpper.Trim
                    sRefLink += "!~!cboAircraftModelID=" + nTmpModelID.ToString
                    sRefLink += "!~!lease_status=Y!~!clearSelection=true');"" title=""Click to view aircraft list"">"
                    sRefLink += FormatNumber(ac_lease, 0, TriState.False, TriState.False, TriState.True).ToString + "</a>"
                Else
                    sRefLink = FormatNumber(ac_lease, 0, TriState.False, TriState.False, TriState.True).ToString
                End If
                marketHtmlOut.Append("<tr><td valign='top' align='left' class='seperator'><span class=""mobileLabel"">Leased:&nbsp;</span></td><td align='left' class='rightside'><span class=""mobileAnswer"">" + sRefLink.Trim + "&nbsp;<span class='tiny'>(" + FormatNumber(per3, 1, TriState.False, TriState.False, TriState.True).ToString + "% of In Operation)</span></span></td></tr>")
            Else
                marketHtmlOut.Append("<tr><td valign='top' align='left' class='seperator'><span class=""mobileLabel"">Leased:&nbsp;</span></td><td align='left' class='rightside'><span class=""mobileAnswer"">0&nbsp;<span class='tiny'>(0% of In Operation)</span></span></td></tr>")
            End If

            If absorp_rate > 0 Then
                'If IsNothing(HttpContext.Current.Session.Item("localPreferences").DefaultAnalysisMonths) Then
                '  HttpContext.Current.Session.Item("localPreferences").DefaultAnalysisMonths = 6
                'End If
                absorp_rate = FormatNumber((FormatNumber(absorp_rate, 2) / number_of_months_divide), 2)
                absorp_rate = (FormatNumber(ac_for_sale, 2) / FormatNumber(absorp_rate, 2))
                If absorp_rate > 0 Then
                    marketHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' class='seperator'><span class=""mobileLabel""><a href='help.aspx?t=6&search_term=Absorption Rate' title='Absorption Rate - The absorption rate is the rate at which available aircraft are sold in the market during a given time period. It is calculated by dividing the average number of sales per month by the total number of available aircraft, to completely exhaust the current inventory of aircraft.' tag='Absorption Rate - The absorption rate is the rate at which available aircraft are sold in the market during a given time period. It is calculated by dividing the average number of sales per month by the total number of available aircraft, to completely exhaust the current inventory of aircraft.' target='_blank'>Absorption Rate</a>:&nbsp;</span></td><td valign='top' align='left' class='rightside'><span class=""mobileAnswer"">" & FormatNumber(absorp_rate, 1) & "&nbsp;Months (Based on " & HttpContext.Current.Session.Item("localPreferences").DefaultAnalysisMonths & " Months of Sales)</span></td></tr>")
                Else
                    marketHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' class='seperator'><span class=""mobileLabel""><a href='help.aspx?t=6&search_term=Absorption Rate' title='Absorption Rate - The absorption rate is the rate at which available aircraft are sold in the market during a given time period. It is calculated by dividing the average number of sales per month by the total number of available aircraft, to completely exhaust the current inventory of aircraft.' tag='Absorption Rate - The absorption rate is the rate at which available aircraft are sold in the market during a given time period. It is calculated by dividing the average number of sales per month by the total number of available aircraft, to completely exhaust the current inventory of aircraft.' target='_blank'>Absorption Rate</a>:&nbsp;</span></td><td valign='top' align='left' class='rightside'><span class=""mobileAnswer"">-</span></td></tr>")
                End If
            Else
                marketHtmlOut.Append("<tr class='alt_row'><td valign='top' align='left' class='seperator'><span class=""mobileLabel""><a href='help.aspx?t=6&search_term=Absorption Rate' title='Absorption Rate - The absorption rate is the rate at which available aircraft are sold in the market during a given time period. It is calculated by dividing the average number of sales per month by the total number of available aircraft, to completely exhaust the current inventory of aircraft.' tag='Absorption Rate - The absorption rate is the rate at which available aircraft are sold in the market during a given time period. It is calculated by dividing the average number of sales per month by the total number of available aircraft, to completely exhaust the current inventory of aircraft.' target='_blank'>Absorption Rate</a>:&nbsp;</span></td><td valign='top' align='left' class='rightside'><span class=""mobileAnswer"">-</span></td></tr>")
            End If


            If HttpContext.Current.Session.Item("isMobile") = False Then
                marketHtmlOut.Append("<tr><td valign='top' align='center' class='rightside' colspan='2'>")
                marketHtmlOut.Append("<a href='MarketSummary.aspx?amod_id=" + nTmpModelID.ToString + "'>Click to view Market Summary Report for this Model</a>")
                marketHtmlOut.Append("</td></tr>")
            End If


            If w_asking_count > 0 Then
                values_avg_asking = (values_avg_asking / w_asking_count)
                values_avg_asking_display = values_avg_asking / 1000
            End If


            marketHtmlOut.Append("</table>")

            values_total_inop = totalInOpcount
            values_in_op = ((ac_for_sale / totalInOpcount) * 100)

            If bHasMaster = False Then
                fleetHtmlOut = fleetHtmlOut.Replace(":ParseForm(", ":window.close();ParseForm(")
                marketHtmlOut = marketHtmlOut.Replace(":ParseForm(", ":window.close();ParseForm(")
            End If

            If searchCriteria.ViewID = 11 Or searchCriteria.ViewID = 1 Or searchCriteria.ViewID = 16 Then 'Or commented out MSW - 8/29/18 
                fleetHtmlOut = New StringBuilder
                fleetHtmlOut.Append("<div class=""row""><div class=""" & IIf(String.IsNullOrEmpty(ModelImage), "four", "three") & " columns enableMarginColumn"">")
                fleetHtmlOut.Append(DisplayFunctions.BuildViewOwnershipBox("", w_owner, s_owner, f_owner, totalInOpcount, alllow, allhigh))
                fleetHtmlOut.Append("</div>")


                fleetHtmlOut.Append("<div class=""" & IIf(String.IsNullOrEmpty(ModelImage), "four", "three") & " columns enableMarginColumn"">")
                fleetHtmlOut.Append(DisplayFunctions.BuildViewLifecycleBox("", o_stage, t_stage, th_stage, f_stage, totalcount))
                fleetHtmlOut.Append("</div><div class=""" & IIf(String.IsNullOrEmpty(ModelImage), "four", "three") & " columns enableMarginColumn"">")
                fleetHtmlOut.Append(DisplayFunctions.BuildViewFleetCompBox("", alllow.ToString & " - " & allhigh.ToString, FormatNumber(all_aftt_low, 0).ToString & " - " & FormatNumber(all_aftt_high, 0).ToString, IIf(us_reg > 0, us_reg & "/" & (th_stage - us_reg), "")))
                fleetHtmlOut.Append("</div>")
                If Not String.IsNullOrEmpty(ModelImage) Then
                    fleetHtmlOut.Append("<div class=""three columns enableMarginColumn""><img src=""" & ModelImage & """ width=""235"" style=""margin-top:-3px;""/></div>")
                End If
                fleetHtmlOut.Append("</div>")
            End If


            out_Build_FleetMarketSummary_text = fleetHtmlOut.ToString.Trim
            out_GetMarketStatus = marketHtmlOut.ToString.Trim

        Catch ex As Exception

            aError = "Error in views_Build_FleetMarketSummary(ByVal in_nModelID As Long, ByVal in_sMakeName As String, ByRef out_Build_FleetMarketSummary_text As String, ByRef out_GetMarketStatus As String, ByRef out_string_for_op_percentage As String, ByRef out_avg_days_on_market As Integer) " + ex.Message

        Finally

        End Try

        fleetHtmlOut = Nothing
        marketHtmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function get_market_up_down(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim nMonthOffset = 0

    Dim currentDate_Month As Integer
    Dim currentDate_Year As Integer

    Try

      ' trend data can be off by a month depending on when the tables get updated
      ' start 2 months back from current date
      nMonthOffset = -2

      sQuery.Append("SELECT SUM(mtrend_total_aircraft_for_sale) AS pastmonthforsale FROM Aircraft_Model_Trend WITH(NOLOCK)")
      sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON mtrend_amod_id = amod_id")
      sQuery.Append(" WHERE (mtrend_year = " + Now.Year.ToString + " AND mtrend_month = " + Month(DateAdd("m", -1, Now)).ToString + ")")

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable.Load(SqlReader)

        If Not IsNothing(atemptable) Then
          If atemptable.Rows.Count > 0 Then
            For Each r As DataRow In atemptable.Rows
              ' we do have current data so we only have to shift back one month
              If Not IsDBNull(r("pastmonthforsale")) Then

                If CLng(r.Item("pastmonthforsale").ToString) > 0 Then
                  nMonthOffset = -1
                End If

              End If
            Next
          End If
        End If

        atemptable = Nothing

      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        aError = "Error in get_market_up_down load datatable " + constrExc.Message
      End Try

      sQuery = Nothing
      sQuery = New StringBuilder()

      If searchCriteria.ViewCriteriaAmodID = -1 And String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append("SELECT amod_make_name, count(*) as currentforsale,")
      Else
        sQuery.Append("SELECT amod_make_name, amod_model_name, amod_id, count(*) as currentforsale,")
      End If

      sQuery.Append("(select SUM(mtrend_total_aircraft_for_sale) FROM Aircraft_Model_Trend WITH(NOLOCK) INNER JOIN")
      sQuery.Append(" Aircraft_Model WITH(NOLOCK) ON mtrend_amod_id = amod_id")

      currentDate_Month = Month(DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now))
      currentDate_Year = Year(DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now))

      If searchCriteria.ViewCriteriaAmodID = -1 And String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(" WHERE amod_make_name = A.amod_make_name AND (mtrend_year = " + currentDate_Year.ToString + ") AND (mtrend_month = " + currentDate_Month.ToString + ")")
      Else
        sQuery.Append(" WHERE amod_make_name = A.amod_make_name AND amod_id = A.amod_id AND (mtrend_year = " + currentDate_Year.ToString + ") AND (mtrend_month = " + currentDate_Month.ToString + ")")
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
      ElseIf Not IsNothing(searchCriteria.ViewCriteriaMakeIDArray) Then
        sQuery.Append(Constants.cAndClause + "amod_make_name IN ('" + searchCriteria.ViewCriteriaAircraftMake.ToUpper.Replace(Constants.cCommaDelim, Constants.cValueSeperator).Trim + "')")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftType.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_type_code = '" + searchCriteria.ViewCriteriaAircraftType.Trim + "'")
      End If

      Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
        Case Constants.VIEW_EXECUTIVE
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'E'")
        Case Constants.VIEW_JETS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'J'")
        Case Constants.VIEW_TURBOPROPS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'T'")
        Case Constants.VIEW_PISTONS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'P'")
        Case Constants.VIEW_HELICOPTERS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='R' AND amod_type_code in ('T','P')")
      End Select

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(" " + commonEvo.MakeMarketProductCodeClause(HttpContext.Current.Session.Item("localPreferences"), False))
      Else
        sQuery.Append(" " + commonEvo.BuildMarketProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False))
      End If

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaWeightClass.Trim) Then
        If searchCriteria.ViewCriteriaWeightClass.Contains(",") Then
          sQuery.Append(Constants.cAndClause + "amod_weight_class IN ('" + searchCriteria.ViewCriteriaWeightClass.ToUpper.Replace(Constants.cCommaDelim, Constants.cValueSeperator).Trim + "')")
        Else
          sQuery.Append(Constants.cAndClause + "amod_weight_class = '" + searchCriteria.ViewCriteriaWeightClass.ToUpper.Trim + "'")
        End If
      End If

      sQuery.Append(") AS pastyearforsale,")

      sQuery.Append(" (select SUM(mtrend_total_aircraft_for_sale) FROM Aircraft_Model_Trend WITH(NOLOCK) INNER JOIN")
      sQuery.Append(" Aircraft_Model WITH(NOLOCK) ON mtrend_amod_id = amod_id")

      currentDate_Month = Month(DateAdd("m", nMonthOffset, Now))
      currentDate_Year = Year(DateAdd("m", nMonthOffset, Now))

      If searchCriteria.ViewCriteriaAmodID = -1 And String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(" WHERE amod_make_name = A.amod_make_name AND (mtrend_year = " + currentDate_Year.ToString + ") AND (mtrend_month = " + currentDate_Month.ToString + ")")
      Else
        sQuery.Append(" WHERE amod_make_name = A.amod_make_name AND amod_id = A.amod_id AND (mtrend_year = " + currentDate_Year.ToString + ") AND (mtrend_month = " + currentDate_Month.ToString + ")")
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
      ElseIf Not IsNothing(searchCriteria.ViewCriteriaMakeIDArray) Then
        sQuery.Append(Constants.cAndClause + "amod_make_name IN ('" + searchCriteria.ViewCriteriaAircraftMake.ToUpper.Replace(Constants.cCommaDelim, Constants.cValueSeperator).Trim + "')")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftType.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_type_code = '" + searchCriteria.ViewCriteriaAircraftType.Trim + "'")
      End If

      Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
        Case Constants.VIEW_EXECUTIVE
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'E'")
        Case Constants.VIEW_JETS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'J'")
        Case Constants.VIEW_TURBOPROPS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'T'")
        Case Constants.VIEW_PISTONS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'P'")
        Case Constants.VIEW_HELICOPTERS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='R' AND amod_type_code in ('T','P')")
      End Select

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(" " + commonEvo.MakeMarketProductCodeClause(HttpContext.Current.Session.Item("localPreferences"), False))
      Else
        sQuery.Append(" " + commonEvo.BuildMarketProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False))
      End If

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaWeightClass.Trim) Then
        If searchCriteria.ViewCriteriaWeightClass.Contains(",") Then
          sQuery.Append(Constants.cAndClause + "amod_weight_class IN ('" + searchCriteria.ViewCriteriaWeightClass.ToUpper.Replace(Constants.cCommaDelim, Constants.cValueSeperator).Trim + "')")
        Else
          sQuery.Append(Constants.cAndClause + "amod_weight_class = '" + searchCriteria.ViewCriteriaWeightClass.ToUpper.Trim + "'")
        End If
      End If

      sQuery.Append(") AS pastmonthforsale")

      sQuery.Append(" FROM aircraft WITH(NOLOCK) INNER JOIN aircraft_model AS A WITH(NOLOCK) ON ac_amod_id = amod_id")
      sQuery.Append(" WHERE ac_forsale_flag = 'Y' AND ac_journ_id = 0")

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
      ElseIf Not IsNothing(searchCriteria.ViewCriteriaMakeIDArray) Then
        sQuery.Append(Constants.cAndClause + "amod_make_name IN ('" + searchCriteria.ViewCriteriaAircraftMake.ToUpper.Replace(Constants.cCommaDelim, Constants.cValueSeperator).Trim + "')")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftType.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_type_code = '" + searchCriteria.ViewCriteriaAircraftType.Trim + "'")
      End If

      Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
        Case Constants.VIEW_EXECUTIVE
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'E'")
        Case Constants.VIEW_JETS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'J'")
        Case Constants.VIEW_TURBOPROPS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'T'")
        Case Constants.VIEW_PISTONS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'P'")
        Case Constants.VIEW_HELICOPTERS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='R' AND amod_type_code in ('T','P')")
      End Select

      If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaWeightClass.Trim) Then
        If searchCriteria.ViewCriteriaWeightClass.Contains(",") Then
          sQuery.Append(Constants.cAndClause + "amod_weight_class IN ('" + searchCriteria.ViewCriteriaWeightClass.ToUpper.Replace(Constants.cCommaDelim, Constants.cValueSeperator).Trim + "')")
        Else
          sQuery.Append(Constants.cAndClause + "amod_weight_class = '" + searchCriteria.ViewCriteriaWeightClass.ToUpper.Trim + "'")
        End If
      End If

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True))
      Else
        sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      End If

      If searchCriteria.ViewCriteriaAmodID = -1 And String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(" GROUP BY amod_make_name ORDER by amod_make_name ASC")
      Else
        sQuery.Append(" GROUP BY amod_make_name, amod_model_name, amod_id ORDER BY amod_make_name, amod_model_name, amod_id")
      End If

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_market_up_down(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal marketViewTimeSpan As Integer)</b><br />" + sQuery.ToString

      SqlConn.ConnectionString = clientConnectString
      SqlConn.Open()
      SqlCommand.Connection = SqlConn
      SqlCommand.CommandType = CommandType.Text
      SqlCommand.CommandTimeout = 60

      SqlCommand.CommandText = sQuery.ToString
      SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

      Try
        atemptable = New DataTable
        atemptable.Load(SqlReader)
      Catch constrExc As System.Data.ConstraintException
        Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
        aError = "Error in get_market_up_down load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_market_up_down(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal marketViewTimeSpan As Integer) " + ex.Message

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


    Public Sub views_display_market_up_down_one_model(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, Optional ByVal is_for_spec As Boolean = False)

    Dim last_year_diff As Double = 0.0
    Dim last_month_diff As Double = 0.0
    Dim last_year_percentage As Double = 0.0
    Dim last_month_percentage As Double = 0.0

    Dim htmlOut As New StringBuilder
    Dim results_table As New DataTable
    Dim toggleRowColor As Boolean = False

    Try

            htmlOut.Append("<table width='100%' cellpadding='2' cellspacing='0'" + IIf(HttpContext.Current.Session.Item("lastView") <> 16, "", " class='blue formatTable datagrid'") + "><tr" + IIf(HttpContext.Current.Session.Item("lastView") <> 16, "", " class='header_row'") + ">")
            htmlOut.Append("<td width='100' valign='top' align='center' class='seperator'>&nbsp;</td><td width='33%' valign='top' align='right' class='seperator' nowrap='nowrap'><strong class=""upperCase largeText"">Last&nbsp;Month&nbsp;+/-</strong></td>")

            If searchCriteria.ViewCriteriaTimeSpan < 12 Then
                htmlOut.Append("<td width='33%' valign='top' align='right' class='seperator' nowrap='nowrap'><strong class=""upperCase largeText"">Last&nbsp;Six&nbsp;Months&nbsp;+/-</strong></td>")
            End If

      If searchCriteria.ViewCriteriaTimeSpan >= 12 Then
                htmlOut.Append("<td width='33%' valign='top' align='right' class='seperator' nowrap='nowrap'><strong class=""upperCase largeText"">Last&nbsp;Year&nbsp;+/-</strong></td>")
            End If

      htmlOut.Append("</tr>")
      htmlOut.Append("<tr><td colspan='3' class='rightside' width=""100%"">")

      results_table = get_market_up_down(searchCriteria)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 10 Then
          htmlOut.Append("<div valign=""top"" style='height:370px; overflow: auto;'><p>")
        End If

        htmlOut.Append("<table width='100%' cellpadding='4' cellspacing='0'>")

        If results_table.Rows.Count > 0 Then

          For Each r As DataRow In results_table.Rows    'amod_make_name, amod_model_name

            If Not toggleRowColor Then
              htmlOut.Append("<tr class='alt_row'>")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor='white'>")
              toggleRowColor = False
            End If


            If searchCriteria.ViewCriteriaAmodID > -1 Then
              If Not IsDBNull(r("amod_make_name")) And Not IsDBNull(r("amod_model_name")) Then
                htmlOut.Append("<td width='33%' valign='top' align='left' nowrap='nowrap' class='seperator'>")
                If is_for_spec = True Then
                  htmlOut.Append(r.Item("amod_make_name").ToString + "&nbsp;/&nbsp;" + r.Item("amod_model_name").ToString)
                  htmlOut.Append("</td>")
                Else
                  htmlOut.Append("<a href='DisplayModelDetail.aspx?id=" + r.Item("amod_id").ToString + "' target='_new' class='underline'>")
                  htmlOut.Append(r.Item("amod_make_name").ToString + "&nbsp;/&nbsp;" + r.Item("amod_model_name").ToString)
                  htmlOut.Append("</a></td>")
                End If

              End If
            Else
              If searchCriteria.ViewCriteriaAmodID = -1 And String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                If Not IsDBNull(r("amod_make_name")) Then
                  htmlOut.Append("<td width='33%' valign='top' align='left' nowrap='nowrap' class='seperator'>")
                  If is_for_spec = True Then
                    htmlOut.Append(r.Item("amod_make_name").ToString + "</td>")
                  Else
                    htmlOut.Append("<a href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&make_name=" + r.Item("amod_make_name").ToString + "' class='underline'>")
                    htmlOut.Append(r.Item("amod_make_name").ToString + "</a></td>")
                  End If

                End If
              Else
                If Not IsDBNull(r("amod_make_name")) And Not IsDBNull(r("amod_model_name")) Then
                  htmlOut.Append("<td width='33%' valign='top' align='left' nowrap='nowrap' class='seperator'>")
                  If is_for_spec = True Then
                    htmlOut.Append(r.Item("amod_make_name").ToString + "&nbsp;/&nbsp;" + r.Item("amod_model_name").ToString)
                    htmlOut.Append("</td>")
                  Else
                    htmlOut.Append("<a href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&amod_id=" + r.Item("amod_id").ToString + "' class='underline'>")
                    htmlOut.Append(r.Item("amod_make_name").ToString + "&nbsp;/&nbsp;" + r.Item("amod_model_name").ToString)
                    htmlOut.Append("</a></td>")
                  End If

                End If
              End If

            End If

            last_year_diff = 0
            last_year_percentage = 0

            If Not IsDBNull(r("pastyearforsale")) Then
              If CDbl(r.Item("pastyearforsale").ToString) > 0 Then
                last_year_diff = CDbl(r.Item("currentforsale").ToString) - CDbl(r.Item("pastyearforsale").ToString)
                last_year_percentage = last_year_diff / CDbl(r.Item("pastyearforsale").ToString)
              End If
            End If

            last_month_diff = 0
            last_month_percentage = 0

            If Not IsDBNull(r("pastmonthforsale")) Then
              If CDbl(r.Item("pastmonthforsale").ToString) > 0 Then
                last_month_diff = CDbl(r.Item("currentforsale").ToString) - CDbl(r.Item("pastmonthforsale").ToString)
                last_month_percentage = last_month_diff / CDbl(r.Item("pastmonthforsale").ToString)
              End If
            End If

            If last_month_diff = 0 Then
              htmlOut.Append("<td valign='top' align='right' class='seperator' title='No Change' width='33%' nowrap='nowrap'><img align='center' src='images/gain_loss_none.jpg'>&nbsp;&nbsp;" + last_month_diff.ToString + " (" + FormatPercent(last_month_percentage, 2, TriState.False, TriState.False, TriState.True).ToString + ")</td>")
            ElseIf last_month_diff < 0 Then
              htmlOut.Append("<td valign='top' align='right' class='seperator' title='Net Loss' width='33%' nowrap='nowrap'><img align='center' src='images/gain_loss_down.jpg'>&nbsp;&nbsp;" + last_month_diff.ToString + " (" + FormatPercent(last_month_percentage, 2, TriState.False, TriState.False, TriState.True).ToString + ")</td>")
            Else
              htmlOut.Append("<td valign='top' align='right' class='seperator' title='Net Gain' width='33%' nowrap='nowrap'><img align='center' src='images/gain_loss_up.jpg'>&nbsp;&nbsp;" + last_month_diff.ToString + " (" + FormatPercent(last_month_percentage, 2, TriState.False, TriState.False, TriState.True).ToString + ")</td>")
            End If

            If last_year_diff = 0 Then
              htmlOut.Append("<td valign='top' align='right' class='seperator' title='No Change' width='33%' nowrap='nowrap'><img align='center' src='images/gain_loss_none.jpg'>&nbsp;&nbsp;" + last_year_diff.ToString + " (" + FormatPercent(last_year_percentage, 2, TriState.False, TriState.False, TriState.True).ToString + ")</td></tr>" + vbCrLf)
            ElseIf last_year_diff < 0 Then
              htmlOut.Append("<td valign='top' align='right' class='seperator' title='Net Loss' width='33%' nowrap='nowrap'><img align='center' src='images/gain_loss_down.jpg'>&nbsp;&nbsp;" + last_year_diff.ToString + " (" + FormatPercent(last_year_percentage, 2, TriState.False, TriState.False, TriState.True).ToString + ")</td></tr>" + vbCrLf)
            Else
              htmlOut.Append("<td valign='top' align='right' class='seperator' title='Net Gain' width='33%' nowrap='nowrap'><img align='center' src='images/gain_loss_up.jpg'>&nbsp;&nbsp;" + last_year_diff.ToString + " (" + FormatPercent(last_year_percentage, 2, TriState.False, TriState.False, TriState.True).ToString + ")</td></tr>" + vbCrLf)
            End If

          Next

        Else
          htmlOut.Append("<tr><td valign='top' align='left' class='seperator' colspan='3'>No data matches for your search criteria</td></tr>" + vbCrLf)
        End If

      Else
        htmlOut.Append("<tr><td valign='top' align='left' class='seperator' colspan='3'>No data matches for your search criteria</td></tr>" + vbCrLf)
      End If

      htmlOut.Append("</table>")

      If results_table.Rows.Count > 10 Then
        htmlOut.Append("</p></div>")
      End If

      htmlOut.Append("</td></tr></table>") ' close outer table

    Catch ex As Exception

      aError = "Error in views_display_market_up_down_one_model(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Sub views_display_market_up_down(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

    Dim last_year_diff As Double = 0.0
    Dim last_month_diff As Double = 0.0
    Dim last_year_percentage As Double = 0.0
    Dim last_month_percentage As Double = 0.0

    Dim sum_pastyear_gain_loss As Integer = 0
    Dim sum_pastmonth_gain_loss As Integer = 0
    Dim sum_lastmonth As Integer = 0
    Dim sum_lastyear As Integer = 0

    Dim total_forsale As Integer = 0

    Dim htmlOut As New StringBuilder
    Dim results_table As New DataTable
    Dim toggleRowColor As Boolean = False

    Try

      htmlOut.Append("<table id='marketTrendsOuterTable' width='100%' cellpadding='2' cellspacing='0' class='module'>")
      htmlOut.Append("<tr><td valign='middle' align='center' class='header'>FOR SALE MARKET TRENDS BY MAKE</td></tr>")
      htmlOut.Append("<tr><td valign='top' align='left' colspan='4'>")
      htmlOut.Append("<table id='marketTrendsInnerTable' width='100%' cellpadding='4' cellspacing='0'>")
      htmlOut.Append("<tr><td valign='bottom' align='left' class='seperator' width='35%'><strong>Make</strong></td>")
      htmlOut.Append("<td valign='bottom' align='center' class='seperator' width='15%' nowrap='nowrap'><strong>For&nbsp;Sale</strong></td>")
      htmlOut.Append("<td valign='bottom' align='center' class='seperator' width='25%' nowrap='nowrap'><strong>Last&nbsp;Month&nbsp;+/-</strong></td>")

      If searchCriteria.ViewCriteriaTimeSpan < 12 Then
        htmlOut.Append("<td valign='bottom' align='center' class='seperator' width='25%' nowrap='nowrap'><strong>Last&nbsp;Six&nbsp;Months&nbsp;+/-</strong></td>")
      End If

      If searchCriteria.ViewCriteriaTimeSpan >= 12 Then
        htmlOut.Append("<td valign='bottom' align='center' class='seperator' width='25%' nowrap='nowrap'><strong>Last&nbsp;Year&nbsp;+/-</strong></td>")
      End If

      htmlOut.Append("</tr>")
      htmlOut.Append("<tr><td colspan='4' class='rightside' width=""100%"">")

      results_table = get_market_up_down(searchCriteria)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 15 Then
          htmlOut.Append("<div valign=""top"" style='height:536px; overflow: auto;'><p>")
        End If

        htmlOut.Append("<table id='marketTrendsDataTable' width='100%' cellpadding='4' cellspacing='0'>")

        If results_table.Rows.Count > 0 Then

          For Each r As DataRow In results_table.Rows

            If Not toggleRowColor Then
              htmlOut.Append("<tr class='alt_row'>")
              toggleRowColor = True
            Else
              htmlOut.Append("<tr bgcolor='white'>")
              toggleRowColor = False
            End If

            If searchCriteria.ViewCriteriaAmodID > -1 Then
              If Not IsDBNull(r("amod_make_name")) And Not IsDBNull(r("amod_model_name")) Then
                htmlOut.Append("<td valign='top' align='left' class='seperator' nowrap='nowrap' title='" + r.Item("amod_make_name").ToString + "&nbsp;/&nbsp;" + r.Item("amod_model_name").ToString + "' width='35%'>")
                htmlOut.Append("<a href='DisplayModelDetail.aspx?id=" + r.Item("amod_id").ToString + "' target='_new' class='underline'>")
                htmlOut.Append(r.Item("amod_make_name").ToString + "&nbsp;/&nbsp;" + r.Item("amod_model_name").ToString + "</a></td>")
                htmlOut.Append("<td valign='top' align='right' class='seperator' title='" + r.Item("amod_make_name").ToString + "&nbsp;/&nbsp;" + r.Item("amod_model_name").ToString + "' width='15%'>" + FormatNumber(r.Item("currentforsale").ToString, 0, TriState.False, TriState.False, TriState.True).ToString + "</td>")
              End If
            Else

              If searchCriteria.ViewCriteriaAmodID = -1 And String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                If Not IsDBNull(r("amod_make_name")) Then
                  htmlOut.Append("<td valign='top' align='left' class='seperator' nowrap='nowrap' title='" + r.Item("amod_make_name").ToString + "' width='35%'>")
                  htmlOut.Append("<a href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&make_name=" + r.Item("amod_make_name").ToString + "' class='underline'>" + r.Item("amod_make_name").ToString + "</a></td>")
                  htmlOut.Append("<td valign='top' align='right' class='seperator' title='" + r.Item("amod_make_name").ToString + "' width='15%'>" + FormatNumber(r.Item("currentforsale").ToString, 0, TriState.False, TriState.False, TriState.True).ToString + "</td>")
                End If
              Else
                If Not IsDBNull(r("amod_make_name")) And Not IsDBNull(r("amod_model_name")) Then
                  htmlOut.Append("<td valign='top' align='left' class='seperator' nowrap='nowrap' title='" + r.Item("amod_make_name").ToString + "&nbsp;/&nbsp;" + r.Item("amod_model_name").ToString + "' width='35%'>")
                  htmlOut.Append("<a href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&amod_id=" + r.Item("amod_id").ToString + "' class='underline'>")
                  htmlOut.Append(r.Item("amod_make_name").ToString + "&nbsp;/&nbsp;" + r.Item("amod_model_name").ToString + "</a></td>")
                  htmlOut.Append("<td valign='top' align='right' class='seperator' title='" + r.Item("amod_make_name").ToString + "&nbsp;/&nbsp;" + r.Item("amod_model_name").ToString + "' width='15%'>" + FormatNumber(r.Item("currentforsale").ToString, 0, TriState.False, TriState.False, TriState.True).ToString + "</td>")
                End If
              End If

            End If

            last_year_diff = 0
            last_year_percentage = 0

            If Not IsDBNull(r("pastyearforsale")) Then
              If CDbl(r.Item("pastyearforsale").ToString) > 0 Then
                last_year_diff = CDbl(r.Item("currentforsale").ToString) - CDbl(r.Item("pastyearforsale").ToString)
                last_year_percentage = last_year_diff / CDbl(r.Item("pastyearforsale").ToString)
                sum_lastyear += CInt(r.Item("pastyearforsale").ToString)
              End If
            End If

            last_month_diff = 0
            last_month_percentage = 0

            If Not IsDBNull(r("pastmonthforsale")) Then
              If CDbl(r.Item("pastmonthforsale").ToString) > 0 Then
                last_month_diff = CDbl(r.Item("currentforsale").ToString) - CDbl(r.Item("pastmonthforsale").ToString)
                last_month_percentage = last_month_diff / CDbl(r.Item("pastmonthforsale").ToString)
                sum_lastmonth += CInt(r.Item("pastmonthforsale").ToString)
              End If
            End If

            If last_month_diff = 0 Then
              htmlOut.Append("<td valign='top' align='right' class='seperator' title='No Change' width='25%' nowrap='nowrap'><img align='center' src='images/gain_loss_none.jpg'>&nbsp;&nbsp;" + last_month_diff.ToString + " (" + FormatPercent(last_month_percentage, 2, TriState.False, TriState.False, TriState.True).ToString + ")</td>")
            ElseIf last_month_diff < 0 Then
              htmlOut.Append("<td valign='top' align='right' class='seperator' title='Net Loss' width='25%' nowrap='nowrap'><img align='center' src='images/gain_loss_down.jpg'>&nbsp;&nbsp;" + last_month_diff.ToString + " (" + FormatPercent(last_month_percentage, 2, TriState.False, TriState.False, TriState.True).ToString + ")</td>")
            Else
              htmlOut.Append("<td valign='top' align='right' class='seperator' title='Net Gain' width='25%' nowrap='nowrap'><img align='center' src='images/gain_loss_up.jpg'>&nbsp;&nbsp;" + last_month_diff.ToString + " (" + FormatPercent(last_month_percentage, 2, TriState.False, TriState.False, TriState.True).ToString + ")</td>")
            End If

            If last_year_diff = 0 Then
              htmlOut.Append("<td valign='top' align='right' class='seperator' title='No Change' width='25%' nowrap='nowrap'><img align='center' src='images/gain_loss_none.jpg'>&nbsp;&nbsp;" + last_year_diff.ToString + " (" + FormatPercent(last_year_percentage, 2, TriState.False, TriState.False, TriState.True).ToString + ")</td></tr>" + vbCrLf)
            ElseIf last_year_diff < 0 Then
              htmlOut.Append("<td valign='top' align='right' class='seperator' title='Net Loss' width='25%' nowrap='nowrap'><img align='center' src='images/gain_loss_down.jpg'>&nbsp;&nbsp;" + last_year_diff.ToString + " (" + FormatPercent(last_year_percentage, 2, TriState.False, TriState.False, TriState.True).ToString + ")</td></tr>" + vbCrLf)
            Else
              htmlOut.Append("<td valign='top' align='right' class='seperator' title='Net Gain' width='25%' nowrap='nowrap'><img align='center' src='images/gain_loss_up.jpg'>&nbsp;&nbsp;" + last_year_diff.ToString + " (" + FormatPercent(last_year_percentage, 2, TriState.False, TriState.False, TriState.True).ToString + ")</td></tr>" + vbCrLf)
            End If

            total_forsale += CLng(r.Item("currentforsale").ToString)

          Next

        Else
          htmlOut.Append("<tr><td valign='top' align='left' class='seperator' colspan='4'>No data matches for your search criteria</td></tr>" + vbCrLf)
        End If

      Else
        htmlOut.Append("<tr><td valign='top' align='left' class='seperator' colspan='4'>No data matches for your search criteria</td></tr>" + vbCrLf)
      End If

      htmlOut.Append("</table>") ' close marketTrendsDataTable

      If results_table.Rows.Count > 15 Then
        htmlOut.Append("</p></div>")
      End If

      htmlOut.Append("</td></tr>")

      htmlOut.Append("<tr><td valign='top' align='left' class='seperator' title='Summary' width='35%'><strong>Total Market</strong></td>" + vbCrLf)
      htmlOut.Append("<td valign='top' align='right' class='seperator' title='Summary' width='15%'><strong>" + FormatNumber(total_forsale, 0, TriState.False, TriState.False, TriState.True).ToString + "</strong></td>" + vbCrLf)

      If sum_lastmonth > 0 Then
        sum_pastmonth_gain_loss = total_forsale - sum_lastmonth

        If sum_pastmonth_gain_loss > 0 Then
          htmlOut.Append("<td valign='top' align='right' class='seperator' title='Summary' width='25%'><img align='center' src='images/gain_loss_up.jpg'><strong> ")
          htmlOut.Append(FormatNumber(sum_pastmonth_gain_loss, 0, TriState.False, TriState.False, TriState.True).ToString + " (" + FormatPercent(sum_pastmonth_gain_loss / sum_lastmonth, 2, TriState.False, TriState.False, TriState.True).ToString + ")</strong></td>" + vbCrLf)
        ElseIf sum_pastmonth_gain_loss < 0 Then
          htmlOut.Append("<td valign='top' align='right' class='seperator' title='Summary' width='25%'><img align='center' src='images/gain_loss_down.jpg'><strong> ")
          htmlOut.Append(FormatNumber(sum_pastmonth_gain_loss, 0, TriState.False, TriState.False, TriState.True).ToString + " (" + FormatPercent(sum_pastmonth_gain_loss / sum_lastmonth, 2, TriState.False, TriState.False, TriState.True).ToString + ")</strong></td>" + vbCrLf)
        Else
          htmlOut.Append("<td valign='top' align='right' class='seperator' title='Summary' width='25%'><img align='center' src='images/gain_loss_none.jpg'><strong> ")
          htmlOut.Append(FormatNumber(sum_pastmonth_gain_loss, 0, TriState.False, TriState.False, TriState.True).ToString + " (" + FormatPercent(sum_pastmonth_gain_loss / sum_lastmonth, 2, TriState.False, TriState.False, TriState.True).ToString + ")</strong></td>" + vbCrLf)
        End If
      Else
        htmlOut.Append("<td valign='top' align='right' class='seperator' title='Summary' width='25%'><img align='center' src='images/gain_loss_none.jpg'><strong> 0 (0.00%)</strong></td>" + vbCrLf)
      End If

      If sum_lastyear > 0 Then
        sum_pastyear_gain_loss = total_forsale - sum_lastyear

        If sum_pastyear_gain_loss > 0 Then
          htmlOut.Append("<td valign='top' align='right' class='seperator' title='Summary' width='25%' style='padding-right:15px;'><img align='center' src='images/gain_loss_up.jpg'><strong> ")
          htmlOut.Append(FormatNumber(sum_pastyear_gain_loss, 0, TriState.False, TriState.False, TriState.True).ToString + " (" + FormatPercent(sum_pastyear_gain_loss / sum_lastyear, 2, TriState.False, TriState.False, TriState.True).ToString + ")</strong></td></tr>" + vbCrLf)
        ElseIf sum_pastyear_gain_loss < 0 Then
          htmlOut.Append("<td valign='top' align='right' class='seperator' title='Summary' width='25%' style='padding-right:15px;'><img align='center' src='images/gain_loss_down.jpg'><strong> ")
          htmlOut.Append(FormatNumber(sum_pastyear_gain_loss, 0, TriState.False, TriState.False, TriState.True).ToString + " (" + FormatPercent(sum_pastyear_gain_loss / sum_lastyear, 2, TriState.False, TriState.False, TriState.True).ToString + ")</strong></td></tr>" + vbCrLf)
        Else
          htmlOut.Append("<td valign='top' align='right' class='seperator' title='Summary' width='25%' style='padding-right:15px;'><img align='center' src='images/gain_loss_none.jpg'><strong> ")
          htmlOut.Append(FormatNumber(sum_pastyear_gain_loss, 0, TriState.False, TriState.False, TriState.True).ToString + " (" + FormatPercent(sum_pastyear_gain_loss / sum_lastyear, 2, TriState.False, TriState.False, TriState.True).ToString + ")</strong></td></tr>" + vbCrLf)
        End If
      Else
        htmlOut.Append("<td valign='top' align='right' class='seperator' title='Summary' width='25%' style='padding-right:15px;'><img align='center' src='images/gain_loss_none.jpg'><strong> 0 (0.00%)</strong></td></tr>" + vbCrLf)
      End If

      htmlOut.Append("</table>") ' close inner table
      htmlOut.Append("</td></tr></table>") ' close outer table

    Catch ex As Exception

      aError = "Error in views_display_market_up_down(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_for_sale_graphs_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable
    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sSeperator As String = ""

    Try

      sQuery.Append("SELECT DISTINCT mtrend_year, mtrend_month, sum(mtrend_total_aircraft_for_sale) AS tforsale,")
      sQuery.Append(" ((sum(cast(mtrend_avail_asking_price_total as float))/sum(cast(NULLIF([mtrend_avail_asking_price_count],0) as float)))) AS avgprice")
      sQuery.Append(" FROM Aircraft_Model_Trend WITH(NOLOCK) INNER JOIN aircraft_model WITH(NOLOCK) ON mtrend_amod_id = amod_id")

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftType.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_type_code = '" + searchCriteria.ViewCriteriaAircraftType.Trim + "'")
      End If

      Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
        Case Constants.VIEW_EXECUTIVE
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'E'")
        Case Constants.VIEW_JETS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'J'")
        Case Constants.VIEW_TURBOPROPS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'T'")
        Case Constants.VIEW_PISTONS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'P'")
        Case Constants.VIEW_HELICOPTERS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='R' AND amod_type_code in ('T','P')")
      End Select

      sQuery.Append(" WHERE (((mtrend_year = year(CONVERT(DATETIME, '" + DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now()).ToString + "',102)))")
      sQuery.Append(Constants.cAndClause + "(mtrend_month >= month(CONVERT(DATETIME, '" + DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now()).ToString + "',102))))")

      Dim sClause As String = ""
      If Year(DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now())) <> Year(Now()) Then
        sClause = " OR "
      Else
        sClause = " AND "
      End If

      sQuery.Append(sClause + "((mtrend_year = year(CONVERT(DATETIME, '" + Now.ToString + "',102)))" + Constants.cAndClause + "(mtrend_month <= month(CONVERT(DATETIME, '" + Now.ToString + "',102)))))")

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(" " + commonEvo.MakeMarketProductCodeClause(HttpContext.Current.Session.Item("localPreferences"), False))
      Else
        sQuery.Append(" " + commonEvo.BuildMarketProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False))
      End If

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True))
      Else
        sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      End If

      sQuery.Append(" GROUP BY mtrend_year, mtrend_month ORDER BY mtrend_year ASC, mtrend_month ASC")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_for_sale_graphs_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_for_sale_graphs_info load datatable " + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_for_sale_graphs_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable" + ex.Message

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

  Public Sub views_display_for_sale_graphs(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal graphID As Integer)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Dim x As Integer = 0
    Dim table_width As String = " width='100%'"
    Dim table_height As String = "536"
    Dim monthToDateAvgAsking As Double = 0.0
    Dim monthToDateForsale As Double = 0.0
    Dim sTmpTitle As String = ""
    Dim temp_string As String = ""

    Try

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sTmpTitle = commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, "")
        table_height = "456"
      ElseIf searchCriteria.ViewCriteriaMakeAmodID > -1 Or Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sTmpTitle = commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaMakeAmodID, True, "")
        If String.IsNullOrEmpty(sTmpTitle) Then
          sTmpTitle = searchCriteria.ViewCriteriaAircraftMake.Trim
        End If
      ElseIf searchCriteria.ViewCriteriaProductType <> Constants.PRODUCT_CODE_ALL Then

        Select Case (searchCriteria.ViewCriteriaProductType)
          Case Constants.PRODUCT_CODE_BUSINESS
            sTmpTitle = "ALL BUSINESS AIRCRAFT"
          Case Constants.PRODUCT_CODE_COMMERCIAL
            sTmpTitle = "ALL COMMERCIAL AIRCRAFT"
          Case Constants.PRODUCT_CODE_HELICOPTERS
            sTmpTitle = "ALL HELICOPTERS"
        End Select

      End If

      results_table = get_for_sale_graphs_info(searchCriteria)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append(vbCrLf + "<script type='text/javascript'>" + vbCrLf)
          htmlOut.Append("google.load('visualization', '1', {'packages':['corechart']});" + vbCrLf)
          htmlOut.Append("google.setOnLoadCallback(drawVisualization" + graphID.ToString + ");" + vbCrLf)
          htmlOut.Append("function drawVisualization" + graphID.ToString + "() {" + vbCrLf)


          ' COMMENTED OUT MSW - GRAPH WAS NOT WORKING ----
          'htmlOut.Append("var data = new google.visualization.DataTable();" + vbCrLf)
          'htmlOut.Append("data.addColumn('string', 'Year');" + vbCrLf)
          'htmlOut.Append("data.addColumn('number', 'For Sale');" + vbCrLf)
          'htmlOut.Append("data.addRows(" + results_table.Rows.Count.ToString + ");" + vbCrLf)


          htmlOut.Append("var data = new google.visualization.DataTable();" + vbCrLf)
          htmlOut.Append("data.addColumn('string', 'Year');" + vbCrLf)
          htmlOut.Append("data.addColumn('number', 'For Sale');" + vbCrLf)
          htmlOut.Append("data.addRows([" + vbCrLf)


          temp_string = ""
          ' loop through and show forsale data
          For Each r As DataRow In results_table.Rows
            If Not IsDBNull(r.Item("mtrend_month")) Then
              If Not String.IsNullOrEmpty(r.Item("mtrend_month").ToString.Trim) Then

                ' COMMENTED OUT MSW - GRAPH WAS NOT WORKING ----
                'If CInt(r.Item("mtrend_month").ToString) = Now.Month Then
                '  '      monthToDateForsale = find_current_forsale()
                '  If monthToDateForsale > 0 Then
                '    htmlOut.Append(" data.setCell(" + x.ToString + ", 0, 'Month to Date');" + vbCrLf)
                '    htmlOut.Append(" data.setCell(" + x.ToString + ", 1, " + FormatNumber((monthToDateForsale / 1000), 0, TriState.False, TriState.False, TriState.False).ToString + ");" + vbCrLf)
                '  Else
                '    htmlOut.Append(" data.setCell(" + x.ToString + ", 0, 'Month to Date');" + vbCrLf)
                '    htmlOut.Append(" data.setCell(" + x.ToString + ", 1, 0);" + vbCrLf)
                '  End If
                'Else
                '  htmlOut.Append(" data.setCell(" + x.ToString + ", 0, '" + r.Item("mtrend_month").ToString + "-" + r.Item("mtrend_year").ToString + "');" + vbCrLf)
                '  htmlOut.Append(" data.setCell(" + x.ToString + ", 1, " + FormatNumber(r.Item("tforsale").ToString, 0, TriState.False, TriState.False, TriState.False) + ");" + vbCrLf)
                'End If

                If Trim(temp_string) <> "" Then
                  temp_string &= ", "
                End If

                If Not IsDBNull(r.Item("tforsale")) Then
                  If IsNumeric(r.Item("tforsale")) Then
                    If CInt(r.Item("mtrend_month").ToString) = Now.Month Then
                      If CDbl(r.Item("tforsale")) = 0 Then
                        ' if its this month, and its 0, do nothing 
                      Else
                        temp_string &= (" ['" + r.Item("mtrend_month").ToString + "-" + r.Item("mtrend_year").ToString + "', " & FormatNumber(r.Item("tforsale").ToString, 0, TriState.False, TriState.False, TriState.False) & "]")
                      End If
                    Else
                      If CDbl(r.Item("tforsale")) = 0 Then
                        temp_string &= (" ['" + r.Item("mtrend_month").ToString + "-" + r.Item("mtrend_year").ToString + "', 0]")
                      Else
                        temp_string &= (" ['" + r.Item("mtrend_month").ToString + "-" + r.Item("mtrend_year").ToString + "', " & FormatNumber(r.Item("tforsale").ToString, 0, TriState.False, TriState.False, TriState.False) & "]")
                      End If
                    End If

                  End If
                End If

                'If CInt(r.Item("mtrend_month").ToString) = Now.Month Then
                '  '      monthToDateForsale = find_current_forsale()
                '  If monthToDateForsale > 0 Then
                '    ' htmlOut.Append(" data.setCell(" + x.ToString + ", 0, 'Month to Date');" + vbCrLf)
                '    htmlOut.Append(" [" + x.ToString + ", 1, " + FormatNumber((monthToDateForsale / 1000), 0, TriState.False, TriState.False, TriState.False).ToString + ");" + vbCrLf)
                '  Else
                '    'htmlOut.Append(" data.setCell(" + x.ToString + ", 0, 'Month to Date');" + vbCrLf)
                '    htmlOut.Append(" [" + x.ToString + ", 1, 0);" + vbCrLf)
                '  End If
                'Else
                '  htmlOut.Append(" [" + x.ToString + ", '" + r.Item("mtrend_month").ToString + "-" + r.Item("mtrend_year").ToString + "');" + vbCrLf)
                '  htmlOut.Append(" [" + x.ToString + ", " + FormatNumber(r.Item("tforsale").ToString, 0, TriState.False, TriState.False, TriState.False) + ");" + vbCrLf)
                'End If




                x += 1

              End If
            End If

          Next


          htmlOut.Append(temp_string & "]);")



          htmlOut.Append("var chart = new google.visualization.LineChart(document.getElementById('visualization" + graphID.ToString + "'));" + vbCrLf)
          htmlOut.Append("chart.draw(data, {chartArea:{width:'95%',height:'85%'}, titleY:'Aircraft For Sale', smoothLine:true, legend:'none', colors:['#4684EE', 'black'] });" + vbCrLf)
          htmlOut.Append("}" + vbCrLf)


          htmlOut.Append(vbCrLf + "google.load('visualization', '1', {'packages':['corechart']});" + vbCrLf)
          htmlOut.Append("google.setOnLoadCallback(drawVisualization" + (graphID + 1).ToString + ");" + vbCrLf)
          htmlOut.Append("function drawVisualization" + (graphID + 1).ToString + "() {" + vbCrLf)
          htmlOut.Append("var data = new google.visualization.DataTable();" + vbCrLf)
          htmlOut.Append("data.addColumn('string', 'Year');" + vbCrLf)
          htmlOut.Append("data.addColumn('number', 'Avg Price');" + vbCrLf)
          htmlOut.Append("data.addRows(" + results_table.Rows.Count.ToString + ");" + vbCrLf)

          x = 0

          ' loop through again to display graph data
          For Each r As DataRow In results_table.Rows

            If Not IsDBNull(r.Item("mtrend_month")) Then
              If Not String.IsNullOrEmpty(r.Item("mtrend_month").ToString.Trim) Then

                If CInt(r.Item("mtrend_month").ToString) = Now.Month Then
                  '      monthToDateAvgAsking = find_current_asking_average()
                  If monthToDateAvgAsking > 0 Then
                    htmlOut.Append(" data.setCell(" + x.ToString + ", 0, 'Month to Date');" + vbCrLf)
                    htmlOut.Append(" data.setCell(" + x.ToString + ", 1, " + FormatNumber((monthToDateAvgAsking / 1000), 2, TriState.False, TriState.False, TriState.False).ToString + ");" + vbCrLf)
                  Else
                    '  htmlOut.Append(" data.setCell(" + x.ToString + ", 0, 'Month to Date');" + vbCrLf)
                    '  htmlOut.Append(" data.setCell(" + x.ToString + ", 1, 0);" + vbCrLf)
                  End If

                Else

                  htmlOut.Append(" data.setCell(" + x.ToString + ", 0, '" + r.Item("mtrend_month").ToString + "-" + r.Item("mtrend_year").ToString + "');" + vbCrLf)

                  If Not IsDBNull(r.Item("avgprice")) Then
                    If Not String.IsNullOrEmpty(r.Item("avgprice").ToString.Trim) Then

                      If CLng(r.Item("avgprice").ToString) > 0 Then
                        htmlOut.Append(" data.setCell(" + x.ToString + ", 1, " + FormatNumber((r.Item("avgprice").ToString / 1000), 2, TriState.False, TriState.False, TriState.False).ToString + ");" + vbCrLf)
                      Else
                        htmlOut.Append(" data.setCell(" + x.ToString + ", 1, 0);" + vbCrLf)
                      End If

                    Else
                      htmlOut.Append(" data.setCell(" + x.ToString + ", 1, 0);" + vbCrLf)
                    End If

                  Else
                    htmlOut.Append(" data.setCell(" + x.ToString + ", 1, 0);" + vbCrLf)
                  End If

                  x += 1

                End If

              End If

            End If

          Next

          htmlOut.Append("var chart = new google.visualization.LineChart(document.getElementById('visualization" + (graphID + 1).ToString + "'));" + vbCrLf)
          htmlOut.Append("chart.draw(data, {chartArea:{width:'95%',height:'85%'}, titleY:'Average Asking Price (US $k)', smoothLine:true, legend:'none', colors:['#DC3912', 'black'] });" + vbCrLf)
          htmlOut.Append("}" + vbCrLf)
          htmlOut.Append("</script>" + vbCrLf)

        End If

      End If

      If Not String.IsNullOrEmpty(htmlOut.ToString.Trim) Then
        htmlOut.Append("<table id='marketTrendsOuterTable' height='" + table_height + "'" + table_width + " cellpadding='2' cellspacing='0' class='module'>")
        htmlOut.Append("<tr><td valign='middle' align='center' class='header'>MARKET TRENDS FOR " + sTmpTitle.Trim + "</td></tr>")
        htmlOut.Append("<tr><td valign='top' align='left'><table id='marketTrendsInnerTable' width='100%' cellspacing='0' cellpadding='1'>")
        htmlOut.Append("<tr><td valign='top' align='left' class='tabheader'><strong> Aircraft For Sale</strong><em> (past " + searchCriteria.ViewCriteriaTimeSpan.ToString + " months)</em></td><td width='20%' class='border_bottom'>&nbsp;</td></tr>")
        htmlOut.Append("<tr><td valign='top' align='left' class='border_bottom_right' colspan='2'><div id='visualization" + graphID.ToString + "' style='height:" + Math.Round(CLng(table_height) / 2, 0).ToString + "px;'></div></td></tr>")
        htmlOut.Append("<tr><td colspan='2'>&nbsp;</td><tr><td valign='top' align='left' class='tabheader'><strong> Average Asking Price</strong><em> (past " + searchCriteria.ViewCriteriaTimeSpan.ToString + " months)</em></td><td width='20%' class='border_bottom'>&nbsp;</td></tr>")
        htmlOut.Append("<tr><td valign='top' align='center' class='border_bottom_right' colspan='2'><div id='visualization" + (graphID + 1).ToString + "' style='height:" + Math.Round(CLng(table_height) / 2, 0).ToString + "px;'></div></td></tr></table></td></tr></table>")
      Else
        htmlOut.Append("<table id='marketTrendsOuterTable' height='" + table_height + "'" + table_width + " cellpadding='2' cellspacing='0' class='module'>")
        htmlOut.Append("<tr><td valign='middle' align='center' class='header'>MARKET TRENDS FOR " + sTmpTitle.Trim + "</td></tr>")
        htmlOut.Append("<tr><td valign='top' align='left'><table id='marketTrendsInnerTable' width='100%' cellspacing='0' cellpadding='1'>")
        htmlOut.Append("<tr><td valign='top' align='left' class='tabheader'><strong> Aircraft For Sale</strong><em> (past " + searchCriteria.ViewCriteriaTimeSpan.ToString + " months)</em></td><td width='20%' class='border_bottom'>&nbsp;</td></tr>")
        htmlOut.Append("<tr><td valign='top' align='center' class='border_bottom_right' colspan='2'><div style='height:" + Math.Round(CLng(table_height) / 2, 0).ToString + "px;'>No Data to display</div></td></tr>")
        htmlOut.Append("<tr><td colspan='2'>&nbsp;</td></tr><tr><td valign='top' align='left' class='tabheader'><strong> Average Asking Price</strong><em> (past " + searchCriteria.ViewCriteriaTimeSpan.ToString + " months)</em></td><td width='20%' class='border_bottom'>&nbsp;</td></tr>")
        htmlOut.Append("<tr><td valign='top' align='center' class='border_bottom_right' colspan='2'><div style='height:" + Math.Round(CLng(table_height) / 2, 0).ToString + "px;'>No Data to display</div></td></tr></table></td></tr></table>")
      End If

    Catch ex As Exception

      aError = "Error in views_display_for_sale_graphs(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal graphID As Integer) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_new_vs_used_piechart_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable
    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sSeperator As String = ""

    Try

      sQuery.Append("SELECT DISTINCT journ_newac_flag, count(*) AS tcount")
      sQuery.Append(" FROM aircraft WITH(NOLOCK) INNER JOIN aircraft_model WITH(NOLOCK) ON ac_amod_id = amod_id")
      sQuery.Append(" INNER JOIN journal WITH(NOLOCK) ON ac_journ_id = journ_id")
      sQuery.Append(" WHERE journ_subcategory_code like 'WS%' AND right(journ_subcategory_code,4) <> 'CORR' AND right(journ_subcategory_code,2) <> 'IT'")

      sQuery.Append(Constants.cAndClause + "(journ_date >= '" + Month(DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now())).ToString + "/01/" + Year(DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now())).ToString + "')")

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftType.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_type_code = '" + searchCriteria.ViewCriteriaAircraftType.Trim + "'")
      End If

      Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
        Case Constants.VIEW_EXECUTIVE
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'E'")
        Case Constants.VIEW_JETS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'J'")
        Case Constants.VIEW_TURBOPROPS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'T'")
        Case Constants.VIEW_PISTONS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'P'")
        Case Constants.VIEW_HELICOPTERS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='R' AND amod_type_code in ('T','P')")
      End Select

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, True))
      Else
        sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      End If

      sQuery.Append(" GROUP BY journ_newac_flag ORDER BY journ_newac_flag")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_new_vs_used_piechart_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_new_vs_used_piechart_info load datatable" + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_new_vs_used_piechart_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable" + ex.Message

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

  Public Sub views_display_new_vs_used_piechart(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal graphID As Integer)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Dim x As Integer = 0
    Dim new_used_sum As Integer = 0
    Dim graph_info As String = ""
    Dim new_used_percent As Double = 0.0
    Dim sTmpTitle As String = ""

    Try

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sTmpTitle = commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, "")
      ElseIf searchCriteria.ViewCriteriaMakeAmodID > -1 Or Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sTmpTitle = commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaMakeAmodID, True, "")
        If String.IsNullOrEmpty(sTmpTitle) Then
          sTmpTitle = searchCriteria.ViewCriteriaAircraftMake.Trim
        End If
      ElseIf searchCriteria.ViewCriteriaProductType <> Constants.PRODUCT_CODE_ALL Then

        Select Case (searchCriteria.ViewCriteriaProductType)
          Case Constants.PRODUCT_CODE_BUSINESS
            sTmpTitle = "All business aircraft"
          Case Constants.PRODUCT_CODE_COMMERCIAL
            sTmpTitle = "All commercial aircraft"
          Case Constants.PRODUCT_CODE_HELICOPTERS
            sTmpTitle = "All helicopters"
        End Select

      End If

      results_table = get_new_vs_used_piechart_info(searchCriteria)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append(vbCrLf + "<script type='text/javascript'>" + vbCrLf)
          htmlOut.Append("google.load('visualization', '1', {'packages':['corechart']});" + vbCrLf)
          htmlOut.Append("google.setOnLoadCallback(drawVisualization" + graphID.ToString + ");" + vbCrLf)
          htmlOut.Append("function drawVisualization" + graphID.ToString + "() {" + vbCrLf)
          htmlOut.Append("var data = new google.visualization.DataTable();" + vbCrLf)
          htmlOut.Append("data.addColumn('string', 'newused');" + vbCrLf)
          htmlOut.Append("data.addColumn('number', '');" + vbCrLf)
          htmlOut.Append("data.addRows(" + results_table.Rows.Count.ToString + ");" + vbCrLf)

          ' loop through once to get new_used_sum
          For Each r As DataRow In results_table.Rows
            new_used_sum += CLng(r.Item("tcount").ToString.Trim)
          Next

          ' loop through again to display graph data
          For Each r As DataRow In results_table.Rows

            new_used_percent = 0.0

            If Not IsDBNull(r.Item("tcount")) Then
              If Not String.IsNullOrEmpty(r.Item("tcount").ToString.Trim) Then

                If CLng(r.Item("tcount").ToString) > 0 Then
                  If r.Item("journ_newac_flag").ToString.ToLower = "n" Then
                    graph_info = "Used"
                  ElseIf r.Item("journ_newac_flag").ToString.ToLower = "y" Then
                    graph_info = "New"
                  End If

                  new_used_percent = ((CDbl(r.Item("tcount").ToString) / new_used_sum) * 100)

                  htmlOut.Append("data.setCell(" + x.ToString + ", 0, '" + graph_info.Trim + "','" + graph_info.Trim + " (" + FormatNumber(new_used_percent, 2, TriState.False, TriState.False, TriState.False).ToString + "%)');" + vbCrLf)
                  htmlOut.Append("data.setCell(" + x.ToString + ", 1, " + Math.Round(CDbl(r.Item("tcount").ToString), 0).ToString + ");" + vbCrLf)
                  x += 1

                End If

              End If
            End If

          Next

          htmlOut.Append("var chart = new google.visualization.PieChart(document.getElementById('visualization" + graphID.ToString + "'));" + vbCrLf)
          htmlOut.Append("chart.draw(data, {chartArea:{width:'95%',height:'85%'}, legend:'top', legendFontSize:12 });" + vbCrLf)
          htmlOut.Append("}" + vbCrLf)
          htmlOut.Append("</script>" + vbCrLf)

        End If

      End If

      If Not String.IsNullOrEmpty(htmlOut.ToString.Trim) Then
        htmlOut.Append("<table width='100%' height='250' cellpadding='2' cellspacing='0' class='module'>")
        htmlOut.Append("<tr><td valign='middle' align='center' class='header' colspan='2'>NEW VS. USED SALES</td></tr>")
        htmlOut.Append("<tr><td valign='top' align='left' class='tabheader'><strong>" + sTmpTitle.Trim + "</strong></td><td width='20%' class='border_bottom'>&nbsp;</td></tr>")
        htmlOut.Append("<tr><td valign='top' align='left' class='border_bottom_right' colspan='2'><div id='visualization" + graphID.ToString + "' style='text-align:center; width:100%; height:250px;'></div></td></tr></table>")
      Else
        htmlOut.Append("<table width='100%' height='250' cellpadding='2' cellspacing='0' class='module'>")
        htmlOut.Append("<tr><td valign='middle' align='center' class='header' colspan='2'>NEW VS. USED SALES</td></tr>")
        htmlOut.Append("<tr><td valign='top' align='left' class='tabheader'><strong>" + sTmpTitle.Trim + "</strong></td><td width='20%' class='border_bottom'>&nbsp;</td></tr>")
        htmlOut.Append("<tr><td valign='top' align='left' class='border_bottom_right' colspan='2'><div style='text-align:center; width:100%; height:250px;'>No Data to display</div></td></tr></table>")
      End If

    Catch ex As Exception

      aError = "Error in views_display_new_vs_used_piechart(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal graphID As Integer) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

  Public Function get_leased_per_month_transactions_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable
    Dim atemptable As New DataTable
    Dim sQuery = New StringBuilder()

    Dim SqlConn As New SqlClient.SqlConnection
    Dim SqlCommand As New SqlClient.SqlCommand
    Dim SqlReader As SqlClient.SqlDataReader
    Dim SqlException As SqlClient.SqlException : SqlException = Nothing

    Dim sSeperator As String = ""

    Try

      sQuery.Append("SELECT YEAR(journ_date) AS aYear, MONTH(journ_date) AS amonth, count(*) AS acount")
      sQuery.Append(" FROM Journal WITH(NOLOCK) INNER JOIN Aircraft WITH(NOLOCK) ON journ_ac_id = ac_id AND journ_id = ac_journ_id")
      sQuery.Append(" INNER JOIN aircraft_model WITH(NOLOCK) ON ac_amod_id = amod_id")
      sQuery.Append(" WHERE ")

      sQuery.Append("((journ_date >= '" + Month(DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now())).ToString + "/01/" + Year(DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now())).ToString + "')")
      sQuery.Append(Constants.cAndClause + "(journ_date < '" + Now.Month.ToString + "/01/" + Now.Year.ToString + "'))")

      sQuery.Append(Constants.cAndClause + "journ_subcat_code_part1 LIKE 'L%' AND journ_subcat_code_part2 NOT IN ('CO') AND journ_subcat_code_part3 NOT IN ('IT', 'RR')")

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
      ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftType.Trim) Then
        sQuery.Append(Constants.cAndClause + "amod_type_code = '" + searchCriteria.ViewCriteriaAircraftType.Trim + "'")
      End If

      Select Case CInt(searchCriteria.ViewCriteriaAirframeType)
        Case Constants.VIEW_EXECUTIVE
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'E'")
        Case Constants.VIEW_JETS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'J'")
        Case Constants.VIEW_TURBOPROPS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'T'")
        Case Constants.VIEW_PISTONS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='F' AND amod_type_code = 'P'")
        Case Constants.VIEW_HELICOPTERS
          sQuery.Append(Constants.cAndClause + "amod_airframe_type_code='R' AND amod_type_code in ('T','P')")
      End Select

      If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
        sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
      Else
        sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
      End If

      sQuery.Append(" GROUP BY year(journ_date), month(journ_date)")
      sQuery.Append(" ORDER BY year(journ_date), month(journ_date)")

      HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_leased_per_month_transactions_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
        aError = "Error in get_leased_per_month_transactions_info load datatable" + constrExc.Message
      End Try

    Catch ex As Exception
      Return Nothing

      aError = "Error in get_leased_per_month_transactions_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable" + ex.Message

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

  Public Sub views_display_leased_per_month_transactions(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal graphID As Integer)

    Dim results_table As New DataTable
    Dim htmlOut As New StringBuilder

    Dim x As Integer = 0
    Dim new_used_sum As Integer = 0
    Dim graph_info As String = ""
    Dim new_used_percent As Double = 0.0
    Dim sTmpTitle As String = ""

    Try

      If searchCriteria.ViewCriteriaAmodID > -1 Then
        sTmpTitle = commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, "")
      ElseIf searchCriteria.ViewCriteriaMakeAmodID > -1 Or Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
        sTmpTitle = commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaMakeAmodID, True, "")
        If String.IsNullOrEmpty(sTmpTitle) Then
          sTmpTitle = searchCriteria.ViewCriteriaAircraftMake.Trim
        End If
      ElseIf searchCriteria.ViewCriteriaProductType <> Constants.PRODUCT_CODE_ALL Then

        Select Case (searchCriteria.ViewCriteriaProductType)
          Case Constants.PRODUCT_CODE_BUSINESS
            sTmpTitle = "All business aircraft"
          Case Constants.PRODUCT_CODE_COMMERCIAL
            sTmpTitle = "All commercial aircraft"
          Case Constants.PRODUCT_CODE_HELICOPTERS
            sTmpTitle = "All helicopters"
        End Select

      End If

      results_table = get_leased_per_month_transactions_info(searchCriteria)

      If Not IsNothing(results_table) Then

        If results_table.Rows.Count > 0 Then

          htmlOut.Append(vbCrLf + "<script type='text/javascript'>" + vbCrLf)
          htmlOut.Append(" google.load('visualization', '1', {'packages':['corechart']});" + vbCrLf)
          htmlOut.Append(" google.setOnLoadCallback(drawVisualization" + graphID.ToString + ");" + vbCrLf)
          htmlOut.Append(" function drawVisualization" + graphID.ToString + "() {" + vbCrLf)
          htmlOut.Append(" var data = new google.visualization.DataTable();" + vbCrLf)
          htmlOut.Append(" data.addColumn('string', 'Month');" + vbCrLf)
          htmlOut.Append(" data.addColumn('number', 'Leased');" + vbCrLf)
          htmlOut.Append(" data.addRows(" + results_table.Rows.Count.ToString + ");" + vbCrLf)

          ' loop through to display graph data
          For Each r As DataRow In results_table.Rows
            If Not IsDBNull(r.Item("aYear")) Then
              If Not String.IsNullOrEmpty(r.Item("aYear").ToString.Trim) Then

                If CInt(r.Item("aMonth").ToString) = Now.Month Then
                  htmlOut.Append(" data.setCell(" + x.ToString + ", 0, 'Month to Date');" + vbCrLf)
                  htmlOut.Append(" data.setCell(" + x.ToString + ", 1, " + FormatNumber(r.Item("aCount").ToString, 0, TriState.False, TriState.False, TriState.False) + ");" + vbCrLf)
                Else
                  htmlOut.Append(" data.setCell(" + x.ToString + ", 0, '" + r.Item("aMonth").ToString + "-" + r.Item("aYear").ToString + "');" + vbCrLf)
                  htmlOut.Append(" data.setCell(" + x.ToString + ", 1, " + FormatNumber(r.Item("aCount").ToString, 0, TriState.False, TriState.False, TriState.False) + ");" + vbCrLf)
                End If

                x += 1

              End If
            End If

          Next

          htmlOut.Append(" var chart = new google.visualization.LineChart(document.getElementById('visualization" + graphID.ToString + "'));" + vbCrLf)
          htmlOut.Append(" chart.draw(data, {chartArea:{width:'95%',height:'85%'}, titleY: 'Leasses', smoothLine: true, legend: 'none', colors:['#4684EE', 'black']});" + vbCrLf)
          htmlOut.Append("}" + vbCrLf)
          htmlOut.Append("</script>" + vbCrLf)

        End If

      End If

      If Not String.IsNullOrEmpty(htmlOut.ToString.Trim) Then
        htmlOut.Append("<table width='100%' height='250' cellpadding='2' cellspacing='0' class='module'>")
        htmlOut.Append("<tr><td valign='middle' align='center' class='header' colspan='2'>LEASE TRENDS (<em># of Lease Transactions</em>)</td></tr>")
        htmlOut.Append("<tr><td valign='top' align='left' class='tabheader'><strong>" + sTmpTitle.Trim + "</strong></td><td width='20%' class='border_bottom'>&nbsp;</td></tr>")
        htmlOut.Append("<tr><td valign='top' align='left' class='border_bottom_right' colspan='2'><div id='visualization" + graphID.ToString + "' style='text-align:center; width:100%; height:250px;'></div></td></tr></table>")
      Else
        htmlOut.Append("<table width='100%' height='250' cellpadding='2' cellspacing='0' class='module'>")
        htmlOut.Append("<tr><td valign='middle' align='center' class='header' colspan='2'>LEASE TRENDS (<em># of Lease Transactions</em>)</td></tr>")
        htmlOut.Append("<tr><td valign='top' align='left' class='tabheader'><strong>" + sTmpTitle.Trim + "</strong></td><td width='20%' class='border_bottom'>&nbsp;</td></tr>")
        htmlOut.Append("<tr><td valign='top' align='left' class='border_bottom_right' colspan='2'><div style='text-align:center; width:100%; height:250px;'>No Data to display</div></td></tr></table>")
      End If

    Catch ex As Exception

      aError = "Error in views_display_leased_per_month_transactions(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByVal graphID As Integer) " + ex.Message

    Finally

    End Try

    'return resulting html string
    out_htmlString = htmlOut.ToString
    htmlOut = Nothing
    results_table = Nothing

  End Sub

#End Region

End Class
