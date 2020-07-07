
' ********************************************************************************
' Copyright 2004-11. JETNET,LLC. All rights reserved.
'
'$$Archive: /commonWebProject/Entity Classes/displayCompanyDetailsFunctions.vb $
'$$Author: Matt $
'$$Date: 6/08/20 3:36p $
'$$Modtime: 6/08/20 1:44p $
'$$Revision: 41 $
'$$Workfile: displayCompanyDetailsFunctions.vb $
'
' ********************************************************************************
Public Class displayCompanyDetailsFunctions

    Public Property class_error() As String


    Public Function Return_Service_Summary(ByVal compID As Long, ByVal JournalID As Long, ByVal is_rollup As String, ByVal Optional getAllSubs As Boolean = False) As DataTable
        Dim sql As String = ""
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim atemptable As New DataTable
        Try

            sql = "select distinct serv_name, sub_service_name, serv_code,"
            sql += " MIN(sub_start_date) AS sub_start_date,"
            sql += " MAX(sub_end_date) AS sub_end_date,"
            sql += " SUM(sub_contract_amount) as CONAMT,"
            sql += " SUM(sub_nbr_of_installs) as LICENSES,"
            sql += " COUNT(distinct sub_id) as SUBSCRIPTIONS,"
            sql += " COUNT(distinct sub_comp_id) as LOCATIONS,"

            sql += " CASE WHEN sub_end_date is null or sub_end_date > GETDATE() then 'ACTIVE' else 'INACTIVE' end AS STATUS"

            If is_rollup = "Y" Then
                sql += " ,"
                sql += " (select SUM(sublogin_contract_amount)"
                sql += " from Subscription_Login with (NOLOCK)"
                sql += " inner join Subscription with (NOLOCK) on sub_id = sublogin_sub_id and sub_serv_code = service.serv_code "
                sql += " inner join Company with (NOLOCK) on comp_id = sub_comp_id and comp_journ_id = 0 "
                sql += " where sublogin_contract_amount > 0 "
                sql += " and comp_id in (select distinct RelCompID from ReturnAllCompanyLocationsByCompId(" + compID.ToString + "))"
                sql += " ) AS tsum"
            Else
                sql += " ,"
                sql += " (select SUM(sublogin_contract_amount)"
                sql += " from Subscription_Login with (NOLOCK)"
                sql += " inner join Subscription with (NOLOCK) on sub_id = sublogin_sub_id and sub_serv_code = service.serv_code "
                sql += " inner join Company with (NOLOCK) on comp_id = sub_comp_id and comp_journ_id = 0 "
                sql += " where sublogin_contract_amount > 0 "
                sql += " and comp_id = " + compID.ToString
                sql += " ) AS tsum"
            End If

            sql += " FROM Subscription with (NOLOCK)"
            sql += " inner join Service with (NOLOCK) on serv_code=sub_serv_code"

            If is_rollup = "Y" Then
                sql += " WHERE sub_comp_id in (select distinct RelCompID from ReturnAllCompanyLocationsByCompId(" + compID.ToString + "))"
            Else
                sql += " WHERE sub_comp_id = " + compID.ToString
            End If


            sql += " AND (sub_frequency <> '') AND (sub_frequency IS NOT NULL) "

            If Not getAllSubs Then
                sql += " AND (sub_start_date <= GETDATE()) AND (sub_end_date > GETDATE() OR sub_end_date IS NULL) "
            End If

            sql += " group by serv_name, sub_service_name, serv_code, CASE WHEN sub_end_date is null or sub_end_date > GETDATE() then 'ACTIVE' else 'INACTIVE' end"
            sql += " order by serv_name, sub_service_name, STATUS"


            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, [GetType]().FullName, sql.ToString)

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
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
        Catch ex As Exception
            class_error = "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message
            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, [GetType]().FullName, ex.Message)
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

    Public Function get_My_Demos_Trials(ByVal type_of As String, ByVal user_or_all As String, ByVal sum_by As String) As DataTable

        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Dim _dataTable As New DataTable
        Dim _recordSet As System.Data.SqlClient.SqlDataReader : _recordSet = Nothing


        Try

            sQuery.Append("  Select Case when comp_name Is NULL then 'PLATFORM: ' + subins_platform_name else '<a href=''DisplayCompanyDetail.aspx?compid=' + cast(comp_id as varchar(10)) + ''' target=''_blank''>' + comp_name + '</a> (' + contact_first_name + ' ' + contact_last_name + ')' END as ASSIGNEDTO, ")
            sQuery.Append(" sub_service_name as SERVICE, sublogin_password As PASSWORD,  ")
            sQuery.Append("  subins_install_date as INSTALLED, subins_last_login_date As LASTLOGIN, EXPIREON, ")
            sQuery.Append(" STATUS, comptrial_user_id USERID, comp_id, contact_id, sub_id, sublogin_login, subins_seq_no ")




            If HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.HOMEBASE = True Then
                sQuery.Append(" From View_Company_Trials  with (NOLOCK) ")
            ElseIf HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER = True Then
                sQuery.Append(" From [Homebase].jetnet_ra.dbo.View_Company_Trials  with (NOLOCK) ")
            Else
                sQuery.Append(" From View_Company_Trials  with (NOLOCK) ")
            End If

            sQuery.Append(" where STATUS in ('Active','Expired') ")

            If Trim(user_or_all) = "All" Then
            Else
                If Not IsNothing(HttpContext.Current.Session.Item("homebaseUserClass").home_user_id) Then
                    If Trim(HttpContext.Current.Session.Item("homebaseUserClass").home_user_id) <> "" Then
                        sQuery.Append("  AND comptrial_user_id = '" & HttpContext.Current.Session.Item("homebaseUserClass").home_user_id & "' ")
                    End If
                End If
            End If

            ' sQuery.Append("  AND comptrial_user_id = 'dj' ")
            sQuery.Append(" order by STATUS, comp_name, subins_platform_name ")


            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, Me.GetType().FullName, sQuery.ToString)

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim


            SqlConn.Open()
            SqlCommand.Connection = SqlConn
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            SqlCommand.CommandText = sQuery.ToString
            _recordSet = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)

            Try
                _dataTable.Load(_recordSet)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = _dataTable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in " & Me.GetType().FullName & " load datatable " + constrExc.Message
            End Try

            _recordSet.Close()
            _recordSet = Nothing

        Catch ex As Exception

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error in " & Me.GetType().FullName & " As Long " + ex.Message

        Finally

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

            SqlCommand.Dispose()
            SqlCommand = Nothing
        End Try

        Return _dataTable

    End Function
    Public Function Return_Trial_Summary(ByVal compID As Long, ByVal JournalID As Long, ByVal is_rollup As String, ByVal Optional getAllSubs As Boolean = False, Optional ByVal ContactID As Long = 0) As DataTable

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Try


            sQuery.Append(" Select sub_service_name As SERVICE, (contact_first_name + ' ' + contact_last_name) as NAME, sublogin_password as PASSWORD,  ")
            sQuery.Append(" STATUS, comptrial_user_id USERID, subins_install_date As INSTALL, sub_id, contact_email_address, sublogin_login ")


            If (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Then
                sQuery.Append(" FROM [Homebase].jetnet_ra.dbo.View_Company_Trials with (NOLOCK) ")
            Else
                sQuery.Append(" From View_Company_Trials with (NOLOCK) ")
            End If

            If getAllSubs = True Then
                sQuery.Append(" Where comp_id = " & compID & " ")
            ElseIf getAllSubs = False Then
                sQuery.Append(" Where STATUS ='Active' ")
                sQuery.Append(" And comp_id = " & compID & " ")
            End If


            sQuery.Append(" And comp_id = " & compID & " ")

            If ContactID > 0 Then
                sQuery.Append(" And contact_id = " & ContactID & " ")
            End If

            sQuery.Append(" Order By comp_name ")

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, [GetType]().FullName, sQuery.ToString)

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            SqlConn.Open()
            SqlCommand.Connection = SqlConn


            SqlCommand.CommandText = sQuery.ToString
            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
            SqlCommand.CommandType = CommandType.Text
            SqlCommand.CommandTimeout = 60

            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
            End Try
        Catch ex As Exception
            class_error = "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message
            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, [GetType]().FullName, ex.Message)
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

    Public Function Return_Subscription_Summary(ByVal compID As Long, ByVal JournalID As Long, ByVal is_rollup As String, ByVal Optional getAllSubs As Boolean = False) As DataTable
        Dim sql As String = ""
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim atemptable As New DataTable
        Try

            sql += "select distinct serv_name, serv_description, sub_id, sub_start_date, sub_end_date, sub_contract_amount as CONAMT, sub_nbr_of_installs as LICENSES,"
            sql += " (select COUNT(*) from view_jetnet_customers v with (NOLOCK) where v.sub_id = subscription.sub_id) as USERS, "
            sql += " sub_nbr_of_spi_installs as VLICENSES, "
            sql += "(select COUNT(*) from View_JETNET_Customers v With (NOLOCK) where subscription.sub_id = v.sub_id And v.sublogin_values_flag='Y') as VUSERS, "

            sql += " sub_comp_id, sub_parent_sub_id, (select * from ReturnServiceFullName(sub_id)) as SUBNAME"
            sql += " ,( select SUM(sublogin_contract_amount) from Subscription_Login with (NOLOCK) where sublogin_sub_id  = subscription.sub_id and sublogin_contract_amount > 0) as tsum"
            sql += " ,( select COUNT(*) from Subscription_Login with (NOLOCK) where sublogin_sub_id  = subscription.sub_id and sublogin_contract_amount > 0) as tcount"
            sql += " from Subscription with (NOLOCK)"
            sql += " inner join View_Service with (NOLOCK) on serv_code = sub_serv_code"
            sql += " inner join Company with (NOLOCK) on comp_id = sub_comp_id and comp_journ_id = 0"

            If is_rollup = "Y" Then
                sql += " where sub_comp_id in (select distinct RelCompID from ReturnAllCompanyLocationsByCompId(" + compID.ToString + "))"
            Else
                sql += " where sub_comp_id = " + compID.ToString
            End If

            sql += " AND (sub_frequency <> '') AND (sub_frequency IS NOT NULL) "

            If Not getAllSubs Then
                sql += " AND (sub_start_date <= GETDATE()) AND (sub_end_date > GETDATE() OR sub_end_date IS NULL)"
            End If

            sql += " order by serv_name, serv_description, sub_id"



            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, [GetType]().FullName, sql.ToString)

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
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
        Catch ex As Exception
            class_error = "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message
            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, [GetType]().FullName, ex.Message)
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


    Public Function Return_ActiveUser_Summary(ByVal compID As Long, ByVal JournalID As Long, ByVal is_rollup As String, ByVal Optional getAllSubs As Boolean = False, Optional ByVal subID As Long = 0) As DataTable
        Dim sql As String = ""
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim atemptable As New DataTable
        Try

            sql += "SELECT DISTINCT contact_first_name as FIRSTNAME, contact_last_name as LASTNAME, contact_email_address as EMAIL,"
            sql += " sublogin_password as PWD, subins_last_login_date as LASTLOGIN,"
            sql += " case when subins_admin_flag = 'Y' then 'YES' else 'NO' end as ADMIN, contact_id as CONTACTID, sub_id as SUBID, sub_start_date, sub_end_date"

            If JournalID > 0 Then
                sql += " FROM View_JETNET_Customers_History with (NOLOCK)"
            Else
                sql += " FROM View_JETNET_Customers with (NOLOCK)"
            End If

            If is_rollup = "Y" Then
                sql += " where sub_comp_id in (select distinct RelCompID from ReturnAllCompanyLocationsByCompId(" + compID.ToString + "))"
            Else
                sql += " where sub_comp_id = " + compID.ToString
            End If

            sql += " AND (sub_frequency <> '') AND (sub_frequency IS NOT NULL) "

            If Not getAllSubs Then
                sql += " AND (sub_start_date <= GETDATE()) AND (sub_end_date > GETDATE() OR sub_end_date IS NULL)"
            End If

            If subID > 0 Then
                sql += " AND sub_id = " + subID.ToString
            End If

            sql += " order by contact_last_name"

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, [GetType]().FullName, sql.ToString)

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
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
        Catch ex As Exception
            class_error = "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message
            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, [GetType]().FullName, ex.Message)
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



    Public Function Return_Services_Used_Summary(ByVal compID As Long, ByVal JournalID As Long, ByVal is_rollup As String) As DataTable
        Dim sql As String = ""
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim atemptable As New DataTable
        Try

            ' select distinct svud_desc FROM Company with (NOLOCK) inner join Company_Services_Used with (NOLOCK) on comp_id = csu_comp_id and comp_journ_id = csu_journ_id 
            'inner Join Services_Used with (NOLOCK) on csu_svud_id = svud_id where comp_journ_id = 0 AND comp_id = 146813 order by svud_desc ASC


            sql += "select distinct svud_desc, csu_end_date, csu_notes"

            If (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Then
                sql += " FROM [Homebase].jetnet_ra.dbo.Company with (NOLOCK)"
                sql += " inner join [Homebase].jetnet_ra.dbo.Company_Services_Used with (NOLOCK) on comp_id = csu_comp_id and comp_journ_id = csu_journ_id"
                sql += " inner join [Homebase].jetnet_ra.dbo.Services_Used with (NOLOCK) on csu_svud_id = svud_id "
            Else
                sql += " FROM Company with (NOLOCK)"
                sql += " inner join Company_Services_Used with (NOLOCK) on comp_id = csu_comp_id and comp_journ_id = csu_journ_id"
                sql += " inner join Services_Used with (NOLOCK) on csu_svud_id = svud_id "
            End If


            If is_rollup = "Y" Then
                sql += " where comp_journ_id = 0 AND comp_id in (select distinct RelCompID from ReturnAllCompanyLocationsByCompId(" + compID.ToString + "))"
            Else
                sql += " where comp_journ_id = 0 AND comp_id = " + compID.ToString
            End If

            sql += " order by svud_desc ASC"

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, [GetType]().FullName, sql.ToString)

            'If (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER And HttpContext.Current.Application.Item("crmClientSiteData").WebSiteType <> eWebSiteTypes.LOCAL) Then
            '  SqlConn.ConnectionString = "Data Source=10.10.254.54;Initial Catalog=jetnet_ra;Persist Security Info=True;User ID=SVC-EVO-RO;Password=R-PL35$31;MultipleActiveResultSets=True;Asynchronous Processing=True"
            'Else
            'SqlConn.ConnectionString = "Data Source=10.10.254.54;Initial Catalog=jetnet_ra;Persist Security Info=True;User ID=SVC-EVO-RO;Password=R-PL35$31;MultipleActiveResultSets=True;Asynchronous Processing=True"

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            'End If

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
        Catch ex As Exception
            class_error = "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message
            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, [GetType]().FullName, ex.Message)
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

    Public Function Return_Customer_Activities_Summary(ByVal compID As Long, ByVal JournalID As Long, ByVal is_rollup As String, Optional ByVal subID As Long = 0) As DataTable
        Dim sql As String = ""
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim atemptable As New DataTable
        Try

            sql += "SELECT TOP(25) cstact_added_date, cstact_added_time, cstact_init, cstact_note, sub_contact_id, contact_first_name, contact_last_name"
            sql += " FROM [customer].[dbo].customer_activity C with (NOLOCK)"
            sql += " INNER JOIN jetnet_ra.dbo.Subscription S with (NOLOCK) ON cstact_techid_value = sub_id"
            sql += " LEFT OUTER JOIN jetnet_ra.dbo.Contact T with (NOLOCK) ON sub_contact_id = contact_id AND contact_journ_id = 0"

            If is_rollup = "Y" Then
                sql += " WHERE S.sub_comp_id in (select distinct RelCompID from ReturnAllCompanyLocationsByCompId(" + compID.ToString + "))"
            Else
                sql += " WHERE S.sub_comp_id = " + compID.ToString
            End If

            If subID > 0 Then
                sql += " AND S.sub_id = " + subID.ToString
            End If

            sql += " ORDER BY cstact_added_date DESC, cstact_added_time DESC"

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, [GetType]().FullName, sql.ToString)

            'If (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER And HttpContext.Current.Application.Item("crmClientSiteData").WebSiteType <> eWebSiteTypes.LOCAL) Then
            '  SqlConn.ConnectionString = "Data Source=10.10.254.54;Initial Catalog=jetnet_ra;Persist Security Info=True;User ID=SVC-EVO-RO;Password=R-PL35$31;MultipleActiveResultSets=True;Asynchronous Processing=True"
            'Else
            'SqlConn.ConnectionString = "Data Source=10.10.254.54;Initial Catalog=jetnet_ra;Persist Security Info=True;User ID=SVC-EVO-RO;Password=R-PL35$31;MultipleActiveResultSets=True;Asynchronous Processing=True"

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            'End If

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
        Catch ex As Exception
            class_error = "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message
            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, [GetType]().FullName, ex.Message)
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

    Public Function Return_Contract_Execution_Summary(ByVal compID As Long, ByVal JournalID As Long, ByVal is_rollup As String, Optional ByVal subID As Long = 0) As DataTable
        Dim sql As String = ""
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim atemptable As New DataTable
        Try

            sql += "SELECT cstexcform_exc_date, cstexcform_monthly_fee, cstexcform_notes, cstexcform_type"
            sql += " FROM [customer].[dbo].Customer_Execution E with (NOLOCK)"
            sql += " INNER JOIN jetnet_ra.dbo.Subscription S with (NOLOCK) ON cstexcform_techid_value = sub_id"

            If is_rollup = "Y" Then
                sql += " WHERE S.sub_comp_id in (select distinct RelCompID from ReturnAllCompanyLocationsByCompId(" + compID.ToString + "))"
            Else
                sql += " WHERE S.sub_comp_id = " + compID.ToString
            End If

            If subID > 0 Then
                sql += " AND S.sub_id = " + subID.ToString
            End If

            sql += " ORDER BY cstexcform_exc_date DESC"

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, [GetType]().FullName, sql.ToString)

            'If (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER And HttpContext.Current.Application.Item("crmClientSiteData").WebSiteType <> eWebSiteTypes.LOCAL) Then
            '  SqlConn.ConnectionString = "Data Source=10.10.254.54;Initial Catalog=jetnet_ra;Persist Security Info=True;User ID=SVC-EVO-RO;Password=R-PL35$31;MultipleActiveResultSets=True;Asynchronous Processing=True"
            'Else
            'SqlConn.ConnectionString = "Data Source=10.10.254.54;Initial Catalog=jetnet_ra;Persist Security Info=True;User ID=SVC-EVO-RO;Password=R-PL35$31;MultipleActiveResultSets=True;Asynchronous Processing=True"

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            'End If

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
        Catch ex As Exception
            class_error = "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message
            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, [GetType]().FullName, ex.Message)
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

    Public Function Return_Contract_List_Summary(ByVal compID As Long, ByVal JournalID As Long, ByVal is_rollup As String, Optional ByVal subID As Long = 0) As DataTable
        Dim sql As String = ""
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim atemptable As New DataTable
        Try

            sql += "SELECT compdoc_id as DOCID, compdoc_doc_date as DOCDATE, compdoc_entry_date as ENTRYDATE, doctype_description AS DOCTYPE, compdoc_description AS SUBJECT"
            sql += " FROM Company_Documents WITH (NOLOCK)"
            sql += " INNER JOIN Document_Type WITH (NOLOCK) ON compdoc_doc_type_code = doctype_code"

            If subID > 0 Then
                sql += " INNER JOIN Subscription WITH (NOLOCK) ON compdoc_comp_id = sub_comp_id"
            End If

            If is_rollup = "Y" Then
                sql += " WHERE (compdoc_journ_id = 0) AND (doctype_contract_doc_view = 'Y') AND compdoc_comp_id IN (SELECT DISTINCT RelCompID FROM ReturnAllCompanyLocationsByCompId(" + compID.ToString + "))"
            Else
                sql += " WHERE (compdoc_journ_id = 0) AND (doctype_contract_doc_view = 'Y') AND compdoc_comp_id = " + compID.ToString
            End If

            If subID > 0 Then
                sql += " AND sub_id = " + subID.ToString
            End If

            sql += " ORDER BY compdoc_id DESC"

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, [GetType]().FullName, sql.ToString)

            'If (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER And HttpContext.Current.Application.Item("crmClientSiteData").WebSiteType <> eWebSiteTypes.LOCAL) Then
            '  SqlConn.ConnectionString = "Data Source=10.10.254.54;Initial Catalog=jetnet_ra;Persist Security Info=True;User ID=SVC-EVO-RO;Password=R-PL35$31;MultipleActiveResultSets=True;Asynchronous Processing=True"
            'Else
            'SqlConn.ConnectionString = "Data Source=10.10.254.54;Initial Catalog=jetnet_ra;Persist Security Info=True;User ID=SVC-EVO-RO;Password=R-PL35$31;MultipleActiveResultSets=True;Asynchronous Processing=True"

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            'End If

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
        Catch ex As Exception
            class_error = "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message
            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, [GetType]().FullName, ex.Message)
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

    Public Function Return_Distinct_NoteType(ByVal compID As Long, ByVal JournalID As Long, ByVal is_rollup As String) As DataTable
        Dim sql As String = ""
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim atemptable As New DataTable
        Try

            sql += " SELECT distinct notetype "

            If (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Then
                sql += " FROM [Homebase].jetnet_ra.dbo.View_Company_Notes with (NOLOCK)"
            Else
                sql += " FROM View_Company_Notes with (NOLOCK)"
            End If

            If is_rollup = "Y" Then
                sql += " WHERE journ_comp_id in (select distinct RelCompID from ReturnAllCompanyLocationsByCompId(" + compID.ToString + "))"
            Else
                sql += " WHERE journ_comp_id = " + compID.ToString
            End If

            sql += " order by notetype "

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, [GetType]().FullName, sql.ToString)

            'If (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER And HttpContext.Current.Application.Item("crmClientSiteData").WebSiteType <> eWebSiteTypes.LOCAL) Then
            '  SqlConn.ConnectionString = "Data Source=10.10.254.54;Initial Catalog=jetnet_ra;Persist Security Info=True;User ID=SVC-EVO-RO;Password=R-PL35$31;MultipleActiveResultSets=True;Asynchronous Processing=True"
            'Else
            'SqlConn.ConnectionString = "Data Source=10.10.254.54;Initial Catalog=jetnet_ra;Persist Security Info=True;User ID=SVC-EVO-RO;Password=R-PL35$31;MultipleActiveResultSets=True;Asynchronous Processing=True"

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            'End If


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
        Catch ex As Exception
            class_error = "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message
            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, [GetType]().FullName, ex.Message)
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

    Public Function DisplayCustomerActivitiesTable(user_table As DataTable, companyID As Long, contactID As Long) As String
        Dim htmlOut As New StringBuilder
        Dim temp_string As String = ""

        If Not IsNothing(user_table) Then


            htmlOut.Append("<table id='customerActivitiesTable' width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" class=""formatTable blue small aircraftTable"">")
            htmlOut.Append("<tr class='header_row'>")
            htmlOut.Append("<td align='left'><b>DATE</b></td>")
            htmlOut.Append("<td align='left'><b>DETAILS</b></td>")
            htmlOut.Append("<td align='left'><b>STAFF</b></td>")

            htmlOut.Append("</tr>")

            If (user_table.Rows.Count > 0) Then

                For Each q As DataRow In user_table.Rows

                    htmlOut.Append("<tr bgcolor=""white"">")

                    htmlOut.Append("<td align=""left"" valign=""top"">")

                    If Not IsDBNull(q("DATE")) Then
                        If Not String.IsNullOrEmpty(q("DATE").ToString.Trim) Then
                            htmlOut.Append(FormatDateTime(q("DATE").ToString, DateFormat.ShortDate))
                        End If
                    End If

                    htmlOut.Append("</td><td align=""left"" valign=""top"">")

                    If Not IsDBNull(q("DETAILS")) Then
                        If Not String.IsNullOrEmpty(q("DETAILS").ToString.Trim) Then

                            Dim tmpDesc As String = ""
                            Dim tmpID As String = ""

                            If Not IsNothing(user_table.Columns.Item("source")) Then

                                If Not IsDBNull(q("Source")) Then

                                    If Not IsDBNull(q("ID")) Then

                                        If Not String.IsNullOrEmpty(q("ID").ToString.Trim) Then
                                            tmpID = q("ID").ToString.Trim
                                        End If

                                    End If


                                    'Select Case (q("Source").ToString.ToLower.Trim)
                                    '    Case "company documents"
                                    '        tmpDesc = "<a class=""underline emphasisColor"" onclick='javascript:load(""homeTables.aspx?type_of=Company&sub_type_of=" + q("Source").ToString.Trim + "&comp_id=" + companyID.ToString + "&activityid=" + tmpID + "&homebase=Y&action=update"","""",""scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no"");return false;' title='Show " + q("Source").ToString.Trim + "'>" & q("notesummary").ToString & ":</a> " & q("DETAILS").ToString.Trim
                                    '    Case "customer activity"
                                    '        tmpDesc = "<a class=""underline emphasisColor"" onclick='javascript:load(""homeTables.aspx?type_of=Company&sub_type_of=" + q("Source").ToString.Trim + "&comp_id=" + companyID.ToString + "&activityid=" + tmpID + "&homebase=Y&action=update"","""",""scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no"");return false;' title='Show " + q("Source").ToString.Trim + "'>" & q("notesummary").ToString & ":</a> " & q("DETAILS").ToString.Trim
                                    '    Case "journal"
                                    '        If q("notegroup").ToString.ToLower.Trim <> "marketing" Then
                                    '            tmpDesc = "<a class=""underline emphasisColor"" onclick='javascript:load(""homeTables.aspx?type_of=Company&sub_type_of=" + q("Source").ToString.Trim + "&comp_id=" + companyID.ToString + "&activityid=" + tmpID + "&homebase=Y&action=update"","""",""scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no"");return false;' title='Show " + q("Source").ToString.Trim + "'>" & q("notesummary").ToString & ":</a> " & q("DETAILS").ToString.Trim
                                    '        Else
                                    '            tmpDesc = "<a class=""underline emphasisColor"" onclick='javascript:load(""homeTables.aspx?type_of=Company&sub_type_of=marketing&table=" + q("Source").ToString.Trim + "&comp_id=" + companyID.ToString + "&activityid=" + tmpID + "&homebase=Y&action=update"","""",""scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no"");return false;' title='Show " + q("Source").ToString.Trim + "'>" & q("notesummary").ToString & ":</a> " & q("DETAILS").ToString.Trim
                                    '        End If
                                    '    Case "customer execution"
                                    '        tmpDesc = "<a class=""underline emphasisColor"" onclick='javascript:load(""homeTables.aspx?type_of=Company&sub_type_of=" + q("Source").ToString.Trim + "&comp_id=" + companyID.ToString + "&activityid=" + tmpID + "&homebase=Y&action=update"","""",""scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no"");return false;' title='Show " + q("Source").ToString.Trim + "'>" & q("notegroup").ToString & ":</a> " & q("DETAILS").ToString
                                    '    Case "subscription"
                                    '        tmpDesc = "<a class=""underline emphasisColor"" onclick='javascript:load(""homeTables.aspx?type_of=Company&sub_type_of=" + q("Source").ToString.Trim + "&comp_id=" + companyID.ToString + "&activityid=" + tmpID + "&homebase=Y&action=update"","""",""scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no"");return false;' title='Show " + q("Source").ToString.Trim + "'>" & q("notesummary").ToString & ":</a> " & q("DETAILS").ToString.Trim
                                    'End Select

                                    If q("Source").ToString.ToLower.Trim = "customer execution" Then
                                        tmpDesc = q("DETAILS").ToString.Trim.Replace("EXECUTION:", "<a class=""underline emphasisColor"" onclick='javascript:load(""homeTables.aspx?type_of=Company&sub_type_of=" + q("Source").ToString.Trim + "&comp_id=" + companyID.ToString + "&activityid=" + tmpID + "&homebase=Y&action=update"","""",""scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no"");return false;' title='Show " + q("Source").ToString.Trim + "'>EXECUTION:</a> ")
                                    ElseIf q("notegroup").ToString.ToLower.Trim <> "marketing" Then
                                        tmpDesc = "<a class=""underline emphasisColor"" onclick='javascript:load(""homeTables.aspx?type_of=Company&sub_type_of=" + q("Source").ToString.Trim + "&comp_id=" + companyID.ToString + "&activityid=" + tmpID + "&homebase=Y&action=update"","""",""scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no"");return false;' title='Show " + q("Source").ToString.Trim + "'>" & q("notesummary").ToString & ":</a> " & q("DETAILS").ToString.Trim
                                    Else
                                        tmpDesc = "<a class=""underline emphasisColor"" onclick='javascript:load(""homeTables.aspx?type_of=Company&sub_type_of=marketing&table=" + q("Source").ToString.Trim + "&comp_id=" + companyID.ToString + "&activityid=" + tmpID + "&homebase=Y&action=update"","""",""scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no"");return false;' title='Show " + q("Source").ToString.Trim + "'>" & q("notesummary").ToString & ":</a> " & q("DETAILS").ToString.Trim
                                    End If


                                Else

                                    tmpDesc = q("DETAILS").ToString.Trim

                                End If

                            Else

                                tmpDesc = q("DETAILS").ToString.Trim

                            End If


                            htmlOut.Append(IIf(tmpDesc.Trim.Length < 125, tmpDesc.Trim, tmpDesc.Trim + "..."))

                        End If
                    End If

                    htmlOut.Append("</td><td align=""left"" valign=""top"">")

                    If Not IsDBNull(q("STAFF")) Then
                        If Not String.IsNullOrEmpty(q("STAFF").ToString.Trim) Then
                            htmlOut.Append(q("STAFF").ToString.Trim)
                        End If
                    End If

                    htmlOut.Append("</td>")

                    htmlOut.Append("</tr>")

                Next

            End If

            htmlOut.Append("</table>")


        Else

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "displayContactDetailsFunctions.vb", "DisplayCustomerActivitiesTable() Datatable is nothing on admin Activities items")

        End If
        Return htmlOut.ToString
    End Function
    Public Function Get_ActionItems_Query(compID As Long, contactID As Long) As DataTable
        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try
            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            SqlConn.Open()

            sQuery.Append(" select journ_date, journ_id, journ_description, user_first_name, user_last_name, journ_subcategory_code  from   ")
            'Prefixes
            If (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Then
                sQuery.Append(" [Homebase].[jetnet_ra].[dbo].Journal with (NOLOCK) inner Join [Homebase].[jetnet_ra].[dbo].[User] with (NOLOCK) on user_id = journ_user_id")
            Else
                'non prefixes
                sQuery.Append(" Journal with (NOLOCK) inner Join [User] with (NOLOCK) on user_id = journ_user_id ")
            End If

            '            Select Case journ_date, journ_id, journ_description, user_first_name, user_last_name from Journal With (NOLOCK) 
            'inner Join [User] with (NOLOCK) on user_id = journ_user_id
            'where journ_subcategory_code ='AIAI'
            'And journ_comp_id = 244271
            'order by journ_date asc


            sQuery.Append(" where journ_subcategory_code in ('AIAI','RALT') and journ_comp_id = @compID ")
            ' sQuery.Append(" where journ_subcategory_code='AIAI' and journ_comp_id = @compID ")

            If contactID > 0 Then
                sQuery.Append(" and journ_contact_id = @contactID ")
            End If
            sQuery.Append(" order by journ_date asc ")




            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "displayCompanyDetailsFunctions.vb", sQuery.ToString)

            Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)


            SqlCommand.Parameters.AddWithValue("@compID", compID)

            If contactID > 0 Then
                SqlCommand.Parameters.AddWithValue("@contactID", contactID)
            End If

            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error In Get_Simple_Display_Query(journID As Long, compID As Long) As DataTable" + constrExc.Message
            End Try

            SqlCommand.Dispose()
            SqlCommand = Nothing

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error In Get_Simple_Display_Query(journID As Long, compID As Long) As DataTable" + ex.Message

        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

        End Try

        Return atemptable

    End Function
    Public Function ReturnActionItemsDisplayTable(actionDataTable As DataTable, companyID As Long, contactID As Long) As String
        Dim htmlOut As New StringBuilder
        If (actionDataTable.Rows.Count > 0) Then
            htmlOut.Append("<table id='adminActionTable' width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" class=""formatTable blue small aircraftTable vertical_align_top"">")
            htmlOut.Append("<tr class='header_row'>")
            htmlOut.Append("<td ><b>DATE</b></td>")
            htmlOut.Append("<td><b>DESCRIPTION</b></td>")
            htmlOut.Append("<td ><b>USER</b></td>")
            htmlOut.Append("<td ><b>TYPE</b></td>")

            htmlOut.Append("</tr>")



            For Each q As DataRow In actionDataTable.Rows

                htmlOut.Append("<tr>")
                htmlOut.Append("<td>")
                If Not IsDBNull(q("journ_date")) Then
                    If Not String.IsNullOrEmpty(q("journ_date").ToString.Trim) Then
                        htmlOut.Append("<a href=""#"" onclick='javascript:var place = window.open(""adminActions.aspx?task=edit&journalid=" + q("journ_id").ToString + "&companyid=" & companyID.ToString & "&contactid=" & contactID.ToString & """,""Action Window"",""dependent=yes,scrollbars=yes,menubar=no,height=900,width=1350,resizable=yes,toolbar=no,location=no,status=no"");place.focus();return true;'>")

                        htmlOut.Append(FormatDateTime(q("journ_date").ToString, DateFormat.ShortDate))
                        htmlOut.Append("</a>")
                    End If
                End If
                htmlOut.Append("</td>")

                htmlOut.Append("<td>")
                If Not IsDBNull(q("journ_description")) Then
                    If Not String.IsNullOrEmpty(q("journ_description").ToString.Trim) Then


                        htmlOut.Append(CommonAircraftFunctions.TrimAndTitleString(q("journ_description").ToString, 200))
                    End If
                End If
                htmlOut.Append("</td>")
                htmlOut.Append("<td>")
                If Not IsDBNull(q("user_first_name")) Then
                    If Not String.IsNullOrEmpty(q("user_first_name").ToString.Trim) Then
                        htmlOut.Append(q("user_first_name").ToString)
                    End If
                End If
                If Not IsDBNull(q("user_last_name")) Then
                    If Not String.IsNullOrEmpty(q("user_last_name").ToString.Trim) Then
                        htmlOut.Append(" " & q("user_last_name").ToString)
                    End If
                End If
                htmlOut.Append("</td>")

                htmlOut.Append("<td>")
                If Not IsDBNull(q("journ_subcategory_code")) Then
                    If Not String.IsNullOrEmpty(q("journ_subcategory_code").ToString.Trim) Then

                        If Trim(q("journ_subcategory_code")) = "AIAI" Then
                            htmlOut.Append("Action Item&nbsp;")
                        ElseIf Trim(q("journ_subcategory_code")) = "RALT" Then
                            htmlOut.Append("Research Action&nbsp;")
                        Else
                            htmlOut.Append("&nbsp;")
                        End If

                    End If
                End If
                htmlOut.Append("</td>")





                htmlOut.Append("</tr>")
            Next
            'Else
            '    actionPanel.Visible = False
            '    action_label.Visible = False
            htmlOut.Append("</table>")
        End If


        Return htmlOut.ToString
    End Function

    Public Function Get_AccountingIssues(compID As Long) As DataTable
        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try
            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            SqlConn.Open()

            sQuery.Append(" select journ_date, journ_description, journ_user_id, user_first_name, user_last_name from   ")
            'Prefixes
            If (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Then
                sQuery.Append(" [Homebase].[jetnet_ra].[dbo].Journal with (NOLOCK) inner Join [Homebase].[jetnet_ra].[dbo].[User] with (NOLOCK) on journ_user_id = user_id")
            Else
                'non prefixes
                sQuery.Append(" Journal with (NOLOCK) inner Join [User] with (NOLOCK) on journ_user_id = user_id ")
            End If

            sQuery.Append(" where journ_subcategory_code = 'CSAC' and journ_comp_id = @compID ")
            sQuery.Append(" order by journ_date asc ")





            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "displayCompanyDetailsFunctions.vb", sQuery.ToString)

            Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)


            SqlCommand.Parameters.AddWithValue("@compID", compID)

            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error In Get_Simple_Display_Query(journID As Long, compID As Long) As DataTable" + constrExc.Message
            End Try

            SqlCommand.Dispose()
            SqlCommand = Nothing

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error In Get_Simple_Display_Query(journID As Long, compID As Long) As DataTable" + ex.Message

        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

        End Try

        Return atemptable

    End Function
    Public Function Get_DONOTMARKET(compID As Long) As DataTable
        Dim atemptable As New DataTable
        Dim sQuery = New StringBuilder()

        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing

        Try
            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            SqlConn.Open()


            sQuery.Append(" select * from ")
            'Prefixes
            If (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Then
                sQuery.Append(" [Homebase].[jetnet_ra].[dbo].company_prospect with (NOLOCK)")
            Else
                'non prefixes
                sQuery.Append(" company_prospect with (NOLOCK) ")
            End If

            sQuery.Append(" where cprospect_comp_id= @compID ")
            sQuery.Append(" and cprospect_type = 'Do Not Market' ")





            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, "displayCompanyDetailsFunctions.vb", sQuery.ToString)

            Dim SqlCommand As New SqlClient.SqlCommand(sQuery.ToString, SqlConn)


            SqlCommand.Parameters.AddWithValue("@compID", compID)

            SqlReader = SqlCommand.ExecuteReader(CommandBehavior.CloseConnection)
            Try
                atemptable.Load(SqlReader)
            Catch constrExc As System.Data.ConstraintException
                Dim rowsErr As System.Data.DataRow() = atemptable.GetErrors()
                HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error In Get_Simple_Display_Query(journID As Long, compID As Long) As DataTable" + constrExc.Message
            End Try

            SqlCommand.Dispose()
            SqlCommand = Nothing

        Catch ex As Exception
            Return Nothing

            HttpContext.Current.Session.Item("localUser").crmUser_DebugText += "Error In Get_Simple_Display_Query(journID As Long, compID As Long) As DataTable" + ex.Message

        Finally
            SqlReader = Nothing

            SqlConn.Dispose()
            SqlConn.Close()
            SqlConn = Nothing

        End Try

        Return atemptable

    End Function

    Public Function Get_Available_Services(ByVal user_id As String) As DataTable
        Dim sql As String = ""
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim atemptable As New DataTable
        Try

            sql = ""
            sql += " Select distinct sub_service_name, sub_id from subscription With (NOLOCK) "
            sql += " inner Join dbo.[User] WITH (NOLOCK) ON (CHARINDEX(CAST(dbo.subscription.sub_id AS varchar(5)) + ',', dbo.[User].user_marketing_subids_allowed, 1) > 0 OR "
            sql += " CHARINDEX(CAST(dbo.subscription.sub_id As varchar(5)), dbo.[User].user_marketing_subids_allowed, 1) = 1 Or "
            sql += "  CAST(dbo.subscription.sub_id As varchar(100)) = RIGHT(dbo.[User].user_marketing_subids_allowed, LEN(CAST(dbo.subscription.sub_id As varchar(100)))) And  "
            sql += " Len(CAST(dbo.subscription.sub_id As varchar(100))) = CHARINDEX(',', REVERSE(dbo.[User].user_marketing_subids_allowed)) - 1 AND CHARINDEX(',', REVERSE(dbo.[User].user_marketing_subids_allowed)) > 0)  "
            sql += " And dbo.[User].user_password <> 'inactive' "
            sql += " where sub_marketing_flag = 'Y' "
            ' sql += " And user_id = '" & user_id & "' "

            If Not IsNothing(HttpContext.Current.Session.Item("homebaseUserClass").home_user_id) Then
                If Trim(HttpContext.Current.Session.Item("homebaseUserClass").home_user_id) <> "" Then
                    sql += " AND user_id = '" & HttpContext.Current.Session.Item("homebaseUserClass").home_user_id & "' "
                End If
            End If

            'sql += " And user_id = 'slh' "

            sql += " order by sub_service_name "


            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, [GetType]().FullName, sql.ToString)


            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim


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
        Catch ex As Exception
            class_error = "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message
            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, [GetType]().FullName, ex.Message)
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

    Public Function Return_Customer_Actions_Summary(ByVal compID As Long, ByVal JournalID As Long, ByVal is_rollup As String, Optional ByVal is_all As Boolean = False, Optional noteGroup As String = "", Optional contactID As Long = 0) As DataTable
        Dim sql As String = ""
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim atemptable As New DataTable
        Try

            sql += "Select" + IIf(is_all, "", " top 50")
            sql += " journ_date As Date,"
            sql += " (Case When notegroup='Execution' then (upper(notetype) + ': ' + notesummary + '-' + left(journ_description,125)) else (left(journ_description,125)) end) as DETAILS, "
            sql += " journ_user_id as STAFF, noteGroup, notetype, notesummary, Source, ID "
            If (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Then
                sql += " FROM [Homebase].jetnet_ra.dbo.View_Customer_Notes with (NOLOCK)"
            Else
                sql += " FROM View_Customer_Notes with (NOLOCK)"
            End If

            If is_rollup = "Y" Then
                sql += " WHERE journ_comp_id in (select distinct RelCompID from ReturnAllCompanyLocationsByCompId(" + compID.ToString + "))"
            Else
                sql += " WHERE journ_comp_id = " + compID.ToString
            End If

            If contactID > 0 Then
                sql += " AND contact_id = " & contactID.ToString
            End If


            If noteGroup <> "" Then
                sql += " AND upper(notegroup) = '" & UCase(noteGroup) & "'"
            End If

            ' If subID > 0 Then
            'sql += " AND S.sub_id = " + subID.ToString
            'End If

            sql += " ORDER BY journ_date DESC"

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, [GetType]().FullName, sql.ToString)

            'If (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER And HttpContext.Current.Application.Item("crmClientSiteData").WebSiteType <> eWebSiteTypes.LOCAL) Then
            '  SqlConn.ConnectionString = "Data Source=10.10.254.54;Initial Catalog=jetnet_ra;Persist Security Info=True;User ID=SVC-EVO-RO;Password=R-PL35$31;MultipleActiveResultSets=True;Asynchronous Processing=True"
            'Else
            'SqlConn.ConnectionString = "Data Source=10.10.254.54;Initial Catalog=jetnet_ra;Persist Security Info=True;User ID=SVC-EVO-RO;Password=R-PL35$31;MultipleActiveResultSets=True;Asynchronous Processing=True"

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            'End If

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
        Catch ex As Exception
            class_error = "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message
            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, [GetType]().FullName, ex.Message)
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


    Public Function Return_Research_Notes(ByVal compID As Long, ByVal JournalID As Long, ByVal is_rollup As String, Optional ByVal is_all As Boolean = False, Optional noteType As String = "") As DataTable
        Dim sql As String = ""
        Dim SqlConn As New SqlClient.SqlConnection
        Dim SqlCommand As New SqlClient.SqlCommand
        Dim SqlReader As SqlClient.SqlDataReader
        Dim SqlException As SqlClient.SqlException : SqlException = Nothing
        Dim atemptable As New DataTable
        Try

            sql += "SELECT top 200 journ_date As DATE, (upper(notetype) + ': ' + left(journ_description,125)) as DETAILS, journ_user_id as STAFF, notegroup, Source, ID "

            If (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER) Then
                sql += " FROM [Homebase].jetnet_ra.dbo.View_Company_Notes with (NOLOCK) "
            Else
                sql += " FROM View_Company_Notes with (NOLOCK)"
            End If


            If is_rollup = "Y" Then
                sql += " WHERE journ_comp_id in (select distinct RelCompID from ReturnAllCompanyLocationsByCompId(" + compID.ToString + "))"
            Else
                sql += " WHERE journ_comp_id = " + compID.ToString
            End If

            If noteType <> "" Then
                sql += " AND upper(notetype) = '" & UCase(noteType) & "'"
            End If

            sql += " ORDER BY journ_date DESC "

            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, [GetType]().FullName, sql.ToString)

            'If (HttpContext.Current.Session.Item("jetnetAppVersion") = Constants.ApplicationVariable.CUSTOMER_CENTER And HttpContext.Current.Application.Item("crmClientSiteData").WebSiteType <> eWebSiteTypes.LOCAL) Then
            '  SqlConn.ConnectionString = "Data Source=10.10.254.54;Initial Catalog=jetnet_ra;Persist Security Info=True;User ID=SVC-EVO-RO;Password=R-PL35$31;MultipleActiveResultSets=True;Asynchronous Processing=True"
            'Else
            'SqlConn.ConnectionString = "Data Source=10.10.254.54;Initial Catalog=jetnet_ra;Persist Security Info=True;User ID=SVC-EVO-RO;Password=R-PL35$31;MultipleActiveResultSets=True;Asynchronous Processing=True"

            SqlConn.ConnectionString = HttpContext.Current.Session.Item("jetnetAdminDatabase").ToString.Trim
            'End If

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
        Catch ex As Exception
            class_error = "<br /><br /><b>" + System.Reflection.MethodBase.GetCurrentMethod().Name.ToString + "</b><br />" + ex.Message
            clsGeneral.clsGeneral.Build_Debug_Text(System.Reflection.MethodBase.GetCurrentMethod().Name.ToString, [GetType]().FullName, ex.Message)
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

End Class
