' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/resdiual_market_value.vb $
'$$Author: Amanda $
'$$Date: 6/16/20 4:02p $
'$$Modtime: 6/16/20 2:20p $
'$$Revision: 4 $
'$$Workfile: resdiual_market_value.vb $
'
' ********************************************************************************

<System.Serializable()> Public Class resdiualMarketValueDataLayer

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


    Public Function get_assett_summary(ByVal searchcriteria As viewSelectionCriteriaClass, ByVal amod_id_string As String, ByVal mfr_year_string As String, ByVal divide_by_year As Boolean, ByRef stringToExclude As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        'Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim sTimeSpan As String = ""
        Dim sTimeSpanQuery As String = ""
        Dim utilizationView As New utilization_functions
        Try

            sQuery.Append(" ")
            sQuery.Append(" select distinct amod_id, amod_make_name, amod_model_name ")
            If divide_by_year = True Then
                sQuery.Append(" , ac_year  ")
            End If

            sQuery.Append(" ,  avg(afmv_value) as avg_value, ")

            sQuery.Append(" (select AVG(aires_residual) AS AVGVALUE ")
            sQuery.Append("  from Asset_Insight_Residual with (NOLOCK)  ")
            sQuery.Append("  inner join Aircraft_Flat a2 with (NOLOCK) on aires_ac_id = a2.ac_id and a2.ac_journ_id = 0  ")
            sQuery.Append(" where aires_date >= (GETDATE() + (355*5))  ") ' -- DONT GO ALL 5 years, dates may be behind that, 355 should be easily far enough
            sQuery.Append("  and a2.amod_id = Aircraft_Flat.amod_id   ")

            If Trim(mfr_year_string) <> "" Or divide_by_year = True Then
                sQuery.Append(" and a2.ac_year = Aircraft_Flat.ac_year ")
            End If

            sQuery.Append("  group by amod_id)   as avg_residual, ")

            sQuery.Append("  (left(( ")
            sQuery.Append("  (select AVG(aires_residual) AS AVGVALUE ")
            sQuery.Append(" from Asset_Insight_Residual with (NOLOCK)  ")
            sQuery.Append(" inner join Aircraft_Flat a2 with (NOLOCK) on aires_ac_id = a2.ac_id and a2.ac_journ_id = 0  ")
            sQuery.Append("  where aires_date >= (GETDATE() + (355*5))  ") ' -- DONT GO ALL 5 years, dates may be behind that, 355 should be easily far enough
            sQuery.Append(" and a2.amod_id = Aircraft_Flat.amod_id   ")

            If Trim(mfr_year_string) <> "" Or divide_by_year = True Then
                sQuery.Append(" and a2.ac_year = Aircraft_Flat.ac_year ")
            End If

            sQuery.Append(" group by amod_id) ")
            sQuery.Append(" / case when avg(afmv_value) = 0 then 1 else avg(afmv_value) end *100), 4)) as percent_of_orig_value ")
            sQuery.Append(" from Aircraft_Flat with (NOLOCK) ")
            sQuery.Append(" inner join Aircraft_FMV with (NOLOCK) on afmv_ac_id = ac_id and afmv_latest_flag='Y' and afmv_status ='Y' ")
            sQuery.Append("  where ac_journ_id = 0 ")

            If Trim(amod_id_string) <> "" And amod_id_string <> "-1" Then
                sQuery.Append(" and amod_id in (" & Trim(amod_id_string) & ") ")
            End If


            If Not IsNothing(searchcriteria.ViewCriteriaAmodIDArray) Then
                sQuery.Append(utilizationView.SetUpModelString(searchcriteria))
            ElseIf Not String.IsNullOrEmpty(searchcriteria.ViewCriteriaAircraftMake) Then
                sQuery.Append(utilizationView.SetUpMakeString(searchcriteria))
            End If

            If Not IsNothing(searchcriteria.ViewCriteriaTypeIDArray) Then
                sQuery.Append(utilizationView.SetUpTypeString(searchcriteria))
            End If



            If Trim(mfr_year_string) <> "" Then
                sQuery.Append(" and ac_year in (" & Trim(mfr_year_string) & ") ")
            End If

            If stringToExclude <> "" Then
                sQuery.Append(" and (" & stringToExclude & ")")
            End If


            sQuery.Append("  group by amod_id, amod_make_name, amod_model_name ")
            If divide_by_year = True Then
                sQuery.Append(" , ac_year  ")
            End If
            If divide_by_year = True Then
                sQuery.Append("  order by ac_year desc   ")
            Else
                sQuery.Append("  order by percent_of_orig_value desc   ")
            End If



            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

            SqlConn.ConnectionString = clientConnectString
            SqlConn.Open()

            Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)


            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                aError = "Error in get_assett_summary load datatable " + constrExc.Message
            End Try


            SqlCommand.Dispose()
            SqlCommand = Nothing

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_assett_summary(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

        End Try


        Return atemptable

    End Function


    Public Sub FillResidualGraph(ByVal split_by_year As CheckBox, ByRef residualValueChart As Label, ByVal localCriteria As viewSelectionCriteriaClass, ByVal graph_type As String, ByVal ModelID As Long, ByRef label_string As String, ByVal parentContainer As Object, ByVal graphID As Integer, ByVal ac_id As Long, ByVal client_ac_id As Long, ByVal div_height As Long, ByVal ac_dlv_year As Long, Optional ByVal show_asking As Boolean = True, Optional ByVal show_sale As Boolean = True, Optional ByVal show_evalues As Boolean = True, Optional ByVal order_by_text As String = "", Optional ByVal from_pdf As String = "N", Optional ByRef table_string As String = "", Optional ByRef count_of_records_visible As String = "", Optional ByRef ticks_string_to_return As String = "", Optional ByVal YearList As String = "", Optional ByVal forsaleFlag As String = "", Optional ByVal regType As String = "", Optional ByVal afttStart As String = "", Optional ByVal afttEnd As String = "", Optional ByRef google_map_array_list As String = "", Optional ByVal VariantList As String = "", Optional ByRef miniGraph As Boolean = False, Optional ByVal CheckForDOMLoad As Boolean = True, Optional ByRef has_info As Boolean = False, Optional ByVal stringToExclude As String = "")

        Dim htmlUtilizationGraph As String = ""
        Dim htmlUtilizationGraphScript As String = ""
        Dim htmlUtilizationFunctionScript As String = ""

        Dim htmlOut As New StringBuilder

        Dim utilization_functions As New utilization_view_functions
        ' Dim searchCriteria As New viewSelectionCriteriaClass


        localCriteria.ViewCriteriaAmodID = ModelID


        assett_residual_graph(split_by_year, residualValueChart, localCriteria, htmlUtilizationFunctionScript, htmlUtilizationGraph, graphID, "", False, False, div_height, ac_dlv_year, show_asking, show_sale, show_evalues, from_pdf, count_of_records_visible, ticks_string_to_return, Trim(graph_type), YearList, forsaleFlag, regType, afttStart, afttEnd, VariantList, miniGraph, False, stringToExclude)


        If Not IsNothing(htmlUtilizationFunctionScript) Then
            If Not String.IsNullOrEmpty(htmlUtilizationFunctionScript.Trim) Then

                htmlUtilizationGraphScript = vbCrLf + "<script type=""text/javascript"">" + vbCrLf

                If CheckForDOMLoad Then
                    htmlUtilizationGraphScript += "$(document).ready(function(){" + vbCrLf
                End If
                htmlUtilizationGraphScript += "  google.charts.setOnLoadCallback(function() {drawVisualization" + graphID.ToString + "();});" + vbCrLf

                If CheckForDOMLoad Then
                    htmlUtilizationGraphScript += "});" + vbCrLf
                End If

                htmlUtilizationGraphScript += htmlUtilizationFunctionScript.Trim
                htmlUtilizationGraphScript += ";ChangeTheMouseCursorOnItemParentDocument('standalone_page');</script>" + vbCrLf

                System.Web.UI.ScriptManager.RegisterStartupScript(parentContainer, parentContainer.GetType(), "showResGraph" + graphID.ToString, htmlUtilizationGraphScript, False)

            End If


            label_string += htmlUtilizationGraph.ToString
        End If


        google_map_array_list = htmlUtilizationFunctionScript

    End Sub

    Public Sub assett_residual_graph(ByVal split_by_year As CheckBox, ByRef residualValueChart As Label, ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_scriptString As String, ByRef out_htmlString As String, ByVal graphID As Integer, Optional ByVal faa_date As String = "", Optional ByVal bFromUtilizationTab As Boolean = False, Optional ByVal ValuePDF As Boolean = False, Optional ByVal div_height As Integer = 295, Optional ByVal ac_dlv_year As Integer = 0, Optional ByVal show_asking As Boolean = True, Optional ByVal show_sale As Boolean = True, Optional ByVal show_evaules As Boolean = True, Optional ByVal from_pdf As String = "", Optional ByRef count_of_records_visible As String = "", Optional ByRef ticks_string_to_return As String = "", Optional ByVal graph_type As String = "", Optional ByVal YearString As String = "", Optional ByVal forsaleFlag As String = "", Optional ByVal regType As String = "", Optional ByVal afttStart As String = "", Optional ByVal afttEnd As String = "", Optional ByVal VariantList As String = "", Optional ByVal miniGraph As Boolean = False, Optional ByRef has_info As Boolean = False, Optional ByRef StringToExclude As String = "")

        Dim htmlOut As New StringBuilder
        Dim scriptOut As New StringBuilder
        Dim column_scriptOut As New StringBuilder
        Dim results_table As New DataTable
        Dim results_table2 As New DataTable

        Dim x As Integer = 0

        Dim sYear As String = ""
        Dim row_added As Boolean = False


        Dim afiltered_Rows As DataRow() = Nothing
        Dim temp_low As String = ""
        Dim temp_avg As String = ""
        Dim temp_high As String = ""
        Dim temp_data As String = ""
        Dim avg_asking As String = ""
        Dim avg_sale As String = ""
        Dim high_number As Long = 0
        Dim low_number As Long = 1000000
        Dim starting_point As Integer = 0
        Dim interval_point As Integer = 1
        Dim ending_point As Integer = 0
        Dim ticks_string As String = ""
        Dim horizontal_tick_string As String = ""
        Dim last_mfr As String = ""
        Dim pre_count_nulls As Integer = 0
        Dim post_count_nulls As Integer = 0
        Dim null_max As Integer = 50
        Dim current_point As Integer = 0
        Dim first_month_ended As Boolean = False
        Dim month1 As String = ""
        Dim year1 As String = ""
        Dim last_year1 As String = ""
        Dim last_month1 As String = ""
        Dim current_rows As Integer = 0
        Dim date_of As String = ""
        Dim date_of_orig As String = ""
        Dim first_date As String = ""
        Dim last_date As String = ""
        Dim utilizationView As New utilization_view_functions

        Try

            results_table = assett_summary_by_model(split_by_year, searchCriteria, "N", 0, YearString, forsaleFlag, regType, afttStart, afttEnd, VariantList, StringToExclude)




            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then
                    residualValueChart.Visible = True
                    If Trim(from_pdf) = "Y" Then
                    Else
                        scriptOut.Append("function drawVisualization" + graphID.ToString + "() {" + vbCrLf)
                        'scriptOut.Append(" alert('drawVisualization" + graphID.ToString + "');" + vbCrLf)  
                        scriptOut.Append(" var data" + graphID.ToString + " = new google.visualization.DataTable();" + vbCrLf)
                    End If

                    scriptOut.Append("data" + graphID.ToString + ".addColumn('string', 'Month/Year'); COLUMN_SCRIPT ")

                    scriptOut.Append(" data" + graphID.ToString + ".addRows([")

                    For Each r As DataRow In results_table.Rows

                        temp_data = "null"
                        temp_low = "null"
                        temp_avg = "null"
                        temp_high = "null"
                        has_info = True


                        ' If Not IsDBNull(r("amod_make_name")) And Not IsDBNull(r("amod_model_name")) Then 
                        '   temp_data = r.Item("amod_make_name") & " " & r("amod_model_name")  
                        ' Else
                        '   temp_data = ""
                        ' End If
                        If Trim(graph_type) = "RESIDUALAC" Then
                            If Not IsDBNull(r("type_of")) Then
                                temp_data = r.Item("type_of")
                            Else
                                temp_data = "null"
                            End If
                        Else
                            If Not String.IsNullOrEmpty(YearString) Or split_by_year.Checked Then
                                If Not IsDBNull(r("ac_year")) Then
                                    temp_data = r.Item("ac_year") + " - "
                                End If
                                'ElseIf Not IsDBNull(r("amod_make_name")) Then
                                '  temp_data = ""
                            ElseIf Not IsDBNull(r("amod_model_name")) Then
                                temp_data = ""
                            End If

                            'If Not IsDBNull(r("amod_make_name")) Then
                            '  temp_data += r.Item("amod_make_name")
                            'End If
                            If Not IsDBNull(r("amod_model_name")) Then
                                temp_data += " " + r.Item("amod_model_name")
                            End If
                        End If



                        If Not IsDBNull(r("year1")) Then
                            year1 = r.Item("year1")
                        Else
                            year1 = ""
                        End If

                        If Not IsDBNull(r("month1")) Then
                            month1 = r.Item("month1")
                        Else
                            month1 = ""
                        End If

                        'date_of = "1/" & month1 & "/" & year1
                        'date_of = FormatDateTime(date_of, DateFormat.ShortDate)
                        'date_of_orig = date_of
                        'date_of = "new Date(" & year1 & ", " & (month1 - 1) & ", 1)" 

                        'Call commonEvo.set_ranges_dates(date_of_orig, first_date, last_date)


                        ' if we have changed month and year, then no longer make the columns 
                        If Trim(year1) <> Trim(last_year1) Or Trim(month1) <> Trim(last_month1) Then
                            If Trim(last_year1) <> "" Then  ' this will be different the first time in too .. so make sure 
                                first_month_ended = True
                                utilizationView.finish_script(scriptOut, current_point, null_max)
                            End If

                            current_point = 0
                            ' first time in, or after it changes, start a new one 
                            'If row_added Then
                            '  scriptOut.Append(",['" & date_of & "'")
                            'Else
                            '  scriptOut.Append("['" & date_of & "'")
                            'End If

                            If row_added Then
                                scriptOut.Append(",['" & month1 & "/" & year1 & "'")
                            Else
                                scriptOut.Append("['" & month1 & "/" & year1 & "'")
                            End If
                        End If


                        If first_month_ended = False Then
                            ' column_scriptOut.Append("data" + graphID.ToString + ".addColumn('number', '" & temp_data & " Low Residual'); ")
                            column_scriptOut.Append("data" + graphID.ToString + ".addColumn('number', '" & temp_data & "'); ")
                            ' column_scriptOut.Append("data" + graphID.ToString + ".addColumn('number', '" & temp_data & " High Residual'); ")
                            current_rows = current_rows + 1
                        End If


                        current_point = current_point + 1
                        last_year1 = year1
                        last_month1 = month1

                        If Not IsDBNull(r("LOWVALUE")) And show_evaules = True Then
                            temp_low = r.Item("LOWVALUE")
                            If CLng(temp_low) > 0 Then
                                temp_low = CLng(temp_low / 1000)
                                ' Call check_high_low(temp_low, high_number, low_number)
                            End If
                        Else
                            temp_low = "null"
                        End If

                        If Not IsDBNull(r("AVGVALUE")) And show_evaules = True Then
                            temp_avg = r.Item("AVGVALUE")
                            If CLng(temp_avg) > 0 Then
                                temp_avg = CLng(temp_avg / 1000)
                                Call utilizationView.check_high_low(temp_avg, high_number, low_number)
                            End If
                        Else
                            temp_avg = "null"
                        End If

                        If Not IsDBNull(r("HIGHVALUE")) And show_evaules = True Then
                            temp_high = r.Item("HIGHVALUE")
                            If CLng(temp_high) > 0 Then
                                temp_high = CLng(temp_high / 1000)
                                ' Call check_high_low(temp_high, high_number, low_number)
                            End If
                        Else
                            temp_high = "null"
                        End If




                        scriptOut.Append("," & temp_avg & "")
                        '  scriptOut.Append(", " & temp_low & ", " & temp_avg & ", " & temp_high & "")


                        row_added = True
                    Next

                    count_of_records_visible = current_rows

                    horizontal_tick_string = ""
                    ' commonEvo.make_ticks_string(first_date, last_date, horizontal_tick_string) 

                    utilizationView.finish_script(scriptOut, current_point, null_max) ' also ends ]

                    current_rows = current_rows + 1
                    utilizationView.finish_column_script(column_scriptOut, current_rows, null_max, graphID)




                    ticks_string = "Y" ' so that is does the extra build
                    commonEvo.set_ranges_for_vsCharts(low_number, high_number, interval_point, starting_point, ending_point, ticks_string)


                    If Trim(from_pdf) = "Y" Then
                    Else
                        scriptOut.Append("]);" + vbCrLf)

                        scriptOut.Append("var options = { " + vbCrLf)
                        scriptOut.Append("  chartArea:{width:'" & IIf(miniGraph, "80", "85") & "%',height:'" & IIf(miniGraph, "68", "95") & "%'}," + vbCrLf)


                        scriptOut.Append("series: { ")


                        If Trim(graph_type) = "RESIDUALAC" Then
                            scriptOut.Append("    0: { lineWidth: 2, pointSize: 2  } ")
                            scriptOut.Append(" ,  1: { lineWidth: 1, pointSize: 1  , lineDashStyle: [4, 4] } ")

                            For i = 2 To null_max
                                scriptOut.Append(",  " & i & ": { lineWidth: 1, pointSize: 1  , lineDashStyle: [4, 4], visibleInLegend: false } ")
                            Next
                        Else
                            scriptOut.Append(" 0: { lineWidth: 1, pointSize: 1  } ")   ' , lineDashStyle: [4, 4]

                            For i = 1 To current_rows - 2
                                scriptOut.Append(",  " & i & ": { lineWidth: 1, pointSize: 1  } ")   '  , lineDashStyle: [4, 4]
                            Next

                            For i = current_rows - 1 To null_max
                                scriptOut.Append(",  " & i & ": { lineWidth: 1, pointSize: 1  , lineDashStyle: [4, 4], visibleInLegend: false } ")
                            Next
                        End If


                        'scriptOut.Append("    0: { lineWidth: 1, pointSize: 1  , lineDashStyle: [4, 4] } ")
                        'scriptOut.Append(" ,  1: { lineWidth: 2, pointSize: 2  , lineDashStyle: [4, 4] } ")
                        'scriptOut.Append(" ,  2: { lineWidth: 1, pointSize: 1  , lineDashStyle: [4, 4] } ")


                        scriptOut.Append("  }  , ")

                        scriptOut.Append("  hAxis: { title: 'Month/Year'," + vbCrLf)
                        scriptOut.Append("           textStyle: { fontSize: " & IIf(miniGraph, "8", "10") & ", bold: false, italic: false }, " + vbCrLf)  ', fontName:  'Arial'  , color: '#01579b', 
                        scriptOut.Append("           titleTextStyle: { fontSize: " & IIf(miniGraph, "8", "15") & ", bold: false, italic: false }" + vbCrLf)
                        If Trim(horizontal_tick_string) <> "" Then
                            scriptOut.Append(", ticks: [ " & horizontal_tick_string & "] ")
                        End If
                        scriptOut.Append("         }," + vbCrLf)
                        scriptOut.Append("  vAxis: { title: 'Price ($k)'," + vbCrLf)
                        scriptOut.Append("           textStyle: {  fontSize: " & IIf(miniGraph, "8", "10") & ", bold: false, italic: false }," + vbCrLf)  '' color: '#1a237e',
                        scriptOut.Append("           titleTextStyle: {  fontSize: " & IIf(miniGraph, "8", "15") & ", bold: false, italic: false }" + vbCrLf)
                        If Trim(ticks_string) <> "" Then
                            scriptOut.Append(", ticks: [ " & ticks_string & "] ")
                        End If
                        scriptOut.Append("        }," + vbCrLf)
                        scriptOut.Append("  smoothLine:true," + vbCrLf)
                        scriptOut.Append(" legend: { position: 'top', textStyle:{fontSize:" & IIf(miniGraph, "8", "9") & "}}, " + vbCrLf)


                        ' scriptOut.Append("  colors: ['#92b0c4','#B7DCF6', '#B7DCF6', '#B7B7B7', '#B7B7B7', '#B7B7B7', '#a3c28d', '#eba059', '#a84543', 'B7DCF6', '#a3c28d', '#eba059', '#a84543']" + vbCrLf)
                        scriptOut.Append("};" + vbCrLf)



                        scriptOut.Append(" var chartVis = new google.visualization.LineChart(document.getElementById('visualization" + graphID.ToString + "'));" + vbCrLf)
                        scriptOut.Append(" chartVis.draw(data" + graphID.ToString + ", options);" + vbCrLf)

                        If ValuePDF Then
                            scriptOut.Append(" document.getElementById('ctl00_ContentPlaceHolder1_visualizationPNG1').innerHTML = '<img src=""' + chartVis.getImageURI() + '"" >'" + vbCrLf)
                            'scriptOut.Append("$(""#visualization1"").addClass(""display_none"");" + vbCrLf)
                            '
                        End If

                        scriptOut.Append("}" + vbCrLf)
                    End If

                End If

            End If

            If Not String.IsNullOrEmpty(scriptOut.ToString.Trim) Then
                htmlOut.Append("<table id=""flightActivityTable"" width=""100%"" cellspacing=""0"" cellpadding=""0"">")
                htmlOut.Append("<tr><td valign=""top"" align=""left""><div id='visualization" + graphID.ToString + "' style=""height:333px;margin-top:-10px;margin-bottom:-15px;""></div></td></tr>")
                htmlOut.Append("</table>" + vbCrLf)
            Else
                htmlOut.Append("<table id=""flightActivityTable"" width=""100%"" cellspacing=""0"" cellpadding=""4"">")
                htmlOut.Append("<tr><td valign=""middle"" align=""center"">No Residuals Found For This Aircraft ...</td></tr>")
                htmlOut.Append("</table>" + vbCrLf)
            End If

        Catch ex As Exception

            aError = "Error in views_display_flight_utilization_graph(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, Optional ByVal faa_date As String = """") " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_scriptString = scriptOut.ToString
        out_scriptString = Replace(out_scriptString, "COLUMN_SCRIPT", column_scriptOut.ToString)

        out_htmlString = htmlOut.ToString
        ticks_string_to_return = ticks_string


        htmlOut = Nothing
        results_table = Nothing
    End Sub

    Public Function assett_summary_by_model(ByVal split_by_year As CheckBox, ByRef searchCriteria As viewSelectionCriteriaClass, ByVal get_acs_dlv_year As String, ByVal ac_dlv_year As Integer, Optional ByVal YearString As String = "", Optional ByVal forsaleFlag As String = "", Optional ByVal regType As String = "", Optional ByVal afttStart As String = "", Optional ByVal afttEnd As String = "", Optional ByVal VariantList As String = "", Optional ByRef StringToExclude As String = "") As DataTable


        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        'Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim sTimeSpan As String = ""
        Dim sTimeSpanQuery As String = ""
        Dim utilizationView As New utilization_functions
        Try



            sQuery.Append(" select distinct 'future_month'  as type_of,  ")

            sQuery.Append(" YEAR(aires_date) as year1, MONTH(aires_date) as month1,  " & IIf((Not String.IsNullOrEmpty(YearString) Or split_by_year.Checked), "ac_year,", "") & " amod_make_name, amod_model_name, ")
            sQuery.Append(" min(aires_residual) AS LOWVALUE, AVG(aires_residual) AS AVGVALUE, MAX(aires_residual) AS HIGHVALUE,COUNT(*) as TOTVALUES, 0.0 as asking_price, 0.0 as take_price, 0.0 as sale_price ")

            sQuery.Append(" from Asset_Insight_Residual with (NOLOCK) ")
            sQuery.Append(" inner join Aircraft_Flat with (NOLOCK) on aires_ac_id = ac_id and ac_journ_id = 0 ")

            sQuery.Append(" where ")
            Dim startDate As New Date
            Dim endDate As New Date
            endDate = DateAdd(DateInterval.Month, 59, Now())
            startDate = DateAdd(DateInterval.Month, 1, Now())

            'If Date.Now.Month = 12 Then
            '  sQuery.Append(" aires_date >= cast('1/1/' + cast(" & (Date.Now.Year + 1) & " as varchar(30)) ")
            'Else
            sQuery.Append(" aires_date >= cast(" & (startDate.Month) & " as varchar(30)) + '/1/' + cast(" & startDate.Year & " as varchar(30)) ")
            sQuery.Append(" and aires_date <= cast(" & (endDate.Month) & " as varchar(30)) + '/1/' + cast(" & endDate.Year & " as varchar(30)) ")
            'End If


            '-- YEAR RANGE
            If Not String.IsNullOrEmpty(YearString) Then
                sQuery.Append(" and ac_year in (" & YearString & ")")
            End If


            If Not String.IsNullOrEmpty(VariantList) Then
                sQuery.Append(" and amod_id in (" & VariantList & ")")
            ElseIf searchCriteria.ViewCriteriaAmodID > 0 Then
                sQuery.Append(" and amod_id = @amodID")
            End If

            If Not IsNothing(searchCriteria.ViewCriteriaAmodIDArray) Then
                sQuery.Append(utilizationView.SetUpModelString(searchCriteria))
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake) Then
                sQuery.Append(utilizationView.SetUpMakeString(searchCriteria))
            End If

            If Not IsNothing(searchCriteria.ViewCriteriaTypeIDArray) Then
                sQuery.Append(utilizationView.SetUpTypeString(searchCriteria))
            End If

            If StringToExclude <> "" Then
                sQuery.Append(" and (" & StringToExclude & ")")
            End If

            sQuery.Append(" group by  YEAR(aires_date), MONTH(aires_date) ,   " & IIf(Not String.IsNullOrEmpty(YearString) Or split_by_year.Checked, "ac_year,", "") & " amod_make_name, amod_model_name ")
            sQuery.Append(" order by YEAR(aires_date) asc, MONTH(aires_date) asc ,   " & IIf(Not String.IsNullOrEmpty(YearString) Or split_by_year.Checked, "ac_year,", "") & " amod_make_name, amod_model_name  ")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)


            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase").ToString
            SqlConn.Open()

            Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)
            SqlCommand.Parameters.AddWithValue("amodID", searchCriteria.ViewCriteriaAmodID)
            'SqlCommand.Parameters.AddWithValue("startAFTT", afttStart)
            'SqlCommand.Parameters.AddWithValue("endAFTT", afttEnd)
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)


            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                aError = "Error in get_residual_assett_summary load datatable " + constrExc.Message
            End Try



            SqlReader.Close()
            SqlReader = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing


        Catch ex As Exception
            Return Nothing

            aError = "Error in get_residual_assett_summary(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

        Finally

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

        End Try

        Return atemptable


    End Function
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

End Class
