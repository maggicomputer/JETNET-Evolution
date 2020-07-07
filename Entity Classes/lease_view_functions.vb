' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/lease_view_functions.vb $
'$$Author: Amanda $
'$$Date: 8/30/19 11:05a $
'$$Modtime: 8/30/19 10:19a $
'$$Revision: 4 $
'$$Workfile: lease_view_functions.vb $
'
' ********************************************************************************

<System.Serializable()> Public Class lease_view_functions

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

#Region "lease_functions"

    Public Function get_leases_by_month(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal use_roll As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT YEAR(journ_date) AS tYear, MONTH(journ_date) AS tMonth, count(*) AS tCount")
            sQuery.Append(" FROM Journal WITH(NOLOCK) INNER JOIN Aircraft WITH(NOLOCK) ON journ_ac_id = ac_id and journ_id = ac_journ_id")
            sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id")
            sQuery.Append(" INNER JOIN Aircraft_Reference WITH(NOLOCK) ON cref_ac_id = ac_id AND cref_journ_id = ac_journ_id AND cref_contact_type IN ('13','57')")


            sQuery.Append(" INNER JOIN Company WITH(NOLOCK) ON comp_id = cref_comp_id AND comp_journ_id = cref_journ_id")

            If searchCriteria.ViewID = 4 Then
                ' "check" "company location information"
                If searchCriteria.ViewCriteriaHasCompanyLocationInfo Then

                    sQuery.Append(" LEFT OUTER JOIN State WITH(NOLOCK) ON state_code = comp_state")

                    sQuery.Append(commonEvo.check_selected_geography_from_dropdowns(IIf(searchCriteria.ViewCriteriaUseContinent, "continent", "region"), searchCriteria.ViewCriteriaContinent, searchCriteria.ViewCriteriaCountry, searchCriteria.ViewCriteriaState, False))

                    If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaTimeZone) Then
                        sQuery.Append(Constants.cAndClause + "comp_timezone IN ('" + commonEvo.TranslateTimeZone(searchCriteria.ViewCriteriaTimeZone).Replace(Constants.cCommaDelim, Constants.cValueSeperator) + "')")
                    End If

                End If

            End If

            If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsStartDate) And Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsEndDate) Then
                sQuery.Append(" WHERE ((journ_date >= '" + searchCriteria.ViewCriteriaDocumentsStartDate + "') AND (journ_date < '" + searchCriteria.ViewCriteriaDocumentsEndDate + "'))")
            Else
                sQuery.Append(" WHERE ((journ_date >= '" + Month(DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now())).ToString + "/01/" + Year(DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now())).ToString + "')")
                sQuery.Append(Constants.cAndClause + "(journ_date < '" + Now.Month.ToString + "/01/" + Now.Year.ToString + "'))")
            End If

            sQuery.Append(Constants.cAndClause + "journ_subcat_code_part1 LIKE 'L%' AND journ_subcat_code_part2 NOT IN ('CO') AND journ_subcat_code_part3 NOT IN ('IT', 'RR')")

            If searchCriteria.ViewCriteriaAmodID > -1 Then
                sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
            End If
            If searchCriteria.ViewCriteriaCompanyID > 0 Then
                If Trim(use_roll) = "Y" Then
                    sQuery.Append(" and comp_id in (select distinct RelCompID from ReturnAllCompanyLocationsByCompId(" & searchCriteria.ViewCriteriaCompanyID & ")) ")
                Else
                    sQuery.Append(Constants.cAndClause + "comp_id = " + searchCriteria.ViewCriteriaCompanyID.ToString)
                End If
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
                sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, False))
            End If

            sQuery.Append(" GROUP BY YEAR(journ_date), month(journ_date) ORDER BY YEAR(journ_date), month(journ_date)")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_leases_by_month(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
                aError = "Error in get_leases_by_month load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_leases_by_month(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Sub views_display_leases_by_month_chart(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByRef LEASES_SOLD_PER_MONTH As DataVisualization.Charting.Chart)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder

        Dim high_number As Integer = 0
        Dim low_number As Integer = 0
        Dim starting_point As Integer = 0
        Dim interval_point As Integer = 1

        Dim sTmpTitle As String = ""

        Dim imgCnt As Integer = 0

        Dim sImageMapPath As String = ""
        Dim sImageSrc As String = ""
        Dim sImageName As String = ""

        Dim subscriptionInfo As String = HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString + "_" + HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString.Trim + "_" + HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString + "_"
        Dim displayFolder As String = HttpContext.Current.Application.Item("crmClientSiteData").ClientFullHostName + HttpContext.Current.Session.Item("MarketSummaryFolderVirtualPath")

        Try

            results_table = get_leases_by_month(searchCriteria, "N")

            LEASES_SOLD_PER_MONTH.Series.Clear()
            LEASES_SOLD_PER_MONTH.Series.Add("LEASES_PER_MONTH").ChartType = UI.DataVisualization.Charting.SeriesChartType.Column
            LEASES_SOLD_PER_MONTH.Series("LEASES_PER_MONTH").YValueType = UI.DataVisualization.Charting.ChartValueType.Int32
            LEASES_SOLD_PER_MONTH.Series("LEASES_PER_MONTH").LabelForeColor = Drawing.Color.Blue
            LEASES_SOLD_PER_MONTH.Series("LEASES_PER_MONTH").Color = Drawing.Color.Blue
            LEASES_SOLD_PER_MONTH.Series("LEASES_PER_MONTH").BorderWidth = 1
            LEASES_SOLD_PER_MONTH.Series("LEASES_PER_MONTH").MarkerSize = 5
            LEASES_SOLD_PER_MONTH.Series("LEASES_PER_MONTH").MarkerStyle = UI.DataVisualization.Charting.MarkerStyle.Circle

            LEASES_SOLD_PER_MONTH.ChartAreas("ChartArea1").AxisY.Title = "Leasses"
            LEASES_SOLD_PER_MONTH.ChartAreas("ChartArea1").AxisX.Title = "Month"

            If searchCriteria.ViewID = 4 Then
                LEASES_SOLD_PER_MONTH.Width = 400
                LEASES_SOLD_PER_MONTH.Height = 350
            Else
                LEASES_SOLD_PER_MONTH.Width = 260
                LEASES_SOLD_PER_MONTH.Height = 260
            End If


            htmlOut.Append("<table id=""leaseChartOuterTable"" width=""100%"" cellpadding=""2"" cellspacing=""0"">")

            If Not IsNothing(results_table) Then

                If searchCriteria.ViewCriteriaAmodID > -1 And searchCriteria.ViewCriteriaCompanyID > 0 Then
                    sTmpTitle = "<br /><em>" + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, "") + " : " + commonEvo.get_company_name_fromID(searchCriteria.ViewCriteriaCompanyID, 0, False, True, "") + "</em>"
                ElseIf searchCriteria.ViewCriteriaCompanyID > 0 Then
                    sTmpTitle = "<br /><em>" + commonEvo.get_company_name_fromID(searchCriteria.ViewCriteriaCompanyID, 0, False, True, "") + "</em>"
                ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then
                    sTmpTitle = "<br /><em>" + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, "") + "</em>"
                Else
                    sTmpTitle = "<br /><em>All Leased Aircraft</em>"
                End If

                If results_table.Rows.Count > 0 Then
                    htmlOut.Append("<tr><td align=""center"" valign=""middle"" class=""header"">LEASES PER MONTH (<em>last " + searchCriteria.ViewCriteriaTimeSpan.ToString + " months</em>) " + sTmpTitle.Trim + "</td></tr>")
                Else
                    htmlOut.Append("<tr><td align=""center"" valign=""middle"" class=""header"">NO LEASES PER MONTH (<em>last " + searchCriteria.ViewCriteriaTimeSpan.ToString + " months</em>) " + sTmpTitle.Trim + "</td></tr>")
                End If

                If results_table.Rows.Count > 0 Then

                    For Each r As DataRow In results_table.Rows

                        If CLng(r.Item("tcount").ToString) > 0 Then

                            If high_number = 0 Or CDbl(r.Item("tCount").ToString) > high_number Then
                                high_number = CDbl(r.Item("tCount").ToString)
                            End If

                            If low_number = 0 Or CDbl(r.Item("tCount").ToString) < low_number Then
                                low_number = CDbl(r.Item("tCount").ToString)
                            End If

                            LEASES_SOLD_PER_MONTH.Series("LEASES_PER_MONTH").Points.AddXY((r.Item("tMonth").ToString + "-" + r.Item("tYear").ToString), CDbl(r.Item("tCount").ToString))

                        End If

                    Next

                    LEASES_SOLD_PER_MONTH.ChartAreas("ChartArea1").AxisY.Maximum = high_number + Math.Round((high_number / 10), 0)
                    LEASES_SOLD_PER_MONTH.ChartAreas("ChartArea1").AxisY.Minimum = 0

                    If high_number >= 100 Then
                        LEASES_SOLD_PER_MONTH.ChartAreas("ChartArea1").AxisY.Interval = Math.Round((high_number / 10), 0)
                    Else
                        LEASES_SOLD_PER_MONTH.ChartAreas("ChartArea1").AxisY.Interval = 10
                    End If

                    LEASES_SOLD_PER_MONTH.Titles.Clear()
                    LEASES_SOLD_PER_MONTH.Titles.Add("Leases Per Month (past " + searchCriteria.ViewCriteriaTimeSpan.ToString + " months)")

                    imgCnt += 1
                    sImageName = subscriptionInfo + commonEvo.GenerateFileName("image_" + imgCnt.ToString, ".jpg", False)
                    sImageMapPath = HttpContext.Current.Server.MapPath(HttpContext.Current.Session.Item("MarketSummaryFolderVirtualPath")) + "\" + sImageName
                    sImageSrc = displayFolder + "/" + sImageName

                    LEASES_SOLD_PER_MONTH.ImageType = DataVisualization.Charting.ChartImageType.Jpeg
                    LEASES_SOLD_PER_MONTH.SaveImage(sImageMapPath, DataVisualization.Charting.ChartImageFormat.Jpeg)

                    htmlOut.Append("<tr><td align=""center"" valign=""middle"" class=""seperator""><img src=""" + sImageSrc + """ title=""Leases Per Month (past " + searchCriteria.ViewCriteriaTimeSpan.ToString + " months)""></td></tr>")

                Else
                    htmlOut.Append("<tr><td valign=""middle"" align=""left"" class=""seperator"" style=""padding-left:3px;""><br />No leases could be found that meet your search criteria</td></tr>")
                End If
            Else
                htmlOut.Append("<tr><td valign=""middle"" align=""left"" class=""seperator"" style=""padding-left:3px;""><br />No leases could be found that meet your search criteria</td></tr>")
            End If

            htmlOut.Append("</table>")

        Catch ex As Exception

            aError = "Error in views_display_leases_by_month_chart(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, ByRef LEASES_SOLD_PER_MONTH As DataVisualization.Charting.Chart) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function get_leases_expired(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal bIsGetLastExpired As Boolean) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try
            If bIsGetLastExpired Then
                sQuery.Append("SELECT TOP 1 amod_make_name, amod_model_name, ac_ser_no_full, ac_reg_no, ac_id, aclease_expiration_date, amod_id, journ_subject, journ_id")
            Else
                sQuery.Append("SELECT amod_make_name, amod_model_name, ac_ser_no_full, ac_reg_no, ac_id, aclease_expiration_date, amod_id, journ_subject, journ_id")
            End If

            sQuery.Append(" FROM aircraft WITH(NOLOCK) INNER JOIN Aircraft_Lease WITH(NOLOCK) ON ac_id = aclease_ac_id and ac_journ_id = aclease_journ_id")
            sQuery.Append(" INNER JOIN aircraft_model WITH(NOLOCK) ON ac_amod_id = amod_id")
            sQuery.Append(" INNER JOIN aircraft_reference WITH(NOLOCK) ON ac_id=cref_ac_id and ac_journ_id = cref_journ_id")

            sQuery.Append(" LEFT OUTER JOIN Company WITH(NOLOCK) ON cref_comp_id = comp_id and cref_journ_id = comp_journ_id")

            If searchCriteria.ViewID = 4 Then
                ' "check" "company location information"
                If searchCriteria.ViewCriteriaHasCompanyLocationInfo Then

                    sQuery.Append(" LEFT OUTER JOIN State WITH(NOLOCK) ON state_code = comp_state")

                    sQuery.Append(commonEvo.check_selected_geography_from_dropdowns(IIf(searchCriteria.ViewCriteriaUseContinent, "continent", "region"), searchCriteria.ViewCriteriaContinent, searchCriteria.ViewCriteriaCountry, searchCriteria.ViewCriteriaState, False))

                    If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaTimeZone) Then
                        sQuery.Append(Constants.cAndClause + "comp_timezone IN ('" + commonEvo.TranslateTimeZone(searchCriteria.ViewCriteriaTimeZone).Replace(Constants.cCommaDelim, Constants.cValueSeperator) + "')")
                    End If

                End If

            End If

            sQuery.Append(" INNER JOIN journal WITH(NOLOCK) ON ac_journ_id = journ_id")

            sQuery.Append(" WHERE ac_journ_id > 0 AND aclease_expired = 'Y' AND cref_contact_type in ('13', '57')")

            If bIsGetLastExpired Then
                sQuery.Append(Constants.cAndClause + "(aclease_expiration_date < '" + Now.Month.ToString + "/01/" + Now.Year.ToString + "')")
            Else
                If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsStartDate) And Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsEndDate) Then
                    sQuery.Append(Constants.cAndClause + "((aclease_expiration_date >= '" + searchCriteria.ViewCriteriaDocumentsStartDate + "') AND (aclease_expiration_date < '" + searchCriteria.ViewCriteriaDocumentsEndDate + "'))")
                Else
                    sQuery.Append(Constants.cAndClause + "((aclease_expiration_date >= '" + Month(DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now())).ToString + "/01/" + Year(DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now())).ToString + "')")
                    sQuery.Append(Constants.cAndClause + "(aclease_expiration_date < '" + Now.Month.ToString + "/01/" + Now.Year.ToString + "'))")
                End If
            End If

            If searchCriteria.ViewCriteriaAmodID > -1 Then
                sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
            End If

            If searchCriteria.ViewCriteriaCompanyID > 0 Then
                sQuery.Append(Constants.cAndClause + "comp_id = " + searchCriteria.ViewCriteriaCompanyID.ToString)
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
                sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, False))
            End If

            If bIsGetLastExpired Then
                sQuery.Append(" ORDER BY aclease_expiration_date DESC, amod_make_name, amod_model_name")
            Else
                sQuery.Append(" ORDER BY aclease_expiration_date ASC, ac_ser_no_full, amod_make_name, amod_model_name")
            End If

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_leases_expired(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal bIsGetLastExpired As Boolean) As DataTable</b><br />" + sQuery.ToString

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
                aError = "Error in get_leases_expired load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_leases_expired(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal bIsGetLastExpired As Boolean) As DataTable " + ex.Message

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

    Public Sub views_display_leases_expired(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal bGetLastLease As Boolean, ByRef out_htmlString As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Dim sTitle As String = ""

        Try

            results_table = get_leases_expired(searchCriteria, bGetLastLease)

            If bGetLastLease Then
                sTitle = "No Leases Expired In Last " + searchCriteria.ViewCriteriaTimeSpan.ToString + " Months<br /><em>Showing last lease to expire</em>"
            Else
                If searchCriteria.ViewCriteriaAmodID = -1 Then
                    sTitle = "ALL MAKES/MODELS<br />"
                End If
                sTitle += "From: " + Month(DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now)).ToString + "/01/" + Year(DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now)).ToString + " Up to: " + Now.Month.ToString + "/01/" + Now.Year.ToString
            End If

            htmlOut.Append("<table id='displayExpiredLeasesOuterTable' width='100%' cellpadding='0' cellspacing='0' class='module'>")

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then
                    htmlOut.Append("<tr><td valign='top' align='center' class='header'>LEASES EXPIRED (<em>last " + searchCriteria.ViewCriteriaTimeSpan.ToString + " months (" + results_table.Rows.Count.ToString + ")</em>)</td></tr>")
                Else
                    htmlOut.Append("<tr><td valign='top' align='center' class='header'>NO LEASES HAVE EXPIRED (<em>last " + searchCriteria.ViewCriteriaTimeSpan.ToString + " months</em>)</td></tr>")
                End If

                If results_table.Rows.Count > 0 Then

                    htmlOut.Append("<tr><td valign='top' align='left'><table id='displayExpiredLeasesInnerTable' width='100%' cellpadding='2' cellspacing='0'>")
                    htmlOut.Append("<tr><td valign='top' align='left' class='tabheader'><strong>" + sTitle.Trim + "</strong></td><td class='border_bottom' width='2%'>&nbsp;</td></tr>")
                    htmlOut.Append("<tr><td class='rightside' colspan='2'>")

                    If results_table.Rows.Count > 5 Then
                        htmlOut.Append("<div valign=""top"" style=""height:270px; overflow: auto;""><p>")
                    End If

                    htmlOut.Append("<table id='displayExpiredLeasesDataTable' width='100%' cellpadding='4' cellspacing='0'>")

                    For Each r As DataRow In results_table.Rows

                        If Not toggleRowColor Then
                            htmlOut.Append("<tr class='alt_row'>")
                            toggleRowColor = True
                        Else
                            htmlOut.Append("<tr bgcolor='white'>")
                            toggleRowColor = False
                        End If

                        htmlOut.Append("<td valign='top' align='left' width='5%' class='seperator'><img src='images/ch_red.jpg' class='bullet' alt='acid : " + r.Item("ac_id").ToString + "' /></td>")
                        htmlOut.Append("<td valign='top' align='left' class='seperator'>")

                        If searchCriteria.ViewCriteriaAmodID = -1 Then
                            htmlOut.Append("<em>" + r.Item("amod_make_name").ToString + " " + r.Item("amod_model_name").ToString + "</em>, ")
                        End If

                        htmlOut.Append(" Serial# <a class='underline' onclick=""javascript:load('DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "&jid=0','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title='Display Aircraft Details'>")
                        htmlOut.Append(r.Item("ac_ser_no_full").ToString + "</a>")
                        htmlOut.Append(", Reg# " + r.Item("ac_reg_no").ToString + " &nbsp;-&nbsp;<b>Expired on: " + FormatDateTime(r.Item("aclease_expiration_date").ToString, DateFormat.GeneralDate) + "</b>")
                        htmlOut.Append("<br />")
                        htmlOut.Append(r.Item("journ_subject").ToString + "</td></tr>")

                    Next

                    htmlOut.Append("</table>")

                    If results_table.Rows.Count > 5 Then
                        htmlOut.Append("</p></div>")
                    End If

                    htmlOut.Append("</td></tr></table></td></tr>")

                Else
                    htmlOut.Append("<tr><td valign='top' align='left' class='seperator' style='padding-left:3px;'><br />No leases could be found that meet your search criteria</td></tr>")
                End If

            Else
                htmlOut.Append("<tr><td valign='top' align='left' class='seperator' style='padding-left:3px;'><br />No leases could be found that meet your search criteria</td></tr>")
            End If

            htmlOut.Append("</table>")

        Catch ex As Exception

            aError = "Error in views_display_leases_expired(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal bGetLastLease As Boolean, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function get_leases_due_to_expire(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal bIsGetNextExpiring As Boolean) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            If bIsGetNextExpiring Then
                sQuery.Append("SELECT TOP 1 amod_make_name, amod_model_name, ac_ser_no_full, ac_reg_no, ac_id, aclease_expiration_date, amod_id, journ_subject, journ_id")
            Else
                sQuery.Append("SELECT amod_make_name, amod_model_name, ac_ser_no_full, ac_reg_no, ac_id, aclease_expiration_date, amod_id, journ_subject, journ_id")
            End If

            sQuery.Append(" FROM aircraft WITH(NOLOCK) INNER JOIN Aircraft_Lease WITH(NOLOCK) ON ac_id = aclease_ac_id and ac_journ_id = aclease_journ_id")
            sQuery.Append(" INNER JOIN aircraft_model WITH(NOLOCK) ON ac_amod_id = amod_id")
            sQuery.Append(" INNER JOIN aircraft_reference WITH(NOLOCK) ON ac_id=cref_ac_id and ac_journ_id = cref_journ_id")

            sQuery.Append(" LEFT OUTER JOIN Company WITH(NOLOCK) ON cref_comp_id = comp_id and cref_journ_id = comp_journ_id")

            If searchCriteria.ViewID = 4 Then
                ' "check" "company location information"
                If searchCriteria.ViewCriteriaHasCompanyLocationInfo Then

                    sQuery.Append(" LEFT OUTER JOIN State WITH(NOLOCK) ON state_code = comp_state")

                    sQuery.Append(commonEvo.check_selected_geography_from_dropdowns(IIf(searchCriteria.ViewCriteriaUseContinent, "continent", "region"), searchCriteria.ViewCriteriaContinent, searchCriteria.ViewCriteriaCountry, searchCriteria.ViewCriteriaState, False))

                    If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaTimeZone) Then
                        sQuery.Append(Constants.cAndClause + "comp_timezone IN ('" + commonEvo.TranslateTimeZone(searchCriteria.ViewCriteriaTimeZone).Replace(Constants.cCommaDelim, Constants.cValueSeperator) + "')")
                    End If

                End If

            End If

            sQuery.Append(" INNER JOIN journal WITH(NOLOCK) ON ac_journ_id = journ_id")

            sQuery.Append(" WHERE ac_journ_id > 0 AND aclease_expired = 'N' AND cref_contact_type in ('13', '57')")

            If bIsGetNextExpiring Then
                sQuery.Append(Constants.cAndClause + "(aclease_expiration_date > '" + Now.Month.ToString & "/01/" + Now.Year.ToString + "')")
            Else
                If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsStartDate) And Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaDocumentsEndDate) Then
                    sQuery.Append(Constants.cAndClause + "((aclease_expiration_date >= '" + searchCriteria.ViewCriteriaDocumentsStartDate + "') AND (aclease_expiration_date < '" + searchCriteria.ViewCriteriaDocumentsEndDate + "'))")
                Else
                    sQuery.Append(Constants.cAndClause + "((aclease_expiration_date >= '" + Now.Month.ToString + "/01/" + Now.Year.ToString + "')")
                    sQuery.Append(Constants.cAndClause + "(aclease_expiration_date < '" + Month(DateAdd("m", searchCriteria.ViewCriteriaTimeSpan, Now())).ToString + "/01/" + Year(DateAdd("m", searchCriteria.ViewCriteriaTimeSpan, Now())).ToString + "'))")
                End If
            End If

            If searchCriteria.ViewCriteriaAmodID > -1 Then
                sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
            End If

            If searchCriteria.ViewCriteriaCompanyID > 0 Then
                sQuery.Append(Constants.cAndClause + "comp_id = " + searchCriteria.ViewCriteriaCompanyID.ToString)
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
                sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, False))
            End If

            If bIsGetNextExpiring Then
                sQuery.Append(" ORDER BY aclease_expiration_date ASC, amod_make_name, amod_model_name")
            Else
                sQuery.Append(" ORDER BY aclease_expiration_date ASC, ac_ser_no_full, amod_make_name, amod_model_name")
            End If

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_leases_due_to_expire(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal bIsGetNextExpiring As Boolean) As DataTable</b><br />" + sQuery.ToString

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
                aError = "Error in get_leases_due_to_expire load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_leases_due_to_expire(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal bIsGetNextExpiring As Boolean) As DataTable " + ex.Message

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

    Public Sub views_display_leases_due_to_expire(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal bGetNextLease As Boolean, ByRef out_htmlString As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Dim sTitle As String = ""

        Try

            results_table = get_leases_due_to_expire(searchCriteria, bGetNextLease)

            If bGetNextLease Then
                sTitle = "No Leases Expiring in Next " + searchCriteria.ViewCriteriaTimeSpan.ToString + " Month(s)<br /><em>Showing next lease to expire</em>"
            Else
                If searchCriteria.ViewCriteriaAmodID = -1 Then
                    sTitle = "ALL MAKES/MODELS<br />"
                End If
                sTitle += "From: " + Now.Month.ToString + "/01/" + Now.Year.ToString + " Up to: " + Month(DateAdd("m", searchCriteria.ViewCriteriaTimeSpan, Now)).ToString + "/01/" + Year(DateAdd("m", searchCriteria.ViewCriteriaTimeSpan, Now)).ToString
            End If

            htmlOut.Append("<table id='leasesDueToExpireOuterTable' width='100%' cellpadding='0' cellspacing='0' class='module'>")

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then
                    htmlOut.Append("<tr><td valign='top' align='center' class='header'>LEASES DUE TO EXPIRE <em>(next " + searchCriteria.ViewCriteriaTimeSpan.ToString + " months (" + results_table.Rows.Count.ToString + "))</em></td></tr>")
                Else
                    htmlOut.Append("<tr><td valign='top' align='center' class='header'>NO LEASES DUE TO EXPIRE <em>(next " + searchCriteria.ViewCriteriaTimeSpan.ToString + " months)</em></td></tr>")
                End If

                If results_table.Rows.Count > 0 Then

                    htmlOut.Append("<tr><td valign='top' align='left'><table id='leasesDueToExpireInnerTable' width='100%' cellpadding='2' cellspacing='0'>")
                    htmlOut.Append("<tr><td valign='top' align='left' class='tabheader'><strong>" + sTitle.Trim + "</strong></td><td class='border_bottom' width='2%'>&nbsp;</td></tr>")
                    htmlOut.Append("<tr><td class='rightside' colspan='2'>")

                    If results_table.Rows.Count > 5 Then
                        htmlOut.Append("<div valign=""top"" style='height:270px; overflow: auto;'><p>")
                    End If

                    htmlOut.Append("<table id='leasesDueToExpireDataTable' width='100%' cellpadding='4' cellspacing='0'>")

                    For Each r As DataRow In results_table.Rows

                        If Not toggleRowColor Then
                            htmlOut.Append("<tr class='alt_row'>")
                            toggleRowColor = True
                        Else
                            htmlOut.Append("<tr bgcolor='white'>")
                            toggleRowColor = False
                        End If

                        htmlOut.Append("<td valign='top' align='left' width='5%' class='seperator'><img src='images/ch_red.jpg' class='bullet' alt='acid : " + r.Item("ac_id").ToString + "' /></td>")
                        htmlOut.Append("<td valign='top' align='left' class='seperator'>")

                        If searchCriteria.ViewCriteriaAmodID = -1 Then
                            htmlOut.Append("<em>" + r.Item("amod_make_name").ToString + " " + r.Item("amod_model_name").ToString + "</em>, ")
                        End If

                        htmlOut.Append(" Serial# <a class='underline' onclick=""javascript:load('DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "&jid=0','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title='Display Aircraft Details'>")
                        htmlOut.Append(r.Item("ac_ser_no_full").ToString + "</a>")
                        htmlOut.Append(", Reg# " + r.Item("ac_reg_no").ToString + " &nbsp;-&nbsp;<b>Expired on: " + FormatDateTime(r.Item("aclease_expiration_date").ToString, DateFormat.GeneralDate) + "</b>")
                        htmlOut.Append("<br />")
                        htmlOut.Append(r.Item("journ_subject").ToString + "</td></tr>")

                    Next

                    htmlOut.Append("</table>")
                    If results_table.Rows.Count > 5 Then
                        htmlOut.Append("</p></div>")
                    End If

                    htmlOut.Append("</td></tr></table></td></tr>")

                Else
                    htmlOut.Append("<tr><td valign='top' align='left' class='seperator' style='padding-left:3px;'><br />No leases could be found that meet your search criteria</td></tr>")
                End If
            Else
                htmlOut.Append("<tr><td valign='top' align='left' class='seperator' style='padding-left:3px;'><br />No leases could be found that meet your search criteria</td></tr>")
            End If

            htmlOut.Append("</table>")

        Catch ex As Exception

            aError = "Error in views_display_leases_due_to_expire(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal bGetNextLease As Boolean, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function get_leased_aircraft(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT DISTINCT ac_ser_no_full, amod_make_name, amod_model_name, ac_id, amod_id, ac_reg_no, comp_name AS lessor, comp_id, comp_country,")
            sQuery.Append(" (SELECT DISTINCT TOP 1 comp_name FROM aircraft_summary b WHERE a.ac_id= b.ac_id AND cref_contact_type='12') AS lessee, cref_contact_type")

            sQuery.Append(" FROM Aircraft_Summary a WITH(NOLOCK) WHERE (ac_lifecycle_stage = 3) AND (ac_lease_flag = 'Y') AND cref_contact_type in ('13', '57')")

            If searchCriteria.ViewCriteriaAmodID > -1 Then
                sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
            End If

            If searchCriteria.ViewCriteriaCompanyID > 0 Then
                sQuery.Append(Constants.cAndClause + "comp_id = " + searchCriteria.ViewCriteriaCompanyID.ToString)
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

            If searchCriteria.ViewCriteriaAFTTStart > 0 Then
                sQuery.Append(Constants.cAndClause + " ((ac_airframe_tot_hrs >= " & searchCriteria.ViewCriteriaAFTTStart & ") or (ac_airframe_tot_hrs IS NULL))")
            End If

            If searchCriteria.ViewCriteriaAFTTEnd > 0 Then
                sQuery.Append(Constants.cAndClause + " ((ac_airframe_tot_hrs <= " & searchCriteria.ViewCriteriaAFTTEnd & ") or (ac_airframe_tot_hrs IS NULL))")
            End If


            If searchCriteria.ViewCriteriaYearStart > 0 Then
                sQuery.Append(Constants.cAndClause + " ac_mfr_year >= " & searchCriteria.ViewCriteriaYearStart)
            End If


            If searchCriteria.ViewCriteriaYearEnd > 0 Then
                sQuery.Append(Constants.cAndClause + " ac_mfr_year <=  " & searchCriteria.ViewCriteriaYearEnd)
            End If



            If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
                sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
            Else
                sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, False))
            End If

            sQuery.Append(" ORDER BY ac_ser_no_full")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_leased_aircraft(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
                aError = "Error in get_leased_aircraft load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_leased_aircraft(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Sub views_display_leased_aircraft(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)
        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False
        Dim bFirstTime As Boolean = True

        Try

            results_table = get_leased_aircraft(searchCriteria)

            htmlOut.Append("<table id='leasedAircraftOuterTable' width='100%' cellspacing='0' cellpadding='0' class='module'>")

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    htmlOut.Append("<tr><td align='center' valign='middle' class='header'>ACTIVE LEASES&nbsp;<em>(" + results_table.Rows.Count.ToString + ")</em>")
                    htmlOut.Append("<br>(Single Aircraft May Have Multiple Active Leases)</td></tr>")

                    htmlOut.Append("<tr><td valign='top' align='left' colspan='2'>")
                    htmlOut.Append("<div valign=""top"" style='height:370px; overflow: auto;'><p>")
                    htmlOut.Append("<table id='leasedAircraftInnerTable' width='100%' cellpadding='4' cellspacing='0' border='0'>")

                    For Each r As DataRow In results_table.Rows

                        If bFirstTime Then

                            If searchCriteria.ViewCriteriaCompanyID > 0 Then
                                htmlOut.Append("<tr><td valign='top' colspan='2' align='left'><strong>")
                                htmlOut.Append("<a class='underline' target='_blank' href='DisplayCompanyDetail.aspx?compid=" + r.Item("comp_id").ToString + "' title='Display Company Details'>" + r.Item("lessor").ToString.Trim + "</a>")
                                htmlOut.Append("</td></tr>")
                            End If

                            bFirstTime = False
                        End If

                        If Not toggleRowColor Then
                            htmlOut.Append("<tr class='alt_row'>")
                            toggleRowColor = True
                        Else
                            htmlOut.Append("<tr bgcolor='white'>")
                            toggleRowColor = False
                        End If

                        htmlOut.Append("<td valign='top' align='left' width='5%' class='seperator'><img src='images/ch_red.jpg' class='bullet' alt='acid : " + r.Item("ac_id").ToString + "' /></td>")
                        htmlOut.Append("<td valign='top' align='left' class='seperator'>")

                        If searchCriteria.ViewCriteriaAmodID = -1 Then
                            htmlOut.Append("<em>" + r.Item("amod_make_name").ToString + " " + r.Item("amod_model_name").ToString + "</em>, ")
                        End If

                        If Not IsDBNull(r.Item("ac_ser_no_full")) And Not String.IsNullOrEmpty(r.Item("ac_ser_no_full").ToString) Then
                            htmlOut.Append("Serial# <a class='underline' onclick=""JavaScript:load('DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "&jid=0','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title='Display Aircraft Details'>")
                            htmlOut.Append(r.Item("ac_ser_no_full").ToString + "</a>, ")
                        End If

                        If Not IsDBNull(r.Item("ac_reg_no")) And Not String.IsNullOrEmpty(r.Item("ac_reg_no").ToString) Then
                            htmlOut.Append("Reg# " + r.Item("ac_reg_no").ToString)
                        End If

                        htmlOut.Append("<br />")

                        If r.Item("cref_contact_type").ToString = "57" Then
                            htmlOut.Append(" Sub")
                        End If

                        htmlOut.Append(" Leased From: ")
                        htmlOut.Append("<a class='underline' onclick=""JavaScript:load('DisplayCompanyDetail.aspx?compid=" + r.Item("comp_id").ToString + "','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title='Display Company Details'>")
                        htmlOut.Append(r.Item("lessor").ToString.Trim + "</a> (" + r.Item("comp_country").ToString.Trim + ")")
                        htmlOut.Append(" To: " + r.Item("lessee").ToString.Trim + "</td></tr>")

                    Next

                    htmlOut.Append("</table></p></div></td></tr>")

                Else
                    htmlOut.Append("<tr><td valign='top' align='left' class='seperator' style='padding-left:3px;'><br />No leases could be found that meet your search criteria</td></tr>")
                End If
            Else
                htmlOut.Append("<tr><td valign='top' align='left' class='seperator' style='padding-left:3px;'><br />No leases could be found that meet your search criteria</td></tr>")
            End If

            htmlOut.Append("</table>")

        Catch ex As Exception

            aError = "Error in views_display_leased_aircraft(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function get_lease_market_status_total(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT count(distinct ac_id) AS totac")
            sQuery.Append(" FROM Aircraft_summary WITH(NOLOCK)")
            sQuery.Append(" WHERE (ac_lifecycle_stage = 3)")

            If searchCriteria.ViewCriteriaAmodID > -1 Then
                sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
            End If

            If searchCriteria.ViewCriteriaCompanyID > 0 Then
                sQuery.Append(Constants.cAndClause + "comp_id = " + searchCriteria.ViewCriteriaCompanyID.ToString)
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

            If searchCriteria.ViewCriteriaAFTTStart > 0 Then
                sQuery.Append(Constants.cAndClause + " ((ac_airframe_tot_hrs >= " & searchCriteria.ViewCriteriaAFTTStart & ") or (ac_airframe_tot_hrs IS NULL))")
            End If

            If searchCriteria.ViewCriteriaAFTTEnd > 0 Then
                sQuery.Append(Constants.cAndClause + " ((ac_airframe_tot_hrs <= " & searchCriteria.ViewCriteriaAFTTEnd & ") or (ac_airframe_tot_hrs IS NULL))")
            End If


            If searchCriteria.ViewCriteriaYearStart > 0 Then
                sQuery.Append(Constants.cAndClause + " ac_mfr_year >= " & searchCriteria.ViewCriteriaYearStart)
            End If


            If searchCriteria.ViewCriteriaYearEnd > 0 Then
                sQuery.Append(Constants.cAndClause + " ac_mfr_year <=  " & searchCriteria.ViewCriteriaYearEnd)
            End If




            If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
                sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
            Else
                sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
            End If

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_lease_market_status_total(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
                aError = "Error in get_lease_market_status_total load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_lease_market_status_total(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Function get_lease_market_status_leased(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT count(distinct ac_id) as totac")
            sQuery.Append(" FROM Aircraft_Summary WITH(NOLOCK)")
            sQuery.Append(" WHERE (ac_lifecycle_stage = 3) AND (ac_lease_flag = 'Y')")

            If searchCriteria.ViewCriteriaAmodID > -1 Then
                sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
            End If

            If searchCriteria.ViewCriteriaCompanyID > 0 Then
                sQuery.Append(Constants.cAndClause + "comp_id = " + searchCriteria.ViewCriteriaCompanyID.ToString)
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
                sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, False))
            End If

            If searchCriteria.ViewCriteriaAFTTStart > 0 Then
                sQuery.Append(Constants.cAndClause + " ((ac_airframe_tot_hrs >= " & searchCriteria.ViewCriteriaAFTTStart & ") or (ac_airframe_tot_hrs IS NULL))")
            End If

            If searchCriteria.ViewCriteriaAFTTEnd > 0 Then
                sQuery.Append(Constants.cAndClause + " ((ac_airframe_tot_hrs <= " & searchCriteria.ViewCriteriaAFTTEnd & ") or (ac_airframe_tot_hrs IS NULL))")
            End If


            If searchCriteria.ViewCriteriaYearStart > 0 Then
                sQuery.Append(Constants.cAndClause + " ac_mfr_year >= " & searchCriteria.ViewCriteriaYearStart)
            End If


            If searchCriteria.ViewCriteriaYearEnd > 0 Then
                sQuery.Append(Constants.cAndClause + " ac_mfr_year <=  " & searchCriteria.ViewCriteriaYearEnd)
            End If



            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_lease_market_status_leased(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
                aError = "Error in get_lease_market_status_leased load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_lease_market_status_leased(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Sub views_display_lease_market_status_block(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

        Dim percent_of_total As Double = 0.0
        Dim lease_total As Integer = 0
        Dim overall_total As Integer = 0

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder

        results_table = get_lease_market_status_total(searchCriteria)

        If Not IsNothing(results_table) Then
            If results_table.Rows.Count > 0 Then
                For Each r As DataRow In results_table.Rows
                    overall_total = CInt(r.Item("totac").ToString)
                Next
            End If
        End If

        results_table = Nothing
        results_table = New DataTable

        results_table = get_lease_market_status_leased(searchCriteria)

        If Not IsNothing(results_table) Then
            If results_table.Rows.Count > 0 Then
                For Each r As DataRow In results_table.Rows
                    lease_total = CInt(r.Item("totac").ToString)
                Next
            End If
        End If

        If lease_total > 0 And overall_total > 0 Then
            percent_of_total = CDbl((lease_total / overall_total) * 100)
        End If

        htmlOut.Append("<table id='leaseMarketStatusOuterTable' width='100%' cellspacing='0' cellpadding='1' class='module'>")
        htmlOut.Append("<tr><td align='left' valign='middle' class='header' style='padding-left:3px;'>LEASE MARKET STATUS</td></tr>")
        htmlOut.Append("<tr><td colspan='4' class='border_bottom_right'>")

        htmlOut.Append("<table id='leaseMarketStatusInnerTable' width='100%' cellpadding='2' cellspacing='0' border='0'><tr>")
        htmlOut.Append("<td valign='top'  bgcolor='#EEEEEE' align='left' width='35%'><strong>In Operation</strong></td>")
        htmlOut.Append("<td valign='top' align='right'><strong>&nbsp;&nbsp;" + FormatNumber(overall_total, 0).ToString + "</strong></td><td>&nbsp;")
        htmlOut.Append("</tr><tr>")
        htmlOut.Append("<td valign='top' bgcolor='#EEEEEE' align='left' width='35%'><strong>Leased</strong></td>")
        htmlOut.Append("<td valign='top' align='right'><strong>&nbsp;&nbsp;" + FormatNumber(lease_total, 0).ToString + " </td><td align='left'><strong>( " + FormatNumber(percent_of_total, 1).ToString + "% of In Operation ) </strong></td>")

        htmlOut.Append("</tr></table></td></tr></table>")

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function get_most_recent_lease_trans(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT top 100 journ_date, journ_id, journ_comp_id, journ_subject, ac_ser_no_full, amod_make_name, amod_model_name,")
            sQuery.Append(" ac_id, comp_id, amod_id, ac_reg_no, ac_amod_id, journ_subcategory_code, comp_city, comp_state, comp_name")
            sQuery.Append(" FROM Aircraft WITH(NOLOCK)")
            sQuery.Append(" INNER JOIN Aircraft_Model WITH(NOLOCK) ON ac_amod_id = amod_id")
            sQuery.Append(" INNER JOIN Aircraft_Reference WITH(NOLOCK) on cref_ac_id = ac_id and cref_journ_id = ac_journ_id AND cref_contact_type in ('13','57')")
            sQuery.Append(" INNER JOIN Journal WITH(NOLOCK) on ac_journ_id = journ_id")
            sQuery.Append(" INNER JOIN Company WITH(NOLOCK) on comp_id = cref_comp_id and comp_journ_id = cref_journ_id")
            sQuery.Append(" WHERE")

            sQuery.Append(" (journ_subcat_code_part1 LIKE 'L%' AND journ_subcat_code_part2 NOT IN ('CO') AND journ_subcat_code_part3 NOT IN ('IT', 'RR'))")

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

            If searchCriteria.ViewCriteriaAmodID > -1 Then
                sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
            End If

            If searchCriteria.ViewCriteriaCompanyID > 0 Then
                sQuery.Append(Constants.cAndClause + "comp_id = " + searchCriteria.ViewCriteriaCompanyID.ToString)
            End If

            If searchCriteria.ViewCriteriaCompanyID = 0 And searchCriteria.ViewCriteriaAmodID = -1 Then
                sQuery.Append(Constants.cAndClause + "(journ_date > '" + Month(DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now())).ToString + "/01/" + Year(DateAdd("m", (-1) * searchCriteria.ViewCriteriaTimeSpan, Now())).ToString + "')")
            End If

            If searchCriteria.ViewCriteriaAFTTStart > 0 Then
                sQuery.Append(Constants.cAndClause + " ((ac_airframe_tot_hrs >= " & searchCriteria.ViewCriteriaAFTTStart & ") or (ac_airframe_tot_hrs IS NULL))")
            End If

            If searchCriteria.ViewCriteriaAFTTEnd > 0 Then
                sQuery.Append(Constants.cAndClause + " ((ac_airframe_tot_hrs <= " & searchCriteria.ViewCriteriaAFTTEnd & ") or (ac_airframe_tot_hrs IS NULL))")
            End If


            If searchCriteria.ViewCriteriaYearStart > 0 Then
                sQuery.Append(Constants.cAndClause + " ac_mfr_year >= " & searchCriteria.ViewCriteriaYearStart)
            End If


            If searchCriteria.ViewCriteriaYearEnd > 0 Then
                sQuery.Append(Constants.cAndClause + " ac_mfr_year <=  " & searchCriteria.ViewCriteriaYearEnd)
            End If



            If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
                sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
            Else
                sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, False))
            End If

            sQuery.Append(" ORDER BY journ_date desc")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_most_recent_lease_trans(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
                aError = "Error in get_most_recent_lease_trans load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_most_recent_lease_trans(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Sub views_display_most_recent_lease_trans(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False

        Try

            results_table = get_most_recent_lease_trans(searchCriteria)

            htmlOut.Append("<table id='mostRecentLeaseOuterTable' width='100%' cellspacing='0' cellpadding='2' class='module'>")

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then
                    htmlOut.Append("<tr><td valign='top' align='center' class='header'>MOST RECENT LEASE TRANSACTIONS<br />(<em>last " + searchCriteria.ViewCriteriaTimeSpan.ToString + " months (" + results_table.Rows.Count.ToString + ")</em>)</td></tr>")
                Else
                    htmlOut.Append("<tr><td align='center' valign='middle' class='header'>MOST RECENT LEASE TRANSACTIONS<br />(<em>Last " + searchCriteria.ViewCriteriaTimeSpan.ToString + " months</em>)</td></tr>")
                End If

                If results_table.Rows.Count > 0 Then

                    If searchCriteria.ViewCriteriaAmodID > -1 And searchCriteria.ViewCriteriaCompanyID > 0 Then
                        htmlOut.Append("<tr><td class='border_bottom_right'><div style='height:250px; width:100%; overflow: auto;'><p>")
                    ElseIf searchCriteria.ViewCriteriaCompanyID > 0 Then
                        htmlOut.Append("<tr><td class='border_bottom_right'><div style='height:275px; width:100%; overflow: auto;'><p>")
                    ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then
                        htmlOut.Append("<tr><td class='border_bottom_right'><div style='height:375px; width:100%; overflow: auto;'><p>")
                    Else
                        htmlOut.Append("<tr><td class='border_bottom_right'><div style='height:850px; width:100%; overflow: auto;'><p>")
                    End If

                    htmlOut.Append("<table id='mostRecentLeaseInnerTable' width='100%' cellpadding='4' cellspacing='0' border='0'>")
                    htmlOut.Append("<tr><td valign='top' colspan='2' align='center' class='seperator'><strong>")

                    If searchCriteria.ViewCriteriaAmodID > -1 And searchCriteria.ViewCriteriaCompanyID > 0 Then
                        htmlOut.Append(commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, "") + " : " + commonEvo.get_company_name_fromID(searchCriteria.ViewCriteriaCompanyID, 0, False, True, ""))
                    ElseIf searchCriteria.ViewCriteriaCompanyID > 0 Then
                        htmlOut.Append(commonEvo.get_company_name_fromID(searchCriteria.ViewCriteriaCompanyID, 0, False, True, ""))
                    ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then
                        htmlOut.Append(commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, ""))
                    Else
                        htmlOut.Append("Lease Information")
                    End If

                    htmlOut.Append("</strong></td></tr>")

                    For Each r As DataRow In results_table.Rows

                        If Not toggleRowColor Then
                            htmlOut.Append("<tr class='alt_row'>")
                            toggleRowColor = True
                        Else
                            htmlOut.Append("<tr bgcolor='white'>")
                            toggleRowColor = False
                        End If

                        htmlOut.Append("<td valign='top' align='left' width='5%' class='seperator'><img src='images/ch_red.jpg' class='bullet' alt='" + FormatDateTime(r.Item("journ_date").ToString, vbShortDate).ToString + "' /></td>")
                        htmlOut.Append("<td valign='top' align='left' width='95%' class='seperator'>" + FormatDateTime(r.Item("journ_date").ToString, vbShortDate).ToString)

                        If searchCriteria.ViewCriteriaAmodID = -1 Then
                            If searchCriteria.ViewCriteriaCompanyID > 0 Then
                                htmlOut.Append(" - <a class='underline' href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&viewCompany=" + searchCriteria.ViewCriteriaCompanyID.ToString + "&amod_id=" + r.Item("ac_amod_id").ToString + "' title='Show lease details for this make/model'>")
                            Else
                                htmlOut.Append(" - <a class='underline' href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&amod_id=" + r.Item("ac_amod_id").ToString + "' title='Show lease details for this make/model'>")
                            End If
                            htmlOut.Append(r.Item("amod_make_name").ToString + " " + r.Item("amod_model_name").ToString + "</a>, ")
                        Else
                            htmlOut.Append(" - ")
                        End If

                        If Not IsDBNull(r.Item("ac_ser_no_full")) And Not String.IsNullOrEmpty(r.Item("ac_ser_no_full").ToString) Then
                            htmlOut.Append("Serial# <a class='underline' onclick=""JavaScript:load('DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "&jid=0','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title='Display Aircraft Details'>")
                            htmlOut.Append(r.Item("ac_ser_no_full").ToString + "</a>, ")
                        End If

                        If Not IsDBNull(r.Item("ac_reg_no")) And Not String.IsNullOrEmpty(r.Item("ac_reg_no").ToString) Then
                            htmlOut.Append("Reg# " + r.Item("ac_reg_no").ToString + ", ")
                        End If

                        htmlOut.Append("<a class='underline' onclick=""javascript:load('DisplayAircraftDetail.aspx?acid=" + r.Item("ac_id").ToString + "&jid=" + r.Item("journ_id").ToString + "','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');"" title='Display Transaction Details'>")
                        htmlOut.Append(r.Item("journ_subject").ToString + "</a>")
                        htmlOut.Append("</td></tr>")

                    Next

                    htmlOut.Append("</table></p></div></td></tr>")

                Else
                    htmlOut.Append("<tr><td valign='top' align='left' class='seperator' style='padding-left:3px;'><br />No aircraft could be found that meet your search criteria</td></tr>")
                End If
            Else
                htmlOut.Append("<tr><td valign='top' align='left' class='seperator' style='padding-left:3px;'><br />No aircraft could be found that meet your search criteria</td></tr>")
            End If

            htmlOut.Append("</table>")

        Catch ex As Exception

            aError = "Error in views_most_recent_lease_trans(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub
    Public Function get_models_leased_info_new_spec(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal roll_up As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery As String = ""

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery &= " select comp_name, comp_city,comp_state, comp_country, comp_id, "
            sQuery &= " sum(case when ac_lease_flag='Y' "
            sQuery &= " " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False)
            sQuery &= "   then 1 else 0 end) as account,"
            sQuery &= " sum(case when cref_operator_flag='Y' "
            sQuery &= " " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False)
            sQuery &= "   then 1 else 0 end) as Operator "
            sQuery &= " from Company with (NOLOCK) "
            sQuery &= " inner join Aircraft_Reference with (NOLOCK) on cref_comp_id = comp_id and cref_journ_id = comp_journ_id"
            sQuery &= " inner join Aircraft with (NOLOCK) on cref_ac_id = ac_id and cref_journ_id = ac_journ_id"
            sQuery &= " inner join Aircraft_Model with (NOLOCK) on ac_amod_id = amod_id"
            sQuery &= " where comp_journ_id = 0 "

            If Trim(roll_up) = "Y" Then
                sQuery &= " and comp_id in (select distinct RelCompID from ReturnAllCompanyLocationsByCompId( " & searchCriteria.ViewCriteriaCompanyID & "))"
            Else
                sQuery &= " AND comp_id = " & searchCriteria.ViewCriteriaCompanyID & " "
            End If

            If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
                sQuery &= "  " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False)
            Else
                sQuery &= "  " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True)
            End If




            sQuery &= " AND cref_contact_type IN ('13','57') "
            sQuery &= " group by comp_name, comp_city,comp_state, comp_country, comp_id"
            sQuery &= " order by comp_name, comp_city,comp_state, comp_country, comp_id"


            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_models_leased_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
                aError = "Error in get_models_leased_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_models_leased_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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
    Public Function get_models_leased_info_company(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal use_roll As String) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT DISTINCT amod_make_name, amod_model_name,amod_id, count(distinct ac_id) AS account ")
            sQuery.Append(" , sum(case when cref_operator_flag='Y' then 1 else 0 end) as Operator ")
            sQuery.Append(" FROM View_Aircraft_Company_Flat WITH(NOLOCK) WHERE (ac_lifecycle_stage = 3) AND (ac_lease_flag = 'Y')  ")
            sQuery.Append(" AND cref_contact_type IN ('13', '57')  ")
            If Trim(use_roll) = "Y" Then
                sQuery.Append(" and comp_id in (select distinct RelCompID from ReturnAllCompanyLocationsByCompId(" & searchCriteria.ViewCriteriaCompanyID & ")) ")
            Else
                sQuery.Append(" AND comp_id = " & searchCriteria.ViewCriteriaCompanyID & " ")
            End If


            If searchCriteria.ViewCriteriaProductType = Constants.PRODUCT_CODE_ALL Then
                sQuery.Append(" " + commonEvo.GenerateProductCodeSelectionQuery(HttpContext.Current.Session.Item("localPreferences"), False, False))
            Else
                sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, True))
            End If

            sQuery.Append(" GROUP BY amod_make_name, amod_model_name, amod_id ORDER BY count(distinct ac_id) desc ")




            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_models_leased_info_company(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
                aError = "Error in get_models_leased_info_company load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_models_leased_info_company(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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
    Public Function get_models_leased_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            If searchCriteria.ViewCriteriaAmodID > -1 Or searchCriteria.ViewCriteriaCompanyID > 0 Then
                sQuery.Append("SELECT DISTINCT amod_make_name, amod_model_name,amod_id, count(distinct ac_id) AS account")
            Else
                sQuery.Append("SELECT DISTINCT top 100 amod_make_name, amod_model_name, amod_id, count(distinct ac_id) AS account")
            End If

            sQuery.Append(" FROM Aircraft_summary WITH(NOLOCK) WHERE (ac_lifecycle_stage = 3) AND (ac_lease_flag = 'Y') AND cref_contact_type IN ('13', '57')")

            If searchCriteria.ViewCriteriaAmodID > -1 Then
                sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
            End If

            If searchCriteria.ViewCriteriaCompanyID > 0 Then
                sQuery.Append(Constants.cAndClause + "comp_id = " + searchCriteria.ViewCriteriaCompanyID.ToString)
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
                sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, False))
            End If

            sQuery.Append(" GROUP BY amod_make_name, amod_model_name, amod_id")
            sQuery.Append(" ORDER BY count(distinct ac_id) desc")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_models_leased_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
                aError = "Error in get_models_leased_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_models_leased_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Sub views_display_models_leased_list(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, Optional ByVal from_spot As String = "", Optional ByVal use_roll As String = "N")

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False
        Dim TotalLease As Long = 0
        Dim TotalOperated As Long = 0
        Dim bgcolor As String = ""
        Dim font_text_title As String = ""
        Dim font_text_start As String = ""
        Dim font_text_end As String = ""
        Dim temp_dir As String = "right"

        Try

            If Trim(from_spot) = "pdf" Then
                font_text_start = "<font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>"
                font_text_title = "<font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT_TITLE") & "'>"
                font_text_end = "</font>"
                temp_dir = "left"
            Else
                font_text_start = ""
                font_text_title = ""
                font_text_end = ""
            End If


            If Trim(from_spot) = "company" Or Trim(from_spot) = "pdf" Then
                results_table = get_models_leased_info_company(searchCriteria, use_roll)
            Else
                results_table = get_models_leased_info(searchCriteria)
            End If


            If Trim(from_spot) = "pdf" Then
                htmlOut.Append("<tr class='" & HttpContext.Current.Session.Item("ROW_CLASS_BOTTOM") & "'><td colspan='12'><font class='" & HttpContext.Current.Session.Item("FONT_CLASS_HEADER") & "'>Models Leased</font></td></tr>")
            ElseIf Trim(from_spot) = "company" Then
                htmlOut.Append("<div class=""Box""><div class=""subHeader"">MODELS LEASED</div><table id='displayModelsLeasedOuterTable' width='100%' cellspacing='0' cellpadding='3' class='formatTable blue'>")
            Else
                htmlOut.Append("<table id='displayModelsLeasedOuterTable' width='100%' cellspacing='0' cellpadding='0' class='module'>")
            End If







            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then


                    If Trim(from_spot) = "company" Or Trim(from_spot) = "pdf" Then
                        htmlOut.Append("<tr class='header_row'>")
                        htmlOut.Append("<td valign='top' align='left' width='60%' class='seperator'><strong>" & font_text_title & "Model&nbsp;Name" & font_text_end & "</strong></td>")
                        htmlOut.Append("<td valign='top' align='right' width='20%' class='seperator'><strong>" & font_text_title & "#AC Leased" & font_text_end & "</strong></td>")
                        htmlOut.Append("<td valign='top' align='right' width='20%' class='seperator'><strong>" & font_text_title & "#AC Operated" & font_text_end & "</strong></td></tr>")
                    Else
                        If searchCriteria.ViewCriteriaAmodID > -1 And searchCriteria.ViewCriteriaCompanyID > 0 Then
                            htmlOut.Append("<tr><td align='center' valign='middle' class='header'>" + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, "") + " LEASED: " + commonEvo.get_company_name_fromID(searchCriteria.ViewCriteriaCompanyID, 0, False, True, "") + "</td></tr>")
                            htmlOut.Append("<tr><td class='border_bottom_right' colspan=""2""><div style='height:375px; width:100%; overflow: auto;'><p>")
                        ElseIf searchCriteria.ViewCriteriaCompanyID > 0 Then
                            htmlOut.Append("<tr><td align='center' valign='middle' class='header'>MODELS LEASED: " + commonEvo.get_company_name_fromID(searchCriteria.ViewCriteriaCompanyID, 0, False, True, "") + "</td></tr>")
                            htmlOut.Append("<tr><td class='border_bottom_right' colspan=""2""><div style='height:505px; width:100%; overflow: auto;'><p>")
                        ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then
                            htmlOut.Append("<tr><td align='center' valign='middle' class='header'>" + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, "") + " LEASED</td></tr>")
                            htmlOut.Append("<tr><td class='border_bottom_right' colspan=""2""><div style='height:400px; width:100%; overflow: auto;'><p>")
                        Else
                            htmlOut.Append("<tr><td align='center' valign='middle' class='header'>TOP 100 MODELS LEASED</td></tr>")
                            htmlOut.Append("<tr><td class='border_bottom_right' colspan=""2""><div style='height:550px; width:100%; overflow: auto;'><p>")
                        End If

                        htmlOut.Append("<table id='displayModelsLeasedInnerTable' cellspacing='0' cellpadding='2' border='0' width='100%'>")
                        htmlOut.Append("<td valign='top' align='left' width='80%' class='seperator'><strong>Model&nbsp;Name</strong></td>")
                        htmlOut.Append("<td valign='top' align='right' width='20%' class='seperator'><strong># AC Leased</strong></td></tr>")
                    End If




                    For Each r As DataRow In results_table.Rows


                        If Trim(from_spot) = "pdf" Then
                            If Not toggleRowColor Then
                                toggleRowColor = True
                                bgcolor = ""
                            Else
                                toggleRowColor = False
                                bgcolor = "#f0f0f0"
                            End If
                            htmlOut.Append("<tr  bgcolor='" & bgcolor & "'>")
                        Else
                            If Not toggleRowColor Then
                                htmlOut.Append("<tr class='alt_row'>")
                                toggleRowColor = True
                            Else
                                htmlOut.Append("<tr bgcolor='white'>")
                                toggleRowColor = False
                            End If
                        End If



                        If Trim(from_spot) = "pdf" Then
                            htmlOut.Append("<td valign='top' align='left' class='seperator'>" & font_text_start & "")
                        ElseIf Trim(from_spot) = "company" Then
                            htmlOut.Append("<td valign='top' align='left' class='seperator'><a class='underline' href='DisplayCompanyDetail.aspx?compid=" & searchCriteria.ViewCriteriaCompanyID.ToString & "&amod_id=" & r.Item("amod_id") & "&use_insight_lease=Y&use_insight_roll=" & use_roll & "&viewCompany=" + searchCriteria.ViewCriteriaCompanyID.ToString + "'' title='Show lease details for this make/model'>")
                        Else
                            If searchCriteria.ViewCriteriaCompanyID > 0 Then
                                htmlOut.Append("<td valign='top' align='left' class='seperator'><a class='underline' href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&viewCompany=" + searchCriteria.ViewCriteriaCompanyID.ToString + "&amod_id=" + r.Item("amod_id").ToString + "' title='Show lease details for this make/model'>")
                            Else
                                htmlOut.Append("<td valign='top' align='left' class='seperator'><a class='underline' href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&amod_id=" + r.Item("amod_id").ToString + "' title='Show lease details for this make/model'>")
                            End If
                        End If

                        htmlOut.Append(r.Item("amod_make_name").ToString + " " + r.Item("amod_model_name").ToString + font_text_end & "</a></td>")


                        htmlOut.Append("<td valign='top' align='right' class='seperator' style='padding-right:5px;'>" & font_text_start & "" + r.Item("account").ToString + "" & font_text_end & "</td>")


                        If Trim(from_spot) = "company" Or Trim(from_spot) = "pdf" Then
                            htmlOut.Append("<td valign='top' align='right' class='seperator' style='padding-right:5px;'>" & font_text_start & "" + r.Item("operator").ToString + "" & font_text_end & "</td></tr>")
                            'Totalling up operator/lease but only on company side.
                            If IsNumeric(r.Item("operator")) Then
                                TotalOperated += r.Item("operator")
                            End If
                            If IsNumeric(r.Item("account")) Then
                                TotalLease += r.Item("account")
                            End If
                        Else
                            htmlOut.Append("</tr>")
                        End If




                    Next

                    If Trim(from_spot) = "company" Or Trim(from_spot) = "pdf" Then
                        'Totals go here:
                        htmlOut.Append("<tr>")
                        htmlOut.Append("<td valign='top' align='left' class='seperator'><strong>" & font_text_title & "Totals:" & font_text_end & "</strong></td>")
                        htmlOut.Append("<td valign='top' align='right' class='seperator'><strong>" & font_text_title & "" & TotalLease.ToString & "" & font_text_end & "</strong></td>")
                        htmlOut.Append("<td valign='top' align='right' class='seperator'><strong>" & font_text_title & "" & TotalOperated.ToString & "" & font_text_end & "</strong></td>")
                    Else
                        htmlOut.Append("</table></p></div></td></tr>")
                    End If


                Else
                    htmlOut.Append("<tr><td valign='top' align='left' class='seperator' style='padding-left:3px;'><br/>No models could be found that meet your search criteria</td></tr>")
                End If
            Else
                htmlOut.Append("<tr><td valign='top' align='left' class='seperator' style='padding-left:3px;'><br/>No models could be found that meet your search criteria</td></tr>")
            End If

            htmlOut.Append("</table>")
            If Trim(from_spot) = "company" Then
                htmlOut.Append("</div>")
            End If

        Catch ex As Exception

            aError = "Error in views_display_models_leased_list(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function get_top_lessors_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            If searchCriteria.ViewCriteriaAmodID > -1 Then
                sQuery.Append("SELECT DISTINCT comp_name, comp_id, comp_address1, comp_address2, comp_city, comp_state, comp_zip_code, comp_country, count(distinct ac_id) AS account")
            Else
                sQuery.Append("SELECT DISTINCT top 100 comp_name, comp_address1, comp_address2, comp_city, comp_state, comp_zip_code, comp_country, comp_id, count(distinct ac_id) AS account")
            End If

            sQuery.Append(" FROM Aircraft_summary WITH(NOLOCK) WHERE (ac_lifecycle_stage = 3) AND (ac_lease_flag = 'Y') AND cref_contact_type IN ('13', '57')")

            If searchCriteria.ViewCriteriaAmodID > -1 Then
                sQuery.Append(Constants.cAndClause + "amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString)
            ElseIf Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                sQuery.Append(Constants.cAndClause + "amod_make_name = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "'")
            End If

            If searchCriteria.ViewCriteriaCompanyID > 0 Then
                sQuery.Append(Constants.cAndClause + "comp_id = " + searchCriteria.ViewCriteriaCompanyID.ToString)
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
                sQuery.Append(" " + commonEvo.BuildProductCodeCheckWhereClause(searchCriteria.ViewCriteriaHasHelicopterFlag, searchCriteria.ViewCriteriaHasBusinessFlag, searchCriteria.ViewCriteriaHasCommercialFlag, False, False, False, False))
            End If



            If searchCriteria.ViewCriteriaAFTTStart > 0 Then
                sQuery.Append(Constants.cAndClause + " ((ac_airframe_tot_hrs >= " & searchCriteria.ViewCriteriaAFTTStart & ") or (ac_airframe_tot_hrs IS NULL))")
            End If

            If searchCriteria.ViewCriteriaAFTTEnd > 0 Then
                sQuery.Append(Constants.cAndClause + " ((ac_airframe_tot_hrs <= " & searchCriteria.ViewCriteriaAFTTEnd & ") or (ac_airframe_tot_hrs IS NULL))")
            End If


            If searchCriteria.ViewCriteriaYearStart > 0 Then
                sQuery.Append(Constants.cAndClause + " ac_mfr_year >= " & searchCriteria.ViewCriteriaYearStart)
            End If


            If searchCriteria.ViewCriteriaYearEnd > 0 Then
                sQuery.Append(Constants.cAndClause + " ac_mfr_year <=  " & searchCriteria.ViewCriteriaYearEnd)
            End If



            sQuery.Append(" GROUP BY comp_name, comp_id, comp_address1, comp_address2, comp_city, comp_state, comp_zip_code, comp_country")
            sQuery.Append(" ORDER BY count(distinct ac_id) desc")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_top_lessors_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
                aError = "Error in get_top_lessors_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_top_lessors_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Function LeaseSummary(ByRef searchCriteria As viewSelectionCriteriaClass)
        Dim sql As String = ""
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim atemptable As New DataTable
        Try
            If Not String.IsNullOrEmpty(HttpContext.Current.Session.Item("jetnetClientDatabase")) Then
                sql = "SELECT DISTINCT comp_name, comp_id, comp_city, comp_state, comp_country, COUNT(*) as tcount "
                '-- GET LEASED TO
                sql += " from Aircraft with (NOLOCK)"
                sql += " inner join Aircraft_Reference with (NOLOCK) on ac_id = cref_ac_id and ac_journ_id = cref_journ_id "
                sql += " inner join Company with (NOLOCK) on cref_comp_id = comp_id and cref_journ_id = comp_journ_id"
                sql += " where cref_contact_type in ('12','39') and ac_journ_id = 0 "
                sql += " and cref_ac_id in (select distinct cref_ac_id from Aircraft_Reference with (NOLOCK) where cref_comp_id =" & searchCriteria.ViewCriteriaCompanyID & " and cref_journ_id = 0 and cref_contact_type in ('13','57') )"
                sql += " group by comp_name, comp_id, comp_city, comp_state, comp_country"
                sql += " order by COUNT(*) desc"


                HttpContext.Current.Session.Item("localUser").crmUser_DebugText = HttpContext.Current.Session.Item("localUser").crmUser_DebugText & "<br /><br /><b>LeaseSummary(ByVal compIDList As String)</b><br />" & sql


                SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetClientDatabase")
                SqlConn.Open()
                SqlCommand.Connection = SqlConn

                SqlCommand.CommandText = sql
                SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
                SqlCommand.CommandType = CommandType.Text
                SqlCommand.CommandTimeout = 60

                Try
                    atemptable.Load(SqlReader)
                Catch constrExc As System.Data.ConstraintException
                    Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                End Try
            End If

            Return atemptable
        Catch ex As Exception
            LeaseSummary = Nothing
        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing
        End Try

    End Function
    Public Sub views_display_top_lessors(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, Optional ByVal from_spot As String = "", Optional ByVal use_roll As String = "N")

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim toggleRowColor As Boolean = False
        Dim bgcolor As String = ""
        Dim font_text_title As String = ""
        Dim font_text_start As String = ""
        Dim font_text_end As String = ""
        Dim temp_dir As String = "right"

        Try


            If Trim(from_spot) = "pdf" Then
                font_text_start = "<font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>"
                font_text_title = "<font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT_TITLE") & "'>"
                font_text_end = "</font>"
                temp_dir = "left"
            Else
                font_text_start = ""
                font_text_title = ""
                font_text_end = ""
            End If


            If from_spot = "company" Or Trim(from_spot) = "pdf" Then
                results_table = get_models_leased_info_new_spec(searchCriteria, use_roll)
            Else
                results_table = get_top_lessors_info(searchCriteria)
            End If

            If Trim(from_spot) = "pdf" Then
                htmlOut.Append("<tr class='" & HttpContext.Current.Session.Item("ROW_CLASS_BOTTOM") & "'><td colspan='12'><font class='" & HttpContext.Current.Session.Item("FONT_CLASS_HEADER") & "'>LESSOR SUMMARY</font></td></tr>")
            ElseIf Trim(from_spot) = "company" Then
                htmlOut.Append("<table id='displayTopLessorsOuterTable' width='100%' cellspacing='0' cellpadding='0' class='data_aircraft_grid'>")
            Else
                htmlOut.Append("<table id='displayTopLessorsOuterTable' width='100%' cellspacing='0' cellpadding='0' class='module'>")
            End If



            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    If Trim(from_spot) = "company" Or Trim(from_spot) = "pdf" Then


                        htmlOut.Append("<tr class='header_row'>")
                        htmlOut.Append("<td valign='top' align='left' width='60%' class='seperator'><strong>" & font_text_title & "Lessor&nbsp;Name" & font_text_end & "</strong></td>")
                        htmlOut.Append("<td valign='top' align='right' width='20%' class='seperator'><strong>" & font_text_title & "#AC Leased" & font_text_end & "</strong></td>")
                        htmlOut.Append("<td valign='top' align='right' width='20%' class='seperator'><strong>" & font_text_title & "#AC Operated" & font_text_end & "</strong></td></tr>")
                    Else
                        If searchCriteria.ViewCriteriaAmodID > -1 And searchCriteria.ViewCriteriaCompanyID > 0 Then
                            htmlOut.Append("<tr><td align='center' valign='middle' class='header'>" + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, "") + " - " + commonEvo.get_company_name_fromID(searchCriteria.ViewCriteriaCompanyID, 0, False, True, "") + "</td></tr>")
                            htmlOut.Append("<tr><td class='border_bottom_right' colspan=""2""><div style='height:375px; width:100%; overflow: auto;'><p>")
                        ElseIf searchCriteria.ViewCriteriaCompanyID > 0 Then
                            htmlOut.Append("<tr><td align='center' valign='middle' class='header'>LESSOR: " + commonEvo.get_company_name_fromID(searchCriteria.ViewCriteriaCompanyID, 0, False, True, "") + "</td></tr>")
                            htmlOut.Append("<tr><td class='border_bottom_right' colspan=""2""><div style='height:85; width:100%; overflow: auto;'><p>")
                        ElseIf searchCriteria.ViewCriteriaAmodID > -1 Then
                            htmlOut.Append("<tr><td align='center' valign='middle' class='header'>LESSOR: " + commonEvo.Get_Aircraft_Model_Info(searchCriteria.ViewCriteriaAmodID, False, "") + "</td></tr>")
                            htmlOut.Append("<tr><td class='border_bottom_right' colspan=""2""><div style='height:500px; width:100%; overflow: auto;'><p>")
                        Else
                            htmlOut.Append("<tr><td align='center' valign='middle' class='header'>TOP 100 LESSORS</td></tr>")
                            htmlOut.Append("<tr><td class='border_bottom_right' colspan=""2""><div style='height:350px; width:100%; overflow: auto;'><p>")
                        End If

                        htmlOut.Append("<table id='displayModelsLeasedInnerTable' cellspacing='0' cellpadding='2' border='0' width='100%'>")
                        htmlOut.Append("<td valign='top' align='left' width='80%' class='seperator'><strong>Lessor&nbsp;Name</strong></td>")
                        htmlOut.Append("<td valign='top' align='right' width='20%' class='seperator'><strong># AC Leased</strong></td></tr>")
                    End If



                    For Each r As DataRow In results_table.Rows

                        If Trim(from_spot) = "pdf" Then
                            If Not toggleRowColor Then
                                toggleRowColor = True
                                bgcolor = ""
                            Else
                                toggleRowColor = False
                                bgcolor = "#f0f0f0"
                            End If
                            htmlOut.Append("<tr bgcolor='" & bgcolor & "'>")
                        Else
                            If Not toggleRowColor Then
                                htmlOut.Append("<tr class='alt_row'>")
                                toggleRowColor = True
                            Else
                                htmlOut.Append("<tr bgcolor='white'>")
                                toggleRowColor = False
                            End If
                        End If



                        If Trim(from_spot) = "company" Or Trim(from_spot) = "pdf" Then
                            If searchCriteria.ViewCriteriaAmodID > -1 Then
                                htmlOut.Append("<td valign='top' align='left' class='seperator'>" & font_text_start & "")
                            Else
                                htmlOut.Append("<td valign='top' align='left' class='seperator'>" & font_text_start & "")
                            End If
                            htmlOut.Append(r.Item("comp_name").ToString + " (")
                        Else
                            If searchCriteria.ViewCriteriaAmodID > -1 Then
                                htmlOut.Append("<td valign='top' align='left' class='seperator'><a class='underline' href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&viewCompany=" + r.Item("comp_id").ToString + "&amod_id=" + searchCriteria.ViewCriteriaAmodID.ToString + "' title='Show lease details for this company'>")
                            Else
                                htmlOut.Append("<td valign='top' align='left' class='seperator'><a class='underline' href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&viewCompany=" + r.Item("comp_id").ToString + "' title='Show lease details for this company'>")
                            End If
                            htmlOut.Append(r.Item("comp_name").ToString + "</a><br />")
                        End If


                        If Trim(from_spot) = "company" Or Trim(from_spot) = "pdf" Then
                            If Not IsDBNull(r.Item("comp_city")) And Not String.IsNullOrEmpty(r.Item("comp_city").ToString) Then
                                htmlOut.Append(r.Item("comp_city").ToString + ",")
                            End If

                            If Not IsDBNull(r.Item("comp_state")) And Not String.IsNullOrEmpty(r.Item("comp_state").ToString) Then
                                htmlOut.Append(r.Item("comp_state").ToString + " ")
                            End If

                            If Not IsDBNull(r.Item("comp_country")) And Not String.IsNullOrEmpty(r.Item("comp_country").ToString) Then
                                htmlOut.Append(r.Item("comp_country").ToString)
                            End If

                            htmlOut.Append(")" & font_text_end & "")
                            htmlOut.Append("</td>")

                        Else
                            If Not IsDBNull(r.Item("comp_address1")) And Not String.IsNullOrEmpty(r.Item("comp_address1").ToString) Then
                                htmlOut.Append(r.Item("comp_address1").ToString + "<br />")
                            End If

                            If Not IsDBNull(r.Item("comp_address2")) And Not String.IsNullOrEmpty(r.Item("comp_address2").ToString) Then
                                htmlOut.Append(r.Item("comp_address2").ToString + "<br />")
                            End If

                            If Not IsDBNull(r.Item("comp_city")) And Not String.IsNullOrEmpty(r.Item("comp_city").ToString) Then
                                htmlOut.Append(r.Item("comp_city").ToString + ",")
                            End If

                            If Not IsDBNull(r.Item("comp_state")) And Not String.IsNullOrEmpty(r.Item("comp_state").ToString) Then
                                htmlOut.Append(r.Item("comp_state").ToString + " ")
                            End If

                            If Not IsDBNull(r.Item("comp_zip_code")) And Not String.IsNullOrEmpty(r.Item("comp_zip_code").ToString) Then
                                htmlOut.Append(r.Item("comp_zip_code").ToString + " ")
                            End If

                            If Not IsDBNull(r.Item("comp_country")) And Not String.IsNullOrEmpty(r.Item("comp_country").ToString) Then
                                htmlOut.Append(r.Item("comp_country").ToString)
                            End If
                            htmlOut.Append("</td>")
                        End If

                        htmlOut.Append("<td valign='top' align='right' class='seperator' style='padding-right:5px;'>" & font_text_start & "" + r.Item("account").ToString + "" & font_text_end & "</td>")

                        If Trim(from_spot) = "company" Or Trim(from_spot) = "pdf" Then
                            htmlOut.Append("<td valign='top' align='right' class='seperator' style='padding-right:5px;'>" & font_text_start & "" + r.Item("operator").ToString + "" & font_text_end & "</td></tr>")
                        Else
                            htmlOut.Append("</tr>")
                        End If




                    Next

                    If Trim(from_spot) = "company" Or Trim(from_spot) = "pdf" Then
                    Else
                        htmlOut.Append("</table></p></div></td></tr>")
                    End If


                Else
                    htmlOut.Append("<tr><td valign='top' align='left' class='seperator' style='padding-left:3px;'><br/>No lessors could be found that meet your search criteria</td></tr>")
                End If

            Else
                htmlOut.Append("<tr><td valign='top' align='left' class='seperator' style='padding-left:3px;'><br/>No lessors could be found that meet your search criteria</td></tr>")
            End If

            htmlOut.Append("</table>")

        Catch ex As Exception

            aError = "Error in views_display_top_lessors(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

#End Region

End Class
