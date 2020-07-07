' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/star_view_functions.vb $
'$$Author: Matt $
'$$Date: 6/22/20 9:24a $
'$$Modtime: 6/22/20 9:06a $
'$$Revision: 7 $
'$$Workfile: star_view_functions.vb $
'
' ********************************************************************************

<System.Serializable()> Public Class star_view_functions

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

#Region "star_tab_functions"

    Public Function get_max_star_report_date_Number(ByRef searchCriteria As viewSelectionCriteriaClass, ByVal table_num As Integer, Optional ByVal bFromMarketSummary As Boolean = False) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Dim sSeperator As String = " WHERE "

        Try

            sQuery.Append("SELECT MAX(ac" & table_num & "_start_date) AS ac" & table_num & "_start_date FROM Aircraft_" & table_num & " WITH(NOLOCK)")

            sQuery.Append(" WHERE (ac" & table_num & "_amod_id = '" + searchCriteria.ViewCriteriaAmodID.ToString + "')")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_max_star_report_date_Number(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = starConnectString
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
                aError = "Error in get_max_star_report_date load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_max_star_report_date(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Function get_max_star_report_date(ByRef searchCriteria As viewSelectionCriteriaClass, Optional ByVal bFromMarketSummary As Boolean = False) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Dim sSeperator As String = " WHERE "

        Try

            sQuery.Append("SELECT MAX(ac1_start_date) AS ac1_start_date FROM Aircraft_1 WITH(NOLOCK)")

            If bFromMarketSummary Then

                If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAirframeTypeStr.Trim) Then
                    sQuery.Append(sSeperator + "(ac1_airframe_type IN (" + searchCriteria.ViewCriteriaAirframeTypeStr.ToString + "))")
                    sSeperator = " AND "
                End If

                If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftType.Trim) Then
                    sQuery.Append(sSeperator + "(ac1_maketype IN (" + searchCriteria.ViewCriteriaAircraftType.ToString + "))")
                    sSeperator = " AND "
                End If

                If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                    sQuery.Append(sSeperator + "(ac1_make IN (" + searchCriteria.ViewCriteriaAircraftMake.ToString + "))")
                    sSeperator = " AND "
                End If

                If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftModel.Trim) Then
                    sQuery.Append(sSeperator + "(ac1_amod_id IN (" + searchCriteria.ViewCriteriaAircraftModel.ToString + "))")
                End If

            Else
                sQuery.Append(" WHERE (ac1_amod_id = '" + searchCriteria.ViewCriteriaAmodID.ToString + "')")
            End If

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_max_star_report_date(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = starConnectString
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
                aError = "Error in get_max_star_report_date load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_max_star_report_date(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Function get_star_aircraft_1_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT ac1_make, ac1_model, ac1_total_fleet, ac1_aircraft_in_production, ac1_percentage_in_production, ")
            sQuery.Append("ac1_aircraft_in_operation, ac1_percentage_in_operation, ac1_aircraft_out_of_operation, ")
            sQuery.Append("ac1_percentage_out_of_operation ")
            sQuery.Append("FROM Aircraft_1 WITH(NOLOCK) ")
            sQuery.Append("WHERE (ac1_amod_id = '" + searchCriteria.ViewCriteriaAmodID.ToString + "') AND (ac1_start_date = '" + FormatDateTime(CDate(searchCriteria.ViewCriteriaStarReportDate.ToString), DateFormat.ShortDate).ToString + "')")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_star_aircraft_1_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = starConnectString
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
                aError = "Error in get_star_aircraft_1_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_star_aircraft_1_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Sub views_display_star_aircraft_1(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)


        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder

        Try

            results_table = get_star_aircraft_1_info(searchCriteria)
            htmlOut.Append("<table cellpadding=""2"" cellspacing=""0"" border=""1"" width=""800"">") 'start inner model star table

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    Dim ac1_make As String = results_table.Rows(0).Item("ac1_make").ToString.Trim
                    Dim ac1_model As String = results_table.Rows(0).Item("ac1_model").ToString.Trim

                    For Each r As DataRow In results_table.Rows

                        ' table header
                        htmlOut.Append("<tr><th class='th_title' align='center' colspan='9'>Aircraft Lifecycle - " + ac1_make + " " + ac1_model + "</th></tr>")

                        ' column headers
                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='th_title_details' align='left'>Make</th>")
                        htmlOut.Append("<th class='th_title_details' align='left'>Model</th>")
                        htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>Total<br />Fleet</th>")
                        htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>Aircraft In<br />Production</th>")
                        htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>Percentage<br />In Production</th>")
                        htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>Aircraft In<br />Operation</th>")
                        htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>Percentage<br />In Operation</th>")
                        htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>Aircraft Out<br />Of Operation</th>")
                        htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>Percentage Out<br />Of Operation</th>")
                        htmlOut.Append("</tr>")

                        ' column data
                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='beige' title='Make' nowrap=""nowrap"" align='left'>" + ac1_make + "</th>")
                        htmlOut.Append("<th class='beige2' title='Model' nowrap=""nowrap"" align='left'>" + ac1_model + "</th>")
                        htmlOut.Append("<td class='lavender' title='" + ac1_make + " " + ac1_model + " - Total Fleet' align='right'>" + r.Item("ac1_total_fleet").ToString + "</td>")
                        htmlOut.Append("<td class='mistyrose' title='" + ac1_make + " " + ac1_model + " - Aircraft In Production' align='right'>" + r.Item("ac1_aircraft_in_production").ToString + "</td>")
                        htmlOut.Append("<td class='mistyrose' title='" + ac1_make + " " + ac1_model + " - Percentage In Production' align='right'>" + r.Item("ac1_percentage_in_production").ToString + "%</td>")
                        htmlOut.Append("<td class='lightgreen' title='" + ac1_make + " " + ac1_model + " - Aircraft In Operation' align='right'>" + r.Item("ac1_aircraft_in_operation").ToString + "</td>")
                        htmlOut.Append("<td class='lightgreen' title='" + ac1_make + " " + ac1_model + " - Percentage In Operation' align='right'>" + r.Item("ac1_percentage_in_operation").ToString + "%</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac1_make + " " + ac1_model + " - Aircraft Out Of Operation' align='right'>" + r.Item("ac1_aircraft_out_of_operation").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac1_make + " " + ac1_model + " - Percentage Out Of Operation' align='right'>" + r.Item("ac1_percentage_out_of_operation").ToString + "%</td>")
                        htmlOut.Append("</tr>")

                    Next
                Else
                    htmlOut.Append("<tr><td align='center' valign='middle'><br />No data available for the model you have selected</td></tr>")
                End If
            Else
                htmlOut.Append("<tr><td align='center' valign='middle'><br />No data available for the model you have selected</td></tr>")
            End If

            htmlOut.Append("</table>") 'end inner model star table

        Catch ex As Exception

            aError = "Error in views_display_star_aircraft_1(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function get_star_aircraft_2_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT ac2_make, ac2_model, ac2_aircraft_for_sale, ac2_aircraft_in_operation, ac2_percentange_for_sale ")
            sQuery.Append("FROM Aircraft_2 WITH(NOLOCK) ")
            sQuery.Append("WHERE (ac2_amod_id = '" + searchCriteria.ViewCriteriaAmodID.ToString + "') AND (ac2_start_date = '" + FormatDateTime(CDate(searchCriteria.ViewCriteriaStarReportDate.ToString), DateFormat.ShortDate).ToString + "')")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_star_aircraft_2_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = starConnectString
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
                aError = "Error in get_star_aircraft_2_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_star_aircraft_2_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Sub views_display_star_aircraft_2(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder

        Try

            results_table = get_star_aircraft_2_info(searchCriteria)
            htmlOut.Append("<table cellpadding=""2"" cellspacing=""0"" border=""1"" width=""800"">") 'start inner model star table

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    Dim ac2_make As String = results_table.Rows(0).Item("ac2_make").ToString.Trim
                    Dim ac2_model As String = results_table.Rows(0).Item("ac2_model").ToString.Trim

                    For Each r As DataRow In results_table.Rows

                        ' table header
                        htmlOut.Append("<tr><th class='th_title' align='center' colspan='5'>Percentage of Fleet For Sale by Make/Model<br />Aircraft Status:" + ac2_make + " " + ac2_model + "</th></tr>")

                        ' column headers
                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='th_title_details' align='left'>Make</th>")
                        htmlOut.Append("<th class='th_title_details' align='left'>Model</th>")
                        htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>Number<br />For Sale</th>")
                        htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>Number<br />In Operation</th>")
                        htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>Percent<br />For Sale</th>")
                        htmlOut.Append("</tr>")

                        ' column data
                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='beige' title='Make' nowrap=""nowrap"" align='left'>" + ac2_make + "</th>")
                        htmlOut.Append("<th class='beige2' title='Model' nowrap=""nowrap"" align='left'>" + ac2_model + "</th>")
                        htmlOut.Append("<td class='lavender' title='" + ac2_make + " " + ac2_model + " - Number For Sale' align='right'>" + r.Item("ac2_aircraft_for_sale").ToString + "</td>")
                        htmlOut.Append("<td class='mistyrose' title='" + ac2_make + " " + ac2_model + " - Number In Operation' align='right'>" + r.Item("ac2_aircraft_in_operation").ToString + "</td>")
                        htmlOut.Append("<td class='mistyrose' title='" + ac2_make + " " + ac2_model + " - Percentage For Sale' align='right'>" + r.Item("ac2_percentange_for_sale").ToString + "%</td>")
                        htmlOut.Append("</tr>")

                    Next
                Else
                    htmlOut.Append("<tr><td align='center' valign='middle'><br />No data available for the model you have selected</td></tr>")
                End If
            Else
                htmlOut.Append("<tr><td align='center' valign='middle'><br />No data available for the model you have selected</td></tr>")
            End If

            htmlOut.Append("</table>") 'end inner model star table

        Catch ex As Exception

            aError = "Error in views_display_star_aircraft_2(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function get_star_aircraft_3_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT ac3_make, ac3_model, ac3_aircraft_for_sale, ac3_aircraft_in_operation, ac3_percentange_for_sale, ")
            sQuery.Append("ac3_1_month_nbr_trans, ac3_3_months_nbr_trans, ac3_6_months_nbr_trans, ac3_12_months_nbr_trans ")
            sQuery.Append("FROM Aircraft_3 WITH(NOLOCK) ")
            sQuery.Append("WHERE (ac3_amod_id = '" + searchCriteria.ViewCriteriaAmodID.ToString + "') AND (ac3_start_date = '" + FormatDateTime(CDate(searchCriteria.ViewCriteriaStarReportDate.ToString), DateFormat.ShortDate).ToString + "')")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_star_aircraft_3_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = starConnectString
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
                aError = "Error in get_star_aircraft_3_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_star_aircraft_3_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Sub views_display_star_aircraft_3(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder

        Try

            results_table = get_star_aircraft_3_info(searchCriteria)
            htmlOut.Append("<table cellpadding=""2"" cellspacing=""0"" border=""1"" width=""800"">") 'start inner model star table

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    Dim ac3_make As String = results_table.Rows(0).Item("ac3_make").ToString.Trim
                    Dim ac3_model As String = results_table.Rows(0).Item("ac3_model").ToString.Trim

                    For Each r As DataRow In results_table.Rows

                        ' table header
                        htmlOut.Append("<tr><th class='th_title' align='center' colspan='9'>&nbsp;Aircraft Status: (NEW &amp;USED)&nbsp;<br />" + ac3_make + " " + ac3_model + "</th></tr>")
                        htmlOut.Append("<tr><th class='th_title' align='center' colspan='9'>Full Sale Transactions<br />Number of Transactions</th></tr>")

                        ' column headers
                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='th_title_details' align='left'>Make</th>")
                        htmlOut.Append("<th class='th_title_details' align='left'>Model</th>")
                        htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>Number<br />For Sale</th>")
                        htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>Number<br />In Operation</th>")
                        htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>Percent<br />For Sale</th>")
                        htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>&nbsp;1<br />Month&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>&nbsp;1<br />Month&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>&nbsp;6<br />Months&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>&nbsp;12<br />Months&nbsp;</th>")
                        htmlOut.Append("</tr>")

                        ' column data
                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='beige' title='Make' nowrap=""nowrap"" align='left'>" + ac3_make + "</th>")
                        htmlOut.Append("<th class='beige2' title='Model' nowrap=""nowrap"" align='left'>" + ac3_model + "</th>")
                        htmlOut.Append("<td class='beige2' title='" + ac3_make + " " + ac3_model + " - Number For Sale' align='right'>" + r.Item("ac3_aircraft_for_sale").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac3_make + " " + ac3_model + " - Number In Operation' align='right'>" + r.Item("ac3_aircraft_in_operation").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac3_make + " " + ac3_model + " - Percentage For Sale' align='right'>" + r.Item("ac3_percentange_for_sale").ToString + "%</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac3_make + " " + ac3_model + " - 1 Month' align='right'>" + r.Item("ac3_1_month_nbr_trans").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac3_make + " " + ac3_model + " - 3 Month' align='right'>" + r.Item("ac3_3_months_nbr_trans").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac3_make + " " + ac3_model + " - 6 Month' align='right'>" + r.Item("ac3_6_months_nbr_trans").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac3_make + " " + ac3_model + " - 12 Month' align='right'>" + r.Item("ac3_12_months_nbr_trans").ToString + "</td>")
                        htmlOut.Append("</tr>")

                    Next
                Else
                    htmlOut.Append("<tr><td align='center' valign='middle'><br />No data available for the model you have selected</td></tr>")
                End If
            Else
                htmlOut.Append("<tr><td align='center' valign='middle'><br />No data available for the model you have selected</td></tr>")
            End If

            htmlOut.Append("</table>") 'end inner model star table

        Catch ex As Exception

            aError = "Error in views_display_star_aircraft_3(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function get_star_aircraft_4_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT ac4_make, ac4_model, ac4_new_nbr_trans, ac4_new_days_owned, ac4_new_months_owned, ")
            sQuery.Append("ac4_new_years_owned, ac4_used_nbr_trans, ac4_used_days_owned, ac4_used_months_owned, ac4_used_years_owned ")
            sQuery.Append("FROM Aircraft_4 WITH(NOLOCK) ")
            sQuery.Append("WHERE (ac4_amod_id = '" + searchCriteria.ViewCriteriaAmodID.ToString + "') AND (ac4_start_date = '" + FormatDateTime(CDate(searchCriteria.ViewCriteriaStarReportDate.ToString), DateFormat.ShortDate).ToString + "')")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_star_aircraft_4_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = starConnectString
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
                aError = "Error in get_star_aircraft_4_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_star_aircraft_4_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Sub views_display_star_aircraft_4(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder

        Try

            results_table = get_star_aircraft_4_info(searchCriteria)
            htmlOut.Append("<table cellpadding=""2"" cellspacing=""0"" border=""1"" width=""800"">") 'start inner model star table

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    Dim ac4_make As String = results_table.Rows(0).Item("ac4_make").ToString.Trim
                    Dim ac4_model As String = results_table.Rows(0).Item("ac4_model").ToString.Trim

                    For Each r As DataRow In results_table.Rows

                        ' table header
                        htmlOut.Append("<tr><th class='th_title' align='center' colspan='10'>Length of Ownership - " + ac4_make + " " + ac4_model + "</th></tr>")

                        htmlOut.Append("<tr><th class='th_title' align='center' colspan='2'>WHOLLY OWNED AIRCRAFT</th>")
                        htmlOut.Append("<th class='th_title' align='center' colspan='8'>AVERAGE LENGTH OF OWNERSHIP</th></tr>")

                        htmlOut.Append("<tr><th class='th_title' align='center' colspan='2'>&nbsp;" + ac4_make + " " + ac4_model + "&nbsp;</th>")
                        htmlOut.Append("<th class='th_title' align='center' colspan='4'>&nbsp;NEW&nbsp;</th>")
                        htmlOut.Append("<th class='th_title' align='center' colspan='4'>&nbsp;USED&nbsp;</th></tr>")

                        ' column headers
                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='th_title_details' align='left'>Make</th>")
                        htmlOut.Append("<th class='th_title_details' align='left'>Model</th>")
                        htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>NBR<br />TRANS&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>&nbsp;DAYS&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>&nbsp;MONTHS&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>&nbsp;YEARS&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>NBR<br />TRANS&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>&nbsp;DAYS&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>&nbsp;MONTHS&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>&nbsp;YEARS&nbsp;</th>")
                        htmlOut.Append("</tr>")

                        ' column data
                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='beige' title='Make' nowrap=""nowrap"" align='left'>" + ac4_make + "</th>")
                        htmlOut.Append("<th class='beige2' title='Model' nowrap=""nowrap"" align='left'>" + ac4_model + "</th>")
                        htmlOut.Append("<td class='beige2' title='" + ac4_make + " " + ac4_model + " - Number Of New Transactions' align='right'>" + r.Item("ac4_new_nbr_trans").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac4_make + " " + ac4_model + " - New Aircraft - In Days' align='right'>" + r.Item("ac4_new_days_owned").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac4_make + " " + ac4_model + " - New Aircraft - In Months' align='right'>" + r.Item("ac4_new_months_owned").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac4_make + " " + ac4_model + " - New Aircraft - In Years' align='right'>" + r.Item("ac4_new_years_owned").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac4_make + " " + ac4_model + " - Number Of Used Transactions' align='right'>" + r.Item("ac4_used_nbr_trans").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac4_make + " " + ac4_model + " - Used Aircraft - In Days' align='right'>" + r.Item("ac4_used_days_owned").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac4_make + " " + ac4_model + " - Used Aircraft - In Months' align='right'>" + r.Item("ac4_used_months_owned").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac4_make + " " + ac4_model + " - Used Aircraft - In Years' align='right'>" + r.Item("ac4_used_years_owned").ToString + "</td>")
                        htmlOut.Append("</tr>")

                    Next
                Else
                    htmlOut.Append("<tr><td align='center' valign='middle'><br />No data available for the model you have selected</td></tr>")
                End If
            Else
                htmlOut.Append("<tr><td align='center' valign='middle'><br />No data available for the model you have selected</td></tr>")
            End If

            htmlOut.Append("</table>") 'end inner model star table

        Catch ex As Exception

            aError = "Error in views_display_star_aircraft_4(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function get_star_aircraft_5_info(ByRef searchCriteria As viewSelectionCriteriaClass, Optional ByVal bFromMarketSummary As Boolean = False) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT ac5_make, ac5_model, ac5_total_sold, ac5_total_upgrades, ac5_percentage_with_upgrade, ")
            sQuery.Append("ac5_upgrade_to_make, ac5_upgrade_to_model, ac5_upgrade_to_total_upgrades, ")
            sQuery.Append("ac5_upgrade_to_percentage_with_upgrade, ac5_upgrade_to_aircraft_for_sale, ")
            sQuery.Append("ac5_upgrade_to_percentage_for_sale ")
            sQuery.Append("FROM Aircraft_5 WITH(NOLOCK) ")

            If bFromMarketSummary Then
                sQuery.Append(" WHERE (ac5_start_date = '" + FormatDateTime(CDate(searchCriteria.ViewCriteriaStarReportDate.ToString), DateFormat.ShortDate).ToString + "')")

                If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAirframeTypeStr.Trim) Then
                    sQuery.Append(" AND (ac5_airframe_type IN (" + searchCriteria.ViewCriteriaAirframeTypeStr.ToString + "))")
                End If

                If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftType.Trim) Then
                    sQuery.Append(" AND (ac5_maketype IN (" + searchCriteria.ViewCriteriaAircraftType.ToString + "))")
                End If

                If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                    sQuery.Append(" AND (ac5_make IN (" + searchCriteria.ViewCriteriaAircraftMake.ToString + "))")
                End If

                If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftModel.Trim) Then
                    sQuery.Append(" AND (ac5_amod_id IN (" + searchCriteria.ViewCriteriaAircraftModel.ToString + "))")
                End If

            Else
                sQuery.Append("WHERE (ac5_amod_id = '" + searchCriteria.ViewCriteriaAmodID.ToString + "') AND (ac5_start_date = '" + FormatDateTime(CDate(searchCriteria.ViewCriteriaStarReportDate.ToString), DateFormat.ShortDate).ToString + "')")
            End If


            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_star_aircraft_5_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = starConnectString
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
                aError = "Error in get_star_aircraft_5_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_star_aircraft_5_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Sub views_display_star_aircraft_5(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, Optional ByVal bFromMarketSummary As Boolean = False)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim bFirstTime As Boolean = False

        Try

            results_table = get_star_aircraft_5_info(searchCriteria, bFromMarketSummary)
            htmlOut.Append("<table cellpadding=""2"" cellspacing=""0"" border=""1"" width=""800"">") 'start inner model star table

            If Not IsNothing(results_table) Then

                Dim ac5_make As String = ""
                Dim ac5_model As String = ""

                If results_table.Rows.Count > 0 Then

                    For Each r As DataRow In results_table.Rows

                        If (Not ac5_make.Trim.ToLower.Contains(r.Item("ac5_make").ToString.Trim.ToLower)) Or
                           (Not ac5_model.Trim.ToLower.Contains(r.Item("ac5_model").ToString.Trim.ToLower)) Then

                            ac5_make = r.Item("ac5_make").ToString.Trim
                            ac5_model = r.Item("ac5_model").ToString.Trim

                            ' table header
                            htmlOut.Append("<tr><th class='th_title' align='center' colspan='11'>Upgrade To Path Model")
                            htmlOut.Append("<br />Based on End User Transactions")
                            htmlOut.Append("<br />" + ac5_make + " " + ac5_model + "</th></tr>")
                            htmlOut.Append("<tr><th class='beige' align='center' colspan='5'>AN OWNER OF</th>")
                            htmlOut.Append("<th class='beige' align='center' colspan='6'>WILL MOST LIKELY BUY</th></tr>")
                            htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center' colSpan='2'>STARTED WITH</th>")
                            htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center' rowSpan='2'>TOTAL<br />SOLD</th>")
                            htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center' rowSpan='2'>TOTAL<br />UPGRADES</th>")
                            htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center' rowSpan='2'>PERCENT<br />WITH<br />UPGRADE<br />PATH</th>")
                            htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center' colSpan='2'>UPGRADED TO</th>")
                            htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center' colSpan='2'>UPGRADES</th>")
                            htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center' colSpan='2'>FOR SALE</th></tr>")

                            ' column headers
                            htmlOut.Append("<tr>")
                            htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>MAKE</th>")
                            htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>MODEL</th>")
                            htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>MAKE</th>")
                            htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>MODEL</th>")
                            htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>TOTAL</th>")
                            htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>PERCENT</th>")
                            htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>TOTAL</th>")
                            htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>PERCENT</th>")
                            htmlOut.Append("</tr>")

                            ' column data
                            htmlOut.Append("<tr>")

                            bFirstTime = False

                        End If

                        If CLng(r.Item("ac5_percentage_with_upgrade").ToString) = 0 Then

                            htmlOut.Append("<td class='lightgrey' title='" + ac5_make + " " + ac5_model + " - No Aircraft Upgrades Found' align='center' colSpan='11'>No Aircraft Upgrades Found</td></tr>")

                            If Not bFromMarketSummary Then
                                Exit For
                            End If

                        Else

                            If Not bFirstTime Then

                                bFirstTime = True

                                htmlOut.Append("<th class='beige' title='Make' nowrap='nowrap' align='left'>" + ac5_make + "</th>")
                                htmlOut.Append("<th class='beige2' title='Model' nowrap='nowrap' align='left'>" + ac5_model + "</th>")
                                htmlOut.Append("<td class='beige2' title='" + ac5_make + " " + ac5_model + " - Total Number Of Sold Reports' nowrap='nowrap' align='left'>" + r.Item("ac5_total_sold").ToString + "</td>")
                                htmlOut.Append("<td class='beige2' title='" + ac5_make + " " + ac5_model + " - Total Number Of Upgrades Found' nowrap='nowrap' align='left'>" + r.Item("ac5_total_upgrades").ToString + "</td>")
                                htmlOut.Append("<td class='beige2' title='" + ac5_make + " " + ac5_model + " - Percentage Of Sold Reports With An Upgrade Path' nowrap='nowrap' align='left'>")

                                ' check to see if there is no data for for the percentage with upgrade
                                ' if = 0 then set the htmlOuptut to 0 if not set the value
                                If CLng(r.Item("ac5_percentage_with_upgrade").ToString) = 0 Then
                                    htmlOut.Append("N/A")
                                Else
                                    htmlOut.Append(r.Item("ac5_percentage_with_upgrade").ToString + "%")
                                End If
                                htmlOut.Append("</td>")

                                htmlOut.Append("<td class='lightcyan' title='" + r.Item("ac5_upgrade_to_make").ToString + " " + r.Item("ac5_upgrade_to_model").ToString + " - Upgraded To Make' align='center'>" + r.Item("ac5_upgrade_to_make").ToString + "</td>")
                                htmlOut.Append("<td class='lightcyan' title='" + r.Item("ac5_upgrade_to_make").ToString + " " + r.Item("ac5_upgrade_to_model").ToString + " - Upgraded To Model' align='center'>" + r.Item("ac5_upgrade_to_model").ToString + "</td>")
                                htmlOut.Append("<td class='beige3' title='" + r.Item("ac5_upgrade_to_make").ToString + " " + r.Item("ac5_upgrade_to_model").ToString + " - Total Upgrades Found Per Model' align='right'>" + r.Item("ac5_upgrade_to_total_upgrades").ToString + "</td>")
                                htmlOut.Append("<td class='beige3' title='" + r.Item("ac5_upgrade_to_make").ToString + " " + r.Item("ac5_upgrade_to_model").ToString + " - Percentage Of Upgrades vs Sold Reports Found' align='right'>" + r.Item("ac5_upgrade_to_percentage_with_upgrade").ToString + "%</td>")
                                htmlOut.Append("<td class='beige4' title='" + r.Item("ac5_upgrade_to_make").ToString + " " + r.Item("ac5_upgrade_to_model").ToString + " - Total Number For Sale' align='right'>" + r.Item("ac5_upgrade_to_aircraft_for_sale").ToString + "</td>")
                                htmlOut.Append("<td class='beige4' title='" + r.Item("ac5_upgrade_to_make").ToString + " " + r.Item("ac5_upgrade_to_model").ToString + " - Total Percentage For Sale' align='right'>" + r.Item("ac5_upgrade_to_percentage_for_sale").ToString + "%</td>")

                            Else

                                htmlOut.Append("<td class='beige4' colSpan='5'>&nbsp;</td>")
                                htmlOut.Append("<td class='lightcyan' title='" + r.Item("ac5_upgrade_to_make").ToString + " " + r.Item("ac5_upgrade_to_model").ToString + " - Upgraded To Make' align='center'>" + r.Item("ac5_upgrade_to_make").ToString + "</td>")
                                htmlOut.Append("<td class='lightcyan' title='" + r.Item("ac5_upgrade_to_make").ToString + " " + r.Item("ac5_upgrade_to_model").ToString + " - Upgraded To Model' align='center'>" + r.Item("ac5_upgrade_to_model").ToString + "</td>")
                                htmlOut.Append("<td class='beige3' title='" + r.Item("ac5_upgrade_to_make").ToString + " " + r.Item("ac5_upgrade_to_model").ToString + " - Total Upgrades Found Per Model' align='right'>" + r.Item("ac5_upgrade_to_total_upgrades").ToString + "</td>")
                                htmlOut.Append("<td class='beige3' title='" + r.Item("ac5_upgrade_to_make").ToString + " " + r.Item("ac5_upgrade_to_model").ToString + " - Percentage Of Upgrades vs Sold Reports Found' align='right'>" + r.Item("ac5_upgrade_to_percentage_with_upgrade").ToString + "%</td>")
                                htmlOut.Append("<td class='beige4' title='" + r.Item("ac5_upgrade_to_make").ToString + " " + r.Item("ac5_upgrade_to_model").ToString + " - Total Number For Sale' align='right'>" + r.Item("ac5_upgrade_to_aircraft_for_sale").ToString + "</td>")
                                htmlOut.Append("<td class='beige4' title='" + r.Item("ac5_upgrade_to_make").ToString + " " + r.Item("ac5_upgrade_to_model").ToString + " - Total Percentage For Sale' align='right'>" + r.Item("ac5_upgrade_to_percentage_for_sale").ToString + "%</td>")

                            End If

                            htmlOut.Append("</tr>")
                        End If

                    Next
                Else
                    htmlOut.Append("<tr><td align='center' valign='middle'><br />No data available for the model you have selected</td></tr>")
                End If
            Else
                htmlOut.Append("<tr><td align='center' valign='middle'><br />No data available for the model you have selected</td></tr>")
            End If

            htmlOut.Append("</table>") 'end inner model star table

        Catch ex As Exception

            aError = "Error in views_display_star_aircraft_5(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function get_star_aircraft_6_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT ac6_make, ac6_model, ac6_africa, ac6_asia, ac6_australia, ac6_europe, ac6_northamerica, ac6_southamerica, ac6_total_ac_based ")
            sQuery.Append("FROM Aircraft_6 WITH(NOLOCK) ")
            sQuery.Append("WHERE (ac6_amod_id = '" + searchCriteria.ViewCriteriaAmodID.ToString + "') AND (ac6_start_date = '" + FormatDateTime(CDate(searchCriteria.ViewCriteriaStarReportDate.ToString), DateFormat.ShortDate).ToString + "')")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_star_aircraft_6_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = starConnectString
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
                aError = "Error in get_star_aircraft_6_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_star_aircraft_6_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Sub views_display_star_aircraft_6(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder

        Try

            results_table = get_star_aircraft_6_info(searchCriteria)
            htmlOut.Append("<table cellpadding=""2"" cellspacing=""0"" border=""1"" width=""800"">") 'start inner model star table

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    Dim ac6_make As String = results_table.Rows(0).Item("ac6_make").ToString.Trim
                    Dim ac6_model As String = results_table.Rows(0).Item("ac6_model").ToString.Trim

                    For Each r As DataRow In results_table.Rows

                        ' table header
                        htmlOut.Append("<tr><th class='th_title' align='center' colspan='9'>Location of Aircraft by Continent - Based In<br />" + ac6_make + " " + ac6_model + "</th></tr>")

                        ' column headers
                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='th_title_details' align='left'>Make</th>")
                        htmlOut.Append("<th class='th_title_details' align='left'>Model</th>")
                        htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>AFRICA</th>")
                        htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>ASIA</th>")
                        htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>AUSTRALIA<br />OCEANIA</th>")
                        htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>EUROPE</th>")
                        htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>NORTH<br />AMERICA</th>")
                        htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>SOUTH<br />AMERICA</th>")
                        htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>TOTAL</th>")
                        htmlOut.Append("</tr>")

                        ' column data
                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='beige' title='Make' noWrap align='left'>" + ac6_make + "</th>")
                        htmlOut.Append("<th class='beige2' title='Model' noWrap align='left'>" + ac6_model + "</th>")
                        htmlOut.Append("<td class='beige2' title='" + ac6_make + " " + ac6_model + " - Africa' align='right'>" + r.Item("ac6_africa").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6_make + " " + ac6_model + " - Asia' align='right'>" + r.Item("ac6_asia").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6_make + " " + ac6_model + " - Australia/Oceania' align='right'>" + r.Item("ac6_australia").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6_make + " " + ac6_model + " - Europe' align='right'>" + r.Item("ac6_europe").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6_make + " " + ac6_model + " - North America' align='right'>" + r.Item("ac6_northamerica").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6_make + " " + ac6_model + " - South America' align='right'>" + r.Item("ac6_southamerica").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6_make + " " + ac6_model + " - Total Active Fleet' align='right'>" + r.Item("ac6_total_ac_based").ToString + "</td>")
                        htmlOut.Append("</tr>")

                    Next
                Else
                    htmlOut.Append("<tr><td align='center' valign='middle'><br />No data available for the model you have selected</td></tr>")
                End If
            Else
                htmlOut.Append("<tr><td align='center' valign='middle'><br />No data available for the model you have selected</td></tr>")
            End If

            htmlOut.Append("</table>") 'end inner model star table

        Catch ex As Exception

            aError = "Error in views_display_star_aircraft_6(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function get_star_aircraft_6a_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT ac6a_make, ac6a_model, ac6a_africa_acbase_total, ac6a_africa_acbase_forsale, ac6a_africa_registered_total, ac6a_africa_registered_forsale, ")
            sQuery.Append("ac6a_africa_operated_total, ac6a_africa_operated_forsale, ac6a_africa_owned_total, ac6a_africa_owned_forsale, ac6a_asia_acbase_total, ")
            sQuery.Append("ac6a_asia_acbase_forsale, ac6a_asia_registered_total, ac6a_asia_registered_forsale, ac6a_asia_operated_total, ac6a_asia_operated_forsale, ")
            sQuery.Append("ac6a_asia_owned_total, ac6a_asia_owned_forsale, ac6a_australia_acbase_total, ac6a_australia_acbase_forsale, ac6a_australia_registered_total, ")
            sQuery.Append("ac6a_australia_registered_forsale, ac6a_australia_operated_total, ac6a_australia_operated_forsale, ac6a_australia_owned_total, ac6a_australia_owned_forsale, ")
            sQuery.Append("ac6a_europe_acbase_total, ac6a_europe_acbase_forsale, ac6a_europe_registered_total, ac6a_europe_registered_forsale, ac6a_europe_operated_total, ")
            sQuery.Append("ac6a_europe_operated_forsale, ac6a_europe_owned_total, ac6a_europe_owned_forsale, ac6a_northamerica_acbase_total, ac6a_northamerica_acbase_forsale, ")
            sQuery.Append("ac6a_northamerica_registered_total, ac6a_northamerica_registered_forsale, ac6a_northamerica_operated_total, ac6a_northamerica_operated_forsale, ")
            sQuery.Append("ac6a_northamerica_owned_total, ac6a_northamerica_owned_forsale, ac6a_southamerica_acbase_total, ac6a_southamerica_acbase_forsale, ")
            sQuery.Append("ac6a_southamerica_registered_total, ac6a_southamerica_registered_forsale, ac6a_southamerica_operated_total, ac6a_southamerica_operated_forsale, ")
            sQuery.Append("ac6a_southamerica_owned_total, ac6a_southamerica_owned_forsale, ac6a_unknown_acbase_total, ac6a_unknown_acbase_forsale, ac6a_unknown_registered_total, ")
            sQuery.Append("ac6a_unknown_registered_forsale, ac6a_unknown_operated_total, ac6a_unknown_operated_forsale, ac6a_unknown_owned_total, ac6a_unknown_owned_forsale, ")
            sQuery.Append("ac6a_total_fleet, ac6a_total_fleet_forsale ")
            sQuery.Append("FROM Aircraft_6a WITH(NOLOCK)")
            sQuery.Append("WHERE (ac6a_make = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "') AND (ac6a_model = '" + searchCriteria.ViewCriteriaAircraftModel.Trim + "') AND (ac6a_start_date = '" + FormatDateTime(CDate(searchCriteria.ViewCriteriaStarReportDate.ToString), DateFormat.ShortDate).ToString + "')")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_star_aircraft_6a_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = starConnectString
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
                aError = "Error in get_star_aircraft_6a_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_star_aircraft_6a_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Sub views_display_star_aircraft_6a(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder

        Try

            results_table = get_star_aircraft_6a_info(searchCriteria)
            htmlOut.Append("<table cellpadding=""2"" cellspacing=""0"" border=""1"" width=""800"">") 'start inner model star table

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    Dim ac6a_make As String = results_table.Rows(0).Item("ac6a_make").ToString.Trim
                    Dim ac6a_model As String = results_table.Rows(0).Item("ac6a_model").ToString.Trim

                    For Each r As DataRow In results_table.Rows

                        ' table header
                        htmlOut.Append("<tr><th class='th_title' align='center' colspan='18'>Aircraft Make/Model by Continent<br />Based, Registered, Operated and Owned<br />WHOLLY OWNED - IN OPERATION<br />")
                        htmlOut.Append(ac6a_make + " " + ac6a_model + "</th></tr>")

                        ' column headers
                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='th_title_details' align='left' rowSpan='3'>MAKE</th>")
                        htmlOut.Append("<th class='th_title_details' align='left' rowSpan='3'>MODEL</th>")
                        htmlOut.Append("<th class='th_title' align='center' colSpan='8'>AFRICA</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colSpan='8'>ASIA</th>")
                        htmlOut.Append("</tr>")

                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='th_title' align='center' colSpan='2'>A/C BASED</th>")
                        htmlOut.Append("<th class='th_title' align='center' colSpan='2'>REGISTERED</th>")
                        htmlOut.Append("<th class='th_title' align='center' colSpan='2'>OPERATED</th>")
                        htmlOut.Append("<th class='th_title' align='center' colSpan='2'>OWNED</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colSpan='2'>A/C BASED</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colSpan='2'>REGISTERED</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colSpan='2'>OPERATED</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colSpan='2'>OWNED</th>")
                        htmlOut.Append("</tr>")

                        ' africa
                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='th_title' align='center'>TOTAL</th>")
                        htmlOut.Append("<th class='th_title' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title' align='center'>TOTAL</th>")
                        htmlOut.Append("<th class='th_title' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title' align='center'>TOTAL</th>")
                        htmlOut.Append("<th class='th_title' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title' align='center'>TOTAL</th>")
                        htmlOut.Append("<th class='th_title' align='center'>FOR<br />SALE</th>")

                        ' asia
                        htmlOut.Append("<th class='th_title_details' align='center'>TOTAL</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>TOTAL</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>TOTAL</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>TOTAL</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("</tr>")

                        ' column data
                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='th_title' title='Make' noWrap align='left' rowspan='18'>" + ac6a_make + "</th>")
                        htmlOut.Append("<th class='beige2' title='Model' noWrap align='left' rowspan='18'>" + ac6a_model + "</th>")

                        ' africa
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_africa_acbase_total").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_africa_acbase_forsale").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_africa_registered_total").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_africa_registered_forsale").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_africa_operated_total").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_africa_operated_forsale").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_africa_owned_total").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_africa_owned_forsale").ToString + "</td>")

                        ' asia
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_asia_acbase_total").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_asia_acbase_forsale").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_asia_registered_total").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_asia_registered_forsale").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_asia_operated_total").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_asia_operated_forsale").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_asia_owned_total").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_asia_owned_forsale").ToString + "</td>")
                        htmlOut.Append("</tr>")

                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='th_title' align='center' colSpan='8'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colSpan='8'>&nbsp;</th>")
                        htmlOut.Append("</tr>")

                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='th_title' align='center' colSpan='8'>AUSTRALIA<br />OCEANIA</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colSpan='8'>EUROPE</th>")
                        htmlOut.Append("</tr>")

                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='th_title' align='center' colSpan='2'>A/C BASED</th>")
                        htmlOut.Append("<th class='th_title' align='center' colSpan='2'>REGISTERED</th>")
                        htmlOut.Append("<th class='th_title' align='center' colSpan='2'>OPERATED</th>")
                        htmlOut.Append("<th class='th_title' align='center' colSpan='2'>OWNED</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colSpan='2'>A/C BASED</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colSpan='2'>REGISTERED</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colSpan='2'>OPERATED</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colSpan='2'>OWNED</th>")
                        htmlOut.Append("</tr>")

                        ' australia oceania
                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='th_title' align='center'>TOTAL</th>")
                        htmlOut.Append("<th class='th_title' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title' align='center'>TOTAL</th>")
                        htmlOut.Append("<th class='th_title' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title' align='center'>TOTAL</th>")
                        htmlOut.Append("<th class='th_title' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title' align='center'>TOTAL</th>")
                        htmlOut.Append("<th class='th_title' align='center'>FOR<br />SALE</th>")

                        ' europe
                        htmlOut.Append("<th class='th_title_details' align='center'>TOTAL</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>TOTAL</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>TOTAL</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>TOTAL</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("</tr>")

                        ' column data
                        htmlOut.Append("<tr>")

                        ' australia oceania
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_australia_acbase_total").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_australia_acbase_forsale").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_australia_registered_total").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_australia_registered_forsale").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_australia_operated_total").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_australia_operated_forsale").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_australia_owned_total").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_australia_owned_forsale").ToString + "</td>")

                        ' europe
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_europe_acbase_total").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_europe_acbase_forsale").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_europe_registered_total").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_europe_registered_forsale").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_europe_operated_total").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_europe_operated_forsale").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_europe_owned_total").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_europe_owned_forsale").ToString + "</td>")
                        htmlOut.Append("</tr>")

                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='th_title' align='center' colSpan='8'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colSpan='8'>&nbsp;</th>")
                        htmlOut.Append("</tr>")

                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='th_title' align='center' colSpan='8'>NORTH<br />AMERICA</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colSpan='8'>SOUTH<br />AMERICA</th>")
                        htmlOut.Append("</tr>")

                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='th_title' align='center' colSpan='2'>A/C BASED</th>")
                        htmlOut.Append("<th class='th_title' align='center' colSpan='2'>REGISTERED</th>")
                        htmlOut.Append("<th class='th_title' align='center' colSpan='2'>OPERATED</th>")
                        htmlOut.Append("<th class='th_title' align='center' colSpan='2'>OWNED</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colSpan='2'>A/C BASED</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colSpan='2'>REGISTERED</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colSpan='2'>OPERATED</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colSpan='2'>OWNED</th>")
                        htmlOut.Append("</tr>")

                        ' australia oceania
                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='th_title' align='center'>TOTAL</th>")
                        htmlOut.Append("<th class='th_title' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title' align='center'>TOTAL</th>")
                        htmlOut.Append("<th class='th_title' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title' align='center'>TOTAL</th>")
                        htmlOut.Append("<th class='th_title' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title' align='center'>TOTAL</th>")
                        htmlOut.Append("<th class='th_title' align='center'>FOR<br />SALE</th>")

                        ' europe
                        htmlOut.Append("<th class='th_title_details' align='center'>TOTAL</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>TOTAL</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>TOTAL</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>TOTAL</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("</tr>")

                        ' column data
                        htmlOut.Append("<tr>")

                        ' north america
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_northamerica_acbase_total").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_northamerica_acbase_forsale").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_northamerica_registered_total").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_northamerica_registered_forsale").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_northamerica_operated_total").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_northamerica_operated_forsale").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_northamerica_owned_total").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_northamerica_owned_forsale").ToString + "</td>")

                        ' south america
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_southamerica_acbase_total").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_southamerica_acbase_forsale").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_southamerica_registered_total").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_southamerica_registered_forsale").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_southamerica_operated_total").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_southamerica_operated_forsale").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_southamerica_owned_total").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right'>" + r.Item("ac6a_southamerica_owned_forsale").ToString + "</td>")
                        htmlOut.Append("</tr>")

                        htmlOut.Append("<tr>")
                        htmlOut.Append("<td class='th_title_details' align='center' colSpan='16'>&nbsp;</td>")
                        htmlOut.Append("</tr>")

                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='th_title' align='center' colSpan='16'>UNKNOWN</th>")
                        htmlOut.Append("</tr>")

                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='th_title' align='center' colSpan='4'>A/C BASED</th>")
                        htmlOut.Append("<th class='th_title' align='center' colSpan='4'>REGISTERED</th>")
                        htmlOut.Append("<th class='th_title' align='center' colSpan='4'>OPERATED</th>")
                        htmlOut.Append("<th class='th_title' align='center' colSpan='4'>OWNED</th>")
                        htmlOut.Append("</tr>")

                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='th_title' align='center' colspan='2'>TOTAL</th>")
                        htmlOut.Append("<th class='th_title' align='center' colspan='2'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title' align='center' colspan='2'>TOTAL</th>")
                        htmlOut.Append("<th class='th_title' align='center' colspan='2'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title' align='center' colspan='2'>TOTAL</th>")
                        htmlOut.Append("<th class='th_title' align='center' colspan='2'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title' align='center' colspan='2'>TOTAL</th>")
                        htmlOut.Append("<th class='th_title' align='center' colspan='2'>FOR<br />SALE</th>")
                        htmlOut.Append("</tr>")

                        ' column data
                        htmlOut.Append("<tr>")

                        ' unknown
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right' colspan='2'>" + r.Item("ac6a_unknown_acbase_total").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right' colspan='2'>" + r.Item("ac6a_unknown_acbase_forsale").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right' colspan='2'>" + r.Item("ac6a_unknown_registered_total").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right' colspan='2'>" + r.Item("ac6a_unknown_registered_forsale").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right' colspan='2'>" + r.Item("ac6a_unknown_operated_total").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right' colspan='2'>" + r.Item("ac6a_unknown_operated_forsale").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right' colspan='2'>" + r.Item("ac6a_unknown_owned_total").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right' colspan='2'>" + r.Item("ac6a_unknown_owned_forsale").ToString + "</td>")
                        htmlOut.Append("</tr>")

                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='th_title_details' align='center' colSpan='8'>TOTAL<br />FLEET</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colSpan='8'>TOTAL<br />FOR<br />SALE</th>")
                        htmlOut.Append("</tr>")

                        ' Totals
                        htmlOut.Append("<tr>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right' colspan='8'>" + r.Item("ac6a_total_fleet").ToString + "</td>")
                        htmlOut.Append("<td class='beige2' title='" + ac6a_make + " " + ac6a_model + "' align='right' colspan='8'>" + r.Item("ac6a_total_fleet_forsale").ToString + "</td>")
                        htmlOut.Append("</tr>")

                    Next
                Else
                    htmlOut.Append("<tr><td align='center' valign='middle'><br />No data available for the model you have selected</td></tr>")
                End If
            Else
                htmlOut.Append("<tr><td align='center' valign='middle'><br />No data available for the model you have selected</td></tr>")
            End If

            htmlOut.Append("</table>") 'end inner model star table

        Catch ex As Exception

            aError = "Error in views_display_star_aircraft_6a(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function get_star_aircraft_7_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT ac7_make, ac7_model, ac7_total_fleet, ac7_aircraft_forsale, ac7_percentage_forsale, ac7_0_5_year_range, ac7_0_5_aircraft_in_operation, ac7_0_5_aircraft_forsale, ")
            sQuery.Append("ac7_0_5_percentage_forsale, ac7_6_10_year_range, ac7_6_10_aircraft_in_operation, ac7_6_10_aircraft_forsale, ac7_6_10_percentage_forsale, ")
            sQuery.Append("ac7_11_15_year_range, ac7_11_15_aircraft_in_operation, ac7_11_15_aircraft_forsale, ac7_11_15_percentage_forsale, ac7_16_20_year_range, ")
            sQuery.Append("ac7_16_20_aircraft_in_operation, ac7_16_20_aircraft_forsale, ac7_16_20_percentage_forsale, ac7_21_25_year_range, ac7_21_25_aircraft_in_operation, ")
            sQuery.Append("ac7_21_25_aircraft_forsale, ac7_21_25_percentage_forsale, ac7_26_30_year_range, ac7_26_30_aircraft_in_operation, ac7_26_30_aircraft_forsale, ")
            sQuery.Append("ac7_26_30_percentage_forsale, ac7_30_plus_year_range, ac7_30_plus_aircraft_in_operation, ac7_30_plus_aircraft_forsale, ac7_30_plus_percentage_forsale, ")
            sQuery.Append("ac7_average_year ")
            sQuery.Append("FROM Aircraft_7 WITH(NOLOCK) ")
            sQuery.Append("WHERE (ac7_amod_id = '" + searchCriteria.ViewCriteriaAmodID.ToString + "') AND (ac7_start_date = '" + FormatDateTime(CDate(searchCriteria.ViewCriteriaStarReportDate.ToString), DateFormat.ShortDate).ToString + "')")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_star_aircraft_7_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = starConnectString
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
                aError = "Error in get_star_aircraft_7_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_star_aircraft_7_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Sub views_display_star_aircraft_7(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder

        Try

            results_table = get_star_aircraft_7_info(searchCriteria)
            htmlOut.Append("<table cellpadding=""2"" cellspacing=""0"" border=""1"" width=""800"">") 'start inner model star table

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    Dim ac7_make As String = results_table.Rows(0).Item("ac7_make").ToString.Trim
                    Dim ac7_model As String = results_table.Rows(0).Item("ac7_model").ToString.Trim

                    For Each r As DataRow In results_table.Rows

                        ' table header
                        htmlOut.Append("<tr><th class='th_title' align='center' colspan='10'>Average Age of Fleet for " + ac7_make + " " + ac7_model + "<br /> WHOLLY OWNED AIRCRAFT</th>")
                        htmlOut.Append("<th class='th_title' align='center' colspan='36'>&nbsp;</th></tr>")

                        ' column headers
                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='th_title_details' align='center' colspan='2'>&nbsp;NEW/USED AIRCRAFT&nbsp;</th><th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colspan='3'>TOTAL AIRCRAFT<br />ALL YEARS</th><th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colspan='3'>0 - 5 YEARS<br />" + Year(Now).ToString + " to " + Year(DateAdd("yyyy", (-1 * (4)), Now)).ToString + "</th><th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colspan='3'>6 - 10 YEARS<br />" + Year(DateAdd("yyyy", (-1 * (5)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (9)), Now)).ToString + "</th><th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colspan='3'>11 - 15 YEARS<br />" + Year(DateAdd("yyyy", (-1 * (10)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (14)), Now)).ToString + "</th><th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colspan='3'>16 - 20 YEARS<br />" + Year(DateAdd("yyyy", (-1 * (15)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (19)), Now)).ToString + "</th><th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colspan='3'>21 - 25 YEARS<br />" + Year(DateAdd("yyyy", (-1 * (20)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (24)), Now)).ToString + "</th><th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colspan='3'>26 - 30 YEARS<br />" + Year(DateAdd("yyyy", (-1 * (25)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (29)), Now)).ToString + "</th><th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colspan='3'>31 PLUS YEARS<br />" + Year(DateAdd("yyyy", (-1 * (30)), Now)).ToString + "-BACK</th><th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colspan='2' rowspan='2'>AVERAGE<br />YEAR</th>")
                        htmlOut.Append("</tr>")

                        ' column headers
                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='th_title_details' align='left'>Make</th>")
                        htmlOut.Append("<th class='th_title_details' align='left'>Model</th>")
                        htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>ACTIVE<br />FLEET</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>% FOR<br />SALE</th>")
                        htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>ACTIVE<br />FLEET</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>% FOR<br />SALE</th>")
                        htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>ACTIVE<br />FLEET</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>% FOR<br />SALE</th>")
                        htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>ACTIVE<br />FLEET</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>% FOR<br />SALE</th>")
                        htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>ACTIVE<br />FLEET</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>% FOR<br />SALE</th>")
                        htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>ACTIVE<br />FLEET</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>% FOR<br />SALE</th>")
                        htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>ACTIVE<br />FLEET</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>% FOR<br />SALE</th>")
                        htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>ACTIVE<br />FLEET</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>% FOR<br />SALE</th>")
                        htmlOut.Append("<th class='separateheader'>&nbsp;</th>")

                        htmlOut.Append("</tr>")

                        ' column data
                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='beige' title='Make' nowrap=""nowrap"" align='left'>" + ac7_make + "</th>")
                        htmlOut.Append("<th class='beige2' title='Model' nowrap=""nowrap"" align='left'>" + ac7_model + "</th>")
                        htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<td class='mistyrose' title='" + ac7_make + " " + ac7_model + " - TOTAL AIRCRAFT ALL YEARS Total Active Fleet' align='right'>" + r.Item("ac7_total_fleet").ToString + "</td>")
                        htmlOut.Append("<td class='mistyrose' title='" + ac7_make + " " + ac7_model + " - TOTAL AIRCRAFT ALL YEARS Total For Sale' align='right'>" + r.Item("ac7_aircraft_forsale").ToString + "</td>")
                        htmlOut.Append("<td class='mistyrose' title='" + ac7_make + " " + ac7_model + " - TOTAL AIRCRAFT ALL YEARS Percentage For Sale' align='right'>" + r.Item("ac7_percentage_forsale").ToString + "%</td>")
                        htmlOut.Append("<th class='separateheader'>&nbsp;</th>")

                        If Not IsDBNull(r.Item("ac7_0_5_aircraft_in_operation")) And Not IsDBNull(r.Item("ac7_0_5_aircraft_forsale")) And Not IsDBNull(r.Item("ac7_0_5_percentage_forsale")) Then
                            If CLng(r.Item("ac7_0_5_aircraft_in_operation").ToString) = 0 And CLng(r.Item("ac7_0_5_aircraft_forsale").ToString) = 0 And CLng(r.Item("ac7_0_5_percentage_forsale").ToString) = 0 Then
                                htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            Else
                                htmlOut.Append("<td class='powderblue' title='" + ac7_make + " " + ac7_model + " - 0 - 5 YEARS " + Year(Now).ToString + " to " + Year(DateAdd("yyyy", (-1 * (4)), Now)).ToString + " Total Active Fleet' align='right'>" + r.Item("ac7_0_5_aircraft_in_operation").ToString + "</td>")
                                htmlOut.Append("<td class='powderblue' title='" + ac7_make + " " + ac7_model + " - 0 - 5 YEARS " + Year(Now).ToString + " to " + Year(DateAdd("yyyy", (-1 * (4)), Now)).ToString + " Total For Sale' align='right'>" + r.Item("ac7_0_5_aircraft_forsale").ToString + "</td>")
                                htmlOut.Append("<td class='powderblue' title='" + ac7_make + " " + ac7_model + " - 0 - 5 YEARS " + Year(Now).ToString + " to " + Year(DateAdd("yyyy", (-1 * (4)), Now)).ToString + " Percentage For Sale' align='right'>" + r.Item("ac7_0_5_percentage_forsale").ToString + "%</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            End If
                        Else
                            htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                            htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        End If

                        If Not IsDBNull(r.Item("ac7_6_10_aircraft_in_operation")) And Not IsDBNull(r.Item("ac7_6_10_aircraft_forsale")) And Not IsDBNull(r.Item("ac7_6_10_percentage_forsale")) Then
                            If CLng(r.Item("ac7_6_10_aircraft_in_operation").ToString) = 0 And CLng(r.Item("ac7_6_10_aircraft_forsale").ToString) = 0 And CLng(r.Item("ac7_6_10_percentage_forsale").ToString) = 0 Then
                                htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            Else
                                htmlOut.Append("<td class='mintcream' title='" + ac7_make + " " + ac7_model + " - 6 - 10 YEARS " + Year(DateAdd("yyyy", (-1 * (5)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (9)), Now)).ToString + " Total Active Fleet' align='right'>" + r.Item("ac7_6_10_aircraft_in_operation").ToString + "</td>")
                                htmlOut.Append("<td class='mintcream' title='" + ac7_make + " " + ac7_model + " - 6 - 10 YEARS " + Year(DateAdd("yyyy", (-1 * (5)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (9)), Now)).ToString + " Total For Sale' align='right'>" + r.Item("ac7_6_10_aircraft_forsale").ToString + "</td>")
                                htmlOut.Append("<td class='mintcream' title='" + ac7_make + " " + ac7_model + " - 6 - 10 YEARS " + Year(DateAdd("yyyy", (-1 * (5)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (9)), Now)).ToString + " Percentage For Sale' align='right'>" + r.Item("ac7_6_10_percentage_forsale").ToString + "%</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            End If
                        Else
                            htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                            htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        End If

                        If Not IsDBNull(r.Item("ac7_11_15_aircraft_in_operation")) And Not IsDBNull(r.Item("ac7_11_15_aircraft_forsale")) And Not IsDBNull(r.Item("ac7_11_15_percentage_forsale")) Then
                            If CLng(r.Item("ac7_11_15_aircraft_in_operation").ToString) = 0 And CLng(r.Item("ac7_11_15_aircraft_forsale").ToString) = 0 And CLng(r.Item("ac7_11_15_percentage_forsale").ToString) = 0 Then
                                htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            Else
                                htmlOut.Append("<td class='mintcream' title='" + ac7_make + " " + ac7_model + " - 11 - 15 YEARS " + Year(DateAdd("yyyy", (-1 * (10)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (14)), Now)).ToString + " Total Active Fleet' align='right'>" + r.Item("ac7_11_15_aircraft_in_operation").ToString + "</td>")
                                htmlOut.Append("<td class='mintcream' title='" + ac7_make + " " + ac7_model + " - 11 - 15 YEARS " + Year(DateAdd("yyyy", (-1 * (10)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (14)), Now)).ToString + " Total For Sale' align='right'>" + r.Item("ac7_11_15_aircraft_forsale").ToString + "</td>")
                                htmlOut.Append("<td class='mintcream' title='" + ac7_make + " " + ac7_model + " - 11 - 15 YEARS " + Year(DateAdd("yyyy", (-1 * (10)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (14)), Now)).ToString + " Percentage For Sale' align='right'>" + r.Item("ac7_11_15_percentage_forsale").ToString + "%</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            End If
                        Else
                            htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                            htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        End If

                        If Not IsDBNull(r.Item("ac7_16_20_aircraft_in_operation")) And Not IsDBNull(r.Item("ac7_16_20_aircraft_forsale")) And Not IsDBNull(r.Item("ac7_16_20_percentage_forsale")) Then
                            If CLng(r.Item("ac7_16_20_aircraft_in_operation").ToString) = 0 And CLng(r.Item("ac7_16_20_aircraft_forsale").ToString) = 0 And CLng(r.Item("ac7_16_20_percentage_forsale").ToString) = 0 Then
                                htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            Else
                                htmlOut.Append("<td class='mintcream' title='" + ac7_make + " " + ac7_model + " - 16 - 20 YEARS " + Year(DateAdd("yyyy", (-1 * (15)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (19)), Now)).ToString + " Total Active Fleet' align='right'>" + r.Item("ac7_16_20_aircraft_in_operation").ToString + "</td>")
                                htmlOut.Append("<td class='mintcream' title='" + ac7_make + " " + ac7_model + " - 16 - 20 YEARS " + Year(DateAdd("yyyy", (-1 * (15)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (19)), Now)).ToString + " Total For Sale' align='right'>" + r.Item("ac7_16_20_aircraft_forsale").ToString + "</td>")
                                htmlOut.Append("<td class='mintcream' title='" + ac7_make + " " + ac7_model + " - 16 - 20 YEARS " + Year(DateAdd("yyyy", (-1 * (15)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (19)), Now)).ToString + " Percentage For Sale' align='right'>" + r.Item("ac7_16_20_percentage_forsale").ToString + "%</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            End If
                        Else
                            htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                            htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        End If

                        If Not IsDBNull(r.Item("ac7_21_25_aircraft_in_operation")) And Not IsDBNull(r.Item("ac7_21_25_aircraft_forsale")) And Not IsDBNull(r.Item("ac7_21_25_percentage_forsale")) Then
                            If CLng(r.Item("ac7_21_25_aircraft_in_operation").ToString) = 0 And CLng(r.Item("ac7_21_25_aircraft_forsale").ToString) = 0 And CLng(r.Item("ac7_21_25_percentage_forsale").ToString) = 0 Then
                                htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            Else
                                htmlOut.Append("<td class='mintcream' title='" + ac7_make + " " + ac7_model + " - 21 - 25 YEARS " + Year(DateAdd("yyyy", (-1 * (20)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (24)), Now)).ToString + " Total Active Fleet' align='right'>" + r.Item("ac7_21_25_aircraft_in_operation").ToString + "</td>")
                                htmlOut.Append("<td class='mintcream' title='" + ac7_make + " " + ac7_model + " - 21 - 25 YEARS " + Year(DateAdd("yyyy", (-1 * (20)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (24)), Now)).ToString + " Total For Sale' align='right'>" + r.Item("ac7_21_25_aircraft_forsale").ToString + "</td>")
                                htmlOut.Append("<td class='mintcream' title='" + ac7_make + " " + ac7_model + " - 21 - 25 YEARS " + Year(DateAdd("yyyy", (-1 * (20)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (24)), Now)).ToString + " Percentage For Sale' align='right'>" + r.Item("ac7_21_25_percentage_forsale").ToString + "%</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            End If
                        Else
                            htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                            htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        End If

                        If Not IsDBNull(r.Item("ac7_26_30_aircraft_in_operation")) And Not IsDBNull(r.Item("ac7_26_30_aircraft_forsale")) And Not IsDBNull(r.Item("ac7_26_30_percentage_forsale")) Then
                            If CLng(r.Item("ac7_26_30_aircraft_in_operation").ToString) = 0 And CLng(r.Item("ac7_26_30_aircraft_forsale").ToString) = 0 And CLng(r.Item("ac7_26_30_percentage_forsale").ToString) = 0 Then
                                htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            Else
                                htmlOut.Append("<td class='mintcream' title='" + ac7_make + " " + ac7_model + " - 26 - 30 YEARS " + Year(DateAdd("yyyy", (-1 * (25)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (29)), Now)).ToString + " Total Active Fleet' align='right'>" + r.Item("ac7_26_30_aircraft_in_operation").ToString + "</td>")
                                htmlOut.Append("<td class='mintcream' title='" + ac7_make + " " + ac7_model + " - 26 - 30 YEARS " + Year(DateAdd("yyyy", (-1 * (25)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (29)), Now)).ToString + " Total For Sale' align='right'>" + r.Item("ac7_26_30_aircraft_forsale").ToString + "</td>")
                                htmlOut.Append("<td class='mintcream' title='" + ac7_make + " " + ac7_model + " - 26 - 30 YEARS " + Year(DateAdd("yyyy", (-1 * (25)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (29)), Now)).ToString + " Percentage For Sale' align='right'>" + r.Item("ac7_26_30_percentage_forsale").ToString + "%</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            End If
                        Else
                            htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                            htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        End If

                        If Not IsDBNull(r.Item("ac7_30_plus_aircraft_in_operation")) And Not IsDBNull(r.Item("ac7_30_plus_aircraft_forsale")) And Not IsDBNull(r.Item("ac7_30_plus_percentage_forsale")) Then
                            If CLng(r.Item("ac7_30_plus_aircraft_in_operation").ToString) = 0 And CLng(r.Item("ac7_30_plus_aircraft_forsale").ToString) = 0 And CLng(r.Item("ac7_30_plus_percentage_forsale").ToString) = 0 Then
                                htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            Else
                                htmlOut.Append("<td class='mintcream' title='" + ac7_make + " " + ac7_model + " - 31 PLUS YEARS " + Year(DateAdd("yyyy", (-1 * (30)), Now)).ToString + " - BACK Total Active Fleet' align='right'>" + r.Item("ac7_30_plus_aircraft_in_operation").ToString + "</td>")
                                htmlOut.Append("<td class='mintcream' title='" + ac7_make + " " + ac7_model + " - 31 PLUS YEARS " + Year(DateAdd("yyyy", (-1 * (30)), Now)).ToString + " - BACK Total For Sale' align='right'>" + r.Item("ac7_30_plus_aircraft_forsale").ToString + "</td>")
                                htmlOut.Append("<td class='mintcream' title='" + ac7_make + " " + ac7_model + " - 31 PLUS YEARS " + Year(DateAdd("yyyy", (-1 * (30)), Now)).ToString + " - BACK Percentage For Sale' align='right'>" + r.Item("ac7_30_plus_percentage_forsale").ToString + "%</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            End If
                        Else
                            htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                            htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        End If

                        htmlOut.Append("<td class='beige2' title='" + ac7_make + " " + ac7_model + " - Model Average Year' align='right'>" + r.Item("ac7_average_year").ToString + "</td>")

                        htmlOut.Append("</tr>")

                    Next
                Else
                    htmlOut.Append("<tr><td align='center' valign='middle'><br />No data available for the model you have selected</td></tr>")
                End If
            Else
                htmlOut.Append("<tr><td align='center' valign='middle'><br />No data available for the model you have selected</td></tr>")
            End If

            htmlOut.Append("</table>") 'end inner model star table

        Catch ex As Exception

            aError = "Error in views_display_star_aircraft_7(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function get_star_aircraft_8_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT ac8_make, ac8_model, ac8_total_fleet, ac8_aircraft_forsale, ac8_percentage_forsale, ac8_0_5_aircraft_in_operation, ac8_0_5_aircraft_forsale, ")
            sQuery.Append("ac8_0_5_percentage_forsale, ac8_6_10_aircraft_in_operation, ac8_6_10_aircraft_forsale, ac8_6_10_percentage_forsale, ac8_11_15_aircraft_in_operation, ")
            sQuery.Append("ac8_11_15_aircraft_forsale, ac8_11_15_percentage_forsale, ac8_16_20_aircraft_in_operation, ac8_16_20_aircraft_forsale, ac8_16_20_percentage_forsale, ")
            sQuery.Append("ac8_21_25_aircraft_in_operation, ac8_21_25_aircraft_forsale, ac8_21_25_percentage_forsale, ac8_26_30_aircraft_in_operation, ac8_26_30_aircraft_forsale, ")
            sQuery.Append("ac8_26_30_percentage_forsale, ac8_30_plus_aircraft_in_operation, ac8_30_plus_aircraft_forsale, ac8_30_plus_percentage_forsale, ac8_average_year ")
            sQuery.Append("FROM Aircraft_8 WITH(NOLOCK) ")
            sQuery.Append("WHERE (ac8_amod_id = '" + searchCriteria.ViewCriteriaAmodID.ToString + "') AND (ac8_start_date = '" + FormatDateTime(CDate(searchCriteria.ViewCriteriaStarReportDate.ToString), DateFormat.ShortDate).ToString + "')")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_star_aircraft_8_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = starConnectString
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
                aError = "Error in get_star_aircraft_8_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_star_aircraft_8_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Sub views_display_star_aircraft_8(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder

        Try

            results_table = get_star_aircraft_8_info(searchCriteria)
            htmlOut.Append("<table cellpadding=""2"" cellspacing=""0"" border=""1"" width=""800"">") 'start inner model star table

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    Dim ac8_make As String = results_table.Rows(0).Item("ac8_make").ToString.Trim
                    Dim ac8_model As String = results_table.Rows(0).Item("ac8_model").ToString.Trim

                    For Each r As DataRow In results_table.Rows

                        ' table header
                        htmlOut.Append("<tr><th class='th_title' align='center' colspan='10'>Average Age of Fleet for " + ac8_make + " " + ac8_model + "<br /> WHOLLY OWNED AIRCRAFT</th>")
                        htmlOut.Append("<th class='th_title' align='center' colspan='36'>&nbsp;</th></tr>")

                        ' column headers
                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='th_title_details' align='center' colspan='2'>&nbsp;NEW AIRCRAFT&nbsp;</th><th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colspan='3'>TOTAL AIRCRAFT<br />ALL YEARS</th><th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colspan='3'>0 - 5 YEARS<br />" + Year(Now).ToString + " to " + Year(DateAdd("yyyy", (-1 * (4)), Now)).ToString + "</th><th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colspan='3'>6 - 10 YEARS<br />" + Year(DateAdd("yyyy", (-1 * (5)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (9)), Now)).ToString + "</th><th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colspan='3'>11 - 15 YEARS<br />" + Year(DateAdd("yyyy", (-1 * (10)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (14)), Now)).ToString + "</th><th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colspan='3'>16 - 20 YEARS<br />" + Year(DateAdd("yyyy", (-1 * (15)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (19)), Now)).ToString + "</th><th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colspan='3'>21 - 25 YEARS<br />" + Year(DateAdd("yyyy", (-1 * (20)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (24)), Now)).ToString + "</th><th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colspan='3'>26 - 30 YEARS<br />" + Year(DateAdd("yyyy", (-1 * (25)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (29)), Now)).ToString + "</th><th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colspan='3'>31 PLUS YEARS<br />" + Year(DateAdd("yyyy", (-1 * (30)), Now)).ToString + "-BACK</th><th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colspan='2' rowspan='2'>AVERAGE<br />YEAR</th>")
                        htmlOut.Append("</tr>")

                        ' column headers
                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='th_title_details' align='left'>Make</th>")
                        htmlOut.Append("<th class='th_title_details' align='left'>Model</th>")
                        htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>ACTIVE<br />FLEET</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>% FOR<br />SALE</th>")
                        htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>ACTIVE<br />FLEET</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>% FOR<br />SALE</th>")
                        htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>ACTIVE<br />FLEET</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>% FOR<br />SALE</th>")
                        htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>ACTIVE<br />FLEET</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>% FOR<br />SALE</th>")
                        htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>ACTIVE<br />FLEET</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>% FOR<br />SALE</th>")
                        htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>ACTIVE<br />FLEET</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>% FOR<br />SALE</th>")
                        htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>ACTIVE<br />FLEET</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>% FOR<br />SALE</th>")
                        htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>ACTIVE<br />FLEET</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>% FOR<br />SALE</th>")
                        htmlOut.Append("<th class='separateheader'>&nbsp;</th>")

                        htmlOut.Append("</tr>")

                        ' column data
                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='beige' title='Make' nowrap=""nowrap"" align='left'>" + ac8_make + "</th>")
                        htmlOut.Append("<th class='beige2' title='Model' nowrap=""nowrap"" align='left'>" + ac8_model + "</th>")
                        htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<td class='mistyrose' title='" + ac8_make + " " + ac8_model + " - TOTAL AIRCRAFT ALL YEARS Total Active Fleet' align='right'>" + r.Item("ac8_total_fleet").ToString + "</td>")
                        htmlOut.Append("<td class='mistyrose' title='" + ac8_make + " " + ac8_model + " - TOTAL AIRCRAFT ALL YEARS Total For Sale' align='right'>" + r.Item("ac8_aircraft_forsale").ToString + "</td>")
                        htmlOut.Append("<td class='mistyrose' title='" + ac8_make + " " + ac8_model + " - TOTAL AIRCRAFT ALL YEARS Percentage For Sale' align='right'>" + r.Item("ac8_percentage_forsale").ToString + "%</td>")
                        htmlOut.Append("<th class='separateheader'>&nbsp;</th>")

                        If Not IsDBNull(r.Item("ac8_0_5_aircraft_in_operation")) And Not IsDBNull(r.Item("ac8_0_5_aircraft_forsale")) And Not IsDBNull(r.Item("ac8_0_5_percentage_forsale")) Then
                            If CLng(r.Item("ac8_0_5_aircraft_in_operation").ToString) = 0 And CLng(r.Item("ac8_0_5_aircraft_forsale").ToString) = 0 And CLng(r.Item("ac8_0_5_percentage_forsale").ToString) = 0 Then
                                htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            Else
                                htmlOut.Append("<td class='powderblue' title='" + ac8_make + " " + ac8_model + " - 0 - 5 YEARS " + Year(Now).ToString + " to " + Year(DateAdd("yyyy", (-1 * (4)), Now)).ToString + " Total Active Fleet' align='right'>" + r.Item("ac8_0_5_aircraft_in_operation").ToString + "</td>")
                                htmlOut.Append("<td class='powderblue' title='" + ac8_make + " " + ac8_model + " - 0 - 5 YEARS " + Year(Now).ToString + " to " + Year(DateAdd("yyyy", (-1 * (4)), Now)).ToString + " Total For Sale' align='right'>" + r.Item("ac8_0_5_aircraft_forsale").ToString + "</td>")
                                htmlOut.Append("<td class='powderblue' title='" + ac8_make + " " + ac8_model + " - 0 - 5 YEARS " + Year(Now).ToString + " to " + Year(DateAdd("yyyy", (-1 * (4)), Now)).ToString + " Percentage For Sale' align='right'>" + r.Item("ac8_0_5_percentage_forsale").ToString + "%</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            End If
                        Else
                            htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                            htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        End If

                        If Not IsDBNull(r.Item("ac8_6_10_aircraft_in_operation")) And Not IsDBNull(r.Item("ac8_6_10_aircraft_forsale")) And Not IsDBNull(r.Item("ac8_6_10_percentage_forsale")) Then
                            If CLng(r.Item("ac8_6_10_aircraft_in_operation").ToString) = 0 And CLng(r.Item("ac8_6_10_aircraft_forsale").ToString) = 0 And CLng(r.Item("ac8_6_10_percentage_forsale").ToString) = 0 Then
                                htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            Else
                                htmlOut.Append("<td class='mintcream' title='" + ac8_make + " " + ac8_model + " - 6 - 10 YEARS " + Year(DateAdd("yyyy", (-1 * (5)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (9)), Now)).ToString + " Total Active Fleet' align='right'>" + r.Item("ac8_6_10_aircraft_in_operation").ToString + "</td>")
                                htmlOut.Append("<td class='mintcream' title='" + ac8_make + " " + ac8_model + " - 6 - 10 YEARS " + Year(DateAdd("yyyy", (-1 * (5)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (9)), Now)).ToString + " Total For Sale' align='right'>" + r.Item("ac8_6_10_aircraft_forsale").ToString + "</td>")
                                htmlOut.Append("<td class='mintcream' title='" + ac8_make + " " + ac8_model + " - 6 - 10 YEARS " + Year(DateAdd("yyyy", (-1 * (5)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (9)), Now)).ToString + " Percentage For Sale' align='right'>" + r.Item("ac8_6_10_percentage_forsale").ToString + "%</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            End If
                        Else
                            htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                            htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        End If

                        If Not IsDBNull(r.Item("ac8_11_15_aircraft_in_operation")) And Not IsDBNull(r.Item("ac8_11_15_aircraft_forsale")) And Not IsDBNull(r.Item("ac8_11_15_percentage_forsale")) Then
                            If CLng(r.Item("ac8_11_15_aircraft_in_operation").ToString) = 0 And CLng(r.Item("ac8_11_15_aircraft_forsale").ToString) = 0 And CLng(r.Item("ac8_11_15_percentage_forsale").ToString) = 0 Then
                                htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            Else
                                htmlOut.Append("<td class='mintcream' title='" + ac8_make + " " + ac8_model + " - 11 - 15 YEARS " + Year(DateAdd("yyyy", (-1 * (10)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (14)), Now)).ToString + " Total Active Fleet' align='right'>" + r.Item("ac8_11_15_aircraft_in_operation").ToString + "</td>")
                                htmlOut.Append("<td class='mintcream' title='" + ac8_make + " " + ac8_model + " - 11 - 15 YEARS " + Year(DateAdd("yyyy", (-1 * (10)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (14)), Now)).ToString + " Total For Sale' align='right'>" + r.Item("ac8_11_15_aircraft_forsale").ToString + "</td>")
                                htmlOut.Append("<td class='mintcream' title='" + ac8_make + " " + ac8_model + " - 11 - 15 YEARS " + Year(DateAdd("yyyy", (-1 * (10)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (14)), Now)).ToString + " Percentage For Sale' align='right'>" + r.Item("ac8_11_15_percentage_forsale").ToString + "%</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            End If
                        Else
                            htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                            htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        End If

                        If Not IsDBNull(r.Item("ac8_16_20_aircraft_in_operation")) And Not IsDBNull(r.Item("ac8_16_20_aircraft_forsale")) And Not IsDBNull(r.Item("ac8_16_20_percentage_forsale")) Then
                            If CLng(r.Item("ac8_16_20_aircraft_in_operation").ToString) = 0 And CLng(r.Item("ac8_16_20_aircraft_forsale").ToString) = 0 And CLng(r.Item("ac8_16_20_percentage_forsale").ToString) = 0 Then
                                htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            Else
                                htmlOut.Append("<td class='mintcream' title='" + ac8_make + " " + ac8_model + " - 16 - 20 YEARS " + Year(DateAdd("yyyy", (-1 * (15)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (19)), Now)).ToString + " Total Active Fleet' align='right'>" + r.Item("ac8_16_20_aircraft_in_operation").ToString + "</td>")
                                htmlOut.Append("<td class='mintcream' title='" + ac8_make + " " + ac8_model + " - 16 - 20 YEARS " + Year(DateAdd("yyyy", (-1 * (15)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (19)), Now)).ToString + " Total For Sale' align='right'>" + r.Item("ac8_16_20_aircraft_forsale").ToString + "</td>")
                                htmlOut.Append("<td class='mintcream' title='" + ac8_make + " " + ac8_model + " - 16 - 20 YEARS " + Year(DateAdd("yyyy", (-1 * (15)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (19)), Now)).ToString + " Percentage For Sale' align='right'>" + r.Item("ac8_16_20_percentage_forsale").ToString + "%</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            End If
                        Else
                            htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                            htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        End If

                        If Not IsDBNull(r.Item("ac8_21_25_aircraft_in_operation")) And Not IsDBNull(r.Item("ac8_21_25_aircraft_forsale")) And Not IsDBNull(r.Item("ac8_21_25_percentage_forsale")) Then
                            If CLng(r.Item("ac8_21_25_aircraft_in_operation").ToString) = 0 And CLng(r.Item("ac8_21_25_aircraft_forsale").ToString) = 0 And CLng(r.Item("ac8_21_25_percentage_forsale").ToString) = 0 Then
                                htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            Else
                                htmlOut.Append("<td class='mintcream' title='" + ac8_make + " " + ac8_model + " - 21 - 25 YEARS " + Year(DateAdd("yyyy", (-1 * (20)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (24)), Now)).ToString + " Total Active Fleet' align='right'>" + r.Item("ac8_21_25_aircraft_in_operation").ToString + "</td>")
                                htmlOut.Append("<td class='mintcream' title='" + ac8_make + " " + ac8_model + " - 21 - 25 YEARS " + Year(DateAdd("yyyy", (-1 * (20)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (24)), Now)).ToString + " Total For Sale' align='right'>" + r.Item("ac8_21_25_aircraft_forsale").ToString + "</td>")
                                htmlOut.Append("<td class='mintcream' title='" + ac8_make + " " + ac8_model + " - 21 - 25 YEARS " + Year(DateAdd("yyyy", (-1 * (20)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (24)), Now)).ToString + " Percentage For Sale' align='right'>" + r.Item("ac8_21_25_percentage_forsale").ToString + "%</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            End If
                        Else
                            htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                            htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        End If

                        If Not IsDBNull(r.Item("ac8_26_30_aircraft_in_operation")) And Not IsDBNull(r.Item("ac8_26_30_aircraft_forsale")) And Not IsDBNull(r.Item("ac8_26_30_percentage_forsale")) Then
                            If CLng(r.Item("ac8_26_30_aircraft_in_operation").ToString) = 0 And CLng(r.Item("ac8_26_30_aircraft_forsale").ToString) = 0 And CLng(r.Item("ac8_26_30_percentage_forsale").ToString) = 0 Then
                                htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            Else
                                htmlOut.Append("<td class='mintcream' title='" + ac8_make + " " + ac8_model + " - 26 - 30 YEARS " + Year(DateAdd("yyyy", (-1 * (25)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (29)), Now)).ToString + " Total Active Fleet' align='right'>" + r.Item("ac8_26_30_aircraft_in_operation").ToString + "</td>")
                                htmlOut.Append("<td class='mintcream' title='" + ac8_make + " " + ac8_model + " - 26 - 30 YEARS " + Year(DateAdd("yyyy", (-1 * (25)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (29)), Now)).ToString + " Total For Sale' align='right'>" + r.Item("ac8_26_30_aircraft_forsale").ToString + "</td>")
                                htmlOut.Append("<td class='mintcream' title='" + ac8_make + " " + ac8_model + " - 26 - 30 YEARS " + Year(DateAdd("yyyy", (-1 * (25)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (29)), Now)).ToString + " Percentage For Sale' align='right'>" + r.Item("ac8_26_30_percentage_forsale").ToString + "%</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            End If
                        Else
                            htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                            htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        End If

                        If Not IsDBNull(r.Item("ac8_30_plus_aircraft_in_operation")) And Not IsDBNull(r.Item("ac8_30_plus_aircraft_forsale")) And Not IsDBNull(r.Item("ac8_30_plus_percentage_forsale")) Then
                            If CLng(r.Item("ac8_30_plus_aircraft_in_operation").ToString) = 0 And CLng(r.Item("ac8_30_plus_aircraft_forsale").ToString) = 0 And CLng(r.Item("ac8_30_plus_percentage_forsale").ToString) = 0 Then
                                htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            Else
                                htmlOut.Append("<td class='mintcream' title='" + ac8_make + " " + ac8_model + " - 31 PLUS YEARS " + Year(DateAdd("yyyy", (-1 * (30)), Now)).ToString + " - BACK Total Active Fleet' align='right'>" + r.Item("ac8_30_plus_aircraft_in_operation").ToString + "</td>")
                                htmlOut.Append("<td class='mintcream' title='" + ac8_make + " " + ac8_model + " - 31 PLUS YEARS " + Year(DateAdd("yyyy", (-1 * (30)), Now)).ToString + " - BACK Total For Sale' align='right'>" + r.Item("ac8_30_plus_aircraft_forsale").ToString + "</td>")
                                htmlOut.Append("<td class='mintcream' title='" + ac8_make + " " + ac8_model + " - 31 PLUS YEARS " + Year(DateAdd("yyyy", (-1 * (30)), Now)).ToString + " - BACK Percentage For Sale' align='right'>" + r.Item("ac8_30_plus_percentage_forsale").ToString + "%</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            End If
                        Else
                            htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                            htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        End If

                        htmlOut.Append("<td class='beige2' title='" + ac8_make + " " + ac8_model + " - Model Average Year' align='right'>" + r.Item("ac8_average_year").ToString + "</td>")

                        htmlOut.Append("</tr>")

                    Next
                Else
                    htmlOut.Append("<tr><td align='center' valign='middle'><br />No data available for the model you have selected</td></tr>")
                End If
            Else
                htmlOut.Append("<tr><td align='center' valign='middle'><br />No data available for the model you have selected</td></tr>")
            End If

            htmlOut.Append("</table>") 'end inner model star table

        Catch ex As Exception

            aError = "Error in views_display_star_aircraft_8(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function get_star_aircraft_9_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT ac9_make, ac9_model, ac9_total_fleet, ac9_aircraft_forsale, ac9_percentage_forsale, ac9_0_5_aircraft_in_operation, ac9_0_5_aircraft_forsale, ")
            sQuery.Append("ac9_0_5_percentage_forsale, ac9_6_10_aircraft_in_operation, ac9_6_10_aircraft_forsale, ac9_6_10_percentage_forsale, ac9_11_15_aircraft_in_operation, ")
            sQuery.Append("ac9_11_15_aircraft_forsale, ac9_11_15_percentage_forsale, ac9_16_20_aircraft_in_operation, ac9_16_20_aircraft_forsale, ac9_16_20_percentage_forsale, ")
            sQuery.Append("ac9_21_25_aircraft_in_operation, ac9_21_25_aircraft_forsale, ac9_21_25_percentage_forsale, ac9_26_30_aircraft_in_operation, ac9_26_30_aircraft_forsale, ")
            sQuery.Append("ac9_26_30_percentage_forsale, ac9_30_plus_aircraft_in_operation, ac9_30_plus_aircraft_forsale, ac9_30_plus_percentage_forsale, ac9_average_year ")
            sQuery.Append("FROM Aircraft_9 WITH(NOLOCK) ")
            sQuery.Append("WHERE (ac9_amod_id = '" + searchCriteria.ViewCriteriaAmodID.ToString + "') AND (ac9_start_date = '" + FormatDateTime(CDate(searchCriteria.ViewCriteriaStarReportDate.ToString), DateFormat.ShortDate).ToString + "')")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_star_aircraft_9_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = starConnectString
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
                aError = "Error in get_star_aircraft_9_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_star_aircraft_9_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Sub views_display_star_aircraft_9(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder

        Try

            results_table = get_star_aircraft_9_info(searchCriteria)
            htmlOut.Append("<table cellpadding=""2"" cellspacing=""0"" border=""1"" width=""800"">") 'start inner model star table

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    Dim ac9_make As String = results_table.Rows(0).Item("ac9_make").ToString.Trim
                    Dim ac9_model As String = results_table.Rows(0).Item("ac9_model").ToString.Trim

                    For Each r As DataRow In results_table.Rows

                        ' table header
                        htmlOut.Append("<tr><th class='th_title' align='center' colspan='10'>Average Age of Fleet for " + ac9_make + " " + ac9_model + "<br /> WHOLLY OWNED AIRCRAFT</th>")
                        htmlOut.Append("<th class='th_title' align='center' colspan='36'>&nbsp;</th></tr>")

                        ' column headers
                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='th_title_details' align='center' colspan='2'>&nbsp;USED AIRCRAFT&nbsp;</th><th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colspan='3'>TOTAL AIRCRAFT<br />ALL YEARS</th><th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colspan='3'>0 - 5 YEARS<br />" + Year(Now).ToString + " to " + Year(DateAdd("yyyy", (-1 * (4)), Now)).ToString + "</th><th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colspan='3'>6 - 10 YEARS<br />" + Year(DateAdd("yyyy", (-1 * (5)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (9)), Now)).ToString + "</th><th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colspan='3'>11 - 15 YEARS<br />" + Year(DateAdd("yyyy", (-1 * (10)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (14)), Now)).ToString + "</th><th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colspan='3'>16 - 20 YEARS<br />" + Year(DateAdd("yyyy", (-1 * (15)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (19)), Now)).ToString + "</th><th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colspan='3'>21 - 25 YEARS<br />" + Year(DateAdd("yyyy", (-1 * (20)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (24)), Now)).ToString + "</th><th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colspan='3'>26 - 30 YEARS<br />" + Year(DateAdd("yyyy", (-1 * (25)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (29)), Now)).ToString + "</th><th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colspan='3'>31 PLUS YEARS<br />" + Year(DateAdd("yyyy", (-1 * (30)), Now)).ToString + "-BACK</th><th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colspan='2' rowspan='2'>AVERAGE<br />YEAR</th>")
                        htmlOut.Append("</tr>")

                        ' column headers
                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='th_title_details' align='left'>Make</th>")
                        htmlOut.Append("<th class='th_title_details' align='left'>Model</th>")
                        htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>ACTIVE<br />FLEET</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>% FOR<br />SALE</th>")
                        htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>ACTIVE<br />FLEET</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>% FOR<br />SALE</th>")
                        htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>ACTIVE<br />FLEET</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>% FOR<br />SALE</th>")
                        htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>ACTIVE<br />FLEET</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>% FOR<br />SALE</th>")
                        htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>ACTIVE<br />FLEET</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>% FOR<br />SALE</th>")
                        htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>ACTIVE<br />FLEET</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>% FOR<br />SALE</th>")
                        htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>ACTIVE<br />FLEET</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>% FOR<br />SALE</th>")
                        htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>ACTIVE<br />FLEET</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>FOR<br />SALE</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>% FOR<br />SALE</th>")
                        htmlOut.Append("<th class='separateheader'>&nbsp;</th>")

                        htmlOut.Append("</tr>")

                        ' column data
                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='beige' title='Make' nowrap=""nowrap"" align='left'>" + ac9_make + "</th>")
                        htmlOut.Append("<th class='beige2' title='Model' nowrap=""nowrap"" align='left'>" + ac9_model + "</th>")
                        htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<td class='mistyrose' title='" + ac9_make + " " + ac9_model + " - TOTAL AIRCRAFT ALL YEARS Total Active Fleet' align='right'>" + r.Item("ac9_total_fleet").ToString + "</td>")
                        htmlOut.Append("<td class='mistyrose' title='" + ac9_make + " " + ac9_model + " - TOTAL AIRCRAFT ALL YEARS Total For Sale' align='right'>" + r.Item("ac9_aircraft_forsale").ToString + "</td>")
                        htmlOut.Append("<td class='mistyrose' title='" + ac9_make + " " + ac9_model + " - TOTAL AIRCRAFT ALL YEARS Percentage For Sale' align='right'>" + r.Item("ac9_percentage_forsale").ToString + "%</td>")
                        htmlOut.Append("<th class='separateheader'>&nbsp;</th>")

                        If Not IsDBNull(r.Item("ac9_0_5_aircraft_in_operation")) And Not IsDBNull(r.Item("ac9_0_5_aircraft_forsale")) And Not IsDBNull(r.Item("ac9_0_5_percentage_forsale")) Then
                            If CLng(r.Item("ac9_0_5_aircraft_in_operation").ToString) = 0 And CLng(r.Item("ac9_0_5_aircraft_forsale").ToString) = 0 And CLng(r.Item("ac9_0_5_percentage_forsale").ToString) = 0 Then
                                htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            Else
                                htmlOut.Append("<td class='powderblue' title='" + ac9_make + " " + ac9_model + " - 0 - 5 YEARS " + Year(Now).ToString + " to " + Year(DateAdd("yyyy", (-1 * (4)), Now)).ToString + " Total Active Fleet' align='right'>" + r.Item("ac9_0_5_aircraft_in_operation").ToString + "</td>")
                                htmlOut.Append("<td class='powderblue' title='" + ac9_make + " " + ac9_model + " - 0 - 5 YEARS " + Year(Now).ToString + " to " + Year(DateAdd("yyyy", (-1 * (4)), Now)).ToString + " Total For Sale' align='right'>" + r.Item("ac9_0_5_aircraft_forsale").ToString + "</td>")
                                htmlOut.Append("<td class='powderblue' title='" + ac9_make + " " + ac9_model + " - 0 - 5 YEARS " + Year(Now).ToString + " to " + Year(DateAdd("yyyy", (-1 * (4)), Now)).ToString + " Percentage For Sale' align='right'>" + r.Item("ac9_0_5_percentage_forsale").ToString + "%</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            End If
                        Else
                            htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                            htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        End If

                        If Not IsDBNull(r.Item("ac9_6_10_aircraft_in_operation")) And Not IsDBNull(r.Item("ac9_6_10_aircraft_forsale")) And Not IsDBNull(r.Item("ac9_6_10_percentage_forsale")) Then
                            If CLng(r.Item("ac9_6_10_aircraft_in_operation").ToString) = 0 And CLng(r.Item("ac9_6_10_aircraft_forsale").ToString) = 0 And CLng(r.Item("ac9_6_10_percentage_forsale").ToString) = 0 Then
                                htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            Else
                                htmlOut.Append("<td class='mintcream' title='" + ac9_make + " " + ac9_model + " - 6 - 10 YEARS " + Year(DateAdd("yyyy", (-1 * (5)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (9)), Now)).ToString + " Total Active Fleet' align='right'>" + r.Item("ac9_6_10_aircraft_in_operation").ToString + "</td>")
                                htmlOut.Append("<td class='mintcream' title='" + ac9_make + " " + ac9_model + " - 6 - 10 YEARS " + Year(DateAdd("yyyy", (-1 * (5)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (9)), Now)).ToString + " Total For Sale' align='right'>" + r.Item("ac9_6_10_aircraft_forsale").ToString + "</td>")
                                htmlOut.Append("<td class='mintcream' title='" + ac9_make + " " + ac9_model + " - 6 - 10 YEARS " + Year(DateAdd("yyyy", (-1 * (5)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (9)), Now)).ToString + " Percentage For Sale' align='right'>" + r.Item("ac9_6_10_percentage_forsale").ToString + "%</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            End If
                        Else
                            htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                            htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        End If

                        If Not IsDBNull(r.Item("ac9_11_15_aircraft_in_operation")) And Not IsDBNull(r.Item("ac9_11_15_aircraft_forsale")) And Not IsDBNull(r.Item("ac9_11_15_percentage_forsale")) Then
                            If CLng(r.Item("ac9_11_15_aircraft_in_operation").ToString) = 0 And CLng(r.Item("ac9_11_15_aircraft_forsale").ToString) = 0 And CLng(r.Item("ac9_11_15_percentage_forsale").ToString) = 0 Then
                                htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            Else
                                htmlOut.Append("<td class='mintcream' title='" + ac9_make + " " + ac9_model + " - 11 - 15 YEARS " + Year(DateAdd("yyyy", (-1 * (10)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (14)), Now)).ToString + " Total Active Fleet' align='right'>" + r.Item("ac9_11_15_aircraft_in_operation").ToString + "</td>")
                                htmlOut.Append("<td class='mintcream' title='" + ac9_make + " " + ac9_model + " - 11 - 15 YEARS " + Year(DateAdd("yyyy", (-1 * (10)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (14)), Now)).ToString + " Total For Sale' align='right'>" + r.Item("ac9_11_15_aircraft_forsale").ToString + "</td>")
                                htmlOut.Append("<td class='mintcream' title='" + ac9_make + " " + ac9_model + " - 11 - 15 YEARS " + Year(DateAdd("yyyy", (-1 * (10)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (14)), Now)).ToString + " Percentage For Sale' align='right'>" + r.Item("ac9_11_15_percentage_forsale").ToString + "%</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            End If
                        Else
                            htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                            htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        End If

                        If Not IsDBNull(r.Item("ac9_16_20_aircraft_in_operation")) And Not IsDBNull(r.Item("ac9_16_20_aircraft_forsale")) And Not IsDBNull(r.Item("ac9_16_20_percentage_forsale")) Then
                            If CLng(r.Item("ac9_16_20_aircraft_in_operation").ToString) = 0 And CLng(r.Item("ac9_16_20_aircraft_forsale").ToString) = 0 And CLng(r.Item("ac9_16_20_percentage_forsale").ToString) = 0 Then
                                htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            Else
                                htmlOut.Append("<td class='mintcream' title='" + ac9_make + " " + ac9_model + " - 16 - 20 YEARS " + Year(DateAdd("yyyy", (-1 * (15)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (19)), Now)).ToString + " Total Active Fleet' align='right'>" + r.Item("ac9_16_20_aircraft_in_operation").ToString + "</td>")
                                htmlOut.Append("<td class='mintcream' title='" + ac9_make + " " + ac9_model + " - 16 - 20 YEARS " + Year(DateAdd("yyyy", (-1 * (15)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (19)), Now)).ToString + " Total For Sale' align='right'>" + r.Item("ac9_16_20_aircraft_forsale").ToString + "</td>")
                                htmlOut.Append("<td class='mintcream' title='" + ac9_make + " " + ac9_model + " - 16 - 20 YEARS " + Year(DateAdd("yyyy", (-1 * (15)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (19)), Now)).ToString + " Percentage For Sale' align='right'>" + r.Item("ac9_16_20_percentage_forsale").ToString + "%</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            End If
                        Else
                            htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                            htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        End If

                        If Not IsDBNull(r.Item("ac9_21_25_aircraft_in_operation")) And Not IsDBNull(r.Item("ac9_21_25_aircraft_forsale")) And Not IsDBNull(r.Item("ac9_21_25_percentage_forsale")) Then
                            If CLng(r.Item("ac9_21_25_aircraft_in_operation").ToString) = 0 And CLng(r.Item("ac9_21_25_aircraft_forsale").ToString) = 0 And CLng(r.Item("ac9_21_25_percentage_forsale").ToString) = 0 Then
                                htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            Else
                                htmlOut.Append("<td class='mintcream' title='" + ac9_make + " " + ac9_model + " - 21 - 25 YEARS " + Year(DateAdd("yyyy", (-1 * (20)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (24)), Now)).ToString + " Total Active Fleet' align='right'>" + r.Item("ac9_21_25_aircraft_in_operation").ToString + "</td>")
                                htmlOut.Append("<td class='mintcream' title='" + ac9_make + " " + ac9_model + " - 21 - 25 YEARS " + Year(DateAdd("yyyy", (-1 * (20)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (24)), Now)).ToString + " Total For Sale' align='right'>" + r.Item("ac9_21_25_aircraft_forsale").ToString + "</td>")
                                htmlOut.Append("<td class='mintcream' title='" + ac9_make + " " + ac9_model + " - 21 - 25 YEARS " + Year(DateAdd("yyyy", (-1 * (20)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (24)), Now)).ToString + " Percentage For Sale' align='right'>" + r.Item("ac9_21_25_percentage_forsale").ToString + "%</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            End If
                        Else
                            htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                            htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        End If

                        If Not IsDBNull(r.Item("ac9_26_30_aircraft_in_operation")) And Not IsDBNull(r.Item("ac9_26_30_aircraft_forsale")) And Not IsDBNull(r.Item("ac9_26_30_percentage_forsale")) Then
                            If CLng(r.Item("ac9_26_30_aircraft_in_operation").ToString) = 0 And CLng(r.Item("ac9_26_30_aircraft_forsale").ToString) = 0 And CLng(r.Item("ac9_26_30_percentage_forsale").ToString) = 0 Then
                                htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            Else
                                htmlOut.Append("<td class='mintcream' title='" + ac9_make + " " + ac9_model + " - 26 - 30 YEARS " + Year(DateAdd("yyyy", (-1 * (25)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (29)), Now)).ToString + " Total Active Fleet' align='right'>" + r.Item("ac9_26_30_aircraft_in_operation").ToString + "</td>")
                                htmlOut.Append("<td class='mintcream' title='" + ac9_make + " " + ac9_model + " - 26 - 30 YEARS " + Year(DateAdd("yyyy", (-1 * (25)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (29)), Now)).ToString + " Total For Sale' align='right'>" + r.Item("ac9_26_30_aircraft_forsale").ToString + "</td>")
                                htmlOut.Append("<td class='mintcream' title='" + ac9_make + " " + ac9_model + " - 26 - 30 YEARS " + Year(DateAdd("yyyy", (-1 * (25)), Now)).ToString + " to " + Year(DateAdd("yyyy", (-1 * (29)), Now)).ToString + " Percentage For Sale' align='right'>" + r.Item("ac9_26_30_percentage_forsale").ToString + "%</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            End If
                        Else
                            htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                            htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        End If

                        If Not IsDBNull(r.Item("ac9_30_plus_aircraft_in_operation")) And Not IsDBNull(r.Item("ac9_30_plus_aircraft_forsale")) And Not IsDBNull(r.Item("ac9_30_plus_percentage_forsale")) Then
                            If CLng(r.Item("ac9_30_plus_aircraft_in_operation").ToString) = 0 And CLng(r.Item("ac9_30_plus_aircraft_forsale").ToString) = 0 And CLng(r.Item("ac9_30_plus_percentage_forsale").ToString) = 0 Then
                                htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            Else
                                htmlOut.Append("<td class='mintcream' title='" + ac9_make + " " + ac9_model + " - 31 PLUS YEARS " + Year(DateAdd("yyyy", (-1 * (30)), Now)).ToString + " - BACK Total Active Fleet' align='right'>" + r.Item("ac9_30_plus_aircraft_in_operation").ToString + "</td>")
                                htmlOut.Append("<td class='mintcream' title='" + ac9_make + " " + ac9_model + " - 31 PLUS YEARS " + Year(DateAdd("yyyy", (-1 * (30)), Now)).ToString + " - BACK Total For Sale' align='right'>" + r.Item("ac9_30_plus_aircraft_forsale").ToString + "</td>")
                                htmlOut.Append("<td class='mintcream' title='" + ac9_make + " " + ac9_model + " - 31 PLUS YEARS " + Year(DateAdd("yyyy", (-1 * (30)), Now)).ToString + " - BACK Percentage For Sale' align='right'>" + r.Item("ac9_30_plus_percentage_forsale").ToString + "%</td>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            End If
                        Else
                            htmlOut.Append("<td class='blankline' align='right' colSpan='3'>&nbsp;</td>")
                            htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        End If

                        htmlOut.Append("<td class='beige2' title='" + ac9_make + " " + ac9_model + " - Model Average Year' align='right'>" + r.Item("ac9_average_year").ToString + "</td>")

                        htmlOut.Append("</tr>")

                    Next
                Else
                    htmlOut.Append("<tr><td align='center' valign='middle'><br />No data available for the model you have selected</td></tr>")
                End If
            Else
                htmlOut.Append("<tr><td align='center' valign='middle'><br />No data available for the model you have selected</td></tr>")
            End If

            htmlOut.Append("</table>") 'end inner model star table

        Catch ex As Exception

            aError = "Error in views_display_star_aircraft_9(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function get_star_aircraft_10_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT ac10_make, ac10_model, ac10_business_type_name, ac10_total_fleet, ac10_aircraft_new, ")
            sQuery.Append("ac10_percentage_new, ac10_aircraft_used, ac10_percentage_used, ac10_aircraft_for_sale, ac10_percentange_for_sale ")
            sQuery.Append("FROM Aircraft_10 WITH(NOLOCK) ")
            sQuery.Append("WHERE (ac10_make = '" + searchCriteria.ViewCriteriaAircraftMake.Trim + "') AND (ac10_model = '" + searchCriteria.ViewCriteriaAircraftModel.Trim + "') AND (ac10_start_date = '" + FormatDateTime(CDate(searchCriteria.ViewCriteriaStarReportDate.ToString), DateFormat.ShortDate).ToString + "')")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_star_aircraft_10_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = starConnectString
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
                aError = "Error in get_star_aircraft_10_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_star_aircraft_10_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Sub views_display_star_aircraft_10(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder
        Dim bIsFirstTime As Boolean = False

        Try

            results_table = get_star_aircraft_10_info(searchCriteria)
            htmlOut.Append("<table cellpadding=""2"" cellspacing=""0"" border=""1"" width=""800"">") 'start inner model star table

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    Dim ac10_make As String = results_table.Rows(0).Item("ac10_make").ToString.Trim
                    Dim ac10_model As String = results_table.Rows(0).Item("ac10_model").ToString.Trim

                    ' table header
                    htmlOut.Append("<tr><th class='th_title' align='center' colspan='14'>Aircraft Owner Usage Report<br />" + ac10_make + " " + ac10_model + "</th></tr>")

                    ' column headers
                    htmlOut.Append("<tr>")
                    htmlOut.Append("<th class='th_title_details' align='center'>Make/Model</th><th class='th_title'>&nbsp;</th>")
                    htmlOut.Append("<th class='th_title_details' align='center'>Business Type</th>")
                    htmlOut.Append("<th class='th_title_details' align='center'>Active<br />Fleet</th><th class='th_title'>&nbsp;</th>")
                    htmlOut.Append("<th class='th_title_details' align='center'>New</th>")
                    htmlOut.Append("<th class='th_title_details' align='center'>Percentage</th><th class='th_title'>&nbsp;</th>")
                    htmlOut.Append("<th class='th_title_details' align='center'>Used</th>")
                    htmlOut.Append("<th class='th_title_details' align='center'>Percentage</th><th class='th_title'>&nbsp;</th>")
                    htmlOut.Append("<th class='th_title_details' align='center'>For-Sale</th>")
                    htmlOut.Append("<th class='th_title_details' align='center'>Percentage</th><th class='th_title'>&nbsp;</th>")
                    htmlOut.Append("</tr>")

                    htmlOut.Append("<tr><th class='blankline' colspan='14'>&nbsp;</th></tr>")

                    ' column data

                    For Each r As DataRow In results_table.Rows

                        htmlOut.Append("<tr>")

                        If Not bIsFirstTime Then
                            bIsFirstTime = True
                            htmlOut.Append("<th class='beige' title='Make/Model' nowrap=""nowrap"" rowSpan='" + results_table.Rows.Count.ToString + "' align='center'>" + ac10_make + "<br />" + ac10_model + "</th>")
                        End If

                        htmlOut.Append("<th class='th_title'>&nbsp;</th>")
                        htmlOut.Append("<td class='ivory' title='Business Type' align='right'>" + r.Item("ac10_business_type_name").ToString + "</td>")
                        htmlOut.Append("<td class='lavender' title='End User Owned - Total Active Fleet' align='right'>" + r.Item("ac10_total_fleet").ToString + "</td>")
                        htmlOut.Append("<th class='th_title'>&nbsp;</th>")
                        htmlOut.Append("<td class='mistyrose' title='Total New Aircraft' align='right'>" + r.Item("ac10_aircraft_new").ToString + "</td>")
                        htmlOut.Append("<td class='mistyrose' title='Percentage Of New Aircraft' align='right'>" + r.Item("ac10_percentage_new").ToString + "%</td>")
                        htmlOut.Append("<th class='th_title'>&nbsp;</th>")
                        htmlOut.Append("<td class='lightgreen' title='Total Used Aircraft' align='right'>" + r.Item("ac10_aircraft_used").ToString + "</td>")
                        htmlOut.Append("<td class='lightgreen' title='Percentage Of Used Aircraft' align='right'>" + r.Item("ac10_percentage_used").ToString + "%</td>")
                        htmlOut.Append("<th class='th_title'>&nbsp;</th>")
                        htmlOut.Append("<td class='lightgreen' title='Total For Sale' align='right'>" + r.Item("ac10_aircraft_for_sale").ToString + "</td>")
                        htmlOut.Append("<td class='lightgreen' title='Percentage For Sale' align='right'>" + r.Item("ac10_percentange_for_sale").ToString + "%</td>")
                        htmlOut.Append("<th class='th_title'>&nbsp;</th>")
                        htmlOut.Append("</tr>")

                    Next
                Else
                    htmlOut.Append("<tr><td align='center' valign='middle'><br />No data available for the model you have selected</td></tr>")
                End If
            Else
                htmlOut.Append("<tr><td align='center' valign='middle'><br />No data available for the model you have selected</td></tr>")
            End If

            htmlOut.Append("</table>") 'end inner model star table

        Catch ex As Exception

            aError = "Error in views_display_star_aircraft_10(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function get_star_aircraft_12_info(ByRef searchCriteria As viewSelectionCriteriaClass, Optional ByVal bFromMarketSummary As Boolean = False) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT ac12_started_with_make, ac12_started_with_model, ac12_started_with_total_upgrades, ")
            sQuery.Append("ac12_started_with_total_upgrades_to_model, ac12_started_with_percentage_with_upgrade, ")
            sQuery.Append("ac12_upgrade_to_make, ac12_upgrade_to_model, ac12_upgrade_to_total_upgrades, ")
            sQuery.Append("ac12_upgrade_to_aircraft_for_sale, ac12_upgrade_to_percentage_for_sale ")
            sQuery.Append("FROM Aircraft_12 WITH(NOLOCK) ")

            If bFromMarketSummary Then
                sQuery.Append(" WHERE (ac12_start_date = '" + FormatDateTime(CDate(searchCriteria.ViewCriteriaStarReportDate.ToString), DateFormat.ShortDate).ToString + "')")

                If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAirframeTypeStr.Trim) Then
                    sQuery.Append(" AND (ac12_airframe_type IN (" + searchCriteria.ViewCriteriaAirframeTypeStr.ToString + "))")
                End If

                If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftType.Trim) Then
                    sQuery.Append(" AND (ac12_maketype IN (" + searchCriteria.ViewCriteriaAircraftType.ToString + "))")
                End If

                If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftMake.Trim) Then
                    sQuery.Append(" AND (ac12_upgrade_to_make IN (" + searchCriteria.ViewCriteriaAircraftMake.ToString + "))")
                End If

                If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaAircraftModel.Trim) Then
                    sQuery.Append(" AND (ac12_upgrade_to_amod_id IN (" + searchCriteria.ViewCriteriaAircraftModel.ToString + "))")
                End If

            Else
                sQuery.Append("WHERE (ac12_upgrade_to_amod_id = '" + searchCriteria.ViewCriteriaAmodID.ToString + "') AND (ac12_start_date = '" + FormatDateTime(CDate(searchCriteria.ViewCriteriaStarReportDate.ToString), DateFormat.ShortDate).ToString + "')")
            End If

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_star_aircraft_12_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = starConnectString
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
                aError = "Error in get_star_aircraft_12_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_star_aircraft_12_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Sub views_display_star_aircraft_12(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, Optional ByVal bFromMarketSummary As Boolean = False)

        Dim results_table As New DataTable
        Dim htmlOut As New StringBuilder

        Dim bFirstTime As Boolean = False

        Try

            results_table = get_star_aircraft_12_info(searchCriteria, bFromMarketSummary)
            htmlOut.Append("<table cellpadding=""2"" cellspacing=""0"" border=""1"" width=""800"">") 'start inner model star table

            If Not IsNothing(results_table) Then

                Dim ac12_make As String = ""
                Dim ac12_model As String = ""

                If results_table.Rows.Count > 0 Then

                    For Each r As DataRow In results_table.Rows

                        If (Not ac12_make.Trim.ToLower.Contains(r.Item("ac12_upgrade_to_make").ToString.Trim.ToLower)) Or
                           (Not ac12_model.Trim.ToLower.Contains(r.Item("ac12_upgrade_to_model").ToString.Trim.ToLower)) Then

                            ac12_make = r.Item("ac12_upgrade_to_make").ToString.Trim
                            ac12_model = r.Item("ac12_upgrade_to_model").ToString.Trim

                            ' table header
                            htmlOut.Append("<tr><th class='th_title' align='center' colspan='10'>Upgrade From Path by Model")
                            htmlOut.Append("<br />Based on End User Transactions")
                            htmlOut.Append("<br />" + ac12_make + " " + ac12_model + "</th></tr>")
                            htmlOut.Append("<tr><th class='beige' align='center' colspan='5'>OWNER(S) OF</th>")
                            htmlOut.Append("<th class='beige' align='center' colspan='5'>WILL MOST LIKELY BUY</th></tr>")
                            htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center' colSpan='2'>STARTED WITH</th>")
                            htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center' rowSpan='2'>TOTAL<br />UPGRADES</th>")
                            htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center' rowSpan='2'>TOTAL<br />UPGRADES<br />TO MODEL</th>")
                            htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center' rowSpan='2'>PERCENT<br />WITH<br />UPGRADE<br />PATH</th>")
                            htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center' colSpan='2'>UPGRADED TO</th>")
                            htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center' rowSpan='2'>TOTAL<br />UPGRADES<br />TO</th>")
                            htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center' colSpan='2'>FOR SALE</th></tr>")

                            ' column headers
                            htmlOut.Append("<tr>")
                            htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>MAKE</th>")
                            htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>MODEL</th>")
                            htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>MAKE</th>")
                            htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>MODEL</th>")
                            htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>TOTAL</th>")
                            htmlOut.Append("<th class='th_title_details' nowrap='nowrap' align='center'>PERCENT</th>")
                            htmlOut.Append("</tr>")

                            ' column data
                            htmlOut.Append("<tr>")

                            bFirstTime = False

                        End If

                        If CLng(r.Item("ac12_started_with_total_upgrades").ToString) = 0 Then

                            htmlOut.Append("<td class='lightgrey' title='" + ac12_make + " " + ac12_model + " No Aircraft Upgrades Found' align='center' colSpan='10'>No Aircraft Upgrades Found</td></tr>")

                            If Not bFromMarketSummary Then
                                Exit For
                            End If

                        Else

                            If Not bFirstTime Then
                                bFirstTime = True


                                htmlOut.Append("<td class='lightcyan' title='" + r.Item("ac12_started_with_make").ToString + " " + r.Item("ac12_started_with_model").ToString + " - Started With Make' align='center'>" + r.Item("ac12_started_with_make").ToString + "</td>")
                                htmlOut.Append("<td class='beige2' title='" + r.Item("ac12_started_with_make").ToString + " " + r.Item("ac12_started_with_model").ToString + " - Started With Model' align='center'>" + r.Item("ac12_started_with_model").ToString + "</td>")
                                htmlOut.Append("<td class='beige3' title='" + r.Item("ac12_started_with_make").ToString + " " + r.Item("ac12_started_with_model").ToString + " - Total Aircraft Upgrades Found' align='right'>" + r.Item("ac12_started_with_total_upgrades").ToString + "</td>")
                                htmlOut.Append("<td class='beige3' title='" + r.Item("ac12_started_with_make").ToString + " " + r.Item("ac12_started_with_model").ToString + " - Total Aircraft Upgrades To Model Found' align='right'>" + r.Item("ac12_started_with_total_upgrades_to_model").ToString + "</td>")
                                htmlOut.Append("<td class='beige4' title='" + r.Item("ac12_started_with_make").ToString + " " + r.Item("ac12_started_with_model").ToString + " - Percentange Of Upgrades To Model' align='right'>" + r.Item("ac12_started_with_percentage_with_upgrade").ToString + "%</td>")


                                htmlOut.Append("<th class='lightcyan' title='" + ac12_make + " " + ac12_model + " - Make Upgraded To' align='center'>" + ac12_make + "</th>")
                                htmlOut.Append("<th class='beige2' title='" + ac12_make + " " + ac12_model + " - Model Upgraded To' align='center'>" + ac12_model + "</th>")
                                htmlOut.Append("<td class='beige2' title='" + ac12_make + " " + ac12_model + " - Total Upgrades To Found' align='center'>" + r.Item("ac12_upgrade_to_total_upgrades").ToString + "</td>")
                                htmlOut.Append("<td class='beige2' title='" + ac12_make + " " + ac12_model + " - Total Active Fleet For Sale' align='center'>" + r.Item("ac12_upgrade_to_aircraft_for_sale").ToString + "</td>")
                                htmlOut.Append("<td class='beige2' title='" + ac12_make + " " + ac12_model + " - Percentange Of Active Fleet For Sale' align='center'>" + r.Item("ac12_upgrade_to_percentage_for_sale").ToString + "%</td>")


                            Else


                                htmlOut.Append("<td class='lightcyan' title='" + r.Item("ac12_started_with_make").ToString + " " + r.Item("ac12_started_with_model").ToString + " - Started With Make' align='center'>" + r.Item("ac12_started_with_make").ToString + "</td>")
                                htmlOut.Append("<td class='beige2' title='" + r.Item("ac12_started_with_make").ToString + " " + r.Item("ac12_started_with_model").ToString + " - Started With Model' align='center'>" + r.Item("ac12_started_with_model").ToString + "</td>")
                                htmlOut.Append("<td class='beige3' title='" + r.Item("ac12_started_with_make").ToString + " " + r.Item("ac12_started_with_model").ToString + " - Total Aircraft Upgrades Found' align='right'>" + r.Item("ac12_started_with_total_upgrades").ToString + "</td>")
                                htmlOut.Append("<td class='beige3' title='" + r.Item("ac12_started_with_make").ToString + " " + r.Item("ac12_started_with_model").ToString + " - Total Aircraft Upgrades To Model Found' align='right'>" + r.Item("ac12_started_with_total_upgrades_to_model").ToString + "</td>")
                                htmlOut.Append("<td class='beige4' title='" + r.Item("ac12_started_with_make").ToString + " " + r.Item("ac12_started_with_model").ToString + " - Percentange Of Upgrades To Model' align='right'>" + r.Item("ac12_started_with_percentage_with_upgrade").ToString + "%</td>")
                                htmlOut.Append("<td class='beige4' colSpan='5'>&nbsp;</td>")

                            End If

                            htmlOut.Append("</tr>")

                        End If

                    Next
                Else
                    htmlOut.Append("<tr><td align='center' valign='middle'><br />No data available for the model you have selected</td></tr>")
                End If
            Else
                htmlOut.Append("<tr><td align='center' valign='middle'><br />No data available for the model you have selected</td></tr>")
            End If

            htmlOut.Append("</table>") 'end inner model star table

        Catch ex As Exception

            aError = "Error in views_display_star_aircraft_12(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        results_table = Nothing

    End Sub

    Public Function get_star_aircraft_13_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT ac13_make, ac13_model, ac13_year_mfr, ac13_all_total_fleet, ac13_all_for_sale, ac13_all_percent_for_sale, ac13_all_average_asking_price, ")
            sQuery.Append("ac13_all_high_asking_price, ac13_all_low_asking_price, ac13_new_total_fleet, ac13_new_for_sale, ac13_new_percent_for_sale, ac13_new_average_asking_price, ")
            sQuery.Append("ac13_new_high_asking_price, ac13_new_low_asking_price, ac13_used_total_fleet, ac13_used_for_sale, ac13_used_percent_for_sale, ")
            sQuery.Append("ac13_used_average_asking_price, ac13_used_high_asking_price, ac13_used_low_asking_price ")
            sQuery.Append("FROM Aircraft_13 WITH(NOLOCK) ")
            sQuery.Append("WHERE (ac13_amod_id = '" + searchCriteria.ViewCriteriaAmodID.ToString + "') AND (ac13_start_date = '" + FormatDateTime(CDate(searchCriteria.ViewCriteriaStarReportDate.ToString), DateFormat.ShortDate).ToString + "') ")
            sQuery.Append("ORDER BY ac13_start_date DESC, ac13_year_mfr")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_star_aircraft_13_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = starConnectString
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
                aError = "Error in get_star_aircraft_13_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_star_aircraft_13_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Sub views_display_star_aircraft_13(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, Optional ByVal do_only_all_section As Boolean = False, Optional ByRef graph_string As String = "", Optional ByRef graph_string2 As String = "", Optional ByRef record_count As Long = 0)

        Dim results_table As New DataTable
        Dim strOut As New StringBuilder
        Dim htmlOut As New StringBuilder
        Dim bIsFirstTime As Boolean = False
        Dim sUnknownString As String = ""

        Try

            results_table = get_star_aircraft_13_info(searchCriteria)
            If do_only_all_section = False Then
                htmlOut.Append("<table cellpadding=""2"" cellspacing=""0"" border=""1"" width=""800"">") 'start inner model star table
            End If


            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    Dim ac13_make As String = results_table.Rows(0).Item("ac13_make").ToString.Trim
                    Dim ac13_model As String = results_table.Rows(0).Item("ac13_model").ToString.Trim

                    ' table header 
                    If do_only_all_section = False Then
                        htmlOut.Append("<tr><th class='th_title' align='center' colspan='24'>" + ac13_make + " " + ac13_model)
                        htmlOut.Append("<br />Aircraft For Sale Statistics By Year of Manufacture<br />(All/New/Used) - In Operation</th></tr>")
                    End If


                    If do_only_all_section = False Then
                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='lightcyan' align='center' colSpan='3'&nbsp;</th><th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colSpan='6'>All Aircraft</th><th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='lightcyan' align='center' colSpan='6'>New Aircraft</th><th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center' colSpan='6'>Used Aircraft</th>")
                        htmlOut.Append("</tr>")
                    End If
                    ' column headers

                    If do_only_all_section = True Then
                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='left'>Year<br />Of Mfr</th>")
                        htmlOut.Append("<th class='right'>Total<br />Fleet</th>")
                        htmlOut.Append("<th class='right'>Number<br />For Sale</th>")
                        htmlOut.Append("<th class='right'>Percent<br />For Sale</th>")
                        htmlOut.Append("<th class='right'>Average<br />Asking<br />Price</th>")
                        htmlOut.Append("<th class='right'>High<br />Asking<br />Price</th>")
                        htmlOut.Append("<th class='right'>Low<br />Asking<br />Price</th>")
                        htmlOut.Append("</tr>")
                        htmlOut.Append("</thead>")
                        htmlOut.Append("<tbody>")
                    Else
                        htmlOut.Append("<tr>")
                        htmlOut.Append("<th class='th_title_details' align='center'>Make</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>Make</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>Year<br />Of Mfr</th>")
                        htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>Total<br />Fleet</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>Number<br />For Sale</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>Percent<br />For Sale</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>Average<br />Asking<br />Price</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>High<br />Asking<br />Price</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>Low<br />Asking<br />Price</th>")
                        htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>Total<br />Fleet</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>Number<br />For Sale</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>Percent<br />For Sale</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>Average<br />Asking<br />Price</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>High<br />Asking<br />Price</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>Low<br />Asking<br />Price</th>")
                        htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>Total<br />Fleet</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>Number<br />For Sale</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>Percent<br />For Sale</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>Average<br />Asking<br />Price</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>High<br />Asking<br />Price</th>")
                        htmlOut.Append("<th class='th_title_details' align='center'>Low<br />Asking<br />Price</th>")
                        htmlOut.Append("</tr>")
                    End If


                    ' column data
                    For Each r As DataRow In results_table.Rows

                        record_count = record_count + 1

                        htmlOut.Append("<tr>")

                        If Not bIsFirstTime Then
                            bIsFirstTime = True
                            If do_only_all_section = True Then
                            Else
                                htmlOut.Append("<th class='beige' title='Make' align='center' nowrap=""nowrap"" rowSpan='" + (results_table.Rows.Count + 1).ToString + "'>" + ac13_make + "</th>")
                                htmlOut.Append("<th class='beige2' title='" + ac13_make + " " + ac13_model + r.Item("ac13_year_mfr").ToString + " Model' align='center' nowrap=""nowrap"" rowSpan='" + results_table.Rows.Count.ToString + "'>" + ac13_model + "</th>")
                            End If
                        End If

                        If r.Item("ac13_year_mfr").ToString.ToLower.Contains("all") Then

                            If do_only_all_section = True Then
                                'strOut.Append("<tr>")
                                'strOut.Append("<td class='th_title_details' align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>Total&nbsp;</font></td>")
                                'strOut.Append("<td class='th_title_details' title='" + ac13_make + " - Total Active Fleet - All Aircraft' align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>" + FormatNumber(r.Item("ac13_all_total_fleet").ToString, 0) + "</font></td>")
                                'strOut.Append("<td class='th_title_details' title='" + ac13_make + " - Total Active Fleet For Sale - All Aircraft' align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>" + FormatNumber(r.Item("ac13_all_for_sale").ToString, 0) + "</font></td>")
                                'strOut.Append("<td class='th_title_details' title='" + ac13_make + " - Percent Of Active Flag For Sale - All Aircrdaft' align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>" + FormatNumber(r.Item("ac13_all_percent_for_sale").ToString, 1) + "%</font></td>")
                                'strOut.Append("<td class='th_title_details' title='" + ac13_make + " - Low Asking Price - All Aircrdaft' align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>$" + FormatNumber(CLng(r.Item("ac13_all_low_asking_price") / 1000), 0, TriState.True, TriState.False, TriState.True).ToString + "</font></td>")
                                'strOut.Append("<td class='th_title_details' title='" + ac13_make + " - Average Asking Price - All Aircrdaft' align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>$" + FormatNumber(CLng(r.Item("ac13_all_average_asking_price") / 1000), 0, TriState.True, TriState.False, TriState.True).ToString + "</font></td>")
                                'strOut.Append("<td class='th_title_details' title='" + ac13_make + " - High Asking Price - All Aircrdaft' align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>$" + FormatNumber(CLng(r.Item("ac13_all_high_asking_price") / 1000), 0, TriState.True, TriState.False, TriState.True).ToString + "</font></td>")

                            Else
                                strOut.Append("<tr>")
                                strOut.Append("<th class='th_title_details' title='" + ac13_make + " Model(s)' align='center' nowrap=""nowrap"" colspan='2'>" + ac13_make + "</th>")
                                strOut.Append("<th class='separateheader'>&nbsp;</th>")
                                strOut.Append("<td class='th_title_details' title='" + ac13_make + " - Total Active Fleet - All Aircraft' align='right'>" + r.Item("ac13_all_total_fleet").ToString + "</td>")
                                strOut.Append("<td class='th_title_details' title='" + ac13_make + " - Total Active Fleet For Sale - All Aircraft' align='right'>" + r.Item("ac13_all_for_sale").ToString + "</td>")
                                strOut.Append("<td class='th_title_details' title='" + ac13_make + " - Percent Of Active Flag For Sale - All Aircrdaft' align='right'>" + r.Item("ac13_all_percent_for_sale").ToString + "%</td>")
                                strOut.Append("<td class='th_title_details' title='" + ac13_make + " - Average Asking Price - All Aircrdaft' align='right'>$" + FormatNumber(CLng(r.Item("ac13_all_average_asking_price").ToString), TriState.False, TriState.True, TriState.False, TriState.True).ToString + "</td>")
                                strOut.Append("<td class='th_title_details' title='" + ac13_make + " - High Asking Price - All Aircrdaft' align='right'>$" + FormatNumber(CLng(r.Item("ac13_all_high_asking_price").ToString), TriState.False, TriState.True, TriState.False, TriState.True).ToString + "</td>")
                                strOut.Append("<td class='th_title_details' title='" + ac13_make + " - Low Asking Price - All Aircrdaft' align='right'>$" + FormatNumber(CLng(r.Item("ac13_all_low_asking_price").ToString), TriState.False, TriState.True, TriState.False, TriState.True).ToString + "</td>")
                                strOut.Append("<th class='separateheader'>&nbsp;</th>")
                                strOut.Append("<td class='th_title_details' title='" + ac13_make + " - Total Active Fleet - New Aircraft' align='right'>" + r.Item("ac13_new_total_fleet").ToString + "</td>")
                                strOut.Append("<td class='th_title_details' title='" + ac13_make + " - Total Active Fleet For Sale - New Aircraft' align='right'>" + r.Item("ac13_new_for_sale").ToString + "</td>")
                                strOut.Append("<td class='th_title_details' title='" + ac13_make + " - Percent Of Active Flag For Sale - New Aircrdaft' align='right'>" + r.Item("ac13_new_percent_for_sale").ToString + "%</td>")
                                strOut.Append("<td class='th_title_details' title='" + ac13_make + " - Average Asking Price - New Aircrdaft' align='right'>$" + FormatNumber(CLng(r.Item("ac13_new_average_asking_price").ToString), TriState.False, TriState.True, TriState.False, TriState.True).ToString + "</td>")
                                strOut.Append("<td class='th_title_details' title='" + ac13_make + " - High Asking Price - New Aircrdaft' align='right'>$" + FormatNumber(CLng(r.Item("ac13_new_high_asking_price").ToString), TriState.False, TriState.True, TriState.False, TriState.True).ToString + "</td>")
                                strOut.Append("<td class='th_title_details' title='" + ac13_make + " - Low Asking Price - New Aircrdaft' align='right'>$" + FormatNumber(CLng(r.Item("ac13_new_low_asking_price").ToString), TriState.False, TriState.True, TriState.False, TriState.True).ToString + "</td>")
                                strOut.Append("<th class='separateheader'>&nbsp;</th>")
                                strOut.Append("<td class='th_title_details' title='" + ac13_make + " - Total Active Fleet - Used Aircraft' align='right'>" + r.Item("ac13_used_total_fleet").ToString + "</td>")
                                strOut.Append("<td class='th_title_details' title='" + ac13_make + " - Total Active Fleet For Sale - Used Aircraft' align='right'>" + r.Item("ac13_used_for_sale").ToString + "</td>")
                                strOut.Append("<td class='th_title_details' title='" + ac13_make + " - Percent Of Active Flag For Sale - Used Aircrdaft' align='right'>" + r.Item("ac13_used_percent_for_sale").ToString + "%</td>")
                                strOut.Append("<td class='th_title_details' title='" + ac13_make + " - Average Asking Price - Used Aircrdaft' align='right'>$" + FormatNumber(CLng(r.Item("ac13_used_average_asking_price").ToString), TriState.False, TriState.True, TriState.False, TriState.True).ToString + "</td>")
                                strOut.Append("<td class='th_title_details' title='" + ac13_make + " - High Asking Price - Used Aircrdaft' align='right'>$" + FormatNumber(CLng(r.Item("ac13_used_high_asking_price").ToString), TriState.False, TriState.True, TriState.False, TriState.True).ToString + "</td>")
                                strOut.Append("<td class='th_title_details' title='" + ac13_make + " - Low Asking Price - Used Aircrdaft' align='right'>$" + FormatNumber(CLng(r.Item("ac13_used_low_asking_price").ToString), TriState.False, TriState.True, TriState.False, TriState.True).ToString + "</td>")
                            End If
                            strOut.Append("</tr>")

                        Else


                            If do_only_all_section = True Then
                                htmlOut.Append("<td class='left' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " Year Of Manufacture' align='center' nowrap=""nowrap""><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>" + r.Item("ac13_year_mfr").ToString + "</font></th>")
                            Else
                                htmlOut.Append("<th class='beige2' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " Year Of Manufacture' align='center' nowrap=""nowrap"">" + r.Item("ac13_year_mfr").ToString + "</th>")
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")
                            End If


                            If Not IsDBNull(r.Item("ac13_all_total_fleet")) Then
                                If CLng(r.Item("ac13_all_total_fleet").ToString) = 0 Then
                                    htmlOut.Append("<td class='blankline' align='right' colSpan='6'>&nbsp;</td>")
                                Else


                                    If do_only_all_section = True Then
                                        htmlOut.Append("<td class='right' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " - Total Active Fleet - All Aircraft' align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>" + r.Item("ac13_all_total_fleet").ToString + "</font></td>")
                                        htmlOut.Append("<td class='right' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " - Total Active Fleet For Sale - All Aircraft' align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>" + r.Item("ac13_all_for_sale").ToString + "</font></td>")
                                        htmlOut.Append("<td class='right' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " - Percent Of Active Flag For Sale - All Aircrdaft' align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>" + FormatNumber(r.Item("ac13_all_percent_for_sale").ToString, 1) + "%</font></td>")

                                        If CLng(r.Item("ac13_all_average_asking_price").ToString) = 0 Then
                                            htmlOut.Append("<td class='right' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " - Average Asking Price - All Aircrdaft' align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>N/A</font></td>")
                                        Else
                                            htmlOut.Append("<td class='right' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " - Average Asking Price - All Aircrdaft' align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>$" + FormatNumber(CLng(r.Item("ac13_all_average_asking_price").ToString / 1000), 0, TriState.True, TriState.False, TriState.True).ToString + "</font></td>")
                                        End If

                                        If CLng(r.Item("ac13_all_high_asking_price").ToString) = 0 Then
                                            htmlOut.Append("<td class='right' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " - High Asking Price - All Aircrdaft' align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>N/A</font></td>")
                                        Else
                                            htmlOut.Append("<td class='right' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " - High Asking Price - All Aircrdaft' align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>$" + FormatNumber(CLng(r.Item("ac13_all_high_asking_price").ToString / 1000), 0, TriState.True, TriState.False, TriState.True).ToString + "</font></td>")
                                        End If

                                        If CLng(r.Item("ac13_all_low_asking_price").ToString) = 0 Then
                                            htmlOut.Append("<td class='right' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " - Low Asking Price - All Aircrdaft' align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>N/A</font></td>")
                                        Else
                                            htmlOut.Append("<td class='right' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " - Low Asking Price - All Aircrdaft' align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>$" + FormatNumber(CLng(r.Item("ac13_all_low_asking_price").ToString / 1000), 0, TriState.True, TriState.False, TriState.True).ToString + "</font></td>")
                                        End If
                                    Else
                                        htmlOut.Append("<td class='beige2' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " - Total Active Fleet - All Aircraft' align='right'>" + r.Item("ac13_all_total_fleet").ToString + "</td>")
                                        htmlOut.Append("<td class='beige2' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " - Total Active Fleet For Sale - All Aircraft' align='right'>" + r.Item("ac13_all_for_sale").ToString + "</td>")
                                        htmlOut.Append("<td class='beige2' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " - Percent Of Active Flag For Sale - All Aircrdaft' align='right'>" + r.Item("ac13_all_percent_for_sale").ToString + "%</td>")

                                        If CLng(r.Item("ac13_all_average_asking_price").ToString) = 0 Then
                                            htmlOut.Append("<td class='beige2' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " - Average Asking Price - All Aircrdaft' align='right'>N/A</td>")
                                        Else
                                            htmlOut.Append("<td class='beige2' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " - Average Asking Price - All Aircrdaft' align='right'>$" + FormatNumber(CLng(r.Item("ac13_all_average_asking_price").ToString), TriState.False, TriState.True, TriState.False, TriState.True).ToString + "</td>")
                                        End If

                                        If CLng(r.Item("ac13_all_high_asking_price").ToString) = 0 Then
                                            htmlOut.Append("<td class='beige2' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " - High Asking Price - All Aircrdaft' align='right'>N/A</td>")
                                        Else
                                            htmlOut.Append("<td class='beige2' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " - High Asking Price - All Aircrdaft' align='right'>$" + FormatNumber(CLng(r.Item("ac13_all_high_asking_price").ToString), TriState.False, TriState.True, TriState.False, TriState.True).ToString + "</td>")
                                        End If

                                        If CLng(r.Item("ac13_all_low_asking_price").ToString) = 0 Then
                                            htmlOut.Append("<td class='beige2' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " - Low Asking Price - All Aircrdaft' align='right'>N/A</td>")
                                        Else
                                            htmlOut.Append("<td class='beige2' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " - Low Asking Price - All Aircrdaft' align='right'>$" + FormatNumber(CLng(r.Item("ac13_all_low_asking_price").ToString), TriState.False, TriState.True, TriState.False, TriState.True).ToString + "</td>")
                                        End If
                                    End If




                                    If Trim(graph_string) <> "" Then
                                        graph_string &= ", "
                                    End If
                                    graph_string += "['" & r.Item("ac13_year_mfr").ToString & "'," & r.Item("ac13_all_for_sale").ToString & " ]"

                                    If Trim(graph_string2) <> "" Then
                                        graph_string2 &= ", "
                                    End If
                                    graph_string2 += "['" & r.Item("ac13_year_mfr").ToString & "'," & Replace(FormatNumber(CLng(r.Item("ac13_all_average_asking_price").ToString), TriState.False, TriState.True, TriState.False, TriState.True).ToString, ",", "") & " ]"

                                End If
                            Else
                                htmlOut.Append("<td class='blankline' align='right' colSpan='6'>&nbsp;</td>")
                            End If

                            If do_only_all_section = False Then
                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")

                                If Not IsDBNull(r.Item("ac13_new_total_fleet")) Then
                                    If CLng(r.Item("ac13_new_total_fleet").ToString) = 0 Then
                                        htmlOut.Append("<td class='blankline' align='right' colSpan='6'>&nbsp;</td>")
                                    Else
                                        htmlOut.Append("<td class='beige2' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " - Total Active Fleet - New Aircraft' align='right'>" + r.Item("ac13_new_total_fleet").ToString + "</td>")
                                        htmlOut.Append("<td class='beige2' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " - Total Active Fleet For Sale - New Aircraft' align='right'>" + r.Item("ac13_new_for_sale").ToString + "</td>")
                                        htmlOut.Append("<td class='beige2' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " - Percent Of Active Flag For Sale - New Aircrdaft' align='right'>" + r.Item("ac13_new_percent_for_sale").ToString + "%</td>")

                                        If CLng(r.Item("ac13_new_average_asking_price").ToString) = 0 Then
                                            htmlOut.Append("<td class='beige2' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " - Average Asking Price - New Aircrdaft' align='right'>N/A</td>")
                                        Else
                                            htmlOut.Append("<td class='beige2' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " - Average Asking Price - New Aircrdaft' align='right'>$" + FormatNumber(CLng(r.Item("ac13_new_average_asking_price").ToString), TriState.False, TriState.True, TriState.False, TriState.True).ToString + "</td>")
                                        End If

                                        If CLng(r.Item("ac13_new_high_asking_price").ToString) = 0 Then
                                            htmlOut.Append("<td class='beige2' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " - High Asking Price - New Aircrdaft' align='right'>N/A</td>")
                                        Else
                                            htmlOut.Append("<td class='beige2' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " - High Asking Price - New Aircrdaft' align='right'>$" + FormatNumber(CLng(r.Item("ac13_new_high_asking_price").ToString), TriState.False, TriState.True, TriState.False, TriState.True).ToString + "</td>")
                                        End If

                                        If CLng(r.Item("ac13_new_low_asking_price").ToString) = 0 Then
                                            htmlOut.Append("<td class='beige2' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " - Low Asking Price - New Aircrdaft' align='right'>N/A</td>")
                                        Else
                                            htmlOut.Append("<td class='beige2' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " - Low Asking Price - New Aircrdaft' align='right'>$" + FormatNumber(CLng(r.Item("ac13_new_low_asking_price").ToString), TriState.False, TriState.True, TriState.False, TriState.True).ToString + "</td>")
                                        End If

                                    End If
                                Else
                                    htmlOut.Append("<td class='blankline' align='right' colSpan='6'>&nbsp;</td>")
                                End If


                                htmlOut.Append("<th class='separateheader'>&nbsp;</th>")

                                If Not IsDBNull(r.Item("ac13_used_total_fleet")) Then
                                    If CLng(r.Item("ac13_used_total_fleet").ToString) = 0 Then
                                        htmlOut.Append("<td class='blankline' align='right' colSpan='6'>&nbsp;</td>")
                                    Else
                                        htmlOut.Append("<td class='beige2' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " - Total Active Fleet - Used Aircraft' align='right'>" + r.Item("ac13_used_total_fleet").ToString + "</td>")
                                        htmlOut.Append("<td class='beige2' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " - Total Active Fleet For Sale - Used Aircraft' align='right'>" + r.Item("ac13_used_for_sale").ToString + "</td>")
                                        htmlOut.Append("<td class='beige2' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " - Percent Of Active Flag For Sale - Used Aircrdaft' align='right'>" + r.Item("ac13_used_percent_for_sale").ToString + "%</td>")

                                        If CLng(r.Item("ac13_used_average_asking_price").ToString) = 0 Then
                                            htmlOut.Append("<td class='beige2' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " - Average Asking Price - Used Aircrdaft' align='right'>N/A</td>")
                                        Else
                                            htmlOut.Append("<td class='beige2' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " - Average Asking Price - Used Aircrdaft' align='right'>$" + FormatNumber(CLng(r.Item("ac13_used_average_asking_price").ToString), TriState.False, TriState.True, TriState.False, TriState.True).ToString + "</td>")
                                        End If

                                        If CLng(r.Item("ac13_used_high_asking_price").ToString) = 0 Then
                                            htmlOut.Append("<td class='beige2' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " - High Asking Price - Used Aircrdaft' align='right'>N/A</td>")
                                        Else
                                            htmlOut.Append("<td class='beige2' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " - High Asking Price - Used Aircrdaft' align='right'>$" + FormatNumber(CLng(r.Item("ac13_used_high_asking_price").ToString), TriState.False, TriState.True, TriState.False, TriState.True).ToString + "</td>")
                                        End If

                                        If CLng(r.Item("ac13_used_low_asking_price").ToString) = 0 Then
                                            htmlOut.Append("<td class='beige2' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " - Low Asking Price - Used Aircrdaft' align='right'>N/A</td>")
                                        Else
                                            htmlOut.Append("<td class='beige2' title='" + ac13_make + " " + ac13_model + " " + r.Item("ac13_year_mfr").ToString + " - Low Asking Price - Used Aircrdaft' align='right'>$" + FormatNumber(CLng(r.Item("ac13_used_low_asking_price").ToString), TriState.False, TriState.True, TriState.False, TriState.True).ToString + "</td>")
                                        End If

                                    End If
                                Else
                                    htmlOut.Append("<td class='blankline' align='right' colSpan='6'>&nbsp;</td>")
                                End If

                            End If
                        End If

                        htmlOut.Append("</tr>")

                    Next

                    ' add the ALL 
                    htmlOut.Append(strOut.ToString())

                Else
                    htmlOut.Append("<tr><td align='center' valign='middle'><br />No data available for the model you have selected</td></tr>")
                End If
            Else
                htmlOut.Append("<tr><td align='center' valign='middle'><br />No data available for the model you have selected</td></tr>")
            End If

            graph_string = " data25.addColumn('string', 'Mfr Year'); data25.addColumn('number', 'Total For Sale'); data25.addRows([ " & graph_string

            graph_string2 = " data26.addColumn('string', 'Mfr Year'); data26.addColumn('number', 'Total For Sale'); data26.addRows([ " & graph_string2

            If do_only_all_section = True Then
                htmlOut.Append("<tr><td class='center' colspan='10' bgcolor='#ffffff'><i> <font size='-1'>Note that data in the reports on this page was compiled on " & searchCriteria.ViewCriteriaStarReportDate & " and therefore may not be a direct match to live data summaries</i></font></font></td></tr>")
                htmlOut.Append("</tbody>")
            End If


            htmlOut.Append("</table>") 'end inner model star table

        Catch ex As Exception

            aError = "Error in views_display_star_aircraft_13(ByRef searchCriteria As viewSelsectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        strOut = Nothing
        results_table = Nothing

    End Sub


    Public Function get_live_star_aircraft_13_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try
            sQuery.Append(" select distinct ac_mfr_year as ac13_year_mfr, count(distinct ac_id) as ac13_all_total_fleet, sum(case when ac_forsale_flag='Y' then 1 else 0 end) as ac13_all_for_sale, ")
            sQuery.Append(" (cast(sum(case when ac_forsale_flag='Y' then 1 else 0 end) as float)/cast(count(distinct ac_id) as float)*100) as ac13_all_percent_for_sale,")
            sQuery.Append(" AVG(case when ac_asking_price > 0 and ac_forsale_flag='Y' then ac_asking_price else NULL end) as ac13_all_average_asking_price,")
            sQuery.Append(" max(case when ac_asking_price > 0 and ac_forsale_flag='Y' then ac_asking_price else NULL end) as ac13_all_high_asking_price,")
            sQuery.Append(" min(case when ac_asking_price > 0 and ac_forsale_flag='Y' then ac_asking_price else NULL end) as ac13_all_low_asking_price")
            sQuery.Append(" from Aircraft_Flat with (NOLOCK)")
            sQuery.Append(" WHERE amod_id = " + searchCriteria.ViewCriteriaAmodID.ToString + " AND ac_lifecycle_stage=3 and ac_journ_id = 0 ")
            sQuery.Append(" group by ac_mfr_year order by ac_mfr_year")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_star_aircraft_13_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

            SqlConn.ConnectionString = clientConnectStr 'starConnectString
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
                aError = "Error in get_star_aircraft_13_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_star_aircraft_13_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Sub views_display_live_star_aircraft_13(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String, Optional ByVal do_only_all_section As Boolean = False, Optional ByRef graph_string As String = "", Optional ByRef graph_string2 As String = "", Optional ByRef record_count As Long = 0)

        Dim results_table As New DataTable
        Dim strOut As New StringBuilder
        Dim htmlOut As New StringBuilder
        Dim bIsFirstTime As Boolean = False
        Dim sUnknownString As String = ""

        Try

            results_table = get_live_star_aircraft_13_info(searchCriteria)

            If Not IsNothing(results_table) Then

                If results_table.Rows.Count > 0 Then

                    htmlOut.Append("<tr>")
                    htmlOut.Append("<th Class='left'>Year<br />Of Mfr</th>")
                    htmlOut.Append("<th class='right'>Total<br />Fleet</th>")
                    htmlOut.Append("<th class='right'>Number<br />For Sale</th>")
                    htmlOut.Append("<th class='right'>Percent<br />For Sale</th>")
                    htmlOut.Append("<th class='right'>Average<br />Asking<br />Price</th>")
                    htmlOut.Append("<th class='right'>High<br />Asking<br />Price</th>")
                    htmlOut.Append("<th class='right'>Low<br />Asking<br />Price</th>")
                    htmlOut.Append("</tr>")
                    htmlOut.Append("</thead>")
                    htmlOut.Append("<tbody>")

                    ' column data
                    For Each r As DataRow In results_table.Rows

                        record_count = record_count + 1

                        htmlOut.Append("<tr>")

                        If r.Item("ac13_year_mfr").ToString.ToLower.Contains("all") Then

                        Else

                            htmlOut.Append("<td class='left' title='" + r.Item("ac13_year_mfr").ToString + " Year Of Manufacture' align='center' nowrap=""nowrap""><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>" + r.Item("ac13_year_mfr").ToString + "</font></th>")

                            If Not IsDBNull(r.Item("ac13_all_total_fleet")) Then
                                If CLng(r.Item("ac13_all_total_fleet").ToString) = 0 Then
                                    htmlOut.Append("<td class='blankline' align='right' colSpan='6'>&nbsp;</td>")
                                Else

                                    htmlOut.Append("<td class='right' title='" + r.Item("ac13_year_mfr").ToString + " - Total Active Fleet - All Aircraft' align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>" + r.Item("ac13_all_total_fleet").ToString + "</font></td>")
                                    htmlOut.Append("<td class='right' title='" + r.Item("ac13_year_mfr").ToString + " - Total Active Fleet For Sale - All Aircraft' align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>" + r.Item("ac13_all_for_sale").ToString + "</font></td>")
                                    htmlOut.Append("<td class='right' title='" + r.Item("ac13_year_mfr").ToString + " - Percent Of Active Flag For Sale - All Aircrdaft' align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>" + FormatNumber(r.Item("ac13_all_percent_for_sale").ToString, 1) + "%</font></td>")

                                    If Not IsDBNull(r.Item("ac13_all_average_asking_price")) Then

                                        If CLng(r.Item("ac13_all_average_asking_price").ToString) = 0 Then
                                            htmlOut.Append("<td class='right' title='" + r.Item("ac13_year_mfr").ToString + " - Average Asking Price - All Aircrdaft' align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>N/A</font></td>")
                                        Else
                                            htmlOut.Append("<td class='right' title='" + r.Item("ac13_year_mfr").ToString + " - Average Asking Price - All Aircrdaft' align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>$" + FormatNumber(CLng(r.Item("ac13_all_average_asking_price").ToString / 1000), 0, TriState.True, TriState.False, TriState.True).ToString + "</font></td>")
                                        End If
                                    Else
                                        htmlOut.Append("<td class='right' title='" + r.Item("ac13_year_mfr").ToString + " - Average Asking Price - All Aircrdaft' align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>N/A</font></td>")
                                    End If

                                    If Not IsDBNull(r.Item("ac13_all_high_asking_price")) Then
                                        If CLng(r.Item("ac13_all_high_asking_price").ToString) = 0 Then
                                            htmlOut.Append("<td class='right' title='" + r.Item("ac13_year_mfr").ToString + " - High Asking Price - All Aircrdaft' align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>N/A</font></td>")
                                        Else
                                            htmlOut.Append("<td class='right' title='" + r.Item("ac13_year_mfr").ToString + " - High Asking Price - All Aircrdaft' align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>$" + FormatNumber(CLng(r.Item("ac13_all_high_asking_price").ToString / 1000), 0, TriState.True, TriState.False, TriState.True).ToString + "</font></td>")
                                        End If
                                    Else
                                        htmlOut.Append("<td class='right' title='" + r.Item("ac13_year_mfr").ToString + " - High Asking Price - All Aircrdaft' align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>N/A</font></td>")
                                    End If

                                    If Not IsDBNull(r.Item("ac13_all_low_asking_price")) Then
                                        If CLng(r.Item("ac13_all_low_asking_price").ToString) = 0 Then
                                            htmlOut.Append("<td class='right' title='" + r.Item("ac13_year_mfr").ToString + " - Low Asking Price - All Aircrdaft' align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>N/A</font></td>")
                                        Else
                                            htmlOut.Append("<td class='right' title='" + r.Item("ac13_year_mfr").ToString + " - Low Asking Price - All Aircrdaft' align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>$" + FormatNumber(CLng(r.Item("ac13_all_low_asking_price").ToString / 1000), 0, TriState.True, TriState.False, TriState.True).ToString + "</font></td>")
                                        End If
                                    Else
                                        htmlOut.Append("<td class='right' title='" + r.Item("ac13_year_mfr").ToString + " - Low Asking Price - All Aircrdaft' align='right'><font class='" & HttpContext.Current.Session("FONT_CLASS_TEXT") & "'>N/A</font></td>")
                                    End If

                                    If Trim(graph_string) <> "" Then
                                        graph_string &= ", "
                                    End If
                                    graph_string += "['" & r.Item("ac13_year_mfr").ToString & "'," & r.Item("ac13_all_for_sale").ToString & " ]"


                                    If Not IsDBNull(r.Item("ac13_all_average_asking_price")) Then

                                        If Trim(graph_string2) <> "" Then
                                            graph_string2 &= ", "
                                        End If
                                        graph_string2 += "['" & r.Item("ac13_year_mfr").ToString & "'," & Replace(FormatNumber(CLng(r.Item("ac13_all_average_asking_price").ToString), TriState.False, TriState.True, TriState.False, TriState.True).ToString, ",", "") & " ]"
                                    End If

                                End If
                            Else
                                htmlOut.Append("<td class='blankline' align='right' colSpan='6'>&nbsp;</td>")
                            End If

                        End If

                        htmlOut.Append("</tr>")

                    Next

                    ' add the ALL 
                    htmlOut.Append(strOut.ToString())

                Else
                    htmlOut.Append("<tr><td align='center' valign='middle'><br />No data available for the model you have selected</td></tr>")
                End If
            Else
                htmlOut.Append("<tr><td align='center' valign='middle'><br />No data available for the model you have selected</td></tr>")
            End If

            graph_string = " data25.addColumn('string', 'Mfr Year'); data25.addColumn('number', 'Total For Sale'); data25.addRows([ " & graph_string

            graph_string2 = " data26.addColumn('string', 'Mfr Year'); data26.addColumn('number', 'Total For Sale'); data26.addRows([ " & graph_string2



            htmlOut.Append("</table>") 'end inner model star table

        Catch ex As Exception

            aError = "Error in views_display_star_aircraft_13(ByRef searchCriteria As viewSelsectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing
        strOut = Nothing
        results_table = Nothing

    End Sub

#End Region

#Region "star_view_functions"

    Public Sub views_display_star_star_report_selections(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

        Dim htmlOut As New StringBuilder

        Try

            If searchCriteria.ViewCriteriaStarReportID = -1 And String.IsNullOrEmpty(searchCriteria.ViewCriteriaStarReportType.Trim) And String.IsNullOrEmpty(searchCriteria.ViewCriteriaStarReportDate.Trim) Then
                htmlOut.Append("&nbsp;&nbsp;&nbsp;<font color='red'>Please select Report Name, Report Date, and Airframe Type</font>")
            ElseIf searchCriteria.ViewCriteriaStarReportID > -1 And String.IsNullOrEmpty(searchCriteria.ViewCriteriaStarReportType.Trim) And String.IsNullOrEmpty(searchCriteria.ViewCriteriaStarReportDate.Trim) Then
                htmlOut.Append("&nbsp;&nbsp;&nbsp;<font color='red'>Please select Report Date, and Airframe Type</font>")
            ElseIf searchCriteria.ViewCriteriaStarReportID > -1 And Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaStarReportType.Trim) And String.IsNullOrEmpty(searchCriteria.ViewCriteriaStarReportDate.Trim) Then
                htmlOut.Append("&nbsp;&nbsp;&nbsp;<font color='red'>Please select a Report Date</font>")
            ElseIf searchCriteria.ViewCriteriaStarReportID > -1 And String.IsNullOrEmpty(searchCriteria.ViewCriteriaStarReportType.Trim) And Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaStarReportDate.Trim) Then
                htmlOut.Append("&nbsp;&nbsp;&nbsp;<font color='red'>Please select a Airframe Type</font>")
            ElseIf searchCriteria.ViewCriteriaStarReportID = -1 And Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaStarReportType.Trim) And String.IsNullOrEmpty(searchCriteria.ViewCriteriaStarReportDate.Trim) Then
                htmlOut.Append("&nbsp;&nbsp;&nbsp;<font color='red'>Please select a Report Name, and a Report Date</font>")
            ElseIf searchCriteria.ViewCriteriaStarReportID = -1 And Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaStarReportType.Trim) And Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaStarReportDate.Trim) Then
                htmlOut.Append("&nbsp;&nbsp;&nbsp;<font color='red'>Please select a Report Name</font>")
            End If

            If ((searchCriteria.ViewCriteriaStarReportID > -1) Or (Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaStarReportType.Trim)) Or (Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaStarReportDate.Trim))) Then
                htmlOut.Append("&nbsp;&nbsp;<a href='view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&reportYear=&reportID=&reportType=&reportDate=&reportPrefix=&reportSuffix=&reportCatagory='><strong>Clear All</strong></a>")
            End If


        Catch ex As Exception

            aError = "Error in starReportSelections(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing

    End Sub

    Public Sub views_display_star_report_sample_frame(ByVal inSampleLink As String, ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

        Dim htmlOut As New StringBuilder
        Dim star_data_text As String = ""
        Try

            If searchCriteria.ViewCriteriaStarReportID > 0 And Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaStarReportDate) And Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaStarReportType) And Not String.IsNullOrEmpty(inSampleLink) Then
                htmlOut.Append("<tr><td align='center' valign='middle' colspan='2'><font size='+1'>** The report displayed below is a sample **</font></td></tr>")
                htmlOut.Append("<tr><td align='center' valign='middle' colspan='2'><iframe src=""" + inSampleLink.Trim + """ scrolling='auto' width='100%' height='500'/></iframe></td></tr>")

                star_data_text = "User Entered View " & Replace(commonEvo.Get_Default_User_View(6), "&nbsp;", " ") & ": Report"

                If searchCriteria.ViewCriteriaStarReportID > 0 Then
                    star_data_text &= " ID: " & searchCriteria.ViewCriteriaStarReportID
                End If

                If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaStarReportDate) Then
                    star_data_text &= " Date: " & searchCriteria.ViewCriteriaStarReportDate
                End If

                If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaStarReportType) Then
                    star_data_text &= " Type: " & searchCriteria.ViewCriteriaStarReportType
                End If


                Call commonLogFunctions.Log_User_Event_Data("UserDisplayView", star_data_text, Nothing, 6, 0, 0, 0, 0, 0, 0)
            ElseIf searchCriteria.ViewCriteriaStarReportID > 0 And Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaStarReportDate) And Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaStarReportType) And String.IsNullOrEmpty(inSampleLink) Then
                htmlOut.Append("<tr><td align='center' valign='middle' colspan='2'><strong>Sample Not Available</strong></td><tr>")
            End If

        Catch ex As Exception

            aError = "Error in starReportSelections(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing

    End Sub

    Public Function get_star_reports_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim sqlWhere As String = ""
        Try

            sQuery.Append("SELECT DISTINCT starrep_category, starrep_id, starrep_title, starrep_description, starrep_file_prefix, starrep_sample_address")
            sQuery.Append(" FROM Star_Report WITH(NOLOCK) INNER JOIN Star_Report_Index WITH(NOLOCK) ON starind_starrep_id = starrep_id")

            If HttpContext.Current.Session.Item("localPreferences").UserBusinessFlag = False And HttpContext.Current.Session.Item("localPreferences").UserCommercialFlag = True And HttpContext.Current.Session.Item("localPreferences").UserHelicopterFlag = True Then
                sQuery.Append("   INNER Join Star_Report_Types WITH(NOLOCK) ON starind_startype_id = startype_id  ")
            End If


            If searchCriteria.ViewCriteriaStarReportID > 0 And Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaStarReportType) Then
                sqlWhere = " WHERE starrep_id = " + searchCriteria.ViewCriteriaStarReportID.ToString + " AND starind_startype_id = " + searchCriteria.ViewCriteriaStarReportType.Trim
                If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaStarReportCatagory) And Not searchCriteria.ViewCriteriaStarReportCatagory.ToLower.Trim.Contains("name") Then
                    sqlWhere += " AND starrep_category = '" + searchCriteria.ViewCriteriaStarReportCatagory.ToLower.Trim + "'"
                End If
            ElseIf searchCriteria.ViewCriteriaStarReportID = -1 And Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaStarReportType) Then
                sqlWhere += " WHERE starind_startype_id = " + searchCriteria.ViewCriteriaStarReportType
                If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaStarReportCatagory) And Not searchCriteria.ViewCriteriaStarReportCatagory.ToLower.Trim.Contains("name") Then
                    sqlWhere += " AND starrep_category = '" + searchCriteria.ViewCriteriaStarReportCatagory.ToLower.Trim + "'"
                End If
            ElseIf searchCriteria.ViewCriteriaStarReportID > 0 And String.IsNullOrEmpty(searchCriteria.ViewCriteriaStarReportType) Then
                sqlWhere += " WHERE starrep_id = " + searchCriteria.ViewCriteriaStarReportID.ToString
                If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaStarReportCatagory) And Not searchCriteria.ViewCriteriaStarReportCatagory.ToLower.Trim.Contains("name") Then
                    sqlWhere += " AND starrep_category = '" + searchCriteria.ViewCriteriaStarReportCatagory.ToLower.Trim + "'"
                End If
            Else
                If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaStarReportCatagory) And Not searchCriteria.ViewCriteriaStarReportCatagory.ToLower.Trim.Contains("name") Then
                    sqlWhere += " AND starrep_category = '" + searchCriteria.ViewCriteriaStarReportCatagory.ToLower.Trim + "'"
                End If

                ' if we are all, but only commercial and heli, just get heli
                If HttpContext.Current.Session.Item("localPreferences").UserBusinessFlag = False And HttpContext.Current.Session.Item("localPreferences").UserCommercialFlag = True And HttpContext.Current.Session.Item("localPreferences").UserHelicopterFlag = True Then
                    sqlWhere += " where lower(startype_title) LIKE '%heli%' "
                End If

            End If
            'Adding where clause on 
            sQuery.Append(sqlWhere)
            ''Appending Aerodex flag if needed
            If Not IsNothing(HttpContext.Current.Session.Item("localPreferences").AerodexFlag) Then
                If (HttpContext.Current.Session.Item("localPreferences").AerodexFlag) Then
                    sQuery.Append(IIf(Not String.IsNullOrEmpty(sqlWhere.Trim), Constants.cAndClause, Constants.cWhereClause) + " starrep_aerodex_flag='Y' ")
                End If
            End If

            sQuery.Append(" GROUP BY starrep_id, starrep_title, starrep_description, starrep_file_prefix, starrep_sample_address, starrep_category")

            If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaStarReportCatagory) And Not searchCriteria.ViewCriteriaStarReportCatagory.ToLower.Trim.Contains("name") Then
                sQuery.Append(" ORDER BY starrep_category, starrep_title")
            Else
                sQuery.Append(" ORDER BY starrep_title")
            End If

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_star_reports_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
                aError = "Error in get_star_reports_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception

            aError = "Error in get_star_reports_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message
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

    Public Function get_star_report_title_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try

            sQuery.Append("SELECT distinct startype_title, startype_order, startype_id, startype_file_suffix FROM Star_Report WITH(NOLOCK)")
            sQuery.Append(" INNER JOIN Star_Report_Index WITH(NOLOCK) ON starrep_id = starind_starrep_id")
            sQuery.Append(" INNER JOIN Star_Report_Types WITH(NOLOCK) ON starind_startype_id = startype_id")
            sQuery.Append(" WHERE starrep_id = " + searchCriteria.ViewCriteriaStarReportID.ToString)

            If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaStarReportType) Then
                sQuery.Append(" AND startype_id = " + searchCriteria.ViewCriteriaStarReportType)
            End If

            sQuery.Append(" ORDER BY startype_order")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_star_report_title_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
                aError = "Error in get_star_report_title_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception
            Return Nothing

            aError = "Error in get_star_report_title_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message

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

    Public Function get_star_report_airframetypes_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Dim sTmpClause As String = ""

        Try

            Dim sErrorString As String = ""

            If Not HttpContext.Current.Session.Item("localPreferences").loadUserSession(sErrorString, CLng(HttpContext.Current.Session.Item("localUser").crmSubSubID.ToString), HttpContext.Current.Session.Item("localUser").crmUserLogin.ToString, CLng(HttpContext.Current.Session.Item("localUser").crmSubSeqNo.ToString), CLng(HttpContext.Current.Session.Item("localUser").crmUserContactID.ToString)) Then
                Return Nothing
            End If

            sQuery.Append("SELECT distinct startype_title, startype_order, startype_id, startype_file_suffix FROM Star_Report WITH(NOLOCK)")
            sQuery.Append(" INNER JOIN Star_Report_Index WITH(NOLOCK) ON starrep_id = starind_starrep_id")
            sQuery.Append(" INNER JOIN Star_Report_Types WITH(NOLOCK) ON starind_startype_id = startype_id")
            sQuery.Append(" WHERE startype_title <> '' ")   ' just so we dont need to worry about the and and where - msw / 4/30/20

            If String.IsNullOrEmpty(searchCriteria.ViewCriteriaStarReportType.Trim) Then

                If HttpContext.Current.Session.Item("localPreferences").UserBusinessFlag Or HttpContext.Current.Session.Item("localPreferences").UserCommercialFlag Then
                    Select Case HttpContext.Current.Session.Item("localPreferences").Tierlevel
                        Case eTierLevelTypes.JETS

                            If HttpContext.Current.Session.Item("localPreferences").UserHelicopterFlag = True Then
                                sTmpClause += " and (   (lower(startype_title) LIKE 'jets%' or lower(startype_title) LIKE 'exec%')  or lower(startype_title) LIKE '%heli%')   "
                            Else
                                sTmpClause += " and lower(startype_title) LIKE 'jets%' or lower(startype_title) LIKE 'exec%'"
                            End If


                        Case eTierLevelTypes.TURBOS

                            If HttpContext.Current.Session.Item("localPreferences").UserHelicopterFlag = True Then
                                sTmpClause += " and (     (lower(startype_title) LIKE 'turbos%' or lower(startype_title) LIKE 'pistons') or lower(startype_title) LIKE '%heli%') "
                            Else
                                sTmpClause += " and lower(startype_title) LIKE 'turbos%' or lower(startype_title) LIKE 'pistons'"
                            End If



                        Case eTierLevelTypes.ALL

                            ' if we are all, but only commercial and heli, just get heli
                            If HttpContext.Current.Session.Item("localPreferences").UserBusinessFlag = False And HttpContext.Current.Session.Item("localPreferences").UserCommercialFlag = True And HttpContext.Current.Session.Item("localPreferences").UserHelicopterFlag = True Then
                                sTmpClause += " and lower(startype_title) LIKE '%heli%'"
                            End If

                    End Select
                ElseIf HttpContext.Current.Session.Item("localPreferences").isHeliOnlyProduct Then
                    sTmpClause += " and lower(startype_title) LIKE '%heli%'"
                End If

                sQuery.Append(IIf(Not String.IsNullOrEmpty(sTmpClause.Trim), sTmpClause, ""))

            End If

            If searchCriteria.ViewCriteriaStarReportID > 0 Then
                sQuery.Append(" and starrep_id = " + searchCriteria.ViewCriteriaStarReportID.ToString)
            End If

            If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaStarReportType.Trim) Then
                sQuery.Append(Constants.cAndClause + " startype_id = " + searchCriteria.ViewCriteriaStarReportType.Trim)
            End If

            If Not IsNothing(HttpContext.Current.Session.Item("localPreferences").AerodexFlag) Then
                If (HttpContext.Current.Session.Item("localPreferences").AerodexFlag) Then
                    sQuery.Append(" and starrep_aerodex_flag='Y' ")
                End If
            End If


            sQuery.Append(" ORDER BY startype_order")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_star_report_airframetypes_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
                aError = "Error in get_star_report_airframetypes_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception

            aError = "Error in get_star_report_airframetypes_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message
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

    Public Sub views_display_star_report_airframe_types(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

        Dim htmlOut As New StringBuilder
        Dim results_table As New DataTable
        Dim toggleRowColor As Boolean = False

        Try

            results_table = get_star_report_airframetypes_info(searchCriteria)

            If Not IsNothing(results_table) Then
                If results_table.Rows.Count > 0 Then
                    For Each r As DataRow In results_table.Rows

                        If Not toggleRowColor Then
                            htmlOut.Append("<tr class='alt_row'>")
                            toggleRowColor = True
                        Else
                            htmlOut.Append("<tr bgcolor='white'>")
                            toggleRowColor = False
                        End If

                        htmlOut.Append("<td align='left' valign='middle' class='papers'>")

                        If String.IsNullOrEmpty(searchCriteria.ViewCriteriaStarReportType.Trim) Then
                            htmlOut.Append("<a href=""view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&reportYear=" + searchCriteria.ViewCriteriaStarReportYear + "&reportType=" + r.Item("startype_id").ToString + "&reportID=" + searchCriteria.ViewCriteriaStarReportID.ToString + "&reportCatagory=" + searchCriteria.ViewCriteriaStarReportCatagory + "&reportDate=" + searchCriteria.ViewCriteriaStarReportDate.ToString + "&reportSuffix=" + r.Item("startype_file_suffix").ToString + "&reportPrefix=" + searchCriteria.ViewCriteriaStarReportPrefix + """>" + r.Item("startype_title").ToString + "</a></td></tr>" + vbCrLf)
                        Else
                            htmlOut.Append("<img src='images/papers.jpg' class='bullet' alt='' /><strong><em>" + r.Item("startype_title").ToString + "</em></strong></td></tr>" + vbCrLf)
                        End If

                    Next
                Else
                    htmlOut.Append("<tr><td align='left' valign='middle'>No Report Types</td></tr>")
                End If
            Else
                htmlOut.Append("<tr><td align='left' valign='middle'>No Report Types</td></tr>")
            End If

        Catch ex As Exception

            aError = "Error in views_display_star_report_airframe_types(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing

    End Sub

    Public Function get_star_report_dates_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable

        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try
            sQuery.Append("SELECT stardate_id, stardate_date_directory FROM Star_Report_Dates WITH(NOLOCK)")

            If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaStarReportDate) Then
                sQuery.Append(" WHERE stardate_date_directory = " + searchCriteria.ViewCriteriaStarReportDate.Trim)
            End If

            sQuery.Append(" ORDER BY stardate_date_directory DESC")

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "<br /><br />get_star_report_dates_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable</b><br />" + sQuery.ToString

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
                aError = "Error in get_star_report_dates_info load datatable " + constrExc.Message
            End Try

        Catch ex As Exception

            aError = "Error in get_star_report_dates_info(ByRef searchCriteria As viewSelectionCriteriaClass) As DataTable " + ex.Message
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

    Public Sub views_display_star_report_dates_available(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String)

        Dim htmlOut As New StringBuilder
        Dim results_table As New DataTable
        Dim toggleRowColor As Boolean = False

        Dim date_display_year As String = ""
        Dim date_display_month As String = ""
        Dim date_display_day As String = ""
        Dim old_date_display_year As String = ""

        Try

            results_table = get_star_report_dates_info(searchCriteria)

            If Not IsNothing(results_table) Then
                If results_table.Rows.Count > 0 Then
                    For Each r As DataRow In results_table.Rows

                        If Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaStarReportYear.Trim) Or Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaStarReportDate.Trim) Then

                            If Not IsDBNull(r.Item("stardate_date_directory")) Then
                                If Not String.IsNullOrEmpty(r.Item("stardate_date_directory").ToString.Trim) Then

                                    date_display_year = r.Item("stardate_date_directory").ToString.Substring(0, 4).Trim

                                    date_display_month = MonthName(CInt(r.Item("stardate_date_directory").ToString.Substring(4, 2).Trim))

                                    date_display_day = CInt(r.Item("stardate_date_directory").ToString.Substring(6, 2).Trim).ToString
                                    If CInt(date_display_day) = 1 Then
                                        date_display_day += "st,"
                                    End If

                                End If
                            End If

                            If searchCriteria.ViewCriteriaStarReportYear.Trim = date_display_year.Trim Or Not String.IsNullOrEmpty(searchCriteria.ViewCriteriaStarReportDate.Trim) Then

                                If Not toggleRowColor Then
                                    htmlOut.Append("<tr class='alt_row'>")
                                    toggleRowColor = True
                                Else
                                    htmlOut.Append("<tr bgcolor='white'>")
                                    toggleRowColor = False
                                End If

                                htmlOut.Append("<td align='left' valign='middle' class='papers'>")
                                If String.IsNullOrEmpty(searchCriteria.ViewCriteriaStarReportDate.Trim) Then
                                    htmlOut.Append("<a href=""view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&reportYear=&reportType=" + searchCriteria.ViewCriteriaStarReportType + "&reportID=" + searchCriteria.ViewCriteriaStarReportID.ToString + "&reportCatagory=" + searchCriteria.ViewCriteriaStarReportCatagory + "&reportDate=" + r.Item("stardate_date_directory").ToString + "&reportSuffix=" + searchCriteria.ViewCriteriaStarReportSuffix + "&reportPrefix=" + searchCriteria.ViewCriteriaStarReportPrefix + """>" + date_display_month + " " + date_display_day + " " + date_display_year + "</a></td>")
                                Else
                                    htmlOut.Append("<img src='images/papers.jpg' class='bullet' alt='' /><strong><em>" + date_display_month + " " + date_display_day + " " + date_display_year + "</em></strong></td>")
                                End If
                                htmlOut.Append("</tr>")
                            End If

                        Else

                            If Not IsDBNull(r.Item("stardate_date_directory")) Then
                                If Not String.IsNullOrEmpty(r.Item("stardate_date_directory").ToString.Trim) Then
                                    date_display_year = r.Item("stardate_date_directory").ToString.Substring(0, 4).Trim
                                End If
                            End If

                            If date_display_year <> old_date_display_year Then

                                If Not toggleRowColor Then
                                    htmlOut.Append("<tr class='alt_row'>")
                                    toggleRowColor = True
                                Else
                                    htmlOut.Append("<tr bgcolor='white'>")
                                    toggleRowColor = False
                                End If

                                htmlOut.Append("<td align='left' valign='middle' class='papers'>")
                                htmlOut.Append("<a href=""view_template.aspx?ViewID=" + searchCriteria.ViewID.ToString + "&ViewName=" + searchCriteria.ViewName + "&reportYear=" + date_display_year + "&reportType=" + searchCriteria.ViewCriteriaStarReportType + "&reportID=" + searchCriteria.ViewCriteriaStarReportID.ToString + "&reportCatagory=" + searchCriteria.ViewCriteriaStarReportCatagory + "&reportDate=&reportSuffix=" + searchCriteria.ViewCriteriaStarReportSuffix + "&reportPrefix=" + searchCriteria.ViewCriteriaStarReportPrefix + """>" + date_display_year + "</a></td>")
                                htmlOut.Append("</tr>")

                            End If

                            old_date_display_year = date_display_year

                        End If

                        date_display_year = ""
                        date_display_month = ""
                        date_display_day = ""

                    Next

                Else
                    htmlOut.Append("<tr><td align='left' valign='middle'>No Report Dates</td></tr>")
                End If

            Else
                htmlOut.Append("<tr><td align='left' valign='middle'>No Report Dates</td></tr>")
            End If


        Catch ex As Exception

            aError = "Error in views_display_star_report_dates_available(ByRef searchCriteria As viewSelectionCriteriaClass, ByRef out_htmlString As String) " + ex.Message

        Finally

        End Try

        'return resulting html string
        out_htmlString = htmlOut.ToString
        htmlOut = Nothing

    End Sub

#End Region

End Class
